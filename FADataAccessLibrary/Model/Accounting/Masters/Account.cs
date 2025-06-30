using fa.api.Accounting;
using fa.report.common;
using System;
using System.ComponentModel.DataAnnotations;

namespace fa.model.Accounting.Masters
{
    public class Account:AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        private string _displayAs;
        [MaxLength(100)]

        public string DisplayAs
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return Name;
                }
                else
                    return _displayAs;
            }

            set
            {
                if (!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        [MaxLength(250)]
        public string Discription { get; set; }
        public bool IsSubAccount { get; set; }
        public long? ParentAccountId { get; set; }
        public virtual Account ParentAccount { get; set; }        
        public long? AccountGroupId { get; set; }
        public virtual AccountGroup AccountGroup { get; set; }
        public double Balance { get; set; }
        public DateTime BalanceAsOf { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public AccountType AccountType { get; set; }

        //used for credit and debit
        private AccountGroup _DerivedAccountGroupForRef;
        private AccountGroup GetAccountGroup()
        {
            if (AccountGroup != null)
            {
                return AccountGroup;
            }
            else
            {
                if (_DerivedAccountGroupForRef == null)
                {
                    _DerivedAccountGroupForRef = AccountGroupManager.Instance.GetAccountGroupById((long)AccountGroupId);
                }
                return _DerivedAccountGroupForRef;
            }
        }


        public void Credit(float Amount)
        {
            try
            {
                this.Balance = Amount * GetAccountGroup().AccountGroupClassification.CreditMultiplier;
            }
            catch (Exception e)
            {
                throw new Exception("Could not credit the amount", e);
            }
        }

        public void Debit(float Amount)
        {           
            try
            {
                this.Balance = Amount * GetAccountGroup().AccountGroupClassification.DebitMultiplier;
            }
            catch (Exception e)
            {
                throw new Exception("Could not debit the amount", e);
            }        
        }

        public CrDr BalanceType()
        {
            return (Balance * GetAccountGroup().AccountGroupClassification.DebitMultiplier >=0 ? CrDr.DR : CrDr.CR);
        }

    }

    public enum AccountType
    {
        ACCOUNT, CUSTOMER, SUPPLIER, EMPLOYEE
    }

}

