using fa.context;
using fa.Data;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report;
using fa.report.Inventory;
using Fa.model.Purchase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Abstractions;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace FADataAccessLibrary.report.Inventory
{
    public enum ComboTypeSelection
    {
        BYDATE, BYCATEGORY, BYPRODUCTFAMILY, BYMANUFACTURER, BYSUPPLIER, BYRACK
    }
    public class RptStockReport : Report
    {
        public long[] LocationIds { get; set; }
        public bool BatchFlag { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public int LocationCount { get; set; }
        public string ReportHeader { get; set; }
        public long ProductId { get; set; }
        public long[] CategoryIds { get; set; }
        public string[] CategoryNames { get; set; }
        public long[] ProductFamilyIds { get; set; }
        public string[] ProductFamilyNames { get; set; }
        public string[] SupplierName { get; set; }
        public string[] ManufactureName { get; set; }
        public string[] RackNumber { get; set; }
        public ComboTypeSelection Type { get; set; }
        public bool IsBatchWise { get; set; }

        public List<StockReportLineItemsData> StockReportLineItemData = null;
        public override string ReportTitle()
        {
            return "Stock Report";
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
            return String.Format("Stock Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
               
                if (Type == ComboTypeSelection.BYDATE)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        if (Type == ComboTypeSelection.BYDATE)
                        {
                            var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId).ToList();
                            var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                            PopulateBatchWiseLineItem(Context, products, items);
                        }
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context, products, items);
                    }
                }
                else if (Type == ComboTypeSelection.BYCATEGORY)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && CategoryIds.Contains((long)p.ProductFamily.ParentId)).ToList();
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                        PopulateBatchWiseLineItem(Context, products, items);
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && CategoryIds.Contains((long)p.ProductFamily.ParentId)).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context, products, items);
                    }
                }
                else if (Type == ComboTypeSelection.BYPRODUCTFAMILY)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && ProductFamilyIds.Contains((long)p.ProductFamily.Id)).ToList();
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                        PopulateBatchWiseLineItem(Context, products, items);
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && ProductFamilyIds.Contains((long)p.ProductFamily.Id)).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context, products, items);
                    }
                }
                else if (Type == ComboTypeSelection.BYMANUFACTURER)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.Manufacturer != null && ManufactureName.Contains(p.Manufacturer)).ToList();
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                        PopulateBatchWiseLineItem(Context, products, items);
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.Manufacturer != null && ManufactureName.Contains(p.Manufacturer)).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context, products, items);
                    }
                }
                else if (Type == ComboTypeSelection.BYSUPPLIER)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.SupplierName != null && SupplierName.Contains(p.SupplierName)).ToList();
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                        PopulateBatchWiseLineItem(Context, products, items);
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.SupplierName != null && SupplierName.Contains(p.SupplierName)).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context, products, items);
                    }
                }
                else if (Type == ComboTypeSelection.BYRACK)
                {
                    StockReportLineItemData = new List<StockReportLineItemsData>();
                    if (IsBatchWise == true)
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.RackNumber != null && RackNumber.Contains(p.RackNumber)).ToList();
                        var items = Context.InventoryBatches.Include(b => b.Inventory).Where(b => b.Inventory != null && b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.Inventory.InventoryLocationId)).ToList();
                        PopulateBatchWiseLineItem(Context, products, items);
                    }
                    else
                    {
                        var products = Context.Products.Include(p => p.Inventorys).Include(p => p.ProductFamily).Include(p => p.ProductFamily.Parent).Where(p => p.CompanyId == this.Company.CompanyId && p.RackNumber != null && RackNumber.Contains(p.RackNumber)).ToList();
                        var items = Context.Inventories.Include(b => b.InventoryBatchs).Where(b => b.CompanyId == this.Company.CompanyId && LocationIds.Contains(b.InventoryLocationId)).ToList();
                        PopulateNonBatchWiseLineItem(Context,products,items);
                    }
                }
            }
        }
        public void PopulateBatchWiseLineItem(AccountMasterContext Context, List<Product> products, List<InventoryBatch> items)
        {
            DateTime FirstDayofCurrentdate = new DateTime(this.FromDate.Year, this.FromDate.Month, 1);
            DateTime lastDayofPreviousMonth = FirstDayofCurrentdate.AddDays(-1);
            DateTime FirstDayofPreviousMonth = new DateTime(lastDayofPreviousMonth.Year, lastDayofPreviousMonth.Month, 1);
            foreach (var product in products)
            {
                var inventoryBatches = items.Where(b => b.ProductId == product.Id).ToList();
                if (!inventoryBatches.Any() && (IsAllLocation == true || LocationCount == 1))
                {
                    var stockReportLineItem = new StockReportLineItemsData(product, null, 0, 0, "No Inventory", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    StockReportLineItemData.Add(stockReportLineItem);
                    continue;
                }
                foreach (var inventoryBatch in inventoryBatches)
                {
                    double ClosingStock = 0;
                    double openingstock = 0;
                    double openingStockAmount = 0;
                    double ClosingStockAmount = 0;

                    var StockMovementForOPS = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.BatchNo == inventoryBatch.BatchNo && p.StockMovement.InventoryStockLocationId == inventoryBatch.Inventory.InventoryLocationId && p.StockMovement.MovementDate < this.FromDate).ToList();
                   
                    var openingstockForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.OPEN_STOCK).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var purchaseReturnsForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var salesReturnsForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var purchaseForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var salesForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var stockInForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var stockOutForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var toPatientForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var damageForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var adjustForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();

                    openingstock = ((purchaseForOPS + stockInForOPS + salesReturnsForOPS + openingstockForOPS + adjustForOPS) - (salesForOPS + stockOutForOPS + damageForOPS + toPatientForOPS + purchaseReturnsForOPS));
                    if (product.isInventoryAtBatch == true)
                    {
                        openingStockAmount = inventoryBatch.Cost * openingstock;
                    }

                    var StockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.BatchNo == inventoryBatch.BatchNo && p.StockMovement.InventoryStockLocationId == inventoryBatch.Inventory.InventoryLocationId && p.StockMovement.MovementDate >= this.FromDate && p.StockMovement.MovementDate <= this.ToDate).ToList();
                    var LastMonthStockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.BatchNo == inventoryBatch.BatchNo && p.StockMovement.InventoryStockLocationId == inventoryBatch.Inventory.InventoryLocationId && p.StockMovement.MovementDate >= FirstDayofPreviousMonth && p.StockMovement.MovementDate <= lastDayofPreviousMonth).ToList();

                    var purchaseReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var salesReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var purchase = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var sales = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var stockIn = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var stockOut = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var toPatient = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var damage = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var adjust = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();
                    var lastMonthSale = LastMonthStockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == inventoryBatch.RetailUOM ? p.Quantity / inventoryBatch.RetailXFactor : p.Quantity * inventoryBatch.WholesaleXFactor).Sum();

                    ClosingStock = ((purchase + stockIn + salesReturns + openingstock) - (sales + stockOut + damage + toPatient + purchaseReturns) + adjust);
                    if (product.isInventoryAtBatch == true)
                    {
                        ClosingStockAmount = inventoryBatch.Cost * ClosingStock;
                    }

                    InventoryLocation inventoryLocations = Context.InventoryLocation.FirstOrDefault(x => x.Id == inventoryBatch.Inventory.InventoryLocationId);

                    var stockReportLineItem = new StockReportLineItemsData(product, inventoryBatch, salesReturns, purchaseReturns, inventoryLocations?.Name ?? "Unknown",
                        purchase, sales, openingstock, stockIn, stockOut, toPatient, damage, adjust, lastMonthSale, ClosingStock, openingStockAmount, ClosingStockAmount);
                    StockReportLineItemData.Add(stockReportLineItem);
                }
            }
        }
        public void PopulateNonBatchWiseLineItem(AccountMasterContext Context, List<Product> products, List<fa.model.OrderManagement.Inventory> items)
        {
            DateTime FirstDayofCurrentdate = new DateTime(this.FromDate.Year, this.FromDate.Month, 1);
            DateTime lastDayofPreviousMonth = FirstDayofCurrentdate.AddDays(-1);
            DateTime FirstDayofPreviousMonth = new DateTime(lastDayofPreviousMonth.Year, lastDayofPreviousMonth.Month, 1);
            foreach (var product in products)
            {
                var inventoryItems = items.Where(b => b.ProductId == product.Id).ToList();
                if (!inventoryItems.Any() && (IsAllLocation == true || LocationCount == 1))
                {
                    var stockReportLineItem = new StockReportLineItemsData(product, null, 0, 0, "No Inventory", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    StockReportLineItemData.Add(stockReportLineItem);
                    continue;
                }
                foreach (var InventoryItem in inventoryItems)
                {
                    double ClosingStock = 0;
                    double openingstock = 0;
                    double openingStockAmount = 0;
                    double ClosingStockAmount = 0;

                    var StockMovementForOPS = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.StockMovement.InventoryStockLocationId == InventoryItem.InventoryLocationId && p.StockMovement.MovementDate < this.FromDate).ToList();

                    var openingstockForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.OPEN_STOCK).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var purchaseReturnsForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var salesReturnsForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var purchaseForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var salesForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var stockInForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var stockOutForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var toPatientForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var damageForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var adjustForOPS = StockMovementForOPS.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();

                    openingstock = ((purchaseForOPS + stockInForOPS + salesReturnsForOPS + openingstockForOPS) - (salesForOPS + stockOutForOPS + damageForOPS + toPatientForOPS + purchaseReturnsForOPS) + adjustForOPS);
                    openingStockAmount += product.CostPrice * openingstock;

                    var StockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.StockMovement.InventoryStockLocationId == InventoryItem.InventoryLocationId && p.StockMovement.MovementDate >= this.FromDate && p.StockMovement.MovementDate <= this.ToDate).ToList();
                    var LastMonthStockMovement = Context.StockMovementDetail.Include(p => p.StockMovement).Where(p => p.CompanyId == this.Company.CompanyId && p.ProductId == product.Id && p.StockMovement.InventoryStockLocationId == InventoryItem.InventoryLocationId && p.StockMovement.MovementDate >= FirstDayofPreviousMonth && p.StockMovement.MovementDate <= lastDayofPreviousMonth).ToList();

                    var purchaseReturns = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCASE_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var salesReturns = StockMovement.Where(p =>  p.StockMovement.Type == InventoryJournalType.SALES_RETURN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var purchase = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PURCHASE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var sales = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var stockIn = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_IN).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var stockOut = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.STOCK_OUT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var toPatient = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.PATIENT_USE).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var damage = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.DAMAGED).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var adjust = StockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.ADJUSTMENT).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();
                    var lastMonthSale = LastMonthStockMovement.Where(p => p.StockMovement.Type == InventoryJournalType.SALES).Select(p => p.Uom == product.RetailUOM ? p.Quantity / product.RetailXFactor : p.Quantity * product.WholesaleXFactor).Sum();

                    ClosingStock = ((purchase + stockIn + salesReturns + openingstock + adjust) - (sales + stockOut + damage + toPatient + purchaseReturns));
                    ClosingStockAmount += product.CostPrice * ClosingStock;

                    InventoryLocation inventoryLocations = Context.InventoryLocation.FirstOrDefault(x => x.Id == InventoryItem.InventoryLocationId);

                    var stockReportLineItem = new StockReportLineItemsData(product, null, salesReturns, purchaseReturns, inventoryLocations?.Name ?? "Unknown",
                        purchase, sales, openingstock, stockIn, stockOut, toPatient, damage, adjust, lastMonthSale, ClosingStock, openingStockAmount, ClosingStockAmount);
                    StockReportLineItemData.Add(stockReportLineItem);
                }
            }
        }
    }
    public class StockReportLineItemsData
    {
        public string MaterialId { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }
        public string RackNumber { get; set; }
        public string Batch { get; set; }
        public DateTime BatchExpDate { get; set; }
        public double OpeningStock { get; set; }
        public double CloseingStock { get; set; }
        public double PurchaseQty { get; set; }
        public double PurchaseReturnQty { get; set; }
        public double SalesQty { get; set; }
        public double SalesReturnQty { get; set; }
        public double StockInQty { get; set; }
        public double StockOutQty { get; set; }
        public double ToPatientQty { get; set; }
        public double DamageQty { get; set; }
        public double AdjustQty { get; set; }
        public double LastMonthSale { get; set; }
        public string LocationName { get; set; }
        public string CategoryName { get; set; }
        public string ProductFamilyName { get; set; }
        public string ManufactureName { get; set; }
        public string SupplierName { get; set; }
        public double OpeningStockAmount { get; set; }
        public double ClosingStockAmount { get; set; }

        public StockReportLineItemsData(Product product, InventoryBatch inventory, double salesReturns, double purchaseReturns, string location, double purchase, 
            double sales, double openingStock, double stockIn, double stockOut, double toPatient, double damage, double adjust, double lastMonthSale, double closingStock,
            double openingStockAmount, double closingStockAmount)
        {
            MaterialId = product.MaterialId;
            Name = product.Name;
            Uom = product.UOM;
            RackNumber = !string.IsNullOrEmpty(product.RackNumber) ? product.RackNumber : "";
            Batch = inventory?.BatchNo??"";
            if(inventory != null)
            {
                BatchExpDate = inventory.ExpDate;
            }
            OpeningStock = openingStock;
            CloseingStock = closingStock;
            PurchaseQty = purchase;
            PurchaseReturnQty = purchaseReturns;
            SalesQty = sales;
            SalesReturnQty = salesReturns;
            StockInQty = stockIn;
            StockOutQty = stockOut;
            ToPatientQty = toPatient;
            DamageQty = damage;
            AdjustQty = adjust;
            LastMonthSale = lastMonthSale;
            LocationName = location;
            CategoryName = product.ProductFamily.Parent.Name;
            ProductFamilyName = product.ProductFamily.Name;
            ManufactureName = !string.IsNullOrEmpty(product.Manufacturer) ? product.Manufacturer : "";
            SupplierName = !string.IsNullOrEmpty(product.SupplierName) ? product.SupplierName : "";
            OpeningStockAmount = openingStockAmount;
            ClosingStockAmount = closingStockAmount;
        }
    }
}
