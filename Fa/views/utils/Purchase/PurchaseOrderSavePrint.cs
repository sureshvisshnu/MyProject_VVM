using fa;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.model.Purchase;
using Fa.reports.Purchase;
using Fa.reports.sales;
using FADataAccessLibrary.report.Purchase;
using FADataAccessLibrary.report.sales;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisioForge.Libs.DirectShowLib;
using static FADataAccessLibrary.report.Purchase.PurchaseOrderReport;

namespace Fa.views.utils.Purchase
{
    class PurchaseOrderSavePrint
    {
        public PurchaseOrderSavePrint()
        {          
        }
        public string fileName = string.Empty;

        public bool ExportOrPrintToFile(PurchaseOrderReport PurchaseOrderReport, string ReportName, string fileExtension, bool isPrint, DateTime fromdate, DateTime todate, string Type)
        {
            if (PurchaseOrderReport != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(PurchaseOrderReport);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, PurchaseOrderReport, ReportName, fileExtension, isPrint, fromdate.ToString(Global.Company.DateFormat), todate.ToString(Global.Company.DateFormat), Type);
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

        readonly String[] purchaseOrderTableByInvoiceVendor = new String[]
        {
            "#","Invoice","Date","Customer Details","Cash/Credit","Tax","Net Amount",
        };
        readonly String[] purchaseOrderTable = new String[]
        {
            "#","Item Code","Item Name","UOM","Quantity","Price","Amount",
        };
        readonly String[] purchaseOrderTableByVendor = new String[]
        {
            "#","Invoice","Date","Address","Cash/Credit","Quantity","Cash Amount","Credit Amount",
        };

        public DataTable DataGridViewAsDataTable(PurchaseOrderReport purchaseOrderReport)
        {
            if (purchaseOrderReport.Type == PurchaseOrderType.BYINVOICE)
            {
                DataTable PurchaseOrderTableByInvoiceVendor = new DataTable();
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.SNO], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.INVOICE_NUMBER], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.INVOICE_DATE], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.CUSTOMER_INFO], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.CREDIT_CASH], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.QTY], typeof(string));
                PurchaseOrderTableByInvoiceVendor.Columns.Add(purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.AMOUNT], typeof(string));

                DataRow PurchaseOrderTableRowByInvoiceVendor = null!;

                int sn = 1;
                double Total = 0;
                double Quantity = 0;
                String dateTime = null!;
                string CustomerInfo = string.Empty;
                foreach (PurchaseOrderReportByInvoiceVendorLineItem LineItem in purchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems)
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                    PurchaseOrderTableRowByInvoiceVendor = PurchaseOrderTableByInvoiceVendor.NewRow();
                    PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.SNO]] = sn;
                    PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.INVOICE_NUMBER]] = LineItem.InvoiceNumber;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.INVOICE_DATE]] = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                        CustomerInfo = string.Empty;
                    }
                    if (CustomerInfo != LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                    {
                        PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.CUSTOMER_INFO]] = LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        CustomerInfo = LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    }
                    PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.CREDIT_CASH]] = LineItem.CreditOrCash;
                    PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.AMOUNT]] = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTableByInvoiceVendor.Rows.Add(PurchaseOrderTableRowByInvoiceVendor);
                    sn++;
                    Total += LineItem.Amount;
                    Quantity += LineItem.Qty;
                }
                PurchaseOrderTableRowByInvoiceVendor = PurchaseOrderTableByInvoiceVendor.NewRow();
                PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.CREDIT_CASH]] = "Total";
                PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.QTY]] = Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableRowByInvoiceVendor[purchaseOrderTableByInvoiceVendor[(int)PurchaseOrderReportByInvoice.AMOUNT]] = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTableByInvoiceVendor.Rows.Add(PurchaseOrderTableRowByInvoiceVendor);
                
                return PurchaseOrderTableByInvoiceVendor;
            }
            else if (purchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY || purchaseOrderReport.Type == PurchaseOrderType.BYPFMAILY)
            {
                DataTable PurchaseOrderTable = new DataTable();
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.SNO], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_CODE], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_NAME], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.PRICE], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT], typeof(string));

                DataRow PurchaseOrderTableRow = null!;

                int sn = 1;
                string Category = string.Empty;
                double SubTotal = 0;
                double Total = 0;
                double SubQty = 0;
                double TotalQty = 0;
                bool SubTotalfalg = false;
                foreach (PurchaseOrderReportLineItem LineItem in purchaseOrderReport.PurchaseOrderReportLineItems.OrderBy(x => x.CatName).ThenBy(x => x.ItemCode))
                {
                    if (Category == string.Empty || Category != LineItem.CatName)
                    {
                        if (SubTotalfalg)
                        {
                            PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                            PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = "Sub Total";
                            PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = SubQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                            SubTotal = 0;
                            SubQty = 0;
                        }
                        PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                        PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.SNO]] = purchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY ? (" Category : " + LineItem.CatName) : (" Product Family : " + LineItem.CatName);
                        PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                        Category = LineItem.CatName;
                        SubTotalfalg = true;
                        sn = 1;
                    }
                    PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.SNO]] = sn;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_CODE]] = LineItem.ItemCode;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_NAME]] = LineItem.ItemName;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = LineItem.UOM.ToString();
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.PRICE]] = LineItem.Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                    SubTotal += LineItem.Amount;
                    Total += LineItem.Amount;
                    SubQty += LineItem.Qty;
                    TotalQty += LineItem.Qty;
                    sn++;
                }
                PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = "Sub Total";
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = SubQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);

                PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = "Total";
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                
                return PurchaseOrderTable;
            }
            else if (purchaseOrderReport.Type == PurchaseOrderType.BYVENDOR)
            {
                DataTable PurchaseOrderTableByVendor = new DataTable();
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.SNO], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.INVOICE_NUMBER], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.INVOICE_DATE], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.VENDOR_ADDRESS], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_CASH], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.QTY], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CASH_AMOUNT], typeof(string));
                PurchaseOrderTableByVendor.Columns.Add(purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT], typeof(string));

                DataRow PurchaseOrderTableByVendorRow = null!;

                int sn = 1;
                double SubCashTotal = 0;
                double SubCreditTotal = 0;
                double SubQuantity = 0;
                double CashTotal = 0;
                double CreditTotal = 0;
                double Quantity = 0;
                string dateTime = null!;
                string CustomerInfo = string.Empty;
                string Vendors = string.Empty;
                bool SubTotalflag = false;
                
                foreach (PurchaseOrderReportByInvoiceVendorLineItem LineItem in purchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems.OrderBy(x => x.SupplierName))
                {
                    if (Vendors == string.Empty || Vendors != LineItem.SupplierName)
                    {
                        if (SubTotalflag)
                        {
                            PurchaseOrderTableByVendorRow = PurchaseOrderTableByVendor.NewRow();

                            PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_CASH]] = "Sub Total";
                            PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.QTY]] = SubQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CASH_AMOUNT]] = SubCashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT]] = SubCreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            PurchaseOrderTableByVendor.Rows.Add(PurchaseOrderTableByVendorRow);
                            SubQuantity = 0;
                            SubCashTotal = 0;
                            SubCreditTotal = 0;
                        }
                        PurchaseOrderTableByVendorRow = PurchaseOrderTableByVendor.NewRow();
                        PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.SNO]] = " Vendor : " + LineItem.SupplierName;
                        PurchaseOrderTableByVendor.Rows.Add(PurchaseOrderTableByVendorRow);
                        SubTotalflag = true;
                        sn = 1;
                    }
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                    PurchaseOrderTableByVendorRow = PurchaseOrderTableByVendor.NewRow();
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.SNO]] = sn;
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.INVOICE_NUMBER]] = LineItem.InvoiceNumber;
                    if (dateTime == null || dateTime != stringLineItemDate || Vendors != LineItem.SupplierName)
                    {
                        PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.INVOICE_DATE]] = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                        CustomerInfo = string.Empty;
                    }
                    if (CustomerInfo != LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                    {
                        PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.VENDOR_ADDRESS]] = LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        CustomerInfo = LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    }
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_CASH]] = LineItem.CreditOrCash;
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CASH_AMOUNT]] = LineItem.CashAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT]] = LineItem.CreditAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTableByVendor.Rows.Add(PurchaseOrderTableByVendorRow);
                    sn++;
                    SubCashTotal += LineItem.CashAmount;
                    SubCreditTotal += LineItem.CreditAmount;
                    SubQuantity += LineItem.Qty;
                    CashTotal += LineItem.CashAmount;
                    CreditTotal += LineItem.CreditAmount;
                    Quantity += LineItem.Qty;
                    Vendors = LineItem.SupplierName;
                }
                PurchaseOrderTableByVendorRow = PurchaseOrderTableByVendor.NewRow();
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_CASH]] = "Sub Total";
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.QTY]] = SubQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CASH_AMOUNT]] = SubCashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT]] = SubCreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTableByVendor.Rows.Add(PurchaseOrderTableByVendorRow);

                PurchaseOrderTableByVendorRow = PurchaseOrderTableByVendor.NewRow();
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_CASH]] = "Total";
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.QTY]] = Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CASH_AMOUNT]] = CashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTableByVendorRow[purchaseOrderTableByVendor[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT]] = CreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTableByVendor.Rows.Add(PurchaseOrderTableByVendorRow);
                
                return PurchaseOrderTableByVendor;
            }
            else
            {
                DataTable PurchaseOrderTable = new DataTable();
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.SNO], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_CODE], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_NAME], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.PRICE], typeof(string));
                PurchaseOrderTable.Columns.Add(purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT], typeof(string));

                DataRow PurchaseOrderTableRow = null!;

                int i = 1;
                double SubTotal = 0;
                double TotalQty = 0;
                foreach (PurchaseOrderReportLineItem LineItem in purchaseOrderReport.PurchaseOrderReportLineItems.OrderBy(x => x.ItemName))
                {
                    PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.SNO]] = i;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_CODE]] = LineItem.ItemCode;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.ITEM_NAME]] = LineItem.ItemName;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = LineItem.UOM;
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.PRICE]] = LineItem.Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                    i++;
                    SubTotal += LineItem.Amount;
                    TotalQty += LineItem.Qty;
                }
                PurchaseOrderTableRow = PurchaseOrderTable.NewRow();
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.UOM]] = "Sub Total";
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.QTY]] = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                PurchaseOrderTableRow[purchaseOrderTable[(int)PurchaseOrderReportColumns.AMOUNT]] = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                PurchaseOrderTable.Rows.Add(PurchaseOrderTableRow);
                return PurchaseOrderTable;
            }
        }
        private void GeneratePDF(DataTable dataTable, PurchaseOrderReport purchaseOrderReport, string reportName, string fileExtension, bool isPrint, string fromdate, string todate, string Type)
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
                    ReportLine1 = "Purchase Order Report ",
                    ReportLine2 = purchaseOrderReport.ReportSubTitle()

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
                    ReportLine1 = "Purchase Order Report ",
                    ReportLine2 = purchaseOrderReport.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(dataTable, "PurchaseOrder" + Type);
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = dataTable.Columns.Count;
                int Rows = dataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[Cols];
                if (purchaseOrderReport.Type == PurchaseOrderType.BYINVOICE)
                {
                    widths = new float[] { 10f, 15f, 15f, 40f, 15f, 15f, 15f };
                }
                else if (purchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY || purchaseOrderReport.Type == PurchaseOrderType.BYPFMAILY || purchaseOrderReport.Type == PurchaseOrderType.BYITEM)
                {
                    widths = new float[] { 10f, 30f, 40f, 15f, 15f, 15f, 15f };
                }
                else if (purchaseOrderReport.Type == PurchaseOrderType.BYVENDOR)
                {
                    widths = new float[] { 10f, 15f, 15f, 45f, 15f, 15f, 15f, 15f };
                }

                ReportMainTable.SetWidths(widths);

                Cursor.Current = Cursors.WaitCursor;
                PdfPCell HeaderCell = new PdfPCell();
                if (purchaseOrderReport.Type == PurchaseOrderType.BYINVOICE)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(dataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                        }
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            var Temp = dataTable.Rows[i][j].ToString();

                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            if (j != 6)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 4)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (i == Rows - 1)
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                if (j > 0 && j < 5)
                                {
                                    RowCell.UseVariableBorders = true;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                if (Temp == "Total")
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                    }
                }
                else if (purchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY || purchaseOrderReport.Type == PurchaseOrderType.BYPFMAILY)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(dataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                        }
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            var Temp = dataTable.Rows[i][j].ToString();
                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;
                            if (dataTable.Rows[i][3].ToString() == "Total" || dataTable.Rows[i][3].ToString() == "Sub Total")
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                else if (j > 0 && j < 4)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                            if (j != 6)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 3)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j == 3)
                            {
                                if (Temp == "Total" || Temp == "Sub Total")
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                            }
                            if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][6].ToString()))
                            {
                                RowCell.Colspan = dataTable.Columns.Count - 1;
                            }
                            if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][6].ToString()))
                            {
                                if (j == 6)
                                {
                                    RowCell.UseVariableBorders = true;
                                    RowCell.BorderColor = BaseColor.GRAY;
                                    RowCell.BorderWidthRight = 0.4f;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                    }
                }
                else if (purchaseOrderReport.Type == PurchaseOrderType.BYVENDOR)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(dataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                        }
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            var Temp = dataTable.Rows[i][j].ToString();
                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;

                            if (dataTable.Rows[i][4].ToString() == "Total" || dataTable.Rows[i][4].ToString() == "Sub Total")
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                else if (j > 0 && j < 5)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                            if (j != 7)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 4)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j == 4)
                            {
                                if (Temp == "Total" || Temp == "Sub Total")
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                            }
                            if (j == 0 && string.IsNullOrEmpty(dataTable.Rows[i][7].ToString()))
                            {
                                RowCell.Colspan = dataTable.Columns.Count - 1;
                            }
                            if (j != 0 && string.IsNullOrEmpty(dataTable.Rows[i][7].ToString()))
                            {
                                if (j == 7)
                                {
                                    RowCell.UseVariableBorders = true;
                                    RowCell.BorderColor = BaseColor.GRAY;
                                    RowCell.BorderWidthRight = 0.4f;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
                    }
                }
                else if (purchaseOrderReport.Type == PurchaseOrderType.BYITEM)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if (TotalWorkingOnPageH > A4Height)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(dataTable.Columns.Count);
                            ReportMainTable.SetWidths(widths);
                        }
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            var Temp = dataTable.Rows[i][j].ToString();
                            RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            RowCell.UseVariableBorders = true;
                            RowCell.BorderColor = BaseColor.GRAY;
                            RowCell.BorderWidthTop = (float)BorderStyle.None;

                            if (dataTable.Rows[i][3].ToString() == "Sub Total")
                            {
                                RowCell.BackgroundColor = new BaseColor(230, 230, 230);
                                if (j == 0)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                }
                                else if (j > 0 && j < 4)
                                {
                                    RowCell.BorderWidthRight = (float)BorderStyle.None;
                                    RowCell.BorderWidthLeft = (float)BorderStyle.None;
                                }
                            }
                            if (j != 6)
                            {
                                RowCell.BorderWidthRight = (float)BorderStyle.None;
                            }
                            if (j > 3)
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (j == 3)
                            {
                                if (Temp == "Total" || Temp == "Sub Total")
                                {
                                    RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                            }
                            ReportMainTable.AddCell(RowCell);
                        }
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
                PdfGeneration.FileName = purchaseOrderReport.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
