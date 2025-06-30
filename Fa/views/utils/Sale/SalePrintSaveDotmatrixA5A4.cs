using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static fa.views.utils.PrinterSetup;
namespace fa.views.utils.Sale
{
    public class SalePrintSaveDotmatrixA5A4
    {
        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        System.IO.StreamWriter rdr;
        public int count;
        private int Line_Char = 125;
        public string fileName;
        int TotalLines = 0;
        double SubTotal = 0.00;
        double TaxAmountTotal = 0.00;
        double LineTotal = 0.00;
        double DiscAmount = 0.00;
        public SalePrintSaveDotmatrixA5A4(object sender)
        {
            fileName = (sender as SalePrintSaveA5A4).fileName;
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            rdr = new System.IO.StreamWriter(windowsTempPath + fileName + ".txt");
        }
        public void Close()
        {
            rdr.Close();
        }
        public void GenerateDotmarixTxt(IList<TaxTable> lTaxTable, DataTable dataTable, DataTable totalTable, SaleEntry SaleEntry, string PrintPaper, bool isPrint)
        {
            if (PrintPaper == "A4 LANDSCAPE")
            {
                Line_Char = 145;
            }
            TotalLines = DotmatrixDataAlignment.SaleDotmatrixHeaderTable(rdr, Line_Char, SaleEntry);
            TotalLines += DotmatrixDataAlignment.SaleDotmatrixMainTable(rdr, Line_Char, SaleEntry);
            PrintDetails(lTaxTable, dataTable, SaleEntry, totalTable, PrintPaper);
            DotmatrixDataAlignment.SkipLine(rdr, 3);
            Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }
        public void PrintDetails(IList<TaxTable> lTaxTable, DataTable dataTable, SaleEntry SaleEntry, DataTable Total, string PrintPaper)
        {
            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count;
            int[] columnSize = new int[] { 4, 27,10, 8, 10, 10, 7,8, 16, 7, 8, 16 };
            if (PrintPaper == "A4 LANDSCAPE")
            {
                columnSize = new int[] { 4, 27,10, 10, 10,10, 9, 12, 5, 4, 6, 8, 12, 6, 7, 12 };
            }
            if (PrintPaper == "A5 LANDSCAPE")
            {
                columnSize = new int[] { 4, 27, 8, 10, 8, 8, 8, 7, 8, 12, 7, 8, 16 };
            }
            //column header
            AddColumnHeader(dataTable, PrintPaper);
            TotalLines += 2;
            for (int i = 0; i < rows; i++)
            {
                if (TotalLines > 31)
                {
                    RunningContinueTotals(dataTable, SaleEntry, PrintPaper);
                }
                var productDetails = "";
                int Index = 0;
                for (int j = 0; j < cols; j++)
                {
                    if ((j == 4 || j == 5 || j == 7 || j == 9) && PrintPaper != "A4 LANDSCAPE")
                    {
                        if (PrintPaper == "A5 LANDSCAPE" && j == 7) { }
                        else { continue; }
                    }
                    var temp = dataTable.Rows[i][j].ToString();
                    if (j <= 5)
                    {
                        if (j == 1 && temp.Contains("\n"))
                        {
                            string[] SplitText = temp.Split('\n');
                            temp = SplitText.First();
                        }
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[Index], AlignmentTypes.Suffix);
                    }
                    else
                    {
                        if (temp.Contains("\n"))
                        {
                            string[] SplitText = temp.Split('\n');
                            temp = SplitText.First();
                        }
                        if(j == 11) { DiscAmount += double.Parse(temp); }
                        if (j == 12) { SubTotal += double.Parse(temp) - DiscAmount; }
                        if (j == 14) { TaxAmountTotal += double.Parse(temp); }
                        if (j == 15) { LineTotal += double.Parse(temp); }
                        if (j == 12)
                        {
                            productDetails += DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), columnSize[Index], AlignmentTypes.Prefix);
                        }
                        else
                        {
                            productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[Index], AlignmentTypes.Prefix);
                        }                        
                    }
                    Index++;
                }
                rdr.WriteLine(productDetails);
                TotalLines++;
            }
            //total space checking
            if (Total.Rows.Count > 3 && TotalLines > 28)
            {
                RunningContinueTotals(dataTable, SaleEntry, PrintPaper);
            }
            //add empty line
            if (TotalLines < 28)
            {
                DotmatrixDataAlignment.SkipLine(rdr, (31 - TotalLines));
            }
            //add total
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            int[] TotalcolumnSize = new int[] { 70, 17, 13, 20 };
            if (PrintPaper == "A4 LANDSCAPE")
            {
                TotalcolumnSize = new int[] { 103, 12, 13, 12 };
            }
            for (int i = 0; i < Total.Rows.Count; i++)
            {
                rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][0].ToString(), TotalcolumnSize[0], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][1].ToString(), TotalcolumnSize[1], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][3].ToString(), TotalcolumnSize[2], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][4].ToString(), TotalcolumnSize[3], AlignmentTypes.Prefix) +
                        DotmatrixPrint.Bold_Off);
            }
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            DotmatrixDataAlignment.SkipLine(rdr, 1);
            if (Global.Company.SalesTaxAccountMaps.Count != 0)
            {
                TaxDetailTable(lTaxTable, dataTable, PrintPaper, SaleEntry.SaleTaxType);
            }
        }
        private void AddColumnHeader(DataTable dataTable, string PrintPaper)
        {
            int[] columnSize = new int[] { 4, 27,12, 12, 10, 6, 7,8, 16, 7, 8, 16 };
            if (PrintPaper == "A4 LANDSCAPE")
            {
                columnSize = new int[] { 4, 25,12, 10, 10, 10, 9, 12, 5, 5, 5, 8, 12, 5, 8, 12 };
            }
            if (PrintPaper == "A5 LANDSCAPE")
            {
                columnSize = new int[] { 4, 27, 8, 10, 8, 8, 8, 7, 8, 12, 7, 8, 16 };
            }
            var lineString = "";
            count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if ((column.Caption == "Batch No" || column.Caption == "Exp Date"|| column.Caption == "MRP" || column.Caption == "Free")
                    && PrintPaper != "A4 LANDSCAPE")
                {
                    if (column.Caption == "MRP" && PrintPaper == "A5 LANDSCAPE") { }
                    else { continue; }
                }
                if (count <= 4)
                {
                    if (column.Caption == "HSN/SAC Code")
                    {
                        column.Caption = "HSN/SAC";
                    }
                    lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Suffix);
                }
                else
                {
                    if (column.Caption == "Dis %")
                    {
                        column.Caption = "Dis%";
                    }
                    else if (column.Caption == "%")
                    {
                        column.Caption = "Tax %";
                    }
                    else if (column.Caption == "Dis Amount")
                    {
                        column.Caption = "Amount";
                    }
                    lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Prefix);
                }
                count++;
            }
            rdr.WriteLine(DotmatrixPrint.Bold_On + lineString + DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            TotalLines += 2;
        }
        private void RunningContinueTotals(DataTable dataTable, SaleEntry SaleEntry, string PrintPaper)
        {
            int[] columnSize = new int[] { 54, 16, 17, 13, 20 };
            if (PrintPaper == "A4 LANDSCAPE")
            {
                columnSize = new int[] { 85, 18, 12, 13, 12 };
            }
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            //running total
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("To Be Continue...", columnSize[0], AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText("Running Total   ", columnSize[1], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(), columnSize[2], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(TaxAmountTotal.ToString(), columnSize[3], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(LineTotal.ToString(), columnSize[4], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            DotmatrixDataAlignment.SkipLine(rdr, 4);
            //next paper
            //mini header
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(Global.Company.Name, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText((SaleEntry.AccountsId != null ? "Customer Name:" + SaleEntry.CustomerName : ""), (Line_Char / 2), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetCenterText((SaleEntry.EntryType == Entrytype.SALE ? "INVOICE" : "QUOTATION"), Line_Char) + DotmatrixPrint.Bold_Off);
            //line
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            //continue total
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText((SaleEntry.EntryType == Entrytype.SALE ? "INVOICE" : "QUOTATION") + ": " + SaleEntry.RefNumber, columnSize[0], AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText("Continue Total   ", columnSize[1], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(), columnSize[2], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(TaxAmountTotal.ToString(), columnSize[3], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(LineTotal.ToString(), columnSize[4], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            TotalLines = 5;
            AddColumnHeader(dataTable, PrintPaper);
        }
        private void TaxDetailTable(IList<TaxTable> lTaxTable, DataTable totalTable, string PrintPaper, SaleTaxType Type)
        {
            int numtax = Global.Company.SalesTaxAccountMaps.Count;
            numtax = Type == SaleTaxType.INTER ? (numtax > 2 ? 2 : numtax) : (numtax > 1 ? 1 : numtax);
            int TaxCol = 5 + (numtax * 2);
            int[] columnSize = new int[TaxCol];
            int j = 0;
            columnSize[j] = 13;
            for (j = 0; j < (numtax * 2); j++)
            {
                columnSize[j + 1] = (j + 1) / 2 == 0 ? 13 : 11;
            }
            columnSize[j + 1] = 25;
            columnSize[j + 2] = PrintPaper == "A4 LANDSCAPE"?50: 40;
            DotmatrixDataAlignment.PrintLine(rdr, 86);
            j = 0;
            rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("Taxable Value", columnSize[j++], AlignmentTypes.Suffix));
            foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
            {
                if ((Type == SaleTaxType.INTER && (Map.AccountId == 31 || Map.AccountId == 32)) || (Type == SaleTaxType.INTRA && Map.AccountId == 28))
                {
                    rdr.Write(
                DotmatrixDataAlignment.GetFormatedText("", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(Map.Name, columnSize[j++], AlignmentTypes.Suffix));
                }
            }
            rdr.Write(
                DotmatrixDataAlignment.GetFormatedText("Total Tax Amount", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("For " + Global.Company.Name, columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            rdr.WriteLine("");
            j = 0;
            for (int nt = 0; nt < numtax; nt++)
            {
                if (nt == 0)
                {
                    rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(" ", columnSize[j++], AlignmentTypes.Suffix));
                }
                rdr.Write(
                DotmatrixDataAlignment.GetFormatedText("%", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("Amount", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            }
            rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(" ", columnSize[j], AlignmentTypes.Suffix));
            rdr.WriteLine("");
            DotmatrixDataAlignment.PrintLine(rdr, 86);
            if (lTaxTable.Count > 0)
            {                
                double taxAmount = 0.00;
                int Count = 1;
                foreach (TaxTable llTaxTable in lTaxTable.OrderBy(x => x.SubTotal))
                {
                    j = 0;
                    if (Count == 1)
                    {
                        rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(llTaxTable.SubTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), columnSize[j++], AlignmentTypes.Suffix) + DotmatrixPrint.Bold_Off);
                    }
                    rdr.Write(
                        DotmatrixPrint.Bold_On + 
                        DotmatrixDataAlignment.GetFormatedText(llTaxTable.TaxPer.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), columnSize[j++], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(llTaxTable.TaxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), columnSize[j++], AlignmentTypes.Prefix) +
                        DotmatrixPrint.Bold_Off);
                    taxAmount += llTaxTable.TaxAmount;
                    if (Count == numtax)
                    {
                        rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(taxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), columnSize[j++], AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
                        Count = 0;
                        taxAmount = 0.00;
                        rdr.WriteLine("");
                    }
                    Count++;
                }
            }
            j = 0;
            rdr.Write(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("", columnSize[j++], AlignmentTypes.Suffix));
            foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
            {
                if ((Type == SaleTaxType.INTER && (Map.AccountId == 31 || Map.AccountId == 32)) || (Type == SaleTaxType.INTRA && Map.AccountId == 28))
                {
                    rdr.Write(
                DotmatrixDataAlignment.GetFormatedText("", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(" ", columnSize[j++], AlignmentTypes.Prefix));
                }
            }
            rdr.Write(
                DotmatrixDataAlignment.GetFormatedText("", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("Authorized Signatory", columnSize[j++], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            rdr.WriteLine("");
            DotmatrixDataAlignment.PrintLine(rdr, 86);
            if (Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations) 
                && Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails))
            {
                string[] Bankdetail = Global.Company.CompanySalesSetup.BankDetails.Split(',');
                string[] Declaration = Global.Company.CompanySalesSetup.Declarations.Split(',');
                for(int i=0;i< (Bankdetail.Length> Declaration.Length? Bankdetail.Length: Declaration.Length);i++)
                {
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText(Bankdetail.Length>i?Bankdetail[i]:"", 57, AlignmentTypes.Suffix) +
                             DotmatrixDataAlignment.GetFormatedText(Declaration.Length > i ? Declaration[i] : "", 57, AlignmentTypes.Suffix));
                }
            }
            else if (Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails))
            {
                string[] Bankdetail = Global.Company.CompanySalesSetup.BankDetails.Split(',');
                for (int i = 0; i < Bankdetail.Length; i++)
                {
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText(Bankdetail[i], 74, AlignmentTypes.Suffix));
                }
            }
            else if (Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations))
            {
                string[] Declaration = Global.Company.CompanySalesSetup.Declarations.Split(',');
                for (int i = 0; i < Declaration.Length; i++)
                {
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText(Declaration[i], 74, AlignmentTypes.Suffix));
                }
            }
            DotmatrixDataAlignment.SkipLine(rdr, 4);
        }
    }
}
