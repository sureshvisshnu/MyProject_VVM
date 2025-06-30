using fa.model.Hms.Master;
using fa.views.controls;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.reports.Hms;
using FADataAccessLibrary.report.Hms;
using Gnostice.Documents.DOC;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.utils.Report.Hms
{
    public class PatientDetailsReportPrintAndSave
    {
        public bool ExportOrPrintToFile(DataViewVerticalScroll GridviewPatientDetails, RptPatientDetails rptPatientDetails, string ReportName, string ReportHeading, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            if (GridviewPatientDetails.Rows.Count > 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(rptPatientDetails);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, ReportName, ReportHeading, isPrint, FromDate, Todate);
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

        static readonly String[] PatientDetailsColumn = new String[]
        {
            "SNo", "Patient Name", "Patient ID", "Gender", "DOB", "Age", "Blood Group", "Address", "Phone/Mobile", "Gaurdian Details", "Emergency Contact"
        };

        private static DataTable DataGridViewAsDataTable(RptPatientDetails rptPatientDetails)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.NAME], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.PATIENTID], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.GENDER], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.DOB], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.AGE], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.BLOODGROUP], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.PADDRESS], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.PHONE], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.GAURDIANDETAIL], typeof(string));
            dataTable.Columns.Add(PatientDetailsColumn[(int)PatientDetailsTableColumn.EMERGENCYCONTACT], typeof(string));
            DataRow PatientDetailstRow = null!;

            int i = 1;
            string EntryDate = DateTime.Now.ToString();
            foreach (RptPatientDetailsLineItems lineItem in rptPatientDetails.PatientDetailsLineItems)
            {
                if (EntryDate != lineItem.Date)
                {
                    PatientDetailstRow = dataTable.NewRow();
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.SNO]] = "Registration Date : " + lineItem.Date;
                    EntryDate = lineItem.Date;
                    dataTable.Rows.Add(PatientDetailstRow);
                    i = 1;
                }
                PatientDetailstRow = dataTable.NewRow();
                if (lineItem.PatientName != null)
                {
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.SNO]] = i;
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.NAME]] = lineItem?.PatientName ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.PATIENTID]] = lineItem?.PatientID ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.GENDER]] = lineItem?.Gender?.ToString() ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.DOB]] = lineItem?.DOB != null ? lineItem?.DOB : "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.AGE]] = lineItem?.Age ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.BLOODGROUP]] = lineItem?.BloodGroup ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.PADDRESS]] = lineItem?.PAddress ?? "";
                    PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.PHONE]] = lineItem?.Mobile ?? "";
                    i++;
                }
                PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.GAURDIANDETAIL]] = lineItem?.GAddress ?? "";
                PatientDetailstRow[PatientDetailsColumn[(int)PatientDetailsTableColumn.EMERGENCYCONTACT]] = lineItem?.EAddress ?? "";
                dataTable.Rows.Add(PatientDetailstRow);
            }
            return dataTable;
        }

        public void GeneratePDF(DataTable dataTable, string fileName, string heading, bool isPrint, string FromDate, string Todate)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 540;
                Document pdfDoc = new Document(PageSize.A4.Rotate(), -55, -55, 30, 0);
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
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "PatientDetailsReport");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(11);
                float[] widths = new float[] { 13f, 48f, 22f, 20f, 18f, 10f, 28f, 50f, 25f, 50f, 50f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
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
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        if (j != 10)
                        {
                            RowCell.BorderWidthRight = (float)BorderStyle.None;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        if (j == 10)
                        {
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BorderWidthRight = 0.4f;
                        }
                        if (j == 5)
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()) && string.IsNullOrEmpty(dataTable.Rows[i][10].ToString()))
                        {
                            RowCell.Colspan = dataTable.Columns.Count;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthRight = 0.4f;
                            RowCell.BorderWidthTop = 0.4f;
                            RowCell.BackgroundColor = new BaseColor(200, 200, 200);
                        }
                        if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][1].ToString()) && string.IsNullOrEmpty(dataTable.Rows[i][9].ToString()) && string.IsNullOrEmpty(dataTable.Rows[i][10].ToString()))
                        {
                            continue;
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                }
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

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName;
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
