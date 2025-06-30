using System.ComponentModel.DataAnnotations;

namespace fa.model.Common
{
    public class Keyword : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public string Text{ get; set; }
    }
}
