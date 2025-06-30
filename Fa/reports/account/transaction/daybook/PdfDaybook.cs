using fa.reports.common.documents;
using fa.reports.common.headers;
using Fa.report.accounting.master;
using iTextSharp.text;

namespace fa.reports.account.transaction.daybook
{
    public class PdfDaybookA4 : PdfDocument
    {
        public RptDayBook Data { get; set; }
        public override Document Render()
        {
            Document DayBookPdf = new Document(PageSize.A4, -30, -30, 30, 10);
            PdfHeader Header = new PdfHeader();
            Header.Company = Data.Company;
            /*
            Header.Title = Data.ReportTitle;

            PdfPTable TableRightContent = new PdfPTable(tableColumnCount);
            PdfPTable HTable = HeaderTable(AllAccountLedger.ReportTitle(), long.Parse(dataTable.Rows[i][5].ToString()));
            pdfDoc.Add(HTable);
            HeadTableHeight = HTable.TotalHeight;*/
            return DayBookPdf;
        }
    }
}
