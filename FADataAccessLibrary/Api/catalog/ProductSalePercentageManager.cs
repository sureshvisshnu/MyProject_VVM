using fa.api.catalog;
using fa.context;
using fa.model.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.catalog
{
    public class ProductSalePercentageManager
    {
        private static volatile ProductSalePercentageManager instance;
        private static object syncRoot = new Object();
        ProductSalePercentageManager() 
        { 

        }

        public static ProductSalePercentageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ProductSalePercentageManager();
                        }
                    }
                }
                return instance;
            }
        }
        public async Task<ProductPercentage> GetProductSalePercentageAsync(string productCode)
        {
            using (var context = new AccountMasterContext())
            {
                return await context.ProductPercentages
                    .FirstOrDefaultAsync(pp => pp.ProductCode == productCode && pp.IsActive);
            }
        }
        public async Task<PercentageStorage?> GetCompanyDefaultPercentagesAsync(long companyId)
        {
            using (var context = new AccountMasterContext())
            {
                return await context.PercentageStorages
                                    .Where(p => p.CompanyId == companyId && p.IsActive)
                                    .FirstOrDefaultAsync();
            }
        }

        public ProductPercentage GetProductSalePercentage(long productId)
        {
            ProductPercentage productPercentage = null;
            using (var context = new AccountMasterContext())
            {
                return productPercentage = context.ProductPercentages
                    .FirstOrDefault(pp => pp.ProductId == productId && pp.IsActive);
            }
        }
        public async Task<List<ProductPercentage>> GetAllProductSalePercentagesAsync()
        {
            using (var context = new AccountMasterContext())
            {
                return await context.ProductPercentages
                    .Where(pp => pp.IsActive)
                    .ToListAsync();
            }
        }
        public async Task<long> GetProductIdByCodeAsync(string productCode)
        {
            using (var context = new AccountMasterContext())
            {
                var product = await context.Products
                    .Where(p => p.MaterialId == productCode)
                    .FirstOrDefaultAsync();

                return product?.Id ?? throw new Exception($"Product with code {productCode} not found");
            }
        }
        public async Task<ProductPercentage> AddProductSalePercentageAsync(ProductPercentage productPercentage)
        {
            using (var context = new AccountMasterContext())
            {
                context.ProductPercentages.Add(productPercentage);
                await context.SaveChangesAsync();
                return productPercentage;
            }
        }
        public async Task<ProductPercentage> UpdateProductSalePercentageAsync(ProductPercentage productPercentage)
        {
            using (var context = new AccountMasterContext())
            {
                context.ProductPercentages.Update(productPercentage);
                await context.SaveChangesAsync();
                return productPercentage;
            }
        }
        public async Task<bool> DeleteProductSalePercentageAsync(long id)
        {
            using (var context = new AccountMasterContext())
            {
                var productPercentage = await context.ProductPercentages.FindAsync(id);
                if (productPercentage == null)
                {
                    return false; // Not found
                }
                productPercentage.IsActive = false; // Soft delete
                context.ProductPercentages.Update(productPercentage);
                await context.SaveChangesAsync();
                return true;
            }
        }
        public async Task SaveProductPercentagesAsync(ProductPercentage percentages)
        {
            using (var context = new AccountMasterContext())
            {
                var existing = await context.ProductPercentages
                    .FirstOrDefaultAsync(p => p.ProductCode == percentages.ProductCode && p.ProductId == percentages.ProductId && p.CompanyId == percentages.CompanyId);

                if (existing != null)
                {
                    // Update existing record
                    existing.AddedCostPercentage = percentages.AddedCostPercentage;
                    existing.RetailMarginPercentage = percentages.RetailMarginPercentage;
                    existing.WholesaleMarginPercentage = percentages.WholesaleMarginPercentage;
                    existing.MrpPercentage = percentages.MrpPercentage;
                    existing.IsActive = percentages.IsActive;
                    existing.Notes = percentages.Notes;
                }
                else
                {
                    // Add new record
                    context.ProductPercentages.Add(percentages);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task AddMissingProductPercentagesAsync()
        {
            using (var context = new AccountMasterContext())
            {
                var existingProductCodes = await context.ProductPercentages
                    .Select(p => p.ProductCode)
                    .ToListAsync();

                var missingProducts = await context.Products
                    .Where(ci => !existingProductCodes.Contains(ci.MaterialId) &&
                                 ci.PurchasePrice > 0 &&
                                 ci.CostPrice > 0 &&
                                 ci.Msrp > 0)
                    .ToListAsync();

                foreach (var product in missingProducts)
                {
                    var addedCostPct = ((product.CostPrice - product.PurchasePrice) / product.PurchasePrice) * 100;
                    var retailMarginPct = ((product.Msrp - product.RetailPrice) / product.Msrp) * 100;
                    var wholesaleMarginPct = ((product.Msrp - product.WholdSalePrice) / product.Msrp) * 100;
                    var mrpPct = ((product.Msrp - product.CostPrice) / product.CostPrice) * 100 + 100;

                    var percentage = new ProductPercentage
                    {
                        ProductId = product.Id,
                        ProductCode = product.MaterialId,
                        CompanyId = product.CompanyId,
                        AddedCostPercentage = Math.Round((decimal)addedCostPct, 0),
                        RetailMarginPercentage = Math.Round((decimal)retailMarginPct, 0),
                        WholesaleMarginPercentage = Math.Round((decimal)wholesaleMarginPct, 0),
                        MrpPercentage = Math.Round((decimal)mrpPct, 0),
                        IsActive = true,
                        CreatedBy = "BulkInsertScript"
                    };

                    // Reuse your save method
                    await SaveProductPercentagesAsync(percentage);
                }
            }
        }

    }
}
