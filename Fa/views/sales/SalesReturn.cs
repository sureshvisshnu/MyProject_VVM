using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.controls.accounting;
using fa.views.controls.grid;
using fa.views.utils;
using Fa.model.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static fa.views.purchase.FormPurchaseReturn;

namespace fa.views.sales
{
    public partial class FormSalesReturn : FormBase
    {
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteErrorText = "Error in Sale return Deleting. !";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string DeleteConfirmText = "Do you want to delete Sale return {0}?";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string EnterSaleDateErrorMsg = "Please enter sale date.";
        public static string EnterReturnDateErrorMsg = "Please enter Return date.";
        public static string EnterQuantityErrorMsg = "Please Enter valid Quantity, Available Quantity for Return is {0}";
        public static string EnterFreeErrorMsg = "Please Enter valid Free Quantity, Available Free Quantity for Return is {0}";
        public static string SelectInventoryLoactionErrorMsg = "Please select inventory location.";
        public static string EnterRemainingQuantityErrorMsg = "Please Enter valid Quantity, Remaining Quantity for Return is {0}";


        public static string Grid_DeleteConfirmText = "Do you want to delete Row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter sale return Items details.";

        public static string InvalidSaleReturnErrorMsg = "Invalid sale return.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could customer name or sale date or sale reference number.";
        public static string SaleSearchOutput = "No sale found.";
        public static string SaleReturnSearchOutput = "No sale return found";
        public enum SaleReturnItemTableColumn
        {
            SNO, PRODUCT, UOM, QTY, FREE, BATNO, EXPDATE, PRICE, RETURNFEEP, TAXP, TAX, DISP, DIS, AMOUNT, REMOVE, ID, ISBAT, SALESDETAILID, BATCHID, SALESRETURNDETAILID
        }
        public enum SaleReturnTableColumn
        {
            SNO, ITEM_NAME, QTY, RETURN, REMAINING, CHOOSE, DETAIL_ID, FREERETURN, FREE
        }
        SalesManager SalesManager = null;
        public bool SaleReturnOnLoad = false;
        public long Detail_Id = 0L;
        public long SearchSalesId = 0L;
        public long SearchSaleReturnId = 0L;
        public FormSalesReturn()
        {
            InitializeComponent();
            SalesManager = SalesManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "TextBoxSaleReturnSearch", "GridViewSaleReturnItem", "DiscountAdditinalChargeGrid" };
            //WorkStationSetup();
        }

