using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using fa;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.views;
using fa.views.account.masters;
using fa.views.catalog;
using Fa.api.catalog;
using Fa.api.Hms;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.DirectShowLib.MultimediaStreaming;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.MediaFramework.Helpers;
using Color = System.Drawing.Color;

namespace Fa.views.catalog
{
    enum CatalogTaxTableColumn
    {
        NAME, PERCENT, FROM, TO, REMOVE, MAPID, ID
    }
    public partial class FormCatalogTax : FormBase
    {
        public long CatalogItemId;
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveSuccessText = "Save success.";
        public static string EnterDateErrorMsg = "Please enter valid date";
        public static string EnterFromDateErrorMsg = "Effective from date alredy exists for tax percentage {0}.";
        public static string EnterToDateErrorMsg = "Effective to date alredy exists for tax percentage {0}.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_InvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_InvalidFromDateErrorMsg = "Please check the effective fom date, {0} date used by other tax.";
        public static string Grid_InvalidToDateErrorMsg = "Please check the effective to date, {0} date used by other tax.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_RowDeleteText = "Row removed success.";
        public static string Grid_RowNotDeleteText = "You do not delete this row, Its used in somewhere else.";
        public static string Grid_InvalidDateErrorMsg = "Effective from date not exceed effective to date.";
        public static string Grid_TaxPercentErrorMsg = "Please enter valid tax,Tax percentage not exceed 49%.";

        public FormCatalogTax()
        {
            InitializeComponent();
        }
        private void FormCatalogTax_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadCatalogItemsTax();
        }
        private void LoadCatalogItemsTax()
        {
            int HistoryRowIndex = 1;
            int ActiveRowIndex = -1;
            CatalogItem catalogItem = CatalogItemManager.Instance.GetCatalogItemInfoById(CatalogItemId);
            if (catalogItem != null)
            {
                TextBoxCatalogName.Text = catalogItem.Name;
                List<CatalogItemSalesTaxMap> lCatalogItemSalesTaxMap = CatalogItemManager.Instance.GetCatalogItemSalesTaxMapInfoById(CatalogItemId);
                if (lCatalogItemSalesTaxMap != null && lCatalogItemSalesTaxMap.Count > 0)
                {
                    foreach (CatalogItemSalesTaxMap Map in lCatalogItemSalesTaxMap)
                    {
                        if ((DateTime)DateUtils.ToDate(Map.EffectiveFrom.ToString(Global.Company.DateFormat), Global.Company.DateFormat) <= (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat) && (DateTime)DateUtils.ToDate(Map.EffectiveTo.ToString(Global.Company.DateFormat), Global.Company.DateFormat) >= (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                        {
                            ActiveRowIndex = DataGridViewCurrentTax.Rows.Add();
                            (DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = null;
                            (DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = Global.Company.SalesTaxAccountMaps;
                            (DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).ValueMember = "MapId";
                            (DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DisplayMember = "Name";
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.NAME].Value = Map.SalesTaxMapId;
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.PERCENT].Value = Map.TaxPercentage;
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.FROM].Value = Map.EffectiveFrom;
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.TO].Value = Map.EffectiveTo;
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.MAPID].Value = Map.SalesTaxMapId;
                            DataGridViewCurrentTax.Rows[ActiveRowIndex].Cells[(int)CatalogTaxTableColumn.ID].Value = Map.Id;
                        }
                        else
                        {
                            HistoryRowIndex = DataGridViewTaxHistory.Rows.Add();
                            DataGridViewTaxHistory.Rows[HistoryRowIndex].Cells[(int)CatalogTaxTableColumn.NAME].Value = Map.CompanySalesTaxAccountMap.Name;
                            DataGridViewTaxHistory.Rows[HistoryRowIndex].Cells[(int)CatalogTaxTableColumn.PERCENT].Value = Map.TaxPercentage;
                            DataGridViewTaxHistory.Rows[HistoryRowIndex].Cells[(int)CatalogTaxTableColumn.FROM].Value = Map.EffectiveFrom;
                            DataGridViewTaxHistory.Rows[HistoryRowIndex].Cells[(int)CatalogTaxTableColumn.TO].Value = Map.EffectiveTo;
                        }
                    }
                }
            }
            this.formIsDirty = false;
        }
        private void ResetForm()
        {
            TextBoxCatalogName.ResetText();
            DataGridViewTaxHistory.Rows.Clear();
            DataGridViewCurrentTax.Rows.Clear();
        }

