using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.api.utils;
using fa.views.utils.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using fa.views.utils;
using fa.reports.Purchase;

namespace Fa.views.utils.Report.Purchase
{
    public class PurchaseGstrReportPrintA4
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            if (ReportGridView.Rows.Count > 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(ReportGridView);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        public DataTable DataGridViewAsDataTable(DataGridView ReportGridView)
        {
            DataTable dt = new DataTable();
            if (ReportGridView.Rows.Count != 0)
            {
                try
                {
                    if (ReportGridView.ColumnCount == 0) return null;
                    foreach (DataGridViewColumn col in ReportGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        dt.Columns.Add(col.Name, typeof(string));
                        dt.Columns[col.Name].Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return null;
                    foreach (DataGridViewRow row in ReportGridView.Rows)
                    {
                        int i = 0;
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.Caption == "State Tax Amount" || col.Caption == "Tax Amount" || col.Caption == "Central Tax Amount"
                                || col.Caption == "Total Value" || col.Caption == "Total Quantity"
                                || col.Caption == "Taxable Value" || col.Caption == "Value")
                            {
                                if (row.Cells[col.ColumnName].Value != null)
                                {
                                    double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString());
                                    drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : temp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                }
                                else
                                {
                                    drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;
                                }
                            }
                            else
                            {
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;
                            }
                            i++;
                        }
                        dt.Rows.Add(drNewRow);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
            return dt;
        }
        public void GeneratePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            Cursor.Current = Cursors.WaitCursor;
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -45, -45, 20, 30);
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();
                pdfDoc.Add(HTable);
                PdfPTable ReportMainTable = MainTable(dataTable, fileName);
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.IsLandScape = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration pdfGeneration = new PdfGeneration();
                pdfGeneration.IsPrint = isPrint;
                pdfGeneration.FileName = fileName;
                pdfGeneration.PdfFile = PdfFileWithFooter;
                pdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        private PdfPTable MainTable(DataTable DataTable, String TypeOfReport)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_9_White = FontFactory.GetFont(FONT, 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null;
            
            if (TypeOfReport == "PurchaseGSTRReportB2B")
            {
                widths = new float[] { 20f, 30f, 50f, 30f, 35f, 30f, 60f, 35f, 30f, 30f, 30f, 30f, 30f };
            }
            
            ReportMainTable.SetWidths(widths);

            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_9_White));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 14;
                HeaderCell.Padding = 4;
                if (column.Caption == "Total Quantity" || column.Caption == "Total Value"
                    || column.Caption == "Central Tax Amount" || column.Caption == "State Tax Amount" || column.Caption == "Tax Amount"
                    || column.Caption == "Taxable Value" || column.Caption == "Rate" || column.Caption == "Value"|| column.Caption == "Rev. Charge")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                ReportMainTable.AddCell(HeaderCell);
            }

            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_8_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 14;
                    RowCell.Padding = 2;
                    
                    if (TypeOfReport == "PurchaseGSTRReportB2B")
                    {
                        if (j == 5 || j == 10 || j == 11 || j == 12)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }

            return ReportMainTable;
        }
    }
}
