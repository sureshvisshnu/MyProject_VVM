using System;
using System.Collections.Generic;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;

namespace fa.model.Accounting.Transaction
{
    public class Invoice : AuditableEntityForCostCenter
    {
        [Key]
        public long InvoiceId { get; set; }
        public long? TermId { get; set; }
        [ForeignKey("TermId")]
        public PaymentTerm Term { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ReferenceNumber { get; set; }
        public long? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Account Customer { get; set; }
        public string Memo { get; set; }
        public string InternalMemo { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public List<InvoiceAdditionalTransaction> InvoiceAdditionalTransactions { get; set; } = new List<InvoiceAdditionalTransaction>();
        public float Total { get; set; }
        public float Paid { get; set; }
        public float Balance { get; set; }
        public float Discount { get; set; }
        public DiscountType DiscountType { get; set; }
        //public ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
    }

    public class InvoiceDetail : AuditableEntity
    {
        [Key]
        public long InvoiceDetailId { get; set; }
        public long? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        public long? SalesAccountId { get; set; }
        [ForeignKey("SalesAccountId")]
        public virtual Account SalesAccount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Rate { get; set; }
        public float Total { get; set; }
    }
}