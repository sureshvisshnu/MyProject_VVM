using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.System;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Log;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.views.utils.Receipts;
using fa.views.common;
using fa.views.utils;
using fa.views.controls.grid;
using fa.views.account.masters;
using fa.views.employee;

namespace fa.views.account.transactions
{
    public partial class FormReceipts : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Receipt {0}?";
        public static string DeleteErrorText = "Error Deleting the Receipt!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete Receipt it used in Receipt";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter Receipt details.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Receipt Date or Receipt Number.";
        public static string SearchOutput = "No Entry Found!";
        public static string ChooseTermErrorMsg = "Please choose Term";
        public static string ChooseCustomerErrorMsg = "Please choose Customer";
        public static string EnterReceiptDateErrorMsg = "Please enter proper Receipt Date";
        public static string ChooseReceiptTypeErrorMsg = "Please choose Receipt Type";
        public static string EnterAmountErrorMsg = "Please enter Amount";
        public static string ChooseBankAccountErrorMsg = "Please choose Bank Account";
        public static string EnterBankTranErrorMsg = "Please enter Transaction";
        public static string EnterCheckDocErrorMsg = "Please enter document";
        public static string EnterCheckDateErrorMsg = "Please enter proper Check Date";
        public static string ChooseCheckAccountErrorMsg = "Please choose check Account";
        public static string EnterCardTranErrorMsg = "Please enter credir card transaction details";
        public static string ChooseCardAccountErrorMsg = "Please choose account";
        public static string EnterCardDateErrorMsg = "Please enter credit card Date";
        public static string EnterAmountWrongDateErrorMsg = "Please enter proper Amount";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        ReceiptManager ReceiptManager = null;
        InvoiceManager InvoiceManager = null;
        AccountManager AccountManager = null;
        AccountGroupManager AccountGroupManager = null;
        KeypressValidation KeypressValidation = null;
        CompanyManager CompanyManager = null;
        DateValidation DateValidation = null;
        DebitNoteManager DebitNoteManager = null;
        ReceiptSavePrint ReceiptSavePrint = null;
        public long SearchReceiptId = 0L;
        public long CustomerId = 0L;

        int[] RequiredColumns = new int[] { 1, 4 };
        string[] RequiredColumnsValidationMsg = new string[] { "", "Please enter description for the line", "", "", "Please enter amount for the line", "Payment exceed balance amount", "First clear invoice amount after that add advance" };

        public FormReceipts()
        {
            AccountManager = AccountManager.Instance;
            ReceiptManager = ReceiptManager.Instance;
            AccountGroupManager = AccountGroupManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            CompanyManager = CompanyManager.Instance;
            DateValidation = DateValidation.Instance;
            InvoiceManager = InvoiceManager.Instance;
            DebitNoteManager = DebitNoteManager.Instance;
            ReceiptSavePrint = new ReceiptSavePrint();
            InitializeComponent();
            excludedObjects = new string[] { "toolStrip1", "DataGridViewOpenInvoices", "DataGridViewReceipt" };
        }



        private void Receipts_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                //DataGridViewReceiptDetails.Columns["Amount"].DefaultCellStyle.Format = "#######.##";
                //DataGridViewReceiptDetails.Columns["InvAmount"].DefaultCellStyle.Format = "#######.##";
                //DataGridViewReceiptDetails.Columns["BalAmount1"].DefaultCellStyle.Format = "#######.##";
                LoadReferenceAccount();
                TextBoxReceiptAccount.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void LoadReferenceAccount()
        {
            if (CustomerId != 0)
            {
                TextBoxReceiptAccount.Id = CustomerId.ToString();
                TextBoxReceiptAccount.Text = (AccountManager.GetAccountById(CustomerId)).Name;
            }
        }
        private void LoadReferenceData()
        {
            i = 0;
            long AccountId = long.Parse(TextBoxReceiptAccount.Id);
            DataGridViewOpenInvoices.Rows.Clear();
            LoadInvoices(AccountId);
            LoadSaleInvoices(AccountId);
            LoadRecepit(AccountId);
        }
        int i = 0;
        private void LoadInvoices(long AccountId)
        {
            IList<Invoice> Invoices = InvoiceManager.GetUnPaidInvoicesByAccountId(AccountId);
            if (Invoices.Count > 0)
            {
                DataGridViewOpenInvoices.Rows.Add(Invoices.Count);
                foreach (var lReceiptInvoices in Invoices)
                {
                    DataGridViewOpenInvoices.Rows[i].Cells[0].Value = false;
                    DataGridViewOpenInvoices.Rows[i].Cells[1].Value = lReceiptInvoices.ReferenceNumber;
                    DataGridViewOpenInvoices.Rows[i].Cells[2].Value = lReceiptInvoices.InvoiceDate.ToString(Global.Company.DateFormat);
                    DataGridViewOpenInvoices.Rows[i].Cells[3].Value = "Invoice";
                    DataGridViewOpenInvoices.Rows[i].Cells[4].Value = lReceiptInvoices.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenInvoices.Rows[i].Cells[5].Value = lReceiptInvoices.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenInvoices.Rows[i].Cells[6].Value = "Invoice";
                    DataGridViewOpenInvoices.Rows[i].Cells[7].Value = lReceiptInvoices.InvoiceId;

                    i++;
                }
            }
        }
        private void LoadSaleInvoices(long AccountId)
        {
            IList<SaleEntry> SaleEntry = SalesManager.Instance.GetSaleEntryByCustomerId(AccountId, Global.Company.CompanyId);
            if (SaleEntry.Count > 0)
            {
                DataGridViewOpenInvoices.Rows.Add(SaleEntry.Count);
                foreach (var lSaleEntry in SaleEntry)
                {
                    DataGridViewOpenInvoices.Rows[i].Cells[0].Value = false;
                    DataGridViewOpenInvoices.Rows[i].Cells[1].Value = lSaleEntry.RefNumber;
                    DataGridViewOpenInvoices.Rows[i].Cells[2].Value = lSaleEntry.SaleDate.ToString(Global.Company.DateFormat);
                    DataGridViewOpenInvoices.Rows[i].Cells[3].Value = "Sale";
                    DataGridViewOpenInvoices.Rows[i].Cells[4].Value = lSaleEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenInvoices.Rows[i].Cells[5].Value = lSaleEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewOpenInvoices.Rows[i].Cells[6].Value = "Sale";
                    DataGridViewOpenInvoices.Rows[i].Cells[7].Value = lSaleEntry.Id;
                    i++;
                }
            }
        }
        private void LoadRecepit(long AccountId)
        {
            DataGridViewReceipt.Rows.Clear();
            IList<Receipt> Receipt = ReceiptManager.ListAllUnAppliedPaymentReceiptByAccount(AccountId);
            if (Receipt.Count > 0)
            {
                DataGridViewReceipt.Rows.Add(Receipt.Count);
                int i = 0;
                foreach (var lReceipt in Receipt)
                {
                    DataGridViewReceipt.Rows[i].Cells[0].Value = lReceipt.Reference;
                    DataGridViewReceipt.Rows[i].Cells[1].Value = lReceipt.TransactionDate.ToString(Global.Company.DateFormat);
                    decimal UnappliedAmount = 0;
                    string Descriptions = string.Empty;
                    foreach (var ReceiptDetail in lReceipt.ReceiptDetails)
                    {
                        if (string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
                        {
                            UnappliedAmount = UnappliedAmount + ReceiptDetail.Amount;
                            Descriptions = Descriptions + " " + ReceiptDetail.Description;
                            DataGridViewReceipt.Rows[i].Cells[2].Value = lReceipt.Account.Name + ": " + Descriptions;
                            DataGridViewReceipt.Rows[i].Cells[4].Value = UnappliedAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                    }
                    DataGridViewReceipt.Rows[i].Cells[5].Value = lReceipt.ReceiptId;
                    DataGridViewReceipt.Rows[i].Cells[6].Value = lReceipt.TransactionType;
                    DataGridViewReceipt.Rows[i].Cells[3].Value = UnappliedAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                    i++;
                }
            }
        }

        private Receipt GetReceiptFromForm()
        {
            Receipt lReceipt = new Receipt();
            lReceipt.Reference = ReceiptsRefNo.Text;
            if (!string.IsNullOrEmpty(TextBoxReceiptId.Text))
            {
                lReceipt.ReceiptId = Convert.ToInt64(TextBoxReceiptId.Text);
            }
            else
            {
                lReceipt.ReceiptId = 0L;
            }
            if (!string.IsNullOrEmpty(TextBoxReceiptAccount.Id))
            {
                Account Account = AccountManager.GetAccountById(long.Parse(TextBoxReceiptAccount.Id));
                if (Account != null)
                {
                    lReceipt.AccountId = Account.Id;
                }
            }
            lReceipt.TransactionDate = (DateTime)DateTimePickerReceipt.Date;
            lReceipt.TransactionType = (PaymentType)ComboBoxReceiptType.SelectedIndex;
            lReceipt.Amount = decimal.Parse(TextBoxReceiptAmount.Text);
            lReceipt.Description = TextBoxReceiptMemo.Text;
            lReceipt.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lReceipt.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count - 1; i++)
            {
                ReceiptDetail ReceiptDetail = new ReceiptDetail();

                ReceiptDetail.AccountId = 17;
                ReceiptDetail.Description = (DataGridViewReceiptDetails.Rows[i].Cells[1].Value != null) ? DataGridViewReceiptDetails.Rows[i].Cells[1].Value.ToString() : "";
                ReceiptDetail.Amount = (decimal.Parse(DataGridViewReceiptDetails.Rows[i].Cells[4].Value.ToString()));
                if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value != null)
                {
                    string Type = DataGridViewReceiptDetails.Rows[i].Cells[7].Value.ToString();
                    ReceiptDetail.InvoiceType = Type == "Invoice" ? InvoiceType.Basic : InvoiceType.ItemBased;
                    ReceiptDetail.ReferenceTrasnactionId = DataGridViewReceiptDetails.Rows[i].Cells[6].Value.ToString();
                }
                lReceipt.ReceiptDetails.Add(ReceiptDetail);
            }
            return lReceipt;
        }

