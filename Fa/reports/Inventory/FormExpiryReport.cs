using fa;
using fa.api.utils;
using fa.libraries.utils;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.report.Inventory;
using fa.views.controls.ComboTreeView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.utils.Report.Inventory;
using Fa.views.utils.Report.Inventory;
using Avalonia.Controls;

namespace Fa.reports.Inventory
{
    enum ExpiryReportTableColumn
    {
        SNO, MID, PNAME, UOM, BAT, EXPDATE, CLSTK, LOCATION
    }
    public partial class FormExpiryReport : FormBase
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter current date to future date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public bool CheckLocation = false;
        public long LocationId = 0L;
        ReportStock ReportStock = null;
        public FormExpiryReport()
        {
            InitializeComponent();
        }
        private void ExpiryReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(ComboExpiryReportLocation, Global.Company.CompanyId);
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            this.Text = "Expiry Report";
            ExpiryReportDataGridView.Rows.Clear();
            ErrorMsg.Text = "";
            EnableButton(false);
            ComboExpiryReportLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboExpiryReportLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            ExpiryReportFromDate.Format = Global.Company.DateFormat;
            ExpiryReportFromDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            ExpiryReportFromDate.MinDate = Global.getTransactionDate();
            ExpirydateComboBox.SelectedIndex = -1;
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Location";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            if (CheckedTreeUtils.SelectedNodes(ComboExpiryReportLocation).Count < 1)
            {
                ErrorMsg.Text = "Please select location";
                ComboExpiryReportLocation.Focus();
                return false;
            }
            if (ExpirydateComboBox.SelectedIndex == -1)
            {
                ErrorMsg.Text = "Please select Expires in ";
                ExpirydateComboBox.Focus();
                return false;
            }
            if (ExpiryReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)ExpiryReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                ExpiryReportFromDate.Focus();
                return false;
            }
            return true;
        }

        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExpiryReportDataGridView.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadExpiryReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.Text = "Error fetching Expiry (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadExpiryReport()
        {
            ReportStock = new ReportStock();
            ReportStock.FromDate = (DateTime)ExpiryReportFromDate.Date;
            DateTime ToDate = (DateTime)ExpiryReportFromDate.Date;
            if (ExpirydateComboBox.SelectedIndex > -1)
            {
                if (ExpirydateComboBox.SelectedIndex == 0)
                {
                    ToDate = ReportStock.FromDate.AddDays(15);
                }
                else if (ExpirydateComboBox.SelectedIndex == 1)
                {
                    ToDate = ReportStock.FromDate.AddDays(30);
                }
                else if (ExpirydateComboBox.SelectedIndex == 2)
                {
                    ToDate = ReportStock.FromDate.AddDays(45);
                }
                else if (ExpirydateComboBox.SelectedIndex == 3)
                {
                    ToDate = ReportStock.FromDate.AddDays(60);
                }
            }


            int rowCount = 0;
            string Loaction = string.Empty;

            ReportStock.ToDate = ToDate;

            ReportStock.Company = Global.Company;
            ReportStock.LocationIds = CheckedTreeUtils.SelectedNodes(ComboExpiryReportLocation).ToArray();
            ReportStock.Location = this.Text;
            ReportStock.IsAllLocation = SelectedNodesText(ComboExpiryReportLocation) == "All" ? true : false;
            ReportStock.IsExpiryReport = true;
            ReportStock.BatchFlag = true;
            ReportStock.GenerateReport();
            ExpiryReportDataGridView.Rows.Clear();

            if (ReportStock.StockLedger != null && ReportStock.StockLedger.Count > 0)
            {
                int j = 2;
                int k = 0;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                EnableButton(true);
                foreach (StockLedger LineItem in ReportStock.StockLedger)
                {
                    CheckLocation = false;
                    int irow = rowCount;
                    if (Loaction != LineItem.Location.Name && !ReportStock.IsAllLocation)
                    {
                        CheckLocation = true;
                        k = 0;
                        irow = ExpiryReportDataGridView.Rows.Add();
                        ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;

                        j++;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.LOCATION].Value = LineItem.Location.Name;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.SNO].Value = LineItem.Location.Name;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.MID].Value = string.Empty;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.PNAME].Value = string.Empty;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.UOM].Value = string.Empty;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.BAT].Value = string.Empty;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.EXPDATE].Value = string.Empty;
                        ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.CLSTK].Value = string.Empty;
                        Loaction = LineItem.Location.Name;
                    }
                    if (LineItem.Batch && LineItem.OpeningStockforBatch.Count > 0)
                    {
                        foreach (BatchOpeningStock BatchWise in LineItem.OpeningStockforBatch.Where(x => x.ClosingStock > 0))
                        {

                            irow = ExpiryReportDataGridView.Rows.Add();
                            ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            ExpiryReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.SNO].Value = k + 1;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.MID].Value = LineItem.MaterialId;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.PNAME].Value = LineItem.Name;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.UOM].Value = LineItem.uom;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.BAT].Value = BatchWise.BatchNo;
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.EXPDATE].Value = String.Format("{0:d}", BatchWise.ExpDate.ToString(Global.Company.DateFormat));
                            ExpiryReportDataGridView.Rows[irow].Cells[(int)ExpiryReportTableColumn.CLSTK].Value = BatchWise.ClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            rowCount++;
                            k++;
                        }
                    }
                    rowCount++;
                }
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ExpiryReportSavePrint ExpiryReportSavePrint = new ExpiryReportSavePrint();
            ExpiryReportSavePrint.ExportOrPrintToFile(ReportStock, "ExpiryReport", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ExpiryReportSavePrint ExpiryReportSavePrint = new ExpiryReportSavePrint();
            ExpiryReportSavePrint.ExportOrPrintToFile(ReportStock, "ExpiryReport", "pdf", true);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                BtnReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboExpiryReportLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboExpiryReportLocation.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboExpiryReportLocation.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Location"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Expiry Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Expiry Report";
            }
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }

        private void ExpiryReportDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && ExpiryReportDataGridView.Rows[e.RowIndex].Cells[(int)ExpiryReportTableColumn.LOCATION].Value != null)
            {
                if (e.ColumnIndex == (int)ExpiryReportTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
    }
}