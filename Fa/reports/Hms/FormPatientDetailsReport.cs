using fa;
using fa.api.utils;
using fa.libraries.utils;
using fa.views.utils;
using Fa.views.utils.Report.Hms;
using FADataAccessLibrary.report.Hms;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;

namespace Fa.reports.Hms
{
    enum PatientDetailsTableColumn
    {
        SNO, NAME, PATIENTID, GENDER, DOB, AGE, BLOODGROUP, PADDRESS, PHONE, GAURDIANDETAIL, EMERGENCYCONTACT, DATE
    }
    public partial class FormPatientDetailsReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string EnterValidTypeErrorMsg = "Please select Type";
        public static string ExceptionThrowErrorMsg = "Error fetching patient details";

        RptPatientDetails rptPatientDetails = null!;
        public FormPatientDetailsReport()
        {
            InitializeComponent();
        }

        private void FormPatientDetailsReport_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientDetailsErrMsg.Text = "";
            GridviewPatientDetails.Rows.Clear();
            ComboBoxType.Text = "By Date";

            PatientDetailsFromDate.Format = Global.Company.DateFormat;
            PatientDetailsFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PatientDetailsToDate.Format = Global.Company.DateFormat;
            PatientDetailsToDate.Date = Global.getTransactionDate();
            EnableButton(false);
            Cursor.Current = Cursors.Default;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && BtnReset.Enabled)
            {
                BtnReset.PerformClick();
                return true;
            }
            if (keyData == Keys.F8 && BtnSave.Enabled)
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == Keys.F9 && BtnPrint.Enabled)
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == Keys.F10 && BtnExit.Enabled)
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string lFromDate = ((DateTime)PatientDetailsFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PatientDetailsToDate.Date!).ToString(Global.Company.DateFormat);
            PatientDetailsReportPrintAndSave patientDetailsReportPrintAndSave = new PatientDetailsReportPrintAndSave();
            patientDetailsReportPrintAndSave.ExportOrPrintToFile(GridviewPatientDetails, rptPatientDetails, rptPatientDetails.ReportName(), "Patient Details Report", "pdf", false, lFromDate, lToDate);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string lFromDate = ((DateTime)PatientDetailsFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)PatientDetailsToDate.Date!).ToString(Global.Company.DateFormat);
            PatientDetailsReportPrintAndSave patientDetailsReportPrintAndSave = new PatientDetailsReportPrintAndSave();
            patientDetailsReportPrintAndSave.ExportOrPrintToFile(GridviewPatientDetails, rptPatientDetails, rptPatientDetails.ReportName(), "Patient Details Report", "pdf", true, lFromDate, lToDate);
            Cursor.Current = Cursors.Default;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PatientDetailsErrMsg.Text = "";
                GridviewPatientDetails.Rows.Clear();
                if (FormValidate())
                {
                    LoadPatientDetails();
                }
            }
            catch (Exception ex)
            {
                PatientDetailsErrMsg.Text = ExceptionThrowErrorMsg + " : " + ex.InnerException?.Message ?? ex.Message;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadPatientDetails()
        {
            rptPatientDetails = new RptPatientDetails();
            rptPatientDetails.FromDate = (DateTime)(PatientDetailsFromDate.Date ?? DateTime.MinValue);
            rptPatientDetails.ToDate = (DateTime)(PatientDetailsToDate.Date ?? DateTime.MinValue);
            rptPatientDetails.Company = Global.Company;
            rptPatientDetails.GenerateReport();
            GridviewPatientDetails.Rows.Clear();
            if (rptPatientDetails.PatientDetailsLineItems != null && rptPatientDetails.PatientDetailsLineItems.Count > 0)
            {
                EnableButton(true);
                int row = -1;
                int i = 1;
                string EntryDate = DateTime.Now.ToString();
                foreach (RptPatientDetailsLineItems lineItem in rptPatientDetails.PatientDetailsLineItems)
                {
                    if (EntryDate != lineItem.Date)
                    {
                        row = GridviewPatientDetails.Rows.Add();
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.DATE].Value = "Registration Date : " + lineItem.Date;
                        GridviewPatientDetails.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                        GridviewPatientDetails.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                        GridviewPatientDetails.Rows[row].DefaultCellStyle.SelectionForeColor = Color.Black;
                        EntryDate = lineItem.Date;
                        i = 1;
                    }
                    row = GridviewPatientDetails.Rows.Add();
                    if (lineItem.PatientName != null)
                    {
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.SNO].Value = i;
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.NAME].Value = lineItem?.PatientName ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.PATIENTID].Value = lineItem?.PatientID ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.GENDER].Value = lineItem?.Gender?.ToString() ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.DOB].Value = lineItem?.DOB != null ? lineItem?.DOB : "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.AGE].Value = lineItem?.Age ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.BLOODGROUP].Value = lineItem?.BloodGroup ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.PADDRESS].Value = lineItem?.PAddress ?? "";
                        GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.PHONE].Value = lineItem?.Mobile ?? "";
                        i++;
                    }
                    GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.GAURDIANDETAIL].Value = lineItem?.GAddress ?? "";
                    GridviewPatientDetails.Rows[row].Cells[(int)PatientDetailsTableColumn.EMERGENCYCONTACT].Value = lineItem?.EAddress ?? "";
                }
            }
            else
            {
                PatientDetailsErrMsg.Text = InformationMsg;
            }
        }

        private bool FormValidate()
        {
            EnableButton(false);
            PatientDetailsErrMsg.Text = "";
            if (ComboBoxType.Text.Trim() == string.Empty)
            {
                PatientDetailsErrMsg.Text = EnterValidTypeErrorMsg;
                ComboBoxType.Focus();
                return false;
            }
            if (PatientDetailsFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientDetailsFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientDetailsErrMsg.Text = EnterValidDateErrorMsg;
                PatientDetailsFromDate.Focus();
                return false;
            }
            if (PatientDetailsToDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientDetailsToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientDetailsErrMsg.Text = EnterValidDateErrorMsg;
                PatientDetailsToDate.Focus();
                return false;
            }
            if (PatientDetailsFromDate.Date != null && PatientDetailsToDate.Date != null && PatientDetailsFromDate.Date > PatientDetailsToDate.Date)
            {
                PatientDetailsErrMsg.Text = CheckValidDateErrorMsg;
                PatientDetailsFromDate.Focus();
                return false;
            }
            return true;
        }

        private void GridviewPatientDetails_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.PATIENTID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.GridviewPatientDetails.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
                this.GridviewPatientDetails.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.DATE].Value != null ? GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.DATE].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridviewPatientDetails_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.NAME].Value == null &&
                GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.GAURDIANDETAIL].Value == null && 
                GridviewPatientDetails.Rows[e.RowIndex].Cells[(int)PatientDetailsTableColumn.EMERGENCYCONTACT].Value == null)
            {
                if (e.ColumnIndex == (int)PatientDetailsTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)PatientDetailsTableColumn.EMERGENCYCONTACT)
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
    }
}
