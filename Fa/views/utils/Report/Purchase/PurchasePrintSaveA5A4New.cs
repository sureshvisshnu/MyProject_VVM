using fa;
using fa.api.Accounting;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.views.utils;
using Fa.Utils.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using static fa.views.utils.PrinterSetup;
using Rectangle = iTextSharp.text.Rectangle;

namespace Fa.views.utils.Report.Purchase
{
    internal class PurchasePrintSaveA5A4New
    {

        public static PdfPTable CusPurchaseHeader(PurchaseEntry PurchaseEntry)
        {
            string CompanyName = Global.Company.DisplayAs + (string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan) + "\n";
            string Address = Global.Company.Address.FullAddressInSingleLine;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string CompanyLicense = string.Empty;
            string CustomerLicense = string.Empty;
            string Heading = string.Empty;
            string SupplierDetails = string.Empty;
            string SupplierName = string.Empty;
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
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "TAX INVOICE" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "PURCHASE RETURN INVOICE" : "RETURN INVOICE";
            //Customer Contact Info
            if (PurchaseEntry.AccountId != null)
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
            SupplierName = PurchaseEntry.Account.DisplayAs;
            SupplierDetails = (String.IsNullOrEmpty(PurchaseEntry.SupplierAddress) ? "" : "\n" + PurchaseEntry.SupplierAddress.Replace("\r", "").Replace("\n", "").Replace(",", ", ")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);
            //Set Header boundaries
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 50f, 30f, 30f };
            if (Global.getLogoAsBytes() != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 40f, 30f, 20f };
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
                image.ScaleAbsolute(60, 70);
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
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.MinimumHeight = 5;
            HeadCell.Padding = -1;
            HeadTable.AddCell(HeadCell);

