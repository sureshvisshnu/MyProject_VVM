using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.views;
using fa.views.controls.accounting;
using fa.views.sales;
using fa.views.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa;
using fa.model.OrderManagement;
using fa.api.OrderManagement;
using VisioForge.Libs.MediaFoundation.OPM;
using fa.api.Accounting;
using fa.views.account.masters;
using fa.api.catalog;
using fa.views.purchase;
using fa.model.Catalog;
using fa.model.Accounting.Transaction;
using fa.libraries.utils;
using Microsoft.Win32;
using Fa.views.utils.Purchase;
using fa.views.utils;
using DocumentFormat.OpenXml.ExtendedProperties;
using static iTextSharp.awt.geom.Point2D;
using fa.api.Hms;
using System.Diagnostics;
using fa.views.controls.grid;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace Fa.views.purchase
{
    public enum PurchaseOrderEntryTableColumn
    {
        SNO, PRODUCT, UOM, QTY, PRICE, AMOUNT, REMOVE, ID, TAXP, PURCHASEDETAILID
    }
    public enum PurchaseOrderEntryTotalTableColumn
    {
        NAME, VALUE
    }
    public partial class FormPurchaseOrder : FormBase
    {
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string DeleteErrorText = "Error in Sales Quote Deleting. !";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string Grid_ChooseItemErrorMsg = "Please choose product.";
        public static string ChooseSupplierErrorMsg = "Please select supplier.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please {0} {1}.";
        public static string EnterPurchaseOrderEntryDateErrorMsg = "Please enter purchase order date.";
        public static string SelectLocationErrMsg = "Please select location.";
        public static string Grid_EmptyErrorMsg = "Please enter purchase Order Items details.";
        public static string InvalidPurchaseOrderEntryErrorMsg = "Invalid purchase.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Supplier Name or Order Date or Order Reference Number.";
        public static string PurchaseOrderSearchOutput = "No purchase order found";
        public static string DeleteConfirmText = "Do you want to delete Purchase Order Entry {0}?";
        public static string UpdateCustomerLicenceErrorMsg = "Please update customer licence info.";

        public long SupplierId = 0L;
        public long ProductId = 0L;
        public long SearchPurchaseId = 0L;

        public FormPurchaseOrder()
        {
            InitializeComponent();
            excludedObjects = new string[] { "toolStripPurchaseOrder" };

            TextBoxPurchaseOrderAddress.KeyDown += TextBoxPurchaseOrderAddress_KeyDown;
            ComboBoxPurchaseOrderInventoryLocation.Leave += ComboBoxPurchaseOrderInventoryLocation_Leave;
            DatetimePickerPurchaseOrderDate.PreviewKeyDown += DatetimePickerPurchaseOrderDate_PreviewKeyDown;
            YesNoRbtPurchaseOrderMethod.PreviewKeyDown += YesNoRbtPurchaseOrderMethod_PreviewKeyDown;
            TextBoxPurchaseOrderSupplier.PreviewKeyDown += TextBoxPurchaseOrderSupplier_PreviewKeyDown;
        }
        private void EnableForm(Boolean enable)
        {

            BtnPurchaseOrderDelete.Enabled = true;
            BtnPurchaseOrderSave.Enabled = true;
            BtnPurchaseOrderCreateSale.Enabled = true;
            YesNoRbtPurchaseOrderMethod.Enabled = true;
            TextBoxPurchaseOrderSupplier.ReadOnly = false;
            TextBoxPurchaseOrderSupplier.TabStop = true;
            TextBoxPurchaseMemo.ReadOnly = false;
            TextBoxPurchaseMemo.TabStop = true;
            DatetimePickerPurchaseOrderDate.ReadOnly = false;
            DatetimePickerPurchaseOrderDate.TabStop = true;
            TextBoxPurchaseOrderAddress.ReadOnly = false;
            TextBoxPurchaseOrderAddress.TabStop = true;
            ComboBoxPurchaseOrderInventoryLocation.Visible = true;
            BtnPurchaseOrderSearchSupplier.Enabled = true;
            BtnPurchaseOrderSearchSupplier.Enabled = true;
            GridViewPurchaseOrderItem.ReadOnly = false;
            GridViewPurchaseOrderItem.TabStop = true;
            GridViewPurchaseOrderItem.ScrollBars = ScrollBars.Vertical;

            if (enable)
            {
                BtnPurchaseOrderNew.Enabled = !enable;
                BtnPurchaseOrderDelete.Enabled = !enable;
                BtnPurchaseOrderPrint.Enabled = !enable;
                BtnPurchaseOrderCreateSale.Enabled = !enable;
                BtnPurchaseOrderCancel.Enabled = enable;
                BtnPurchaseOrderSave.Enabled = enable;
            }
            else
            {
                BtnPurchaseOrderNew.Enabled = !enable;
                BtnPurchaseOrderDelete.Enabled = !enable;
                BtnPurchaseOrderPrint.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? enable : !enable;
                BtnPurchaseOrderCreateSale.Enabled = !enable;
                BtnPurchaseOrderCancel.Enabled = !enable;
                BtnPurchaseOrderSave.Enabled = !enable;
            }
            PurchaseEntry Entry = GetSavedPurchaseOrder();
            if (Entry != null)
            {

                if (Entry.PurchaseEntrytype == PurchaseEntrytype.ORDER && Entry.isPurchaseEntryLocked)
                {
                    if (Entry.isPurchaseEntryLocked)
                    {
                        BtnPurchaseOrderDelete.Enabled = enable;
                    }
                    BtnPurchaseOrderSave.Enabled = enable;
                    YesNoRbtPurchaseOrderMethod.Enabled = enable;
                    ComboBoxPurchaseOrderInventoryLocation.Visible = enable;
                    DatetimePickerPurchaseOrderDate.ReadOnly = !enable;
                    DatetimePickerPurchaseOrderDate.TabStop = enable;
                    TextBoxPurchaseOrderSupplier.ReadOnly = !enable;
                    TextBoxPurchaseOrderSupplier.TabStop = enable;
                    TextBoxPurchaseOrderAddress.ReadOnly = !enable;
                    TextBoxPurchaseOrderAddress.TabStop = enable;
                    TextBoxPurchaseMemo.ReadOnly = !enable;
                    TextBoxPurchaseMemo.TabStop = enable;
                    BtnPurchaseOrderNew.Enabled = !enable;
                    BtnPurchaseOrderSearchSupplier.Enabled = enable;
                    GridViewPurchaseOrderItem.ReadOnly = !enable;
                    GridViewPurchaseOrderItem.TabStop = enable;
                }
            }
        }

        private void ResetForm()
        {
            PurchaseOrderReferenceNumber.Text = "000000";
            TextBoxPurchaseOrderSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorPurchaseOrder.Text = "";
            TextBoxPurchaseOrderId.ResetText();
            TextBoxPurchaseMemo.ResetText();
            TextBoxPurchaseOrderAddress.ResetText();
            DatetimePickerPurchaseOrderDate.Format = Global.Company.DateFormat;
            DatetimePickerPurchaseOrderDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            YesNoRadioPurchaseOrderType.Checked = (Global.Company.BusinessType == BuisnessType.Wholesale ? false : true);
            YesNoRbtPurchaseOrderMethod.Checked = true;
            PrevPurchaseOrderReferenceNumber.Text = CompanyManager.Instance.GetPurchasePrevRef(Global.Company, PurchaseEntrytype.ORDER, (DateTime)DatetimePickerPurchaseOrderDate.Date);
            ComboUtils.InitializeStockLocationCombo(ComboBoxPurchaseOrderInventoryLocation, Global.Company.CompanyId);
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App")!;
            ComboBoxPurchaseOrderInventoryLocation.SelectedIndex = key != null ? (key.GetValue("StockLocation") != null && !string.IsNullOrEmpty(key.GetValue("StockLocation")!.ToString())) ? ComboBoxPurchaseOrderInventoryLocation.FindStringExact(key.GetValue("StockLocation")!.ToString()) : -1 : -1;
            TextBoxPurchaseOrderSupplier.ResetText();
            GridViewPurchaseOrderItem.Rows.Clear();
            GridViewPurchaseOrderItem.Rows.Add();
            GridViewPurchaseOrderItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseOrderItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewPurchaseOrderItem.Columns["QuotePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewPurchaseOrderItem.Columns["QuoteAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewPurchaseOrderItemTotal.Columns["Value"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
        }
        private PurchaseEntry GetSavedPurchaseOrder()
        {
            PurchaseEntry PurchaseOrderEntry = null!;
            if (!string.IsNullOrEmpty(TextBoxPurchaseOrderId.Text))
            {
                PurchaseOrderEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(long.Parse(TextBoxPurchaseOrderId.Text));
            }
            return PurchaseOrderEntry;
        }
        private void FormPurchaseOrder_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
        }

        bool IsOverrideTabCtr = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2) && (GridViewPurchaseOrderItem.CurrentCell.ColumnIndex != (int)PurchaseOrderEntryTableColumn.PRODUCT) && ((GridViewPurchaseOrderItem.CurrentCell.ReadOnly) ? true : false))
            {
                BtnPurchaseOrderSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F2) && (GridViewPurchaseOrderItem.Focused && (GridViewPurchaseOrderItem.CurrentCell.ColumnIndex != (int)PurchaseOrderEntryTableColumn.PRODUCT) && ((GridViewPurchaseOrderItem.CurrentCell.ReadOnly) ? true : false)))
            {
                BtnPurchaseOrderSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F2) && (TextBoxPurchaseOrderSupplier.Focused || BtnPurchaseOrderSearchSupplier.Focused))
            {
                BtnPurchaseOrderSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnPurchaseOrderNew.Enabled)
                {
                    BtnPurchaseOrderNew_Click(this, null!);
                }
                else
                {
                    if (TextBoxPurchaseOrderSupplier.Focused)
                        BtnPurchaseOrderNewSupplier.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnPurchaseOrderDelete_Click(this, null!);
            }
            else if (keyData == (Keys.F9))
            {
                BtnPurchaseOrderPrint_Click(this, null!);
            }
            else if (keyData == (Keys.F8))
            {
                BtnPurchaseOrderSave_Click(this, null!);
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPurchaseOrderCancel_Click(this, null!);
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnPurchaseOrderExit_Click(this, null!);
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == BtnPurchaseOrderSearchSupplier)
            {
                TextBoxPurchaseOrderAddress.Select();
                return true;
            }
            else if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnPurchaseOrderSearchSupplier)
            {
                TextBoxPurchaseOrderSupplier.Select();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == BtnPurchaseOrderSave)
            {
                TextBoxPurchaseOrderSupplier.Select();
                return true;
            }
            else if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnPurchaseOrderSave)
            {
                GridViewPurchaseOrderItem.Select();
                return true;
            }
            try
            {
                int lastRowIndex = GridViewPurchaseOrderItem.Rows.Count - 1;

                if ((keyData == Keys.F2) && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRODUCT)
                {
                    SearchProduct();
                    return true;
                }
                if ((keyData == Keys.Tab) && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRICE)
                {
                    object idCellValue = GridViewPurchaseOrderItem.Rows[GridViewPurchaseOrderItem.CurrentRow.Index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value;

                    if (idCellValue == null || string.IsNullOrEmpty(idCellValue.ToString()))
                    {
                        BtnPurchaseOrderSave.Focus();
                        return true;
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }

                }
                if ((keyData == Keys.Tab) && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRODUCT)
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.QTY)
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRODUCT)
                {
                    if (GridViewPurchaseOrderItem.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    else
                    {
                        TextBoxPurchaseMemo.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FormPurchaseOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    YesNoRbtPurchaseOrderMethod.Focus();
                    e.Cancel = true;
                }
            }
        }
        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    IList<Product> Product = CatalogProductManager.Instance.GetProductByExactSearchQuery(((TextBox)sender).Text, Global.Company.CompanyId);
                    if (Product.Count > 0)
                    {
                        if (Product.Count > 1)
                        {
                            SearchProduct();
                            if (ProductId != 0)
                            {
                                DirtyFlag(IsDirty);
                                return;
                            }
                        }
                        long CheckForAddRow = (GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.ID].Value : 0L;
                        if (GridViewPurchaseOrderItem.Rows[GridViewPurchaseOrderItem.CurrentRow.Index].Cells[(int)PurchaseOrderEntryTableColumn.PURCHASEDETAILID].Value != null && CheckForAddRow == Product.First().Id)
                        {
                            DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (Result == DialogResult.No)
                            {
                                return;
                            }
                        }
                        LoadUomTax(Product.First().Id);
                        GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (CheckForAddRow == 0 && GridViewPurchaseOrderItem.Rows.Count - 1 == GridViewPurchaseOrderItem.CurrentRow.Index)
                        {
                            GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewPurchaseOrderItem.Rows.Add();
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewPurchaseOrderItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewPurchaseOrderItem.CurrentRow.Index);
                }
            }
        }
        private void UomColumnComboTextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1 && !string.IsNullOrEmpty(((ComboBox)sender).Text))
            {
                int index = ((ComboBox)sender).FindStringExact(((ComboBox)sender).Text);
                if (index != -1)
                {
                    DataGridViewComboBoxEditingControl ck = (DataGridViewComboBoxEditingControl)sender;
                    ck.SelectedIndex = index;
                    UomColumnComboSelectionChanged(ck, e);
                }
                else
                {
                    int Index = GridViewPurchaseOrderItem.CurrentCell.RowIndex;
                    GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }
        }
        private void UomColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewPurchaseOrderItem.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value == null)
                {
                    GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = 0.00;
                }
            }
        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    GridViewPurchaseOrderItem.Rows[Index].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                }
            }
        }
        private void SearchProduct()
        {
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.ID].Value : 0L;
            ProductId = 0L;
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            var item = GridViewPurchaseOrderItem.CurrentRow.Cells[(int)PurchaseOrderEntryTableColumn.PRODUCT].Value;
            FormSearchItems.SearchText = (item != null) ? (string)item : string.Empty;
            FormSearchItems.ShowDialog();
            if (ProductId != 0)
            {
                if (GridViewPurchaseOrderItem.Rows[GridViewPurchaseOrderItem.CurrentRow.Index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value != null && Check == ProductId)
                {
                    DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        return;
                    }
                }
                LoadUomTax(ProductId);
                if (Check == 0 && GridViewPurchaseOrderItem.Rows.Count - 1 == GridViewPurchaseOrderItem.CurrentRow.Index)
                {
                    GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewPurchaseOrderItem.Rows.Add();
                }
                GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[3, GridViewPurchaseOrderItem.CurrentRow.Index];
                GridViewPurchaseOrderItem.CurrentCell.Selected = true;
            }
            else
            {
                ProductId = Check;
                DirtyFlag(IsDirty);
                return;
            }
        }
        private void LoadUomTax(long ProductId)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
            if (Product != null)
            {
                int index = GridViewPurchaseOrderItem.CurrentRow.Index;
                (GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.UOM] as DataGridViewComboBoxCell)?.Items.Add(Product.RetailUOM);
                (GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.UOM] as DataGridViewComboBoxCell)?.Items.Add(Product.WholesaleUOM);
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value = Product.WholesaleUOM;
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.PRODUCT].Value = Product.Name;
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = 0.00;
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = Product.PurchasePrice;
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value = 0;
                GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value = Product.Id;

                TextBoxPurchaseOrderProductName.Text = Product.Name;
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.PRODUCT].Value = null;
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value = null;
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value = 0;
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = 0.00;
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = 0.00;
            GridViewPurchaseOrderItem.Rows[index].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value = null;

            TextBoxPurchaseOrderProductName.ResetText();
            ComputeFormTotal();
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewPurchaseOrderItem.Rows.Count - 1; i++)
            {
                double Quantity = (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value.ToString()!));
                double Pprice = (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value.ToString()!));

                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;
                Amount = (Pprice * Quantity);
                if (Amount > 0)
                {
                    GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = Amount;
                    TotalAmount = TotalAmount + Amount;
                }
                else
                {
                    GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = 0.00;
                }
            }
            GridViewPurchaseOrderItemTotal.Rows[0].Cells[(int)PurchaseOrderEntryTotalTableColumn.VALUE].Value = TotalAmount;
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void Row_Removed()
        {
            ReSequence();
            ComputeFormTotal();
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            return;
        }
        private void BtnPurchaseOrderSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxPurchaseOrderSupplier.Id == null ? 0L : long.Parse(TextBoxPurchaseOrderSupplier.Id);
            SupplierId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = false;
            FormSearchAccount.IncludeGeneralAccounts = false;
            FormSearchAccount.ShowDialog();
            if (SupplierId != 0)
            {
                Supplier Supplier = SupplierManager.Instance.GetSupplierById(SupplierId);
                if (Supplier != null)
                {
                    TextBoxPurchaseOrderSupplier.Text = Supplier.Name;
                    TextBoxPurchaseOrderSupplier.Id = SupplierId.ToString();
                    TextBoxPurchaseOrderAddress.Text = Supplier.Address.FullAddress.Replace("\n", System.Environment.NewLine);
                }
                else
                {
                    Customer customer = CustomerManager.Instance.GetCustomerById(SupplierId);
                    if (customer != null)
                    {
                        TextBoxPurchaseOrderSupplier.Text = customer.Name;
                        TextBoxPurchaseOrderSupplier.Id = SupplierId.ToString();
                        TextBoxPurchaseOrderAddress.Text = customer.BillingAddress.FullAddress.Replace("\n", System.Environment.NewLine);
                    }
                    else
                    {
                        SupplierId = (long)TempSupplierId;
                        MessageBox.Show("The selected account is not available anymore");
                        return;
                    }
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                }
            }
            TextBoxPurchaseOrderSupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void BtnPurchaseOrderewSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long? TempSupplierId = TextBoxPurchaseOrderSupplier.Id == null ? 0L : long.Parse(TextBoxPurchaseOrderSupplier.Id);
            SupplierId = 0L;
            FormSupplier FormCustomer = new FormSupplier(this);
            FormCustomer.CreateSupplierOnLoad = true;
            FormCustomer.ShowDialog(this);
            if (SupplierId != 0)
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(SupplierId);
                if (customer != null)
                {
                    TextBoxPurchaseOrderSupplier.Text = customer.Name;
                    TextBoxPurchaseOrderSupplier.Id = SupplierId.ToString();
                    TextBoxPurchaseOrderAddress.Text = customer.BillingAddress.FullAddress.Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    SupplierId = (long)TempSupplierId;
                    MessageBox.Show("The saved supplier is not available anymore");
                    return;
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                    DirtyFlag(IsDirty);
                }
            }
            TextBoxPurchaseOrderSupplier.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseOrderExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPurchaseOrderSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    PurchaseEntry PurchaseEntries = GetPurchaseOrderEntryFromForm();
                    PurchaseEntries.PurchaseEntrytype = PurchaseEntrytype.ORDER;
                    if (PurchaseEntries.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PURCHASE_ORDER, (DateTime)DatetimePickerPurchaseOrderDate.Date!);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            PurchaseEntries.RefNumber = RefNumber;
                            try
                            {
                                PurchaseEntryManager.Instance.AddPurchaseEntry(PurchaseEntries);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(RefNoErrorMsg);
                            return;
                        }
                    }
                    else
                    {
                        if (PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseEntries.Id) != null)
                        {
                            PurchaseEntryManager.Instance.UpdatePurchaseEntry(PurchaseEntries);
                        }
                        else
                        {
                            DisplaySystemError("Somthing went wrong, the selected purchase order is not valid.");
                            return;
                        }
                    }
                    if (PurchaseEntries != null)
                    {
                        TextBoxPurchaseOrderId.Text = PurchaseEntries.Id.ToString();
                        PurchaseOrderReferenceNumber.Text = PurchaseEntries.RefNumber;
                        LoadPurchaseOrderEntry(PurchaseEntries.Id);
                        BtnPurchaseOrderPrint.Select();

                    }
                    ToolStripStatusLabelErrorPurchaseOrder.Text = SaveSuccessText;
                    DirtyFlag(false);
                }
                finally { Cursor.Current = Cursors.Default; }
            }
        }
        private void LoadPurchaseOrderEntry(long PurchaseEntrieId)
        {
            ResetForm();
            PurchaseEntry PurchaseEntryEntrys = PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseEntrieId);
            if (PurchaseEntryEntrys != null)
            {
                PurchaseOrderReferenceNumber.Text = PurchaseEntryEntrys.RefNumber;
                TextBoxPurchaseOrderId.Text = PurchaseEntryEntrys.Id.ToString();
                if (PurchaseEntryEntrys.AccountId == null)
                {
                    TextBoxPurchaseOrderSupplier.Text = PurchaseEntryEntrys.SupplierName;
                    TextBoxPurchaseOrderAddress.Text = PurchaseEntryEntrys.SupplierAddress.Replace(",", "," + System.Environment.NewLine); ;
                }
                else
                {
                    TextBoxPurchaseOrderSupplier.Text = PurchaseEntryEntrys.Account.Name;
                    TextBoxPurchaseOrderSupplier.Id = PurchaseEntryEntrys.Account.Id.ToString();
                    TextBoxPurchaseOrderAddress.Text = PurchaseEntryEntrys.SupplierAddress;
                }
                DatetimePickerPurchaseOrderDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntryEntrys.RefDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtPurchaseOrderMethod.Checked = (PurchaseEntryEntrys.PurchaseMethod == PurchaseMethod.Credit) ? true : false;
                if (PurchaseEntryEntrys.InventoryLocationId != null && PurchaseEntryEntrys.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)PurchaseEntryEntrys.InventoryLocationId);
                    if (Location != null)
                    {
                        ComboBoxPurchaseOrderInventoryLocation.SelectedIndex = ComboBoxPurchaseOrderInventoryLocation.FindStringExact(Location.Name);
                    }
                }
                TextBoxPurchaseMemo.Text = !string.IsNullOrEmpty(PurchaseEntryEntrys.Memo) ? PurchaseEntryEntrys.Memo.Replace("\n", System.Environment.NewLine) : string.Empty;
                if (PurchaseEntryEntrys.PurchaseDetails.Count > 0)
                {
                    GridViewPurchaseOrderItem.Rows.Add(PurchaseEntryEntrys.PurchaseDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var PurchaseDetail in PurchaseEntryEntrys.PurchaseDetails)
                    {
                        PurchaseDetails lPurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(PurchaseDetail.Id);
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.SNO].Value = i + 1;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PRODUCT].Value = lPurchaseDetails.Product.Name;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value = lPurchaseDetails.Product.RetailUOM == lPurchaseDetails.WholesaleUOM ? lPurchaseDetails.Product.RetailUOM : lPurchaseDetails.Product.WholesaleUOM;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value = lPurchaseDetails.Quantity;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = lPurchaseDetails.PurchasePrice;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = lPurchaseDetails.Amount;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value = lPurchaseDetails.ProductId;
                        GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PURCHASEDETAILID].Value = lPurchaseDetails.Id;
                        i++;
                    }
                    Row_Added();
                    ReSequence();
                }
                if (PurchaseEntryEntrys.isPurchaseEntryLocked)
                {
                    BtnPurchaseOrderPrint.Select();
                }
                else
                {
                    TextBoxPurchaseOrderSupplier.Focus();
                    TextBoxPurchaseOrderSupplier.SelectAll();
                }
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                SearchPurchaseId = 0L;
                DisplaySystemError("The selected purchase order is not available anymore");
                return;
            }
        }

        private Boolean ValidateForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if ((YesNoRbtPurchaseOrderMethod.Checked) &&
                    (string.IsNullOrEmpty(TextBoxPurchaseOrderSupplier.Text.Trim()) ||
                    string.IsNullOrEmpty(TextBoxPurchaseOrderSupplier?.Id?.Trim()) ||
                    AccountManager.Instance.GetAccountById(long.Parse(TextBoxPurchaseOrderSupplier.Id)) == null))
                {
                    TextBoxPurchaseOrderSupplier!.Select();
                    ToolStripStatusLabelErrorPurchaseOrder.Text = ChooseSupplierErrorMsg;
                    ResetTimmer();
                    return false;
                }
                if ((YesNoRbtPurchaseOrderMethod.Checked) && (!string.IsNullOrEmpty(TextBoxPurchaseOrderSupplier.Text.Trim()) ||
                    !string.IsNullOrEmpty(TextBoxPurchaseOrderSupplier.Id.Trim())))
                {
                    Supplier Supplier = null!;
                    Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxPurchaseOrderSupplier.Id));
                    if (Customer == null)
                    {
                        Supplier = SupplierManager.Instance.GetSupplierById(long.Parse(TextBoxPurchaseOrderSupplier.Id));
                    }
                    if ((Customer != null && Global.Company.CompanyCustomerLicenseMaster.Count > 0 && Customer.CustomerLicenceDetail.Count == 0)
                            || (Supplier != null && Global.Company.CompanySupplierLicenseMaster.Count > 0 && Supplier.SupplierLicenceDetail.Count == 0))
                    {
                        TextBoxPurchaseOrderSupplier.Select();
                        ToolStripStatusLabelErrorPurchaseOrder.Text = UpdateCustomerLicenceErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                }
                if (DatetimePickerPurchaseOrderDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerPurchaseOrderDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    DatetimePickerPurchaseOrderDate.Focus();
                    ToolStripStatusLabelErrorPurchaseOrder.Text = EnterPurchaseOrderEntryDateErrorMsg;
                    ResetTimmer();
                    return false;
                }
                if (ComboBoxPurchaseOrderInventoryLocation.SelectedIndex < 0)
                {
                    ComboBoxPurchaseOrderInventoryLocation.Select();
                    ToolStripStatusLabelErrorPurchaseOrder.Text = SelectLocationErrMsg;
                    ResetTimmer();
                    return false;
                }
                int Count = GridViewPurchaseOrderItem.Rows.Count;
                if (Count > 1)
                {
                    for (int i = 0; i < Count - 1; i++)
                    {
                        if (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value == null)
                        {
                            GridViewPurchaseOrderItem.Select();
                            GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[(int)PurchaseOrderEntryTableColumn.PRODUCT, i];
                            GridViewPurchaseOrderItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchaseOrder.Text = Grid_ChooseItemErrorMsg;
                            ResetTimmer();
                            return false;
                        }
                        if (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value == null
                            || string.IsNullOrEmpty(GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value.ToString())
                            )
                        {
                            GridViewPurchaseOrderItem.Select();
                            GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[(int)PurchaseOrderEntryTableColumn.UOM, i];
                            GridViewPurchaseOrderItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchaseOrder.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, "select", GridViewPurchaseOrderItem.Columns[(int)PurchaseOrderEntryTableColumn.UOM].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        double Quantity = (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value.ToString()!);

                        for (int j = 3; j < 6; j++)
                        {
                            if (j != (int)PurchaseOrderEntryTableColumn.AMOUNT && (GridViewPurchaseOrderItem.Rows[i].Cells[j].Value == null || GridViewPurchaseOrderItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewPurchaseOrderItem.Rows[i].Cells[j].Value.ToString()!) <= 0))
                            {
                                GridViewPurchaseOrderItem.Select();
                                GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[j, i];

                                GridViewPurchaseOrderItem.BeginEdit(true);
                                ToolStripStatusLabelErrorPurchaseOrder.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, "enter", GridViewPurchaseOrderItem.Columns[j].HeaderText);
                                ResetTimmer();
                                return false;
                            }

                        }
                    }
                }
                else
                {
                    GridViewPurchaseOrderItem.Select();
                    GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[(int)PurchaseOrderEntryTableColumn.PRODUCT, 0];
                    GridViewPurchaseOrderItem.BeginEdit(true);
                    ToolStripStatusLabelErrorPurchaseOrder.Text = Grid_EmptyErrorMsg;
                    ResetTimmer();
                    return false;
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return true;
        }
        public PurchaseEntry GetPurchaseOrderEntryFromForm()
        {
            PurchaseEntry lPurchaseOrderEntries = new PurchaseEntry();
            lPurchaseOrderEntries.Id = TextBoxPurchaseOrderId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxPurchaseOrderId.Text);
            lPurchaseOrderEntries.RefNumber = PurchaseOrderReferenceNumber.Text;
            if (TextBoxPurchaseOrderSupplier.Id == null)
            {
                lPurchaseOrderEntries.SupplierName = TextBoxPurchaseOrderSupplier.Text;
            }
            else
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxPurchaseOrderSupplier.Id));
                if (Customer != null)
                {
                    lPurchaseOrderEntries.AccountId = Customer.Id;
                    lPurchaseOrderEntries.SupplierName = Customer.Name;
                    lPurchaseOrderEntries.SupplierAddress = Customer.BillingAddress.FullAddress;
                }
                else
                {
                    Supplier supplier = SupplierManager.Instance.GetSupplierById(long.Parse(TextBoxPurchaseOrderSupplier.Id));
                    if (supplier != null)
                    {
                        lPurchaseOrderEntries.AccountId = supplier.Id;
                        lPurchaseOrderEntries.SupplierName = supplier.Name;
                        lPurchaseOrderEntries.SupplierAddress = supplier.Address.FullAddress;
                    }
                }
            }
            if (ComboBoxPurchaseOrderInventoryLocation.SelectedIndex > -1)
            {
                lPurchaseOrderEntries.InventoryLocationId = ((InventoryLocation)ComboBoxPurchaseOrderInventoryLocation.Items[ComboBoxPurchaseOrderInventoryLocation.SelectedIndex]).Id;
            }
            lPurchaseOrderEntries.Memo = TextBoxPurchaseMemo.Text.Replace("\r", "").Replace(",", "").Replace("\n", ",");
            lPurchaseOrderEntries.SupplierAddress = TextBoxPurchaseOrderAddress.Text;
            lPurchaseOrderEntries.PurchaseMethod = (YesNoRbtPurchaseOrderMethod.Checked) ? PurchaseMethod.Credit : PurchaseMethod.Cash;
            if (DatetimePickerPurchaseOrderDate.Date != null)
            {
                lPurchaseOrderEntries.RefDate = (DateTime)DatetimePickerPurchaseOrderDate.Date;
            }
            lPurchaseOrderEntries.CompanyId = Global.Company.CompanyId;
            lPurchaseOrderEntries.ReturnDate = DateTime.Now.Date;
            lPurchaseOrderEntries.NetAmount = Convert.ToDouble(GridViewPurchaseOrderItemTotal.Rows[0].Cells[(int)PurchaseOrderEntryTotalTableColumn.VALUE].Value);
            if (Global.CostCenter != null)
            {
                lPurchaseOrderEntries.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < GridViewPurchaseOrderItem.Rows.Count - 1; i++)
            {
                PurchaseDetails PurchaseDetail = new PurchaseDetails();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    PurchaseDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        PurchaseDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    PurchaseDetail.Id = (GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PURCHASEDETAILID].Value == null) ? 0L : long.Parse(GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PURCHASEDETAILID].Value.ToString()!);
                    PurchaseDetail.ProductId = Product.Id;
                    PurchaseDetail.isBatch = Product.isInventoryAtBatch == null ? false : (bool)Product.isInventoryAtBatch;
                    PurchaseDetail.MaterialId = Product.MaterialId;
                    PurchaseDetail.WholesaleUOM = GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.UOM].Value.ToString();
                    PurchaseDetail.ExpDate = DateTime.Now.Date;
                    var Quantity = GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value;
                    var Pprice = GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value;
                    var Amount = GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value;
                    if (Quantity != null && Pprice != null && Amount != null)
                    {
                        PurchaseDetail.Quantity = double.Parse(Quantity.ToString()!);
                        PurchaseDetail.PurchasePrice = float.Parse(Pprice.ToString()!);
                        PurchaseDetail.Amount = float.Parse(Amount.ToString()!);
                    }
                }
                lPurchaseOrderEntries.PurchaseDetails.Add(PurchaseDetail);
            }
            return lPurchaseOrderEntries;
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerPurchaseOrder.Stop();
            TimerPurchaseOrder.Start();
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewPurchaseOrderItem.Rows.Count; i++)
            {
                GridViewPurchaseOrderItem.Rows[i].Cells[(int)PurchaseOrderEntryTableColumn.SNO].Value = i + 1;
            }
        }
        private void GridViewPurchaseOrderItem_KeyPressNew(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewPurchaseOrderItem.EditingControl).DroppedDown = false;
        }
        private void GridViewPurchaseOrderItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewPurchaseOrderItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewPurchaseOrderItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged!);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged!);
                ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged!;
                ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged!;
                e.Control.KeyPress += new KeyPressEventHandler(GridViewPurchaseOrderItem_KeyPressNew!);
            }
            if (GridViewPurchaseOrderItem.CurrentCell.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange!;
                ((TextBox)e.Control).TextChanged += ProductTextChange!;
            }
        }

        private void GridViewPurchaseOrderItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)PurchaseOrderEntryTableColumn.QTY || e.ColumnIndex == (int)PurchaseOrderEntryTableColumn.PRICE)
            {
                Row_Added();
            }
        }

        private void GridViewPurchaseOrderItem_Leave(object sender, EventArgs e)
        {
            GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[(int)PurchaseOrderEntryTableColumn.PRICE, GridViewPurchaseOrderItem.CurrentRow.Index];
        }

        private void GridViewPurchaseOrderItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewPurchaseOrderItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                DirtyFlag(IsDirty);
            }
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.SNO].ReadOnly = true;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.UOM].ReadOnly = true;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.QTY].ReadOnly = true;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].ReadOnly = true;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.ID].Value != null)
            {
                //GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.UOM].ReadOnly = false;
                GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.QTY].ReadOnly = false;
                GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].ReadOnly = false;
            }
        }

        private void GridViewPurchaseOrderItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.SNO].Value = GridViewPurchaseOrderItem.Rows.Count;
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.PRICE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DirtyFlag(IsDirty);
        }

        private void BtnPurchaseOrderCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DirtyFlag(false);
            TextBoxPurchaseOrderSupplier.Select();
            ComboBoxPurchaseOrderInventoryLocation.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorPurchaseOrder.Text = "";
            if (string.IsNullOrEmpty(TextBoxPurchaseOrderSearch.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchaseOrder.Text = SearchBoxEmptyErrorMsg;
                TextBoxPurchaseOrderSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private void RecentPurchase()
        {
            ToolStripStatusLabelErrorPurchaseOrder.Text = "";
            String SearchText = TextBoxPurchaseOrderSearch.Text.Trim();
            IList<PurchaseEntry> PurchaseInfo = null!;
            if (string.IsNullOrEmpty(SearchText))
            {
                PurchaseInfo = PurchaseEntryManager.Instance.GetRecentPurchaseEntrys(Global.Company.CompanyId, PurchaseEntrytype.ORDER);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                PurchaseInfo = PurchaseEntryManager.Instance.GetPurchaseOrderByAmountRefNo(Amount, SearchText, Global.Company.CompanyId, PurchaseEntrytype.ORDER);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                if (Date.HasValue)
                {
                    DateTime actualDate = Date.Value;
                    PurchaseInfo = PurchaseEntryManager.Instance.GetPurchaseEntryByDate((DateTime)actualDate, Global.Company.CompanyId, PurchaseEntrytype.ORDER);
                }
            }
            else
            {
                PurchaseInfo = PurchaseEntryManager.Instance.GetPurchaseEntryBySupplierName(SearchText, Global.Company.CompanyId, PurchaseEntrytype.ORDER);
            }
            if (PurchaseInfo != null && PurchaseInfo.Count > 0)
            {
                LoadPurchaseOrderEntry(PurchaseInfo);
                EnableForm(false);
            }
            else
            {
                SearchPurchaseId = 0L;
                ToolStripStatusLabelErrorPurchaseOrder.Text = PurchaseOrderSearchOutput;
            }
        }
        public void LoadPurchaseOrderEntry(IList<PurchaseEntry> PurchaseEntryInfo)
        {
            if (PurchaseEntryInfo.Count > 0)
            {
                SearchPurchaseId = 0L;
                FormRecentPurchase FormRecentPurchase = new FormRecentPurchase(this);
                FormRecentPurchase.Text = "Recent Order";
                FormRecentPurchase.PurchaseEntryInfo = PurchaseEntryInfo;
                FormRecentPurchase.ShowDialog();

            }
            else
            {
                ToolStripStatusLabelErrorPurchaseOrder.Text = PurchaseOrderSearchOutput;
            }
        }
        private double RoundOff(double TotalAmount)
        {
            double _roundoff = 0.00;
            double mod = TotalAmount % 1;
            if (mod > 0.50)
            {
                _roundoff = 1 - mod;
            }
            else
            {
                _roundoff = -mod;
            }
            return Math.Round(_roundoff, 2);
        }
        private void BtnPurchaseOrderSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                RecentPurchase();
                if (SearchPurchaseId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnPurchaseOrderSave_Click(sender, e);
                                LoadPurchaseOrderEntry(SearchPurchaseId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadPurchaseOrderEntry(SearchPurchaseId);
                        }
                    }
                    else
                    {
                        LoadPurchaseOrderEntry(SearchPurchaseId);
                    }
                }
            }
        }

        private void BtnPurchaseOrderNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnPurchaseOrderSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxPurchaseOrderSupplier.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            TextBoxPurchaseOrderSupplier.Focus();
            DirtyFlag(false);
            ComboBoxPurchaseOrderInventoryLocation.Text = string.Empty;
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseOrderCreateSale_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPurchaseEntryNew FormPurchaseEntryNew = new FormPurchaseEntryNew();
            FormPurchaseEntryNew.PurchaseOrderOnLoad = true;
            FormPurchaseEntryNew.SearchPurchaseId = long.Parse(TextBoxPurchaseOrderId.Text);
            FormPurchaseEntryNew.ShowDialog();
            LoadPurchaseOrderEntry(long.Parse(TextBoxPurchaseOrderId.Text));
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseOrderDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPurchaseOrderId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this purchase order is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PurchaseOrderReferenceNumber.Text), "Delete Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long PurchaseId = Convert.ToInt64(TextBoxPurchaseOrderId.Text);
                PurchaseEntry SaleEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseId);
                if (SaleEntry != null)
                {
                    bool DeleteResult = PurchaseEntryManager.Instance.DeletePurchaseEntry(PurchaseId);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        YesNoRbtPurchaseOrderMethod.Focus();
                        DirtyFlag(false);
                    }
                    else
                    {
                        MessageBox.Show(DeleteErrorText);
                    }
                }
                else
                {
                    DisplaySystemError("Somthing went wrong, the selected quotes is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void TextBox_Enter(object? sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectionStart = 0;
            }
        }

        private void TextBoxPurchaseOrderAddress_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (sender != null)
                {
                    SelectNextControl((Control)sender, true, true, true, true);
                }
            }
        }

        private void BtnPurchaseOrderPrint_Click(object sender, EventArgs e)
        {
            if (long.TryParse(TextBoxPurchaseOrderId.Text, out long purchaseOrderId))
            {
                PurchaseEntry PurchaseOrderEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(purchaseOrderId);
                if (PurchaseOrderEntry != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PrinterSetup.PurchaseOrderPrintSetup(long.Parse(TextBoxPurchaseOrderId.Text), false);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    DisplaySystemError("Somting went wrong, please check this purchase is still valid.");
                    return;
                }
            }
        }

        private void TextBoxPurchaseOrderSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnPurchaseOrderSearch_Click(sender, e);
            }
        }

        private void TextBoxPurchaseOrderSearch_Leave(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPurchaseOrder.Text = "";
        }

        private void ComboBoxPurchaseOrderInventoryLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab)
            {
                TextBoxPurchaseMemo.Select();
            }
            if (e.KeyCode == Keys.Escape)
            {
                BtnPurchaseOrderCancel_Click(this, null!);
            }
            if (e.KeyCode == Keys.F3)
            {
                if (BtnPurchaseOrderNew.Enabled)
                {
                    BtnPurchaseOrderNew_Click(this, null!);
                }
                else
                {
                    if (TextBoxPurchaseOrderSupplier.Focused)
                        BtnPurchaseOrderNewSupplier.ShowDropDown();
                }
            }
            if (e.KeyCode == Keys.F4)
            {
                BtnPurchaseOrderDelete_Click(this, null!);
            }
            if (e.KeyCode == Keys.F8)
            {
                BtnPurchaseOrderSave_Click(this, null!);
            }
            if (e.KeyCode == Keys.F9)
            {
                BtnPurchaseOrderPrint_Click(this, null!);
            }
            if (e.KeyCode == (Keys.F10))
            {
                BtnPurchaseOrderExit_Click(this, null!);
            }
        }
        private void TextBoxPurchaseMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxPurchaseMemo.SelectionLength = 0;
                e.IsInputKey = true;
                if (GridViewPurchaseOrderItem.Rows.Count > 0 && GridViewPurchaseOrderItem.Columns.Count > 1)
                {
                    int targetRowIndex = 0;
                    int targetColIndex = 1;
                    GridViewPurchaseOrderItem.Select();
                    GridViewPurchaseOrderItem.CurrentCell = GridViewPurchaseOrderItem[targetColIndex, targetRowIndex];
                }
            }
        }
        private void BtnPurchaseOrderNewSupplier_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxPurchaseOrderSupplier.Id == null ? 0L : long.Parse(TextBoxPurchaseOrderSupplier.Id);
            SupplierId = 0L;
            if (e.ClickedItem.Text == "Supplier")
            {
                FormSupplier FormSupplier = new FormSupplier(this);
                FormSupplier.CreateSupplierOnLoad = true;
                FormSupplier.ShowDialog(this);
            }
            else if (e.ClickedItem.Text == "Customer")
            {
                FormCustomers FormCustomers = new FormCustomers(this);
                FormCustomers.CreateCustomerOnLoad = true;
                FormCustomers.ShowDialog(this);
            }
            if (SupplierId != 0)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(SupplierId);
                if (Customer != null)
                {
                    TextBoxPurchaseOrderSupplier.Text = Customer.Name;
                    TextBoxPurchaseOrderSupplier.Id = SupplierId.ToString();
                    TextBoxPurchaseOrderAddress.Text = Customer.BillingAddress.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById(SupplierId);
                    if (Supplier != null)
                    {
                        TextBoxPurchaseOrderSupplier.Text = Supplier.Name;
                        TextBoxPurchaseOrderSupplier.Id = SupplierId.ToString();
                        TextBoxPurchaseOrderAddress.Text = Supplier.Address.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    }
                    else
                    {
                        SupplierId = (long)TempSupplierId;
                        MessageBox.Show("Somting went wrong, please check this account is still valid.");
                        return;
                    }
                }
            }
            else
            {
                if (TempSupplierId != null)
                {
                    SupplierId = (long)TempSupplierId;
                    this.formIsDirty = false;
                }
            }
            TextBoxPurchaseOrderSupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }
        protected override void ProductIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductIdTransport.Text))
            {
                ProductId = long.Parse(ProductIdTransport.Text);
            }
        }

        private void TextBoxPurchaseOrderSupplier_TextChanged(object sender, EventArgs e)
        {
            TextBoxPurchaseOrderSupplier.Id = null!;
        }
        private void ComboBoxPurchaseOrderInventoryLocation_Leave(object? sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                YesNoRbtPurchaseOrderMethod.Focus();
            }
        }

        private void YesNoRbtPurchaseOrderMethod_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                ComboBoxPurchaseOrderInventoryLocation.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift)
            {
                DatetimePickerPurchaseOrderDate.maskedTextBox.Focus();
                DatetimePickerPurchaseOrderDate.maskedTextBox.SelectAll();
                e.IsInputKey = true;
            }
        }

        private void DatetimePickerPurchaseOrderDate_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                YesNoRbtPurchaseOrderMethod.Focus();
                e.IsInputKey = true;
            }
            else if (e.KeyCode == Keys.Tab && e.Shift)
            {
                TextBoxPurchaseOrderAddress.Focus();
                e.IsInputKey = true;
            }
        }

        private void TextBoxPurchaseOrderAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                DatetimePickerPurchaseOrderDate.maskedTextBox.Focus();
                DatetimePickerPurchaseOrderDate.maskedTextBox.SelectAll();
                e.IsInputKey = true;
            }
        }

        private void TextBoxPurchaseOrderAddress_Leave(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                BtnPurchaseOrderSearchSupplier.Focus();
            }
        }

        private void TextBoxPurchaseOrderSupplier_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                e.IsInputKey = true;
                BtnPurchaseOrderSearchSupplier.Focus();
            }
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                e.IsInputKey = true;
                BtnPurchaseOrderSave.Focus();
            }
        }

        private void BtnPurchaseOrderSearchSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && e.KeyCode != Keys.Shift)
            {
                TextBoxPurchaseOrderAddress.Focus();
            }
            if (e.KeyCode == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxPurchaseOrderSupplier.Focus();
            }
        }
        private void TextBoxPurchaseMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                TextBoxPurchaseMemo.SelectionLength = 0;
            }
        }

        private void GridViewPurchaseOrderItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)PurchaseOrderEntryTableColumn.REMOVE && (GridViewPurchaseOrderItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(ConfirmRowDeleteText, GridViewPurchaseOrderItem.Rows[e.RowIndex].Cells[(int)PurchaseOrderEntryTableColumn.SNO].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewPurchaseOrderItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewPurchaseOrderItem.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }
    }
}
