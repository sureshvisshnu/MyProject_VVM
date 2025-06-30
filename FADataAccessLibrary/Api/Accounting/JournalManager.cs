using fa.context;
using fa.model.Accounting.Transaction;
using Fa.api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class JournalManager
    {

        private static volatile JournalManager instance;
        private static object syncRoot = new Object();
        JournalManager()
        {

        }
        public static JournalManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new JournalManager();
                    }
                }

                return instance;

            }
        }

        public Journal AddJournal(Journal journal)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Journals.Add(journal);
                        Context.SaveChanges();
                        //daybook
                        JournalDoubleEntryManager.Instance.RecordJournal(journal, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        journal = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return journal;
        }

        public Journal UpdateJournal(Journal Journal)
        {
            Journal JournalInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        JournalInfo = Context.Journals.Find(Journal.JournalId);
                        if (JournalInfo != null)
                        {

                            Journal JournalInfoFromDB = GetJournal(Journal.JournalId);
                            if (JournalInfoFromDB.JournalDetails.Count > 0)
                            {
                                Context.JournalDetails.Where(p => p.JournalId == JournalInfoFromDB.JournalId).ToList().ForEach(p => Context.JournalDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            Journal.Company = null;
                            Context.Entry(JournalInfo).CurrentValues.SetValues(Journal);
                            foreach (JournalDetail Detail in Journal.JournalDetails)
                            {
                                Detail.Journal = null;
                                Detail.JournalId = Journal.JournalId;
                                Context.JournalDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                        }
                        //daybook
                        JournalDoubleEntryManager.Instance.RecordJournal(Journal, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        JournalInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return JournalInfo;
        }


        public Boolean DeleteJournal(long JournalId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Journal JournalInfo = GetJournal(JournalId);
                        if (JournalInfo.JournalDetails.Count > 0)
                        {
                            Context.JournalDetails.Where(p => p.JournalId == JournalInfo.JournalId).ToList().ForEach(p => Context.JournalDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        Context.Journals.Remove(Context.Journals.Find(JournalId));
                        Context.SaveChanges();
                        //daybook delete
                        JournalDoubleEntryManager.Instance.DeleteJournals(JournalInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Deleted;
        }
        public Journal GetJournal(long JournalId)
        {
            Journal Journal = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Journal = Context.Journals.Include("Company").Include("JournalDetails").FirstOrDefault(x => x.JournalId == JournalId);
            }
            return Journal;
        }
        
        public IList<Journal> GetJournalByDate(DateTime? Date, long CompanyId)
        {
            IList<Journal> JournalInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                JournalInfo = Context.Journals.Include("JournalDetails").Where(x => x.TransactionDate == Date && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Journal>();
                return JournalInfo;
            }
        }

        public IList<Journal> GetJournalByReferenceNo(string RefNo, long CompanyId)
        {
            IList<Journal> JournalInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                JournalInfo = Context.Journals.Include("JournalDetails").Where(x => x.ReferenceNumber.Contains(RefNo) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Journal>();
                return JournalInfo;
            }
        }

        public IList<Journal> GetJournals(long CompanyId)
        {
            IList<Journal> JournalInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                JournalInfo = Context.Journals.Include("JournalDetails").Where(x =>x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Journal>();
                return JournalInfo;
            }
        }

        public JournalDetail GetJournalDetailAccount(long JournalDetailId)
        {
            JournalDetail JournalDetailed = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                JournalDetailed = Context.JournalDetails.FirstOrDefault(x => x.JournalDetailId == JournalDetailId);
            }
            return JournalDetailed;
        }
    }
}
