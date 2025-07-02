using fa;
using fa.api.utils;
using fa.libraries.Validation;
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

namespace Fa.views.catalog
{
    public partial class FormItemSpecialPriceCalculator : FormBase
    {
        public static string EnterRetailXFactorErrorMsg = "Please enter product retail X-factor";
        public static string EnterWholeSaleXFactorErrorMsg = "Please enter product whole sale X-factor";
        public static string EnterCostErrorMsg = "Please enter product cost";
        public static string EnterRetailMarginErrorMsg = "Please enter retail margin";
        public static string EnterWholeSalMarginErrorMsg = "Please Enter whole sale margin";
        public static string EnterPercentageErrorMsg = "percentage do not exceed 100.";
        public static string EnterAddedCostPercentageErrorMsg = "Please enter added cost percentage";

        FormBase parent = null!;
        public FormItemSpecialPriceCalculator(object sender)
        {
            if (sender is FormCatalog)
            {
                parent = (FormCatalog)sender;
            }
            else if (sender is FormPurchaseEntryNew)
            {
                parent = (FormPurchaseEntryNew)sender;
            }
            InitializeComponent();
        }

        private void FormItemSpecialPriceCalculator_Load(object sender, EventArgs e)
        {
            // Set default percentages
            TextBoxAddedCostPercentage.Text = "15";
            TextBoxRetailMargin.Text = "30";    // Default 30% retail margin
            TextBoxWholesaleMargin.Text = "40"; // Default 40% wholesale margin

            if (parent is FormCatalog)
            {
                TextBoxProductCode.Text = ((FormCatalog)parent).TextBoxProductCode.Text;
                TextBoxProductName.Text = ((FormCatalog)parent).TextBoxProductName.Text;
                TextBoxPurchasePrice.Text = ((FormCatalog)parent).TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = ((FormCatalog)parent).TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = ((FormCatalog)parent).TextBoxProductXFactorWholeSale.Text;

                UpdateAllCalculations();
            }
            else if (parent is FormPurchaseEntryNew)
            {
                TextBoxProductCode.Text = ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryMaterialId.Text;
                TextBoxProductName.Text = ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryProductName.Text;
                TextBoxPurchasePrice.Text = ((FormPurchaseEntryNew)parent).TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = ((FormPurchaseEntryNew)parent).TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = ((FormPurchaseEntryNew)parent).TextBoxProductXFactorWholeSale.Text;

                UpdateAllCalculations();
                TextBoxPurchasePrice.Enabled = false;
            }
        }


        private void MarginCalulator()
        {
            if (!string.IsNullOrEmpty(TextBoxPurchasePrice.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(TextBoxXFactorRetail.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(TextBoxXFactorWholeSale.Text.Trim()))
                    {
                        CatalogErrorMsg.Text = "";
                        decimal Cost = decimal.Parse(TextBoxPurchasePrice.Text != "" ? TextBoxPurchasePrice.Text : "0");
                        decimal RetailPer = decimal.Parse(TextBoxRetailMargin.Text != "" ? TextBoxRetailMargin.Text : "0");
                        decimal WholeSalePer = decimal.Parse(TextBoxWholesaleMargin.Text != "" ? TextBoxWholesaleMargin.Text : "0");
                        int RXFactor = int.Parse(TextBoxXFactorRetail.Text.Trim());
                        int WXFactor = int.Parse(TextBoxXFactorWholeSale.Text.Trim());

                        if (RetailPer > 100 || WholeSalePer > 100)
                        {
                            CatalogErrorMsg.Text = EnterPercentageErrorMsg;
                        }
                        else
                        {
                            decimal RetailMar = ((Cost / 100) * RetailPer) + Cost;
                            decimal WholosaleMar = ((Cost / 100) * WholeSalePer) + Cost;

                            TextBoxRetailPrice.Text = ((RetailMar / RXFactor) + (RetailMar % RXFactor)).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            TextBoxWholesalePrice.Text = ((WholosaleMar / WXFactor) + (WholosaleMar % WXFactor)).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                    }
                    else
                    {
                        CatalogErrorMsg.Text = EnterWholeSaleXFactorErrorMsg;
                    }
                }
                else
                {
                    CatalogErrorMsg.Text = EnterRetailXFactorErrorMsg;
                }
            }
            else
            {
                CatalogErrorMsg.Text = EnterCostErrorMsg;
            }
        }
        private Boolean validate()
        {
            CatalogErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxProductCost.Text.Trim()))
            {
                CatalogErrorMsg.Text = "Please enter purchase price";
                TextBoxProductCost.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxAddedCostPercentage.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterAddedCostPercentageErrorMsg;
                TextBoxAddedCostPercentage.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxPurchasePrice.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterCostErrorMsg;
                TextBoxPurchasePrice.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxRetailMargin.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterRetailMarginErrorMsg;
                TextBoxRetailMargin.Select();
                return false;
            }
            decimal RetailPer = decimal.Parse(TextBoxRetailMargin.Text != "" ? TextBoxRetailMargin.Text : "0");
            if (RetailPer > 100)
            {
                CatalogErrorMsg.Text = EnterPercentageErrorMsg;
                TextBoxRetailMargin.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxWholesaleMargin.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterWholeSalMarginErrorMsg;
                TextBoxWholesaleMargin.Select();
                return false;
            }
            decimal WholeSalePer = decimal.Parse(TextBoxWholesaleMargin.Text != "" ? TextBoxWholesaleMargin.Text : "0");
            if (WholeSalePer > 100)
            {
                CatalogErrorMsg.Text = EnterPercentageErrorMsg;
                TextBoxWholesaleMargin.Select();
                return false;
            }
            return true;
        }

