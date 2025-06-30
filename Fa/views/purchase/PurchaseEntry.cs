using fa.api.Accounting;
using fa.api.catalog;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.account.masters;
using fa.views.catalog;
using fa.views.utils;
using fa.views.common;
using fa.views.controls.accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using fa.api.Hms;
using fa.views.controls.grid;
using fa.views;
using Fa.views.catalog;

namespace fa.views.purchase
{
    public enum PurchaseEntryTableColumn
    {
        SNO,PRODUCT,UOM,QTY,FREE,BATNO,EXPDATE,PRICE,COST,TAXP,TAX,DISP,DIS,AMOUNT,REMOVE,ID,ISBAT,PurchDetailID,BATCHID,RETAIL,WHOLESALE,MSRP,RUOM,RXFACT,WUOM,WXFACT
    }
    public enum PurchaseEntryTotalTableColumn
    {
       NAME,VALUE
    }
    public partial class FormPurchaseEntry : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteConfirmText = "Do you want to delete purchase Entry {0}?";
        public static string DeleteErrorText = "Error in purchase Deleting. !";
        public static string NotAllowDeleteErrorText = "Do not Delete Purchase it used in payment";
        public static string DeleteValidationErrorText = "Do Not Delete Purchase Entry Some of the Items are Sold out";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string SelectInventoryLoactionErrorMsg = "Please select inventory location.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string ChooseSupplierErrorMsg = "Please select supplier.";
        public static string EnterPurchaseEntryDateErrorMsg = "Please enter valid Purchase date.";
        public static string EnterInvoiceDateErrorMsg = "Please enter valid invoice date.";
        public static string EnterInvoiceNumberErrorMsg = "Please enter invoice Number.";
        public static string UniqueInvoiceNumberErrorMsg = "Invoice Number {0} already exists for supplier {1} ,Check the Invoice Number";

        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter Purchase Items details.";
        public static string Grid_RowDeleteErrorMsg = "Do Not Delete Row {0} Item Its Sold out";
        public static string Grid_RowEditErrorMsg = "Do Not Modify Row {0} Item Its Sold out";
        public static string Grid_ItemInvalidQuantityErrorMsg = "please Enter valid Quantity";

        public static string InvalidPurchaseEntryErrorMsg = "Invalid Purchase.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could supplier name or purchase date or purchase reference number.";
        public static string PurchaseSearchOutput = "No Purchase found.";

        public static string EnterDefaultDisErrorMsg = "Please enter valid discount.";
        public static string EnterProductNameErrorMsg = "Please enter product name.";
        public static string EnterRUomErrorMsg = "Please enter retail UOM.";
        public static string EnterWUomErrorMsg = "Please enter whole sale UOM.";
        public static string EnterWXfactorErrorMsg = "Please enter whole sale xfactor.";
        public static string EnterRXfactorErrorMsg = "Please enter retail xfactor.";
        public static string UniqueProductNameErrorMsg = "Product name {0} already exists.";
        public static string EnterMrpErrorMsg = "Please Enter Correct Msrp.";


        PurchaseEntryManager PurchaseEntryManager = null;
        public long ProductId = 0L;
        public long BatchId;
        public long SupplierId = 0L;
        public long SearchPurchaseId = 0L;
        public long LocationId = 0L;

