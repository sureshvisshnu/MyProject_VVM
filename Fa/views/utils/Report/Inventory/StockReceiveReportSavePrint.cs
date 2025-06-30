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
using fa;
using fa.api.utils;

namespace Fa.views.utils.Report.Inventory
{
    class StockReceiveReportSavePrint
    {
        public bool ExportOrPrintToFile(RptStockReceive RptStockReceive, string ReportName, string fileExtension, bool isPrint, int Receive)
        {
            if (RptStockReceive != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptStockReceive, Receive );
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptStockReceive, ReportName, "pdf", isPrint);
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

        static readonly String[] StockReceiveColumn = new String[]
        {
           "#", "Date", "Reference","Received","Source", "Quantity","Free","Received By"
        };
        static readonly String[] StockReceiveColumn1 = new String[]
        {
           "#","Code","Item Name","Batch Number","Exp_Date","Quantity","Free"
        };
        static readonly String[] StockReceiveColumn2 = new String[]
        {
           "#", "Date", "Reference","Source", "Quantity","Free" ,"Received By"
        };
        public DataTable DataGridViewAsDataTable(RptStockReceive RptStockReceive, int Recive)
        {
            DataTable PriceListTable = new DataTable();
            DataTable PriceListTable1 = new DataTable();
            DataTable PriceListTable2 = new DataTable();

            if (RptStockReceive.Type == RecevieType.BYDATE)
            {
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.SNO], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.DATE], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.REF], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.REQUEST], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.DESTINATION], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.QTY], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.FREE], typeof(string));
                PriceListTable.Columns.Add(StockReceiveColumn[(int)StockReciveReportByDateTableColumn.RECEVIEDBY], typeof(string));

                DataRow PriceListTableRow = null;
                int rn = 0;
                String dateTime = null;
                foreach (StockRecevieReportLine LineItem in RptStockReceive.StockRecevie)
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    PriceListTableRow = PriceListTable.NewRow();

                    PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.SNO]] = rn + 1;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                    }
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.REF]] = LineItem.Reference;
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.REQUEST]] = LineItem.RecevieLoaction;
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.DESTINATION]] = LineItem.ToLocation;
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        PriceListTableRow[StockReceiveColumn[(int)StockReciveReportByDateTableColumn.RECEVIEDBY]] = LineItem.CreatedBy;
                        PriceListTable.Rows.Add(PriceListTableRow);
                    rn++;
                }
                return PriceListTable;
            }
            else if (RptStockReceive.Type == RecevieType.BYLOCATION)
            {
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.SNO], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.DATE], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.REF], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.DESTINATION], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.QTY], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.FREE], typeof(string));
                PriceListTable2.Columns.Add(StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.RECEVIEDBY], typeof(string));

                DataRow PriceListTableRow2 = null;
                int rn = 0;
                string Location = string.Empty;
                string datetime = null;

                foreach (StockRecevieReportLine LineItem in RptStockReceive.StockRecevie.OrderBy(x => x.RecevieLoaction))
                {
                    if (Location == string.Empty || Location != LineItem.RecevieLoaction)
                    {
                        rn = 0;
                        PriceListTableRow2 = PriceListTable2.NewRow();
                        PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.SNO]] = LineItem.RecevieLoaction;
                        PriceListTable2.Rows.Add(PriceListTableRow2);
                        Location = LineItem.RecevieLoaction;
                    }
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    PriceListTableRow2 = PriceListTable2.NewRow();

                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.SNO]] = rn + 1;
                    if (datetime == null || datetime != stringLineItemDate)
                    {
                        PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        datetime = stringLineItemDate;
                    }
                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.REF]] = LineItem.Reference;
                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.DESTINATION]] = LineItem.ToLocation;
                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow2[StockReceiveColumn2[(int)StockReciveReportByLocationTableColumn.RECEVIEDBY]] = LineItem.CreatedBy;
                    PriceListTable2.Rows.Add(PriceListTableRow2);
                    rn++;
                }
                return PriceListTable2;
            }
            else
            {
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.SNO], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.ITEM], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.ITEM_NAME], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.BATCH_NUMBER], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.EXP_DATE], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.QUANTITY], typeof(string));
                PriceListTable1.Columns.Add(StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.FREE], typeof(string));

                DataRow PriceListTableRow1 = null;
                int rn1 = 0;

                foreach (StockRecevieByItemReportLine LineItem in RptStockReceive.StockRecevieByItems)
                {
                    PriceListTableRow1 = PriceListTable1.NewRow();
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.SNO]] = rn1 + 1;
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.ITEM]] = LineItem.Code;
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.ITEM_NAME]] = LineItem.Name;
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.BATCH_NUMBER]] = LineItem.BatchNo;
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.EXP_DATE]] = LineItem.ExpDate.ToString(Global.Company.DateFormat);
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.QUANTITY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTableRow1[StockReceiveColumn1[(int)StockReciveReportByItemTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PriceListTable1.Rows.Add(PriceListTableRow1);
                    rn1++;
                }
            }
           
            return PriceListTable1;
        }
        public void GeneratePDF(DataTable DataTable, RptStockReceive RptStockReceive, string ReportName, string fileExtension, bool isPrint)
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
                    ReportLine1 = RptStockReceive.ReportTitle(),
                    ReportLine2 = RptStockReceive.ReportSubTitle()

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
                    ReportLine1 = RptStockReceive.ReportTitle(),
                    ReportLine2 = RptStockReceive.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptStockReceive.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 30f, 20f, 40f, 40f, 20f, 30f, 30f };
                if (RptStockReceive.Type == RecevieType.BYITEM)
                {
                    widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f, 30f };
                }
                else if (RptStockReceive.Type == RecevieType.BYLOCATION)
                {
                    widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f, 30f };
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
                        if (RptStockReceive.Type == RecevieType.BYLOCATION && j == (int)StockReciveReportByLocationTableColumn.SNO && string.IsNullOrEmpty(DataTable.Rows[i][j + 2].ToString()))
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                        }
                        if (RptStockReceive.Type == RecevieType.BYLOCATION && string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)StockReciveReportByLocationTableColumn.RECEVIEDBY)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            else if (j == (int)StockReciveReportByLocationTableColumn.SNO)
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
                PdfGeneration.FileName = RptStockReceive.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}

