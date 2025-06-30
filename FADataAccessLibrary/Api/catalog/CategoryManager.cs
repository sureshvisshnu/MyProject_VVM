using fa.model.Catalog;
using fa.context;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore;
using fa.model.Accounting.Masters;
using fa.model.Hms.Master;

namespace fa.api.catalog
{
    public class CategoryManager
    {
        private static volatile CategoryManager instance;
        private static object syncRoot = new Object();
        CategoryManager()
        {

        }
        public static CategoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CategoryManager();
                    }
                }
                return instance;
            }
        }

        public bool IsContainCategory(long CompanyId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Where(p => p.Type == CatalogItemType.CATEGORY && p.CompanyId==CompanyId).FirstOrDefault();
            }
            return CatalogItemInfo != null ? true : false;
        }

        public CatalogItem TaxMap(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Company.SalesTaxAccountMaps").Include("Parent").Include("SalesTaxMapLocal").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();
            }
            return CatalogItemInfo;
        }
        public CatalogItem SAc(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent").Include("SalesAccountLocal").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();
            }
            return CatalogItemInfo;
        }
        public CatalogItem PAc(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent").Include("PurchaseAccountLocal").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();
            }
            return CatalogItemInfo;
        }
        public CatalogItem DAc(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent").Include("DiscountAccountLocal").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();
            }
            return CatalogItemInfo;
        }
        public CatalogItem IAc(long CatalogItemId)
        {
            CatalogItem CatalogItemInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemInfo = Context.CatalogItems.Include("Parent").Include("InventoryAccountLocal").Where(p => p.Id == CatalogItemId).FirstOrDefault<CatalogItem>();
            }
            return CatalogItemInfo;
        }

        public Boolean CategoryNameUniqueById(Category Category)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (Category.Id == 0)
                    {
                        Context.Categories.Where(p => p.ParentId == Category.ParentId && p.Name == Category.Name && p.CompanyId == Category.CompanyId && p.Type==CatalogItemType.CATEGORY).First<Category>();
                        Status = false;
                    }
                    else
                    {
                        Context.Categories.Where(p => p.ParentId == Category.ParentId && p.Name == Category.Name && p.CompanyId == Category.CompanyId && p.Id != Category.Id && p.Type == CatalogItemType.CATEGORY).First<Category>();
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

        public Category GetCategoryInfoById(long CategoryId)
        {
            Category CategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
               CategoryInfo = (from Cat in Context.Categories.Include("Company").Include("Parent").Include("Supplier").Include("SalesAccountLocal").Include("PurchaseAccountLocal").Include("InventoryAccountLocal").Include("SalesTaxMapLocal").Include("DiscountAccountLocal") where Cat.Id == CategoryId select Cat).FirstOrDefault();

                if(CategoryInfo.Company!=null)
                {
                    CategoryInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == CategoryInfo.Company.CompanyId).ToList();
                }
            }
            return CategoryInfo;
        }
        public IList<Category> ListAllParentCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Category> CategoryInfo = Context.Categories.Where(x => x.CompanyId == CompanyId).ToList();
                return CategoryInfo;
            }
        }
        public IList<Category> GetAllParentCategoryByCompanyId(long CompanyId)
        {
            IList<Category> CategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CategoryInfo = (from Category in Context.Categories where Category.CompanyId == CompanyId where Category.ParentId.Equals(null)  select Category).ToList();

            }
            return CategoryInfo;
        }
        public IList<Category> ListParentCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Category> CategoryInfo = (from Category in Context.Categories where Category.CompanyId == CompanyId where Category.ParentId.Equals(null) select Category).ToList();
                return CategoryInfo;
            }
        }       
        public IList<Category> ListCategoryByCompanyId(long CompanyId)
        {
            IList<Category> CategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {               
                CategoryInfo = (from Category in Context.Categories.Include("Company") where Category.CompanyId == CompanyId && Category.Type == CatalogItemType.CATEGORY select Category).ToList<Category>();
            }
            return CategoryInfo;
        }
        public Category AddCategory(Category category)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        category.Parent = null;
                        Context.Categories.Add(category);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        category = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return category;
        }
        public Category UpdateCategory(Category Category)
        {
            Category CategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        CategoryInfo = Context.Categories.FirstOrDefault(x=>x.Id==Category.Id);
                        if (CategoryInfo != null)
                        {
                            Category CategoryInfoFromDB = GetCategoryInfoById(Category.Id);                           
                            Category.Parent = null;
                            Context.Entry(CategoryInfo).CurrentValues.SetValues(Category);                                                   
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CategoryInfo = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return CategoryInfo;
        }

        public IList<CatalogItem> GetAllChildOfSelected(long CategoryId, IList<CatalogItem> CatalogItems)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItem lCatalogItems = (from CatalogItem in Context.CatalogItems where CatalogItem.Id== CategoryId select CatalogItem).FirstOrDefault();
                if(lCatalogItems!=null)
                {
                    CatalogItems.Add(lCatalogItems);
                    IList<CatalogItem> llCatalogItems = (from CatalogItem in Context.CatalogItems where CatalogItem.ParentId == CategoryId select CatalogItem).ToList();
                    if (llCatalogItems != null && llCatalogItems.Count>0)
                    {
                        foreach(CatalogItem CatalogItem in llCatalogItems)
                        {
                            GetAllChildOfSelected(CatalogItem.Id, CatalogItems);
                        }
                    }
                }

            }
            return CatalogItems;
        }
        public Boolean DeleteCategory(long CategoryId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        IList<CatalogItem> lCatalogItems = new List<CatalogItem>();
                        IList<CatalogItem> CatalogItems = GetAllChildOfSelected(CategoryId, lCatalogItems).ToList();
                        foreach (CatalogItem cat in CatalogItems.OrderBy(x => x.Type == CatalogItemType.CATEGORY && x.ParentId==null).ThenBy(x => x.Type == CatalogItemType.CATEGORY).ThenBy(x => x.Type == CatalogItemType.PRODUCTFAMILY).ThenBy(x => x.Type == CatalogItemType.PRODUCT))
                        {
                            CatalogItem CategoryInfo = Context.CatalogItems.Find(cat.Id);
                            if (CategoryInfo.Type == CatalogItemType.PRODUCT)
                            {
                                Inventory Inventory = Context.Inventories.FirstOrDefault(x=>x.ProductId==CategoryInfo.Id);
                                if (Inventory != null)
                                {
                                    if (CategoryInfo.isInventoryAtBatch != null && (bool)CategoryInfo.isInventoryAtBatch)
                                    {
                                        Context.InventoryBatches.Where(p => p.InventoryId == Inventory.Id).ToList().ForEach(p => Context.InventoryBatches.Remove(p));
                                        Context.SaveChanges();
                                    }
                                    Context.Inventories.Remove(Inventory);
                                }                    
                            }
                            Context.CatalogItemSalesTaxMaps.Where(p => p.CatalogItemId == cat.Id).ToList().ForEach(p => Context.CatalogItemSalesTaxMaps.Remove(p));
                            Context.SaveChanges();

                            Context.CatalogItems.Remove(CategoryInfo);
                            Context.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return Deleted;
        }
    }
}
