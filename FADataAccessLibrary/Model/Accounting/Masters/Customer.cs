using fa.model.Common;
using Fa.model.Accounting.Masters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    [Table("Customers")]
    public class Customer : Account
    {
        public Customer()
        {
            this.AccountGroupId = 101;
        }
        public long? BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; }
        public long? ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public virtual Address ShippingAddress {get;set;}
        public long? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public virtual ContactInfo ContactInfo { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
        [MaxLength(50)]
        public string DisplayNameOnCheck { get; set; }
        public long? PaymentMethodId { get; set; }
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        public long? PaymentTermId { get; set; }
        [ForeignKey("PaymentTermId")]
        public virtual PaymentTerm PaymentTerm { get; set; }
        public ICollection<CustomerLicenceDetail> CustomerLicenceDetail { get; set; }=new List<CustomerLicenceDetail>();
        public double? PaymentLimit {  get; set; }
        public bool LockBill {  get; set; }
        [MaxLength(20)]
        public string GSTNo { get; set; }
        public bool BillWithPreviousPrice { get; set; }
    }
}
