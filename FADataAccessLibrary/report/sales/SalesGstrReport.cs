using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using FaData.Utils;
using FADataAccessLibrary.report.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Fa.report.sales
{
    public enum SalesGstrReportType
    {
        HSN = 1, BUSINESS_TO_BUSINESS = 2, BUSINESS_TO_CUST_LARGE = 3, BUSINESS_TO_CUST_SMALL = 4,BUSINESS_TO_BUSINESS_OTHERS = 5
    }

    public class SalesGstrReportNames
    {
        static string[] ReportNames = {"Sales GSTR report",
                                "Sales GSTR report",
                                "Sales GSTR report",
                                "Sales GSTR report",
                                "Sales GSTR report"
                                };

        public static string getSalesReportName(SalesGstrReportType SalesGstrReport)
        {
            return ReportNames[(int)SalesGstrReport - 1];
        }
    }
    public abstract class SalesGstrReport : Report
    {
    }

    /*.......HSN..........*/

    public class SalesGstrReportHsn : SalesGstrReport
    {
        SalesGstrReportType Type = SalesGstrReportType.HSN;
        IList<SalesGstrReportHsnLineItem> _LineItems = new List<SalesGstrReportHsnLineItem>();

        public IList<SalesGstrReportHsnLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return SalesGstrReportNames.getSalesReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesGstrReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }

        public override void GenerateReport()
        {
            List<SaleDetail> SaleDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == SalesGstrReportType.HSN)
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        SaleDetail = (from Detail in Context.SaleDetail where (Detail.Sale.EntryType == Entrytype.SALE && Detail.CompanyId == this.Company.CompanyId && Detail.Sale.SaleDate >= this.FromDate && Detail.Sale.SaleDate <= this.ToDate) select Detail).OrderBy(x => x.Product.HSNCode).ToList();
                    }
                    if (SaleDetail != null && SaleDetail.Count > 0)
                    {
                        int SaleTaxCount = Context.CompanySalesTaxAccountMaps.ToList().Select(x => x.Name).Distinct().Count();
                        string Hsn = "####";
                        double Qty = 0.00;
                        double Free = 0.00;
                        float Total = 0;
                        float[] TaxAmount = new float[SaleTaxCount];
                        SaleDetail llSaleDetail = new SaleDetail();

                        int i = 0;
                        foreach (SaleDetail lSaleDetail in SaleDetail)
                        {
                            if (lSaleDetail.Sale == null)
                            {
                                lSaleDetail.Sale = Context.SaleEntry.Include("Account").FirstOrDefault(x => x.Id == lSaleDetail.SaleId);
                            }
                            if (lSaleDetail.Product == null)
                            {
                                lSaleDetail.Product = Context.Products.Find(lSaleDetail.ProductId);
                            }
                            if (lSaleDetail.TaxDetails.Count == 0)
                            {
                                lSaleDetail.TaxDetails = Context.ItemLevelSaleTaxDetails.Where(x => x.SaleDetailsId == lSaleDetail.Id).ToList<ItemLevelSaleTaxDetail>();
                            }
                            int c = 0;
                            if (Hsn != lSaleDetail.Product.HSNCode)
                            {
                                if (i != 0)
                                {
                                    llSaleDetail.Quantity = Qty;
                                    llSaleDetail.FreeQuantity = Free;
                                    llSaleDetail.Amount = Total;
                                    llSaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                    c = 0;
                                    for (int j = 0; j < SaleTaxCount; j++)
                                    {
                                        ItemLevelSaleTaxDetail detail = new ItemLevelSaleTaxDetail();
                                        detail.TaxSequence = c + 1;
                                        detail.Amount = TaxAmount[c];
                                        llSaleDetail.TaxDetails.Add(detail);
                                        c++;
                                    }

                                    SalesGstrReportHsnLineItem LineItem = new SalesGstrReportHsnLineItem(llSaleDetail);
                                    LineItems.Add(LineItem);
                                }
                                llSaleDetail = lSaleDetail;
                                Hsn = lSaleDetail.Product.HSNCode;
                                Qty = lSaleDetail.Quantity;
                                Free = lSaleDetail.FreeQuantity;
                                Total = lSaleDetail.Amount;
                                TaxAmount = new float[SaleTaxCount];
                                c = 0;
                                foreach (ItemLevelSaleTaxDetail detail in lSaleDetail.TaxDetails)
                                {
                                    TaxAmount[c] = detail.Amount;
                                    c++;
                                }
                            }
                            else
                            {
                                Qty = Qty + lSaleDetail.Quantity;
                                Free = Free + lSaleDetail.FreeQuantity;
                                Total = Total + lSaleDetail.Amount;
                                c = 0;
                                foreach (ItemLevelSaleTaxDetail detail in lSaleDetail.TaxDetails)
                                {
                                    TaxAmount[c] = TaxAmount[c] + detail.Amount;
                                    c++;
                                }
                            }
                            i++;
                            if (i == SaleDetail.Count)
                            {
                                llSaleDetail.Quantity = Qty;
                                llSaleDetail.FreeQuantity = Free;
                                llSaleDetail.Amount = Total;
                                llSaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                c = 0;
                                for (int j = 0; j < SaleTaxCount; j++)
                                {
                                    ItemLevelSaleTaxDetail detail = new ItemLevelSaleTaxDetail();
                                    detail.TaxSequence = c + 1;
                                    detail.Amount = TaxAmount[c];
                                    llSaleDetail.TaxDetails.Add(detail);
                                    c++;
                                }
                                SalesGstrReportHsnLineItem LineItem = new SalesGstrReportHsnLineItem(llSaleDetail);
                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
            }
        }
    }

    public class SalesGstrReportHsnLineItem
    {
        public String Hsn { get; set; }
        public String Description { get; set; }
        public String Uqc { get; set; }
        public String CustomerAddress { get; set; }
        public Double TotalQuantity { get; set; }
        public Double TotalValue { get; set; }
        public Double TaxableValue { get; set; }
        public Double CentralTaxAmount { get; set; }
        public Double StateTaxAmount { get; set; }
        public Double CessAmount { get; set; }

        public SalesGstrReportHsnLineItem()
        {
        }

        public SalesGstrReportHsnLineItem(SaleDetail SaleDetail)
        {
            this.Hsn = SaleDetail.Product.HSNCode;
            this.Description = "";
            this.Uqc = SaleDetail.MaterialId;
            this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
            this.TotalQuantity = SaleDetail.Quantity + SaleDetail.FreeQuantity;
            this.TotalValue = SaleDetail.Amount;
            this.CessAmount = 0.00;
            this.TaxableValue = SaleDetail.Amount - (SaleDetail.TaxDetails.Sum(x => x.Amount));
            foreach (ItemLevelSaleTaxDetail Detail in SaleDetail.TaxDetails)
            {
                if (Detail.TaxSequence == 1)
                {
                    this.CentralTaxAmount = Detail.Amount;
                }
                else
                {
                    this.StateTaxAmount = Detail.Amount;
                }
            }

        }
    }

    /*Business to Business*/
    public class SalesGstrReportBtoB : SalesGstrReport
    {
        SalesGstrReportType Type = SalesGstrReportType.BUSINESS_TO_BUSINESS;
        IList<SalesGstrReportBtoBLineItem> _LineItems = new List<SalesGstrReportBtoBLineItem>();

        public IList<SalesGstrReportBtoBLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return SalesGstrReportNames.getSalesReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesGstrReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }
        public int InvoiceCount = 0;
        public int ReceipientCount = 0;

        public override void GenerateReport()
        {
            List<SaleEntry> SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == SalesGstrReportType.BUSINESS_TO_BUSINESS)
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        SaleEntry = (from Sale in Context.SaleEntry where (Sale.EntryType == Entrytype.SALE && Sale.SaleTaxType == SaleTaxType.INTER && Sale.CompanyId == this.Company.CompanyId && Sale.SaleDate >= this.FromDate && Sale.SaleDate <= this.ToDate) select Sale).OrderByDescending(x => x.SaleDate).ToList();
                    }
                    if (SaleEntry != null && SaleEntry.Count > 0)
                    {
                        // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
                        InvoiceCount = SaleEntry.Count;
                        ReceipientCount = SaleEntry.Select(x => x.AccountsId).Distinct().ToList().Count;
                        foreach (SaleEntry lSaleEntry in SaleEntry)
                        {
                            Customer Customer = null;
                            Supplier Supplier = null;
                            string Gst = string.Empty;
                            if (lSaleEntry.SaleDetails.Count == 0)
                            {
                                lSaleEntry.SaleDetails = Context.SaleDetail.Include("TaxDetails").Where(x => x.SaleId == lSaleEntry.Id).OrderBy(x => x.TaxDetails.Sum(s => s.TaxRate)).ToList();
                            }
                            if (lSaleEntry.AccountsId != null)
                            {
                                Customer = CustomerManager.Instance.GetCustomerById((long)lSaleEntry.AccountsId);
                                if (Customer == null)
                                {
                                    Supplier = SupplierManager.Instance.GetSupplierById((long)lSaleEntry.AccountsId);
                                }
                                if ((Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                                        || (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0))
                                {
                                    Gst = Customer != null && Customer.CustomerLicenceDetail.Count > 0 ? Customer.CustomerLicenceDetail.First().Value : Supplier.SupplierLicenceDetail.First().Value;
                                    float TaxAmount = 0;
                                    float Amount = 0;
                                    float TaxPercent = -1;
                                    SaleDetail SaleDetail = new SaleDetail();
                                    int i = 0;
                                    foreach (SaleDetail Detail in lSaleEntry.SaleDetails)
                                    {
                                        if (TaxPercent != Detail.TaxDetails.Sum(x => x.TaxRate))
                                        {
                                            if (i != 0)
                                            {
                                                SaleDetail.Amount = Amount;
                                                SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                                ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                                ItemLevelSaleTax.Amount = TaxAmount;
                                                ItemLevelSaleTax.TaxRate = TaxPercent;
                                                SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                                SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail, Gst);
                                                LineItems.Add(LineItem);
                                            }
                                            SaleDetail = Detail;
                                            TaxPercent = Detail.TaxDetails.Sum(x => x.TaxRate);

                                            TaxAmount = Detail.TaxDetails.Sum(x => x.Amount);
                                            Amount = Detail.Amount;
                                        }
                                        else
                                        {
                                            TaxAmount += Detail.TaxDetails.Sum(x => x.Amount);
                                            Amount += Detail.Amount;
                                        }
                                        i++;
                                        if (i == lSaleEntry.SaleDetails.Count)
                                        {
                                            SaleDetail.Amount = Amount;
                                            SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                            ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                            ItemLevelSaleTax.Amount = TaxAmount;
                                            ItemLevelSaleTax.TaxRate = TaxPercent;
                                            SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                            SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail, Gst);
                                            LineItems.Add(LineItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class SalesGstrReportBtoBLineItem
    {
        public String Name { get; set; }
        public String Gstn { get; set; }
        public String Invoice { get; set; }
        public DateTime Date { get; set; }
        public Double value { get; set; }
        public string place { get; set; }
        public string RevCharge { get; set; }
        public string Type { get; set; }
        public string EcomGstn { get; set; }
        public Double Rate { get; set; }
        public Double TaxableValue { get; set; }
        public Double CessAmount { get; set; }

        public SalesGstrReportBtoBLineItem()
        {
        }

        public SalesGstrReportBtoBLineItem(SaleEntry SaleEntry, SaleDetail SaleDetail ,string Gst)
        {
            // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
            this.Name = SaleEntry.CustomerName;
            this.Gstn = Gst;           
            this.Invoice = SaleEntry.RefNumber;
            this.Date = SaleEntry.SaleDate.Date;
            this.value = SaleDetail.Amount;
            this.RevCharge = "N";
            this.Type = "Regular";
            this.EcomGstn = "";
            this.Rate = SaleDetail.TaxDetails.First().TaxRate;
            this.CessAmount = SaleDetail.TaxDetails.Sum(x=>x.Amount);
            this.TaxableValue = SaleDetail.Amount - SaleDetail.TaxDetails.First().Amount;
        }

        public SalesGstrReportBtoBLineItem(SaleDetail SaleDetail)
        {
            this.Type = "OE";
            this.EcomGstn = "";
            this.Rate = SaleDetail.TaxDetails.First().TaxRate;
            this.CessAmount = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.TaxableValue = SaleDetail.Amount - SaleDetail.TaxDetails.First().Amount;
        }
    }

    /*Business to Business Others*/
    public class SalesGstrReportBtoBA : SalesGstrReport
    {
        SalesGstrReportType Type = SalesGstrReportType.BUSINESS_TO_BUSINESS_OTHERS;
        IList<SalesGstrReportBtoBLineItem> _LineItems = new List<SalesGstrReportBtoBLineItem>();

        public IList<SalesGstrReportBtoBLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return SalesGstrReportNames.getSalesReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesGstrReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }
        public int InvoiceCount = 0;
        public int ReceipientCount = 0;

        public override void GenerateReport()
        {
            List<SaleEntry> SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntry = (from Sale in Context.SaleEntry where (Sale.EntryType == Entrytype.SALE && Sale.SaleTaxType==SaleTaxType.INTRA && Sale.CompanyId == this.Company.CompanyId && Sale.SaleDate >= this.FromDate && Sale.SaleDate <= this.ToDate) select Sale).OrderByDescending(x => x.SaleDate).ToList();
                }
                if (SaleEntry != null && SaleEntry.Count > 0)
                {
                    // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
                    //InvoiceCount = SaleEntry.Count;
                   // ReceipientCount = SaleEntry.Select(x => x.AccountsId).Distinct().ToList().Count;
                    foreach (SaleEntry lSaleEntry in SaleEntry)
                    {
                        Customer Customer = null;
                        Supplier Supplier = null;
                        string Gst = string.Empty;
                        if (lSaleEntry.SaleDetails.Count == 0)
                        {
                            lSaleEntry.SaleDetails = Context.SaleDetail.Include("TaxDetails").Where(x => x.SaleId == lSaleEntry.Id).OrderBy(x => x.TaxDetails.Sum(s => s.TaxRate)).ToList();
                        }
                        if (lSaleEntry.AccountsId != null)
                        {
                            Customer = CustomerManager.Instance.GetCustomerById((long)lSaleEntry.AccountsId);
                            if (Customer == null)
                            {
                                Supplier = SupplierManager.Instance.GetSupplierById((long)lSaleEntry.AccountsId);
                            }
                            if ((Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                                    || (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0))
                            {
                                Gst = Customer != null && Customer.CustomerLicenceDetail.Count > 0 ? Customer.CustomerLicenceDetail.First().Value : Supplier.SupplierLicenceDetail.First().Value;
                                float TaxAmount = 0;
                                float Amount = 0;
                                float TaxPercent = -1;
                                SaleDetail SaleDetail = new SaleDetail();
                                int i = 0;
                                foreach (SaleDetail Detail in lSaleEntry.SaleDetails)
                                {
                                    if (TaxPercent != Detail.TaxDetails.Sum(x => x.TaxRate))
                                    {
                                        if (i != 0)
                                        {
                                            SaleDetail.Amount = Amount;
                                            SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                            ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                            ItemLevelSaleTax.Amount = TaxAmount;
                                            ItemLevelSaleTax.TaxRate = TaxPercent;
                                            SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                            SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail, Gst);
                                            LineItems.Add(LineItem);
                                        }
                                        SaleDetail = Detail;
                                        TaxPercent = Detail.TaxDetails.Sum(x => x.TaxRate);

                                        TaxAmount = Detail.TaxDetails.Sum(x => x.Amount);
                                        Amount = Detail.Amount;
                                    }
                                    else
                                    {
                                        TaxAmount += Detail.TaxDetails.Sum(x => x.Amount);
                                        Amount += Detail.Amount;
                                    }
                                    i++;
                                    if (i == lSaleEntry.SaleDetails.Count)
                                    {
                                        SaleDetail.Amount = Amount;
                                        SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                        ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                        ItemLevelSaleTax.Amount = TaxAmount;
                                        ItemLevelSaleTax.TaxRate = TaxPercent;
                                        SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                        SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail, Gst);
                                        LineItems.Add(LineItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //public class SalesGstrReportBtoBLineItem
    //{
    //    public String Name { get; set; }
    //    public String Gstn { get; set; }
    //    public String Invoice { get; set; }
    //    public DateTime Date { get; set; }
    //    public Double value { get; set; }
    //    public string place { get; set; }
    //    public string RevCharge { get; set; }
    //    public string Type { get; set; }
    //    public string EcomGstn { get; set; }
    //    public Double Rate { get; set; }
    //    public Double TaxableValue { get; set; }
    //    public Double CessAmount { get; set; }

    //    public SalesGstrReportBtoBLineItem()
    //    {
    //    }

    //    public SalesGstrReportBtoBLineItem(SaleEntry SaleEntry, SaleDetail SaleDetail, string Gst)
    //    {
    //        // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
    //        this.Name = SaleEntry.CustomerName;
    //        this.Gstn = Gst;
    //        this.Invoice = SaleEntry.RefNumber;
    //        this.Date = SaleEntry.SaleDate.Date;
    //        this.value = SaleDetail.Amount;
    //        this.RevCharge = "N";
    //        this.Type = "Regular";
    //        this.EcomGstn = "";
    //        this.Rate = SaleDetail.TaxDetails.First().TaxRate;
    //        this.CessAmount = SaleDetail.TaxDetails.Sum(x => x.Amount);
    //        this.TaxableValue = SaleDetail.Amount - SaleDetail.TaxDetails.First().Amount;
    //    }

    //    public SalesGstrReportBtoBLineItem(SaleDetail SaleDetail)
    //    {
    //        this.Type = "OE";
    //        this.EcomGstn = "";
    //        this.Rate = SaleDetail.TaxDetails.First().TaxRate;
    //        this.CessAmount = SaleDetail.TaxDetails.Sum(x => x.Amount);
    //        this.TaxableValue = SaleDetail.Amount - SaleDetail.TaxDetails.First().Amount;
    //    }
    //}

    /*Business to Customer Large*/
    public class SalesGstrReportBtoCL : SalesGstrReport
    {
        SalesGstrReportType Type = SalesGstrReportType.BUSINESS_TO_CUST_LARGE;
        IList<SalesGstrReportBtoBLineItem> _LineItems = new List<SalesGstrReportBtoBLineItem>();

        public IList<SalesGstrReportBtoBLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return SalesGstrReportNames.getSalesReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesGstrReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }
        public int InvoiceCount = 0;

        public override void GenerateReport()
        {
            List<SaleEntry> SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntry = (from Sale in Context.SaleEntry where (Sale.EntryType == Entrytype.SALE && Sale.NetAmount > 250000 && Sale.CompanyId == this.Company.CompanyId && Sale.SaleDate >= this.FromDate && Sale.SaleDate <= this.ToDate) select Sale).OrderByDescending(x => x.SaleDate).ToList();
                }
                if (SaleEntry != null && SaleEntry.Count > 0)
                {
                    InvoiceCount = SaleEntry.Count;
                    foreach (SaleEntry lSaleEntry in SaleEntry)
                    {
                        Customer Customer = null;
                        Supplier Supplier = null;
                        AccountType AccountType = AccountType.CUSTOMER;
                        if (lSaleEntry.SaleDetails.Count == 0)
                        {
                            lSaleEntry.SaleDetails = Context.SaleDetail.Include("TaxDetails").Where(x => x.SaleId == lSaleEntry.Id).OrderBy(x => x.TaxDetails.Sum(s => s.TaxRate)).ToList();
                        }
                        if (lSaleEntry.AccountsId != null)
                        {
                            Customer = CustomerManager.Instance.GetCustomerById((long)lSaleEntry.AccountsId);
                            if (Customer == null)
                            {
                                AccountType = AccountType.SUPPLIER;
                                Supplier = SupplierManager.Instance.GetSupplierById((long)lSaleEntry.AccountsId);
                            }
                        }
                        float TaxAmount = 0;
                        float Amount = 0;
                        float TaxPercent = -1;
                        SaleDetail SaleDetail = new SaleDetail();
                        int i = 0;
                        if (lSaleEntry.AccountsId == null || ((AccountType == AccountType.CUSTOMER && Customer == null || (Customer != null && Customer.CustomerLicenceDetail.Count == 0)))
                                || ((AccountType == AccountType.SUPPLIER && Supplier == null || (Supplier != null && Supplier.SupplierLicenceDetail.Count == 0))))
                        {
                            foreach (SaleDetail Detail in lSaleEntry.SaleDetails)
                            {
                                if (TaxPercent != Detail.TaxDetails.Sum(x => x.TaxRate))
                                {
                                    if (i != 0)
                                    {
                                        SaleDetail.Amount = Amount;
                                        SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                        ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                        ItemLevelSaleTax.Amount = TaxAmount;
                                        ItemLevelSaleTax.TaxRate = TaxPercent;
                                        SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                        SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail,string.Empty);
                                        LineItems.Add(LineItem);
                                    }
                                    SaleDetail = Detail;
                                    TaxPercent = Detail.TaxDetails.Sum(x => x.TaxRate);

                                    TaxAmount = Detail.TaxDetails.Sum(x => x.Amount);
                                    Amount = Detail.Amount;
                                }
                                else
                                {
                                    TaxAmount += Detail.TaxDetails.Sum(x => x.Amount);
                                    Amount += Detail.Amount;
                                }
                                i++;
                                if (i == lSaleEntry.SaleDetails.Count)
                                {
                                    SaleDetail.Amount = Amount;
                                    SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                    ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                    ItemLevelSaleTax.Amount = TaxAmount;
                                    ItemLevelSaleTax.TaxRate = TaxPercent;
                                    SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                    SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail,string.Empty);
                                    LineItems.Add(LineItem);
                                }
                            }
                        }
                        else
                        {
                            i++;
                            if (i == lSaleEntry.SaleDetails.Count)
                            {
                                SaleDetail.Amount = Amount;
                                SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                ItemLevelSaleTax.Amount = TaxAmount;
                                ItemLevelSaleTax.TaxRate = TaxPercent;
                                SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(lSaleEntry, SaleDetail, string.Empty);
                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
            }
        }
    }

    /*Business to Customer small*/
    public class SalesGstrReportBtoCS : SalesGstrReport
    {
        SalesGstrReportType Type = SalesGstrReportType.BUSINESS_TO_CUST_SMALL;
        IList<SalesGstrReportBtoBLineItem> _LineItems = new List<SalesGstrReportBtoBLineItem>();

        public IList<SalesGstrReportBtoBLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return SalesGstrReportNames.getSalesReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = SalesGstrReportNames.getSalesReportName(Type) + " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }

        public override void GenerateReport()
        {
            List<SaleDetail> lSaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    lSaleDetails = Context.SaleDetail.Include("Sale").Include("TaxDetails").Where(x => x.Sale.EntryType == Entrytype.SALE &&x.Sale.CompanyId==this.Company.CompanyId && x.Sale.NetAmount<=250000 && x.Sale.SaleDate >= this.FromDate && x.Sale.SaleDate <= this.ToDate).OrderBy(x => x.TaxDetails.Sum(s => s.TaxRate)).ToList();
                }
                float TaxAmount = 0;
                float Amount = 0;
                float TaxPercent = -1;
                SaleDetail SaleDetail = new SaleDetail();
                int i = 0;
                foreach (SaleDetail Detail in lSaleDetails.OrderBy(x=>x.TaxDetails.Sum(s=>s.TaxRate)))
                {
                    Customer Customer = null;
                    Supplier Supplier = null; 
                    AccountType AccountType = AccountType.CUSTOMER;
                    if (Detail.Sale.AccountsId != null)
                    {
                        Customer = CustomerManager.Instance.GetCustomerById((long)Detail.Sale.AccountsId);
                        if (Customer == null)
                        {
                            AccountType = AccountType.SUPPLIER;
                            Supplier = SupplierManager.Instance.GetSupplierById((long)Detail.Sale.AccountsId);
                        }
                    }
                    if (Detail.Sale.AccountsId==null || (AccountType== AccountType.CUSTOMER && (Customer == null || (Customer != null && Customer.CustomerLicenceDetail.Count == 0)))
                            || (AccountType == AccountType.SUPPLIER && (Supplier == null || (Supplier != null && Supplier.SupplierLicenceDetail.Count == 0))))
                    {

                        if (TaxPercent != Detail.TaxDetails.Sum(x => x.TaxRate))
                        {
                            if (i != 0)
                            {
                                SaleDetail.Amount = Amount;
                                SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                                ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                                ItemLevelSaleTax.Amount = TaxAmount;
                                ItemLevelSaleTax.TaxRate = TaxPercent;
                                SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                                SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(SaleDetail);
                                LineItems.Add(LineItem);
                            }
                            SaleDetail = Detail;
                            TaxPercent = Detail.TaxDetails.Sum(x => x.TaxRate);

                            TaxAmount = Detail.TaxDetails.Sum(x => x.Amount);
                            Amount = Detail.Amount;
                        }
                        else
                        {
                            TaxAmount += Detail.TaxDetails.Sum(x => x.Amount);
                            Amount += Detail.Amount;
                        }
                        i++;
                        if (i == lSaleDetails.Count)
                        {
                            SaleDetail.Amount = Amount;
                            SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                            ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                            ItemLevelSaleTax.Amount = TaxAmount;
                            ItemLevelSaleTax.TaxRate = TaxPercent;
                            SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                            SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(SaleDetail);
                            LineItems.Add(LineItem);
                        }
                        
                    }
                    else
                    {
                        i++;
                        if (i == lSaleDetails.Count)
                        {
                            SaleDetail.Amount = Amount;
                            SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                            ItemLevelSaleTaxDetail ItemLevelSaleTax = new ItemLevelSaleTaxDetail();
                            ItemLevelSaleTax.Amount = TaxAmount;
                            ItemLevelSaleTax.TaxRate = TaxPercent;
                            SaleDetail.TaxDetails.Add(ItemLevelSaleTax);

                            SalesGstrReportBtoBLineItem LineItem = new SalesGstrReportBtoBLineItem(SaleDetail);
                            LineItems.Add(LineItem);
                        }
                    }
                }
            }
            }    
        }
    }

