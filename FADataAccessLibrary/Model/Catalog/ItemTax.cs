using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;

namespace fa.model.catalog
{
    [Index(nameof(Code),nameof(CompanyId), Name = "IDX_HsnCode", IsUnique = true)]
    public class ItemTax: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public string Description { get; set; }
        public ICollection<ItemSalesTaxMap> SalesTaxMapLocal { get; set; } = new List<ItemSalesTaxMap>();
    }

    public class ItemSalesTaxMap: AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long? ItemTaxId { get; set; }
        [ForeignKey("ItemTaxId")]
        public  ItemTax ItemTax { get; set; }
        public long? SalesTaxMapId { get; set; }
        [ForeignKey("SalesTaxMapId")]
        public virtual CompanySalesTaxAccountMap CompanySalesTaxAccountMap { get; set; }
        public float TaxPercentage { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
    }
}
