using fa;
using fa.views.utils;
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
using static FADataAccessLibrary.report.Hms.RptFeeCollection;
using Font = iTextSharp.text.Font;
using fa.api.utils;
using Fa.reports.Hms;

namespace Fa.views.utils.Report.Hms
{
    public class FeeCollectionReportSavePrint
    {
        public bool ExportOrPrintToFile(DataGridView ReportGridView, RptFeeCollection FeeCollectionReport, string ReportName, string ReportHeading, string fileExtension, bool isPrint, string FromDate, string Todate, int ReportIndex)
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

                            DataTable DataTable = DataGridViewAsDataTable(FeeCollectionReport, ReportIndex);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, ReportName, ReportHeading, isPrint, FromDate, Todate, ReportIndex);
                            }
                            break;
                        default:
                            MessageBox.Show("Unsupported file extension.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error. Please contact system admin.");
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
            return true;
        }
        
        enum FeeCollectionReportTableColumn
        {
            SNO, DATE, PATIENT, OPIP, CONSULTANT, FEETYPE, AMOUNT, AID, ROWHEADING
        }
        readonly String[] FeeCollectionDataTableColumn = new String[]
        {
            "#", "Date", "Patient", "OP/IP", "Consultant", "Fee Type", "Charge Amount", "AID"
        };
        public DataTable DataGridViewAsDataTable(RptFeeCollection rptFeeCollection, int ReportIndex)
        {
            DataTable FeeCollectionTable = new DataTable();
            FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO], typeof(string));
            FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.DATE], typeof(string));
            FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.PATIENT], typeof(string));
            if (ReportIndex == 3)
            {
                FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP] = "OP", typeof(string));
            }
            else if (ReportIndex == 4)
            {
                FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP] = "IP", typeof(string));
            }
            else
            {
                FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP], typeof(string));
            }
            if (ReportIndex != 1)
            {
                FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT], typeof(string));
            }
            if (ReportIndex != 2)
            {
                FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE], typeof(string));
            }
            FeeCollectionTable.Columns.Add(FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT], typeof(string));
            DataRow FeeCollectionTableRow = null!;

            int i = 1;
            string patientName = string.Empty;
            string ConsultantName = string.Empty;
            string Ftype = string.Empty;
            bool isFirstRow = true;
            DateTime? lDate = null!;
            double subTotal = 0;
            double GrandTotal = 0;

            List<FeeChargeReportLineItem> GroupedLineItems = null!;
            if (ReportIndex == 0 || ReportIndex == 3 || ReportIndex == 4)
            {
                List<FeeChargeReportLineItem> Lineitems = rptFeeCollection.LineItems.Where(x => x.Fee != 0).ToList();
                if (ReportIndex == 0)
                {
                    GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.Fee != 0).ToList();
                }
                else if (ReportIndex == 3)
                {
                    GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.OpIp != "IP").ToList();
                }
                else if (ReportIndex == 4)
                {
                    GroupedLineItems = rptFeeCollection.LineItems.Where(x => x.OpIp != "OP").ToList();
                }
                foreach (FeeChargeReportLineItem Lineitem in GroupedLineItems)
                {
                    FeeCollectionTableRow = FeeCollectionTable.NewRow();
                    if (lDate == null || lDate != Lineitem.Date.Date)
                    {
                        if (!isFirstRow)
                        {
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = "SubTotal";
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            subTotal = 0;
                            i = 1;
                            FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                            FeeCollectionTableRow = FeeCollectionTable.NewRow();
                        }
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.DATE]] = Lineitem.Date.ToString(Global.Company.DateFormat);
                        isFirstRow = false;
                    }
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO]] = i;
                    if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date)
                    {
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.PATIENT]] = Lineitem.PatientName;
                        patientName = Lineitem.PatientName;
                    }
                    lDate = Lineitem.Date.Date;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP]] = Lineitem.OpIp;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT]] = Lineitem.ConsultantName;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = Lineitem.FeeType;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    subTotal += Lineitem.Fee;
                    GrandTotal += Lineitem.Fee;
                    i++;
                    FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                }
                AddSubTotalRow(subTotal, GrandTotal, FeeCollectionTableRow, FeeCollectionTable, ReportIndex);
            }
            else if (ReportIndex == 1)
            {
                foreach (FeeChargeReportLineItem Lineitem in rptFeeCollection.LineItems.Where(x => x.Fee != 0).OrderBy(x => x.ConsultantName))
                {
                    FeeCollectionTableRow = FeeCollectionTable.NewRow();
                    if (ConsultantName == string.Empty || ConsultantName != Lineitem.ConsultantName)
                    {
                        if (!isFirstRow)
                        {
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = "SubTotal";
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            subTotal = 0;
                            i = 1;
                            FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                            FeeCollectionTableRow = FeeCollectionTable.NewRow();
                        }
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO]] = "Consultant Name : " + Lineitem.ConsultantName;
                        ConsultantName = Lineitem.ConsultantName;
                        isFirstRow = false;
                        FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                        FeeCollectionTableRow = FeeCollectionTable.NewRow();
                    }
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO]] = i;
                    if (lDate == null || lDate != Lineitem.Date.Date)
                    {
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.DATE]] = Lineitem.Date.ToString(Global.Company.DateFormat);
                    }
                    if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date)
                    {
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.PATIENT]] = Lineitem.PatientName;
                        patientName = Lineitem.PatientName;
                    }
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP]] = Lineitem.OpIp;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = Lineitem.FeeType;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    lDate = Lineitem.Date.Date;
                    subTotal += Lineitem.Fee;
                    GrandTotal += Lineitem.Fee;
                    i++; 
                    FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                }
                AddSubTotalRow(subTotal, GrandTotal, FeeCollectionTableRow, FeeCollectionTable, ReportIndex);
            }
            else if (ReportIndex == 2)
            {
                foreach (FeeChargeReportLineItem Lineitem in rptFeeCollection.LineItems.Where(x => x.Fee != 0).OrderBy(x => x.FeeType))
                {
                    FeeCollectionTableRow = FeeCollectionTable.NewRow();
                    if (Ftype == string.Empty || Ftype != Lineitem.FeeType)
                    {
                        if (!isFirstRow)
                        {
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT]] = "SubTotal";
                            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            subTotal = 0;
                            i = 1;
                            FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                            FeeCollectionTableRow = FeeCollectionTable.NewRow();
                        }
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO]] = "Fee Type : " + Lineitem.FeeType;
                        isFirstRow = false;
                        FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                        FeeCollectionTableRow = FeeCollectionTable.NewRow();
                    }
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.SNO]] = i;
                    if (lDate == null || lDate != Lineitem.Date.Date || Ftype != Lineitem.FeeType)
                    {
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.DATE]] = Lineitem.Date.ToString(Global.Company.DateFormat);
                    }
                    if (patientName == string.Empty || patientName != Lineitem.PatientName || lDate != Lineitem.Date.Date || Ftype != Lineitem.FeeType)
                    {
                        FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.PATIENT]] = Lineitem.PatientName;
                        patientName = Lineitem.PatientName;
                    }
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.OPIP]] = Lineitem.OpIp;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT]] = Lineitem.ConsultantName;
                    FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = Lineitem.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    lDate = Lineitem.Date.Date;
                    Ftype = Lineitem.FeeType;
                    subTotal += Lineitem.Fee;
                    GrandTotal += Lineitem.Fee;
                    i++;
                    FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
                }
                AddSubTotalRow(subTotal, GrandTotal, FeeCollectionTableRow, FeeCollectionTable, ReportIndex);
            }
            return FeeCollectionTable;
        }
        private void AddSubTotalRow(double subTotal, double GrandTotal, DataRow FeeCollectionTableRow, DataTable FeeCollectionTable, int ReportIndex)
        {
            FeeCollectionTableRow = FeeCollectionTable.NewRow();
            if (ReportIndex == 2)
            {
                FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT]] = "SubTotal";
            }
            else
            {
                FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = "SubTotal";
            }
            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = subTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            FeeCollectionTable.Rows.Add(FeeCollectionTableRow);

            FeeCollectionTableRow = FeeCollectionTable.NewRow();
            if (ReportIndex == 2)
            {
                FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.CONSULTANT]] = "GrandTotal";
            }
            else
            {
                FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.FEETYPE]] = "GrandTotal";
            }
            FeeCollectionTableRow[FeeCollectionDataTableColumn[(int)FeeCollectionReportTableColumn.AMOUNT]] = GrandTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            FeeCollectionTable.Rows.Add(FeeCollectionTableRow);
        }
        public void GeneratePDF(DataTable dataTable, string fileName, string heading, bool isPrint, string FromDate, string Todate, int ReportIndex)
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
                PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = false,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = heading,
                    ReportLine2 = FromDate + " to " + Todate
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "FeeChargeReport");
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths;

                if (ReportIndex == 1)
                {
                    widths = new float[] { 7f, 10f, 34f, 7f, 40f, 15f };
                    SetColumnVisibility(ReportMainTable, (int)FeeCollectionReportTableColumn.CONSULTANT, false);
                }
                else if (ReportIndex == 2)
                {
                    widths = new float[] { 7f, 10f, 34f, 7f, 40f, 15f };
                    SetColumnVisibility(ReportMainTable, (int)FeeCollectionReportTableColumn.FEETYPE, false);
                }
                else
                {
                    widths = new float[] { 7f, 10f, 26f, 7f, 20, 28f, 15f };
                }

                ReportMainTable.SetWidths(widths);
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
                        RowCell.BorderWidthTop = (float)BorderStyle.None;
                        if (ReportIndex == 0 || ReportIndex == 3 || ReportIndex == 4)
                        {
                            if (j == 6)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][5].ToString() == "SubTotal") || (dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][5].ToString() == "GrandTotal"))
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                else if (j > 0 && j < 5)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                                if (j == 5)
                                {
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                        }
                        else
                        {
                            if (j == 5)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if ((dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][4].ToString() == "SubTotal") || (dataTable.Rows[i][0].ToString() == "" && dataTable.Rows[i][4].ToString() == "GrandTotal"))
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                else if (j > 0 && j < 4)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                                if (j == 4)
                                {
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                            if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][4].ToString()))
                            {
                                if (i == 0)
                                {
                                    RowCell.BorderWidthTop = (float)0.3;
                                }
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.Colspan = dataTable.Columns.Count - 1;
                            }
                            if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][4].ToString()))
                            {
                                if (j != 5)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        RowCell.BorderWidthTop = (float)0.3;
                                    }
                                    RowCell.BorderWidthRight = (float)0.5;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                    RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                }
                            }
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
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = fileName;
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