        private void FormSalesReturn_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Visible = false;
            setSize();
            this.Visible = true;
            ResetForm();
            EnableForm(true);
            TextBoxSaleSearch.Select();
            if (SaleReturnOnLoad)
            {
                CheckSaleReturn();
                GridViewSaleReturnItem.Select();
            }
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void WorkStationSetup()
        {
            if (!WorkStationValidation.Instance.InitializeWorkstationValidation())
            {
                this.Close();
            }

        }
        private void CheckSaleReturn()
        {
            EnableForm(true);
            LoadSaleEntry(SearchSalesId);
            DirtyFlag(false);
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
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
            DiscountAdditinalChargeGrid.IsDirty = Enable;
        }
        private void LoadSaleReturnEntry(long SaleReturnId)
        {
            // Changes made here CustomerId to AccountId and Customer to Account due to sales mdel change
            ResetForm();
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SaleReturnId);
            if (SaleEntry != null)
            {
                TextBoxSaleReturnId.Text = SaleReturnId.ToString();
                if (SaleEntry.AccountsId == null)
                {
                    TextBoxSalesCustomerDetails.Text = SaleEntry.CustomerName + "\n" + SaleEntry.CustomerAddress.Replace(",", "," + System.Environment.NewLine); ;
                }
                else
                {
                    TextBoxSalesCustomerDetails.Text = SaleEntry.Account.Name;
                    TextBoxSalesCustomerDetails.Id = SaleEntry.Account.Id.ToString();
                    Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                    if (Customer != null && Customer.BillingAddress != null)
                    {
                        TextBoxSalesCustomerDetails.Text = (TextBoxSalesCustomerDetails.Text + "," + Customer.BillingAddress.FullAddress).Replace(",", "," + System.Environment.NewLine);
                    }
                }
                if (SaleEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)SaleEntry.InventoryLocationId);
                    if (Location != null)
                    {
                        ComboBoxStockLocation.SelectedIndex = ComboBoxStockLocation.FindStringExact(Location.Name);
                    }
                }
                TextBoxSalesMemo.Text = !string.IsNullOrEmpty(SaleEntry.Memo) ? SaleEntry.Memo.Replace(",", System.Environment.NewLine) : string.Empty;
                DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                YesNoRbtSalesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                LabelSaleReturnReferenceNumber.Text = SaleEntry.RefNumber;
                DatetimePickerSalesReturnDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.ReturnDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    GridViewSalesItem.Rows.Clear();
                    GridViewSalesItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    foreach (var SaleDetail in SaleEntry.SaleDetails)
                    {
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SNO].Value = i + 1;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.UOM].Value = lSaleDetail.Uom;
                        if (lSaleDetail.isBatch)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATNO].Value = lSaleDetail.BatchNo;
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.EXPDATE].Value = lSaleDetail.ExpDate;
                        }
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value = lSaleDetail.Quantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value = lSaleDetail.FreeQuantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value = lSaleDetail.Price;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value = lSaleDetail.OverridePrice;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value = (lSaleDetail.Discounts.Count > 0) ? lSaleDetail.Discounts.First().Discount : 0.00;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value = lSaleDetail.Amount;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value = lSaleDetail.ProductId;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ISBAT].Value = lSaleDetail.isBatch;
                        //for tax

                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESDETAILID].Value = lSaleDetail.Id;
                        if (lSaleDetail.isBatch && lSaleDetail.BatchNo != null)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lSaleDetail.ProductId, lSaleDetail.BatchNo, (long)SaleEntry.InventoryLocationId).Id;
                        }


                        i++;
                    }
                }
                if (SaleEntry.SaleAdditionalTransactions.Count > 0)
                {
                    DiscountAdditinalChargeGrid.AdditionalTransactions = SaleEntry.SaleAdditionalTransactions.ToList<AdditionalTransaction>();
                }
                Row_Added();
                BtnSaleReturnDelete.Select();
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("The selected sale return is not available anymore");
            }
        }
        private void LoadSaleEntry(long SalesId)
        {
            // Changes made here CustomerId to AccountId and Customer to Account due to sales mdel change
            ResetForm();
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry != null)
            {
                SearchSalesId = SaleEntry.Id;
                if (SaleEntry.AccountsId == null)
                {
                    TextBoxSalesCustomerDetails.Text = SaleEntry.CustomerName + "\n" + SaleEntry.CustomerAddress.Replace(",", "," + System.Environment.NewLine);
                }
                else
                {
                    TextBoxSalesCustomerDetails.Text = SaleEntry.Account.Name;
                    TextBoxSalesCustomerDetails.Id = SaleEntry.Account.Id.ToString();
                    Customer Customer = CustomerManager.Instance.GetCustomerById((long)SaleEntry.AccountsId);
                    if (Customer != null)
                    {
                        if (Customer.BillingAddress != null)
                        {
                            TextBoxSalesCustomerDetails.Text = (TextBoxSalesCustomerDetails.Text + "," + Customer.BillingAddress.FullAddress).Replace(",", "," + System.Environment.NewLine);
                        }
                    }
                    else
                    {
                        Supplier Supplier = SupplierManager.Instance.GetSupplierById((long)SaleEntry.AccountsId);
                        if (Supplier != null)
                        {
                            if (Supplier.Address != null)
                            {
                                TextBoxSalesCustomerDetails.Text = (TextBoxSalesCustomerDetails.Text + "," + Supplier.Address.FullAddress).Replace(",", "," + System.Environment.NewLine);
                            }
                        }
                    }
                }
                if (SaleEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)SaleEntry.InventoryLocationId);
                    if (Location != null)
                    {
                        ComboBoxStockLocation.SelectedIndex = ComboBoxStockLocation.FindStringExact(Location.Name);
                    }
                }
                TextBoxSalesMemo.Text = !string.IsNullOrEmpty(SaleEntry.Memo) ? SaleEntry.Memo.Replace(",", System.Environment.NewLine) : string.Empty;
                DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                YesNoRbtSalesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    //calculate old return qty
                    IList<SaleEntry> SaleReturns = SalesManager.ListSaleReturnsBysaleId(SalesId);
                    GridViewSaleReturnItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    foreach (var SaleDetail in SaleEntry.SaleDetails)
                    {
                        //calculate old return qty
                        double Qty = 0;
                        double Free = 0;
                        if (SaleReturns.Count > 0)
                        {
                            foreach (SaleEntry SaleReturn in SaleReturns)
                            {
                                foreach (SaleDetail SaleDetailReturn in SaleReturn.SaleDetails)
                                {
                                    if (SaleDetail.Id.ToString() == SaleDetailReturn.SaleDetailId.ToString() && SaleDetailReturn.ProductId == SaleDetail.ProductId && SaleDetailReturn.BatchNo == SaleDetail.BatchNo)
                                    {
                                        Qty = Qty + SaleDetailReturn.Quantity;
                                        Free = Free + SaleDetailReturn.FreeQuantity;
                                    }
                                }
                            }
                        }
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.SNO].Value = i + 1;
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.ITEM_NAME].Value = lSaleDetail.Product.Name;
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.QTY].Value = (lSaleDetail.Quantity + lSaleDetail.FreeQuantity);
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.REMAINING].Value = ((lSaleDetail.Quantity + lSaleDetail.FreeQuantity) - (Qty + Free));
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.RETURN].Value = (Qty + Free);
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.FREERETURN].Value = Free;
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.FREE].Value = lSaleDetail.FreeQuantity;
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.CHOOSE].Value = false;
                        GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value = lSaleDetail.Id;
                        i++;
                    }
                    ReSequence(GridViewSaleReturnItem);
                    if (SaleEntry.SaleAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = SaleEntry.SaleAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                OverallTotal();
                GridViewSaleReturnItem.Focus();
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("The selected sale is not available anymore");
            }
        }
        private double ProductTaxPercentage(IList<ItemLevelSaleTaxDetail> SaleTaxDetails, SaleEntry SaleEntry)
        {
            double TaxTotal = 0.00;
            foreach (ItemLevelSaleTaxDetail Tax in SaleTaxDetails)
            {
                TaxTotal = TaxTotal + Tax.TaxRate;
            }
            return TaxTotal;
        }

        private void ReSequence(DataGridView DataGridView)
        {
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                DataGridView.Rows[i].Cells[(int)SaleReturnItemTableColumn.SNO].Value = i + 1;
            }
        }
        private void OverallTotal()
        {
            double RoundedTotal = DiscountAdditinalChargeGrid.OutputAmount;
            double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
            labelRoundOff.Text = "Round Off (" + (RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + ")";
            DiscountAdditinalChargeGrid.OutputAmount = RoundedTotal + RoundoffAmount;
            LabelSalesFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void Row_Removed()
        {
            ReSequence(GridViewSalesItem);
            ComputeFormTotal();
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerSales.Stop();
            TimerSales.Start();
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewSalesItem.Rows.Count; i++)
            {
                double Quantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value.ToString()));
                double FreeQuantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value.ToString()));
                double Pprice = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value.ToString()));
                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;
                Amount = (Pprice * Quantity);
                if (Amount > 0)
                {
                    double ReturnFeePercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value.ToString()));
                    double ReturnFee = Amount * (ReturnFeePercentage / 100);
                    double DiscountPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value.ToString()));
                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;
                    double TaxPercentage = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value.ToString()));
                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAX].Value = TaxAmount;
                    Amount = Amount + TaxAmount + ReturnFee;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value = Amount;
                    TotalAmount = TotalAmount + Amount;
                }
                else
                {
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DIS].Value = 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAX].Value = 0.00;
                }
            }
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TotalAmount;
            DiscountAdditinalChargeGrid.Quantity = TotalQuantity;
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;
            OverallTotal();
        }
        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.SALES_RETURN).RoundOff;
        private void ResetForm()
        {
            ComboUtils.InitializeStockLocationCombo(ComboBoxStockLocation, Global.Company.CompanyId);
            ComboBoxStockLocation.SelectedIndex = -1;
            ComboBoxStockLocation.ResetText();
            TextBoxSalesMemo.ResetText();
            LabelSaleReturnReferenceNumber.Text = "000000";
            TextBoxSaleSearch.ResetText();
            TextBoxSearchReturn.TextBox.ResetText();
            ErrorMsgSaleReturn.Text = "";
            TextBoxSaleReturnId.ResetText();
            DatetimePickerSalesDate.Format = Global.Company.DateFormat;
            DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            DatetimePickerSalesReturnDate.Format = Global.Company.DateFormat;
            DatetimePickerSalesReturnDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            GridViewSaleReturnItem.Rows.Clear();
            LabePrevlSaleReturnReferenceNumber.Text = CompanyManager.Instance.GetSalePrevRef(Global.Company, Entrytype.RETURN, (DateTime)DatetimePickerSalesDate.Date);
            YesNoRbtSalesMethod.Checked = true;
            GridViewSalesItem.Rows.Clear();
            DiscountAdditinalChargeGrid.GridType = GridType.Sales;
            DiscountAdditinalChargeGrid.Clear();
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LabelSalesFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxSalesCustomerDetails.ResetText();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["ReturnPrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["ReturnTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["ReturnDic"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewSalesItem.Columns["ReturnAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewPurchaseItemTotal.Columns["Value"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxStockLocation.Visible = false;
            GridViewSalesItem.ScrollBars = ScrollBars.Both;
            YesNoRbtSalesMethod.Enabled = false;
            GridViewSalesItem.ReadOnly = !enable;
            GridViewSalesItem.TabStop = enable;
            GridViewSaleReturnItem.Enabled = enable;
            if (Global.isDateModification && enable)
            {
                DatetimePickerSalesReturnDate.Enabled = true;
            }
            else
            {
                DatetimePickerSalesReturnDate.Enabled = false;
            }
            DiscountAdditinalChargeGrid.Enabled = enable;
            TextBoxSaleSearch.ReadOnly = !enable;
            TextBoxSaleSearch.TabStop = enable;
            if (enable)
            {
                BtnSaleSearch.Enabled = enable;
                GridViewSaleReturnItem.ReadOnly = enable;
                GridViewSaleReturnItem.TabStop = !enable;
                BtnSalesReturnEdit.Enabled = !enable;
                BtnSaleReturnNew.Enabled = enable;
                BtnSaleReturnDelete.Enabled = !enable;
                BtnSaleReturnPrint.Enabled = !enable;
                BtnSaleReturnCancel.Enabled = enable;
                BtnSaleReturnSave.Enabled = enable;
                BtnReceivePayment.Enabled = !enable;
                if (!string.IsNullOrEmpty(TextBoxSaleReturnId.Text))
                {
                    BtnSaleReturnDelete.Enabled = enable;
                    BtnSaleReturnPrint.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? !enable : enable;
                    BtnReceivePayment.Enabled = enable;

                }
            }
            else
            {
                BtnSalesReturnEdit.Enabled = !enable;
                BtnSaleSearch.Enabled = enable;
                BtnSaleReturnNew.Enabled = !enable;
                BtnSaleReturnDelete.Enabled = !enable;
                BtnSaleReturnPrint.Enabled = string.IsNullOrEmpty(Global.getDefaultPrinter()) ? enable : !enable;
                BtnSaleReturnCancel.Enabled = enable;
                BtnSaleReturnSave.Enabled = enable;
                BtnReceivePayment.Enabled = !enable;
            }
        }
        private void BtnSaleSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RecentSaless(Entrytype.SALE);
            if (SearchSalesId != 0)
            {
                CheckSaleReturn();
            }
            else
            {
                SearchSalesId = 0L;
                DisplaySystemError("The selected sale is not available anymore");
                return;
            }
            Cursor.Current = Cursors.Default;
        }
        private void RecentSaless(Entrytype Type)
        {
            ErrorMsgSaleReturn.Text = "";
            String SearchText = TextBoxSaleSearch.Text.Trim();
            IList<SaleEntry> SalesInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                SalesInfo = SalesManager.GetRecentSaleEntrys(Global.Company.CompanyId, Type);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
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
            if (SalesInfo.Count > 0)
            {
                LoadSales(SalesInfo);
            }
            else
            {
                SearchSalesId = 0L;
                ErrorMsgSaleReturn.Text = SaleSearchOutput;

            }
        }
        public void LoadSales(IList<SaleEntry> SalesInfo)
        {
            if (SalesInfo.Count > 0)
            {
                SearchSalesId = 0L;
                FormRecentSales FormRecentSales = new FormRecentSales(this);
                FormRecentSales.RecentEntrytype = Entrytype.SALE;
                FormRecentSales.SaleEntryInfo = SalesInfo;
                FormRecentSales.ShowDialog();
            }
            else
            {
                ErrorMsgSaleReturn.Text = SaleSearchOutput;
            }
        }
        private SaleEntry GetSaleEntryFromForm()
        {
            SaleEntry lSaleEntry = new SaleEntry();
            lSaleEntry.WorkStationId = Global.getWorkStationId();
            lSaleEntry.WorkStationName = Global.getWorkStationName();
            lSaleEntry.Id = TextBoxSaleReturnId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxSaleReturnId.Text);
            lSaleEntry.EntryType = Entrytype.RETURN;
            SaleEntry Sale = SalesManager.GetSaleEntry(SearchSalesId);
            lSaleEntry.SaleEntryId = Sale.Id;
            lSaleEntry.PatientId = Sale.PatientId;
            lSaleEntry.RegistrationId = Sale.RegistrationId;
            lSaleEntry.RefNumber = LabelSaleReturnReferenceNumber.Text;
            lSaleEntry.CustomerName = Sale.CustomerName;
            lSaleEntry.Memo = Sale.Memo;
            lSaleEntry.AccountsId = Sale.AccountsId;
            lSaleEntry.CustomerAddress = Sale.CustomerAddress;
            lSaleEntry.SaleTaxType = Sale.SaleTaxType;
            lSaleEntry.SaleMethod = (YesNoRbtSalesMethod.Checked) ? SaleMethod.Credit : SaleMethod.Cash;
            lSaleEntry.SaleDate = (DateTime)DatetimePickerSalesDate.Date;
            lSaleEntry.ReturnDate = (DateTime)DatetimePickerSalesReturnDate.Date;
            lSaleEntry.CompanyId = Global.Company.CompanyId;
            //for sql
            lSaleEntry.QuotaionExpireAt = DateTime.Now.Date;
            if (Global.CostCenter != null)
            {
                lSaleEntry.CostCenterId = Global.CostCenter.CostCenterId;
            }
            if (ComboBoxStockLocation.SelectedIndex > -1)
            {
                lSaleEntry.InventoryLocationId = ((InventoryLocation)ComboBoxStockLocation.Items[ComboBoxStockLocation.SelectedIndex]).Id;
            }
            float TotalAmount = float.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)SaleEntryTotalTableColumn.VALUE].Value.ToString());
            float TotalTaxAmount = 0;
            float TotalDiscountPercentage = 0;
            float TotalDiscountAmount = 0;
            float[] TaxWisePercentageTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            for (int i = 0; i < GridViewSalesItem.Rows.Count; i++)
            {
                SaleDetail SaleDetail = new SaleDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value);
                if (Product != null)
                {
                    SaleDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        SaleDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    SaleDetail.Uom = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.UOM].Value.ToString();
                    SaleDetail.Id = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESDETAILID].Value == null) ? 0L : long.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESDETAILID].Value.ToString());
                    SaleDetail.SaleDetailId = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value == null) ? 0L : long.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value.ToString());
                    SaleDetail.ProductId = Product.Id;
                    SaleDetail.MaterialId = Product.MaterialId;
                    SaleDetail.isBatch = (bool)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ISBAT].Value;
                    SaleDetail.ExpDate = DateTime.Now.Date;
                    if (SaleDetail.isBatch)
                    {
                        SaleDetail.BatchNo = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATNO].Value.ToString();
                        SaleDetail.ExpDate = (DateTime)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.EXPDATE].Value;
                    }
                    double Quantity = 0;
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY] != null &&
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value != null &&
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value.ToString()))
                    {
                        Quantity = double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value.ToString()!);
                    }

                    double FreeQuantity = 0;
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE] != null &&
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value != null &&
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value != DBNull.Value &&
                        !string.IsNullOrWhiteSpace(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value.ToString()))
                    {
                        FreeQuantity = double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value.ToString()!);
                    }

                    float Pprice = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value.ToString());
                    if (FreeQuantity > 0)
                    {
                        SaleDetail.isFree = true;
                    }
                    SaleDetail.Quantity = Quantity;
                    SaleDetail.FreeQuantity = FreeQuantity;
                    SaleDetail.Price = Pprice;
                    SaleDetail.ReturnFee = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value.ToString()) : 0;
                    SaleDetail.Amount = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value != null ? float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value.ToString()) : 0;
                    double Amount = Quantity * Pprice;
                    float Discount = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value.ToString());
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
                    float TaxPercentage = float.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value.ToString());
                    if (TaxPercentage > 0)
                    {
                        int j = 0;
                        if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value != null)
                        {
                            SaleDetail SalesDetail = SalesManager.GetSaleDetail((long)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value);
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
                                    SaleTaxDetail.CatalogItemSalesTaxMap = null;
                                    SaleDetail.TaxDetails.Add(SaleTaxDetail);
                                    j++;
                                }
                            }
                        }
                        else
                        {
                            foreach (CatalogItemSalesTaxMap PTaxMap in Product.SalesTax)
                            {
                                if (PTaxMap.EffectiveFrom <= Sale.SaleDate && PTaxMap.EffectiveTo >= Sale.SaleDate)
                                {
                                    CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId);
                                    if (CMap != null)
                                    {
                                        if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, Sale.SaleDate, Sale.AccountsId == null ? 0L : (long)Sale.AccountsId))
                                        {
                                            if (PTaxMap.TaxPercentage > 0)
                                            {
                                                TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                float TaxAmount = (float)(Amount * (PTaxMap.TaxPercentage / 100));
                                                TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                ItemLevelSaleTaxDetail ItemSaleTaxDetail = new ItemLevelSaleTaxDetail();
                                                ItemSaleTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId).AccountId;
                                                ItemSaleTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                ItemSaleTaxDetail.Amount = TaxAmount;
                                                ItemSaleTaxDetail.TaxSequence = j + 1;
                                                ItemSaleTaxDetail.ItemTaxMapId = PTaxMap.Id;
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
            lSaleEntry.TotalAmount = double.Parse(LabelSalesFinalAmount.Text);

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
        private void BtnSaleReturnNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnSaleReturnSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxSaleSearch.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            SearchSalesId = 0L;
            SearchSaleReturnId = 0L;
            TextBoxSaleSearch.Select();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSaleReturnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSaleReturnId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this sale return is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, LabelSaleReturnReferenceNumber.Text), "Delete Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long SalesID = Convert.ToInt64(TextBoxSaleReturnId.Text);
                SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesID);
                if (SaleEntry != null)
                {
                    bool DeleteResult = SalesManager.DeleteSaleEntry(SalesID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        TextBoxSaleSearch.Select();
                        DirtyFlag(false);
                    }
                    else
                    {
                        MessageBox.Show(DeleteErrorText);
                    }
                }
                else
                {
                    DisplaySystemError("Somthing went wrong, the selected sale return is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnSaleReturnCancel_Click(object sender, EventArgs e)
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
            if (SearchSaleReturnId != 0)
            {
                //CheckSaleReturn();
                LoadSaleReturn(SearchSaleReturnId);
            }
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void BtnSaleReturnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    SaleEntry lSales = GetSaleEntryFromForm();
                    SaleEntry lSalesFromDB = null!;
                    if (lSales.Id == 0)
                    {
                        IList<SaleEntry> lSaleEntryInfo = SalesManager.GetSaleEntryBySaleEntryId(SearchSalesId);
                        if (lSaleEntryInfo != null && lSaleEntryInfo.Count > 0)
                        {
                            foreach (SaleEntry saleEntry in lSaleEntryInfo)
                            {
                                bool DeleteResult = SalesManager.DeleteSaleEntry(saleEntry.Id);
                            }
                        }
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.SALES_RETURN, (DateTime)DatetimePickerSalesReturnDate.Date!);
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
                        SaleEntry SaleEntryInfo = SalesManager.GetSaleEntry(lSales.Id);
                        if (SaleEntryInfo != null)
                        {
                            IList<SaleEntry> lSaleEntryInfo = SalesManager.GetSaleEntryBySaleEntryId((long)lSales.SaleEntryId!);
                            if (lSaleEntryInfo != null && lSaleEntryInfo.Count > 1)
                            {
                                foreach (SaleEntry saleEntry in lSaleEntryInfo)
                                {
                                    if (lSales.Id != saleEntry.Id)
                                    {
                                        bool DeleteResult = SalesManager.DeleteSaleEntry(saleEntry.Id);
                                    }
                                }
                            }
                            lSalesFromDB = new SaleEntry();
                            lSalesFromDB = SalesManager.UpdateSaleEntry(lSales);
                        }
                        else
                        {
                            DisplaySystemError("Somthing went wrong, the selected sale return is not valid.");
                            return;
                        }
                    }
                    TextBoxSaleReturnId.Text = lSalesFromDB.Id.ToString();
                    LabelSaleReturnReferenceNumber.Text = lSalesFromDB.RefNumber;
                    if (lSalesFromDB != null)
                    {
                        LoadSaleReturn(lSalesFromDB.Id);
                        BtnSaleReturnPrint.Select();
                    }
                    ErrorMsgSaleReturn.Text = SaveSuccessText;
                    EnableForm(false);
                    DirtyFlag(false);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private Boolean ValidateForm()
        {
            if (DatetimePickerSalesDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerSalesDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerSalesDate.Focus();
                ErrorMsgSaleReturn.Text = EnterSaleDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DatetimePickerSalesReturnDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerSalesReturnDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerSalesReturnDate.Focus();
                ErrorMsgSaleReturn.Text = EnterReturnDateErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewSalesItem.Rows.Count;
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value == null)
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.PRODUCT, i];
                        GridViewSalesItem.BeginEdit(true);
                        ErrorMsgSaleReturn.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value.ToString());
                    double Free = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value.ToString());
                    for (int j = 3; j < 12; j++)
                    {
                        double Price = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value != null ? double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value.ToString()) : 0.00;
                        double Oprice = GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value != null ? double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value.ToString()) : 0.00;
                        if ((j == (int)SaleReturnItemTableColumn.QTY || j == (int)SaleReturnItemTableColumn.FREE) && !(Quantity <= 0 && Free <= 0)) { continue; }
                        if ((!((bool)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ISBAT].Value)) && (j == (int)SaleReturnItemTableColumn.EXPDATE || j == (int)SaleReturnItemTableColumn.BATNO)) { continue; }
                        if (j == (int)SaleReturnItemTableColumn.FREE) { continue; }
                        if (j == (int)SaleReturnItemTableColumn.PRICE && !(Oprice <= 0 && Price <= 0)) { continue; }
                        if (j == (int)SaleReturnItemTableColumn.RETURNFEEP && !(Oprice <= 0 && Price <= 0))
                        { j = j + 2; continue; }
                        if ((j == 7 || j == 8) && Quantity <= 0 && Free > 0 && Oprice <= 0 && Price <= 0)
                        { if (j == 8) { j = j + 2; } continue; }
                        if (j != 5 && j != 6 && j != 11 && (GridViewSalesItem.Rows[i].Cells[j].Value == null || GridViewSalesItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewSalesItem.Rows[i].Cells[j].Value.ToString()) <= 0))
                        {
                            GridViewSalesItem.Select();
                            if (j == 3 && GridViewSalesItem[j, i].ReadOnly)
                            {
                                GridViewSalesItem.CurrentCell = GridViewSalesItem[j + 1, i];
                            }
                            else
                            {
                                GridViewSalesItem.CurrentCell = GridViewSalesItem[j, i];
                            }
                            GridViewSalesItem.BeginEdit(true);
                            ErrorMsgSaleReturn.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewSalesItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATCHID].Value == null)
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.BATNO, i];
                            GridViewSalesItem.BeginEdit(true);
                            ErrorMsgSaleReturn.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewSalesItem.Columns[(int)SaleReturnItemTableColumn.BATNO].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 11 && GridViewSalesItem.Rows[i].Cells[j].Value != null && float.Parse(GridViewSalesItem.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            GridViewSalesItem.Select();
                            GridViewSalesItem.CurrentCell = GridViewSalesItem[j, i];
                            GridViewSalesItem.BeginEdit(true);
                            ErrorMsgSaleReturn.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewSalesItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 8)
                        {
                            j = j + 2;
                        }
                    }

                    SaleDetail Detail = SalesManager.Instance.GetSaleDetail((long)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value);
                    double CurQty = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value.ToString());
                    double CurFree = (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value.ToString());
                    if (CurQty > Detail.Quantity)
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, i];
                        GridViewSalesItem.BeginEdit(true);
                        ErrorMsgSaleReturn.Text = string.Format(EnterQuantityErrorMsg, Detail.Quantity);
                        ResetTimmer();
                        return false;
                    }
                    if (CurFree > Detail.FreeQuantity)
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.FREE, i];
                        GridViewSalesItem.BeginEdit(true);
                        ErrorMsgSaleReturn.Text = string.Format(EnterFreeErrorMsg, Detail.FreeQuantity);
                        ResetTimmer();
                        return false;
                    }
                    //returned qty
                    double ReturnedQty = 0.00;
                    IList<SaleDetail> lDetail = SalesManager.ListReturnSaleDetail(Detail.Id);
                    foreach (SaleDetail Rdetail in lDetail)
                    {
                        ReturnedQty += (Rdetail.FreeQuantity + Rdetail.Quantity);
                    }
                    //sale qty
                    double SaleQty = (Detail.Quantity + Detail.FreeQuantity);
                    if ((CurQty + CurFree) > (SaleQty))
                    {
                        GridViewSalesItem.Select();
                        GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, i];
                        GridViewSalesItem.BeginEdit(true);
                        ErrorMsgSaleReturn.Text = string.Format(EnterRemainingQuantityErrorMsg, (SaleQty - ReturnedQty));
                        ResetTimmer();
                        return false;
                    }
                }
            }
            else
            {
                GridViewSaleReturnItem.Select();
                ErrorMsgSaleReturn.Text = Grid_ChooseItemErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!DiscountAdditinalChargeGrid.IsDiscountAdditionalChargeValidationResult())
            {
                ErrorMsgSaleReturn.Text = DiscountAdditinalChargeGrid.ErrorMsg();
                ResetTimmer();
                return false;
            }
            if (float.Parse(LabelSalesFinalAmount.Text) < 0)
            {
                GridViewSalesItem.Select();
                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.PRODUCT, 0];
                GridViewSalesItem.BeginEdit(true);
                ErrorMsgSaleReturn.Text = InvalidSaleReturnErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxStockLocation.SelectedIndex < 0)
            {
                ErrorMsgSaleReturn.Text = SelectInventoryLoactionErrorMsg;
                ComboBoxStockLocation.Select();
                ResetTimmer();
                return false;
            }
            ErrorMsgSaleReturn.Text = SaveSuccessText;
            return true;
        }
        private void BtnSaleReturnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnSaleReturnPrint_Click(object sender, EventArgs e)
        {
            if (SalesManager.GetSaleEntry(long.Parse(TextBoxSaleReturnId.Text)) != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                PrinterSetup.SalePrintSetupNew(long.Parse(TextBoxSaleReturnId.Text), false, Entrytype.RETURN);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this sale is still valid.");
                return;
            }
        }
        private void BtnReceivePayment_Click(object sender, EventArgs e)
        {
            FormPOSReceivePayment POSPayment = new FormPOSReceivePayment();
            POSPayment.SalePaymentOnLoad = true;
            POSPayment.SearchSalesId = long.Parse(TextBoxSaleReturnId.Text);
            POSPayment.ShowDialog();
            LoadSaleReturn(long.Parse(TextBoxSaleReturnId.Text));
        }
        private void GridViewSaleReturnItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSaleReturnItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bool Checked;
                if (e.ColumnIndex == (int)SaleReturnTableColumn.CHOOSE && GridViewSaleReturnItem.Enabled)
                {
                    if (GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.REMAINING].Value.ToString() != "0")
                    {
                        Checked = (bool)GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.CHOOSE].Value;
                        GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.CHOOSE].Value = !Checked;
                        if (!Checked)
                        {
                            LoadProductSaleDetails((long)GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value);
                        }
                        else
                        {
                            RemoveProductSaleDetails((long)GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value);
                        }
                        ComputeFormTotal();
                        ReSequence(GridViewSalesItem);
                    }
                }
            }
        }
        private void RemoveProductSaleDetails(long DetailId)
        {
            for (int i = 0; i < GridViewSaleReturnItem.Rows.Count; i++)
            {
                if (((long)GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value) == DetailId)
                {
                    GridViewSalesItem.Rows.RemoveAt(i);
                    return;
                }
            }
        }
        private void LoadProductSaleDetails(long DetailId)
        {

            GridViewSalesItem.Rows.Add();
            int i = GridViewSalesItem.Rows.Count - 1;
            SaleDetail lSaleDetail = SalesManager.GetSaleDetail(DetailId);
            if (lSaleDetail != null)
            {
                SaleEntry SaleEntry = SalesManager.GetSaleEntry((long)lSaleDetail.SaleId);
                if (SaleEntry != null)
                {
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SNO].Value = i + 1;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.UOM].Value = lSaleDetail.Uom;
                    if (lSaleDetail.isBatch)
                    {
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATNO].Value = lSaleDetail.BatchNo;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.EXPDATE].Value = lSaleDetail.ExpDate;
                    }
                    double Return = (double)GridViewSaleReturnItem.CurrentRow.Cells[(int)SaleReturnTableColumn.RETURN].Value;
                    double FreeReturn = (double)GridViewSaleReturnItem.CurrentRow.Cells[(int)SaleReturnTableColumn.FREERETURN].Value;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value = (lSaleDetail.Quantity - (Return - FreeReturn));
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value = (lSaleDetail.FreeQuantity - FreeReturn);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value = lSaleDetail.Price;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value = lSaleDetail.ReturnFee;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry);
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value = (lSaleDetail.Discounts.Count > 0) ? lSaleDetail.Discounts.First().Discount : 0.00;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value = lSaleDetail.Amount;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value = lSaleDetail.ProductId;
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ISBAT].Value = lSaleDetail.isBatch;
                    //for tax
                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value = lSaleDetail.Id;
                    if (!string.IsNullOrEmpty(TextBoxSaleReturnId.Text))
                    {
                        SaleDetail llSaleDetail = SalesManager.GetReturnSaleDetail(DetailId);
                        if (llSaleDetail != null)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESDETAILID].Value = llSaleDetail.Id;
                        }
                    }
                    if (lSaleDetail.isBatch && lSaleDetail.BatchNo != null)
                    {
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lSaleDetail.ProductId, lSaleDetail.BatchNo, (long)SaleEntry.InventoryLocationId).Id;
                    }
                    ReSequence(GridViewSalesItem);
                }
            }
        }
        private void GridViewSalesItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.SNO].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.PRODUCT].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.UOM].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.QTY].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.FREE].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.BATNO].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.EXPDATE].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.PRICE].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.TAXP].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.TAX].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.DISP].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.DIS].ReadOnly = true;
            GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.ID].Value != null)
            {
                if (GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value != null)
                {
                    foreach (DataGridViewRow row in GridViewSaleReturnItem.Rows)
                    {
                        if (row.Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value.ToString())
                        {
                            double Qty = (double)row.Cells[(int)SaleReturnTableColumn.QTY].Value;
                            double FreeQty = (double)row.Cells[(int)SaleReturnTableColumn.FREE].Value;

                            double Return = (double)row.Cells[(int)SaleReturnTableColumn.RETURN].Value;
                            double FreeReturn = (double)row.Cells[(int)SaleReturnTableColumn.FREERETURN].Value;
                            if (((Qty - FreeQty) - (Return - FreeReturn)) > 0)
                            {
                                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.QTY].ReadOnly = false;
                            }
                            if ((FreeQty - FreeReturn) > 0)
                            {
                                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.FREE].ReadOnly = false;
                            }
                            if ((Qty - Return) > 0)
                            {
                                GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].ReadOnly = false;
                            }
                        }
                    }
                }

            }
        }
        private void GridViewSalesItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewSaleReturnItem.Enabled)
            {
                if (e.ColumnIndex == (int)SaleReturnItemTableColumn.REMOVE)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_DeleteConfirmText, GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.SNO].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewSalesItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (GridViewSaleReturnItem.Rows[e.RowIndex].Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value != null)
                        {
                            for (int i = 0; i <= GridViewSaleReturnItem.Rows.Count - 1; i++)
                            {
                                if (GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewSalesItem.Rows[e.RowIndex].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value.ToString())
                                {
                                    GridViewSaleReturnItem.Rows[i].Cells[(int)SaleReturnTableColumn.CHOOSE].Value = false;
                                    break;
                                }
                            }
                        }
                        GridViewSalesItem.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }
            }
        }
        public double Oprice = 0.00;
        public double Price = 0.00;
        private void GridViewSalesItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)SaleReturnItemTableColumn.QTY ||
               e.ColumnIndex == (int)SaleReturnItemTableColumn.FREE ||
               e.ColumnIndex == (int)SaleReturnItemTableColumn.RETURNFEEP ||
               e.ColumnIndex == (int)SaleReturnItemTableColumn.DISP)
            {
                Row_Added();
            }
        }
        bool IsOverrideTabCtr = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnSaleReturnNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnSaleReturnDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnSalesReturnEdit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnSaleReturnPrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnSaleReturnSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSaleReturnCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnSaleReturnExit.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewSaleReturnItem.CurrentRow.Index > -1)
                    {
                        int Index = GridViewSaleReturnItem.CurrentCell.RowIndex;

                        if (GridViewSaleReturnItem.CurrentCell.RowIndex != GridViewSaleReturnItem.Rows.Count - 1)
                        {
                            GridViewSaleReturnItem.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridViewSaleReturnItem.CurrentCell = GridViewSaleReturnItem[(int)SaleReturnTableColumn.CHOOSE, Index + 1];
                            }));
                        }
                        else
                        {
                            if (GridViewSaleReturnItem.CurrentCell.ColumnIndex == (int)SaleReturnTableColumn.CHOOSE)
                            {
                                SendKeys.Send("{+tab}");
                            }
                            YesNoRbtSalesMethod.Focus();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSaleReturnItem.CurrentRow.Index > -1)
                    {
                        if (GridViewSaleReturnItem.CurrentRow.Index != 0)
                        {
                            GridViewSaleReturnItem.CurrentCell = GridViewSaleReturnItem[(int)SaleReturnTableColumn.CHOOSE, GridViewSaleReturnItem.CurrentCell.RowIndex];
                        }
                        else
                        {
                            if (BtnSaleReturnSave.Enabled) { BtnSaleReturnSave.Select(); }
                            else
                            { TextBoxSaleSearch.Select(); }
                        }
                    }
                }
                if (GridViewSalesItem.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleReturnItemTableColumn.FREE)
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleReturnItemTableColumn.RETURNFEEP))
                    {
                        IsOverrideTabCtr = false;
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleReturnItemTableColumn.RETURNFEEP)
                    {
                        IsOverrideTabCtr = true;
                        if (GridViewSalesItem.CurrentCell.RowIndex != GridViewSalesItem.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && (GridViewSalesItem.CurrentCell.ColumnIndex == (int)SaleReturnItemTableColumn.QTY))
                    {
                        if (GridViewSalesItem.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                        else
                        {
                            DatetimePickerSalesReturnDate.Focus();
                        }
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
        private void DiscountAdditinalChargeGrid_Load(object sender, EventArgs e)
        {
            OverallTotal();
        }
        private void DiscountAdditinalChargeGrid_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.formIsDirty)
            {
                this.formIsDirty = DiscountAdditinalChargeGrid.IsDirty;
            }
        }
        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSaleReturnSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSalesItem.Rows.Count == 0)
                {
                    DatetimePickerSalesReturnDate.Focus();
                }
                else
                {
                    GridViewSalesItem.Select();
                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, 0];
                }
            }
        }
        private void GridViewSalesItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSalesItem_Leave(object sender, EventArgs e)
        {
            if (GridViewSalesItem.Rows.Count > 0) GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.UOM, GridViewSalesItem.CurrentRow.Index];
        }
        private bool GridFocus = false;
        private void GridViewSaleReturnItem_Enter(object sender, EventArgs e)
        {
            if (GridViewSaleReturnItem.Rows.Count > 0)
            {
                GridViewSaleReturnItem.CurrentCell = GridViewSaleReturnItem[(int)SaleReturnTableColumn.CHOOSE, 0];
            }
            GridFocus = true;
        }
        private void GridViewSaleReturnItem_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }
        private void BtnSaleReturnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewSaleReturnItem.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiscountAdditinalChargeGrid.Focus();
                return;
            }
        }
        private void DatetimePickerSalesReturnDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSalesItem.Rows.Count == 0)
                {
                    DiscountAdditinalChargeGrid.Focus();
                }
                else
                {
                    GridViewSalesItem.Select();
                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, 0];
                }
            }
        }
        private void BtnSaleSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSaleReturnItem.Rows.Count == 0)
                {
                    BtnSaleReturnExit.Select();
                }
            }
        }
        private void LoadSaleReturn(long SalesId)
        {
            ResetForm();
            SaleEntry SaleEntry = SalesManager.GetSaleEntry(SalesId);
            if (SaleEntry != null)
            {
                SearchSalesId = (long)SaleEntry.SaleEntryId;
                LoadSaleEntry((long)SaleEntry.SaleEntryId);
                LabelSaleReturnReferenceNumber.Text = SaleEntry.RefNumber;
                TextBoxSaleReturnId.Text = SaleEntry.Id.ToString();
                DatetimePickerSalesReturnDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.ReturnDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                DatetimePickerSalesDate.Date = (DateTime)DateUtils.ToDate(SaleEntry.SaleDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                YesNoRbtSalesMethod.Checked = (SaleEntry.SaleMethod == SaleMethod.Credit) ? true : false;
                if (SaleEntry.SaleDetails.Count > 0)
                {
                    GridViewSalesItem.Rows.Add(SaleEntry.SaleDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var SaleDetail in SaleEntry.SaleDetails)
                    {
                        foreach (DataGridViewRow Row in GridViewSaleReturnItem.Rows)
                        {
                            if (Row.Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value.ToString() == SaleDetail.SaleDetailId.ToString())
                            {
                                GridViewSaleReturnItem.Rows[Row.Index].Cells[(int)SaleReturnTableColumn.CHOOSE].Value = true;
                            }
                        }
                        SaleDetail lSaleDetail = SalesManager.GetSaleDetail(SaleDetail.Id);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SNO].Value = i + 1;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRODUCT].Value = lSaleDetail.Product.Name;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.UOM].Value = lSaleDetail.Uom;
                        if (lSaleDetail.isBatch)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATNO].Value = lSaleDetail.BatchNo;
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.EXPDATE].Value = lSaleDetail.ExpDate;
                        }
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].Value = lSaleDetail.Quantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].Value = lSaleDetail.FreeQuantity;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.PRICE].Value = lSaleDetail.Price;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].Value = lSaleDetail.ReturnFee;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lSaleDetail.TaxDetails, SaleEntry);
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.DISP].Value = (lSaleDetail.Discounts.Count > 0) ? lSaleDetail.Discounts.First().Discount : 0.00;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.AMOUNT].Value = lSaleDetail.Amount;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value = lSaleDetail.ProductId;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ISBAT].Value = lSaleDetail.isBatch;
                        //for tax
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESDETAILID].Value = lSaleDetail.Id;
                        GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value = lSaleDetail.SaleDetailId;

                        if (lSaleDetail.isBatch && lSaleDetail.BatchNo != null)
                        {
                            GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lSaleDetail.ProductId, lSaleDetail.BatchNo, (long)SaleEntry.InventoryLocationId).Id;
                        }
                        i++;
                    }
                    Row_Added();
                    ReSequence(GridViewSalesItem);

                    if (SaleEntry.SaleAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = SaleEntry.SaleAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                EnableForm(false);
                OverallTotal();
            }
        }
        private void TextBoxSaleReturnSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSaleSearch_Click(sender, e);
            }
        }
        private void FormSalesReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatetimePickerSalesReturnDate.Focus();
                    e.Cancel = true;
                }
            }
        }
        private void BtnSearchReturn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RecentReturn();
            if (SearchSaleReturnId != 0)
            {
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnSaleReturnSave_Click(sender, e);
                            LoadSaleReturn(SearchSaleReturnId);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadSaleReturn(SearchSaleReturnId);
                    }
                }
                else
                {
                    LoadSaleReturn(SearchSaleReturnId);
                }
            }
            else
            {
                //SearchSaleReturnId = 0L;
                //DisplaySystemError("The selected sale return is not available anymore");
                //return;
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            DirtyFlag(false);
            BtnSaleReturnCancel.PerformClick();
            return;
        }
        private void RecentReturn()
        {
            ErrorMsgSaleReturn.Text = "";
            String SearchText = TextBoxSearchReturn.Text.Trim();
            IList<SaleEntry> SalesInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                SalesInfo = SalesManager.GetRecentSaleEntrys(Global.Company.CompanyId, Entrytype.RETURN);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                SalesInfo = SalesManager.GetSaleEntryByAmount(Amount, SearchText, Global.Company.CompanyId, Entrytype.RETURN);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                SalesInfo = SalesManager.GetSaleEntryByDate((DateTime)Date, Global.Company.CompanyId, Entrytype.RETURN);
            }
            else
            {
                SalesInfo = SalesManager.GetSaleEntryByCustomerName(SearchText, Global.Company.CompanyId, Entrytype.RETURN);
            }
            if (SalesInfo.Count > 0)
            {
                LoadSaleReturn(SalesInfo);
            }
            else
            {
                SearchSaleReturnId = 0L;
                ErrorMsgSaleReturn.Text = SaleReturnSearchOutput;
            }
        }
        public void LoadSaleReturn(IList<SaleEntry> SalesInfo)
        {
            if (SalesInfo.Count > 0)
            {
                SearchSaleReturnId = 0L;
                FormRecentSales FormRecentSales = new FormRecentSales(this);
                FormRecentSales.Text = "Recent Sale Return";
                FormRecentSales.RecentEntrytype = Entrytype.RETURN;
                FormRecentSales.SaleEntryInfo = SalesInfo;
                FormRecentSales.ShowDialog();
            }
            else
            {
                ErrorMsgSaleReturn.Text = SaleReturnSearchOutput;
            }
        }
        private void TextBoxSearchReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchReturn_Click(sender, e);
            }
        }

        private void BtnSalesReturnEdit_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult result = MessageBox.Show(SaveConfirmText, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnSaleReturnSave_Click(sender, e);
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            EnableForm(true);
            TextBoxSaleSearch.ReadOnly = false;
            GridViewSalesItem.ReadOnly = false;
            BtnSaleReturnSave.Enabled = true;
            BtnSaleReturnCancel.Enabled = true;
            BtnSaleReturnNew.Enabled = false;
            BtnSaleReturnDelete.Enabled = false;

            for (int i = 0; i < GridViewSalesItem.Rows.Count; i++)
            {
                if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.ID].Value != null)
                {
                    if (GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value != null)
                    {
                        foreach (DataGridViewRow row in GridViewSaleReturnItem.Rows)
                        {
                            if (row.Cells[(int)SaleReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.SALESRETURNDETAILID].Value.ToString())
                            {
                                double Qty = (double)row.Cells[(int)SaleReturnTableColumn.QTY].Value;
                                double FreeQty = (double)row.Cells[(int)SaleReturnTableColumn.FREE].Value;

                                double Return = (double)row.Cells[(int)SaleReturnTableColumn.RETURN].Value;
                                double FreeReturn = (double)row.Cells[(int)SaleReturnTableColumn.FREERETURN].Value;
                                GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, 0];
                                if ((FreeQty - FreeReturn) > 0)
                                {
                                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.FREE].ReadOnly = false;
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.FREE, 0];
                                }
                                if (((Qty - FreeQty) - (Return - FreeReturn)) > 0)
                                {
                                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.QTY].ReadOnly = false;
                                    GridViewSalesItem.CurrentCell = GridViewSalesItem[(int)SaleReturnItemTableColumn.QTY, 0];
                                }
                                if ((Qty - Return) > 0)
                                {
                                    GridViewSalesItem.Rows[i].Cells[(int)SaleReturnItemTableColumn.RETURNFEEP].ReadOnly = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
