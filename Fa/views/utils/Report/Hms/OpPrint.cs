using fa.api.Accounting;
using fa.api.utils;
using fa.model.Hms.Master;
using fa.report.Hms;
using fa.reports.common.headers;
using fa.reports.Hms;
using fa.views.utils.Common;
using Fa.reports.sales;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.report.Hms.RptOPLineItem;

namespace fa.views.utils.Report.Hms
{
    class OpPrint
    {
        readonly String[] OpDataTableColumn = new String[]
        {
            "Sno","Date", "Patient Details", "Age","Gender", "Patient Number", "Token No", "Requested Doctor", "Fees"
        };
        public void ExportToFileOrPrint(RptOpRegister RptOpRegister, bool isPrint)
        {
            LaserPrint(RptOpRegister, isPrint);
        }
        private DataTable OpAlignment(RptOpRegister RptOpRegister)
        {
            DataTable OpRegisterTable = new DataTable();
            if (RptOpRegister.Type == OpPatientTypeSelection.BYALLPATIENT)
            {
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.SNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.DATE], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT_AGE], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT_GENDER], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENTNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.TOKENNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.FEE], typeof(string));

                DataRow OpTableTableRow = null;
                String dateTime = null;

                List<RptOPLineItem> LineItems = (List<RptOPLineItem>)RptOpRegister.LineItems;
                if (LineItems != null && LineItems.Count > 0)
                {
                    int i = 1;
                    double Total = 0;
                    foreach (RptOPLineItem LineItem in LineItems)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        OpTableTableRow = OpRegisterTable.NewRow();
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.SNO]] = i;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT]] = (LineItem.Patientdetail);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT_AGE]] = (LineItem.Age);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT_GENDER]] = (LineItem.Gender);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENTNO]] = (LineItem.PatientNo);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.TOKENNO]] = (LineItem.TokenNo);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT]] = (LineItem.ConsultantDetail);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.FEE]] = LineItem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        OpRegisterTable.Rows.Add(OpTableTableRow);
                        Total += LineItem.Fee;
                        i++;
                    }
                    OpTableTableRow = OpRegisterTable.NewRow();
                    OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT]] = "Total";
                    OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.FEE]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    OpRegisterTable.Rows.Add(OpTableTableRow);
                }
            }
            else if (RptOpRegister.Type == OpPatientTypeSelection.BYNEWPATIENT || RptOpRegister.Type == OpPatientTypeSelection.BYREPATEDPATIENT)
            {
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.SNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.DATE], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT_AGE], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENT_GENDER], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.PATIENTNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.TOKENNO], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT], typeof(string));
                OpRegisterTable.Columns.Add(OpDataTableColumn[(int)OpReportGridColumn.FEE], typeof(string));

                DataRow OpTableTableRow = null;
                String dateTime = null;

                if (RptOpRegister.PatientOpRepoertLineItems != null && RptOpRegister.PatientOpRepoertLineItems.Count > 0)
                {
                    int i = 1;
                    double Total = 0;
                    foreach (PatientOpRepoertLineItem LineItem in RptOpRegister.PatientOpRepoertLineItems.OrderBy(x => x.Date))
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        OpTableTableRow = OpRegisterTable.NewRow();
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.SNO]] = i;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT]] = (LineItem.Patientdetail);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT_AGE]] = (LineItem.Age);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENT_GENDER]] = (LineItem.Gender);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.PATIENTNO]] = (LineItem.PatientNo);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.TOKENNO]] = (LineItem.TokenNo);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT]] = (LineItem.ConsultantDetail);
                        OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.FEE]] = LineItem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        OpRegisterTable.Rows.Add(OpTableTableRow);
                        Total += LineItem.Fee;
                        i++;
                    }
                    OpTableTableRow = OpRegisterTable.NewRow();
                    OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.CONSULTANT]] = "Total";
                    OpTableTableRow[OpDataTableColumn[(int)OpReportGridColumn.FEE]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    OpRegisterTable.Rows.Add(OpTableTableRow);
                }
            }
            return OpRegisterTable;
        }
        private void CalculateColumn(PdfPTable ReportBodyTable, int startRow, int endRow, out double totalAmount)
        {
            totalAmount = 0;
            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = 0; j < ReportBodyTable.NumberOfColumns; j++)
                {
                    PdfPCell cell = ReportBodyTable.Rows[i].GetCells()[j];
                    if (j == 5)
                    {
                        double tempTotal;
                        if (double.TryParse(cell.Phrase.Content, out tempTotal))
                        {
                            totalAmount += tempTotal;
                        }
                    }
                }
            }
        }
        public void LaserPrint(RptOpRegister RptOpRegister, bool isPrint)
        { 
            DataTable DataTable = OpAlignment(RptOpRegister);
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                double A4Height = 760;
                double totalAmount = 0;

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = RptOpRegister.ReportTitle(),
                    ReportLine2 = RptOpRegister.ReportSubTitle()
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
                    ReportLine1 = RptOpRegister.ReportTitle(),
                    ReportLine2 = RptOpRegister.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "OpReport");

                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                float[] BodyTableWidths = new float[] { 15f, 30f, 65f, 30f, 30f, 30f, 25f, 40f, 20f };

                PdfPTable ReportBodyTable = new PdfPTable(Cols);
                ReportBodyTable.SetWidths(BodyTableWidths);

                PdfPCell rowCell = new PdfPCell();
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        // Add a row at the bottom of the current page before the page break
                        totalAmount = 0;
                        CalculateColumn(ReportBodyTable, 0, ReportBodyTable.Rows.Count - 1, out totalAmount);
                        AddBottomRow(ReportBodyTable, Cols, totalAmount);

                        pdfDoc.Add(ReportBodyTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MiniHTable);
                        pdfDoc.Add(MTable);
                        ReportBodyTable = new PdfPTable(Cols);
                        ReportBodyTable.SetWidths(BodyTableWidths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;

                        if (i == Rows - 1)
                        {
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 1 || j == 2 || j == 3 || j == 4 || j == 5)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (j == 6)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (DataTable.Columns[j].ColumnName == "Consultant/ Doctor")
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                        if (DataTable.Columns[j].ColumnName == "Fees" || DataTable.Columns[j].ColumnName == "Token No" || DataTable.Columns[j].ColumnName == "Age")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (DataTable.Columns[j].ColumnName == "Date" || DataTable.Columns[j].ColumnName == "Patient Details" || DataTable.Columns[j].ColumnName == "Patient Number")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        ReportBodyTable.AddCell(RowCell);
                    }
                    k++;
                }
                pdfDoc.Add(ReportBodyTable);
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
                PdfGeneration.FileName = RptOpRegister.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
        private void AddBottomRow(PdfPTable table, int Cols, double totalAmount)
        {
            for (int j = 0; j < Cols; j++)
            {
                PdfPCell cell;
                if (j == 0)
                {
                    Phrase phrase = new Phrase("Page Total's", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Colspan = 5;
                }
                else if (j == 5)
                {
                    Phrase phrase = new Phrase(totalAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    continue;
                }
                cell.BorderColor = new BaseColor(160, 160, 160);
                cell.BackgroundColor = new BaseColor(230, 230, 230);
                table.AddCell(cell);
            }
        }
    }
}
