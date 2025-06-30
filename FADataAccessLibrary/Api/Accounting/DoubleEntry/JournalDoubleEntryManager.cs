using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Fa.api.Accounting.DoubleEntry
{
    public class JournalDoubleEntryManager
    {
        static readonly string CurrentCompanyNullMsg = "Could not find the company, Please contact your administrator";
        private static volatile JournalDoubleEntryManager instance;
        private static object syncRoot = new Object();
        JournalDoubleEntryManager()
        {
        }
        public static JournalDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new JournalDoubleEntryManager();
                    }
                }
                return instance;
            }
        }        
        public void RecordJournal(Journal Journal, AccountMasterContext Context)
        {
            DeleteJournals(Journal, Context);
            RecordJournals(Journal, Context);
        }
        public void DeleteJournals(Journal Journal, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Journal.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Journal && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Journal.ReferenceNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordJournals(Journal Journal, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Journal.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.AddRange(getEntriesForJournal(Journal));
            Context.SaveChanges();
        }
        private List<DayBook> getEntriesForJournal(Journal Journal)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            foreach (JournalDetail JournalDetail in Journal.JournalDetails)
            {
                DayBook dBook = new DayBook();
                Account Account = AccountManager.Instance.GetAccountById((long) JournalDetail.ToAccountId);
                if (Account != null)
                {
                    dBook.AccountId = Account.Id;
                    dBook.CompanyId = Journal.CompanyId;
                    dBook.CostCenterId = Journal.CostCenterId;
                    dBook.Date = Journal.TransactionDate;
                    string FromStr = String.Empty;
                    if (JournalDetail.ReferenceAccountId != null)
                    {
                        Account RefAccount = AccountManager.Instance.GetAccountById((long)JournalDetail.ReferenceAccountId);
                        FromStr = (RefAccount != null ? " from " + RefAccount.DisplayAs : "");
                    }
                    dBook.Description = FromStr + (String.IsNullOrEmpty(FromStr) ? "" : ", ") + "for " + JournalDetail.Description;
                    dBook.Amount = (double)JournalDetail.Amount;
                    dBook.ReferenceTrasnactionId = Journal.ReferenceNumber;
                    dBook.TransactionType = DaybookTransactionType.Journal;
                    dayBooks.Add(dBook);
                }
            }
            return dayBooks;
        }
    }
}
