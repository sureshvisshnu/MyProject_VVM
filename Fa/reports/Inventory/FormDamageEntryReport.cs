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
    enum DamageEntryReportByDateTableColumn
    {
        SNO, DATE, REFERENCE, LOCATION, QTY, FREE, ENTEREDBY, ID
    }
    enum DamageEntryReportByItemTableColumn
    {
        SNO, CODE, NAME, BATCH, EXP_DATE, QTY, FREE, ID
    }
    enum DamageEntryReportByLocationTableColumn
    {
        SNO, DATE, REFERENCE, QTY, FREE, ENTEREDBY, ID
    }
    public partial class FormDamageEntryReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectreferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public long LocationId = 0L;
        RptDamageEntry ReportDamageEntry = null;
        public FormDamageEntryReport()
        {
            InitializeComponent();
        }
        private void FormDamageEntryReport_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadCombo();
            ResetForm();
            LabelLocation.Visible = false;
            CheckedTreeComboLocation.Visible = false;
            Cursor.Current = Cursors.Default;
        }
        public void ResetForm()
        {
            GridviewForByItem.Rows.Clear();
            GridviewForByDate.Rows.Clear();
            GridviewForByLocation.Rows.Clear();
            DamageEntryErrMsg.Text = "";
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
            DamageBtnSave.Enabled = Enable;
            DamageBtnPrint.Enabled = Enable;
            ToolStripPrint.Enabled = Enable;
            ToolStripSave.Enabled = Enable;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeStockLocationCombo(CheckedTreeComboLocation, Global.Company.CompanyId);
        }
        private void damageEntryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            EnableButtons(false);
            DamageEntryErrMsg.Text = "";
            GridviewForByItem.Rows.Clear();
            GridviewForByDate.Rows.Clear();
            GridviewForByLocation.Rows.Clear();
            if (damageEntryComboBoxType.SelectedIndex == 0)
            {
                GridviewForByItem.Visible = false;
                GridviewForByDate.Visible = true;
                GridviewForByLocation.Visible = false;
                LabelLocation.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                this.Text = "Damage Entry Report";
            }
            else if (damageEntryComboBoxType.SelectedIndex == 1)
            {
                GridviewForByItem.Visible = true;
                GridviewForByDate.Visible = false;
                GridviewForByLocation.Visible = false;
                LabelLocation.Visible = false;
                CheckedTreeComboLocation.Visible = false;
                this.Text = "Damage Entry Report";
            }
            else if (damageEntryComboBoxType.SelectedIndex == 2)
            {
                CheckedTreeComboLocation.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboLocation.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                GridviewForByLocation.Visible = true;
                LabelLocation.Visible = true;
                CheckedTreeComboLocation.Visible = true;
                GridviewForByItem.Visible = false;
                GridviewForByDate.Visible = false;
                this.Text = "Damage Entry Report";
            }
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridviewForByDate.Rows.Clear();
                EnableButtons(false);
                if (FormValidate())
                {
                    LoadDamageEntryReport();
                }
            }
            catch (Exception ex)
            {
                DamageEntryErrMsg.Text = "Error fetching Damage Entry (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool FormValidate()
        {
            DamageEntryErrMsg.Text = "";
            if (damageEntryComboBoxType.SelectedIndex < 0)
            {
                DamageEntryErrMsg.Text = "Please select type";
                damageEntryComboBoxType.Select();
                return false;
            }
            if (damageEntryComboBoxType.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).Count < 1)
            {
                DamageEntryErrMsg.Text = "Please select location";
                CheckedTreeComboLocation.Focus();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DamageEntryErrMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DamageEntryErrMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if(FromDate.Date > ToDate.Date)
            {
                DamageEntryErrMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            return true;
        }
        private void LoadDamageEntryReport()
        {
            ReportDamageEntry = new RptDamageEntry();
            ReportDamageEntry.FromDate = (DateTime)FromDate.Date;
            ReportDamageEntry.ToDate = (DateTime)ToDate.Date;
            ReportDamageEntry.Company = Global.Company;
            ReportDamageEntry.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboLocation);
            ReportDamageEntry.Type = damageEntryComboBoxType.SelectedIndex == 0 ? DamageEntryReportFilterType.BYDATE : damageEntryComboBoxType.SelectedIndex == 1 ? DamageEntryReportFilterType.BYITEM : DamageEntryReportFilterType.BYLOCATION;
            ReportDamageEntry.LocationIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboLocation).ToArray();
            ReportDamageEntry.Location = this.Text;
            ReportDamageEntry.IsAllLocation = SelectedNodesText(CheckedTreeComboLocation) == "All Location" ? true : false;
            ReportDamageEntry.GenerateReport();
            int irow = 0;
            int j = 2;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            if (ReportDamageEntry.Type == DamageEntryReportFilterType.BYITEM)
            {
                GridviewForByItem.Rows.Clear();
                if (ReportDamageEntry.DamageEntryByItems != null && ReportDamageEntry.DamageEntryByItems.Count > 0)
                {
                    EnableButtons(true);
                    foreach (DamageEntryByItemReportLine LineItem in ReportDamageEntry.DamageEntryByItems)
                    {
                        irow = GridviewForByItem.Rows.Add();
                        GridviewForByItem.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridviewForByItem.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridviewForByItem.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.SNO].Value = irow + 1;
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.CODE].Value = LineItem.Code;
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.NAME].Value = LineItem.Name;
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.BATCH].Value = LineItem.BatchNo;
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.EXP_DATE].Value = string.IsNullOrEmpty(LineItem.BatchNo) ? string.Empty : LineItem.ExpDate.ToString(Global.Company.DateFormat);
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.QTY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        GridviewForByItem.Rows[irow].Cells[(int)DamageEntryReportByItemTableColumn.ID].Value = LineItem.Id;
                    }
                }
                else
                {
                    DamageEntryErrMsg.Text = InformationMsg;
                }
            }
            else
            {
                GridviewForByDate.Rows.Clear();
                GridviewForByLocation.Rows.Clear();
                if (ReportDamageEntry.DamageEntry != null && ReportDamageEntry.DamageEntry.Count > 0)
                {
                    EnableButtons(true);
                    if (ReportDamageEntry.Type == DamageEntryReportFilterType.BYDATE)
                    {
                        GridviewForByDate.Rows.Clear();
                        String dateTime = null;
                        long Sno = 0L;
                        foreach (DamageEntryReportLine LineItem in ReportDamageEntry.DamageEntry.OrderBy(x => x.Date))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            irow = GridviewForByDate.Rows.Add();

                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.SNO].Value = Sno + 1;
                            if (dateTime == null || dateTime != stringLineItemDate)
                            {
                                GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                dateTime = stringLineItemDate;
                            }
                            GridviewForByDate.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewForByDate.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewForByDate.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.REFERENCE].Value = LineItem.Reference;
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.LOCATION].Value = LineItem.Location;
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.QTY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.ENTEREDBY].Value = LineItem.EnteredBy;
                            GridviewForByDate.Rows[irow].Cells[(int)DamageEntryReportByDateTableColumn.ID].Value = LineItem.Id;
                            Sno++;
                        }
                    }
                    else
                    {
                        GridviewForByLocation.Rows.Clear();
                        string Location = string.Empty;
                        int i = 0;
                        String Date = null;
                        foreach (DamageEntryReportLine LineItem in ReportDamageEntry.DamageEntry.OrderBy(x => x.Location))
                        {
                            String stringLineItemDate = DateUtils.FormatDate(LineItem.Date, Global.Company.DateFormat);
                            if (Location == string.Empty || Location != LineItem.Location)
                            {
                                irow = GridviewForByLocation.Rows.Add();
                                GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.SNO].Value = LineItem.Location;
                                Location = LineItem.Location;
                                i = 1;
                            }
                            irow = GridviewForByLocation.Rows.Add();
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewForByLocation.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;

                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.SNO].Value = i;
                            if (Date == null || Date != stringLineItemDate)
                            {
                                GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.DATE].Value = LineItem.Date.ToString(Global.Company.DateFormat);
                                Date = stringLineItemDate;
                            }
                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.REFERENCE].Value = LineItem.Reference;
                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.QTY].Value = LineItem.Quantity.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.FREE].Value = LineItem.Free.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.ENTEREDBY].Value = LineItem.EnteredBy;
                            GridviewForByLocation.Rows[irow].Cells[(int)DamageEntryReportByLocationTableColumn.ID].Value = LineItem.Id;
                            i++;
                        }
                    }
                }
                else
                {
                    DamageEntryErrMsg.Text = InformationMsg;
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
        private void DamageBtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DamageBtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadCombo();
            this.Text = "Damage Entry Report";
            damageEntryComboBoxType.SelectedIndex = 0;
        }

        private void ComboDamEryRptLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
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
                this.Text = "Damage Entry Report " + " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Damage Entry Report";
            }
        }
        private void GridviewForByLocation_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridviewForByLocation.Rows[e.RowIndex].Cells[(int)DamageEntryReportByLocationTableColumn.ID].Value == null && e.RowIndex > -1)
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
            if (e.RowIndex > -1 && GridviewForByLocation.Rows[e.RowIndex].Cells[(int)DamageEntryReportByLocationTableColumn.ID].Value == null)
            {
                if (e.ColumnIndex == (int)DamageEntryReportByLocationTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)DamageEntryReportByLocationTableColumn.ENTEREDBY)
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
                DamageBtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                DamageBtnPrint.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                DamageBtnReset.PerformClick();
                return true;
            }
            else if (keyData == Keys.F10)
            {
                DamageBtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DamageBtnSave_Click(object sender, EventArgs e)
        {
            DamageEntryReportSavePrint damageEntryReportSavePrint = new DamageEntryReportSavePrint();
            damageEntryReportSavePrint.ExportOrPrintToFile(ReportDamageEntry, "DamageEntryReport", "pdf", false);
        }

        private void DamageBtnPrint_Click(object sender, EventArgs e)
        {
            DamageEntryReportSavePrint damageEntryReportSavePrint = new DamageEntryReportSavePrint();
            damageEntryReportSavePrint.ExportOrPrintToFile(ReportDamageEntry, "DamageEntryReport", "pdf", true);
        }
    }
}
