using fa.api.Accounting;
using fa.api.catalog;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.account.masters;
using fa.views.common;
using fa.views.controls.accounting;
using fa.views.controls.grid;
using fa.views.controls.text;
using fa.views.purchase;
using fa.views.utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using VisioForge.Libs.NDI;

namespace fa.views.sales
{
    public enum SaleQuoteEntryTableColumn
    {
        SNO, PRODUCT, UOM, QTY, FREE, PRICE, OPRICE, TAXP, TAX, DISP, DIS, AMOUNT, REMOVE, ID, ISBAT, SALESDETAILID, BATCHID
    }
    public partial class FormQuote : FormBase
    {
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string DeleteErrorText = "Error in Sales Quote Deleting. !";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string DeleteConfirmText = "Do you want to delete Sale Quote Entry {0}?";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string ChooseCustomerErrorMsg = "Please select customer.";
        public static string EnterSaleQuoteEntryDateErrorMsg = "Please enter sale Quote date.";
        public static string EnterExpDateErrorMsg = "Please enter Exp date.";

        public static string Grid_DeleteConfirmText = "Do you want to delete Sale Quote Entry {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter sale Quote Items details.";
        public static string Grid_InvalidBatchErrorMsg = "Batch not available.";
        public static string Grid_InvalidDisErrorMsg = "Please enter valid discount.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";

        public static string InvalidSaleQuoteEntryErrorMsg = "Invalid sale.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Customer Name or Quote Date or Quote Reference Number.";
        public static string SaleQuoteSearchOutput = "No sale quote found";

        SalesManager SalesManager = null!;

