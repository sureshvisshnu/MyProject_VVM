using fa.api.utils;
using fa.libraries.utils;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.sales;
using fa.views.utils.Report.Catalog;
using Fa.report.catalog;
using Fa.reports.Hms;
using Fa.reports.sales;
using FADataAccessLibrary.report.sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.catalog
{
    enum ItemReportTableColumn
    {
        SNO, PCODE, PNAME, PUOM, R_XFAC, R_UOM, W_XFAC, W_UOM, P_PRICE, COST, R_PRICE, W_PRICE, MSRP, O_STOCK, P_QTY, S_QTY, C_QTY, CATEGORY_NAME
    }
    //List<string> GridColumns = new List<string>();
    //GridColumns.a{ "SNO", "PCODE", "PNAME", "PUOM", "R_XFAC", "R_UOM",
    //    " W_XFAC"," W_UOM", "P_PRICE"," COST", "R_PRICE"," W_PRICE", "MSRP", "O_STOCK", "P_QTY", "S_QTY" ,"C_QTY "};
    public partial class FormItemReport : Form
    {
        AllItemsReport AllItemsReport = null;
        public FormItemReport()
        {
            InitializeComponent();
        }
        private void FormItemReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCombo();
            ResetForm();
            EnableButtons(false);
            GridViewItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            GridViewItems.MultiSelect = false;
            Cursor.Current = Cursors.Default;
        }
        private void LoadItemsDetails()
        {
            ErrorMsg.Text = string.Empty;
            EnableButtons(false);
            GridViewItems.Rows.Clear();
            AllItemsReport = new AllItemsReport();
            AllItemsReport.Company = Global.Company;
            AllItemsReport.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
            AllItemsReport.GenerateReport();
            
            if (AllItemsReport.LineItems != null && AllItemsReport.LineItems.Count > 0)
            {
                EnableButtons(true);
                GridViewItems.Rows.Add(AllItemsReport.LineItems.Count);
                int rowCount = 0;
                int Sn = 0;
                string Category = string.Empty;
                foreach (ItemReportLineItems LineItem in AllItemsReport.LineItems.OrderBy(x => x.CategoryName))
                {
                    if (Category != LineItem.CategoryName)
                    {
                        GridViewItems.Rows.Add();
                        GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.SNO].Value = " Category : " + LineItem.CategoryName;
                        Category = LineItem.CategoryName;
                        Sn = 0;
                        rowCount++;
                    }
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.SNO].Value = Sn + 1;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.COST].Value = LineItem.Cost.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.C_QTY].Value = LineItem.CurrentQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.MSRP].Value = LineItem.MSRP.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.O_STOCK].Value = LineItem.OpenStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.PCODE].Value = LineItem.MaterialId;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.PNAME].Value = LineItem.Name;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.PUOM].Value = LineItem.Uom;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.P_PRICE].Value = LineItem.ProductPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.P_QTY].Value = LineItem.PurchaseQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.R_PRICE].Value = LineItem.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.R_UOM].Value = LineItem.RetailUOM;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.R_XFAC].Value = LineItem.RetailXfactor;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.S_QTY].Value = LineItem.SalesQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.W_PRICE].Value = LineItem.wholeSaleprice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.W_UOM].Value = LineItem.WholesaleUOM;
                    GridViewItems.Rows[rowCount].Cells[(int)ItemReportTableColumn.W_XFAC].Value = LineItem.WholeSaleXfactor;
                    rowCount++;
                    Sn++;
                }
                GridColumnVisibleChange();
                BtnSave.Select();
            }
            else
            {
                ErrorMsg.Text = "No Information Found..!";
            }
        }
        private void GridColumnVisibleChange()
        {
            foreach (ComboTreeNode ComboTreeNode in ComboBoxColumns.Nodes)
            {
                if (ComboTreeNode != null)
                {
                    if (ComboTreeNode.Name == "0" || ComboTreeNode.Name == "1" || ComboTreeNode.Name == "2")
                    {
                        ComboBoxColumns.Nodes[int.Parse(ComboTreeNode.Name)].Checked = true;
                        continue;
                    }
                    if (ComboTreeNode.Checked == true)
                    {
                        GridViewItems.Columns[int.Parse(ComboTreeNode.Name)].Visible = true;
                    }
                    else
                    {
                        GridViewItems.Columns[int.Parse(ComboTreeNode.Name)].Visible = false;
                    }
                }
            }
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnsave.Enabled = Enable;
        }
        private void ab2ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void LoadCombo()
        {
            ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SavePrintCatalogItemReport SavePrintCatalogItemReport = new SavePrintCatalogItemReport();
            SavePrintCatalogItemReport.ExportToFileOrPrint(GridViewItems, AllItemsReport, "Items Report", "ItemsReport", "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SavePrintCatalogItemReport SavePrintCatalogItemReport = new SavePrintCatalogItemReport();
            SavePrintCatalogItemReport.ExportToFileOrPrint(GridViewItems, AllItemsReport, "Items Report", "ItemsReport", "pdf", true);
            Cursor.Current = Cursors.Default;
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableButtons(false);
            GridViewItems.Rows.Clear();
            GridColumnVisibleChange();
            Cursor.Current = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            foreach (ComboTreeNode ComboTreeNode in ComboBoxCategory.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxColumns.Nodes)
            {
                ComboTreeNode.Checked = true;
            }
            this.Text = "Item Report ";
            ErrorMsg.Text = "";
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
        private void BtnGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Validation())
            {
                LoadItemsDetails();
            }
            Cursor.Current = Cursors.Default;
        }

        private void GridViewItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void BtnExit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnGo.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCancel.Select();
            }
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
                this.Text = "Item Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Item Report ";
            }
        }

        private void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private bool Validation()
        {
            if (CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
            {
                ErrorMsg.Text = "Please select category..";
                ComboBoxCategory.Select();
                return false;
            }
            return true;
        }

        private void GridViewItems_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.SNO].Value == null
                && GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.C_QTY].Value != null)
            {
                if (e.ColumnIndex == (int)ItemReportTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.C_QTY].Value == null &&
                    GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.PCODE].Value == null)
            {
                if (e.ColumnIndex == (int)ItemReportTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)ItemReportTableColumn.C_QTY)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridViewItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.C_QTY].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.GridViewItems.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.GridViewItems.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = GridViewItems.Rows[e.RowIndex].Cells[(int)ItemReportTableColumn.SNO].Value?.ToString() ?? string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }
    }
}
