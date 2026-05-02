using fa.api.Accounting;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Hms.common;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using FADataAccessLibrary.Api.Accounting;
using FADataAccessLibrary.Model.Accounting.Transactions;
using Pango;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Timer = System.Windows.Forms.Timer;

namespace fa.views.sales
{
    public enum GridPendingInvColumn
    {
        DATE, REFNO, AMOUNT, ID
    }
    public partial class FormPOSReceivePayment : FormBase
    {
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Sales Date or Sales Number.";
        public static string SearchOutput = "No Entry Found!";
        public static string ChooseBankAccountErrorMsg = "Please choose Bank Account";
        public static string EnterBankTranErrorMsg = "Please enter Transaction";
        public static string EnterCheckDocErrorMsg = "Please enter document";
        public static string EnterCheckDateErrorMsg = "Please enter proper Check Date";
        public static string ChooseCheckAccountErrorMsg = "Please choose check Account";
        public static string EnterAmountErrorMsg = "Please enter Amount";
        public static string EnterAmountWrongErrorMsg = "Please enter proper Amount";

        public static string EnterCardTranErrorMsg = "Please enter credir card transaction details";
        public static string ChooseCardAccountErrorMsg = "Please choose account";
        public static string EnterCardDateErrorMsg = "Please enter credit card Date";

        public static string Grid_NoPendingInvoice = "No Pending Invoice Available..";
        public double EditableAmount = 0.00;

        PaymentManager PaymentManager = null!;
        public long SearchSalesId = 0L;
        public bool SalePaymentOnLoad = false;
        public long AccountID = 0L;

        public FormPOSReceivePayment()
        {
            PaymentManager = PaymentManager.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "VVMToolStrip" };

        }

