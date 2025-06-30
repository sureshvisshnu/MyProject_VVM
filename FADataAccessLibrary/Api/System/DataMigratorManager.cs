using fa.api.Accounting;
using fa.api.catalog;
using fa.context;
using fa.Data;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using Fa.model.Purchase;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Principal;

namespace fa.api.System
{
    public class DataMigratorManager
    {
        private static volatile DataMigratorManager instance;
        private static object syncRoot = new Object();
        DataMigratorManager()
        {

        }
        public static DataMigratorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DataMigratorManager();
                    }
                }

                return instance;
            }
        }


        public Category CheckCategoryName(String CategoryName, long CompanyId, CatalogItemType Type)
        {
            Category CategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CategoryInfo = Context.Categories.FirstOrDefault(x => x.Name == CategoryName && x.Type == Type && x.CompanyId == CompanyId);
                return CategoryInfo;
            }
        }
        public CatalogItem CheckProductFamilyName(String ProductFamilyName, String CategoryName, long CompanyId)
        {
            CatalogItem CatalogItemInfo = null;
            CatalogItem CheckCat = CheckCategoryName(CategoryName, CompanyId, CatalogItemType.CATEGORY);
            if (CheckCat != null)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    CatalogItemInfo = Context.CatalogItems.FirstOrDefault(x => x.Name == ProductFamilyName && x.Type == CatalogItemType.PRODUCTFAMILY && x.ParentId == CheckCat.Id && x.CompanyId == CompanyId);
                    return CatalogItemInfo;
                }
            }
            return CatalogItemInfo;
        }
        public bool ProductNameUnique(long ProductFamilyId, String ProductName, CatalogItemType CatalogItemType, long CompanyId)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (Context.Products.FirstOrDefault(x => x.Name == ProductName && x.Type == CatalogItemType && x.ParentId == ProductFamilyId && x.CompanyId == CompanyId) != null)
                    {
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


        public bool UpdateProducts(Product lProduct, String ProductCategory, string ProductFamily,string SalesAc,string PurchaseAc, string InventoryAc)
        {
            bool Status = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        long? CategoryId = null;
                        if (!string.IsNullOrEmpty(ProductCategory))
                        {
                            CatalogItem CategoryInfo = null;
                            CategoryInfo = Context.CatalogItems.FirstOrDefault(x => x.CompanyId == lProduct.CompanyId && x.Type == CatalogItemType.CATEGORY && x.Name == ProductCategory);
                            if (CategoryInfo != null)
                            {
                                CategoryId = CategoryInfo.Id;
                            }
                            else
                            {
                                Category lCategory = new Category();
                                lCategory.CompanyId = lProduct.CompanyId;
                                lCategory.Name = ProductCategory.Trim();
                                lCategory.Description = "";
                                lCategory.DisplayAs = ProductCategory.Trim();
                                lCategory.Type = CatalogItemType.CATEGORY;
                                CategoryInfo = CategoryManager.Instance.AddCategory(lCategory);
                                CategoryId = CategoryInfo.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(ProductFamily))
                        {
                            CatalogItem CatalogItemInfo = null;
                            CatalogItemInfo = Context.CatalogItems.FirstOrDefault(x => x.CompanyId == lProduct.CompanyId && x.Type == CatalogItemType.PRODUCTFAMILY  && x.Name == ProductFamily && x.ParentId == CategoryId);
                            if (CatalogItemInfo != null)
                            {
                                lProduct.ParentId= CatalogItemInfo.Id;
                                lProduct.ProductFamilyId = CatalogItemInfo.Id;
                            }
                            else
                            {
                                ProductFamily lProductFamily = new ProductFamily();
                                lProductFamily.CompanyId = lProduct.CompanyId;
                                lProductFamily.Name = ProductFamily;
                                lProductFamily.Description = "";
                                lProductFamily.DisplayAs = ProductFamily;
                                lProductFamily.Type = CatalogItemType.PRODUCTFAMILY;
                                lProductFamily._isInventoryAtBatch = false;
                                lProductFamily.Manufacturer = "";
                                lProductFamily.SupplierName = "";
                                lProductFamily.ParentId = CategoryId;
                                CatalogItemInfo = CatalogProductFamilyManager.Instance.AddProductFamily(lProductFamily);
                                lProduct.ParentId = CatalogItemInfo.Id;
                                lProduct.ProductFamilyId = CatalogItemInfo.Id;
                            }
                        }
                        if (!string.IsNullOrEmpty(SalesAc))
                        {
                            lProduct.SalesAccount = CheckAndInsertAccounts(SalesAc, 1, lProduct.CompanyId, Context);
                        }
                        if (!string.IsNullOrEmpty(PurchaseAc))
                        {
                            lProduct.PurchaseAccount = CheckAndInsertAccounts(PurchaseAc, 2, lProduct.CompanyId, Context);
                        }
                        if (!string.IsNullOrEmpty(InventoryAc))
                        {
                            lProduct.InventoryAccount = CheckAndInsertAccounts(InventoryAc, 3, lProduct.CompanyId, Context);
                        }
                        Product llProduct = Context.Products.FirstOrDefault(x => x.CompanyId == lProduct.CompanyId && x.MaterialId == lProduct.MaterialId);
                        if (llProduct != null)
                        {
                            lProduct.Id = llProduct.Id;
                        }
                        //if (CatalogProductManager.Instance.ProductNameUniqueById(lProduct,Context))
                        {
                            if (lProduct.Id != 0L)
                            {
                                lProduct.Id = llProduct.Id;
                                CatalogProductManager.Instance.UpdateProductFromExcel(lProduct,Context);
                            }
                            else
                            {
                                Context.Products.Add(lProduct);
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                            Status= true;
                        }
                        //else
                        //{
                        //    Status= false;
                        //}
                       
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Status;    
        }
        public Account CheckAndInsertAccounts(string AccountName, int Type,long CompanyId, AccountMasterContext Context)
        {
            Account AccountInfo = null;
            AccountInfo = Context.Accounts.FirstOrDefault(x => x.CompanyId == CompanyId && x.Name == AccountName);
            if (AccountInfo != null)
            {
                return AccountInfo;
            }
            else
            {
                Account lAccount = new Account();
                lAccount.Name = AccountName.Trim();
                lAccount.Discription = "";
                lAccount.Balance = 0;
                lAccount.BalanceAsOf = DateTime.Today.Date;
                lAccount.CompanyId = CompanyId;
                lAccount.DisplayAs = AccountName.Trim();
                lAccount.IsSubAccount = false;
                lAccount.AccountType = AccountType.ACCOUNT;
                lAccount.AccountGroupId = Type == 1 ? 1104 : Type == 2 ? 1422 : 1422;
                Context.Accounts.Add(lAccount);
                Context.SaveChanges();
                return lAccount;
            }
        }
    }
}
