using fa.model.Catalog;
using fa.views;
using fa.views.catalog;
using fa.views.purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.NDI;

namespace Fa.views.catalog
{
    public partial class FormSpecialPrice : FormBase
    {
        public static string EnterRetailXFactorErrorMsg = "Please enter product retail X-factor";
        public static string EnterWholeSaleXFactorErrorMsg = "Please enter product whole sale X-factor";
        public static string EnterCostErrorMsg = "Please enter product cost";
        public static string EnterRetailMarginErrorMsg = "Please enter retail margin";
        public static string EnterWholeSalMarginErrorMsg = "Please Enter whole sale margin";
        public static string EnterPercentageErrorMsg = "percentage do not exceed 100.";
        public static string EnterAddedCostPercentageErrorMsg = "Please enter added cost percentage";

        FormBase parent = null!;
        public static decimal MrpPercentage = 100; // NOT nullable
        private bool isUpdating = false;
        private float _mrp = 0;
        private float _wholesalePrice;

        public FormSpecialPrice(object sender, float wholesalePrice)
        {
            _wholesalePrice = wholesalePrice;

            if (sender is FormCatalog)
                parent = (FormCatalog)sender;
            else if (sender is FormPurchaseEntryNew)
                parent = (FormPurchaseEntryNew)sender;

            InitializeComponent();
        }

        private void FormSpecialPrice_Load(object sender, EventArgs e)
        {
            // defaults if textboxes don’t exist or are empty
            float lineMargin = 40f;
            float specMargin = 50f;

            // If you DO have margin textboxes, try parse; else keep defaults
            if (float.TryParse(TextBoxLineMargin?.Text, out var lm)) lineMargin = lm;
            if (float.TryParse(TextBoxSpecialMargin?.Text, out var sm)) specMargin = sm;

            TextBoxProductWholeSalePrice.Text = _wholesalePrice.ToString("0.##");

            // Pull basic product info from parent
            Product productFromParent = null!;
            if (parent is FormCatalog cat)
            {
                TextBoxProductCode.Text = cat.TextBoxProductCode.Text;
                TextBoxProductName.Text = cat.TextBoxProductName.Text;
            }


            // Prefill identity fields (as you already do)
            // ...

            // Prefill Line/Special:
            float? mrp = (float?)productFromParent?.Msrp;
            float? existingLine = (float?)(productFromParent?.LinePrice ?? 0f);
            float? existingSpec = (float?)(productFromParent?.SpecialPrice ?? 0f);

            var (resolvedLine, resolvedSpec) =
                PriceHelper.ResolveLineAndSpecialPrice(mrp, existingLine, existingSpec, lineMargin, specMargin);

            // If both are still 0 (e.g., MRP is 0 and no saved prices), leave blank for user to type
            TextBoxLinePrice.Text = resolvedLine > 0 ? resolvedLine.ToString("0.##") : "";
            TextBoxSpecialPrice.Text = resolvedSpec > 0 ? resolvedSpec.ToString("0.##") : "";
            // Default margins
            //TextBoxLineMargin.Text = "25";
            //TextBoxSpecialMargin.Text = "30";

            //// ✅ Fetch parent values
            //if (parent is FormCatalog catalog)
            //{
            //    TextBoxProductCode.Text = catalog.TextBoxProductCode.Text;
            //    TextBoxProductName.Text = catalog.TextBoxProductName.Text;
            //    TextBoxXFactorRetail.Text = catalog.TextBoxProductXFactorRetail.Text;
            //    TextBoxXFactorWholeSale.Text = catalog.TextBoxProductXFactorWholeSale.Text;

            //    float.TryParse(catalog.TextBoxProductMSRP.Text, out _mrp);
            //}

            //if (_mrp > 0)
            //{
            //    RecalculatePrices();
            //}
            //else
            //{
            //    // Allow manual entry when no MRP
            //    TextBoxLinePrice.Text = "0.00";
            //    TextBoxSpecialPrice.Text = "0.00";
            //}

            //// Hook events for live calculation
            //TextBoxLineMargin.TextChanged += MarginTextChanged;
            //TextBoxSpecialMargin.TextChanged += MarginTextChanged;
        }


        private void BtnPriceCalculatorSave_Click(object sender, EventArgs e)
        {
            if (parent is FormCatalog catalogParent)
            {
                if (float.TryParse(TextBoxLinePrice.Text, out float linePrice))
                    catalogParent.LineWholesalePrice = linePrice;   // <-- store here ONLY

                if (float.TryParse(TextBoxSpecialPrice.Text, out float specialPrice))
                    catalogParent.SpecialPriceItem = specialPrice;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void MarginTextChanged(object? sender, EventArgs e)
        {
            if (_mrp > 0)   // Only auto-calc when MRP exists
            {
                RecalculatePrices();
            }
        }

        private void RecalculatePrices()
        {
            if (_mrp <= 0) return;

            float lineMargin = 0, specialMargin = 0;
            float.TryParse(TextBoxLineMargin.Text, out lineMargin);
            float.TryParse(TextBoxSpecialMargin.Text, out specialMargin);

            float linePrice = _mrp - (_mrp * lineMargin / 100f);
            float specialPrice = _mrp - (_mrp * specialMargin / 100f);

            TextBoxLinePrice.Text = linePrice.ToString("0.00");
            TextBoxSpecialPrice.Text = specialPrice.ToString("0.00");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
