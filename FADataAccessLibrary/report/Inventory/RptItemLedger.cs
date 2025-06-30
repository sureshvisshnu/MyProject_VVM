using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Catalog;
using fa.model.OrderManagement;
using FaData.Utils;
using FADataAccessLibrary.Utils;
using Microsoft.EntityFrameworkCore;

namespace fa.report.Inventory
{
    public class RptItemLedger : Report
    {
        public long[] LocationIds { get; set; }
        public long ItemId { get; set; }
        public long[] ItemIds { get; set; }
        public bool LabeledBatchWise { get; set; }
        public bool IsAllLocation { get; set; }
        public bool IsShowValue { get; set; }
        List<ItemLedger> _ItemLedger = new List<ItemLedger>();
        public string ReportHeader { get; set; }
        public IList<ItemLedger> ItemLedger
        {
            get
            {
                if (IsAllLocation)
                {
                    if (_ItemLedger.Count > 0)
                    {
                        List<ItemLedger> _lItemLedger = new List<ItemLedger>();
                        ItemLedger lItemLedger = new ItemLedger();
                        lItemLedger.OpeningStock = _ItemLedger.Sum(x => x.OpeningStock);
                        foreach (ItemLedger Ledger in _ItemLedger)
                        {
                            lItemLedger.LineItems.AddRange(Ledger.LineItems);
                        }
                        _lItemLedger.Add(lItemLedger);
                        return _lItemLedger;
                    }
                    return _ItemLedger;
                }
                else
                {
                    return _ItemLedger;
                }
            }
        }
        public string ReportDate()
        {
            return String.Format("{0} to {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportTitle()
        {
            return "Item Ledger";
        }

        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Item Ledger {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public static double CalculateValue(double Qty, double prize)
        {
            return (Qty * prize);
        }
        public static double LoadClosingStock(double ClosingStock, double Qty, string Type)
        {
            if (Type == "Purchase" || Type == "Sale Return" || Type == "Stock In" || Type == "Opening Stock" || Type == "Stock Adjustment")
            {
                return (ClosingStock + Qty);
            }
            if (Type == "Sale" || Type == "Purchase Return" || Type == "Stock Out" || Type == "To Patient" || Type == "Damage")
            {
                return (ClosingStock - Qty);
            }
            return ClosingStock;
        }
        public static string Sign(double Qty, string Type,int QuantityPricision)
        {
            if (Type == "Sale" || Type == "Purchase Return" || Type == "Stock Out" || Type == "To Patient" || Type == "Damage" || (Type == "Stock Adjustment" && Qty < 0))
            {
                return ("(" + Math.Abs(Qty).ToString(TextUtils.DecimalPlace(QuantityPricision)) + ")");
            }
            else
            {
                return (Qty.ToString(TextUtils.DecimalPlace(QuantityPricision)));
            }
        }
        public static string Sign(double Qty, int QuantityPricision)
        {
            if (Qty < 0)
            {
                return ("(" + Math.Abs(Qty).ToString(TextUtils.DecimalPlace(QuantityPricision)) + ")");
            }
            else
            {
                return (Qty.ToString(TextUtils.DecimalPlace(QuantityPricision)));
            }
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    foreach (long Items in ItemIds)
                    {
                        ItemId = Items;
                        Product Product = CatalogProductManager.Instance.GetProductInfoById(Items);
                        Boolean IsBatch = (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && LabeledBatchWise) ? true : false;
                        if (Product != null)
                        {
                            foreach (long LocationId in LocationIds)
                            {
                                InventoryLocation InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(LocationId);
                                if (InventoryLocation != null)
                                {
                                    ItemLedger ItemLedger = new ItemLedger();
                                    ItemLedger.CostPrice = Product.CostPrice;
                                    ItemLedger.Location = InventoryLocation;
                                    ItemLedger.ItemId = Product.Id;
                                    ItemLedger.ItemName = Product.Name;
                                    ItemLedger.RackNumber = Product.RackNumber!=null?Product.RackNumber:string.Empty;
                                    fa.model.OrderManagement.Inventory Inventory = Context.Inventories.FirstOrDefault(x => x.ProductId == Product.Id && x.InventoryLocationId == InventoryLocation.Id);
                                    if (Inventory != null)
                                    {
                                        //OPENING STOCK
                                        if (IsBatch)
                                        {
                                            ItemLedger.OpeningStockBatchWise = new List<BatchWiseOpeningStock>();

                                            IList<InventoryBatch> lBatch = Context.InventoryBatches.Where(x => x.InventoryId == Inventory.Id).ToList();
                                            foreach (InventoryBatch Batch in lBatch)
                                            {
                                                BatchWiseOpeningStock BatchWiseOpeningStock = new BatchWiseOpeningStock();
                                                BatchWiseOpeningStock.BatchNo = Batch.BatchNo;
                                                BatchWiseOpeningStock.Uom = Batch.StockUOM != null ? Batch.StockUOM : Batch.WholesaleUOM;
                                                BatchWiseOpeningStock.ExpDate = Batch.ExpDate;
                                                BatchWiseOpeningStock.CostPrice = Batch.Cost;
                                                BatchWiseOpeningStock.ItemId = Batch.ProductId;
                                                ItemLedger.OpeningStockBatchWise.Add(BatchWiseOpeningStock);
                                            }
                                        }
                                        IList<StockMovement> llStockMovement = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.Type == InventoryJournalType.OPEN_STOCK && x.MovementDate <= ToDate).ToList<StockMovement>();
                                        if (llStockMovement != null && llStockMovement.Count > 0)
                                        {
                                            foreach (StockMovement Entry in llStockMovement.OrderBy(x => x.MovementDate))
                                            {
                                                IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                if (lStockMovementDetails != null)
                                                {
                                                    foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                    {
                                                        double stock = Detail.Quantity;
                                                        ItemLedger.OpeningStock += Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                        ItemLedger.Uom = Product.RetailUOM == Detail.Uom ? Product.RetailUOM : Product.WholesaleUOM;
                                                    }
                                                    if (IsBatch && ItemLedger.OpeningStockBatchWise.Count > 0)
                                                    {
                                                        foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                        {
                                                            if (ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                            {
                                                                double stock = Detail.Quantity;
                                                                ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo).OpeningStock += Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;                                                                
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //From Stock Movement
                                        IList<StockMovement> lStockMovement = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.MovementDate < FromDate).ToList<StockMovement>();
                                        if (lStockMovement != null && lStockMovement.Count > 0)
                                        {
                                            foreach (StockMovement Entry in lStockMovement.OrderBy(x => x.MovementDate))
                                            {
                                                if (Entry.Type == InventoryJournalType.STOCK_OUT || Entry.Type == InventoryJournalType.PURCASE_RETURN || Entry.Type == InventoryJournalType.SALES || Entry.Type == InventoryJournalType.DAMAGED)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                        {
                                                            double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                            ItemLedger.OpeningStock -= Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                            ItemLedger.Uom = Product.RetailUOM == Detail.Uom ? Product.RetailUOM : Product.WholesaleUOM;
                                                        }
                                                        if (IsBatch && ItemLedger.OpeningStockBatchWise.Count > 0)
                                                        {
                                                            foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                            {
                                                                if (ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                {
                                                                    double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                                    ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo).OpeningStock -= Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.STOCK_IN || Entry.Type == InventoryJournalType.PURCHASE || Entry.Type == InventoryJournalType.SALES_RETURN || Entry.Type == InventoryJournalType.ADJUSTMENT)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                        {
                                                            double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                            ItemLedger.OpeningStock += Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                            ItemLedger.Uom = Product.RetailUOM == Detail.Uom ? Product.RetailUOM : Product.WholesaleUOM;
                                                        }
                                                        if (IsBatch && ItemLedger.OpeningStockBatchWise.Count > 0)
                                                        {
                                                            foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                            {
                                                                if (ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                {
                                                                    double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                                    ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo).OpeningStock += Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.PATIENT_USE)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                        {
                                                            double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                            ItemLedger.OpeningStock -= Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                            ItemLedger.Uom = Product.RetailUOM == Detail.Uom ? Product.RetailUOM : Product.WholesaleUOM;
                                                        }
                                                        if (IsBatch && ItemLedger.OpeningStockBatchWise.Count > 0)
                                                        {
                                                            foreach (StockMovementDetail Detail in lStockMovementDetails)
                                                            {
                                                                if (ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo) != null)
                                                                {
                                                                    double stock = (Detail.Quantity + Detail.FreeQuantity);
                                                                    ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == Detail.BatchNo).OpeningStock -= Product.RetailUOM == Detail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        ItemLedger.LineItems = new List<ItemLedgerLineItem>();
                                        bool HaveDetail = true;
                                        //From Stock Movement
                                        lStockMovement = Context.StockMovement.Where(x => x.InventoryStockLocationId == LocationId && x.MovementDate >= FromDate && x.MovementDate <= ToDate).ToList<StockMovement>();
                                        if (lStockMovement != null && lStockMovement.Count > 0)
                                        {
                                            foreach (StockMovement Entry in lStockMovement.OrderBy(x => x.MovementDate))
                                            {
                                                if (Entry.Type == InventoryJournalType.PURCHASE)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {
                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.Uom = Batch.WholesaleUOM;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null? Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Purchase";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementPurchase StockMovementPurchase = Context.StockMovementPurchase.Include("PurchaseEntry").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementPurchase != null && StockMovementPurchase.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.ToLocation = StockMovementPurchase.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + StockMovementPurchase.PurchaseEntry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Purchase";
                                                            ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            StockMovementPurchase StockMovementPurchase = Context.StockMovementPurchase.Include("PurchaseEntry").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementPurchase != null && StockMovementPurchase.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.ToLocation = StockMovementPurchase.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + StockMovementPurchase.PurchaseEntry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.PURCASE_RETURN)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {

                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Batch.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock -= Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Purchase Return";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementPurchaseReturn StockMovementPurchaseReturn = Context.StockMovementPurchaseReturn.Include("PurchaseReturnEntry").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementPurchaseReturn != null && StockMovementPurchaseReturn.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.ToLocation = StockMovementPurchaseReturn.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + StockMovementPurchaseReturn.PurchaseReturnEntry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Purchase Return";
                                                            ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            StockMovementPurchaseReturn StockMovementPurchaseReturn = Context.StockMovementPurchaseReturn.Include("PurchaseReturnEntry").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementPurchaseReturn != null && StockMovementPurchaseReturn.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.ToLocation = StockMovementPurchaseReturn.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + StockMovementPurchaseReturn.PurchaseReturnEntry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.SALES)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {

                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock -= Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Sale";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementSales StockMovementSales = Context.StockMovementSales.Include("Sale").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementSales != null && StockMovementSales.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.FromLocation = StockMovementSales.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + StockMovementSales.Sale.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Sale";
                                                            StockMovementSales StockMovementSales = Context.StockMovementSales.Include("Sale").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementSales != null && StockMovementSales.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.FromLocation = StockMovementSales.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + StockMovementSales.Sale.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.SALES_RETURN)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {
                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Sale Return";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementSalesReturn StockMovementSalesReturn = Context.StockMovementSalesReturn.Include("SaleReturn").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementSalesReturn != null && StockMovementSalesReturn.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.FromLocation = StockMovementSalesReturn.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + StockMovementSalesReturn.SaleReturn.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Sale Return";
                                                            StockMovementSalesReturn StockMovementSalesReturn = Context.StockMovementSalesReturn.Include("SaleReturn").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementSalesReturn != null && StockMovementSalesReturn.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.FromLocation = StockMovementSalesReturn.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + StockMovementSalesReturn.SaleReturn.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.STOCK_IN)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {
                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.CostPrice = Product.CostPrice;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Stock In";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementIn StockMovementIn = Context.StockMovementIn.Include("InventoryLocationFrom").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementIn != null && StockMovementIn.InventoryLocationFrom != null)
                                                                        {
                                                                            ItemLedgerLineItem.FromLocation = StockMovementIn.InventoryLocationFrom.Name;
                                                                        }
                                                                        ItemLedgerLineItem.ToLocation = InventoryLocation.Name;
                                                                        ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Stock In";
                                                            StockMovementIn StockMovementIn = Context.StockMovementIn.Include("InventoryLocationFrom").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementIn != null && StockMovementIn.InventoryLocationFrom != null)
                                                            {
                                                                ItemLedgerLineItem.FromLocation = StockMovementIn.InventoryLocationFrom.Name;
                                                            }
                                                            ItemLedgerLineItem.ToLocation = InventoryLocation.Name;
                                                            ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.STOCK_OUT)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {

                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock -= Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Stock Out";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementOut StockMovementOut = Context.StockMovementOut.Include("InventoryLocationTo").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementOut != null && StockMovementOut.InventoryLocationTo != null)
                                                                        {
                                                                            ItemLedgerLineItem.ToLocation = StockMovementOut.InventoryLocationTo.Name;
                                                                        }
                                                                        ItemLedgerLineItem.FromLocation = InventoryLocation.Name;
                                                                        ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Stock Out";
                                                            StockMovementOut StockMovementOut = Context.StockMovementOut.Include("InventoryLocationTo").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementOut != null && StockMovementOut.InventoryLocationTo != null)
                                                            {
                                                                ItemLedgerLineItem.ToLocation = StockMovementOut.InventoryLocationTo.Name;
                                                            }
                                                            ItemLedgerLineItem.FromLocation = InventoryLocation.Name; ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.ADJUSTMENT)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {
                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.CostPrice = Product.CostPrice;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = lDetail.Quantity;
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Stock Adjustment";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovement StockMovement = Context.StockMovement.Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovement != null && StockMovement.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.FromLocation = StockMovement.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = lDetail.Quantity;
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Stock Adjustment";
                                                            StockMovement StockMovement = Context.StockMovement.Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovement != null && StockMovement.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.FromLocation = StockMovement.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.DAMAGED)
                                                {
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {

                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = lDetail.Quantity;
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock -= Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "Damage";
                                                                    if (HaveDetail)
                                                                    {
                                                                        StockMovementDamaged StockMovementDamaged = Context.StockMovementDamaged.Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                                        if (StockMovementDamaged != null && StockMovementDamaged.InventoryStockLocation != null)
                                                                        {
                                                                            ItemLedgerLineItem.FromLocation = StockMovementDamaged.InventoryStockLocation.Name;
                                                                        }
                                                                        ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "Damage";
                                                            StockMovementDamaged StockMovementDamaged = Context.StockMovementDamaged.Include("InventoryStockLocation").FirstOrDefault(x => x.Id == Entry.Id);
                                                            if (StockMovementDamaged != null && StockMovementDamaged.InventoryStockLocation != null)
                                                            {
                                                                ItemLedgerLineItem.FromLocation = StockMovementDamaged.InventoryStockLocation.Name;
                                                            }
                                                            ItemLedgerLineItem.Description = "Ref# " + Entry.RefNumber;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                                else if (Entry.Type == InventoryJournalType.PATIENT_USE)
                                                {
                                                    StockMovementPatientUse StockMovementPatientUse = Context.StockMovementPatientUse.FirstOrDefault(x => x.Id == Entry.Id);
                                                    IList<StockMovementDetail> lStockMovementDetails = Context.StockMovementDetail.Include("Product").Where(x => x.StockMovementId == Entry.Id && x.ProductId == ItemId).ToList<StockMovementDetail>();
                                                    if (lStockMovementDetails != null && lStockMovementDetails.Count > 0)
                                                    {
                                                        if (IsBatch)
                                                        {
                                                            HaveDetail = true;
                                                            IList<InventoryBatch> lInventoryBatchs = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(Product.Id, LocationId);
                                                            foreach (InventoryBatch Batch in lInventoryBatchs)
                                                            {
                                                                StockMovementDetail Detail = lStockMovementDetails.FirstOrDefault(x => x.BatchNo == Batch.BatchNo);
                                                                if (Detail != null)
                                                                {
                                                                    ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                                    ItemLedgerLineItem.Date = Entry.MovementDate;
                                                                    ItemLedgerLineItem.BatchDetail = Detail.BatchNo;
                                                                    ItemLedgerLineItem.ExpDate = Detail.ExpDate;
                                                                    ItemLedgerLineItem.ItemName = Batch.Product.Name;
                                                                    ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                                    ItemLedgerLineItem.RackNumber = Batch.Product.RackNumber!=null?Batch.Product.RackNumber:string.Empty;
                                                                    ItemLedgerLineItem.ItemId = Batch.ProductId;
                                                                    foreach (StockMovementDetail lDetail in lStockMovementDetails.Where(x => x.BatchNo == Batch.BatchNo))
                                                                    {
                                                                        double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                        ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                        ItemLedger.OpeningStockBatchWise.FirstOrDefault(x => x.BatchNo == lDetail.BatchNo).ClosingStock -= Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                    }
                                                                    ItemLedgerLineItem.TransactionType = "To Patient";
                                                                    if (HaveDetail)
                                                                    {
                                                                        ItemLedgerLineItem.FromLocation = InventoryLocation.Name;
                                                                        ItemLedgerLineItem.Description = "Patient " + StockMovementPatientUse.Patient.Name + "\n" + Entry.RefNumber;
                                                                    }
                                                                    HaveDetail = false;
                                                                    ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ItemLedgerLineItem ItemLedgerLineItem = new ItemLedgerLineItem();
                                                            ItemLedgerLineItem.Date = Entry.MovementDate;
                                                            foreach (StockMovementDetail lDetail in lStockMovementDetails)
                                                            {
                                                                ItemLedgerLineItem.RackNumber = lDetail.Product.RackNumber != null ? lDetail.Product.RackNumber : string.Empty;
                                                                double stock = (lDetail.Quantity + lDetail.FreeQuantity);
                                                                ItemLedgerLineItem.Qty += Product.RetailUOM == lDetail.Uom ? (stock / Product.RetailXFactor) : stock;
                                                                ItemLedgerLineItem.Uom = Product.WholesaleUOM;
                                                            }
                                                            ItemLedgerLineItem.TransactionType = "To Patient";
                                                            ItemLedgerLineItem.FromLocation = InventoryLocation.Name;
                                                            ItemLedgerLineItem.Description = "Patient " + StockMovementPatientUse.Patient.Name + "\n" + Entry.RefNumber;
                                                            ItemLedgerLineItem.CostPrice = Product.CostPrice;
                                                            ItemLedgerLineItem.ItemId = Product.Id;
                                                            ItemLedgerLineItem.ItemName = Product.Name;
                                                            ItemLedgerLineItem.RackNumber = Product.RackNumber!=null? Product.RackNumber:string.Empty;
                                                            ItemLedger.LineItems.Add(ItemLedgerLineItem);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        _ItemLedger.Add(ItemLedger);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class ItemLedger
    {
        public InventoryLocation Location { get; set; }
        public double OpeningStock { get; set; }
        public bool IsBatch { get; set; }
        public double CostPrice { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string RackNumber { get; set; }
        public string Uom { get; set; }

        public List<BatchWiseOpeningStock> OpeningStockBatchWise { get; set; } = new List<BatchWiseOpeningStock>();
        public List<ItemLedgerLineItem> LineItems { get; set; } = new List<ItemLedgerLineItem>();
    }
    public class BatchWiseOpeningStock
    {
        public DateTime ExpDate { get; set; }
        public String BatchNo { get; set; }
        public String Uom { get; set; }
        public double CostPrice { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
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

    }
    public class ItemLedgerLineItem
    {
        public DateTime Date { get; set; }
        public DateTime ExpDate { get; set; }
        public String Description { get; set; }
        public String BatchDetail { get; set; }
        public String Uom { get; set; }
        public double Qty { get; set; }
        public String TransactionType { get; set; }
        public String FromLocation { get; set; }
        public String ToLocation { get; set; }
        public double Stock { get; set; }
        public double CostPrice { get; set; }
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string RackNumber { get; set; }

    }
}
