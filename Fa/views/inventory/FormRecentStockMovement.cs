using fa.api.Hms;
using fa.model.OrderManagement;
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
    public enum RecentStockMovementTableColumn
    {
        DATE, FROMLOCATION, TOLOCATION, REFNO, ID
    }
    public partial class FormRecentStockMovement : Form
    {
        public static string ChooseInvoiceErrorMsg = "Please select {0}";
        public InventoryJournalType InventoryJournalType = InventoryJournalType.STOCK_OUT;
        public IList<StockMovementOut> StockMovementInfo;
        public IList<StockMovementAdjustment> StockMovementAdjustmentInfo;
        public IList<StockMovementDamaged> StockMovementDamageInfo;
        public IList<StockMovementRequest> StockMovementRequestInfo;
        FormBase parent = null;
        public FormRecentStockMovement(Object sender)
        {
            if (sender is FormAdjustInventory)
            {
                parent = (FormAdjustInventory)sender;
            }
            else if (sender is FormIntraStockMovement)
            {
                parent = (FormIntraStockMovement)sender;
            }
            else if (sender is FormDamageEntry)
            {
                parent = (FormDamageEntry)sender;
            }
            else if (sender is FormIntraStockRequest)
            {
                parent = (FormIntraStockRequest)sender;
            }
            InitializeComponent();
        }

        private void FormRecentStockMovement_Load(object sender, EventArgs e)
        {
            if (InventoryJournalType == InventoryJournalType.ADJUSTMENT)
            {
                this.Text = "Recent Stock Adjustement";
            }
            else if (InventoryJournalType == InventoryJournalType.STOCK_OUT)
            {
                this.Text = "Recent Stock Movement";
            }
            else if (InventoryJournalType == InventoryJournalType.STOCK_REQUEST)
            {
                this.Text = "Recent Stock Request";
            }
            else
            {
                this.Text = "Recent Stock Damage";
            }
            LoadStockEntry();
        }

        private void GridViewStockMovementRecentInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                long InvoiceNumber = 0L;
                if (InventoryJournalType == InventoryJournalType.ADJUSTMENT)
                {
                    InvoiceNumber = (long)GridViewStockAdjustment.Rows[e.RowIndex].Cells[(int)RecentStockMovementTableColumn.ID].Value;
                    ((FormAdjustInventory)parent).SearchStockAdjustmentId = InvoiceNumber;
                }
                else if (InventoryJournalType == InventoryJournalType.STOCK_OUT)
                {
                    InvoiceNumber = (long)GridViewStockMovementRecentInvoice.Rows[e.RowIndex].Cells[(int)RecentStockMovementTableColumn.ID].Value;
                    ((FormIntraStockMovement)parent).SearchStockMovementId = InvoiceNumber;
                }
                else if (InventoryJournalType == InventoryJournalType.STOCK_REQUEST)
                {
                    InvoiceNumber = (long)GridViewStockMovementRecentInvoice.Rows[e.RowIndex].Cells[(int)RecentStockMovementTableColumn.ID].Value;
                    if (parent is FormIntraStockMovement)
                    {
                        ((FormIntraStockMovement)parent).SearchStockRequestId = InvoiceNumber;
                    }
                    else
                    {
                        ((FormIntraStockRequest)parent).SearchStockRequestId = InvoiceNumber;
                    }
                }
                else if (InventoryJournalType == InventoryJournalType.DAMAGED)
                {
                    InvoiceNumber = (long)GridViewStockAdjustment.Rows[e.RowIndex].Cells[(int)RecentStockMovementTableColumn.ID].Value;
                    ((FormDamageEntry)parent).SearchStockDamageId = InvoiceNumber;
                }
                this.Close();
            }
        }
        public void LoadStockEntry()
        {
            GridViewStockAdjustment.Visible = false;
            GridViewStockMovementRecentInvoice.Visible = false; ;
            if ((StockMovementInfo != null && StockMovementInfo.Count > 0 && InventoryJournalType == InventoryJournalType.STOCK_OUT))
            {
                GridViewStockMovementRecentInvoice.Visible = true;
                int i = 0;
                GridViewStockMovementRecentInvoice.Rows.Add(StockMovementInfo.Count);
                foreach (var lStockMovementInfo in StockMovementInfo)
                {
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.DATE].Value = lStockMovementInfo.MovementDate.ToString(Global.Company.DateFormat);
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.InventoryStockLocationId);
                    if (Location != null)
                    {
                        GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.FROMLOCATION].Value = Location.Name;
                        Location = null;
                    }
                    Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.InventoryLocationToId);
                    if (Location != null)
                    {
                        GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.TOLOCATION].Value = Location.Name;
                        Location = null;
                    }
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.REFNO].Value = lStockMovementInfo.RefNumber;
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.ID].Value = lStockMovementInfo.Id;
                    i++;
                }
                GridViewStockMovementRecentInvoice.Select();
            }
            else if ((StockMovementRequestInfo != null && StockMovementRequestInfo.Count > 0 && InventoryJournalType == InventoryJournalType.STOCK_REQUEST))
            {
                GridViewStockMovementRecentInvoice.Visible = true;
                int i = 0;
                GridViewStockMovementRecentInvoice.Rows.Add(StockMovementRequestInfo.Count);
                foreach (var lStockMovementInfo in StockMovementRequestInfo)
                {
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.DATE].Value = lStockMovementInfo.MovementDate.ToString(Global.Company.DateFormat);
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.RequestInventoryLocationId);
                    if (Location != null)
                    {
                        GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.FROMLOCATION].Value = Location.Name;
                        Location = null;
                    }
                    Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.InventoryStockLocationId);
                    if (Location != null)
                    {
                        GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.TOLOCATION].Value = Location.Name;
                        Location = null;
                    }
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.REFNO].Value = lStockMovementInfo.RefNumber;
                    GridViewStockMovementRecentInvoice.Rows[i].Cells[(int)RecentStockMovementTableColumn.ID].Value = lStockMovementInfo.Id;
                    i++;
                }
                GridViewStockMovementRecentInvoice.Select();
            }
            else if ((StockMovementAdjustmentInfo != null && StockMovementAdjustmentInfo.Count > 0 && InventoryJournalType == InventoryJournalType.ADJUSTMENT)
                || (StockMovementDamageInfo != null && StockMovementDamageInfo.Count > 0 && InventoryJournalType == InventoryJournalType.DAMAGED))
            {
                GridViewStockAdjustment.Visible = true;
                int i = 0;
                if (InventoryJournalType == InventoryJournalType.ADJUSTMENT)
                {
                    GridViewStockAdjustment.Rows.Add(StockMovementAdjustmentInfo.Count);
                    foreach (var lStockMovementInfo in StockMovementAdjustmentInfo)
                    {
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.DATE].Value = lStockMovementInfo.MovementDate.ToString(Global.Company.DateFormat);
                        InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.InventoryStockLocationId);
                        if (Location != null)
                        {
                            GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.FROMLOCATION].Value = Location.Name;
                            Location = null;
                        }
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.REFNO].Value = lStockMovementInfo.RefNumber;
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.ID].Value = lStockMovementInfo.Id;
                        i++;
                    }
                }
                else
                {
                    GridViewStockAdjustment.Rows.Add(StockMovementDamageInfo.Count);
                    foreach (var lStockMovementInfo in StockMovementDamageInfo)
                    {
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.DATE].Value = lStockMovementInfo.MovementDate.ToString(Global.Company.DateFormat);
                        InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(lStockMovementInfo.InventoryStockLocationId);
                        if (Location != null)
                        {
                            GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.FROMLOCATION].Value = Location.Name;
                            Location = null;
                        }
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.REFNO].Value = lStockMovementInfo.RefNumber;
                        GridViewStockAdjustment.Rows[i].Cells[(int)RecentStockMovementTableColumn.ID].Value = lStockMovementInfo.Id;
                        i++;
                    }
                }
                GridViewStockAdjustment.Select();
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (InventoryJournalType == InventoryJournalType.STOCK_OUT || InventoryJournalType == InventoryJournalType.STOCK_REQUEST)
            {
                if (GridViewStockMovementRecentInvoice.Rows.Count > 0)
                {
                    if (GridViewStockMovementRecentInvoice.CurrentRow.Index > -1)
                    {
                        if (InventoryJournalType == InventoryJournalType.STOCK_OUT)
                        {
                            ((FormIntraStockMovement)parent).SearchStockMovementId = (long)GridViewStockMovementRecentInvoice.CurrentRow.Cells[(int)RecentStockMovementTableColumn.ID].Value;
                        }
                        else
                        {
                            if (parent is FormIntraStockMovement)
                            {
                                ((FormIntraStockMovement)parent).SearchStockRequestId = (long)GridViewStockMovementRecentInvoice.CurrentRow.Cells[(int)RecentStockMovementTableColumn.ID].Value;
                            }
                            else
                            {
                                ((FormIntraStockRequest)parent).SearchStockRequestId = (long)GridViewStockMovementRecentInvoice.CurrentRow.Cells[(int)RecentStockMovementTableColumn.ID].Value;
                            }
                        }
                        this.Close();
                    }
                    else
                    {
                        RecentStockMovementErrorMsg.Text = string.Format(ChooseInvoiceErrorMsg, "stock movement");
                    }
                }
            }
            else
            {
                if (GridViewStockAdjustment.Rows.Count > 0)
                {
                    if (GridViewStockAdjustment.CurrentRow.Index > -1)
                    {
                        if (InventoryJournalType == InventoryJournalType.ADJUSTMENT)
                        {
                            ((FormAdjustInventory)parent).SearchStockAdjustmentId = (long)GridViewStockAdjustment.CurrentRow.Cells[(int)RecentStockMovementTableColumn.ID].Value;
                        }
                        else
                        {
                            ((FormDamageEntry)parent).SearchStockDamageId = (long)GridViewStockAdjustment.CurrentRow.Cells[(int)RecentStockMovementTableColumn.ID].Value;
                        }
                        this.Close();
                    }
                    else
                    {
                        RecentStockMovementErrorMsg.Text = string.Format(ChooseInvoiceErrorMsg, (InventoryJournalType == InventoryJournalType.ADJUSTMENT ? "stock adjustment" : "stock damage"));
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelect.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void GridViewStockMovementRecentInvoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSelect_Click(sender, e);
            }
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
