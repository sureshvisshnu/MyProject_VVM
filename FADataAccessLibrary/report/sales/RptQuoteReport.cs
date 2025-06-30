using fa.api.catalog;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report;
using fa.report.sales;
using FaData.Utils;
using FADataAccessLibrary.report.Hms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.report.sales.SalesReportByProductFamily;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FADataAccessLibrary.report.sales
{
    public enum QouteType
    {
        BYINVOICE, BYCUSTOMER, BYCATEGORY, BYPFMAILY, BYITEM
    }
    public class RptQuoteReport : Report
    {
        public long[] CustomerIds { get; set; }
        public string Customer;
        public bool IsAllCustomer { get; set; }
        public Company Company { get; set; }
        public long[] CategoryIds { get; set; }
        public string Category;
        public bool IsAllCategory { get; set; }
        public long[] PfamilyIds { get; set; }
        public string[] ProductFamilyNames { get; set; }
        public bool IsAllPfamily { get; set; }
        public QouteType Type { get; set; }
        public string ReportHeader { get; set; }

        public List<QuoteReportByInvoiceLineItem> QuoteReportByInvoiceLineItems = null;
        public List<QuoteReportByCustomerLineItem> QuoteReportByCustomerLineItems = null;
        public IList<QouteReportByCategoryLineItem> _LineItems;
        public IList<QouteReportByProductFamilyLineItem> _LineItemsPfamily;
        public IList<QouteReportByItemLineItem> _LineItemsItem;
        public override string ReportTitle()
        {
            return "Quote Report " + (Type == QouteType.BYINVOICE ? "By Invoice" : Type == QouteType.BYCATEGORY ? "By Category" : Type == QouteType.BYCUSTOMER ? "By Customer" : Type == QouteType.BYPFMAILY ? "By Product Family" :"By Item");
        }
        public string ReportSubTitle()
        {
            return String.Format("From {0} To {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Quote Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public IList<QouteReportByCategoryLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public IList<QouteReportByProductFamilyLineItem> LineItemsPfamily
        {
            get
            {
                return _LineItemsPfamily;
            }
        }
        public IList<QouteReportByItemLineItem> LineItemsItems
        {
            get
            {
                return _LineItemsItem;
            }
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == QouteType.BYINVOICE)
                {
                    IList<SaleEntry> QuoteEntries = SalesManager.Instance.GetSaleEntryWithQuoteByCompanyId(Company.CompanyId).Where(x => x.SaleDate >= this.FromDate && x.SaleDate <= this.ToDate).ToList();
                    if (QuoteEntries != null && QuoteEntries.Count > 0)
                    {
                        QuoteReportByInvoiceLineItems = new List<QuoteReportByInvoiceLineItem>();
                        foreach(SaleEntry details in QuoteEntries)
                        {
                            QuoteReportByInvoiceLineItem lineItem = new QuoteReportByInvoiceLineItem(details);
                            QuoteReportByInvoiceLineItems.Add(lineItem);
                        }
                    }
                }
                else if (Type == QouteType.BYCUSTOMER)
                {
                    List<long> CustomerId = CustomerIds.ToList();
                    IList<SaleEntry> QuoteEntries = Context.SaleEntry.Where(x => x.EntryType == Entrytype.QUOTE && x.Company == Company && x.SaleDate >= this.FromDate && x.SaleDate <= this.ToDate && CustomerId.Contains(x.Account.Id)).ToList();
                    if (QuoteEntries != null && QuoteEntries.Count > 0)
                    {
                        QuoteReportByCustomerLineItems = new List<QuoteReportByCustomerLineItem>();
                        foreach (SaleEntry details in QuoteEntries)
                        {
                            QuoteReportByCustomerLineItem lineItem = new QuoteReportByCustomerLineItem(details);
                            QuoteReportByCustomerLineItems.Add(lineItem);
                        }
                    }
                }
                else if (Type == QouteType.BYCATEGORY)
                {
                    List<SaleDetail> Sales = null;

                    if (CategoryIds != null && CategoryIds.ToList().Count > 0)
                    {
                        var ProductAndCatalogInfo = (from lCatalogItem in Context.CatalogItems
                                                     join pFamily in Context.CatalogItems on lCatalogItem.ParentId equals pFamily.Id
                                                     where CategoryIds.Contains(pFamily.ParentId ?? 0)
                                                        && lCatalogItem.Type == fa.model.Catalog.CatalogItemType.PRODUCT
                                                     select new { ProductId = lCatalogItem.Id, CatalogName = pFamily.Parent.Name })
                                                     .ToList();

                        var ProductIds = ProductAndCatalogInfo.Select(info => info.ProductId).ToList();
                        var CatalogNames = ProductAndCatalogInfo.ToDictionary(info => info.ProductId, info => info.CatalogName);

                        Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails")
                                 where ProductIds.Contains((long)SaleDetails.ProductId)
                                    && (SaleDetails.Sale.EntryType == Entrytype.QUOTE
                                        && SaleDetails.CompanyId == this.Company.CompanyId
                                        && SaleDetails.Sale.SaleDate >= this.FromDate
                                        && SaleDetails.Sale.SaleDate <= this.ToDate)
                                 select SaleDetails).ToList();

                        if (Sales != null && Sales.Count > 0)
                        {
                            _LineItems = new List<QouteReportByCategoryLineItem>();

                            foreach (SaleDetail Sale in Sales)
                            {
                                if (CatalogNames.TryGetValue((long)Sale.ProductId, out var catalogName))
                                {
                                    QouteReportByCategoryLineItem LineItem = new QouteReportByCategoryLineItem(Sale, catalogName);
                                    QouteReportByCategoryLineItem OldLineItem = null;

                                    if (_LineItems == null)
                                    {
                                        _LineItems = new List<QouteReportByCategoryLineItem>();
                                    }

                                    OldLineItem = _LineItems.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);

                                    if (OldLineItem != null)
                                    {
                                        LineItem.Qty += OldLineItem.Qty;
                                        LineItem.Free += OldLineItem.Free;
                                        LineItem.SubTotal += OldLineItem.SubTotal;
                                        LineItem.Tax += OldLineItem.Tax;
                                        LineItem.Total += OldLineItem.Total;

                                        _LineItems.Remove(OldLineItem);
                                    }
                                    _LineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                else if (Type == QouteType.BYPFMAILY)
                {
                    List<SaleDetail> Sales = null;
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        if (ProductFamilyNames.ToList().Count > 0)
                        {
                            var ProductIds = (from product in Context.Products
                                              join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                                              where product.CompanyId == Company.CompanyId
                                                    && product.Type == CatalogItemType.PRODUCT
                                                    && ProductFamilyNames.Contains(productFamily.Name)
                                              select product.Id).ToList();
                            Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") where ProductIds.Contains((long)SaleDetails.ProductId) where (SaleDetails.Sale.EntryType == Entrytype.QUOTE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                        }
                        else
                        {
                            return;
                        }
                    }
                    _LineItemsPfamily = new List<QouteReportByProductFamilyLineItem>();
                    if (Sales != null && Sales.Count > 0)
                    {
                        foreach (SaleDetail Sale in Sales)
                        {
                            ProductFamily productFamily = CatalogProductFamilyManager.Instance.GetProductFamilyInfoById(Sale.Product.ProductFamilyId, Company.CompanyId);
                            QouteReportByProductFamilyLineItem LineItem = new QouteReportByProductFamilyLineItem(Sale, productFamily);
                            QouteReportByProductFamilyLineItem OldLineItem = null;
                            OldLineItem = new QouteReportByProductFamilyLineItem();
                            if (Sale.isBatch)
                            {
                                OldLineItem = LineItemsPfamily.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);
                            }
                            else
                            {
                                OldLineItem = LineItemsPfamily.FirstOrDefault(x => x.ProductId == Sale.ProductId);
                            }
                            if (OldLineItem != null)
                            {
                                LineItem.Qty += OldLineItem.Qty;
                                LineItem.Free += OldLineItem.Free;
                                LineItem.SubTotal += OldLineItem.SubTotal;
                                LineItem.Tax += OldLineItem.Tax;
                                LineItem.Total += OldLineItem.Total;

                                LineItemsPfamily.Remove(OldLineItem);
                            }
                            LineItemsPfamily.Add(LineItem);
                        }
                    }
                }
                else
                {
                    List<SaleDetail> Sales = null;
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        Sales = (from SaleDetails in Context.SaleDetail.Include("Sale").Include("Product").Include("TaxDetails") /*where ItemIds.Contains((long)SaleDetails.ProductId)*/ where (SaleDetails.Sale.EntryType == Entrytype.QUOTE && SaleDetails.CompanyId == this.Company.CompanyId && SaleDetails.Sale.SaleDate >= this.FromDate && SaleDetails.Sale.SaleDate <= this.ToDate) select SaleDetails).ToList();
                    }
                    _LineItemsItem = new List<QouteReportByItemLineItem>();
                    if (Sales != null && Sales.Count > 0)
                    {
                        foreach (SaleDetail Sale in Sales)
                        {
                            QouteReportByItemLineItem LineItem = new QouteReportByItemLineItem(Sale);
                            QouteReportByItemLineItem OldLineItem = null;
                            if (Sale.isBatch)
                            {
                                OldLineItem = LineItemsItems.FirstOrDefault(x => x.ProductId == Sale.ProductId && x.BatchNumber == Sale.BatchNo);
                            }
                            else
                            {
                                OldLineItem = LineItemsItems.FirstOrDefault(x => x.ProductId == Sale.ProductId);
                            }
                            if (OldLineItem != null)
                            {
                                LineItem.Qty += OldLineItem.Qty;
                                LineItem.Free += OldLineItem.Free;
                                LineItem.SubTotal += OldLineItem.SubTotal;
                                LineItem.Tax += OldLineItem.Tax;
                                LineItem.Total += OldLineItem.Total;

                                LineItemsItems.Remove(OldLineItem);
                            }
                            LineItemsItems.Add(LineItem);
                        }
                    }
                }
            }
        }
    }
    public class QuoteReportByInvoiceLineItem
    {
        public String InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public String CustomerName { get; set; }
        public String CustomerAddress { get; set; }
        public Double InvoiceAmount { get; set; }
        public Double InvoiceTax { get; set; }
        public Double InvoiceNetAmount { get; set; }
        public String InvoiceType { get; set; }

        public QuoteReportByInvoiceLineItem()
        {
            
        }

        public QuoteReportByInvoiceLineItem(SaleEntry details)
        {
            this.InvoiceNetAmount = details.NetAmount;
            this.InvoiceTax = details.TaxAmount;
            this.InvoiceAmount = details.TotalAmount;
            this.InvoiceType = details.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.CustomerName = details.CustomerName;
            this.CustomerAddress = details.CustomerAddress.Replace("\r", "").Replace("\n", "");
            this.InvoiceNumber = details.RefNumber;
            this.InvoiceDate = details.SaleDate;
        }
    }
    public class QuoteReportByCustomerLineItem
    {
        public String CustomerInvoiceNumber { get; set; }
        public DateTime CustomerInvoiceDate { get; set; }
        public Double CustomerInvoiceTax { get; set; }
        public Double CustomerInvoiceDiscount { get; set; }
        public Double CustomerInvoiceAmount { get; set; }
        public String InvoiceType { get; set; }
        public String CustomerName { get; set; }

        public QuoteReportByCustomerLineItem()
        {
        }

        public QuoteReportByCustomerLineItem(SaleEntry details)
        {
            this.CustomerInvoiceNumber = details.RefNumber;
            this.CustomerInvoiceDate = details.SaleDate;
            this.CustomerInvoiceTax = details.TaxAmount;
            this.CustomerInvoiceDiscount = details.DisAmount;
            this.InvoiceType = details.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.CustomerInvoiceAmount = details.TotalAmount;
            this.CustomerName = details.CustomerName;
        }
    }
    public class QouteReportByCategoryLineItem
    {
        public String Item { get; set; }
        public String ItemName { get; set; }
        public String CustomerAddress { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public long ProductId { get; set; }
        public String Category { get; set; }


        public QouteReportByCategoryLineItem()
        {
        }

        public QouteReportByCategoryLineItem(SaleDetail SaleDetail , string catalogName)
        {
            this.Item = SaleDetail.Product.MaterialId;
            this.ItemName = SaleDetail.Product.Name;
            this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
            this.Qty = SaleDetail.Quantity;
            this.Free = SaleDetail.FreeQuantity;
            this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
            this.ExpDate = SaleDetail.ExpDate;
            this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = SaleDetail.Amount;
            this.ProductId = (long)SaleDetail.ProductId;
            this.Category = catalogName;
        }
    }
    public class QouteReportByProductFamilyLineItem
    {
        public String Item { get; set; }
        public String ItemName { get; set; }
        public String CustomerAddress { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public String ProductFamily { get; set; }
        public long ProductId { get; set; }

        public QouteReportByProductFamilyLineItem()
        {
        }
        public QouteReportByProductFamilyLineItem(SaleDetail SaleDetail, ProductFamily productFamily)
        {
            this.Item = SaleDetail.Product.MaterialId;
            this.ItemName = SaleDetail.Product.Name;
            this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
            this.Qty = SaleDetail.Quantity;
            this.Free = SaleDetail.FreeQuantity;
            this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
            this.ExpDate = SaleDetail.ExpDate;
            this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = SaleDetail.Amount;
            this.ProductFamily = productFamily.ToString();
            this.ProductId = (long)SaleDetail.ProductId;
        }
    }
    public class QouteReportByItemLineItem
    {
        public String Item { get; set; }
        public String ItemName { get; set; }
        public String CustomerAddress { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public String BatchNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public Double SubTotal { get; set; }
        public Double Tax { get; set; }
        public Double Total { get; set; }
        public long ProductId { get; set; }


        public QouteReportByItemLineItem()
        {
        }

        public QouteReportByItemLineItem(SaleDetail SaleDetail)
        {

            this.Item = SaleDetail.Product.MaterialId;
            this.ItemName = SaleDetail.Product.Name;
            this.CustomerAddress = SaleDetail.Sale.CustomerAddress;
            this.Qty = SaleDetail.Quantity;
            this.Free = SaleDetail.FreeQuantity;
            this.BatchNumber = SaleDetail.isBatch ? SaleDetail.BatchNo : "";
            this.ExpDate = SaleDetail.ExpDate;
            this.SubTotal = (SaleDetail.Amount - SaleDetail.TaxDetails.Sum(x => x.Amount));
            this.Tax = SaleDetail.TaxDetails.Sum(x => x.Amount);
            this.Total = SaleDetail.Amount;
            this.ProductId = (long)SaleDetail.ProductId;
        }
    }
}

