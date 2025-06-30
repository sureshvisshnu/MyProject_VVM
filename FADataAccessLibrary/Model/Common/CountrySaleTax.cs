using fa.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Common;
using fa.model.Hms.Master;
using fa.model.Accounting.Masters;

namespace FADataAccessLibrary.Model.Common
{
    public class CountrySaleTax : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Rule { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        
    }
}
