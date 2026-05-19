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
using fa.model.Accounting.Transactions;
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
using Fa.reports.catalog;
using Fa.views.sales;
using Fa.views.Systems;
using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Data;
using System.Globalization;
using System.Text;

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

    // row.Cells[(int)SaleEntryTableColumn.PRICE].Value = lProduct.RetailPrice; 
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
        public long PrevPriceProductId = 0L;
        public long BatchId;
        public long CustomerId = 0L;
        public long CustomerNameId = 0L;
        public long SearchSalesId = 0L;
        public bool IsScanner;
        public long NoteId = 0L;
        public long PatientId = 0L;
        public long OPId = 0L;
        public long LocationId = 0L;
        public long FormLocationId = 0L;
        public bool BillWithPreviousPrice { get; set; }

        private const string BarcodePrefix = "{SCAN}";
        private bool _isScannerInput = false;
        private string ScannedBarcode = string.Empty;
        private bool _isEditProcessing = false;
        private bool _isManualSearchRequested = false;
        private bool _isBarcodeProcessing = false;
        private bool _isEnterKeyInQty = false;
        private DateTime _lastBarcodeTime = DateTime.MinValue;
        private DateTime _lastScannerInput = DateTime.MinValue;
        private StringBuilder _scannerBuffer = new StringBuilder();
        private DateTime _lastKeyTime = DateTime.MinValue;
        private const int SCANNER_THRESHOLD_MS = 30;
        private DateTime _lastKeyPressTime = DateTime.MinValue;
        private StringBuilder _barcodeBuffer = new StringBuilder();
        private const int SCANNER_MAX_DELAY_MS = 50;
        private const int BarcodeThresholdMs = 100; // Max time between barcode chars
        public FormItembasedSales()
        {
            InitializeComponent();
            checkBoxGST.CheckedChanged += checkBoxGST_CheckedChanged!;
            SalesManager = SalesManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "groupBox2", "DiscountAdditinalChargeGrid", "ToolStripStatusLabelErrorPurchase", "ComboBoxSaleInventoryLocation", "SaleProductDetails" };
            GridViewSalesItem.EditingControlShowing += GridViewSalesItem_EditingControlShowing!;
            GridViewSalesItem.CellEndEdit += GridViewSalesItem_CellEndEdit!;
            GridViewSalesItem.KeyPress += GridViewSalesItem_KeyPress!;
            ComboBoxInvoicePriceBy.SelectedIndexChanged += ComboBoxInvoicePriceBy_SelectedIndexChanged!;
            InitializeScannerHandling();
        }
        private void FormItembasedSales_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            InitializePrintingComboBox();
            LoadUomTax(0L);
            this.Visible = false;
            setSize();
            this.Visible = true;
            ResetForm();
            ComboUtils.InitializeStockLocationCombo(ComboBoxSaleInventoryLocation, Global.Company.CompanyId);
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp");
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
            lSaleEntry.SaleDate = (DateTime)DatetimePickerSalesDate.Date!;
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
            lSaleEntry.PaymentType = ComboBoxPaymentType.SelectedIndex > -1 ? (long?)(PaymentTransactioType)ComboBoxPaymentType.SelectedIndex : null;
            float TotalAmount = float.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value.ToString()!);
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
                    SaleDetail.Id = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value == null) ? 0L : long.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value.ToString()!);
                    SaleDetail.ProductId = Product.Id;
                    SaleDetail.MaterialId = Product.MaterialId;
                    SaleDetail.Msrp = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.MSRP].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.MSRP].Value.ToString()!) : 0;
                    SaleDetail.isBatch = (bool)GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ISBAT].Value;
                    SaleDetail.Uom = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString();
                    SaleDetail.OrderNo = i + 1;
                    SaleDetail.PrintOrderNo = i + 1;
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
                        Quantity = double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!);
                    }
                    double FreeQuantity = 0;
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE] != null)
                    {
                        FreeQuantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value == null) ? 0 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString());
                    }
                    float Pprice = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value.ToString()!);
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
                    float TaxPercentage = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value.ToString()!);
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
                double mod = TotalAmount % (Round * 2);
                if (mod >= Round)
                {
                    _roundoff = (Round * 2) - mod;
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
            POSPayment.AccountID = TextBoxSalesEntryCustomer.Id != null ? long.Parse(TextBoxSalesEntryCustomer.Id) : 0L;

            POSPayment.ShowDialog();
            LoadSaleEntry(long.Parse(TextBoxSalesId.Text));
            Cursor.Current = Cursors.Default;
        }
        private void TextBoxSalesEntryCustomer_TextChanged(object sender, EventArgs e)
        {
            TextBoxSalesEntryCustomer.Id = null!;
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
        private void DisplaySystemSavePerform(string Message)
        {
            MessageBox.Show(Message);
            return;
        }
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            //if (SalesManager.GetSaleEntry(long.Parse(TextBoxSalesId.Text)) != null)
            //{
            //    Cursor.Current = Cursors.WaitCursor;
            //    string paperSelection = ComboBoxPrintingPaper.Text.Trim();
            //    PrinterSetup.SalePrintSetup(long.Parse(TextBoxSalesId.Text), true, Entrytype.SALE, "Sales", paperSelection);
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            //    DisplaySystemError("Something went wrong, please check this sale is still valid.");
            //    return;
            //}

        }
        private void BtnSalesPrint_Click(object sender, EventArgs e)
        {
            if (SalesManager.GetSaleEntry(long.Parse(TextBoxSalesId.Text)) != null)
            {
                Cursor.Current = Cursors.WaitCursor;

                // Get the selected paper format from the combo box
                string paperFormatName;
                //var selectedFormat = ComboBoxPrintingPaper.SelectedItem as PrintPaperFormat;
                if (ComboBoxPrintingPaper.SelectedItem is PrintPaperFormat selectedFormat)
                {
                    // Use selected format's name
                    paperFormatName = selectedFormat.DisplayName!;
                }
                else if (ComboBoxPrintingPaper.Items.Count > 0)
                {
                    // Fallback to first item's name if nothing selected
                    if (ComboBoxPrintingPaper.Items[0] is PrintPaperFormat firstFormat)
                    {
                        paperFormatName = firstFormat.DisplayName!;
                    }
                    else
                    {
                        // If casting fails, use the displayed text
                        paperFormatName = ComboBoxPrintingPaper.Text;
                    }
                }
                else
                {
                    // If combo box is empty, use the displayed text
                    paperFormatName = ComboBoxPrintingPaper.Text;
                }

                // Determine print type based on checkbox
                bool isGSTPrint = checkBoxGST.Checked;

                // Call print function with GST status and selected paper format
                // PrinterSetup.SalePrintSetup(long.Parse(TextBoxSalesId.Text), false, Entrytype.SALE, "Sales", paperFormatName);

                PrinterSetup.SalePrintAndWhatsUpSetup(
                    long.Parse(TextBoxSalesId.Text),
                    false,
                    Entrytype.SALE,
                    isGSTPrint, true,
                    paperFormatName!); // Pass the selected paper format name

                Cursor.Current = Cursors.Default;
            }
            else
            {
                DisplaySystemError("Something went wrong, please check this sale is still valid.");
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
            CustomerId = 0L;
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
                if (!PriceValidation())
                {
                    return;
                }
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

                    DisplaySystemSavePerform("Sales Bill No : " + SalesReferenceNumber.Text + " = Saved!");
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
                                    if (i != k && GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.ID].Value != null && PId == (long)GridViewSalesItem.Rows[k].Cells[(int)SaleEntryTableColumn.ID].Value)
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
            _isEditProcessing = false;
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
            PurchaseRate.ResetText();
            SellingRate.ResetText();
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
            ComboBoxPaymentType.SelectedIndex = 0;
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
                BtnWhatsUpEmail.Enabled = !enable;
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
                BtnWhatsUpEmail.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? enable : !enable;
                BtnWhatsUpEmail.Enabled = !enable;
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
                BtnWhatsUpEmail.Enabled = enable;
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
                ComboBoxInvoicePriceBy.SelectedIndex = (int)Global.Company.CompanySalesSetup.PriceType;
                if (SaleEntry.EntryType == Entrytype.SALE)
                {
                    SalesReferenceNumber.Text = SaleEntry.RefNumber;
                    TextBoxSalesId.Text = SaleEntry.Id.ToString();
                }
                TextBoxSalesType.Text = SaleEntry.EntryType.ToString();
                if (SaleEntry.AccountsId == null)
                {
                    //CustomerNameId = (long)SaleEntry.AccountsId!;
                    TextBoxSalesEntryCustomer.Text = SaleEntry.CustomerName;
                }
                else
                {
                    TextBoxSalesEntryCustomer.Text = SaleEntry.Account.Name;
                    TextBoxSalesEntryCustomer.Id = SaleEntry.Account.Id.ToString();
                    //CustomerNameId = SaleEntry.Account.Id;
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
                DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtSalesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                // ✅ Restore payment type
                if (SaleEntry.PaymentType != null)
                {
                    ComboBoxPaymentType.SelectedIndex = (int)SaleEntry.PaymentType.Value;
                }
                else
                {
                    ComboBoxPaymentType.SelectedIndex = -1;
                }
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    GridViewSalesItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    foreach (var SaleDetail in SaleEntry.SaleDetails.OrderBy(x => x.OrderNo))
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.SNO].Value = i + 1;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                        (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(lSaleDetail.Product.RetailUOM);
                        (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(lSaleDetail.Product.WholesaleUOM);
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
                double Quantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!));
                double FreeQuantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value.ToString()!));
                double Pprice = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value.ToString()!));
                double Oprice = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value.ToString()!));
                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;
                Amount = (Pprice * Quantity);
                if (Amount > 0)
                {
                    double DiscountPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value.ToString()!));
                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;
                    double TaxPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value.ToString()!));
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
        private void ComputeFormTotalNew()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            int roundingPrecision = Global.Company.PrimaryCurrency.RoundingPrecision;

            for (int i = 0; i < GridViewSalesItem.Rows.Count - 1; i++)
            {
                // Safe parsing helpers
                double Quantity = 0.0;
                double FreeQuantity = 0.0;
                double Pprice = 0.0;
                double Oprice = 0.0;

                double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.QTY].Value?.ToString(), out Quantity);
                double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.FREE].Value?.ToString(), out FreeQuantity);
                double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.PRICE].Value?.ToString(), out Pprice);
                double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.OPRICE].Value?.ToString(), out Oprice);

                // UOM and product id
                string selectedUom = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.UOM].Value?.ToString() ?? string.Empty;
                object idCell = GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.ID].Value;
                long productId = idCell == null ? 0L : Convert.ToInt64(idCell);

                double Amount = 0.0;

                if (productId > 0 && Quantity > 0)
                {
                    Product product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(productId);
                    if (product != null)
                    {
                        // Determine factor for selected UOM (defensive: fallback to 1)
                        double selectedFactor = 1.0;
                        if (!string.IsNullOrEmpty(selectedUom))
                        {
                            if (selectedUom.Equals(product.WholesaleUOM, StringComparison.OrdinalIgnoreCase))
                                selectedFactor = product.WholesaleXFactor > 0 ? product.WholesaleXFactor : 1.0;
                            else
                                selectedFactor = product.RetailXFactor > 0 ? product.RetailXFactor : 1.0;
                        }

                        // Determine unit price to use:
                        // - If Oprice (override) present (>0) -> treat it as unit price already for selected UOM (do NOT convert)
                        // - Else use Pprice and convert Pprice (which is stored as price for product's base UOM) to selected UOM
                        double unitPrice = 0.0;
                        if (Oprice > 0.0)
                        {
                            unitPrice = Oprice; // assume override already matches selected UOM
                        }
                        else
                        {
                            // Pprice is assumed to be price for the product's base UOM (wholesale or retail UOM).
                            // We divide by selectedFactor if Pprice is per larger UOM. This follows your approach:
                            // Pprice = Pprice / factor
                            // unitPrice = Pprice / factor  (same as Pprice = Pprice / factor; then multiply qty)
                            // Defensive: if factor is 0, use 1
                            if (selectedFactor <= 0) selectedFactor = 1.0;
                            unitPrice = Pprice / selectedFactor;
                        }

                        // Amount is unit price * quantity (free quantity not charged)
                        Amount = unitPrice * Quantity;
                    }
                }

                TotalQuantity += Quantity;

                if (Amount > 0.0)
                {
                    // Discount %
                    double DiscountPercentage = 0.0;
                    double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DISP].Value?.ToString(), out DiscountPercentage);

                    double DiscountAmount = Math.Round(Amount * (DiscountPercentage / 100.0), roundingPrecision);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;

                    // Tax %
                    double TaxPercentage = 0.0;
                    double.TryParse(GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAXP].Value?.ToString(), out TaxPercentage);

                    double TaxAmount = Math.Round(Amount * (TaxPercentage / 100.0), roundingPrecision);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAX].Value = TaxAmount;
                    Amount = Amount + TaxAmount;

                    // Final amount per row (rounded)
                    Amount = Math.Round(Amount, roundingPrecision);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = Amount;
                    TotalAmount += Amount;
                }
                else
                {
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                }
            }

            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value =
                TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

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
                    _isEditProcessing = true;
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
        private bool dont_jump;
        private int col_index;
        private int row_index;
        private void GridViewSalesItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT && !_isBarcodeProcessing)
            {
                var barcode = GridViewSalesItem.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (!string.IsNullOrEmpty(barcode))
                {
                    _isBarcodeProcessing = true;
                    //SearchProductByBarCode(barcode);
                    _isBarcodeProcessing = false;
                }
                dont_jump = true;
                col_index = e.ColumnIndex + 2;
                row_index = e.RowIndex;
            }

            if (e.ColumnIndex == (int)SaleEntryTableColumn.QTY ||
                e.ColumnIndex == (int)SaleEntryTableColumn.FREE ||
                e.ColumnIndex == (int)SaleEntryTableColumn.PRICE ||
                e.ColumnIndex == (int)SaleEntryTableColumn.DISP ||
                e.ColumnIndex == (int)SaleEntryTableColumn.BATNO)
            {
                Row_Added();
            }
            if (e.ColumnIndex == (int)SaleEntryTableColumn.QTY && _isEnterKeyInQty)
            {
                _isEnterKeyInQty = false;

                int nextRow = e.RowIndex + 1;
                if (nextRow < GridViewSalesItem.Rows.Count)
                {
                    // Schedule navigation after current events
                    BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            GridViewSalesItem.CurrentCell = GridViewSalesItem.Rows[nextRow].Cells[(int)SaleEntryTableColumn.PRODUCT];
                            GridViewSalesItem.BeginEdit(true);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                        }
                    }));
                }
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
                    try
                    {
                        var idCell = GridViewSalesItem.Rows[e.RowIndex]
                            .Cells[(int)SaleEntryTableColumn.ID].Value;

                        if (idCell != null)
                            LocalProductId = (long)idCell;
                    }
                    catch
                    {
                        LocalProductId = null;
                    }
                }
            }
            catch
            {
                LocalProductId = null;
            }
            if (e.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                if (LocalProductId != null && CustomerId > 0)
                {
                    _ = ShowPreviousPricesAsync(CustomerId, (long)LocalProductId);
                }
                //Combine product
                if (isCombineProduct && LocalProductId != null)
                {
                    //Combine product
                    foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                    {
                        if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null && row.Cells[(int)SaleEntryTableColumn.ID].Value.ToString() == LocalProductId.ToString())
                        {
                            // row exists
                            string prevQty = row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!;
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
                string Uom = GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.UOM].Value.ToString()!;
                SaleProductDetails.BatchId = long.Parse(GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleEntryTableColumn.BATCHID].Value.ToString()!);
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
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Reset the Enter key flag on any cell click
                    _isEnterKeyInQty = false;

                    // Ensure proper focus for clicked cell
                    GridViewSalesItem.CurrentCell = GridViewSalesItem[e.ColumnIndex, e.RowIndex];
                    GridViewSalesItem.BeginEdit(true);
                }
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

        private void HandleCellValue(string input)
        {
            if (input.StartsWith("[SCAN]") && input.EndsWith("[END]"))
            {
                string barcode = input.Replace("[SCAN]", "").Replace("[END]", "");
                SearchProductByBarCode(barcode);
            }
        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value != null)
            {
                LoadEditableStock(Index, Uom);
                Product product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value);
                if (product != null)
                {
                    double Tax = product.UseHsnTax ? ProductTaxPercentage(product) : ProductTaxPercentage(product.SalesTax.ToList());

                    // Get selected price type from combobox
                    PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

                    // Determine factor for selected UOM (defensive: fallback to 1)
                    double selectedFactor = 1.0;
                    if (!string.IsNullOrEmpty(Uom))
                    {
                        if (Uom.Equals(product.WholesaleUOM, StringComparison.OrdinalIgnoreCase))
                            selectedFactor = product.WholesaleXFactor > 0 ? product.WholesaleXFactor : 1.0;
                        else
                            selectedFactor = product.RetailXFactor > 0 ? product.RetailXFactor : 1.0;
                    }

                    double unitPrice = 0.0;

                    if ((bool)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ISBAT].Value)
                    {
                        if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(
                                (long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value,
                                LocationId);

                            if (InventoryBatch != null)
                            {
                                double price = GetPriceByType(selectedPriceType, InventoryBatch);

                                // Convert price to selected UOM
                                if (selectedFactor <= 0) selectedFactor = 1.0;
                                unitPrice = price / selectedFactor;

                                // Apply tax if needed
                                unitPrice = Global.Company.CompanySalesSetup.IncludingTax ?
                                    unitPrice - ((unitPrice * (unitPrice * (Tax / 100))) / (unitPrice + (unitPrice * (Tax / 100)))) :
                                    unitPrice;

                                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Math.Round(unitPrice, Global.Company.PrimaryCurrency.RoundingPrecision);
                                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                                SaleProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            double price = GetPriceByType(selectedPriceType, product);

                            // Convert price to selected UOM
                            if (selectedFactor <= 0) selectedFactor = 1.0;
                            unitPrice = price / selectedFactor;

                            // Apply tax if needed
                            unitPrice = Global.Company.CompanySalesSetup.IncludingTax ?
                                unitPrice - ((unitPrice * (unitPrice * (Tax / 100))) / (unitPrice + (unitPrice * (Tax / 100)))) :
                                unitPrice;

                            GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Math.Round(unitPrice, Global.Company.PrimaryCurrency.RoundingPrecision);
                            SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                            SaleProductDetails.ProductId = product.Id;
                        }
                    }
                    else
                    {
                        double price = GetPriceByType(selectedPriceType, product);

                        // Convert price to selected UOM
                        if (selectedFactor <= 0) selectedFactor = 1.0;
                        unitPrice = price / selectedFactor;

                        // Apply tax if needed
                        unitPrice = Global.Company.CompanySalesSetup.IncludingTax ?
                            unitPrice - ((unitPrice * (unitPrice * (Tax / 100))) / (unitPrice + (unitPrice * (Tax / 100)))) :
                            unitPrice;

                        GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Math.Round(unitPrice, Global.Company.PrimaryCurrency.RoundingPrecision);
                        SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                        SaleProductDetails.ProductId = product.Id;
                    }
                }
            }
        }

        private void LoadPriceOld(int Index, string Uom)
        {
            if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value != null)
            {
                LoadEditableStock(Index, Uom);
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    double Tax = Product.UseHsnTax ? ProductTaxPercentage(Product) : ProductTaxPercentage(Product.SalesTax.ToList());

                    // Get selected price type from combobox
                    PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

                    if ((bool)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.ISBAT].Value)
                    {
                        if (GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(
                                (long)GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.BATCHID].Value,
                                LocationId);

                            if (InventoryBatch != null)
                            {
                                double price = GetPriceByType(selectedPriceType, InventoryBatch);
                                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value =
                                    Global.Company.CompanySalesSetup.IncludingTax ?
                                        price - ((price * (price * (Tax / 100))) / (price + (price * (Tax / 100)))) :
                                        price;
                                GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                                SaleProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            double price = GetPriceByType(selectedPriceType, Product);
                            GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value =
                                Global.Company.CompanySalesSetup.IncludingTax ?
                                    price - ((price * (price * (Tax / 100))) / (price + (price * (Tax / 100)))) :
                                    price;
                            SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                            SaleProductDetails.ProductId = Product.Id;
                        }
                    }
                    else
                    {
                        double price = GetPriceByType(selectedPriceType, Product);
                        GridViewSalesItem.Rows[Index].Cells[(int)SaleEntryTableColumn.PRICE].Value =
                            Global.Company.CompanySalesSetup.IncludingTax ?
                                price - ((price * (price * (Tax / 100))) / (price + (price * (Tax / 100)))) :
                                price;
                        SaleProductDetails.CurrentDate = DatetimePickerSalesDate.Date;
                        SaleProductDetails.ProductId = Product.Id;
                    }
                }
            }
        }

        // Helper methods remain the same
        private double GetPriceByType(PriceType priceType, InventoryBatch batch)
        {
            return priceType switch
            {
                PriceType.Retail => batch.RetailSalePrice,
                PriceType.Wholesale => batch.WholeSalePrice,
                PriceType.MaxRetailPrice => batch.MaxRetailPrice,
                _ => batch.RetailSalePrice
            };
        }

        private double GetPriceByType(PriceType priceType, Product product)
        {
            return priceType switch
            {
                PriceType.Retail => product.RetailPrice,
                PriceType.Wholesale => product.WholdSalePrice,
                PriceType.MaxRetailPrice => product.Msrp,
                _ => product.RetailPrice
            };
        }
        private void LoadPricexx(int Index, string Uom)
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
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.KeyDown -= ProductTextBox_KeyDown!;
                    textBox.KeyDown += ProductTextBox_KeyDown!;
                }
            }
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
                //MoveFocusToQuantityColumn();
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
            if (_isBarcodeProcessing)
                return;

            var textBox = (TextBox)sender;

            // Only process if we're in the PRODUCT column and text is modified
            if (!textBox.Modified || GridViewSalesItem.CurrentCell.ColumnIndex != (int)SaleEntryTableColumn.PRODUCT)
                return;

            string inputText = textBox.Text;

            // Skip if empty
            if (string.IsNullOrEmpty(inputText))
            {
                ResetProductDetails(GridViewSalesItem.CurrentRow.Index);
                return;
            }

            // Handle barcode case separately - this will be processed by the barcode scanner logic
            if (IsLikelyBarcode(inputText))
            {
                return;
            }

            // Normal product name search flow
            if (_barcodeBuffer == null)
            {
                ProcessProductSearch(inputText);
            }
            else
            {

            }
        }

        private void ProcessProductSearch(string searchText)

        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                bool IsDirty = this.formIsDirty;
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = searchText;
                DirtyFlag(IsDirty);

                IList<Product> products = CatalogProductManager.Instance.GetProductByExactSearchQuery(searchText, Global.Company.CompanyId);

                if (products.Count == 0)
                {
                    ResetProductDetails(GridViewSalesItem.CurrentRow.Index);
                    DirtyFlag(IsDirty);
                    return;
                }

                // Handle single product found
                if (products.Count == 1)
                {
                    ProcessSingleProduct(products.First());
                    return;
                }

                // Multiple products found - show search dialog
                IsScanner = true;
                SearchProduct();
                if (ProductId != 0)
                {
                    DirtyFlag(IsDirty);
                    // After search dialog, focus on quantity
                    MoveFocusToQuantityColumn();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ProcessSingleProduct(Product product)
        {
            long currentProductId = product.Id;
            long existingRowProductId = GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null ?
                (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;

            // Check if we need to reset tax for existing product
            if (existingRowProductId == currentProductId &&
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null &&
                !string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                DialogResult result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            LoadUomTax(currentProductId);
            LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(currentProductId));

            // Add new row if needed
            if (existingRowProductId == 0 && GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
            {
                GridViewSalesItem.Rows.Add();
            }

            GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = product.Name;

            // Focus on quantity column
            MoveFocusToQuantityColumn();
        }

        private void MoveFocusToQuantityColumn(string productId)
        {
            // Step 1: Find and update the product if it exists
            foreach (DataGridViewRow row in GridViewSalesItem.Rows)
            {
                var idCell = row.Cells[(int)SaleEntryTableColumn.ID];
                if (idCell.Value != null && idCell.Value.ToString() == productId)
                {
                    // Update QTY
                    var qtyCell = row.Cells[(int)SaleEntryTableColumn.QTY];
                    int currentQty = int.Parse(qtyCell.Value?.ToString() ?? "0");
                    qtyCell.Value = (currentQty + 1).ToString();

                    ComputeFormTotal(); // Update totals

                    // Step 2: FORCE FOCUS to the QTY cell (100% working method)
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        // Ensure the row is selected (sometimes needed for focus)
                        row.Selected = true;
                        GridViewSalesItem.CurrentCell = qtyCell;

                        // REQUIRED: Call BeginEdit TWICE with DoEvents (ensures editing control loads)
                        GridViewSalesItem.BeginEdit(true);
                        Application.DoEvents(); // Forces UI to process pending events
                        GridViewSalesItem.BeginEdit(true); // Ensures the textbox is ready

                        // Select all text in the QTY cell
                        if (GridViewSalesItem.EditingControl is TextBox qtyTextBox)
                        {
                            qtyTextBox.Focus();
                            qtyTextBox.SelectAll();
                        }
                    });
                    return;
                }
            }

            // If product not found, add it here (your logic)
        }
        private void MoveFocusToQuantityColumn()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(MoveFocusToQuantityColumn));
                return;
            }

            if (GridViewSalesItem.CurrentRow == null) return;

            // Get QTY cell
            var qtyCell = GridViewSalesItem[(int)SaleEntryTableColumn.QTY, GridViewSalesItem.CurrentRow.Index];

            // Set current cell
            GridViewSalesItem.CurrentCell = qtyCell;

            // Start editing
            GridViewSalesItem.BeginEdit(true);

            // This replaces the MessageBox delay
            var focusTimer = new System.Windows.Forms.Timer();
            focusTimer.Interval = 50; // Same delay as MessageBox would create
            focusTimer.Tick += (s, e) =>
            {
                focusTimer.Stop();

                // Force focus to editing control
                if (GridViewSalesItem.EditingControl != null)
                {
                    GridViewSalesItem.EditingControl.Focus();

                    // If it's a TextBox, select all text
                    if (GridViewSalesItem.EditingControl is TextBox tb)
                    {
                        tb.SelectAll();
                    }
                }
            };
            focusTimer.Start();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void SetFocus(IntPtr hWnd);
        private void MoveFocusToQuantityColumnzzz()
        {
            // Use BeginInvoke to ensure this happens after the current operation completes
            this.BeginInvoke((MethodInvoker)delegate
            {
                GridViewSalesItem.CurrentCell = GridViewSalesItem[
                    (int)SaleEntryTableColumn.QTY,
                    GridViewSalesItem.CurrentRow.Index];
                GridViewSalesItem.BeginEdit(true);

                // Select all text in quantity field for easy editing
                var qtyTextBox = GridViewSalesItem.EditingControl as TextBox;
                if (qtyTextBox != null)
                {
                    qtyTextBox.SelectAll();
                }
            });
        }

        private void ProcessSingleProductxx(Product product)
        {
            long currentProductId = product.Id;
            long existingRowProductId = GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null ?
                (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;

            // Check if we need to reset tax for existing product
            if (existingRowProductId == currentProductId &&
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.SALESDETAILID].Value != null &&
                !string.IsNullOrEmpty(TextBoxSalesId.Text))
            {
                DialogResult result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            LoadUomTax(currentProductId);
            LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(currentProductId));

            // Add new row if needed
            if (existingRowProductId == 0 && GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
            {
                GridViewSalesItem.Rows.Add();
            }

            GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = product.Name;

            // Focus on quantity
            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.QTY, GridViewSalesItem.CurrentRow.Index];
            GridViewSalesItem.BeginEdit(true);
        }

        private bool IsLikelyBarcode(string input)
        {
            // Barcode detection logic
            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Typical barcode characteristics:
            // - Minimum length (e.g., 6 characters)
            // - No whitespace
            // - Alphanumeric but not matching product name patterns
            return input.Length >= 6 &&
                   !input.Contains(" ") &&
                   (input.All(char.IsDigit) || (input.Any(char.IsLetter) && input.Any(char.IsDigit)));
        }
        private void ProductTextChangezzz(object sender, EventArgs e)
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

                    // Check if this looks like a barcode scan (typically rapid input)
                    // You might want to add additional barcode validation logic here if needed
                    if (IsLikelyBarcode(((TextBox)sender).Text))
                    {
                        // Delay slightly to allow the text to be fully entered
                        Task.Delay(100).ContinueWith(_ =>
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                SearchProduct();
                                // After search, move focus to quantity column if product was found
                                if (ProductId != 0)
                                {
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.QTY, GridViewSalesItem.CurrentRow.Index];
                                    GridViewSalesItem.BeginEdit(true);
                                }
                            });
                        });
                        return;
                    }

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

                        // Move focus to quantity column after product is found
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.QTY, GridViewSalesItem.CurrentRow.Index];
                        GridViewSalesItem.BeginEdit(true);
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

        // Helper method to detect barcode input
        private bool IsLikelyBarcodezzz(string input)
        {
            // Add your barcode detection logic here
            // This could be based on length, pattern, or other characteristics
            // For now, we'll assume any input longer than 6 chars is a barcode
            return input.Length >= 6 && !input.Contains(" ") && (input.All(char.IsDigit) ||
            (input.Any(char.IsLetter) && input.Any(char.IsDigit)));
        }
        private void ProductTextChangeOld(object sender, EventArgs e)
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
            if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
            {
                // Record key press time
                var currentTime = DateTime.Now;
                var timeSinceLastKey = (currentTime - _lastKeyPressTime).TotalMilliseconds;
                _lastKeyPressTime = currentTime;

                // If keys are coming fast, assume scanner
                if (timeSinceLastKey < SCANNER_MAX_DELAY_MS)
                {
                    _barcodeBuffer.Append(e.KeyChar);
                }
                else
                {
                    _barcodeBuffer.Clear();
                    _barcodeBuffer.Append(e.KeyChar);
                }
            }
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
            if (e.KeyCode == Keys.Enter)
            {
                // Only handle for QTY column
                if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.QTY)
                {
                    // Set flag to indicate Enter key was pressed
                    _isEnterKeyInQty = true;

                    // Commit edit immediately
                    GridViewSalesItem.EndEdit();

                    // Move to next row/product column
                    int nextRow = GridViewSalesItem.CurrentCell.RowIndex + 1;
                    if (nextRow < GridViewSalesItem.Rows.Count)
                    {
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[
                            (int)SaleEntryTableColumn.PRODUCT, nextRow];
                        GridViewSalesItem.BeginEdit(true);
                    }

                    // Suppress default behavior
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    // Move focus two columns back
                    SendKeys.Send("+{TAB}+{TAB}");
                }
            }

            // Existing paste handling remains unchanged
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
        private void LoadUomTaxChangesForUomCqalc(long ProductId)
        {
            Cursor.Current = Cursors.WaitCursor;
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);

            if (Product != null)
            {
                LoadProductCombo();
                int index = GridViewSalesItem.CurrentRow.Index;

                double Tax = Product.UseHsnTax
                    ? ProductTaxPercentage(Product)
                    : ProductTaxPercentage(Product.SalesTax.ToList());

                // Clear and reload UOMs
                var uomCell = (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!;
                uomCell.Items.Clear();
                uomCell.Items.Add(Product.RetailUOM);
                if (!Product.RetailUOM.Equals(Product.WholesaleUOM, StringComparison.OrdinalIgnoreCase))
                    uomCell.Items.Add(Product.WholesaleUOM);

                // Get selected price type (retail or wholesale)
                PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

                // Default UOM
                string selectedUOM = selectedPriceType == PriceType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
                uomCell.Value = selectedUOM;

                // Fill product row defaults
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRODUCT].Value = Product.Name;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.QTY].Value = 0;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.FREE].Value = 0;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.OPRICE].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.MSRP].Value = Product.Msrp;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAX].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.TAXP].Value = Tax;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DIS].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.AMOUNT].Value = 0.00;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ID].Value = Product.Id;

                // --- Price Calculation with UOM & XFactor ---
                double basePrice = 0.0;
                double divisor = 1.0;

                if (selectedUOM == Product.WholesaleUOM)
                {
                    divisor = Product.WholesaleXFactor; // usually 12 for dozen
                    basePrice = (selectedPriceType == PriceType.Wholesale ? Product.WholdSalePrice : Product.RetailPrice);
                }
                else if (selectedUOM == Product.RetailUOM)
                {
                    divisor = Product.RetailXFactor; // usually 1 for piece
                    basePrice = (selectedPriceType == PriceType.Wholesale ? Product.WholdSalePrice : Product.RetailPrice);
                }

                // Adjust for tax (if price includes tax)
                double finalPrice = Global.Company.CompanySalesSetup.IncludingTax
                    ? basePrice / (1 + (Tax / 100))
                    : basePrice;

                // Price per selected UOM = basePrice ÷ XFactor
                double unitPrice = finalPrice / divisor;

                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = Math.Round(unitPrice, 2);
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch ?? false;
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
                (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Clear();
                (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(Product.RetailUOM);
                UomCompare = Product.RetailUOM.Equals(Product.WholesaleUOM, StringComparison.OrdinalIgnoreCase);
                if (!UomCompare)
                {
                    (GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(Product.WholesaleUOM);
                }

                // Get price type from combobox instead of global setting
                PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;
                GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value = selectedPriceType == PriceType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
                //GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.UOM].Value = Global.Company.CompanySalesSetup.PriceType == PriceType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
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

                // Update price based on selected price type
                if (Product.isInventoryAtBatch != null && (bool)Product.isInventoryAtBatch)
                {
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = 0.00;
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    long customerId = TextBoxSalesEntryCustomer.Id == null ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id);

                    // Calculate normal price once
                    decimal normalPrice =
                        (decimal)(selectedPriceType == PriceType.Retail ?
                            (Global.Company.CompanySalesSetup.IncludingTax ?
                                Product.RetailPrice -
                                ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) /
                                (Product.RetailPrice + (Product.RetailPrice * (Tax / 100))))
                                : Product.RetailPrice)
                        : selectedPriceType == PriceType.Wholesale ?
                            (Global.Company.CompanySalesSetup.IncludingTax ?
                                Product.WholdSalePrice -
                                ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) /
                                (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100))))
                                : Product.WholdSalePrice)
                        :
                            Product.Msrp -
                            ((Product.Msrp * (Product.Msrp * (Tax / 100))) /
                            (Product.Msrp + (Product.Msrp * (Tax / 100)))));

                    // Only check previous price if needed
                    decimal? previousPrice = null;

                    if (BillWithPreviousPrice)
                    {
                        previousPrice = GetPreviousCustomerPrice(customerId, Product.Id);
                    }

                    // Decide final price
                    decimal finalPrice = normalPrice;

                    if (BillWithPreviousPrice && previousPrice.HasValue)
                    {
                        finalPrice = previousPrice.Value;
                    }

                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value = finalPrice;
                    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
                }

                //else
                //{
                //    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.PRICE].Value =
                //        selectedPriceType == PriceType.Retail ?
                //            (Global.Company.CompanySalesSetup.IncludingTax ?
                //                Product.RetailPrice - ((Product.RetailPrice * (Product.RetailPrice * (Tax / 100))) /
                //                (Product.RetailPrice + (Product.RetailPrice * (Tax / 100)))) :
                //                Product.RetailPrice) :
                //        selectedPriceType == PriceType.Wholesale ?
                //            (Global.Company.CompanySalesSetup.IncludingTax ?
                //                Product.WholdSalePrice - ((Product.WholdSalePrice * (Product.WholdSalePrice * (Tax / 100))) /
                //                (Product.WholdSalePrice + (Product.WholdSalePrice * (Tax / 100)))) :
                //                Product.WholdSalePrice) :
                //            Product.Msrp - ((Product.Msrp * (Product.Msrp * (Tax / 100))) / (Product.Msrp + (Product.Msrp * (Tax / 100))));
                //    GridViewSalesItem.Rows[index].Cells[(int)SaleEntryTableColumn.ISBAT].Value = false;
                //}
            }
        }

        private void LoadUomTaxxx(long ProductId)
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
        private void SearchPreviousPrice()
        {
            long? TempCustomerId = TextBoxSalesEntryCustomer.Id == null ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id);
            Cursor.Current = Cursors.WaitCursor;
            using (var form = new FormSalsePriceSeeking(this))
            {
                if (_isEditProcessing == true)
                {
                    PrevPriceProductId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                    form.CustomerId = (long)TempCustomerId; // Get customer ID from the current form context
                }
                else
                {
                    form.CustomerId = CustomerNameId; // Get customer ID from the current form context
                }
                form.ProductId = GridViewSalesItem.CurrentRow?.Cells[(int)SaleEntryTableColumn.ID]?.Value as long? ?? 0L;
                form.ProductName = (string)(GridViewSalesItem.CurrentRow?.Cells[(int)SaleEntryTableColumn.PRODUCT]?.Value)!;
                form.ShowDialog();
                //if (form.ShowDialog() == DialogResult.OK && form.GridViewItems.CurrentRow != null)
                //{
                //    // Apply selected price to current row
                //    decimal selectedPrice = Convert.ToDecimal(form.GridViewItems.CurrentRow.Cells["price"].Value);
                //    GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRICE].Value = selectedPrice;
                //}
            }
            Cursor.Current = Cursors.Default;
        }
        private decimal? GetPreviousCustomerPrice(long customerId, long productId)
        {
            if (customerId <= 0 || productId <= 0)
                return null;

            var lastSale = SalesManager.Instance
                .GetLastPricesByProductAndCustomer(
                    Global.Company.CompanyId,
                    productId,
                    customerId)
                ?.FirstOrDefault();

            return lastSale?.Price != null ? (decimal?)lastSale.Price : null;

        }

        private void ShowPreviousPrices(long customerId, long productId)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Reset labels
                PurchaseRate.Text = "0.00";
                SellingRate.Text = "0.00";

                // 🔹 1. Get last selling price for this customer
                var sales = SalesManager.Instance
                    .GetLastPricesByProductAndCustomer(
                        Global.Company.CompanyId,
                        productId,
                        customerId
                    );

                var lastSale = sales?.FirstOrDefault();
                if (lastSale != null)
                {
                    SellingRate.Text =
                        lastSale.Price.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }

                // 🔹 2. Get last purchase rate

                var productFamily = CatalogProductManager.Instance.GetProductInfoByProductId(Global.Company.CompanyId, productId);
                if (productFamily != null && productFamily.PurchasePrice > 0)
                {
                    PurchaseRate.Text =
                        productFamily.PurchasePrice.ToString(
                            Global.Company.PrimaryCurrency.CurrencyFormat
                        );
                }
                else
                {
                    PurchaseRate.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ShowPrice()
        {
            if (GridViewSalesItem.CurrentRow != null)
            {
                long productId = GridViewSalesItem.CurrentRow?.Cells[(int)SaleEntryTableColumn.ID]?.Value as long? ?? 0L;

                FormProductPriceSeeker formProductPriceSeeker = new FormProductPriceSeeker(productId);
                formProductPriceSeeker.ShowDialog();
            }
        }

        private void SearchProduct()
        {
            if (_isBarcodeProcessing)
                return;
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
            ProductId = 0L;
            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchItems.SearchText = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value != null) ? GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value.ToString()! : null!;
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

                                string prevQty = row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!;
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
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[3, GridViewSalesItem.CurrentRow.Index];
                        GridViewSalesItem.CurrentCell.Selected = true;
                        GridViewSalesItem.BeginEdit(true);
                    }));
                    //ShowPreviousPrices(CustomerId, ProductId);
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
                            TotalStock += (((Detail.StockDate != null && Detail.StockDate <= DatetimePickerSalesDate.Date ? Detail.OpeningStock : 0) + Detail.QuantityOnHand) * (Product.RetailUOM == Uom ? (Detail.WholesaleXFactor * Detail.RetailXFactor) : Detail.WholesaleXFactor));
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
                    CustomerNameId = CustomerId;
                    TextBoxSalesCustomerAddress.Text = customer.BillingAddress.FullAddress.Replace("\n", System.Environment.NewLine);
                    BillWithPreviousPrice = customer.BillWithPreviousPrice;
                }
                else
                {
                    Supplier supplier = SupplierManager.Instance.GetSupplierById(CustomerId);
                    if (supplier != null)
                    {
                        TextBoxSalesEntryCustomer.Text = supplier.Name;
                        TextBoxSalesEntryCustomer.Id = CustomerId.ToString();
                        CustomerNameId = CustomerId;
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
            ComboBoxInvoicePriceBy.SelectedIndex = (int)Global.Company.CompanySalesSetup.PriceType;
            YesNoRadioPriceTo.Checked = true;
        }
        private void EnableProductAdditinalDetails()
        {
            SaleProductDetails.ReadyOnly = true;
        }


        private void GridViewSalesItem_Leave(object sender, EventArgs e)
        {
            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleEntryTableColumn.UOM, GridViewSalesItem.CurrentRow.Index];
            if (GridViewSalesItem.CurrentCell != null)
            {
                GridViewSalesItem.CurrentCell.Selected = true;
                GridViewSalesItem.BeginEdit(true);
            }
        }

        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            OverallTotal();
        }
        bool IsOverrideTabCtr = true;


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                bool inGrid = GridViewSalesItem.Focused || GridViewSalesItem.IsCurrentCellInEditMode;

                // 🔹 GLOBAL SHORTCUTS (only work when NOT inside grid)
                if (!inGrid)
                {
                    if (keyData == Keys.F2 && (TextBoxSalesEntryCustomer.Focused || BtnSalesSearchCustomer.Focused))
                    {
                        BtnSalesSearchCustomer.PerformClick();
                        return true;
                    }

                    if (keyData == Keys.F3)
                    {
                        if (BtnSalesNew.Enabled)
                            BtnSalesNew_Click(this, null!);
                        else
                            BtnSalesNewCustomer.ShowDropDown();
                        return true;
                    }
                    if (keyData == Keys.F4 && BtnSalesDelete.Enabled)
                    {
                        BtnSalesDelete_Click(this, null!);
                        return true;
                    }

                    if (keyData == Keys.F6 && BtnReceivePayment.Enabled)
                    {
                        BtnReceivePayment_Click(this, null!);
                        return true;
                    }

                    if (keyData == Keys.F8 && BtnSalesSave.Enabled)
                    {
                        BtnSalesSave_Click(this, null!);
                        return true;
                    }

                    if (keyData == Keys.F9 && BtnSalesPrint.Enabled)
                    {
                        BtnSalesPrint_Click(this, null!);
                        return true;
                    }

                    if (keyData == Keys.F10 && BtnSalesExit.Enabled)
                    {
                        BtnSalesExit_Click(this, null!);
                        return true;
                    }

                    if (keyData == Keys.F11)
                    {
                        BtnAdditionalDetail.PerformClick();
                        return true;
                    }

                    if (keyData == Keys.Escape && BtnSalesCancel.Enabled)
                    {
                        BtnSalesCancel_Click(this, null!);
                        return true;
                    }
                }

                // 🔹 GRID SHORTCUTS (only when inside grid)
                if (inGrid && GridViewSalesItem.CurrentCell != null)
                {
                    int colIndex = GridViewSalesItem.CurrentCell.ColumnIndex;

                    if (colIndex == (int)SaleEntryTableColumn.PRODUCT)
                    {
                        if (keyData == Keys.F2 && !_isBarcodeProcessing)
                        {
                            SearchProduct();
                            return true;
                        }
                        if (keyData == Keys.Up || keyData == Keys.Down)
                        {
                            long? TempCustomerId = TextBoxSalesEntryCustomer.Id == null ? 0L : long.Parse(TextBoxSalesEntryCustomer.Id);
                            PrevPriceProductId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null) ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value : 0L;
                            _ = ShowPreviousPricesAsync((long)TempCustomerId, PrevPriceProductId);
                        }

                        if (keyData == Keys.Enter && !_isBarcodeProcessing)
                        {
                            // Debounce check (200ms)
                            if ((DateTime.Now - _lastBarcodeTime).TotalMilliseconds < 200)
                                return true;

                            _lastBarcodeTime = DateTime.Now;
                            _isBarcodeProcessing = true;

                            string barcode;

                            // Ensure last character is committed
                            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewSalesItem.EndEdit();

                            // Read from editing control if still active
                            if (GridViewSalesItem.EditingControl is TextBox tb)
                                barcode = tb.Text;
                            else
                                barcode = GridViewSalesItem.CurrentCell.Value?.ToString()!;

                            if (!string.IsNullOrEmpty(barcode))
                            {
                                ScannedBarcode = barcode.StartsWith(BarcodePrefix) ? barcode.Substring(BarcodePrefix.Length) : barcode;
                                var product = GetProductByBarcodeFromCache(ScannedBarcode);

                                if (product != null)
                                {
                                    LoadProductIntoGrid(product);
                                }
                            }
                            //ShowPreviousPrices(CustomerId, ProductId);
                            _isBarcodeProcessing = false;
                            return true;
                        }


                        if (keyData == Keys.F5)
                        {
                            SearchPreviousPrice();
                            return true;
                        }
                        if (keyData == Keys.F7)
                        {
                            ShowPrice();
                            return true;
                        }
                    }
                    if (colIndex == (int)SaleEntryTableColumn.QTY)
                    {
                        if (keyData == Keys.Enter)
                        {
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[1, GridViewSalesItem.CurrentRow.Index + 1];
                            GridViewSalesItem.CurrentCell.Selected = true;
                            GridViewSalesItem.BeginEdit(true);

                        }
                    }
                    if (keyData == Keys.F2 && colIndex == (int)SaleEntryTableColumn.BATNO && !GridViewSalesItem.CurrentCell.ReadOnly)
                    {
                        if (ComboBoxSaleInventoryLocation.SelectedIndex < 0)
                        {
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

                    // 🔹 keep your Tab navigation logic here (FREE, BATNO, OPRICE, DISP etc.)
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
                    if ((keyData == Keys.F5) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                    {
                        SearchPreviousPrice();
                        return true;
                    }
                    if (keyData == (Keys.Tab) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.FREE)
                    {
                        if (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.BATNO].ReadOnly)
                        {
                            SendKeys.Send("{tab}{tab}");
                            // SendKeys.Send("{tab}{tab}{tab}");
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
            }
            catch (Exception ex)
            {
                _isBarcodeProcessing = false;
                Console.WriteLine($"Error: {ex.Message}");
                return true;
            }

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
                        OPId = (long)Note.OpRegistrationId!;
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
                InventoryLocation Location = null!;
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

        private void checkBoxGST_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxGST.Text = checkBoxGST.Checked ? "GST Print" : "Non-GST Print";

            // Optional: You can also update the print button text if needed
            // BtnSalesPrint.Text = checkBoxGST.Checked ? "Print GST Invoice" : "Print Non-GST Invoice";
        }

        private void InitializePrintingComboBox()
        {
            try
            {
                DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
                DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();

                // Get the saved print paper format ID
                long? PrintPaperId = Global.Company.IdSpaces
                    .FirstOrDefault(x => x.YearStartDate == YearStartDate &&
                                         x.YearEndDate == YearEndDate &&
                                         x.EntryType == EntryType.SALES)
                    ?.PrintPaperFormat?.Id;

                // Get all paper formats and sort with A4 PORTRAIT first
                var allPaperFormats = PaperFormatManager.Instance.ListPrintPaperFormat();
                var sortedFormats = allPaperFormats
                    .OrderByDescending(x => x.Name == "A4 PORTRAIT")
                    .ThenBy(x => x.Name)
                    .ToList();

                // Bind the sorted list to ComboBox
                ComboBoxPrintingPaper.DataSource = sortedFormats;
                ComboBoxPrintingPaper.DisplayMember = "Name";
                ComboBoxPrintingPaper.ValueMember = "Id";

                // Try to select saved PrintPaperId
                if (PrintPaperId.HasValue &&
                    sortedFormats.Any(x => x.Id == PrintPaperId.Value))
                {
                    ComboBoxPrintingPaper.SelectedValue = PrintPaperId.Value;
                }
                // Else fallback to A4 PORTRAIT if available
                else if (sortedFormats.Any(x => x.Name == "A4 PORTRAIT"))
                {
                    ComboBoxPrintingPaper.SelectedValue =
                        sortedFormats.First(x => x.Name == "A4 PORTRAIT").Id;
                }
                // Else fallback to first item
                else if (sortedFormats.Count > 0)
                {
                    ComboBoxPrintingPaper.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing print format: {ex.Message}");
                // Fallback: hardcoded list
                ComboBoxPrintingPaper.DataSource = new List<PrintPaperFormat>
                {
                    new PrintPaperFormat { FormatId = 1, DisplayName = "A4 PORTRAIT" },
                    new PrintPaperFormat { FormatId = 2, DisplayName = "A5 LANDSCAPE" }
                };
                ComboBoxPrintingPaper.DisplayMember = "Name";
                ComboBoxPrintingPaper.ValueMember = "Id";
                ComboBoxPrintingPaper.SelectedIndex = 0;
            }
        }

        private bool SearchProductByBarCode(string barcode)
        {
            // First check if this is actually a barcode scan or just normal text entry
            if (!IsLikelyBarcode(barcode))
            {
                return false; // Let normal product search handle this
            }

            if (string.IsNullOrWhiteSpace(barcode))
            {
                DisplaySystemError("Please enter a valid barcode");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Clear any previous error
                ToolStripStatusLabelErrorPurchase.Text = string.Empty;

                // Search product by barcode (MaterialId) - only do this for actual barcodes
                var product = CatalogProductManager.Instance.GetProductByBarcode(Global.Company.CompanyId, barcode.Trim());
                if (product == null)
                {
                    DisplaySystemError($"Product with barcode '{barcode}' not found");
                    // Reset the cell value since this was a failed barcode scan
                    GridViewSalesItem.CurrentCell.Value = string.Empty;
                    GridViewSalesItem.BeginEdit(true);
                    return false;
                }

                // Product found - handle it directly
                ProductId = product.Id;

                // Check if product already exists in grid (for combine logic)
                bool isCombineProduct = Global.Company.CompanySalesSetup.CombineItem;
                if (isCombineProduct)
                {
                    foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                    {
                        if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null &&
                            row.Cells[(int)SaleEntryTableColumn.ID].Value.ToString() == ProductId.ToString())
                        {
                            // Product exists - increment quantity
                            double prevQty = row.Cells[(int)SaleEntryTableColumn.QTY].Value != null ?
                                double.Parse(row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!) : 0;

                            row.Cells[(int)SaleEntryTableColumn.QTY].Value = (prevQty + 1).ToString();
                            ComputeFormTotal();

                            // Focus and select quantity
                            row.Cells[(int)SaleEntryTableColumn.QTY].ReadOnly = false;
                            GridViewSalesItem.CurrentCell = row.Cells[(int)SaleEntryTableColumn.PRODUCT];
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[1, GridViewSalesItem.CurrentRow.Index];
                            GridViewSalesItem.CurrentCell.Selected = true;
                            GridViewSalesItem.BeginEdit(true);

                            var quantityTextBox = GridViewSalesItem.EditingControl as TextBox;
                            if (quantityTextBox != null)
                            {
                                quantityTextBox.SelectAll();
                            }

                            return true;
                        }
                    }
                }

                // New product - load details
                LoadUomTax(ProductId);
                LoadProductAdditinalDetails(product);

                // Set product details in current row
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value = product.Id;
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT].Value = product.Name;
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.QTY].Value = "1"; // Default to 1 for scanned items

                // Add new row if needed
                if (GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
                {
                    GridViewSalesItem.Rows.Add();
                }

                // Focus on quantity and select all text
                GridViewSalesItem.CurrentCell = GridViewSalesItem[
                    (int)SaleEntryTableColumn.QTY,
                    GridViewSalesItem.CurrentRow.Index];
                GridViewSalesItem.BeginEdit(true);

                var qtyTextBox = GridViewSalesItem.EditingControl as TextBox;
                if (qtyTextBox != null)
                {
                    qtyTextBox.SelectAll();
                }

                ComputeFormTotal();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                DisplaySystemError($"Error processing barcode: {ex.Message}");
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool SearchProductByBarCodexxx(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                DisplaySystemError("Please enter a valid barcode");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Search product by barcode (MaterialId)
                var product = CatalogProductManager.Instance.GetProductByBarcode(Global.Company.CompanyId, barcode.Trim());
                if (product == null)
                {
                    DisplaySystemError($"Product with barcode '{barcode}' not found");
                    GridViewSalesItem.CurrentCell = GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.PRODUCT];
                    GridViewSalesItem.BeginEdit(true);
                    return false;
                }

                // Product found - handle it directly
                ProductId = product.Id;

                // Check if product already exists in grid (for combine logic)
                bool isCombineProduct = Global.Company.CompanySalesSetup.CombineItem;
                if (isCombineProduct)
                {
                    foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                    {
                        if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null &&
                            long.TryParse(row.Cells[(int)SaleEntryTableColumn.ID].Value.ToString(), out long rowProductId) &&
                            rowProductId == ProductId)
                        {
                            if (int.TryParse(row.Cells[(int)SaleEntryTableColumn.QTY].Value?.ToString(), out int prevQty))
                            {
                                row.Cells[(int)SaleEntryTableColumn.QTY].Value = (prevQty + 1).ToString();
                                ComputeFormTotal();

                                GridViewSalesItem.CurrentCell = row.Cells[(int)SaleEntryTableColumn.QTY];
                                GridViewSalesItem.BeginEdit(true);
                                var quantityTextBox = GridViewSalesItem.EditingControl as TextBox;
                                if (quantityTextBox != null)
                                {
                                    quantityTextBox.SelectAll();
                                }
                                return true;
                            }
                        }
                    }
                }

                // New product - load details
                LoadUomTax(ProductId);
                LoadProductAdditinalDetails(product);

                // Add new row if needed
                if (GridViewSalesItem.Rows.Count - 1 == GridViewSalesItem.CurrentRow.Index)
                {
                    GridViewSalesItem.Rows.Add();
                }

                // Set default quantity to 1 for new items
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.QTY].Value = "1";

                // Focus on quantity and select all text
                GridViewSalesItem.CurrentCell = GridViewSalesItem[
                    (int)SaleEntryTableColumn.QTY,
                    GridViewSalesItem.CurrentRow.Index];
                GridViewSalesItem.BeginEdit(true);
                var qtyTextBox = GridViewSalesItem.EditingControl as TextBox;
                if (qtyTextBox != null)
                {
                    qtyTextBox.SelectAll();
                }

                ComputeFormTotal();
                return true;
            }
            catch (Exception ex)
            {
                DisplaySystemError($"Error processing barcode: {ex.Message}");
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private static ConcurrentDictionary<string, Product> _barcodeCache = new();


        public void ClearBarcodeCache()
        {
            _barcodeCache.Clear();
        }
        private void InitializeBarcodeHandling()
        {
            GridViewSalesItem.EditingControlShowing += (sender, e) =>
            {
                if (e.Control is TextBox textBox &&
                    GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                {
                    textBox.KeyDown -= ProductTextBox_KeyDown!;
                    textBox.KeyDown += ProductTextBox_KeyDown!;
                    textBox.PreviewKeyDown -= ProductTextBox_PreviewKeyDown!;
                    textBox.PreviewKeyDown += ProductTextBox_PreviewKeyDown!;
                }
            };
        }
        private void InitializeScannerHandling()
        {
            GridViewSalesItem.CellEndEdit += (sender, e) =>
            {
                if (e.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
                {
                    HandlePotentialBarcodeInput();
                }
            };
        }

        private async void HandlePotentialBarcodeInput()
        {
            // Debounce check (200ms threshold)
            if ((DateTime.Now - _lastScannerInput).TotalMilliseconds < 200)
                return;

            _lastScannerInput = DateTime.Now;

            var cell = GridViewSalesItem.CurrentCell;
            if (cell == null || cell.Value == null) return;

            var barcode = cell.Value.ToString();
            if (string.IsNullOrWhiteSpace(barcode)) return;

            try
            {
                _isScannerInput = true;

                // Process barcode directly
                if (!SearchProductByBarCode(barcode))
                {
                    // Product not found - keep focus in cell
                    GridViewSalesItem.BeginEdit(true);
                }
            }
            finally
            {
                await Task.Delay(300); // Safety delay
                _isScannerInput = false;
            }
        }
        private void ProductTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                var textBox = (TextBox)sender;
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    // Debounce check
                    if ((DateTime.Now - _lastBarcodeTime).TotalMilliseconds < 200) return;
                    _lastBarcodeTime = DateTime.Now;

                    // Direct barcode processing
                    //SearchProductByBarCode(textBox.Text);
                }
            }
        }
        private void ProductTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Mark Enter as not input key so ProcessCmdKey won't handle it
            if (e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = false;
            }
        }


        private void InitializePrintingComboBoxxxx()
        {
            try
            {

                IList<PrintPaperFormat> formats = (IList<PrintPaperFormat>)PaperFormatManager.Instance.ListPrintPaperFormat();

                ComboBoxPrintingPaper.DataSource = formats;
                ComboBoxPrintingPaper.DisplayMember = "DisplayName";
                ComboBoxPrintingPaper.ValueMember = "FormatId";

                // Optional: Format how items appear in the dropdown
                ComboBoxPrintingPaper.Format += (sender, e) =>
                {
                    if (e.ListItem is PrintPaperFormat format)
                        e.Value = $"{format.DisplayName} ({format.Dimensions})";
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load paper formats: {ex.Message}");
            }
        }

        private void GridViewSalesItem_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == (int)SaleEntryTableColumn.QTY)
            {
                // Select all text when entering QTY cell
                BeginInvoke(new Action(() =>
                {
                    if (GridViewSalesItem.EditingControl is TextBox textBox)
                    {
                        textBox.SelectAll();
                    }
                }));
            }
        }

        private void GridViewSalesItem_SelectionChanged(object sender, EventArgs e)
        {
            if (dont_jump)
            {
                dont_jump = false;
                GridViewSalesItem.CurrentCell = GridViewSalesItem[col_index, row_index];
            }
        }

        private float GetBasePrice(Product product, string uom)
        {
            PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

            // If product uses different prices per UOM, check that here
            if (selectedPriceType == PriceType.MaxRetailPrice)
                return product.Msrp;
            else if (selectedPriceType == PriceType.Wholesale)
                return product.WholdSalePrice;
            else
                return product.RetailPrice;
        }
        private decimal GetPriceByPriceType(Product product)
        {
            PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

            switch (selectedPriceType)
            {
                case PriceType.Wholesale:
                    return (decimal)product.WholdSalePrice;
                case PriceType.MaxRetailPrice:
                    return (decimal)product.Msrp;
                default:
                    return (decimal)product.RetailPrice;
            }
        }

        private void ComboBoxInvoicePriceBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (YesNoRadioPriceTo.Checked) // "All" is selected
            {
                UpdateAllRowsPrices();
            }
            else // "Single" is selected
            {
                UpdateCurrentRowPrice();
            }
        }
        private void UpdateCurrentRowPrice()
        {
            if (GridViewSalesItem.CurrentRow != null &&
                GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
            {
                long productId = (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value;
                string uom = GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.UOM].Value?.ToString()!;

                if (!string.IsNullOrEmpty(uom))
                {
                    LoadPrice(GridViewSalesItem.CurrentRow.Index, uom);
                    ComputeFormTotal();
                }
            }
        }

        private void UpdateAllRowsPrices()
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow row in GridViewSalesItem.Rows)
            {
                // Skip the empty row at the end if present
                if (row.IsNewRow || row.Cells[(int)SaleEntryTableColumn.ID].Value == null)
                    continue;

                long productId = (long)row.Cells[(int)SaleEntryTableColumn.ID].Value;
                string uom = row.Cells[(int)SaleEntryTableColumn.UOM].Value?.ToString()!;

                if (!string.IsNullOrEmpty(uom))
                {
                    // Temporarily set current row to update prices correctly
                    GridViewSalesItem.CurrentCell = row.Cells[0];

                    // Load price for this row with the new price type
                    LoadPrice(row.Index, uom);
                }
            }

            ComputeFormTotal();
            Cursor.Current = Cursors.Default;
        }
        //bool IsOverrideTabCtr = true;
        private StringBuilder barcodeBuffer = new StringBuilder();
        private DateTime lastKeystrokeTime = DateTime.Now;
        private const int barcodeTimeoutMs = 50; // ms between scanner keystrokes

        private Product GetProductByBarcodeFromCacheOld(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return null!;

            // Ensure list is loaded
            if (Global.ProductDetailList == null || Global.ProductDetailList.Count == 0)
            {
                Global.ProductDetailList = CatalogProductManager.Instance
                    .ListProductByCompanyId(Global.Company.CompanyId);
            }

            // Match exactly like FormSearchItems does
            var product = Global.ProductDetailList
                .FirstOrDefault(x => x.MaterialId.Equals(barcode.Trim(), StringComparison.OrdinalIgnoreCase));

            return product!;
        }
        private char ConvertKeyCodeToChar(Keys key)
        {
            if (key >= Keys.D0 && key <= Keys.D9)
                return (char)('0' + (key - Keys.D0));
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                return (char)('0' + (key - Keys.NumPad0));
            if (key >= Keys.A && key <= Keys.Z)
                return (char)('A' + (key - Keys.A));
            return '\0';
        }

        private Product GetProductByBarcodeFromCache(string barcode)
        {
            if (Global.ProductDetailList == null || Global.ProductDetailList.Count == 0)
            {
                Global.ProductDetailList = CatalogProductManager.Instance
                    .ListProductByCompanyId(Global.Company.CompanyId);
            }

            return Global.ProductDetailList
                .FirstOrDefault(x => x.MaterialId.Equals(barcode, StringComparison.OrdinalIgnoreCase))!;
        }

        private void LoadProductIntoGrid(Product product)
        {
            long productId = product.Id;
            long currentId = (GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value != null)
                ? (long)GridViewSalesItem.CurrentRow.Cells[(int)SaleEntryTableColumn.ID].Value
                : 0L;

            // Combine product if needed
            if (Global.Company.CompanySalesSetup.CombineItem)
            {
                foreach (DataGridViewRow row in GridViewSalesItem.Rows)
                {
                    if (row.Cells[(int)SaleEntryTableColumn.ID].Value != null &&
                        (long)row.Cells[(int)SaleEntryTableColumn.ID].Value == productId)
                    {
                        int finalQty = int.Parse(row.Cells[(int)SaleEntryTableColumn.QTY].Value.ToString()!) + 1;
                        row.Cells[(int)SaleEntryTableColumn.QTY].Value = finalQty.ToString();
                        ComputeFormTotal();
                        return;
                    }
                }
            }

            LoadUomTax(productId);
            LoadProductAdditinalDetails(product);

            if (currentId == 0 && GridViewSalesItem.CurrentRow.Index == GridViewSalesItem.Rows.Count - 1)
            {
                GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewSalesItem.Rows.Add();
            }

            GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);

            GridViewSalesItem.BeginInvoke(new MethodInvoker(delegate ()
            {
                GridViewSalesItem.CurrentCell = GridViewSalesItem[3, GridViewSalesItem.CurrentRow.Index];
                GridViewSalesItem.CurrentCell.Selected = true;
                GridViewSalesItem.BeginEdit(true);
            }));
        }

        private void GridViewSalesItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.QTY)
            //    {
            //        SendKeys.Send("+{tab}+{tab}");
            //        e.IsInputKey = true;
            //    }
            //}
        }

        private void BtnWhatsUpEmail_Click(object sender, EventArgs e)
        {

            long saleId = string.IsNullOrWhiteSpace(TextBoxSalesId.Text) ? 0 : long.Parse(TextBoxSalesId.Text);

            string paperSelection = "";

            //var selectedFormat = ComboBoxPrintingPaper.SelectedItem as PrintPaperFormat;
            if (ComboBoxPrintingPaper.SelectedItem is PrintPaperFormat selectedFormat)
            {
                // Use selected format's name
                paperSelection = selectedFormat.DisplayName!;
            }
            else if (ComboBoxPrintingPaper.Items.Count > 0)
            {
                // Fallback to first item's name if nothing selected
                if (ComboBoxPrintingPaper.Items[0] is PrintPaperFormat firstFormat)
                {
                    paperSelection = firstFormat.DisplayName!;
                }
                else
                {
                    // If casting fails, use the displayed text
                    paperSelection = ComboBoxPrintingPaper.Text;
                }
            }
            else
            {
                // If combo box is empty, use the displayed text
                paperSelection = ComboBoxPrintingPaper.Text;
            }

            FormEmailWhatsUpSender formEmailSend = new FormEmailWhatsUpSender();
            formEmailSend.Document = new SalesInvoiceSharable(saleId, paperSelection);
            //formEmailSend.CurrentShareType = ShareDocumentType.SalesInvoice;
            formEmailSend.SaleEntryId = saleId;

            formEmailSend.ShowDialog(this);

        }

        private CancellationTokenSource _priceCts;

        private async Task ShowPreviousPricesAsync(long customerId, long productId)
        {
            _priceCts?.Cancel();
            _priceCts = new CancellationTokenSource();
            var token = _priceCts.Token;

            try
            {
                // Reset immediately (fast feedback)
                PurchaseRate.Text = "0.00";
                SellingRate.Text = "0.00";

                await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();

                    // 🔹 Last selling price
                    var sales = SalesManager.Instance
                        .DisplayLastPricesByProductAndCustomer(
                            Global.Company.CompanyId,
                            productId,
                            customerId
                        );

                    var lastSale = sales?.FirstOrDefault();

                    // 🔹 Purchase price
                    var product = CatalogProductManager.Instance
                        .GetProductInfoByProductId(Global.Company.CompanyId, productId);

                    // UI update on main thread
                    BeginInvoke(new Action(() =>
                    {
                        if (token.IsCancellationRequested) return;

                        if (lastSale != null)
                        {
                            SellingRate.Text =
                                lastSale.Price.ToString(
                                    Global.Company.PrimaryCurrency.CurrencyFormat
                                );
                        }

                        if (product != null && product.PurchasePrice > 0)
                        {
                            PurchaseRate.Text =
                                product.PurchasePrice.ToString(
                                    Global.Company.PrimaryCurrency.CurrencyFormat
                                );
                        }
                    }));
                }, token);
            }
            catch (OperationCanceledException)
            {
                // Expected → ignore
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private bool PriceValidation()
        {
            // Loop through all rows
            foreach (DataGridViewRow row in GridViewSalesItem.Rows)
            {
                // Skip new empty row
                // Skip new empty row
                // Skip new empty row
                if (row.IsNewRow)
                    continue;

                int rowIndex = row.Index;

                // Get price cell
                DataGridViewCell priceCell =
                    GridViewSalesItem[(int)SaleEntryTableColumn.PRICE, rowIndex];

                // Check null
                if (priceCell == null || priceCell.Value == null)
                    continue;

                // Convert selling price
                decimal sellPrice = 0;

                if (!decimal.TryParse(
                        priceCell.Value.ToString(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out sellPrice))
                {
                    continue;
                }

                // Get ProductId cell value
                object productIdObject =
                    GridViewSalesItem[(int)SaleEntryTableColumn.ID, rowIndex].Value;

                if (productIdObject == null)
                    continue;

                // Convert ProductId
                long productId = 0;

                if (!long.TryParse(productIdObject.ToString(), out productId))
                    continue;

                // Get product from database
                Product product =
                    CatalogProductManager.Instance
                    .GetProductInfoByIdForProductLoad(productId);

                if (product == null)
                    continue;

                // Get cost price
                decimal costPrice = Convert.ToDecimal(product.CostPrice);

                // Validation
                if (sellPrice < costPrice)
                {
                    // Product name
                    string productName =
                        GridViewSalesItem[(int)SaleEntryTableColumn.PRODUCT, rowIndex]
                        ?.Value?.ToString()
                        ?? productId.ToString();

                    // Show warning
                    MessageBox.Show(
                        $"Selling price for '{productName}' is less than cost price."
                        + Environment.NewLine + Environment.NewLine
                        + $"Cost Price : {costPrice}"
                        + Environment.NewLine
                        + $"Selling Price : {sellPrice}",
                        "Price Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Select row
                    row.Selected = true;

                    // Focus exact price cell
                    GridViewSalesItem.CurrentCell = priceCell;

                    // Scroll to row
                    GridViewSalesItem.FirstDisplayedScrollingRowIndex = row.Index;

                    // Focus grid
                    GridViewSalesItem.Focus();

                    // Enter edit mode
                    GridViewSalesItem.BeginEdit(true);

                    return false;
                }
            }

            return true;
        }
        /*
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
    if (GridViewSalesItem.CurrentCell.ColumnIndex != (int)SaleEntryTableColumn.PRODUCT)
    {
       return base.ProcessCmdKey(ref msg, keyData);
    }
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
           BtnSalesNew_Click(this, null!);
       }
       else
       {
           BtnSalesNewCustomer.ShowDropDown();
       }
    }
    else if (keyData == (Keys.F4) && BtnSalesDelete.Enabled)
    {
       BtnSalesDelete_Click(this, null!);
    }
    else if (keyData == (Keys.F6) && BtnReceivePayment.Enabled)
    {
       BtnReceivePayment_Click(this, null!);
    }
    else if (keyData == (Keys.F9) && BtnSalesPrint.Enabled)
    {
       BtnSalesPrint_Click(this, null!);
    }
    else if (keyData == (Keys.F8) && BtnSalesSave.Enabled)
    {
       BtnSalesSave_Click(this, null!);
    }
    else if (keyData == (Keys.F11))
    {
       BtnAdditionalDetail.PerformClick();
    }
    else if (keyData == (Keys.Escape) && BtnSalesCancel.Enabled)
    {
       BtnSalesCancel_Click(this, null!);
       return false;
    }
    else if (keyData == (Keys.F10) && BtnSalesExit.Enabled)
    {
       BtnSalesExit_Click(this, null!);
       return true;
    }
    try
    {
       if (GridViewSalesItem.CurrentCell.ColumnIndex != (int)SaleEntryTableColumn.PRODUCT)
       {
           return base.ProcessCmdKey(ref msg, keyData);
       }

       // 1. Handle F2 - ONLY opens SearchProduct()
       if (keyData == Keys.F2)
       {
           if (!_isBarcodeProcessing) // Ensure we're not scanning
           {
               SearchProduct();
           }
           return true;
       }

       // 2. Handle Enter key - ONLY for barcode scanning
       if (keyData == Keys.Enter && !_isBarcodeProcessing)
       {
           // Debounce check (200ms)
           if ((DateTime.Now - _lastBarcodeTime).TotalMilliseconds < 200)
               return true;

           _lastBarcodeTime = DateTime.Now;
           _isBarcodeProcessing = true;

           string barcode;

           // Ensure last character is committed
           GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
           GridViewSalesItem.EndEdit();

           // Read from editing control if still active
           if (GridViewSalesItem.EditingControl is TextBox tb)
               barcode = tb.Text;
           else
               barcode = GridViewSalesItem.CurrentCell.Value?.ToString()!;

           if (!string.IsNullOrEmpty(barcode))
           {
               ScannedBarcode = barcode.StartsWith(BarcodePrefix) ? barcode.Substring(BarcodePrefix.Length) : barcode;
               var product = GetProductByBarcodeFromCache(ScannedBarcode);

               if (product != null)
               {
                   LoadProductIntoGrid(product);
               }
           }

           _isBarcodeProcessing = false;
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
       if ((keyData == Keys.F5) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleEntryTableColumn.PRODUCT)
       {
           SearchPreviousPrice();
           return true;
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
    catch (Exception ex)
    {
       _isBarcodeProcessing = false;
       Console.WriteLine($"Error: {ex.Message}");
       return true;
    }
    return base.ProcessCmdKey(ref msg, keyData);
    }
    */

    }
    public class PrintPaperFormat
    {
        public int FormatId { get; set; }
        public string? DisplayName { get; set; }
        public string? Dimensions { get; set; }
    }
}