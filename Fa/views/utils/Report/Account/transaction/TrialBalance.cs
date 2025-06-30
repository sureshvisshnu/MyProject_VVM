using fa;
using fa.model.Accounting.Masters;
using fa.reports.account.transaction.trialbalance;
using fa.views.utils.Common;
using fa.views.utils;
using Fa.report.accounting.master;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.utils.Report.Account.transaction
{
    internal class TrialBalance
    {
        readonly String[] TrialBalanceDataTableColumn = new String[]
        {
            "Account Name", "Credit", "Debit"
        };
        public DataTable AsDataTable(RptTrialBalance RptTrialBalance)
        {
            DataTable TrialBalanceDataTable = new DataTable();
            TrialBalanceDataTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.ACCOUNT_NAME], typeof(string));
            TrialBalanceDataTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.CREDIT], typeof(string));
            TrialBalanceDataTable.Columns.Add(TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.DEBIT], typeof(string));
            string AccountType = string.Empty;
            double CreitTotal = 0.00;
            double DebitTotal = 0.00;
            DataRow TrialBalanceDataTableRow = null;
            foreach (RptTrialBalanceLineItem LineItem in RptTrialBalance.LineItems.OrderBy(x => x.AccountType))
            {
                if (AccountType != LineItem.AccountType.ToString())
                {
                    TrialBalanceDataTableRow = TrialBalanceDataTable.NewRow();
                    TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.ACCOUNT_NAME]]= LineItem.AccountType.ToString();
                    AccountType = LineItem.AccountType.ToString();
                    TrialBalanceDataTable.Rows.Add(TrialBalanceDataTableRow);
                }
                TrialBalanceDataTableRow = TrialBalanceDataTable.NewRow();
                TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.ACCOUNT_NAME]] = LineItem.Name;
                if (LineItem.Nature == fa.report.common.CrDr.CR)
                {
                    TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.CREDIT]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    CreitTotal += Math.Abs(LineItem.Amount);
                }
                else
                {
                    TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.DEBIT]] = Math.Abs(LineItem.Amount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    DebitTotal += Math.Abs(LineItem.Amount);
                }
                TrialBalanceDataTable.Rows.Add(TrialBalanceDataTableRow);
            }
            //Balance Printing
            TrialBalanceDataTableRow = TrialBalanceDataTable.NewRow();
            TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.ACCOUNT_NAME]] = "Total";
            TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.CREDIT]] = Math.Abs(CreitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            TrialBalanceDataTableRow[TrialBalanceDataTableColumn[(int)TrialBalanceGridColumn.DEBIT]] = Math.Abs(DebitTotal).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            TrialBalanceDataTable.Rows.Add(TrialBalanceDataTableRow);
            return TrialBalanceDataTable;
        }       
        public void GeneratePDF(RptTrialBalance RptTrialBalance, string ReportName, string fileExtension, bool isPrint)
        {
            var DataTable = AsDataTable(RptTrialBalance);
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
                    ReportLine1 = RptTrialBalance.ReportTitle(),
                    ReportLine2 = RptTrialBalance.ReportSubTitle()

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
                    ReportLine1 = RptTrialBalance.ReportTitle(),
                    ReportLine2 = RptTrialBalance.ReportSubTitle()

                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, ReportName);
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 50f, 25f, 25f };
                
                ReportMainTable.SetWidths(widths);
                int k = 2;                
                Cursor.Current = Cursors.WaitCursor;
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
                        k = 2;
                    }
                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);

                        if (DataTable.Columns[j].ColumnName == "Credit" || DataTable.Columns[j].ColumnName == "Debit")
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }
                        if (string.IsNullOrEmpty(Temp))
                        {
                            if (j == (int)TrialBalanceGridColumn.DEBIT)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            else if (j == (int)TrialBalanceGridColumn.ACCOUNT_NAME)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.BLACK;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                            }
                            else
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.BorderColorRight = BaseColor.WHITE;
                            }
                        }
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
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
                PdfGeneration.FileName = RptTrialBalance.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }

    }
}
