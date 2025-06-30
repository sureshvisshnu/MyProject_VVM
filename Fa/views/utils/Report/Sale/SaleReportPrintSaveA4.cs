using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.report.Inventory;
using fa.report.Purchase;
using fa.report.sales;
using fa.reports.Inventory;
using fa.reports.Purchase;
using fa.reports.sales;
using fa.views.utils.Common;
using Fa.report.Purchase;
using Fa.reports.Hms;
using Fa.views.utils.Report.Sale;
using FADataAccessLibrary.report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using static fa.report.sales.SalesReportByProductFamily;

namespace fa.views.utils.Report.Sale
{
    class SaleReportPrintSaveA4
    {
        public string fileName = string.Empty;
        public bool ExportToFileOrPrint(DataGridView ReportGridView, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();
            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES)!.IsDotMatrix;
            fileName = ReportName;
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
                                if (isPrint && ReportName == "SalesReportBySchedule" && IsDotMatrix)
                                {
                                    SaleReportPrintDotmatrix SaleReportPrintDotmatrix = new SaleReportPrintDotmatrix(this);
                                    SaleReportPrintDotmatrix.GeneratePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
                                }
                                else
                                {
                                    GeneratePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
                                }
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
                    if (ReportGridView.ColumnCount == 0) return null!;
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
                        int i = 0;
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if ((ReportGridView.Name=="GridViewForTax" && i>4) ||col.Caption == "Tax" || col.Caption == "Net Amount" || col.Caption == "Discount" || col.Caption == "Total"
                            || col.Caption == "Cash Amount" || col.Caption == "Credit Amount" || col.Caption == "Sub Total" 
                            || col.Caption == "Sales Value" || col.Caption == "Actual Cost" || col.Caption == "Profit / Margin")
                            {
                                if (row.Cells[col.ColumnName].Value == null) { continue; }
                                double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString()!);
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : Math.Round(temp).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            }
                            else
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
                    return null!;
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
                Document pdfDoc = new Document((fileName == "TaxWiseSalesReport" || fileName== "TaxWisePurchaseReport" || fileName== "SalesReportBySchedule") ? PageSize.A4.Rotate() : PageSize.A4, -45, -45, 20, 30);
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
                if (fileName == "SalesReportBySchedule")
                {
                    PdfPTable MainTableForSalesreport = MainTableForSalesreportByseries(dataTable, fileName);
                    pdfDoc.Add(MainTableForSalesreport);
                }
                else
                {
                    PdfPTable ReportMainTable = MainTable(dataTable, fileName);
                    pdfDoc.Add(ReportMainTable);
                }
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, (fileName == "TaxWiseSalesReport" || fileName== "TaxWisePurchaseReport" || fileName== "SalesReportBySchedule") ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
                Cursor.Current = Cursors.Default;
            }
        }
        private PdfPTable MainTable(DataTable DataTable, String TypeOfReport)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count - 1;
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null;
            if (TypeOfReport == "InvoiceWiseSalesReport" || TypeOfReport == "InvoiceWiseSalesReturnReport" || TypeOfReport == "BillWisePurchaseReport" || TypeOfReport == "BillWisePurchaseReturnReport"
               || TypeOfReport == "ReferedWiseSalesReport" || TypeOfReport == "SoldWiseSalesReport")
            {
                widths = new float[] { 10f, 20f, 25f, 60f, 25f, 30f, 25f };
            }
            else if (TypeOfReport == "CustomerWiseSalesReport" || TypeOfReport == "SupplierWisePurchaseReport" || TypeOfReport == "SupplierWisePurchaseReturnReport")
            {
                widths = new float[] { 10f, 25f, 40f, 25f, 25f, 25f, 27f };
            }
            else if (TypeOfReport == "ItemWisePurchaseReport" || TypeOfReport == "ItemWiseSalesReport"
                || TypeOfReport == "CategoryWisePurchaseReport")
            {
                widths = new float[] { 10f, 25f, 25f, 20f, 15f, 20f, 25f, 20f, 25f, 20f };
            }
            else if (TypeOfReport == "TaxWiseSalesReport" || TypeOfReport== "TaxWisePurchaseReport")
            {
                widths = new float[] { 10f, 20f, 25f, 25f, 15f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
            }
            else if (TypeOfReport == "MarginWiseSalesReport")
            {
                widths = new float[] { 10f, 25f, 25f, 25f, 25f, 25f };
            }
            ReportMainTable.SetWidths(widths);

            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_9_Black));
                HeaderCell.BackgroundColor = new BaseColor(230, 230, 230);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 14;
                HeaderCell.Padding = 4;
                if (column.Caption == "#" || column.Caption == "Invoice" || column.Caption == "Bill" || column.Caption == "Customer Details" || column.Caption == "Supplier Details" || column.Caption == "Cash/Credit"
                    || column.Caption == "Item Code" || column.Caption == "Name" || column.Caption == "Batch No" || column.Caption == "Sold" || column.Caption == "Referer"
                    || column.Caption == "Date" || column.Caption == "DateTime" || column.Caption == "Exp. Date" || column.Caption == "Bill Number")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                if (column.Caption != "#")
                {
                    HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                    HeaderCell.BorderWidthBottom = (float)BorderStyle.None;
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
                    if (i != Rows - 1)
                    {
                        RowCell.BorderWidthBottom = (float)BorderStyle.None;
                    }
                    if (j != 0)
                    {
                        RowCell.BorderWidthLeft = (float)BorderStyle.None;
                    }
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 14;
                    RowCell.Padding = 2;
                    if (TypeOfReport == "ItemWisePurchaseReport" || TypeOfReport == "ItemWiseSalesReport" ||
                        TypeOfReport == "CategoryWisePurchaseReport")
                    {
                        if (j == 0 || j == 1 || j == 2 || j == 5 || j == 6)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    else if (TypeOfReport == "CustomerWiseSalesReport" || TypeOfReport == "SupplierWisePurchaseReport" || TypeOfReport == "SupplierWisePurchaseReturnReport")
                    {
                        if (j == 0 || j == 1 || j == 2)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    else if(TypeOfReport == "ReferedWiseSalesReport" || TypeOfReport == "SoldWiseSalesReport")
                    {
                        if (j == 6)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    else if (TypeOfReport== "TaxWisePurchaseReport" || TypeOfReport == "TaxWiseSalesReport" || TypeOfReport == "InvoiceWiseSalesReport" || TypeOfReport == "InvoiceWiseSalesReturnReport" || TypeOfReport == "BillWisePurchaseReport" || TypeOfReport == "BillWisePurchaseReturnReport")
                    {
                        if (j == 0 || j == 1 || j == 2 || j == 3 || j == 4)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    else if (TypeOfReport == "MarginWiseSalesReport")
                    {
                        if((string)DataTable.Rows[i][0] == " " && (string)DataTable.Rows[i][1] == " " && ((string)DataTable.Rows[i][2] == "Sub Total" || (string)DataTable.Rows[i][2] == "Grand Total")) 
                        {
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        }
                        if (j == 0 || j == 1 || (j == 2 && ((string)DataTable.Rows[i][2] != "Sub Total") && (j == 2 && (string)DataTable.Rows[i][2] != "Grand Total")))
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (((string)DataTable.Rows[i][2] == "Sub Total" && i != Rows - 1) || ((string)DataTable.Rows[i][2] == "Sub Total" && i == Rows - 1))
                        {
                            if (j == 0)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 1)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (j == 2)
                            {
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                        }
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }

            //total
            for (int i = Rows; i < Rows + 1; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_8_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 14;
                    RowCell.Padding = 2;
                    RowCell.BorderWidthTop = (float)BorderStyle.None;
                    if (TypeOfReport == "TaxWiseSalesReport" || TypeOfReport== "TaxWisePurchaseReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j == 0)
                        {
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j < 4 && j > 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j > 3)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "BillWisePurchaseReport" || TypeOfReport == "BillWisePurchaseReturnReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j > 0 && j < 4)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j > 3)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (j == 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j == 4 || j == 5 || j == 6)
                        {
                            RowCell.BorderColor = BaseColor.BLACK;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "InvoiceWiseSalesReturnReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j > 0 && j < 4)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j == 0)
                        {
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j == 4)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (j > 3)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "InvoiceWiseSalesReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j> 0 && j < 4)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                            RowCell.BorderColorTop = BaseColor.BLACK;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j == 5 || j == 6)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.BLACK;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 4)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.BLACK;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "CustomerWiseSalesReport" || TypeOfReport == "SupplierWisePurchaseReport"
                        || TypeOfReport == "SupplierWisePurchaseReturnReport")
                    {
                        if (j < 3)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.BorderColorTop = BaseColor.WHITE;
                            RowCell.BorderColorRight = j == 3 ? BaseColor.BLACK : BaseColor.WHITE;
                            RowCell.BorderColorBottom = BaseColor.WHITE;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.BorderColor = BaseColor.BLACK;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "ReferedWiseSalesReport" || TypeOfReport == "SoldWiseSalesReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j == 0)
                        {
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j < 5 && j > 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if(j > 4)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "ItemWiseSalesReport" || TypeOfReport == "ItemWisePurchaseReport"
                        || TypeOfReport == "CategoryWisePurchaseReport" )
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if(j == 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j > 0 && j < 6)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if(j > 5)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (TypeOfReport == "MarginWiseSalesReport")
                    {
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j == 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        else if (j == 1)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        else if (j == 2)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
       
        private PdfPTable MainTableForSalesreportByseries(DataTable DataTable, String TypeOfReport)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_7_Black = FontFactory.GetFont(FONT, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = DataTable.Columns.Count+1;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = null!;
            widths = new float[] { 10f, 15f, 15f, 45f, 20f, 35f, 10f, 20f, 20f, 20f, 20f };
            ReportMainTable.SetWidths(widths);
            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_9_Black));
                HeaderCell.BackgroundColor = new BaseColor(230, 230, 230);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 20;
                HeaderCell.Padding = 4;
                if (column.Caption == "Qty")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                if (column.Caption != "#")
                {
                    HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                }
                ReportMainTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase("Signature", Font_Bold_Italic_9_Black));
            HeaderCell.BackgroundColor = new BaseColor(230, 230, 230);
            HeaderCell.BorderColor = BaseColor.BLACK;
            HeaderCell.MinimumHeight = 20;
            HeaderCell.Padding = 4;
            HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ReportMainTable.AddCell(HeaderCell);
            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = j == 10 ? "" : DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_7_Black));
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderWidthTop = (float)BorderStyle.None;
                    if (j != 0 )
                    {
                        RowCell.BorderWidthLeft = (float)BorderStyle.None;
                    }
                    if (DataTable.Rows[i][0].ToString() == " " && DataTable.Rows[i][1].ToString() == " " && j != 3)
                    {
                        if (j == 0)
                        {
                            RowCell.BorderColorLeft = BaseColor.BLACK;
                        }
                        Temp = DataTable.Rows[i][2].ToString();
                        if (Temp == "Cash" || Temp == "Credit")
                        {
                            RowCell.BorderColorTop = BaseColor.BLACK;
                        }
                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    else if (DataTable.Rows[i][0].ToString() == " " && DataTable.Rows[i][1].ToString() != " ")
                    {
                        if (j == 0)
                        {
                            Temp = DataTable.Rows[i][1].ToString();
                            RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_7_Black));
                            RowCell.Colspan = 4;
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            RowCell.UseVariableBorders = true;
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        RowCell.BorderColor = BaseColor.BLACK;
                        RowCell.MinimumHeight = 20;
                        RowCell.Padding = 2;
                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        if (j == 6)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                    }
                    if (i == (Rows - 1))
                    {
                        RowCell.BorderColorBottom = BaseColor.BLACK;
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }
            return ReportMainTable;
        }
        public bool CustomerWiseSaleReturnExportOrPrint(SalesReportByCustomer SalesReportByCustomer1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByCustomer1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewCustomerAsDataTable(SalesReportByCustomer1);
                            if (DataTable != null)
                            {
                                GenerateCustomerWisePDF(DataTable, SalesReportByCustomer1, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        public bool ExportToFileOrPrintForUserWise(SalesReportByUser SalesReportByUser, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByUser != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewUserAsDataTable(SalesReportByUser);
                            if (DataTable != null)
                            {
                                GenerateUserWisePDF(DataTable, SalesReportByUser, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] UserwiseListColumn = new String[]
        {
                "#", "Invoice", "Date", "Customer Name", "Tax", "Total Amount", "Pay Type","Receive", "Balance"
        };
        public static DataTable DataGridViewUserAsDataTable(SalesReportByUser SalesReportByUser)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.INVOICE_NUMBER], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.INVOICE_DATE], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.CUSTOMER_INFO], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.NET], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.TYPE], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.RECEIVED], typeof(string));
            dataTable.Columns.Add(UserwiseListColumn[(int)SalesReportByUserTableColumn.BALANCE], typeof(string));

            DataRow ByUserTableRow = null;
            int Sno = 0;
            double SubTaxTotal = 0, SubNetAmountTotal = 0, SubRecevieAmountTotal = 0, SubBalanceAmountTotal = 0;
            double GrandTaxTotal = 0, GrandNetAmountTotal = 0, GrandReceiveAmountTotal = 0, GrandBalanceAmountTotal = 0;
            string User = string.Empty;
            foreach (SalesReportByUserLineItem LineItem in SalesReportByUser.LineItems.OrderBy(x => x.User))
            {
                if (User != LineItem.User)
                {
                    if (!string.IsNullOrEmpty(User))
                    {
                        ByUserTableRow = dataTable.NewRow();
                        ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.CUSTOMER_INFO]] = "Sub Total";
                        ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.TAX]] = SubTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.NET]] = SubNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.RECEIVED]] = SubRecevieAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.BALANCE]] = SubBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        dataTable.Rows.Add(ByUserTableRow);
                        SubTaxTotal = 0; SubNetAmountTotal = 0; SubRecevieAmountTotal = 0; SubBalanceAmountTotal = 0;
                    }
                }

                if (User != LineItem.User)
                {
                    string trimmedUser = LineItem.User.Contains("[") ? LineItem.User.Substring(0, LineItem.User.IndexOf("[")).Trim() : LineItem.User;
                    ByUserTableRow = dataTable.NewRow();
                    ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.SNO]] = "User Name : " + trimmedUser;
                    dataTable.Rows.Add(ByUserTableRow);
                    User = LineItem.User;
                    Sno = 0;
                }
                double BalanceAmount = 0;
                ByUserTableRow = dataTable.NewRow();
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.SNO]] = ++Sno;
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.INVOICE_NUMBER]] = LineItem.InvoiceNumber;
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.INVOICE_DATE]] = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.CUSTOMER_INFO]] = LineItem.CustomerName;
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.TAX]] = LineItem.InvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.NET]] = LineItem.InvoiceAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.TYPE]] = LineItem.PaymentType;
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.RECEIVED]] = LineItem.ReceiveAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                BalanceAmount = LineItem.InvoiceAmount - (double)LineItem.ReceiveAmount;
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.BALANCE]] = BalanceAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                dataTable.Rows.Add(ByUserTableRow);

                GrandNetAmountTotal += LineItem.InvoiceAmount; GrandTaxTotal += LineItem.InvoiceTax; GrandReceiveAmountTotal += (double)LineItem.ReceiveAmount; GrandBalanceAmountTotal += BalanceAmount;
                SubNetAmountTotal += LineItem.InvoiceAmount; SubTaxTotal += LineItem.InvoiceTax; SubBalanceAmountTotal += BalanceAmount; SubRecevieAmountTotal += (double)LineItem.ReceiveAmount;
            }

            if (!string.IsNullOrEmpty(User))
            {
                ByUserTableRow = dataTable.NewRow();
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.CUSTOMER_INFO]] = "Sub Total";
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.TAX]] = SubTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.NET]] = SubNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.RECEIVED]] = SubRecevieAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.BALANCE]] = SubBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                dataTable.Rows.Add(ByUserTableRow);
                SubTaxTotal = 0; SubNetAmountTotal = 0; SubRecevieAmountTotal = 0; SubBalanceAmountTotal = 0;
            }

            ByUserTableRow = dataTable.NewRow();
            ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.CUSTOMER_INFO]] = "Grand Total";
            ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.TAX]] = GrandTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.NET]] = GrandNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.RECEIVED]] = GrandReceiveAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByUserTableRow[UserwiseListColumn[(int)SalesReportByUserTableColumn.BALANCE]] = GrandBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByUserTableRow);

            return dataTable;

        }
        public void GenerateUserWisePDF(DataTable dataTable, SalesReportByUser SalesReportByUser, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            double A4Height = 760;
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
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "UserWiseSalesReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count - 1;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;
                widths = new float[] { 10f, 25f, 25f, 40f, 20f, 25f, 25f, 25f, 25f };
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(HTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        if (i != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorTop = BaseColor.WHITE;
                        }
                        if (j != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                        }
                        if (j == 4 || j == 5 || j == 7 || j == 8)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (dataTable.Rows[i][3].ToString() == "Sub Total")
                        {
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j >= 1 && j < 3)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 3)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][8].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][8].ToString()))
                        {
                            continue;
                        }

                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                for (int i = Rows; i < Rows + 1; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    BaseColor CurRowColor = RowColor[k % 2];
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        RowCell.BorderWidthTop = (float)BorderStyle.None;
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (j == 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j > 2)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j > 0 && j < 3)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 3)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
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
                PdfGeneration.FileName = SalesReportByUser.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportOrPrintToFileFamilywise(SalesReportByProductFamily SalesReportByProductFamily, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByProductFamily != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewFamilyAsDataTable(SalesReportByProductFamily);
                            if (DataTable != null)
                            {
                                GenerateFamilyWisePDF(DataTable, SalesReportByProductFamily, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] FamilywiseListColumn = new String[]
        {
            "#", "Item Code", "Name", "Quantity", "Free", "Batch No", "Exp. Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable DataGridViewFamilyAsDataTable(SalesReportByProductFamily SalesReportByProductFamily)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TOTAL], typeof(string));

            DataRow ByFamilyTableRow = null;
            int Sno = 0;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            string PFamily = string.Empty;

            foreach (SalesReportByProductFamilyLineItem LineItem in SalesReportByProductFamily.LineItems.OrderBy(x => x.ProductFamily))
            {
                if (PFamily != LineItem.ProductFamily)
                {
                    ByFamilyTableRow = dataTable.NewRow();
                    ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SNO]] = "Product Family : " + LineItem.ProductFamily;
                    dataTable.Rows.Add(ByFamilyTableRow);
                    PFamily = LineItem.ProductFamily;
                    Sno = 0;
                }
                ByFamilyTableRow = dataTable.NewRow();
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SNO]] = Sno + 1;
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.ITEM]] = LineItem.Item;
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.EXP_DATE]] = LineItem.ExpDate.ToString(Global.Company.DateFormat);
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TAX]] = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                dataTable.Rows.Add(ByFamilyTableRow);
                Sno++;
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
            }
            ByFamilyTableRow = dataTable.NewRow();
            ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.EXP_DATE]] = "Total";
            ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TAX]] = Math.Round(TaxTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByFamilyTableRow[FamilywiseListColumn[(int)SalesReportByPFamilyTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByFamilyTableRow);
            return dataTable;
        }
        public void GenerateFamilyWisePDF(DataTable dataTable, SalesReportByProductFamily SalesReportByProductFamily, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            double A4Height = 760;
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
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "ProductFamilyWiseSalesReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count - 1;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;
                widths = new float[] { 10f, 20f, 35f, 20f, 15f, 20f, 20f, 20f, 20f, 20f };
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(HTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        RowCell.BorderWidthBottom = (float)BorderStyle.None;
                        if (j != 0)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (j == 3 || j == 4 || j == 7 || j == 8 || j == 9)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][1].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][1].ToString()))
                        {
                            continue;
                        }
                        
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }

                for (int i = Rows; i < Rows + 1; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    BaseColor CurRowColor = RowColor[k % 2];
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        if (fileName == "ProductFamilyWiseSalesReport")
                        {
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 0 && j < 6)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j > 5)
                            {
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
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
                PdfGeneration.FileName = SalesReportByProductFamily.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }

        public bool ExportOrPrintToFileAreawise(SalesReportByArea SalesReportByArea, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByArea != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAreaAsDataTable(SalesReportByArea);
                            if (DataTable != null)
                            {
                                GenerateAreaWisePDF(DataTable, SalesReportByArea, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] AreawiseListColumn = new String[]
        {
            "#", "Item Code", "Name", "Quantity", "Free", "Batch No", "Exp. Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable DataGridViewAreaAsDataTable(SalesReportByArea SalesReportByArea)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(AreawiseListColumn[(int)SalesReportByAreaTableColumn.TOTAL], typeof(string));

            DataRow ByAreaTableRow = null;
            int Sno = 0;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            double Qty = 0;
            double Free = 0;
            double GTotal = 0;
            double GSubTotal = 0;
            double GTaxTotal = 0;
            double GQty = 0;
            double GFree = 0;
            string Customername = "000000";
            string City = "000000";
            foreach (SalesReportByAreaLineItem LineItem in SalesReportByArea.LineItems.OrderBy(x => x.City).ThenBy(x => x.CustomerName))
            {
                if (City != LineItem.City)
                {
                    if (City != "000000")
                    {
                        ByAreaTableRow = dataTable.NewRow();
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM_NAME]] = "Sub Total";
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.QUANTITY]] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.FREE]] = Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        dataTable.Rows.Add(ByAreaTableRow);
                        Qty = 0;
                        Free = 0;
                        SubTotal = 0;
                        TaxTotal = 0;
                        Total = 0;
                    }
                    ByAreaTableRow = dataTable.NewRow();
                    ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SNO]] = LineItem.City == string.Empty ? "Unknown Area" : LineItem.City;
                    dataTable.Rows.Add(ByAreaTableRow);
                    City = LineItem.City;
                    Sno = 0;
                }
                if (Customername != LineItem.CustomerName)
                {
                    ByAreaTableRow = dataTable.NewRow();
                    ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SNO]] = LineItem.CustomerName == string.Empty ? "Unknown Customer" : LineItem.CustomerName;
                    dataTable.Rows.Add(ByAreaTableRow);
                    Customername = LineItem.CustomerName;
                    Sno = 0;
                }
                ByAreaTableRow = dataTable.NewRow();
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SNO]] = Sno + 1;
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM]] = LineItem.Item;
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.EXP_DATE]] = LineItem.ExpDate.ToString(Global.Company.DateFormat);
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TAX]] = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                dataTable.Rows.Add(ByAreaTableRow);
                Sno++;
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
                Qty += LineItem.Qty;
                Free += LineItem.Free;
                GTotal += LineItem.Total;
                GTaxTotal += LineItem.Tax;
                GSubTotal += LineItem.SubTotal;
                GQty += LineItem.Qty;
                GFree += LineItem.Free;
            }
            ByAreaTableRow = dataTable.NewRow();
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM_NAME]] = "Sub Total";
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.QUANTITY]] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.FREE]] = Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataTable.Rows.Add(ByAreaTableRow);

            ByAreaTableRow = dataTable.NewRow();
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.ITEM_NAME]] = "Grand Total";
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.SUB_TOTAL]] = Math.Round(GSubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TAX]] = Math.Round(GTaxTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.TOTAL]] = Math.Round(GTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.QUANTITY]] = GQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            ByAreaTableRow[AreawiseListColumn[(int)SalesReportByAreaTableColumn.FREE]] = GFree.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            dataTable.Rows.Add(ByAreaTableRow);

            return dataTable;
        }
        public void GenerateAreaWisePDF(DataTable dataTable, SalesReportByArea SalesReportByArea, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            double A4Height = 760;
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
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "AreaWiseSalesReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count - 1;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;
                widths = new float[] { 10f, 20f, 35f, 20f, 15f, 20f, 20f, 20f, 20f, 20f };
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(HTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);

                        if (j != 0)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (i!=Rows-1)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorBottom = BaseColor.WHITE;
                        }
                        if (j == 3 || j == 4 || j == 7 || j == 8 || j == 9)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][1].ToString() == "" && dataTable.Rows[i][2].ToString() == "Sub Total")
                        {
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            if (j == 0 || j == 1)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j != 0)
                            {
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (j == 1 || j == 2)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][2].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][2].ToString()))
                        {
                            continue;
                        }

                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }

                for (int i = Rows; i < Rows + 1; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    BaseColor CurRowColor = RowColor[k % 2];
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BorderWidthTop = (float)BorderStyle.None;
                        RowCell.BackgroundColor = CurRowColor;
                        if (fileName == "AreaWiseSalesReport")
                        {
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            if (j == 0 || j == 1)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j != 0)
                            {
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (j == 1 || j == 2)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
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
                PdfGeneration.FileName = SalesReportByArea.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportOrPrintToFileCustomer(SalesReportByCustomer SalesReportByCustomer1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByCustomer1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewCustomerAsDataTable(SalesReportByCustomer1);
                            if (DataTable != null)
                            {
                                GenerateCustomerWisePDF(DataTable, SalesReportByCustomer1, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] CustomerwiseListColumn = new String[]
        {
            "#", "Invoice", "Date", "Tax", "Discount", "Cash Amount", "Credit Amount"
        };
        public static DataTable DataGridViewCustomerAsDataTable(SalesReportByCustomer SalesReportByCustomer1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.INVOICE_DATE], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.DIS], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT], typeof(string));
            dataTable.Columns.Add(CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT], typeof(string));
            DataRow ByCustomerTableRow = null;

            double CashTotal = 0;
            double CreditTotal = 0;
            double taxTotal = 0;
            double discountTotal = 0;
            string customer = "";
            int i = 1;
            int RoundingPrecision = Global.Company.PrimaryCurrency.RoundingPrecision;
            string formatSpecifier = $"F{RoundingPrecision}";

            foreach (SalesReportByCustomerLineItem LineItem in SalesReportByCustomer1.LineItems.OrderBy(x => x.Customer))
            {
                double cashAmount = Math.Round(LineItem.CustomerInvoiceAmount, MidpointRounding.AwayFromZero);
                ByCustomerTableRow = dataTable.NewRow();
                if (customer == "" || customer != LineItem.Customer)
                {
                    ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.SNO]] = "Name : " + LineItem.Customer;
                    customer = LineItem.Customer;
                    i = 1;
                    dataTable.Rows.Add(ByCustomerTableRow);
                    ByCustomerTableRow = dataTable.NewRow();
                }
                ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.SNO]] = i;
                ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER]] = LineItem.CustomerInvoiceNumber;
                ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.INVOICE_DATE]] = LineItem.CustomerInvoiceDate.Date.ToString(Global.Company.DateFormat);
                ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.TAX]] = Math.Round(LineItem.CustomerInvoiceTax, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
                ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.DIS]] = Math.Round(LineItem.CustomerInvoiceDiscount, MidpointRounding.AwayFromZero).ToString(formatSpecifier);

                if (LineItem.InvoiceType == "Credit")
                {   
                    ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT]] = Math.Round(LineItem.CustomerInvoiceAmount, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
                    ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT]] = Math.Round(0.00, MidpointRounding.AwayFromZero).ToString(formatSpecifier);

                    CreditTotal += LineItem.CustomerInvoiceAmount;
                }
                else
                {
                    ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT]] = Math.Round(0.00, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
                    ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT]] = Math.Round(LineItem.CustomerInvoiceAmount, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
                    CashTotal += LineItem.CustomerInvoiceAmount;
                }
                taxTotal += LineItem.CustomerInvoiceTax;
                discountTotal += LineItem.CustomerInvoiceDiscount;
                dataTable.Rows.Add(ByCustomerTableRow);
                i++;
            }
            ByCustomerTableRow = dataTable.NewRow();
            ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.INVOICE_DATE]] = "Total";
            ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.TAX]] = Math.Round(taxTotal, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
            ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.DIS]] = Math.Round(discountTotal, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
            ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT]] = Math.Round(CashTotal, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
            ByCustomerTableRow[CustomerwiseListColumn[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT]] = Math.Round(CreditTotal, MidpointRounding.AwayFromZero).ToString(formatSpecifier);
            dataTable.Rows.Add(ByCustomerTableRow);
            return dataTable;
        }
        public void GenerateCustomerWisePDF(DataTable dataTable, SalesReportByCustomer SalesReportByCustomer1, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                pdfDoc.Add(HTable);
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "CustomerwiseSalesReport");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(7);
                float[] widths = new float[] { 15f, 25f, 40f, 25f, 25f, 25f, 27f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
                bool page = false;
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        page = true;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;

                        if (i != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorTop = BaseColor.WHITE;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        if (j != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (dataTable.Columns[j].ColumnName == "Tax" || dataTable.Columns[j].ColumnName == "Discount"|| dataTable.Columns[j].ColumnName == "Cash Amount"|| dataTable.Columns[j].ColumnName == "Credit Amount")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (i == Rows - 1 && dataTable.Columns[j].ColumnName == "Date")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (string.IsNullOrEmpty(dataTable.Rows[i][j].ToString()) || Temp == "Total" || i == Rows - 1)
                        {
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][1].ToString() == "") || i == Rows - 1)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            }
                            if (i == Rows - 1 && j == 1)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if(j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][5].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][5].ToString()))
                        {
                            continue;
                        }

                        if (page)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthTop = 0.25f;
                            RowCell.BorderColorTop = BaseColor.GRAY;
                        }
                        RowCell.MinimumHeight = 15;
                        ReportMainTable.AddCell(RowCell);
                    }
                    page = false;
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

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName + DateTime.Now.Date.ToString(Global.Company.DateFormat);
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportToFileOrPrintCategorywise(SalesReportByCategory SalesReportByCategory1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByCategory1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewCatagoryAsDataTable(SalesReportByCategory1);
                            if (DataTable != null)
                            {
                                GenerateCatagoryWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] CatagorywiseListColumn = new String[]
        {
            "#", "Item Code", "Name", "Qty", "Free", "Batch.No", "Exp.Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable DataGridViewCatagoryAsDataTable(SalesReportByCategory SalesReportByCategory1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TOTAL], typeof(string));
            DataRow ByCatagoryTableRow = null;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            string catagory = string.Empty;
            int i = 1;

            foreach (SalesReportByCategoryLineItem LineItem in SalesReportByCategory1.LineItems.OrderBy(x => x.Catagory))
            {
                ByCatagoryTableRow = dataTable.NewRow();
                if (catagory == string.Empty || catagory != LineItem.Catagory)
                {
                    ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SNO]] = "Catagory : " + LineItem.Catagory;
                    catagory = LineItem.Catagory;
                    dataTable.Rows.Add(ByCatagoryTableRow);
                    ByCatagoryTableRow = dataTable.NewRow();
                    i = 1;
                }
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SNO]] = i;
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.ITEM]] = LineItem.Item;
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE]] = LineItem.ExpDate.ToString(SalesReportByCategory1.Company.DateFormat);
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TAX]] = Math.Round(LineItem.Tax).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
                dataTable.Rows.Add(ByCatagoryTableRow);
                i++;
            }
            ByCatagoryTableRow = dataTable.NewRow();
            ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE]] = "Total";
            ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
            ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByCatagoryTableRow[CatagorywiseListColumn[(int)SalesReportByCategoryTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByCatagoryTableRow);

            return dataTable;
        }
        public void GenerateCatagoryWisePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                pdfDoc.Add(HTable);
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "CatagorywiseSalesReport");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(10);
                float[] widths = new float[] { 10f, 30f, 45f, 13f, 12f, 20f, 20f, 20f, 15f, 20f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
                bool page = false;
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        page = true;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;

                        if (i != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorTop = BaseColor.WHITE;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        if (j != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (dataTable.Columns[j].ColumnName == "Tax" || dataTable.Columns[j].ColumnName == "Total" || dataTable.Columns[j].ColumnName == "Sub Total" || dataTable.Columns[j].ColumnName == "Qty" || dataTable.Columns[j].ColumnName == "Free")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (i == Rows - 1 && dataTable.Columns[j].ColumnName == "Exp.Date")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (string.IsNullOrEmpty(dataTable.Rows[i][j].ToString()) || Temp == "Total" || i == Rows - 1)
                        {
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][1].ToString() == "") || i == Rows - 1)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            }
                            if (i == Rows - 1 && (j >= 1 & j < 6))
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()))
                        {
                            continue;
                        }

                        if (page)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthTop = 0.25f;
                            RowCell.BorderColorTop = BaseColor.GRAY;
                        }
                        RowCell.MinimumHeight = 15;
                        ReportMainTable.AddCell(RowCell);
                    }
                    page = false;
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

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName + DateTime.Now.Date.ToString(Global.Company.DateFormat);
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportToFileOrPrintItemWise(SalesReportByItem SalesReportByItem1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (SalesReportByItem1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewItemAsDataTable(SalesReportByItem1);
                            if (DataTable != null)
                            {
                                GenerateCatagoryWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] ItemWiseSalesListColumn = new String[]
        {
            "#", "Item Code", "Name", "Qty", "Free", "Batch.No", "Exp.Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable DataGridViewItemAsDataTable(SalesReportByItem SalesReportByItem1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TOTAL], typeof(string));
            DataRow ByItemsTableRow = null;

            int rowCount = 0;
            int i = 1;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            string ItemName = string.Empty;
            foreach (SalesReportByCategoryLineItem LineItem in SalesReportByItem1.LineItems.OrderBy(x => x.ItemName))
            {
                ByItemsTableRow = dataTable.NewRow();
                if (ItemName == string.Empty || ItemName != LineItem.ItemName)
                {
                    i = 1;
                    ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SNO]] = "Name : " + LineItem.ItemName;
                    ItemName = LineItem.ItemName;
                    dataTable.Rows.Add(ByItemsTableRow);
                    ByItemsTableRow = dataTable.NewRow();
                    rowCount++;
                }
                if (ItemName == LineItem.ItemName)
                {
                    ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SNO]] = i;
                }
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.ITEM]] = LineItem.Item;
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE]] = LineItem.ExpDate.ToString(SalesReportByItem1.Company.DateFormat);
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TAX]]  = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                rowCount++;
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
                dataTable.Rows.Add(ByItemsTableRow);
                i++;
            }
            ByItemsTableRow = dataTable.NewRow();
            ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.EXP_DATE]] = "Total";
            ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByItemsTableRow[ItemWiseSalesListColumn[(int)SalesReportByCategoryTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByItemsTableRow);
            return dataTable;
        }
        public bool ExportSupplierToFileOrPrint(PurchaseReportBySupplier PurchaseReportBySupplier1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (PurchaseReportBySupplier1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewSupplierAsDataTable(PurchaseReportBySupplier1);
                            if (DataTable != null)
                            {
                                GenerateSupplierWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] SupplierywiseListColumn = new String[]
        {
            "#", "Bill", "Date", "Tax", "Discount", "Cash Amount", "Credit Amount"
        };
        public static DataTable DataGridViewSupplierAsDataTable(PurchaseReportBySupplier PurchaseReportBySupplier1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_NUMBER], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT], typeof(string));
            DataRow BySupplierTableRow = null;

            int rowCount = 0;
            int i = 1;
            double CashTotal = 0;
            double CreditTotal = 0;
            double taxTotal = 0;
            double discountTotal = 0;
            string supplierName = string.Empty;

            foreach (PurchaseReportBySupplierLineItem LineItem in PurchaseReportBySupplier1.LineItems.OrderBy(x => x.SupplierName))
            {
                BySupplierTableRow = dataTable.NewRow();
                if (supplierName == string.Empty || supplierName != LineItem.SupplierName)
                {
                    i = 1;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO]] = "vendor name : " + LineItem.SupplierName;
                    supplierName = LineItem.SupplierName;
                    dataTable.Rows.Add(BySupplierTableRow);
                    BySupplierTableRow = dataTable.NewRow();
                    rowCount++;
                }
                if (supplierName == LineItem.SupplierName)
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO]] = i;
                }
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_NUMBER]] = LineItem.SupplierBillNumber;
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE]] = LineItem.SupplierBillDate.Date.ToString(Global.Company.DateFormat);
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX]] = LineItem.SupplierBillTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS]] = LineItem.SupplierBillDiscount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;

                if (LineItem.BillType == "Credit")
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = LineItem.SupplierBillAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;

                    CreditTotal += LineItem.SupplierBillAmount;
                }
                else
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = LineItem.SupplierBillAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    CashTotal += LineItem.SupplierBillAmount;
                }
                taxTotal += LineItem.SupplierBillTax;
                discountTotal += LineItem.SupplierBillDiscount;
                rowCount++;
                dataTable.Rows.Add(BySupplierTableRow);
                i++;
            }
            BySupplierTableRow = dataTable.NewRow();
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE]] = "Total";
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX]] = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS]] = discountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = CashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = CreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            dataTable.Rows.Add(BySupplierTableRow);
            return dataTable;
        }
        public void GenerateSupplierWisePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                pdfDoc.Add(HTable);
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "SupplierWisePurchaseReport");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(7);
                float[] widths = new float[] { 10f, 25f, 40f, 25f, 25f, 25f, 27f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
                bool page = false;
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        page = true;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        RowCell.BorderWidthTop = (float)BorderStyle.None;

                        if (j > 2 || (i == Rows - 1 && j == 2))
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j != 0)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (string.IsNullOrEmpty(dataTable.Rows[i][j].ToString()) || Temp == "Total" || i == Rows - 1)
                        {
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][1].ToString() == "") || i == Rows - 1)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            }
                            if (i == Rows - 1 && (j == 1))
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (i == Rows - 1 && j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (i == Rows - 1 && j == 2)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][6].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][6].ToString()))
                        {
                            continue;
                        }
                        if (page)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthTop = 0.25f;
                            RowCell.BorderColorTop = BaseColor.GRAY;
                        }
                        RowCell.MinimumHeight = 15;
                        ReportMainTable.AddCell(RowCell);
                    }
                    page = false;
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

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName + DateTime.Now.Date.ToString(Global.Company.DateFormat);
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportCatagoryToFileOrPrint(PurchaseReportByCategory PurchaseReportByCategory1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (PurchaseReportByCategory1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = GridViewCatagoryAsDataTable(PurchaseReportByCategory1);
                            if (DataTable != null)
                            {
                                GenerateCatagoryItemWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] CatagoryWiseListColumn = new String[]
        {
            "#", "Item Code", "Name", "Quantity", "Free", "Batch No", "Exp.Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable GridViewCatagoryAsDataTable(PurchaseReportByCategory PurchaseReportByCategory1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL], typeof(string));
            DataRow ByCatagoryTableRow = null;

            int rowCount = 0;
            int i = 1;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            string CatagoryName = string.Empty;

            foreach (PurchaseReportByCategoryLineItem LineItem in PurchaseReportByCategory1.LineItems.OrderBy(x => x.Catagory))
            {
                ByCatagoryTableRow = dataTable.NewRow();
                if (CatagoryName == string.Empty || CatagoryName != LineItem.Catagory)
                {
                    i = 1;
                    ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO]] = "Catagory : " + LineItem.Catagory;
                    CatagoryName = LineItem.Catagory;
                    dataTable.Rows.Add(ByCatagoryTableRow);
                    ByCatagoryTableRow = dataTable.NewRow();
                    rowCount++;
                }
                if (CatagoryName == LineItem.Catagory)
                {
                    ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO]] = i;
                }
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM]] = LineItem.Item;
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE]] = LineItem.ExpDate.Date.ToString(PurchaseReportByCategory1.Company.DateFormat);
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL]] = LineItem.SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX]] = LineItem.Tax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL]] = LineItem.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                rowCount++;
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
                dataTable.Rows.Add(ByCatagoryTableRow);
                i++;
            }
            ByCatagoryTableRow = dataTable.NewRow();
            ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE]] = "Total";
            ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL]] = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX]] = TaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByCatagoryTableRow[CatagoryWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL]] = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByCatagoryTableRow);
            return dataTable;
        }
        public void GenerateCatagoryItemWisePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                pdfDoc.Add(HTable);
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "CatagoryWisePurchaseReport");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(10);
                float[] widths = new float[] { 10f, 25f, 40f, 15f, 15f, 20f, 20f, 20f, 20f, 20f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
                bool page = false;
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        page = true;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        RowCell.BorderWidthTop = (float)BorderStyle.None;

                        if ((i == Rows - 1 && j > 5) || ( j == 3 || j == 4 || j == 7 || j == 8 || j == 9))
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j != 0)
                        {
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (string.IsNullOrEmpty(dataTable.Rows[i][j].ToString()) || Temp == "Total" || i == Rows - 1)
                        {
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][1].ToString() == "") || i == Rows - 1)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            }
                            if (i == Rows - 1 && (j > 0 && j < 6))
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (i == Rows - 1 && j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (i == Rows - 1 && j == 6)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()))
                        {
                            continue;
                        }
                        if (page)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthTop = 0.25f;
                            RowCell.BorderColorTop = BaseColor.GRAY;
                        }
                        RowCell.MinimumHeight = 15;
                        ReportMainTable.AddCell(RowCell);
                    }
                    page = false;
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

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName + DateTime.Now.Date.ToString(Global.Company.DateFormat);
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool ExportItemsToFileOrPrint(PurchaseReportByItem PurchaseReportByItem1, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (PurchaseReportByItem1 != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = GridViewItemAsDataTable(PurchaseReportByItem1);
                            if (DataTable != null)
                            {
                                GenerateCatagoryItemWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        static readonly String[] ItemWiseListColumn = new String[]
        {
            "#", "Item Code", "Name", "Quantity", "Free", "Batch No", "Exp.Date", "Sub Total", "Tax", "Total"
        };
        public static DataTable GridViewItemAsDataTable(PurchaseReportByItem PurchaseReportByItem1)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.QUANTITY], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.FREE], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL], typeof(string));
            DataRow ByItemTableRow = null;

            int rowCount = 0;
            int i = 1;
            double Total = 0;
            double SubTotal = 0;
            double TaxTotal = 0;
            string ItemName = string.Empty;
            foreach (PurchaseReportByCategoryLineItem LineItem in PurchaseReportByItem1.LineItems.OrderBy(x => x.ItemName))
            {
                ByItemTableRow = dataTable.NewRow();
                if (ItemName == string.Empty || ItemName != LineItem.ItemName)
                {
                    i = 1;
                    ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO]] = "Name : " + LineItem.ItemName;
                    ItemName = LineItem.ItemName;
                    dataTable.Rows.Add(ByItemTableRow);
                    ByItemTableRow = dataTable.NewRow();
                    rowCount++;
                }
                if (ItemName == LineItem.ItemName)
                {
                    ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SNO]] = i;
                }
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM]] = LineItem.Item;
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME]] = LineItem.ItemName;
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER]] = LineItem.BatchNumber;
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE]] = LineItem.ExpDate.Date.ToString(PurchaseReportByItem1.Company.DateFormat);
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL]] = LineItem.SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX]] = LineItem.Tax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL]] = LineItem.Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                rowCount++;
                Total += LineItem.Total;
                TaxTotal += LineItem.Tax;
                SubTotal += LineItem.SubTotal;
                dataTable.Rows.Add(ByItemTableRow);
                i++;
            }
            ByItemTableRow = dataTable.NewRow();
            ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.EXP_DATE]] = "Total";
            ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL]] = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TAX]] = TaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ByItemTableRow[ItemWiseListColumn[(int)PurchaseReportByCategoryTableColumn.TOTAL]] = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            dataTable.Rows.Add(ByItemTableRow);
            return dataTable;
        }
        public bool ExportPurchaseReturnSupplierToFileOrPrint(PurchaseReturnReportBySupplier purchaseReturnReportBySupplier, string ReportHeading, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            fileName = ReportName;
            if (purchaseReturnReportBySupplier != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewPurchaseReturnSupplierAsDataTable(purchaseReturnReportBySupplier);
                            if (DataTable != null)
                            {
                                GenerateSupplierWisePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint, FromDate, Todate);
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
        public static DataTable DataGridViewPurchaseReturnSupplierAsDataTable(PurchaseReturnReportBySupplier PurchaseReturnReportBySupplier)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_NUMBER], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT], typeof(string));
            dataTable.Columns.Add(SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT], typeof(string));
            DataRow BySupplierTableRow = null;

            int rowCount = 0;
            int i = 1;
            double CashTotal = 0;
            double CreditTotal = 0;
            double taxTotal = 0;
            double discountTotal = 0;
            string supplierName = string.Empty;
            String dateTime = null;
            foreach (PurchaseReturnReportBySupplierLineItem LineItem in PurchaseReturnReportBySupplier.LineItems.OrderBy(x => x.SupplierName))
            {
                String stringLineItemDate = DateUtils.FormatDate(LineItem.SupplierBillDate, Global.Company.DateFormat);
                BySupplierTableRow = dataTable.NewRow();
                if (supplierName == string.Empty || supplierName != LineItem.SupplierName)
                {
                    i = 1;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO]] = "vendor name : " + LineItem.SupplierName;
                    supplierName = LineItem.SupplierName;
                    dataTable.Rows.Add(BySupplierTableRow);
                    BySupplierTableRow = dataTable.NewRow();
                    rowCount++;
                }
                if (supplierName == LineItem.SupplierName)
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.SNO]] = i;
                }
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_NUMBER]] = LineItem.SupplierBillNumber;
                if (dateTime == null || dateTime != stringLineItemDate)
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE]] = LineItem.SupplierBillDate.Date.ToString(Global.Company.DateFormat);
                    dateTime = stringLineItemDate;
                }
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX]] = LineItem.SupplierBillTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS]] = LineItem.SupplierBillDiscount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;

                if (LineItem.BillType == "Credit")
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = LineItem.SupplierBillAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;

                    CreditTotal += LineItem.SupplierBillAmount;
                }
                else
                {
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = LineItem.SupplierBillAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
                    CashTotal += LineItem.SupplierBillAmount;
                }
                taxTotal += LineItem.SupplierBillTax;
                discountTotal += LineItem.SupplierBillDiscount;
                rowCount++;
                dataTable.Rows.Add(BySupplierTableRow);
                i++;
            }
            BySupplierTableRow = dataTable.NewRow();
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.BILL_DATE]] = "Total";
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.TAX]] = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.DIS]] = discountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT]] = CashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            BySupplierTableRow[SupplierywiseListColumn[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT]] = CreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); ;
            dataTable.Rows.Add(BySupplierTableRow);
            return dataTable;
        }
    }
}
