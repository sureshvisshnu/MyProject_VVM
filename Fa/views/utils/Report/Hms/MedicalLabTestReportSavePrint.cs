using DocumentFormat.OpenXml.Wordprocessing;
using fa.api.utils;
using fa.reports.Inventory;
using fa.views.utils.Common;
using Fa.reports.Hms;
using FADataAccessLibrary.report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using Pango;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static FADataAccessLibrary.report.Hms.RptLabTestDetails;
using Document = iTextSharp.text.Document;
using Element = iTextSharp.text.Element;
using Font = iTextSharp.text.Font;
using PageSize = iTextSharp.text.PageSize;

namespace fa.views.utils.Report.Hms
{
    class MedicalLabTestReportSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, RptLabTestDetails LabTestReport, string fileExtension, bool isPrint, int RType)
        {
            RptLabTestDetails ReporptLabTestDetails = new RptLabTestDetails();
            if (ReportGridView.Rows.Count != 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(LabTestReport, RType);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, LabTestReport, fileExtension, isPrint, LabTestReport.FromDate.ToString(Global.Company.DateFormat), LabTestReport.ToDate.ToString(Global.Company.DateFormat), RType);
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
        readonly String[] LabTestDataTableColumn = new String[]
        {
            "Date", "PatientId", "Patient Name", "Test Name", "Element", "Uom", "Class", "SubClass", "Single Value", "Range From", "Range To", "Result"
        };

        public DataTable DataGridViewAsDataTable(RptLabTestDetails LabTestReport, int RType)
        {
            DataTable LabTestTable = new DataTable();
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTID], typeof(string));

            if (RType == 0 || RType == 2)
            {
                LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTNAME], typeof(string));
            }

            if (RType == 0 || RType == 1)
            {
                LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.TESTNAME], typeof(string));
            }

            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.ELEMENT], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.UOM], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.CLASS], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.SUBCLASS], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.SINGLEVALUE], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGEFROM], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGETO], typeof(string));
            LabTestTable.Columns.Add(LabTestDataTableColumn[(int)LabTestReportTableColumn.RESULT], typeof(string));

            DataRow LabTestTableRow = null!;
            string LabTestDate = string.Empty;
            string PName = string.Empty;
            string TName = string.Empty;
            string prevPatientId = string.Empty;

            // OrderBy based on RType
            var orderedLineItems = RType switch
            {
                0 => LabTestReport.LineLabTestReport.OrderBy(t => t.Date).ThenBy(x => x.Patient),
                1 => LabTestReport.LineLabTestReport.OrderBy(t => t.Patient).ThenBy(x => x.Date),
                2 => LabTestReport.LineLabTestReport.OrderBy(t => t.TestName).ThenBy(x => x.Date),
                _ => throw new ArgumentException("Invalid RType value")
            };

            foreach (RptLabTestLineItem LineItem in orderedLineItems)
            {
                if (RType == 1 && PName != LineItem.Patient)
                {
                    LabTestTableRow = LabTestTable.Rows.Add();
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = "Patient Name : " + LineItem.Patient;
                    PName = LineItem.Patient;
                }
                if (RType == 2 && TName != LineItem.TestName)
                {
                    LabTestTableRow = LabTestTable.Rows.Add();
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = "Test Name : " + LineItem.TestName;
                    TName = LineItem.TestName;
                }

                LabTestTableRow = LabTestTable.Rows.Add();
                if (LabTestDate != LineItem.Date.ToString(Global.Company.DateFormat))
                {
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                    if (RType == 1 && (PName == LineItem.Patient && LabTestDate == LineItem.Date.ToString(Global.Company.DateFormat)))
                    {
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = "";
                    }
                    if (RType == 2 && (TName == LineItem.TestName && LabTestDate == LineItem.Date.ToString(Global.Company.DateFormat)))
                    {
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = "";
                    }
                }
                if (RType == 0)
                {
                    if (prevPatientId != LineItem.PatientId || LabTestDate != LineItem.Date.ToString(Global.Company.DateFormat))
                    {
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTID]] = (LineItem.PatientId != null) ? LineItem.PatientId : string.Empty;
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTNAME]] = (LineItem?.Patient != null) ? LineItem.Patient : string.Empty;
                    }
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.TESTNAME]] = (LineItem?.TestName != null) ? LineItem.TestName : string.Empty;
                }
                else if (RType == 1)
                {
                    if (prevPatientId != LineItem.PatientId || LabTestDate != LineItem.Date.ToString(Global.Company.DateFormat))
                    {
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTID]] = (LineItem.PatientId != null) ? LineItem.PatientId : string.Empty;
                    }
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.TESTNAME]] = (LineItem?.TestName != null) ? LineItem.TestName : string.Empty;
                }
                else if (RType == 2)
                {
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTID]] = (LineItem.PatientId != null) ? LineItem.PatientId : string.Empty;
                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTNAME]] = (LineItem?.Patient != null) ? LineItem.Patient : string.Empty;
                }

                LabTestDate = LineItem!.Date.ToString(Global.Company.DateFormat);
                PName = LineItem.Patient;
                TName = LineItem.TestName;
                prevPatientId = LineItem.PatientId;
                // Initialize elementRow outside the loop
                if (LineItem != null)
                {
                    if (LineItem.HasElemnet)
                    {
                        int elementcount = 0;
                        foreach (var element in LineItem.Elements)
                        {
                            if (elementcount == 0)
                            {
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.ELEMENT]] = (element?.ElementName != null) ? element.ElementName : string.Empty;
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.UOM]] = (element?.UOM != null) ? element.UOM : string.Empty;
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.CLASS]] = (element?.Class != null) ? element.Class.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SUBCLASS]] = (element?.SubClass != null) ? element.SubClass.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SINGLEVALUE]] = (element?.SingleValue != null) ? element.SingleValue.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGEFROM]] = (element?.RangeFrom != null) ? element.RangeFrom.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGETO]] = (element?.RangeTo != null) ? element.RangeTo.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RESULT]] = (element?.ResultDescription != null) ? element.ResultDescription.ToString() : "";
                            }
                            else
                            {
                                LabTestTableRow = LabTestTable.Rows.Add();
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.DATE]] = string.Empty;
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTID]] = string.Empty;
                                if (RType == 0 || RType == 1)
                                {
                                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.TESTNAME]] = "";
                                }
                                if (RType == 0 || RType == 2)
                                {
                                    LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.PATIENTNAME]] = string.Empty;
                                }
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.ELEMENT]] = (element?.ElementName != null) ? element.ElementName : string.Empty;
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.UOM]] = (element?.UOM != null) ? element.UOM : string.Empty;
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.CLASS]] = (element?.Class != null) ? element.Class.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SUBCLASS]] = (element?.SubClass != null) ? element.SubClass.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SINGLEVALUE]] = (element?.SingleValue != null) ? element.SingleValue.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGEFROM]] = (element?.RangeFrom != null) ? element.RangeFrom.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGETO]] = (element?.RangeTo != null) ? element.RangeTo.ToString() : "";
                                LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RESULT]] = (element?.ResultDescription != null) ? element.ResultDescription.ToString() : "";
                            }
                            elementcount++;
                        }
                    }
                    else
                    {
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.CLASS]] = "";
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SUBCLASS]] = "";
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.SINGLEVALUE]] = "";
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGEFROM]] = "";
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RANGETO]] = "";
                        LabTestTableRow[LabTestDataTableColumn[(int)LabTestReportTableColumn.RESULT]] = "";
                    }
                }
            }
            return LabTestTable;
        }
        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, RptLabTestDetails LTReport, string fileExtension, bool isPrint, string fromDate, string toDate, int reportType)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -45, -45, 20, 40);
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
                    ReportLine1 = LTReport.ReportTitle(),
                    ReportLine2 = LTReport.ReportSubTitle()
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
                    ReportLine1 = LTReport.ReportTitle(),
                    ReportLine2 = LTReport.ReportSubTitle()
                };

                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "LabTestReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;

                if (reportType == 1)
                {
                    widths = new float[] { 20f, 25f, 35f, 35f, 20f, 20f, 15f, 20f, 20f, 20f, 20f };
                    SetColumnVisibility(ReportMainTable, (int)LabTestReportTableColumn.PATIENTNAME, false);
                }
                else if (reportType == 2)
                {
                    widths = new float[] { 20f, 25f, 35f, 35f, 20f, 20f, 15f, 20f, 20f, 20f, 20f };
                    SetColumnVisibility(ReportMainTable, (int)LabTestReportTableColumn.TESTNAME, false);
                }
                else
                {
                    widths = new float[] { 18f, 20f, 30f, 30f, 30f, 18f, 15f, 15f, 18f, 18f, 18f, 18f };
                }

                ReportMainTable.SetWidths(widths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                // ---
                
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
                        ReportMainTable = new PdfPTable(Global.Company.MaintainRackNumber ? DataTable.Columns.Count : DataTable.Columns.Count);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, FNIB7Font));
                        //RowCell.BorderWidth = 0.5f;
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BorderWidth = 0.25f;
                        RowCell.BackgroundColor = CurRowColor;
                        string columnName = DataTable.Columns[j].ColumnName;
                        if (columnName == "Date" || columnName == "PatientId" || columnName == "Patient Name" || columnName == "Test Name" || columnName == "Element" || columnName == "Uom")
                        {
                            if(j == 0)
                            {
                                if ((reportType == 1 || reportType == 2) && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][3].ToString()))
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_TOP;
                                    //RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                                else
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                            }                                
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                        }
                        if (reportType != 0 && j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][3].ToString()))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (reportType != 0 && j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(DataTable.Rows[i][3].ToString()))
                        {
                            continue;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                //---
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
                PdfGeneration.FileName = LTReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
        private static void addCellWithColSpan(PdfPTable table, string text, int colspan, bool colorStatus, bool alignmentStatus, bool fontStatus)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, FNIB7Font));
            cell.Colspan = colspan;
            if (alignmentStatus == false)
            {
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            }
            else
            {
                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            }
            if (colorStatus == true)
            {
                cell.BackgroundColor = new BaseColor(179, 179, 179);
            }
            table.AddCell(cell);
        }
        private void SetColumnVisibility(PdfPTable table, int columnIndex, bool isVisible)
        {
            foreach (PdfPRow row in table.Rows)
            {
                if (row.GetCells().Length > columnIndex)
                {
                    PdfPCell cell = row.GetCells()[columnIndex];
                    cell.Colspan = isVisible ? 1 : 0;
                }
            }
        }
    }
}
