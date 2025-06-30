using fa.model.Accounting.Transaction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.OrderManagement
{
    public class DiscountDetail: AuditableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int DiscountSequence { get; set; }
        public DiscountType DisccountType { get; set; }
        public float Discount { get; set; }
        public float DiscountAmount { get; set; }
        public string DiscountDescription { get; set; }
    }
    public class OrderLevelSaleDiscount : DiscountDetail
    {
        public long? SaleId { get; set; }
        [ForeignKey("SaleId")]
        public SaleEntry SaleEntry { get; set; }
    }

    public class ItemLevelSaleDiscount : DiscountDetail
    {
        public long? SaleDetailsId { get; set; }
        [ForeignKey("SaleDetailsId")]
        public SaleDetail SaleDetails { get; set; }
    }

    public class OrderLevelPurchaseDiscount : DiscountDetail
    {
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId ")]
        public PurchaseEntry PurchaseEntry { get; set; }
    }

    public class LineLevelPurchaseDiscount : DiscountDetail
    {
        public long? PurchaseDetailsId { get; set; }
        [ForeignKey("PurchaseDetailsId")]
        public PurchaseDetails PurchaseDetails { get; set; }
    }
}
