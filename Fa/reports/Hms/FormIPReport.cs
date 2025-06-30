using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.report.Ip;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.views.utils.Report.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.reports.Inventory;
using fa.views.controls.ComboTreeView;
using Syncfusion.Pdf.Parsing;
using fa.report.Inventory;
using NPOI.SS.UserModel;
using Fa.reports.Inventory;
using VisioForge.MediaFramework.FFMPEGCore.Instance;
using static fa.views.utils.Common.DataGridViewColoumnAdjustment;
using Fa.reports.Hms;
using fa.views.controls;


namespace fa.reports.Hms
{
    enum IpAllReportTableColumn
    {
        WARD, BED, PID, PNAM, PDOB, PAGE, PADMT, PDOCT, PNURS, PDISC, ROWHEADING
    }
    public partial class FormIPReport : Form
    {
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string SelectWardErrorMsg = "Please select ward.";
        public static string SelectStatusErrorMsg = "Please select status.";
        public static string SelectOrderErrorMsg = "Please select which order you want to display inpatient deatails.";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public static string TypeSelectionErrorMsg = "Please select the Type";
        public static string TypeValidationErrorMsg = "Please select the valid Type";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public int ReportIndex = -1;
        IpReport IpReport = null!;
        WardManager WardManager = null!;
        public FormIPReport()
        {
            WardManager = WardManager.Instance;
            InitializeComponent();
        }
        private void FormIPReport_Load(object sender, EventArgs e)
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
            IpReportErrorMsg.Text = "";
            this.Text = "In Patient Report";
            IpReportDataGridView.Rows.Clear();
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
            ComboIpReportStatus.SelectedIndex = 0;
            ComboIpReportOrder.SelectedIndex = 0;
            IpReportFromDate.Format = Global.Company.DateFormat;
            IpReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            IpReportToDate.Format = Global.Company.DateFormat;
            IpReportToDate.Date = Global.getTransactionDate();
            EnableButton(false);
            ReportIndex = 0;
        }


