using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using Fa.api.OrderManagement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Catalog;
using fa.api.catalog;
using fa.api.System;
using fa.libraries.utils;
using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.views.controls;

namespace fa.views.inventory
{

    public partial class FormIntraStockReceive : FormBase
    {
        public static string SaveSuccessMsg = "Saved success.";
        public static string EnterReferenceErrorMsg = "Please select valid {0}";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public long LocationId = 0L;
        public long LocationToId = 0L;
        public long StockMovementId = 0L;
        HospitalInventoryManager HospitalInventoryManager = null;
        StockMovementManager StockMovementManager = null;

        public FormIntraStockReceive()
        {
            StockMovementManager = StockMovementManager.Instance;
            HospitalInventoryManager = HospitalInventoryManager.Instance;
            InitializeComponent();
        }
        private void FormIntraStockReceive_Load(object sender, EventArgs e)
        {
            ResetForm();
            GetLocation();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp");
            ComboLocationSelection.SelectedIndex = key != null ? (key.GetValue("StockLocation") != null && !string.IsNullOrEmpty(key.GetValue("StockLocation").ToString())) ? ComboLocationSelection.FindStringExact(key.GetValue("StockLocation").ToString()) : -1 : -1;
            this.ComboLocationSelection.ComboBox.Select();
            ResetDirtyFlag();
        }
        private void BtnStockSearch_Click(object sender, EventArgs e)
        {
            if (ValidateFormForStockReceive())
            {
                ResetForm();
                GetStockOutDetails();
                BtnStockMovementSave.Select();
            }
        }


