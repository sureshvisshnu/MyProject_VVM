using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using fa.model.UserProfile;
using fa.model.Common;

namespace fa.model.Employee
{
    public class Employee : Account
    {
        public Employee()
        {
            this.AccountGroupId = 602;
            this.AccountType = AccountType.EMPLOYEE;
        }
        public long? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public long? TitleId { get; set; }
        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }
        public long? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public long? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public ContactInfo ContactInfo { get; set; }
        public long? TaxInfoId { get; set; }
        [ForeignKey("TaxInfoId")]
        public TaxInfo TaxInfo { get; set; }
        public DateTime? DateOfBirth {get;set;}
        public override string ToString()
        {
            return Name;
        }
        public byte[] EmployeePhoto { get; set; }
        public bool IsServiceProvider {  get; set; }
        public bool IsRxSymbolDisplayOn { get; set; }
        public bool IsRSignatureDisplayOn { get; set; }
        public byte[] DigitalSignature { get; set; }
    }
    public class Department : AuditableEntityForCompany
    {
        private string _displayAs;
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
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
        [MaxLength(250)]
        public string Discription { get; set; }
        public bool IsSubDepartment { get; set; }
        public long? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }       
        public override string ToString()
        {
            return Name;
        }
    }

    public class Title : AuditableEntityForCompany
    {
        private string _displayAs;
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
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
        [MaxLength(250)]
        public string Discription { get; set; }      
        public override string ToString()
        {
            return Name;
        }
    }
}
