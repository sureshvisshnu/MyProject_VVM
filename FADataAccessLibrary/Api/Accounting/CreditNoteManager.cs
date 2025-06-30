using fa.context;
using fa.model.Accounting.Transactions;
using Fa.api.Accounting.DoubleEntry;
using FADataAccessLibrary.Api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting.Transactions
{
    public class CreditNoteManager
    {
        private static volatile CreditNoteManager instance;
        private static object syncRoot = new Object();
        CreditNoteManager()
        {

        }
        public static CreditNoteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CreditNoteManager();
                    }
                }

                return instance;
            }
        }
        public CreditNote GetCreditNote(long CreditNoteId)
        {
            CreditNote CreditNote = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditNote = Context.CreditNotes.Include("Company").Include("CreditNoteDetails.Account").Include("CreditNoteDetails").Include("Account").FirstOrDefault(x => x.CreditNoteId == CreditNoteId);
            }
            return CreditNote;
        }
        public CreditNote AddCreditNote(CreditNote creditNote)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.CreditNotes.Add(creditNote);
                        Context.SaveChanges();
                        CreditNoteDoubleEntryManager.Instance.RecordCreditNote(creditNote, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        creditNote = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return creditNote;
        }
        
        public CreditNote UpdateCreditNote(CreditNote CreditNote)
        {
            CreditNote CreditNoteInfo = null;           
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        CreditNoteInfo = Context.CreditNotes.Find(CreditNote.CreditNoteId);
                        if (CreditNoteInfo != null)
                        {
                            CreditNote CreditNoteInfoFromDB = GetCreditNote(CreditNote.CreditNoteId);
                            if (CreditNoteInfoFromDB.CreditNoteDetails.Count > 0)
                            {
                                Context.CreditNoteDetails.Where(p => p.CreditNoteId == CreditNoteInfoFromDB.CreditNoteId).ToList().ForEach(p => Context.CreditNoteDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            Context.Entry(CreditNoteInfo).CurrentValues.SetValues(CreditNote);
                            foreach (CreditNoteDetail Detail in CreditNote.CreditNoteDetails)
                            {
                                Detail.CreditNoteId = CreditNote.CreditNoteId;
                                Context.CreditNoteDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                            CreditNoteDoubleEntryManager.Instance.RecordCreditNote(CreditNote, Context);
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CreditNoteInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CreditNoteInfo;
        }

        public Boolean DeleteCreditNote(long CreditNoteId)
        {
            Boolean Deleted = false;
            CreditNote CreditNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        CreditNoteInfo = Context.CreditNotes.Include("CreditNoteDetails").FirstOrDefault(x => x.CreditNoteId == CreditNoteId);
                        if (CreditNoteInfo.CreditNoteDetails.Count > 0)
                        {
                            CreditNoteDetail CreditNoteDetail = Context.CreditNoteDetails.FirstOrDefault(x => x.CreditNoteId == CreditNoteId);
                            if (CreditNoteDetail != null)
                            {
                                Context.CreditNoteDetails.Where(p => p.CreditNoteId == CreditNoteId).ToList().ForEach(p => Context.CreditNoteDetails.Remove(p));
                            }
                        }
                        CreditNoteInfo = Context.CreditNotes.Find(CreditNoteId);
                        Context.CreditNotes.Remove(CreditNoteInfo);
                        Context.SaveChanges();
                        CreditNoteDoubleEntryManager.Instance.DeleteCreditNote(CreditNoteInfo, Context);
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
        public IList<CreditNote> GetCreditNoteByDate(DateTime Date, long CompanyId)
        {
            IList<CreditNote> CreditNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditNoteInfo = Context.CreditNotes.Include("CreditNoteDetails").Include("Account").Where(x => x.TransactionDate.Day == Date.Day && x.TransactionDate.Month == Date.Month && x.TransactionDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<CreditNote>();
                return CreditNoteInfo;
            }
        }

        public IList<CreditNote> GetCreditNoteByAmount(double Amount, string RefNo, long CompanyId)
        {
            IList<CreditNote> CreditNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditNoteInfo = Context.CreditNotes.Include("CreditNoteDetails").Include("Account").Where(x => (x.Amount.Equals(Amount) || x.ReferenceNumber.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<CreditNote>();
                return CreditNoteInfo;
            }
        }
        public IList<CreditNote> GetCreditNoteByCustomerName(string CustomerName, long CompanyId)
        {
            IList<CreditNote> CreditNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditNoteInfo = Context.CreditNotes.Include("CreditNoteDetails").Include("Account").Where(x => (x.Account.Name.Contains(CustomerName) || x.ReferenceNumber.Contains(CustomerName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<CreditNote>();
                return CreditNoteInfo;
            }
        }
    }
    }

