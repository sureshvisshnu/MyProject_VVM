using fa.api.utils;
using fa.model.Common;
using fa.report.Ip;
using fa.reports.Hms;
using fa.reports.Inventory;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Windows.Forms;
using VisioForge.MediaFramework.FFMPEGCore.Instance;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Report.Hms
{
    class InPatientReportSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, IpReport IpReport, string fileExtension, bool isPrint, int RType)
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
                            var DataTable = DataGridViewAsDataTable(IpReport, RType);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, IpReport, fileExtension, isPrint, IpReport.FromDate.ToString(Global.Company.DateFormat), IpReport.ToDate.ToString(Global.Company.DateFormat), RType);
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
        readonly String[] IpDataTableColumn = new String[]
        {
            "Ward", "Bed","Patient Id","Name & Address", "DOB", "Age", "Admitted On","Primary Doctor","Primary Nurse","Discharged On"
        };
        enum IpReportTableColumn
        {
            WARD, BED, PID, PNAM, PDOB, PAGE, PADMT, PDOCT, PNURS, PDISC, ROWHEADING
        }
        public  DataTable DataGridViewAsDataTable(IpReport IpReport, int RType)
        {
            DataTable IpTable = new DataTable();            
            if (RType != 4)
            {
                IpTable.Columns.Add(IpDataTableColumn[(int)(IpReport.OrdrStatus == 0 ? IpReportTableColumn.WARD : IpReport.OrdrStatus == 1 ? IpReportTableColumn.PADMT : IpReportTableColumn.PDISC)], typeof(string));
            }
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.BED], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PID], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PNAM], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PDOB], typeof(string));            
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PAGE], typeof(string));            
            IpTable.Columns.Add(IpDataTableColumn[(int)((RType == 4 && IpReport.OrdrStatus == 1) ? IpReportTableColumn.PADMT : (IpReport.OrdrStatus == 1 ? IpReportTableColumn.WARD : IpReportTableColumn.PADMT))], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PDOCT], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)IpReportTableColumn.PNURS], typeof(string));
            IpTable.Columns.Add(IpDataTableColumn[(int)((RType == 4 && IpReport.OrdrStatus == 2) ? IpReportTableColumn.PDISC : (IpReport.OrdrStatus == 2 ? IpReportTableColumn.WARD : IpReportTableColumn.PDISC))], typeof(string));            
            DataRow IpTableRow = null;
            string Consultant = string.Empty;
            string DepartmentOfConsutant = string.Empty;
            string Insurance = string.Empty;
            string WardName = string.Empty;

            foreach (InPatientReport LineItem in IpReport.LinePatients)
            {
                if (RType == 1)
                {
                    if (Consultant != LineItem.PrimaryDr)
                    {
                        IpTableRow = IpTable.Rows.Add();
                        if (IpReport.OrdrStatus == 0)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "Consultant : " + LineItem.PrimaryDr;
                        }
                        else if (IpReport.OrdrStatus == 1)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PADMT]] = "Consultant : " + LineItem.PrimaryDr;
                        }
                        else if (IpReport.OrdrStatus == 2)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDISC]] = "Consultant : " + LineItem.PrimaryDr;
                        }
                        Consultant = LineItem.PrimaryDr;
                    }
                }
                if (RType == 2)
                {
                    if (DepartmentOfConsutant != LineItem.DepartmenOfConsutant)
                    {

                        IpTableRow = IpTable.Rows.Add();
                        if (IpReport.OrdrStatus == 0)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "Department : " + LineItem.DepartmenOfConsutant;
                        }
                        else if (IpReport.OrdrStatus == 1)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PADMT]] = "Department : " + LineItem.DepartmenOfConsutant;
                        }
                        else if (IpReport.OrdrStatus == 2)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDISC]] = "Department : " + LineItem.DepartmenOfConsutant;
                        }
                        DepartmentOfConsutant = LineItem.DepartmenOfConsutant;
                    }
                }

                if (RType == 3)
                {
                    if (Insurance != LineItem.InsuranceComp)
                    {

                        IpTableRow = IpTable.Rows.Add();
                        if (IpReport.OrdrStatus == 0)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "Insurance : " + LineItem.InsuranceComp;
                        }
                        else if (IpReport.OrdrStatus == 1)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PADMT]] = "Insurance : " + LineItem.InsuranceComp;
                        }
                        else if (IpReport.OrdrStatus == 2)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDISC]] = "Insurance : " + LineItem.InsuranceComp;
                        }
                        Insurance = LineItem.InsuranceComp;
                    }
                }
                if (RType == 4)
                {
                    if (WardName != LineItem.WardName)
                    {
                        IpTableRow = IpTable.Rows.Add();
                        IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.BED]] = "Ward : " + LineItem.WardName;                        
                        WardName = LineItem.WardName;
                    }

                }
                IpTableRow = IpTable.NewRow();
                if (RType != 4)
                {
                    if (IpReport.OrdrStatus == 0)
                    {
                        if (LineItem.WardName != null)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = LineItem.WardName;
                        }
                        else
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "n/a";
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.BED]] = "n/a";
                        }
                    }
                    else if (IpReport.OrdrStatus == 1)
                    {
                        if (LineItem.WardName != null)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = LineItem.WardName;
                        }
                        else
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "n/a";
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.BED]] = "n/a";
                        }
                    }
                    else if (IpReport.OrdrStatus == 2)
                    {
                        if (LineItem.WardName != null)
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = LineItem.WardName;
                        }
                        else
                        {
                            IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.WARD]] = "n/a";
                        }
                    }
                }
                if (LineItem.BedName != null)
                {
                    IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.BED]] = LineItem.BedName;
                }
                else
                {
                    IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.BED]] = "n/a";
                }
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PID]] = LineItem.PatientId;
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PNAM]] = LineItem.PatientName + (string.IsNullOrEmpty(LineItem.Address) ? "" : Environment.NewLine + LineItem.Address);
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDOB]] = LineItem.DOB.ToString(Global.Company.DateFormat); ;
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PAGE]] = LineItem.Age;
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PADMT]] = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDOCT]] = LineItem.PrimaryDr;
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PNURS]] = LineItem.PrimaryCT;
                IpTableRow[IpDataTableColumn[(int)IpReportTableColumn.PDISC]] = LineItem.DischargedOn.Year < 1900 ? string.Empty : LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                IpTable.Rows.Add(IpTableRow);
            }
            return IpTable;

        }

        private static readonly Font FNIB7Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black"));

        public void GeneratePDF(DataTable DataTable, IpReport IpReport, string fileExtension, bool isPrint, string FromDate, string Todate, int RType)
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
                    ReportLine1 = IpReport.ReportTitle(),
                    ReportLine2 = IpReport.ReportSubTitle()
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
                    ReportLine1 = IpReport.ReportTitle(),
                    ReportLine2 = IpReport.ReportSubTitle()
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
                    SetColumnVisibility(ReportMainTable, (int)IpReportTableColumn.WARD, false);
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
                        RowCell = new PdfPCell(new Phrase(Temp, FNIB7Font));
                        if (Temp != null && (Temp.Equals("N/A", StringComparison.OrdinalIgnoreCase) || Temp.Equals("n/a", StringComparison.OrdinalIgnoreCase)))
                        {
                            RowCell = new PdfPCell(new Phrase("", FNIB7Font));
                        }
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        if (j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "Ward" || DataTable.Columns[j].ColumnName == "AdmitedOn" || DataTable.Columns[j].ColumnName == "DischargedOn")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else if(DataTable.Columns[j].ColumnName == "Age")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }                        
                        if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                        {
                            RowCell.Colspan = DataTable.Columns.Count;
                        }
                        if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                        {
                            continue;
                        }
                        else if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][2].ToString()))
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
                PdfGeneration.FileName = IpReport.ReportName();
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
        private PdfPTable MainTable(DataTable DataTable)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_White = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] Defaultwidths = new float[] { 20f, 15f, 30f, 45f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
            float[] widths = new float[Cols];
            for (int i = 0; i < Cols; i++)
            {
                widths[i] = Defaultwidths[i];
            }
            ReportMainTable.SetWidths(widths);
            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_9_White));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 25;
                HeaderCell.Padding = 4;
                if (column.Caption == "Ward" || column.Caption == "AdmitedOn" || column.Caption == "DischargedOn")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }

                ReportMainTable.AddCell(HeaderCell);
            }

            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_8_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 20;
                    RowCell.Padding = 2;
                    if (j == 1 || j == 2 || DataTable.Columns[j].ColumnName == "Ward" || DataTable.Columns[j].ColumnName == "AdmitedOn" || DataTable.Columns[j].ColumnName == "DischargedOn")
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                    {
                        RowCell.Colspan = DataTable.Columns.Count;
                    }
                    ReportMainTable.AddCell(RowCell);
                }
            }

            for (int j = 0; j < Cols; j++)
            {
                PdfPCell rowCell = new PdfPCell();
                rowCell = new PdfPCell(new Phrase("", Font_Normal_Italic_8_Black));
                rowCell.BorderColor = BaseColor.BLACK;

                rowCell.MinimumHeight = 1f;
                ReportMainTable.AddCell(rowCell);
            }
            return ReportMainTable;
        }
    }
}
