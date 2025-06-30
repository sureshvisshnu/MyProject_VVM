using fa.report.Ip;
using fa.reports.Hms;
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
using fa;
using FADataAccessLibrary.report.Ip;
using Fa.reports.Hms;

namespace Fa.views.utils.Report.Hms
{
    class DischargePatientReportSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, DischargeReport DischargeReport, string fileExtension, bool isPrint, int RType)
        {
            if (ReportGridView.Rows.Count != 0)
            {
                switch (fileExtension.ToLower())
                {
                    case "xls":
                        break;
                    case "pdf":
                        var DataTable = DataGridViewAsDataTable(DischargeReport, RType);
                        if (DataTable != null)
                        {
                            GeneratePDF(DataTable, DischargeReport, fileExtension, isPrint, DischargeReport.FromDate.ToString(Global.Company.DateFormat), DischargeReport.ToDate.ToString(Global.Company.DateFormat), RType);
                            break;
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            return true;
        }
        readonly String[] DischargeDataTableColumn = new String[]
        {
            "Ward", "Bed","Patient Id","Name & Address", "DOB", "Age", "Admitted On","Primary Doctor","Primary Nurse","Discharged On"
        };
        public DataTable DataGridViewAsDataTable(DischargeReport DischargeReport, int RType)
        {
            DataTable DischargeTable = new DataTable();
            if (RType != 4)
            {
                DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.WARD], typeof(string));
            }
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.BED], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PID], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PNAM], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PDOB], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PAGE], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PADMT], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PDOCT], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PNURS], typeof(string));
            DischargeTable.Columns.Add(DischargeDataTableColumn[(int)DischargeReportTableColumn.PDISC], typeof(string));

                DataRow DischargeTableRow = null;
            string Consultant = string.Empty;
            string DepartmentOfConsutant = string.Empty;
            string Insurance = string.Empty;
            string WardName = string.Empty;

            foreach (DischargePatientReport LineItem in DischargeReport.DischargePatientReport.OrderBy(x => x.WardName))
            {
                if (RType == 1)
                {
                    if (Consultant != LineItem.PrimaryDr)
                    {
                        DischargeTableRow = DischargeTable.Rows.Add();
                        DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.WARD]] = "Consultant : " + LineItem.PrimaryDr;
                        Consultant = LineItem.PrimaryDr;
                    }
                }
                if (RType == 2)
                {
                    if (DepartmentOfConsutant != LineItem.DepartmenOfConsutant)
                    {

                        DischargeTableRow = DischargeTable.Rows.Add();
                        DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.WARD]] = "Department : " + LineItem.DepartmenOfConsutant;
                        DepartmentOfConsutant = LineItem.DepartmenOfConsutant;
                    }
                }

                if (RType == 3)
                {
                    if (Insurance != LineItem.InsuranceComp)
                    {

                        DischargeTableRow = DischargeTable.Rows.Add();
                        DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.WARD]] = "Insurance : " + LineItem.InsuranceComp;
                        Insurance = LineItem.InsuranceComp;
                    }
                }
                if (RType == 4)
                {
                    if (WardName != LineItem.WardName)
                    {
                        DischargeTableRow = DischargeTable.Rows.Add();
                        DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.BED]] = "Ward : " + LineItem.WardName;
                        WardName = LineItem.WardName;
                    }
                }
                DischargeTableRow = DischargeTable.NewRow();
                if (RType != 4)
                {
                    DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.WARD]] = LineItem.WardName;
                }
                else
                {
                    DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.BED]] = LineItem.WardName;

                }
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.BED]] = LineItem.BedName;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PID]] = LineItem.PatientId;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PNAM]] = LineItem.PatientName + (string.IsNullOrEmpty(LineItem.Address) ? "" : Environment.NewLine + LineItem.Address);
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PDOB]] = LineItem.DOB.ToString(Global.Company.DateFormat); ;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PAGE]] = LineItem.Age;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PADMT]] = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PDOCT]] = LineItem.PrimaryDr;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PNURS]] = LineItem.PrimaryCT;
                DischargeTableRow[DischargeDataTableColumn[(int)DischargeReportTableColumn.PDISC]] = LineItem.DischargedOn.Year < 1900 ? string.Empty : LineItem.DischargedOn.ToString(Global.Company.DateFormat);

                DischargeTable.Rows.Add(DischargeTableRow);
            }
            return DischargeTable;

        }
        public void GeneratePDF(DataTable DataTable, DischargeReport DischargeReport, string fileExtension, bool isPrint, string FromDate, string Todate, int RType)
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
                    ReportLine1 = DischargeReport.ReportTitle(),
                    ReportLine2 = DischargeReport.ReportSubTitle()
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
                    ReportLine1 = DischargeReport.ReportTitle(),
                    ReportLine2 = DischargeReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "IpReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;

                // Update widths based on RType
                if (RType == 4)
                {
                    widths = new float[] { 20f, 20f, 30f, 20f, 10f, 20f, 20f, 20f, 22f };
                    SetColumnVisibility(ReportMainTable, (int)DischargeReportTableColumn.WARD, false);
                }
                else
                {
                    widths = new float[] { 20f, 15f, 30f, 53f, 20f, 10f, 20f, 20f, 20f, 22f };
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
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        if (DataTable.Columns[j].ColumnName == "Age")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (RType != 5)
                        {
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                            {
                                continue;
                            }
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
                PdfGeneration.FileName = DischargeReport.ReportName();
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