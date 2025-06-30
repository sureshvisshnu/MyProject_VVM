using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.OrderManagement;
using fa.views.hms;
using fa.views.sales;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.purchase
{
    public enum SearchBatchTableColumn
    {
        BATCHNO, EXPDATE, QTY, UOM, LOCATION, MRP, ID
    }
    public partial class FormSearchBatch : Form
    {
        public static string Grid_ItemBatchExpire = "Please select valid {0},This batch item is expired.";
        public static string SearchOutput = "No Batch found!";
        public static string NoBatchfoundErrorMsg = "You do not have any batch, Please add a batch!";
        public static string ChooseBatchErrorMsg = "Please choose Batch";
        public static string EnterBatchNumberErrorMsg = "Please enter batch Number";
        public static string DeletedBatchErrorMsg = "Your Select Batch is Remove, Please press Go button then select the Batch";
        public long LocationId = 0L;
        public string SearchText;
        public long ProductId;
        FormBase parent = null;
        public FormSearchBatch(object sender)
        {
            parent = (FormBase)sender;
            InitializeComponent();
        }
        private void FormSearchBatch_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (!string.IsNullOrEmpty(SearchText))
            {
                TextBoxSearchBatch.Text = SearchText;
                BtnSearchBatch_Click(sender, e);
                TextBoxSearchBatch.TextBox.Select();
                return;
            }
            loadDefaultBatch();
            TextBoxSearchBatch.TextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnSearchBatch_Click(object sender, EventArgs e)
        {
            BatchSearchErrorMsg.Text = "";
            if (!string.IsNullOrEmpty(TextBoxSearchBatch.Text))
            {
                loadDefaultBatch();
            }
            else
            {
                BatchSearchErrorMsg.Text = EnterBatchNumberErrorMsg;
            }
        }

        private void loadDefaultBatch()
        {
            IList<InventoryBatch> InventoryBatch = null;
            if (LocationId != 0L)
            {
                InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetailbySearchText(ProductId, TextBoxSearchBatch.Text, LocationId);
            }
            else
            {
                InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetailbySearchText(ProductId, TextBoxSearchBatch.Text);
            }
            if (InventoryBatch != null && InventoryBatch.Count > 0)
            {
                LoadInventoryBatch(InventoryBatch);
            }
            else
            {
                GridViewBatch.Rows.Clear();
                BatchSearchErrorMsg.Text = NoBatchfoundErrorMsg;
            }
        }
        private void LoadInventoryBatch(IList<InventoryBatch> InventoryBatch)
        {
            GridViewBatch.Rows.Clear();
            BatchSearchErrorMsg.Text = "";
            BtnSearchSelect.Enabled = false;
            if (InventoryBatch.Count > 0)
            {
                int i = 0;
                foreach (InventoryBatch lInventoryBatch in InventoryBatch)
                {
                    if (((lInventoryBatch.StockDate != null && lInventoryBatch.StockDate.Value.Date <= Global.getTransactionDate().Date ? lInventoryBatch.OpeningStock : 0) + lInventoryBatch.QuantityOnHand) > 0 && lInventoryBatch.BatchNo != null)
                    {
                        if(lInventoryBatch.BatchNo != "")
                        {
                            InventoryBatch InventoryBatchFDB = InventoryLocationManager.Instance.GetInventoryByBatchIdWithLocation(lInventoryBatch.Id, LocationId);
                            i = GridViewBatch.Rows.Add();
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.BATCHNO].Value = lInventoryBatch.BatchNo;
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.EXPDATE].Value = api.utils.DateUtils.FormatDate(lInventoryBatch.ExpDate, Global.Company.DateFormat);
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.QTY].Value = LocationId == 0L ? ((lInventoryBatch.StockDate != null && lInventoryBatch.StockDate <= Global.getTransactionDate().Date ? lInventoryBatch.OpeningStock : 0) + lInventoryBatch.QuantityOnHand) : InventoryBatchFDB == null ? 0 : ((InventoryBatchFDB.StockDate != null && InventoryBatchFDB.StockDate.Value.Date <= Global.getTransactionDate().Date ? InventoryBatchFDB.OpeningStock : 0) + InventoryBatchFDB.QuantityOnHand);
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.UOM].Value = lInventoryBatch.WholesaleUOM;
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.LOCATION].Value = lInventoryBatch.Inventory.InventoryLocation.Name;
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.MRP].Value = lInventoryBatch.MaxRetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewBatch.Rows[i].Cells[(int)SearchBatchTableColumn.ID].Value = lInventoryBatch.Id;
                            i++;
                        }
                    }
                }
                BtnSearchSelect.Enabled = true;
            }
            else
            {
                BatchSearchErrorMsg.Text = SearchOutput;
            }
        }
        private void ResetForm()
        {
            TextBoxSearchBatch.TextBox.ResetText();
            BtnSearchSelect.Enabled = false;
            loadDefaultBatch();
        }

        private void BtnSearchSelect_Click(object sender, EventArgs e)
        {
            if (GridViewBatch.Rows.Count > 0)
            {
                if (GridViewBatch.CurrentRow.Index > -1)
                {
                    if ((parent is FormItembasedSales || parent is FormPurchaseEntryNew) && ((DateTime)api.utils.DateUtils.ToDate(GridViewBatch.CurrentRow.Cells[(int)SearchBatchTableColumn.EXPDATE].Value.ToString(), Global.Company.DateFormat)) < ((DateTime)Global.getTransactionDate().Date))
                    {
                        BatchSearchErrorMsg.Text = string.Format(Grid_ItemBatchExpire, "Batch");
                    }
                    else
                    {
                        parent.ProductBatchIdTransport.ResetText();
                        parent.ProductBatchIdTransport.Text = GridViewBatch.CurrentRow.Cells[(int)SearchBatchTableColumn.ID].Value.ToString();
                        this.Close();
                    }
                }
                else
                {
                    BatchSearchErrorMsg.Text = ChooseBatchErrorMsg;
                }
            }
        }

        private void GridViewBatch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnSearchSelect_Click(sender, e);
            }
        }

        private void TextBoxSearchBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchBatch_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewBatch.Rows.Count > 0)
                {
                    GridViewBatch.Select();
                    GridViewBatch.CurrentCell = GridViewBatch[0, 0];
                }
            }
        }

        private void TextBoxSearchBatch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxSearchBatch.Text))
            {
                loadDefaultBatch();
            }
        }

        private void GridViewBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Convert.ToChar(Keys.Enter))
            {
                BtnSearchSelect_Click(sender, e);
            }
        }
        private bool GridFocus = false;
        private void GridViewBatch_Enter(object sender, EventArgs e)
        {
            GridFocus = true;
        }

        private void GridViewBatch_Leave(object sender, EventArgs e)
        {
            GridFocus = false;
        }

        private void BtnSearchSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxSearchBatch.TextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewBatch.Rows.Count > 0)
                {
                    GridViewBatch.Select();
                    GridViewBatch.CurrentCell = GridViewBatch[0, 0];
                }
                else
                {
                    TextBoxSearchBatch.TextBox.Select();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSearchSelect.PerformClick();
            }

            else if (keyData == (Keys.Escape))
            {
                BtnSearchCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridFocus)
                {
                    if (keyData == (Keys.Tab) && GridViewBatch.CurrentRow.Index > -1)
                    {
                        if (GridViewBatch.CurrentCell.RowIndex != GridViewBatch.Rows.Count - 1)
                        {
                            GridViewBatch.CurrentCell = GridViewBatch[0, GridViewBatch.CurrentCell.RowIndex + 1];
                        }
                        else
                        {
                            BtnSearchSelect.Select();
                        }

                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewBatch.CurrentRow.Index > -1)
                    {
                        if (GridViewBatch.CurrentRow.Index != 0)
                        {
                            GridViewBatch.CurrentCell = GridViewBatch[0, GridViewBatch.CurrentCell.RowIndex - 1];
                        }
                        else
                        {
                            TextBoxSearchBatch.TextBox.Select();
                        }

                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
