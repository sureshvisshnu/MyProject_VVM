using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.purchase;
using fa.views.utils.Inventory;
using Fa.api.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace fa.views.inventory
{
    public partial class FormAdjustInventory : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteConfirmText = "Do you want to delete Stock Adjustment Entry {0}?";
        public static string DeleteErrorText = "Error in Stock Adjustment Deleting. !";
        public static string NotAllowDeleteErrorText = "Do not Delete Stock Adjustment it used in payment";
        public static string DeleteValidationErrorText = "Do Not Delete Stock Adjustment Entry Some of the Items are Sold out";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string SelectLocationErrorMsg = "Please select stock location.";
        public static string EnterStockAdjustmentEntryDateErrorMsg = "Please enter Stock Adjustment date.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter Stock Adjustment Items details.";
        public static string Grid_RowDeleteErrorMsg = "Do Not Delete Row {0} Item Its Sold out";
        public static string Grid_RowEditErrorMsg = "Do Not Modify Row {0} Item Its Sold out";
        public static string Grid_ItemInvalidQuantityErrorMsg = "please Enter valid Quantity";
        public static string InvalidStockAdjustmentEntryErrorMsg = "Invalid Stock Adjustment.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Stock Adjustment date or Stock Adjustment reference number.";
        public static string StockAdjustmentSearchOutput = "No Stock Adjustment found.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        StockMovementManager StockMovementManager = null;
        public long ProductId = 0L;
        public long BatchId;
        public long SearchStockAdjustmentId = 0L;
        public long FromLocationId = 0L;
        public FormAdjustInventory()
        {
            InitializeComponent();
            StockMovementManager = StockMovementManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "StockAdjustmentProductDetails" };
        }

        private void FormAdjustInventory_Load(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Visible = false;
                setSize();
                this.Visible = true;
                ResetForm();
                EnableForm(true);
                DatetimePickerStockAdjustmentDate.Focus();
                DirtyFlag(false);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
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
        private StockMovementAdjustment GetStockAdjustmentEntryFromForm()
        {
            StockMovementAdjustment lStockMovementAdjustment = new StockMovementAdjustment();
            lStockMovementAdjustment.Id = TextBoxStockAdjustmentId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxStockAdjustmentId.Text);
            lStockMovementAdjustment.RefNumber = StockAdjustmentReferenceNumber.Text;
            lStockMovementAdjustment.MovementDate = (DateTime)DatetimePickerStockAdjustmentDate.Date;
            lStockMovementAdjustment.CompanyId = Global.Company.CompanyId;
            InventoryLocation InventoryLocation = ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]);
            if (InventoryLocation != null)
            {
                lStockMovementAdjustment.InventoryStockLocationId = InventoryLocation.Id;
                InventoryLocation = null;
            }
            if (Global.CostCenter != null)
            {
                lStockMovementAdjustment.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < GridViewStockAdjustmentItem.Rows.Count - 1; i++)
            {
                StockMovementDetail StockMovementDetail = new StockMovementDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    StockMovementDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        StockMovementDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    StockMovementDetail.Uom = GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value.ToString();
                    StockMovementDetail.Id = (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value == null) ? 0L : long.Parse(GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value.ToString());
                    StockMovementDetail.ProductId = Product.Id;
                    StockMovementDetail.MaterialId = Product.MaterialId;
                    StockMovementDetail.isBatch = (bool)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value;
                    //FOR SQL
                    StockMovementDetail.ExpDate = Global.getTransactionDate();
                    if (StockMovementDetail.isBatch)
                    {
                        StockMovementDetail.BatchNo = GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value.ToString();
                        StockMovementDetail.ExpDate = (DateTime)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.EXPDATE].Value;
                    }
                    double Quantity = (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString());
                    double FreeQuantity = (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value.ToString());
                    if (FreeQuantity > 0)
                    {
                        StockMovementDetail.isFree = true;
                    }
                    else
                    {
                        StockMovementDetail.isFree = false;
                    }
                    StockMovementDetail.Quantity = Quantity;
                    StockMovementDetail.FreeQuantity = FreeQuantity;
                    lStockMovementAdjustment.StockMovementDetails.Add(StockMovementDetail);
                }
            }
            return lStockMovementAdjustment;
        }

        private void BtnStockAdjustmentNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnStockAdjustmentSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    DatetimePickerStockAdjustmentDate.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DatetimePickerStockAdjustmentDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockAdjustmentDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxStockAdjustmentId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this stock adjustment is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, StockAdjustmentReferenceNumber.Text), "Delete Confirm",
         MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long ID = long.Parse(TextBoxStockAdjustmentId.Text);
                StockMovement StockMovementAdjustment = StockMovementManager.GetStockMovementById(ID);
                if (StockMovementAdjustment != null)
                {
                    if (ValidateRemoveStockAdjustment(ID))
                    {
                        bool DeleteResult = StockMovementManager.DeleteStockAdjustment(ID);
                        if (DeleteResult)
                        {
                            ResetForm();
                            EnableForm(true);
                            DatetimePickerStockAdjustmentDate.Focus();
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
                    DisplaySystemErrorPerformCancel("Somthing went wrong, the selected Purchase is not valid.");
                    return;
                }
                Cursor.Current = Cursors.Default;

            }
        }
        private bool ValidateRemoveStockAdjustment(long StockMovementID)
        {
            StockAdjustmentErrorMsg.Text = string.Empty;
            if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
            {
                IList<StockMovementDetail> StockMovementDetails = StockMovementManager.Instance.GetStockMovementById(StockMovementID).StockMovementDetails.ToList();
                foreach (StockMovementDetail Details in StockMovementDetails)
                {
                    double OldQty = Details.Quantity;
                    if (Details.isBatch)
                    {
                        InventoryLocation Location = ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]);
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)Details.ProductId, Details.BatchNo, Location.Id);
                        if (InventoryBatch != null)
                        {
                            if (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockAdjustmentDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) < OldQty)
                            {
                                StockAdjustmentErrorMsg.Text = DeleteValidationErrorText;
                                ResetTimmer();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId((long)Details.ProductId);
                        if (Inventory != null)
                        {
                            if (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockAdjustmentDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) < OldQty)
                            {
                                StockAdjustmentErrorMsg.Text = DeleteValidationErrorText;
                                ResetTimmer();
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }

        private void BtnStockAdjustmentCancel_Click(object sender, EventArgs e)
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
            DatetimePickerStockAdjustmentDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockAdjustmentSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    StockMovementAdjustment lStockMovementAdjustment = GetStockAdjustmentEntryFromForm();
                    StockMovementAdjustment lStockAdjustmentFromDB = null;
                    if (lStockMovementAdjustment.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.STOCK_ADJUSTMENT, (DateTime)DatetimePickerStockAdjustmentDate.Date);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lStockMovementAdjustment.RefNumber = RefNumber;
                            lStockAdjustmentFromDB = new StockMovementAdjustment();
                            try
                            {
                                lStockAdjustmentFromDB = StockMovementManager.Instance.AddStockMovementAdjustment(lStockMovementAdjustment);
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
                        }
                    }
                    else
                    {
                        StockMovement StockAdjustmentInfo = StockMovementManager.Instance.GetStockMovementById(lStockMovementAdjustment.Id);
                        if (StockAdjustmentInfo != null)
                        {

                            lStockAdjustmentFromDB = new StockMovementAdjustment();
                            lStockAdjustmentFromDB = StockMovementManager.Instance.UpdateStockMovementAdjustment(lStockMovementAdjustment);
                        }
                        else
                        {
                            DisplaySystemErrorPerformCancel("Somthing went wrong, the selected stock adjustment is not valid.");
                            return;
                        }
                    }

                    TextBoxStockAdjustmentId.Text = lStockAdjustmentFromDB.Id.ToString();
                    StockAdjustmentReferenceNumber.Text = lStockAdjustmentFromDB.RefNumber;

                    if (lStockAdjustmentFromDB != null)
                    {
                        EnableForm(false);
                        LoadStockAdjustmentEntry(lStockAdjustmentFromDB.Id);
                        BtnStockAdjustmentPrint.Select();
                        StockAdjustmentErrorMsg.Text = SaveSuccessText;
                    }
                    DirtyFlag(false);

                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void BtnStockAdjustmentExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
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
            BtnStockAdjustmentCancel.PerformClick();
            return;
        }
        private void FormAdjustInventory_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatetimePickerStockAdjustmentDate.Focus();
                    e.Cancel = true;
                }
            }
        }
        private Boolean ValidateForm()
        {
            StockAdjustmentErrorMsg.Text = "";

            if (DatetimePickerStockAdjustmentDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerStockAdjustmentDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerStockAdjustmentDate.Focus();
                StockAdjustmentErrorMsg.Text = EnterStockAdjustmentEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxLocation.SelectedIndex < 0)
            {
                ComboBoxLocation.Select();
                StockAdjustmentErrorMsg.Text = SelectLocationErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewStockAdjustmentItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value == null)
                    {
                        GridViewStockAdjustmentItem.Select();
                        GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, i];
                        GridViewStockAdjustmentItem.BeginEdit(true);
                        StockAdjustmentErrorMsg.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value == null
                        || string.IsNullOrEmpty(GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value.ToString())
                        )
                    {
                        GridViewStockAdjustmentItem.Select();
                        GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.UOM, i];
                        GridViewStockAdjustmentItem.BeginEdit(true);
                        StockAdjustmentErrorMsg.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewStockAdjustmentItem.Columns[(int)StockMovementTableColumn.UOM].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString());
                    for (int j = 3; j < 7; j++)
                    {
                        if ((j == (int)StockMovementTableColumn.QTY) && !(Quantity <= 0)) { continue; }
                        if ((!((bool)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value)) && (j == (int)StockMovementTableColumn.EXPDATE || j == (int)StockMovementTableColumn.BATNO)) { continue; }
                        if (j == (int)StockMovementTableColumn.FREE) { continue; }
                        if (j != 5 && j != 6 && (GridViewStockAdjustmentItem.Rows[i].Cells[j].Value == null || GridViewStockAdjustmentItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewStockAdjustmentItem.Rows[i].Cells[j].Value.ToString()) == 0))
                        {
                            GridViewStockAdjustmentItem.Select();
                            GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[j, i];
                            GridViewStockAdjustmentItem.BeginEdit(true);
                            StockAdjustmentErrorMsg.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewStockAdjustmentItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.BATCHID].Value == null)
                        {
                            GridViewStockAdjustmentItem.Select();
                            GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[j, i];
                            GridViewStockAdjustmentItem.BeginEdit(true);
                            StockAdjustmentErrorMsg.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewStockAdjustmentItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }

                    }
                    long PId = (long)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value;
                    Inventory Inventory = null;
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed && Quantity < 0)
                    {
                        //for from location
                        double NewQty = Quantity;
                        double OldQty = 0;
                        string Uom = GridViewStockAdjustmentItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.UOM].Value.ToString();
                        Product Product = CatalogProductManager.Instance.GetProductInfoById(PId);
                        if (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value != null)
                        {
                            long PDetailId = (long)GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value;
                            StockMovementDetail StockMovementDetails = StockMovementManager.Instance.GetStockMovementDetail(PDetailId);
                            if (StockMovementDetails != null)
                            {
                                OldQty = StockMovementDetails.Quantity;
                                if (StockMovementDetails.Uom != Uom)
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

                        double Stock;
                        if ((bool)GridViewStockAdjustmentItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value)
                        {
                            long BId = (long)GridViewStockAdjustmentItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.BATCHID].Value;
                            Stock = StockByXfactorBatch(BId, Uom);
                            if (OldQty > NewQty && Math.Round(Stock, MidpointRounding.AwayFromZero) < (Math.Round((OldQty - NewQty), MidpointRounding.AwayFromZero)))
                            {
                                GridViewStockAdjustmentItem.Select();
                                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)PurchaseEntryTableColumn.QTY, i];
                                GridViewStockAdjustmentItem.BeginEdit(true);
                                StockAdjustmentErrorMsg.Text = "Please Enter valid Quantity, Available Quantity is " + (StockByXfactorBatch(BId, Uom) - OldQty);
                                ResetTimmer();
                                return false;
                            }
                        }
                        else
                        {
                            Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, FromLocationId);
                            if (Inventory == null)
                            {
                                GridViewStockAdjustmentItem.Select();
                                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)PurchaseEntryTableColumn.PRODUCT, i];
                                GridViewStockAdjustmentItem.BeginEdit(true);
                                StockAdjustmentErrorMsg.Text = "No stock Available";
                                ResetTimmer();
                                return false;
                            }
                            else if (OldQty > NewQty && (StockByXfactor(PId, Uom) < (OldQty - NewQty)))
                            {
                                GridViewStockAdjustmentItem.Select();
                                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)PurchaseEntryTableColumn.QTY, i];
                                GridViewStockAdjustmentItem.BeginEdit(true);
                                StockAdjustmentErrorMsg.Text = "Please Enter valid Quantity, Available Quantity is " + (StockByXfactor(PId, Uom) - OldQty);
                                ResetTimmer();
                                return false;
                            }
                        }
                    }
                    bool isBatch = (bool)GridViewStockAdjustmentItem.Rows[i].Cells[(int)PurchaseEntryTableColumn.ISBAT].Value;
                    Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, FromLocationId);
                    if (Inventory == null)
                    {
                        if (!isBatch && FromLocationId != 0L)
                        {
                            Inventory lInventory = new Inventory();
                            lInventory.OpeningStock = 0;
                            lInventory.CompanyId = Global.Company.CompanyId;
                            lInventory.ProductId = PId;
                            lInventory.InventoryLocationId = FromLocationId;
                            InventoryLocationManager.Instance.AddInventory(lInventory);
                        }
                        else
                        {
                            StockAdjustmentErrorMsg.Text = "Batch not available.";
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                GridViewStockAdjustmentItem.Select();
                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, 0];
                GridViewStockAdjustmentItem.BeginEdit(true);
                StockAdjustmentErrorMsg.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            return true;
        }
        private double StockByXfactorBatch(long BatchId, string Uom)
        {
            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatchId, FromLocationId);
            if (InventoryBatch != null)
            {
                return (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockAdjustmentDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) * (InventoryBatch.RetailUOM == Uom ? (InventoryBatch.WholesaleXFactor * InventoryBatch.RetailXFactor) : InventoryBatch.WholesaleXFactor));
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
                    IList<InventoryBatch> InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchbyProductId(ProductId, FromLocationId);
                    if (InventoryBatch.Count > 0)
                    {
                        double TotalStock = 0.00;
                        foreach (InventoryBatch Detail in InventoryBatch)
                        {
                            TotalStock += (((Detail.StockDate != null && Detail.StockDate <= DatetimePickerStockAdjustmentDate.Date ? Detail.OpeningStock : 0) + Detail.QuantityOnHand) * (Product.RetailUOM == Uom ? (Detail.WholesaleXFactor * Detail.RetailXFactor) : Detail.WholesaleXFactor));
                        }
                        return TotalStock;
                    }
                }
                else
                {
                    Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(ProductId, FromLocationId);
                    if (Inventory != null)
                    {
                        double TotalStock = (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockAdjustmentDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) * (Product.RetailUOM == Uom ? (Product.WholesaleXFactor * Product.RetailXFactor) : Product.WholesaleXFactor));
                        return TotalStock;
                    }
                }
            }
            return 0;
        }


        private void ResetForm()
        {
            StockAdjustmentReferenceNumber.Text = "000000";
            TextBoxStockAdjustmentSearch.TextBox.ResetText();
            StockAdjustmentErrorMsg.Text = "";
            TextBoxStockAdjustmentId.ResetText();
            DatetimePickerStockAdjustmentDate.Format = Global.Company.DateFormat;
            DatetimePickerStockAdjustmentDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            GridViewStockAdjustmentItem.Rows.Clear();
            GridViewStockAdjustmentItem.Rows.Add();
            ComboBoxLocation.ResetText();
            ComboUtils.InitializeStockLocationCombo(ComboBoxLocation, Global.Company.CompanyId);
            ComboBoxLocation.SelectedIndex = -1;
            LastStockAdjustmentReferenceNumber.Text = CompanyManager.Instance.GetMovementPrevRef(Global.Company, InventoryJournalType.ADJUSTMENT, (DateTime)DatetimePickerStockAdjustmentDate.Date);
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxLocation.Visible = enable;
            BtnStockAdjustmentDelete.Enabled = true;
            BtnStockAdjustmentSave.Enabled = true;
            DatetimePickerStockAdjustmentDate.ReadOnly = !enable;
            DatetimePickerStockAdjustmentDate.TabStop = enable;
            GridViewStockAdjustmentItem.ReadOnly = false;
            GridViewStockAdjustmentItem.TabStop = true;
            GridViewStockAdjustmentItem.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnStockAdjustmentNew.Enabled = !enable;
                BtnStockAdjustmentDelete.Enabled = !enable;
                BtnStockAdjustmentPrint.Enabled = !enable;
                BtnStockAdjustmentCancel.Enabled = enable;
                BtnStockAdjustmentSave.Enabled = enable;
            }
            else
            {
                BtnStockAdjustmentNew.Enabled = !enable;
                BtnStockAdjustmentDelete.Enabled = !enable;
                BtnStockAdjustmentPrint.Enabled = !enable;
                BtnStockAdjustmentCancel.Enabled = !enable;
                BtnStockAdjustmentSave.Enabled = !enable;
            }

            if (!string.IsNullOrEmpty(TextBoxStockAdjustmentId.Text))
            {
                if (StockMovementManager.IsStockOutReceived(long.Parse(TextBoxStockAdjustmentId.Text)))
                {
                    GridViewStockAdjustmentItem.ReadOnly = !enable;
                    BtnStockAdjustmentDelete.Enabled = enable;
                    BtnStockAdjustmentSave.Enabled = enable;
                    BtnStockAdjustmentCancel.Enabled = enable;

                }
            }

        }
        private void LoadStockAdjustmentEntry(long StockAdjustmentId)
        {
            ResetForm();
            StockMovement StockMovementAdjustment = StockMovementManager.Instance.GetStockMovementById(StockAdjustmentId);
            if (StockMovementAdjustment != null)
            {
                StockAdjustmentReferenceNumber.Text = StockMovementAdjustment.RefNumber;
                TextBoxStockAdjustmentId.Text = StockMovementAdjustment.Id.ToString();
                DatetimePickerStockAdjustmentDate.Date = (DateTime)DateUtils.ToDate(StockMovementAdjustment.MovementDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);

                InventoryLocation InventoryLocation = null;
                if (StockMovementAdjustment.InventoryStockLocationId != 0L)
                {
                    InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(StockMovementAdjustment.InventoryStockLocationId);
                    ComboBoxLocation.SelectedIndex = ComboBoxLocation.FindStringExact(InventoryLocation.Name);
                    InventoryLocation = null;
                }
                if (StockMovementAdjustment.StockMovementDetails.Count > 0)
                {
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var StockMovementDetails in StockMovementAdjustment.StockMovementDetails)
                    {
                        GridViewStockAdjustmentItem.Rows.Add();
                        StockMovementDetail lStockMovementDetails = StockMovementManager.GetStockMovementDetail(StockMovementDetails.Id);
                        if (lStockMovementDetails != null)
                        {
                            (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                            (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.RetailUOM);
                            (GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.WholesaleUOM);
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.SNO].Value = i + 1;
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.PRODUCT].Value = lStockMovementDetails.Product.Name;
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value = lStockMovementDetails.Product.RetailUOM == lStockMovementDetails.Uom ? lStockMovementDetails.Product.RetailUOM : lStockMovementDetails.Product.WholesaleUOM; ;
                            if (lStockMovementDetails.isBatch)
                            {
                                GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value = lStockMovementDetails.BatchNo;
                                GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.EXPDATE].Value = lStockMovementDetails.ExpDate;
                            }
                            if (lStockMovementDetails.isBatch && lStockMovementDetails.BatchNo != null)
                            {
                                GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lStockMovementDetails.ProductId, lStockMovementDetails.BatchNo, StockMovementAdjustment.InventoryStockLocationId).Id;
                            }
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value = lStockMovementDetails.Quantity;
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value = lStockMovementDetails.FreeQuantity;
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value = lStockMovementDetails.ProductId;
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value = lStockMovementDetails.isBatch;
                            //for tax
                            GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value = lStockMovementDetails.Id;

                            i++;
                        }

                    }
                    ReSequence();
                }
                EnableForm(false);
                GridViewStockAdjustmentItem.Select();
                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, 0];
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("Something went wrong,please check purchase");
            }
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewStockAdjustmentItem.Rows.Count; i++)
            {
                GridViewStockAdjustmentItem.Rows[i].Cells[(int)StockMovementTableColumn.SNO].Value = i + 1;
            }
        }

        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerStock.Stop();
            TimerStock.Start();
        }
        private void TimerStock_Click(object sender, EventArgs e)
        {
            this.StockAdjustmentErrorMsg.Visible = !this.StockAdjustmentErrorMsg.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerStock.Stop();
                StockAdjustmentErrorMsg.Visible = true;
            }
        }

        private void GridViewStockAdjustmentItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)StockMovementTableColumn.BATNO && GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
            {
                LoadBatchDetails((long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value);
            }
        }

        private void GridViewStockAdjustmentItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewStockAdjustmentItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                DirtyFlag(IsDirty);
            }
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.UOM].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.FREE].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.EXPDATE].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRICE].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.COST].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.TAXP].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.TAX].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DISP].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DIS].ReadOnly = true;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.UOM].ReadOnly = false;
                GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].ReadOnly = false;
                GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.FREE].ReadOnly = false;
                GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRICE].ReadOnly = false;
                GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DISP].ReadOnly = false;
                if ((bool)GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                {
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed && !ValidateLineItemModifiedOrRemove(e.RowIndex))
                    {
                        GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = true;
                    }
                    else
                    {
                        GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = false;
                        GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRODUCT].ReadOnly = false;
                    }
                }

            }
        }

        private void GridViewStockAdjustmentItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                StockAdjustmentErrorMsg.Text = string.Empty;
                if (e.ColumnIndex == (int)StockMovementTableColumn.REMOVE && (GridViewStockAdjustmentItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value.ToString()), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {

                        if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                        {
                            StockAdjustmentErrorMsg.Text = string.Format(Grid_RowDeleteErrorMsg, GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value.ToString());
                            ResetTimmer();
                            return;
                        }
                        GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockAdjustmentItem.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        private bool ValidateLineItemModifiedOrRemove(int Index)
        {
            if (GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.DETAILID].Value != null
                            && GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                long PDetailId = (long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.DETAILID].Value;
                long PId = (long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value;
                StockMovementDetail StockMovementDetails = StockMovementManager.Instance.GetStockMovementDetail(PDetailId);
                if (StockMovementDetails != null)
                {
                    double OldQty = StockMovementDetails.Quantity;

                    if ((bool)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                    {
                        String BNo = (string)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATNO].Value;
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(PId, BNo, FromLocationId);
                        if (InventoryBatch != null)
                        {
                            if (InventoryBatch != null && (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockAdjustmentDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) < OldQty))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId);
                        if (Inventory != null)
                        {
                            if (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockAdjustmentDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) < OldQty)
                            {
                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        }

        private void GridViewStockAdjustmentItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
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
                    int Index = GridViewStockAdjustmentItem.CurrentCell.RowIndex;
                    GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }

        }
        private void UomColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewStockAdjustmentItem.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.UOM].Value = ((ComboBox)sender).Text;
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.UOM].Value == null)
                {
                    GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
                }
            }

        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    StockAdjustmentProductDetails.LocationId = FromLocationId;
                    if ((bool)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                    {
                        if (GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value, FromLocationId);
                            if (InventoryBatch != null)
                            {
                                GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice;
                                StockAdjustmentProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                            StockAdjustmentProductDetails.ProductId = Product.Id;
                        }
                    }
                    else
                    {
                        GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                        StockAdjustmentProductDetails.ProductId = Product.Id;
                    }
                }
            }
        }
        private void GridViewStockAdjustmentItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewStockAdjustmentItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }

                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged);

                ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged;
                ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged;

                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockAdjustmentItem_KeyPress1);
            }
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockAdjustmentItem, "TaxDetailsNumberChecking", GridViewStockAdjustmentItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockAdjustmentItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockAdjustmentItem_KeyDown;
            }
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockAdjustmentItem, "NameCheckingProduct", GridViewStockAdjustmentItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockAdjustmentItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockAdjustmentItem_KeyDown;
            }
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange;
                ((TextBox)e.Control).TextChanged += ProductTextChange;
            }
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= BatchTextChange;
                ((TextBox)e.Control).TextChanged += BatchTextChange;
            }
        }
        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex != (int)StockMovementTableColumn.BATNO)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    IList<Product> Product = CatalogProductManager.Instance.GetProductByExactSearchQuery(((TextBox)sender).Text, Global.Company.CompanyId);
                    if (Product.Count > 0)
                    {
                        if (ComboBoxLocation.SelectedIndex > -1)
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
                            long CheckForAddRow = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
                            LoadUomTax(Product.First().Id);
                            GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            if (CheckForAddRow == 0 && GridViewStockAdjustmentItem.Rows.Count - 1 == GridViewStockAdjustmentItem.CurrentRow.Index)
                            {
                                GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                GridViewStockAdjustmentItem.Rows.Add();
                            }
                        }
                        else
                        {
                            MessageBox.Show("please select stock from location.", "Warning");
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewStockAdjustmentItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewStockAdjustmentItem.CurrentRow.Index);
                }
            }
        }
        private void BatchTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified)
            {
                long PId = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
                StockAdjustmentProductDetails.LocationId = FromLocationId;

                if (!string.IsNullOrEmpty(((TextBox)sender).Text) && GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    InventoryLocation Location = ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]);
                    InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value, ((TextBox)sender).Text, Location.Id);
                    if (BatchDetails != null)
                    {
                        LoadBatchDetails(BatchDetails.Id);
                        StockAdjustmentProductDetails.BatchId = BatchDetails.Id;
                    }
                    else
                    {
                        ResetBatchDetails();
                        StockAdjustmentProductDetails.ProductId = PId;

                    }
                    GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetBatchDetails();
                    if (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null)
                    {
                        StockAdjustmentProductDetails.ProductId = (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value;
                    }
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.PRODUCT].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.QTY].Value = 0;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.FREE].Value = 0;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.COST].Value = 0.00;

            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = 0.00;

            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.TAXP].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DISP].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.AMOUNT].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
            //for tax update purchase
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
            StockAdjustmentProductDetails.Clear();

        }
        private void GridViewStockAdjustmentItem_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewStockAdjustmentItem.EditingControl).DroppedDown = false;

        }
        private void GridViewStockAdjustmentItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
            }
            if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
            }
        }

        private void GridViewStockAdjustmentItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
                if (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
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
                int index = GridViewStockAdjustmentItem.CurrentRow.Index;
                (GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                (GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                (GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.PRODUCT].Value = Product.Name;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.QTY].Value = 0;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.FREE].Value = 0;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = null;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = Product.PurchasePrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.COST].Value = 0.00;

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = Product.RetailPrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = Product.Msrp;

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = Product.RetailUOM;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = Product.RetailXFactor;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = Product.WholesaleUOM;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = Product.WholesaleXFactor;

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.TAX].Value = 0.00;
                //for tax update purchase
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DIS].Value = 0.00;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.AMOUNT].Value = 0.00;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null)
                {
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
                }

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
            if (ComboBoxLocation.SelectedIndex > -1)
            {
                Cursor.Current = Cursors.WaitCursor;
                bool IsDirty = this.formIsDirty;
                long Check = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
                ProductId = 0L;
                FormSearchItems FormSearchItems = new FormSearchItems(this);
                GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                FormSearchItems.SearchText = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value != null) ? GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value.ToString() : null;
                FormSearchItems.LocationId = FromLocationId;
                FormSearchItems.ShowDialog();
                if (ProductId != 0L)
                {
                    Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                    if (Product != null)
                    {
                        LoadUomTax(ProductId);
                        LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId));

                        if (Check == 0 && GridViewStockAdjustmentItem.Rows.Count - 1 == GridViewStockAdjustmentItem.CurrentRow.Index)
                        {
                            GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewStockAdjustmentItem.Rows.Add();
                        }
                        GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[2, GridViewStockAdjustmentItem.CurrentRow.Index];
                        GridViewStockAdjustmentItem.CurrentCell.Selected = true;
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
            else
            {
                MessageBox.Show("please select stock location.", "Warning");
            }
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
            long Check = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value != null) ? (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value : 0L;
            BatchId = 0L;
            FormSearchBatch FormSearchBatch = new FormSearchBatch(this);
            GridViewStockAdjustmentItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchBatch.ProductId = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
            FormSearchBatch.SearchText = (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value != null) ? GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value.ToString() : string.Empty;
            FormSearchBatch.LocationId = FromLocationId;
            FormSearchBatch.ShowDialog();
            if (BatchId != 0)
            {
                LoadBatchDetails(BatchId);
                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, GridViewStockAdjustmentItem.CurrentRow.Index + 1];
                GridViewStockAdjustmentItem.CurrentCell.Selected = true;
            }
            else
            {
                DirtyFlag(IsDirty);
                return;
            }
        }
        private void LoadBatchDetails(long BatId)
        {
            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
            if (InventoryBatch != null)
            {
                int index = GridViewStockAdjustmentItem.CurrentRow.Index;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.BATCHID].Value = InventoryBatch.Id;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;

                GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
                StockAdjustmentProductDetails.BatchId = InventoryBatch.Id;
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected Batch is not valid.");
                return;
            }
        }
        private void ResetBatchDetails()
        {
            int index = GridViewStockAdjustmentItem.CurrentRow.Index;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.BATCHID].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = 0.00;

            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = 1;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = null;
            GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = 1;

            if (GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = Product.RetailPrice;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = Product.Msrp;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = Product.RetailUOM;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = Product.RetailXFactor;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = Product.WholesaleUOM;
                    GridViewStockAdjustmentItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                }
            }
        }

        private void GridViewStockAdjustmentItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value = GridViewStockAdjustmentItem.Rows.Count;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
            DirtyFlag(IsDirty);
        }

        private void GridViewStockAdjustmentItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadProductDetails(e.RowIndex);
                //for item sold out
                StockAdjustmentErrorMsg.Text = string.Empty;
                if (GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DETAILID].Value != null)
                {
                    if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                    {
                        GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRODUCT].ReadOnly = true;
                        if ((bool)GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                        {
                            GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = true;
                        }
                        StockAdjustmentErrorMsg.Text = string.Format(Grid_RowEditErrorMsg, GridViewStockAdjustmentItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value.ToString());
                    }
                }
            }
        }
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
        }
        private void LoadProductDetails(int Index)
        {
            Cursor.Current = Cursors.WaitCursor;
            StockAdjustmentProductDetails.EditableStock = 0.00;
            if (GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    LoadProductAdditinalDetails(Product);
                    if (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
                    {
                        StockAdjustmentProductDetails.BatchId = (long)GridViewStockAdjustmentItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value;
                    }
                }
            }
            else
            {
                StockAdjustmentProductDetails.Clear();
                EnableProductAdditinalDetails();
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            StockAdjustmentProductDetails.LocationId = FromLocationId;
            StockAdjustmentProductDetails.ProductId = Product.Id;
            EnableProductAdditinalDetails();
        }
        private void EnableProductAdditinalDetails()
        {
            StockAdjustmentProductDetails.ReadyOnly = true;
        }
        private void ComboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            InventoryLocation lLocation = null;
            if (ComboBoxLocation.SelectedIndex > -1)
            {
                lLocation = (InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex];
            }
            if (!(GridViewStockAdjustmentItem.Rows.Count == 1 || (Location != null && lLocation.Id == FromLocationId)))
            {
                DialogResult Result = MessageBox.Show("Items will be removed from the sale if you change the stock location", "Confirm Stock Location",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.OK)
                {
                    GridViewStockAdjustmentItem.Rows.Clear();
                    GridViewStockAdjustmentItem.Rows.Add();
                }
                else
                {
                    InventoryLocation Location = FromLocationId != 0L ? HospitalInventoryManager.Instance.GetLocationById(FromLocationId) : null;
                    if (Location != null)
                    {
                        ComboBoxLocation.SelectedIndex = ComboBoxLocation.FindStringExact(Location.Name);
                    }
                }
            }
            FromLocationId = ComboBoxLocation.SelectedIndex > -1 ? ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]).Id : 0L;
        }

        private void BtnStockAdjustmentSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                Cursor.Current = Cursors.WaitCursor;
                RecentPurchases();
                if (SearchStockAdjustmentId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnStockAdjustmentSave_Click(sender, e);
                                LoadStockAdjustmentEntry(SearchStockAdjustmentId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadStockAdjustmentEntry(SearchStockAdjustmentId);
                        }
                    }
                    else
                    {
                        LoadStockAdjustmentEntry(SearchStockAdjustmentId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private bool isValidSearchCriteria()
        {
            StockAdjustmentErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxStockAdjustmentSearch.Text.Trim()))
            {
                StockAdjustmentErrorMsg.Text = SearchBoxEmptyErrorMsg;
                TextBoxStockAdjustmentSearch.TextBox.Select();
                return true;
            }
            return true;
        }

        private void RecentPurchases()
        {
            StockAdjustmentErrorMsg.Text = "";
            String SearchText = TextBoxStockAdjustmentSearch.Text.Trim();
            IList<StockMovementAdjustment> StockMovementInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                StockMovementInfo = StockMovementManager.GetRecentStockAdjustments(Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                StockMovementInfo = StockMovementManager.GetStockAdjustmentByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                StockMovementInfo = StockMovementManager.GetStockAdjustmentByReferenceNo(SearchText, Global.Company.CompanyId);
            }

            if (StockMovementInfo.Count > 0)
            {
                LoadStockAdjustmentEntry(StockMovementInfo);
            }
            else
            {
                SearchStockAdjustmentId = 0L;
                StockAdjustmentErrorMsg.Text = StockAdjustmentSearchOutput;
            }
        }
        public void LoadStockAdjustmentEntry(IList<StockMovementAdjustment> StockMovementInfo)
        {
            if (StockMovementInfo.Count > 0)
            {
                SearchStockAdjustmentId = 0L;
                FormRecentStockMovement FormRecentStockMovement = new FormRecentStockMovement(this);
                FormRecentStockMovement.StockMovementAdjustmentInfo = StockMovementInfo;
                FormRecentStockMovement.InventoryJournalType = InventoryJournalType.ADJUSTMENT;
                FormRecentStockMovement.ShowDialog();

            }
            else
            {
                StockAdjustmentErrorMsg.Text = StockAdjustmentSearchOutput;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                if (BtnStockAdjustmentNew.Enabled)
                {
                    BtnStockAdjustmentNew.PerformClick();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnStockAdjustmentDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnStockAdjustmentPrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnStockAdjustmentSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                GridViewStockAdjustmentItem.EndEdit();
                BtnStockAdjustmentCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnStockAdjustmentExit.PerformClick();
                return true;
            }
            try
            {
                if (GridViewStockAdjustmentItem.CurrentCell != null)
                {
                    if ((keyData == Keys.F2) && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
                    {
                        SearchProduct();
                        return true;
                    }
                    if ((keyData == Keys.F2) && !GridViewStockAdjustmentItem.CurrentCell.ReadOnly && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
                    {
                        SearchBatch();
                        return true;
                    }
                    if (keyData == (Keys.Tab) && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.QTY)
                    {
                        if (GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].ReadOnly)
                        {
                            if (GridViewStockAdjustmentItem.CurrentRow.Index == GridViewStockAdjustmentItem.Rows.Count - 1)
                            {
                                BtnStockAdjustmentSave.Select();
                            }
                            else
                            {
                                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, GridViewStockAdjustmentItem.CurrentRow.Index + 1];
                                return true;
                            }
                        }
                    }
                    if (keyData == (Keys.Tab) && (GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO))
                    {
                        if (!GridViewStockAdjustmentItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].ReadOnly)
                        {

                            SendKeys.Send("{tab}{tab}{tab}");
                        }
                        else
                        {
                            if (GridViewStockAdjustmentItem.CurrentRow.Index == GridViewStockAdjustmentItem.Rows.Count - 1)
                            {
                                BtnStockAdjustmentSave.Select();
                            }
                            else
                            {
                                SendKeys.Send("{tab}{tab}");
                            }
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewStockAdjustmentItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
                    {
                        if (GridViewStockAdjustmentItem.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}");
                        }
                        else
                        {
                            if (ComboBoxLocation.Visible)
                            {
                                ComboBoxLocation.Focus();
                            }
                            else
                            {
                                BtnStockAdjustmentSave.Select();
                                return true;
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

        private void TextBoxStockAdjustmentSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnStockAdjustmentSearch_Click(sender, e);
            }
        }

        private void BtnStockAdjustmentSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DatetimePickerStockAdjustmentDate.ReadOnly)
                {
                    GridViewStockAdjustmentItem.Select();
                    GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, 0];
                }
                else
                {
                    DatetimePickerStockAdjustmentDate.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockAdjustmentItem.Select();
                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, 0];
                GridViewStockAdjustmentItem.CurrentCell.Selected = true;
            }
        }

        private void ComboBoxLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockAdjustmentItem.Select();
                GridViewStockAdjustmentItem.CurrentCell = GridViewStockAdjustmentItem[(int)StockMovementTableColumn.PRODUCT, 0];
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerStockAdjustmentDate.Select();
            }
        }

        private void DatetimePickerStockAdjustmentDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxLocation.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnStockAdjustmentSave.Select();
            }
        }

        private void BtnStockAdjustmentPrint_Click(object sender, EventArgs e)
        {
            AdjustmentEntryPrint AdjustmentEntryPrint = new AdjustmentEntryPrint();
            AdjustmentEntryPrint.PrintAdjustmentEntry(long.Parse(TextBoxStockAdjustmentId.Text), "Adjustment Entry");
        }
    }
}
