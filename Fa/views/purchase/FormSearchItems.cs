using fa.api.catalog;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.catalog;
using Fa.views.purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.Libs.NDI;
using static fa.api.catalog.CatalogProductManager;

namespace fa.views.purchase
{
    public enum SearchItemTableColumn
    {
        NAME, MID, UOM, SPRICE, ID
    }
    public partial class FormSearchItems : FormBase
    {
        public static string SearchOutput = "No Item found!";
        public static string NoItemfoundErrorMsg = "You do not have any Item, Please add a Item!";
        public static string ChooseItemErrorMsg = "Please choose Item";
        public static string EnterItemDetailErrorMsg = "Please enter Item details,[MaterialId/Name]";
        public static string DeletedItemErrorMsg = "Your Select Item is Remove, Please press Go button then select the Item";
        public long LocationId = 0L;
        public string SearchText;
        public Product SelectedProduct { get; private set; }
        FormBase parent = null;
        public FormSearchItems(object sender)
        {
            parent = (FormBase)sender;
            InitializeComponent();
        }

        private void FormSearchItems_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (!string.IsNullOrEmpty(SearchText))
            {
                TextBoxSearchProduct.Text = SearchText;
                TextBoxSearchProduct.Select();
                return;
            }
            LoadProductInTextChange();
            TextBoxSearchProduct.Select();
            Cursor.Current = Cursors.Default;
        }

