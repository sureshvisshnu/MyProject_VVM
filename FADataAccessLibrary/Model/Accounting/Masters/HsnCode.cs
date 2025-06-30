using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.model.Accounting.Masters
{
    public class HsnCode
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(14)]
        public string Code { get; set; }
        
    }
}