        private List<CatalogItemSalesTaxMap> CatalogSaleTaxMapFromFrom()
        {
            List<CatalogItemSalesTaxMap> catalogItemSalesTaxMaps = new List<CatalogItemSalesTaxMap>();
            foreach (DataGridViewRow Row in DataGridViewCurrentTax.Rows)
            {
                if (Row.Index < DataGridViewCurrentTax.Rows.Count - 1)
                {
                    CatalogItemSalesTaxMap map = new CatalogItemSalesTaxMap();
                    map.SalesTaxMapId = Row.Cells[(int)CatalogTaxTableColumn.NAME].Value == null ? 0 : (long)Row.Cells[(int)CatalogTaxTableColumn.NAME].Value;
                    map.CatalogItemId = CatalogItemId;
                    map.TaxPercentage = Row.Cells[(int)CatalogTaxTableColumn.PERCENT].Value != null ? float.Parse(Row.Cells[(int)CatalogTaxTableColumn.PERCENT].Value.ToString()) : 0;
                    map.EffectiveFrom = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.FROM].Value;
                    map.EffectiveTo = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.TO].Value;
                    map.Id = Row.Cells[(int)CatalogTaxTableColumn.ID].Value != null ? (long)Row.Cells[(int)CatalogTaxTableColumn.ID].Value : 0L;
                    catalogItemSalesTaxMaps.Add(map);
                }
            }
            return catalogItemSalesTaxMaps;

        }
        private void BtnItemTaxSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                List<CatalogItemSalesTaxMap> catalogItemSalesTaxMaps = CatalogSaleTaxMapFromFrom();
                if (catalogItemSalesTaxMaps.Count > 0)
                {
                    CatalogItemManager.Instance.AddCatalogTax(catalogItemSalesTaxMaps, CatalogItemId);
                    ErrorMsgItemTax.Text = SaveSuccessText;
                    TextBoxCatalogName.Select();
                    ResetForm();
                    LoadCatalogItemsTax();
                    this.formIsDirty = false;
                }
            }
        }
        private bool ValidateForm()
        {
            if (DataGridViewCurrentTax.RowCount > 1)
            {
                foreach (DataGridViewRow Row in DataGridViewCurrentTax.Rows)
                {
                    if (Row.Index < DataGridViewCurrentTax.Rows.Count - 1)
                    {
                        for (int i = 0; i < DataGridViewCurrentTax.ColumnCount - 4; i++)
                        {
                            if (Row.Cells[i].Value == null || string.IsNullOrEmpty(Row.Cells[i].Value.ToString()))
                            {
                                DataGridViewCurrentTax.Select();
                                DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[i, Row.Index];
                                DataGridViewCurrentTax.BeginEdit(true);
                                ErrorMsgItemTax.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewCurrentTax.Columns[i].HeaderText);
                                return false;
                            }
                            if (i == (int)CatalogTaxTableColumn.PERCENT && Global.Company.Country.Name == "India" && (double.Parse(Row.Cells[i].Value.ToString()) > 49))
                            {
                                DataGridViewCurrentTax.Select();
                                DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[i, Row.Index];
                                DataGridViewCurrentTax.BeginEdit(true);
                                DataGridViewCurrentTax.CurrentCell.Selected = true;
                                ErrorMsgItemTax.Text = Grid_TaxPercentErrorMsg;
                                return false;
                            }
                            if ((i == (int)CatalogTaxTableColumn.FROM || i == (int)CatalogTaxTableColumn.TO))
                            {
                                if (Row.Cells[i].Value == null || !DateUtils.ValidDate(((DateTime)Row.Cells[i].Value).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                                {
                                    DataGridViewCurrentTax.Select();
                                    DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[i, Row.Index];
                                    DataGridViewCurrentTax.BeginEdit(true);
                                    ErrorMsgItemTax.Text = string.Format(Grid_InvalidDataErrorMsg, DataGridViewCurrentTax.Columns[i].HeaderText);
                                    return false;
                                }
                            }
                        }
                    }
                }
                foreach (DataGridViewRow Row in DataGridViewCurrentTax.Rows)
                {
                    if (Row.Index < DataGridViewCurrentTax.Rows.Count - 1)
                    {
                        long Id =  Row.Cells[(int)CatalogTaxTableColumn.ID].Value == null || string.IsNullOrEmpty(Row.Cells[(int)CatalogTaxTableColumn.ID].Value.ToString()) ? 0 : long.Parse(Row.Cells[(int)CatalogTaxTableColumn.ID].Value.ToString());
                        long MapId = long.Parse(Row.Cells[(int)CatalogTaxTableColumn.MAPID].Value.ToString()) == null ? 0 : long.Parse(Row.Cells[(int)CatalogTaxTableColumn.MAPID].Value.ToString());
                        DateTime FromDate = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.FROM].Value;
                        DateTime ToDate = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.TO].Value;
                        if (FromDate > ToDate)
                        {
                            DataGridViewCurrentTax.Select();
                            DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[(int)CatalogTaxTableColumn.FROM, Row.Index];
                            DataGridViewCurrentTax.BeginEdit(true);
                            ErrorMsgItemTax.Text = Grid_InvalidDateErrorMsg;
                            return false;
                        }
                        foreach (DataGridViewRow lRow in DataGridViewCurrentTax.Rows)
                        {
                            if (lRow.Index < DataGridViewCurrentTax.Rows.Count - 1)
                            {

                                long lMapId = long.Parse(lRow.Cells[(int)CatalogTaxTableColumn.MAPID].Value.ToString()) == null ? 0 : long.Parse(lRow.Cells[(int)CatalogTaxTableColumn.MAPID].Value.ToString());
                                DateTime lFromDate = (DateTime)lRow.Cells[(int)CatalogTaxTableColumn.FROM].Value;
                                DateTime lToDate = (DateTime)lRow.Cells[(int)CatalogTaxTableColumn.TO].Value;
                                if (Row.Index != lRow.Index)
                                {
                                    if (MapId == lMapId && (FromDate <= lFromDate && ToDate >= lFromDate))
                                    {
                                        DataGridViewCurrentTax.Select();
                                        DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[(int)CatalogTaxTableColumn.FROM, lRow.Index];
                                        DataGridViewCurrentTax.BeginEdit(true);
                                        ErrorMsgItemTax.Text = string.Format(Grid_InvalidFromDateErrorMsg, DataGridViewCurrentTax.Columns[(int)CatalogTaxTableColumn.FROM].HeaderText);
                                        return false;
                                    }
                                    if (MapId == lMapId && (FromDate <= lToDate && ToDate >= lToDate))
                                    {
                                        DataGridViewCurrentTax.Select();
                                        DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[(int)CatalogTaxTableColumn.TO, lRow.Index];
                                        DataGridViewCurrentTax.BeginEdit(true);
                                        ErrorMsgItemTax.Text = string.Format(Grid_InvalidToDateErrorMsg, DataGridViewCurrentTax.Columns[(int)CatalogTaxTableColumn.TO].HeaderText);
                                        return false;
                                    }
                                }
                            }
                        }
                        if (CatalogItemManager.Instance.CheckTaxMapHaveSameFromDate(MapId, FromDate, CatalogItemId,Id))
                        {
                            DataGridViewCurrentTax.Select();
                            DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[(int)CatalogTaxTableColumn.FROM, Row.Index];
                            DataGridViewCurrentTax.BeginEdit(true);
                            ErrorMsgItemTax.Text = string.Format(Grid_InvalidFromDateErrorMsg, DataGridViewCurrentTax.Columns[(int)CatalogTaxTableColumn.FROM].HeaderText);
                            return false;
                        }
                        if (CatalogItemManager.Instance.CheckTaxMapHaveSameToDate(MapId, ToDate, CatalogItemId,Id))
                        {
                            DataGridViewCurrentTax.Select();
                            DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[(int)CatalogTaxTableColumn.TO, Row.Index];
                            DataGridViewCurrentTax.BeginEdit(true);
                            ErrorMsgItemTax.Text = string.Format(Grid_InvalidToDateErrorMsg, DataGridViewCurrentTax.Columns[(int)CatalogTaxTableColumn.TO].HeaderText);
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        private void BtnItemTaxExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCatalogTax_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DataGridViewCurrentTax.Focus();
                    e.Cancel = true;
                }
            }
        }

        private void DataGridViewCurrentTax_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.REMOVE].ReadOnly = true;
                if (e.RowIndex < DataGridViewCurrentTax.Rows.Count - 1)
                {
                    if (DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value != null)
                    {
                        long ItemMapId = (long)DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value;
                        if (CatalogItemManager.Instance.CheckTaxMapHaveEntries(ItemMapId))
                        {
                            DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.NAME].ReadOnly = true;
                            DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.PERCENT].ReadOnly = true;
                            DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.FROM].ReadOnly = true;
                            if (DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].Value != null && DateUtils.ValidDate(((DateTime)DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].Value).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                            {
                                if (CatalogItemManager.Instance.CheckTaxMapHaveOtherEntryAfterToDate((long)DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.NAME].Value, (DateTime)DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].Value))
                                {
                                    DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].ReadOnly = true;
                                }
                            }
                        }
                    }
                }
            }
            if (e.ColumnIndex == (int)CatalogTaxTableColumn.NAME)
            {
                DataGridViewCurrentTax.CurrentCell.ReadOnly = false;
            }
        }

        private void DataGridViewCurrentTax_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (Global.Company.SalesTaxAccountMaps != null)
            {
                DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.PERCENT].Value = "0.00";
                DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.REMOVE].Value = "X";
                (DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = Global.Company.SalesTaxAccountMaps;
                (DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).ValueMember = "MapId";
                (DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.NAME] as DataGridViewComboBoxCell).DisplayMember = "Name";
                DataGridViewCurrentTax.Rows[e.RowIndex].Cells[(int)CatalogTaxTableColumn.TO].Value = Global.getTransactionDate().AddYears(2400 - Global.getTransactionDate().Year);
            }
        }

        private void DataGridViewCurrentTax_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)CatalogTaxTableColumn.REMOVE && (DataGridViewCurrentTax.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, (e.RowIndex + 1).ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        long ItemMapId = DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value != null ? (long)DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value : 0L;
                        if (ItemMapId == 0L)
                        {
                            DataGridViewCurrentTax.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            DataGridViewCurrentTax.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            if (CatalogItemManager.Instance.CheckTaxMapHaveEntries(ItemMapId))
                            {
                                MessageBox.Show(Grid_RowNotDeleteText);
                            }
                            else
                            {
                                if (CatalogItemManager.Instance.DeleteCatalogTax(ItemMapId))
                                {
                                    DataGridViewCurrentTax.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    DataGridViewCurrentTax.Rows.RemoveAt(e.RowIndex);
                                    ErrorMsgItemTax.Text = Grid_RowDeleteText;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DataGridViewCurrentTax_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DataGridViewTaxHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void DataGridViewCurrentTax_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).FormattingEnabled = true;

                if (DataGridViewCurrentTax.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                else
                {
                    if (DataGridViewCurrentTax.CurrentCell.FormattedValue != null)
                    {
                        List<CompanySalesTaxAccountMap> MapTax = Global.Company.SalesTaxAccountMaps.ToList();
                        CompanySalesTaxAccountMap companySalesTaxMap = MapTax.FirstOrDefault(x => x.Name == DataGridViewCurrentTax.CurrentCell.FormattedValue.ToString());
                        if (companySalesTaxMap == null)
                        {
                            ((ComboBox)e.Control).SelectedIndex = -1;
                            ((ComboBox)e.Control).Text = DataGridViewCurrentTax.CurrentCell.Value.ToString();
                        }
                    }
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewCurrentTax_KeyPress1);

            }
        }
        private void DataGridViewCurrentTax_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewCurrentTax.EditingControl).DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnItemTaxSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.F10))
            {
                BtnItemTaxExit.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnItemTaxExit)
            {
                TextBoxCatalogName.Select();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxCatalogName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (DataGridViewCurrentTax.Rows.Count > 1 && DataGridViewCurrentTax.Rows != null)
                {
                    e.IsInputKey = true;
                    DataGridViewCurrentTax.Select();
                    DataGridViewCurrentTax.CurrentCell = DataGridViewCurrentTax[0, 0];
                }
            }
        }
        private void DataGridViewCurrentTax_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (!string.IsNullOrEmpty(DataGridViewCurrentTax.CurrentCell.EditedFormattedValue.ToString().Trim()))
                {
                    DataGridViewCurrentTax.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    List<CompanySalesTaxAccountMap> MapTax = Global.Company.SalesTaxAccountMaps.ToList();
                    CompanySalesTaxAccountMap OutputSalesTaxMap = MapTax.FirstOrDefault(x => x.Name == DataGridViewCurrentTax.CurrentCell.EditedFormattedValue.ToString());
                    if (OutputSalesTaxMap == null)
                    {
                        DataGridViewCurrentTax.CurrentCell.Value = DataGridViewCurrentTax.CurrentCell.EditedFormattedValue.ToString();
                    }
                    else
                    {
                        DataGridViewCurrentTax.CurrentCell.Value = OutputSalesTaxMap.MapId;
                        DataGridViewCurrentTax.CurrentRow.Cells[(int)CatalogTaxTableColumn.MAPID].Value = OutputSalesTaxMap.MapId;
                    }
                }
            }
        }

        private void DataGridViewCurrentTax_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
    }
}
