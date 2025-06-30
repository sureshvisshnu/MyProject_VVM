using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fa.api.Accounting.DoubleEntry
{
    class SaleReturnDoubleEntryManager
    {
        static string CustomerEntryForSaleText = "For Sale Return {0}";
        static string SalesAccountEntryForInvoiceText = "For Sale Return {0}, by {1}";
        static string TaxAccountEntryForInvoiceText = "Tax collected against Sale Return {0}, by {1}";
        static string AdditionalTransactionText = "{0} applied against Sale Return {1}, by {2}";
        static string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile SaleReturnDoubleEntryManager instance;
        private static object syncRoot = new Object();
        SaleReturnDoubleEntryManager()
        {
        }
        public static SaleReturnDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SaleReturnDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordSaleReturn(SaleEntry Sale, AccountMasterContext Context)
        {
            DeleteSaleReturns(Sale, Context);
            RecordSaleReturns(Sale, Context);
        }
        /*
         * Deleteing all exisiting entries for the given company 
         */
        public void DeleteSaleReturns(SaleEntry Sale, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.SalesReturn && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Sale.RefNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordSaleReturns(SaleEntry Sale, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getCashOnHandEntry(Sale));
            Context.DoubleEntries.Add(getSaleReturnEntry(Sale));
            Context.DoubleEntries.AddRange(getSaleReturnTaxEntries(Sale));
            Context.DoubleEntries.AddRange(getSaleReturnAdditionalTransactionEntries(Sale));
            if (Sale.RoundOff != 0)
            {
                Context.DoubleEntries.Add(getRoundOffEntry(Sale));
            }
            Context.SaveChanges();
        }
        public DayBook getRoundOffEntry(SaleEntry Sale)
        {
            //for sale roundoff
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            Account account = AccountManager.Instance.GetAccountByName("Indirect Expense", Company.CompanyId);
            if (account == null)
            {
                account = new Account();
                account.AccountGroupId = 1503;
                account.Discription = "To track gains or losses";
                account.Name = "Indirect Expense";
                account.CompanyId = Sale.CompanyId;
                AccountManager.Instance.AddAccount(account);    
            }
            DayBook dBook = new DayBook()
            {
                AccountId = account.Id,
                CompanyId = Sale.CompanyId,
                CostCenterId = Sale.CostCenterId,
                Date = Sale.ReturnDate,
                Description = string.Format(CustomerEntryForSaleText, Sale.RefNumber + " round off amount"),
                TransactionType = DaybookTransactionType.SalesReturn,
                ReferenceTrasnactionId = Sale.RefNumber,
                Amount = Sale.RoundOff
            };
            return dBook;
        }
        public DayBook getCashOnHandEntry(SaleEntry Sale)
        {
            //for cash sale
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            DayBook dBook = new DayBook();
            dBook.AccountId = Sale.SaleMethod == SaleMethod.Credit ? Sale.AccountsId : Company.CashOnHandAccountId;
            dBook.CompanyId = Sale.CompanyId;
            dBook.CostCenterId = Sale.CostCenterId;
            dBook.Date = Sale.ReturnDate;
            dBook.Credit(Sale.TotalAmount);
            dBook.Description = string.Format(CustomerEntryForSaleText, Sale.RefNumber);
            dBook.TransactionType = DaybookTransactionType.SalesReturn;
            dBook.ReferenceTrasnactionId = Sale.RefNumber;
            return dBook;
        }
        public DayBook getSaleReturnEntry(SaleEntry Sale)
        {
            DayBook dBook = new DayBook();
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            //for cash sale 
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            //get the company sales a/c
            if (Company.SalesAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account SaleCustomer = null;
            if (Sale.SaleMethod == SaleMethod.Credit)
            {
                SaleCustomer = AccountManager.Instance.GetAccountById((long)Sale.AccountsId);
                if (SaleCustomer == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            dBook.AccountId = Company.SalesAccount.Id;
            dBook.CompanyId = Sale.CompanyId;
            dBook.CostCenterId = Sale.CostCenterId;
            dBook.Date = Sale.ReturnDate;
            dBook.Debit((Sale.TotalAmount - Sale.TaxAmount - getSaleReturnAdditionalAmount(Sale)));
            dBook.Description = string.Format(SalesAccountEntryForInvoiceText, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : Company.CashOnHandAccount.DisplayAs);
            dBook.TransactionType = DaybookTransactionType.SalesReturn;
            dBook.ReferenceTrasnactionId = Sale.RefNumber;
            return dBook;
        }
        public List<DayBook> getSaleReturnTaxEntries(SaleEntry Sale)
        {
            List<DayBook> dBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            //for cash sale 
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            //get the company sales a/c
            if (Company.SalesAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account SaleCustomer = null;
            if (Sale.SaleMethod == SaleMethod.Credit)
            {
                SaleCustomer = AccountManager.Instance.GetAccountById((long)Sale.AccountsId);
                if (SaleCustomer == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            foreach (OrderLevelSaleTaxDetail saleTaxDetail in Sale.TaxDetails)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = saleTaxDetail.TaxAccountId;
                dBook.CompanyId = Sale.CompanyId;
                dBook.CostCenterId = Sale.CostCenterId;
                dBook.Date = Sale.ReturnDate;
                dBook.Debit(saleTaxDetail.Amount);
                dBook.Description = string.Format(TaxAccountEntryForInvoiceText, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : Company.CashOnHandAccount.DisplayAs);
                dBook.TransactionType = DaybookTransactionType.SalesReturn;
                dBook.ReferenceTrasnactionId = Sale.RefNumber;
                dBooks.Add(dBook);
            }
            return dBooks;
        }
        public List<DayBook> getSaleReturnAdditionalTransactionEntries(SaleEntry Sale)
        {
            List<DayBook> dBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            //for cash sale
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            //get the company sales a/c
            if (Company.SalesAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account SaleCustomer = null;
            if (Sale.SaleMethod == SaleMethod.Credit)
            {
                SaleCustomer = AccountManager.Instance.GetAccountById((long)Sale.AccountsId);
                if (SaleCustomer == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            foreach (SaleAdditionalTransaction SaleAdditionalTransaction in Sale.SaleAdditionalTransactions)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = SaleAdditionalTransaction.AccountId;
                dBook.CompanyId = Sale.CompanyId;
                dBook.CostCenterId = Sale.CostCenterId;
                dBook.Date = Sale.ReturnDate;
                dBook.Debit((SaleAdditionalTransaction.Amount * (SaleAdditionalTransaction.Action == AdditionalTransactionAction.CR ? 1 : -1)));
                dBook.Description = string.Format(AdditionalTransactionText, SaleAdditionalTransaction.Name, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : Company.CashOnHandAccount.DisplayAs);
                dBook.TransactionType = DaybookTransactionType.SalesReturn;
                dBook.ReferenceTrasnactionId = Sale.RefNumber;
                dBooks.Add(dBook);
            }
            return dBooks;
        }
        public double getSaleReturnAdditionalAmount(SaleEntry Sale)
        {
            double amount = 0F;
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            //for cash sale
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            //get the company sales a/c
            if (Company.SalesAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account SaleCustomer = null;
            if (Sale.SaleMethod == SaleMethod.Credit)
            {
                SaleCustomer = AccountManager.Instance.GetAccountById((long)Sale.AccountsId);
                if (SaleCustomer == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
            }
            foreach (SaleAdditionalTransaction SaleAdditionalTransaction in Sale.SaleAdditionalTransactions)
            {
                amount += (SaleAdditionalTransaction.Action == AdditionalTransactionAction.CR ? 1 : -1) * SaleAdditionalTransaction.Amount;
            }
            return amount;
        }
    }
}
