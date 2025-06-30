using fa.context;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using fa.report.catalog;
using fa.report.sales;
using FaData.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.report.sales
{
    public class DeliveryReport : Report
    {

        public Entrytype entrytype;
        public long CompanyId;
        public DateTime DeliveryFromDate;
        public DateTime DeliveryToDate;
        public DateTime CurrentDate;

        public string ReportDate()
        {
            return String.Format("{0} to {1}", DateUtils.FormatDate(this.DeliveryFromDate, Company.DateFormat), DateUtils.FormatDate(this.DeliveryToDate, Company.DateFormat));
        }

        public override string ReportTitle()
        {
            return String.Format("Delivery Report");
        }
        public string ReportSubTitle()
        {
            string TransactionDate = this.CurrentDate.ToString(Company.DateFormat);
            char Separator = TransactionDate.Contains("-") ? '-' : TransactionDate.Contains("/") ? '/' : '.';
            string[] Date = TransactionDate.Split(Separator);
            return String.Format("Date : {0}-{1}-{2}", Date[0], Date[1], Date[2]);
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Delivery Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        IList<DeliveryReportLineItem> _LineItems = new List<DeliveryReportLineItem>();

        public IList<DeliveryReportLineItem> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        
        public override void GenerateReport()
        {
            List<SaleEntry> Sales = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    Sales = (from saleEntry in Context.SaleEntry
                             where saleEntry.EntryType == entrytype &&
                                   saleEntry.CompanyId == CompanyId &&
                                   saleEntry.SaleDate >= DeliveryFromDate &&
                                   saleEntry.SaleDate <= DeliveryToDate
                             orderby saleEntry.hasDelivered ascending
                             select saleEntry)
                            .ToList();
                }
            }

            if (Sales != null && Sales.Count > 0)
            {
                foreach (SaleEntry sale in Sales)
                {
                    DeliveryReportLineItem lineItem = new DeliveryReportLineItem(sale);
                    LineItems.Add(lineItem);
                }
            }
        }
        
    }
    public class DeliveryReportLineItem
    {
        public String CustomerName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Double Amount { get; set; }
        public String PaymentType { get; set; }
        public String Status { get; set; }

        public DeliveryReportLineItem()
        {
        }

        public DeliveryReportLineItem(SaleEntry Sale)
        {
            this.CustomerName = Sale.CustomerName;
            this.DeliveryDate = Sale.SaleDate;
            this.Amount = Sale.TotalAmount;
            this.PaymentType = Sale.SaleMethod == SaleMethod.Credit ? "Credit" : "Cash";
            this.Status = Sale.hasDelivered == true ? "Delivered" : "Not Delivered";
        }
    }
}
