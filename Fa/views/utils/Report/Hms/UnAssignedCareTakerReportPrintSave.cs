using fa;
using fa.views.utils.Common;
using fa.views.utils;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using Fa.reports.sales;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.sales;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.utils;
using fa.reports.sales;
using Rectangle = iTextSharp.text.Rectangle;
using fa.api.Accounting;
using NPOI.SS.UserModel;
using fa.model.Employee;
using Fa.api.Hms;
using fa.model.Hms.Master;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;
using DocumentFormat.OpenXml.Bibliography;


namespace Fa.views.utils.Report.Hms
{
    internal class UnAssignedCareTakerReportPrintSave
    {
        public bool ExportOrPrintToFile(RptUnAssignedReport RptUnAssignedReport, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptUnAssignedReport != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptUnAssignedReport);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptUnAssignedReport, ReportName, fileExtension, isPrint, RptUnAssignedReport.FromDate.ToString(Global.Company.DateFormat), RptUnAssignedReport.ToDate.ToString(Global.Company.DateFormat));
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
        readonly String[] UnAssignedCareTakerReportByDateTableColumn = new String[]
        {
            "#","Name","Age","Address","Job Title","Department","Status",
        };
        readonly String[] UnAssignedCareTakerReportByDoctorTableColumn = new String[]
        {
            "#","Date","Age","Address","Department","Status",
        };
        readonly String[] UnAssignedCareTakerReportByNurseTableColumn = new String[]
        {
            "#","Date","Age","Address","Department","Status",
        };
        readonly String[] UnAssignedCareTakerReportByDepartmentTableColumn = new String[]
        {
            "#","Date","Name","Age","Address","Job Title","Status",
        };
        public DataTable DataGridViewAsDataTable(RptUnAssignedReport RptUnAssignedReport)
        {
            DataTable UnAsignedCaretakerByDateColumn = new DataTable();
            if (RptUnAssignedReport.Type == ReportType.BYDATE)
            {
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.SNO], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.NAME], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.AGE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.ADDRESS], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.JOB_TITLE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.DEPARTMENT], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.STATUS], typeof(string));

                int Sn = 1;
                String currentAssignDate = null;
                DataRow UnAsignedCaretakerByDateRow = null;
                foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.AssignDate).ThenBy(x => x.JobTitle))
                {
                    String formattedAssignDate = DateUtils.FormatDate(LineItem.AssignDate, Global.Company.DateFormat);
                    if (currentAssignDate == null || currentAssignDate != formattedAssignDate)
                    {
                        UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                        UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.SNO]] = " Date : " + formattedAssignDate;
                        UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                        currentAssignDate = formattedAssignDate;
                        Sn = 1;
                    }
                    UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.SNO]] = Sn;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.NAME]] = LineItem.Name;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.AGE]] = LineItem.Age;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.ADDRESS]] = LineItem.Address;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.JOB_TITLE]] = LineItem.JobTitle;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.DEPARTMENT]] = LineItem.DepartmentOfConsultant;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDateTableColumn.STATUS]] = LineItem.TaskStatus;
                    Sn++;
                    UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                }
            }
            else if (RptUnAssignedReport.Type == ReportType.BYDOCTOR)
            {
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.SNO], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.DATE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.AGE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.ADDRESS], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.DEPARTMENT], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.STATUS], typeof(string));

                int Sn = 1;
                String DoctorName = string.Empty;
                DataRow UnAsignedCaretakerByDateRow = null;
                foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.Name))
                {
                    if (DoctorName == null || DoctorName != LineItem.Name)
                    {
                        UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                        UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDateTableColumn[(int)UnAssignReportByDoctorTableColumn.SNO]] = " Name : " + LineItem.Name;
                        UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                        DoctorName = LineItem.Name;
                        Sn = 1;
                    }
                    UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.SNO]] = Sn;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.DATE]] = LineItem.AssignDate.ToString(Global.Company.DateFormat);
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.AGE]] = LineItem.Age;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.ADDRESS]] = LineItem.Address;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.DEPARTMENT]] = LineItem.DepartmentOfConsultant;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDoctorTableColumn[(int)UnAssignReportByDoctorTableColumn.STATUS]] = LineItem.TaskStatus;
                    Sn++;
                    UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                }
            }
            else if (RptUnAssignedReport.Type == ReportType.BYNURSE)
            {
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.SNO], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.DATE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.AGE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.ADDRESS], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.DEPARTMENT], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.STATUS], typeof(string));

                int Sn = 1;
                String DoctorName = string.Empty;
                DataRow UnAsignedCaretakerByDateRow = null;
                foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.Name))
                {
                    if (DoctorName == null || DoctorName != LineItem.Name)
                    {
                        UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                        UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.SNO]] = " Name : " + LineItem.Name;
                        UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                        DoctorName = LineItem.Name;
                        Sn = 1;
                    }
                    UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.SNO]] = Sn;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.DATE]] = LineItem.AssignDate.ToString(Global.Company.DateFormat);
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.AGE]] = LineItem.Age;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.ADDRESS]] = LineItem.Address;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.DEPARTMENT]] = LineItem.DepartmentOfConsultant;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByNurseTableColumn[(int)UnAssignReportByNurseTableColumn.STATUS]] = LineItem.TaskStatus;
                    Sn++;
                    UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                }
            }
            else
            {
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.SNO], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.DATE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.NAME], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.AGE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.ADDRESS], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.JOB_TITLE], typeof(string));
                UnAsignedCaretakerByDateColumn.Columns.Add(UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.STATUS], typeof(string));

                int Sn = 1;
                String Department = string.Empty;
                String currentAssignDate = null;
                DataRow UnAsignedCaretakerByDateRow = null;
                foreach (UnAssignCareTakerReportLineItemByDate LineItem in RptUnAssignedReport.UnAssignCareTakerReportLineItemByDate.OrderBy(x => x.DepartmentOfConsultant).ThenBy(x => x.AssignDate))
                {
                    String formattedAssignDate = DateUtils.FormatDate(LineItem.AssignDate, Global.Company.DateFormat);
                    if (Department == null || Department != LineItem.DepartmentOfConsultant)
                    {
                        UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                        UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.SNO]] = " Department Name : " + LineItem.DepartmentOfConsultant;
                        Department = LineItem.DepartmentOfConsultant;
                        Sn = 1;
                        currentAssignDate = null;
                        UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                    }
                    UnAsignedCaretakerByDateRow = UnAsignedCaretakerByDateColumn.NewRow();
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.SNO]] = Sn;
                    if (currentAssignDate == null || currentAssignDate != formattedAssignDate)
                    {
                        UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.DATE]] = formattedAssignDate;
                        currentAssignDate = formattedAssignDate;
                    }
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.NAME]] = LineItem.Name;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.AGE]] = LineItem.Age;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.ADDRESS]] = LineItem.Address;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.JOB_TITLE]] = LineItem.JobTitle;
                    UnAsignedCaretakerByDateRow[UnAssignedCareTakerReportByDepartmentTableColumn[(int)UnAssignReportByDepartmentTableColumn.STATUS]] = LineItem.TaskStatus;
                    Sn++;
                    UnAsignedCaretakerByDateColumn.Rows.Add(UnAsignedCaretakerByDateRow);
                }
            }
            return UnAsignedCaretakerByDateColumn;
        }
        public void GeneratePDF(DataTable DataTable, RptUnAssignedReport RptUnAssignedReport, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = "UnAssigned CareTaker Report",
                    ReportLine2 = RptUnAssignedReport.ReportSubTitle()
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
                    ReportLine1 = "UnAssigned CareTaker Report",
                    ReportLine2 = RptUnAssignedReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptUnAssignedReport.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count - 1;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 30f, 15f, 60f, 40f, 30f };
                if (RptUnAssignedReport.Type == ReportType.BYNURSE || RptUnAssignedReport.Type == ReportType.BYDOCTOR)
                {
                    widths = new float[] {10f, 30f, 15f, 60f, 40f, 30f };
                }
                if(RptUnAssignedReport.Type == ReportType.BYDATE)
                {
                    widths = new float[] { 10f, 40f, 15f, 60f, 30f, 40f, 30f };
                }
                if(RptUnAssignedReport.Type == ReportType.BYDEPARTMENT)
                {
                    widths = new float[] { 10f, 30f, 40f, 15f, 60f, 30f, 30f };
                }
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;
                PdfPCell HeaderCell = new PdfPCell();
                if(RptUnAssignedReport.Type == ReportType.BYDEPARTMENT || RptUnAssignedReport.Type == ReportType.BYDATE)
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(HTable);
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
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorTop = BaseColor.WHITE;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            if(RptUnAssignedReport.Type == ReportType.BYDEPARTMENT)
                            {
                                if (j == 3)
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                            }
                            else if (RptUnAssignedReport.Type == ReportType.BYDATE)
                            {
                                if (j == 2)
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                            }
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][6].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][6].ToString()))
                            {
                                continue;
                            }

                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                }
                else
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(HTable);
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
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorTop = BaseColor.WHITE;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            if (j == 2)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][5].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][5].ToString()))
                            {
                                continue;
                            }

                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
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
                PdfGeneration.FileName = RptUnAssignedReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
