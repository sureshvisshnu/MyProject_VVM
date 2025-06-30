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
    public class ReceiptDoubleEntryManager
    {
        static readonly string AccountEntryForReceipt = "Receipt {0} {1}" ;      
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile ReceiptDoubleEntryManager instance;
        private static object syncRoot = new Object();
        ReceiptDoubleEntryManager()
        {
        }
        public static ReceiptDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReceiptDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordReceipt(Receipt Receipt, AccountMasterContext Context)
        {
            DeleteReceipts(Receipt, Context);
            RecordReceipts(Receipt, Context);
        }
        //Deleteing all exisiting entries for the given company  
        public void DeleteReceipts(Receipt Receipt, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Receipt.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Receipt && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Receipt.Reference).ToList();
            if (xx.Count > 0)
            {
                Context.DoubleEntries.RemoveRange(xx);
                Context.SaveChanges();
            }
        }
        public void RecordReceipts(Receipt Receipt, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Receipt.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(GetFromAccountEntry(Receipt));
            Context.DoubleEntries.AddRange(GetToAccountEntry(Receipt));
            Context.SaveChanges();
        }
        public DayBook GetFromAccountEntry(Receipt Receipt)
        {
            DayBook dBook = new DayBook
            {
                AccountId = Receipt.AccountId,
                CompanyId = Receipt.CompanyId,
                CostCenterId = Receipt.CostCenterId,
                Date = Receipt.TransactionDate,
                Description = string.Format(AccountEntryForReceipt, Receipt.Reference, Receipt.Description),
                ReferenceTrasnactionId = Receipt.Reference,
                TransactionType = DaybookTransactionType.Receipt
            };
            dBook.Credit((double)Receipt.Amount);
            return dBook;
        }
        static readonly string ReceiptEntryCaption = "Trasnsfer Id:{0}";
        static readonly string ReceiptCheckEntryCaption = "Cheque/DD:{0} Dated:{1} Trasnsfer Id:{2}";
        static readonly string ReceiptBankEntryCaption = "Bank:{0} Dated:{1} Trasnsfer Id:{2}";
        static readonly string ReceiptCardEntryCaption = "CreditCard:{0} Dated:{1} Trasnsfer Id:{2}";
        public List<DayBook> GetToAccountEntry(Receipt Receipt)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Receipt.CompanyId);
            if (Company.CashOnHandAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account Account = AccountManager.Instance.GetAccountById((long) Receipt.AccountId);
            if (Account == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account lAccount = null;
            string Desc = string.Empty;
            if (Receipt.TransactionType == PaymentType.CASH)
            {
                lAccount = Company.CashOnHandAccount;
                Desc = string.Format(ReceiptEntryCaption,lAccount.DisplayAs);
            }
            else if (Receipt.TransactionType == PaymentType.CHECK)
            {
                CheckReceipt CheckReceipt = ((CheckReceipt)Receipt);
                lAccount = AccountManager.Instance.GetAccountById((long)CheckReceipt.DepositedIntoId);
                Desc = string.Format(ReceiptCheckEntryCaption,  CheckReceipt.DocumentNumber, CheckReceipt.DocumentDate, lAccount.DisplayAs);
            }
            else if (Receipt.TransactionType == PaymentType.BANKTRANSFER)
            {
                BankTransferReceipt BankTransferReceipt = ((BankTransferReceipt)Receipt);
                lAccount = AccountManager.Instance.GetAccountById((long)BankTransferReceipt.BankTransferId);
                Desc = string.Format(ReceiptBankEntryCaption,  BankTransferReceipt.TransactionNumber, BankTransferReceipt.TransactionDate, lAccount.DisplayAs);
            }
            else if (Receipt.TransactionType == PaymentType.CREDITCARD)
            {
                CreditCardReceipt CreditCardReceipt = ((CreditCardReceipt)Receipt);
                lAccount = AccountManager.Instance.GetAccountById((long)CreditCardReceipt.CCAccountId);
                Desc = string.Format(ReceiptCardEntryCaption,  CreditCardReceipt.CCTransactionNumber, CreditCardReceipt.CCTransactionDate, lAccount.DisplayAs);
            }
            foreach (ReceiptDetail Detail in Receipt.ReceiptDetails)
            {
                DayBook dBook = new DayBook
                {
                    AccountId = lAccount.Id,
                    CompanyId = Receipt.CompanyId,
                    CostCenterId = Receipt.CostCenterId,
                    Date = Receipt.TransactionDate,
                    Description = string.Format(AccountEntryForReceipt, Receipt.Reference + (string.IsNullOrEmpty(Detail.Description) ? "" : " " + Detail.Description), Desc),
                    ReferenceTrasnactionId = Receipt.Reference,
                    TransactionType = DaybookTransactionType.Receipt
                };
                dBook.Debit((double)Detail.Amount);
                dayBooks.Add(dBook);
            }
            return dayBooks;
        }
    }
}
