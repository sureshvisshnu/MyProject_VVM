using fa;
using fa.libraries.utils;
using FADataAccessLibrary.report.Hms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using NPOI.SS.UserModel;
using fa.api.utils;
using fa.views.utils.Report.Hms;
using Fa.views.utils.Report.Hms;
using DocumentFormat.OpenXml.InkML;
using static FADataAccessLibrary.report.Hms.RptFeeCollection;
using VisioForge.MediaFramework.GStreamer.Base;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.VariantTypes;


namespace Fa.reports.Hms
{
    public partial class FormFeeCollectionReport : Form
    {
        enum FeeCollectionAssignTableColumn
        {
            SNO, DATE, PATIENT, OPIP, CONSULTANT, FEETYPE, AMOUNT, AID, ROWHEADING
        }

        public int ReportIndex = 0;
        public int ReportCount = 0;
        public int GetComboIndex = 0;
        public int GetCurrentIndex = -1;

        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public static string InformationMsg = "No Information Found..!";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        private bool allowSelectedIndexChanged = false;
        private List<string>? originalItems;
        private Dictionary<int, int> originalColumnWidths = new Dictionary<int, int>();
        RptFeeCollection rptFeeCollection = null!;

        public FormFeeCollectionReport()
        {
            InitializeComponent();
        }

