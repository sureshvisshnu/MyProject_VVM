using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Payment Method, this Model will define all payment type for the every company
 */
namespace fa.model.Accounting.Masters
{
    public class PaymentMethod : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        private string _displayAs;
        [MaxLength(50)]
        public string DisplayAs
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
        public bool CreditCard { get; set; }        
        [Required]
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
       
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
