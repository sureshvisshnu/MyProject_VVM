using fa.context;
using FADataAccessLibrary.Model; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.OrderManagement
{
    public class ItemSalesManager
    {
        private readonly AccountMasterContext _context;

        public ItemSalesManager(AccountMasterContext context)
        {
            _context = context;
        }

        // Synchronous version
        public List<ItemSalesData> GetItemSalesReport(long itemId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var salesData = _context.SaleEntry
                    .Where(se => se.SaleDate >= fromDate && se.SaleDate <= toDate)
                    .SelectMany(se => se.SaleDetails
                        .Where(sd => sd.ProductId == itemId)
                        .Select(sd => new ItemSalesData
                        {
                            SaleDate = se.SaleDate,
                            CustomerName = se.CustomerName,
                            RefNumber = se.RefNumber,
                            Quantity = sd.Quantity,
                            Price = sd.Price,
                            Amount = sd.Amount,
                            SaleType = se.SaleType.ToString(),
                            SaleMethod = se.SaleMethod.ToString(),
                            CustomerAddress = se.CustomerAddress,
                            ProductName = sd.Product.Name, // Assuming Product has a Name property
                            FreeQuantity = sd.FreeQuantity,
                            Uom = sd.Uom
                        }))
                    .ToList();

                return salesData;
            }
            catch (Exception ex)
            {
                // Log error here
                throw new ApplicationException("Error retrieving item sales data", ex);
            }
        }

        // Async version (requires Microsoft.EntityFrameworkCore)
        public async Task<List<ItemSalesData>> GetItemSalesReportAsync(long itemId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var salesData = await _context.SaleEntry
                    .Where(se => se.SaleDate >= fromDate && se.SaleDate <= toDate)
                    .SelectMany(se => se.SaleDetails
                        .Where(sd => sd.ProductId == itemId)
                        .Select(sd => new ItemSalesData
                        {
                            SaleDate = se.SaleDate,
                            CustomerName = se.CustomerName,
                            RefNumber = se.RefNumber,
                            Quantity = sd.Quantity,
                            Price = sd.Price,
                            Amount = sd.Amount,
                            SaleType = se.SaleType.ToString(),
                            SaleMethod = se.SaleMethod.ToString(),
                            CustomerAddress = se.CustomerAddress,
                            ProductName = sd.Product.Name,
                            FreeQuantity = sd.FreeQuantity,
                            Uom = sd.Uom
                        }))
                    .ToListAsync(); // This requires Microsoft.EntityFrameworkCore

                return salesData;
            }
            catch (Exception ex)
            {
                // Log error here
                throw new ApplicationException("Error retrieving item sales data", ex);
            }
        }

        public ItemSalesSummary GetItemSalesSummary(long itemId, DateTime fromDate, DateTime toDate)
        {
            var salesData = GetItemSalesReport(itemId, fromDate, toDate);

            return new ItemSalesSummary
            {
                TotalQuantity = salesData.Sum(item => item.Quantity),
                TotalAmount = salesData.Sum(item => item.Amount),
                TotalFreeQuantity = salesData.Sum(item => item.FreeQuantity),
                SaleCount = salesData.Count,
                AveragePrice = salesData.Any() ? salesData.Average(item => item.Price) : 0
            };
        }

        // Async version of summary
        public async Task<ItemSalesSummary> GetItemSalesSummaryAsync(long itemId, DateTime fromDate, DateTime toDate)
        {
            var salesData = await GetItemSalesReportAsync(itemId, fromDate, toDate);

            return new ItemSalesSummary
            {
                TotalQuantity = salesData.Sum(item => item.Quantity),
                TotalAmount = salesData.Sum(item => item.Amount),
                TotalFreeQuantity = salesData.Sum(item => item.FreeQuantity),
                SaleCount = salesData.Count,
                AveragePrice = salesData.Any() ? salesData.Average(item => item.Price) : 0
            };
        }

        public List<ItemSalesData> GetItemSalesByCustomer(long itemId, DateTime fromDate, DateTime toDate)
        {
            var salesData = GetItemSalesReport(itemId, fromDate, toDate);

            return salesData
                .GroupBy(item => item.CustomerName)
                .Select(group => new ItemSalesData
                {
                    CustomerName = group.Key,
                    Quantity = group.Sum(item => item.Quantity),
                    Amount = group.Sum(item => item.Amount),
                    FreeQuantity = group.Sum(item => item.FreeQuantity),
                    SaleCount = group.Count()
                })
                .OrderByDescending(item => item.Amount)
                .ToList();
        }

        // Async version of sales by customer
        public async Task<List<ItemSalesData>> GetItemSalesByCustomerAsync(long itemId, DateTime fromDate, DateTime toDate)
        {
            var salesData = await GetItemSalesReportAsync(itemId, fromDate, toDate);

            return salesData
                .GroupBy(item => item.CustomerName)
                .Select(group => new ItemSalesData
                {
                    CustomerName = group.Key,
                    Quantity = group.Sum(item => item.Quantity),
                    Amount = group.Sum(item => item.Amount),
                    FreeQuantity = group.Sum(item => item.FreeQuantity),
                    SaleCount = group.Count()
                })
                .OrderByDescending(item => item.Amount)
                .ToList();
        }
    }

    public class ItemSalesData
    {
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string RefNumber { get; set; }
        public double Quantity { get; set; }
        public double FreeQuantity { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }
        public string SaleType { get; set; }
        public string SaleMethod { get; set; }
        public string ProductName { get; set; }
        public string Uom { get; set; }
        public int SaleCount { get; set; } // For grouped data
    }

    public class ItemSalesSummary
    {
        public double TotalQuantity { get; set; }
        public double TotalFreeQuantity { get; set; }
        public float TotalAmount { get; set; }
        public int SaleCount { get; set; }
        public double AveragePrice { get; set; }
    }
}