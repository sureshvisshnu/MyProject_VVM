using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa;
using fa.report.Hms;
using fa.report.Ip;
using fa.views.utils.Common;
using fa.views.utils;
using Fa.reports.Hms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using VisioForge.Libs.Accord;
using static fa.report.Hms.RptWardAndBed;
using fa.api.utils;

namespace Fa.views.utils.Report.Hms
{
    class WardBedReportSavePrint
    {
        public bool SaveOrPrintToFile(DataGridView dataGridView, RptWardAndBed RptWardAndBed, string fileExtension, bool isPrint)
        {
            if (dataGridView.Rows.Count != 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptWardAndBed);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptWardAndBed, fileExtension, isPrint, RptWardAndBed.FromDate.ToString(Global.Company.DateFormat), RptWardAndBed.ToDate.ToString(Global.Company.DateFormat));
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

        readonly String[] WardBedDataTableColumn = new String[]
        {
            "Ward","BedName", "BedType", "RentPeriod", "Rent", "AdmittedOn", "IPNumber", "PatientName", "Available"
        };

        public DataTable DataGridViewAsDataTable(RptWardAndBed RptWardAndBed)
        {
            DataTable WardBedTable = new DataTable();
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.Ward], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.BedNumber], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.BedType], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.RentType], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.Rent], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.AdmittedOn], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.IPNumber], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.PatientName], typeof(string));
            WardBedTable.Columns.Add(WardBedDataTableColumn[(int)WardBedReportGridColumn.Available], typeof(string));

            DataRow WardBedTableRow = null;
            foreach (WardBedReport LineItem in RptWardAndBed.LineItems)
            {
                WardBedTableRow = WardBedTable.NewRow();
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.Ward]] = LineItem.Ward;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.BedNumber]] = LineItem.BedNumber;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.BedType]] = LineItem.BedType;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.RentType]] = LineItem.RentType;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.Rent]] = LineItem.Rent.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.AdmittedOn]] = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.Date.ToString(Global.Company.DateFormat);
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.IPNumber]] = LineItem.IPNumber;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.PatientName]] = LineItem.PatientName;
                WardBedTableRow[WardBedDataTableColumn[(int)WardBedReportGridColumn.Available]] = LineItem.Available;
                WardBedTable.Rows.Add(WardBedTableRow);
            }
            return WardBedTable;
        }
        public void GeneratePDF(DataTable DataTable, RptWardAndBed RptWardAndBed, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = RptWardAndBed.ReportTitle(),
                    ReportLine2 = RptWardAndBed.ReportSubTitle()
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
                    ReportLine1 = RptWardAndBed.ReportTitle(),
                    ReportLine2 = RptWardAndBed.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "RptWardAndBed");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 30f, 25f, 20f, 30f, 20f, 40f, 30f, 40f, 20f };
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
                        if (j == 4)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
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
                PdfGeneration.FileName = RptWardAndBed.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
