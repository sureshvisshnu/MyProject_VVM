using fa.context;
using fa.model.Catalog;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Microsoft.EntityFrameworkCore.Infrastructure;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore.Storage;
using FADataAccessLibrary.Model.Common;
using fa.Data;

namespace fa.api.catalog
{
    public class CatalogItemManager
    {
        private static volatile CatalogItemManager instance;
        private static object syncRoot = new Object();
        CatalogItemManager()
        {

        }
        public static CatalogItemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CatalogItemManager();
                    }
                }
                return instance;
            }
        }
        public IList<CatalogItem> ListParentCatalogItemsByCompanyId(long CompanyId, String Stxt, int SearchType)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<CatalogItem> lCategoryInfo = null;
                List<CatalogItem> CategoryInfo = null;
                if (SearchType == 1)
                {
                    CategoryInfo = lCategoryInfo = (from Items in Context.CatalogItems where Items.CompanyId == CompanyId where Items.Type != CatalogItemType.PRODUCT && Items.Type != CatalogItemType.PRODUCTFAMILY where Items.Name.Contains(Stxt) orderby Items.Name select Items).ToList();
                }
                else
                {
                    CategoryInfo = lCategoryInfo = (from Items in Context.CatalogItems where Items.CompanyId == CompanyId where Items.Type != CatalogItemType.PRODUCT where Items.Name.Contains(Stxt) orderby Items.Name select Items).ToList();
                }
                foreach (CatalogItem CatalogItem in CategoryInfo.ToList())
                {
                    lCategoryInfo = Parent(lCategoryInfo, CatalogItem);
                }
                return lCategoryInfo;
            }
        }

        public IList<CatalogItem> ListCatalogItemsByCompanyId(long CompanyId, String Stxt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<CatalogItem> lCategoryInfo = new List<CatalogItem>();
                try
                {
                    string Query = null;
                    if (Stxt == null || Stxt == string.Empty)
                    {
                        lCategoryInfo = (from cat in Context.CatalogItems where cat.CompanyId == CompanyId select cat).ToList();
                    }
                    else
                    {
                        var Filters = new MySqlParameter("@Filter", "%" + Stxt + "%");
                        var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                        Query = "SELECT C.* FROM Catalogitems C LEFT JOIN Catalogitems P ON C.Id = P.ParentId LEFT JOIN Catalogitems G ON P.Id = G.ParentId LEFT JOIN Catalogitems L ON G.Id = L.ParentId LEFT JOIN Catalogitems R ON L.Id = R.ParentId WHERE R.Id IN (SELECT Id FROM Catalogitems WHERE Id IN (SELECT Id FROM Catalogitems WHERE (Name LIKE @Filter OR MaterialId LIKE @Filter) AND CompanyId = @CompanyIds)) GROUP BY  G.ParentId UNION " +
                                "SELECT C.* FROM Catalogitems C LEFT JOIN Catalogitems P ON C.Id = P.ParentId LEFT JOIN Catalogitems G ON P.Id = G.ParentId LEFT JOIN Catalogitems L ON G.Id = L.ParentId LEFT JOIN Catalogitems R ON L.Id = R.ParentId WHERE L.Id IN (SELECT Id from Catalogitems WHERE Id IN (SELECT Id FROM Catalogitems WHERE (Name LIKE @Filter OR MaterialId LIKE @Filter) AND CompanyId = @CompanyIds)) GROUP BY  P.ParentId UNION " +
                                "SELECT C.* FROM Catalogitems C LEFT JOIN Catalogitems P ON C.Id = P.ParentId LEFT JOIN Catalogitems G ON P.Id = G.ParentId LEFT JOIN Catalogitems L ON G.Id = L.ParentId LEFT JOIN Catalogitems R ON L.Id = R.ParentId WHERE G.Id IN (SELECT Id FROM Catalogitems WHERE Id IN (SELECT Id FROM Catalogitems WHERE (Name LIKE @Filter OR MaterialId LIKE @Filter) AND CompanyId = @CompanyIds)) UNION " +
                                "SELECT C.* FROM Catalogitems C LEFT JOIN Catalogitems P ON C.Id = P.ParentId LEFT JOIN Catalogitems G ON P.Id = G.ParentId LEFT JOIN Catalogitems L ON G.Id = L.ParentId LEFT JOIN Catalogitems R ON L.Id = R.ParentId WHERE P.Id IN (SELECT Id FROM Catalogitems WHERE Id IN (SELECT Id FROM Catalogitems WHERE (Name LIKE @Filter OR MaterialId LIKE @Filter) AND CompanyId = @CompanyIds)) UNION " +
                                "SELECT C.* FROM Catalogitems C LEFT JOIN Catalogitems P ON C.Id = P.ParentId LEFT JOIN Catalogitems G ON P.Id = G.ParentId LEFT JOIN Catalogitems L ON G.Id = L.ParentId LEFT JOIN Catalogitems R ON L.Id = R.ParentId WHERE C.Id IN (SELECT Id FROM Catalogitems WHERE Id IN (SELECT Id FROM Catalogitems WHERE (Name LIKE @Filter OR MaterialId LIKE @Filter) AND CompanyId = @CompanyIds))";
                        lCategoryInfo = Context.CatalogItems.FromSqlRaw(Query, Filters, companyId).ToList();
                    }
                }
                catch (MySqlException ex)
                {
                    int errorcode = ex.Number;
                    if (ex.HResult == -2147467259)
                    {
                        Console.WriteLine("Query was stoped: " + ex.HResult);
                    }
                }
                return lCategoryInfo;
            }
        }
        private List<CatalogItem> Parent(List<CatalogItem> lCategoryInfo, CatalogItem CatalogItem)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (CatalogItem.ParentId != null)
                {
                    if (lCategoryInfo.FirstOrDefault(x => x.Id == CatalogItem.ParentId) == null)
                    {
                        CatalogItem lCatalogItem = Context.CatalogItems.Find(CatalogItem.ParentId);
                        lCategoryInfo.Add(lCatalogItem);
                        lCategoryInfo = Parent(lCategoryInfo, lCatalogItem);
                    }
                }
            }
            return lCategoryInfo;
        }
        public IList<CatalogItem> ListItemsByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CatalogItem> CategoryInfo = (from Items in Context.CatalogItems where Items.CompanyId == CompanyId where Items.Type == CatalogItemType.PRODUCT orderby Items.Name select Items).ToList();
                return CategoryInfo;
            }
        }
        public CatalogItem GetCatalogItemInfoById(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();

            }
            return CatalogItemInfo;
        }
        public CatalogItem GetParentInfoById(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent.Parent").FirstOrDefault(p => p.Id == CatalogItemId);
            }
            return CatalogItemInfo;
        }
        public List<CatalogItemSalesTaxMap> GetCatalogItemSalesTaxMapInfoById(long CatalogItemId)
        {
            List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMapInfo = Context.CatalogItemSalesTaxMaps.Include("CompanySalesTaxAccountMap").Where(p => p.CatalogItemId == CatalogItemId).ToList();
            }
            return CatalogItemSalesTaxMapInfo;
        }
        public CatalogItemSalesTaxMap GetCatalogItemSalesTaxItemMapInfoByMapId(long MapId,DateTime CurrentDate)
        {
            CatalogItemSalesTaxMap CatalogItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMapInfo = Context.CatalogItemSalesTaxMaps.Include("CompanySalesTaxAccountMap").FirstOrDefault(p => p.SalesTaxMapId == MapId && p.EffectiveFrom <= CurrentDate && p.EffectiveTo >= CurrentDate);              
            }
            return CatalogItemSalesTaxMapInfo;
        }
        public bool CheckTaxMapHaveSameFromDate(long ItemMapId,DateTime From,long CatalogId,long Id)
        {
            List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMapInfo = Context.CatalogItemSalesTaxMaps.Where(p => p.SalesTaxMapId == ItemMapId && p.Id != Id && p.CatalogItemId== CatalogId && p.EffectiveFrom<=From && p.EffectiveTo>=From).ToList();
                if (CatalogItemSalesTaxMapInfo != null && CatalogItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckTaxMapHaveSameToDate(long ItemMapId, DateTime To, long CatalogId,long Id)
        {
            List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMapInfo = Context.CatalogItemSalesTaxMaps.Where(p => p.SalesTaxMapId == ItemMapId && p.Id!=Id && p.CatalogItemId == CatalogId && p.EffectiveFrom <= To && p.EffectiveTo >= To).ToList();
                if (CatalogItemSalesTaxMapInfo != null && CatalogItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckTaxMapHaveOtherEntryAfterToDate(long MapId, DateTime To)
        {
            List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemSalesTaxMapInfo = Context.CatalogItemSalesTaxMaps.Where(p => p.SalesTaxMapId == MapId && p.EffectiveFrom > To).ToList();
                if (CatalogItemSalesTaxMapInfo != null && CatalogItemSalesTaxMapInfo.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckTaxMapHaveEntries(long MapId)
        {
            List<TaxDetail> TaxDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TaxDetail = Context.TaxDetails.Where(p => p.ItemTaxMapId == MapId).ToList();
                if (TaxDetail != null && TaxDetail.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool DeleteCatalogTax(long MapId)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    CatalogItemSalesTaxMap map = Context.CatalogItemSalesTaxMaps.Find(MapId);
                    if (map != null)
                    {
                        Context.CatalogItemSalesTaxMaps.Remove(map);
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
        public bool AddCatalogTax(List<CatalogItemSalesTaxMap> lcatalogItemSalesTaxMaps,long CatalogItemId)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapInfoFromDB = Context.CatalogItemSalesTaxMaps.Where(p => p.EffectiveFrom <= Global.TransactionDate && p.EffectiveTo >= Global.TransactionDate && p.CatalogItemId == CatalogItemId).ToList();
                        foreach (CatalogItemSalesTaxMap Map in lcatalogItemSalesTaxMaps)
                        {
                            if(Map.Id==0L)
                            {
                                Context.CatalogItemSalesTaxMaps.Add(Map);
                                Context.SaveChanges();
                            }
                            else
                            {
                                CatalogItemSalesTaxMap catalogItemSalesTaxMap = Context.CatalogItemSalesTaxMaps.Find(Map.Id);
                                if (catalogItemSalesTaxMap != null)
                                {
                                    //if (CatalogItemSalesTaxMapInfoFromDB.Count > 0)
                                    //{
                                    //    CatalogItemSalesTaxMap catalogItemSalesTax = CatalogItemSalesTaxMapInfoFromDB.FirstOrDefault(x => x.Id == catalogItemSalesTaxMap.Id);
                                    //    if (catalogItemSalesTax != null)
                                    //    {
                                    //        CatalogItemSalesTaxMapInfoFromDB.Remove(catalogItemSalesTax);
                                    //    }
                                    //}
                                    Context.Entry(catalogItemSalesTaxMap).CurrentValues.SetValues(Map);
                                    Context.SaveChanges();
                                }
                            }
                        }
                        //if (CatalogItemSalesTaxMapInfoFromDB.Count > 0)
                        //{
                        //    foreach (CatalogItemSalesTaxMap Map in CatalogItemSalesTaxMapInfoFromDB)
                        //    {
                        //        Context.CatalogItemSalesTaxMaps.Remove(Context.CatalogItemSalesTaxMaps.Find(Map.Id));
                        //        Context.SaveChanges();
                        //    }
                        //}
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Status = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Status;
        }
    }
}
