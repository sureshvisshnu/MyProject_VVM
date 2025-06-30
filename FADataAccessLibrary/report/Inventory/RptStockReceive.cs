using fa.context;
using fa.model.OrderManagement;
using fa.report;
using fa.report.Inventory;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.report.Inventory
{
    public enum RecevieType
    {
        BYDATE, BYITEM, BYLOCATION
    }
    public class RptStockReceive : Report
    {
        public long[] LocationIds { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public string ReportHeader { get; set; }
        public List<StockRecevieReportLine> StockRecevie = null;
        public List<StockRecevieByItemReportLine> StockRecevieByItems = null;

        public RecevieType Type { get; set; }

        public override string ReportTitle()
        {
            return "Stock Receive " + (Type == RecevieType.BYDATE ? "By Date" : Type == RecevieType.BYITEM ? "By Item" : "By Location"); ;
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
            return String.Format("Stock Receive {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == RecevieType.BYITEM)
                {
                    IList<StockMovementDetail> StockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovement.Type == InventoryJournalType.STOCK_IN && x.StockMovement.MovementDate <= ToDate && x.StockMovement.MovementDate >= FromDate).ToList();
                    if (StockMovementDetails != null && StockMovementDetails.Count > 0)
                    {
                        StockRecevieByItems = new List<StockRecevieByItemReportLine>();
                        foreach (StockMovementDetail detail in StockMovementDetails)
                        {
                            StockRecevieByItemReportLine StockRecevieByItemReportLine = new StockRecevieByItemReportLine(detail);
                            StockRecevieByItems.Add(StockRecevieByItemReportLine);
                        }
                    }
                }
                else
                {
                    IList<StockMovementIn> StockMovement = null;
                    if (Type == RecevieType.BYLOCATION)
                    {
                        StockMovement = Context.StockMovementIn.Include("InventoryLocationFrom").Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.MovementDate <= ToDate && x.MovementDate >= FromDate && LocationIds.Contains(x.InventoryLocationFromId)).ToList();
                    }
                    else if (Type == RecevieType.BYDATE)
                    {
                        StockMovement = Context.StockMovementIn.Include("InventoryLocationFrom").Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.MovementDate <= ToDate && x.MovementDate >= FromDate).ToList();
                    }
                    if (StockMovement != null && StockMovement.Count > 0)
                    {
                        StockRecevie = new List<StockRecevieReportLine>();
                        foreach (StockMovementIn Recevie in StockMovement)
                        {
                            long Id = long.Parse(Recevie.CreatedBy.Split(':').Last().Trim(']'));
                            StockRecevieReportLine StockRecevieReportLine = new StockRecevieReportLine( Recevie, Context.Users.Find(Id).Name);
                            StockRecevie.Add(StockRecevieReportLine);
                        }
                    }
                }
            }
        }
    }

    public class StockRecevieReportLine
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public String RecevieLoaction { get; set; }
        public String ToLocation { get; set; }
        public double Qty { get; set; }
        public double Free { get; set; }
        public string CreatedBy { get; set; }

        public StockRecevieReportLine()
        {
        }
        public StockRecevieReportLine( StockMovementIn Recevie, string CreatedBy)
        {
            this.Id = Recevie.Id;
            this.Date = Recevie.MovementDate;
            this.CreatedBy = CreatedBy;
            this.Reference = Recevie.RefNumber;
            this.RecevieLoaction = Recevie.InventoryLocationFrom.Name;
            this.ToLocation = Recevie.InventoryStockLocation.Name;
            this.Qty = Recevie.StockMovementDetails.Sum(x => x.Quantity);
            this.Free = Recevie.StockMovementDetails.Sum(x=> x.FreeQuantity);
        }
    }
    public class StockRecevieByItemReportLine
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }

        public StockRecevieByItemReportLine()
        {
        }
        public StockRecevieByItemReportLine(StockMovementDetail detail)
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
