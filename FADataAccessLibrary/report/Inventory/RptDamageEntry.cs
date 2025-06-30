using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.context;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore;
using fa.report;
using fa.report.Inventory;
using FaData.Utils;
using fa;

namespace FADataAccessLibrary.report.Inventory
{
    public enum DamageEntryReportFilterType
    {
        BYDATE, BYITEM, BYLOCATION
    }
    public class RptDamageEntry : Report
    {
        public long[] LocationIds { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public string ReportHeader { get; set; }
        public List<DamageEntryReportLine> DamageEntry = null;
        public List<DamageEntryByItemReportLine> DamageEntryByItems = null;

        public DamageEntryReportFilterType Type { get; set; }

        public override string ReportTitle()
        {
            return "Damage Entry " + (Type == DamageEntryReportFilterType.BYDATE ? "By Date" : Type == DamageEntryReportFilterType.BYITEM ? "By Item" : "By Location");
        }
        public string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", FaData.Utils.DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));

        }
        public override string ReportName()
        {
            return String.Format("DamageEntry" + " " + DateTime.Now.ToString("dd-MM-yyyy").Replace(" / ", " - "));
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == DamageEntryReportFilterType.BYITEM)
                {
                    IList<StockMovementDetail> StockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.CompanyId == Company.CompanyId && x.StockMovement.Type == InventoryJournalType.DAMAGED && x.StockMovement.MovementDate <= ToDate && x.StockMovement.MovementDate >= FromDate).ToList();
                    if (StockMovementDetails != null && StockMovementDetails.Count > 0)
                    {
                        DamageEntryByItems = new List<DamageEntryByItemReportLine>();
                        foreach (StockMovementDetail detail in StockMovementDetails)
                        {
                            DamageEntryByItemReportLine damageEntryByItemReportLine = new DamageEntryByItemReportLine(detail);
                            DamageEntryByItems.Add(damageEntryByItemReportLine);
                        }
                    }
                }
                else
                {
                    IList<StockMovement> stockMovement = null;
                    if (Type == DamageEntryReportFilterType.BYLOCATION)
                    {
                        stockMovement = Context.StockMovement.Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.CompanyId == Company.CompanyId && x.StockMovementDetails.First().StockMovement.Type == InventoryJournalType.DAMAGED && x.MovementDate <= ToDate && x.MovementDate >= FromDate && LocationIds.Contains(x.InventoryStockLocationId)).ToList();
                    }
                    if (Type == DamageEntryReportFilterType.BYDATE)
                    {
                        stockMovement = Context.StockMovement.Include("InventoryStockLocation").Include("StockMovementDetails").Where(x => x.CompanyId == Company.CompanyId && x.StockMovementDetails.First().StockMovement.Type == InventoryJournalType.DAMAGED && x.MovementDate <= ToDate && x.MovementDate >= FromDate).ToList();
                    }
                    if (stockMovement != null && stockMovement.Count > 0)
                    {
                        DamageEntry = new List<DamageEntryReportLine>();
                        foreach (StockMovement Movement in stockMovement)
                        {
                            long Id = long.Parse(Movement.CreatedBy.Split(':').Last().Trim(']'));
                            DamageEntryReportLine DamageEntryReportLine = new DamageEntryReportLine(Movement, Context.Users.Find(Id).Name);
                            DamageEntry.Add(DamageEntryReportLine);
                        }
                    }
                }
            }
        }
    }
    public class DamageEntryReportLine
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string Location { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }
        public string EnteredBy { get; set; }

        public DamageEntryReportLine()
        {
        }
        public DamageEntryReportLine(StockMovement Movement, string EnteredBy)
        {
            this.Id = Movement.Id;
            this.Date = Movement.MovementDate;
            this.Reference = Movement.RefNumber;
            this.Location = Movement.InventoryStockLocation.ToString();
            this.Quantity = Movement.StockMovementDetails.Sum(x => x.Quantity);
            this.Free = Movement.StockMovementDetails.Sum(x => x.FreeQuantity);
            this.EnteredBy = EnteredBy;
        }
    }
    public class DamageEntryByItemReportLine
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public double Quantity { get; set; }
        public double Free { get; set; }
        
        public DamageEntryByItemReportLine()
        {
        }
        public DamageEntryByItemReportLine(StockMovementDetail detail)
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
}
