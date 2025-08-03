using fa.api.OrderManagement;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.utils;
using Fa.api.OrderManagement;
using Microsoft.Win32;
using fa.views.utils.QRCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace fa.views.purchase
{
    public enum BarcodePrintGridColumn
    {
        SNO, SELECT_ITEM, ITEM_CODE, ITEM_NAME, BATCH_NO, ITEM_DES, RATE, MRP, P_MRP, QTY, ISBATCH, ID
    }
    public partial class FormPurchaseBarcodePrint : Form
    {
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_EmptyErrorMsg = "Please choose Items for Print";
        public static string ChoosepaperSizeErrorMsg = "Please choose paper Size";
        public static string ChooseLabelSizeErrorMsg = "Please Choose Label Size";

        public long ProductId = 0L;
        public string BatchNo = "";
        public long? PurchaseEntryId = 0L;
        public long? StockMovementId = 0L;

        public FormPurchaseBarcodePrint()
        {
            InitializeComponent();
        }
        private void ResetForm()
        {
            ComboBoxDefaultPrinter.Items.Clear();
            ComboBoxPaperSize.SelectedIndex = 0;
            ComboBoxQRCodeSize.SelectedIndex = -1;
            ComboBoxDefaultPrinter.Items.AddRange(ComboUtils.GetAvailablePrinter().ToArray<string>());
            ComboBoxDefaultPrinter.SelectedIndex = 5;
            if (ComboBoxDefaultPrinter.SelectedIndex > 0 && ComboBoxDefaultPrinter.Items.Count > 0)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VVMApp");
                if (key.GetValue("DefaultPrinter") != null && !string.IsNullOrEmpty(key.GetValue("DefaultPrinter").ToString()))
                {
                    ComboBoxDefaultPrinter.SelectedIndex = ComboBoxDefaultPrinter.FindStringExact(key.GetValue("DefaultPrinter").ToString());
                }
            }
        }
        private void FormPurchaseBarcodePrint_Load(object sender, EventArgs e)
        {
            ResetFormTitle();
            Cursor.Current = Cursors.WaitCursor;
            loadPurchaseDetails();
            ComboBoxPaperSize.Select();
            ComboBoxPaperSize.SelectedIndex = 0;
            ResetForm();
            Cursor.Current = Cursors.Default;
        }
        private void ResetFormTitle()
        {
            if (PurchaseEntryId == 0L)
            {
                this.Text = "Intra Stock Movement Barcode Print";
            }
            else
            {
                this.Text = "Purchase Barcode Print";
            }
        }
        private void loadPurchaseDetails()
        {
            GridViewPurchaseDetails.Rows.Clear();
            if (PurchaseEntryId != 0L)
            {
                IList<PurchaseDetails> PurchaseDetails = null;
                PurchaseEntry PurchaseEntry = PurchaseEntryManager.Instance.GetPurchaseEntry((long)PurchaseEntryId);
                if (PurchaseEntry != null)
                {
                    if (PurchaseEntry.PurchaseDetails != null && PurchaseEntry.PurchaseDetails.Count > 0)
                    {
                        PurchaseDetails = new List<PurchaseDetails>();
                        foreach (PurchaseDetails lPurchaseDetails in PurchaseEntry.PurchaseDetails)
                        {
                            PurchaseDetails OldPurchaseDetails = null;
                            if (lPurchaseDetails.isBatch)
                            {
                                OldPurchaseDetails = PurchaseDetails.FirstOrDefault(x => x.ProductId == lPurchaseDetails.ProductId && x.BatchNo == lPurchaseDetails.BatchNo);
                            }
                            else
                            {
                                OldPurchaseDetails = PurchaseDetails.FirstOrDefault(x => x.ProductId == lPurchaseDetails.ProductId);
                            }

                            if (OldPurchaseDetails != null)
                            {
                                lPurchaseDetails.Quantity += OldPurchaseDetails.Quantity;
                                lPurchaseDetails.FreeQuantity += OldPurchaseDetails.FreeQuantity;

                                PurchaseDetails.Remove(OldPurchaseDetails);
                            }
                            PurchaseDetails.Add(lPurchaseDetails);
                        }

                        GridViewPurchaseDetails.Rows.Add(PurchaseDetails.Count);
                        int rowCount = 0;
                        foreach (PurchaseDetails lPurchaseDetails in PurchaseDetails)
                        {
                            double Rate = Global.Company.BusinessType == BuisnessType.Wholesale ? lPurchaseDetails.Product.WholdSalePrice : lPurchaseDetails.Product.RetailPrice;
                            double Mrp = lPurchaseDetails.Product.Msrp;
                            if (lPurchaseDetails.isBatch)
                            {
                                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lPurchaseDetails.ProductId, lPurchaseDetails.BatchNo, (long)PurchaseEntry.InventoryLocationId);
                                if (InventoryBatch != null)
                                {
                                    Rate = Global.Company.BusinessType == BuisnessType.Wholesale ? InventoryBatch.WholeSalePrice : InventoryBatch.RetailSalePrice;
                                    Mrp = InventoryBatch.MaxRetailPrice;
                                }
                            }
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.SNO].Value = rowCount + 1;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value = true;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value = lPurchaseDetails.MaterialId;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value = lPurchaseDetails.Product.Name;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.BATCH_NO].Value = lPurchaseDetails.BatchNo;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_DES].Value = PurchaseEntry.RefNumber + PurchaseEntry.SupplierName;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.RATE].Value = Rate;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.MRP].Value = Mrp;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.P_MRP].Value = false;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.QTY].Value = (lPurchaseDetails.Quantity + lPurchaseDetails.FreeQuantity);
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ISBATCH].Value = lPurchaseDetails.isBatch;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ID].Value = lPurchaseDetails.Product.Id;
                            rowCount++;
                        }

                    }
                    GridViewPurchaseDetails.Focus();
                    GridViewPurchaseDetails.CurrentCell = GridViewPurchaseDetails[(int)BarcodePrintGridColumn.SELECT_ITEM, 0];
                }
                else
                {
                    MessageBox.Show("something went wrong, please check purchase is still valid");
                    return;
                }
            }
            else if (StockMovementId != 0L)
            {
                IList<StockMovementDetail> StockMovementDetails = null;
                StockMovement StockMovement = StockMovementManager.Instance.GetStockMovementById((long)StockMovementId);
                if (StockMovement != null)
                {
                    if (StockMovement.StockMovementDetails != null && StockMovement.StockMovementDetails.Count > 0)
                    {
                        StockMovementDetails = new List<StockMovementDetail>();
                        foreach (StockMovementDetail lStockMovementDetails in StockMovement.StockMovementDetails)
                        {
                            StockMovementDetail OldStockMovementDetails = null;
                            if (lStockMovementDetails.isBatch)
                            {
                                OldStockMovementDetails = StockMovementDetails.FirstOrDefault(x => x.ProductId == lStockMovementDetails.ProductId && x.BatchNo == lStockMovementDetails.BatchNo);
                            }
                            else
                            {
                                OldStockMovementDetails = StockMovementDetails.FirstOrDefault(x => x.ProductId == lStockMovementDetails.ProductId);
                            }

                            if (OldStockMovementDetails != null)
                            {
                                lStockMovementDetails.Quantity += OldStockMovementDetails.Quantity;
                                lStockMovementDetails.FreeQuantity += OldStockMovementDetails.FreeQuantity;

                                StockMovementDetails.Remove(OldStockMovementDetails);
                            }
                            StockMovementDetails.Add(lStockMovementDetails);
                        }

                        GridViewPurchaseDetails.Rows.Add(StockMovementDetails.Count);
                        int rowCount = 0;
                        foreach (StockMovementDetail lStockMovementDetail in StockMovementDetails)
                        {
                            double Rate = Global.Company.BusinessType == BuisnessType.Wholesale ? lStockMovementDetail.Product.WholdSalePrice : lStockMovementDetail.Product.RetailPrice;
                            double Mrp = lStockMovementDetail.Product.Msrp;
                            if (lStockMovementDetail.isBatch)
                            {
                                InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail((long)lStockMovementDetail.ProductId, lStockMovementDetail.BatchNo, StockMovement.InventoryStockLocationId);
                                if (InventoryBatch != null)
                                {
                                    Rate = Global.Company.BusinessType == BuisnessType.Wholesale ? InventoryBatch.WholeSalePrice : InventoryBatch.RetailSalePrice;
                                    Mrp = InventoryBatch.MaxRetailPrice;
                                }
                            }
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.SNO].Value = rowCount + 1;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value = true;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_CODE].Value = lStockMovementDetail.MaterialId;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_NAME].Value = lStockMovementDetail.Product.Name;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.BATCH_NO].Value = lStockMovementDetail.BatchNo;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ITEM_DES].Value = StockMovement.RefNumber;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.RATE].Value = Rate;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.MRP].Value = Mrp;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.P_MRP].Value = false;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.QTY].Value = (lStockMovementDetail.Quantity + lStockMovementDetail.FreeQuantity);
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ISBATCH].Value = lStockMovementDetail.isBatch;
                            GridViewPurchaseDetails.Rows[rowCount].Cells[(int)BarcodePrintGridColumn.ID].Value = lStockMovementDetail.Product.Id;
                            rowCount++;
                        }

                    }
                    GridViewPurchaseDetails.Focus();
                    GridViewPurchaseDetails.CurrentCell = GridViewPurchaseDetails[(int)BarcodePrintGridColumn.SELECT_ITEM, 0];
                }
                else
                {
                    MessageBox.Show("something went wrong, please check stock movement is still valid");
                    return;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            loadPurchaseDetails();
            ResetForm();
        }
        private void BtnPrint_ItemClickedEvent(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Bar Code")
            {
                if (ValidateCheckedEntry())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (ComboBoxPaperSize.Text == "A4")
                    {
                        SavePrintBarcode SavePrintBarcode = new SavePrintBarcode();
                        SavePrintBarcode.GenerateBarcodeA4(GridViewPurchaseDetails, "A4SheetBarCode", "pdf", true, TextBoxLocation.Text, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "Bar Code" ? LabelType.BARCODE : LabelType.QRCODE);
                    }
                    else
                    {
                        if (ComboBoxLabelSize.Text == "35 mm * 25 mm")
                        {
                            SavePrintBarcode SavePrintBarcode = new SavePrintBarcode();
                            SavePrintBarcode.GenerateBarcodeLabel(GridViewPurchaseDetails, LabelSize.THREE, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "Bar Code" ? LabelType.BARCODE : LabelType.QRCODE);
                        }
                        else if (ComboBoxLabelSize.Text == "50 mm * 25 mm")
                        {
                            SavePrintBarcode SavePrintBarcode = new SavePrintBarcode();
                            SavePrintBarcode.GenerateBarcodeLabel(GridViewPurchaseDetails, LabelSize.TWO, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "Bar Code" ? LabelType.BARCODE : LabelType.QRCODE);
                        }
                        else if (ComboBoxLabelSize.Text == "100 mm* 23 mm")
                        {
                            SavePrintBarcode SavePrintBarcode = new SavePrintBarcode();
                            SavePrintBarcode.GenerateBarcodeLabel(GridViewPurchaseDetails, LabelSize.ONE, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "Bar Code" ? LabelType.BARCODE : LabelType.QRCODE);
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            else if (e.ClickedItem.Text == "QR Code")
            {
                if (ValidateCheckedEntry())
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (ComboBoxPaperSize.Text == "A4")
                    {
                        SavePrintQRCode SavePrintQRCode = new SavePrintQRCode();
                        SavePrintQRCode.GenerateQRcodeA4(GridViewPurchaseDetails, "A4SheetQRCode", "pdf", true, TextBoxLocation.Text, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "QR Code" ? LabelType.QRCODE : LabelType.BARCODE);
                    }
                    else
                    {
                        SavePrintQRCode SavePrintQRCode = new SavePrintQRCode();
                        if (ComboBoxLabelSize.Text == "35 mm * 25 mm")
                        {
                            SavePrintQRCode.GenerateQRcodeLabel(GridViewPurchaseDetails, LabelSize.THREE, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "QR Code" ? LabelType.QRCODE : LabelType.BARCODE);
                        }
                        else if (ComboBoxLabelSize.Text == "50 mm * 25 mm")
                        {
                            SavePrintQRCode.GenerateQRcodeLabel(GridViewPurchaseDetails, LabelSize.TWO, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "QR Code" ? LabelType.QRCODE : LabelType.BARCODE);
                        }
                        else if (ComboBoxLabelSize.Text == "100 mm* 23 mm")
                        {
                            SavePrintQRCode.GenerateQRcodeLabel(GridViewPurchaseDetails, LabelSize.ONE, ComboBoxDefaultPrinter.Text, e.ClickedItem.Text == "QR Code" ? LabelType.QRCODE : LabelType.BARCODE);
                        }
                    }

                }
            }
        }
        private DataGridView CheckedRows()
        {
            DataGridView DataGridView = new DataGridView();
            foreach (DataGridViewRow Checked in GridViewPurchaseDetails.Rows)
            {
                if ((bool)Checked.Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                {
                    DataGridView.Rows.Add(Checked);
                }
            }
            return DataGridView;
        }
        private Boolean ValidateCheckedEntry()
        {
            ErrorMsg.Text = string.Empty;
            if (ComboBoxPaperSize.SelectedIndex < 0)
            {
                ComboBoxPaperSize.Select();
                ErrorMsg.Text = ChoosepaperSizeErrorMsg;
                return false;
            }
            if (ComboBoxQRCodeSize.Visible && ComboBoxQRCodeSize.SelectedIndex < 0)
            {
                ComboBoxQRCodeSize.Select();
                ErrorMsg.Text = "Choose QR Code Size";
                return false;
            }
            if (ComboBoxPaperSize.SelectedIndex == 1 && ComboBoxLabelSize.SelectedIndex < 0)
            {
                ComboBoxLabelSize.Select();
                ErrorMsg.Text = ChooseLabelSizeErrorMsg;
                return false;
            }
            if (ComboBoxDefaultPrinter.SelectedIndex < 0)
            {
                ComboBoxDefaultPrinter.Select();
                ErrorMsg.Text = "Please Choose Printer";
                return false;
            }
            if (ComboBoxPaperSize.SelectedIndex == 0 && string.IsNullOrEmpty(TextBoxLocation.Text))
            {
                TextBoxLocation.Focus();
                ErrorMsg.Text = "Please enter starting location";
                return false;
            }
            Regex regex = new Regex(@"^\d+,\d+$");
            if (!regex.IsMatch(TextBoxLocation.Text))
            {
                TextBoxLocation.Focus();
                ErrorMsg.Text = "Please enter numbers in the format 'x,y' and do not include spaces.";
                return false;
            }
            int Count = GridViewPurchaseDetails.Rows.Count;
            int CheckedRows = 0;
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if ((bool)GridViewPurchaseDetails.Rows[i].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value)
                    {
                        CheckedRows++;
                        for (int j = 3; j < 9; j++)
                        {
                            if (j == (int)BarcodePrintGridColumn.P_MRP || j == (int)BarcodePrintGridColumn.ITEM_DES) { continue; }
                            if (j == (int)BarcodePrintGridColumn.MRP) { continue; }

                            if ((j != 3 && j != 4) && (GridViewPurchaseDetails.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(GridViewPurchaseDetails.Rows[i].Cells[j].Value.ToString()) || float.Parse(GridViewPurchaseDetails.Rows[i].Cells[j].Value.ToString()) <= 0))
                            {
                                GridViewPurchaseDetails.Select();
                                GridViewPurchaseDetails.CurrentCell = GridViewPurchaseDetails[j, i];
                                GridViewPurchaseDetails.BeginEdit(true);
                                ErrorMsg.Text = string.Format(Grid_MantatoryFiledErrorMsg, GridViewPurchaseDetails.Columns[j].HeaderText);
                                return false;
                            }
                        }
                    }
                }
                if (CheckedRows == 0)
                {
                    GridViewPurchaseDetails.Select();
                    GridViewPurchaseDetails.CurrentCell = GridViewPurchaseDetails[(int)BarcodePrintGridColumn.SELECT_ITEM, 0];
                    GridViewPurchaseDetails.BeginEdit(true);
                    ErrorMsg.Text = Grid_EmptyErrorMsg;
                    return false;
                }
            }
            return true;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridViewPurchaseDetails_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.SNO].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.ITEM_CODE].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.BATCH_NO].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.P_MRP].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.ITEM_DES].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.ITEM_NAME].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.MRP].ReadOnly = true;
            GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.RATE].ReadOnly = true;
        }

        private void GridViewPurchaseDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool Checked;
            if (e.ColumnIndex == (int)BarcodePrintGridColumn.SELECT_ITEM && e.RowIndex > -1)
            {
                Checked = (bool)GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value;
                GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.SELECT_ITEM].Value = !Checked;
            }
            if (e.ColumnIndex == (int)BarcodePrintGridColumn.P_MRP && e.RowIndex > -1)
            {
                Checked = (bool)GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.P_MRP].Value;
                GridViewPurchaseDetails.Rows[e.RowIndex].Cells[(int)BarcodePrintGridColumn.P_MRP].Value = !Checked;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                BtnPrint.ShowDropDown();
            }
            else if (keyData == (Keys.Escape))
            {
                GridViewPurchaseDetails.EndEdit();
                BtnCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            try
            {
                if (GridViewPurchaseDetails.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewPurchaseDetails.CurrentCell.ColumnIndex == (int)BarcodePrintGridColumn.QTY)
                    {
                        if (GridViewPurchaseDetails.CurrentCell.RowIndex != GridViewPurchaseDetails.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            BtnPrint.Select();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewPurchaseDetails.CurrentCell.ColumnIndex == (int)BarcodePrintGridColumn.SELECT_ITEM)
                    {
                        if (GridViewPurchaseDetails.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            ComboBoxPaperSize.Focus();
                        }

                    }
                    if (!ComboBoxPaperSize.Focused && GridViewPurchaseDetails.Focused)
                    {

                        if (keyData == (Keys.Tab) && GridViewPurchaseDetails.CurrentCell.ColumnIndex == (int)BarcodePrintGridColumn.SELECT_ITEM)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}");
                        }



                        if (keyData == (Keys.Tab | Keys.Shift) && (GridViewPurchaseDetails.CurrentCell.ColumnIndex == (int)BarcodePrintGridColumn.P_MRP))
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}{tab}");
                        }


                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ComboBoxPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxPaperSize.SelectedIndex == 1)
            {
                LabelLabelSize.Visible = true;
                ComboBoxLabelSize.Visible = true;
                ComboBoxLabelSize.SelectedIndex = -1;
                LabelQRCodeSize.Visible = false;
                ComboBoxQRCodeSize.Visible = false;
                LabelStartLocation.Visible = false;
                TextBoxLocation.Visible = false;
                ComboBoxDefaultPrinter.SelectedIndex = -1;
            }
            else
            {
                LabelLabelSize.Visible = false;
                ComboBoxLabelSize.Visible = false;
                LabelQRCodeSize.Visible = false;
                ComboBoxQRCodeSize.Visible = false;
                foreach (DataGridViewRow row in GridViewPurchaseDetails.Rows)
                {
                    if ((bool)row.Cells[(int)BarcodePrintGridColumn.ISBATCH].Value)
                    {
                        LabelQRCodeSize.Visible = true;
                        ComboBoxQRCodeSize.Visible = true;
                        ComboBoxQRCodeSize.SelectedIndex = -1;
                    }
                }
                LabelStartLocation.Visible = true;
                TextBoxLocation.Visible = true;
                ComboBoxDefaultPrinter.SelectedIndex = -1;
            }
        }

        private void BtnPrint_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxPaperSize.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                GridViewPurchaseDetails.Focus();
                GridViewPurchaseDetails.CurrentCell = GridViewPurchaseDetails[(int)BarcodePrintGridColumn.SELECT_ITEM, 0];
            }
        }
    }
}
