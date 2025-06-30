using fa.context;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using Microsoft.EntityFrameworkCore;

namespace Fa.report.Purchase
{
    public enum PurchaseGstrReportType
    {
        BUSINESS_TO_BUSINESS = 1
    }

    public class PurchaseGstrReportNames
    {
        static string[] ReportNames = { "Purchase GSTR Report" };

        public static string getPurchaseReportName(PurchaseGstrReportType PurchaseGstrReport)
        {
            return ReportNames[(int)PurchaseGstrReport - 1];
        }
    }
    public abstract class PurchaseGstrReport : Report
    {
    }
    /*Business to Business*/
    public class PurchaseGstrReportBtoB : PurchaseGstrReport
    {
        PurchaseGstrReportType Type = PurchaseGstrReportType.BUSINESS_TO_BUSINESS;
        IList<PurchaseGstrReportBtoBLineItem> _LineItems = new List<PurchaseGstrReportBtoBLineItem>();

        public IList<PurchaseGstrReportBtoBLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return PurchaseGstrReportNames.getPurchaseReportName(Type);
        }
        public override string ReportTitle()
        {
            string reportTitle = " For the period of " + this.FromDate.ToString(this.Company.DateFormat) + " - " + this.ToDate.ToString(this.Company.DateFormat);
            return reportTitle;
        }
        public int InvoiceCount = 0;
        public int ReceipientCount = 0;

        public override void GenerateReport()
        {
            List<PurchaseEntry> PurchaseEntries = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    PurchaseEntries = (from Purchase in Context.PurchaseEntry
                                       where (Purchase.CompanyId == this.Company.CompanyId && Purchase.RefDate >= this.FromDate && Purchase.RefDate <= this.ToDate)
                                       select Purchase).OrderByDescending(x => x.RefDate).ToList();
                }

                if (PurchaseEntries != null && PurchaseEntries.Count > 0)
                {
                    InvoiceCount = PurchaseEntries.Count;
                    ReceipientCount = PurchaseEntries.Select(x => x.AccountId).Distinct().ToList().Count;

                    foreach (PurchaseEntry lPurchaseEntry in PurchaseEntries)
                    {
                        if (lPurchaseEntry.PurchaseDetails.Count == 0)
                        {
                            lPurchaseEntry.PurchaseDetails = Context.PurchaseDetails.Include("TaxDetails").Where(x => x.PurchaseEntryId == lPurchaseEntry.Id).OrderBy(x => x.TaxDetails.Sum(s => s.TaxRate)).ToList();
                        }

                        float TaxAmount = 0;
                        float Amount = 0;
                        float TaxPercent = -1;
                        string Gstn = string.Empty;

                        if (lPurchaseEntry.AccountId != 0)
                        {
                            Customer Customer = Context.Customers.Include("CustomerLicenceDetail").FirstOrDefault(x => x.Id == lPurchaseEntry.AccountId);
                            if (Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                            {
                                Gstn = Customer.CustomerLicenceDetail.First().Value;
                            }
                            else
                            {
                                Supplier Supplier = Context.Suppliers.Include("SupplierLicenceDetail").FirstOrDefault(x => x.Id == lPurchaseEntry.AccountId);
                                if (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0)
                                {
                                    Gstn = Supplier.SupplierLicenceDetail.First().Value;
                                }
                            }

                            PurchaseDetails PurchaseDetail = null;
                            int i = 0;
            
                            if (lPurchaseEntry.PurchaseInvNumber != null)
                            {
                                foreach (PurchaseDetails Detail in lPurchaseEntry.PurchaseDetails)
                                {
                                    if (TaxPercent != Detail.TaxDetails.Sum(x => x.TaxRate))
                                    {
                                        if (i != 0)
                                        {
                                            PurchaseDetail.Amount = Amount;
                                            PurchaseDetail.TaxDetails = new List<LineLevelPurchaseTaxDetail>();
                                            LineLevelPurchaseTaxDetail ItemLevelSaleTax = new LineLevelPurchaseTaxDetail();
                                            ItemLevelSaleTax.Amount = TaxAmount;
                                            ItemLevelSaleTax.TaxRate = TaxPercent;
                                            PurchaseDetail.TaxDetails.Add(ItemLevelSaleTax);

                                            PurchaseGstrReportBtoBLineItem LineItem = new PurchaseGstrReportBtoBLineItem(lPurchaseEntry, PurchaseDetail, Gstn);
                                            LineItems.Add(LineItem);
                                        }
                                        PurchaseDetail = Detail;
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
                                    if (i == lPurchaseEntry.PurchaseDetails.Count)
                                    {
                                        PurchaseDetail.Amount = Amount;
                                        PurchaseDetail.TaxDetails = new List<LineLevelPurchaseTaxDetail>();
                                        LineLevelPurchaseTaxDetail ItemLevelSaleTax = new LineLevelPurchaseTaxDetail();
                                        ItemLevelSaleTax.Amount = TaxAmount;
                                        ItemLevelSaleTax.TaxRate = TaxPercent;
                                        PurchaseDetail.TaxDetails.Add(ItemLevelSaleTax);

                                        PurchaseGstrReportBtoBLineItem LineItem = new PurchaseGstrReportBtoBLineItem(lPurchaseEntry, PurchaseDetail, Gstn);
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


    public class PurchaseGstrReportBtoBLineItem
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

        public PurchaseGstrReportBtoBLineItem()
        {
        }

        public PurchaseGstrReportBtoBLineItem(PurchaseEntry PurchaseEntry, PurchaseDetails PurchaseDetails,string Gstn)
        {
            this.Name = PurchaseEntry.SupplierName;
            this.Gstn = Gstn;
            this.Invoice = PurchaseEntry.PurchaseInvNumber;
            this.Date = PurchaseEntry.PurchaseInvDate.Date;
            this.value = PurchaseDetails.Amount;
            this.RevCharge = "N";
            this.Type = "Regular";
            this.EcomGstn = "";
            this.Rate = PurchaseDetails.TaxDetails.First().TaxRate;
            this.CessAmount = PurchaseDetails.TaxDetails.Sum(x => x.Amount);
            this.TaxableValue = PurchaseDetails.Amount - PurchaseDetails.TaxDetails.First().Amount;
        }
        
    }
}
