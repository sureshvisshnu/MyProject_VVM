using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.utils;
using fa.libraries.utils;
using fa.views.account.masters;
using fa.api.Accounting;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.api.System;
using fa.api.Log;
using fa.views.common;
using fa.views.employee;
using fa.views.utils;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.views.controls.grid;

namespace fa.views.account.transactions
{
    enum ExpenseDataGridColumns
    {
        SEQ, TOACCOUNT, DESCRIPTION, AMOUNT, DELETE, ID, TYPE
    }


    public partial class FormExpense : FormBase
    {
        public long SupplierId = 0L;

        public static string SaveSuccessText = "Save successfully...";
        public static string DeleteConfirmText = "Do you want to delete the expense {0}?";
        public static string DeleteErrorText = "Error deleting the expense!, Please retry";
        public static string NotAllowDeleteErrorText = "Could not delete the expense as there is a payment against it";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please enter expense details";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could be a supplier, general account, date or expense transaction id";
        public static string SearchOutput = "No entry found";

        public static string ChooseSupplierErrorMsg = "Please choose payee";
        public static string EnterExpenseDateErrorMsg = "Please enter expense date";
        public static string ChoosePaymentTypeErrorMsg = "Please choose payment type";
        public static string EnterAmountErrorMsg = "Please enter amount";
        public static string ChooseBankAccountErrorMsg = "Please choose bank account";
        public static string EnterBankTranErrorMsg = "Please enter transaction";
        public static string EnterCheckDocErrorMsg = "Please enter document";
        public static string EnterCheckDateErrorMsg = "Please enter proper check date";
        public static string ChooseCheckAccountErrorMsg = "Please choose the bank account";

        public static string EnterCardTranErrorMsg = "Please enter credir card transaction details";
        public static string ChooseCardAccountErrorMsg = "Please choose account";
        public static string EnterCardDateErrorMsg = "Please enter credit card Date";
        public static string EnterAmountWrongDateErrorMsg = "Please enter valid amount";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";


        BillManager BillManager = null;
        ExpenseManager ExpenseManager = null;
        AccountManager AccountManager = null;
        AccountGroupManager AccountGroupManager = null;
        KeypressValidation KeypressValidation = null;
        CompanyManager CompanyManager = null;
        DateValidation DateValidation = null;

        int[] RequiredColumns = new int[] { 1, 4 };
        string[] RequiredColumnsValidationMsg = new string[] { "", "Please enter description for the line", "", "", "Please enter amount for the line", "Payment exceed balance amount", "First clear invoice amount after that add advance" };

