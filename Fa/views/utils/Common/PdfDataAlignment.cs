using fa.api.Accounting;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using Fa.model.Accounting.Masters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.utils
{
    class PdfDataAlignment
    {
        public static iTextSharp.text.Font GetFont(string Name)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_White = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Bold_Italic_9_White = FontFactory.GetFont(FONT, 9, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Bold_Italic_8_White = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Bold_Italic_7_White = FontFactory.GetFont(FONT, 7, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Bold_Italic_6_White = FontFactory.GetFont(FONT, 6, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_7_Black = FontFactory.GetFont(FONT, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_5_Black = FontFactory.GetFont(FONT, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_4_Black = FontFactory.GetFont(FONT, 4, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_6_Black = FontFactory.GetFont(FONT, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_9_Black = FontFactory.GetFont(FONT, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_10_Black = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font Font_Normal_Italic_16_Black = FontFactory.GetFont(FONT, 16, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            iTextSharp.text.Font Font_Bold_Italic_12_Black = FontFactory.GetFont(FONT, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_14_Black = FontFactory.GetFont(FONT, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_16_Black = FontFactory.GetFont(FONT, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_20_Black = FontFactory.GetFont(FONT, 20, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_Black = FontFactory.GetFont(FONT, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_7_Black = FontFactory.GetFont(FONT, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_6_Black = FontFactory.GetFont(FONT, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_5_Black = FontFactory.GetFont(FONT, 5, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            iTextSharp.text.Font Font_Bold_Italic_32_LightGray = FontFactory.GetFont(FONT, 32, iTextSharp.text.Font.BOLD, BaseColor.LIGHT_GRAY);
            iTextSharp.text.Font Font_Bold_Italic_8_LightGray = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.LIGHT_GRAY);
            iTextSharp.text.Font Font_Bold_Italic_20_LightGray = FontFactory.GetFont(FONT, 20, iTextSharp.text.Font.BOLD, BaseColor.LIGHT_GRAY);
            iTextSharp.text.Font Font_Bold_Italic_15_LightGray = FontFactory.GetFont(FONT, 15, iTextSharp.text.Font.BOLD, BaseColor.LIGHT_GRAY);
            iTextSharp.text.Font Font_Bold_Italic_12_LightGray = FontFactory.GetFont(FONT, 11, iTextSharp.text.Font.BOLD, BaseColor.LIGHT_GRAY);

            return (Name == "Font_Normal_Italic_8_Black") ? Font_Normal_Italic_8_Black : (Name == "Font_Bold_Italic_9_White") ? Font_Bold_Italic_9_White :
                (Name == "Font_Bold_Italic_8_White") ? Font_Bold_Italic_8_White : (Name == "Font_Bold_Italic_7_White") ? Font_Bold_Italic_7_White :
                (Name == "Font_Normal_Italic_7_Black") ? Font_Normal_Italic_7_Black : (Name == "Font_Bold_Italic_5_Black") ? Font_Bold_Italic_5_Black :
                (Name == "Font_Normal_Italic_5_Black") ? Font_Normal_Italic_5_Black : (Name == "Font_Normal_Italic_10_Black") ? Font_Normal_Italic_10_Black :
                (Name == "Font_Bold_Italic_12_Black") ? Font_Bold_Italic_12_Black : (Name == "Font_Bold_Italic_14_Black") ? Font_Bold_Italic_14_Black :
                (Name == "Font_Bold_Italic_16_Black") ? Font_Bold_Italic_16_Black : (Name == "Font_Bold_Italic_20_Black") ? Font_Bold_Italic_20_Black :
                (Name == "Font_Bold_Italic_32_LightGray") ? Font_Bold_Italic_32_LightGray : (Name == "Font_Normal_Italic_9_Black") ? Font_Normal_Italic_9_Black :
                (Name == "Font_Normal_Italic_6_Black") ? Font_Normal_Italic_6_Black : (Name == "Font_Bold_Italic_20_LightGray") ? Font_Bold_Italic_20_LightGray :
                (Name == "Font_Bold_Italic_15_LightGray") ? Font_Bold_Italic_15_LightGray : (Name == "Font_Normal_Italic_16_Black") ? Font_Normal_Italic_16_Black :
                (Name == "Font_Bold_Italic_6_Black") ? Font_Bold_Italic_6_Black : (Name == "Font_Bold_Italic_10_Black") ? Font_Bold_Italic_10_Black :
                (Name == "Font_Bold_Italic_8_Black") ? Font_Bold_Italic_8_Black : (Name == "Font_Bold_Italic_7_Black") ? Font_Bold_Italic_7_Black :
                (Name == "Font_Normal_Italic_4_Black") ? Font_Normal_Italic_4_Black : (Name == "Font_Bold_Italic_6_White") ? Font_Bold_Italic_6_White :
                (Name == "Font_Bold_Italic_10_White") ? Font_Bold_Italic_10_White : (Name == "Font_Bold_Italic_9_Black") ? Font_Bold_Italic_9_Black :
                (Name == "Font_Bold_Italic_12_LightGray") ? Font_Bold_Italic_12_LightGray : (Name == "Font_Bold_Italic_8_LightGray") ? Font_Bold_Italic_8_LightGray : null!;
        }

        //For ledger and daybook
        public static PdfPTable LedgerDaybookHeaderTable(string heading, long AccountId)
        {
            var CompanyName = Global.Company.DisplayAs;
            var Address = AddressManager.Instance.GetAddress(Global.Company).FullAddressInSingleLine;

            int HeadColumns = 3;
            int MinimumHeight = 10;
            float[] HeadWidths = new float[] { 65f, 10f, 25f };
            if (Global.getLogoAsBytes() != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 50f, 10f, 25f };
            }
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();

            HeadTable.SetWidths(HeadWidths);
            if (Global.getLogoAsBytes() != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(200f, 20f);
                image.ScaleAbsolute(50, 50);
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 25;
                HeadCell.Padding = 4;
                HeadCell.Rowspan = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_TOP;
                HeadTable.AddCell(HeadCell);
            }
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            fa.model.Accounting.Masters.Account lAccount = AccountManager.Instance.GetAccountById(AccountId);

            var AccountHeadingData = AccountId != 0 ? "A/C: " : "";
            HeadCell = new PdfPCell(new Phrase(AccountHeadingData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            var AccountData = AccountId != 0 ? lAccount.Name : "";
            HeadCell = new PdfPCell(new Phrase(AccountData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);



            AddressData = (string.IsNullOrEmpty(Address) ? "" : Address + "\n") + heading + "\n ";
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = 40;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);


            AccountHeadingData = AccountId != 0 ? "Group: " : "";
            HeadCell = new PdfPCell(new Phrase(AccountHeadingData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            AccountData = AccountId != 0 ? lAccount.AccountGroup.Name : "";
            HeadCell = new PdfPCell(new Phrase(AccountData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }
        public static PdfPTable LedgerDaybookMainTable(DataTable DataTable, String TypeOfReport)
        {
            int Cols = DataTable.Columns.Count - 1;
            int Rows = DataTable.Rows.Count - 1;
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null!;
            if (TypeOfReport == "Ledger" || TypeOfReport == "Patient Ledger")
            {
                widths = new float[] { 25f, 60f, 25f, 30f, 25f };
            }
            if (TypeOfReport == "DayBook")
            {
                widths = new float[] { 25f, 60f, 25f, 30f };
            }
            if (TypeOfReport == "ItemLedger")
            {
                widths = new float[] { 25f, 25f, 25f, 40f, 25f, 15f, 25f, 25f, 25f, 15f };
            }
            ReportMainTable.SetWidths(widths);
            PdfPCell HeaderCell = new PdfPCell();

            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, GetFont("Font_Bold_Italic_9_White")));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                if (TypeOfReport == "DayBook")
                {
                    HeaderCell = new PdfPCell(new Phrase(column.Caption, GetFont("Font_Bold_Italic_9_White")));
                    HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                    HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                    if (column.Caption == "Date")
                    {
                        HeaderCell.BorderWidthLeft = 0.5f;
                    }
                }
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 25;
                HeaderCell.Padding = 4;
                if (column.Caption == "Fee Charged" || column.Caption == "Amount Received" || column.Caption == "Credit" || column.Caption == "Debit" || column.Caption == "Balance" || column.Caption == "Stock"
                    )
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }

                ReportMainTable.AddCell(HeaderCell);
            }
            return ReportMainTable;
        }

        //For sale and quote 
        public static PdfPTable SaleHeader(SaleEntry SaleEntry)
        {
            string CompanyName = Global.Company.DisplayAs + "\n";
            string Address = Global.Company.Address.FullAddressInSingleLine;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;

            //Company Contact Info
            if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "\nPhone: " + Global.Company.ContactInfo.Phone + fax;
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
            {
                Email = Global.Company.ContactInfo.Email == "" ? "" : "\nEmail: " + (Global.Company.ContactInfo).Email;
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
            {
                Web = Global.Company.ContactInfo.WebSite == "" ? "" : "\nWeb: " + (Global.Company.ContactInfo).WebSite;
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


            //Heading
            Heading = SaleEntry.EntryType == Entrytype.SALE ? "INVOICE" : "QUOTATION";

            //Customer Contact Info
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                CustomerDetail = SaleEntry.CustomerName + "\n" + Customer.BillingAddress.FullAddressInSingleLine + (string.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : "\n") + (!string.IsNullOrEmpty(Customer.ContactInfo.Phone) ? Customer.ContactInfo.Phone + "\n" : "") + "Customer ID: " + SaleEntry.AccountsId + "\n\n";
                if (Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster != null && Licence.CompanyCustomerLicenseMaster.IncludeInReport)
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

            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 50f, 30f, 25f };
            if (Global.getLogoAsBytes() != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 40f, 25f, 25f };
            }

            //Create Header
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            //For Company Logo
            if (Global.getLogoAsBytes() != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(200f, 20f);
                image.ScaleAbsolute(70, 70);
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 16;
                HeadCell.Padding = 4;
                HeadCell.Rowspan = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeadTable.AddCell(HeadCell);
            }
            //company details
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(AddressData, GetFont("Font_Bold_Italic_12_Black")));
            AddressData = Address + Phone + Email + Web + CompanyLicense;
            HeadCell.AddElement(new Phrase(AddressData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.Rowspan = 2;
            HeadTable.AddCell(HeadCell);

            //customer address
            HeadCell = new PdfPCell(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(CustomerDetail, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadCell.Rowspan = 2;
            HeadTable.AddCell(HeadCell);

            //header

            HeadCell = new PdfPCell(new Phrase(Heading, GetFont("Font_Bold_Italic_20_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = HeadColumns;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            //license
            HeadCell = new PdfPCell(new Phrase(CustomerLicense, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }
        //sale mini head
        public static PdfPTable SaleMiniHeader(SaleEntry SaleEntry)
        {
            string CompanyName = Global.Company.DisplayAs + "\n";
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;
            //Heading text
            Heading = SaleEntry.EntryType == Entrytype.SALE ? "INVOICE" : "QUOTATION";
            //Customer Contact Info
            if (SaleEntry.AccountsId != null)
            {
                CustomerDetail = "Customer ID: " + SaleEntry.AccountsId;
            }
            else
            {
                CustomerDetail = SaleEntry.CustomerName;
            }
            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 50f, 30f, 25f };
            //Create Header
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            //company name           
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Bold_Italic_12_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            //customer detail
            HeadCell = new PdfPCell(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(CustomerDetail, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            //Heading

            HeadCell = new PdfPCell(new Phrase(Heading, GetFont("Font_Bold_Italic_20_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        public static PdfPTable SaleMainTable(SaleEntry SaleEntry)
        {
            string PaymentTerm = string.Empty;
            string PaymentDate = string.Empty;
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                PaymentTerm = SaleEntry.Account != null ? Customer != null ? Customer.PaymentTerm.ToString() : "Immediate Payment" : "";
                PaymentDate = SaleEntry.EntryType == Entrytype.QUOTE ? SaleEntry.QuotaionExpireAt.ToString(Global.Company.DateFormat) : string.Empty;
            }
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.SALE ? "Invoice Number" : SaleEntry.EntryType == Entrytype.RETURN ? "Return Number" : "Quote Number");
            invoiceContent.Add("Method");
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.SALE ? "Invoice Date" : SaleEntry.EntryType == Entrytype.RETURN ? "Return Date" : "Quote Date");
            invoiceContent.Add(SaleEntry.EntryType != Entrytype.RETURN ? "Payment Terms" : "Sale Inv Number");
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.SALE ? "Due Date" : SaleEntry.EntryType == Entrytype.RETURN ? "Sale Inv Date" : "Exp Date");
            //Table Content
            invoiceContent.Add(SaleEntry.RefNumber);
            invoiceContent.Add(SaleEntry.SaleMethod == SaleMethod.Credit ? "CREDIT" : "CASH");
            invoiceContent.Add(SaleEntry.SaleDate.ToString(Global.Company.DateFormat));
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.RETURN ? SaleEntry.SaleRefQuoteReturn.RefNumber : PaymentTerm);
            invoiceContent.Add(SaleEntry.EntryType == Entrytype.RETURN ? SaleEntry.SaleRefQuoteReturn.SaleDate.ToString(Global.Company.DateFormat) : PaymentDate);
            int Count = invoiceContent.Count;
            PdfPTable MainTable = new PdfPTable((Count / 2));
            PdfPCell MainTableContent = new PdfPCell(new Phrase(""));
            for (int i = 0; i < Count; i++)
            {
                if (i < (Count / 2))
                {
                    MainTableContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), GetFont("Font_Bold_Italic_8_Black")));
                    MainTableContent.BorderColor = BaseColor.BLACK;
                    MainTableContent.BackgroundColor = new BaseColor(200, 200, 200);
                    MainTableContent.MinimumHeight = 16;
                    MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTable.AddCell(MainTableContent);
                }
                else
                {
                    MainTableContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), GetFont("Font_Normal_Italic_8_Black")));
                    MainTableContent.MinimumHeight = 14;
                    MainTableContent.BorderColor = BaseColor.BLACK;
                    MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTable.AddCell(MainTableContent);
                }
            }
            return MainTable;
        }

        public static PdfPTable PurchaseHeader(PurchaseEntry PurchaseEntry)
        {
            string CompanyName = Global.Company.DisplayAs + "\n";
            string Address = Global.Company.Address.FullAddressInSingleLine;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string SupplierDetail = string.Empty;

            //Company Contact Info
            if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "\nPhone: " + Global.Company.ContactInfo.Phone + fax;
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
            {
                Email = Global.Company.ContactInfo.Email == "" ? "" : "\nEmail: " + (Global.Company.ContactInfo).Email;
            }
            if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
            {
                Web = Global.Company.ContactInfo.WebSite == "" ? "" : "\nWeb: " + (Global.Company.ContactInfo).WebSite;
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

            //Heading
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "INVOICE" : "PURCHASE RETURN";

            //Customer Contact Info
            if (PurchaseEntry.AccountId != null)
            {
                Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)PurchaseEntry.AccountId);
                SupplierDetail = PurchaseEntry.SupplierName + "\n" + Supplier.Address.FullAddressInSingleLine + (string.IsNullOrEmpty(PurchaseEntry.SupplierAddress) ? "" : "\n") + (!string.IsNullOrEmpty(Supplier.ContactInfo.Phone) ? Supplier.ContactInfo.Phone + "\n" : "") + "Customer ID: " + PurchaseEntry.AccountId + "\n\n";
                if (Supplier.SupplierLicenceDetail.Count > 0)
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
            else
            {
                SupplierDetail = PurchaseEntry.SupplierName + "\n" + PurchaseEntry.SupplierAddress;
            }

            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 50f, 30f, 25f };
            if (Global.getLogoAsBytes() != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 40f, 25f, 25f };
            }

            //Create Header
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            //For Company Logo
            if (Global.getLogoAsBytes() != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(200f, 20f);
                image.ScaleAbsolute(70, 70);
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 16;
                HeadCell.Padding = 4;
                HeadCell.Rowspan = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeadTable.AddCell(HeadCell);
            }
            //company details
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(AddressData, GetFont("Font_Bold_Italic_12_Black")));
            AddressData = Address + Phone + Email + Web + CompanyLicense;
            HeadCell.AddElement(new Phrase(AddressData, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.Rowspan = 2;
            HeadTable.AddCell(HeadCell);

            //customer address
            HeadCell = new PdfPCell(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(SupplierDetail, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadCell.Rowspan = 2;
            HeadTable.AddCell(HeadCell);

            //header

            HeadCell = new PdfPCell(new Phrase(Heading, GetFont("Font_Bold_Italic_20_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = HeadColumns;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            //license
            HeadCell = new PdfPCell(new Phrase(CustomerLicense, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        public static PdfPTable PurchaseMiniHeader(PurchaseEntry PurchaseEntry)
        {
            string CompanyName = Global.Company.DisplayAs + "\n";
            string SupplierLicense = string.Empty;
            string Heading = string.Empty;
            string SupplierDetail = string.Empty;
            //Heading text
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "INVOICE" : "";
            //Supplier Contact Info
            if (PurchaseEntry.AccountId != null)
            {
                SupplierDetail = "Supplier ID: " + PurchaseEntry.AccountId;
            }
            else
            {
                SupplierDetail = PurchaseEntry.SupplierName;
            }
            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 50f, 30f, 25f };
            //Create Header
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            //company name           
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, GetFont("Font_Bold_Italic_12_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            //customer detail
            HeadCell = new PdfPCell(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(SupplierDetail, GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            //Heading

            HeadCell = new PdfPCell(new Phrase(Heading, GetFont("Font_Bold_Italic_20_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        public static PdfPTable PurchaseMainTable(PurchaseEntry PurchaseEntry)
        {
            string PaymentTerm = string.Empty;
            string PaymentDate = string.Empty;
            DateTime date = PurchaseEntry.ReturnDate;
            PurchaseEntry lEntry = null!;
            if (PurchaseEntry.PurchaseEntryId != null)
            {
                lEntry = PurchaseEntryManager.Instance.GetPurchaseEntry((long)PurchaseEntry.PurchaseEntryId);
            }
            if (PurchaseEntry.AccountId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)PurchaseEntry.AccountId);
                PaymentTerm = PurchaseEntry.Account != null ? Customer != null ? Customer.PaymentTerm.ToString() : "Immediate Payment" : "";
                // PaymentDate = PurchaseEntry.PurchaseEntryType == Entrytype.QUOTE ? PurchaseEntry..ToString(Global.Company.DateFormat) : string.Empty;
            }
            List<string> invoiceContent = new List<string>();
            //Table Heading
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Invoice Number" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "Purchase Inv Number" : "Purchase Order Number");
            if (PurchaseEntry.PurchaseEntrytype != PurchaseEntrytype.ORDER)
            {
                invoiceContent.Add("Ref Number");
            }
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Payment Method" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "Payment Method" : "Payment Method");
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "Return Date" : "Order Date");
            if (PurchaseEntry.PurchaseEntrytype != PurchaseEntrytype.ORDER)
            {
                invoiceContent.Add(lEntry.PurchaseInvNumber ?? "");
            }
            //Table Content
            invoiceContent.Add(PurchaseEntry.RefNumber);
            invoiceContent.Add(PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit ? "CREDIT" : "CASH");
            //invoiceContent.Add(PurchaseEntry.PurchaseInvDate.ToString(Global.Company.DateFormat));
            // invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? PurchaseEntry.SaleRefQuoteReturn.RefNumber : PaymentTerm);
            invoiceContent.Add(PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.ORDER ? PurchaseEntry.RefDate.ToString(Global.Company.DateFormat) : PurchaseEntry.ReturnDate.ToString(Global.Company.DateFormat));
            int Count = invoiceContent.Count;
            PdfPTable MainTable = new PdfPTable((Count / 2));
            PdfPCell MainTableContent = new PdfPCell(new Phrase(""));
            for (int i = 0; i < Count; i++)
            {
                if (i < (Count / 2))
                {
                    MainTableContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), GetFont("Font_Bold_Italic_9_White")));
                    MainTableContent.BorderColor = BaseColor.BLACK;
                    MainTableContent.BackgroundColor = new BaseColor(160, 160, 160);
                    MainTableContent.MinimumHeight = 16;
                    MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTable.AddCell(MainTableContent);
                }
                else
                {
                    MainTableContent = new PdfPCell(new Phrase(invoiceContent[i].Trim(), GetFont("Font_Normal_Italic_8_Black")));
                    MainTableContent.MinimumHeight = 14;
                    MainTableContent.BorderColor = BaseColor.BLACK;
                    MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTable.AddCell(MainTableContent);
                }
            }
            return MainTable;
        }


        public static PdfPTable PatientDetailHeader(string Memo)
        {
            List<string> Details = new List<string>();
            Details.AddRange(Memo.Split('\r'));
            PdfPTable PdfPTable = new PdfPTable(Details.Count);
            PdfPCell MainTableContent = new PdfPCell(new Phrase(""));
            for (int i = 0; i < Details.Count; i++)
            {
                MainTableContent = new PdfPCell(new Phrase(Details[i].Trim(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                MainTableContent.MinimumHeight = 14;
                MainTableContent.BorderColor = BaseColor.BLACK;
                MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPTable.AddCell(MainTableContent);
            }
            return PdfPTable;
        }
        public static PdfPTable PatientDetailHeader105(string Memo)
        {
            List<string> Details = new List<string>();
            Details.AddRange(Memo.Split('\r'));
            PdfPTable PdfPTable = new PdfPTable(Details.Count);
            PdfPCell MainTableContent = new PdfPCell(new Phrase(""));
            for (int i = 0; i < Details.Count; i++)
            {
                MainTableContent = new PdfPCell(new Phrase(Details[i].Trim(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                MainTableContent.UseVariableBorders = true;
                MainTableContent.BorderColorLeft = BaseColor.BLACK;
                MainTableContent.BorderColorTop = BaseColor.WHITE;
                MainTableContent.BorderColorRight = BaseColor.WHITE;
                MainTableContent.BorderColorBottom = BaseColor.BLACK;
                MainTableContent.MinimumHeight = 14;
                MainTableContent.BorderColor = BaseColor.BLACK;
                MainTableContent.HorizontalAlignment = Element.ALIGN_LEFT;
                PdfPTable.AddCell(MainTableContent);
            }
            return PdfPTable;
        }
        public static DataTable? DataGridViewAsDataTableAc(DataGridView dataGridView)
        {
            DataTable DataTable = new DataTable();
            if (dataGridView.Rows.Count != 0)
            {
                try
                {
                    if (dataGridView.ColumnCount == 0) return null;
                    foreach (DataGridViewColumn col in dataGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        DataTable.Columns.Add(col.Name, typeof(string));
                        DataTable.Columns[col.Name]!.Caption = col.HeaderText;
                    }
                    if (DataTable.Columns.Count == 0) return null;
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        DataRow drNewRow = DataTable.NewRow();
                        foreach (DataColumn col in DataTable.Columns)
                        {
                            if (col.ColumnName == "Balance")
                            {
                                double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString()!);
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : temp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            }
                            else
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : row.Cells[col.ColumnName].Value;
                        }
                        DataTable.Rows.Add(drNewRow);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
            return DataTable;
        }
        public static DataTable? DataGridViewAsDataTable(DataGridView ReportGridView)
        {
            DataTable dt = new DataTable();
            if (ReportGridView.Rows.Count != 0)
            {
                try
                {
                    if (ReportGridView.ColumnCount == 0) return null;
                    foreach (DataGridViewColumn col in ReportGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        dt.Columns.Add(col.Name, typeof(string));
                        dt.Columns[col.Name]!.Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return null;
                    String TempDate = string.Empty;
                    foreach (DataGridViewRow row in ReportGridView.Rows)
                    {
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.Caption == "P. Price" || col.Caption == "Cost" || col.Caption == "R. Price" || col.Caption == "W. Price"
                            || col.Caption == "MSRP")
                            {
                                double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString()!);
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : temp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            }
                            else
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;

                        }
                        dt.Rows.Add(drNewRow);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
            return dt;
        }
        public static PdfPTable DummyTable(int Row, int Column, int Height)
        {
            PdfPTable DummyTable = new PdfPTable(Column);
            for (int i = 0; i < (Row * Column); i++)
            {
                PdfPCell rowCell = new PdfPCell();
                rowCell = new PdfPCell(new Phrase("", GetFont("Font_Normal_Italic_8_Black")));
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderWidthTop = 0.1f;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                rowCell.MinimumHeight = Height;
                DummyTable.AddCell(rowCell);
            }
            return DummyTable;
        }
        public static PdfPTable DummyTableWithoutBoard(int Row, int Column, int Height)
        {
            PdfPTable DummyTable = new PdfPTable(Column);
            for (int i = 0; i < (Row * Column); i++)
            {
                PdfPCell rowCell = new PdfPCell();
                rowCell = new PdfPCell(new Phrase("", GetFont("Font_Normal_Italic_8_Black")));
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                rowCell.MinimumHeight = Height;
                DummyTable.AddCell(rowCell);
            }
            return DummyTable;
        }
        public static double CalculatePdfTableHeight(PdfPTable PdfPTable)
        {
            MemoryStream myMemoryStream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4, -30, -30, 30, 10);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
            pdfDoc.Open();
            pdfDoc.Add(PdfPTable);
            return PdfPTable.TotalHeight;
        }
        public static double ProductTaxPercentage(IList<ItemLevelSaleTaxDetail> SaleTaxDetails, SaleTaxType SaleTaxType)
        {
            double TaxTotal = 0.00;
            foreach (ItemLevelSaleTaxDetail Tax in SaleTaxDetails)
            {
                TaxTotal = TaxTotal + Tax.TaxRate;
            }
            return TaxTotal;
        }

        public static double ProductTaxPercentagePurchase(IList<LineLevelPurchaseTaxDetail> PurchaseTaxDetails, PurchaseTaxType PurchaseTaxType)
        {
            double TaxTotal = 0.00;
            foreach (LineLevelPurchaseTaxDetail Tax in PurchaseTaxDetails)
            {
                TaxTotal = TaxTotal + Tax.TaxRate;
            }
            return TaxTotal;
        }
        public static PdfPCell CreateFooterPageDateCell()
        {
            PdfPCell rowCell = new PdfPCell();
            var Temp = "Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString();
            rowCell = new PdfPCell(new Phrase(Temp, GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 10;
            rowCell.Padding = 2;
            rowCell.Colspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        public static PdfPCell CreateFooterPageNumberCell(string Number, string TotalPageNumber)
        {
            PdfPCell rowCell = new PdfPCell();
            var Temp = Number + "/" + TotalPageNumber;
            rowCell = new PdfPCell(new Phrase(Temp, GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 10;
            rowCell.Padding = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            return rowCell;
        }
        public static PdfPCell CreateEmptyCell()
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("", GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 20;
            rowCell.Padding = 2;
            return rowCell;
        }
        public static PdfPCell CreateEmptyCellWithHeigh(int Height)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("", GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = Height;
            return rowCell;
        }
        public static PdfPCell CreateEmptyCellWithTopBlackBorder()
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("", GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.MinimumHeight = 30;
            rowCell.Padding = 2;
            return rowCell;
        }
        public static PdfPCell CreateImageCellWithTopBlackBorder(iTextSharp.text.Image Img, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(Img);
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarder(string text, int span, iTextSharp.text.Font SetFont)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, SetFont));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 5;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarderLabTest(string text, int span, iTextSharp.text.Font SetFont)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, SetFont));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.MinimumHeight = 1;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithTopBoarder(string text, int span, iTextSharp.text.Font SetFont)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, SetFont));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.MinimumHeight = 1;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarder1(string text, int span, iTextSharp.text.Font SetFont)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, SetFont));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.MinimumHeight = 1;
            rowCell.Colspan = span;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarder2(string text, int span, iTextSharp.text.Font SetFont)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, SetFont));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 10;
            rowCell.Colspan = span;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarderandBold(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_TOP;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithNOBoarderandBold(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_TOP;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCells(iTextSharp.text.Image Img, int span, float imageWidth, float imageHeight)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(Img);
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = 0.01f;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            rowCell.Colspan = span;
            return rowCell;
        }

        public static PdfPCell CreateCellWithoutBoarderLeftAlign(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarderLeftAlignPatientChart(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        public static PdfPCell CreateCellWithTopBoarder(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.DARK_GRAY;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        public static PdfPCell CreateCellWithTopBoarderForPatinetChart(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.MinimumHeight = 10;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        public static PdfPCell CreateCellWithTopBoarder1(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_10_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.DARK_GRAY;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            return rowCell;
        }
        //public static PdfPCell CreateCellWithTopBoarderForLabTestResult(string text, int span)
        //{
        //    PdfPCell rowCell = new PdfPCell();
        //    rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
        //    rowCell.UseVariableBorders = true;
        //    rowCell.BorderColorLeft = BaseColor.WHITE;
        //    rowCell.BorderColorTop = BaseColor.WHITE;
        //    rowCell.BorderColorRight = BaseColor.WHITE;
        //    rowCell.BorderColorBottom = BaseColor.WHITE;
        //    rowCell.MinimumHeight = 14;
        //    rowCell.Colspan = span;
        //    rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    return rowCell;
        //}
        public static PdfPCell CreateCell(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCells(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = 0.01f;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCell1(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.PaddingBottom = 5f;
            rowCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell PatientInfoFristCell(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell PatientInfoSecoundCell(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateLastCells(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell Prec_HoursCellCreateCells(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellHeading(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellRightAlign(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourandBold(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_10_Black")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourandBoldWhiteText(string text, float Height)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_White")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = Height;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourandBoldWhiteTextRightAlign(string text, float Height)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_White")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = Height;
            return rowCell;
        }
        public static PdfPCell CreateHeadingCell(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_10_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = 2;
            return rowCell;
        }
        public static PdfPCell CreateHeadingCellPatientChart(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 1;
            rowCell.Colspan = 11;
            rowCell.Rowspan = 1;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            return rowCell;
        }
        public static PdfPCell CreateHeadingCellLab(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = 0.7f;
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.PaddingTop = 2;
            rowCell.PaddingBottom = 5;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            rowCell.Rowspan = 1;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return rowCell;
        }
        public static PdfPCell CreateDummyRow(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderWidth = (float)BorderStyle.None;
            rowCell.MinimumHeight = 3;
            rowCell.Colspan = span;
            rowCell.Rowspan = 1;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return rowCell;
        }
        public static PdfPCell CreateLabTestHeadingHeading(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 14;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColour(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_10_Black")));
            rowCell.BorderColor = BaseColor.DARK_GRAY;
            rowCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 25;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColours(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthBottom = 0.01f;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell HeaderLastCell(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.BorderColor = BaseColor.BLACK;
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = 0.01f;
            rowCell.BorderWidthTop = 0.01f;
            rowCell.BorderWidthLeft = 0.01f;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateHeadingCellWithSpan(string text, int cspan, int rspan)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = cspan;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_TOP;
            rowCell.NoWrap = false;
            return rowCell;
        }

        internal static string CreateCellWithTopBoarder1(string v1, int v2, iTextSharp.text.Font font)
        {
            throw new NotImplementedException();
        }
    }
}
