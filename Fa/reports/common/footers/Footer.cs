using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.reports.common.footers
{
    public abstract class Footer
    {
        public bool PrintDateTime { get; set; }
        public bool PrintPageNumber { get; set; }
        public string FooterContent {get;set;}
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public iTextSharp.text.Font Font { get; set; }
    }
}
