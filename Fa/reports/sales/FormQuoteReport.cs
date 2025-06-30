using System;
using System.Windows.Forms;
using fa.report.sales;
using fa.api.utils;
using System.Collections.Generic;
using fa.views.controls.text;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.views.controls.ComboTreeView;
using fa.views.controls;
using fa.model.OrderManagement;
using fa.views.utils.Report.Sale;
using fa.model.Common;
using static fa.report.sales.SalesReportBySerialLineItem;
using static fa.report.sales.SalesReportBySerialLineItem.SalesReportByDate;
using static fa.report.sales.SalesReportByProductFamily;
using Fa.reports.Hms;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa;
using FADataAccessLibrary.report.sales;
using FADataAccessLibrary.report.Hms;
using OpenCvSharp.Dnn;
using VisioForge.Libs.MediaFoundation.OPM;
using fa.reports.sales;
using NPOI.SS.Formula.Functions;
using Fa.views.utils.Report.Sale;
using NPOI.SS.UserModel;

namespace Fa.reports.sales
{
    enum QuoteReportByInvoiceTableColumn
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, CUSTOMER_INFO, TYPE, TAX, NET
    }
    enum QuoteReportByCustomerTableColumn
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, TAX, DIS, CASH_AMOUNT, CREDIT_AMOUNT, TYPE, CUSTOMER_NAME
    }
    enum QuoteReportByCategoryTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, SUB_TOTAL, TAX, TOTAL, CATEGORY_NAME
    }
    enum QuoteReportByPFamilyTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, SUB_TOTAL, TAX, TOTAL, FAMILY_NAME
    }
    enum QuoteReportByItemTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, SUB_TOTAL, TAX, TOTAL
    }
    public partial class FormQuoteReport : Form
    {
        RptQuoteReport RptQuoteReport = null;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public FormQuoteReport()
        {
            InitializeComponent();
        }

        private void FormQuoteReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            LoadCombo();
            ResetForm();
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ErrorMsg.Text = "";
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ResetForm();
                GridViewForCategory.Visible = false;
                GridViewForCustomers.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                LabelCategory.Visible = false;
                LabelCustomer.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
                GridViewForInvoice.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForCustomers.Visible = false;
                ComboBoxCustomer.Visible = false;
                LabelCustomer.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
                GridViewForCategory.Visible = true;
                LabelCategory.Visible = true;
                ComboBoxCategory.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ResetForm();
                labelProductFamily.Visible = true;
                ComboBoxProductFamily.Visible = true;
                GridViewForPFamily.Visible = true;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                LabelCategory.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
                GridViewForCustomers.Visible = false;
                LabelCustomer.Visible = false;
                ComboBoxCustomer.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                LabelCategory.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
                GridViewForCustomers.Visible = true;
                LabelCustomer.Visible = true;
                ComboBoxCustomer.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForCustomers.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxCustomer.Visible = false;
                LabelCategory.Visible = false;
                LabelCustomer.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
                GridViewForCategory.Visible = false;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForPFamily.Visible = false;
                GridViewForItem.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
            ComboUtils.InitializeAllPFamilyCombo(ComboBoxProductFamily, Global.Company.CompanyId);
            ComboUtils.InitializeCustomerComboForReport(ComboBoxCustomer, Global.Company.CompanyId);
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
            if (ComboBoxReportType.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
            {
                ErrorMsg.Text = "Please select category..";
                ComboBoxCategory.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
            {
                ErrorMsg.Text = "Please select product family..";
                ComboBoxProductFamily.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 3 && CheckedTreeUtils.SelectedNodes(ComboBoxCustomer).Count < 1)
            {
                ErrorMsg.Text = "Please select customer..";
                ComboBoxCustomer.Select();
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
            return true;
        }
        private void ResetForm()
        {
            GridViewForCategory.Rows.Clear();
            GridViewForCustomers.Rows.Clear();
            GridViewForInvoice.Rows.Clear();
            GridViewForItem.Rows.Clear();
            ComboBoxCategory.Nodes.Clear();
            ComboBoxCustomer.Nodes.Clear();
            GridViewForPFamily.Rows.Clear();
            ComboBoxProductFamily.Nodes.Clear();
            LoadCombo();
            EnableButtons(false);
            this.Text = "Quote Report ";
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

        private void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ComboBoxCustomer_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxProductFamily_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxCategory.CheckedNodes != null && ComboBoxCategory.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCategory.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All catagories"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Quote Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxCustomer.CheckedNodes != null && ComboBoxCustomer.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCustomer.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All customers"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Quote Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxProductFamily.CheckedNodes != null && ComboBoxProductFamily.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxProductFamily.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Product families"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Quote Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Quote Report ";
            }
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewForCategory.Rows.Clear();
                GridViewForCustomers.Rows.Clear();
                GridViewForInvoice.Rows.Clear();
                GridViewForItem.Rows.Clear();
                GridViewForPFamily.Rows.Clear();
                EnableButtons(false);
                if (Validation())
                {
                    LoadQuoteReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnCancel_Click_1(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
            GridViewForInvoice.Visible = true;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
        }
        private void LoadQuoteReport()
        {
            RptQuoteReport = new RptQuoteReport();
            RptQuoteReport.FromDate = (DateTime)FromDate.Date;
            RptQuoteReport.ToDate = (DateTime)ToDate.Date;
            RptQuoteReport.Company = Global.Company;
            RptQuoteReport.Type = ComboBoxReportType.SelectedIndex == 0 ? QouteType.BYINVOICE : ComboBoxReportType.SelectedIndex == 1 ? QouteType.BYCATEGORY : ComboBoxReportType.SelectedIndex == 2 ? QouteType.BYPFMAILY : ComboBoxReportType.SelectedIndex == 3 ? QouteType.BYCUSTOMER : QouteType.BYITEM;
            RptQuoteReport.CustomerIds = CheckedTreeUtils.SelectedNodes(ComboBoxCustomer).ToArray();
            RptQuoteReport.Customer = this.Text;
            RptQuoteReport.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
            RptQuoteReport.Category = this.Text;
            RptQuoteReport.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();
            RptQuoteReport.GenerateReport();

            if (RptQuoteReport.Type == QouteType.BYINVOICE)
            {
                GridViewForInvoice.Rows.Clear();

                if (RptQuoteReport.QuoteReportByInvoiceLineItems != null && RptQuoteReport.QuoteReportByInvoiceLineItems.Count > 0)
                {
                    int sn = 1;
                    double Total = 0;
                    double taxTotal = 0;
                    EnableButtons(true);
                    String dateTime = null;
                    string CustomerInfo = string.Empty;

                    foreach (QuoteReportByInvoiceLineItem LineItem in RptQuoteReport.QuoteReportByInvoiceLineItems)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                        int irow = GridViewForInvoice.Rows.Add();
                        GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.SNO].Value = sn;
                        GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                            CustomerInfo = string.Empty;
                        }
                        if (CustomerInfo != LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                        {
                            GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                            CustomerInfo = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        }
                        GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.TYPE].Value = LineItem.InvoiceType;
                        GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.TAX].Value = LineItem.InvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForInvoice.Rows[irow].Cells[(int)QuoteReportByInvoiceTableColumn.NET].Value = Math.Round(LineItem.InvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        sn++;
                        Total += LineItem.InvoiceAmount;
                        taxTotal += LineItem.InvoiceTax;
                    }

                    int lastRowIndex = GridViewForInvoice.Rows.Add();
                    GridViewForInvoice.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForInvoice.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.TYPE].Value = "Total";
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.TAX].Value = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.NET].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
            else if (RptQuoteReport.Type == QouteType.BYCATEGORY)
            {
                ErrorMsg.Text = string.Empty;
                EnableButtons(false);
                GridViewForCategory.Rows.Clear();

                if (RptQuoteReport._LineItems != null && RptQuoteReport._LineItems.Count > 0)
                {
                    EnableButtons(true);

                    int rowCount = 0;
                    int Sn = 0;
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
                            GridViewForCategory.Rows.Add();
                            GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.CATEGORY_NAME].Value = " Category : " + LineItem.Category;
                            Category = LineItem.Category;
                            Sn = 0;
                            rowCount++;
                        }
                        rowCount = GridViewForCategory.Rows.Add();
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.SNO].Value = Sn + 1;
                        if (ProductId != LineItem.Item)
                        {
                            GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.ITEM].Value = LineItem.Item;
                            ProductId = LineItem.Item;
                            ProductName = string.Empty;
                        }
                        if (ProductName != LineItem.ItemName)
                        {
                            GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                            ProductName = LineItem.ItemName;
                        }
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)QuoteReportByCategoryTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                        Sn++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;
                    }

                    int lastRowIndex = GridViewForCategory.Rows.Add();
                    GridViewForCategory.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForCategory.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForCategory.Rows[lastRowIndex].Cells[(int)QuoteReportByCategoryTableColumn.FREE].Value = "Total";
                    GridViewForCategory.Rows[lastRowIndex].Cells[(int)QuoteReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[lastRowIndex].Cells[(int)QuoteReportByCategoryTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCategory.Rows[lastRowIndex].Cells[(int)QuoteReportByCategoryTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
            else if (RptQuoteReport.Type == QouteType.BYPFMAILY)
            {
                ErrorMsg.Text = string.Empty;
                EnableButtons(false);
                GridViewForPFamily.Rows.Clear();
                if (RptQuoteReport._LineItemsPfamily != null && RptQuoteReport._LineItemsPfamily.Count > 0)
                {
                    EnableButtons(true);
                    int Sno = 0;
                    int rowCount = 0;
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
                            GridViewForPFamily.Rows.Add();
                            GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.FAMILY_NAME].Value = " Product Family : " + LineItem.ProductFamily;
                            PFamily = LineItem.ProductFamily;
                            Sno = 0;
                            rowCount++;
                        }
                        GridViewForPFamily.Rows.Add();
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.SNO].Value = Sno + 1;
                        if (ProductId != LineItem.Item)
                        {
                            GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.ITEM].Value = LineItem.Item;
                            ProductId = LineItem.Item;
                            ProductName = string.Empty;
                        }
                        if (ProductName != LineItem.ItemName)
                        {
                            GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                            ProductName = LineItem.ItemName;
                        }
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                        Sno++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;
                    }
                    GridViewForPFamily.Rows.Add();
                    GridViewForPFamily.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForPFamily.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.FREE].Value = "Total";
                    GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForPFamily.Rows[rowCount].Cells[(int)QuoteReportByPFamilyTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
            else if (RptQuoteReport.Type == QouteType.BYCUSTOMER)
            {
                ErrorMsg.Text = string.Empty;
                EnableButtons(false);
                GridViewForCustomers.Rows.Clear();

                if (RptQuoteReport.QuoteReportByCustomerLineItems != null && RptQuoteReport.QuoteReportByCustomerLineItems.Count > 0)
                {
                    EnableButtons(true);
                    int Sno = 0;
                    int rowIndex = 0;
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
                            GridViewForCustomers.Rows.Add();
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CUSTOMER_NAME].Value = " Customer Name : " + LineItem.CustomerName;
                            CustomerName = LineItem.CustomerName;
                            Sno = 0;
                            rowIndex++;
                            dateTime = null;
                        }
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.CustomerInvoiceDate, Global.Company.DateFormat);
                        rowIndex = GridViewForCustomers.Rows.Add();
                        GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.SNO].Value = Sno + 1;
                        GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.INVOICE_NUMBER].Value = LineItem.CustomerInvoiceNumber;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.INVOICE_DATE].Value = LineItem.CustomerInvoiceDate.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }
                        GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.TAX].Value = LineItem.CustomerInvoiceTax.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.TYPE].Value = LineItem.InvoiceType;
                        GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.DIS].Value = Math.Round(LineItem.CustomerInvoiceDiscount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        if (LineItem.InvoiceType == "Credit")
                        {
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                            CreditTotal += LineItem.CustomerInvoiceAmount;
                        }
                        else
                        {
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                            GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            CashTotal += LineItem.CustomerInvoiceAmount;
                        }
                        taxTotal += LineItem.CustomerInvoiceTax;
                        discountTotal += LineItem.CustomerInvoiceDiscount;
                        Sno++;
                        rowIndex++;
                    }

                    GridViewForCustomers.Rows.Add();
                    GridViewForCustomers.Rows[rowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForCustomers.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.INVOICE_DATE].Value = "Total";
                    GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.TAX].Value = taxTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.DIS].Value = Math.Round(discountTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(CashTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomers.Rows[rowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(CreditTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
            else
            {
                ErrorMsg.Text = string.Empty;
                EnableButtons(false);
                GridViewForItem.Rows.Clear();

                if (RptQuoteReport._LineItemsItem != null && RptQuoteReport._LineItemsItem.Count > 0)
                {
                    EnableButtons(true);
                    int rowCount = 0;
                    double Total = 0;
                    double SubTotal = 0;
                    double TaxTotal = 0;

                    foreach (QouteReportByItemLineItem LineItem in RptQuoteReport._LineItemsItem)
                    {
                        GridViewForItem.Rows.Add();
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.SNO].Value = rowCount + 1;
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.ITEM].Value = LineItem.Item;
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.TOTAL].Value = Math.Round(LineItem.Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        rowCount++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;

                    }
                    GridViewForItem.Rows.Add();
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.FREE].Value = "Total";
                    GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.TAX].Value = Math.Round(TaxTotal, 2).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForItem.Rows[rowCount].Cells[(int)QuoteReportByItemTableColumn.TOTAL].Value = Math.Round(Total).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
        }

        private void GridViewForPFamily_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.SNO].Value == null
                && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.ITEM || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.ITEM_NAME || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.QUANTITY)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.TOTAL].Value == null &&
                    GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.ITEM_NAME].Value == null)
            {
                if (e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.QUANTITY || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.FREE || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.SUB_TOTAL || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.TAX || e.ColumnIndex == (int)QuoteReportByPFamilyTableColumn.TOTAL)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void GridViewForPFamily_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.TOTAL].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridViewForPFamily.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridViewForPFamily.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.FAMILY_NAME].Value != null ? GridViewForPFamily.Rows[e.RowIndex].Cells[(int)QuoteReportByPFamilyTableColumn.FAMILY_NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            QuoteReportPrintSave QuoteReportPrintSave = new QuoteReportPrintSave();
            QuoteReportPrintSave.ExportOrPrintToFile(RptQuoteReport, "Quote Report", "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            QuoteReportPrintSave QuoteReportPrintSave = new QuoteReportPrintSave();
            QuoteReportPrintSave.ExportOrPrintToFile(RptQuoteReport, "Quote Report", "pdf", true);
            Cursor.Current = Cursors.Default;
        }

        private void GridViewForCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.TOTAL].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridViewForCategory.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridViewForCategory.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.CATEGORY_NAME].Value != null ? GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.CATEGORY_NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridViewForCategory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.SNO].Value == null
                && GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.ITEM || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.ITEM_NAME || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.QUANTITY)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.TOTAL].Value == null &&
                    GridViewForCategory.Rows[e.RowIndex].Cells[(int)QuoteReportByCategoryTableColumn.ITEM_NAME].Value == null)
            {
                if (e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.QUANTITY || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.FREE || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.SUB_TOTAL || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.TAX || e.ColumnIndex == (int)QuoteReportByCategoryTableColumn.TOTAL)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void GridViewForItem_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForItem.Rows[e.RowIndex].Cells[(int)QuoteReportByItemTableColumn.SNO].Value == null
                && GridViewForItem.Rows[e.RowIndex].Cells[(int)QuoteReportByItemTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex == (int)QuoteReportByItemTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)QuoteReportByItemTableColumn.ITEM || e.ColumnIndex == (int)QuoteReportByItemTableColumn.ITEM_NAME || e.ColumnIndex == (int)QuoteReportByItemTableColumn.QUANTITY)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewForItem.Rows[e.RowIndex].Cells[(int)QuoteReportByItemTableColumn.TOTAL].Value == null &&
                    GridViewForItem.Rows[e.RowIndex].Cells[(int)QuoteReportByItemTableColumn.ITEM_NAME].Value == null)
            {
                if (e.ColumnIndex == (int)QuoteReportByItemTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)QuoteReportByItemTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if(e.ColumnIndex == (int)QuoteReportByItemTableColumn.QUANTITY || e.ColumnIndex == (int)QuoteReportByItemTableColumn.FREE || e.ColumnIndex == (int)QuoteReportByItemTableColumn.SUB_TOTAL || e.ColumnIndex == (int)QuoteReportByItemTableColumn.TAX || e.ColumnIndex == (int)QuoteReportByItemTableColumn.TOTAL)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void GridViewForInvoice_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.SNO].Value == null
                && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.NET].Value != null)
            {
                if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.INVOICE_NUMBER || e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.INVOICE_DATE || e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.CUSTOMER_INFO)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.NET].Value == null &&
                    GridViewForInvoice.Rows[e.RowIndex].Cells[(int)QuoteReportByInvoiceTableColumn.INVOICE_NUMBER].Value == null)
            {
                if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.NET)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.TYPE && e.RowIndex == GridViewForInvoice.Rows.Count - 1)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            }
            if (e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.TAX || e.ColumnIndex == (int)QuoteReportByInvoiceTableColumn.NET)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void BtnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridViewForCustomers_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridViewForCustomers.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridViewForCustomers.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CUSTOMER_NAME].Value != null ? GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CUSTOMER_NAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridViewForCustomers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.SNO].Value == null
                && GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value != null)
            {
                if (e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.SNO || e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.INVOICE_NUMBER)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }

            }
            else if (e.RowIndex > -1 && GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT].Value == null &&
                    GridViewForCustomers.Rows[e.RowIndex].Cells[(int)QuoteReportByCustomerTableColumn.INVOICE_NUMBER].Value == null)
            {
                if (e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.INVOICE_DATE && e.RowIndex == GridViewForCustomers.Rows.Count - 1)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

            }
            if (e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.TAX || e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.DIS || e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.CASH_AMOUNT || e.ColumnIndex == (int)QuoteReportByCustomerTableColumn.CREDIT_AMOUNT)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
    }
}
