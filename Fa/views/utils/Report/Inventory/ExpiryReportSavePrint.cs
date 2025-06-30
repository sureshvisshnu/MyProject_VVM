using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.utils.Common;
using fa.views.utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fa.reports.Inventory;
using fa.api.utils;
using fa;

namespace Fa.views.utils.Report.Inventory
{
    class ExpiryReportSavePrint
    {
        public bool ExportOrPrintToFile(ReportStock ReportStock, string ReportName, string fileExtension, bool isPrint)
        {
            if (ReportStock != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewDataTable(ReportStock);
                            if (DataTable != null)
                            {
                                GeneratePDFStocReport(DataTable, ReportStock, "pdf", isPrint);
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
        
        static readonly String[] ExpiryListColumn = new String[]
        {
            "#", "Code", "Name", "UOM", "Batch", "Exp Date",  "Quantity"
        };
        public static DataTable DataGridViewDataTable(ReportStock ReportStock)
        {
            DataTable StockListTable = new DataTable();
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.SNO], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.MID], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.PNAME], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.UOM], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.BAT], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.EXPDATE], typeof(string));
            StockListTable.Columns.Add(ExpiryListColumn[(int)ExpiryReportTableColumn.CLSTK], typeof(string));
            
            DataRow PriceListTableRow = null;
            int rn = 0;
            string Loaction = string.Empty;
            foreach (StockLedger LineItem in ReportStock.StockLedger)
            {
                if (Loaction != LineItem.Location.Name && !ReportStock.IsAllLocation)
                {
                    rn = 0;
                    PriceListTableRow = StockListTable.NewRow();
                    PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.SNO]] = LineItem.Location.Name;
                    StockListTable.Rows.Add(PriceListTableRow);
                    Loaction = LineItem.Location.Name;
                }
                if (LineItem.Batch && LineItem.OpeningStockforBatch.Count > 0)
                {
                    foreach (BatchOpeningStock BatchWise in LineItem.OpeningStockforBatch.Where(x => x.ClosingStock > 0))
                    {
                        PriceListTableRow = StockListTable.NewRow();
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.SNO]] = rn + 1;
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.MID]] = LineItem.MaterialId;
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.PNAME]] = LineItem.Name;
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.UOM]] = LineItem.uom;
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.BAT]] = BatchWise.BatchNo ;
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.EXPDATE]] = BatchWise.ExpDate.ToString(Global.Company.DateFormat);
                        PriceListTableRow[ExpiryListColumn[(int)ExpiryReportTableColumn.CLSTK]] = BatchWise.ClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockListTable.Rows.Add(PriceListTableRow);
                        rn++;
                    }
                }             
            }
            return StockListTable;
        }
        public void GeneratePDFStocReport(DataTable DataTable, ReportStock ReportStock, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 540;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    PaperTypes = PaperTypes.A4_PORTRAIT,
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = ReportStock.ReportTitle(),
                    ReportLine2 = ReportStock.ReportHeader + ReportStock.ReportDate()
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
                    ReportLine1 = ReportStock.ReportTitle(),
                    ReportLine2 = ReportStock.ReportHeader + ReportStock.ReportDate()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "ExpiryReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 25f, 50f, 20f, 20f, 20f, 20f};

                ReportMainTable.SetWidths(widths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;

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
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Code" || DataTable.Columns[j].ColumnName == "Name" || DataTable.Columns[j].ColumnName == "UOM" || DataTable.Columns[j].ColumnName == "Batch" || DataTable.Columns[j].ColumnName == "Exp Date")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                        {
                            RowCell.Colspan = 15;
                        }
                        if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                        {
                            continue;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                Cursor.Current = Cursors.Default;

                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.IsLandScape = false;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = ReportStock.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
