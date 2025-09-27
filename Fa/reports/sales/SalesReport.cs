using fa.api.catalog;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.OrderManagement;
using fa.report;
using fa.report.Inventory;
using fa.report.sales;
using fa.reports.Inventory;
using fa.reports.Purchase;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.controls.text;
using fa.views.utils.Report.Sale;
using Fa.report.Purchase;
using Fa.reports.Hms;
using Fa.reports.sales;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static fa.report.sales.SalesReportByArea;
using static fa.report.sales.SalesReportByProductFamily;
using static fa.report.sales.SalesReportBySerialLineItem;
using static fa.report.sales.SalesReportBySerialLineItem.SalesReportByDate;

namespace fa.reports.sales
{
    enum SalesReportBySerialTableColumn
    {
        SNO, DATE, INVOICE_NUMBER, CUSTOMER_INFO, REFERED, ITEM, QTY, BATCH, EXPDATE, SCHEDULE
    }
    enum SalesReportByTaxTableColumn
    {
        SNO, INVOICE_DATE, INVOICE_NUMBER, CUSTOMER_INFO, GSTN, ZVALUE, FVALUE, FAMOUNT, TVALUE, TAMOUNT, EVALUE, EAMOUNT, TEVALUE, TEAMOUNT, OVALUE, OAMOUNT, CHARGES, TOTAL
    }
    enum SalesReportByInvoiceTableColumn
    { 
        SNO, INVOICE_NUMBER, INVOICE_DATE, CUSTOMER_INFO, PAYMENTTYPE, TYPE, TAX, NET
    }
    enum SalesReportByUserTableColumn
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, CUSTOMER_INFO, TAX, NET, TYPE, RECEIVED, BALANCE, USER
    }
    enum SalesReportByAreaTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, BATCH_NUMBER, EXP_DATE, SUB_TOTAL, TAX, TOTAL, AREANAME, CUSNAME
    }
    enum SalesReportByCustomerTableColumn
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, TAX, DIS, CASH_AMOUNT, CREDIT_AMOUNT, TYPE, CUSTOMER
    }
    enum SalesReportByCategoryTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, BATCH_NUMBER, EXP_DATE, SUB_TOTAL, TAX, TOTAL, CATAGORY
    }
    enum SalesReportByPFamilyTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, BATCH_NUMBER, EXP_DATE, SUB_TOTAL, TAX, TOTAL, FAMILY_NAME
    }
    enum SalesReportByRefererTableColumn
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, CUSTOMER_INFO, REFERER_INFO, TYPE, NET
    }
    enum SalesReportByDateTableColumn
    {
        SNO, INVOICE_DATE, INVOICE_NUMBER, SALES_VALUE, ACTUAL_COST, PROFIT
    }
    public partial class FormSalesReport : Form
    {
        SalesReportByProductFamily salesReportByProductFamily = null!;
        SalesReportByArea lSalesReportByArea = null!;
        SalesReportByCustomer SalesReportByCustomer1 = null!;
        SalesReportByCategory SalesReportByCategory1 = null!;
        SalesReportByItem SalesReportByItem1 = null!;
        SalesReportByUser SalesReportByUser = null!;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string NoInformationErrorMsg =  "No Information Found..!";
        public FormSalesReport()
        {
            InitializeComponent();
        }
        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            ResetForm();
            if (Global.softwareType == SoftwareType.VVMATRIX)
            {
                if (Global.Company.BusinessType != BuisnessType.Pharmacy)
                {
                    ComboBoxReportType.Items.Remove("By Schedule");
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ErrorMsg.Text = "";
            if (ComboBoxReportType.Text == "By Invoice")
            {
                ResetForm();
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                labelpayment.Visible = false;
                LabelType.Visible = false;
                CheckedComboBoxItem.Visible = false;
                GridViewForSoldBy.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForInvoice.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
                toolStripSeparator5.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Category")
            {
                ResetForm();
                ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = true;
                labelpayment.Visible = false;
                LabelType.Visible = true;
                LabelType.Text = "Category";
                CheckedComboBoxItem.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForCategory.Visible = true;
                ComboBoxCategory.Width = 200;
                ComboBoxCategory.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Product Family")
            {
                ResetForm();
                ComboUtils.InitializeAllPFamilyCombo(ComboBoxProductFamily, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                ComboBoxProductFamily.Width = 200;
                ComboBoxProductFamily.Visible = true;
                GridViewForPFamily.Visible = true;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                toolStripSeparator5.Visible = true;
                labelpayment.Visible = false;
                LabelType.Visible = true;
                LabelType.Text = "Product Family";
                CheckedComboBoxItem.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCustomer.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Customer")
            {
                ResetForm();
                ComboUtils.InitializeCustomerComboForReport(ComboBoxCustomer, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                labelpayment.Visible = false;
                toolStripSeparator5.Visible = true;
                LabelType.Visible = true;
                LabelType.Text = "Customer";
                CheckedComboBoxItem.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForCustomer.Visible = true;
                ComboBoxCustomer.Width = 200;
                ComboBoxCustomer.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Item")
            {
                ResetForm();
                List<Product> lProduct = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                if (lProduct != null && lProduct.Count > 0)
                {
                    if (lProduct.Count > 1)
                    {
                        CheckedComboBoxItem.Add("All", 0);
                    }
                    foreach (Product p in lProduct.ToList())
                    {
                        CheckedComboBoxItem.Add(p.Name + " " + "(" + p.MaterialId + ")", p.Id);
                    }
                }
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = true;
                labelpayment.Visible = false;
                LabelType.Visible = true;
                LabelType.Text = "Item";
                CheckedComboBoxItem.Width = 200;
                CheckedComboBoxItem.Visible = true;
                ComboBoxReferer.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForCategory.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Referer")
            {
                ResetForm();
                ComboUtils.InitializeReferedComboBox(ComboBoxReferer, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                GridViewForTax.Visible = false;
                labelpayment.Visible = false;
                toolStripSeparator5.Visible = true;
                LabelType.Visible = true;
                LabelType.Text = "Referer";
                CheckedComboBoxItem.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForReferer.Visible = true;
                GridViewForSoldBy.Visible = false;
                ComboBoxReferer.Width = 200;
                ComboBoxReferer.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Sold")
            {
                ResetForm();
                ComboUtils.InitializeReferedComboBox(ComboBoxReferer, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                labelpayment.Visible = false;
                toolStripSeparator5.Visible = true;
                LabelType.Visible = true;
                LabelType.Text = "Sold By";
                CheckedComboBoxItem.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSoldBy.Visible = true;
                GridViewForMargin.Visible = false;
                ComboBoxReferer.Width = 200;
                ComboBoxReferer.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Tax")
            {
                ResetForm();
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = false;
                labelpayment.Visible = false;
                LabelType.Visible = false;
                CheckedComboBoxItem.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForTax.Visible = true;
                GridViewForSoldBy.Visible = false;
                ComboBoxReferer.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Schedule")
            {
                ResetForm();
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = false;
                labelpayment.Visible = false;
                LabelType.Visible = false;
                CheckedComboBoxItem.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForSeries.Visible = true;
                GridViewForSoldBy.Visible = false;
                ComboBoxReferer.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Margin")
            {
                ResetForm();
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = false;
                labelpayment.Visible = false;
                LabelType.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForSoldBy.Visible = false;
                CheckedComboBoxItem.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForMargin.Visible = true;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By Area")
            {
                ResetForm();
                ComboUtils.InitializeAreaComboForReport(CheckedTreeComboBoxArea, Global.Company.CompanyId);
                ComboUtils.InitializeAllPFamilyCombo(ComboBoxProductFamily, Global.Company.CompanyId);
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                toolStripSeparator5.Visible = true;
                LabelType.Visible = true;
                LabelType.Text = "Area";
                CheckedTreeComboBoxArea.Width = 150;
                CheckedTreeComboBoxArea.Visible = true;
                CheckedComboBoxItem.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCustomer.Visible = false;
                labelpayment.Margin = new Padding(5, 0, 0, 0);
                labelpayment.Visible = true;
                labelpayment.Text = "Product Family";
                ComboBoxProductFamily.Margin = new Padding(3, 0, 0, 0);
                ComboBoxProductFamily.Width = 140;
                ComboBoxProductFamily.Visible = true;
                GridViewForPFamily.Visible = false;
                
                GridViewForArea.Visible = true;
                CheckedComboBoxUser.Visible = false;
                SalesReportUserGrid.Visible = false;
                ComboBoxPaymentType.Visible = false;
            }
            else if (ComboBoxReportType.Text == "By User")
            {
                ResetForm();
                SalesReportUserGrid.Rows.Clear();
                ComboUtils.InitializeConsultantTypeCombo(CheckedComboBoxUser, Global.Company.CompanyId);
                GridViewForArea.Visible = false;
                CheckedTreeComboBoxArea.Visible = false;
                GridViewForSeries.Visible = false;
                GridViewForTax.Visible = false;
                GridViewForReferer.Visible = false;
                GridViewForSerial.Visible = false;
                GridViewForInvoice.Visible = false;
                GridViewForCustomer.Visible = false;
                ComboBoxCustomer.Visible = false;
                toolStripSeparator5.Visible = true;
                LabelType.Visible = true;
                LabelType.Text = "User";
                CheckedComboBoxItem.Visible = false;
                ComboBoxReferer.Visible = false;
                GridViewForSoldBy.Visible = false;
                GridViewForMargin.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                CheckedComboBoxUser.Width = 150;
                CheckedComboBoxUser.Visible = true;
                SalesReportUserGrid.Visible = true;
                labelpayment.Margin = new Padding(5, 0, 0, 0);
                labelpayment.Visible = true;
                labelpayment.Text = "Payment Type";
                ComboBoxProductFamily.Margin = new Padding(3, 0, 0, 0);
                ComboBoxPaymentType.Width = 150;
                ComboBoxPaymentType.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                Cursor.Current = Cursors.WaitCursor;
                if (ComboBoxReportType.Text == "By Series")
                {
                    generateSalesReportBySerial();
                }
                else if (ComboBoxReportType.Text == "By Invoice")
                {
                    generateSalesReportByInvoice();
                }
                else if (ComboBoxReportType.Text == "By Area")
                {
                    generateSalesReportByArea();
                }
                else if (ComboBoxReportType.Text == "By Customer")
                {
                    generateSalesReportByCustomer();
                }
                else if (ComboBoxReportType.Text == "By Item")
                {
                    generateSalesReportByItem();
                    this.ActiveControl = ComboBoxReportType.Control;
                    ComboBoxReportType.Select();
                }
                else if (ComboBoxReportType.Text == "By Category")
                {
                    generateSalesReportByCategory();
                }
                else if (ComboBoxReportType.Text == "By Product Family")
                {
                    generateSalesReportByProductFamily();
                }
                else if (ComboBoxReportType.Text == "By Schedule")
                {
                    generateSalesReportBySerial();
                }
                else if (ComboBoxReportType.Text == "By Referer")
                {
                    generateSalesReportByReferer();
                }
                else if (ComboBoxReportType.Text == "By Sold")
                {
                    generateSalesReportBySold();
                }
                else if (ComboBoxReportType.Text == "By Tax")
                {
                    generateSalesReportByTax();
                }
                else if (ComboBoxReportType.Text == "By Margin")
                {
                    generateSalesReportByMargin();
                }
                else if (ComboBoxReportType.Text == "By User")
                {
                    generateSalesReportByUser();
                }
                Cursor.Current = Cursors.Default;
            }
        }
        public static List<string> SelectedNameNode(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            List<string> Names = new List<string>();
            if (ComboTreeBox.CheckedNodes != null && ComboTreeBox.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboTreeBox.CheckedNodes)
                {
                    if (node.Name == "All")
                    {
                        Names.Add(node.Text);
                    }
                    break;
                }
            }
            return Names;
        }
        //sales report by customer
        private void generateSalesReportByCustomer()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCustomer.Rows.Clear();
            if (ComboBoxCustomer.Text != null)
            {
                SalesReportByCustomer1 = new SalesReportByCustomer();
                SalesReportByCustomer1.FromDate = (DateTime)FromDate.Date!;
                SalesReportByCustomer1.ToDate = (DateTime)ToDate.Date!;
                SalesReportByCustomer1.Company = Global.Company;
                SalesReportByCustomer1.entrytype = Entrytype.SALE;
                SalesReportByCustomer1.CustomerIds = CheckedTreeUtils.SelectedNodes(ComboBoxCustomer).ToArray();
                SalesReportByCustomer1.AllType = SelectedNameNode(ComboBoxCustomer).ToArray();
                SalesReportByCustomer1.GenerateReport();
                if (SalesReportByCustomer1.LineItems != null && SalesReportByCustomer1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    //GridViewForCustomer.Rows.Add(SalesReportByCustomer1.LineItems.Count + 1);
                    int rowCount = -1;
                    double CashTotal = 0;
                    double CreditTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    string customer = "";
                    int i = 1;

                    foreach (SalesReportByCustomerLineItem LineItem in SalesReportByCustomer1.LineItems.OrderBy(x => x.Customer))
                    {
                        if (customer == "" || customer != LineItem.Customer)
                        {
                            rowCount = GridViewForCustomer.Rows.Add();
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CUSTOMER].Value = LineItem.Customer;
                            customer = LineItem.Customer;
                            rowCount++;
                            i = 1;
                        }
                        GridViewForCustomer.Rows.Add();
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.SNO].Value = i;
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER].Value = LineItem.CustomerInvoiceNumber;
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_DATE].Value = LineItem.CustomerInvoiceDate.Date.ToString(Global.Company.DateFormat);
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TAX].Value = LineItem.CustomerInvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TYPE].Value = LineItem.InvoiceType;
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.DIS].Value = Math.Round(LineItem.CustomerInvoiceDiscount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        if (LineItem.InvoiceType == "Credit")
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                            CreditTotal += LineItem.CustomerInvoiceAmount;
                        }
                        else
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            CashTotal += LineItem.CustomerInvoiceAmount;
                        }
                        taxTotal += LineItem.CustomerInvoiceTax;
                        discountTotal += LineItem.CustomerInvoiceDiscount;
                        rowCount++;
                        i++;
                    }
                    rowCount = GridViewForCustomer.Rows.Add();
                    GridViewForCustomer.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForCustomer.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_DATE].Value = "Total";
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TAX].Value = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.DIS].Value = Math.Round(discountTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(CashTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(CreditTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = NoInformationErrorMsg;
                }
            }
        }

        //sales report by Invoice
        private void generateSalesReportByInvoice()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            //generate the SalesReportBy Invoice
            GridViewForInvoice.Rows.Clear();
            SalesReportByInvoice SalesReportByInvoice1 = new SalesReportByInvoice();
            SalesReportByInvoice1.FromDate = (DateTime)FromDate.Date!;
            SalesReportByInvoice1.ToDate = (DateTime)ToDate.Date!;
            SalesReportByInvoice1.Company = Global.Company;
            SalesReportByInvoice1.entrytype = Entrytype.SALE;
            SalesReportByInvoice1.GenerateReport();
            if (SalesReportByInvoice1.LineItems != null && SalesReportByInvoice1.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForInvoice.Rows.Add(SalesReportByInvoice1.LineItems.Count + 1);
                int rowCount = 0;
                double Total = 0;
                double taxTotal = 0;
                foreach (SalesReportByInvoiceLineItem LineItem in SalesReportByInvoice1.LineItems)
                {
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.SNO].Value = rowCount + 1;
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(SalesReportByInvoice1.Company.DateFormat);
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    if (LineItem.PaymentType != null)
                    {
                        var paymentTypeValue = (int)LineItem.PaymentType; // since it's stored as long
                        string paymentTypeText = Enum.IsDefined(typeof(PaymentTransactioType), paymentTypeValue)
                            ? ((PaymentTransactioType)paymentTypeValue).ToString()
                            : "Unknown";
                        GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.PAYMENTTYPE].Value = paymentTypeText;
                    }
                    else
                    {
                        GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.PAYMENTTYPE].Value = "Cash"; // default
                    }

                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TYPE].Value = LineItem.InvoiceType;
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.NET].Value = Math.Round(LineItem.InvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TAX].Value = LineItem.InvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Total += LineItem.InvoiceAmount;
                    taxTotal += LineItem.InvoiceTax;
                }
                GridViewForInvoice.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForInvoice.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TYPE].Value = "Total";
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TAX].Value = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.NET].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }
        private void generateSalesReportByUser()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            SalesReportUserGrid.Rows.Clear();
            SalesReportByUser = new SalesReportByUser();
            SalesReportByUser.FromDate = (DateTime)FromDate.Date!;
            SalesReportByUser.ToDate = (DateTime)ToDate.Date!;
            SalesReportByUser.Company = Global.Company;
            SalesReportByUser.entrytype = Entrytype.SALE;
            SalesReportByUser.PaymentType = ComboBoxPaymentType.SelectedIndex == 0 ? "All" : ComboBoxPaymentType.SelectedIndex == 1 ? "BANKTRANSFER" : ComboBoxPaymentType.SelectedIndex == 2 ? "CASH" :
                                            ComboBoxPaymentType.SelectedIndex == 3 ? "CHECK" : ComboBoxPaymentType.SelectedIndex == 4 ? "CREDITCARD" :  "UPI";
            SalesReportByUser.UserId = CheckedTreeUtils.SelectedNodes(CheckedComboBoxUser).ToArray();
            SalesReportByUser.GenerateReport();
            if (SalesReportByUser.LineItems != null && SalesReportByUser.LineItems.Count > 0)
            {
                EnableButtons(true);
                int rowCount = 0;
                int Sn = 0;
                string UserName = string.Empty;
                double SubTaxTotal = 0, SubNetAmountTotal = 0, SubRecevieAmountTotal = 0, SubBalanceAmountTotal = 0;
                double GrandTaxTotal = 0, GrandNetAmountTotal = 0, GrandReceiveAmountTotal = 0, GrandBalanceAmountTotal = 0;

                foreach (SalesReportByUserLineItem LineItem in SalesReportByUser.LineItems.OrderBy(x => x.User))
                {
                    if (UserName != LineItem.User)
                    {
                        if (!string.IsNullOrEmpty(UserName))
                        {
                            SalesReportUserGrid.Rows.Add();
                            SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                            SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value = "Sub Total";
                            SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.TAX].Value = SubTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.NET].Value = SubNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.RECEIVED].Value = SubRecevieAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.BALANCE].Value = SubBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            rowCount++;
                            SubTaxTotal = 0; SubNetAmountTotal = 0; SubRecevieAmountTotal = 0; SubBalanceAmountTotal = 0;
                        }
                    }
                    if (UserName != LineItem.User)
                    {
                        string trimmedUser = LineItem.User.Contains("[") ? LineItem.User.Substring(0, LineItem.User.IndexOf("[")).Trim() : LineItem.User;
                        SalesReportUserGrid.Rows.Add();
                        SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.USER].Value = " User Name : " + trimmedUser;
                        UserName = LineItem.User;
                        Sn = 0;
                        rowCount++;
                    }
                    double BalanceAmount = 0;
                    SalesReportUserGrid.Rows.Add();
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.SNO].Value = Sn + 1;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(SalesReportByUser.Company.DateFormat);
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.NET].Value = LineItem.InvoiceAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.TAX].Value = LineItem.InvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.TYPE].Value = LineItem.PaymentType;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.RECEIVED].Value = LineItem.ReceiveAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    BalanceAmount = LineItem.InvoiceAmount - (double)LineItem.ReceiveAmount;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.BALANCE].Value = BalanceAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Sn++;
                    GrandNetAmountTotal += LineItem.InvoiceAmount; GrandTaxTotal += LineItem.InvoiceTax; GrandReceiveAmountTotal += (double)LineItem.ReceiveAmount; GrandBalanceAmountTotal += BalanceAmount;
                    SubNetAmountTotal += LineItem.InvoiceAmount; SubTaxTotal += LineItem.InvoiceTax; SubBalanceAmountTotal += BalanceAmount; SubRecevieAmountTotal += (double)LineItem.ReceiveAmount;
                }
                if (!string.IsNullOrEmpty(UserName))
                {
                    SalesReportUserGrid.Rows.Add();
                    SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                    SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value = "Sub Total";
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.TAX].Value = SubTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.NET].Value = SubNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.RECEIVED].Value = SubRecevieAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.BALANCE].Value = SubBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    SubTaxTotal = 0; SubNetAmountTotal = 0; SubRecevieAmountTotal = 0; SubBalanceAmountTotal = 0;
                }
                SalesReportUserGrid.Rows.Add();
                SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                SalesReportUserGrid.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value = "Grand Total";
                SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.TAX].Value = GrandTaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.NET].Value = GrandNetAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.RECEIVED].Value = GrandReceiveAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                SalesReportUserGrid.Rows[rowCount].Cells[(int)SalesReportByUserTableColumn.BALANCE].Value = GrandBalanceAmountTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }
        //sales report by Area        
        private void generateSalesReportByArea()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForArea.Rows.Clear();

            lSalesReportByArea = new SalesReportByArea();
            lSalesReportByArea.FromDate = (DateTime)FromDate.Date!;
            lSalesReportByArea.ToDate = (DateTime)ToDate.Date!;
            lSalesReportByArea.Company = Global.Company;
            lSalesReportByArea.entrytype = Entrytype.SALE;
            lSalesReportByArea.IsAllArea = CheckedTreeComboBoxArea.Nodes[0].Checked && CheckedTreeComboBoxArea.Nodes[0].Text == "All";
            lSalesReportByArea.IsUnknownAreas = CheckedTreeComboBoxArea.Nodes[0].Checked && CheckedTreeComboBoxArea.Nodes[0].Text == "UnKnown Area";
            bool isAllSelected = lSalesReportByArea.IsAllArea;
            if (isAllSelected)
            {
                lSalesReportByArea.Areas = CheckedTreeComboBoxArea.Nodes
                    .Cast<ComboTreeNode>() 
                    .Select(node => node.Text) 
                    .ToArray(); 
            }
            else
            {
                lSalesReportByArea.Areas = CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxArea).ToArray();
            }
            lSalesReportByArea.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();

            lSalesReportByArea.GenerateReport();

            if (lSalesReportByArea.LineItems != null && lSalesReportByArea.LineItems.Count > 0)
            {
                var filteredLineItems = lSalesReportByArea.LineItems
                    .Where(item => lSalesReportByArea.Areas.Contains(item.City))
                    .OrderBy(x => x.City)
                    .ThenBy(x => x.CustomerName)
                    .ToList();

                if (filteredLineItems.Count == 0)
                {
                    ErrorMsg.Text = "No data found for the selected areas.";
                    return;
                }

                EnableButtons(true);

                int Sno = 0;
                int rowCount = 0;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                double Qty = 0;
                double Free = 0;
                double GTotal = 0;
                double GSubTotal = 0;
                double GTaxTotal = 0;
                double GQty = 0;
                double GFree = 0;

                string? City = null; 

                foreach (SalesReportByAreaLineItem LineItem in filteredLineItems)
                {
                    if (City != LineItem.City)
                    {
                        if (City != null) 
                        {
                            GridViewForArea.Rows.Add();
                            GridViewForArea.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewForArea.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.ITEM_NAME].Value = "Sub Total";
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.QUANTITY].Value = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.FREE].Value = Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            rowCount++;
                            Qty = 0;
                            Free = 0;
                            SubTotal = 0;
                            TaxTotal = 0;
                            Total = 0;
                        }

                        GridViewForArea.Rows.Add();
                        GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.AREANAME].Value = LineItem.City == string.Empty ? "Unknown Area" : LineItem.City;
                        City = LineItem.City; 
                        Sno = 0;
                        rowCount++;
                    }

                    GridViewForArea.Rows.Add();
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.SNO].Value = Sno + 1;
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.ITEM].Value = LineItem.Item;
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(lSalesReportByArea.Company.DateFormat);
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Sno++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                    Qty += LineItem.Qty;
                    Free += LineItem.Free;
                    GTotal += LineItem.Total;
                    GTaxTotal += LineItem.Tax;
                    GSubTotal += LineItem.SubTotal;
                    GQty += LineItem.Qty;
                    GFree += LineItem.Free;
                }

                GridViewForArea.Rows.Add();
                GridViewForArea.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForArea.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.ITEM_NAME].Value = "Sub Total";
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.QUANTITY].Value = Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.FREE].Value = Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                rowCount++;

                GridViewForArea.Rows.Add();
                GridViewForArea.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForArea.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.ITEM_NAME].Value = "Grand Total";
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.SUB_TOTAL].Value = Math.Round(GSubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TAX].Value = Math.Round(GTaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value = Math.Round(GTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.QUANTITY].Value = GQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                GridViewForArea.Rows[rowCount].Cells[(int)SalesReportByAreaTableColumn.FREE].Value = GFree.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }

        //sales report by Tax
        private void generateSalesReportByTax()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForTax.Rows.Clear();
            SalesReportByTax SalesReportByTax = new SalesReportByTax();
            SalesReportByTax.FromDate = (DateTime)FromDate.Date!;
            SalesReportByTax.ToDate = (DateTime)ToDate.Date!;
            SalesReportByTax.Company = Global.Company;
            SalesReportByTax.GenerateReport();
            if (SalesReportByTax.LineItems != null && SalesReportByTax.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForTax.Rows.Add(SalesReportByTax.LineItems.Count + 1);
                int rowCount = 0;
                double Total = 0;
                double Charges = 0;
                double ztaxTotal = 0;
                double ftaxTotal = 0;
                double ttaxTotal = 0;
                double etaxTotal = 0;
                double tetaxTotal = 0;
                double otaxTotal = 0;
                double ztaxValTotal = 0;
                double ftaxValTotal = 0;
                double ttaxValTotal = 0;
                double etaxValTotal = 0;
                double tetaxValTotal = 0;
                double otaxValTotal = 0;
                foreach (SalesReportByTaxLineItem LineItem in SalesReportByTax.LineItems)
                {
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.SNO].Value = rowCount + 1;
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(SalesReportByTax.Company.DateFormat);
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName;
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.ZVALUE].Value = LineItem.ZeroTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.FVALUE].Value = LineItem.FiveTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.FAMOUNT].Value = LineItem.FiveTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TVALUE].Value = LineItem.TwelveTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TAMOUNT].Value = LineItem.TwelveTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.EVALUE].Value = LineItem.EighteenTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.EAMOUNT].Value = LineItem.EighteenTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TEVALUE].Value = LineItem.TwentyeightTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TEAMOUNT].Value = LineItem.TwentyeightTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.OVALUE].Value = LineItem.OtherTaxvalue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.OAMOUNT].Value = LineItem.OtherTaxAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.CHARGES].Value = Math.Round(LineItem.Charges).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Total += LineItem.Total;
                    Charges += LineItem.Charges;
                    ztaxTotal += LineItem.ZeroTaxAmount;
                    ftaxTotal += LineItem.FiveTaxAmount;
                    ttaxTotal += LineItem.TwelveTaxAmount;
                    etaxTotal += LineItem.EighteenTaxAmount;
                    tetaxTotal += LineItem.TwentyeightTaxAmount;
                    otaxTotal += LineItem.OtherTaxAmount;
                    ztaxValTotal += LineItem.ZeroTaxvalue;
                    ftaxValTotal += LineItem.FiveTaxvalue;
                    ttaxValTotal += LineItem.TwelveTaxvalue;
                    etaxValTotal += LineItem.EighteenTaxvalue;
                    tetaxValTotal += LineItem.TwentyeightTaxvalue;
                    otaxValTotal += LineItem.OtherTaxvalue;
                }
                GridViewForTax.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForTax.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.GSTN].Value = "Total";
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.ZVALUE].Value = ztaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.FVALUE].Value = ftaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TVALUE].Value = ttaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.EVALUE].Value = etaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TEVALUE].Value = tetaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.OVALUE].Value = otaxValTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.FAMOUNT].Value = ftaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TAMOUNT].Value = ttaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.EAMOUNT].Value = etaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TEAMOUNT].Value = tetaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.OAMOUNT].Value = otaxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.CHARGES].Value = Math.Round(Charges).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForTax.Rows[rowCount].Cells[(int)SalesReportByTaxTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }
        //sales report by Sold
        private void generateSalesReportBySold()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForSoldBy.Rows.Clear();
            SalesReportBySold SalesReportBySold1 = new SalesReportBySold();
            SalesReportBySold1.FromDate = (DateTime)FromDate.Date!;
            SalesReportBySold1.ToDate = (DateTime)ToDate.Date!;
            SalesReportBySold1.Company = Global.Company;
            if (ComboBoxReferer.Nodes.Count > 0)
            {
                SalesReportBySold1.SoldIds = CheckedTreeUtils.SelectedNodes(ComboBoxReferer).ToArray();
            }
            SalesReportBySold1.GenerateReport();
            if (SalesReportBySold1.LineItems != null && SalesReportBySold1.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForSoldBy.Rows.Add(SalesReportBySold1.LineItems.Count + 1);
                int rowCount = 0;
                double Total = 0;
                foreach (SalesReportByRefererLineItem LineItem in SalesReportBySold1.LineItems)
                {
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.SNO].Value = rowCount + 1;
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(SalesReportBySold1.Company.DateFormat);
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress;
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.TYPE].Value = LineItem.InvoiceType;
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.NET].Value = Math.Round(LineItem.InvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.REFERER_INFO].Value = LineItem.Referer;
                    rowCount++;
                    Total += LineItem.InvoiceAmount;
                }
                GridViewForSoldBy.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForSoldBy.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.TYPE].Value = "Total";
                GridViewForSoldBy.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.NET].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }

        //sales report by Referer
        private void generateSalesReportByReferer()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForReferer.Rows.Clear();
            SalesReportByReferer SalesReportByReferer1 = new SalesReportByReferer();
            SalesReportByReferer1.FromDate = (DateTime)FromDate.Date!;
            SalesReportByReferer1.ToDate = (DateTime)ToDate.Date!;
            SalesReportByReferer1.Company = Global.Company;
            if (ComboBoxReferer.Nodes.Count > 0)
            {
                SalesReportByReferer1.RefererIds = CheckedTreeUtils.SelectedNodes(ComboBoxReferer).ToArray();
            }
            SalesReportByReferer1.GenerateReport();
            if (SalesReportByReferer1.LineItems != null && SalesReportByReferer1.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForReferer.Rows.Add(SalesReportByReferer1.LineItems.Count + 1);
                int rowCount = 0;
                double Total = 0;
                //double taxTotal = 0;
                foreach (SalesReportByRefererLineItem LineItem in SalesReportByReferer1.LineItems)
                {
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.SNO].Value = rowCount + 1;
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(SalesReportByReferer1.Company.DateFormat);
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress;
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.TYPE].Value = LineItem.InvoiceType;
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.NET].Value = Math.Round(LineItem.InvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.REFERER_INFO].Value = LineItem.Referer;
                    rowCount++;
                    Total += LineItem.InvoiceAmount;
                }
                GridViewForReferer.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForReferer.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.TYPE].Value = "Total";
                GridViewForReferer.Rows[rowCount].Cells[(int)SalesReportByRefererTableColumn.NET].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }


        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
        }
        private bool Validation()
        {
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                ErrorMsg.Text = "Please select type";
                ComboBoxReportType.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Category" && CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
            {
                ErrorMsg.Text = "Please select category..";
                ComboBoxCategory.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Product Family" && CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
            {
                ErrorMsg.Text = "Please select product family..";
                ComboBoxProductFamily.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Customer" && CheckedTreeUtils.SelectedNodes(ComboBoxCustomer).Count < 1)
            {
                ErrorMsg.Text = "Please select customer..";
                ComboBoxCustomer.Select();
                return false;
            }
            if ((ComboBoxReportType.Text == "By Referer" || ComboBoxReportType.Text == "By Sold") && CheckedTreeUtils.SelectedNodes(ComboBoxReferer).Count < 1)
            {
                ErrorMsg.Text = "Please select the referer..";
                ComboBoxReferer.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Area" && CheckedTreeUtils.SelectedNameNodes(CheckedTreeComboBoxArea).Count < 1)
            {
                ErrorMsg.Text = "Please select area..";
                CheckedTreeComboBoxArea.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Area" && CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
            {
                ErrorMsg.Text = "Please select product family..";
                ComboBoxProductFamily.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Item" && CheckedTreeUtils.SelectedItems(CheckedComboBoxItem).Count < 1)
            {
                ErrorMsg.Text = "Please select items..";
                CheckedComboBoxItem.Select();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if (ComboBoxReportType.Text == "By User" && CheckedTreeUtils.SelectedNameNodes(CheckedComboBoxUser).Count < 0)
            {
                ErrorMsg.Text = "Please select User..";
                CheckedComboBoxUser.Select();
                return false;
            }
            if(ComboBoxReportType.Text == "By User" && ComboBoxPaymentType.SelectedIndex < 0)
            {
                ErrorMsg.Text = "Please select Payment Type..";
                ComboBoxPaymentType.Select();
                return false;
            }

            return true;
        }

        //sales report by Category
        private void generateSalesReportByCategory()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCategory.Rows.Clear();
            if (ComboBoxCategory.Text != null)
            {
                SalesReportByCategory1 = new SalesReportByCategory();
                SalesReportByCategory1.FromDate = (DateTime)FromDate.Date!;
                SalesReportByCategory1.ToDate = (DateTime)ToDate.Date!;
                SalesReportByCategory1.Company = Global.Company;
                SalesReportByCategory1.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
                SalesReportByCategory1.GenerateReport();
                if (SalesReportByCategory1.LineItems != null && SalesReportByCategory1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    //GridViewForCategory.Rows.Add(SalesReportByCategory1.LineItems.Count + 1);
                    int rowCount = 0;
                    double Total = 0;
                    double SubTotal = 0;
                    double TaxTotal = 0;
                    string catagory = string.Empty;
                    int i = 1;
                    foreach (SalesReportByCategoryLineItem LineItem in SalesReportByCategory1.LineItems.OrderBy(x => x.Catagory))
                    {
                        GridViewForCategory.Rows.Add();
                        if (catagory == string.Empty || catagory != LineItem.Catagory)
                        {
                            GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.CATAGORY].Value = "Catagory : " + LineItem.Catagory;
                            catagory = LineItem.Catagory;
                            rowCount++;
                            GridViewForCategory.Rows.Add();
                            i = 1;
                        }
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SNO].Value = i;
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.ITEM].Value = LineItem.Item;
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(SalesReportByCategory1.Company.DateFormat);
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;
                        i++;
                    }
                    GridViewForCategory.Rows.Add();
                    GridViewForCategory.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForCategory.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.EXP_DATE].Value = "Total";
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = NoInformationErrorMsg;
                }
            }
        }

        //sales report By Product Family
        private void generateSalesReportByProductFamily()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForPFamily.Rows.Clear();
            if (GridViewForPFamily.Text != null)
            {
                salesReportByProductFamily = new SalesReportByProductFamily();
                salesReportByProductFamily.FromDate = (DateTime)FromDate.Date!;
                salesReportByProductFamily.ToDate = (DateTime)ToDate.Date!;
                salesReportByProductFamily.Company = Global.Company;
                salesReportByProductFamily.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();
                salesReportByProductFamily.GenerateReport();
                if (salesReportByProductFamily.LineItems != null && salesReportByProductFamily.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    int Sno = 0;
                    int rowCount = 0;
                    double Total = 0;
                    double SubTotal = 0;
                    double TaxTotal = 0;
                    string PFamily = string.Empty;

                    foreach (SalesReportByProductFamilyLineItem LineItem in salesReportByProductFamily.LineItems.OrderBy(x => x.ProductFamily))
                    {
                        if (PFamily != LineItem.ProductFamily)
                        {
                            GridViewForPFamily.Rows.Add();
                            GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.FAMILY_NAME].Value = "Product Family : " + LineItem.ProductFamily;
                            PFamily = LineItem.ProductFamily;
                            Sno = 0;
                            rowCount++;
                        }
                        GridViewForPFamily.Rows.Add();
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.SNO].Value = Sno + 1;
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.ITEM].Value = LineItem.Item;
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(salesReportByProductFamily.Company.DateFormat);
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                        Sno++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;
                    }
                    GridViewForPFamily.Rows.Add();
                    GridViewForPFamily.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForPFamily.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.EXP_DATE].Value = "Total";
                    GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForPFamily.Rows[rowCount].Cells[(int)SalesReportByPFamilyTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = NoInformationErrorMsg;
                }
            }
        }
        //sales report by Item
        private void generateSalesReportByItem()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCategory.Rows.Clear();
            SalesReportByItem1 = new SalesReportByItem();
            SalesReportByItem1.FromDate = (DateTime)FromDate.Date!;
            SalesReportByItem1.ToDate = (DateTime)ToDate.Date!;
            SalesReportByItem1.Company = Global.Company;
            SalesReportByItem1.ItemIds = CheckedTreeUtils.SelectedItems(CheckedComboBoxItem).ToArray();
            SalesReportByItem1.GenerateReport();
            if (SalesReportByItem1.LineItems != null && SalesReportByItem1.LineItems.Count > 0)
            {
                EnableButtons(true);
                int rowCount = 0;
                int i = 1;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                string ItemName = string.Empty;
                foreach (SalesReportByCategoryLineItem LineItem in SalesReportByItem1.LineItems.OrderBy(x => x.ItemName))
                {
                    GridViewForCategory.Rows.Add();
                    if (ItemName == string.Empty || ItemName != LineItem.ItemName)
                    {
                        i = 1;
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.CATAGORY].Value = "Name : " + LineItem.ItemName;
                        ItemName = LineItem.ItemName;
                        GridViewForCategory.Rows.Add();
                        rowCount++;
                    }
                    if (ItemName == LineItem.ItemName)
                    {
                        GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SNO].Value = i;
                    }
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.ITEM].Value = LineItem.Item;
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(SalesReportByItem1.Company.DateFormat);
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                    i++;
                }
                GridViewForCategory.Rows.Add();
                GridViewForCategory.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForCategory.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.EXP_DATE].Value = "Total";
                GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForCategory.Rows[rowCount].Cells[(int)SalesReportByCategoryTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }
        //sales report by Serial
        private void generateSalesReportBySerial()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForSeries.Rows.Clear();
            SalesReportBySerial SalesReportBySerial1 = new SalesReportBySerial();
            SalesReportBySerial1.FromDate = (DateTime)FromDate.Date!;
            SalesReportBySerial1.ToDate = (DateTime)ToDate.Date!;
            SalesReportBySerial1.Company = Global.Company;
            SalesReportBySerial1.GenerateReport();
            if (SalesReportBySerial1.LineItems != null && SalesReportBySerial1.LineItems.Count > 0)
            {
                EnableButtons(true);
                int i = 1;
                int rowCount = 0;
                string Date = string.Empty;
                string Customer = string.Empty;
                string Refered = string.Empty;
                string InvNum = string.Empty;
                DateTime Temp = ((DateTime)FromDate.Date).AddDays(-1);
                foreach (SalesReportBySerialLineItem LineItem in SalesReportBySerial1.LineItems.OrderBy(x => x.Date).ThenByDescending(x => x.InvoiceNumber))
                {
                    string lcustomer = LineItem.CustomerName.Replace(" ,", ",").Replace(",", "," + System.Environment.NewLine);
                    GridViewForSeries.Rows.Add();

                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.SNO].Value = i;
                    if (InvNum == string.Empty || InvNum != LineItem.InvoiceNumber)
                    {
                        GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                        InvNum = LineItem.InvoiceNumber;
                    }
                    if (Date == string.Empty || Date != LineItem.Date.ToString(Global.Company.DateFormat))
                    {
                        GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                        Date = LineItem.Date.ToString(Global.Company.DateFormat);
                    }

                    if (Customer == string.Empty || Customer != lcustomer)
                    {
                        GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.CUSTOMER_INFO].Value = lcustomer;
                        Customer = lcustomer;
                    }
                    if (Refered == string.Empty || Refered != LineItem.Refered)
                    {
                        GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.REFERED].Value = LineItem.Refered;
                        Refered = LineItem.Refered;
                    }
                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.ITEM].Value = LineItem.Item;
                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.BATCH].Value = LineItem.Batchno;
                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.EXPDATE].Value = LineItem.ExpDate;
                    GridViewForSeries.Rows[rowCount].Cells[(int)SalesReportBySerialTableColumn.SCHEDULE].Value = LineItem.ScheduleName;
                    rowCount++;
                    i++;
                }
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }
        // Sales Report By Date
        private void generateSalesReportByMargin()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForMargin.Rows.Clear();
            SalesReportByDate salesReportByDate = new SalesReportByDate();
            salesReportByDate.FromDate = (DateTime)FromDate.Date!;
            salesReportByDate.ToDate = (DateTime)ToDate.Date!;
            salesReportByDate.Company = Global.Company;
            salesReportByDate.GenerateReport();
            if (salesReportByDate.LineItems != null && salesReportByDate.LineItems.Count > 0)
            {
                EnableButtons(true);
                double GrandTotalforSalesValue = 0.00;
                double GrandTotalforActuaAmount = 0.00;
                double GrandTotalforProfit = 0.00;
                int rowCount = 0;
                int serialNumber = 1;
                double SubTotalforSalesValue = 0.00;
                double SubTotalforActualAmount = 0.00;
                double SubTotalforProfit = 0.00;
                String LastDate = null!;
                foreach (SalesReportByDateLineItem LineItem in salesReportByDate.LineItems)
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                    GridViewForMargin.Rows.Add(1);
                    GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;

                    if (LastDate != stringLineItemDate)
                    {
                        if (LastDate != null)
                        {
                            // Add subtotal row for the previous date
                            GridViewForMargin.Rows.Add(1);
                            GridViewForMargin.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                            GridViewForMargin.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value = "Sub Total";
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SALES_VALUE].Value = SubTotalforSalesValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.ACTUAL_COST].Value = SubTotalforActualAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.PROFIT].Value = SubTotalforProfit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            rowCount++;

                            // Reset subtotals for the new date
                            SubTotalforSalesValue = 0.00;
                            SubTotalforActualAmount = 0.00;
                            SubTotalforProfit = 0.00;
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SNO].Value = serialNumber;
                            GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                        }
                        GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SNO].Value = serialNumber;
                        GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(salesReportByDate.Company.DateFormat);
                        LastDate = stringLineItemDate;

                    }
                    GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SNO].Value = serialNumber;
                    GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SALES_VALUE].Value = LineItem.SalesValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.ACTUAL_COST].Value = LineItem.ActualAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.PROFIT].Value = LineItem.Profit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    serialNumber++;
                    GrandTotalforSalesValue += LineItem.SalesValue;
                    GrandTotalforActuaAmount += LineItem.ActualAmount;
                    GrandTotalforProfit += LineItem.Profit;

                    SubTotalforSalesValue += LineItem.SalesValue;
                    SubTotalforActualAmount += LineItem.ActualAmount;
                    SubTotalforProfit += LineItem.Profit;

                    rowCount++;
                }
                // last row subtotal for the last date
                GridViewForMargin.Rows.Add();
                GridViewForMargin.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForMargin.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value = "Sub Total";
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SALES_VALUE].Value = SubTotalforSalesValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.ACTUAL_COST].Value = SubTotalforActualAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.PROFIT].Value = SubTotalforProfit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                rowCount++;
                // for Grand Toatal
                GridViewForMargin.Rows.Add();
                GridViewForMargin.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                GridViewForMargin.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value = "Grand Total";
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.SALES_VALUE].Value = GrandTotalforSalesValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.ACTUAL_COST].Value = GrandTotalforActuaAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                GridViewForMargin.Rows[rowCount].Cells[(int)SalesReportByDateTableColumn.PROFIT].Value = GrandTotalforProfit.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
            else
            {
                ErrorMsg.Text = NoInformationErrorMsg;
            }
        }

        private void ResetForm()
        {
            GridViewForArea.Rows.Clear();
            GridViewForSeries.Rows.Clear();
            GridViewForSerial.Rows.Clear();
            GridViewForCategory.Rows.Clear();
            GridViewForCustomer.Rows.Clear();
            GridViewForInvoice.Rows.Clear();
            GridViewForReferer.Rows.Clear();
            GridViewForSoldBy.Rows.Clear();
            GridViewForTax.Rows.Clear();
            CheckedComboBoxItem.Reset();
            GridViewForMargin.Rows.Clear();
            ComboBoxCategory.Nodes.Clear();
            ComboBoxCustomer.Nodes.Clear();
            GridViewForPFamily.Rows.Clear();
            ComboBoxProductFamily.Nodes.Clear();
            EnableButtons(false);
            this.Text = "Sales Report ";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
            GridViewForInvoice.Visible = true;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date!).ToString(Global.Company.DateFormat);

            if (ComboBoxReportType.Text == "By Invoice")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForInvoice, "Sales Report", "InvoiceWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Customer")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileCustomer(SalesReportByCustomer1, "Sales Report", "CustomerWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Category")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintCategorywise(SalesReportByCategory1, "Sales Report", "CategoryWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Product Family")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileFamilywise(salesReportByProductFamily, "Sales Report", "ProductFamilyWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Area")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileAreawise(lSalesReportByArea, "Sales Report", "AreaWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Item")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintItemWise(SalesReportByItem1, "Sales Report", "ItemWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Schedule")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForSeries, "Sales Report", "SalesReportBySchedule", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Referer")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForReferer, "Sales Report", "ReferedWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Sold")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForSoldBy, "Sales Report", "SoldWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Tax")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForTax, "Sales Report", "TaxWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Margin")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForMargin, "Sales Report", "MarginWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By User")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintForUserWise(SalesReportByUser, "Sales Report", "UserWiseSalesReport", "pdf", true, lFromDate, lToDate);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date!).ToString(Global.Company.DateFormat);

            if (ComboBoxReportType.Text == "By Invoice")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForInvoice, "Sales Report", "InvoiceWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Customer")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileCustomer(SalesReportByCustomer1, "Sales Report", "CustomerWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Category")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintCategorywise(SalesReportByCategory1, "Sales Report", "CategoryWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Product Family")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileFamilywise(salesReportByProductFamily, "Sales Report", "ProductFamilyWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Area")
            {
                SaleReportPrintSaveA4.ExportOrPrintToFileAreawise(lSalesReportByArea, "Sales Report", "AreaWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Item")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintItemWise(SalesReportByItem1, "Sales Report", "ItemWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Schedule")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForSeries, "Sales Report", "SalesReportBySchedule", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Referer")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForReferer, "Sales Report", "ReferedWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Sold")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForSoldBy, "Sales Report", "SoldWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Tax")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForTax, "Sales Report", "TaxWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Margin")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForMargin, "Sales Report", "MarginWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By User")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrintForUserWise(SalesReportByUser, "Sales Report", "UserWiseSalesReport", "pdf", false, lFromDate, lToDate);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                BtnCancel.Focus();
            }
        }
        private void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxCustomer_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxReferer_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void CheckedTreeComboBoxArea_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void CheckedComboBoxUser_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            string CheckedPFamilyNodes = string.Empty;
            if (ComboBoxCategory.CheckedNodes != null && ComboBoxCategory.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCategory.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All catagories"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
            }
            else if (CheckedTreeComboBoxArea.CheckedNodes != null && CheckedTreeComboBoxArea.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedTreeComboBoxArea.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Area"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text); 
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
                if (ComboBoxProductFamily.Visible)
                {
                    if (ComboBoxProductFamily.CheckedNodes != null && ComboBoxProductFamily.CheckedNodes.Count > 0)
                    {
                        foreach (ComboTreeNode node in ComboBoxProductFamily.CheckedNodes)
                        {
                            if (node.Name == "All") { CheckedPFamilyNodes = "All Product families"; break; }
                            CheckedPFamilyNodes += ((string.IsNullOrEmpty(CheckedPFamilyNodes) ? "" : ", ") + node.Text);
                        }
                        this.Text = "Sales Report " + " @ " + CheckedNodes + " @ " + CheckedPFamilyNodes;
                    }
                }
            }
            else if (CheckedComboBoxUser.CheckedNodes != null && CheckedComboBoxUser.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedComboBoxUser.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All User's"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
            }

            else if (ComboBoxCustomer.CheckedNodes != null && ComboBoxCustomer.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCustomer.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All customers"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxReferer.CheckedNodes != null && ComboBoxReferer.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxReferer.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All referrers"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxProductFamily.CheckedNodes != null && ComboBoxProductFamily.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxProductFamily.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Product families"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Sales Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Sales Report ";
            }
        }
        private void GridViewForPFamily_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.SNO].Value == null
                && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.ITEM || e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.ITEM_NAME || e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.QUANTITY || e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.FREE)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.BATCH_NUMBER)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.TOTAL].Value == null &&
                    GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.ITEM_NAME].Value == null)
            {
                if (e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByPFamilyTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex == GridViewForPFamily.Rows.Count - 1 && e.ColumnIndex == 6)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            }
        }

        private void ComboBoxProductFamily_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void GridViewForPFamily_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.TOTAL].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForPFamily.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForPFamily.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.FAMILY_NAME].Value != null ? GridViewForPFamily.Rows[e.RowIndex].Cells[(int)SalesReportByPFamilyTableColumn.FAMILY_NAME].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewForArea_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForArea.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForArea.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.AREANAME].Value != null ? GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.AREANAME].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewForArea_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value == null)
            {
                if (e.ColumnIndex == (int)SalesReportByAreaTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByAreaTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1 && GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.SNO].Value == null && GridViewForArea.Rows[e.RowIndex].Cells[(int)SalesReportByAreaTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex < 2)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == 2)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForCustomer_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForCustomer.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForCustomer.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.CUSTOMER].Value != null ? GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.CUSTOMER].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private void GridViewForReferer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GridViewForCustomer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value == null && e.RowIndex != GridViewForCustomer.Rows.Count - 1)
            {
                if (e.ColumnIndex == (int)SalesReportByCustomerTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1 && e.RowIndex == GridViewForCustomer.Rows.Count - 1)
            {
                if (e.ColumnIndex == (int)SalesReportByCustomerTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByCustomerTableColumn.INVOICE_NUMBER)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByCustomerTableColumn.INVOICE_DATE)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForInvoice_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)SalesReportByInvoiceTableColumn.INVOICE_NUMBER].Value == null && e.RowIndex == GridViewForInvoice.Rows.Count - 1)
            {
                if (e.ColumnIndex == (int)SalesReportByInvoiceTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByInvoiceTableColumn.CUSTOMER_INFO)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex < 4)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByInvoiceTableColumn.TYPE)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCategory.Rows[e.RowIndex].Cells[(int)SalesReportByCategoryTableColumn.SNO].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForCategory.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForCategory.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCategory.Rows[e.RowIndex].Cells[(int)SalesReportByCategoryTableColumn.CATAGORY].Value != null ? GridViewForCategory.Rows[e.RowIndex].Cells[(int)SalesReportByCategoryTableColumn.CATAGORY].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private void GridViewForCategory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)SalesReportByCategoryTableColumn.SNO].Value == null && e.RowIndex != GridViewForCategory.Rows.Count - 1)
            {
                if (e.ColumnIndex == (int)SalesReportByCategoryTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByCategoryTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex == GridViewForCategory.Rows.Count - 1)
            {
                if (e.ColumnIndex < 6)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == 6)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForReferer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForReferer.Rows[e.RowIndex].Cells[(int)SalesReportByRefererTableColumn.SNO].Value == null && e.RowIndex == GridViewForReferer.Rows.Count - 1)
            {
                if (e.ColumnIndex < (int)SalesReportByRefererTableColumn.TYPE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByRefererTableColumn.TYPE)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForSoldBy_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForSoldBy.Rows[e.RowIndex].Cells[(int)SalesReportByRefererTableColumn.SNO].Value == null && e.RowIndex == GridViewForSoldBy.Rows.Count - 1)
            {
                if (e.ColumnIndex < (int)SalesReportByRefererTableColumn.TYPE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByRefererTableColumn.TYPE)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForTax_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForTax.Rows[e.RowIndex].Cells[(int)SalesReportByTaxTableColumn.SNO].Value == null && e.RowIndex == GridViewForTax.Rows.Count - 1)
            {
                if (e.ColumnIndex < (int)SalesReportByTaxTableColumn.GSTN)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByTaxTableColumn.GSTN)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
        private void GridViewForMargin_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForMargin.Rows[e.RowIndex].Cells[(int)SalesReportByDateTableColumn.SNO].Value == null)
            {
                if (e.ColumnIndex < (int)SalesReportByDateTableColumn.INVOICE_NUMBER && (GridViewForMargin.Rows[e.RowIndex].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value == "Sub Total"
                    || GridViewForMargin.Rows[e.RowIndex].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value == "Grand Total"))
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)SalesReportByDateTableColumn.INVOICE_NUMBER && (GridViewForMargin.Rows[e.RowIndex].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value == "Sub Total"
                    || GridViewForMargin.Rows[e.RowIndex].Cells[(int)SalesReportByDateTableColumn.INVOICE_NUMBER].Value == "Grand Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void CheckedComboBoxItem_ItemCheckedEvent(object sender, ItemCheckEventArgs e)
        {
            if (CheckedComboBoxItem.CheckedItems.Count == 0)
            {
                this.Text = "Sales Report ";
            }
            else
            {
                bool allItemsChecked = CheckedComboBoxItem.CheckedItems.Contains("All");
                if (allItemsChecked)
                {
                    this.Text = "Sales Report @ All Items";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    int count = 0;
                    foreach (var item in CheckedComboBoxItem.CheckedItems)
                    {
                        if (item.ToString() == "All")
                        {
                            sb.Append("All Items");
                            break;
                        }
                        if (count > 0)
                            sb.Append(", ");
                        sb.Append(item.ToString());
                        count++;
                        if (count >= 5)
                        {
                            sb.Append(", ...");
                            break;
                        }
                    }
                    this.Text = "Sales Report @ " + sb.ToString();
                }
            }
        }
        private void ab2ToolStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            this.ActiveControl = ComboBoxReportType.Control;
            ComboBoxReportType.Select();
        }

        private void SalesReportUserGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.SNO].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.SalesReportUserGrid.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.SalesReportUserGrid.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.USER].Value != null ? SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.USER].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void SalesReportUserGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.USER].Value != null)
            {
                if (e.ColumnIndex == (int)SalesReportByUserTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.SNO].Value == null && SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.NET].Value != null)
            {
                if (e.ColumnIndex == (int)SalesReportByUserTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByUserTableColumn.CUSTOMER_INFO)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)SalesReportByUserTableColumn.INVOICE_NUMBER || e.ColumnIndex == (int)SalesReportByUserTableColumn.INVOICE_DATE)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value == "Grand Total" || SalesReportUserGrid.Rows[e.RowIndex].Cells[(int)SalesReportByUserTableColumn.CUSTOMER_INFO].Value == "Sub Total")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }
    }
}
