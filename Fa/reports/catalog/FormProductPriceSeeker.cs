using fa;
using fa.api.catalog;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.reports.catalog
{
    public partial class FormProductPriceSeeker : Form
    {
        public FormProductPriceSeeker()
        {
            InitializeComponent();
            TextBoxCatalogSearch.TextChanged += TextBoxCatalogSearch_TextChanged!;
            BtnExit.Click += BtnExit_Click!;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxCatalogSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = TextBoxCatalogSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                PRODUCTNAME.Text = string.Empty;
                ClearPrices();
                return;
            }

            // Ensure products are loaded
            if (Global.ProductDetailList == null || Global.ProductDetailList.Count == 0)
            {
                Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            }

            // Search by MaterialId, Name, or Barcode
            Product product = Global.ProductDetailList
                .FirstOrDefault(p =>
                    (!string.IsNullOrEmpty(p.MaterialId) && p.MaterialId.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(searchText)) /* ||
                    (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(searchText)) */
                )!;

            if (product != null)
            {
                ShowProductPrices(product);
            }
            else
            {
                PRODUCTNAME.Text = "Product Not Found!";
                ClearPrices();
            }
        }
        private void ShowProductPrices(Product product)
        {
            PRODUCTNAME.Text = product.Name;

            LabelPPP.Text = product.PurchasePrice.ToString("N2");
            LabelCP.Text = product.CostPrice.ToString("N2");
            LabelMRP.Text = product.Msrp.ToString("N2");
            LabelRP.Text = product.RetailPrice.ToString("N2");
            LabelWSP.Text = product.WholdSalePrice.ToString("N2");
            LabelLP.Text = product.LinePrice.ToString("N2");
        }

        private void ClearPrices()
        {
            LabelPPP.Text = "0.00";
            LabelCP.Text = "0.00";
            LabelMRP.Text = "0.00";
            LabelRP.Text = "0.00";
            LabelWSP.Text = "0.00";
            LabelLP.Text = "0.00";
        }
    }
}