        private void BtnPriceCalculatorSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if (parent is FormCatalog)
                {
                    // Save all calculated values back to parent form
                    ((FormCatalog)parent).TextBoxProductPurchasePrice.Text = TextBoxPurchasePrice.Text;
                    ((FormCatalog)parent).TextBoxProductCost.Text = TextBoxProductCost.Text;
                    ((FormCatalog)parent).TextBoxProductRetailPrice.Text = TextBoxRetailPrice.Text;
                    ((FormCatalog)parent).TextBoxProductWholeSalePrice.Text = TextBoxWholesalePrice.Text;
                    ((FormCatalog)parent).TextBoxProductMSRP.Text = TextBoxMrpPrice.Text;

                    // Save the margin percentages if needed
                    //((FormCatalog)parent).TextBoxProductRetailMargin.Text = TextBoxRetailMargin.Text;
                    //((FormCatalog)parent).TextBoxProductWholesaleMargin.Text = TextBoxWholesaleMargin.Text;

                    // Save the added cost percentage if needed
                    //((FormCatalog)parent).TextBoxProductAddedCostPercentage.Text = TextBoxAddedCostPercentage.Text;
                }
                else if (parent is FormPurchaseEntryNew)
                {
                    ((FormPurchaseEntryNew)parent).TextBoxProductPurchasePrice.Text = TextBoxPurchasePrice.Text;
                    ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryRetailPrice.Text = TextBoxRetailPrice.Text;
                    ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryWholeSalePrice.Text = TextBoxWholesalePrice.Text;
                    // Uncomment if MRP field exists in purchase entry form
                    // ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryMRPPrice.Text = TextBoxMrpPrice.Text;
                }

                // Set dialog result if needed
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void UpdateProductCostAndMRP()
        {
            if (decimal.TryParse(TextBoxPurchasePrice.Text, out decimal purchasePrice) &&
                decimal.TryParse(TextBoxAddedCostPercentage.Text, out decimal addedCostPercentage))
            {
                if (addedCostPercentage > 100)
                {
                    CatalogErrorMsg.Text = EnterPercentageErrorMsg;
                    return;
                }

                // Calculate product cost
                decimal productCost = purchasePrice + (purchasePrice * addedCostPercentage / 100);
                TextBoxProductCost.Text = productCost.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                // Calculate MRP (220% of purchase price)
                TextBoxMrpPrice.Text = (purchasePrice * 2.2m).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
        }

        private void UpdateMarginPrices()
        {
            if (decimal.TryParse(TextBoxMrpPrice.Text, out decimal mrpPrice) &&
                decimal.TryParse(TextBoxRetailMargin.Text, out decimal retailMargin) &&
                decimal.TryParse(TextBoxWholesaleMargin.Text, out decimal wholesaleMargin))
            {
                // Calculate retail price (MRP - Retail Margin % of MRP)
                decimal retailPrice = mrpPrice - (mrpPrice * retailMargin / 100);
                TextBoxRetailPrice.Text = retailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                // Calculate wholesale price (MRP - Wholesale Margin % of MRP)
                decimal wholesalePrice = mrpPrice - (mrpPrice * wholesaleMargin / 100);
                TextBoxWholesalePrice.Text = wholesalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
        }

        private void UpdateAllCalculations()
        {
            // First update product cost and MRP
            UpdateProductCostAndMRP();

            // Then update retail and wholesale prices based on MRP
            UpdateMarginPrices();
        }

        private void TextBoxAddedPercentsage_TextChanged(object sender, EventArgs e)
        {
            UpdateAllCalculations(); // This will update all dependent prices
        }

        private void TextBoxPurchasePrice_TextChanged(object sender, EventArgs e)
        {
            UpdateAllCalculations(); // This will update all dependent prices
        }

        private void TextBoxRetailMargin_TextChanged(object sender, EventArgs e)
        {
            UpdateMarginPrices(); // Only update retail/wholesale prices
        }

        private void TextBoxWholesaleMargin_TextChanged(object sender, EventArgs e)
        {
            UpdateMarginPrices(); // Only update retail/wholesale prices
        }

        private void BtnPriceCalculatorCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnPriceCalculatorCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F8))
            {
                BtnPriceCalculatorSave.PerformClick();
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }

        private void TextBoxXFactorRetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }

        private void TextBoxXFactorWholeSale_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }
    }
}
