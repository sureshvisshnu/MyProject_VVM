using fa;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
using Fa.api.Hms;
using Fa.reports.Inventory;
using Fa.reports.sales;
using Fa.views.utils.Report.Hms;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.sales;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using Global = fa.Global;


namespace Fa.reports.Hms
{
    enum UnAssignReportByDateTableColumn
    {
        SNO, NAME, AGE, ADDRESS, JOB_TITLE, DEPARTMENT, STATUS, DATE
    }
    enum UnAssignReportByDoctorTableColumn
    {
        SNO, DATE, AGE, ADDRESS, DEPARTMENT, STATUS, NAME
    }
    enum UnAssignReportByNurseTableColumn
    {
        SNO, DATE, AGE, ADDRESS, DEPARTMENT, STATUS, NAME
    }
    enum UnAssignReportByDepartmentTableColumn
    {
        SNO, DATE, NAME, AGE, ADDRESS, JOB_TITLE, STATUS, DEPARTMENT
    }
    public partial class FormCareTakerUnAssignedReport : Form
    {
        RptUnAssignedReport RptUnAssignedReport = null;

        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string ComboBoxReportTypeErrorMsg = "Please select type...";
        public static string ComboBoxDoctorErrorMsg = "Please select Doctor's...";
        public static string ComboBoxNurseErrorMsg  = "Please select Nurse's...";
        public static string ComboBoxDepartmentErrorMsg = "Please select Department...";
        public static string CTReportFromDateErrorMsg = "Please adjust the 'From' date, as it cannot be later than the 'To' date...";
        public static string NoInformationErrorMsg = "No information found...";
        public static string UnAssignedReportFormText = "Care Taker UnAssigned Report";
        public FormCareTakerUnAssignedReport()
        {
            InitializeComponent();
        }

        private void FormCareTakerUnAssignedReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCombo();
            ResetForm();
            Cursor.Current = Cursors.Default;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeDoctorCombo(ComboBoxDoctor, Global.Company.CompanyId);
            ComboUtils.InitializeNurseCombo(ComboBoxNurse, Global.Company.CompanyId);
            ComboUtils.InitializeAllDepartmentCombo(ComboBoxDepartment, Global.Company.CompanyId);
        }
        private void ResetForm()
        {
            EnableButtons(false);
            this.Text = "Care Taker UnAssigned Report";
            GridviewUnAssignReportByDate.Rows.Clear();
            GridviewUnAssignReportByDepartment.Rows.Clear();
            GridviewUnAssignReportByDoctor.Rows.Clear();
            GridviewUnAssignReportByNurse.Rows.Clear();
            ComboBoxDepartment.Nodes.Clear();
            ComboBoxDoctor.Nodes.Clear();
            ComboBoxReportType.SelectedIndex = 0;
            CTReportFromDate.Format = Global.Company.DateFormat;
            CTReportToDate.Format = Global.Company.DateFormat;
            CTReportFromDate.Date = DateTime.Now.AddDays(-5);
            CTReportToDate.Date = DateTime.Now;
            ComboBoxDepartment.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxDepartment.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            ComboBoxDoctor.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxDoctor.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            ComboBoxNurse.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxNurse.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UnAssignReportErrorMsg.Text = "";
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                this.Text = UnAssignedReportFormText;
                GridviewUnAssignReportByDoctor.Visible = false;
                GridviewUnAssignReportByDepartment.Visible = false;
                GridviewUnAssignReportByDate.Visible = true;
                ComboBoxDepartment.Visible = false;
                ComboBoxDoctor.Visible = false;
                LabelDoctor.Visible = false;
                LabelDepartment.Visible = false;
                SeparatorDepDoc.Visible = false;
                LableNurse.Visible = false;
                ComboBoxNurse.Visible = false;
                GridviewUnAssignReportByNurse.Visible = false;
                GridviewUnAssignReportByDepartment.Rows.Clear();
                GridviewUnAssignReportByDoctor.Rows.Clear();
                GridviewUnAssignReportByNurse.Rows.Clear();
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ComboBoxDoctor.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in ComboBoxDoctor.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                LoadCombo();
                this.Text = UnAssignedReportFormText;
                GridviewUnAssignReportByDoctor.Visible = true;
                ComboBoxDoctor.Visible = true;
                GridviewUnAssignReportByDepartment.Visible = false;
                GridviewUnAssignReportByDate.Visible = false;
                ComboBoxDepartment.Visible = false;
                LabelDepartment.Visible = false;
                LabelDoctor.Visible = true;
                SeparatorDepDoc.Visible = true;
                LableNurse.Visible = false;
                ComboBoxNurse.Visible = false;
                GridviewUnAssignReportByNurse.Visible = false;
                GridviewUnAssignReportByDate.Rows.Clear();
                GridviewUnAssignReportByDepartment.Rows.Clear();
                GridviewUnAssignReportByNurse.Rows.Clear();

            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ComboBoxNurse.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in ComboBoxNurse.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                LoadCombo();
                this.Text = UnAssignedReportFormText;
                GridviewUnAssignReportByDoctor.Visible = false;
                ComboBoxDoctor.Visible = false;
                GridviewUnAssignReportByDepartment.Visible = false;
                GridviewUnAssignReportByDate.Visible = false;
                ComboBoxDepartment.Visible = false;
                LabelDepartment.Visible = false;
                LabelDoctor.Visible = false;
                SeparatorDepDoc.Visible = true;
                LableNurse.Visible = true;
                ComboBoxNurse.Visible = true;
                GridviewUnAssignReportByNurse.Visible = true;
                GridviewUnAssignReportByDate.Rows.Clear();
                GridviewUnAssignReportByDepartment.Rows.Clear();
                GridviewUnAssignReportByDoctor.Rows.Clear();
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                ComboBoxDepartment.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in ComboBoxDepartment.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                LoadCombo();
                this.Text = UnAssignedReportFormText;
                GridviewUnAssignReportByDepartment.Visible = true;
                GridviewUnAssignReportByDoctor.Visible = false;
                GridviewUnAssignReportByDate.Visible = false;
                ComboBoxDoctor.Visible = false;
                ComboBoxDepartment.Visible = true;
                LabelDoctor.Visible = false;
                LabelDepartment.Visible = true;
                SeparatorDepDoc.Visible = true;
                LableNurse.Visible = false;
                ComboBoxNurse.Visible = false;
                GridviewUnAssignReportByNurse.Visible = false;
                GridviewUnAssignReportByDate.Rows.Clear();
                GridviewUnAssignReportByDoctor.Rows.Clear();
                GridviewUnAssignReportByNurse.Rows.Clear();
            }
            Cursor.Current = Cursors.Default;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReset.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private bool FormValidate()
        {
            UnAssignReportErrorMsg.Text = "";
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                UnAssignReportErrorMsg.Text = ComboBoxReportTypeErrorMsg;
                ComboBoxReportType.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(ComboBoxDoctor).Count < 1)
            {
                UnAssignReportErrorMsg.Text = ComboBoxDoctorErrorMsg;
                ComboBoxDoctor.Focus();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(ComboBoxNurse).Count < 1)
            {
                UnAssignReportErrorMsg.Text = ComboBoxNurseErrorMsg;
                ComboBoxNurse.Focus();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 3 && CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
            {
                UnAssignReportErrorMsg.Text = ComboBoxDepartmentErrorMsg;
                ComboBoxDepartment.Focus();
                return false;
            }
            if (CTReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)CTReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                UnAssignReportErrorMsg.Text = EnterValidDateErrorMsg;
                CTReportFromDate.Focus();
                return false;
            }
            if (CTReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)CTReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                UnAssignReportErrorMsg.Text = EnterValidDateErrorMsg;
                CTReportToDate.Focus();
                return false;
            }
            if (CTReportToDate.Date < CTReportFromDate.Date)
            {
                UnAssignReportErrorMsg.Text = CTReportFromDateErrorMsg;
                CTReportFromDate.Focus();
                return false;
            }
            return true;
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxReportType.SelectedIndex == 1)
            {
                if (ComboBoxDoctor.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in ComboBoxDoctor.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Doctor's"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = UnAssignedReportFormText + " For " + CheckedNodes;
                }
                else
                {
                    this.Text = UnAssignedReportFormText;
                }
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                if (ComboBoxDepartment.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in ComboBoxDepartment.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Department's"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = UnAssignedReportFormText + " For " + CheckedNodes;
                }
                else
                {
                    this.Text = UnAssignedReportFormText;
                }
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                if (ComboBoxNurse.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in ComboBoxNurse.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Nurse's"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = UnAssignedReportFormText + " For " + CheckedNodes;
                }
                else
                {
                    this.Text = UnAssignedReportFormText;
                }
            }
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Nodes";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }

        private void ComboBoxDepartment_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ComboBoxDoctor_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxNurse_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadUnAssignReport();
                }
            }
            catch (Exception ex)
            {
                UnAssignReportErrorMsg.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            GridviewUnAssignReportByDate.DefaultCellStyle.SelectionForeColor = GridviewUnAssignReportByDate.DefaultCellStyle.ForeColor;
            GridviewUnAssignReportByDate.DefaultCellStyle.SelectionBackColor = GridviewUnAssignReportByDate.DefaultCellStyle.BackColor;

            GridviewUnAssignReportByDepartment.DefaultCellStyle.SelectionForeColor = GridviewUnAssignReportByDepartment.DefaultCellStyle.ForeColor;
            GridviewUnAssignReportByDepartment.DefaultCellStyle.SelectionBackColor = GridviewUnAssignReportByDepartment.DefaultCellStyle.BackColor;

            GridviewUnAssignReportByDoctor.DefaultCellStyle.SelectionForeColor = GridviewUnAssignReportByDoctor.DefaultCellStyle.ForeColor;
            GridviewUnAssignReportByDoctor.DefaultCellStyle.SelectionBackColor = GridviewUnAssignReportByDoctor.DefaultCellStyle.BackColor;

            GridviewUnAssignReportByNurse.DefaultCellStyle.SelectionForeColor = GridviewUnAssignReportByNurse.DefaultCellStyle.ForeColor;
            GridviewUnAssignReportByNurse.DefaultCellStyle.SelectionBackColor = GridviewUnAssignReportByNurse.DefaultCellStyle.BackColor;
        }
        private void LoadUnAssignReport()
        {
            RptUnAssignedReport = new RptUnAssignedReport();
            RptUnAssignedReport.FromDate = (DateTime)CTReportFromDate.Date;
            RptUnAssignedReport.ToDate = (DateTime)CTReportToDate.Date;
            RptUnAssignedReport.Company = Global.Company;
            RptUnAssignedReport.Type = ComboBoxReportType.SelectedIndex == 0 ? ReportType.BYDATE : ComboBoxReportType.SelectedIndex == 1 ? ReportType.BYDOCTOR : ComboBoxReportType.SelectedIndex == 2 ? ReportType.BYNURSE : ReportType.BYDEPARTMENT;
            if (ComboBoxReportType.SelectedIndex == 1)
            {
                RptUnAssignedReport.ReportHeader = "For" + " @ " + SelectedNodesText(ComboBoxDoctor);
            }
            if (ComboBoxReportType.SelectedIndex == 1)
            {
                RptUnAssignedReport.DoctorId = CheckedTreeUtils.SelectedNodes(ComboBoxDoctor).ToArray();
                RptUnAssignedReport.Doctor = this.Text;
                RptUnAssignedReport.IsAllDoctorId = SelectedNodesText(ComboBoxDoctor) == "All Nodes" ? true : false;
            }
            if (ComboBoxReportType.SelectedIndex == 2)
            {
                RptUnAssignedReport.ReportHeader = "For" + " @ " + SelectedNodesText(ComboBoxNurse);
            }
            if (ComboBoxReportType.SelectedIndex == 2)
            {
                RptUnAssignedReport.NurseId = CheckedTreeUtils.SelectedNodes(ComboBoxNurse).ToArray();
                RptUnAssignedReport.Nurse = this.Text;
                RptUnAssignedReport.IsAllNurseId = SelectedNodesText(ComboBoxNurse) == "All Nodes" ? true : false;
            }
            if (ComboBoxReportType.SelectedIndex == 3)
            {
                RptUnAssignedReport.ReportHeader = "For" + " @ " + SelectedNodesText(ComboBoxDepartment);
            }
            if (ComboBoxReportType.SelectedIndex == 3)
            {
                RptUnAssignedReport.NurseId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();
                RptUnAssignedReport.Nurse = this.Text;
                RptUnAssignedReport.IsAllNurseId = SelectedNodesText(ComboBoxDepartment) == "All Nodes" ? true : false;
            }
            RptUnAssignedReport.DoctorId = CheckedTreeUtils.SelectedNodes(ComboBoxDoctor).ToArray();
            RptUnAssignedReport.NurseId = CheckedTreeUtils.SelectedNodes(ComboBoxNurse).ToArray();
            RptUnAssignedReport.DepartmentId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();
            RptUnAssignedReport.GenerateReport();

            if (RptUnAssignedReport.Type == ReportType.BYDATE)
            {
                GridviewUnAssignReportByDate.Rows.Clear();
                EnableButtons(false);

                if (RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate != null && RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.Count > 0)
                {
                    UnAssignReportErrorMsg.Text = string.Empty;
                    EnableButtons(true);
                    int Sn = 1;
                    int rowCount = 0;
                    String currentAssignDate = null;

                    foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.AssignDate).ThenBy(x => x.JobTitle))
                    {
                        String formattedAssignDate = DateUtils.FormatDate(LineItem.AssignDate, Global.Company.DateFormat);

                        if (currentAssignDate == null || currentAssignDate != formattedAssignDate)
                        {
                            GridviewUnAssignReportByDate.Rows.Add();
                            GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.DATE].Value = " Date : " + formattedAssignDate;

                            currentAssignDate = formattedAssignDate;
                            Sn = 1;
                            rowCount++;
                        }
                        GridviewUnAssignReportByDate.Rows.Add();
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.SNO].Value = Sn;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.NAME].Value = LineItem.Name;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.AGE].Value = LineItem.Age;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.ADDRESS].Value = LineItem.Address;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.JOB_TITLE].Value = LineItem.JobTitle;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.DEPARTMENT].Value = LineItem.DepartmentOfConsultant;
                        GridviewUnAssignReportByDate.Rows[rowCount].Cells[(int)UnAssignReportByDateTableColumn.STATUS].Value = LineItem.TaskStatus;
                        Sn++;
                        rowCount++;
                    }
                }
                else
                {
                    UnAssignReportErrorMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (RptUnAssignedReport.Type == ReportType.BYDOCTOR)
            {
                GridviewUnAssignReportByDoctor.Rows.Clear();
                EnableButtons(false);

                if (RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate != null && RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.Count > 0)
                {
                    UnAssignReportErrorMsg.Text = string.Empty;
                    EnableButtons(true);
                    int Sn = 1;
                    int rowCount = 0;
                    String DoctorName = string.Empty;

                    foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.Name))
                    {
                        if (DoctorName == null || DoctorName != LineItem.Name)
                        {
                            GridviewUnAssignReportByDoctor.Rows.Add();
                            GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.NAME].Value = " Doctor Name : " + LineItem.Name;

                            DoctorName = LineItem.Name;
                            Sn = 1;
                            rowCount++;
                        }
                        GridviewUnAssignReportByDoctor.Rows.Add();
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.SNO].Value = Sn;
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.DATE].Value = LineItem.AssignDate.ToString(Global.Company.DateFormat);
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.AGE].Value = LineItem.Age;
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.ADDRESS].Value = LineItem.Address;
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.DEPARTMENT].Value = LineItem.DepartmentOfConsultant;
                        GridviewUnAssignReportByDoctor.Rows[rowCount].Cells[(int)UnAssignReportByDoctorTableColumn.STATUS].Value = LineItem.TaskStatus;
                        Sn++;
                        rowCount++;
                    }
                }
                else
                {
                    UnAssignReportErrorMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (RptUnAssignedReport.Type == ReportType.BYNURSE)
            {
                GridviewUnAssignReportByNurse.Rows.Clear();
                EnableButtons(false);

                if (RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate != null && RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.Count > 0)
                {
                    UnAssignReportErrorMsg.Text = string.Empty;
                    EnableButtons(true);
                    int Sn = 1;
                    int rowCount = 0;
                    String NurseName = string.Empty;

                    foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.Name))
                    {
                        if (NurseName == null || NurseName != LineItem.Name)
                        {
                            GridviewUnAssignReportByNurse.Rows.Add();
                            GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.NAME].Value = " Nurse Name : " + LineItem.Name;

                            NurseName = LineItem.Name;
                            Sn = 1;
                            rowCount++;
                        }
                        GridviewUnAssignReportByNurse.Rows.Add();
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.SNO].Value = Sn;
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.DATE].Value = LineItem.AssignDate.ToString(Global.Company.DateFormat);
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.AGE].Value = LineItem.Age;
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.ADDRESS].Value = LineItem.Address;
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.DEPARTMENT].Value = LineItem.DepartmentOfConsultant;
                        GridviewUnAssignReportByNurse.Rows[rowCount].Cells[(int)UnAssignReportByNurseTableColumn.STATUS].Value = LineItem.TaskStatus;
                        Sn++;
                        rowCount++;
                    }
                }
                else
                {
                    UnAssignReportErrorMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (RptUnAssignedReport.Type == ReportType.BYDEPARTMENT)
            {
                GridviewUnAssignReportByDepartment.Rows.Clear();
                EnableButtons(false);

                if (RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate != null && RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.Count > 0)
                {
                    UnAssignReportErrorMsg.Text = string.Empty;
                    EnableButtons(true);
                    int Sn = 1;
                    int rowCount = 0;
                    String Department = string.Empty;
                    String currentAssignDate = null;

                    foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.DepartmentOfConsultant).ThenBy(x => x.AssignDate))
                    {
                        if (Department == null || Department != LineItem.DepartmentOfConsultant)
                        {
                            GridviewUnAssignReportByDepartment.Rows.Add();
                            GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.DEPARTMENT].Value = " Department Name : " + LineItem.DepartmentOfConsultant;

                            Department = LineItem.DepartmentOfConsultant;
                            Sn = 1;
                            rowCount++;
                            currentAssignDate = null;
                        }
                        GridviewUnAssignReportByDepartment.Rows.Add();
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.SNO].Value = Sn;
                        String formattedAssignDate = DateUtils.FormatDate(LineItem.AssignDate, Global.Company.DateFormat);

                        if (currentAssignDate == null || currentAssignDate != formattedAssignDate)
                        {
                            GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.DATE].Value = formattedAssignDate;
                            currentAssignDate = formattedAssignDate;
                        }
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.NAME].Value = LineItem.Name;
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.AGE].Value = LineItem.Age;
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.ADDRESS].Value = LineItem.Address;
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.JOB_TITLE].Value = LineItem.JobTitle;
                        GridviewUnAssignReportByDepartment.Rows[rowCount].Cells[(int)UnAssignReportByDepartmentTableColumn.STATUS].Value = LineItem.TaskStatus;
                        Sn++;
                        rowCount++;
                    }
                }
                else
                {
                    UnAssignReportErrorMsg.Text = NoInformationErrorMsg;
                }
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UnAssignedCareTakerReportPrintSave UnAssignedCareTakerReportPrintSave = new UnAssignedCareTakerReportPrintSave();
            UnAssignedCareTakerReportPrintSave.ExportOrPrintToFile(RptUnAssignedReport, "UnAssigned CareTaker Report", "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            UnAssignedCareTakerReportPrintSave UnAssignedCareTakerReportPrintSave = new UnAssignedCareTakerReportPrintSave();
            UnAssignedCareTakerReportPrintSave.ExportOrPrintToFile(RptUnAssignedReport, "UnAssigned CareTaker Report", "pdf", true);
            Cursor.Current = Cursors.Default;
        }
        private void GridviewUnAssignReportByDate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewUnAssignReportByDate.Rows[e.RowIndex].Cells[(int)UnAssignReportByDateTableColumn.STATUS].Value == null)
            {
                if (e.ColumnIndex == (int)UnAssignReportByDateTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)UnAssignReportByDateTableColumn.STATUS)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridviewUnAssignReportByDate_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewUnAssignReportByDate.Rows[e.RowIndex].Cells[(int)UnAssignReportByDateTableColumn.STATUS].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridviewUnAssignReportByDate.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridviewUnAssignReportByDate.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridviewUnAssignReportByDate.Rows[e.RowIndex].Cells[(int)UnAssignReportByDateTableColumn.DATE].Value != null ? GridviewUnAssignReportByDate.Rows[e.RowIndex].Cells[(int)UnAssignReportByDateTableColumn.DATE].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridviewUnAssignReportByDoctor_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewUnAssignReportByDoctor.Rows[e.RowIndex].Cells[(int)UnAssignReportByDoctorTableColumn.STATUS].Value == null)
            {
                if (e.ColumnIndex == (int)UnAssignReportByDoctorTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)UnAssignReportByDoctorTableColumn.STATUS)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridviewUnAssignReportByDoctor_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewUnAssignReportByDoctor.Rows[e.RowIndex].Cells[(int)UnAssignReportByDoctorTableColumn.STATUS].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridviewUnAssignReportByDoctor.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridviewUnAssignReportByDoctor.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridviewUnAssignReportByDoctor.Rows[e.RowIndex].Cells[(int)UnAssignReportByDoctorTableColumn.NAME].Value != null ? GridviewUnAssignReportByDoctor.Rows[e.RowIndex].Cells[(int)UnAssignReportByDoctorTableColumn.NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridviewUnAssignReportByNurse_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewUnAssignReportByNurse.Rows[e.RowIndex].Cells[(int)UnAssignReportByNurseTableColumn.STATUS].Value == null)
            {
                if (e.ColumnIndex == (int)UnAssignReportByNurseTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)UnAssignReportByNurseTableColumn.STATUS)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridviewUnAssignReportByNurse_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewUnAssignReportByNurse.Rows[e.RowIndex].Cells[(int)UnAssignReportByNurseTableColumn.STATUS].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridviewUnAssignReportByNurse.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridviewUnAssignReportByNurse.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridviewUnAssignReportByNurse.Rows[e.RowIndex].Cells[(int)UnAssignReportByNurseTableColumn.NAME].Value != null ? GridviewUnAssignReportByNurse.Rows[e.RowIndex].Cells[(int)UnAssignReportByNurseTableColumn.NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridviewUnAssignReportByDepartment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewUnAssignReportByDepartment.Rows[e.RowIndex].Cells[(int)UnAssignReportByDepartmentTableColumn.STATUS].Value == null)
            {
                if (e.ColumnIndex == (int)UnAssignReportByDepartmentTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)UnAssignReportByDepartmentTableColumn.STATUS)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridviewUnAssignReportByDepartment_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewUnAssignReportByDepartment.Rows[e.RowIndex].Cells[(int)UnAssignReportByDepartmentTableColumn.STATUS].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridviewUnAssignReportByDepartment.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridviewUnAssignReportByDepartment.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridviewUnAssignReportByDepartment.Rows[e.RowIndex].Cells[(int)UnAssignReportByDepartmentTableColumn.DEPARTMENT].Value != null ? GridviewUnAssignReportByDepartment.Rows[e.RowIndex].Cells[(int)UnAssignReportByDepartmentTableColumn.DEPARTMENT].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }
    }
}
