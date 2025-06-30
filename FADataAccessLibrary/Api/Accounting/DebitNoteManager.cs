using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using FADataAccessLibrary.Api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public  class DebitNoteManager
    {
 private static volatile DebitNoteManager instance;
        private static object syncRoot = new Object();
        DebitNoteManager()
        {

        }
        public static DebitNoteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DebitNoteManager();
                    }
                }

                return instance;
            }
        }
        public DebitNote GetDebitNote(long DebitNoteId)
        {
            DebitNote DebitNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNote = Context.DebitNotes.Include("Company").Include("DebitNoteDetails").Include("DebitNoteDetails.Account").Include("Account").FirstOrDefault(x => x.DebitNoteId == DebitNoteId);
            }
            return DebitNote;
        }
        public DebitNote AddDebitNote(DebitNote debitNote)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.DebitNotes.Add(debitNote);
                        Context.SaveChanges();
                        DebitNoteDoubleEntryManager.Instance.RecordDebitNote(debitNote, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        debitNote = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return debitNote;
        }
        public IList<DebitNoteDetail> GetAllDebitNoteById(long DebitNoteId)
        {
            IList<DebitNoteDetail> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = (from DebitNoteDetail in Context.DebitNoteDetails where DebitNoteDetail.DebitNoteId == DebitNoteId select DebitNoteDetail).ToList();
                return DebitNoteInfo;
            }
        }
        public DebitNote UpdateDebitNote(DebitNote DebitNote)
        {
            DebitNote DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        DebitNoteInfo = Context.DebitNotes.Find(DebitNote.DebitNoteId);
                        if (DebitNoteInfo != null)
                        {
                            DebitNote DebitNoteInfoFromDB = GetDebitNote(DebitNote.DebitNoteId);
                            if (DebitNoteInfoFromDB.DebitNoteDetails.Count > 0)
                            {
                                Context.DebitNoteDetails.Where(p => p.DebitNoteId == DebitNoteInfoFromDB.DebitNoteId).ToList().ForEach(p => Context.DebitNoteDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            Context.Entry(DebitNoteInfo).CurrentValues.SetValues(DebitNote);
                            foreach (DebitNoteDetail Detail in DebitNote.DebitNoteDetails)
                            {
                                Detail.DebitNoteId = DebitNote.DebitNoteId;
                                Context.DebitNoteDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                            DebitNoteDoubleEntryManager.Instance.RecordDebitNote(DebitNoteInfo, Context);
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        DebitNoteInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return DebitNoteInfo;
        }

        public Boolean DeleteDebitNote(long DebitNoteId)
        {
            Boolean Deleted = false;
            DebitNote DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        DebitNoteInfo = Context.DebitNotes.Include("DebitNoteDetails").FirstOrDefault(x => x.DebitNoteId == DebitNoteId);
                        if (DebitNoteInfo.DebitNoteDetails.Count > 0)
                        {
                            DebitNoteDetail DebitNoteDetail = Context.DebitNoteDetails.FirstOrDefault(x => x.DebitNoteId == DebitNoteId);
                            if (DebitNoteDetail != null)
                            {
                                Context.DebitNoteDetails.Where(p => p.DebitNoteId == DebitNoteId).ToList().ForEach(p => Context.DebitNoteDetails.Remove(p));
                            }
                        }
                        DebitNoteInfo = Context.DebitNotes.Find(DebitNoteId);
                        Context.DebitNotes.Remove(DebitNoteInfo);
                        Context.SaveChanges();
                        DebitNoteDoubleEntryManager.Instance.DeleteDebitNote(DebitNoteInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168 // variable declared but not used.
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
        
        public IList<Account> ListDebitNoteService(long CompanyId, long?[] AccountGroupId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountGroupId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }
        
        public IList<DebitNote> GetDebitNotes(long AccountId)
        {
            IList<DebitNote> DebitNotes = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNotes = (from DebitNote in Context.DebitNotes where DebitNote.AccountId == AccountId select DebitNote).ToList();
            }
            return DebitNotes;
        }

        public IList<DebitNote> GetDebitNoteByDate(DateTime Date, long CompanyId)
        {
            IList<DebitNote> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.DebitNotes.Include("DebitNoteDetails").Include("Account").Where(x => x.TransactionDate.Day == Date.Day && x.TransactionDate.Month == Date.Month && x.TransactionDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<DebitNote>();
                return DebitNoteInfo;
            }
        }
        
        public IList<DebitNote> GetDebitNoteByAmount(double Amount, string RefNo, long CompanyId)
        {
            IList<DebitNote> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.DebitNotes.Include("DebitNoteDetails").Include("Account").Where(x => (x.Amount.Equals(Amount) || x.ReferenceNumber.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<DebitNote>();
                return DebitNoteInfo;
            }
        }
        public IList<DebitNote> GetDebitNoteBySupplierName(string SupplierName, long CompanyId)
        {
            IList<DebitNote> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.DebitNotes.Include("DebitNoteDetails").Include("Account").Where(x => (x.Account.Name.Contains(SupplierName) || x.ReferenceNumber.Contains(SupplierName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<DebitNote>();
                return DebitNoteInfo;
            }
        }
    }
}
