using fa.libraries.utils;
using fa.report.Inventory;
using fa.views.controls.ComboTreeView;
using fa.views.controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa;
using fa.api.utils;
using FADataAccessLibrary.report.Inventory;
using Fa.views.utils.Report.Inventory;

namespace Fa.reports.Inventory
{
    enum StockAdjustmentReportByDateTableColumn
    {
        SNO, DATE, REF, ADJUST, QTY, ADJUSTBY, ID
    }
    enum StockAdjustmentReportByLocationTableColumn
    {
        SNO, DATE, REF, QTY, ADJUSTBY, ID
    }
    enum StockAdjustmentReporByItemtTableColumn
    {
        SNO, CODE, NAME, BATCH_NUMBER, EXP_DATE, QUANTITY, ID
    }
    public partial class FormAdjustmentEntryReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public long LocationId = 0L;
        RptAdjustmentEntryReport RptAdjustmentEntryReport = null;
        public FormAdjustmentEntryReport()
        {
            InitializeComponent();
        }
        private void FormAdjustmentEntryReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private void EnableButton(bool Enable)
        {
            AdjustmentEntryBtnPrint.Enabled = Enable;
            AdjustmentEntryBtnSave.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridviewAdjustmentEntryByDate.Rows.Clear();
            GridviewAdjustmentEntryByItem.Rows.Clear();
            GridviewAdjustmentEntryByLocation.Rows.Clear();
            AdjustmentEntryErrorMsg.Text = "";
            EnableButton(false);
            AdjustmentEntryComboxLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in AdjustmentEntryComboxLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
            AdjustmentEntryComboBoxType.SelectedIndex = 0;
        }
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(AdjustmentEntryComboxLocation, Global.Company.CompanyId);
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridviewAdjustmentEntryByDate.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadStockReport();
                }
            }
            catch (Exception ex)
            {
                AdjustmentEntryErrorMsg.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
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
        private void LoadStockReport()
        {
            RptAdjustmentEntryReport = new RptAdjustmentEntryReport();
            RptAdjustmentEntryReport.FromDate = (DateTime)FromDate.Date;
            RptAdjustmentEntryReport.ToDate = (DateTime)ToDate.Date;
            RptAdjustmentEntryReport.Company = Global.Company;
            RptAdjustmentEntryReport.ReportHeader = "For" + " @ " + SelectedNodesText(AdjustmentEntryComboxLocation);
            RptAdjustmentEntryReport.Type = AdjustmentEntryComboBoxType.SelectedIndex == 0 ? InventoryReportFilterType.BYDATE : AdjustmentEntryComboBoxType.SelectedIndex == 1 ? InventoryReportFilterType.BYITEM : InventoryReportFilterType.BYLOCATION;
            RptAdjustmentEntryReport.LocationIds = CheckedTreeUtils.SelectedNodes(AdjustmentEntryComboxLocation).ToArray();
            RptAdjustmentEntryReport.Location = this.Text;
            RptAdjustmentEntryReport.IsAllLocation = SelectedNodesText(AdjustmentEntryComboxLocation) == "All Location" ? true : false;
            RptAdjustmentEntryReport.GenerateReport();
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYITEM)
            {
                GridviewAdjustmentEntryByItem.Rows.Clear();
                if (RptAdjustmentEntryReport.StockAdjustByItemReportLines != null && RptAdjustmentEntryReport.StockAdjustByItemReportLines.Count > 0)
                {
                    EnableButton(true);
                    foreach (StockAdjustByItemReportLine LineItem in RptAdjustmentEntryReport.StockAdjustByItemReportLines)
                    {
                        irow = GridviewAdjustmentEntryByItem.Rows.Add();
                        GridviewAdjustmentEntryByItem.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridviewAdjustmentEntryByItem.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridviewAdjustmentEntryByItem.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.SNO].Value = irow + 1;
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.CODE].Value = LineItem.Code;
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.NAME].Value = LineItem.Name;
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.BATCH_NUMBER].Value = LineItem.BatchNo;
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.EXP_DATE].Value = string.IsNullOrEmpty(LineItem.BatchNo) ? string.Empty : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.QUANTITY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridviewAdjustmentEntryByItem.Rows[irow].Cells[(int)StockAdjustmentReporByItemtTableColumn.ID].Value = LineItem.Id;
                    }
                }
            }
            else
            {
                GridviewAdjustmentEntryByDate.Rows.Clear();
                GridviewAdjustmentEntryByLocation.Rows.Clear();
                if (RptAdjustmentEntryReport.StockAdjustReportLines != null && RptAdjustmentEntryReport.StockAdjustReportLines.Count > 0)
                {
                    EnableButton(true);
                    if (RptAdjustmentEntryReport.Type == InventoryReportFilterType.BYDATE)
                    {
                        GridviewAdjustmentEntryByDate.Rows.Clear();
                        DateTime? dateTime = null;
                        foreach (StockAdjustReportLine LineItem in RptAdjustmentEntryReport.StockAdjustReportLines.OrderBy(x => x.Date))
                        {
                            irow = GridviewAdjustmentEntryByDate.Rows.Add();
                            if (dateTime == null || ((DateTime)dateTime).Date != LineItem.Date)
                            {
                                GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = LineItem.Date;
                            }
                            GridviewAdjustmentEntryByDate.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewAdjustmentEntryByDate.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewAdjustmentEntryByDate.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.SNO].Value = irow + 1;
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.REF].Value = LineItem.Reference;
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.ADJUST].Value = LineItem.AdjustmentLoaction;
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.ID].Value = LineItem.Id;
                            GridviewAdjustmentEntryByDate.Rows[irow].Cells[(int)StockAdjustmentReportByDateTableColumn.ADJUSTBY].Value = LineItem.CreatedBy;
                        }
                    }
                    else
                    {
                        GridviewAdjustmentEntryByLocation.Rows.Clear();
                        string Location = string.Empty;
                        int i = 0;
                        foreach (StockAdjustReportLine LineItem in RptAdjustmentEntryReport.StockAdjustReportLines.OrderBy(x => x.AdjustmentLoaction))
                        {
                            if (Location == string.Empty || Location != LineItem.AdjustmentLoaction)
                            {
                                irow = GridviewAdjustmentEntryByLocation.Rows.Add();
                                GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.SNO].Value = LineItem.AdjustmentLoaction;
                                Location = LineItem.AdjustmentLoaction;
                                i = 1;
                            }
                            irow = GridviewAdjustmentEntryByLocation.Rows.Add();
                            GridviewAdjustmentEntryByLocation.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewAdjustmentEntryByLocation.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewAdjustmentEntryByLocation.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.SNO].Value = i;
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.REF].Value = LineItem.Reference;
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.ID].Value = LineItem.Id;
                            GridviewAdjustmentEntryByLocation.Rows[irow].Cells[(int)StockAdjustmentReportByLocationTableColumn.ADJUSTBY].Value = LineItem.CreatedBy;
                            i++;
                        }
                    }
                }
                else
                {
                    AdjustmentEntryErrorMsg.Text = InformationMsg;
                }
            }
        }
        private bool FormValidate()
        {
            AdjustmentEntryErrorMsg.Text = "";
            if (AdjustmentEntryComboBoxType.SelectedIndex < 0)
            {
                AdjustmentEntryErrorMsg.Text = "Please select type";
                AdjustmentEntryComboBoxType.Select();
                return false;
            }
            if (AdjustmentEntryComboBoxType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(AdjustmentEntryComboxLocation).Count < 1)
            {
                AdjustmentEntryErrorMsg.Text = "Please select location";
                AdjustmentEntryComboxLocation.Focus();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                AdjustmentEntryErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                AdjustmentEntryErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if (FromDate.Date > ToDate.Date)
            {
                AdjustmentEntryErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            return true;
        }
        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
            this.Text = "Stock Requests Report";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            StockAdjustmentReportSavePrint StockAdjustmentReportSavePrint = new StockAdjustmentReportSavePrint();
            StockAdjustmentReportSavePrint.ExportOrPrintToFile(RptAdjustmentEntryReport, "StockAdjustReport", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StockAdjustmentReportSavePrint StockAdjustmentReportSavePrint = new StockAdjustmentReportSavePrint();
            StockAdjustmentReportSavePrint.ExportOrPrintToFile(RptAdjustmentEntryReport, "StockAdjustReport", "pdf", true);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                AdjustmentEntryBtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                AdjustmentEntryBtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                AdjustmentEntryBtnReset.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                AdjustmentEntryBtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AdjustmentEntryComboxLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (AdjustmentEntryComboxLocation.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in AdjustmentEntryComboxLocation.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Location"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Stock Adjustment Entry Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Adjustment Entry Report";
            }
        }

        private void AdjustmentEntryComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustmentEntryBtnPrint.Enabled = false;
            AdjustmentEntryBtnSave.Enabled = false;
            GridviewAdjustmentEntryByDate.Rows.Clear();
            GridviewAdjustmentEntryByItem.Rows.Clear();
            GridviewAdjustmentEntryByLocation.Rows.Clear();
            if (AdjustmentEntryComboBoxType.SelectedIndex == 1)
            {
                LabelAdjustmentEntryLocation.Visible = false;
                AdjustmentEntryComboxLocation.Visible = false;
                GridviewAdjustmentEntryByDate.Visible = false;
                GridviewAdjustmentEntryByItem.Visible = true;
                GridviewAdjustmentEntryByLocation.Visible = false;
                this.Text = "Stock Adjustment Entry Report";
            }
            else if (AdjustmentEntryComboBoxType.SelectedIndex == 2)
            {
                AdjustmentEntryComboxLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in AdjustmentEntryComboxLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                LabelAdjustmentEntryLocation.Visible = true;
                AdjustmentEntryComboxLocation.Visible = true;
                GridviewAdjustmentEntryByDate.Visible = false;
                GridviewAdjustmentEntryByItem.Visible = false;
                GridviewAdjustmentEntryByLocation.Visible = true;
                this.Text = "Stock Adjustment Entry Report";
            }
            else
            {
                LabelAdjustmentEntryLocation.Visible = false;
                AdjustmentEntryComboxLocation.Visible = false;
                GridviewAdjustmentEntryByDate.Visible = true;
                GridviewAdjustmentEntryByItem.Visible = false;
                GridviewAdjustmentEntryByLocation.Visible = false;
                this.Text = "Stock Adjustment Entry Report";
            }
        }

        private void GridviewAdjustmentEntryByLocation_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewAdjustmentEntryByLocation.Rows[e.RowIndex].Cells[(int)StockAdjustmentReportByLocationTableColumn.ID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top, this.GridviewAdjustmentEntryByLocation.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
            this.GridviewAdjustmentEntryByLocation.HorizontalScrollingOffset,
            e.RowBounds.Height);
                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString("", drawFont, drawBrush, rowBounds);
            }
        }

        private void GridviewAdjustmentEntryByLocation_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewAdjustmentEntryByLocation.Rows[e.RowIndex].Cells[(int)StockAdjustmentReportByLocationTableColumn.ID].Value == null)
            {
                if (e.ColumnIndex == (int)StockAdjustmentReportByLocationTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)StockAdjustmentReportByLocationTableColumn.ADJUSTBY)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
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