        private BankTransferReceipt GetBankTransferReceiptFromForm()
        {
            Receipt Receipt = GetReceiptFromForm();
            BankTransferReceipt lBankTransferReceipt = new BankTransferReceipt();
            lBankTransferReceipt.ReceiptDetails = Receipt.ReceiptDetails;
            lBankTransferReceipt.Reference = Receipt.Reference;
            lBankTransferReceipt.ReceiptId = Receipt.ReceiptId;
            lBankTransferReceipt.AccountId = Receipt.AccountId;
            lBankTransferReceipt.TransactionDate = Receipt.TransactionDate;
            lBankTransferReceipt.TransactionType = Receipt.TransactionType;
            lBankTransferReceipt.Amount = Receipt.Amount;
            lBankTransferReceipt.Description = Receipt.Description;
            lBankTransferReceipt.CompanyId = Receipt.CompanyId;
            lBankTransferReceipt.CostCenterId = Receipt.CostCenterId;
            lBankTransferReceipt.TransactionNumber = TextBoxReceiptBankTransaction.Text;
            Account lAccountBank = (Account)ComboBoxReceiptBankAccount.Items[ComboBoxReceiptBankAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lBankTransferReceipt.BankTransferId = AccountBank.Id;
                }
            }
            return lBankTransferReceipt;
        }

        private CheckReceipt GetCheckReceiptFromForm()
        {
            Receipt Receipt = GetReceiptFromForm();

            CheckReceipt lCheckReceipt = new CheckReceipt();
            lCheckReceipt.ReceiptDetails = Receipt.ReceiptDetails;
            lCheckReceipt.Reference = Receipt.Reference;
            lCheckReceipt.ReceiptId = Receipt.ReceiptId;
            lCheckReceipt.AccountId = Receipt.AccountId;
            lCheckReceipt.TransactionDate = Receipt.TransactionDate;
            lCheckReceipt.TransactionType = Receipt.TransactionType;
            lCheckReceipt.Amount = Receipt.Amount;
            lCheckReceipt.Description = Receipt.Description;
            lCheckReceipt.CompanyId = Receipt.CompanyId;
            lCheckReceipt.CostCenterId = Receipt.CostCenterId;

            lCheckReceipt.DocumentNumber = TextBoxReceiptCheckDocument.Text;
            lCheckReceipt.DocumentDate = (DateTime)DateTimePickerReceiptCheckDate.Date;
            lCheckReceipt.InstitutionName = ComboBoxReceiptCheckAccount.Text;
            Account lAccountDepositedInto = (Account)ComboBoxReceiptCheckAccount.Items[ComboBoxReceiptCheckAccount.SelectedIndex];
            if (lAccountDepositedInto != null)
            {
                Account AccountDepositedInto = AccountManager.GetAccountById(lAccountDepositedInto.Id);
                if (AccountDepositedInto != null)
                {
                    lCheckReceipt.DepositedIntoId = AccountDepositedInto.Id;
                    lCheckReceipt.DepositedInto = AccountDepositedInto;
                }
            }
            return lCheckReceipt;
        }

        private CreditCardReceipt GetCreditCardReceiptFromForm()
        {
            Receipt Receipt = GetReceiptFromForm();

            CreditCardReceipt lCreditCardReceipt = new CreditCardReceipt();
            lCreditCardReceipt.ReceiptDetails = Receipt.ReceiptDetails;
            lCreditCardReceipt.Reference = Receipt.Reference;
            lCreditCardReceipt.ReceiptId = Receipt.ReceiptId;
            lCreditCardReceipt.AccountId = Receipt.AccountId;
            lCreditCardReceipt.TransactionDate = Receipt.TransactionDate;
            lCreditCardReceipt.TransactionType = Receipt.TransactionType;
            lCreditCardReceipt.Amount = Receipt.Amount;
            lCreditCardReceipt.Description = Receipt.Description;
            lCreditCardReceipt.CompanyId = Receipt.CompanyId;
            lCreditCardReceipt.CostCenterId = Receipt.CostCenterId;


            lCreditCardReceipt.CCTransactionDate = (DateTime)DateTimePickerReceiptCreditDate.Date;
            lCreditCardReceipt.CCTransactionNumber = TextBoxReceiptCreditTransaction.Text;

            Account lAccountBank = (Account)ComboBoxReceiptCreditCardAccount.Items[ComboBoxReceiptCreditCardAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lCreditCardReceipt.CCAccountId = AccountBank.Id;
                }
            }
            return lCreditCardReceipt;
        }
        private void TextBoxReceiptAccount_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxReceiptAccount.Id))
            {
                RemoveInvoice();
                Row_Removed();
                LoadReferenceData();
            }
        }

        private void RemoveInvoice()
        {
            if (DataGridViewReceiptDetails.Rows.Count > 1)
            {
                for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count - 1; i++)
                {
                    if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value != null)
                    {
                        DataGridViewReceiptDetails.Rows.RemoveAt(i);
                        RemoveInvoice();
                    }
                }

            }
        }
        private void comboBoxPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxReceiptType.SelectedIndex == 0)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxCashPayment.Visible = true;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Visible = false;

            }
            else if (ComboBoxReceiptType.SelectedIndex == 1)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxCheckInfomation.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = true;
                DateTimePickerReceiptCheckDate.Format = Global.Company.DateFormat;
                DateTimePickerReceiptCheckDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeBankAccountCombo(ComboBoxReceiptCheckAccount, Global.Company.CompanyId);
            }
            else if (ComboBoxReceiptType.SelectedIndex == 2)
            {
                GroupBoxCreditCard.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCreditCard.Visible = true;
                DateTimePickerReceiptCreditDate.Format = Global.Company.DateFormat;
                DateTimePickerReceiptCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeCreditCardAccountCombo(ComboBoxReceiptCreditCardAccount, Global.Company.CompanyId);
            }
            else if (ComboBoxReceiptType.SelectedIndex == 3)
            {
                GroupBoxCashPayment.Visible = false;
                GroupBoxCreditCard.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Location = GroupBoxCashPayment.Location;
                GroupBoxBankTransfer.Visible = true;
                ComboUtils.InitializeBankAccountCombo(ComboBoxReceiptBankAccount, Global.Company.CompanyId);
            }
        }

        private void BtnReceiptNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnReceiptSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxReceiptAccount.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxReceiptAccount.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnReceiptDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxReceiptId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this receipt is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, ReceiptsRefNo.Text), "Delete Confirm",
           MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                long ReceiptID = Convert.ToInt64(TextBoxReceiptId.Text);
                Receipt Receipt = ReceiptManager.GetReceipt(ReceiptID);
                if (Receipt != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool DeleteResult = ReceiptManager.DeleteReceipt(ReceiptID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        BtnReceiptNew.Select();
                        // LoadRecepit();
                        TextBoxReceiptAccount.Select();
                        this.formIsDirty = false;

                    }
                    else
                    {
                        ToolStripStatusLabelErrorReceipt.Text = NotAllowDeleteErrorText;
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                    return;
                }
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            BtnReceiptCancel.PerformClick();
            return;
        }
        private void BtnReceiptPrint_Click(object sender, EventArgs e)
        {
            if (ReceiptManager.GetReceipt(long.Parse(TextBoxReceiptId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxReceiptId.Text), TransactionTypes.RECEIPT);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                return;
            }
        }

        private void BtnReceiptCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxReceiptAccount.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxReceiptAccount.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnReceiptSave_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorReceipt.Text = "";
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Receipt ReceiptFromDB = new Receipt();
                    switch (ComboBoxReceiptType.SelectedIndex)
                    {
                        case 0:
                            Receipt lReceipt = GetReceiptFromForm();

                            if (lReceipt.ReceiptId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.RECEIPT, (DateTime)DateTimePickerReceipt.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lReceipt.Reference = RefNo;
                                    ReceiptFromDB = ReceiptManager.AddReceipt(lReceipt);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorReceipt.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (ReceiptManager.GetReceipt(lReceipt.ReceiptId) != null)
                                {
                                    ReceiptFromDB = ReceiptManager.UpdateReceipt(lReceipt);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                                    return;
                                }
                            }
                            break;
                        case 1:
                            CheckReceipt lCheckReceipt = GetCheckReceiptFromForm();
                            CheckReceipt CheckReceiptFromDB = null;
                            if (lCheckReceipt.ReceiptId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.RECEIPT, (DateTime)DateTimePickerReceipt.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lCheckReceipt.Reference = RefNo;
                                    CheckReceiptFromDB = ReceiptManager.AddCheckReceipt(lCheckReceipt);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorReceipt.Text = RefNoErrorMsg;
                                    return;
                                }
                            }
                            else
                            {
                                if (ReceiptManager.GetReceipt(lCheckReceipt.ReceiptId) != null)
                                {
                                    CheckReceiptFromDB = ReceiptManager.UpdateCheckReceipt(lCheckReceipt);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                                    return;
                                }
                            }
                            ReceiptFromDB.ReceiptId = CheckReceiptFromDB.ReceiptId;

                            break;
                        case 2:
                            CreditCardReceipt lCreditCardReceipt = GetCreditCardReceiptFromForm();
                            CreditCardReceipt CreditCardReceiptFromDB = null;
                            if (lCreditCardReceipt.ReceiptId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.RECEIPT, (DateTime)DateTimePickerReceipt.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lCreditCardReceipt.Reference = RefNo;
                                    CreditCardReceiptFromDB = ReceiptManager.AddCreditCardReceipt(lCreditCardReceipt);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorReceipt.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (ReceiptManager.GetReceipt(lCreditCardReceipt.ReceiptId) != null)
                                {
                                    CreditCardReceiptFromDB = ReceiptManager.UpdateCreditCardReceipt(lCreditCardReceipt);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                                    return;
                                }
                            }
                            ReceiptFromDB.ReceiptId = CreditCardReceiptFromDB.ReceiptId;
                            break;
                        case 3:
                            BankTransferReceipt lBankTransferReceipt = GetBankTransferReceiptFromForm();
                            BankTransferReceipt BankTransferReceiptFromDB = null;
                            if (lBankTransferReceipt.ReceiptId == 0)
                            {
                                string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.RECEIPT, (DateTime)DateTimePickerReceipt.Date);
                                if (!string.IsNullOrEmpty(RefNo))
                                {
                                    lBankTransferReceipt.Reference = RefNo;
                                    BankTransferReceiptFromDB = ReceiptManager.AddBankTransferReceipt(lBankTransferReceipt);
                                }
                                else
                                {
                                    ToolStripStatusLabelErrorReceipt.Text = RefNoErrorMsg;
                                }
                            }
                            else
                            {
                                if (ReceiptManager.GetReceipt(lBankTransferReceipt.ReceiptId) != null)
                                {
                                    BankTransferReceiptFromDB = ReceiptManager.UpdateBankTransferReceipt(lBankTransferReceipt);
                                }
                                else
                                {
                                    DisplaySystemError("Somting went wrong, please check this receipt is still valid.");
                                    return;
                                }
                            }
                            ReceiptFromDB.ReceiptId = BankTransferReceiptFromDB.ReceiptId;
                            break;
                    }
                    LoadReceipt(ReceiptFromDB.ReceiptId);
                    ToolStripStatusLabelErrorReceipt.Text = SaveSuccessText;
                    this.formIsDirty = false;
                }
                catch (Exception ex)
                {
                    ToolStripStatusLabelErrorReceipt.Text = ex.Message;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxReceiptAccount.Id))
            {
                TextBoxReceiptAccount.Select();
                ToolStripStatusLabelErrorReceipt.Text = ChooseCustomerErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxReceiptAccount.Id) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxReceiptAccount.Id)) == null)
            {
                TextBoxReceiptAccount.Select();
                ToolStripStatusLabelErrorReceipt.Text = "Somting went wrong, please check this account is still valid.";
                ResetTimmer();
                return false;
            }
            if (DateTimePickerReceipt.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerReceipt.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerReceipt.Focus();
                ToolStripStatusLabelErrorReceipt.Text = EnterReceiptDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (ComboBoxReceiptType.SelectedIndex < 0)
            {
                ComboBoxReceiptType.Select();
                ToolStripStatusLabelErrorReceipt.Text = ChooseReceiptTypeErrorMsg;
                ResetTimmer();
                return false;
            }
            if (float.Parse(TextBoxReceiptAmount.Text) == 0)
            {
                TextBoxReceiptAmount.Select();
                ToolStripStatusLabelErrorReceipt.Text = EnterAmountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && ComboBoxReceiptBankAccount.SelectedIndex < 0)
            {
                ComboBoxReceiptBankAccount.Select();
                ToolStripStatusLabelErrorReceipt.Text = ChooseBankAccountErrorMsg;
                ResetTimmer();
                return false;

            }
            if (GroupBoxBankTransfer.Visible == true && string.IsNullOrEmpty(TextBoxReceiptBankTransaction.Text.Trim()))
            {
                TextBoxReceiptBankTransaction.Select();
                ToolStripStatusLabelErrorReceipt.Text = EnterBankTranErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && string.IsNullOrEmpty(TextBoxReceiptCheckDocument.Text.Trim()))
            {
                TextBoxReceiptCheckDocument.Select();
                ToolStripStatusLabelErrorReceipt.Text = EnterCheckDocErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && (DateTimePickerReceiptCheckDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerReceiptCheckDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerReceiptCheckDate.Focus();
                ToolStripStatusLabelErrorReceipt.Text = EnterCheckDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && ComboBoxReceiptCheckAccount.SelectedIndex < 0)
            {
                ComboBoxReceiptCheckAccount.Select();
                ToolStripStatusLabelErrorReceipt.Text = ChooseCheckAccountErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCreditCard.Visible == true && ComboBoxReceiptCreditCardAccount.SelectedIndex < 0)
            {
                ComboBoxReceiptCreditCardAccount.Select();
                ToolStripStatusLabelErrorReceipt.Text = ChooseCardAccountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && TextBoxReceiptCreditTransaction.Text.Replace(" ", "") == "")
            {
                TextBoxReceiptCreditTransaction.Select();
                ToolStripStatusLabelErrorReceipt.Text = EnterCardTranErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCreditCard.Visible == true && (DateTimePickerReceiptCreditDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerReceiptCreditDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerReceiptCreditDate.Focus();
                ToolStripStatusLabelErrorReceipt.Text = EnterCardDateErrorMsg;
                ResetTimmer();
                return false;
            }

            int Count = DataGridViewReceiptDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        if (!DataGridViewReceiptDetails.Rows[i].Cells[j].ReadOnly &&
                            isRequiredColumn(j) &&
                            (DataGridViewReceiptDetails.Rows[i].Cells[j].Value == null || DataGridViewReceiptDetails.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision))))
                        {
                            DataGridViewReceiptDetails.Select();
                            DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[j, i];
                            DataGridViewReceiptDetails.BeginEdit(true);
                            ToolStripStatusLabelErrorReceipt.Text = RequiredColumnsValidationMsg[j];
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value != null && float.Parse(DataGridViewReceiptDetails.Rows[i].Cells[3].Value.ToString()) > 0)
                    {
                        decimal Balance = decimal.Parse(DataGridViewReceiptDetails.Rows[i].Cells[3].Value.ToString());
                        if (DataGridViewReceiptDetails.Rows[i].Cells[8].Value != null)
                        {
                            ReceiptDetail ReceiptDetail = ReceiptManager.GetReceiptDetail((long)DataGridViewReceiptDetails.Rows[i].Cells[8].Value);
                            if (ReceiptDetail != null)
                            {
                                Balance += ReceiptDetail.Amount;
                            }
                        }
                        decimal Amount = decimal.Parse(DataGridViewReceiptDetails.Rows[i].Cells[4].Value.ToString());
                        if (Balance < Amount)
                        {
                            DataGridViewReceiptDetails.Select();
                            DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[4, i];
                            ToolStripStatusLabelErrorReceipt.Text = RequiredColumnsValidationMsg[5];
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (float.Parse(DataGridViewReceiptDetails.Rows[i].Cells[2].Value.ToString()) > 0 && float.Parse(DataGridViewReceiptDetails.Rows[i].Cells[3].Value.ToString()) == 0)
                    {
                        decimal Balance = decimal.Parse(DataGridViewReceiptDetails.Rows[i].Cells[3].Value.ToString());
                        if (DataGridViewReceiptDetails.Rows[i].Cells[8].Value != null)
                        {
                            ReceiptDetail ReceiptDetail = ReceiptManager.GetReceiptDetail((long)DataGridViewReceiptDetails.Rows[i].Cells[8].Value);
                            if (ReceiptDetail != null)
                            {
                                Balance += ReceiptDetail.Amount;
                            }
                        }
                        decimal Amount = decimal.Parse(DataGridViewReceiptDetails.Rows[i].Cells[4].Value.ToString());
                        if (Balance < Amount)
                        {
                            DataGridViewReceiptDetails.Select();
                            DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[4, i];
                            ToolStripStatusLabelErrorReceipt.Text = RequiredColumnsValidationMsg[5];
                            ResetTimmer();
                            return false;
                        }
                    }
                    if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value == null)
                    {
                        for (int k = 0; k < Count - 1; k++)
                        {
                            if (DataGridViewReceiptDetails.Rows[k].Cells[6].Value != null && float.Parse(DataGridViewReceiptDetails.Rows[i].Cells[3].Value.ToString()) > 0)
                            {
                                float Balance = float.Parse(DataGridViewReceiptDetails.Rows[k].Cells[3].Value.ToString());
                                float Amount = float.Parse(DataGridViewReceiptDetails.Rows[k].Cells[4].Value.ToString());
                                if (Balance > Amount)
                                {
                                    DataGridViewReceiptDetails.Select();
                                    DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[4, i];
                                    ToolStripStatusLabelErrorReceipt.Text = RequiredColumnsValidationMsg[6];
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
                DataGridViewReceiptDetails.Select();
                DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[1, 0];
                DataGridViewReceiptDetails.BeginEdit(true);
                ToolStripStatusLabelErrorReceipt.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            ToolStripStatusLabelErrorReceipt.Text = SaveSuccessText;
            DataGridViewReceiptDetails.CurrentCell.Style.SelectionBackColor = Color.White;
            if (double.Parse(TextBoxReceiptAmount.Text) != double.Parse(DataGridViewReceiptDetailsTotal.Rows[0].Cells[1].Value.ToString()))
            {
                TextBoxReceiptAmount.Select();
                ToolStripStatusLabelErrorReceipt.Text = EnterAmountWrongDateErrorMsg;
                ResetTimmer();
                return false;
            }
            return true;
        }

        private void ResetForm()
        {
            TextBoxSearchReceipt.TextBox.ResetText();
            ToolStripStatusLabelErrorReceipt.Text = "";
            TimerReceipts.Stop();
            DataGridViewOpenInvoices.Rows.Clear();
            DataGridViewReceipt.Rows.Clear();
            GroupBoxCreditCard.Visible = false;
            GroupBoxCheckInfomation.Visible = false;
            GroupBoxBankTransfer.Visible = false;
            DateTimePickerReceipt.Format = Global.Company.DateFormat;
            DateTimePickerReceipt.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerReceipt.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerReceipt.MaxDate = Global.getCurrentFiscalYearEndDate();
            ToolStripStatusLabelErrorReceipt.Text = "";
            TextBoxReceiptAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxReceiptId.ResetText();
            TextBoxReceiptMemo.ResetText();
            ReceiptsRefNo.Text = "000000";
            TextBoxReceiptCheckDocument.ResetText();
            TextBoxReceiptBankTransaction.ResetText();
            TextBoxReceiptAccount.ResetText();
            ComboBoxReceiptBankAccount.ResetText();
            ComboBoxReceiptBankAccount.SelectedIndex = -1;
            ComboBoxReceiptType.SelectedIndex = 0;
            ComboBoxReceiptCheckAccount.ResetText();
            ComboBoxReceiptCheckAccount.SelectedIndex = -1;
            DateTimePickerReceiptCreditDate.Format = Global.Company.DateFormat;
            DateTimePickerReceiptCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DateTimePickerReceiptCreditDate.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerReceiptCreditDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            ComboBoxReceiptCreditCardAccount.ResetText();
            ComboBoxReceiptCreditCardAccount.SelectedIndex = -1;
            TextBoxReceiptCreditTransaction.ResetText();
            LastReceiptsRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.RECEIPT, (DateTime)DateTimePickerReceipt.Date!);
            DataGridViewReceiptDetails.Rows.Clear();
            DataGridViewReceiptDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewReceiptDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewReceiptDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)DataGridViewReceiptDetails.Columns["InvAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)DataGridViewReceiptDetails.Columns["InvBalance"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxReceiptType.Visible = true;

            if (enable)
            {
                BtnReceiptNew.Enabled = !enable;
                BtnReceiptDelete.Enabled = !enable;
                BtnReceiptPrint.Enabled = !enable;
                BtnReceiptCancel.Enabled = enable;
                BtnReceiptSave.Enabled = enable;
            }
            else
            {
                BtnReceiptNew.Enabled = !enable;
                BtnReceiptDelete.Enabled = !enable;
                BtnReceiptPrint.Enabled = !enable;
                BtnReceiptCancel.Enabled = !enable;
                BtnReceiptSave.Enabled = !enable;
            }
        }

        private void DataGridViewReceiptDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5 && !DataGridViewReceiptDetails.Rows[e.RowIndex].IsNewRow)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewReceiptDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[6].Value != null)
                        {
                            for (int i = 0; i <= DataGridViewOpenInvoices.Rows.Count - 1; i++)
                            {
                                if (DataGridViewOpenInvoices.Rows[i].Cells[7].Value.ToString() == DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[6].Value.ToString()
                                    && DataGridViewOpenInvoices.Rows[i].Cells[6].Value.ToString() == DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[7].Value.ToString())
                                {
                                    DataGridViewOpenInvoices.Rows[i].Cells[0].Value = false;
                                    break;
                                }
                            }
                        }
                        DataGridViewReceiptDetails.Rows.RemoveAt(e.RowIndex);
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

        private void DataGridViewReceiptDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 4)
            {
                if (DataGridViewReceiptDetails.CurrentCell.Value == null || DataGridViewReceiptDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewReceiptDetails.CurrentCell.Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                }
                else
                {
                    DataGridViewReceiptDetails.CurrentCell.Value = decimal.Parse(DataGridViewReceiptDetails.CurrentCell.Value.ToString()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    this.formIsDirty = false;
                }
                Row_Added();
            }
        }

        private void Row_Added()
        {
            ComputeFormTotal();

        }

        private void DataGridViewReceiptDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{tab}");
            }
            DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            if (DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[5].Value == null)
            {
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            }
            if (DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[1].Value != null &&
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[5].Value == null)
            {
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        private void DataGridViewReceiptDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;

        private void DataGridViewReceiptDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                if (DataGridViewReceiptDetails.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewReceipt_KeyPress1);
            }
            if (DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 4)
            {
                KeypressValidation.AddContextMenuGridCell(e, DataGridViewReceiptDetails, "NumberDot", DataGridViewReceiptDetails.CurrentCell.ColumnIndex);

                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewReceiptDetails_KeyPress);
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown += DataGridViewReceiptDetails_KeyDown;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void DataGridViewReceiptDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
            if (Account != null)
            {
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewReceiptDetails.Rows.Count;
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[4].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                DataGridViewReceiptDetails.Rows[e.RowIndex].Cells[5].Value = "X";

            }
        }
        private void DataGridViewReceipt_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewReceiptDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewReceiptDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewReceiptDetails.Rows.Count > 0 && DataGridViewReceiptDetails.CurrentCell != null)
            {
                if (DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 4)
                {
                    //if (DataGridViewReceiptDetails.CurrentCell.Value != null)
                    //{
                    //    KeypressValidation.Keypress_NumberDot(sender, e, DataGridViewReceiptDetails.CurrentCell.Value.ToString());
                    //}
                    //else
                    //{
                    //    KeypressValidation.Keypress_Number(sender, e);
                    //}
                }
            }
        }
        private void DataGridViewReceiptDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }
        }
        private void TextBoxReceiptAmount_Leave(object sender, EventArgs e)
        {
            RedistributeTotalAmount();
        }

        private void TextBoxReceiptBankTransaction_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxReceiptCheckDocument_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxReceiptCheckBankName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxReceiptCreditCardName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        //private void ComboBoxReceiptAccount_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxReceiptAccount.DroppedDown = false;
        //}
        private void ComboBoxReceiptType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxReceiptType.DroppedDown = false;
        }
        private void ComboBoxReceiptBankAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxReceiptBankAccount.DroppedDown = false;
        }
        private void ComboBoxReceiptCheckAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxReceiptCheckAccount.DroppedDown = false;
        }

        //private void ComboBoxReceiptCreditCardType_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxReceiptCreditCardType.DroppedDown = false;
        //}
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnReceiptSearchCustomer.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnReceiptNew.Enabled)
                {
                    BtnReceiptNew.PerformClick();
                }
                else
                {
                    if (TextBoxReceiptAccount.Focused)
                    {
                        BtnReceiptNewCustomers.ShowDropDown();
                    }
                }
            }
            else if (keyData == (Keys.F4))
            {
                if (BtnReceiptDelete.Enabled)
                {
                    BtnReceiptDelete.PerformClick();
                }
            }
            else if (keyData == (Keys.F9))
            {
                BtnReceiptPrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnReceiptSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReceiptCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnReceiptExit.PerformClick();
                return true;
            }
            if (keyData == (Keys.Tab) && BtnSearchReceipt.Selected)
            {
                TextBoxReceiptAccount.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnReceiptSave)
            {
                BtnReceiptCancel.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnReceiptSave)
            {
                BtnReceiptExit.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnReceiptCancel)
            {
                BtnReceiptExit.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnReceiptCancel)
            {
                BtnReceiptSave.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnReceiptExit)
            {
                BtnReceiptCancel.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnReceiptExit)
            {
                BtnReceiptSave.Select();
                return true;
            }
            try
            {
                if (DataGridViewReceiptDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 1)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 4)
                    {
                        if (DataGridViewReceiptDetails.CurrentCell.RowIndex != DataGridViewReceiptDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 4)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewReceiptDetails.CurrentCell.ColumnIndex == 1)
                    {
                        if (DataGridViewReceiptDetails.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            TextBoxReceiptMemo.Focus();
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
            TimerReceipts.Stop();
            TimerReceipts.Start();
        }
        private void TimerLableBLink_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorReceipt.Visible = !this.ToolStripStatusLabelErrorReceipt.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerReceipts.Stop();
                ToolStripStatusLabelErrorReceipt.Visible = true;
            }
        }
        private void TextBoxReceiptMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxReceiptMemo.SelectionLength = 0;
                e.IsInputKey = true;
                if (GroupBoxCashPayment.Visible == true)
                {
                    DataGridViewReceiptDetails.Select();
                    DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[0, 0];
                }
                else if (GroupBoxCheckInfomation.Visible == true) { TextBoxReceiptCheckDocument.Select(); }
                else if (GroupBoxCreditCard.Visible == true) { ComboBoxReceiptCreditCardAccount.Select(); }
                else if (GroupBoxBankTransfer.Visible == true) { ComboBoxReceiptBankAccount.Select(); }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxReceiptMemo.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxReceiptAmount.Select();
            }
        }
        private void ComboBoxReceiptCheckAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewReceiptDetails.Select();
                DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[0, 0];
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerReceiptCheckDate.Focus();
            }
        }
        private void DateTimePickerReceiptCreditDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewReceiptDetails.Select();
                DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxReceiptCreditTransaction.Focus();
            }
        }
        private void TextBoxReceiptCreditCVV_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
        private void TextBoxReceiptBankTransaction_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewReceiptDetails.Select();
                DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxReceiptBankAccount.Select();
            }
        }

        private void BtnReceiptCancel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnReceiptSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewReceiptDetails.Rows.Count > 0)
                {
                    DataGridViewReceiptDetails.Select();
                    DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[3, DataGridViewReceiptDetails.Rows.Count - 1];
                }
            }
        }

        private void DataGridViewOpenInvoices_CellClick(object sender, DataGridViewCellEventArgs arg)
        {
            try
            {
                if (arg.ColumnIndex == 0 && arg.RowIndex > -1)
                {
                    //Vincent
                    int Count = DataGridViewReceiptDetails.Rows.Count - 1;
                    DataGridViewOpenInvoices.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataGridViewCheckBoxCell Cell = DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
                    if (DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[0].Value.ToString() == "False")
                    {
                        DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[0].Value = true;
                        DataGridViewReceiptDetails.Rows.Add(1);
                        for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count; i++)
                        {
                            DataGridViewReceiptDetails.Rows[i].Cells[0].Value = i + 1;
                        }

                        if (DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[1].Value != null)
                        {
                            var InvoiceId = Convert.ToInt64(DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[7].Value);
                            string TransactionType = (string)DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[6].Value;
                            if (TransactionType != null)
                            {
                                switch (TransactionType)
                                {
                                    case "Invoice":
                                        Invoice SelectedInvoice = InvoiceManager.GetInvoice(InvoiceId);
                                        if (SelectedInvoice != null)
                                        {
                                            float FormBalance = GetFormBalance();
                                            DataGridViewReceiptDetails.Rows[Count].Cells[7].Value = TransactionType;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[6].Value = SelectedInvoice.InvoiceId;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[1].Value = "Reference to Invoice #: " + SelectedInvoice.ReferenceNumber;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[2].Value = SelectedInvoice.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            DataGridViewReceiptDetails.Rows[Count].Cells[3].Value = SelectedInvoice.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            if (FormBalance > 0)
                                            {
                                                if (FormBalance >= SelectedInvoice.Balance)
                                                {
                                                    DataGridViewReceiptDetails.Rows[Count].Cells[4].Value = SelectedInvoice.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                                else
                                                {
                                                    DataGridViewReceiptDetails.Rows[Count].Cells[4].Value = FormBalance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                            }
                                            Row_Added();
                                            DataGridViewReceiptDetails.Rows[Count].Cells[1].ReadOnly = false;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[2].ReadOnly = true;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[3].ReadOnly = true;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[4].ReadOnly = false;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[5].ReadOnly = true;
                                            DataGridViewReceiptDetails.Select();
                                            DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[1, Count + 1];

                                        }
                                        else
                                        {
                                            throw new Exception("Could not load the Invoice");
                                        }
                                        break;
                                    case "Sale":
                                        SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(InvoiceId);
                                        if (SaleEntry != null)
                                        {
                                            float FormBalance = GetFormBalance();
                                            DataGridViewReceiptDetails.Rows[Count].Cells[7].Value = TransactionType;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[6].Value = SaleEntry.Id;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[1].Value = "Reference to Sale #: " + SaleEntry.Id;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[2].Value = SaleEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            DataGridViewReceiptDetails.Rows[Count].Cells[3].Value = SaleEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            if (FormBalance > 0)
                                            {
                                                if (FormBalance >= SaleEntry.Balance)
                                                {
                                                    DataGridViewReceiptDetails.Rows[Count].Cells[4].Value = SaleEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                                else
                                                {
                                                    DataGridViewReceiptDetails.Rows[Count].Cells[4].Value = FormBalance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                                }
                                            }
                                            Row_Added();
                                            DataGridViewReceiptDetails.Rows[Count].Cells[1].ReadOnly = false;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[2].ReadOnly = true;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[3].ReadOnly = true;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[4].ReadOnly = false;
                                            DataGridViewReceiptDetails.Rows[Count].Cells[5].ReadOnly = true;
                                            DataGridViewReceiptDetails.Select();
                                            DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[1, Count + 1];

                                        }
                                        else
                                        {
                                            throw new Exception("Could not load the Sale");
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
                    else if (DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[0].Value.ToString() == "True")
                    {
                        DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[0].Value = false;
                        for (int i = 0; i <= Count; i++)
                        {
                            if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value != null)
                            {
                                if (DataGridViewReceiptDetails.Rows[i].Cells[6].Value.ToString() == DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[7].Value.ToString()
                                    && DataGridViewReceiptDetails.Rows[i].Cells[7].Value.ToString() == DataGridViewOpenInvoices.Rows[arg.RowIndex].Cells[6].Value.ToString())
                                {
                                    DataGridViewReceiptDetails.Rows.RemoveAt(i);
                                    Row_Removed();
                                    DataGridViewReceiptDetails.Select();
                                    DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[1, Count - 1];
                                    return;
                                }
                            }
                        }
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception e)
            {
                MessageBox.Show("Technical Error, Please contact your provider", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#pragma warning restore 0168
        }
        private void TextBoxReceiptMemo_KeyPress(object sender, KeyPressEventArgs e)
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

                float FormTotal = (float)Convert.ToDouble(TextBoxReceiptAmount.Text);
                float FormLineItemTotal = (float)Convert.ToDouble(DataGridViewReceiptDetailsTotal.Rows[0].Cells[1].Value);
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
            for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count - 1; i++)
            {
                float LineItemTotal = (float)GetLineItemTotal(i);
                if (DataGridUtils.readColumValueAsFloat(DataGridViewReceiptDetails, i, 4) == 0 && TotalAmount > 0 && !DataGridViewReceiptDetails.Rows[i].IsNewRow)
                {
                    float AmountToApply = (TotalAmount > LineItemTotal ? LineItemTotal : TotalAmount);
                    DataGridViewReceiptDetails.Rows[i].Cells[4].Value = AmountToApply.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    TotalAmount = TotalAmount - LineItemTotal;
                }
            }
            ComputeFormTotal();
        }

        private void ComputeFormTotal()
        {
            double TotalAmount = 0;
            for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count - 1; i++)
            {
                double Amount = (DataGridViewReceiptDetails.Rows[i].Cells[4].Value) == null ? 0.00 : (double.Parse(DataGridViewReceiptDetails.Rows[i].Cells[4].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewReceiptDetailsTotal.Rows[0].Cells[1].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }

        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewReceiptDetails.Rows.Count; i++)
            {
                DataGridViewReceiptDetails.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private float GetReceiptTotalAmount()
        {
            try
            {
                return (float)Convert.ToDouble(TextBoxReceiptAmount.Text);
            }
#pragma warning disable 0168 // variable declared but not used.
            catch (Exception e)
            {
                return 0F;
            }
        }

        private double GetLineItemTotal(int row)
        {
            try
            {
                return (DataGridViewReceiptDetails.Rows[row].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewReceiptDetails.Rows[row].Cells[3].Value.ToString()));
            }
            catch (Exception e)
            {
                throw e;
            }
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

        private void DataGridViewReceipt_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
                            BtnReceiptSave_Click(sender, e);
                            LoadReceipt((long)DataGridViewReceipt.Rows[e.RowIndex].Cells[5].Value);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadReceipt((long)DataGridViewReceipt.Rows[e.RowIndex].Cells[5].Value);
                    }

                }
                else
                {
                    LoadReceipt((long)DataGridViewReceipt.Rows[e.RowIndex].Cells[5].Value);
                }
            }
        }


        private void LoadReceipt(long ReceiptId)
        {
            ResetForm();
            Receipt Receipt = ReceiptManager.GetReceipt(ReceiptId);
            if (Receipt != null)
            {
                ReceiptsRefNo.Text = Receipt.Reference;
                TextBoxReceiptId.Text = Receipt.ReceiptId.ToString();
                //ComboBoxReceiptAccount.SelectedIndex = ComboBoxReceiptAccount.FindStringExact(Receipt.Account.Name);
                TextBoxReceiptAccount.Id = Receipt.AccountId.ToString();
                TextBoxReceiptAccount.Text = Receipt.Account.Name;
                DateTimePickerReceipt.Date = (DateTime)DateUtils.ToDate(Receipt.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxReceiptType.SelectedIndex = ComboBoxReceiptType.FindStringExact(((PaymentType)Receipt.TransactionType).ToString());
                TextBoxReceiptMemo.Text = Receipt.Description;
                TextBoxReceiptAmount.Text = Receipt.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                SearchReceiptId = 0L;
                MessageBox.Show("Somting went wrong, please check this receipt is still valid.");
                return;
            }
            if (Receipt.TransactionType == PaymentType.BANKTRANSFER)
            {
                BankTransferReceipt BankTransferReceipt = ReceiptManager.GetBankTransferReceipt(ReceiptId);
                ComboBoxReceiptBankAccount.SelectedIndex = ComboBoxReceiptBankAccount.FindStringExact(BankTransferReceipt.BankTransfer.Name);
                TextBoxReceiptBankTransaction.Text = BankTransferReceipt.TransactionNumber;
            }
            if (Receipt.TransactionType == PaymentType.CHECK)
            {
                CheckReceipt CheckReceipt = ReceiptManager.GetCheckReceipt(ReceiptId);
                TextBoxReceiptCheckDocument.Text = CheckReceipt.DocumentNumber;
                DateTimePickerReceiptCheckDate.Date = (DateTime)DateUtils.ToDate(CheckReceipt.DocumentDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxReceiptCheckAccount.SelectedIndex = ComboBoxReceiptCheckAccount.FindStringExact(CheckReceipt.DepositedInto.Name);

            }
            if (Receipt.TransactionType == PaymentType.CREDITCARD)
            {
                CreditCardReceipt CreditCardReceipt = ReceiptManager.GetCreditCardReceipt(ReceiptId);
                ComboBoxReceiptCreditCardAccount.SelectedIndex = ComboBoxReceiptCreditCardAccount.FindStringExact(((Account)CreditCardReceipt.CCAccount).ToString());

                DateTimePickerReceiptCreditDate.Date = (DateTime)DateUtils.ToDate(CreditCardReceipt.CCTransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxReceiptCreditTransaction.Text = CreditCardReceipt.CCTransactionNumber;

            }
            if (Receipt.ReceiptDetails.Count > 0)
            {
                DataGridViewReceiptDetails.Rows.Add(Receipt.ReceiptDetails.Count);
                int i = 0;
                foreach (var ReceiptDetail in Receipt.ReceiptDetails)
                {
                    DataGridViewReceiptDetails.Rows[i].Cells[8].Value = ReceiptDetail.ReceiptDetailId;
                    DataGridViewReceiptDetails.Rows[i].Cells[0].Value = i + 1;
                    DataGridViewReceiptDetails.Rows[i].Cells[1].Value = ReceiptDetail.Description;
                    DataGridViewReceiptDetails.Rows[i].Cells[2].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                    DataGridViewReceiptDetails.Rows[i].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                    if (!string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
                    {
                        for (int j = 0; j < DataGridViewOpenInvoices.Rows.Count; j++)
                        {
                            if ((long)DataGridViewOpenInvoices.Rows[j].Cells[7].Value == long.Parse(ReceiptDetail.ReferenceTrasnactionId))
                            {
                                DataGridViewOpenInvoices.Rows[j].Cells[0].Value = true;

                            }
                        }
                        if (ReceiptDetail.InvoiceType == InvoiceType.Basic)
                        {
                            Invoice Invoice = InvoiceManager.GetInvoice(long.Parse(ReceiptDetail.ReferenceTrasnactionId));
                            if (Invoice != null)
                            {
                                DataGridViewReceiptDetails.Rows[i].Cells[2].Value = Invoice.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewReceiptDetails.Rows[i].Cells[3].Value = Invoice.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewReceiptDetails.Rows[i].Cells[6].Value = Invoice.InvoiceId;
                                DataGridViewReceiptDetails.Rows[i].Cells[7].Value = "Invoice";
                            }
                        }
                        else
                        {
                            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(long.Parse(ReceiptDetail.ReferenceTrasnactionId));
                            if (SaleEntry != null)
                            {
                                DataGridViewReceiptDetails.Rows[i].Cells[2].Value = SaleEntry.TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewReceiptDetails.Rows[i].Cells[3].Value = SaleEntry.Balance.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                DataGridViewReceiptDetails.Rows[i].Cells[6].Value = SaleEntry.Id;
                                DataGridViewReceiptDetails.Rows[i].Cells[7].Value = "Sale";
                            }
                        }
                    }
                    DataGridViewReceiptDetails.Rows[i].Cells[4].Value = ReceiptDetail.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewReceiptDetails.Rows[i].Cells[5].Value = "X";
                    i++;
                }
                Row_Added();
                ReSequence();
            }

            EnableForm(false);
            BtnReceiptSave.Select();
            this.formIsDirty = false;
        }


        private void BtnReceiptSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxReceiptAccount.Select();
            }

            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewReceiptDetails.Rows.Count > 0)
                {
                    DataGridViewReceiptDetails.Select();
                    DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[4, DataGridViewReceiptDetails.Rows.Count - 1];
                }
            }
        }
        private void TextBoxReceiptAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerReceipt.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnReceiptSave.Select();
            }
        }
        //private void ComboBoxReceiptAccount_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{

        //}

        private void BtnReceiptExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormReceipts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                    TextBoxReceiptAccount.Select();
                }
            }
        }

        private void BtnSearchReceipt_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (isValidSearchCriteria())
            {
                RecentReceipt();
                if (SearchReceiptId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnReceiptSave_Click(sender, e);
                                LoadReceipt(SearchReceiptId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadReceipt(SearchReceiptId);
                        }
                    }
                    else
                    {
                        LoadReceipt(SearchReceiptId);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void RecentReceipt()
        {
            ToolStripStatusLabelErrorReceipt.Text = "";
            String SearchText = TextBoxSearchReceipt.Text.Trim();
            IList<Receipt> ReceiptInfo = null;

            if (TextUtils.isAmount(SearchText))
            {
                ReceiptInfo = ReceiptManager.GetReceiptByAmount(float.Parse(SearchText), SearchText, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                ReceiptInfo = ReceiptManager.GetReceiptByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                ReceiptInfo = ReceiptManager.GetReceiptByCustomerName(SearchText, Global.Company.CompanyId);
            }
            if (ReceiptInfo.Count > 0)
            {
                LoadReceipt(ReceiptInfo);
            }
            else
            {
                SearchReceiptId = 0L;
                ToolStripStatusLabelErrorReceipt.Text = SearchOutput;
            }
        }
        public void LoadReceipt(IList<Receipt> ReceiptInfo)
        {
            SearchReceiptId = 0L;
            FormRecentPayment FormRecentPayment = new FormRecentPayment(this);
            FormRecentPayment.Text = "Recent Receipt";
            FormRecentPayment.ReceiptInfo = ReceiptInfo;
            FormRecentPayment.ShowDialog();
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorReceipt.Text = "";
            if (string.IsNullOrEmpty(TextBoxSearchReceipt.Text.Trim()))
            {
                ToolStripStatusLabelErrorReceipt.Text = SearchBoxEmptyErrorMsg;
                TextBoxSearchReceipt.TextBox.Focus();
                return false;
            }
            return true;
        }

        private void TextBoxSearchReceipt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchReceipt_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnSearchReceipt.PerformClick();
            }
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                CustomerId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnReceiptSearchCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxReceiptAccount.Id == null ? 0L : long.Parse(TextBoxReceiptAccount.Id);
            CustomerId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = true;
            FormSearchAccount.IncludeGeneralAccounts = true;
            FormSearchAccount.ShowDialog();
            if (CustomerId != 0)
            {
                Account customer = AccountManager.Instance.GetAccountById(CustomerId);
                if (customer != null)
                {
                    TextBoxReceiptAccount.ResetText();
                    TextBoxReceiptAccount.Id = CustomerId.ToString();
                    TextBoxReceiptAccount.Text = customer.Name;
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }

            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                }
            }
            TextBoxReceiptAccount.Select();
            Cursor.Current = Cursors.Default;
        }

        private void DataGridViewReceiptDetails_Leave(object sender, EventArgs e)
        {
            if (DataGridViewReceiptDetails.CurrentRow != null)
            {
                DataGridViewReceiptDetails.CurrentCell = DataGridViewReceiptDetails[2, DataGridViewReceiptDetails.CurrentRow.Index];
            }
        }

        private void ComboBoxReceiptCreditCardAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxReceiptCreditCardAccount.DroppedDown = false;
        }

        private void BtnPaymentNewCustomers_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxReceiptAccount.Id == null ? 0L : long.Parse(TextBoxReceiptAccount.Id);
            CustomerId = 0L;
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
            if (CustomerId != 0)
            {
                Account Account = AccountManager.Instance.GetAccountById(CustomerId);
                if (Account != null)
                {
                    TextBoxReceiptAccount.Text = Account.Name;
                    TextBoxReceiptAccount.Id = CustomerId.ToString();
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("Somting went wrong, please check this account is still valid.");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                    this.formIsDirty = false;
                }
            }
            TextBoxReceiptAccount.Select();
            Cursor.Current = Cursors.Default;
        }
    }
}
