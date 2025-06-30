using fa.api.catalog;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Catalog;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fa.api.OrderManagement
{
    public class StockMovementManager
    {
        private static volatile StockMovementManager instance;
        private static object syncRoot = new Object();
        StockMovementManager()
        {

        }
        public static StockMovementManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StockMovementManager();
                    }
                }
                return instance;
            }
        }
        public bool IsStockOutReceived(long StockMovementOutId)
        {
            StockMovementIn StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementIn.Include("StockMovementDetails").FirstOrDefault(x => x.StockMovementOutId == StockMovementOutId);
                if(StockMovement!=null)
                {
                    return true;
                }
            }
            return false;
        }
        
        public StockMovementRequest GetStockMovementRequest(long StockMovementId)
        {
            StockMovementRequest StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementRequest.Include("StockMovementDetails.Product").Include("InventoryStockLocation").Include("RequestInventoryLocation").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovementOut GetStockMovementOut(long StockMovementId)
        {
            StockMovementOut StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementOut.Include("StockMovementDetails.Product").Include("InventoryStockLocation").Include("InventoryLocationTo").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovementDamaged GetStockDamaged(long StockMovementId)
        {
            StockMovementDamaged StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementDamaged.Include("StockMovementDetails.Product").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovementAdjustment GetStockAdjustment(long StockMovementId)
        {
            StockMovementAdjustment StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementAdjustment.Include("StockMovementDetails.Product").Include("InventoryStockLocation").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovement GetStockMovementById(long StockMovementId)
        {
            StockMovement StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovement.Include("StockMovementDetails.Product").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovementIn GetStockMovementIn(long StockMovementId)
        {
            StockMovementIn StockMovement = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovement = Context.StockMovementIn.Include("StockMovementDetails").FirstOrDefault(x => x.Id == StockMovementId);
            }
            return StockMovement;
        }
        public StockMovementDetail GetStockMovementDetail(long StockMovementDetailId)
        {
            StockMovementDetail StockMovementDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementDetail = Context.StockMovementDetail.Include("Product").FirstOrDefault(x => x.Id == StockMovementDetailId);
            }
            return StockMovementDetail;
        }
       
        public IList<StockMovementOut> GetRecentStockMovements(long CompanyId)
        {
            IList<StockMovementOut> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentStockMovementInfo = Context.StockMovementOut.Include("StockMovementDetails").Where(p => p.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementOut>().Take(50);
                if (RecentStockMovementInfo != null)
                {
                    StockMovementInfo = RecentStockMovementInfo.ToList();
                }
                return StockMovementInfo;
            }
        }
        public IList<StockMovementOut> GetStockMovementByDate(DateTime Date, long CompanyId)
        {
            IList<StockMovementOut> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementOut.Where(x => x.MovementDate.Day == Date.Day && x.MovementDate.Month == Date.Month && x.MovementDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementOut>();
                return StockMovementInfo;
            }
        }
        public IList<StockMovementOut> GetStockMovementByReferenceNo(string RefNo, long CompanyId)
        {
            IList<StockMovementOut> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementOut.Where(x => (x.RefNumber.Contains(RefNo) || x.InventoryLocationTo.Name.Contains(RefNo) || x.InventoryStockLocation.Name.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementOut>();
                return StockMovementInfo;
            }
        }
        public IList<StockMovementRequest> GetRecentStockRequests(long CompanyId,bool CheckReqStatus)
        {
            IList<StockMovementRequest> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementRequest.Include("StockMovementDetails").Where(p => p.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).Take(50).ToList<StockMovementRequest>();
                if(CheckReqStatus)
                {
                    StockMovementInfo = StockMovementInfo.Where(x => x.HasRequestCompleted == true).ToList();
                }
                return StockMovementInfo;
            }
        }
        public IList<StockMovementRequest> GetStockRequestByDate(DateTime Date, long CompanyId, bool CheckReqStatus)
        {
            IList<StockMovementRequest> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementRequest.Where(x => x.MovementDate.Day == Date.Day && x.MovementDate.Month == Date.Month && x.MovementDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementRequest>();
                if (CheckReqStatus)
                {
                    StockMovementInfo = StockMovementInfo.Where(x => x.HasRequestCompleted == true).ToList();
                }
                return StockMovementInfo;
            }
        }
        public IList<StockMovementRequest> GetStockRequestByReferenceNo(string RefNo, long CompanyId, bool CheckReqStatus)
        {
            IList<StockMovementRequest> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementRequest.Where(x => (x.RefNumber.Contains(RefNo) || x.RequestInventoryLocation.Name.Contains(RefNo)|| x.InventoryStockLocation.Name.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementRequest>();
                if (CheckReqStatus)
                {
                    StockMovementInfo = StockMovementInfo.Where(x => x.HasRequestCompleted == true).ToList();
                }
                return StockMovementInfo;
            }
        }
        public IList<StockMovementOut> GetStockMovementByLocation(long LoctnId, long CompanyId)
        {
            IList<StockMovementOut> StockMovementLoctnInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementLoctnInfo = (from item in Context.StockMovementOut
                                                 where item.InventoryLocationToId == LoctnId
                                                 where item.InventoryLocationToId != (from subitem in Context.StockMovementIn
                                                                   where subitem.StockMovementOutId == item.Id
                                                                   select subitem.InventoryStockLocationId).FirstOrDefault()
                                                 select item).ToList();

                return StockMovementLoctnInfo;
            }
        }
        

        //Adjustment entry
        public IList<StockMovementAdjustment> GetRecentStockAdjustments(long CompanyId)
        {
            IList<StockMovementAdjustment> StockMovementAdjustmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentStockMovementInfo = Context.StockMovementAdjustment.Include("StockMovementDetails").Where(p => p.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementAdjustment>().Take(50);
                if (RecentStockMovementInfo != null)
                {
                    StockMovementAdjustmentInfo = RecentStockMovementInfo.ToList();
                }
                return StockMovementAdjustmentInfo;
            }
        }
        public IList<StockMovementAdjustment> GetStockAdjustmentByDate(DateTime Date, long CompanyId)
        {
            IList<StockMovementAdjustment> StockMovementAdjustmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementAdjustmentInfo = Context.StockMovementAdjustment.Where(x => x.MovementDate.Day == Date.Day && x.MovementDate.Month == Date.Month && x.MovementDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementAdjustment>();
                return StockMovementAdjustmentInfo;
            }
        }
        public IList<StockMovementAdjustment> GetStockAdjustmentByReferenceNo(string RefNo, long CompanyId)
        {
            IList<StockMovementAdjustment> StockMovementAdjustmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementAdjustmentInfo = Context.StockMovementAdjustment.Where(x => x.RefNumber == RefNo && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementAdjustment>();
                return StockMovementAdjustmentInfo;
            }
        }
        //Damage entry
        public IList<StockMovementDamaged> GetRecentStockDamages(long CompanyId)
        {
            IList<StockMovementDamaged> StockMovementDamagedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentStockMovementInfo = Context.StockMovementDamaged.Include("StockMovementDetails").Where(p => p.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementDamaged>().Take(50);
                if (RecentStockMovementInfo != null)
                {
                    StockMovementDamagedInfo = RecentStockMovementInfo.ToList();
                }
                return StockMovementDamagedInfo;
            }
        }
        public IList<StockMovementDamaged> GetStockDamageByDate(DateTime Date, long CompanyId)
        {
            IList<StockMovementDamaged> StockMovementDamagedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementDamagedInfo = Context.StockMovementDamaged.Where(x => x.MovementDate.Day == Date.Day && x.MovementDate.Month == Date.Month && x.MovementDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementDamaged>();
                return StockMovementDamagedInfo;
            }
        }
        public IList<StockMovementDamaged> GetStockDamageByReferenceNo(string RefNo, long CompanyId)
        {
            IList<StockMovementDamaged> StockMovementDamagedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementDamagedInfo = Context.StockMovementDamaged.Where(x => x.RefNumber == RefNo && x.CompanyId == CompanyId).OrderByDescending(x => x.MovementDate).ToList<StockMovementDamaged>();
                return StockMovementDamagedInfo;
            }
        }
        public StockMovementIn AddStockMovementIn(StockMovementIn stockMovementIn)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var DbContextTransactiom = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InventoryLocationManager.Instance.RecorStockMovementIn(stockMovementIn, Context);
                        Context.StockMovementIn.Add(stockMovementIn);
                        Context.SaveChanges();
                        DbContextTransactiom.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        stockMovementIn = null;
                        DbContextTransactiom.Rollback();
                        throw (e);
                    }
                }
            }
            return stockMovementIn;
        }
        public StockMovementOut AddStockMovementOut(StockMovementOut stockMovementOut)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                       //add to inventory to list
                        InventoryLocationManager.Instance.RecordStockMovementOut(stockMovementOut, Context);
                        Context.StockMovementOut.Add(stockMovementOut);
                        Context.SaveChanges();
                        if (stockMovementOut.RequestId != 0L)
                        {
                            UpdateStockMovementRequest((long)stockMovementOut.RequestId, stockMovementOut.Id, Context,true);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        stockMovementOut = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return stockMovementOut;
        }
        public StockMovementRequest AddStockMovementRequest(StockMovementRequest stockMovementRequest)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.StockMovementRequest.Add(stockMovementRequest);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        stockMovementRequest = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return stockMovementRequest;
        }
        public StockMovementAdjustment AddStockMovementAdjustment(StockMovementAdjustment stockMovement)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var DbContextTransactiom = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InventoryLocationManager.Instance.RecordStockAdjustment(stockMovement, Context);
                        Context.StockMovementAdjustment.Add(stockMovement);
                        Context.SaveChanges();
                        DbContextTransactiom.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        stockMovement = null;
                        DbContextTransactiom.Rollback();
                        throw (e);
                    }
                }
            }
            return stockMovement;
        }
        public StockMovementDamaged AddStockMovementDamaged(StockMovementDamaged stockMovement)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var DbContextTransactiom = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InventoryLocationManager.Instance.RecordStockDamaged(stockMovement, Context);
                        Context.StockMovementDamaged.Add(stockMovement);
                        Context.SaveChanges();
                        DbContextTransactiom.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        stockMovement = null;
                        DbContextTransactiom.Rollback();
                        throw (e);
                    }
                }
            }
            return stockMovement;
        }
        public StockMovementOut UpdateStockMovementOut(StockMovementOut StockMovementOut)
        {
            StockMovementOut StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementOut.Find(StockMovementOut.Id);
                        if (StockMovementInfo != null)
                        {
                            StockMovementOut StockMovementInfoFromDB = GetStockMovementOut(StockMovementOut.Id);                                                     
                            Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementOut);
                            foreach (StockMovementDetail OldDetail in StockMovementInfoFromDB.StockMovementDetails.ToList())
                            {
                                StockMovementDetail NewDetail = StockMovementOut.StockMovementDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.StockMovementDetail.Remove(Context.StockMovementDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                                    OldDetail.FreeQuantity = 0;
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockMovementOutDetails(OldDetail, Context, StockMovementInfoFromDB);
                                }
                                else
                                {
                                    StockMovementOut.StockMovementDetails.Remove(NewDetail);
                                    NewDetail.StockMovementId = StockMovementOut.Id;
                                    StockMovementDetail StockMovementDetail = Context.StockMovementDetail.Find(NewDetail.Id);
                                    StockMovementDetail.StockMovement = null;
                                    Context.Entry(StockMovementDetail).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();

                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockMovementOutDetails(NewDetail, Context, StockMovementOut);
                                }
                            }
                            if (StockMovementOut.StockMovementDetails.Count > 0)
                            {
                                //add to inventory to list
                                InventoryLocationManager.Instance.RecordStockMovementOut(StockMovementOut, Context);
                            }
                            foreach (StockMovementDetail Detail in StockMovementOut.StockMovementDetails)
                            {
                                Detail.StockMovementId = StockMovementOut.Id;
                                Context.StockMovementDetail.Add(Detail);
                                Context.SaveChanges();
                            }                           
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public StockMovementRequest UpdateStockMovementRequest(long RequestId, long StockMovementId, AccountMasterContext Context,bool Response)
        {
            StockMovementRequest StockMovementInfo = null;
            try
            {
                StockMovementInfo = Context.StockMovementRequest.Find(RequestId);
                if (StockMovementInfo != null)
                {
                    StockMovementInfo.StockOutId = null;
                    StockMovementInfo.IsResponsed = Response;
                    if (Response)
                    {
                        StockMovementInfo.StockOutId = StockMovementId;
                    }
                    Context.Entry(Context.StockMovementRequest.Find(RequestId)).CurrentValues.SetValues(StockMovementInfo);                       
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                StockMovementInfo = null;
                throw (e);
            }         
            return StockMovementInfo;
        }
        public StockMovementRequest CompleteStockMovementRequest(long RequestId)
        {
            StockMovementRequest StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementRequest.Find(RequestId);
                        if (StockMovementInfo != null)
                        {
                            StockMovementInfo.HasRequestCompleted = true;
                            Context.Entry(Context.StockMovementRequest.Find(RequestId)).CurrentValues.SetValues(StockMovementInfo);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public StockMovementRequest UpdateStockMovementRequest(StockMovementRequest StockMovementRequest)
        {
            StockMovementRequest StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementRequest.Find(StockMovementRequest.Id);
                        if (StockMovementInfo != null)
                        {
                            StockMovementRequest StockMovementInfoFromDB = GetStockMovementRequest(StockMovementRequest.Id);
                            Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementRequest);
                            foreach (StockMovementDetail OldDetail in StockMovementInfoFromDB.StockMovementDetails.ToList())
                            {
                                StockMovementDetail NewDetail = StockMovementRequest.StockMovementDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.StockMovementDetail.Remove(Context.StockMovementDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                                }
                                else
                                {
                                    StockMovementRequest.StockMovementDetails.Remove(NewDetail);
                                    NewDetail.StockMovementId = StockMovementRequest.Id;
                                    StockMovementDetail StockMovementDetail = Context.StockMovementDetail.Find(NewDetail.Id);
                                    StockMovementDetail.StockMovement = null;
                                    Context.Entry(StockMovementDetail).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();
                                }
                            }
                            foreach (StockMovementDetail Detail in StockMovementRequest.StockMovementDetails)
                            {
                                Detail.StockMovementId = StockMovementRequest.Id;
                                Context.StockMovementDetail.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public StockMovementIn UpdateStockMovementIn(StockMovementIn StockMovementIn)
        {
            StockMovementIn StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementIn.Find(StockMovementIn.Id);
                        if (StockMovementInfo != null)
                        {
                            StockMovementIn StockMovementInfoFromDB = GetStockMovementIn(StockMovementIn.Id);                                                   
                            Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementIn);
                            foreach (StockMovementDetail OldDetail in StockMovementInfoFromDB.StockMovementDetails.ToList())
                            {
                                StockMovementDetail NewDetail = StockMovementIn.StockMovementDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.StockMovementDetail.Remove(Context.StockMovementDetail.FirstOrDefault(x => x.Id == OldDetail.Id));
                                    OldDetail.FreeQuantity = 0;
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockMovementInDetail(OldDetail, Context, StockMovementInfoFromDB);
                                    
                                }
                                else
                                {
                                    StockMovementIn.StockMovementDetails.Remove(NewDetail);
                                    NewDetail.StockMovementId = StockMovementIn.Id;
                                    StockMovementDetail StockMovementDetail = Context.StockMovementDetail.Find(NewDetail.Id);
                                    StockMovementDetail.StockMovement = null;
                                    Context.Entry(StockMovementDetail).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();

                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockMovementInDetail(NewDetail, Context, StockMovementIn);
                                }
                            }
                            if (StockMovementIn.StockMovementDetails.Count > 0)
                            {
                                //add to inventory to list
                                InventoryLocationManager.Instance.RecorStockMovementIn(StockMovementIn, Context);
                            }
                            foreach (StockMovementDetail Detail in StockMovementIn.StockMovementDetails)
                            {
                                Detail.StockMovementId = StockMovementIn.Id;
                                Context.StockMovementDetail.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public StockMovementAdjustment UpdateStockMovementAdjustment(StockMovementAdjustment StockMovement)
        {
            StockMovementAdjustment StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementAdjustment.Find(StockMovement.Id);
                        if (StockMovementInfo != null)
                        {
                            StockMovementAdjustment StockMovementInfoFromDB = GetStockAdjustment(StockMovement.Id);
                            Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovement);
                            foreach (StockMovementDetail OldDetail in StockMovementInfoFromDB.StockMovementDetails.ToList())
                            {
                                StockMovementDetail NewDetail = StockMovement.StockMovementDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.StockMovementDetail.Remove(Context.StockMovementDetail.Find(OldDetail.Id));
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockAdjustmentDetails(OldDetail, Context, StockMovementInfoFromDB);
                                }
                                else
                                {
                                    StockMovement.StockMovementDetails.Remove(NewDetail);
                                    NewDetail.StockMovementId = StockMovement.Id;
                                    StockMovementDetail StockMovementDetail = Context.StockMovementDetail.Find(NewDetail.Id);
                                    StockMovementDetail.StockMovement = null;
                                    Context.Entry(StockMovementDetail).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();

                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockAdjustmentDetails(NewDetail, Context, StockMovement);
                                }
                            }
                            if (StockMovement.StockMovementDetails.Count > 0)
                            {
                                //add to inventory to list
                                InventoryLocationManager.Instance.RecordStockAdjustment(StockMovement, Context);                            
                                foreach (StockMovementDetail Detail in StockMovement.StockMovementDetails)
                                {
                                    Detail.StockMovementId = StockMovement.Id;
                                    Context.StockMovementDetail.Add(Detail);
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
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public StockMovementDamaged UpdateStockMovementDamaged(StockMovementDamaged StockMovement)
        {
            StockMovementDamaged StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementInfo = Context.StockMovementDamaged.Find(StockMovement.Id);
                        if (StockMovementInfo != null)
                        {
                            StockMovementDamaged StockMovementInfoFromDB = GetStockDamaged(StockMovement.Id);
                            Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovement);
                            foreach (StockMovementDetail OldDetail in StockMovementInfoFromDB.StockMovementDetails.ToList())
                            {
                                StockMovementDetail NewDetail = StockMovement.StockMovementDetails.FirstOrDefault(x => x.Id == OldDetail.Id);
                                if (NewDetail == null)
                                {
                                    Context.StockMovementDetail.Remove(Context.StockMovementDetail.Find(OldDetail.Id));
                                    OldDetail.Quantity = 0;
                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockDamagedDetails(OldDetail, Context, StockMovementInfoFromDB);
                                }
                                else
                                {
                                    StockMovement.StockMovementDetails.Remove(NewDetail);
                                    NewDetail.StockMovementId = StockMovement.Id;
                                    StockMovementDetail StockMovementDetail = Context.StockMovementDetail.Find(NewDetail.Id);
                                    StockMovementDetail.StockMovement = null;
                                    Context.Entry(StockMovementDetail).CurrentValues.SetValues(NewDetail);
                                    Context.SaveChanges();

                                    //add to inventory
                                    InventoryLocationManager.Instance.RecordStockDamagedDetails(NewDetail, Context, StockMovement);
                                }
                            }
                            if (StockMovement.StockMovementDetails.Count > 0)
                            {
                                //add to inventory to list
                                InventoryLocationManager.Instance.RecordStockDamaged(StockMovement, Context);
                                foreach (StockMovementDetail Detail in StockMovement.StockMovementDetails)
                                {
                                    Detail.StockMovementId = StockMovement.Id;
                                    Context.StockMovementDetail.Add(Detail);
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
                        StockMovementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return StockMovementInfo;
        }
        public Boolean DeleteStockMovement(long StockMovementId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementOut StockMovementInfo = GetStockMovementOut(StockMovementId);
                        //add to inventory to list while remove
                        InventoryLocationManager.Instance.RecordStockMovementOutInDelete(StockMovementInfo, Context);
                        if (StockMovementInfo.StockMovementDetails.Count > 0)
                        {
                            Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                            Context.SaveChanges();
                        }
                        StockMovementInfo = Context.StockMovementOut.Find(StockMovementId);
                        StockMovementRequest Request = Context.StockMovementRequest.FirstOrDefault(x=>x.StockOutId== StockMovementId);
                        if (Request != null)
                        {
                            UpdateStockMovementRequest(Request.Id, StockMovementInfo.Id, Context, false);
                        }
                        Context.StockMovementOut.Remove(StockMovementInfo);
                        Context.SaveChanges();
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
        public Boolean DeleteStockMovementRequest(long StockMovementId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementRequest StockMovementInfo = GetStockMovementRequest(StockMovementId);
                        if (StockMovementInfo.StockMovementDetails.Count > 0)
                        {
                            Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                            Context.SaveChanges();
                        }
                        StockMovementInfo = Context.StockMovementRequest.Find(StockMovementId);
                        Context.StockMovementRequest.Remove(StockMovementInfo);
                        Context.SaveChanges();
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
        public Boolean DeleteStockAdjustment(long StockMovementId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementAdjustment StockMovementInfo = Context.StockMovementAdjustment.Include("StockMovementDetails").FirstOrDefault(x=>x.Id==StockMovementId);
                        //add to inventory to list while remove
                        InventoryLocationManager.Instance.RecordStockAdjustmentDelete(StockMovementInfo, Context);
                        if (StockMovementInfo.StockMovementDetails.Count > 0)
                        {
                            Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                            Context.SaveChanges();
                        }
                        StockMovementInfo = Context.StockMovementAdjustment.Find(StockMovementId);
                        Context.StockMovement.Remove(StockMovementInfo);
                        Context.SaveChanges();
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
        public Boolean DeleteStockDamaged(long StockMovementId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        StockMovementDamaged StockMovementInfo = Context.StockMovementDamaged.Include("StockMovementDetails").FirstOrDefault(x => x.Id == StockMovementId);
                        //add to inventory to list while remove
                        InventoryLocationManager.Instance.RecordStockDamagedDelete(StockMovementInfo, Context);
                        if (StockMovementInfo.StockMovementDetails.Count > 0)
                        {
                            Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                            Context.SaveChanges();
                        }
                        StockMovementInfo = Context.StockMovementDamaged.Find(StockMovementId);
                        Context.StockMovement.Remove(StockMovementInfo);
                        Context.SaveChanges();
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
        
        List<long> ListProductIds = new List<long>();
        public void AddStockMovementOpeningStock(StockMovementOpeningStock StockMovementOpeningStock, Product Item, long CompanyId, AccountMasterContext Context,bool IsBegin)
        {
            //reset stock reseted list
            if (IsBegin) { ListProductIds = new List<long>(); }
           //start load
            StockMovementOpeningStock lStockMovementOpeningStockDB = Context.StockMovementOpeningStock.Include("StockMovementDetails").FirstOrDefault(x => x.CompanyId == CompanyId  && x.InventoryStockLocationId == StockMovementOpeningStock.InventoryStockLocationId && (x.StockMovementDetails.Count == 0 || x.StockMovementDetails.First().ProductId == Item.Id));
            if (lStockMovementOpeningStockDB == null)
            {
                Context.StockMovementOpeningStock.Add(StockMovementOpeningStock);
                Context.SaveChanges();
                InventoryLocationManager.Instance.RecordOpeningStock(StockMovementOpeningStock, Context);
                ListProductIds.Add(Item.Id);
            }
            else
            {
                StockMovementOpeningStock.Id = lStockMovementOpeningStockDB.Id;
                if (!ListProductIds.Contains(Item.Id))
                {
                    //reset old entries
                    List<StockMovementDetail> lStockMovementDetail = Context.StockMovementDetail.Include("StockMovement").Where(p => p.StockMovement.CompanyId == CompanyId  && p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.ProductId == Item.Id).ToList();
                    if (lStockMovementDetail != null && lStockMovementDetail.Count > 0)
                    {
                        InventoryLocationManager.Instance.ResetOpeningStock(lStockMovementDetail, Context);
                        Context.StockMovementDetail.Where(p => p.StockMovement.CompanyId == CompanyId  && p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.ProductId == Item.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                        Context.SaveChanges();
                    }
                    //update reset details
                    ListProductIds.Add(Item.Id);
                }
                else
                {
                    foreach (StockMovementDetail OldDetail in lStockMovementOpeningStockDB.StockMovementDetails.ToList())
                    {
                        StockMovementDetail NewDetail = StockMovementOpeningStock.StockMovementDetails.FirstOrDefault(x => (x.ProductId == OldDetail.ProductId && !x.isBatch) || (x.ProductId == OldDetail.ProductId && x.BatchNo == OldDetail.BatchNo && x.isBatch));
                        if (NewDetail != null && OldDetail.Uom != NewDetail.Uom)
                        {
                            NewDetail.Id = 0;
                        }
                        else if (NewDetail != null)
                        {
                            StockMovementOpeningStock.StockMovementDetails.Remove(NewDetail);
                            NewDetail.StockMovementId = lStockMovementOpeningStockDB.Id;
                            NewDetail.Id = OldDetail.Id;
                            Context.Entry(Context.StockMovementDetail.Find(OldDetail.Id)).CurrentValues.SetValues(NewDetail);
                            Context.SaveChanges();

                            //add to inventory
                            InventoryLocationManager.Instance.ResetOpeningStock(StockMovementOpeningStock.InventoryStockLocationId, NewDetail, Context);
                        }
                    }
                    
                }
                Context.Entry(Context.StockMovementOpeningStock.Find(lStockMovementOpeningStockDB.Id)).CurrentValues.SetValues(StockMovementOpeningStock);
                if (StockMovementOpeningStock.StockMovementDetails.Count > 0)
                {
                    //add to inventory to list
                    InventoryLocationManager.Instance.RecordOpeningStock(StockMovementOpeningStock, Context);
                    foreach (StockMovementDetail Detail in StockMovementOpeningStock.StockMovementDetails)
                    {
                        Detail.StockMovementId = StockMovementOpeningStock.Id;
                        Context.StockMovementDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                }
            }            
        }
        //public void AddStockMovementOpeningStock(StockMovementOpeningStock StockMovementOpeningStock, Product Item, long CompanyId,bool IsBegin)
        //{
        //    using (AccountMasterContext Context = new AccountMasterContext())
        //    {
        //        using (var dbContextTransaction = Context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                AddStockMovementOpeningStock(StockMovementOpeningStock,Item, CompanyId, Context, IsBegin);
        //                dbContextTransaction.Commit();
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //                dbContextTransaction.Rollback();
        //                throw (e);
        //            }

        //        }
        //    }
        //}
        public void AddStockMovementOpeningStock(IList<StockMovementOpeningStock> StockMovementOpeningStock,Product Item, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        bool IsBegin = true;
                        foreach (StockMovementOpeningStock OpeningStock in StockMovementOpeningStock)
                        {
                            AddStockMovementOpeningStock(OpeningStock, Item, CompanyId, Context, IsBegin);
                            IsBegin = false;
                        }
                        if (StockMovementOpeningStock.Count == 0)
                        {
                            Product product = CatalogProductManager.Instance.GetProductInfoById(Item.Id);
                            List<InventoryBatch> Batches = Context.InventoryBatches.Where(x => x.ProductId == Item.Id).ToList<InventoryBatch>();
                            foreach (InventoryBatch batch in Batches)
                            {
                                List<StockMovementDetail> lStockMovementDetail = Context.StockMovementDetail.Include("StockMovement").Where(p => p.StockMovement.CompanyId == CompanyId && p.StockMovement.Type != InventoryJournalType.OPEN_STOCK && p.ProductId == Item.Id && p.BatchNo == batch.BatchNo).ToList();

                                if (lStockMovementDetail.Count == 0)
                                {
                                    List<StockMovementDetail> StockMovementDetail = Context.StockMovementDetail.Include("StockMovement").Where(p => p.StockMovement.CompanyId == CompanyId && p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.ProductId == Item.Id && p.BatchNo == batch.BatchNo).ToList();
                                    if (StockMovementDetail != null && StockMovementDetail.Count > 0)
                                    {
                                        InventoryLocationManager.Instance.ResetOpeningStock(StockMovementDetail, Context);
                                        Context.StockMovementDetail.Where(p => p.StockMovement.CompanyId == CompanyId && p.StockMovement.Type == InventoryJournalType.OPEN_STOCK && p.ProductId == Item.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                                        Context.SaveChanges();
                                    }
                                }
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
        }
        public IList<StockMovementDetail> GetOpeningStockDetails(long CompanyId,long ProductId)
        {
            IList<StockMovementDetail> StockMovementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementInfo = Context.StockMovementDetail.Where(p => p.StockMovement.Type==InventoryJournalType.OPEN_STOCK && p.StockMovement.CompanyId == CompanyId && p.ProductId==ProductId).ToList<StockMovementDetail>();              
                return StockMovementInfo;
            }
        }

        public StockMovementSales AddStockMovementSale(StockMovementSales stockMovementSales, AccountMasterContext Context)
        {        
            try
            {
                Context.StockMovementSales.Add(stockMovementSales);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                stockMovementSales = null;
                throw (e);
            }          
            return stockMovementSales;
        }
        public StockMovementSales UpdateStockMovementSale(StockMovementSales StockMovementSales, AccountMasterContext Context)
        {
            StockMovementSales StockMovementInfo = null;            
            try
            {
                StockMovementInfo = Context.StockMovementSales.Find(StockMovementSales.Id);
                if (StockMovementInfo != null)
                {
                    StockMovementSales StockMovementInfoFromDB = Context.StockMovementSales.Include("StockMovementDetails.Product").FirstOrDefault(x => x.Id == StockMovementSales.Id);
                    Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementSales);
                    Context.SaveChanges();
                   
                    Context.StockMovementDetail.Where(p => p.StockMovementId == StockMovementSales.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                    Context.SaveChanges();

                    foreach (StockMovementDetail Detail in StockMovementSales.StockMovementDetails)
                    {
                        Detail.StockMovementId = StockMovementSales.Id;
                        Context.StockMovementDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                StockMovementInfo = null;
                throw (e);
            }               
            return StockMovementInfo;
        }

        public Boolean DeleteStockMovementSale(long StockMovementSaleId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            try
            {
                StockMovementSales StockMovementInfo = Context.StockMovementSales.Include("StockMovementDetails.Product").FirstOrDefault(x => x.SaleId == StockMovementSaleId);
                if (StockMovementInfo != null)
                {
                    if (StockMovementInfo.StockMovementDetails.Count > 0)
                    {
                        Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                        Context.SaveChanges();
                    }

                    StockMovementInfo = Context.StockMovementSales.Find(StockMovementInfo.Id);
                    Context.StockMovementSales.Remove(StockMovementInfo);
                    Context.SaveChanges();
                Deleted = true;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }               
            return Deleted;
        }
        public StockMovementSalesReturn AddStockMovementSaleReturn(StockMovementSalesReturn stockMovementSales, AccountMasterContext Context)
        {
            try
            {
                Context.StockMovementSalesReturn.Add(stockMovementSales);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                stockMovementSales = null;
                throw (e);
            }
            return stockMovementSales;
        }
        public StockMovementSalesReturn UpdateStockMovementSaleReturn(StockMovementSalesReturn StockMovementSales, AccountMasterContext Context)
        {
            StockMovementSalesReturn StockMovementInfo = null;
            try
            {
                StockMovementInfo = Context.StockMovementSalesReturn.Find(StockMovementSales.Id);
                if (StockMovementInfo != null)
                {
                    StockMovementSalesReturn StockMovementInfoFromDB = Context.StockMovementSalesReturn.Include("StockMovementDetails.Product").FirstOrDefault(x => x.Id == StockMovementSales.Id);
                    Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementSales);
                    Context.SaveChanges();

                    Context.StockMovementDetail.Where(p => p.StockMovementId == StockMovementSales.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                    Context.SaveChanges();

                    foreach (StockMovementDetail Detail in StockMovementSales.StockMovementDetails)
                    {
                        Detail.StockMovementId = StockMovementSales.Id;
                        Context.StockMovementDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                StockMovementInfo = null;
                throw (e);
            }
            return StockMovementInfo;
        }

        public Boolean DeleteStockMovementSaleReturn(long StockMovementSaleId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            try
            {
                StockMovementSalesReturn StockMovementInfo = Context.StockMovementSalesReturn.Include("StockMovementDetails.Product").FirstOrDefault(x => x.SaleReturnId == StockMovementSaleId);
                if (StockMovementInfo != null)
                {
                    if (StockMovementInfo.StockMovementDetails.Count > 0)
                    {
                        Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                        Context.SaveChanges();
                    }

                    StockMovementInfo = Context.StockMovementSalesReturn.Find(StockMovementInfo.Id);
                    Context.StockMovementSalesReturn.Remove(StockMovementInfo);
                    Context.SaveChanges();
                    Deleted = true;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return Deleted;
        }
        public StockMovementPurchase AddStockMovementPurchase(StockMovementPurchase stockMovementPurchases, AccountMasterContext Context)
        {
            try
            {
                Context.StockMovementPurchase.Add(stockMovementPurchases);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                stockMovementPurchases = null;
                throw (e);
            }
            return stockMovementPurchases;
        }
        public StockMovementPurchase UpdateStockMovementPurchase(StockMovementPurchase StockMovementPurchases, AccountMasterContext Context)
        {
            StockMovementPurchase StockMovementInfo = null;
            try
            {
                StockMovementInfo = Context.StockMovementPurchase.Find(StockMovementPurchases.Id);
                if (StockMovementInfo != null)
                {
                    StockMovementPurchase StockMovementInfoFromDB = Context.StockMovementPurchase.Include("StockMovementDetails.Product").FirstOrDefault(x => x.Id == StockMovementPurchases.Id);
                    Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementPurchases);
                    Context.SaveChanges();

                    Context.StockMovementDetail.Where(p => p.StockMovementId == StockMovementPurchases.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                    Context.SaveChanges();

                    foreach (StockMovementDetail Detail in StockMovementPurchases.StockMovementDetails)
                    {
                        Detail.StockMovementId = StockMovementPurchases.Id;
                        Context.StockMovementDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                StockMovementInfo = null;
                throw (e);
            }
            return StockMovementInfo;
        }

        public Boolean DeleteStockMovementPurchase(long StockMovementPurchaseId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            try
            {
                StockMovementPurchase StockMovementInfo = Context.StockMovementPurchase.Include("StockMovementDetails.Product").FirstOrDefault(x => x.PurchaseEntryId == StockMovementPurchaseId);
                if (StockMovementInfo != null)
                {
                    if (StockMovementInfo.StockMovementDetails.Count > 0)
                    {
                        Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                        Context.SaveChanges();
                    }

                    StockMovementInfo = Context.StockMovementPurchase.Find(StockMovementInfo.Id);
                    Context.StockMovementPurchase.Remove(StockMovementInfo);
                    Context.SaveChanges();
                    Deleted = true;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return Deleted;
        }

        public StockMovementPurchaseReturn AddStockMovementPurchaseReturn(StockMovementPurchaseReturn stockMovementPurchaseReturns, AccountMasterContext Context)
        {
            try
            {
                Context.StockMovementPurchaseReturn.Add(stockMovementPurchaseReturns);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                stockMovementPurchaseReturns = null;
                throw (e);
            }
            return stockMovementPurchaseReturns;
        }
        public StockMovementPurchaseReturn UpdateStockMovementPurchaseReturn(StockMovementPurchaseReturn StockMovementPurchaseReturns, AccountMasterContext Context)
        {
            StockMovementPurchaseReturn StockMovementInfo = null;
            try
            {
                StockMovementInfo = Context.StockMovementPurchaseReturn.Find(StockMovementPurchaseReturns.Id);
                if (StockMovementInfo != null)
                {
                    StockMovementPurchase StockMovementInfoFromDB = Context.StockMovementPurchase.Include("StockMovementDetails.Product").FirstOrDefault(x => x.Id == StockMovementPurchaseReturns.Id);
                    Context.Entry(StockMovementInfo).CurrentValues.SetValues(StockMovementPurchaseReturns);
                    Context.SaveChanges();

                    Context.StockMovementDetail.Where(p => p.StockMovementId == StockMovementPurchaseReturns.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                    Context.SaveChanges();

                    foreach (StockMovementDetail Detail in StockMovementPurchaseReturns.StockMovementDetails)
                    {
                        Detail.StockMovementId = StockMovementPurchaseReturns.Id;
                        Context.StockMovementDetail.Add(Detail);
                        Context.SaveChanges();
                    }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                StockMovementInfo = null;
                throw (e);
            }
            return StockMovementInfo;
        }

        public Boolean DeleteStockMovementPurchaseReturn(long StockMovementPurchaseReturnId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            try
            {
                StockMovementPurchaseReturn StockMovementInfo = Context.StockMovementPurchaseReturn.Include("StockMovementDetails.Product").FirstOrDefault(x => x.PurchaseReturnEntryId == StockMovementPurchaseReturnId);
                if (StockMovementInfo != null)
                {
                    if (StockMovementInfo.StockMovementDetails.Count > 0)
                    {
                        Context.StockMovementDetail.Include("Product").Where(p => p.StockMovementId == StockMovementInfo.Id).ToList().ForEach(p => Context.StockMovementDetail.Remove(p));
                        Context.SaveChanges();
                    }

                    StockMovementInfo = Context.StockMovementPurchaseReturn.Find(StockMovementInfo.Id);
                    Context.StockMovementPurchaseReturn.Remove(StockMovementInfo);
                    Context.SaveChanges();
                    Deleted = true;
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return Deleted;
        }
        public StockMovementDetail GetStockMovementDetailByProductId(long ProductId)
        {
            StockMovementDetail StockMovementDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                StockMovementDetail = Context.StockMovementDetail.FirstOrDefault(x => x.ProductId == ProductId);
            }
            return StockMovementDetail;
        }
        public bool IsCompanyHaveMovementEntries(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(Context.StockMovementDetail.Where(x => x.CompanyId == CompanyId).ToList().Count>0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
