using System;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.api.Accounting;
using fa.report.common;
using System.ComponentModel;
using fa.model.Hms.Master;

namespace fa.model.Accounting.Transaction
{
    public class DayBook : AuditableEntityForCostCenter
    {
        [Key]
        public long Id { get; set; }
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [Required]
        public double Amount { get; set; }
        public DaybookTransactionType TransactionType { get; set; }
        public string ReferenceTrasnactionId { get; set; }
        [DefaultValue("false")]
        public bool LedgerOnly { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        //used for credit and debit
        private Account _DerivedAccountForRef;
        private Account GetAccount()
        {
            if(Account!=null)
            {
                return Account;
            }
            else
            {
                if (_DerivedAccountForRef == null)
                {
                    _DerivedAccountForRef = AccountManager.Instance.GetAccountById((long)AccountId);
                }
                return _DerivedAccountForRef;
            }
        }
        
        public void Credit(double Amount)
        {
            try
            {
                this.Amount = Amount * (long)GetAccount().AccountGroup.CreditMultiplier;
            }
            catch(Exception e)
            {
                throw new Exception("Could not credit the amount",e);
            }
        }

        public void Debit(double Amount)
        {
            try
            {
                this.Amount = Amount * (long)GetAccount().AccountGroup.DebitMultiplier;
            }
            catch(Exception e)
            {
                throw new Exception("Could not debit the amount", e);
            }
        }

        public CrDr CreditOrDebit() 
        {
            return (this.Amount * GetAccount().AccountGroup.DebitMultiplier >= 0 ? CrDr.DR : CrDr.CR);
        }
    }


    public enum DaybookTransactionType
    {
        Purchase=10,
        PurchaseReturn=20,
        Sales=30,
        SalesReturn=40,
        Payment=50,
        Receipt=60,
        Expense=70,
        Invoice = 80,
        Bill = 90,
        Journal = 100,
        Invoice_Hms=110,
        Payment_Hms=120,
        CreditNote=130,
        DebitNote=140
    }
}
