using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.api.Accounting;
using fa.api.System;
using fa.api.Log;
using fa.api.utils;
using fa.model.Accounting.Transaction;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.views.utils.Payments;
using fa.views.common;
using fa.views.utils;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.views.controls.grid;
using fa.views.account.masters;
using fa.views.employee;

namespace fa.views.account.transactions
{
    public partial class FormPayment : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Payment {0}?";
        public static string DeleteErrorText = "Error Deleting the Payment!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete Payment it used in payment";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter Payment details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Supplier Name or Payment Date or Payment Number.";
        public static string SearchOutput = "No Entry Found!";

        public static string ChooseTermErrorMsg = "Please choose Term";
        public static string ChooseSupplierErrorMsg = "Please choose Supplier";
        public static string EnterPaymentDateErrorMsg = "Please enter proper Payment Date";
        public static string ChoosePaymentTypeErrorMsg = "Please choose Payment Type";
        public static string EnterAmountErrorMsg = "Please enter Amount";
        public static string ChooseBankAccountErrorMsg = "Please choose Bank Account";
        public static string EnterBankTranErrorMsg = "Please enter Transaction";
        public static string EnterCheckDocErrorMsg = "Please enter document";
        public static string EnterCheckDateErrorMsg = "Please enter proper Check Date";
        public static string ChooseCheckAccountErrorMsg = "Please choose check Account";

        //public static string EntercCardNumberWrongErrorMsg = "Please card number properly";
        //public static string EnterCardNumberErrorMsg = "Please enter card number";
        //public static string EnterCardNameErrorMsg = "Please enter card name";
        //public static string ChooseCardTypeErrorMsg = "Please choose card type";
        //public static string EnterCardCVVWrongErrorMsg = "Please card cvv number properly";
        //public static string EnterCardCVVErrorMsg = "Please enter card cvv number";
        //public static string EnterCardDateErrorMsg = "Please enter proper Card Date";
        public static string EnterCardTranErrorMsg = "Please enter credir card transaction details";
        public static string ChooseCardAccountErrorMsg = "Please choose account";
        public static string EnterCardDateErrorMsg = "Please enter credit card Date";
        public static string EnterAmountWrongDateErrorMsg = "Please enter proper Amount";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";

        BillManager BillManager = null;
        PaymentManager PaymentManager = null;
        AccountManager AccountManager = null;
        AccountGroupManager AccountGroupManager = null;
        KeypressValidation KeypressValidation = null;
        CompanyManager CompanyManager = null;
        DateValidation DateValidation = null;
        PaymentSavePrint PaymentSavePrint = null;
        public long SearchPaymentId = 0L;
        public long SupplierId = 0L;


        int[] RequiredColumns = new int[] { 1, 4 };
        string[] RequiredColumnsValidationMsg = new string[] { "", "Please enter description for the line", "", "", "Please enter amount for the line", "Payment exceed balance amount", "First clear invoice amount after that add advance" };

