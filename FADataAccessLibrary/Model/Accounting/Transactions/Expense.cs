using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;

namespace fa.model.Accounting.Transactions
{
    public class Expense : AuditableEntityForCostCenter
    {
        [Key]
        public long ExpenseId { get; set; }       
        [MaxLength(10)]
        public String Reference { get; set; }
        public long? PayeeId { get; set; }
        [ForeignKey("PayeeId")]
        public virtual Account Payee { get; set; }
        public DateTime TransactionDate { set; get; }
        public PaymentType TransctionType { get; set; }
        public float Amount { get; set; }
        [MaxLength(250)]
        public string Memo { get; set; }
        public ICollection<ExpenseDetail> ExpenseDetails { get; set; } = new List<ExpenseDetail>();
    }

    public class CashExpense : Expense
    {
         public long? CashAccountId { get; set; }
        [ForeignKey("CashAccountId")]
        public virtual Account CashAccount { get; set; }
    }

    public class CheckExpense : Expense
    {
        [MaxLength(20)]
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? BankAccountId { get; set; }
        [ForeignKey("BankAccountId ")]
        public virtual Account BankAccount { get; set; }
    }

    public class CreditCardExpense : Expense
    {
        public long? CCAccountId { get; set; }
        [ForeignKey("CCAccountId ")]
        public virtual Account CCAccount { get; set; }
        public string CCTransactionNumber { get; set; }
        public DateTime CCTransactionDate { set; get; }

    }

    public class BankTransferExpense : Expense
    {
        public long? BankTransferId { get; set; }
        [ForeignKey("BankTransferId")]
        public virtual Account BankTransfer { get; set; }
        [MaxLength(20)]
        public string TransactionNumber { get; set; }
    }

    public class ExpenseDetail : AuditableEntity
    {
        [Key]
        public long ExpenseDetailId { get; set; }
        public long ExpenseId { get; set; }
        [ForeignKey("ExpenseId")]
        public Expense Expense { get; set; }
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
