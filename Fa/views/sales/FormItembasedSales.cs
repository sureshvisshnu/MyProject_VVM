using DocumentFormat.OpenXml.Spreadsheet;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.Log;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.catalog;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.views.account.masters;
using fa.views.common;
using fa.views.controls.accounting;
using fa.views.controls.grid;
using fa.views.hms.helper;
using fa.views.purchase;
using fa.views.utils;
using Fa.api.catalog;
using Fa.report.accounting.master;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VisioForge.MediaFramework.Helpers;
using VisioForge.Libs.NDI;
using Syncfusion.Styles;

namespace fa.views.sales
{
    public enum SaleEntryTableColumn
    {
        SNO, PRODUCT, UOM, QTY, FREE, BATNO, EXPDATE, PRICE, OPRICE, TAXP, TAX, DISP, DIS, AMOUNT, REMOVE, ID, ISBAT, SALESDETAILID, BATCHID, MSRP
    }
    public enum SaleEntryTotalTableColumn
    {
        NAME, VALUE
    }
    public partial class FormItembasedSales : FormBase
    {
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteErrorText = "Error in Sales Deleting. !";
        public static string NotAllowDeleteErrorText = "Do not Delete Sale it used in Receipt";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string DeleteConfirmText = "Do you want to delete Sale Entry {0}?";
        public static string SelectInventoryLoactionErrorMsg = "Please select inventory location.";
        public static string ChooseCustomerErrorMsg = "Please select customer.";
        public static string ChooseCustomerStateErrorMsg = "Please select customer state.";
        public static string EnterSaleEntryDateErrorMsg = "Please enter valid sale date.";
        public static string EnterInvoiceDateErrorMsg = "Please enter valid invoice date.";
        public static string Grid_DeleteConfirmText = "Do you want to delete Row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_ItemBatchExpire = "Please enter valid {0},This batch item is expired.";
        public static string TextChange_ItemBatchExpire = "Batch no {0} is expired,Please select valid batch no.";
        public static string Grid_EmptyErrorMsg = "Please enter sale Items details.";
        public static string Grid_InvalidBatchErrorMsg = "Batch not available.";
        public static string PatientPurchaseAccountErrorMsg = "Please set patient purchase account.";
        public static string InvalidSaleEntryErrorMsg = "Invalid sale.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Sale Date or Sale Reference Number.";
        public static string QuoteBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Quote Date or Quote Reference Number.";
        public static string SaleSearchOutput = "No sale found.";
        public static string SaleQuoteSearchOutput = "No sale quote found";
        public static string SlectedBatchNotInCurrentLocationErrorMsg = "Your selected Batch not present in current location.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string UpdateCustomerLicenceErrorMsg = "Please update customer licence info.";
        public static string NoCreditLimitErrMsg = "Customer {0} have no credit limit";
        public static string NotAlowedErrMsg = "Customer {0} allowed only for cash payment";
        public static string ExceedCreditLimitErrMsg = "Customer {0} has exceeded the credit limit. Available credit limit ";
        SalesManager SalesManager;
        //AccountManager AccountManager = null;
        public bool ItemBasedsaleOnLoad = false;
        public long ProductId = 0L;
        public long BatchId;
        public long CustomerId = 0L;
        public long SearchSalesId = 0L;
        public bool IsScanner;
        public long NoteId = 0L;
        public long PatientId = 0L;
        public long OPId = 0L;
        public long LocationId = 0L;
        public long FormLocationId = 0L;
        public FormItembasedSales()
        {
            InitializeComponent();
            SalesManager = SalesManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "groupBox2", "DiscountAdditinalChargeGrid", "ToolStripStatusLabelErrorPurchase", "ComboBoxSaleInventoryLocation", "SaleProductDetails" };
        }
        private void FormItembasedSales_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadUomTax(0L);
            this.Visible = false;
            setSize();
            this.Visible = true;
            ResetForm();
            ComboUtils.InitializeStockLocationCombo(ComboBoxSaleInventoryLocation, Global.Company.CompanyId);
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
            ComboBoxSaleInventoryLocation.SelectedIndex = key != null ? (key.GetValue("StockLocation") != null && !string.IsNullOrEmpty(key.GetValue("StockLocation")?.ToString())) ? ComboBoxSaleInventoryLocation.FindStringExact(key.GetValue("StockLocation")?.ToString()) : -1 : -1;
            if (ComboBoxSaleInventoryLocation.SelectedIndex > -1)
            {
                FormLocationId = ((InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex]).Id;
            }
            EnableForm(true);
            LoadProductCombo();
            GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
            {
                GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                GridViewSalesItem.CurrentCell.Selected = true;
                GridViewSalesItem.BeginEdit(true);
            }));
            DirtyFlag(false);
            if (ItemBasedsaleOnLoad)
            {
                LoadSaleEntry(SearchSalesId);
            }
            Cursor.Current = Cursors.Default;
        }
        private void setSize()
        {
            var _ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var _ScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            if (_ScreenWidth < 1350)
            {
                this.Width = 1104;
            }
        }
        private SaleEntry GetSaleEntryFromForm()
        {
            SaleEntry lSaleEntry = new SaleEntry();
            lSaleEntry.WorkStationId = Global.getWorkStationId();
            lSaleEntry.WorkStationName = Global.getWorkStationName();
            lSaleEntry.NoteId = NoteId;
            lSaleEntry.PatientId = PatientId != 0L ? PatientId : null;
            lSaleEntry.RegistrationId = OPId != 0L ? OPId : null;
            lSaleEntry.Id = TextBoxSalesId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxSalesId.Text);
            lSaleEntry.EntryType = TextBoxSalesType.Text == string.Empty ? Entrytype.SALE : (Entrytype)Enum.Parse(typeof(Entrytype), TextBoxSalesType.Text);
            lSaleEntry.RefNumber = SalesReferenceNumber.Text;
            if (TextBoxSalesEntryCustomer.Id == null)
            {
                lSaleEntry.CustomerName = TextBoxSalesEntryCustomer.Text;
            }
            else
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxSalesEntryCustomer.Id));
                if (Customer != null)
                {
                    lSaleEntry.AccountsId = Customer.Id;
                    lSaleEntry.CustomerName = Customer.Name;
                }
                else
                {
                    Supplier supplier = SupplierManager.Instance.GetSupplierById(long.Parse(TextBoxSalesEntryCustomer.Id));
                    if (supplier != null)
                    {
                        lSaleEntry.AccountsId = supplier.Id;
                        lSaleEntry.CustomerName = supplier.Name;
                    }
                }
            }

            if (ComboBoxSaleInventoryLocation.SelectedIndex > -1)
            {
                lSaleEntry.InventoryLocationId = ((InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex]).Id;
            }
            lSaleEntry.Memo = TextBoxSalesMemo.Text.Replace("\r", "").Replace(",", "").Replace("\n", ",");
            lSaleEntry.CustomerAddress = TextBoxSalesCustomerAddress.Text.Replace("\r", "").Replace(",", "").Replace("\n", ",");
            lSaleEntry.SaleMethod = (YesNoRbtSalesMethod.Checked) ? SaleMethod.Credit : SaleMethod.Cash;
            lSaleEntry.SaleDate = (DateTime)DatetimePickerSalesDate.Date;
            //FOR SQL
            lSaleEntry.QuotaionExpireAt = DateTime.Now.Date;
            lSaleEntry.ReturnDate = DateTime.Now.Date;
            lSaleEntry.CompanyId = Global.Company.CompanyId;
            if (ComboBoxSoldby.SelectedIndex > -1) { lSaleEntry.SoldById = ((Refered)ComboBoxSoldby.Items[ComboBoxSoldby.SelectedIndex]).Id; }
            if (ComboBoxreferedby.SelectedIndex > -1) { lSaleEntry.ReferedById = ((Refered)ComboBoxreferedby.Items[ComboBoxreferedby.SelectedIndex]).Id; }
            if (Global.CostCenter != null)
            {
                lSaleEntry.CostCenterId = Global.CostCenter.CostCenterId;
            }
            float TotalAmount = float.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value.ToString());
            float TotalTaxAmount = 0;
            float TotalDiscountPercentage = 0;
            float TotalDiscountAmount = 0;
            float[] TaxWisePercentageTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            for (int i = 0; i < GridViewSalesItem.Rows.Count - 1; i++)
            {
                SaleDetail SaleDetail = new SaleDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    SaleDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        SaleDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    SaleDetail.Id = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value == null) ? 0L : long.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value.ToString());
                    SaleDetail.ProductId = Product.Id;
                    SaleDetail.MaterialId = Product.MaterialId;
                    SaleDetail.Msrp = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.MSRP].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.MSRP].Value.ToString()) : 0;
                    SaleDetail.isBatch = (bool)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ISBAT].Value;
                    SaleDetail.Uom = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                    //FOR SQL
                    SaleDetail.ExpDate = DateTime.Now.Date;
                    if (SaleDetail.isBatch)
                    {
                        SaleDetail.BatchNo = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATNO].Value.ToString();
                        SaleDetail.ExpDate = (DateTime)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.EXPDATE].Value;
                    }
                    double Quantity = 0;
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY] != null)
                    {
                        Quantity = double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString());
                    }
                    double FreeQuantity = 0;
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE] != null)
                    {
                        FreeQuantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value == null) ? 0 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString());
                    }
                    float Pprice = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value.ToString());
                    if (FreeQuantity > 0)
                    {
                        SaleDetail.isFree = true;
                    }
                    else
                    {
                        SaleDetail.isFree = false;
                    }
                    SaleDetail.Quantity = Quantity;
                    SaleDetail.FreeQuantity = FreeQuantity;
                    SaleDetail.Price = Pprice;
                    SaleDetail.OverridePrice = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value.ToString()) : 0;
                    SaleDetail.Amount = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value.ToString()) : 0;
                    double Amount = Quantity * Pprice;
                    float Discount = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value.ToString()) : 0;
                    if (Discount > 0)
                    {
                        TotalDiscountPercentage = TotalDiscountPercentage + Discount;
                        float DiscountAmount = (float)Amount * (Discount / 100);
                        TotalDiscountAmount = TotalDiscountAmount + DiscountAmount;
                        ItemLevelSaleDiscount SaleDiscount = new ItemLevelSaleDiscount();
                        SaleDiscount.DisccountType = DiscountType.PERCENT;
                        SaleDiscount.Discount = Discount;
                        SaleDiscount.DiscountAmount = DiscountAmount;
                        SaleDiscount.DiscountSequence = 1;
                        SaleDetail.Discounts.Add(SaleDiscount);
                        Amount = Amount - DiscountAmount;
                    }
                    SaleDetail.TaxDetails = new List<ItemLevelSaleTaxDetail>();
                    float TaxPercentage = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value.ToString());
                    if (TaxPercentage > 0)
                    {
                        int j = 0;
                        if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null)
                        {
                            SaleDetail SalesDetail = SalesManager.GetSaleDetail((long)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value);
                            if (SalesDetail.TaxDetails.Count > 0)
                            {
                                foreach (ItemLevelSaleTaxDetail SaleTaxDetail in SalesDetail.TaxDetails)
                                {
                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + SaleTaxDetail.TaxRate;
                                    float TaxAmount = (float)(Amount * (SaleTaxDetail.TaxRate / 100));
                                    TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                    SaleTaxDetail.Amount = TaxAmount;
                                    SaleTaxDetail.Id = 0L;
                                    SaleTaxDetail.SaleDetails = null;
                                    SaleDetail.TaxDetails.Add(SaleTaxDetail);
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            DateTime CurrentDate = DatetimePickerSalesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) ? (DateTime)DatetimePickerSalesDate.Date : Global.getTransactionDate();
                            if (Product.UseHsnTax)
                            {
                                List<ItemSalesTaxMap> lItemSalesTaxMap = ItemTaxManager.Instance.GetItemTaxCodeByCode(Product.HSNCode, Global.Company.CompanyId).SalesTaxMapLocal.ToList();
                                foreach (ItemSalesTaxMap PTaxMap in lItemSalesTaxMap)
                                {
                                    if (PTaxMap.EffectiveFromDate <= CurrentDate && PTaxMap.EffectiveToDate >= CurrentDate)
                                    {
                                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId);
                                        if (CMap != null)
                                        {
                                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, CurrentDate.Date, (string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id) ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id))))
                                            {
                                                if (PTaxMap.TaxPercentage > 0)
                                                {
                                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                    float TaxAmount = (float)(Amount * (PTaxMap.TaxPercentage / 100));
                                                    TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                    ItemLevelSaleTaxDetail ItemSaleTaxDetail = new ItemLevelSaleTaxDetail();
                                                    ItemSaleTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId).AccountId;
                                                    ItemSaleTaxDetail.ItemCodeTaxMapId = PTaxMap.Id;
                                                    ItemSaleTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                    ItemSaleTaxDetail.Amount = TaxAmount;
                                                    ItemSaleTaxDetail.TaxSequence = j + 1;
                                                    SaleDetail.TaxDetails.Add(ItemSaleTaxDetail);
                                                }
                                                j++;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (CatalogItemSalesTaxMap PTaxMap in Product.SalesTax)
                                {
                                    if (PTaxMap.EffectiveFrom <= CurrentDate && PTaxMap.EffectiveTo >= CurrentDate)
                                    {
                                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId);
                                        if (CMap != null)
                                        {
                                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, CurrentDate.Date, (string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id) ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id))))
                                            {
                                                if (PTaxMap.TaxPercentage > 0)
                                                {
                                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                    float TaxAmount = (float)(Amount * (PTaxMap.TaxPercentage / 100));
                                                    TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                    ItemLevelSaleTaxDetail ItemSaleTaxDetail = new ItemLevelSaleTaxDetail();
                                                    ItemSaleTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId).AccountId;
                                                    ItemSaleTaxDetail.ItemTaxMapId = PTaxMap.Id;
                                                    ItemSaleTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                    ItemSaleTaxDetail.Amount = TaxAmount;
                                                    ItemSaleTaxDetail.TaxSequence = j + 1;
                                                    SaleDetail.TaxDetails.Add(ItemSaleTaxDetail);
                                                }
                                                j++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    lSaleEntry.SaleDetails.Add(SaleDetail);
                }
            }
            lSaleEntry.TaxDetails = new List<OrderLevelSaleTaxDetail>();
            int k = 0;
            foreach (CompanySalesTaxAccountMap TaxMap in Global.Company.SalesTaxAccountMaps)
            {
                if (TaxWisePercentageTotals[k] > 0)
                {
                    TotalTaxAmount = TotalTaxAmount + TaxWiseAmountTotals[k];
                    OrderLevelSaleTaxDetail SaleTaxDetail = new OrderLevelSaleTaxDetail();
                    SaleTaxDetail.TaxAccountId = TaxMap.AccountId;
                    SaleTaxDetail.TaxRate = TaxWisePercentageTotals[k];
                    SaleTaxDetail.Amount = TaxWiseAmountTotals[k];
                    SaleTaxDetail.TaxSequence = k + 1;
                    lSaleEntry.TaxDetails.Add(SaleTaxDetail);
                }
                k++;
            }
            int l = 1;
            if (TotalDiscountPercentage > 0)
            {
                OrderLevelSaleDiscount SaleDiscount = new OrderLevelSaleDiscount();
                SaleDiscount.DisccountType = DiscountType.PERCENT;
                SaleDiscount.Discount = TotalDiscountPercentage;
                SaleDiscount.DiscountAmount = TotalDiscountAmount;
                SaleDiscount.DiscountSequence = l;
                lSaleEntry.Discounts.Add(SaleDiscount);
            }
            if (DiscountAdditinalChargeGrid.AdditionalTransactions != null)
            {
                lSaleEntry.SaleAdditionalTransactions = new List<SaleAdditionalTransaction>();
                foreach (AdditionalTransaction AdditionalTransaction in DiscountAdditinalChargeGrid.AdditionalTransactions.ToList())
                {
                    SaleAdditionalTransaction SaleAdditionalTransaction = new SaleAdditionalTransaction();
                    SaleAdditionalTransaction.Action = AdditionalTransaction.Action;
                    SaleAdditionalTransaction.Amount = AdditionalTransaction.Amount;
                    SaleAdditionalTransaction.DisplayName = AdditionalTransaction.DisplayName;
                    SaleAdditionalTransaction.Name = AdditionalTransaction.Name;
                    SaleAdditionalTransaction.Sequence = AdditionalTransaction.Sequence;
                    SaleAdditionalTransaction.Type = AdditionalTransaction.Type;
                    SaleAdditionalTransaction.Value = AdditionalTransaction.Value;
                    SaleAdditionalTransaction.AccountId = AdditionalTransaction.AccountId;
                    lSaleEntry.SaleAdditionalTransactions.Add(SaleAdditionalTransaction);
                }
            }
            lSaleEntry.DisAmount = TotalDiscountAmount;
            lSaleEntry.TaxAmount = TotalTaxAmount;
            lSaleEntry.TotalAmount = double.Parse(LabelSalesFinalAmount.Text);
            //round off
            lSaleEntry.RoundOff = lSaleEntry.TotalAmount - TotalAmount;
            lSaleEntry.NetAmount = TotalAmount;
            return lSaleEntry;
        }

        double Rounds = (Global.Company.IdSpaces?.FirstOrDefault(x => x.EntryType == EntryType.SALES && x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate())?.RoundOff) ?? 0.0;
        private double RoundOff(double TotalAmount)
        {
            double Round = (Rounds / 2);
            double _roundoff = 0.00;
            if (Round > 0)
            {
                double mod = TotalAmount % (Round*2);
                if (mod >= Round)
                {
                    _roundoff = (Round*2) - mod;
                }
                else
                {
                    _roundoff = -mod;
                }
            }
            return _roundoff;
        }
        private void BtnReceivePayment_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPOSReceivePayment POSPayment = new FormPOSReceivePayment();
            POSPayment.SalePaymentOnLoad = true;
            POSPayment.SearchSalesId = long.Parse(TextBoxSalesId.Text);
            POSPayment.ShowDialog();
            LoadSaleEntry(long.Parse(TextBoxSalesId.Text));
            Cursor.Current = Cursors.Default;
        }
        private void TextBoxSalesEntryCustomer_TextChanged(object sender, EventArgs e)
        {
            TextBoxSalesEntryCustomer.Id = null;
        }
        private void BtnSalesNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnSalesSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                    GridViewSalesItem.CurrentCell.Selected = true;
                    GridViewSalesItem.BeginEdit(true);
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            NoteId = 0L;
            PatientId = 0L;
            OPId = 0L;
            SearchSalesId = 0L;
            EnableForm(true);
            GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
            GridViewSalesItem.CurrentCell.Selected = true;
            GridViewSalesItem.BeginEdit(true);
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSalesDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this sale is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, SalesReferenceNumber.Text), "Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long SalesID = Convert.ToInt64(TextBoxSalesId.Text);
                SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesID);
                if (SaleEntry != null)
                {
                    if (SaleEntry.Paid != 0)
                    {
                        ToolStripStatusLabelErrorPurchase.Text = NotAllowDeleteErrorText;
                        ResetTimmer();
                    }
                    else
                    {
                        bool DeleteResult = SalesManager.DeleteSaleEntry(SalesID);
                        if (DeleteResult)
                        {
                            AllAdditionalDetails = AllAdditionalDetails = new List<AdditionalDetail>();
                            AdditionalDetailsManager.Instance.AddAdditionalDetail(this.AllAdditionalDetails, SalesID.ToString(), AdditionalDetailSourceType.Sales);
                            //Unlock quote Delete sale entry                 
                            if (SaleEntry.SaleEntryId != null)
                            {
                                SaleEntry lSalesQuote = SalesManager.GetSaleEntry((long)SaleEntry.SaleEntryId);
                                lSalesQuote.isSaleLocked = false;
                                SalesManager.UpdateSaleEntry(lSalesQuote);
                            }
                            if (SaleEntry.PaymentId != null)
                            {
                                PaymentManager.Instance.DeletePayment((long)SaleEntry.PaymentId);
                            }
                            ResetForm();
                            NoteId = 0L;
                            PatientId = 0L;
                            OPId = 0L;
                            EnableForm(true);
                            GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                            GridViewSalesItem.CurrentCell.Selected = true;
                            GridViewSalesItem.BeginEdit(true);
                            DirtyFlag(false);
                        }
                        else
                        {
                            MessageBox.Show(DeleteErrorText);
                        }
                    }
                }
                else
                {
                    DisplaySystemErrorPerformCancel("Somthing went wrong, the selected sale is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
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
            BtnSalesCancel.PerformClick();
            return;
        }
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (SalesManager.GetSaleEntry(long.Parse(TextBoxSalesId.Text)) != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                PrinterSetup.SalePrintSetup(long.Parse(TextBoxSalesId.Text), true, Entrytype.SALE);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this sale is still valid.");
                return;
            }
        }
        private void BtnSalesPrint_Click(object sender, EventArgs e)
        {
            if (SalesManager.GetSaleEntry(long.Parse(TextBoxSalesId.Text)) != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                PrinterSetup.SalePrintSetup(long.Parse(TextBoxSalesId.Text), false, Entrytype.SALE);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this sale is still valid.");
                return;
            }
        }
        private void BtnSalesCancel_Click(object sender, EventArgs e)
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
            NoteId = 0L;
            PatientId = 0L;
            OPId = 0L;
            SearchSalesId = 0L;
            EnableForm(true);
            GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
            GridViewSalesItem.CurrentCell.Selected = true;
            GridViewSalesItem.BeginEdit(true);
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private Refered GenerateReference(Refered lRefered)
        {
            Refered Refered = ReferedManager.Instance.GetReferedByName(lRefered.Name, Global.Company.CompanyId);
            if (Refered == null)
            {
                Refered = ReferedManager.Instance.AddRefered(lRefered);
            }
            return Refered;
        }
        private void BtnSalesSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    SaleEntry lSales = GetSaleEntryFromForm();
                    if (lSales.SoldById == null && !string.IsNullOrEmpty(ComboBoxSoldby.Text))
                    {
                        Refered lRefered = new Refered();
                        lRefered.Name = ComboBoxSoldby.Text;
                        lRefered.CompanyId = Global.Company.CompanyId;
                        lSales.SoldById = GenerateReference(lRefered).Id;
                    }
                    if (lSales.ReferedById == null && !string.IsNullOrEmpty(ComboBoxreferedby.Text))
                    {
                        Refered lRefered = new Refered();
                        lRefered.Name = ComboBoxreferedby.Text;
                        lRefered.CompanyId = Global.Company.CompanyId;

                        lSales.ReferedById = GenerateReference(lRefered).Id;
                    }
                    SaleEntry lSalesFromDB = null;
                    lSales.Balance = lSales.TotalAmount;
                    lSales.Paid = 0F;
                    if (lSales.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.SALES, (DateTime)DatetimePickerSalesDate.Date);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lSales.RefNumber = RefNumber;
                            lSalesFromDB = new SaleEntry();
                            try
                            {
                                if (lSales.EntryType == Entrytype.QUOTE)
                                {
                                    // lock quote generate sale entry
                                    lSales.SaleEntryId = SearchSalesId;
                                    lSales.EntryType = Entrytype.SALE;

                                    SaleEntry lSalesQuote = SalesManager.GetSaleEntry(SearchSalesId);
                                    lSalesQuote.isSaleLocked = true;
                                    SalesManager.UpdateSaleEntry(lSalesQuote);
                                }
                                lSalesFromDB = SalesManager.AddSaleEntry(lSales);                               
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
                        SaleEntry SaleEntryInfo = SalesManager.GetSaleEntry(lSales.Id);
                        if (SaleEntryInfo != null)
                        {
                            if (SaleEntryInfo.TotalAmount > lSales.TotalAmount)
                            {
                                lSales.Balance = SaleEntryInfo.Balance - (SaleEntryInfo.TotalAmount - lSales.TotalAmount);
                            }
                            else
                            {
                                lSales.Balance = SaleEntryInfo.Balance + (lSales.TotalAmount - SaleEntryInfo.TotalAmount);
                            }
                            lSales.Paid = SaleEntryInfo.Paid;
                            lSalesFromDB = new SaleEntry();
                            lSalesFromDB = SalesManager.UpdateSaleEntry(lSales);
                        }
                        else
                        {
                            DisplaySystemErrorPerformCancel("Somthing went wrong, the selected sale is not valid.");
                            return;
                        }
                    }
                    TextBoxSalesId.Text = lSalesFromDB.Id.ToString();
                    TextBoxSalesType.Text = lSalesFromDB.EntryType.ToString();
                    SalesReferenceNumber.Text = lSalesFromDB.RefNumber;
                    if (lSalesFromDB != null)
                    {
                        //add additinal details
                        AdditionalDetailsManager.Instance.AddAdditionalDetail(this.AllAdditionalDetails, lSalesFromDB.Id.ToString(), AdditionalDetailSourceType.Sales);
                        EnableForm(false);
                        LoadProductCombo();
                        LoadSaleEntry(lSalesFromDB.Id);
                        BtnSalesPrint.Select();
                    }
                    ToolStripStatusLabelErrorPurchase.Text = SaveSuccessText;
                    DirtyFlag(false);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void BtnSalesExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (NoteId != 0L && Global.Company.PatientPurchaseAccount == null)
            {
                TextBoxSalesEntryCustomer.Select();
                ToolStripStatusLabelErrorPurchase.Text = "Please configure patient purchase account.";
                ResetTimmer();
                return false;
            }
            if (ComboBoxSaleInventoryLocation.SelectedIndex < 0)
            {
                ComboBoxSaleInventoryLocation.Select();
                ToolStripStatusLabelErrorPurchase.Text = "Please select stock location.";
                ResetTimmer();
                return false;
            }
            if ((YesNoRbtSalesMethod.Checked) &&
                (string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Text.Trim()) ||
                string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id.Trim()) ||
                AccountManager.Instance.GetAccountById(long.Parse(TextBoxSalesEntryCustomer.Id)) == null))
            {
                TextBoxSalesEntryCustomer.Select();
                ToolStripStatusLabelErrorPurchase.Text = ChooseCustomerErrorMsg;
                ResetTimmer();
                return false;
            }
            if ((YesNoRbtSalesMethod.Checked) && (!string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Text.Trim()) ||
                !string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id.Trim())))
            {
                Supplier Supplier = null;
                Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxSalesEntryCustomer.Id));
                if (Customer == null)
                {
                    Supplier = SupplierManager.Instance.GetSupplierById(long.Parse(TextBoxSalesEntryCustomer.Id));
                }
                if ((Customer != null && Global.Company.CompanyCustomerLicenseMaster.Count > 0 && Customer.CustomerLicenceDetail.Count == 0)
                        || (Supplier != null && Global.Company.CompanySupplierLicenseMaster.Count > 0 && Supplier.SupplierLicenceDetail.Count == 0))
                {
                    TextBoxSalesEntryCustomer.Select();
                    ToolStripStatusLabelErrorPurchase.Text = UpdateCustomerLicenceErrorMsg;
                    ResetTimmer();
                    return false;
                }
            }
            if (DatetimePickerSalesDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerSalesDate.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterSaleEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewSalesItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value == null)
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, i];
                        GridViewSalesItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value == null
                         || string.IsNullOrEmpty(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString())
                         )
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.UOM, i];
                        GridViewSalesItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewSalesItem.Columns[(int)SaleEntryTableColumn.UOM].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString());
                    double Free = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString());
                    for (int j = 3; j < 12; j++)
                    {
                        double Price = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value != null ? double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value.ToString()) : 0;
                        double Oprice = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value != null ? double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value.ToString()) : 0;

                        if ((j == (int)SaleEntryTableColumn.QTY || j == (int)SaleEntryTableColumn.FREE) && !(Quantity <= 0 && Free <= 0)) { continue; }
                        if ((!((bool)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ISBAT].Value)) && (j == (int)SaleEntryTableColumn.EXPDATE || j == (int)SaleEntryTableColumn.BATNO)) { continue; }
                        if (j == (int)SaleEntryTableColumn.FREE) { continue; }
                        if (j == (int)SaleEntryTableColumn.PRICE && !(Oprice <= 0 && Price <= 0)) { continue; }
                        if (j == (int)SaleEntryTableColumn.OPRICE && !(Oprice <= 0 && Price <= 0))
                        { j = j + 2; continue; }
                        if ((j == 7 || j == 8) && Quantity <= 0 && Free <= 0 && Oprice <= 0 && Price <= 0)
                        { if (j == 8) { j = j + 2; } continue; }
                        if (j != 5 && j != 6 && j != 11 && (GridViewSalesItem.Rows[i].Cells[j].Value == null || GridViewSalesItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewSalesItem.Rows[i].Cells[j].Value.ToString()) <= 0))
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[(j == 7 && (Global.Company.BusinessType != BuisnessType.Hospital && Global.Company.BusinessType != BuisnessType.Wholesale) ? 8 : j), i];
                            GridViewSalesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewSalesItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATCHID].Value == null)
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.BATNO, i];
                            GridViewSalesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewSalesItem.Columns[(int)SaleEntryTableColumn.BATNO].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null && ((DateTime)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.EXPDATE].Value) < ((DateTime)DatetimePickerSalesDate.Date))
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.BATNO, i];
                            GridViewSalesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemBatchExpire, GridViewSalesItem.Columns[(int)SaleEntryTableColumn.BATNO].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 11 && GridViewSalesItem.Rows[i].Cells[j].Value != null && float.Parse(GridViewSalesItem.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[j, i];
                            GridViewSalesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewSalesItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 8)
                        {
                            j = j + 2;
                        }
                    }
                    long PId = (long)GridViewSalesItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ID].Value;
                    Inventory Inventory = null;
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                    {
                        Product Product = CatalogProductManager.Instance.GetProductInfoById(PId);
                        if (Product.ShouldMaintainInventory != null && (bool)Product.ShouldMaintainInventory)
                        {
                            bool IsMultipleRow = false;
                            double CompineQty = 0.00;
                            double CompineOldQty = 0.00;
                            if (!Global.Company.CompanySalesSetup.CombineItem)
                            {
                                for (int k = 0; k < Count - 1; k++)
                                {
                                    var Batch1 = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATNO].Value;
                                    if (i != k && GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.ID].Value!=null && PId == (long)GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.ID].Value)
                                    {
                                        var Batch2 = GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.BATNO].Value;
                                        string lUom = GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                                        double lOldQty = 0;
                                        if (GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null && TextBoxSalesType.Text != "QUOTE")
                                        {
                                            long PDetailId = (long)GridViewSalesItem.Rows[k].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value;
                                            SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetail(PDetailId);
                                            if (SaleDetail != null)
                                            {
                                                lOldQty = (SaleDetail.Quantity + SaleDetail.FreeQuantity);
                                                if (SaleDetail.Uom != lUom)
                                                {
                                                    if (Product != null)
                                                    {
                                                        if (Product.RetailUOM == lUom)
                                                        {
                                                            lOldQty = (lOldQty * Product.RetailXFactor);
                                                        }
                                                        else
                                                        {
                                                            lOldQty = (lOldQty / Product.RetailXFactor);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (Batch1 == Batch2)
                                        {
                                            double lQuantity = (GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString());
                                            double lFree = (GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString());
                                            double lNewQty = (lQuantity + lFree);
                                            IsMultipleRow = lNewQty > 0 ? true : false;
                                            CompineQty += lNewQty;
                                            CompineOldQty += lOldQty;
                                        }
                                    }
                                }
                            }
                            string Uom = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                            double OldQty = 0;
                            if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null && TextBoxSalesType.Text != "QUOTE")
                            {
                                long PDetailId = (long)GridViewSalesItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.PurchDetailID].Value;
                                SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetail(PDetailId);
                                if (SaleDetail != null)
                                {
                                    OldQty = (SaleDetail.Quantity + SaleDetail.FreeQuantity);
                                    if (SaleDetail.Uom != Uom)
                                    {
                                        if (Product != null)
                                        {
                                            if (Product.RetailUOM == Uom)
                                            {
                                                OldQty = (OldQty * Product.RetailXFactor);
                                            }
                                            else
                                            {
                                                OldQty = (OldQty / Product.RetailXFactor);
                                            }
                                        }
                                    }
                                }
                            }
                            OldQty += CompineOldQty;
                            double NewQty = (Quantity + Free + CompineQty);
                            double Stock;
                            if ((bool)GridViewSalesItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                            {
                                long BId = (long)GridViewSalesItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value;
                                Stock = StockByXfactorBatch(BId, Uom);
                                if (OldQty < NewQty && Math.Round(Stock, MidpointRounding.AwayFromZero) < (Math.Round((NewQty - OldQty), MidpointRounding.AwayFromZero)))
                                {
                                    GridViewSalesItem.Select();
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)PurchaseEntryTableColumn.QTY, i];
                                    GridViewSalesItem.BeginEdit(true);
                                    ToolStripStatusLabelErrorPurchase.Text = (IsMultipleRow ? "Already Entered the Available Qty in Another row. Please check!" : "Please Enter valid Quantity") + ", Available Quantity is " + (StockByXfactorBatch(BId, Uom) + OldQty);
                                    ResetTimmer();
                                    return false;
                                }
                            }
                            else
                            {
                                Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, LocationId);
                                if (Inventory == null)
                                {
                                    GridViewSalesItem.Select();
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)PurchaseEntryTableColumn.PRODUCT, i];
                                    GridViewSalesItem.BeginEdit(true);
                                    ToolStripStatusLabelErrorPurchase.Text = "No stock Available";
                                    ResetTimmer();
                                    return false;
                                }
                                else if (OldQty < NewQty && (StockByXfactor(PId, Uom) < (NewQty - OldQty)))
                                {
                                    GridViewSalesItem.Select();
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)PurchaseEntryTableColumn.QTY, i];
                                    GridViewSalesItem.BeginEdit(true);
                                    ToolStripStatusLabelErrorPurchase.Text = (IsMultipleRow ? "Already Entered the Available Qty in Another row. Please check!" : "Please Enter valid Quantity") + ", Available Quantity is " + (StockByXfactor(PId, Uom) + OldQty);
                                    ResetTimmer();
                                    return false;
                                }
                            }
                        }
                    }
                    bool isBatch = (bool)GridViewSalesItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value;
                    Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, LocationId);
                    if (Inventory == null)
                    {
                        if (!isBatch && LocationId != 0L)
                        {
                            Inventory lInventory = new Inventory();
                            lInventory.OpeningStock = 0;
                            lInventory.CompanyId = Global.Company.CompanyId;
                            lInventory.ProductId = PId;
                            lInventory.InventoryLocationId = LocationId;
                            InventoryLocationManager.Instance.AddInventory(lInventory);
                        }
                        else
                        {
                            ToolStripStatusLabelErrorPurchase.Text = Grid_InvalidBatchErrorMsg;
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                GridViewSalesItem.Select();
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, 0];
                GridViewSalesItem.BeginEdit(true);
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
            if (float.Parse(LabelSalesFinalAmount.Text) < 0)
            {
                GridViewSalesItem.Select();
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, 0];
                GridViewSalesItem.BeginEdit(true);
                ToolStripStatusLabelErrorPurchase.Text = InvalidSaleEntryErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxSaleInventoryLocation.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorPurchase.Text = SelectInventoryLoactionErrorMsg;
                ComboBoxSaleInventoryLocation.Select();
                ResetTimmer();
                return false;
            }
            if (TextBoxSalesEntryCustomer.Id != null && !string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id.Trim()))
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxSalesEntryCustomer.Id));
                if (customer != null)
                {
                    if (customer.LockBill == true && YesNoRbtSalesMethod.Checked == true)
                    {
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(NotAlowedErrMsg, customer);
                        return false;
                    }
                }
                //if (customer != null)
                //{
                //    double ledgerAmount = 0.00;
                //    Account account = AccountManager.Instance.GetAccountDetailByName(customer.Name, Global.Company.CompanyId);
                //    List<DayBook> dayBookDetails = AccountManager.Instance.GetDaybookDetailByAccountId(account.Id, Global.Company.CompanyId);
                //    if (dayBookDetails.Count != 0)
                //    {
                //        foreach (DayBook DayBook in dayBookDetails)
                //        {
                //            ledgerAmount += DayBook.Amount;
                //        }
                //    }
                //    double ClosingBalance = ledgerAmount + account.Balance;
                //    double CurrentSaleBalance = double.Parse(LabelSalesFinalAmount.Text);

                //    if (YesNoRbtSalesMethod.Checked == true)
                //    {
                //        if (CurrentSaleBalance <= customer.PaymentLimit && ClosingBalance < customer.PaymentLimit && CurrentSaleBalance <= (customer.PaymentLimit - ClosingBalance))
                //        {
                //        }
                //        else
                //        {
                //            ToolStripStatusLabelErrorPurchase.Text = string.Format(ExceedCreditLimitErrMsg, customer.Name) + Math.Abs((double)customer.PaymentLimit - ClosingBalance);
                //            return false;
                //        }
                //    }
                //}
            }
            return true;
        }
        private void ResetForm()
        {
            
            if (FormLocationId != 0L)
            {
                InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(FormLocationId);
                if (Location != null)
                {
                    ComboBoxSaleInventoryLocation.SelectedIndex = ComboBoxSaleInventoryLocation.FindStringExact(Location.Name);
                }
            }
            AllAdditionalDetails = new List<AdditionalDetail>();
            ComboBoxreferedby.SelectedIndex = -1;
            ComboBoxSoldby.SelectedIndex = -1;
            TextBoxSalesType.ResetText();
            SalesReferenceNumber.Text = "000000";
            TextBoxSalesSearch.TextBox.ResetText();
            TextBoxSalesQuotesSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorPurchase.Text = "";
            TextBoxSalesId.ResetText();
            TextBoxSearchPrescription.TextBox.ResetText();
            TextBoxSalesType.ResetText();
            TextBoxSalesCustomerAddress.ResetText();
            TextBoxSalesMemo.ResetText();
            DatetimePickerSalesDate.Format = Global.Company.DateFormat;
            DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            YesNoRbtSalesMethod.Checked = (Global.Company.CompanySalesSetup.DefaultSalesType == SaleMethod.Cash ? false : true);
            PrevSalesReferenceNumber.Text = CompanyManager.Instance.GetSalePrevRef(Global.Company, Entrytype.SALE, (DateTime)DatetimePickerSalesDate.Date);
            TextBoxSalesEntryCustomer.ResetText();
            GridViewSalesItem.Rows.Clear();
            GridViewSalesItem.Rows.Add();
            DiscountAdditinalChargeGrid.GridType = GridType.Sales;
            DiscountAdditinalChargeGrid.Clear();
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LabelSalesFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxSalesCustomerAddress.ResetText();
            SaleProductDetails.Clear();
            labelRoundOff.Text = "Round Off (" + TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision) + ")";
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["SalesAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["SalesPrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["OverridePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["SalesTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["SalesDicount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
            DataGridViewCurrencyColumn currencyColumn5 = (DataGridViewCurrencyColumn)GridViewPurchaseItemTotal.Columns["Value"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces5)) currencyColumn5.DecimalPlaces = decimalPlaces5;
        }
        private SaleEntry GetSavedSale()
        {
            SaleEntry SaleEntry = null;
            if (!string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                SaleEntry = SalesManager.GetSaleEntry(long.Parse(TextBoxSalesId.Text));
            }
            return SaleEntry;
        }
        private void EnableForm(Boolean enable)
        {

            BtnSalesDelete.Enabled = true;
            BtnSalesSave.Enabled = true;
            YesNoRbtSalesMethod.Enabled = true;
            DatetimePickerSalesDate.ReadOnly = false;
            DatetimePickerSalesDate.TabStop = true;
            TextBoxSalesEntryCustomer.ReadOnly = YesNoRbtSalesMethod.Checked;
            TextBoxSalesEntryCustomer.TabStop = !YesNoRbtSalesMethod.Checked;
            TextBoxSalesCustomerAddress.ReadOnly = false;
            TextBoxSalesCustomerAddress.TabStop = true;
            TextBoxSalesMemo.ReadOnly = false;
            TextBoxSalesMemo.TabStop = true;
            ComboBoxSaleInventoryLocation.Visible = true;
            BtnSalesNewCustomer.Enabled = true;
            BtnSalesSearchCustomer.Enabled = true;
            GridViewSalesItem.ReadOnly = false;
            GridViewSalesItem.TabStop = true;
            DiscountAdditinalChargeGrid.Enabled = true;
            GridViewSalesItem.ScrollBars = ScrollBars.Vertical;
            if (Global.Company.BusinessType == BuisnessType.Hospital || Global.Company.BusinessType == BuisnessType.Pharmacy)
            {
                TextBoxSearchPrescription.Visible = true;
                toolStripLabel3.Visible = true;
                toolStripSeparator2.Visible = true;
                BtnPrescriptionBySearch.Visible = true;
            }
            else
            {
                TextBoxSearchPrescription.Visible = false;
                toolStripLabel3.Visible = false;
                toolStripSeparator2.Visible = false;
                BtnPrescriptionBySearch.Visible = false;
            }
            if (enable)
            {
                BtnSalesNew.Enabled = !enable;
                BtnSalesDelete.Enabled = !enable;
                BtnSalesPrint.Enabled = !enable;
                BtnExportPdf.Enabled = !enable;
                BtnSalesCancel.Enabled = enable;
                BtnSalesSave.Enabled = enable;
                BtnSalesReturn.Enabled = !enable;
                BtnReceivePayment.Enabled = !enable;

            }
            else
            {
                BtnSalesNew.Enabled = !enable;
                BtnSalesDelete.Enabled = !enable;
                BtnSalesPrint.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? enable : !enable;
                BtnExportPdf.Enabled = !enable;
                BtnSalesCancel.Enabled = !enable;
                BtnSalesSave.Enabled = !enable;
                BtnSalesReturn.Enabled = !enable;
                BtnReceivePayment.Enabled = !enable;

            }
            SaleEntry Entry = GetSavedSale();
            if (TextBoxSalesType.Text == "QUOTE" && Entry == null)
            {
                BtnReceivePayment.Enabled = enable;
                BtnSalesReturn.Enabled = enable;
                BtnSalesNew.Enabled = enable;
                BtnSalesDelete.Enabled = enable;
                BtnSalesPrint.Enabled = enable;
                BtnExportPdf.Enabled = enable;
            }
            else if (TextBoxSalesType.Text == "QUOTE")
            {
                BtnReceivePayment.Enabled = enable;
                BtnSalesReturn.Enabled = enable;
                LableStockLocation.Visible = false;
            }
            if (Entry != null)
            {
                if (Entry.SaleMethod == SaleMethod.Credit)
                {
                    BtnReceivePayment.Enabled = false;
                }
                if (Entry.EntryType == Entrytype.SALE && (Entry.isSaleLocked || Entry.isPaymentReceived))
                {
                    if (Entry.isSaleLocked)
                    {
                        BtnSalesDelete.Enabled = enable;
                    }
                    BtnSalesSave.Enabled = enable;
                    YesNoRbtSalesMethod.Enabled = enable;
                    ComboBoxSaleInventoryLocation.Visible = enable;
                    DatetimePickerSalesDate.ReadOnly = !enable;
                    DatetimePickerSalesDate.TabStop = enable;
                    TextBoxSalesEntryCustomer.ReadOnly = !enable;
                    TextBoxSalesEntryCustomer.TabStop = enable;
                    TextBoxSalesCustomerAddress.ReadOnly = !enable;
                    TextBoxSalesCustomerAddress.TabStop = enable;
                    TextBoxSalesMemo.ReadOnly = !enable;
                    TextBoxSalesMemo.TabStop = enable;
                    BtnSalesNewCustomer.Enabled = enable;
                    BtnSalesSearchCustomer.Enabled = enable;
                    GridViewSalesItem.ReadOnly = !enable;
                    GridViewSalesItem.TabStop = enable;
                    DiscountAdditinalChargeGrid.Enabled = enable;
                }
            }
            if (NoteId != 0L)
            {
                BtnSalesNewCustomer.Enabled = false;
                BtnSalesSearchCustomer.Enabled = false;
                TextBoxSalesEntryCustomer.ReadOnly = true;
                TextBoxSalesEntryCustomer.TabStop = false;
            }
            else
            {
                BtnSalesNewCustomer.Enabled = true;
                BtnSalesSearchCustomer.Enabled = true;
                TextBoxSalesEntryCustomer.ReadOnly = false;
                TextBoxSalesEntryCustomer.TabStop = true;
            }
            if (!Global.Company.CompanySalesSetup.IsReceivePayment)
            {
                BtnReceivePayment.Enabled = false;
            }
        }
        private void LoadSaleEntry(long SalesId)
        {
            ResetForm();
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry != null)
            {
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteBySaleId(SalesId);
                if (Note != null)
                {
                    NoteId = Note.Id;
                    PatientId = Note.PatientId;
                    if (Note.OpRegistrationId != null)
                    {
                        OPId = (long)Note.OpRegistrationId;
                    }
                }
                if (SaleEntry.EntryType == Entrytype.SALE)
                {
                    SalesReferenceNumber.Text = SaleEntry.RefNumber;
                    TextBoxSalesId.Text = SaleEntry.Id.ToString();
                }
                TextBoxSalesType.Text = SaleEntry.EntryType.ToString();
                if (SaleEntry.AccountsId == null)
                {
                    TextBoxSalesEntryCustomer.Text = SaleEntry.CustomerName;
                }
                else
                {
                    TextBoxSalesEntryCustomer.Text = SaleEntry.Account.Name;
                    TextBoxSalesEntryCustomer.Id = SaleEntry.Account.Id.ToString();
                }
                TextBoxSalesCustomerAddress.Text = SaleEntry.CustomerAddress.Replace("\n", "").Replace(" ,", "" + System.Environment.NewLine).Replace(", ", "" + System.Environment.NewLine).Replace(",", "," + System.Environment.NewLine);
                if (SaleEntry.InventoryLocationId != null && SaleEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)SaleEntry.InventoryLocationId);
                    if (Location != null)
                    {
                        ComboBoxSaleInventoryLocation.SelectedIndex = ComboBoxSaleInventoryLocation.FindStringExact(Location.Name);
                    }
                }
                if (SaleEntry.SoldBy != null)
                {
                    ComboBoxSoldby.SelectedIndex = ComboBoxSoldby.FindStringExact(SaleEntry.SoldBy.Name);
                }
                if (SaleEntry.ReferedBy != null)
                {
                    ComboBoxreferedby.SelectedIndex = ComboBoxreferedby.FindStringExact(SaleEntry.ReferedBy.Name);
                }
                TextBoxSalesMemo.Text = !string.IsNullOrEmpty(SaleEntry.Memo) ? SaleEntry.Memo.Replace("\n", System.Environment.NewLine) : string.Empty;
                DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                YesNoRbtSalesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    GridViewSalesItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    foreach (var SaleDetail in SaleEntry.SaleDetails.OrderBy(x => x.Id))
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SNO].Value = i + 1;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                        (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lSaleDetail.Product.RetailUOM);
                        (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lSaleDetail.Product.WholesaleUOM);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value = lSaleDetail.Product.RetailUOM == lSaleDetail.Uom ? lSaleDetail.Product.RetailUOM : lSaleDetail.Product.WholesaleUOM;
                        if (SaleEntry.EntryType == Entrytype.QUOTE)
                        {
                            if (!lSaleDetail.isBatch && lSaleDetail.BatchNo != null)
                            {
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATNO].Value = lSaleDetail.BatchNo;
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = lSaleDetail.ExpDate;
                            }
                        }
                        else
                        {
                            if (lSaleDetail.isBatch)
                            {
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATNO].Value = lSaleDetail.BatchNo;
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = lSaleDetail.ExpDate;
                            }
                        }
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.MSRP].Value = lSaleDetail.Msrp;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value = lSaleDetail.Quantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value = lSaleDetail.FreeQuantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value = lSaleDetail.Price;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value = lSaleDetail.OverridePrice;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value = ProductTaxPercentage(lSaleDetail.TaxDetails);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value = (lSaleDetail.Discounts.Count > 0) ? lSaleDetail.Discounts.First().Discount : 0.00;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = lSaleDetail.Amount;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value = lSaleDetail.ProductId;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ISBAT].Value = lSaleDetail.isBatch;
                        //for tax
                        if (SaleEntry.EntryType != Entrytype.QUOTE)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = lSaleDetail.Id;
                        }
                        if (lSaleDetail.isBatch && lSaleDetail.BatchNo != null)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lSaleDetail.ProductId, lSaleDetail.BatchNo, (long)SaleEntry.InventoryLocationId).Id;
                        }
                        i++;
                    }
                    Row_Added();
                    ReSequence();
                    if (SaleEntry.SaleAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = SaleEntry.SaleAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                ComputeFormTotal();
                //for additional details
                IList<AdditionalDetail> AdditionalDetail = AdditionalDetailsManager.Instance.GetAllAdditionalDetail(SaleEntry.Id.ToString(), AdditionalDetailSourceType.Sales, Global.Company.CompanyId);
                if (AdditionalDetail != null)
                {
                    this.AllAdditionalDetails = AdditionalDetail;
                }
                if (SaleEntry.EntryType == Entrytype.QUOTE || !SaleEntry.isSaleLocked)
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                    GridViewSalesItem.CurrentCell.Selected = true;
                    GridViewSalesItem.BeginEdit(true);
                }
                else
                {
                    BtnSalesPrint.Select();
                }
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                SearchSalesId = 0L;
                DisplaySystemErrorPerformCancel("The selected entry is not available anymore");
                return;
            }
        }

        private double ProductTaxPercentage(IList<ItemLevelSaleTaxDetail> SaleTaxDetails)
        {
            double TaxTotal = 0.00;
            foreach (ItemLevelSaleTaxDetail Tax in SaleTaxDetails)
            {
                TaxTotal = TaxTotal + Tax.TaxRate;
            }
            return TaxTotal;
        }
        private double ProductTaxPercentage(Product Product)
        {
            double TaxTotal = 0.00;
            DateTime CurrentDate = DatetimePickerSalesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) ? (DateTime)DatetimePickerSalesDate.Date : Global.getTransactionDate();
            List<ItemSalesTaxMap> lItemSalesTaxMap = ItemTaxManager.Instance.GetItemTaxCodeByCode(Product.HSNCode, Global.Company.CompanyId) == null ? null : ItemTaxManager.Instance.GetItemTaxCodeByCode(Product.HSNCode, Global.Company.CompanyId).SalesTaxMapLocal.ToList();
            if (lItemSalesTaxMap != null)
            {
                foreach (ItemSalesTaxMap PTaxMap in lItemSalesTaxMap)
                {
                    if (PTaxMap.EffectiveFromDate <= CurrentDate && PTaxMap.EffectiveToDate >= CurrentDate)
                    {
                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId);
                        if (CMap != null)
                        {
                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, CurrentDate.Date, (string.IsNullOrEmpty(TextBoxSalesEntryCustomer.Id) ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id))))
                            {
                                if (PTaxMap.TaxPercentage > 0)
                                {
                                    TaxTotal = TaxTotal + PTaxMap.TaxPercentage;
                                }
                            }
                        }
                    }
                }
            }
            return TaxTotal;
        }
        private double ProductTaxPercentage(List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMaps)
        {
            double TaxTotal = 0.00;
            DateTime CurrentDate = DatetimePickerSalesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) ? (DateTime)DatetimePickerSalesDate.Date : Global.getTransactionDate();
            foreach (CatalogItemSalesTaxMap Map in CatalogItemSalesTaxMaps)
            {
                if (Map != null)
                {
                    if (Map.EffectiveFrom <= CurrentDate && Map.EffectiveTo >= CurrentDate)
                    {
                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)Map.SalesTaxMapId);
                        if (CMap != null)
                        {
                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, CurrentDate, (CustomerId == 0L ? 0L : CustomerId)))
                            {
                                TaxTotal = TaxTotal + Map.TaxPercentage;
                            }
                        }
                    }
                }
            }
            return TaxTotal;
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewSalesItem.Rows.Count; i++)
            {
                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SNO].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewSalesItem.Rows.Count - 1; i++)
            {
                double Quantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()));
                double FreeQuantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString()));
                double Pprice = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value.ToString()));
                double Oprice = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value.ToString()));
                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;
                Amount = (Pprice * Quantity);
                if (Amount > 0)
                {
                    double DiscountPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value.ToString()));
                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;
                    double TaxPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value.ToString()));
                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAX].Value = TaxAmount;
                    Amount = Amount + TaxAmount;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = Amount;
                    TotalAmount = TotalAmount + Amount;
                }
                else
                {
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                }
            }
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            DiscountAdditinalChargeGrid.Quantity = TotalQuantity;
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;
            OverallTotal();
        }
        private void OverallTotal()
        {
            double RoundedTotal = DiscountAdditinalChargeGrid.OutputAmount;
            double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
            labelRoundOff.Text = "Round Off (" + (RoundoffAmount == 0 ? 0 : RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + ")";
            DiscountAdditinalChargeGrid.OutputAmount = RoundedTotal + RoundoffAmount;
            LabelSalesFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerSales.Stop();
            TimerSales.Start();
        }
        private void TimerSales_Tick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabelErrorPurchase.Visible = !this.ToolStripStatusLabelErrorPurchase.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerSales.Stop();
                ToolStripStatusLabelErrorPurchase.Visible = true;
            }
        }
        private void BtnSalesQuotesSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria(TextBoxSalesQuotesSearch, Entrytype.QUOTE))
            {
                Cursor.Current = Cursors.WaitCursor;
                RecentSaless(Entrytype.QUOTE, TextBoxSalesQuotesSearch);
                if (SearchSalesId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnSalesSave_Click(sender, e);
                                LoadSaleEntry(SearchSalesId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadSaleEntry(SearchSalesId);
                        }
                    }
                    else
                    {
                        LoadSaleEntry(SearchSalesId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnSalesSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RecentSaless(Entrytype.SALE, TextBoxSalesSearch);
            if (SearchSalesId != 0)
            {
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnSalesSave_Click(sender, e);
                            LoadSaleEntry(SearchSalesId);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadSaleEntry(SearchSalesId);
                    }
                }
                else
                {
                    LoadSaleEntry(SearchSalesId);
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private bool isValidSearchCriteria(ToolStripTextBox TextBox, Entrytype Type)
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBox.Text.Trim()))
            {
                if (Type == Entrytype.SALE)
                {
                    ToolStripStatusLabelErrorPurchase.Text = SearchBoxEmptyErrorMsg;
                }
                else if (Type == Entrytype.QUOTE)
                {
                    ToolStripStatusLabelErrorPurchase.Text = QuoteBoxEmptyErrorMsg;
                }
                TextBox.TextBox.Focus();
                return false;
            }
            return true;
        }
        private void RecentSaless(Entrytype Type, ToolStripTextBox TextBox)
        {
            if (isValidSearchCriteria(TextBox, Type))
            {
                String SearchText = TextBox.Text.Trim();
                IList<SaleEntry> SalesInfo = null;
                if (string.IsNullOrEmpty(SearchText))
                {
                    SalesInfo = SalesManager.GetRecentSaleEntrys(Global.Company.CompanyId, Type);
                }
                else if (TextUtils.isAmount(SearchText))
                {
                    double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                    double Amount = (Rounds > 0 ? RoundOff(SearchAmount) : 0 + SearchAmount);
                    SalesInfo = SalesManager.GetSaleEntryByAmount(Amount, SearchText, Global.Company.CompanyId, Type);
                }
                else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
                {
                    DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                    SalesInfo = SalesManager.GetSaleEntryByDate((DateTime)Date, Global.Company.CompanyId, Type);
                }
                else
                {
                    SalesInfo = SalesManager.GetSaleEntryByCustomerName(SearchText, Global.Company.CompanyId, Type);
                }
                if (SalesInfo != null && SalesInfo.Count > 0)
                {
                    LoadSales(SalesInfo);
                }
                else
                {
                    SearchSalesId = 0L;
                    ToolStripStatusLabelErrorPurchase.Text = SaleSearchOutput;
                    if (Type == Entrytype.QUOTE)
                    {
                        ToolStripStatusLabelErrorPurchase.Text = SaleQuoteSearchOutput;
                    }
                }
            }
        }
        public void LoadSales(IList<SaleEntry> SalesInfo)
        {
            SearchSalesId = 0L;
            FormRecentSales FormRecentSales = new FormRecentSales(this);
            FormRecentSales.SaleEntryInfo = SalesInfo;
            FormRecentSales.ShowDialog();
        }
        private void OverridePriceAlertMsg(int RIndex)
        {
            DialogResult Result = MessageBox.Show("Max retail price not less than retail price \n Do you want to change it.", "Change Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.OPRICE, RIndex];
                }));
            }
            else
            {
                GridViewSalesItem.Rows[RIndex].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0;
                GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.DISP, RIndex];
                }));
            }
        }
        public double Oprice = 0.00;
        public double Price = 0.00;
        private void GridViewSalesItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex == (int)SaleEntryTableColumn.QTY ||
                e.ColumnIndex == (int)SaleEntryTableColumn.FREE ||
                e.ColumnIndex == (int)SaleEntryTableColumn.PRICE ||
                e.ColumnIndex == (int)SaleEntryTableColumn.DISP ||
                e.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
            {
                Row_Added();
            }
            if (e.ColumnIndex == (int)SaleEntryTableColumn.OPRICE && GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ID].Value != null)
            {
                Product lProduct = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ID].Value);
                if (lProduct != null)
                {
                    bool IsOverride = true;
                    bool IsBatch = (bool)GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ISBAT].Value;
                    long BatchId = GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null ? (long)GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATCHID].Value : 0L;
                    string Uom = GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                    Oprice = GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].Value != null ? double.Parse(GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].Value.ToString()) : 0;
                    //for tax included
                    double CurPrice = 0;
                    InventoryBatch InventoryBatch = null;
                    if (IsBatch && BatchId != 0L)
                    {
                        InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatchId);
                        if (InventoryBatch != null)
                        {
                            CurPrice = Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice ? InventoryBatch.MaxRetailPrice : (lProduct.RetailUOM == Uom ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice);
                            if ((Oprice > 0 && Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice && InventoryBatch.RetailSalePrice > Oprice)
                                || (InventoryBatch.MaxRetailPrice > 0 && Global.Company.CompanySalesSetup.PriceType != PriceType.MaxRetailPrice && lProduct.RetailUOM == Uom && InventoryBatch.MaxRetailPrice < Oprice))
                            {
                                OverridePriceAlertMsg(e.RowIndex);
                                IsOverride = false;
                            }
                        }
                    }
                    else
                    {
                        CurPrice = Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice ? lProduct.Msrp : (lProduct.RetailUOM == Uom ? lProduct.RetailPrice : lProduct.WholdSalePrice);
                        if ((Oprice > 0 && Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice && lProduct.RetailPrice > Oprice)
                                    || (lProduct.Msrp > 0 && Global.Company.CompanySalesSetup.PriceType != PriceType.MaxRetailPrice && lProduct.RetailUOM == Uom && lProduct.Msrp < Oprice))
                        {
                            OverridePriceAlertMsg(e.RowIndex);
                            IsOverride = false;
                        }
                    }
                    if (IsOverride && CurPrice != Oprice && Oprice > 0)
                    {
                        if (IsBatch && BatchId == 0L)
                        {
                            ToolStripStatusLabelErrorPurchase.Text = "Please select item Batch.";
                        }
                        else 
                        { 
                            Cursor.Current = Cursors.WaitCursor;
                            if (IsOverrideTabCtr)
                            {
                                SendKeys.Send("+{tab}+{tab}");
                            }
                            PriceOverride PriceOverride = new PriceOverride(this);
                            PriceOverride.ProductId = lProduct.Id;
                            PriceOverride.dateTime = DatetimePickerSalesDate.Date;
                            PriceOverride.IsBatch = IsBatch;
                            PriceOverride.BatchId = BatchId;
                            PriceOverride.TextBoxOverridePrice.Text = Oprice.ToString();
                            PriceOverride.TextBoxPrice.Text = CurPrice.ToString();
                            PriceOverride.PriceType = Global.Company.CompanySalesSetup.PriceType == PriceType.MaxRetailPrice ? PriceType.MaxRetailPrice : (lProduct.RetailUOM == Uom ? PriceType.Retail : PriceType.Wholesale);
                            PriceOverride.ShowDialog();

                            double Tax = lProduct.UseHsnTax ? ProductTaxPercentage(lProduct) : ProductTaxPercentage(lProduct.SalesTax.ToList());
                            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRICE].Value = Global.Company.CompanySalesSetup.IncludingTax ? Price - ((Price * (Price * (Tax / 100))) / (Price + (Price * (Tax / 100)))) : Price;
                            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].Value = Oprice;
                            if (IsOverrideTabCtr)
                            {
                                SendKeys.Send("{tab}{tab}");
                            }
                            Cursor.Current = Cursors.Default;
                        }
                        
                    }
                }
                Row_Added();
            }
            bool isCombineProduct = Global.Company.CompanySalesSetup.CombineItem;
            long? LocalProductId = null;
            try
            {
                if (GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.ID].Value != null)
                {
                    LocalProductId = (long)GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.ID].Value;
                }
            }
            catch
            {
                LocalProductId = null;
            }
            if (e.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                //Combine product
                if (isCombineProduct && LocalProductId != null)
                {
                    //Combine product
                    foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                    {
                        if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null && row.Cells[(int)SaleEntryTableColumn.ID].Value.ToString() == LocalProductId.ToString())
                        {
                            // row exists
                            string prevQty = row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString();
                            double finalQty = double.Parse(prevQty) + 1;
                            row.Cells[(int)SaleEntryTableColumn.QTY].Value = finalQty.ToString();
                            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(long.Parse(LocalProductId.ToString()));
                            if (Product != null)
                            {
                                row.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.Name;
                            }
                            if (GridViewSalesItem.CurrentRow != null && row.Index != GridViewSalesItem.CurrentRow.Index)
                            {
                                try
                                {
                                    GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[1].Value = "";
                                    GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    GridViewSalesItem.Rows.Remove(GridViewSalesItem.CurrentRow);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex);
                                }
                            }
                            ComputeFormTotal();
                            return;
                        }
                        //Combine product End
                    }
                }
                if (LocalProductId != null)
                {
                    Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(long.Parse(LocalProductId.ToString()));
                    if (Product != null)
                    {
                        GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.Name;
                    }
                }
            }
            if (e.ColumnIndex == (int)SaleEntryTableColumn.BATNO && GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null)
            {
                string Uom = GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                SaleProductDetails.BatchId = long.Parse(GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATCHID].Value.ToString());
            }
        }
        private void BtnSalesNewCustomer_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxSalesEntryCustomer.Id == null ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id);
            CustomerId = 0L;
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
            if (CustomerId != 0)
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(CustomerId);
                if (Customer != null)
                {
                    TextBoxSalesEntryCustomer.Text = Customer.Name;
                    TextBoxSalesEntryCustomer.Id = CustomerId.ToString();
                    TextBoxSalesCustomerAddress.Text = Customer.BillingAddress.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    Supplier Supplier = SupplierManager.Instance.GetSupplierById(CustomerId);
                    if (Supplier != null)
                    {
                        TextBoxSalesEntryCustomer.Text = Supplier.Name;
                        TextBoxSalesEntryCustomer.Id = CustomerId.ToString();
                        TextBoxSalesCustomerAddress.Text = Supplier.Address.FullAddress.Replace("\n", "").Replace(",", "," + System.Environment.NewLine);
                    }
                    else
                    {
                        CustomerId = (long)TempCustomerId;
                        MessageBox.Show("Somting went wrong, please check this account is still valid.");
                        return;
                    }
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                    this.formIsDirty = false;
                }
            }
            TextBoxSalesEntryCustomer.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewSalesItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRODUCT].ReadOnly)
            {
                if (!GridViewSalesItem.CurrentCell.ReadOnly)
                {
                    bool IsDirty = this.formIsDirty;
                    DirtyFlag(IsDirty);
                }
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.SNO].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.UOM].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.QTY].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.FREE].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.EXPDATE].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRICE].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.TAXP].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.TAX].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.DISP].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.DIS].ReadOnly = true;
                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.AMOUNT].ReadOnly = true;
                if (GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ID].Value != null)
                {
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.UOM].ReadOnly = false;
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.QTY].ReadOnly = false;
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.FREE].ReadOnly = false;
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.DISP].ReadOnly = false;
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].ReadOnly = false;
                    if ((bool)GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ISBAT].Value)
                    {
                        GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly = false;
                    }
                    //if(Global.Company.BusinessType==BuisnessType.Hospital || Global.Company.BusinessType == BuisnessType.Wholesale)
                    //{
                    GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRICE].ReadOnly = false;
                    //}
                }
            }
        }
        private void GridViewSalesItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (e.RowIndex > -1)
            {
                if (!GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRODUCT].ReadOnly)
                {
                    if (e.ColumnIndex == (int)SaleEntryTableColumn.REMOVE && (GridViewSalesItem.Rows.Count - 1) != e.RowIndex)
                    {
                        DialogResult Result = MessageBox.Show(string.Format(Grid_DeleteConfirmText, GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.SNO].Value.ToString()), "Delete Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewSalesItem.Rows.RemoveAt(e.RowIndex);
                            Row_Removed();
                        }
                    }
                }
            }
        }
        private void GridViewSalesItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSalesItem_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewSalesItem.EditingControl).DroppedDown = false;
        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value != null)
            {
                LoadEditableStock(Index, Uom);
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    double Tax = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());
                    SaleProductDetails.LocationId = LocationId;
                    if ((bool)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ISBAT].Value)
                    {
                        if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value, LocationId);
                            if (InventoryBatch != null)
                            {
                                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.RetailSalePrice - ((InventoryBatch.RetailSalePrice * (InventoryBatch.RetailSalePrice * (Tax / 100))) / (InventoryBatch.RetailSalePrice + (InventoryBatch.RetailSalePrice * (Tax / 100)))) : InventoryBatch.RetailSalePrice) : (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.WholeSalePrice - ((InventoryBatch.WholeSalePrice * (InventoryBatch.WholeSalePrice * (Tax / 100))) / (InventoryBatch.WholeSalePrice + (InventoryBatch.WholeSalePrice * (Tax / 100)))) : InventoryBatch.WholeSalePrice);
                                SaleProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.RetailPrice - ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) / (Product.RetailPrice + (Product.RetailPrice * (Tax / 100)))) : Product.RetailPrice) : (Global.Company.CompanySalesSetup.IncludingTax ? Product.WholdSalePrice - ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) / (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100)))) : Product.WholdSalePrice);
                            SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                            SaleProductDetails.ProductId = Product.Id;
                        }
                    }
                    else
                    {
                        GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.RetailPrice - ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) / (Product.RetailPrice + (Product.RetailPrice * (Tax / 100)))) : Product.RetailPrice) : (Global.Company.CompanySalesSetup.IncludingTax ? Product.WholdSalePrice - ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) / (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100)))) : Product.WholdSalePrice);
                        SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                        SaleProductDetails.ProductId = Product.Id;
                    }
                    ComputeFormTotal();
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
                    int Index = GridViewSalesItem.CurrentCell.RowIndex;
                    GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }

        }
        private void UomColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewSalesItem.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.UOM].Value = ((ComboBox)sender).Text;
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.UOM].Value == null)
                {
                    GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                }
            }

        }
        private void GridViewSalesItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewSalesItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                if (Global.Company.CompanySalesSetup.PriceType != PriceType.MaxRetailPrice)
                {
                    ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged!);
                    ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged!);

                    ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged!;
                    ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged!;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewSalesItem_KeyPress1!);
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewSalesItem, "TaxDetailsNumberChecking", GridViewSalesItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewSalesItem_KeyPress!);
                DataGridViewTextBoxEditingControl? tb = e.Control as DataGridViewTextBoxEditingControl;
                if (tb != null)
                {
                    tb.KeyDown += GridViewSalesItem_KeyDown!;
                }
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewSalesItem, "NameCheckingProduct", GridViewSalesItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewSalesItem_KeyPress!);
                DataGridViewTextBoxEditingControl? tb = e.Control as DataGridViewTextBoxEditingControl;
                if (tb != null)
                {
                    tb.KeyDown += GridViewSalesItem_KeyDown!;
                }
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange!;
                ((TextBox)e.Control).TextChanged += ProductTextChange!;
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= BatchTextChange!;
                ((TextBox)e.Control).TextChanged += BatchTextChange!;
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.QTY)
            {
                GridQtyFocus(((TextBox)e.Control).Text, (TextBox)e.Control);
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.FREE)
            {
                GridQtyFocus(((TextBox)e.Control).Text, (TextBox)e.Control);
            }
        }
        private bool GridQtyFocus(String input, TextBox sender)
        {
            if (input.Length < 0)
            {
                return false;
            }
            else
            {
                sender.Focus();
                sender.SelectionStart = 0;
                sender.SelectionLength = sender.Text.Length;
                return true;
            }
        }
        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewSalesItem.CurrentCell.ColumnIndex != (int)SaleEntryTableColumn.BATNO
                && GridViewSalesItem.CurrentCell.ColumnIndex != (int)SaleEntryTableColumn.UOM)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    IList<Product> Product = CatalogProductManager.Instance.GetProductByExactSearchQuery(((TextBox)sender).Text, Global.Company.CompanyId);
                    if (Product.Count > 0)
                    {
                        IsScanner = true;
                        if (Product.Count > 1)
                        {
                            SearchProduct();
                            if (ProductId != 0)
                            {
                                DirtyFlag(IsDirty);
                                return;
                            }
                        }
                        long CheckForAddRow = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                        //for tax update Sales
                        if (GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null &&
                        CheckForAddRow == Product.First().Id)
                        {
                            DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (Result == DialogResult.No)
                            {
                                return;
                            }
                        }
                        LoadUomTax(Product.First().Id);
                        LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(Product.First().Id));
                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (CheckForAddRow == 0 && GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
                        {
                            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewSalesItem.Rows.Add();
                        }
                        GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.First().Name;
                    }
                    else
                    {
                        ResetProductDetails(GridViewSalesItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewSalesItem.CurrentRow.Index);
                }
            }
        }
        private void BatchTextChange(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPurchase.Text = string.Empty;
            if (ComboBoxSaleInventoryLocation.SelectedIndex < 0)
            {
                ComboBoxSaleInventoryLocation.Focus();
                ComboBoxSaleInventoryLocation.Focus();
                ToolStripStatusLabelErrorPurchase.Text = SelectInventoryLoactionErrorMsg;
                return;
            }
            else
            {
                if (((TextBox)sender).Modified)
                {
                    long PId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                    SaleProductDetails.LocationId = LocationId;
                    if (!string.IsNullOrEmpty(((TextBox)sender).Text) && GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        bool IsDirty = this.formIsDirty;
                        GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].Value = ((TextBox)sender).Text;
                        DirtyFlag(IsDirty);
                        InventoryLocation Location = null!;
                        if (ComboBoxSaleInventoryLocation.SelectedIndex > -1)
                        {
                            Location = (InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex];
                        }
                        DataTable BatchResult = InventoryLocationManager.Instance.GetInventoryBatchCode(PId, ((TextBox)sender).Text, Location.Id);
                        List<DataRow> BatchDetails = BatchResult.AsEnumerable().ToList();
                        //IList<InventoryBatch> BatchDetails = Location != null ?
                        //    InventoryLocationManager.Instance.GetInventoryBatchDetailbyExactSearchText(PId, ((TextBox)sender).Text, Location.Id)
                        //    : InventoryLocationManager.Instance.GetInventoryBatchDetailbyExactSearchText(PId, ((TextBox)sender).Text);
                        if (BatchDetails.Count > 1)
                        {
                            SearchBatch();
                        }
                        else if (BatchDetails.Count > 0)
                        {
                            DataRow firstBatch = BatchDetails.First();
                            DateTime expDate = firstBatch.Field<DateTime>("ExpDate");

                            if (expDate >= Global.getTransactionDate().Date)
                            {
                                long batchId = firstBatch.Field<long>("Id");
                                LoadBatchDetails(batchId);
                                SaleProductDetails.BatchId = batchId;
                            }
                            else
                            {
                                string batchNo = firstBatch.Field<string>("BatchNo")!;
                                DisplaySystemError(string.Format(TextChange_ItemBatchExpire, batchNo));
                            }
                        }
                        else
                        {
                            GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                            ResetBatchDetails();
                            if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                            {
                                SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                                SaleProductDetails.ProductId = (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value;
                            }
                        }


                        //else if (BatchDetails.Count > 0)
                        //{
                        //    if (BatchDetails.First().ExpDate >= ((DateTime)Global.getTransactionDate().Date))
                        //    {
                        //        LoadBatchDetails(BatchDetails.First().Id);
                        //        SaleProductDetails.BatchId = BatchDetails.First().Id;
                        //    }
                        //    else
                        //    {
                        //        DisplaySystemError(string.Format(TextChange_ItemBatchExpire, BatchDetails.First().BatchNo));
                        //    }
                        //}
                        //else
                        //{
                        //    GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                        //    ResetBatchDetails();
                        //    if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                        //    {
                        //        SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                        //        SaleProductDetails.ProductId = (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value;
                        //    }
                        //}
                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                        ResetBatchDetails();
                        if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                        {
                            SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                            SaleProductDetails.ProductId = (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value;
                        }
                    }
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.QTY].Value = 0;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.FREE].Value = 0;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATNO].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAXP].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DISP].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
            //for tax update Sales
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
            SaleProductDetails.Clear();
            ComputeFormTotal();
        }
        

        private void GridViewSalesItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
                if (Convert.ToChar(e.KeyChar) == '#')
                {
                    GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, GridViewSalesItem.CurrentRow.Index];
                    }));
                }
            }
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
                if (Convert.ToChar(e.KeyChar) == '#')
                {
                    GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                    {
                        GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
                        {
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.BATNO, GridViewSalesItem.CurrentRow.Index];
                        }));
                    }
                }
            }
        }
        private void GridViewSalesItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
                if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                {
                    KeypressValidation.Instance.Keypress_PasteCheckingProduct(sender, e, "NameChecking");

                }
            }
        }
        private void LoadUomTax(long ProductId)
        {
            Cursor.Current = Cursors.WaitCursor;
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
            if (Product != null)
            {
                LoadProductCombo();
                int index = GridViewSalesItem.CurrentRow.Index;
                double Tax = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());
                bool UomCompare = false;
                (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                UomCompare = Product.RetailUOM.Equals(Product.WholesaleUOM, StringComparison.OrdinalIgnoreCase);
                if (!UomCompare)
                {
                    (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);
                }
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.Name;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.QTY].Value = 0;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.FREE].Value = 0;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATNO].Value = null;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = Product.Msrp;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAXP].Value = Tax;
                //for tax update Sales
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null && (bool)Product.isInventoryAtBatch)
                {
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Retail ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.RetailPrice - ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) / (Product.RetailPrice + (Product.RetailPrice * (Tax / 100)))) : Product.RetailPrice) : Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.WholdSalePrice - ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) / (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100)))) : Product.WholdSalePrice) : Product.Msrp - ((Product.Msrp * (Product.Msrp * (Tax / 100))) / (Product.Msrp + (Product.Msrp * (Tax / 100))));
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
                }
            }
        }
        protected override void AccountIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AccountIdTransport.Text))
            {
                CustomerId = long.Parse(AccountIdTransport.Text);
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
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
            ProductId = 0L;
            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchItems.SearchText = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value != null) ? GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value.ToString() : null;
            FormSearchItems.ShowDialog();
            if (ProductId != 0)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                if (Product != null)
                {
                    //for tax update Sales
                    if (GridViewSalesItem.Rows[GridViewSalesItem.CurrentRow.Index].Cells[(int)SaleEntryTableColumn.ID].Value != null && Check == ProductId && !string.IsNullOrEmpty(TextBoxSalesId.Text))
                    {
                        DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.No)
                        {
                            return;
                        }
                    }
                    bool isCombineProduct = Global.Company.CompanySalesSetup.CombineItem;
                    if (isCombineProduct)
                    {
                        //Combine product
                        foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                        {
                            if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null && row.Cells[(int)SaleEntryTableColumn.ID].Value.ToString() == ProductId.ToString())
                            {
                                // row exists

                                string prevQty = row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString();
                                int finalQty = int.Parse(prevQty) + 1;
                                row.Cells[(int)SaleEntryTableColumn.QTY].Value = finalQty.ToString();
                                ComputeFormTotal();
                                return;
                            }
                        }
                        //Combine product End
                    }

                    LoadUomTax(ProductId);
                    LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId));
                    if (Check == 0 && GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
                    {
                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSalesItem.Rows.Add();
                    }
                    GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[2, GridViewSalesItem.CurrentRow.Index];
                        GridViewSalesItem.CurrentCell.Selected = true;
                        GridViewSalesItem.BeginEdit(true);
                    }));
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
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            BatchId = 0L;
            FormSearchBatch FormSearchBatch = new FormSearchBatch(this);
            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchBatch.ProductId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
            //FormSearchBatch.SearchText = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].Value != null) ? GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].Value.ToString() : string.Empty;
            FormSearchBatch.SearchText = GridViewSalesItem.CurrentRow?.Cells[(int)SaleEntryTableColumn.BATNO]?.Value?.ToString() ?? string.Empty;
            FormSearchBatch.LocationId = LocationId;
            FormSearchBatch.ShowDialog();
            if (BatchId != 0)
            {
                LoadBatchDetails(BatchId);
                //GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.OPRICE, GridViewSalesItem.CurrentRow.Index];
                GridViewSalesItem.CurrentCell = GridViewSalesItem.CurrentRow != null ? GridViewSalesItem[(int)SaleEntryTableColumn.OPRICE, GridViewSalesItem.CurrentRow.Index] : GridViewSalesItem.CurrentCell;
                GridViewSalesItem.CurrentCell.Selected = true;
            }
            else
            {
                DirtyFlag(IsDirty);
                return;
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadBatchDetails(long BatId)
        {
            InventoryBatch InventoryBatch = LocationId == 0L ? InventoryLocationManager.Instance.GetInventoryByBatchId(BatId) : InventoryLocationManager.Instance.GetInventoryByBatchId(BatId, LocationId);
            if (InventoryBatch != null)
            {
                int index = GridViewSalesItem.CurrentRow.Index;
                long ProductId = GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value != null ? (long)GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                if (ProductId != 0L)
                {
                    Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                    if (Product != null)
                    {
                        double Tax = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());
                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = InventoryBatch.Id;
                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
                        if (Global.Company.CompanySalesSetup.PriceType != PriceType.MaxRetailPrice &&
                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value != null &&
                            !string.IsNullOrEmpty(GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString()))
                        {
                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString() == Product.RetailUOM ? (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.RetailSalePrice - ((InventoryBatch.RetailSalePrice * (InventoryBatch.RetailSalePrice * (Tax / 100))) / (InventoryBatch.RetailSalePrice + (InventoryBatch.RetailSalePrice * (Tax / 100)))) : InventoryBatch.RetailSalePrice) : GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString() == Product.WholesaleUOM ? (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.WholeSalePrice - ((InventoryBatch.WholeSalePrice * (InventoryBatch.WholeSalePrice * (Tax / 100))) / (InventoryBatch.WholeSalePrice + (InventoryBatch.WholeSalePrice * (Tax / 100)))) : InventoryBatch.WholeSalePrice) : InventoryBatch.MaxRetailPrice - ((InventoryBatch.MaxRetailPrice * (InventoryBatch.MaxRetailPrice * (Tax / 100))) / (InventoryBatch.MaxRetailPrice + (InventoryBatch.MaxRetailPrice * (Tax / 100))));
                        }
                        else
                        {
                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Retail ? (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.RetailSalePrice - ((InventoryBatch.RetailSalePrice * (InventoryBatch.RetailSalePrice * (Tax / 100))) / (InventoryBatch.RetailSalePrice + (InventoryBatch.RetailSalePrice * (Tax / 100)))) : InventoryBatch.RetailSalePrice) : Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? (Global.Company.CompanySalesSetup.IncludingTax ? InventoryBatch.WholeSalePrice - ((InventoryBatch.WholeSalePrice * (InventoryBatch.WholeSalePrice * (Tax / 100))) / (InventoryBatch.WholeSalePrice + (InventoryBatch.WholeSalePrice * (Tax / 100)))) : InventoryBatch.WholeSalePrice) : InventoryBatch.MaxRetailPrice - ((InventoryBatch.MaxRetailPrice * (InventoryBatch.MaxRetailPrice * (Tax / 100))) / (InventoryBatch.MaxRetailPrice + (InventoryBatch.MaxRetailPrice * (Tax / 100))));
                        }
                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                        SaleProductDetails.BatchId = InventoryBatch.Id;
                    }
                }
            }
            else
            {
                DisplaySystemError(SlectedBatchNotInCurrentLocationErrorMsg);
                return;
            }
        }
        private double StockByXfactorBatch(long BatchId, string Uom)
        {
            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatchId, LocationId);
            if (InventoryBatch != null)
            {
                return (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerSalesDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) * (InventoryBatch.RetailUOM == Uom ? (InventoryBatch.WholesaleXFactor * InventoryBatch.RetailXFactor) : InventoryBatch.WholesaleXFactor));
            }
            return 0;
        }
        private double StockByXfactor(long ProductId, string Uom)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoById(ProductId);
            if (Product != null)
            {
                if (Product.isInventoryAtBatch != null && (bool)Product.isInventoryAtBatch)
                {
                    IList<InventoryBatch> InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(ProductId, LocationId);
                    if (InventoryBatch.Count > 0)
                    {
                        double TotalStock = 0.00;
                        foreach (InventoryBatch Detail in InventoryBatch)
                        {
                            TotalStock += (((Detail.StockDate!=null && Detail.StockDate<=DatetimePickerSalesDate.Date?Detail.OpeningStock:0)+ Detail.QuantityOnHand) * (Product.RetailUOM == Uom ? (Detail.WholesaleXFactor * Detail.RetailXFactor) : Detail.WholesaleXFactor));
                        }
                        return TotalStock;
                    }
                }
                else
                {
                    Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(ProductId, LocationId);
                    if (Inventory != null)
                    {
                        return (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerSalesDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) * (Product.RetailUOM == Uom ? (Product.WholesaleXFactor * Product.RetailXFactor) : Product.WholesaleXFactor));
                    }
                }
            }
            return 0;
        }
        private void ResetBatchDetails()
        {
            int index = GridViewSalesItem.CurrentRow.Index;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
        }
        private void GridViewSalesItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.SNO].Value = GridViewSalesItem.Rows.Count;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.FREE].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.PRICE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.TAX].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.DIS].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.OPRICE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DirtyFlag(IsDirty);
        }
        private void FormItembasedSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                    GridViewSalesItem.CurrentCell.Selected = true;
                    GridViewSalesItem.BeginEdit(true);
                    e.Cancel = true;
                }
            }
        }

        private void BtnSalesSearchCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            long? TempCustomerId = TextBoxSalesEntryCustomer.Id == null ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id);
            CustomerId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = true;
            FormSearchAccount.IncludeEmployees = false;
            FormSearchAccount.IncludeGeneralAccounts = false;
            FormSearchAccount.FromSales = true;
            FormSearchAccount.ShowDialog();
            if (CustomerId != 0 && !checkBoxIsPatient.Checked)
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(CustomerId);
                if (customer != null)
                {
                    TextBoxSalesEntryCustomer.Text = customer.Name;
                    TextBoxSalesEntryCustomer.Id = CustomerId.ToString();
                    TextBoxSalesCustomerAddress.Text = customer.BillingAddress.FullAddress.Replace("\n", System.Environment.NewLine);
                }
                else
                {
                    Supplier supplier = SupplierManager.Instance.GetSupplierById(CustomerId);
                    if (supplier != null)
                    {
                        TextBoxSalesEntryCustomer.Text = supplier.Name;
                        TextBoxSalesEntryCustomer.Id = CustomerId.ToString();
                        TextBoxSalesCustomerAddress.Text = supplier.Address.FullAddress.Replace("\n", System.Environment.NewLine);
                    }
                }
            }
            else if (Global.Company.BusinessType == BuisnessType.Hospital && CustomerId != 0 && checkBoxIsPatient.Checked)
            {
                Patient patient = PatientManager.Instance.GetPatientById(CustomerId);
                if (patient != null)
                {
                    TextBoxSalesEntryCustomer.Text = patient.Name;
                    TextBoxSalesCustomerAddress.Text = "Patient # : " + patient.PatientNumber + System.Environment.NewLine + patient.Address.FullAddress.Replace("\n", System.Environment.NewLine);
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("The selected customer is not available anymore");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void LoadEditableStock(int Index, string Uom)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value);
            if (Product != null)
            {
                long DetailId = GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null ? long.Parse(GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value.ToString()) : 0L;
                if (DetailId != 0)
                {
                    SaleDetail Detail = SalesManager.GetSaleDetail(DetailId);
                    if (Detail != null)
                    {
                        double OldQty = (Detail.Quantity + Detail.FreeQuantity);
                        if (Detail.Uom != Uom)
                        {
                            if (Product != null)
                            {
                                if (Product.RetailUOM == Uom)
                                {
                                    OldQty = (OldQty * Product.RetailXFactor);
                                }
                                else
                                {
                                    OldQty = (OldQty / Product.RetailXFactor);
                                }
                            }
                        }
                        SaleProductDetails.EditableStock = 0.00;
                    }
                }
            }
        }
        private void GridViewSalesItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadProductDetails(e.RowIndex);
            }

        }
        private void LoadProductDetails(int Index)
        {
            Cursor.Current = Cursors.WaitCursor;
            SaleProductDetails.EditableStock = 0.00;
            var row = GridViewSalesItem.Rows[Index];
            var productIdCell = row.Cells[(int)SaleEntryTableColumn.ID].Value;
            if (productIdCell != null)
            {
                long productId = (long)productIdCell;
                Product product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(productId);
                if (product != null)
                {
                    LoadProductAdditinalDetails(product);
                    var batchIdCell = row.Cells[(int)SaleEntryTableColumn.BATCHID].Value;
                    if (product._isInventoryAtBatch == true && batchIdCell != null)
                    {
                        SaleProductDetails.BatchId = (long)batchIdCell;
                    }
                }
            }
            else
            {
                SaleProductDetails.Clear();
                EnableProductAdditinalDetails();
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            SaleProductDetails.LocationId = LocationId;
            SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
            SaleProductDetails.ProductId = Product.Id;
            EnableProductAdditinalDetails();
        }

        private void LoadProductCombo()
        {
            ComboUtils.InitializeReferedCombo(ComboBoxSoldby);
            ComboUtils.InitializeReferedCombo(ComboBoxreferedby);
        }
        private void EnableProductAdditinalDetails()
        {
            SaleProductDetails.ReadyOnly = true;
        }


        private void GridViewSalesItem_Leave(object sender, EventArgs e)
        {
            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.UOM, GridViewSalesItem.CurrentRow.Index];
        }

        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            OverallTotal();
        }
        bool IsOverrideTabCtr = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab && BtnSalesQuotesSearch.Selected == true && Global.Company.BusinessType != BuisnessType.Hospital)
            {
                YesNoRbtSalesMethod.Focus();
                return true;
            }
            if (keyData == Keys.Tab && BtnPrescriptionBySearch.Selected == true && Global.Company.BusinessType == BuisnessType.Hospital)
            {
                YesNoRbtSalesMethod.Focus();
                return true;
            }
            if (keyData == (Keys.F12))
            {
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, GridViewSalesItem.Rows.Count - 1];
                GridViewSalesItem.BeginEdit(true);
                return true;
            }
            if (keyData == (Keys.F2) && (TextBoxSalesEntryCustomer.Focused || BtnSalesSearchCustomer.Focused))
            {
                BtnSalesSearchCustomer.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnSalesNew.Enabled)
                {
                    BtnSalesNew_Click(this, null);
                }
                else
                {
                    BtnSalesNewCustomer.ShowDropDown();
                }
            }
            else if (keyData == (Keys.F4) && BtnSalesDelete.Enabled)
            {
                BtnSalesDelete_Click(this, null);
            }
            else if (keyData == (Keys.F6) && BtnReceivePayment.Enabled)
            {
                BtnReceivePayment_Click(this, null);
            }
            else if (keyData == (Keys.F9) && BtnSalesPrint.Enabled)
            {
                BtnSalesPrint_Click(this, null);
            }
            else if (keyData == (Keys.F8) && BtnSalesSave.Enabled)
            {
                BtnSalesSave_Click(this, null);
            }
            else if (keyData == (Keys.F11))
            {
                BtnAdditionalDetail.PerformClick();
            }
            else if (keyData == (Keys.Escape) && BtnSalesCancel.Enabled)
            {
                BtnSalesCancel_Click(this, null);
                return false;
            }
            else if (keyData == (Keys.F10) && BtnSalesExit.Enabled)
            {
                BtnSalesExit_Click(this, null);
                return true;
            }
            try
            {
                if ((keyData == Keys.F2) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                {
                    SearchProduct();
                    return true;
                }
                if ((keyData == Keys.F2) && !GridViewSalesItem.CurrentCell.ReadOnly && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
                {
                    if (ComboBoxSaleInventoryLocation.SelectedIndex < 0)
                    {
                        ComboBoxSaleInventoryLocation.Focus();
                        ComboBoxSaleInventoryLocation.Focus();
                        ToolStripStatusLabelErrorPurchase.Text = SelectInventoryLoactionErrorMsg;
                        return true;
                    }
                    else
                    {
                        SearchBatch();
                        return true;
                    }
                }
                if (keyData == (Keys.Tab) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.FREE)
                {
                    if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly)
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.BATNO))
                {
                    if (!GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.OPRICE))
                {
                    IsOverrideTabCtr = true;
                    SendKeys.Send("{tab}{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.OPRICE))
                {
                    IsOverrideTabCtr = false;
                }
                if (keyData == (Keys.Tab) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.DISP)
                {
                    if (GridViewSalesItem.CurrentCell.RowIndex != GridViewSalesItem.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.OPRICE)
                {
                    if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly)
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.DISP))
                {
                    SendKeys.Send("{tab}{tab}");
                }

                if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                {
                    if (GridViewSalesItem.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        TextBoxSalesMemo.Focus();
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception ex)
            {
            }
#pragma warning restore 0168
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnSalesSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (YesNoRbtSalesMethod.Enabled)
                {
                    GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.RowCount - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                    GridViewSalesItem.CurrentCell.Selected = true;
                    GridViewSalesItem.BeginEdit(true);
                }
                else
                {
                    BtnSalesPrint.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiscountAdditinalChargeGrid.Focus();
                return;
            }
        }
        private void YesNoRbtSalesMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerSalesDate.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && Global.Company.BusinessType != BuisnessType.Hospital)
            {
                e.IsInputKey = true;

                TextBoxSalesQuotesSearch.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab && Global.Company.BusinessType == BuisnessType.Hospital)
            {
                e.IsInputKey = true;

                TextBoxSearchPrescription.Focus();
            }
        }
        private void TextBoxSalesMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewSalesItem.Select();
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, 0];
                GridViewSalesItem.CurrentCell.Selected = true;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxreferedby.Select();
            }
        }

        private void TextBoxSalesSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSalesSearch_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnSalesSearch.PerformClick();
            }
        }

        private void YesNoRbtSalesMethod_Load(object sender, EventArgs e)
        {
            if (NoteId != 0L || YesNoRbtSalesMethod.Checked)
            {
                TextBoxSalesEntryCustomer.ReadOnly = true;
                TextBoxSalesEntryCustomer.TabStop = false;
                if (TextBoxSalesEntryCustomer.Id == null)
                {
                    TextBoxSalesEntryCustomer.ResetText();
                }
            }
            else
            {
                TextBoxSalesEntryCustomer.ReadOnly = false;
                TextBoxSalesEntryCustomer.TabStop = true;
            }
        }
        public void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
            DiscountAdditinalChargeGrid.IsDirty = Enable;
        }

        private void DiscountAdditinalChargeGrid_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.formIsDirty)
            {
                this.InputControls_OnChange(sender, e);
            }
        }

        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSalesSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewSalesItem.Select();
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, 0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSalesReturn SalesReturn = new FormSalesReturn();
            SalesReturn.SaleReturnOnLoad = true;
            SalesReturn.SearchSalesId = long.Parse(TextBoxSalesId.Text);
            SalesReturn.ShowDialog();
            LoadSaleEntry(long.Parse(TextBoxSalesId.Text));
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSalesQuotesSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSalesQuotesSearch_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnSalesQuotesSearch.PerformClick();
            }
        }

        private void TextBoxSalesCustomerAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }
        public override void InputControls_OnChange(object sender, EventArgs e)
        {
            this.formIsDirty = true;
        }

        private void BtnAdditionalDetail_Click(object sender, EventArgs e)
        {
            FormAdditionalDetails FormAdditionalDetails = new FormAdditionalDetails(this);
            FormAdditionalDetails.AllAdditionalDetails = this.AllAdditionalDetails;
            FormAdditionalDetails.ShowDialog();
        }
        private void TextBoxSearchPrescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Global.Company.BusinessType == BuisnessType.Hospital || Global.Company.BusinessType == BuisnessType.Retail)
                {
                    BtnPrescriptionBySearch.PerformClick();
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                if (Global.Company.BusinessType == BuisnessType.Hospital || Global.Company.BusinessType == BuisnessType.Retail)
                {
                    BtnPrescriptionBySearch.PerformClick();
                }
            }
        }
        private bool Validation()
        {
            ToolStripStatusLabelErrorPurchase.Text = string.Empty;
            if (string.IsNullOrEmpty(TextBoxSearchPrescription.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = "Please enter patient number/name/date of consult.";
                return false;
            }
            return true;
        }
        private void RecentPrescription()
        {
            string SearchText = TextBoxSearchPrescription.Text.Trim();
            IList<ConsultationNote> Note = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                Note = ConsultationNoteManager.Instance.ListNotesEntryByCompany(Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                Note = ConsultationNoteManager.Instance.ListNotesEntryByCurrentDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                Note = ConsultationNoteManager.Instance.ListNotesEntryBySearchText(SearchText, Global.Company.CompanyId);
            }
            if (Note != null && Note.Count > 0)
            {
                NoteId = 0L;
                PatientId = 0L;
                OPId = 0L;
                FormRecentPrescription FormRecentPrescription = new FormRecentPrescription(this);
                FormRecentPrescription.Notes = Note;
                FormRecentPrescription.ShowDialog();
            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = "No entry found.";
            }
        }
        private void LoadPrescriptions(long NoteId)
        {
            if (Global.Company.PatientPurchaseAccountId != null)
            {
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(NoteId);
                if (Note != null)
                {

                    if (Note.SaleEntryId != null)
                    {
                        LoadSaleEntry((long)Note.SaleEntryId);
                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[GridViewSalesItem.Rows.Count - 1].Cells[(int)SaleEntryTableColumn.PRODUCT];
                        GridViewSalesItem.CurrentCell.Selected = true;
                        GridViewSalesItem.BeginEdit(true);
                    }
                    else
                    {
                        ResetForm();
                        EnableForm(true);
                        PatientId = Note.PatientId;
                        OPId = (long)Note.OpRegistrationId;
                        IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(NoteId);
                        if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                        {
                            int index = 0;
                            foreach (var ConsPres in lConsultedPrescription)
                            {
                                Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById((long)ConsPres.PrescriptionId);
                                if (PrescriptionFromDB != null)
                                {
                                    if (PrescriptionFromDB.ProductId != null)
                                    {
                                        Product Product = CatalogProductManager.Instance.GetProductInfoById((long)PrescriptionFromDB.ProductId);
                                        if (Product != null)
                                        {
                                            double Tax = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());
                                            LoadProductCombo();
                                            GridViewSalesItem.Rows.Add();
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value = Product.Id;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.Name;
                                            (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                                            (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.QTY].Value = 0;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.FREE].Value = 0;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATNO].Value = null;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Retail ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.RetailPrice - ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) / (Product.RetailPrice + (Product.RetailPrice * (Tax / 100)))) : Product.RetailPrice) : Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? (Global.Company.CompanySalesSetup.IncludingTax ? Product.WholdSalePrice - ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) / (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100)))) : Product.WholdSalePrice) : Product.Msrp - ((Product.Msrp * (Product.Msrp * (Tax / 100))) / (Product.Msrp + (Product.Msrp * (Tax / 100))));
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0.00;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = Product.Msrp;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAXP].Value = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());
                                            //for tax update Sales
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DISP].Value = Product.DefaultDiscount;
                                            GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                                            if (Product.isInventoryAtBatch != null)
                                            {
                                                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                                            }
                                            else
                                            {
                                                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
                                            }
                                            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                            index++;
                                        }
                                    }
                                    else
                                    {
                                        GridViewSalesItem.Rows.Add();
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = PrescriptionFromDB.CustomProduct;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.QTY].Value = 0;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.FREE].Value = 0;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATNO].Value = null;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAXP].Value = 0.00;
                                        //for tax update Sales
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value = null;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DISP].Value = 0.00;
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                                        
                                        GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
                                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                        index++;
                                    }
                                }
                            }
                            Row_Added();
                            ReSequence();
                            string Memo = string.Empty;
                            Patient Patient = PatientManager.Instance.GetPatientById(Note.PatientId);
                            if (Patient != null)
                            {
                                Memo = "Patient #: " + Patient.PatientNumber + System.Environment.NewLine;
                                if (Global.Company.PatientPurchaseAccount != null)
                                {
                                    TextBoxSalesEntryCustomer.Text = Global.Company.PatientPurchaseAccount.Name;
                                    TextBoxSalesEntryCustomer.Id = Global.Company.PatientPurchaseAccount.Id.ToString();
                                }
                                if (Patient.AddressId != null)
                                {
                                    Address Address = AddressManager.Instance.GetAddressById((long)Patient.AddressId);
                                    if (Address != null)
                                    {
                                        TextBoxSalesCustomerAddress.Text = Address.FullAddress.Replace("\n", "").Replace(" ,", "," + System.Environment.NewLine).Replace(", ", "," + System.Environment.NewLine);
                                    }
                                }
                                if (Note.InPatientAdmissionId != null)
                                {
                                    InPatientAdmission InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById((long)Note.InPatientAdmissionId);
                                    if (InPatientAdmission != null)
                                    {
                                        if (InPatientAdmission.CurrentLocation != null)
                                        {
                                            Memo += "Ward #: " + InPatientAdmission.CurrentLocation.Ward.Name + System.Environment.NewLine;
                                            Memo += "Bed #: " + InPatientAdmission.CurrentLocation.Bed.Name + System.Environment.NewLine;
                                        }
                                    }
                                }
                                else if (Note.OpRegistrationId != null)
                                {
                                    Registration Registration = OpManager.Instance.GetOpRegistrationById((long)Note.OpRegistrationId);
                                    if (Registration != null)
                                    {
                                        Memo += "Tocken #: " + Registration.TockenNo + System.Environment.NewLine;
                                    }
                                }
                                if (Note.Consultant != null)
                                {
                                    Memo += "Consultant: " + Note.Consultant.Name;
                                }
                            }
                            TextBoxSalesMemo.Text = Memo;
                            TextBoxSalesEntryCustomer.Focus();
                        }

                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[0].Cells[(int)SaleEntryTableColumn.UOM];
                        GridViewSalesItem.CurrentCell.Selected = true;
                        GridViewSalesItem.BeginEdit(true);

                    }
                    this.formIsDirty = false;
                }
            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = PatientPurchaseAccountErrorMsg;
                NoteId = 0L;
            }
        }

        private void ResetForLocationChange()
        {
            SaleProductDetails.LocationId = LocationId;
            if (GridViewSalesItem.CurrentCell != null)
            {
                LoadProductDetails(GridViewSalesItem.CurrentCell.RowIndex);
            }

        }
        private void LocationIndexChange()
        {
            if (GridViewSalesItem.Rows.Count > 0)
            {
                foreach (DataGridViewRow Row in GridViewSalesItem.Rows)
                {
                    if (LocationId != (ComboBoxSaleInventoryLocation.SelectedIndex > -1 ? ((InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex]).Id : 0L))
                    {
                        Row.Cells[(int)SaleEntryTableColumn.BATNO].Value = null;
                        Row.Cells[(int)SaleEntryTableColumn.EXPDATE].Value = null;
                        Row.Cells[(int)SaleEntryTableColumn.BATCHID].Value = null;
                    }
                }
            }
            LocationId = ComboBoxSaleInventoryLocation.SelectedIndex > -1 ? ((InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex]).Id : 0L;
            ResetForLocationChange();
            if (string.IsNullOrEmpty(TextBoxSalesId.Text) && ComboBoxSaleInventoryLocation.SelectedIndex > -1)
            {
                FormLocationId = ((InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex]).Id;
            }


        }
        private void ComboBoxSaleInventoryLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationIndexChange();
            if (string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                this.formIsDirty = false;
            }
        }

        private void ComboBoxSaleInventoryLocation_TextChanged(object sender, EventArgs e)
        {
            LocationIndexChange();
            if (string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                this.formIsDirty = false;
            }
        }

        private void BtnPrescriptionBySearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Validation())
            {
                RecentPrescription();
                if (NoteId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnSalesSave_Click(sender, e);
                                LoadPrescriptions(NoteId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadPrescriptions(NoteId);
                        }
                    }
                    else
                    {
                        LoadPrescriptions(NoteId);
                    }

                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void TextBoxSalesMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSalesSearch_Leave(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
        }

        private void TextBoxSalesQuotesSearch_Leave(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
        }
        private void GridViewSalesItem_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)SaleEntryTableColumn.QTY && GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null
                && (bool)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ISBAT].Value && GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATCHID].Value == null)
            {
                long PId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                SaleProductDetails.LocationId = LocationId;
                InventoryLocation Location = null;
                if (ComboBoxSaleInventoryLocation.SelectedIndex > -1)
                {
                    Location = (InventoryLocation)ComboBoxSaleInventoryLocation.Items[ComboBoxSaleInventoryLocation.SelectedIndex];
                }
                IList<InventoryBatch> BatchDetails = Location != null ?
                    InventoryLocationManager.Instance.GetInventoryBatchbyProductId(PId, Location.Id)
                    : InventoryLocationManager.Instance.GetInventoryStockBatchbyProductId(PId);
                if (BatchDetails.Count > 1)
                {
                    SearchBatch();
                }
                else if (BatchDetails.Count > 0)
                {
                    if (BatchDetails.First().ExpDate >= ((DateTime)Global.getTransactionDate().Date))
                    {
                        LoadBatchDetails(BatchDetails.First().Id);
                        SaleProductDetails.BatchId = BatchDetails.First().Id;
                    }
                    else
                    {
                        DisplaySystemError(string.Format(TextChange_ItemBatchExpire, BatchDetails.First().BatchNo));
                    }
                }
                else
                {
                    GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                    ResetBatchDetails();
                    if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                    {
                        SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                        SaleProductDetails.ProductId = (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value;
                    }
                }
                GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void TextBoxSalesEntryCustomer_ModifiedChanged(object sender, EventArgs e)
        {
            if (CustomerId != null)
            {
                if (GridViewSalesItem.Rows.Count > 1)
                {
                    for (int i = 0; i <= GridViewSalesItem.Rows.Count - 2; i++)
                    {
                        if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value != null)
                        {
                            long ProductId = (long)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value;
                            Product lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                            if (lProduct != null)
                            {
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value = lProduct.UseHsnTax ? ProductTaxPercentage(lProduct) : ProductTaxPercentage(lProduct.SalesTax.ToList());
                            }
                        }
                    }
                    ComputeFormTotal();
                }
            }
        }

        private void DatetimePickerSalesDate_Leave(object sender, EventArgs e)
        {
            if (DatetimePickerSalesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                if (GridViewSalesItem.Rows.Count > 1)
                {
                    for (int i = 0; i <= GridViewSalesItem.Rows.Count - 2; i++)
                    {
                        if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value != null)
                        {
                            long ProductId = (long)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value;
                            Product lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                            if (lProduct != null)
                            {
                                GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value = lProduct.UseHsnTax ? ProductTaxPercentage(lProduct) : ProductTaxPercentage(lProduct.SalesTax.ToList());
                                LoadProductAdditinalDetails(lProduct);
                            }
                        }
                    }
                    ComputeFormTotal();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}