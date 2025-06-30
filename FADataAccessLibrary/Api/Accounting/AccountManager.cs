using fa.model.Accounting.Masters;
using fa.context;
using System.Data;
using Microsoft.EntityFrameworkCore;
using fa.model.Accounting.Transaction;
using fa.Data;
using Fa.report.accounting.master;

namespace fa.api.Accounting
{
    public class AccountManager
    {
        public IList<Account> ListCompanySalesTaxReceivableAccounts(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListInvoiceService(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListDebitNoteService(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListCreditNoteService(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListAllAccountByGroupIds(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListAllAccountByIds(long CompanyId, long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.Id) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        public IList<Account> ListAllCreditDebitAccount(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where (Account.AccountGroupId == 58) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }

        // All our manager classes are singleton objects, do not create any member variables.
        private static volatile AccountManager instance;
        private static object syncRoot = new Object();

        AccountManager()
        {

        }
        
        //Implementing the static instance of every manager class.
        public static AccountManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountManager();
                    }
                }

                return instance;
            }
        }

        public Account GetAccountById(long AccountId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.Include("AccountGroup").Include("AccountGroup.ParentAccountGroup").Include("AccountGroup.AccountGroupClassification").FirstOrDefault(x=>x.Id==AccountId);
                return AccountInfo;
            }
        }
        public IList<Account> ListAccountByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where Account.CompanyId == CompanyId where Account.AccountType == AccountType.ACCOUNT select Account).OrderBy(x=>x.Name).ToList();
                return AccountInfo;
            }
        }
        
        public IList<Account> GetAllGeneralAccountsByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts.Include("AccountGroup") where Account.CompanyId == CompanyId where Account.AccountType == AccountType.ACCOUNT select Account).OrderBy(y=>y.Name).ToList();
                return AccountInfo;
            }
        }

        public IList<Account> GetAllAccountsByCompanyId(long CompanyId)
        {
            IList<Account> AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = (from Account in Context.Accounts where Account.CompanyId == CompanyId select Account).ToList();
                
            }
            return AccountInfo;
        }

        public IList<Account> ListParentAccountByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts.Include("AccountGroup").Include("AccountGroup.ParentAccountGroup") where Account.CompanyId == CompanyId where Account.ParentAccountId.Equals(null) where !Context.Customers.Any(customer => (customer.Id == Account.Id)) where !Context.Suppliers.Any(suplier => (suplier.Id == Account.Id)) select Account).ToList();
                return AccountInfo;
            }
        }
        
        public Account CheckAllAccountByName(String AccountrName, long CompanyId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.FirstOrDefault(x => x.Name == AccountrName && x.CompanyId == CompanyId);
                return AccountInfo;
            }
        }
        
        public IList<Account> GetAllGeneralAccountsByCompanyIdSearchTextWithHelpFilter(String AccountrName, Company Company, long HelpId)
        {
            IList<Account> AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountGroupForHelp ahp = Context.AccountGroupForHelp.Where(x => x.AccountGroupForHelpId == HelpId).SingleOrDefault();
                if(ahp!=null)
                {
                    if(String.IsNullOrEmpty(AccountrName))
                    {
                        AccountInfo = Context.Accounts.Include("AccountGroup").Include("AccountGroup.AccountGroupsForHelp").Where(x => x.CompanyId == Company.CompanyId 
                                  && x.AccountType == AccountType.ACCOUNT 
                                  //&& x.AccountGroup.AccountGroupsForHelp.Contains(ahp) 
                                  )
                                  .ToList<Account>();
                    }
                    else
                    {
                        AccountInfo = Context.Accounts.
                            //.SqlQuery("select * from accounts where AccountGroupId in (select AccountGroupId  from  accountgroupandaccountgrouphelp where AccountGroupHelpId=" + HelpId + ")").ToList();
                            Include("AccountGroup")
                                  .Where(x => x.Name.Contains(AccountrName)
                                  && x.CompanyId == Company.CompanyId
                                  && x.AccountType == AccountType.ACCOUNT
                                  //&& x.AccountGroup.AccountGroupsForHelp.Contains(ahp)
                                  )
                                  .ToList<Account>();
                    }

                    IList<Account> FilteredAccountInfo = new List<Account>();

                    foreach (Account Account in AccountInfo)
                    {
                        if (Account.AccountGroup.AccountGroupsForHelp != null)
                        {
                            foreach(AccountGroupForHelp helpGroup in Account.AccountGroup.AccountGroupsForHelp)
                            {
                                if(helpGroup.AccountGroupForHelpId == HelpId)
                                {
                                    FilteredAccountInfo.Add(Account);
                                }
                            }
                        }
                    }
                    AccountInfo = FilteredAccountInfo;
                }
                else
                {
                    if (String.IsNullOrEmpty(AccountrName))
                    {
                        AccountInfo = Context.Accounts.Include("AccountGroup").Where(x => x.CompanyId == Company.CompanyId
                                                        && x.AccountType == AccountType.ACCOUNT)
                                                        .ToList<Account>();
                    }
                    else
                    {
                        AccountInfo = Context.Accounts.Include("AccountGroup").Where(x => x.Name.Contains(AccountrName)
                                                        && x.CompanyId == Company.CompanyId
                                                        && x.AccountType == AccountType.ACCOUNT)
                                                        .ToList<Account>();
                    }
                }
                return AccountInfo;
            }
        }
        private IList<Account> GetFilteredAccounts(AccountMasterContext Context,
                                           string AccountName,
                                           Company Company,
                                           AccountGroupForHelp HelpGroup,
                                           long HelpId)
        {
            IList<Account> AccountInfo = String.IsNullOrEmpty(AccountName)
                ? Context.Accounts.Include("AccountGroup").Include("AccountGroup.AccountGroupsForHelp")
                    .Where(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT)
                    .ToList()
                : Context.Accounts.Include("AccountGroup").Include("AccountGroup.AccountGroupsForHelp")
                    .Where(x => x.Name.Contains(AccountName) && x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT)
                    .ToList();

            // Filter based on HelpGroup
            return AccountInfo.Where(account =>
                account.AccountGroup.AccountGroupsForHelp != null &&
                account.AccountGroup.AccountGroupsForHelp.Any(help => help.AccountGroupForHelpId == HelpId)
            ).ToList();
        }

        private IList<Account> GetGeneralAccounts(AccountMasterContext Context,
                                          string AccountName,
                                          Company Company)
        {
            return String.IsNullOrEmpty(AccountName)
                ? Context.Accounts.Include("AccountGroup")
                    .Where(x => x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT)
                    .ToList()
                : Context.Accounts.Include("AccountGroup")
                    .Where(x => x.Name.Contains(AccountName) && x.CompanyId == Company.CompanyId && x.AccountType == AccountType.ACCOUNT)
                    .ToList();
        }

        public Account GetAccountByName(String AccountrName, long CompanyId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.FirstOrDefault(x => x.Name == AccountrName && x.CompanyId == CompanyId && !Context.Customers.Any(customer => (customer.Id == x.Id)) && !Context.Suppliers.Any(Supplier => (Supplier.Id == x.Id)));
                return AccountInfo;
            }
        }
        
        public Account GetAccountDetailByName(String AccountrName, long CompanyId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.FirstOrDefault(x => x.Name == AccountrName && x.CompanyId == CompanyId);
                return AccountInfo;
            }
        }
        public List<DayBook> GetDaybookDetailByAccountId(long AccountId, long CompanyId)
        {
            List<DayBook> DayBookInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DayBookInfo = Context.DoubleEntries.Where(x => x.AccountId == AccountId && x.CompanyId == CompanyId).ToList<DayBook>();
                return DayBookInfo;
            }
        }
        public Account CheckAccountNameInUpdate(String AccountName, long AccountId, long CompanyId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.FirstOrDefault(x => x.Name == AccountName && x.CompanyId == CompanyId && x.AccountType==AccountType.ACCOUNT && !x.Id.Equals(AccountId));
                return AccountInfo;
            }
        }
        public Account CheckSubAccount(long AccountId)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.FirstOrDefault(x => x.ParentAccountId == AccountId);
            }
            return AccountInfo;
        }

        public bool DeleteAccount(long AccountId)
        {
            bool deleted = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        PaymentMethod PaymentMethod = context.PaymentMethods.FirstOrDefault(x => x.AccountId == AccountId);
                        if (PaymentMethod != null)
                        {
                            context.PaymentMethods.Where(p => p.AccountId == AccountId).ToList().ForEach(p => context.PaymentMethods.Remove(p));
                        }
                        context.Accounts.Where(p => p.Id == AccountId).ToList().ForEach(p => context.Accounts.Remove(p));
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }



            return deleted;
        }

        public Account AddAccount(Account account)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                       // Context.Entry(Account.AccountGroup).State = EntityState.Unchanged;
                        Context.Accounts.Add(account);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        account = null;
                        dbContextTransaction.Rollback();
                        throw e;
                    }
                }
            }
            return account;
        }

        public Account UpdateAccount(Account Account)
        {
            Account AccountInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AccountInfo = Context.Accounts.Find(Account.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (AccountInfo != null)
                        {
                            Context.Entry(AccountInfo).CurrentValues.SetValues(Account);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Account = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Account;
        }
    }
}