        private void InitializeColumnWidths(int indexedColumn)
        {
            if (indexedColumn == 0 || indexedColumn == 3 || indexedColumn == 4)
            {
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.SNO].Width = 50;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.DATE].Width = 100;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.PATIENT].Width = 225;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].Width = 125;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Width = 200;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Width = 260;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AMOUNT].Width = 130;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AID].Width = 2;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.ROWHEADING].Width = 2;
            }
            else if (indexedColumn == 1)
            {
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.SNO].Width = 50;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.DATE].Width = 120;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.PATIENT].Width = 300;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].Width = 150;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Width = 0;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Width = 320;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AMOUNT].Width = 150;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AID].Width = 2;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.ROWHEADING].Width = 2;
            }
            else if (indexedColumn == 2)
            {
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.SNO].Width = 50;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.DATE].Width = 120;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.PATIENT].Width = 250;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].Width = 130;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Width = 390;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Width = 0;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AMOUNT].Width = 150;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AID].Width = 2;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.ROWHEADING].Width = 2;
            }
            else 
            {
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.SNO].Width = 50;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.DATE].Width = 100;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.PATIENT].Width = 225;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].Width = 150;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Width = 225;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Width = 375;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AMOUNT].Width = 150;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.AID].Width = 2;
                GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.ROWHEADING].Width = 2;
            }
        }
        private void StoreComboItems()
        {
            ComboBoxFeeCollectionType.Items.Clear();
            foreach (string item in originalItems!)
            {
                ComboBoxFeeCollectionType.Items.Add(item);
            }
        }
        private void FormFeeCollectionReport_Load(object sender, EventArgs e)
        {
            StoreOriginalColumnWidths();
            StoreOriginalItems();
            ResetForm();
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            ComboUtils.InitializeConsultantTypeCombo(CheckedTreeComboBoxConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeConsultationCombo(CheckedTreeComboBoxConsultations, Global.Company.CompanyId);
        }

        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripFeeCollectionReportPrint.Enabled = Enable;
            ToolStripFeeCollectionReportSave.Enabled = Enable;
        }

        private void StoreOriginalColumnWidths()
        {
            originalColumnWidths.Clear();
            foreach (DataGridViewColumn column in GridViewFeeCollection.Columns)
            {
                originalColumnWidths[column.Index] = column.Width;
            }
        }
        private void StoreOriginalItems()
        {
            originalItems = new List<string>();
            foreach (string item in ComboBoxFeeCollectionType.Items)
            {
                originalItems.Add(item);
            }
        }

        private void ResetForm()
        {
            FeeCollectionReportErrMsg.Text = "";
            this.Text = "Fee Charge Report";
            GridViewFeeCollection.Rows.Clear();
            CheckedTreeComboBoxConsultant.SelectedNode = null!;
            CheckedTreeComboBoxConsultations.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsultant.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsultations.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            FeeCollectionFromDate.Format = Global.Company.DateFormat;
            FeeCollectionFromDate.Date = Global.getTransactionDate().AddDays(-30);
            FeeCollectionToDate.Format = Global.Company.DateFormat;
            FeeCollectionToDate.Date = Global.getTransactionDate();
            EnableButton(false);
        }

        private void ToolStripTabIndexChanged()
        {
            if (ComboBoxFeeCollectionType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 0;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 1;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By Fees", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 2;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By OP", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 3;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By IP", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 4;
            }
            else
            {
                GetComboIndex = 0;
            }
        }
        private void SetComboIndex()
        {
            if (ComboBoxFeeCollectionType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 0;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 1;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By Fees", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 2;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By OP", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 3;
            }
            else if (ComboBoxFeeCollectionType.Text.Equals("By IP", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 4;
            }
            else
            {
                GetCurrentIndex = -1;
            }
        }
        private void ExecuteComboBoxSelection()
        {
            try
            {
                ToolStripTabIndexChanged();
                if (GetCurrentIndex != GetComboIndex)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ResetForm();
                    SetComboIndex();
                    if (ComboBoxFeeCollectionType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 0;
                        ComboBoxFeeCollectionType.Text = "By Date";
                        GridViewFeeCollection.Visible = true;
                        CheckedTreeComboBoxConsultant.Visible = false;
                        CheckedTreeComboBoxConsultations.Visible = false;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].HeaderText = "OP/IP";
                        LabelType.Visible = false;
                    }
                    else if (ComboBoxFeeCollectionType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 1;
                        GridViewFeeCollection.Visible = true;
                        CheckedTreeComboBoxConsultations.Visible = false;
                        CheckedTreeComboBoxConsultant.Visible = true;
                        CheckedTreeComboBoxConsultant.Size = new Size(200, 25);
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Visible = false;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].HeaderText = "OP/IP";
                        LabelType.Visible = true;
                        LabelType.Text = "Consultant";

                    }
                    else if (ComboBoxFeeCollectionType.Text.Equals("By Fees", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 2;
                        GridViewFeeCollection.Visible = true;
                        CheckedTreeComboBoxConsultant.Visible = false;
                        CheckedTreeComboBoxConsultations.Visible = true;
                        CheckedTreeComboBoxConsultations.Size = new Size(200, 25);
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Visible = false;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].HeaderText = "OP/IP";
                        LabelType.Visible = true;
                        LabelType.Text = "Fee Type";

                    }
                    else if (ComboBoxFeeCollectionType.Text.Equals("By OP", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 3;
                        GridViewFeeCollection.Visible = true;
                        CheckedTreeComboBoxConsultant.Visible = false;
                        CheckedTreeComboBoxConsultations.Visible = false;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].HeaderText = "OP";
                        LabelType.Visible = false;
                        LabelType.Text = "OP";
                    }
                    else if (ComboBoxFeeCollectionType.Text.Equals("By IP", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 4;
                        GridViewFeeCollection.Visible = true;
                        CheckedTreeComboBoxConsultant.Visible = false;
                        CheckedTreeComboBoxConsultations.Visible = false;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.CONSULTANT].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.FEETYPE].Visible = true;
                        GridViewFeeCollection.Columns[(int)FeeCollectionAssignTableColumn.OPIP].HeaderText = "IP";
                        LabelType.Visible = false;
                        LabelType.Text = "IP";
                    }
                    InitializeColumnWidths(ReportIndex);
                    ComboBoxFeeCollectionType.DroppedDown = false;
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ComboBoxFeeCollectionType_DropDown(object sender, EventArgs e)
        {
            var toolStripComboBox = sender as ToolStripComboBox;
            if (toolStripComboBox == null || toolStripComboBox.Name != "ComboBoxFeeCollectionType")
                return;

            allowSelectedIndexChanged = true;
            toolStripComboBox.Items.Clear();
            toolStripComboBox.Items.AddRange(originalItems!.ToArray());
        }

        private void ComboBoxFeeCollectionType_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxFeeCollectionType.Items.Count == 0)
            {
                StoreComboItems();
            }
        }

        private void ComboBoxFeeCollectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!allowSelectedIndexChanged)
                    return;
                ExecuteComboBoxSelection();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void LoadFeeCollectionHistory()
        {
            FeeCollectionReportErrMsg.Text = "";
            rptFeeCollection = new RptFeeCollection();
            rptFeeCollection.FromDate = (DateTime)(FeeCollectionFromDate?.Date ?? DateTime.MinValue);
            rptFeeCollection.ToDate = (DateTime)(FeeCollectionToDate?.Date ?? DateTime.MinValue);
            rptFeeCollection.Company = Global.Company;
            rptFeeCollection.FilterIndex = ReportIndex;
            rptFeeCollection.ConsultantId = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsultant).ToArray();
            rptFeeCollection.FeeTypes = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxConsultations).ToArray();
            string[] FeeTypes = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxConsultations).ToArray();
            rptFeeCollection.GenerateReport();
            if (rptFeeCollection.LineItems != null && rptFeeCollection.LineItems.Count > 0)
            {
                int i = 1;
                int rowIndex = -1;
                string patientName = string.Empty;
                string ConsultantName = string.Empty;
                string Ftype = string.Empty;
                bool isFirstRow = true;
                DateTime? lDate = null!;
                double subTotal = 0;
                double GrandTotal = 0;
                List<FeeChargeReportLineItem> GroupedLineItems = null!;
                if (ReportIndex == 0 || ReportIndex == 3 || ReportIndex == 4)
                {
                    List<FeeChargeReportLineItem> Lineitems = rptFeeCollection.LineItems.Where(x => x.Fee != 0).ToList();
                    if (ReportIndex == 0)
                    {
                        GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.Fee != 0).ToList();
                    }
                    else if (ReportIndex == 3)
                    {
                        GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.OpIp != "IP").ToList();
                    }
                    else if (ReportIndex == 4)
                    {
                        GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.OpIp != "OP").ToList();
                    }
                    if (GroupedLineItems.Count > 0)
                    {
                        EnableButton(true);
                        foreach (FeeChargeReportLineItem Lineitem in GroupedLineItems)
                        {
                            rowIndex = GridViewFeeCollection.Rows.Add();
                            if (lDate == null || lDate != Lineitem.Date.Date)
                            {
                                if (!isFirstRow)
                                {
                                    GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                                    GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                    GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = "SubTotal";
                                    GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    subTotal = 0;
                                    i = 1;
                                    rowIndex = GridViewFeeCollection.Rows.Add();
                                }
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.DATE].Value = Lineitem.Date.ToString(Global.Company.DateFormat);
                                isFirstRow = false;
                            }
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value = i;
                            if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date)
                            {
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.PATIENT].Value = Lineitem.PatientName;
                                patientName = Lineitem.PatientName;
                            }
                            lDate = Lineitem.Date.Date;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.OPIP].Value = Lineitem.OpIp;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value = Lineitem.ConsultantName;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = Lineitem.FeeType;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            subTotal += Lineitem.Fee;
                            GrandTotal += Lineitem.Fee;
                            i++;
                        }
                        AddSubTotalRow(rowIndex, subTotal, GrandTotal);
                    }
                    else
                    {
                        FeeCollectionReportErrMsg.Text = InformationMsg;
                    }
                }
                else if (ReportIndex == 1)
                {
                    foreach (FeeChargeReportLineItem Lineitem in rptFeeCollection.LineItems.Where(x => x.Fee != 0).OrderBy(x => x.ConsultantName))
                    {
                        rowIndex = GridViewFeeCollection.Rows.Add();
                        if (ConsultantName == string.Empty || ConsultantName != Lineitem.ConsultantName)
                        {
                            if (!isFirstRow)
                            {
                                GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = "SubTotal";
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                subTotal = 0;
                                i = 1;
                                rowIndex = GridViewFeeCollection.Rows.Add();
                            }
                            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.ROWHEADING].Value = "Consultant Name : " + Lineitem.ConsultantName;
                            ConsultantName = Lineitem.ConsultantName;
                            isFirstRow = false;
                            rowIndex = GridViewFeeCollection.Rows.Add();
                        }
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value = i;
                        if (lDate == null || lDate != Lineitem.Date.Date)
                        {
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.DATE].Value = Lineitem.Date.ToString(Global.Company.DateFormat);
                        }
                        if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date)
                        {
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.PATIENT].Value = Lineitem.PatientName;
                            patientName = Lineitem.PatientName;
                        }
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.OPIP].Value = Lineitem.OpIp;
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = Lineitem.FeeType;
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        lDate = Lineitem.Date.Date;
                        subTotal += Lineitem.Fee;
                        GrandTotal += Lineitem.Fee;
                        i++;
                    }
                    AddSubTotalRow(rowIndex, subTotal, GrandTotal);
                }
                else if (ReportIndex == 2)
                {
                    foreach (FeeChargeReportLineItem Lineitem in rptFeeCollection.LineItems.Where(x => x.Fee != 0).OrderBy(x => x.FeeType))
                    {
                        rowIndex = GridViewFeeCollection.Rows.Add();
                        if (Ftype == string.Empty || Ftype != Lineitem.FeeType)
                        {
                            if (!isFirstRow)
                            {
                                GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value = "SubTotal";
                                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                subTotal = 0;
                                i = 1;
                                rowIndex = GridViewFeeCollection.Rows.Add();
                            }
                            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.ROWHEADING].Value = "Fee Type : " + Lineitem.FeeType;
                            isFirstRow = false;
                            rowIndex = GridViewFeeCollection.Rows.Add();
                        }
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value = i;
                        if (lDate == null || lDate != Lineitem.Date.Date || Ftype != Lineitem.FeeType)
                        {
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.DATE].Value = Lineitem.Date.ToString(Global.Company.DateFormat);
                        }
                        if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date || Ftype != Lineitem.FeeType)
                        {
                            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.PATIENT].Value = Lineitem.PatientName;
                            patientName = Lineitem.PatientName;
                        }
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.OPIP].Value = Lineitem.OpIp;
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value = Lineitem.ConsultantName;
                        GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        lDate = Lineitem.Date.Date;
                        Ftype = Lineitem.FeeType;
                        subTotal += Lineitem.Fee;
                        GrandTotal += Lineitem.Fee;
                        i++;
                    }
                    AddSubTotalRow(rowIndex, subTotal, GrandTotal);
                }
            }
            else
            {
                FeeCollectionReportErrMsg.Text = InformationMsg;
            }
        }
        private void AddSubTotalRow(int rowIndex, double subTotal, double GrandTotal)
        {
            rowIndex = GridViewFeeCollection.Rows.Add();
            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            if (ReportIndex == 2)
            {
                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value = "SubTotal";
            }
            else
            {
                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = "SubTotal";
            }
            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            rowIndex = GridViewFeeCollection.Rows.Add();
            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            GridViewFeeCollection.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            if (ReportIndex == 2)
            {
                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value = "GrandTotal";
            }
            else
            {
                GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value = "GrandTotal";
            }
            GridViewFeeCollection.Rows[rowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value = GrandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void FeeCollectionGotButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewFeeCollection.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadFeeCollectionHistory();
                }
            }
            catch (Exception ex)
            {
                FeeCollectionReportErrMsg.Text = "Error fetching Lab Test (Error:" + ex.InnerException?.Message?? "" + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool FormValidate()
        {
            FeeCollectionReportErrMsg.Text = "";
            if (ComboBoxFeeCollectionType.SelectedIndex < 0)
            {
                FeeCollectionReportErrMsg.Text = string.Format(EnterValidTypeErrorMsg, "Type...");
                GetCurrentIndex = -1;
                ComboBoxFeeCollectionType.Select();
                return false;
            }
            if (ComboBoxFeeCollectionType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsultant).Count < 1)
            {
                FeeCollectionReportErrMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                GetCurrentIndex = -1;
                CheckedTreeComboBoxConsultant.Focus();
                return false;
            }
            if (ComboBoxFeeCollectionType.Text.Equals("By Fees", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxConsultations).Count < 1)
            {
                FeeCollectionReportErrMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                GetCurrentIndex = -1;
                CheckedTreeComboBoxConsultations.Focus();
                return false;
            }
            if (FeeCollectionFromDate.Date == null || !DateUtils.ValidDate(((DateTime)FeeCollectionFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                FeeCollectionReportErrMsg.Text = EnterValidDateErrorMsg;
                GetCurrentIndex = -1;
                FeeCollectionFromDate.Focus();
                return false;
            }
            if (FeeCollectionToDate.Date == null || !DateUtils.ValidDate(((DateTime)FeeCollectionToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                FeeCollectionReportErrMsg.Text = EnterValidDateErrorMsg;
                GetCurrentIndex = -1;
                FeeCollectionToDate.Focus();
                return false;
            }
            if (FeeCollectionFromDate.Date != null && FeeCollectionToDate.Date != null && FeeCollectionFromDate.Date > FeeCollectionToDate.Date)
            {
                FeeCollectionReportErrMsg.Text = CheckValidDateErrorMsg;
                GetCurrentIndex = -1;
                FeeCollectionFromDate.Focus();
                return false;
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
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
                else if (keyData == (Keys.Shift | Keys.Tab))
                {
                    return true;
                }
                else if (keyData == Keys.Tab)
                {
                    ToolStripTabIndexChanged();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void GridViewFeeCollection_SelectionChanged(object sender, EventArgs e)
        {
            GridViewFeeCollection.ClearSelection();
        }

        private void GridViewFeeCollection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridViewFeeCollection.ClearSelection();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string lFromDate = ((DateTime)FeeCollectionFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)FeeCollectionToDate.Date!).ToString(Global.Company.DateFormat);
            DateTime transactionDate = Global.getTransactionDate();
            FeeCollectionReportSavePrint FCReportSavePrint = new FeeCollectionReportSavePrint();
            FCReportSavePrint.ExportOrPrintToFile(GridViewFeeCollection, rptFeeCollection, "Fee Charge Report " + transactionDate.ToString(Global.Company.DateFormat), "Fee Charge Report", "pdf", false, lFromDate, lToDate, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void ToolStripFeeCollectionReportSave_Click(object sender, EventArgs e)
        {
            BtnSave.PerformClick();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string lFromDate = ((DateTime)FeeCollectionFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)FeeCollectionToDate.Date!).ToString(Global.Company.DateFormat);
            DateTime transactionDate = Global.getTransactionDate();
            FeeCollectionReportSavePrint FCReportSavePrint = new FeeCollectionReportSavePrint();
            FCReportSavePrint.ExportOrPrintToFile(GridViewFeeCollection, rptFeeCollection, "Fee Charge Report " + transactionDate.ToString(Global.Company.DateFormat), "Fee Charge Report", "pdf", true, lFromDate, lToDate, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void ToolStripFeeCollectionReportPrint_Click(object sender, EventArgs e)
        {
            BtnPrint.PerformClick();
        }

        private void GridViewFeeCollection_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5 && (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value == "SubTotal" ||
                    GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value == "GrandTotal"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                if (e.ColumnIndex == 4 && (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value == "SubTotal" ||
                    GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value == "GrandTotal"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }

                if (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value == null && 
                    (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value == "SubTotal" ||
                    GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.FEETYPE].Value == "GrandTotal"))
                {
                    if (e.ColumnIndex == (int)FeeCollectionAssignTableColumn.SNO)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    else if (e.ColumnIndex < 5)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                if (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value == null && 
                    (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value == "SubTotal" ||
                    GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.CONSULTANT].Value == "GrandTotal"))
                {
                    if (e.ColumnIndex == (int)FeeCollectionAssignTableColumn.SNO)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    else if (e.ColumnIndex < 4)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                if (GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value == null &&
                    GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.AMOUNT].Value == null)
                {
                    if (e.ColumnIndex == (int)FeeCollectionAssignTableColumn.SNO)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    else if (e.ColumnIndex < 6)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
            }
        }

        private void GridViewFeeCollection_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == (int)FeeCollectionAssignTableColumn.SNO) && e.Value != null)
            {
                DataGridViewCell cell = GridViewFeeCollection.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.WrapMode = DataGridViewTriState.False;
            }
        }

        private void GridViewFeeCollection_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.SNO].Value == null) && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.GridViewFeeCollection.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.GridViewFeeCollection.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string? rr = GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.ROWHEADING].Value != null ? GridViewFeeCollection.Rows[e.RowIndex].Cells[(int)FeeCollectionAssignTableColumn.ROWHEADING].Value?.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private void DisplayCheckedInformation()
        {
            string CheckedConsultantNodes = string.Empty;
            string CheckedConsultationNodes = string.Empty;
            if (CheckedTreeComboBoxConsultant.CheckedNodes != null && CheckedTreeComboBoxConsultant.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedTreeComboBoxConsultant.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedConsultantNodes = " All Consultants"; break; }
                    CheckedConsultantNodes += ((string.IsNullOrEmpty(CheckedConsultantNodes) ? " " : ", ") + node.Text);
                }
                this.Text = "Fee Charge Report @ Consultant : " + CheckedConsultantNodes;
            }
            if (CheckedTreeComboBoxConsultations.CheckedNodes != null && CheckedTreeComboBoxConsultations.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedTreeComboBoxConsultations.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedConsultationNodes = " All Consultations"; break; }
                    CheckedConsultationNodes += ((string.IsNullOrEmpty(CheckedConsultationNodes) ? " " : ", ") + node.Text);
                }
                this.Text = "Fee Charge Report @ Consultation : " + CheckedConsultationNodes;
            }
            if (CheckedConsultantNodes == "" && CheckedConsultationNodes == "")
            {
                this.Text = "Fee Charge Report";
            }
        }

        private void CheckedTreeComboBoxConsultant_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void CheckedTreeComboBoxConsultations_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
    }
}
