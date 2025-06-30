using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Common
{
    public class State : AuditableEntity
    {
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public string DisplayAs { get; set; }
        public long? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public string Code { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