        public long ProductId = 0L;
        public long BatchId;
        public long CustomerId = 0L;
        public long SearchSalesId = 0L;
        public FormQuote()
        {
            InitializeComponent();
            SalesManager = SalesManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "groupBox2", "DiscountAdditinalChargeGrid" };
            ComboBoxInvoicePriceBy.SelectedIndexChanged += ComboBoxInvoicePriceBy_SelectedIndexChanged!;
            //YesNoRadioPriceTo.CheckedChanged += YesNoRadioPriceTo_CheckedChanged!;
        }
        private void FormQuote_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Visible = false;
            setSize();
            this.Visible = true;
            ResetForm();
            EnableForm(true);
            TextBoxSaleQuoteCustomer.Focus();
            DirtyFlag(false);
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
            lSaleEntry.Id = TextBoxSalesQuotesId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxSalesQuotesId.Text);
            lSaleEntry.RefNumber = SalesQuotesReferenceNumber.Text;
            if (TextBoxSaleQuoteCustomer.Id == null)
            {
                lSaleEntry.CustomerName = TextBoxSaleQuoteCustomer.Text;
            }
            else
            {
                Customer Customer = CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxSaleQuoteCustomer.Id));
                if (Customer != null)
                {
                    lSaleEntry.AccountsId = Customer.Id;
                    lSaleEntry.CustomerName = Customer.Name;
                    lSaleEntry.CustomerAddress = Customer.BillingAddress.FullAddress;
                }
            }
            lSaleEntry.SaleType = YesNoRadioSaleType.Checked ? SaleType.Retail : SaleType.WholeSale;
            lSaleEntry.CustomerAddress = TextBoxSalesQuotesCustomerAddress.Text;
            lSaleEntry.SaleMethod = (YesNoRbtSalesQuotesMethod.Checked) ? SaleMethod.Credit : SaleMethod.Cash;
            lSaleEntry.SaleDate = (DateTime)DatetimePickerSalesQuotesDate.Date!;
            lSaleEntry.QuotaionExpireAt = (DateTime)DatetimePickerSalesQuotesExp.Date!;
            lSaleEntry.CompanyId = Global.Company.CompanyId;
            lSaleEntry.Memo = TextBoxQuoteMemo.Text;
            //FOR SQL
            lSaleEntry.ReturnDate = DateTime.Now.Date;
            if (Global.CostCenter != null)
            {
                lSaleEntry.CostCenterId = Global.CostCenter.CostCenterId;
            }
            float TotalAmount = float.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value.ToString()!);
            float TotalTaxAmount = 0;
            float TotalDiscountPercentage = 0;
            float TotalDiscountAmount = 0;
            float[] TaxWisePercentageTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            for (int i = 0; i < GridViewSalesQuotesItem.Rows.Count - 1; i++)
            {
                SaleDetail SaleDetail = new SaleDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    SaleDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        SaleDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    SaleDetail.Id = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value == null) ? 0L : long.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value.ToString()!);
                    SaleDetail.ProductId = Product.Id;
                    SaleDetail.MaterialId = Product.MaterialId;
                    SaleDetail.isBatch = (bool)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value;
                    SaleDetail.Uom = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value.ToString();
                    //FOR SQL
                    SaleDetail.ExpDate = DateTime.Now.Date;
                    double Quantity = 0;
                    if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value != null)
                    {
                        Quantity = double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value.ToString()!);
                    }
                    double FreeQuantity = 0;
                    if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value != null)
                    {
                        FreeQuantity = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value.ToString()!);
                    }
                    float Pprice = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value != null ? float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value.ToString()!) : 0;
                    if (FreeQuantity > 0)
                    {
                        SaleDetail.isFree = true;
                    }
                    SaleDetail.Quantity = Quantity;
                    SaleDetail.FreeQuantity = FreeQuantity;
                    SaleDetail.Price = Pprice;
                    SaleDetail.OverridePrice = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value != null ? float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value.ToString()!) : 0;
                    SaleDetail.Amount = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value != null ? float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value.ToString()!) : 0;
                    double Amount = Quantity * Pprice;
                    float Discount = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value != null ? float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value.ToString()!) : 0;
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
                    float TaxPercentage = float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value.ToString()!);
                    if (TaxPercentage > 0)
                    {
                        int j = 0;

                        if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value != null)
                        {
                            SaleDetail SalesDetail = SalesManager.GetSaleDetail((long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value);
                            if (SalesDetail.TaxDetails.Count > 0)
                            {
                                foreach (ItemLevelSaleTaxDetail SaleTaxDetail in SalesDetail.TaxDetails)
                                {
                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + SaleTaxDetail.TaxRate;
                                    float TaxAmount = (float)Amount * (SaleTaxDetail.TaxRate / 100);
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
                            DateTime CurrentDate = DatetimePickerSalesQuotesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesQuotesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) ? (DateTime)DatetimePickerSalesQuotesDate.Date : Global.getTransactionDate();
                            foreach (CatalogItemSalesTaxMap PTaxMap in Product.SalesTax)
                            {
                                if (PTaxMap.EffectiveFrom <= CurrentDate && PTaxMap.EffectiveTo >= CurrentDate)
                                {
                                    CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId!);
                                    if (CMap != null)
                                    {
                                        if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, CurrentDate, (string.IsNullOrEmpty(TextBoxSaleQuoteCustomer.Id) ? 0L : long.Parse(TextBoxSaleQuoteCustomer.Id))))
                                        {
                                            if (PTaxMap.TaxPercentage > 0)
                                            {
                                                TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                float TaxAmount = (float)Amount * (PTaxMap.TaxPercentage / 100);
                                                TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                ItemLevelSaleTaxDetail ItemSaleTaxDetail = new ItemLevelSaleTaxDetail();
                                                ItemSaleTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId)!.AccountId/* PTaxMap.SalesTaxMapId*/;
                                                ItemSaleTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                ItemSaleTaxDetail.Amount = TaxAmount;
                                                ItemSaleTaxDetail.ItemTaxMapId = PTaxMap.Id;
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
            lSaleEntry.TotalAmount = double.Parse(LabelQuotesSalesFinalAmount.Text);

            //round off
            lSaleEntry.RoundOff = TotalAmount - lSaleEntry.TotalAmount;
            lSaleEntry.NetAmount = TotalAmount;

            return lSaleEntry;
        }
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
                else if (mod == 0)
                {
                    _roundoff = mod;
                }
                else
                {
                    _roundoff = -mod;
                }
            }
            return _roundoff;
        }
        private void TextBoxSaleQuoteCustomer_TextChanged(object sender, EventArgs e)
        {
            TextBoxSaleQuoteCustomer.Id = null!;
        }
        private void BtnSalesQuotesNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnSalesQuotesSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    YesNoRbtSalesQuotesMethod.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            YesNoRbtSalesQuotesMethod.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSalesQuotesDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSalesQuotesId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this sale quote is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, SalesQuotesReferenceNumber.Text), "Delete Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long SalesID = Convert.ToInt64(TextBoxSalesQuotesId.Text);
                SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesID);
                if (SaleEntry != null)
                {
                    bool DeleteResult = SalesManager.DeleteSaleEntry(SalesID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        YesNoRbtSalesQuotesMethod.Focus();
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
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            DirtyFlag(false);
            BtnSalesQuotesCancel.PerformClick();
            return;
        }
        private void BtnSalesQuotesPrint_Click(object sender, EventArgs e)
        {
            //if (SalesManager.GetSaleEntry(long.Parse(TextBoxSalesQuotesId.Text)) != null)
            //{
            //    Cursor.Current = Cursors.WaitCursor;
            //    PrinterSetup.SalePrintSetup(long.Parse(TextBoxSalesQuotesId.Text), false, Entrytype.QUOTE);
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            //    DisplaySystemError("Somting went wrong, please check this sale quote is still valid.");
            //    return;
            //}

        }
        private void BtnSalesQuotesCancel_Click(object sender, EventArgs e)
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
            DatetimePickerSalesQuotesDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSalesQuotesSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    SaleEntry lSales = GetSaleEntryFromForm();
                    lSales.EntryType = Entrytype.QUOTE;
                    SaleEntry lSalesFromDB = null!;
                    if (lSales.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.SALES_QUOTE, (DateTime)DatetimePickerSalesQuotesDate.Date!);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lSales.RefNumber = RefNumber;
                            lSalesFromDB = new SaleEntry();
                            try
                            {
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
                        if (SalesManager.GetSaleEntry(lSales.Id) != null)
                        {
                            lSalesFromDB = new SaleEntry();
                            lSalesFromDB = SalesManager.UpdateSaleEntry(lSales);
                        }
                        else
                        {
                            DisplaySystemError("Somthing went wrong, the selected quote is not valid.");
                            return;
                        }
                    }

                    TextBoxSalesQuotesId.Text = lSalesFromDB.Id.ToString();
                    SalesQuotesReferenceNumber.Text = lSalesFromDB.RefNumber;

                    if (lSalesFromDB != null)
                    {
                        EnableForm(false);
                        LoadSaleEntry(lSalesFromDB.Id);
                        BtnSalesQuotesPrint.Select();
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
        private void BtnSalesQuotesExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            if (string.IsNullOrEmpty(TextBoxSaleQuoteCustomer.Text.Trim()) || TextBoxSaleQuoteCustomer.Id == null ||
                string.IsNullOrEmpty(TextBoxSaleQuoteCustomer.Id.Trim()) || CustomerManager.Instance.GetCustomerById(long.Parse(TextBoxSaleQuoteCustomer.Id)) == null)
            {
                BtnSalesQuotesSearchCustomer.Select();
                ToolStripStatusLabelErrorPurchase.Text = ChooseCustomerErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DatetimePickerSalesQuotesDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerSalesQuotesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerSalesQuotesDate.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterSaleQuoteEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }

            if (DatetimePickerSalesQuotesExp.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerSalesQuotesExp.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerSalesQuotesExp.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterExpDateErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewSalesQuotesItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value == null)
                    {
                        GridViewSalesQuotesItem.Select();
                        GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.PRODUCT, i];
                        GridViewSalesQuotesItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value == null
                        || string.IsNullOrEmpty(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value.ToString())
                        )
                    {
                        GridViewSalesQuotesItem.Select();
                        GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.UOM, i];
                        GridViewSalesQuotesItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewSalesQuotesItem.Columns[(int)SaleQuoteEntryTableColumn.UOM].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value.ToString()!);
                    double Free = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value.ToString()!);

                    for (int j = 3; j < 12; j++)
                    {
                        double Price = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value != null ? double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value.ToString()!) : 0.00;
                        double Oprice = GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value != null ? double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value.ToString()!) : 0.00;

                        if ((j == (int)SaleQuoteEntryTableColumn.QTY || j == (int)SaleQuoteEntryTableColumn.FREE) && !(Quantity <= 0 && Free <= 0))
                        { continue; }
                        if (j == (int)SaleQuoteEntryTableColumn.FREE)
                        { continue; }
                        if (j == (int)SaleQuoteEntryTableColumn.PRICE && !(Oprice <= 0 && Price <= 0))
                        { continue; }
                        if (j == (int)SaleQuoteEntryTableColumn.OPRICE && !(Oprice <= 0 && Price <= 0))
                        { j = j + 2; continue; }
                        if (j == 9 && GridViewSalesQuotesItem.Rows[i].Cells[j].Value != null &&
                            !string.IsNullOrEmpty(GridViewSalesQuotesItem.Rows[i].Cells[j].Value.ToString()) &&
                            float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[j].Value.ToString()!) > 100)
                        {
                            GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[j, i];
                            GridViewSalesQuotesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = Grid_InvalidDisErrorMsg;
                            ResetTimmer();
                            return false;
                        }
                        if (j == (int)SaleQuoteEntryTableColumn.DISP)
                        { j = j + 2; continue; }
                        if (j == (int)SaleQuoteEntryTableColumn.TAXP)
                        { continue; }
                        if ((j == (int)SaleQuoteEntryTableColumn.TAXP || j == (int)SaleQuoteEntryTableColumn.TAX) && Quantity <= 0 && Free > 0 && Oprice <= 0 && Price <= 0)
                        { continue; }
                        if (j != (int)SaleQuoteEntryTableColumn.AMOUNT && (GridViewSalesQuotesItem.Rows[i].Cells[j].Value == null || GridViewSalesQuotesItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[j].Value.ToString()!) <= 0))
                        {
                            GridViewSalesQuotesItem.Select();
                            if (j == 5)
                            {
                                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[j + 1, i];
                            }
                            else
                            {
                                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[j, i];
                            }
                            GridViewSalesQuotesItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewSalesQuotesItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }

                    }
                }
            }
            else
            {
                GridViewSalesQuotesItem.Select();
                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.PRODUCT, 0];
                GridViewSalesQuotesItem.BeginEdit(true);
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
            if (float.Parse(LabelQuotesSalesFinalAmount.Text) < 0)
            {
                GridViewSalesQuotesItem.Select();
                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.PRODUCT, 0];
                GridViewSalesQuotesItem.BeginEdit(true);
                ToolStripStatusLabelErrorPurchase.Text = InvalidSaleQuoteEntryErrorMsg;
                ResetTimmer();
                return false;
            }
            ToolStripStatusLabelErrorPurchase.Text = SaveSuccessText;
            return true;
        }
        private void ResetForm()
        {
            SalesQuotesReferenceNumber.Text = "000000";
            TextBoxSaleQuoteSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorPurchase.Text = "";
            TextBoxSalesQuotesId.ResetText();
            TextBoxSalesQuotesCustomerAddress.ResetText();
            DatetimePickerSalesQuotesDate.Format = Global.Company.DateFormat;
            DatetimePickerSalesQuotesDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            YesNoRadioSaleType.Checked = (Global.Company.BusinessType == BuisnessType.Wholesale ? false : true);
            DatetimePickerSalesQuotesExp.Format = Global.Company.DateFormat;
            DatetimePickerSalesQuotesExp.maskedTextBox.Text = string.Empty;
            PrevSalesQuotesReferenceNumber.Text = CompanyManager.Instance.GetSalePrevRef(Global.Company, Entrytype.QUOTE, (DateTime)DatetimePickerSalesQuotesDate.Date);
            YesNoRbtSalesQuotesMethod.Checked = true;
            TextBoxSaleQuoteCustomer.ResetText();
            GridViewSalesQuotesItem.Rows.Clear();
            GridViewSalesQuotesItem.Rows.Add();
            DiscountAdditinalChargeGrid.GridType = GridType.Sales;
            DiscountAdditinalChargeGrid.Clear();
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LabelQuotesSalesFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxSalesQuotesCustomerAddress.ResetText();
            TextBoxQuoteMemo.ResetText();
            DatetimePickerSalesQuotesExp.MinDate = Global.getTransactionDate();
            YesNoRadioPriceTo.Checked = true;
            ComboBoxInvoicePriceBy.SelectedIndex = (int)Global.Company.CompanySalesSetup.PriceType;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewSalesQuotesItem.Columns["QuotePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewSalesQuotesItem.Columns["OverridePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewSalesQuotesItem.Columns["QuoteTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewSalesQuotesItem.Columns["QuoteDic"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewSalesQuotesItem.Columns["QuoteAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
            DataGridViewCurrencyColumn currencyColumn5 = (DataGridViewCurrencyColumn)GridViewPurchaseItemTotal.Columns["Value"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces5)) currencyColumn5.DecimalPlaces = decimalPlaces5;
        }
        private void EnableForm(Boolean enable)
        {
            BtnSalesQuotesDelete.Enabled = true;
            BtnSalesQuotesSave.Enabled = true;
            YesNoRbtSalesQuotesMethod.Enabled = true;
            DatetimePickerSalesQuotesDate.ReadOnly = false;
            DatetimePickerSalesQuotesDate.TabStop = true;
            DatetimePickerSalesQuotesExp.ReadOnly = false;
            DatetimePickerSalesQuotesExp.TabStop = true;
            TextBoxSaleQuoteCustomer.ReadOnly = false;
            TextBoxSaleQuoteCustomer.TabStop = true;
            TextBoxSalesQuotesCustomerAddress.ReadOnly = false;
            TextBoxSalesQuotesCustomerAddress.TabStop = true;
            BtnSalesQuotesNewCustomer.Enabled = true;
            BtnSalesQuotesSearchCustomer.Enabled = true;
            GridViewSalesQuotesItem.Enabled = true;
            DiscountAdditinalChargeGrid.Enabled = true;
            GridViewSalesQuotesItem.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnSalesQuotesNew.Enabled = !enable;
                BtnSalesQuotesDelete.Enabled = !enable;
                BtnSalesQuotesPrint.Enabled = !enable;
                BtnSalesQuotesCreateSale.Enabled = !enable;
                BtnSalesQuotesCancel.Enabled = enable;
                BtnSalesQuotesSave.Enabled = enable;
            }
            else
            {
                BtnSalesQuotesNew.Enabled = !enable;
                BtnSalesQuotesDelete.Enabled = !enable;
                BtnSalesQuotesPrint.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? enable : !enable;
                BtnSalesQuotesCreateSale.Enabled = !enable;
                BtnSalesQuotesCancel.Enabled = !enable;
                BtnSalesQuotesSave.Enabled = !enable;
            }
            SaleEntry Entry = GetSavedSaleQuote();
            if (Entry != null)
            {
                if (Entry.isSaleLocked == true)
                {
                    BtnSalesQuotesDelete.Enabled = enable;
                    BtnSalesQuotesSave.Enabled = enable;
                    YesNoRbtSalesQuotesMethod.Enabled = enable;
                    DatetimePickerSalesQuotesDate.ReadOnly = !enable;
                    DatetimePickerSalesQuotesDate.TabStop = enable;
                    DatetimePickerSalesQuotesExp.ReadOnly = !enable;
                    DatetimePickerSalesQuotesExp.TabStop = enable;
                    TextBoxSaleQuoteCustomer.ReadOnly = !enable;
                    TextBoxSaleQuoteCustomer.TabStop = enable;
                    TextBoxSalesQuotesCustomerAddress.ReadOnly = !enable;
                    TextBoxSalesQuotesCustomerAddress.TabStop = enable;
                    BtnSalesQuotesNewCustomer.Enabled = enable;
                    BtnSalesQuotesSearchCustomer.Enabled = enable;
                    GridViewSalesQuotesItem.Enabled = enable;
                    DiscountAdditinalChargeGrid.Enabled = enable;
                }
            }
            if (Global.Company.AllowWholSale)
            {
                YesNoRadioSaleType.Enabled = true;
            }
            else
            {
                YesNoRadioSaleType.Enabled = false;
            }
        }
        private SaleEntry GetSavedSaleQuote()
        {
            SaleEntry SaleEntry = null!;
            if (!string.IsNullOrEmpty(TextBoxSalesQuotesId.Text))
            {
                SaleEntry = SalesManager.GetSaleEntry(long.Parse(TextBoxSalesQuotesId.Text));
            }
            return SaleEntry;
        }
        private void LoadSaleEntry(long SalesId)
        {
            // Changes made here CustomerId to AccountId and Customer to Account due to sales mdel change
            ResetForm();
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry != null)
            {
                SalesQuotesReferenceNumber.Text = SaleEntry.RefNumber;
                TextBoxSalesQuotesId.Text = SaleEntry.Id.ToString();
                if (SaleEntry.AccountsId == null)
                {
                    TextBoxSaleQuoteCustomer.Text = SaleEntry.CustomerName;
                    TextBoxSalesQuotesCustomerAddress.Text = SaleEntry.CustomerAddress.Replace(",", "," + System.Environment.NewLine); ;
                }
                else
                {
                    TextBoxSaleQuoteCustomer.Text = SaleEntry.Account.Name;
                    TextBoxSaleQuoteCustomer.Id = SaleEntry.Account.Id.ToString();
                    TextBoxSalesQuotesCustomerAddress.Text = SaleEntry.CustomerAddress;
                }
                YesNoRadioSaleType.Checked = SaleEntry.SaleType == SaleType.Retail ? true : false;
                DatetimePickerSalesQuotesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                DatetimePickerSalesQuotesExp.Date = (DateTime)DateUtils.ToDate(SaleEntry.QuotaionExpireAt.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtSalesQuotesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                TextBoxQuoteMemo.Text = SaleEntry.Memo;
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    GridViewSalesQuotesItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var SaleDetail in SaleEntry.SaleDetails)
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SNO].Value = i + 1;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                        (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(lSaleDetail.Product.RetailUOM);
                        (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(lSaleDetail.Product.WholesaleUOM);
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = lSaleDetail.Product.RetailUOM == lSaleDetail.Uom ? lSaleDetail.Product.RetailUOM : lSaleDetail.Product.WholesaleUOM;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value = lSaleDetail.Quantity;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value = lSaleDetail.FreeQuantity;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = lSaleDetail.Price;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value = lSaleDetail.OverridePrice;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value = ProductTaxPercentage(lSaleDetail.TaxDetails);
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value = (lSaleDetail.Discounts.Count > 0) ? lSaleDetail.Discounts.First().Discount : 0.00;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = lSaleDetail.Amount;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value = lSaleDetail.ProductId;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value = lSaleDetail.isBatch;
                        //for tax
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value = lSaleDetail.Id;
                        i++;
                    }
                    Row_Added();
                    ReSequence();
                    if (SaleEntry.SaleAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = SaleEntry.SaleAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                OverallTotal();
                if (SaleEntry.isSaleLocked)
                {
                    BtnSalesQuotesPrint.Select();
                }
                else
                {
                    DatetimePickerSalesQuotesDate.Focus();
                }
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                SearchSalesId = 0L;
                DisplaySystemError("The selected quation is not available anymore");
                return;
            }
        }
        private double ProductTaxPercentage(List<ItemLevelSaleTaxDetail> SaleTaxDetails)
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
            DateTime CurrentDate = DatetimePickerSalesQuotesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesQuotesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat) ? (DateTime)DatetimePickerSalesQuotesDate.Date : Global.getTransactionDate();
            foreach (CatalogItemSalesTaxMap Map in Product.SalesTax.ToList())
            {
                if (Map != null)
                {
                    if (Map.EffectiveFrom <= CurrentDate && Map.EffectiveTo >= CurrentDate)
                    {
                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)Map.SalesTaxMapId!);
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
            for (int i = 0; i < GridViewSalesQuotesItem.Rows.Count; i++)
            {
                GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.SNO].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewSalesQuotesItem.Rows.Count - 1; i++)
            {
                double Quantity = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value.ToString()!));
                double FreeQuantity = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value.ToString()!));
                double Pprice = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value.ToString()!));
                double Oprice = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value.ToString()!));

                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;
                if (Oprice > 0)
                {
                    Amount = (Oprice * Quantity);
                }
                else
                {
                    Amount = (Pprice * Quantity);
                }
                if (Amount > 0)
                {
                    double DiscountPercentage = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value.ToString()!));
                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;
                    double TaxPercentage = (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value.ToString()!));
                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAX].Value = TaxAmount;
                    Amount = Amount + TaxAmount;
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = Amount;
                    TotalAmount = TotalAmount + Amount;
                }
                else
                {
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = 0.00;
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.DIS].Value = 0.00;
                    GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAX].Value = 0.00;
                }
            }
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TotalAmount;
            DiscountAdditinalChargeGrid.Quantity = TotalQuantity;
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;
            OverallTotal();
        }
        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES_QUOTE)!.RoundOff;
        private void OverallTotal()
        {
            double RoundedTotal = DiscountAdditinalChargeGrid.OutputAmount;
            double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
            labelRoundOff.Text = "Round Off (" + (RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + ")";
            DiscountAdditinalChargeGrid.OutputAmount = RoundedTotal + RoundoffAmount;
            LabelQuotesSalesFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
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
            if (isValidSearchCriteria())
            {
                RecentSaless();
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
                                BtnSalesQuotesSave_Click(sender, e);
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
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBoxSaleQuoteSearch.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = SearchBoxEmptyErrorMsg;
                TextBoxSaleQuoteSearch.TextBox.Select();
                return false;
            }
            return true;
        }
        private void RecentSaless()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            String SearchText = TextBoxSaleQuoteSearch.Text.Trim();
            IList<SaleEntry> SalesInfo = null!;
            if (string.IsNullOrEmpty(SearchText))
            {
                SalesInfo = SalesManager.GetRecentSaleEntrys(Global.Company.CompanyId, Entrytype.QUOTE);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                SalesInfo = SalesManager.GetSaleEntryByAmount(Amount, SearchText, Global.Company.CompanyId, Entrytype.QUOTE);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat)!;
                SalesInfo = SalesManager.GetSaleEntryByDate((DateTime)Date, Global.Company.CompanyId, Entrytype.QUOTE);
            }
            else
            {
                SalesInfo = SalesManager.GetSaleEntryByCustomerName(SearchText, Global.Company.CompanyId, Entrytype.QUOTE);
            }
            if (SalesInfo.Count > 0)
            {
                LoadSales(SalesInfo);
            }
            else
            {
                SearchSalesId = 0L;
                ToolStripStatusLabelErrorPurchase.Text = SaleQuoteSearchOutput;
            }
        }
        public void LoadSales(IList<SaleEntry> SalesInfo)
        {
            if (SalesInfo.Count > 0)
            {
                SearchSalesId = 0L;
                FormRecentSales FormRecentSales = new FormRecentSales(this);
                FormRecentSales.Text = "Recent Quotes";
                FormRecentSales.SaleEntryInfo = SalesInfo;
                FormRecentSales.ShowDialog();
            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = SaleQuoteSearchOutput;
            }
        }
        public double Oprice = 0.00;
        public double Price = 0.00;
        private void GridViewSalesQuotesItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)SaleQuoteEntryTableColumn.QTY ||
               e.ColumnIndex == (int)SaleQuoteEntryTableColumn.FREE ||
               e.ColumnIndex == (int)SaleQuoteEntryTableColumn.PRICE ||
               e.ColumnIndex == (int)SaleQuoteEntryTableColumn.DISP ||
               e.ColumnIndex == (int)SaleQuoteEntryTableColumn.TAXP)
            {
                Row_Added();
            }
            if (e.ColumnIndex == (int)SaleQuoteEntryTableColumn.OPRICE)
            {
                Price = GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value != null ? double.Parse(GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value.ToString()!) : 0;
                Oprice = GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value != null ? double.Parse(GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value.ToString()!) : 0;
                if (Price != Oprice && Oprice > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (IsOverrideTabCtr)
                    {
                        SendKeys.Send("+{tab}+{tab}");
                    }
                    PriceOverride PriceOverride = new PriceOverride(this);
                    PriceOverride.ProductId = (long)GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.ID].Value;
                    PriceOverride.TextBoxOverridePrice.Text = Oprice.ToString();
                    PriceOverride.TextBoxPrice.Text = Price.ToString();
                    PriceOverride.ShowDialog();
                    GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value = Oprice;
                    GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = Price;
                    if (IsOverrideTabCtr)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    Cursor.Current = Cursors.Default;
                }
                Row_Added();
            }
        }
        private void BtnSalesQuotesNewCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long? TempCustomerId = TextBoxSaleQuoteCustomer.Id == null ? 0L : long.Parse(TextBoxSaleQuoteCustomer.Id);
            CustomerId = 0L;
            FormCustomers FormCustomer = new FormCustomers(this);
            FormCustomer.CreateCustomerOnLoad = true;
            FormCustomer.ShowDialog(this);
            if (CustomerId != 0)
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(CustomerId);
                if (customer != null)
                {
                    TextBoxSaleQuoteCustomer.Text = customer.Name;
                    TextBoxSaleQuoteCustomer.Id = CustomerId.ToString();
                    TextBoxSalesQuotesCustomerAddress.Text = customer.BillingAddress.FullAddress.Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    CustomerId = (long)TempCustomerId;
                    MessageBox.Show("The saved customer is not available anymore");
                    return;
                }
            }
            else
            {
                if (TempCustomerId != null)
                {
                    CustomerId = (long)TempCustomerId;
                    DirtyFlag(IsDirty);
                }
            }
            TextBoxSaleQuoteCustomer.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewSalesQuotesItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewSalesQuotesItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                DirtyFlag(IsDirty);
            }
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.SNO].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.UOM].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.QTY].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.FREE].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.PRICE].ReadOnly = false;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.TAXP].ReadOnly = false;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.TAX].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.DISP].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.DIS].ReadOnly = true;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
            {
                GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.UOM].ReadOnly = false;
                GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.QTY].ReadOnly = false;
                GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.FREE].ReadOnly = false;
                GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.DISP].ReadOnly = false;
                GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].ReadOnly = false;

            }
        }
        private void GridViewSalesQuotesItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)SaleQuoteEntryTableColumn.REMOVE && (GridViewSalesQuotesItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_DeleteConfirmText, GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.SNO].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSalesQuotesItem.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }
        private void GridViewSalesQuotesItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSalesQuotesItem_KeyPress1(object? sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewSalesQuotesItem.EditingControl).DroppedDown = false;
        }
        private void UomColumnComboTextChanged(object? sender, EventArgs e)
        {
            if (((ComboBox)sender!).SelectedIndex == -1 && !string.IsNullOrEmpty(((ComboBox)sender).Text))
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
                    int Index = GridViewSalesQuotesItem.CurrentCell.RowIndex;
                    GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }
        }
        private void UomColumnComboSelectionChanged(object? sender, EventArgs e)
        {
            int Index = GridViewSalesQuotesItem.CurrentCell.RowIndex;
            if (((ComboBox)sender!).SelectedIndex > -1)
            {
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value == null)
                {
                    GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = 0.00;
                }
            }
        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value == null)
                return;

            Product product = CatalogProductManager.Instance.GetProductInfoById(
                (long)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value);

            if (product == null) return;

            // Get price type from ComboBox
            PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;

            // Get tax (if any)
            double tax = product.UseHsnTax
                ? ProductTaxPercentage(product)
                : ProductTaxPercentage(product.SalesTax.ToList());

            bool isBatch = GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value != null &&
                           (bool)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value;

            double price = 0;

            if (isBatch)
            {
                if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value != null)
                {
                    InventoryBatch inventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(
                        (long)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value);

                    if (inventoryBatch != null)
                    {
                        price = GetPriceByType(selectedPriceType, inventoryBatch);
                        GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = inventoryBatch.MaxRetailPrice;
                    }
                }
                else
                {
                    price = GetPriceByType(selectedPriceType, product);
                }
            }
            else
            {
                price = GetPriceByType(selectedPriceType, product);
            }

            // Apply tax if needed
            if (Global.Company.CompanySalesSetup.IncludingTax)
            {
                price = price - ((price * (price * (tax / 100))) / (price + (price * (tax / 100))));
            }

            GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = price;

            // Optional: store product date/details if needed for quote
            // SaleProductDetails.CurrentDate = DateTimePickerSaleQuoteDate.Date;
            // SaleProductDetails.ProductId = product.Id;
        }

        private void LoadPricexxx(int Index, string Uom)
        {
            if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value);
                if (Product != null)
                {
                    PriceType selectedPriceType = (PriceType)ComboBoxInvoicePriceBy.SelectedIndex;
                    if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value != null && (bool)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value)
                    {
                        if (GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value);
                            if (InventoryBatch != null)
                            {
                                GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice;
                            }
                        }
                        else
                        {
                            GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                        }
                    }
                    else
                    {
                        GridViewSalesQuotesItem.Rows[Index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                    }
                }
            }
        }
        private void GridViewSalesQuotesItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewSalesQuotesItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged;
                ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged;
                e.Control.KeyPress += new KeyPressEventHandler(GridViewSalesQuotesItem_KeyPress1);
            }
            if (GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange;
                ((TextBox)e.Control).TextChanged += ProductTextChange;
            }
        }
        private void ProductTextChange(object? sender, EventArgs e)
        {
            if (((TextBox)sender!).Modified)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
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
                        long CheckForAddRow = (GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null) ? (long)GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value : 0L;
                        //for tax update Sales
                        if (GridViewSalesQuotesItem.Rows[GridViewSalesQuotesItem.CurrentRow.Index].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value != null &&
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
                        GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (CheckForAddRow == 0 && GridViewSalesQuotesItem.Rows.Count - 1 == GridViewSalesQuotesItem.CurrentRow.Index)
                        {
                            GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewSalesQuotesItem.Rows.Add();
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewSalesQuotesItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewSalesQuotesItem.CurrentRow.Index);
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value = null;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = null;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value = 0;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value = 0;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = 0.00;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value = 0.00;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value = 0.00;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value = 0.00;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = 0.00;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value = null;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value = false;
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value = null;
            TextBoxSalesProductName.ResetText();
            //for tax update Sales
            GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value = null;
            ComputeFormTotal();
        }
        private void LoadUomTax(long ProductId)
        {
            Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
            if (Product != null)
            {
                int index = GridViewSalesQuotesItem.CurrentRow.Index;
                (GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(Product.RetailUOM);
                (GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.UOM] as DataGridViewComboBoxCell)!.Items.Add(Product.WholesaleUOM);
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = Global.Company.BusinessType == BuisnessType.Wholesale ? Product.WholesaleUOM : Product.RetailUOM;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value = Product.Name;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value = 0;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value = 0;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value = null;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = YesNoRadioSaleType.Checked ? Product.RetailPrice : Product.WholdSalePrice;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value = 0.00;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.TAX].Value = 0.00;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value = ProductTaxPercentage(Product);
                //for tax update Sales
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.SALESDETAILID].Value = null;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.DIS].Value = 0.00;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = 0.00;
                GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null)
                {
                    GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewSalesQuotesItem.Rows[index].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value = false;
                }
                TextBoxSalesProductName.Text = Product.Name;
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
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null) ? (long)GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value : 0L;
            ProductId = 0L;
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchItems.SearchText = (GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value != null) ? GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.PRODUCT].Value.ToString() : null;
            FormSearchItems.ShowDialog();
            if (ProductId != 0)
            {
                if (GridViewSalesQuotesItem.Rows[GridViewSalesQuotesItem.CurrentRow.Index].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null && Check == ProductId)
                {
                    DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        return;
                    }
                }
                LoadUomTax(ProductId);
                if (Check == 0 && GridViewSalesQuotesItem.Rows.Count - 1 == GridViewSalesQuotesItem.CurrentRow.Index)
                {
                    GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewSalesQuotesItem.Rows.Add();
                }
                GridViewSalesQuotesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[3, GridViewSalesQuotesItem.CurrentRow.Index];
                GridViewSalesQuotesItem.CurrentCell.Selected = true;
            }
            else
            {
                ProductId = Check;
                DirtyFlag(IsDirty);
                return;
            }
        }
        protected override void ProductBatchIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductBatchIdTransport.Text))
            {
                BatchId = long.Parse(ProductBatchIdTransport.Text);
            }
        }
        private void GridViewSalesQuotesItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.SNO].Value = GridViewSalesQuotesItem.Rows.Count;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value = false;
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.FREE].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.OPRICE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.TAX].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.DIS].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewSalesQuotesItem.Rows[e.RowIndex].Cells[(int)SaleQuoteEntryTableColumn.AMOUNT].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DirtyFlag(IsDirty);
        }
        private void FormQuote_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    YesNoRbtSalesQuotesMethod.Focus();
                    e.Cancel = true;
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
        private void BtnSalesQuotesSearchCustomer_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            long? TempCustomerId = TextBoxSaleQuoteCustomer.Id == null ? 0L : long.Parse(TextBoxSaleQuoteCustomer.Id);
            CustomerId = 0L;
            FormAccountSearch FormSearchAccount = new FormAccountSearch(this);
            FormSearchAccount.IncludeCustomers = true;
            FormSearchAccount.IncludeSuppliers = false;
            FormSearchAccount.IncludeEmployees = false;
            FormSearchAccount.IncludeGeneralAccounts = false;
            FormSearchAccount.ShowDialog();
            if (CustomerId != 0)
            {
                Customer customer = CustomerManager.Instance.GetCustomerById(CustomerId);
                if (customer != null)
                {
                    TextBoxSaleQuoteCustomer.Text = customer.Name;
                    TextBoxSaleQuoteCustomer.Id = CustomerId.ToString();
                    TextBoxSalesQuotesCustomerAddress.Text = customer.BillingAddress.FullAddress.Replace("\n", System.Environment.NewLine);
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
            TextBoxSaleQuoteCustomer.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GridViewSalesQuotesItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                // LoadProductDetails(e.RowIndex);
            }
        }
        private void GridViewSalesQuotesItem_Leave(object sender, EventArgs e)
        {
            GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.UOM, GridViewSalesQuotesItem.CurrentRow.Index];
        }
        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            OverallTotal();
        }
        bool IsOverrideTabCtr = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2) && (GridViewSalesQuotesItem.CurrentCell.ColumnIndex != (int)SaleQuoteEntryTableColumn.PRODUCT) && ((GridViewSalesQuotesItem.CurrentCell.ReadOnly) ? true : /*GridViewSalesQuotesItem.CurrentCell.ColumnIndex != (int)SaleQuoteEntryTableColumn.BATNO*/false))
            {
                BtnSalesQuotesSearchCustomer_Click(this, null!);
                return true;
            }
            if (keyData == (Keys.F2) && (GridViewSalesQuotesItem.Focused && (GridViewSalesQuotesItem.CurrentCell.ColumnIndex != (int)SaleQuoteEntryTableColumn.PRODUCT) && ((GridViewSalesQuotesItem.CurrentCell.ReadOnly) ? true : /*GridViewSalesQuotesItem.CurrentCell.ColumnIndex != (int)SaleQuoteEntryTableColumn.BATNO*/false)))
            {
                BtnSalesQuotesSearchCustomer_Click(this, null!);
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnSalesQuotesNew.Enabled)
                {
                    BtnSalesQuotesNew_Click(this, null!);
                    return true;
                }
                else
                {
                    BtnSalesQuotesNewCustomer_Click(this, null!);
                    return true;
                }
            }
            else if (keyData == (Keys.F4))
            {
                if (BtnSalesQuotesDelete.Enabled)
                {
                    BtnSalesQuotesDelete_Click(this, null!);
                    return true;
                }
            }
            else if (keyData == (Keys.F9))
            {
                if (BtnSalesQuotesPrint.Enabled)
                {
                    BtnSalesQuotesPrint_Click(this, null!);
                    return true;
                }
            }
            else if (keyData == (Keys.F8))
            {
                if (BtnSalesQuotesSave.Enabled)
                {
                    BtnSalesQuotesSave_Click(this, null!);
                    return true;
                }
            }
            else if (keyData == (Keys.Escape))
            {
                if (BtnSalesQuotesCancel.Enabled)
                {
                    BtnSalesQuotesCancel_Click(this, null!);
                    return true;
                }
            }
            else if (keyData == (Keys.F10))
            {
                if (BtnSalesQuotesExit.Enabled)
                {
                    BtnSalesQuotesExit_Click(this, null!);
                    return true;
                }
            }
            try
            {
                if ((keyData == Keys.F2) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.PRODUCT)
                {
                    SearchProduct();
                    return true;
                }
                if (keyData == (Keys.Tab) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.TAXP)
                {
                    SendKeys.Send("{tab}");

                }
                //if (keyData == (Keys.Tab) && (GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.OPRICE))
                //{
                //    IsOverrideTabCtr = true;
                //    SendKeys.Send("{tab}{tab}");
                //}
                //if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.OPRICE))
                //{
                //    IsOverrideTabCtr = false;
                //    //SendKeys.Send("{tab}");
                //}
                if (keyData == (Keys.Tab) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.DISP)
                {
                    if (GridViewSalesQuotesItem.CurrentCell.RowIndex != GridViewSalesQuotesItem.Rows.Count - 1)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.PRICE)
                {
                    //SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.DISP))
                {
                    SendKeys.Send("{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.OPRICE)
                {
                    ///SendKeys.Send("{tab}{tab}");
                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewSalesQuotesItem.CurrentCell.ColumnIndex == (int)SaleQuoteEntryTableColumn.PRODUCT)
                {
                    if (GridViewSalesQuotesItem.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    else
                    {
                        TextBoxQuoteMemo.Focus();
                    }
                }
            }
#pragma warning disable 0168 // variable declared but not used.
            catch (Exception ex)
            {
            }
#pragma warning restore 0168
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnSalesQuotesSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSaleQuoteCustomer.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiscountAdditinalChargeGrid.Focus();
                return;
            }
        }
        private void TextBoxSaleQuoteSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSalesQuotesSearch_Click(sender, e);
            }
        }
        private void YesNoRbtSalesQuotesMethod_Load(object sender, EventArgs e)
        {
            if (YesNoRbtSalesQuotesMethod.Checked)
            {
                TextBoxSaleQuoteCustomer.ReadOnly = true;
                TextBoxSaleQuoteCustomer.TabStop = false;
                if (TextBoxSaleQuoteCustomer.Id == null)
                {
                    TextBoxSaleQuoteCustomer.ResetText();
                }
            }
            else
            {
                TextBoxSaleQuoteCustomer.ReadOnly = false;
                TextBoxSaleQuoteCustomer.TabStop = true;
            }
        }
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
            DiscountAdditinalChargeGrid.IsDirty = Enable;
        }
        private void DiscountAdditinalChargeGrid_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.formIsDirty)
            {
                this.formIsDirty = DiscountAdditinalChargeGrid.IsDirty;
            }
        }
        private void DatetimePickerSalesQuotesDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerSalesQuotesExp.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSalesQuotesCustomerAddress.Select();
            }
        }
        private void TextBoxSalesQuotesCustomerAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerSalesQuotesDate.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSalesQuotesSearchCustomer.Select();
            }
        }
        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSalesQuotesSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewSalesQuotesItem.Select();
                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.PRODUCT, 0];
            }
        }
        private void TextBoxSalesQuotesCustomerAddress_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxSalesQuotesCustomerAddress.Text.Contains("\t"))
            {
                TextBoxSalesQuotesCustomerAddress.Text = TextBoxSalesQuotesCustomerAddress.Text.Trim();
            }
        }
        private void BtnSalesQuotesCreateSale_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormItembasedSales FormItembasedSales = new FormItembasedSales();
            FormItembasedSales.ItemBasedsaleOnLoad = true;
            FormItembasedSales.SearchSalesId = long.Parse(TextBoxSalesQuotesId.Text);
            FormItembasedSales.ShowDialog();
            LoadSaleEntry(long.Parse(TextBoxSalesQuotesId.Text));
            Cursor.Current = Cursors.Default;
        }
        private void TextBoxSalesQuotesCustomerAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }
        public override void InputControls_OnChange(object sender, EventArgs e)
        {
            this.formIsDirty = true;
            BtnSalesQuotesCreateSale.Enabled = false;
            BtnSalesQuotesPrint.Enabled = false;
        }
        private void YesNoRadioSaleType_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < GridViewSalesQuotesItem.Rows.Count - 1; i++)
            {
                if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
                {
                    Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value);
                    if ((bool)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ISBAT].Value)
                    {
                        if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.BATCHID].Value);
                            if (InventoryBatch != null)
                            {
                                GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = YesNoRadioSaleType.Checked ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice;
                                GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = YesNoRadioSaleType.Checked ? Product.RetailUOM : Product.WholesaleUOM;
                            }
                        }
                        else
                        {
                            GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = YesNoRadioSaleType.Checked ? Product.RetailPrice : Product.WholdSalePrice;
                            GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = YesNoRadioSaleType.Checked ? Product.RetailUOM : Product.WholesaleUOM;
                        }
                    }
                    else
                    {
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.PRICE].Value = YesNoRadioSaleType.Checked ? Product.RetailPrice : Product.WholdSalePrice;
                        GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.UOM].Value = YesNoRadioSaleType.Checked ? Product.RetailUOM : Product.WholesaleUOM;
                    }
                }
            }
        }
        private void TextBoxSaleQuoteSearch_Leave(object sender, EventArgs e)
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
        }

        private void TextBoxSaleQuoteCustomer_ModifiedChanged(object sender, EventArgs e)
        {
            if (CustomerId != null)
            {
                if (GridViewSalesQuotesItem.Rows.Count > 1)
                {
                    for (int i = 0; i <= GridViewSalesQuotesItem.Rows.Count - 2; i++)
                    {
                        if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
                        {
                            long ProductId = (long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value;
                            Product lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                            if (lProduct != null)
                            {
                                GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value = ProductTaxPercentage(lProduct);
                            }
                        }
                    }
                    ComputeFormTotal();
                }
            }
        }

        private void DatetimePickerSalesQuotesDate_Leave(object sender, EventArgs e)
        {
            if (DatetimePickerSalesQuotesDate.Date != null && DateUtils.ValidDate(((DateTime)DatetimePickerSalesQuotesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                if (GridViewSalesQuotesItem.Rows.Count > 1)
                {
                    for (int i = 0; i <= GridViewSalesQuotesItem.Rows.Count - 2; i++)
                    {
                        if (GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
                        {
                            long ProductId = (long)GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.ID].Value;
                            Product lProduct = CatalogProductManager.Instance.GetProductInfoById(ProductId);
                            if (lProduct != null)
                            {
                                GridViewSalesQuotesItem.Rows[i].Cells[(int)SaleQuoteEntryTableColumn.TAXP].Value = ProductTaxPercentage(lProduct);
                            }
                        }
                    }
                    ComputeFormTotal();
                }
            }
        }

        private void TextBoxQuoteMemo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewSalesQuotesItem.Select();
                GridViewSalesQuotesItem.CurrentCell = GridViewSalesQuotesItem[(int)SaleQuoteEntryTableColumn.PRODUCT, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                YesNoRbtSalesQuotesMethod.Focus();
            }
        }

        private void TextBoxQuoteMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void TextBoxSaleQuoteCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSalesQuotesSearchCustomer.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSalesQuotesSave.Select();
            }
        }

        private void BtnSalesQuotesSearchCustomer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSalesQuotesCustomerAddress.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSaleQuoteCustomer.Select();
            }
        }

        private void YesNoRbtSalesQuotesMethod_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxQuoteMemo.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerSalesQuotesExp.Focus();
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
            if (GridViewSalesQuotesItem.CurrentRow != null &&
                GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value != null)
            {
                long productId = (long)GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.ID].Value;
                string uom = GridViewSalesQuotesItem.CurrentRow.Cells[(int)SaleQuoteEntryTableColumn.UOM].Value?.ToString()!;

                if (!string.IsNullOrEmpty(uom))
                {
                    LoadPrice(GridViewSalesQuotesItem.CurrentRow.Index, uom);
                    ComputeFormTotal();
                }
            }
        }

        private void UpdateAllRowsPrices()
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow row in GridViewSalesQuotesItem.Rows)
            {
                // Skip the empty row at the end if present
                if (row.IsNewRow || row.Cells[(int)SaleQuoteEntryTableColumn.ID].Value == null)
                    continue;

                long productId = (long)row.Cells[(int)SaleQuoteEntryTableColumn.ID].Value;
                string uom = row.Cells[(int)SaleQuoteEntryTableColumn.UOM].Value?.ToString()!;

                if (!string.IsNullOrEmpty(uom))
                {
                    // Temporarily set current row to update prices correctly
                    GridViewSalesQuotesItem.CurrentCell = row.Cells[0];

                    // Load price for this row with the new price type
                    LoadPrice(row.Index, uom);
                }
            }

            ComputeFormTotal();
            Cursor.Current = Cursors.Default;
        }
        private void YesNoRadioPriceTo_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle between Retail/Wholesale pricing logic
            if (YesNoRadioPriceTo.Checked)
            {
                ToolStripStatusLabelErrorPurchase.Text = "Price To: Retail";
            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = "Price To: Wholesale";
            }
        }
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
        private double ProductTaxPercentage(List<CatalogItemSalesTaxMap> salesTaxList)
        {
            return salesTaxList.Sum(t => t.TaxPercentage) / salesTaxList.Count;
        }

    }
}
