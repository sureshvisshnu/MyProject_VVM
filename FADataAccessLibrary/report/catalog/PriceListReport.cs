using fa.context;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
namespace fa.report.catalog
{
    public class PriceReport : Report
    {
        public bool StockItem;
        public List<PriceReportLineItems> LineItems = new List<PriceReportLineItems>();
        public override string ReportTitle()
        {
            return String.Format("Price List Report");
        }
        public string ReportSubTitle()
        {
            string TransactionDate = this.FromDate.ToString(Company.DateFormat);
            char Separator = TransactionDate.Contains("-") ? '-' : TransactionDate.Contains("/") ? '/' : '.';
            string[] Date = TransactionDate.Split(Separator);
            return String.Format("Date : {0}-{1}-{2}", Date[0], Date[1], Date[2]);
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("PriceList Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            List<Product> lProduct = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (StockItem)
                    {
                        lProduct = (from Product in Context.Products
                                    where (Product.CompanyId == this.Company.CompanyId 
                                    && Context.Inventories.Where(x=>x.ProductId==Product.Id).Sum(x=>(x.OpeningStock + x.Purchased + x.In) - (x.Sold + x.Out + x.Damage + x.ToPatient)) >0)
                                    select Product).ToList();
                    }
                    else
                    {
                        lProduct = (from Product in Context.Products
                                    where (Product.CompanyId == this.Company.CompanyId)
                                    select Product).ToList();
                    }
                }
            }
            if (lProduct != null && lProduct.Count > 0)
            {
                foreach (Product Price in lProduct)
                {                   
                    LineItems.Add(new PriceReportLineItems(Price));
                }
            }
        }
    }
    public class PriceReportLineItems
    {
        public String MaterialId { get; set; }
        public String Name { get; set; }
        public String Uom { get; set; }
        public String RetailUOM { get; set; }
        public float RetailPrice { get; set; }
        public String WholesaleUOM { get; set; }
        public float wholeSaleprice { get; set; }
        public float MSRP { get; set; }
        public double OpenStock { get; set; }
        public double CurrentQty { get; set; }
        public PriceReportLineItems()
        {
        }
        public PriceReportLineItems(Product ItemPrice)
        {            
            this.MaterialId = ItemPrice.MaterialId;
            this.MSRP = ItemPrice.Msrp;
            this.Name = ItemPrice.Name;
            this.RetailPrice = ItemPrice.RetailPrice;
            this.RetailUOM = ItemPrice.RetailUOM;
            this.Uom = ItemPrice.UOM;
            this.wholeSaleprice = ItemPrice.WholdSalePrice;
            this.WholesaleUOM = ItemPrice.WholesaleUOM;
        }
    }
}
