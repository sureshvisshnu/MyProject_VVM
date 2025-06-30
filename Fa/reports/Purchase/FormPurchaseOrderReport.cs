using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa;
using fa.api.utils;
using fa.libraries.utils;
using fa.views.controls.ComboTreeView;
using Fa.reports.sales;
using Fa.views.utils.Purchase;
using FADataAccessLibrary.report.Purchase;
using FADataAccessLibrary.report.sales;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FADataAccessLibrary.report.Purchase.PurchaseOrderReport;

namespace Fa.reports.Purchase
{
    enum PurchaseOrderReportByInvoice
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, CUSTOMER_INFO, CREDIT_CASH, QTY, AMOUNT
    }
    enum PurchaseOrderReportColumns
    {
        SNO, ITEM_CODE, ITEM_NAME, UOM, QTY, PRICE, AMOUNT, CNAME, ID
    }
    enum PurchaseOrderReportByVendors
    {
        SNO, INVOICE_NUMBER, INVOICE_DATE, VENDOR_ADDRESS, CREDIT_CASH, QTY, CASH_AMOUNT, CREDIT_AMOUNT, ID, VNAME
    }
    public partial class FormPurchaseOrderReport : Form
    {
        public static string EnterValidFromDateErrorMsg = "Please enter valid From date.";
        public static string EnterValidToDateErrorMsg = "Please enter valid To date.";
        public static string CheckValidDateErrorMsg = "From date is greater than To date";
        public static string CheckValidTypeErrorMsg = "Please select type";
        public static string CheckValidCatagoryErrorMsg = "Please select category..";
        public static string CheckValidFamilyErrorMsg = "Please select product family..";
        public static string CheckValidCustomerErrorMsg = "Please select vendor..";
        public static string NoInformationErrorMsg = "No Information Found..!";
        public FormPurchaseOrderReport()
        {
            InitializeComponent();
        }
        PurchaseOrderReport PurchaseOrderReport = null!;

        private void FormPurchaseOrderReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Date = Global.getTransactionDate();
            ResetForm();
            LoadCombo();
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            ErrMsg.Text = "";
            GridViewForCategory.Rows.Clear();
            GridViewForVendors.Rows.Clear();
            GridViewForInvoice.Rows.Clear();
            GridViewForItem.Rows.Clear();
            ComboBoxCategory.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxCategory.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            ComboBoxProductFamily.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxProductFamily.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            ComboBoxVendors.SelectedNode = null!;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxVendors.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            EnableButtons(false);
            this.Text = "Purchase Order Report ";
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
            ComboUtils.InitializeAllPFamilyCombo(ComboBoxProductFamily, Global.Company.CompanyId);
            ComboUtils.InitializeVendorsComboForReport(ComboBoxVendors, Global.Company.CompanyId);
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
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
                BtnReset.PerformClick();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ErrMsg.Text = "";
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ResetForm();
                GridViewForCategory.Visible = false;
                GridViewForVendors.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxVendors.Visible = false;
                LabelCategory.Visible = false;
                LabelCustomer.Visible = false;
                GridViewForInvoice.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForVendors.Visible = false;
                ComboBoxVendors.Visible = false;
                LabelCustomer.Visible = false;
                GridViewForCategory.Visible = true;
                LabelCategory.Visible = true;
                ComboBoxCategory.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ResetForm();
                labelProductFamily.Visible = true;
                ComboBoxProductFamily.Visible = true;
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = true;
                ComboBoxCategory.Visible = false;
                LabelCategory.Visible = false;
                GridViewForVendors.Visible = false;
                LabelCustomer.Visible = false;
                ComboBoxVendors.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForCategory.Visible = false;
                ComboBoxCategory.Visible = false;
                LabelCategory.Visible = false;
                GridViewForVendors.Visible = true;
                LabelCustomer.Visible = true;
                ComboBoxVendors.Visible = true;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForVendors.Visible = false;
                ComboBoxCategory.Visible = false;
                ComboBoxVendors.Visible = false;
                LabelCategory.Visible = false;
                LabelCustomer.Visible = false;
                GridViewForCategory.Visible = false;
                labelProductFamily.Visible = false;
                ComboBoxProductFamily.Visible = false;
                GridViewForItem.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }
        private bool Validation()
        {
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                ErrMsg.Text = CheckValidTypeErrorMsg;
                ComboBoxReportType.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
            {
                ErrMsg.Text = CheckValidCatagoryErrorMsg;
                ComboBoxCategory.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
            {
                ErrMsg.Text = CheckValidFamilyErrorMsg;
                ComboBoxProductFamily.Select();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 3 && CheckedTreeUtils.SelectedNodes(ComboBoxVendors).Count < 1)
            {
                ErrMsg.Text = CheckValidCustomerErrorMsg;
                ComboBoxVendors.Select();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrMsg.Text = EnterValidFromDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrMsg.Text = EnterValidToDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if (FromDate.Date != null && ToDate.Date != null && FromDate.Date > ToDate.Date)
            {
                ErrMsg.Text = CheckValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            return true;
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewForCategory.Rows.Clear();
                GridViewForVendors.Rows.Clear();
                GridViewForInvoice.Rows.Clear();
                GridViewForItem.Rows.Clear();
                EnableButtons(false);
                if (Validation())
                {
                    LoadQuoteReport();
                }
            }
            catch (Exception ex)
            {
                ErrMsg.Text = "Error fetching Stock (Error:" + ex.InnerException?.Message ?? ex.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadQuoteReport()
        {
            ErrMsg.Text = "";
            PurchaseOrderReport = new PurchaseOrderReport();
            PurchaseOrderReport.FromDate = (DateTime)FromDate.Date!;
            PurchaseOrderReport.ToDate = (DateTime)ToDate.Date!;
            PurchaseOrderReport.Company = Global.Company;
            PurchaseOrderReport.Type = ComboBoxReportType.SelectedIndex == 0 ? PurchaseOrderType.BYINVOICE : ComboBoxReportType.SelectedIndex == 1 ? PurchaseOrderType.BYCATEGORY : ComboBoxReportType.SelectedIndex == 2 ? PurchaseOrderType.BYPFMAILY : ComboBoxReportType.SelectedIndex == 3 ? PurchaseOrderType.BYVENDOR : PurchaseOrderType.BYITEM;
            PurchaseOrderReport.VendorsIds = CheckedTreeUtils.SelectedNodes(ComboBoxVendors).Cast<long?>().ToArray();
            PurchaseOrderReport.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
            PurchaseOrderReport.PfamilyIds = CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).ToArray();
            PurchaseOrderReport.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();
            PurchaseOrderReport.GenerateReport();

            if (PurchaseOrderReport.Type == PurchaseOrderType.BYINVOICE)
            {
                if (PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems != null && PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems.Count > 0)
                {
                    GridViewForInvoice.Rows.Clear();
                    EnableButtons(true);
                    int sn = 1;
                    double Total = 0;
                    double Quantity = 0;
                    String dateTime = null!;
                    string CustomerInfo = string.Empty;
                    foreach (PurchaseOrderReportByInvoiceVendorLineItem LineItem in PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                        int irow = GridViewForInvoice.Rows.Add();
                        GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.SNO].Value = sn;
                        GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                            CustomerInfo = string.Empty;
                        }
                        if (CustomerInfo != LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                        {
                            GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.CUSTOMER_INFO].Value = LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                            CustomerInfo = LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        }
                        GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.CREDIT_CASH].Value = LineItem.CreditOrCash;
                        GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForInvoice.Rows[irow].Cells[(int)PurchaseOrderReportByInvoice.AMOUNT].Value = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        sn++;
                        Total += LineItem.Amount;
                        Quantity += LineItem.Qty;
                    }
                    int lastRowIndex = GridViewForInvoice.Rows.Add();
                    GridViewForInvoice.Rows[lastRowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForInvoice.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)PurchaseOrderReportByInvoice.CREDIT_CASH].Value = "Total";
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)PurchaseOrderReportByInvoice.QTY].Value = Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForInvoice.Rows[lastRowIndex].Cells[(int)PurchaseOrderReportByInvoice.AMOUNT].Value = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (PurchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY || PurchaseOrderReport.Type == PurchaseOrderType.BYPFMAILY)
            {
                if (PurchaseOrderReport.PurchaseOrderReportLineItems != null && PurchaseOrderReport.PurchaseOrderReportLineItems.Count > 0)
                {
                    GridViewForCategory.Rows.Clear();
                    EnableButtons(true);
                    int sn = 1;
                    int irow = 0;
                    string Category = string.Empty;
                    double SubTotal = 0;
                    double Total = 0;
                    double SubQty = 0;
                    double TotalQty = 0;
                    bool SubTotalfalg = false;
                    foreach (PurchaseOrderReportLineItem LineItem in PurchaseOrderReport.PurchaseOrderReportLineItems.OrderBy(x => x.CatName).ThenBy(x => x.ItemCode))
                    {
                        if (Category == string.Empty || Category != LineItem.CatName)
                        {
                            if (SubTotalfalg)
                            {
                                irow = GridViewForCategory.Rows.Add();
                                GridViewForCategory.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewForCategory.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                                GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.UOM].Value = "Sub Total";
                                GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.QTY].Value = SubQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                SubTotal = 0;
                                SubQty = 0;
                            }
                            irow = GridViewForCategory.Rows.Add();
                            GridViewForCategory.Rows[irow].DefaultCellStyle.BackColor = SystemColors.Control;
                            GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            GridViewForCategory.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                            GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.CNAME].Value = PurchaseOrderReport.Type == PurchaseOrderType.BYCATEGORY ? (" Category : " + LineItem.CatName) : (" Product Family : " + LineItem.CatName);
                            Category = LineItem.CatName;
                            SubTotalfalg = true;
                            sn = 1;
                        }
                        irow = GridViewForCategory.Rows.Add();
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.SNO].Value = sn;
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.ITEM_CODE].Value = LineItem.ItemCode;
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.UOM].Value = LineItem.UOM.ToString();
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.PRICE].Value = LineItem.Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.ID].Value = LineItem.Id;
                        SubTotal += LineItem.Amount;
                        Total += LineItem.Amount;
                        SubQty += LineItem.Qty;
                        TotalQty += LineItem.Qty;
                        sn++;
                    }
                    irow = GridViewForCategory.Rows.Add();
                    GridViewForCategory.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.UOM].Value = "Sub Total";
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.QTY].Value = SubQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                    irow = GridViewForCategory.Rows.Add();
                    GridViewForCategory.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                    GridViewForCategory.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.UOM].Value = "Total";
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.QTY].Value = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[irow].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = Total.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (PurchaseOrderReport.Type == PurchaseOrderType.BYVENDOR)
            {
                if (PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems != null && PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems.Count > 0)
                {
                    GridViewForVendors.Rows.Clear();
                    EnableButtons(true);
                    int sn = 1;
                    int irow = 0;
                    double SubCashTotal = 0;
                    double SubCreditTotal = 0;
                    double SubQuantity = 0;
                    double CashTotal = 0;
                    double CreditTotal = 0;
                    double Quantity = 0;
                    String dateTime = null!;
                    string CustomerInfo = string.Empty;
                    string Vendors = string.Empty;
                    bool SubTotalflag = false;
                    foreach (PurchaseOrderReportByInvoiceVendorLineItem LineItem in PurchaseOrderReport.PurchaseOrderReportByInvoiceVendorLineItems.OrderBy(x => x.SupplierName))
                    {
                        if (Vendors == string.Empty || Vendors != LineItem.SupplierName)
                        {
                            if (SubTotalflag)
                            {
                                irow = GridViewForVendors.Rows.Add();
                                GridViewForVendors.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                                GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                                GridViewForVendors.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                                GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_CASH].Value = "Sub Total";
                                GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.QTY].Value = SubQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                                GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CASH_AMOUNT].Value = SubCashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT].Value = SubCreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                SubQuantity = 0;
                                SubCashTotal = 0;
                                SubCreditTotal = 0;
                            }
                            irow = GridViewForVendors.Rows.Add();
                            GridViewForVendors.Rows[irow].DefaultCellStyle.BackColor = SystemColors.Control;
                            GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                            GridViewForVendors.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                            GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.VNAME].Value = " Vendor : " + LineItem.SupplierName;

                            SubTotalflag = true;
                            sn = 1;
                        }
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.InvoiceDate, Global.Company.DateFormat);
                        irow = GridViewForVendors.Rows.Add();
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.SNO].Value = sn;
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.INVOICE_NUMBER].Value = LineItem.InvoiceNumber;
                        if (dateTime == null || dateTime != stringLineItemDate || Vendors != LineItem.SupplierName)
                        {
                            GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.INVOICE_DATE].Value = LineItem.InvoiceDate.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                            CustomerInfo = string.Empty;
                        }
                        if (CustomerInfo != LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine))
                        {
                            GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.VENDOR_ADDRESS].Value = LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                            CustomerInfo = LineItem.SupplierAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                        }
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_CASH].Value = LineItem.CreditOrCash;
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CASH_AMOUNT].Value = LineItem.CashAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT].Value = LineItem.CreditAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.ID].Value = LineItem.Id;
                        sn++;
                        SubCashTotal += LineItem.CashAmount;
                        SubCreditTotal += LineItem.CreditAmount;
                        SubQuantity += LineItem.Qty;
                        CashTotal += LineItem.CashAmount;
                        CreditTotal += LineItem.CreditAmount;
                        Quantity += LineItem.Qty;
                        Vendors = LineItem.SupplierName;
                    }
                    irow = GridViewForVendors.Rows.Add();
                    GridViewForVendors.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForVendors.Rows[irow].DefaultCellStyle.ForeColor = Color.Black;
                    GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_CASH].Value = "Sub Total";
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.QTY].Value = SubQuantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CASH_AMOUNT].Value = SubCashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT].Value = SubCreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                    irow = GridViewForVendors.Rows.Add();
                    GridViewForVendors.Rows[irow].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForVendors.Rows[irow].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_CASH].Value = "Total";
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.QTY].Value = Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CASH_AMOUNT].Value = CashTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForVendors.Rows[irow].Cells[(int)PurchaseOrderReportByVendors.CREDIT_AMOUNT].Value = CreditTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (PurchaseOrderReport.Type == PurchaseOrderType.BYITEM)
            {
                if (PurchaseOrderReport.PurchaseOrderReportLineItems != null && PurchaseOrderReport.PurchaseOrderReportLineItems.Count > 0)
                {
                    GridViewForItem.Rows.Clear();
                    EnableButtons(true);
                    int i = 1;
                    int rowCount = 0;
                    double SubTotal = 0;
                    double TotalQty = 0;
                    foreach (PurchaseOrderReportLineItem LineItem in PurchaseOrderReport.PurchaseOrderReportLineItems.OrderBy(x => x.ItemName))
                    {
                        rowCount = GridViewForItem.Rows.Add();
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.SNO].Value = i;
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.ITEM_CODE].Value = LineItem.ItemCode;
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.UOM].Value = LineItem.UOM;
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.PRICE].Value = LineItem.Price.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = LineItem.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.ID].Value = LineItem.Id;
                        i++;
                        SubTotal += LineItem.Amount;
                        TotalQty += LineItem.Qty;
                    }
                    rowCount = GridViewForItem.Rows.Add();
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGray;
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.SelectionBackColor = Color.LightGray;
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.ForeColor = Color.Black;
                    GridViewForItem.Rows[rowCount].DefaultCellStyle.SelectionForeColor = Color.Black;
                    GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.UOM].Value = "Sub Total";
                    GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.QTY].Value = TotalQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForItem.Rows[rowCount].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value = SubTotal.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    ErrMsg.Text = NoInformationErrorMsg;
                }
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
                this.Text = "Purchase Order Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxVendors.CheckedNodes != null && ComboBoxVendors.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxVendors.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All vendors"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Purchase Order Report " + " @ " + CheckedNodes;
            }
            else if (ComboBoxProductFamily.CheckedNodes != null && ComboBoxProductFamily.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxProductFamily.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Product families"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Purchase Order Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Purchase Order Report ";
            }
        }

        private void GridViewForInvoice_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByInvoice.SNO].Value == null
                && GridViewForInvoice.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByInvoice.CREDIT_CASH].Value != null)
            {
                if (e.ColumnIndex == (int)PurchaseOrderReportByInvoice.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex < 4)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                var cell = GridViewForInvoice.Rows[e.RowIndex]?.Cells[(int)PurchaseOrderReportByInvoice.CREDIT_CASH];
                if (cell?.Value != null && cell.Value is string value && value == "Total")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewForCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.ID].Value == null && GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value == null)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(0, e.RowBounds.Top, this.GridViewForCategory.Columns.GetColumnsWidth(
                                                     DataGridViewElementStates.Visible) - this.GridViewForCategory.HorizontalScrollingOffset, e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.CNAME].Value != null ? GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.CNAME].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridViewForCategory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.ID].Value == null)
            {
                if (GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value == null)
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex > 0 && e.ColumnIndex < 6)
                    {
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                else if (GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.AMOUNT].Value != null)
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex > 0 && e.ColumnIndex < 3)
                    {
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                var cell = GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.UOM];
                if (cell?.Value != null && cell.Value is string value && (value == "Total" || value == "Sub Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewForVendors_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.ID].Value == null)
            {
                if (GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.QTY].Value == null)
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex > 0 && e.ColumnIndex < 6)
                    {
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                else if (GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.QTY].Value != null)
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                    if (e.ColumnIndex > 0 && e.ColumnIndex < 4)
                    {
                        e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    }
                }
                var cell = GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.CREDIT_CASH];
                if (cell?.Value != null && cell.Value is string value && (value == "Total" || value == "Sub Total"))
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void GridViewForVendors_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.ID].Value == null && GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.QTY].Value == null)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(0, e.RowBounds.Top, this.GridViewForVendors.Columns.GetColumnsWidth(
                                                     DataGridViewElementStates.Visible) - this.GridViewForVendors.HorizontalScrollingOffset, e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.VNAME].Value != null ? GridViewForVendors.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportByVendors.VNAME].Value.ToString()! : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void GridViewForItem_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.ID].Value == null)
            {
                if (e.ColumnIndex == 0)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex > 0 && e.ColumnIndex < 3)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                var cell = GridViewForItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderReportColumns.UOM];
                if (cell?.Value != null && cell.Value is string value && value == "Sub Total")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PurchaseOrderSavePrint PurchaseOrderSavePrint = new PurchaseOrderSavePrint();
            PurchaseOrderSavePrint.ExportOrPrintToFile(PurchaseOrderReport, "Purchase Order Report", "pdf", false, (DateTime)FromDate.Date!, (DateTime)ToDate.Date!, PurchaseOrderReport.Type.ToString());
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PurchaseOrderSavePrint PurchaseOrderSavePrint = new PurchaseOrderSavePrint();
            PurchaseOrderSavePrint.ExportOrPrintToFile(PurchaseOrderReport, "Purchase Order Report", "pdf", true, (DateTime)FromDate.Date!, (DateTime)ToDate.Date!, PurchaseOrderReport.Type.ToString());
            Cursor.Current = Cursors.Default;
        }
    }
}
