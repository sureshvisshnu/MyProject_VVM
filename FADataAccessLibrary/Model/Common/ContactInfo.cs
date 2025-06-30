using System.ComponentModel.DataAnnotations;

namespace fa.model.Common
{
    public class ContactInfo : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(15)]
        public string Phone { get; set; }
        [MaxLength(15)]
        public string Mobile { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string WebSite { get; set; }

    }
}
