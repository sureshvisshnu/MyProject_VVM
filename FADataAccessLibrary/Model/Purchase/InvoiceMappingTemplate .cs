using fa.model;
using Fa.model.Purchase;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FADataAccessLibrary.Model.Purchase
{
    public class InvoiceMappingTemplate : AuditableEntityForCompany
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<HeaderMapping> HeaderMappings { get; set; } = new List<HeaderMapping>();
        public virtual ICollection<TransactionMapping> TransactionMappings { get; set; } = new List<TransactionMapping>();
        public virtual ICollection<FooterMapping> FooterMappings { get; set; } = new List<FooterMapping>();

        public virtual ICollection<ProductMappingTemplate> ProductMappings { get; set; } = new List<ProductMappingTemplate>();
    }

    public abstract class BaseMapping : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MappedHeader { get; set; }

        [Required]
        public int MappedColumn { get; set; }

        // Foreign Key to InvoiceMappingTemplate
        public long InvoiceMappingTemplateId { get; set; }

        [ForeignKey(nameof(InvoiceMappingTemplateId))]
        public virtual InvoiceMappingTemplate InvoiceMappingTemplate { get; set; }
    }

    public class HeaderMapping : BaseMapping { }
    public class TransactionMapping : BaseMapping { }
    public class FooterMapping : BaseMapping { }

    public class ProductMappingTemplate : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string InvoiceProductName { get; set; }

        [Required]
        [StringLength(50)]
        public string MaterialId { get; set; }

        [Required]
        [StringLength(100)]
        public string CatalogProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string InvoiceUOM { get; set; }

        [Required]
        [StringLength(20)]
        public string CatalogUOM { get; set; }

        // Foreign Key to InvoiceMappingTemplate
        public long InvoiceMappingTemplateId { get; set; }

        [ForeignKey(nameof(InvoiceMappingTemplateId))]
        public virtual InvoiceMappingTemplate InvoiceMappingTemplate { get; set; }
    }
}