using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.context;
using FADataAccessLibrary.Model.Purchase;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.EntityFrameworkCore;

namespace FADataAccessLibrary.Api.OrderManagement
{
    public class ProductMappingManager
    {
        private static volatile ProductMappingManager instance;
        private static object syncRoot = new Object();

        public ProductMappingManager() { }
        public static ProductMappingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ProductMappingManager();
                    }
                }
                return instance;
            }
        }
        public ProductMappingTemplate AddProductMappingDetails(ProductMappingTemplate productMapping)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.ProductMappingTemplates.Add(productMapping);
                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return productMapping;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        Console.WriteLine($"Error saving product mapping: {ex.Message}");
                        return null!; 
                    }
                }
            }
        }
        public bool SaveProductMappings(long invoiceMappingTemplateId, List<ProductMappingTemplate> productMappings)
        {
            using (var context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var parentTemplate = context.InvoiceMappingTemplates
                            .Include(t => t.ProductMappings)
                            .FirstOrDefault(t => t.Id == invoiceMappingTemplateId);

                        if (parentTemplate == null)
                        {
                            Console.WriteLine("Parent template not found!");
                            return false;
                        }

                        var existingMappings = parentTemplate.ProductMappings.ToList();
                        var existingProductNames = existingMappings
                            .ToDictionary(em => em.InvoiceProductName, em => em);

                        foreach (var incomingMapping in productMappings)
                        {
                            incomingMapping.InvoiceMappingTemplateId = invoiceMappingTemplateId;

                            if (incomingMapping.Id != 0)
                            {
                                var existingById = existingMappings.FirstOrDefault(em => em.Id == incomingMapping.Id);
                                if (existingById != null)
                                {
                                    UpdateExistingMapping(context, existingById, incomingMapping);
                                    continue;
                                }
                            }

                            if (existingProductNames.TryGetValue(incomingMapping.InvoiceProductName, out var existingByName))
                            {
                                incomingMapping.Id = existingByName.Id; // Key fix
                                UpdateExistingMapping(context, existingByName, incomingMapping);
                            }
                            else
                            {
                                incomingMapping.Id = 0; 
                                context.ProductMappingTemplates.Add(incomingMapping);
                                existingProductNames.Add(incomingMapping.InvoiceProductName, incomingMapping);
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error saving mappings: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        // Helper method to update existing mappings while preserving audit fields
        private void UpdateExistingMapping(AccountMasterContext context, ProductMappingTemplate existing, ProductMappingTemplate incoming)
        {
            context.Entry(existing).CurrentValues.SetValues(incoming);
        }        
        public long? SaveProductMappingsNew(long invoiceMappingTemplateId, List<ProductMappingTemplate> productMappings)
        {
            using (var context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var parentTemplate = context.InvoiceMappingTemplates
                            .Include(t => t.ProductMappings)
                            .FirstOrDefault(t => t.Id == invoiceMappingTemplateId);

                        if (parentTemplate == null) return null;

                        foreach (var mapping in productMappings)
                        {
                            if (mapping.Id == 0)
                            {
                                context.ProductMappingTemplates.Add(mapping);
                            }
                            else
                            {
                                context.ProductMappingTemplates.Update(mapping);
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();

                        return productMappings.FirstOrDefault()?.Id;
                    }
                    catch
                    {
                        transaction.Rollback();
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }
        public List<ProductMappingTemplate> GetAllProductMappingTemplate(long invoiceTempID)
        {
            using (var context = new AccountMasterContext())
            {
                return context.InvoiceMappingTemplates
                    .Include(t => t.ProductMappings)  
                    .Where(t => t.Id == invoiceTempID)
                    .SelectMany(t => t.ProductMappings) 
                    .ToList();
            }
        }
        public ProductMappingTemplate GetTemplateProductByName(string Product, long invoiceTempID)
        {
            ProductMappingTemplate ProductInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProductInfo = Context.ProductMappingTemplates.Where(p => p.InvoiceProductName == Product && p.InvoiceMappingTemplateId == invoiceTempID).FirstOrDefault<ProductMappingTemplate>();
                return ProductInfo;
            }
        }
    }
}