        public FormExpense()
        {
            BillManager = BillManager.Instance;
            ExpenseManager = ExpenseManager.Instance;
            AccountManager = AccountManager.Instance;
            AccountGroupManager = AccountGroupManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            CompanyManager = CompanyManager.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "toolStrip1", "GridViewExpenseSearch" };
        }
        private void FormExpense_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                EnableForm(true);
                TextBoxExpensePayee.Select();
                RecentExpense();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private Expense GetExpenseFromForm()
        {
            Expense lExpense = new Expense();
            lExpense.Reference = ExpenseRefNo.Text;
            if (!string.IsNullOrEmpty(TextBoxExpenseId.Text))
            {
                lExpense.ExpenseId = Convert.ToInt64(TextBoxExpenseId.Text);
            }
            else
            {
                lExpense.ExpenseId = 0L;
            }
            Account lAccount = AccountManager.GetAccountById(long.Parse(TextBoxExpensePayee.Id));
            if (lAccount != null)
            {
                lExpense.PayeeId = lAccount.Id;
            }
            lExpense.TransactionDate = (DateTime)DateTimePickerExpensePayment.Date;
            lExpense.TransctionType = (PaymentType)ComboBoxExpensePaymentType.SelectedIndex;
            lExpense.Amount = float.Parse(TextBoxExpenseAmount.Text);
            lExpense.Memo = TextBoxExpenseMemo.Text;
            lExpense.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lExpense.CostCenterId = Global.CostCenter.CostCenterId;
            }
            lExpense.ExpenseDetails = new List<ExpenseDetail>();
            for (int i = 0; i < DataGridViewExpenseDetails.Rows.Count - 1; i++)
            {
                ExpenseDetail ExpenseDetail = new ExpenseDetail();
                Account Account = AccountManager.Instance.GetAccountById((long)DataGridViewExpenseDetails.Rows[i].Cells[1].Value);

                ExpenseDetail.AccountId = Account.Id;
                ExpenseDetail.Description = (DataGridViewExpenseDetails.Rows[i].Cells[2].Value != null) ? (DataGridViewExpenseDetails.Rows[i].Cells[2].Value.ToString()).Trim() : "";
                ExpenseDetail.Amount = (float.Parse(DataGridViewExpenseDetails.Rows[i].Cells[3].Value.ToString()));
                ExpenseDetail.ExpenseId = lExpense.ExpenseId;

                lExpense.ExpenseDetails.Add(ExpenseDetail);
            }
            return lExpense;
        }


        private BankTransferExpense GetBankTransferExpenseFromForm()
        {
            Expense Expense = GetExpenseFromForm();
            BankTransferExpense lBankTransferExpense = new BankTransferExpense();
            lBankTransferExpense.ExpenseDetails = Expense.ExpenseDetails;
            lBankTransferExpense.Reference = Expense.Reference;
            lBankTransferExpense.ExpenseId = Expense.ExpenseId;
            lBankTransferExpense.PayeeId = Expense.PayeeId;
            lBankTransferExpense.TransactionDate = Expense.TransactionDate;
            lBankTransferExpense.TransctionType = Expense.TransctionType;
            lBankTransferExpense.Amount = Expense.Amount;
            lBankTransferExpense.Memo = Expense.Memo;
            lBankTransferExpense.CompanyId = Expense.CompanyId;
            lBankTransferExpense.CostCenterId = Expense.CostCenterId;
            lBankTransferExpense.TransactionNumber = TextBoxExpenseBankTransaction.Text;
            Account lAccountBank = (Account)ComboBoxExpenseBankAccount.Items[ComboBoxExpenseBankAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lBankTransferExpense.BankTransferId = AccountBank.Id;
                }
            }
            return lBankTransferExpense;
        }
        private CheckExpense GetCheckExpenseFromForm()
        {
            Expense Expense = GetExpenseFromForm();
            CheckExpense lCheckExpense = new CheckExpense();
            lCheckExpense.ExpenseDetails = Expense.ExpenseDetails;
            lCheckExpense.Reference = Expense.Reference;
            lCheckExpense.ExpenseId = Expense.ExpenseId;
            lCheckExpense.PayeeId = Expense.PayeeId;
            lCheckExpense.TransactionDate = Expense.TransactionDate;
            lCheckExpense.TransctionType = Expense.TransctionType;
            lCheckExpense.Amount = Expense.Amount;
            lCheckExpense.Memo = Expense.Memo;
            lCheckExpense.CompanyId = Expense.CompanyId;
            lCheckExpense.CostCenterId = Expense.CostCenterId;
            lCheckExpense.DocumentNumber = TextBoxExpenseCheckDocument.Text;
            lCheckExpense.DocumentDate = (DateTime)DateTimePickerExpenseCheckDate.Date;

            Account lAccountBank = (Account)ComboBoxExpenseCheckAccount.Items[ComboBoxExpenseCheckAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lCheckExpense.BankAccountId = AccountBank.Id;
                }
            }



            return lCheckExpense;
        }
        private CreditCardExpense GetCreditCardExpenseFromForm()
        {
            Expense Expense = GetExpenseFromForm();
            CreditCardExpense lCreditCardExpense = new CreditCardExpense();
            lCreditCardExpense.ExpenseDetails = Expense.ExpenseDetails;
            lCreditCardExpense.Reference = Expense.Reference;
            lCreditCardExpense.ExpenseId = Expense.ExpenseId;
            lCreditCardExpense.PayeeId = Expense.PayeeId;
            lCreditCardExpense.TransactionDate = Expense.TransactionDate;
            lCreditCardExpense.TransctionType = Expense.TransctionType;
            lCreditCardExpense.CCTransactionNumber = TextBoxExpenseCreditTransaction.Text;
            lCreditCardExpense.CCTransactionDate = (DateTime)DateTimePickerCreditDate.Date;
            lCreditCardExpense.Amount = Expense.Amount;
            lCreditCardExpense.Memo = Expense.Memo;
            lCreditCardExpense.CompanyId = Expense.CompanyId;
            lCreditCardExpense.CostCenterId = Expense.CostCenterId;

            Account lAccountBank = (Account)ComboBoxExpenseCreditCardAccount.Items[ComboBoxExpenseCreditCardAccount.SelectedIndex];
            if (lAccountBank != null)
            {
                Account AccountBank = AccountManager.GetAccountById(lAccountBank.Id);
                if (AccountBank != null)
                {
                    lCreditCardExpense.CCAccountId = AccountBank.Id;
                }
            }
            return lCreditCardExpense;
        }

        private void ComboBoxPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxExpensePaymentType.SelectedIndex == 0)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxCashExpense.Visible = true;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Visible = false;

            }
            else if (ComboBoxExpensePaymentType.SelectedIndex == 1)
            {
                GroupBoxCreditCard.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCashExpense.Visible = false;
                GroupBoxCheckInfomation.Location = GroupBoxCashExpense.Location;
                GroupBoxCheckInfomation.Visible = true;
                DateTimePickerExpenseCheckDate.Format = Global.Company.DateFormat;
                DateTimePickerExpenseCheckDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeBankAccountCombo(ComboBoxExpenseCheckAccount, Global.Company.CompanyId);
            }
            else if (ComboBoxExpensePaymentType.SelectedIndex == 2)
            {
                GroupBoxCreditCard.Location = GroupBoxCashExpense.Location;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxCashExpense.Visible = false;
                GroupBoxBankTransfer.Visible = false;
                GroupBoxCreditCard.Visible = true;
                DateTimePickerCreditDate.Format = Global.Company.DateFormat;
                DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboUtils.InitializeCreditCardAccountCombo(ComboBoxExpenseCreditCardAccount, Global.Company.CompanyId);

            }
            else if (ComboBoxExpensePaymentType.SelectedIndex == 3)
            {
                GroupBoxCashExpense.Visible = false;
                GroupBoxCreditCard.Visible = false;
                GroupBoxCheckInfomation.Visible = false;
                GroupBoxBankTransfer.Location = GroupBoxCashExpense.Location;
                GroupBoxBankTransfer.Visible = true;
                ComboUtils.InitializeBankAccountCombo(ComboBoxExpenseBankAccount, Global.Company.CompanyId);
            }

        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }



        private void BtnExpenseNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnExpenseSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxExpensePayee.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxExpensePayee.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnExpenseDelete_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorExpense.Text = "";
            if (string.IsNullOrEmpty(TextBoxExpenseId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this expense is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, ExpenseRefNo.Text), "Delete Confirm",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                long ExpenseID = Convert.ToInt64(TextBoxExpenseId.Text);
                Expense Expense = ExpenseManager.GetExpense(ExpenseID);
                if (Expense != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool DeleteResult = ExpenseManager.DeleteExpense(ExpenseID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        BtnExpenseNew.Select();
                        //LoadExpense();
                        RecentExpense();
                        this.formIsDirty = false;
                    }
                    else
                    {
                        ToolStripStatusLabelErrorExpense.Text = DeleteErrorText;
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                    return;
                }
            }
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            this.formIsDirty = false;
            BtnExpenseCancel.PerformClick();
            RecentExpense();
            return;
        }
        private void BtnExpensePrint_Click(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorExpense.Text = "";
            if (ExpenseManager.GetExpense(long.Parse(TextBoxExpenseId.Text)) != null)
            {
                PrinterSetup.TransactionPrintSetup(long.Parse(TextBoxExpenseId.Text), TransactionTypes.EXPENSE);
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                return;
            }
        }

        private void BtnExpenseCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxExpensePayee.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxExpensePayee.Select();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void BtnExpenseExit_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnExpenseSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                Expense ExpenseFromDB = new Expense();
                switch (ComboBoxExpensePaymentType.SelectedIndex)
                {
                    case 0:
                        Expense lExpense = GetExpenseFromForm();

                        if (lExpense.ExpenseId == 0)
                        {
                            string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.EXPENSE, (DateTime)DateTimePickerExpensePayment.Date);
                            if (!string.IsNullOrEmpty(RefNo))
                            {
                                lExpense.Reference = RefNo;
                                ExpenseFromDB = ExpenseManager.AddExpense(lExpense);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorExpense.Text = RefNoErrorMsg;
                            }
                        }
                        else
                        {
                            if (ExpenseManager.GetExpense(lExpense.ExpenseId) != null)
                            {
                                ExpenseFromDB = ExpenseManager.UpdateExpense(lExpense);
                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                                return;
                            }
                        }
                        break;
                    case 1:
                        CheckExpense lCheckExpense = GetCheckExpenseFromForm();
                        CheckExpense CheckExpenseFromDB = null;
                        if (lCheckExpense.ExpenseId == 0)
                        {
                            string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.EXPENSE, (DateTime)DateTimePickerExpensePayment.Date);
                            if (!string.IsNullOrEmpty(RefNo))
                            {
                                lCheckExpense.Reference = RefNo;
                                CheckExpenseFromDB = ExpenseManager.AddCheckExpense(lCheckExpense);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorExpense.Text = RefNoErrorMsg;
                            }
                        }
                        else
                        {
                            if (ExpenseManager.GetExpense(lCheckExpense.ExpenseId) != null)
                            {
                                CheckExpenseFromDB = ExpenseManager.UpdateCheckExpense(lCheckExpense);
                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                                return;
                            }
                        }
                        ExpenseFromDB.ExpenseId = CheckExpenseFromDB.ExpenseId;
                        break;
                    case 2:
                        CreditCardExpense lCreditCardExpense = GetCreditCardExpenseFromForm();
                        CreditCardExpense CreditCardExpenseFromDB = null;
                        if (lCreditCardExpense.ExpenseId == 0)
                        {
                            string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.EXPENSE, (DateTime)DateTimePickerExpensePayment.Date);
                            if (!string.IsNullOrEmpty(RefNo))
                            {
                                lCreditCardExpense.Reference = RefNo;
                                CreditCardExpenseFromDB = ExpenseManager.AddCreditCardExpense(lCreditCardExpense);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorExpense.Text = RefNoErrorMsg;
                            }
                        }
                        else
                        {
                            if (ExpenseManager.GetExpense(lCreditCardExpense.ExpenseId) != null)
                            {
                                CreditCardExpenseFromDB = ExpenseManager.UpdateCreditCardExpense(lCreditCardExpense);
                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                                return;
                            }
                        }
                        ExpenseFromDB.ExpenseId = CreditCardExpenseFromDB.ExpenseId;
                        break;
                    case 3:
                        BankTransferExpense lBankTransferExpense = GetBankTransferExpenseFromForm();
                        BankTransferExpense BankTransferExpenseFromDB = null;
                        if (lBankTransferExpense.ExpenseId == 0)
                        {
                            string RefNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.EXPENSE, (DateTime)DateTimePickerExpensePayment.Date);
                            if (!string.IsNullOrEmpty(RefNo))
                            {
                                lBankTransferExpense.Reference = RefNo;
                                BankTransferExpenseFromDB = ExpenseManager.AddBankTransferExpense(lBankTransferExpense);
                            }
                            else
                            {
                                ToolStripStatusLabelErrorExpense.Text = RefNoErrorMsg;
                                return;
                            }
                        }
                        else
                        {
                            if (ExpenseManager.GetExpense(lBankTransferExpense.ExpenseId) != null)
                            {
                                BankTransferExpenseFromDB = ExpenseManager.UpdateBankTransferExpense(lBankTransferExpense);
                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this expense is still valid.");
                                return;
                            }
                        }
                        ExpenseFromDB.ExpenseId = BankTransferExpenseFromDB.ExpenseId;
                        break;
                }


                //ExpenseManager.AddExpenseDetail(ExpenseFromDB);
                TextBoxExpenseId.Text = ExpenseFromDB.ExpenseId.ToString();
                ExpenseRefNo.Text = ExpenseManager.GetExpense(ExpenseFromDB.ExpenseId).Reference;
                LoadExpense(ExpenseFromDB.ExpenseId);
                EnableForm(false);
                if (string.IsNullOrEmpty(TextBoxExpenseSearch.Text))
                {
                    RecentExpense();
                }
                else
                {
                    ExpenseSearchGo_Click(sender, e);
                }

                ToolStripStatusLabelErrorExpense.Text = SaveSuccessText;
                BtnExpensePrint.Select();
                this.formIsDirty = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxExpensePayee.Text.Trim()))
            {
                TextBoxExpensePayee.Select();
                ToolStripStatusLabelErrorExpense.Text = ChooseSupplierErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxExpensePayee.Text.Trim()) && AccountManager.Instance.GetAccountById(long.Parse(TextBoxExpensePayee.Id)) == null)
            {
                ToolStripStatusLabelErrorExpense.Text = "Somting went wrong, please check this account is still valid.";
                TextBoxExpensePayee.Select();
                return false;
            }
            if (DateTimePickerExpensePayment.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerExpensePayment.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerExpensePayment.Focus();
                ToolStripStatusLabelErrorExpense.Text = EnterExpenseDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DateTimePickerExpensePayment.Date != null && !DateUtils.ValidDate_TillFinancialPeriodsStartEndDate(((DateTime)DateTimePickerExpensePayment.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DateTimePickerExpensePayment.Focus();
                ToolStripStatusLabelErrorExpense.Text = EnterExpenseDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxExpensePaymentType.SelectedIndex < 0)
            {
                ComboBoxExpensePaymentType.Select();
                ToolStripStatusLabelErrorExpense.Text = ChoosePaymentTypeErrorMsg;
                ResetTimmer();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxExpenseAmount.Text.Trim()) || float.Parse(TextBoxExpenseAmount.Text) == 0)
            {
                TextBoxExpenseAmount.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterAmountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && ComboBoxExpenseBankAccount.SelectedIndex < 0)
            {
                ComboBoxExpenseBankAccount.Select();
                ToolStripStatusLabelErrorExpense.Text = ChooseBankAccountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxBankTransfer.Visible == true && string.IsNullOrEmpty(TextBoxExpenseBankTransaction.Text.Trim()))
            {
                TextBoxExpenseBankTransaction.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterBankTranErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCheckInfomation.Visible == true && ComboBoxExpenseCheckAccount.SelectedIndex < 0)
            {
                ComboBoxExpenseCheckAccount.Select();
                ToolStripStatusLabelErrorExpense.Text = ChooseCheckAccountErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && (DateTimePickerExpenseCheckDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerExpenseCheckDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerExpenseCheckDate.Focus();
                ToolStripStatusLabelErrorExpense.Text = EnterCheckDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (GroupBoxCheckInfomation.Visible == true && string.IsNullOrEmpty(TextBoxExpenseCheckDocument.Text.Trim()))
            {
                TextBoxExpenseCheckDocument.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterCheckDocErrorMsg;
                ResetTimmer();
                return false;
            }


            if (GroupBoxCreditCard.Visible == true && ComboBoxExpenseCreditCardAccount.SelectedIndex < 0)
            {
                ComboBoxExpenseCreditCardAccount.Select();
                ToolStripStatusLabelErrorExpense.Text = ChooseCardAccountErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && TextBoxExpenseCreditTransaction.Text.Replace(" ", "") == "")
            {
                TextBoxExpenseCreditTransaction.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterCardTranErrorMsg;
                ResetTimmer();
                return false;
            }
            if (GroupBoxCreditCard.Visible == true && (DateTimePickerCreditDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerCreditDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)))
            {
                DateTimePickerCreditDate.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterCardDateErrorMsg;
                ResetTimmer();
                return false;
            }


            int Count = DataGridViewExpenseDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 3; j++)
                    {
                        if (DataGridViewExpenseDetails.Rows[i].Cells[j].Value == null || DataGridViewExpenseDetails.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || DataGridViewExpenseDetails.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            DataGridViewExpenseDetails.Select();
                            DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[j, i];
                            ToolStripStatusLabelErrorExpense.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewExpenseDetails.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                DataGridViewExpenseDetails.Select();
                DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[1, 0];
                ToolStripStatusLabelErrorExpense.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }

            ToolStripStatusLabelErrorExpense.Text = SaveSuccessText;
            if (float.Parse(TextBoxExpenseAmount.Text) != float.Parse(DataGridViewExpenseDetailsTotal.Rows[0].Cells[1].Value.ToString()))
            {
                TextBoxExpenseAmount.Select();
                ToolStripStatusLabelErrorExpense.Text = EnterAmountWrongDateErrorMsg;
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
            GroupBoxCreditCard.Visible = false;
            GroupBoxCheckInfomation.Visible = false;
            GroupBoxBankTransfer.Visible = false;
            DateTimePickerExpensePayment.Format = Global.Company.DateFormat;
            DateTimePickerExpensePayment.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerExpensePayment.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerExpensePayment.MaxDate = Global.getCurrentFiscalYearEndDate();
            DateTimePickerCreditDate.Format = Global.Company.DateFormat;
            DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerCreditDate.MinDate = Global.getCurrentFiscalYearStartDate();
            DateTimePickerCreditDate.MaxDate = Global.getCurrentFiscalYearEndDate();
            DateTimePickerExpenseCheckDate.Format = Global.Company.DateFormat;
            DateTimePickerExpenseCheckDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerExpenseCheckDate.MinDate = Global.getCurrentFiscalYearStartDate();
            TextBoxExpenseId.ResetText();
            TextBoxExpenseMemo.ResetText();
            ExpenseRefNo.Text = "000000";
            ToolStripStatusLabelErrorExpense.Text = "";
            TextBoxExpenseSearch.TextBox.ResetText();
            TextBoxExpenseCheckDocument.ResetText();
            TextBoxExpenseBankTransaction.ResetText();
            TextBoxExpenseAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxExpensePayee.ResetText();
            ComboBoxExpenseBankAccount.ResetText();
            ComboBoxExpenseBankAccount.SelectedIndex = -1;
            ComboBoxExpenseCheckAccount.ResetText();
            ComboBoxExpenseCheckAccount.SelectedIndex = -1;
            ComboBoxExpenseCreditCardAccount.ResetText();
            ComboBoxExpenseCreditCardAccount.SelectedIndex = -1;
            TextBoxExpenseCreditTransaction.ResetText();
            ComboBoxExpensePaymentType.SelectedIndex = 0;
            DataGridViewExpenseDetails.Rows.Clear();
            DataGridViewExpenseDetailsTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewExpenseDetailsTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewExpenseDetails.Columns["Amount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            LastExpenseRefNo.Text = CompanyManager.Instance.GetAccountPrevRef(Global.Company, EntryType.EXPENSE, (DateTime)DateTimePickerExpensePayment.Date!);
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxExpensePaymentType.Visible = true;
            TextBoxExpensePayee.ReadOnly = true;
            if (enable)
            {
                BtnExpenseNew.Enabled = !enable;
                BtnExpenseDelete.Enabled = !enable;
                BtnExpensePrint.Enabled = !enable;
                BtnExpenseCancel.Enabled = enable;
                BtnExpenseSave.Enabled = enable;
            }
            else
            {
                BtnExpenseNew.Enabled = !enable;
                BtnExpenseDelete.Enabled = !enable;
                BtnExpensePrint.Enabled = !enable;
                BtnExpenseCancel.Enabled = !enable;
                BtnExpenseSave.Enabled = !enable;
            }
        }
        private void DataGridViewExpenseDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 4 && !DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[4].ReadOnly)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewExpenseDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        DataGridViewExpenseDetails.Rows.RemoveAt(e.RowIndex);
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
        private void DataGridViewExpenseDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewExpenseDetails.CurrentCell.ColumnIndex == 3)
            {
                if (DataGridViewExpenseDetails.CurrentCell.Value == null || DataGridViewExpenseDetails.CurrentCell.Value.Equals(string.Empty))
                {
                    DataGridViewExpenseDetails.CurrentCell.Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                }
                else
                {
                    DataGridViewExpenseDetails.CurrentCell.Value = String.Format("{0:0.00}", float.Parse(DataGridViewExpenseDetails.CurrentCell.Value.ToString()));
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
            for (int i = 0; i < DataGridViewExpenseDetails.Rows.Count - 1; i++)
            {
                double Amount = (DataGridViewExpenseDetails.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewExpenseDetails.Rows[i].Cells[3].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewExpenseDetailsTotal.Rows[0].Cells[1].Value = String.Format("{0:0.00}", TotalAmount);
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewExpenseDetails.Rows.Count; i++)
            {
                DataGridViewExpenseDetails.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void DataGridViewExpenseDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[0].ReadOnly = true;
            DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[2].ReadOnly = true;
            DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[3].ReadOnly = true;
            DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[4].ReadOnly = true;
            if (DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[1].Value != null)
            {
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }
        private void DataGridViewExpenseDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void DataGridViewExpenseDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
                if (DataGridViewExpenseDetails.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewExpenseDetails_KeyPress1);
            }
            if (DataGridViewExpenseDetails.CurrentCell.ColumnIndex == 3)
            {
                KeypressValidation.AddContextMenuGridCell(e, DataGridViewExpenseDetails, "NumberDot", DataGridViewExpenseDetails.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewExpenseDetails_KeyPress);
                if (e.Control is DataGridViewTextBoxEditingControl)
                {
                    DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                    tb.KeyDown += DataGridViewExpenseDetails_KeyDown;
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void DataGridViewExpenseDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
            if (Account != null)
            {
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[0].Value = DataGridViewExpenseDetails.Rows.Count;
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[3].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[4].Value = "X";
                (DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                (DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                (DataGridViewExpenseDetails.Rows[e.RowIndex].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
            }
        }
        private void DataGridViewExpenseDetails_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewExpenseDetails.EditingControl).DroppedDown = false;
        }
        private void DataGridViewExpenseDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataGridViewExpenseDetails.Rows.Count > 0 && DataGridViewExpenseDetails.CurrentCell != null)
            {
                if (DataGridViewExpenseDetails.CurrentCell.ColumnIndex == 3)
                {
                    if (DataGridViewExpenseDetails.CurrentCell.Value != null)
                    {
                        KeypressValidation.Keypress_NumberDot(sender, e, DataGridViewExpenseDetails.CurrentCell.Value.ToString());
                    }
                    else
                    {
                        KeypressValidation.Keypress_Number(sender, e);
                    }
                }
            }
        }
        private void DataGridViewExpenseDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                KeypressValidation.Keypress_PasteChecking(sender, e, "NumberDot");
            }
        }
        private void TextBoxExpenseAmount_Leave(object sender, EventArgs e)
        {
            if (TextBoxExpenseAmount.Text == "." || TextBoxExpenseAmount.Text == "")
            {
                TextBoxExpenseAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            }
            else
            {
                TextBoxExpenseAmount.Text = String.Format("{0:0.00}", float.Parse(TextBoxExpenseAmount.Text));
            }
            RedistributeTotalAmount();
        }
        private void TextBoxExpenseAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Unselected = TextBoxExpenseAmount.Text;
            if (TextBoxExpenseAmount.SelectionLength > 0) { Unselected = TextBoxExpenseAmount.Text.Remove(TextBoxExpenseAmount.SelectionStart, TextBoxExpenseAmount.SelectionLength); }
            KeypressValidation.Keypress_NumberDot(sender, e, Unselected);
        }
        private void TextBoxExpenseCreditTransaction_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxExpenseCheckDocument_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        private void TextBoxExpenseBankTransaction_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Keypress_NameChecking(sender, e);
        }
        //private void ComboBoxExpenseAccount_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    this.ComboBoxExpenseAccount.DroppedDown = false;
        //}
        private void ComboBoxExpensePaymentType_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxExpensePaymentType.DroppedDown = false;
        }
        private void ComboBoxExpenseCreditCardAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxExpenseCreditCardAccount.DroppedDown = false;
        }
        private void ComboBoxExpenseCheckAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxExpenseCheckAccount.DroppedDown = false;
        }
        private void ComboBoxExpenseBankAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxExpenseBankAccount.DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnExpenseSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnExpenseNew.Enabled)
                {
                    BtnExpenseNew.PerformClick();
                }
                else
                {
                    if (TextBoxExpensePayee.Focused)
                        BtnExpenseNewSupplier.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnExpenseDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnExpensePrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnExpenseSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnExpenseCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExpenseExit.PerformClick();
                return true;
            }
            if (keyData == (Keys.Tab) && ExpenseSearchGo.Selected)
            {
                TextBoxExpensePayee.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnExpenseSave)
            {
                BtnExpenseCancel.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnExpenseSave)
            {
                BtnExpenseExit.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnExpenseCancel)
            {
                BtnExpenseExit.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnExpenseCancel)
            {
                BtnExpenseSave.Select();
                return true;
            }
            if (keyData == (Keys.Right) && ActiveControl == BtnExpenseExit)
            {
                BtnExpenseCancel.Select();
                return true;
            }
            if (keyData == (Keys.Left) && ActiveControl == BtnExpenseExit)
            {
                BtnExpenseSave.Select();
                return true;
            }
            try
            {
                if (DataGridViewExpenseDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewExpenseDetails.CurrentCell.ColumnIndex == 3)
                    {
                        if (DataGridViewExpenseDetails.CurrentCell.RowIndex != DataGridViewExpenseDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewExpenseDetails.CurrentCell.ColumnIndex == 1)
                    {
                        if (DataGridViewExpenseDetails.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
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
            TimerExpense.Stop();
            TimerExpense.Start();
        }
        private void TimerExpense_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorExpense.Visible = !this.ToolStripStatusLabelErrorExpense.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerExpense.Stop();
                ToolStripStatusLabelErrorExpense.Visible = true;
            }
        }
        private void TextBoxExpenseMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxExpenseMemo.SelectionLength = 0;
                e.IsInputKey = true;
                if (GroupBoxCashExpense.Visible == true)
                {
                    DataGridViewExpenseDetails.Select();
                    DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[1, 0];
                }
                else if (GroupBoxCheckInfomation.Visible == true) { ComboBoxExpenseCheckAccount.Select(); }
                else if (GroupBoxCreditCard.Visible == true) { ComboBoxExpenseCreditCardAccount.Select(); }
                else if (GroupBoxBankTransfer.Visible == true) { ComboBoxExpenseBankAccount.Select(); }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxExpenseMemo.SelectionLength = 0;
                e.IsInputKey = true;
                TextBoxExpenseAmount.Select();
            }
        }
        private void TextBoxExpenseCheckDocument_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewExpenseDetails.Select();
                DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerExpenseCheckDate.Focus();
            }
        }
        private void DateTimePickerCreditDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewExpenseDetails.Select();
                DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxExpenseCreditTransaction.Focus();
            }
        }
        private void TextBoxExpenseBankTransaction_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewExpenseDetails.Select();
                DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[1, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxExpenseBankAccount.Focus();
            }
        }
        private void BtnExpenseSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxExpensePayee.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewExpenseDetails.Rows.Count > 0)
                {
                    DataGridViewExpenseDetails.Select();
                    DataGridViewExpenseDetails.CurrentCell = DataGridViewExpenseDetails[3, DataGridViewExpenseDetails.Rows.Count - 1];
                }
            }
        }
        private void TextBoxExpenseMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }
        private void RedistributeTotalAmount()
        {
            float TotalAmount = GetReceiptTotalAmount();
            for (int i = 0; i < DataGridViewExpenseDetails.Rows.Count - 1; i++)
            {
                float LineItemTotal = (float)GetLineItemTotal(i);
                if (/*DataGridUtils.readColumValueAsFloat(DataGridViewExpenseDetails, i, 3) == 0 &&*/ TotalAmount > 0 && !DataGridViewExpenseDetails.Rows[i].IsNewRow)
                {
                    float AmountToApply = (TotalAmount > LineItemTotal ? LineItemTotal == 0 ? TotalAmount : LineItemTotal : TotalAmount);
                    DataGridViewExpenseDetails.Rows[i].Cells[3].Value = String.Format("{0:0.00}", AmountToApply);
                    TotalAmount = TotalAmount - AmountToApply;
                }
            }
            ComputeFormTotal();
        }
        private float GetReceiptTotalAmount()
        {
            try
            {
                return (float)Convert.ToDouble(TextBoxExpenseAmount.Text);
            }
#pragma warning disable 0168
            catch (Exception e)
            {
                return 0F;
            }
#pragma warning restore 0168
        }
        private double GetLineItemTotal(int row)
        {
            try
            {
                return (DataGridViewExpenseDetails.Rows[row].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewExpenseDetails.Rows[row].Cells[3].Value.ToString()));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void GridViewExpenseSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                long ExpenseNumber = (long)GridViewExpenseSearch.Rows[e.RowIndex].Cells[4].Value;
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnExpenseSave_Click(sender, e);
                            LoadExpense(ExpenseNumber);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadExpense(ExpenseNumber);
                    }
                }
                else
                {
                    LoadExpense(ExpenseNumber);
                }
            }
        }
        private void LoadExpense(long ExpenseId)
        {
            ResetForm();
            EnableForm(false);
            Expense Expense = ExpenseManager.GetExpense(ExpenseId);
            if (Expense != null)
            {
                ExpenseRefNo.Text = Expense.Reference;
                TextBoxExpenseId.Text = Expense.ExpenseId.ToString();
                TextBoxExpensePayee.Text = Expense.Payee.Name;
                TextBoxExpensePayee.Id = Expense.Payee.Id.ToString();
                DateTimePickerExpensePayment.Date = (DateTime)DateUtils.ToDate(Expense.TransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxExpensePaymentType.SelectedIndex = ComboBoxExpensePaymentType.FindStringExact(((PaymentType)Expense.TransctionType).ToString());
                TextBoxExpenseMemo.Text = Expense.Memo;
                TextBoxExpenseAmount.Text = String.Format("{0:0.00}", Expense.Amount);
            }
            else
            {
                MessageBox.Show("Somting went wrong, please check this expense is still valid.");
                return;
            }
            if (Expense.TransctionType == PaymentType.CASH)
            {
                CashExpense CashExpense = ExpenseManager.GetCashExpense(ExpenseId);

            }
            if (Expense.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferExpense BankTransferExpense = ExpenseManager.GetBankTransferExpense(ExpenseId);
                ComboBoxExpenseBankAccount.SelectedIndex = ComboBoxExpenseBankAccount.FindStringExact(BankTransferExpense.BankTransfer.Name);
                TextBoxExpenseBankTransaction.Text = BankTransferExpense.TransactionNumber;
            }
            if (Expense.TransctionType == PaymentType.CHECK)
            {
                CheckExpense CheckExpense = ExpenseManager.GetCheckExpense(ExpenseId);
                TextBoxExpenseCheckDocument.Text = CheckExpense.DocumentNumber;
                DateTimePickerExpenseCheckDate.Date = (DateTime)DateUtils.ToDate(CheckExpense.DocumentDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                ComboBoxExpenseCheckAccount.SelectedIndex = ComboBoxExpenseCheckAccount.FindStringExact(CheckExpense.BankAccount.Name);

            }
            if (Expense.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardExpense CreditCardExpense = ExpenseManager.GetCreditCardExpense(ExpenseId);
                ComboBoxExpenseCreditCardAccount.SelectedIndex = ComboBoxExpenseCreditCardAccount.FindStringExact(((Account)CreditCardExpense.CCAccount).ToString());

                DateTimePickerCreditDate.Date = (DateTime)DateUtils.ToDate(CreditCardExpense.CCTransactionDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxExpenseCreditTransaction.Text = CreditCardExpense.CCTransactionNumber;
            }
            if (Expense.ExpenseDetails.Count > 0)
            {
                DataGridViewExpenseDetails.Rows.Add(Expense.ExpenseDetails.Count);
                int i = 0;
                IList<Account> Account = AccountManager.Instance.GetAllGeneralAccountsByCompanyId(Global.Company.CompanyId);
                foreach (var ExpenseDetail in Expense.ExpenseDetails)
                {
                    (DataGridViewExpenseDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = null;
                    (DataGridViewExpenseDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                    (DataGridViewExpenseDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).ValueMember = "Id";
                    (DataGridViewExpenseDetails.Rows[i].Cells[1] as DataGridViewComboBoxCell).DisplayMember = "Name";
                    DataGridViewExpenseDetails.Rows[i].Cells[4].Value = "X";
                    DataGridViewExpenseDetails.Rows[i].Cells[0].Value = i + 1;
                    DataGridViewExpenseDetails.Rows[i].Cells[1].Value = ExpenseDetail.AccountId;
                    DataGridViewExpenseDetails.Rows[i].Cells[2].Value = ExpenseDetail.Description;
                    DataGridViewExpenseDetails.Rows[i].Cells[3].Value = String.Format("{0:0.00}", ExpenseDetail.Amount);
                    i++;
                }
                Row_Added();
                ReSequence();
            }
            //BtnExpenseNew.Enabled = false;
            this.formIsDirty = false;
        }
        private void ExpenseSearchGo_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                GridViewExpenseSearch.Rows.Clear();
                IList<Expense> ExpenseInfo = SearchExpense();
                LoadExpense(ExpenseInfo);
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorExpense.Text = "";
            if (string.IsNullOrEmpty(TextBoxExpenseSearch.Text))
            {
                ToolStripStatusLabelErrorExpense.Text = SearchBoxEmptyErrorMsg;
                TextBoxExpenseSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private IList<Expense> SearchExpense()
        {
            string text = TextBoxExpenseSearch.Text;
            IList<Expense> ExpenseInfo = null;
            if (TextUtils.isReferenceNumber(text))
            {
                ExpenseInfo = ExpenseManager.GetExpenseByReferenceNo(text, Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(text, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat);
                ExpenseInfo = ExpenseManager.GetExpenseByDate(Date, Global.Company.CompanyId);
            }
            else
            {
                ExpenseInfo = ExpenseManager.GetExpenseByPayeeName(text, Global.Company.CompanyId);
            }
            return ExpenseInfo;
        }
        private void RecentExpense()
        {
            GridViewExpenseSearch.Rows.Clear();
            IList<Expense> ExpenseInfo = ExpenseManager.GetRecentExpense(Global.Company.CompanyId);
            LoadExpense(ExpenseInfo);
        }
        public void LoadExpense(IList<Expense> ExpenseInfo)
        {
            ToolStripStatusLabelErrorExpense.Text = "";
            if (ExpenseInfo.Count > 0)
            {
                GridViewExpenseSearch.Rows.Add(ExpenseInfo.Count);
                int i = 0;
                foreach (var lExpenseInfo in ExpenseInfo)
                {
                    GridViewExpenseSearch.Rows[i].Cells[0].Value = lExpenseInfo.TransactionDate.ToShortDateString();
                    GridViewExpenseSearch.Rows[i].Cells[1].Value = lExpenseInfo.Reference;
                    GridViewExpenseSearch.Rows[i].Cells[2].Value = lExpenseInfo.Payee.Name;
                    GridViewExpenseSearch.Rows[i].Cells[3].Value = lExpenseInfo.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewExpenseSearch.Rows[i].Cells[4].Value = lExpenseInfo.ExpenseId;
                    i++;
                }
            }
            else
            {
                ToolStripStatusLabelErrorExpense.Text = SearchOutput;
            }
        }
        private void TextBoxExpenseSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxExpenseSearch.Text))
            {
                RecentExpense();
            }
        }
        private void TextBoxExpensePayee_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerExpensePayment.maskedTextBox.Focus();
                DateTimePickerExpensePayment.maskedTextBox.SelectAll();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnExpenseSave.Select();
            }
        }
        private void DataGridViewExpenseDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Account Acc = AccountManager.Instance.GetAccountByName(DataGridViewExpenseDetails.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Acc != null)
                {
                    DataGridViewExpenseDetails.CurrentCell.Value = Acc.Id;
                }
            }
        }
        private void TextBoxExpenseSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExpenseSearchGo.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                ExpenseSearchGo.PerformClick();
            }
        }
        private void FormExpense_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxExpensePayee.Focus();
                    e.Cancel = true;
                }
            }
        }
        private void BtnExpenseSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxExpensePayee.Id == null ? 0L : long.Parse(TextBoxExpensePayee.Id);
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
                    TextBoxExpensePayee.Text = Account.Name;
                    TextBoxExpensePayee.Id = SupplierId.ToString();
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
            TextBoxExpensePayee.Select();
            Cursor.Current = Cursors.Default;
        }
        private void BtnExpenseNewSupplier_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxExpensePayee.Id == null ? 0L : long.Parse(TextBoxExpensePayee.Id);
            SupplierId = 0L;
            if (e.ClickedItem.Text == "Account")
            {
                FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
                FormGeneralAccounts.CreateAccountOnLoad = true;
                FormGeneralAccounts.ShowDialog(this);
                ResetToAccounts();
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
                    TextBoxExpensePayee.Text = Account.Name;
                    TextBoxExpensePayee.Id = SupplierId.ToString();
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
                    this.formIsDirty = false;
                }
            }
            TextBoxExpensePayee.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetToAccounts()
        {
            List<AccountHelperData> Account = AccountHelper.GetHelpData(1, false, false, true, false, null, Global.Company);
            if (DataGridViewExpenseDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DataGridViewExpenseDetails.Rows)
                {
                    (row.Cells[1] as DataGridViewComboBoxCell).DataSource = Account;
                }
            }
        }

        private void DataGridViewExpenseDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
            }
        }
        private void ComboBoxExpensePaymentType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxExpenseAmount.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DateTimePickerExpensePayment.maskedTextBox.Focus();
                DateTimePickerExpensePayment.maskedTextBox.SelectAll();
            }
        }
    }
}
