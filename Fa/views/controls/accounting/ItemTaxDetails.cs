using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.model.Catalog;
using fa.model.Accounting.Masters;
using fa.api.catalog;
using fa.api.utils;
using fa.api.Accounting;
using fa.model.catalog;
using Fa.api.catalog;

namespace fa.views.controls.accounting
{
    public enum ItemTaxGridColumn
    {
        NAME, PERCENT, ID
    }
    public partial class ItemTaxDetails : UserControl
    {
        protected static int MIN_WIDTH = 224;
        protected static int DATAGRID_WIDTH_DIFF = 8;
        protected static int DATAGRID_DESC_COLUMN_DIFF = 120;
        public ItemTaxDetails()
        {
            InitializeComponent();
            labeltitle.ForeColor = Color.Black;
        }
        private void SizeChange()
        {
            if (this.Width - DATAGRID_WIDTH_DIFF < MIN_WIDTH)
            {
                this.Width = MIN_WIDTH;
            }
            else
            {
                GridViewProductTaxDetails.Width = this.Width - DATAGRID_WIDTH_DIFF;
                GridViewProductTaxDetails.Columns[0].Width = GridViewProductTaxDetails.Width - DATAGRID_DESC_COLUMN_DIFF;
            }
            GridViewProductTaxDetails.Height = this.Height - 25;
        }
        private bool _EnableEdit;
        public bool EnableEdit
        {
            get
            {
                return _EnableEdit;
            }
            set
            {
                _EnableEdit = value;
                if (GridViewProductTaxDetails.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in GridViewProductTaxDetails.Rows)
                    {
                        GridViewProductTaxDetails.ReadOnly = !value;
                        row.Cells[0].ReadOnly = value;
                    }
                }
            }
        }
        private DateTime? _CurrentDate = null;
        public DateTime? CurrentDate
        {
            get
            {
                return _CurrentDate;
            }
            set
            {
                _CurrentDate = value;
            }
        }



