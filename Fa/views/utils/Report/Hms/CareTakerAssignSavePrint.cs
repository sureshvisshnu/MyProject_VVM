using fa.report.Ip;
using fa.reports.Hms;
using fa.views.utils;
using fa.views.utils.Common;
using FADataAccessLibrary.report.Hms;
using Gst;
using Gst.Rtp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Element = iTextSharp.text.Element;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Report.Hms
{
    class CareTakerAssignSavePrint
    {   
        public bool ExportOrPrintToFile(DataGridView ReportGridView, RptCareTakerAssign CareTakerReport, string fileExtension, bool isPrint, int RType)
        {
            if (ReportGridView.Rows.Count != 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(CareTakerReport, RType);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, CareTakerReport, fileExtension, isPrint, CareTakerReport.FromDate.ToString(Global.Company.DateFormat), CareTakerReport.ToDate.ToString(Global.Company.DateFormat), RType);
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
        enum CareTakerReportTableColumn
        {
            FROM, TO, DEPT, PDOCTOR, SDOCTOR, PNURSE, SNURSE, NOTE, ADOCTOR, AID
        }
        readonly String[] CareTakerDataTableColumn = new String[]
        {
            "From", "To", "Department", "Prim. Dr", "Sec. Dr", "Prim. Nurse", "Sec. Nurse", "Notes", "Authorized By", "AID"
        };
        public DataTable DataGridViewAsDataTable(RptCareTakerAssign CareTakerAssignReport, int RType)
        {
            DataTable CareTakerTable = new DataTable();
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.FROM], typeof(string));
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.TO], typeof(string));
            if (RType != 2)
            {
                CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.DEPT], typeof(string));
            }
            if (RType != 1)
            {
                CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.PDOCTOR], typeof(string));
            }
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.SDOCTOR], typeof(string));
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.PNURSE], typeof(string));
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.SNURSE], typeof(string));
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.NOTE], typeof(string));
            CareTakerTable.Columns.Add(CareTakerDataTableColumn[(int)CareTakerReportTableColumn.ADOCTOR], typeof(string));
            
            DataRow CareTakerTableRow = null;
            string Consultant = string.Empty;
            string DepartmentOfConsultant = string.Empty;
            foreach (CareTakerReport LineItem in CareTakerAssignReport.LineCareTakerReport)
            {
                if (RType == 1)
                {
                    if (Consultant != LineItem.PrimaryDr)
                    {
                        CareTakerTableRow = CareTakerTable.Rows.Add();
                        CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.FROM]] = "Consultant : " + LineItem.PrimaryDr;
                        Consultant = LineItem.PrimaryDr;
                    }
                }
                if (RType == 2)
                {
                    if (Consultant != LineItem.PrimaryCT)
                    {
                        CareTakerTableRow = CareTakerTable.Rows.Add();
                        CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.FROM]] = "Nurse : " + LineItem.PrimaryCT;
                        Consultant = LineItem.PrimaryCT;
                    }
                }
                if (RType == 4)
                {
                    if (DepartmentOfConsultant != LineItem.DepartmentOfConsultant)
                    {
                        CareTakerTableRow = CareTakerTable.Rows.Add();
                        CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.FROM]] = "Department : " + LineItem.DepartmentOfConsultant;
                        DepartmentOfConsultant = LineItem.DepartmentOfConsultant;
                    }
                }
                CareTakerTableRow = CareTakerTable.Rows.Add();
                {
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.FROM]] = (LineItem.From != System.DateTime.MinValue) ? LineItem.From.Date.ToString(Global.Company.DateFormat) : string.Empty;
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.TO]] = (LineItem.To != System.DateTime.MinValue) ? LineItem.To.Date.ToString(Global.Company.DateFormat) : "n/a";
                    if (RType != 2)
                    {
                        CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.DEPT]] = (LineItem?.DepartmentOfConsultant != null) ? LineItem.DepartmentOfConsultant : string.Empty;
                    }
                    if (RType != 1)
                    {
                        CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.PDOCTOR]] = (LineItem?.PrimaryDr != null) ? LineItem.PrimaryDr : string.Empty;
                    }
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.SDOCTOR]] = (LineItem?.SecondaryDr != null) ? LineItem.SecondaryDr : string.Empty;
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.PNURSE]] = (LineItem?.PrimaryCT != null) ? LineItem.PrimaryCT : string.Empty;
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.SNURSE]] = (LineItem?.SecondaryCT != null) ? LineItem.SecondaryCT : string.Empty;
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.NOTE]] = (LineItem?.Notes != null) ? LineItem.Notes : string.Empty;
                    CareTakerTableRow[CareTakerDataTableColumn[(int)CareTakerReportTableColumn.ADOCTOR]] = (LineItem?.AuthorizedDr != null) ? LineItem.AuthorizedDr : string.Empty;
                }
            }
            return CareTakerTable;
        }
        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, RptCareTakerAssign CTReport, string fileExtension, bool isPrint, string fromDate, string toDate, int reportType)
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
                    ReportLine1 = CTReport.ReportTitle(),
                    ReportLine2 = CTReport.ReportSubTitle()
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
                    ReportLine1 = CTReport.ReportTitle(),
                    ReportLine2 = CTReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "CareTakerReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;

                if (reportType == 1 || reportType == 2 )
                {
                    widths = new float[] { 28f, 28f, 22f, 22f, 22f, 22f, 25f, 22f };
                    SetColumnVisibility(ReportMainTable, (int)CareTakerReportTableColumn.FROM, false);
                }
                else
                {
                    widths = new float[] { 28f, 28f, 22f, 22f, 22f, 22f, 22f, 25f, 22f };
                }

                ReportMainTable.SetWidths(widths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                string previousValue = string.Empty;

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
                        previousValue = string.Empty;
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, FNIB7Font));
                        if (Temp != null && (Temp.Equals("N/A", StringComparison.OrdinalIgnoreCase) || Temp.Equals("n/a", StringComparison.OrdinalIgnoreCase)))
                        {
                            RowCell = new PdfPCell(new Phrase("", FNIB7Font));
                        }
                       
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BorderWidth = 0.25f;
                        RowCell.BackgroundColor = CurRowColor;
                        if (j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "From" )
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if(reportType != 0)
                        {
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                        }
                        if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                        {
                            continue;
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
                PdfGeneration.FileName = CTReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
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
