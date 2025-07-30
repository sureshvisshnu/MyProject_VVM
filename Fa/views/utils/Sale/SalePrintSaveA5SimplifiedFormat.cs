using fa;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.views.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Rectangle = iTextSharp.text.Rectangle;

namespace Fa.views.utils.Sale
{
    public class SalePrintSaveA5SimplifiedFormat
    {
        readonly String[] SaleDetailsTableColumnName = new String[]
        {
            "S.No",
            "Description",
            "Qty",
            "Unit",  // New column header
            "Rate",
            "Amount"
        };

        public enum SaleDetailsTableColumn
        {
            SNO,
            DESC,
            QTY,
            UNIT,  // New column for UOM
            RATE,
            TOTALAMOUNT
        }

        public void ExportToFileOrPrint20072025(long SalesId, string PrintPaper, string fileExtension, bool isPrint, Entrytype entrytype, bool isLandscape = false)
        {
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);

            if (SaleEntry == null)
            {
                MessageBox.Show("Something went wrong, the selected sale is not valid.");
                return;
            }

            DataTable SaleDetailsTable = new DataTable();

            // Create columns for the simplified table
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.UNIT], typeof(string)); // New column
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TOTALAMOUNT], typeof(string));

            if (SaleEntry.SaleDetails.Count != 0)
            {
                double TotalAmount = 0;
                double TotalQty = 0;
                int count = 0;

                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x => x.Id))
                    {
                        count++;
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);

                        double Qty = SaleDetails.Quantity;
                        double Price = SaleDetails.OverridePrice == 0 ? SaleDetails.Price : SaleDetails.OverridePrice;
                        double DiscountAmount = lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        double LineTotal = (Qty * Price) - DiscountAmount;

                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SNO] = count.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = TruncateString(SaleDetails.Product?.Name ?? "", 30);
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.QTY] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.UNIT] = SaleDetails.Uom; // Add UOM
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.RATE] = Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(LineTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                        TotalAmount += LineTotal;
                        TotalQty += Qty;
                    }

                    // Add total row
                    DataRow TotalRow = SaleDetailsTable.NewRow();
                    TotalRow[(int)SaleDetailsTableColumn.DESC] = "TOTAL";
                    TotalRow[(int)SaleDetailsTableColumn.QTY] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    TotalRow[(int)SaleDetailsTableColumn.UNIT] = ""; // Empty for total row
                    TotalRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(TotalAmount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SaleDetailsTable.Rows.Add(TotalRow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating invoice: " + ex.Message);
                    return;
                }

                if (fileExtension == "Laser")
                {

                    GeneratePDF(SaleDetailsTable, SaleEntry, PrintPaper, "pdf", isPrint, isLandscape, TotalAmount, entrytype);
                }
            }
        }
        public void GeneratePDF20072025(DataTable dataTable, SaleEntry saleEntry, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape, double TotalAmount)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                // Set page size based on orientation
                var pageSize = isLandscape ? PageSize.A5.Rotate() : PageSize.A5;

                Document pdfDoc = new Document(pageSize, 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                // Add header
                PdfPTable DocHeader = InvoiceHeader("SALES INVOICE", saleEntry.RefNumber, saleEntry.SaleDate, Entrytype.SALE);
                pdfDoc.Add(DocHeader);

                // Add customer info
                PdfPTable CustomerTable = CustomerDetails(saleEntry);
                pdfDoc.Add(CustomerTable);

                // Add space between sections
                pdfDoc.Add(new Paragraph(" "));

                // Create main items table
                PdfPTable table = new PdfPTable(SaleDetailsTableColumnName.Length);

                // Set column widths based on orientation
                float[] widths = isLandscape
                    ? new float[] { 8f, 70f, 20f, 20f, 25f, 30f }  // Added width for Unit column
                    : new float[] { 8f, 40f, 15f, 15f, 20f, 25f }; // Added width for Unit column

                table.SetWidths(widths);
                table = CreateSalesMainTableHeader(table, dataTable);

                // Add data rows
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        var temp = dataTable.Rows[i][j].ToString();
                        var font = i == dataTable.Rows.Count - 1 // Last row (total)
                            ? PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")
                            : PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black");

                        PdfPCell rowCell = new PdfPCell(new Phrase(temp, font));

                        // Styling for total row
                        if (i == dataTable.Rows.Count - 1)
                        {
                            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                        }

                        // Set alignment - right for numeric columns, left for others
                        if (j == (int)SaleDetailsTableColumn.QTY || j == (int)SaleDetailsTableColumn.RATE || j == (int)SaleDetailsTableColumn.TOTALAMOUNT)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        }

                        rowCell.MinimumHeight = 15;
                        rowCell.BorderWidth = 0.5f;
                        table.AddCell(rowCell);
                    }
                }

                pdfDoc.Add(table);

                // Add amount in words
                pdfDoc.Add(new Paragraph(" "));
                PdfPTable AmountInWords = AmtInWordsColumn(TotalAmount);
                pdfDoc.Add(AmountInWords);

                // Add signature line
                pdfDoc.Add(new Paragraph(" "));
                PdfPTable SignatureTable = SignatureColumn();
                pdfDoc.Add(SignatureTable);

                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, "SaleInvoice", fileExtension, isPrint,
                    isLandscape ? PaperTypes.A5_LANDSCAPE : PaperTypes.A5_PORTRAIT);
            }
        }
        public void ExportToFileOrPrint(long SalesId, string PrintPaper, string fileExtension, bool isPrint, Entrytype entrytype, bool isLandscape = false)
        {
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);

            if (SaleEntry == null)
            {
                MessageBox.Show("Something went wrong, the selected sale is not valid.");
                return;
            }

            DataTable SaleDetailsTable = new DataTable();

            // Create columns for the simplified table
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.UNIT], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TOTALAMOUNT], typeof(string));

            if (SaleEntry.SaleDetails.Count != 0)
            {
                double TotalAmount = 0;
                double TotalQty = 0;
                int count = 0;

                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x => x.Id))
                    {
                        count++;
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);

                        double Qty = SaleDetails.Quantity;
                        double Price = SaleDetails.OverridePrice == 0 ? SaleDetails.Price : SaleDetails.OverridePrice;
                        double DiscountAmount = lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        double LineTotal = (Qty * Price) - DiscountAmount;

                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SNO] = count.ToString();

                        // Truncate product name if too long
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = TruncateString(SaleDetails.Product?.Name ?? "", 30);
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.QTY] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        // Add UOM
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.UNIT] = SaleDetails.Uom;
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.RATE] = Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(LineTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                        TotalAmount += LineTotal;
                        TotalQty += Qty;
                    }

                    // Add total row
                    DataRow TotalRow = SaleDetailsTable.NewRow();
                    TotalRow[(int)SaleDetailsTableColumn.DESC] = "TOTAL";
                    TotalRow[(int)SaleDetailsTableColumn.QTY] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    TotalRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(TotalAmount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SaleDetailsTable.Rows.Add(TotalRow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating invoice: " + ex.Message);
                    return;
                }

                if (fileExtension == "Laser")
                {
                    GeneratePDF(SaleDetailsTable, SaleEntry, PrintPaper, "pdf", isPrint, isLandscape, TotalAmount, entrytype);
                }
            }
        }

        //public void GeneratePDF(DataTable dataTable, SaleEntry saleEntry, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape, double TotalAmount)
        public void GeneratePDF(DataTable dataTable, SaleEntry saleEntry, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape, double TotalAmount, Entrytype entrytype)

        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                // Set page size based on orientation
                var pageSize = isLandscape ? PageSize.A5.Rotate() : PageSize.A5;

                Document pdfDoc = new Document(pageSize, 20, 10, 15, 40);
                PageNumberHelper pageNumberHelper = new PageNumberHelper { IsLandScape = isLandscape };
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                writer.PageEvent = pageNumberHelper;

                pdfDoc.Open();

                // Add header
                //PdfPTable DocHeader = InvoiceHeader("SALES INVOICE", saleEntry.RefNumber, saleEntry.SaleDate);
                string title = entrytype == Entrytype.QUOTE ? "ESTIMATE" : "SALES INVOICE"; PdfPTable DocHeader = InvoiceHeader(title, saleEntry.RefNumber, saleEntry.SaleDate, entrytype);
                pdfDoc.Add(DocHeader);

                // Add customer info
                PdfPTable CustomerTable = CustomerDetails(saleEntry);
                pdfDoc.Add(CustomerTable);

                // Add space between sections
                pdfDoc.Add(new Paragraph(" "));

                // Create main items table
                PdfPTable table = new PdfPTable(SaleDetailsTableColumnName.Length);

                // Set column widths based on orientation
                float[] widths = isLandscape
                    ? new float[] { 20f, 90f, 20f, 20f, 25f, 30f }
                    : new float[] { 20f, 100f, 20f, 20f, 25f, 30f };
                // { 8f, 40f, 12f, 12f, 20f, 25f };
                table.SetWidths(widths);
                table = CreateSalesMainTableHeader(table, dataTable);

                // Add data rows
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        var temp = dataTable.Rows[i][j].ToString();
                        var font = i == dataTable.Rows.Count - 1 // Last row (total)
                            ? PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")
                            : PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black");

                        string truncated = temp.Length > 30 ? temp.Substring(0, 30) + "..." : temp;

                        PdfPCell rowCell = new PdfPCell(new Phrase(truncated, font));
                        rowCell.NoWrap = true;

                        //PdfPCell rowCell = new PdfPCell(new Phrase(temp, font));

                        // Styling for total row
                        if (i == dataTable.Rows.Count - 1)
                        {
                            rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                        }

                        if (j == 0)
                            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;  // S.No
                        else if (j == 1)
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;   // Product Name
                        else
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;  // Others

                        rowCell.MinimumHeight = 15;
                        rowCell.BorderWidth = 0.5f;
                        table.AddCell(rowCell);
                    }
                }

                pdfDoc.Add(table);

                // Add amount in words
                pdfDoc.Add(new Paragraph(" "));
                PdfPTable AmountInWords = AmtInWordsColumn(TotalAmount);
                pdfDoc.Add(AmountInWords);

                // Add signature line
                pdfDoc.Add(new Paragraph(" "));
                PdfPTable SignatureTable = SignatureColumn();
                pdfDoc.Add(SignatureTable);


                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, "SaleInvoice", fileExtension, isPrint,
                    isLandscape ? PaperTypes.A5_LANDSCAPE : PaperTypes.A5_PORTRAIT);
            }
        }

        private PdfPTable InvoiceHeader(string Heading, string invoiceNo, DateTime invoiceDate, Entrytype entrytype)
        {
            PdfPTable HeadTable = new PdfPTable(1);
            HeadTable.WidthPercentage = 100;

            PdfPCell HeadCell = new PdfPCell(new Phrase(Global.Company.DisplayAs, PdfDataAlignment.GetFont("Font_Bold_Italic_12_Black")));
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Global.Company.Address.FullAddressInSingleLine, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadCell.PaddingTop = 5f;
            HeadTable.AddCell(HeadCell);

            string label = entrytype == Entrytype.QUOTE ? "Quotation No:" : "Invoice No:";
            HeadCell = new PdfPCell(new Phrase($"{label} {invoiceNo}   Date: {invoiceDate.ToString(Global.Company.DateFormat)}",
                PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));

            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        private PdfPTable CustomerDetails(SaleEntry saleEntry)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 20f, 80f };
            table.SetWidths(widths);

            PdfPCell cell = new PdfPCell(new Phrase("Customer:", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            string customerName = saleEntry.AccountsId != null
                ? saleEntry.Account.DisplayAs
                : (!string.IsNullOrEmpty(saleEntry.CustomerName) ? saleEntry.CustomerName : "");
            string customerDetails = customerName;

            if (!string.IsNullOrEmpty(saleEntry.CustomerAddress))
            {
                customerDetails += "\n" + saleEntry.CustomerAddress.Replace("\r", "").Replace("\n", "").Replace(",", ", ");
            }

            cell = new PdfPCell(new Phrase(customerDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            return table;
        }
        private PdfPTable CreateSalesMainTableHeader20072025(PdfPTable table, DataTable dataTable)
        {
            // Create header row
            for (int i = 0; i < SaleDetailsTableColumnName.Length; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase(SaleDetailsTableColumnName[i], PdfDataAlignment.GetFont("Font_Bold_9_Black")));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = new BaseColor(220, 220, 220);
                cell.BorderWidth = 0.5f;
                table.AddCell(cell);
            }
            return table;
        }
        private PdfPTable CreateSalesMainTableHeader(PdfPTable table, DataTable dataTable)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                var temp = dataTable.Columns[j].ToString();
                PdfPCell headerCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                headerCell.BackgroundColor = new BaseColor(220, 220, 220);
                headerCell.MinimumHeight = 18;
                headerCell.HorizontalAlignment = j > 1 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                headerCell.BorderWidth = 0.5f;
                table.AddCell(headerCell);
            }
            return table;
        }

        private PdfPTable AmtInWordsColumn(double totalAmount)
        {
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Amount in words: " + ConvertToINR(totalAmount),
                PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            return table;
        }

        private PdfPTable SignatureColumn()
        {
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;

            // Add empty space for signature
            table.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, MinimumHeight = 30 });

            PdfPCell cell = new PdfPCell(new Phrase("Authorized Signatory",
                PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            cell.Border = Rectangle.TOP_BORDER;
            cell.BorderWidthTop = 0.5f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingTop = 5f;
            table.AddCell(cell);

            return table;
        }

        private static string TruncateString(string input, int maxLength, bool addEllipsis = true)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;

            return addEllipsis
                ? input.Substring(0, maxLength - 3) + "..."
                : input.Substring(0, maxLength);
        }

        // Reuse the ConvertToINR method from the original class
        public static string ConvertToINR(double amount)
        {
            if (amount == 0)
                return "zero rupees only";

            string words = "";

            long rupees = (long)Math.Floor(amount);
            int paise = (int)Math.Round((amount - rupees) * 100);

            if (rupees > 0)
            {
                words += ConvertIntegerToWords(rupees) + " rupee" + (rupees == 1 ? "" : "s");
            }

            if (paise > 0)
            {
                if (rupees > 0)
                    words += " and ";
                words += ConvertIntegerToWords(paise) + " paise" + (paise == 1 ? "" : "");
            }

            return words + " only";
        }

        private static string ConvertIntegerToWords(long number)
        {
            string[] Units = {
                "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
                "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
                "seventeen", "eighteen", "nineteen"
            };

            string[] Tens = {
                "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
            };

            if (number == 0)
                return Units[0];

            if (number < 0)
                return "minus " + ConvertIntegerToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += ConvertIntegerToWords(number / 10000000) + " crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertIntegerToWords(number / 100000) + " lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertIntegerToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertIntegerToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += Units[number];
                else
                {
                    words += Tens[(number / 10)];
                    if ((number % 10) > 0)
                        words += "-" + Units[(number % 10)];
                }
            }

            return words.Trim();
        }
    }
    public class PageNumberHelper : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate template;
        BaseFont bf = null;
        public bool IsLandScape { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException) { }
            catch (IOException) { }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            int pageN = writer.PageNumber;
            string text = "Page " + pageN + " of ";
            float len = bf.GetWidthPoint(text, 8);
            float x = IsLandScape ? document.PageSize.Width - 100 : document.PageSize.Width - 80;
            float y = document.PageSize.GetBottom(30);

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(x, y);
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, x + len, y);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }

}
/*
 *  PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.IsLandScape = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();
*/