        public void Clear()
        {
            CurrentDate = null;
            GridViewProductTaxDetails.Rows.Clear();
        }
        public IList<CatalogItemSalesTaxMap> CatalogItemSalesTaxMaps
        {
            get
            {
                if (ProductId != 0L)
                {
                    return ListCatalogItemSalesTaxMap();
                }
                else
                {
                    return new List<CatalogItemSalesTaxMap>();
                }
            }
        }
        private long _ProductId = 0L;
        public long ProductId
        {
            get
            {
                return _ProductId;
            }
            set
            {
                _ProductId = value;
                LoadCatalogItemSalesTaxMap(value);
            }
        }
        private void LoadCatalogItemSalesTaxMap(long ProductId)
        {
            if (ProductId != 0L)
            {
                DateTime NewCurrentDate = CurrentDate != null ? (DateTime)CurrentDate : Global.getTransactionDate();
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                if (Product != null)
                {
                    if (Product.UseHsnTax)
                    {
                        ItemTax ItemTax = ItemTaxManager.Instance.GetItemTaxCodeByCode(Product.HSNCode, Global.Company.CompanyId);
                        if (ItemTax != null)
                        {
                            GridViewProductTaxDetails.Rows.Clear();
                            if (ItemTax.SalesTaxMapLocal.Count > 0)
                            {
                                int i = 0;
                                foreach (ItemSalesTaxMap ItemTaxMap in ItemTax.SalesTaxMapLocal)
                                {
                                    if (ItemTaxMap != null)
                                    {
                                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)ItemTaxMap.SalesTaxMapId);
                                        if (CMap != null && CMap.CountrySaleTax.EffectiveFrom <= Global.getTransactionDate() && CMap.CountrySaleTax.EffectiveTo >= Global.getTransactionDate())
                                        {
                                            GridViewProductTaxDetails.Rows.Add();
                                            GridViewProductTaxDetails.Rows[i].Cells[0].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == ItemTaxMap.SalesTaxMapId).Name;

                                            if (ItemTaxMap.EffectiveFromDate <= Global.getTransactionDate() && ItemTaxMap.EffectiveToDate >= Global.getTransactionDate())
                                            {
                                                GridViewProductTaxDetails.Rows[i].Cells[1].Value = ItemTaxMap.TaxPercentage.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                            }
                                            else
                                            {
                                                GridViewProductTaxDetails.Rows[i].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                                            }
                                            GridViewProductTaxDetails.Rows[i].Cells[2].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == ItemTaxMap.SalesTaxMapId).MapId;
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow dgveiwempty in GridViewProductTaxDetails.Rows)
                            {
                                GridViewProductTaxDetails.Rows[dgveiwempty.Index].Cells[1].Value = "0.00";
                            }
                        }
                    }
                    else
                    {
                        IList<CatalogItemSalesTaxMap> lCatalogItemSalesTaxMap = Product.SalesTax.ToList();
                        if (lCatalogItemSalesTaxMap != null && lCatalogItemSalesTaxMap.Count > 0)
                        {
                            GridViewProductTaxDetails.Rows.Clear();
                            int i = 0;
                            foreach (CatalogItemSalesTaxMap CatalogItemSalesTaxMap in lCatalogItemSalesTaxMap)
                            {
                                if (CatalogItemSalesTaxMap != null)
                                {
                                    CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)CatalogItemSalesTaxMap.SalesTaxMapId);
                                    if (CMap != null && CMap.CountrySaleTax.EffectiveFrom <= CurrentDate && CMap.CountrySaleTax.EffectiveTo >= CurrentDate)
                                    {
                                        GridViewProductTaxDetails.Rows.Add();
                                        GridViewProductTaxDetails.Rows[i].Cells[0].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == CatalogItemSalesTaxMap.SalesTaxMapId).Name;
                                        if (CatalogItemSalesTaxMap.EffectiveFrom <= CurrentDate && CatalogItemSalesTaxMap.EffectiveTo >= CurrentDate)
                                        {
                                            GridViewProductTaxDetails.Rows[i].Cells[1].Value = CatalogItemSalesTaxMap.TaxPercentage.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                        }
                                        else
                                        {
                                            GridViewProductTaxDetails.Rows[i].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                                        }
                                        GridViewProductTaxDetails.Rows[i].Cells[2].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == CatalogItemSalesTaxMap.SalesTaxMapId).MapId;
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private List<CatalogItemSalesTaxMap> ListCatalogItemSalesTaxMap()
        {
            List<CatalogItemSalesTaxMap> SalesTaxFromForm = new List<CatalogItemSalesTaxMap>();
            for (int i = 0; i < GridViewProductTaxDetails.Rows.Count; i++)
            {
                CatalogItemSalesTaxMap CatalogItemSalesTaxMap = new CatalogItemSalesTaxMap();
                long TaxId = ((long)GridViewProductTaxDetails.Rows[i].Cells[2].Value);
                CompanySalesTaxAccountMap CompanySalesTaxAccountMap = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == TaxId);
                if (CompanySalesTaxAccountMap != null)
                {
                    CatalogItemSalesTaxMap.SalesTaxMapId = CompanySalesTaxAccountMap.MapId;
                    CatalogItemSalesTaxMap.TaxPercentage = float.Parse(GridViewProductTaxDetails.Rows[i].Cells[1].Value.ToString());
                    SalesTaxFromForm.Add(CatalogItemSalesTaxMap);
                }
            }
            return SalesTaxFromForm;
        }
        private void ItemTaxDetails_Enter(object sender, EventArgs e)
        {
            if (GridViewProductTaxDetails.Rows.Count > 0)
            {
                GridViewProductTaxDetails.CurrentCell = GridViewProductTaxDetails[1, 0];
            }
        }
        private void GridViewProductTaxDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                int count = Global.Company.SalesTaxAccountMaps.Count - 1;
                if (GridViewProductTaxDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewProductTaxDetails.CurrentCell.ColumnIndex == 1 && GridViewProductTaxDetails.CurrentCell.RowIndex != count)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewProductTaxDetails.CurrentCell.ColumnIndex == 1)
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        string Msg;
        public String ErrorMsg()
        {
            return Msg;
        }
        public Boolean IsItemTaxDetailsValidationResult()
        {
            Msg = string.Empty;
            int Count = GridViewProductTaxDetails.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 2; j++)
                    {
                        if (GridViewProductTaxDetails.Rows[i].Cells[(int)AddChargeGridColumn.TYPE].Value != null &&
                            float.Parse(GridViewProductTaxDetails.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            Msg = "Please Valid Tax Percentage";
                            GridViewProductTaxDetails.Select();
                            GridViewProductTaxDetails.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridViewProductTaxDetails.CurrentCell = GridViewProductTaxDetails[j, i];
                            }));
                            GridViewProductTaxDetails.BeginEdit(true);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private void ItemTaxDetails_Load(object sender, EventArgs e)
        {

        }
        private void ItemTaxDetails_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void GridViewProductTaxDetails_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void ItemTaxDetails_EnabledChanged(object sender, EventArgs e)
        {
            labeltitle.ForeColor = Color.Black;
        }
    }
}
