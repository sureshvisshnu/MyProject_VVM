using FADataAccessLibrary.report.Hms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.views.utils;
using fa.views.utils.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static FADataAccessLibrary.report.Hms.RptFeeCollection;
using Font = iTextSharp.text.Font;
using fa.api.utils;
using Fa.report.accounting.master;
using fa.api.Accounting;
using System.Reflection.Metadata;
using fa.reports.account.transaction.trialbalance;
using Document = iTextSharp.text.Document;
using Syncfusion.Styles;

namespace Fa.views.utils.Report.Account
{
    public class TrialBalanceReportSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, DateTime FromDate, DateTime ToDate, string fileExtension, bool isPrint)
        {
            if (ReportGridView.Rows.Count == 0)
            {
                MessageBox.Show("No data to export or print.");
                return false;
            }
            try
            {
                switch (fileExtension.ToLower())
                {
                    case "xls":
                        break;
                    case "pdf":
                        DataTable DataTable = DataGridViewAsDataTable(ReportGridView);
                        if (DataTable != null)
                        {
                            GeneratePDF(
                                DataTable,
                                fileExtension, 
                                isPrint,
                                FromDate,
                                ToDate                      
                            );
                        }
                        break;

                    default:
                        MessageBox.Show("Unsupported file extension.");
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("File Error. Please contact system admin.");
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
        enum TrialBalanceReportTableColumn
        {
            ACCOUNT_NAME, DEBIT, CREDIT,  ID
        }
        readonly String[] TrialBalanceDataTableColumn = new String[]
        {
            "AccountName", "Debit", "Credit", "AID"
        };

        public DataTable DataGridViewAsDataTable(DataGridView ReportGridView)
        {
            DataTable TrialBalanceTable = new DataTable();
            if (ReportGridView.Rows.Count > 0)            {
                
                TrialBalanceTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.ACCOUNT_NAME], typeof(string));
                TrialBalanceTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.DEBIT], typeof(string));
                TrialBalanceTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.CREDIT], typeof(string));
                DataRow TrialBalanceTableRow = null!;
                foreach (DataGridViewRow dataRow in ReportGridView.Rows)
                {
                    TrialBalanceTableRow = TrialBalanceTable.Rows.Add();
                    TrialBalanceTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.ACCOUNT_NAME]] = dataRow.Cells[0].Value;
                    TrialBalanceTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.DEBIT]] = dataRow.Cells[1].Value;
                    TrialBalanceTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceReportTableColumn.CREDIT]] = dataRow.Cells[2].Value;
                }
            }
            return TrialBalanceTable;
        }
        public string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Global.Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Trial Balance Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public string ReportDate(DateTime FromDate, DateTime ToDate )
        {           
            return String.Format("As On - {1}", DateUtils.FormatDate(FromDate, Global.Company.DateFormat), DateUtils.FormatDate(ToDate, Global.Company.DateFormat));
        }
        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));
        private static readonly Font FNIBLD7Font = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_7_Black"));
        public void GeneratePDF(DataTable DataTable, string fileExtension, bool isPrint, DateTime fromDate, DateTime toDate)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = "Trial Balance Report",
                    ReportLine2 = ReportDate(fromDate, toDate)
                };
                PdfPTable HTable = PdfHeader.PageHeader();
                PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = false,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = "Trial Balance Report",
                    ReportLine2 = ReportDate(fromDate, toDate)
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable;
                MTable = PdfTableHeader.ReportTableHeader(DataTable, "TrialBalanceReport");

                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;

                widths = new float[] { 50f, 25f, 25f };

                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                string previousValue = string.Empty;

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MiniHTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        previousValue = string.Empty;
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, FNIB7Font));
                        if (Temp != null && (Temp.Equals("N/A", StringComparison.OrdinalIgnoreCase) || Temp.Equals("n/a", StringComparison.OrdinalIgnoreCase)))
                        {
                            RowCell = new PdfPCell(new Phrase("", FNIB7Font));
                        }
                        if (Temp != null && (Temp.Equals("Total", StringComparison.OrdinalIgnoreCase) || Temp.Equals("n/a", StringComparison.OrdinalIgnoreCase)))
                        {
                            previousValue = Temp;
                            RowCell = new PdfPCell(new Phrase(Temp, FNIBLD7Font));
                            RowCell.BorderColor = new BaseColor(160, 160, 160);
                            RowCell.BorderWidth = 0.25f;
                            RowCell.BackgroundColor = CurRowColor;
                        }
                        if ((j == 1 || j == 2)  && previousValue == "Total"  )
                        {
                            RowCell = new PdfPCell(new Phrase(Temp, FNIBLD7Font));
                        }
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BorderWidth = 0.25f;
                        RowCell.BackgroundColor = CurRowColor;
                        if (j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "Debit")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        
                        if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][2].ToString()))
                        {
                            RowCell = new PdfPCell(new Phrase(Temp, FNIBLD7Font));
                            RowCell.BorderColor = new BaseColor(160, 160, 160);
                            RowCell.BorderWidth = 0.25f;
                            RowCell.BackgroundColor = CurRowColor;
                            RowCell.Colspan = DataTable.Columns.Count;

                        }
                        if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][2].ToString()))
                        {
                            continue;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
