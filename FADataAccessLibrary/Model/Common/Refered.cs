using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Common
{
    public class Refered: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public String Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
