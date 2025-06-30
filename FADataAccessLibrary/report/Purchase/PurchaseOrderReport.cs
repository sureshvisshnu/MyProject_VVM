using fa.api.catalog;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.report;
using FaData.Utils;
using FADataAccessLibrary.report.sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FADataAccessLibrary.report.Purchase.PurchaseOrderReport;

namespace FADataAccessLibrary.report.Purchase
{
    public enum PurchaseOrderType
    {
        BYINVOICE, BYVENDOR, BYCATEGORY, BYPFMAILY, BYITEM
    }
    public class PurchaseOrderReport : Report
    {
        public PurchaseOrderType Type { get; set; }
        public long?[] VendorsIds { get; set; }
        public long[] CategoryIds { get; set; }
        public long[] PfamilyIds { get; set; }
        public string[] ProductFamilyNames { get; set; }

        public List<PurchaseOrderReportByInvoiceVendorLineItem> PurchaseOrderReportByInvoiceVendorLineItems = null;
        public List<PurchaseOrderReportLineItem> PurchaseOrderReportLineItems = null;

        public override string ReportTitle()
        {
            return "Purchase Order Report " + (Type == PurchaseOrderType.BYINVOICE ? "By Invoice" : Type == PurchaseOrderType.BYCATEGORY ? "By Category" : Type == PurchaseOrderType.BYVENDOR ? "By Vendor" : Type == PurchaseOrderType.BYPFMAILY ? "By Product Family" : "By Item");
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
            return String.Format("Purchase Order {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == PurchaseOrderType.BYINVOICE)
                {
                    IList<PurchaseEntry> purchaseEntries = PurchaseEntryManager.Instance.ListPurchaseOrdersById(Company.CompanyId, this.FromDate, this.ToDate);
                    if (purchaseEntries != null && purchaseEntries.Count > 0)
                    {
                        PurchaseOrderReportByInvoiceVendorLineItems = new List<PurchaseOrderReportByInvoiceVendorLineItem>();
                        foreach (PurchaseEntry lPurchaseEntry in purchaseEntries)
                        {
                            PurchaseOrderReportByInvoiceVendorLineItem lineItem = new PurchaseOrderReportByInvoiceVendorLineItem(lPurchaseEntry, PurchaseOrderType.BYINVOICE);
                            PurchaseOrderReportByInvoiceVendorLineItems.Add(lineItem);
                        }
                    }
                }
                else if (Type == PurchaseOrderType.BYCATEGORY)
                {
                    if (CategoryIds != null && CategoryIds.ToList().Count > 0)
                    {
                        IList<PurchaseDetails> details = PurchaseEntryManager.Instance.ListPurchaseDetailsByCompanyId(Company.CompanyId, this.FromDate, this.ToDate);
                        PurchaseOrderReportLineItems = new List<PurchaseOrderReportLineItem>();
                        PurchaseOrderReportLineItem OldLineItem = new PurchaseOrderReportLineItem();

                        foreach (PurchaseDetails detail in details)
                        {
                            bool isCategorisedProduct = false;
                            if (detail.Product?.Parent?.Parent != null)
                            {
                                isCategorisedProduct = Context.CatalogItems.Any(x => x.Id == detail.ProductId && CategoryIds.Contains(detail.Product.Parent.Parent.Id));
                                if (isCategorisedProduct)
                                {
                                    PurchaseOrderReportLineItem lineItem = new PurchaseOrderReportLineItem(detail, PurchaseOrderType.BYCATEGORY);

                                    OldLineItem = PurchaseOrderReportLineItems.FirstOrDefault(x => x.ItemCode == detail.MaterialId && x.ItemName == detail.Product.Name);
                                    if (OldLineItem != null && OldLineItem.Price == lineItem.Price)
                                    {
                                        lineItem.Qty += OldLineItem.Qty;
                                        lineItem.Amount += OldLineItem.Amount;
                                        PurchaseOrderReportLineItems.Remove(OldLineItem);
                                    }
                                    PurchaseOrderReportLineItems.Add(lineItem);
                                }
                            }
                        }
                    }
                }
                else if (Type == PurchaseOrderType.BYPFMAILY)
                {
                    if (PfamilyIds != null && PfamilyIds.ToList().Count > 0)
                    {
                        IList<PurchaseDetails> details = PurchaseEntryManager.Instance.ListPurchaseDetailsByCompanyId(Company.CompanyId, this.FromDate, this.ToDate);
                        PurchaseOrderReportLineItems = new List<PurchaseOrderReportLineItem>();
                        PurchaseOrderReportLineItem OldLineItem = new PurchaseOrderReportLineItem();

                        foreach (PurchaseDetails detail in details)
                        {
                            bool isCategorisedProduct = false;
                            if (detail.Product?.Parent != null)
                            {
                                isCategorisedProduct = Context.CatalogItems.Any(x => x.Id == detail.ProductId && PfamilyIds.Contains(detail.Product.Parent.Id));
                                if (isCategorisedProduct)
                                {
                                    PurchaseOrderReportLineItem lineItem = new PurchaseOrderReportLineItem(detail, PurchaseOrderType.BYPFMAILY);

                                    OldLineItem = PurchaseOrderReportLineItems.FirstOrDefault(x => x.ItemCode == detail.MaterialId && x.ItemName == detail.Product.Name);
                                    if (OldLineItem != null && OldLineItem.Price == lineItem.Price)
                                    {
                                        lineItem.Qty += OldLineItem.Qty;
                                        lineItem.Amount += OldLineItem.Amount;
                                        PurchaseOrderReportLineItems.Remove(OldLineItem);
                                    }
                                    PurchaseOrderReportLineItems.Add(lineItem);
                                }
                            }
                        }
                    }
                }
                else if (Type == PurchaseOrderType.BYVENDOR)
                {
                    if (VendorsIds != null && VendorsIds.ToList().Count > 0)
                    {
                        IList<PurchaseEntry> purchaseEntries = PurchaseEntryManager.Instance.ListPurchaseOrdersById(Company.CompanyId, this.FromDate, this.ToDate);
                        IList<PurchaseEntry> lpurchaseEntries = purchaseEntries.Where(x => VendorsIds.Contains(x.AccountId)).ToList();
                        if (lpurchaseEntries != null && lpurchaseEntries.Count > 0)
                        {
                            PurchaseOrderReportByInvoiceVendorLineItems = new List<PurchaseOrderReportByInvoiceVendorLineItem>();
                            foreach (PurchaseEntry lPurchaseEntry in lpurchaseEntries)
                            {
                                PurchaseOrderReportByInvoiceVendorLineItem lineItem = new PurchaseOrderReportByInvoiceVendorLineItem(lPurchaseEntry, PurchaseOrderType.BYVENDOR);
                                PurchaseOrderReportByInvoiceVendorLineItems.Add(lineItem);
                            }
                        }
                    }
                }
                else if (Type == PurchaseOrderType.BYITEM)
                {
                    IList<PurchaseDetails> details = PurchaseEntryManager.Instance.ListPurchaseDetailsByCompanyId(Company.CompanyId, this.FromDate, this.ToDate);
                    if (details != null && details.Count > 0)
                    {
                        PurchaseOrderReportLineItems = new List<PurchaseOrderReportLineItem>();
                        PurchaseOrderReportLineItem OldLineItem = new PurchaseOrderReportLineItem();
                        foreach (PurchaseDetails detail in details)
                        {
                            PurchaseOrderReportLineItem lineItem = new PurchaseOrderReportLineItem(detail, PurchaseOrderType.BYITEM);

                            OldLineItem = PurchaseOrderReportLineItems.FirstOrDefault(x => x.ItemCode == detail.MaterialId && x.ItemName == detail.Product.Name);
                            if (OldLineItem != null && OldLineItem.Price == lineItem.Price)
                            {
                                lineItem.Qty += OldLineItem.Qty;
                                lineItem.Amount += OldLineItem.Amount;
                                PurchaseOrderReportLineItems.Remove(OldLineItem);
                            }
                            PurchaseOrderReportLineItems.Add(lineItem);
                        }
                    }
                }
            }
        }

