using fa.api.utils;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Report.Inventory
{
    class StockRequestReportSavePrint
    {
        public bool ExportOrPrintToFile(RptStockRequest RptStockRequest, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptStockRequest != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptStockRequest);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptStockRequest, ReportName, "pdf", isPrint);
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
        readonly String[] StockRequestColumn = new String[]
        {
            "#","Date", "Reference","Request","Destination", "Quantity", "Created By"
        };
        readonly String[] StockRequestByLocationColumn = new String[]
        {
            "#","Date", "Reference","Destination", "Quantity", "Created By"
        };
        readonly String[] StockRequestByItemColumn = new String[]
        {
            "#","Code", "Name","Batch No", "Exp Date", "Quantity"
        };
        public DataTable DataGridViewAsDataTable(RptStockRequest RptStockRequest)
        {
            DataTable PriceListTable = new DataTable();
            if (RptStockRequest.Type == InventoryReportFilterType.BYDATE)
            {
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.DATE], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.REF], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.REQUEST], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.DESTINATION], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.QTY], typeof(string));
                PriceListTable.Columns.Add(StockRequestColumn[(int)StockRequestReportTableColumn.CREATEDBY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                DateTime? dateTime = null;

                foreach (StockRequestReportLine LineItem in RptStockRequest.StockRequests)
                {
                    PriceListTableRow = PriceListTable.NewRow();
                    if (dateTime == null || ((DateTime)dateTime).Date != LineItem.Date)
                    {
                        PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        dateTime = LineItem.Date;
                    }
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.REF]] = LineItem.Reference;
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.REQUEST]] = LineItem.RequestLoaction;
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.DESTINATION]] = LineItem.ToLocation;
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow[StockRequestColumn[(int)StockRequestReportTableColumn.CREATEDBY]] = LineItem.CreatedBy;
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            else if (RptStockRequest.Type == InventoryReportFilterType.BYLOCATION)
            {
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.DATE], typeof(string));
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.REF], typeof(string));
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.DESTINATION], typeof(string));
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.QTY], typeof(string));
                PriceListTable.Columns.Add(StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.CREATEDBY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                string Location = string.Empty;

                foreach (StockRequestReportLine LineItem in RptStockRequest.StockRequests)
                {
                    if (Location == string.Empty || Location != LineItem.RequestLoaction)
                    {
                        PriceListTableRow = PriceListTable.NewRow();
                        PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.SNO]] = LineItem.RequestLoaction;
                        PriceListTable.Rows.Add(PriceListTableRow);
                        Location = LineItem.RequestLoaction;
                        rn = 0;
                    }
                    PriceListTableRow = PriceListTable.NewRow();
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.REF]] = LineItem.Reference;                    
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.DESTINATION]] = LineItem.ToLocation;
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow[StockRequestByLocationColumn[(int)StockRequestReportByLocationTableColumn.CREATEDBY]] = LineItem.CreatedBy;
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            else if (RptStockRequest.Type == InventoryReportFilterType.BYITEM)
            {
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.ITEM], typeof(string));
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.ITEM_NAME], typeof(string));
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.BATCH_NUMBER], typeof(string));
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.EXP_DATE], typeof(string));
                PriceListTable.Columns.Add(StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.QUANTITY], typeof(string));
                DataRow PriceListTableRow = null;
                int rn = 0;
                foreach (StockRequestByItemReportLine LineItem in RptStockRequest.StockRequestByItems)
                {
                    PriceListTableRow = PriceListTable.NewRow();
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.ITEM]] = LineItem.Code;
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.SNO]] = rn + 1;
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.ITEM_NAME]] = LineItem.Name;
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.BATCH_NUMBER]] = LineItem.BatchNo;
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.EXP_DATE]] =string.IsNullOrEmpty(LineItem.BatchNo)?"": LineItem.ExpDate.ToString(Global.Company.DateFormat);
                    PriceListTableRow[StockRequestByItemColumn[(int)StockRequestReporByItemtTableColumn.QUANTITY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
            }
            return PriceListTable;
        }
        public void GeneratePDF(DataTable DataTable, RptStockRequest RptStockRequest,string ReportName, string fileExtension, bool isPrint)
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
                    ReportLine1 = RptStockRequest.ReportTitle(),
                    ReportLine2 = RptStockRequest.ReportSubTitle()

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
                    ReportLine1 = RptStockRequest.ReportTitle(),
                    ReportLine2 = RptStockRequest.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptStockRequest.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f,30f, 20f, 40f, 40f, 20f, 30f };
                if(RptStockRequest.Type== InventoryReportFilterType.BYITEM)
                {
                    widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f };
                }
                else if (RptStockRequest.Type == InventoryReportFilterType.BYLOCATION)
                {
                    widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f };
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
                        if (RptStockRequest.Type == InventoryReportFilterType.BYLOCATION && j == (int)StockRequestReportByLocationTableColumn.SNO &&  string.IsNullOrEmpty(DataTable.Rows[i][j+2].ToString()))
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.BLACK;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                        }
                        if (RptStockRequest.Type == InventoryReportFilterType.BYLOCATION && string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)StockRequestReportByLocationTableColumn.CREATEDBY)
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
                PdfGeneration.FileName = RptStockRequest.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
