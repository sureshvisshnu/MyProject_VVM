using System;
using System.Collections.Generic;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Transaction
{
    public class Journal : AuditableEntityForCostCenter
    {
        [Key]
        public long JournalId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public ICollection<JournalDetail> JournalDetails { get; set; } = new List<JournalDetail>();     
    }

    public class JournalDetail : AuditableEntity
    {
        [Key]
        public long JournalDetailId { get; set; }
        public long? JournalId { get; set; }
        [ForeignKey("JournalId")]
        public virtual Journal Journal { get; set; }
        public long? ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        public virtual Account ToAccount { get; set; }
        public decimal Amount { get; set; }
        public long? ReferenceAccountId { get; set; }
        [ForeignKey("ReferenceAccountId")]
        public virtual Account ReferenceAccount { get; set; }
        public string Description { get; set; }
    }
}
