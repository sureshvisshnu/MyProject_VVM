using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.context;
using fa.model.OrderManagement;
using fa.report;
using fa.report.Inventory;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;

namespace FADataAccessLibrary.report.Inventory
{
    public enum StockMovementReportFilterType
    {
        BYDATE, BYITEM, BYLOCATION
    }
    public class RptStockMovement : Report
    {
        public long[] LocationIds { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public string ReportHeader { get; set; }
        public List<StockMovementReportLine> StockMovementByDate = null;
        public List<StockMovementByItemReportLine> StockMovementByItems = null;

        public StockMovementReportFilterType Type { get; set; }

        public override string ReportTitle()
        {
            return "Stock Movement " + (Type == StockMovementReportFilterType.BYDATE ? "By Date" : Type == StockMovementReportFilterType.BYITEM ? "By Item" : "By Location");
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
            return String.Format("Stock Movement {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == StockMovementReportFilterType.BYITEM)
                {
                    IList<StockMovementDetail> StockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.CompanyId == Company.CompanyId && x.StockMovement.Type == InventoryJournalType.STOCK_OUT && x.StockMovement.MovementDate <= ToDate && x.StockMovement.MovementDate >= FromDate).ToList();
                    if (StockMovementDetails != null && StockMovementDetails.Count > 0)
                    {
                        StockMovementByItems = new List<StockMovementByItemReportLine>();
                        foreach (StockMovementDetail detail in StockMovementDetails)
                        {
                            StockMovementByItemReportLine stockMovementByItemReportLine = new StockMovementByItemReportLine(detail);
                            StockMovementByItems.Add(stockMovementByItemReportLine);
                        }
                    }
                }
                else
                {
                    IList<StockMovementOut> StockMovement = null;
                    if (Type == StockMovementReportFilterType.BYDATE)
                    {
                        StockMovement = Context.StockMovementOut.Include("InventoryStockLocation").Include("InventoryLocationTo").Include("StockMovementDetails").Where(x => x.CompanyId == Company.CompanyId && x.Type == InventoryJournalType.STOCK_OUT && x.MovementDate <= ToDate && x.MovementDate >= FromDate).ToList();
                    }
                    else if (Type == StockMovementReportFilterType.BYLOCATION)
                    {
                        StockMovement = Context.StockMovementOut.Include("InventoryStockLocation").Include("InventoryLocationTo").Include("StockMovementDetails").Where(x => x.CompanyId == Company.CompanyId && x.Type == InventoryJournalType.STOCK_OUT && x.MovementDate <= ToDate && x.MovementDate >= FromDate && LocationIds.Contains(x.InventoryStockLocationId)).ToList();
                    }
                    if (StockMovement != null && StockMovement.Count > 0)
                    {
                        StockMovementByDate = new List<StockMovementReportLine>();
                        foreach (StockMovementOut MovementOut in StockMovement)
                        {
                            long Id = long.Parse(MovementOut.CreatedBy.Split(':').Last().Trim(']'));
                            StockMovementReportLine StockMovementReportLine = new StockMovementReportLine(MovementOut, Context.Users.Find(Id).Name);
                            StockMovementByDate.Add(StockMovementReportLine);
                        }
                    }
                }
            }
        }
    }
    public class StockMovementByItemReportLine
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }

        public StockMovementByItemReportLine()
        {
        }
        public StockMovementByItemReportLine(StockMovementDetail detail)
        {
            this.Id = detail.Id;
            this.Code = detail.MaterialId;
            this.Name = detail.Product.Name;
            this.BatchNo = detail.BatchNo;
            this.ExpDate = detail.ExpDate;
            this.Quantity = detail.Quantity;
            this.Free = detail.FreeQuantity;
        }
    }
    public class StockMovementReportLine
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public String Source { get; set; }
        public String Destination { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public string MovedBy { get; set; }

        public StockMovementReportLine()
        {
        }
        public StockMovementReportLine(StockMovementOut Movement, string MovedBy)
        {
            this.Id = Movement.Id;
            this.Date = Movement.MovementDate;
            this.Reference = Movement.RefNumber;
            this.Source = Movement.InventoryStockLocation.Name;
            this.Destination = Movement.InventoryLocationTo.Name;
            this.Qty = Movement.StockMovementDetails.Sum(x => x.Quantity);
            this.Free = Movement.StockMovementDetails.Sum(x => x.FreeQuantity);
            this.MovedBy = MovedBy;
        }
    }
}
