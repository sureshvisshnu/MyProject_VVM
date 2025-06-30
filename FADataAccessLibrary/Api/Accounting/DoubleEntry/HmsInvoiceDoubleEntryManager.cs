using fa.api.accounting.doubleentry;
using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Accounting.DoubleEntry
{
    internal class HmsInvoiceDoubleEntryManager
    {
        static string CustomerEntryForInvoice = "Patient Invoice {0}";
        static string SalesAccountEntryForInvoice = "Invoice {0}, to Patient {1}";
        static string ReceivableAccountForInvoice = "Account Receivable for Invoice {0}";
        static string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile HmsInvoiceDoubleEntryManager instance;
        private static object syncRoot = new Object();
        HmsInvoiceDoubleEntryManager()
        {
        }
        public static HmsInvoiceDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HmsInvoiceDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordInvoice(PatientInvoice Invoice, AccountMasterContext Context)
        {
            DeleteInvoices(Invoice, Context);
            RecordInvoices(Invoice, Context);
        }
        public void DeleteInvoices(PatientInvoice Invoice, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Invoice.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Invoice_Hms && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Invoice.ReferenceNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordInvoices(PatientInvoice Invoice, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Invoice.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            var dayBookEntries = GetPatientEntryForInvoice(Invoice);
            foreach (var entry in dayBookEntries)
            {
                Context.DoubleEntries.Add(entry);
            }
            Context.SaveChanges();
        }
        public List<DayBook> GetPatientEntryForInvoice(PatientInvoice Invoice)
        {
            DayBook dBook = new DayBook
            {
                PatientId = Invoice.PatientId,
                CompanyId = Invoice.CompanyId,
                Date = Invoice.InvoiceDate,
                Description = string.Format(CustomerEntryForInvoice, Invoice.ReferenceNumber),
                ReferenceTrasnactionId = Invoice.ReferenceNumber,
                TransactionType = DaybookTransactionType.Invoice_Hms,
                Amount = Invoice.Total
            };
            DayBook dBook1 = new DayBook
            {
                PatientId = Invoice.PatientId,
                CompanyId = Invoice.CompanyId,
                Date = Invoice.InvoiceDate,
                Description = string.Format(ReceivableAccountForInvoice, Invoice.ReferenceNumber),
                ReferenceTrasnactionId = Invoice.ReferenceNumber,
                TransactionType = DaybookTransactionType.Invoice_Hms,
                Amount = -Invoice.Total
            };

            return new List<DayBook> { dBook, dBook1 };
        }
    }
}