        private void LoadProducts(List<Product> Products)
        {
            GridViewItems.Rows.Clear();
            BtnSearchSelect.Enabled = false;
            if (Products != null && Products.Count > 0)
            {
                GridViewItems.Rows.Add(Products.Count);
                int i = 0;
                foreach (var lProduct in Products.ToList())
                {
                    GridViewItems.Rows[i].Cells[(int)SearchItemTableColumn.NAME].Value = lProduct.Name;
                    GridViewItems.Rows[i].Cells[(int)SearchItemTableColumn.MID].Value = lProduct.MaterialId;
                    GridViewItems.Rows[i].Cells[(int)SearchItemTableColumn.UOM].Value = lProduct.UOM;                    
                    GridViewItems.Rows[i].Cells[(int)SearchItemTableColumn.SPRICE].Value = lProduct.WholdSalePrice.ToString(Global.Company.PrimaryCurrency.CurrencyFormat); ;
                    GridViewItems.Rows[i].Cells[(int)SearchItemTableColumn.ID].Value = lProduct.Id;
                    i++;
                }
                if (GridViewItems.CurrentRow != null)
                {
                    var currentValue = GridViewItems.CurrentRow.Cells[(int)SearchItemTableColumn.ID].Value.ToString();
                    if( currentValue != null )
                    {
                        loadItemDetails(long.Parse(currentValue));
                    }
                }
                BtnSearchSelect.Enabled = true;
            }
            else
            {
                PatientSearchErrorMsg.Text = SearchOutput;
            }
        }
        private void ResetForm()
        {
            TextBoxSearchProduct.ResetText();
            BtnSearchSelect.Enabled = false;
        }
        private void BtnSearchSelect_Click(object sender, EventArgs e)
        {
            if (GridViewItems.Rows.Count == 0 || GridViewItems.CurrentRow == null || GridViewItems.CurrentRow.Index < 0)
            {
                PatientSearchErrorMsg.Text = ChooseItemErrorMsg;
                return;
            }

            var productId = long.Parse(GridViewItems.CurrentRow.Cells[(int)SearchItemTableColumn.ID].Value.ToString()!);

            // For FormProductResolution
            if (parent is FormProductResolution)
            {
                // Retrieve full product details
                SelectedProduct = Global.ProductDetailList.FirstOrDefault(p => p.Id == productId)!;
            }
            // For ItemBasedSales and other forms
            else
            {
                // Original logic for backward compatibility
                parent.ProductIdTransport.ResetText();
                parent.ProductIdTransport.Text = productId.ToString();
            }

            this.Close();
        }
        private void GridViewItems_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void GridViewItems_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (GridViewItems.Rows.Count > 0 && GridViewItems.CurrentRow != null)
            {
                if (GridViewItems.CurrentRow.Index > -1 && GridViewItems.CurrentRow.Cells[(int)SearchItemTableColumn.ID].Value != null)
                {
                    var currentValue = GridViewItems.CurrentRow.Cells[(int)SearchItemTableColumn.ID].Value.ToString();
                    if (currentValue != null)
                    {
                        loadItemDetails(long.Parse(currentValue));
                    }
                }
            }
        }
        private void loadItemDetails(long Id)
        {
            CatalogItem CatalogItem = CatalogItemManager.Instance.GetParentInfoById(Id);
            if (CatalogItem != null)
            {
                TextBoxManufacturer.Text = CatalogItem.Manufacturer;
                TextBoxSupplier.Text = CatalogItem.SupplierName;
                TextBoxCategory.Text = CatalogItem.Parent.Parent.Name;
                TextBoxProductFamily.Text = CatalogItem.Parent.Name;
            }
        }
        private void GridViewItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSearchSelect_Click(sender, e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSearchSelect.PerformClick();
            }
            if (keyData == (Keys.F3))
            {
                BtnNewProduct.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSearchCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewItems.CurrentRow.Index > -1)
                    {
                        if (GridViewItems.CurrentCell.RowIndex != GridViewItems.Rows.Count - 1)
                        {
                            GridViewItems.CurrentCell = GridViewItems[0, GridViewItems.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            BtnSearchSelect.Select();
                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewItems.CurrentRow.Index > -1)
                    {
                        if (GridViewItems.CurrentRow.Index != 0)
                        {
                            GridViewItems.CurrentCell = GridViewItems[0, GridViewItems.CurrentCell.RowIndex - 1];
                        }
                        else
                        {
                            TextBoxSearchProduct.Select();
                        }

                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public bool IsReload = false;
        private void BtnNewProduct_Click(object sender, EventArgs e)
        {
            FormCatalog FormCatalog = new FormCatalog(this);
            FormCatalog.CreateCatalogOnLoad = true;
            FormCatalog.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            if (IsReload)
            {
                Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                LoadProductInTextChange();
            }
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSearchProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadProductInTextChange();
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewItems.Rows.Count > 0)
                {
                    GridViewItems.Select();
                    GridViewItems.CurrentCell = GridViewItems[0, 0];
                }
            }
        }

        private void LoadProductInTextChange()
        {
            List<Product> Product = null!;
            if (Global.ProductDetailList == null || Global.ProductDetailList.Count == 0)
            {
                Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            }
            if (string.IsNullOrEmpty(TextBoxSearchProduct.Text))
            {
                Product = Global.ProductDetailList.ToList();
            }
            else
            {
                Product = Global.ProductDetailList.Where(x => (x.MaterialId.ToLower().Contains(TextBoxSearchProduct.Text.Trim().ToLower()) || x.Name.ToLower().Contains(TextBoxSearchProduct.Text.Trim().ToLower()))).ToList();
            }
            LoadProducts(Product);
        }

        private void BtnSearchSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearchProduct.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewItems.Rows.Count > 0)
                {
                    GridViewItems.Select();
                    GridViewItems.CurrentCell = GridViewItems[0, 0];
                }
                else
                {
                    TextBoxSearchProduct.Select();
                }
            }
        }

        private void GridViewItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSearchSelect_Click(sender, e);
            }
        }
        private bool GridFocus = false;
        private void GridViewItems_Enter(object sender, EventArgs e)
        {
            GridFocus = true;
        }
        private void GridViewItems_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }

        private void BtnSearchItemReload_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            LoadProductInTextChange();
            TextBoxSearchProduct.Select();
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSearchProduct_TextChanged(object sender, EventArgs e)
        {
            LoadProductInTextChange();
        }
    }
}
