using System;
using System.Collections.Generic;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
namespace fa.model.Accounting.Transactions
{
    public class DebitNote : AuditableEntityForCostCenter
    {
        [Key]
        public long DebitNoteId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public long? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Note { get; set; }
        public string InternalNote { get; set; }
        public float Amount { get; set; }
        public float Paid { get; set; }
        public float Balance { get; set; }
        public ICollection<DebitNoteDetail> DebitNoteDetails { get; set; } = new List<DebitNoteDetail>();
      
    }

    public class DebitNoteDetail : AuditableEntity
    {
        [Key]
        public long DebitNoteDetailsId { get; set; }
        public long? DebitNoteId { get; set; }
        public virtual DebitNote DebitNote { get; set; }
        public long? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
