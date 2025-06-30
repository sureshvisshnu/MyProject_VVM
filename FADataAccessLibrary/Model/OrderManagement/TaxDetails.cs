using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.OrderManagement
{
    public class TaxDetail : AuditableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int TaxSequence { get; set; }
        public long? TaxAccountId { get; set; }
        [ForeignKey("TaxAccountId")]
        public Account TaxAccount { get; set; }
        public float TaxRate { get; set; }
        public float Amount { get; set; }
        public long? ItemTaxMapId { get; set; }
        [ForeignKey("ItemTaxMapId")]
        public CatalogItemSalesTaxMap CatalogItemSalesTaxMap { get; set; }
        public long? ItemCodeTaxMapId { get; set; }
        [ForeignKey("ItemCodeTaxMapId")]
        public ItemSalesTaxMap ItemSalesTaxMap { get; set; }
    }

    public class OrderLevelSaleTaxDetail : TaxDetail
    {
        public long? SaleId { get; set; }
        [ForeignKey("SaleId")]
        public SaleEntry SaleEntry { get; set; }
    }

    public class ItemLevelSaleTaxDetail : TaxDetail
    {
        public long? SaleDetailsId { get; set; }
        [ForeignKey("SaleDetailsId")]
        public SaleDetail SaleDetails { get; set; }
    }

    public class OrderLevelPurchaseTaxDetail : TaxDetail
    {
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId ")]
        public PurchaseEntry PurchaseEntry { get; set; }
    }

    public class LineLevelPurchaseTaxDetail : TaxDetail
    {
        public long? PurchaseDetailsId { get; set; }
        [ForeignKey("PurchaseDetailsId")]
        public PurchaseDetails PurchaseDetails { get; set; }
    }

}
