using fa.api.catalog;
using fa.api.UserProfile;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Catalog;
using fa.model.UserProfile;
using System;
using System.Linq;
using fa.api.security;
using System.Windows.Forms;
using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using fa.views.controls.accounting;

namespace fa.views.sales
{
    public partial class PriceOverride : FormBase
    {
        public static string SaveSuccessText = "Successfully updated.";
        public static string EnterUserNameErrorMsg = "Please enter User Name.";
        public static string EnterPasswordErrorMsg = "Please enter Password.";
        public static string InvalidLoginErrorMsg = "Invalid Username/Password.";

        FormBase parent = null;
        public long ProductId = 0L;
        public long BatchId = 0L;
        public bool IsBatch;
        public DateTime? dateTime;
        public PriceType PriceType;
        public PriceOverride(object sender)
        {
            if (sender is FormItembasedSales)
            {
                parent = (FormItembasedSales)sender;
            }
            else if (sender is FormQuote)
            {
                parent = (FormQuote)sender;
            }
            InitializeComponent();
        }

        private void PriceOverride_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TextBoxOverridePassword.ResetText();
            TextBoxOverrideUser.ResetText();
            LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoById(ProductId));
            TextBoxOverridePrice.Select();
            TextBoxOverridePrice.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            LoadProductCombo();
            TextBoxPurchaseEntryMaterialId.Text = Product.MaterialId;
            TextBoxPurchaseEntryProductName.Text = Product.Name;
            TextBoxProductPurchasePrice.Text = Product.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryRetailPrice.Text = Product.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryWholeSalePrice.Text = Product.WholdSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryMsrp.Text = Product.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ComboBoxProductPurchesUOM.Text = Product.UOM;
            ComboBoxProductRetailUOM.Text = Product.RetailUOM;
            ComboBoxProductWholeSaleUOM.Text = Product.WholesaleUOM;
            TextBoxProductXFactorRetail.Text = Product.RetailXFactor.ToString();
            TextBoxProductXFactorWholeSale.Text = Product.WholesaleXFactor.ToString();
            ItemTaxDetails.Clear();
            ItemTaxDetails.CurrentDate = dateTime;
            ItemTaxDetails.ProductId = Product.Id;
            EnableProductAdditinalDetails(false);
        }
        private void LoadProductCombo()
        {
            ComboUtils.InitializeAllUniquePurchaseUOMCombo(ComboBoxProductPurchesUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueRetailUOMCombo(ComboBoxProductRetailUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueWholeSaleUOMCombo(ComboBoxProductWholeSaleUOM, Global.Company.CompanyId);
        }
        private void EnableProductAdditinalDetails(bool enable)
        {
            ComboBoxProductPurchesUOM.Visible = enable;
            ComboBoxProductRetailUOM.Visible = enable;
            ComboBoxProductWholeSaleUOM.Visible = enable;
        }
        private Product GetProductFromForm()
        {
            Product lProduct = new Product();
            lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            if(PriceType==PriceType.Retail)
            {
                lProduct.RetailPrice = float.Parse(TextBoxOverridePrice.Text);
            }
            else if (PriceType == PriceType.Wholesale)
            {
                lProduct.WholdSalePrice = float.Parse(TextBoxOverridePrice.Text);
            }
            else if (PriceType == PriceType.MaxRetailPrice)
            {
                lProduct.Msrp = float.Parse(TextBoxOverridePrice.Text);
            }
            if (ItemTaxDetails.CatalogItemSalesTaxMaps.Count > 0)
            {
                lProduct.SalesTax = ItemTaxDetails.CatalogItemSalesTaxMaps;
            }
            lProduct.Company = null;
            lProduct.InventoryAccount= null;
            lProduct.PurchaseAccount = null;
            lProduct.SalesAccount= null;
            lProduct.InventoryAccountLocal= null;
            lProduct.PurchaseAccountLocal = null;
            lProduct.SalesAccountLocal= null;
            lProduct.Parent = null;
            lProduct.ProductFamily = null;
            return lProduct;
        }
        private InventoryBatch GetInventoryBatchFromForm()
        {
            InventoryBatch lInventoryBatch = new InventoryBatch();
            lInventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatchId);
            if (PriceType == PriceType.Retail)
            {
                lInventoryBatch.RetailSalePrice = float.Parse(TextBoxOverridePrice.Text);
            }
            else if (PriceType == PriceType.Wholesale)
            {
                lInventoryBatch.WholeSalePrice = float.Parse(TextBoxOverridePrice.Text);
            }
            else if (PriceType == PriceType.MaxRetailPrice)
            {
                lInventoryBatch.MaxRetailPrice = float.Parse(TextBoxOverridePrice.Text);
            }          
            return lInventoryBatch;
        }
        private void BtnOverride_Click(object sender, EventArgs e)
        {
            if (validateFormProduct())
            {
                Product lProduct = GetProductFromForm();
                if (lProduct!=null)
                {
                    if(IsBatch)
                    {
                        InventoryBatch lInventoryBatch = GetInventoryBatchFromForm();
                        if (lInventoryBatch != null)
                        {
                            Product lProductFromDB = CatalogProductManager.Instance.UpdateProduct(lProduct, lInventoryBatch);
                        }
                    }
                    else
                    {
                        Product lProductFromDB = CatalogProductManager.Instance.UpdateProduct(lProduct);
                    }
                    ErrorMsg.Text = SaveSuccessText;
                }
                else
                {
                    MessageBox.Show("Somthing went wrong, please check this product is still valid.");
                }
                this.Close();
            }
        }
        private Boolean validateFormProduct()
        {
            ErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxOverrideUser.Text.Trim()))
            {
                ErrorMsg.Text = EnterUserNameErrorMsg;
                TextBoxOverrideUser.Select();
                return false;
            }

            if (string.IsNullOrEmpty(TextBoxOverridePassword.Text.Trim()))
            {
                ErrorMsg.Text = EnterPasswordErrorMsg;
                TextBoxOverridePassword.Select();
                return false;
            }
            User User = UserManager.Instance.GetUserByLogin(TextBoxOverrideUser.Text);
            Encryptor encryptor = new Encryptor();
            if (User != null && User.Password == encryptor.EncryptText(TextBoxOverridePassword.Text))
            {
                return true;
            }
            ErrorMsg.Text = InvalidLoginErrorMsg;  
            return false;
        }
        
        private void PriceOverride_FormClosing(object sender, FormClosingEventArgs e)
        {
            double Price = 0.00;
            if (IsBatch && BatchId!=0L)
            {
                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatchId);
                if (InventoryBatch != null)
                {
                    Price = PriceType == PriceType.Retail ? InventoryBatch.RetailSalePrice : PriceType == PriceType.Wholesale ? InventoryBatch.WholeSalePrice : InventoryBatch.MaxRetailPrice;
                }
            }
            else
            {
                Product lProductFromDB = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                if (lProductFromDB != null)
                {
                    Price = PriceType == PriceType.Retail ? lProductFromDB.RetailPrice : PriceType == PriceType.Wholesale ? lProductFromDB.WholdSalePrice : lProductFromDB.Msrp;
                }
            }
            if (parent is FormQuote)
            {
                ((FormQuote)parent).Oprice = ((FormQuote)parent).Price = Price;
            }
            else
            {
                ((FormItembasedSales)parent).Oprice = ((FormItembasedSales)parent).Price = Price;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
