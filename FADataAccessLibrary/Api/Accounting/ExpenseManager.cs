using fa.context;
using fa.model.Accounting.Transactions;
using Fa.api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class ExpenseManager
    {
        private static volatile ExpenseManager instance;
        private static object syncRoot = new Object();
        ExpenseManager()
        {

        }
        public static ExpenseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ExpenseManager();
                    }
                }

                return instance;
            }
        }

        public Expense AddExpense(Expense expense)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddExpense(expense,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        expense = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return expense;
        }
        public BankTransferExpense AddBankTransferExpense(BankTransferExpense bankTransferExpense)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddBankTransferExpense(bankTransferExpense,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        bankTransferExpense = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return bankTransferExpense;
        }
        public CheckExpense AddCheckExpense(CheckExpense checkExpense)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCheckExpense(checkExpense,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        checkExpense = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return checkExpense;
        }
        public CreditCardExpense AddCreditCardExpense(CreditCardExpense creditCardExpense)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCreditCardExpense(creditCardExpense,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        creditCardExpense = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return creditCardExpense;
        }
        public Expense AddExpense(Expense expense, AccountMasterContext Context)
        {
            Context.Expenses.Add(expense);
            Context.SaveChanges();
            ExpenseDoubleEntryManager.Instance.RecordExpense(expense, Context);
            return expense;
        }
        public BankTransferExpense AddBankTransferExpense(BankTransferExpense bankTransferExpense, AccountMasterContext Context)
        {
            Context.BankTransferExpenses.Add(bankTransferExpense);
            Context.SaveChanges();
            ExpenseDoubleEntryManager.Instance.RecordExpense(bankTransferExpense, Context);
            return bankTransferExpense;
        }
        public CheckExpense AddCheckExpense(CheckExpense checkExpense, AccountMasterContext Context)
        {
            Context.CheckExpenses.Add(checkExpense);
            Context.SaveChanges();
            ExpenseDoubleEntryManager.Instance.RecordExpense(checkExpense, Context);
            return checkExpense;
        }
        public CreditCardExpense AddCreditCardExpense(CreditCardExpense creditCardExpense, AccountMasterContext Context)
        {
            Context.CreditCardExpenses.Add(creditCardExpense);
            Context.SaveChanges();
            ExpenseDoubleEntryManager.Instance.RecordExpense(creditCardExpense, Context);
            return creditCardExpense;
        }
        public Expense UpdateExpense(Expense Expense)
        {
            Expense ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ExpenseInfo = Context.Expenses.Find(Expense.ExpenseId);
                        if (ExpenseInfo != null)
                        {
                            if (ExpenseInfo.TransctionType == PaymentType.CASH)
                            {
                                Expense ExpenseInfoFromDB = GetExpense(Expense.ExpenseId);
                                if (ExpenseInfoFromDB.ExpenseDetails.Count > 0)
                                {
                                    Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfoFromDB.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(ExpenseInfo).CurrentValues.SetValues(Expense);
                                foreach (ExpenseDetail Detail in Expense.ExpenseDetails)
                                {
                                    Detail.ExpenseId = Expense.ExpenseId;
                                    Context.ExpenseDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ExpenseDoubleEntryManager.Instance.RecordExpense(Expense, Context);
                            }
                            else
                            {
                                if (DeleteExpense(ExpenseInfo.ExpenseId, Context))
                                {
                                    Expense.ExpenseId = 0L;
                                    AddExpense(Expense, Context);
                                }
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Expense = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Expense;
        }

        public BankTransferExpense UpdateBankTransferExpense(BankTransferExpense BankTransferExpense)
        {
            BankTransferExpense BankTransferExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Expense ExpenseInfo = Context.Expenses.Find(BankTransferExpense.ExpenseId);
                        if (ExpenseInfo.TransctionType == PaymentType.BANKTRANSFER)
                        {
                            BankTransferExpenseInfo = Context.BankTransferExpenses.Find(BankTransferExpense.ExpenseId);
                            if (BankTransferExpenseInfo != null)
                            {
                                BankTransferExpense ExpenseInfoFromDB = GetBankTransferExpense(BankTransferExpense.ExpenseId);
                                if (ExpenseInfoFromDB.ExpenseDetails.Count > 0)
                                {
                                    Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfoFromDB.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(BankTransferExpenseInfo).CurrentValues.SetValues(BankTransferExpense);
                                foreach (ExpenseDetail Detail in BankTransferExpense.ExpenseDetails)
                                {
                                    Detail.ExpenseId = BankTransferExpense.ExpenseId;
                                    Context.ExpenseDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ExpenseDoubleEntryManager.Instance.RecordExpense(BankTransferExpense, Context);
                            }
                        }
                        else
                        {
                            if (DeleteExpense(ExpenseInfo.ExpenseId, Context))
                            {
                                BankTransferExpense.ExpenseId = 0L;
                                AddBankTransferExpense(BankTransferExpense, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BankTransferExpense = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return BankTransferExpense;
        }

        public CheckExpense UpdateCheckExpense(CheckExpense CheckExpense)
        {
            CheckExpense CheckExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
        {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Expense ExpenseInfo = Context.Expenses.Find(CheckExpense.ExpenseId);
                        if (ExpenseInfo.TransctionType == PaymentType.CHECK)
                        {
                            CheckExpenseInfo = Context.CheckExpenses.Find(CheckExpense.ExpenseId);
                            if (CheckExpenseInfo != null)
                            {
                                CheckExpense ExpenseInfoFromDB = GetCheckExpense(CheckExpense.ExpenseId);
                                if (ExpenseInfoFromDB.ExpenseDetails.Count > 0)
                                {
                                    Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfoFromDB.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CheckExpenseInfo).CurrentValues.SetValues(CheckExpense);
                                foreach (ExpenseDetail Detail in CheckExpense.ExpenseDetails)
                                {
                                    Detail.ExpenseId = CheckExpense.ExpenseId;
                                    Context.ExpenseDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ExpenseDoubleEntryManager.Instance.RecordExpense(CheckExpense, Context);
                            }
                        }
                        else
                        {
                            if (DeleteExpense(ExpenseInfo.ExpenseId, Context))
                            {
                                CheckExpense.ExpenseId = 0L;
                                AddCheckExpense(CheckExpense, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CheckExpense = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CheckExpense;
        }

        public CreditCardExpense UpdateCreditCardExpense(CreditCardExpense CreditCardExpense)
        {
            CreditCardExpense CreditCardExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Expense ExpenseInfo = Context.Expenses.Find(CreditCardExpense.ExpenseId);
                        if (ExpenseInfo.TransctionType == PaymentType.CREDITCARD)
                        {
                            CreditCardExpenseInfo = Context.CreditCardExpenses.Find(CreditCardExpense.ExpenseId);
                            if (CreditCardExpenseInfo != null)
                            {
                                CreditCardExpense ExpenseInfoFromDB = GetCreditCardExpense(CreditCardExpense.ExpenseId);
                                if (ExpenseInfoFromDB.ExpenseDetails.Count > 0)
                                {
                                    Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfoFromDB.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CreditCardExpenseInfo).CurrentValues.SetValues(CreditCardExpense);
                                foreach (ExpenseDetail Detail in CreditCardExpense.ExpenseDetails)
                                {
                                    Detail.ExpenseId = CreditCardExpense.ExpenseId;
                                    Context.ExpenseDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ExpenseDoubleEntryManager.Instance.RecordExpense(CreditCardExpense, Context);
                            
                            }
                        }
                        else
                        {
                            if (DeleteExpense(ExpenseInfo.ExpenseId, Context))
                            {
                                CreditCardExpense.ExpenseId = 0L;
                                AddCreditCardExpense(CreditCardExpense, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CreditCardExpense = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CreditCardExpense;
        }
        public Boolean DeleteExpense(long ExpenseId, AccountMasterContext Context)
        {
            Boolean Deleted = false;            
            try
            {
                Expense ExpenseInfo = Context.Expenses.Include("ExpenseDetails").FirstOrDefault(x => x.ExpenseId == ExpenseId);
                if (ExpenseInfo.ExpenseDetails.Count > 0)
                {
                    ExpenseDetail ExpenseDetail = Context.ExpenseDetails.FirstOrDefault(x => x.ExpenseId == ExpenseInfo.ExpenseId);
                    if (ExpenseDetail != null)
                    {
                        Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfo.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                    }
                }
                ExpenseInfo = Context.Expenses.Find(ExpenseId);
                Context.Expenses.Remove(ExpenseInfo);
                Context.SaveChanges();
                ExpenseDoubleEntryManager.Instance.DeleteExpenses(ExpenseInfo, Context);
                Deleted = true;
            }
            catch (Exception e)
            {
                Deleted = false;
            }
            return Deleted;
        }
        public Boolean DeleteExpense(long ExpenseId)
        {
            Boolean Deleted = false;
            Expense ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ExpenseInfo = Context.Expenses.Include("ExpenseDetails").FirstOrDefault(x => x.ExpenseId == ExpenseId);
                        if (ExpenseInfo.ExpenseDetails.Count > 0)
                        {
                            ExpenseDetail ExpenseDetail = Context.ExpenseDetails.FirstOrDefault(x => x.ExpenseId == ExpenseInfo.ExpenseId);
                            if (ExpenseDetail != null)
                            {
                                Context.ExpenseDetails.Where(p => p.ExpenseId == ExpenseInfo.ExpenseId).ToList().ForEach(p => Context.ExpenseDetails.Remove(p));
                            }
                        }
                        ExpenseInfo = Context.Expenses.Find(ExpenseId);
                        Context.Expenses.Remove(ExpenseInfo);
                        Context.SaveChanges();
                        ExpenseDoubleEntryManager.Instance.DeleteExpenses(ExpenseInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return Deleted;
        }
        public IList<Expense> GetExpenseByReferenceNo(string RefNo, long CompanyId)
        {
            IList<Expense> ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ExpenseInfo = Context.Expenses.Include("ExpenseDetails").Include("Payee").Where(x => x.Reference == RefNo && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Expense>();
                return ExpenseInfo;
            }
        }
        public IList<Expense> GetExpenseByDate(DateTime? Date, long CompanyId)
        {
            IList<Expense> ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ExpenseInfo = Context.Expenses.Include("ExpenseDetails").Include("Payee").Where(x => x.TransactionDate == Date && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Expense>();
                return ExpenseInfo;
            }
        }
        public IList<Expense> GetExpenseByPayeeName(string PayeeName, long CompanyId)
        {
            IList<Expense> ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ExpenseInfo = Context.Expenses.Include("ExpenseDetails").Include("Payee").Where(x => (x.Payee.Name.Contains(PayeeName)|| x.Reference.Contains(PayeeName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Expense>();
                return ExpenseInfo;
            }
        }
        public IList<Expense> GetRecentExpense(long CompanyId)
        {
            IList<Expense> ExpenseInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentExpenseInfo = Context.Expenses.Include("ExpenseDetails").Include("Payee").Where(x =>x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Expense>().Take(20);
                if (RecentExpenseInfo != null)
                {
                    ExpenseInfo = RecentExpenseInfo.ToList();
                }
                return ExpenseInfo;
            }
        }

        public Expense GetExpense(long ExpenseId)
        {
            Expense Expense = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Expense = Context.Expenses.Include("ExpenseDetails").Include("Payee").FirstOrDefault(x => x.ExpenseId == ExpenseId);
            }
            return Expense;
        }
        public CashExpense GetCashExpense(long ExpenseId)
        {
            CashExpense CashExpense = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CashExpense = Context.CashExpenses.Include("ExpenseDetails").Include("CashAccount").FirstOrDefault(x => x.ExpenseId == ExpenseId);
            }
            return CashExpense;
        }
        public BankTransferExpense GetBankTransferExpense(long ExpenseId)
        {
            BankTransferExpense BankTransferExpense = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BankTransferExpense = Context.BankTransferExpenses.Include("ExpenseDetails").Include("BankTransfer").FirstOrDefault(x => x.ExpenseId == ExpenseId);
            }
            return BankTransferExpense;
        }
        public CheckExpense GetCheckExpense(long ExpenseId)
        {
            CheckExpense CheckExpense = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CheckExpense = Context.CheckExpenses.Include("ExpenseDetails").Include("BankAccount").FirstOrDefault(x => x.ExpenseId == ExpenseId);
            }
            return CheckExpense;
        }
        public CreditCardExpense GetCreditCardExpense(long ExpenseId)
        {
            CreditCardExpense CreditCardExpense = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditCardExpense = Context.CreditCardExpenses.Include("ExpenseDetails").Include("CCAccount").FirstOrDefault(x => x.ExpenseId == ExpenseId);
            }
            return CreditCardExpense;
        }

    }
}
