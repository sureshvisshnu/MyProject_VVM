using fa.model;
using fa.model.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.Model.Accounting.Masters
{
    public class TaxDocumentType : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TaxTypeId { get; set; }
        [MaxLength(3)]
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
        public virtual ICollection<Country> Country { get; set; }

    }
}
