using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.views.utils.Common;
using fa.views.utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using Fa.reports.Hms;
using FADataAccessLibrary.report.Hms;
using fa.reports.sales;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;
using fa;
using fa.api.Hms;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.api.utils;

namespace Fa.views.utils.Report.Hms
{
    public class PatientDueListReportPrintSave
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, RptPatientDueList RptPatientDueList, string ReportName, string ReportHeading, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            if (ReportGridView.Rows.Count > 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptPatientDueList);
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
        static readonly String[] PatientDueListColumn = new String[]
        {
            "#", "Name&Address", "Age", "Patient Id", "Fee Type", "Description", "Amount"
        };
        public static DataTable DataGridViewAsDataTable(RptPatientDueList RptPatientDueList)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.SNO], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.PDETAIL], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.PAGE], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.PID], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.FEETYPE], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.DESC], typeof(string));
            dataTable.Columns.Add(PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT], typeof(string));
            DataRow PatientDueListRow = null;

            int i = 0;
            string patientdetail = null;
            double subTotal = 0.00;
            double Total = 0.00;
            string Admissiondetail = "";
            string Admission = "";
            bool admissionAdded = false;
            //InPatientAdmission? inPatientAdmission = null;

            foreach (RptPatientDueListLineItem lineItem in RptPatientDueList.LineItems.OrderBy(x => x.isIp).ThenBy(b => b.PatientNo))
            {
                if (RptPatientDueList.PatientType.Contains("All") && RptPatientDueList.PatientType.Contains("By OutPatient") && RptPatientDueList.PatientType.Contains("By InPatient")) 
                {
                    Admission = lineItem.isIp == true ? "In Patients" : "Out Patients";
                    if (!admissionAdded && Admissiondetail != Admission)
                    {
                        PatientDueListRow = dataTable.NewRow();
                        PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]] = lineItem.isIp == true ? "In Patients" : "Out Patients";
                        dataTable.Rows.Add(PatientDueListRow);
                        Admissiondetail = PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]].ToString();
                        admissionAdded = true;
                    }
                }
                if (lineItem.openingAmount != 0 && patientdetail == null)
                {
                    PatientDueListRow = dataTable.NewRow();
                    PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]] = "Opening Amount";
                    PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(lineItem.openingAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.openingAmount < 0 ? " Dr" : " Cr");
                    dataTable.Rows.Add(PatientDueListRow);
                    subTotal = lineItem.openingAmount;
                    Total = lineItem.openingAmount;
                }
                if (lineItem.Fee != 0 || lineItem.openingAmount != 0)
                {
                    PatientDueListRow = dataTable.NewRow();
                    if (patientdetail != lineItem.Patientdetail)
                    {
                        if (patientdetail != null)
                        {
                            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DESC]] = "Sub Total";
                            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(subTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (subTotal < 0 ? " Dr" : " Cr");
                            dataTable.Rows.Add(PatientDueListRow);
                            if (admissionAdded && Admissiondetail != Admission)
                            {
                                PatientDueListRow = dataTable.NewRow();
                                PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]] = lineItem.isIp == true ? "In Patients" : "Out Patients";
                                dataTable.Rows.Add(PatientDueListRow);
                                Admissiondetail = PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]].ToString();
                                admissionAdded = true;
                            }
                            subTotal = 0.00;
                            PatientDueListRow = dataTable.NewRow();
                        }
                        if (subTotal == 0.00 && lineItem.openingAmount != 0)
                        {
                            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]] = "Opening Amount";
                            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(lineItem.openingAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.openingAmount < 0 ? " Dr" : " Cr"); 
                            dataTable.Rows.Add(PatientDueListRow);
                            PatientDueListRow = dataTable.NewRow();
                            subTotal = lineItem.openingAmount;
                            Total += lineItem.openingAmount;
                        }
                        PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.SNO]] = i + 1;
                        PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.PAGE]] = lineItem.Age;
                        PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.PID]] = lineItem.PatientNo;
                        PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.PDETAIL]] = lineItem.Patientdetail;
                        patientdetail = lineItem.Patientdetail;
                        i++;
                    }
                    PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.FEETYPE]] = lineItem.FeeType;
                    PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DESC]] = lineItem.Description;
                    PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(lineItem.Fee).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (lineItem.Fee < 0 ? " Dr" : " Cr");
                    dataTable.Rows.Add(PatientDueListRow);
                    subTotal += lineItem.Fee;
                    Total += lineItem.Fee;
                }
            }
            PatientDueListRow = dataTable.NewRow();
            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DESC]] = "Sub Total";
            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(subTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (subTotal < 0 ? " Dr" : " Cr");
            dataTable.Rows.Add(PatientDueListRow);

            PatientDueListRow = dataTable.NewRow();
            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DESC]] = "Total Amount";
            PatientDueListRow[PatientDueListColumn[(int)PatientDueListTableColumn.DUEAMOUNT]] = Math.Abs(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (Total < 0 ? " Dr" : " Cr");
            dataTable.Rows.Add(PatientDueListRow);

            return dataTable;
        }
        
        public void GeneratePDF(DataTable dataTable, string fileName, string heading, bool isPrint, string FromDate, string Todate)
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
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                pdfDoc.Add(HTable);
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "Patient Due List");
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                PdfPTable ReportMainTable = new PdfPTable(7);
                float[]  widths = new float[] { 15f, 60f, 15f, 40f, 40f, 60f, 30f };
                ReportMainTable.SetWidths(widths);
                Cursor.Current = Cursors.WaitCursor;
                bool page = false;
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
                        page = true;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();

                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.UseVariableBorders = true;
                        RowCell.BorderColor = BaseColor.GRAY;
                        if (i != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorTop = BaseColor.WHITE;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        if (j != 0)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColorLeft = BaseColor.WHITE;
                            RowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }

                        if (dataTable.Columns[j].ColumnName == "Age" || dataTable.Columns[j].ColumnName == "Amount")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }

                        if (string.IsNullOrEmpty(dataTable.Rows[i][j].ToString()) || Temp == "Sub Total" || Temp == "Total Amount" || j == (int)PatientDueListTableColumn.DUEAMOUNT)
                        {
                            if (dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][4].ToString() == "")
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            }
                            if (j == 5)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                        if ((i == (Rows - 1)) || (dataTable.Rows[i][j].ToString() == "" && dataTable.Rows[i][5].ToString() == "Sub Total"))
                        {
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            else if (j > 0 && j < 4)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (j == 4)
                            {
                                RowCell.BorderWidthRight = 0.5f;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                        }
                        if (string.IsNullOrEmpty(dataTable.Rows[i][1].ToString()))
                        {
                            if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][5].ToString()))
                            {
                                RowCell.Colspan = dataTable.Columns.Count - 1;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][5].ToString()))
                            {
                                if (j != 6)
                                {
                                    continue;
                                }
                                else
                                {
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                        }
                        if (page)
                        {
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderWidthTop = 0.25f;
                            RowCell.BorderColorTop = BaseColor.GRAY;
                        }
                        RowCell.MinimumHeight = 15;
                        ReportMainTable.AddCell(RowCell);
                    }
                    page = false;
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
