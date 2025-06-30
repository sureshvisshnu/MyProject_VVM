using fa;
using fa.api.catalog;
using fa.libraries.utils;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.controls.ComboTreeView;
using fa.views.controls;
using fa.model.Hms.Master;
using fa.api.utils;
using fa.report.Inventory;
using FADataAccessLibrary.report.Hms;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;
using fa.reports.Inventory;
using Fa.views.utils.Report.Hms;
using fa.report.Ip;
using fa.api.Hms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using fa.reports.Hms;
using fa.reports.sales;
using Syncfusion.Reflection;
using fa.model.Hms.Ip;
using fa.model.Hms.Op;
using static System.Windows.Forms.CheckedListBox;
using fa.views.controls.ComboListView;
using fa.views.utils;
using Fa.views.controls;
using fa.api.Accounting;

namespace Fa.reports.Hms
{
    enum PatientDueListTableColumn
    {
        SNO, PDETAIL, PAGE, PID, FEETYPE, DESC, DUEAMOUNT, OP_AMOUNT
    }
    public partial class FormPatientDueList : Form
    {
        RptPatientDueList RptPatientDueList = null!;
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string SelectWardErrorMsg = "Please select ward.";
        public static string SelectTypeErrorMsg = "Please select patient type.";
        public static string SelectPatientErrorMsg = "Please select the patient.";
        public static string NoInfoFoundErrorMsg = "No information found..";
        ComboTreeNode? selectedNode;
        public FormPatientDueList()
        {
            InitializeComponent();
        }
        string CheckedWardNodes = string.Empty;
        string CheckedPatientNodes = string.Empty;

