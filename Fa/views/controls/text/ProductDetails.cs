using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Catalog;
using fa.api.catalog;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.api.utils;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using fa.libraries.utils;

namespace fa.views.controls.text
{
    public enum QOHTableColumn
    {
        NAME, RQty, WQty, ID
    }
    public partial class ProductDetails : UserControl
    {
        public ProductDetails()
        {
            InitializeComponent();
            GroupBoxProductDetail.Size = this.Size;
        }
        public void Clear()
        {
            GridViewQOH.Rows.Clear();
            TextBoxSalesMaterialId.ResetText();
            TextBoxSalesProductName.ResetText();
            TextBoxSalesProductUOM.ResetText();
            ProductItemTaxDetails.Clear();
            _BatchId = 0L;
            _LocationId = 0L;
            _ProductId = 0L;
        }
        public bool ReadyOnly
        {
            set
            {
                TextBoxSalesMaterialId.ReadOnly = value;
                TextBoxSalesProductName.ReadOnly = value;
                TextBoxSalesProductUOM.ReadOnly = value;
                TextBoxSalesMaterialId.TabStop = !value;
                TextBoxSalesProductName.TabStop = !value;
                TextBoxSalesProductUOM.TabStop = !value;
                ProductItemTaxDetails.EnableEdit = !value;
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
                if (value != 0L)
                {
                    LoadProductDetail();
                    ProductItemTaxDetails.CurrentDate = CurrentDate;
                    ProductItemTaxDetails.ProductId = value;
                }
            }
        }
        private long _BatchId = 0L;
        public long BatchId
        {
            get
            {
                return _BatchId;
            }
            set
            {
                _BatchId = value;
                if (ProductId != 0L)
                {
                    LoadProductDetail();
                }
            }
        }
        private double _EditableStock = 0.00;
        public double EditableStock
        {
            get
            {
                return _EditableStock;
            }
            set
            {
                _EditableStock = value;
                if (ProductId != 0L)
                {
                    LoadProductDetail();
                }
            }
        }
        private long _LocationId = 0L;
        public long LocationId
        {
            get
            {
                return _LocationId;
            }
            set
            {
                _LocationId = value;
                if (ProductId != 0L)
                {
                    LoadProductDetail();
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
        public string ProductName
        {
            get
            {
                return TextBoxSalesProductName.Text;
            }
        }
        private void LoadProductDetail()
        {
            if (ProductId != 0L)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                TextBoxSalesMaterialId.Text = Product.MaterialId;
                TextBoxSalesProductName.Text = Product.Name;
                TextBoxSalesProductUOM.Text = Product.UOM;
                StockByXfactor();
            }
        }
        private void StockByXfactor()
        {
            GridViewQOH.Rows.Clear();
            if (ProductId != 0L)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                if (Product != null)
                {
                    NumberFormatInfo DecimalNos = new CultureInfo("en-US", false).NumberFormat;
                    DecimalNos.NumberDecimalDigits = Global.Company.QuantityPricision;
                    IList<Inventory> lInventory = InventoryLocationManager.Instance.ListInventoryByProductIdWithLocation(ProductId);
                    if (lInventory != null && lInventory.Count > 0)
                    {
                        int Row;
                        foreach (Inventory Inv in lInventory)
                        {
                            Row = GridViewQOH.Rows.Add();
                            GridViewQOH.Rows[Row].Cells[(int)QOHTableColumn.NAME].Value = Inv.InventoryLocation.Name;
                            GridViewQOH.Rows[Row].Cells[(int)QOHTableColumn.ID].Value = Inv.InventoryLocation.Id;
                            GridViewQOH.Rows[Row].Cells[(int)QOHTableColumn.RQty].Value = Math.Round(((((Inv.StockDate != null && ((DateTime)DateUtils.ToDate(((DateTime)Inv.StockDate).ToString(Global.Company.DateFormat), Global.Company.DateFormat)).Date  <= Global.getTransactionDate().Date ? Inv.OpeningStock : 0) + Inv.QuantityOnHand) * (Product.WholesaleXFactor * Product.RetailXFactor)) + EditableStock), Global.Company.QuantityPricision).ToString("N", DecimalNos);
                            GridViewQOH.Rows[Row].Cells[(int)QOHTableColumn.WQty].Value = Math.Round(((((Inv.StockDate != null && ((DateTime)DateUtils.ToDate(((DateTime)Inv.StockDate).ToString(Global.Company.DateFormat), Global.Company.DateFormat)).Date  <= Global.getTransactionDate().Date ? Inv.OpeningStock : 0) + Inv.QuantityOnHand) * Product.WholesaleXFactor) + EditableStock), Global.Company.QuantityPricision).ToString("N", DecimalNos);
                        }
                    }
                }
            }
        }
        private void GroupBoxProductDetail_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void ProductDetails_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            GroupBoxProductDetail.Size = new Size(this.Width - 10, this.Height - 5);
            GridViewQOH.Width = this.Width - 25;
            ProductItemTaxDetails.Width = this.Width - 25;
            TextBoxSalesMaterialId.Width = this.Width - 25;
            TextBoxSalesProductName.Width = this.Width - 25;
            GridViewQOH.Columns[0].Width = GridViewQOH.Width - 170;
        }
        private bool TxtFocus(string input, TextBox sender)
        {
            if (input.Length < 0)
            {
                return false;
            }
            else
            {
                sender.Focus();
                sender.SelectionStart = 0;
                sender.SelectionLength = sender.Text.Length;
                return true;
            }
        }
        private void TextBoxSalesMaterialId_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxSalesMaterialId.Text, TextBoxSalesMaterialId);
        }
        private void TextBoxSalesProductName_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxSalesProductName.Text, TextBoxSalesProductName);
        }
        private void TextBoxSalesProductUOM_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxSalesProductUOM.Text, TextBoxSalesProductUOM);
        }

        private void GroupBoxProductDetail_Enter(object sender, EventArgs e)
        {

        }


    }
}
