using fa.model;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using fa.model.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FADataAccessLibrary.Model.Accounting.Transactions
{
    public class PaymentNew : AuditableEntityForCostCenter
    {
        [Key]
        public long PaymentNewId { get; set; }  // Changed from PaymentId

        public DateTime TransactionDate { get; set; }

        [MaxLength(10)]
        public string Reference { get; set; }

        public long? AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PaymentType TransactionType { get; set; } // Fixed spelling

        public ICollection<PaymentDetailNew> PaymentDetails { get; set; }
            = new List<PaymentDetailNew>(); // Points to new PaymentDetail

        public long SalesId { get; set; }

        public DateTime PaymentDueDate { get; set; }
        public DateTime PaymentDueDateUtc { get; set; }
        public bool IsDeleted { get; set; }

        public long? DeletedById { get; set; }

        [ForeignKey(nameof(DeletedById))]
        public virtual User DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string DeletionReason { get; set; }

        // Add a discriminator to know which type of payment this is
        public string PaymentTypeDiscriminator { get; set; } // "Check", "CreditCard", "BankTransfer", "UPI", "Regular"
    }
    public class PaymentDetailNew : AuditableEntity
    {
        [Key]
        public long PaymentDetailNewId { get; set; } // Changed from PaymentDetailId

        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long PaymentNewId { get; set; } // Changed from PaymentId
        public PaymentNew Payment { get; set; } // Points to new Payment

        // Invoice reference
        public long SalesId { get; set; }
        public SaleEntry Sales { get; set; }

        public decimal Amount { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public string ReferenceTransactionId { get; set; } // Fixed spelling
        public string Description { get; set; }
    }
    public class CheckPaymentNew : PaymentNew
    {
        [MaxLength(20)]
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? DepositedIntoId { get; set; }

        [ForeignKey("DepositedIntoId")]
        public virtual Account BankAccount { get; set; }
        public DateTime DepositedDate { get; set; }
    }
    public class CardPaymentNew : PaymentNew
    {
        public long? CAccountId { get; set; }

        [ForeignKey("CAccountId")]
        public virtual Account CAccount { get; set; }
        public string CTransactionNumber { get; set; }
        public DateTime CCTransactionDate { get; set; }
        public string CardType { get; set; } // e.g., "Visa", "MasterCard", etc.
        public string CardHolderName { get; set; }
        public string LastFourDigits { get; set; } // For security, only store last 4 digits
        public string AuthorizationCode { get; set; } // For tracking authorization from card issuer
    }
    public class BankTransferPaymentNew : PaymentNew
    {
        public long? BankTransferId { get; set; }

        [ForeignKey("BankTransferId")]
        public virtual Account BankTransfer { get; set; }

        [MaxLength(20)]
        public string TransactionNumber { get; set; }
    }
    public class UpiTransactionPaymentNew : PaymentNew
    {
        public long? UpiTransactionId { get; set; }

        [ForeignKey("UpiTransactionId")]
        public virtual Account UpiAccount { get; set; }

        public string UpiTransactionNumber { get; set; }
        public DateTime UpiTransactionDate { get; set; }
    }
}
