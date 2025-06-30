using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.views.utils.PrinterSetup;
using fa.api.OrderManagement;
using fa.model.OrderManagement;
using System.Data;
using static Fa.views.utils.Report.Purchase.PurchasePrintSaveA5A4;
using Fa.model.Purchase;
using fa.views.utils;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.api.utils;
using fa;
using iTextSharp.text;
using fa.model.Accounting.Masters;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;
using fa.api.Accounting;
using Font = iTextSharp.text.Font;


namespace Fa.views.utils.Report.Purchase
{
    internal class PurchaseOrderPrintSaveA5A4
    {
        public string? fileName;

        readonly String[] PurchaseOrderDetailsTableColumnName = new String[]
        {
        "#", "Item Name","UOM", "Qty", "Price", "Amount","Line Total"
        };
        public enum PurchaseOrderDetailsTableColumn
        {
            SNO, PRODUCT, UOM, QTY, PRICE, AMOUNT, REMOVE, ID, PURCHASEDETAILID
        }

        public void ExportToFileOrPrint(long PurchaseEntryId, string PrintPaper, string fileExtension, bool isPrint)
        {
            IList<TaxTable> lTaxTable = new List<TaxTable>();
            PurchaseEntryManager PurchaseEntryManager = PurchaseEntryManager.Instance;
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchaseEntryId);
            if (PurchaseEntry == null)
            {
                MessageBox.Show("Somthing went wrong, the selected sale is not valid.");
                return;
            }
            DataTable PurchaseOrderDataTableTotal = new DataTable();
            DataTable PurchaseOrderDataTable = new DataTable();
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.SNO], typeof(string));
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.PRODUCT], typeof(string));
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.UOM], typeof(string));
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.QTY], typeof(string));
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.PRICE], typeof(string));
            PurchaseOrderDataTable.Columns.Add(PurchaseOrderDetailsTableColumnName[(int)PurchaseOrderDetailsTableColumn.AMOUNT], typeof(string));
            if (PurchaseEntry.PurchaseDetails.Count != 0)
            {
                double SubTotal = 0;
                float LintTotal = 0;
                float TaxTotal = 0;
                float Discount = 0;
                int count = 0;
                try
                {
                    foreach (PurchaseDetails PurchaseDetails in PurchaseEntry.PurchaseDetails.OrderBy(x => x.Id))
                    {                        
                        DataRow PurchaseOrderDataTableNewRow = PurchaseOrderDataTable.NewRow();
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.SNO] = count.ToString();
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.UOM] = PurchaseDetails.Product.RetailUOM == PurchaseDetails.RetailUOM ? PurchaseDetails.Product.RetailUOM : PurchaseDetails.Product.WholesaleUOM;
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.PRODUCT] = PurchaseDetails.Product.Name.ToString();
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.QTY] = PurchaseDetails.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.PRICE] = PurchaseDetails.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        PurchaseOrderDataTableNewRow[(int)PurchaseOrderDetailsTableColumn.AMOUNT] = PurchaseDetails.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        //Add row
                        PurchaseOrderDataTable.Rows.Add(PurchaseOrderDataTableNewRow);
                        LintTotal = LintTotal + PurchaseDetails.Amount;
                        count++;
                    }
                    PurchaseOrderDataTableTotal.Columns.Add("1", typeof(string));
                    PurchaseOrderDataTableTotal.Columns.Add("2", typeof(string));
                    
                    PurchaseOrderDataTableTotal.Rows.Add(new object[] { "Grant Total", LintTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    double LintTotalTrans = Math.Round(LintTotal, 2);
                    
                }
                catch (DocumentException dex)
                {
                    MessageBox.Show(dex.ToString());
                }
                catch (IOException ioex)
                {
                    MessageBox.Show(ioex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ex.Message);
                }
            }
            PdfPTable signatureTable = SignatureTable();

            if (PurchaseOrderDataTable.Rows.Count != 0)
            {
                if (fileExtension == "Laser")
                {

                    PurchaseOrderPrintSaveA5A4 PurchaseOrderPrintSaveA5A4New = new PurchaseOrderPrintSaveA5A4();
                    //Customer need
                    PurchaseOrderPrintSaveA5A4New.GeneratePDF(PurchaseOrderDataTable, PurchaseOrderDataTableTotal, PurchaseEntry, PrintPaper, "pdf", isPrint, signatureTable);
                }
                else if (fileExtension == "Dotmatrix")
                {
                    fileName = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "PurchaseOrder" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.ORDER ? "PurchaseOrder" : "";
                    PurchaseOrderPrintSaveA5A4 PurchaseOrderPrintSaveA5A4New = new PurchaseOrderPrintSaveA5A4();
                    PurchaseOrderPrintSaveA5A4New.GenerateDotMatrixPDF(PurchaseOrderDataTable, PurchaseOrderDataTableTotal, PurchaseEntry, PrintPaper, "pdf", isPrint, signatureTable);
                }
            }
        }
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
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "TAX INVOICE" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.ORDER ? "PURCHASE ORDER" : "PURCHASE ORDER";
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
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.MinimumHeight = 5;
            HeadCell.Padding = -1;
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

            //customer address
            var xx = new Chunk(SupplierName, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"));
            xx.SetTextRise(-11);
            xx.setLineHeight(4);
            HeadCell = new PdfPCell(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Rowspan = 2;
            HeadCell.Padding = -4;
            HeadCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            HeadCell.AddElement(new Phrase("To", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.AddElement(xx);
            HeadCell.AddElement(new Phrase(SupplierDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
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
            Heading = PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "ORDER" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.ORDER ? "PURCHASE ORDER" : "PURCHASE ORDER";
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
        private static readonly Font FNI6BFont = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black"));
        private static readonly Font FBI9WFont = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_9_White"));

        public void GeneratePDF( DataTable dataTable, DataTable totalTable, PurchaseEntry PurchaseEntry, string PrintPaper, string fileExtension, bool isPrint, PdfPTable signatureTable)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = PrintPaper == "A5 LANDSCAPE" ? new Rectangle(595, 421) : PrintPaper == "A4 LANDSCAPE" ? PageSize.A4.Rotate() : PrintPaper == "A4 PORTRAIT" ? PageSize.A4 : new Rectangle(0, 0);
                double A4Height = PrintPaper == "A5 LANDSCAPE" ? 370 : PrintPaper == "A4 LANDSCAPE" ? 600 : PrintPaper == "A4 PORTRAIT" ? 700 : 0;
                int cols = PrintPaper == "A4 LANDSCAPE" ? 8 : 6;
                int rows = dataTable.Rows.Count;
                Document pdfDoc = new Document(pageSize, -30, -30, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                double lineHeight = 14;
                PdfPTable PurchaseMemoHeader = !string.IsNullOrEmpty(PurchaseEntry.Memo) ? PdfDataAlignment.PatientDetailHeader(PurchaseEntry.Memo) : null;
                PdfPTable PurchaseHeader = CusPurchaseHeader(PurchaseEntry);
                PdfPTable SaleMiniHeader = CusPurchaseMiniHeader(PurchaseEntry);
                PdfPTable PurchaseMainTable = PdfDataAlignment.PurchaseMainTable(PurchaseEntry);
                PdfPTable MiniDummyTableWithoutBoard = PdfDataAlignment.DummyTableWithoutBoard(1, 1, 2);
                PdfPTable MiniDummyTable = PdfDataAlignment.DummyTable(1, 1, 4);
                pdfDoc.Add(PurchaseHeader);
                if (!string.IsNullOrEmpty(PurchaseEntry.Memo))
                {
                    pdfDoc.Add(PurchaseMemoHeader);
                }
                pdfDoc.Add(PurchaseMainTable);
                pdfDoc.Add(MiniDummyTable);
                PdfPTable table = new PdfPTable(cols);
                float[] widths = new float[] { 15f, 100f, 25f, 32f, 40f, 22f };
                if (PrintPaper == "A4 LANDSCAPE")
                {
                    widths = new float[] { 10f, 75f, 20f, 30f, 30f, 30f, 35f, 35f };
                }
                double HeaderTableHeight = PurchaseHeader.TotalHeight + PurchaseMainTable.TotalHeight + MiniDummyTable.TotalHeight;
                double TotalWorkingOnPageH = HeaderTableHeight;
                table.SetWidths(widths);
                table = ColumnCaption(table, dataTable, PrintPaper);
                double PageSubTotal = 0;
                double PageTaxTotal = 0;
                double PageLineTotal = 0;
                string RefNumber = (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Prurchase : " : "Order : ") + PurchaseEntry.RefNumber;
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
                        var temp = dataTable.Rows[i][j].ToString();
                        if (j == 0)
                        {
                            temp = (i + 1).ToString();
                        }
                        rowCell.Padding = 4;
                        rowCell = new PdfPCell(new Phrase(temp, FNI6BFont));
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
                        if (j == 6)
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
                        else if ( j == 0 ||  j == 2)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
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
                    for (int j = 0; j < 2; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, FNI6BFont));
                        if (j == 0)
                        {
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 4 : 3;
                        }
                        rowCell.Padding = 4;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        totalPdfTable.AddCell(rowCell);
                    }
                }
                TotalWorkingOnPageH = (HeaderTableHeight + PdfDataAlignment.CalculatePdfTableHeight(table) + PdfDataAlignment.CalculatePdfTableHeight(totalPdfTable));
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
                    for (int j = 0; j < 2; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, FNI6BFont));
                        if (j == 0)
                        {
                            //changes for add dis total
                            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 6 : 5;
                        }
                        rowCell.Padding = 4;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);
                // Signature table
                double SignTableHeight = 0;
                PdfPTable SignPdfPTable = SignatureTable();
                SignTableHeight = PdfDataAlignment.CalculatePdfTableHeight(SignPdfPTable) + MiniDummyTable.HeaderHeight;
                TotalWorkingOnPageH = TotalWorkingOnPageH + SignTableHeight;

                if (TotalWorkingOnPageH > A4Height)
                {
                    // If the current content exceeds the page height, start a new page
                    pdfDoc.NewPage();
                    pdfDoc.Add(SaleMiniHeader);
                    pdfDoc.Add(MiniDummyTableWithoutBoard);
                    // Reset the working height for the new page
                    TotalWorkingOnPageH = MiniDummyTableWithoutBoard.TotalHeight + SaleMiniHeader.TotalHeight;
                }
                else
                {
                    pdfDoc.Add(MiniDummyTable);
                    pdfDoc.Add(SignPdfPTable);
                }
                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, (PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.PURCHASE ? "Purchase Order" : PurchaseEntry.PurchaseEntrytype == PurchaseEntrytype.RETURN ? "PurchaseReturnInvoice" : "PurchaseOrder"), fileExtension, isPrint, PrintPaper == "A5 LANDSCAPE" ? PaperTypes.A5_LANDSCAPE : PrintPaper == "A4 LANDSCAPE" ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
            }
        }
        public void GenerateDotMatrixPDF(DataTable dataTable, DataTable totalTable, PurchaseEntry PurchaseEntry, string PrintPaper, string fileExtension, bool isPrint, PdfPTable signatureTable)
        {

        }
        private PdfPTable ColumnCaption(PdfPTable table, DataTable dataTable, string PrintPaper)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                PdfPCell headerCell = new PdfPCell();

                headerCell = new PdfPCell(new Phrase(column.Caption, FBI9WFont));
                headerCell.BackgroundColor = new BaseColor(160, 160, 160);
                headerCell.BorderColor = BaseColor.BLACK;
                headerCell.MinimumHeight = 25;
                headerCell.Padding = 4;
                headerCell.Rowspan = 2;
                
                if (column.Caption == "Rate" || column.Caption == "Qty" || column.Caption == "Amount" || column.Caption == "Price")
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    headerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                table.AddCell(headerCell);
            }            
            return table;
        }
        private PdfPTable RunningTotal(PdfPTable table, string PrintPaper, double PageSubTotal, double PageTaxTotal, double PageLineTotal)
        {
            int Minimumheight = 12;
            PdfPCell rowCell = new PdfPCell();

            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK; rowCell.Padding = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            rowCell.MinimumHeight = 4;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 14 : 10;
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("To Be Continue...", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 7 : 3;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("Running Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.GRAY;
            rowCell.BorderColorRight = BaseColor.GRAY;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = 3;
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
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Padding = 3;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            table.AddCell(rowCell);
            rowCell = new PdfPCell(new Phrase("Continue Total", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = Minimumheight;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 7 : 3;
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
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.BLACK;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.BLACK;
            rowCell.MinimumHeight = 4;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Colspan = PrintPaper == "A4 LANDSCAPE" ? 14 : 10; ;
            table.AddCell(rowCell);
            return table;
        }
        private PdfPTable SignatureTable()
        {
            PdfPTable signPdfTable = new PdfPTable(3);
            float[] column = new float[] { 20f, 20f, 20f };
            signPdfTable.SetWidths(column);

            int minimumHeight = 12;

            PdfPCell rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n\n\n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
            rowCell.MinimumHeight = minimumHeight;
            rowCell.Rowspan = 2;
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;

            signPdfTable.AddCell(rowCell);

            return signPdfTable;
        }

        private PdfPTable SignatureTableby(DataTable totalTable)
        {
            int SignCol = 6;
            PdfPTable SignPdfTable = new PdfPTable(SignCol);
            float[] column = new float[SignCol];
            int j = 0;
            column[j] = 20f;
            for (j = 0; j < 5; j++)
            {
                column[j + 1] = (j + 1) / 2 == 0 ? 15f : 20f;
            }
            column[j ] = 20f;           
            int MinimumHeight = 12;
            SignPdfTable.SetWidths(column);
            PdfPCell rowCell = new PdfPCell();
            
            //signature
            rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Colspan = 3;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.BLACK;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            SignPdfTable.AddCell(rowCell);                     
            return SignPdfTable;
        }
        private PdfPTable TaxTable(IList<TaxTable> lTaxTable, DataTable totalTable)
        {
            int numtax = Global.Company.SalesTaxAccountMaps.Count;
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
            int MinimumHeight = 12;
            taxPdfTable.SetWidths(column);
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("Taxable Value", FNI6BFont));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            taxPdfTable.AddCell(rowCell);
            foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
            {
                rowCell = new PdfPCell(new Phrase(Map.Name, FNI6BFont));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = 2;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                taxPdfTable.AddCell(rowCell);
            }
            rowCell = new PdfPCell(new Phrase("Total Tax Amount", FNI6BFont));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
            taxPdfTable.AddCell(rowCell);
            //signature
            rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name + " \n Authorized Signatory", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
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
                rowCell = new PdfPCell(new Phrase("Rate", FNI6BFont));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                taxPdfTable.AddCell(rowCell);

                rowCell = new PdfPCell(new Phrase("Amount", FNI6BFont));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(220, 220, 220);
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
                        rowCell = new PdfPCell(new Phrase(llTaxTable.SubTotal.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), FNI6BFont));
                        rowCell.MinimumHeight = MinimumHeight;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        taxPdfTable.AddCell(rowCell);
                    }
                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxPer.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), FNI6BFont));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);

                    rowCell = new PdfPCell(new Phrase(llTaxTable.TaxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), FNI6BFont));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    taxAmount += llTaxTable.TaxAmount;
                    if (Count == numtax)
                    {
                        rowCell = new PdfPCell(new Phrase(taxAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), FNI6BFont));
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
                rowCell = new PdfPCell(new Phrase(totalTable.Rows[0][1].ToString(), FNI6BFont));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
                for (int i = 0; i < numtax; i++)
                {
                    rowCell = new PdfPCell(new Phrase("0.00", FNI6BFont));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                    rowCell = new PdfPCell(new Phrase("0.00", FNI6BFont));
                    rowCell.MinimumHeight = MinimumHeight;
                    rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    taxPdfTable.AddCell(rowCell);
                }
                rowCell = new PdfPCell(new Phrase("0.00", FNI6BFont));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                taxPdfTable.AddCell(rowCell);
            }
            return taxPdfTable;
        }
    }
}
