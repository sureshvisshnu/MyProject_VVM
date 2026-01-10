using fa.context;
using fa.model.Catalog;
using Microsoft.EntityFrameworkCore;

namespace fa.api.catalog
{
    public class CatalogProductFamilyManager
    {
        private static volatile CatalogProductFamilyManager instance;
        private static object syncRoot = new Object();
        CatalogProductFamilyManager()
        {

        }
        public static CatalogProductFamilyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CatalogProductFamilyManager();
                    }
                }

                return instance;
            }
        }
        public bool ProductFamilyNameUniqueById(ProductFamily ProductFamily)
        {
            bool Status = true;


            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (ProductFamily.Id == 0)
                    {
                        Context.ProductFamilies.Where(p => p.ParentId == ProductFamily.ParentId && p.Name == ProductFamily.Name && p.CompanyId == ProductFamily.CompanyId && p.Type == CatalogItemType.PRODUCTFAMILY).First<ProductFamily>();
                        Status = false;
                    }
                    else
                    {
                        Context.ProductFamilies.Where(p => p.ParentId == ProductFamily.ParentId && p.Name == ProductFamily.Name && p.CompanyId == ProductFamily.CompanyId && p.Id != ProductFamily.Id && p.Type == CatalogItemType.PRODUCTFAMILY).First<ProductFamily>();
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
        public ProductFamily GetProductFamilyInfoById(long ProductFamilyId)
        {
            ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductFamilyInfo =(from PFamily in Context.ProductFamilies.Include("Company").Include("Parent").Include("Supplier").Include("SalesAccountLocal").Include("PurchaseAccountLocal").Include("InventoryAccountLocal").Include("SalesTaxMapLocal").Include("DiscountAccountLocal") where PFamily.Id == ProductFamilyId select PFamily).FirstOrDefault();
                if (ProductFamilyInfo != null)
                {
                    if (ProductFamilyInfo.Company != null)
                    {
                        ProductFamilyInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == ProductFamilyInfo.Company.CompanyId).ToList();
                    }
                    if (ProductFamilyInfo.Products == null || ProductFamilyInfo.Products.Count==0)
                    {
                        ProductFamilyInfo.Products = Context.Products.Where(x => x.CompanyId == ProductFamilyInfo.Company.CompanyId && x.ProductFamilyId== ProductFamilyInfo.Id).ToList();
                    }
                }
            }

            return ProductFamilyInfo;
        }
        public ProductFamily GetProductFamilyInfoByIdtest(long? ProductFamilyId, long ParentId)
        {
            ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductFamilyInfo = (from PFamily in Context.ProductFamilies.Include("Company").Include("Parent").Include("Supplier").Include("SalesAccountLocal").Include("PurchaseAccountLocal").Include("InventoryAccountLocal").Include("SalesTaxMapLocal").Include("DiscountAccountLocal") where PFamily.Id == ProductFamilyId select PFamily).FirstOrDefault();
                if (ProductFamilyInfo != null)
                {
                    if (ProductFamilyInfo.Company != null)
                    {
                        ProductFamilyInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == ProductFamilyInfo.Company.CompanyId).ToList();
                    }
                    if (ProductFamilyInfo.Products == null || ProductFamilyInfo.Products.Count == 0)
                    {
                        ProductFamilyInfo.Products = Context.Products.Where(x => x.CompanyId == ProductFamilyInfo.Company.CompanyId && x.ProductFamilyId == ProductFamilyInfo.Id).ToList();
                    }
                }
            }

            return ProductFamilyInfo;
        }
        public ProductFamily GetProductFamilyInfoById(long ProductFamilyId, long CompanyId)
        {
            ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductFamilyInfo = (from PFamily in Context.ProductFamilies.Include("Company").Include("Parent") where PFamily.CompanyId == CompanyId && PFamily.Id == ProductFamilyId select PFamily).FirstOrDefault();
            }
            return ProductFamilyInfo;
        }

        public bool IsContainProductfamily(long CompanyId)
        {
            ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductFamilyInfo = Context.ProductFamilies.Where(x=>x.CompanyId== CompanyId).OrderByDescending(p => p.Id).FirstOrDefault();
            }
            return ProductFamilyInfo != null ? true : false;
        }
        public IDictionary<string, List<long>> GetProductFamilyIdsByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var productFamilies = (from ProductFamily in Context.ProductFamilies
                                       where ProductFamily.CompanyId == CompanyId && ProductFamily.Type == CatalogItemType.PRODUCTFAMILY
                                       select ProductFamily).ToList();

                var productFamilyIdsByName = productFamilies
                    .Where(pf => pf.CompanyId == CompanyId && pf.Type == CatalogItemType.PRODUCTFAMILY)
                    .GroupBy(pf => pf.Name)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(pf => pf.Id).ToList()
                    );

                return productFamilyIdsByName;
            }
        }
        public ProductFamily GetProductFamilyByProductId(long productId, long companyId)
        {
            using var context = new AccountMasterContext();

            return context.Products
                .Where(p => p.Id == productId && p.CompanyId == companyId)
                .Select(p => p.ProductFamily)
                .Include(f => f.Parent)
                .FirstOrDefault();
        }

        public IList<ProductFamily> ListProductFamilyByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ProductFamily> ProductFamilyInfo = (from ProductFamily in Context.ProductFamilies where ProductFamily.CompanyId == CompanyId && ProductFamily.Type==CatalogItemType.PRODUCTFAMILY select ProductFamily).ToList();
                //IList<ProductFamily> distinctProductFamilies = ProductFamilyInfo
                //                    .Where(pf => pf.CompanyId == CompanyId && pf.Type == CatalogItemType.PRODUCTFAMILY)
                //                    .GroupBy(pf => pf.Name)
                //                    .Select(group => group.First())
                //                    .ToList();
                return ProductFamilyInfo;
            }
        }
        public IList<CatalogItem> ListProductCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CatalogItem> ProductCategoryInfo = (from CatalogItem in Context.CatalogItems where CatalogItem.CompanyId == CompanyId && CatalogItem.Type == CatalogItemType.CATEGORY select CatalogItem).ToList();
                return ProductCategoryInfo;
            }
        }
        public IList<ProductFamily> ListProductFamilyByCategoryId(long CategoryId, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<ProductFamily> ProductFamilyInfo = (from ProductFamily in Context.ProductFamilies where ProductFamily.CompanyId == CompanyId && ProductFamily.ParentId == CategoryId && ProductFamily.Type == CatalogItemType.PRODUCTFAMILY select ProductFamily).ToList();
                return ProductFamilyInfo;
            }
        }

        public ProductFamily AddProductFamily(ProductFamily productFamily)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        productFamily.Parent = null;
                        Context.ProductFamilies.Add(productFamily);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        productFamily = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return productFamily;
        }
        public ProductFamily UpdateProductFamily(ProductFamily ProductFamily)
        {
            ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ProductFamilyInfo = Context.ProductFamilies.Find(ProductFamily.Id);
                        if (ProductFamilyInfo != null)
                        {
                            ProductFamily ProductFamilyInfoFromDB = GetProductFamilyInfoById(ProductFamily.Id);                          
                            ProductFamily.Parent = null;
                            Context.Entry(ProductFamilyInfo).CurrentValues.SetValues(ProductFamily);
                            if(ProductFamily.SalesTaxMapLocal !=null)
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ProductFamilyInfo = null;
                        dbContextTransaction.Rollback();
                    //    throw (e);
                    }
                }
            }
            return ProductFamilyInfo;
        }
        public Boolean DeleteProductFamily(long ProductFamilyId)
        {
            Boolean Deleted = false;
            //ProductFamily ProductFamilyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //IList<CatalogItem> CatalogItems =CategoryManager.Instance.GetAllChildOfSelected(ProductFamilyId).Reverse().ToList();
                        //foreach (CatalogItem cat in CatalogItems)
                        //{
                        //    ProductFamily ProductFamilyInfo = Context.ProductFamilies.Find(cat.Id);
                        //    Context.Categories.Remove(ProductFamilyInfo);
                        //    Context.SaveChanges();
                        //}

                        //ProductFamilyInfo = Context.ProductFamilies.Find(ProductFamilyId);
                        //Context.ProductFamilies.Remove(ProductFamilyInfo);
                        //Context.SaveChanges();
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
