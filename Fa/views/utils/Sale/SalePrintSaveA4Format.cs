using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.views.utils;
using fa;
using fa.api.utils;

namespace Fa.views.utils.Sale
{
    public class SalePrintSaveA4Format
    {
        readonly String[] SaleDetailsTableColumnName = new String[]
        {
            "#", "Description", "Qty", "Rate", "Amount"
        };

        public enum SaleDetailsTableColumn
        {
            SNO, DESC, QTY, RATE, TOTALAMOUNT
        }

        public void ExportToFileOrPrint(long SalesId, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape = false)
        {
            try
            {
                if (Global.Company == null)
                {
                    MessageBox.Show("Company information is not available.");
                    return;
                }

                SalesManager SalesManager = SalesManager.Instance;
                SaleEntry SaleEntry = SalesManager?.GetSaleEntry(SalesId);

                if (SaleEntry == null)
                {
                    MessageBox.Show("Something went wrong, the selected sale is not valid.");
                    return;
                }

                // Override the reference number with the specified format
                SaleEntry.RefNumber = "NON 90176509074";

                DataTable SaleDetailsTable = new DataTable();

                // Create columns for the simplified table
                foreach (var columnName in SaleDetailsTableColumnName)
                {
                    SaleDetailsTable.Columns.Add(columnName, typeof(string));
                }

                if (SaleEntry.SaleDetails?.Count > 0)
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
                            double DiscountAmount = lSaleDetail?.Discounts?.Sum(X => X.DiscountAmount) ?? 0;
                            double LineTotal = (Qty * Price) - DiscountAmount;

                            DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SNO] = count.ToString();

                            // Truncate product name if too long
                            string productName = SaleDetails.Product?.Name ?? string.Empty;
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = TruncateString(productName, 50); // Increased length for A4

                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.QTY] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.RATE] = Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency?.RoundingPrecision ?? 2));
                            SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(LineTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency?.RoundingPrecision ?? 2));

                            SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                            TotalAmount += LineTotal;
                            TotalQty += Qty;
                        }

                        // Add total row
                        DataRow TotalRow = SaleDetailsTable.NewRow();
                        TotalRow[(int)SaleDetailsTableColumn.DESC] = "TOTAL";
                        TotalRow[(int)SaleDetailsTableColumn.QTY] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        TotalRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(TotalAmount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency?.RoundingPrecision ?? 2));
                        SaleDetailsTable.Rows.Add(TotalRow);

                        GeneratePDF(SaleDetailsTable, SaleEntry, PrintPaper, fileExtension, isPrint, isLandscape, TotalAmount);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error generating invoice: " + ex.Message);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("No sale details found for this invoice.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        public void GeneratePDF(DataTable dataTable, SaleEntry saleEntry, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape, double TotalAmount)
        {
            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to generate PDF.");
                    return;
                }

                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    // Set page size to A4
                    var pageSize = isLandscape ? PageSize.A4.Rotate() : PageSize.A4;

                    Document pdfDoc = new Document(pageSize, 20, 20, 30, 20); // Increased margins for A4
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                    try
                    {
                        pdfDoc.Open();

                        // Add header with larger fonts for A4
                        PdfPTable DocHeader = InvoiceHeader("SALES INVOICE", saleEntry.RefNumber, saleEntry.SaleDate);
                        if (DocHeader != null) pdfDoc.Add(DocHeader);

                        // Add customer info
                        PdfPTable CustomerTable = CustomerDetails(saleEntry);
                        if (CustomerTable != null) pdfDoc.Add(CustomerTable);

                        // Add space between sections
                        pdfDoc.Add(new Paragraph(" "));

                        // Create main items table with wider columns for A4
                        PdfPTable table = new PdfPTable(SaleDetailsTableColumnName.Length);

                        // Set column widths for A4
                        float[] widths = isLandscape
                            ? new float[] { 5f, 120f, 25f, 30f, 40f } // Landscape widths
                            : new float[] { 5f, 90f, 20f, 25f, 30f };  // Portrait widths

                        table.SetWidths(widths);
                        table = CreateSalesMainTableHeader(table, dataTable);

                        // Add data rows
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                var temp = dataTable.Rows[i][j].ToString();
                                var font = i == dataTable.Rows.Count - 1 // Last row (total)
                                    ? PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black") // Slightly larger font for A4
                                    : PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"); // Slightly larger font for A4

                                PdfPCell rowCell = new PdfPCell(new Phrase(temp, font));

                                // Styling for total row
                                if (i == dataTable.Rows.Count - 1)
                                {
                                    rowCell.BackgroundColor = new BaseColor(220, 220, 220);
                                }

                                rowCell.HorizontalAlignment = j > 1 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                                rowCell.MinimumHeight = 20; // Increased height for A4
                                rowCell.BorderWidth = 0.5f;
                                table.AddCell(rowCell);
                            }
                        }

                        pdfDoc.Add(table);

                        // Add amount in words
                        pdfDoc.Add(new Paragraph(" "));
                        PdfPTable AmountInWords = AmtInWordsColumn(TotalAmount);
                        if (AmountInWords != null) pdfDoc.Add(AmountInWords);

                        // Add signature line
                        pdfDoc.Add(new Paragraph(" "));
                        PdfPTable SignatureTable = SignatureColumn();
                        if (SignatureTable != null) pdfDoc.Add(SignatureTable);

                        PdfGeneration.SaveMemoryStream(myMemoryStream, "SaleInvoice", fileExtension, isPrint,
                            isLandscape ? PaperTypes.A4_LANDSCAPE : PaperTypes.A4_PORTRAIT);
                    }
                    finally
                    {
                        if (pdfDoc.IsOpen())
                        {
                            pdfDoc.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
        }

        private PdfPTable InvoiceHeader(string Heading, string invoiceNo, DateTime invoiceDate)
        {
            PdfPTable HeadTable = new PdfPTable(1);
            HeadTable.WidthPercentage = 100;

            PdfPCell HeadCell = new PdfPCell(new Phrase(Global.Company.DisplayAs, PdfDataAlignment.GetFont("Font_Bold_Italic_14_Black"))); // Larger font
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Global.Company.Address.FullAddressInSingleLine, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black"))); // Larger font
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_12_Black"))); // Larger font
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadCell.PaddingTop = 10f; // More padding
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase($"Invoice No: {invoiceNo}   Date: {invoiceDate.ToString(Global.Company.DateFormat)}",
                PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black"))); // Larger font
            HeadCell.Border = Rectangle.NO_BORDER;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        private PdfPTable CustomerDetails(SaleEntry saleEntry)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 15f, 85f }; // Adjusted for A4
            table.SetWidths(widths);

            PdfPCell cell = new PdfPCell(new Phrase("Customer:", PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"))); // Larger font
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

            cell = new PdfPCell(new Phrase(customerDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black"))); // Larger font
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            return table;
        }

        private PdfPTable CreateSalesMainTableHeader(PdfPTable table, DataTable dataTable)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                var temp = dataTable.Columns[j].ToString();
                PdfPCell headerCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"))); // Larger font
                headerCell.BackgroundColor = new BaseColor(220, 220, 220);
                headerCell.MinimumHeight = 22; // Increased height
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
                PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black"))); // Larger font
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
            table.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER, MinimumHeight = 40 }); // More space

            PdfPCell cell = new PdfPCell(new Phrase("Authorized Signatory",
                PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black"))); // Larger font
            cell.Border = Rectangle.TOP_BORDER;
            cell.BorderWidthTop = 0.5f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.PaddingTop = 10f; // More padding
            table.AddCell(cell);

            return table;
        }

        private static string TruncateString(string input, int maxLength, bool addEllipsis = true)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            if (input.Length <= maxLength) return input;

            return addEllipsis
                ? input.Substring(0, maxLength - 3) + "..."
                : input.Substring(0, maxLength);
        }

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
}