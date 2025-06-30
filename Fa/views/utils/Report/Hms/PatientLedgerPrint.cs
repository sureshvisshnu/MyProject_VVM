using fa.api.Accounting;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Hms.Master;
using fa.views.utils.Common;
using Fa.report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.views.utils.Report.Hms
{
    enum PatientLedgerTableColumn
    {
        DATE, DESC, FEECHARGED, AMOUNTRECEIVED, BALANCE, ID
    }
    public class PatientLedgerPrint
    {
        public void ExportToFileOrPrint(RptPatientLedger PatientsLedger, bool isPrint)
        {
            LaserPrint(PatientsLedger, isPrint);
        }

        readonly String[] LedgerDataTableColumn = new String[]
        {
            "Date", "Description", "Fee Charged", "Amount Received", "Balance", "Id"
        };

        private DataTable LedgerAlignment(RptPatientLedger PatientsLedger)
        {
            DataTable LedgerTable = new DataTable();
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.DATE], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.DESC], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.FEECHARGED], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.AMOUNTRECEIVED], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)PatientLedgerTableColumn.ID], typeof(string));

            DataRow LedgerTableRow = null;
            if (PatientsLedger.LineItems.Count > 0)
            {
                LedgerTableRow = LedgerTable.NewRow();
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DATE]] = DateUtils.FormatDate(PatientsLedger.FromDate.Date, Global.Company.DateFormat); // PatientsLedger.FromDate.Date.ToShortDateString();
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DESC]] = "Op. Balance";
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE]] = Math.Abs(PatientsLedger.OpeningBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (PatientsLedger.OpeningBalance == 0 ? "      " : PatientsLedger.OpeningBalance > 0 ? " DR" : " CR"); //                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE]] = Math.Abs(PatientsLedger.OpeningBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + (PatientsLedger.CreditOrDebit(PatientsLedger.OpeningBalance) == report.common.CrDr.CR ? " CR" : " DR");
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.ID]] = PatientsLedger.PatientId;
                LedgerTable.Rows.Add(LedgerTableRow);

                DateTime? lDate = null;
                double RBalance = PatientsLedger.OpeningBalance;
                //Print Line items
                foreach (PatientLedgerLineItem LineItem in PatientsLedger.LineItems)
                {
                    LedgerTableRow = LedgerTable.NewRow();
                    if (lDate != LineItem.Date.Date)
                    {
                        LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DATE]] = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        lDate = LineItem.Date.Date;
                    }
                    LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DESC]] = LineItem.Description;
                    if (LineItem.CreditOrDebit() == report.common.CrDr.CR)
                    {
                        LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.AMOUNTRECEIVED]] = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    else
                    {
                        LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.FEECHARGED]] = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    }
                    RBalance += LineItem.Amount;
                    LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE]] = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");  // Math.Abs(RBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + " " + PatientsLedger.CreditOrDebit(RBalance);
                    LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.ID]] = PatientsLedger.PatientId;
                    LedgerTable.Rows.Add(LedgerTableRow);
                    if (LineItem.Description.StartsWith("To Invoice #"))
                    {
                        RBalance -= LineItem.Amount;
                        DataRow accountsReceivableRow = LedgerTable.NewRow();
                        accountsReceivableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DESC]] = "By Accounts Receivable";
                        accountsReceivableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.AMOUNTRECEIVED]] = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        accountsReceivableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE]] = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "      " : RBalance > 0 ? " DR" : " CR");
                        accountsReceivableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.ID]] = PatientsLedger.PatientId;
                        LedgerTable.Rows.Add(accountsReceivableRow);
                    }
                }
                LedgerTableRow = LedgerTable.NewRow();
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.DESC]] = "Cl. Balance";
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.BALANCE]] = Math.Abs(PatientsLedger.ClosingBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (PatientsLedger.ClosingBalance == 0 ? "      " : PatientsLedger.ClosingBalance > 0 ? " DR" : " CR");  // Math.Abs(PatientsLedger.ClosingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + (PatientsLedger.CreditOrDebit(PatientsLedger.ClosingBalance) == report.common.CrDr.CR ? " CR" : " DR");
                LedgerTableRow[LedgerDataTableColumn[(int)PatientLedgerTableColumn.ID]] = PatientsLedger.PatientId;
                LedgerTable.Rows.Add(LedgerTableRow);
            }
            return LedgerTable;
        }

        public void LaserPrint(RptPatientLedger PatientsLedger, bool isPrint)
        {
            DataTable dataTable = LedgerAlignment(PatientsLedger);
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -15, -15, 30, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                Patient lPatient = PatientManager.Instance.GetPatientById(PatientsLedger.PatientId);
                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = PatientsLedger.ReportTitle(),
                    ReportLine2 = "Patient Name: " + lPatient.Name+"\n\n"+ "Patient Id: " + lPatient.PatientNumber + "\n\n" + PatientsLedger.ReportSubTitle()
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
                    ReportLine2 = PatientsLedger.ReportTitle(),
                    ReportLine1 = "Patient Name: " + lPatient.Name + "\n" + "Patient Id: " + lPatient.PatientNumber
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "Patient Ledger");

                double A4Height = 760;
                double TotalWorkingOnPageH = 0;

                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count - 1;
                int Rows = dataTable.Rows.Count;
                float[] BodyTableWidths = new float[] { 25f, 60f, 25f, 30f, 25f };

                PdfPTable ReportBodyTable = new PdfPTable(Cols);
                ReportBodyTable.SetWidths(BodyTableWidths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
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
                        var Temp = dataTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.BorderColor = new BaseColor(160, 160, 160);
                        rowCell.BackgroundColor = CurRowColor;
                        if (j == 0 || j == 1)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            if(Temp.Trim() == "0.00")
                            {
                                rowCell.PaddingRight = 17;
                            }
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        ReportBodyTable.AddCell(rowCell);
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
                PdfGeneration.FileName = PatientsLedger.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
