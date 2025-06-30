using fa;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Inventory;
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

namespace Fa.reports.Inventory
{
    enum StockReciveReportByItemTableColumn
    {
        SNO, ITEM, ITEM_NAME, BATCH_NUMBER, EXP_DATE, QUANTITY, FREE, ID
    }
    enum StockReciveReportByDateTableColumn
    {
        SNO, DATE, REF, REQUEST, DESTINATION, QTY, FREE, RECEVIEDBY, ID
    }
    enum StockReciveReportByLocationTableColumn
    {
        SNO, DATE, REF, DESTINATION, QTY, FREE, RECEVIEDBY, ID
    }
    public partial class FromStockreceivereports : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public long LocationId = 0L;
        RptStockReceive RptStockReceive = null;
        public FromStockreceivereports()
        {
            InitializeComponent();
        }

        private void IntraStockreceivereports_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
            ToolStripLabelStockReceiveLocation.Visible = false;
            ToolStripSeparatorStockReceive1.Visible = false;
            CheckedTreeComboBoxLocation.Visible = false;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridViewByItemStockRecive.Rows.Clear();
            GridViewByDateStockRecive.Rows.Clear();
            GridViewByLocationStockRecive.Rows.Clear();
            ErrorMsgStockReciveReport.Text = "";
            EnableButton(false);
            CheckedTreeComboBoxLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            StockReciveReportFromDate.Format = Global.Company.DateFormat;
            StockReciveReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            StockReciveReportToDate.Format = Global.Company.DateFormat;
            StockReciveReportToDate.Date = Global.getTransactionDate();
            ComboBoxTypeSelection.SelectedIndex = 0;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ComboTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                rtyp = 1;
                CheckedTreeComboBoxLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxLocation.Visible = false;
                GridViewByDateStockRecive.Visible = false;
                GridViewByItemStockRecive.Visible = true;
                GridViewByLocationStockRecive.Visible = false;
                ToolStripLabelStockReceiveLocation.Visible = false;
                ToolStripSeparatorStockReceive1.Visible = false;
                GridViewByDateStockRecive.Rows.Clear();
                GridViewByLocationStockRecive.Rows.Clear();
                GridViewByItemStockRecive.Rows.Clear();
                EnableButton(false);
                this.Text = "Stock Receive Report";
                ErrorMsgStockReciveReport.Text = "";
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 0)
            {
                rtyp = 0;
                CheckedTreeComboBoxLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxLocation.Visible = false;
                GridViewByDateStockRecive.Visible = true;
                GridViewByItemStockRecive.Visible = false;
                GridViewByLocationStockRecive.Visible = false;
                ToolStripLabelStockReceiveLocation.Visible = false;
                ToolStripSeparatorStockReceive1.Visible = false;
                GridViewByDateStockRecive.Rows.Clear();
                GridViewByLocationStockRecive.Rows.Clear();
                GridViewByItemStockRecive.Rows.Clear();
                EnableButton(false);
                this.Text = "Stock Receive Report";
                ErrorMsgStockReciveReport.Text = "";
            }
            else
            {
                rtyp = 2;
                CheckedTreeComboBoxLocation.Visible = true;
                GridViewByDateStockRecive.Visible = false;
                GridViewByItemStockRecive.Visible = false;
                GridViewByLocationStockRecive.Visible = true;
                ToolStripLabelStockReceiveLocation.Visible = true;
                ToolStripSeparatorStockReceive1.Visible = true;
                GridViewByDateStockRecive.Rows.Clear();
                GridViewByLocationStockRecive.Rows.Clear();
                GridViewByItemStockRecive.Rows.Clear();
                EnableButton(false);
                this.Text = "Stock Receive Report";
                ErrorMsgStockReciveReport.Text = "";
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
            this.Text = "Stock Receive Report";
            ToolStripLabelStockReceiveLocation.Visible = false;
            ToolStripSeparatorStockReceive1.Visible = false;
            CheckedTreeComboBoxLocation.Visible = false;
        }
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(CheckedTreeComboBoxLocation, Global.Company.CompanyId);
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
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (CheckedTreeComboBoxLocation.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in CheckedTreeComboBoxLocation.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Location"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Stock Receive Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Receive Report";
            }
        }
        private bool FormValidate()
        {
            ErrorMsgStockReciveReport.Text = "";
            if (ComboBoxTypeSelection.SelectedIndex < 0)
            {
                ErrorMsgStockReciveReport.Text = "Please select type";
                ComboBoxTypeSelection.Select();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxLocation).Count < 1)
            {
                ErrorMsgStockReciveReport.Text = "Please select location";
                CheckedTreeComboBoxLocation.Focus();
                return false;
            }
            if (StockReciveReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)StockReciveReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgStockReciveReport.Text = EnterValidDateErrorMsg;
                StockReciveReportFromDate.Focus();
                return false;
            }
            if (StockReciveReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)StockReciveReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgStockReciveReport.Text = EnterValidDateErrorMsg;
                StockReciveReportToDate.Focus();
                return false;
            }
            if(StockReciveReportFromDate.Date > StockReciveReportToDate.Date)
            {
                ErrorMsgStockReciveReport.Text = EnterValidDateErrorMsg;
                StockReciveReportFromDate.Focus();
                return false;
            }
            return true;
        }
        private void LoadStockReport()
        {
            RptStockReceive = new RptStockReceive();
            RptStockReceive.FromDate = (DateTime)StockReciveReportFromDate.Date;
            RptStockReceive.ToDate = (DateTime)StockReciveReportToDate.Date;
            RptStockReceive.Company = Global.Company;
            RptStockReceive.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxLocation);
            RptStockReceive.Type = ComboBoxTypeSelection.SelectedIndex == 0 ? RecevieType.BYDATE : ComboBoxTypeSelection.SelectedIndex == 1 ? RecevieType.BYITEM : RecevieType.BYLOCATION;
            RptStockReceive.LocationIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxLocation).ToArray();
            RptStockReceive.Location = this.Text;
            RptStockReceive.IsAllLocation = SelectedNodesText(CheckedTreeComboBoxLocation) == "All Location" ? true : false;
            RptStockReceive.GenerateReport();
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (RptStockReceive.Type == RecevieType.BYITEM)
            {
                GridViewByItemStockRecive.Rows.Clear();
                if (RptStockReceive.StockRecevieByItems != null && RptStockReceive.StockRecevieByItems.Count > 0)
                {
                    int b = 1;
                    EnableButton(true);
                    foreach (StockRecevieByItemReportLine LineItem in RptStockReceive.StockRecevieByItems)
                    {
                        irow = GridViewByItemStockRecive.Rows.Add();
                        GridViewByItemStockRecive.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridViewByItemStockRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridViewByItemStockRecive.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.SNO].Value = b;
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.ITEM].Value = LineItem.Code;
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.ITEM_NAME].Value = LineItem.Name;
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.BATCH_NUMBER].Value = LineItem.BatchNo;
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.EXP_DATE].Value = string.IsNullOrEmpty(LineItem.BatchNo) ? string.Empty : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.QUANTITY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridViewByItemStockRecive.Rows[irow].Cells[(int)StockReciveReportByItemTableColumn.ID].Value = LineItem.Id;
                        b++;
                    }
                }
                else
                {
                    ErrorMsgStockReciveReport.Text = InformationMsg;
                }
            }
            else
            {
                if (RptStockReceive.StockRecevie != null && RptStockReceive.StockRecevie.Count > 0)
                {
                    int a = 1;
                    EnableButton(true);
                    if (RptStockReceive.Type == RecevieType.BYDATE)
                    {
                        GridViewByDateStockRecive.Rows.Clear();
                        String dateTime = null;
                        foreach (StockRecevieReportLine LineItem in RptStockReceive.StockRecevie.OrderBy(x => x.Date))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            irow = GridViewByDateStockRecive.Rows.Add();

                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.SNO].Value = a;
                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }
                            GridViewByDateStockRecive.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewByDateStockRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewByDateStockRecive.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.REF].Value = LineItem.Reference;
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.REQUEST].Value = LineItem.RecevieLoaction;
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.DESTINATION].Value = LineItem.ToLocation;
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.ID].Value = LineItem.Id;
                            GridViewByDateStockRecive.Rows[irow].Cells[(int)StockReciveReportByDateTableColumn.RECEVIEDBY].Value = LineItem.CreatedBy;
                            a++;
                        }
                    }
                    else
                    {
                        GridViewByLocationStockRecive.Rows.Clear();
                        string Location = string.Empty;
                        String dateTime = null;
                        int i = 0;
                        foreach (StockRecevieReportLine LineItem in RptStockReceive.StockRecevie.OrderBy(x => x.RecevieLoaction))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            if (Location == string.Empty || Location != LineItem.RecevieLoaction)
                            {
                                irow = GridViewByLocationStockRecive.Rows.Add();
                                GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.SNO].Value = LineItem.RecevieLoaction;
                                Location = LineItem.RecevieLoaction;
                                i = 1;
                            }
                            irow = GridViewByLocationStockRecive.Rows.Add();
                            GridViewByLocationStockRecive.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridViewByLocationStockRecive.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridViewByLocationStockRecive.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.SNO].Value = i;
                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }

                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.REF].Value = LineItem.Reference;
                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.DESTINATION].Value = LineItem.ToLocation;
                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.ID].Value = LineItem.Id;
                            GridViewByLocationStockRecive.Rows[irow].Cells[(int)StockReciveReportByLocationTableColumn.RECEVIEDBY].Value = LineItem.CreatedBy;
                            i++;
                        }
                    }
                }
                else
                {
                    ErrorMsgStockReciveReport.Text = InformationMsg;
                }
            }
        }

        private void ByLocationStockReciveGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= 0 && GridViewByLocationStockRecive.Rows[e.RowIndex].Cells[(int)StockReciveReportByLocationTableColumn.ID].Value == null)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    GridViewByLocationStockRecive.RowHeadersWidth,
                    e.RowBounds.Top,
                    GridViewByLocationStockRecive.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                    GridViewByLocationStockRecive.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string locationText = string.Empty;


            }
        }


        private void BtnStockReciveSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewByDateStockRecive.Rows.Clear();
                GridViewByLocationStockRecive.Rows.Clear();
                GridViewByItemStockRecive.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadStockReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgStockReciveReport.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            GridViewByLocationStockRecive.DefaultCellStyle.SelectionForeColor = GridViewByLocationStockRecive.DefaultCellStyle.ForeColor;
            GridViewByLocationStockRecive.DefaultCellStyle.SelectionBackColor = GridViewByLocationStockRecive.DefaultCellStyle.BackColor;
        }

        private void CheckedTreeComboLocation_NodeClickedEvent_1(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void ByLocationStockReciveGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && GridViewByLocationStockRecive.Rows[e.RowIndex].Cells[(int)StockReciveReportByLocationTableColumn.ID].Value == null)
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
        public int rtyp = 0;
        private void BtnSave_Click(object sender, EventArgs e)
        {
            StockReceiveReportSavePrint StockReceiveReportSavePrint = new StockReceiveReportSavePrint();
            StockReceiveReportSavePrint.ExportOrPrintToFile(RptStockReceive, "StockReceiveReport", "pdf", false, rtyp);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StockReceiveReportSavePrint StockReceiveReportSavePrint = new StockReceiveReportSavePrint();
            StockReceiveReportSavePrint.ExportOrPrintToFile(RptStockReceive, "StockReceiveReport", "pdf", true, rtyp);
        }
    }
}
