using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.api.utils;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Fa.views.utils.Report.Inventory
{
    public class StockMovementReportSavePrint
    {
        public bool ExportOrPrintToFile(RptStockMovement RptStockMovement, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptStockMovement != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptStockMovement);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptStockMovement, ReportName, "pdf", isPrint);
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
        readonly String[] StockMovementByDateTableColumn = new String[]
        {
            "#","Date", "Reference", "Source", "Destination", "Quantity", "Free", "Moved By"
        };
        readonly String[] StockMovementByItemTableColumn = new String[]
        {
            "#", "Code", "Name","Batch", "Expiry Date", "Quantity", "Free"
        };
        readonly String[] StockMovementByLocationTableColumn = new String[]
        {
            "#","Date", "Reference", "Destination", "Quantity", "Free", "Moved By"
        };
        public DataTable DataGridViewAsDataTable(RptStockMovement RptStockMovement)
        {
            DataTable StockMovementDataTable = new DataTable();
            if (RptStockMovement.Type == StockMovementReportFilterType.BYDATE)
            {
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.SNO], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.DATE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.REF], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.SOURCE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.DESTINATION], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.QTY], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.FREE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.MOVEDBY], typeof(string));
                DataRow StockMovementDataTableRow = null;
                int rn = 0;
                String dateTime = null;

                foreach (StockMovementReportLine LineItem in RptStockMovement.StockMovementByDate.OrderBy(x => x.Date))
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    StockMovementDataTableRow = StockMovementDataTable.NewRow();

                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.SNO]] = rn + 1;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                    }
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.REF]] = LineItem.Reference;
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.SOURCE]] = LineItem.Source;
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.DESTINATION]] = LineItem.Destination;
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    StockMovementDataTableRow[StockMovementByDateTableColumn[(int)StockMovementReportByDateTableColumn.MOVEDBY]] = LineItem.MovedBy;
                    StockMovementDataTable.Rows.Add(StockMovementDataTableRow);
                    rn++;
                }
            }
            else if (RptStockMovement.Type == StockMovementReportFilterType.BYLOCATION)
            {
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.SNO], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.DATE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.REF], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.DESTINATION], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.QTY], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.FREE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.MOVEDBY], typeof(string));
                DataRow StockMovementDataTableRow = null;
                int rn = 0;
                string Location = string.Empty;
                String Date = null;
                foreach (StockMovementReportLine LineItem in RptStockMovement.StockMovementByDate.OrderBy(x => x.Source))
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    if (Location == string.Empty || Location != LineItem.Source)
                    {
                        StockMovementDataTableRow = StockMovementDataTable.NewRow();
                        StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.SNO]] = LineItem.Source;
                        StockMovementDataTable.Rows.Add(StockMovementDataTableRow);
                        Location = LineItem.Source;
                        rn = 0;
                    }
                    StockMovementDataTableRow = StockMovementDataTable.NewRow();

                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.SNO]] = rn + 1;
                    if (Date == null || Date != stringLineItemDate)
                    {
                        StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        Date = stringLineItemDate;
                    }
                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.REF]] = LineItem.Reference;
                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.DESTINATION]] = LineItem.Destination;
                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    StockMovementDataTableRow[StockMovementByLocationTableColumn[(int)StockMovementReportByLocationTableColumn.MOVEDBY]] = LineItem.MovedBy;
                    StockMovementDataTable.Rows.Add(StockMovementDataTableRow);
                    rn++;
                }
            }
            else if (RptStockMovement.Type == StockMovementReportFilterType.BYITEM)
            {
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.SNO], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.CODE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.NAME], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.BATCH], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.EXP_DATE], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.QUANTITY], typeof(string));
                StockMovementDataTable.Columns.Add(StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.FREE], typeof(string));
                DataRow DamageEntryDataTableRow = null;
                int rn = 0;
                foreach (StockMovementByItemReportLine LineItem in RptStockMovement.StockMovementByItems)
                {
                    DamageEntryDataTableRow = StockMovementDataTable.NewRow();
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.CODE]] = LineItem.Code;
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.SNO]] = rn + 1;
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.NAME]] = LineItem.Name;
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.BATCH]] = LineItem.BatchNo;
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.EXP_DATE]] = string.IsNullOrEmpty(LineItem.BatchNo) ? "" : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.QUANTITY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[StockMovementByItemTableColumn[(int)StockMovementReportByItemtTableColumn.FREE]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    StockMovementDataTable.Rows.Add(DamageEntryDataTableRow);
                    rn++;
                }
            }
            return StockMovementDataTable;
        }
        public void GeneratePDF(DataTable DataTable, RptStockMovement RptStockMovement, string ReportName, string fileExtension, bool isPrint)
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
                    ReportLine1 = RptStockMovement.ReportTitle(),
                    ReportLine2 = RptStockMovement.ReportSubTitle()

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
                    ReportLine1 = RptStockMovement.ReportTitle(),
                    ReportLine2 = RptStockMovement.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptStockMovement.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 40f, 30f, 40f, 40f, 30f, 30f, 40f };
                if (RptStockMovement.Type == StockMovementReportFilterType.BYITEM)
                {
                    widths = new float[] { 10f, 40f, 70f, 30f, 30f, 20f, 20f };
                }
                else if (RptStockMovement.Type == StockMovementReportFilterType.BYLOCATION)
                {
                    widths = new float[] { 20f, 40f, 30f, 50f, 30f, 30f, 40f };
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

                        if (DataTable.Columns[j].ColumnName == "Quantity" || DataTable.Columns[j].ColumnName == "Free")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (RptStockMovement.Type == StockMovementReportFilterType.BYLOCATION && j == (int)StockMovementReportByLocationTableColumn.SNO && string.IsNullOrEmpty(DataTable.Rows[i][j + 2].ToString()))
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                        }
                        if (RptStockMovement.Type == StockMovementReportFilterType.BYLOCATION && string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)StockMovementReportByLocationTableColumn.MOVEDBY)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            else if (j == (int)StockMovementReportByLocationTableColumn.SNO)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.BLACK;
                                RowCell.BorderColorRight = BaseColor.WHITE;
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
                PdfGeneration.FileName = RptStockMovement.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
