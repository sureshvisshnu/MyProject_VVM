using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using fa.reports.catalog;
using fa.views.controls;
using fa.views.hms.masters.upload;
using fa.views.hms.Masters;
using fa.views.utils.Report.Catalog;
using Fa.api.catalog;
using Fa.views.catalog;
using FADataAccessLibrary.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.MediaFramework.Helpers;
using Color = System.Drawing.Color;

namespace fa.views.catalog
{
    public enum ItemTaxTableColumn
    {
        CODE, DESC, ID
    }
    public enum TaxTableColumn
    {
        TAX, PERCENT, EFFECTIVEFROM, EFFECTIVETO, REMOVE, MAPID, ID
    }
    public enum ActiveTaxTableColumn
    {
        TAX, PERCENT, EFFECTIVEFROM, EFFECTIVETO, REMOVE, MAPID, ID
    }
    public partial class FormTaxCode : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string DeleteConfirmText = "Do you want to delete the Tax Code {0}?";
        public static string DeleteErrorText = "Error Deleting the Tax Code!, Please retry";
        public static string NotAllowDeleteErrorText = "Do not Delete Tax code it used in catalog";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string EnterCodeErrorMsg = "Please enter tax code";
        public static string PercentageConfirmText = "Any one Active Tax should filled with percentage value";
        public static string DateConfirmText = "Already Tax included for this Tax Code {0} on the same date";
        public static string DeleteSuccessMsg = "Deleted";
        public static string UpdateSuccessMsg = "Updated";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_RowDeleteText = "Row removed success.";
        public static string Grid_RowNotDeleteText = "You do not delete this row, Its used in somewhere else.";
        public static string Grid_InvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_InvalidFromDateErrorMsg = "Please check the effective from date of {0}, date cannot overlap with other entries.";
        public static string Grid_InvalidToDateErrorMsg = "Please check the effective to date of {0}, date cannot overlap with other entries.";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_InvalidDateErrorMsg = "Effective from date not exceed effective to date.";
        public static string UniqueTaxCodeErrorMsg = "Tax code {0} already exists in company {1}";
        public static string UpdateTaxCodeOnloadText = "Update Tax Code";
        public static string Grid_TaxPercentErrorMsg = "Please enter valid tax,Tax percentage not exceed 49%.";
        public static string GridTaxCodeNameErrorMsg = "Tax code not selected properly re-select}";

