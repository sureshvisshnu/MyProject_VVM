using fa.context;
using fa.model.Catalog;
using fa.api.OrderManagement;
using fa.model.OrderManagement;
using fa.api.Hms;
using fa.api.catalog;
using FaData.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MySqlConnector;
using fa.model.Accounting.Masters;
using fa.Data;
using fa.model.Common;
using NPOI.SS.Formula.Functions;
using System.Data;
using NPOI.POIFS.Crypt.Dsig;
using System.ComponentModel.Design;
using System.Security.Cryptography;

namespace fa.report.Inventory
{
    public class ReportStock : Report
    {
        public long[] LocationIds { get; set; }
        public bool BatchFlag { get; set; }
        public string Location;
        public bool IsAllLocation { get; set; }
        public bool IsExpiryReport { get; set; }
        public string ReportHeader { get; set; }
        public long ProductId { get; set; }
        public long[] CategoryIds { get; set; }
        public string[] CategoryNames { get; set; }
        public long[] ProductFamilyIds { get; set; }
        public string[] ProductFamilyNames { get; set; }
        public string[] SupplierName {  get; set; }
        public int CategoryCall = 0;
        List<StockLedger> _StockLedger = new List<StockLedger>();       
        public IList<StockLedger> StockLedger
        {
            get
            {
                if(_StockLedger.Count>0 && IsAllLocation)
                {
                    List<StockLedger> _lStockLedger = new List<StockLedger>();
                    StockLedger StockLedger = new StockLedger();
                    string MaterialId = string.Empty;
                    int i = 0;
                    foreach (StockLedger lStockLedger in _StockLedger.OrderBy(x => x.MaterialId).ThenBy(x => x.Batch))
                    {
                        if (lStockLedger.MaterialId != MaterialId)
                        {
                            if (i != 0)
                            {
                                List<BatchOpeningStock> lBatchOpeningStock = new List<BatchOpeningStock>();
                                BatchOpeningStock BatchOpeningStock = new BatchOpeningStock();
                                string BatchNo = string.Empty;
                                int j = 0;
                                double ClosingStock = 0.00;
                                foreach (BatchOpeningStock BatchWise in StockLedger.OpeningStockforBatch.OrderBy(x => x.BatchNo))
                                {
                                    if (BatchWise.BatchNo != BatchNo)
                                    {
                                        if (j != 0)
                                        {
                                            BatchOpeningStock.ClosingStock = ClosingStock;
                                            lBatchOpeningStock.Add(BatchOpeningStock);
                                        }
                                        ClosingStock = 0.00;
                                        BatchOpeningStock = new BatchOpeningStock();
                                        BatchOpeningStock.BatchNo = BatchWise.BatchNo;
                                        BatchOpeningStock.ExpDate = BatchWise.ExpDate;
                                        BatchOpeningStock.OpeningStock += BatchWise.OpeningStock;
                                        ClosingStock += ((BatchWise.OpeningStock + BatchWise.BatchPurchaseQty + BatchWise.BatchSalesRtnQty + BatchWise.BatchStockMovedInQty) - (BatchWise.BatchPurchaseRtnQty + BatchWise.BatchSalesQty + BatchWise.BatchStockMovedOutQty + BatchWise.BatchPatientConsumedQty  + BatchWise.BatchDamagedQty) + BatchWise.BatchAdjustQty);
                                        BatchOpeningStock.BatchPurchaseQty += BatchWise.BatchPurchaseQty;
                                        BatchOpeningStock.BatchPurchaseRtnQty += BatchWise.BatchPurchaseRtnQty;
                                        BatchOpeningStock.BatchSalesQty += BatchWise.BatchSalesQty;
                                        BatchOpeningStock.BatchSalesRtnQty += BatchWise.BatchSalesRtnQty;
                                        BatchOpeningStock.BatchStockMovedInQty += BatchWise.BatchStockMovedInQty;
                                        BatchOpeningStock.BatchStockMovedOutQty += BatchWise.BatchStockMovedOutQty;
                                        BatchOpeningStock.BatchPatientConsumedQty += BatchWise.BatchPatientConsumedQty;
                                        BatchOpeningStock.BatchDamagedQty += BatchWise.BatchDamagedQty;
                                        BatchOpeningStock.BatchAdjustQty += BatchWise.BatchAdjustQty;
                                        BatchNo = BatchWise.BatchNo;
                                    }
                                    else
                                    {
                                        BatchOpeningStock.OpeningStock += BatchWise.OpeningStock;
                                        ClosingStock += ((BatchWise.OpeningStock + BatchWise.BatchPurchaseQty + BatchWise.BatchSalesRtnQty + BatchWise.BatchStockMovedInQty) - (BatchWise.BatchPurchaseRtnQty + BatchWise.BatchSalesQty + BatchWise.BatchStockMovedOutQty + BatchWise.BatchPatientConsumedQty + BatchWise.BatchDamagedQty) + BatchWise.BatchAdjustQty);
                                        BatchOpeningStock.BatchPurchaseQty += BatchWise.BatchPurchaseQty;
                                        BatchOpeningStock.BatchPurchaseRtnQty += BatchWise.BatchPurchaseRtnQty;
                                        BatchOpeningStock.BatchSalesQty += BatchWise.BatchSalesQty;
                                        BatchOpeningStock.BatchSalesRtnQty += BatchWise.BatchSalesRtnQty;
                                        BatchOpeningStock.BatchStockMovedInQty += BatchWise.BatchStockMovedInQty;
                                        BatchOpeningStock.BatchStockMovedOutQty += BatchWise.BatchStockMovedOutQty;
                                        BatchOpeningStock.BatchPatientConsumedQty += BatchWise.BatchPatientConsumedQty;
                                        BatchOpeningStock.BatchDamagedQty += BatchWise.BatchDamagedQty;
                                        BatchOpeningStock.BatchAdjustQty += BatchWise.BatchAdjustQty;
                                    }
                                    if (StockLedger.OpeningStockforBatch.Count == (j + 1))
                                    {
                                        BatchOpeningStock.ClosingStock = ClosingStock;
                                        lBatchOpeningStock.Add(BatchOpeningStock);
                                    }
                                    j++;
                                    
                                }
                                StockLedger.OpeningStockforBatch = lBatchOpeningStock;
                                _lStockLedger.Add(StockLedger);
                            }
                            StockLedger = new StockLedger();
                            StockLedger.MaterialId = lStockLedger.MaterialId;
                            StockLedger.Name = lStockLedger.Name;
                            StockLedger.RackNumber = lStockLedger.RackNumber;
                            StockLedger.uom = lStockLedger.uom;
                            StockLedger.Batch = lStockLedger.Batch;
                            StockLedger.Location = lStockLedger.Location;
                            StockLedger.OpeningStock += lStockLedger.OpeningStock;
                            StockLedger.CloseingStock += lStockLedger.CloseingStock;
                            StockLedger.PurchaseQty += lStockLedger.PurchaseQty;
                            StockLedger.PurchaseRtnQty += lStockLedger.PurchaseRtnQty;
                            StockLedger.SalesQty += lStockLedger.SalesQty;
                            StockLedger.SalesRtnQty += lStockLedger.SalesRtnQty;
                            StockLedger.StockMovedInQty += lStockLedger.StockMovedInQty;
                            StockLedger.StockMovedOutQty += lStockLedger.StockMovedOutQty;
                            StockLedger.PatientConsumedQty += lStockLedger.PatientConsumedQty;
                            StockLedger.DamagedQty += lStockLedger.DamagedQty;
                            StockLedger.AdjustQty += lStockLedger.AdjustQty;
                            StockLedger.OpeningStockforBatch.AddRange(lStockLedger.OpeningStockforBatch);
                            StockLedger.Supplier = lStockLedger.Supplier;
                            StockLedger.Manufacturer = lStockLedger.Manufacturer;
                            StockLedger.CatType = lStockLedger?.CatType;
                            StockLedger.CatPFType = lStockLedger?.CatPFType;
                            MaterialId = lStockLedger.MaterialId;
                        }
                        else 
                        { 
                            StockLedger.OpeningStock += lStockLedger.OpeningStock;
                            StockLedger.CloseingStock += lStockLedger.CloseingStock;
                            StockLedger.PurchaseQty += lStockLedger.PurchaseQty;
                            StockLedger.PurchaseRtnQty += lStockLedger.PurchaseRtnQty;
                            StockLedger.SalesQty += lStockLedger.SalesQty;
                            StockLedger.SalesRtnQty += lStockLedger.SalesRtnQty;
                            StockLedger.StockMovedInQty += lStockLedger.StockMovedInQty;
                            StockLedger.StockMovedOutQty += lStockLedger.StockMovedOutQty;
                            StockLedger.PatientConsumedQty += lStockLedger.PatientConsumedQty;
                            StockLedger.DamagedQty += lStockLedger.DamagedQty;
                            StockLedger.AdjustQty += lStockLedger.AdjustQty;
                            StockLedger.OpeningStockforBatch.AddRange(lStockLedger.OpeningStockforBatch);
                        }       
                        if(_StockLedger.Count==(i+1))
                        {
                            List<BatchOpeningStock> lBatchOpeningStock = new List<BatchOpeningStock>();
                            BatchOpeningStock BatchOpeningStock = new BatchOpeningStock();
                            string BatchNo = string.Empty;
                            string LocationName = string.Empty;
                            double ClosingStock = 0.00;
                            int j = 0;
                            foreach (BatchOpeningStock BatchWise in StockLedger.OpeningStockforBatch.OrderBy(x => x.BatchNo))
                            {
                                if (BatchWise.BatchNo != BatchNo)
                                {
                                    if (j != 0)
                                    {
                                        BatchOpeningStock.ClosingStock = ClosingStock;
                                        lBatchOpeningStock.Add(BatchOpeningStock);
                                    }
                                    ClosingStock = 0.00;
                                    BatchOpeningStock = new BatchOpeningStock();
                                    BatchOpeningStock.BatchNo = BatchWise.BatchNo;
                                    BatchOpeningStock.ExpDate = BatchWise.ExpDate;
                                    BatchOpeningStock.OpeningStock += BatchWise.OpeningStock;
                                    ClosingStock += ((BatchWise.OpeningStock+BatchWise.BatchPurchaseQty + BatchWise.BatchSalesRtnQty + BatchWise.BatchStockMovedInQty) - (BatchWise.BatchPurchaseRtnQty + BatchWise.BatchSalesQty + BatchWise.BatchStockMovedOutQty + BatchWise.BatchPatientConsumedQty + BatchWise.BatchDamagedQty) + BatchWise.BatchAdjustQty);
                                    BatchOpeningStock.BatchPurchaseQty += BatchWise.BatchPurchaseQty;
                                    BatchOpeningStock.BatchPurchaseRtnQty += BatchWise.BatchPurchaseRtnQty;
                                    BatchOpeningStock.BatchSalesQty += BatchWise.BatchSalesQty;
                                    BatchOpeningStock.BatchSalesRtnQty += BatchWise.BatchSalesRtnQty;
                                    BatchOpeningStock.BatchStockMovedInQty += BatchWise.BatchStockMovedInQty;
                                    BatchOpeningStock.BatchStockMovedOutQty += BatchWise.BatchStockMovedOutQty;
                                    BatchOpeningStock.BatchPatientConsumedQty += BatchWise.BatchPatientConsumedQty;
                                    BatchOpeningStock.BatchDamagedQty += BatchWise.BatchDamagedQty;
                                    BatchOpeningStock.BatchAdjustQty += BatchWise.BatchAdjustQty;
                                    BatchNo = BatchWise.BatchNo;
                                }
                                else
                                {
                                    BatchOpeningStock.OpeningStock += BatchWise.OpeningStock;
                                    ClosingStock += ((BatchWise.OpeningStock + BatchWise.BatchPurchaseQty + BatchWise.BatchSalesRtnQty + BatchWise.BatchStockMovedInQty) - (BatchWise.BatchPurchaseRtnQty + BatchWise.BatchSalesQty + BatchWise.BatchStockMovedOutQty + BatchWise.BatchPatientConsumedQty + BatchWise.BatchDamagedQty) + BatchWise.BatchAdjustQty);
                                    BatchOpeningStock.BatchPurchaseQty += BatchWise.BatchPurchaseQty;
                                    BatchOpeningStock.BatchPurchaseRtnQty += BatchWise.BatchPurchaseRtnQty;
                                    BatchOpeningStock.BatchSalesQty += BatchWise.BatchSalesQty;
                                    BatchOpeningStock.BatchSalesRtnQty += BatchWise.BatchSalesRtnQty;
                                    BatchOpeningStock.BatchStockMovedInQty += BatchWise.BatchStockMovedInQty;
                                    BatchOpeningStock.BatchStockMovedOutQty += BatchWise.BatchStockMovedOutQty;
                                    BatchOpeningStock.BatchPatientConsumedQty += BatchWise.BatchPatientConsumedQty;
                                    BatchOpeningStock.BatchDamagedQty += BatchWise.BatchDamagedQty;
                                    BatchOpeningStock.BatchAdjustQty += BatchWise.BatchAdjustQty;
                                }
                                if (StockLedger.OpeningStockforBatch.Count == (j + 1))
                                {
                                    BatchOpeningStock.ClosingStock = ClosingStock;
                                    lBatchOpeningStock.Add(BatchOpeningStock);
                                }
                                j++;
                            }
                            StockLedger.OpeningStockforBatch = lBatchOpeningStock;
                            _lStockLedger.Add(StockLedger);
                        }
                        i++;
                    }
                    return _lStockLedger;
                }
                return _StockLedger;
            }
        }
               
