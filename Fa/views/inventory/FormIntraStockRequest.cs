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
    public partial class FormIntraStockRequest : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string RequestSuccessText = "Request completed...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteConfirmText = "Do you want to delete Stock movement request Entry {0}?";
        public static string DeleteErrorText = "Error in Stock movement request Deleting. !";
        public static string NotAllowDeleteErrorText = "Do not delete Stock movement it used in intra stock movement";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string ResetItemTaxConfirmText = "Reset product it's takes latest tax";
        public static string SelectFromLocationErrorMsg = "Please select stock request location.";
        public static string SelectToLocationErrorMsg = "Please select stock to location.";
        public static string EnterStockMovementEntryDateErrorMsg = "Please enter Stock movement request date.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_ChooseItemErrorMsg = "Please choose product/item.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemInvalidDataErrorMsg = "Please enter valid {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter Stock movement request Items details.";
        public static string Grid_ItemInvalidQuantityErrorMsg = "please Enter valid quantity";
        public static string InvalidStockMovementEntryErrorMsg = "Invalid stock movement request.";
        public static string SearchBoxEmptyErrorMsg = "Please enter search text, it could from/to location name or Stock movement request date/reference number.";
        public static string StockMovementSearchOutput = "No stock movement found.";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public long ProductId = 0L;
        public long BatchId;
        public long SearchStockRequestId = 0L;
        public long FromLocationId = 0L;
        public long ToLocationId = 0L;
        StockMovementManager StockMovementManager = null;
        public FormIntraStockRequest()
        {
            InitializeComponent();
            StockMovementManager = StockMovementManager.Instance;
            excludedObjects = new string[] { "toolStrip1", "StockMovementProductDetails" };
        }
        private void FormIntraStockRequest_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Visible = false;
                setSize();
                this.Visible = true;
                ResetForm();
                EnableForm(true);
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
        private StockMovementRequest GetStockMovementRequestEntryFromForm()
        {
            StockMovementRequest lStockMovementRequest = new StockMovementRequest();
            lStockMovementRequest.Id = TextBoxStockMovementId.Text == string.Empty ? 0L : Convert.ToInt64(TextBoxStockMovementId.Text);
            lStockMovementRequest.RefNumber = StockMovementReferenceNumber.Text;
            lStockMovementRequest.MovementDate = (DateTime)DatetimePickerStockMovementDate.Date;
            lStockMovementRequest.CompanyId = Global.Company.CompanyId;

            InventoryLocation InventoryLocation = ((InventoryLocation)ComboBoxToLocation.Items[ComboBoxToLocation.SelectedIndex]);
            if (InventoryLocation != null)
            {
                lStockMovementRequest.RequestInventoryLocationId = InventoryLocation.Id;
                InventoryLocation = null;
            }
            InventoryLocation = ((InventoryLocation)ComboBoxFromLocation.Items[ComboBoxFromLocation.SelectedIndex]);
            if (InventoryLocation != null)
            {
                lStockMovementRequest.InventoryStockLocationId = InventoryLocation.Id;
            }

            if (Global.CostCenter != null)
            {
                lStockMovementRequest.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < GridViewStockMovementItem.Rows.Count - 1; i++)
            {
                StockMovementDetail StockMovementDetail = new StockMovementDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    StockMovementDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        StockMovementDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    StockMovementDetail.Uom = GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value.ToString();
                    StockMovementDetail.Id = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value == null) ? 0L : long.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value.ToString());
                    StockMovementDetail.ProductId = Product.Id;
                    StockMovementDetail.MaterialId = Product.MaterialId;
                    StockMovementDetail.isBatch = (bool)GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value;
                    //FOR SQL
                    StockMovementDetail.ExpDate = Global.getTransactionDate();
                    if (StockMovementDetail.isBatch)
                    {
                        StockMovementDetail.BatchNo = GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value != null ? GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value.ToString() : "";
                        if (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.EXPDATE].Value != null)
                        {
                            StockMovementDetail.ExpDate = (DateTime)GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.EXPDATE].Value;
                        }
                    }
                    double Quantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString());
                    double FreeQuantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value.ToString());
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
                    lStockMovementRequest.StockMovementDetails.Add(StockMovementDetail);
                }
            }
            return lStockMovementRequest;
        }

        private void BtnStockMovementNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (ValidateForm())
                    {
                        BtnStockMovementSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    DatetimePickerStockMovementDate.Focus();
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            SearchStockRequestId = 0L;
            EnableForm(true);
            DatetimePickerStockMovementDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockMovementDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxStockMovementId.Text))
            {
                MessageBox.Show("Somting went wrong, please check this stock movement is still valid.");
                return;
            }
            DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, StockMovementReferenceNumber.Text), "Delete Confirm",
         MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                long ID = Convert.ToInt64(TextBoxStockMovementId.Text);
                StockMovementOut StockMovementOut = StockMovementManager.GetStockMovementOut(ID);
                if (StockMovementOut != null)
                {
                    bool DeleteResult = StockMovementManager.DeleteStockMovementRequest(ID);
                    if (DeleteResult)
                    {
                        ResetForm();
                        EnableForm(true);
                        DatetimePickerStockMovementDate.Focus();
                        DirtyFlag(false);
                    }
                    else
                    {
                        MessageBox.Show(DeleteErrorText);
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
        private void BtnStockMovementCancel_Click(object sender, EventArgs e)
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
            SearchStockRequestId = 0L;
            EnableForm(true);
            DatetimePickerStockMovementDate.Focus();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnStockMovementSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    StockMovementRequest lStockMovementRequest = GetStockMovementRequestEntryFromForm();
                    StockMovementRequest lStockMovementFromDB = null;
                    if (lStockMovementRequest.Id == 0)
                    {
                        string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.STOCK_OUT, (DateTime)DatetimePickerStockMovementDate.Date);
                        if (!string.IsNullOrEmpty(RefNumber))
                        {
                            lStockMovementRequest.RefNumber = RefNumber;
                            lStockMovementFromDB = new StockMovementRequest();
                            try
                            {
                                lStockMovementFromDB = StockMovementManager.Instance.AddStockMovementRequest(lStockMovementRequest);
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
                        StockMovementRequest StockMovementInfo = StockMovementManager.Instance.GetStockMovementRequest(lStockMovementRequest.Id);
                        if (StockMovementInfo != null)
                        {

                            lStockMovementFromDB = new StockMovementRequest();
                            lStockMovementFromDB = StockMovementManager.Instance.UpdateStockMovementRequest(lStockMovementRequest);
                        }
                        else
                        {
                            DisplaySystemErrorPerformCancel("Somthing went wrong, the selected stock movement is not valid.");
                            return;
                        }
                    }

                    TextBoxStockMovementId.Text = lStockMovementFromDB.Id.ToString();
                    StockMovementReferenceNumber.Text = lStockMovementFromDB.RefNumber;

                    if (lStockMovementFromDB != null)
                    {
                        EnableForm(false);
                        LoadStockMovementEntry(lStockMovementFromDB.Id);
                        BtnStockMovementPrint.Select();
                        ToolStripStatusLabelErrorPurchase.Text = SaveSuccessText;
                    }
                    DirtyFlag(false);

                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void BtnStockMovementExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Close();
            Cursor.Current = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (ComboBoxFromLocation.SelectedIndex < 1)
            {
                ComboBoxFromLocation.Select();
                ToolStripStatusLabelErrorPurchase.Text = SelectFromLocationErrorMsg;
                ResetTimmer();
                return false;
            }
            if (DatetimePickerStockMovementDate.Date == null || !DateUtils.ValidDate(((DateTime)DatetimePickerStockMovementDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                DatetimePickerStockMovementDate.Focus();
                ToolStripStatusLabelErrorPurchase.Text = EnterStockMovementEntryDateErrorMsg;
                ResetTimmer();
                return false;
            }
            if (ComboBoxFromLocation.SelectedIndex < 1)
            {
                ToolStripStatusLabelErrorPurchase.Text = SelectFromLocationErrorMsg;
                ComboBoxFromLocation.Select();
                ResetTimmer();
                return false;
            }
            if (ComboBoxToLocation.SelectedIndex < 1)
            {
                ToolStripStatusLabelErrorPurchase.Text = SelectToLocationErrorMsg;
                ComboBoxToLocation.Select();
                ResetTimmer();
                return false;
            }
            int Count = GridViewStockMovementItem.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    if (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value == null)
                    {
                        GridViewStockMovementItem.Select();
                        GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, i];
                        GridViewStockMovementItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = Grid_ChooseItemErrorMsg;
                        ResetTimmer();
                        return false;
                    }
                    if (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value == null
                        || string.IsNullOrEmpty(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value.ToString())
                        )
                    {
                        GridViewStockMovementItem.Select();
                        GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.UOM, i];
                        GridViewStockMovementItem.BeginEdit(true);
                        ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsgs, GridViewStockMovementItem.Columns[(int)StockMovementTableColumn.UOM].HeaderText);
                        ResetTimmer();
                        return false;
                    }
                    double Quantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString());
                    double Free = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value.ToString());

                    for (int j = 3; j < 7; j++)
                    {

                        if ((j == (int)StockMovementTableColumn.QTY || j == (int)StockMovementTableColumn.FREE) && !(Quantity <= 0 && Free <= 0)) { continue; }
                        if ((!(bool)GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value) && (j == (int)StockMovementTableColumn.EXPDATE || j == (int)StockMovementTableColumn.BATNO)) { continue; }
                        if (j == (int)StockMovementTableColumn.FREE) { continue; }
                        if (j != 5 && j != 6 && j != 11 && (GridViewStockMovementItem.Rows[i].Cells[j].Value == null || GridViewStockMovementItem.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || float.Parse(GridViewStockMovementItem.Rows[i].Cells[j].Value.ToString()) <= 0))
                        {
                            GridViewStockMovementItem.Select();
                            GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[j, i];
                            GridViewStockMovementItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemMantatoryFiledErrorMsg, GridViewStockMovementItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if ((j == 6 || j == 5) && GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATCHID].Value == null && GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value != null && !string.IsNullOrEmpty(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value.ToString()))
                        {
                            GridViewStockMovementItem.Select();
                            GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.BATNO, i];
                            GridViewStockMovementItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewStockMovementItem.Columns[(int)StockMovementTableColumn.BATNO].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 11 && GridViewStockMovementItem.Rows[i].Cells[j].Value != null && float.Parse(GridViewStockMovementItem.Rows[i].Cells[j].Value.ToString()) > 100)
                        {
                            GridViewStockMovementItem.Select();
                            GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[j, i];
                            GridViewStockMovementItem.BeginEdit(true);
                            ToolStripStatusLabelErrorPurchase.Text = string.Format(Grid_ItemInvalidDataErrorMsg, GridViewStockMovementItem.Columns[j].HeaderText);
                            ResetTimmer();
                            return false;
                        }
                        if (j == 7)
                        {
                            j = j + 3;
                        }
                    }
                }
            }
            else
            {
                GridViewStockMovementItem.Select();
                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, 0];
                GridViewStockMovementItem.BeginEdit(true);
                ToolStripStatusLabelErrorPurchase.Text = Grid_EmptyErrorMsg;
                ResetTimmer();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            StockMovementReferenceNumber.Text = "000000";
            TextBoxStockMovementSearch.TextBox.ResetText();
            ToolStripStatusLabelErrorPurchase.Text = "";
            TextBoxStockMovementId.ResetText();
            DatetimePickerStockMovementDate.Format = Global.Company.DateFormat;
            DatetimePickerStockMovementDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            GridViewStockMovementItem.Rows.Clear();
            GridViewStockMovementItem.Rows.Add();
            GridViewStockMovementItem.Rows[0].Cells[(int)StockMovementTableColumn.QTY].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewStockItemTotal.Rows.Clear();
            GridViewStockItemTotal.Rows.Add();
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.NAME].Value = "Total : ";
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            ComboBoxFromLocation.ResetText();
            ComboBoxToLocation.ResetText();
            ComboUtils.InitializeStockLocationComboWithEmpty(ComboBoxFromLocation, Global.Company.CompanyId);
            ComboBoxFromLocation.SelectedIndex = 0;
            ComboUtils.InitializeStockLocationComboWithEmpty(ComboBoxToLocation, Global.Company.CompanyId);
            ComboBoxToLocation.SelectedIndex = 0;
            LastStockMovementReferenceNumber.Text = CompanyManager.Instance.GetMovementPrevRef(Global.Company, InventoryJournalType.STOCK_REQUEST, (DateTime)DatetimePickerStockMovementDate.Date);
        }
        private void EnableForm(Boolean enable)
        {
            ComboBoxFromLocation.Visible = true;
            ComboBoxToLocation.Visible = true;
            BtnStockMovementDelete.Enabled = true;
            BtnStockMovementSave.Enabled = true;
            DatetimePickerStockMovementDate.ReadOnly = !enable;
            DatetimePickerStockMovementDate.TabStop = enable;
            GridViewStockMovementItem.ReadOnly = false;
            GridViewStockMovementItem.TabStop = true;
            BtnStockRequestComplete.Enabled = false;
            GridViewStockMovementItem.ScrollBars = ScrollBars.Vertical;
            if (enable)
            {
                BtnStockMovementNew.Enabled = !enable;
                BtnStockMovementDelete.Enabled = !enable;
                BtnStockMovementPrint.Enabled = !enable;
                BtnStockMovementCancel.Enabled = enable;
                BtnStockMovementSave.Enabled = enable;
            }
            else
            {
                BtnStockMovementNew.Enabled = !enable;
                BtnStockMovementDelete.Enabled = !enable;
                BtnStockMovementPrint.Enabled = !enable;
                BtnStockMovementCancel.Enabled = !enable;
                BtnStockMovementSave.Enabled = !enable;
            }

            if (!string.IsNullOrEmpty(TextBoxStockMovementId.Text))
            {
                BtnStockRequestComplete.Enabled = true;
                StockMovementRequest Request = StockMovementManager.GetStockMovementRequest(long.Parse(TextBoxStockMovementId.Text));
                if (Request != null && Request.HasRequestCompleted)
                {
                    GridViewStockMovementItem.ReadOnly = !enable;
                    BtnStockMovementDelete.Enabled = enable;
                    BtnStockMovementSave.Enabled = enable;
                    BtnStockMovementCancel.Enabled = enable;
                    BtnStockRequestComplete.Enabled = enable;
                    ComboBoxFromLocation.Visible = enable;
                    ComboBoxToLocation.Visible = enable;
                }
            }
        }
        private void LoadStockMovementEntry(long StockMovementId)
        {
            ResetForm();
            StockMovementRequest StockMovementOut = StockMovementManager.GetStockMovementRequest(StockMovementId);
            if (StockMovementOut != null)
            {
                StockMovementReferenceNumber.Text = StockMovementOut.RefNumber;
                TextBoxStockMovementId.Text = StockMovementOut.Id.ToString();
                DatetimePickerStockMovementDate.Date = (DateTime)DateUtils.ToDate(StockMovementOut.MovementDate.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                InventoryLocation InventoryLocation = null;
                if (StockMovementOut.InventoryStockLocationId != 0L)
                {
                    InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(StockMovementOut.InventoryStockLocationId);
                    ComboBoxFromLocation.SelectedIndex = ComboBoxFromLocation.FindStringExact(InventoryLocation.Name);
                }
                if (StockMovementOut.RequestInventoryLocationId != 0L)
                {
                    InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(StockMovementOut.RequestInventoryLocationId);
                    ComboBoxToLocation.SelectedIndex = ComboBoxToLocation.FindStringExact(InventoryLocation.Name);
                }
                if (StockMovementOut.StockMovementDetails.Count > 0)
                {
                    GridViewStockMovementItem.Rows.Add(StockMovementOut.StockMovementDetails.Count);
                    int i = 0;
                    IList<Product> Product = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                    foreach (var StockMovementDetails in StockMovementOut.StockMovementDetails)
                    {
                        StockMovementDetail lStockMovementDetails = StockMovementManager.GetStockMovementDetail(StockMovementDetails.Id);
                        if (lStockMovementDetails != null)
                        {
                            (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                            (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.RetailUOM);
                            (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(lStockMovementDetails.Product.WholesaleUOM);
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.SNO].Value = i + 1;
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.PRODUCT].Value = lStockMovementDetails.Product.Name;
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.UOM].Value = lStockMovementDetails.Product.RetailUOM == lStockMovementDetails.Uom ? lStockMovementDetails.Product.RetailUOM : lStockMovementDetails.Product.WholesaleUOM;
                            if (lStockMovementDetails.isBatch && lStockMovementDetails.BatchNo != null && !string.IsNullOrEmpty(lStockMovementDetails.BatchNo))
                            {
                                GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATNO].Value = lStockMovementDetails.BatchNo;
                                GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.EXPDATE].Value = lStockMovementDetails.ExpDate;
                                InventoryBatch lInventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lStockMovementDetails.ProductId, lStockMovementDetails.BatchNo);
                                if (lInventoryBatch != null)
                                {
                                    GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.BATCHID].Value = lInventoryBatch.Id;
                                }
                            }
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value = lStockMovementDetails.Quantity;
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value = lStockMovementDetails.FreeQuantity;
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ID].Value = lStockMovementDetails.ProductId;
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.ISBAT].Value = lStockMovementDetails.isBatch;
                            //for tax
                            GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.DETAILID].Value = lStockMovementDetails.Id;
                            i++;
                        }
                    }
                    ReSequence();
                    ComputeFormTotal();
                }
                EnableForm(false);
                GridViewStockMovementItem.Select();
                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, 0];
                DirtyFlag(false);
            }
            else
            {
                MessageBox.Show("Something went wrong,please check purchase");
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewStockMovementItem.Rows.Count; i++)
            {
                GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.SNO].Value = i + 1;
            }
        }
        public int blinkCount;
        private void ResetTimmer()
        {
            blinkCount = 0;
            TimerStock.Stop();
            TimerStock.Start();
        }
        private void BtnStockSearch_Click(object sender, EventArgs e)
        {
            if (isValidSearchCriteria())
            {
                Cursor.Current = Cursors.WaitCursor;
                RecentPurchases();
                if (SearchStockRequestId != 0)
                {
                    if (this.formIsDirty)
                    {
                        DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                        if (Result == DialogResult.Yes)
                        {
                            if (ValidateForm())
                            {
                                BtnStockMovementSave_Click(sender, e);
                                LoadStockMovementEntry(SearchStockRequestId);
                            }
                        }
                        else if (Result == DialogResult.No)
                        {
                            LoadStockMovementEntry(SearchStockRequestId);
                        }
                    }
                    else
                    {
                        LoadStockMovementEntry(SearchStockRequestId);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private bool isValidSearchCriteria()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            if (string.IsNullOrEmpty(TextBoxStockMovementSearch.Text.Trim()))
            {
                ToolStripStatusLabelErrorPurchase.Text = SearchBoxEmptyErrorMsg;
                TextBoxStockMovementSearch.TextBox.Select();
                return true;
            }
            return true;
        }
        private void RecentPurchases()
        {
            ToolStripStatusLabelErrorPurchase.Text = "";
            String SearchText = TextBoxStockMovementSearch.Text.Trim();
            IList<StockMovementRequest> StockMovementInfo = null;
            if (string.IsNullOrEmpty(SearchText))
            {
                StockMovementInfo = StockMovementManager.GetRecentStockRequests(Global.Company.CompanyId, false);
            }
            else if (DateUtils.ValidDate(SearchText, Global.Company.DateFormat))
            {
                DateTime? Date = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat);
                StockMovementInfo = StockMovementManager.GetStockRequestByDate((DateTime)Date, Global.Company.CompanyId, false);
            }
            else
            {
                StockMovementInfo = StockMovementManager.GetStockRequestByReferenceNo(SearchText, Global.Company.CompanyId, false);
            }

            if (StockMovementInfo.Count > 0)
            {
                LoadStockMovement(StockMovementInfo);
            }
            else
            {
                SearchStockRequestId = 0L;
                ToolStripStatusLabelErrorPurchase.Text = StockMovementSearchOutput;
            }
        }
        public void LoadStockMovement(IList<StockMovementRequest> StockMovementInfo)
        {
            if (StockMovementInfo.Count > 0)
            {
                SearchStockRequestId = 0L;
                FormRecentStockMovement FormRecentStockMovement = new FormRecentStockMovement(this);
                FormRecentStockMovement.StockMovementRequestInfo = StockMovementInfo;
                FormRecentStockMovement.InventoryJournalType = InventoryJournalType.STOCK_REQUEST;
                FormRecentStockMovement.ShowDialog();
            }
            else
            {
                ToolStripStatusLabelErrorPurchase.Text = StockMovementSearchOutput;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalQuantity = 0.00;
            for (int i = 0; i < GridViewStockMovementItem.Rows.Count - 1; i++)
            {
                double Quantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString()));
                double FreeQuantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value.ToString()));
                TotalQuantity += (FreeQuantity + Quantity);
            }
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.VALUE].Value = TotalQuantity;
        }
        private void GridViewStockMovementItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)StockMovementTableColumn.QTY ||
                e.ColumnIndex == (int)StockMovementTableColumn.FREE ||
                e.ColumnIndex == (int)StockMovementTableColumn.PRICE ||
                e.ColumnIndex == (int)StockMovementTableColumn.DISP ||
                e.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                ComputeFormTotal();
            }

            if (e.ColumnIndex == (int)StockMovementTableColumn.BATNO && GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
            {
                LoadBatchDetails((long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value);
            }
        }

        private void GridViewStockMovementItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!GridViewStockMovementItem.CurrentCell.ReadOnly)
            {
                bool IsDirty = this.formIsDirty;
                DirtyFlag(IsDirty);
            }
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.UOM].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.FREE].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.EXPDATE].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRICE].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.COST].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.TAXP].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.TAX].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DISP].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DIS].ReadOnly = true;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.AMOUNT].ReadOnly = true;
            if (GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.UOM].ReadOnly = false;
                GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].ReadOnly = false;
                GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.FREE].ReadOnly = false;
                GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRICE].ReadOnly = false;
                GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.DISP].ReadOnly = false;
                if ((bool)GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                {
                    GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.BATNO].ReadOnly = false;
                    GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRODUCT].ReadOnly = false;
                }
            }
        }
        private void GridViewStockMovementItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.PRODUCT].ReadOnly)
            {
                ToolStripStatusLabelErrorPurchase.Text = string.Empty;
                if (e.ColumnIndex == (int)StockMovementTableColumn.REMOVE && (GridViewStockMovementItem.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value.ToString()), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockMovementItem.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        private void GridViewStockMovementItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewStockMovementItem_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewStockMovementItem.EditingControl).DroppedDown = false;
        }
        private void LoadPrice(int Index, string Uom)
        {
            if (GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    StockMovementProductDetails.LocationId = FromLocationId;
                    if ((bool)GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.ISBAT].Value)
                    {
                        if (GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
                        {
                            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId((long)GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value, FromLocationId);
                            if (InventoryBatch != null)
                            {
                                GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? InventoryBatch.RetailSalePrice : InventoryBatch.WholeSalePrice;
                                StockMovementProductDetails.BatchId = InventoryBatch.Id;
                            }
                        }
                        else
                        {
                            GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                            StockMovementProductDetails.ProductId = Product.Id;
                        }
                    }
                    else
                    {
                        GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = (Product.RetailUOM == Uom) ? Product.RetailPrice : Product.WholdSalePrice;
                        StockMovementProductDetails.ProductId = Product.Id;
                    }
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
                    int Index = GridViewStockMovementItem.CurrentCell.RowIndex;
                    GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                UomColumnComboSelectionChanged(sender, e);
            }
        }
        private void UomColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewStockMovementItem.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.UOM].Value = ((ComboBox)sender).Text;
                LoadPrice(Index, ((ComboBox)sender).Text);
            }
            else
            {
                if (GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.UOM].Value == null)
                {
                    GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
                }
            }

        }
        private void GridViewStockMovementItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl && GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.UOM)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                if (GridViewStockMovementItem.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(UomColumnComboSelectionChanged);
                ((ComboBox)e.Control).TextChanged -= UomColumnComboTextChanged;
                ((ComboBox)e.Control).TextChanged += UomColumnComboTextChanged;
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockMovementItem_KeyPress1);
            }
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockMovementItem, "TaxDetailsNumberChecking", GridViewStockMovementItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockMovementItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockMovementItem_KeyDown;
            }
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, GridViewStockMovementItem, "NameCheckingProduct", GridViewStockMovementItem.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(GridViewStockMovementItem_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += GridViewStockMovementItem_KeyDown;
            }
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= ProductTextChange;
                ((TextBox)e.Control).TextChanged += ProductTextChange;
            }
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
                ((TextBox)e.Control).TextChanged -= BatchTextChange;
                ((TextBox)e.Control).TextChanged += BatchTextChange;
            }
        }
        private void ProductTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified && GridViewStockMovementItem.CurrentCell.ColumnIndex != (int)StockMovementTableColumn.BATNO)
            {
                if (!string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    IList<Product> Product = CatalogProductManager.Instance.GetProductByExactSearchQuery(((TextBox)sender).Text, Global.Company.CompanyId);
                    if (Product.Count > 0)
                    {
                        if (ComboBoxFromLocation.SelectedIndex > 0)
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
                            long CheckForAddRow = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
                            //for tax update purchase
                            if (GridViewStockMovementItem.Rows[GridViewStockMovementItem.CurrentRow.Index].Cells[(int)StockMovementTableColumn.DETAILID].Value != null &&
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
                            GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            if (CheckForAddRow == 0 && GridViewStockMovementItem.Rows.Count - 1 == GridViewStockMovementItem.CurrentRow.Index)
                            {
                                GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                GridViewStockMovementItem.Rows.Add();
                            }
                        }
                        else
                        {
                            MessageBox.Show(SelectFromLocationErrorMsg, "Warning");
                        }
                    }
                    else
                    {
                        ResetProductDetails(GridViewStockMovementItem.CurrentRow.Index);
                        DirtyFlag(IsDirty);
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetProductDetails(GridViewStockMovementItem.CurrentRow.Index);
                }
            }
        }
        private void BatchTextChange(object sender, EventArgs e)
        {
            if (((TextBox)sender).Modified)
            {
                long PId = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
                StockMovementProductDetails.LocationId = FromLocationId;
                if (!string.IsNullOrEmpty(((TextBox)sender).Text) && GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool IsDirty = this.formIsDirty;
                    GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value = ((TextBox)sender).Text;
                    DirtyFlag(IsDirty);
                    InventoryLocation Location = ((InventoryLocation)ComboBoxFromLocation.Items[ComboBoxFromLocation.SelectedIndex]);
                    InventoryBatch BatchDetails = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value, ((TextBox)sender).Text, Location.Id);
                    if (BatchDetails != null)
                    {
                        LoadBatchDetails(BatchDetails.Id);
                        StockMovementProductDetails.BatchId = BatchDetails.Id;
                    }
                    else
                    {
                        ResetBatchDetails();
                        StockMovementProductDetails.ProductId = PId;
                    }
                    GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ResetBatchDetails();
                    if (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null)
                    {
                        StockMovementProductDetails.ProductId = (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value;
                    }
                }
            }
        }
        private void ResetProductDetails(int index)
        {
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.PRODUCT].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.QTY].Value = 0;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.FREE].Value = 0;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.COST].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.TAXP].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DISP].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.AMOUNT].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
            //for tax update purchase
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
            StockMovementProductDetails.Clear();
        }
        private void GridViewStockMovementItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
            }
            if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
            {
                KeypressValidation.Instance.Keypress_NameCheckingProduct(sender, e);
            }
        }
        private void GridViewStockMovementItem_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
                if (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
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
                int index = GridViewStockMovementItem.CurrentRow.Index;
                (GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Clear();
                (GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                (GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.UOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.PRODUCT].Value = Product.Name;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.QTY].Value = 0;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.FREE].Value = 0;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = null;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = Product.PurchasePrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.COST].Value = 0.00;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = Product.RetailPrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = Product.Msrp;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = Product.RetailUOM;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = Product.RetailXFactor;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = Product.WholesaleUOM;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.TAX].Value = 0.00;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DIS].Value = 0.00;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DISP].Value = Product.DefaultDiscount;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.AMOUNT].Value = 0.00;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value = Product.Id;
                if (Product.isInventoryAtBatch != null)
                {
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = Product.isInventoryAtBatch;
                }
                else
                {
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
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
            Cursor.Current = Cursors.WaitCursor;
            bool IsDirty = this.formIsDirty;
            long Check = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
            ProductId = 0L;
            FormSearchItems FormSearchItems = new FormSearchItems(this);
            GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchItems.SearchText = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value != null) ? GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.PRODUCT].Value.ToString() : null;
            FormSearchItems.ShowDialog();
            if (ProductId != 0L)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId);
                if (Product != null)
                {
                    if (GridViewStockMovementItem.Rows[GridViewStockMovementItem.CurrentRow.Index].Cells[(int)StockMovementTableColumn.DETAILID].Value != null && Check == ProductId)
                    {
                        DialogResult Result = MessageBox.Show(ResetItemTaxConfirmText, "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Result == DialogResult.No)
                        {
                            return;
                        }
                    }
                    LoadUomTax(ProductId);
                    LoadProductAdditinalDetails(CatalogProductManager.Instance.GetProductInfoByIdForProductLoad(ProductId));
                    if (Check == 0 && GridViewStockMovementItem.Rows.Count - 1 == GridViewStockMovementItem.CurrentRow.Index)
                    {
                        GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewStockMovementItem.Rows.Add();
                    }
                    GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[2, GridViewStockMovementItem.CurrentRow.Index];
                    GridViewStockMovementItem.CurrentCell.Selected = true;
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
            ComputeFormTotal();
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
            long Check = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value != null) ? (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATCHID].Value : 0L;
            BatchId = 0L;
            FormSearchBatch FormSearchBatch = new FormSearchBatch(this);
            GridViewStockMovementItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSearchBatch.ProductId = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value != null) ? (long)GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.ID].Value : 0L;
            FormSearchBatch.SearchText = (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value != null) ? GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].Value.ToString() : string.Empty;
            FormSearchBatch.ShowDialog();
            if (BatchId != 0)
            {
                LoadBatchDetails(BatchId);
                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, GridViewStockMovementItem.CurrentRow.Index + 1];
                GridViewStockMovementItem.CurrentCell.Selected = true;
            }
            else
            {
                DirtyFlag(IsDirty);
                return;
            }
            ComputeFormTotal();
        }
        private void LoadBatchDetails(long BatId)
        {
            InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryByBatchId(BatId);
            if (InventoryBatch != null)
            {
                int index = GridViewStockMovementItem.CurrentRow.Index;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.BATNO].Value = InventoryBatch.BatchNo;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.BATCHID].Value = InventoryBatch.Id;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.PRICE].Value = InventoryBatch.PurchasePrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = InventoryBatch.RetailSalePrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = InventoryBatch.WholeSalePrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;
                GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
                StockMovementProductDetails.BatchId = InventoryBatch.Id;
            }
            else
            {
                DisplaySystemError("Somthing went wrong, the selected Batch is not valid.");
                return;
            }
        }
        private void ResetBatchDetails()
        {
            int index = GridViewStockMovementItem.CurrentRow.Index;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.EXPDATE].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.BATCHID].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.DETAILID].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = 0.00;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = 1;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = null;
            GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = 1;
            if (GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RETAIL].Value = Product.RetailPrice;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WHOLESALE].Value = Product.WholdSalePrice;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.MSRP].Value = Product.Msrp;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RUOM].Value = Product.RetailUOM;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.RXFACT].Value = Product.RetailXFactor;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WUOM].Value = Product.WholesaleUOM;
                    GridViewStockMovementItem.Rows[index].Cells[(int)StockMovementTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                }
            }
        }
        private void GridViewStockMovementItem_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            bool IsDirty = this.formIsDirty;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.SNO].Value = GridViewStockMovementItem.Rows.Count;
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.QTY].Value = "0.00";
            GridViewStockMovementItem.Rows[e.RowIndex].Cells[(int)StockMovementTableColumn.ISBAT].Value = false;
            DirtyFlag(IsDirty);
        }
        private void FormStockMovement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    DatetimePickerStockMovementDate.Focus();
                    e.Cancel = true;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                if (BtnStockMovementNew.Enabled)
                {
                    BtnStockMovementNew.PerformClick();
                }
            }
            else if (keyData == (Keys.F4))
            {
                BtnStockMovementDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnStockMovementPrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnStockMovementSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                GridViewStockMovementItem.EndEdit();
                BtnStockMovementCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnStockMovementExit.PerformClick();
                return true;
            }
            try
            {
                if (GridViewStockMovementItem.CurrentCell != null)
                {
                    if ((keyData == Keys.F2) && GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
                    {
                        SearchProduct();
                        return true;
                    }
                    if ((keyData == Keys.F2) && !GridViewStockMovementItem.CurrentCell.ReadOnly && GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO)
                    {
                        SearchBatch();
                        return true;
                    }
                    if (keyData == (Keys.Tab) && GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.QTY)
                    {
                        if (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].ReadOnly)
                        {
                            if (GridViewStockMovementItem.CurrentRow.Index == GridViewStockMovementItem.Rows.Count - 1)
                            {
                                BtnStockMovementSave.Select();
                            }
                            else
                            {
                                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, GridViewStockMovementItem.CurrentRow.Index + 1];
                            }
                        }
                    }
                    if (keyData == (Keys.Tab) && (GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.BATNO))
                    {
                        if (GridViewStockMovementItem.CurrentRow.Index != GridViewStockMovementItem.Rows.Count - 1)
                        {
                            GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, GridViewStockMovementItem.CurrentRow.Index + 1];
                            return true;
                        }
                        else
                        {
                            BtnStockMovementSave.Select();
                        }
                    }

                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewStockMovementItem.CurrentCell.ColumnIndex == (int)StockMovementTableColumn.PRODUCT)
                    {
                        if (GridViewStockMovementItem.CurrentRow.Index != 0)
                        {
                            if (GridViewStockMovementItem.CurrentRow.Cells[(int)StockMovementTableColumn.BATNO].ReadOnly)
                            {
                                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.QTY, GridViewStockMovementItem.CurrentRow.Index - 1];
                            }
                            else
                            {
                                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.BATNO, GridViewStockMovementItem.CurrentRow.Index - 1];
                            }
                        }
                        else
                        {
                            if (ComboBoxToLocation.Visible)
                            {
                                ComboBoxToLocation.Focus();
                            }
                            else
                            {
                                BtnStockMovementSave.Select();
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

        private void DisplaySystemError(string Message)
        {
            MessageBox.Show(Message);
            return;
        }
        private void DisplaySystemErrorPerformCancel(string Message)
        {
            MessageBox.Show(Message);
            DirtyFlag(false);
            BtnStockMovementCancel.PerformClick();
            return;
        }

        private void GridViewStockMovementItem_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadProductDetails(e.RowIndex);
            }
        }
        private void LoadProductDetails(int Index)
        {
            Cursor.Current = Cursors.WaitCursor;
            StockMovementProductDetails.EditableStock = 0.00;
            if (GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value != null)
            {
                Product Product = CatalogProductManager.Instance.GetProductInfoByIdForProductLoad((long)GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.ID].Value);
                if (Product != null)
                {
                    LoadProductAdditinalDetails(Product);
                    if (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value != null)
                    {
                        StockMovementProductDetails.BatchId = (long)GridViewStockMovementItem.Rows[Index].Cells[(int)StockMovementTableColumn.BATCHID].Value;
                    }
                }
            }
            else
            {
                StockMovementProductDetails.Clear();
                EnableProductAdditinalDetails();
            }
            Cursor.Current = Cursors.Default;
        }
        private void LoadProductAdditinalDetails(Product Product)
        {
            StockMovementProductDetails.CurrentDate = DatetimePickerStockMovementDate.Date;
            StockMovementProductDetails.LocationId = FromLocationId;
            StockMovementProductDetails.ProductId = Product.Id;
            EnableProductAdditinalDetails();
        }
        private void EnableProductAdditinalDetails()
        {
            StockMovementProductDetails.ReadyOnly = true;
        }
        private void DirtyFlag(bool Enable)
        {
            this.formIsDirty = Enable;
        }
        private void ResetForLocationChange()
        {
            GridViewStockMovementItem.Rows.Clear();
            GridViewStockMovementItem.Rows.Add();
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.NAME].Value = "Total : ";
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
        }
        bool Status = true;
        bool AllowCondition = false;
        private void ComboBoxFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AllowCondition && Status)
            {
                FromLocationIndexChange();
            }
        }
        private void FromLocationIndexChange()
        {
            FromLocationId = ComboBoxFromLocation.SelectedIndex > 0 ? ((InventoryLocation)ComboBoxFromLocation.Items[ComboBoxFromLocation.SelectedIndex]).Id : 0L;
            string ToLocation = ComboBoxToLocation.TextBox.Text;
            ComboUtils.InitializeStockLocationComboWithEmpty(ComboBoxToLocation, Global.Company.CompanyId);
            if (ComboBoxFromLocation.SelectedIndex > 0)
            {
                if (ComboBoxToLocation.FindStringExact(ComboBoxFromLocation.TextBox.Text) > 0)
                {
                    ComboBoxToLocation.Items.RemoveAt(ComboBoxToLocation.FindStringExact(ComboBoxFromLocation.TextBox.Text));
                }
            }
            if (!string.IsNullOrEmpty(ToLocation))
            {
                Status = false;
                ComboBoxToLocation.SelectedIndex = ComboBoxToLocation.FindStringExact(ToLocation);
                Status = true;
            }
        }
        private void ComboBoxToLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToLocationIndexChange();
        }
        private void ComboBoxFromLocation_Enter(object sender, EventArgs e)
        {
            AllowCondition = true;
        }
        private void ToLocationIndexChange()
        {
            if (Status)
            {
                string ToLocation = ComboBoxFromLocation.TextBox.Text;
                ComboUtils.InitializeStockLocationComboWithEmpty(ComboBoxFromLocation, Global.Company.CompanyId);
                if (ComboBoxToLocation.SelectedIndex > 0)
                {
                    if (ComboBoxFromLocation.FindStringExact(ComboBoxToLocation.TextBox.Text) > 0)
                    {
                        ComboBoxFromLocation.Items.RemoveAt(ComboBoxFromLocation.FindStringExact(ComboBoxToLocation.TextBox.Text));
                    }
                }
                if (!string.IsNullOrEmpty(ToLocation))
                {
                    Status = false;
                    ComboBoxFromLocation.SelectedIndex = ComboBoxFromLocation.FindStringExact(ToLocation);
                    Status = true;
                }
            }
        }
        private void ComboBoxFromLocation_TextChanged(object sender, EventArgs e)
        {
            FromLocationIndexChange();
        }
        private void ComboBoxToLocation_TextChanged(object sender, EventArgs e)
        {
            ToLocationIndexChange();
        }
        private void BtnStockMovementSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DatetimePickerStockMovementDate.ReadOnly)
                {
                    GridViewStockMovementItem.Select();
                    GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, 0]; return;
                }
                else
                {
                    DatetimePickerStockMovementDate.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockMovementItem.Select();
                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, 0]; return;
            }
        }
        private void ComboBoxToLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewStockMovementItem.Select();
                GridViewStockMovementItem.CurrentCell = GridViewStockMovementItem[(int)StockMovementTableColumn.PRODUCT, 0];
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxFromLocation.Select();
            }
        }
        private void TextBoxStockMovementSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnStockSearch_Click(sender, e);
            }
        }
        private void BtnStockMovementPrint_Click(object sender, EventArgs e)
        {
            StockMovementPrint StockMovementPrint = new StockMovementPrint();
            StockMovementPrint.PrintStockMovement(long.Parse(TextBoxStockMovementId.Text), EntryType.STOCK_REQUEST);
        }
        private void DatetimePickerStockMovementDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxFromLocation.Select();
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnStockMovementSave.Select();
            }
        }

        private void ComboBoxFromLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxToLocation.Select();
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DatetimePickerStockMovementDate.Focus();
            }
        }

        private void BtnStockRequestComplete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxStockMovementId.Text))
            {
                if (formIsDirty)
                {
                    BtnStockMovementSave_Click(sender, e);
                    CompleteRequest();
                }
                else
                {
                    CompleteRequest();
                }
            }
        }
        private void CompleteRequest()
        {
            StockMovementRequest StockMovementFromDB = StockMovementManager.CompleteStockMovementRequest(long.Parse(TextBoxStockMovementId.Text));
            if (StockMovementFromDB != null)
            {
                EnableForm(false);
                LoadStockMovementEntry(StockMovementFromDB.Id);
                BtnStockMovementPrint.Select();
                ToolStripStatusLabelErrorPurchase.Text = RequestSuccessText;
            }
            DirtyFlag(false);
        }
    }
}
