using fa.api.utils;
using fa.model.Common;
using fa.report.catalog;
using fa.reports.catalog;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.Formula.Functions;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Windows.Forms;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Report.Inventory
{
    class PriceListSavePrint
    {
        // PriceReport = null;
        public bool ExportOrPrintToFile(PriceReport PriceReport, string ReportName, string fileExtension, bool isPrint, string rw)
        {
            if(PriceReport!=null)
            {
                try
                {
                    if (rw == "R")
                    {
                        switch (fileExtension.ToLower())
                        {
                            case "xls":
                                break;
                            case "pdf":
                                var DataTable = DataGridViewAsDataTableRetail(PriceReport);
                                if (DataTable != null)
                                {
                                    GeneratePDF(DataTable, PriceReport, "pdf", isPrint, rw);
                                    break;
                                }
                                else
                                    break;
                            default:
                                break;
                        }
                    }
                    if (rw == "W")
                    {
                        switch (fileExtension.ToLower())
                        {
                            case "xls":
                                break;
                            case "pdf":
                                var DataTable = DataGridViewAsDataTable(PriceReport);
                                if (DataTable != null)
                                {
                                    GeneratePDF(DataTable, PriceReport, "pdf", isPrint, rw);
                                    break;
                                }
                                else
                                    break;
                            default:
                                break;
                        }
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
        readonly String[] PriceListColumn = new String[]
        {
            "#", "Product Code","Product Name","Retail UOM", "Retail Price", "WholeSale UOM", "WholeSale Price","MSRP"
        };
        public DataTable DataGridViewAsDataTable(PriceReport PriceReport)
        {            
            DataTable PriceListTable = new DataTable();            
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.SNO], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.PCODE], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.PNAME], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.R_UOM], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.R_PRICE], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.W_UOM], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.W_PRICE], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.MSRP], typeof(string));

            DataRow PriceListTableRow = null;
            int rn = 0;
            foreach (PriceReportLineItems LineItem in PriceReport.LineItems)
            {
                PriceListTableRow = PriceListTable.NewRow();
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.SNO]] = rn + 1;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.PCODE]] = LineItem.MaterialId;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.PNAME]] = LineItem.Name;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.R_UOM]] = LineItem.RetailUOM;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.R_PRICE]] = LineItem.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.W_UOM]] = LineItem.WholesaleUOM;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.W_PRICE]] = LineItem.wholeSaleprice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.MSRP]] = LineItem.MSRP.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PriceListTable.Rows.Add(PriceListTableRow);
                rn++;
            }
            return PriceListTable;

        }        
        public DataTable DataGridViewAsDataTableRetail(PriceReport PriceReport)
        {
            DataTable PriceListTable = new DataTable();
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.SNO], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.PCODE], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.PNAME], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.R_UOM], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.R_PRICE], typeof(string));
            PriceListTable.Columns.Add(PriceListColumn[(int)PriceListTableColumn.MSRP], typeof(string));

            DataRow PriceListTableRow = null;
            int rn = 0;
            foreach (PriceReportLineItems LineItem in PriceReport.LineItems)
            {
                PriceListTableRow = PriceListTable.NewRow();
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.SNO]] = rn + 1;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.PCODE]] = LineItem.MaterialId;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.PNAME]] = LineItem.Name;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.R_UOM]] = LineItem.RetailUOM;
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.R_PRICE]] = LineItem.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PriceListTableRow[PriceListColumn[(int)PriceListTableColumn.MSRP]] = LineItem.MSRP.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PriceListTable.Rows.Add(PriceListTableRow);
                rn++;
            }
            return PriceListTable;
        }
        private static readonly Font H1Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, PriceReport PriceReport, string fileExtension, bool isPrint, string rw)
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
                    ReportLine1 = PriceReport.ReportTitle(),
                    ReportLine2 = PriceReport.ReportSubTitle()
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
                    ReportLine1 = PriceReport.ReportTitle(),
                    ReportLine2 = PriceReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();                
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "PriceList");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 25f, 55f, 20f, 20f, 20f, 20f, 20f };
                if (Cols == 6)
                { widths = new float[] { 10f, 25f, 55f, 20f, 20f, 20f }; }
                
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;
                
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    //double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    //if (TotalWorkingOnPageH > A4Height)
                    //{
                    //    pdfDoc.Add(ReportMainTable);
                    //    pdfDoc.NewPage();
                    //    pdfDoc.Add(MiniHTable);
                    //    pdfDoc.Add(MTable);
                    //    ReportMainTable = new PdfPTable(Cols);
                    //    ReportMainTable.SetWidths(widths);
                    //    k = 2;
                    //}
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, H1Font));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                       
                        if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Product Code" || DataTable.Columns[j].ColumnName == "Product Name" || DataTable.Columns[j].ColumnName == "Retail UOM" || DataTable.Columns[j].ColumnName == "WholeSale UOM")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }                        
                        else
                        {
                            if (DataTable.Columns[j].ColumnName == "Retail Price" || DataTable.Columns[j].ColumnName == "WholeSale Price" || DataTable.Columns[j].ColumnName == "MSRP")
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
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
                PdfGeneration.FileName = PriceReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
