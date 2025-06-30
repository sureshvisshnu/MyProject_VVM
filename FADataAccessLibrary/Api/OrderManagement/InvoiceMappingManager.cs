using fa.api.OrderManagement;
using fa.context;
using fa.model.Catalog;
using fa.model.Employee;
using fa.model.Hms.Master;
using FADataAccessLibrary.Model.Purchase;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.OrderManagement
{
    public class InvoiceMappingManager
    {
        private static volatile InvoiceMappingManager instance;
        private static object syncRoot = new Object();
        public InvoiceMappingManager() { }
        public static InvoiceMappingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InvoiceMappingManager();
                    }
                }
                return instance;
            }
        }
        public InvoiceMappingTemplate AddInvoiceTemplate(InvoiceMappingTemplate invoiceMapping)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.InvoiceMappingTemplates.Add(invoiceMapping);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        invoiceMapping = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }            
            return invoiceMapping;
        }
        public bool InvoiceTemplateUniqueByIdNew(InvoiceMappingTemplate invoiceMappingTemplate)
        {
            using (var context = new AccountMasterContext())
            {
                try
                {
                    // Check for existing templates with same name and company
                    bool exists = invoiceMappingTemplate.Id == 0
                        // For new templates
                        ? context.InvoiceMappingTemplates.Any(x =>
                            x.Name == invoiceMappingTemplate.Name &&
                            x.CompanyId == invoiceMappingTemplate.CompanyId)
                        // For existing templates (exclude current ID)
                        : context.InvoiceMappingTemplates.Any(x =>
                            x.Name == invoiceMappingTemplate.Name &&
                            x.CompanyId == invoiceMappingTemplate.CompanyId &&
                            x.Id != invoiceMappingTemplate.Id);

                    // Return true if unique (no existing record)
                    return !exists;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Validation error: {ex}");
                    return false; // Fail-safe return
                }
            }
        }
        public Boolean InvoiceTemplateUniqueById(InvoiceMappingTemplate invoiceMappingTemplate)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if(invoiceMappingTemplate != null)
                    {
                        InvoiceMappingTemplate InvoiceMappingTemplateInfo = null;
                        if (invoiceMappingTemplate.Id == 0)
                        {
                            InvoiceMappingTemplateInfo = Context.InvoiceMappingTemplates.FirstOrDefault(x => x.Name == invoiceMappingTemplate.Name && x.CompanyId == invoiceMappingTemplate.CompanyId);
                        }
                        else
                        {
                            InvoiceMappingTemplateInfo = Context.InvoiceMappingTemplates.FirstOrDefault(x => x.Name == invoiceMappingTemplate.Name && x.CompanyId == invoiceMappingTemplate.CompanyId && !x.Id.Equals(invoiceMappingTemplate.Id));
                        }
                        if (InvoiceMappingTemplateInfo != null)
                        {
                            Status = false;
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Status = false;
                    Console.WriteLine(ex.ToString());
                }
            }
            return Status;
        }
        public List<string> GetAllInvoiceMappingTemplate(long CompanyId)
        {
            List<string> InvoiceMapping = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InvoiceMapping = Context.InvoiceMappingTemplates.Where(c => c.CompanyId == CompanyId).Select(p => p.Name).Distinct().ToList();
            }
            return InvoiceMapping;
        }
        public IList<InvoiceMappingTemplate> ListInvoiceMappingByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<InvoiceMappingTemplate> InvoiceMappingInfo = (from InvoiceMappingTemplates in Context.InvoiceMappingTemplates where InvoiceMappingTemplates.CompanyId == CompanyId select (InvoiceMappingTemplates)).ToList(); // Select(p => p.WholesaleUOM).Distinct().ToList()
                return InvoiceMappingInfo;
            }
        }
        public InvoiceMappingTemplate GetInvoiceMappingByName(string MappingName, long CompanyID) 
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                return Context.InvoiceMappingTemplates
                    .FirstOrDefault(m => m.Name == MappingName && m.CompanyId == CompanyID); 
            }
        }
        public InvoiceMappingTemplate GetInvoiceMappingById(long MappingId, long CompanyID)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                return Context.InvoiceMappingTemplates
                    .FirstOrDefault(m => m.Id == MappingId && m.CompanyId == CompanyID);
            }
        }
        public InvoiceMappingTemplate GetFullTemplateWithMappings(long templateId)
        {
            using (var context = new AccountMasterContext())
            {
                return context.InvoiceMappingTemplates
                    .Include(t => t.HeaderMappings)          // Header mappings
                    .Include(t => t.TransactionMappings)    // Transaction mappings
                    .Include(t => t.FooterMappings)         // Footer mappings
                    .Include(t => t.ProductMappings)        // Product mappings
                    .FirstOrDefault(t => t.Id == templateId);
            }
        }
    }
}
