using fa.api.utils;
using fa.libraries.Validation;
using fa.views.purchase;
using System;
using System.Windows.Forms;

namespace fa.views.catalog
{
    public partial class FormItemPriceCalculator : FormBase
    {
        public static string EnterRetailXFactorErrorMsg = "Please enter product retail X-factor";
        public static string EnterWholeSaleXFactorErrorMsg = "Please enter product whole sale X-factor";
        public static string EnterCostErrorMsg = "Please enter product cost";
        public static string EnterRetailMarginErrorMsg = "Please enter retail margin";
        public static string EnterWholeSalMarginErrorMsg = "Please Enter whole sale margin";
        public static string EnterPercentageErrorMsg = "percentage do not exceed 100.";
        FormBase parent = null;
        public FormItemPriceCalculator(object sender)
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

        private void FormItemPriceCalculator_Load(object sender, EventArgs e)
        {
            if (parent is FormCatalog)
            {
                TextBoxProductCode.Text = ((FormCatalog)parent).TextBoxProductCode.Text;
                TextBoxProductName.Text = ((FormCatalog)parent).TextBoxProductName.Text;
                TextBoxProductCost.Text = ((FormCatalog)parent).TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = ((FormCatalog)parent).TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = ((FormCatalog)parent).TextBoxProductXFactorWholeSale.Text;

            }
            else if (parent is FormPurchaseEntryNew)
            {
                TextBoxProductCode.Text = ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryMaterialId.Text;
                TextBoxProductName.Text = ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryProductName.Text;
                TextBoxProductCost.Text = ((FormPurchaseEntryNew)parent).TextBoxProductPurchasePrice.Text;
                TextBoxXFactorRetail.Text = ((FormPurchaseEntryNew)parent).TextBoxProductXFactorRetail.Text;
                TextBoxXFactorWholeSale.Text = ((FormPurchaseEntryNew)parent).TextBoxProductXFactorWholeSale.Text;
                TextBoxProductCost.Enabled = false;
            }
        }

        private void TextBoxProductCost_TextChanged(object sender, EventArgs e)
        {
            MarginCalulator();
            //if (decimal.TryParse(TextBoxProductCost.Text, out decimal productCost))
            //{
            //    TextBoxProductCost.Text = productCost.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            //}
        }

        private void MarginCalulator()
        {
            if (!string.IsNullOrEmpty(TextBoxProductCost.Text.Trim()))
            {
                if (!string.IsNullOrEmpty(TextBoxXFactorRetail.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(TextBoxXFactorWholeSale.Text.Trim()))
                    {
                        CatalogErrorMsg.Text = "";
                        decimal Cost = decimal.Parse(TextBoxProductCost.Text != "" ? TextBoxProductCost.Text : "0");
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
                CatalogErrorMsg.Text = EnterCostErrorMsg;
                TextBoxProductCost.Select();
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

        private void BtnCatalogSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if (parent is FormCatalog)
                {
                    ((FormCatalog)parent).TextBoxProductCost.Text = TextBoxProductCost.Text;
                    ((FormCatalog)parent).TextBoxProductRetailPrice.Text = TextBoxRetailPrice.Text;
                    ((FormCatalog)parent).TextBoxProductWholeSalePrice.Text = TextBoxWholesalePrice.Text;
                }
                else if (parent is FormPurchaseEntryNew)
                {
                    ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryRetailPrice.Text = TextBoxRetailPrice.Text;
                    ((FormPurchaseEntryNew)parent).TextBoxPurchaseEntryWholeSalePrice.Text = TextBoxWholesalePrice.Text;
                }
                this.Close();
            }
        }

        private void BtnCatalogCancel_Click(object sender, EventArgs e)
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
    }
}
