using System;
using System.Diagnostics;
using System.Windows.Forms;
using fa.api.utils;
using fa.report.catalog;
using fa.views.controls.grid;
using fa.views.utils.Report.Inventory;
using MathNet.Numerics.Distributions;

namespace fa.reports.catalog
{
    enum PriceListTableColumn
    {
        SNO, PCODE, PNAME, R_UOM, R_PRICE, W_UOM, W_PRICE, MSRP
    }
    public partial class FormPriceList : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        PriceReport PriceReport = null;
        public FormPriceList()
        {
            InitializeComponent();
        }
        private bool FormValidate()
        {
            ErrorMsgItemLedger.Text = "";
            if (ComboBoxSelectType.SelectedIndex < 0)
            {
                ErrorMsgItemLedger.Text = string.Format(SelectferenceErrorMsg, "Type.");
                ComboBoxSelectType.Select();
                return false;
            }
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewItems.Columns["RetailPrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewItems.Columns["WholesalePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewItems.Columns["msrp"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            return true;
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnsave.Enabled = Enable;
        }
        private void RearrangeColoumn()
        {
            if (ComboBoxSelectType.SelectedIndex == 0)
            {
                GridViewItems.Columns[(int)PriceListTableColumn.W_UOM].Visible = false;
                GridViewItems.Columns[(int)PriceListTableColumn.W_PRICE].Visible = false;
            }
        }
        private void LoadPriceDetails()
        {
            PriceReport = new PriceReport();
            PriceReport.Company = Global.Company;
            PriceReport.StockItem = CheckBoxStockItem.Checked;
            PriceReport.FromDate = Global.getTransactionDate(); //Global.getTransactionDate().ToString(Global.Company.DateFormat)
            PriceReport.GenerateReport();
            if (PriceReport.LineItems != null && PriceReport.LineItems.Count > 0)
            {
                GridViewItems.Rows.Add(PriceReport.LineItems.Count);
                int rowCount = 0;
                foreach (PriceReportLineItems LineItem in PriceReport.LineItems)
                {
                    if (ComboBoxSelectType.SelectedIndex == 0)
                    {
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.SNO].Value = rowCount + 1;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.PCODE].Value = LineItem.MaterialId;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.PNAME].Value = LineItem.Name;
                        GridViewItems.Columns[1].Width = 160;
                        GridViewItems.Columns[2].Width = 360;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.R_UOM].Value = LineItem.RetailUOM;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.R_PRICE].Value = Math.Round(LineItem.RetailPrice, 2);
                        GridViewItems.Columns[5].Visible = false;
                        GridViewItems.Columns[6].Visible = false;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.MSRP].Value = Math.Round(LineItem.MSRP, 2);
                        rowCount++;
                    }
                    else
                    {
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.SNO].Value = rowCount + 1;
                        GridViewItems.Columns[1].Width = 100;
                        GridViewItems.Columns[2].Width = 250;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.PCODE].Value = LineItem.MaterialId;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.PNAME].Value = LineItem.Name;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.R_UOM].Value = LineItem.RetailUOM;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.R_PRICE].Value = Math.Round(LineItem.RetailPrice, 2); 
                        GridViewItems.Columns[5].Visible = true;
                        GridViewItems.Columns[6].Visible = true;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.W_UOM].Value = LineItem.WholesaleUOM;
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.W_PRICE].Value = Math.Round(LineItem.wholeSaleprice, 2);
                        GridViewItems.Rows[rowCount].Cells[(int)PriceListTableColumn.MSRP].Value = Math.Round(LineItem.MSRP, 2);
                        rowCount++;
                    }
                }
                EnableButtons(true);
            }
            else
            {
                ErrorMsgItemLedger.Text = InformationMsg;
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            GridViewItems.Rows.Clear();
            EnableButtons(false);
            if (FormValidate())
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadPriceDetails();
                Cursor.Current = Cursors.Default;
            }
            GridViewItems.DefaultCellStyle.SelectionForeColor = GridViewItems.DefaultCellStyle.ForeColor;
            GridViewItems.DefaultCellStyle.SelectionBackColor = GridViewItems.DefaultCellStyle.BackColor;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ErrorMsgItemLedger.Text = "";
            GridViewItems.Rows.Clear();
            ComboBoxSelectType.SelectedIndex = -1;
            CheckBoxStockItem.Checked = false;
            EnableButtons(false);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            PriceListSavePrint PriceListSavePrint = new PriceListSavePrint();
            if (ComboBoxSelectType.SelectedIndex == 0)
            {
                PriceListSavePrint.ExportOrPrintToFile(PriceReport, "PriceList", "pdf", false, "R");
            }
            else
            {
                PriceListSavePrint.ExportOrPrintToFile(PriceReport, "PriceList", "pdf", false, "W");
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PriceListSavePrint PriceListSavePrint = new PriceListSavePrint();
            if (ComboBoxSelectType.SelectedIndex == 0)
            {               
                PriceListSavePrint.ExportOrPrintToFile(PriceReport, "PriceList", "pdf", true, "R");                
            }
            else
            {
                PriceListSavePrint.ExportOrPrintToFile(PriceReport, "PriceList", "pdf", true, "W");
            }
        }
        private void FormPriceList_Load(object sender, EventArgs e)
        {
            GridcolumnAlignment();
            EnableButtons(false);
        }

        private void GridcolumnAlignment()
        {
            foreach (DataGridViewColumn column in GridViewItems.Columns)
            {
                if (column.Index == 4 || column.Index == 6 || column.Index == 7)
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
            }
        }
        private void ComboBoxSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableButtons(false);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
