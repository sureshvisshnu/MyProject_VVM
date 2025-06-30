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
    public class RptAdjustmentEntryReport:Report
    {
        public long[] LocationIds { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public string ReportHeader { get; set; }
        public List<StockAdjustReportLine> StockAdjustReportLines = null;
        public List<StockAdjustByItemReportLine> StockAdjustByItemReportLines = null;

        public InventoryReportFilterType Type { get; set; }

        public override string ReportTitle()
        {
            return "Stock Adjustment " + (Type == InventoryReportFilterType.BYDATE ? "By Date" : Type == InventoryReportFilterType.BYITEM ? "By Item" : "By Location");
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
            return String.Format("Stock Adjustment {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == InventoryReportFilterType.BYITEM)
                {
                    IList<StockMovementDetail> StockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.CompanyId == this.Company.CompanyId && x.StockMovement.Type == InventoryJournalType.ADJUSTMENT && x.StockMovement.MovementDate <= ToDate && x.StockMovement.MovementDate >= FromDate).ToList();
                    if (StockMovementDetails != null && StockMovementDetails.Count > 0)
                    {
                        StockAdjustByItemReportLines = new List<StockAdjustByItemReportLine>();
                        foreach (StockMovementDetail detail in StockMovementDetails)
                        {
                            StockAdjustByItemReportLine StockAdjustByItemReportLine = new StockAdjustByItemReportLine(detail);
                            StockAdjustByItemReportLines.Add(StockAdjustByItemReportLine);
                        }
                    }
                }
                else
                {
                    IList<StockMovementAdjustment> StockMovement = null;
                    if (Type == InventoryReportFilterType.BYLOCATION)
                    {
                        StockMovement = Context.StockMovementAdjustment.Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.CompanyId == this.Company.CompanyId && x.MovementDate <= ToDate && x.MovementDate >= FromDate && LocationIds.Contains(x.InventoryStockLocationId)).ToList();
                    }
                    else if (Type == InventoryReportFilterType.BYDATE)
                    {
                        StockMovement = Context.StockMovementAdjustment.Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.CompanyId == this.Company.CompanyId && x.MovementDate <= ToDate && x.MovementDate >= FromDate).ToList();
                    }
                    if (StockMovement != null && StockMovement.Count > 0)
                    {
                        StockAdjustReportLines = new List<StockAdjustReportLine>();
                        foreach (StockMovementAdjustment Adjustment in StockMovement)
                        {
                            long Id = long.Parse(Adjustment.CreatedBy.Split(':').Last().Trim(']'));
                            StockAdjustReportLine StockAdjustReportLine = new StockAdjustReportLine(Adjustment, Context.Users.Find(Id).Name);
                            StockAdjustReportLines.Add(StockAdjustReportLine);
                        }
                    }
                }
            }
        }
    }

    public class StockAdjustReportLine
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public String AdjustmentLoaction { get; set; }
        public double Qty { get; set; }
        public string CreatedBy { get; set; }

        public StockAdjustReportLine()
        {
        }
        public StockAdjustReportLine(StockMovementAdjustment Adjustment, string CreatedBy)
        {
            this.Id = Adjustment.Id;
            this.Date = Adjustment.MovementDate;
            this.CreatedBy = CreatedBy;
            this.Reference = Adjustment.RefNumber;
            this.AdjustmentLoaction = Adjustment.InventoryStockLocation.Name;
            this.Qty = Adjustment.StockMovementDetails.Sum(x => x.Quantity);
        }
    }
    public class StockAdjustByItemReportLine
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }

        public StockAdjustByItemReportLine()
        {
        }
        public StockAdjustByItemReportLine(StockMovementDetail detail)
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
