using fa.api.OrderManagement;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.OrderManagement;
using FaData.Utils;
//using FADataAccessLibrary.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static NPOI.HSSF.Util.HSSFColor;
namespace fa.api.catalog
{
    public class CatalogProductManager
    {
        private static volatile CatalogProductManager instance;
        private static object syncRoot = new Object();
        CatalogProductManager()
        {

        }
        public static CatalogProductManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CatalogProductManager();
                    }
                }

                return instance;
            }
        }
        public bool FindMaterialIdUnique(Product Product)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (Product.Id == 0)
                    {
                        Context.Products.Where(p => p.CompanyId == Product.CompanyId && p.MaterialId == Product.MaterialId && p.Type == CatalogItemType.PRODUCT).First<Product>();
                        Status = false;
                    }
                    else
                    {
                        Context.Products.Where(p => p.CompanyId == Product.CompanyId && p.MaterialId == Product.MaterialId && p.Id != Product.Id && p.Type == CatalogItemType.PRODUCT).First<Product>();
                        Status = false;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return Status;
        }
        public bool ProductNameUniqueById(Product Product)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (Product.Id == 0)
                    {
                        Product lProduct = Context.Products.Where(p => p.ParentId == Product.ParentId && p.Name == Product.Name && p.CompanyId == Product.CompanyId && p.Type == CatalogItemType.PRODUCT).First<Product>();
                        if (lProduct != null)
                        {
                            Status = false;
                        }
                    }
                    else
                    {
                        Product lProduct = Context.Products.Where(p => p.ParentId == Product.ParentId && p.Name == Product.Name && p.CompanyId == Product.CompanyId && p.Id != Product.Id && p.Type == CatalogItemType.PRODUCT).First<Product>();
                        if (lProduct != null)
                        {
                            Status = false;
                        }
                    }
                }
#pragma warning disable 0168
                catch (Exception ex)
                {
                }
