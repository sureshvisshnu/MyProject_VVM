using fa.api.catalog;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Catalog;
using fa.report.sales;
using fa.reports.sales;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.controls.grid;
using fa.views.utils.Report.Sale;
using Fa.report.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.Purchase
{
    enum PurchaseReportByTaxTableColumn
    {
        SNO, BILL_DATE, BILL_NUMBER, SUPPLIER_INFO, GSTN, ZVALUE, FVALUE, FAMOUNT, TVALUE, TAMOUNT, EVALUE, EAMOUNT, TEVALUE, TEAMOUNT, OVALUE, OAMOUNT, CHARGES, TOTAL
    }
    enum PurchaseReportByBillTableColumn
    {
        SNO, BILL_NUMBER, BILL_DATE, SUPPLIER_INFO, TYPE, TAX, NET
    }
    enum PurchaseReportBySupplierTableColumn
    {
        SNO, BILL_NUMBER, BILL_DATE, TAX, DIS, CASH_AMOUNT, CREDIT_AMOUNT, TYPE, SUPNAME
    }
    enum PurchaseReportByCategoryTableColumn
    {
        SNO, ITEM, ITEM_NAME, QUANTITY, FREE, BATCH_NUMBER, EXP_DATE, SUB_TOTAL, TAX, TOTAL, CATNAME
    }
    public partial class FormPurchaseReport : Form
    {
        PurchaseReportBySupplier PurchaseReportBySupplier1 = null!;
        PurchaseReportByCategory PurchaseReportByCategory1 = null!;
        PurchaseReportByItem PurchaseReportByItem1 = null!;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public FormPurchaseReport()
        {
            InitializeComponent();
        }
        private void FormPurchaseReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            ResetForm();
            Cursor.Current = Cursors.Default;
        }
        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ResetForm();
                GridViewForTax.Visible = false;
                GridViewForBill.Visible = true;
                GridViewForCategory.Visible = false;
                GridViewForSupplier.Visible = false;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = false;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = false;
                LabelItem.Visible = false;
                CheckedComboBoxItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ResetForm();
                ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
                GridViewForTax.Visible = false;
                GridViewForBill.Visible = false;
                GridViewForCategory.Visible = true;
                GridViewForSupplier.Visible = false;
                ComboBoxCategory.Visible = true;
                TreeComboBoxSupplier.Visible = false;
                LabelCategory.Visible = true;
                LabelSupplier.Visible = false;
                LabelItem.Visible = false;
                CheckedComboBoxItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ResetForm();
                ComboUtils.InitializeAllSupplierCombo(TreeComboBoxSupplier, Global.Company.CompanyId);
                GridViewForTax.Visible = false;
                GridViewForBill.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForSupplier.Visible = true;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = true;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = true;
                LabelItem.Visible = false;
                CheckedComboBoxItem.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
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
                GridViewForTax.Visible = false;
                GridViewForBill.Visible = false;
                GridViewForCategory.Visible = true;
                GridViewForSupplier.Visible = false;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = false;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = false;
                LabelItem.Visible = true;
                CheckedComboBoxItem.Visible = true;
            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                ResetForm();
                GridViewForTax.Visible = true;
                GridViewForBill.Visible = false;
                GridViewForCategory.Visible = false;
                GridViewForSupplier.Visible = false;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = false;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = false;
                LabelItem.Visible = false;
                CheckedComboBoxItem.Visible = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Validation())
            {
                if (ComboBoxReportType.Text == "By Bill")
                {
                    generatePurchaseReportByBill();
                }
                else if (ComboBoxReportType.Text == "By Supplier")
                {
                    generatePurchaseReportBySupplier();
                }
                else if (ComboBoxReportType.Text == "By Item")
                {
                    generatePurchaseReportByItem();
                    this.ActiveControl = ComboBoxReportType.Control;
                    ComboBoxReportType.Select();
                }
                else if (ComboBoxReportType.Text == "By Category")
                {
                    generatePurchaseReportByCategory();
                }
                else if (ComboBoxReportType.Text == "Tax Report")
                {
                    generatePurchaseReportByTax();
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private bool Validation()
        {
            if (ComboBoxReportType.Text == "By Category" && CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
            {
                ErrorMsg.Text = "Please select category..";
                ComboBoxCategory.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Supplier" && CheckedTreeUtils.SelectedNodes(TreeComboBoxSupplier).Count < 1)
            {
                ErrorMsg.Text = "Please select vendor..";
                TreeComboBoxSupplier.Select();
                return false;
            }
            if (ComboBoxReportType.Text == "By Item" && CheckedTreeUtils.SelectedItems(CheckedComboBoxItem).Count < 1)
            {
                ErrorMsg.Text = "Please select items..";
                CheckedComboBoxItem.Select();
                return false;
            }
            if (FromDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
        }
        //Purchase report by Tax
        private void generatePurchaseReportByTax()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForTax.Rows.Clear();
            PurchaseReportByTax PurchaseReportByTax = new PurchaseReportByTax();
            PurchaseReportByTax.FromDate = (DateTime)FromDate.Date;
            PurchaseReportByTax.ToDate = (DateTime)ToDate.Date;
            PurchaseReportByTax.Company = Global.Company;
            PurchaseReportByTax.GenerateReport();
            if (PurchaseReportByTax.LineItems != null && PurchaseReportByTax.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForTax.Rows.Add(PurchaseReportByTax.LineItems.Count + 1);
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
                foreach (PurchaseReportByTaxLineItem LineItem in PurchaseReportByTax.LineItems)
                {
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.SNO].Value = rowCount + 1;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.BILL_NUMBER].Value = LineItem.InvoiceNumber;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.BILL_DATE].Value = LineItem.InvoiceDate.ToString(PurchaseReportByTax.Company.DateFormat);
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.SUPPLIER_INFO].Value = LineItem.CustomerName;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.ZVALUE].Value = LineItem.ZeroTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.FVALUE].Value = LineItem.FiveTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.FAMOUNT].Value = LineItem.FiveTaxAmount;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TVALUE].Value = LineItem.TwelveTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TAMOUNT].Value = LineItem.TwelveTaxAmount;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.EVALUE].Value = LineItem.EighteenTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.EAMOUNT].Value = LineItem.EighteenTaxAmount;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TEVALUE].Value = LineItem.TwentyeightTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TEAMOUNT].Value = LineItem.TwentyeightTaxAmount;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.OVALUE].Value = LineItem.OtherTaxvalue;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.OAMOUNT].Value = LineItem.OtherTaxAmount;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.CHARGES].Value = LineItem.Charges;
                    GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TOTAL].Value = LineItem.Total;
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
                GridViewForTax.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                GridViewForTax.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.GSTN].Value = "Total";
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.ZVALUE].Value = ztaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.FVALUE].Value = ftaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TVALUE].Value = ttaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.EVALUE].Value = etaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TEVALUE].Value = tetaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.OVALUE].Value = otaxValTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.FAMOUNT].Value = ftaxTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TAMOUNT].Value = ttaxTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.EAMOUNT].Value = etaxTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TEAMOUNT].Value = tetaxTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.OAMOUNT].Value = otaxTotal;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.CHARGES].Value = Charges;
                GridViewForTax.Rows[rowCount].Cells[(int)PurchaseReportByTaxTableColumn.TOTAL].Value = Total;
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        //Purchase report by Supplier
        private void generatePurchaseReportBySupplier()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForSupplier.Rows.Clear();
            if (TreeComboBoxSupplier.Text != null)
            {
                PurchaseReportBySupplier1 = new PurchaseReportBySupplier();
                PurchaseReportBySupplier1.FromDate = (DateTime)FromDate.Date;
                PurchaseReportBySupplier1.ToDate = (DateTime)ToDate.Date;
                PurchaseReportBySupplier1.Company = Global.Company;
                PurchaseReportBySupplier1.SupplierIds = CheckedTreeUtils.SelectedNodes(TreeComboBoxSupplier).ToArray();
                PurchaseReportBySupplier1.GenerateReport();
                if (PurchaseReportBySupplier1.LineItems != null && PurchaseReportBySupplier1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    int rowCount = 0;
                    int i = 1;
                    double CashTotal = 0;
                    double CreditTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    string supplierName = string.Empty;

                    foreach (PurchaseReportBySupplierLineItem LineItem in PurchaseReportBySupplier1.LineItems.OrderBy(x => x.SupplierName))
                    {
                        GridViewForSupplier.Rows.Add();
                        if (supplierName == string.Empty || supplierName != LineItem.SupplierName)
                        {
                            i = 1;
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.SUPNAME].Value = "vendor name : " + LineItem.SupplierName;
                            supplierName = LineItem.SupplierName;
                            GridViewForSupplier.Rows.Add();
                            rowCount++;
                        }
                        if (supplierName == LineItem.SupplierName)
                        {
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.SNO].Value = i;
                        }
                        GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.BILL_NUMBER].Value = LineItem.SupplierBillNumber;
                        GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.BILL_DATE].Value = LineItem.SupplierBillDate.Date.ToString(Global.Company.DateFormat);
                        GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.TAX].Value = LineItem.SupplierBillTax;
                        GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.TYPE].Value = LineItem.BillType;
                        GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.DIS].Value = LineItem.SupplierBillDiscount;

                        if (LineItem.BillType == "Credit")
                        {
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT].Value = LineItem.SupplierBillAmount;
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT].Value = 0.00;

                            CreditTotal += LineItem.SupplierBillAmount;
                        }
                        else
                        {
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT].Value = 0.00;
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT].Value = LineItem.SupplierBillAmount;
                            CashTotal += LineItem.SupplierBillAmount;
                        }
                        taxTotal += LineItem.SupplierBillTax;
                        discountTotal += LineItem.SupplierBillDiscount;
                        rowCount++;
                        i++;
                    }
                    GridViewForSupplier.Rows.Add();
                    GridViewForSupplier.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForSupplier.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.BILL_DATE].Value = "Total";
                    GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.TAX].Value = taxTotal;
                    GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.DIS].Value = discountTotal;
                    GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT].Value = CashTotal;
                    GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT].Value = CreditTotal;
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
        }
        private void child(ComboTreeNode lComboTreeNode, List<long> Ids, int i)
        {
            foreach (ComboTreeNode ComboTreeNode in lComboTreeNode.Nodes)
            {
                if (ComboTreeNode != null)
                {
                    if (ComboTreeNode.Checked == true)
                    {
                        Ids.Add(long.Parse(ComboTreeNode.Name));
                        i++;
                    }
                    if (ComboTreeNode.Nodes.Count > 0)
                    {
                        child(ComboTreeNode, Ids, i);
                    }
                }
            }
        }

        //Purchase report by Bill
        private void generatePurchaseReportByBill()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForBill.Rows.Clear();
            PurchaseReportByBill PurchaseReportByBill1 = new PurchaseReportByBill();
            PurchaseReportByBill1.FromDate = (DateTime)FromDate.Date;
            PurchaseReportByBill1.ToDate = (DateTime)ToDate.Date;
            PurchaseReportByBill1.Company = Global.Company;
            PurchaseReportByBill1.GenerateReport();
            if (PurchaseReportByBill1.LineItems != null && PurchaseReportByBill1.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewForBill.Rows.Add(PurchaseReportByBill1.LineItems.Count + 1);
                int rowCount = 0;
                double Total = 0;
                double taxTotal = 0;
                foreach (PurchaseReportByBillLineItem LineItem in PurchaseReportByBill1.LineItems)
                {
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.SNO].Value = rowCount + 1;
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.BILL_NUMBER].Value = LineItem.BillNumber;
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.BILL_DATE].Value = LineItem.BillDate.ToString(PurchaseReportByBill1.Company.DateFormat);
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.SUPPLIER_INFO].Value = LineItem.SupplierName + System.Environment.NewLine + LineItem.SupplierAddress.Replace("\n", "").Replace("\r", "").Replace(", ", "," + System.Environment.NewLine);
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.TYPE].Value = LineItem.BillType;
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.NET].Value = LineItem.BillAmount;
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.TAX].Value = LineItem.BillTax;
                    rowCount++;
                    Total += LineItem.BillAmount;
                    taxTotal += LineItem.BillTax;
                }
                GridViewForBill.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                GridViewForBill.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.TYPE].Value = "Total";
                GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.TAX].Value = taxTotal;
                GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.NET].Value = Total;
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
        }

        //Purchase report by Category
        private void generatePurchaseReportByCategory()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCategory.Rows.Clear();
            if (ComboBoxCategory.Text != null)
            {
                PurchaseReportByCategory1 = new PurchaseReportByCategory();
                PurchaseReportByCategory1.FromDate = (DateTime)FromDate.Date;
                PurchaseReportByCategory1.ToDate = (DateTime)ToDate.Date;
                PurchaseReportByCategory1.Company = Global.Company;
                PurchaseReportByCategory1.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
                PurchaseReportByCategory1.GenerateReport();
                if (PurchaseReportByCategory1.LineItems != null && PurchaseReportByCategory1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    int rowCount = 0;
                    int i = 1;
                    double Total = 0;
                    double SubTotal = 0;
                    double TaxTotal = 0;
                    string CatagoryName = string.Empty;

                    foreach (PurchaseReportByCategoryLineItem LineItem in PurchaseReportByCategory1.LineItems.OrderBy(x => x.Catagory))
                    {
                        GridViewForCategory.Rows.Add();
                        if (CatagoryName == string.Empty || CatagoryName != LineItem.Catagory)
                        {
                            i = 1;
                            GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.CATNAME].Value = "Catagory : " + LineItem.Catagory;
                            CatagoryName = LineItem.Catagory;
                            GridViewForCategory.Rows.Add();
                            rowCount++;
                        }
                        if (CatagoryName == LineItem.Catagory)
                        {
                            GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SNO].Value = i;
                        }
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.ITEM].Value = LineItem.Item;
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(PurchaseReportByCategory1.Company.DateFormat);
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal, 2);
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2);
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TOTAL].Value = Math.Round(LineItem.Total, 2);
                        rowCount++;
                        Total += LineItem.Total;
                        TaxTotal += LineItem.Tax;
                        SubTotal += LineItem.SubTotal;
                        i++;
                    }
                    GridViewForCategory.Rows.Add();
                    GridViewForCategory.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForCategory.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.EXP_DATE].Value = "Total";
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal, 2);
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TAX].Value = Math.Round(TaxTotal, 2);
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TOTAL].Value = Math.Round(Total, 2);
                }
                else
                {
                    ErrorMsg.Text = "No Information Found..!";
                }
            }
        }

        //Purchase report by Item
        private void generatePurchaseReportByItem()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCategory.Rows.Clear();
            PurchaseReportByItem1 = new PurchaseReportByItem();
            PurchaseReportByItem1.FromDate = (DateTime)FromDate.Date;
            PurchaseReportByItem1.ToDate = (DateTime)ToDate.Date;
            PurchaseReportByItem1.Company = Global.Company;
            PurchaseReportByItem1.ItemIds = CheckedTreeUtils.SelectedItems(CheckedComboBoxItem).ToArray();
            PurchaseReportByItem1.GenerateReport();
            if (PurchaseReportByItem1.LineItems != null && PurchaseReportByItem1.LineItems.Count > 0)
            {
                EnableButtons(true);
                int rowCount = 0;
                int i = 1;
                double Total = 0;
                double SubTotal = 0;
                double TaxTotal = 0;
                string ItemName = string.Empty;
                foreach (PurchaseReportByCategoryLineItem LineItem in PurchaseReportByItem1.LineItems.OrderBy(x => x.ItemName))
                {
                    GridViewForCategory.Rows.Add();
                    if (ItemName == string.Empty || ItemName != LineItem.ItemName)
                    {
                        i = 1;
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.CATNAME].Value = "Name : " + LineItem.ItemName;
                        ItemName = LineItem.ItemName;
                        GridViewForCategory.Rows.Add();
                        rowCount++;
                    }
                    if (ItemName == LineItem.ItemName)
                    {
                        GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SNO].Value = i;
                    }
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.ITEM].Value = LineItem.Item;
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.ITEM_NAME].Value = LineItem.ItemName;
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.QUANTITY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.BATCH_NUMBER].Value = LineItem.BatchNumber;
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.EXP_DATE].Value = LineItem.ExpDate.ToString(PurchaseReportByItem1.Company.DateFormat);
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(LineItem.SubTotal, 2);
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TAX].Value = Math.Round(LineItem.Tax, 2);
                    GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TOTAL].Value = Math.Round(LineItem.Total, 2);
                    rowCount++;
                    Total += LineItem.Total;
                    TaxTotal += LineItem.Tax;
                    SubTotal += LineItem.SubTotal;
                    i++;
                }
                GridViewForCategory.Rows.Add();
                GridViewForCategory.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                GridViewForCategory.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.EXP_DATE].Value = "Total";
                GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.SUB_TOTAL].Value = Math.Round(SubTotal, 2);
                GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TAX].Value = Math.Round(TaxTotal, 2);
                GridViewForCategory.Rows[rowCount].Cells[(int)PurchaseReportByCategoryTableColumn.TOTAL].Value = Math.Round(Total, 2);
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void ResetForm()
        {
            ErrorMsg.Text = "";
            GridViewForCategory.Rows.Clear();
            GridViewForSupplier.Rows.Clear();
            GridViewForBill.Rows.Clear();
            GridViewForTax.Rows.Clear();
            ComboBoxCategory.Nodes.Clear();
            TreeComboBoxSupplier.Nodes.Clear();
            EnableButtons(false);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewForBill.Columns["BillTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewForBill.Columns["BillNetAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewForCategory.Columns["CsubTotal"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewForCategory.Columns["Ctax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewForCategory.Columns["Ctotal"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
            DataGridViewCurrencyColumn currencyColumn5 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["Stax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces5)) currencyColumn5.DecimalPlaces = decimalPlaces5;
            DataGridViewCurrencyColumn currencyColumn6 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["Sdic"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces6)) currencyColumn6.DecimalPlaces = decimalPlaces6;
            DataGridViewCurrencyColumn currencyColumn7 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["CrAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces7)) currencyColumn7.DecimalPlaces = decimalPlaces7;
            DataGridViewCurrencyColumn currencyColumn8 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["CaAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces8)) currencyColumn8.DecimalPlaces = decimalPlaces8;

            DataGridViewCurrencyColumn currencyColumn9 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["ExValue"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces9)) currencyColumn9.DecimalPlaces = decimalPlaces9;
            DataGridViewCurrencyColumn currencyColumn10 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["FiveTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces10)) currencyColumn10.DecimalPlaces = decimalPlaces10;
            DataGridViewCurrencyColumn currencyColumn11 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["FiveAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces11)) currencyColumn11.DecimalPlaces = decimalPlaces11;
            DataGridViewCurrencyColumn currencyColumn12 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["twelveTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces12)) currencyColumn12.DecimalPlaces = decimalPlaces12;
            DataGridViewCurrencyColumn currencyColumn13 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["twelveAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces13)) currencyColumn13.DecimalPlaces = decimalPlaces13;
            DataGridViewCurrencyColumn currencyColumn14 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["eighteenTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces14)) currencyColumn14.DecimalPlaces = decimalPlaces14;
            DataGridViewCurrencyColumn currencyColumn15 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["eighteenAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces15)) currencyColumn15.DecimalPlaces = decimalPlaces15;
            DataGridViewCurrencyColumn currencyColumn16 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["TwentyeightTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces16)) currencyColumn16.DecimalPlaces = decimalPlaces16;
            DataGridViewCurrencyColumn currencyColumn17 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["TwentyeightAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces17)) currencyColumn17.DecimalPlaces = decimalPlaces17;
            DataGridViewCurrencyColumn currencyColumn18 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["OtTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces18)) currencyColumn18.DecimalPlaces = decimalPlaces18;
            DataGridViewCurrencyColumn currencyColumn19 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["OtAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces19)) currencyColumn19.DecimalPlaces = decimalPlaces19;
            DataGridViewCurrencyColumn currencyColumn20 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["Charges"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces20)) currencyColumn20.DecimalPlaces = decimalPlaces20;
            DataGridViewCurrencyColumn currencyColumn21 = (DataGridViewCurrencyColumn)GridViewForTax.Columns["Total"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces21)) currencyColumn21.DecimalPlaces = decimalPlaces21;
            this.Text = "Purchase Report ";

        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
            GridViewForBill.Visible = true;
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

            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);
            if (ComboBoxReportType.Text == "By Bill")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForBill, "Purchase Report", "BillWisePurchaseReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Supplier")
            {
                SaleReportPrintSaveA4.ExportSupplierToFileOrPrint(PurchaseReportBySupplier1, "Purchase Report", "SupplierWisePurchaseReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Category")
            {
                SaleReportPrintSaveA4.ExportCatagoryToFileOrPrint(PurchaseReportByCategory1, "Purchase Report", "CatagoryWisePurchaseReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Item")
            {
                SaleReportPrintSaveA4.ExportItemsToFileOrPrint(PurchaseReportByItem1, "Purchase Report", "ItemWisePurchaseReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "Tax Report")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForTax, "Purchase Report", "TaxWisePurchaseReport", "pdf", true, lFromDate, lToDate);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);
            if (ComboBoxReportType.Text == "By Bill")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForBill, "Purchase Report", "BillWisePurchaseReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Supplier")
            {
                SaleReportPrintSaveA4.ExportSupplierToFileOrPrint(PurchaseReportBySupplier1, "Purchase Report", "SupplierWisePurchaseReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Category")
            {
                SaleReportPrintSaveA4.ExportCatagoryToFileOrPrint(PurchaseReportByCategory1, "Purchase Report", "CatagoryWisePurchaseReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Item")
            {
                SaleReportPrintSaveA4.ExportItemsToFileOrPrint(PurchaseReportByItem1, "Purchase Report", "ItemWisePurchaseReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "Tax Report")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForTax, "Purchase Report", "TaxWisePurchaseReport", "pdf", false, lFromDate, lToDate);
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
        private void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
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
                this.Text = "Purchase Report " + " @ " + CheckedNodes;
            }
            else if (TreeComboBoxSupplier.CheckedNodes != null && TreeComboBoxSupplier.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in TreeComboBoxSupplier.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All vendors"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Purchase Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Purchase Report ";
            }
        }

        private void GridViewForCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseReportByCategoryTableColumn.TOTAL].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForCategory.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForCategory.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseReportByCategoryTableColumn.CATNAME].Value != null ? GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseReportByCategoryTableColumn.CATNAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewForCategory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCategory.Rows[e.RowIndex].Cells[(int)PurchaseReportByCategoryTableColumn.SNO].Value == null && e.RowIndex != GridViewForCategory.Rows.Count - 1)
            {
                if (e.ColumnIndex > (int)PurchaseReportByCategoryTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportByCategoryTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PurchaseReportByCategoryTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
            if (e.RowIndex > -1 && e.RowIndex == GridViewForCategory.Rows.Count - 1)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                if (e.ColumnIndex >= (int)PurchaseReportByCategoryTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportByCategoryTableColumn.EXP_DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        private void GridViewForBill_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex == GridViewForBill.Rows.Count - 1)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                if (e.ColumnIndex >= (int)PurchaseReportByBillTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportByBillTableColumn.TYPE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        private void GridViewForTax_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex == GridViewForTax.Rows.Count - 1)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                if (e.ColumnIndex >= (int)PurchaseReportByTaxTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportByTaxTableColumn.GSTN)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridViewForSupplier_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForSupplier.Rows[e.RowIndex].Cells[(int)PurchaseReportBySupplierTableColumn.SNO].Value == null && e.RowIndex != GridViewForSupplier.Rows.Count - 1)
            {
                if (e.ColumnIndex > (int)PurchaseReportBySupplierTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportBySupplierTableColumn.CREDIT_AMOUNT)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PurchaseReportBySupplierTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
            if (e.RowIndex > -1 && e.RowIndex == GridViewForSupplier.Rows.Count - 1)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                if (e.ColumnIndex >= (int)PurchaseReportBySupplierTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportBySupplierTableColumn.BILL_DATE)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridViewForSupplier_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForSupplier.Rows[e.RowIndex].Cells[(int)PurchaseReportBySupplierTableColumn.CASH_AMOUNT].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewForSupplier.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewForSupplier.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForSupplier.Rows[e.RowIndex].Cells[(int)PurchaseReportBySupplierTableColumn.SUPNAME].Value != null ? GridViewForSupplier.Rows[e.RowIndex].Cells[(int)PurchaseReportBySupplierTableColumn.SUPNAME].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private void TreeComboBoxSupplier_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void ComboBoxItem_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void CheckedComboBoxItem_ItemCheckedEvent(object sender, ItemCheckEventArgs e)
        {
            if (CheckedComboBoxItem.CheckedItems.Count == 0)
            {
                this.Text = "Purchase Report ";
            }
            else
            {
                bool allItemsChecked = CheckedComboBoxItem.CheckedItems.Contains("All");
                if (allItemsChecked)
                {
                    this.Text = "Purchase Report @ All Items";
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
                    this.Text = "Purchase Report @ " + sb.ToString();
                }
            }
        }
        private void Ab2ToolStripForPurchase_MouseClick(object sender, MouseEventArgs e)
        {
            this.ActiveControl = ComboBoxReportType.Control;
            ComboBoxReportType.Select();
        }
    }
}