        private void FormPatientDueList_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ComboUtils.InitializeAllWardCombo(ComboBoxWard, Global.Company.CompanyId);
            LoadPatientTypeCombo();
            LoadControlPatientCombo(CheckedListComboBoxPatient);
            ResetForm();
        }
        private void LoadControlPatientCombo(ToolstripCheckedListComboBox SelectBox)
        {
            IList<Patient> lConsultation = PatientManager.Instance.ListAllPatient(Global.Company.CompanyId).ToArray<Patient>();

            CheckedListComboBox checkedComboBox = SelectBox.CheckedListComboBox!;
            checkedComboBox.ClearNodes();
            foreach (var item in lConsultation)
            {
                checkedComboBox.AddNode(item.Name + " (" + item.PatientNumber + ")", item.Id);
            }
            checkedComboBox.Refresh();
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientDueListErrorMsg.Text = "";
            DataGridViewPatientDueList.Rows.Clear();
            ComboBoxWard.Text = string.Empty;
            ComboBoxWard.SelectedNode = null!;
            CheckedTreeComboBoxType.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxWard.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxType.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CheckedListComboBoxPatient.Reset();
            PatientDueListFromDate.Format = Global.Company.DateFormat;
            PatientDueListFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PatientDueListToDate.Format = Global.Company.DateFormat;
            PatientDueListToDate.Date = Global.getTransactionDate();
            EnableButton(false);
            Cursor.Current = Cursors.Default;
        }
        public class PatientType
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
        private void LoadPatientTypeCombo()
        {
            List<PatientType> patientTypes = new List<PatientType>
            {
                new PatientType { Id = 1, Name = "By InPatient" },
                new PatientType { Id = 2, Name = "By OutPatient" }
            };
            CheckedTreeComboBoxType.Nodes.Clear();
            if (patientTypes.Count > 0)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                CheckedTreeComboBoxType.TreeNodes = parent;
            }
            foreach (var patientType in patientTypes)
            {
                ComboTreeNode node = new ComboTreeNode();
                node.Name = patientType.Id.ToString();
                node.Text = patientType.Name!;
                CheckedTreeComboBoxType.TreeNodes = node;
            }
        }

        private void DisplayCheckedNodes()
        {
            List<string> checkedItems = CheckedListComboBoxPatient.checkedStates.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
            List<string> Items = CheckedListComboBoxPatient.Nodes.ToList();
            CheckedWardNodes = "";
            CheckedPatientNodes = "";
            if (ComboBoxWard.CheckedNodes != null && ComboBoxWard.CheckedNodes.Count > -1)
            {
                foreach (ComboTreeNode node in ComboBoxWard.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedWardNodes = " All wards"; break; }
                    CheckedWardNodes += ((string.IsNullOrEmpty(CheckedWardNodes) ? " " : ", ") + node.Text);
                }
                this.Text = CheckedPatientNodes == "" ? "Patient due list @ ward : " + CheckedWardNodes : ("Patient due list @ ward : " + CheckedWardNodes + CheckedPatientNodes);
            }
            if (checkedItems.Count > 0)
            {
                if (checkedItems.Contains("All"))
                {
                    CheckedPatientNodes = "All Patients";
                }
                else if (checkedItems.Count > 10)
                {
                    CheckedPatientNodes += string.Join(", ", checkedItems.Take(10));
                }
                else
                {
                    CheckedPatientNodes += string.Join(", ", checkedItems);
                }
                if (checkedItems.Count == 0)
                {
                    CheckedPatientNodes = "";
                }
                this.Text = CheckedPatientNodes == "" ? "Patient due list @ ward : " + CheckedWardNodes : ("Patient due list @ ward : " + CheckedWardNodes + " @ patient : " + CheckedPatientNodes);
            }
            if (CheckedWardNodes == "")
            {
                this.Text = CheckedPatientNodes == "" ? "Patient due list" : "Patient due list @ patient : " + CheckedPatientNodes;
            }
            Cursor.Current = Cursors.Default;
        }

        private void CheckedTreeComboBoxType_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            this.Text = "Patient due list";
            PatientDueListErrorMsg.Text = "";
            DataGridViewPatientDueList.Rows.Clear();
            EnableButton(false);
            ComboBoxWard.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxWard.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CheckedListComboBoxPatient.Reset();
            string[] TypeName = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxType).ToArray();
            if (CheckedTreeComboBoxType.CheckedNodes != null && CheckedTreeComboBoxType.CheckedNodes.Count > 0)
            {
                selectedNode = e.Node;
                if (selectedNode != null)
                {
                    if (TypeName.Contains("By OutPatient") && !TypeName.Contains("By InPatient") && !TypeName.Contains("All"))
                    {
                        WardSeparator.Visible = false;
                        toolStripLabelWard.Visible = false;
                        ComboBoxWard.Visible = false;
                        PatientSeparator.Visible = true;
                        toolStripLabelPatient.Visible = true;
                        CheckedListComboBoxPatient.Visible = true;
                    }
                    else
                    {
                        WardSeparator.Visible = true;
                        toolStripLabelWard.Visible = true;
                        ComboBoxWard.Visible = true;
                        PatientSeparator.Visible = true;
                        toolStripLabelPatient.Visible = true;
                        CheckedListComboBoxPatient.Visible = true;
                    }
                }
            }
            else
            {
                WardSeparator.Visible = false;
                toolStripLabelWard.Visible = false;
                ComboBoxWard.Visible = false;
                PatientSeparator.Visible = false;
                toolStripLabelPatient.Visible = false;
                CheckedListComboBoxPatient.Visible = false;
            }
        }
        private void ComboBoxWard_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedNodes();
        }

        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PatientDueListErrorMsg.Text = "";
                DataGridViewPatientDueList.Rows.Clear();
                if (FormValidate())
                {
                    LoadDueList();
                }
            }
            catch (Exception ex)
            {
                PatientDueListErrorMsg.Text = "Errod fetching ledger (Error:" + ex.InnerException!.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool FormValidate()
        {
            EnableButton(false);
            string[] TypeName = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxType).ToArray();
            PatientDueListErrorMsg.Text = "";
            if (CheckedTreeComboBoxType.CheckedNodes.Count < 1)
            {
                PatientDueListErrorMsg.Text = SelectTypeErrorMsg;
                CheckedTreeComboBoxType.Focus();
                return false;
            }
            if (ComboBoxWard.Visible && CheckedTreeUtils.SelectedNodes(ComboBoxWard).Count < 1)
            {
                PatientDueListErrorMsg.Text = SelectWardErrorMsg;
                ComboBoxWard.Focus();
                return false;
            }
            if (CheckedListComboBoxPatient.Visible && SelectedNodes(CheckedListComboBoxPatient).Count < 1)
            {
                PatientDueListErrorMsg.Text = SelectPatientErrorMsg;
                CheckedListComboBoxPatient.Focus();
                return false;
            }

            if (PatientDueListFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientDueListFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientDueListErrorMsg.Text = "Enter valid from date";
                PatientDueListFromDate.Focus();
                return false;
            }
            if (PatientDueListToDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientDueListToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientDueListErrorMsg.Text = "Enter valid to date";
                PatientDueListToDate.Focus();
                return false;
            }
            return true;
        }
        public static List<long> SelectedNodes(ToolstripCheckedListComboBox CheckedComboBox)
        {
            List<long> Ids = new List<long>();
            var checkedComboBox = CheckedComboBox.CheckedListComboBox;
            if (checkedComboBox != null)
            {
                List<string> checkedItems = CheckedComboBox.checkedStates.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
                if (checkedItems.Count > 0)
                {
                    if (checkedItems.Contains("All"))
                    {
                        Ids.AddRange(PatientManager.Instance.ListAllPatient(Global.Company.CompanyId).Select(x => x.Id));
                    }
                    else
                    {
                        foreach (string node in checkedItems)
                        {
                            long id = checkedComboBox.GetId(node);
                            if (id != -1)
                            {
                                Ids.Add(id);
                            }
                        }
                    }
                }
            }
            return Ids;
        }

        private void LoadDueList()
        {
            RptPatientDueList = new RptPatientDueList();
            string[] Type = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxType).ToArray();
            RptPatientDueList.PatientType = Type;
            RptPatientDueList.WardId = CheckedTreeUtils.SelectedNodes(ComboBoxWard).Cast<long?>().ToArray();
            RptPatientDueList.PatientIds = SelectedNodes(CheckedListComboBoxPatient).Cast<long?>().ToArray();
            RptPatientDueList.FromDate = (DateTime)PatientDueListFromDate.Date!;
            RptPatientDueList.ToDate = (DateTime)PatientDueListToDate.Date!;
            RptPatientDueList.Company = Global.Company;
            RptPatientDueList.TransactionDate = Global.getTransactionDate();
            RptPatientDueList.GenerateReport();
            DataGridViewPatientDueList.Rows.Clear();
            if (RptPatientDueList.LineItems != null && RptPatientDueList.LineItems.Count > 0)
            {
                EnableButton(true);
                int i = 0;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                int row = -1;
                string patientdetail = null!;
                double subTotal = 0.00;
                double Total = 0.00;
                string Admissiondetail = "";
                string Admission = "";
                bool admissionAdded = false;

                foreach (RptPatientDueListLineItem lineItem in RptPatientDueList.LineItems.OrderBy(x => x.isIp == true).ThenBy(b => b.PatientNo))
                {
                    if (Type.Contains("All") && Type.Contains("By OutPatient") && Type.Contains("By InPatient"))
                    {
                        Admission = lineItem.isIp == true ? "In Patients" : "Out Patients";
                        if (!admissionAdded && Admissiondetail != Admission)
                        {
                            row = DataGridViewPatientDueList.Rows.Add();
                            DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value = lineItem.isIp == true ? "In Patients" : "Out Patients";
                            Admissiondetail = DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value.ToString()!;
                            admissionAdded = true;
                        }
                    }
                    if (lineItem.openingAmount != 0 && patientdetail == null)
                    {
                        row = DataGridViewPatientDueList.Rows.Add();
                        DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value = "Opening Amount";
                        DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(lineItem.openingAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.openingAmount < 0 ? " Dr" : " Cr");
                        subTotal = lineItem.openingAmount;
                        Total = lineItem.openingAmount;
                    }
                    if (lineItem.Fee != 0 || lineItem.openingAmount != 0)
                    {
                        row = DataGridViewPatientDueList.Rows.Add();

                        if (patientdetail != lineItem.Patientdetail)
                        {
                            if (patientdetail != null)
                            {
                                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;

                                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DESC].Value = "Sub Total";
                                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(subTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (subTotal < 0 ? " Dr" : " Cr");
                                if (admissionAdded && Admissiondetail != Admission)
                                {
                                    row = DataGridViewPatientDueList.Rows.Add();
                                    DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value = lineItem.isIp == true ? "In Patients" : "Out Patients";
                                    Admissiondetail = DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value.ToString()!;
                                    admissionAdded = true;
                                }
                                row++;
                                subTotal = 0.00;
                                DataGridViewPatientDueList.Rows.Add();
                            }
                            if (subTotal == 0.00 && lineItem.openingAmount != 0)
                            {
                                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value = "Opening Amount";
                                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(lineItem.openingAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.openingAmount < 0 ? " Dr" : " Cr");
                                row++;
                                DataGridViewPatientDueList.Rows.Add();
                                subTotal = lineItem.openingAmount;
                                Total += lineItem.openingAmount;
                            }
                            DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.SNO].Value = i + 1;
                            DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.PAGE].Value = lineItem.Age;
                            DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.PID].Value = lineItem.PatientNo;
                            DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.PDETAIL].Value = lineItem.Patientdetail;
                            patientdetail = lineItem.Patientdetail;
                            i++;
                        }
                        DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.FEETYPE].Value = lineItem.FeeType;
                        DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DESC].Value = lineItem.Description;
                        DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(lineItem.Fee).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.Fee < 0 ? " Dr" : " Cr");
                        subTotal += lineItem.Fee;
                        Total += lineItem.Fee;
                    }
                }
                row++;
                DataGridViewPatientDueList.Rows.Add();
                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;

                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DESC].Value = "Sub Total";
                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(subTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (subTotal < 0 ? " Dr" : " Cr");

                row++;
                DataGridViewPatientDueList.Rows.Add();
                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                DataGridViewPatientDueList.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;

                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DESC].Value = "Total Amount";
                DataGridViewPatientDueList.Rows[row].Cells[(int)PatientDueListTableColumn.DUEAMOUNT].Value = Math.Abs(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (Total < 0 ? " Dr" : " Cr");
            }
            else
            {
                PatientDueListErrorMsg.Text = NoInfoFoundErrorMsg;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            WardSeparator.Visible = false;
            toolStripLabelWard.Visible = false;
            ComboBoxWard.Visible = false;
            toolStripLabelPatient.Visible = false;
            PatientSeparator.Visible = false;
            CheckedListComboBoxPatient.Visible = false;
            this.Text = "Patient due list";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)PatientDueListFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PatientDueListToDate.Date!).ToString(Global.Company.DateFormat);
            PatientDueListReportPrintSave patientDueListReportPrintSave = new PatientDueListReportPrintSave();
            patientDueListReportPrintSave.ExportOrPrintToFile(DataGridViewPatientDueList, RptPatientDueList, RptPatientDueList.ReportName(), "Patient Due List", "pdf", false, lFromDate, lToDate);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)PatientDueListFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PatientDueListToDate.Date!).ToString(Global.Company.DateFormat);
            PatientDueListReportPrintSave patientDueListReportPrintSave = new PatientDueListReportPrintSave();
            patientDueListReportPrintSave.ExportOrPrintToFile(DataGridViewPatientDueList, RptPatientDueList, RptPatientDueList.ReportName(), "Patient Due List", "pdf", true, lFromDate, lToDate);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                BtnReset.PerformClick();
                return true;
            }
            else if (keyData == Keys.F8)
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == Keys.F10)
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DataGridViewPatientDueList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.FEETYPE].Value == null)
            {
                if (e.ColumnIndex == (int)PatientDueListTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PatientDueListTableColumn.PAGE || e.ColumnIndex == (int)PatientDueListTableColumn.PDETAIL || e.ColumnIndex == (int)PatientDueListTableColumn.PID)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PatientDueListTableColumn.FEETYPE)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PatientDueListTableColumn.DESC)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
            if (e.RowIndex > -1 && DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.FEETYPE].Value == null
                    && DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.DESC].Value == null)
            {
                if (e.ColumnIndex == (int)PatientDueListTableColumn.DESC)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PatientDueListTableColumn.FEETYPE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        private void DataGridViewPatientDueList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.DESC].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.DataGridViewPatientDueList.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
                this.DataGridViewPatientDueList.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value != null ? DataGridViewPatientDueList.Rows[e.RowIndex].Cells[(int)PatientDueListTableColumn.OP_AMOUNT].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void CheckedListComboBoxPatient_ItemCheckedEvent(object sender, EventArgs e)
        {
            DisplayCheckedNodes();
        }
    }
}
