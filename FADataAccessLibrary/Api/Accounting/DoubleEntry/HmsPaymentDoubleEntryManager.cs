using Fa.api.Accounting.DoubleEntry;
using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.api.Hms;
using fa.model.Accounting.Transactions;

namespace FADataAccessLibrary.Api.Accounting.DoubleEntry
{
    internal class HmsPaymentDoubleEntryManager
    {
        static readonly string SupplierEntryForPayment = "Payment received {0}";
        static readonly string SalesAccountEntryForPayment = "Payment {0}, from Patient {1}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile HmsPaymentDoubleEntryManager instance;
        private static object syncRoot = new Object();
        HmsPaymentDoubleEntryManager()
        {
        }
        public static HmsPaymentDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HmsPaymentDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordPayment(PatientLedger Payment, AccountMasterContext Context)
        {
            DeletePayments(Payment, Context);
            RecordPayments(Payment, Context);
        }
        public void DeletePayments(PatientLedger Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Payment_Hms && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Payment.RefNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordPayments(PatientLedger Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getFromPatientEntryForPayment(Payment));
            Context.SaveChanges();
        }
        public DayBook getFromPatientEntryForPayment(PatientLedger Payment)
        {
            DayBook dBook = new DayBook();
            dBook.PatientId = Payment.PatientId;
            dBook.CompanyId = Payment.CompanyId;
            dBook.Date = Payment.Date;
            dBook.Amount=-Payment.Amount;
            dBook.Description = string.Format(SupplierEntryForPayment, Payment.RefNumber);
            dBook.ReferenceTrasnactionId = Payment.RefNumber;
            dBook.TransactionType = DaybookTransactionType.Payment_Hms;
            return dBook;
        }
       
    }
}
