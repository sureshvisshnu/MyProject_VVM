using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.reports.common.headers;

namespace fa.reports.common.documents
{
    public abstract class PdfDocument:AB2Document
    {
        public iTextSharp.text.PageSize PageSize { get; set; }
        public Header Header { get; set; }
        public Header Footer { get; set; }        
        public abstract iTextSharp.text.Document Render();
    }
}
