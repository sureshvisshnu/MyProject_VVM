using fa.model;
using fa.model.Accounting.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    public class LicenseInfo: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(20), Required]
        public string Name { get; set; }
        private string _displayAs;
        //If display name is empty then return Name
        [MaxLength(20)]
        public string DisplayName 
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
                if (this.Name!=null &&!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        [Required]
        public bool IncludeInInvoice { get; set; }
        [Required]
        public bool IncludeInReport { get; set; }
    }

    public class CompanyLicence : LicenseInfo
    {
        [Required]
        public string Value { get; set; }
    }

    public class CompanyCustomerLicenseMaster: LicenseInfo
    {
        [Required]
        public bool Required { get; set; }
    }

    public class CompanySupplierLicenseMaster : LicenseInfo
    {
        [Column("Required1")]
        public bool Required { get; set; }
    }

    public class CustomerLicenceDetail: AuditableEntityForCompany
    {
        [Key]
        public long CustomerLicenceId { get; set; }
        [Required]
        public long CompanyCustomerLicenseMasterId { get; set; }       
        [ForeignKey("CompanyCustomerLicenseMasterId ")]
        public virtual CompanyCustomerLicenseMaster CompanyCustomerLicenseMaster { get; set; }
        //if display name is empty then take it from CompanyCustomerLicenseMaster.DisplayName
        [Required]
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer{ get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }

    public class SupplierLicenceDetail : AuditableEntityForCompany
    {
        [Key]
        public long SupplierLicenceId { get; set; }
        [Required]
        public long CompanySupplierLicenseMasterId { get; set; }
        [ForeignKey("CompanySupplierLicenseMasterId ")]
        public virtual CompanySupplierLicenseMaster CompanySupplierLicenseMaster { get; set; }
        //if display name is empty then take it from CompanyCustomerLicenseMaster.DisplayName
        [Required]
        public long SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }

}