        private void AdjustColumnOrder()
        {
            if (ComboIpReportOrder.SelectedIndex == 0)
            {
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.WARD].DisplayIndex = 0;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PADMT].DisplayIndex = 6;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PDISC].DisplayIndex = 9;
            }
            else if (ComboIpReportOrder.SelectedIndex == 1)
            {
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.WARD].DisplayIndex = 6;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PADMT].DisplayIndex = 0;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PDISC].DisplayIndex = 9;

            }
            else if (ComboIpReportOrder.SelectedIndex == 2)
            {
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.WARD].DisplayIndex = 9;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PADMT].DisplayIndex = 6;
                IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PDISC].DisplayIndex = 0;

            }
        }
        private void LoadCompleteInPatientReport()
        {
            IpReportErrorMsg.Text = "";
            IpReport = new IpReport();

            IpReport.FromDate = (DateTime)IpReportFromDate.Date!;
            IpReport.ToDate = (DateTime)IpReportToDate.Date!;
            IpReport.Company = Global.Company;
            IpReport.FilterIndex = ReportIndex;

            IpReport.WardId = CheckedTreeUtils.SelectedNodes(ComboBoxWard).ToArray();
            IpReport.ConsultantId = CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).ToArray();
            IpReport.InsuranceId = CheckedTreeUtils.SelectedNodes(ComboBoxInsurance).ToArray();
            IpReport.DepartmentId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();

            IpReport.FltrStatus = ComboIpReportStatus.SelectedIndex;
            IpReport.OrdrStatus = ComboIpReportOrder.SelectedIndex;
            IpReport.GenerateReport();
            if (IpReport.LinePatients != null && IpReport.LinePatients.Count > 0)
            {
                int j = 2;
                int k = 0;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                EnableButton(true);
                IpReportDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                int rowCount = 0;
                AdjustColumnOrder();
                string Consultant = string.Empty;
                string DepartmentOfConsutant = string.Empty;
                string Insurance = string.Empty;
                string WardName = string.Empty;

                foreach (InPatientReport LineItem in IpReport.LinePatients)
                {
                    int irow = rowCount;

                    if (ReportIndex == 1)
                    {
                        if (Consultant != LineItem.PrimaryDr)
                        {
                            irow = IpReportDataGridView.Rows.Add();
                            k = 0;
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            if (ComboIpReportOrder.SelectedIndex == 0)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Consultant : " + LineItem.PrimaryDr;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 1)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Consultant : " + LineItem.PrimaryDr;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 2)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Consultant : " + LineItem.PrimaryDr;
                            }
                            Consultant = LineItem.PrimaryDr;
                        }
                    }

                    if (ReportIndex == 2)
                    {
                        if (DepartmentOfConsutant != LineItem.DepartmenOfConsutant)
                        {
                            k = 0;
                            irow = IpReportDataGridView.Rows.Add();
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            if (ComboIpReportOrder.SelectedIndex == 0)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Department : " + LineItem.DepartmenOfConsutant;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 1)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Department : " + LineItem.DepartmenOfConsutant;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 2)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Department : " + LineItem.DepartmenOfConsutant;
                            }
                            DepartmentOfConsutant = LineItem.DepartmenOfConsutant;
                        }
                    }

                    if (ReportIndex == 3)
                    {
                        if (Insurance != LineItem.InsuranceComp)
                        {
                            k = 0;
                            irow = IpReportDataGridView.Rows.Add();
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            if (ComboIpReportOrder.SelectedIndex == 0)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Insurance : " + LineItem.InsuranceComp;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 1)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Insurance : " + LineItem.InsuranceComp;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 2)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Insurance : " + LineItem.InsuranceComp;
                            }
                            Insurance = LineItem.InsuranceComp;
                        }
                    }
                    if (ReportIndex == 4)
                    {
                        if (WardName != LineItem.WardName)
                        {
                            k = 0;
                            irow = IpReportDataGridView.Rows.Add();
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            if (ComboIpReportOrder.SelectedIndex == 0)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Ward : " + LineItem.WardName;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 1)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Ward : " + LineItem.WardName;
                            }
                            else if (ComboIpReportOrder.SelectedIndex == 2)
                            {
                                IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value = "Ward : " + LineItem.WardName;
                            }
                            WardName = LineItem.WardName;
                        }
                    }
                    irow = IpReportDataGridView.Rows.Add();
                    if (ReportIndex == 4)
                    {
                        IpReportDataGridView.Columns[(int)IpAllReportTableColumn.WARD].Visible = false;
                    }
                    else
                    {
                        IpReportDataGridView.Columns[(int)IpAllReportTableColumn.WARD].Visible = true;

                    }
                    if (ReportIndex == 1)
                    {
                        IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PDOCT].Visible = false;
                    }
                    else
                    {
                        IpReportDataGridView.Columns[(int)IpAllReportTableColumn.PDOCT].Visible = true;

                    }
                    k = 0;
                    IpReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                    IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    IpReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    j++;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.WARD].Value = LineItem.WardName;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.BED].Value = LineItem.BedName;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PID].Value = LineItem.PatientId;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PNAM].Value = LineItem.PatientName + (string.IsNullOrEmpty(LineItem.Address) ? "" : Environment.NewLine + LineItem.Address);
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PDOB].Value = LineItem.DOB.ToString(Global.Company.DateFormat);
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PAGE].Value = LineItem.Age;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PADMT].Value = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PDOCT].Value = LineItem.PrimaryDr;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PNURS].Value = LineItem.PrimaryCT;
                    IpReportDataGridView.Rows[irow].Cells[(int)IpAllReportTableColumn.PDISC].Value = LineItem.DischargedOn.Year < 1900 ? string.Empty : LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                    rowCount++;
                    k++;
                }
                AdjustColumnWidthsAfterHiding(IpReportDataGridView);
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
                IpReportErrorMsg.Text = "No Information Found..!";
            }
        }
        private bool ValidateForm()
        {
            if (ComboBoxReportType.Text == string.Empty || ReportIndex == -1)
            {
                IpReportErrorMsg.Text = TypeSelectionErrorMsg;
                ComboBoxReportType.Focus();
                return false;
            }
            if (ComboBoxReportType.Text != "By Date" && ComboBoxReportType.Text != "By Consultant" && ComboBoxReportType.Text != "By Department" && ComboBoxReportType.Text != "By Insurance" && ComboBoxReportType.Text != "By Ward")
            {
                IpReportErrorMsg.Text = TypeValidationErrorMsg;
                ComboBoxReportType.Focus();
                return false;
            }
            if (IpReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)IpReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                IpReportErrorMsg.Text = EnterValidDateErrorMsg;
                IpReportFromDate.Focus();
                return false;
            }
            if (IpReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)IpReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                IpReportErrorMsg.Text = EnterValidDateErrorMsg;
                IpReportToDate.Focus();
                return false;
            }
            if (IpReportFromDate.Date != null && IpReportToDate.Date != null && IpReportFromDate.Date > IpReportToDate.Date)
            {
                IpReportErrorMsg.Text = CheckValidDateErrorMsg;
                IpReportFromDate.Focus();
                return false;
            }
            if (ReportIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).Count < 1)
                {
                    IpReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxConsultant.Focus();
                    return false;
                }
            }
            if (ReportIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
                {
                    IpReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxDepartment.Focus();
                    return false;
                }
            }
            if (ReportIndex == 3)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxInsurance).Count < 1)
                {
                    IpReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxInsurance.Focus();
                    return false;
                }
            }
            if (ReportIndex == 4)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxWard).Count < 1)
                {
                    IpReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxWard.Focus();
                    return false;
                }
            }
            if (ComboIpReportStatus.SelectedIndex < 0)
            {
                IpReportErrorMsg.Text = SelectStatusErrorMsg;
                ComboIpReportStatus.Focus();
                return false;
            }
            if (ComboIpReportOrder.SelectedIndex < 0)
            {
                IpReportErrorMsg.Text = SelectOrderErrorMsg;
                ComboIpReportOrder.Focus();
                return false;
            }
            return true;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            InPatientReportSavePrint IpReportSavePrint = new InPatientReportSavePrint();
            IpReportSavePrint.ExportOrPrintToFile(IpReportDataGridView, IpReport, "pdf", false, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            InPatientReportSavePrint IpReportSavePrint = new InPatientReportSavePrint();
            IpReportSavePrint.ExportOrPrintToFile(IpReportDataGridView, IpReport, "pdf", true, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
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

        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            IpReportDataGridView.Rows.Clear();
            EnableButton(false);
            if (ValidateForm())
            {
                LoadCompleteInPatientReport();
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
                IpReportDataGridView.Visible = true;
                ComboBoxConsultant.Visible = false;
                ComboBoxDepartment.Visible = false;
                ComboBoxInsurance.Visible = false;
                ComboBoxWard.Visible = false;

                LabelType.Visible = false;

            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ReportIndex = 1;
                IpReportDataGridView.Visible = true;
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
                IpReportDataGridView.Visible = true;
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
                IpReportDataGridView.Visible = true;
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
                IpReportDataGridView.Visible = true;
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

        private void IpReportDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && IpReportDataGridView.Rows[e.RowIndex].Cells[(int)IpAllReportTableColumn.PID].Value == null)
            {
                if (e.ColumnIndex == (int)IpAllReportTableColumn.WARD)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)IpAllReportTableColumn.PDISC)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)IpAllReportTableColumn.PADMT)
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

        private void IpReportDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((IpReportDataGridView.Rows[e.RowIndex].Cells[(int)IpAllReportTableColumn.WARD].Value == null || IpReportDataGridView.Rows[e.RowIndex].Cells[(int)IpAllReportTableColumn.BED].Value == null) && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.IpReportDataGridView.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.IpReportDataGridView.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string? rr = IpReportDataGridView.Rows[e.RowIndex].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value != null ? IpReportDataGridView.Rows[e.RowIndex].Cells[(int)IpAllReportTableColumn.ROWHEADING].Value?.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void IpReportDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == (int)IpAllReportTableColumn.WARD || e.ColumnIndex == (int)IpAllReportTableColumn.BED || e.ColumnIndex == (int)IpAllReportTableColumn.PDISC || e.ColumnIndex == (int)IpAllReportTableColumn.PADMT) && e.Value != null)
            {
                DataGridViewCell cell = IpReportDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.WrapMode = DataGridViewTriState.False;
                //AdjustColumnWidth(IpReportDataGridView, e.ColumnIndex);
            }
        }
        private void AdjustColumnWidth(DataGridView dataGridView, int columnIndex)
        {
            dataGridView.AutoResizeColumn(columnIndex, DataGridViewAutoSizeColumnMode.AllCells);
            DataGridViewColumn column = dataGridView.Columns[columnIndex];
            int width = column.Width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Width = width;
        }
        private void DisplayCheckedInformation()
        {

            string ConsultantNodes = GetCheckedNodes(ComboBoxConsultant, "For All Consultant");
            string DepartmentNodes = GetCheckedNodes(ComboBoxDepartment, "For All Department");
            string InsuranceNodes = GetCheckedNodes(ComboBoxInsurance, "For All Insurance");
            string WardNodes = GetCheckedNodes(ComboBoxWard, "For All Ward");

            string DisplayFor = "";
            string title = "In Patient Report";
            if (ReportIndex == 1)
            {
                if (ConsultantNodes != null && ConsultantNodes.ToString() != "For All Consultant")
                {
                    DisplayFor = " For Consultant ";
                }
            }
            if (ReportIndex == 2)
            {
                if (DepartmentNodes != null && DepartmentNodes.ToString() != "For All Department")
                {
                    DisplayFor = " For Department ";
                }
            }
            if (ReportIndex == 3)
            {
                if (InsuranceNodes != null && InsuranceNodes.ToString() != "For All Insurance")
                {
                    DisplayFor = " For Insurance ";
                }
            }
            if (ReportIndex == 4)
            {
                if (WardNodes != null && WardNodes.ToString() != "For All Ward")
                {
                    DisplayFor = " For Ward ";
                }
            }

            title += AppendToTitleIfNotEmpty(ConsultantNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(DepartmentNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(InsuranceNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(WardNodes!, DisplayFor);

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

        private void ComboBoxWard_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxDepartment_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxInsurance_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxConsultant_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
    }
}