using fa.api.Accounting;
using fa.api.System;
using fa.context;
using fa.Data;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using Fa.api.Accounting.DoubleEntry;
using Fa.api.OrderManagement;
using Microsoft.EntityFrameworkCore;
using fa;
using FADataAccessLibrary.Model.Accounting.Transactions;

namespace fa.api.OrderManagement
{
    public class PurchaseEntryManager
    {
        private static volatile PurchaseEntryManager instance;
        private static object syncRoot = new Object();
        PurchaseEntryManager()
        {

        }
        public static PurchaseEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PurchaseEntryManager();
                    }
                }
                return instance;
            }
        }

        public bool PurchaseEntryInvoiceNumberUniqueBySuppier(PurchaseEntry PurchaseEntry)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (PurchaseEntry.Id == 0)
                    {
                        Context.PurchaseEntry.Where(p => p.CompanyId == PurchaseEntry.CompanyId && p.AccountId == PurchaseEntry.AccountId && p.PurchaseInvNumber == PurchaseEntry.PurchaseInvNumber).First<PurchaseEntry>();
                        Status = false;
                    }
                    else
                    {
                        Context.PurchaseEntry.Where(p => p.CompanyId == PurchaseEntry.CompanyId && p.Id != PurchaseEntry.Id && p.AccountId == PurchaseEntry.AccountId && p.PurchaseInvNumber == PurchaseEntry.PurchaseInvNumber).First<PurchaseEntry>();
                        Status = false;
                    }
                }
                #pragma warning disable 0168 // variable declared but not used.
                catch (Exception ex)
                {
                }
                #pragma warning restore 0168
            }
            return Status;
        }
        public void ApplyPayment(PaymentDetail PaymentDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(PaymentDetail.ReferenceTrasnactionId))
            {
                PurchaseEntry lPurchaseEntry = GetPurchaseEntry(long.Parse(PaymentDetail.ReferenceTrasnactionId));
                if (lPurchaseEntry.Balance >= (double)PaymentDetail.Amount)
                {
                    lPurchaseEntry.Paid += (double)PaymentDetail.Amount;
                    lPurchaseEntry.Balance = Math.Round(((double)lPurchaseEntry.Balance - (double)PaymentDetail.Amount),2);
                    UpdatePurchaseEntryForApplayAndReversePayment(lPurchaseEntry, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + PaymentDetail.ReferenceTrasnactionId);
            }
        }
        public void ApplyNewPayment(PaymentDetailNew PaymentDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(PaymentDetail.ReferenceTransactionId))
            {
                PurchaseEntry lPurchaseEntry = GetPurchaseEntry(long.Parse(PaymentDetail.ReferenceTransactionId));
                if (lPurchaseEntry.Balance >= (double)PaymentDetail.Amount)
                {
                    lPurchaseEntry.Paid += (double)PaymentDetail.Amount;
                    lPurchaseEntry.Balance = Math.Round(((double)lPurchaseEntry.Balance - (double)PaymentDetail.Amount), 2);
                    UpdatePurchaseEntryForApplayAndReversePayment(lPurchaseEntry, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + PaymentDetail.ReferenceTransactionId);
            }
        }
        public void ReversePayment(PaymentDetail PaymentDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(PaymentDetail.ReferenceTrasnactionId))
            {
                PurchaseEntry lPurchaseEntry = GetPurchaseEntry(long.Parse(PaymentDetail.ReferenceTrasnactionId));
                if (lPurchaseEntry!=null && PaymentDetail.Amount > 0)
                {
                    lPurchaseEntry.Paid -= (double)PaymentDetail.Amount;
                    lPurchaseEntry.Balance += (double)PaymentDetail.Amount;
                    UpdatePurchaseEntryForApplayAndReversePayment(lPurchaseEntry, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + PaymentDetail.ReferenceTrasnactionId);
            }
        }
        private void UpdatePurchaseEntryForApplayAndReversePayment(PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            PurchaseEntry PurchaseEntryInfo = null;
            try
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Find(PurchaseEntry.Id);
                if (PurchaseEntryInfo != null)
                {
                    PurchaseEntry PurchaseEntryInfoFromDB = GetPurchaseEntry(PurchaseEntry.Id);
                    if (PurchaseEntryInfoFromDB.PurchaseDetails.Count > 0)
                    {
                        foreach (PurchaseDetails Detail in PurchaseEntryInfoFromDB.PurchaseDetails)
                        {
                            Context.LineLevelPurchaseTaxDetails.Where(p => p.PurchaseDetailsId == Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseTaxDetails.Remove(p));
                            Context.SaveChanges();

                            Context.LineLevelPurchaseDiscounts.Where(p => p.PurchaseDetailsId == Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseDiscounts.Remove(p));
                            Context.SaveChanges();
                        }

                    }
                    if (PurchaseEntryInfoFromDB.PurchaseAttachments.Count > 0)
                    {
                        Context.PurchaseAttachments.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAttachments.Remove(p));
                        Context.SaveChanges();
                    }

                    if (PurchaseEntryInfoFromDB.PurchaseAdditionalTransactions.Count > 0)
                    {
                        Context.PurchaseAdditionalTransactionses.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAdditionalTransactionses.Remove(p));
                        Context.SaveChanges();
                    }
                    if (PurchaseEntryInfoFromDB.Discounts.Count > 0)
                    {
                        Context.OrderLevelPurchaseDiscounts.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseDiscounts.Remove(p));
                        Context.SaveChanges();
                    }
                    if (PurchaseEntryInfoFromDB.TaxDetails.Count > 0)
                    {
                        Context.OrderLevelPurchaseTaxDetails.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseTaxDetails.Remove(p));
                        Context.SaveChanges();
                    }
                    PurchaseEntry.Account = null;
                    Context.Entry(PurchaseEntryInfo).CurrentValues.SetValues(PurchaseEntry);

                    foreach (PurchaseDetails OldDetail in PurchaseEntryInfoFromDB.PurchaseDetails)
                    {
                        PurchaseDetails NewDetail = PurchaseEntry.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                        if (NewDetail == null)
                        {
                            Context.PurchaseDetails.Remove(Context.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id));
                            OldDetail.FreeQuantity = 0;
                            OldDetail.Quantity = 0;                                  
                        }
                        else
                        {
                            PurchaseEntry.PurchaseDetails.Remove(NewDetail);
                            NewDetail.PurchaseEntryId = PurchaseEntry.Id;
                            PurchaseDetails PurchaseDetails = Context.PurchaseDetails.Find(NewDetail.Id);
                            PurchaseDetails.PurchaseEntry = null;
                            Context.Entry(PurchaseDetails).CurrentValues.SetValues(NewDetail);
                            Context.SaveChanges();

                            foreach (LineLevelPurchaseDiscount Detail in NewDetail.Discounts)
                            {
                                Detail.PurchaseDetails = null;
                                Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                Context.LineLevelPurchaseDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (LineLevelPurchaseTaxDetail Detail in NewDetail.TaxDetails)
                            {
                                Detail.PurchaseDetails = null;
                                Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                Context.LineLevelPurchaseTaxDetails.Add(Detail);
                                Context.SaveChanges();
                            }                                    
                        }
                    }                            
                    foreach (PurchaseDetails Detail in PurchaseEntry.PurchaseDetails)
                    {
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseDetails.Add(Detail);
                        Context.SaveChanges();
                    }

                    foreach (PurchaseAttachment Detail in PurchaseEntry.PurchaseAttachments)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseAttachments.Add(Detail);
                        Context.SaveChanges();
                    }

                    foreach (PurchaseAdditionalTransaction Detail in PurchaseEntry.PurchaseAdditionalTransactions)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseAdditionalTransactionses.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelPurchaseDiscount Detail in PurchaseEntry.Discounts)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.OrderLevelPurchaseDiscounts.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelPurchaseTaxDetail Detail in PurchaseEntry.TaxDetails)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.OrderLevelPurchaseTaxDetails.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }                       
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchaseEntryInfo = null;
                throw (e);
            }
        }

        public void RecordStockMovementPurchaseInPurchaseEntry(PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            Company Company = CompanyManager.Instance.GetCompany(PurchaseEntry.CompanyId);
            if (Company != null)
            {
                if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                {
                    StockMovementPurchase StockMovementPurchaseFDB = Context.StockMovementPurchase.FirstOrDefault(x => x.PurchaseEntryId == PurchaseEntry.Id);
                    StockMovementPurchase StockMovementPurchases = new StockMovementPurchase();
                    StockMovementPurchases.CompanyId = PurchaseEntry.CompanyId;
                    StockMovementPurchases.CostCenterId = PurchaseEntry.CostCenterId;
                    StockMovementPurchases.Id = StockMovementPurchaseFDB == null ? 0L : StockMovementPurchaseFDB.Id;
                    StockMovementPurchases.InventoryStockLocationId = (long)PurchaseEntry.InventoryLocationId;
                    StockMovementPurchases.MovementDate = PurchaseEntry.RefDate;
                    StockMovementPurchases.PurchaseEntryId = PurchaseEntry.Id;
                    if(StockMovementPurchaseFDB == null)
                    {
                        StockMovementPurchases.RefNumber = CompanyManager.Instance.GetIdSpace(Company,EntryType.STOCK_PURCHASE, PurchaseEntry.RefDate);
                    }
                    else
                    {
                        StockMovementPurchases.RefNumber = StockMovementPurchaseFDB.RefNumber;
                    }
                    foreach (PurchaseDetails details in PurchaseEntry.PurchaseDetails)
                    {
                        StockMovementDetail StockMovementDetail = new StockMovementDetail();
                        StockMovementDetail.BatchNo = details.BatchNo;
                        StockMovementDetail.CompanyId = details.CompanyId;
                        StockMovementDetail.CostCenterId = details.CostCenterId;
                        StockMovementDetail.ExpDate = details.ExpDate;
                        StockMovementDetail.Quantity = (details.Quantity + details.FreeQuantity);
                        StockMovementDetail.ProductId = details.ProductId;
                        StockMovementDetail.isBatch = details.isBatch;
                        StockMovementDetail.MaterialId = details.MaterialId;
                        StockMovementDetail.Uom = details.WholesaleUOM;

                        StockMovementPurchases.StockMovementDetails.Add(StockMovementDetail);
                    }
                    if (StockMovementPurchases.Id == 0L)
                    {
                        StockMovementManager.Instance.AddStockMovementPurchase(StockMovementPurchases, Context);
                    }
                    else
                    {
                        StockMovementManager.Instance.UpdateStockMovementPurchase(StockMovementPurchases, Context);
                    }
                }
                else
                {
                    StockMovementPurchaseReturn StockMovementPurchaseFDB = Context.StockMovementPurchaseReturn.FirstOrDefault(x => x.PurchaseReturnEntryId == PurchaseEntry.Id);
                    StockMovementPurchaseReturn StockMovementPurchaseReturn = new StockMovementPurchaseReturn();
                    StockMovementPurchaseReturn.CompanyId = PurchaseEntry.CompanyId;
                    StockMovementPurchaseReturn.CostCenterId = PurchaseEntry.CostCenterId;
                    StockMovementPurchaseReturn.Id = StockMovementPurchaseFDB == null ? 0L : StockMovementPurchaseFDB.Id;
                    StockMovementPurchaseReturn.InventoryStockLocationId = (long)PurchaseEntry.InventoryLocationId;
                    StockMovementPurchaseReturn.MovementDate = PurchaseEntry.RefDate;
                    StockMovementPurchaseReturn.PurchaseReturnEntryId = PurchaseEntry.Id;
                    StockMovementPurchaseReturn.RefNumber = StockMovementPurchaseFDB == null ? CompanyManager.Instance.GetIdSpace(Company, EntryType.STOCK_PURCHASE, PurchaseEntry.RefDate) : StockMovementPurchaseFDB.RefNumber;
                    foreach (PurchaseDetails details in PurchaseEntry.PurchaseDetails)
                    {
                        StockMovementDetail StockMovementDetail = new StockMovementDetail();
                        StockMovementDetail.BatchNo = details.BatchNo;
                        StockMovementDetail.CompanyId = details.CompanyId;
                        StockMovementDetail.CostCenterId = details.CostCenterId;
                        StockMovementDetail.ExpDate = details.ExpDate;
                        StockMovementDetail.Quantity = (details.Quantity + details.FreeQuantity);
                        StockMovementDetail.ProductId = details.ProductId;
                        StockMovementDetail.isBatch = details.isBatch;
                        StockMovementDetail.MaterialId = details.MaterialId;
                        StockMovementDetail.Uom = details.WholesaleUOM;
                        StockMovementPurchaseReturn.StockMovementDetails.Add(StockMovementDetail);
                    }
                    if (StockMovementPurchaseReturn.Id == 0L)
                    {
                        StockMovementManager.Instance.AddStockMovementPurchaseReturn(StockMovementPurchaseReturn, Context);
                    }
                    else
                    {
                        StockMovementManager.Instance.UpdateStockMovementPurchaseReturn(StockMovementPurchaseReturn, Context);
                    }
                }
            }

        }
        public PurchaseEntry AddPurchaseEntry(PurchaseEntry purchaseEntry)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //add to inventory to list
                        if (purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE || purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            if (purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                            {
                                //lock purchase entry generate return
                                PurchaseEntry lPurchaseEntry = GetPurchaseEntry((long)purchaseEntry.PurchaseEntryId);
                                lPurchaseEntry.isPurchaseEntryLocked = true;
                                UpdatePurchaseEntry(lPurchaseEntry, Context);
                            }
                            InventoryLocationManager.Instance.RecordPurchase(purchaseEntry, Context);
                        }
                        Context.PurchaseEntry.Add(purchaseEntry);
                        Context.SaveChanges();
                        if (purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE || purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            //record purchase movent
                            RecordStockMovementPurchaseInPurchaseEntry(purchaseEntry, Context);
                        }
                        Context.SaveChanges();
                        //daybook
                        if (purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                        {
                            PurchaseDoubleEntryManager.Instance.RecordPurchase(purchaseEntry, Context);
                        }
                        if (purchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            PurchaseReturnDoubleEntryManager.Instance.RecordPurchaseReturn(purchaseEntry, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        purchaseEntry = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return purchaseEntry;
        }
        public PurchaseEntry UpdatePurchaseEntry(PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            PurchaseEntry PurchaseEntryInfo = null;
                
            try
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Find(PurchaseEntry.Id);
                if (PurchaseEntryInfo != null)
                {
                    PurchaseEntry PurchaseEntryInfoFromDB = GetPurchaseEntry(PurchaseEntry.Id);
                    if (PurchaseEntryInfoFromDB.PurchaseDetails.Count > 0)
                    {
                        foreach (PurchaseDetails Detail in PurchaseEntryInfoFromDB.PurchaseDetails)
                        {
                            Context.LineLevelPurchaseTaxDetails.Where(p => p.PurchaseDetailsId == Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseTaxDetails.Remove(p));
                            Context.SaveChanges();

                            Context.LineLevelPurchaseDiscounts.Where(p => p.PurchaseDetailsId == Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseDiscounts.Remove(p));
                            Context.SaveChanges();
                        }

                    }
                    if (PurchaseEntryInfoFromDB.PurchaseAttachments.Count > 0)
                    {
                        Context.PurchaseAttachments.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAttachments.Remove(p));
                        Context.SaveChanges();
                    }

                    if (PurchaseEntryInfoFromDB.PurchaseAdditionalTransactions.Count > 0)
                    {
                        Context.PurchaseAdditionalTransactionses.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAdditionalTransactionses.Remove(p));
                        Context.SaveChanges();
                    }
                    if (PurchaseEntryInfoFromDB.Discounts.Count > 0)
                    {
                        Context.OrderLevelPurchaseDiscounts.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseDiscounts.Remove(p));
                        Context.SaveChanges();
                    }
                    if (PurchaseEntryInfoFromDB.TaxDetails.Count > 0)
                    {
                        Context.OrderLevelPurchaseTaxDetails.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseTaxDetails.Remove(p));
                        Context.SaveChanges();
                    }
                    PurchaseEntry.Account = null;
                    Context.Entry(PurchaseEntryInfo).CurrentValues.SetValues(PurchaseEntry);
                    //daybook
                    if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                    {
                        PurchaseDoubleEntryManager.Instance.RecordPurchase(PurchaseEntry, Context);
                    }
                    if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                    {
                        PurchaseReturnDoubleEntryManager.Instance.RecordPurchaseReturn(PurchaseEntry, Context);
                    }
                    foreach (PurchaseDetails OldDetail in PurchaseEntryInfoFromDB.PurchaseDetails)
                    {
                        PurchaseDetails NewDetail = PurchaseEntry.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                        if (NewDetail == null)
                        {
                            Context.PurchaseDetails.Remove(Context.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id));
                            OldDetail.FreeQuantity = 0;
                            OldDetail.Quantity = 0;
                            //add to inventory
                            InventoryLocationManager.Instance.RecordPurchaseDetails(OldDetail, PurchaseEntryInfoFromDB, Context);

                        }
                        else
                        {
                            PurchaseEntry.PurchaseDetails.Remove(NewDetail);
                            NewDetail.PurchaseEntryId = PurchaseEntry.Id;
                            PurchaseDetails PurchaseDetails = Context.PurchaseDetails.Find(NewDetail.Id);
                            PurchaseDetails.PurchaseEntry = null;
                            PurchaseDetails.Product = null;
                            Context.Entry(PurchaseDetails).CurrentValues.SetValues(NewDetail);
                            Context.SaveChanges();
                            foreach (LineLevelPurchaseDiscount Detail in NewDetail.Discounts)
                            {
                                Detail.PurchaseDetails = null;
                                Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                Context.LineLevelPurchaseDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (LineLevelPurchaseTaxDetail Detail in NewDetail.TaxDetails)
                            {
                                Detail.PurchaseDetails = null;
                                Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                Context.LineLevelPurchaseTaxDetails.Add(Detail);
                                Context.SaveChanges();
                            }

                            //add to inventory
                            InventoryLocationManager.Instance.RecordPurchaseDetails(NewDetail, PurchaseEntry, Context);
                        }
                    }
                    if (PurchaseEntry.PurchaseDetails.Count > 0)
                    {
                        //add to inventory to list
                        InventoryLocationManager.Instance.RecordPurchase(PurchaseEntry, Context);
                    }
                    foreach (PurchaseDetails Detail in PurchaseEntry.PurchaseDetails)
                    {
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseDetails.Add(Detail);
                        Context.SaveChanges();
                    }

                    foreach (PurchaseAttachment Detail in PurchaseEntry.PurchaseAttachments)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseAttachments.Add(Detail);
                        Context.SaveChanges();
                    }

                    foreach (PurchaseAdditionalTransaction Detail in PurchaseEntry.PurchaseAdditionalTransactions)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.PurchaseAdditionalTransactionses.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelPurchaseDiscount Detail in PurchaseEntry.Discounts)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.OrderLevelPurchaseDiscounts.Add(Detail);
                        Context.SaveChanges();
                    }
                    foreach (OrderLevelPurchaseTaxDetail Detail in PurchaseEntry.TaxDetails)
                    {
                        Detail.PurchaseEntry = null;
                        Detail.PurchaseEntryId = PurchaseEntry.Id;
                        Context.OrderLevelPurchaseTaxDetails.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PurchaseEntryInfo = null;
                throw (e);
            }               
            return PurchaseEntryInfo;
        }
        public PurchaseEntry UpdatePurchaseEntry(PurchaseEntry PurchaseEntry)
        {
            PurchaseEntry PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PurchaseEntryInfo = Context.PurchaseEntry.Find(PurchaseEntry.Id);
                        if (PurchaseEntryInfo != null)
                        {                           
                            PurchaseEntry PurchaseEntryInfoFromDB = GetPurchaseEntry(PurchaseEntry.Id);
                            if (PurchaseEntryInfoFromDB.PurchaseDetails.Count > 0)
                            {
                                foreach (PurchaseDetails Detail in PurchaseEntryInfoFromDB.PurchaseDetails)
                                {
                                    Context.LineLevelPurchaseTaxDetails.Where(p => p.PurchaseDetailsId== Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseTaxDetails.Remove(p));
                                    Context.SaveChanges();

                                    Context.LineLevelPurchaseDiscounts.Where(p => p.PurchaseDetailsId == Detail.Id).ToList().ForEach(p => Context.LineLevelPurchaseDiscounts.Remove(p));
                                    Context.SaveChanges();
                                }
                                
                            }
                            if (PurchaseEntryInfoFromDB.PurchaseAttachments.Count > 0)
                            {
                                Context.PurchaseAttachments.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAttachments.Remove(p));
                                Context.SaveChanges();
                            }
                            
                            if (PurchaseEntryInfoFromDB.PurchaseAdditionalTransactions.Count > 0)
                            {
                                Context.PurchaseAdditionalTransactionses.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.PurchaseAdditionalTransactionses.Remove(p));
                                Context.SaveChanges();
                            }
                            if (PurchaseEntryInfoFromDB.Discounts.Count > 0)
                            {
                                Context.OrderLevelPurchaseDiscounts.Where(p => p.PurchaseEntryId== PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseDiscounts.Remove(p));
                                Context.SaveChanges();
                            }
                            if (PurchaseEntryInfoFromDB.TaxDetails.Count > 0)
                            {
                                Context.OrderLevelPurchaseTaxDetails.Where(p => p.PurchaseEntryId == PurchaseEntryInfoFromDB.Id).ToList().ForEach(p => Context.OrderLevelPurchaseTaxDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            PurchaseEntry.Account = null;
                            Context.Entry(PurchaseEntryInfo).CurrentValues.SetValues(PurchaseEntry);
                            if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE || PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                            {
                                RecordStockMovementPurchaseInPurchaseEntry(PurchaseEntry, Context);
                            }
                            //daybook
                            if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                            {
                                PurchaseDoubleEntryManager.Instance.RecordPurchase(PurchaseEntry, Context);
                            }
                            if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                            {
                                PurchaseReturnDoubleEntryManager.Instance.RecordPurchaseReturn(PurchaseEntry, Context);
                            }
                            foreach (PurchaseDetails OldDetail in PurchaseEntryInfoFromDB.PurchaseDetails)
                            {
                                PurchaseDetails NewDetail = PurchaseEntry.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.PurchaseDetails.Remove(Context.PurchaseDetails.FirstOrDefault(x => x.Id == OldDetail.Id));
                                    OldDetail.FreeQuantity = 0;
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordPurchaseDetails(OldDetail, PurchaseEntryInfoFromDB, Context);

                                    //for wrongly enter batch number
                                    InventoryBatch InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.BatchNo == OldDetail.BatchNo && x.Inventory.ProductId == OldDetail.ProductId && x.Inventory.InventoryLocationId == PurchaseEntryInfoFromDB.InventoryLocationId);
                                    //InventoryLocationManager.Instance.GetInventoryBatchDetail((long)OldDetail.ProductId, OldDetail.BatchNo, (long)PurchaseEntryInfoFromDB.InventoryLocationId);
                                    if (InventoryBatch != null)
                                    {
                                        InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                                        if (lInventoryBatch.Sold == 0 && lInventoryBatch.OpeningStock == 0 && lInventoryBatch.Purchased == 0)
                                        {
                                            lInventoryBatch.Inventory = null;
                                            Context.InventoryBatches.Remove(lInventoryBatch);
                                            Context.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    PurchaseEntry.PurchaseDetails.Remove(NewDetail);
                                    NewDetail.PurchaseEntryId = PurchaseEntry.Id;
                                    PurchaseDetails PurchaseDetails = Context.PurchaseDetails.Find(NewDetail.Id);
                                    PurchaseDetails.PurchaseEntry = null;
                                    PurchaseDetails.Product = null;
                                    Context.Entry(PurchaseDetails).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();

                                    foreach (LineLevelPurchaseDiscount Detail in NewDetail.Discounts)
                                    {
                                        Detail.PurchaseDetails = null;
                                        Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                        Context.LineLevelPurchaseDiscounts.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                    foreach (LineLevelPurchaseTaxDetail Detail in NewDetail.TaxDetails)
                                    {
                                        Detail.PurchaseDetails = null;
                                        Detail.PurchaseDetailsId = PurchaseDetails.Id;
                                        Detail.CatalogItemSalesTaxMap = null;
                                        Context.LineLevelPurchaseTaxDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }                 
                                    
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordPurchaseDetails(NewDetail, PurchaseEntry, Context);
                                    
                                }
                            }
                            if(PurchaseEntry.PurchaseDetails.Count>0)
                            {
                                //add to inventory to list
                                InventoryLocationManager.Instance.RecordPurchase(PurchaseEntry, Context);
                            }
                            foreach (PurchaseDetails Detail in PurchaseEntry.PurchaseDetails)
                            {
                                Detail.PurchaseEntryId = PurchaseEntry.Id;
                                Context.PurchaseDetails.Add(Detail);
                                Context.SaveChanges();
                            }

                            foreach (PurchaseAttachment Detail in PurchaseEntry.PurchaseAttachments)
                            {
                                Detail.PurchaseEntry = null;
                                Detail.PurchaseEntryId = PurchaseEntry.Id;
                                Context.PurchaseAttachments.Add(Detail);
                                Context.SaveChanges();
                            }
                           
                            foreach (PurchaseAdditionalTransaction Detail in PurchaseEntry.PurchaseAdditionalTransactions)
                            {
                                Detail.PurchaseEntry = null;
                                Detail.PurchaseEntryId = PurchaseEntry.Id;
                                Context.PurchaseAdditionalTransactionses.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (OrderLevelPurchaseDiscount Detail in PurchaseEntry.Discounts)
                            {
                                Detail.PurchaseEntry = null;
                                Detail.PurchaseEntryId = PurchaseEntry.Id;
                                Context.OrderLevelPurchaseDiscounts.Add(Detail);
                                Context.SaveChanges();
                            }
                            if (PurchaseEntry.TaxDetails != null)
                            {
                                foreach (OrderLevelPurchaseTaxDetail Detail in PurchaseEntry.TaxDetails)
                                {
                                    Detail.PurchaseEntry = null;
                                    Detail.PurchaseEntryId = PurchaseEntry.Id;
                                    Context.OrderLevelPurchaseTaxDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                            }
                            Context.SaveChanges();
                        }
                        
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PurchaseEntryInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return PurchaseEntryInfo;
        }


        public Boolean DeletePurchaseEntry(long PurchaseEntryId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PurchaseEntry PurchaseEntryInfo = GetPurchaseEntry(PurchaseEntryId);
                        if (PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                        {
                           StockMovementManager.Instance.DeleteStockMovementPurchase(PurchaseEntryInfo.Id, Context);
                        }
                        if (PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            StockMovementManager.Instance.DeleteStockMovementPurchaseReturn(PurchaseEntryInfo.Id, Context);
                        }
                        if (PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.PURCHASE || PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            //add to inventory to list while remove
                            InventoryLocationManager.Instance.RecordPurchaseInDelete(PurchaseEntryInfo, Context);
                            //daybook delete
                            if (PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                            {
                                PurchaseDoubleEntryManager.Instance.DeletePurchase(PurchaseEntryInfo, Context);
                            }
                            else
                            {
                                PurchaseReturnDoubleEntryManager.Instance.DeletePurchaseReturn(PurchaseEntryInfo, Context);
                            }
                        }
                        if (PurchaseEntryInfo.PurchaseDetails.Count > 0)
                        {
                            Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").Where(p => p.PurchaseEntryId == PurchaseEntryInfo.Id).ToList().ForEach(p => Context.PurchaseDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        if (PurchaseEntryInfo.PurchaseAttachments.Count > 0)
                        {
                            Context.PurchaseAttachments.Where(p => p.PurchaseEntryId == PurchaseEntryInfo.Id).ToList().ForEach(p => Context.PurchaseAttachments.Remove(p));
                            Context.SaveChanges();
                        }
                       
                        if (PurchaseEntryInfo.PurchaseAdditionalTransactions.Count > 0)
                        {
                            Context.PurchaseAdditionalTransactionses.Where(p => p.PurchaseEntryId == PurchaseEntryInfo.Id).ToList().ForEach(p => Context.PurchaseAdditionalTransactionses.Remove(p));
                            Context.SaveChanges();
                        }
                        if (PurchaseEntryInfo.Discounts.Count > 0)
                        {
                            Context.OrderLevelPurchaseDiscounts.Where(p => p.PurchaseEntryId == PurchaseEntryInfo.Id).ToList().ForEach(p => Context.OrderLevelPurchaseDiscounts.Remove(p));
                            Context.SaveChanges();
                        }
                        if (PurchaseEntryInfo.TaxDetails.Count > 0)
                        {
                            Context.OrderLevelPurchaseTaxDetails.Where(p => p.PurchaseEntryId == PurchaseEntryInfo.Id).ToList().ForEach(p => Context.OrderLevelPurchaseTaxDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        PurchaseEntry lPurchaseEntryInfo = Context.PurchaseEntry.Find(PurchaseEntryId);
                        Context.PurchaseEntry.Remove(lPurchaseEntryInfo);
                        Context.SaveChanges();

                        if (PurchaseEntryInfo.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                        {
                            //Unlock sale entry delete return
                            if (PurchaseEntryInfo.PurchaseEntryId != null)
                            {
                                IList<PurchaseEntry> PurchaseEntrys = Context.PurchaseEntry.Include("PurchaseDetails").Where(x => x.PurchaseEntryId == (long)PurchaseEntryInfo.PurchaseEntryId).ToList<PurchaseEntry>();
                                if (PurchaseEntrys == null || PurchaseEntrys.Count == 0)
                                {
                                    PurchaseEntry lPurchaseEntry = GetPurchaseEntry((long)PurchaseEntryInfo.PurchaseEntryId);
                                    lPurchaseEntry.isPurchaseEntryLocked = false;
                                    UpdatePurchaseEntry(lPurchaseEntry, Context);
                                }
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

        public List<PurchaseEntry> GetPurchaseEntryByCompanyId(long CompanyId)
        {
            List<PurchaseEntry> PurchaseEntrys = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    PurchaseEntrys = (from PurchaseEntry in Context.PurchaseEntry where PurchaseEntry.CompanyId == CompanyId select PurchaseEntry).ToList();
                }
            }
            return PurchaseEntrys;
        }

        public PurchaseEntry GetPurchaseEntry(long PurchaseEntryId)
        {
            PurchaseEntry PurchaseEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntry = Context.PurchaseEntry.AsNoTracking().Include("PurchaseDetails").Include("PurchaseDetails.Product").Include("PurchaseDetails.TaxDetails").Include("PurchaseAttachments").Include("Account").Include("Discounts").Include("TaxDetails").Include("PurchaseAdditionalTransactions").FirstOrDefault(x => x.Id == PurchaseEntryId);
            }
            return PurchaseEntry;
        }
        public IList<PurchaseEntry> GetPurchaseEntryByPurchaseEntryId(long PurchaseEntryId)
        {
            IList<PurchaseEntry> PurchaseEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntry = Context.PurchaseEntry.Include("PurchaseDetails").Include("PurchaseDetails.Product").Include("PurchaseDetails.TaxDetails").Include("PurchaseAttachments").Include("Account").Include("Discounts").Include("TaxDetails").Include("PurchaseAdditionalTransactions").Where(x => x.PurchaseEntryId == PurchaseEntryId).ToList();
            }
            return PurchaseEntry;
        }
        public PurchaseDetails GetPurchaseDetail(long PurchaseDetailsId)
        {
            PurchaseDetails PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").Include("TaxDetails.CatalogItemSalesTaxMap").Include("TaxDetails.ItemSalesTaxMap").FirstOrDefault(x => x.Id == PurchaseDetailsId);
            }
            return PurchaseDetails;
        }
        public PurchaseDetails GetReturnPurchaseDetail(long PurchaseDetailsId)
        {
            PurchaseDetails PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").FirstOrDefault(x => x.PurchaseDetailsId == PurchaseDetailsId);
            }
            return PurchaseDetails;
        }
        public PurchaseDetails GetPurchaseOrderDetail(long PurchaseOrderId)
        {
            PurchaseDetails PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").FirstOrDefault(x => x.PurchaseDetailsId == PurchaseOrderId);
            }
            return PurchaseDetails;
        }
        public IList<PurchaseDetails> ListReturnPurchaseDetail(long PurchaseDetailsId)
        {
            IList<PurchaseDetails> PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").Where(x => x.PurchaseDetailsId == PurchaseDetailsId).ToList();
            }
            return PurchaseDetails;
        }

        public IList<PurchaseDetails> ListPurchaseDetailByPurchaseEntryId(long PurchaseId)
        {
            IList<PurchaseDetails> PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.Include("Product").Include("Discounts").Include("TaxDetails").Where(x => x.PurchaseEntryId == PurchaseId).ToList();
            }
            return PurchaseDetails;
        }
        public IList<PurchaseEntry> GetRecentPurchaseEntrys(long CompanyId,PurchaseEntrytype PurchaseEntrytype)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentPurchaseEntryInfo = Context.PurchaseEntry.Include("PurchaseDetails").Include("Account").Where(p=>p.CompanyId==CompanyId && p.PurchaseEntrytype== PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>().Take(50);
                if (RecentPurchaseEntryInfo != null)
                {
                    PurchaseEntryInfo = RecentPurchaseEntryInfo.ToList();
                }
                return PurchaseEntryInfo;
            }
        }
        public PurchaseEntry GetPurchasOrderByAmountRefNo(double Amount, string RefNo, long CompanyId, PurchaseEntrytype PurchaseEntrytype)
        {
            PurchaseEntry purchaseEntryInfo = null;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                purchaseEntryInfo = context.PurchaseEntry
                    .Include("Account")
                    .Where(x => x.CompanyId == CompanyId && (x.NetAmount == Amount || x.RefNumber.Contains(RefNo)) && x.PurchaseEntrytype == PurchaseEntrytype)
                    .OrderByDescending(x => x.RefDate)
                    .FirstOrDefault();

                return purchaseEntryInfo;
            }
        }

        public IList<PurchaseEntry> GetPurchaseEntryByDate(DateTime Date, long CompanyId, PurchaseEntrytype PurchaseEntrytype)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x => x.RefDate.Day == Date.Day && x.RefDate.Month == Date.Month && x.RefDate.Year == Date.Year && x.CompanyId == CompanyId && x.PurchaseEntrytype == PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                return PurchaseEntryInfo;
            }
        }
        public IList<PurchaseEntry> GetPurchaseOrderByAmountRefNo(double Amount, string RefNo, long CompanyId, PurchaseEntrytype PurchaseEntrytype)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x => x.CompanyId == CompanyId && x.NetAmount == Amount  &&  x.PurchaseEntrytype == PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                if( PurchaseEntryInfo != null && PurchaseEntryInfo.Count == 0)
                {
                    PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x => x.CompanyId == CompanyId && x.RefNumber == RefNo &&  x.PurchaseEntrytype == PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                }
                return PurchaseEntryInfo;
            }
        }
        public IList<PurchaseEntry> GetPurchaseEntryByAmount(double Amount, string RefNo, long CompanyId, PurchaseEntrytype PurchaseEntrytype)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x =>(x.NetAmount== Amount || x.RefNumber.Contains(RefNo)) && x.CompanyId == CompanyId && x.PurchaseEntrytype == PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                return PurchaseEntryInfo;
            }
        }
        public IList<PurchaseEntry> GetPurchaseEntryBySupplierName(string SupplierName, long CompanyId, PurchaseEntrytype PurchaseEntrytype)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x => (x.Account.Name.Contains(SupplierName) || x.SupplierName.Contains(SupplierName) || x.RefNumber.Contains(SupplierName)) && x.CompanyId == CompanyId && x.PurchaseEntrytype == PurchaseEntrytype).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                return PurchaseEntryInfo;
            }
        }
        public PurchaseDetails GetPurchaseDetailsByProductId(long ProductId)
        {
            PurchaseDetails PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.FirstOrDefault(x => x.ProductId == ProductId);
            }
            return PurchaseDetails;
        }
        public PurchaseDetails GetPurchaseDetailsByProductIdBatch(long ProductId,string BatchNo)
        {
            PurchaseDetails PurchaseDetails = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseDetails = Context.PurchaseDetails.FirstOrDefault(x => x.ProductId == ProductId && x.BatchNo== BatchNo);
            }
            return PurchaseDetails;
        }
        public IList<PurchaseEntry> GetPurchaseEntryBySupplierId(long SupplierId, long CompanyId)
        {
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntryInfo = Context.PurchaseEntry.Include("Account").Where(x => x.AccountId == SupplierId && x.Balance>0 && x.CompanyId == CompanyId && x.PurchaseMethod == PurchaseMethod.Credit).OrderByDescending(x => x.RefDate).ToList<PurchaseEntry>();
                return PurchaseEntryInfo;
            }
        }
        public IList<PurchaseEntry> ListPurchaseReturnsByPurchaseId(long PurchaseEntryId)
        {
            IList<PurchaseEntry> PurchaseEntry = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseEntry = Context.PurchaseEntry.Include("PurchaseDetails").Where(x => x.PurchaseEntryId == PurchaseEntryId).ToList<PurchaseEntry>();
            }
            return PurchaseEntry;
        }
        public IList<PurchaseEntry> ListPurchasePurchaseIdByCreateDate(DateTime startFromDate, DateTime endToDate, long companyIdSelected)
        {
            IList<PurchaseEntry> purchaseEntriesFrom = null;
            IList<PurchaseEntry> purchaseEntriesTo = null;

            using (AccountMasterContext context = new AccountMasterContext())
            {
                purchaseEntriesFrom = context.PurchaseEntry.Include("PurchaseDetails")
                                  .Where(x => x.CreatedDate <= startFromDate && x.CompanyId == companyIdSelected)
                                  .ToList();
                purchaseEntriesTo = context.PurchaseEntry.Include("PurchaseDetails")
                                  .Where(x => x.CreatedDate >= endToDate && x.CompanyId == companyIdSelected)
                                  .ToList();
            }

            // Combine both lists into one
            IList<PurchaseEntry> combinedPurchaseEntries = new List<PurchaseEntry>();
            combinedPurchaseEntries = purchaseEntriesFrom.Concat(purchaseEntriesTo).ToList();

            return combinedPurchaseEntries;
        }

        public IList<PurchaseEntry> ListPurchaseOrdersById(long companyId, DateTime fromDate, DateTime toDate)
        {
            IList<PurchaseEntry> purchaseEntries = null;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                purchaseEntries = context.PurchaseEntry.Include("PurchaseDetails").Where(x => x.CompanyId == companyId && x.PurchaseEntrytype == PurchaseEntrytype.ORDER && x.CreatedDate >= fromDate && x.CreatedDate <= toDate).ToList();
            }
            return purchaseEntries;
        }

        public IList<PurchaseDetails> ListPurchaseDetailsByCompanyId(long companyId, DateTime fromDate, DateTime toDate)
        {
            IList<PurchaseDetails> PurchaseDetails = null;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                PurchaseDetails = context.PurchaseDetails.Include("Product.Parent.Parent").Include("PurchaseEntry").Where(x => x.CompanyId == companyId && x.PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.ORDER && x.CreatedDate >= fromDate && x.CreatedDate <= toDate).ToList();
            }
            return PurchaseDetails;
        }
    }
}
