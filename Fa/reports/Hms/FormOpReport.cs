using fa.api.utils;
using fa.report.Hms;
using fa.views.controls.grid;
using fa.views.utils.Report.Hms;
using Fa.reports.sales;
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
using static fa.report.Hms.RptOPLineItem;

namespace fa.reports.Hms
{
    enum OpReportGridColumn
    {
        SNO, DATE, PATIENT,PATIENT_AGE,PATIENT_GENDER, PATIENTNO, TOKENNO, CONSULTANT, FEE
    }
    public partial class FormOpReport : Form
    {
        RptOpRegister db = null!;
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public FormOpReport()
        {
            InitializeComponent();
        }

        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridviewOpReport.Rows.Clear();
                EnableButton(false);
                if (ValidateForm())
                {
                    RunReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = "Errod fetching OP (Error:" + ex.InnerException!.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool ValidateForm()
        {
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex < 0)
            {
                ErrorMsg.Text = "Please select patient type...";
                ComboBoxTypeSelection.Focus();
                return false;
            }
            return true;
        }
        private void RunReport()
        {
            db = new RptOpRegister();
            db.Company = Global.Company;
            db.FromDate = (DateTime)FromDate.Date!;
            db.ToDate = (DateTime)ToDate.Date!;
            db.Type = ComboBoxTypeSelection.SelectedIndex == 0 ? OpPatientTypeSelection.BYALLPATIENT : ComboBoxTypeSelection.SelectedIndex == 1 ? OpPatientTypeSelection.BYNEWPATIENT : OpPatientTypeSelection.BYREPATEDPATIENT;
            db.GenerateReport();
            LoadData(db);
            //EnableButton(true);
        }
        private void LoadData(RptOpRegister OpRegister)
        {
            int i = 1;
            String dateTime = null!;
            ErrorMsg.Text = "";
            GridviewOpReport.Rows.Clear();
            GridviewOpReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            if (db.Type == OpPatientTypeSelection.BYALLPATIENT)
            {
                List<RptOPLineItem> LineItems = (List<RptOPLineItem>)OpRegister.LineItems;
                if (LineItems != null && LineItems.Count > 0)
                {
                    double Total = 0;
                    foreach (RptOPLineItem LineItem in LineItems)
                    {
                        int NewRow = 0;
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        NewRow = GridviewOpReport.Rows.Add();
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.SNO].Value = i;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }

                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT].Value = LineItem.Patientdetail;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT_AGE].Value = LineItem.Age;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT_GENDER].Value = LineItem.Gender;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENTNO].Value = LineItem.PatientNo;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.TOKENNO].Value = LineItem.TokenNo;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.CONSULTANT].Value = LineItem.ConsultantDetail;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.FEE].Value = LineItem.Fee;
                        Total += LineItem.Fee;
                        i++;
                    }
                    EnableButton(true);
                    int lastRowIndex = GridviewOpReport.Rows.Add();
                    GridviewOpReport.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridviewOpReport.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridviewOpReport.Rows[lastRowIndex].Cells[(int)OpReportGridColumn.CONSULTANT].Value = "Total";
                    GridviewOpReport.Rows[lastRowIndex].Cells[(int)OpReportGridColumn.FEE].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    EnableButton(false);
                    ErrorMsg.Text = "No Entry Found.";
                }
            }
            else if (db.Type == OpPatientTypeSelection.BYNEWPATIENT || db.Type == OpPatientTypeSelection.BYREPATEDPATIENT)
            {
                if (db.PatientOpRepoertLineItems != null && db.PatientOpRepoertLineItems.Count > 0)
                {
                    double Total = 0;
                    foreach (PatientOpRepoertLineItem LineItem in db.PatientOpRepoertLineItems.OrderBy(x => x.Date))
                    {
                        int NewRow = 0;
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        NewRow = GridviewOpReport.Rows.Add();
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.SNO].Value = i;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }

                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT].Value = LineItem.Patientdetail;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT_AGE].Value = LineItem.Age;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENT_GENDER].Value = LineItem.Gender;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.PATIENTNO].Value = LineItem.PatientNo;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.TOKENNO].Value = LineItem.TokenNo;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.CONSULTANT].Value = LineItem.ConsultantDetail;
                        GridviewOpReport.Rows[NewRow].Cells[(int)OpReportGridColumn.FEE].Value = LineItem.Fee;
                        Total += LineItem.Fee;
                        i++;
                    }
                    EnableButton(true);
                    int lastRowIndex = GridviewOpReport.Rows.Add();
                    GridviewOpReport.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridviewOpReport.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridviewOpReport.Rows[lastRowIndex].Cells[(int)OpReportGridColumn.CONSULTANT].Value = "Total";
                    GridviewOpReport.Rows[lastRowIndex].Cells[(int)OpReportGridColumn.FEE].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    EnableButton(false);
                    ErrorMsg.Text = "No Entry Found.";
                }
            }
        }
        private void FormOpReport_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                //RunReport();               
                FromDate.Select();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ResetForm()
        {
            GridviewOpReport.Rows.Clear();
            ErrorMsg.Text = "";
            EnableButton(false);
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
            ComboBoxTypeSelection.SelectedIndex = -1;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridviewOpReport.Columns["Fee"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            EnableButton(false);
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            FromDate.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OpPrint OpPrint = new OpPrint();
            OpPrint.ExportToFileOrPrint(db, true);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            OpPrint OpPrint = new OpPrint();
            OpPrint.ExportToFileOrPrint(db, false);
            Cursor.Current = Cursors.Default;
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

        private void GridviewOpReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewOpReport.Rows[e.RowIndex].Cells[(int)OpReportGridColumn.PATIENTNO].Value == null
                && GridviewOpReport.Rows[e.RowIndex].Cells[(int)OpReportGridColumn.FEE].Value != null)
            {
                if (e.ColumnIndex == (int)OpReportGridColumn.SNO || e.ColumnIndex == (int)OpReportGridColumn.DATE || e.ColumnIndex == (int)OpReportGridColumn.PATIENT || e.ColumnIndex == (int)OpReportGridColumn.PATIENTNO || e.ColumnIndex == (int)OpReportGridColumn.PATIENT_AGE
                    || e.ColumnIndex == (int)OpReportGridColumn.PATIENT_GENDER || e.ColumnIndex == (int)OpReportGridColumn.TOKENNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)OpReportGridColumn.CONSULTANT && e.RowIndex == GridviewOpReport.Rows.Count - 1)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }

            }
            if (e.ColumnIndex == (int)OpReportGridColumn.FEE || e.ColumnIndex == (int)OpReportGridColumn.TOKENNO)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            }
        }

        private void ComboBoxTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 0)
            {
                GridviewOpReport.Rows.Clear();
                EnableButton(false);
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                GridviewOpReport.Rows.Clear();
                EnableButton(false);
            }
            else
            {
                GridviewOpReport.Rows.Clear();
                EnableButton(false);
            }
        }
    }
}
