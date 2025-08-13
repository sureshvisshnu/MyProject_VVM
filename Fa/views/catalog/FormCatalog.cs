using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.hms.common;
using fa.model.OrderManagement;
using fa.reports.catalog;
using fa.views.controls;
using fa.views.purchase;
using Fa.api.catalog;
using Fa.api.OrderManagement;
using Fa.views.catalog;
using FADataAccessLibrary.Api.catalog;
using FADataAccessLibrary.Model.Common;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.Libs.NDI;
using VisioForge.MediaFramework.Helpers;
using static fa.api.catalog.CatalogItemManager;

namespace fa.views.catalog
{
    public partial class FormCatalog : FormBase
    {
        public static string NoCategoryFoundErrorMsg = "No category found !. Please add category.";
        public static string NoProductFamilyFoundErrorMsg = "No product family found !. Please add product family.";
        public static string NoProductFoundErrorMsg = "No such {0} like product found !.";
        public static string RemoveSuccess = "{0} romove successfull";
        public static string DeleteCategoryConfirmText = "Do you want to delete the category {0}?\n[If you delete Categeory it delete there subcategory,Productfamily and product also]";
        public static string DeleteProductFamilyConfirmText = "Do you want to delete the productfamily {0}?\n[If you delete product family it delete there product also]";
        public static string DeleteProductConfirmText = "Do you want to delete the product {0}?";
        public static string SaveCatalogSuccessText = "Saved success...";
        public static string SaveProductFamilySuccessText = "Saved success...";
        public static string SaveProductSuccessText = "Saved success...";
        public static string DeleteErrorText = "Error deleting the {0}!, Please retry";
        public static string DeleteErrorProduct = "Item cannot be deleted as it is used in sales/purchase";
        public static string UniqueNameErrorMsg = "{0} {1} already exists";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string UniqueProductCodeErrorMsg = "Product code {0} already used, Generate new code";
        public static string EnterNameErrorMsg = "Please enter {0} name";
        public static string EnterDefaultDisErrorMsg = "Please enter proper default discount";
        public static string ChooseProductParentErrorMsg = "Please choose product parent";
        public static string ChooseProductFamilyParentErrorMsg = "Please choose product family parent";
        public static string EnterPurUomErrorMsg = "Please enter purchase UOM";
        public static string EnterRetailUomErrorMsg = "Please enter retail UOM";
        public static string EnterRackNumberErrorMsg = "Please enter rack number";
        public static string EnterRetailXFactorErrorMsg = "Please enter retail Xfactor";
        public static string EnterWholeSalUomErrorMsg = "Please enter whole sale UOM";
        public static string EnterWholeSalXFactorErrorMsg = "Please enter whole sale Xfactor";
        public static string EnterProductCodeErrorMsg = "Please enter product code";
        public static string EnterBatchDetailErrorMsg = "Please enter opening stock detail";
        public static string EnterOpenStockErrorMsg = "Please check opening stock";
        public static string EnterMrpErrorMsg = "Please enter correct msrp.";
        public static string EnterCharErrorMsg = "Please enter minimum 2 character.";

        public ProductPercentage PendingPercentages { get; set; }

        public bool CreateCatalogOnLoad = false;
        private bool FixMissingPercentagesOnLoad = false;

        AccountManager AccountManager = null!;
        CategoryManager CategoryManager = null!;
        CatalogProductManager CatalogProductManager = null!;
        CatalogProductFamilyManager CatalogProductFamilyManager = null!;
        DateTime EffectiveStartDate = new DateTime(2017, 07, 01);
        DateTime EffectiveEndDate = new DateTime(2400, 12, 31);
        public long TaxCodeId = 0L;

        FormBase parent = null!;
        public FormCatalog(object sender)
        {
            if (sender is FormSearchItems)
            {
                parent = (FormSearchItems)sender;
            }
            CategoryManager = CategoryManager.Instance;
            AccountManager = AccountManager.Instance;

            CatalogProductManager = CatalogProductManager.Instance;
            CatalogProductFamilyManager = CatalogProductFamilyManager.Instance;
            InitializeComponent();
            excludedObjects = new string[] { "GridViewProductFamilyChildProduct", "TextBoxCatalogSearch", "TextBoxOpeningstock" };

        }
        private void FormCatalog_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetCategoryTab();
                LoadCategoryCombo();
                CheckProductPercentage();
                if (Global.softwareType == SoftwareType.VVMATRIX)
                {
                    if (Global.Company.BusinessType != BuisnessType.Pharmacy)
                    {
                        ComboBoxProductSchedule.Visible = false;
                        label50.Visible = false;
                        ComboBoxProductRackNumber.Location = new Point(4, 340);
                        LabelRackNumber.Location = new Point(4, 324);
                    }
                }

                if (CreateCatalogOnLoad)
                {
                    TextBoxCatalogSearch.Visible = false;
                    this.BtnCatalogEdit.Visible = false;
                    BtnCatalogNew.Visible = false;
                    BtnCatalogCancel.Visible = false;
                    BtnCatalogDelete.Visible = false;
                    BtnCatalogImport.Visible = false;
                    BtnCatalogCancel.Visible = false;
                    BtnCatalogImport.Visible = false;
                    BtnCatalogReport.Visible = false;
                    TreeViewCatalog.Visible = false;
                    TabControlCategory.Location = new Point(12, 12);
                    TabControlProductFamily.Location = new Point(12, 12);
                    TabControlProduct.Location = new Point(12, 12);
                    TabControl TabControl = (TabControlCategory.Visible) ? TabControlCategory : (TabControlProductFamily.Visible) ? TabControlProductFamily : TabControlProduct;
                    BtnCatalogSave.Location = new Point(TabControl.Width - 74, TabControl.Height + 15);
                    this.Size = new Size(TabControl.Right + 25, TabControl.Bottom + 90);
                    this.CenterToParent();
                    newProductSKUToolStripMenuItem.PerformClick();
                }
                else
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    LoadCatalogWithFilter();
                    sw.Stop();
                    Console.WriteLine("Tree load Elapsed={0}", sw.Elapsed);

                }

