using fa.api.Accounting;
using fa.api.System;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.model.System;
using fa.report.accounting.transcation;
using fa.reports.account.transaction;
using fa.views.utils.Common;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Report.Account.transaction
{
    public class Transaction
    {
        public static string PaperFormatNotSupportedErrorMsg = "Paper format {0} is not supported.please try some other format.";

        private EntryType GetEntryType(string Transaction)
        {
            return Transaction == "Invoice" ? EntryType.INVOICE :
                Transaction == "Bill" ? EntryType.INVOICE :
                Transaction == "Receipt" ? EntryType.INVOICE :
                Transaction == "Payment" ? EntryType.INVOICE :
                Transaction == "Debit Note" ? EntryType.DEBIT_NOTE :
                Transaction == "Credit Note" ? EntryType.CREDIT_NOTE :
                Transaction == "Journal" ? EntryType.JOURNAL : EntryType.EXPENSE;
        }
        public void ExportToFileOrPrint(RbtTransaction RbtTransaction, bool isPrint)
        {
            //CompanyEntryConfiguration CompanyEntryConfiguration = CompanyManager.Instance.GetCompanyEntryConfiguration(Global.Company, GetEntryType(RbtTransaction.Transaction));
            //if (CompanyEntryConfiguration != null && CompanyEntryConfiguration.IsDotMatrix)
            //{
                //DotMatrixPrint(RbtTransaction, isPrint);
            //}
            //else
            //{
                LaserPrint(RbtTransaction, 0L, isPrint);
            //}
        }

        readonly String[] LedgerDataTableColumn = new String[]
        {
            "Date", "Reference", "Account", "Description", "Amount"
        };

        public DataTable TransactionAlignment(RbtTransaction Transaction)
        {
            DataTable LedgerTable = new DataTable();
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DATE], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DESC], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.CR], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DR], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.BALANCE], typeof(string));
            Currency Currency = api.Accounting.CurrencyManager.Instance.GetCurrencyById((long)Global.Company.PrimaryCurrencyId);
            DataRow LedgerTableRow = null;
            foreach (TransactionLineItem LineItem in Transaction.TransactionLineItems)
            {
                LedgerTableRow = LedgerTable.NewRow();
                LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.DATE]] = LineItem.Date.ToString(Global.Company.DateFormat);
                LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.REF]] = LineItem.Reference;
                LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.ACCOUNT]] = LineItem.Account.Name;
                LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.DESC]] = LineItem.Description;
                LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.AMOUNT]] = Transaction.Transaction == "Journal" ? LineItem.Amount < 0 ? (Math.Abs(LineItem.Amount).ToString(Currency.CurrencyFormat) + "CR") : (Math.Abs(LineItem.Amount).ToString(Currency.CurrencyFormat) + "DR"): LineItem.Amount.ToString(Currency.CurrencyFormat);
                LedgerTable.Rows.Add(LedgerTableRow);
            }
            LedgerTableRow = LedgerTable.NewRow();
            LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.REF]] = string.Empty;
            LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.ACCOUNT]] = string.Empty;
            LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.DESC]] = "Total";
            LedgerTableRow[LedgerDataTableColumn[(int)TransactionTableColumn.AMOUNT]] = Transaction.Total.ToString(Currency.CurrencyFormat);
            LedgerTable.Rows.Add(LedgerTableRow);

            return LedgerTable;
        }

        public void LaserPrint(RbtTransaction RbtTransaction, long PaperFormatId, bool isPrint)
        {
            string[] PageNumberToPage = new string[1000];
            DataTable dataTable = TransactionAlignment(RbtTransaction);

            var path = AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                var pageSize = PageSize.A4;
                //PrintPaperFormat PrintPaperFormat = PaperFormatManager.Instance.GetPrintPaperFormatById(PaperFormatId);
                //if (PrintPaperFormat != null)
                //{
                //    if (PrintPaperFormat.Name == "A4 PORTRAIT")
                //    {
                //        A4Height = 760;
                //    }
                //    else if (PrintPaperFormat.Name == "A4 LANDSCAPE")
                //    {
                //        A4Height = 500;
                //        pageSize = PageSize.A4.Rotate();
                //    }
                //    else if (PrintPaperFormat.Name == "A5 LANDSCAPE")
                //    {
                //        A4Height = 335;
                //        pageSize = new Rectangle(595, 421);
                //    }
                //    else
                //    {
                //        MessageBox.Show(string.Format(PaperFormatNotSupportedErrorMsg, PrintPaperFormat.Name));
                //        return;
                //    }
                //}
                Document pdfDoc = new Document(pageSize, -30, -30, 30, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                
                
                double ItemTableHeight = A4Height;

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;

                float[] BodyTableWidths = new float[] { 25f, 25f, 35f, 45f, 25f };

                PdfPTable ReportBodyTable = null;
                ReportBodyTable = new PdfPTable(Cols);
                ReportBodyTable.SetWidths(BodyTableWidths);

                PdfPageHeader PdfHeader = new PdfPageHeader
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = RbtTransaction.ReportTitle(),
                    ReportLine2 = RbtTransaction.ReportSubTitle()
                };
                PdfPTable HTable = PdfHeader.PageHeader();

                PdfHeader = new PdfPageHeader
                {
                    IsMainHeader = false,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = RbtTransaction.ReportTitle(),
                    ReportLine2 = RbtTransaction.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "Transaction");

                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable));
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
                        if (j != (int)TransactionTableColumn.AMOUNT)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (i == Rows - 1)
                        {
                            if (j == 0 || j == 1)
                            {
                                rowCell.UseVariableBorders = true;
                                rowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 2 || j == 1)
                            {
                                rowCell.UseVariableBorders = true;
                                rowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
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
                //PdfFooter.PaperFormatId = PaperFormatId;
                byte[] PdfFileWithFooter= PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close(); 

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RbtTransaction.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
