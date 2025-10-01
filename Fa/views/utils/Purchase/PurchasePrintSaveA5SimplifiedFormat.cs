using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using fa;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.utils;
using Fa.views.utils.Sale;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;
using Rectangle = iTextSharp.text.Rectangle;

namespace Fa.views.utils.Purchase
{
    public class PurchasePrintSaveA5SimplifiedFormat
    {
        readonly String[] PurchaseDetailsTableColumnName = new String[]
        {
            "S.No",
            "Description",
            "Qty",
            "Unit",  // New column header
            "Rate",
            "Amount"
        };
        public enum PurchaseDetailsTableColumn
        {
            SNO,
            DESC,
            QTY,
            UNIT,  // New column for UOM
            RATE,
            TOTALAMOUNT
        }

        public void ExportToFileOrPrint(long PurchaseId, string PrintPaper, string fileExtension, bool isPrint, Entrytype entrytype, bool isLandscape = false)
        {
            PurchaseEntryManager PurchaseManager = PurchaseEntryManager.Instance;
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseId);

            if (PurchaseEntry == null)
            {
                MessageBox.Show("Something went wrong, the selected sale is not valid.");
                return;
            }

            DataTable PurchaseDetailsTable = new DataTable();

            // Create columns for the simplified table
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.SNO], typeof(string));
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.DESC], typeof(string));
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.QTY], typeof(string));
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.UNIT], typeof(string));
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.RATE], typeof(string));
            PurchaseDetailsTable.Columns.Add(PurchaseDetailsTableColumnName[(int)PurchaseDetailsTableColumn.TOTALAMOUNT], typeof(string));

            if (PurchaseEntry.PurchaseDetails.Count != 0)
            {
                double TotalAmount = 0;
                double TotalQty = 0;
                int count = 0;

                try
                {
                    foreach (PurchaseDetails PurchaseDetails in PurchaseEntry.PurchaseDetails.OrderBy(x => x.Id))
                    {
                        count++;
                        PurchaseDetails lPurchaseDetail = PurchaseEntryManager.Instance.GetPurchaseDetail(PurchaseDetails.Id);

                        double Qty = PurchaseDetails.Quantity;
                        double Price = PurchaseDetails.PurchasePrice;
                        double LineTotal = Qty * Price;


                        DataRow SaleDetailsTableNewRow = PurchaseDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.SNO] = count.ToString();

                        // Truncate product name if too long
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.DESC] = TruncateString(PurchaseDetails.Product?.Name ?? "", 30);
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.QTY] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        // Add UOM
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.UNIT] = PurchaseDetails.WholesaleUOM;
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.RATE] = Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)PurchaseDetailsTableColumn.TOTALAMOUNT] = Math.Round(LineTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        PurchaseDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                        TotalAmount += LineTotal;
                        TotalQty += Qty;
                    }

                    // Add total row
                    DataRow TotalRow = PurchaseDetailsTable.NewRow();
                    TotalRow[(int)PurchaseDetailsTableColumn.DESC] = "TOTAL";
                    TotalRow[(int)PurchaseDetailsTableColumn.QTY] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    TotalRow[(int)PurchaseDetailsTableColumn.TOTALAMOUNT] = Math.Round(TotalAmount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseDetailsTable.Rows.Add(TotalRow);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating invoice: " + ex.Message);
                    return;
                }

                if (fileExtension == "Laser")
                {
                    GeneratePDF(PurchaseDetailsTable, PurchaseEntry, PrintPaper, "pdf", isPrint, isLandscape, TotalAmount, entrytype);
                }
            }
        }
        private static string TruncateString(string input, int maxLength, bool addEllipsis = true)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;

            return addEllipsis
                ? input.Substring(0, maxLength - 3) + "..."
                : input.Substring(0, maxLength);
        }
        public void GeneratePDF(DataTable dataTable, PurchaseEntry purchaseEntry, string PrintPaper, string fileExtension, bool isPrint, bool isLandscape, double TotalAmount, Entrytype entrytype)
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
                string title = entrytype == Entrytype.QUOTE ? "ESTIMATE" : "PURCHASE INVOICE"; PdfPTable DocHeader = InvoiceHeader(title, purchaseEntry.RefNumber, purchaseEntry.PurchaseInvDate, entrytype);
                pdfDoc.Add(DocHeader);

                // Add customer info
                PdfPTable CustomerTable = CustomerDetails(purchaseEntry);
                pdfDoc.Add(CustomerTable);

                // Add space between sections
                pdfDoc.Add(new Paragraph(" "));

                // Create main items table
                PdfPTable table = new PdfPTable(PurchaseDetailsTableColumnName.Length);

                // Set column widths based on orientation
                float[] widths = isLandscape
                    ? new float[] { 20f, 90f, 20f, 20f, 25f, 30f }
                    : new float[] { 18f, 100f, 20f, 25f, 25f, 30f };
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
                PdfGeneration.SaveMemoryStream(myMemoryStream, "PurchaseInvoice", fileExtension, isPrint,
                    isLandscape ? PaperTypes.A5_LANDSCAPE : PaperTypes.A5_PORTRAIT);
            }
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
        private PdfPTable CustomerDetails(PurchaseEntry purchaseEntry)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 20f, 80f };
            table.SetWidths(widths);

            PdfPCell cell = new PdfPCell(new Phrase("Customer:", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            string supplierName = purchaseEntry.AccountId != null
                ? purchaseEntry.Account.DisplayAs
                : (!string.IsNullOrEmpty(purchaseEntry.SupplierName) ? purchaseEntry.SupplierName : "");
            string supplierDetails = supplierName;

            if (!string.IsNullOrEmpty(purchaseEntry.SupplierAddress))
            {
                supplierDetails += "\n" + purchaseEntry.SupplierAddress.Replace("\r", "").Replace("\n", "").Replace(",", ", ");
            }

            cell = new PdfPCell(new Phrase(supplierDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

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
    }
}
