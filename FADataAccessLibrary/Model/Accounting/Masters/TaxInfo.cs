using System.ComponentModel.DataAnnotations;

namespace fa.model.Accounting.Masters
{
    public class TaxInfo : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(20)]
        public string PAN { get; set; }
        [MaxLength(20)]
        public string CST { get; set; }
        [MaxLength(20)]
        public string GST { get; set; }
        [MaxLength(20)]
        public string TIN { get; set; }
        [MaxLength(20)]
        public string TIN1 { get; set; }
    }
}