        ItemTaxManager ItemTaxManager = null;
        FormBase parent = null;
        public bool UpdateTaxCodeOnLoad = false;
        public long TaxCodeId = 0L;
        public FormTaxCode()
        {
            InitializeComponent();
            ItemTaxManager = ItemTaxManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "GridViewTaxCode" };
        }

        private void FormTaxCode_Load(object sender, EventArgs e)
        {
            ResetForm();
            ListTaxCodes();
            EnableForm(false);
            if (UpdateTaxCodeOnLoad)
            {
                BtnTaxCodeReport.Visible = false;
                BtnTaxCodeExport.Visible = false;
                BtnTaxCodeImport.Visible = false;
                this.BtnTaxCodeEdit.Visible = false;
                BtnTaxCodeCancel.Visible = false;
                BtnTaxCodeNew.Visible = false;
                BtnTaxCodeDelete.Visible = false;
                GridViewTaxCodeList.Visible = false;
                TabControlTaxCode.Location = new Point(12, 12);
                BtnTaxCodeSave.Location = new Point(TabControlTaxCode.Width - 74, TabControlTaxCode.Height + 15);
                this.Size = new Size(TabControlTaxCode.Right + 25, TabControlTaxCode.Bottom + 90);
                this.CenterToParent();
                this.Text = UpdateTaxCodeOnloadText;
                TextBoxTaxCodeId.Text = TaxCodeId.ToString();
                BtnTaxCodeEdit_Click(this, null);
            }
            this.formIsDirty = false;
        }
        private void ListTaxCodes()
        {
            GridViewTaxCodeList.Rows.Clear();
            IList<ItemTax> ItemTaxInfo = new List<ItemTax>();
            if (string.IsNullOrEmpty(TextBoxTaxCodeSearch.Text.Trim()))
            {
                ItemTaxInfo = ItemTaxManager.GetItemTaxs(Global.Company.CompanyId);
            }
            else
            {
                ItemTaxInfo = ItemTaxManager.GetItemTaxs(Global.Company.CompanyId, TextBoxTaxCodeSearch.Text);
            }
            if (ItemTaxInfo.Count > 0)
            {
                int i = 0;
                GridViewTaxCodeList.Rows.Add(ItemTaxInfo.Count);
                foreach (ItemTax Tax in ItemTaxInfo)
                {
                    GridViewTaxCodeList.Rows[i].Cells[(int)ItemTaxTableColumn.ID].Value = Tax.Id;
                    GridViewTaxCodeList.Rows[i].Cells[(int)ItemTaxTableColumn.CODE].Value = Tax.Code;
                    GridViewTaxCodeList.Rows[i].Cells[(int)ItemTaxTableColumn.DESC].Value = Tax.Description;
                    i++;
                }
                GridViewTaxCodeList.CurrentCell = GridViewTaxCodeList[(int)ItemTaxTableColumn.CODE, 0];
                LoadItemTax();
            }
            else
            {
                BtnTaxCodeNew.Select();
            }
        }
        private void LoadItemTax()
        {
            if (GridViewTaxCodeList.CurrentRow != null && GridViewTaxCodeList.CurrentRow.Cells[(int)ItemTaxTableColumn.ID].Value != null)
            {
                ItemTax ItemTax = ItemTaxManager.GetItemTaxCodeById((long)GridViewTaxCodeList.CurrentRow.Cells[(int)ItemTaxTableColumn.ID].Value);
                if (ItemTax != null)
                {
                    TextBoxTaxCodeCode.Text = ItemTax.Code;
                    TextBoxTaxCodeDetail.Text = ItemTax.Description;
                    TextBoxTaxCodeId.Text = ItemTax.Id.ToString();
                    if (ItemTax.SalesTaxMapLocal.Count > 0)
                    {
                        LoadTaxDetails(ItemTax.SalesTaxMapLocal.ToList());
                    }
                    EnableForm(false);
                    this.formIsDirty = false;
                }
            }
        }        
        private void LoadTaxDetails(IList<ItemSalesTaxMap> lItemSalesTaxMap)
        {
            DataGridViewTaxCodeCurrentTaxCode.Rows.Clear();
            DataGridViewTaxCodeTaxCodeHistory.Rows.Clear();

            var uniqueDates = new HashSet<DateTime>(); 
            foreach (ItemSalesTaxMap Map in lItemSalesTaxMap)
            {
                uniqueDates.Add(Map.EffectiveFromDate.Date);
            }

            DateTime latestDate = uniqueDates.Max(); 

            foreach (ItemSalesTaxMap Map in lItemSalesTaxMap)
            {
                if (Map.EffectiveFromDate.Date == latestDate )
                {
                    int ActiveRowIndex = DataGridViewTaxCodeCurrentTaxCode.Rows.Add();
                    PopulateDataGridViewRowCurrentTax(DataGridViewTaxCodeCurrentTaxCode.Rows[ActiveRowIndex], Map);
                }
                else
                {
                    int HistoryRowIndex = DataGridViewTaxCodeTaxCodeHistory.Rows.Add();
                    PopulateDataGridViewRowHistoryTax(DataGridViewTaxCodeTaxCodeHistory.Rows[HistoryRowIndex], Map);
                }
            }
        }

        private void PopulateDataGridViewRowCurrentTax(DataGridViewRow row, ItemSalesTaxMap map)
        {
            bool ActiveTaxTableColumnTAX = false;
            var comboBoxCell = DataGridViewTaxCodeCurrentTaxCode.Rows[row.Index].Cells[(int)TaxTableColumn.TAX] as DataGridViewComboBoxCell;
            if (comboBoxCell != null)
            {
                ActiveTaxTableColumnTAX = true;
                comboBoxCell.DataSource = null;
                comboBoxCell.DataSource = null; // Clear the existing data source
                comboBoxCell.DataSource = Global.Company.SalesTaxAccountMaps; // Set the new data source
                comboBoxCell.ValueMember = "MapId";
                comboBoxCell.DisplayMember = "Name";
            }

            if(ActiveTaxTableColumnTAX == true)
            {
                row.Cells[(int)ActiveTaxTableColumn.TAX].Value = map.SalesTaxMapId;
                row.Cells[(int)ActiveTaxTableColumn.PERCENT].Value = map.TaxPercentage;
                row.Cells[(int)ActiveTaxTableColumn.EFFECTIVEFROM].Value = map.EffectiveFromDate;
                row.Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].Value = map.EffectiveToDate;
                row.Cells[(int)ActiveTaxTableColumn.ID].Value = map.Id;
                row.Cells[(int)ActiveTaxTableColumn.MAPID].Value = map.SalesTaxMapId;
            }
        }
        private void PopulateDataGridViewRowHistoryTax(DataGridViewRow row, ItemSalesTaxMap map)
        {
            bool ActiveTaxTableColumnTAX = false;

            var textBoxCell = DataGridViewTaxCodeTaxCodeHistory.Rows[row.Index].Cells[(int)TaxTableColumn.TAX] as DataGridViewTextBoxCell;
            if (textBoxCell != null)
            {
                ActiveTaxTableColumnTAX = true;
                textBoxCell.Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == map.SalesTaxMapId)?.Name;
                row.Cells[(int)TaxTableColumn.TAX].Value = textBoxCell.Value;
            }

            if (ActiveTaxTableColumnTAX)
            {
                row.Cells[(int)TaxTableColumn.PERCENT].Value = map.TaxPercentage;
                row.Cells[(int)TaxTableColumn.EFFECTIVEFROM].Value = map.EffectiveFromDate;
                row.Cells[(int)TaxTableColumn.EFFECTIVETO].Value = map.EffectiveToDate;
                row.Cells[(int)TaxTableColumn.ID].Value = map.Id;
                row.Cells[(int)TaxTableColumn.MAPID].Value = map.SalesTaxMapId;
            }
        }



        private void ResetForm()
        {
            TaxCodeErrorMsg.Text = String.Empty;
            TextBoxTaxCodeId.ResetText();
            TextBoxTaxCodeCode.ResetText();
            TextBoxTaxCodeDetail.ResetText();
            DataGridViewTaxCodeCurrentTaxCode.Rows.Clear();
            DataGridViewTaxCodeTaxCodeHistory.Rows.Clear();
        }
        private void EnableForm(Boolean enable)
        {
            if (GridViewTaxCodeList.Rows.Count > 0)
            {
                GridViewTaxCodeList.Enabled = !enable;
                TextBoxTaxCodeSearch.ReadOnly = enable;
                TextBoxTaxCodeSearch.TabStop = !enable;
            }
            else
            {
                GridViewTaxCodeList.Enabled = false;
                TextBoxTaxCodeSearch.ReadOnly = true;
                TextBoxTaxCodeSearch.TabStop = false;
                BtnTaxCodeNew.Select();
            }
            TextBoxTaxCodeCode.ReadOnly = !enable;
            TextBoxTaxCodeCode.TabStop = enable;
            TextBoxTaxCodeDetail.ReadOnly = !enable;
            TextBoxTaxCodeDetail.TabStop = enable;
            DataGridViewTaxCodeCurrentTaxCode.ReadOnly = !enable;
            DataGridViewTaxCodeCurrentTaxCode.TabStop = enable;
            DataGridViewTaxCodeTaxCodeHistory.ReadOnly = true;
            DataGridViewTaxCodeTaxCodeHistory.TabStop = false;
            if (!enable)
            {
                BtnTaxCodeCancel.Enabled = enable;
                if (string.IsNullOrEmpty(TextBoxTaxCodeId.Text))
                {
                    BtnTaxCodeDelete.Enabled = enable;
                    BtnTaxCodeEdit.Enabled = enable;
                }
                else
                {
                    BtnTaxCodeDelete.Enabled = !enable;
                    BtnTaxCodeEdit.Enabled = !enable;
                }
                BtnTaxCodeNew.Enabled = !enable;
                BtnTaxCodeSave.Enabled = enable;
            }
            else
            {
                BtnTaxCodeNew.Enabled = !enable;
                BtnTaxCodeDelete.Enabled = !enable;
                BtnTaxCodeEdit.Enabled = !enable;
                BtnTaxCodeCancel.Enabled = enable;
                BtnTaxCodeSave.Enabled = enable;
            }
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxTaxCodeCode.Text.Trim()))
            {
                TextBoxTaxCodeCode.Select();
                TaxCodeErrorMsg.Text = EnterCodeErrorMsg;
                return false;
            }
            if (DataGridViewTaxCodeCurrentTaxCode.RowCount > 1)
            {
                foreach (DataGridViewRow Row in DataGridViewTaxCodeCurrentTaxCode.Rows)
                {
                    if (Row.Index < DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1)
                    {
                        for (int i = 0; i < DataGridViewTaxCodeCurrentTaxCode.ColumnCount - 4; i++)
                        {
                            if (Row.Cells[i].Value == null || string.IsNullOrEmpty(Row.Cells[i].Value.ToString()))
                            {
                                DataGridViewTaxCodeCurrentTaxCode.Select();
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[i, Row.Index];
                                DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                TaxCodeErrorMsg.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[i].HeaderText);
                                return false;
                            }
                            if (i == (int)ActiveTaxTableColumn.PERCENT && Global.Company.Country.Name == "India" && (double.Parse(Row.Cells[i].Value.ToString()) > 49))
                            {
                                DataGridViewTaxCodeCurrentTaxCode.Select();
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[i, Row.Index];
                                DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                TaxCodeErrorMsg.Text = Grid_TaxPercentErrorMsg;
                                return false;
                            }
                            if ((i == (int)ActiveTaxTableColumn.EFFECTIVEFROM || i == (int)ActiveTaxTableColumn.EFFECTIVETO))
                            {
                                if (Row.Cells[i].Value == null || !DateUtils.ValidDate(((DateTime)Row.Cells[i].Value).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                                {
                                    DataGridViewTaxCodeCurrentTaxCode.Select();
                                    DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[i, Row.Index];
                                    DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                    TaxCodeErrorMsg.Text = string.Format(Grid_InvalidDataErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[i].HeaderText);
                                    return false;
                                }
                            }
                        }
                    }
                }
                foreach (DataGridViewRow Row in DataGridViewTaxCodeCurrentTaxCode.Rows)
                {
                    if (Row.Index < DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1)
                    {
                        long Id = Row.Cells[(int)ActiveTaxTableColumn.ID].Value == null || string.IsNullOrEmpty(Row.Cells[(int)ActiveTaxTableColumn.ID].Value.ToString()) ? 0 : long.Parse(Row.Cells[(int)ActiveTaxTableColumn.ID].Value.ToString());
                        long MapId = long.TryParse(Row.Cells[(int)ActiveTaxTableColumn.MAPID]?.Value?.ToString(), out var parsedMapIdValue) ? parsedMapIdValue : 0;
                        DateTime FromDate = (DateTime)Row.Cells[(int)ActiveTaxTableColumn.EFFECTIVEFROM].Value;
                        DateTime ToDate = (DateTime)Row.Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].Value;
                        if (FromDate > ToDate)
                        {
                            DataGridViewTaxCodeCurrentTaxCode.Select();
                            DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVEFROM, Row.Index];
                            DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                            TaxCodeErrorMsg.Text = Grid_InvalidDateErrorMsg;
                            return false;
                        }
                        foreach (DataGridViewRow lRow in DataGridViewTaxCodeCurrentTaxCode.Rows)
                        {
                            if (lRow.Index < DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1)
                            {

                                long lMapId = long.TryParse(lRow.Cells[(int)ActiveTaxTableColumn.MAPID]?.Value?.ToString(), out var parsedlMapIdValue) ? parsedlMapIdValue : 0;
                                DateTime lFromDate = (DateTime)lRow.Cells[(int)ActiveTaxTableColumn.EFFECTIVEFROM].Value;
                                DateTime lToDate = (DateTime)lRow.Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].Value;
                                if (Row.Index != lRow.Index)
                                {
                                    if (MapId == lMapId && (FromDate <= lFromDate && ToDate >= lFromDate))
                                    {
                                        DataGridViewTaxCodeCurrentTaxCode.Select();
                                        DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVEFROM, lRow.Index];
                                        DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                        TaxCodeErrorMsg.Text = string.Format(Grid_InvalidFromDateErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[(int)ActiveTaxTableColumn.EFFECTIVEFROM].HeaderText);
                                        return false;
                                    }
                                    if (MapId == lMapId && (FromDate <= lToDate && ToDate >= lToDate))
                                    {
                                        DataGridViewTaxCodeCurrentTaxCode.Select();
                                        DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVETO, lRow.Index];
                                        DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                        TaxCodeErrorMsg.Text = string.Format(Grid_InvalidToDateErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[(int)ActiveTaxTableColumn.EFFECTIVETO].HeaderText);
                                        return false;
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(TextBoxTaxCodeId.Text))
                        {
                            long ItemTaxId = long.Parse(TextBoxTaxCodeId.Text);
                            if (ItemTaxManager.Instance.CheckItemTaxMapHaveSameFromDate(MapId, ItemTaxId, FromDate,Id))
                            {
                                DataGridViewTaxCodeCurrentTaxCode.Select();
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVEFROM, Row.Index];
                                DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                TaxCodeErrorMsg.Text = string.Format(Grid_InvalidFromDateErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[(int)ActiveTaxTableColumn.EFFECTIVEFROM].HeaderText);
                                return false;
                            }
                            if (ItemTaxManager.Instance.CheckItemTaxMapHaveSameToDate(MapId, ItemTaxId, ToDate,Id))
                            {
                                DataGridViewTaxCodeCurrentTaxCode.Select();
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVETO, Row.Index];
                                DataGridViewTaxCodeCurrentTaxCode.BeginEdit(true);
                                TaxCodeErrorMsg.Text = string.Format(Grid_InvalidToDateErrorMsg, DataGridViewTaxCodeCurrentTaxCode.Columns[(int)ActiveTaxTableColumn.EFFECTIVETO].HeaderText);
                                return false;
                            }
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
        private void TextBoxTaxCodeSearch_TextChanged(object sender, EventArgs e)
        {
            ListTaxCodes();
            TextBoxTaxCodeSearch.Select();
        }
        private ItemTax GetItemTaxFromForm()
        {
            ItemTax ItemTax = new ItemTax();
            ItemTax.Code = TextBoxTaxCodeCode.Text.Trim();
            ItemTax.Description = TextBoxTaxCodeDetail.Text.Trim();
            ItemTax.Id = string.IsNullOrEmpty(TextBoxTaxCodeId.Text) ? 0L : long.Parse(TextBoxTaxCodeId.Text);
            ItemTax.CompanyId = Global.Company.CompanyId;
            if (DataGridViewTaxCodeCurrentTaxCode.Rows.Count > 1)
            {
                List<ItemSalesTaxMap> lItemSalesTaxMap = new List<ItemSalesTaxMap>();
                foreach (DataGridViewRow Row in DataGridViewTaxCodeCurrentTaxCode.Rows)
                {
                    if (Row.Index < DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1)
                    {
                        ItemSalesTaxMap map = new ItemSalesTaxMap();
                        map.SalesTaxMapId = Row.Cells[(int)CatalogTaxTableColumn.MAPID].Value == null ? (long)Row.Cells[(int)CatalogTaxTableColumn.NAME].Value : (long)Row.Cells[(int)CatalogTaxTableColumn.MAPID].Value;
                        map.TaxPercentage = Row.Cells[(int)CatalogTaxTableColumn.PERCENT].Value != null ? float.Parse(Row.Cells[(int)CatalogTaxTableColumn.PERCENT].Value.ToString()) : 0;
                        map.EffectiveFromDate = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.FROM].Value;
                        map.EffectiveToDate = (DateTime)Row.Cells[(int)CatalogTaxTableColumn.TO].Value;
                        map.Id = Row.Cells[(int)CatalogTaxTableColumn.ID].Value != null ? (long)Row.Cells[(int)CatalogTaxTableColumn.ID].Value : 0L;
                        ItemTax.SalesTaxMapLocal.Add(map);
                    }
                }
            }
            return ItemTax;
        }
        private void BtnTaxCodeNew_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TextBoxTaxCodeSearch.ResetText();
            ResetForm();
            EnableForm(true);
            TextBoxTaxCodeCode.Select();
            GridViewTaxCodeList.ClearSelection();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnTaxCodeDelete_Click(object sender, EventArgs e)
        {

            TaxCodeErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxTaxCodeId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this tax code is still valid.");
                return;
            }
            else
            {
                CurrentCodeId = long.Parse(TextBoxTaxCodeId.Text);
                LoadCurrentRow();
            }
            long TaxCodeID = Convert.ToInt64(TextBoxTaxCodeId.Text);
            ItemTax ItemTax = ItemTaxManager.GetItemTaxCodeById(TaxCodeID);
            if (ItemTax != null)
            {
                DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, TextBoxTaxCodeCode.Text), "Delete Confirm",
              MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (!CatalogProductManager.Instance.HSNCodeUsedInProduct(ItemTax.Code, Global.Company.CompanyId))
                    {
                        bool DeleteResult = ItemTaxManager.DeleteItemTax(TaxCodeID);
                        if (DeleteResult)
                        {
                            ResetForm();
                            ListTaxCodes();
                            EnableForm(false);
                            TaxCodeErrorMsg.Text = DeleteSuccessMsg;
                            this.formIsDirty = false;
                        }
                        else
                        {
                            TaxCodeErrorMsg.Text = DeleteErrorText;
                        }
                    }
                    else
                    {
                        TaxCodeErrorMsg.Text = "Tax code used in product, Do not allow to delete the tax code.";
                        return;
                    }
                    Cursor.Current = Cursors.Default;
                }

            }
            else
            {
                TaxCodeErrorMsg.Text = "Somting went wrong, please check this tax code is still valid.";
                return;
            }
        }
        private void BtnTaxCodeEdit_Click(object sender, EventArgs e)
        {
            CurrentCodeId = long.Parse(TextBoxTaxCodeId.Text);
            ResetForm();
            LoadCurrentRow();
            LoadItemTax();
            EnableForm(true);
            GridViewTaxCodeList.ClearSelection();
            {
                TextBoxTaxCodeCode.Focus();
                TextBoxTaxCodeCode.SelectAll();
            }
            this.formIsDirty = false;
        }
        long CurrentCodeId = 0L;
        private void LoadCurrentRow()
        {
            if (CurrentCodeId != 0L)
            {
                foreach (DataGridViewRow row in GridViewTaxCodeList.Rows)
                {
                    if ((long)row.Cells[(int)ItemTaxTableColumn.ID].Value == CurrentCodeId)
                    {
                        GridViewTaxCodeList.ClearSelection();
                        GridViewTaxCodeList.CurrentCell = GridViewTaxCodeList[0, row.Index];
                        GridViewTaxCodeList.Rows[row.Index].Selected = true;
                    }
                }
                CurrentCodeId = 0L;
            }
        }
        private void BtnTaxCodeCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxTaxCodeCode.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (string.IsNullOrEmpty(TextBoxTaxCodeSearch.Text))
            {
                ListTaxCodes();
            }
            else
            {
                TextBoxTaxCodeSearch.Clear();
            }
            EnableForm(false);
            if (GridViewTaxCodeList.Rows.Count > 0)
            {
                GridViewTaxCodeList.Select();
            }
            else
            {
                BtnTaxCodeNew.Select();
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void BtnTaxCodeSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    ItemTax lItemTax = GetItemTaxFromForm();
                    ItemTax lItemTaxFromDB = null;

                    if (ItemTaxManager.ItemTaxCodeUniqueById(lItemTax))
                    {
                        if (lItemTax.Id == 0)
                        {
                            lItemTaxFromDB = ItemTaxManager.AddItemTax(lItemTax);
                        }
                        else
                        {
                            lItemTaxFromDB = ItemTaxManager.UpdateItemTax(lItemTax);
                        }
                        if (lItemTaxFromDB != null)
                        {
                            ResetForm();
                            ListTaxCodes();
                            GridViewTaxCodeList.Focus();
                            CurrentCodeId = lItemTaxFromDB.Id;
                            LoadCurrentRow();
                            LoadItemTax();
                            EnableForm(false);
                            if (UpdateTaxCodeOnLoad)
                            {
                                this.Close();
                            }
                            TaxCodeErrorMsg.Text = SaveSuccessText;
                            this.formIsDirty = false;
                        }
                    }
                    else
                    {
                        TextBoxTaxCodeCode.Select();
                        TaxCodeErrorMsg.Text = string.Format(UniqueTaxCodeErrorMsg, TextBoxTaxCodeCode.Text.Trim(), Global.Company.Name);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnTaxCodeExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnTaxCodeImport_Click(object sender, EventArgs e)
        {
            TaxCodeFileUpload FileUploadDialog = new TaxCodeFileUpload();
            FileUploadDialog.ShowDialog();
            ResetForm();
            ListTaxCodes();
            EnableForm(false);
            this.formIsDirty = false;
        }
        private void TextBoxTaxCodeDetail_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewTaxCodeCurrentTaxCode.Rows.Count > 0)
                {
                    DataGridViewTaxCodeCurrentTaxCode.Select();
                    DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.TAX, 0];
                }
                else
                {
                    BtnTaxCodeSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTaxCodeCode.Select();
            }
        }
        private void BtnTaxCodeSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int lastRowIndex = DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1;
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTaxCodeCode.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewTaxCodeCurrentTaxCode.Rows.Count > 0)
                {
                    DataGridViewTaxCodeCurrentTaxCode.Select();
                    DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[(int)ActiveTaxTableColumn.EFFECTIVETO, lastRowIndex];
                }
                else
                {
                    TextBoxTaxCodeDetail.Select();
                }
            }
        }
        private void TextBoxTaxCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTaxCodeDetail.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnTaxCodeSave.Select();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnTaxCodeNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnTaxCodeDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnTaxCodeEdit.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnTaxCodeSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnTaxCodeCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnTaxCodeExit.PerformClick();
                return true;
            }
            try
            {
                int currentColumnIndex = DataGridViewTaxCodeCurrentTaxCode.ColumnCount-3;
                int currentRowIndex = DataGridViewTaxCodeCurrentTaxCode.Rows.Count;

                if (DataGridViewTaxCodeCurrentTaxCode != null && DataGridViewTaxCodeCurrentTaxCode.CurrentCell != null)
                {
                    currentColumnIndex = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.ColumnIndex;
                    currentRowIndex = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.RowIndex;
                }
                if (DataGridViewTaxCodeCurrentTaxCode.CurrentCell != null && DataGridViewTaxCodeCurrentTaxCode.CurrentCell.Selected)
                {
                    if (keyData == (Keys.Tab) && DataGridViewTaxCodeCurrentTaxCode.CurrentCell.ColumnIndex == (int)ActiveTaxTableColumn.EFFECTIVETO && DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1 != DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Index)
                    {
                        SendKeys.Send("{tab}");
                    }
                    else if (keyData == (Keys.Tab) && DataGridViewTaxCodeCurrentTaxCode.CurrentCell.ColumnIndex == (int)ActiveTaxTableColumn.EFFECTIVETO && DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1 == DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Index)
                    {
                        DataGridViewTaxCodeCurrentTaxCode.CurrentCell = null;
                        BtnTaxCodeSave.Select();
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewTaxCodeCurrentTaxCode?.CurrentCell?.ColumnIndex == (int)ActiveTaxTableColumn.TAX && DataGridViewTaxCodeCurrentTaxCode.CurrentRow?.Index != 0)
                    {
                        if (currentColumnIndex > 0)
                        {
                            if(currentColumnIndex == (int)ActiveTaxTableColumn.EFFECTIVETO)
                            {
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[currentColumnIndex - 1, currentRowIndex-1];
                            }
                            else
                            {
                                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[currentColumnIndex - 1, currentRowIndex];
                            }
                            return true;
                        }
                        else if (currentRowIndex > 0)
                        {
                            DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[DataGridViewTaxCodeCurrentTaxCode.ColumnCount - 1, currentRowIndex - 1];
                            return true;
                        }
                        else
                        {
                            TextBoxTaxCodeDetail.Select();
                        }                       
                    }
                }
                if (GridViewTaxCodeList.CurrentCell != null && GridViewTaxCodeList.CurrentCell.Selected)
                {
                    int Index = GridViewTaxCodeList.CurrentRow.Index;
                    if (keyData == (Keys.Tab) && GridViewTaxCodeList.CurrentRow != null && DataGridViewTaxCodeTaxCodeHistory.Rows.Count - 1 != Index)
                    {
                        GridViewTaxCodeList.Select();
                        GridViewTaxCodeList.CurrentCell = GridViewTaxCodeList[0, Index + 1];
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewTaxCodeList.CurrentRow != null && Index != 0)
                    {
                        GridViewTaxCodeList.Select();
                        GridViewTaxCodeList.CurrentCell = GridViewTaxCodeList[0, Index - 1];
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void MoveFocusBackward()
        {
            int currentColumnIndex = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.ColumnIndex;
            int currentRowIndex = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.RowIndex;

            if (currentColumnIndex > 0)
            {
                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[currentColumnIndex - 1, currentRowIndex];
            }
            else if (currentRowIndex > 0)
            {
                DataGridViewTaxCodeCurrentTaxCode.CurrentCell = DataGridViewTaxCodeCurrentTaxCode[DataGridViewTaxCodeCurrentTaxCode.ColumnCount - 1, currentRowIndex - 1];
            }
            else
            {
                // Already at the first cell of the first row
                // Handle as needed
            }
        }
        private void GridViewTaxCode_Leave(object sender, EventArgs e)
        {
            GridViewTaxCodeList.CurrentCell = null;
        }
        private void BtnTaxCodeReport_Click(object sender, EventArgs e)
        {
            FormTaxCodeReport FormTaxCodeReport = new FormTaxCodeReport();
            FormTaxCodeReport.Show();
            if (FormTaxCodeReport.StartPosition == FormStartPosition.CenterParent)
            {
                var x = Location.X + (Width - FormTaxCodeReport.Width) / 2;
                var y = Location.Y + (Height - FormTaxCodeReport.Height) / 2;
                FormTaxCodeReport.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
            }
        }
        private void BtnTaxCodeExport_Click(object sender, EventArgs e)
        {
            TaxCodeExportFile TaxCodeExportFiles = new TaxCodeExportFile();
            Cursor.Current = Cursors.WaitCursor;
            TaxCodeExportFiles.GenerateFile();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewTaxCode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewTaxCodeList.Rows[e.RowIndex].Cells[(int)ItemTaxTableColumn.ID].Value != null)
                {
                    ResetForm();
                    LoadItemTax();
                    EnableForm(false);
                    this.formIsDirty = false;
                }
            }
        }

        private void DataGridViewCurrentTax_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).FormattingEnabled = true;


                if (DataGridViewTaxCodeCurrentTaxCode.CurrentCell.Value == null)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                else
                {
                    if (DataGridViewTaxCodeCurrentTaxCode.CurrentCell.FormattedValue != null)
                    {
                        List<CompanySalesTaxAccountMap> MapTax = Global.Company.SalesTaxAccountMaps.ToList();
                        CompanySalesTaxAccountMap companySalesTaxMap = MapTax.FirstOrDefault(x => x.Name == DataGridViewTaxCodeCurrentTaxCode.CurrentCell.FormattedValue.ToString());
                        if (companySalesTaxMap == null)
                        {
                            ((ComboBox)e.Control).SelectedIndex = -1;
                            ((ComboBox)e.Control).Text = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.Value.ToString();
                        }
                    }
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewCurrentTax_KeyPress);
            }
        }
        private void DataGridViewCurrentTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewTaxCodeCurrentTaxCode.EditingControl).DroppedDown = false;
        }
        private void DataGridViewCurrentTax_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (Global.Company.SalesTaxAccountMaps != null)
            {
                DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.PERCENT].Value = "0.00";
                DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.REMOVE].Value = "X";
                (DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.TAX] as DataGridViewComboBoxCell).DataSource = null;
                (DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.TAX] as DataGridViewComboBoxCell).DataSource = Global.Company.SalesTaxAccountMaps;
                (DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.TAX] as DataGridViewComboBoxCell).ValueMember = "MapId";
                (DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.TAX] as DataGridViewComboBoxCell).DisplayMember = "Name";
                DataGridViewTaxCodeCurrentTaxCode.Rows[e.RowIndex].Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].Value = Global.getTransactionDate().AddYears(2400 - Global.getTransactionDate().Year);
            }
        }
        private void DataGridViewCurrentTax_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.REMOVE].ReadOnly = true;
                if (e.RowIndex < DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1)
                {
                    if (DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.ID].Value != null)
                    {
                        long ItemMapId = (long)DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.ID].Value;
                        if (ItemTaxManager.Instance.CheckTaxCodeMapHaveEntries(ItemMapId))
                        {
                            DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.TAX].ReadOnly = true;
                            DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.PERCENT].ReadOnly = true;
                            DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.EFFECTIVEFROM].ReadOnly = true;
                            if (DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].Value != null && DateUtils.ValidDate(((DateTime)DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].Value).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                            {
                                if (ItemTaxManager.Instance.CheckTaxCodeMapHaveOtherEntryAfterToDate((long)DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)CatalogTaxTableColumn.NAME].Value, (DateTime)DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)CatalogTaxTableColumn.TO].Value))
                                {
                                    DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.EFFECTIVETO].ReadOnly = true;
                                }
                            }
                        }
                    }
                }
            }
            if (e.ColumnIndex == (int)CatalogTaxTableColumn.NAME)
            {
                DataGridViewTaxCodeCurrentTaxCode.CurrentCell.ReadOnly = false;
            }
        }
        private void DataGridViewCurrentTax_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)CatalogTaxTableColumn.REMOVE && (DataGridViewTaxCodeCurrentTaxCode.Rows.Count - 1) != e.RowIndex && !DataGridViewTaxCodeCurrentTaxCode.ReadOnly)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, (e.RowIndex + 1).ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        long ItemMapId = DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value != null ? (long)DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)CatalogTaxTableColumn.ID].Value : 0L;
                        if (ItemMapId == 0L)
                        {
                            DataGridViewTaxCodeCurrentTaxCode.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            DataGridViewTaxCodeCurrentTaxCode.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            if (ItemTaxManager.Instance.CheckTaxCodeMapHaveEntries(ItemMapId))
                            {
                                MessageBox.Show(Grid_RowNotDeleteText);
                            }
                            else
                            {
                                if (ItemTaxManager.Instance.DeleteCatalogCodeTax(ItemMapId))
                                {
                                    DataGridViewTaxCodeCurrentTaxCode.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    DataGridViewTaxCodeCurrentTaxCode.Rows.RemoveAt(e.RowIndex);
                                    TaxCodeErrorMsg.Text = Grid_RowDeleteText;
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
        private void TextBoxTaxCodeDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void FormTaxCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DataGridViewTaxCodeCurrentTaxCode_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (!string.IsNullOrEmpty(DataGridViewTaxCodeCurrentTaxCode?.CurrentCell?.EditedFormattedValue?.ToString()?.Trim()))
                {
                    DataGridViewTaxCodeCurrentTaxCode.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    List<CompanySalesTaxAccountMap> MapTax = Global.Company.SalesTaxAccountMaps.ToList();
                    CompanySalesTaxAccountMap OutputSalesTaxMap = MapTax.FirstOrDefault(x => x.Name == DataGridViewTaxCodeCurrentTaxCode.CurrentCell.EditedFormattedValue.ToString())!;
                    if (OutputSalesTaxMap == null)
                    {
                        DataGridViewTaxCodeCurrentTaxCode.CurrentCell.Value = DataGridViewTaxCodeCurrentTaxCode.CurrentCell.EditedFormattedValue.ToString();
                    }
                    else
                    {
                        DataGridViewTaxCodeCurrentTaxCode.CurrentCell.Value = OutputSalesTaxMap.MapId;
                        DataGridViewTaxCodeCurrentTaxCode.CurrentRow.Cells[(int)ActiveTaxTableColumn.MAPID].Value = OutputSalesTaxMap.MapId;
                    }
                }
            }
        }

        private void DataGridViewTaxCodeCurrentTaxCode_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.SelectionBackColor = Color.White;
            e.CellStyle.SelectionForeColor = Color.Black;
        }
    }
}