        public FormPayment()
        {
            BillManager = BillManager.Instance;
            PaymentManager = PaymentManager.Instance;
            AccountManager = AccountManager.Instance;
            AccountGroupManager = AccountGroupManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            CompanyManager = CompanyManager.Instance;
            DateValidation = DateValidation.Instance;
            PaymentSavePrint = new PaymentSavePrint();
            InitializeComponent();
            excludedObjects = new string[] { "toolStripPayment", "DataGridViewOpenBills", "DataGridViewPayment" };
        }
        private void FormPayment_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //ComboUtils.InitializePaymentCombo(ComboBoxPaymentAccount, Global.Company.CompanyId);
                ResetForm();
                EnableForm(true);
                DataGridViewPaymentDetails.Columns["Amount"].DefaultCellStyle.Format = "#######.##";
                LoadReferenceAccount();
                TextBoxPaymentAccount.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadReferenceAccount()
        {
            if (SupplierId != 0)
            {
                TextBoxPaymentAccount.Id = SupplierId.ToString();
                TextBoxPaymentAccount.Text = (AccountManager.GetAccountById(SupplierId)).Name;
            }

        }
        private void LoadReferenceData()
        {
            i = 0;
            long AccountId = long.Parse(TextBoxPaymentAccount.Id);
            DataGridViewOpenBills.Rows.Clear();
            LoadPaymentBills(AccountId);
            LoadPurchaseBills(AccountId);
            LoadPaymentByAccount(AccountId);
        }
        int i = 0;
        private void LoadPaymentBills(long AccountId)
        {
            IList<Bill> PaymentBills = BillManager.GetUnPaidBillsByAccountId(AccountId);
            if (PaymentBills.Count > 0)
            {
                DataGridViewOpenBills.Rows.Add(PaymentBills.Count);
                foreach (var lPaymentBills in PaymentBills)
                {
                    DataGridViewOpenBills.Rows[i].Cells[0].Value = false;
                    DataGridViewOpenBills.Rows[i].Cells[1].Value = lPaymentBills.ReferenceNumber;
                    DataGridViewOpenBills.Rows[i].Cells[2].Value = "Bill";
                    DataGridViewOpenBills.Rows[i].Cells[3].Value = lPaymentBills.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenBills.Rows[i].Cells[4].Value = lPaymentBills.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenBills.Rows[i].Cells[5].Value = "Bill";
                    DataGridViewOpenBills.Rows[i].Cells[6].Value = lPaymentBills.BillId;
                    i++;
                }
            }
        }
        private void LoadPurchaseBills(long AccountId)
        {
            IList<PurchaseEntry> PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntryBySupplierId(AccountId, Global.Company.CompanyId);
            if (PurchaseEntry.Count > 0)
            {
                DataGridViewOpenBills.Rows.Add(PurchaseEntry.Count);
                foreach (var lPurchaseEntry in PurchaseEntry)
                {
                    DataGridViewOpenBills.Rows[i].Cells[0].Value = false;
                    DataGridViewOpenBills.Rows[i].Cells[1].Value = lPurchaseEntry.RefNumber;
                    DataGridViewOpenBills.Rows[i].Cells[2].Value = "Purchase";
                    DataGridViewOpenBills.Rows[i].Cells[3].Value = lPurchaseEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenBills.Rows[i].Cells[4].Value = lPurchaseEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenBills.Rows[i].Cells[5].Value = "Purchase";
                    DataGridViewOpenBills.Rows[i].Cells[6].Value = lPurchaseEntry.Id;

                    i++;
                }
            }
        }
        //private void LoadPayment()
        //{
        //    if (ComboBoxPaymentAccount.SelectedIndex > -1)
        //    {
        //        Account Account = (Account)ComboBoxPaymentAccount.Items[ComboBoxPaymentAccount.SelectedIndex];
        //        LoadPayment(Account);
        //    }
        //}
        private void LoadPaymentByAccount(long AccountId)
        {
            DataGridViewPayment.Rows.Clear();
            IList<Payment> Payment = PaymentManager.ListAllUnAppliedPaymentPaymentByAccount(AccountId);
            if (Payment.Count > 0)
            {
                DataGridViewPayment.Rows.Add(Payment.Count);
                int i = 0;
                foreach (var lPayment in Payment)
                {
                    DataGridViewPayment.Rows[i].Cells[0].Value = lPayment.Reference;
                    DataGridViewPayment.Rows[i].Cells[1].Value = lPayment.TransactionDate.ToShortDateString();
                    //DataGridViewPayment.Rows[i].Cells[3].Value = lPayment.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    decimal UnappliedAmount = 0;
                    string Descriptions = string.Empty;
                    foreach (var PaymentDetails in lPayment.PaymentDetails)
                    {
                        if (string.IsNullOrEmpty(PaymentDetails.ReferenceTrasnactionId))
                        {
                            UnappliedAmount = UnappliedAmount + PaymentDetails.Amount;
                            Descriptions = Descriptions + " " + PaymentDetails.Description;
                            DataGridViewPayment.Rows[i].Cells[2].Value = lPayment.Account.Name + ": " + Descriptions;
                            DataGridViewPayment.Rows[i].Cells[4].Value = UnappliedAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                    }
                    DataGridViewPayment.Rows[i].Cells[5].Value = lPayment.PaymentId;
                    DataGridViewPayment.Rows[i].Cells[6].Value = lPayment.TransctionType;
                    DataGridViewPayment.Rows[i].Cells[3].Value = UnappliedAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    i++;
                }
            }
        }
        private Payment GetPaymentFromForm()
        {
            Payment lPayment = new Payment();
            lPayment.Reference = PaymentRefNo.Text;
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                lPayment.PaymentId = Convert.ToInt64(TextBoxPaymentId.Text);
            }
            else
            {
                lPayment.PaymentId = 0L;
            }
            if (!string.IsNullOrEmpty(TextBoxPaymentAccount.Id))
            {
                Account Account = AccountManager.GetAccountById(long.Parse(TextBoxPaymentAccount.Id));
                if (Account != null)
                {
                    lPayment.AccountId = Account.Id;
                }
            }

            lPayment.TransactionDate = (DateTime)DateTimePickerPayment.Date;
            lPayment.TransctionType = (PaymentType)ComboBoxPaymentType.SelectedIndex;
            lPayment.Amount = decimal.Parse(TextBoxPaymentAmount.Text);
            lPayment.Description = TextBoxPaymentMemo.Text;
            lPayment.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lPayment.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count - 1; i++)
            {
                PaymentDetail PaymentDetail = new PaymentDetail();
                PaymentDetail.AccountId = 18;
                PaymentDetail.Description = (DataGridViewPaymentDetails.Rows[i].Cells[1].Value != null) ? DataGridViewPaymentDetails.Rows[i].Cells[1].Value.ToString() : "";
                PaymentDetail.Amount = (decimal.Parse(DataGridViewPaymentDetails.Rows[i].Cells[4].Value.ToString()));
                if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value != null)
                {
                    string Type = DataGridViewPaymentDetails.Rows[i].Cells[7].Value.ToString();
                    PaymentDetail.InvoiceType = Type == "Bill" ? InvoiceType.Basic : InvoiceType.ItemBased;
                    PaymentDetail.ReferenceTrasnactionId = DataGridViewPaymentDetails.Rows[i].Cells[6].Value.ToString();
                }
                lPayment.PaymentDetails.Add(PaymentDetail);
            }
            return lPayment;
        }

        private BankTransferPayment GetBankTransferPaymentFromForm()
        {
            Payment Payment = GetPaymentFromForm();
            BankTransferPayment lBankTransferPayment = new BankTransferPayment();
            lBankTransferPayment.PaymentDetails = Payment.PaymentDetails;
            lBankTransferPayment.Reference = Payment.Reference;
            lBankTransferPayment.PaymentId = Payment.PaymentId;
            lBankTransferPayment.AccountId = Payment.AccountId;
            lBankTransferPayment.TransactionDate = Payment.TransactionDate;
            lBankTransferPayment.TransctionType = Payment.TransctionType;
            lBankTransferPayment.Amount = Payment.Amount;
            lBankTransferPayment.Description = Payment.Description;
            lBankTransferPayment.CompanyId = Payment.CompanyId;
            lBankTransferPayment.CostCenterId = Payment.CostCenterId;
            lBankTransferPayment.TransactionNumber = TextBoxPaymentBankTransaction.Text;
            Account lAccountBank = (Account)ComboBoxPaymentBankAccount.Items[ComboBoxPaymentBankAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lBankTransferPayment.BankTransferId = AccountBank.Id;
                }
            }
            return lBankTransferPayment;
        }
        private CheckPayment GetCheckPaymentFromForm()
        {
            Payment Payment = GetPaymentFromForm();
            CheckPayment lCheckPayment = new CheckPayment();
            lCheckPayment.PaymentDetails = Payment.PaymentDetails;
            lCheckPayment.Reference = Payment.Reference;
            lCheckPayment.PaymentId = Payment.PaymentId;
            lCheckPayment.AccountId = Payment.AccountId;
            lCheckPayment.TransactionDate = Payment.TransactionDate;
            lCheckPayment.TransctionType = Payment.TransctionType;
            lCheckPayment.Amount = Payment.Amount;
            lCheckPayment.Description = Payment.Description;
            lCheckPayment.CompanyId = Payment.CompanyId;
            lCheckPayment.CostCenterId = Payment.CostCenterId;
            lCheckPayment.DocumentNumber = TextBoxPaymentCheckDocument.Text;
            lCheckPayment.DocumentDate = (DateTime)DateTimePickerPaymentCheckDate.Date;
            Account lAccountDepositedInto = (Account)ComboBoxPaymentCheckAccount.Items[ComboBoxPaymentCheckAccount.SelectedIndex];
            if (lAccountDepositedInto != null)
            {
                Account AccountDepositedInto = AccountManager.GetAccountById(lAccountDepositedInto.Id);
                if (AccountDepositedInto != null)
                {
                    lCheckPayment.DepositedIntoId = AccountDepositedInto.Id;
                }
            }

            return lCheckPayment;
        }
        private CreditCardPayment GetCreditCardPaymentFromForm()
        {
            Payment Payment = GetPaymentFromForm();
            CreditCardPayment lCreditCardPayment = new CreditCardPayment();
            lCreditCardPayment.PaymentDetails = Payment.PaymentDetails;
            lCreditCardPayment.Reference = Payment.Reference;
            lCreditCardPayment.PaymentId = Payment.PaymentId;
            lCreditCardPayment.AccountId = Payment.AccountId;
            lCreditCardPayment.TransactionDate = Payment.TransactionDate;
            lCreditCardPayment.TransctionType = Payment.TransctionType;
            lCreditCardPayment.Amount = Payment.Amount;
            lCreditCardPayment.Description = Payment.Description;
            lCreditCardPayment.CompanyId = Payment.CompanyId;
            lCreditCardPayment.CostCenterId = Payment.CostCenterId;
            lCreditCardPayment.CCTransactionDate = (DateTime)DateTimePickerPaymentCreditDate.Date;
            lCreditCardPayment.CCTransactionNumber = TextBoxPaymentCreditTransaction.Text;


            Account lAccountBank = (Account)ComboBoxPaymentCreditCardAccount.Items[ComboBoxPaymentCreditCardAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lCreditCardPayment.CCAccountId = AccountBank.Id;
                }
            }
            return lCreditCardPayment;
        }

        private void ComboBoxPaymentCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void RemoveBill()
        {
            if (DataGridViewPaymentDetails.Rows.Count > 1)
            {
                for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count - 1; i++)
                {
                    if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value != null)
                    {
                        DataGridViewPaymentDetails.Rows.RemoveAt(i);
                        RemoveBill();
                    }
                }
            }
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
                DateTimePickerPaymentCheckDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
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
                DateTimePickerPaymentCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeCreditCardAccountCombo(ComboBoxPaymentCreditCardAccount, Global.Company.CompanyId);

            }
            else if (ComboBoxPaymentType.SelectedIndex == 3)
            {
                GroupBoxCashPayment.Visible = false;
                GroupBoxCreditCard.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Location = GroupBoxCashPayment.Location;
                GroupBoxBankTransfer.Visible = true;
                ComboUtils.InitializeBankAccountCombo(ComboBoxPaymentBankAccount, Global.Company.CompanyId);
            }
        }

        private void BtnPaymentNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnPaymentSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxPaymentAccount.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxPaymentAccount.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnPaymentDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this payment is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PaymentRefNo.Text), "Delete Confirm",
         MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                long PaymentID = Convert.ToInt64(TextBoxPaymentId.Text);
                Payment Payment = PaymentManager.GetPayment(PaymentID);
                if (Payment != null)
                {
                    bool DeleteResult = PaymentManager.DeletePayment(Convert.ToInt64(TextBoxPaymentId.Text));
                    if (DeleteResult)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        ResetForm();
                        EnableForm(true);
                        BtnPaymentNew.Select();
                        //LoadPayment();
                        TextBoxPaymentAccount.Select();
                        this.formIsDirty = false;
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPayment.Text = NotAllowDeleteErrorText;
                    }
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                    return;
                }
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            BtnPaymentCancel.PerformClick();
            return;
        }
        private void BtnPaymentPrint_Click(object sender, EventArgs e)
        {
            if (PaymentManager.GetPayment(long.Parse(TextBoxPaymentId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxPaymentId.Text), TransactionTypes.PAYMENT);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                return;
            }
        }
        private void BtnPaymentCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxPaymentAccount.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxPaymentAccount.Select();
            if (!string.IsNullOrEmpty(TextBoxBillAccountId.Text))
            {
                LoadReferenceAccount();
                DateTimePickerPayment.Focus();
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnPaymentSave_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Payment PaymentFromDB = new Payment();
                    switch (ComboBoxPaymentType.SelectedIndex)
                    {
                        case 0:
                            Payment lPayment = GetPaymentFromForm();

                            if (lPayment.PaymentId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PAYMENT, (DateTime)DateTimePickerPayment.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lPayment.Reference = RefNo;
                                    PaymentFromDB = PaymentManager.AddPayment(lPayment);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorPayment.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (PaymentManager.GetPayment(lPayment.PaymentId) != null)
                                {
                                    PaymentFromDB = PaymentManager.UpdatePayment(lPayment);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                                    return;
                                }
                            }
                            break;
                        case 1:
                            CheckPayment lCheckPayment = GetCheckPaymentFromForm();
                            CheckPayment CheckPaymentFromDB = null;
                            if (lCheckPayment.PaymentId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PAYMENT, (DateTime)DateTimePickerPayment.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lCheckPayment.Reference = RefNo;
                                    CheckPaymentFromDB = PaymentManager.AddCheckPayment(lCheckPayment);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorPayment.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (PaymentManager.GetPayment(lCheckPayment.PaymentId) != null)
                                {
                                    CheckPaymentFromDB = PaymentManager.UpdateCheckPayment(lCheckPayment);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                                    return;
                                }
                            }
                            PaymentFromDB.PaymentId = CheckPaymentFromDB.PaymentId;
                            break;
                        case 2:
                            CreditCardPayment lCreditCardPayment = GetCreditCardPaymentFromForm();
                            CreditCardPayment CreditCardPaymentFromDB = null;
                            if (lCreditCardPayment.PaymentId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PAYMENT, (DateTime)DateTimePickerPayment.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lCreditCardPayment.Reference = RefNo;
                                    CreditCardPaymentFromDB = PaymentManager.AddCreditCardPayment(lCreditCardPayment);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorPayment.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (PaymentManager.GetPayment(lCreditCardPayment.PaymentId) != null)
                                {
                                    CreditCardPaymentFromDB = PaymentManager.UpdateCreditCardPayment(lCreditCardPayment);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                                    return;
                                }
                            }
                            PaymentFromDB.PaymentId = CreditCardPaymentFromDB.PaymentId;
                            break;
                        case 3:
                            BankTransferPayment lBankTransferPayment = GetBankTransferPaymentFromForm();
                            BankTransferPayment BankTransferPaymentFromDB = null;
                            if (lBankTransferPayment.PaymentId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PAYMENT, (DateTime)DateTimePickerPayment.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lBankTransferPayment.Reference = RefNo;
                                    BankTransferPaymentFromDB = PaymentManager.AddBankTransferPayment(lBankTransferPayment);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorPayment.Text = RefNoErrorMsg;
                                    return;
                                }
                            }
                            else
                            {
                                if (PaymentManager.GetPayment(lBankTransferPayment.PaymentId) != null)
                                {
                                    BankTransferPaymentFromDB = PaymentManager.UpdateBankTransferPayment(lBankTransferPayment);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this payment is still valid.");
                                    return;
                                }
                            }
                            PaymentFromDB.PaymentId = BankTransferPaymentFromDB.PaymentId;
                            break;
                    }
                    LoadPayment(PaymentFromDB.PaymentId);
                    ToolStripStatusLabelErrorPayment.Text = SaveSuccessText;
                    this.formIsDirty = false;
                }
                catch (Exception ex)
                {
                    ToolStripStatusLabelErrorPayment.Text = ex.Message;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxPaymentAccount.Id))
            {
                TextBoxPaymentAccount.Select();
                ToolStripStatusLabelErrorPayment.Text = ChooseSupplierErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxPaymentAccount.Id) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxPaymentAccount.Id)) == null)
            {
                TextBoxPaymentAccount.Select();
                ToolStripStatusLabelErrorPayment.Text = "Somting went wrong, please check this account is still valid.";
                ResetTimmer();
                return false;
            }
            if (DateTimePickerPayment.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPayment.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerPayment.Focus();
                ToolStripStatusLabelErrorPayment.Text = EnterPaymentDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (ComboBoxPaymentType.SelectedIndex < 0)
            {
                ComboBoxPaymentType.Select();
                ToolStripStatusLabelErrorPayment.Text = ChoosePaymentTypeErrorMsg;
                ResetTimmer();
                return false;
            }
            if (float.Parse(TextBoxPaymentAmount.Text) == 0)
            {
                TextBoxPaymentAmount.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterAmountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && ComboBoxPaymentBankAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentBankAccount.Select();
                ToolStripStatusLabelErrorPayment.Text = ChooseBankAccountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && string.IsNullOrEmpty(TextBoxPaymentBankTransaction.Text.Trim()))
            {
                TextBoxPaymentBankTransaction.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterBankTranErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && string.IsNullOrEmpty(TextBoxPaymentCheckDocument.Text.Trim()))
            {
                TextBoxPaymentCheckDocument.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterCheckDocErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && (DateTimePickerPaymentCheckDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPaymentCheckDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerPaymentCheckDate.Focus();
                ToolStripStatusLabelErrorPayment.Text = EnterCheckDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && ComboBoxPaymentCheckAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentCheckAccount.Select();
                ToolStripStatusLabelErrorPayment.Text = ChooseCheckAccountErrorMsg;
                ResetTimmer();
                return false;
            }

            //if (GroupBoxCreditCard.Visible == true && ComboBoxPaymentCreditCardType.SelectedIndex < 0)
            //{
            //    ComboBoxPaymentCreditCardType.Select();
            //    ToolStripStatusLabelErrorPayment.Text =ChooseCardTypeErrorMsg;
            //    ResetTimmer();
            //    return false;
            //}
            //if (GroupBoxCreditCard.Visible == true && string.IsNullOrEmpty(TextBoxPaymentCreditCardName.Text.Trim()))
            //{
            //    TextBoxPaymentCreditCardName.Select();
            //    ToolStripStatusLabelErrorPayment.Text = EnterCardNameErrorMsg;
            //    ResetTimmer();
            //    return false;
            //}
            //if (GroupBoxCreditCard.Visible == true && TextBoxPaymentCreditCardNumber.Text.Replace(" ", "") == "---")
            //{
            //    TextBoxPaymentCreditCardNumber.Select();
            //    ToolStripStatusLabelErrorPayment.Text =EnterCardNumberErrorMsg;
            //    ResetTimmer();
            //    return false;
            //}
            //if (GroupBoxCreditCard.Visible == true && TextBoxPaymentCreditCardNumber.Text.Replace(" ", "").Length != 19)
            //{
            //    TextBoxPaymentCreditCardNumber.Select();
            //    ToolStripStatusLabelErrorPayment.Text =EntercCardNumberWrongErrorMsg;
            //    ResetTimmer();
            //    return false;
            //}
            if (GroupBoxCreditCard.Visible == true && ComboBoxPaymentCreditCardAccount.SelectedIndex < 0)
            {
                ComboBoxPaymentCreditCardAccount.Select();
                ToolStripStatusLabelErrorPayment.Text = ChooseCardAccountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && TextBoxPaymentCreditTransaction.Text.Replace(" ", "") == "")
            {
                TextBoxPaymentCreditTransaction.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterCardTranErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCreditCard.Visible == true && (DateTimePickerPaymentCreditDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPaymentCreditDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerPaymentCreditDate.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterCardDateErrorMsg;
                ResetTimmer();
                return false;
            }

            int Count = DataGridViewPaymentDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        if (!DataGridViewPaymentDetails.Rows[i].Cells[j].ReadOnly &&
                            isRequiredColumn(j) &&
                            (DataGridViewPaymentDetails.Rows[i].Cells[j].Value == null || DataGridViewPaymentDetails.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision))))
                        {
                            DataGridViewPaymentDetails.Select();
                            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[j, i];
                            DataGridViewPaymentDetails.BeginEdit(true);
                            ToolStripStatusLabelErrorPayment.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewPaymentDetails.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value != null && float.Parse(DataGridViewPaymentDetails.Rows[i].Cells[3].Value.ToString()) > 0)
                    {
                        decimal Balance = decimal.Parse(DataGridViewPaymentDetails.Rows[i].Cells[3].Value.ToString());
                        if (DataGridViewPaymentDetails.Rows[i].Cells[8].Value != null)
                        {
                            PaymentDetail PaymentDetail = PaymentManager.GetPaymentDetail((long)DataGridViewPaymentDetails.Rows[i].Cells[8].Value);
                            if (PaymentDetail != null)
                            {
                                Balance += PaymentDetail.Amount;
                            }
                        }
                        decimal Amount = decimal.Parse(DataGridViewPaymentDetails.Rows[i].Cells[4].Value.ToString());
                        if (Balance < Amount)
                        {
                            DataGridViewPaymentDetails.Select();
                            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[4, i];
                            ToolStripStatusLabelErrorPayment.Text = RequiredColumnsValidationMsg[5];
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (float.Parse(DataGridViewPaymentDetails.Rows[i].Cells[2].Value.ToString()) > 0 && float.Parse(DataGridViewPaymentDetails.Rows[i].Cells[3].Value.ToString()) == 0)
                    {
                        decimal Balance = decimal.Parse(DataGridViewPaymentDetails.Rows[i].Cells[3].Value.ToString());
                        if (DataGridViewPaymentDetails.Rows[i].Cells[8].Value != null)
                        {
                            PaymentDetail PaymentDetail = PaymentManager.GetPaymentDetail((long)DataGridViewPaymentDetails.Rows[i].Cells[8].Value);
                            if (PaymentDetail != null)
                            {
                                Balance += PaymentDetail.Amount;
                            }
                        }
                        decimal Amount = decimal.Parse(DataGridViewPaymentDetails.Rows[i].Cells[4].Value.ToString());
                        if (Balance < Amount)
                        {
                            DataGridViewPaymentDetails.Select();
                            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[4, i];
                            ToolStripStatusLabelErrorPayment.Text = RequiredColumnsValidationMsg[5];
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value == null)
                    {
                        for (int k = 0; k < Count - 1; k++)
                        {
                            if (DataGridViewPaymentDetails.Rows[k].Cells[6].Value != null && float.Parse(DataGridViewPaymentDetails.Rows[i].Cells[3].Value.ToString()) > 0)
                            {
                                float Balance = float.Parse(DataGridViewPaymentDetails.Rows[k].Cells[3].Value.ToString());
                                float Amount = float.Parse(DataGridViewPaymentDetails.Rows[k].Cells[4].Value.ToString());
                                if (Balance > Amount)
                                {
                                    DataGridViewPaymentDetails.Select();
                                    DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[4, i];
                                    DataGridViewPaymentDetails.BeginEdit(true);
                                    ToolStripStatusLabelErrorPayment.Text = RequiredColumnsValidationMsg[6];
                                    ResetTimmer();
                                    return false;
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                DataGridViewPaymentDetails.Select();
                DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, 0];
                DataGridViewPaymentDetails.BeginEdit(true);
                ToolStripStatusLabelErrorPayment.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            DataGridViewPaymentDetails.CurrentCell.Style.SelectionBackColor = Color.White;
            if (double.Parse(TextBoxPaymentAmount.Text) != double.Parse(DataGridViewPaymentDetailsTotal.Rows[0].Cells[1].Value.ToString()))
            {
                TextBoxPaymentAmount.Select();
                ToolStripStatusLabelErrorPayment.Text = EnterAmountWrongDateErrorMsg;
                ResetTimmer();
                return false;
            }
            return true;
        }
        private bool isRequiredColumn(int Column)
        {
            foreach (int SelectedColumn in RequiredColumns)
            {
                if (Column == SelectedColumn)
                {
                    return true;
                }
            }
            return false;
        }
        private void ResetForm()
        {
            TextBoxSearchPayment.TextBox.ResetText();
            GroupBoxCreditCard.Visible = false;
            GroupBoxCheckInfomation.Visible = false;
            GroupBoxBankTransfer.Visible = false;
            DateTimePickerPayment.Format = Global.Company.DateFormat;
            DateTimePickerPayment.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerPayment.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerPayment.MaxDate = Global.getCurrentFiscalYearEndDate();
            TextBoxPaymentId.ResetText();
            TextBoxPaymentMemo.ResetText();
            PaymentRefNo.Text = "000000";
            ToolStripStatusLabelErrorPayment.Text = "";
            TextBoxPaymentCheckDocument.ResetText();
            TextBoxPaymentBankTransaction.ResetText();
            TextBoxPaymentAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxPaymentAccount.ResetText();
            ComboBoxPaymentBankAccount.ResetText();
            ComboBoxPaymentBankAccount.SelectedIndex = -1;
            ComboBoxPaymentCheckAccount.ResetText();
            ComboBoxPaymentCheckAccount.SelectedIndex = -1;
            ComboBoxPaymentType.SelectedIndex = 0;
            DateTimePickerPaymentCreditDate.Format = Global.Company.DateFormat;
            DateTimePickerPaymentCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerPaymentCreditDate.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerPaymentCreditDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            ComboBoxPaymentCreditCardAccount.ResetText();
            ComboBoxPaymentCreditCardAccount.SelectedIndex = -1;
            TextBoxPaymentCreditTransaction.ResetText();
            DataGridViewOpenBills.Rows.Clear();
            DataGridViewPayment.Rows.Clear();
            DataGridViewPaymentDetails.Rows.Clear();
            DataGridViewPaymentDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewPaymentDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewPaymentDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)DataGridViewPaymentDetails.Columns["BillBalance"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)DataGridViewPaymentDetails.Columns["BillAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            LastPaymentRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.PAYMENT, (DateTime)DateTimePickerPayment.Date!);
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxPaymentType.Visible = true;
            if (enable)
            {
                BtnPaymentNew.Enabled = !enable;
                BtnPaymentDelete.Enabled = !enable;
                BtnPaymentPrint.Enabled = !enable;
                BtnPaymentCancel.Enabled = enable;
                BtnPaymentSave.Enabled = enable;
            }
            else
            {
                BtnPaymentNew.Enabled = !enable;
                BtnPaymentDelete.Enabled = !enable;
                BtnPaymentPrint.Enabled = !enable;
                BtnPaymentCancel.Enabled = !enable;
                BtnPaymentSave.Enabled = !enable;
            }
        }
        private void DataGridViewPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5 && !DataGridViewPaymentDetails.Rows[e.RowIndex].IsNewRow)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewPaymentDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[6].Value != null)
                        {
                            for (int i = 0; i <= DataGridViewOpenBills.Rows.Count - 1; i++)
                            {
                                if (DataGridViewOpenBills.Rows[i].Cells[6].Value.ToString() == DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[6].Value.ToString()
                                    && DataGridViewOpenBills.Rows[i].Cells[5].Value.ToString() == DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[7].Value.ToString())
                                {
                                    DataGridViewOpenBills.Rows[i].Cells[0].Value = false;
                                    break;
                                }
                            }
                        }
                        DataGridViewPaymentDetails.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }
        private void Row_Removed()
        {
            ReSequence();
            ComputeFormTotal();
        }
        private void DataGridViewPayment_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 4)
            {
                if (DataGridViewPaymentDetails.CurrentCell.Value == null || DataGridViewPaymentDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewPaymentDetails.CurrentCell.Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision); ;
                }
                else
                {
                    DataGridViewPaymentDetails.CurrentCell.Value = double.Parse(DataGridViewPaymentDetails.CurrentCell.Value.ToString()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                Row_Added();
            }
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count - 1; i++)
            {
                double Amount = (DataGridViewPaymentDetails.Rows[i].Cells[4].Value) == null ? 0.00 : (double.Parse(DataGridViewPaymentDetails.Rows[i].Cells[4].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewPaymentDetailsTotal.Rows[0].Cells[1].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count; i++)
            {
                DataGridViewPaymentDetails.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void DataGridViewPayment_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            if (DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[5].Value == null)
            {
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            }
            if (DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[1].Value != null &&
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[5].Value == null)
            {
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        private void DataGridViewPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void DataGridViewPayment_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (DataGridViewPaymentDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewPayment_KeyPress1);
            }
            if (DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 4)
            {
                KeypressValidation.AddContextMenuGridCell(e, DataGridViewPaymentDetails, "NumberDot", DataGridViewPaymentDetails.CurrentCell.ColumnIndex);

                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewPayment_KeyPress);
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown += DataGridViewPayment_KeyDown;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void DataGridViewPayment_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
            if (Account != null)
            {
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewPaymentDetails.Rows.Count;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision); ;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision); ;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[4].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision); ;
                DataGridViewPaymentDetails.Rows[e.RowIndex].Cells[5].Value = "X";
            }
        }
        private void DataGridViewPayment_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewPaymentDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewPayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewPaymentDetails.Rows.Count > 0 && DataGridViewPaymentDetails.CurrentCell != null)
            {
                if (DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 4)
                {
                    if (DataGridViewPaymentDetails.CurrentCell.Value != null)
                    {
                        KeypressValidation.Keypress_NumberDot(sender, e, DataGridViewPaymentDetails.CurrentCell.Value.ToString());
                    }
                    else
                    {
                        KeypressValidation.Keypress_Number(sender, e);
                    }
                }
            }
        }
        private void DataGridViewPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }
        }
        private void TextBoxPaymentAmount_Leave(object sender, EventArgs e)
        {
            RedistributeTotalAmount();
        }
        private void TextBoxPaymentBankTransaction_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxPaymentCheckDocument_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxPaymentCheckBankName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }

        private void TextBoxPaymentCreditCardName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);

        }

        //private void ComboBoxPaymentCustomer_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxPaymentAccount.DroppedDown = false;

        //}

        private void ComboBoxPaymentType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPaymentType.DroppedDown = false;
        }

        private void ComboBoxPaymentBankAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPaymentBankAccount.DroppedDown = false;

        }
        private void ComboBoxPaymentCheckAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPaymentCheckAccount.DroppedDown = false;

        }
        //private void ComboBoxPaymentCreditCardType_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxPaymentCreditCardType.DroppedDown = false;

        //}
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnPaymentSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnPaymentNew.Enabled)
                {
                    BtnPaymentNew.PerformClick();
                }
                else
                {
                    if (TextBoxPaymentAccount.Focused)
                    {
                        BtnPaymentNewCustomers.ShowDropDown();
                    }
                }
            }
            else if (keyData == (Keys.F4))
            {
                if (BtnPaymentDelete.Enabled)
                {
                    BtnPaymentDelete.PerformClick();
                }
            }
            else if (keyData == (Keys.F9))
            {
                BtnPaymentPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnPaymentSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPaymentCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnPaymentExit.PerformClick();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnPaymentSave)
            {
                BtnPaymentCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnPaymentSave)
            {
                BtnPaymentExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnPaymentCancel)
            {
                BtnPaymentSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnPaymentCancel)
            {
                BtnPaymentExit.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnPaymentExit)
            {
                BtnPaymentSave.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnPaymentExit)
            {
                BtnPaymentCancel.Select();
                return true;
            }
            if (keyData == Keys.Tab && this.ActiveControl == toolStripPayment && BtnSearchPayment.Selected)
            {
                TextBoxPaymentAccount.Focus();
                return true;
            }
            try
            {
                if (DataGridViewPaymentDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 1)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 4)
                    {
                        if (DataGridViewPaymentDetails.CurrentCell.RowIndex != DataGridViewPaymentDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 4)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewPaymentDetails.CurrentCell.ColumnIndex == 1)
                    {
                        if (DataGridViewPaymentDetails.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            TextBoxPaymentMemo.Focus();
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerPayment.Stop();
            TimerPayment.Start();
        }

        private void TimerPayment_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorPayment.Visible = !this.ToolStripStatusLabelErrorPayment.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerPayment.Stop();
                ToolStripStatusLabelErrorPayment.Visible = true;
            }

        }
        private void TextBoxPaymentMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxPaymentMemo.SelectionLength = 0;
                e.IsInputKey = true;
                if (GroupBoxCashPayment.Visible == true)
                {
                    DataGridViewPaymentDetails.Select();
                    DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, 0];
                }
                else if (GroupBoxCheckInfomation.Visible == true) { TextBoxPaymentCheckDocument.Select(); }
                else if (GroupBoxCreditCard.Visible == true) { ComboBoxPaymentCreditCardAccount.Select(); }
                else if (GroupBoxBankTransfer.Visible == true) { ComboBoxPaymentBankAccount.Select(); }
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxPaymentMemo.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxPaymentAmount.Select();
            }
        }
        private void ComboBoxPaymentCheckAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewPaymentDetails.Select();
                DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, 0];
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerPaymentCheckDate.Focus();
            }
        }
        private void DateTimePickerPaymentCreditDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewPaymentDetails.Select();
                DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentCreditTransaction.Focus();
            }
        }
        private void TextBoxPaymentBankTransaction_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewPaymentDetails.Select();
                DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxPaymentBankAccount.Select();
            }
        }

        private void DataGridViewOpenBills_CellClick(object sender, DataGridViewCellEventArgs arg)
        {
            try
            {
                if (arg.ColumnIndex == 0 && arg.RowIndex > -1)
                {
                    int Count = DataGridViewPaymentDetails.Rows.Count - 1;
                    DataGridViewOpenBills.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataGridViewCheckBoxCell Cell = DataGridViewOpenBills.Rows[arg.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
                    if (DataGridViewOpenBills.Rows[arg.RowIndex].Cells[0].Value.ToString() == "False")
                    {
                        DataGridViewOpenBills.Rows[arg.RowIndex].Cells[0].Value = true;
                        DataGridViewPaymentDetails.Rows.Add(1);
                        for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count; i++)
                        {
                            DataGridViewPaymentDetails.Rows[i].Cells[0].Value = i + 1;
                        }

                        if (DataGridViewOpenBills.Rows[arg.RowIndex].Cells[1].Value != null)
                        {
                            var BillId = Convert.ToInt64(DataGridViewOpenBills.Rows[arg.RowIndex].Cells[6].Value);
                            string TransactionType = (string)DataGridViewOpenBills.Rows[arg.RowIndex].Cells[5].Value;
                            if (TransactionType != null)
                            {
                                switch (TransactionType)
                                {
                                    case "Bill":
                                        Bill SelectedBill = BillManager.GetBill(BillId);
                                        if (SelectedBill != null)
                                        {
                                            float FormBalance = GetFormBalance();
                                            DataGridViewPaymentDetails.Rows[Count].Cells[7].Value = TransactionType;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[6].Value = SelectedBill.BillId;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[1].Value = "Reference to Bill #: " + SelectedBill.ReferenceNumber;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[2].Value = SelectedBill.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            DataGridViewPaymentDetails.Rows[Count].Cells[3].Value = SelectedBill.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            if (FormBalance > 0)
                                            {
                                                if (FormBalance >= SelectedBill.Balance)
                                                {
                                                    DataGridViewPaymentDetails.Rows[Count].Cells[4].Value = SelectedBill.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                                else
                                                {
                                                    DataGridViewPaymentDetails.Rows[Count].Cells[4].Value = FormBalance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                            }
                                            Row_Added();
                                            DataGridViewPaymentDetails.Rows[Count].Cells[1].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[2].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[3].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[4].ReadOnly = false;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[5].ReadOnly = true;

                                            DataGridViewPaymentDetails.Select();
                                            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, Count + 1];
                                        }
                                        else
                                        {
                                            throw new Exception("Could not load the Bill");
                                        }
                                        break;
                                    case "Purchase":
                                        PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(BillId);
                                        if (PurchaseEntry != null)
                                        {
                                            float FormBalance = GetFormBalance();
                                            DataGridViewPaymentDetails.Rows[Count].Cells[7].Value = TransactionType;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[6].Value = PurchaseEntry.Id;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[1].Value = "Reference to Purchase #: " + PurchaseEntry.RefNumber;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[2].Value = PurchaseEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            DataGridViewPaymentDetails.Rows[Count].Cells[3].Value = PurchaseEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            if (FormBalance > 0)
                                            {
                                                if (FormBalance >= PurchaseEntry.Balance)
                                                {
                                                    DataGridViewPaymentDetails.Rows[Count].Cells[4].Value = PurchaseEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                                else
                                                {
                                                    DataGridViewPaymentDetails.Rows[Count].Cells[4].Value = FormBalance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                            }
                                            Row_Added();
                                            DataGridViewPaymentDetails.Rows[Count].Cells[1].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[2].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[3].ReadOnly = true;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[4].ReadOnly = false;
                                            DataGridViewPaymentDetails.Rows[Count].Cells[5].ReadOnly = true;

                                            DataGridViewPaymentDetails.Select();
                                            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, Count + 1];
                                        }
                                        else
                                        {
                                            throw new Exception("Could not load the Bill");
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid Transaction Type");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Transaction Id");
                        }
                    }
                    else if (DataGridViewOpenBills.Rows[arg.RowIndex].Cells[0].Value.ToString() == "True")
                    {
                        DataGridViewOpenBills.Rows[arg.RowIndex].Cells[0].Value = false;
                        for (int i = 0; i <= Count; i++)
                        {
                            if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value != null)
                            {
                                if (DataGridViewPaymentDetails.Rows[i].Cells[6].Value.ToString() == DataGridViewOpenBills.Rows[arg.RowIndex].Cells[6].Value.ToString()
                                    && DataGridViewPaymentDetails.Rows[i].Cells[7].Value.ToString() == DataGridViewOpenBills.Rows[arg.RowIndex].Cells[5].Value.ToString())
                                {
                                    DataGridViewPaymentDetails.Rows.RemoveAt(i);
                                    Row_Removed();
                                    DataGridViewPaymentDetails.Select();
                                    DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[1, Count - 1];
                                    return;
                                }
                            }
                        }
                    }
                }
            }
#pragma warning disable 0168 // variable declared but not used.
            catch (Exception e)
            {
                MessageBox.Show("Technical Error, Please contact your provider", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TextBoxPaymentMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private float GetFormBalance()
        {
            try
            {

                float FormTotal = (float)Convert.ToDouble(TextBoxPaymentAmount.Text);
                float FormLineItemTotal = (float)Convert.ToDouble(DataGridViewPaymentDetailsTotal.Rows[0].Cells[1].Value);
                float FormBalance = FormTotal - FormLineItemTotal;
                return FormBalance;
            }
            catch (Exception e)
            {
                Logger.LogError(new Exception("Could not calculate form balance", e));
                return 0F;
            }
        }


        private void RedistributeTotalAmount()
        {
            float TotalAmount = GetReceiptTotalAmount();
            for (int i = 0; i < DataGridViewPaymentDetails.Rows.Count - 1; i++)
            {
                float LineItemTotal = (float)GetLineItemTotal(i);
                if (DataGridUtils.readColumValueAsFloat(DataGridViewPaymentDetails, i, 4) == 0 && TotalAmount > 0 && !DataGridViewPaymentDetails.Rows[i].IsNewRow)
                {
                    float AmountToApply = (TotalAmount > LineItemTotal ? LineItemTotal : TotalAmount);
                    DataGridViewPaymentDetails.Rows[i].Cells[4].Value = AmountToApply.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    TotalAmount = TotalAmount - LineItemTotal;
                }
            }
            ComputeFormTotal();
        }
        private float GetReceiptTotalAmount()
        {
            try
            {
                return (float)Convert.ToDouble(TextBoxPaymentAmount.Text);
            }
            catch (Exception e)
            {
                return 0F;
            }
        }
        private double GetLineItemTotal(int row)
        {
            try
            {
                return (DataGridViewPaymentDetails.Rows[row].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewPaymentDetails.Rows[row].Cells[3].Value.ToString()));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void DataGridViewPayment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnPaymentSave_Click(sender, e);
                            LoadPayment((long)DataGridViewPayment.Rows[e.RowIndex].Cells[5].Value);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadPayment((long)DataGridViewPayment.Rows[e.RowIndex].Cells[5].Value);
                    }

                }
                else
                {
                    LoadPayment((long)DataGridViewPayment.Rows[e.RowIndex].Cells[5].Value);
                }
            }
        }
        private void LoadPayment(long PaymentId)
        {
            ResetForm();
            Payment Payment = PaymentManager.GetPayment(PaymentId);
            if (Payment != null)
            {
                PaymentRefNo.Text = Payment.Reference;
                TextBoxPaymentId.Text = Payment.PaymentId.ToString();
                TextBoxPaymentAccount.Id = Payment.AccountId.ToString();
                TextBoxPaymentAccount.Text = Payment.Account.Name;
                DateTimePickerPayment.Date = (DateTime)DateUtils.ToDate(Payment.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxPaymentType.SelectedIndex = ComboBoxPaymentType.FindStringExact(((PaymentType)Payment.TransctionType).ToString());
                TextBoxPaymentMemo.Text = Payment.Description;
                TextBoxPaymentAmount.Text = Payment.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                SearchPaymentId = 0L;
                MessageBox.Show("Somting went wrong, please check this payment is still valid.");
                return;
            }
            if (Payment.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferPayment BankTransferPayment = PaymentManager.GetBankTransferPayment(PaymentId);
                ComboBoxPaymentBankAccount.SelectedIndex = ComboBoxPaymentBankAccount.FindStringExact(BankTransferPayment.BankTransfer.Name);
                TextBoxPaymentBankTransaction.Text = BankTransferPayment.TransactionNumber;
            }
            if (Payment.TransctionType == PaymentType.CHECK)
            {
                CheckPayment CheckPayment = PaymentManager.GetCheckPayment(PaymentId);
                TextBoxPaymentCheckDocument.Text = CheckPayment.DocumentNumber;
                DateTimePickerPaymentCheckDate.Date = (DateTime)DateUtils.ToDate(CheckPayment.DocumentDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxPaymentCheckAccount.SelectedIndex = ComboBoxPaymentCheckAccount.FindStringExact(CheckPayment.BankAccount.Name);
            }
            if (Payment.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardPayment CreditCardPayment = PaymentManager.GetCreditCardPayment(PaymentId);

                ComboBoxPaymentCreditCardAccount.SelectedIndex = ComboBoxPaymentCreditCardAccount.FindStringExact(((Account)CreditCardPayment.CCAccount).ToString());

                DateTimePickerPaymentCreditDate.Date = (DateTime)DateUtils.ToDate(CreditCardPayment.CCTransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxPaymentCreditTransaction.Text = CreditCardPayment.CCTransactionNumber;

                //ComboBoxPaymentCreditCardType.SelectedIndex = ComboBoxPaymentCreditCardType.FindStringExact(((CardType)CreditCardPayment.CardType).ToString());
                //TextBoxPaymentCreditCardName.Text = CreditCardPayment.NameOnTheCard;
                //TextBoxPaymentCreditCardNumber.Text = CreditCardPayment.CardNumber;
                //DateTimePickerPaymentCreditDate.Date = (DateTime)DateUtils.ToDate(CreditCardPayment.ExpirationDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                //TextBoxPaymentCreditCVV.Text = CreditCardPayment.CVV;
            }
            if (Payment.PaymentDetails.Count > 0)
            {
                DataGridViewPaymentDetails.Rows.Add(Payment.PaymentDetails.Count);
                int i = 0;
                foreach (var PaymentDetail in Payment.PaymentDetails)
                {
                    DataGridViewPaymentDetails.Rows[i].Cells[8].Value = PaymentDetail.PaymentDetailId;
                    DataGridViewPaymentDetails.Rows[i].Cells[0].Value = i + 1;
                    DataGridViewPaymentDetails.Rows[i].Cells[1].Value = PaymentDetail.Description;
                    DataGridViewPaymentDetails.Rows[i].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                    DataGridViewPaymentDetails.Rows[i].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                    if (!string.IsNullOrEmpty(PaymentDetail.ReferenceTrasnactionId))
                    {

                        for (int j = 0; j < DataGridViewOpenBills.Rows.Count; j++)
                        {
                            if ((long)DataGridViewOpenBills.Rows[j].Cells[6].Value == long.Parse(PaymentDetail.ReferenceTrasnactionId))
                            {
                                DataGridViewOpenBills.Rows[j].Cells[0].Value = true;

                            }
                        }
                        if (PaymentDetail.InvoiceType == InvoiceType.Basic)
                        {
                            Bill Bill = BillManager.GetBill(long.Parse(PaymentDetail.ReferenceTrasnactionId));
                            if (Bill != null)
                            {
                                DataGridViewPaymentDetails.Rows[i].Cells[2].Value = Bill.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewPaymentDetails.Rows[i].Cells[3].Value = Bill.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewPaymentDetails.Rows[i].Cells[6].Value = Bill.BillId;
                                DataGridViewPaymentDetails.Rows[i].Cells[7].Value = "Bill";
                            }
                        }
                        else
                        {
                            PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(long.Parse(PaymentDetail.ReferenceTrasnactionId));
                            if (PurchaseEntry != null)
                            {
                                DataGridViewPaymentDetails.Rows[i].Cells[2].Value = PurchaseEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewPaymentDetails.Rows[i].Cells[3].Value = PurchaseEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewPaymentDetails.Rows[i].Cells[6].Value = PurchaseEntry.Id;
                                DataGridViewPaymentDetails.Rows[i].Cells[7].Value = "Purchase";
                            }
                        }
                    }
                    DataGridViewPaymentDetails.Rows[i].Cells[4].Value = PaymentDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewPaymentDetails.Rows[i].Cells[5].Value = "X";


                    i++;
                }
                Row_Added();
                ReSequence();
            }
            EnableForm(false);
            DateTimePickerPayment.Focus();
            this.formIsDirty = false;

        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            if (string.IsNullOrEmpty(TextBoxSearchPayment.Text.Trim()))
            {
                ToolStripStatusLabelErrorPayment.Text = SearchBoxEmptyErrorMsg;
                TextBoxSearchPayment.TextBox.Focus();
                return false;
            }
            return true;
        }
        private void BtnPaymentSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPaymentAccount.Select();
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewPaymentDetails.Rows.Count > 0)
                {
                    DataGridViewPaymentDetails.Select();
                    DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails[4, DataGridViewPaymentDetails.Rows.Count - 1];
                }
            }
        }

        private void ComboBoxPaymentAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void BtnPaymentExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPayment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                    TextBoxPaymentAccount.Select();
                }
            }
        }

        private void BtnSearchPayment_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isValidSearchCriteria())
            {
                RecentPayment();
                if (SearchPaymentId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnPaymentSave_Click(sender, e);
                                LoadPayment(SearchPaymentId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadPayment(SearchPaymentId);
                        }
                    }
                    else
                    {
                        LoadPayment(SearchPaymentId);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void RecentPayment()
        {
            ToolStripStatusLabelErrorPayment.Text = "";
            String SearchText = TextBoxSearchPayment.Text.Trim();
            IList<Payment> PaymentInfo = null;

            if (TextUtils.isAmount(SearchText))
            {
                PaymentInfo = PaymentManager.GetPaymentByAmount(float.Parse(SearchText), SearchText, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                PaymentInfo = PaymentManager.GetPaymentByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                PaymentInfo = PaymentManager.GetPaymentBySupplierName(SearchText, Global.Company.CompanyId);
            }
            if (PaymentInfo.Count > 0)
            {
                LoadPayment(PaymentInfo);
            }
            else
            {
                SearchPaymentId = 0L;
                ToolStripStatusLabelErrorPayment.Text = SearchOutput;
            }
        }
        public void LoadPayment(IList<Payment> PaymentInfo)
        {
            SearchPaymentId = 0L;
            FormRecentPayment FormRecentPayment = new FormRecentPayment(this);
            FormRecentPayment.Text = "Recent Payment";
            FormRecentPayment.PaymentInfo = PaymentInfo;
            FormRecentPayment.ShowDialog();
        }

        private void TextBoxSearchPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchPayment_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnSearchPayment.PerformClick();
            }
        }

        private void TextBoxPaymentAccount_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxPaymentAccount.Id))
            {
                RemoveBill();
                Row_Removed();
                LoadReferenceData();
            }
        }

        private void TextBoxPaymentAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerPayment.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPaymentSave.Select();
            }
        }

        private void DataGridViewPaymentDetails_Leave(object sender, EventArgs e)
        {
            DataGridViewPaymentDetails.CurrentCell = DataGridViewPaymentDetails.CurrentCell != null ? DataGridViewPaymentDetails[2, DataGridViewPaymentDetails.CurrentRow.Index] : null;
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnPaymentSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxPaymentAccount.Id == null ? 0L : long.Parse(TextBoxPaymentAccount.Id);
            SupplierId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = true;
            FormSearchAccount.IncludeGeneralAccounts = true;
            FormSearchAccount.ShowDialog();
            if (SupplierId != 0)
            {

                Account Account = AccountManager.Instance.GetAccountById(SupplierId);
                if (Account != null)
                {
                    TextBoxPaymentAccount.ResetText();
                    TextBoxPaymentAccount.Id = SupplierId.ToString();
                    TextBoxPaymentAccount.Text = Account.Name;
                }
                else
                {
                    SupplierId = (long)TempSupplierId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                }
            }
            TextBoxPaymentAccount.Select();
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxPaymentCreditCardAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPaymentCreditCardAccount.DroppedDown = false;
        }

        private void BtnPaymentNewCustomers_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxPaymentAccount.Id == null ? 0L : long.Parse(TextBoxPaymentAccount.Id);
            SupplierId = 0L;
            if (e.ClickedItem.Text == "Account")
            {
                FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
                FormGeneralAccounts.CreateAccountOnLoad = true;
                FormGeneralAccounts.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Supplier")
            {
                FormSupplier FormSupplier = new FormSupplier(this);
                FormSupplier.CreateSupplierOnLoad = true;
                FormSupplier.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Customer")
            {
                FormCustomers FormCustomers = new FormCustomers(this);
                FormCustomers.CreateCustomerOnLoad = true;
                FormCustomers.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Employee")
            {
                FormEmployee FormEmployee = new FormEmployee(this);
                FormEmployee.CreateEmployeeOnLoad = true;
                FormEmployee.ShowDialog(this);
            }
            if (SupplierId != 0)
            {
                Account Account = AccountManager.Instance.GetAccountById(SupplierId);
                if (Account != null)
                {
                    TextBoxPaymentAccount.Text = Account.Name;
                    TextBoxPaymentAccount.Id = SupplierId.ToString();
                }
                else
                {
                    SupplierId = (long)TempCustomerId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    SupplierId = (long)TempCustomerId;
                    this.formIsDirty = false;
                }
            }
            TextBoxPaymentAccount.Select();
            Cursor.Current = Cursors.Default;
        }
    }
}
