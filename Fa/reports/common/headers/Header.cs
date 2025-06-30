using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.reports.common.headers
{
    public class Header
    {        
        public Company Company { get; set; }     
        public string Title { get; set; }
        public Dictionary<String,String> map = new Dictionary<string, string>();        
    }
}
