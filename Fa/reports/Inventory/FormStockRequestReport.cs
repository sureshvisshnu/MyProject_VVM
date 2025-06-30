using fa.api.utils;
using fa.libraries.utils;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
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

namespace fa.reports.Inventory
{
    enum StockRequestReportTableColumn
    {
        SNO, DATE, REF, REQUEST, DESTINATION, QTY, CREATEDBY, ID
    }
    enum StockRequestReportByLocationTableColumn
    {
        SNO, DATE, REF, DESTINATION, QTY, CREATEDBY, ID
    }
    enum StockRequestReporByItemtTableColumn
    {
        SNO, ITEM, ITEM_NAME, BATCH_NUMBER, EXP_DATE, QUANTITY, ID
    }
    public partial class FormStockRequestReport : FormBase
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public long LocationId = 0L;
        RptStockRequest RptStockRequest = null;
        public FormStockRequestReport()
        {
            InitializeComponent();
        }

        private void FormStockRequestReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            BtnToolStripPrint.Enabled = Enable;
            BtnToolStripSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridViewStockRequest.Rows.Clear();
            GridViewStockRequestByItem.Rows.Clear();
            GridViewStockRequestByLocation.Rows.Clear();
            ErrorMsgStockRequestReport.Text = "";
            EnableButton(false);
            CheckedTreeComboLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
            ComboBoxType.SelectedIndex = 0;
        }
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(CheckedTreeComboLocation, Global.Company.CompanyId);
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewStockRequest.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadStockReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgStockRequestReport.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
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
            RptStockRequest = new RptStockRequest();
            RptStockRequest.FromDate = (DateTime)FromDate.Date;
            RptStockRequest.ToDate = (DateTime)ToDate.Date;
            RptStockRequest.Company = Global.Company;
            RptStockRequest.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboLocation);
            RptStockRequest.Type = ComboBoxType.SelectedIndex == 0 ? InventoryReportFilterType.BYDATE : ComboBoxType.SelectedIndex == 1 ? InventoryReportFilterType.BYITEM : InventoryReportFilterType.BYLOCATION;
            RptStockRequest.LocationIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).ToArray();
            RptStockRequest.Location = this.Text;
            RptStockRequest.IsAllLocation = SelectedNodesText(CheckedTreeComboLocation) == "All Location" ? true : false;
            RptStockRequest.GenerateReport();
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (RptStockRequest.Type == InventoryReportFilterType.BYITEM)
            {
                GridViewStockRequestByItem.Rows.Clear();
                if (RptStockRequest.StockRequestByItems != null && RptStockRequest.StockRequestByItems.Count > 0)
                {
                    EnableButton(true);
                    foreach (StockRequestByItemReportLine LineItem in RptStockRequest.StockRequestByItems)
                    {
                        irow = GridViewStockRequestByItem.Rows.Add();
                        GridViewStockRequestByItem.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridViewStockRequestByItem.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridViewStockRequestByItem.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.SNO].Value = irow + 1;
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.ITEM].Value = LineItem.Code;
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.ITEM_NAME].Value = LineItem.Name;
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.BATCH_NUMBER].Value = LineItem.BatchNo;
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.EXP_DATE].Value = string.IsNullOrEmpty(LineItem.BatchNo) ? string.Empty : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.QUANTITY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewStockRequestByItem.Rows[irow].Cells[(int)StockRequestReporByItemtTableColumn.ID].Value = LineItem.Id;
                    }
                }
                else
                {
                    ErrorMsgStockRequestReport.Text = InformationMsg;
                }
            }
            else
            {
                if (RptStockRequest.StockRequests != null && RptStockRequest.StockRequests.Count > 0)
                {
                    EnableButton(true);
                    if (RptStockRequest.Type == InventoryReportFilterType.BYDATE)
                    {
                        GridViewStockRequest.Rows.Clear();
                        DateTime? dateTime = null;
                        foreach (StockRequestReportLine LineItem in RptStockRequest.StockRequests.OrderBy(x => x.Date))
                        {
                            irow = GridViewStockRequest.Rows.Add();
                            if (dateTime == null || ((DateTime)dateTime).Date != LineItem.Date)
                            {
                                GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = LineItem.Date;
                            }
                            GridViewStockRequest.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewStockRequest.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewStockRequest.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.SNO].Value = irow + 1;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.REF].Value = LineItem.Reference;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.REQUEST].Value = LineItem.RequestLoaction;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.DESTINATION].Value = LineItem.ToLocation;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.ID].Value = LineItem.Id;
                            GridViewStockRequest.Rows[irow].Cells[(int)StockRequestReportTableColumn.CREATEDBY].Value = LineItem.CreatedBy;
                        }
                    }
                    else
                    {
                        GridViewStockRequestByLocation.Rows.Clear();
                        string Location = string.Empty;
                        int i = 0;
                        foreach (StockRequestReportLine LineItem in RptStockRequest.StockRequests.OrderBy(x => x.RequestLoaction))
                        {
                            if (Location == string.Empty || Location != LineItem.RequestLoaction)
                            {
                                irow = GridViewStockRequestByLocation.Rows.Add();
                                GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.SNO].Value = LineItem.RequestLoaction;
                                Location = LineItem.RequestLoaction;
                                i = 1;
                            }
                            irow = GridViewStockRequestByLocation.Rows.Add();
                            GridViewStockRequestByLocation.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewStockRequestByLocation.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewStockRequestByLocation.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.SNO].Value = i;
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.REF].Value = LineItem.Reference;
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.DESTINATION].Value = LineItem.ToLocation;
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.ID].Value = LineItem.Id;
                            GridViewStockRequestByLocation.Rows[irow].Cells[(int)StockRequestReportByLocationTableColumn.CREATEDBY].Value = LineItem.CreatedBy;
                            i++;
                        }
                    }
                }
                else
                {
                    ErrorMsgStockRequestReport.Text = InformationMsg;
                }
            }
        }
        private bool FormValidate()
        {
            ErrorMsgStockRequestReport.Text = "";
            if (ComboBoxType.SelectedIndex < 0)
            {
                ErrorMsgStockRequestReport.Text = "Please select type";
                ComboBoxType.Select();
                return false;
            }
            if (ComboBoxType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).Count < 1)
            {
                ErrorMsgStockRequestReport.Text = "Please select location";
                CheckedTreeComboLocation.Focus();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgStockRequestReport.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgStockRequestReport.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if(FromDate.Date > ToDate.Date)
            {
                ErrorMsgStockRequestReport.Text = EnterValidDateErrorMsg;
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
            StockRequestReportSavePrint StockRequestReportSavePrint = new StockRequestReportSavePrint();
            StockRequestReportSavePrint.ExportOrPrintToFile(RptStockRequest, "StockRequestReport", "pdf", false);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StockRequestReportSavePrint StockRequestReportSavePrint = new StockRequestReportSavePrint();
            StockRequestReportSavePrint.ExportOrPrintToFile(RptStockRequest, "StockRequestReport", "pdf", true);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                BtnReset.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CheckedTreeComboLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (CheckedTreeComboLocation.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedTreeComboLocation.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Location"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Stock Requests Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Requests Report";
            }
        }

        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewStockRequest.Rows.Clear();
            GridViewStockRequestByItem.Rows.Clear();
            GridViewStockRequestByLocation.Rows.Clear();
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            if (ComboBoxType.SelectedIndex == 1)
            {
                ToolStripLocationLabel.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                GridViewStockRequest.Visible = false;
                GridViewStockRequestByItem.Visible = true;
                GridViewStockRequestByLocation.Visible = false;
                this.Text = "Stock Requests Report";
            }
            else if (ComboBoxType.SelectedIndex == 2)
            {
                CheckedTreeComboLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                ToolStripLocationLabel.Visible = true;
                CheckedTreeComboLocation.Visible = true;
                GridViewStockRequest.Visible = false;
                GridViewStockRequestByItem.Visible = false;
                GridViewStockRequestByLocation.Visible = true;
                this.Text = "Stock Requests Report";
            }
            else
            {
                ToolStripLocationLabel.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                GridViewStockRequest.Visible = true;
                GridViewStockRequestByItem.Visible = false;
                GridViewStockRequestByLocation.Visible = false;
                this.Text = "Stock Requests Report";
            }
        }

        private void GridViewStockRequestByLocation_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewStockRequestByLocation.Rows[e.RowIndex].Cells[(int)StockRequestReportByLocationTableColumn.ID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top, this.GridViewStockRequestByLocation.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
            this.GridViewStockRequestByLocation.HorizontalScrollingOffset,
            e.RowBounds.Height);
                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString("", drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewStockRequestByLocation_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewStockRequestByLocation.Rows[e.RowIndex].Cells[(int)StockRequestReportByLocationTableColumn.ID].Value == null)
            {
                if (e.ColumnIndex == (int)StockRequestReportByLocationTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)StockRequestReportByLocationTableColumn.CREATEDBY)
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
