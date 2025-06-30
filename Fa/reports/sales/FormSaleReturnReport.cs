using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using fa.report.sales;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Sale;
using FADataAccessLibrary.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.sales
{
    public partial class FormSaleReturnReport : Form
    {
        public FormSaleReturnReport()
        {
            InitializeComponent();
        }
        SalesReportByCustomer SalesReportByCustomer1 = null;

        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        private void FormSaleReturnReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            LoadCombo();
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ResetForm();
                GridViewForInvoice.Visible = true;
                GridViewForCustomer.Visible = false;
                ComboBoxCustomerforReturn.Visible = false;
                LabelCustomer.Visible = false;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ResetForm();
                GridViewForInvoice.Visible = false;
                GridViewForCustomer.Visible = true;
                ComboBoxCustomerforReturn.Visible = true;
                LabelCustomer.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GridViewForCustomer.Rows.Clear();
            GridViewForInvoice.Rows.Clear();
            if (Validation())
            {
                if (ComboBoxReportType.Text == "By Invoice")
                {
                    generateSalesReportByInvoice();
                }
                else if (ComboBoxReportType.Text == "By Customer")
                {
                    generateSalesReportByCustomer();
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeCustomerComboForReturn(ComboBoxCustomerforReturn, Global.Company.CompanyId);
        }

        private void generateDefaultReport()
        {
            generateSalesReportByInvoice();
        }        
        private bool Validation()
        {
            if (FromDate.Date > ToDate.Date)
            {
                SaleReturnReportErrorMsg.Text = "Please select valid date..";
                FromDate.Focus();
                return false;
            }
            if (ComboBoxReportType.Text == "By Customer" && CheckedTreeUtils.SelectedNameNodes(ComboBoxCustomerforReturn).Count < 1)
            {
                SaleReturnReportErrorMsg.Text = "Please select customer..";
                ComboBoxCustomerforReturn.Select();
                return false;
            }
            if (FromDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                SaleReturnReportErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                SaleReturnReportErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
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
            SaleReturnReportErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewForCustomer.Rows.Clear();
            if (ComboBoxCustomerforReturn.Text != null)
            {
                SalesReportByCustomer1 = new SalesReportByCustomer();
                SalesReportByCustomer1.FromDate = (DateTime)FromDate.Date;
                SalesReportByCustomer1.ToDate = (DateTime)ToDate.Date;
                SalesReportByCustomer1.Company = Global.Company;
                SalesReportByCustomer1.entrytype = Entrytype.RETURN;
                SalesReportByCustomer1.Customers = CheckedTreeUtils.SelectedNameNodes(ComboBoxCustomerforReturn).ToArray();
                SalesReportByCustomer1.AllType = SelectedNameNode(ComboBoxCustomerforReturn).ToArray();
                SalesReportByCustomer1.GenerateReport();
                if (SalesReportByCustomer1.LineItems != null && SalesReportByCustomer1.LineItems.Count > 0)
                {
                    EnableButtons(true);
                    int rowCount = 0;
                    double CashTotal = 0;
                    double CreditTotal = 0;
                    double taxTotal = 0;
                    double discountTotal = 0;
                    string customer = "";
                    string invoiceDate = "";
                    int i = 1;
                    foreach (SalesReportByCustomerLineItem LineItem in SalesReportByCustomer1.LineItems.OrderBy(x => x.Customer))
                    {
                        GridViewForCustomer.Rows.Add();
                        if (customer == "" || customer != LineItem.Customer)
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TYPE].Value = "Name : " + LineItem.Customer;
                            customer = LineItem.Customer;
                            GridViewForCustomer.Rows.Add();
                            rowCount++;
                            i = 1;
                        }
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.SNO].Value = i;
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER].Value = LineItem.CustomerInvoiceNumber;
                        if (string.IsNullOrEmpty(invoiceDate) || invoiceDate != LineItem.CustomerInvoiceDate.Date.ToString())
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_DATE].Value = LineItem.CustomerInvoiceDate.Date.ToString(Global.Company.DateFormat);
                            invoiceDate = LineItem.CustomerInvoiceDate.Date.ToString();
                        }
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TAX].Value = Math.Round(LineItem.CustomerInvoiceTax, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TYPE].Value = LineItem.InvoiceType;
                        GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.DIS].Value = Math.Round(LineItem.CustomerInvoiceDiscount, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                        if (LineItem.InvoiceType == "Credit")
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            CreditTotal += LineItem.CustomerInvoiceAmount;
                        }
                        else
                        {
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = 0.00.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(LineItem.CustomerInvoiceAmount, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            CashTotal += LineItem.CustomerInvoiceAmount;
                        }
                        taxTotal += LineItem.CustomerInvoiceTax;
                        discountTotal += LineItem.CustomerInvoiceDiscount;
                        rowCount++;
                        i++;
                    }
                    GridViewForCustomer.Rows.Add();
                    GridViewForCustomer.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewForCustomer.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_DATE].Value = "Total";
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.TAX].Value = Math.Round(taxTotal, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.DIS].Value = Math.Round(discountTotal, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CASH_AMOUNT].Value = Math.Round(CashTotal, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForCustomer.Rows[rowCount].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value = Math.Round(CreditTotal, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    SaleReturnReportErrorMsg.Text = "No Information Found..!";
                }
            }
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

        //sales report by Invoice
        private void generateSalesReportByInvoice()
        {
            SaleReturnReportErrorMsg.Text = string.Empty;
            EnableButtons(false);
            //generate the SalesReportBy Invoice
            GridViewForInvoice.Rows.Clear();
            SalesReportByInvoice SalesReportByInvoice1 = new SalesReportByInvoice();
            SalesReportByInvoice1.FromDate = (DateTime)FromDate.Date;
            SalesReportByInvoice1.ToDate = (DateTime)ToDate.Date;
            SalesReportByInvoice1.Company = Global.Company;
            SalesReportByInvoice1.entrytype = Entrytype.RETURN;
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
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.CUSTOMER_INFO].Value = LineItem.CustomerName + System.Environment.NewLine + LineItem.CustomerAddress.Replace(",", "," + System.Environment.NewLine);
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TYPE].Value = LineItem.InvoiceType;
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.NET].Value = Math.Round(LineItem.InvoiceAmount, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TAX].Value = Math.Round(LineItem.InvoiceTax, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    rowCount++;
                    Total += LineItem.InvoiceAmount;
                    taxTotal += LineItem.InvoiceTax;
                }
                GridViewForInvoice.Rows[rowCount].DefaultCellStyle.BackColor = SystemColors.Control;
                GridViewForInvoice.Rows[rowCount].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TYPE].Value = "Total";
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.TAX].Value = Math.Round(taxTotal, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
                GridViewForInvoice.Rows[rowCount].Cells[(int)SalesReportByInvoiceTableColumn.NET].Value = Math.Round(Total, MidpointRounding.AwayFromZero).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)); 
            }
            else
            {
                SaleReturnReportErrorMsg.Text = "No Information Found..!";
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
            SaleReturnReportErrorMsg.Text = "";
            this.Text = "Sales Return Report";
            GridViewForCustomer.Rows.Clear();
            GridViewForInvoice.Rows.Clear();
            ComboBoxCustomerforReturn.Nodes.Clear();
            FromDate.Format = Global.Company.DateFormat;
            ToDate.Format = Global.Company.DateFormat;
            FromDate.Date = DateTime.Now.AddDays(-30);
            ToDate.Date = DateTime.Now.AddDays(1);
            LoadCombo();
            EnableButtons(false);
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            ComboBoxReportType.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();
            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);

            if (ComboBoxReportType.Text == "By Invoice")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForInvoice, "Sales Return Report", "InvoiceWiseSalesReturnReport", "pdf", false, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Customer")
            {
                SaleReportPrintSaveA4.CustomerWiseSaleReturnExportOrPrint(SalesReportByCustomer1, "Sales Return Report", "CustomerWiseSalesReturnReport", "pdf", false, lFromDate, lToDate);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SaleReportPrintSaveA4 SaleReportPrintSaveA4 = new SaleReportPrintSaveA4();

            string lFromDate = ((DateTime)FromDate.Date).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ToDate.Date).ToString(Global.Company.DateFormat);

            if (ComboBoxReportType.Text == "By Invoice")
            {
                SaleReportPrintSaveA4.ExportToFileOrPrint(GridViewForInvoice, "Sales Return Report", "InvoiceWiseSalesReturnReport", "pdf", true, lFromDate, lToDate);
            }
            else if (ComboBoxReportType.Text == "By Customer")
            {
                SaleReportPrintSaveA4.CustomerWiseSaleReturnExportOrPrint(SalesReportByCustomer1, "Sales Return Report", "CustomerWiseSalesReturnReport", "pdf", true, lFromDate, lToDate);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ComboBoxCustomerforReturn_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxCustomerforReturn.CheckedNodes != null && ComboBoxCustomerforReturn.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCustomerforReturn.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All customers"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + (string.IsNullOrEmpty(node.Text) ? "Unknown Customer" : node.Text));
                }
                this.Text = "Sales Return Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Sales Return Report ";
            }
        }

        private void GridViewForCustomer_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.INVOICE_NUMBER].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(0, e.RowBounds.Top, this.GridViewForCustomer.Columns.
                GetColumnsWidth(DataGridViewElementStates.Visible) - this.GridViewForCustomer.HorizontalScrollingOffset, e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.TYPE].Value != null ? GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.TYPE].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private void GridViewForCustomer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewForCustomer.Rows[e.RowIndex].Cells[(int)SalesReportByCustomerTableColumn.CREDIT_AMOUNT].Value == null && e.RowIndex != GridViewForCustomer.Rows.Count - 1)
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
                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == 2)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridViewForInvoice_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex == GridViewForInvoice.Rows.Count - 1)
            {
                if (e.ColumnIndex > 0 && e.ColumnIndex < 4)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == 4)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                if (e.ColumnIndex == 0)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
    }
}
