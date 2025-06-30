using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;

namespace fa.model.Accounting.Transactions
{
    public class Receipt : AuditableEntityForCostCenter
    {
        [Key]
        public long ReceiptId { get; set; }
        public DateTime TransactionDate { set; get; }
        [MaxLength(10)]
        public String Reference { get; set; }       
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }        
        public decimal Amount { get; set; }
        public PaymentType TransactionType { get; set; }
        public ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();
        public string DayBookMemo
        {
            get
            {
                return !String.IsNullOrEmpty(Description) ? ",(" + Description + ")" : "";
            }
        }
    }

    public class CheckReceipt : Receipt
    {
        [MaxLength(20)]
        public string DocumentNumber { get; set; }
        [MaxLength(30)]
        public string InstitutionName { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? DepositedIntoId { get; set; }
        [ForeignKey("DepositedIntoId")]
        public virtual Account DepositedInto { get; set; }
        public DateTime DepositedDate { get; set; }
    }

    public class CreditCardReceipt : Receipt
    {
        public long? CCAccountId { get; set; }
        [ForeignKey("CCAccountId ")]
        public virtual Account CCAccount { get; set; }
        public string CCTransactionNumber { get; set; }
        public DateTime CCTransactionDate { set; get; }
    }

    public class BankTransferReceipt : Receipt
    {
        public long? BankTransferId { get; set; }
        [ForeignKey("BankTransferId")]
        public virtual Account BankTransfer { get; set; }
        [MaxLength(20)]
        public string TransactionNumber { get; set; }
    }

    public class ReceiptDetail : AuditableEntity
    {
        [Key]
        public long ReceiptDetailId { get; set; }
        public long ReceiptId { get; set; }
        [ForeignKey("ReceiptId")]
        public Receipt Receipt { get; set; }
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public string ReferenceTrasnactionId { get; set; }
    }
    
}
