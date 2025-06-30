using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.views.utils;
using System.Data;
using fa.views.utils.Sale;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Rectangle = iTextSharp.text.Rectangle;
using VisioForge.MediaFramework.FFMPEGCore.Enums;
using static fa.views.utils.PrinterSetup;
using fa.model.Accounting.Masters;
using fa;
using fa.api.Accounting;
using static fa.views.utils.Sale.SalePrintSaveA5A4;
using fa.api.utils;
using NPOI.SS.Formula.Functions;
using Fa.Utils.utils;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Fa.views.utils.Sale
{
    public class SalePrintSaveA4EinvoiceFormat
    {
        readonly String[] SaleDetailsTableColumnName = new String[]
        {
            "#", "MFRID", "Description of Goods", "UOM", "HSN", "Bat / Exp", "Qty", "Free", "MRP", "Rate", "Dis %", "GST %", "Amount"
        };
        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES)!.RoundOff;

        private static readonly string[] Units = {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
            "seventeen", "eighteen", "nineteen"
        };

        private static readonly string[] Tens = {
            "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };
        public enum SaleDetailsTableColumn
        {
            SNO, MFRID, DESC, UOM, HSN, BATCH, QTY, FREE, MRP, RATE, DISCP, TAXP, TOTALAMOUNT
        }
        public void ExportToFileOrPrint(long SalesId, string PrintPaper, string fileExtension, bool isPrint)
        {
            SalesManager SalesManager = SalesManager.Instance;
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            IList<TaxTable> lTaxTable = new List<TaxTable>();
            if (SaleEntry == null)
            {
                MessageBox.Show("Somthing went wrong, the selected sale is not valid.");
                return;
            }
            List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)Global.Company.SalesTaxAccountMaps;
            DataTable SaleDataTableTotal = new DataTable();
            DataTable SaleDetailsTable = new DataTable();

            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.SNO], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.MFRID], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DESC], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.UOM], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.HSN], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.BATCH], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.QTY], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.FREE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.MRP], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.RATE], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.DISCP], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TAXP], typeof(string));
            SaleDetailsTable.Columns.Add(SaleDetailsTableColumnName[(int)SaleDetailsTableColumn.TOTALAMOUNT], typeof(string));

            if (SaleEntry.SaleDetails.Count != 0)
            {
                double SubTotalForTax = 0;
                double SubTotal = 0;
                float LintTotal = 0;
                float TaxTotal = 0;
                float Discount = 0;
                int count = 0;
                double Qty = 0;
                double TotalQty = 0;
                double TotalFree = 0;
                double Price = 0;
                double TotalAmount = 0;
                double GrossAmount = 0;
                double TotalSaleAmount = 0;
                double TotalCGST = 0;
                double TotalSGST = 0;
                double TotalRoundedSaleAmount = 0;

                try
                {
                    foreach (SaleDetail SaleDetails in SaleEntry.SaleDetails.OrderBy(x => x.Id))
                    {
                        Discount = 0;
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetails.Id);
                        string salesTax = "\n";
                        string subTotal = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity).ToString("F") : (SaleDetails.OverridePrice * SaleDetails.Quantity).ToString("F");
                        SubTotalForTax = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        var overAll = SaleDetails.OverridePrice == 0 ? (SaleDetails.Price * SaleDetails.Quantity) : (SaleDetails.OverridePrice * SaleDetails.Quantity);
                        SubTotal = SubTotal + overAll;

                        foreach (TaxDetail Taxes in lSaleDetail.TaxDetails)
                        {
                            salesTax += Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == (Taxes.CatalogItemSalesTaxMap != null ? Taxes.CatalogItemSalesTaxMap.SalesTaxMapId : Taxes.ItemSalesTaxMap.SalesTaxMapId))!.Name + " @" + Taxes.TaxRate.ToString() + "% ";
                        }

                        if (lSaleDetail.Discounts.Count > 0)
                        {
                            salesTax += "\nDiscount @" + lSaleDetail.Discounts.Sum(X => X.Discount).ToString() + "%";
                            subTotal += "\n(" + Math.Round(lSaleDetail.Discounts.Sum(X => X.DiscountAmount), 2).ToString("F") + ")\n";
                            subTotal = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F") : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)).ToString("F");
                            SubTotalForTax = SaleDetails.OverridePrice == 0 ? ((SaleDetails.Price * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)) : ((SaleDetails.OverridePrice * SaleDetails.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount));
                            Discount = lSaleDetail.Discounts.Sum(X => X.DiscountAmount);
                        }
                        count++;

                        foreach (ItemLevelSaleTaxDetail SalesTax in lSaleDetail.TaxDetails)
                        {
                            TaxTable TaxTable = null!;
                            string taxName = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId))!.Name;

                            TaxTable = lTaxTable.FirstOrDefault(x => x.TaxPer == SalesTax.TaxRate && x.SaleTaxMapId == (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId))!;
                            if (TaxTable == null)
                            {
                                TaxTable = new TaxTable();
                                TaxTable.TaxPer = SalesTax.TaxRate;
                                TaxTable.TaxAmount = SalesTax.Amount;
                                TaxTable.SaleTaxMapId = (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId);
                                TaxTable.SubTotal = SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                            else
                            {
                                lTaxTable.Remove(TaxTable);
                                TaxTable.TaxAmount += SalesTax.Amount;
                                TaxTable.SubTotal += SubTotalForTax;
                                lTaxTable.Add(TaxTable);
                            }
                            if (taxName == "CGST")
                            {
                                TotalCGST += SalesTax.Amount;
                            }
                            else
                            {
                                TotalSGST += SalesTax.Amount;
                            }
                        }
                        LintTotal = LintTotal + SaleDetails.Amount;
                        if (lSaleDetail.TaxDetails.Count > 0)
                        {
                            TaxTotal = TaxTotal + lSaleDetail.TaxDetails.Sum(X => X.Amount);
                        }
                        double TaxAmount = PdfDataAlignment.ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry.SaleTaxType);
                        Qty = SaleDetails.Quantity;
                        Price = SaleDetails.OverridePrice == 0 ? SaleDetails.Price : SaleDetails.OverridePrice;
                        TotalAmount = (Qty * Price) - Discount;

                        DataRow SaleDetailsTableNewRow = SaleDetailsTable.NewRow();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.SNO] = count.ToString();
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.MFRID] = lSaleDetail.Product.ProductFamily?.Name?.ToString()?? "";
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DESC] = SaleDetails.Product?.Name?.ToString()?? "";
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.UOM] = SaleDetails.Uom?.ToString() ?? "";
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.HSN] = SaleDetails.Product?.HSNCode?.ToString() ?? "";
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.BATCH] = (SaleDetails.BatchNo?.ToString() ?? "") + "\n" + (SaleDetails.ExpDate != DateTime.MinValue ? SaleDetails.ExpDate.ToString("MM-yy") : "");
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.QTY] = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.FREE] = SaleDetails.FreeQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.MRP] = SaleDetails.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.RATE] = Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.DISCP] = SaleDetails.Discounts.Sum(x => x.Discount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TAXP] = SaleDetails.TaxDetails.Count > 1 ? (SaleDetails.TaxDetails.First().TaxRate * 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : (SaleDetails.TaxDetails.Count > 0 ? SaleDetails.TaxDetails.First().TaxRate.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        SaleDetailsTableNewRow[(int)SaleDetailsTableColumn.TOTALAMOUNT] = Math.Round(TotalAmount, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        SaleDetailsTable.Rows.Add(SaleDetailsTableNewRow);
                        GrossAmount += TotalAmount;
                        TotalQty += SaleDetails.Quantity;
                        TotalFree += SaleDetails.FreeQuantity;
                    }
                    SaleDataTableTotal.Columns.Add("1", typeof(string));
                    SaleDataTableTotal.Columns.Add("2", typeof(string));
                    SaleDataTableTotal.Columns.Add("3", typeof(string));
                    SaleDataTableTotal.Columns.Add("4", typeof(string));
                    SaleDataTableTotal.Columns.Add("5", typeof(string));
                    SaleDataTableTotal.Columns.Add("6", typeof(string));
                    SaleDataTableTotal.Columns.Add("7", typeof(string));
                    SaleDataTableTotal.Columns.Add("8", typeof(string));
                    SaleDataTableTotal.Columns.Add("9", typeof(string));
                    SaleDataTableTotal.Columns.Add("10", typeof(string));
                    SaleDataTableTotal.Columns.Add("11", typeof(string));
                    SaleDataTableTotal.Columns.Add("12", typeof(string));
                    SaleDataTableTotal.Columns.Add("13", typeof(string));

                    TotalSaleAmount = GrossAmount + double.Parse(TaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                    double RoundoffAmount = Rounds > 0 ? RoundOff(TotalSaleAmount) : 0;

                    SaleDataTableTotal.Rows.Add(new object[] { "   ", "   ", "   ", "    ", "     ", "CGST", "      ", "      ", "      ", "      ", "      ", "     ", TotalCGST.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    SaleDataTableTotal.Rows.Add(new object[] { "   ", "   ", "   ", "    ", "     ", "SGST", "      ", "      ", "      ", "      ", "      ", "     ", TotalSGST.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    SaleDataTableTotal.Rows.Add(new object[] { "   ", "   ", "   ", "    ", "     ", "Round off", "      ", "      ", "      ", "      ", "      ", "     ", RoundoffAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    SaleDataTableTotal.Rows.Add(new object[] { "   ", "   ", "   ", "    ", "     ", "Total", TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)), TotalFree.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)), "      ", "      ", "      ", "      ", "Rs. " + (TotalSaleAmount + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                    TotalRoundedSaleAmount = double.Parse((TotalSaleAmount + RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
                }
                catch (DocumentException dex)
                {
                    MessageBox.Show(dex.ToString());
                }
                catch (IOException ioex)
                {
                    MessageBox.Show(ioex.ToString());
                }
                catch (Exception ee)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ee.ToString());
                }
                if (fileExtension == "Laser")
                {
                    GeneratePDF(lTaxTable, SaleDetailsTable, SaleDataTableTotal, SaleEntry, PrintPaper, "pdf", isPrint, TotalRoundedSaleAmount, LintTotal);
                }
            }
        }

        public void GeneratePDF(IList<PrinterSetup.TaxTable> lTaxTable, DataTable dataTable, DataTable totalTable, SaleEntry saleEntry, string PrintPaper, string fileExtension, bool isPrint, double TotalSaleAmount, float LintTotal)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                var pageSize = PageSize.A4;
                int cols = 13;
                int rows = dataTable.Rows.Count;

                Document pdfDoc = new Document(pageSize, -55, -55, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                PdfPTable DocHeader = InvoiceHeader("TAX INVOICE");
                PdfPTable CompanyAndInvoiceDetail = CompanyAndInvoiceDetails(saleEntry);
                PdfPTable CustomerAndDeliveryDetail = CustomerAndDeliveryDetails(saleEntry);
                pdfDoc.Add(DocHeader);
                pdfDoc.Add(CompanyAndInvoiceDetail);
                pdfDoc.Add(CustomerAndDeliveryDetail);

                PdfPTable table = new PdfPTable(cols);
                float[] widths = new float[] { 8f, 30f, 60f, 18f, 23F, 30f, 15f, 15f, 18f, 18f, 18f, 20f, 32f };
                table.SetWidths(widths);
                table = CreateSalesMainTableHeader(table, dataTable);

                for (int i = 0; i < (rows <= 10 ? 10 : rows); i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        var temp = i < rows ? dataTable.Rows[i][j].ToString() : " ";
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        
                        if (j < 12)
                        {
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        rowCell.HorizontalAlignment = j > 5 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        rowCell.MinimumHeight = 15;
                        rowCell.BorderColorTop = BaseColor.WHITE;
                        rowCell.BorderColorBottom = BaseColor.WHITE;
                        rowCell.BorderWidthTop = (float)BorderStyle.None;
                        rowCell.BorderWidthBottom = (float)BorderStyle.None;
                        table.AddCell(rowCell);
                    }
                }
                int rowstotalTable = totalTable.Rows.Count;
                for (int i = 0; i < rowstotalTable; i++)
                {
                    PdfPCell rowCell = new PdfPCell();
                    for (int j = 0; j < totalTable.Columns.Count; j++)
                    {
                        var temp = totalTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        if (i != 2)
                        {
                            rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                        }
                        if (i == totalTable.Rows.Count - 1)
                        {
                            if (j == 6 || j == 7)
                            {
                                rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            }
                        }
                        if (i == 0 || i == 1)
                        {
                            rowCell.BorderWidthTop = (float)BorderStyle.None;
                        }
                        if (j < 12)
                        {
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                        }
                        if ((j > 0 && j < 6) && i != 0 && i != 1)
                        {
                            rowCell.BorderWidthLeft = (float)BorderStyle.None;
                        }
                        rowCell.BorderWidthBottom = (float)BorderStyle.None;
                        rowCell.MinimumHeight = 15;
                        rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        rowCell.BorderColor = BaseColor.BLACK;
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);

                PdfPTable AmountInWordsColumn = AmtInWordsColumn(TotalSaleAmount);
                pdfDoc.Add(AmountInWordsColumn);

                if (lTaxTable.Count != 0)
                {
                    PdfPTable TaxPdfPTable = TaxTable(lTaxTable, totalTable, saleEntry);
                    pdfDoc.Add(TaxPdfPTable);
                }

                PdfPTable SignatureColumn = QRWithSignatureColumn(saleEntry);
                pdfDoc.Add(SignatureColumn);

                pdfDoc.Close();
                PdfGeneration.SaveMemoryStream(myMemoryStream, "SaleInvoice", fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }

        private PdfPTable AmtInWordsColumn(double totalSaleAmount)
        {
            totalSaleAmount = double.Parse(totalSaleAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)));
            PdfPTable table = new PdfPTable(8);
            float[] widths = new float[] { 13f, 94f, 25F, 30f, 20f, 25f, 25f, 36f };
            table.SetWidths(widths);
            int MinimumHeight = 15;
            PdfPCell rowCell = new PdfPCell();

            rowCell = new PdfPCell(new Phrase("Amount Chargable (in words)", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Colspan = 7;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.UseVariableBorders = true;
            table.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("E & O.E", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.UseVariableBorders = true;
            table.AddCell(rowCell);
            
            rowCell = new PdfPCell(new Phrase(ConvertToINR(totalSaleAmount), PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Colspan = 8;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.UseVariableBorders = true;
            table.AddCell(rowCell);

            return table;

        }

        private PdfPTable TaxTable(IList<TaxTable> lTaxTable, DataTable totalTable, SaleEntry saleEntry)
        {
            int numtax = lTaxTable.Where(x => x.SaleTaxMapId != null).ToList().Count;
            PdfPTable taxPdfTable = new PdfPTable(8);
            float[] widths = new float[] { 13f, 94f, 25F, 25f, 25f, 25f, 25f, 36f };
            taxPdfTable.SetWidths(widths);
            int MinimumHeight = 15;
            PdfPCell rowCell = new PdfPCell();

            rowCell = new PdfPCell(new Phrase("HSN / SAC", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
            rowCell.Rowspan = 2;
            rowCell.Colspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.UseVariableBorders = true;
            taxPdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("Taxable Value", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);
            for (int k = 0; k < 2; k++)
            {
                rowCell = new PdfPCell(new Phrase(k == 0 ? "CGST" : "SGST", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = 2;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.BorderWidthRight = (float)BorderStyle.None;
                rowCell.BorderWidthBottom = (float)BorderStyle.None;
                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                taxPdfTable.AddCell(rowCell);
            }
            rowCell = new PdfPCell(new Phrase("Tax Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BackgroundColor = new BaseColor(200, 200, 200);
            taxPdfTable.AddCell(rowCell);
            
            for (int nt = 0; nt < 2; nt++)
            {
                rowCell = new PdfPCell(new Phrase("%", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                rowCell.BorderWidthRight = (float)BorderStyle.None;
                rowCell.BorderWidthBottom = (float)BorderStyle.None;
                taxPdfTable.AddCell(rowCell);

                rowCell = new PdfPCell(new Phrase("Amount", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.BackgroundColor = new BaseColor(200, 200, 200);
                rowCell.BorderWidthRight = (float)BorderStyle.None;
                rowCell.BorderWidthBottom = (float)BorderStyle.None;
                taxPdfTable.AddCell(rowCell);
            }
            double TotalTaxValue = 0;
            double TotalTaxAmount = 0;
            double TotalGSTAmount = 0;

            if (lTaxTable.Count > 0)
            {
                double taxAmount = 0.00;
                double percentage = 0.00;
                double SubTotalForTax = 0;
                string Batch = "0000000";
                foreach (SaleDetail detail in saleEntry.SaleDetails)
                {
                    SubTotalForTax = 0;
                    SaleDetail lSaleDetail = SalesManager.Instance.GetSaleDetail(detail.Id);
                    List<ItemLevelSaleTaxDetail> SalesTaxs = SalesManager.Instance.GetSaleTaxDetail(detail.Id);
                    SubTotalForTax = detail.OverridePrice == 0 ? ((detail.Price * detail.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount)) : ((detail.OverridePrice * detail.Quantity) - lSaleDetail.Discounts.Sum(X => X.DiscountAmount));
                    lTaxTable = new List<TaxTable>();

                    foreach (var SalesTax in SalesTaxs)
                    {
                        TaxTable TaxTable = new TaxTable();
                        TaxTable.TaxPer = SalesTax.TaxRate;
                        TaxTable.TaxAmount = SalesTax.Amount;
                        TaxTable.SaleTaxMapId = (SalesTax.CatalogItemSalesTaxMap != null ? SalesTax.CatalogItemSalesTaxMap.SalesTaxMapId : SalesTax.ItemSalesTaxMap.SalesTaxMapId);
                        TaxTable.SubTotal = SubTotalForTax;
                        lTaxTable.Add(TaxTable);
                    }
                    foreach (TaxTable llTaxTable in lTaxTable.OrderBy(x => x.SubTotal))
                    {
                        if (llTaxTable.TaxPer != percentage || detail.BatchNo != Batch)
                        {
                            rowCell = new PdfPCell(new Phrase(string.IsNullOrEmpty(detail.Product?.HSNCode) ? "NA" : detail.Product?.HSNCode, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.Colspan = 2;
                            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);

                            rowCell = new PdfPCell(new Phrase(llTaxTable.SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);

                            rowCell = new PdfPCell(new Phrase(llTaxTable.TaxPer.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);

                            rowCell = new PdfPCell(new Phrase(llTaxTable.TaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);
                            taxAmount += llTaxTable.TaxAmount;
                            TotalTaxAmount += llTaxTable.TaxAmount;

                            rowCell = new PdfPCell(new Phrase(lTaxTable.Count > 1 ? llTaxTable.TaxPer.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : "0", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);

                            rowCell = new PdfPCell(new Phrase(lTaxTable.Count > 1 ? llTaxTable.TaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : "0.00", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthRight = (float)BorderStyle.None;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);
                            taxAmount += lTaxTable.Count > 1 ? llTaxTable.TaxAmount : 0;

                            rowCell = new PdfPCell(new Phrase(taxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                            rowCell.MinimumHeight = MinimumHeight;
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            rowCell.BorderWidthBottom = (float)BorderStyle.None;
                            taxPdfTable.AddCell(rowCell);

                            percentage = llTaxTable.TaxPer;
                            TotalTaxValue += llTaxTable.SubTotal;
                            TotalGSTAmount += taxAmount;
                            taxAmount = 0;
                        }
                        Batch = detail.BatchNo;
                    }
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

            rowCell = new PdfPCell(new Phrase("Total", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.Colspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);
            
            rowCell = new PdfPCell(new Phrase(TotalTaxValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(TotalTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);
            
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(lTaxTable.Count > 1 ? TotalTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) : "0.00", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);
            
            rowCell = new PdfPCell(new Phrase(TotalGSTAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)), PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);
            
            rowCell = new PdfPCell(new Phrase("Tax Amount (in words) : " + ConvertToINR(double.Parse(TotalGSTAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)))), PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.MinimumHeight = 15;
            rowCell.Colspan = 8;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            taxPdfTable.AddCell(rowCell);

            return taxPdfTable;
        }

        private PdfPTable QRWithSignatureColumn(SaleEntry SaleEntry)
        {
            int cols = 3;
            int MinimumHeight = 10;
            PdfPTable PdfTable = new PdfPTable(cols);
            float[] widths = new float[] { 25f, 40f, 35f };
            PdfTable.SetWidths(widths);
            PdfPCell rowCell = new PdfPCell();

            bool IsUPI = Global.Company.CompanySalesSetup.IsPrintQRCode;
            bool IsBank = Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails) ? true : false;
            bool IsDeclaration = Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations) ? true : false;

            rowCell = new PdfPCell(new Phrase(IsUPI ? "Scan QR code to pay" : "", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            PdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(IsBank ? Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine) : "", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorRight = BaseColor.WHITE;
            rowCell.Rowspan = 2;
            PdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase(IsDeclaration ? "For " + Global.Company.DisplayAs : "", PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            PdfTable.AddCell(rowCell);

            if (IsUPI)
            {
                string UpiUrl = Global.Company.CompanySalesSetup.UPIId;
                UpiUrl = UpiUrl.Replace("&am=100.00", "&am=" + SaleEntry.NetAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",", ""));
                MemoryStream MStream = new MemoryStream();
                var Image = QRCode.GenerateQRCode(UpiUrl);
                Image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(MStream.ToArray());
                image.ScaleAbsoluteHeight(60);
                image.ScaleAbsoluteWidth(60);

                rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                rowCell.AddElement(image);
                rowCell.PaddingLeft = 10f;
                rowCell.PaddingBottom = 3f;
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.Colspan = 1;
                PdfTable.AddCell(rowCell);
            }
            else
            {
                rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.Colspan = 1;
                PdfTable.AddCell(rowCell);
            }

            rowCell = new PdfPCell(new Phrase(IsDeclaration ? Global.Company.CompanySalesSetup.Declarations.Replace(",", System.Environment.NewLine) : "", PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorLeft = BaseColor.WHITE;
            rowCell.BorderColorTop = BaseColor.WHITE;
            rowCell.Colspan = 1;
            PdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("Declaration :", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = 10;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Colspan = 8;
            rowCell.UseVariableBorders = true;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderColorBottom = BaseColor.WHITE;
            PdfTable.AddCell(rowCell);

            rowCell = new PdfPCell(new Phrase("We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            rowCell.MinimumHeight = MinimumHeight;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.Colspan = 8;
            rowCell.UseVariableBorders = true;
            rowCell.BorderColorTop = BaseColor.WHITE;
            PdfTable.AddCell(rowCell);

            return PdfTable;
        }

        private PdfPTable CustomerAndDeliveryDetails(SaleEntry SaleEntry)
        {
            string CustomerDetail = string.Empty;
            string CustomerName = string.Empty;
            string CustomerLicense = string.Empty;
            string Memo = SaleEntry.Memo;

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
                CustomerName = SaleEntry.Account.DisplayAs;
            }
            else
            {
                CustomerName = !string.IsNullOrEmpty(SaleEntry.CustomerName) ? SaleEntry.CustomerName : "";
            }
            CustomerDetail = (String.IsNullOrEmpty(SaleEntry.CustomerAddress) ? "" : "\n" + SaleEntry.CustomerAddress.Replace("\r", "").Replace("\n", "").Replace(",", ", ")) + (String.IsNullOrEmpty(CustomerLicense) ? "" : "\n" + CustomerLicense);


            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 40f, 35f, 25f };
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            HeadCell = new PdfPCell(new Phrase("Buyer (Bill to)", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 20;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Dispatched Doc No", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.Rowspan = 2;
            HeadCell.MinimumHeight = 20;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Delivery Note Date", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.Rowspan = 2;
            HeadCell.MinimumHeight = 20;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(CustomerName, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 20;
            HeadTable.AddCell(HeadCell);

            var AddressData = CustomerDetail;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.PaddingTop = -10f;
            HeadCell.MinimumHeight = 60;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(Memo, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.Colspan = 2;
            HeadCell.MinimumHeight = 60;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        private PdfPTable CompanyAndInvoiceDetails(SaleEntry SaleEntry)
        {
            string CompanyName = Global.Company.DisplayAs + (string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan) + "\n";
            string Address = Global.Company.Address.FullAddressInSingleLine;
            string Phone = string.Empty;
            string Email = string.Empty;
            string Web = string.Empty;
            string CompanyLicense = string.Empty;
            //Company Contact Info
            if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
            {
                var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                Phone = "\nPhone: " + Global.Company.ContactInfo.Phone.Trim('_') + fax;
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
                        CompanyLicense = (string.IsNullOrEmpty(CompanyLicense) ? CompanyLicense : CompanyLicense + ", \n") + (Licence.DisplayName + ": " + Licence.Value);
                    }
                }
                CompanyLicense = "\n" + CompanyLicense + "\n";
            }

            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 40f, 35f, 25f };
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            HeadCell = new PdfPCell(new Phrase("Consigner :", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Invoice No : " + SaleEntry.RefNumber, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Dated : " + SaleEntry.SaleDate.ToString(Global.Company.DateFormat), PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(CompanyName, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.PaddingTop = 6f;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.Rowspan = 2;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Delivery Note : ", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.Rowspan = 2;
            HeadCell.MinimumHeight = 30;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Mode / Terms of Payment", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);
            
            HeadCell = new PdfPCell(new Phrase(SaleEntry.SaleMethod.ToString(), PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            var AddressData = Address + Phone + Email + Web + CompanyLicense;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.PaddingTop = -2f;
            HeadCell.MinimumHeight = 60;
            HeadCell.Rowspan = 2;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Reference No & Date" + "\n" + (SaleEntry.RefNumber + " & " + SaleEntry.SaleDate.ToString(Global.Company.DateFormat)), PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Other References", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 10;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Destination", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 30;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("Dated : ", PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.BorderWidthBottom = (float)BorderStyle.None;
            HeadCell.MinimumHeight = 30;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

        private PdfPTable InvoiceHeader(string Heading)
        {
            int HeadColumns = 3;
            float[] HeadWidths = new float[] { 35f, 35f, 30f };
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            HeadCell = new PdfPCell(new Phrase(Heading, PdfDataAlignment.GetFont("Font_Bold_Italic_14_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 3;
            HeadCell.PaddingBottom = 5;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }
        private PdfPTable CreateSalesMainTableHeader(PdfPTable table, DataTable dataTable)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                var temp = dataTable.Columns[j].ToString();
                PdfPCell headerCell = new PdfPCell();
                headerCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                headerCell.BorderColor = BaseColor.BLACK;
                headerCell.BackgroundColor = new BaseColor(200, 200, 200);
                headerCell.MinimumHeight = 18;
                if (j < 12)
                {
                    headerCell.BorderWidthRight = (float)BorderStyle.None;
                }
                if (j == 12)
                {
                    headerCell.BorderWidthRight = 0.5f;
                }
                headerCell.HorizontalAlignment = j > 5 ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                table.AddCell(headerCell);
            }
            return table;
        }
        private double RoundOff(double TotalAmount)
        {
            double Round = (Rounds / 2);
            double _roundoff = 0.00;
            if (Round > 0)
            {
                double mod = TotalAmount % (Round * 2);
                if (mod >= Round)
                {
                    _roundoff = (Round * 2) - mod;
                }
                else if (mod == 0)
                {
                    _roundoff = 0;
                }
                else
                {
                    _roundoff = -mod;
                }
            }
            return _roundoff;
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