#pragma warning restore 0168
            }
            return Status;
        }

        public Product GetProductByName(string Product, long CompanyId)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.Products.Where(p => p.Name == Product && p.CompanyId == CompanyId).FirstOrDefault<Product>();
                return ProductInfo;
            }
        }
        public bool ProductNameUniqueById(Product Product, AccountMasterContext Context)
        {
            bool Status = true;
            try
            {
                if (Product.Id == 0)
                {
                    CatalogItem lProduct = Context.CatalogItems.Where(p => p.CompanyId == Product.CompanyId && p.Type == CatalogItemType.PRODUCT && p.Name == Product.Name && p.ParentId == Product.ParentId).First<CatalogItem>();
                    if (lProduct != null)
                    {
                        Status = false;
                    }
                }
                else
                {
                    CatalogItem lProduct = Context.CatalogItems.Where(p => p.CompanyId == Product.CompanyId && p.Type == CatalogItemType.PRODUCT && p.Name == Product.Name && p.ParentId == Product.ParentId && p.Id != Product.Id).First<CatalogItem>();
                    if (lProduct != null)
                    {
                        Status = false;
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception ex)
            {
            }
#pragma warning restore 0168
            return Status;
        }
        public Product GetProductInfoByIdForProductLoad(long ProductId)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.Products.Include("Company.SalesTaxAccountMaps").Include("Parent").Include("Supplier").Include("SalesTaxMapLocal").Where(p => p.Id == ProductId).FirstOrDefault<Product>();
                if (ProductInfo != null)
                {
                    if (ProductInfo.Company != null)
                    {
                        ProductInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == ProductInfo.Company.CompanyId).ToList();
                    }
                }
            }
            return ProductInfo;
        }
        public Product? GetProductInfoByMaterialId(string materialId, long CompanyId)
        {
            return ListProductByCompanyId(CompanyId).FirstOrDefault(p => p.MaterialId.Equals(materialId, StringComparison.OrdinalIgnoreCase));
        }
        public Product GetProductInfoBySaleDetailId(long ProductId)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = (Product)Context.Products.Where(p => p.Id == ProductId).FirstOrDefault<Product>();
            }
            return ProductInfo;
        }
        public Product GetProductInfoById(long ProductId)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.Products.Include("Company").Include("Inventorys").Include("ProductFamily").Include("Parent").Include("Supplier").Include("SalesAccountLocal").Include("PurchaseAccountLocal").Include("InventoryAccountLocal").Include("DiscountAccountLocal").Include("SalesTaxMapLocal").Where(p => p.Id == ProductId).FirstOrDefault<Product>();

                if (ProductInfo != null)
                {
                    if (ProductInfo.Company != null)
                    {
                        ProductInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == ProductInfo.Company.CompanyId).ToList();
                    }
                }
            }
            return ProductInfo;
        }
        public IList<CatalogItem> ListAllManufacture(long CompanyId)
        {
            IList<CatalogItem> CatalogItemManufacturerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemManufacturerInfo = (from CatalogItem in Context.CatalogItems.Include("Company") where CatalogItem.CompanyId == CompanyId && CatalogItem.Manufacturer  != "" && CatalogItem.Manufacturer != null select CatalogItem).Distinct().ToList<CatalogItem>();
            }
            return CatalogItemManufacturerInfo;
        }
        public IList<CatalogItem> ListAllSupplier(long CompanyId)
        {
            IList<CatalogItem> CatalogItemManufacturerInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CatalogItemManufacturerInfo = (from CatalogItem in Context.CatalogItems.Include("Company") where CatalogItem.CompanyId == CompanyId && CatalogItem.SupplierName != "" && CatalogItem.SupplierName != null select CatalogItem).Distinct().ToList<CatalogItem>();
            }
            return CatalogItemManufacturerInfo;
        }
        public List<string> GetAllUniqueProductManufacture(long CompanyId)
        {
            List<string> PurchaseUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseUOM = Context.CatalogItems.Where(c => c.Manufacturer != "" && c.Manufacturer != null && c.CompanyId == CompanyId).Select(p => p.Manufacturer).Distinct().ToList();
            }
            return PurchaseUOM;
        }
        public IList<Product> ListAllRackNumber(long CompanyId)
        {
            IList<Product> ProductRackInfo = null;

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductRackInfo = (from Product in Context.Products.Include("Company") where Product.CompanyId == CompanyId && Product.RackNumber != null select Product).Distinct().ToList<Product>();
            }
            return ProductRackInfo;
        }
        public List<string> GetAllUniqueRackNumber(long CompanyId)
        {
            List<string> RackNumber = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RackNumber = Context.Products.Where(c => c.CompanyId == CompanyId && c.RackNumber != null).Select(p => p.RackNumber).Distinct().ToList();
            }
            return RackNumber;
        }
        public List<string> GetAllUniquePurchaseUOM(long CompanyId)
        {
            List<string> PurchaseUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PurchaseUOM = Context.Products.Where(c => c.CompanyId == CompanyId).Select(p => p.UOM).Distinct().ToList();
            }
            return PurchaseUOM;
        }
        public List<string> GetAllUniqueRetailUOM(long CompanyId)
        {
            List<string> RetailUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                RetailUOM = Context.Products.Where(c => c.CompanyId == CompanyId && c.RetailUOM != null).Select(p => p.RetailUOM).Distinct().ToList();
            }
            return RetailUOM;
        }
        public List<string> GetAllUniqueWholeSaleUOM(long CompanyId)
        {
            List<string> WholesaleUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                WholesaleUOM = Context.Products.Where(c => c.CompanyId == CompanyId && c.WholesaleUOM != null).Select(p => p.WholesaleUOM).Distinct().ToList();
            }
            return WholesaleUOM;
        }
        public Product AddProduct(Product product)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        product.Parent = null;
                        Context.Products.Add(product);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        product = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return product;
        }
        public Product UpdateProductWithContext(Product Product, AccountMasterContext Context)
        {
            Product ProductInfo = null;
            try
            {
                ProductInfo = Context.Products.Find(Product.Id);
                if (ProductInfo != null)
                {
                    Product ProductInfoFromDB = GetProductInfoById(Product.Id);
                    ProductInfoFromDB.Company = null;
                    ProductInfoFromDB.InventoryAccount = null;
                    ProductInfoFromDB.PurchaseAccount = null;
                    ProductInfoFromDB.SalesAccount = null;
                    ProductInfoFromDB.InventoryAccountLocal = null;
                    ProductInfoFromDB.PurchaseAccountLocal = null;
                    ProductInfoFromDB.SalesAccountLocal = null;
                    ProductInfoFromDB.Parent = null;
                    ProductInfoFromDB.ProductFamily = null;
                    if (ProductInfoFromDB.SalesTaxMapLocal.Count > 0)
                    {
                        //Context.CatalogItemSalesTaxMaps.Where(p => p.CatalogItemId == ProductInfoFromDB.Id).ToList().ForEach(p => Context.CatalogItemSalesTaxMaps.Remove(p));
                        //Context.SaveChanges();
                    }
                    Product.Parent = null;
                    Product.Company = null;
                    Context.Entry(ProductInfo).CurrentValues.SetValues(Product);
                    if (Product.SalesTaxMapLocal != null)
                        foreach (CatalogItemSalesTaxMap Map in Product.SalesTaxMapLocal)
                        {
                            Map.CatalogItemId = Product.Id;
                            if (Map.CompanySalesTaxAccountMap != null)
                            {
                                Map.CompanySalesTaxAccountMap.Company = null;
                            }
                            CatalogItemSalesTaxMap lCatalogItemSalesTaxMap = Context.CatalogItemSalesTaxMaps.Find(Map.Id);
                            Context.Entry(lCatalogItemSalesTaxMap).CurrentValues.SetValues(Map);
                            Context.SaveChanges();
                        }
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ProductInfo = null;
                throw (e);
            }
            return ProductInfo;
        }
        public Product UpdateProduct(Product Product)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ProductInfo = Context.Products.Find(Product.Id);
                        if (ProductInfo != null)
                        {
                            Product.Parent = null;
                            Context.Entry(ProductInfo).CurrentValues.SetValues(Product);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ProductInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ProductInfo;
        }
        public Product UpdateProductFromExcel(Product Product, AccountMasterContext Context)
        {
            Product ProductInfo = null;
            ProductInfo = Context.Products.Find(Product.Id);
            if (ProductInfo != null)
            {
                if (Product.SalesTaxMapLocal != null && Product.SalesTaxMapLocal.Count > 0)
                {
                    foreach (CatalogItemSalesTaxMap Map in Product.SalesTaxMapLocal)
                    {
                        CatalogItemSalesTaxMap MapDB = Context.CatalogItemSalesTaxMaps.FirstOrDefault(x => x.CatalogItemId == Product.Id && x.SalesTaxMapId == Map.SalesTaxMapId && (x.EffectiveFrom == Map.EffectiveFrom ) && (x.EffectiveTo == Map.EffectiveTo));
                        if (MapDB != null)
                        {
                            Map.Id = MapDB.Id;
                            Map.CatalogItemId = Product.Id;
                            Context.Entry(Context.CatalogItemSalesTaxMaps.Find(Map.Id)).CurrentValues.SetValues(Map);
                            Context.SaveChanges();
                        }
                        else
                        {
                            if (!CatalogItemManager.Instance.CheckTaxMapHaveSameFromDate((Map.SalesTaxMapId!=null?(long)Map.SalesTaxMapId:0L), Map.EffectiveFrom, Product.Id, 0L) && !CatalogItemManager.Instance.CheckTaxMapHaveSameToDate((Map.SalesTaxMapId != null ? (long)Map.SalesTaxMapId : 0L), Map.EffectiveTo, Product.Id, 0L))
                            {
                                Map.CatalogItemId = Product.Id;
                                Context.CatalogItemSalesTaxMaps.Add(Map);
                                Context.SaveChanges();
                            }
                        }
                    }
                }
                Product.Parent = null;
                if (Context.StockMovementDetail.Where(x => x.ProductId == Product.Id).ToList().Count > 0)
                {
                    Product._isInventoryAtBatch = ProductInfo._isInventoryAtBatch;
                    Product._shouldMaintainInventory = ProductInfo._shouldMaintainInventory;
                    Product.UOM = ProductInfo.UOM;
                    Product.RetailUOM = ProductInfo.RetailUOM;
                    Product.WholesaleUOM = ProductInfo.WholesaleUOM;
                }
                Context.Entry(ProductInfo).CurrentValues.SetValues(Product);
                Context.SaveChanges();
            }
            return ProductInfo;
        }
        public Product UpdateProduct(Product Product, InventoryBatch InventoryBatch)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                                ProductInfo = Context.Products.Find(Product.Id);
                        if (ProductInfo != null)
                        {
                            Product ProductInfoFromDB = GetProductInfoById(Product.Id);
                            Product.Parent = null;
                            Context.Entry(ProductInfo).CurrentValues.SetValues(Product);
                            Context.SaveChanges();
                            if (InventoryBatch != null)
                            {
                                InventoryBatch lInventoryBatch = Context.InventoryBatches.Find(InventoryBatch.Id);
                                if (lInventoryBatch != null)
                                {
                                    Context.Entry(lInventoryBatch).CurrentValues.SetValues(InventoryBatch);
                                    Context.SaveChanges();
                                }
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ProductInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ProductInfo;
        }
        public IList<Product> ListProductByCompanyId(long LocationId, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Product> ProductInfo = null;
                if (LocationId != 0L)
                {
                    long[] ProductIds = Context.Inventories.Where(x => x.InventoryLocationId == LocationId).Select(x => x.ProductId).ToArray();
                    ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT && ProductIds.Contains(Product.Id) select Product).ToList();
                }
                else
                {
                    ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT select Product).ToList();
                }
                return ProductInfo;
            }
        }
        public IList<Product> ListAllProductByCompanyIdAndCategoryType(long LocationId, long CompanyId, long[] categoryIds, AccountMasterContext Context)
        {
            IList<Product> ProductInfo = null;

            if (LocationId != 0L)
            {
                long[] ProductIds = Context.Inventories
                    .Where(x => x.InventoryLocationId == LocationId)
                    .Select(x => x.ProductId)
                    .ToArray();

                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               join category in Context.Categories on (int)productFamily.Type equals category.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && ProductIds.Contains(product.Id)
                                     && categoryIds.Contains(category.Id)
                               select product).Include(p => p.Supplier).ToList();
            }
            else
            {
                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               join category in Context.Categories on (int)productFamily.Type equals category.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && categoryIds.Contains(category.Id)
                               select product).Include(p => p.Supplier).ToList();
            }

            return ProductInfo;
        }
        //public IList<Product> ListProductByCompanyIdAndCategoryType(long LocationId, long CompanyId, long[] preferredCategoryIds, AccountMasterContext Context)
        //{
        //    IList<Product> ProductInfo = null;

        //    if (LocationId != 0L)
        //    {
        //        long[] ProductIds = Context.Inventories
        //            .Where(x => x.InventoryLocationId == LocationId)
        //            .Select(x => x.ProductId)
        //            .ToArray();

        //        ProductInfo = (from product in Context.Products
        //                       join productFamily in Context.ProductFamilies.Include(pf => pf.Parent) on product.ProductFamilyId equals productFamily.Id
        //                       where product.CompanyId == CompanyId
        //                             && product.Type == CatalogItemType.PRODUCT
        //                             && ProductIds.Contains(product.Id)
        //                             && preferredCategoryIds.Contains((long)productFamily.ParentId)
        //                       select product).Include(p => p.Supplier).ToList();
        //    }
        //    else
        //    {
        //        ProductInfo = (from product in Context.Products
        //                       join productFamily in Context.ProductFamilies.Include(pf => pf.Parent) on product.ProductFamilyId equals productFamily.Id
        //                       where product.CompanyId == CompanyId
        //                             && product.Type == CatalogItemType.PRODUCT
        //                             && preferredCategoryIds.Contains((long)productFamily.ParentId)
        //                       select product).Include(p => p.Supplier).ToList();
        //    }

        //    return ProductInfo;
        //}

        public IList<Product> ListProductByCompanyIdAndCategoryType(long LocationId, long CompanyId, long[] preferredCategoryIds, AccountMasterContext Context)
        {
            IList<Product> ProductInfo = null;

            if (LocationId != 0L)
            {
                long[] ProductIds = Context.Inventories
                    .Where(x => x.InventoryLocationId == LocationId)
                    .Select(x => x.ProductId)
                    .ToArray();

                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && ProductIds.Contains(product.Id)
                                     && preferredCategoryIds.Contains((long)productFamily.ParentId)
                               select product).Include(p => p.Supplier).ToList();
            }
            else
            {
                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && preferredCategoryIds.Contains((long)productFamily.ParentId)
                               select product).Include(p => p.Supplier).ToList();
            }

            return ProductInfo;
        }
        public IList<Product> ListProductByCompanyIdAndProductFamilyTypeName(long LocationId, long CompanyId, string[] preferredProductFamilyNames, AccountMasterContext Context)
        {
            IList<Product> ProductInfo = null;

            if (LocationId != 0L)
            {
                long[] ProductIds = Context.Inventories
                    .Where(x => x.InventoryLocationId == LocationId)
                    .Select(x => x.ProductId)
                    .ToArray();

                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && ProductIds.Contains(product.Id)
                                     && preferredProductFamilyNames.Contains(productFamily.Name)
                               select product).Include(p => p.Supplier).ToList();
            }
            else
            {
                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && preferredProductFamilyNames.Contains(productFamily.Name)
                               select product).Include(p => p.Supplier).ToList();
            }

            return ProductInfo;
        }

        public IList<Product> ListProductByCompanyIdAndProductFamilyType(long LocationId, long CompanyId, long[] preferredProductFamilyIds, AccountMasterContext Context)
        {
            IList<Product> ProductInfo = null;

            if (LocationId != 0L)
            {
                long[] ProductIds = Context.Inventories
                    .Where(x => x.InventoryLocationId == LocationId)
                    .Select(x => x.ProductId)
                    .ToArray();

                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && ProductIds.Contains(product.Id)
                                     && preferredProductFamilyIds.Contains((long)productFamily.Id)
                               select product).Include(p => p.Supplier).ToList();
            }
            else
            {
                ProductInfo = (from product in Context.Products
                               join productFamily in Context.ProductFamilies on product.ProductFamilyId equals productFamily.Id
                               where product.CompanyId == CompanyId
                                     && product.Type == CatalogItemType.PRODUCT
                                     && preferredProductFamilyIds.Contains((long)productFamily.Id)
                               select product).Include(p => p.Supplier).ToList();
            }

            return ProductInfo;
        }




        public IList<Product> ListProductByCompanyId(long LocationId, long CompanyId, AccountMasterContext Context)
        {
            IList<Product> ProductInfo = null;
            if (LocationId != 0L)
            {
                long[] ProductIds = Context.Inventories.Where(x => x.InventoryLocationId == LocationId).Select(x => x.ProductId).ToArray();
                ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT && ProductIds.Contains(Product.Id) select Product).Include(p => p.Supplier).ToList();
            }
            else
            {
                ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT select Product).ToList();
            }
            return ProductInfo;
        }
        public IList<Product> ListBatchExpiryProductByCompanyId(long LocationId, long CompanyId, DateTime fromDate, DateTime toDate)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Product> ProductInfo = null;
                if (LocationId != 0L)
                {
                    long[] ProductIds = Context.Inventories
                        .Where(x => x.InventoryLocationId == LocationId)
                        .Select(x => x.ProductId)
                        .ToArray();

                    ProductInfo = (from Product in Context.Products
                                   where Product.CompanyId == CompanyId
                                         && Product.Type == CatalogItemType.PRODUCT
                                         && Product._isInventoryAtBatch != null
                                         && (bool)Product._isInventoryAtBatch == true
                                         && ProductIds.Contains(Product.Id)
                                         && Context.InventoryBatches
                                            .Any(b => b.ProductId == Product.Id && b.ExpDate >= fromDate && b.ExpDate <= toDate)
                                   select Product).ToList();
                }
                else
                {
                    ProductInfo = (from Product in Context.Products
                                   where Product.CompanyId == CompanyId
                                         && Product.Type == CatalogItemType.PRODUCT
                                         && (bool)Product.isInventoryAtBatch == true
                                         && Context.InventoryBatches
                                            .Any(b => b.ProductId == Product.Id && b.ExpDate >= fromDate && b.ExpDate <= toDate)
                                   select Product).ToList();
                }
                return ProductInfo;
            }
        }


        public IList<Product> ListBatchProductByCompanyId(long LocationId, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Product> ProductInfo = null;
                if (LocationId != 0L)
                {
                    long[] ProductIds = Context.Inventories.Where(x => x.InventoryLocationId == LocationId).Select(x => x.ProductId).ToArray();
                    ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT && Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch == true && ProductIds.Contains(Product.Id) select Product).ToList();
                }
                else
                {
                    ProductInfo = (from Product in Context.Products where Product.CompanyId == CompanyId && Product.Type == CatalogItemType.PRODUCT && (bool)Product.isInventoryAtBatch == true select Product).ToList();
                }
                return ProductInfo;
            }
        }
        public List<CatalogItem> ListProductByCompanyIdName(long CompanyId, string FilterString)
        {
            List<CatalogItem> lProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lProductInfo = (from lProduct in Context.CatalogItems where lProduct.CompanyId == CompanyId && lProduct.Type == CatalogItemType.PRODUCTFAMILY && lProduct.Name == FilterString select lProduct).ToList();
            }
            return lProductInfo;
        }
        public List<Product> ListProductByCompanyId(long CompanyId)
        {
            List<Product> lProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lProductInfo = (from lProduct in Context.Products where lProduct.CompanyId == CompanyId && lProduct.Type == CatalogItemType.PRODUCT select lProduct).ToList();
            }
            return lProductInfo;
        }
        public List<Product> ListProductWithDetailsByCompanyId(long CompanyId)
        {
            List<Product> lProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lProductInfo = (from lProduct in Context.Products.Include("ProductFamily.Parent").Include("ProductFamily").Include("Supplier").Include("SalesAccountLocal").Include("PurchaseAccountLocal").Include("InventoryAccountLocal").Include("SalesTaxMapLocal") where lProduct.CompanyId == CompanyId && lProduct.Type == CatalogItemType.PRODUCT select lProduct).ToList();
            }
            return lProductInfo;
        }
        public List<Product> GetProductsBySearchQuery(string FilterString, long CompanyId)
        {
            List<Product> lProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lProductInfo = (from lProduct in Context.Products where lProduct.CompanyId == CompanyId && lProduct.Type == CatalogItemType.PRODUCT && (lProduct.MaterialId.Contains(FilterString) || lProduct.Name.Contains(FilterString)) select lProduct).ToList();

            }
            return lProductInfo;
        }
        public IList<Product> GetProductByExactSearchQuery(string FilterString, long CompanyId)
        {
            IList<Product> ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.Products.Where(x => (x.MaterialId == FilterString || x.Name == FilterString/* || x.UOM==FilterString*/) && x.CompanyId == CompanyId).ToList<Product>();
                return ProductInfo;
            }
        }
        public Product GetProductByProductCode(string PCode, long CompanyId)
        {
            Product ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.Products.FirstOrDefault(x => x.CompanyId == CompanyId && x.MaterialId == PCode);
                if (ProductInfo != null)
                {
                    if (ProductInfo.Company != null)
                    {
                        ProductInfo.Company.SalesTaxAccountMaps = Context.CompanySalesTaxAccountMaps.Where(x => x.CompanyId == ProductInfo.Company.CompanyId).ToList();
                    }
                }
                return ProductInfo;
            }
        }
        public bool HSNCodeUsedInProduct(string HSNCode, long CompanyId)
        {
            List<Product> ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ItemTax TaxCodeInfo = Context.ItemTaxs.Include("SalesTaxMapLocal").FirstOrDefault(x => x.Code == HSNCode && x.CompanyId == CompanyId);
                if (TaxCodeInfo != null)
                {
                    List<TaxDetail> lTaxDetail = Context.TaxDetails.Where(X => TaxCodeInfo.SalesTaxMapLocal.Select(x => x.Id).ToList().Contains((long)X.ItemCodeTaxMapId)).ToList();
                    if (lTaxDetail == null || lTaxDetail.Count == 0)
                    {
                        ProductInfo = Context.Products.Where(x => x.HSNCode == HSNCode && x.UseHsnTax == true && x.Type == CatalogItemType.PRODUCT && x.CompanyId == CompanyId).ToList();
                        if (ProductInfo != null && ProductInfo.Count > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }               
        public DataTable ExecuteStoredProcedure(long CompanyId)
        {
            try
            {
                DataTable dataTable = new DataTable();
                AccountMasterContext Context = new AccountMasterContext();
                string ConString = Context.Database.GetDbConnection().ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(ConString))
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("GetCatalogItemsWithSalesTax", connection))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        adapter.SelectCommand.Parameters.AddWithValue("@p_CompanyIds", CompanyId);
                        adapter.Fill(dataTable);
                    }
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
            finally
            {

            }
        }        
    }
}
