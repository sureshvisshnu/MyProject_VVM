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
    public class CreditNoteDoubleEntryManager
    {
        static readonly string SupplierEntryForCreditNote = "CreditNote {0} {1}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        static readonly string CreditNoteEntryCaption = "By CreditNote {0}, {1} by {2}";
        private static volatile CreditNoteDoubleEntryManager instance;
        private static object syncRoot = new Object();
        CreditNoteDoubleEntryManager()
        {
        }
        public static CreditNoteDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CreditNoteDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordCreditNote(CreditNote CreditNote, AccountMasterContext Context)
        {
            DeleteCreditNote(CreditNote, Context);
            RecordCreditNotes(CreditNote, Context);
        }
        public void DeleteCreditNote(CreditNote CreditNote, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(CreditNote.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.CreditNote && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + CreditNote.ReferenceNumber).ToList();
            if (xx.Count > 0)
            {
                Context.DoubleEntries.RemoveRange(xx);
                Context.SaveChanges();
            }
        }
        public void RecordCreditNotes(CreditNote CreditNote, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(CreditNote.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(GetFromAccountEntry(CreditNote));
            Context.DoubleEntries.AddRange(GetToAccountEntry(CreditNote));
            Context.SaveChanges();
        }
        public DayBook GetFromAccountEntry(CreditNote CreditNote)
        {
            DayBook dBook = new DayBook
            {
                AccountId = CreditNote.AccountId,
                CompanyId = CreditNote.CompanyId,
                CostCenterId = CreditNote.CostCenterId,
                Date = CreditNote.TransactionDate,
                Description = string.Format(SupplierEntryForCreditNote, CreditNote.ReferenceNumber, CreditNote.Note),
                ReferenceTrasnactionId = CreditNote.ReferenceNumber,
                TransactionType = DaybookTransactionType.CreditNote
            };
            dBook.Credit(CreditNote.Amount);
            return dBook;
        }

        public List<DayBook> GetToAccountEntry(CreditNote CreditNote)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Account Account = AccountManager.Instance.GetAccountById((long)CreditNote.AccountId);
            if (Account == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            foreach (CreditNoteDetail Detail in CreditNote.CreditNoteDetails)
            {
                DayBook dBook = new DayBook();
                Account llAccount = AccountManager.Instance.GetAccountById((long)Detail.AccountId);
                if (llAccount != null)
                {
                    dBook.AccountId = llAccount.Id;
                    dBook.CompanyId = CreditNote.CompanyId;
                    dBook.CostCenterId = CreditNote.CostCenterId;
                    dBook.Date = CreditNote.TransactionDate;
                    dBook.Debit(Detail.Amount);
                    dBook.Description = string.Format(CreditNoteEntryCaption,Detail.CreditNote.ReferenceNumber+ (string.IsNullOrEmpty(Detail.Description) ? "" : " "+Detail.Description), Account.AccountGroup.Name, Account.DisplayAs);
                    dBook.ReferenceTrasnactionId = CreditNote.ReferenceNumber;
                    dBook.TransactionType = DaybookTransactionType.CreditNote;
                    dayBooks.Add(dBook);
                }
            }
            return dayBooks;
        }
    }
}

