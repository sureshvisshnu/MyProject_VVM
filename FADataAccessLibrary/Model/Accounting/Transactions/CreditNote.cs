using System;
using System.Collections.Generic;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;

namespace fa.model.Accounting.Transactions
{
    public class CreditNote : AuditableEntityForCostCenter
    {
        [Key]
        public long CreditNoteId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public long? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Note { get; set; }
        public string InternalNote { get; set; }
        public float Amount { get; set; }
        public float Paid { get; set; }
        public float Balance { get; set; }
        public ICollection<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
       
    }

    public class CreditNoteDetail : AuditableEntity
    {
        [Key]
        public long CreditNoteDetailsId { get; set; }
        public long? CreditNoteId { get; set; }
        public virtual CreditNote CreditNote { get; set; }
        public long? AccountId { get; set; }
        public virtual Account Account { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
