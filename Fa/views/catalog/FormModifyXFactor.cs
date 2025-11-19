using fa.api.catalog;
using fa.model.Catalog;
using fa.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.catalog
{
    public partial class FormModifyXFactor : FormBase
    {
        private long? _productId;
        private Product? _product;

        public string ProductCode
        {
            get => TextBoxProductCode.Text;
            set => TextBoxProductCode.Text = value;
        }

        public string ProductName
        {
            get => TextBoxProductName.Text;
            set => TextBoxProductName.Text = value;
        }

        public string RetailUOM
        {
            get => ComboBoxProductRetailUOM.Text;
            set => ComboBoxProductRetailUOM.Text = value;
        }

        public string WholesaleUOM
        {
            get => ComboBoxProductWholeSaleUOM.Text;
            set => ComboBoxProductWholeSaleUOM.Text = value;
        }

        public string RetailXFactor
        {
            get => TextBoxXFactorRetail.Text;
            set => TextBoxXFactorRetail.Text = value;
        }

        public string WholesaleXFactor
        {
            get => TextBoxXFactorWholeSale.Text;
            set => TextBoxXFactorWholeSale.Text = value;
        }


        public FormModifyXFactor(long productId)
        {
            InitializeComponent();
            _productId = productId;
        }

        private void BtnPriceCalculatorSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBoxXFactorRetail.Text, out int retailFactor) || retailFactor <= 0)
            {
                CatalogErrorMsg.Text = "Enter valid Retail X-Factor!";
                return;
            }

            if (!int.TryParse(TextBoxXFactorWholeSale.Text, out int wholesaleFactor) || wholesaleFactor <= 0)
            {
                CatalogErrorMsg.Text = "Enter valid Wholesale X-Factor!";
                return;
            }

            string retailUOM = ComboBoxProductRetailUOM.Text.Trim();
            string wholesaleUOM = ComboBoxProductWholeSaleUOM.Text.Trim();

            if (string.IsNullOrEmpty(retailUOM) || string.IsNullOrEmpty(wholesaleUOM))
            {
                CatalogErrorMsg.Text = "Enter valid UOM!";
                return;
            }

            // Assign new values
            _product.RetailUOM = retailUOM;
            _product.WholesaleUOM = wholesaleUOM;
            _product.RetailXFactor = retailFactor;
            _product.WholesaleXFactor = wholesaleFactor;

            // Save to DB
            
            Product p = new Product
            {
                Id = long.Parse(TextBoxProductCode.Text),
                RetailUOM = ComboBoxProductRetailUOM.Text,
                WholesaleUOM = ComboBoxProductWholeSaleUOM.Text,
                RetailXFactor = int.Parse(TextBoxXFactorRetail.Text),
                WholesaleXFactor = int.Parse(TextBoxXFactorWholeSale.Text)
            };

            CatalogProductManager.Instance.UpdateProductXFactor(p);

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void ReloadFormData()
        {
            // Fill values
            TextBoxProductName.Text = _product.Name;
            TextBoxProductCode.Text = _product.MaterialId;

            ComboBoxProductRetailUOM.Text = _product.RetailUOM;
            ComboBoxProductWholeSaleUOM.Text = _product.WholesaleUOM;

            TextBoxXFactorRetail.Text = _product.RetailXFactor.ToString();
            TextBoxXFactorWholeSale.Text = _product.WholesaleXFactor.ToString();

            // Make editable
            TextBoxXFactorRetail.ReadOnly = false;
            TextBoxXFactorWholeSale.ReadOnly = false;
            ComboBoxProductRetailUOM.Visible = true;
            ComboBoxProductWholeSaleUOM.Visible = true;
        }

        private void FormModifyXFactor_Load(object sender, EventArgs e)
        {
            _product = CatalogProductManager.Instance.GetProductInfoById((long)_productId!);

            if (_product == null)
            {
                CatalogErrorMsg.Text = "Product not found!";
                return;
            }

            ReloadFormData();
        }
    }
}
