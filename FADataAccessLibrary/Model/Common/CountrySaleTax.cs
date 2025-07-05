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

        [Column(TypeName = "datetime")] // MySQL compatible
        public DateTime EffectiveFrom { get; set; } = new DateTime(2020, 1, 1);

        [Column(TypeName = "datetime")] // MySQL compatible
        public DateTime EffectiveTo { get; set; } = new DateTime(2400, 1, 1);
    }
}
