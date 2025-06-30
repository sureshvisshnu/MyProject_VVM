using fa.model.Accounting.Masters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Common
{
    public class Country : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string ISOCode2 { get; set; }
        public string ISOCode3 { get; set; }
        public long? DefaultCurrencyId { get; set; }
        [ForeignKey("DefaultCurrencyId")]
        public virtual Currency Currency { get; set; }
        public string DefaultDateFormat { get; set; }
        public string DefaultPhoneFormat { get; set; }
        public string DefaultMobileFormat { get; set; }
        public bool Active { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
        public virtual ICollection<TaxDocumentType> TaxType { get; set; }
        public long? AccountingMethodId { get; set; }
        public virtual AccountingMethod AccountingMethod { get; set; }
        public long? CompanyTypeId { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public int AccountingStartDate { get; set; }
        public int IncomeTaxStartDate { get; set; }
    }
}
