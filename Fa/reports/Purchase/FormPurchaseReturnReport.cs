using fa.api.utils;
using fa.libraries.utils;
using fa.report.Purchase;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.controls.grid;
using fa.views.utils.Report.Sale;
using Fa.report.Purchase;
using Fa.reports.sales;
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

namespace fa.reports.Purchase
{
    public partial class FormPurchaseReturnReport : Form
    {
        PurchaseReturnReportBySupplier PurchaseReturnReportBySupplier1 = null;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        public FormPurchaseReturnReport()
        {
            InitializeComponent();
        }

        private void FormPurchaseReturnReport_Load(object sender, EventArgs e)
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
                GridViewForBill.Visible = true;
                GridViewForSupplier.Visible = false;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = false;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = false;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
            }

            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ResetForm();
                GridViewForBill.Visible = false;
                GridViewForSupplier.Visible = true;
                ComboBoxCategory.Visible = false;
                TreeComboBoxSupplier.Visible = true;
                LabelCategory.Visible = false;
                LabelSupplier.Visible = true;
                LabelItem.Visible = false;
                ComboBoxItem.Visible = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GridViewForSupplier.Rows.Clear();
            GridViewForBill.Rows.Clear();
            if (ComboBoxReportType.Text == "By Bill")
            {
                generatePurchaseReportByBill();
            }
            else if (Validation() && ComboBoxReportType.Text == "By Supplier")
            {
                generatePurchaseReportBySupplier();
            }
            Cursor.Current = Cursors.Default;
        }
        private bool Validation()
        {
            if (ComboBoxReportType.Text == "By Supplier" && CheckedTreeUtils.SelectedNodes(TreeComboBoxSupplier).Count < 1)
            {
                ErrorMsg.Text = "Please select vendor..";
                TreeComboBoxSupplier.Select();
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
        private void LoadCombo()
        {
            ComboUtils.InitializeAllSupplierCombo(TreeComboBoxSupplier, Global.Company.CompanyId);
        }
        private List<long> SelectedNodes(ToolStripComboTree ComboTreeBox)
        {
            int i = 0;
            List<long> Ids = new List<long>();
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
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
            return Ids;
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
        //Purchase report by Supplier
        private void generatePurchaseReportBySupplier()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForSupplier.Rows.Clear();
            if (TreeComboBoxSupplier.Text != null)
            {
                PurchaseReturnReportBySupplier1 = new PurchaseReturnReportBySupplier();
                PurchaseReturnReportBySupplier1.FromDate = (DateTime)FromDate.Date;
                PurchaseReturnReportBySupplier1.ToDate = (DateTime)ToDate.Date;
                PurchaseReturnReportBySupplier1.Company = Global.Company;
                PurchaseReturnReportBySupplier1.SupplierIds = CheckedTreeUtils.SelectedNodes(TreeComboBoxSupplier).ToArray();
                PurchaseReturnReportBySupplier1.GenerateReport();
                if (PurchaseReturnReportBySupplier1.LineItems != null && PurchaseReturnReportBySupplier1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    int rowCount = 0;
                    int i = 1;
                    double CashTotal = 0;
                    double CreditTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    string supplierName = string.Empty;
                    String dateTime = null;

                    foreach (PurchaseReturnReportBySupplierLineItem LineItem in PurchaseReturnReportBySupplier1.LineItems.OrderBy(x => x.SupplierName))
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.SupplierBillDate, Global.Company.DateFormat);
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
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridViewForSupplier.Rows[rowCount].Cells[(int)PurchaseReportBySupplierTableColumn.BILL_DATE].Value = LineItem.SupplierBillDate.Date.ToString(PurchaseReturnReportBySupplier1.Company.DateFormat);
                            dateTime = stringLineItemDate;
                        }
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
                        i++;
                        rowCount++;
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
        //Purchase report by Bill
        private void generatePurchaseReportByBill()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForBill.Rows.Clear();
            PurchaseReturnReportByBill PurchaseReportByBill1 = new PurchaseReturnReportByBill();
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
                String dateTime = null;
                foreach (PurchaseReturnReportByBillLineItem LineItem in PurchaseReportByBill1.LineItems)
                {
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.BillDate, Global.Company.DateFormat);
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.SNO].Value = rowCount + 1;
                    GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.BILL_NUMBER].Value = LineItem.BillNumber;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        GridViewForBill.Rows[rowCount].Cells[(int)PurchaseReportByBillTableColumn.BILL_DATE].Value = LineItem.BillDate.ToString(PurchaseReportByBill1.Company.DateFormat);
                        dateTime = stringLineItemDate;
                    }
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
        private void ResetForm()
        {
            ErrorMsg.Text = "";
            GridViewForSupplier.Rows.Clear();
            GridViewForBill.Rows.Clear();
            TreeComboBoxSupplier.Nodes.Clear();
            LoadCombo();
            ComboBoxItem.SelectedIndex = -1;
            EnableButtons(false);
            this.Text = "Purchase Return Report ";
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewForBill.Columns["Tax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewForBill.Columns["NetAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;

            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["Stax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["Sdic"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["CaAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
            DataGridViewCurrencyColumn currencyColumn5 = (DataGridViewCurrencyColumn)GridViewForSupplier.Columns["CrAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces5)) currencyColumn5.DecimalPlaces = decimalPlaces5;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);
            if (ComboBoxReportType.Text == "By Bill")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForBill, PurchaseReturnReportNames.getPurchaseReturnReportName(PurchaseReturnReportType.BY_BILL), "BillWisePurchaseReturnReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Supplier")
            {
                SaleReportPrintSaveA4.ExportPurchaseReturnSupplierToFileOrPrint(PurchaseReturnReportBySupplier1, "Purchase Return Report", "SupplierWisePurchaseReturnReport", "pdf", false, lFromDate, lToDate);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);
            if (ComboBoxReportType.Text == "By Bill")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForBill, PurchaseReturnReportNames.getPurchaseReturnReportName(PurchaseReturnReportType.BY_BILL), "BillWisePurchaseReturnReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Supplier")
            {
                SaleReportPrintSaveA4.ExportPurchaseReturnSupplierToFileOrPrint(PurchaseReturnReportBySupplier1, "Purchase Return Report", "SupplierWisePurchaseReturnReport", "pdf", true, lFromDate, lToDate);
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
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (TreeComboBoxSupplier.CheckedNodes != null && TreeComboBoxSupplier.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in TreeComboBoxSupplier.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All vendors"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Purchase Return Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Purchase Return Report ";
            }
        }

        private void GridViewForBill_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForBill.Rows[e.RowIndex].Cells[(int)PurchaseReportByBillTableColumn.SNO].Value == null && e.RowIndex != GridViewForBill.Rows.Count - 1)
            {
                if (e.ColumnIndex > (int)PurchaseReportByBillTableColumn.SNO && e.ColumnIndex < (int)PurchaseReportByBillTableColumn.TAX)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PurchaseReportByBillTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
            }
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
    }
}