            //customer address
            var xx = new Chunk(SupplierName, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"));
            xx.SetTextRise(-6);
            xx.setLineHeight(9);
            HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Rowspan = 3;
            HeadCell.PaddingLeft = 4;
            HeadCell.PaddingTop = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            HeadCell.AddElement(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(xx);
            HeadCell.AddElement(new Phrase(SupplierDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_15_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Rowspan = 3;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            AddressData = Address + Phone + Email + Web + CompanyLicense;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Padding = -1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.Rowspan = 2;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadCell.Colspan = HeadColumns;
            HeadTable.AddCell(HeadCell);
            return HeadTable;
        }
        //Purchase mini head
        public PdfPTable CusPurchaseMiniHeader(PurchaseEntry PurchaseEntry)
        {
            string CompanyName = Global.Company.DisplayAs + (string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan) + "\n";
            string SupplierLicense = string.Empty;
            string Heading = string.Empty;
            string SupplierDetail = string.Empty;
            //Heading text
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "TAX INVOICE" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "PURCHASE RETURN INVOICE" : "RETURN INVOICE";
            //Customer Contact Info
            if (PurchaseEntry.AccountId != null)
            {
                SupplierDetail = PurchaseEntry.SupplierName;
            }
            else if (!string.IsNullOrEmpty(PurchaseEntry.SupplierName))
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
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_12_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            //customer detail
            HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(new Phrase(SupplierDetail, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);
            //Heading
            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_15_LightGray")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);
            return HeadTable;
        }
        public void GeneratePDF(IList<TaxTable> lTaxTable, DataTable dataTable, DataTable totalTable, PurchaseEntry PurchaseEntry, string PrintPaper, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = PrintPaper == "A5 LANDSCAPE" ? new Rectangle(595, 421) : PrintPaper == "A4 LANDSCAPE" ? PageSize.A4.Rotate() : PrintPaper == "A4 PORTRAIT" ? PageSize.A4 : new Rectangle(0, 0);
                double A4Height = PrintPaper == "A5 LANDSCAPE" ? 370 : PrintPaper == "A4 LANDSCAPE" ? 600 : PrintPaper == "A4 PORTRAIT" ? 700 : 0;
                int cols = PrintPaper == "A4 LANDSCAPE" ? 16 : 13;
                int rows = dataTable.Rows.Count;
                Document pdfDoc = new Document(pageSize, -30, -30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                double lineHeight = 14;
                //PdfPTable PatientHeader = !string.IsNullOrEmpty(PurchaseEntry.Memo) ? PdfDataAlignment.PatientDetailHeader(PurchaseEntry.Memo) : null;
                PdfPTable PurchaseHeader = CusPurchaseHeader(PurchaseEntry);
                PdfPTable SaleMiniHeader = CusPurchaseMiniHeader(PurchaseEntry);
                PdfPTable PurchaseMainTable = PdfDataAlignment.PurchaseMainTable(PurchaseEntry);
                PdfPTable MiniDummyTableWithoutBoard = PdfDataAlignment.DummyTableWithoutBoard(1, 1, 2);
                PdfPTable MiniDummyTable = PdfDataAlignment.DummyTable(1, 1, 4);
                pdfDoc.Add(PurchaseHeader);
                //if (!string.IsNullOrEmpty(PurchaseEntry.Memo))
                //{
                //    pdfDoc.Add(PatientHeader);
                //}
                pdfDoc.Add(PurchaseMainTable);
                pdfDoc.Add(MiniDummyTable);
                PdfPTable table = new PdfPTable(cols);
                float[] widths = new float[] { 15f, 100f, 25f, 32f, 22f, 22f, 18f, 23f, 35f, 40f, 23f, 35f, 50f };
                if (PrintPaper == "A4 LANDSCAPE")
                {
                    widths = new float[] { 10f, 75f, 20f, 30f, 30f, 30f, 35f, 35f, 20f, 15f, 20f, 25f, 35f, 20f, 25f, 50f };
                }
                double HeaderTableHeight = PurchaseHeader.TotalHeight + PurchaseMainTable.TotalHeight + MiniDummyTable.TotalHeight;
                double TotalWorkingOnPageH = HeaderTableHeight;
                table.SetWidths(widths);
                table = ColumnCaption(table, dataTable, PrintPaper);
                double DisTotal = 0;
                double PageSubTotal = 0;
                double PageTaxTotal = 0;
                double PageLineTotal = 0;
                string RefNumber = (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Invoice: " : "Quotes: ") + PurchaseEntry.RefNumber;
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
                        HeaderTableHeight = SaleMiniHeader.TotalHeight + MiniDummyTableWithoutBoard.TotalHeight;
                        //Continue Total
                        table = ContinueTotal(table, PrintPaper, RefNumber, PageSubTotal, PageTaxTotal, PageLineTotal);
                        //Item Table Header New Page
                        table = ColumnCaption(table, dataTable, PrintPaper);
                    }
                    double TempHeight = lineHeight;
                    //add item detail
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if ((j == 4 || j == 5 || j == 7) && PrintPaper != "A4 LANDSCAPE")
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
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
                            string[] lines = temp.Split(new[] { "\n" }, StringSplitOptions.None);
                            TempHeight += lines.Count() == 1 ? 0 : lines.Count() == 2 ? 10.5 : 20.5;
                            float size = lines[0].Length > 40 ? 6 : lines[0].Length > 30 ? 7 : lines[0].Length > 20 ? 8 : 10;
                            TempHeight += size == 10 ? 0 : 10.5;
                            string FName = size == 6 ? "Font_Normal_Italic_5_Black" : size == 7 ? "Font_Normal_Italic_5_Black" : "Font_Normal_Italic_6_Black";
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
                            string[] lines = temp.Split(new[] { "\n" }, StringSplitOptions.None);
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
                    for (int j = 0; j < 5; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        if (j == 0)
                        {
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 12 : 9;
                        }
                        rowCell.Padding = 4;
                        rowCell.PaddingRight = 2;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        totalPdfTable.AddCell(rowCell);
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
                    TotalWorkingOnPageH = MiniDummyTableWithoutBoard.TotalHeight + SaleMiniHeader.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable);
                }
                for (int i = 0; i < rowstotalTable; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < 5; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        if (j == 0)
                        {
                            //changes for add dis total
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 12 : 9;
                        }
                        rowCell.Padding = 4;
                        rowCell.PaddingRight = 2;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);
                //tax table
                double TaxTableHeight = 0;
                if (Global.Company.SalesTaxAccountMaps.Count != 0)
                {
                    PdfPTable TaxPdfPTable = TaxTable(lTaxTable, totalTable, PurchaseEntry);
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
                PdfGeneration.SaveMemoryStream(myMemoryStream, (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Purchase Invoice" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "PurchaseReturnInvoice" : "SaleQuote"), fileExtension, isPrint, PrintPaper == "A5 LANDSCAPE" ? PaperTypes.A5_LANDSCAPE : PrintPaper == "A4 LANDSCAPE" ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
            }
        }
        private PdfPTable ColumnCaption(PdfPTable table, DataTable dataTable, string PrintPaper)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                PdfPCell headerCell = new PdfPCell();
                headerCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
                headerCell.BackgroundColor = new BaseColor(160, 160, 160);
                headerCell.BorderColor = BaseColor.BLACK;
                headerCell.MinimumHeight = 18;
                headerCell.Rowspan = 2;
                if ((column.Caption == "Batch No" || column.Caption == "Exp Date" || column.Caption == "MRP") && PrintPaper != "A4 LANDSCAPE")
                {
                    continue;
                }
                if ((column.Caption == "Dis %" || column.Caption == "Dis Amount"))
                {
                    if (column.Caption == "Dis %")
                    {
                        PdfPCell headerCell1 = new PdfPCell();
                        headerCell1 = new PdfPCell(new Phrase("Discount", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
                        headerCell1.Rowspan = 1;
                        headerCell1.Colspan = 2;
                        headerCell1.BackgroundColor = new BaseColor(160, 160, 160);
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
                    headerCell1 = new PdfPCell(new Phrase("Tax", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
                    headerCell1.Rowspan = 1;
                    headerCell1.Colspan = 2;
                    headerCell1.BackgroundColor = new BaseColor(160, 160, 160);
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
            headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            headerCellTaxDis = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_7_White")));
            headerCellTaxDis.BackgroundColor = new BaseColor(160, 160, 160);
            headerCellTaxDis.BorderColor = BaseColor.BLACK;
            headerCellTaxDis.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(headerCellTaxDis);
            return table;
        }
        private PdfPTable RunningTotal(PdfPTable table, string PrintPaper, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK; rowCell.Padding = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            rowCell.MinimumHeight = 4;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 16 : 12;
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("To Be Continue...", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 9 : 4;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Running Total", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 4 : 5;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.GRAY;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
            rowCell = new PdfPCell(new Phrase(RefNumber, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Continue Total", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 9 : 5;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageSubTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageTaxTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase(PageLineTotal.ToString("F"), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 16 : 12;
            table.AddCell(rowCell);
            return table;
        }
        private PdfPTable TaxTable(IList<TaxTable> lTaxTable, DataTable totalTable, PurchaseEntry PurchaseEntry)
        {
            int numtax = lTaxTable.Where(x => x.SaleTaxMapId != null).ToList().Count;
            //numtax = PurchaseEntry.SaleTaxType == SaleTaxType.INTER ? (numtax > 2 ? 2 : numtax) : (numtax > 1 ? 1 : numtax);
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
            rowCell = new PdfPCell(new Phrase("Taxable Value", PdfDataAlignment.GetFont("Font_Bold_Italic_6_White")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(160, 160, 160);
            taxPdfTable.AddCell(rowCell);
            foreach (TaxTable tax in lTaxTable/*CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps*/)
            {
                //if ((PurchaseEntry.SaleTaxType == SaleTaxType.INTER && (Map.AccountId == 31 || Map.AccountId == 32)) || (PurchaseEntry.SaleTaxType == SaleTaxType.INTRA && Map.AccountId == 28))
                //{
                rowCell = new PdfPCell(new Phrase(tax.SaleTaxMapId != null ? Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == tax.SaleTaxMapId)!.Name : "", PdfDataAlignment.GetFont("Font_Bold_Italic_6_White")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = 2;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.BackgroundColor = new BaseColor(160, 160, 160);
                taxPdfTable.AddCell(rowCell);
                //}
            }
            rowCell = new PdfPCell(new Phrase("Total Tax Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_6_White")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(160, 160, 160);
            taxPdfTable.AddCell(rowCell);
            //signature
            rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n\n\n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = (2 + (lTaxTable.Count > 0 ? lTaxTable.Count / 2 : 1));
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            taxPdfTable.AddCell(rowCell);
            for (int nt = 0; nt < numtax; nt++)
            {
                rowCell = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_6_White")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(160, 160, 160);
                taxPdfTable.AddCell(rowCell);
                rowCell = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_6_White")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(160, 160, 160);
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
                        rowCell = new PdfPCell(new Phrase(llTaxTable.SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                    }
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxPer.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    taxAmount += llTaxTable.TaxAmount;
                    if (Count == numtax)
                    {
                        rowCell = new PdfPCell(new Phrase(taxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
                rowCell = new PdfPCell(new Phrase(totalTable.Rows[0][1].ToString(), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
                for (int i = 0; i < numtax; i++)
                {
                    rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                }
                rowCell = new PdfPCell(new Phrase(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
            }
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
            int Cspan = IsBank && IsDeclaration && IsUPI ? TaxCol / 3 :
                        (IsBank && IsDeclaration && !IsUPI) ||
                        (IsBank && !IsDeclaration && IsUPI) ||
                        (!IsBank && IsDeclaration && IsUPI) ? TaxCol - (TaxCol / 2) :
                        (IsBank || IsDeclaration || IsUPI) ? TaxCol : 1;

            if (IsBank)
            {
                rowCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
                UpiUrl = UpiUrl.Replace("&am=100.00", "&am=" + PurchaseEntry.NetAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", ""));
                MemoryStream MStream = new MemoryStream();
                var Image = QRCode.GenerateQRCode(UpiUrl);
                Image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(MStream.ToArray());
                image.ScaleAbsoluteHeight(30);
                image.ScaleAbsoluteWidth(30);

                rowCell = new PdfPCell(new Phrase("Scan QR Code To Pay.", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
                rowCell.AddElement(new Phrase("  ", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.AddElement(image);
                taxPdfTable.AddCell(rowCell);
            }
            if (IsDeclaration)
            {
                rowCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.Declarations.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
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
