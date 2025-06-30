using FADataAccessLibrary.report.sales;
using System.Data;
using Fa.reports.sales;
using fa.report.catalog;
using fa.reports.catalog;
using fa.views.utils.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using fa.views.utils;
using Font = iTextSharp.text.Font;
using fa.report.Inventory;
using fa.api.utils;
using fa;

namespace Fa.views.utils.Report.Sale
{
    class DeliveryReportSavePrint
    {
        public string fileName = string.Empty;

        public bool ExportOrPrintToFile(DeliveryReport DeliveryReport, string ReportName, string fileExtension, bool isPrint)
        {
            if (DeliveryReport != null) 
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            fileName = ReportName;
                            var DataTable = DataGridViewAsDataTableRetail(DeliveryReport);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, DeliveryReport, "pdf", isPrint);
                                break;
                            }
                            else
                                break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            } 
            return true;
        }
        private static readonly Font H1Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, DeliveryReport DeliveryReport, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 40);
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
                    ReportLine1 = DeliveryReport.ReportTitle(),
                    ReportLine2 = DeliveryReport.ReportDate()
                    //ReportLine2 = DeliveryReport.ReportSubTitle() + "\n" + DeliveryReport.ReportDate()
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
                    ReportLine1 = DeliveryReport.ReportTitle(),
                    ReportLine2 = DeliveryReport.ReportDate()
                    // ReportLine2 = DeliveryReport.ReportSubTitle() + "\n" + DeliveryReport.ReportDate()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "DeliveryReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 20f, 55f, 25f, 20f, 20f };
                
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    BaseColor CurRowColor = RowColor[k % 2];
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, H1Font));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;

                        if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Date" || DataTable.Columns[j].ColumnName == "Customer Name" || DataTable.Columns[j].ColumnName == "Payment Type" || DataTable.Columns[j].ColumnName == "Status")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                
                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, fileName == "DeliveryReport" ? PaperTypes.A4_PORTRAIT : PaperTypes.A4_LANDSCAPE);
                Cursor.Current = Cursors.Default;
            }
        }
        readonly String[] DeliveryReportColumn = new String[]
        {
                    "#", "Date", "Customer Name", "Amount", "Payment Type", "Status"
        };
        public DataTable DataGridViewAsDataTableRetail(DeliveryReport DeliveryReport)
        {
            DataTable DeliveryReportTable = new DataTable(); 
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.SNO], typeof(string));
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.DELIVERYDATE], typeof(string));
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.NAME], typeof(string));
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.AMOUNT], typeof(string));
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.PAYMENTTYPE], typeof(string));
            DeliveryReportTable.Columns.Add(DeliveryReportColumn[(int)DeliveryReportTableColumn.STATUS], typeof(string));

            DataRow DeliveryReportTableRow = null;
            int rn = 0;
            foreach (DeliveryReportLineItem LineItem in DeliveryReport.LineItems)
            {
                DeliveryReportTableRow = DeliveryReportTable.NewRow();
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.SNO]] = rn + 1;
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.DELIVERYDATE]] = LineItem.DeliveryDate.Date.ToString(Global.Company.DateFormat);
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.NAME]] = LineItem.CustomerName;
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.AMOUNT]] = Math.Round(LineItem.Amount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.PAYMENTTYPE]] =LineItem.PaymentType;
                DeliveryReportTableRow[DeliveryReportColumn[(int)DeliveryReportTableColumn.STATUS]] = LineItem.Status;
                
                DeliveryReportTable.Rows.Add(DeliveryReportTableRow);
                rn++;
            }
            return DeliveryReportTable;
        }

    }
}
