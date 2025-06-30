using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;

namespace fa.model.Accounting.Transactions
{
    public class Payment : AuditableEntityForCostCenter
    {
        [Key]
        public long PaymentId { get; set; }
        public DateTime TransactionDate { set; get; }
        [MaxLength(10)]
        public String Reference { get; set; }
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        public long? SalesId { get; set; }
        [ForeignKey("SalesId")]
        public virtual SaleEntry Sales { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public PaymentType TransctionType { get; set; }
        public ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
    }

    public class CheckPayment : Payment
    {
        [MaxLength(20)]
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? DepositedIntoId { get; set; }
        [ForeignKey("DepositedIntoId")]
        public virtual Account BankAccount { get; set; }
        public DateTime DepositedDate { get; set; }
    }

    public class CreditCardPayment : Payment
    {
        public long? CCAccountId { get; set; }
        [ForeignKey("CCAccountId ")]
        public virtual Account CCAccount { get; set; }
        public string CCTransactionNumber { get; set; }
        public DateTime CCTransactionDate { set; get; }
    }

    public class BankTransferPayment : Payment
    {
        public long? BankTransferId { get; set; }
        [ForeignKey("BankTransferId")]
        public virtual Account BankTransfer { get; set; }
        [MaxLength(20)]
        public string TransactionNumber { get; set; }
    }
    public class UpiTransactionPayment : Payment
    {
        public long? UpiTransactionId { get; set; }
        [ForeignKey("UpiTransactionId")]
        public virtual Account UpiAccount { get; set; }
        public string  UpiTransactionNumber { get; set; }
        public DateTime UpiTransactionDate { set; get; }
    }

    public class PaymentDetail : AuditableEntity
    {
        [Key]
        public long PaymentDetailId { get; set; }
        public long PaymentId { get; set; }
        public Payment Payment { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public string ReferenceTrasnactionId { get; set; }
    }
    public enum InvoiceType
    {
        Basic,
        ItemBased,
    }
}
