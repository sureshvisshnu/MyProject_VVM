using fa.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.report.catalog
{
    public abstract class TaxCodeReport: Report
    {
    }
    public class AllTaxCodeReport : TaxCodeReport
    {
        IList<TaxCodeReportLineItems> _LineItems = new List<TaxCodeReportLineItems>();
        public IList<TaxCodeReportLineItems> LineItems
        {
            get
            {
                return _LineItems;
            }
        }
        public override string ReportName()
        {
            return null;
        }
        public override string ReportTitle()
        {
            return null;
        }
        public override void GenerateReport()
        {
        }
    }
    public class TaxCodeReportLineItems
    {
        public String Code { get; set; }
        public String Description { get; set; }
        
    }
}
