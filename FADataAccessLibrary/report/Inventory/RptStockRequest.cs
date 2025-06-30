using fa.context;
using fa.model.OrderManagement;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace fa.report.Inventory
{
    public enum InventoryReportFilterType
    {
        BYDATE, BYITEM, BYLOCATION
    }
    public class RptStockRequest : Report
    {
        public long[] LocationIds { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public string ReportHeader { get; set; }
        public List<StockRequestReportLine> StockRequests=null;
        public List<StockRequestByItemReportLine> StockRequestByItems = null;

        public InventoryReportFilterType Type { get; set; }
    
        public override string ReportTitle()
        {
            return "Stock Request "+(Type== InventoryReportFilterType.BYDATE?"By Date": Type == InventoryReportFilterType.BYITEM?"By Item":"By Location");
        }
        public string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));

        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Stock Request {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == InventoryReportFilterType.BYITEM)
                {
                    IList<StockMovementDetail> StockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x =>x.StockMovement.Type==InventoryJournalType.STOCK_REQUEST && x.StockMovement.MovementDate <= ToDate && x.StockMovement.MovementDate >= FromDate).ToList();
                    if (StockMovementDetails != null && StockMovementDetails.Count > 0)
                    {
                        StockRequestByItems = new List<StockRequestByItemReportLine>();
                        foreach (StockMovementDetail detail in StockMovementDetails)
                        {
                            StockRequestByItemReportLine StockRequestByItemReportLine = new StockRequestByItemReportLine(detail);
                            StockRequestByItems.Add(StockRequestByItemReportLine);
                        }
                    }
                }
                else
                {
                    IList<StockMovementRequest> StockMovement = null;
                    if (Type == InventoryReportFilterType.BYLOCATION)
                    {
                        StockMovement = Context.StockMovementRequest.Include("RequestInventoryLocation").Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.MovementDate <= ToDate && x.MovementDate >= FromDate && LocationIds.Contains(x.RequestInventoryLocationId)).ToList();
                    }
                    else if (Type == InventoryReportFilterType.BYDATE)
                    {
                        StockMovement = Context.StockMovementRequest.Include("RequestInventoryLocation").Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.MovementDate <= ToDate && x.MovementDate >= FromDate).ToList();
                    }
                    if (StockMovement != null && StockMovement.Count > 0)
                    {
                        StockRequests = new List<StockRequestReportLine>();
                        foreach (StockMovementRequest Request in StockMovement)
                        {
                            long Id = long.Parse(Request.CreatedBy.Split(':').Last().Trim(']'));
                            StockRequestReportLine StockRequestReportLine = new StockRequestReportLine(Request, Context.Users.Find(Id).Name);
                            StockRequests.Add(StockRequestReportLine);
                        }
                    }
                }
            }
        }
    }

    public class StockRequestReportLine
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public String RequestLoaction { get; set; }
        public String ToLocation { get; set; }
        public double Qty { get; set; }
        public string CreatedBy { get; set; }

        public StockRequestReportLine()
        {
        }
        public StockRequestReportLine(StockMovementRequest Request,string CreatedBy)
        {
            this.Id = Request.Id;
            this.Date = Request.MovementDate;
            this.CreatedBy = CreatedBy;
            this.Reference = Request.RefNumber;
            this.RequestLoaction = Request.RequestInventoryLocation.Name;
            this.ToLocation = Request.InventoryStockLocation.Name;
            this.Qty = Request.StockMovementDetails.Sum(x=>x.Quantity);
        }
    }
    public class StockRequestByItemReportLine
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }

        public StockRequestByItemReportLine()
        {
        }
        public StockRequestByItemReportLine(StockMovementDetail detail)
        {
            this.Id = detail.Id;
            this.Code = detail.MaterialId;
            this.Name = detail.Product.Name;
            this.Quantity = detail.Quantity;
            this.Free = detail.FreeQuantity;
            this.BatchNo = detail.BatchNo;
            this.ExpDate = detail.ExpDate;
        }
    }
}

