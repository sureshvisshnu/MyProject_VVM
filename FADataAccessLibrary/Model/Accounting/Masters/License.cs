using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;

namespace fa.model.accounting.masters
{
    //public class License
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public string LicenseName { get; set; }
    //    public string LicenseValue { get; set; }
    //    public bool DisplayInHeader { get; set; }
    //    public bool DisplayInShortHeader { get; set; }
    //}

    //public class CompanyLicense : AuditableEntity
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public List<License> Licenses { get; set; }
    //    public long? CompanyId { get; set; }
    //    [ForeignKey("CompanyId")]
    //    public Company Company { get; set; }
    //}


    //public class SupplierLicense  : AuditableEntity
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public List<License> Licenses { get; set; } 
    //    public long? SupplierId { get; set; }
    //    [ForeignKey("SupplierId")]
    //    public Supplier Supplier { get; set; }
    //}

    //public class CustomerLicense : AuditableEntity
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public List<License> Licenses { get; set; }
    //    public long? CustomerId { get; set; }
    //    [ForeignKey("CustomerId")]
    //    public Customer Customer { get; set; }
    //}

}
