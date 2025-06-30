using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Common
{
    public class Currency : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CurrencyId { get; set; }
        [MaxLength(20)]
        public String CurrencyCodeISO { get; set; }
        [MaxLength(50)]
        public String Name { get; set; }
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
        public int RoundingPrecision { get; set; }
        [MaxLength(20)]
        public string CurrencyFormat { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

    }
}