        private void FormPOSReceivePayment_Load(object sender, EventArgs e)
        {
            ResetForm();
            if (SalePaymentOnLoad)
            {
                EnableForm(true);
                CheckPaymentOnload();
                if (!TextBoxCashAmount.ReadOnly) { TextBoxCashAmount.Select(); }
            }
            else
            {
                EnableForm(false);
                LoadPendingInvoice();
            }
            this.formIsDirty = false;
        }
        private void CheckPaymentOnload()
        {
            label1.Visible = false;
            GridViewPendingInvoice.Visible = false;
            VVMToolStrip.Visible = false;
            TabControlInvoiceDetails.Location = new Point(GridViewPendingInvoice.Location.X, (GridViewPendingInvoice.Location.Y - 40));
            GroupBoxPayMethod.Location = new Point(GridViewPendingInvoice.Location.X, (TabControlInvoiceDetails.Location.Y + TabControlInvoiceDetails.Height + 10));
            GroupBoxCashPayment.Location = new Point(GridViewPendingInvoice.Location.X, (GroupBoxPayMethod.Location.Y + GroupBoxPayMethod.Height + 10));
            BtnReceiveDeliver.Location = new Point(GroupBoxCashPayment.Width - 70, BtnReceiveDeliver.Location.Y);
            BtnReceive.Location = new Point(GroupBoxCashPayment.Width - 165, BtnReceiveDeliver.Location.Y);
            BtnCancel.Location = new Point(GroupBoxCashPayment.Width - 250, BtnReceiveDeliver.Location.Y);
            this.Size = new Size(TabControlInvoiceDetails.Width + 40, this.Height);
            this.CenterToParent();
            LoadPaymentDetails(SearchSalesId);
        }
        private void LoadPendingInvoice()
        {
            ErrorMsg.Text = string.Empty;
            GridViewPendingInvoice.Rows.Clear();
            List<SaleEntry> SaleEntrys = SalesManager.Instance.GetSaleRecentPaymentByCompanyId(Global.Company.CompanyId);
            if (SaleEntrys != null && SaleEntrys.Count > 0)
            {
                GridViewPendingInvoice.Rows.Add(SaleEntrys.Count);
                int i = 0;
                foreach (SaleEntry Entry in SaleEntrys)
                {
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.DATE].Value = Entry.SaleDate.ToString(Global.Company.DateFormat);
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.REFNO].Value = Entry.RefNumber;
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.AMOUNT].Value = Entry.TotalAmount;
                    GridViewPendingInvoice.Rows[i].Cells[(int)GridPendingInvColumn.ID].Value = Entry.Id;
                    i++;
                }
                EnableForm(true);
                this.formIsDirty = false;
                GridViewPendingInvoice.Select();
            }
            else
            {
                TextBoxCashAmount.ReadOnly = true;
                TextBoxCashAmount.TabStop = false;
                GroupBoxPayMethod.Enabled = false;
                ErrorMsg.Text = Grid_NoPendingInvoice;
            }
        }
        private void ResetForm()
        {
            ErrorMsg.Text = string.Empty;
            RbtCash.Checked = true;
            ErrorMsg.Text = string.Empty;
            TextBoxAddress.ResetText();
            TextBoxName.ResetText();
            TextBoxRefNo.ResetText();
            DateTimePickerInvoiceDate.Format = Global.Company.DateFormat;
            DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            TextBoxSaleId.ResetText();
            TextBoxSalesNetAmount.ResetText();
            TextBoxPaymentId.ResetText();
            TextBoxReceivePaymentSearch.TextBox.ResetText();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewPendingInvoice.Columns["InvAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            ResetPayementMenthod();
        }
        private void ResetPayementMenthod()
        {
            TextBoxBankTransaction.ResetText();
            DateTimePickerCreditDate.Format = Global.Company.DateFormat;
            DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;

            ComboBoxCreditCardAccount.ResetText();
            ComboBoxCreditCardAccount.SelectedIndex = -1;
            TextBoxCreditTransaction.ResetText();
            TextBoxCashAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxCheckDocument.ResetText();
            TextBoxCashBalance.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            ComboBoxBankAccount.SelectedIndex = -1;
            ComboBoxCheckAccount.SelectedIndex = -1;
            ComboBoxUpiAccount.ResetText();
            TextBoxUpiNumber.ResetText();
        }
        private void EnableForm(bool enable)
        {
            TextBoxCashAmount.ReadOnly = false;
            TextBoxCashAmount.TabStop = true;

            GroupBoxPayMethod.Enabled = true;
            BtnReceive.Enabled = enable;
            BtnReceiveDeliver.Enabled = enable;
            GridViewPendingInvoice.Enabled = true;
            GridViewPendingInvoice.ReadOnly = true;
            GroupBoxPayMethod.Enabled = true;

            if (!string.IsNullOrEmpty(TextBoxSaleId.Text))
            {
                SaleEntry Entry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
                if (Entry.isPaymentReceived)
                {
                    BtnReceive.Enabled = true;
                    BtnReceiveDeliver.Text = "Deliver [F9]";
                    TextBoxCashAmount.ReadOnly = false;
                    TextBoxCashAmount.TabStop = true;
                    GroupBoxPayMethod.Enabled = true;
                    TextBoxUpiAmount.ReadOnly = false;
                    TextBoxCreditCardAmount.ReadOnly = false;
                }
                if (Entry.hasDelivered)
                {
                    BtnReceive.Enabled = true;
                    
                    TextBoxCashAmount.ReadOnly = false;
                    TextBoxCashAmount.TabStop = true;
                    GridViewPendingInvoice.Enabled = true;
                    TextBoxUpiAmount.ReadOnly = false;
                    TextBoxCreditCardAmount.ReadOnly = false;
                    BtnReceiveDeliver.Text = "Receive & Deliver[F9]";
                }
            }
            GridViewPendingInvoice.TabStop = false;
        }

        private void GridViewPendingInvoice_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewPendingInvoice.Rows[e.RowIndex].Cells[(int)GridPendingInvColumn.ID].Value != null)
                {
                    if (GridViewPendingInvoice.Rows[e.RowIndex].Cells[(int)GridPendingInvColumn.ID].Value.ToString() == TextBoxSaleId.Text)
                    {
                        return;
                    }

                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Cancel)
                        {
                            return;
                        }
                        else if (Result == DialogResult.Yes)
                        {
                            BtnReceive_Click(sender, e);
                            if (this.formIsDirty) { return; }
                        }
                    }
                    ResetForm();
                    EnableForm(true);

                    LoadPaymentDetails((long)GridViewPendingInvoice.Rows[e.RowIndex].Cells[(int)GridPendingInvColumn.ID].Value);
                    double TotalAmountReceived = 0;
                    TotalAmountReceived = AmountReceived();
                    LableTotalAmountReceived.Text = TotalAmountReceived.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    RbtCash_CheckedChanged(this, null);
                }
            }
            this.formIsDirty = false;
        }
        private void LoadPaymentDetails(long SaleEntryId)
        {
            TextBoxCashBalance.Text = "0.00";
            TextBoxUpiBalance.Text = "0.00";
            TextBoxCreditCardBalance.Text = "0.00";
            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(SaleEntryId);
            if (SaleEntry != null)
            {
                TextBoxSaleId.Text = SaleEntry.Id.ToString();
                TextBoxSalesNetAmount.Text = SaleEntry.NetAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                //Cash
                TextBoxCashBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxCashAmount.Text = "0.00";
                //UPI
                TextBoxUpiBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxUpiAmount.Text = "0.00";
                //CreditCard
                TextBoxCreditCardBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxCreditCardAmount.Text = "0.00";
                TextBoxAddress.Text = SaleEntry.CustomerAddress.Replace(",", "," + System.Environment.NewLine);
                TextBoxName.Text = SaleEntry.CustomerName;
                TextBoxRefNo.Text = SaleEntry.RefNumber;
                DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                if (SaleEntry.isPaymentReceived)
                {
                    LoadPayment(SaleEntry.Id);
                }
            }
            else
            {
                MessageBox.Show("The selected sale is not available anymore");
                ResetForm();
                this.formIsDirty = false;
                LoadPendingInvoice();
            }
        }
        


        private void RbtCash_CheckedChanged(object sender, EventArgs e)
        {
            double TotalAmountReceived = 0;
            double BalanceAmount = 0;
            BalanceAmount = GetBalance();
            if (RbtCash.Checked)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxCashPayment.Visible = true;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                TextBoxCashBalance.TabStop = false;
                GroupBoxUpiPayment.Visible = false;
                TextBoxCashBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxCashAmount.Text = "0.00";
                IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.CASH).ToList();
                if (Payment != null)
                {
                    TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                }
                else
                {
                    TotalAmountReceived = 0.00;
                }
                TextBoxCashAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else if (RbtCheque.Checked)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxCheckInfomation.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = true;
                GroupBoxUpiPayment.Visible = false;
                DateTimePickerCheckDate.Format = Global.Company.DateFormat;
                DateTimePickerCheckDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                ComboUtils.InitializeBankAccountCombo(ComboBoxCheckAccount, Global.Company.CompanyId);
            }
            else if (RbtCard.Checked)
            {
                GroupBoxCreditCard.Location = GroupBoxCashPayment.Location;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCreditCard.Visible = true;
                GroupBoxUpiPayment.Visible = false;
                DateTimePickerCreditDate.Format = Global.Company.DateFormat;
                DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                ComboUtils.InitializeCreditCardAccountCombo(ComboBoxCreditCardAccount, Global.Company.CompanyId);
                TextBoxCreditCardBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxCreditCardAmount.Text = "0.00";
                IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.CREDITCARD).ToList();
                if (Payment != null)
                {
                    TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                }
                else
                {
                    TotalAmountReceived = 0.00;
                }
                TextBoxCreditAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else if (RbtBank.Checked)
            {
                GroupBoxCashPayment.Visible = false;
                GroupBoxCreditCard.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Location = GroupBoxCashPayment.Location;
                GroupBoxBankTransfer.Visible = true;
                GroupBoxUpiPayment.Visible = false;
                ComboUtils.InitializeBankAccountCombo(ComboBoxBankAccount, Global.Company.CompanyId);
            }
            else if (RbtUpi.Checked)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxCashPayment.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                TextBoxCashBalance.TabStop = false;
                GroupBoxUpiPayment.Visible = true;
                GroupBoxUpiPayment.Location = GroupBoxCashPayment.Location;
                UpiDateTime.Format = Global.Company.DateFormat;
                UpiDateTime.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                ComboUtils.InitializeBankAccountCombo(ComboBoxUpiAccount, Global.Company.CompanyId);
                TextBoxUpiBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxUpiAmount.Text = "0.00";
                IList<PaymentNew> Payment = (IList<PaymentNew>)PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.UPI).ToList();
                if (Payment != null)
                {
                    TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                }
                else
                {
                    TotalAmountReceived = 0.00;
                }
                TextBoxUpiAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
        }
        private Payment GetPaymentFromForm()
        {
            Payment lPayment = new Payment();
            lPayment.PaymentId = 0L;
            lPayment.TransactionDate = (DateTime)DateTimePickerInvoiceDate.Date!;
            lPayment.TransctionType = (RbtBank.Checked) ? PaymentType.BANKTRANSFER : (RbtCard.Checked) ? PaymentType.CREDITCARD : (RbtCheque.Checked) ? PaymentType.CHECK : (RbtUpi.Checked) ? PaymentType.UPI : PaymentType.CASH;
            lPayment.Amount = decimal.Parse(TextBoxCashAmount.Text);
            lPayment.CompanyId = Global.Company.CompanyId;
            lPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            if (Global.CostCenter != null)
            {
                lPayment.CostCenterId = Global.CostCenter.CostCenterId;
            }

            return lPayment;
        }
        private PaymentNew GetNewPaymentFromForm()
        {
            PaymentNew lPayment = new PaymentNew();
            lPayment.PaymentNewId = 0L;
            lPayment.TransactionDate = (DateTime)DateTimePickerInvoiceDate.Date!;
            lPayment.TransactionType = (RbtBank.Checked) ? PaymentType.BANKTRANSFER : (RbtCard.Checked) ? PaymentType.CREDITCARD : (RbtCheque.Checked) ? PaymentType.CHECK : (RbtUpi.Checked) ? PaymentType.UPI : PaymentType.CASH;
            lPayment.Amount = decimal.Parse(TextBoxCashAmount.Text);
            lPayment.CompanyId = Global.Company.CompanyId;
            lPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            lPayment.AccountId = AccountID;
            if (Global.CostCenter != null)
            {
                lPayment.CostCenterId = Global.CostCenter.CostCenterId;
            }

            return lPayment;
        }

        private BankTransferPaymentNew GetBankTransferPaymentFromForm()
        {
            PaymentNew Payment = GetNewPaymentFromForm();
            double GetPaymentAmount = 0;
            GetPaymentAmount = GetBalance();
            BankTransferPaymentNew lBankTransferPayment = new BankTransferPaymentNew();
            lBankTransferPayment.PaymentNewId = 0;
            lBankTransferPayment.TransactionDate = Payment.TransactionDate;
            lBankTransferPayment.TransactionType = Payment.TransactionType;
            lBankTransferPayment.Amount = (decimal)GetPaymentAmount;
            lBankTransferPayment.CompanyId = Payment.CompanyId;
            lBankTransferPayment.CostCenterId = Payment.CostCenterId;
            lBankTransferPayment.TransactionNumber = TextBoxBankTransaction.Text;
            lBankTransferPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            Account lAccountBank = (Account)ComboBoxBankAccount.Items[ComboBoxBankAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.Instance.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lBankTransferPayment.BankTransferId = AccountBank.Id;
                    lBankTransferPayment.AccountId = AccountBank.Id;
                }
            }
            return lBankTransferPayment;
        }
        private CheckPaymentNew GetCheckPaymentFromForm()
        {
            PaymentNew Payment = GetNewPaymentFromForm();
            double GetPaymentAmount = 0;
            GetPaymentAmount = GetBalance();
            CheckPaymentNew lCheckPayment = new CheckPaymentNew();
            lCheckPayment.PaymentNewId = 0;
            lCheckPayment.TransactionDate = Payment.TransactionDate;
            lCheckPayment.TransactionType = Payment.TransactionType;
            lCheckPayment.Amount = (decimal)GetPaymentAmount;
            lCheckPayment.CompanyId = Payment.CompanyId;
            lCheckPayment.CostCenterId = Payment.CostCenterId;
            lCheckPayment.DocumentNumber = TextBoxCheckDocument.Text;
            lCheckPayment.DocumentDate = (DateTime)DateTimePickerCheckDate.Date!;
            lCheckPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            Account lAccountDepositedInto = (Account)ComboBoxCheckAccount.Items[ComboBoxCheckAccount.SelectedIndex];
            if (lAccountDepositedInto != null)
            {
                Account AccountDepositedInto = AccountManager.Instance.GetAccountById(lAccountDepositedInto.Id);
                if (AccountDepositedInto != null)
                {
                    lCheckPayment.DepositedIntoId = AccountDepositedInto.Id;
                    lCheckPayment.AccountId = AccountDepositedInto.Id;
                }
            }

            return lCheckPayment;
        }
        private CardPaymentNew GetCreditCardPaymentFromForm()
        {
            PaymentNew Payment = GetNewPaymentFromForm();
            CardPaymentNew lCreditCardPayment = new CardPaymentNew();
            lCreditCardPayment.PaymentNewId = 0;
            lCreditCardPayment.TransactionDate = Payment.TransactionDate;
            lCreditCardPayment.TransactionType = Payment.TransactionType;
            lCreditCardPayment.Amount = decimal.Parse(TextBoxCreditCardAmount.Text);
            lCreditCardPayment.CompanyId = Payment.CompanyId;
            lCreditCardPayment.CostCenterId = Payment.CostCenterId;
            lCreditCardPayment.CCTransactionDate = (DateTime)DateTimePickerCreditDate.Date!;
            lCreditCardPayment.CTransactionNumber = TextBoxCreditTransaction.Text;
            lCreditCardPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            Account lAccountBank = (Account)ComboBoxCreditCardAccount.Items[ComboBoxCreditCardAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.Instance.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lCreditCardPayment.CAccountId = AccountBank.Id;
                    lCreditCardPayment.AccountId = AccountBank.Id;
                }
            }
            return lCreditCardPayment;
        }
        private UpiTransactionPaymentNew GetUpiTransactionPaymentFromForm()
        {
            PaymentNew Payment = GetNewPaymentFromForm();
            UpiTransactionPaymentNew UpiTransactionPayment = new UpiTransactionPaymentNew();
            UpiTransactionPayment.PaymentNewId = 0;
            UpiTransactionPayment.TransactionDate = Payment.TransactionDate;
            UpiTransactionPayment.TransactionType= Payment.TransactionType;
            UpiTransactionPayment.Amount = decimal.Parse(TextBoxUpiAmount.Text);
            UpiTransactionPayment.CompanyId = Payment.CompanyId;
            UpiTransactionPayment.CostCenterId = Payment.CostCenterId;
            UpiTransactionPayment.UpiTransactionNumber = TextBoxUpiNumber.Text;
            UpiTransactionPayment.UpiTransactionDate = (DateTime)UpiDateTime.Date!;
            UpiTransactionPayment.SalesId = long.Parse(TextBoxSaleId.Text);
            Account lAccountUpi = (Account)ComboBoxUpiAccount.Items[ComboBoxUpiAccount.SelectedIndex];
            if (lAccountUpi != null)
            {
                Account AccountUpi = AccountManager.Instance.GetAccountById(lAccountUpi.Id);
                if (AccountUpi != null)
                {
                    UpiTransactionPayment.UpiTransactionId = AccountUpi.Id;
                    UpiTransactionPayment.AccountId = AccountUpi.Id;
                }
            }
            return UpiTransactionPayment;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (SalePaymentOnLoad)
            {
                this.Close();
            }
            ResetForm();
            EnableForm(false);
            if (!SalePaymentOnLoad)
            {
                LoadPendingInvoice();
            }
            this.formIsDirty = false;
            if (GridViewPendingInvoice.Rows.Count == 0)
            {
                this.Close();
            }
        }
        private void BtnReceive_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PaymentNew lPayment = Payment();
                UpdateSale(true, BtnReceiveDeliver.Enabled ? false : true, lPayment.PaymentNewId);
                TextBoxCashAmount.Text = "0.00";
                TextBoxUpiAmount.Text = "0.00";
                TextBoxCreditCardAmount.Text = "0.00";
                if (!SalePaymentOnLoad)
                {
                    Timer delayedExecution = new Timer();
                    delayedExecution.Interval = 1000;
                    delayedExecution.Tick += (s, args) =>
                    {
                        delayedExecution.Stop();
                        delayedExecution.Dispose();
                        LoadPendingInvoice();
                    };
                    delayedExecution.Start();
                }

                ErrorMsg.Text = "Saved";
                EnableForm(true);
                this.formIsDirty = false;
            }
        }
        private void BtnReceiveDeliver_Click(object sender, EventArgs e)
        {
            double balanceAmount = GetBalance();
            if (balanceAmount == 0)
            {
                SaleEntry Entry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
                UpdateSale(true, true, (long)Entry.PaymentNewId);
                if (SalePaymentOnLoad)
                {
                    this.Close();
                }
                if (!SalePaymentOnLoad)
                {
                    LoadPendingInvoice();
                }
                EnableForm(true);
                this.formIsDirty = false;
            }
            else
            {
                ErrorMsg.Text = "Please receive the full amount before making the delivery.";
            }
        }

        private PaymentNew Payment()
        {

            PaymentNew PaymentFromDB = new PaymentNew();
            if (RbtCash.Checked)
            {
                PaymentNew lPayment = GetNewPaymentFromForm();

                ////if (lPayment.PaymentId == 0)
                //{
                PaymentFromDB = PaymentsNewManager.Instance.AddPayment(lPayment);
                //}
                //else
                //{
                //    PaymentFromDB = PaymentManager.UpdatePayment(lPayment);
                //}
            }
            else if (RbtCheque.Checked)
            {
                CheckPaymentNew lCheckPayment = GetCheckPaymentFromForm();
                CheckPaymentNew CheckPaymentFromDB = PaymentsNewManager.Instance.AddCheckPayment(lCheckPayment);
                PaymentFromDB.PaymentNewId = CheckPaymentFromDB.PaymentNewId;
            }
            else if (RbtCard.Checked)
            {
                CardPaymentNew lCreditCardPayment = GetCreditCardPaymentFromForm();
                CardPaymentNew CreditCardPaymentFromDB = null!;
                //if (lCreditCardPayment.PaymentId == 0)
                //{
                CreditCardPaymentFromDB = PaymentsNewManager.Instance.AddCreditCardPayment(lCreditCardPayment);
                //}
                //else
                //{
                //    CreditCardPaymentFromDB = PaymentManager.UpdateCreditCardPayment(lCreditCardPayment);
                //}
                PaymentFromDB.PaymentNewId = CreditCardPaymentFromDB.PaymentNewId;
            }
            else if (RbtBank.Checked)
            {
                BankTransferPaymentNew lBankTransferPayment = GetBankTransferPaymentFromForm();
                BankTransferPaymentNew BankTransferPaymentFromDB = null!;
                //if (lBankTransferPayment.PaymentId == 0)
                //{
                BankTransferPaymentFromDB = PaymentsNewManager.Instance.AddBankTransferPayment(lBankTransferPayment);
                //}
                //else
                //{
                //    BankTransferPaymentFromDB = PaymentManager.UpdateBankTransferPayment(lBankTransferPayment);
                //}
                PaymentFromDB.PaymentNewId = BankTransferPaymentFromDB.PaymentNewId;
            }
            else if (RbtUpi.Checked)
            {
                UpiTransactionPaymentNew upiTransactionPayment = GetUpiTransactionPaymentFromForm();
                UpiTransactionPaymentNew upiTransactionPaymentFromDB = null!;
                //if (upiTransactionPayment.PaymentId == 0)
                //{
                upiTransactionPaymentFromDB = PaymentsNewManager.Instance.AddUpiTransactionPayment(upiTransactionPayment);
                //}
                //else
                //{
                //    upiTransactionPaymentFromDB = PaymentManager.UpdateUpiTransactionPayment(upiTransactionPayment);
                //}
                PaymentFromDB.PaymentNewId = upiTransactionPaymentFromDB.PaymentNewId;
            }
            TextBoxPaymentId.Text = PaymentFromDB.PaymentNewId.ToString();
            return PaymentFromDB;
        }
        private void UpdateSaleOld(bool Received, bool ReceivedDeliver, long PaymentNewId)
        {
            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
            if (SaleEntry != null)
            {
                SaleEntry.isPaymentReceived = Received;
                SaleEntry.hasDelivered = ReceivedDeliver;
                SaleEntry.PaymentNewId = PaymentNewId;
                SalesManager.Instance.UpdateSaleEntry(SaleEntry);
            }
            else
            {
                MessageBox.Show("The selected sale is not available anymore");
            }
        }
        private void UpdateSale(bool Received, bool ReceivedDeliver, long PaymentNewId)
        {
            SaleEntry SaleEntry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
            if (SaleEntry != null)
            {
                SaleEntry.isPaymentReceived = Received;
                SaleEntry.hasDelivered = ReceivedDeliver;

                // ✅ USE THIS
                SaleEntry.PaymentNewId = PaymentNewId;

                SalesManager.Instance.UpdateSaleEntry(SaleEntry);
            }
        }


        private Boolean ValidateForm()
        {
            double balanceAmount = GetBalance();
            if(balanceAmount != 0)
            {
                //Cash
                if (GroupBoxCashPayment.Visible)
                {
                    if (!float.TryParse(TextBoxCashBalance.Text, out float Balance) || Balance == 0)
                    {

                    }
                    if (!float.TryParse(TextBoxCashAmount.Text, out float cashAmount) || cashAmount == 0)
                    {
                        TextBoxCashAmount.Select();
                        ErrorMsg.Text = EnterAmountErrorMsg;
                        return false;
                    }

                    if (!float.TryParse(TextBoxSalesNetAmount.Text, out float salesNetAmount))
                    {
                        TextBoxSalesNetAmount.Select();
                        ErrorMsg.Text = EnterAmountWrongErrorMsg;
                        return false;
                    }
                }
                //UPI
                if (GroupBoxUpiPayment.Visible)
                {
                    if (!float.TryParse(TextBoxUpiAmount.Text, out float cashAmount) || cashAmount == 0)
                    {
                        TextBoxUpiAmount.Select();
                        ErrorMsg.Text = EnterAmountErrorMsg;
                        return false;
                    }

                    if (!float.TryParse(TextBoxSalesNetAmount.Text, out float salesNetAmount))
                    {
                        TextBoxSalesNetAmount.Select();
                        ErrorMsg.Text = EnterAmountWrongErrorMsg;
                        return false;
                    }
                    if ((UpiDateTime.Date == null || !DateUtils.ValidDate(((DateTime)UpiDateTime.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
                    {
                        UpiDateTime.Focus();
                        ErrorMsg.Text = EnterCheckDateErrorMsg;
                        return false;
                    }
                    if (float.Parse(TextBoxUpiAmount.Text) == 0)
                    {
                        TextBoxUpiAmount.Select();
                        ErrorMsg.Text = EnterAmountErrorMsg;
                        return false;
                    }
                    if (ComboBoxUpiAccount.SelectedIndex < 0)
                    {
                        ComboBoxUpiAccount.Select();
                        ErrorMsg.Text = ChooseBankAccountErrorMsg;
                        return false;
                    }
                }
                //BankTransfer
                if (GroupBoxBankTransfer.Visible)
                {
                    if (ComboBoxBankAccount.SelectedIndex < 0)
                    {
                        ComboBoxBankAccount.Select();
                        ErrorMsg.Text = ChooseBankAccountErrorMsg;
                        return false;
                    }
                    if (string.IsNullOrEmpty(TextBoxBankTransaction.Text.Trim()))
                    {
                        TextBoxBankTransaction.Select();
                        ErrorMsg.Text = EnterBankTranErrorMsg;
                        return false;
                    }
                }
                //Check
                if (GroupBoxCheckInfomation.Visible)
                {
                    if (string.IsNullOrEmpty(TextBoxCheckDocument.Text.Trim()))
                    {
                        TextBoxCheckDocument.Select();
                        ErrorMsg.Text = EnterCheckDocErrorMsg;
                        return false;
                    }
                    if ((DateTimePickerCheckDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerCheckDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
                    {
                        DateTimePickerCheckDate.Focus();
                        ErrorMsg.Text = EnterCheckDateErrorMsg;
                        return false;
                    }

                    if (ComboBoxCheckAccount.SelectedIndex < 0)
                    {
                        ComboBoxCheckAccount.Select();
                        ErrorMsg.Text = ChooseCheckAccountErrorMsg;
                        return false;
                    }
                }
                //CreditCard
                if (GroupBoxCreditCard.Visible == true)
                {
                    if (ComboBoxCreditCardAccount.SelectedIndex < 0)
                    {
                        ComboBoxCreditCardAccount.Select();
                        ErrorMsg.Text = ChooseCardAccountErrorMsg;
                        return false;
                    }
                    if (TextBoxCreditTransaction.Text.Replace(" ", "") == "")
                    {
                        TextBoxCreditTransaction.Select();
                        ErrorMsg.Text = EnterCardTranErrorMsg;
                        return false;
                    }

                    if ((DateTimePickerCreditDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerCreditDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
                    {
                        DateTimePickerCreditDate.Select();
                        ErrorMsg.Text = EnterCardDateErrorMsg;
                        return false;
                    }
                }
            }
            else
            {
                ErrorMsg.Text = "You have get the entire received amount. You can now proceed with the delivery.";
                return false;
            }
            return true;
        }
        private void BtnSearchReceivePayment_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                RecentSaless();

                if (SearchSalesId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnReceive_Click(sender, e);
                                LoadPayment(SearchSalesId);
                                SelectUndeliverBill(SearchSalesId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadPayment(SearchSalesId);
                            SelectUndeliverBill(SearchSalesId);
                        }
                    }
                    else
                    {
                        LoadPayment(SearchSalesId);
                        SelectUndeliverBill(SearchSalesId);
                    }
                }
            }

        }
        private void SelectUndeliverBill(long SearchSalesId)
        {
            SaleEntry SaleEntry = SalesManager.Instance.GetPaymentSaleEntry(SearchSalesId);
            if (!SaleEntry.hasDelivered && GridViewPendingInvoice.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataGridViewRow row in GridViewPendingInvoice.Rows)
                {
                    if (((long)row.Cells[(int)GridPendingInvColumn.ID].Value) == SearchSalesId)
                    {
                        GridViewPendingInvoice.CurrentCell = GridViewPendingInvoice[0, i];
                        GridViewPendingInvoice.Focus();
                        if (!SaleEntry.isPaymentReceived)
                        {
                            TextBoxCashAmount.Select();
                        }
                    }
                    i++;
                }
            }
        }
        private bool isValidSearchCriteria()
        {
            ErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxReceivePaymentSearch.Text.Trim()))
            {
                ErrorMsg.Text = SearchBoxEmptyErrorMsg;
                TextBoxReceivePaymentSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private double RoundOff(double TotalAmount)
        {
            double _roundoff = 0.00;
            double mod = TotalAmount % 1;
            if (mod > 0.50)
            {
                _roundoff = 1 - mod;
            }
            else
            {
                _roundoff = -mod;
            }
            return Math.Round(_roundoff, 2);
        }
        private void RecentSaless()
        {
            SalesManager SalesManager = SalesManager.Instance;
            ErrorMsg.Text = "";
            String SearchText = TextBoxReceivePaymentSearch.Text.Trim();
            IList<SaleEntry> SalesInfo = null!;
            if (string.IsNullOrEmpty(SearchText))
            {
                SalesInfo = SalesManager.GetSaleInvoiceByCompanyId(Global.Company.CompanyId);
            }
            //else if (TextUtils.isReferenceNumber(SearchText))
            //{
            //    SalesInfo = SalesManager.GetSaleRecentPaymentByReferenceNo(SearchText, Global.Company.CompanyId);
            //}
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                SalesInfo = SalesManager.GetSaleRecentPaymentByAmount(Amount, SearchText, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat)!;
                SalesInfo = SalesManager.GetSaleRecentPaymentByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                SalesInfo = SalesManager.GetSaleRecentPaymentByCustomerName(SearchText, Global.Company.CompanyId);
            }

            if (SalesInfo.Count > 0)
            {
                LoadSales(SalesInfo);
            }
            else
            {
                SearchSalesId = 0L;
                ErrorMsg.Text = SearchOutput;
            }
        }
        public void LoadSales(IList<SaleEntry> SaleEntrys)
        {

            if (SaleEntrys != null && SaleEntrys.Count > 0)
            {
                FormRecentSalePayment FormRecentSalePayment = new FormRecentSalePayment(this);
                FormRecentSalePayment.SaleEntryInfo = SaleEntrys;
                FormRecentSalePayment.ShowDialog();
            }
            else
            {
                ErrorMsg.Text = SearchOutput;
            }
        }
        private void LoadPayment(long SalesId)
        {
            ResetForm();
            EnableForm(true);
            TextBoxCashBalance.Text = "0.00";
            TextBoxUpiBalance.Text = "0.00";
            TextBoxCreditCardBalance.Text = "0.00";
            SaleEntry SaleEntry = SalesManager.Instance.GetPaymentSaleEntry(SalesId);
            if (SaleEntry != null)
            {
                double TotalAmountReceived = 0.00;
                TextBoxPaymentId.Text = (SaleEntry.SalePayment != null) ? SaleEntry.SalePayment.PaymentId.ToString() : "";
                TextBoxSaleId.Text = SaleEntry.Id.ToString();
                TextBoxSalesNetAmount.Text = SaleEntry.NetAmount.ToString();
                TextBoxAddress.Text = SaleEntry.CustomerAddress.Replace(" ,", "," + System.Environment.NewLine);
                TextBoxName.Text = SaleEntry.CustomerName;
                TextBoxRefNo.Text = SaleEntry.RefNumber;
                DateTimePickerInvoiceDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                if (SaleEntry.SalePayment != null)
                {
                    if (SaleEntry.SalePayment.TransctionType == PaymentType.CASH)
                    {
                        RbtCash.Checked = true;
                        TextBoxCashBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        TextBoxCashAmount.Text = "0.00";
                        IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.CASH).ToList();
                        if (Payment != null)
                        {
                            TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                        }
                        else
                        {
                            TotalAmountReceived = 0.00;
                        }
                        TextBoxCashAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (SaleEntry.SalePayment.TransctionType == PaymentType.BANKTRANSFER)
                    {
                        RbtBank.Checked = true;
                        BankTransferPayment BankTransferPayment = PaymentManager.GetBankTransferPayment((long)SaleEntry.PaymentId!);
                        ComboBoxBankAccount.SelectedIndex = ComboBoxBankAccount.FindStringExact(BankTransferPayment.BankTransfer.Name);
                        TextBoxBankTransaction.Text = BankTransferPayment.TransactionNumber;
                    }
                    else if (SaleEntry.SalePayment.TransctionType == PaymentType.CHECK)
                    {
                        RbtCheque.Checked = true;
                        CheckPayment CheckPayment = PaymentManager.GetCheckPayment((long)SaleEntry.PaymentId!);
                        TextBoxCheckDocument.Text = CheckPayment.DocumentNumber;
                        DateTimePickerCheckDate.Date = (DateTime)DateUtils.ToDate(CheckPayment.DocumentDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                        ComboBoxCheckAccount.SelectedIndex = ComboBoxCheckAccount.FindStringExact(CheckPayment.BankAccount.Name);
                    }
                    else if (SaleEntry.SalePayment.TransctionType == PaymentType.CREDITCARD)
                    {
                        RbtCard.Checked = true;
                        CreditCardPayment CreditCardPayment = PaymentManager.GetCreditCardPayment((long)SaleEntry.PaymentId!);
                        ComboBoxCreditCardAccount.SelectedIndex = ComboBoxCreditCardAccount.FindStringExact(((Account)CreditCardPayment.CCAccount).ToString());

                        DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(CreditCardPayment.CCTransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                        TextBoxCreditTransaction.Text = CreditCardPayment.CCTransactionNumber;

                        TextBoxCreditCardBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        TextBoxCreditCardAmount.Text = "0.00";
                        IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.CREDITCARD).ToList();
                        if (Payment != null)
                        {
                            TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                        }
                        else
                        {
                            TotalAmountReceived = 0.00;
                        }
                        TextBoxCreditAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else if (SaleEntry.SalePayment.TransctionType == PaymentType.UPI)
                    {
                        RbtUpi.Checked = true;
                        UpiTransactionPayment upiTransactionPayment = PaymentManager.GetUpiTranscationPayment((long)SaleEntry.PaymentId!);
                        ComboBoxUpiAccount.SelectedIndex = ComboBoxUpiAccount.FindStringExact(upiTransactionPayment.UpiAccount.Name);
                        TextBoxUpiBalance.Text = Math.Abs(GetBalance()).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        TextBoxUpiAmount.Text = "0.00";
                        TextBoxUpiNumber.Text = upiTransactionPayment.UpiTransactionNumber.ToString();
                        UpiDateTime.Date = (DateTime)DateUtils.ToDate(upiTransactionPayment.UpiTransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                        IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text)).Where(x => x.TransactionType == PaymentType.UPI).ToList();
                        if (Payment != null)
                        {
                            TotalAmountReceived = (double)Payment.Sum(x => x.Amount);
                        }
                        else
                        {
                            TotalAmountReceived = 0.00;
                        }
                        TextBoxUpiAmountReceived.Text = Math.Abs(TotalAmountReceived).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                }
                EnableForm(true);
                //if (SaleEntry.hasDelivered)
                //{
                //    BtnReceive.Enabled = false;
                //    BtnReceiveDeliver.Enabled = false;
                //    BtnReceiveDeliver.Text = "Receive & Deliver[F9]";
                //}
                //else if (SaleEntry.isPaymentReceived)
                //{
                //    BtnReceive.Enabled = false;
                //    BtnReceiveDeliver.Text = "Deliver [F9]";
                //}
            }
            this.formIsDirty = false;
        }
        private double FeeCharged()
        {
            double FeeCharged = 0.00;
            if (!string.IsNullOrEmpty(TextBoxSaleId.Text))
            {
                SaleEntry Entry = SalesManager.Instance.GetSaleEntry(long.Parse(TextBoxSaleId.Text));
                if (Entry != null)
                {
                    FeeCharged = Entry.NetAmount;
                }
            }
            return FeeCharged;
        }
        private double AmountReceived()
        {
            double AmountReceived = 0.00;
            if (!string.IsNullOrEmpty(TextBoxSaleId.Text))
            {
                IList<PaymentNew> Payment = PaymentsNewManager.Instance.ListAllUnAppliedPaymentPaymentBySale(long.Parse(TextBoxSaleId.Text));
                if (Payment != null)
                {
                    AmountReceived = (double)Payment.Sum(x => x.Amount);
                }
                else
                {
                    AmountReceived = 0.00;
                }
            }
            return AmountReceived;
        }
        private double GetBalance()
        {
            double Balance = 0.00;
            double Charged = Math.Round(FeeCharged(), Global.Company.PrimaryCurrency.RoundingPrecision);
            double Received = Math.Round(AmountReceived(), Global.Company.PrimaryCurrency.RoundingPrecision);
            if (Charged > Math.Abs(Received))
            {
                Balance = Charged - Received;
            }
            return Balance;
        }
        private double GetOutStandingBalance()
        {
            double Balance = 0.00;
            double Charged = Math.Round(FeeCharged(), Global.Company.PrimaryCurrency.RoundingPrecision);
            double Received = Math.Round(AmountReceived(), Global.Company.PrimaryCurrency.RoundingPrecision);
            if (Charged < Math.Abs(Received))
            {
                Balance = Received - Charged;
            }
            return Balance;
        }
        private bool GridFocus = false;
        private void GridViewPendingInvoice_Enter(object sender, EventArgs e)
        {
            GridFocus = true;
        }

        private void GridViewPendingInvoice_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnReceive.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnReceiveDeliver.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewPendingInvoice.CurrentRow.Index > -1)
                    {
                        if (GridViewPendingInvoice.CurrentCell.RowIndex != GridViewPendingInvoice.Rows.Count - 1)
                        {
                            GridViewPendingInvoice.CurrentCell = GridViewPendingInvoice[0, GridViewPendingInvoice.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            bool enable = GroupBoxPayMethod.Enabled;
                            if (RbtBank.Checked)
                            {
                                if (enable) { RbtBank.Select(); } else { ComboBoxBankAccount.Select(); }
                            }
                            else if (RbtCard.Checked)
                            {
                                if (enable) { RbtCard.Select(); } else { ComboBoxCreditCardAccount.Select(); }
                            }
                            else if (RbtCheque.Checked)
                            {
                                if (enable) { RbtCheque.Select(); } else { TextBoxCheckDocument.Select(); }
                            }
                            else if (RbtUpi.Checked)
                            {
                                if (enable) { RbtUpi.Select(); } else { TextBoxUpiAmount.Select(); }
                            }
                            else
                            {
                                if (enable) { RbtCash.Select(); } else { TextBoxCashAmount.Select(); }
                            }

                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewPendingInvoice.CurrentRow.Index > -1)
                    {
                        if (GridViewPendingInvoice.CurrentRow.Index != 0)
                        {
                            GridViewPendingInvoice.CurrentCell = GridViewPendingInvoice[0, GridViewPendingInvoice.CurrentCell.RowIndex];
                        }
                        else
                        {
                            BtnReceive.Select();

                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxCashAmount_TextChanged(object sender, EventArgs e)
        {
            double Received = string.IsNullOrEmpty(TextBoxCashAmount.Text.Trim()) ? 0.00 : double.Parse(TextBoxCashAmount.Text);
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                Received = Math.Abs(EditableAmount - Received);
            }
            double NewBalance = Received - GetBalance();
            TextBoxCashBalance.Text = (Math.Abs(GetOutStandingBalance()) + NewBalance) > 0 ? (Math.Abs(GetOutStandingBalance()) + NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : (Math.Abs(GetOutStandingBalance()) - NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

            if (!double.TryParse(TextBoxCashAmount.Text, out double receivedAmount))
            {
                TextBoxCashAmount.Text = "0.00";
                return;
            }

            double balanceAmount = GetBalance();

            if (receivedAmount > balanceAmount)
            {
                //MessageBox.Show("Received amount cannot be greater than Balance Amount!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxCashAmount.Text = balanceAmount.ToString("0.00");
                TextBoxCashAmount.Select(TextBoxCashAmount.Text.Length, 0);
            }
        }

        private void DateTimePickerCreditDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void GroupBoxCashPayment_Enter(object sender, EventArgs e)
        {

        }

        private void TextBoxReceivePaymentSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchReceivePayment_Click(sender, e);
            }
        }

        private void TextBoxUpiAmount_TextChanged(object sender, EventArgs e)
        {
            double Received = string.IsNullOrEmpty(TextBoxUpiAmount.Text.Trim()) ? 0.00 : double.Parse(TextBoxUpiAmount.Text);
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                Received = Math.Abs(EditableAmount - Received);
            }
            double NewBalance = Received - GetBalance();
            TextBoxUpiBalance.Text = (Math.Abs(GetOutStandingBalance()) + NewBalance) > 0 ? (Math.Abs(GetOutStandingBalance()) + NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : (Math.Abs(GetOutStandingBalance()) - NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

            if (!double.TryParse(TextBoxUpiAmount.Text, out double receivedAmount))
            {
                TextBoxUpiAmount.Text = "0.00";
                return;
            }

            double balanceAmount = GetBalance();

            if (receivedAmount > balanceAmount)
            {
                //MessageBox.Show("Received amount cannot be greater than Balance Amount!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxUpiAmount.Text = balanceAmount.ToString("0.00");
                TextBoxUpiAmount.Select(TextBoxUpiAmount.Text.Length, 0);
            }
        }

        private void TextBoxCreditCardAmount_TextChanged(object sender, EventArgs e)
        {
            double Received = string.IsNullOrEmpty(TextBoxCreditCardAmount.Text.Trim()) ? 0.00 : double.Parse(TextBoxCreditCardAmount.Text);
            if (!string.IsNullOrEmpty(TextBoxPaymentId.Text))
            {
                Received = Math.Abs(EditableAmount - Received);
            }
            double NewBalance = Received - GetBalance();
            TextBoxCreditCardBalance.Text = (Math.Abs(GetOutStandingBalance()) + NewBalance) > 0 ? (Math.Abs(GetOutStandingBalance()) + NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : (Math.Abs(GetOutStandingBalance()) - NewBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

            if (!double.TryParse(TextBoxCreditCardAmount.Text, out double receivedAmount))
            {
                TextBoxCreditCardAmount.Text = "0.00";
                return;
            }

            double balanceAmount = GetBalance();

            if (receivedAmount > balanceAmount)
            {
                //MessageBox.Show("Received amount cannot be greater than Balance Amount!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBoxCreditCardAmount.Text = balanceAmount.ToString("0.00");
                TextBoxCreditCardAmount.Select(TextBoxCreditCardAmount.Text.Length, 0);
            }
        }
    }
}
