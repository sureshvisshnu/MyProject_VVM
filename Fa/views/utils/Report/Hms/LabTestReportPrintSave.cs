using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.views.utils.Common;
using fa.views.utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Fa.reports.Hms;
using NPOI.SS.Formula.Functions;
using VisioForge.MediaFramework.DSP;
using fa.api.utils;
using fa;
using fa.report.sales;

namespace Fa.views.utils.Report.Hms
{
    public class LabTestReportPrintSave
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
                            if (col.Caption == "Fees")
                            {
                                double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString());
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "0.00" : Math.Round(temp).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            else if (col.Caption == "Hi Abs" ||col.Caption == "Hi Nor" ||col.Caption == "Hi Cri" ||col.Caption == "Obs Hi"
                                  || col.Caption == "Low Abs" || col.Caption == "Low Nor" || col.Caption == "Low Cri" )
                            {
                                if (row.Cells[col.ColumnName].Value != null)
                                {
                                    if (!double.TryParse(row.Cells[col.ColumnName].Value.ToString(), out double result)) continue;
                                    double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString());
                                    drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "0.0000" : temp.ToString("0.0000");
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
        public void GeneratePDF(DataTable dataTable, string fileName, string heading, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            Cursor.Current = Cursors.WaitCursor;
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -55, -55, 20, 30);
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
                PdfFooter.IsLandScape = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName;
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
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

            if (TypeOfReport == "Lab Test By Date")
            {
                widths = new float[] { 35f, 45f, 45f, 40f, 40f, 30f, 30f, 30f, 30f, 30f, 35f, 30f, 30f, 35f, 30f };
            }
            if (TypeOfReport == "Lab Test By Patient")
            {
                widths = new float[] { 45f, 45f, 35f, 40f, 40f, 30f, 30f, 30f, 30f, 30f, 35f, 30f, 30f, 35f, 30f };
            }
            if (TypeOfReport == "Lab Test By Test Name")
            {
                widths = new float[] { 45f, 35f, 45f, 40f, 40f, 30f, 30f, 30f, 30f, 30f, 35f, 30f, 30f, 35f, 30f };
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
                if (column.Caption == "Hi Abs" || column.Caption == "Hi Nor" || column.Caption == "Hi Cri" || column.Caption == "Obs Hi" || column.Caption == "Fees"
                    || column.Caption == "Low Abs" || column.Caption == "Low Nor" || column.Caption == "Low Cri" || column.Caption == "Obs Low")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
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
                    if (j > 5)
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    if (i == (Rows - 1) || i == (Rows - 2))
                    {
                        if (j < 13)
                        {
                            if (i == Rows - 1)
                            {
                                RowCell.BorderColorTop = BaseColor.WHITE;
                            }
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.BorderColorBottom = BaseColor.WHITE;
                        }
                    }
                    if (DataTable.Columns[j].ColumnName == "ByDateObsLow" && (string)DataTable.Rows[i][j] == "Sub Total"
                        || DataTable.Columns[j].ColumnName == "ByPatientObsLow" && (string)DataTable.Rows[i][j] == "Sub Total"
                        || DataTable.Columns[j].ColumnName == "ByTestObsLow" && (string)DataTable.Rows[i][j] == "Sub Total")
                    {
                        RowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        isSubTotalRow = true;
                    }
                    if (isSubTotalRow)
                    {
                        RowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
    }
}
