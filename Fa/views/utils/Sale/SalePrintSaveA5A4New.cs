using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.views.purchase;
using Fa.Utils.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using VisioForge.MediaFramework.Helpers;
using static fa.views.utils.PrinterSetup;
using Rectangle = iTextSharp.text.Rectangle;

namespace fa.views.utils.Sale
{
    public class SalePrintSaveA5A4New
    {
        public static PdfPTable CusSaleHeader(SaleEntry SaleEntry)
        {
            string CompanyName = Global.Company.DisplayAs+ (string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan) + "\n";
            string Address = Global.Company.Address.FullAddressInSingleLine;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;
            string CustomerName = string.Empty;
            string ReferrerName = string.Empty;
            ReferrerName = SaleEntry.ReferedById != null ? SaleEntry.ReferedBy.Name : string.Empty;
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
            Heading = SaleEntry.EntryType == Entrytype.SALE ? "TAX INVOICE" : SaleEntry.EntryType == Entrytype.QUOTE ? "QUOTATION": "RETURN INVOICE";
            //Customer Contact Info
            if (SaleEntry.AccountsId != null)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                if (Customer!=null && Customer.CustomerLicenceDetail.Count > 0)
                {
                    foreach (CustomerLicenceDetail Licence in Customer.CustomerLicenceDetail)
                    {
                        if (Licence.CompanyCustomerLicenseMaster!=null && Licence.CompanyCustomerLicenseMaster.IncludeInInvoice)
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
                CustomerName = SaleEntry.Account.DisplayAs;
            }
            else
            {
                CustomerName = !string.IsNullOrEmpty(SaleEntry.CustomerName) ? SaleEntry.CustomerName : "";
            }
            CustomerDetail = (String.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : SaleEntry.CustomerAddress.Replace("\r", "").Replace("\n", "").Replace(",", ", ")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);
            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 40f, 35f, 25f };
            if (Global.getLogoAsBytes() != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 40f, 30f, 15f };
            }
            //Create Header
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);
            //For Company Logo
            if (Global.getLogoAsBytes() != null)
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                float maxWidth = 60f;
                float maxHeight = 70f;
                image.ScaleToFit(maxWidth, maxHeight);
                //image.ScaleToFit(200f, 20f);
                //image.ScaleAbsolute(60, 70);
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 16;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadCell.Padding = 8;
                HeadCell.Rowspan = 3;
                HeadTable.AddCell(HeadCell);
            }
            //company details
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.MinimumHeight = 5;
            HeadCell.Padding = -1;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_12_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Rowspan = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            AddressData = Address + Phone + Email + Web + CompanyLicense;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.Rowspan = 3;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            
            HeadCell = new PdfPCell(new Phrase(CustomerName, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            
            HeadCell = new PdfPCell(new Phrase(CustomerDetail, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(ReferrerName != string.Empty ?  "Referred By : " + ReferrerName : " ", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadCell.Colspan = HeadColumns;
            HeadTable.AddCell(HeadCell);
            return HeadTable;
        }       
        //sale mini head
        public PdfPTable CusSaleMiniHeader(SaleEntry SaleEntry)
        {
            string CompanyName = Global.Company.DisplayAs+(string.IsNullOrEmpty(Global.Company.Slogan)?"":"\n"+ Global.Company.Slogan) + "\n";
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string CustomerDetail = string.Empty;
            //Heading text
            Heading = SaleEntry.EntryType == Entrytype.SALE ? "TAX INVOICE" : SaleEntry.EntryType == Entrytype.QUOTE ? "QUOTATION" : "RETURN INVOICE";
            //Customer Contact Info
            if (SaleEntry.AccountsId != null)
            {
                CustomerDetail =SaleEntry.CustomerName;
            }
            else if(!string.IsNullOrEmpty(SaleEntry.CustomerName))
            {
                CustomerDetail =SaleEntry.CustomerName;
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
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            //customer detail
            HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(CustomerDetail, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);
            //Heading
            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_12_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);
            return HeadTable;
        }
        public void GeneratePDF(IList<TaxTable> lTaxTable, DataTable dataTable, DataTable totalTable, SaleEntry SaleEntry, string PrintPaper, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = PrintPaper == "A5 LANDSCAPE" ? new Rectangle(595, 421) : PrintPaper == "A4 LANDSCAPE" ? PageSize.A4.Rotate() : PrintPaper == "A4 PORTRAIT" ? PageSize.A4 : new Rectangle(0, 0);
                double A4Height = PrintPaper == "A5 LANDSCAPE" ? 339 : PrintPaper == "A4 LANDSCAPE" ? 700 : PrintPaper == "A4 PORTRAIT" ? 700 : 0;
                int cols = PrintPaper == "A4 LANDSCAPE" ? 16 : PrintPaper == "A5 LANDSCAPE" ? 13 : 12;
                int rows = dataTable.Rows.Count;
                Document pdfDoc = new Document(pageSize, -50, -60, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                double lineHeight = 24;
                PdfPTable PatientHeader =!string.IsNullOrEmpty(SaleEntry.Memo)?PdfDataAlignment.PatientDetailHeader(SaleEntry.Memo):null;
                PdfPTable SaleHeader = CusSaleHeader(SaleEntry);
                PdfPTable SaleMiniHeader = CusSaleMiniHeader(SaleEntry);
                PdfPTable SaleMainTable = PdfDataAlignment.SaleMainTable(SaleEntry);
                PdfPTable MiniDummyTableWithoutBoard = PdfDataAlignment.DummyTableWithoutBoard(1, 1, 2);
                PdfPTable MiniDummyTable = PdfDataAlignment.DummyTable(1, 1, 4);
                pdfDoc.Add(SaleHeader);
                if(!string.IsNullOrEmpty(SaleEntry.Memo))
                {
                    pdfDoc.Add(PatientHeader);
                }
                pdfDoc.Add(SaleMainTable);
                pdfDoc.Add(MiniDummyTable);
                PdfPTable table = new PdfPTable(cols);
                float[] widths = new float[] { 15f, 100f,25f, 32f, 40f, 22f, 23f, 35f, 40f, 23f, 35f, 50f };
                if (PrintPaper == "A4 LANDSCAPE")
                {
                    widths = new float[] { 10f, 75f,20f, 30f, 30f, 30f, 35f, 35f, 20f, 15f, 20f, 25f, 35f, 20f, 25f, 50f };
                }
                if (PrintPaper == "A5 LANDSCAPE")
                {
                    widths = new float[] { 15f, 100f, 25f, 32f, 30f, 25f, 22f, 23f, 35f, 35f, 23f, 35f, 45f };
                }
                double HeaderTableHeight = SaleHeader.TotalHeight + SaleMainTable.TotalHeight + MiniDummyTable.TotalHeight+(!string.IsNullOrEmpty(SaleEntry.Memo) ? PatientHeader .TotalHeight: 0);
                double TotalWorkingOnPageH = HeaderTableHeight;
                table.SetWidths(widths);
                table = ColumnCaption(table, dataTable, PrintPaper);
                double DisTotal = 0;
                double PageSubTotal = 0;
                double PageTaxTotal = 0;
                double PageLineTotal = 0;
                string RefNumber = (SaleEntry.EntryType == Entrytype.SALE ? "Invoice: " : "Quotes: ") + SaleEntry.RefNumber;
                //Add Item
                for (int i = 0; i < rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    if (TotalWorkingOnPageH >= A4Height)
                    {
                        //Running Total
                        table = RunningTotal(table, PrintPaper, PageSubTotal, PageTaxTotal, PageLineTotal);
                        pdfDoc.Add(table);
                        pdfDoc.NewPage();
                        table = new PdfPTable(cols);
                        table.SetWidths(widths);
                        pdfDoc.Add(SaleMiniHeader);
                        pdfDoc.Add(MiniDummyTableWithoutBoard);
                        HeaderTableHeight = SaleMiniHeader.TotalHeight+ MiniDummyTableWithoutBoard.TotalHeight;
                        //Continue Total
                        table = ContinueTotal(table, PrintPaper, RefNumber, PageSubTotal, PageTaxTotal, PageLineTotal);
                        //Item Table Header New Page
                        table = ColumnCaption(table, dataTable, PrintPaper);
                    }
                    double TempHeight = lineHeight;
                    //add item detail
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if ((j == 4 || j == 5 || j == 7 || j == 9 ) && PrintPaper != "A4 LANDSCAPE")
                        {
                            if ((PrintPaper == "A5 LANDSCAPE" && j == 7) || (PrintPaper == "A4 PORTRAIT" && j == 7)) { }
                            else { continue; }
                        }
                        if (j == 6  && PrintPaper == "A4 PORTRAIT")
                        {
                            continue;
                        }
                        var temp = (j == 9 ? dataTable.Rows[i][j /*+ 1*/].ToString() : dataTable.Rows[i][j].ToString());
                        DisTotal += j == 9 ? double.Parse(temp) : 0;
                        if (j == 11)
                        {
                            string[] lines = temp.Split(new[] { "\n" }, StringSplitOptions.None);
                            temp = lines[lines.Count() - 1];
                        }
                        rowCell.Padding = 4;
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.UseVariableBorders = true;
                        rowCell.BorderColorLeft = BaseColor.LIGHT_GRAY;
                        rowCell.BorderColorTop = BaseColor.WHITE;
                        rowCell.BorderColorRight = BaseColor.LIGHT_GRAY;
                        rowCell.BorderColorBottom = BaseColor.WHITE;
                        if (i == 0)
                        {
                            rowCell.BorderColorTop = BaseColor.BLACK;
                        }
                        if (j == 0)
                        {
                            rowCell.BorderColorLeft = BaseColor.BLACK;
                        }
                        if (j == 15)
                        {
                            rowCell.BorderColorRight = BaseColor.BLACK;
                        }
                        //Row Odd Even Color
                        if (i % 2 != 0)
                        {
                            rowCell.BackgroundColor = new BaseColor(242, 242, 242);
                        }
                        else
                        {
                            rowCell.BackgroundColor = BaseColor.WHITE;
                        }
                        if (j == 1)
                        {
                            string[] lines = temp.Split(new[] { "\r" }, StringSplitOptions.None);
                            TempHeight += lines.Count() == 1 ? 0 : lines.Count() == 2 ? 10.5 : 20.5;
                            float size = lines[0].Length > 40 ? 6 : lines[0].Length > 30 ? 7 : lines[0].Length > 20 ? 8 : 10;
                            TempHeight += size == 10 ? 0 : 10.5;
                            string FName = size == 6 ? "Font_Normal_Italic_7_Black" : size == 7 ? "Font_Normal_Italic_7_Black" : "Font_Normal_Italic_8_Black";
                            rowCell.AddElement(new Phrase(lines[0], PdfDataAlignment.GetFont(FName)));
                            rowCell.HorizontalAlignment = Element.ALIGN_BASELINE;
                            rowCell.VerticalAlignment = Element.ALIGN_BASELINE;
                            rowCell.PaddingTop = -3;
                        }
                        else if (j == 3 || j == 4 || j == 0 || j == 2)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            if (j == 4)
                            {
                                string[] lines = temp.Split(new[] { "\n" },
                                                               StringSplitOptions.None
                                                                );
                                float TH = lines.Count() == 1 ? 14 : 25;

                                if (TH > TempHeight)
                                {
                                    TempHeight = lines.Count() == 1 ? 14 : 25;
                                }
                            }
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        if (j == 12)
                        {
                            string[] lines = temp.Split(new[] { "\n" },StringSplitOptions.None);
                            PageSubTotal += double.Parse(lines[lines.Count() - 1]);
                        }
                        if (j == 14)
                        {
                            PageTaxTotal += double.Parse(temp);
                        }
                        if (j == 15)
                        {
                            PageLineTotal += double.Parse(temp);
                        }
                        table.AddCell(rowCell);
                    }
                    TotalWorkingOnPageH = (HeaderTableHeight + PdfDataAlignment.CalculatePdfTableHeight(table));
                }
                //Total Table
                int rowstotalTable = totalTable.Rows.Count;
                PdfPTable totalPdfTable = new PdfPTable(cols);
                totalPdfTable.SetWidths(widths);
                for (int i = 0; i < rowstotalTable; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < 6; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        if (i == (rowstotalTable - 1) && j == 5)
                        {
                            rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        }
                        rowCell.PaddingTop = 4;
                        rowCell.PaddingBottom = 4;
                        rowCell.PaddingRight = 2;
                        rowCell.BorderColor = BaseColor.BLACK;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        if (j == 0)
                        {
                            rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 10 : PrintPaper == "A5 LANDSCAPE" ? 7 : 6;
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            rowCell.PaddingTop = 2;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if (j == 1)
                        {
                            rowCell.Colspan = 2;
                            rowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        table.AddCell(rowCell);
                    }
                }
                TotalWorkingOnPageH = (HeaderTableHeight + PdfDataAlignment.CalculatePdfTableHeight(table)
                    + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable));
                if (TotalWorkingOnPageH > A4Height)
                {
                    PdfPCell rowCell = new PdfPCell();
                    //Running Total
                    table = RunningTotal(table, PrintPaper, PageSubTotal, PageTaxTotal, PageLineTotal);
                    pdfDoc.Add(table);
                    pdfDoc.NewPage();
                    pdfDoc.Add(SaleMiniHeader);
                    pdfDoc.Add(MiniDummyTableWithoutBoard);
                    table = new PdfPTable(cols);
                    table.SetWidths(widths);
                    //Continue Total
                    table = ContinueTotal(table, PrintPaper, RefNumber, PageSubTotal, PageTaxTotal, PageLineTotal);
                    TotalWorkingOnPageH = MiniDummyTableWithoutBoard.TotalHeight+ SaleMiniHeader.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable);
                }
                pdfDoc.Add(table);
                //tax table
                double TaxTableHeight = 0;
                if (Global.Company.SalesTaxAccountMaps.Count != 0)
                {
                    PdfPTable TaxPdfPTable = TaxTable(lTaxTable, totalTable, SaleEntry);
                    TaxTableHeight = PdfDataAlignment.CalculatePdfTableHeight(TaxPdfPTable) + MiniDummyTable.HeaderHeight;
                    TotalWorkingOnPageH = TotalWorkingOnPageH + TaxTableHeight;
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.NewPage();
                        pdfDoc.Add(SaleMiniHeader);
                        pdfDoc.Add(MiniDummyTableWithoutBoard);
                    }
                    else
                    {
                        pdfDoc.Add(MiniDummyTable);
                    }
                    pdfDoc.Add(TaxPdfPTable);
                }      
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, (SaleEntry.EntryType == Entrytype.SALE ? "SaleInvoice" : SaleEntry.EntryType == Entrytype.RETURN ? "SaleReturnInvoice": "SaleQuote"), fileExtension, isPrint, PrintPaper == "A5 LANDSCAPE" ? PaperTypes.A5_LANDSCAPE : PrintPaper == "A4 LANDSCAPE" ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
            }
        }
        private PdfPTable ColumnCaption(PdfPTable table, DataTable dataTable, string PrintPaper)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                PdfPCell headerCell = new PdfPCell();
                headerCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                headerCell.BackgroundColor = new BaseColor(200, 200, 200);
                headerCell.BorderColor = BaseColor.BLACK;
                headerCell.MinimumHeight = 18;
                headerCell.Rowspan = 2;
                if ((column.Caption == "Batch No" || column.Caption == "Exp Date" || column.Caption == "MRP" || column.Caption == "Free") && PrintPaper != "A4 LANDSCAPE")
                {
                    if (column.Caption == "MRP" && PrintPaper == "A5 LANDSCAPE") { }
                    else{ continue;}
                }
                if ((column.Caption == "Dis %" || column.Caption == "Dis Amount"))
                {
                    if (column.Caption == "Dis %")
                    {
                        PdfPCell headerCell1 = new PdfPCell();
                        headerCell1 = new PdfPCell(new Phrase("Discount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        headerCell1.Rowspan = 1;
                        headerCell1.Colspan = 2;
                        headerCell1.BackgroundColor = new BaseColor(200, 200, 200);
                        headerCell1.BorderColor = BaseColor.BLACK;
                        headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(headerCell1);
                        continue;
                    }
                    if (column.Caption == "Dis Amount")
                    {
                        headerCell.Rowspan = 1;
                        continue;
                    }                    
                }
                if (column.Caption == "%")
                {
                    PdfPCell headerCell1 = new PdfPCell();
                    headerCell1 = new PdfPCell(new Phrase("Tax", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    headerCell1.Rowspan = 1;
                    headerCell1.Colspan = 2;
                    headerCell1.BackgroundColor = new BaseColor(200, 200, 200);
                    headerCell1.BorderColor = BaseColor.BLACK;
                    headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(headerCell1);
                    continue;
                }
                if (column.Caption == "Amount")
                {
                    headerCell.Rowspan = 1;
                    continue;
                }
                if (column.Caption == "Free" || column.Caption == "Sub Total" || column.Caption == "Line Total" || column.Caption == "Rate" || column.Caption == "Qty" || column.Caption == "Amount" || column.Caption == "MRP")
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                table.AddCell(headerCell);
            }
            PdfPCell headerCellTaxDis = new PdfPCell();
            headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            headerCellTaxDis.BackgroundColor = new BaseColor(200, 200, 200);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            headerCellTaxDis.BackgroundColor = new BaseColor(200, 200, 200);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            headerCellTaxDis.BackgroundColor = new BaseColor(200, 200, 200);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            headerCellTaxDis.BackgroundColor = new BaseColor(200, 200, 200);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            return table;
        }
        private PdfPTable RunningTotal(PdfPTable table, string PrintPaper, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("To Be Continue...", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 8 : 4;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Running Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 4:5;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.BLACK;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            return table;
        }
        private PdfPTable ContinueTotal(PdfPTable table, string PrintPaper, string RefNumber, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(RefNumber, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Continue Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 8 : 5;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            //rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            //rowCell.UseVariableBorders = true;
            //rowCell.BorderColorLeft = BaseColor.WHITE;
            //rowCell.BorderColorTop = BaseColor.BLACK;
            //rowCell.BorderColorRight = BaseColor.WHITE;
            //rowCell.BorderColorBottom = BaseColor.BLACK;
            //rowCell.MinimumHeight = 4;
            //rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 16 :12;
            //table.AddCell(rowCell);
            return table;
        }
        private PdfPTable TaxTable(IList<TaxTable> lTaxTable, DataTable totalTable, SaleEntry SaleEntry)
        {
            int numtax = lTaxTable.Where(x=>x.SaleTaxMapId!=null).ToList().Count;
            //numtax = SaleEntry.SaleTaxType == SaleTaxType.INTER ? (numtax > 2 ? 2 : numtax) : (numtax > 1 ? 1 : numtax);
            int TaxCol = 5 + (numtax * 2);
            PdfPTable taxPdfTable = new PdfPTable(TaxCol);
            float[] column = new float[TaxCol];
            int j = 0;
            column[j] = 20f;
            for (j = 0; j < (numtax * 2); j++)
            {
                column[j + 1] = (j + 1) / 2 == 0 ? 15f : 20f;
            }
            column[j + 1] = 20f;
            column[j + 2] = 30f;
            column[j + 3] = 30f;
            column[j + 4] = 30f;
            int MinimumHeight = 10;
            taxPdfTable.SetWidths(column);
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("Taxable Value", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
            taxPdfTable.AddCell(rowCell);
            foreach (TaxTable tax in lTaxTable/*CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps*/)
            {
                //if ((SaleEntry.SaleTaxType == SaleTaxType.INTER && (Map.AccountId == 31 || Map.AccountId == 32)) || (SaleEntry.SaleTaxType == SaleTaxType.INTRA && Map.AccountId == 28))
                //{
                    rowCell = new PdfPCell(new Phrase(tax.SaleTaxMapId != null?Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == tax.SaleTaxMapId).Name:"", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.Colspan = 2;
                    rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                taxPdfTable.AddCell(rowCell);
                //}
            }
            rowCell = new PdfPCell(new Phrase("Total Tax Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
            rowCell.BorderColorRight = BaseColor.BLACK;
            taxPdfTable.AddCell(rowCell);
            //signature
            rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n\n\n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = (2 + (lTaxTable.Count > 0 ? lTaxTable.Count / 2 : 1));
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            taxPdfTable.AddCell(rowCell);
            for (int nt = 0; nt < numtax; nt++)
            {
                rowCell = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                taxPdfTable.AddCell(rowCell);
                rowCell = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                taxPdfTable.AddCell(rowCell);
            }
            if (lTaxTable.Count > 0)
            {
                double taxAmount = 0.00;
                int Count = 1;
                foreach (TaxTable llTaxTable in lTaxTable.OrderBy(x => x.SubTotal))
                {
                    if (Count == 1)
                    {
                        rowCell = new PdfPCell(new Phrase(llTaxTable.SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                    }
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxPer.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    taxAmount += llTaxTable.TaxAmount;
                    if (Count == numtax)
                    {
                        rowCell = new PdfPCell(new Phrase(taxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                        Count = 0;
                        taxAmount = 0.00;
                    }
                    Count++;
                }
            }
            else
            {
                rowCell = new PdfPCell(new Phrase(totalTable.Rows[0][1].ToString(), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
                for (int i = 0; i < numtax; i++)
                {
                    rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                }
                rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);                
            }
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Colspan = TaxCol;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            taxPdfTable.AddCell(rowCell);

            bool IsBank = Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails) ? true : false;
            bool IsDeclaration = Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations) ? true : false;
            bool IsUPI = Global.Company.CompanySalesSetup.IsPrintQRCode;
            int Cspan = IsBank && IsDeclaration && IsUPI ? (TaxCol == 9 || TaxCol == 7) ? 3 : TaxCol == 5 ? 2 : 1:
                        (IsBank && IsDeclaration && !IsUPI) ||
                        (IsBank && !IsDeclaration && IsUPI) ||
                        (!IsBank && IsDeclaration && IsUPI) ? TaxCol - (TaxCol / 2) :
                        (IsBank || IsDeclaration || IsUPI) ? TaxCol : 1;

            if (IsBank)
            {
                rowCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = Cspan;
                rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                taxPdfTable.AddCell(rowCell);
            }
            if (IsUPI)
            {
                string UpiUrl = Global.Company.CompanySalesSetup.UPIId;
                UpiUrl = UpiUrl.Replace("&am=100.00", "&am=" + SaleEntry.NetAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", ""));
                MemoryStream MStream = new MemoryStream();
                var Image = QRCode.GenerateQRCode(UpiUrl);
                Image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(MStream.ToArray());
                image.ScaleAbsoluteHeight(50);
                image.ScaleAbsoluteWidth(50);

                rowCell = new PdfPCell(new Phrase("Scan QR Code To Pay.", PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                rowCell.MinimumHeight = 20;
                rowCell.Colspan = Cspan;
                rowCell.Rowspan = 3;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                rowCell.AddElement(new Phrase("Scan QR Code To Pay.", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                rowCell.AddElement(new Phrase("  ", PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                rowCell.AddElement(image);
                taxPdfTable.AddCell(rowCell);
            }
            if (IsDeclaration)
            {
                rowCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.Declarations.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = Cspan;
                rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                taxPdfTable.AddCell(rowCell);
            }           
            return taxPdfTable;
        }
    }
}
