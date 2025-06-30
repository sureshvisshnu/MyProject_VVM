using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.report.Purchase
{
    public enum PurchaseReturnReportType
    {
        BY_BILL = 1, BY_SUPPLIER = 2
    }
    public class PurchaseReturnReportNames
    {
        static string[] ReportNames = {"Purchase Return Report",
                                "Purchase Return Report"
                                };

        public static string getPurchaseReturnReportName(PurchaseReturnReportType PurchaseReturnReport)
        {
            return ReportNames[(int)PurchaseReturnReport - 1];
        }
    }
    public abstract class PurchaseReturnReport:Report
    {
    }
    //By supplier wise purchase return report
    public class PurchaseReturnReportBySupplier : PurchaseReturnReport
    {
        public long[] SupplierIds { get; set; }
        PurchaseReturnReportType Type = PurchaseReturnReportType.BY_SUPPLIER;
        IList<PurchaseReturnReportBySupplierLineItem> _LineItems = new List<PurchaseReturnReportBySupplierLineItem>();
        public IList<PurchaseReturnReportBySupplierLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return null;
        }
        public override string ReportTitle()
        {
            string reportTitle = PurchaseReturnReportNames.getPurchaseReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        public override void GenerateReport()
        {
            List<PurchaseEntry> Purchase = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                //fetch from multiple Supplier
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (SupplierIds.ToList().Count > 0)
                    {
                        Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN && PurchaseEntry.CompanyId == this.Company.CompanyId && SupplierIds.Contains((long)PurchaseEntry.AccountId) && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                    }
                    else
                    {
                        Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN && PurchaseEntry.CompanyId == this.Company.CompanyId && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                    }
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseEntry lPurchase in Purchase)
                {
                    Account account = AccountManager.Instance.GetAccountById((long)lPurchase.AccountId);
                    PurchaseReturnReportBySupplierLineItem LineItem = new PurchaseReturnReportBySupplierLineItem(lPurchase, account);
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class PurchaseReturnReportBySupplierLineItem
    {
        public String SupplierBillNumber { get; set; }
        public String SupplierName { get; set; }
        public DateTime SupplierBillDate { get; set; }
        public Double SupplierBillTax { get; set; }
        public Double SupplierBillDiscount { get; set; }
        public Double SupplierBillAmount { get; set; }
        public String BillType { get; set; }

        public PurchaseReturnReportBySupplierLineItem()
        {
        }

        public PurchaseReturnReportBySupplierLineItem(PurchaseEntry Purchase, Account account)
        {
            this.SupplierBillNumber = Purchase.RefNumber;
            this.SupplierName = account.Name;
            this.SupplierBillDate = Purchase.RefDate;
            this.SupplierBillTax = Purchase.TaxAmount;
            this.SupplierBillDiscount = Purchase.DisAmount;
            this.BillType = Purchase.PurchaseMethod == PurchaseMethod.Credit ? "Credit" : "Cash";
            this.SupplierBillAmount = Purchase.TotalAmount;
        }
    }

    //By bill purchase return report
    public class PurchaseReturnReportByBill : PurchaseReturnReport
    {
        PurchaseReturnReportType Type = PurchaseReturnReportType.BY_BILL;
        IList<PurchaseReturnReportByBillLineItem> _LineItems = new List<PurchaseReturnReportByBillLineItem>();

        public IList<PurchaseReturnReportByBillLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportTitle()
        {
            string reportTitle = PurchaseReturnReportNames.getPurchaseReturnReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }
        public override string ReportName()
        {
            return null;
        }
        //generate the report
        public override void GenerateReport()
        {
            List<PurchaseEntry> Purchase = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    //Should include the pagination later on when needed.
                    Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN && PurchaseEntry.CompanyId == this.Company.CompanyId && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseEntry lPurchase in Purchase)
                {
                    PurchaseReturnReportByBillLineItem LineItem = new PurchaseReturnReportByBillLineItem(lPurchase);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    public class PurchaseReturnReportByBillLineItem
    {
        public String BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public String SupplierName { get; set; }
        public String SupplierAddress { get; set; }
        public Double BillAmount { get; set; }
        public Double BillTax { get; set; }
        public Double BillNetAmount { get; set; }
        public String BillType { get; set; }

        public PurchaseReturnReportByBillLineItem()
        {
            //Default Constructor
        }

        public PurchaseReturnReportByBillLineItem(PurchaseEntry Purchase)
        {
            this.BillNetAmount = Purchase.NetAmount;
            this.BillTax = Purchase.TaxAmount;
            this.BillAmount = Purchase.TotalAmount;
            this.BillType = Purchase.PurchaseMethod == PurchaseMethod.Credit ? "Credit" : "Cash";
            this.SupplierName = Purchase.SupplierName;
            this.SupplierAddress = Purchase.SupplierAddress;
            this.BillNumber = Purchase.RefNumber;
            this.BillDate = Purchase.RefDate;
        }
    }
}
