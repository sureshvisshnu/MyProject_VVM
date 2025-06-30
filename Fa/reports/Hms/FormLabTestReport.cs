using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Wordprocessing;
using fa;
using fa.api.utils;
using fa.context;
using fa.libraries.utils;
using fa.model.Accounting.Transactions;
using fa.model.Hms.Master;
using fa.reports.Inventory;
using fa.views.controls.ComboTreeView;
using fa.views.controls.text;
using fa.views.utils;
using Fa.reports.Inventory;
using Fa.views.utils.Report.Hms;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using static FADataAccessLibrary.report.Hms.RptLabTestDetails;
using Color = System.Drawing.Color;
using static fa.views.utils.Common.DataGridViewColoumnAdjustment;
using fa.views.controls;
using DocumentFormat.OpenXml.Spreadsheet;
using fa.views.utils.Report.Hms;
using Fa.views.controls.ComboBoxSearchable;
using fa.reports.Hms;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using Fa.reports.Hms;
using NPOI.OpenXmlFormats.Spreadsheet;
using fa.views.controls.grid;


namespace Fa.reports.Hms
{
    enum LabTestReportTableColumn
    {
        DATE, PATIENTID, PATIENTNAME, TESTNAME, ELEMENT, UOM, CLASS, SUBCLASS, SINGLEVALUE, RANGEFROM, RANGETO, RESULT, ROWHEADING
    }
    public partial class FormLabTestReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string EnterValidTypeErrorMsg = "Please select {0}";

        RptLabTestDetails rptLabTestDetails = null!;
        RptLabTestDetails ReporptLabTestDetails = new RptLabTestDetails();
        public int ReportIndex = 0;
        public int ReportCount = 0;
        public int GetComboIndex = 0;
        public int GetCurrentIndex = -1;
        private List<string>? originalItems;

        public FormLabTestReport()
        {
            InitializeComponent();
        }
        private void FormLabTestReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            StoreOriginalColumnWidths();
            StoreOriginalItems();
            LoadCombo();
            ResetForm();
            CheckedTreeComboBoxLabTestName.Visible = false;
            toolStripSeparatorthree.Visible = true;
            LabelType.Visible = false;
            CheckedTreeComboBoxPatient.Visible = false;

