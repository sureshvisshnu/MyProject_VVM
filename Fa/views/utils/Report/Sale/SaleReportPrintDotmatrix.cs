using fa.views.utils.Common;
using fa.views.utils;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using System.Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using fa.views.utils.Sale;
using System.Data;
using DataTable = System.Data.DataTable;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa;
using fa.views.controls.text;
using fa.views.utils.Report.Sale;

namespace Fa.views.utils.Report.Sale
{
    public class SaleReportPrintDotmatrix
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

        public SaleReportPrintDotmatrix(object sender)
        {
            fileName = (sender as SaleReportPrintSaveA4).fileName;
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            rdr = new System.IO.StreamWriter(windowsTempPath + fileName + ".txt");
        }
        public void Close()
        {
            rdr.Close();
        }
        public void GeneratePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            int Totallines = 6;
            string CompanyName = Global.Company.DisplayAs;
            string blankSpace = "";
            if (CompanyName.Count() < Line_Char)
            {
                int ct = CompanyName.Count();
                for (var i = 0; i < (ct - Line_Char) / 2; i++)
                {
                    blankSpace += " ";
                }
            }
            CompanyName = blankSpace + Global.Company.DisplayAs;
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string contactNo = Global.Company.ContactInfo != null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Phone) ? "Contact : " + Global.Company.ContactInfo.Phone.ToString() : "";
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;
            Address = Global.Company.Address != null ? Global.Company.Address.FullAddressInSingleLine : "";
            if (Global.Company.ContactInfo != null && Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "Phone: " + Global.Company.ContactInfo.Phone + fax + "\n";
            }
            if (Global.Company.ContactInfo != null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
            {
                Email = Global.Company.ContactInfo.Email == "" ? "" : "Email: " + (Global.Company.ContactInfo).Email + "\n";
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
            {
                Web = Global.Company.ContactInfo.WebSite == "" ? "" : "Web: " + (Global.Company.ContactInfo).WebSite + "\n\n";
            }
            //Company license Info
            if (Global.Company.CompanyLicence != null && Global.Company.CompanyLicence.Count > 0)
            {
                foreach (CompanyLicence Licence in Global.Company.CompanyLicence)
                {
                    if (Licence.IncludeInInvoice)
                    {
                        CompanyLicense = (string.IsNullOrEmpty(CompanyLicense) ? CompanyLicense : CompanyLicense + ", ") + (Licence.DisplayName + ": " + Licence.Value);
                    }
                }
            }
            //Heading
            string[] Headings = new string[2];
            Headings[0] = heading;
            Headings[1] = (fileName == "SalesReportBySchedule" ? "\n" + FromDate + " to " + Todate : "");

            string[] Companylines = new string[10];
            int k = 0;
            if (!string.IsNullOrEmpty(CompanyName))
            {
                Companylines[k] = CompanyName;
                k++;
            }
            if (!string.IsNullOrEmpty(Address))
            {
                Companylines[k] = Address;
                k++;
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                Companylines[k] = Phone;
                k++;
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Companylines[k] = Email;
                k++;
            }
            if (!string.IsNullOrEmpty(Web))
            {
                Companylines[k] = Web;
                k++;
            }
            if (!string.IsNullOrEmpty(CompanyLicense))
            {
                Companylines[k] = CompanyLicense;
                k++;
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                Companylines[k] = Phone;
                k++;
            }
            rdr.Write(DotmatrixPrint.Reverse_Paper);
            rdr.Write(DotmatrixPrint.Reverse_Paper);
            rdr.Write(DotmatrixPrint.compressed_On);
            for (int j = 0; j < (Companylines.Length); j++)
            {
                if (!string.IsNullOrEmpty(Companylines[j])) rdr.WriteLine((j == 0 ? DotmatrixPrint.Bold_On : "") + DotmatrixDataAlignment.GetFormatedText(!string.IsNullOrEmpty(Companylines[j]) ? Companylines[j] : "", (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(j==0? Headings[0]:j==1? Headings[1]:"", (Line_Char / 2), AlignmentTypes.Prefix) + (j == 0 ? DotmatrixPrint.Bold_Off : "")); Totallines++;
            }
            rdr.WriteLine(DotmatrixDataAlignment.GetFormatedText("", Line_Char, AlignmentTypes.Prefix));
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            if (fileName == "SalesReportBySchedule")
            {
                MainTableForSalesreportByseries(dataTable, fileName);
            }            
            DotmatrixDataAlignment.SkipLine(rdr, 3);
            Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }
        private void MainTableForSalesreportByseries(DataTable DataTable, String TypeOfReport)
        {
            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            int[] columnSize = new int[] { 3,10 , 30, 20, 30, 10, 15, 20,20 };
            var lineString = "";
            int count = 0;
            foreach (DataColumn column in DataTable.Columns)
            {
                lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Prefix);
                count++;
            }
            rdr.WriteLine(DotmatrixPrint.Bold_On + lineString + DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(rdr, Line_Char);
            TotalLines += 2;
            for (int i = 0; i < Rows; i++)
            {
                lineString = "";
                for (int j = 0; j < Cols; j++)
                {
                    if (j == 1 || j == 2)
                    {
                        lineString += DotmatrixDataAlignment.GetFormatedText(j==2? DataTable.Rows[i][j].ToString().Replace("\n","")+"  " : DataTable.Rows[i][j].ToString() + "  ", columnSize[j]-1, AlignmentTypes.Prefix);
                    }
                    else
                    {
                        lineString += DotmatrixDataAlignment.GetFormatedText(DataTable.Rows[i][j].ToString()+"  ", columnSize[j]-1, AlignmentTypes.Prefix);
                    }
                }
                rdr.WriteLine(lineString);
                TotalLines++;
            }
        }
    }
}
