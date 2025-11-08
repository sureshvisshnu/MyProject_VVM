using fa.api.utils;
using fa.model.Common;
using fa.report.catalog;
using fa.views.utils.Common;
using Fa.report.catalog;
using FADataAccessLibrary.report.sales;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SkiaSharp;
using System;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Report.Catalog
{
    class SavePrintCatalogItemReport
    {
        public bool ExportToFileOrPrint(DataGridView ReportGridView, AllItemsReport allItemsReport, string ReportHeading, string ReportName, string fileExtension, bool isPrint)
        {
            if (ReportGridView.Rows.Count != 0)
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
                                GeneratePDF(DataTable, allItemsReport, ReportHeading, ReportName, fileExtension, isPrint);
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
                        dt.Columns[col.Name]!.Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return null!;
                    foreach (DataGridViewRow row in ReportGridView.Rows)
                    {
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;
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

        public void GeneratePDF(DataTable dataTable, AllItemsReport allItemsReport, string heading, string fileName, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                string userPrinter = string.Empty;
                if (isPrint)
                {
                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        userPrinter = printDialog.PrinterSettings.PrinterName;
                        if (!PdfPrinter.IsOnline(userPrinter))
                        {
                            MessageBox.Show("Printer " + userPrinter + " is offline");
                            return;
                        }
                        else
                        {
                            if (userPrinter == "Microsoft Print to PDF")
                            {
                                
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                Document pdfDoc;
                if (dataTable.Columns.Count > 13)
                {
                    pdfDoc = new Document(PageSize.A2, -45, -45, 20, 40);
                }
                else if (dataTable.Columns.Count > 9)
                {
                    pdfDoc = new Document(PageSize.A3, -45, -45, 20, 40);
                }
                else
                {
                    pdfDoc = new Document(PageSize.A4, -45, -45, 20, 40);
                }
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
                    ReportLine1 = allItemsReport.ReportTitle(),
                };
                PdfPTable HTable = PdfHeader.PageHeader();
                pdfDoc.Add(HTable);

                PdfPTable ReportMainTable = MainTable(dataTable, fileName);
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                if (dataTable.Columns.Count > 13)
                {
                    PdfGeneration.SaveMemoryStreams(myMemoryStream, fileName, userPrinter, fileExtension, isPrint, PaperTypes.A2);
                }
                else if (dataTable.Columns.Count > 9)
                {
                    PdfGeneration.SaveMemoryStreams(myMemoryStream, fileName, userPrinter, fileExtension, isPrint, PaperTypes.A3);
                }
                else
                {
                    PdfGeneration.SaveMemoryStreams(myMemoryStream, fileName, userPrinter, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
                }
            }

        }
        private PdfPTable MainTable(DataTable DataTable, string TypeOfReport)
        {
            Font Font_Bold_Italic_10_White = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_10_White"));
            Font Font_Normal_Italic_9_Black = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"));

            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] Defaultwidths = new float[] { 5f, 10f, 33f, 10f, 9f, 10f, 9f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 8f, 8f, 8f };
            float[] widths = new float[Cols];
            for (int i = 0; i < Cols; i++)
            {
                widths[i] = Defaultwidths[i];
            }

            ReportMainTable.SetWidths(widths);

            // Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_10_White));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 25;
                HeaderCell.Padding = 4;
                if (column.Caption == "") { continue; }
                if (column.Caption == "#" || column.Caption == "Product Code" || column.Caption == "Product Name" || column.Caption == "P. UOM"
                    || column.Caption == "R. UOM" || column.Caption == "W. UOM")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }

                ReportMainTable.AddCell(HeaderCell);
            }

            // Main Table body
            PdfPCell RowCell = new PdfPCell();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_9_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 20;
                    RowCell.Padding = 2;
                    if(DataTable.Columns.Count == 17)
                    {
                        var cellValue = DataTable.Rows[i][16];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 16)
                    {
                        var cellValue = DataTable.Rows[i][15];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 15)
                    {
                        var cellValue = DataTable.Rows[i][14];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 14)
                    {
                        var cellValue = DataTable.Rows[i][13];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 13)
                    {
                        var cellValue = DataTable.Rows[i][12];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 12)
                    {
                        var cellValue = DataTable.Rows[i][11];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 11)
                    {
                        var cellValue = DataTable.Rows[i][10];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 10)
                    {
                        var cellValue = DataTable.Rows[i][9];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 9)
                    {
                        var cellValue = DataTable.Rows[i][8];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 8)
                    {
                        var cellValue = DataTable.Rows[i][7];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 7)
                    {
                        var cellValue = DataTable.Rows[i][6];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 6)
                    {
                        var cellValue = DataTable.Rows[i][5];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 5)
                    {
                        var cellValue = DataTable.Rows[i][4];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 4)
                    {
                        var cellValue = DataTable.Rows[i][3];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (DataTable.Columns.Count == 3)
                    {
                        var cellValue = DataTable.Rows[i][2];
                        if (j == 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString())))
                        {
                            continue;
                        }
                    }
                    if (j == 0 || j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "PurchaseUOM" || DataTable.Columns[j].ColumnName == "RetailUOM" || DataTable.Columns[j].ColumnName == "WholeSaleUOM")
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
    }
}
