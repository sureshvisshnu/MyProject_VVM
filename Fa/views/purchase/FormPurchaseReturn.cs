using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.controls.accounting;
using fa.views.controls.grid;
using fa.views.utils;
using Fa.model.Purchase;
using Fa.report.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.purchase
{
    public partial class FormPurchaseReturn : FormBase
    {
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteErrorText = "Error in purchase return deleting. !";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product it takes latest Tax";
        public static string DeleteConfirmText = "Do you want to delete Purchase return {0}?";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string EnterPurchaseDateErrorMsg = "Please enter purchase date.";
        public static string EnterReturnDateErrorMsg = "Please enter valid return date.";
        public static string EnterQuantityErrorMsg = "Please Enter valid Quantity, Available Quantity for Return is {0}";
        public static string EnterFreeErrorMsg = "Please Enter valid Free Quantity, Available Free Quantity for Return is {0}";
        public static string SelectInventoryLoactionErrorMsg = "Please select inventory location.";


        public static string Grid_DeleteConfirmText = "Do you want to delete Row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter purchase return items details.";

        public static string InvalidPurchaseReturnErrorMsg = "Invalid purchase return.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could supplier name or purchase date or purchase reference number.";
        public static string PurchaseSearchOutput = "No purchase found.";
        public static string PurchaseReturnSearchOutput = "No purchase return found";
        public static string PurchaseReturn_ReturnAllQtyErrorMsg = "All purchased quantities are returned.";
        public static string PurchaseReturn_ForReturnNoStockErrorMsg = "Unable to return this item, No stock available for return";
        public static string EnterRemainingQuantityErrorMsg = "Please Enter valid Quantity, Remaining Quantity for Return is {0}";

        public enum PurchaseReturnItemTableColumn
        {
            SNO, PRODUCT, UOM, QTY, FREE, BATNO, EXPDATE, PRICE, RETURNFEEP, TAXP, TAX, DISP, DIS, AMOUNT, REMOVE, ID, ISBAT, PURCHASEDETAILID, BATCHID, PURCHASERETURNDETAILID
        }
        public enum PurchaseReturnTableColumn
        {
            SNO, ITEM_NAME, QTY, RETURN, REMAINING, CHOOSE, DETAIL_ID, FREERETURN, FREE, ID, BATCHNO
        }
        PurchaseEntryManager PurchaseEntryManager = null!;
        public bool PurchaseReturnOnLoad = false;
        public long Detail_Id = 0L;
        public long SearchPurchaseId = 0L;
        public long SearchPurchaseReturnId = 0L;
        public FormPurchaseReturn()
        {
            InitializeComponent();
            PurchaseEntryManager = PurchaseEntryManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "TextBoxSearchReturn", "GridViewPurchaseItem", "DiscountAdditinalChargeGrid" };
        }

        private void FormPurchaseReturn_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Visible = false;
            setSize();
            this.Visible = true;
            ResetForm();
            EnableForm(true);
            TextBoxPurchaseSearch.Select();
            if (PurchaseReturnOnLoad)
            {
                CheckPurchaseReturn();
                GridViewPurchaseReturnItem.Select();
            }
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void CheckPurchaseReturn()
        {
            EnableForm(true);
            LoadPurchaseEntry(SearchPurchaseId);
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
        private void LoadPurchaseReturnEntry(long PurchaseReturnId)
        {
            ResetForm();
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchaseReturnId);
            if (PurchaseEntry != null)
            {
                TextBoxPurchaseReturnId.Text = PurchaseReturnId.ToString();
                if (PurchaseEntry.Account != null)
                {
                    TextBoxPurchaseSupplierDetails.Text = PurchaseEntry.Account.Name;
                    TextBoxPurchaseSupplierDetails.Id = PurchaseEntry.AccountId.ToString()!;
                    if (!string.IsNullOrEmpty(PurchaseEntry.SupplierAddress))
                    {
                        TextBoxPurchaseSupplierDetails.Text = PurchaseEntry.SupplierAddress.Replace("\n", "").Replace(", ", "," + System.Environment.NewLine);
                    }
                }
                if (PurchaseEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)PurchaseEntry.InventoryLocationId!);
                    if (Location != null)
                    {
                        ComboBoxStockLocation.SelectedIndex = ComboBoxStockLocation.FindStringExact(Location.Name);
                    }
                }
                DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.RefDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtPurchaseMethod.Checked = (PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit) ? true : false;
                LabelPurchaseReturnReferenceNumber.Text = PurchaseEntry.RefNumber;
                DatetimePickerPurchaseReturnDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.ReturnDate.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                if (PurchaseEntry.PurchaseDetails.Count > 0)
                {
                    GridViewPurchaseItem.Rows.Clear();
                    GridViewPurchaseItem.Rows.Add(PurchaseEntry.PurchaseDetails.Count);
                    int i = 0;
                    foreach (var PurchaseDetail in PurchaseEntry.PurchaseDetails)
                    {
                        PurchaseDetails lPurchaseDetail = PurchaseEntryManager.GetPurchaseDetail(PurchaseDetail.Id);
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.SNO].Value = i + 1;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRODUCT].Value = lPurchaseDetail.Product.Name;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.UOM].Value = lPurchaseDetail.Product.WholesaleUOM;
                        if (lPurchaseDetail.isBatch)
                        {
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value = lPurchaseDetail.BatchNo;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.EXPDATE].Value = lPurchaseDetail.ExpDate;
                        }
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value = lPurchaseDetail.Quantity;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value = lPurchaseDetail.FreeQuantity;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value = lPurchaseDetail.PurchasePrice;
                        // GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.OPRICE].Value = lPurchaseDetail.OverridePrice;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lPurchaseDetail.TaxDetails);
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value = (lPurchaseDetail.Discounts.Count > 0) ? lPurchaseDetail.Discounts.First().Discount : 0.00;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value = lPurchaseDetail.Amount;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value = lPurchaseDetail.ProductId;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ISBAT].Value = lPurchaseDetail.isBatch;
                        //for tax

                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value = lPurchaseDetail.Id;
                        if (lPurchaseDetail.isBatch && lPurchaseDetail.BatchNo != null)
                        {
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetail.ProductId!, lPurchaseDetail.BatchNo, (long)PurchaseEntry.InventoryLocationId).Id;
                        }


                        i++;
                    }
                }
                if (PurchaseEntry.PurchaseAdditionalTransactions.Count > 0)
                {
                    DiscountAdditinalChargeGrid.AdditionalTransactions = PurchaseEntry.PurchaseAdditionalTransactions.ToList<AdditionalTransaction>();
                }
                Row_Added();
                BtnPurchaseReturnDelete.Select();
                BtnPurchaseReturnPrint.Select();
                EnableForm(false);
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("The selected Purchase return is not available anymore");
            }
        }
        private void LoadPurchaseEntry(long PurchaseId)
        {
            ResetForm();
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchaseId);
            if (PurchaseEntry != null)
            {
                SearchPurchaseId = PurchaseEntry.Id;
                if (PurchaseEntry.AccountId != null)
                {
                    TextBoxPurchaseSupplierDetails.Text = PurchaseEntry.Account.Name;
                    TextBoxPurchaseSupplierDetails.Id = PurchaseEntry.AccountId.ToString()!;
                    if (!string.IsNullOrEmpty(PurchaseEntry.SupplierAddress))
                    {
                        TextBoxPurchaseSupplierDetails.Text = (TextBoxPurchaseSupplierDetails.Text + "," + "\n" + PurchaseEntry.SupplierAddress).Replace("\n", System.Environment.NewLine);
                    }
                }
                if (PurchaseEntry.InventoryLocationId != 0L)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById((long)PurchaseEntry.InventoryLocationId!);
                    if (Location != null)
                    {
                        ComboBoxStockLocation.SelectedIndex = ComboBoxStockLocation.FindStringExact(Location.Name);
                    }
                }
                DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.RefDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                DatetimePickerPurchaseReturnDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtPurchaseMethod.Checked = (PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit) ? true : false;
                if (PurchaseEntry.PurchaseDetails.Count > 0)
                {
                    //calculate old return qty
                    IList<PurchaseEntry> PurchaseReturns = PurchaseEntryManager.ListPurchaseReturnsByPurchaseId(PurchaseId);
                    GridViewPurchaseReturnItem.Rows.Add(PurchaseEntry.PurchaseDetails.Count);
                    int i = 0;
                    foreach (var PurchaseDetail in PurchaseEntry.PurchaseDetails)
                    {
                        //calculate old return qty
                        double Qty = 0;
                        double Free = 0;
                        if (PurchaseReturns.Count > 0)
                        {
                            foreach (PurchaseEntry PurchaseReturn in PurchaseReturns)
                            {
                                foreach (PurchaseDetails PurchaseDetailReturn in PurchaseReturn.PurchaseDetails)
                                {
                                    if (PurchaseDetail.Id.ToString() == PurchaseDetailReturn.PurchaseDetailsId.ToString() && PurchaseDetailReturn.ProductId == PurchaseDetail.ProductId && PurchaseDetailReturn.BatchNo == PurchaseDetail.BatchNo)
                                    {
                                        Qty = Qty + PurchaseDetailReturn.Quantity;
                                        Free = Free + PurchaseDetailReturn.FreeQuantity;
                                    }
                                }
                            }
                        }

                        PurchaseDetails lPurchaseDetail = PurchaseEntryManager.GetPurchaseDetail(PurchaseDetail.Id);
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.SNO].Value = i + 1;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.ITEM_NAME].Value = lPurchaseDetail.Product.Name;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.QTY].Value = (lPurchaseDetail.Quantity + lPurchaseDetail.FreeQuantity);
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.REMAINING].Value = ((lPurchaseDetail.Quantity + lPurchaseDetail.FreeQuantity) - (Qty + Free));
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.RETURN].Value = (Qty + Free);
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.FREERETURN].Value = Free;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.FREE].Value = lPurchaseDetail.FreeQuantity;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.CHOOSE].Value = false;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value = lPurchaseDetail.Id;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.ID].Value = lPurchaseDetail.ProductId;
                        GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.BATCHNO].Value = lPurchaseDetail.BatchNo;

                        i++;
                    }
                    ReSequence(GridViewPurchaseReturnItem);
                    if (PurchaseEntry.PurchaseAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = PurchaseEntry.PurchaseAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }

                OverallTotal();
                GridViewPurchaseReturnItem.Focus();
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("The selected Purchase is not available anymore");
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

        private void ReSequence(DataGridView DataGridView)
        {
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                DataGridView.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.SNO].Value = i + 1;
            }
        }
        private void OverallTotal()
        {
            double RoundedTotal = DiscountAdditinalChargeGrid.OutputAmount;
            double RoundoffAmount = Rounds > 0 ? RoundOff(RoundedTotal) : 0;
            labelRoundOff.Text = "Round Off (" + (RoundoffAmount).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) + ")";
            DiscountAdditinalChargeGrid.OutputAmount = RoundedTotal + RoundoffAmount;
            LabelPurchaseFinalAmount.Text = DiscountAdditinalChargeGrid.OutputAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private void Row_Added()
        {
            ComputeFormTotal();
        }
        private void Row_Removed()
        {
            ReSequence(GridViewPurchaseItem);
            ComputeFormTotal();
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerPurchaseReturn.Stop();
            TimerPurchaseReturn.Start();
        }

        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            double TotalQuantity = 0;
            for (int i = 0; i < GridViewPurchaseItem.Rows.Count; i++)
            {
                double Quantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value.ToString()!));
                double FreeQuantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value.ToString()!));
                double Pprice = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value) == null ? 0.00 : (double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value.ToString()!));

                TotalQuantity = TotalQuantity + Quantity;
                double Amount = 0.00;

                Amount = (Pprice * Quantity);
                if (Amount > 0)
                {
                    double ReturnFeePercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].Value.ToString()!));
                    double ReturnFee = Amount * (ReturnFeePercentage / 100);

                    double DiscountPercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value.ToString()!));

                    double DiscountAmount = Amount * (DiscountPercentage / 100);
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DIS].Value = DiscountAmount;
                    Amount = Amount - DiscountAmount;

                    double TaxPercentage = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value) == null ? 0.00 : (float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value.ToString()!));

                    double TaxAmount = Amount * (TaxPercentage / 100);
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAX].Value = TaxAmount;
                    Amount = Amount + TaxAmount + ReturnFee;

                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value = Amount;

                    TotalAmount = TotalAmount + Amount;
                }
                else
                {
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DIS].Value = 0.00;
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAX].Value = 0.00;
                }
            }
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value = TotalAmount;

            DiscountAdditinalChargeGrid.Quantity = TotalQuantity;
            DiscountAdditinalChargeGrid.InputAmount = TotalAmount;
            OverallTotal();
        }

        private void ResetForm()
        {
            ComboUtils.InitializeStockLocationCombo(ComboBoxStockLocation, Global.Company.CompanyId);
            ComboBoxStockLocation.SelectedIndex = -1;
            ComboBoxStockLocation.ResetText();
            LabelPurchaseReturnReferenceNumber.Text = "000000";
            TextBoxPurchaseSearch.ResetText();
            TextBoxSearchReturn.TextBox.ResetText();
            ErrorMsgPurchaseReturn.Text = "";
            TextBoxPurchaseReturnId.ResetText();
            TextBoxPurchaseSupplierDetails.ResetText();
            DatetimePickerPurchaseDate.Format = Global.Company.DateFormat;
            DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DatetimePickerPurchaseReturnDate.Format = Global.Company.DateFormat;
            DatetimePickerPurchaseReturnDate.MinDate = Global.getTransactionDate();
            DatetimePickerPurchaseReturnDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            LabelPrevPurchaseReturnReferenceNumber.Text = CompanyManager.Instance.GetPurchasePrevRef(Global.Company, PurchaseEntrytype.RETURN, (DateTime)DatetimePickerPurchaseReturnDate.Date);
            GridViewPurchaseReturnItem.Rows.Clear();
            YesNoRbtPurchaseMethod.Checked = true;
            TextBoxPurchaseSupplierDetails.ResetText();
            GridViewPurchaseItem.Rows.Clear();
            DiscountAdditinalChargeGrid.GridType = GridType.Purchase;
            DiscountAdditinalChargeGrid.Clear();
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.NAME].Value = "Total : ";
            GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            LabelPurchaseFinalAmount.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            TextBoxPurchaseSupplierDetails.ResetText();

            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewPurchaseItem.Columns["ReturnPrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)GridViewPurchaseItem.Columns["ReturnAmount"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)GridViewPurchaseItem.Columns["ReturnTax"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)GridViewPurchaseItem.Columns["ReturnDic"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;

            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)GridViewPurchaseItemTotal.Columns["Value"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxStockLocation.Visible = false;
            GridViewPurchaseItem.ScrollBars = ScrollBars.Both;

            YesNoRbtPurchaseMethod.Enabled = false;

            GridViewPurchaseItem.ReadOnly = !enable;
            GridViewPurchaseItem.TabStop = enable;
            GridViewPurchaseReturnItem.Enabled = enable;

            if (Global.isDateModification && enable)
            {
                DatetimePickerPurchaseReturnDate.Enabled = true;
            }
            else
            {
                DatetimePickerPurchaseReturnDate.Enabled = false;
            }
            DiscountAdditinalChargeGrid.Enabled = enable;
            TextBoxPurchaseSearch.ReadOnly = !enable;
            TextBoxPurchaseSearch.TabStop = enable;
            if (enable)
            {
                BtnPurchaseSearch.Enabled = enable;
                GridViewPurchaseReturnItem.ReadOnly = enable;
                GridViewPurchaseReturnItem.TabStop = !enable;
                BtnPurchaseReturnNew.Enabled = enable;
                BtnPurchaseReturnDelete.Enabled = !enable;
                BtnPurchaseReturnEdit.Enabled = !enable;
                BtnPurchaseReturnCancel.Enabled = enable;
                BtnPurchaseReturnSave.Enabled = enable;
                BtnPurchaseReturnPrint.Enabled = !enable;
                if (!string.IsNullOrEmpty(TextBoxPurchaseReturnId.Text))
                {
                    BtnPurchaseReturnDelete.Enabled = enable;
                    BtnPurchaseReturnPrint.Enabled = enable;
                }
            }
            else
            {
                BtnPurchaseSearch.Enabled = enable;
                BtnPurchaseReturnNew.Enabled = !enable;
                BtnPurchaseReturnDelete.Enabled = !enable;
                BtnPurchaseReturnEdit.Enabled = !enable;
                BtnPurchaseReturnCancel.Enabled = enable;
                BtnPurchaseReturnSave.Enabled = enable;
                BtnPurchaseReturnPrint.Enabled = !enable;
            }

        }

        private void BtnPurchaseSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RecentPurchasess(PurchaseEntrytype.PURCHASE);
            if (SearchPurchaseId != 0)
            {
                CheckPurchaseReturn();
                //if (this.formIsDirty)
                //{
                //    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                //    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                //    if (Result == DialogResult.Yes)
                //    {
                //        if (ValidateForm())
                //        {
                //            BtnPurchaseReturnSave_Click(sender, e);
                //            CheckPurchaseReturn();
                //        }
                //    }
                //    else if (Result == DialogResult.No)
                //    {
                //        CheckPurchaseReturn();
                //    }
                //}
                //else
                //{
                //    CheckPurchaseReturn();
                //}
            }
            else
            {
                //SearchPurchaseId = 0L;
                //DisplaySystemError("The selected Purchase is not available anymore");
                //return;
            }
            Cursor.Current = Cursors.Default;
        }

        private void RecentPurchasess(PurchaseEntrytype Type)
        {
            ErrorMsgPurchaseReturn.Text = "";
            String SearchText = TextBoxPurchaseSearch.Text.Trim();
            IList<PurchaseEntry> PurchasesInfo = null!;
            if (string.IsNullOrEmpty(SearchText))
            {
                PurchasesInfo = PurchaseEntryManager.GetRecentPurchaseEntrys(Global.Company.CompanyId, Type);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryByAmount(Amount, SearchText, Global.Company.CompanyId, Type);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat)!;
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryByDate((DateTime)Date, Global.Company.CompanyId, Type);
            }
            else
            {
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryBySupplierName(SearchText, Global.Company.CompanyId, Type);
            }

            if (PurchasesInfo.Count > 0)
            {
                LoadPurchases(PurchasesInfo);
            }
            else
            {
                SearchPurchaseId = 0L;
                ErrorMsgPurchaseReturn.Text = PurchaseSearchOutput;

            }
        }
        public void LoadPurchases(IList<PurchaseEntry> PurchasesInfo)
        {
            if (PurchasesInfo.Count > 0)
            {
                SearchPurchaseId = 0L;
                FormRecentPurchase FormRecentPurchases = new FormRecentPurchase(this);
                FormRecentPurchases.RecentEntrytype = PurchaseEntrytype.PURCHASE;
                FormRecentPurchases.PurchaseEntryInfo = PurchasesInfo;
                FormRecentPurchases.ShowDialog();

            }
            else
            {
                ErrorMsgPurchaseReturn.Text = PurchaseSearchOutput;
            }
        }
        private PurchaseEntry GetPurchaseEntryFromForm()
        {
            PurchaseEntry lPurchaseEntry = new PurchaseEntry();

            lPurchaseEntry.Id = TextBoxPurchaseReturnId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxPurchaseReturnId.Text);
            lPurchaseEntry.PurchaseEntrytype = PurchaseEntrytype.RETURN;
            lPurchaseEntry.RefNumber = LabelPurchaseReturnReferenceNumber.Text;
            PurchaseEntry Purchase = PurchaseEntryManager.GetPurchaseEntry(SearchPurchaseId);
            if (Purchase != null)
            {
                lPurchaseEntry.PurchaseEntryId = Purchase.Id;
                lPurchaseEntry.SupplierName = Purchase.SupplierName;
                lPurchaseEntry.AccountId = Purchase.AccountId;
                lPurchaseEntry.SupplierAddress = Purchase.SupplierAddress;
            }
            lPurchaseEntry.PurchaseMethod = (YesNoRbtPurchaseMethod.Checked) ? PurchaseMethod.Credit : PurchaseMethod.Cash;
            lPurchaseEntry.RefDate = (DateTime)DatetimePickerPurchaseDate.Date!;
            if (lPurchaseEntry.Id != 0)
            {
                PurchaseEntry PurchaseEndry = PurchaseEntryManager.GetPurchaseEntry(lPurchaseEntry.Id);
                lPurchaseEntry.ReturnDate = PurchaseEndry.ReturnDate;
            }
            else
            {
                lPurchaseEntry.ReturnDate = (DateTime)DatetimePickerPurchaseReturnDate.Date!;
            }
            lPurchaseEntry.PurchaseInvDate = (DateTime)DatetimePickerPurchaseDate.Date;
            lPurchaseEntry.CompanyId = Global.Company.CompanyId;
            //for sql
            if (Global.CostCenter != null)
            {
                lPurchaseEntry.CostCenterId = Global.CostCenter.CostCenterId;
            }
            if (ComboBoxStockLocation.SelectedIndex > -1)
            {
                lPurchaseEntry.InventoryLocationId = ((InventoryLocation)ComboBoxStockLocation.Items[ComboBoxStockLocation.SelectedIndex]).Id;
            }
            float TotalAmount = float.Parse(GridViewPurchaseItemTotal.Rows[0].Cells[(int)PurchaseEntryTotalTableColumn.VALUE].Value.ToString()!);
            float TotalTaxAmount = 0;
            float TotalDiscountPercentage = 0;
            float TotalDiscountAmount = 0;


            float[] TaxWisePercentageTotals = new float[Global.Company.SalesTaxAccountMaps.Count];
            float[] TaxWiseAmountTotals = new float[Global.Company.SalesTaxAccountMaps.Count];

            for (int i = 0; i < GridViewPurchaseItem.Rows.Count; i++)
            {
                PurchaseDetails PurchaseDetail = new PurchaseDetails();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value);
                if (Product != null)
                {
                    PurchaseDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        PurchaseDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    PurchaseDetail.Id = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value == null) ? 0L : long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value.ToString()!);
                    PurchaseDetail.PurchaseDetailsId = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value == null) ? 0L : long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value.ToString()!);
                    PurchaseDetail.ProductId = Product.Id;
                    PurchaseDetail.MaterialId = Product.MaterialId;
                    PurchaseDetail.isBatch = (bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ISBAT].Value;
                    PurchaseDetail.ExpDate = DateTime.Now.Date;
                    if (PurchaseDetail.isBatch)
                    {
                        PurchaseDetail.BatchNo = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value.ToString();
                        PurchaseDetail.ExpDate = (DateTime)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.EXPDATE].Value;
                    }
                    double Quantity = 0;
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY] != null)
                    {
                        Quantity = double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value?.ToString() ?? "0.00");

                    }
                    double FreeQuantity = 0;
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE] != null)
                    {
                        FreeQuantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value.ToString()!);
                    }
                    float Pprice = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value.ToString()!);
                    if (FreeQuantity > 0)
                    {
                        PurchaseDetail.isFree = true;
                    }
                    PurchaseDetail.Quantity = Quantity;
                    PurchaseDetail.FreeQuantity = FreeQuantity;
                    PurchaseDetail.PurchasePrice = Pprice;
                    PurchaseDetail.WholesaleUOM = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.UOM].Value.ToString();
                    PurchaseDetail.Amount = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value != null ? float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value.ToString()!) : 0;
                    double Amount = Quantity * Pprice;
                    float Discount = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value.ToString()!);
                    if (Discount > 0)
                    {
                        TotalDiscountPercentage = TotalDiscountPercentage + Discount;
                        float DiscountAmount = (float)Amount * (Discount / 100);
                        TotalDiscountAmount = TotalDiscountAmount + DiscountAmount;
                        LineLevelPurchaseDiscount PurchaseDiscount = new LineLevelPurchaseDiscount();
                        PurchaseDiscount.DisccountType = DiscountType.PERCENT;
                        PurchaseDiscount.Discount = Discount;
                        PurchaseDiscount.DiscountAmount = DiscountAmount;
                        PurchaseDiscount.DiscountSequence = 1;
                        PurchaseDetail.Discounts.Add(PurchaseDiscount);

                        Amount = Amount - DiscountAmount;
                    }

                    PurchaseDetail.TaxDetails = new List<LineLevelPurchaseTaxDetail>();
                    float TaxPercentage = float.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value.ToString()!);
                    if (TaxPercentage > 0)
                    {
                        int j = 0;

                        if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value != null)
                        {
                            PurchaseDetails PurchasesDetail = PurchaseEntryManager.GetPurchaseDetail((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value);
                            if (PurchasesDetail.TaxDetails.Count > 0)
                            {
                                foreach (LineLevelPurchaseTaxDetail PurchaseTaxDetail in PurchasesDetail.TaxDetails)
                                {
                                    TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PurchaseTaxDetail.TaxRate;
                                    float TaxAmount = (float)Amount * (PurchaseTaxDetail.TaxRate / 100);
                                    TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                    PurchaseTaxDetail.Amount = TaxAmount;
                                    PurchaseTaxDetail.Id = 0L;
                                    PurchaseTaxDetail.PurchaseDetails = null;
                                    PurchaseDetail.TaxDetails.Add(PurchaseTaxDetail);
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
                                        CompanySalesTaxAccountMap CMap = CompanyManager.Instance.GetCompanySaleTaxMapById((long)PTaxMap.SalesTaxMapId!);
                                        if (CMap != null)
                                        {
                                            if (CountryManager.Instance.IncludeTax(CMap.CountrySaleTax, Global.Company, Global.getTransactionDate(), (Purchase!.AccountId == null ? 0L : (long)Purchase.AccountId)))
                                            {
                                                TaxWisePercentageTotals[j] = TaxWisePercentageTotals[j] + PTaxMap.TaxPercentage;
                                                float TaxAmount = (float)Amount * (PTaxMap.TaxPercentage / 100);
                                                TaxWiseAmountTotals[j] = TaxWiseAmountTotals[j] + TaxAmount;
                                                LineLevelPurchaseTaxDetail ItemPurchaseTaxDetail = new LineLevelPurchaseTaxDetail();
                                                ItemPurchaseTaxDetail.TaxAccountId = Global.Company.SalesTaxAccountMaps.FirstOrDefault(x => x.MapId == PTaxMap.SalesTaxMapId)!.AccountId;
                                                ItemPurchaseTaxDetail.TaxRate = PTaxMap.TaxPercentage;
                                                ItemPurchaseTaxDetail.Amount = TaxAmount;
                                                ItemPurchaseTaxDetail.TaxSequence = j + 1;
                                                ItemPurchaseTaxDetail.ItemTaxMapId = PTaxMap.Id;
                                                PurchaseDetail.TaxDetails.Add(ItemPurchaseTaxDetail);
                                            }
                                        }
                                    }
                                    j++;
                                }
                            }
                        }
                    }



                    lPurchaseEntry.PurchaseDetails.Add(PurchaseDetail);
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
                foreach (AdditionalTransaction AdditionalTransaction in DiscountAdditinalChargeGrid.AdditionalTransactions.ToList())
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
            lPurchaseEntry.DisAmount = TotalDiscountAmount;
            lPurchaseEntry.TaxAmount = TotalTaxAmount;
            lPurchaseEntry.TotalAmount = double.Parse(LabelPurchaseFinalAmount.Text);

            lPurchaseEntry.RoundOff = TotalAmount - lPurchaseEntry.TotalAmount;
            lPurchaseEntry.NetAmount = TotalAmount;

            return lPurchaseEntry;
        }
        double Rounds = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.PURCHASE_RETURN)!.RoundOff;
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

        private void BtnPurchaseReturnNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnPurchaseReturnSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxPurchaseSearch.Select();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            SearchPurchaseId = 0L;
            SearchPurchaseReturnId = 0L;
            TextBoxPurchaseSearch.Select();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseReturnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPurchaseReturnId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this Purchase return is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, LabelPurchaseReturnReferenceNumber.Text), "Delete Confirm",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long PurchasesID = Convert.ToInt64(TextBoxPurchaseReturnId.Text);
                PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchasesID);
                if (PurchaseEntry != null)
                {
                    bool DeleteResult = PurchaseEntryManager.DeletePurchaseEntry(PurchasesID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        TextBoxPurchaseSearch.Select();
                        DirtyFlag(false);
                    }
                    else
                    {
                        MessageBox.Show(DeleteErrorText);
                    }
                }
                else
                {
                    DisplaySystemError("Somthing went wrong, the selected Purchase return is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnPurchaseReturnCancel_Click(object sender, EventArgs e)
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
            if (SearchPurchaseReturnId != 0)
            {
                //CheckPurchaseReturn();
                LoadPurchaseReturn(SearchPurchaseReturnId);
            }
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPurchaseReturnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    PurchaseEntry lPurchases = GetPurchaseEntryFromForm();
                    PurchaseEntry lPurchasesFromDB = null!;
                    if (lPurchases.Id == 0)
                    {
                        IList<PurchaseEntry> lPurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntryByPurchaseEntryId(SearchPurchaseId);
                        if (lPurchaseEntryInfo != null && lPurchaseEntryInfo.Count > 0)
                        {
                            foreach (PurchaseEntry purchaseEntry in lPurchaseEntryInfo)
                            {
                                bool DeleteResult = PurchaseEntryManager.DeletePurchaseEntry(purchaseEntry.Id);
                            }
                        }
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PURCHASE_RETURN, (DateTime)DatetimePickerPurchaseReturnDate.Date!);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lPurchases.RefNumber = RefNumber;
                            lPurchasesFromDB = new PurchaseEntry();
                            try
                            {
                                lPurchasesFromDB = PurchaseEntryManager.AddPurchaseEntry(lPurchases);
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
                        PurchaseEntry PurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntry(lPurchases.Id);
                        if (PurchaseEntryInfo != null)
                        {
                            IList<PurchaseEntry> lPurchaseEntryInfo = PurchaseEntryManager.GetPurchaseEntryByPurchaseEntryId((long)lPurchases.PurchaseEntryId!);
                            if (lPurchaseEntryInfo != null && lPurchaseEntryInfo.Count > 1)
                            {
                                foreach (PurchaseEntry purchaseEntry in lPurchaseEntryInfo)
                                {
                                    if (lPurchases.Id != purchaseEntry.Id)
                                    {
                                        bool DeleteResult = PurchaseEntryManager.DeletePurchaseEntry(purchaseEntry.Id);
                                    }
                                }
                            }
                            lPurchasesFromDB = new PurchaseEntry();
                            lPurchasesFromDB = PurchaseEntryManager.UpdatePurchaseEntry(lPurchases);
                        }
                        else
                        {
                            DisplaySystemError("Somthing went wrong, the selected Purchase return is not valid.");
                            return;
                        }
                    }
                    TextBoxPurchaseReturnId.Text = lPurchasesFromDB.Id.ToString();
                    LabelPurchaseReturnReferenceNumber.Text = lPurchasesFromDB.RefNumber;

                    if (lPurchasesFromDB != null)
                    {
                        LoadPurchaseReturn(lPurchasesFromDB.Id);
                    }
                    ErrorMsgPurchaseReturn.Text = SaveSuccessText;
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
            if (DatetimePickerPurchaseDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerPurchaseDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerPurchaseDate.Focus();
                ErrorMsgPurchaseReturn.Text = EnterPurchaseDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (TextBoxPurchaseReturnId.Text == string.Empty)
            {
                if (DatetimePickerPurchaseReturnDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerPurchaseReturnDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    DatetimePickerPurchaseReturnDate.Focus();
                    ErrorMsgPurchaseReturn.Text = EnterReturnDateErrorMsg;
                    ResetTimmer();
                    return false;
                }
            }
            int Count = GridViewPurchaseItem.Rows.Count;
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value == null)
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.PRODUCT, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ErrorMsgPurchaseReturn.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value.ToString()!);
                    double Free = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value.ToString()!);

                    for (int j = 3; j < 12; j++)
                    {
                        double Price = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value != null ? double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value.ToString()!) : 0.00;
                        double Oprice = GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].Value != null ? double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].Value.ToString()!) : 0.00;

                        if ((j == (int)PurchaseReturnItemTableColumn.QTY || j == (int)PurchaseReturnItemTableColumn.FREE) && !(Quantity <= 0 && Free <= 0)) { continue; }
                        if ((!((bool)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ISBAT].Value)) && (j == (int)PurchaseReturnItemTableColumn.EXPDATE || j == (int)PurchaseReturnItemTableColumn.BATNO)) { continue; }
                        if (j == (int)PurchaseReturnItemTableColumn.FREE) { continue; }
                        if (j == (int)PurchaseReturnItemTableColumn.PRICE && !(Oprice <= 0 && Price <= 0)) { continue; }
                        if (j == (int)PurchaseReturnItemTableColumn.RETURNFEEP && !(Oprice <= 0 && Price <= 0))
                        { j = j + 2; continue; }
                        if ((j == 7 || j == 8) && Quantity <= 0 && Free > 0 && Oprice <= 0 && Price <= 0)
                        { if (j == 8) { j = j + 2; } continue; }
                        if (j != 5 && j != 6 && j != 11 && (GridViewPurchaseItem.Rows[i].Cells[j].Value == null || GridViewPurchaseItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewPurchaseItem.Rows[i].Cells[j].Value.ToString()!) <= 0))
                        {
                            GridViewPurchaseItem.Select();
                            if (j == 3 && GridViewPurchaseItem[j, i].ReadOnly)
                            {
                                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j + 1, i];
                            }
                            else
                            {
                                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j, i];
                            }
                            GridViewPurchaseItem.BeginEdit(true);
                            ErrorMsgPurchaseReturn.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewPurchaseItem.Columns[j].HeaderText == "QTY" ? "Quantity" : GridViewPurchaseItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATCHID].Value == null)
                        {
                            GridViewPurchaseItem.Select();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.BATNO, i];
                            GridViewPurchaseItem.BeginEdit(true);
                            ErrorMsgPurchaseReturn.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewPurchaseItem.Columns[(int)PurchaseReturnItemTableColumn.BATNO].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 11 && GridViewPurchaseItem.Rows[i].Cells[j].Value != null && float.Parse(GridViewPurchaseItem.Rows[i].Cells[j].Value.ToString()!) > 100)
                        {
                            GridViewPurchaseItem.Select();
                            GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[j, i];
                            GridViewPurchaseItem.BeginEdit(true);
                            ErrorMsgPurchaseReturn.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewPurchaseItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 8)
                        {
                            j = j + 2;
                        }
                    }

                    PurchaseDetails Detail = PurchaseEntryManager.Instance.GetPurchaseDetail((long)GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value);
                    double CurQty = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value.ToString()!);
                    double CurFree = (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value.ToString()!);
                    if (CurQty > Detail.Quantity)
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ErrorMsgPurchaseReturn.Text = string.Format(EnterQuantityErrorMsg, Detail.Quantity);
                        ResetTimmer();
                        return false;
                    }
                    if (CurFree > Detail.FreeQuantity)
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.FREE, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ErrorMsgPurchaseReturn.Text = string.Format(EnterFreeErrorMsg, Detail.FreeQuantity);
                        ResetTimmer();
                        return false;
                    }

                    long DetailId = long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value.ToString()!);
                    double ReturnedQty = 0.00;
                    IList<PurchaseDetails> lDetail = PurchaseEntryManager.ListReturnPurchaseDetail(DetailId);
                    foreach (PurchaseDetails Rdetail in lDetail)
                    {
                        ReturnedQty += (Rdetail.FreeQuantity + Rdetail.Quantity);
                    }

                    //for compare old return
                    double PurchasedQty = (Detail.Quantity + Detail.FreeQuantity);

                    if ((CurQty + CurFree) > (PurchasedQty))
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ErrorMsgPurchaseReturn.Text = string.Format(EnterRemainingQuantityErrorMsg, (PurchasedQty - ReturnedQty));
                        ResetTimmer();
                        return false;
                    }

                    //check quatity on hand
                    double QtyOnHand = 0.00;
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value != null)
                    {
                        InventoryLocation InventoryLocation = ((InventoryLocation)ComboBoxStockLocation.Items[ComboBoxStockLocation.SelectedIndex]);
                        Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value.ToString()!), InventoryLocation.Id);
                        if (Inventory != null)
                        {
                            QtyOnHand = ((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerPurchaseDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand);
                        }
                        if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(long.Parse(GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value.ToString()!), GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value.ToString(), InventoryLocation.Id);
                            if (InventoryBatch != null)
                            {
                                QtyOnHand = ((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerPurchaseReturnDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand);
                            }
                        }
                    }
                    if (Global.Company.CompanySalesSetup.IsNegativeStockAllowed && ((CurQty + CurFree) > QtyOnHand))
                    {
                        GridViewPurchaseItem.Select();
                        GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, i];
                        GridViewPurchaseItem.BeginEdit(true);
                        ErrorMsgPurchaseReturn.Text = string.Format(EnterQuantityErrorMsg, QtyOnHand);
                        ResetTimmer();
                        return false;
                    }

                }
            }
            else
            {
                GridViewPurchaseReturnItem.Select();
                ErrorMsgPurchaseReturn.Text = Grid_ChooseItemErrorMsg;
                ResetTimmer();
                return false;
            }
            if (!DiscountAdditinalChargeGrid.IsDiscountAdditionalChargeValidationResult())
            {
                ErrorMsgPurchaseReturn.Text = DiscountAdditinalChargeGrid.ErrorMsg();
                ResetTimmer();
                return false;
            }
            if (float.Parse(LabelPurchaseFinalAmount.Text) < 0)
            {
                GridViewPurchaseItem.Select();
                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.PRODUCT, 0];
                GridViewPurchaseItem.BeginEdit(true);
                ErrorMsgPurchaseReturn.Text = InvalidPurchaseReturnErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxStockLocation.SelectedIndex < 0)
            {
                ErrorMsgPurchaseReturn.Text = SelectInventoryLoactionErrorMsg;
                ComboBoxStockLocation.Select();
                ResetTimmer();
                return false;
            }
            ErrorMsgPurchaseReturn.Text = SaveSuccessText;
            return true;
        }

        private void BtnPurchaseReturnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GridViewPurchaseReturnItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewPurchaseReturnItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bool Checked;
                if (e.ColumnIndex == (int)PurchaseReturnTableColumn.CHOOSE && GridViewPurchaseReturnItem.Enabled)
                {
                    if (GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.REMAINING].Value.ToString() != "0")
                    {
                        double Remaining = double.Parse(GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.REMAINING].Value.ToString()!);
                        double QtyOnHand = 0.00;
                        if (GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.ID].Value != null)
                        {
                            InventoryLocation InventoryLocation = ((InventoryLocation)ComboBoxStockLocation.Items[ComboBoxStockLocation.SelectedIndex]);
                            Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(long.Parse(GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.ID].Value.ToString()!), InventoryLocation.Id);
                            if (Inventory != null)
                            {
                                QtyOnHand = ((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerPurchaseDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand);
                            }
                            if (GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.BATCHNO].Value != null
                                && !string.IsNullOrEmpty(GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.BATCHNO].Value.ToString()))
                            {
                                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(long.Parse(GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.ID].Value.ToString()!), GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.BATCHNO].Value.ToString(), InventoryLocation.Id);
                                if (InventoryBatch != null)
                                {
                                    QtyOnHand = ((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerPurchaseReturnDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand);
                                }
                            }
                        }
                        if (QtyOnHand != 0)
                        {
                            Checked = (bool)GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.CHOOSE].Value;
                            GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.CHOOSE].Value = !Checked;
                            if (!Checked)
                            {
                                LoadProductPurchaseDetails((long)GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value);
                            }
                            else
                            {
                                RemoveProductPurchaseDetails((long)GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value);

                            }
                            ComputeFormTotal();
                            ReSequence(GridViewPurchaseItem);
                        }
                        else
                        {
                            ErrorMsgPurchaseReturn.Text = PurchaseReturn_ForReturnNoStockErrorMsg;
                            ResetTimmer();
                        }
                    }
                    else
                    {
                        ErrorMsgPurchaseReturn.Text = PurchaseReturn_ReturnAllQtyErrorMsg;
                        ResetTimmer();
                    }
                }
            }
        }
        private void GridViewPurchaseReturnItem_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }
        private void GridViewPurchaseItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.SNO].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.PRODUCT].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.UOM].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.QTY].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.FREE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.BATNO].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.EXPDATE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.PRICE].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.TAXP].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.TAX].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.DISP].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.DIS].ReadOnly = true;
            GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.ID].Value != null)
            {
                if (GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value != null)
                {
                    foreach (DataGridViewRow row in GridViewPurchaseReturnItem.Rows)
                    {
                        if (row.Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value.ToString())
                        {
                            double Qty = (double)row.Cells[(int)PurchaseReturnTableColumn.QTY].Value;
                            double FreeQty = (double)row.Cells[(int)PurchaseReturnTableColumn.FREE].Value;

                            double Return = (double)row.Cells[(int)PurchaseReturnTableColumn.RETURN].Value;
                            double FreeReturn = (double)row.Cells[(int)PurchaseReturnTableColumn.FREERETURN].Value;
                            if (((Qty - FreeQty) - (Return - FreeReturn)) > 0)
                            {
                                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.QTY].ReadOnly = false;
                            }
                            if ((FreeQty - FreeReturn) > 0)
                            {
                                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.FREE].ReadOnly = false;
                            }
                            if ((Qty - Return) > 0)
                            {
                                GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].ReadOnly = false;
                            }
                        }
                    }
                }

            }
        }
        private void GridViewPurchaseItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewPurchaseReturnItem.Enabled)
            {
                if (e.ColumnIndex == (int)PurchaseReturnItemTableColumn.REMOVE)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_DeleteConfirmText, GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.SNO].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewPurchaseItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        if (GridViewPurchaseReturnItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value != null)
                        {
                            for (int i = 0; i <= GridViewPurchaseReturnItem.Rows.Count - 1; i++)
                            {
                                if (GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewPurchaseItem.Rows[e.RowIndex].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value.ToString())
                                {
                                    GridViewPurchaseReturnItem.Rows[i].Cells[(int)PurchaseReturnTableColumn.CHOOSE].Value = false;
                                    break;
                                }
                            }
                        }
                        GridViewPurchaseItem.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();
                    }
                }

            }
        }
        public double Oprice = 0.00;
        public double Price = 0.00;
        private void GridViewPurchaseItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)PurchaseReturnItemTableColumn.QTY ||
               e.ColumnIndex == (int)PurchaseReturnItemTableColumn.FREE ||
               e.ColumnIndex == (int)PurchaseReturnItemTableColumn.RETURNFEEP ||
               e.ColumnIndex == (int)PurchaseReturnItemTableColumn.DISP)
            {
                Row_Added();
            }

        }
        bool IsOverrideTabCtr = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnPurchaseReturnNew.PerformClick();
            }
            else if (keyData == (Keys.F4))
            {
                BtnPurchaseReturnDelete.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnPurchaseReturnEdit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnPurchaseReturnSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPurchaseReturnCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnPurchaseReturnExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnPurchaseReturnPrint.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewPurchaseReturnItem.CurrentRow.Index > -1)
                    {
                        int Index = GridViewPurchaseReturnItem.CurrentCell.RowIndex;

                        if (GridViewPurchaseReturnItem.CurrentCell.RowIndex != GridViewPurchaseReturnItem.Rows.Count - 1)
                        {
                            GridViewPurchaseReturnItem.BeginInvoke(new MethodInvoker(delegate ()
                            {
                                GridViewPurchaseReturnItem.CurrentCell = GridViewPurchaseReturnItem[(int)PurchaseReturnTableColumn.CHOOSE, Index + 1];

                            }));

                        }
                        else
                        {
                            if (GridViewPurchaseReturnItem.CurrentCell.ColumnIndex == (int)PurchaseReturnTableColumn.CHOOSE)
                            {
                                SendKeys.Send("{+tab}");
                            }
                            YesNoRbtPurchaseMethod.Focus();
                        }


                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseReturnItem.CurrentRow.Index > -1)
                    {
                        if (GridViewPurchaseReturnItem.CurrentRow.Index != 0)
                        {
                            GridViewPurchaseReturnItem.CurrentCell = GridViewPurchaseReturnItem[(int)PurchaseReturnTableColumn.CHOOSE, GridViewPurchaseReturnItem.CurrentCell.RowIndex];
                        }
                        else
                        {
                            if (BtnPurchaseReturnSave.Enabled) { BtnPurchaseReturnSave.Select(); }
                            else
                            { TextBoxPurchaseSearch.Select(); }

                        }
                    }
                }
                if (GridViewPurchaseItem.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseReturnItemTableColumn.FREE)
                    {
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseReturnItemTableColumn.RETURNFEEP))
                    {
                        IsOverrideTabCtr = false;
                        SendKeys.Send("{tab}{tab}{tab}");
                    }
                    if (keyData == (Keys.Tab) && GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseReturnItemTableColumn.RETURNFEEP)
                    {
                        IsOverrideTabCtr = true;
                        if (GridViewPurchaseItem.CurrentCell.RowIndex != GridViewPurchaseItem.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && (GridViewPurchaseItem.CurrentCell.ColumnIndex == (int)PurchaseReturnItemTableColumn.QTY))
                    {
                        if (GridViewPurchaseItem.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}{tab}");
                        }
                        else
                        {
                            DatetimePickerPurchaseReturnDate.Focus();
                        }
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
        private void GridViewPurchaseItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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
        private bool GridFocus = false;
        private void GridViewPurchaseItem_Enter(object sender, EventArgs e)
        {
            if (GridViewPurchaseReturnItem.Rows.Count > 0)
            {
                GridViewPurchaseReturnItem.CurrentCell = GridViewPurchaseReturnItem[(int)PurchaseReturnTableColumn.CHOOSE, 0];
            }
            GridFocus = true;
        }

        private void GridViewPurchaseItem_Leave(object sender, EventArgs e)
        {
            if (GridViewPurchaseItem.Rows.Count > 0) GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.UOM, GridViewPurchaseItem.CurrentRow.Index];
        }

        private void DiscountAdditinalChargeGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPurchaseReturnSave.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPurchaseItem.Rows.Count == 0)
                {
                    DatetimePickerPurchaseReturnDate.Focus();
                }
                else
                {
                    GridViewPurchaseItem.Select();
                    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, 0];
                }

            }
        }

        private void BtnPurchaseReturnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewPurchaseReturnItem.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiscountAdditinalChargeGrid.Focus();
                return;
            }
        }

        private void DatetimePickerPurchaseReturnDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPurchaseItem.Rows.Count == 0)
                {
                    DiscountAdditinalChargeGrid.Focus();
                }
                else
                {
                    GridViewPurchaseItem.Select();
                    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, 0];
                }
            }
        }

        private void BtnPurchaseSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPurchaseReturnItem.Rows.Count == 0)
                {
                    BtnPurchaseReturnExit.Select();
                }
            }
        }
        private void RemoveProductPurchaseDetails(long DetailId)
        {
            for (int i = 0; i < GridViewPurchaseReturnItem.Rows.Count; i++)
            {
                if (((long)GridViewPurchaseItem.Rows[0].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value) == DetailId)
                {
                    GridViewPurchaseItem.Rows.RemoveAt(i);
                    return;
                }
            }
        }
        private void LoadProductPurchaseDetails(long DetailId)
        {
            GridViewPurchaseItem.Rows.Add();
            int i = GridViewPurchaseItem.Rows.Count - 1;
            PurchaseDetails lPurchaseDetail = PurchaseEntryManager.GetPurchaseDetail(DetailId);
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.SNO].Value = i + 1;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRODUCT].Value = lPurchaseDetail.Product.Name;
            if (lPurchaseDetail.isBatch)
            {
                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value = lPurchaseDetail.BatchNo;
                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.EXPDATE].Value = lPurchaseDetail.ExpDate;
            }
            double Return = (double)GridViewPurchaseReturnItem.CurrentRow.Cells[(int)PurchaseReturnTableColumn.RETURN].Value;
            double FreeReturn = (double)GridViewPurchaseReturnItem.CurrentRow.Cells[(int)PurchaseReturnTableColumn.FREERETURN].Value;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.UOM].Value = lPurchaseDetail.Product.WholesaleUOM;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value = (lPurchaseDetail.Quantity - (Return - FreeReturn));
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value = (lPurchaseDetail.FreeQuantity - FreeReturn);
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value = lPurchaseDetail.PurchasePrice;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].Value = /*lPurchaseDetail.ReturnFee*/0.00;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lPurchaseDetail.TaxDetails);
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value = (lPurchaseDetail.Discounts.Count > 0) ? lPurchaseDetail.Discounts.First().Discount : 0.00;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value = lPurchaseDetail.Amount;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value = lPurchaseDetail.ProductId;
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ISBAT].Value = lPurchaseDetail.isBatch;


            //for tax
            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value = lPurchaseDetail.Id;
            if (!string.IsNullOrEmpty(TextBoxPurchaseReturnId.Text))
            {
                PurchaseDetails llPurchaseDetail = PurchaseEntryManager.GetReturnPurchaseDetail(DetailId);
                if (llPurchaseDetail != null)
                {
                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value = llPurchaseDetail.Id;
                }
            }

            if (lPurchaseDetail.isBatch && lPurchaseDetail.BatchNo != null)
            {
                PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry((long)lPurchaseDetail.PurchaseEntryId!);
                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetail.ProductId!, lPurchaseDetail.BatchNo, (long)PurchaseEntry.InventoryLocationId!).Id;
            }
            ReSequence(GridViewPurchaseItem);
        }
        private void LoadPurchaseReturn(long PurchasesId)
        {
            ResetForm();
            PurchaseEntry PurchaseEntry = PurchaseEntryManager.GetPurchaseEntry(PurchasesId);
            if (PurchaseEntry != null)
            {
                SearchPurchaseId = (long)PurchaseEntry.PurchaseEntryId!;
                LoadPurchaseEntry((long)PurchaseEntry.PurchaseEntryId);
                LabelPurchaseReturnReferenceNumber.Text = PurchaseEntry.RefNumber;
                TextBoxPurchaseReturnId.Text = PurchaseEntry.Id.ToString();
                DatetimePickerPurchaseReturnDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.ReturnDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                DatetimePickerPurchaseDate.Date = (DateTime)DateUtils.ToDate(PurchaseEntry.RefDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                YesNoRbtPurchaseMethod.Checked = (PurchaseEntry.PurchaseMethod == PurchaseMethod.Credit) ? true : false;
                if (PurchaseEntry.PurchaseDetails.Count > 0)
                {
                    GridViewPurchaseItem.Rows.Add(PurchaseEntry.PurchaseDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var PurchaseDetail in PurchaseEntry.PurchaseDetails)
                    {
                        PurchaseDetails lPurchaseDetail = PurchaseEntryManager.GetPurchaseDetail(PurchaseDetail.Id);
                        foreach (DataGridViewRow Row in GridViewPurchaseReturnItem.Rows)
                        {
                            if (Row.Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value.ToString() == PurchaseDetail.PurchaseDetailsId.ToString())
                            {
                                GridViewPurchaseReturnItem.Rows[Row.Index].Cells[(int)PurchaseReturnTableColumn.CHOOSE].Value = true;

                            }
                        }
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.SNO].Value = i + 1;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRODUCT].Value = lPurchaseDetail.Product.Name;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.UOM].Value = lPurchaseDetail.Product.UOM;
                        if (lPurchaseDetail.isBatch)
                        {
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATNO].Value = lPurchaseDetail.BatchNo;
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.EXPDATE].Value = lPurchaseDetail.ExpDate;
                            InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetail.ProductId!, lPurchaseDetail.BatchNo, (long)PurchaseEntry.InventoryLocationId!);
                            if (BatchDetails != null)
                            {
                                GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.UOM].Value = BatchDetails.WholesaleUOM;
                            }
                        }
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].Value = lPurchaseDetail.Quantity;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].Value = lPurchaseDetail.FreeQuantity;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PRICE].Value = lPurchaseDetail.PurchasePrice;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.TAXP].Value = ProductTaxPercentage(lPurchaseDetail.TaxDetails);
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.DISP].Value = (lPurchaseDetail.Discounts.Count > 0) ? lPurchaseDetail.Discounts.First().Discount : 0.00;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.AMOUNT].Value = lPurchaseDetail.Amount;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value = lPurchaseDetail.ProductId;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ISBAT].Value = lPurchaseDetail.isBatch;


                        //for tax
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASEDETAILID].Value = lPurchaseDetail.Id;
                        GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value = lPurchaseDetail.PurchaseDetailsId;

                        if (lPurchaseDetail.isBatch && lPurchaseDetail.BatchNo != null)
                        {
                            GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetail.ProductId!, lPurchaseDetail.BatchNo, (long)PurchaseEntry.InventoryLocationId!).Id;
                        }
                        i++;
                    }
                    Row_Added();
                    ReSequence(GridViewPurchaseItem);

                    if (PurchaseEntry.PurchaseAdditionalTransactions.Count > 0)
                    {
                        DiscountAdditinalChargeGrid.AdditionalTransactions = PurchaseEntry.PurchaseAdditionalTransactions.ToList<AdditionalTransaction>();
                    }
                }
                EnableForm(false);
                ComputeFormTotal();
            }
        }

        private void TextBoxPurchaseSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnPurchaseSearch_Click(sender, e);
            }
        }

        private void BtnSearchReturn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RecentReturn();
            if (SearchPurchaseReturnId != 0)
            {
                if (this.formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                    if (Result == DialogResult.Yes)
                    {
                        if (ValidateForm())
                        {
                            BtnPurchaseReturnSave_Click(sender, e);
                            LoadPurchaseReturn(SearchPurchaseReturnId);
                        }
                    }
                    else if (Result == DialogResult.No)
                    {
                        LoadPurchaseReturn(SearchPurchaseReturnId);
                    }
                }
                else
                {
                    LoadPurchaseReturn(SearchPurchaseReturnId);
                }
            }
            else
            {
                //SearchPurchaseReturnId = 0L;
                //DisplaySystemError("The selected Purchase return is not available anymore");
                //return;
            }
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            DirtyFlag(false);
            BtnPurchaseReturnCancel.PerformClick();
            return;
        }
        private void RecentReturn()
        {
            ErrorMsgPurchaseReturn.Text = "";
            String SearchText = TextBoxSearchReturn.Text.Trim();
            IList<PurchaseEntry> PurchasesInfo = null!;
            if (string.IsNullOrEmpty(SearchText))
            {
                PurchasesInfo = PurchaseEntryManager.GetRecentPurchaseEntrys(Global.Company.CompanyId, PurchaseEntrytype.RETURN);
            }
            else if (TextUtils.isAmount(SearchText))
            {
                double SearchAmount = Math.Round(float.Parse(SearchText), 2);
                double Amount = (RoundOff(SearchAmount) + SearchAmount);
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryByAmount(Amount, SearchText, Global.Company.CompanyId, PurchaseEntrytype.RETURN);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat)!;
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryByDate((DateTime)Date, Global.Company.CompanyId, PurchaseEntrytype.RETURN);
            }
            else
            {
                PurchasesInfo = PurchaseEntryManager.GetPurchaseEntryBySupplierName(SearchText, Global.Company.CompanyId, PurchaseEntrytype.RETURN);
            }

            if (PurchasesInfo.Count > 0)
            {
                LoadPurchaseReturn(PurchasesInfo);
            }
            else
            {
                SearchPurchaseReturnId = 0L;
                ErrorMsgPurchaseReturn.Text = PurchaseReturnSearchOutput;
            }
        }
        public void LoadPurchaseReturn(IList<PurchaseEntry> PurchasesInfo)
        {
            if (PurchasesInfo.Count > 0)
            {
                SearchPurchaseReturnId = 0L;
                FormRecentPurchase FormRecentPurchase = new FormRecentPurchase(this);
                FormRecentPurchase.Text = "Recent Purchase Return";
                FormRecentPurchase.RecentEntrytype = PurchaseEntrytype.RETURN;
                FormRecentPurchase.PurchaseEntryInfo = PurchasesInfo;
                FormRecentPurchase.ShowDialog();
            }
            else
            {
                ErrorMsgPurchaseReturn.Text = PurchaseReturnSearchOutput;
            }
        }

        private void FormPurchaseReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatetimePickerPurchaseReturnDate.Focus();
                    e.Cancel = true;
                }
            }
        }

        private void TimerPurchaseReturn_Tick(object sender, EventArgs e)
        {
            this.ErrorMsgPurchaseReturn.Visible = !this.ErrorMsgPurchaseReturn.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerPurchaseReturn.Stop();
                ErrorMsgPurchaseReturn.Visible = true;
            }
        }

        private void TextBoxSearchReturn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchReturn_Click(sender, e);
            }
        }

        private void BtnPurchaseReturnPrint_Click(object sender, EventArgs e)
        {
            if (PurchaseEntryManager.GetPurchaseEntry(long.Parse(TextBoxPurchaseReturnId.Text)) != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                PrinterSetup.PurchasePrintSetup(long.Parse(TextBoxPurchaseReturnId.Text), false);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                DisplaySystemError("Somting went wrong, please check this purchase is still valid.");
                return;
            }
        }

        private void BtnPurchaseReturnEdit_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult result = MessageBox.Show(SaveConfirmText, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnPurchaseReturnSave_Click(sender, e);
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            EnableForm(true);
            TextBoxPurchaseSearch.ReadOnly = false;
            GridViewPurchaseItem.ReadOnly = false;
            BtnPurchaseReturnSave.Enabled = true;
            BtnPurchaseReturnCancel.Enabled = true;
            BtnPurchaseReturnNew.Enabled = false;
            BtnPurchaseReturnDelete.Enabled = false;

            for (int i = 0; i < GridViewPurchaseItem.Rows.Count; i++)
            {
                if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.ID].Value != null)
                {
                    if (GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value != null)
                    {
                        foreach (DataGridViewRow row in GridViewPurchaseReturnItem.Rows)
                        {
                            if (row.Cells[(int)PurchaseReturnTableColumn.DETAIL_ID].Value.ToString() == GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.PURCHASERETURNDETAILID].Value.ToString())
                            {
                                double Qty = (double)row.Cells[(int)PurchaseReturnTableColumn.QTY].Value;
                                double FreeQty = (double)row.Cells[(int)PurchaseReturnTableColumn.FREE].Value;

                                double Return = (double)row.Cells[(int)PurchaseReturnTableColumn.RETURN].Value;
                                double FreeReturn = (double)row.Cells[(int)PurchaseReturnTableColumn.FREERETURN].Value;
                                GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, 0];
                                if ((FreeQty - FreeReturn) > 0)
                                {
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.FREE].ReadOnly = false;
                                    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.FREE, 0];
                                }
                                if (((Qty - FreeQty) - (Return - FreeReturn)) > 0)
                                {
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.QTY].ReadOnly = false;
                                    GridViewPurchaseItem.CurrentCell = GridViewPurchaseItem[(int)PurchaseReturnItemTableColumn.QTY, 0];
                                }
                                if ((Qty - Return) > 0)
                                {
                                    GridViewPurchaseItem.Rows[i].Cells[(int)PurchaseReturnItemTableColumn.RETURNFEEP].ReadOnly = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
