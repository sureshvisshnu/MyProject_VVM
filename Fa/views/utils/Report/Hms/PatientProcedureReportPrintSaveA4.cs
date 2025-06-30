using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.api.utils;
using fa.views.utils.Common;
using fa.views.utils;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Fa.views.utils.Report.Hms
{
    public class PatientProcedureReportPrintSaveA4
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, string ReportName, string ReportHeading, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                                GeneratePDF(DataTable, ReportName, ReportHeading, fileExtension, isPrint, FromDate, Todate);
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
        public static DataTable DataGridViewAsDataTable(DataGridView ReportGridView)
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
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;
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
        public void GeneratePDF(DataTable dataTable, string fileName, string heading, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            Cursor.Current = Cursors.WaitCursor;
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 30);
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
                PdfPTable ReportMainTable = MainTable(dataTable, heading);
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

            if (TypeOfReport == "Patient Procedure Report")
            {
                widths = new float[] { 20f, 30f, 40f, 40f, 40f, 40f, 40f, 40f };
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
                ReportMainTable.AddCell(HeaderCell);
            }
            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                bool isSubTotalRow = false;
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_8_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 14;
                    RowCell.Padding = 2;
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
    }
}
