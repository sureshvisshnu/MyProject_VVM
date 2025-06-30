using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using Fa.model.Accounting.Masters;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static fa.views.utils.Common.BlankTablesWithBorder;

namespace fa.views.utils.Common
{
    class DotmatrixDataAlignment
    {
        public static void DotmatrixHeaderTable(StreamWriter Writer, int Line_Char, string heading, long AccountId)
        {
            int[] columnSize = { 90, 15, 30 };

            var CompanyName = Global.Company.DisplayAs + "\n";
            var Address = Global.Company.Address.FullAddressInSingleLine;
            var lheading = heading;
            fa.model.Accounting.Masters.Account lAccount = AccountManager.Instance.GetAccountById(AccountId);
            var Account = AccountId != 0 ? "A/C: " : "";
            var AccountGroup = AccountId != 0 ? "Group: " : "";

            Writer.Write(DotmatrixPrint.compressed_On);
            SkipLine(Writer, 1);
            Writer.WriteLine(DotmatrixPrint.Bold_On + GetFormatedText(CompanyName, columnSize[0], AlignmentTypes.Suffix) + DotmatrixPrint.Bold_Off + GetFormatedText(Account, columnSize[1], AlignmentTypes.Prefix) + GetFormatedText((AccountId != 0 ? lAccount.Name : ""), columnSize[2], AlignmentTypes.Suffix));
            Writer.WriteLine(GetFormatedText(Address, columnSize[0], AlignmentTypes.Suffix) + GetFormatedText(AccountGroup, columnSize[1], AlignmentTypes.Prefix) + GetFormatedText((AccountId != 0 ? lAccount.AccountGroup.Name : ""), columnSize[2], AlignmentTypes.Suffix));
            Writer.WriteLine(GetFormatedText(lheading, (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText("", (Line_Char / 2), AlignmentTypes.Prefix));
            PrintLine(Writer, Line_Char);
        }
        public static void DotmatrixMainTable(StreamWriter Writer, int Line_Char, DataTable dataTable)
        {
            int[] columnSize = { 20, 73, 21, 21 };
            var lineString = "";
            int count = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (count == 4)
                {
                    continue;
                }
                if (count <= 1)
                {
                    lineString += GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Suffix);
                }
                else
                {
                    lineString += GetFormatedText(column.Caption, columnSize[count], AlignmentTypes.Prefix);
                }
                count++;
            }
            Writer.WriteLine(DotmatrixPrint.Bold_On + lineString + DotmatrixPrint.Bold_Off);
            PrintLine(Writer, Line_Char);
        }
        public static void PrintPageNumber(StreamWriter Writer, int Line_Char, string Number, string TotalNumber)
        {
            Writer.WriteLine(GetFormatedText("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString(), (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(Number + "/" + TotalNumber, (Line_Char / 2), AlignmentTypes.Prefix));
        }
        public static void PrintLine(StreamWriter Writer, int Line_Char)
        {
            int i;
            string Lstr = "";
            for (i = 1; i <= Line_Char; i++)
            {
                Lstr = Lstr + "-";
            }
            Writer.WriteLine(Lstr);
        }
        public static void SkipLine(StreamWriter Writer, int LineNos)
        {
            int i;
            for (i = 1; i <= LineNos; i++)
            {
                Writer.WriteLine("");
            }
        }
        public static string GetCenterText(string Str, int Line_Char)
        {
            string blankSpace = "";
            int rLoc = Line_Char - Str.Length;

            if (rLoc < 0)
            {
                Str = Str.Substring(0, Line_Char);
            }
            else if (Str.Count() < Line_Char)
            {

                int ct = Str.Count();// = ;
                int loopCount = (Line_Char - ct) / 2;
                for (var i = 0; i < loopCount; i++)
                {
                    blankSpace += " ";
                }
            }
            return (blankSpace + Str);
        }
        public static string GetFormatedText(string Cont, int Length, AlignmentTypes AlignmentType)
        {
            Cont = Cont.Trim();
            int rLoc = Length - Cont.Length;

            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                for (nos = 0; nos < rLoc; nos++)
                {
                    if (AlignmentType == AlignmentTypes.Prefix)
                    {
                        Cont = " " + Cont;
                    }
                    else
                    {
                        Cont = Cont + " ";
                    }
                }
            }
            return (Cont);
        }

        //for sale
        public static int SaleDotmatrixHeaderTable(StreamWriter rdr, int Line_Char, SaleEntry SaleEntry)
        {
            int Totallines = 6;
            string CompanyName = Global.Company.DisplayAs;
            string CompanySlogan = Global.Company.Slogan;
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
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                blankSpace = "";
                if (CompanySlogan.Count() < Line_Char)
                {
                    int ct = CompanySlogan.Count();
                    for (var i = 0; i < (ct - Line_Char) / 2; i++)
                    {
                        blankSpace += " ";
                    }
                }
                CompanySlogan = blankSpace + CompanySlogan;
            }
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string contactNo = Global.Company.ContactInfo!=null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Phone) ? "Contact : " + Global.Company.ContactInfo.Phone.ToString():"";
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;
            Address = Global.Company.Address!=null? Global.Company.Address.FullAddressInSingleLine:"";
            if (Global.Company.ContactInfo != null && Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "Phone: " + Global.Company.ContactInfo.Phone + fax + "\n";
            }
            if (Global.Company.ContactInfo!=null && !string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
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
            Heading = SaleEntry.EntryType == Entrytype.SALE ? "TAX INVOICE" : SaleEntry.EntryType == Entrytype.RETURN ? "RETURN INVOICE" :"QUOTATION";
            //Customer
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                if (Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster != null && Licence.CompanyCustomerLicenseMaster.IncludeInInvoice)
                        {
                            CustomerLicense = (string.IsNullOrEmpty(CustomerLicense) ? CustomerLicense : CustomerLicense + "\n") + (Licence.CompanyCustomerLicenseMaster.DisplayName + ": " + Licence.Value);
                        }
                    }
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)SaleEntry.AccountsId);
                    if (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0)
                    {
                        foreach (SupplierLicenceDetail Licence in Supplier.SupplierLicenceDetail)
                        {
                            if (Licence.CompanySupplierLicenseMaster != null && Licence.CompanySupplierLicenseMaster.IncludeInReport)
                            {
                                CustomerLicense = (string.IsNullOrEmpty(CustomerLicense) ? CustomerLicense : CustomerLicense + "\n") + (Licence.CompanySupplierLicenseMaster.DisplayName + ": " + Licence.Value);
                            }
                        }
                    }
                }
            }
            if (SaleEntry.AccountsId != null)
            {
                CustomerDetail = SaleEntry.Account.DisplayAs + (String.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : "\n" + SaleEntry.CustomerAddress.Replace("\r", "").Replace("\n", "").Replace(",", "\n")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);
            }
            else
            {
                CustomerDetail = SaleEntry.CustomerName + (String.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : "\n" + SaleEntry.CustomerAddress.Replace("\r", "").Replace("\n", "").Replace(",", "\n")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);
            }
            string[] temp = CustomerDetail.Split(new[] { "\n" }, StringSplitOptions.None);
            string[] lines = new string[10];
            for (int i = 0; i < 10; i++)
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
            string[] Companylines = new string[10];
            int k = 0;
            if (!string.IsNullOrEmpty(CompanyName))
            {
                Companylines[k] = CompanyName;
                k++;
            }
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                Companylines[k] = CompanySlogan;
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
            for (int j = 0; j < (lines.Length > Companylines.Length ? lines.Length : Companylines.Length); j++)
            {
                if (!string.IsNullOrEmpty(Companylines[j]) || !string.IsNullOrEmpty(lines[j])) rdr.WriteLine((j==0?DotmatrixPrint.Bold_On:"") + GetFormatedText(!string.IsNullOrEmpty(Companylines[j]) ? Companylines[j] : "", (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(!string.IsNullOrEmpty(lines[j]) ? lines[j] : "", (Line_Char / 2), AlignmentTypes.Prefix) + (j == 0 ? DotmatrixPrint.Bold_Off:"")); Totallines++;
            }
            rdr.WriteLine(GetFormatedText("", Line_Char, AlignmentTypes.Prefix));
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(Heading, Line_Char) + DotmatrixPrint.Bold_Off);
            PrintLine(rdr, Line_Char);
            return Totallines;
        }
        public static int SaleDotmatrixMainTable(StreamWriter rdr, int Line_Char, SaleEntry SaleEntry)
        {
            int Totallines = 3;
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.RETURN ? "Return Number" : SaleEntry.EntryType == Entrytype.SALE ? "Invoice Number" : "Quote Number");
            invoiceContent.Add("Method");
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.RETURN ? "Return Date" : SaleEntry.EntryType == Entrytype.SALE ? "Invoice Date" : "Quote Date");
            invoiceContent.Add("Payment Terms");
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.SALE ? "Due Date" : "Exp Date");
            //Table Content
            invoiceContent.Add(SaleEntry.RefNumber);
            invoiceContent.Add(SaleEntry.SaleMethod == SaleMethod.Credit ? "CREDIT" : "CASH");
            invoiceContent.Add(SaleEntry.SaleDate.ToString(Global.Company.DateFormat));
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                if (Customer != null)
                {
                    invoiceContent.Add(Customer.PaymentTerm.ToString());
                    invoiceContent.Add(Customer != null ? (SaleEntry.EntryType == Entrytype.SALE ? SaleEntry.SaleDate : SaleEntry.QuotaionExpireAt).AddDays(Customer.PaymentTerm.NoOfDays).ToString(Global.Company.DateFormat).ToString() : DateTime.Now.Date.ToString(Global.Company.DateFormat));
                }                    
            }
            else
            {
                invoiceContent.Add("Cash Payment");
                invoiceContent.Add(DateTime.Today.ToString(Global.Company.DateFormat));
            }
            //Invoice Bill Details
            rdr.WriteLine(GetFormatedText(DotmatrixPrint.Bold_On + invoiceContent[0].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[1].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[2].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[3].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[4].Trim(), 27, AlignmentTypes.Suffix) +
                        DotmatrixPrint.Bold_Off
                        );
            rdr.WriteLine(GetFormatedText(invoiceContent[5].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[6].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[7].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(DateTime.Today.Date.ToString(Global.Company.DateFormat), 27, AlignmentTypes.Suffix)
                        );
            PrintLine(rdr, Line_Char);
            return Totallines;
        }

        public static int PurchaseDotmatrixHeaderTable(StreamWriter rdr, int Line_Char, PurchaseEntry PurchaseEntry)
        {
            int Totallines = 6;
            string CompanyName = Global.Company.DisplayAs;
            string CompanySlogan = Global.Company.Slogan;
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
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                blankSpace = "";
                if (CompanySlogan.Count() < Line_Char)
                {
                    int ct = CompanySlogan.Count();
                    for (var i = 0; i < (ct - Line_Char) / 2; i++)
                    {
                        blankSpace += " ";
                    }
                }
                CompanySlogan = blankSpace + CompanySlogan;
            }
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
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "TAX INVOICE" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "RETURN INVOICE" : "QUOTATION";
            //Customer
            if (PurchaseEntry.AccountId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)PurchaseEntry.AccountId);
                if (Customer != null && Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster != null && Licence.CompanyCustomerLicenseMaster.IncludeInInvoice)
                        {
                            CustomerLicense = (string.IsNullOrEmpty(CustomerLicense) ? CustomerLicense : CustomerLicense + "\n") + (Licence.CompanyCustomerLicenseMaster.DisplayName + ": " + Licence.Value);
                        }
                    }
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)PurchaseEntry.AccountId);
                    if (Supplier != null && Supplier.SupplierLicenceDetail.Count > 0)
                    {
                        foreach (SupplierLicenceDetail Licence in Supplier.SupplierLicenceDetail)
                        {
                            if (Licence.CompanySupplierLicenseMaster != null && Licence.CompanySupplierLicenseMaster.IncludeInReport)
                            {
                                CustomerLicense = (string.IsNullOrEmpty(CustomerLicense) ? CustomerLicense : CustomerLicense + "\n") + (Licence.CompanySupplierLicenseMaster.DisplayName + ": " + Licence.Value);
                            }
                        }
                    }
                }
            }
            CustomerDetail = PurchaseEntry.SupplierName + (String.IsNullOrEmpty(PurchaseEntry.SupplierAddress) ? "" : "\n" + PurchaseEntry.SupplierAddress.Replace("\r", "").Replace("\n", "").Replace(",", "\n")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);
            string[] temp = CustomerDetail.Split(new[] { "\n" }, StringSplitOptions.None);
            string[] lines = new string[10];
            for (int i = 0; i < 10; i++)
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
            string[] Companylines = new string[10];
            int k = 0;
            if (!string.IsNullOrEmpty(CompanyName))
            {
                Companylines[k] = CompanyName;
                k++;
            }
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                Companylines[k] = CompanySlogan;
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
            for (int j = 0; j < (lines.Length > Companylines.Length ? lines.Length : Companylines.Length); j++)
            {
                if (!string.IsNullOrEmpty(Companylines[j]) || !string.IsNullOrEmpty(lines[j])) rdr.WriteLine((j == 0 ? DotmatrixPrint.Bold_On : "") + GetFormatedText(!string.IsNullOrEmpty(Companylines[j]) ? Companylines[j] : "", (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(!string.IsNullOrEmpty(lines[j]) ? lines[j] : "", (Line_Char / 2), AlignmentTypes.Prefix) + (j == 0 ? DotmatrixPrint.Bold_Off : "")); Totallines++;
            }
            rdr.WriteLine(GetFormatedText("", Line_Char, AlignmentTypes.Prefix));
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(Heading, Line_Char) + DotmatrixPrint.Bold_Off);
            PrintLine(rdr, Line_Char);
            return Totallines;
        }

        public static int PurchaseDotmatrixMainTable(StreamWriter rdr, int Line_Char, PurchaseEntry PurchaseEntry)
        {
            int Totallines = 3;
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "Return Number" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Invoice Number" : "Quote Number");
            invoiceContent.Add("Method");
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "Return Date" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Invoice Date" : "Quote Date");
            invoiceContent.Add("Payment Terms");
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Due Date" : "Exp Date");
            //Table Content
            invoiceContent.Add(PurchaseEntry.RefNumber);
            invoiceContent.Add(PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit ? "CREDIT" : "CASH");
            invoiceContent.Add(PurchaseEntry.PurchaseInvDate.ToString(Global.Company.DateFormat));
            if (PurchaseEntry.AccountId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)PurchaseEntry.AccountId);
                if (Customer != null)
                {
                    invoiceContent.Add(Customer.PaymentTerm.ToString());
                   // invoiceContent.Add(Customer != null ? (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? PurchaseEntry.PurchaseInvDate : PurchaseEntry.QuotaionExpireAt).AddDays(Customer.PaymentTerm.NoOfDays).ToString(Global.Company.DateFormat).ToString() : DateTime.Now.Date.ToString(Global.Company.DateFormat));
                }
            }
            else
            {
                invoiceContent.Add("Cash Payment");
                invoiceContent.Add(DateTime.Today.ToString(Global.Company.DateFormat));
            }
            //Invoice Bill Details
            rdr.WriteLine(GetFormatedText(DotmatrixPrint.Bold_On + invoiceContent[0].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[1].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[2].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[3].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[4].Trim(), 27, AlignmentTypes.Suffix) +
                        DotmatrixPrint.Bold_Off
                        );
            rdr.WriteLine(GetFormatedText(invoiceContent[5].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[6].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[7].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(DateTime.Today.Date.ToString(Global.Company.DateFormat), 27, AlignmentTypes.Suffix)
                        );
            PrintLine(rdr, Line_Char);
            return Totallines;
        }


        // sale pre printed bill
        public static int SaleDotmatrixHeaderTableForPreprint(StreamWriter rdr, int Line_Char, SaleEntry SaleEntry,bool iscontinuePrint)
        {
            int Totallines = 0;          
            string CustomerLicense = string.Empty;
            string CustomerDetail = string.Empty;
            
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                CustomerDetail = SaleEntry.CustomerName + "\n"  + (string.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : SaleEntry.CustomerAddress+ "\n") + (Customer.ContactInfo!=null &&!string.IsNullOrEmpty(Customer.ContactInfo.Phone) ? Customer.ContactInfo.Phone + "\n" : "") + "Customer ID: " + SaleEntry.AccountsId + "\n\n";
                if (Customer!=null && Customer.CustomerLicenceDetail!=null && Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster.IncludeInReport)
                        {
                            CustomerLicense = (string.IsNullOrEmpty(CustomerLicense) ? CustomerLicense : CustomerLicense + "\n") + (Licence.CompanyCustomerLicenseMaster.DisplayName + ": " + Licence.Value);
                        }
                    }
                }
            }
            else
            {
                CustomerDetail = SaleEntry.CustomerName + "\n" + SaleEntry.CustomerAddress;
            }

            string[] temp = CustomerDetail.Split(new[] { "\n" }, StringSplitOptions.None);
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
            if (!iscontinuePrint)
            {
                rdr.Write(DotmatrixPrint.Reverse_Paper);
                rdr.Write(DotmatrixPrint.Reverse_Paper);
                rdr.Write(DotmatrixPrint.Reverse_Paper);
                rdr.Write(DotmatrixPrint.compressed_On);
                rdr.WriteLine();
                rdr.WriteLine();
            }
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetFormatedText("", (Line_Char-50), AlignmentTypes.Suffix) + GetFormatedText(lines[0], (25), AlignmentTypes.Suffix) + DotmatrixPrint.Bold_Off);
            rdr.WriteLine(GetFormatedText(" ", (Line_Char - 50), AlignmentTypes.Suffix) + GetFormatedText((lines[1]), (30), AlignmentTypes.Suffix)); 
            rdr.WriteLine(GetFormatedText(" ", (Line_Char - 50), AlignmentTypes.Suffix) + GetFormatedText((lines[2]), (30), AlignmentTypes.Suffix));
            rdr.WriteLine(GetFormatedText(" ", (Line_Char - 50), AlignmentTypes.Suffix) + GetFormatedText((lines[3]), (30), AlignmentTypes.Suffix));
            rdr.WriteLine(GetFormatedText(" ", (Line_Char - 50), AlignmentTypes.Suffix) + GetFormatedText((lines[4]), (30), AlignmentTypes.Suffix));
            rdr.WriteLine(GetFormatedText(" ", (Line_Char - 50), AlignmentTypes.Suffix) + GetFormatedText((lines[5]), (30), AlignmentTypes.Suffix));
            //rdr.WriteLine();
            rdr.WriteLine(DotmatrixPrint.Enlarged+GetFormatedText(" ", (24), AlignmentTypes.Suffix)+ GetFormatedText(SaleEntry.RefNumber, (19), AlignmentTypes.Suffix) + 
                GetFormatedText(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), (12), AlignmentTypes.Suffix) +
                GetFormatedText(DateTime.Now.ToShortTimeString(), (8), AlignmentTypes.Suffix)+ DotmatrixPrint.Enlargedoff);
            return Totallines;
        }

        public static int PatientInvoiceDotmatrixHeaderTable(StreamWriter rdr, int Line_Char)
        {
            int Totallines = 6;
            string CompanyName = Global.Company.DisplayAs;
            string CompanySlogan = Global.Company.Slogan;
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
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                blankSpace = "";
                if (CompanySlogan.Count() < Line_Char)
                {
                    int ct = CompanySlogan.Count();
                    for (var i = 0; i < (ct - Line_Char) / 2; i++)
                    {
                        blankSpace += " ";
                    }
                }
                CompanySlogan = blankSpace + CompanySlogan;
            }
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
            Heading = "PATIENT INVOICE";

            string[] lines = new string[10];
            
            string[] Companylines = new string[10];
            int k = 0;
            if (!string.IsNullOrEmpty(CompanyName))
            {
                Companylines[k] = CompanyName;
                k++;
            }
            if (!string.IsNullOrEmpty(CompanySlogan))
            {
                Companylines[k] = CompanySlogan;
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
            for (int j = 0; j < (lines.Length > Companylines.Length ? lines.Length : Companylines.Length); j++)
            {
                if (!string.IsNullOrEmpty(Companylines[j]) || !string.IsNullOrEmpty(lines[j])) rdr.WriteLine((j == 0 ? DotmatrixPrint.Bold_On : "") + GetFormatedText(!string.IsNullOrEmpty(Companylines[j]) ? Companylines[j] : "", (Line_Char / 2), AlignmentTypes.Suffix) + GetFormatedText(!string.IsNullOrEmpty(lines[j]) ? lines[j] : "", (Line_Char / 2), AlignmentTypes.Prefix) + (j == 0 ? DotmatrixPrint.Bold_Off : "")); Totallines++;
            }
            rdr.WriteLine(GetFormatedText("", Line_Char, AlignmentTypes.Prefix));
            rdr.WriteLine(DotmatrixPrint.Bold_On + GetCenterText(Heading, Line_Char) + DotmatrixPrint.Bold_Off);
            PrintLine(rdr, Line_Char);
            return Totallines;
        }

        public static int PatientInvoiceDotmatrixMainTable(StreamWriter rdr, int Line_Char, PatientInvoice Invoice, Patient patient, PatientLedger ledger, InPatientAdmission InPatientAdmission, Registration Registration, InPatientLocation InPatientLocation, Registration Consultant)
        {
            int Totallines = 3;
            List<string> invoiceContent = new List<string>();

            invoiceContent.Add("Patient Name   : ");
            invoiceContent.Add(patient.Name);
            invoiceContent.Add("Bill Number       : ");
            invoiceContent.Add(Invoice.ReferenceNumber);
            invoiceContent.Add("Age/Sex        : ");
            invoiceContent.Add(patient.Age.ToString()! + "/" + patient.Gender.ToString());
            invoiceContent.Add("Bill Date         : ");
            invoiceContent.Add(Invoice.InvoiceDate.Date.ToString(Global.Company.DateFormat));
            invoiceContent.Add("Patient Id     : ");
            invoiceContent.Add(patient.PatientNumber);
            if (ledger.InPatientAdmissionId != null)
            {
                invoiceContent.Add("Admission Date    : ");
                invoiceContent.Add(InPatientAdmission.DateOfAdmission.Date.ToString(Global.Company.DateFormat));
            }
            else
            {
                invoiceContent.Add("Registration Date : ");
                invoiceContent.Add(Registration.DateOfRegistration.Date.ToString(Global.Company.DateFormat));
            }
            invoiceContent.Add("OutPatient Id  : ");
            invoiceContent.Add(ledger.InPatientAdmissionId != null ? (InPatientAdmission.Status == InPatientStatus.DISCHARGED ? "" : (!string.IsNullOrEmpty(Registration.PatientOPNumber) ? Registration.PatientOPNumber : "")) : (!string.IsNullOrEmpty(Registration.PatientOPNumber) ? Registration.PatientOPNumber : ""));

            if (Registration != null)
            {
                if (Registration.Status == Status.INPATIENT)
                {
                    if (ledger.InPatientAdmissionId != null)
                    {
                        invoiceContent.Add("InPatient Id : ");
                        invoiceContent.Add(InPatientAdmission.PatientIPNumber != null ? InPatientAdmission.PatientIPNumber : "");

                        if (InPatientLocation != null)
                        {
                            invoiceContent.Add("Ward/Bed :");
                            invoiceContent.Add(InPatientLocation.Ward.Name + "/" + InPatientLocation.Bed.Name);
                        }
                        invoiceContent.Add("Primary Doctor    :");
                        invoiceContent.Add(InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name);

                        invoiceContent.Add("Address        : ");
                        invoiceContent.Add(patient.Address.FullAddressInSingleLine.Trim());

                        invoiceContent.Add("Discharge Date :");
                        invoiceContent.Add(InPatientAdmission.Status == InPatientStatus.DISCHARGED ? InPatientLocation.DateMovedOut.ToString(Global.Company.DateFormat) : "");
                    }
                    else
                    {
                        invoiceContent.Add("Primary Doctor    :");
                        invoiceContent.Add(Consultant.RequestedDoctor != null ? Consultant.RequestedDoctor.Name : "");

                        invoiceContent.Add("Address        : ");
                        invoiceContent.Add(patient.Address.FullAddressInSingleLine.Trim());
                    }
                }
                else if (InPatientAdmission != null)
                {
                    if (InPatientAdmission.Status == InPatientStatus.DISCHARGED)
                    {
                        if (ledger.InPatientAdmissionId == null)
                        {
                            invoiceContent.Add("Primary Doctor    :");
                            invoiceContent.Add(InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name);

                            invoiceContent.Add("Address        :");
                            invoiceContent.Add(patient.Address.FullAddressInSingleLine.Trim());

                            invoiceContent.Add("Discharge Date :");
                            invoiceContent.Add("");
                        }
                        else
                        {
                            invoiceContent.Add("Primary Doctor    :");
                            invoiceContent.Add(InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name);

                            invoiceContent.Add("Address        :");
                            invoiceContent.Add(patient.Address.FullAddressInSingleLine.Trim());

                            invoiceContent.Add("Discharge Date    :");
                            invoiceContent.Add(InPatientAdmission.Status == InPatientStatus.DISCHARGED ? InPatientLocation.DateMovedOut.ToString(Global.Company.DateFormat) : "");
                        }
                    }
                }
                else
                {
                    invoiceContent.Add("Primary Doctor    :");
                    invoiceContent.Add(Consultant.RequestedDoctor != null ? Consultant.RequestedDoctor.Name : "");

                    invoiceContent.Add("Address        :");
                    invoiceContent.Add(patient.Address.FullAddressInSingleLine.Trim());
                }
            }

            rdr.WriteLine(GetFormatedText(invoiceContent[0].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[1].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[2].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[3].Trim(), 27, AlignmentTypes.Suffix));

            rdr.WriteLine(GetFormatedText(invoiceContent[4].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[5].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[6].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[7].Trim(), 27, AlignmentTypes.Suffix));

            rdr.WriteLine(GetFormatedText(invoiceContent[8].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[9].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[10].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[11].Trim(), 27, AlignmentTypes.Suffix));

            rdr.WriteLine(GetFormatedText(invoiceContent[12].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[13].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[14].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[15].Trim(), 27, AlignmentTypes.Suffix));

            if (invoiceContent.Count == 18)
            {
                rdr.WriteLine(GetFormatedText(invoiceContent[16].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[17].Trim(), 27, AlignmentTypes.Suffix));
                PrintLine(rdr, Line_Char);
            }
            else
            {
                rdr.WriteLine(GetFormatedText(invoiceContent[16].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[17].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[18].Trim(), 27, AlignmentTypes.Suffix) +
                        GetFormatedText(invoiceContent[19].Trim(), 27, AlignmentTypes.Suffix));
                PrintLine(rdr, Line_Char);
            }

            return Totallines;
        }
    }
}
