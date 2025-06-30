using fa.api.Accounting;
using fa.api.catalog;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.report;
using Fa.model.Purchase;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Properties;

namespace Fa.report.Purchase
{
    public enum PurchaseReportType
    {
        BY_BILL = 1, BY_SUPPLIER = 2, BY_CATEGORY = 3, BY_ITEM = 4, BY_TAX = 5
    }

    public class PurchaseReportNames
    {
        static string[] ReportNames = {"Bill Wise Purchase Report",
                                "Supplier Wise Purchase Report",
                                "Category Wise Purchase Report",
                                "Item Wise Purchase Report",
                                "Tax Wise Purchase Report"
                                };

        public static string getPurchaseReportName(PurchaseReportType PurchaseReport)
        {
            return ReportNames[(int)PurchaseReport - 1];
        }
    }
    public abstract class PurchaseReport:Report
    {
    }
    //By Supplier wise Purchase report
    public class PurchaseReportBySupplier : PurchaseReport
    {
        public long[] SupplierIds { get; set; }
        PurchaseReportType Type = PurchaseReportType.BY_SUPPLIER;
        IList<PurchaseReportBySupplierLineItem> _LineItems = new List<PurchaseReportBySupplierLineItem>();
        public IList<PurchaseReportBySupplierLineItem> LineItems
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
            string reportTitle = PurchaseReportNames.getPurchaseReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
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
                        Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype==PurchaseEntrytype.PURCHASE && PurchaseEntry.CompanyId == this.Company.CompanyId && SupplierIds.Contains((long)PurchaseEntry.AccountId) && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                    }
                    else
                    {
                        Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && PurchaseEntry.CompanyId == this.Company.CompanyId && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                    }
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseEntry lPurchase in Purchase)
                {
                    PurchaseReportBySupplierLineItem LineItem = new PurchaseReportBySupplierLineItem(lPurchase);
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class PurchaseReportBySupplierLineItem
    {
        public String SupplierBillNumber { get; set; }
        public DateTime SupplierBillDate { get; set; }
        public Double SupplierBillTax { get; set; }
        public Double SupplierBillDiscount { get; set; }
        public Double SupplierBillAmount { get; set; }
        public String BillType { get; set; }
        public String SupplierName { get; set; }

        public PurchaseReportBySupplierLineItem()
        {
        }

        public PurchaseReportBySupplierLineItem(PurchaseEntry Purchase)
        {
            this.SupplierBillNumber = Purchase.RefNumber;
            this.SupplierBillDate = Purchase.RefDate;
            this.SupplierBillTax = Purchase.TaxAmount;
            this.SupplierBillDiscount = Purchase.DisAmount;
            this.BillType = Purchase.PurchaseMethod == PurchaseMethod.Credit ? "Credit" : "Cash";
            this.SupplierBillAmount = Purchase.TotalAmount;
            this.SupplierName = Purchase.SupplierName;
        }
    }

    //By Bill Purchase report
    public class PurchaseReportByBill : PurchaseReport
    {
        PurchaseReportType Type = PurchaseReportType.BY_BILL;
        IList<PurchaseReportByBillLineItem> _LineItems = new List<PurchaseReportByBillLineItem>();

        public IList<PurchaseReportByBillLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportTitle()
        {
            string reportTitle = PurchaseReportNames.getPurchaseReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
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
                    Purchase = (from PurchaseEntry in Context.PurchaseEntry where (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && PurchaseEntry.CompanyId == this.Company.CompanyId && PurchaseEntry.RefDate >= this.FromDate && PurchaseEntry.RefDate <= this.ToDate) select PurchaseEntry).ToList();
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseEntry lPurchase in Purchase)
                {
                    PurchaseReportByBillLineItem LineItem = new PurchaseReportByBillLineItem(lPurchase);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    public class PurchaseReportByBillLineItem
    {
        public String BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public String SupplierName { get; set; }
        public String SupplierAddress { get; set; }
        public Double BillAmount { get; set; }
        public Double BillTax { get; set; }
        public Double BillNetAmount { get; set; }
        public String BillType { get; set; }

        public PurchaseReportByBillLineItem()
        {
            //Default Constructor
        }

        public PurchaseReportByBillLineItem(PurchaseEntry Purchase)
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
    //By Tax purchase report
    public class PurchaseReportByTax : PurchaseReport
    {
        PurchaseReportType Type = PurchaseReportType.BY_TAX;
        IList<PurchaseReportByTaxLineItem> _LineItems = new List<PurchaseReportByTaxLineItem>();

        public IList<PurchaseReportByTaxLineItem> LineItems
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
            string reportTitle = PurchaseReportNames.getPurchaseReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //generate the report
        public override void GenerateReport()
        {
            List<PurchaseEntry> PurchaseEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    //Should include the pagination later on when needed.
                    PurchaseEntrys = (from purchase in Context.PurchaseEntry.Include("PurchaseDetails").Include("PurchaseDetails.TaxDetails").Include("PurchaseAdditionalTransactions") where (purchase.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && purchase.CompanyId == this.Company.CompanyId && purchase.RefDate >= this.FromDate && purchase.RefDate <= this.ToDate) select purchase).ToList();
                }
            }
            if (PurchaseEntrys != null && PurchaseEntrys.Count > 0)
            {
                foreach (PurchaseEntry Purchase in PurchaseEntrys)
                {
                    Customer Customer = null;
                    Supplier Supplier = null;
                    string Gst = string.Empty;
                    if (Purchase.AccountId != null)
                    {
                        Customer = CustomerManager.Instance.GetCustomerById((long)Purchase.AccountId);
                        if (Customer == null)
                        {
                            Supplier = SupplierManager.Instance.GetSupplierById((long)Purchase.AccountId);
                        }
                        if ((Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                                || (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0))
                        {
                            Gst = Customer != null && Customer.CustomerLicenceDetail.Count > 0 ? Customer.CustomerLicenceDetail.First().Value : Supplier.SupplierLicenceDetail.First().Value;
                        }
                    }
                    PurchaseReportByTaxLineItem LineItem = new PurchaseReportByTaxLineItem(Purchase, Gst);
                    LineItems.Add(LineItem);
                }
            }
        }
    }

    public class PurchaseReportByTaxLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public String Gstn { get; set; }
        public Double ZeroTaxvalue { get; set; }
        public Double ZeroTaxAmount { get; set; }
        public Double FiveTaxvalue { get; set; }
        public Double FiveTaxAmount { get; set; }
        public Double TwelveTaxvalue { get; set; }
        public Double TwelveTaxAmount { get; set; }
        public Double EighteenTaxvalue { get; set; }
        public Double EighteenTaxAmount { get; set; }
        public Double TwentyeightTaxvalue { get; set; }
        public Double TwentyeightTaxAmount { get; set; }
        public Double OtherTaxvalue { get; set; }
        public Double OtherTaxAmount { get; set; }
        public Double Charges { get; set; }
        public Double Total { get; set; }

        public PurchaseReportByTaxLineItem()
        {
            //Default Constructor
        }

        public PurchaseReportByTaxLineItem(PurchaseEntry Purchase, string Gst)
        {
            foreach (PurchaseDetails Detail in Purchase.PurchaseDetails)
            {
                float TaxRate = Detail.TaxDetails.Sum(s => s.TaxRate);
                if (TaxRate == 0)
                {
                    this.ZeroTaxvalue += Detail.Amount;
                }
                else if (TaxRate == 5)
                {
                    this.FiveTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.FiveTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 12)
                {
                    this.TwelveTaxvalue += (Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount));
                    this.TwelveTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 18)
                {
                    this.EighteenTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.EighteenTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else if (TaxRate == 28)
                {
                    this.TwentyeightTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.TwentyeightTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
                else
                {
                    this.OtherTaxvalue += Detail.Amount - Detail.TaxDetails.Sum(s => s.Amount);
                    this.OtherTaxAmount += Detail.TaxDetails.Sum(s => s.Amount);
                }
            }
            this.CustomerName = Purchase.SupplierName;
            this.Gstn = Gst;
            this.InvoiceNumber = Purchase.RefNumber;
            this.InvoiceDate = Purchase.RefDate;
            this.Charges = Purchase.PurchaseAdditionalTransactions.Sum(x => x.Action == AdditionalTransactionAction.DR ? x.Amount : -x.Amount);
            this.Total = Purchase.TotalAmount;

        }
    }

    //By Categoery Purchase report
    public class PurchaseReportByCategory : PurchaseReport
    {
        public long[] CategoryIds { get; set; }
        PurchaseReportType Type = PurchaseReportType.BY_CATEGORY;
        IList<PurchaseReportByCategoryLineItem> _LineItems = new List<PurchaseReportByCategoryLineItem>();

        public IList<PurchaseReportByCategoryLineItem> LineItems
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
            string reportTitle = PurchaseReportNames.getPurchaseReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //fetch from multiple category
        public override void GenerateReport()
        {
            List<PurchaseDetails> Purchase = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (CategoryIds.ToList().Count > 0)
                    {
                        var ProductIds = (from lCatalogItem in Context.CatalogItems where (((from Pfamily in Context.CatalogItems where CategoryIds.Contains((long)Pfamily.ParentId) select Pfamily.Id).ToList()).Contains((long)lCatalogItem.ParentId)) where (lCatalogItem.Type == CatalogItemType.PRODUCT) select lCatalogItem.Id).ToList();
                        Purchase = (from PurchaseDetails in Context.PurchaseDetails.Include("PurchaseEntry").Include("Product").Include("TaxDetails") where ProductIds.Contains((long)PurchaseDetails.ProductId) where (PurchaseDetails.PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && PurchaseDetails.CompanyId == this.Company.CompanyId && PurchaseDetails.PurchaseEntry.RefDate >= this.FromDate && PurchaseDetails.PurchaseEntry.RefDate <= this.ToDate) select PurchaseDetails).ToList();
                    }
                    else
                    {
                        Purchase = (from PurchaseDetails in Context.PurchaseDetails.Include("PurchaseEntry").Include("Product").Include("TaxDetails") where (PurchaseDetails.PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && PurchaseDetails.CompanyId == this.Company.CompanyId && PurchaseDetails.PurchaseEntry.RefDate >= this.FromDate && PurchaseDetails.PurchaseEntry.RefDate <= this.ToDate) select PurchaseDetails).ToList();
                    }
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseDetails PurchaseDetail in Purchase)
                {
                    CatalogItem catalogItem = CatalogItemManager.Instance.GetParentInfoById((long)PurchaseDetail.ProductId);
                    PurchaseReportByCategoryLineItem LineItem = new PurchaseReportByCategoryLineItem(PurchaseDetail, catalogItem);
                    PurchaseReportByCategoryLineItem OldLineItem = null;
                    if (PurchaseDetail.isBatch)
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == PurchaseDetail.ProductId && x.BatchNumber == PurchaseDetail.BatchNo);
                    }
                    else
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == PurchaseDetail.ProductId);
                    }
                    if (OldLineItem != null)
                    {
                        LineItem.Qty += OldLineItem.Qty;
                        LineItem.Free += OldLineItem.Free;
                        LineItem.SubTotal += OldLineItem.SubTotal;
                        LineItem.Tax += OldLineItem.Tax;
                        LineItem.Total += OldLineItem.Total;
                        LineItems.Remove(OldLineItem);
                    }
                    LineItems.Add(LineItem);
                }
            }
        }
    }
    public class PurchaseReportByCategoryLineItem
    {
        public String Item { get; set; }
        public String ItemName { get; set; }
        public String Catagory { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public long ProductId { get; set; }

        public PurchaseReportByCategoryLineItem()
        {
        }
        public PurchaseReportByCategoryLineItem(PurchaseDetails PurchaseDetail, CatalogItem catalogItem)
        {
            this.Item = PurchaseDetail.Product.MaterialId;
            this.ItemName = PurchaseDetail.Product.Name;
            this.Catagory = catalogItem.Parent.Parent.Name;
            this.Qty = PurchaseDetail.Quantity;
            this.Free = PurchaseDetail.FreeQuantity;
            this.BatchNumber = PurchaseDetail.isBatch ? PurchaseDetail.BatchNo : "";
            this.ExpDate = PurchaseDetail.ExpDate;
            this.SubTotal = (PurchaseDetail.Amount - PurchaseDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = PurchaseDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = PurchaseDetail.Amount;
            this.ProductId = (long)PurchaseDetail.ProductId;
        }
    }

    //By Item Purchase report
    public class PurchaseReportByItem : PurchaseReport
    {
        PurchaseReportType Type = PurchaseReportType.BY_ITEM;
        public long[] ItemIds { get; set; }
        IList<PurchaseReportByCategoryLineItem> _LineItems = new List<PurchaseReportByCategoryLineItem>();
        public IList<PurchaseReportByCategoryLineItem> LineItems
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
            string reportTitle = PurchaseReportNames.getPurchaseReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return null;
        }

        //fetch from multiple category
        public override void GenerateReport()
        {
            List<PurchaseDetails> Purchase = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    Purchase = (from PurchaseDetails in Context.PurchaseDetails.Include("PurchaseEntry").Include("Product").Include("TaxDetails") where ItemIds.Contains((long)PurchaseDetails.ProductId) where (PurchaseDetails.PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE && PurchaseDetails.CompanyId == this.Company.CompanyId && PurchaseDetails.PurchaseEntry.RefDate >= this.FromDate && PurchaseDetails.PurchaseEntry.RefDate <= this.ToDate) select PurchaseDetails).ToList();
                }
            }
            if (Purchase != null && Purchase.Count > 0)
            {
                foreach (PurchaseDetails PurchaseDetails in Purchase)
                {
                    CatalogItem catalogItem = CatalogItemManager.Instance.GetParentInfoById((long)PurchaseDetails.ProductId);
                    PurchaseReportByCategoryLineItem LineItem = new PurchaseReportByCategoryLineItem(PurchaseDetails, catalogItem);
                    PurchaseReportByCategoryLineItem OldLineItem = null;
                    if (PurchaseDetails.isBatch)
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == PurchaseDetails.ProductId && x.BatchNumber == PurchaseDetails.BatchNo);
                    }
                    else
                    {
                        OldLineItem = LineItems.FirstOrDefault(x => x.ProductId == PurchaseDetails.ProductId);
                    }
                    if (OldLineItem != null)
                    {
                        LineItem.Qty += OldLineItem.Qty;
                        LineItem.Free += OldLineItem.Free;
                        LineItem.SubTotal += OldLineItem.SubTotal;
                        LineItem.Tax += OldLineItem.Tax;
                        LineItem.Total += OldLineItem.Total;

                        LineItems.Remove(OldLineItem);
                    }
                    LineItems.Add(LineItem);
                }
            }
        }
    }
}