        public override string ReportTitle()
        {
            return IsExpiryReport? "Expiry Report" : "Stock Report";
        }
        public string ReportSubTitle()
        {
            string TransactionDate = this.FromDate.ToString(Company.DateFormat);
            char Separator = TransactionDate.Contains("-") ? '-' : TransactionDate.Contains("/") ? '/' : '.';
            string[] Date = TransactionDate.Split(Separator);
            return String.Format("{0} to {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public string ReportDate()
        {
            return String.Format("{0} to {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format((IsExpiryReport ? "Expiry Report" : "Stock Report")+" {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            if (IsExpiryReport)
            {
                ExpiryReportBatchWise();
            }
            else
            {
                StockReportBatchWise();
            }
        }
        private void StockReportBatchWise()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                foreach (long LocationId in LocationIds)
                {
                    IList<Product> ProductList = null;
                    if(CategoryCall == 0 || CategoryCall == 3 || CategoryCall == 4 || CategoryCall == 5  )
                    {
                        ProductList = CatalogProductManager.Instance.ListProductByCompanyId(LocationId, this.Company.CompanyId, Context);
                    }
                    else if(CategoryCall == 1)
                    {
                        ProductList = CatalogProductManager.Instance.ListProductByCompanyIdAndCategoryType(LocationId, this.Company.CompanyId, CategoryIds, Context);
                    }
                    else if(CategoryCall == 2)
                    {
                        ProductList = CatalogProductManager.Instance.ListProductByCompanyIdAndProductFamilyTypeName(LocationId, this.Company.CompanyId, ProductFamilyNames, Context); 
                    }
                    if (ProductList != null)
                    {

                        foreach (var Productsin in ProductList)
                        {
                           Product Products = Context.Products.Include(p => p.ProductFamily).ThenInclude(pf => pf.Parent).FirstOrDefault(x => x.Id == Productsin.Id);
                            Boolean IsBatch = (Products._isInventoryAtBatch != null && (bool)Products._isInventoryAtBatch && BatchFlag) ? true : false;
                            if (Products != null)
                            {
                                InventoryLocation InventoryLocation = Context.InventoryLocation.FirstOrDefault(x => x.Id == LocationId);
                                if (InventoryLocation != null)
                                {
                                    StockLedger StockLedger = new StockLedger();
                                    if (FromDate <= ToDate)
                                    {
                                        StockLedger.Location = InventoryLocation;
                                        StockLedger.MaterialId = Products.MaterialId;
                                        StockLedger.Name = Products.Name;
                                        StockLedger.uom = Products.UOM;
                                        StockLedger.RackNumber = Products.RackNumber;
                                        StockLedger.Supplier = Products?.Supplier?.ToString() ?? "";
                                        StockLedger.Manufacturer = Products?.Manufacturer?.ToString() ?? "";
                                        StockLedger.Batch = IsBatch;
                                        StockLedger.CatType = Products?.ProductFamily?.Parent?.Name ?? "";
                                        StockLedger.CatPFType = Products?.ProductFamily?.Name ?? "";

                                        fa.model.OrderManagement.Inventory Inventory = Context.Inventories.FirstOrDefault(x => x.ProductId == Products.Id && x.InventoryLocationId == InventoryLocation.Id);
                                        if (Inventory != null)
                                        {
                                            //OPENING STOCK
                                            if (IsBatch)
                                            {
                                                StockLedger.OpeningStockforBatch = new List<BatchOpeningStock>();
                                                IList<InventoryBatch> lBatch = Context.InventoryBatches.Where(x => x.InventoryId == Inventory.Id).ToList();
                                                if (lBatch.Count == 0)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    foreach (InventoryBatch Batch in lBatch)
                                                    {
                                                        BatchOpeningStock BatchOpeningStock = new BatchOpeningStock();
                                                        BatchOpeningStock.BatchNo = Batch.BatchNo;
                                                        BatchOpeningStock.ExpDate = Batch.ExpDate;
                                                        BatchOpeningStock.Location = InventoryLocation;
                                                        StockLedger.OpeningStockforBatch.Add(BatchOpeningStock);
                                                    }
                                                }
                                            //}s
                                            //if (IsBatch)
                                            //{
                                                foreach (BatchOpeningStock IBatch in StockLedger.OpeningStockforBatch)
                                                {
                                                    using System.Data.DataTable StockTable = ExecuteStoredProcedure("GetOpeningStockLastMonthQtyByBatch", Products.WholesaleUOM, Products.Id, Company.CompanyId, LocationId, ToDate, ToDate.AddDays(-30), 0, IBatch.BatchNo);
                                                    var ItemList = StockTable.Rows[0];
                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).OpeningStock += ItemList[0] != DBNull.Value ? double.Parse(ItemList[0].ToString()) : 0;

                                                    if (StockLedger.OpeningStockforBatch.Count > 0)
                                                    {
                                                        StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchLMSQty += ItemList[1] != DBNull.Value ? double.Parse(ItemList[1].ToString()) : 0;
                                                        StockLedgerLineItem.LMSQty = ItemList[1] != DBNull.Value ? double.Parse(ItemList[1].ToString()) : 0;
                                                        StockLedger.LMSQty += StockLedgerLineItem.LMSQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                        //stock movement
                                                        for (int k = 1; k < 10; k++)
                                                        {
                                                            using System.Data.DataTable StockMovementTable = ExecuteStoredProcedure("GetOpeningStockByMovementByBatch", Products.WholesaleUOM, Products.Id, Company.CompanyId, LocationId, FromDate, ToDate, k, IBatch.BatchNo);
                                                            ItemList = StockMovementTable.Rows[0];
                                                            var Stock = ItemList[0] != DBNull.Value ? double.Parse(ItemList[0].ToString()) : 0;
                                                            if (k == 1 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock += Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchStockMovedInQty += Stock;
                                                                StockLedgerLineItem.StockMovedInQty = Stock;
                                                                StockLedger.StockMovedInQty += StockLedgerLineItem.StockMovedInQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 2 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock -= Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchStockMovedOutQty += Stock;
                                                                StockLedgerLineItem.StockMovedOutQty = Stock;
                                                                StockLedger.StockMovedOutQty += StockLedgerLineItem.StockMovedOutQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 3 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock -= Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchPatientConsumedQty += Stock;
                                                                StockLedgerLineItem.PatientConsumedQty = Stock;
                                                                StockLedger.PatientConsumedQty += StockLedgerLineItem.PatientConsumedQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 4 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock -= Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchDamagedQty += Stock;
                                                                StockLedgerLineItem.DamagedQty = Stock;
                                                                StockLedger.DamagedQty += StockLedgerLineItem.DamagedQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 5 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock += Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchPurchaseQty += Stock;
                                                                StockLedgerLineItem.PurchaseQty = Stock;
                                                                StockLedger.PurchaseQty += StockLedgerLineItem.PurchaseQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 6 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock -= Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchSalesQty += Stock;
                                                                StockLedgerLineItem.SalesQty = Stock;
                                                                StockLedger.SalesQty += StockLedgerLineItem.SalesQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 7 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock -= Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchPurchaseRtnQty += Stock;
                                                                StockLedgerLineItem.PurchaseRtnQty = Stock;
                                                                StockLedger.PurchaseRtnQty += StockLedgerLineItem.PurchaseRtnQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 8 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock += Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchSalesRtnQty += Stock;
                                                                StockLedgerLineItem.SalesRtnQty = Stock;
                                                                StockLedger.SalesRtnQty += StockLedgerLineItem.SalesRtnQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }
                                                            else if (k == 9 && Stock != 0)
                                                            {
                                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).ClosingStock += Stock;
                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == IBatch.BatchNo).BatchAdjustQty += Stock;
                                                                StockLedgerLineItem.AdjustQty = Stock;
                                                                StockLedger.AdjustQty += StockLedgerLineItem.AdjustQty;
                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                using System.Data.DataTable StockTable=ExecuteStoredProcedure("GetOpeningStockLastMonthQty", Products.WholesaleUOM, Products.Id, Company.CompanyId, LocationId, ToDate, ToDate.AddDays(-30),0,string.Empty);
                                                var ItemList = StockTable.Rows[0];
                                                StockLedger.OpeningStock += ItemList[0] != DBNull.Value?double.Parse(ItemList[0].ToString()):0;
                                                StockLedgerLineItem StockLedgerLineItem = null;
                                                StockLedgerLineItem = new StockLedgerLineItem();
                                                StockLedgerLineItem.LMSQty = ItemList[1] != DBNull.Value ? double.Parse(ItemList[1].ToString()) : 0;
                                                StockLedger.LMSQty += StockLedgerLineItem.LMSQty;
                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                for (int k = 1; k < 10; k++)
                                                {
                                                    using System.Data.DataTable StockMovementTable = ExecuteStoredProcedure("GetOpeningStockByMovement", Products.WholesaleUOM, Products.Id, Company.CompanyId, LocationId, FromDate, ToDate,k,string.Empty);
                                                    ItemList = StockMovementTable.Rows[0];
                                                    var Stock= ItemList[0] != DBNull.Value ? double.Parse(ItemList[0].ToString()) : 0;
                                                    if (k == 1 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.StockMovedInQty = Stock;
                                                        StockLedger.StockMovedInQty += StockLedgerLineItem.StockMovedInQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 2 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.StockMovedOutQty = Stock;
                                                        StockLedger.StockMovedOutQty += StockLedgerLineItem.StockMovedOutQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 3 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.PatientConsumedQty = Stock;
                                                        StockLedger.PatientConsumedQty += StockLedgerLineItem.PatientConsumedQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 4 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.DamagedQty = Stock;
                                                        StockLedger.DamagedQty += StockLedgerLineItem.DamagedQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 5 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.PurchaseQty = Stock;
                                                        StockLedger.PurchaseQty += StockLedgerLineItem.PurchaseQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 6 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.SalesQty = Stock;
                                                        StockLedger.SalesQty += StockLedgerLineItem.SalesQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 7 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.PurchaseRtnQty = Stock;
                                                        StockLedger.PurchaseRtnQty += StockLedgerLineItem.PurchaseRtnQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 8 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.SalesRtnQty = Stock;
                                                        StockLedger.SalesRtnQty += StockLedgerLineItem.SalesRtnQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                    else if (k == 9 && Stock != 0)
                                                    {
                                                        StockLedgerLineItem = new StockLedgerLineItem();
                                                        StockLedgerLineItem.AdjustQty = Stock;
                                                        StockLedger.AdjustQty += StockLedgerLineItem.AdjustQty;
                                                        StockLedger.LineItems.Add(StockLedgerLineItem);
                                                    }
                                                }
                                            }
                                            
                                            if (IsBatch)
                                            {
                                                StockLedger.CloseingStock = (StockLedger.OpeningStock + StockLedger.BatchPurchaseQty - StockLedger.BatchPurchaseRtnQty - StockLedger.BatchSalesQty + StockLedger.BatchSalesRtnQty - StockLedger.BatchStockMovedOutQty + StockLedger.BatchStockMovedInQty - StockLedger.BatchPatientConsumedQty - StockLedger.BatchDamagedQty + StockLedger.BatchAdjustQty);
                                            }
                                            else
                                            {
                                                StockLedger.CloseingStock = (StockLedger.OpeningStock + StockLedger.PurchaseQty - StockLedger.PurchaseRtnQty - StockLedger.SalesQty + StockLedger.SalesRtnQty - StockLedger.StockMovedOutQty + StockLedger.StockMovedInQty - StockLedger.PatientConsumedQty - StockLedger.DamagedQty + StockLedger.AdjustQty);
                                            }

                                            _StockLedger.Add(StockLedger);

                                            // ======================  ################  ====================
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }       
        public DataTable ExecuteStoredProcedure(string Name, string Uom, long PId, long CompanyId, long LocationId, DateTime FDate, DateTime PMDate, int Type, string BatNo)
        {
            DataTable dataTable = new DataTable();
            AccountMasterContext Context = new AccountMasterContext();
            string ConString = Context.Database.GetDbConnection().ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                connection.Open();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(Name, connection))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.AddWithValue("@Uom", Uom);
                    adapter.SelectCommand.Parameters.AddWithValue("@PId", PId);
                    adapter.SelectCommand.Parameters.AddWithValue("@ComId", CompanyId);
                    adapter.SelectCommand.Parameters.AddWithValue("@LoId", LocationId);
                    adapter.SelectCommand.Parameters.AddWithValue("@FDate", FDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@PMDate", PMDate);
                    if (Name == "GetOpeningStockLastMonthQtyByBatch")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SType", Type);
                        adapter.SelectCommand.Parameters.AddWithValue("@BatNo", BatNo);
                    }
                    else if (Name == "GetOpeningStockLastMonthQty" || Name == "GetOpeningStockByMovement")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SType", Type);
                    }
                    else if (Name == "GetOpeningStockByMovementByBatch")
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SType", Type);
                        adapter.SelectCommand.Parameters.AddWithValue("@BatNo", BatNo);
                    }
                    adapter.Fill(dataTable);
                }
            }
            Console.WriteLine("Execution completed successfully.");
            return dataTable;
        }
        private void ExpiryReportBatchWise()
        {
        using (AccountMasterContext Context = new AccountMasterContext())
        {
            foreach (long LocationId in LocationIds)
            {
                IList<Product> ProductList = null;
                ProductList = CatalogProductManager.Instance.ListBatchExpiryProductByCompanyId(LocationId, this.Company.CompanyId, FromDate, ToDate);

                if (ProductList != null)
                {
                    using (var dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        foreach (var Productsin in ProductList)
                        {
                            Product Products = CatalogProductManager.Instance.GetProductInfoById(Productsin.Id);
                            Boolean IsBatch = (Products._isInventoryAtBatch != null && (bool)Products._isInventoryAtBatch && BatchFlag) ? true : false;
                            if (Products != null)
                            {
                                InventoryLocation InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(LocationId);
                                if (InventoryLocation != null)
                                {
                                    StockLedger StockLedger = new StockLedger();
                                    if (FromDate <= ToDate)
                                    {
                                        StockLedger.Location = InventoryLocation;
                                        StockLedger.MaterialId = Products.MaterialId;
                                        StockLedger.Name = Products.Name;
                                        StockLedger.uom = Products.UOM;
                                        StockLedger.Batch = IsBatch;

                                        fa.model.OrderManagement.Inventory Inventory = Context.Inventories.FirstOrDefault(x => x.ProductId == Products.Id && x.InventoryLocationId == InventoryLocation.Id);
                                        if (Inventory != null)
                                        {
                                            //OPENING STOCK
                                            if (IsBatch)
                                            {
                                                StockLedger.OpeningStockforBatch = new List<BatchOpeningStock>();
                                                IList<InventoryBatch> lBatch = Context.InventoryBatches.Where(x => x.InventoryId == Inventory.Id && x.ExpDate>=FromDate &&x.ExpDate<=ToDate).ToList();
                                                foreach (InventoryBatch Batch in lBatch)
                                                {
                                                    BatchOpeningStock BatchOpeningStock = new BatchOpeningStock();
                                                    BatchOpeningStock.BatchNo = Batch.BatchNo;
                                                    BatchOpeningStock.ExpDate = Batch.ExpDate;
                                                    BatchOpeningStock.Location = InventoryLocation;
                                                    StockLedger.OpeningStockforBatch.Add(BatchOpeningStock);
                                                }
                                            }
                                            IList<StockMovement> lStockMovementInfo = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.Type == InventoryJournalType.OPEN_STOCK && x.MovementDate <= ToDate).ToList<StockMovement>();
                                            if (lStockMovementInfo != null && lStockMovementInfo.Count > 0)
                                            {
                                                foreach (StockMovement Entry in lStockMovementInfo.OrderBy(x => x.MovementDate))
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null)
                                                    {
                                                        if (IsBatch && StockLedger.OpeningStockforBatch.Count > 0)
                                                        {
                                                            foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x=> x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity+x.FreeQuantity)>0))
                                                            {
                                                                StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.StockMovementId == Entry.Id);
                                                                if (Details != null)
                                                                {
                                                                    if (Details.Uom == Products.WholesaleUOM)
                                                                    {
                                                                        if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                        {
                                                                            StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock += Details.Quantity;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                        {
                                                                            StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock += (Details.Quantity / Products.RetailXFactor);
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }                                                      
                                                    }
                                                }
                                            }
                                            // STOCK MOVEMENT
                                            IList<StockMovement> StockMovementInfo = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.MovementDate < FromDate).ToList<StockMovement>();
                                            if (StockMovementInfo != null && StockMovementInfo.Count > 0)
                                            {
                                                foreach (StockMovement Entry in StockMovementInfo.OrderBy(x => x.MovementDate))
                                                {
                                                    if (Entry.Type == InventoryJournalType.STOCK_IN || Entry.Type == InventoryJournalType.PURCHASE || Entry.Type == InventoryJournalType.SALES_RETURN || Entry.Type == InventoryJournalType.ADJUSTMENT)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null)
                                                        {
                                                            if (IsBatch && StockLedger.OpeningStockforBatch.Count > 0)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.StockMovementId == Entry.Id);
                                                                    if (Details != null)
                                                                    {
                                                                        if (Details.Uom == Products.WholesaleUOM)
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock += (Details.Quantity + Details.FreeQuantity);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock += ((Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                            }                                                        
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.STOCK_OUT || Entry.Type == InventoryJournalType.PURCASE_RETURN || Entry.Type == InventoryJournalType.SALES)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null)
                                                        {
                                                            if (IsBatch && StockLedger.OpeningStockforBatch.Count > 0)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.StockMovementId == Entry.Id);
                                                                    if (Details != null)
                                                                    {
                                                                        if (Details.Uom == Products.WholesaleUOM)
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= (Details.Quantity + Details.FreeQuantity);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= ((Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                           
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.PATIENT_USE)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null)
                                                        {

                                                            if (IsBatch && StockLedger.OpeningStockforBatch.Count > 0)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.StockMovementId == Entry.Id);
                                                                    if (Details != null)
                                                                    {
                                                                        if (Details.Uom == Products.WholesaleUOM)
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= (Details.Quantity + Details.FreeQuantity);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= ((Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.DAMAGED)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null)
                                                        {

                                                            if (IsBatch && StockLedger.OpeningStockforBatch.Count > 0)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.StockMovementId == Entry.Id);
                                                                    if (Details != null)
                                                                    {
                                                                        if (Details.Uom == Products.WholesaleUOM)
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= Details.Quantity;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                            {
                                                                                StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).OpeningStock -= (Details.Quantity / Products.RetailXFactor);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                }
                                            }

                                            //Stock Movement
                                            StockMovementInfo = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.MovementDate >= FromDate && x.MovementDate <= ToDate).ToList<StockMovement>();
                                            if (StockMovementInfo != null && StockMovementInfo.Count > 0)
                                            {
                                                foreach (StockMovement Entry in StockMovementInfo.OrderBy(x => x.MovementDate))
                                                {
                                                    if (Entry.Type == InventoryJournalType.ADJUSTMENT)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchAdjustQty += (Details.Quantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchAdjustQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity));
                                                                                StockLedger.BatchAdjustQty += StockLedgerLineItem.BatchAdjustQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {

                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity) / Products.RetailXFactor;
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchAdjustQty += (Details.Quantity) / Products.RetailXFactor;
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchAdjustQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) / Products.RetailXFactor);
                                                                                StockLedger.BatchAdjustQty += StockLedgerLineItem.BatchAdjustQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.PURCHASE)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPurchaseQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPurchaseQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchPurchaseQty += StockLedgerLineItem.BatchPurchaseQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {

                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor;
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPurchaseQty += (Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor;
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPurchaseQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity + x.FreeQuantity) / Products.RetailXFactor);
                                                                                StockLedger.BatchPurchaseQty += StockLedgerLineItem.BatchPurchaseQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.PURCASE_RETURN)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPurchaseRtnQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPurchaseRtnQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchPurchaseRtnQty += StockLedgerLineItem.BatchPurchaseRtnQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= ((Details.Quantity + Detail.FreeQuantity) / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPurchaseRtnQty += ((Details.Quantity + Detail.FreeQuantity) / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPurchaseRtnQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => (x.Quantity + x.FreeQuantity)) / Products.RetailXFactor);
                                                                                StockLedger.BatchPurchaseRtnQty += StockLedgerLineItem.BatchPurchaseRtnQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.SALES)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchSalesQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchSalesQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchSalesQty += StockLedgerLineItem.BatchSalesQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor;
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchSalesQty += (Details.Quantity + Details.FreeQuantity) / Products.RetailXFactor;
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchSalesQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity + x.FreeQuantity) / Products.RetailXFactor);
                                                                                StockLedger.BatchSalesQty += StockLedgerLineItem.BatchSalesQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    if (Entry.Type == InventoryJournalType.SALES_RETURN)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchSalesRtnQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchSalesRtnQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchSalesRtnQty += StockLedgerLineItem.BatchSalesRtnQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {

                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchSalesRtnQty += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchSalesRtnQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity + x.FreeQuantity) / Products.RetailXFactor);
                                                                                StockLedger.BatchSalesRtnQty += StockLedgerLineItem.BatchSalesRtnQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.STOCK_IN)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchStockMovedInQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchStockMovedInQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchStockMovedInQty += StockLedgerLineItem.BatchStockMovedInQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {

                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchStockMovedInQty += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchStockMovedInQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity + x.FreeQuantity) / Products.RetailXFactor);
                                                                                StockLedger.BatchStockMovedInQty += StockLedgerLineItem.BatchStockMovedInQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.STOCK_OUT)
                                                    {
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchStockMovedOutQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchStockMovedOutQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchStockMovedOutQty += StockLedgerLineItem.BatchStockMovedOutQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchStockMovedOutQty += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchStockMovedOutQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity / Products.RetailXFactor) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity / Products.RetailXFactor));
                                                                                StockLedger.BatchStockMovedOutQty += StockLedgerLineItem.BatchStockMovedOutQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.PATIENT_USE)
                                                    {
                                                        StockMovementPatientUse StockMovementPatientUse = Context.StockMovementPatientUse.FirstOrDefault(x => x.Id == Entry.Id);
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPatientConsumedQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPatientConsumedQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchPatientConsumedQty += StockLedgerLineItem.BatchPatientConsumedQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchPatientConsumedQty += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchPatientConsumedQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity / Products.RetailXFactor) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity / Products.RetailXFactor));
                                                                                StockLedger.BatchPatientConsumedQty += StockLedgerLineItem.BatchPatientConsumedQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                    else if (Entry.Type == InventoryJournalType.DAMAGED)
                                                    {
                                                        StockMovementPatientUse StockMovementPatientUse = Context.StockMovementPatientUse.FirstOrDefault(x => x.Id == Entry.Id);
                                                        IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == Products.Id).ToList<StockMovementDetail>();
                                                        if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                        {
                                                            if (IsBatch)
                                                            {
                                                                foreach (StockMovementDetail Detail in lStockMovementDetails.Where(x => x.ExpDate >= FromDate && x.ExpDate <= ToDate && (x.Quantity + x.FreeQuantity) > 0))
                                                                {
                                                                    IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Products.Id, LocationId);
                                                                    foreach (InventoryBatch Batch in lInventoryBatchs)
                                                                    {
                                                                        StockMovementDetail Details = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo && x.Id == Detail.Id);
                                                                        if (Details != null)
                                                                        {
                                                                            if (Details.Uom == Products.WholesaleUOM)
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity + Details.FreeQuantity);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchDamagedQty += (Details.Quantity + Details.FreeQuantity);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchDamagedQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity));
                                                                                StockLedger.BatchDamagedQty += StockLedgerLineItem.BatchDamagedQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo) != null)
                                                                                {
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).ClosingStock -= (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                    StockLedger.OpeningStockforBatch.FirstOrDefault(x => x.BatchNo == Details.BatchNo).BatchDamagedQty += (Details.Quantity / Products.RetailXFactor + Details.FreeQuantity / Products.RetailXFactor);
                                                                                }
                                                                                StockLedgerLineItem StockLedgerLineItem = new StockLedgerLineItem();
                                                                                StockLedgerLineItem.BatchDamagedQty = (lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.Quantity / Products.RetailXFactor) + lStockMovementDetails.Where(x => x.Id == Details.Id).Sum(x => x.FreeQuantity / Products.RetailXFactor));
                                                                                StockLedger.BatchDamagedQty += StockLedgerLineItem.BatchDamagedQty;
                                                                                StockLedger.LineItems.Add(StockLedgerLineItem);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            
                                                        }
                                                    }
                                                }
                                            }
                                            if (IsBatch)
                                            {
                                                StockLedger.CloseingStock = (StockLedger.OpeningStock + StockLedger.BatchPurchaseQty - StockLedger.BatchPurchaseRtnQty - StockLedger.BatchSalesQty + StockLedger.BatchSalesRtnQty - StockLedger.BatchStockMovedOutQty + StockLedger.BatchStockMovedInQty - StockLedger.BatchPatientConsumedQty - StockLedger.BatchDamagedQty + StockLedger.BatchAdjustQty);
                                            }                                         
                                            _StockLedger.Add(StockLedger);

                                            // ======================  ################  ====================
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}














