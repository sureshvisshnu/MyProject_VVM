using fa.views.utils.Common;
using FADataAccessLibrary.report.Hms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = iTextSharp.text.Font;
using SharpCompress;

namespace fa.views.utils.Report.Hms
{
    class CareTakerUnAssignSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, RptCareTakerUnAssign CareTakerReport, string fileExtension, bool isPrint, int RType)
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
        enum CareTakerUnAssignReportTableColumn
        {
            SLNO, NAME, AGE, ADDRESS, DEPT, ADATE, TASK, AID
        }
        readonly String[] CareTakerUnAssignDataTableColumn = new String[]
        {
            "Sl. No", "Name", "Age", "Address", "Department", "Task Status", "Authorized Dr", "AID"
        };
        public DataTable DataGridViewAsDataTable(RptCareTakerUnAssign CareTakerUnAssignReport, int RType)
        {
            DataTable CareTakerTable = new DataTable();

            CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.SLNO], typeof(string));
            if (RType != 1)
            {
                CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.NAME], typeof(string));
            }            
            CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.AGE], typeof(string));
            CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.ADDRESS], typeof(string));
            if (RType != 2)
            {
                CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.DEPT], typeof(string));
            }
            CareTakerTable.Columns.Add(CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.TASK], typeof(string));

            DataRow CareTakerTableRow = null!;
            string Consultant = string.Empty;
            string DepartmentOfConsultant = string.Empty;
            int RowCount = 1;
            foreach (UnAssignCareTakerReport LineItem in CareTakerUnAssignReport.LineUnAssignCareTakerReport)
            {
                if (RType == 1)
                {
                    if (Consultant != LineItem.Name)
                    {
                        CareTakerTableRow = CareTakerTable.Rows.Add();
                        CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.SLNO]] = "Consultant : " + LineItem.Name;
                        Consultant = LineItem.Name;
                    }
                }
                if (RType == 2)
                {
                    if (DepartmentOfConsultant != LineItem.DepartmentOfConsultant)
                    {
                        CareTakerTableRow = CareTakerTable.Rows.Add();
                        CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.SLNO]] = "Department : " + LineItem.DepartmentOfConsultant;
                        DepartmentOfConsultant = LineItem.DepartmentOfConsultant;
                    }
                }
                CareTakerTableRow = CareTakerTable.Rows.Add();
                {
                    CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.SLNO]] = RowCount;

                    if (RType != 1)
                    {
                        CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.NAME]] = (LineItem?.Name != null) ? LineItem.Name : string.Empty;
                    }                    
                    CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.AGE]] = (LineItem?.Age != null) ? LineItem.Age : string.Empty;
                    CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.ADDRESS]] = (LineItem?.Address != null) ? LineItem.Address : string.Empty;
                    if (RType != 2)
                    {
                        CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.DEPT]] = (LineItem?.DepartmentOfConsultant != null) ? LineItem.DepartmentOfConsultant : string.Empty;
                    }
                    CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.TASK]] = (LineItem?.TaskStatus != null) ? LineItem.TaskStatus : string.Empty;
                    //CareTakerTableRow[CareTakerUnAssignDataTableColumn[(int)CareTakerUnAssignReportTableColumn.ADATE]] = (LineItem?.AuthorizedDr != null) ? LineItem.AuthorizedDr : string.Empty;
                    //CareTakerTable.Rows.Add(CareTakerTableRow);
                }
                RowCount++;
            }
            return CareTakerTable;
        }
        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, RptCareTakerUnAssign CTReport, string fileExtension, bool isPrint, string fromDate, string toDate, int reportType)
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
                PdfPTable MTable;
                if (reportType == 2)
                {
                    MTable = PdfTableHeader.ReportTableHeader(DataTable, "UnAssignCareTakerReportDept");
                }
                else
                {
                    MTable = PdfTableHeader.ReportTableHeader(DataTable, "UnAssignCareTakerReport");
                }                
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);
                
                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;
               
                if (reportType == 1 )
                {
                    widths = new float[] { 5f, 5f, 30f, 25f, 15f };
                    SetColumnVisibility(ReportMainTable, (int)CareTakerUnAssignReportTableColumn.SLNO, false);
                }
                else if ( reportType == 2)
                {
                    widths = new float[] { 5f, 25f, 5f, 25f, 15f };
                    SetColumnVisibility(ReportMainTable, (int)CareTakerUnAssignReportTableColumn.SLNO, false);

                }
                else
                {
                    widths = new float[] { 5f, 25f, 5f, 30f, 25f, 15f };
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
                        if (j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "From")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (reportType != 0)
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
