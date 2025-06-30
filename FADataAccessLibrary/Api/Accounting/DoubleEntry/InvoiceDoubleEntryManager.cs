using System;
using System.Collections.Generic;
using System.Linq;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.api.Accounting;
using NPOI.SS.Formula.Functions;
using fa.model.OrderManagement;

namespace fa.api.accounting.doubleentry
{
    public class InvoiceDoubleEntryManager
    {
        static string CustomerEntryForInvoice = "Invoice {0}";
        static string SalesAccountEntryForInvoice = "Invoice {0}, to Customer {1}";
        static string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        static readonly string AdditionalTransactionText = "{0} applied against invoice {1}, by {2}";
        private static volatile InvoiceDoubleEntryManager instance;
        private static object syncRoot = new Object();
        InvoiceDoubleEntryManager()
        {
        }
        public static InvoiceDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InvoiceDoubleEntryManager();
                    }
                }            
                return instance;
            }
        }
        public void RecordInvoice(Invoice Invoice, AccountMasterContext Context)
        {
            DeleteInvoices(Invoice, Context);
            RecordInvoices(Invoice, Context);
        }
        //Deleteing all exisiting entries for the given company      
        public void DeleteInvoices(Invoice Invoice, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Invoice.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Invoice && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Invoice.ReferenceNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordInvoices(Invoice Invoice , AccountMasterContext Context )
        {            
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Invoice.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);               
                throw e;
            }
            Context.DoubleEntries.Add(getCustomerEntryForInvoice(Invoice));
            Context.DoubleEntries.AddRange(getSalesEntryForInvoice(Invoice));
            Context.DoubleEntries.AddRange(getAdditionalTransactionEntriesForInvoice(Invoice));
            Context.SaveChanges();
        }

        private List<DayBook> getAdditionalTransactionEntriesForInvoice(Invoice Invoice)
        {
            List<DayBook> dBooks = new List<DayBook>();
            if (Invoice.InvoiceAdditionalTransactions != null && Invoice.InvoiceAdditionalTransactions.Count > 0)
            {
                Company CurrentCompany = CompanyManager.Instance.GetCompany(Invoice.CompanyId);
                foreach (InvoiceAdditionalTransaction InvoiceAdditionalTransaction in Invoice.InvoiceAdditionalTransactions)
                {
                    DayBook dBook = new DayBook
                    {
                        AccountId = InvoiceAdditionalTransaction.AccountId,
                        CompanyId = Invoice.CompanyId,
                        CostCenterId = Invoice.CostCenterId,
                        Date = Invoice.InvoiceDate,
                        Description = string.Format(AdditionalTransactionText, InvoiceAdditionalTransaction.Name, Invoice.ReferenceNumber, CurrentCompany.CashOnHandAccount.DisplayAs),
                        TransactionType = DaybookTransactionType.Invoice,
                        ReferenceTrasnactionId = Invoice.ReferenceNumber
                    };
                    dBook.Debit(InvoiceAdditionalTransaction.Amount);
                    dBooks.Add(dBook);
                }
            }
            return dBooks;
        }

        public DayBook getCustomerEntryForInvoice(Invoice Invoice)
        {
            DayBook dBook = new DayBook
            {
                AccountId = Invoice.CustomerId,
                CompanyId = Invoice.CompanyId,
                CostCenterId = Invoice.CostCenterId,
                Date = Invoice.InvoiceDate,                
                Description = string.Format(CustomerEntryForInvoice, Invoice.ReferenceNumber + (string.IsNullOrEmpty(Invoice.Memo) ? "" : " " + Invoice.Memo)),
                ReferenceTrasnactionId = Invoice.ReferenceNumber,
                TransactionType = DaybookTransactionType.Invoice
            };
            dBook.Debit(Invoice.Total);
            return dBook;            
        }
        public List<DayBook> getSalesEntryForInvoice(Invoice Invoice)
        {
            List<DayBook> dayBooks = new List<DayBook>();            
            Account InvoiceCustomer = AccountManager.Instance.GetAccountById((long)Invoice.CustomerId);
            if (InvoiceCustomer == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            foreach (InvoiceDetail InvoiceDetail in Invoice.InvoiceDetails)
            {
                Account SalesAccount = AccountManager.Instance.GetAccountById((long)InvoiceDetail.SalesAccountId);
                if (SalesAccount != null)
                {
                    DayBook dBook = new DayBook();
                    dBook.AccountId = SalesAccount.Id;
                    dBook.CompanyId = Invoice.CompanyId;
                    dBook.CostCenterId = Invoice.CostCenterId;
                    dBook.Date = Invoice.InvoiceDate;
                    dBook.Description = string.Format(SalesAccountEntryForInvoice, InvoiceDetail.Invoice.ReferenceNumber + (string.IsNullOrEmpty(InvoiceDetail.Description) ? "" : " " + InvoiceDetail.Description), InvoiceCustomer.DisplayAs);                    
                    dBook.ReferenceTrasnactionId = Invoice.ReferenceNumber;
                    dBook.TransactionType = DaybookTransactionType.Invoice;
                    dBook.Credit(InvoiceDetail.Total);
                    dayBooks.Add(dBook);
                }
                else
                {
                    throw new Exception("Could not find the account information for " + InvoiceDetail.SalesAccountId);
                }
            }
            return dayBooks;
        }
    }
}
