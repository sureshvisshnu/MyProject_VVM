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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.inventory
{
    public enum StockDamageTableColumn
    {
        SNO, PRODUCT, UOM, QTY, BATNO, EXPDATE, DISCRI, REMOVE, COST, PRICE, AMOUNT, ID, ISBAT, DETAILID, BATCHID, RETAIL, WHOLESALE, MSRP, RUOM, RXFACT, WUOM, WXFACT
    }
    public partial class FormDamageEntry : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteConfirmText = "Do you want to delete Stock Damage Entry {0}?";
        public static string DeleteErrorText = "Error in Stock Damage Deleting. !";
        public static string NotAllowDeleteErrorText = "Do not Delete Stock Damage it used in payment";
        public static string DeleteValidationErrorText = "Do Not Delete Stock Damage Entry Some of the Items are Sold out";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string ResetItemTaxConfirmText = "Reset product It takes latest Tax";
        public static string SelectLocationErrorMsg = "Please select stock location.";
        public static string EnterStockDamageEntryDateErrorMsg = "Please enter Stock Damage date.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter Stock Damage Items details.";
        public static string Grid_RowDeleteErrorMsg = "Do Not Delete Row {0} Item Its Sold out";
        public static string Grid_RowEditErrorMsg = "Do Not Modify Row {0} Item Its Sold out";
        public static string Grid_ItemInvalidQuantityErrorMsg = "please Enter valid Quantity";
        public static string InvalidStockDamageEntryErrorMsg = "Invalid Stock Damage.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could Stock Damage date or Stock Damage reference number.";
        public static string StockDamageSearchOutput = "No Stock Damage found.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        StockMovementManager StockMovementManager = null;
        public long ProductId = 0L;
        public long BatchId;
        public long SearchStockDamageId = 0L;
        public long FromLocationId = 0L;
        public FormDamageEntry()
        {
            InitializeComponent();
            StockMovementManager = StockMovementManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "StockDamageProductDetails" };
        }

        private void FormDamageEntry_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Visible = false;
                setSize();
                this.Visible = true;
                ResetForm();
                EnableForm(true);
                DatetimePickerStockDamageDate.Focus();
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
        private StockMovementDamaged GetStockDamageEntryFromForm()
        {
            StockMovementDamaged lStockMovementDamage = new StockMovementDamaged();
            lStockMovementDamage.Id = TextBoxStockDamageId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxStockDamageId.Text);
            lStockMovementDamage.RefNumber = StockDamageReferenceNumber.Text;
            lStockMovementDamage.MovementDate = (DateTime)DatetimePickerStockDamageDate.Date;
            lStockMovementDamage.CompanyId = Global.Company.CompanyId;
            InventoryLocation InventoryLocation = ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]);
            if (InventoryLocation != null)
            {
                lStockMovementDamage.InventoryStockLocationId = InventoryLocation.Id;
                InventoryLocation = null;
            }
            if (Global.CostCenter != null)
            {
                lStockMovementDamage.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < GridViewStockDamageItem.Rows.Count - 1; i++)
            {
                StockMovementDetail StockMovementDetail = new StockMovementDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ID].Value);
                if (Product != null)
                {
                    StockMovementDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        StockMovementDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    StockMovementDetail.Description = GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DISCRI].Value.ToString();
                    StockMovementDetail.Uom = GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM].Value.ToString();
                    StockMovementDetail.Id = (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DETAILID].Value == null) ? 0L : long.Parse(GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DETAILID].Value.ToString());
                    StockMovementDetail.ProductId = Product.Id;
                    StockMovementDetail.MaterialId = Product.MaterialId;
                    StockMovementDetail.isBatch = (bool)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ISBAT].Value;
                    //FOR SQL
                    StockMovementDetail.ExpDate = Global.getTransactionDate();
                    if (StockMovementDetail.isBatch)
                    {
                        StockMovementDetail.BatchNo = GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.BATNO].Value.ToString();
                        StockMovementDetail.ExpDate = (DateTime)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.EXPDATE].Value;
                    }
                    double Quantity = (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.QTY].Value.ToString());
                    StockMovementDetail.Quantity = Quantity;
                    lStockMovementDamage.StockMovementDetails.Add(StockMovementDetail);
                }
            }
            return lStockMovementDamage;
        }

        private void BtnStockDamageNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnStockDamageSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    DatetimePickerStockDamageDate.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            EnableForm(true);
            DatetimePickerStockDamageDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockDamageDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxStockDamageId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this stock Damage is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, StockDamageReferenceNumber.Text), "Delete Confirm",
         MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long ID = long.Parse(TextBoxStockDamageId.Text);
                StockMovement StockMovementDamage = StockMovementManager.GetStockMovementById(ID);
                if (StockMovementDamage != null)
                {
                    if (ValidateRemoveStockDamage(ID))
                    {
                        bool DeleteResult = StockMovementManager.DeleteStockDamaged(ID);
                        if (DeleteResult)
                        {
                            ResetForm();
                            EnableForm(true);
                            DatetimePickerStockDamageDate.Focus();
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
        private bool ValidateRemoveStockDamage(long StockMovementID)
        {
            StockDamageErrorMsg.Text = string.Empty;
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
                            if (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockDamageDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) < OldQty)
                            {
                                StockDamageErrorMsg.Text = DeleteValidationErrorText;
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
                            if (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockDamageDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) < OldQty)
                            {
                                StockDamageErrorMsg.Text = DeleteValidationErrorText;
                                ResetTimmer();
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }

        private void BtnStockDamageCancel_Click(object sender, EventArgs e)
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
            DatetimePickerStockDamageDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockDamageSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    StockMovementDamaged lStockMovementDamage = GetStockDamageEntryFromForm();
                    StockMovementDamaged lStockDamageFromDB = null;
                    if (lStockMovementDamage.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.STOCK_DAMAGE, (DateTime)DatetimePickerStockDamageDate.Date);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lStockMovementDamage.RefNumber = RefNumber;
                            lStockDamageFromDB = new StockMovementDamaged();
                            try
                            {
                                lStockDamageFromDB = StockMovementManager.Instance.AddStockMovementDamaged(lStockMovementDamage);
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
                        StockMovement StockDamageInfo = StockMovementManager.Instance.GetStockMovementById(lStockMovementDamage.Id);
                        if (StockDamageInfo != null)
                        {

                            lStockDamageFromDB = new StockMovementDamaged();
                            lStockDamageFromDB = StockMovementManager.Instance.UpdateStockMovementDamaged(lStockMovementDamage);
                        }
                        else
                        {
                            DisplaySystemErrorPerformCancel("Somthing went wrong, the selected stock Damage is not valid.");
                            return;
                        }
                    }

                    TextBoxStockDamageId.Text = lStockDamageFromDB.Id.ToString();
                    StockDamageReferenceNumber.Text = lStockDamageFromDB.RefNumber;

                    if (lStockDamageFromDB != null)
                    {
                        EnableForm(false);
                        LoadStockDamageEntry(lStockDamageFromDB.Id);
                        BtnStockDamagePrint.Select();
                        StockDamageErrorMsg.Text = SaveSuccessText;
                    }
                    DirtyFlag(false);

                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void BtnStockDamageExit_Click(object sender, EventArgs e)
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
            BtnStockDamageCancel.PerformClick();
            return;
        }
        private void FormDamageEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatetimePickerStockDamageDate.Focus();
                    e.Cancel = true;
                }
            }
        }
        private Boolean ValidateForm()
        {
            StockDamageErrorMsg.Text = "";

            if (DatetimePickerStockDamageDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerStockDamageDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerStockDamageDate.Focus();
                StockDamageErrorMsg.Text = EnterStockDamageEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxLocation.SelectedIndex < 0)
            {
                ComboBoxLocation.Select();
                StockDamageErrorMsg.Text = SelectLocationErrorMsg;
                ResetTimmer();
                return false;
            }
            int Count = GridViewStockDamageItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ID].Value == null)
                    {
                        GridViewStockDamageItem.Select();
                        GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, i];
                        GridViewStockDamageItem.BeginEdit(true);
                        StockDamageErrorMsg.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM].Value == null
                        || string.IsNullOrEmpty(GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM].Value.ToString())
                        )
                    {
                        GridViewStockDamageItem.Select();
                        GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.UOM, i];
                        GridViewStockDamageItem.BeginEdit(true);
                        StockDamageErrorMsg.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewStockDamageItem.Columns[(int)StockDamageTableColumn.UOM].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DISCRI].Value == null
                        || string.IsNullOrEmpty(GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DISCRI].Value.ToString())
                        )
                    {
                        GridViewStockDamageItem.Select();
                        GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.DISCRI, i];
                        GridViewStockDamageItem.BeginEdit(true);
                        StockDamageErrorMsg.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewStockDamageItem.Columns[(int)StockDamageTableColumn.DISCRI].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.QTY].Value.ToString());
                    for (int j = 3; j < 7; j++)
                    {
                        if ((j == (int)StockDamageTableColumn.QTY) && !(Quantity <= 0)) { continue; }
                        if ((!((bool)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ISBAT].Value)) && (j == (int)StockDamageTableColumn.EXPDATE || j == (int)StockDamageTableColumn.BATNO)) { continue; }
                        if (j == (int)StockDamageTableColumn.QTY && (GridViewStockDamageItem.Rows[i].Cells[j].Value == null || GridViewStockDamageItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewStockDamageItem.Rows[i].Cells[j].Value.ToString()) == 0))
                        {
                            GridViewStockDamageItem.Select();
                            GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[j, i];
                            GridViewStockDamageItem.BeginEdit(true);
                            StockDamageErrorMsg.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewStockDamageItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 5 || j == 4) && GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.BATCHID].Value == null)
                        {
                            GridViewStockDamageItem.Select();
                            GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[j, i];
                            GridViewStockDamageItem.BeginEdit(true);
                            StockDamageErrorMsg.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewStockDamageItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }

                    }
                    long PId = (long)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ID].Value;
                    Inventory Inventory = null;
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed && Quantity < 0)
                    {
                        double NewQty = Quantity;
                        double OldQty = 0;
                        string Uom = GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM].Value.ToString();
                        Product Product = CatalogProductManager.Instance.GetProductInfoById(PId);
                        if (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DETAILID].Value != null)
                        {
                            long PDetailId = (long)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DETAILID].Value;
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
                        if ((bool)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ISBAT].Value)
                        {
                            long BId = (long)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.BATCHID].Value;
                            Stock = StockByXfactorBatch(BId, Uom);
                            if (NewQty > OldQty && Math.Round(Stock, MidpointRounding.AwayFromZero) < (Math.Round((NewQty - OldQty), MidpointRounding.AwayFromZero)))
                            {
                                GridViewStockDamageItem.Select();
                                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.QTY, i];
                                GridViewStockDamageItem.BeginEdit(true);
                                StockDamageErrorMsg.Text = "Please Enter valid Quantity, Available Quantity is " + (StockByXfactorBatch(BId, Uom) - OldQty);
                                ResetTimmer();
                                return false;
                            }
                        }
                        else
                        {
                            Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(PId, FromLocationId);
                            if (Inventory == null)
                            {
                                GridViewStockDamageItem.Select();
                                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)PurchaseEntryTableColumn.PRODUCT, i];
                                GridViewStockDamageItem.BeginEdit(true);
                                StockDamageErrorMsg.Text = "No stock Available";
                                ResetTimmer();
                                return false;
                            }
                            else if (NewQty > OldQty && (StockByXfactor(PId, Uom) < (NewQty - OldQty)))
                            {
                                GridViewStockDamageItem.Select();
                                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)PurchaseEntryTableColumn.QTY, i];
                                GridViewStockDamageItem.BeginEdit(true);
                                StockDamageErrorMsg.Text = "Please Enter valid Quantity, Available Quantity is " + (StockByXfactor(PId, Uom) - OldQty);
                                ResetTimmer();
                                return false;
                            }
                        }
                    }
                    bool isBatch = (bool)GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ISBAT].Value;
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
                            StockDamageErrorMsg.Text = "Batch not available.";
                            ResetTimmer();
                            return false;
                        }
                    }
                }
            }
            else
            {
                GridViewStockDamageItem.Select();
                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, 0];
                GridViewStockDamageItem.BeginEdit(true);
                StockDamageErrorMsg.Text = Grid_EmptyErrorMsg;
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
                return (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockDamageDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) * (InventoryBatch.RetailUOM == Uom ? (InventoryBatch.WholesaleXFactor * InventoryBatch.RetailXFactor) : InventoryBatch.WholesaleXFactor));
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
                            TotalStock += (((Detail.StockDate != null && Detail.StockDate <= DatetimePickerStockDamageDate.Date ? Detail.OpeningStock : 0) + Detail.QuantityOnHand) * (Product.RetailUOM == Uom ? (Detail.WholesaleXFactor * Detail.RetailXFactor) : Detail.WholesaleXFactor));
                        }
                        return TotalStock;
                    }
                }
                else
                {
                    Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(ProductId, FromLocationId);
                    if (Inventory != null)
                    {
                        double TotalStock = (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockDamageDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) * (Product.RetailUOM == Uom ? (Product.WholesaleXFactor * Product.RetailXFactor) : Product.WholesaleXFactor));
                        return TotalStock;
                    }
                }
            }
            return 0;
        }


        private void ResetForm()
        {
            StockDamageReferenceNumber.Text = "000000";
            TextBoxStockDamageSearch.TextBox.ResetText();
            StockDamageErrorMsg.Text = "";
            TextBoxStockDamageId.ResetText();
            DatetimePickerStockDamageDate.Format = Global.Company.DateFormat;
            DatetimePickerStockDamageDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            GridViewStockDamageItem.Rows.Clear();
            GridViewStockDamageItem.Rows.Add();
            ComboBoxLocation.ResetText();
            ComboUtils.InitializeStockLocationCombo(ComboBoxLocation, Global.Company.CompanyId);
            ComboBoxLocation.SelectedIndex = -1;
            LastStockDamageReferenceNumber.Text = CompanyManager.Instance.GetMovementPrevRef(Global.Company, InventoryJournalType.DAMAGED, (DateTime)DatetimePickerStockDamageDate.Date);
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxLocation.Visible = enable;
            BtnStockDamageDelete.Enabled = true;
            BtnStockDamageSave.Enabled = true;
            DatetimePickerStockDamageDate.ReadOnly = !enable;
            DatetimePickerStockDamageDate.TabStop = enable;
            GridViewStockDamageItem.ReadOnly = false;
            GridViewStockDamageItem.TabStop = true;
            GridViewStockDamageItem.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnStockDamageNew.Enabled = !enable;
                BtnStockDamageDelete.Enabled = !enable;
                BtnStockDamagePrint.Enabled = !enable;
                BtnStockDamageCancel.Enabled = enable;
                BtnStockDamageSave.Enabled = enable;
            }
            else
            {
                BtnStockDamageNew.Enabled = !enable;
                BtnStockDamageDelete.Enabled = !enable;
                BtnStockDamagePrint.Enabled = !enable;
                BtnStockDamageCancel.Enabled = !enable;
                BtnStockDamageSave.Enabled = !enable;
            }

            if (!string.IsNullOrEmpty(TextBoxStockDamageId.Text))
            {
                if (StockMovementManager.IsStockOutReceived(long.Parse(TextBoxStockDamageId.Text)))
                {
                    GridViewStockDamageItem.ReadOnly = !enable;
                    BtnStockDamageDelete.Enabled = enable;
                    BtnStockDamageSave.Enabled = enable;
                    BtnStockDamageCancel.Enabled = enable;

                }
            }

        }
        private void LoadStockDamageEntry(long StockDamageId)
        {
            ResetForm();
            StockMovement StockMovementDamage = StockMovementManager.Instance.GetStockMovementById(StockDamageId);
            if (StockMovementDamage != null)
            {
                StockDamageReferenceNumber.Text = StockMovementDamage.RefNumber;
                TextBoxStockDamageId.Text = StockMovementDamage.Id.ToString();
                DatetimePickerStockDamageDate.Date = (DateTime)DateUtils.ToDate(StockMovementDamage.MovementDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);

                InventoryLocation InventoryLocation = null;
                if (StockMovementDamage.InventoryStockLocationId != 0L)
                {
                    InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(StockMovementDamage.InventoryStockLocationId);
                    ComboBoxLocation.SelectedIndex = ComboBoxLocation.FindStringExact(InventoryLocation.Name);
                    InventoryLocation = null;
                }
                if (StockMovementDamage.StockMovementDetails.Count > 0)
                {
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var StockMovementDetails in StockMovementDamage.StockMovementDetails)
                    {
                        GridViewStockDamageItem.Rows.Add();
                        StockMovementDetail lStockMovementDetails = StockMovementManager.GetStockMovementDetail(StockMovementDetails.Id);
                        if (lStockMovementDetails != null)
                        {
                            (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                            (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.RetailUOM);
                            (GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.WholesaleUOM);
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.SNO].Value = i + 1;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.PRODUCT].Value = lStockMovementDetails.Product.Name;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.UOM].Value = lStockMovementDetails.Product.RetailUOM == lStockMovementDetails.Uom ? lStockMovementDetails.Product.RetailUOM : lStockMovementDetails.Product.WholesaleUOM; ;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DISCRI].Value = lStockMovementDetails.Description;
                            if (lStockMovementDetails.isBatch)
                            {
                                GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.BATNO].Value = lStockMovementDetails.BatchNo;
                                GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.EXPDATE].Value = lStockMovementDetails.ExpDate;
                            }
                            if (lStockMovementDetails.isBatch && lStockMovementDetails.BatchNo != null)
                            {
                                GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.BATCHID].Value = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lStockMovementDetails.ProductId, lStockMovementDetails.BatchNo, StockMovementDamage.InventoryStockLocationId).Id;
                            }
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.QTY].Value = lStockMovementDetails.Quantity;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ID].Value = lStockMovementDetails.ProductId;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.ISBAT].Value = lStockMovementDetails.isBatch;
                            GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.DETAILID].Value = lStockMovementDetails.Id;
                            i++;
                        }

                    }
                    ReSequence();
                }
                EnableForm(false);
                GridViewStockDamageItem.Select();
                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, 0];
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("Something went wrong,please check purchase");
            }
        }

        private void ReSequence()
        {
            for (int i = 0; i < GridViewStockDamageItem.Rows.Count; i++)
            {
                GridViewStockDamageItem.Rows[i].Cells[(int)StockDamageTableColumn.SNO].Value = i + 1;
            }
        }

        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerStock.Stop();
            TimerStock.Start();
        }
        private void TimerStock_Tick(object sender, EventArgs e)
        {
            this.StockDamageErrorMsg.Visible = !this.StockDamageErrorMsg.Visible;
            blinkCount++;
            if (blinkCount == 3 * 2)
            {
                TimerStock.Stop();
                StockDamageErrorMsg.Visible = true;
            }
        }

        private void GridViewStockDamageItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)StockDamageTableColumn.BATNO && GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATCHID].Value != null)
            {
                LoadBatchDetails((long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATCHID].Value);
            }
        }

        private void GridViewStockDamageItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewStockDamageItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                DirtyFlag(IsDirty);
            }
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.REMOVE].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.SNO].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.UOM].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.QTY].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.DISCRI].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.EXPDATE].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.PRICE].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.COST].ReadOnly = true;
            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.ID].Value != null)
            {
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.UOM].ReadOnly = false;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.QTY].ReadOnly = false;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.DISCRI].ReadOnly = false;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.PRICE].ReadOnly = false;
                if ((bool)GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.ISBAT].Value)
                {
                    if (!Global.Company.CompanySalesSetup.IsNegativeStockAllowed && !ValidateLineItemModifiedOrRemove(e.RowIndex))
                    {
                        GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].ReadOnly = true;
                    }
                    else
                    {
                        GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].ReadOnly = false;
                        GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.PRODUCT].ReadOnly = false;
                    }
                }
            }
            if (GridViewStockDamageItem.Rows[e.RowIndex].Cells[4].Value == null)
            {
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].Value = "";
            }
        }

        private void GridViewStockDamageItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                StockDamageErrorMsg.Text = string.Empty;
                if (e.ColumnIndex == (int)StockDamageTableColumn.REMOVE && (GridViewStockDamageItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.SNO].Value.ToString()), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {

                        if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                        {
                            StockDamageErrorMsg.Text = string.Format(Grid_RowDeleteErrorMsg, GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.SNO].Value.ToString());
                            ResetTimmer();
                            return;
                        }
                        GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockDamageItem.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        private bool ValidateLineItemModifiedOrRemove(int Index)
        {
            if (GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.DETAILID].Value != null
                            && GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value != null)
            {
                long PDetailId = (long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.DETAILID].Value;
                long PId = (long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value;
                StockMovementDetail StockMovementDetails = StockMovementManager.Instance.GetStockMovementDetail(PDetailId);
                if (StockMovementDetails != null)
                {
                    double OldQty = StockMovementDetails.Quantity;

                    if ((bool)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ISBAT].Value)
                    {
                        String BNo = (string)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.BATNO].Value;
                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(PId, BNo, FromLocationId);
                        if (InventoryBatch != null)
                        {
                            if (InventoryBatch != null && (((InventoryBatch.StockDate != null && InventoryBatch.StockDate <= DatetimePickerStockDamageDate.Date ? InventoryBatch.OpeningStock : 0) + InventoryBatch.QuantityOnHand) < OldQty))
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
                            if (((Inventory.StockDate != null && Inventory.StockDate <= DatetimePickerStockDamageDate.Date ? Inventory.OpeningStock : 0) + Inventory.QuantityOnHand) < OldQty)
                            {
                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        }

        private void GridViewStockDamageItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                    int Index = GridViewStockDamageItem.CurrentCell.RowIndex;
                    GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }

        }
        private void UomColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewStockDamageItem.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.UOM].Value = ((ComboBox)sender).Text;
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.UOM].Value == null)
                {
                    GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.PRICE].Value = 0.00;
                }
            }

        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value);
                if (Product != null)
                {
                    StockDamageProductDetails.LocationId = FromLocationId;
                    if ((bool)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ISBAT].Value)
                    {
                        if (GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.BATCHID].Value, FromLocationId);
                            if (InventoryBatch != null)
                            {
                                GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice;
                                StockDamageProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                            StockDamageProductDetails.ProductId = Product.Id;
                        }
                    }
                    else
                    {
                        GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                        StockDamageProductDetails.ProductId = Product.Id;
                    }
                }
            }
        }
        private void GridViewStockDamageItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewStockDamageItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }

                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged);

                ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged;
                ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged;

                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockDamageItem_KeyPress1);
            }
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockDamageItem, "TaxDetailsNumberChecking", GridViewStockDamageItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockDamageItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockDamageItem_KeyDown;
            }
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockDamageItem, "NameCheckingProduct", GridViewStockDamageItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockDamageItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockDamageItem_KeyDown;
            }
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).TextChanged -= ProductTextChange;
                ((TextBox)e.Control).TextChanged += ProductTextChange;
            }
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO)
            {
                ((TextBox)e.Control).TextChanged -= BatchTextChange;
                ((TextBox)e.Control).TextChanged += BatchTextChange;
            }
        }
        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewStockDamageItem.CurrentCell.ColumnIndex != (int)StockDamageTableColumn.BATNO && GridViewStockDamageItem.CurrentCell.ColumnIndex != (int)StockDamageTableColumn.DISCRI)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
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
                            long CheckForAddRow = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null) ? (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value : 0L;
                            LoadUomTax(Product.First().Id);
                            GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            if (CheckForAddRow == 0 && GridViewStockDamageItem.Rows.Count - 1 == GridViewStockDamageItem.CurrentRow.Index)
                            {
                                GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                GridViewStockDamageItem.Rows.Add();
                            }
                        }
                        else
                        {
                            MessageBox.Show("please select stock from location.", "Warning");
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewStockDamageItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewStockDamageItem.CurrentRow.Index);
                }
            }
        }
        private void BatchTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewStockDamageItem.CurrentCell.ColumnIndex != (int)StockDamageTableColumn.DISCRI
                && GridViewStockDamageItem.CurrentCell.ColumnIndex != (int)StockDamageTableColumn.REMOVE)
            {
                long PId = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null) ? (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value : 0L;
                StockDamageProductDetails.LocationId = FromLocationId;

                if (!string.IsNullOrEmpty(((TextBox)sender).Text) && GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATNO].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    InventoryLocation Location = ((InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex]);
                    InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value, ((TextBox)sender).Text, Location.Id);
                    if (BatchDetails != null)
                    {
                        LoadBatchDetails(BatchDetails.Id);
                        StockDamageProductDetails.BatchId = BatchDetails.Id;
                    }
                    else
                    {
                        ResetBatchDetails();
                        StockDamageProductDetails.ProductId = PId;

                    }
                    GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetBatchDetails();
                    if (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null)
                    {
                        StockDamageProductDetails.ProductId = (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value;
                    }
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.PRODUCT].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.UOM].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.QTY].Value = 0;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DISCRI].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.BATNO].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.EXPDATE].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.PRICE].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.COST].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RETAIL].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.MSRP].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.AMOUNT].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ID].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ISBAT].Value = false;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DETAILID].Value = null;
            StockDamageProductDetails.Clear();

        }
        private void GridViewStockDamageItem_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewStockDamageItem.EditingControl).DroppedDown = false;

        }
        private void GridViewStockDamageItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
            }
            if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
            }
        }

        private void GridViewStockDamageItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
                if (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
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
                int index = GridViewStockDamageItem.CurrentRow.Index;
                (GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                (GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                (GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.PRODUCT].Value = Product.Name;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.QTY].Value = 0;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DISCRI].Value = null;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.BATNO].Value = null;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.EXPDATE].Value = null;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.PRICE].Value = Product.PurchasePrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.COST].Value = 0.00;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RETAIL].Value = Product.RetailPrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.MSRP].Value = Product.Msrp;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RUOM].Value = Product.RetailUOM;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RXFACT].Value = Product.RetailXFactor;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WUOM].Value = Product.WholesaleUOM;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DETAILID].Value = null;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.AMOUNT].Value = 0.00;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null)
                {
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ISBAT].Value = false;
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
                long Check = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null) ? (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value : 0L;
                ProductId = 0L;
                FormSearchItems FormSearchItems = new FormSearchItems(this);
                GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                FormSearchItems.SearchText = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.PRODUCT].Value != null) ? GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.PRODUCT].Value.ToString() : null;
                FormSearchItems.LocationId = FromLocationId;
                FormSearchItems.ShowDialog();
                if (ProductId != 0L)
                {
                    Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                    if (Product != null)
                    {
                        LoadUomTax(ProductId);
                        LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId));

                        if (Check == 0 && GridViewStockDamageItem.Rows.Count - 1 == GridViewStockDamageItem.CurrentRow.Index)
                        {
                            GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewStockDamageItem.Rows.Add();
                        }
                        GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[2, GridViewStockDamageItem.CurrentRow.Index];
                        GridViewStockDamageItem.CurrentCell.Selected = true;
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
            long Check = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATCHID].Value != null) ? (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATCHID].Value : 0L;
            BatchId = 0L;
            FormSearchBatch FormSearchBatch = new FormSearchBatch(this);
            GridViewStockDamageItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchBatch.ProductId = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value != null) ? (long)GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.ID].Value : 0L;
            FormSearchBatch.SearchText = (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATNO].Value != null) ? GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATNO].Value.ToString() : string.Empty;
            FormSearchBatch.LocationId = FromLocationId;
            FormSearchBatch.ShowDialog();
            if (BatchId != 0)
            {
                LoadBatchDetails(BatchId);
                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.DISCRI, GridViewStockDamageItem.CurrentRow.Index];
                GridViewStockDamageItem.CurrentCell.Selected = true;
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
                int index = GridViewStockDamageItem.CurrentRow.Index;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.BATCHID].Value = InventoryBatch.Id;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;
                GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DETAILID].Value = null;
                StockDamageProductDetails.BatchId = InventoryBatch.Id;
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected Batch is not valid.");
                return;
            }
        }
        private void ResetBatchDetails()
        {
            int index = GridViewStockDamageItem.CurrentRow.Index;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.EXPDATE].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.BATCHID].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.DETAILID].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RETAIL].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.MSRP].Value = 0.00;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RUOM].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RXFACT].Value = 1;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WUOM].Value = null;
            GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WXFACT].Value = 1;

            if (GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.ID].Value);
                if (Product != null)
                {
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RETAIL].Value = Product.RetailPrice;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.MSRP].Value = Product.Msrp;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RUOM].Value = Product.RetailUOM;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.RXFACT].Value = Product.RetailXFactor;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WUOM].Value = Product.WholesaleUOM;
                    GridViewStockDamageItem.Rows[index].Cells[(int)StockDamageTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                }
            }
        }

        private void GridViewStockDamageItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                bool IsDirty = this.formIsDirty;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.SNO].Value = GridViewStockDamageItem.Rows.Count;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.PRODUCT].Value = "";
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.ISBAT].Value = false;
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
                GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].Value = "";
                DirtyFlag(IsDirty);
            }
        }

        private void GridViewStockDamageItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadProductDetails(e.RowIndex);
                StockDamageErrorMsg.Text = string.Empty;
                if (GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.DETAILID].Value != null)
                {
                    if (!ValidateLineItemModifiedOrRemove(e.RowIndex) && !Global.Company.CompanySalesSetup.IsNegativeStockAllowed)
                    {
                        GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.PRODUCT].ReadOnly = true;
                        if ((bool)GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.ISBAT].Value)
                        {
                            GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.BATNO].ReadOnly = true;
                        }
                        StockDamageErrorMsg.Text = string.Format(Grid_RowEditErrorMsg, GridViewStockDamageItem.Rows[e.RowIndex].Cells[(int)StockDamageTableColumn.SNO].Value.ToString());
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
            StockDamageProductDetails.EditableStock = 0.00;
            if (GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.ID].Value);
                if (Product != null)
                {
                    LoadProductAdditinalDetails(Product);
                    if (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.BATCHID].Value != null)
                    {
                        StockDamageProductDetails.BatchId = (long)GridViewStockDamageItem.Rows[Index].Cells[(int)StockDamageTableColumn.BATCHID].Value;
                    }
                }
            }
            else
            {
                StockDamageProductDetails.Clear();
                EnableProductAdditinalDetails();
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            StockDamageProductDetails.LocationId = FromLocationId;
            StockDamageProductDetails.ProductId = Product.Id;
            EnableProductAdditinalDetails();
        }
        private void EnableProductAdditinalDetails()
        {
            StockDamageProductDetails.ReadyOnly = true;
        }
        private void ComboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            InventoryLocation lLocation = null;
            if (ComboBoxLocation.SelectedIndex > -1)
            {
                lLocation = (InventoryLocation)ComboBoxLocation.Items[ComboBoxLocation.SelectedIndex];
            }
            if (!(GridViewStockDamageItem.Rows.Count == 1 || (Location != null && lLocation.Id == FromLocationId)))
            {
                DialogResult Result = MessageBox.Show("Items will be removed from the sale if you change the stock location", "Confirm Stock Location",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.OK)
                {
                    GridViewStockDamageItem.Rows.Clear();
                    GridViewStockDamageItem.Rows.Add();
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

        private void BtnStockDamageSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                Cursor.Current = Cursors.WaitCursor;
                RecentPurchases();
                if (SearchStockDamageId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnStockDamageSave_Click(sender, e);
                                LoadStockDamageEntry(SearchStockDamageId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadStockDamageEntry(SearchStockDamageId);
                        }
                    }
                    else
                    {
                        LoadStockDamageEntry(SearchStockDamageId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private bool isValidSearchCriteria()
        {
            StockDamageErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxStockDamageSearch.Text.Trim()))
            {
                StockDamageErrorMsg.Text = SearchBoxEmptyErrorMsg;
                TextBoxStockDamageSearch.TextBox.Select();
                return true;
            }
            return true;
        }

        private void RecentPurchases()
        {
            StockDamageErrorMsg.Text = "";
            String SearchText = TextBoxStockDamageSearch.Text.Trim();
            IList<StockMovementDamaged> StockMovementInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                StockMovementInfo = StockMovementManager.GetRecentStockDamages(Global.Company.CompanyId);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                StockMovementInfo = StockMovementManager.GetStockDamageByDate((DateTime)Date, Global.Company.CompanyId);
            }
            else
            {
                StockMovementInfo = StockMovementManager.GetStockDamageByReferenceNo(SearchText, Global.Company.CompanyId);
            }

            if (StockMovementInfo.Count > 0)
            {
                LoadStockDamageEntry(StockMovementInfo);
            }
            else
            {
                SearchStockDamageId = 0L;
                StockDamageErrorMsg.Text = StockDamageSearchOutput;
            }
        }
        public void LoadStockDamageEntry(IList<StockMovementDamaged> StockMovementInfo)
        {
            if (StockMovementInfo.Count > 0)
            {
                SearchStockDamageId = 0L;
                FormRecentStockMovement FormRecentStockMovement = new FormRecentStockMovement(this);
                FormRecentStockMovement.StockMovementDamageInfo = StockMovementInfo;
                FormRecentStockMovement.InventoryJournalType = InventoryJournalType.DAMAGED;
                FormRecentStockMovement.ShowDialog();

            }
            else
            {
                StockDamageErrorMsg.Text = StockDamageSearchOutput;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                if (BtnStockDamageNew.Enabled)
                {
                    BtnStockDamageNew.PerformClick();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnStockDamageDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnStockDamagePrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnStockDamageSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                GridViewStockDamageItem.EndEdit();
                BtnStockDamageCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnStockDamageExit.PerformClick();
                return true;
            }
            try
            {
                if (GridViewStockDamageItem.CurrentCell != null)
                {
                    if ((keyData == Keys.F2) && GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
                    {
                        SearchProduct();
                        return true;
                    }
                    if ((keyData == Keys.F2) && !GridViewStockDamageItem.CurrentCell.ReadOnly && GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO)
                    {
                        SearchBatch();
                        return true;
                    }
                    if (keyData == (Keys.Tab) && (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.DISCRI))
                    {
                        if (GridViewStockDamageItem.CurrentRow.Index == GridViewStockDamageItem.Rows.Count - 1)
                        {
                            BtnStockDamageSave.Select();
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab) && (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.BATNO))
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab) && (GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.QTY) && GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATNO].ReadOnly)
                    {
                        SendKeys.Send("{tab}");
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.PRODUCT)
                    {
                        if (GridViewStockDamageItem.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            if (ComboBoxLocation.Visible)
                            {
                                ComboBoxLocation.Focus();
                            }
                            else
                            {
                                BtnStockDamageSave.Select();
                                return true;
                            }
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewStockDamageItem.CurrentCell.ColumnIndex == (int)StockDamageTableColumn.DISCRI)
                    {
                        if (GridViewStockDamageItem.CurrentRow.Cells[(int)StockDamageTableColumn.BATNO].ReadOnly)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxStockDamageSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnStockDamageSearch_Click(sender, e);
            }
        }

        private void BtnStockDamageSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DatetimePickerStockDamageDate.ReadOnly)
                {
                    GridViewStockDamageItem.Select();
                    GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, 0];
                }
                else
                {
                    DatetimePickerStockDamageDate.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockDamageItem.Select();
                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, 0]; return;
            }
        }

        private void ComboBoxLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockDamageItem.Select();
                GridViewStockDamageItem.CurrentCell = GridViewStockDamageItem[(int)StockDamageTableColumn.PRODUCT, 0];
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerStockDamageDate.Focus();
            }
        }

        private void DatetimePickerStockDamageDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxLocation.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnStockDamageSave.Select();
            }
        }
        private void BtnStockDamagePrint_Click(object sender, EventArgs e)
        {
            DamageEntryPrint DamageEntryPrint = new DamageEntryPrint();
            DamageEntryPrint.PrintDamageEntry(long.Parse(TextBoxStockDamageId.Text), "Damage Entry");
        }

        private void GridViewStockDamageItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
                if (e.ColumnIndex == 7)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                }
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
    }
}
