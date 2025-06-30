using fa.model.OrderManagement;
using fa.views.utils.Common;
using System.Data;
using System.IO;
using System.Linq;

namespace fa.views.utils.Sale
{
    public class SalePrintSaveDotmatrixPrePrintA5A4
    {
        public string fileName;
       

        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        System.IO.StreamWriter rdr;
        public int count;
        private int Line_Char = 135;

        int TotalLines = 0;
        bool IsTax = false;
        string TaxContent = string.Empty;
        double Qty = 0.00;
        double SubTotal = 0.00;
        double TaxAmountTotal = 0.00;
        double LineTotal = 0.00;
        public SalePrintSaveDotmatrixPrePrintA5A4(object sender)
        {
            fileName = (sender as SalePrintSaveA5A4).fileName;
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            rdr = new System.IO.StreamWriter(windowsTempPath + fileName + ".txt");
        }
        public  void Close()
        {
            rdr.Close();
        }
        public void GenerateDotmarixTxt(DataTable dataTable, DataTable totalTable, SaleEntry SaleEntry, string PrintPaper, string fileExtension, bool isPrint)
        {
            DotmatrixDataAlignment.SaleDotmatrixHeaderTableForPreprint(rdr, Line_Char, SaleEntry,false);
            DotmatrixDataAlignment.SkipLine(rdr, 4);
            PrintDetails(dataTable, SaleEntry, totalTable, PrintPaper);
            DotmatrixDataAlignment.SkipLine(rdr,13);
            Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }

        public void PrintDetails(DataTable dataTable, SaleEntry SaleEntry, DataTable Total, string PrintPaper)
        {
            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count;
            
            int[] columnSize = new int[] { 4, 35,10, 10, 9, 8, 10,10, 10, 8, 0, 8, 0,0, 0, 0, 10 };

            for (int i = 0; i < rows; i++)
            {             
                if (TotalLines > 11)
                {            
                    //running total           
                    rdr.WriteLine();                   
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("", (20), AlignmentTypes.Suffix) +
               DotmatrixDataAlignment.GetFormatedText((i).ToString(), (30), AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText(Qty.ToString(), (15), AlignmentTypes.Suffix) +
                 DotmatrixDataAlignment.GetFormatedText("To Be Continue.. Running Total   ", (53), AlignmentTypes.Suffix) +
                 DotmatrixDataAlignment.GetFormatedText(LineTotal.ToString(), (20), AlignmentTypes.Suffix));
                    rdr.WriteLine();
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("To Be Continue.. Running Total   ", (45), AlignmentTypes.Suffix));
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("", (20), AlignmentTypes.Suffix) +
              DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(), (55), AlignmentTypes.Suffix) +
               DotmatrixDataAlignment.GetFormatedText((TaxAmountTotal / 2).ToString(), (35), AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText((TaxAmountTotal / 2).ToString(), (20), AlignmentTypes.Suffix));

                    RunningContinueTotals(dataTable, SaleEntry, PrintPaper);

                    DotmatrixDataAlignment.SaleDotmatrixHeaderTableForPreprint(rdr, Line_Char, SaleEntry,true);
                    DotmatrixDataAlignment.SkipLine(rdr, 4);
                    TotalLines = 0;
                }
                IsTax = false;
                TaxContent = string.Empty;
                var productDetails = "";
                int Index = 0;
                for (int j = 0; j < cols; j++)
                {
                   
                    if ((j == 2 || j == 10 || j == 12 || j == 14 ))
                    {
                        continue;
                    }
                    var temp = dataTable.Rows[i][j].ToString();
                    if (j == 15)
                    {
                        TaxAmountTotal += double.Parse(temp); continue;
                    }
                    if (j == 13) { SubTotal += double.Parse(temp); continue; }


                    if (j <= 6)
                    {
                        //check item tax
                        if (j == 1 && temp.Contains("\n"))
                        {
                            string[] SplitText = temp.Split('\n');
                            temp = SplitText.First();
                        }
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[Index], AlignmentTypes.Suffix);
                    }
                    else
                    {
                        if (j == 9)
                        { temp = (double.Parse(temp) + double.Parse(dataTable.Rows[i][j + 1].ToString())).ToString(); Qty += double.Parse(temp); }
                        if (j == 16) { LineTotal += double.Parse(temp); }
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[Index-3], AlignmentTypes.Prefix);
                    }
                    Index++;
                }
                rdr.WriteLine(productDetails);
                TotalLines++;
                //add item tax
                if (IsTax)
                {
                    rdr.WriteLine(TaxContent);
                }

            }
            
            //add empty line
            if (TotalLines < 11)
            {
                DotmatrixDataAlignment.SkipLine(rdr, (13 - TotalLines));
            }
            //add total
            int[] TotalcolumnSize = TotalcolumnSize = new int[] { 93, 12, 13, 12 };

            for (int i = 0; i < Total.Rows.Count; i++)
            {
                if(Total.Rows[i][0].ToString()== "Grant Total")
                {
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("", (20), AlignmentTypes.Suffix) +
               DotmatrixDataAlignment.GetFormatedText(SaleEntry.SaleDetails.Count.ToString(), (30), AlignmentTypes.Suffix)+
                DotmatrixDataAlignment.GetFormatedText(Qty.ToString(), (68), AlignmentTypes.Suffix)+
                 DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][4].ToString(), (20), AlignmentTypes.Suffix));
                    rdr.WriteLine();
                    rdr.WriteLine();
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("", (20), AlignmentTypes.Suffix) +
               DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][1].ToString(), (55), AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][2].ToString(), (35), AlignmentTypes.Suffix) +
                 DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][3].ToString(), (20), AlignmentTypes.Suffix));
                    continue;
                }
                if (Total.Rows[i][0].ToString() == "Round off")
                {
                    rdr.WriteLine();
                    rdr.WriteLine(DotmatrixPrint.Enlarged + DotmatrixDataAlignment.GetFormatedText("", (30), AlignmentTypes.Suffix) +
               DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][4].ToString(), (30), AlignmentTypes.Suffix)+ DotmatrixPrint.Enlargedoff);
                    continue;
                }
                rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][0].ToString(), TotalcolumnSize[0], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][1].ToString(), TotalcolumnSize[1], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][3].ToString(), TotalcolumnSize[2], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][4].ToString(), TotalcolumnSize[3], AlignmentTypes.Prefix) +
                        DotmatrixPrint.Bold_Off);
            }
        }
       
        private void RunningContinueTotals(DataTable dataTable, SaleEntry SaleEntry, string PrintPaper)
        {      
            int[] columnSize = new int[] { 70, 18, 8, 13, 12 };          
            //next paper
            //continue total
            DotmatrixDataAlignment.SkipLine(rdr, 5);
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText((SaleEntry.EntryType == Entrytype.SALE ? "INVOICE" : "QUOTATION") + ": " + SaleEntry.RefNumber, columnSize[0], AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText("Continue Total   ", columnSize[1], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("", columnSize[2], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("", columnSize[3], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(LineTotal.ToString(), columnSize[4], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.SkipLine(rdr, 1);
        }
    }
}
