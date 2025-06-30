using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Hms.Master;
using fa.report.Ip;
using fa.reports.Hms;
using fa.views.controls.ComboTreeView;
using fa.views.hms;
using fa.views.utils.Report.Hms;
using Fa.views.utils.Report.Hms;
using FADataAccessLibrary.report.Ip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.MediaFramework.FFMPEGCore.Instance;
using static fa.views.utils.Common.DataGridViewColoumnAdjustment;

namespace Fa.reports.Hms
{
    enum DischargeReportTableColumn
    {
        WARD, BED, PID, PNAM, PDOB, PAGE, PADMT, PDOCT, PNURS, PDISC, SHEAD
    }
    public partial class FromDischargePatientDetailReport : FormPatientBase
    {
        public static string EnterValidFromDateErrorMsg = "Please enter valid from date.";
        public static string EnterValidToDateErrorMsg = "Please enter valid to date.";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public static string NoInfoFountErrorMsg = "No Information Found..!";
        public static string TypeSelectionErrorMsg = "Please select the Type";
        public static string TypeValidationErrorMsg = "Please select the valid Type";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public int ReportIndex = -1;
        DischargeReport DischargeReport = null!;
        WardManager WardManager = null!;
        public FromDischargePatientDetailReport()
        {
            WardManager = WardManager.Instance;
            InitializeComponent();
        }

