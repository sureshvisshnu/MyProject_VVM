using fa.model.Common;
using fa.model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.OrderManagement;
//using FADataAccessLibrary.Migrations;
using FADataAccessLibrary.Model.Common;

namespace fa.model.Accounting.Masters
{
    public class Company : AuditableEntity
    {
        public Company()
        {
            CostCenters = new List<CostCenter>();
        }
        [Key]
        public long CompanyId { get; set; }
        [MaxLength(100)]
        public String Name { get; set; }
        private string _displayAs;
        [MaxLength(100)]
        public string DisplayAs
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return this.Name;
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
        public long? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public Boolean IsBranch { get; set; }
        public long? ParentCompanyId { get; set; }
        [ForeignKey("ParentCompanyId")]
        public virtual Company ParentCompany { get; set; }
        public long? CompanyTypeId { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public long? AccountingMethodId { get; set; }
        public virtual AccountingMethod AccountingMethod { get; set; }
        public long? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public long? ContactInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public long? TaxInfoId { get; set; }
        public TaxInfo TaxInfo { get; set; }
        public int AccountingStartDate { get; set; }
        public int IncomeTaxStartDate { get; set; }
        [MaxLength(10)]
        public String DateFormat { get; set; }
        public String PhoneFormat { get; set; }
        public String MobileFormat { get; set; }
        public ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();
        public long? PrimaryCurrencyId { get; set; }
        [ForeignKey("PrimaryCurrencyId")]
        public virtual Currency PrimaryCurrency { get; set; }
        public String Slogan { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public ICollection<CompanyFinancialPeriods> CompanyFinancialPeriods { get; set; } = new List<CompanyFinancialPeriods>();
        public byte[] Logo { get; set; }        
        public long? CashOnHandAccountId { get; set; }
        [ForeignKey("CashOnHandAccountId")]
        public Account CashOnHandAccount { get; set; }
        public long? UndepositedFundAccountId { get; set; }
        [ForeignKey("UndepositedFundAccountId")]
        public Account UndepositedFundAccount { get; set; }
        public long? PurchaseAccountId { get; set; }
        [ForeignKey("PurchaseAccountId")]
        public Account PurchaseAccount { get; set; }
        public long? SalesAccountId { get; set; }
        [ForeignKey("SalesAccountId")]
        public Account SalesAccount { get; set; }
        public long? SalesReturnFeeAccountId { get; set; }
        [ForeignKey("SalesReturnFeeAccountId")]
        public Account SalesReturnFeeAccount { get; set; }
        public long? AccountRecivableId { get; set; }
        [ForeignKey("AccountRecivableId")]
        public Account AccountRecivable { get; set; }
        public long? AccountPayableId { get; set; }
        [ForeignKey("AccountPayableId")]
        public Account AccountPayable { get; set; }
        public long? IncomceAccountId { get; set; }
        [ForeignKey("IncomceAccountId")]
        public Account IncomceAccount { get; set; }
        public long? ExpenseAccountId { get; set; }
        [ForeignKey("ExpenseAccountId")]
        public Account ExpenseAccount { get; set; }
        public long? RoundOffAccountId { get; set; }
        [ForeignKey("RoundOffAccountId")]
        public Account RoundOffAccount { get; set; }
        public ICollection<CompanySalesTaxAccountMap> SalesTaxAccountMaps { get; set; } = new List<CompanySalesTaxAccountMap>();
        public bool HasProductCatalog { get; set; }
        public int QuantityPricision { get; set; }
        public long? CompanyPurchaseSetupId { get; set; }
        [ForeignKey("CompanyPurchaseSetupId")]
        public virtual CompanyPurchaseSetup CompanyPurchaseSetup { get; set; }
        public long? CompanySalesSetupId { get; set; }
        [ForeignKey("CompanySalesSetupId")]
        public virtual CompanySalesSetup CompanySalesSetup { get; set; }
        public long? CompanyStockMovementSetupId { get; set; }
        [ForeignKey("CompanyStockMovementSetupId")]
        public virtual CompanyStockMovementSetup CompanyStockMovementSetup { get; set; }
        public ICollection<CompanyLicence> CompanyLicence { get; set; } = new List<CompanyLicence>();
        public ICollection<CompanyCustomerLicenseMaster> CompanyCustomerLicenseMaster { get; set; } = new List<CompanyCustomerLicenseMaster>();
        public ICollection<CompanySupplierLicenseMaster> CompanySupplierLicenseMaster { get; set; } = new List<CompanySupplierLicenseMaster>();
        public String DayBookType { get; set; }
        public BuisnessType BusinessType { get; set; }
        public bool AllowWholSale { get; set; }
        public List<Narration> Narration { get; set; } = new List<Narration>();
        public List<IdSpace> IdSpaces { get; set; } = new List<IdSpace>();
        public long? PatientPurchaseAccountId { get; set; }
        [ForeignKey("PatientPurchaseAccountId")]
        public Customer PatientPurchaseAccount { get; set; }
        public long? StateId { get; set; }
        public virtual State State { get; set; }
        public bool MaintainRackNumber { get; set; }
    }
    public class Narration
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
    public class CompanyFinancialPeriods : AuditableEntityForCompany
    {
        [Key]
        public long PeriodsId { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Closed { get; set; }
    }
    public class CompanySalesTaxAccountMap : AuditableEntityForCompany
    {
        [Key]
        public long MapId { get; set; }
        public string Name { get; set; }
        public long AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public long? CountrySaleTaxId { get; set; }
        [ForeignKey("CountrySaleTaxId")]
        public CountrySaleTax CountrySaleTax { get; set; }
    }
    
    
    public class CompanyAdditionalTransactionSetup : AuditableEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public AdditionalTransactionType Type { get; set; }
        public AdditionalTransactionAction TransactionAction { get; set; }
        public virtual ICollection<CompanyPurchaseSetup> CompanyPurchaseSetup { get; set; }
        public virtual ICollection<CompanySalesSetup> CompanySalesSetup { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public enum AdditionalTransactionType
    {
        PERCENT, VALUE
    }
    public enum AdditionalTransactionAction
    {
        CR, DR
    }    
    public enum EntryType
    {
        INVOICE=0, 
        BILL=1,
        RECEIPT=2,
        PAYMENT=3,
        CREDIT_NOTE=4,
        DEBIT_NOTE=5,
        EXPENSE=6,
        JOURNAL=7,
        PURCHASE=8,
        PURCHASE_RETURN=9,
        PURCHASE_ORDER=10,
        SALES=11,
        SALES_QUOTE=12,
        SALES_RETURN=13,
        STOCK_IN=14,
        STOCK_OUT=15,
        PATIENT_FEE_RECEIPT=16,
        OP_TOKEN=17,
        PRESCRIPTION=18,
        PATIENT_INVOICE = 19,
        STOCK_OPENING = 20,
        STOCK_PURCHASE = 21,
        STOCK_SALE = 22,
        STOCK_PURCHASERETURN = 23,
        STOCK_SALERETURN = 24,
        STOCK_ADJUSTMENT = 25,
        STOCK_DAMAGE = 26,
        STOCK_TOPATIENT = 27,
        STOCK_REQUEST = 28,
        PATIENT_ID = 29,
        OP_ID = 30,
        IP_ID = 31
    }
    public class CompanySalesSetup : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public List<CompanyAdditionalTransactionSetup> AdditionalTransactions { get; set; }       
        public bool IncludingTax { get; set; }
        public SaleMethod DefaultSalesType { get; set; }
        public bool CombineItem { get; set; }
        public bool IsReceivePayment { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsNegativeStockAllowed { get; set; }
        public bool IsBankDetailDisplayOnInvoice { get; set; }
        public bool IsDeclarationDisplayOnInvoice { get; set; }
        public String BankDetails { get; set; }
        public String Declarations { get; set; }
        public PriceType PriceType { get; set; }
        public SaleTaxType SaleTaxType { get; set; }
        public bool IsPrintQRCode { get; set; }
        public String UPIId { get; set; }

    }
    public class CompanyPurchaseSetup : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public List<CompanyAdditionalTransactionSetup> AdditionalTransactions { get; set; }
    }
    public class CompanyStockMovementSetup : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
    }
    public enum BuisnessType
    {
        Retail,Wholesale,Professionals,Hospital,Pharmacy
    }
    public enum PriceType
    {
        Retail, Wholesale, MaxRetailPrice
    }
    public enum SaleTaxType
    {
        INTER=0, INTRA=1
    }
    public enum SoftwareType
    {
        VVMATRIX, MEDICARE
    }
}
