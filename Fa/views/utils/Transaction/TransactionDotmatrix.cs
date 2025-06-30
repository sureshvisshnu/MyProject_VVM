using fa.model.Common;
using fa.views.utils.Common;
using fa.views.utils.Receipts;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace fa.views.utils.Transaction
{
    public class TransactionDotmatrix
    {
        DotmatrixPrint DotmatrixPrint = new DotmatrixPrint();
        
        System.IO.StreamWriter Reader;
        public string tran_type;
        public string Discount;
        public string bill_amt;
        public string NetAmount;
        public string reciept_amount;
        public decimal MRPTotal = 0, SavedTotal = 0;
        public int count;
        private int Line_Char = 135;
        public string fileName;
        int TotalLines = 0;

        public TransactionDotmatrix(object sender)
        {
            if(sender is PrintBase)
            {
                fileName = (sender as PrintBase).fileName;
            }       
            var windowsTempPath = System.IO.Path.GetTempPath();
            Directory.CreateDirectory(windowsTempPath + "");
            Reader = new System.IO.StreamWriter(windowsTempPath + fileName + ".txt");
        }
        
        public  void GenerateTXT(DataTable dataTable, DataTable totalTable, List<string> heading, List<string> invoiceContent, string fileExtension, bool isPrint)
        {
            PrintHeader(heading);
            PrintDetails(dataTable, invoiceContent, totalTable);
            DotmatrixDataAlignment.SkipLine(Reader, 3);
            Reader.Close();
            DotmatrixPrint.DoPrint(fileName, isPrint);
        }
        public void PrintHeader(List<string> heading)
        {
            string CompanyName = Global.Company.DisplayAs;
            string blankSpace = "";
            if (CompanyName.Count() < Line_Char)
            {
                int ct = CompanyName.Count();// = ;
                for (var i = 0; i < (ct - Line_Char) / 2; i++)
                {
                    blankSpace += " ";
                }
            }
            CompanyName = blankSpace + Global.Company.DisplayAs;

            string Address = Global.Company.Address.FullAddressInSingleLine; 
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string contactNo = "Contact : " + Global.Company.ContactInfo.Phone.ToString();
            string CompanyLicense = string.Empty;

            
            if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "Phone: " + Global.Company.ContactInfo.Phone + fax + "\n";
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
            {
                Email = Global.Company.ContactInfo.Email == "" ? "" : "Email: " + (Global.Company.ContactInfo).Email + "\n";
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
            {
                Web = Global.Company.ContactInfo.WebSite == "" ? "" : "Web: " + (Global.Company.ContactInfo).WebSite + "\n\n";
            }
            //Company license Info
            if (Global.Company.CompanyLicence.Count > 0)
            {
                foreach (CompanyLicence Licence in Global.Company.CompanyLicence)
                {
                    if (Licence.IncludeInInvoice)
                    {
                        CompanyLicense = (string.IsNullOrEmpty(CompanyLicense) ? CompanyLicense : CompanyLicense + ", ") + (Licence.DisplayName + ": " + Licence.Value);
                    }
                }
                CompanyLicense = "\n" + CompanyLicense + "\n";
            }
            string Heading = heading.Count>1? heading[1] :"";
            string[] temp = Heading.Split(new[] { "\n" }, StringSplitOptions.None);
            string[] lines = new string[10];
            for (int i = 0; i < 7; i++)
            {
                try
                {
                    lines[i] = temp[i];
                }
                catch
                {
                    lines[i] = "";
                }
            }
            Reader.Write(DotmatrixPrint.Reverse_Paper);
            Reader.Write(DotmatrixPrint.Reverse_Paper);
            Reader.Write(DotmatrixPrint.compressed_On);
            Reader.WriteLine();
            Reader.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(CompanyName, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[0], (Line_Char / 2), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(Address, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[1], (Line_Char / 2), AlignmentTypes.Prefix));

            if (!string.IsNullOrEmpty(Phone) || !string.IsNullOrEmpty(lines[2])) { Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(Phone, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[2], (Line_Char / 2), AlignmentTypes.Prefix)); TotalLines+=1; }
            if (!string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(lines[3])) { Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(Email, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[3], (Line_Char / 2), AlignmentTypes.Prefix)); TotalLines += 1; }
            if (!string.IsNullOrEmpty(Web) || !string.IsNullOrEmpty(lines[4])) { Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(Web, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[4], (Line_Char / 2), AlignmentTypes.Prefix)); TotalLines += 1; }
            if (!string.IsNullOrEmpty(CompanyLicense) || !string.IsNullOrEmpty(lines[5])) { Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(CompanyLicense, (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText(lines[5], (Line_Char / 2), AlignmentTypes.Prefix)); TotalLines += 1; }


            Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(lines[6], Line_Char, AlignmentTypes.Prefix));
            Reader.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetCenterText(heading[0], Line_Char) + DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(Reader, Line_Char);
            TotalLines += 5;
        }
        public void PrintDetails(DataTable dataTable, List<string> invoiceContent, DataTable Total)
        {
            if (fileName == "DebitNote" || fileName == "CreditNote" || fileName == "Journal")
            {
                Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(DotmatrixPrint.Bold_On + invoiceContent[0].Trim(), 72, AlignmentTypes.Suffix) +
                           DotmatrixDataAlignment.GetFormatedText(invoiceContent[1].Trim(), 60, AlignmentTypes.Prefix) +
                           DotmatrixPrint.Bold_Off
                           );
                Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(invoiceContent[2].Trim(), 72, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[3].Trim(), 60, AlignmentTypes.Prefix)
                            );
            }
            else
            {
                Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(DotmatrixPrint.Bold_On + invoiceContent[0].Trim(), 31, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[1].Trim(), 32, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[2].Trim(), 32, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[3].Trim(), 32, AlignmentTypes.Suffix) +
                            DotmatrixPrint.Bold_Off
                            );
                Reader.WriteLine(DotmatrixDataAlignment.GetFormatedText(invoiceContent[4].Trim(), 31, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[5].Trim(), 32, AlignmentTypes.Suffix) /*+
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[6].Trim(), 32, AlignmentTypes.Suffix) +
                            DotmatrixDataAlignment.GetFormatedText(invoiceContent[7].Trim(), 32, AlignmentTypes.Suffix)*/
                            );
            }
            DotmatrixDataAlignment.PrintLine(Reader, Line_Char);

            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count;
            int[] columnSize =new int[] { 4, 115, 12 };
            if(fileName=="Invoice")
            {
                columnSize = new int[] { 4, 70,10,10,20, 12 };
            }
            else if (fileName == "Journal")
            {
                columnSize = new int[] { 4, 20, 50, 20, 15, 17 };
            }
            else if (fileName == "DebitNote" || fileName == "CreditNote")
            {
                columnSize = new int[] { 4, 30, 72, 20 };
            }
            //table column name
            var lineString = "";
            count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (count <= 1 ||(fileName == "Journal" && count <= 4)
                    || ((fileName == "DebitNote" || fileName == "CreditNote") && count == 2))
                {
                    lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Suffix);
                }              
                else
                {
                    lineString += DotmatrixDataAlignment.GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Prefix);
                }
                count++;
            }
            Reader.WriteLine(DotmatrixPrint.Bold_On + lineString + DotmatrixPrint.Bold_Off);
            DotmatrixDataAlignment.PrintLine(Reader, Line_Char);
            TotalLines += 5;

            //table rows
            for (int i = 0; i < rows; i++)
            {
                var productDetails = "";
                for (int j = 0; j < cols; j++)
                {
                    var temp = dataTable.Rows[i][j].ToString();
                    if (j <= 1 || (fileName == "Journal" && j <= 4)
                        || ((fileName == "DebitNote" || fileName == "CreditNote") && count == 2))
                    {
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[j], AlignmentTypes.Suffix);
                    }
                    else
                    {
                        productDetails += DotmatrixDataAlignment.GetFormatedText(temp, columnSize[j], AlignmentTypes.Prefix);
                    }
                }
                Reader.WriteLine(productDetails);
                TotalLines++;
            }
            //empty line
            if((TotalLines+ Total.Rows.Count)<30)
            {
                DotmatrixDataAlignment.SkipLine(Reader,27- (TotalLines + Total.Rows.Count));
            }
            DotmatrixDataAlignment.PrintLine(Reader, Line_Char);
            //total
            for (int i = 0; i < Total.Rows.Count; i++)
            {
                if (fileName == "Invoice")
                {
                    Reader.WriteLine(DotmatrixPrint.Bold_On +
                       DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][0].ToString(), 10, AlignmentTypes.Prefix) +
                       DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][1].ToString(), 88, AlignmentTypes.Prefix) +
                       DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][2].ToString(), 15, AlignmentTypes.Prefix) +
                       DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][3].ToString(), 12, AlignmentTypes.Prefix) +
                       DotmatrixPrint.Bold_Off);
                }
                else
                {
                    int[] TotalWidth = new int[] { 4, 115, 12 };
                    if (fileName == "Journal")
                    {
                        TotalWidth = new int[] { 82, 15, 28 };
                    }
                    Reader.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][0].ToString(), TotalWidth[0], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][1].ToString(), TotalWidth[1], AlignmentTypes.Prefix) +
                        DotmatrixDataAlignment.GetFormatedText(Total.Rows[i][2].ToString(), TotalWidth[2], AlignmentTypes.Prefix)+
                        DotmatrixPrint.Bold_Off);
                }

            }
            DotmatrixDataAlignment.PrintLine(Reader, Line_Char);
            DotmatrixDataAlignment.SkipLine(Reader, 2);
            //signature
            if (fileName== "Payment")
            {
                Reader.WriteLine(DotmatrixPrint.Bold_On  + DotmatrixDataAlignment.GetFormatedText("For " + Global.Company.Name + " Authorised", (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText("Receiver's Signature", (Line_Char / 2), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            }
            else if (fileName == "Bill")
            {
                Reader.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("For " + Global.Company.Name , (Line_Char), AlignmentTypes.Suffix) + DotmatrixPrint.Bold_Off);
            }
            else if(fileName == "Receipt")
            {               
                Reader.WriteLine(DotmatrixPrint.Bold_On + DotmatrixDataAlignment.GetFormatedText("Receiver's Signature", (Line_Char / 2), AlignmentTypes.Suffix) + DotmatrixDataAlignment.GetFormatedText("For " + Global.Company.Name + " Authorised", (Line_Char / 2), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            }
            else
            {
                Reader.WriteLine(DotmatrixPrint.Bold_On  + DotmatrixDataAlignment.GetFormatedText("For " + Global.Company.Name , (Line_Char), AlignmentTypes.Prefix) + DotmatrixPrint.Bold_Off);
            }
            DotmatrixDataAlignment.SkipLine(Reader, 4);
        }

    }
}