public class StockLedger
    {
        public InventoryLocation Location { get; set; }
        public double OpeningStock { get; set; }        
        public String MaterialId { get; set; }
        public String Name { get; set; }
        public String uom { get; set; }
        public bool Batch { get; set; }
        public double PurchaseQty { get; set; }        
        public double SalesQty { get; set; }
        public double CloseingStock { get; set; }
        public double CloseingValue { get; set; }
        public double PurchaseRate { get; set; }
        public double PurchaseRtnQty { get; set; }
        public double SalesRtnQty { get; set; }
        public double StockMovedInQty { get; set; }
        public double StockMovedOutQty { get; set; }
        public double PatientConsumedQty { get; set; }
        public double DamagedQty { get; set; }
        public double AdjustQty { get; set; }
        public double LMSQty { get; set; }
        public double BatchSalesQty { get; set; }
        public double BatchPurchaseQty { get; set; }
        public double BatchPurchaseRtnQty { get; set; }
        public double BatchSalesRtnQty { get; set; }
        public double BatchStockMovedInQty { get; set; }
        public double BatchStockMovedOutQty { get; set; }
        public double BatchPatientConsumedQty { get; set; }
        public double BatchDamagedQty { get; set; }
        public double BatchAdjustQty { get; set; }
        public double BatchLMSQty { get; set; }
        public List<BatchOpeningStock> OpeningStockforBatch { get; set; } = new List<BatchOpeningStock>();
        public List<StockLedgerLineItem> LineItems { get; set; } = new List<StockLedgerLineItem>();
        public String RackNumber { get; set; }
        public String Supplier { get; set; }
        public String Manufacturer { get; set; }
        public String CatType { get; set; }
        public String CatPFType { get; set; }

    }
    public class BatchOpeningStock
    {
        public InventoryLocation Location { get; set; }
        public DateTime ExpDate { get; set; }
        public String BatchNo { get; set; }
        public double BatchPurchaseRtnQty { get; set; }
        public double BatchSalesQty { get; set; }
        public double BatchPurchaseQty { get; set; }
        public double BatchSalesRtnQty { get; set; }
        public double BatchStockMovedInQty { get; set; }
        public double BatchStockMovedOutQty { get; set; }
        public double BatchPatientConsumedQty { get; set; }
        public double BatchDamagedQty { get; set; }
        public double BatchAdjustQty { get; set; }
        public double BatchLMSQty { get; set; }
        private double _OpeningStock = 0.00;
        public double OpeningStock
        {
            get
            {
                return _OpeningStock;
            }
            set
            {
                _OpeningStock = value;
                ClosingStock = _OpeningStock;
            }
        }
        public double ClosingStock { get; set; }
        public String RackNumber { get; set; }

    }
    public class StockLedgerLineItem
    {
        public DateTime Date { get; set; }       
        public double PurchaseQty { get; set; }
        public double SalesQty { get; set; }
        public double PurchaseRtnQty { get; set; }
        public double SalesRtnQty { get; set; }
        public double StockMovedInQty { get; set; }
        public double StockMovedOutQty { get; set; }
        public double PatientConsumedQty { get; set; }
        public double DamagedQty { get; set; }
        public double AdjustQty { get; set; }
        public double LMSQty { get; set; }
        public double BatchSalesQty { get; set; }
        public double BatchPurchaseQty { get; set; }
        public double BatchPurchaseRtnQty { get; set; }
        public double BatchSalesRtnQty { get; set; }
        public double BatchStockMovedInQty { get; set; }
        public double BatchStockMovedOutQty { get; set; }
        public double BatchPatientConsumedQty { get; set; }
        public double BatchDamagedQty { get; set; }
        public double BatchAdjustQty { get; set; }
        public double BatchLMSQty { get; set; }

    }
}