        public class PurchaseOrderReportByInvoiceVendorLineItem
        {
            public String InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public String SupplierName { get; set; }
            public String SupplierAddress { get; set; }
            public string CreditOrCash { get; set; }
            public Double Qty { get; set; }
            public Double Amount { get; set; }
            public Double CashAmount { get; set; }
            public Double CreditAmount { get; set; }
            public String Id { get; set; }

            public PurchaseOrderReportByInvoiceVendorLineItem() { }

            public PurchaseOrderReportByInvoiceVendorLineItem(PurchaseEntry PurchaseEntry, PurchaseOrderType Type)
            {
                this.InvoiceNumber = PurchaseEntry.RefNumber;
                this.InvoiceDate = PurchaseEntry.RefDate;
                this.SupplierName = PurchaseEntry.SupplierName;
                this.SupplierAddress = PurchaseEntry.SupplierAddress.Replace("\r", "").Replace("\n", "");
                this.CreditOrCash = PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit ? "Credit" : "Cash";
                this.Qty = PurchaseEntry.PurchaseDetails.Sum(x => x.Quantity);
                if (Type == PurchaseOrderType.BYINVOICE)
                {
                    this.Amount = PurchaseEntry.NetAmount;
                }
                else
                {
                    this.CashAmount = PurchaseEntry.PurchaseMethod == PurchaseMethod.Cash ? PurchaseEntry.NetAmount : 0;
                    this.CreditAmount = PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit ? PurchaseEntry.NetAmount : 0;
                    this.Id = PurchaseEntry.Id.ToString();
                }
            }
        }
        public class PurchaseOrderReportLineItem
        {
            public String ItemCode { get; set; }
            public String ItemName { get; set; }
            public String UOM { get; set; }
            public Double Qty { get; set; }
            public Double Price { get; set; }
            public Double Amount { get; set; }
            public String CatName { get; set; }
            public String Id { get; set; }

            public PurchaseOrderReportLineItem() { }
            public PurchaseOrderReportLineItem(PurchaseDetails detail, PurchaseOrderType Type)
            {
                this.ItemCode = detail.MaterialId;
                this.ItemName = detail.Product.Name;
                this.UOM = detail.Product.WholesaleUOM;
                this.Qty = detail.Quantity;
                this.Price = detail.PurchasePrice;
                this.Amount = detail.Amount;
                this.CatName = Type == PurchaseOrderType.BYCATEGORY ? detail.Product.Parent?.Parent?.Name : Type == PurchaseOrderType.BYPFMAILY ? detail.Product.Parent?.Name : "";
                this.Id = detail.Id.ToString();
            }
        }
    }
}
