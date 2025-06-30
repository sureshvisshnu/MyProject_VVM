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
using fa.libraries.utils;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Inventory;

namespace Fa.reports.Inventory
{
    enum StockMovementReportByDateTableColumn
    {
        SNO, DATE, REF, SOURCE, DESTINATION, QTY, FREE, MOVEDBY, ID
    }
    enum StockMovementReportByLocationTableColumn
    {
        SNO, DATE, REF, DESTINATION, QTY, FREE, MOVEDBY, ID
    }
    enum StockMovementReportByItemtTableColumn
    {
        SNO, CODE, NAME, BATCH, EXP_DATE, QUANTITY, FREE, ID
    }
    public partial class FormStockMovementReports : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectreferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        RptStockMovement RptStockMovement = null;
        public FormStockMovementReports()
        {
            InitializeComponent();
        }
        private void FormStockMovementReports_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCombo();
            ResetForm();
            ToolStripLocationLabel.Visible = false;
            CheckedTreeComboLocation.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            EnableButtons(false);
            StckMvtRptErrMsg.Text = "";
            GridviewForByItem.Rows.Clear();
            GridviewForByDate.Rows.Clear();
            GridviewForByLocation.Rows.Clear();
            if (ComboBoxType.SelectedIndex == 0)
            {
                GridviewForByDate.Visible = true;
                ToolStripLocationLabel.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                GridviewForByItem.Visible = false;
                GridviewForByLocation.Visible = false;
                this.Text = "Stock Movement Report";
            }
            else if (ComboBoxType.SelectedIndex == 1)
            {
                GridviewForByDate.Visible = false;
                GridviewForByItem.Visible = true;
                ToolStripLocationLabel.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                GridviewForByLocation.Visible = false;
                this.Text = "Stock Movement Report";
            }
            else if (ComboBoxType.SelectedIndex == 2)
            {
                CheckedTreeComboLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                GridviewForByDate.Visible = false;
                ToolStripLocationLabel.Visible = true;
                CheckedTreeComboLocation.Visible = true;
                GridviewForByItem.Visible = false;
                GridviewForByLocation.Visible = true;
                this.Text = "Stock Movement Report";
            }
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeStockLocationCombo(CheckedTreeComboLocation, Global.Company.CompanyId);
        }
        public void ResetForm()
        {
            GridviewForByItem.Rows.Clear();
            GridviewForByDate.Rows.Clear();
            GridviewForByLocation.Rows.Clear();
            StckMvtRptErrMsg.Text = "";
            EnableButtons(false);
            CheckedTreeComboLocation.SelectedNode = null;
            foreach (ComboTreeNode comboTreeNode in CheckedTreeComboLocation.Nodes)
            {
                comboTreeNode.Checked = false;
            }
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
        }
        private void EnableButtons(bool Enable)
        {
            BtnSMRSave.Enabled = Enable;
            BtnSMRPrint.Enabled = Enable;
            BtnToolStripPrint.Enabled = Enable;
            BtnToolStripSave.Enabled = Enable;
        }
        private bool FormValidate()
        {
            StckMvtRptErrMsg.Text = "";
            if (ComboBoxType.SelectedIndex < 0)
            {
                StckMvtRptErrMsg.Text = "Please select type";
                ComboBoxType.Select();
                return false;
            }
            if (ComboBoxType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).Count < 1)
            {
                StckMvtRptErrMsg.Text = "Please select location";
                CheckedTreeComboLocation.Focus();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                StckMvtRptErrMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                StckMvtRptErrMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if(FromDate.Date > ToDate.Date)
            {
                StckMvtRptErrMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            return true;
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
                this.Text = "Stock Movement Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Movement Report";
            }
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridviewForByDate.Rows.Clear();
                GridviewForByLocation.Rows.Clear();
                GridviewForByItem.Rows.Clear();
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadStockMovementReport();
                }
            }
            catch (Exception ex)
            {
                StckMvtRptErrMsg.Text = "Error fetching Damage Entry (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadStockMovementReport()
        {
            RptStockMovement = new RptStockMovement();
            RptStockMovement.FromDate = (DateTime)FromDate.Date;
            RptStockMovement.ToDate = (DateTime)ToDate.Date;
            RptStockMovement.Company = Global.Company;
            RptStockMovement.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboLocation);
            RptStockMovement.Type = ComboBoxType.SelectedIndex == 0 ? StockMovementReportFilterType.BYDATE : ComboBoxType.SelectedIndex == 1 ? StockMovementReportFilterType.BYITEM : StockMovementReportFilterType.BYLOCATION;
            RptStockMovement.LocationIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).ToArray();
            RptStockMovement.Location = this.Text;
            RptStockMovement.IsAllLocation = SelectedNodesText(CheckedTreeComboLocation) == "All Location" ? true : false;
            RptStockMovement.GenerateReport();
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (RptStockMovement.Type == StockMovementReportFilterType.BYITEM)
            {
                GridviewForByItem.Rows.Clear();
                if (RptStockMovement.StockMovementByItems != null && RptStockMovement.StockMovementByItems.Count > 0)
                {
                    EnableButtons(true);
                    foreach (StockMovementByItemReportLine LineItem in RptStockMovement.StockMovementByItems)
                    {
                        irow = GridviewForByItem.Rows.Add();
                        GridviewForByItem.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridviewForByItem.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridviewForByItem.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.SNO].Value = irow + 1;
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.CODE].Value = LineItem.Code;
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.NAME].Value = LineItem.Name;
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.BATCH].Value = LineItem.BatchNo;
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.EXP_DATE].Value = string.IsNullOrEmpty(LineItem.BatchNo) ? string.Empty : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.QUANTITY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridviewForByItem.Rows[irow].Cells[(int)StockMovementReportByItemtTableColumn.ID].Value = LineItem.Id;
                    }
                }
                else
                {
                    StckMvtRptErrMsg.Text = InformationMsg;
                }
            }
            else
            {
                GridviewForByLocation.Rows.Clear();
                GridviewForByDate.Rows.Clear();
                if (RptStockMovement.StockMovementByDate != null && RptStockMovement.StockMovementByDate.Count > 0)
                {
                    EnableButtons(true);
                    if (RptStockMovement.Type == StockMovementReportFilterType.BYDATE)
                    {
                        GridviewForByDate.Rows.Clear();
                        String dateTime = null;
                        long i = 0L;
                        foreach (StockMovementReportLine LineItem in RptStockMovement.StockMovementByDate.OrderBy(x => x.Date))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            irow = GridviewForByDate.Rows.Add();

                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.SNO].Value = i + 1;
                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }
                            GridviewForByDate.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewForByDate.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewForByDate.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.REF].Value = LineItem.Reference;
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.SOURCE].Value = LineItem.Source;
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.DESTINATION].Value = LineItem.Destination;
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.ID].Value = LineItem.Id;
                            GridviewForByDate.Rows[irow].Cells[(int)StockMovementReportByDateTableColumn.MOVEDBY].Value = LineItem.MovedBy;
                            i++;
                        }
                    }
                    else
                    {
                        GridviewForByLocation.Rows.Clear();
                        string Location = string.Empty;
                        String Date = null;
                        int i = 0;
                        foreach (StockMovementReportLine LineItem in RptStockMovement.StockMovementByDate.OrderBy(x => x.Source))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            if (Location == string.Empty || Location != LineItem.Source)
                            {
                                irow = GridviewForByLocation.Rows.Add();
                                GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.SNO].Value = LineItem.Source;
                                Location = LineItem.Source;
                                i = 1;
                            }
                            irow = GridviewForByLocation.Rows.Add();
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.SNO].Value = i;
                            if (Date == null || Date != stringLineItemDate)
                            {
                                GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                Date = stringLineItemDate;
                            }
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.REF].Value = LineItem.Reference;
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.DESTINATION].Value = LineItem.Destination;
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.QTY].Value = LineItem.Qty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.ID].Value = LineItem.Id;
                            GridviewForByLocation.Rows[irow].Cells[(int)StockMovementReportByLocationTableColumn.MOVEDBY].Value = LineItem.MovedBy;
                            i++;
                        }
                    }
                }
                else
                {
                    StckMvtRptErrMsg.Text = InformationMsg;
                }
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
        private void BtnSMRCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadCombo();
            this.Text = "Stock Movement Report";
            ComboBoxType.SelectedIndex = 0;
        }

        private void BtnSMRExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridviewForByLocation_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewForByLocation.Rows[e.RowIndex].Cells[(int)StockMovementReportByLocationTableColumn.ID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top, this.GridviewForByLocation.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                this.GridviewForByLocation.HorizontalScrollingOffset, e.RowBounds.Height);
                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString("", drawFont, drawBrush, rowBounds);
            }
        }

        private void GridviewForByLocation_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewForByLocation.Rows[e.RowIndex].Cells[(int)StockMovementReportByLocationTableColumn.ID].Value == null)
            {
                if (e.ColumnIndex == (int)StockMovementReportByLocationTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)StockMovementReportByLocationTableColumn.MOVEDBY)
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSMRSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnSMRPrint.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                BtnSMRReset.PerformClick();
                return true;
            }
            else if (keyData == Keys.F10)
            {
                BtnSMRExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnSMRSave_Click(object sender, EventArgs e)
        {
            StockMovementReportSavePrint stockMovementReportSavePrint = new StockMovementReportSavePrint();
            stockMovementReportSavePrint.ExportOrPrintToFile(RptStockMovement, "StockMovementReport", "pdf", false);
        }
        private void BtnSMRPrint_Click(object sender, EventArgs e)
        {
            StockMovementReportSavePrint stockMovementReportSavePrint = new StockMovementReportSavePrint();
            stockMovementReportSavePrint.ExportOrPrintToFile(RptStockMovement, "StockMovementReport", "pdf", true);
        }
    }
}
