using Fa.api.Accounting.DoubleEntry;
using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Accounting.DoubleEntry
{
    public class DebitNoteDoubleEntryManager
    {
        static readonly string AccountEntryForDebitNote = "DebitNote {0} {1}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        static readonly string DebitNoteEntryCaption = "By DebitNote {0}, {1} by {2}";
        private static volatile DebitNoteDoubleEntryManager instance;
        private static object syncRoot = new Object();
        DebitNoteDoubleEntryManager()
        {
        }
        public static DebitNoteDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DebitNoteDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordDebitNote(DebitNote DebitNote, AccountMasterContext Context)
        {
            DeleteDebitNote(DebitNote, Context);
            RecordDebitNotes(DebitNote, Context);
        }
        public void DeleteDebitNote(DebitNote DebitNote, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(DebitNote.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.DebitNote && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + DebitNote.ReferenceNumber).ToList();
            if (xx.Count > 0)
            {
                Context.DoubleEntries.RemoveRange(xx);
                Context.SaveChanges();
            }
        }
        public void RecordDebitNotes(DebitNote DebitNote, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(DebitNote.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getFromSupplierEntryForDebitNote(DebitNote));
            Context.DoubleEntries.AddRange(getToAccountEntryForDebitNote(DebitNote));
            Context.SaveChanges();
        }
        public DayBook getFromSupplierEntryForDebitNote(DebitNote DebitNote)
        {
            DayBook dBook = new DayBook();
            dBook.AccountId = DebitNote.AccountId;
            dBook.CompanyId = DebitNote.CompanyId;
            dBook.CostCenterId = DebitNote.CostCenterId;
            dBook.Date = DebitNote.TransactionDate;
            dBook.Debit(DebitNote.Amount);
            dBook.Description = string.Format(AccountEntryForDebitNote, DebitNote.ReferenceNumber, DebitNote.Note);
            dBook.ReferenceTrasnactionId = DebitNote.ReferenceNumber;
            dBook.TransactionType = DaybookTransactionType.DebitNote;
            return dBook;
        }

        public List<DayBook> getToAccountEntryForDebitNote(DebitNote DebitNote)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Account DebitNoteAccount = AccountManager.Instance.GetAccountById((long)DebitNote.AccountId);
            if (DebitNoteAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            foreach (DebitNoteDetail Detail in DebitNote.DebitNoteDetails)
            {
                DayBook dBook = new DayBook();
                Account llAccount = AccountManager.Instance.GetAccountById((long)Detail.AccountId);
                if (llAccount != null)
                {
                    dBook.AccountId = llAccount.Id;
                    dBook.CompanyId = DebitNote.CompanyId;
                    dBook.CostCenterId = DebitNote.CostCenterId;
                    dBook.Date = DebitNote.TransactionDate;
                    dBook.Credit(Detail.Amount);
                    dBook.Description = string.Format(DebitNoteEntryCaption,Detail.DebitNote.ReferenceNumber + (string.IsNullOrEmpty(Detail.Description) ? "" : " " + Detail.Description), DebitNoteAccount.AccountGroup.Name, DebitNoteAccount.DisplayAs);
                    dBook.ReferenceTrasnactionId = DebitNote.ReferenceNumber;
                    dBook.TransactionType = DaybookTransactionType.DebitNote;
                    dayBooks.Add(dBook);
                }
            }
            return dayBooks;
        }
    }
}

