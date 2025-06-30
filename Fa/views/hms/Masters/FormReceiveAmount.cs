using fa.api.Hms;
using fa.api.utils;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.views.hms.patient;
using fa.model.Hms.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.hms.Masters;
using fa.reports.Hms;
using fa.model.hms.config;
using fa.model.Accounting.Masters;
using fa.api.System;
using fa.Printing;
using fa.model.Accounting.Transactions;
using fa.api.Accounting;
using fa.libraries.utils;
using static fa.views.controls.hms.OutPatientVisit;
using fa.views.controls.hms;
using fa.model.Hms.Ip;
using fa.views.controls.grid;

namespace fa.views.hms.Masters
{
    public enum ReceiveAmountGridColumn
    {
        DATE, REF, OPIP, DESC, CONSULTEDBY, FEECHARGED, AMOUNTRECEIVED, EDIT, PRINT, LEDGERID, OPIPID
    }
    public partial class FormReceiveAmount : FormPatientBase
    {
        public static string ChoosePaymentTypeErrorMsg = "Please choose payment type";
        public static string EnterBankTranErrorMsg = "Please enter transaction";
        public static string EnterBankDateErrorMsg = "Please enter proper bank transaction date";
        public static string EnterCheckDocErrorMsg = "Please enter check number";
        public static string EnterCheckDateErrorMsg = "Please enter proper check date";
        public static string EnterCardTranErrorMsg = "Please enter credir card transaction";
        public static string EnterCardDateErrorMsg = "Please enter credit card date";
        public static string NothingFoundMsg = "No Patient found!";
        public static string EnterSearchTextMsg = "Please enter search text, it could patient Name or Phone Number or Date of Birth.";
        public static string NothingFoundIpOpMsg = "No pending op/ip";
        public static string SaveSuccessMsg = "Saved success.";
        public static string PaymentNotReceivedMsg = "Please enter the amount received.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string ChooseBankAccountErrorMsg = "Please choose bank account";
        public static string ChooseCheckAccountErrorMsg = "Please choose check account";
        public static string ChooseCardAccountErrorMsg = "Please choose card account";
        public static string DeleteConfirmText = "Do you want to delete the Payment {0}?";
        public double EditableAmount = 0.00;
        public FormReceiveAmount()
        {
            InitializeComponent();
        }
        private void FormReceiveAmount_Load(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxPatientSearch.TextBox.Select();
        }
        private void ResetForm()
        {
            GroupBoxCreditCard.Visible = false;
            GroupBoxCheckInfomation.Visible = false;
            GroupBoxBankTransfer.Visible = false;
            ComboBoxPaymentType.SelectedIndex = 0;
            TextBoxPaymentBankTransaction.ResetText();
            DateTimePickerPaymentBankDate.Format = Global.Company.DateFormat;
            DateTimePickerPaymentBankDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxPaymentCreditTransaction.ResetText();
            DateTimePickerPaymentCreditDate.Format = Global.Company.DateFormat;
            DateTimePickerPaymentCreditDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxPaymentCheckDocument.ResetText();
            DateTimePickerPaymentCheckDate.Format = Global.Company.DateFormat;
            DateTimePickerPaymentCheckDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            TextBoxPaymentId.ResetText();
            TextBoxPatientSearch.TextBox.ResetText();
            GridviewReceiveAmount.Rows.Clear();
            GridViewTotal.Rows.Clear();
            GridViewTotal.Rows.Add();
            GridViewBalance.Rows.Clear();
            GridViewBalance.Rows.Add();
            TextBoxReceiveAmountAmountReceived.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxReceiveAmountOutStandingBalance.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            PatientInfoMiniHorizontal.Clear();
            BtnDelete.Enabled = false;
            BtnSave.Enabled = false;
            BtnPrintLedger.Enabled = false;
            GridViewTotal.Rows[0].Cells[1].Value = 0.00;
            GridViewTotal.Rows[0].Cells[2].Value = 0.00;
            GridViewBalance.Rows[0].Cells[1].Value = 0.00;
            GridViewBalance.Rows[0].Cells[2].Value = 0.00;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridviewReceiveAmount.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridviewReceiveAmount.Columns["FeesCharged"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewBalance.Columns["BalCharged"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlace2)) currencyColumn2.DecimalPlaces = decimalPlace2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewBalance.Columns["BalReceived"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewTotal.Columns["Charged"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
            DataGridViewCurrencyColumn currencyColumn5 = (DataGridViewCurrencyColumn)GridViewTotal.Columns["Received"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces5)) currencyColumn5.DecimalPlaces = decimalPlaces5;
        }
        private void BtnPatientSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
                FormPatientSearch.Searchstring = TextBoxPatientSearch.Text.Trim();
                FormPatientSearch.PatientSearchType = PatientSearchType.SearchPatient;
                FormPatientSearch.ShowDialog();
                if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
                {
                    ResetForm();
                    LoadPatientInfo();
                    BtnPrintLedger.Enabled = true;
                }
            }
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
            TextBoxPatientId.Text = PatientIdTransport.Text;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                ResetForm();
                LoadPatientInfo();
                BtnPrintLedger.Enabled = true;
            }
        }
        private void LoadPatientInfo()
        {
            Patient PatientData = PatientManager.Instance.GetPatientById(long.Parse(TextBoxPatientId.Text));
            if (PatientData != null)
            {
                PatientInfoMiniHorizontal.PatientId = PatientData.Id;
                loadPatientIpOp(PatientData);
                BtnSave.Enabled = true;
                if (GridviewReceiveAmount.Rows.Count > 0)
                {
                    GridViewTotal.Rows[0].Cells[1].Value = FeeCharged();
                    GridViewTotal.Rows[0].Cells[2].Value = AmountReceived();
                    if (FeeCharged() > AmountReceived())
                    {
                        GridViewTotal.Rows[0].Cells[3].Value = "Dr";
                    }
                    else if (AmountReceived() > FeeCharged())
                    {
                        GridViewTotal.Rows[0].Cells[3].Value = "Cr";
                    }
                    GridViewBalance.Rows[0].Cells[1].Value = GetBalance();
                    GridViewBalance.Rows[0].Cells[2].Value = GetOutStandingBalance();
                    if (GetBalance() > GetOutStandingBalance())
                    {
                        GridViewBalance.Rows[0].Cells[3].Value = "Dr";
                    }
                    else if (GetOutStandingBalance() > GetBalance())
                    {
                        GridViewBalance.Rows[0].Cells[3].Value = "Cr";
                    }
                }
                if (GetBalance() > GetOutStandingBalance())
                {
                    TextBoxReceiveAmountOutStandingBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " Dr";
                    TextBoxReceiveAmountAmountReceived.Text = "0.00";
                }
                else if (GetOutStandingBalance() > GetBalance())
                {
                    TextBoxReceiveAmountOutStandingBalance.Text = Math.Abs(GetOutStandingBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " Cr";
                    TextBoxReceiveAmountAmountReceived.Text = "0.00";
                }
            }
        }
        private void TextBoxReceiveAmountAmountReceived_TextChanged(object sender, EventArgs e)
        {
            ErrorMsg.Text = "";
            double Received = string.IsNullOrEmpty(TextBoxReceiveAmountAmountReceived.Text.Trim()) ? 0.00 : double.Parse(TextBoxReceiveAmountAmountReceived.Text);
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                Received = Math.Abs(EditableAmount - Received);
            }
            double NewBalance = Received - GetBalance();
            TextBoxReceiveAmountOutStandingBalance.Text = (Math.Abs(GetOutStandingBalance()) + NewBalance) > 0 ? (Math.Abs(GetOutStandingBalance()) + NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : (Math.Abs(GetOutStandingBalance()) - NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

        }

        private void loadPatientIpOp(Patient Patient)
        {
            IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByPatientId(Patient.Id);

            if (lPatientLedger != null && lPatientLedger.Count > 0)
            {
                GridviewReceiveAmount.Rows.Clear();

                foreach (PatientLedger PatientLedger in lPatientLedger)
                {
                    if ((PatientLedger.Type == TransactionType.PAYMENT && (PatientLedger.Amount == 0 || PatientLedger?.Amount == null)) ||
                        (PatientLedger.Type == TransactionType.WAIVER && (PatientLedger.Amount == 0 || PatientLedger?.Amount == null)) ||
                        (PatientLedger.Type != TransactionType.PAYMENT && PatientLedger.Type != TransactionType.WAIVER && (PatientLedger.Amount == 0 || PatientLedger?.Amount == null)))
                    {
                        continue;
                    }

                    int rowIndex = GridviewReceiveAmount.Rows.Add();

                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.DATE].Value =
                        DateUtils.FormatDate(PatientLedger.Date, Global.Company.DateFormat) + " " + PatientLedger.Date.ToShortTimeString();
                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.OPIP].Value =
                        PatientLedger.OpRegistrationId != null && PatientLedger.InPatientAdmissionId == null ? "OP" : PatientLedger.InPatientAdmissionId != null ? "IP" : "";
                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.DESC].Value = PatientLedger.Description;
                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.CONSULTEDBY].Value = PatientLedger.ConsultantName();
                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.REF].Value = PatientLedger.RefNumber;

                    if (PatientLedger.Type == TransactionType.PAYMENT)
                    {
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.AMOUNTRECEIVED].Value =
                            PatientLedger.Amount == 0 ? "" : PatientLedger.Amount.ToString();
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.PRINT].Value = ImageListReceiveAmount.Images[0];
                        if (PatientLedger.IsReceivedPayment)
                        {
                            GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.EDIT].Value = ImageListReceiveAmount.Images[1];
                        }
                        else
                        {
                            GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.EDIT] = new DataGridViewTextBoxCell();
                        }
                    }
                    else if (PatientLedger.Type == TransactionType.WAIVER)
                    {
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.AMOUNTRECEIVED].Value =
                            PatientLedger.Amount == 0 ? "" : PatientLedger.Amount.ToString();
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.PRINT] = new DataGridViewTextBoxCell();
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.EDIT] = new DataGridViewTextBoxCell();
                    }
                    else
                    {
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.FEECHARGED].Value =
                            PatientLedger.Amount == 0 ? "" : PatientLedger.Amount.ToString();
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.PRINT] = new DataGridViewTextBoxCell();
                        GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.EDIT] = new DataGridViewTextBoxCell();
                    }

                    GridviewReceiveAmount.Rows[rowIndex].Cells[(int)ReceiveAmountGridColumn.LEDGERID].Value = PatientLedger.Id;
                }

                TextBoxReceiveAmountAmountReceived.BeginInvoke(new MethodInvoker(delegate ()
                {
                    TextBoxReceiveAmountAmountReceived.Select();
                }));
            }
            else
            {
                ErrorMsg.Text = NothingFoundIpOpMsg;
            }
        }
        private double FeeCharged()
        {
            double FeeCharged = 0.00;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListFeeChargeFromPatientLedgerByPatientId(long.Parse(TextBoxPatientId.Text));
                if (lPatientLedger != null && lPatientLedger.Count > 0)
                {
                    FeeCharged = lPatientLedger.Sum(x => x.Amount);
                }
            }
            return FeeCharged;
        }
        private double AmountReceived()
        {
            double AmountReceived = 0.00;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListPaymentReceivedFromPatientLedgerByPatientId(long.Parse(TextBoxPatientId.Text));
                if (lPatientLedger != null && lPatientLedger.Count > 0)
                {
                    AmountReceived = lPatientLedger.Sum(x => x.Amount);
                }
            }
            return AmountReceived;
        }
        private double GetBalance()
        {
            double Balance = 0.00;
            double Charged = FeeCharged();
            double Received = AmountReceived();
            if (Charged > Math.Abs(Received))
            {
                Balance = Charged - Received;
            }
            return Balance;
        }
        private double GetOutStandingBalance()
        {
            double Balance = 0.00;
            double Charged = FeeCharged();
            double Received = AmountReceived();
            if (Charged < Math.Abs(Received))
            {
                Balance = Received - Charged;
            }
            return Balance;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxPatientSearch.TextBox.Select();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (isValidate())
            {
                PatientLedger PatientLedger = new PatientLedger();
                PatientLedger PatientLedgerFromDB = null!;
                if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
                {
                    PatientLedgerFromDB = PatientLedgerManager.Instance.GetPatientLedgerById(long.Parse(TextBoxPaymentId.Text));
                    if (PatientLedgerFromDB != null)
                    {
                        PatientLedger.Id = PatientLedgerFromDB.Id;
                    }
                }
                DateTime DateTimeAmtReceived = new DateTime(Global.getTransactionDate().Year, Global.getTransactionDate().Month, Global.getTransactionDate().Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string RefNum = PatientLedger.Id == 0L ? CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PATIENT_FEE_RECEIPT, DateTimeAmtReceived) : PatientLedgerFromDB!.RefNumber;
                if (!string.IsNullOrEmpty(RefNum))
                {
                    Registration Registration = OpManager.Instance.GetPatientLastOp(long.Parse(TextBoxPatientId.Text), Global.getTransactionDate());
                    if (Registration != null)
                    {
                        InPatientAdmission InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId(Registration.Id);
                        PatientLedger.IsReceivedPayment = true;
                        PatientLedger.OpRegistrationId = Registration.Id;
                        if (InPatientAdmission != null)
                        {
                            PatientLedger.InPatientAdmissionId = InPatientAdmission.Id;
                        }
                    }
                    PatientLedger.RefNumber = RefNum;
                    PatientLedger.Date = DateTimeAmtReceived;
                    PatientLedger.PatientId = long.Parse(TextBoxPatientId.Text);
                    PatientLedger.Amount = double.Parse(TextBoxReceiveAmountAmountReceived.Text);
                    PatientLedger.Description = "Payment received as per ref #" + RefNum;
                    PatientLedger.CompanyId = Global.Company.CompanyId;
                    PatientLedger.Type = TransactionType.PAYMENT;
                    if (ComboBoxPaymentType.SelectedIndex > -1)
                    {
                        PatientPaymentDetail PatientPaymentDetail = new PatientPaymentDetail();
                        PatientPaymentDetail.PatientLedgerId = PatientLedger.Id;
                        PatientPaymentDetail.CompanyId = Global.Company.CompanyId;
                        PatientPaymentDetail.Date = Global.getTransactionDate();
                        PatientPaymentDetail.PaymentType = (PaymentType)ComboBoxPaymentType.SelectedIndex;
                        if (ComboBoxPaymentType.SelectedIndex > 0)
                        {
                            PatientPaymentDetail.DocumentNumber = ComboBoxPaymentType.SelectedIndex == 1 ? TextBoxPaymentCheckDocument.Text : ComboBoxPaymentType.SelectedIndex == 2 ? TextBoxPaymentCreditTransaction.Text : TextBoxPaymentBankTransaction.Text;
                            PatientPaymentDetail.DocumentDate = ComboBoxPaymentType.SelectedIndex == 1 ? (DateTime)DateTimePickerPaymentCheckDate.Date! : ComboBoxPaymentType.SelectedIndex == 2 ? (DateTime)DateTimePickerPaymentCreditDate.Date! : (DateTime)DateTimePickerPaymentBankDate.Date!;
                            PatientPaymentDetail.AccountId = ComboBoxPaymentType.SelectedIndex == 1 ? ((Account)ComboBoxPaymentCheckAccount.Items[ComboBoxPaymentCheckAccount.SelectedIndex]).Id : ComboBoxPaymentType.SelectedIndex == 2 ? ((Account)ComboBoxPaymentCreditCardAccount.Items[ComboBoxPaymentCreditCardAccount.SelectedIndex]).Id : ((Account)ComboBoxPaymentBankAccount.Items[ComboBoxPaymentBankAccount.SelectedIndex]).Id;
                        }
                        PatientLedger.PatientPaymentDetails.Add(PatientPaymentDetail);
                    }
                    PatientLedgerManager.Instance.UpdatePatientLedger(PatientLedger);
                    ResetForm();
                    LoadPatientInfo();
                    BtnPrintLedger.Enabled = true;
                    ErrorMsg.Text = SaveSuccessMsg;
                }
                else
                {
                    MessageBox.Show(RefNoErrorMsg);
                }
            }
        }
        private bool isValidate()
        {
            ErrorMsg.Text = string.Empty;
            if (string.IsNullOrEmpty(TextBoxReceiveAmountAmountReceived.Text)
                    || double.Parse(TextBoxReceiveAmountAmountReceived.Text) == 0)
            {
                TextBoxReceiveAmountAmountReceived.Select();
                ErrorMsg.Text = PaymentNotReceivedMsg;
                return false;
            }
            if (ComboBoxPaymentType.SelectedIndex < 0)
            {
                ComboBoxPaymentType.Select();
                ErrorMsg.Text = ChoosePaymentTypeErrorMsg;
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && ComboBoxPaymentBankAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentBankAccount.Select();
                ErrorMsg.Text = ChooseBankAccountErrorMsg;
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && string.IsNullOrEmpty(TextBoxPaymentBankTransaction.Text.Trim()))
            {
                TextBoxPaymentBankTransaction.Select();
                ErrorMsg.Text = EnterBankTranErrorMsg;
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && (DateTimePickerPaymentBankDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPaymentBankDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerPaymentBankDate.Focus();
                ErrorMsg.Text = EnterBankDateErrorMsg;
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && ComboBoxPaymentCheckAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentCheckAccount.Select();
                ErrorMsg.Text = ChooseCheckAccountErrorMsg;
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && string.IsNullOrEmpty(TextBoxPaymentCheckDocument.Text.Trim()))
            {
                TextBoxPaymentCheckDocument.Select();
                ErrorMsg.Text = EnterCheckDocErrorMsg;
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && (DateTimePickerPaymentCheckDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPaymentCheckDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerPaymentCheckDate.Focus();
                ErrorMsg.Text = EnterCheckDateErrorMsg;
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && ComboBoxPaymentCreditCardAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentCreditCardAccount.Select();
                ErrorMsg.Text = ChooseCardAccountErrorMsg;
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && string.IsNullOrEmpty(TextBoxPaymentCreditTransaction.Text.Trim()))
            {
                TextBoxPaymentCreditTransaction.Select();
                ErrorMsg.Text = EnterCardTranErrorMsg;
                return false;
            }

            if (GroupBoxCreditCard.Visible == true && (DateTimePickerPaymentCreditDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPaymentCreditDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerPaymentCreditDate.Select();
                ErrorMsg.Text = EnterCardDateErrorMsg;
                return false;
            }
            return true;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxPatientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnPatientSearch_Click(sender, e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnPatientSearch.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            if (keyData == (Keys.F4))
            {
                BtnDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrintLedger.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                TextBoxReceiveAmountAmountReceived.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxReceiveAmountAmountReceived.Select();

            }
        }

        private void BtnPrintLedger_Click(object sender, EventArgs e)
        {
            FormPatientLedger FormPatientLedger = new FormPatientLedger();
            FormPatientLedger.PatientId = long.Parse(TextBoxPatientId.Text);
            FormPatientLedger.CreateLedgerOnLoad = true;
            FormPatientLedger.ShowDialog();
        }

        private void GridviewReceiveAmount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)ReceiveAmountGridColumn.PRINT)
                {
                    if (GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.PRINT].Value != null
                        && GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.LEDGERID].Value != null)
                    {
                        OpTokenPrinting OpReceiptPrinting = new OpTokenPrinting();
                        OpReceiptPrinting.PrintReceiptFromLedger((long)GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.LEDGERID].Value, true);
                    }
                }
                else if (e.ColumnIndex == (int)ReceiveAmountGridColumn.EDIT)
                {
                    if (GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.EDIT].Value != null
                        && GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.LEDGERID].Value != null)
                    {
                        long LedgerId = (long)GridviewReceiveAmount.Rows[e.RowIndex].Cells[(int)ReceiveAmountGridColumn.LEDGERID].Value;
                        PatientLedger Ledger = PatientLedgerManager.Instance.GetPatientLedgerById(LedgerId);
                        if (Ledger != null)
                        {
                            LoadLedger(Ledger);
                        }
                    }
                }
            }
        }
        private void LoadLedger(PatientLedger Ledger)
        {
            TextBoxPaymentId.Text = Ledger.Id.ToString();
            EditableAmount = Ledger.Amount;
            TextBoxReceiveAmountAmountReceived.Text = Ledger.Amount.ToString();
            PatientPaymentDetail Detail = Ledger.PatientPaymentDetails.First();
            if (Detail != null)
            {
                ComboBoxPaymentType.SelectedIndex = (int)Detail.PaymentType;
                if (Detail.PaymentType == PaymentType.BANKTRANSFER)
                {
                    ComboBoxPaymentBankAccount.SelectedIndex = ComboBoxPaymentBankAccount.FindStringExact(Detail.Account.Name);
                    DateTimePickerPaymentBankDate.Date = (DateTime?)DateUtils.ToDate(Detail.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                    TextBoxPaymentBankTransaction.Text = Detail.DocumentNumber;
                }
                if (Detail.PaymentType == PaymentType.CHECK)
                {
                    TextBoxPaymentCheckDocument.Text = Detail.DocumentNumber;
                    DateTimePickerPaymentCheckDate.Date = (DateTime?)DateUtils.ToDate(Detail.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                    ComboBoxPaymentCheckAccount.SelectedIndex = ComboBoxPaymentCheckAccount.FindStringExact(Detail.Account.Name);
                }
                if (Detail.PaymentType == PaymentType.CREDITCARD)
                {
                    ComboBoxPaymentCreditCardAccount.SelectedIndex = ComboBoxPaymentCreditCardAccount.FindStringExact(Detail.Account.Name);
                    DateTimePickerPaymentCreditDate.Date = (DateTime?)DateUtils.ToDate(Detail.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                    TextBoxPaymentCreditTransaction.Text = Detail.DocumentNumber;
                }
                BtnDelete.Enabled = true;
            }
        }
        private void GridviewReceiveAmount_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void ComboBoxPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxPaymentType.SelectedIndex == 0)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxCashPayment.Visible = true;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Visible = false;
            }
            else if (ComboBoxPaymentType.SelectedIndex == 1)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxCheckInfomation.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = true;
                DateTimePickerPaymentCheckDate.Format = Global.Company.DateFormat;
                DateTimePickerPaymentCheckDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeBankAccountCombo(ComboBoxPaymentCheckAccount, Global.Company.CompanyId);
            }
            else if (ComboBoxPaymentType.SelectedIndex == 2)
            {
                GroupBoxCreditCard.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCreditCard.Visible = true;
                DateTimePickerPaymentCreditDate.Format = Global.Company.DateFormat;
                DateTimePickerPaymentCreditDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeCreditCardAccountCombo(ComboBoxPaymentCreditCardAccount, Global.Company.CompanyId);
            }
            else if (ComboBoxPaymentType.SelectedIndex == 3)
            {
                GroupBoxCashPayment.Visible = false;
                GroupBoxCreditCard.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Location = GroupBoxCashPayment.Location;
                GroupBoxBankTransfer.Visible = true;
                DateTimePickerPaymentBankDate.Format = Global.Company.DateFormat;
                DateTimePickerPaymentBankDate.Date = (DateTime?)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeBankAccountCombo(ComboBoxPaymentBankAccount, Global.Company.CompanyId);
            }
        }

        private void PatientInfoMiniHorizontal_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            TextBoxReceiveAmountAmountReceived.BeginInvoke(new MethodInvoker(delegate ()
            {
                TextBoxReceiveAmountAmountReceived.Select();
            }));
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                PatientLedger Ledger = PatientLedgerManager.Instance.GetPatientLedgerById(long.Parse(TextBoxPaymentId.Text));
                if (Ledger != null)
                {
                    DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, Ledger.RefNumber), "Delete Confirm",
             MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        PatientLedgerManager.Instance.UpdatePatientLedgerForDeletePayment(Ledger.Id);
                        ResetForm();
                        LoadPatientInfo();
                        BtnPrintLedger.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Somting went wrong, please check this payment is still valid.");
                    return;
                }
            }
        }
    }
}
