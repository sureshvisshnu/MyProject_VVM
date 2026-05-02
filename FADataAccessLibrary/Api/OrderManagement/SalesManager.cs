using fa.context;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using fa.api.accounting.doubleentry;
using Fa.api.Accounting.DoubleEntry;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.hms.common;
using fa.api.System;
using Fa.api.OrderManagement;
using Microsoft.EntityFrameworkCore;

namespace fa.api.OrderManagement
{
    public class SalesManager
    {
        private static volatile SalesManager instance;
        private static object syncRoot = new Object();
        SalesManager()
        {

        }
        public static SalesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SalesManager();
                    }
                }
                return instance;
            }
        }
        public void ApplyPayment(ReceiptDetail ReceiptDetail, AccountMasterContext Context, List<SaleEntry> SaleEntry)
        {
            if (!string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
            {
                SaleEntry lSalesEntry = GetSaleEntry(long.Parse(ReceiptDetail.ReferenceTrasnactionId));
                if (SaleEntry != null)
                {
                    foreach (SaleEntry saleEntry in SaleEntry)
                    {
                        if (saleEntry.Id == lSalesEntry.Id)
                        {
                            lSalesEntry.Balance = saleEntry.Balance;
                            lSalesEntry.Paid = saleEntry.Paid;
                            break;
                        }
                    }
                }
                if (lSalesEntry.Balance >= (double)ReceiptDetail.Amount)
                {
                    lSalesEntry.Paid += (double)ReceiptDetail.Amount;
                    lSalesEntry.Balance -= (double)ReceiptDetail.Amount;
                    UpdateSaleEntryForApplayAndReversePayment(lSalesEntry, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + ReceiptDetail.ReferenceTrasnactionId);
            }
        }
        public void ReversePayment(ReceiptDetail ReceiptDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
            {
                SaleEntry lSalesEntry = GetSaleEntry(long.Parse(ReceiptDetail.ReferenceTrasnactionId));
                if (lSalesEntry != null && ReceiptDetail.Amount > 0)
                {
                    lSalesEntry.Paid -= (double)ReceiptDetail.Amount;
                    lSalesEntry.Balance += (double)ReceiptDetail.Amount;
                    UpdateSaleEntryForApplayAndReversePayment(lSalesEntry, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + ReceiptDetail.ReferenceTrasnactionId);
            }
        }
        private void UpdateSaleEntryForApplayAndReversePayment(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            SaleEntry SaleEntryInfo = null;
            try
            {
                SaleEntryInfo = Context.SaleEntry.Find(SaleEntry.Id);
                if (SaleEntryInfo != null)
                {
                    SaleEntry.SaleEntryId = SaleEntryInfo.SaleEntryId;
                    SaleEntry SaleEntryInfoFromDB = GetSaleEntry(SaleEntry.Id);
                    if (SaleEntryInfoFromDB.SaleDetails.Count > 0)
                    {
                        foreach (SaleDetail Detail in SaleEntryInfoFromDB.SaleDetails)
                        {
                            Context.ItemLevelSaleTaxDetails.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleTaxDetails.Remove(p));
                            Context.SaveChanges();

                            Context.ItemLevelSaleDiscounts.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleDiscounts.Remove(p));
                            Context.SaveChanges();
                        }
                    }
                    if (SaleEntryInfoFromDB.SaleAdditionalTransactions.Count > 0)
                    {
                        Context.SaleAdditionalTransactions.Where(p => p.SaleEntryId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.SaleAdditionalTransactions.Remove(p));
                        Context.SaveChanges();
                    }
                    if (SaleEntryInfoFromDB.Discounts.Count > 0)
                    {
                        Context.OrderLevelSaleDiscounts.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleDiscounts.Remove(p));
                        Context.SaveChanges();
                    }
                    if (SaleEntryInfoFromDB.TaxDetails.Count > 0)
                    {
                        Context.OrderLevelSaleTaxDetails.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleTaxDetails.Remove(p));
                        Context.SaveChanges();
                    }
                    // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
                    SaleEntry.Account = null;
                    SaleEntry.SalePayment = null;
                    SaleEntry.SalePaymentNew = null;
                    Context.Entry(SaleEntryInfo).CurrentValues.SetValues(SaleEntry);
                    Context.SaveChanges();
                    ReceiptManager.Instance.salesEntry.Add(SaleEntryInfo);
                    foreach (SaleDetail OldDetail in SaleEntryInfoFromDB.SaleDetails)
                    {
                        SaleDetail NewDetail = SaleEntry.SaleDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                        if (NewDetail == null)
                        {
                            Context.SaleDetail.Remove(Context.SaleDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                            OldDetail.FreeQuantity = 0;
                            OldDetail.Quantity = 0;
                        }
                        else
                        {
                            SaleEntry.SaleDetails.Remove(NewDetail);
                            NewDetail.SaleId = SaleEntry.Id;
                            SaleDetail SaleDetail = Context.SaleDetail.Find(NewDetail.Id);
                            SaleDetail.Sale = null;
                            Context.Entry(SaleDetail).CurrentValues.SetValues(NewDetail/*.TaxDetails*/);
                            Context.SaveChanges();
                            foreach (ItemLevelSaleDiscount Detail in NewDetail.Discounts)
                            {
                                Detail.SaleDetails = null;
                                Detail.SaleDetailsId = SaleDetail.Id;
                                Context.ItemLevelSaleDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (ItemLevelSaleTaxDetail Detail in NewDetail.TaxDetails)
                            {
                                Detail.SaleDetails = null;
                                Detail.SaleDetailsId = SaleDetail.Id;
                                Context.ItemLevelSaleTaxDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                        }
                    }
                    foreach (SaleDetail Detail in SaleEntry.SaleDetails)
                    {
                        Detail.SaleId = SaleEntry.Id;
                        Context.SaleDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (SaleAdditionalTransaction Detail in SaleEntry.SaleAdditionalTransactions)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleEntryId = SaleEntry.Id;
                        Context.SaleAdditionalTransactions.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelSaleDiscount Detail in SaleEntry.Discounts)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleId = SaleEntry.Id;
                        Context.OrderLevelSaleDiscounts.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelSaleTaxDetail Detail in SaleEntry.TaxDetails)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleId = SaleEntry.Id;
                        Context.OrderLevelSaleTaxDetails.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                SaleEntryInfo = null;
                throw (e);
            }
        }
        public void RecordStockMovementSaleInSaleEntry(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            Company Company = CompanyManager.Instance.GetCompany(SaleEntry.CompanyId);
            if (Company != null)
            {
                if (SaleEntry.EntryType == Entrytype.SALE)
                {
                    StockMovementSales StockMovementSalesFDB = Context.StockMovementSales.FirstOrDefault(x => x.SaleId == SaleEntry.Id);
                    StockMovementSales StockMovementSales = new StockMovementSales();
                    StockMovementSales.CompanyId = SaleEntry.CompanyId;
                    StockMovementSales.CostCenterId = SaleEntry.CostCenterId;
                    StockMovementSales.Id = StockMovementSalesFDB == null ? 0L : StockMovementSalesFDB.Id;
                    StockMovementSales.InventoryStockLocationId = (long)SaleEntry.InventoryLocationId;
                    StockMovementSales.MovementDate = SaleEntry.SaleDate;
                    StockMovementSales.SaleId = SaleEntry.Id;
                    StockMovementSales.WorkStationName = SaleEntry.WorkStationName;
                    StockMovementSales.WorkStationId = SaleEntry.WorkStationId;
                    StockMovementSales.RefNumber = StockMovementSalesFDB == null ? CompanyManager.Instance.GetIdSpace(Company, EntryType.STOCK_SALE, SaleEntry.SaleDate) : StockMovementSalesFDB.RefNumber;
                    foreach (SaleDetail details in SaleEntry.SaleDetails)
                    {
                        StockMovementDetail StockMovementDetail = new StockMovementDetail();
                        StockMovementDetail.BatchNo = details.BatchNo;
                        StockMovementDetail.CompanyId = details.CompanyId;
                        StockMovementDetail.CostCenterId = details.CostCenterId;
                        StockMovementDetail.ExpDate = details.ExpDate;
                        StockMovementDetail.Quantity = (details.Quantity + details.FreeQuantity);
                        StockMovementDetail.ProductId = details.ProductId;
                        StockMovementDetail.Uom = details.Uom;
                        StockMovementDetail.isBatch = details.isBatch;
                        StockMovementDetail.MaterialId = details.MaterialId;
                        StockMovementSales.StockMovementDetails.Add(StockMovementDetail);
                    }

                    if (StockMovementSales.Id == 0L)
                    {
                        StockMovementManager.Instance.AddStockMovementSale(StockMovementSales, Context);
                    }
                    else
                    {
                        StockMovementManager.Instance.UpdateStockMovementSale(StockMovementSales, Context);
                    }
                }
                else
                {
                    StockMovementSalesReturn StockMovementSalesFDB = Context.StockMovementSalesReturn.FirstOrDefault(x => x.SaleReturnId == SaleEntry.Id);
                    StockMovementSalesReturn StockMovementSales = new StockMovementSalesReturn();
                    StockMovementSales.CompanyId = SaleEntry.CompanyId;
                    StockMovementSales.CostCenterId = SaleEntry.CostCenterId;
                    StockMovementSales.Id = StockMovementSalesFDB == null ? 0L : StockMovementSalesFDB.Id;
                    StockMovementSales.InventoryStockLocationId = (long)SaleEntry.InventoryLocationId;
                    StockMovementSales.MovementDate = SaleEntry.SaleDate;
                    StockMovementSales.SaleReturnId = SaleEntry.Id;
                    StockMovementSales.WorkStationName = SaleEntry.WorkStationName;
                    StockMovementSales.WorkStationId = SaleEntry.WorkStationId;
                    StockMovementSales.RefNumber = StockMovementSalesFDB == null ? CompanyManager.Instance.GetIdSpace(Company, EntryType.STOCK_SALERETURN, SaleEntry.ReturnDate) : StockMovementSalesFDB.RefNumber;
                    foreach (SaleDetail details in SaleEntry.SaleDetails)
                    {
                        StockMovementDetail StockMovementDetail = new StockMovementDetail();
                        StockMovementDetail.BatchNo = details.BatchNo;
                        StockMovementDetail.CompanyId = details.CompanyId;
                        StockMovementDetail.CostCenterId = details.CostCenterId;
                        StockMovementDetail.ExpDate = details.ExpDate;
                        StockMovementDetail.Quantity = (details.Quantity + details.FreeQuantity);
                        StockMovementDetail.ProductId = details.ProductId;
                        StockMovementDetail.Uom = details.Uom;
                        StockMovementDetail.isBatch = details.isBatch;
                        StockMovementDetail.MaterialId = details.MaterialId;
                        StockMovementSales.StockMovementDetails.Add(StockMovementDetail);
                    }

                    if (StockMovementSales.Id == 0L)
                    {
                        StockMovementManager.Instance.AddStockMovementSaleReturn(StockMovementSales, Context);
                    }
                    else
                    {
                        StockMovementManager.Instance.UpdateStockMovementSaleReturn(StockMovementSales, Context);
                    }
                }
            }

        }

        public SaleEntry AddSaleEntry(SaleEntry saleEntry)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //add to inventory to list
                        if (saleEntry.EntryType == Entrytype.SALE || saleEntry.EntryType == Entrytype.RETURN)
                        {
                            if (saleEntry.EntryType == Entrytype.RETURN)
                            {
                                //lock sale entry generate return
                                SaleEntry lSalesEntry = GetSaleEntry((long)saleEntry.SaleEntryId);
                                lSalesEntry.isSaleLocked = true;
                                UpdateSaleEntry(lSalesEntry, Context);
                            }
                            InventoryLocationManager.Instance.RecordSales(saleEntry, Context);
                        }
                        Context.SaleEntry.Add(saleEntry);
                        Context.SaveChanges();
                        if (saleEntry.EntryType == Entrytype.SALE || saleEntry.EntryType == Entrytype.RETURN)
                        {
                            RecordStockMovementSaleInSaleEntry(saleEntry, Context);
                        }
                        if (saleEntry.NoteId != 0L && saleEntry.NoteId != null)
                        {
                            ConsultationNoteManager.Instance.UpdateNoteForPrescriptionSaleUpdate((long)saleEntry.NoteId, saleEntry.Id, Context);
                        }
                        //daybook
                        if (saleEntry.EntryType == Entrytype.SALE)
                        {
                            SaleDoubleEntryManager.Instance.RecordSale(saleEntry, Context);
                        }
                        if (saleEntry.EntryType == Entrytype.RETURN)
                        {
                            SaleReturnDoubleEntryManager.Instance.RecordSaleReturn(saleEntry, Context);
                        }
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        saleEntry = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return saleEntry;
        }
        public SaleEntry UpdateSaleEntry(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            SaleEntry SaleEntryInfo = null;

            try
            {
                SaleEntryInfo = Context.SaleEntry.Find(SaleEntry.Id);
                if (SaleEntryInfo != null)
                {
                    SaleEntry.SaleEntryId = SaleEntryInfo.SaleEntryId;
                    SaleEntry SaleEntryInfoFromDB = GetSaleEntry(SaleEntry.Id);
                    if (SaleEntryInfoFromDB.SaleDetails.Count > 0)
                    {
                        foreach (SaleDetail Detail in SaleEntryInfoFromDB.SaleDetails)
                        {
                            Context.ItemLevelSaleTaxDetails.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleTaxDetails.Remove(p));
                            Context.SaveChanges();

                            Context.ItemLevelSaleDiscounts.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleDiscounts.Remove(p));
                            Context.SaveChanges();
                        }

                    }

                    if (SaleEntryInfoFromDB.SaleAdditionalTransactions.Count > 0)
                    {
                        Context.SaleAdditionalTransactions.Where(p => p.SaleEntryId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.SaleAdditionalTransactions.Remove(p));
                        Context.SaveChanges();
                    }
                    if (SaleEntryInfoFromDB.Discounts.Count > 0)
                    {
                        Context.OrderLevelSaleDiscounts.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleDiscounts.Remove(p));
                        Context.SaveChanges();
                    }
                    if (SaleEntryInfoFromDB.TaxDetails.Count > 0)
                    {
                        Context.OrderLevelSaleTaxDetails.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleTaxDetails.Remove(p));
                        Context.SaveChanges();
                    }
                    // Changes made here CustomerId to AccountId and Customer to Account due to sales model change
                    SaleEntry.Account = null;
                    SaleEntry.SalePayment = null;
                    SaleEntry.SalePaymentNew = null;
                    Context.Entry(SaleEntryInfo).CurrentValues.SetValues(SaleEntry);

                    ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteBySaleId(SaleEntry.Id);
                    if (Note != null)
                    {
                        ConsultationNoteManager.Instance.UpdateNoteForPrescriptionSaleUpdate(Note.Id, SaleEntry.Id, Context);
                    }
                    //daybook
                    if (SaleEntry.EntryType == Entrytype.SALE)
                    {
                        SaleDoubleEntryManager.Instance.RecordSale(SaleEntry, Context);
                    }
                    if (SaleEntryInfo.EntryType == Entrytype.RETURN)
                    {
                        SaleReturnDoubleEntryManager.Instance.RecordSaleReturn(SaleEntry, Context);
                    }
                    foreach (SaleDetail OldDetail in SaleEntryInfoFromDB.SaleDetails)
                    {
                        SaleDetail NewDetail = SaleEntry.SaleDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                        if (NewDetail == null)
                        {
                            Context.SaleDetail.Remove(Context.SaleDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                            OldDetail.FreeQuantity = 0;
                            OldDetail.Quantity = 0;
                            //add to inventory
                            if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                            {
                                InventoryLocationManager.Instance.RecordSalesDetails(OldDetail, Context, (long)SaleEntryInfoFromDB.InventoryLocationId, SaleEntry.EntryType/*, SaleEntry.SaleType*/);
                            }

                        }
                        else
                        {
                            SaleEntry.SaleDetails.Remove(NewDetail);
                            NewDetail.SaleId = SaleEntry.Id;
                            SaleDetail SaleDetail = Context.SaleDetail.Find(NewDetail.Id);
                            SaleDetail.Sale = null;
                            Context.Entry(SaleDetail).CurrentValues.SetValues(NewDetail);
                            Context.SaveChanges();
                            foreach (ItemLevelSaleDiscount Detail in NewDetail.Discounts)
                            {
                                Detail.SaleDetails = null;
                                Detail.SaleDetailsId = SaleDetail.Id;
                                Context.ItemLevelSaleDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (ItemLevelSaleTaxDetail Detail in NewDetail.TaxDetails)
                            {
                                Detail.SaleDetails = null;
                                Detail.SaleDetailsId = SaleDetail.Id;
                                Context.ItemLevelSaleTaxDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            //add to inventory
                            if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                            {
                                InventoryLocationManager.Instance.RecordSalesDetails(NewDetail, Context, (long)SaleEntryInfo.InventoryLocationId, SaleEntryInfo.EntryType/*, SaleEntryInfo.SaleType*/);
                            }
                        }
                    }
                    if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                    {
                        if (SaleEntry.SaleDetails.Count > 0)
                        {
                            //add to inventory to list
                            InventoryLocationManager.Instance.RecordSales(SaleEntry, Context);
                        }
                    }
                    foreach (SaleDetail Detail in SaleEntry.SaleDetails)
                    {
                        Detail.SaleId = SaleEntry.Id;
                        Context.SaleDetail.Add(Detail);
                        Context.SaveChanges();
                    }

                    foreach (SaleAdditionalTransaction Detail in SaleEntry.SaleAdditionalTransactions)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleEntryId = SaleEntry.Id;
                        Context.SaleAdditionalTransactions.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelSaleDiscount Detail in SaleEntry.Discounts)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleId = SaleEntry.Id;
                        Context.OrderLevelSaleDiscounts.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelSaleTaxDetail Detail in SaleEntry.TaxDetails)
                    {
                        Detail.SaleEntry = null;
                        Detail.SaleId = SaleEntry.Id;
                        Context.OrderLevelSaleTaxDetails.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                SaleEntryInfo = null;
                throw (e);
            }
            return SaleEntryInfo;
        }
        public SaleEntry UpdateSaleEntry(SaleEntry SaleEntry)
        {
            SaleEntry SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SaleEntryInfo = Context.SaleEntry.Find(SaleEntry.Id);
                        if (SaleEntryInfo != null)
                        {
                            SaleEntry.SaleEntryId = SaleEntryInfo.SaleEntryId;
                            SaleEntry SaleEntryInfoFromDB = GetSaleEntry(SaleEntry.Id);
                            if (SaleEntryInfoFromDB.SaleDetails.Count > 0)
                            {
                                foreach (SaleDetail Detail in SaleEntryInfoFromDB.SaleDetails)
                                {
                                    Context.ItemLevelSaleTaxDetails.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleTaxDetails.Remove(p));
                                    Context.SaveChanges();

                                    Context.ItemLevelSaleDiscounts.Where(p => p.SaleDetailsId == Detail.Id).ToList().ForEach(p => Context.ItemLevelSaleDiscounts.Remove(p));
                                    Context.SaveChanges();
                                }

                            }

                            if (SaleEntryInfoFromDB.SaleAdditionalTransactions.Count > 0)
                            {
                                Context.SaleAdditionalTransactions.Where(p => p.SaleEntryId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.SaleAdditionalTransactions.Remove(p));
                                Context.SaveChanges();
                            }
                            if (SaleEntryInfoFromDB.Discounts.Count > 0)
                            {
                                Context.OrderLevelSaleDiscounts.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleDiscounts.Remove(p));
                                Context.SaveChanges();
                            }
                            if (SaleEntryInfoFromDB.TaxDetails.Count > 0)
                            {
                                Context.OrderLevelSaleTaxDetails.Where(p => p.SaleId == SaleEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelSaleTaxDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            // Changes made here CustomerId to AccountId and Customer to Account due to sales model change

                            SaleEntry.Account = null;
                            SaleEntry.SalePayment = null;
                            SaleEntry.SalePaymentNew = null;
                            Context.Entry(SaleEntryInfo).CurrentValues.SetValues(SaleEntry);
                            if (SaleEntry.EntryType == Entrytype.SALE || SaleEntry.EntryType == Entrytype.RETURN)
                            {
                                RecordStockMovementSaleInSaleEntry(SaleEntry, Context);
                            }
                            ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteBySaleId(SaleEntry.Id);
                            if (Note != null)
                            {
                                ConsultationNoteManager.Instance.UpdateNoteForPrescriptionSaleUpdate(Note.Id, SaleEntry.Id, Context);
                            }
                            //daybook
                            if (SaleEntry.EntryType == Entrytype.SALE)
                            {
                                SaleDoubleEntryManager.Instance.RecordSale(SaleEntry, Context);
                            }
                            if (SaleEntryInfo.EntryType == Entrytype.RETURN)
                            {
                                SaleReturnDoubleEntryManager.Instance.RecordSaleReturn(SaleEntry, Context);
                            }
                            foreach (SaleDetail OldDetail in SaleEntryInfoFromDB.SaleDetails)
                            {
                                SaleDetail NewDetail = SaleEntry.SaleDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.SaleDetail.Remove(Context.SaleDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                                    OldDetail.FreeQuantity = 0;
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                                    {
                                        InventoryLocationManager.Instance.RecordSalesDetails(OldDetail, Context, (long)SaleEntryInfoFromDB.InventoryLocationId, SaleEntry.EntryType/*, SaleEntry.SaleType*/);
                                    }
                                }
                                else
                                {
                                    SaleEntry.SaleDetails.Remove(NewDetail);
                                    NewDetail.SaleId = SaleEntry.Id;
                                    SaleDetail SaleDetail = Context.SaleDetail.Find(NewDetail.Id);
                                    SaleDetail.Sale = null;
                                    Context.Entry(SaleDetail).CurrentValues.SetValues(NewDetail/*.TaxDetails*/);
                                    Context.SaveChanges();
                                    foreach (ItemLevelSaleDiscount Detail in NewDetail.Discounts)
                                    {
                                        Detail.SaleDetails = null;
                                        Detail.SaleDetailsId = SaleDetail.Id;
                                        Context.ItemLevelSaleDiscounts.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                    foreach (ItemLevelSaleTaxDetail Detail in NewDetail.TaxDetails)
                                    {
                                        Detail.CatalogItemSalesTaxMap = null;
                                        Detail.SaleDetails = null;
                                        Detail.SaleDetailsId = SaleDetail.Id;
                                        Context.ItemLevelSaleTaxDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                    //add to inventory
                                    if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                                    {
                                        InventoryLocationManager.Instance.RecordSalesDetails(NewDetail, Context, (long)SaleEntryInfo.InventoryLocationId, SaleEntryInfo.EntryType/*,SaleEntryInfo.SaleType*/);
                                    }
                                }
                            }
                            if (SaleEntry.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                            {
                                if (SaleEntry.SaleDetails.Count > 0)
                                {
                                    //add to inventory to list
                                    InventoryLocationManager.Instance.RecordSales(SaleEntry, Context);
                                }
                            }
                            foreach (SaleDetail Detail in SaleEntry.SaleDetails)
                            {
                                Detail.SaleId = SaleEntry.Id;
                                Context.SaleDetail.Add(Detail);
                                Context.SaveChanges();
                            }

                            foreach (SaleAdditionalTransaction Detail in SaleEntry.SaleAdditionalTransactions)
                            {
                                Detail.SaleEntry = null;
                                Detail.SaleEntryId = SaleEntry.Id;
                                Context.SaleAdditionalTransactions.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (OrderLevelSaleDiscount Detail in SaleEntry.Discounts)
                            {
                                Detail.SaleEntry = null;
                                Detail.SaleId = SaleEntry.Id;
                                Context.OrderLevelSaleDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (OrderLevelSaleTaxDetail Detail in SaleEntry.TaxDetails)
                            {
                                Detail.SaleEntry = null;
                                Detail.SaleId = SaleEntry.Id;
                                Context.OrderLevelSaleTaxDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();

                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        SaleEntryInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return SaleEntryInfo;
        }


        public Boolean DeleteSaleEntry(long SaleEntryId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SaleEntry SaleEntryInfo = GetSaleEntry(SaleEntryId);
                        if (SaleEntryInfo.EntryType == Entrytype.SALE)
                        {
                            StockMovementManager.Instance.DeleteStockMovementSale(SaleEntryInfo.Id, Context);
                        }
                        if (SaleEntryInfo.EntryType == Entrytype.RETURN)
                        {
                            StockMovementManager.Instance.DeleteStockMovementSaleReturn(SaleEntryInfo.Id, Context);
                        }
                        //add to inventory to list while remove
                        if (SaleEntryInfo.EntryType == Entrytype.SALE || SaleEntryInfo.EntryType == Entrytype.RETURN)
                        {
                            InventoryLocationManager.Instance.RecordSalesInDelete(SaleEntryInfo, Context);
                            //daybook delete
                            if (SaleEntryInfo.EntryType == Entrytype.SALE)
                            {
                                SaleDoubleEntryManager.Instance.DeleteSale(SaleEntryInfo, Context);

                                //for consultation note
                                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteBySaleId(SaleEntryId);
                                if (Note != null)
                                {
                                    ConsultationNoteManager.Instance.UpdateNoteForPrescriptionSaleDelete(Note.Id, Context);
                                }
                            }
                            else
                            {
                                SaleReturnDoubleEntryManager.Instance.DeleteSaleReturns(SaleEntryInfo, Context);
                            }

                        }

                        if (SaleEntryInfo.SaleDetails.Count > 0)
                        {
                            foreach (SaleDetail detail in SaleEntryInfo.SaleDetails)
                            {
                                Context.ItemLevelSaleTaxDetails.Where(p => p.SaleDetailsId == detail.Id).ToList().ForEach(p => Context.ItemLevelSaleTaxDetails.Remove(p));
                                Context.ItemLevelSaleDiscounts.Where(p => p.SaleDetailsId == detail.Id).ToList().ForEach(p => Context.ItemLevelSaleDiscounts.Remove(p));
                            }
                            Context.SaleDetail.Where(p => p.SaleId == SaleEntryInfo.Id).ToList().ForEach(p => Context.SaleDetail.Remove(p));
                            Context.SaveChanges();
                        }
                        if (SaleEntryInfo.SaleAdditionalTransactions.Count > 0)
                        {
                            Context.SaleAdditionalTransactions.Where(p => p.SaleEntryId == SaleEntryInfo.Id).ToList().ForEach(p => Context.SaleAdditionalTransactions.Remove(p));
                            Context.SaveChanges();
                        }
                        if (SaleEntryInfo.Discounts.Count > 0)
                        {
                            Context.OrderLevelSaleDiscounts.Where(p => p.SaleId == SaleEntryInfo.Id).ToList().ForEach(p => Context.OrderLevelSaleDiscounts.Remove(p));
                            Context.SaveChanges();
                        }
                        if (SaleEntryInfo.TaxDetails.Count > 0)
                        {
                            Context.OrderLevelSaleTaxDetails.Where(p => p.SaleId == SaleEntryInfo.Id).ToList().ForEach(p => Context.OrderLevelSaleTaxDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        SaleEntry lSaleEntryInfo = Context.SaleEntry.Find(SaleEntryId);
                        Context.SaleEntry.Remove(lSaleEntryInfo);
                        Context.SaveChanges();

                        if (SaleEntryInfo.EntryType == Entrytype.RETURN)
                        {
                            //Unlock sale entry delete return
                            if (SaleEntryInfo.SaleEntryId != null)
                            {
                                IList<SaleEntry> SaleEntrys = Context.SaleEntry.Include("SaleDetails").Where(x => x.SaleEntryId == SaleEntryInfo.SaleEntryId).ToList<SaleEntry>();
                                if (SaleEntrys == null || SaleEntrys.Count == 0)
                                {
                                    SaleEntry lSalesEntry = GetSaleEntry((long)SaleEntryInfo.SaleEntryId);
                                    lSalesEntry.isSaleLocked = false;
                                    UpdateSaleEntry(lSalesEntry, Context);
                                }
                            }
                            if (SaleEntryInfo.PaymentId != null)
                            {
                                PaymentManager.Instance.DeletePayment((long)SaleEntryInfo.PaymentId, Context);
                            }
                        }

                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Deleted;
        }

        public List<SaleEntry> GetSaleEntryWithQuoteByCompanyId(long CompanyId)
        {
            List<SaleEntry> SaleEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntrys = (from SaleEntry in Context.SaleEntry where SaleEntry.CompanyId == CompanyId && SaleEntry.EntryType == Entrytype.QUOTE select SaleEntry).ToList();
                }
            }
            return SaleEntrys;
        }
        public List<SaleEntry> GetSaleEntryByPatientIdOpId(long? PatientId, long? RegistrationId)
        {
            List<SaleEntry> SaleEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntrys = (from SaleEntry in Context.SaleEntry.Include("SaleDetails").Include("SaleDetails.Product") where SaleEntry.PatientId == PatientId && SaleEntry.RegistrationId == RegistrationId select SaleEntry).ToList();
            }
            return SaleEntrys;
        }
        public List<string> GetCustomerFromSaleEntry(long CompanyId)
        {
            List<string> Customers = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Customers = (from SaleEntry in Context.SaleEntry where SaleEntry.CompanyId == CompanyId select SaleEntry).Select(x => x.CustomerName).Distinct().ToList();
            }
            return Customers;
        }
        public List<SaleEntry> GetSaleDeliveryPendingInvoiceByCompanyId(long CompanyId)
        {
            List<SaleEntry> SaleEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntrys = (from SaleEntry in Context.SaleEntry where SaleEntry.CompanyId == CompanyId && SaleEntry.EntryType == Entrytype.SALE && SaleEntry.hasDelivered == false && SaleEntry.isPaymentReceived == true select SaleEntry).ToList();
                }
            }
            return SaleEntrys;
        }

        public List<SaleEntry> GetSaleRecentPaymentByCompanyId(long CompanyId)
        {
            List<SaleEntry> SaleEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntrys = (from SaleEntry in Context.SaleEntry.Include("SalePayment") where SaleEntry.CompanyId == CompanyId && SaleEntry.EntryType == Entrytype.SALE && SaleEntry.SaleMethod == SaleMethod.Cash && SaleEntry.hasDelivered == false select SaleEntry).ToList();
                }
            }
            return SaleEntrys;
        }
        public List<SaleEntry> GetSaleInvoiceByCompanyId(long CompanyId)
        {
            List<SaleEntry> SaleEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    SaleEntrys = (from SaleEntry in Context.SaleEntry where SaleEntry.CompanyId == CompanyId select SaleEntry).ToList();
                }
            }
            return SaleEntrys;
        }
        public IList<SaleEntry> GetSaleRecentPaymentByDate(DateTime Date, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("SalePayment").Include("Account").Where(x => x.SaleDate.Day == Date.Day && x.SaleDate.Month == Date.Month && x.SaleDate.Year == Date.Year && x.EntryType == Entrytype.SALE && x.CompanyId == CompanyId).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }

        public IList<SaleEntry> GetSaleRecentPaymentByReferenceNo(string RefNo, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("SalePayment").Include("Account").Where(x => x.RefNumber == RefNo && x.EntryType == Entrytype.SALE && x.CompanyId == CompanyId).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public SaleEntry CheckDublicateReferenceNo(string RefNo, long Id, long CompanyId)
        {
            SaleEntry SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("SalePayment").Include("Account").FirstOrDefault(x => x.RefNumber == RefNo && x.EntryType == Entrytype.SALE && x.Id != Id && x.CompanyId == CompanyId);
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleRecentPaymentByAmount(double Amount, string search, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("SalePayment").Include("Account").Where(x => (x.NetAmount == Amount || x.RefNumber == search) && x.EntryType == Entrytype.SALE && x.CompanyId == CompanyId).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleRecentPaymentByCustomerName(string CustomerName, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("SalePayment").Include("Account").Where(x => (x.Account.Name.Contains(CustomerName) || x.CustomerName.Contains(CustomerName)) && x.EntryType == Entrytype.SALE && x.CompanyId == CompanyId).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public SaleEntry GetPaymentSaleEntry(long SaleEntryId)
        {
            SaleEntry SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntry = Context.SaleEntry.Include("SalePayment").Include("Account").FirstOrDefault(x => x.Id == SaleEntryId);
            }
            return SaleEntry;
        }
        public IList<SaleEntry> ListSaleReturnsBysaleId(long SaleEntryId)
        {
            IList<SaleEntry> SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntry = Context.SaleEntry.Include("SaleDetails").Include("SaleDetails.TaxDetails").Where(x => x.SaleEntryId == SaleEntryId).ToList<SaleEntry>();
            }
            return SaleEntry;
        }
        public SaleEntry GetSaleEntry(long SaleEntryId)
        {
            return GetSaleOrReturnOrQuote(SaleEntryId);
        }


        public SaleEntry GetSaleOrReturnOrQuote(long SaleEntryId)
        {
            SaleEntry SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Where(p => p.Id == SaleEntryId).First<SaleEntry>();
                if (SaleEntryInfo != null)
                {
                    if (SaleEntryInfo.SaleEntryId > 0)
                    {
                        SaleEntryInfo.SaleRefQuoteReturn = Context.SaleEntry.Where(x => x.Id == SaleEntryInfo.SaleEntryId).First();
                    }
                    if (SaleEntryInfo.SaleDetails != null && SaleEntryInfo.SaleDetails.Count == 0)
                    {
                        SaleEntryInfo.SaleDetails = Context.SaleDetail.Include("Discounts").Include("TaxDetails").Include("Product").Where(x => x.SaleId == SaleEntryInfo.Id).ToList();
                    }
                    if (SaleEntryInfo.AccountsId > 0)
                    {
                        Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntryInfo.AccountsId);
                        if (Customer != null)
                        {
                            SaleEntryInfo.Account = Context.Customers.Include("ContactInfo").Include("PaymentTerm").Include("BillingAddress").Where(x => x.Id == SaleEntryInfo.AccountsId).First();
                        }
                        else
                        {
                            Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)SaleEntryInfo.AccountsId);
                            if (Supplier != null)
                            {
                                SaleEntryInfo.Account = Context.Suppliers.Include("ContactInfo").Include("Address").Where(x => x.Id == SaleEntryInfo.AccountsId).First();
                            }
                        }
                    }
                    if (SaleEntryInfo.TaxDetails != null && SaleEntryInfo.TaxDetails.Count == 0)
                    {
                        SaleEntryInfo.TaxDetails = Context.OrderLevelSaleTaxDetails.Where(x => x.SaleId == SaleEntryInfo.Id).ToList();
                    }
                    if (SaleEntryInfo.Discounts != null && SaleEntryInfo.Discounts.Count == 0)
                    {
                        SaleEntryInfo.Discounts = Context.OrderLevelSaleDiscounts.Where(x => x.SaleId == SaleEntryInfo.Id).ToList();
                    }
                    if (SaleEntryInfo.SaleAdditionalTransactions != null && SaleEntryInfo.SaleAdditionalTransactions.Count == 0)
                    {
                        SaleEntryInfo.SaleAdditionalTransactions = Context.SaleAdditionalTransactions.Where(x => x.SaleEntryId == SaleEntryInfo.Id).ToList();
                    }
                    if (SaleEntryInfo.SoldById > 0)
                    {
                        SaleEntryInfo.SoldBy = Context.Refereds.Where(x => x.Id == SaleEntryInfo.SoldById).First();
                    }
                    if (SaleEntryInfo.ReferedById > 0)
                    {
                        SaleEntryInfo.ReferedBy = Context.Refereds.Where(x => x.Id == SaleEntryInfo.ReferedById).First();
                    }
                    if (SaleEntryInfo.SaleEntryId > 0)
                    {
                        SaleEntryInfo.SaleRefQuoteReturn = Context.SaleEntry.Where(x => x.Id == SaleEntryInfo.SaleEntryId).First();
                    }
                }
            }
            return SaleEntryInfo;
        }

        public SaleDetail GetSaleDetail(long SaleDetailsId)
        {
            SaleDetail SaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleDetails = Context.SaleDetail.Include("Sale").Include("Product").Include("Product.Parent").Include("Discounts").Include("TaxDetails.CatalogItemSalesTaxMap").Include("TaxDetails.ItemSalesTaxMap").FirstOrDefault(x => x.Id == SaleDetailsId);
            }
            return SaleDetails;
        }
        public SaleDetail GetReturnSaleDetail(long SaleDetailsId)
        {
            SaleDetail SaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleDetails = Context.SaleDetail.Include("Sale").Include("Product").Include("Discounts").Include("TaxDetails").FirstOrDefault(x => x.SaleDetailId == SaleDetailsId);
            }
            return SaleDetails;
        }
        public SaleDetail GetSaleDetailByProductId(long ProductId)
        {
            SaleDetail SaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleDetails = Context.SaleDetail.FirstOrDefault(x => x.ProductId == ProductId);
            }
            return SaleDetails;
        }
        public SaleDetail GetSaleDetailByProductIdBatch(long ProductId, string BatchNo)
        {
            SaleDetail SaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleDetails = Context.SaleDetail.FirstOrDefault(x => x.ProductId == ProductId && x.BatchNo == BatchNo);
            }
            return SaleDetails;
        }
        public IList<SaleEntry> GetRecentSaleEntrys(long CompanyId, Entrytype Type)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentSaleEntryInfo = Context.SaleEntry.Include("SaleDetails").Include("Account").Where(p => p.CompanyId == CompanyId && p.EntryType == Type).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>().Take(50);
                if (RecentSaleEntryInfo != null)
                {
                    SaleEntryInfo = RecentSaleEntryInfo.ToList();
                }
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleEntryByDate(DateTime Date, long CompanyId, Entrytype Type)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("Account").Where(x => x.SaleDate.Day == Date.Day && x.SaleDate.Month == Date.Month && x.SaleDate.Year == Date.Year && x.CompanyId == CompanyId && x.EntryType == Type).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }

        public IList<SaleEntry> GetSaleEntryByReferenceNo(string RefNo, long CompanyId, Entrytype Type)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("Account").Where(x => x.RefNumber.Contains(RefNo) && x.CompanyId == CompanyId && x.EntryType == Type).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleEntryByAmount(double Amount, string Refno, long CompanyId, Entrytype Type)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("Account").Where(x => (x.NetAmount == Amount || x.RefNumber.Contains(Refno)) && x.CompanyId == CompanyId && x.EntryType == Type).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleEntryByCustomerName(string CustomerName, long CompanyId, Entrytype Type)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("Account").Where(x => (x.Account.Name.Contains(CustomerName) || x.CustomerName.Contains(CustomerName) || x.RefNumber.Contains(CustomerName)) && x.CompanyId == CompanyId && x.EntryType == Type).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public IList<SaleEntry> GetSaleEntryByCustomerId(long CustomerId, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfo = Context.SaleEntry.Include("Account").Where(x => x.AccountsId == CustomerId && x.Balance > 0 && x.CompanyId == CompanyId && x.EntryType == Entrytype.SALE && x.SaleMethod == SaleMethod.Credit).OrderByDescending(x => x.SaleDate).ToList<SaleEntry>();
                return SaleEntryInfo;
            }
        }
        public IList<SaleDetail> ListReturnSaleDetail(long SaleDetailId)
        {
            IList<SaleDetail> SaleDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleDetails = Context.SaleDetail.Include("Product").Include("Discounts").Include("TaxDetails").Where(x => x.SaleDetailId == SaleDetailId).ToList();
            }
            return SaleDetails;
        }
        public IList<SaleEntry> ListSaleEntryIdByCreateDate(DateTime StartFromDate, DateTime EndToDate, long CompanyId)
        {
            IList<SaleEntry> SaleEntryInfoFrom = null;
            IList<SaleEntry> SaleEntryInfoTo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntryInfoFrom = Context.SaleEntry.Include("SaleDetails")
                    .Where(x => x.CreatedDate <= StartFromDate && x.CompanyId == CompanyId)
                    .ToList();

                SaleEntryInfoTo = Context.SaleEntry.Include("SaleDetails")
                    .Where(x => x.CreatedDate >= EndToDate && x.CompanyId == CompanyId)
                    .ToList();
            }

            IList<SaleEntry> combinedSaleEntryInfo = new List<SaleEntry>();
            combinedSaleEntryInfo = SaleEntryInfoFrom.Concat(SaleEntryInfoTo).ToList();

            return combinedSaleEntryInfo;

        }

        public IList<SaleEntry> GetSaleEntryBySaleEntryId(long SaleEntryId)
        {
            IList<SaleEntry> SaleEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SaleEntry = Context.SaleEntry.Include("SaleDetails").Include("SaleDetails.Product").Include("SaleDetails.TaxDetails").Include("Account").Include("Discounts").Include("TaxDetails").Include("SaleAdditionalTransactions").Where(x => x.SaleEntryId == SaleEntryId).ToList();
            }
            return SaleEntry;
        }

        public List<ItemLevelSaleTaxDetail> GetSaleTaxDetail(long saledetailId)
        {
            List<ItemLevelSaleTaxDetail> saleTaxDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                saleTaxDetails = Context.Set<ItemLevelSaleTaxDetail>().Include("CatalogItemSalesTaxMap").Include("ItemSalesTaxMap").Where(x => x.SaleDetailsId == saledetailId).ToList();
            }
            return saleTaxDetails;
        }
        public List<SaleEntry> GetLastPricesByProductAndCustomerxxx(long companyId, long productId, long customerId, int recordCount = 5)
        {
            using (var context = new AccountMasterContext())
            {
                return context.SaleEntry
                    .Include(se => se.SaleDetails) // Include SaleDetails for access
                    .ThenInclude(sd => sd.Price) // Include Product in SaleDetails
                    .Where(se => se.CompanyId == companyId
                              && se.AccountsId == customerId
                              && se.SaleDetails.Any(sd => sd.ProductId == productId))
                    .OrderByDescending(se => se.SaleDate)
                    .Take(recordCount)
                    .ToList();
            }
        }
        public List<SaleDetail> GetLastPricesByProductAndCustomer(
            long companyId,
            long productId,
            long customerId,
            int recordCount = 5)
        {
            using (var context = new AccountMasterContext())
            {
                return context.SaleDetail
                    .Include(sd => sd.Sale) // so you can still access SaleDate, etc.
                    .Where(sd =>
                        sd.ProductId == productId &&
                        sd.Sale.CompanyId == companyId &&
                        sd.Sale.AccountsId == customerId)
                    .OrderByDescending(sd => sd.Sale.SaleDate)
                    .Take(recordCount)
                    .ToList();
            }
        }
        public List<SaleDetail> DisplayLastPricesByProductAndCustomer(
            long companyId,
            long productId,
            long customerId,
            int recordCount = 5)
        {
            using (var context = new AccountMasterContext())
            {
                return context.SaleDetail
                    .Include(sd => sd.Sale) // so you can still access SaleDate, etc.
                    .Where(sd =>
                        sd.ProductId == productId &&
                        sd.Sale.CompanyId == companyId &&
                        sd.Sale.AccountsId == customerId)
                    .OrderByDescending(sd => sd.Sale.SaleDate)
                    .Take(recordCount)
                    .ToList();
            }
        }
    }
}
