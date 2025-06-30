using fa.context;
using fa.model.Catalog;
using fa.report;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fa.report.catalog
{
    public abstract class ItemReport : Report
    {
    }

    public class AllItemsReport : ItemReport
    {
        public long[] CategoryIds { get; set; }
        IList<ItemReportLineItems> _LineItems = new List<ItemReportLineItems>();
        public IList<ItemReportLineItems> LineItems
        {
            get
            {
                return _LineItems;
            }
        }

        public override string ReportTitle()
        {
            return "Items Report";
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
            return String.Format("Items Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            List<Product> Items = new List<Product>();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (CategoryIds.ToList().Count > 0)
                {
                    var ProductIds = (from lCatalogItem in Context.CatalogItems
                                      where (((from Pfamily in Context.CatalogItems
                                               where CategoryIds.Contains((long)Pfamily.ParentId)
                                               select Pfamily.Id).ToList()).Contains((long)lCatalogItem.ParentId))
                                      where (lCatalogItem.Type == CatalogItemType.PRODUCT)
                                      select lCatalogItem.Id).ToList();

                    Items = (from Product in Context.Products.Include("Inventorys")
                             where (Product.CompanyId == this.Company.CompanyId)
                             where ProductIds.Contains((long)Product.Id)
                             select Product).ToList();

                    var ProductAndCatalogInfo = (from lCatalogItem in Context.CatalogItems
                                                 join pFamily in Context.CatalogItems on lCatalogItem.ParentId equals pFamily.Id
                                                 where CategoryIds.Contains(pFamily.ParentId ?? 0)
                                                    && lCatalogItem.Type == fa.model.Catalog.CatalogItemType.PRODUCT
                                                 select new { ProductId = lCatalogItem.Id, CatalogName = pFamily.Parent.Name })
                                                         .ToList();

                    var CatalogNames = ProductAndCatalogInfo.ToDictionary(info => info.ProductId, info => info.CatalogName);

                    if (Items != null && Items.Count > 0)
                    {
                        foreach (Product Item in Items)
                        {
                            ItemReportLineItems LineItem = new ItemReportLineItems(Item, CatalogNames[Item.Id]);
                            LineItems.Add(LineItem);
                        }
                    }
                }
                else
                {
                    Items = (from Product in Context.Products.Include("Inventorys")
                             where (Product.CompanyId == this.Company.CompanyId)
                             select Product).ToList();

                    if (Items != null && Items.Count > 0)
                    {
                        foreach (Product Item in Items)
                        {
                            ItemReportLineItems LineItem = new ItemReportLineItems(Item, "");
                            LineItems.Add(LineItem);
                        }
                    }
                }
            }
        }
    }

    public class ItemReportLineItems
    {
        public long Id { get; set; }
        public String MaterialId { get; set; }
        public String Name { get; set; }
        public String Uom { get; set; }
        public String RetailUOM { get; set; }
        public int RetailXfactor { get; set; }
        public int WholeSaleXfactor { get; set; }
        public String WholesaleUOM { get; set; }
        public float ProductPrice { get; set; }
        public float Cost { get; set; }
        public float RetailPrice { get; set; }
        public float wholeSaleprice { get; set; }
        public float MSRP { get; set; }
        public double OpenStock { get; set; }
        public double PurchaseQty { get; set; }
        public double SalesQty { get; set; }
        public double CurrentQty { get; set; }
        public String CategoryName { get; set; }

        public ItemReportLineItems()
        {
        }

        public ItemReportLineItems(Product Item, string catalogName)
        {
            this.Cost = Item.CostPrice;
            this.MaterialId = Item.MaterialId;
            this.MSRP = Item.Msrp;
            this.Name = Item.Name;
            this.ProductPrice = Item.PurchasePrice;
            this.RetailPrice = Item.RetailPrice;
            this.RetailUOM = Item.RetailUOM;
            this.RetailXfactor = Item.RetailXFactor;
            this.Uom = Item.UOM;
            this.wholeSaleprice = Item.WholdSalePrice;
            this.WholesaleUOM = Item.WholesaleUOM;
            this.WholeSaleXfactor = Item.WholesaleXFactor;
            if (Item.Inventorys.Count > 0)
            {
                this.OpenStock = Item.Inventorys.Sum(x => x.OpeningStock);
                this.CurrentQty = ((Item.Inventorys.Sum(x => x.OpeningStock) + Item.Inventorys.Sum(x => x.Purchased)) - Item.Inventorys.Sum(x => x.Sold));
                this.SalesQty = Item.Inventorys.Sum(x => x.Sold);
                this.PurchaseQty = Item.Inventorys.Sum(x => x.Purchased);
            }
            this.CategoryName = catalogName;
        }
    }
}
