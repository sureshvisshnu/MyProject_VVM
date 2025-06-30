using fa.api.catalog;
using fa.context;
using fa.model.Catalog;
using fa.model.OrderManagement;
using Fa.api.OrderManagement;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;

namespace fa.api.OrderManagement
{
    public class InventoryLocationManager
    {
        private static volatile InventoryLocationManager instance;
        private static object syncRoot = new Object();
        InventoryLocationManager()
        {

        }
        public static InventoryLocationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InventoryLocationManager();
                    }
                }
                return instance;
            }
        }
        public bool DeleteInventory(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Inventory Inventory = Context.Inventories.Find(Id);
                    Context.Inventories.Remove(Inventory);
                    Context.SaveChanges();
                    return true;
                }
                #pragma warning disable 0168
                catch (Exception e)
                {
                    return false;
                }
                #pragma warning restore 0168
            }
        }
        public bool DeleteInventoryBatch(long BatchId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    InventoryBatch InventoryBatch = Context.InventoryBatches.Find(BatchId);
                    Context.InventoryBatches.Remove(InventoryBatch);
                    Context.SaveChanges();
                    return true;
                }
                #pragma warning disable 0168
                catch (Exception e)
                {
                    return false;
                }
                #pragma warning restore 0168
            }
        }
        public void AddInventoryBatch(IList<InventoryBatch> lInventoryBatch,long ProductId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        IList<Inventory> OldInventory = Context.Inventories.Where(x => x.ProductId == ProductId && x.OpeningStock > 0).ToList();
                        foreach (Inventory Inventory in OldInventory)
                        {                           
                            if(Inventory.Purchased == 0 && Inventory.In == 0 && Inventory.Sold == 0 && Inventory.Out == 0 && Inventory.ToPatient == 0 && Inventory.Damage == 0)
                            {
                                Context.InventoryBatches.Where(p => p.ProductId == ProductId && p.Inventory.InventoryLocationId== Inventory.InventoryLocationId).ToList().ForEach(p => Context.InventoryBatches.Remove(p));
                                Context.SaveChanges();
                                Context.Inventories.Remove(Context.Inventories.Find(Inventory.Id));
                                Context.SaveChanges();
                            }
                            else
                            {
                                Inventory.OpeningStock = 0;
                                Context.Entry(Context.Inventories.Find(Inventory.Id)).CurrentValues.SetValues(Inventory);
                                Context.SaveChanges();
                            }
                        }
                        
                        IList<InventoryBatch> OldInventoryBatch = Context.InventoryBatches.Include("Inventory").Where(x => x.ProductId == ProductId && x.OpeningStock > 0).ToList();
                        foreach (InventoryBatch OldDetail in OldInventoryBatch)
                        {
                            InventoryBatch NewDetail = lInventoryBatch.FirstOrDefault(x => x.Id == OldDetail.Id);
                            if (NewDetail == null)
                            {                               
                                if (OldDetail.Purchased == 0 && OldDetail.In == 0 && OldDetail.Sold == 0 && OldDetail.Out == 0 && OldDetail.ToPatient == 0 && OldDetail.Damage == 0)
                                {
                                    Context.InventoryBatches.Remove(Context.InventoryBatches.Find(OldDetail.Id));
                                }
                                else
                                {
                                    InventoryBatch InventoryBatchDetails = Context.InventoryBatches.Find(OldDetail.Id);
                                    InventoryBatchDetails.OpeningStock = 0;
                                    Context.Entry(Context.InventoryBatches.Find(InventoryBatchDetails.Id)).CurrentValues.SetValues(InventoryBatchDetails);
                                    Context.SaveChanges();
                                }
                            }
                            else
                            {
                                lInventoryBatch.Remove(NewDetail);
                                InventoryBatch InventoryBatchDetails = Context.InventoryBatches.Find(NewDetail.Id);

                                //for inventory
                                Inventory InventoryDetails = Context.Inventories.Find(InventoryBatchDetails.InventoryId);
                                InventoryDetails.OpeningStock += NewDetail.OpeningStock;
                                Context.Entry(Context.Inventories.Find(InventoryDetails.Id)).CurrentValues.SetValues(InventoryDetails);
                                Context.SaveChanges();
                                
                                //for inventory batch
                                NewDetail.Purchased = InventoryBatchDetails.Purchased;
                                NewDetail.Sold = InventoryBatchDetails.Sold;
                                NewDetail.In = InventoryBatchDetails.In;
                                NewDetail.Damage = InventoryBatchDetails.Damage;
                                NewDetail.ToPatient = InventoryBatchDetails.ToPatient;
                                NewDetail.Out = InventoryBatchDetails.Out;
                                NewDetail.Adjust = InventoryBatchDetails.Adjust;
                                NewDetail.OpeningStock += InventoryBatchDetails.OpeningStock;
                                Context.Entry(Context.InventoryBatches.Find(InventoryBatchDetails.Id)).CurrentValues.SetValues(NewDetail);
                                Context.SaveChanges();
                            }
                        }
                        foreach (InventoryBatch InventoryBatch in lInventoryBatch)
                        {
                            InventoryBatch InventoryBatchFromDB = Context.InventoryBatches.FirstOrDefault(x => x.ProductId == InventoryBatch.ProductId && x.Inventory.InventoryLocationId == InventoryBatch.LocationId && x.BatchNo== InventoryBatch.BatchNo);
                            if (InventoryBatchFromDB == null)
                            {
                                Inventory inventoryFromDB = Context.Inventories.FirstOrDefault(x => x.ProductId == InventoryBatch.ProductId && x.InventoryLocationId == InventoryBatch.LocationId);
                                if(inventoryFromDB == null)
                                {
                                    inventoryFromDB = new Inventory();
                                    inventoryFromDB.OpeningStock = InventoryBatch.OpeningStock;
                                    inventoryFromDB.StockUOM = InventoryBatch.StockUOM;
                                    inventoryFromDB.ProductId = InventoryBatch.ProductId;
                                    inventoryFromDB.CompanyId = InventoryBatch.CompanyId;
                                    inventoryFromDB.InventoryLocationId = InventoryBatch.LocationId;
                                    Context.Inventories.Add(inventoryFromDB);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    inventoryFromDB.OpeningStock += InventoryBatch.OpeningStock;
                                    Context.Entry(Context.Inventories.Find(inventoryFromDB.Id)).CurrentValues.SetValues(inventoryFromDB);
                                    Context.SaveChanges();
                                }

                                InventoryBatch.InventoryId = inventoryFromDB.Id;
                                Context.InventoryBatches.Add(InventoryBatch);
                                Context.SaveChanges();
                            }
                            else
                            {
                                //for inventory
                                Inventory InventoryDetails = Context.Inventories.Find(InventoryBatchFromDB.InventoryId);
                                InventoryDetails.OpeningStock += InventoryBatch.OpeningStock;
                                Context.Entry(Context.Inventories.Find(InventoryDetails.Id)).CurrentValues.SetValues(InventoryDetails);
                                Context.SaveChanges();
                                //for inventory batch
                                InventoryBatch.Id = InventoryBatchFromDB.Id;
                                InventoryBatch.Purchased = InventoryBatchFromDB.Purchased;
                                InventoryBatch.Sold = InventoryBatchFromDB.Sold;
                                InventoryBatch.In = InventoryBatchFromDB.In;
                                InventoryBatch.Damage = InventoryBatchFromDB.Damage;
                                InventoryBatch.ToPatient = InventoryBatchFromDB.ToPatient;
                                InventoryBatch.Out = InventoryBatchFromDB.Out;
                                InventoryBatch.Adjust = InventoryBatchFromDB.Adjust;
                                InventoryBatch.InventoryId = InventoryBatchFromDB.InventoryId;
                                Context.Entry(Context.InventoryBatches.Find(InventoryBatchFromDB.Id)).CurrentValues.SetValues(InventoryBatch);
                                Context.SaveChanges();
                            }
                        }

                        Product ProductFDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                        if(ProductFDB!=null)
                        {
                            ProductFDB._isInventoryAtBatch = true;
                            CatalogProductManager.Instance.UpdateProductWithContext(ProductFDB,Context);
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
        public InventoryBatch AddInventoryBatch(InventoryBatch inventoryBatch)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.InventoryBatches.Add(inventoryBatch);
                Context.SaveChanges();
            }
            return inventoryBatch;
        }
        public void AddInventory(IList<Inventory> lInventory,long ProductId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.InventoryBatches.Where(p => p.ProductId == ProductId).ToList().ForEach(p => Context.InventoryBatches.Remove(p));
                        Context.SaveChanges();
                        IList<Inventory> OldInventory = Context.Inventories.Where(x => x.ProductId == ProductId && x.OpeningStock > 0).ToList();                        
                        foreach (Inventory OldDetail in OldInventory)
                        {
                            Inventory NewDetail = lInventory.FirstOrDefault(x => x.Id == OldDetail.Id);
                            if (NewDetail == null)
                            {
                               
                                Context.InventoryBatches.Where(p => p.InventoryId == OldDetail.Id).ToList().ForEach(p => Context.InventoryBatches.Remove(p));
                                if (OldDetail.Purchased == 0 && OldDetail.In == 0 && OldDetail.Sold == 0 && OldDetail.Out == 0 && OldDetail.ToPatient == 0 && OldDetail.Damage == 0)
                                {
                                    Context.Inventories.Remove(Context.Inventories.Find(OldDetail.Id));
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    Inventory InventoryDetails = Context.Inventories.Find(OldDetail.Id);
                                    InventoryDetails.OpeningStock = 0;
                                    Context.Entry(Context.Inventories.Find(InventoryDetails.Id)).CurrentValues.SetValues(InventoryDetails);
                                    Context.SaveChanges();
                                }


                            }
                            else
                            {
                                lInventory.Remove(NewDetail);
                                Context.InventoryBatches.Where(p => p.InventoryId == OldDetail.Id).ToList().ForEach(p => Context.InventoryBatches.Remove(p));

                                Inventory InventoryDetails = Context.Inventories.Find(NewDetail.Id);
                                NewDetail.Purchased = InventoryDetails.Purchased;
                                NewDetail.Sold = InventoryDetails.Sold;
                                NewDetail.In = InventoryDetails.In;
                                NewDetail.Damage = InventoryDetails.Damage;
                                NewDetail.ToPatient = InventoryDetails.ToPatient;
                                NewDetail.Out = InventoryDetails.Out;
                                NewDetail.Adjust = InventoryDetails.Adjust;
                                Context.Entry(InventoryDetails).CurrentValues.SetValues(NewDetail);
                                Context.SaveChanges();                                
                            }
                        }
                        foreach (Inventory Inventory in lInventory)
                        {
                            Inventory InventoryFromDB = Context.Inventories.FirstOrDefault(x => x.ProductId == Inventory.ProductId && x.InventoryLocationId == Inventory.InventoryLocationId);
                            if (InventoryFromDB == null)
                            {
                                Context.Inventories.Add(Inventory);
                                Context.SaveChanges();
                            }
                            else
                            {
                                Inventory.Id = InventoryFromDB.Id;
                                Inventory.Purchased = InventoryFromDB.Purchased;
                                Inventory.Sold = InventoryFromDB.Sold;
                                Inventory.In = InventoryFromDB.In;
                                Inventory.Damage = InventoryFromDB.Damage;
                                Inventory.ToPatient = InventoryFromDB.ToPatient;
                                Inventory.Out = InventoryFromDB.Out;
                                Inventory.Adjust = InventoryFromDB.Adjust;
                                Context.Entry(Context.Inventories.Find(InventoryFromDB.Id)).CurrentValues.SetValues(Inventory);
                                Context.SaveChanges();
                            }
                        }
                        Product ProductFDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                        if (ProductFDB != null)
                        {
                            ProductFDB._isInventoryAtBatch = false;
                            CatalogProductManager.Instance.UpdateProductWithContext(ProductFDB, Context);
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
        public Inventory AddInventory(Inventory inventory)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.Inventories.Add(inventory);
                Context.SaveChanges();
            }
            return inventory;
        }
        public Inventory UpdateInventory(Inventory Inventory)
        {
            Inventory InventoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryInfo = Context.Inventories.FirstOrDefault(x=>x.ProductId== Inventory.ProductId);
                Context.Entry(InventoryInfo).CurrentValues.SetValues(Inventory);
                Context.SaveChanges();
            }
            return InventoryInfo;
        }
        public InventoryBatch UpdateInventoryBatch(InventoryBatch InventoryBatch)
        {
            InventoryBatch InventoryBatchInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatchInfo = Context.InventoryBatches.Find(InventoryBatch.Id);
                Context.Entry(InventoryBatchInfo).CurrentValues.SetValues(InventoryBatch);
                Context.SaveChanges();
            }
            return InventoryBatchInfo;
        }
        public InventoryBatch GetInventoryBatchDetail(long ProductId, string BatchNo)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.BatchNo == BatchNo && x.Inventory.ProductId == ProductId);
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryBatchDetail(long ProductId,string BatchNo,long InvLocationId)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.BatchNo == BatchNo &&x.Inventory.ProductId== ProductId && x.Inventory.InventoryLocationId==InvLocationId);
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryBatchDetail(long ProductId, string BatchNo,long InvLocationId, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = null;
            InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.BatchNo == BatchNo && x.Inventory.ProductId == ProductId && x.Inventory.InventoryLocationId== InvLocationId);
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchDetailbySearchText(long ProductId, string BatchNo,long LocationId)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Inventory.InventoryLocation").Where(x => x.BatchNo.Contains(BatchNo) && x.Inventory.ProductId == ProductId && x.Inventory.InventoryLocationId== LocationId).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchDetailbySearchText(long ProductId,long LocationId)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Inventory.InventoryLocation").Include("Product").Where(x => x.Inventory.ProductId == ProductId && x.Inventory.InventoryLocationId== LocationId).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchDetailbySearchText(long ProductId, string BatchNo)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Inventory.InventoryLocation").Where(x => x.BatchNo.Contains(BatchNo) && x.Inventory.ProductId == ProductId ).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public DataTable GetInventoryBatchCode(long ProductId, string BatchNo, long LocationId)
        {
            string ConString = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConString = Context.Database.GetDbConnection().ConnectionString;
            }

            using (var connection = new MySqlConnection(ConString))
            {
                connection.Open();

                string sqlQuery = @"
            SELECT 
                IB.Id, IB.InventoryId, IB.ProductId, IB.BatchNo,
                IB.ExpDate, IB.Purchased, IB.Sold, IB.OpeningStock,
                IB.In, IB.Out, IB.Damage, IB.ToPatient,
                IB.Adjust, IB.RetailUOM, IB.RetailXFactor,
                IB.WholesaleUOM, IB.WholesaleXFactor,
                IB.PurchasePrice, IB.Cost, IB.RetailSalePrice,
                IB.WholeSalePrice, IB.MaxRetailPrice,
                IB.StockUOM, IB.CreatedDate, IB.CreatedBy,
                IB.LastModifiedDate, IB.LastModifiedBy, IB.CompanyId
            FROM
                inventorybatches IB
                LEFT JOIN
                inventories I ON IB.InventoryId = I.Id
            WHERE
                IB.BatchNo = @BatchNo
                AND (I.ProductId = @ProductId OR I.ProductId IS NULL)
                AND (I.InventoryLocationId = @LocationId OR I.InventoryLocationId IS NULL);";

                using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@BatchNo", BatchNo);
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@LocationId", LocationId);

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public IList<InventoryBatch> GetInventoryBatchDetailbyExactSearchText(long ProductId, string BatchNo, long LocationId)
        {
            string ConString = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConString = Context.Database.GetDbConnection().ConnectionString;
            }
            using (var connection = new MySqlConnection(ConString))
            {
                connection.Open();

                string sqlQuery = @"
                            SELECT 
                                IB.Id, IB.InventoryId, IB.ProductId, IB.BatchNo,
                                IB.ExpDate, IB.Purchased, IB.Sold, IB.OpeningStock,
                                IB.In, IB.Out, IB.Damage, IB.ToPatient,
                                IB.Adjust, IB.RetailUOM, IB.RetailXFactor,
                                IB.WholesaleUOM, IB.WholesaleXFactor,
                                IB.PurchasePrice, IB.Cost, IB.RetailSalePrice,
                                IB.WholeSalePrice, IB.MaxRetailPrice,
                                IB.StockUOM, IB.CreatedDate, IB.CreatedBy,
                                IB.LastModifiedDate, IB.LastModifiedBy, IB.CompanyId
                            FROM
                                inventorybatches IB
                                    LEFT JOIN
                                inventories I ON IB.InventoryId = I.Id
                            WHERE
                                IB.BatchNo = @BatchNo
                                    AND (I.ProductId = @ProductId
                                    OR I.ProductId IS NULL)
                                    AND (I.InventoryLocationId = @LocationId
                                    OR I.InventoryLocationId IS NULL);";
                using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@BatchNo", BatchNo);
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@LocationId", LocationId);

                    using (var reader = command.ExecuteReader())
                    {
                        var inventoryBatches = new List<InventoryBatch>();

                        while (reader.Read())
                        {
                            var inventoryBatch = new InventoryBatch
                            {
                                BatchNo = reader.GetString("BatchNo"),
                                ExpDate = reader.GetDateTime("ExpDate")
                                // Map the columns from the reader to your InventoryBatch object
                            };

                            inventoryBatches.Add(inventoryBatch);
                        }

                        return inventoryBatches;
                    }
                }
            }

            // ----------------------------

            //using (AccountMasterContext Context = new AccountMasterContext())
            //{
            //    return Context.InventoryBatches
            //        .Where(x => x.BatchNo == BatchNo &&
            //                    (x.Inventory == null || x.Inventory.ProductId == ProductId) &&
            //                    (x.Inventory == null || x.Inventory.InventoryLocationId == LocationId))
            //        .ToList();
            //}

            // ---------------------------------------------------
            //IList<InventoryBatch> InventoryBatch = null;
            //using (AccountMasterContext Context = new AccountMasterContext())
            //{
            //    InventoryBatch = Context.InventoryBatches.Where(x => x.BatchNo==BatchNo && x.Inventory.ProductId == ProductId && x.Inventory.InventoryLocationId == LocationId).ToList<InventoryBatch>();
            //}
            //return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchDetailbyExactSearchText(long ProductId, string BatchNo)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Where(x => x.BatchNo==BatchNo && x.Inventory.ProductId == ProductId).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryStockBatchbyProductId(long ProductId)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Where(x => x.ProductId == ProductId && x.OpeningStock>0).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchbyProductId(long ProductId, long InvLocationId)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Product").Where(x => x.ProductId == ProductId && x.Inventory.InventoryLocationId== InvLocationId).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetInventoryBatchbyCompanyId(long CompanyId)
        {
            IList<InventoryBatch> InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Product").Include("Inventory.InventoryLocation").Where(x => x.CompanyId == CompanyId).ToList<InventoryBatch>();
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryByBatchId(long BatchId)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.Id == BatchId);
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryByBatchIdWithLocation(long BatchId)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Inventory.InventoryLocation").FirstOrDefault(x => x.Id == BatchId);
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryByBatchIdWithLocation(long BatchId, long InvLocationId)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.Include("Inventory.InventoryLocation").FirstOrDefault(x => x.Id == BatchId && x.Inventory.InventoryLocationId == InvLocationId);
            }
            return InventoryBatch;
        }
        public InventoryBatch GetInventoryByBatchId(long BatchId, long InvLocationId)
        {
            InventoryBatch InventoryBatch = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.Id == BatchId && x.Inventory.InventoryLocationId== InvLocationId);
            }
            return InventoryBatch;
        }
        public Inventory GetInventoryByProductId(long ProductId)
        {
            Inventory Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.FirstOrDefault(x => x.ProductId == ProductId);
            }
            return Inventory;
        }
        public IList<Inventory> ListInventoryByCompanyId(long CompanyId)
        {
            IList <Inventory> Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("Product").Include("InventoryLocation").Where(x => x.CompanyId == CompanyId).ToList<Inventory>();
            }
            return Inventory;
        }
        public IList<Inventory> ListInventoryByProductId(long ProductId)
        {
            IList<Inventory> Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("InventoryBatchs").Where(x => x.ProductId == ProductId).ToList();
            }
            return Inventory;
        }
        public IList<Inventory> ListInventoryByProductIdWithLocation(long ProductId)
        {
            IList<Inventory> Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("InventoryLocation").Where(x => x.ProductId == ProductId).ToList();
            }
            return Inventory;
        }
        public IList<Inventory> ListInventoryOpeningStockByProductId(long ProductId)
        {
            IList<Inventory> Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("InventoryBatchs").Where(x => x.ProductId == ProductId && x.OpeningStock>0 ).ToList();
            }
            return Inventory;
        }
        public Inventory GetInventoryByProductId(long ProductId,long InvLocationId, AccountMasterContext Context)
        {
            Inventory Inventory = null;
            Inventory = Context.Inventories.FirstOrDefault(x => x.ProductId == ProductId && x.InventoryLocationId== InvLocationId);
            return Inventory;
        }
        public Inventory GetInventoryByProductId(long ProductId, long InvLocationId)
        {
            Inventory Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("InventoryBatchs").FirstOrDefault(x => x.ProductId == ProductId && x.InventoryLocationId == InvLocationId);
            }
            return Inventory;
        }

        //Add from purchase
        public void RecordPurchaseInDelete(PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            foreach (PurchaseDetails Details in PurchaseEntry.PurchaseDetails.ToList())
            {
                Details.Quantity = 0;
                Details.FreeQuantity = 0;
                if(Details.isBatch)
                {
                    InventoryBatch InventoryBatch = GetInventoryBatchDetail((long)Details.ProductId,Details.BatchNo, (long)PurchaseEntry.InventoryLocationId);
                    if (InventoryBatch != null)
                    {
                        Details.Retailprice = InventoryBatch.RetailSalePrice;
                        Details.Wholesaleprice = InventoryBatch.WholeSalePrice;
                        Details.Msrp = InventoryBatch.MaxRetailPrice;

                        Details.RetailUOM = InventoryBatch.RetailUOM;
                        Details.RetailXFactor = InventoryBatch.RetailXFactor;
                        Details.WholesaleUOM = InventoryBatch.WholesaleUOM;
                        Details.WholesaleXFactor = InventoryBatch.WholesaleXFactor;
                    }
                }
                RecordPurchaseDetails(Details, PurchaseEntry, Context);
            }
        }
        public void RecordPurchase(PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            foreach (PurchaseDetails Details in PurchaseEntry.PurchaseDetails.ToList())
            {
                RecordPurchaseDetails(Details, PurchaseEntry, Context);
            }
        }
        public void RecordPurchaseDetails(PurchaseDetails Details, PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        { 
            Inventory inventory = GetInventory(Details, PurchaseEntry, Context);
            if (inventory.Id != 0)
            {
                Inventory lInventory = Context.Inventories.Find(inventory.Id);
                Context.Entry(lInventory).CurrentValues.SetValues(inventory);
            }
            else
            {
                inventory.InventoryLocationId = (long)PurchaseEntry.InventoryLocationId;
                Context.Inventories.Add(inventory);
            }
            Context.SaveChanges();
            if (Details.isBatch)
            {
                InventoryBatch InventoryBatch = GetBatch(Details, inventory, PurchaseEntry, Context);
                if(InventoryBatch.Id != 0)
                {
                    InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                }
                else
                {
                    Context.InventoryBatches.Add(InventoryBatch);
                }
                Context.SaveChanges();
            }

            //for price update
            PurchaseDetails PurchaseDetails = null;
            if(Details.Id != 0L)
            {
                PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(Details.Id);
            }

        }
        public Inventory GetInventory(PurchaseDetails Details, PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {          
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity + Details.FreeQuantity;
            Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, (long)PurchaseEntry.InventoryLocationId, Context);
            if (lInventory != null)
            {
                Inventory.Sold = lInventory.Sold;
                Inventory.OpeningStock = lInventory.OpeningStock;
                Inventory.StockUOM = lInventory.StockUOM;
                Inventory.In = lInventory.In;
                Inventory.Damage = lInventory.Damage;
                Inventory.ToPatient = lInventory.ToPatient;
                Inventory.Out = lInventory.Out;
                Inventory.Adjust = lInventory.Adjust;

                double OldQut = 0;
                if (Details.Id != 0)
                {
                    PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(Details.Id);
                    OldQut = PurchaseDetails?.Quantity ?? 0 + PurchaseDetails?.FreeQuantity ?? 0;
                }

                if(OldQut> Qut)
                {
                    if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                    {
                        Inventory.Purchased = lInventory.Purchased - (OldQut - Qut);
                    }
                    else
                    {
                        Inventory.Purchased = lInventory.Purchased + (OldQut - Qut);
                    }
                }
                else
                {
                    if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                    {
                        Inventory.Purchased = lInventory.Purchased + (Qut- OldQut);
                    }
                    else
                    {
                        Inventory.Purchased = lInventory.Purchased - (Qut - OldQut);
                    }
                }
                Inventory.Id = lInventory.Id;
                Inventory.InventoryLocationId = lInventory.InventoryLocationId;
            }
            else
            {
                Inventory.Purchased = Qut;
                Inventory.Id = 0L;
            }
            return Inventory;
        }
        public InventoryBatch GetBatch(PurchaseDetails Details,Inventory InventoryFromDB, PurchaseEntry PurchaseEntry, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;

            if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
            {
                InventoryBatch.PurchasePrice = Details.PurchasePrice;
                InventoryBatch.Cost = Details.PurchaseCost;
                InventoryBatch.RetailSalePrice = Details.Retailprice;
                InventoryBatch.WholeSalePrice = Details.Wholesaleprice;
                InventoryBatch.MaxRetailPrice = Details.Msrp;

                InventoryBatch.RetailUOM = Details.RetailUOM;
                InventoryBatch.RetailXFactor = Details.RetailXFactor;
                InventoryBatch.WholesaleUOM = Details.WholesaleUOM;
                InventoryBatch.WholesaleXFactor = Details.WholesaleXFactor;
            }

            double Qut = Details.Quantity + Details.FreeQuantity;
            if (InventoryFromDB != null)
            {
                InventoryBatch.InventoryId = InventoryFromDB.Id;
                InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, (long)PurchaseEntry.InventoryLocationId, Context);
                if(lInventoryBatch!=null)
                {
                    //for price update
                    if (Details.Id != 0L)
                    {
                        PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(Details.Id);
                        if (PurchaseDetails?.PurchasePrice == Details.PurchasePrice)
                        {
                            InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                        }
                    }
                   

                    InventoryBatch.Sold = lInventoryBatch.Sold;
                    InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                    InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                    InventoryBatch.In = lInventoryBatch.In;
                    InventoryBatch.Damage = lInventoryBatch.Damage;
                    InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                    InventoryBatch.Out = lInventoryBatch.Out;
                    InventoryBatch.Adjust = lInventoryBatch.Adjust;
                    if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN)
                    {
                        InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                        InventoryBatch.Cost = lInventoryBatch.Cost;
                        InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                        InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                        InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;

                        InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                        InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                        InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                        InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;
                    }
                    Product Product = Context.Products.FirstOrDefault(x => x.Id == Details.ProductId);
                    if (Product != null)
                    {
                        InventoryBatch.RetailUOM = InventoryBatch.RetailUOM == null ? Product.RetailUOM: lInventoryBatch.RetailUOM;
                        InventoryBatch.RetailXFactor = InventoryBatch.RetailXFactor == 0 ? Product .RetailXFactor: lInventoryBatch.RetailXFactor;
                        InventoryBatch.WholesaleUOM = InventoryBatch.WholesaleUOM == null ? Product .WholesaleUOM: lInventoryBatch.WholesaleUOM;
                        InventoryBatch.WholesaleXFactor = InventoryBatch.WholesaleXFactor == 0 ? Product .WholesaleXFactor: lInventoryBatch.WholesaleXFactor;
                        InventoryBatch.RetailSalePrice = InventoryBatch.RetailSalePrice == 0 ? Product.RetailPrice : lInventoryBatch.RetailSalePrice;
                        InventoryBatch.WholeSalePrice = InventoryBatch.WholeSalePrice == 0 ? Product.WholdSalePrice : lInventoryBatch.WholeSalePrice;
                        InventoryBatch.MaxRetailPrice = InventoryBatch.MaxRetailPrice == 0 ? Product.Msrp : lInventoryBatch.MaxRetailPrice;
                    }

                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(Details.Id);
                        OldQut = PurchaseDetails?.Quantity ?? 0 + PurchaseDetails?.FreeQuantity ?? 0;
                    }
                    if (OldQut > Qut)
                    {
                        if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                        {
                            InventoryBatch.Purchased = lInventoryBatch.Purchased - (OldQut - Qut);
                        }
                        else
                        {
                            InventoryBatch.Purchased = lInventoryBatch.Purchased + (OldQut - Qut);
                        }
                    }
                    else
                    {
                        if (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE)
                        {
                            InventoryBatch.Purchased = lInventoryBatch.Purchased + (Qut - OldQut);
                        }
                        else
                        {
                            InventoryBatch.Purchased = lInventoryBatch.Purchased - (Qut - OldQut);
                        }
                    }
                    InventoryBatch.Id = lInventoryBatch.Id;
                }
                else
                {
                    InventoryBatch.Id = 0L;
                    InventoryBatch.Purchased = Qut;
                }
            }
            else
            {
                InventoryBatch.Purchased = Qut;
            }

            return InventoryBatch;
        }
        //opening stock
        public void ResetOpeningStock(List<StockMovementDetail> lStockMovementDetail, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in lStockMovementDetail)
            {
                Details.Quantity = 0;
                ResetOpeningStock(Details.StockMovement.InventoryStockLocationId,Details, Context);
            }
        }
        public void ResetOpeningStock(long LocationId,StockMovementDetail Details, AccountMasterContext Context)
        {           
            Product Product = Context.Products.FirstOrDefault(x => x.Id == Details.ProductId);
            if (Product != null)
            {
                double Qty = Product.RetailUOM == Details.Uom ? Details.Quantity / Product.RetailXFactor : Details.Quantity;
                Inventory Inventory = Context.Inventories.FirstOrDefault(x => x.InventoryLocationId == LocationId && x.ProductId == Details.ProductId);
                if (Inventory != null)
                {
                    Inventory.OpeningStock = Qty;
                    Inventory.StockDate = Details.StockDate;
                    Context.Entry(Context.Inventories.Find(Inventory.Id)).CurrentValues.SetValues(Inventory);
                    Context.SaveChanges();
                }
                if (Details.isBatch && Inventory != null)
                {
                    InventoryBatch InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.InventoryId == Inventory.Id && x.BatchNo == Details.BatchNo);
                    if (InventoryBatch != null)
                    {
                        InventoryBatch.OpeningStock = Qty;
                        InventoryBatch.StockDate = Details.StockDate;
                        Context.Entry(Context.InventoryBatches.Find(InventoryBatch.Id)).CurrentValues.SetValues(InventoryBatch);
                        Context.SaveChanges();
                    }
                }
                    
            }
        }
        public void RecordOpeningStock(StockMovementOpeningStock StockMovementInfo, AccountMasterContext Context)
        {
           
            foreach (StockMovementDetail Details in StockMovementInfo.StockMovementDetails)
            {
                double UpdateQty = 0;
                Product Product = Context.Products.FirstOrDefault(x => x.Id == Details.ProductId);
                if (Product != null)
                {                   
                    double Qty = Product.RetailUOM == Details.Uom ? Details.Quantity / Product.RetailXFactor : Details.Quantity;
                    Inventory inventory = Context.Inventories.FirstOrDefault(x => x.InventoryLocationId == StockMovementInfo.InventoryStockLocationId && x.ProductId == Details.ProductId);
                    if (inventory != null)
                    {
                        UpdateQty += Qty;
                        inventory.OpeningStock += Qty; // UpdateQty;
                        inventory.StockDate = Details.StockDate;
                        Context.Entry(Context.Inventories.Find(inventory.Id)).CurrentValues.SetValues(inventory);
                        Context.SaveChanges();
                    }
                    else
                    {
                        inventory = new Inventory
                        {
                            ProductId = (long)Details.ProductId,
                            CompanyId = Details.CompanyId,
                            CostCenterId = Details.CostCenterId,
                            InventoryLocationId = StockMovementInfo.InventoryStockLocationId,
                            StockDate = Details.StockDate,
                            StockUOM = Details.Uom,
                            OpeningStock = Qty,
                        };
                        Context.Inventories.Add(inventory);
                        Context.SaveChanges();

                    }
                    if (Details.isBatch && inventory != null)
                    {
                        InventoryBatch InventoryBatch = Context.InventoryBatches.FirstOrDefault(x => x.InventoryId == inventory.Id && x.BatchNo == Details.BatchNo);
                        if (InventoryBatch != null)
                        {
                            InventoryBatch.ExpDate = Details.ExpDate;
                            InventoryBatch.PurchasePrice = Details.PurchasePrice;
                            InventoryBatch.Cost = Details.PurchaseCost;
                            InventoryBatch.RetailSalePrice = Details.Retailprice;
                            InventoryBatch.WholeSalePrice = Details.Wholesaleprice;
                            InventoryBatch.MaxRetailPrice = Details.Msrp;
                            InventoryBatch.StockDate = Details.StockDate;
                            InventoryBatch.OpeningStock += Qty;
                            Context.Entry(Context.InventoryBatches.Find(InventoryBatch.Id)).CurrentValues.SetValues(InventoryBatch);
                            Context.SaveChanges();
                        }
                        else
                        {
                            InventoryBatch lInventoryBatch = new InventoryBatch();
                            lInventoryBatch.ProductId = (long)Details.ProductId;
                            lInventoryBatch.CompanyId = Details.CompanyId;
                            lInventoryBatch.CostCenterId = Details.CostCenterId;
                            lInventoryBatch.InventoryId = inventory.Id;
                            lInventoryBatch.StockUOM = Details.Uom;
                            lInventoryBatch.OpeningStock = Qty;
                            lInventoryBatch.BatchNo = Details.BatchNo;
                            lInventoryBatch.ExpDate = Details.ExpDate;
                            lInventoryBatch.RetailXFactor = Details.RetailXFactor;
                            lInventoryBatch.RetailUOM = Details.RetailUOM;
                            lInventoryBatch.WholesaleXFactor = Details.WholesaleXFactor;
                            lInventoryBatch.WholesaleUOM = Details.WholesaleUOM;
                            lInventoryBatch.PurchasePrice = Details.PurchasePrice;
                            lInventoryBatch.Cost = Details.PurchaseCost;
                            lInventoryBatch.RetailSalePrice = Details.Retailprice;
                            lInventoryBatch.WholeSalePrice = Details.Wholesaleprice;
                            lInventoryBatch.MaxRetailPrice = Details.Msrp;
                            lInventoryBatch.StockDate = Details.StockDate;
                            Context.InventoryBatches.Add(lInventoryBatch);
                            Context.SaveChanges();
                        }
                        Product lProduct= Context.Products.Find(Details.ProductId);
                        if(lProduct!=null)
                        {
                            lProduct.RetailPrice = Details.Retailprice;
                            lProduct.WholdSalePrice = Details.Wholesaleprice;
                            lProduct.Msrp = Details.Msrp;
                            lProduct.CostPrice = Details.PurchaseCost;
                            lProduct.PurchasePrice = Details.PurchasePrice;
                            CatalogProductManager.Instance.UpdateProductWithContext(lProduct, Context);
                        }
                    }
                }
            }
        }
        
        //Add from Sales
        public void RecordSalesInDelete(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            foreach (SaleDetail Details in SaleEntry.SaleDetails)
            {
                Details.Quantity = 0;
                Details.FreeQuantity = 0;
                RecordSalesDetails(Details, Context, (long)SaleEntry.InventoryLocationId, SaleEntry.EntryType/*,SaleEntry.SaleType*/);
            }
        }
        public void RecordSales(SaleEntry SaleEntry, AccountMasterContext Context)
        {
            foreach (SaleDetail Details in SaleEntry.SaleDetails)
            {
                RecordSalesDetails(Details, Context, (long)SaleEntry.InventoryLocationId, SaleEntry.EntryType/*, SaleEntry.SaleType*/);
            }
        }
        public void RecordSalesDetails(SaleDetail Details, AccountMasterContext Context,long InvLocationId, Entrytype Entrytype/*,SaleType SaleType*/)
        {
            Inventory InventoryFromDB = null;
            Inventory Inventory = GetInventory(Details, Context, InvLocationId, Entrytype/*, SaleType*/);
            if (Inventory.Id != 0)
            {
                Inventory lInventory = Context.Inventories.Find(Inventory.Id);
                Context.Entry(lInventory).CurrentValues.SetValues(Inventory);
                InventoryFromDB = Inventory;
            }
           
            Context.SaveChanges();
            if (Details.isBatch)
            {
                InventoryBatch InventoryBatch = GetBatch(Details, InventoryFromDB, InvLocationId, Context, Entrytype/*, SaleType*/);
                if (InventoryBatch.Id != 0)
                {
                    InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                }
                
                Context.SaveChanges();
            }
        }
        public Inventory GetInventory(SaleDetail Details, AccountMasterContext Context, long InvLocationId, Entrytype Entrytype/*, SaleType SaleType*/)
        {
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity + Details.FreeQuantity;

            InventoryBatch InventoryBatchDB = null;
            if (Details.isBatch)
            {
                InventoryBatchDB = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, InvLocationId, Context);
            }
            
            Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
            Qut = (Qut / (Product.RetailUOM==Details.Uom ? ((InventoryBatchDB==null?Product.WholesaleXFactor: InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor: InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor: InventoryBatchDB.WholesaleXFactor)));

            Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, InvLocationId, Context);
            if (lInventory != null)
            {
                Inventory.InventoryLocationId = lInventory.InventoryLocationId;
                Inventory.Purchased = lInventory.Purchased;
                Inventory.OpeningStock = lInventory.OpeningStock;
                Inventory.StockUOM = lInventory.StockUOM;
                Inventory.In = lInventory.In;
                Inventory.Damage = lInventory.Damage;
                Inventory.ToPatient = lInventory.ToPatient;
                Inventory.Out = lInventory.Out;
                Inventory.Adjust = lInventory.Adjust;
                Inventory.StockDate = lInventory.StockDate;
                double OldQut = 0;
                if (Details.Id != 0)
                {
                    SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetail(Details.Id);
                    if (SaleDetail!=null)
                    {
                        OldQut = SaleDetail?.Quantity ?? 0 + SaleDetail?.FreeQuantity ?? 0;
                        OldQut = (OldQut / (Product.RetailUOM == SaleDetail.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));
                    }
                }

                if (OldQut > Qut)
                {
                    if(Entrytype==Entrytype.SALE)
                    {
                        Inventory.Sold = lInventory.Sold - (OldQut - Qut);
                    }
                    else
                    {
                        Inventory.Sold = lInventory.Sold + (OldQut - Qut);
                    }
                }
                else
                {
                    if (Entrytype == Entrytype.SALE)
                    {
                        Inventory.Sold = lInventory.Sold + (Qut - OldQut);
                    }
                    else
                    {
                        Inventory.Sold = lInventory.Sold - (Qut - OldQut);
                    }
                }
                Inventory.Id = lInventory.Id;
            }
            
            return Inventory;
        }
        public InventoryBatch GetBatch(SaleDetail Details, Inventory InventoryFromDB,long InvLocationId, AccountMasterContext Context, Entrytype Entrytype/*, SaleType SaleType*/)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;

            double Qut = Details.Quantity + Details.FreeQuantity;
            
            if (InventoryFromDB != null)
            {
                InventoryBatch.InventoryId = InventoryFromDB.Id;
                InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, InvLocationId, Context);
                if (lInventoryBatch != null)
                {
                    Qut = (Qut / (lInventoryBatch.RetailUOM == Details.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));

                    InventoryBatch.Purchased = lInventoryBatch.Purchased;
                    InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                    InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                    InventoryBatch.In = lInventoryBatch.In;
                    InventoryBatch.Damage = lInventoryBatch.Damage;
                    InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                    InventoryBatch.Out = lInventoryBatch.Out;
                    InventoryBatch.Adjust = lInventoryBatch.Adjust;

                    InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                    InventoryBatch.Cost = lInventoryBatch.Cost;
                    InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                    InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                    InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;
                    InventoryBatch.StockDate = lInventoryBatch.StockDate;
                    InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                    InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                    InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                    InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;

                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetail(Details.Id);
                        if (SaleDetail!=null)
                        {
                            OldQut = SaleDetail?.Quantity ?? 0 + SaleDetail?.FreeQuantity ?? 0;
                            OldQut = (OldQut / (lInventoryBatch.RetailUOM == SaleDetail.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                        }
                    }
                    if (OldQut > Qut)
                    {
                        if (Entrytype == Entrytype.SALE)
                        {
                            InventoryBatch.Sold = lInventoryBatch.Sold - (OldQut - Qut);
                        }
                        else
                        {
                            InventoryBatch.Sold = lInventoryBatch.Sold + (OldQut - Qut);
                        }
                    }
                    else
                    {
                        if (Entrytype == Entrytype.SALE)
                        {
                            InventoryBatch.Sold = lInventoryBatch.Sold + (Qut - OldQut);
                        }
                        else
                        {
                            InventoryBatch.Sold = lInventoryBatch.Sold - (Qut - OldQut);
                        }

                    }
                    InventoryBatch.Id = lInventoryBatch.Id;
                }
                
            }

            return InventoryBatch;
        }


        //ADD STOCK MOVEMENT OUT
        public void RecordStockMovementOutInDelete(StockMovementOut StockMovementOut, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementOut.StockMovementDetails)
            {
                Details.Quantity = 0;
                Details.FreeQuantity = 0;
                RecordStockMovementOutDetails(Details, Context, StockMovementOut);
            }
        }
        
        
        public void RecordStockMovementOut(StockMovementOut StockMovementOut, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementOut.StockMovementDetails)
            {
                RecordStockMovementOutDetails(Details, Context, StockMovementOut);
            }
        }
        public void RecordStockMovementOutDetails(StockMovementDetail Details, AccountMasterContext Context, StockMovementOut StockMovementOut)
        {
                //Stock Out

                Inventory InventoryFromDB = null;
                Inventory Inventory = GetInventory(Details, Context, StockMovementOut);
                if (Inventory.Id != 0)
                {
                    Inventory lInventory = Context.Inventories.Find(Inventory.Id);
                    Context.Entry(lInventory).CurrentValues.SetValues(Inventory);
                    InventoryFromDB = Inventory;
                }

                Context.SaveChanges();
                if (Details.isBatch)
                {
                    InventoryBatch InventoryBatch = GetBatch(Details, InventoryFromDB, StockMovementOut, Context);
                    if (InventoryBatch.Id != 0)
                    {
                        InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                        Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                    }

                    Context.SaveChanges();
                }
            
        }
       
        public Inventory GetInventory(StockMovementDetail Details, AccountMasterContext Context, StockMovementOut StockMovementOut)
        {
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity + Details.FreeQuantity;

                InventoryBatch InventoryBatchDB = null;
                if (Details.isBatch)
                {
                    InventoryBatchDB = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementOut.InventoryStockLocationId, Context);
                }
                //for stock
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
                Qut = (Qut / (Product.RetailUOM == Details.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));

                Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, StockMovementOut.InventoryStockLocationId, Context);
                if (lInventory != null)
                {
                    Inventory.InventoryLocationId = lInventory.InventoryLocationId;
                    Inventory.Purchased = lInventory.Purchased;
                    Inventory.OpeningStock = lInventory.OpeningStock;
                    Inventory.StockUOM = lInventory.StockUOM;
                    Inventory.In = lInventory.In;
                    Inventory.Damage = lInventory.Damage;
                    Inventory.ToPatient = lInventory.ToPatient;
                    Inventory.Sold = lInventory.Sold;
                    Inventory.Adjust = lInventory.Adjust;


                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                        if (StockMovementDetail != null)
                        {                            
                            OldQut = StockMovementDetail.Quantity + StockMovementDetail.FreeQuantity;
                            OldQut = (OldQut / (Product.RetailUOM == StockMovementDetail.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));
                        }
                    }

                    if (OldQut > Qut)
                    {
                        Inventory.Out = lInventory.Out - (OldQut - Qut);
                    }
                    else
                    {
                        Inventory.Out = lInventory.Out + (Qut - OldQut);
                    }
                    Inventory.Id = lInventory.Id;
                }
            
            
            return Inventory;
            

        }
        public InventoryBatch GetBatch(StockMovementDetail Details, Inventory InventoryFromDB, StockMovementOut StockMovementOut, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;
            double Qut = Details.Quantity + Details.FreeQuantity;
            
                if (InventoryFromDB != null)
                {
                    InventoryBatch.InventoryId = InventoryFromDB.Id;
                    InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementOut.InventoryStockLocationId, Context);
                    if (lInventoryBatch != null)
                    {
                        //for stock
                        Qut = (Qut / (lInventoryBatch.RetailUOM == Details.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));

                        InventoryBatch.Purchased = lInventoryBatch.Purchased;
                        InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                        InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                        InventoryBatch.In = lInventoryBatch.In;
                        InventoryBatch.Damage = lInventoryBatch.Damage;
                        InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                        InventoryBatch.Sold = lInventoryBatch.Sold;
                        InventoryBatch.Adjust = lInventoryBatch.Adjust;


                        InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                        InventoryBatch.Cost = lInventoryBatch.Cost;
                        InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                        InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                        InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;

                        InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                        InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                        InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                        InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;

                        double OldQut = 0;
                        if (Details.Id != 0)
                        {
                            StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                            if (StockMovementDetail != null)
                            {
                                OldQut = StockMovementDetail.Quantity + StockMovementDetail.FreeQuantity;
                                OldQut = (OldQut / (lInventoryBatch.RetailUOM == StockMovementDetail.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                            }
                        }
                        if (OldQut > Qut)
                        {
                            InventoryBatch.Out = lInventoryBatch.Out - (OldQut - Qut);
                        }
                        else
                        {
                            InventoryBatch.Out = lInventoryBatch.Out + (Qut - OldQut);
                        }
                        InventoryBatch.Id = lInventoryBatch.Id;
                    }
                }
            
           
            return InventoryBatch;
        }

        //ADD STOCK ADJUSTMENT
        public void RecordStockAdjustmentDelete(StockMovementAdjustment StockMovementAdjustment, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementAdjustment.StockMovementDetails)
            {
                Details.Quantity = 0;
                RecordStockAdjustmentDetails(Details, Context, StockMovementAdjustment);
            }
        }
        public void RecordStockAdjustment(StockMovementAdjustment StockMovementAdjustment, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementAdjustment.StockMovementDetails)
            {
                RecordStockAdjustmentDetails(Details, Context, StockMovementAdjustment);
            }
        }
        public void RecordStockAdjustmentDetails(StockMovementDetail Details, AccountMasterContext Context, StockMovementAdjustment StockMovementAdjustment)
        {
            Inventory InventoryFromDB = null;
            Inventory Inventory = GetAdjustmentInventory(Details, Context, StockMovementAdjustment);
            if (Inventory.Id != 0)
            {
                Inventory lInventory = Context.Inventories.Find(Inventory.Id);
                Context.Entry(lInventory).CurrentValues.SetValues(Inventory);
                InventoryFromDB = Inventory;
            }

            Context.SaveChanges();
            if (Details.isBatch)
            {
                InventoryBatch InventoryBatch = GetAdjustmentBatch(Details, InventoryFromDB, StockMovementAdjustment, Context);
                if (InventoryBatch.Id != 0)
                {
                    InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                }

                Context.SaveChanges();
            }

        }

        public Inventory GetAdjustmentInventory(StockMovementDetail Details, AccountMasterContext Context, StockMovementAdjustment StockMovementOut)
        {
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity;

            InventoryBatch InventoryBatchDB = null;
            if (Details.isBatch)
            {
                InventoryBatchDB = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementOut.InventoryStockLocationId, Context);
            }
            //for stock
            Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
            Qut = (Qut / (Product.RetailUOM == Details.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));

            Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, StockMovementOut.InventoryStockLocationId, Context);
            if (lInventory != null)
            {
                Inventory.InventoryLocationId = lInventory.InventoryLocationId;
                Inventory.Purchased = lInventory.Purchased;
                Inventory.OpeningStock = lInventory.OpeningStock;
                Inventory.StockUOM = lInventory.StockUOM;
                Inventory.In = lInventory.In;
                Inventory.Damage = lInventory.Damage;
                Inventory.ToPatient = lInventory.ToPatient;
                Inventory.Sold = lInventory.Sold;

                double OldQut = 0;
                if (Details.Id != 0)
                {
                    StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                    if (StockMovementDetail != null)
                    {
                        OldQut = StockMovementDetail.Quantity;
                        OldQut = (OldQut / (Product.RetailUOM == StockMovementDetail.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));
                    }
                }

                if (OldQut > Qut)
                {
                    Inventory.Adjust = lInventory.Adjust - (OldQut - Qut);
                }
                else
                {
                    Inventory.Adjust = lInventory.Adjust + (Qut - OldQut);
                }
                Inventory.Id = lInventory.Id;
            }


            return Inventory;


        }
        public InventoryBatch GetAdjustmentBatch(StockMovementDetail Details, Inventory InventoryFromDB, StockMovementAdjustment StockMovementAdjustment, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;
            double Qut = Details.Quantity;

            if (InventoryFromDB != null)
            {
                InventoryBatch.InventoryId = InventoryFromDB.Id;
                InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementAdjustment.InventoryStockLocationId, Context);
                if (lInventoryBatch != null)
                {
                    Qut = (Qut / (lInventoryBatch.RetailUOM == Details.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                    InventoryBatch.Purchased = lInventoryBatch.Purchased;
                    InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                    InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                    InventoryBatch.In = lInventoryBatch.In;
                    InventoryBatch.Damage = lInventoryBatch.Damage;
                    InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                    InventoryBatch.Sold = lInventoryBatch.Sold;


                    InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                    InventoryBatch.Cost = lInventoryBatch.Cost;
                    InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                    InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                    InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;

                    InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                    InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                    InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                    InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;

                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                        if (StockMovementDetail != null)
                        {
                            OldQut = StockMovementDetail.Quantity;
                            OldQut = (OldQut / (lInventoryBatch.RetailUOM == StockMovementDetail.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                        }
                    }
                    if (OldQut > Qut)
                    {
                        InventoryBatch.Adjust = lInventoryBatch.Adjust - (OldQut - Qut);
                    }
                    else
                    {
                        InventoryBatch.Adjust = lInventoryBatch.Adjust + (Qut - OldQut);
                    }
                    InventoryBatch.Id = lInventoryBatch.Id;
                }
            }


            return InventoryBatch;
        }
        //ADD STOCK DAMAGED
        public void RecordStockDamagedDelete(StockMovementDamaged StockMovementDamaged, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementDamaged.StockMovementDetails)
            {
                Details.Quantity = 0;
                RecordStockDamagedDetails(Details, Context, StockMovementDamaged);
            }
        }
        public void RecordStockDamaged(StockMovementDamaged StockMovementDamaged, AccountMasterContext Context)
        {
            foreach (StockMovementDetail Details in StockMovementDamaged.StockMovementDetails)
            {
                RecordStockDamagedDetails(Details, Context, StockMovementDamaged);
            }
        }
        public void RecordStockDamagedDetails(StockMovementDetail Details, AccountMasterContext Context, StockMovementDamaged StockMovementDamaged)
        {
            Inventory InventoryFromDB = null;
            Inventory Inventory = GetDamagedInventory(Details, Context, StockMovementDamaged);
            if (Inventory.Id != 0)
            {
                Inventory lInventory = Context.Inventories.Find(Inventory.Id);
                Context.Entry(lInventory).CurrentValues.SetValues(Inventory);
                InventoryFromDB = Inventory;
            }

            Context.SaveChanges();
            if (Details.isBatch)
            {
                InventoryBatch InventoryBatch = GetDamagedBatch(Details, InventoryFromDB, StockMovementDamaged, Context);
                if (InventoryBatch.Id != 0)
                {
                    InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                }

                Context.SaveChanges();
            }

        }

        public Inventory GetDamagedInventory(StockMovementDetail Details, AccountMasterContext Context, StockMovementDamaged StockMovementDamaged)
        {
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity;

            InventoryBatch InventoryBatchDB = null;
            if (Details.isBatch)
            {
                InventoryBatchDB = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementDamaged.InventoryStockLocationId, Context);
            }
            //for stock
            Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
            Qut = (Qut / (Product.RetailUOM == Details.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));

            Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, StockMovementDamaged.InventoryStockLocationId, Context);
            if (lInventory != null)
            {
                Inventory.InventoryLocationId = lInventory.InventoryLocationId;
                Inventory.Purchased = lInventory.Purchased;
                Inventory.OpeningStock = lInventory.OpeningStock;
                Inventory.StockUOM = lInventory.StockUOM;
                Inventory.In = lInventory.In;
                Inventory.Adjust = lInventory.Adjust;
                Inventory.ToPatient = lInventory.ToPatient;
                Inventory.Sold = lInventory.Sold;

                double OldQut = 0;
                if (Details.Id != 0)
                {
                    StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                    if (StockMovementDetail != null)
                    {
                        OldQut = StockMovementDetail.Quantity;
                        OldQut = (OldQut / (Product.RetailUOM == StockMovementDetail.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));
                    }
                }

                if (OldQut > Qut)
                {
                    Inventory.Damage = lInventory.Damage - (OldQut - Qut);
                }
                else
                {
                    Inventory.Damage = lInventory.Damage + (Qut - OldQut);
                }
                Inventory.Id = lInventory.Id;
            }


            return Inventory;


        }
        public InventoryBatch GetDamagedBatch(StockMovementDetail Details, Inventory InventoryFromDB, StockMovementDamaged StockMovementDamaged, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;
            double Qut = Details.Quantity;

            if (InventoryFromDB != null)
            {
                InventoryBatch.InventoryId = InventoryFromDB.Id;
                InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementDamaged.InventoryStockLocationId, Context);
                if (lInventoryBatch != null)
                {
                    Qut = (Qut / (lInventoryBatch.RetailUOM == Details.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                    InventoryBatch.Purchased = lInventoryBatch.Purchased;
                    InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                    InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                    InventoryBatch.In = lInventoryBatch.In;
                    InventoryBatch.Adjust = lInventoryBatch.Adjust;
                    InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                    InventoryBatch.Sold = lInventoryBatch.Sold;


                    InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                    InventoryBatch.Cost = lInventoryBatch.Cost;
                    InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                    InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                    InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;

                    InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                    InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                    InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                    InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;

                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                        if (StockMovementDetail != null)
                        {
                            OldQut = StockMovementDetail.Quantity;
                            OldQut = (OldQut / (lInventoryBatch.RetailUOM == StockMovementDetail.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                        }
                    }
                    if (OldQut > Qut)
                    {
                        InventoryBatch.Damage = lInventoryBatch.Damage - (OldQut - Qut);
                    }
                    else
                    {
                        InventoryBatch.Damage = lInventoryBatch.Damage + (Qut - OldQut);
                    }
                    InventoryBatch.Id = lInventoryBatch.Id;
                }
            }


            return InventoryBatch;
        }
        //stock mpvement In
        public void RecorStockMovementIn(StockMovementIn StockMovementIn, AccountMasterContext Context)
        {
            foreach (StockMovementDetail StockInDetails in StockMovementIn.StockMovementDetails)
            {
                RecordStockMovementInDetail(StockInDetails, Context, StockMovementIn);
            }
        }
        public void RecordStockMovementInDetail(StockMovementDetail StockInDetail, AccountMasterContext Context, StockMovementIn StockMovementIn)
        {
            Inventory inventory = GetInventoryForStockIn(StockInDetail, Context, StockMovementIn);
            if (inventory.Id != 0)
            {
                Inventory lInventory = Context.Inventories.Find(inventory.Id);
                Context.Entry(lInventory).CurrentValues.SetValues(inventory);
            }
            else
            {
                inventory.InventoryLocationId = StockMovementIn.InventoryStockLocationId;
                Context.Inventories.Add(inventory);
            }
            Context.SaveChanges();
            if (StockInDetail.isBatch)
            {
                InventoryBatch InventoryBatch = GetBatchForInMove(StockInDetail, inventory, StockMovementIn, Context);
                if (InventoryBatch.Id != 0)
                {
                    InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                }
                else
                {
                    Context.InventoryBatches.Add(InventoryBatch);
                }
                Context.SaveChanges();
            }
        }
        public Inventory GetInventoryForStockIn(StockMovementDetail Details, AccountMasterContext Context, StockMovementIn StockMovementIn)
        {
            Inventory Inventory = new Inventory();
            Inventory.ProductId = (long)Details.ProductId;
            Inventory.CompanyId = Details.CompanyId;
            Inventory.CostCenterId = Details.CostCenterId;
            double Qut = Details.Quantity + Details.FreeQuantity;

            InventoryBatch InventoryBatchDB = null;
            if (Details.isBatch)
            {
                InventoryBatchDB = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementIn.InventoryStockLocationId, Context);
            }
            //for stock
            Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
            Qut = (Qut / (Product.RetailUOM == Details.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));

            Inventory lInventory = GetInventoryByProductId((long)Details.ProductId, StockMovementIn.InventoryStockLocationId, Context);
            if (lInventory != null)
            {
                Inventory.InventoryLocationId = lInventory.InventoryLocationId;
                Inventory.Purchased = lInventory.Purchased;
                Inventory.OpeningStock = lInventory.OpeningStock;
                Inventory.StockUOM = lInventory.StockUOM;
                Inventory.Out = lInventory.Out;
                Inventory.Damage = lInventory.Damage;
                Inventory.ToPatient = lInventory.ToPatient;
                Inventory.Sold = lInventory.Sold;
                Inventory.Adjust = lInventory.Adjust;

                double OldQut = 0;
                if (Details.Id != 0)
                {
                    StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                    if (StockMovementDetail != null)
                    {
                        OldQut = StockMovementDetail.Quantity + StockMovementDetail.FreeQuantity;
                        OldQut = (OldQut / (Product.RetailUOM == StockMovementDetail.Uom ? ((InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor) * (InventoryBatchDB == null ? Product.RetailXFactor : InventoryBatchDB.RetailXFactor)) : (InventoryBatchDB == null ? Product.WholesaleXFactor : InventoryBatchDB.WholesaleXFactor)));
                    }
                }

                if (OldQut > Qut)
                {
                    Inventory.In = lInventory.In - (OldQut - Qut);
                }
                else
                {
                    Inventory.In = lInventory.In + (Qut - OldQut);
                }
                Inventory.Id = lInventory.Id;
            }
            else
            {
                Inventory.In = Qut;
                Inventory.Id = 0L;
            }
            return Inventory;
        }

        public InventoryBatch GetBatchForInMove(StockMovementDetail Details, Inventory InventoryFromDB, StockMovementIn StockMovementIn, AccountMasterContext Context)
        {
            InventoryBatch InventoryBatch = new InventoryBatch();
            InventoryBatch.BatchNo = Details.BatchNo;
            InventoryBatch.ExpDate = Details.ExpDate;
            InventoryBatch.CompanyId = Details.CompanyId;
            InventoryBatch.CostCenterId = Details.CostCenterId;
            InventoryBatch.ProductId = (long)Details.ProductId;

            Product Product = CatalogProductManager.Instance.GetProductInfoById((long)Details.ProductId);
            if (Product != null)
            {
                InventoryBatch.PurchasePrice = Product.PurchasePrice;
                InventoryBatch.Cost = Product.CostPrice;
                InventoryBatch.RetailSalePrice = Product.RetailPrice;
                InventoryBatch.WholeSalePrice = Product.WholdSalePrice;
                InventoryBatch.MaxRetailPrice = Product.Msrp;

                InventoryBatch.RetailUOM = Product.RetailUOM;
                InventoryBatch.RetailXFactor = Product.RetailXFactor;
                InventoryBatch.WholesaleUOM = Product.WholesaleUOM;
                InventoryBatch.WholesaleXFactor = Product.WholesaleXFactor;
            }
            double Qut = Details.Quantity + Details.FreeQuantity;

            if (InventoryFromDB != null)
            {
                InventoryBatch.InventoryId = InventoryFromDB.Id;
                InventoryBatch lInventoryBatch = GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, StockMovementIn.InventoryStockLocationId, Context);
                if (lInventoryBatch != null)
                {
                    //for stock
                    Qut = (Qut / (lInventoryBatch.RetailUOM == Details.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));

                    InventoryBatch.Purchased = lInventoryBatch.Purchased;
                    InventoryBatch.OpeningStock = lInventoryBatch.OpeningStock;
                    InventoryBatch.StockUOM = lInventoryBatch.StockUOM;
                    InventoryBatch.Out = lInventoryBatch.Out;
                    InventoryBatch.Damage = lInventoryBatch.Damage;
                    InventoryBatch.ToPatient = lInventoryBatch.ToPatient;
                    InventoryBatch.Sold = lInventoryBatch.Sold;
                    InventoryBatch.Adjust = lInventoryBatch.Adjust;


                    InventoryBatch.PurchasePrice = lInventoryBatch.PurchasePrice;
                    InventoryBatch.Cost = lInventoryBatch.Cost;
                    InventoryBatch.RetailSalePrice = lInventoryBatch.RetailSalePrice;
                    InventoryBatch.WholeSalePrice = lInventoryBatch.WholeSalePrice;
                    InventoryBatch.MaxRetailPrice = lInventoryBatch.MaxRetailPrice;

                    InventoryBatch.RetailUOM = lInventoryBatch.RetailUOM;
                    InventoryBatch.RetailXFactor = lInventoryBatch.RetailXFactor;
                    InventoryBatch.WholesaleUOM = lInventoryBatch.WholesaleUOM;
                    InventoryBatch.WholesaleXFactor = lInventoryBatch.WholesaleXFactor;

                    double OldQut = 0;
                    if (Details.Id != 0)
                    {
                        StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetail(Details.Id);
                        if (StockMovementDetail != null)
                        {
                            OldQut = StockMovementDetail.Quantity + StockMovementDetail.FreeQuantity;
                            OldQut = (OldQut / (lInventoryBatch.RetailUOM == StockMovementDetail.Uom ? (lInventoryBatch.WholesaleXFactor * lInventoryBatch.RetailXFactor) : lInventoryBatch.WholesaleXFactor));
                        }
                    }
                    if (OldQut > Qut)
                    {
                        InventoryBatch.In = lInventoryBatch.In - (OldQut - Qut);
                    }
                    else
                    {
                        InventoryBatch.In = lInventoryBatch.In + (Qut - OldQut);
                    }
                    InventoryBatch.Id = lInventoryBatch.Id;
                }
                else
                {
                    //for stock
                    Qut = (Qut / (InventoryBatch.RetailUOM == Details.Uom ? (InventoryBatch.WholesaleXFactor * InventoryBatch.RetailXFactor) : InventoryBatch.WholesaleXFactor));
                    InventoryBatch.In = Qut;
                }
        }
            return InventoryBatch;
        }
        public IList<InventoryBatch> GetOpeningInventoryStockDetails(long CompanyId, long ProductId)
        {
            IList<InventoryBatch> InventoryStockInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryStockInfo = Context.InventoryBatches.Where(b => b.CompanyId == CompanyId && b.ProductId == ProductId).ToList<InventoryBatch>(); 
                return InventoryStockInfo;
            }
        }
        public Inventory GetInventoryStockByProductId(long CompanyId, long InventoryId)
        {
            Inventory Inventory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Inventory = Context.Inventories.Include("InventoryBatchs").FirstOrDefault(x => x.Id == InventoryId && x.CompanyId == CompanyId);
            }
            return Inventory;
        }
        public InventoryBatch GetInventoryByBatchId(long value, object locationId)
        {
            throw new NotImplementedException();
        }
    }
}