                EnableForm(false);
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void CheckProductPercentage()
        {
            if (Global.User.IsSuperAdmin) // or however your system defines it
            {
                BtnPercentage.Visible = true;
                BtnPercentage.Enabled = true;
            }
            else
            {
                BtnPercentage.Visible = false;
            }
        }
        private IList<CatalogItem> ItemList = null;
        private void LoadCatalogWithFilter()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string FilterString = TextBoxCatalogSearch.Text.Trim();
                TreeViewCatalog.Nodes.Clear();
                ItemList = CatalogItemManager.Instance.ListCatalogItemsByCompanyId(Global.Company.CompanyId, FilterString);
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var CatalogItem in ItemList.Where(x => x.ParentId == null))
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = CatalogItem.ToString();
                        treeNode.Name = CatalogItem.Id.ToString();
                        treeNode.ImageIndex = 0;
                        treeNode.ContextMenuStrip = contextSubCategeory;
                        TreeViewCatalog.Nodes.Add(treeNode);
                        LoadChildNodes(treeNode, CatalogItem.Id);
                    }
                }
                else
                {
                    if (TreeViewCatalog.Nodes.Count > 0)
                    {
                        CatalogErrorMsg.Text = "";
                    }
                    else
                    {
                        CatalogErrorMsg.Text = string.Format(NoProductFoundErrorMsg, FilterString);
                    }
                }
                if (TreeViewCatalog.Nodes.Count > 0)
                {
                    if (!string.IsNullOrEmpty(TextBoxCatalogSearch.Text.Trim()))
                    {
                        TreeViewCatalog.ExpandAll();
                    }
                    TreeViewCatalog.SelectedNode = TreeViewCatalog.Nodes[0];
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadChildNodes(TreeNode ParentNode, long ParentId)
        {
            foreach (var CatalogItem in ItemList.Where(x => x.ParentId == ParentId))
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Text = "...";
                ChildNode.Name = "...";
                ChildNode.ImageIndex = 0;
                ChildNode.ContextMenuStrip = contextSubCategeory;
                ParentNode.Nodes.Add(ChildNode);
                break;
            }
        }
        private void TreeViewCatalog_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (IsExpand)
            {
                Expand(e.Node);
            }
            Cursor.Current = Cursors.Default;
        }
        private void Expand(TreeNode node)
        {
            node.Nodes.Clear();
            foreach (var CatalogItem in ItemList.Where(x => x.ParentId == long.Parse(node.Name.Replace("@", "").Replace("#", ""))))
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Text = CatalogItem.ToString();
                if (CatalogItemType.CATEGORY == CatalogItem.Type)
                {
                    ChildNode.Name = CatalogItem.Id.ToString();
                    ChildNode.ImageIndex = 0;
                    ChildNode.ContextMenuStrip = contextSubCategeory;
                }
                else if (CatalogItemType.PRODUCTFAMILY == CatalogItem.Type)
                {
                    ChildNode.Name = CatalogItem.Id.ToString() + "#";
                    ChildNode.ImageIndex = 1;
                    ChildNode.ContextMenuStrip = contextNewProductFamily;
                }
                else if (CatalogItemType.PRODUCT == CatalogItem.Type)
                {
                    ChildNode.Name = CatalogItem.Id.ToString() + "@";
                    ChildNode.ImageIndex = 2;
                    ChildNode.ContextMenuStrip = contextNewProductFamily;
                }
                node.Nodes.Add(ChildNode);
                if (CatalogItem.Type != CatalogItemType.PRODUCT)
                {
                    LoadChildNodes(ChildNode, CatalogItem.Id);
                }
            }
        }
        private void TreeViewCatalog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TreeNode node = e.Node;
            node.SelectedImageIndex = node.ImageIndex;
            if (e.Node.Name.Contains("@"))
            {
                ResetProductTab();
                LoadProductCombo();
            }
            else if (e.Node.Name.Contains("#"))
            {
                ResetProductFamilyTab();
                LoadProductFamilyCombo();
            }
            else if (!e.Node.Name.Contains("@") && !e.Node.Name.Contains("#"))
            {
                ResetCategoryTab();
                LoadCategoryCombo();
            }
            LoadCatalogInfo();
            EnableForm(false);
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void LoadCategoryCombo()
        {
            ComboUtils.InitializeAccountCombo(ComboBoxCategoryDiscountAC, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCategoryInventoryAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCategoryPurchaseAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxCategorySalesAc, Global.Company.CompanyId);

        }
        private void LoadProductFamilyCombo()
        {
            ComboUtils.InitializeAccountCombo(ComboBoxProductFamilyDiscountAC, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductFamilyInventoryAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductFamilyPurchaseAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductFamilySalesAc, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueProductManufactureCombo(ComboBoxProductFamilyManufacturer, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueProductSupplierCombo(ComboBoxProductFamilySupplier, Global.Company.CompanyId);
        }
        private void LoadProductCombo()
        {
            ComboUtils.InitializeAccountCombo(ComboBoxProductDiscountAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductInventoryAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductPurchaseAc, Global.Company.CompanyId);
            ComboUtils.InitializeAccountCombo(ComboBoxProductSalesAc, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniquePurchaseUOMCombo(ComboBoxProductPurchesUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueRetailUOMCombo(ComboBoxProductRetailUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueWholeSaleUOMCombo(ComboBoxProductWholeSaleUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueProductManufactureCombo(ComboBoxProductManufacturer, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueProductSupplierCombo(ComboBoxProductSupplier, Global.Company.CompanyId);
            ComboUtils.InitializeScheduleCombo(ComboBoxProductSchedule);
            ComboUtils.InitializeAllUniqueRackNumberCombo(ComboBoxProductRackNumber, Global.Company.CompanyId);

        }

        private ProductFamily GetProductFamilyInfo()
        {
            if (TreeViewCatalog.SelectedNode != null && TreeViewCatalog.SelectedNode.Name.Contains('#'))
            {
                ProductFamily ProductFamilyFromDB = CatalogProductFamilyManager.GetProductFamilyInfoById(long.Parse(TreeViewCatalog.SelectedNode.Name.Trim('#')));
                if (ProductFamilyFromDB != null)
                {
                    return ProductFamilyFromDB;
                }
            }
            return null;
        }
        private Product GetProductInfo()
        {
            if (TreeViewCatalog.SelectedNode != null && TreeViewCatalog.SelectedNode.Name.Contains('@'))
            {
                Product ProductFromDB = CatalogProductManager.GetProductInfoById(long.Parse(TreeViewCatalog.SelectedNode.Name.Trim('@').ToString()));
                if (ProductFromDB != null)
                {
                    return ProductFromDB;
                }
            }
            return null;
        }
        private Category GetCategoryInfo()
        {
            Category CategoryFromDB = CategoryManager.GetCategoryInfoById(long.Parse(TreeViewCatalog.SelectedNode.Name));
            if (CategoryFromDB != null)
            {
                return CategoryFromDB;
            }
            return null;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            LoadCatalogWithFilter();
            return;
        }
        private void LoadCatalogInfo()
        {
            if (TreeViewCatalog.SelectedNode != null)
            {

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (TreeViewCatalog.SelectedNode.Name.EndsWith("#"))
                    {
                        ProductFamily ProductFamilyFromDB = GetProductFamilyInfo();
                        if (ProductFamilyFromDB != null)
                        {
                            TextBoxCatalogId.Text = ProductFamilyFromDB.Id.ToString();
                            TextBoxProductFamilyName.Text = ProductFamilyFromDB.Name;
                            TextBoxProductFamilyDisplayName.Text = ProductFamilyFromDB.DisplayAs;
                            TextBoxProductFamilyDescription.Text = ProductFamilyFromDB.Description;
                            ComboBoxProductFamilyManufacturer.Text = ProductFamilyFromDB.Manufacturer;
                            if (ProductFamilyFromDB.SupplierId == null)
                            {
                                TextBoxProductfamilysupplier.Text = ProductFamilyFromDB.SupplierName;
                                ComboBoxProductFamilySupplier.Text = ProductFamilyFromDB.SupplierName;
                            }
                            else
                            {
                                ComboBoxProductFamilySupplier.Text = ProductFamilyFromDB.Supplier.Name;
                                TextBoxProductfamilysupplier.Text = ProductFamilyFromDB.Supplier.Name;
                            }
                            if (ProductFamilyFromDB.Parent != null)
                            {
                                TextBoxProductFamilyParent.Id = ProductFamilyFromDB.Parent.Id.ToString();
                                TextBoxProductFamilyParent.Text = ProductFamilyFromDB.Parent.Name;
                            }
                            LoadProductFamilyAccount(ProductFamilyFromDB);
                            LoadSalesTax(GridViewProductFamilyTaxDetails, ProductFamilyFromDB.SalesTax.ToList());
                            if (ProductFamilyFromDB.Products.Count > 0)
                            {
                                GridViewProductFamilyChildProduct.Rows.Clear();
                                GridViewProductFamilyChildProduct.Rows.Add(ProductFamilyFromDB.Products.Count);
                                int i = 0;
                                foreach (Product ChildProducts in ProductFamilyFromDB.Products)
                                {
                                    GridViewProductFamilyChildProduct.Rows[i].Cells[0].Value = ChildProducts.MaterialId;
                                    GridViewProductFamilyChildProduct.Rows[i].Cells[1].Value = ChildProducts.Name;
                                    GridViewProductFamilyChildProduct.Rows[i].Cells[2].Value = ChildProducts.UOM;
                                    i++;
                                }
                            }
                        }
                        else if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                        {
                            DisplaySystemError("Somthing went wrong, the selected item is not valid.");
                            return;
                        }

                        if (!TabControlProductFamily.Visible)
                        {
                            TabControlCategory.Visible = false;
                            TabControlProduct.Visible = false;
                            TabControlProductFamily.Visible = true;
                        }
                    }
                    else if (TreeViewCatalog.SelectedNode.Name.Contains('@'))
                    {
                        Product ProductFromDB = GetProductInfo();

                        if (ProductFromDB != null)
                        {
                            TextBoxCatalogId.Text = ProductFromDB.Id.ToString();
                            TextBoxProductName.Text = ProductFromDB.Name;
                            TextBoxProductDisplayName.Text = ProductFromDB.DisplayAs;
                            TextBoxProductDescription.Text = ProductFromDB.Description;
                            TextBoxProductXFactorRetail.Text = ProductFromDB.RetailXFactor.ToString();
                            TextBoxProductXFactorWholeSale.Text = ProductFromDB.WholesaleXFactor.ToString();
                            TextBoxProductCost.Text = ProductFromDB.CostPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            TextBoxProductPurchasePrice.Text = ProductFromDB.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            TextBoxProductRetailPrice.Text = ProductFromDB.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            TextBoxProductWholeSalePrice.Text = ProductFromDB.WholdSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            TextBoxProductMSRP.Text = ProductFromDB.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            ComboBoxProductPurchesUOM.Text = ProductFromDB.UOM;
                            ComboBoxProductRetailUOM.Text = ProductFromDB.RetailUOM;
                            ComboBoxProductWholeSaleUOM.Text = ProductFromDB.WholesaleUOM;
                            ComboBoxProductSchedule.Text = ProductFromDB.Schedule;
                            ComboBoxProductRackNumber.Text = ProductFromDB.RackNumber;

                            ComboBoxProductManufacturer.Text = ProductFromDB.Manufacturer;
                            if (ProductFromDB.SupplierId == null)
                            {
                                TextBoxProductSupplier.Text = ProductFromDB.SupplierName;
                                ComboBoxProductSupplier.Text = ProductFromDB.SupplierName;
                            }
                            else
                            {
                                TextBoxProductSupplier.Text = ProductFromDB.Supplier.Name;
                                ComboBoxProductSupplier.Text = ProductFromDB.Supplier.Name;
                            }
                            barCodeProduct.NamePrice = ProductFromDB.Name + " / Rs. " + ProductFromDB.RetailPrice;
                            barCodeProduct.Text = ProductFromDB.MaterialId;
                            TextBoxProductCode.Text = ProductFromDB.MaterialId;
                            TextBoxProductHSN.Text = ProductFromDB.HSNCode;
                            if (ProductFromDB.Parent != null)
                            {
                                TextBoxProductParent.Id = ProductFromDB.Parent.Id.ToString();
                                TextBoxProductParent.Text = ProductFromDB.Parent.Name;
                            }
                            LoadProductAccount(ProductFromDB);
                            if (CheckBoxUseHsnCode.Checked)
                            {
                                LoadTaxCodeTax(GridViewProductTaxDetails);
                            }
                            else
                            {
                                LoadSalesTax(GridViewProductTaxDetails, ProductFromDB.SalesTax.ToList());
                            }
                            OpeningStock();
                            CheckBoxUseHsnCode.Checked = ProductFromDB.UseHsnTax;
                        }
                        else if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                        {
                            DisplaySystemError("Somthing went wrong, the selected item is not valid.");
                            return;
                        }

                        if (!TabControlProduct.Visible)
                        {
                            TabControlCategory.Visible = false;
                            TabControlProduct.Visible = true;
                            TabControlProductFamily.Visible = false;
                        }
                    }
                    else
                    {
                        Category CategoryFromDB = GetCategoryInfo();
                        if (CategoryFromDB != null)
                        {
                            TextBoxCatalogId.Text = CategoryFromDB.Id.ToString();
                            TextBoxCategoryName.Text = CategoryFromDB.Name;
                            TextBoxCategoryDisplayName.Text = CategoryFromDB.DisplayAs;
                            TextBoxCategoryDescription.Text = CategoryFromDB.Description;
                            if (CategoryFromDB.Parent != null)
                            {
                                TextBoxCategoryParent.Id = CategoryFromDB.Parent.Id.ToString();
                                TextBoxCategoryParent.Text = CategoryFromDB.Parent.Name;
                            }
                            LoadCategeoryAccount(CategoryFromDB);
                            LoadSalesTax(GridViewCategoryTaxDetails, CategoryFromDB.SalesTax.ToList());
                        }
                        else if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                        {
                            DisplaySystemError("Somthing went wrong, the selected item is not valid.");
                            return;
                        }

                        if (!TabControlCategory.Visible)
                        {
                            TabControlCategory.Visible = true;
                            TabControlProduct.Visible = false;
                            TabControlProductFamily.Visible = false;
                        }
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

        }
        private void OpeningStock()
        {
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                IList<Inventory> lInventory = InventoryLocationManager.Instance.ListInventoryOpeningStockByProductId(long.Parse(TextBoxCatalogId.Text));
                if (lInventory.Count > 0)
                {
                    TextBoxOpeningstock.Text = Math.Round(lInventory.Sum(X => X.OpeningStock), Global.Company.QuantityPricision).ToString();
                }
                else
                {
                    TextBoxOpeningstock.Text = Math.Round(0.00, Global.Company.QuantityPricision).ToString();

                }
            }
        }
        private void LoadCategeoryAccount(CatalogItem Category)
        {
            CheckBoxCategoryMaintainInventory.Checked = Category.ShouldMaintainInventory == true ? true : false;
            CheckBoxCategoryAtBatchLevel.Checked = Category.isInventoryAtBatch == true ? true : false;
            TextBoxCategoryDefaultDiscount.Text = ((float)Category.DefaultDiscount).ToString("0.00");
            if (Category.SalesAccount != null)
            {
                ComboBoxCategorySalesAc.SelectedIndex = ComboBoxCategorySalesAc.FindStringExact(Category.SalesAccount.Name);
            }
            if (Category.PurchaseAccount != null)
            {
                ComboBoxCategoryPurchaseAc.SelectedIndex = ComboBoxCategoryPurchaseAc.FindStringExact(Category.PurchaseAccount.Name);
            }
            if (Category.DiscountAccount != null)
            {
                ComboBoxCategoryDiscountAC.SelectedIndex = ComboBoxCategoryDiscountAC.FindStringExact(Category.DiscountAccount.Name);
            }
            if (Category.InventoryAccount != null)
            {
                ComboBoxCategoryInventoryAc.SelectedIndex = ComboBoxCategoryInventoryAc.FindStringExact(Category.InventoryAccount.Name);
            }
        }
        private void LoadProductFamilyAccount(CatalogItem Category)
        {
            CheckBoxProductFamilyMaintainInventory.Checked = Category.ShouldMaintainInventory == true ? true : false;
            CheckBoxProductFamilyAtBatchLevel.Checked = Category.isInventoryAtBatch == true ? true : false;
            TextBoxProductFamilyDefaultDiscount.Text = ((float)Category.DefaultDiscount).ToString("0.00");
            if (Category.SalesAccount != null)
            {
                ComboBoxProductFamilySalesAc.SelectedIndex = ComboBoxProductFamilySalesAc.FindStringExact(Category.SalesAccount.Name);
            }
            if (Category.PurchaseAccount != null)
            {
                ComboBoxProductFamilyPurchaseAc.SelectedIndex = ComboBoxProductFamilyPurchaseAc.FindStringExact(Category.PurchaseAccount.Name);
            }
            if (Category.DiscountAccount != null)
            {
                ComboBoxProductFamilyDiscountAC.SelectedIndex = ComboBoxProductFamilyDiscountAC.FindStringExact(Category.DiscountAccount.Name);
            }
            if (Category.InventoryAccount != null)
            {
                ComboBoxProductFamilyInventoryAc.SelectedIndex = ComboBoxProductFamilyInventoryAc.FindStringExact(Category.InventoryAccount.Name);
            }
        }
        private void LoadProductAccount(CatalogItem ProductFamily)
        {
            CheckBoxProductMaintainInventory.Checked = ProductFamily.ShouldMaintainInventory == true ? true : false;
            CheckBoxProductAtBatchLevel.Checked = ProductFamily.isInventoryAtBatch == true ? true : false;
            TextBoxProductDefaultDiscount.Text = ((float)ProductFamily.DefaultDiscount).ToString("0.00");
            if (ProductFamily.SalesAccount != null)
            {
                ComboBoxProductSalesAc.SelectedIndex = ComboBoxProductSalesAc.FindStringExact(ProductFamily.SalesAccount.Name);
            }
            if (ProductFamily.PurchaseAccount != null)
            {
                ComboBoxProductPurchaseAc.SelectedIndex = ComboBoxProductPurchaseAc.FindStringExact(ProductFamily.PurchaseAccount.Name);
            }
            if (ProductFamily.DiscountAccount != null)
            {
                ComboBoxProductDiscountAc.SelectedIndex = ComboBoxProductDiscountAc.FindStringExact(ProductFamily.DiscountAccount.Name);
            }
            if (ProductFamily.InventoryAccount != null)
            {
                ComboBoxProductInventoryAc.SelectedIndex = ComboBoxProductInventoryAc.FindStringExact(ProductFamily.InventoryAccount.Name);
            }

        }
        private List<CatalogItemSalesTaxMap> SalesTaxMapFromCompany()
        {
            List<CatalogItemSalesTaxMap> SalesTaxMaps = new List<CatalogItemSalesTaxMap>();
            foreach (CompanySalesTaxAccountMap SalesTaxMapFromCompany in Global.Company.SalesTaxAccountMaps)
            {
                CatalogItemSalesTaxMap SalesTaxMapNew = new CatalogItemSalesTaxMap();
                SalesTaxMapNew.SalesTaxMapId = SalesTaxMapFromCompany.MapId;
                SalesTaxMapNew.TaxPercentage = 0.0F;
                SalesTaxMaps.Add(SalesTaxMapNew);
            }
            return SalesTaxMaps;
        }
        private void LoadSalesTax(DataGridView DataGridView, List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMaps)
        {
            DataGridView.Rows.Clear();
            int i = 0;
            foreach (CatalogItemSalesTaxMap CatalogItemSalesTaxMap in CatalogItemSalesTaxMaps)
            {
                if (CatalogItemSalesTaxMap != null)
                {
                    CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)CatalogItemSalesTaxMap.SalesTaxMapId);
                    if (CMap != null && CMap.CountrySaleTax.EffectiveFrom <= Global.getTransactionDate() && CMap.CountrySaleTax.EffectiveTo >= Global.getTransactionDate())
                    {
                        DataGridView.Rows.Add();
                        DataGridView.Rows[i].Cells[0].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == CatalogItemSalesTaxMap.SalesTaxMapId).Name;

                        if (CatalogItemSalesTaxMap.EffectiveFrom <= Global.getTransactionDate() && CatalogItemSalesTaxMap.EffectiveTo >= Global.getTransactionDate())
                        {
                            DataGridView.Rows[i].Cells[1].Value = CatalogItemSalesTaxMap.TaxPercentage.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                        }
                        else
                        {
                            DataGridView.Rows[i].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                        }
                        DataGridView.Rows[i].Cells[2].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == CatalogItemSalesTaxMap.SalesTaxMapId).MapId;
                        i++;
                    }
                }
            }
        }
        private Category GetCategoryFromForm()
        {
            Category lCategory = new Category();
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                lCategory.Id = Convert.ToInt64(TextBoxCatalogId.Text);
            }
            else
            {
                lCategory.Id = 0L;
            }
            lCategory.CompanyId = Global.Company.CompanyId;
            lCategory.Name = TextBoxCategoryName.Text.Trim();
            lCategory.Description = TextBoxCategoryDescription.Text.Trim();
            lCategory.DisplayAs = TextBoxCategoryDisplayName.Text.Trim();
            if (CheckBoxCategoryAtBatchLevel.Checked)
            {
                lCategory._isInventoryAtBatch = true;
            }
            if (CheckBoxCategoryMaintainInventory.Checked)
            {
                lCategory._shouldMaintainInventory = true;
            }
            if (!string.IsNullOrEmpty(TextBoxCategoryParent.Text))
            {
                lCategory.ParentId = long.Parse(TextBoxCategoryParent.Id);
            }
            lCategory.DefaultDiscount = string.IsNullOrEmpty(TextBoxCategoryDefaultDiscount.Text.Trim()) ? 0 : float.Parse(TextBoxCategoryDefaultDiscount.Text);

            if (ComboBoxCategorySalesAc.SelectedIndex >= 0)
            {
                lCategory.SalesAccount = ((Account)ComboBoxCategorySalesAc.Items[ComboBoxCategorySalesAc.SelectedIndex]);
            }
            if (ComboBoxCategoryPurchaseAc.SelectedIndex >= 0)
            {
                lCategory.PurchaseAccount = ((Account)ComboBoxCategoryPurchaseAc.Items[ComboBoxCategoryPurchaseAc.SelectedIndex]);
            }
            if (ComboBoxCategoryDiscountAC.SelectedIndex >= 0)
            {
                lCategory.DiscountAccount = ((Account)ComboBoxCategoryDiscountAC.Items[ComboBoxCategoryDiscountAC.SelectedIndex]);
            }
            if (ComboBoxCategoryInventoryAc.SelectedIndex >= 0)
            {
                lCategory.InventoryAccount = ((Account)ComboBoxCategoryInventoryAc.Items[ComboBoxCategoryInventoryAc.SelectedIndex]);
            }
            // lCategory.SalesTax = AddSalesTax(GridViewCategoryTaxDetails);
            return lCategory;
        }
        private ProductFamily GetProductFamilyFromForm()
        {
            ProductFamily lProductFamily = new ProductFamily();
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                lProductFamily.Id = Convert.ToInt64(TextBoxCatalogId.Text);
            }
            else
            {
                lProductFamily.Id = 0L;
            }
            lProductFamily.CompanyId = Global.Company.CompanyId;
            lProductFamily.Name = TextBoxProductFamilyName.Text.Trim();
            lProductFamily.Description = TextBoxProductFamilyDescription.Text.Trim();
            lProductFamily.DisplayAs = TextBoxProductFamilyDisplayName.Text.Trim();
            if (CheckBoxProductFamilyAtBatchLevel.Checked)
            {
                lProductFamily._isInventoryAtBatch = true;
            }
            if (CheckBoxProductFamilyMaintainInventory.Checked)
            {
                lProductFamily._shouldMaintainInventory = true;
            }
            lProductFamily.Manufacturer = ComboBoxProductFamilyManufacturer.Text;

            if (ComboBoxProductFamilySupplier.SelectedIndex > -1)
            {
                lProductFamily.SupplierId = ((Supplier)ComboBoxProductFamilySupplier.Items[ComboBoxProductFamilySupplier.SelectedIndex]).Id;
                lProductFamily.SupplierName = ComboBoxProductFamilySupplier.Text;
            }
            else
            {
                lProductFamily.SupplierName = ComboBoxProductFamilySupplier.Text;
            }
            lProductFamily.ParentId = long.Parse(TextBoxProductFamilyParent.Id);
            lProductFamily.DefaultDiscount = string.IsNullOrEmpty(TextBoxProductFamilyDefaultDiscount.Text.Trim()) ? 0 : float.Parse(TextBoxProductFamilyDefaultDiscount.Text);

            if (ComboBoxProductFamilySalesAc.SelectedIndex >= 0)
            {
                lProductFamily.SalesAccount = ((Account)ComboBoxProductFamilySalesAc.Items[ComboBoxProductFamilySalesAc.SelectedIndex]);
            }
            if (ComboBoxProductFamilyPurchaseAc.SelectedIndex >= 0)
            {
                lProductFamily.PurchaseAccount = ((Account)ComboBoxProductFamilyPurchaseAc.Items[ComboBoxProductFamilyPurchaseAc.SelectedIndex]);
            }
            if (ComboBoxProductFamilyDiscountAC.SelectedIndex >= 0)
            {
                lProductFamily.DiscountAccount = ((Account)ComboBoxProductFamilyDiscountAC.Items[ComboBoxProductFamilyDiscountAC.SelectedIndex]);
            }
            if (ComboBoxProductFamilyInventoryAc.SelectedIndex >= 0)
            {
                lProductFamily.InventoryAccount = ((Account)ComboBoxProductFamilyInventoryAc.Items[ComboBoxProductFamilyInventoryAc.SelectedIndex]);
            }
            //lProductFamily.SalesTax = AddSalesTax(GridViewProductFamilyTaxDetails);
            return lProductFamily;
        }
        private Product GetProductFromForm()
        {
            Product lProduct = new Product();
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                lProduct.Id = Convert.ToInt64(TextBoxCatalogId.Text);
            }
            else
            {
                lProduct.Id = 0L;
            }
            lProduct.CompanyId = Global.Company.CompanyId;
            lProduct.Name = TextBoxProductName.Text.Trim();
            lProduct.Description = TextBoxProductDescription.Text.Trim();
            lProduct.UOM = ComboBoxProductPurchesUOM.Text.Trim();
            lProduct.RetailUOM = ComboBoxProductRetailUOM.Text.Trim();
            lProduct.Schedule = ComboBoxProductSchedule.Text.Trim();
            lProduct.RackNumber = ComboBoxProductRackNumber.Text.Trim();
            lProduct.RetailXFactor = int.Parse(TextBoxProductXFactorRetail.Text.Trim());
            lProduct.WholesaleUOM = ComboBoxProductWholeSaleUOM.Text.Trim();
            lProduct.WholesaleXFactor = int.Parse(TextBoxProductXFactorWholeSale.Text.Trim());
            lProduct.PurchasePrice = string.IsNullOrEmpty(TextBoxProductPurchasePrice.Text.Trim()) ? 0 : float.Parse(TextBoxProductPurchasePrice.Text.Trim());
            lProduct.CostPrice = string.IsNullOrEmpty(TextBoxProductCost.Text.Trim()) ? 0 : float.Parse(TextBoxProductCost.Text.Trim());
            lProduct.RetailPrice = string.IsNullOrEmpty(TextBoxProductRetailPrice.Text.Trim()) ? 0 : float.Parse(TextBoxProductRetailPrice.Text.Trim());
            lProduct.WholdSalePrice = string.IsNullOrEmpty(TextBoxProductWholeSalePrice.Text.Trim()) ? 0 : float.Parse(TextBoxProductWholeSalePrice.Text.Trim());
            lProduct.Msrp = string.IsNullOrEmpty(TextBoxProductMSRP.Text.Trim()) ? 0 : float.Parse(TextBoxProductMSRP.Text.ToString());
            lProduct.Manufacturer = ComboBoxProductManufacturer.Text.Trim();
            lProduct.UseHsnTax = CheckBoxUseHsnCode.Checked;
            if (ComboBoxProductSupplier.SelectedIndex > -1)
            {
                lProduct.SupplierId = ((Supplier)ComboBoxProductSupplier.Items[ComboBoxProductSupplier.SelectedIndex]).Id;
                lProduct.SupplierName = ComboBoxProductSupplier.Text;
            }
            else
            {
                lProduct.SupplierName = ComboBoxProductSupplier.Text;
            }

            lProduct.MaterialId = TextBoxProductCode.Text;
            lProduct.HSNCode = TextBoxProductHSN.Text;
            if (CheckBoxProductAtBatchLevel.Checked)
            {
                lProduct._isInventoryAtBatch = true;
            }
            else if (!string.IsNullOrEmpty(TextBoxProductParent.Id))
            {
                CatalogItem CatalogItem = CatalogItemManager.Instance.GetCatalogItemInfoById(long.Parse(TextBoxProductParent.Id));
                if (CatalogItem != null)
                {
                    if (CatalogItem.isInventoryAtBatch != null)
                    {
                        lProduct._isInventoryAtBatch = false;
                    }
                }
            }


            double PresentQty = double.Parse(TextBoxOpeningstock.Text);
            lProduct.QuantityOnHand = PresentQty;
            if (CheckBoxProductMaintainInventory.Checked)
            {
                lProduct._shouldMaintainInventory = true;
            }
            else if (!string.IsNullOrEmpty(TextBoxProductParent.Id))
            {
                CatalogItem CatalogItem = CatalogItemManager.Instance.GetCatalogItemInfoById(long.Parse(TextBoxProductParent.Id));
                if (CatalogItem != null)
                {
                    if (CatalogItem.ShouldMaintainInventory != null)
                    {
                        lProduct._shouldMaintainInventory = false;
                    }
                }
            }


            lProduct.ParentId = long.Parse(TextBoxProductParent.Id);
            lProduct.ProductFamilyId = long.Parse(TextBoxProductParent.Id);
            lProduct.DefaultDiscount = string.IsNullOrEmpty(TextBoxProductDefaultDiscount.Text.Trim()) ? 0 : float.Parse(TextBoxProductDefaultDiscount.Text);
            if (ComboBoxProductSalesAc.SelectedIndex > 1)
            {
                lProduct.SalesAccount = ((Account)ComboBoxProductSalesAc.Items[ComboBoxProductSalesAc.SelectedIndex]);
            }
            if (ComboBoxProductPurchaseAc.SelectedIndex > 1)
            {
                lProduct.PurchaseAccount = ((Account)ComboBoxProductPurchaseAc.Items[ComboBoxProductPurchaseAc.SelectedIndex]);
            }
            if (ComboBoxProductDiscountAc.SelectedIndex > 1)
            {
                lProduct.DiscountAccount = ((Account)ComboBoxProductDiscountAc.Items[ComboBoxProductDiscountAc.SelectedIndex]);
            }
            if (ComboBoxProductInventoryAc.SelectedIndex > 1)
            {
                lProduct.InventoryAccount = ((Account)ComboBoxProductInventoryAc.Items[ComboBoxProductInventoryAc.SelectedIndex]);
            }
            lProduct.SalesTax = AddSalesTax(GridViewProductTaxDetails);
            return lProduct;
        }
        private List<CatalogItemSalesTaxMap> AddSalesTax(DataGridView DataGridView)
        {
            List<CatalogItemSalesTaxMap> SalesTaxFromForm = new List<CatalogItemSalesTaxMap>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {

                CatalogItemSalesTaxMap CatalogItemSalesTaxMap = new CatalogItemSalesTaxMap();
                long TaxId = ((long)DataGridView.Rows[i].Cells[2].Value);
                CompanySalesTaxAccountMap CompanySalesTaxAccountMap = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == TaxId);
                if (CompanySalesTaxAccountMap != null)
                {
                    CatalogItemSalesTaxMap.SalesTaxMapId = CompanySalesTaxAccountMap.MapId;
                    CatalogItemSalesTaxMap.TaxPercentage = float.Parse(DataGridView.Rows[i].Cells[1].Value.ToString());
                    CatalogItemSalesTaxMap.EffectiveFrom = EffectiveStartDate;
                    CatalogItemSalesTaxMap.EffectiveTo = EffectiveEndDate;
                    SalesTaxFromForm.Add(CatalogItemSalesTaxMap);
                }
            }
            return SalesTaxFromForm;
        }

        //With conditions
        private void newCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TextBoxCatalogSearch.ResetText();
                Cursor.Current = Cursors.WaitCursor;
                if (!TabControlCategory.Visible)
                {
                    TabControlCategory.Visible = true;
                    TabControlProduct.Visible = false;
                    TabControlProductFamily.Visible = false;
                }
                ResetCategoryTab();
                LoadCategoryCombo();
                LoadSalesTax(GridViewCategoryTaxDetails, SalesTaxMapFromCompany());
                EnableForm(true);
                TextBoxCategoryName.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void newProductFamilyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                TextBoxCatalogSearch.ResetText();
                Cursor.Current = Cursors.WaitCursor;
                if (!CategoryManager.IsContainCategory(Global.Company.CompanyId))
                {
                    newCatalogToolStripMenuItem.PerformClick();
                    CatalogErrorMsg.Text = NoCategoryFoundErrorMsg;
                }
                else
                {
                    if (!TabControlProductFamily.Visible)
                    {
                        TabControlCategory.Visible = false;
                        TabControlProduct.Visible = false;
                        TabControlProductFamily.Visible = true;
                    }
                    ResetProductFamilyTab();
                    LoadProductFamilyCombo();
                    LoadSalesTax(GridViewProductFamilyTaxDetails, SalesTaxMapFromCompany());
                    EnableForm(true);
                    TextBoxProductFamilyName.Select();
                    this.formIsDirty = false;
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void newProductSKUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TextBoxCatalogSearch.ResetText();
                Cursor.Current = Cursors.WaitCursor;
                if (!CategoryManager.IsContainCategory(Global.Company.CompanyId))
                {
                    newCatalogToolStripMenuItem.PerformClick();
                    CatalogErrorMsg.Text = NoCategoryFoundErrorMsg;
                }
                else if (CatalogProductFamilyManager.IsContainProductfamily(Global.Company.CompanyId))
                {
                    if (!TabControlProduct.Visible)
                    {
                        TabControlCategory.Visible = false;
                        TabControlProduct.Visible = true;
                        TabControlProductFamily.Visible = false;
                    }
                    ResetProductTab();
                    LoadProductCombo();
                    LoadSalesTax(GridViewProductTaxDetails, SalesTaxMapFromCompany());
                    EnableForm(true);
                    CheckBoxUseHsnCode.Enabled = false;
                    TextBoxProductName.Select();
                    this.formIsDirty = false;
                }
                else
                {
                    newProductFamilyToolStripMenuItem1.PerformClick();
                    CatalogErrorMsg.Text = NoProductFamilyFoundErrorMsg;
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        //No conditions
        private TreeNode ParentNode;
        private void addSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!TabControlCategory.Visible)
                {
                    TabControlCategory.Visible = true;
                    TabControlProduct.Visible = false;
                    TabControlProductFamily.Visible = false;
                }
                ResetCategoryTab();
                LoadCategoryCombo();
                if (TreeViewCatalog.SelectedNode != null)
                {
                    TextBoxCategoryParent.Id = ParentNode.Name;
                    TextBoxCategoryParent.Text = ParentNode.Text;
                }
                Category Category = CategoryManager.GetCategoryInfoById(long.Parse(ParentNode.Name));
                if (Category != null)
                {
                    LoadSalesTax(GridViewCategoryTaxDetails, Category.SalesTax.ToList());
                }
                EnableForm(true);
                TextBoxCategoryName.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void addNewProductFamilyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!TabControlProductFamily.Visible)
                {
                    TabControlCategory.Visible = false;
                    TabControlProduct.Visible = false;
                    TabControlProductFamily.Visible = true;
                }
                ResetProductFamilyTab();
                LoadProductFamilyCombo();
                if (TreeViewCatalog.SelectedNode != null)
                {
                    TextBoxProductFamilyParent.Id = ParentNode.Name;
                    TextBoxProductFamilyParent.Text = ParentNode.Text;
                }
                Category Category = CategoryManager.GetCategoryInfoById(long.Parse(ParentNode.Name));
                if (Category != null)
                {
                    LoadSalesTax(GridViewProductFamilyTaxDetails, Category.SalesTax.ToList());
                }
                EnableForm(true);
                TextBoxProductFamilyName.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!TabControlProduct.Visible)
                {
                    TabControlCategory.Visible = false;
                    TabControlProduct.Visible = true;
                    TabControlProductFamily.Visible = false;
                }
                ResetProductTab();
                LoadProductCombo();
                string ProductFamilyId = ParentNode.Name.Replace("#", "");
                if (TreeViewCatalog.SelectedNode != null && ParentNode.Name.Contains('#'))
                {
                    TextBoxProductParent.Id = ProductFamilyId;
                    TextBoxProductParent.Text = ParentNode.Text;

                    ProductFamily ProductFamily = CatalogProductFamilyManager.GetProductFamilyInfoById(long.Parse(ProductFamilyId));
                    if (ProductFamily != null)
                    {
                        LoadSalesTax(GridViewProductTaxDetails, ProductFamily.SalesTax.ToList());
                    }
                }
                else if (TreeViewCatalog.SelectedNode != null && ParentNode.Name.Contains('@'))
                {
                    ProductFamily ProductFamily = CatalogProductFamilyManager.GetProductFamilyInfoById(long.Parse(ParentNode.Parent.Name.Replace("#", "")));
                    if (ProductFamily != null)
                    {
                        TextBoxProductParent.Id = ProductFamily.Id.ToString();
                        TextBoxProductParent.Text = ProductFamily.Name;
                        LoadSalesTax(GridViewProductTaxDetails, ProductFamily.SalesTax.ToList());
                    }
                }
                EnableForm(true);
                CheckBoxUseHsnCode.Enabled = false;
                TextBoxProductName.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewCatalog.SelectedNode = ParentNode;
            BtnCatalogDelete_Click(sender, e);
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewCatalog.SelectedNode = ParentNode;
            BtnCatalogEdit_Click(sender, e);
        }
        private void BtnCatalogNew_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            CatalogErrorMsg.Text = "";
            if (e.ClickedItem.Text == "Category")
            {
                newCatalogToolStripMenuItem_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Product Family")
            {
                newProductFamilyToolStripMenuItem1_Click(sender, e);
            }
            else if (e.ClickedItem.Text == "Product (SKU)")
            {
                newProductSKUToolStripMenuItem_Click(sender, e);
            }
        }

        private void BtnCatalogDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    MessageBox.Show("Somting went wrong, please check this entry is still valid.");
                    return;
                }
                CatalogItem CatalogItem = CatalogItemManager.Instance.GetCatalogItemInfoById(long.Parse(TextBoxCatalogId.Text));
                if (CatalogItem != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    CatalogErrorMsg.Text = "";
                    if (TabControlCategory.Visible)
                    {
                        DialogResult Result = MessageBox.Show(string.Format(DeleteCategoryConfirmText, TextBoxCategoryName.Text), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            if (CategoryManager.DeleteCategory(long.Parse(TextBoxCatalogId.Text)))
                            {
                                ResetCategoryTab();
                                LoadCategoryCombo();
                                LoadCatalogWithFilter();
                                TextBoxCatalogSearch.Clear();
                                EnableForm(false);
                                if (TreeViewCatalog.Nodes.Count > 0)
                                {
                                    CatalogErrorMsg.Text = string.Format(RemoveSuccess, "Categeory");
                                }
                                else
                                {
                                    CatalogErrorMsg.Text = NoCategoryFoundErrorMsg;
                                }
                                this.formIsDirty = false;
                            }
                            else
                            {
                                CatalogErrorMsg.Text = string.Format(DeleteErrorText, "Categeory");
                            }
                        }
                    }
                    else if (TabControlProductFamily.Visible)
                    {
                        DialogResult Result = MessageBox.Show(string.Format(DeleteProductFamilyConfirmText, TextBoxProductFamilyName.Text), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            if (CategoryManager.DeleteCategory(long.Parse(TextBoxCatalogId.Text)))
                            {
                                ResetCategoryTab();
                                LoadCategoryCombo();
                                LoadCatalogWithFilter();
                                TextBoxCatalogSearch.Clear();
                                EnableForm(false);
                                CatalogErrorMsg.Text = string.Format(RemoveSuccess, "Product Family");
                                this.formIsDirty = false;
                            }
                            else
                            {
                                CatalogErrorMsg.Text = DeleteErrorProduct;
                            }
                        }
                    }
                    else if (TabControlProduct.Visible)
                    {
                        DialogResult Result = MessageBox.Show(string.Format(DeleteProductConfirmText, TextBoxProductName.Text), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetailByProductId(long.Parse(TextBoxCatalogId.Text));
                            PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetailsByProductId(long.Parse(TextBoxCatalogId.Text));

                            if (SaleDetail == null && PurchaseDetails == null)
                            {
                                if (CategoryManager.DeleteCategory(long.Parse(TextBoxCatalogId.Text)))
                                {
                                    ResetCategoryTab();
                                    LoadCategoryCombo();
                                    LoadCatalogWithFilter();
                                    TextBoxCatalogSearch.Clear();
                                    EnableForm(false);
                                    CatalogErrorMsg.Text = string.Format(RemoveSuccess, "Product");
                                    this.formIsDirty = false;
                                }
                                else
                                {
                                    CatalogErrorMsg.Text = DeleteErrorProduct;
                                }
                            }
                            else
                            {
                                CatalogErrorMsg.Text = DeleteErrorProduct;
                            }
                        }
                    }
                }
                else
                {
                    DisplaySystemError("Somthing went wrong, the selected entry is not valid.");
                    return;
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void TabFocused()
        {
            if (TabControlCategory.Visible)
            {
                TextBoxCategoryName.Select();
            }
            else if (TabControlProductFamily.Visible)
            {
                TextBoxProductFamilyName.Select();
            }
            else if (TabControlProduct.Visible)
            {
                TextBoxProductName.Select();
            }
        }

        private void BtnCatalogEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    DisplaySystemError("Somting went wrong, please check this product is still valid.");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                CatalogErrorMsg.Text = "";
                EnableForm(true);
                if (TabControlProduct.Visible)
                {
                    IList<Inventory> lInventory = InventoryLocationManager.Instance.ListInventoryOpeningStockByProductId(long.Parse(TextBoxCatalogId.Text));
                    StockMovementDetail StockMovementDetail = StockMovementManager.Instance.GetStockMovementDetailByProductId(long.Parse(TextBoxCatalogId.Text));
                    if (StockMovementDetail != null)
                    {
                        CheckBoxProductMaintainInventory.Enabled = false;
                        CheckBoxProductAtBatchLevel.Enabled = false;
                    }
                    if (StockMovementDetail != null || (lInventory != null && lInventory.Count > 0))
                    {
                        TextBoxProductXFactorRetail.ReadOnly = true;
                        TextBoxProductXFactorRetail.TabStop = false;
                        TextBoxProductXFactorWholeSale.ReadOnly = true;
                        TextBoxProductXFactorWholeSale.TabStop = false;
                        ComboBoxProductPurchesUOM.Visible = false;
                        ComboBoxProductRetailUOM.Visible = false;
                        ComboBoxProductWholeSaleUOM.Visible = false;
                    }
                    if (!string.IsNullOrEmpty(TextBoxProductHSN.Text.Trim()))
                    {
                        ItemTax ItemTax = ItemTaxManager.Instance.GetItemTaxCodeByCode(TextBoxProductHSN.Text.Trim(), Global.Company.CompanyId);
                        if (ItemTax != null)
                        {
                            CheckBoxUseHsnCode.Enabled = true;
                        }
                        else
                        {
                            CheckBoxUseHsnCode.Checked = false;
                            CheckBoxUseHsnCode.Enabled = false;
                        }
                    }
                }
                if (TabControlCategory.Visible)
                {
                    if (TreeViewCatalog.Nodes.Count == 1 && TreeViewCatalog.SelectedNode.Parent == null)
                    {
                        BtnCategoryParent.Enabled = false;
                    }
                }
                TabFocused();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnCatalogCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        TabFocused();
                        return;
                    }
                }
                if (TabControlCategory.Visible)
                {
                    ResetCategoryTab();
                    LoadCategoryCombo();
                }
                else if (TabControlProductFamily.Visible)
                {
                    ResetProductFamilyTab();
                    LoadProductFamilyCombo();
                }
                else if (TabControlProduct.Visible)
                {
                    ResetProductTab();
                    LoadProductCombo();
                }
                if (TreeViewCatalog.Nodes.Count > 0)
                {
                    CatalogErrorMsg.Text = "";
                }
                else
                {
                    CatalogErrorMsg.Text = NoCategoryFoundErrorMsg;
                }
                LoadCatalogInfo();
                EnableForm(false);
                CheckBoxUseHsnCode.Enabled = false;
                TextBoxCatalogSearch.Select();
                this.formIsDirty = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        bool IsExpand = true;
        List<long> ParentIds = new List<long>();
        private void PointSaveorUpdatedNode(TreeNodeCollection nodes, string findtext)
        {
            ParentIds = new List<long>();
            ParentIds.Add(long.Parse(findtext.Replace("@", "").Replace("#", "")));
            GetCatalogItemParentById(long.Parse(findtext.Replace("@", "").Replace("#", "")));
            ParentIds.Reverse();
            if (ParentIds.Count > 0)
            {
                foreach (long PId in ParentIds)
                {
                    foreach (TreeNode node in nodes)
                    {
                        if (node.Name.Replace("@", "").Replace("#", "") == PId.ToString())
                        {
                            if (node.Name == findtext)
                            {
                                IsExpand = false;
                                node.TreeView.SelectedNode = node;
                                IsExpand = true;
                                TreeViewCatalog.SelectedNode = node;
                                node.TreeView.Focus();
                                break;
                            }
                            else
                            {
                                Expand(node);
                                nodes = node.Nodes;
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void GetCatalogItemParentById(long Id)
        {
            CatalogItem CatalogItemInfo = ItemList.Where(p => p.Id == Id).FirstOrDefault<CatalogItem>();
            if (CatalogItemInfo.ParentId != null)
            {
                ParentIds.Add((long)CatalogItemInfo.ParentId);
                GetCatalogItemParentById((long)CatalogItemInfo.ParentId);
            }
            else
            {
                return;
            }
            return;
        }
        private async void BtnCatalogSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (TabControlCategory.Visible && validateFormCategory())
                {
                    IsPerformSearch = false;
                    TextBoxCatalogSearch.ResetText();
                    Category lCategory = GetCategoryFromForm();
                    if (CategoryManager.CategoryNameUniqueById(lCategory))
                    {
                        Category lCategoryFromDB = null!;
                        if (lCategory.Id == 0)
                        {
                            lCategoryFromDB = CategoryManager.AddCategory(lCategory);

                        }
                        else
                        {
                            Category lCategoryById = CategoryManager.GetCategoryInfoById(lCategory.Id);
                            if (lCategoryById != null)
                            {
                                lCategoryFromDB = CategoryManager.UpdateCategory(lCategory);

                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this category is still valid.");
                                return;
                            }
                        }
                        if (CreateCatalogOnLoad)
                        {
                            FormCatalog_Load(sender, e);
                            return;
                        }
                        ResetCategoryTab();
                        LoadCategoryCombo();
                        LoadCatalogWithFilter();
                        if (lCategoryFromDB != null)
                            PointSaveorUpdatedNode(TreeViewCatalog.Nodes, lCategoryFromDB.Id.ToString());
                        EnableForm(false);
                        CatalogErrorMsg.Text = SaveCatalogSuccessText;
                        this.formIsDirty = false;
                    }
                    else
                    {
                        CatalogErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Catageory", lCategory.Name);
                        TextBoxCategoryName.Select();
                        return;
                    }
                }
                else if (TabControlProductFamily.Visible && validateFormProductFamily())
                {
                    IsPerformSearch = false;
                    TextBoxCatalogSearch.ResetText();
                    ProductFamily lProductFamily = GetProductFamilyFromForm();
                    if (CatalogProductFamilyManager.ProductFamilyNameUniqueById(lProductFamily))
                    {
                        ProductFamily lProductFamilyFromDB = null!;
                        if (lProductFamily.Id == 0)
                        {
                            lProductFamilyFromDB = CatalogProductFamilyManager.AddProductFamily(lProductFamily);
                        }
                        else
                        {
                            ProductFamily lProductFamilyById = CatalogProductFamilyManager.GetProductFamilyInfoById(lProductFamily.Id);
                            if (lProductFamilyById != null)
                            {
                                lProductFamilyFromDB = CatalogProductFamilyManager.UpdateProductFamily(lProductFamily);
                            }
                            else
                            {
                                DisplaySystemError("Somting went wrong, please check this productfamily is still valid.");
                                return;
                            }
                        }
                        if (CreateCatalogOnLoad)
                        {
                            FormCatalog_Load(sender, e);
                            return;
                        }
                        ResetProductFamilyTab();
                        LoadProductFamilyCombo();
                        LoadCatalogWithFilter();
                        PointSaveorUpdatedNode(TreeViewCatalog.Nodes, lProductFamilyFromDB.Id.ToString() + "#");
                        EnableForm(false);
                        CatalogErrorMsg.Text = SaveProductFamilySuccessText;
                        this.formIsDirty = false;
                    }
                    else
                    {
                        CatalogErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Product Family", lProductFamily.Name);
                        TextBoxProductFamilyName.Select();
                        return;
                    }
                }
                else if (TabControlProduct.Visible && validateFormProduct())
                {
                    IsPerformSearch = false;
                    TextBoxCatalogSearch.ResetText();
                    Product lProduct = GetProductFromForm();

                    if (CatalogProductManager.ProductNameUniqueById(lProduct))
                    {
                        if (CatalogProductManager.FindMaterialIdUnique(lProduct))
                        {
                            Product lProductFromDB = null!;
                            if (lProduct.Id == 0)
                            {
                                lProductFromDB = CatalogProductManager.AddProduct(lProduct);

                                // ✅ Assign newly generated ProductId to PendingPercentages before saving
                                if (this.PendingPercentages != null)
                                {
                                    this.PendingPercentages.ProductId = lProductFromDB.Id;
                                    this.PendingPercentages.ProductCode = lProductFromDB.MaterialId;
                                }

                                // Save percentages (now with valid ProductId)
                                await SaveOrCalculateProductPercentage(lProductFromDB);
                            }

                            else
                            {
                                Product lProductById = CatalogProductManager.GetProductInfoById(lProduct.Id);
                                if (lProductById != null)
                                {
                                    lProductFromDB = CatalogProductManager.UpdateProduct(lProduct);

                                    if (this.PendingPercentages != null)
                                    {
                                        this.PendingPercentages.ProductId = lProductFromDB.Id;
                                        this.PendingPercentages.ProductCode = lProductFromDB.MaterialId;
                                    }
                                    // Update percentages if they exist (async operation)
                                    await SaveOrCalculateProductPercentage(lProductFromDB);

                                }
                                else
                                {
                                    DisplaySystemError("Something went wrong, please check this product is still valid.");
                                    return;
                                }
                            }

                            // Rest of existing product save logic...
                            if (CreateCatalogOnLoad)
                            {
                                this.formIsDirty = false;
                                if (parent is FormSearchItems) { ((FormSearchItems)parent).IsReload = true; }
                                this.Close();
                            }

                            // Update form fields with saved prices
                            if (lProductFromDB != null)
                            {
                                TextBoxProductPurchasePrice.Text = lProductFromDB.PurchasePrice.ToString();
                                TextBoxProductCost.Text = lProductFromDB.CostPrice.ToString();
                                TextBoxProductRetailPrice.Text = lProductFromDB.RetailPrice.ToString();
                                TextBoxProductWholeSalePrice.Text = lProductFromDB.WholdSalePrice.ToString();
                                TextBoxProductMSRP.Text = lProductFromDB.Msrp.ToString();
                            }

                            ResetProductTab();
                            LoadProductCombo();
                            LoadCatalogWithFilter();
                            PointSaveorUpdatedNode(TreeViewCatalog.Nodes, lProductFromDB.Id.ToString() + "@");
                            EnableForm(false);
                            CatalogErrorMsg.Text = SaveProductSuccessText;
                            this.formIsDirty = false;
                        }
                        else
                        {
                            CatalogErrorMsg.Text = string.Format(UniqueProductCodeErrorMsg, TextBoxProductCode.Text);
                            TextBoxProductCode.Select();
                            return;
                        }
                    }
                    else
                    {
                        CatalogErrorMsg.Text = string.Format(UniqueNameErrorMsg, "Product", lProduct.Name);
                        TextBoxProductName.Select();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnCatalogExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Boolean validateFormCategory()
        {
            CatalogErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxCategoryName.Text.Trim()))
            {
                CatalogErrorMsg.Text = string.Format(EnterNameErrorMsg, "Category");
                TextBoxCategoryName.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCategoryParent.Text.Trim()) && CategoryManager.GetCategoryInfoById(long.Parse(TextBoxCategoryParent.Id)) == null)
            {
                CatalogErrorMsg.Text = "Somting went wrong, please check this parent is still valid.";
                TextBoxCategoryParent.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCategoryDefaultDiscount.Text.Trim()) && (float.Parse(TextBoxCategoryDefaultDiscount.Text) > 100))
            {
                CatalogErrorMsg.Text = EnterDefaultDisErrorMsg;
                TextBoxCategoryDefaultDiscount.Select();
                return false;
            }
            return true;
        }
        private Boolean validateFormProduct()
        {
            CatalogErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxProductName.Text.Trim()))
            {
                CatalogErrorMsg.Text = string.Format(EnterNameErrorMsg, "Product");
                TextBoxProductName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductParent.Text.Trim()))
            {
                CatalogErrorMsg.Text = ChooseProductParentErrorMsg;
                BtnProductParent.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxProductParent.Text.Trim()) && CatalogProductFamilyManager.GetProductFamilyInfoById(long.Parse(TextBoxProductParent.Id)) == null)
            {
                CatalogErrorMsg.Text = "Somting went wrong, please check this parent is still valid.";
                TextBoxProductParent.Select();
                return false;
            }
            if (string.IsNullOrEmpty(ComboBoxProductPurchesUOM.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterPurUomErrorMsg;
                ComboBoxProductPurchesUOM.Select();
                return false;
            }


            if (string.IsNullOrEmpty(ComboBoxProductRetailUOM.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterRetailUomErrorMsg;
                ComboBoxProductRetailUOM.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductXFactorRetail.Text.Trim()) || int.Parse(TextBoxProductXFactorRetail.Text.Trim()) < 1)
            {
                CatalogErrorMsg.Text = EnterRetailXFactorErrorMsg;
                TextBoxProductXFactorRetail.Select();
                return false;
            }
            if (string.IsNullOrEmpty(ComboBoxProductWholeSaleUOM.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterWholeSalUomErrorMsg;
                ComboBoxProductWholeSaleUOM.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductXFactorWholeSale.Text.Trim()) || int.Parse(TextBoxProductXFactorWholeSale.Text.Trim()) < 1)
            {
                CatalogErrorMsg.Text = EnterWholeSalXFactorErrorMsg;
                TextBoxProductXFactorWholeSale.Select();
                return false;
            }
            double Retail = double.Parse(TextBoxProductRetailPrice.Text);
            double Msrp = double.Parse(TextBoxProductMSRP.Text);
            if (Msrp != 0 && Retail > Msrp)
            {
                CatalogErrorMsg.Text = EnterMrpErrorMsg;
                TextBoxProductMSRP.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductCode.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterProductCodeErrorMsg;
                TextBoxProductCode.Select();
                return false;
            }
            if (ComboBoxProductRackNumber.TxtVisible && string.IsNullOrEmpty(ComboBoxProductRackNumber.Text.Trim()))
            {
                CatalogErrorMsg.Text = EnterRackNumberErrorMsg;
                ComboBoxProductRackNumber.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxProductDefaultDiscount.Text.Trim()) && (float.Parse(TextBoxProductDefaultDiscount.Text) > 100))
            {
                CatalogErrorMsg.Text = EnterDefaultDisErrorMsg;
                TextBoxProductDefaultDiscount.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                long PId = long.Parse(TextBoxCatalogId.Text);
                if (double.Parse(TextBoxOpeningstock.Text) != 0)
                {
                    IList<Inventory> Inventory = InventoryLocationManager.Instance.ListInventoryByProductId(PId);
                    if (!(Inventory != null && Inventory.Count > 0))
                    {
                        CatalogErrorMsg.Text = EnterBatchDetailErrorMsg;
                        TextBoxOpeningstock.Select();
                        return false;
                    }
                    if (Inventory != null && Inventory.Count > 0 && Math.Round(Inventory.Sum(x => x.OpeningStock), Global.Company.QuantityPricision) != double.Parse(TextBoxOpeningstock.Text))
                    {
                        CatalogErrorMsg.Text = EnterOpenStockErrorMsg;
                        TextBoxOpeningstock.Select();
                        return false;
                    }
                }

            }
            return true;
        }
        private Boolean validateFormProductFamily()
        {
            CatalogErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxProductFamilyName.Text.Trim()))
            {
                CatalogErrorMsg.Text = string.Format(EnterNameErrorMsg, "Product Family");
                TextBoxProductFamilyName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductFamilyParent.Text.Trim()))
            {
                CatalogErrorMsg.Text = ChooseProductFamilyParentErrorMsg;
                BtnProductFamilyParent.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxProductFamilyParent.Text.Trim()) && CategoryManager.GetCategoryInfoById(long.Parse(TextBoxProductFamilyParent.Id)) == null)
            {
                CatalogErrorMsg.Text = "Somting went wrong, please check this parent is still valid.";
                BtnProductFamilyParent.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxProductFamilyDefaultDiscount.Text.Trim()) && (float.Parse(TextBoxProductFamilyDefaultDiscount.Text) > 100))
            {
                CatalogErrorMsg.Text = EnterDefaultDisErrorMsg;
                TextBoxProductFamilyDefaultDiscount.Select();
                return false;
            }
            return true;
        }
        private void ResetCategoryTab()
        {
            TextBoxCategoryParent.ResetText();
            CatalogErrorMsg.Text = "";
            TextBoxCatalogId.ResetText();
            TextBoxCategoryDescription.ResetText();
            TextBoxCategoryDisplayName.ResetText();
            TextBoxCategoryName.ResetText();
            CheckBoxCategoryAtBatchLevel.Checked = false;
            ComboBoxCategoryDiscountAC.SelectedIndex = -1;
            ComboBoxCategoryInventoryAc.SelectedIndex = -1;
            ComboBoxCategoryPurchaseAc.SelectedIndex = -1;
            ComboBoxCategorySalesAc.SelectedIndex = -1;
            GridViewCategoryTaxDetails.Rows.Clear();
            TextBoxCategoryDefaultDiscount.ResetText();
            TextBoxCategoryDefaultDiscount.Decimals = 2;
        }
        private void ResetProductFamilyTab()
        {
            TextBoxProductFamilyParent.ResetText();
            CatalogErrorMsg.Text = "";
            TextBoxCatalogId.ResetText();
            TextBoxProductFamilyDescription.ResetText();
            TextBoxProductFamilyDisplayName.ResetText();
            TextBoxProductFamilyName.ResetText();
            CheckBoxProductFamilyAtBatchLevel.Checked = false;
            ComboBoxProductFamilyDiscountAC.SelectedIndex = -1;
            ComboBoxProductFamilyInventoryAc.SelectedIndex = -1;
            ComboBoxProductFamilyPurchaseAc.SelectedIndex = -1;
            ComboBoxProductFamilySalesAc.SelectedIndex = -1;
            ComboBoxProductFamilyManufacturer.SelectedIndex = -1;
            ComboBoxProductFamilySupplier.SelectedIndex = -1;
            ComboBoxProductFamilySupplier.ResetText();
            TextBoxProductfamilysupplier.ResetText();
            GridViewProductFamilyTaxDetails.Rows.Clear();
            GridViewProductFamilyChildProduct.Rows.Clear();
            TextBoxProductFamilyDefaultDiscount.ResetText();
            TextBoxProductFamilyDefaultDiscount.Decimals = 2;
        }
        private void ResetProductTab()
        {
            TextBoxProductParent.ResetText();
            barCodeProduct.NamePrice = "";
            barCodeProduct.Text = "0000000000";
            CatalogErrorMsg.Text = "";
            TextBoxCatalogId.ResetText();
            TextBoxProductCode.ResetText();
            TextBoxProductHSN.ResetText();
            TextBoxProductCost.ResetText();
            TextBoxProductCost.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxProductDescription.ResetText();
            TextBoxProductDisplayName.ResetText();
            CheckBoxProductMaintainInventory.Checked = false;
            CheckBoxProductAtBatchLevel.Checked = false;
            TextBoxProductName.ResetText();
            TextBoxProductPurchasePrice.ResetText();
            TextBoxProductRetailPrice.ResetText();
            TextBoxProductWholeSalePrice.ResetText();
            TextBoxProductMSRP.ResetText();
            TextBoxProductPurchasePrice.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxProductRetailPrice.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxProductWholeSalePrice.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxProductMSRP.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxProductXFactorRetail.Text = "1";
            TextBoxProductXFactorWholeSale.Text = "1";
            TextBoxOpeningstock.Text = Math.Round(0.00, Global.Company.QuantityPricision).ToString();
            CheckBoxUseHsnCode.Checked = false;
            ComboBoxProductInventoryAc.SelectedIndex = -1;
            ComboBoxProductPurchaseAc.SelectedIndex = -1;
            TextBoxProductDefaultDiscount.ResetText();
            TextBoxProductDefaultDiscount.Decimals = 2;
            ComboBoxProductDiscountAc.SelectedIndex = -1;
            ComboBoxProductSalesAc.SelectedIndex = -1;
            ComboBoxProductPurchesUOM.SelectedIndex = -1;
            ComboBoxProductRetailUOM.SelectedIndex = -1;
            ComboBoxProductSchedule.SelectedIndex = -1;
            ComboBoxProductRackNumber.SelectedIndex = -1;
            ComboBoxProductWholeSaleUOM.SelectedIndex = -1;
            ComboBoxProductManufacturer.SelectedIndex = -1;
            ComboBoxProductSupplier.ResetText();
            TextBoxProductSupplier.ResetText();
            ComboBoxProductSupplier.SelectedIndex = -1;
            GridViewProductTaxDetails.Rows.Clear();

        }

        private void EnableForm(Boolean enable)
        {
            if (TreeViewCatalog.Nodes.Count > 0)
            {
                TreeViewCatalog.Enabled = !enable;
                TextBoxCatalogSearch.ReadOnly = enable;
                TextBoxCatalogSearch.TabStop = !enable;

            }
            else if (!CreateCatalogOnLoad)
            {
                TreeViewCatalog.Enabled = false;
                TabControlCategory.Visible = true;
                TabControlProduct.Visible = false;
                TabControlProductFamily.Visible = false;
                TextBoxCatalogSearch.ReadOnly = true;
                TextBoxCatalogSearch.TabStop = false;
            }
            if (CreateCatalogOnLoad)
            {
                enable = true;
            }
            if (TabControlCategory.Visible)
            {
                TextBoxCategoryDefaultDiscount.ReadOnly = !enable;
                TextBoxCategoryDescription.ReadOnly = !enable;
                TextBoxCategoryDisplayName.ReadOnly = !enable;
                TextBoxCategoryName.ReadOnly = !enable;
                ComboBoxCategoryDiscountAC.Visible = enable;

                ComboBoxCategoryInventoryAc.Visible = enable;
                ComboBoxCategoryPurchaseAc.Visible = enable;
                ComboBoxCategorySalesAc.Visible = enable;
                BtnCategoryParent.Enabled = enable;

                if (TreeViewCatalog.Nodes.Count == 0)
                {
                    BtnCategoryParent.Enabled = false;
                }
                GridViewCategoryTaxDetails.Enabled = enable;
                CheckBoxCategoryMaintainInventory.Enabled = enable;
                CheckBoxCategoryAtBatchLevel.Enabled = enable;
                if (enable && !CheckBoxCategoryMaintainInventory.Checked)
                {
                    CheckBoxCategoryAtBatchLevel.Enabled = false;
                }
                TextBoxCategoryDescription.TabStop = enable;
                TextBoxCategoryDisplayName.TabStop = enable;
                TextBoxCategoryName.TabStop = enable;
                TextBoxCategoryDefaultDiscount.TabStop = enable;
                EditCategoryTaxLink.Visible = false;
                if (enable && !string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    EditCategoryTaxLink.Visible = true;
                }
            }
            else if (TabControlProductFamily.Visible)
            {
                TextBoxProductFamilyDefaultDiscount.ReadOnly = !enable;
                TextBoxProductFamilyDefaultDiscount.TabStop = enable;
                TextBoxProductFamilyDescription.ReadOnly = !enable;
                TextBoxProductFamilyDisplayName.ReadOnly = !enable;
                TextBoxProductFamilyName.ReadOnly = !enable;

                ComboBoxProductFamilyManufacturer.Visible = enable;
                ComboBoxProductFamilySupplier.Visible = enable;
                TextBoxProductfamilysupplier.Visible = !enable;
                TextBoxProductfamilysupplier.TabStop = enable;
                TextBoxProductfamilysupplier.BringToFront();

                ComboBoxProductFamilyInventoryAc.Visible = enable;
                ComboBoxProductFamilyDiscountAC.Visible = enable;

                BtnProductFamilyParent.Enabled = enable;
                ComboBoxProductFamilyPurchaseAc.Visible = enable;
                ComboBoxProductFamilySalesAc.Visible = enable;

                GridViewProductFamilyTaxDetails.Enabled = enable;
                CheckBoxProductFamilyMaintainInventory.Enabled = enable;
                CheckBoxProductFamilyAtBatchLevel.Enabled = enable;
                if (enable && !CheckBoxProductFamilyMaintainInventory.Checked)
                {
                    CheckBoxProductFamilyAtBatchLevel.Enabled = false;
                }
                TextBoxProductFamilyDescription.TabStop = enable;
                TextBoxProductFamilyDisplayName.TabStop = enable;
                TextBoxProductFamilyName.TabStop = enable;
                EditProductFamilyTaxLink.Visible = false;
                if (enable && !string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    EditProductFamilyTaxLink.Visible = true;
                }
            }
            else if (TabControlProduct.Visible)
            {
                ComboBoxProductRackNumber.TxtVisible = LabelRackNumber.Visible = Global.Company.MaintainRackNumber;
                BtnViewEditBatch.Text = enable ? string.IsNullOrEmpty(TextBoxCatalogId.Text) ? "Add Opening Stock" : "Edit Opening Stock" : "Edit Opening Stock";
                TextBoxOpeningstock.TabStop = false;
                TextBoxProductDefaultDiscount.ReadOnly = !enable;
                TextBoxProductDefaultDiscount.TabStop = enable;
                TextBoxProductCode.ReadOnly = !enable;
                TextBoxProductHSN.ReadOnly = !enable;
                TextBoxProductCost.ReadOnly = !enable;
                TextBoxProductDescription.ReadOnly = !enable;
                TextBoxProductDisplayName.ReadOnly = !enable;
                TextBoxProductName.ReadOnly = !enable;
                TextBoxProductPurchasePrice.ReadOnly = !enable;
                TextBoxProductRetailPrice.ReadOnly = !enable;
                TextBoxProductWholeSalePrice.ReadOnly = !enable;
                TextBoxProductMSRP.ReadOnly = !enable;
                TextBoxProductXFactorRetail.ReadOnly = !enable;
                TextBoxProductXFactorWholeSale.ReadOnly = !enable;
                CheckBoxUseHsnCode.Enabled = CreateCatalogOnLoad ? !enable : enable && string.IsNullOrEmpty(TextBoxProductHSN.Text.Trim()) ? !enable : enable;
                ComboBoxProductPurchesUOM.Visible = enable;
                ComboBoxProductRetailUOM.Visible = enable;
                if (Global.softwareType == SoftwareType.VVMATRIX || Global.softwareType == SoftwareType.MEDICARE)
                {
                    if (Global.Company.BusinessType == BuisnessType.Pharmacy || Global.Company.BusinessType == BuisnessType.Hospital)
                    {
                        ComboBoxProductSchedule.Visible = true;
                        ComboBoxProductSchedule.TxtVisible = label50.Visible = true;
                        ComboBoxProductSchedule.Visible = enable;
                    }
                    else
                    {
                        ComboBoxProductSchedule.Visible = false;
                        ComboBoxProductSchedule.TxtVisible = label50.Visible = false;
                    }
                }
                ComboBoxProductRackNumber.Visible = Global.Company.MaintainRackNumber ? enable : false;
                ComboBoxProductWholeSaleUOM.Visible = enable;

                TextBoxProductCode.TabStop = enable;
                TextBoxProductHSN.TabStop = enable;
                TextBoxProductCost.TabStop = enable;
                TextBoxProductDescription.TabStop = enable;
                TextBoxProductDisplayName.TabStop = enable;

                TextBoxProductName.TabStop = enable;
                TextBoxProductPurchasePrice.TabStop = enable;
                TextBoxProductRetailPrice.TabStop = enable;
                TextBoxProductWholeSalePrice.TabStop = enable;
                TextBoxProductMSRP.TabStop = enable;
                TextBoxProductXFactorRetail.TabStop = enable;
                TextBoxProductXFactorWholeSale.TabStop = enable;
                ComboBoxProductManufacturer.Visible = enable;
                ComboBoxProductSupplier.Visible = enable;

                TextBoxProductSupplier.Visible = !enable;
                TextBoxProductSupplier.TabStop = enable;
                TextBoxProductSupplier.BringToFront();
                ComboBoxProductInventoryAc.Visible = enable;
                BtnProductParent.Enabled = enable;
                BtnPriceCalculator.Enabled = enable;
                ComboBoxProductPurchaseAc.Visible = enable;
                ComboBoxProductDiscountAc.Visible = enable;
                ComboBoxProductSalesAc.Visible = enable;

                GridViewProductTaxDetails.Enabled = enable;
                CheckBoxProductMaintainInventory.Enabled = enable;
                CheckBoxProductAtBatchLevel.Enabled = enable;
                BtnViewEditBatch.Enabled = enable;
                if (enable && string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    CheckBoxProductAtBatchLevel.Enabled = !enable;
                }
                if (enable && !string.IsNullOrEmpty(TextBoxCatalogId.Text) && !CheckBoxProductMaintainInventory.Checked)
                {
                    CheckBoxProductAtBatchLevel.Enabled = !enable;
                }
                if (enable && CheckBoxProductMaintainInventory.Checked)
                {
                    CheckBoxProductMaintainInventory.Enabled = true;

                    CheckBoxProductAtBatchLevel.Enabled = true;
                }
                if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    BtnPrintBarCode.Enabled = false;
                    BtnPrintTocken.Enabled = false;
                }
                else
                {
                    BtnPrintBarCode.Enabled = true;
                    BtnPrintTocken.Enabled = true;

                }
                EditProductTaxLink.Visible = false;
                if (enable && !string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    EditProductTaxLink.Visible = true;
                }
            }
            if (!enable)
            {
                if (TreeViewCatalog.SelectedNode == null)
                {
                    BtnCatalogImport.Enabled = !enable;
                    BtnCatalogDelete.Enabled = enable;
                    BtnCatalogEdit.Enabled = enable;
                }
                else
                {
                    BtnCatalogImport.Enabled = !enable;
                    BtnCatalogDelete.Enabled = !enable;
                    BtnCatalogEdit.Enabled = !enable;
                }
                BtnCatalogCancel.Enabled = enable;
                BtnCatalogNew.Enabled = !enable;
                BtnCatalogSave.Enabled = enable;
            }
            else
            {
                BtnCatalogImport.Enabled = !enable;
                BtnCatalogDelete.Enabled = !enable;
                BtnCatalogEdit.Enabled = !enable;
                BtnCatalogCancel.Enabled = enable;
                BtnCatalogNew.Enabled = !enable;
                BtnCatalogSave.Enabled = enable;
            }
        }

        private void ComboBoxCategorySalesAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCategorySalesAc.DroppedDown = false;
        }

        private void ComboBoxCategoryPurchaseAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCategoryPurchaseAc.DroppedDown = false;
        }

        private void ComboBoxCategoryInventoryAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCategoryInventoryAc.DroppedDown = false;
        }
        private void ComboBoxProductPurchesUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductPurchesUOM.DroppedDown = false;

        }

        private void ComboBoxProductRetailUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductRetailUOM.DroppedDown = false;
        }
        private void ComboBoxProductSchedule_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductSchedule.DroppedDown = false;
        }

        private void ComboBoxProductRackNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductRackNumber.DroppedDown = false;
        }
        private void ComboBoxProductWholeSaleUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductWholeSaleUOM.DroppedDown = false;
        }
        private void ComboBoxProductSalesAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductSalesAc.DroppedDown = false;
        }

        private void ComboBoxProductPurchaseAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductPurchaseAc.DroppedDown = false;
        }
        private void CombBoxProductDiscountAC_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductDiscountAc.DroppedDown = false;
        }
        private void ComboBoxProductFamilyDiscountAC_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductFamilyDiscountAC.DroppedDown = false;

        }
        private void ComboBoxCategoryDiscountAC_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxCategoryDiscountAC.DroppedDown = false;

        }
        private void ComboBoxProductInventoryAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductInventoryAc.DroppedDown = false;
        }

        private void ComboBoxProductFamilySalesAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductFamilySalesAc.DroppedDown = false;
        }

        private void ComboBoxProductFamilyPurchaseAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductFamilyPurchaseAc.DroppedDown = false;
        }

        private void ComboBoxProductFamilyInventoryAc_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxProductFamilyInventoryAc.DroppedDown = false;
        }

        private void TextBoxProductXFactorRetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }
        private void FormCatalog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (!BtnCatalogEdit.Enabled)
                {
                    DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        TabFocused();
                        e.Cancel = true;
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnCatalogNew.ShowDropDown();
            }
            if (keyData == (Keys.F4))
            {
                BtnCatalogDelete.PerformClick();
            }
            if (keyData == (Keys.F7))
            {
                BtnCatalogEdit.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnCatalogSave.PerformClick();
            }
            if (keyData == (Keys.F10))
            {
                BtnCatalogExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                if (CreateCatalogOnLoad)
                {
                    BtnCatalogExit.PerformClick();
                }
                else
                {
                    BtnCatalogCancel.PerformClick();
                }
                return true;
            }
            try
            {
                int count = Global.Company.SalesTaxAccountMaps.Count - 1;
                if (keyData == Keys.Tab && ComboBoxProductDiscountAc.Focused)
                {
                    if (GridViewCategoryTaxDetails.RowCount == 0)
                    {
                        BtnCatalogSave.Select();
                    }
                    else
                    {
                        GridViewCategoryTaxDetails.Select();
                    }
                    return true;
                }

                if (GridViewCategoryTaxDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab))
                    {
                        if (GridViewCategoryTaxDetails.Visible)
                        {
                            if (GridViewCategoryTaxDetails.CurrentCell.ColumnIndex == 1 && GridViewCategoryTaxDetails.CurrentCell.RowIndex != count)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                        else if (GridViewProductFamilyTaxDetails.Visible)
                        {
                            if (GridViewProductFamilyTaxDetails.CurrentCell != null && GridViewProductFamilyTaxDetails.CurrentCell.ColumnIndex == 1 && GridViewProductFamilyTaxDetails.CurrentCell.RowIndex != count)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                        else if (GridViewProductTaxDetails.Visible)
                        {

                            if (GridViewProductTaxDetails.CurrentCell.ColumnIndex == 1 && GridViewProductTaxDetails.CurrentCell.RowIndex != count)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift))
                    {
                        if (GridViewCategoryTaxDetails.Visible)
                        {
                            if (GridViewCategoryTaxDetails.CurrentCell.ColumnIndex == 1)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                        else if (GridViewProductFamilyTaxDetails.Visible)
                        {
                            if (GridViewProductFamilyTaxDetails.CurrentCell.ColumnIndex == 1)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                        else if (GridViewProductTaxDetails.Visible)
                        {
                            if (GridViewProductTaxDetails.CurrentCell.ColumnIndex == 1)
                            {
                                SendKeys.Send("{tab}");
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ComboBoxProductFamilyManufacturer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductFamilyManufacturer.DroppedDown = false;
        }

        private void ComboBoxProductFamilySupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductFamilySupplier.DroppedDown = false;
        }
        private void ComboBoxProductManufacturer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductManufacturer.DroppedDown = false;

        }

        private void ComboBoxProductSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxProductName_KeyPress(sender, e);
            this.ComboBoxProductSupplier.DroppedDown = false;

        }
        private void TreeViewCatalog_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ParentNode = e.Node;
            }
        }
        private void TextBoxCategoryParent_TextChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (BtnCategoryParent.Enabled && !string.IsNullOrEmpty(TextBoxCategoryParent.Text))
            {
                BtnCategoryRemoveParent.Visible = true;
            }
            else
            {
                BtnCategoryRemoveParent.Visible = false;
            }
            CatalogItem Category = null;
            long? Id = null;
            //if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            //{
            //    Id = long.Parse(TextBoxCatalogId.Text.ToString());
            //}
            if (!string.IsNullOrEmpty(TextBoxCategoryParent.Text))
            {
                Id = long.Parse(TextBoxCategoryParent.Id);
            }
            if (string.IsNullOrEmpty(TextBoxCatalogId.Text) && Id != null)
            {
                Category = CategoryManager.GetCategoryInfoById((long)Id);
            }
            else if (Id == null)
            {
                ComboBoxCategoryInventoryAc.SelectedIndex = -1;
                ComboBoxCategoryPurchaseAc.SelectedIndex = -1;
                ComboBoxCategorySalesAc.SelectedIndex = -1;
                ComboBoxCategoryDiscountAC.SelectedIndex = -1;

                LoadSalesTax(GridViewCategoryTaxDetails, SalesTaxMapFromCompany());
                return;
            }
            else
            {
                Category = CategoryManager.GetCategoryInfoById(long.Parse(TextBoxCatalogId.Text));
                Category.Parent = new Category();
                Category.ParentId = Id;
            }
            LoadCategeoryAccount(Category);
            LoadSalesTax(GridViewCategoryTaxDetails, Category.SalesTax.ToList());
            Cursor.Current = Cursors.Default;

        }

        private void TextBoxProductParent_TextChanged(object sender, EventArgs e)
        {
            CatalogItem ProductFamily = null; long? Id = null;
            if (!string.IsNullOrEmpty(TextBoxProductParent.Text))
            {
                Id = long.Parse(TextBoxProductParent.Id);
            }
            if (string.IsNullOrEmpty(TextBoxCatalogId.Text) && Id != null)
            {
                ProductFamily = CatalogProductFamilyManager.GetProductFamilyInfoById((long)Id);
            }
            else if (Id == null)
            {
                ComboBoxProductInventoryAc.SelectedIndex = -1;
                ComboBoxProductPurchaseAc.SelectedIndex = -1;
                ComboBoxProductDiscountAc.SelectedIndex = -1;
                ComboBoxProductSalesAc.SelectedIndex = -1;
                LoadSalesTax(GridViewProductTaxDetails, SalesTaxMapFromCompany());
                return;
            }
            else
            {
                ProductFamily = CatalogProductManager.GetProductInfoById(long.Parse(TextBoxCatalogId.Text));
                if (ProductFamily != null)
                {
                    ProductFamily.ParentId = Id;
                }
            }
            if (ProductFamily != null)
            {
                LoadProductAccount(ProductFamily);
                LoadSalesTax(GridViewProductTaxDetails, ProductFamily.SalesTax.ToList());
            }
        }

        private void TextBoxProductFamilyParent_TextChanged(object sender, EventArgs e)
        {
            CatalogItem Category = null;
            long? Id = null;
            if (!string.IsNullOrEmpty(TextBoxProductFamilyParent.Text))
            {
                Id = long.Parse(TextBoxProductFamilyParent.Id);
            }
            if (string.IsNullOrEmpty(TextBoxCatalogId.Text) && Id != null)
            {
                Category = CategoryManager.GetCategoryInfoById((long)Id);
            }
            else if (Id == null)
            {
                ComboBoxProductFamilyInventoryAc.SelectedIndex = -1;
                ComboBoxProductFamilyPurchaseAc.SelectedIndex = -1;
                ComboBoxProductFamilySalesAc.SelectedIndex = -1;
                ComboBoxProductFamilyDiscountAC.SelectedIndex = -1;

                LoadSalesTax(GridViewProductFamilyTaxDetails, SalesTaxMapFromCompany());
                return;
            }
            else
            {
                Category = CatalogProductFamilyManager.GetProductFamilyInfoById(long.Parse(TextBoxCatalogId.Text));
                if (Category != null)
                {
                    Category.ParentId = Id;
                }
            }
            if (Category != null)
            {
                LoadProductFamilyAccount(Category);
                LoadSalesTax(GridViewProductFamilyTaxDetails, Category.SalesTax.ToList());
            }
        }
        private void ResetTabs()
        {
            if (TabControlCategory.Visible)
            {
                ResetCategoryTab();
            }
            else if (TabControlProductFamily.Visible)
            {
                ResetProductFamilyTab();
            }
            else if (TabControlProduct.Visible)
            {
                ResetProductTab();
            }
        }
        bool IsPerformSearch = true;
        private void TextBoxCatalogSearch_TextChanged(object sender, EventArgs e)
        {
            if (IsPerformSearch)
            {
                CatalogSearchTextChange();
            }
            else
            {
                IsPerformSearch = true;
            }
        }
        private void CatalogSearchTextChange()
        {
            ResetTabs();
            LoadCatalogWithFilter();
            this.formIsDirty = false;
        }
        private void ComboBoxProductDiscountAc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (EditProductTaxLink.Visible)
                {
                    EditProductTaxLink.Select();
                }
                else
                {
                    BtnCatalogSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxProductDefaultDiscount.Select();
            }
        }
        private void ComboBoxProductFamilyDiscountAC_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (EditProductFamilyTaxLink.Visible)
                {
                    EditProductFamilyTaxLink.Select();
                }
                else
                {
                    BtnCatalogSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxProductFamilyDefaultDiscount.Select();
            }
        }
        private void ComboBoxCategoryDiscountAC_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (EditCategoryTaxLink.Visible)
                {
                    EditCategoryTaxLink.Select();
                }
                else
                {
                    BtnCatalogSave.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxCategoryDefaultDiscount.Select();
            }
        }
        private void BtnCatalogSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlCategory.Visible)
                {
                    TextBoxCategoryName.Select();
                }
                else if (TabControlProductFamily.Visible)
                {
                    TextBoxProductFamilyName.Select();
                }
                else if (TabControlProduct.Visible)
                {
                    TextBoxProductName.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (TabControlCategory.Visible)
                {
                    if (EditCategoryTaxLink.Visible)
                    {
                        EditCategoryTaxLink.Select();
                    }
                    else
                    {
                        ComboBoxCategoryDiscountAC.Select();
                    }
                }
                else if (TabControlProductFamily.Visible)
                {
                    if (EditProductFamilyTaxLink.Visible)
                    {
                        EditProductFamilyTaxLink.Select();
                    }
                    else
                    {
                        ComboBoxProductFamilyDiscountAC.Select();
                    }
                }
                else if (TabControlProduct.Visible)
                {
                    if (EditProductTaxLink.Visible)
                    {
                        EditProductTaxLink.Select();
                    }
                    else
                    {
                        ComboBoxProductDiscountAc.Select();
                    }
                }
            }
        }
        private void TextBoxCategoryName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxCategoryDisplayName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCatalogSave.Select();
            }
        }
        private void TextBoxProductName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxProductDisplayName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCatalogSave.Select();
            }
        }
        private void TextBoxProductFamilyName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxProductFamilyDisplayName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCatalogSave.Select();
            }
        }
        private void TextBoxProductCode_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxProductCode.Text))
            {
                barCodeProduct.NamePrice = "";
                barCodeProduct.Text = "0000000000";
            }
            else
            {
                barCodeProduct.NamePrice = TextBoxProductName.Text + " / Rs. " + TextBoxProductRetailPrice.Text;
                barCodeProduct.Text = TextBoxProductCode.Text;
            }
        }
        private void TextBoxProductName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
        }
        private void TextBoxProductCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
        }
        private void TextBoxProductHSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
        }
        private void BtnPrintBarCode_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Enabled = false; // Disable parent form
            try
            {
                if (string.IsNullOrEmpty(TextBoxCatalogId.Text) || CatalogProductManager.GetProductInfoById(long.Parse(TextBoxCatalogId.Text)) == null)
                {
                    DisplaySystemError("Somthing went wrong, the selected product is not valid.");
                    return;
                }

                using (FormCatalogBarCodePrint printForm = new FormCatalogBarCodePrint(this))
                {
                    printForm.ProductId = long.Parse(TextBoxCatalogId.Text);
                    printForm.ShowDialog();
                }
            }
            finally
            {
                this.Enabled = true; // Re-enable parent form
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnPrintTocken_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(TextBoxCatalogId.Text) || CatalogProductManager.GetProductInfoById(long.Parse(TextBoxCatalogId.Text)) == null)
            {
                DisplaySystemError("Somthing went wrong, the selected product is not valid.");
                return;
            }
            FormCatalogTokenPrint printForm = new FormCatalogTokenPrint();
            printForm.ProductId = long.Parse(TextBoxCatalogId.Text);
            printForm.Show();
            Cursor.Current = Cursors.Default;
        }
        private void BtnCategoryParent_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCatalogParent FormCatalogParent = new FormCatalogParent(this);
            FormCatalogParent.Type = Type();
            FormCatalogParent.CatalogItemId = CatalogItemId();
            FormCatalogParent.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        private void BtnCategoryParentOrProductFmly_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCatalogParent FormCatalogParent = new FormCatalogParent(this);
            FormCatalogParent.SearchType = FormCatalogParent.SEARCH_TYPE_PARENT_AND_PRODUCTFMLY;
            FormCatalogParent.Type = Type();
            FormCatalogParent.CatalogItemId = CatalogItemId();
            FormCatalogParent.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        private long CatalogItemId()
        {
            long Id = 0L;
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                Id = Convert.ToInt64(TextBoxCatalogId.Text);
            }
            return Id;
        }
        private CatalogItemType Type()
        {
            CatalogItemType CatalogItemType = CatalogItemType.CATEGORY;
            if (TabControlProductFamily.Visible)
            {
                CatalogItemType = CatalogItemType.PRODUCTFAMILY;
            }
            else if (TabControlProduct.Visible)
            {
                CatalogItemType = CatalogItemType.PRODUCT;
            }
            return CatalogItemType;
        }
        private void TextBoxCatalogSearch_KeyDown(object sender, KeyEventArgs e)
        {
            CatalogErrorMsg.Text = "";
            if (e.KeyCode == Keys.Down)
            {
                TreeViewCatalog.Select();
            }
        }
        private void BtnCategoryRemoveParent_Click(object sender, EventArgs e)
        {
            TextBoxCategoryParent.ResetText();
        }
        private void BtnCategoryParent_EnabledChanged(object sender, EventArgs e)
        {
            if (BtnCategoryParent.Enabled && !string.IsNullOrEmpty(TextBoxCategoryParent.Text))
            {
                BtnCategoryRemoveParent.Visible = true;
            }
            else
            {
                BtnCategoryRemoveParent.Visible = false;
            }
        }
        private void TreeViewCatalog_KeyDown(object sender, KeyEventArgs e)
        {
            if (TreeViewCatalog.SelectedNode != null && e.KeyCode == Keys.Apps)
            {
                ParentNode = TreeViewCatalog.SelectedNode;
            }
        }
        private void TextBoxOpeningstock_Leave(object sender, EventArgs e)
        {
            if (CheckBoxProductMaintainInventory.Checked)
            {
                if (formIsDirty || string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    if (validateFormProduct())
                    {
                        BtnCatalogSave_Click(sender, e);
                        if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
                        {
                            LoadOpeningStock();
                            BtnCatalogEdit_Click(sender, e);
                        }
                    }
                }
                else
                {
                    LoadOpeningStock();
                    BtnCatalogEdit_Click(sender, e);
                }
            }
        }
        private void LoadOpeningStock()
        {
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                FormCatalogBatchEntry BatchEntry = new FormCatalogBatchEntry(this);
                BatchEntry.ProductId = TextBoxCatalogId.Text;
                BatchEntry.EditMode = !BtnCatalogEdit.Enabled;
                BatchEntry.IsBatch = CheckBoxProductAtBatchLevel.Checked;
                BatchEntry.ShowDialog();
                Product Product = CatalogProductManager.GetProductInfoById(long.Parse(TextBoxCatalogId.Text));
                if (Product != null)
                {
                    IList<Inventory> lInventory = InventoryLocationManager.Instance.ListInventoryOpeningStockByProductId(Product.Id);
                    TextBoxOpeningstock.Text = Math.Round(lInventory.Sum(X => X.OpeningStock), Global.Company.QuantityPricision).ToString();
                }
                LoadCatalogInfo();
            }
        }
        string TempQty = string.Empty;
        private void CheckBoxMaintainInventory_CheckedChanged(object sender, EventArgs e)
        {
            if (!CheckBoxProductMaintainInventory.Checked)
            {
                CheckBoxProductAtBatchLevel.Checked = false;
                CheckBoxProductAtBatchLevel.Enabled = false;
                TempQty = TextBoxOpeningstock.Text;
                TextBoxOpeningstock.Text = Math.Round(0.00, Global.Company.QuantityPricision).ToString();
            }

            if (!CheckBoxProductMaintainInventory.Checked)
            {
                BtnViewEditBatch.Visible = false;
            }
            else
            {
                BtnViewEditBatch.Visible = true;
                if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
                {
                    if (InventoryLocationManager.Instance.GetInventoryByProductId(long.Parse(TextBoxCatalogId.Text)) != null)
                    {
                        TextBoxOpeningstock.Text = Math.Round(String.IsNullOrEmpty(TempQty) ? default(decimal) : Convert.ToDecimal(TempQty), Global.Company.QuantityPricision).ToString();
                    }
                }
            }

            if (CheckBoxProductMaintainInventory.Checked)
            {
                CheckBoxProductAtBatchLevel.Enabled = true;
            }
        }
        private void CheckBoxProductAtBatchLevel_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                if (CheckBoxProductAtBatchLevel.Checked)
                {
                    TempQty = TextBoxOpeningstock.Text;
                    TextBoxOpeningstock.Text = Math.Round(0.00, Global.Company.QuantityPricision).ToString();
                }
                else
                {
                    BtnViewEditBatch.Visible = false;
                    TextBoxOpeningstock.Text = Math.Round(String.IsNullOrEmpty(TempQty) ? default(decimal) : Convert.ToDecimal(TempQty), Global.Company.QuantityPricision).ToString();
                }
            }
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text) && CheckBoxProductAtBatchLevel.Checked)
            {
                BtnViewEditBatch.Visible = true;

            }
        }
        private void BtnPriceCalculator_Click(object sender, EventArgs e)
        {
            FormItemSpecialPriceCalculator PriceCalculator = new FormItemSpecialPriceCalculator(this);
            PriceCalculator.ShowDialog(this);
        }
        private void CheckBoxProductFamilyMaintainInventory_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxProductFamilyMaintainInventory.Checked)
            {
                CheckBoxProductFamilyAtBatchLevel.Enabled = true;
            }
            else
            {
                CheckBoxProductFamilyAtBatchLevel.Checked = false;
                CheckBoxProductFamilyAtBatchLevel.Enabled = false;

            }
        }
        private void CheckBoxCategoryMaintainInventory_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxCategoryMaintainInventory.Checked)
            {
                CheckBoxCategoryAtBatchLevel.Enabled = true;
            }
            else
            {
                CheckBoxCategoryAtBatchLevel.Checked = false;
                CheckBoxCategoryAtBatchLevel.Enabled = false;

            }
        }
        public bool IsResetCatalog = false;
        private void BtnCatalogImport_Click(object sender, EventArgs e)
        {
            FormDataMigrator Migrator = new FormDataMigrator(this);
            Migrator.ShowDialog();
            if (IsResetCatalog)
            {
                ResetCategoryTab();
                LoadCategoryCombo();
                LoadCatalogWithFilter();
                EnableForm(false);
                this.formIsDirty = false;
            }
        }
        private void BtnCatalogReport_Click(object sender, EventArgs e)
        {
            CatalogErrorMsg.Text = "";
            FormItemReport ItemReport = new FormItemReport();
            ItemReport.ShowDialog();
        }
        private void TextBoxProductHSN_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxProductHSN.Text.Trim()))
            {
                ItemTax ItemTax = ItemTaxManager.Instance.GetItemTaxByCode(TextBoxProductHSN.Text.Trim(), Global.Company.CompanyId);
                if (ItemTax != null)
                {
                    CheckBoxUseHsnCode.Enabled = true;
                }
            }
        }
        private void TextBoxProductHSN_TextChanged(object sender, EventArgs e)
        {
            CheckBoxUseHsnCode.Checked = false;
            CheckBoxUseHsnCode.Enabled = false;
            LoadTaxCodeTax(GridViewProductTaxDetails);
        }
        private void CheckBoxUseHsnCode_CheckedChanged(object sender, EventArgs e)
        {
            LoadTaxCodeTax(GridViewProductTaxDetails);
        }
        private void LoadTaxCodeTax(DataGridView DataGridView)
        {
            if (CheckBoxUseHsnCode.Checked)
            {
                ItemTax ItemTax = ItemTaxManager.Instance.GetItemTaxCodeByCode(TextBoxProductHSN.Text.Trim(), Global.Company.CompanyId);
                if (ItemTax != null)
                {
                    DataGridView.Rows.Clear();
                    TaxCodeId = ItemTax.Id;
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
                                    DataGridView.Rows.Add();
                                    DataGridView.Rows[i].Cells[0].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == ItemTaxMap.SalesTaxMapId).Name;

                                    if (ItemTaxMap.EffectiveFromDate <= Global.getTransactionDate() && ItemTaxMap.EffectiveToDate >= Global.getTransactionDate())
                                    {
                                        DataGridView.Rows[i].Cells[1].Value = ItemTaxMap.TaxPercentage.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                                    }
                                    else
                                    {
                                        DataGridView.Rows[i].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);

                                    }
                                    DataGridView.Rows[i].Cells[2].Value = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == ItemTaxMap.SalesTaxMapId).MapId;
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
            else if (TabControlProduct.Visible && !string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                Product Product = GetProductInfo();
                if (Product != null)
                {
                    LoadSalesTax(GridViewProductTaxDetails, Product.SalesTax.ToList());
                }
            }
            else
            {
                LoadSalesTax(GridViewProductTaxDetails, SalesTaxMapFromCompany());
            }
            GridViewProductTaxDetails.Columns[1].ReadOnly = true;
        }
        private void TextBoxProductName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Instance.Keypress_PasteCheckingProduct(sender, e, "NameChecking");
        }
        private void TextBoxProductName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.Instance.AddContextMenuProduct(TextBoxProductName, "NameChecking");
            }
        }

        private void CheckBoxUseHsnCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && BtnPrintBarCode.Enabled)
            {
                BtnPrintBarCode.Focus();
            }
            if ((e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab))
            {
                TextBoxProductHSN.Focus();
            }
        }

        private void BtnPrintBarCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && BtnPrintTocken.Enabled)
            {
                BtnPrintTocken.Focus();
            }
            if ((e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab) && CheckBoxUseHsnCode.Enabled)
            {
                CheckBoxUseHsnCode.Focus();
            }
        }

        private void BtnPrintTocken_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && BtnViewEditBatch.Enabled)
            {
                BtnViewEditBatch.Focus();
            }
            if ((e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab) && BtnPrintBarCode.Enabled)
            {
                BtnPrintBarCode.Focus();
            }
        }

        private void BtnViewEditBatch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && CheckBoxProductMaintainInventory.Enabled)
            {
                CheckBoxProductMaintainInventory.Select();
            }
            if (e.KeyCode == Keys.Tab && !CheckBoxProductMaintainInventory.Enabled)
            {
                ComboBoxProductSalesAc.Select();
            }
            if ((e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab) && BtnPrintTocken.Enabled)
            {
                BtnPrintTocken.Focus();
            }
        }

        private void CheckBoxProductMaintainInventory_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab) && BtnViewEditBatch.Visible)
            {
                e.IsInputKey = true;
                BtnViewEditBatch.Focus();
            }
            else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxProductSchedule.Select();
            }
        }

        private void editTaxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxCatalogId.Text))
            {
                if (TabControlProduct.Visible && CheckBoxUseHsnCode.Checked)
                {
                    FormTaxCode FormTaxCode = new FormTaxCode();
                    FormTaxCode.UpdateTaxCodeOnLoad = true;
                    FormTaxCode.TaxCodeId = TaxCodeId;
                    FormTaxCode.ShowDialog();
                    LoadTaxCodeTax(GridViewProductTaxDetails);
                }
                else
                {
                    FormCatalogTax formCatalogTax = new FormCatalogTax();
                    formCatalogTax.CatalogItemId = long.Parse(TextBoxCatalogId.Text);
                    formCatalogTax.ShowDialog();
                    if (TabControlCategory.Visible)
                    {
                        Category CategoryFromDB = GetCategoryInfo();
                        if (CategoryFromDB != null)
                        {
                            LoadSalesTax(GridViewCategoryTaxDetails, CategoryFromDB.SalesTax.ToList());
                        }
                    }
                    else if (TabControlProductFamily.Visible)
                    {
                        ProductFamily ProductFamilyFromDB = GetProductFamilyInfo();
                        if (ProductFamilyFromDB != null)
                        {
                            LoadSalesTax(GridViewProductFamilyTaxDetails, ProductFamilyFromDB.SalesTax.ToList());
                        }
                    }
                    else if (TabControlProduct.Visible)
                    {
                        Product Product = GetProductInfo();
                        if (Product != null)
                        {
                            LoadSalesTax(GridViewProductTaxDetails, Product.SalesTax.ToList());
                        }
                    }
                }
            }
            this.formIsDirty = false;
        }
        private void EditCategoryTaxLink_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCatalogSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxCategoryDiscountAC.Select();
            }
        }

        private void EditProductFamilyTaxLink_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxProductFamilyDiscountAC.Select();
            }
        }

        private void GridViewCategoryTaxDetails_PreviewKeyDown(global::System.Object sender, global::System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (GridViewCategoryTaxDetails.CurrentCell == GridViewCategoryTaxDetails[1, GridViewCategoryTaxDetails.Rows.Count - 1])
            {
                if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
                {
                    e.IsInputKey = true;
                    if (EditCategoryTaxLink.Visible)
                    {
                        EditCategoryTaxLink.Select();
                    }
                    else
                    {
                        BtnCatalogSave.Select();
                    }
                }
            }
        }

        private void EditProductTaxLink_PreviewKeyDown(global::System.Object sender, global::System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnCatalogSave.Select();
            }
            else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxProductDiscountAc.Select();
            }
        }

        private void GridViewProductTaxDetails_PreviewKeyDown(global::System.Object sender, global::System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (GridViewProductTaxDetails.CurrentCell == GridViewProductTaxDetails[1, GridViewProductTaxDetails.Rows.Count - 1])
                {
                    e.IsInputKey = true;
                    if (EditProductTaxLink.Visible)
                    {
                        EditProductTaxLink.Select();
                    }
                    else
                    {
                        BtnCatalogSave.Select();
                    }
                }
            }
        }
        private async Task SaveOrCalculateProductPercentage(Product product)
        {
            ProductPercentage toSave = this.PendingPercentages ?? CalculatePercentageFromPrices(product);

            if (toSave != null)
            {
                await ProductSalePercentageManager.Instance.SaveProductPercentagesAsync(toSave);
                this.PendingPercentages = null!;
            }
        }

        private ProductPercentage CalculatePercentageFromPrices(Product product)
        {
            decimal.TryParse(TextBoxProductPurchasePrice.Text, out decimal purchasePrice);
            decimal.TryParse(TextBoxProductCost.Text, out decimal costPrice);
            decimal.TryParse(TextBoxProductRetailPrice.Text, out decimal retailPrice);
            decimal.TryParse(TextBoxProductWholeSalePrice.Text, out decimal wholesalePrice);
            decimal.TryParse(TextBoxProductMSRP.Text, out decimal mrpPrice);

            // If no purchase price, nothing can be calculated
            if (purchasePrice <= 0)
                return null!;

            // Safe calculations
            var addedCostPct = (costPrice > 0)
                ? ((costPrice - purchasePrice) / purchasePrice) * 100
                : 0;

            var retailMarginPct = (mrpPrice > 0 && retailPrice > 0)
                ? ((mrpPrice - retailPrice) / mrpPrice) * 100
                : 0;

            var wholesaleMarginPct = (mrpPrice > 0 && wholesalePrice > 0)
                ? ((mrpPrice - wholesalePrice) / mrpPrice) * 100
                : 0;

            var mrpPct = (costPrice > 0)
                ? ((mrpPrice - costPrice) / costPrice) * 100 + 100
                : 0;

            return new ProductPercentage
            {
                ProductId = product.Id,
                ProductCode = product.MaterialId,
                CompanyId = Global.Company.CompanyId,
                AddedCostPercentage = Math.Round(addedCostPct, 0),
                RetailMarginPercentage = Math.Round(retailMarginPct, 0),
                WholesaleMarginPercentage = Math.Round(wholesaleMarginPct, 0),
                MrpPercentage = Math.Round(mrpPct, 0),
                IsActive = true
            };
        }


        private async void BtnPercentage_Click(object sender, EventArgs e)
        {
            try
            {
                await ProductSalePercentageManager.Instance.AddMissingProductPercentagesAsync();
                CatalogErrorMsg.Text = "Missing product percentages added successfully.";
            }
            catch (Exception ex)
            {
                CatalogErrorMsg.Text = "Failed to update product percentages: " + ex.Message;
            }
        }

    }
}
