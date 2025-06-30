using fa.api.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.reports.common.footers
{
    public class PdfFooter : Footer
    {
        public PdfPTable Render()
        {
            if(getNumberOfColumns()>0)
            {
                PdfPTable FooterContentTable = new PdfPTable(getNumberOfColumns());
                if(PrintDateTime)
                {
                    string DateTimeAsString = String.Format("Printed on {0}" + DateUtils.FormatDate(DateTime.Now, Global.Company.DateFormat));
                    PdfPCell cell = new PdfPCell(new Phrase(DateTimeAsString));
                    FooterContentTable.AddCell(cell);
                }
                if (String.IsNullOrEmpty(FooterContent))
                {
                    PdfPCell cell = new PdfPCell(new Phrase(FooterContent));
                    FooterContentTable.AddCell(cell);
                }
                if (PrintPageNumber)
                {
                    string PageNumberContent = String.Format("Page {0}/{1}", CurrentPage, TotalPage);
                    PdfPCell cell = new PdfPCell(new Phrase(PageNumberContent));
                    FooterContentTable.AddCell(cell);
                }
                return FooterContentTable;
            }
            return null;
        }

        public int getNumberOfColumns()
        {
            int TotalCols = 0;
            if (PrintDateTime)
                TotalCols++;
            if (PrintPageNumber)
                TotalCols++;
            if (String.IsNullOrEmpty(FooterContent))
                TotalCols++;
            return TotalCols;
        }
    }
}
