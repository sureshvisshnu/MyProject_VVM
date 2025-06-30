using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.OrderManagement
{
    public class AdditionalTransaction: AuditableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }       
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public AdditionalTransactionType Type { get; set; }
        public AdditionalTransactionAction Action { get; set; }
        public double Value { get; set; }
        public double Amount { get; set; }
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
    public class PurchaseAdditionalTransaction : AdditionalTransaction
    {
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId")]
        public PurchaseEntry PurchaseEntry { get; set; }
    }
    public class SaleAdditionalTransaction : AdditionalTransaction
    {
        public long? SaleEntryId { get; set; }
        [ForeignKey("SaleEntryId")]
        public SaleEntry SaleEntry { get; set; }
    }
    public class InvoiceAdditionalTransaction : AdditionalTransaction
    {
        public long? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
    }
}
