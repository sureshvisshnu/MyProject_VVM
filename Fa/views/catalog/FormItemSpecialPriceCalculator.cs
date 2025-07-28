using fa;
using fa.api.utils;
using fa.libraries.Validation;
using fa.model.Catalog;
using fa.views;
using fa.views.catalog;
using fa.views.purchase;
using FADataAccessLibrary.Api.catalog;
using Microsoft.EntityFrameworkCore;
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
        public static decimal MrpPercentage = 100; // NOT nullable

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

        private async void FormItemSpecialPriceCalculator_Load(object sender, EventArgs e)
        {
            comboMrpPercentage.Items.AddRange(new object[] { 50, 70, 100, 120 });
            comboMrpPercentage.DropDownStyle = ComboBoxStyle.DropDown; // Allow manual input
            comboMrpPercentage.SelectedItem = 120;

            // Set default percentages
            TextBoxAddedCostPercentage.Text = "15";
            TextBoxRetailMargin.Text = "30";    // Default 30% retail margin
            TextBoxWholesaleMargin.Text = "40"; // Default 40% wholesale margin

            // ✅ First: set product information from parent
            if (parent is FormCatalog catalog)
            {
                TextBoxProductCode.Text = catalog.TextBoxProductCode.Text;
                TextBoxProductName.Text = catalog.TextBoxProductName.Text;
                TextBoxPurchasePrice.Text = catalog.TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = catalog.TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = catalog.TextBoxProductXFactorWholeSale.Text;

                TextBoxPurchasePrice.Enabled = true;
            }
            else if (parent is FormPurchaseEntryNew entry)
            {
                TextBoxProductCode.Text = entry.TextBoxPurchaseEntryMaterialId.Text;
                TextBoxProductName.Text = entry.TextBoxPurchaseEntryProductName.Text;
                TextBoxPurchasePrice.Text = entry.TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = entry.TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = entry.TextBoxProductXFactorWholeSale.Text;

                TextBoxPurchasePrice.Enabled = false;
            }

            // ✅ Then: fetch default or saved percentages using the correct product code
            try
            {
                await GetPercentage(); // await was missing here
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading percentages: {ex.Message}");
                // Use defaults already set
            }

            // ✅ Finally: update calculated values
            UpdateAllCalculations();
        }

        private async Task GetPercentage()
        {
            var percentages = await ProductSalePercentageManager.Instance.GetProductSalePercentageAsync(TextBoxProductCode.Text);

            decimal mrpToSet = 120; // fallback

            if (percentages != null)
            {
                TextBoxAddedCostPercentage.Text = percentages.AddedCostPercentage.ToString();
                TextBoxRetailMargin.Text = percentages.RetailMarginPercentage.ToString();
                TextBoxWholesaleMargin.Text = percentages.WholesaleMarginPercentage.ToString();
                mrpToSet = percentages.MrpPercentage;
            }
            else
            {
                var storage = await ProductSalePercentageManager.Instance.GetCompanyDefaultPercentagesAsync(Global.Company.CompanyId);
                if (storage != null)
                {
                    TextBoxAddedCostPercentage.Text = storage.DefaultAddedCostPercentage.ToString();
                    TextBoxRetailMargin.Text = storage.DefaultRetailMarginPercentage.ToString();
                    TextBoxWholesaleMargin.Text = storage.DefaultWholesaleMarginPercentage.ToString();
                    mrpToSet = storage.DefaultMrpPercentage;
                }
            }

            // Add to combo box if not already in the list
            if (!comboMrpPercentage.Items.Contains(mrpToSet))
            {
                comboMrpPercentage.Items.Add(mrpToSet);
            }

            comboMrpPercentage.SelectedItem = mrpToSet;
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

        private async void BtnPriceCalculatorSave_Click(object sender, EventArgs e) // Note async void for event handler
        {
            if (validate())
            {
                try
                {
                    var percentages = new ProductPercentage
                    {
                        ProductCode = TextBoxProductCode.Text,
                        // Don't try to get ProductId here - let FormCatalog handle it
                        ProductId = 0, // Temporary value
                        CompanyId = Global.Company.CompanyId,
                        AddedCostPercentage = decimal.Parse(TextBoxAddedCostPercentage.Text),
                        RetailMarginPercentage = decimal.Parse(TextBoxRetailMargin.Text),
                        WholesaleMarginPercentage = decimal.Parse(TextBoxWholesaleMargin.Text),
                        MrpPercentage = decimal.Parse(TextBoxMrpPercentage.Text),
                        IsActive = true
                    };

                    // Return the percentages to FormCatalog
                    if (parent is FormCatalog catalogParent)
                    {
                        // Store the percentages in FormCatalog
                        catalogParent.PendingPercentages = percentages;
                        UpdateParentForm(catalogParent);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid numeric values for all percentages",
                                  "Invalid Input",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
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

                // Calculate MRP using selected percentage (convert to multiplier) and round to whole number
                decimal mrpMultiplier = 1 + (MrpPercentage / 100);
                decimal mrpPrice = productCost * mrpMultiplier;
                TextBoxMrpPrice.Text = Math.Round(mrpPrice, 0).ToString(); // Rounds to nearest whole number
            }
        }
        private void UpdateMarginPrices()
        {
            if (decimal.TryParse(TextBoxMrpPrice.Text, out decimal mrpPrice) &&
                decimal.TryParse(TextBoxRetailMargin.Text, out decimal retailMargin) &&
                decimal.TryParse(TextBoxWholesaleMargin.Text, out decimal wholesaleMargin))
            {
                // Calculate retail price (MRP - Retail Margin % of MRP) and round to whole number
                decimal retailPrice = mrpPrice - (mrpPrice * retailMargin / 100);
                TextBoxRetailPrice.Text = Math.Round(retailPrice, 0).ToString(); // Rounds to nearest whole number

                // Calculate wholesale price (MRP - Wholesale Margin % of MRP) and round to whole number
                decimal wholesalePrice = mrpPrice - (mrpPrice * wholesaleMargin / 100);
                TextBoxWholesalePrice.Text = Math.Round(wholesalePrice, 0).ToString(); // Rounds to nearest whole number
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

        private void comboMrpPercentage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMrpPercentage.SelectedItem != null)
            {
                MrpPercentage = Convert.ToDecimal(comboMrpPercentage.SelectedItem);
                TextBoxMrpPercentage.Text = MrpPercentage.ToString();
                UpdateAllCalculations(); // Recalculate all prices
            }
        }
        #region Helper Methods
        private decimal ValidatePercentage(string input, string fieldName)
        {
            if (!decimal.TryParse(input, out var value) || value < 0 || value > 100)
            {
                throw new ArgumentException($"Invalid {fieldName} percentage (0-100)");
            }
            return value;
        }

        private decimal ValidateMrpPercentage()
        {
            if (comboMrpPercentage.SelectedItem == null)
            {
                throw new ArgumentException("Please select an MRP percentage");
            }
            return decimal.Parse(comboMrpPercentage.SelectedItem.ToString()!);
        }

        private async Task SavePercentagesWithRetry(ProductPercentage percentages)
        {
            const int maxRetries = 2;
            int attempt = 0;

            while (true)
            {
                try
                {
                    await ProductSalePercentageManager.Instance.SaveProductPercentagesAsync(percentages);
                    return;
                }
                catch (DbUpdateConcurrencyException) when (attempt++ < maxRetries)
                {
                    // Wait and retry
                    await Task.Delay(500);
                }
            }
        }

        private void UpdateParentForm(FormCatalog catalogParent)
        {
            catalogParent.TextBoxProductPurchasePrice.Text = TextBoxPurchasePrice.Text;
            catalogParent.TextBoxProductCost.Text = TextBoxProductCost.Text;
            catalogParent.TextBoxProductRetailPrice.Text = TextBoxRetailPrice.Text;
            catalogParent.TextBoxProductWholeSalePrice.Text = TextBoxWholesalePrice.Text;
            catalogParent.TextBoxProductMSRP.Text = TextBoxMrpPrice.Text;
        }
        #endregion

        private void comboMrpPercentage_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(comboMrpPercentage.Text, out var mrp))
            {
                MrpPercentage = mrp;
                TextBoxMrpPercentage.Text = MrpPercentage.ToString();
                UpdateAllCalculations(); // trigger recalculation
            }
        }
    }
}
