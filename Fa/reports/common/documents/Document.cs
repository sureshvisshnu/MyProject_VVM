using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.reports.common.documents
{
    public class AB2Document
    {
        public Company Company { get; set; }
        public string Title { get; set; }
        public String[][] AdditionalHeaderData { get; set; }
        public bool PrintDateTime { get; set; }
        public bool PrintPageNumber { get; set; }
        public string FooterContent { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