        private void FromDischargePatientDetailReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadComboBox();
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeDoctorCombo(ComboBoxConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeAllDepartmentCombo(ComboBoxDepartment, Global.Company.CompanyId);
            ComboUtils.InitializeAllInsuranceCombo(ComboBoxInsurance, Global.Company.CompanyId);
            ComboUtils.InitializeAllWardCombo(ComboBoxWard, Global.Company.CompanyId);
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
            this.Text = "Discharge Patients Detail Report ";
            DischargeReportErrorMsg.Text = "";
            DischargeReportDataGridView.Rows.Clear();
            foreach (ComboTreeNode ComboTreeNode in ComboBoxConsultant.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxDepartment.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxInsurance.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxWard.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            DischargeReportFromDate.Format = Global.Company.DateFormat;
            DischargeReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            DischargeReportToDate.Format = Global.Company.DateFormat;
            DischargeReportToDate.Date = Global.getTransactionDate();
            EnableButton(false);
            ReportIndex = 0;
        }
        private bool ValidateForm()
        {
            if (ComboBoxReportType.Text == string.Empty || ReportIndex == -1)
            {
                DischargeReportErrorMsg.Text = TypeSelectionErrorMsg;
                ComboBoxReportType.Focus();
                return false;
            }
            if (ComboBoxReportType.Text != "By Date" && ComboBoxReportType.Text != "By Consultant" && ComboBoxReportType.Text != "By Department" && ComboBoxReportType.Text != "By Insurance" && ComboBoxReportType.Text != "By Ward")
            {
                DischargeReportErrorMsg.Text = TypeValidationErrorMsg;
                ComboBoxReportType.Focus();
                return false;
            }
            if (DischargeReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)DischargeReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DischargeReportErrorMsg.Text = EnterValidFromDateErrorMsg;
                DischargeReportFromDate.Focus();
                return false;
            }
            if (DischargeReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)DischargeReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DischargeReportErrorMsg.Text = EnterValidToDateErrorMsg;
                DischargeReportToDate.Focus();
                return false;
            }
            if (ReportIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).Count < 1)
                {
                    DischargeReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxConsultant.Focus();
                    return false;
                }
            }
            if (ReportIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
                {
                    DischargeReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxDepartment.Focus();
                    return false;
                }
            }
            if (ReportIndex == 3)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxInsurance).Count < 1)
                {
                    DischargeReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxInsurance.Focus();
                    return false;
                }
            }
            if (ReportIndex == 4)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxWard).Count < 1)
                {
                    DischargeReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxWard.Focus();
                    return false;
                }
            }
            if (DischargeReportFromDate.Date != null && DischargeReportToDate.Date != null && DischargeReportFromDate.Date > DischargeReportToDate.Date)
            {
                DischargeReportErrorMsg.Text = CheckValidDateErrorMsg;
                DischargeReportFromDate.Focus();
                return false;
            }
            return true;
        }

        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            DischargeReportDataGridView.Rows.Clear();
            EnableButton(false);
            if (ValidateForm())
            {
                LoadCompleteDischargePatientReport();
            }
        }
        private void LoadCompleteDischargePatientReport()
        {
            DischargeReportErrorMsg.Text = "";
            DischargeReport = new DischargeReport();

            DischargeReport.FromDate = (DateTime)DischargeReportFromDate.Date!;
            DischargeReport.ToDate = (DateTime)DischargeReportToDate.Date!;
            DischargeReport.Company = Global.Company;
            DischargeReport.FilterIndex = ReportIndex;

            DischargeReport.WardId = CheckedTreeUtils.SelectedNodes(ComboBoxWard).ToArray();
            DischargeReport.ConsultantId = CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).ToArray();
            DischargeReport.InsuranceId = CheckedTreeUtils.SelectedNodes(ComboBoxInsurance).ToArray();
            DischargeReport.DepartmentId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();

            DischargeReport.GenerateReport();
            if (DischargeReport.DischargePatientReport != null && DischargeReport.DischargePatientReport.Count > 0)
            {
                int j = 2;
                int k = 0;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                EnableButton(true);
                int rowCount = 0;
                string Consultant = string.Empty;
                string DepartmentOfConsutant = string.Empty;
                string Insurance = string.Empty;
                string WardName = string.Empty;

                foreach (DischargePatientReport LineItem in DischargeReport.DischargePatientReport.OrderBy(x => x.WardName))
                {
                    int irow = rowCount;
                    if (ReportIndex == 1)
                    {
                        if (Consultant != LineItem.PrimaryDr)
                        {
                            irow = DischargeReportDataGridView.Rows.Add();
                            k = 0;
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.SHEAD].Value = "Consultant : " + LineItem.PrimaryDr;
                            Consultant = LineItem.PrimaryDr;
                        }
                    }
                    if (ReportIndex == 2)
                    {
                        if (DepartmentOfConsutant != LineItem.DepartmenOfConsutant)
                        {
                            k = 0;
                            irow = DischargeReportDataGridView.Rows.Add();
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.SHEAD].Value = "Department : " + LineItem.DepartmenOfConsutant;
                            DepartmentOfConsutant = LineItem.DepartmenOfConsutant;
                        }
                    }
                    if (ReportIndex == 3)
                    {
                        if (Insurance != LineItem.InsuranceComp)
                        {
                            k = 0;
                            irow = DischargeReportDataGridView.Rows.Add();
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.SHEAD].Value = "Insurance : " + LineItem.InsuranceComp;
                            Insurance = LineItem.InsuranceComp;
                        }
                    }
                    if (ReportIndex == 4)
                    {
                        if (WardName != LineItem.WardName)
                        {
                            k = 0;
                            irow = DischargeReportDataGridView.Rows.Add();
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.SHEAD].Value = "Ward : " + LineItem.WardName;
                            WardName = LineItem.WardName;
                        }
                    }
                    irow = DischargeReportDataGridView.Rows.Add();
                    if (ReportIndex == 4)
                    {
                        DischargeReportDataGridView.Columns[(int)DischargeReportTableColumn.WARD].Visible = false;
                    }
                    else
                    {
                        DischargeReportDataGridView.Columns[(int)DischargeReportTableColumn.WARD].Visible = true;
                    }
                    if (ReportIndex == 1)
                    {
                        DischargeReportDataGridView.Columns[(int)DischargeReportTableColumn.PDOCT].Visible = false;
                    }
                    else
                    {
                        DischargeReportDataGridView.Columns[(int)DischargeReportTableColumn.PDOCT].Visible = true;
                    }
                    k = 0;
                    DischargeReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                    DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    DischargeReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    j++;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.WARD].Value = LineItem.WardName;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.BED].Value = LineItem.BedName;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PID].Value = LineItem.PatientId;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PNAM].Value = LineItem.PatientName + (string.IsNullOrEmpty(LineItem.Address) ? "" : Environment.NewLine + LineItem.Address);
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PDOB].Value = LineItem.DOB.ToString(Global.Company.DateFormat);
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PAGE].Value = LineItem.Age;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PADMT].Value = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PDOCT].Value = LineItem.PrimaryDr;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PNURS].Value = LineItem.PrimaryCT;
                    DischargeReportDataGridView.Rows[irow].Cells[(int)DischargeReportTableColumn.PDISC].Value = LineItem.DischargedOn.Year < 1900 ? string.Empty : LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                    rowCount++;
                    k++;
                }
                AdjustColumnWidthsAfterHiding(DischargeReportDataGridView);
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
                DischargeReportErrorMsg.Text = NoInfoFountErrorMsg;
            }
        }
        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ReportIndex = 0;
                ComboBoxReportType.Text = "By Date";
                DischargeReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = false;
                ComboBoxDepartment.Visible = false;
                ComboBoxInsurance.Visible = false;
                ComboBoxWard.Visible = false;
                LabelType.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ReportIndex = 1;
                DischargeReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = true;
                ComboBoxConsultant.Size = new Size(200, 25);
                ComboBoxDepartment.Visible = false;
                ComboBoxInsurance.Visible = false;
                ComboBoxWard.Visible = false;
                LabelType.Visible = true;
                LabelType.Text = "Consultant";
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ReportIndex = 2;
                DischargeReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = false;
                ComboBoxDepartment.Visible = true;
                ComboBoxDepartment.Size = new Size(200, 25);
                ComboBoxInsurance.Visible = false;
                ComboBoxWard.Visible = false;
                LabelType.Visible = true;
                LabelType.Text = "Department";
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                ReportIndex = 3;
                DischargeReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = false;
                ComboBoxDepartment.Visible = false;
                ComboBoxInsurance.Visible = true;
                ComboBoxWard.Visible = false;
                ComboBoxInsurance.Size = new Size(200, 25);
                LabelType.Visible = true;
                LabelType.Text = "Insurance";
            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                ReportIndex = 4;
                DischargeReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = false;
                ComboBoxDepartment.Visible = false;
                ComboBoxInsurance.Visible = false;
                ComboBoxWard.Visible = true;
                ComboBoxWard.Size = new Size(200, 25);
                LabelType.Visible = true;
                LabelType.Text = "Ward";
            }
            Cursor.Current = Cursors.Default;
        }

        private void DischargeReportDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && DischargeReportDataGridView.Rows[e.RowIndex].Cells[(int)DischargeReportTableColumn.PID].Value == null)
            {
                if (e.ColumnIndex == (int)DischargeReportTableColumn.WARD)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)DischargeReportTableColumn.PDISC)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)DischargeReportTableColumn.PADMT)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void DischargeReportDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (DischargeReportDataGridView.Rows[e.RowIndex].Cells[(int)DischargeReportTableColumn.PID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.DischargeReportDataGridView.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.DischargeReportDataGridView.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = DischargeReportDataGridView.Rows[e.RowIndex].Cells[(int)DischargeReportTableColumn.SHEAD].Value?.ToString() ?? string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void DischargeReportDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == (int)DischargeReportTableColumn.WARD && e.Value != null || e.ColumnIndex == (int)DischargeReportTableColumn.BED && e.Value != null || e.ColumnIndex == (int)DischargeReportTableColumn.PDISC && e.Value != null || e.ColumnIndex == (int)DischargeReportTableColumn.PADMT && e.Value != null)
            {
                DataGridViewCell cell = DischargeReportDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.WrapMode = DataGridViewTriState.True;
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DischargePatientReportSavePrint DischargePatientReportSavePrint = new DischargePatientReportSavePrint();
            DischargePatientReportSavePrint.ExportOrPrintToFile(DischargeReportDataGridView, DischargeReport, "pdf", false, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DischargePatientReportSavePrint DischargePatientReportSavePrint = new DischargePatientReportSavePrint();
            DischargePatientReportSavePrint.ExportOrPrintToFile(DischargeReportDataGridView, DischargeReport, "pdf", true, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxWard.CheckedNodes != null && ComboBoxWard.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxWard.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Wards"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Discharge Patients Detail Report" + " @ " + CheckedNodes;
            }
            else if (ComboBoxDepartment.CheckedNodes != null && ComboBoxDepartment.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxDepartment.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Departments"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Discharge Patients Detail Report" + " @ " + CheckedNodes;
            }
            else if (ComboBoxConsultant.CheckedNodes != null && ComboBoxConsultant.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxConsultant.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Consultants"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Discharge Patients Detail Report" + " @ " + CheckedNodes;
            }
            else if (ComboBoxInsurance.CheckedNodes != null && ComboBoxInsurance.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxInsurance.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Insurances"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Discharge Patients Detail Report" + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Discharge Patients Detail Report ";
            }
        }
        private void ComboBoxWard_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ComboBoxDepartment_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ComboBoxConsultant_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ComboBoxInsurance_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
    }
}