            Cursor.Current = Cursors.Default;
        }
        private void StoreOriginalItems()
        {
            originalItems = new List<string>();
            foreach (string item in ComboBoxType.Items)
            {
                originalItems.Add(item);
            }
        }
        private void LoadCombo()
        {
            ComboUtils.InitializePatientTypeCombo(CheckedTreeComboBoxPatient, Global.Company.CompanyId);
            ComboUtils.InitializeLabTestCombo(CheckedTreeComboBoxLabTestName, Global.Company.CompanyId);
        }
        private void ResetForm()
        {
            LabTestRptErrMsg.Text = string.Empty;
            this.Text = "LabTest Report";
            GridViewByDateLabTestReport.Rows.Clear();
            AdjustColumnWidthsAfterHiding(GridViewByDateLabTestReport);
            CheckedTreeComboBoxLabTestName.SelectedNode = null!;
            foreach (ComboTreeNode comboTreeNode in CheckedTreeComboBoxLabTestName.Nodes)
            {
                comboTreeNode.Checked = false;
            }
            CheckedTreeComboBoxPatient.SelectedNode = null!;
            foreach (ComboTreeNode comboTreeNode in CheckedTreeComboBoxPatient.Nodes)
            {
                comboTreeNode.Checked = false;
            }
            EnableButtons(false);
            LabTestFromDate.Format = Global.Company.DateFormat;
            LabTestFromDate.Date = Global.getTransactionDate().AddDays(-30);
            LabTestToDate.Format = Global.Company.DateFormat;
            LabTestToDate.Date = Global.getTransactionDate();
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
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
        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewByDateLabTestReport.Rows.Clear();
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadLabTestReportHistory();
                }
            }
            catch (Exception ex)
            {
                LabTestRptErrMsg.Text = "Error fetching Lab Test (Error:" + ex.InnerException!.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private bool FormValidate()
        {
            LabTestRptErrMsg.Text = "";
            if (ComboBoxType.SelectedIndex < 0)
            {
                LabTestRptErrMsg.Text = string.Format(EnterValidTypeErrorMsg, "Type...");
                GetCurrentIndex = -1;
                ComboBoxType.Select();
                return false;
            }
            if (ComboBoxType.Text.Equals("By Patient", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxPatient).Count < 1)
            {
                LabTestRptErrMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                GetCurrentIndex = -1;
                CheckedTreeComboBoxPatient.Focus();
                return false;
            }
            if (ComboBoxType.Text.Equals("By Test Name", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxLabTestName).Count < 1)
            {
                LabTestRptErrMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                GetCurrentIndex = -1;
                CheckedTreeComboBoxLabTestName.Focus();
                return false;
            }
            if (LabTestFromDate.Date == null || !DateUtils.ValidDate(((DateTime)LabTestFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                LabTestRptErrMsg.Text = EnterValidDateErrorMsg;
                GetCurrentIndex = -1;
                LabTestFromDate.Focus();
                return false;
            }
            if (LabTestToDate.Date == null || !DateUtils.ValidDate(((DateTime)LabTestToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                LabTestRptErrMsg.Text = EnterValidDateErrorMsg;
                GetCurrentIndex = -1;
                LabTestToDate.Focus();
                return false;
            }
            if (LabTestFromDate.Date != null && LabTestToDate.Date != null && LabTestFromDate.Date > LabTestToDate.Date)
            {
                LabTestRptErrMsg.Text = CheckValidDateErrorMsg;
                GetCurrentIndex = -1;
                LabTestFromDate.Focus();
                return false;
            }
            return true;
        }
        public void LoadLabTestReportHistory()
        {
            LabTestRptErrMsg.Text = "";
            ReporptLabTestDetails = new RptLabTestDetails();
            ReporptLabTestDetails.FromDate = (DateTime)(LabTestFromDate.Date ?? DateTime.MinValue);
            ReporptLabTestDetails.ToDate = (DateTime)(LabTestToDate.Date ?? DateTime.MinValue);
            ReporptLabTestDetails.Company = Global.Company;
            ReporptLabTestDetails.FilterIndex = ReportIndex;
            ReporptLabTestDetails.PatientId = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxPatient).ToArray();
            ReporptLabTestDetails.LabTestId = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxLabTestName).ToArray();
            ReporptLabTestDetails.GenerateReport();

            int irow = 0;
            Color[] RowColor = new Color[4];
            RowColor[0] = Color.WhiteSmoke;
            RowColor[1] = Color.WhiteSmoke;
            RowColor[2] = Color.LightGray;
            RowColor[3] = Color.Gold;

            if (ReporptLabTestDetails.LineLabTestReport != null && ReporptLabTestDetails.LineLabTestReport.Count > 0)
            {
                EnableButtons(true);
                GridViewByDateLabTestReport.Rows.Clear();
                string prevDate = string.Empty;
                string prevPatientId = string.Empty;
                string prevTestName = string.Empty;
                string prevPatientName = string.Empty;
                string PatientNames = string.Empty;
                string TestNames = string.Empty;
                string currentSelectionName = string.Empty;

                var groupedItems0 = ReporptLabTestDetails.LineLabTestReport
                    .OrderBy(group => group.Date).ThenBy(group => group.Patient);
                
                var groupedItems1 = ReporptLabTestDetails.LineLabTestReport
                    .OrderBy(group => group.Patient).ThenBy(group => group.Date);

                var groupedItems2 = ReporptLabTestDetails.LineLabTestReport
                    .OrderBy(group => group.TestName).ThenBy(group => group.Date);

                var groupedItems = ReportIndex == 1 ? groupedItems1 : ReportIndex == 2 ? groupedItems2 : groupedItems0;

                var groupedItemsList = groupedItems.ToList();

                ReportCount = 0;
                foreach (var LineItem in groupedItems)
                {
                    string patientName = LineItem.Patient;
                    ReportCount++;
                    
                    if (ReportIndex == 1)
                    {
                        if (PatientNames != LineItem.PatientType)
                        {
                            irow = GridViewByDateLabTestReport.Rows.Add();
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.ROWHEADING].Value = "Patient Name : " + LineItem.PatientType;
                            PatientNames = LineItem.PatientType;
                        }
                    }
                    if (ReportIndex == 2)
                    {
                        if (TestNames != LineItem.TestType)
                        {
                            irow = GridViewByDateLabTestReport.Rows.Add();
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewByDateLabTestReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.ROWHEADING].Value = "Test Name : " + LineItem.TestType;
                            TestNames = LineItem.TestType;
                        }
                    }
                    string lineDate = LineItem.Date.ToString(ReporptLabTestDetails.Company.DateFormat);
                    if (ReportIndex == 1)
                    {
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Visible = false;
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Visible = true;
                    }
                    if (ReportIndex == 2)
                    {
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Visible = false;
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Visible = true;
                    }

                    irow = GridViewByDateLabTestReport.Rows.Add();
                    if (prevDate != lineDate)
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.DATE].Value = lineDate;
                        if (ReportIndex == 1 && (prevPatientName == LineItem.Patient && prevDate == lineDate))
                        {
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.DATE].Value = "";
                        }
                        if (ReportIndex == 2 && (prevTestName == LineItem.TestName && prevDate == lineDate))
                        {
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.DATE].Value = "";
                        }
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.DATE].Value = "";
                    }
                    if (prevPatientId != LineItem.PatientId || prevDate != lineDate)
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTID].Value = LineItem.PatientId;
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTNAME].Value = LineItem.Patient;
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTID].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTNAME].Value = "";
                        if (ReportIndex == 2 && prevTestName != LineItem.TestName)
                        {
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTID].Value = LineItem.PatientId;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.PATIENTNAME].Value = LineItem.Patient;
                        }
                    }
                    if (prevTestName != LineItem.TestName || prevDate != lineDate)
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.TESTNAME].Value = LineItem.TestName;
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.TESTNAME].Value = "";
                    }
                    prevDate = lineDate;
                    prevPatientId = LineItem.PatientId;
                    prevTestName = LineItem.TestName;
                    // Add element rows
                    if (LineItem.HasElemnet && LineItem.Elements.Count > 0)
                    {
                        int elementcount = 0;
                        foreach (var element in LineItem.Elements)
                        {
                            if (elementcount > 0)
                            {
                                irow = GridViewByDateLabTestReport.Rows.Add();
                            }
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.ELEMENT].Value = element.ElementName;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.UOM].Value = element.UOM;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.CLASS].Value = element.Class;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.SUBCLASS].Value = element.SubClass;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.SINGLEVALUE].Value = element.SingleValue;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RANGEFROM].Value = element.RangeFrom;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RANGETO].Value = element.RangeTo;
                            GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RESULT].Value = element.ResultDescription;
                            elementcount++;
                        }
                    }
                    else
                    {
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.CLASS].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.SUBCLASS].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.SINGLEVALUE].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RANGEFROM].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RANGETO].Value = "";
                        GridViewByDateLabTestReport.Rows[irow].Cells[(int)LabTestReportTableColumn.RESULT].Value = "";
                    }
                }
            }
            else
            {
                EnableButton(false);
                GetCurrentIndex = -1;
                LabTestRptErrMsg.Text = InformationMsg;
            }
        }

        // Function to calculate subtotal for a specific patient
        private double CalculateSubtotalForPatient(string ReportName, int Reporting)
        {
            double subtotal = 0.00;
            foreach (var item in ReporptLabTestDetails.LineLabTestReport)
            {
                if (Reporting == 1)
                {
                    if (item.Patient == ReportName)
                    {
                        subtotal += item.Fees;
                    }
                }
                else if (Reporting == 2)
                {
                    if (item.TestName == ReportName)
                    {
                        subtotal += item.Fees;
                    }
                }
            }
            return subtotal;
        }
        private void CheckedTreeComboBoxPatient_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void CheckedTreeComboBoxLabTestName_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
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
                else if (keyData == (Keys.Shift | Keys.Tab)) // Shift + Tab
                {
                    HandleShiftTabKey();
                    return true;
                }
                else if (keyData == Keys.Tab) // Tab
                {
                    ToolStripTabIndexChanged();
                    HandleTabKey();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleShiftTabKey()
        {
            // Handle Shift + Tab key press for reverse navigation
            if (this.ActiveControl == LabTestFromDate.Control)
            {
                if (GetComboIndex == 0)
                {
                    ComboBoxType.Focus();
                }
                else if (GetComboIndex == 1)
                {
                    CheckedTreeComboBoxPatient.Focus();
                }
                else if (GetComboIndex == 2)
                {
                    CheckedTreeComboBoxLabTestName.Focus();
                }
            }
            else if (this.ActiveControl == CheckedTreeComboBoxPatient.Control)
            {
                ComboBoxType.Focus();
            }
            else if (this.ActiveControl == CheckedTreeComboBoxLabTestName.Control)
            {
                ComboBoxType.Focus();
            }
            else if (this.ActiveControl == LabTestToDate.Control)
            {
                LabTestFromDate.Focus();
            }
            else if (ToolStripBtnGo.Selected == true)
            {
                ToolStripBtnGo.Checked = false;
                LabTestToDate.Focus();
            }
            else if (ToolStripBtnSave.Selected)
            {
                toolStripLabTestRpt.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (ToolStripBtnPrint.Selected)
            {
                toolStripLabTestRpt.Focus();
                ToolStripBtnSave.Select();
            }
            else if (this.ActiveControl == BtnSave)
            {
                toolStripLabTestRpt.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (this.ActiveControl == BtnPrint)
            {
                BtnSave.Focus();
            }
            else if (this.ActiveControl == BtnReset)
            {
                BtnPrint.Focus();
            }
            else if (this.ActiveControl == BtnExit)
            {
                BtnReset.Focus();
            }
        }

        private void HandleTabKey()
        {
            // Handle regular Tab key press
            if (this.ActiveControl == ComboBoxType.Control)
            {
                if (GetComboIndex == 0)
                {
                    ExecuteComboBoxSelection();
                    LabTestFromDate.Focus();
                }
                else if (GetComboIndex == 1)
                {
                    ExecuteComboBoxSelection();
                    CheckedTreeComboBoxPatient.Focus();
                }
                else if (GetComboIndex == 2)
                {
                    ExecuteComboBoxSelection();
                    CheckedTreeComboBoxLabTestName.Focus();
                }
            }
            else if (this.ActiveControl == CheckedTreeComboBoxPatient.Control || this.ActiveControl == CheckedTreeComboBoxLabTestName.Control)
            {
                LabTestFromDate.Focus();
            }
            else if (this.ActiveControl == LabTestFromDate.Control)
            {
                LabTestToDate.Focus();
            }
            else if (this.ActiveControl == LabTestToDate.Control)
            {
                toolStripLabTestRpt.Focus();
                ToolStripBtnGo.Select();
            }
            else if (ToolStripBtnGo.Selected)
            {
                if (ToolStripBtnSave.Enabled)
                {
                    BtnSave.Focus();
                }
                else
                {
                    BtnExit.Focus();
                }
            }
            else if (ToolStripBtnSave.Selected)
            {
                toolStripLabTestRpt.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (ToolStripBtnPrint.Selected)
            {
                BtnSave.Focus();
            }
            else if (this.ActiveControl == BtnSave)
            {
                BtnPrint.Focus();
            }
            else if (this.ActiveControl == BtnPrint)
            {
                BtnReset.Focus();
            }
            else if (this.ActiveControl == BtnReset)
            {
                BtnExit.Focus();
            }
            else if (this.ActiveControl == BtnExit)
            {
                toolStripLabTestRpt.Focus();
                ComboBoxType.Focus();
            }
        }

        private void GridViewByDateLabTestReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.PATIENTID].Value == null)
            {
                if (GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.PATIENTID].Value == null && GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.ELEMENT].Value == null && (ReportIndex == 1 || ReportIndex == 2))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MedicalLabTestReportSavePrint LTReportSavePrint = new MedicalLabTestReportSavePrint();
            LTReportSavePrint.ExportOrPrintToFile(GridViewByDateLabTestReport, ReporptLabTestDetails, "pdf", true, ReportIndex);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            MedicalLabTestReportSavePrint LTReportSavePrint = new MedicalLabTestReportSavePrint();
            LTReportSavePrint.ExportOrPrintToFile(GridViewByDateLabTestReport, ReporptLabTestDetails, "pdf", false, ReportIndex);
            Cursor.Current = Cursors.Default;
        }
        private void DisplayCheckedInformation()
        {

            string PatientNodes = GetCheckedNodes(CheckedTreeComboBoxPatient, "For All Patient");
            string LabTestNodes = GetCheckedNodes(CheckedTreeComboBoxLabTestName, "For All Lab Test");

            string DisplayFor = "";
            string title = "Lab Test Report";
            if (ReportIndex == 1)
            {
                if (PatientNodes != null && PatientNodes.ToString() != "For All Patient")
                {
                    DisplayFor = " For Patient ";
                }
            }
            if (ReportIndex == 2)
            {
                if (LabTestNodes != null && LabTestNodes.ToString() != "For All Lab Test")
                {
                    DisplayFor = " For Lab Test ";
                }
            }

            title += AppendToTitleIfNotEmpty(PatientNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(LabTestNodes!, DisplayFor);

            this.Text = title;
        }

        private string AppendToTitleIfNotEmpty(string nodes, string prefix)
        {
            return string.IsNullOrEmpty(nodes) ? "" : $"{prefix} {nodes}";
        }
        private string GetCheckedNodes(ToolstripCheckedTreeComboBox comboBox, string nodeName)
        {
            if (comboBox == null || comboBox.CheckedNodes == null || comboBox.CheckedNodes.Count == 0)
            {
                return string.Empty;
            }

            bool isAllSelected = comboBox.CheckedNodes.Any(node => node.Name == "All");

            if (isAllSelected)
            {
                return nodeName;
            }

            string checkedNodes = string.Join(", ", comboBox.CheckedNodes.Select(node => node.Text));
            return checkedNodes;
        }

        private void GridViewByDateLabTestReport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.DATE].Value == null) && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.GridViewByDateLabTestReport.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.GridViewByDateLabTestReport.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string? rr = GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.ROWHEADING].Value != null ? GridViewByDateLabTestReport.Rows[e.RowIndex].Cells[(int)LabTestReportTableColumn.ROWHEADING].Value?.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private Dictionary<int, int> originalColumnWidths = new Dictionary<int, int>();
        private void StoreOriginalColumnWidths()
        {
            originalColumnWidths.Clear(); // Clear previous stored widths
            foreach (DataGridViewColumn column in GridViewByDateLabTestReport.Columns)
            {
                originalColumnWidths[column.Index] = column.Width;
            }
        }
        private void InitializeColumnWidths(int indexedColumn)
        {
            if(indexedColumn == 1)
            {
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.DATE].Width = 97; 
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTID].Width = 120;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Width = 2;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Width = 250;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.ELEMENT].Width = 200;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.UOM].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.CLASS].Width = 90;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.SUBCLASS].Width = 70;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.SINGLEVALUE].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RANGEFROM].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RANGETO].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RESULT].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.ROWHEADING].Width = 2;
            }
            else if (indexedColumn == 2)
            {
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.DATE].Width = 97;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTID].Width = 120;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Width = 250;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Width = 2;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.ELEMENT].Width = 200;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.UOM].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.CLASS].Width = 90;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.SUBCLASS].Width = 70;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.SINGLEVALUE].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RANGEFROM].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RANGETO].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.RESULT].Width = 80;
                GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.ROWHEADING].Width = 2;
            }
        }

        private void ResetColumnWidths()
        {
            foreach (DataGridViewColumn column in GridViewByDateLabTestReport.Columns)
            {
                if (column.Visible)
                {
                    column.Width = 2;
                }
            }
        }
        private void ResetColumnWidthsOriginal()
        {
            foreach (var kvp in originalColumnWidths)
            {
                int originalcolumnIndex = kvp.Key;
                int originalWidth = kvp.Value;
                GridViewByDateLabTestReport.Columns[originalcolumnIndex].Width = originalWidth;
            }
        }

        private void ComboBoxType_DropDown(object sender, EventArgs e)
        {
            var toolStripComboBox = sender as ToolStripComboBox;
            if (toolStripComboBox == null || toolStripComboBox.Name != "ComboBoxType")
                return;

            allowSelectedIndexChanged = true;
            toolStripComboBox.Items.Clear();
            toolStripComboBox.Items.AddRange(originalItems!.ToArray());
        }

        private bool allowSelectedIndexChanged = false;
        private void ComboBoxType_KeyDown(object sender, KeyEventArgs e)
        {
            ToolStripComboBox? comboBox = sender as ToolStripComboBox;
            if (comboBox == null)
                return;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                allowSelectedIndexChanged = true;
                ExecuteComboBoxSelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                ToolStripTabIndexChanged();
                allowSelectedIndexChanged = false;
            }
        }

        public List<string> listNew = new List<string>();
        private void ComboBoxType_TextUpdate(object sender, EventArgs e)
        {
            if (this.ComboBoxType == null || this.ComboBoxType.IsDisposed)
                return;

            try
            {
                int cursorPosition = this.ComboBoxType.SelectionStart;

                this.ComboBoxType.Items.Clear();
                listNew.Clear();

                if (string.IsNullOrWhiteSpace(this.ComboBoxType.Text))
                    return;

                foreach (var item in originalItems!)
                {
                    if (item.ToLower().Contains(this.ComboBoxType.Text.ToLower()))
                    {
                        listNew.Add(item);
                    }
                }

                if (listNew.Count == 0)
                {
                    this.ComboBoxType.SelectionStart = this.ComboBoxType.Text.Length;
                    return;
                }

                this.ComboBoxType.Items.AddRange(listNew.ToArray());
                this.ComboBoxType.DroppedDown = true;
                if (!string.IsNullOrEmpty(this.ComboBoxType.Text))
                    this.ComboBoxType.SelectionStart = cursorPosition;

                Cursor = Cursors.Default;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
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
                    if (ComboBoxType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 0;
                        ComboBoxType.Text = "By Date";
                        GridViewByDateLabTestReport.Visible = true;
                        CheckedTreeComboBoxPatient.Visible = false;
                        CheckedTreeComboBoxLabTestName.Visible = false;
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Visible = true;
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Visible = true;
                        ResetColumnWidths();
                        ResetColumnWidthsOriginal();
                        LabelType.Visible = false;
                    }
                    else if (ComboBoxType.Text.Equals("By Patient", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 1;
                        GridViewByDateLabTestReport.Visible = true;
                        CheckedTreeComboBoxPatient.Visible = true;
                        CheckedTreeComboBoxPatient.Size = new Size(200, 25);
                        CheckedTreeComboBoxLabTestName.Visible = false;
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Visible = false;
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Visible = true;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Patient";
                    }
                    else if (ComboBoxType.Text.Equals("By Test Name", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 2;
                        GridViewByDateLabTestReport.Visible = true;
                        CheckedTreeComboBoxPatient.Visible = false;
                        CheckedTreeComboBoxLabTestName.Visible = true;
                        CheckedTreeComboBoxLabTestName.Size = new Size(200, 25);
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.PATIENTNAME].Visible = true;
                        GridViewByDateLabTestReport.Columns[(int)LabTestReportTableColumn.TESTNAME].Visible = false;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Medical Test";
                    }
                    ComboBoxType.DroppedDown = false;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ToolStripTabIndexChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ComboBoxType_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

        }

        private void ToolStripTabIndexChanged()
        {
            if (ComboBoxType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 0;
            }
            else if (ComboBoxType.Text.Equals("By Patient", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 1;
            }
            else if (ComboBoxType.Text.Equals("By Test Name", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 2;
            }
            else
            {
                GetComboIndex = 0;
            }
        }

        private void FormLabTestReport_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle clientRect = this.ClientRectangle;

            // Check if the mouse cursor is outside the client rectangle
            if (!clientRect.Contains(this.PointToClient(Cursor.Position)))
            {
                // Calculate the new cursor position to keep it inside the client rectangle
                int newX = Math.Max(clientRect.Left, Math.Min(clientRect.Right, Cursor.Position.X));
                int newY = Math.Max(clientRect.Top, Math.Min(clientRect.Bottom, Cursor.Position.Y));

                // Set the new cursor position
                Cursor.Position = new Point(newX, newY);
            }
        }
        private void SetComboIndex()
        {
            if (ComboBoxType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 0;
            }
            else if (ComboBoxType.Text.Equals("By Patient", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 1;
            }
            else if (ComboBoxType.Text.Equals("By Test Name", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 2;
            }
            else
            {
                GetCurrentIndex = -1;
            }
        }
        private void StoreComboItems()
        {
            ComboBoxType.Items.Clear();
            foreach (string item in originalItems!)
            {
                ComboBoxType.Items.Add(item);
            }
        }

        private void ComboBoxType_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxType.Items.Count == 0)
            {
                StoreComboItems();
            }
        }
    }
}
