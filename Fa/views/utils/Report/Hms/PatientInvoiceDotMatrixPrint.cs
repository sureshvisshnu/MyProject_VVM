using Fa.api.Hms;
using fa.model.Hms.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.views.utils.Sale.SalePrintSaveA5A4;
using fa.model.OrderManagement;
using static fa.views.utils.PrinterSetup;
using VisioForge.MediaFramework.FFMPEGCore.Enums;
using fa.views.utils.Common;
using fa.views.utils.Report.Hms;
using DocumentFormat.OpenXml.Wordprocessing;
using fa.model.Hms.Master;
using fa.model.Hms.Ip;
using fa.model.Hms.Op;
using fa.views.utils;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa;
using fa.model.Accounting.Masters;

namespace Fa.views.utils.Report.Hms
{
    public class PatientInvoiceDotMatrixPrint
    {
        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        private int Line_Char = 125;
        public string fileName;
        int TotalLines = 0;
        double SubTotal = 0.00;
        System.IO.StreamWriter rdr;
        public PatientInvoiceDotMatrixPrint(object sender)
        {
            fileName = (sender as PatientInvoicePrint).fileName;
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            rdr = new System.IO.StreamWriter(windowsTempPath + fileName + ".txt");
        }

        public void Close()
        {
            rdr.Close();
        }
        public void GenerateDotmarixPrint(Patient patient, PatientInvoice Invoice, bool isPrint, PatientLedger ledger, InPatientAdmission InPatientAdmission, Registration Registration, InPatientLocation InPatientLocation, Registration Consultant, DataTable dataTable, DataTable TotalDataTable)
        {
            TotalLines = DotmatrixDataAlignment.PatientInvoiceDotmatrixHeaderTable(rdr, Line_Char);
            TotalLines += DotmatrixDataAlignment.PatientInvoiceDotmatrixMainTable(rdr, Line_Char, Invoice, patient, ledger, InPatientAdmission, Registration, InPatientLocation, Consultant);
            PrintDetails(dataTable, TotalDataTable, Invoice);
            DotmatrixDataAlignment.SkipLine(rdr, 3);
            Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }

        private void PrintDetails(DataTable dataTable, DataTable TotalDataTable, PatientInvoice Invoice)
        {
            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count;
            int[] columnSize = new int[] { 6, 16, 6, 40, 10, 12, 12 };
            //column header
            AddColumnHeader(dataTable);
            TotalLines += 2;
            for (int i = 0; i < rows; i++)
            {
                if (TotalLines > 31)
                {
                    RunningContinueTotals(dataTable, Invoice);
                }
                var productDetails = "";
                int Index = 0;
                for (int j = 0; j < cols; j++)
                {
                    var temp = dataTable.Rows[i][j].ToString();
                    if (j <= 3)
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
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[Index], AlignmentTypes.Prefix);
                    }
                    Index++;
                }
                rdr.WriteLine(productDetails);
                TotalLines++;
                SubTotal += Convert.ToDouble(dataTable.Rows[i][6]);
            }
            //total space checking
            if (TotalDataTable.Rows.Count > 1 && TotalLines > 28)
            {
                RunningContinueTotals(dataTable, Invoice);
            }
            //add total
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            int[] TotalcolumnSize = new int[] { 87, 12 };
            for (int i = 0; i < TotalDataTable.Rows.Count; i++)
            {
                rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(TotalDataTable.Rows[i][0].ToString()!, TotalcolumnSize[0], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(TotalDataTable.Rows[i][1].ToString()!, TotalcolumnSize[1], AlignmentTypes.Prefix) +
                        DotmatrixPrint.Bold_Off);
                DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            }

            DotmatrixDataAlignment.SkipLine(rdr, 1);
            DotmatrixDataAlignment.SkipLine(rdr, 1);
            
            rdr.Write(
                DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("", 77, AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText("Authorized Signatory", 20, AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            rdr.WriteLine("");
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            if (Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations)
                && Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails))
            {
                string[] Bankdetail = Global.Company.CompanySalesSetup.BankDetails.Split(',');
                string[] Declaration = Global.Company.CompanySalesSetup.Declarations.Split(',');
                for (int i = 0; i < (Bankdetail.Length > Declaration.Length ? Bankdetail.Length : Declaration.Length); i++)
                {
                    rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText(Bankdetail.Length > i ? Bankdetail[i] : "", 77, AlignmentTypes.Suffix) +
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
        private void AddColumnHeader(DataTable dataTable)
        {
            int[] columnSize = new int[] { 6, 14, 8, 40, 12, 12, 12 };
            
            var lineString = "";
            int count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Suffix);
                count++;
            }
            rdr.WriteLine(DotmatrixPrint.Bold_On + lineString + DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            TotalLines += 2;
        }
        private void RunningContinueTotals(DataTable dataTable, PatientInvoice Invoice)
        {
            int[] columnSize = new int[] { 68, 20, 10, 13, 20 };
            
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            //running total
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("To Be Continue...", columnSize[0], AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText("Running Total   ", columnSize[1], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(), columnSize[2], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            DotmatrixDataAlignment.SkipLine(rdr, 4);
            //next paper
            //mini header
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(Global.Company.Name, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText((""), (Line_Char / 2), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetCenterText(("INVOICE"), Line_Char) + DotmatrixPrint.Bold_Off);
            //line
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            //continue total
            rdr.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(("INVOICE") + ": " + Invoice.ReferenceNumber, columnSize[0], AlignmentTypes.Suffix) +
                DotmatrixDataAlignment.GetFormatedText("Continue Total   ", columnSize[1], AlignmentTypes.Prefix) +
                DotmatrixDataAlignment.GetFormatedText(SubTotal.ToString(), columnSize[2], AlignmentTypes.Prefix) +
                DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            TotalLines = 5;
            AddColumnHeader(dataTable);
        }
    }
}
