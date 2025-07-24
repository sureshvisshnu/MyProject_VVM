using fa.model.Catalog;
using fa.model.Common;
using Fa.model.Accounting.Masters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    [Table("Suppliers")]
    public class Supplier:Account
    {
        public Supplier()
        {
            this.AccountGroupId = 601;
        }
        public long? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public long? ContactInfoId { get; set; }
        [ForeignKey("ContactInfoId")]
        public ContactInfo ContactInfo { get; set; }
        public long? TaxInfoId { get; set; }
        [ForeignKey("TaxInfoId")]
        public TaxInfo TaxInfo { get; set; }
        public ICollection<SupplierLicenceDetail> SupplierLicenceDetail { get; set; } = new List<SupplierLicenceDetail>();
        public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();

    }

    [Table("SupplierProducts")]
    public class SupplierProduct
    {
        public long Id { get; set; }

        public long SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }  // Ensure this `Product` model exists
    }

}
