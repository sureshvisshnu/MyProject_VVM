using fa.context;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.catalog
{
    public class ProductPriceHistoryManager
    {
        private static ProductPriceHistoryManager _instance;
        public static ProductPriceHistoryManager Instance => _instance ??= new ProductPriceHistoryManager();

        private ProductPriceHistoryManager() { }

        public void AddPriceHistory(long productId, long companyId, float oldPrice, float newPrice, string source = "Manual Edit")
        {
            if (oldPrice == newPrice)
                return;

            using (var context = new AccountMasterContext())
            {
                var history = new ProductPriceHistory
                {
                    ProductId = productId,
                    CompanyId = companyId,
                    OldPurchasePrice = oldPrice,
                    NewPurchasePrice = newPrice,
                    ChangedDate = DateTime.Now,
                    Source = source
                };

                context.ProductPriceHistories.Add(history);
                context.SaveChanges();
            }
        }

        public IList<ProductPriceHistory> GetPriceHistoryByProductId(long productId, long companyId)
        {
            using (var context = new AccountMasterContext())
            {
                return context.ProductPriceHistories
                    .Where(p => p.ProductId == productId && p.CompanyId == companyId)
                    .OrderByDescending(p => p.ChangedDate)
                    .ToList();
            }
        }

    }
}
