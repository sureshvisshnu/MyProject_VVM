using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.views.utils.Common;
using Fa.report.accounting.master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace fa.views.utils.Report.Account
{
    public enum LedgerTableColumn
    {
        DATE, DESC, CR, DR, BALANCE, ID
    }
    public class Ledger
    {
        public void ExportToFileOrPrint(RptLedger AccountsLedger, bool isPrint)
        {
            if (Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES).IsDotMatrix)
            {
                DotMatrixPrint(AccountsLedger, isPrint);
            }
            else
            {
                LedgerPrint(AccountsLedger, isPrint);
            }
        }

        readonly String[] LedgerDataTableColumn = new String[]
        {
            "Date", "Description", "Credit", "Debit", "Balance", "Id"
        };

        private DataTable LedgerAlignment(RptLedger AccountsLedger)
        {
            DataTable LedgerTable = new DataTable();
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DATE], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DESC], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.CR], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.DR], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.BALANCE], typeof(string));
            LedgerTable.Columns.Add(LedgerDataTableColumn[(int)LedgerTableColumn.ID], typeof(string));

            DataRow LedgerTableRow = null;
            double totalCredit = 0;
            double totalDebit = 0;
            foreach (Fa.report.accounting.master.Ledger Ledger in AccountsLedger.Ledgers)
            {
                if (Ledger.LineItems.Count > 0 || Ledger.OpeningBalance != 0)
                {
                    LedgerTableRow = LedgerTable.NewRow();
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DATE]] = DateUtils.FormatDate(AccountsLedger.FromDate, Global.Company.DateFormat); // AccountsLedger.FromDate.Date.ToShortDateString();
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DESC]] = "Op. Balance";
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.BALANCE]] = Math.Abs(Ledger.OpeningBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + (Ledger.OpeningBalance == 0 ? "\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0" : Ledger.OpeningBalance > 0 ? " DR" : " CR");

                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.ID]] = Ledger.Account.Id;
                    totalCredit = 0;
                    totalDebit = 0;
                    LedgerTable.Rows.Add(LedgerTableRow);

                    DateTime? lDate = null;
                    double RBalance = Ledger.OpeningBalance;
                    //Print Line items
                    foreach (LedgerLineItem LineItem in Ledger.LineItems)
                    {
                        LedgerTableRow = LedgerTable.NewRow();

                        if (lDate != LineItem.Date.Date)
                        {
                            LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DATE]] = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            lDate = LineItem.Date.Date;
                        }
                        if (LineItem.CreditOrDebit() == report.common.CrDr.CR)
                        {
                            LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DESC]] = "By " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + Environment.NewLine + LineItem.Description;
                            LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            RBalance -= Math.Abs(LineItem.Amount);
                            totalCredit += Math.Abs(LineItem.Amount);
                        }
                        else
                        {
                            LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DESC]] = "To " + (LineItem.Account != null ? LineItem.Account.DisplayAs : LineItem.Patient.Name) + (!String.IsNullOrEmpty(LineItem.Description) ? Environment.NewLine + LineItem.Description : "");
                            LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(LineItem.Amount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            RBalance += Math.Abs(LineItem.Amount);
                            totalDebit += Math.Abs(LineItem.Amount);
                        }
                        LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.BALANCE]] = Math.Abs(RBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (RBalance == 0 ? "\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0" : RBalance > 0 ? " DR" : " CR"); //Math.Abs(RBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + " " + Ledger.CreditOrDebit(RBalance);
                        LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.ID]] = LineItem.Account.Id;
                        LedgerTable.Rows.Add(LedgerTableRow);
                    }
                    //Print Closing Balance
                    LedgerTableRow = LedgerTable.NewRow();
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DESC]] = "Sub Total";
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(totalCredit).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(totalDebit).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.ID]] = Ledger.Account.Id;
                    LedgerTable.Rows.Add(LedgerTableRow);

                    LedgerTableRow = LedgerTable.NewRow();
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.DESC]] = "Cl. Balance";
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.BALANCE]] = Math.Abs(Ledger.ClosingBalance).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + " " + (Ledger.ClosingBalance == 0 ? "\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0" : Ledger.ClosingBalance > 0 ? " DR" : " CR");  // Math.Abs(Ledger.ClosingBalance).ToString(TextUtils.DecimalPlace(Currency.RoundingPrecision)) + (Ledger.CreditOrDebit(Ledger.ClosingBalance) == report.common.CrDr.CR ? " CR" : " DR");
                    LedgerTableRow[LedgerDataTableColumn[(int)LedgerTableColumn.ID]] = Ledger.Account.Id;
                    LedgerTable.Rows.Add(LedgerTableRow);
                }
            }

            return LedgerTable;
        }

        //*Dot matrix printing

        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        System.IO.StreamWriter Writer = null;
        private int Line_Char = 135;
        int LineCountPerpage = 0;
        int LinePerpage = 74;

        int PageNumber = 0;
        int TotalPageNumber = 0;
        int LineCounTotalPage = 0;

        public void DotMatrixPrint(RptLedger AccountsLedger, bool isPrint)
        {
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            Writer = new System.IO.StreamWriter(windowsTempPath + AccountsLedger.ReportName() + ".txt");

            DataTable dataTable = LedgerAlignment(AccountsLedger);

            int Cols = dataTable.Columns.Count - 1;
            int Rows = dataTable.Rows.Count;
            int[] columnSize = { 20, 52, 21, 21, 21 };

            string CurrentAccountId = string.Empty;
            for (int i = 0; i < Rows; i++)
            {
                //split by account
                if (CurrentAccountId != dataTable.Rows[i][5].ToString())
                {
                    if (i != 0)
                    {
                        if (LineCountPerpage < LinePerpage)
                        {
                            DotmatrixDataAlignment.SkipLine(Writer, LinePerpage - LineCountPerpage);
                            DotmatrixDataAlignment.PrintPageNumber(Writer, Line_Char, (PageNumber + 1).ToString(), TotalPageNumber.ToString());
                            TotalPageNumber = 0;
                        }
                        DotmatrixDataAlignment.SkipLine(Writer, 6);
                        PageNumber = 0;
                        LineCountPerpage = 0;
                        LineCountPerpage += 9;
                    }

                    //company detail head
                    if (i == 0)
                    {
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        Writer.Write(DotmatrixPrint.Reverse_Paper);
                        LineCountPerpage += 10;

                    }
                    DotmatrixDataAlignment.DotmatrixHeaderTable(Writer, Line_Char, AccountsLedger.ReportTitle(), long.Parse(dataTable.Rows[i][5].ToString()));
                    LineCountPerpage += 5;
                    //ledger heading
                    DotmatrixDataAlignment.DotmatrixMainTable(Writer, Line_Char, dataTable);
                    LineCountPerpage += 2;
                    CurrentAccountId = dataTable.Rows[i][5].ToString();

                    //for page total
                    LineCounTotalPage = 0;
                    for (int p = i; p < Rows; p++)
                    {
                        LineCounTotalPage++;
                        if (CurrentAccountId != dataTable.Rows[p][5].ToString() || p == (Rows - 1))
                        {
                            int pagetotal = LineCounTotalPage + 7;
                            if (pagetotal > 70)
                            {
                                TotalPageNumber = Convert.ToInt32(((pagetotal / 70) + (pagetotal % 70 > 0 ? 1 : 0)));
                            }
                            else
                            {
                                TotalPageNumber = 1;
                            }
                            break;
                        }
                    }
                }

                //add account details
                var productDetails = "";
                for (int j = 0; j < Cols; j++)
                {
                    var temp = dataTable.Rows[i][j].ToString();
                    if (j <= 1)
                    {
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[j], AlignmentTypes.Suffix);
                    }
                    else
                    {
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[j], AlignmentTypes.Prefix);
                    }
                }

                //page spliting
                if (LineCountPerpage == 70)
                {
                    DotmatrixDataAlignment.SkipLine(Writer, 4);
                }

                if (dataTable.Rows[i][1].ToString() == "Cl. Balance")
                {
                    DotmatrixDataAlignment.PrintLine(Writer, Line_Char);
                    LineCountPerpage++;
                }
                Writer.WriteLine(productDetails);
                LineCountPerpage++;
                if (dataTable.Rows[i][1].ToString() == "Cl. Balance")
                {
                    DotmatrixDataAlignment.PrintLine(Writer, Line_Char);
                    LineCountPerpage++;
                }


            }
            if (LineCountPerpage < LinePerpage)
            {
                DotmatrixDataAlignment.SkipLine(Writer, LinePerpage - LineCountPerpage);
                //fordate
                DotmatrixDataAlignment.PrintPageNumber(Writer, Line_Char, (PageNumber + 1).ToString(), TotalPageNumber.ToString());
                PageNumber++;
            }
            Writer.Close();
            DotmatrixPrint.DoPrint(AccountsLedger.ReportName(), isPrint);

        }

        // Saving and Printing  Accounts Ledger
        public void LedgerPrint(RptLedger AccountsLedger, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                DataTable dataTable = LedgerAlignment(AccountsLedger);
                if (dataTable.Rows.Count == 0) { return; }
                Document pdfDoc = new Document(PageSize.A4, -15, -15, 30, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                fa.model.Accounting.Masters.Account lAccount = AccountManager.Instance.GetAccountById(long.Parse(dataTable.Rows[0][5].ToString()));

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = AccountsLedger.ReportTitle() + " " + lAccount.Name,
                    ReportLine2 = "Group : " + lAccount.AccountGroup.Name + "\n\n" + AccountsLedger.ReportSubTitle()  // + "GRoup : " + AccountsLedger   + "\n" + "Patient Id: " + lPatient.PatientNumber + "\n" + AccountsLedger.ReportSubTitle()

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
                    ReportLine2 = AccountsLedger.ReportTitle() + lAccount.Name,
                    //ReportLine1 = "Patient Name: " + lPatient.Name + "\n" + "Patient Id: " + lPatient.PatientNumber
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();

                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "Ledger");

                double A4Height = 760;
                double TotalWorkingOnPageH = 0;

                string AccountId = string.Empty;

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

                string CurrentAccountId = string.Empty;

                for (int i = 0; i < Rows; i++)
                {

                    PdfPCell rowCell = new PdfPCell();
                    if (CurrentAccountId != dataTable.Rows[i][5].ToString())
                    {
                        string DumyT = dataTable.Rows[i][5].ToString();

                        TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable));

                        if (i != 0)
                        {
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
                            else
                            {
                                if (!string.IsNullOrEmpty(dataTable.Rows[i][5].ToString()))
                                {
                                    fa.model.Accounting.Masters.Account lAccountsub = AccountManager.Instance.GetAccountById(long.Parse(dataTable.Rows[i][5].ToString()));

                                    PdfHeader = new PdfPageHeader()
                                    {
                                        IsMainHeader = false,
                                        Islogo = true,
                                        IsAddress = true,
                                        IsPhone = false,
                                        IsEmail = false,
                                        IsWebsite = false,
                                        IsLicenceInfo = false,
                                        //ReportLine1 = AccountsLedger.ReportTitle() + lAccountsub.Name,
                                        ReportLine2 = AccountsLedger.ReportTitle() + " " + lAccountsub.DisplayAs + "\n\n" + "Group : " + lAccountsub.AccountGroup.Name + "\n\n" + AccountsLedger.ReportSubTitle(),
                                    };
                                    PdfPTable MiniHTableSUB = PdfHeader.PageHeader();


                                    pdfDoc.Add(ReportBodyTable);
                                    pdfDoc.NewPage();
                                    pdfDoc.Add(MiniHTableSUB);
                                    pdfDoc.Add(MTable);
                                    ReportBodyTable = new PdfPTable(Cols);
                                    ReportBodyTable.SetWidths(BodyTableWidths);
                                }
                            }
                        }
                    }
                    var value = dataTable.Rows[i][1].ToString();
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
                            if (Temp.Trim() == "0.00")
                            {
                                //rowCell.PaddingRight = 17;
                            }
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 4 && Temp.Trim() == "0.00")
                        {
                            rowCell.PaddingRight = 18;
                        }
                        if (value == "Sub Total" || value == "Cl. Balance")
                        {
                            if (j == 0)
                            {
                                rowCell.UseVariableBorders = true;
                                rowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 1)
                            {
                                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                rowCell.UseVariableBorders = true;
                                rowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            if (value == "Cl. Balance")
                            {
                                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                            }
                        }
                        ReportBodyTable.AddCell(rowCell);
                    }
                    CurrentAccountId = dataTable.Rows[i][5].ToString();

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
                PdfGeneration.FileName = AccountsLedger.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
    }
}
