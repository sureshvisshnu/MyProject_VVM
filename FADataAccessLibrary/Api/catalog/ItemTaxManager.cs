using fa.context;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using FADataAccessLibrary.Model.Common;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace Fa.api.catalog
{
     public class ItemTaxManager
    {
        private static volatile ItemTaxManager instance;
        private static object syncRoot = new Object();
        ItemTaxManager()
        {

        }
        public static ItemTaxManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ItemTaxManager();
                    }
                }

                return instance;
            }
        }

        public IList<ItemTax> GetItemTaxs(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ItemTax> ItemTaxInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").Where(x=>x.CompanyId==CompanyId).ToList<ItemTax>();
                return ItemTaxInfo;
            }
        }
        public IList<ItemTax> GetItemTaxs(long CompanyId,string SearchCode)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ItemTax> ItemTaxInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").Where(x => x.CompanyId == CompanyId && x.Code.Contains(SearchCode)).ToList<ItemTax>();
                return ItemTaxInfo;
            }
        }

        public ItemTax GetItemTaxCodeById(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemTax ItemTaxInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").Include("SalesTaxMapLocal.CompanySalesTaxAccountMap").FirstOrDefault(x => x.Id==Id);
                return ItemTaxInfo;
            }
        }
        //public ItemTax GetItemTaxByCode(string Code)
        //{
        //    using (AccountMasterContext Context = new AccountMasterContext())
        //    {
        //        ItemTax ItemTaxInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").Include("SalesTaxMapLocal.CompanySalesTaxAccountMap").FirstOrDefault(x => x.Code == Code);
        //        return ItemTaxInfo;
        //    }
        //}

        public ItemTax GetItemTaxCodeByCode(String ItemTaxInfoCode, long CompanyId)
        {
            ItemTax TaxCodeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxCodeInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").FirstOrDefault(x => x.Code == ItemTaxInfoCode && x.CompanyId == CompanyId);
                return TaxCodeInfo;
            }
        }
        public Boolean ItemTaxCodeUniqueByDate(long MapId, DateTime To, DateTime From)
        {
            List<ItemSalesTaxMap> ItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemSalesTaxMapInfo = Context.ItemSalesTaxMaps.Where(p => p.SalesTaxMapId == MapId && p.EffectiveFromDate == From && p.EffectiveToDate == To).ToList();
                if (ItemSalesTaxMapInfo != null && ItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean ItemTaxCodeUniqueById(ItemTax ItemTax)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemTax lItemTax = null;
                if (ItemTax.Id == 0)
                {
                    lItemTax = Context.ItemTaxs.FirstOrDefault(x => x.Code == ItemTax.Code && x.CompanyId == ItemTax.CompanyId);
                }
                else
                {
                    lItemTax = Context.ItemTaxs.FirstOrDefault(x => x.Code == ItemTax.Code && x.CompanyId == ItemTax.CompanyId && !x.Id.Equals(ItemTax.Id));
                }
                if (lItemTax != null)
                {
                    Status = false;
                }
            }
            return Status;
        }

        public bool CheckTaxCodeMapHaveOtherEntryAfterToDate(long MapId, DateTime To)
        {
            List<ItemSalesTaxMap> ItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemSalesTaxMapInfo = Context.ItemSalesTaxMaps.Where(p => p.SalesTaxMapId == MapId && p.EffectiveFromDate > To).ToList();
                if (ItemSalesTaxMapInfo != null && ItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckTaxCodeMapHaveEntries(long MapId)
        {
            List<TaxDetail> TaxDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxDetail = Context.TaxDetails.Where(p => p.ItemCodeTaxMapId == MapId).ToList();
                if (TaxDetail != null && TaxDetail.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool DeleteCatalogCodeTax(long MapId)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    ItemSalesTaxMap map = Context.ItemSalesTaxMaps.Find(MapId);
                    if (map != null)
                    {
                        Context.ItemSalesTaxMaps.Remove(map);
                        Context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Status = false;
                    throw (e);
                }
            }
            return Status;
        }
        public ItemTax GetItemTaxByCode(String ItemTaxCode, long CompanyId)
        {
            ItemTax ItemTaxInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemTaxInfo = Context.ItemTaxs.FirstOrDefault(x => x.Code == ItemTaxCode && x.CompanyId == CompanyId);
                return ItemTaxInfo;
            }
        }
        public bool CheckItemTaxMapHaveSameFromDate(long SalesMapId, long ItemMapId, DateTime From, long Id)
        {
            List<ItemSalesTaxMap> ItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemSalesTaxMapInfo = Context.ItemSalesTaxMaps.Where(p => p.SalesTaxMapId == SalesMapId && p.Id != Id && p.ItemTaxId == ItemMapId  && p.EffectiveFromDate <= From && p.EffectiveToDate >= From).ToList();
                if (ItemSalesTaxMapInfo != null && ItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckItemTaxMapHaveSameToDate(long SalesMapId, long ItemMapId, DateTime To, long Id)
        {
            List<ItemSalesTaxMap> ItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemSalesTaxMapInfo = Context.ItemSalesTaxMaps.Where(p => p.SalesTaxMapId == SalesMapId && p.Id != Id && p.ItemTaxId == ItemMapId && p.EffectiveFromDate <= To && p.EffectiveToDate >= To).ToList();
                if (ItemSalesTaxMapInfo != null && ItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public IList<ItemSalesTaxMap> GetResentDateByCode(long ItemTaxIds, DateTime effectivdate)
        {

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ItemSalesTaxMap> ItemSalesTaxInfo = Context.ItemSalesTaxMaps.Where(x => x.ItemTaxId == ItemTaxIds && x.EffectiveFromDate == effectivdate).ToList<ItemSalesTaxMap>();
                return ItemSalesTaxInfo;
            }            
        }

        public ItemTax AddItemTax(ItemTax itemTax)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.ItemTaxs.Add(itemTax);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        itemTax = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return itemTax;
        }
        public ItemTax UpdateItemTax(ItemTax ItemTax)
        {
            ItemTax ItemTaxInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ItemTaxInfo = Context.ItemTaxs.Find(ItemTax.Id);
                        if (ItemTaxInfo != null)
                        {
                            foreach (ItemSalesTaxMap Detail in ItemTax.SalesTaxMapLocal)
                            {
                                if(Detail.Id==0L)
                                {
                                    Detail.ItemTaxId = ItemTax.Id;
                                    Context.ItemSalesTaxMaps.Add(Detail);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    Detail.ItemTaxId = ItemTax.Id;
                                    Context.Entry(Context.ItemSalesTaxMaps.Find(Detail.Id)).CurrentValues.SetValues(Detail);
                                    Context.SaveChanges();

                                }                               
                            }
                            Context.Entry(ItemTaxInfo).CurrentValues.SetValues(ItemTax);               
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ItemTaxInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ItemTaxInfo;
        }
        public ItemTax UpdateItemTaxFromExcel(ItemTax ItemTax)
        {
            ItemTax ItemTaxInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ItemTaxInfo = Context.ItemTaxs.Find(ItemTax.Id);
                        if (ItemTaxInfo != null)
                        {
                            foreach (ItemSalesTaxMap Detail in ItemTax.SalesTaxMapLocal)
                            {
                                ItemSalesTaxMap DetailDB = Context.ItemSalesTaxMaps.FirstOrDefault(x => x.ItemTaxId == ItemTax.Id && x.SalesTaxMapId == Detail.SalesTaxMapId && x.EffectiveFromDate.Date == Detail.EffectiveFromDate.Date && x.EffectiveToDate.Date == Detail.EffectiveToDate.Date);
                                if (DetailDB == null)
                                {
                                    Detail.ItemTaxId = ItemTax.Id;
                                    Context.ItemSalesTaxMaps.Add(Detail);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    Detail.Id = DetailDB.Id;
                                    Detail.ItemTaxId = ItemTax.Id;
                                    Context.Entry(Context.ItemSalesTaxMaps.Find(Detail.Id)).CurrentValues.SetValues(Detail);
                                    Context.SaveChanges();

                                }
                            }
                            Context.Entry(ItemTaxInfo).CurrentValues.SetValues(ItemTax);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ItemTaxInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ItemTax;
        }
        public Boolean DeleteItemTax(long ItemTaxId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ItemTax ItemTaxInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").FirstOrDefault(x => x.Id == ItemTaxId);
                        if (ItemTaxInfo.SalesTaxMapLocal.Count > 0)
                        {
                            Context.ItemSalesTaxMaps.Where(p => p.ItemTaxId == ItemTaxInfo.Id).ToList().ForEach(p => Context.ItemSalesTaxMaps.Remove(p));
                            Context.SaveChanges();
                        }
                        Context.ItemTaxs.Remove(Context.ItemTaxs.Find(ItemTaxId));
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
    }
}
