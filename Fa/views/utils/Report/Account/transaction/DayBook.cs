using fa.api.Accounting;
using fa.api.utils;
using fa.common;
using fa.report.common;
using fa.views.utils.Common;
using Fa.report.accounting.master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Accounting.Masters;
using ScottPlot.Palettes;

namespace fa.views.utils.Report.Account.transaction
{
    class DayBook
    {
        readonly String[] DayBookDataTableColumn = new String[]
        {
            "Date", "Description", "Credit", "Debit", "Balance", "Id"
        };
        public void ExportToFileOrPrint(RptDayBook RptDayBook, bool isPrint)
        {
            if (Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES).IsDotMatrix)
            {
               DotMatrixPrint(RptDayBook, isPrint);
            }
            else
            {
                LaserPrint(RptDayBook, isPrint);
            }
        }
        private DataTable DayBookAlignment(RptDayBook RptDayBook)
        {
            DataTable DayBookTable = new DataTable();
            DayBookTable.Columns.Add(DayBookDataTableColumn[(int)LedgerTableColumn.DATE], typeof(string));
            DayBookTable.Columns.Add(DayBookDataTableColumn[(int)LedgerTableColumn.DESC], typeof(string));
            DayBookTable.Columns.Add(DayBookDataTableColumn[(int)LedgerTableColumn.DR], typeof(string));
            DayBookTable.Columns.Add(DayBookDataTableColumn[(int)LedgerTableColumn.CR], typeof(string));
            DayBookTable.Columns.Add(DayBookDataTableColumn[(int)LedgerTableColumn.ID], typeof(string));
            DataRow DayBookTableRow = null;

            RptDayBookLineItem ObalLineItem = RptDayBook.OpeningBalance;
            List<RptDayBookLineItem> LineItems = (List<RptDayBookLineItem>)RptDayBook.LineItems;
            if (LineItems != null && LineItems.Count > 0)
            {
                double OpeningAmount = RptDayBook.OpeningBalance.Amount;
                double DailyBalance = 0;
                double DailyDebitTotal = 0;
                double DailyCreditTotal = 0;
                String LastDate = null;
                bool flag = false;
                string dailyDate = null;

                foreach (RptDayBookLineItem LineItem in LineItems)
                {
                    double ZeroAmount = Math.Round(LineItem.Amount, Global.Company.PrimaryCurrency.RoundingPrecision);

                    if (ZeroAmount == 0)
                    {
                        continue;
                    }
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                    if (LastDate != stringLineItemDate)
                    {
                        if (LastDate != null)
                        {
                            // Add subtotal row for the previous date
                            DayBookTableRow = DayBookTable.NewRow();
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Day Total's";
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(DailyDebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(DailyCreditTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DayBookTable.Rows.Add(DayBookTableRow);

                            // Add closing balance row for the previous date
                            DayBookTableRow = DayBookTable.NewRow();
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Cash Balance";
                            DailyBalance = DailyDebitTotal - DailyCreditTotal;
                            if (DailyDebitTotal > DailyCreditTotal)
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(DailyBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(DailyBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            DayBookTable.Rows.Add(DayBookTableRow);

                            // Set closing balance as the opening balance for the next day
                            OpeningAmount = DailyBalance;
                            ObalLineItem.Description = "Opening Balance";
                            dailyDate = null;
                        }
                        // Add opening balance row for the new date
                        DayBookTableRow = DayBookTable.NewRow();
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = DateUtils.FormatDate(RptDayBook.FromDate, Global.Company.DateFormat);
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = ObalLineItem.Description;
                        if (!flag)
                        {
                            if (ObalLineItem.CreditOrDebit() == CrDr.CR)
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(OpeningAmount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                DailyCreditTotal = 0;
                                DailyDebitTotal = 0;
                                DailyCreditTotal += Math.Abs(OpeningAmount);
                            }
                            else
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(OpeningAmount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                DailyDebitTotal = 0;
                                DailyCreditTotal = 0;
                                DailyDebitTotal += Math.Abs(OpeningAmount);
                            }
                            flag = true;
                            DailyBalance = 0;
                            LastDate = stringLineItemDate;
                        }
                        else
                        {
                            if (DailyDebitTotal > DailyCreditTotal)
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(OpeningAmount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                DailyDebitTotal = 0;
                                DailyCreditTotal = 0;
                                DailyDebitTotal += Math.Abs(OpeningAmount);
                            }
                            else
                            {
                                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(OpeningAmount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                                DailyCreditTotal = 0;
                                DailyDebitTotal = 0;
                                DailyCreditTotal += Math.Abs(OpeningAmount);
                            }
                            DailyBalance = 0;
                            LastDate = stringLineItemDate;
                        }
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.ID]] = ObalLineItem.Account.Id;
                        DayBookTable.Rows.Add(DayBookTableRow);
                    }
                    // Process the line item
                    DayBookTableRow = DayBookTable.NewRow();
                    if (dailyDate == null)
                    {
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                        dailyDate = LineItem.Date.ToString();
                    }
                    if (LineItem.CreditOrDebit() == CrDr.CR)
                    {
                        if (LineItem.Patient != null && LineItem.Patient.Id != null)
                        {
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = (!String.IsNullOrEmpty(LineItem.Description) ? LineItem.Description : "") + Environment.NewLine + "By " + (LineItem.Account != null ? LineItem.Account.Name : LineItem.Patient.Name);
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DailyCreditTotal += Math.Abs(LineItem.Amount);
                        }
                        else
                        {
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "By " + (LineItem.Account != null ? LineItem.Account.Name : LineItem.Patient.Name) + (!String.IsNullOrEmpty(LineItem.Description) ? Environment.NewLine + LineItem.Description : "");
                            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            DailyCreditTotal += Math.Abs(LineItem.Amount);
                        }
                    }
                    else
                    {
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "To " + (LineItem.Account != null ? LineItem.Account.Name : LineItem.Patient.Name) + Environment.NewLine + LineItem.Description;
                        DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        DailyDebitTotal += Math.Abs(LineItem.Amount);
                    }
                    DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.ID]] = (LineItem.Account != null ? LineItem.Account.Id : LineItem.Patient.Id);
                    DayBookTable.Rows.Add(DayBookTableRow);
                }

                // Add subtotal row for the last date
                DayBookTableRow = DayBookTable.NewRow();
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Day Total's";
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(DailyDebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(DailyCreditTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DayBookTable.Rows.Add(DayBookTableRow);

                // Add closing balance row for the last date
                DayBookTableRow = DayBookTable.NewRow();
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Cash Balance";
                DailyBalance = DailyDebitTotal - DailyCreditTotal;
                if (DailyDebitTotal > DailyCreditTotal)
                {
                    DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(DailyBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(DailyBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                DayBookTable.Rows.Add(DayBookTableRow);

                // Add total credit/debit row
                DayBookTableRow = DayBookTable.NewRow();
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Total";
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(RptDayBook.TotalCredit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(RptDayBook.TotalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.ID]] = LineItems.Last().Account != null ? LineItems.Last().Account.Id : LineItems.Last().Patient.Id;
                DayBookTable.Rows.Add(DayBookTableRow);

                // Add final closing cash balance row
                DayBookTableRow = DayBookTable.NewRow();
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Closing Cash Balance";
                double finalBalance = RptDayBook.TotalCredit - RptDayBook.TotalDebit;
                if (finalBalance > 0)
                {
                    DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(finalBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(finalBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.ID]] = LineItems.Last().Account != null ? LineItems.Last().Account.Id : LineItems.Last().Patient.Id;
                DayBookTable.Rows.Add(DayBookTableRow);
            }
            else if (RptDayBook.OpeningBalance.Amount != 0)
            {
                AddOpeningBalanceRow(DayBookTable, RptDayBook.OpeningBalance);
                AddClosingBalanceRow(DayBookTable, RptDayBook);
            }
            return DayBookTable;
        }
        private void AddOpeningBalanceRow(DataTable DayBookTable, RptDayBookLineItem LineItem)
        {
            DataRow DayBookTableRow = DayBookTable.NewRow();
            String OpeningBalanceDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = OpeningBalanceDate;
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Opening balance";
            if (LineItem.CreditOrDebit() == CrDr.CR)
            {
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            DayBookTable.Rows.Add(DayBookTableRow);
        }
        private void AddClosingBalanceRow(DataTable DayBookTable, RptDayBook dBook)
        {
            // Add "Total" row
            DataRow DayBookTableRow = DayBookTable.NewRow();
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Total";
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(dBook.TotalCredit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(dBook.TotalDebit).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            DayBookTable.Rows.Add(DayBookTableRow);

            // Add "Closing Cash Balance" row
            DayBookTableRow = DayBookTable.NewRow();
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DATE]] = null;
            DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DESC]] = "Closing Cash Balance";

            // Calculate closing balance and assign to correct column
            double closingBalance = dBook.TotalCredit - dBook.TotalDebit;
            if (closingBalance > 0)
            {
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.CR]] = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            else
            {
                DayBookTableRow[DayBookDataTableColumn[(int)LedgerTableColumn.DR]] = Math.Abs(closingBalance).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            }
            DayBookTable.Rows.Add(DayBookTableRow);
        }

        private void CalculateColumnsTwoAndThree(PdfPTable reportBodyTable, int startRow, int endRow, out double debitAmount, out double creditAmount)
        {
            debitAmount = 0;
            creditAmount = 0;

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = 0; j < reportBodyTable.NumberOfColumns; j++)
                {
                    PdfPCell cell = reportBodyTable.Rows[i].GetCells()[j];
                    if (j == 2) 
                    {
                        double tempDebit;
                        if (double.TryParse(cell.Phrase.Content, out tempDebit))
                        {
                            debitAmount += tempDebit;
                        }
                    }
                    else if (j == 3)
                    {
                        double tempCredit;
                        if (double.TryParse(cell.Phrase.Content, out tempCredit))
                        {
                            creditAmount += tempCredit;
                        }
                    }
                }
            }
        }
        public void LaserPrint(RptDayBook RptDayBook, bool isPrint)
        {
            int PageIndex = 0;
            string[] PageNumberToPage = new string[1000];
            DataTable dataTable = DayBookAlignment(RptDayBook);
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();
                double A4Height = 760;
                double ItemTableHeight = A4Height;
                double TotalWorkingOnPageH = 0;
                double TotalWorkingOnPageHeightWothoutBody = 0;

                double HeadTableHeight = 0;
                double MainTableHeight = 0;
                double SplitingHeight = 0;
                double debitAmount = 0;
                double creditAmount = 0;

                int Cols = dataTable.Columns.Count - 1;
                int Rows = dataTable.Rows.Count;

                float[] BodyTableWidths = new float[] { 25f, 60f, 25f, 30f };
                float[] PageNumberTableWidths = new float[] { 50f, 50f };

                PdfPTable ReportBodyTable = null;
                PdfPTable PageTable = null;
                int PageNumber = 0;
                int TotalPageNumber = 0;
                int AddedCell = 0;
                int? NPage = null;

                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    // Order by Account
                    if (i < Rows)
                    {
                        var Tempe = dataTable.Rows[i][1].ToString();
                        if (Tempe == "Cash Balance")
                        {
                            if (i != Rows - 3)
                            {
                                NPage = i;
                            }
                        }
                    }
                    if (i == 0 || i == NPage + 1)
                    {
                        TotalWorkingOnPageHeightWothoutBody = (HeadTableHeight + MainTableHeight) - SplitingHeight;
                        if (i != 0)
                        {
                            TotalWorkingOnPageH = (HeadTableHeight + MainTableHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable)) - SplitingHeight;
                            for (int j = 1; j < Cols; j++)
                            {
                                ReportBodyTable.AddCell(PdfDataAlignment.CreateEmptyCellWithTopBlackBorder());
                                AddedCell++;
                            }
                            // THis is Calculate in C/O Oppening Balance Amount
                            if (!ShouldSkipAddOneRow(ReportBodyTable))
                            {
                                AddOneRow(ReportBodyTable, Cols, debitAmount, creditAmount);
                            }
                            PageNumberToPage[PageIndex] = (PageNumber + 1).ToString() + " of " + TotalPageNumber.ToString();
                            PageIndex++;
                            //*Added account detail table
                            pdfDoc.Add(ReportBodyTable);
                            pdfDoc.NewPage();
                            AddedCell = 0;
                            PageNumber = 0;
                        }

                        //*Create account detail table
                        ReportBodyTable = new PdfPTable(Cols);
                        ReportBodyTable.SetWidths(BodyTableWidths);

                        //*Add Company Detail table
                        PdfPTable HTable = PdfDataAlignment.LedgerDaybookHeaderTable(RptDayBook.ReportTitle(), 0L);
                        pdfDoc.Add(HTable);
                        HeadTableHeight = HTable.TotalHeight;

                        //*Added Ledger heading table
                        PdfPTable MTable = PdfDataAlignment.LedgerDaybookMainTable(dataTable, "DayBook");
                        for (int h = 0; h < MTable.Rows[0].GetCells().Length; h++)
                        {
                            ReportBodyTable.AddCell(MTable.Rows[0].GetCells()[h]);
                        }
                        ReportBodyTable.HeaderRows = 1;

                        MainTableHeight = MTable.TotalHeight;
                        SplitingHeight = 0;

                        // for page count per account         
                        PageTable = new PdfPTable(Cols);
                        PageTable.SetWidths(BodyTableWidths);
                        for (int p = i; p < Rows; p++)
                        {
                            for (int j = 0; j < Cols; j++)
                            {
                                var Temp = dataTable.Rows[p][j].ToString();
                                rowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                                rowCell.BorderColor = BaseColor.BLACK;
                                rowCell.MinimumHeight = 20;
                                PageTable.AddCell(rowCell);
                            }
                            if (p == 1) { continue; }
                            if (!string.IsNullOrEmpty(dataTable.Rows[p][0].ToString()) || p == (Rows - 1))
                            {
                                double pagetotal = PdfDataAlignment.CalculatePdfTableHeight(PageTable) + HeadTableHeight + MainTableHeight;
                                if (pagetotal > 730)
                                {
                                    TotalPageNumber = Convert.ToInt32(((pagetotal / 730) + (pagetotal % 730 > 0 ? 1 : 0)));
                                    for (int n = 1; n < TotalPageNumber; n++)
                                    {
                                        PageNumberToPage[PageIndex] = (PageNumber + 1).ToString() + " of " + TotalPageNumber.ToString();
                                        PageIndex++;
                                        PageNumber++;
                                    }
                                }
                                else
                                {
                                    TotalPageNumber = 1;
                                }
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = dataTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.BorderColor = BaseColor.BLACK;
                        rowCell.MinimumHeight = 20;
                        rowCell.BorderWidthTop = (float)BorderStyle.None;
                        rowCell.Padding = 2;
                        if (j != 0)
                        {
                            rowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        if (j == 0 || j == 1)
                        {
                            if (j == 0 && (dataTable.Rows[i][j + 1].ToString() == "Day Total's" || dataTable.Rows[i][j + 1].ToString() == "Cash Balance"))
                            {
                                rowCell.UseVariableBorders = true;
                                rowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (Temp == "Day Total's" || Temp == "Cash Balance")
                        {
                            rowCell.UseVariableBorders = true;
                            rowCell.BorderWidthLeft = (float)BorderStyle.None;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (dataTable.Rows[i][1].ToString() == "Day Total's")
                        {
                            rowCell.UseVariableBorders = true;
                            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
                        }
                        if (dataTable.Rows[i][1].ToString() == "Cash Balance")
                        {
                            rowCell.UseVariableBorders = true;
                            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                        }
                        if (i == Rows - 1 || i == Rows - 2)
                        {
                            rowCell.BackgroundColor = new BaseColor(170, 170, 170);
                            if (j == 0 || j == 1)
                            {
                                rowCell.UseVariableBorders = true;
                                if (j == 0)
                                {
                                    rowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                if (j == 1)
                                {
                                    rowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                        }
                        ReportBodyTable.AddCell(rowCell);
                        AddedCell++;

                        // Check if page needs to be split
                        TotalWorkingOnPageH = (HeadTableHeight + MainTableHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable)) - SplitingHeight;
                        if (TotalWorkingOnPageH > 730)
                        {
                            TotalWorkingOnPageHeightWothoutBody = (HeadTableHeight + MainTableHeight) - SplitingHeight;

                            // THis is Calculate in C/O Cash Balance & B/F Day Total's Amount
                            if (!ShouldSkipAddTwoRows(ReportBodyTable))
                            {
                                debitAmount = 0;
                                creditAmount = 0;
                                CalculateColumnsTwoAndThree(ReportBodyTable, 0, ReportBodyTable.Rows.Count - 1, out debitAmount, out creditAmount);

                                AddTwoRows(ReportBodyTable, Cols, debitAmount, creditAmount);
                            }


                            PageNumberToPage[PageIndex] = (PageNumber + 1).ToString() + " of " + TotalPageNumber.ToString();
                            PageIndex++;

                            //*Added account detail table
                            pdfDoc.Add(ReportBodyTable);
                            pdfDoc.NewPage();

                            //*Create account detail table for the new page
                            ReportBodyTable = new PdfPTable(Cols);
                            ReportBodyTable.SetWidths(BodyTableWidths);

                            // Add headers to the new table
                            PdfPTable MTable = PdfDataAlignment.LedgerDaybookMainTable(dataTable, "DayBook");
                            for (int h = 0; h < MTable.Rows[0].GetCells().Length; h++)
                            {
                                ReportBodyTable.AddCell(MTable.Rows[0].GetCells()[h]);
                            }
                            ReportBodyTable.HeaderRows = 1;
                        }
                    }
                }
                TotalWorkingOnPageHeightWothoutBody = (HeadTableHeight + MainTableHeight) - SplitingHeight;
                TotalWorkingOnPageH = (HeadTableHeight + MainTableHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportBodyTable)) - SplitingHeight;
                for (int j = 1; j < Cols; j++)
                {
                    ReportBodyTable.AddCell(PdfDataAlignment.CreateEmptyCellWithTopBlackBorder());
                    AddedCell++;
                }
                if (TotalWorkingOnPageH > 660)
                {
                    TotalWorkingOnPageH = (TotalWorkingOnPageH % 660);
                }
                // THis is Calculate in C/O Oppening Balance Amount For Last Page
                if (!ShouldSkipAddOneRow(ReportBodyTable))
                {
                    if(TotalPageNumber != 1)
                    {
                        AddOneRow(ReportBodyTable, Cols, debitAmount, creditAmount);
                    }
                }
                PageNumberToPage[PageIndex] = (PageNumber + 1).ToString() + " of " + TotalPageNumber.ToString();
                PageIndex++;
                //Added account detail table
                pdfDoc.Add(ReportBodyTable);
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStreamLedger(myMemoryStream, RptDayBook.ReportName(), "pdf", isPrint, PaperTypes.A4_PORTRAIT, PageNumberToPage);
            }
        }
        //This is C/O Opening Balance Row Add
        private void AddOneRow(PdfPTable table, int cols, double debitTotal, double creditTotal)
        {
            List<PdfPCell> newRowCells = new List<PdfPCell>();    
            for (int j = 0; j < cols; j++)
            {
                PdfPCell cell;
                if (j == 0)
                {
                    Phrase phrase = new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else if (j == 1)
                {
                    Phrase phrase = new Phrase("C/O Opening Balance", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else if (j == 2)
                {
                    Phrase phrase = new Phrase(debitTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else if (j == 3)
                {
                    Phrase phrase = new Phrase(creditTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    cell = PdfDataAlignment.CreateEmptyCellWithTopBlackBorder();
                }

                cell.MinimumHeight = 20;
                newRowCells.Add(cell);
            }
            table.Rows.Insert(1, new PdfPRow(newRowCells.ToArray()));
        }
        //This is C/O Cash Balance & B/F Day Total's Rows Add
        private void AddTwoRows(PdfPTable table, int cols, double debitTotal, double creditTotal)
        {
            int totalRows = table.Rows.Count;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    PdfPCell cell;

                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            Phrase phrase = new Phrase("B/F Day Total's", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                            cell.Colspan = 2;
                            j++;
                        }
                        else if (j == 2)
                        {
                            Phrase phrase = new Phrase(Math.Abs(debitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                        }
                        else if (j == 3)
                        {
                            Phrase phrase = new Phrase(Math.Abs(creditTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                        }
                        else
                        {
                            continue;
                        }
                        cell.BackgroundColor = new BaseColor(230, 230, 230);
                    }
                    else if (i == 1)
                    {
                        if (j == 0)
                        {
                            Phrase phrase = new Phrase("C/O Cash Balance", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                            cell.Colspan = 2;
                            j++;
                        }
                        else if (j == 2)
                        {
                            Phrase phrase = new Phrase(Math.Abs(debitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                        }
                        else if (j == 3)
                        {
                            Phrase phrase = new Phrase(Math.Abs(creditTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                            cell = new PdfPCell(phrase);
                        }
                        else
                        {
                            continue;
                        }
                        cell.BackgroundColor = new BaseColor(200, 200, 200);
                    }
                    else
                    {
                        continue;
                    }

                    cell.BorderColor = BaseColor.BLACK;
                    cell.MinimumHeight = 20;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                }
            }
        }
        //This is Skip C/O Cash Balance & B/F Day Total's Rows Add
        private bool ShouldSkipAddTwoRows(PdfPTable table)
        {
            int rowCount = table.Rows.Count;
            for (int i = Math.Max(0, rowCount - 3); i < rowCount; i++)
            {
                foreach (PdfPCell cell in table.Rows[i].GetCells())
                {
                    if (cell.Phrase != null &&
                        (cell.Phrase.Content == "Day Total's" || cell.Phrase.Content == "Cash Balance"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //This is Skip C/O Opening Balance Row Add
        private bool ShouldSkipAddOneRow(PdfPTable table)
        {
            int rowCount = table.Rows.Count;
            for (int i = 0; i < Math.Min(2, rowCount); i++)
            {
                foreach (PdfPCell cell in table.Rows[i].GetCells())
                {
                    if (cell.Phrase != null && cell.Phrase.Content == "Opening Balance")
                    {
                        return true;
                    }
                }
            }
            return false;
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

        public void DotMatrixPrint(RptDayBook RptDayBook, bool isPrint)
        {
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            Writer = new System.IO.StreamWriter(windowsTempPath + RptDayBook.ReportName() + ".txt");

            DataTable dataTable = DayBookAlignment(RptDayBook);

            int Cols = dataTable.Columns.Count - 1;
            int Rows = dataTable.Rows.Count;
            int[] columnSize = { 20, 73, 21, 21 };

            for (int i = 0; i < Rows; i++)
            {
                //split by account
                if (!string.IsNullOrEmpty(dataTable.Rows[i][0].ToString()) && i != 1)
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
                    DotmatrixDataAlignment.DotmatrixHeaderTable(Writer, Line_Char, RptDayBook.ReportTitle(), 0L);
                    LineCountPerpage += 5;
                    //ledger heading
                    DotmatrixDataAlignment.DotmatrixMainTable(Writer, Line_Char, dataTable);
                    LineCountPerpage += 2;

                    //for page total
                    LineCounTotalPage = 0;
                    for (int p = i; p < Rows; p++)
                    {
                        LineCounTotalPage++;
                        if (p == 1) { continue; }
                        if (!string.IsNullOrEmpty(dataTable.Rows[p][0].ToString()) || p == (Rows - 1))
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
                    if(j==1)
                    {
                        temp=temp.Replace("\r\n"," ");
                    }
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

                if (dataTable.Rows[i][1].ToString() == "Total" || dataTable.Rows[i][1].ToString() == "Closing Cash Balance")
                {
                    DotmatrixDataAlignment.PrintLine(Writer, Line_Char);
                    LineCountPerpage++;
                }
                Writer.WriteLine(productDetails);
                LineCountPerpage++;
                if (dataTable.Rows[i][1].ToString() == "Closing Cash Balance")
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
            DotmatrixPrint.DoPrint(RptDayBook.ReportName(), isPrint);
        }
    }
}
