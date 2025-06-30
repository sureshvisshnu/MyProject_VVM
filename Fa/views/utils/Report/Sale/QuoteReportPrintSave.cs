using fa;
using fa.views.utils.Common;
using fa.views.utils;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using Fa.reports.sales;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.sales;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.utils;
using fa.reports.sales;
using Rectangle = iTextSharp.text.Rectangle;

namespace Fa.views.utils.Report.Sale
{
    internal class QuoteReportPrintSave
    {
        public bool ExportOrPrintToFile(RptQuoteReport RptQuoteReport, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptQuoteReport != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptQuoteReport);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptQuoteReport, ReportName, fileExtension, isPrint, RptQuoteReport.FromDate.ToString(Global.Company.DateFormat), RptQuoteReport.ToDate.ToString(Global.Company.DateFormat));
                                break;
                            }
                            else
                                break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            return true;
        }
        readonly String[] QuoteReportDataTableColumnByInvoice = new String[]
        {
            "#","Invoice","Date","Customer Details","Cash/Credit","Tax","Net Amount",
        };
        readonly String[] QuoteReportDataTableColumnByCustomer = new String[]
        {
            "#","Invoice","Date","Tax","Discount","Cash Amount","Credit Amount",
        };
        readonly String[] QuoteReportDataTableColumn = new String[]
        {
            "#", "Item Code", "Name", "Quantity", "Free", "Sub Total", "Tax", "Total"
        };
        public DataTable DataGridViewAsDataTable(RptQuoteReport RptQuoteReport)
        {
            if(RptQuoteReport.Type == QouteType.BYINVOICE)
            {
                DataTable QuoteTableByInvoice = new DataTable();
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.SNO], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.INVOICE_NUMBER], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.INVOICE_DATE], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.CUSTOMER_INFO], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TYPE], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TAX], typeof(string));
                QuoteTableByInvoice.Columns.Add(QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.NET], typeof(string));

                DataRow QuoteTableRowByInvoice = null;
                int sn = 1;
                double Total = 0;
                double taxTotal = 0;
                String dateTime = null;
                string CustomerInfo = string.Empty;

                foreach (QuoteReportByInvoiceLineItem LineItem in RptQuoteReport.QuoteReportByInvoiceLineItems)
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                    QuoteTableRowByInvoice = QuoteTableByInvoice.NewRow();
                    QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.SNO]] = sn;
                    QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.INVOICE_NUMBER]] = LineItem.InvoiceNumber;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.INVOICE_DATE]] = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                        CustomerInfo = string.Empty;
                    }
                    if (CustomerInfo != LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                    {
                        QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.CUSTOMER_INFO]] = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        CustomerInfo = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    }
                    QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TYPE]] = LineItem.InvoiceType; 
                    QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TAX]] = LineItem.InvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                    QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.NET]] = Math.Round(LineItem.InvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    sn++;
                    Total += LineItem.InvoiceTax;
                    taxTotal += LineItem.InvoiceAmount;
                    QuoteTableByInvoice.Rows.Add(QuoteTableRowByInvoice);
                }
                QuoteTableRowByInvoice = QuoteTableByInvoice.NewRow();
                QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TYPE]] = "Total";
                QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.TAX]] = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                QuoteTableRowByInvoice[QuoteReportDataTableColumnByInvoice[(int)QuoteReportByInvoiceTableColumn.NET]] = Math.Round(taxTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                QuoteTableByInvoice.Rows.Add(QuoteTableRowByInvoice);
                return QuoteTableByInvoice;
            }
            else if(RptQuoteReport.Type == QouteType.BYCUSTOMER)
            {
                DataTable QuoteTableByCustomer = new DataTable();
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.SNO], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.INVOICE_NUMBER], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.INVOICE_DATE], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.TAX], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.DIS], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT], typeof(string));
                QuoteTableByCustomer.Columns.Add(QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT], typeof(string));

                DataRow QuoteTableRowByCustomer = null;
                int sn = 1;
                double CashTotal = 0;
                double CreditTotal = 0;
                double taxTotal = 0;
                double discountTotal = 0;
                String dateTime = null;
                string CustomerName = string.Empty;

                foreach (QuoteReportByCustomerLineItem LineItem in RptQuoteReport.QuoteReportByCustomerLineItems.OrderBy(x => x.CustomerName))
                {
                    if (CustomerName != LineItem.CustomerName)
                    {
                        QuoteTableRowByCustomer = QuoteTableByCustomer.NewRow();
                        QuoteTableRowByCustomer[QuoteReportDataTableColumn[(int)QuoteReportByCustomerTableColumn.SNO]] = " CustomerName : " + LineItem.CustomerName;
                        QuoteTableByCustomer.Rows.Add(QuoteTableRowByCustomer);
                        CustomerName = LineItem.CustomerName;
                        sn = 0;
                        dateTime = null;
                    }
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.CustomerInvoiceDate, Global.Company.DateFormat);
                    QuoteTableRowByCustomer = QuoteTableByCustomer.NewRow();
                    QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.SNO]] = sn + 1;
                    QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.INVOICE_NUMBER]] = LineItem.CustomerInvoiceNumber;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.INVOICE_DATE]] = LineItem.CustomerInvoiceDate.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                    }
                    QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.TAX]] = LineItem.CustomerInvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                    QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.DIS]] = Math.Round(LineItem.CustomerInvoiceDiscount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                    if (LineItem.InvoiceType == "Credit")
                    {
                        QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT]] = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT]] = Math.Round(LineItem.CustomerInvoiceAmount).ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        CreditTotal += LineItem.CustomerInvoiceAmount;
                    }
                    else
                    {
                        QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT]] = Math.Round(LineItem.CustomerInvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT]] = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                        CashTotal += LineItem.CustomerInvoiceAmount;
                    }
                    taxTotal += LineItem.CustomerInvoiceTax;
                    discountTotal += LineItem.CustomerInvoiceDiscount;
                    sn++;
                    QuoteTableByCustomer.Rows.Add(QuoteTableRowByCustomer);
                }
                QuoteTableRowByCustomer = QuoteTableByCustomer.NewRow();
                QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.INVOICE_DATE]] = "Total";
                QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.TAX]] = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.DIS]] = Math.Round(discountTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT]] = Math.Round(CashTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByCustomer[QuoteReportDataTableColumnByCustomer[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT]] = Math.Round(CreditTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableByCustomer.Rows.Add(QuoteTableRowByCustomer);
                return QuoteTableByCustomer;
            }
            else if(RptQuoteReport.Type == QouteType.BYCATEGORY)
            {
                DataTable QuoteTableByCategory = new DataTable();
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SNO], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.ITEM], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.ITEM_NAME], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.QUANTITY], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.FREE], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SUB_TOTAL], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TAX], typeof(string));
                QuoteTableByCategory.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TOTAL], typeof(string));

                DataRow QuoteTableRowByCategory = null;
                int sn = 0;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                string Category = string.Empty;
                string ProductId = string.Empty;
                string ProductName = string.Empty;

                foreach (QouteReportByCategoryLineItem LineItem in RptQuoteReport._LineItems.OrderBy(x => x.Category).ThenBy(x => x.Item).ThenBy(x => x.ItemName))
                {
                    if (Category != LineItem.Category)
                    {
                        QuoteTableRowByCategory = QuoteTableByCategory.NewRow();
                        QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SNO]] = " Category : " + LineItem.Category;
                        QuoteTableByCategory.Rows.Add(QuoteTableRowByCategory);
                        Category = LineItem.Category;
                        sn = 0;
                    }
                    QuoteTableRowByCategory = QuoteTableByCategory.NewRow();
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SNO]] = sn + 1;
                    if (ProductId != LineItem.Item)
                    {
                        QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.ITEM]] = LineItem.Item;
                        ProductId = LineItem.Item;
                        ProductName = string.Empty;
                    }
                    if (ProductName != LineItem.ItemName)
                    {
                        QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.ITEM_NAME]] = LineItem.ItemName;
                        ProductName = LineItem.ItemName;
                    }
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TAX]] = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                    QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                    sn++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                    QuoteTableByCategory.Rows.Add(QuoteTableRowByCategory);
                }
                QuoteTableRowByCategory = QuoteTableByCategory.NewRow();
                QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.FREE]] = "Total";
                QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByCategory[QuoteReportDataTableColumn[(int)QuoteReportByCategoryTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableByCategory.Rows.Add(QuoteTableRowByCategory);
                return QuoteTableByCategory;
            }
            else if(RptQuoteReport.Type == QouteType.BYPFMAILY)
            {
                DataTable QuoteTableByPFmaily = new DataTable();
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SNO], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.ITEM], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.ITEM_NAME], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.QUANTITY], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.FREE], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SUB_TOTAL], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TAX], typeof(string));
                QuoteTableByPFmaily.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TOTAL], typeof(string));

                DataRow QuoteTableRowByPFmaily = null;
                int Sno = 0;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                string PFamily = string.Empty;
                string ProductId = string.Empty;
                string ProductName = string.Empty;

                foreach (QouteReportByProductFamilyLineItem LineItem in RptQuoteReport._LineItemsPfamily.OrderBy(x => x.ProductFamily))
                {
                    if (PFamily != LineItem.ProductFamily)
                    {
                        QuoteTableRowByPFmaily = QuoteTableByPFmaily.NewRow();
                        QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SNO]] = "Product Family : " + LineItem.ProductFamily;
                        QuoteTableByPFmaily.Rows.Add(QuoteTableRowByPFmaily);
                        PFamily = LineItem.ProductFamily;
                        Sno = 0;
                    }
                    QuoteTableRowByPFmaily = QuoteTableByPFmaily.NewRow();
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SNO]] = Sno + 1;
                    if (ProductId != LineItem.Item)
                    {
                        QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.ITEM]] = LineItem.Item;
                        ProductId = LineItem.Item;
                        ProductName = string.Empty;
                    }
                    if (ProductName != LineItem.ItemName)
                    {
                        QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.ITEM_NAME]] = LineItem.ItemName;
                        ProductName = LineItem.ItemName;
                    }
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TAX]] = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    QuoteTableByPFmaily.Rows.Add(QuoteTableRowByPFmaily);
                    Sno++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                }
                QuoteTableRowByPFmaily = QuoteTableByPFmaily.NewRow();
                QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.FREE]] = "Total";
                QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByPFmaily[QuoteReportDataTableColumn[(int)QuoteReportByPFamilyTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableByPFmaily.Rows.Add(QuoteTableRowByPFmaily);
                return QuoteTableByPFmaily;
            }
            else
            {
                DataTable QuoteTableByItem = new DataTable();
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.SNO], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.ITEM], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.ITEM_NAME], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.QUANTITY], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.FREE], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.SUB_TOTAL], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TAX], typeof(string));
                QuoteTableByItem.Columns.Add(QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TOTAL], typeof(string));

                DataRow QuoteTableRowByItem = null;
                int sn = 1;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                foreach (QouteReportByItemLineItem LineItem in RptQuoteReport._LineItemsItem.OrderBy(x => x.Item))
                {
                    QuoteTableRowByItem = QuoteTableByItem.NewRow();
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.SNO]] = sn;
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.ITEM]] = LineItem.Item;
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.ITEM_NAME]] = LineItem.ItemName;
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.QUANTITY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.FREE]] = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.SUB_TOTAL]] = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TAX]] = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TOTAL]] = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    sn++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                    QuoteTableByItem.Rows.Add(QuoteTableRowByItem);
                }
                QuoteTableRowByItem = QuoteTableByItem.NewRow();
                QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.FREE]] = "Total";
                QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.SUB_TOTAL]] = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TAX]] = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableRowByItem[QuoteReportDataTableColumn[(int)QuoteReportByItemTableColumn.TOTAL]] = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                QuoteTableByItem.Rows.Add(QuoteTableRowByItem);
                return QuoteTableByItem;
            }
        }
        public void GeneratePDF(DataTable DataTable, RptQuoteReport RptQuoteReport, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = "Quotation Report ",
                    ReportLine2 = RptQuoteReport.ReportSubTitle()

                };
                PdfPTable HTable = PdfHeader.PageHeader();
                PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = false,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = false,
                    IsEmail = false,
                    IsWebsite = false,
                    IsLicenceInfo = false,
                    ReportLine1 = "Quotation Report ",
                    ReportLine2 = RptQuoteReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptQuoteReport.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count - 1;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] { 10f, 20f, 25f, 60f, 25f, 30f, 25f };
                if (RptQuoteReport.Type == QouteType.BYCUSTOMER)
                {
                    widths = new float[] { 10f, 25f, 40f, 25f, 25f, 25f, 27f };
                }
                else if (RptQuoteReport.Type == QouteType.BYCATEGORY || RptQuoteReport.Type == QouteType.BYPFMAILY || RptQuoteReport.Type == QouteType.BYITEM)
                {
                    widths = new float[] { 10f, 25f, 50f, 18f, 18f, 18f, 18f, 18f };
                }
                ReportMainTable.SetWidths(widths);
                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                Cursor.Current = Cursors.WaitCursor;
                PdfPCell HeaderCell = new PdfPCell();
                if (RptQuoteReport.Type == QouteType.BYINVOICE || RptQuoteReport.Type == QouteType.BYITEM)
                {
                    for (int i = 0; i < DataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(DataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];
                        for (int j = 0; j < DataTable.Columns.Count; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthTop = (float)BorderStyle.None;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (i == DataTable.Rows.Count - 1 && j > 0 && j < 4)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                if (i == DataTable.Rows.Count - 1)
                                {
                                    RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else
                                {
                                    if (DataTable.Columns[j].ColumnName == "#" || DataTable.Columns[j].ColumnName == "Invoice" || DataTable.Columns[j].ColumnName == "Customer Details" || DataTable.Columns[j].ColumnName == "Cash/Credit"
                                            || DataTable.Columns[j].ColumnName == "Item Code" || DataTable.Columns[j].ColumnName == "Name" || DataTable.Columns[j].ColumnName == "Date")
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    }
                                    else if (DataTable.Columns[j].ColumnName == "Net Amount")
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                    else
                                    {
                                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                }
                            }

                            ReportMainTable.AddCell(RowCell);
                        }

                        k++;
                    }
                }
                else if(RptQuoteReport.Type == QouteType.BYPFMAILY || RptQuoteReport.Type == QouteType.BYCATEGORY)
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(HTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(Cols);
                            ReportMainTable.SetWidths(widths);
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];

                        for (int j = 0; j < Cols; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorTop = BaseColor.WHITE;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            if (j == 3 || j == 4 || j == 5 || j == 6 || j == 7)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][7].ToString()))
                            {
                                continue;
                            }

                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                    for (int i = Rows; i < Rows + 1; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        BaseColor CurRowColor = RowColor[k % 2];
                        for (int j = 0; j < Cols; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();
                            
                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 3)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j > 0 && j < 4)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j == 4)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                }
                else if(RptQuoteReport.Type == QouteType.BYCUSTOMER)
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(HTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(Cols);
                            ReportMainTable.SetWidths(widths);
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];

                        for (int j = 0; j < Cols; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            if (i != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorTop = BaseColor.WHITE;
                            }
                            if (j != 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }

                            if (j == 3 || j == 4 || j == 5 || j == 6)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            if (j == 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                            {
                                RowCell.Colspan = DataTable.Columns.Count;
                            }
                            if (j != 0 && string.IsNullOrEmpty(DataTable.Rows[i][1].ToString()))
                            {
                                continue;
                            }

                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                    for (int i = Rows; i < Rows + 1; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        BaseColor CurRowColor = RowColor[k % 2];
                        for (int j = 0; j < Cols; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();
                            
                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (j > 2)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderColorLeft = BaseColor.WHITE;
                            }
                            if (j > 0 && j < 2)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 0)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j == 2)
                            {
                                RowCell.UseVariableBorders = true;
                                RowCell.BorderWidthLeft = (float)BorderStyle.None;
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                        k++;
                    }
                }
                Cursor.Current = Cursors.Default;
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();
                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                Cursor.Current = Cursors.WaitCursor;
                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptQuoteReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
