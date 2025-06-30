using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    public class CostCenter : AuditableEntity
    {
        [Key]
        public long CostCenterId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        private string _displayAs;
        [MaxLength(50)]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return this.Name;
                }
                else
                    return _displayAs;
            }
            set
            {
                if (!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        [MaxLength(250)]
        public string Description { get; set; }
        public long ParentCompanyId { get; set; }
        [ForeignKey("ParentCompanyId")]
        public Company ParentCompany { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
