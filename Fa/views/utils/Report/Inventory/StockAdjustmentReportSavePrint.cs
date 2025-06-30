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
using FADataAccessLibrary.report.Inventory;
using Fa.reports.Inventory;
using fa.api.utils;
using fa;

namespace Fa.views.utils.Report.Inventory
{
    public class StockAdjustmentReportSavePrint
    {
        public bool ExportOrPrintToFile(RptAdjustmentEntryReport RptAdjustmentEntryReport, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptAdjustmentEntryReport != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptAdjustmentEntryReport);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptAdjustmentEntryReport, ReportName, "pdf", isPrint);
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
        readonly String[] StockAdjustmentBydateColumn = new String[]
        {
            "#","Date", "Reference","Adjust Location", "Quantity", "Created By"
        };
        readonly String[] StockAdjustmentByLocationColumn = new String[]
        {
            "#","Date", "Reference", "Quantity", "Created By"
        };
        readonly String[] StockAdjustmentByItemColumn = new String[]
        {
            "#","Code", "Name","Batch No", "Exp Date", "Quantity"
        };
        public DataTable DataGridViewAsDataTable(RptAdjustmentEntryReport RptAdjustmentEntryReport)
        {
            DataTable PriceListTable = new DataTable();
            if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYDATE)
            {
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.DATE], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.REF], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.ADJUST], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.QTY], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.ADJUSTBY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                DateTime? dateTime = null;

                foreach (StockAdjustReportLine LineItem in RptAdjustmentEntryReport.StockAdjustReportLines)
                {
                    PriceListTableRow = PriceListTable.NewRow();
                    if (dateTime == null || ((DateTime)dateTime).Date != LineItem.Date)
                    {
                        PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        dateTime = LineItem.Date;
                    }
                    PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.REF]] = LineItem.Reference;
                    PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.ADJUST]] = LineItem.AdjustmentLoaction;
                    PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow[StockAdjustmentBydateColumn[(int)StockAdjustmentReportByDateTableColumn.ADJUSTBY]] = LineItem.CreatedBy;
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            else if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYLOCATION)
            {
                PriceListTable.Columns.Add(StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.DATE], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.REF], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.QTY], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.ADJUSTBY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                string Location = string.Empty;

                foreach (StockAdjustReportLine LineItem in RptAdjustmentEntryReport.StockAdjustReportLines)
                {
                    if (Location == string.Empty || Location != LineItem.AdjustmentLoaction)
                    {
                        PriceListTableRow = PriceListTable.NewRow();
                        PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.SNO]] = LineItem.AdjustmentLoaction;
                        PriceListTable.Rows.Add(PriceListTableRow);
                        Location = LineItem.AdjustmentLoaction;
                        rn = 0;
                    }
                    PriceListTableRow = PriceListTable.NewRow();
                    PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                    PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.REF]] = LineItem.Reference;
                    PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow[StockAdjustmentByLocationColumn[(int)StockAdjustmentReportByLocationTableColumn.ADJUSTBY]] = LineItem.CreatedBy;
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            else if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYITEM)
            {
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.CODE], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.NAME], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.BATCH_NUMBER], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.EXP_DATE], typeof(string));
                PriceListTable.Columns.Add(StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.QUANTITY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                foreach (StockAdjustByItemReportLine LineItem in RptAdjustmentEntryReport.StockAdjustByItemReportLines)
                {
                    PriceListTableRow = PriceListTable.NewRow();
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.CODE]] = LineItem.Code;
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.NAME]] = LineItem.Name;
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.BATCH_NUMBER]] = LineItem.BatchNo;
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.EXP_DATE]] = string.IsNullOrEmpty(LineItem.BatchNo) ? "" : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                    PriceListTableRow[StockAdjustmentByItemColumn[(int)StockAdjustmentReporByItemtTableColumn.QUANTITY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            return PriceListTable;
        }
        public void GeneratePDF(DataTable DataTable, RptAdjustmentEntryReport RptAdjustmentEntryReport, string ReportName, string fileExtension, bool isPrint)
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
                    ReportLine1 = RptAdjustmentEntryReport.ReportTitle(),
                    ReportLine2 = RptAdjustmentEntryReport.ReportSubTitle()

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
                    ReportLine1 = RptAdjustmentEntryReport.ReportTitle(),
                    ReportLine2 = RptAdjustmentEntryReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptAdjustmentEntryReport.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f };
                if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYITEM)
                {
                    widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f };
                }
                else if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYLOCATION)
                {
                    widths = new float[] { 10f, 30f, 20f, 20f, 30f };
                }
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

                        if (DataTable.Columns[j].ColumnName != "Quantity")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYLOCATION && j == (int)StockAdjustmentReportByLocationTableColumn.SNO && string.IsNullOrEmpty(DataTable.Rows[i][j + 2].ToString()))
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.BLACK;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                        }
                        if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYLOCATION && string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)StockAdjustmentReportByLocationTableColumn.ADJUSTBY)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderColorRight = BaseColor.BLACK;
                            }
                            else
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                            }
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
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptAdjustmentEntryReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
