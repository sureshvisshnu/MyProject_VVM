using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    public class AccountingMethod : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AccountingMethodId { get; set; }
        [Required, MaxLength(30)]
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
