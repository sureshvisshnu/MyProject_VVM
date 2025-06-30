using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.api.utils;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Fa.views.utils.Report.Inventory
{
    public class DamageEntryReportSavePrint
    {
        public bool ExportOrPrintToFile(RptDamageEntry RptDamageEntry, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptDamageEntry != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptDamageEntry);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptDamageEntry, ReportName, "pdf", isPrint);
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
        readonly String[] DamageReportByDateTableColumn = new String[]
        {
            "#","Date", "Reference","Location", "Quantity", "Free", "Entered By"
        };
        readonly String[] DamageReportByItemTableColumn = new String[]
        {
            "#", "Code", "Name","Batch", "Expiry Date", "Quantity", "Free"
        };
        readonly String[] DamageReportByLocationTableColumn = new String[]
        {
            "#","Date", "Reference", "Quantity", "Free", "Entered By"
        };
        public DataTable DataGridViewAsDataTable(RptDamageEntry RptDamageEntry)
        {
            DataTable DamageEntryDataTable = new DataTable();
            if (RptDamageEntry.Type == DamageEntryReportFilterType.BYDATE)
            {
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.SNO], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.DATE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.REFERENCE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.LOCATION], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.QTY], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.FREE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.ENTEREDBY], typeof(string));
                DataRow DamageEntryDataTableRow = null;
                int rn = 0;
                String dateTime = null;

                foreach (DamageEntryReportLine LineItem in RptDamageEntry.DamageEntry.OrderBy(x => x.Date))
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    DamageEntryDataTableRow = DamageEntryDataTable.NewRow();

                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.SNO]] = rn + 1;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                    }
                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.REFERENCE]] = LineItem.Reference;
                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.LOCATION]] = LineItem.Location;
                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.QTY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[DamageReportByDateTableColumn[(int)DamageEntryReportByDateTableColumn.ENTEREDBY]] = LineItem.EnteredBy;
                    DamageEntryDataTable.Rows.Add(DamageEntryDataTableRow);
                    rn++;
                }
            }
            else if (RptDamageEntry.Type == DamageEntryReportFilterType.BYLOCATION)
            {
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.SNO], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.DATE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.REFERENCE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.QTY], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.FREE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.ENTEREDBY], typeof(string));
                DataRow DamageEntryDataTableRow = null;
                int rn = 0;
                string Location = string.Empty;
                String Date = null;
                foreach (DamageEntryReportLine LineItem in RptDamageEntry.DamageEntry.OrderBy(x => x.Location))
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    if (Location == string.Empty || Location != LineItem.Location)
                    {
                        DamageEntryDataTableRow = DamageEntryDataTable.NewRow();
                        DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.SNO]] = LineItem.Location;
                        DamageEntryDataTable.Rows.Add(DamageEntryDataTableRow);
                        Location = LineItem.Location;
                        rn = 0;
                    }
                    DamageEntryDataTableRow = DamageEntryDataTable.NewRow();

                    DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.SNO]] = rn + 1;
                    if (Date == null || Date != stringLineItemDate)
                    {
                        DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                        Date = stringLineItemDate;
                    }
                    DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.REFERENCE]] = LineItem.Reference;
                    DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.QTY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[DamageReportByLocationTableColumn[(int)DamageEntryReportByLocationTableColumn.ENTEREDBY]] = LineItem.EnteredBy;
                    DamageEntryDataTable.Rows.Add(DamageEntryDataTableRow);
                    rn++;
                }
            }
            else if (RptDamageEntry.Type == DamageEntryReportFilterType.BYITEM)
            {
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.SNO], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.CODE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.NAME], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.BATCH], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.EXP_DATE], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.QTY], typeof(string));
                DamageEntryDataTable.Columns.Add(DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.FREE], typeof(string));
                DataRow DamageEntryDataTableRow = null;
                int rn = 0;
                foreach (DamageEntryByItemReportLine LineItem in RptDamageEntry.DamageEntryByItems)
                {
                    DamageEntryDataTableRow = DamageEntryDataTable.NewRow();
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.CODE]] = LineItem.Code;
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.SNO]] = rn + 1;
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.NAME]] = LineItem.Name;
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.BATCH]] = LineItem.BatchNo;
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.EXP_DATE]] = string.IsNullOrEmpty(LineItem.BatchNo) ? "" : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.QTY]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTableRow[DamageReportByItemTableColumn[(int)DamageEntryReportByItemTableColumn.FREE]] = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    DamageEntryDataTable.Rows.Add(DamageEntryDataTableRow);
                    rn++;
                }
            }
            return DamageEntryDataTable;
        }
        public void GeneratePDF(DataTable DataTable, RptDamageEntry RptDamageEntry, string ReportName, string fileExtension, bool isPrint)
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
                    ReportLine1 = RptDamageEntry.ReportTitle(),
                    ReportLine2 = RptDamageEntry.ReportSubTitle()

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
                    ReportLine1 = RptDamageEntry.ReportTitle(),
                    ReportLine2 = RptDamageEntry.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptDamageEntry.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 40f, 30f, 40f, 30f, 30f, 40f };
                if (RptDamageEntry.Type == DamageEntryReportFilterType.BYITEM)
                {
                    widths = new float[] { 10f, 40f, 70f, 30f, 30f, 20f, 20f };
                }
                else if (RptDamageEntry.Type == DamageEntryReportFilterType.BYLOCATION)
                {
                    widths = new float[] { 10f, 40f, 30f, 30f, 30f, 40f };
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
                        if (RptDamageEntry.Type == DamageEntryReportFilterType.BYLOCATION && j == (int)DamageEntryReportByLocationTableColumn.SNO && string.IsNullOrEmpty(DataTable.Rows[i][j + 2].ToString()))
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorRight = BaseColor.WHITE;
                        }
                        if (RptDamageEntry.Type == DamageEntryReportFilterType.BYLOCATION && string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)DamageEntryReportByLocationTableColumn.ENTEREDBY)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            else if (j == (int)DamageEntryReportByLocationTableColumn.SNO)
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
                PdfGeneration.FileName = RptDamageEntry.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
