using System;
using System.Collections.Generic;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
using fa.model.Accounting.Transactions;

namespace fa.model.Accounting.Transaction
{
    public class Bill : AuditableEntityForCostCenter
    {
        [Key]
        public long BillId { get; set; }
        public long TermId { get; set; }
        public PaymentTerm Term { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ReferenceNumber { get; set; }
        public long VendorId { get; set; }
        public Account Vendor { get; set; }
        public string Memo { get; set; }
        public string InternalMemo { get; set; }
        public ICollection<BillAttachment> BillAttachments { get; set; } = new List<BillAttachment>();
        public ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
        public float Total { get; set; }
        public float Paid { get; set; }
        public float Balance { get; set; }
        //public ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
    }

    public class BillAttachment : AuditableEntity
    {
        [Key]
        public long BillAttachementId { get; set; }
        public long BillId { get; set; }
        public Bill Bill { get; set; }
        public byte[] Attachment { get; set; }
    }

    public class BillDetail : AuditableEntity
    {
        [Key]
        public long BillDetailId { get; set; }
        public long BillId { get; set; }
        public Bill Bill { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}