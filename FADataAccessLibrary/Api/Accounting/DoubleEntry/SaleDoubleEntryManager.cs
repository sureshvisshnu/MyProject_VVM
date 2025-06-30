using System;
using System.Collections.Generic;
using System.Linq;
using fa.context;
using fa.model.OrderManagement;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.api.catalog;
using fa.model.Catalog;
using Fa.model.Purchase;

namespace fa.api.accounting.doubleentry
{
    public class SaleDoubleEntryManager
    {
        static readonly string CustomerEntryForSaleText = "For Sale/invoice {0}";
        static readonly string SalesAccountEntryForInvoiceText = "For Sale/invoice {0}, by {1}";
        static readonly string TaxAccountEntryForInvoiceText = "Tax collected against sale/invoice {0}, by {1}";
        static readonly string AdditionalTransactionText = "{0} applied against sale/invoice {1}, by {2}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile SaleDoubleEntryManager instance;
        private static object syncRoot = new Object();
        SaleDoubleEntryManager()
        {
        }
        public static SaleDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SaleDoubleEntryManager();
                    }
                }
                return instance;
            }
        }   
        public void RecordSale(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            DeleteSale(SaleEntry, Context);
            Company Company = CompanyManager.Instance.GetCompany(SaleEntry.CompanyId);
            if (SaleEntry.EntryType == Entrytype.SALE && (SaleEntry.SaleMethod == SaleMethod.Credit
                                || (SaleEntry.SaleMethod == SaleMethod.Cash && !Company.CompanySalesSetup.IsReceivePayment)
                                || (SaleEntry.SaleMethod == SaleMethod.Cash && Company.CompanySalesSetup.IsReceivePayment && SaleEntry.isPaymentReceived == true)))
            {
                AddSale(SaleEntry, Context);
            }
        }
        /*
         * Deleteing all exisiting entries for the given company 
         */
        public void DeleteSale(SaleEntry Sale, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Sales && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Sale.RefNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void AddSale(SaleEntry Sale, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getCustomerEntry(Sale));
            Context.DoubleEntries.AddRange(GetSaleReturnEntry(Sale));
            Context.DoubleEntries.AddRange(getSaleReturnTaxEntries(Sale));
            Context.DoubleEntries.AddRange(getSaleReturnAdditionalTransactionEntries(Sale));
            if(Sale.RoundOff != 0) 
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
                Date = Sale.SaleDate,
                Description = string.Format(CustomerEntryForSaleText, Sale.RefNumber + " round off amount"),
                TransactionType = DaybookTransactionType.Sales,
                ReferenceTrasnactionId = Sale.RefNumber,
                Amount = Sale.RoundOff
            };
            return dBook;
        }
        public DayBook getCustomerEntry(SaleEntry Sale)
        {
            //for cash sale
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            if (Company.CashOnHandAccount == null && Sale.SaleMethod==SaleMethod.Cash)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            DayBook dBook = new DayBook()
            {
                AccountId = Sale.SaleMethod == SaleMethod.Credit ? Sale.AccountsId : Company.CashOnHandAccountId,
                CompanyId = Sale.CompanyId,
                CostCenterId = Sale.CostCenterId,
                Date = Sale.SaleDate,
                Description = string.Format(CustomerEntryForSaleText, Sale.RefNumber),
                TransactionType = DaybookTransactionType.Sales,
                ReferenceTrasnactionId = Sale.RefNumber
            };
            dBook.Debit(Sale.TotalAmount);
            if (Sale.SaleMethod == SaleMethod.Cash)
            {
                dBook.Debit(Sale.TotalAmount);
            }
            return dBook;
        }
        public List<DayBook> GetSaleReturnEntry(SaleEntry Sale)
        {
            List<DayBook> dBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Sale.CompanyId);
            //for cash sale 
            if (Company.CashOnHandAccount == null && Sale.SaleMethod == SaleMethod.Cash)
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
            double Amount = 0.00;
            foreach (SaleDetail Detail in Sale.SaleDetails)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Detail.ProductId);
                //get the company sales a/c
                if (Company.SalesAccount == null)
                {
                    Exception e = new Exception(InvalidCompanyConfigMsg);
                    throw e;
                }
                if (Product.SalesAccount == null || Company.SalesAccount.Id == Product.SalesAccount.Id)
                {
                    Amount += (Detail.Amount - Detail.TaxDetails.Sum(x => x.Amount));
                }
                else
                {
                    DayBook dBook = new DayBook
                    {
                        AccountId = Product.SalesAccount.Id,
                        CompanyId = Sale.CompanyId,
                        CostCenterId = Sale.CostCenterId,
                        Date = Sale.SaleDate,
                        Description = string.Format(SalesAccountEntryForInvoiceText, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : "cash"),
                        TransactionType = DaybookTransactionType.Sales,
                        ReferenceTrasnactionId = Sale.RefNumber
                    };
                    dBook.Credit(Detail.Amount - Detail.TaxDetails.Sum(x => x.Amount));
                    dBooks.Add(dBook);
                }
            }
            if (Amount > 0)
            {
                DayBook dBook = new DayBook
                {
                    AccountId = Company.SalesAccount.Id,
                    CompanyId = Sale.CompanyId,
                    CostCenterId = Sale.CostCenterId,
                    Date = Sale.SaleDate,
                    Description = string.Format(SalesAccountEntryForInvoiceText, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : "cash"),
                    TransactionType = DaybookTransactionType.Sales,
                    ReferenceTrasnactionId = Sale.RefNumber
                };
                dBook.Credit(Amount);
                dBooks.Add(dBook);
            }
            return dBooks;
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
                DayBook dBook = new DayBook
                {
                    AccountId = saleTaxDetail.TaxAccountId,
                    CompanyId = Sale.CompanyId,
                    CostCenterId = Sale.CostCenterId,
                    Date = Sale.SaleDate,
                    Amount = saleTaxDetail.Amount,
                    Description = string.Format(TaxAccountEntryForInvoiceText, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : Company.CashOnHandAccount.DisplayAs),
                    TransactionType = DaybookTransactionType.Sales,
                    ReferenceTrasnactionId = Sale.RefNumber
                };
                dBook.Credit(saleTaxDetail.Amount);
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
                DayBook dBook = new DayBook
                {
                    AccountId = SaleAdditionalTransaction.AccountId,
                    CompanyId = Sale.CompanyId,
                    CostCenterId = Sale.CostCenterId,
                    Date = Sale.SaleDate,
                    Description = string.Format(AdditionalTransactionText, SaleAdditionalTransaction.Name, Sale.RefNumber, Sale.SaleMethod == SaleMethod.Credit ? SaleCustomer.DisplayAs : Company.CashOnHandAccount.DisplayAs),
                    TransactionType = DaybookTransactionType.Sales,
                    ReferenceTrasnactionId = Sale.RefNumber
                };
                dBook.Credit(SaleAdditionalTransaction.Amount);
                dBooks.Add(dBook);                
            }
            return dBooks;
        }        
    }
}