        private void ResetForm()
        {
            ErrorMsgIntrastockReceive.Text = "";
            StockMovementReferenceNumber.Text = "000000";
            TxtBoxLoctnFrom.ResetText();
            TxtBoxLoctnTo.ResetText();
            DatetimePickerStockMovementDate.Format = Global.Company.DateFormat;
            DatetimePickerStockMovementDate.Date = (DateTime)DateUtils.ToDate(Global.getTransactionDate().ToString(Global.Company.DateFormat), Global.Company.DateFormat);
            GridViewStockMovementItem.Rows.Clear();
            GridViewStockMovementItem.Rows.Add();
            GridViewStockMovementItem.Rows[0].Cells[(int)StockMovementTableColumn.QTY].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewStockMovementItem.Rows[0].Cells[(int)StockMovementTableColumn.FREE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            GridViewStockItemTotal.Rows.Add();
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.NAME].Value = "Total : ";
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.VALUE].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            BtnStockMovementSave.Enabled = false;
            LastStockMovementReferenceNumber.Text = CompanyManager.Instance.GetMovementPrevRef(Global.Company, InventoryJournalType.STOCK_IN, (DateTime)DatetimePickerStockMovementDate.Date);
        }
        private Boolean ValidateFormForStockReceive()
        {
            ErrorMsgIntrastockReceive.Text = "";
            if (ComboLocationSelection.SelectedIndex < 0)
            {
                ErrorMsgIntrastockReceive.Text = string.Format(EnterReferenceErrorMsg, "location");
                GridViewStockMovementItem.Rows.Clear();
                TxtBoxLoctnFrom.ResetText();
                TxtBoxLoctnTo.ResetText();
                ComboLocationSelection.Select();
                return false;
            }
            if (ComboIncomingRefernce.SelectedIndex < 0)
            {
                ErrorMsgIntrastockReceive.Text = string.Format(EnterReferenceErrorMsg, "incoming stock");
                GridViewStockMovementItem.Rows.Clear();
                TxtBoxLoctnFrom.ResetText();
                TxtBoxLoctnTo.ResetText();
                ComboIncomingRefernce.Text = string.Empty;
                ComboIncomingRefernce.Select();
                return false;
            }

            return true;
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        private void GetStockOutDetails()
        {
            GridViewStockMovementItem.Rows.Clear();
            if (ValidateFormForStockReceive())
            {
                InventoryLocation InventoryLocation = (InventoryLocation)ComboLocationSelection.Items[ComboLocationSelection.SelectedIndex];
                if (InventoryLocation != null)
                {
                    LocationToId = InventoryLocation.Id;
                    TxtBoxLoctnTo.Text = InventoryLocation.Name;
                }
                StockMovementOut lStockMovementOut = (StockMovementOut)ComboIncomingRefernce.Items[ComboIncomingRefernce.SelectedIndex];
                if (lStockMovementOut != null)
                {
                    InventoryLocation InventoryToLocation = HospitalInventoryManager.Instance.GetLocationById(lStockMovementOut.InventoryStockLocationId);
                    if (InventoryToLocation != null)
                    {
                        LocationId = InventoryToLocation.Id;
                        TxtBoxLoctnFrom.Text = InventoryToLocation.Name;
                    }
                    StockMovementId = lStockMovementOut.Id;

                    IList<StockMovementDetail> StockReferenceDetails = StockMovementManager.GetStockMovementById(StockMovementId).StockMovementDetails.ToList();
                    if (StockReferenceDetails != null)
                    {
                        int cnt = 0;
                        foreach (StockMovementDetail Details in StockReferenceDetails)
                        {
                            GridViewStockMovementItem.Rows.Add();
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.SNO].Value = cnt + 1;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.PRODUCT].Value = Details.MaterialId;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.UOM].Value = Details.Uom;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.QTY].Value = Details.Quantity;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.FREE].Value = Details.FreeQuantity;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.BATNO].Value = Details.BatchNo;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.EXPDATE].Value = Details.ExpDate;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.ID].Value = Details.ProductId;
                            GridViewStockMovementItem.Rows[cnt].Cells[(int)StockMovementTableColumn.ISBAT].Value = Details.isBatch;
                            cnt++;
                        }
                        GetTotal();
                        BtnStockMovementSave.Enabled = true;
                    }
                }
            }
        }

        private void GetLocationReference()
        {
            ComboIncomingRefernce.Items.Clear();
            InventoryLocation InventoryLocation = HospitalInventoryManager.Instance.GetLocationById(((InventoryLocation)ComboLocationSelection.Items[ComboLocationSelection.SelectedIndex]).Id);
            if (InventoryLocation != null)
            {
                IList<StockMovementOut> LocationReference = StockMovementManager.GetStockMovementByLocation(InventoryLocation.Id, Global.Company.CompanyId);
                if (LocationReference != null)
                {
                    ComboIncomingRefernce.Items.AddRange(LocationReference.ToArray<StockMovementOut>());
                }
                ComboIncomingRefernce.SelectedIndex = -1;
            }
        }
        private void GetLocation()
        {
            ComboUtils.InitializeStockLocationCombo(ComboLocationSelection, Global.Company.CompanyId);
            ComboLocationSelection.SelectedIndex = -1;
        }
        private void BtnStockMovementExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void ComboLocationSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboIncomingRefernce.SelectedIndex = -1;
            ComboIncomingRefernce.Text = string.Empty;
            if (ComboLocationSelection.SelectedIndex > -1)
            {
                GetLocationReference();
            }
            else
            {
                ComboIncomingRefernce.Items.Clear();
            }
        }
        private void GetTotal()
        {
            double TotalQuantity = 0.00;
            for (int i = 0; i < GridViewStockMovementItem.Rows.Count; i++)
            {
                double Quantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value) == null ? 0.00 : (double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.QTY].Value.ToString()));
                double FreeQuantity = (GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value) == null ? 0.00 : (double.Parse(GridViewStockMovementItem.Rows[i].Cells[(int)StockMovementTableColumn.FREE].Value.ToString()));
                TotalQuantity += (FreeQuantity + Quantity);
            }
            GridViewStockItemTotal.Rows[0].Cells[(int)StockMovementTotalTableColumn.VALUE].Value = TotalQuantity;
        }
        private StockMovementIn GetStockMovementInEntry()
        {
            StockMovementIn lStockMovementIn = new StockMovementIn();
            lStockMovementIn.MovementDate = Global.getTransactionDate();
            lStockMovementIn.CompanyId = Global.Company.CompanyId;
            lStockMovementIn.InventoryStockLocationId = LocationToId;
            lStockMovementIn.InventoryLocationFromId = LocationId;
            lStockMovementIn.StockMovementOutId = StockMovementId;
            if (Global.CostCenter != null)
            {
                lStockMovementIn.CostCenterId = Global.CostCenter.CostCenterId;
            }
            for (int i = 0; i < GridViewStockMovementItem.Rows.Count; i++)
            {
                StockMovementDetail StockInMovementDetail = new StockMovementDetail();
                Product Product = CatalogProductManager.Instance.GetProductInfoById((long)GridViewStockMovementItem.Rows[i].Cells[15].Value);
                if (Product != null)
                {
                    StockInMovementDetail.Uom = GridViewStockMovementItem.Rows[i].Cells[2].Value.ToString(); ;
                    StockInMovementDetail.CompanyId = Global.Company.CompanyId;
                    StockInMovementDetail.ProductId = Product.Id;
                    StockInMovementDetail.MaterialId = Product.MaterialId;
                    StockInMovementDetail.isBatch = (bool)GridViewStockMovementItem.Rows[i].Cells[16].Value;
                    StockInMovementDetail.ExpDate = DateTime.Now.Date;
                    if (StockInMovementDetail.isBatch)
                    {
                        StockInMovementDetail.BatchNo = GridViewStockMovementItem.Rows[i].Cells[5].Value.ToString();
                        StockInMovementDetail.ExpDate = (DateTime)GridViewStockMovementItem.Rows[i].Cells[6].Value;
                    }
                    double Quantity = (GridViewStockMovementItem.Rows[i].Cells[3].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[3].Value.ToString());
                    double FreeQuantity = (GridViewStockMovementItem.Rows[i].Cells[4].Value == null) ? 0.00 : double.Parse(GridViewStockMovementItem.Rows[i].Cells[4].Value.ToString());
                    if (FreeQuantity > 0)
                    {
                        StockInMovementDetail.isFree = true;
                    }
                    else
                    {
                        StockInMovementDetail.isFree = false;
                    }
                    StockInMovementDetail.Quantity = Quantity;
                    StockInMovementDetail.FreeQuantity = FreeQuantity;
                    lStockMovementIn.StockMovementDetails.Add(StockInMovementDetail);
                }
            }
            return lStockMovementIn;
        }
        private void BtnStockMovementCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            LocationId = 0L;
            LocationToId = 0L;
            StockMovementId = 0L;
            ComboIncomingRefernce.Text = "";
            ComboLocationSelection.Text = "";
            ResetForm();
            GetLocation();
            ResetDirtyFlag();
            ComboLocationSelection.Focus();
        }
        private void BtnStockMovementSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StockMovementIn lStockMovementIn = GetStockMovementInEntry();
                StockMovementIn lStockMovementFromDB = null;
                if (lStockMovementIn.Id == 0)
                {
                    string RefNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.STOCK_IN, lStockMovementIn.MovementDate);
                    if (!string.IsNullOrEmpty(RefNumber))
                    {
                        lStockMovementIn.RefNumber = RefNumber;
                        lStockMovementFromDB = new StockMovementIn();
                        try
                        {
                            lStockMovementFromDB = StockMovementManager.Instance.AddStockMovementIn(lStockMovementIn);
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
                    StockMovementOut StockMovementInfo = StockMovementManager.Instance.GetStockMovementOut(lStockMovementIn.Id);
                    if (StockMovementInfo != null)
                    {
                        lStockMovementFromDB = new StockMovementIn();
                        lStockMovementFromDB = StockMovementManager.Instance.UpdateStockMovementIn(lStockMovementIn);
                    }
                    else
                    {
                        return;
                    }
                }
                StockMovementReferenceNumber.Text = lStockMovementFromDB.RefNumber;
                if (lStockMovementFromDB != null)
                {
                    BtnStockMovementSave.Enabled = false;
                    ComboLocationSelection.ComboBox.Select();
                    ErrorMsgIntrastockReceive.Text = SaveSuccessMsg;
                }
                ClearForm();
                ResetDirtyFlag();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnStockMovementSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnStockMovementCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnStockMovementExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnStockMovementSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboLocationSelection.ComboBox.Select();
                return;
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnStockSearch.Select();
            }
        }
    }
}
