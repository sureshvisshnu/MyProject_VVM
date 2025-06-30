using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Fa.api.Accounting.DoubleEntry
{
    public class BillDoubleEntryManager
    {
        static readonly string SupplierEntryForBill = "Bill {0}";
        static readonly string PurchaseAccountEntryForBill = "Bill {0}, from Supplier {1}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile BillDoubleEntryManager instance;
        private static object syncRoot = new Object();
        BillDoubleEntryManager()
        {
        }
        public static BillDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BillDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordBill(Bill Bill, AccountMasterContext Context)
        {
            DeleteBills(Bill, Context);
            RecordBills(Bill, Context);
        }
        //Deleteing all exisiting entries for the given company      
        public void DeleteBills(Bill Bill, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Bill.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Bill && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Bill.ReferenceNumber).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordBills(Bill Bill, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Bill.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getSupplierEntryForBill(Bill));
            Context.DoubleEntries.AddRange(getPurchaseEntryForBill(Bill));
            Context.SaveChanges();
        }
        public DayBook getSupplierEntryForBill(Bill Bill)
        {
            DayBook dBook = new DayBook();
            dBook.AccountId = Bill.VendorId;
            dBook.CompanyId = Bill.CompanyId;
            dBook.CostCenterId = Bill.CostCenterId;
            dBook.CostCenterId = Bill.CostCenterId;
            dBook.Date = Bill.BillDate;
            dBook.Credit(Bill.Total);
            dBook.Description = string.Format(SupplierEntryForBill, Bill.ReferenceNumber + (string.IsNullOrEmpty(Bill.Memo) ? "" : " " + Bill.Memo));
            dBook.ReferenceTrasnactionId = Bill.ReferenceNumber;
            dBook.TransactionType = DaybookTransactionType.Bill;
            return dBook;
        }
        public List<DayBook> getPurchaseEntryForBill(Bill Bill)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Account BillSupplier = AccountManager.Instance.GetAccountById((long)Bill.VendorId);
            if (BillSupplier == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            foreach (BillDetail BillDetail in Bill.BillDetails)
            {
                DayBook dBook = new DayBook();
                Account PurchaseAccount = AccountManager.Instance.GetAccountById((long)BillDetail.AccountId);
                if (PurchaseAccount != null)
                {
                    dBook.AccountId = PurchaseAccount.Id;
                    dBook.CompanyId = Bill.CompanyId;
                    dBook.CostCenterId = Bill.CostCenterId;
                    dBook.Date = Bill.BillDate;
                    dBook.Debit(BillDetail.Amount);
                    dBook.Description = string.Format(PurchaseAccountEntryForBill, BillDetail.Bill.ReferenceNumber + (string.IsNullOrEmpty(BillDetail.Description) ? "" : " " + BillDetail.Description), BillSupplier.DisplayAs);
                    dBook.ReferenceTrasnactionId = Bill.ReferenceNumber;
                    dBook.TransactionType = DaybookTransactionType.Bill;
                    dayBooks.Add(dBook);
                }
            }
            return dayBooks;
        }
    }
}
