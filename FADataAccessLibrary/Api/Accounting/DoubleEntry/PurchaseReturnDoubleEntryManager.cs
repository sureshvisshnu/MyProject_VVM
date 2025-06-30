using fa.api.Accounting;
using fa.api.catalog;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Fa.api.Accounting.DoubleEntry
{
    public class PurchaseReturnDoubleEntryManager
    {
        static string SupplierEntryForPurchaseText = "For Purchase Return {0}";
        static string PurchasesAccountEntryForInvoiceText = "For Purchase Return {0}, from {1}";
        static string AdditionalTransactionText = "{0} applied against Purchase Return {1}, from {2}";
        static string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile PurchaseReturnDoubleEntryManager instance;
        private static object syncRoot = new Object();
        PurchaseReturnDoubleEntryManager()
        {
        }
        public static PurchaseReturnDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PurchaseReturnDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordPurchaseReturn(PurchaseEntry Purchase, AccountMasterContext Context)
        {
            DeletePurchaseReturn(Purchase, Context);
            AddPurchaseReturn(Purchase, Context);
        }
        /*
         * Deleteing all exisiting entries for the given company 
         */
        public void DeletePurchaseReturn(PurchaseEntry Purchase, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.PurchaseReturn && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Purchase.RefNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void AddPurchaseReturn(PurchaseEntry Purchase, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            //Delete the current entries
            Context.DoubleEntries.Add(getCreditSupplierEntry(Purchase));
            Context.DoubleEntries.AddRange(getPurchaseEntry(Purchase));
            Context.DoubleEntries.AddRange(getPurchaseAdditionalTransactionEntries(Purchase));
            if (Purchase.RoundOff != 0)
            {
                Context.DoubleEntries.Add(getRoundOffEntry(Purchase));
            }
            Context.SaveChanges();
        }
        public DayBook getRoundOffEntry(PurchaseEntry Purchase)
        {
            //for sale roundoff
            Company Company = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            Account account = AccountManager.Instance.GetAccountByName("Indirect Expense", Company.CompanyId);
            if (account == null)
            {
                account = new Account();
                account.AccountGroupId = 1503;
                account.Discription = "To track gains or losses";
                account.Name = "Indirect Expense";
                account.CompanyId = Purchase.CompanyId;
                AccountManager.Instance.AddAccount(account);
            }
            DayBook dBook = new DayBook()
            {
                AccountId = account.Id,
                CompanyId = Purchase.CompanyId,
                CostCenterId = Purchase.CostCenterId,
                Date = Purchase.ReturnDate,
                Description = string.Format(SupplierEntryForPurchaseText, Purchase.RefNumber + " round off amount"),
                TransactionType = DaybookTransactionType.PurchaseReturn,
                ReferenceTrasnactionId = Purchase.RefNumber,
                Amount = Purchase.RoundOff
            };
            return dBook;
        }
        public DayBook getCreditSupplierEntry(PurchaseEntry Purchase)
        {
            //for cash purchase
            Company Company = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            if (Company.CashOnHandAccount == null && Purchase.PurchaseMethod == PurchaseMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            DayBook dBook = new DayBook();
            dBook.AccountId = Purchase.PurchaseMethod == PurchaseMethod.Credit ? Purchase.AccountId : Company.CashOnHandAccountId;
            dBook.CompanyId = Purchase.CompanyId;
            dBook.CostCenterId = Purchase.CostCenterId;
            dBook.Date = Purchase.ReturnDate;
            dBook.Debit(Purchase.TotalAmount);
            dBook.Description = string.Format(SupplierEntryForPurchaseText, Purchase.RefNumber);
            dBook.TransactionType = DaybookTransactionType.PurchaseReturn;
            dBook.ReferenceTrasnactionId = Purchase.RefNumber;
            return dBook;
        }
        public List<DayBook> getPurchaseEntry(PurchaseEntry Purchase)
        {
            List<DayBook> dBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            //for cash purchase 
            if (Company.CashOnHandAccount == null && Purchase.PurchaseMethod == PurchaseMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account PurchaseSupplier = null;
            if (Purchase.PurchaseMethod == PurchaseMethod.Credit)
            {
                PurchaseSupplier = AccountManager.Instance.GetAccountById((long)Purchase.AccountId);
                if (PurchaseSupplier == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            double Amount = 0.00;
            foreach (PurchaseDetails Detail in Purchase.PurchaseDetails)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Detail.ProductId);
                //get the company Purchases a/c
                if (Company.PurchaseAccount == null && Product.PurchaseAccount == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
                if (Product.PurchaseAccount == null || Company.PurchaseAccount.Id == Product.PurchaseAccount.Id)
                {
                    Amount += Detail.Amount;
                }
                else
                {
                    DayBook dBook = new DayBook();
                    dBook.AccountId = Product.PurchaseAccount == null ? Company.PurchaseAccount.Id : Product.PurchaseAccount.Id;
                    dBook.CompanyId = Purchase.CompanyId;
                    dBook.CostCenterId = Purchase.CostCenterId;
                    dBook.Date = Purchase.ReturnDate;
                    dBook.Credit(Detail.Amount);
                    dBook.Description = string.Format(PurchasesAccountEntryForInvoiceText, Purchase.RefNumber, Purchase.PurchaseMethod == PurchaseMethod.Credit ? PurchaseSupplier.DisplayAs : Company.CashOnHandAccount.DisplayAs);
                    dBook.TransactionType = DaybookTransactionType.PurchaseReturn;
                    dBook.ReferenceTrasnactionId = Purchase.RefNumber;
                    dBooks.Add(dBook);
                }
            }
            if (Amount > 0)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = Company.PurchaseAccount.Id;
                dBook.CompanyId = Purchase.CompanyId;
                dBook.CostCenterId = Purchase.CostCenterId;
                dBook.Date = Purchase.ReturnDate;
                dBook.Credit(Amount);
                dBook.Description = string.Format(PurchasesAccountEntryForInvoiceText, Purchase.RefNumber, Purchase.PurchaseMethod == PurchaseMethod.Credit ? PurchaseSupplier.DisplayAs : Company.CashOnHandAccount.DisplayAs);
                dBook.TransactionType = DaybookTransactionType.PurchaseReturn;
                dBook.ReferenceTrasnactionId = Purchase.RefNumber;
                dBooks.Add(dBook);
            }
            return dBooks;
        }
        public List<DayBook> getPurchaseAdditionalTransactionEntries(PurchaseEntry Purchase)
        {
            List<DayBook> dBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Purchase.CompanyId);
            //for cash purchase 
            if (Company.CashOnHandAccount == null && Purchase.PurchaseMethod == PurchaseMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            //get the company Purchases a/c
            if (Company.PurchaseAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account PurchaseSupplier = null;
            if (Purchase.PurchaseMethod == PurchaseMethod.Credit)
            {
                PurchaseSupplier = AccountManager.Instance.GetAccountById((long)Purchase.AccountId);
                if (PurchaseSupplier == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            foreach (PurchaseAdditionalTransaction PurchaseAdditionalTransaction in Purchase.PurchaseAdditionalTransactions)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = PurchaseAdditionalTransaction.AccountId;
                dBook.CompanyId = Purchase.CompanyId;
                dBook.CostCenterId = Purchase.CostCenterId;
                dBook.Date = Purchase.ReturnDate;
                dBook.Credit(PurchaseAdditionalTransaction.Amount * (PurchaseAdditionalTransaction.Action == AdditionalTransactionAction.CR ? 1 : -1));
                dBook.Description = string.Format(AdditionalTransactionText, PurchaseAdditionalTransaction.Name, Purchase.RefNumber, Purchase.PurchaseMethod == PurchaseMethod.Credit ? PurchaseSupplier.DisplayAs : Company.CashOnHandAccount.DisplayAs);
                dBook.TransactionType = DaybookTransactionType.PurchaseReturn;
                dBook.ReferenceTrasnactionId = Purchase.RefNumber;
                dBooks.Add(dBook);
            }
            return dBooks;
        }
    }
}