        public FormPurchaseEntry()
        {
            InitializeComponent();
            PurchaseEntryManager = PurchaseEntryManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "groupBox2", "DiscountAdditinalChargeGrid" };
        }
        private void FormPurchaseEntry_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                LoadUomTax(0L);
                this.Visible = false;
                setSize();
                this.Visible = true;
                ResetForm();
                EnableForm(true);
                LoadProductCombo();
                YesNoRbtPurchaseMethod.Focus();
                DirtyFlag(false);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void setSize()
        {
            //Currently this get the resultion of primary screen, it should actually get the screen where the application and then adjust the size
            var _ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var _ScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            if (_ScreenWidth < 1350)
            {
                this.Width = 1104;
            }
        }

        private PurchaseEntry GetPurchaseEntryFromForm()
        {
            PurchaseEntry lPurchaseEntry = new PurchaseEntry();
            lPurchaseEntry.Id = TextBoxPurchaseId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxPurchaseId.Text);
            lPurchaseEntry.PurchaseEntrytype = PurchaseEntrytype.PURCHASE;
            lPurchaseEntry.RefNumber = PurchaseReferenceNumber.Text;
            if (TextBoxPurchaseEntrySupplier.Id == null)
            {
                lPurchaseEntry.SupplierName = TextBoxPurchaseEntrySupplier.Text;
            }
            else
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxPurchaseEntrySupplier.Id));
                if (Customer != null)
                {
                    lPurchaseEntry.AccountId = Customer.Id;
                    lPurchaseEntry.SupplierName = TextBoxPurchaseEntrySupplier.Text;
                }
                else
                {
                    Supplier supplier = SupplierManager.Instance.GetSupplierById(long.Parse(TextBoxPurchaseEntrySupplier.Id));
                    if (supplier != null)
                    {
                        lPurchaseEntry.AccountId = supplier.Id;
                        lPurchaseEntry.SupplierName = TextBoxPurchaseEntrySupplier.Text;
                    }
                }                
            }
            if (ComboBoxPurchaseEntryInventoryLocation.SelectedIndex > -1)
            {
                lPurchaseEntry.InventoryLocationId = ((InventoryLocation)ComboBoxPurchaseEntryInventoryLocation.Items[ComboBoxPurchaseEntryInventoryLocation.SelectedIndex]).Id;
            }
            lPurchaseEntry.SupplierAddress = TextBoxPurchaseEntryAddress.Text;
            lPurchaseEntry.PurchaseMethod = (YesNoRbtPurchaseMethod.Checked) ? PurchaseMethod.Credit : PurchaseMethod.Cash;
            lPurchaseEntry.RefDate = (DateTime)DatetimePickerPurchaseDate.Date;
            lPurchaseEntry.PurchaseInvNumber = TextBoxPurcahaseInvoiceNo.Text;
            lPurchaseEntry.PurchaseInvDate = (DateTime)DatetimePickerPurchaseInvoiceDate.Date;
            lPurchaseEntry.CompanyId = Global.Company.CompanyId;
            if (Global.CostCenter != null)
            {
                lPurchaseEntry.CostCenterId = Global.CostCenter.CostCenterId;
            }
            double TotalAmount = double.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value.ToString());
            double TotalTaxAmount = 0;
            float TotalDiscountPercentage = 0;
            float TotalDiscountAmount = 0;


            float[] TaxWisePercentageTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];


            for (int i = 0; i < GridViewPurchaseItem.Rows.Count - 1; i++)
            {
                PurchaseDetails PurchaseDetails = new PurchaseDetails();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    PurchaseDetails.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        PurchaseDetails.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    PurchaseDetails.Id = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value == null) ? 0L : long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value.ToString());
                    PurchaseDetails.ProductId = Product.Id;
                    PurchaseDetails.MaterialId = Product.MaterialId;
                    PurchaseDetails.isBatch = (bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value;
                    //FOR SQL
                    PurchaseDetails.ExpDate = DateTime.Now.Date;
                    if (PurchaseDetails.isBatch)
                    {
                        PurchaseDetails.BatchNo = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString();
                        PurchaseDetails.ExpDate = (DateTime)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value;
                    }

                    double Quantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value.ToString());
                    double FreeQuantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value.ToString());
                    float Pprice = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value.ToString());
                    if (FreeQuantity > 0)
                    {
                        PurchaseDetails.isFree = true;
                    }
                    else
                    {
                        PurchaseDetails.isFree = false;
                    }
                    PurchaseDetails.Quantity = Quantity;
                    PurchaseDetails.FreeQuantity = FreeQuantity;
                    PurchaseDetails.PurchasePrice = Pprice;
                    PurchaseDetails.PurchaseCost = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.COST].Value.ToString());
                    PurchaseDetails.Retailprice = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value.ToString());
                    PurchaseDetails.Wholesaleprice = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value.ToString());
                    PurchaseDetails.Msrp = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value.ToString());

                    PurchaseDetails.RetailUOM = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value.ToString();
                    PurchaseDetails.RetailXFactor = int.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value.ToString());
                    PurchaseDetails.WholesaleUOM = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value.ToString();
                    PurchaseDetails.WholesaleXFactor = int.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value.ToString());

                    PurchaseDetails.Amount = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value.ToString());

                    double Amount = Quantity * Pprice;

                    PurchaseDetails.Discounts = new List<LineLevelPurchaseDiscount>();
                    float Discount = 0;
                    Object DiscountColumnValue = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value;
                    if (DiscountColumnValue != null)
                    {
                        Discount = float.Parse(DiscountColumnValue.ToString());
                        if (Discount > 0)
                        {
                            TotalDiscountPercentage = TotalDiscountPercentage + Discount;
                            float DiscountAmount = (float)Amount * (Discount / 100);
                            TotalDiscountAmount = TotalDiscountAmount + DiscountAmount;
                            LineLevelPurchaseDiscount ItemPurchaseDiscount = new LineLevelPurchaseDiscount();
                            ItemPurchaseDiscount.DisccountType = DiscountType.PERCENT;
                            ItemPurchaseDiscount.Discount = Discount;
                            ItemPurchaseDiscount.DiscountAmount = DiscountAmount;
                            ItemPurchaseDiscount.DiscountSequence = 1;
                            PurchaseDetails.Discounts.Add(ItemPurchaseDiscount);

                            Amount = Amount - DiscountAmount;
                        }
                    }

                    PurchaseDetails.TaxDetails = new List<LineLevelPurchaseTaxDetail>();
                    float TaxPercentage = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value.ToString());
                    if (TaxPercentage > 0)
                    {
                        int j = 0;

                        //for tax update purchase                        
                        if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null)
                        {
                            PurchaseDetails PurchaseDetail = PurchaseEntryManager.GetPurchaseDetail((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value);
                            if (PurchaseDetail.TaxDetails.Count > 0)
                            {
                                foreach (LineLevelPurchaseTaxDetail PurchaseTaxDetail in PurchaseDetail.TaxDetails)
                                {
                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PurchaseTaxDetail.TaxRate;
                                    float TaxAmount = (float)Amount * (PurchaseTaxDetail.TaxRate / 100);
                                    TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                    PurchaseTaxDetail.Amount = TaxAmount;
                                    PurchaseTaxDetail.Id = 0L;
                                    PurchaseTaxDetail.PurchaseDetails = null;
                                    PurchaseDetails.TaxDetails.Add(PurchaseTaxDetail);
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            foreach (CatalogItemSalesTaxMap PTaxMap in Product.SalesTax)
                            {
                                if (PTaxMap.TaxPercentage > 0)
                                {
                                    if (PTaxMap.EffectiveFrom <= Global.getTransactionDate() && PTaxMap.EffectiveTo >= Global.getTransactionDate())
                                    {
                                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId);
                                        if (CMap != null)
                                        {
                                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, Global.getTransactionDate(), (string.IsNullOrEmpty(TextBoxPurchaseEntrySupplier.Id) ? 0L : long.Parse(TextBoxPurchaseEntrySupplier.Id))))
                                            {
                                                TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                float TaxAmount = (float)Amount * (PTaxMap.TaxPercentage / 100);
                                                TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                LineLevelPurchaseTaxDetail ItemPurchaseTaxDetail = new LineLevelPurchaseTaxDetail();
                                                ItemPurchaseTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId).AccountId;
                                                ItemPurchaseTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                ItemPurchaseTaxDetail.Amount = TaxAmount;
                                                ItemPurchaseTaxDetail.TaxSequence = j + 1;
                                                ItemPurchaseTaxDetail.ItemTaxMapId = PTaxMap.Id;
                                                PurchaseDetails.TaxDetails.Add(ItemPurchaseTaxDetail);
                                            }
                                        }
                                    }
                                    j++;
                                }
                            }
                        }
                    }

                    lPurchaseEntry.PurchaseDetails.Add(PurchaseDetails);
                }
            }

            lPurchaseEntry.TaxDetails = new List<OrderLevelPurchaseTaxDetail>();
            int k = 0;
            foreach (CompanySalesTaxAccountMap TaxMap in Global.Company.SalesTaxAccountMaps)
            {
                if (TaxWisePercentageTotals[k] > 0)
                {
                    TotalTaxAmount = TotalTaxAmount + TaxWiseAmountTotals[k];
                    OrderLevelPurchaseTaxDetail PurchaseTaxDetail = new OrderLevelPurchaseTaxDetail();
                    PurchaseTaxDetail.TaxAccountId = TaxMap.AccountId;
                    PurchaseTaxDetail.TaxRate = TaxWisePercentageTotals[k];
                    PurchaseTaxDetail.Amount = TaxWiseAmountTotals[k];
                    PurchaseTaxDetail.TaxSequence = k + 1;
                    lPurchaseEntry.TaxDetails.Add(PurchaseTaxDetail);
                }
                k++;
            }
            lPurchaseEntry.Discounts = new List<OrderLevelPurchaseDiscount>();
            int l = 1;
            if (TotalDiscountPercentage > 0)
            {
                OrderLevelPurchaseDiscount PurchaseDiscount = new OrderLevelPurchaseDiscount();
                PurchaseDiscount.DisccountType = DiscountType.PERCENT;
                PurchaseDiscount.Discount = TotalDiscountPercentage;
                PurchaseDiscount.DiscountAmount = TotalDiscountAmount;
                PurchaseDiscount.DiscountSequence = l;
                lPurchaseEntry.Discounts.Add(PurchaseDiscount);
            }
            if (DiscountAdditinalChargeGrid.AdditionalTransactions != null)
            {
                lPurchaseEntry.PurchaseAdditionalTransactions = new List<PurchaseAdditionalTransaction>();
                foreach (AdditionalTransaction AdditionalTransaction in DiscountAdditinalChargeGrid.AdditionalTransactions)
                {
                    PurchaseAdditionalTransaction PurchaseAdditionalTransaction = new PurchaseAdditionalTransaction();
                    PurchaseAdditionalTransaction.Action = AdditionalTransaction.Action;
                    PurchaseAdditionalTransaction.Amount = AdditionalTransaction.Amount;
                    PurchaseAdditionalTransaction.DisplayName = AdditionalTransaction.DisplayName;
                    PurchaseAdditionalTransaction.Name = AdditionalTransaction.Name;
                    PurchaseAdditionalTransaction.Sequence = AdditionalTransaction.Sequence;
                    PurchaseAdditionalTransaction.Type = AdditionalTransaction.Type;
                    PurchaseAdditionalTransaction.Value = AdditionalTransaction.Value;
                    PurchaseAdditionalTransaction.AccountId = AdditionalTransaction.AccountId;
                    lPurchaseEntry.PurchaseAdditionalTransactions.Add(PurchaseAdditionalTransaction);
                }
            }
            if (PurchaseBillGrid.PurchaseAttachment != null)
            {
                lPurchaseEntry.PurchaseAttachments = PurchaseBillGrid.PurchaseAttachment.ToList();
            }
            lPurchaseEntry.DisAmount = TotalDiscountAmount;
            lPurchaseEntry.TaxAmount = TotalTaxAmount;
            lPurchaseEntry.TotalAmount = double.Parse(LabelPurchaseEntryFinalAmount.Text);
            double RoundOffTotalAmount = Math.Round(lPurchaseEntry.TotalAmount, 2);
            lPurchaseEntry.RoundOff = RoundOff(RoundOffTotalAmount);
            lPurchaseEntry.NetAmount = RoundOffTotalAmount + lPurchaseEntry.RoundOff;

            return lPurchaseEntry;
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

        private void TextBoxPurchaseEntrySupplier_TextChanged(object sender, EventArgs e)
        {
            TextBoxPurchaseEntrySupplier.Id = null;
        }

        private void BtnPurchaseNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnPurchaseSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    YesNoRbtPurchaseMethod.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            YesNoRbtPurchaseMethod.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPurchaseId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this Purchase is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PurchaseReferenceNumber.Text), "Delete Confirm",
         MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long PurchaseID = Convert.ToInt64(TextBoxPurchaseId.Text);
                PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchaseID);
                if (PurchaseEntry != null)
                {
                    if (ValidateRemovePurchaseEntry(PurchaseID))
                    {
                        if (PurchaseEntry.Paid != 0)
                        {
                            ToolStripStatusLabelErrorPurchase.Text = NotAllowDeleteErrorText;
                            ResetTimmer();
                        }
                        else
                        {
                            bool DeleteResult = PurchaseEntryManager.DeletePurchaseEntry(PurchaseID);
                            if (DeleteResult)
                            {
                                ResetForm();
                                EnableForm(true);
                                YesNoRbtPurchaseMethod.Focus();
                                DirtyFlag(false);
                            }
                            else
                            {
                                MessageBox.Show(DeleteErrorText);
                            }
                        }
                    }
                }
                else
                {
                    DisplaySystemErrorPerformCancel("Somthing went wrong, the selected Purchase is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;

            }
        }
        private bool ValidateRemovePurchaseEntry(long PurchaseID)
        {
            ToolStripStatusLabelErrorPurchase.Text = string.Empty;
            if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
            {
                PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry(PurchaseID);
                IList<PurchaseDetails> PurchaseDetails = PurchaseEntry.PurchaseDetails.ToList();
                foreach (PurchaseDetails Details in PurchaseDetails)
                {
                    double OldQty = Details.Quantity + Details.FreeQuantity;
                    if (Details.isBatch)
                    {
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, (long)PurchaseEntry.InventoryLocationId);
                        if (InventoryBatch != null)
                        {
                            if (((InventoryBatch.OpeningStock + InventoryBatch.Purchased) - InventoryBatch.Sold) < OldQty)
                            {
                                ToolStripStatusLabelErrorPurchase.Text = DeleteValidationErrorText;
                                ResetTimmer();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId((long)Details.ProductId, (long)PurchaseEntry.InventoryLocationId);
                        if (Inventory != null)
                        {
                            if (((Inventory.OpeningStock + Inventory.Purchased) - Inventory.Sold) < OldQty)
                            {
                                ToolStripStatusLabelErrorPurchase.Text = DeleteValidationErrorText;
                                ResetTimmer();
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }
        private void BtnPurchaseCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            ResetForm();
            EnableForm(true);
            YesNoRbtPurchaseMethod.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PurchaseEntry lPurchase = GetPurchaseEntryFromForm();
                    if (PurchaseEntryManager.PurchaseEntryInvoiceNumberUniqueBySuppier(lPurchase))
                    {
                        lPurchase.Balance = lPurchase.NetAmount;
                        lPurchase.Paid = 0F;
                        PurchaseEntry lPurchaseFromDB = null;
                        if (lPurchase.Id == 0)
                        {
                            string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PURCHASE, (DateTime)DatetimePickerPurchaseDate.Date);
                            if (!string.IsNullOrEmpty(RefNumber))
                            {
                                lPurchase.RefNumber = RefNumber;
                                lPurchaseFromDB = new PurchaseEntry();
                                try
                                {
                                    lPurchaseFromDB = PurchaseEntryManager.AddPurchaseEntry(lPurchase);
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
                            PurchaseEntry PurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntry(lPurchase.Id);
                            if (PurchaseEntryInfo != null)
                            {
                                if (PurchaseEntryInfo.NetAmount > lPurchase.NetAmount)
                                {
                                    lPurchase.Balance = PurchaseEntryInfo.Balance - (PurchaseEntryInfo.NetAmount - lPurchase.NetAmount);
                                }
                                else
                                {
                                    lPurchase.Balance = PurchaseEntryInfo.Balance + (lPurchase.NetAmount - PurchaseEntryInfo.NetAmount);
                                }
                                lPurchase.Paid = PurchaseEntryInfo.Paid;
                                lPurchaseFromDB = new PurchaseEntry();
                                lPurchaseFromDB = PurchaseEntryManager.UpdatePurchaseEntry(lPurchase);
                            }
                            else
                            {
                                DisplaySystemErrorPerformCancel("Somthing went wrong, the selected purchase is not valid.");
                                return;
                            }
                        }

                        TextBoxPurchaseId.Text = lPurchaseFromDB.Id.ToString();
                        PurchaseReferenceNumber.Text = lPurchaseFromDB.RefNumber;

                        if (lPurchaseFromDB != null)
                        {
                            EnableForm(false);
                            LoadPurchaseEntry(lPurchaseFromDB.Id);
                            ToolStripStatusLabelErrorPurchase.Text = SaveSuccessText;
                        }
                        DirtyFlag(false);
                    }
                    else
                    {
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(UniqueInvoiceNumberErrorMsg, TextBoxPurcahaseInvoiceNo.Text, TextBoxPurchaseEntrySupplier.Text);
                        TextBoxPurcahaseInvoiceNo.Select();
                        return;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void BtnPurchaseExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBoxPurchaseEntrySupplier.Text.Trim()) ||
                string.IsNullOrEmpty(TextBoxPurchaseEntrySupplier.Id.Trim()) )
            {
                TextBoxPurchaseEntrySupplier.Select();
                ToolStripStatusLabelErrorPurchase.Text = ChooseSupplierErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DatetimePickerPurchaseDate.Date==null||!DateUtils.ValidDate(((DateTime)DatetimePickerPurchaseDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerPurchaseDate.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterPurchaseEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxPurcahaseInvoiceNo.Text.Trim()))
            {
                TextBoxPurcahaseInvoiceNo.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterInvoiceNumberErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DatetimePickerPurchaseInvoiceDate.Date==null || !DateUtils.ValidDate(((DateTime)DatetimePickerPurchaseInvoiceDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerPurchaseInvoiceDate.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterInvoiceDateErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewPurchaseItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value == null)
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value.ToString());
                    double Free = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value.ToString());

                    for (int j = 3; j < 12; j++)
                    {

                        if ((j == (int)PurchaseEntryTableColumn.QTY || j == (int)PurchaseEntryTableColumn.FREE) && !(Quantity <= 0 && Free <= 0)) { continue; }
                        if ((!((bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)) && (j == (int)PurchaseEntryTableColumn.EXPDATE || j == (int)PurchaseEntryTableColumn.BATNO)) { continue; }
                        if (j == (int)PurchaseEntryTableColumn.FREE) { continue; }
                        if (j != 5 && j != 6 && j != 11 && (GridViewPurchaseItem.Rows[i].Cells[j].Value == null || GridViewPurchaseItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewPurchaseItem.Rows[i].Cells[j].Value.ToString()) <= 0))
                        {
                            GridViewPurchaseItem.Select();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j, i];
                            GridViewPurchaseItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewPurchaseItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewPurchaseItem.Rows[i].Cells[j].Value == null)
                        {
                            GridViewPurchaseItem.Select();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j, i];
                            GridViewPurchaseItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewPurchaseItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 11 && GridViewPurchaseItem.Rows[i].Cells[j].Value != null && float.Parse(GridViewPurchaseItem.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            GridViewPurchaseItem.Select();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j, i];
                            GridViewPurchaseItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewPurchaseItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 7)
                        {
                            j = j + 3;
                        }
                    }

                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                    {
                        if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null)
                        {
                            long PDetailId = (long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value;
                            long PId = (long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value;
                            PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(PDetailId);
                            if (PurchaseDetails != null)
                            {
                                double OldQty = PurchaseDetails.Quantity + PurchaseDetails.FreeQuantity;
                                double NewQty = Quantity + Free;
                                PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry((long)PurchaseDetails.PurchaseEntryId);

                                if ((bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                                {
                                    String BNo = (string)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value;

                                    InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(PId, BNo, (long)PurchaseEntry.InventoryLocationId);
                                    if (InventoryBatch != null)
                                    {
                                        if (OldQty > NewQty && ((InventoryBatch.OpeningStock + InventoryBatch.Purchased) - InventoryBatch.Sold) < (OldQty - NewQty))
                                        {
                                            GridViewPurchaseItem.Select();
                                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.QTY, i];
                                            GridViewPurchaseItem.BeginEdit(true);
                                            ToolStripStatusLabelErrorPurchase.Text = Grid_ItemInvalidQuantityErrorMsg;
                                            ResetTimmer();
                                            return false;
                                        }
                                    }

                                }
                                else
                                {
                                    Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, (long)PurchaseEntry.InventoryLocationId);
                                    if (Inventory != null)
                                    {
                                        if (OldQty > NewQty && ((Inventory.OpeningStock + Inventory.Purchased) - Inventory.Sold) < (OldQty - NewQty))
                                        {
                                            GridViewPurchaseItem.Select();
                                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.QTY, i];
                                            GridViewPurchaseItem.BeginEdit(true);
                                            ToolStripStatusLabelErrorPurchase.Text = Grid_ItemInvalidQuantityErrorMsg;
                                            ResetTimmer();
                                            return false;
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
            }
            else
            {
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
                GridViewPurchaseItem.BeginEdit(true);
                ToolStripStatusLabelErrorPurchase.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!DiscountAdditinalChargeGrid.IsDiscountAdditionalChargeValidationResult())
            {
                ToolStripStatusLabelErrorPurchase.Text = DiscountAdditinalChargeGrid.ErrorMsg();
                ResetTimmer();
                return false;
            }
            if (float.Parse(LabelPurchaseEntryFinalAmount.Text) < 0)
            {
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
                GridViewPurchaseItem.BeginEdit(true);
                ToolStripStatusLabelErrorPurchase.Text = InvalidPurchaseEntryErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxPurchaseEntryInventoryLocation.SelectedIndex < 0)
            {
                if (ComboBoxPurchaseEntryInventoryLocation.Text == string.Empty)
                {
                    ToolStripStatusLabelErrorPurchase.Text = SelectInventoryLoactionErrorMsg;
                    ComboBoxPurchaseEntryInventoryLocation.Select();
                    ResetTimmer();
                    return false;
                }
                else
                {
                    string cmbtxt = ComboBoxPurchaseEntryInventoryLocation.Text;
                    int cnt = 0;
                    foreach (var item in ComboBoxPurchaseEntryInventoryLocation.Items)
                    {
                        if (item.ToString() == cmbtxt)
                        {
                            ComboBoxPurchaseEntryInventoryLocation.SelectedIndex = cnt;
                        }
                        cnt++;
                    }
                }
            }

            return true;
        }
        private void ResetForm()
        {
            PurchaseReferenceNumber.Text = "000000";
            TextBoxPurchaseSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorPurchase.Text = "";
            TextBoxPurchaseId.ResetText();
            TextBoxPurcahaseInvoiceNo.ResetText();
            TextBoxPurchaseEntryAddress.ResetText();
            DatetimePickerPurchaseDate.Format = Global.Company.DateFormat;
            DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            ComboUtils.InitializeStockLocationCombo(ComboBoxPurchaseEntryInventoryLocation, Global.Company.CompanyId);
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            ComboBoxPurchaseEntryInventoryLocation.SelectedIndex = key != null ? (key.GetValue("StockLocation") != null && !string.IsNullOrEmpty(key.GetValue("StockLocation").ToString())) ? ComboBoxPurchaseEntryInventoryLocation.FindStringExact(key.GetValue("StockLocation").ToString()) : -1 : -1;
            DatetimePickerPurchaseInvoiceDate.Format = Global.Company.DateFormat;
            DatetimePickerPurchaseInvoiceDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            YesNoRbtPurchaseMethod.Checked = false;
            TextBoxPurchaseEntrySupplier.ResetText();
            GridViewPurchaseItem.Rows.Clear();
            GridViewPurchaseItem.Rows.Add();
            PurchaseBillGrid.Clear();
            DiscountAdditinalChargeGrid.GridType = GridType.Purchase;
            DiscountAdditinalChargeGrid.Clear();
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LabelPurchaseEntryFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            ResetProductAdditinalDetails();

        }
        private PurchaseEntry GetSavedPurchaseEntry()
        {
            PurchaseEntry PurchaseEntry = null;
            if (!string.IsNullOrEmpty(TextBoxPurchaseId.Text))
            {
                PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(long.Parse(TextBoxPurchaseId.Text));
            }
            return PurchaseEntry;
        }
        private void EnableForm(Boolean enable)
        {
            BtnPurchaseDelete.Enabled = true;
            BtnPurchaseSave.Enabled = true;
            YesNoRbtPurchaseMethod.Enabled = true;

            DatetimePickerPurchaseDate.ReadOnly = false;
            DatetimePickerPurchaseDate.TabStop = true;
            TextBoxPurchaseEntrySupplier.ReadOnly = true;
            TextBoxPurchaseEntrySupplier.TabStop = true;
            TextBoxPurchaseEntryAddress.ReadOnly = false;
            TextBoxPurchaseEntryAddress.TabStop = true;

            ComboBoxPurchaseEntryInventoryLocation.Visible = true;
            BtnPurchaseNewSupplier.Enabled = true;
            BtnPurchaseSearchSupplier.Enabled = true;
            GridViewPurchaseItem.ReadOnly = false;
            GridViewPurchaseItem.TabStop = true;
            DiscountAdditinalChargeGrid.Enabled = true;
            GridViewPurchaseItem.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnPurchaseNew.Enabled = !enable;
                BtnPurchaseDelete.Enabled = !enable;
                BtnBarCodeprint.Enabled = !enable;
                BtnPurchaseCancel.Enabled = enable;
                BtnPurchaseSave.Enabled = enable;
            }
            else
            {
                BtnPurchaseNew.Enabled = !enable;
                BtnPurchaseDelete.Enabled = !enable;
                BtnBarCodeprint.Enabled = !enable;
                BtnPurchaseCancel.Enabled = !enable;
                BtnPurchaseSave.Enabled = !enable;
            }
            PurchaseEntry Entry = GetSavedPurchaseEntry();
            if (Entry != null && Entry.isPurchaseEntryLocked)
            {
                BtnPurchaseDelete.Enabled = enable;
                BtnPurchaseSave.Enabled = enable;
                YesNoRbtPurchaseMethod.Enabled = enable;

                DatetimePickerPurchaseDate.ReadOnly = !enable;
                DatetimePickerPurchaseDate.TabStop = enable;
                TextBoxPurchaseEntrySupplier.ReadOnly = !enable;
                TextBoxPurchaseEntrySupplier.TabStop = enable;
                TextBoxPurchaseEntryAddress.ReadOnly = !enable;
                TextBoxPurchaseEntryAddress.TabStop = enable;
                ComboBoxPurchaseEntryInventoryLocation.Visible = enable;
                BtnPurchaseNewSupplier.Enabled = enable;
                BtnPurchaseSearchSupplier.Enabled = enable;
                GridViewPurchaseItem.ReadOnly = !enable;
                GridViewPurchaseItem.TabStop = enable;
                DiscountAdditinalChargeGrid.Enabled = enable;
            }
        }
        private void LoadPurchaseEntry(long PurchaseId)
        {
            ResetForm();
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchaseId);
            if (PurchaseEntry != null)
            {
                PurchaseReferenceNumber.Text = PurchaseEntry.RefNumber;
                TextBoxPurchaseId.Text = PurchaseEntry.Id.ToString();
                if (PurchaseEntry.AccountId == null)
                {
                    TextBoxPurchaseEntrySupplier.Text = PurchaseEntry.SupplierName;
                }
                else
                {
                    TextBoxPurchaseEntrySupplier.Text = PurchaseEntry.Account.Name;
                    TextBoxPurchaseEntrySupplier.Id = PurchaseEntry.AccountId.ToString();
                    CustomerWiseTaxChange();
                }
                TextBoxPurchaseEntryAddress.Text = PurchaseEntry.SupplierAddress.Replace(",", "," + System.Environment.NewLine);
                if (PurchaseEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)PurchaseEntry.InventoryLocationId);
                    if (Location != null)
                    {
                        ComboBoxPurchaseEntryInventoryLocation.SelectedIndex = ComboBoxPurchaseEntryInventoryLocation.FindStringExact(Location.Name);
                    }
                }
                DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.RefDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                DatetimePickerPurchaseInvoiceDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.PurchaseInvDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                TextBoxPurcahaseInvoiceNo.Text = PurchaseEntry.PurchaseInvNumber;
                YesNoRbtPurchaseMethod.Checked = (PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit) ? true : false;
                if (PurchaseEntry.PurchaseDetails.Count > 0)
                {
                    GridViewPurchaseItem.Rows.Add(PurchaseEntry.PurchaseDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var PurchaseDetails in PurchaseEntry.PurchaseDetails)
                    {
                        PurchaseDetails lPurchaseDetails = PurchaseEntryManager.GetPurchaseDetail(PurchaseDetails.Id);
                        if (lPurchaseDetails != null)
                        {
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.SNO].Value = i + 1;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = lPurchaseDetails.Product.Name;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.UOM].Value = lPurchaseDetails.Product.UOM;
                            if (lPurchaseDetails.isBatch)
                            {
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = lPurchaseDetails.BatchNo;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = lPurchaseDetails.ExpDate;

                                InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetails.ProductId, lPurchaseDetails.BatchNo, (long)PurchaseEntry.InventoryLocationId);
                                if (BatchDetails != null)
                                {
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = BatchDetails.Id;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = BatchDetails.RetailSalePrice;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = BatchDetails.WholeSalePrice;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = BatchDetails.MaxRetailPrice;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = BatchDetails.RetailUOM;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = BatchDetails.RetailXFactor;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = BatchDetails.WholesaleUOM;
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = BatchDetails.WholesaleXFactor;
                                }
                            }
                            else
                            {
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = lPurchaseDetails.Product.RetailPrice;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = lPurchaseDetails.Product.WholdSalePrice;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = lPurchaseDetails.Product.Msrp;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = lPurchaseDetails.Product.RetailUOM;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = lPurchaseDetails.Product.RetailXFactor;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = lPurchaseDetails.Product.WholesaleUOM;
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = lPurchaseDetails.Product.WholesaleXFactor;
                            }
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value = lPurchaseDetails.Quantity;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value = lPurchaseDetails.FreeQuantity;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = lPurchaseDetails.PurchasePrice;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.COST].Value = lPurchaseDetails.PurchaseCost;


                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(lPurchaseDetails.TaxDetails);
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value = (lPurchaseDetails.Discounts.Count > 0) ? lPurchaseDetails.Discounts.First().Discount : 0.00;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = lPurchaseDetails.Amount;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value = lPurchaseDetails.ProductId;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value = lPurchaseDetails.isBatch;
                            //for tax
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = lPurchaseDetails.Id;

                            i++;
                        }

                    }
                    Row_Added();
                    ReSequence();
                    if (PurchaseEntry.PurchaseAttachments.Count > 0)
                    {
                        PurchaseBillGrid.PurchaseAttachment = PurchaseEntry.PurchaseAttachments;
                    }

                    if (PurchaseEntry.PurchaseAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = PurchaseEntry.PurchaseAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                OverallTotal();
                YesNoRbtPurchaseMethod.Focus();
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("Something went wrong,please check purchase");
            }
        }
        private double ProductTaxPercentage(IList<LineLevelPurchaseTaxDetail> PurchaseTaxDetails)
        {
            double TaxTotal = 0.00;
            foreach (LineLevelPurchaseTaxDetail Tax in PurchaseTaxDetails)
            {
                TaxTotal = TaxTotal + Tax.TaxRate;
            }
            return TaxTotal;
        }
        private double ProductTaxPercentage(Product Product)
        {
            double TaxTotal = 0.00;
            foreach (CatalogItemSalesTaxMap Map in Product.SalesTax)
            {
                if (Map.EffectiveFrom <= Global.getTransactionDate() && Map.EffectiveTo >= Global.getTransactionDate())
                {
                    CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)Map.SalesTaxMapId);
                    if (CMap != null)
                    {
                        if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, Global.getTransactionDate(), (SupplierId == 0L ? 0L : SupplierId)))
                        {
                            TaxTotal = TaxTotal + Map.TaxPercentage;
                        }
                    }
                }
            }
            return TaxTotal;
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewPurchaseItem.Rows.Count; i++)
            {
                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.SNO].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewPurchaseItem.Rows.Count - 1; i++)
            {
                double Quantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.QTY].Value.ToString()));
                double FreeQuantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.FREE].Value.ToString()));
                double Pprice = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value.ToString()));
                TotalQuantity = TotalQuantity + Quantity;
                double Amount = (Pprice * Quantity);

                if (Amount > 0)
                {
                    double DiscountPercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value.ToString()));

                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DIS].Value = DiscountAmount;

                    Amount = Amount - DiscountAmount;

                    double TaxPercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value.ToString()));

                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAX].Value = TaxAmount;

                    Amount = Amount + TaxAmount;

                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = Amount;

                    double Cost = Amount / (FreeQuantity + Quantity);

                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.COST].Value = Cost;

                    TotalAmount = TotalAmount + Amount;
                }
                else if (FreeQuantity > 0)
                {
                    Amount = FreeQuantity * Pprice;
                    double TaxPercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value.ToString()));
                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAX].Value = TaxAmount;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = TaxAmount;

                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.COST].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DIS].Value = 0.00;
                    TotalAmount = TotalAmount + TaxAmount;
                }
                else
                {
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.COST].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DIS].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAX].Value = 0.00;
                }
            }
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value = TotalAmount;

            DiscountAdditinalChargeGrid.Quantity = TotalQuantity;
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;

            OverallTotal();
        }
        private void OverallTotal()
        {
            LabelPurchaseEntryFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
        public int blinkCount;
        private readonly object Keypress_Validation;

        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerPurchase.Stop();
            TimerPurchase.Start();
        }
        private void TimerPurchase_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorPurchase.Visible = !this.ToolStripStatusLabelErrorPurchase.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerPurchase.Stop();
                ToolStripStatusLabelErrorPurchase.Visible = true;
            }
        }
        private void BtnPurchaseSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                Cursor.Current = Cursors.WaitCursor;
                RecentPurchases();
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
                                BtnPurchaseSave_Click(sender, e);
                                LoadPurchaseEntry(SearchPurchaseId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadPurchaseEntry(SearchPurchaseId);
                        }
                    }
                    else
                    {
                        LoadPurchaseEntry(SearchPurchaseId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBoxPurchaseSearch.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = SearchBoxEmptyErrorMsg;
                TextBoxPurchaseSearch.TextBox.Select();
                return true;
            }
            return true;
        }

        private void RecentPurchases()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            String SearchText = TextBoxPurchaseSearch.Text.Trim();
            PurchaseEntrytype Type = PurchaseEntrytype.PURCHASE;
            IList<PurchaseEntry> PurchaseEntryInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                PurchaseEntryInfo = PurchaseEntryManager.GetRecentPurchaseEntrys(Global.Company.CompanyId, Type);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                PurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntryByAmount(Amount, SearchText, Global.Company.CompanyId, Type);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                PurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntryByDate((DateTime)Date, Global.Company.CompanyId, Type);
            }
            else
            {
                PurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntryBySupplierName(SearchText, Global.Company.CompanyId, Type);
            }

            if (PurchaseEntryInfo.Count > 0)
            {
                LoadPurchaseEntry(PurchaseEntryInfo);
            }
            else
            {
                SearchPurchaseId = 0L;
                ToolStripStatusLabelErrorPurchase.Text = PurchaseSearchOutput;
            }
        }
        public void LoadPurchaseEntry(IList<PurchaseEntry> PurchaseEntryInfo)
        {
            if (PurchaseEntryInfo.Count > 0)
            {
                SearchPurchaseId = 0L;
                FormRecentPurchase FormRecentPurchase = new FormRecentPurchase(this);
                FormRecentPurchase.PurchaseEntryInfo = PurchaseEntryInfo;
                FormRecentPurchase.ShowDialog();

            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = PurchaseSearchOutput;
            }
        }

        private void GridViewPurchaseItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)PurchaseEntryTableColumn.QTY ||
                e.ColumnIndex == (int)PurchaseEntryTableColumn.FREE ||
                e.ColumnIndex == (int)PurchaseEntryTableColumn.PRICE ||
                e.ColumnIndex == (int)PurchaseEntryTableColumn.DISP ||
                e.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
            {
                Row_Added();
            }
            if (e.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
            {
                //bool IsDirty = this.formIsDirty;
                //GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = TextBoxPurchaseEntryProductName.Text;
                //DirtyFlag(IsDirty);
            }
            if (e.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO && GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value != null)
            {
                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value, LocationId);
                if (InventoryBatch != null)
                {
                    LoadProductAdditinalDetailsByBatch(InventoryBatch);
                }
                LoadBatchDetails((long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value);
            }
        }
        private void BtnPurchaseNewSupplier_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxPurchaseEntrySupplier.Id == null ? 0L : long.Parse(TextBoxPurchaseEntrySupplier.Id);
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
                    TextBoxPurchaseEntrySupplier.Text = Customer.Name;
                    TextBoxPurchaseEntrySupplier.Id = SupplierId.ToString();
                    CustomerWiseTaxChange();
                    TextBoxPurchaseEntryAddress.Text = Customer.BillingAddress.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById(SupplierId);
                    if (Supplier != null)
                    {
                        TextBoxPurchaseEntrySupplier.Text = Supplier.Name;
                        TextBoxPurchaseEntrySupplier.Id = SupplierId.ToString();
                        CustomerWiseTaxChange();
                        TextBoxPurchaseEntryAddress.Text = Supplier.Address.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);

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
            TextBoxPurchaseEntrySupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewPurchaseItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewPurchaseItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = TextBoxPurchaseEntryProductName.Text;
                DirtyFlag(IsDirty);
            }
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.SNO].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.UOM].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.QTY].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.FREE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.BATNO].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.EXPDATE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRICE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.COST].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.TAXP].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.TAX].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.DISP].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.DIS].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
            {
                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.QTY].ReadOnly = false;
                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.FREE].ReadOnly = false;
                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRICE].ReadOnly = false;
                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.DISP].ReadOnly = false;
                if ((bool)GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                {
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed && !ValidateLineItemModifiedOrRemove(e.RowIndex))
                    {
                        GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.BATNO].ReadOnly = true;
                    }
                    else
                    {
                        GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.BATNO].ReadOnly = false;
                        GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRODUCT].ReadOnly = false;
                    }
                    GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.EXPDATE].ReadOnly = false;
                }

            }

        }

        private void GridViewPurchaseItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRODUCT].ReadOnly)
            {
                ToolStripStatusLabelErrorPurchase.Text = string.Empty;
                if (e.ColumnIndex == (int)PurchaseEntryTableColumn.REMOVE && (GridViewPurchaseItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.SNO].Value.ToString()), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {

                        if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                        {
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_RowDeleteErrorMsg, GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.SNO].Value.ToString());
                            ResetTimmer();
                            return;
                        }
                        GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewPurchaseItem.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();

                    }
                }

            }
        }
        private bool ValidateLineItemModifiedOrRemove(int Index)
        {
            if (GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null
                            && GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
            {
                long PDetailId = (long)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value;
                long PId = (long)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.ID].Value;
                PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail(PDetailId);
                if (PurchaseDetails != null)
                {
                    double OldQty = PurchaseDetails.Quantity + PurchaseDetails.FreeQuantity;
                    PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry((long)PurchaseDetails.PurchaseEntryId);

                    if ((bool)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                    {
                        String BNo = (string)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value;
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(PId, BNo, (long)PurchaseEntry.InventoryLocationId);
                        if (InventoryBatch != null)
                        {
                            if (InventoryBatch != null && ((InventoryBatch.OpeningStock + InventoryBatch.Purchased) - InventoryBatch.Sold) < OldQty)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, (long)PurchaseEntry.InventoryLocationId);
                        if (Inventory != null)
                        {
                            if (((Inventory.OpeningStock + Inventory.Purchased) - Inventory.Sold) < OldQty)
                            {
                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        }
        private void GridViewPurchaseItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewPurchaseItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewPurchaseItem, "TaxDetailsNumberChecking", GridViewPurchaseItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewPurchaseItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewPurchaseItem_KeyDown;
            }
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewPurchaseItem, "NameCheckingProduct", GridViewPurchaseItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewPurchaseItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewPurchaseItem_KeyDown;
            }
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange;
                ((TextBox)e.Control).TextChanged += ProductTextChange;
            }
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= BatchTextChange;
                ((TextBox)e.Control).TextChanged += BatchTextChange;
            }
        }

        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewPurchaseItem.CurrentCell.ColumnIndex != (int)PurchaseEntryTableColumn.BATNO)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
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
                        long CheckForAddRow = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value : 0L;

                        //for tax update purchase
                        if (GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null &&
                     CheckForAddRow == Product.First().Id)
                        {
                            DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (Result == DialogResult.No)
                            {
                                return;
                            }
                        }

                        LoadUomTax(Product.First().Id);
                        GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (CheckForAddRow == 0 && GridViewPurchaseItem.Rows.Count - 1 == GridViewPurchaseItem.CurrentRow.Index)
                        {
                            GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewPurchaseItem.Rows.Add();
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewPurchaseItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewPurchaseItem.CurrentRow.Index);
                }
            }
        }
        private void BatchTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text) && GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);

                    if (!string.IsNullOrEmpty(TextBoxPurchaseId.Text))
                    {
                        PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(long.Parse(TextBoxPurchaseId.Text));
                        InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value, ((TextBox)sender).Text, (long)PurchaseEntry.InventoryLocationId);
                        if (BatchDetails != null)
                        {
                            LoadBatchDetails(BatchDetails.Id);
                          
                        }
                    }
                    else
                    {
                        ResetBatchDetails();
                    }
                    GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetBatchDetails();
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.UOM].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.QTY].Value = 0;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.FREE].Value = 0;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.COST].Value = 0.00;

            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = 0.00;

            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.DISP].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ID].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value = false;
            //for tax update purchase
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
            ResetProductAdditinalDetails();
            ComputeFormTotal();
        }

        private void GridViewPurchaseItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
            }
            if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
            }
        }
        private void GridViewPurchaseItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
                if (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
                {
                    KeypressValidation.Instance.Keypress_PasteCheckingProduct(sender, e, "NameChecking");
                }
            }
        }
        private void LoadUomTax(long ProductId)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
            if (Product != null)
            {
                LoadProductCombo();
                int index = GridViewPurchaseItem.CurrentRow.Index;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = Product.Name;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.UOM].Value = Product.UOM;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.QTY].Value = 0;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.FREE].Value = 0;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = null;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = null;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = Product.PurchasePrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.COST].Value = 0.00;

                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = Product.RetailPrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = Product.Msrp;

                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = Product.RetailUOM;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = Product.RetailXFactor;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = Product.WholesaleUOM;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = Product.WholesaleXFactor;

                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.TAX].Value = 0.00;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(Product);
                //for tax update purchase
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.DIS].Value = 0.00;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.AMOUNT].Value = 0.00;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null)
                {
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value = false;
                }

                LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId));
            }
        }
        protected override void ProductIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductIdTransport.Text))
            {
                ProductId = long.Parse(ProductIdTransport.Text);
            }
        }
        private void SearchProduct()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value : 0L;
            ProductId = 0L;
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchItems.SearchText = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value != null) ? GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value.ToString() : null;
            FormSearchItems.ShowDialog();
            if (ProductId != 0)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                if (Product != null)
                {
                    //for tax update purchase
                    if (GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null && Check == ProductId)
                    {
                        DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Result == DialogResult.No)
                        {
                            return;
                        }
                    }

                    LoadUomTax(ProductId);
                    if (Check == 0 && GridViewPurchaseItem.Rows.Count - 1 == GridViewPurchaseItem.CurrentRow.Index)
                    {
                        GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewPurchaseItem.Rows.Add();
                    }
                    GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[3, GridViewPurchaseItem.CurrentRow.Index];
                    GridViewPurchaseItem.CurrentCell.Selected = true;
                }
                else
                {
                    ProductId = Check;
                    DisplaySystemError("Somthing went wrong, the selected item is not valid.");
                    return;
                }
            }
            else
            {
                ProductId = Check;
                DirtyFlag(IsDirty);
                return;
            }
            Cursor.Current = Cursors.Default;
        }
        protected override void ProductBatchIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductBatchIdTransport.Text))
            {
                BatchId = long.Parse(ProductBatchIdTransport.Text);
            }
        }
        private void SearchBatch()
        {
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value != null) ? (long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value : 0L;
            BatchId = 0L;
            FormSearchBatch FormSearchBatch = new FormSearchBatch(this);
            GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchBatch.ProductId = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value : 0L;
            FormSearchBatch.SearchText = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value != null) ? GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString() : string.Empty;
            FormSearchBatch.LocationId = LocationId;
            FormSearchBatch.ShowDialog();
            if (BatchId != 0)
            {
                LoadBatchDetails(BatchId);
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.EXPDATE, GridViewPurchaseItem.CurrentRow.Index];
                GridViewPurchaseItem.CurrentCell.Selected = true;
            }
            else
            {
                DirtyFlag(IsDirty);
                return;
            }

        }
        private void LoadBatchDetails(long BatId)
        {
            if (LocationId == 0)
            {
                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
                if (InventoryBatch != null)
                {
                    int index = GridViewPurchaseItem.CurrentRow.Index;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = InventoryBatch.Id;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;

                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;

                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
                }
            }
            else
            {
                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId, LocationId);
                if (InventoryBatch != null)
                {
                    int index = GridViewPurchaseItem.CurrentRow.Index;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = InventoryBatch.Id;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;

                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;

                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
                }
                else
                {
                    InventoryBatch lInventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
                    if (lInventoryBatch != null)
                    {
                        GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = lInventoryBatch.BatchNo;
                    }
                    else
                    {
                        DisplaySystemError("Somthing went wrong, the selected Batch is not valid.");
                        return;
                    }
                }
            }
        }
        private void ResetBatchDetails()
        {
            int index = GridViewPurchaseItem.CurrentRow.Index;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = 0.00;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = 0.00;

            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = 1;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = null;
            GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = 1;

            if (GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = Product.RetailPrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = Product.Msrp;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = Product.RetailUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = Product.RetailXFactor;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = Product.WholesaleUOM;
                    GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                }
            }
        }
        private void GridViewPurchaseItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.SNO].Value = GridViewPurchaseItem.Rows.Count;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value = false;
            (GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.EXPDATE] as CalendarCell).MinDate = Global.getTransactionDate();
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.FREE].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            DirtyFlag(IsDirty);
        }

        private void FormPurchaseEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    YesNoRbtPurchaseMethod.Focus();
                    e.Cancel = true;
                }
            }
        }

        private void BtnPurchaseInvoiceAttachment_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerPurchaseInvoiceDate.Select();
            }
        }
        private void BtnPurchaseSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                YesNoRbtPurchaseMethod.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiscountAdditinalChargeGrid.Focus();
                return;
            }
        }
        private void YesNoRbtPurchaseMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerPurchaseDate.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPurchaseSave.Select();
            }
        }
        private void BtnPurchaseSearchSupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }
        private void TextBoxPurchaseEntrySupplier_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPurchaseEntryAddress.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerPurchaseInvoiceDate.Focus();
            }
        }
        private void TextBoxPurchaseEntryAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPurchaseEntrySupplier.Focus();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2) && (TextBoxPurchaseEntrySupplier.Focused || BtnPurchaseSearchSupplier.Focused))
            {
                BtnPurchaseSearchSupplier.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnPurchaseNew.Enabled)
                {
                    BtnPurchaseNew.PerformClick();
                }
                else
                {
                    if (TextBoxPurchaseEntrySupplier.Focused)
                        BtnPurchaseNewSupplier.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnPurchaseDelete.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnPurchaseSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                GridViewPurchaseItem.EndEdit();
                BtnPurchaseCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnPurchaseExit.PerformClick();
                return true;
            }
            else if (keyData ==(Keys.Delete) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
            {
                GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value = null;
            }
            try
            {
                if ((keyData == Keys.F2) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
                {
                    SearchProduct();
                    return true;
                }
                if ((keyData == Keys.F2) && !GridViewPurchaseItem.CurrentCell.ReadOnly && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO)
                {
                    SearchBatch();
                    return true;
                }                
                if (keyData == (Keys.Tab) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.BATNO && !GridViewPurchaseItem.CurrentCell.ReadOnly)
                {
                    SearchBatchbyNo();
                    return true;                    
                }
                if (keyData == (Keys.Tab) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.FREE)
                {
                    if (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].ReadOnly)
                    {                        
                        SendKeys.Send("{tab}{tab}");
                    }                    
                }
                if (keyData == (Keys.Tab) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT))
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRICE))
                {
                    SendKeys.Send("{tab}{tab}{tab}");
                }

                if (keyData == (Keys.Tab) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.DISP)
                {
                    if (GridViewPurchaseItem.CurrentCell.RowIndex != GridViewPurchaseItem.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRICE)
                {
                    if (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.EXPDATE].ReadOnly)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.DISP))
                {
                    SendKeys.Send("{tab}{tab}{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.QTY))
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseEntryTableColumn.PRODUCT)
                {
                    if (GridViewPurchaseItem.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        TextBoxPurchaseEntryAddress.Focus();
                    }
                }

            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DatetimePickerPurchaseInvoiceDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPurchaseEntrySupplier.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPurcahaseInvoiceNo.Select();
            }
        }

        private void TextBoxPurchaseSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnPurchaseSearch_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnPurchaseSearch.PerformClick();
            }
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                SupplierId = long.Parse(AccountIdTransport.Text);
            }
        }
        private void BtnPurchaseSearchSupplier_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempSupplierId = TextBoxPurchaseEntrySupplier.Id == null ? 0L : long.Parse(TextBoxPurchaseEntrySupplier.Id);
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
                    TextBoxPurchaseEntrySupplier.Text = Supplier.Name;
                    TextBoxPurchaseEntrySupplier.Id = SupplierId.ToString();
                    CustomerWiseTaxChange();
                    TextBoxPurchaseEntryAddress.Text = Supplier.Address.FullAddress.Replace("\n", System.Environment.NewLine);
                }
                else
                {
                    Customer customer = CustomerManager.Instance.GetCustomerById(SupplierId);
                    if (customer != null)
                    {
                        TextBoxPurchaseEntrySupplier.Text = customer.Name;
                        TextBoxPurchaseEntrySupplier.Id = SupplierId.ToString();
                        CustomerWiseTaxChange();
                        TextBoxPurchaseEntryAddress.Text = customer.BillingAddress.FullAddress.Replace("\n", System.Environment.NewLine);
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
            TextBoxPurchaseEntrySupplier.Select();
            Cursor.Current = Cursors.Default;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            return;
        }
        private void DisplaySystemErrorPerformCancel(string Message)
        {
            MessageBox.Show(Message);
            DirtyFlag(false);
            BtnPurchaseCancel.PerformClick();
            return;
        }
        private void GridViewPurchaseItem_Enter(object sender, EventArgs e)
        {
            //GridViewPurchaseItem.BeginInvoke(new MethodInvoker(delegate ()
            //{
            //    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
            //}));
        }
        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            OverallTotal();
        }

        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPurchaseSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, 0];
            }
        }

        private void GridViewPurchaseItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadProductDetails(e.RowIndex);

                //for item sold out
                ToolStripStatusLabelErrorPurchase.Text = string.Empty;
                if (GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null)
                {
                    if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                    {
                        GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.PRODUCT].ReadOnly = true;
                        if ((bool)GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                        {
                            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.BATNO].ReadOnly = true;
                        }
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_RowEditErrorMsg, GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseEntryTableColumn.SNO].Value.ToString());
                    }
                }
            }
        }
        private void LoadProductDetails(int Index)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    LoadProductAdditinalDetails(Product);
                    if (Product.isInventoryAtBatch != null && ((bool)Product.isInventoryAtBatch) && GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value != null)
                    {
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewPurchaseItem.Rows[Index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value, LocationId);
                        if (InventoryBatch != null)
                        {
                            LoadProductAdditinalDetailsByBatch(InventoryBatch);
                        }
                    }

                }
            }
            else
            {
                ResetProductAdditinalDetails();
                EnableProductAdditinalDetails(false);
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetailsByBatch(InventoryBatch InventoryBatch)
        {
            ComboBoxProductRetailUOM.Text = InventoryBatch.RetailUOM;
            ComboBoxProductWholeSaleUOM.Text = InventoryBatch.WholesaleUOM;
            TextBoxProductXFactorRetail.Text = InventoryBatch.RetailXFactor.ToString();
            TextBoxProductXFactorWholeSale.Text = InventoryBatch.WholesaleXFactor.ToString();
            TextBoxProductPurchasePrice.Text = InventoryBatch.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryRetailPrice.Text = InventoryBatch.RetailSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryWholeSalePrice.Text = InventoryBatch.WholeSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryMsrp.Text = InventoryBatch.MaxRetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetailByProductIdBatch(InventoryBatch.ProductId, InventoryBatch.BatchNo);
            if (SaleDetail != null)
            {
                TextBoxProductXFactorRetail.ReadOnly = true;
                TextBoxProductXFactorWholeSale.ReadOnly = true;
                TextBoxProductXFactorRetail.TabStop = false;
                TextBoxProductXFactorWholeSale.TabStop = false;
            }
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            TextBoxPurchaseEntryMaterialId.Text = Product.MaterialId;
            TextBoxPurchaseEntryProductName.Text = Product.Name;
            TextBoxCostHidden.Text = Product.CostPrice.ToString();
            TextBoxProductPurchasePrice.Text = Product.PurchasePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryRetailPrice.Text = Product.RetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryWholeSalePrice.Text = Product.WholdSalePrice.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxPurchaseEntryMsrp.Text = Product.Msrp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            TextBoxDefaultDiscount.Text = ((float)Product.DefaultDiscount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            ComboBoxProductPurchesUOM.Text = Product.UOM;
            ComboBoxProductRetailUOM.Text = Product.RetailUOM;
            ComboBoxProductWholeSaleUOM.Text = Product.WholesaleUOM;
            TextBoxProductXFactorRetail.Text = Product.RetailXFactor.ToString();
            TextBoxProductXFactorWholeSale.Text = Product.WholesaleXFactor.ToString();
            if (Product != null)
            {
                ItemTaxDetails.ProductId = Product.Id;
            }
            EnableProductAdditinalDetails(true);
            SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetailByProductId(Product.Id);
            if (SaleDetail != null && !SaleDetail.isBatch)
            {
                TextBoxProductXFactorRetail.ReadOnly = true;
                TextBoxProductXFactorWholeSale.ReadOnly = true;
                TextBoxProductXFactorRetail.TabStop = false;
                TextBoxProductXFactorWholeSale.TabStop = false;
            }
        }

        private void ResetProductAdditinalDetails()
        {
            TextBoxPurchaseEntryMaterialId.ResetText();
            TextBoxPurchaseEntryProductName.ResetText();
            TextBoxCostHidden.ResetText();
            TextBoxPurchaseEntryRetailPrice.ResetText();
            TextBoxProductPurchasePrice.ResetText();
            TextBoxPurchaseEntryWholeSalePrice.ResetText();
            TextBoxDefaultDiscount.ResetText();
            TextBoxPurchaseEntryMsrp.ResetText();
            ComboBoxProductPurchesUOM.ResetText();
            ComboBoxProductRetailUOM.ResetText();
            ComboBoxProductWholeSaleUOM.ResetText();
            ComboBoxProductPurchesUOM.SelectedIndex = -1;
            ComboBoxProductRetailUOM.SelectedIndex = -1;
            ComboBoxProductWholeSaleUOM.SelectedIndex = -1;
            TextBoxProductXFactorRetail.ResetText();
            TextBoxProductXFactorWholeSale.ResetText();
            ItemTaxDetails.Clear();
        }

        private void EnableProductAdditinalDetails(bool enable)
        {
            BtnProductReset.Enabled = enable;
            BtnProductSave.Enabled = enable;
            BtnPriceCalculator.Enabled = enable;
            ItemTaxDetails.EnableEdit = false;
            EditProductTaxLink.Visible= enable;
            TextBoxPurchaseEntryProductName.ReadOnly = !enable;
            TextBoxProductPurchasePrice.ReadOnly = !enable;
            TextBoxPurchaseEntryRetailPrice.ReadOnly = !enable;
            TextBoxPurchaseEntryWholeSalePrice.ReadOnly = !enable;
            TextBoxDefaultDiscount.ReadOnly = !enable;
            TextBoxPurchaseEntryMsrp.ReadOnly = !enable;
            TextBoxProductXFactorRetail.ReadOnly = !enable;
            TextBoxProductXFactorWholeSale.ReadOnly = !enable;
            TextBoxPurchaseEntryMaterialId.ReadOnly = true;
            TextBoxProductPurchasePrice.ReadOnly = !enable;

            ComboBoxProductPurchesUOM.Visible = false;
            ComboBoxProductRetailUOM.Visible = enable;
            ComboBoxProductWholeSaleUOM.Visible = enable;

            TextBoxPurchaseEntryProductName.TabStop = enable;
            TextBoxProductPurchasePrice.TabStop = enable;
            TextBoxPurchaseEntryRetailPrice.TabStop = enable;
            TextBoxPurchaseEntryWholeSalePrice.TabStop = enable;
            TextBoxDefaultDiscount.TabStop = enable;
            TextBoxPurchaseEntryMsrp.TabStop = enable;
            TextBoxProductXFactorRetail.TabStop = enable;
            TextBoxProductXFactorWholeSale.TabStop = enable;

        }
        private void LoadProductCombo()
        {
            ComboUtils.InitializeAllUniquePurchaseUOMCombo(ComboBoxProductPurchesUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueRetailUOMCombo(ComboBoxProductRetailUOM, Global.Company.CompanyId);
            ComboUtils.InitializeAllUniqueWholeSaleUOMCombo(ComboBoxProductWholeSaleUOM, Global.Company.CompanyId);
        }
        private void GridViewPurchaseItem_Leave(object sender, EventArgs e)
        {
            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.UOM, GridViewPurchaseItem.CurrentRow.Index];
        }
        private void ComboBoxProductPurchesUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxPurchaseEntryProductName_KeyPress(sender, e);
            this.ComboBoxProductPurchesUOM.DroppedDown = false;

        }

        private void ComboBoxProductRetailUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxPurchaseEntryProductName_KeyPress(sender, e);
            this.ComboBoxProductRetailUOM.DroppedDown = false;
        }

        private void ComboBoxProductWholeSaleUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBoxPurchaseEntryProductName_KeyPress(sender, e);
            this.ComboBoxProductWholeSaleUOM.DroppedDown = false;
        }

        private void TextBoxPurchaseEntryProductName_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
        }

        private void TextBoxProductXFactorRetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeypressValidation.Instance.Keypress_Num(sender, e);
        }

        private void BtnProductReset_Click(object sender, EventArgs e)
        {
            LoadProductDetails(GridViewPurchaseItem.CurrentRow.Index);
        }
        private Product GetProductFromForm()
        {
            Product lProduct = new Product();
            if (GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    lProduct.Id = Product.Id;

                    lProduct.CompanyId = Global.Company.CompanyId;
                    lProduct.Name = TextBoxPurchaseEntryProductName.Text.Trim();
                    lProduct.Description = Product.Description;
                    lProduct.UOM = ComboBoxProductPurchesUOM.Text.Trim();
                    lProduct.RetailUOM = ComboBoxProductRetailUOM.Text.Trim();
                    lProduct.RetailXFactor = int.Parse(TextBoxProductXFactorRetail.Text.Trim());
                    lProduct.WholesaleUOM = ComboBoxProductWholeSaleUOM.Text.Trim();
                    lProduct.WholesaleXFactor = int.Parse(TextBoxProductXFactorWholeSale.Text.Trim());
                    lProduct.PurchasePrice = float.Parse(TextBoxProductPurchasePrice.Text.Trim());
                    lProduct.CostPrice = Product.CostPrice;
                    lProduct.RetailPrice = float.Parse(TextBoxPurchaseEntryRetailPrice.Text.Trim());
                    lProduct.WholdSalePrice = float.Parse(TextBoxPurchaseEntryWholeSalePrice.Text.Trim());
                    lProduct.DefaultDiscount = float.Parse(TextBoxDefaultDiscount.Text.Trim());
                    lProduct.Msrp = float.Parse(TextBoxPurchaseEntryMsrp.Text.ToString());
                    lProduct.Manufacturer = Product.Manufacturer;
                    lProduct.Supplier = Product.Supplier;
                    lProduct.MaterialId = Product.MaterialId;

                    lProduct._isInventoryAtBatch = Product._isInventoryAtBatch;

                    lProduct.ParentId = Product.ParentId;
                    lProduct.ProductFamilyId = Product.ProductFamilyId;
                    lProduct.SalesAccount = Product.SalesAccount;
                    lProduct.PurchaseAccount = Product.PurchaseAccount;
                    lProduct.InventoryAccount = Product.InventoryAccount;
                    //if (ItemTaxDetails.CatalogItemSalesTaxMaps != null && ItemTaxDetails.CatalogItemSalesTaxMaps.Count > 0)
                    //{
                    //    lProduct.SalesTax = ItemTaxDetails.CatalogItemSalesTaxMaps;
                    //}
                }
                else
                {
                    MessageBox.Show("Something went wrong, the product is not available");
                }
            }
            return lProduct;
        }

        private void BtnProductSave_Click(object sender, EventArgs e)
        {
            if (validateFormProduct())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Product lProduct = GetProductFromForm();
                    Product lProductById = CatalogProductManager.Instance.GetProductInfoById(lProduct.Id);
                    if (lProductById != null)
                    {
                        if (CatalogProductManager.Instance.ProductNameUniqueById(lProduct))
                        {
                            Product lProductFromDB = null;

                            lProductFromDB = CatalogProductManager.Instance.UpdateProduct(lProduct);
                            InventoryBatch InventoryBatch = null;
                            //for batch
                            if (lProductFromDB.isInventoryAtBatch != null && ((bool)lProductFromDB.isInventoryAtBatch) &&
                                GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value != null &&
                                !string.IsNullOrEmpty(GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString()))
                            {
                                if (GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value != null)
                                {
                                    //for old batch
                                    InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value, LocationId);
                                    if (InventoryBatch != null)
                                    {
                                        InventoryBatch.Inventory = null;
                                        InventoryBatch.Product = null;
                                        InventoryBatch.PurchasePrice = lProductFromDB.PurchasePrice;
                                        InventoryBatch.RetailSalePrice = lProductFromDB.RetailPrice;
                                        InventoryBatch.WholeSalePrice = lProductFromDB.WholdSalePrice;
                                        InventoryBatch.MaxRetailPrice = lProductFromDB.Msrp;

                                        InventoryBatch.RetailUOM = lProductFromDB.RetailUOM;
                                        InventoryBatch.RetailXFactor = lProductFromDB.RetailXFactor;
                                        InventoryBatch.WholesaleUOM = lProductFromDB.WholesaleUOM;
                                        InventoryBatch.WholesaleXFactor = lProductFromDB.WholesaleXFactor;

                                        InventoryLocationManager.Instance.UpdateInventoryBatch(InventoryBatch);
                                    }
                                }
                                else
                                {
                                    //for new batch
                                    Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(lProductFromDB.Id, LocationId);
                                    if (Inventory == null && LocationId != 0L)
                                    {
                                        //for new inventory
                                        Inventory lInventory = new Inventory();
                                        lInventory.CompanyId = Global.Company.CompanyId;
                                        if (Global.CostCenter != null) { lInventory.CostCenterId = Global.CostCenter.CostCenterId; }
                                        lInventory.ProductId = lProductFromDB.Id;
                                        lInventory.InventoryLocationId = LocationId;
                                        Inventory = InventoryLocationManager.Instance.AddInventory(lInventory);
                                    }

                                    if (Inventory != null)
                                    {
                                        InventoryBatch = new InventoryBatch();
                                        InventoryBatch.InventoryId = Inventory.Id;
                                        InventoryBatch.ProductId = Inventory.ProductId;
                                        InventoryBatch.CompanyId = Global.Company.CompanyId;
                                        if (Global.CostCenter != null) { InventoryBatch.CostCenterId = Global.CostCenter.CostCenterId; }
                                        InventoryBatch.PurchasePrice = lProductFromDB.PurchasePrice;
                                        InventoryBatch.RetailSalePrice = lProductFromDB.RetailPrice;
                                        InventoryBatch.WholeSalePrice = lProductFromDB.WholdSalePrice;
                                        InventoryBatch.MaxRetailPrice = lProductFromDB.Msrp;

                                        InventoryBatch.RetailUOM = lProductFromDB.RetailUOM;
                                        InventoryBatch.RetailXFactor = lProductFromDB.RetailXFactor;
                                        InventoryBatch.WholesaleUOM = lProductFromDB.WholesaleUOM;
                                        InventoryBatch.WholesaleXFactor = lProductFromDB.WholesaleXFactor;

                                        InventoryBatch.BatchNo = GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString();
                                        InventoryLocationManager.Instance.AddInventoryBatch(InventoryBatch);
                                    }
                                }

                            }


                            int RIndex = GridViewPurchaseItem.CurrentRow.Index;
                            for (int i = 0; i < GridViewPurchaseItem.Rows.Count - 1; i++)
                            {
                                if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value.ToString() == lProductFromDB.Id.ToString())
                                {
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRODUCT].Value = lProductFromDB.Name;

                                    PurchaseDetails PurchaseDetails = null;
                                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value != null)
                                    {
                                        PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetail((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value);
                                    }

                                    if (PurchaseDetails == null ||
                                        (PurchaseDetails != null && (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value == null ||
                                            string.IsNullOrEmpty(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value.ToString()) ||
                                            PurchaseDetails.PurchasePrice != float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value.ToString()))))
                                    {

                                        if ((bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                                        {
                                            if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value != null && InventoryBatch.BatchNo == GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString())
                                            {
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = InventoryBatch.Id;

                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(CatalogProductManager.Instance.GetProductInfoById(lProductFromDB.Id));
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value = lProductFromDB.DefaultDiscount;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = lProductFromDB.PurchasePrice;

                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = lProductFromDB.RetailPrice;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = lProductFromDB.WholdSalePrice;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = lProductFromDB.Msrp;

                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = lProductFromDB.RetailUOM;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = lProductFromDB.RetailXFactor;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = lProductFromDB.WholesaleUOM;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = lProductFromDB.WholesaleXFactor;

                                            }

                                            if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value == null ||
                                                string.IsNullOrEmpty(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString()) || (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value == null))
                                            {
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(CatalogProductManager.Instance.GetProductInfoById(lProductFromDB.Id));
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value = lProductFromDB.DefaultDiscount;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = lProductFromDB.PurchasePrice;

                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = lProductFromDB.RetailPrice;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = lProductFromDB.WholdSalePrice;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = lProductFromDB.Msrp;

                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = lProductFromDB.RetailUOM;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = lProductFromDB.RetailXFactor;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = lProductFromDB.WholesaleUOM;
                                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = lProductFromDB.WholesaleXFactor;

                                            }
                                        }
                                        else
                                        {

                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(CatalogProductManager.Instance.GetProductInfoById(lProductFromDB.Id));
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.DISP].Value = lProductFromDB.DefaultDiscount;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = lProductFromDB.PurchasePrice;

                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = lProductFromDB.RetailPrice;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = lProductFromDB.WholdSalePrice;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = lProductFromDB.Msrp;

                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = lProductFromDB.RetailUOM;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = lProductFromDB.RetailXFactor;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = lProductFromDB.WholesaleUOM;
                                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = lProductFromDB.WholesaleXFactor;

                                        }

                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            ComputeFormTotal();
                            GridViewPurchaseItem.Focus();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.PRODUCT, RIndex];

                            LoadProductCombo();
                            EnableForm(false);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(UniqueProductNameErrorMsg, lProduct.Name);
                            TextBoxPurchaseEntryProductName.Select();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong, the product is not available");
                        return;
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private Boolean validateFormProduct()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBoxPurchaseEntryProductName.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterProductNameErrorMsg;
                TextBoxPurchaseEntryProductName.Select();
                return false;
            }

            if (string.IsNullOrEmpty(ComboBoxProductRetailUOM.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterRUomErrorMsg;
                ComboBoxProductRetailUOM.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductXFactorRetail.Text.Trim()) || int.Parse(TextBoxProductXFactorRetail.Text.Trim()) < 1)
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterRXfactorErrorMsg;
                TextBoxProductXFactorRetail.Select();
                return false;
            }
            if (string.IsNullOrEmpty(ComboBoxProductWholeSaleUOM.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterWUomErrorMsg;
                ComboBoxProductWholeSaleUOM.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxProductXFactorWholeSale.Text.Trim()) || int.Parse(TextBoxProductXFactorWholeSale.Text.Trim()) < 1)
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterWXfactorErrorMsg;
                TextBoxProductXFactorWholeSale.Select();
                return false;
            }
            //double purchase = double.Parse(TextBoxProductPurchasePrice.Text);
            double Retail = double.Parse(TextBoxPurchaseEntryRetailPrice.Text);
            //double Wprice = double.Parse(TextBoxPurchaseEntryWholeSalePrice.Text);
            double Msrp = double.Parse(TextBoxPurchaseEntryMsrp.Text);
            if (Retail > Msrp)
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterMrpErrorMsg;
                TextBoxPurchaseEntryMsrp.Select();
                return false;
            }
            if (!string.IsNullOrEmpty(TextBoxDefaultDiscount.Text.Trim()) && (float.Parse(TextBoxDefaultDiscount.Text) > 100))
            {
                ToolStripStatusLabelErrorPurchase.Text = EnterDefaultDisErrorMsg;
                TextBoxDefaultDiscount.Select();
                return false;
            }
            return true;
        }

        private void TextBoxPurchaseEntryProductName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxProductRetailUOM.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnProductSave.Select();
            }
        }

        private void BtnProductSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxPurchaseEntryProductName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ItemTaxDetails.Focus();
            }
        }

        private void YesNoRbtPurchaseMethod_Load(object sender, EventArgs e)
        {
            if (YesNoRbtPurchaseMethod.Checked)
            {
                TextBoxPurchaseEntrySupplier.ReadOnly = true;
                TextBoxPurchaseEntrySupplier.TabStop = true;
                if (TextBoxPurchaseEntrySupplier.Id == null)
                {
                    TextBoxPurchaseEntrySupplier.ResetText();
                }
            }
            else
            {
                TextBoxPurchaseEntrySupplier.ReadOnly = true;
                TextBoxPurchaseEntrySupplier.TabStop = true;
            }

        }
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
            DiscountAdditinalChargeGrid.IsDirty = Enable;
        }
        private void DiscountAdditinalChargeGrid_Enter(object sender, EventArgs e)
        {
            //if (!this.formIsDirty)
            //{
            //    this.formIsDirty = DiscountAdditinalChargeGrid.IsDirty;
            //}
        }

        private void DiscountAdditinalChargeGrid_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.formIsDirty)
            {
                this.formIsDirty = DiscountAdditinalChargeGrid.IsDirty;
            }
        }

        private void TextBoxPurchaseEntryAddress_TextChanged(object sender, EventArgs e)
        {
            //if (TextBoxPurchaseEntryAddress.Text.Contains("\t"))
            //{
            //    TextBoxPurchaseEntryAddress.Text = TextBoxPurchaseEntryAddress.Text.Trim();
            //}
        }

        private void BtnPriceCalculator_Click(object sender, EventArgs e)
        {
            FormItemPriceCalculator PriceCalculator = new FormItemPriceCalculator(this);
            PriceCalculator.ShowDialog(this);
        }

        private void BtnBarCodeprint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPurchaseBarcodePrint FormPurchaseBarcodePrint = new FormPurchaseBarcodePrint();
            FormPurchaseBarcodePrint.PurchaseEntryId = long.Parse(TextBoxPurchaseId.Text);
            FormPurchaseBarcodePrint.ShowDialog();
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxPurchaseEntryAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void TextBoxPurchaseEntryProductName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                KeypressValidation.Instance.AddContextMenuProduct(TextBoxPurchaseEntryProductName, "NameChecking");
            }
        }

        private void TextBoxPurchaseEntryProductName_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressValidation.Instance.Keypress_PasteCheckingProduct(sender, e, "NameChecking");
        }

        private void ComboBoxPurchaseEntryInventoryLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationId = ComboBoxPurchaseEntryInventoryLocation.SelectedIndex > -1 ? ((InventoryLocation)ComboBoxPurchaseEntryInventoryLocation.Items[ComboBoxPurchaseEntryInventoryLocation.SelectedIndex]).Id : 0L;
        }
        private void LoadBatchDetailsDirect(long BatId, long BatchLocationId)
        {
            InventoryBatch InventoryBatch = null;
            if (BatchLocationId != 0)
            {
                InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId, BatchLocationId);
            }
            else if (BatId != 0 && BatchLocationId == 0)
            {
                InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
            }
            if (InventoryBatch != null)
            {
                int index = GridViewPurchaseItem.CurrentRow.Index;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value = InventoryBatch.Id;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;

                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;

                GridViewPurchaseItem.Rows[index].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value = null;
            }
            else
            {
                InventoryBatch lInventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
                if (lInventoryBatch != null)
                {
                    GridViewPurchaseItem.Rows[GridViewPurchaseItem.CurrentRow.Index].Cells[(int)PurchaseEntryTableColumn.BATNO].Value = lInventoryBatch.BatchNo;
                }
                else
                {
                    DisplaySystemError("Somthing went wrong, the selected Batch is not valid.");
                    return;
                }
            }
        }
        private bool SearchBatchbyNo() 
        {
            bool IsDirty = this.formIsDirty;
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value  != null) //if ((GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value ?? null).ToString() != null)
            {
                BatchId = 0;
                long ProductIdBatno = (GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value != null) ? (long)GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.ID].Value : 0L;// GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATCHID].Value;
                long PurchaseLocationId = ComboBoxPurchaseEntryInventoryLocation.SelectedIndex > -1 ? ((InventoryLocation)ComboBoxPurchaseEntryInventoryLocation.Items[ComboBoxPurchaseEntryInventoryLocation.SelectedIndex]).Id : 0L; ;
                string Batchno = GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.BATNO].Value.ToString();
                InventoryBatch BatchDetail = null;
                if (PurchaseLocationId != 0L)
                {
                    BatchDetail = InventoryLocationManager.Instance.GetInventoryBatchDetail(ProductIdBatno, Batchno, PurchaseLocationId);

                }
                else if(PurchaseLocationId == 0L && string.IsNullOrWhiteSpace(ComboBoxPurchaseEntryInventoryLocation.Text) || ComboBoxPurchaseEntryInventoryLocation.SelectedIndex < 0)
                {
                    ComboBoxPurchaseEntryInventoryLocation.Text = "";
                    BatchDetail = InventoryLocationManager.Instance.GetInventoryBatchDetail(ProductIdBatno, Batchno);
                }
                if (BatchDetail != null)
                {
                    BatchId = BatchDetail.Id;
                    if (BatchId != 0)
                    {
                        //LoadBatchDetails(BatchId);
                        LoadBatchDetailsDirect(BatchId, PurchaseLocationId);
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.EXPDATE, GridViewPurchaseItem.CurrentRow.Index];
                        GridViewPurchaseItem.CurrentCell.Selected = true;
                        return true;
                    }
                    else
                    {
                        DirtyFlag(IsDirty);
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.EXPDATE, GridViewPurchaseItem.CurrentRow.Index];
                        GridViewPurchaseItem.CurrentCell.Selected = true;
                        return false;
                    }
                }
                DirtyFlag(IsDirty);
                GridViewPurchaseItem.CurrentRow.Cells[(int)PurchaseEntryTableColumn.EXPDATE].Value = Global.getTransactionDate();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseEntryTableColumn.EXPDATE, GridViewPurchaseItem.CurrentRow.Index];
                GridViewPurchaseItem.CurrentCell.Selected = true;
                return false;
            }
            //ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, "Batch No");
            return true;
        }

        private void EditProductTaxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ProductId != 0L)
            {
                FormCatalogTax formCatalogTax = new FormCatalogTax();
                formCatalogTax.CatalogItemId = ProductId;
                formCatalogTax.ShowDialog();
                itemTaxDetails1.ProductId = ProductId;
            }
        }
        private void CustomerWiseTaxChange()
        {
            if (SupplierId != null)
            {
                if (GridViewPurchaseItem.Rows.Count > 1)
                {
                    for (int i = 0; i <= GridViewPurchaseItem.Rows.Count - 2; i++)
                    {
                        if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value != null)
                        {
                            long ProductId = (long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value;
                            Product lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                            if (lProduct != null)
                            {
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.TAXP].Value = ProductTaxPercentage(lProduct);
                            }
                        }
                    }
                    ComputeFormTotal();
                }
            }
        }
        
    }
}
