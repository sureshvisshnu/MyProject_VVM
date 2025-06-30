using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Fa.api.Accounting.DoubleEntry
{
    class ExpenseDoubleEntryManager
    {
        static readonly string SupplierEntryForExpense = "Expense {0} {1}" ;
        static readonly string AccountEntryForExpense = "Expense {0} {1} {2}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile ExpenseDoubleEntryManager instance;
        private static object syncRoot = new Object();
        ExpenseDoubleEntryManager()
        {
        }
        public static ExpenseDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ExpenseDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordExpense(Expense Expense, AccountMasterContext Context)
        {
            DeleteExpenses(Expense, Context);
            RecordExpenses(Expense, Context);
        }
        //Deleteing all exisiting entries for the given company      
        public void DeleteExpenses(Expense Expense, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Expense.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Expense && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Expense.Reference).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordExpenses(Expense Expense, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Expense.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            //Context.DoubleEntries.Add(GetPayeeEntryForExpense(Expense));
            Context.DoubleEntries.Add(GetAccountExpenseEntry(Expense));
            Context.DoubleEntries.AddRange(getExpenseEntryForExpense(Expense));
            Context.SaveChanges();            
        }
        public DayBook GetPayeeEntryForExpense(Expense Expense)
        {
            DayBook dBook = new DayBook
            {
                AccountId = Expense.PayeeId,
                CompanyId = Expense.CompanyId,
                CostCenterId = Expense.CostCenterId,
                Date = Expense.TransactionDate,
                Description = string.Format(SupplierEntryForExpense, Expense.Reference, Expense.Memo),
                ReferenceTrasnactionId = Expense.Reference,
                TransactionType = DaybookTransactionType.Expense
            };
            dBook.Debit(Expense.Amount);
            return dBook;
        }
        static readonly string ExpenseCashEntryCaption = "Expense {0}, {1} by {2}";
        static readonly string ExpenseCheckEntryCaption = "Expense {0}, Cheque/DD:{1} Dated:{2} Trasnsfer Id:{3}";
        static readonly string ExpenseBankEntryCaption = "Expense {0}, Bank:{1} Dated:{2} Trasnsfer Id:{3}";
        static readonly string ExpenseCardEntryCaption = "Expense {0}, CreditCard:{1} Dated:{2} Trasnsfer Id:{3}";
        public DayBook GetAccountExpenseEntry(Expense Expense)
        {
            Company Company = CompanyManager.Instance.GetCompany(Expense.CompanyId);
            if (Company.CashOnHandAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account Account = AccountManager.Instance.GetAccountById((long)Expense.PayeeId);
            if (Account == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account lAccount =null;
            string Desc = string.Empty;
            if (Expense.TransctionType == PaymentType.CASH)
            {
                lAccount = Company.CashOnHandAccount;
                Desc = string.Format(ExpenseCashEntryCaption, Expense.Reference, Account.AccountGroup.Name, Account.DisplayAs);
            }
            else if (Expense.TransctionType == PaymentType.CHECK)
            {
                CheckExpense CheckExpense = (CheckExpense)Expense;
                lAccount =AccountManager.Instance.GetAccountById((long)CheckExpense.BankAccountId);
                Desc = string.Format(ExpenseCheckEntryCaption, Expense.Reference, CheckExpense.DocumentNumber, CheckExpense.DocumentDate, lAccount.DisplayAs);
            }
            else if (Expense.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferExpense BankTransferExpense = ((BankTransferExpense)Expense);
                lAccount = AccountManager.Instance.GetAccountById((long)BankTransferExpense.BankTransferId);
                Desc = string.Format(ExpenseBankEntryCaption, Expense.Reference, BankTransferExpense.TransactionNumber, BankTransferExpense.TransactionDate, lAccount.DisplayAs);
            }
            else if (Expense.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardExpense CreditCardExpense = ((CreditCardExpense)Expense);
                lAccount = AccountManager.Instance.GetAccountById((long)CreditCardExpense.CCAccountId);
                Desc = string.Format(ExpenseCardEntryCaption, Expense.Reference, CreditCardExpense.CCTransactionNumber, CreditCardExpense.CCTransactionDate, lAccount.DisplayAs);
            }
            DayBook dBook = new DayBook
            {
                AccountId = lAccount.Id,
                CompanyId = Expense.CompanyId,
                CostCenterId = Expense.CostCenterId,
                Date = Expense.TransactionDate,
                Description = Desc,
                ReferenceTrasnactionId = Expense.Reference,
                TransactionType = DaybookTransactionType.Expense
            };
            dBook.Credit(Expense.Amount);
            return dBook;
        }
        public List<DayBook> getExpenseEntryForExpense(Expense Expense)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            foreach (ExpenseDetail ExpenseDetail in Expense.ExpenseDetails)
            {
                Account Payee = AccountManager.Instance.GetAccountById((long)Expense.PayeeId);
                Account Account = AccountManager.Instance.GetAccountById((long)ExpenseDetail.AccountId);                                            
                if (Account != null)
                {
                    DayBook dBook = new DayBook
                    {
                        AccountId = Account.Id,
                        CompanyId = Expense.CompanyId,
                        CostCenterId = Expense.CostCenterId,
                        Date = Expense.TransactionDate,
                        ReferenceTrasnactionId = Expense.Reference,
                        TransactionType = DaybookTransactionType.Expense,
                        Description = string.Format(AccountEntryForExpense, Expense.Reference,(!String.IsNullOrEmpty(ExpenseDetail.Description)? ","+ExpenseDetail.Description : "") ,  ", Paid to " + Payee.DisplayAs),
                    };
                    dBook.Debit(ExpenseDetail.Amount);
                    dayBooks.Add(dBook);
                }               
            }
            return dayBooks;
        }
    }
}
