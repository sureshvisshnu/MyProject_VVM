using fa.api.Hms;
using fa.api.utils;
using fa.model.Hms.Master;
using fa.views.hms;
using fa.views.hms.patient;
using fa.views.utils.Report.Hms;
using fa.model.Hms.common;
using Fa.report.Hms;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Fa.api.Hms;
using fa.model.Accounting.Masters;
using System.IO;
using System.Text;
using fa.views.utils.Common;
using fa.Printing;
using fa.api.Accounting;
using fa.views.hms.Masters;
using Fa.views.utils.Report.Hms;
using ScottPlot.Drawing.Colormaps;
using fa.model.System;

namespace fa.reports.Hms
{
    enum PatientLedgerTableColumn
    {
        ICON, DATE, DESC, OPIP, FEECHARGED, AMOUNTRECEIVED, BALANCE, ID, INVOICE, ISCRTINV, ISPNTINV, INVID, LEDGDATE, OPID, LEDGID, ISDELETEINV
    }
    public partial class FormPatientLedger : FormPatientBase
    {
        public long PatientId = 0L;
        public long PatientIPId = 0L;
        RptPatientLedger RptPatientLedger = null!;
        public bool CreateLedgerOnLoad = false;
        public bool CreateLedgerOnLoadFromDischarge = false;
        public FormPatientLedger()
        {
            InitializeComponent();
        }
        public static string DeleteConfirmText = "Do you want to delete {0}?";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string SelectPatientErrorMsg = "Please select Patient.";
        public static string SelectValidPatientErrorMsg = "Please select valid Patient.";
        public static string EnterSearchTextMsg = "Please enter search text, it could patient Name or Phone Number or Date of Birth.";
        public static string RefNoErrorMsg = "Some thing went worng please contact administrator.";
        private void FormPatientLedger_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                TextBoxPatientSearch.TextBox.Select();
                if (CreateLedgerOnLoad || CreateLedgerOnLoadFromDischarge)
                {
                    if (PatientId != 0L)
                    {
                        if (CreateLedgerOnLoadFromDischarge)
                        {
                            CheckBoxLoadAllTransaction.Checked = false;
                        }
                        Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                        TextBoxPatientSearch.Text = Patient.Name;
                        RunReportButton.PerformClick();
                        BtnPrint.Select();
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PatientLedgerDataGridView.Rows.Clear();
                EnableButton(false);

                if (FormValidate())
                {
                    DateTime FiscalYearStartDate = Global.getCurrentFiscalYearStartDate();
                    DateTime LedgerFromDate = PatientLedgerFromDate.Date ?? DateTime.MinValue;
                    DateTime LedgerToDate = PatientLedgerToDate.Date ?? DateTime.MinValue;
                    if (LedgerFromDate.Date != FiscalYearStartDate && CheckBoxLoadAllTransaction.Checked == true)
                    {
                        PatientLedgerFromDate.Date = LedgerFromDate;
                        PatientLedgerToDate.Date = LedgerToDate;
                    }
                    LoadPatientLedger();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errod fetching Patient ledger (Error:" + ex.Message + ")");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadPatientLedger()
        {
            RptPatientLedger = new RptPatientLedger();
            RptPatientLedger.FromDate = (DateTime)PatientLedgerFromDate.Date!;
            RptPatientLedger.ToDate = (DateTime)PatientLedgerToDate.Date!;
            RptPatientLedger.Company = Global.Company;
            RptPatientLedger.PatientId = PatientId;
            RptPatientLedger.PatientIPId = PatientIPId;
            RptPatientLedger.GenerateReport();
            PatientLedgerDataGridView.Rows.Clear();
            if (RptPatientLedger.LineItems.Count > 0)
            {
                EnableButton(true);
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                int row = PatientLedgerDataGridView.Rows.Add();
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.Gray;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.ForeColor = Color.White;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.Gray;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.SelectionForeColor = Color.White;
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewTextBoxCell();

                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ICON].Value = "-";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = "Patient Name : " + Patient.Name + ",  " + "Patient ID : " + Patient.PatientNumber;
                //Print opening Balance
                row = PatientLedgerDataGridView.Rows.Add();
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DATE].Value = DateUtils.FormatDate(RptPatientLedger.FromDate, Global.Company.DateFormat);
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = "Op. Balance";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.BALANCE].Value = Math.Abs(RptPatientLedger.OpeningBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RptPatientLedger.OpeningBalance == 0 ? "      " : RptPatientLedger.OpeningBalance > 0 ? " DR" : " CR");
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ID].Value = RptPatientLedger.PatientId;
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewTextBoxCell();
                DateTime? lDate = null;
                double RBalance = RptPatientLedger.OpeningBalance;
                double FeeCharged = 0.00;
                TransactionType Type = TransactionType.PAYMENT;
                //Print Line items
                foreach (PatientLedgerLineItem LineItem in RptPatientLedger.LineItems.OrderBy(x => x.Date.Date).ThenBy(x => x.Id))
                {
                    row = PatientLedgerDataGridView.Rows.Add();
                    if (lDate != LineItem.Date.Date)
                    {
                        PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DATE].Value = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        lDate = LineItem.Date.Date;
                    }
                    if (LineItem.IsPntInv)
                    {
                        row = GenerateRowForInvoicePrintAndDelete(row, LineItem, RBalance);
                    }
                    else
                    {
                        Type = LineItem.Type;
                        if (string.IsNullOrEmpty(LineItem.Description))
                        {
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = LineItem.Name;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(LineItem.Name))
                            {
                                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = LineItem.Description;
                            }
                            else
                            {
                                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = LineItem.Name + " - " + LineItem.Description;
                            }
                        }
                        if (LineItem.Type == TransactionType.PAYMENT || LineItem.Type == TransactionType.WAIVER)
                        {
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.AMOUNTRECEIVED].Value = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                        else
                        {
                            FeeCharged += LineItem.Amount;
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.FEECHARGED].Value = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.OPIP].Value = LineItem.IPId == null ? "OP" : "IP";

                        }
                        RBalance += LineItem.Amount;
                        PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.LEDGDATE].Value = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.BALANCE].Value = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");
                        PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ID].Value = LineItem.Patient.Id;
                        if (LineItem.IsPntInv)
                        {
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVID].Value = LineItem.InvId;
                        }
                        if (LineItem.Type == TransactionType.PAYMENT)
                        {
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.LEDGID].Value = LineItem.Id;
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].Value = "Print";
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].ToolTipText = "Print payment receipt";
                        }
                        else
                        {
                            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewTextBoxCell();
                        }
                        if (LineItem.IsCrtInv)
                        {
                            GenerateRowForInvoice(row, LineItem);
                        }
                    }
                }
                row = PatientLedgerDataGridView.Rows.Add();

                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = "Cl. Balance";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.BALANCE].Value = Math.Abs(RptPatientLedger.ClosingBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RptPatientLedger.ClosingBalance == 0 ? "      " : RptPatientLedger.ClosingBalance > 0 ? " DR" : " CR");
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ID].Value = RptPatientLedger.PatientId;
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewTextBoxCell();
            }
            else
            {
                ErrorMsg.Text = "No Record Found..";
            }
        }

        private void GenerateRowForInvoice(int row, PatientLedgerLineItem lineItem)
        {
            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewButtonCell();
            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].Value = "Create";
            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].ToolTipText = "Create invoice";
            PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ISCRTINV].Value = true;
            PatientLedgerDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
            PatientLedgerDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
        }
        private int GenerateRowForInvoicePrintAndDelete(int row, PatientLedgerLineItem lineItem, double RBalance)
        {
            if (lineItem.InvId != 0L)
            {
                PatientInvoice PatientInvoice = PatientInvoiceManager.Instance.GetInvoiceById((long)lineItem.InvId!);
                if (PatientInvoice != null)
                {
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE] = new DataGridViewButtonCell();
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = "To Invoice #" + PatientInvoice.ReferenceNumber;
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.FEECHARGED].Value = Math.Abs(PatientInvoice.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    RBalance += PatientInvoice.Total;
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.BALANCE].Value = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].Value = "Print";
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].ToolTipText = "Print invoice";
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVID].Value = lineItem.InvId;
                    PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ISPNTINV].Value = true;
                }
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;

                row = PatientLedgerDataGridView.Rows.Add();
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.DESC].Value = "By Accounts Receivable";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.AMOUNTRECEIVED].Value = Math.Abs(PatientInvoice!.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                RBalance -= PatientInvoice.Total;
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.BALANCE].Value = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVID].Value = lineItem.InvId;
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].Value = "Delete";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.INVOICE].ToolTipText = "Delete invoice";
                PatientLedgerDataGridView.Rows[row].Cells[(int)PatientLedgerTableColumn.ISDELETEINV].Value = true;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
                PatientLedgerDataGridView.Rows[row].DefaultCellStyle.SelectionBackColor = Color.LightGray;
            }
            return row;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            PatientId = 0L;
            BtnPatientSearch.Select();
            Cursor.Current = Cursors.Default;
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            PatientLedgerPrint PatientLedgerPrint = new PatientLedgerPrint();
            PatientLedgerPrint.ExportToFileOrPrint(RptPatientLedger, false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PatientLedgerPrint PatientLedgerPrint = new PatientLedgerPrint();
            PatientLedgerPrint.ExportToFileOrPrint(RptPatientLedger, true);
            TextBoxPatientSearch.Focus();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            if (PatientId == 0L)
            {
                ErrorMsg.Text = SelectPatientErrorMsg;
                BtnPatientSearch.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxPatientSearch.Text))
            {
                ErrorMsg.Text = SelectPatientErrorMsg;
                TextBoxPatientSearch.Select();
                return false;
            }
            if (ValidPatient())
            {
                ErrorMsg.Text = SelectValidPatientErrorMsg;
                TextBoxPatientSearch.Select();
                return false;
            }
            if (PatientLedgerFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientLedgerFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                PatientLedgerFromDate.Focus();
                return false;
            }
            if (PatientLedgerToDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientLedgerToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                PatientLedgerToDate.Focus();
                return false;
            }
            return true;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            BtnLineItem.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            ErrorMsg.Text = "";
            PatientLedgerDataGridView.Rows.Clear();
            EnableButton(false);
            TextBoxPatientSearch.TextBox.ResetText();
            PatientLedgerFromDate.Format = Global.Company.DateFormat;
            PatientLedgerFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PatientLedgerToDate.Format = Global.Company.DateFormat;
            PatientLedgerToDate.Date = Global.getTransactionDate();
            CheckBoxLoadAllTransaction.Checked = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnPatientSearch.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReset.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void TextBoxPatientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnPatientSearch_Click(sender, e);
            }
        }
        private void BtnPatientSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
                FormPatientSearch.Searchstring = TextBoxPatientSearch.Text.Trim();
                FormPatientSearch.PatientSearchType = PatientSearchType.SearchPatient;
                FormPatientSearch.ShowDialog();
                if (PatientId != 0L)
                {
                    Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                    TextBoxPatientSearch.Text = Patient.Name;
                }
            }
        }
        private bool ValidPatient()
        {
            ErrorMsg.Text = string.Empty;
            string searchText = TextBoxPatientSearch.Text.Trim();
            Patient Patient = PatientManager.Instance.GetPatientByName(searchText, Global.Company.CompanyId);
            if (Patient != null)
            {
                if (TextBoxPatientSearch.Text == Patient.Name.ToString())
                {
                    return false;
                }
                return true;
            }
            return true;
        }
        private bool isValidSearchCriteria()
        {
            ErrorMsg.Text = string.Empty;
            string searchText = TextBoxPatientSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                ErrorMsg.Text = EnterSearchTextMsg;
                TextBoxPatientSearch.Select();
                return false;
            }
            return true;
        }
        protected override void PatientIdTransportReload(object sender, EventArgs e)
        {
            PatientId = string.IsNullOrEmpty(PatientIdTransport.Text) ? 0L : long.Parse(PatientIdTransport.Text);
            ResetForm();
        }
        private void CheckBoxLoadAllTransaction_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxLoadAllTransaction.Checked)
            {
                PatientLedgerFromDate.Date = Global.getTransactionDate().AddDays(-30);
            }
            else
            {
                IdSpace idSpace = CompanyManager.Instance.GetCompanyIdSpaceEntries(Global.Company.CompanyId);
                PatientLedgerFromDate.Date = idSpace.YearStartDate.Date;
            }
        }
        private void PatientLedgerDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)PatientLedgerTableColumn.INVOICE)
            {
                if (PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISCRTINV].Value != null &&
                    (bool)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISCRTINV].Value)
                {
                    CreateInvoice();
                    RunReportButton.PerformClick();
                }
                else if (PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.LEDGID].Value != null)
                {
                    OpTokenPrinting OpReceiptPrinting = new OpTokenPrinting();
                    OpReceiptPrinting.PrintReceiptFromLedger((long)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.LEDGID].Value, true);
                }
                else if (PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISPNTINV].Value != null && (bool)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISPNTINV].Value
                    && PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.INVOICE].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string PrintPaperFormat = GetPrintPaperFormat();
                    if (PrintPaperFormat == "A4 PORTRAIT")
                    {
                        PatientInvoicePrint PatientInvoicePrint = new PatientInvoicePrint();
                        PatientInvoicePrint.ExportOrPrintToFile((long)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.INVID].Value, "INVOICE", "pdf");
                    }
                    else if (PrintPaperFormat == "A5 LANDSCAPE")
                    {
                        PatientInvoicePrint patientInvoicePrint = new PatientInvoicePrint();
                        patientInvoicePrint.ExportA5LandscapeDotMatrixPrint((long)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.INVID].Value, "A5 LANDSCAPE", "Invoice", true);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else if (PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISDELETEINV].Value != null && (bool)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.ISDELETEINV].Value
                    && PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.INVOICE].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PatientLedgerDataGridView.Rows[e.RowIndex - 1].Cells[(int)PatientLedgerTableColumn.DESC].Value.ToString()!.Remove(0, 3)), "Delete Confirm",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        PatientInvoiceManager.Instance.DeleteInvoice((long)PatientLedgerDataGridView.Rows[e.RowIndex].Cells[(int)PatientLedgerTableColumn.INVID].Value);
                        RunReportButton.PerformClick();
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        public string GetPrintPaperFormat()
        {
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();
            string PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.PATIENT_INVOICE)!.PrintPaperFormat.Name;
            return PrintPaper;
        }
        private void CreateInvoice()
        {
            IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListPatientLedgerByPatientIdDate(PatientId);
            if (lPatientLedger != null && lPatientLedger.Count > 0)
            {
                string RefNum = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PATIENT_INVOICE, Global.getTransactionDate());
                if (!string.IsNullOrEmpty(RefNum))
                {
                    PatientInvoice PatientInvoice = new PatientInvoice();
                    PatientInvoice.CompanyId = Global.Company.CompanyId;
                    PatientInvoice.PatientId = PatientId;
                    PatientInvoice.ReferenceNumber = RefNum;
                    PatientInvoice.Description = "Payable";
                    PatientInvoice.InvoiceDate = Global.getTransactionDate();
                    PatientInvoiceManager.Instance.AddInvoiceFromLedger(PatientInvoice,RptPatientLedger.FromDate ,RptPatientLedger.ToDate);
                }
                else
                {
                    MessageBox.Show(RefNoErrorMsg);
                }
            }
        }
        private void PatientLedgerDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void BtnLineItem_Click(object sender, EventArgs e)
        {
            FormLedgerManageLineItems FormLedgerManageLineItems = new FormLedgerManageLineItems();
            FormLedgerManageLineItems.PatientId = PatientId;
            FormLedgerManageLineItems.ShowDialog();
            RunReportButton.PerformClick();
        }
    }
}
