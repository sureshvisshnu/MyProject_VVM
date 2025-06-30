using fa.api.catalog;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.libraries.Validation;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.controls.grid;
using Fa.api.OrderManagement;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace fa.views.catalog
{
    public enum BatchEntryTableColumn
    {
        SNO, LOCATION, QTY, SUOM,ASOF, BATCHNO, EXPDATE, PPRICE, COST, RPRICE, WPRICE, MSRP, RUOM, RXFACT, WUOM, WXFACT, QRCODE, REMOVE, INVBATCHID, INVID, DETAILID, LID
    }
    public partial class FormCatalogBatchEntry : FormBase
    {
        public static string SaveSuccessText = "Saved success...";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want Exit?";
        public static string Grid_NotAllowRowDeleteText = "Do Not Delete Row {0} this batch item Sold out";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string Grid_EmptyErrorMsg = "Please Enter Quantity";
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string EnterQtyErrorMsg = "Please Enter valid Quantity";
        public static string EnterValidQtyErrorMsg = "Please Check the entry, Quantity will not match Opening Stock..";
        public static string EnterExpDateErrorMsg = "Please Enter Exp date";
        public static string EnterAsofDateErrorMsg = "Please Enter As of date";
        public static string EnterMsrpErrorMsg = "Please Enter Correct Msrp";
        public static string Grid_LocationRepeatedErrorMsg = "Stock Location {0} Repeated.";
        public string ProductId { get; set; }
        public double Quantity { get; set; }
        public bool EditMode { get; set; }
        public bool IsBatch { get; set; }
        FormCatalog Parent = null;
        public FormCatalogBatchEntry(object Sender)
        {
            Parent = (FormCatalog)Sender;
            InitializeComponent();
        }
        private void FormCatalogBatchEntry_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (String.IsNullOrEmpty(ProductId))
            {
                BatchGrid.Enabled = false;
                BtnSave.Enabled = false;
            }
            else
            {
                this.Text = "Opening stock for " + CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId)).Name;
                LoadBatches();
                PurchaseDetails PurchaseDetails = PurchaseEntryManager.Instance.GetPurchaseDetailsByProductId(long.Parse(ProductId));
                SaleDetail SaleDetail = SalesManager.Instance.GetSaleDetailByProductId(long.Parse(ProductId));
                BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                {
                    BatchGrid.CurrentCell = BatchGrid[1, 0];
                    BatchGrid.BeginEdit(true);
                }));
            }
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void LoadBatches()
        {
            BatchGrid.Rows.Clear();
            Product ProductInfoFromDB = CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId));
            if (ProductInfoFromDB != null)
            {
                IList<StockMovementDetail> StockMovementInfo = StockMovementManager.Instance.GetOpeningStockDetails(Global.Company.CompanyId, ProductInfoFromDB.Id);
                if (StockMovementInfo != null && StockMovementInfo.Count > 0)
                {
                    int i = 0;
                    foreach (StockMovementDetail detail in StockMovementInfo)
                    {
                        StockMovement stockMovement = StockMovementManager.Instance.GetStockMovementById((long)detail.StockMovementId);
                        if (stockMovement != null)
                        {
                            BatchGrid.Rows.Add(1);
                            LoadGridCombo(i);
                            Inventory Inventory = InventoryLocationManager.Instance.GetInventoryByProductId(ProductInfoFromDB.Id, stockMovement.InventoryStockLocationId);
                            if (Inventory != null)
                            {
                                if (!IsBatch || (IsBatch && ProductInfoFromDB._isInventoryAtBatch != null && !(bool)ProductInfoFromDB._isInventoryAtBatch))
                                {
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Clear();
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(ProductInfoFromDB.RetailUOM);
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(ProductInfoFromDB.WholesaleUOM);
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value = detail.Uom;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SNO].Value = i + 1;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION].Value = stockMovement.InventoryStockLocationId;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LID].Value = stockMovement.InventoryStockLocationId;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value = detail.Quantity;
                                    if (Inventory.StockDate != null)
                                    {
                                        BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.ASOF].Value = Inventory.StockDate;
                                    }
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.DETAILID].Value = detail.Id;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QRCODE].Value = "Print";
                                }
                                else
                                {
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Clear();
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(ProductInfoFromDB.RetailUOM);
                                    (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(ProductInfoFromDB.WholesaleUOM);
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value = detail.Uom;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SNO].Value = i + 1;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION].Value = stockMovement.InventoryStockLocationId;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LID].Value = stockMovement.InventoryStockLocationId;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value = detail.Quantity;
                                    if (Inventory.StockDate != null)
                                    {
                                        BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.ASOF].Value = Inventory.StockDate;
                                    }
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.DETAILID].Value = detail.Id;
                                    BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QRCODE].Value = "Print";
                                    if (IsBatch)
                                    {
                                        InventoryBatch InventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetail(ProductInfoFromDB.Id, detail.BatchNo, stockMovement.InventoryStockLocationId);
                                        if (InventoryBatch != null)
                                        {
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.BATCHNO].Value = InventoryBatch.BatchNo;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.EXPDATE].Value = InventoryBatch.ExpDate;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RUOM].Value = InventoryBatch.RetailUOM;
                                            if (InventoryBatch.StockDate != null)
                                            {
                                                BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.ASOF].Value = InventoryBatch.StockDate;
                                            }
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RXFACT].Value = InventoryBatch.RetailXFactor;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WUOM].Value = InventoryBatch.WholesaleUOM;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WXFACT].Value = InventoryBatch.WholesaleXFactor;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.PPRICE].Value = InventoryBatch.PurchasePrice;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.COST].Value = InventoryBatch.Cost;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RPRICE].Value = InventoryBatch.RetailSalePrice;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WPRICE].Value = InventoryBatch.WholeSalePrice;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value = InventoryBatch.MaxRetailPrice;
                                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.INVBATCHID].Value = InventoryBatch.Id;
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            ReSequence();
        }
        private void LoadGridCombo(int i)
        {
            IList<InventoryLocation> Location = HospitalInventoryManager.Instance.ListAllInventoryLocation(Global.Company.CompanyId);
            if (Location != null && Location.Count > 0)
            {
                (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION] as DataGridViewComboBoxCell).DataSource = null;
                (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION] as DataGridViewComboBoxCell).DataSource = Location;
                (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION] as DataGridViewComboBoxCell).ValueMember = "Id";
                (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION] as DataGridViewComboBoxCell).DisplayMember = "Name";
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    BatchGrid.CurrentCell = BatchGrid[1, 0];
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadBatches();
            DirtyFlag(false);
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            TextBoxOpeningStock.Text = Math.Round(0.00, Global.Company.QuantityPricision).ToString();
            BatchGrid.Rows.Clear();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)BatchGrid.Columns["PurchasePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            DataGridViewCurrencyColumn currencyColumn1 = (DataGridViewCurrencyColumn)BatchGrid.Columns["Cost"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces1)) currencyColumn1.DecimalPlaces = decimalPlaces1;
            DataGridViewCurrencyColumn currencyColumn2 = (DataGridViewCurrencyColumn)BatchGrid.Columns["RetailPrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces2)) currencyColumn2.DecimalPlaces = decimalPlaces2;
            DataGridViewCurrencyColumn currencyColumn3 = (DataGridViewCurrencyColumn)BatchGrid.Columns["WholeSalePrice"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces3)) currencyColumn3.DecimalPlaces = decimalPlaces3;
            DataGridViewCurrencyColumn currencyColumn4 = (DataGridViewCurrencyColumn)BatchGrid.Columns["Msrp"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces4)) currencyColumn4.DecimalPlaces = decimalPlaces4;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                if (CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId)) == null)
                {
                    MessageBox.Show("Somthing went wrong, the selected product is not valid.");
                    return;
                }
                Product ProductInfoFromDB = CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId));
                IList<StockMovementOpeningStock> StockMovementOpeningStock = new List<StockMovementOpeningStock>();
                for (int i = 0; i < BatchGrid.Rows.Count - 1; i++)
                {
                    InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(long.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString()));
                    StockMovementOpeningStock lStockMovementOpeningStock = new StockMovementOpeningStock();
                    lStockMovementOpeningStock.Id = 0L;
                    lStockMovementOpeningStock.Type = InventoryJournalType.OPEN_STOCK;
                    lStockMovementOpeningStock.MovementDate = Global.getTransactionDate().Date;
                    lStockMovementOpeningStock.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        lStockMovementOpeningStock.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    if (Location != null)
                    {
                        lStockMovementOpeningStock.InventoryStockLocationId = Location.Id;
                    }
                    double NewQty = double.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value.ToString());
                    string SUom = BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString();
                    StockMovementDetail StockMovementDetail = new StockMovementDetail();
                    StockMovementDetail.CompanyId = Global.Company.CompanyId;
                    if (Global.CostCenter != null)
                    {
                        StockMovementDetail.CostCenterId = Global.CostCenter.CostCenterId;
                    }
                    StockMovementDetail.Uom = SUom;
                    StockMovementDetail.ProductId = ProductInfoFromDB.Id;
                    StockMovementDetail.MaterialId = ProductInfoFromDB.MaterialId;
                    StockMovementDetail.isBatch = IsBatch;
                    StockMovementDetail.ExpDate = DateTime.Now.Date;
                    StockMovementDetail.StockDate = (DateTime)BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.ASOF].Value;
                    if (StockMovementDetail.isBatch)
                    {
                        StockMovementDetail.BatchNo = BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.BATCHNO].Value.ToString();
                        StockMovementDetail.ExpDate = (DateTime)BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.EXPDATE].Value;
                        StockMovementDetail.PurchasePrice = float.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.PPRICE].Value.ToString());
                        StockMovementDetail.PurchaseCost = float.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.COST].Value.ToString());
                        StockMovementDetail.Retailprice = float.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RPRICE].Value.ToString());
                        StockMovementDetail.Wholesaleprice = float.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WPRICE].Value.ToString());
                        StockMovementDetail.Msrp = float.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value.ToString());
                        StockMovementDetail.RetailUOM = BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RUOM].Value.ToString();
                        StockMovementDetail.RetailXFactor = int.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RXFACT].Value.ToString());
                        StockMovementDetail.WholesaleUOM = BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WUOM].Value.ToString();
                        StockMovementDetail.WholesaleXFactor = int.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.WXFACT].Value.ToString());
                    }
                    StockMovementDetail.isFree = false;
                    StockMovementDetail.Quantity = NewQty;
                    lStockMovementOpeningStock.StockMovementDetails.Add(StockMovementDetail);
                    StockMovementOpeningStock.Add(lStockMovementOpeningStock);
                }
                StockMovementManager.Instance.AddStockMovementOpeningStock(StockMovementOpeningStock, ProductInfoFromDB, Global.Company.CompanyId);
                LoadBatches();
                Cursor.Current = Cursors.Default;
                ErrorMsg.Text = SaveSuccessText;
                DirtyFlag(false);
            }
        }
        private bool ValidateForm()
        {
            ErrorMsg.Text = string.Empty;
            if(BatchGrid.Rows.Count != 0)
            {
                for (int i = 0; i < BatchGrid.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        if (!IsBatch && j > 3) { continue; }
                        if (BatchGrid.Rows[i].Cells[j].Value == null || string.IsNullOrEmpty(BatchGrid.Rows[i].Cells[j].Value.ToString().Trim()))
                        {
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[j, i];
                            BatchGrid.BeginEdit(true);
                            ErrorMsg.Text = string.Format(Grid_MantatoryFiledErrorMsg, j == 1 ? "Quantity" : BatchGrid.Columns[j].HeaderText);
                            return false;
                        }
                        if ((j == (int)BatchEntryTableColumn.QTY) && double.Parse(BatchGrid.Rows[i].Cells[j].Value.ToString()) == 0)
                        {
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.QTY, i];
                            BatchGrid.BeginEdit(true);
                            ErrorMsg.Text = EnterQtyErrorMsg;
                            return false;
                        }
                        if (j == (int)BatchEntryTableColumn.ASOF && (BatchGrid.Rows[i].Cells[j].Value == null))
                        {
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.ASOF, i];
                            BatchGrid.BeginEdit(true);
                            ErrorMsg.Text = EnterAsofDateErrorMsg;
                            return false;
                        }
                        if (j == (int)BatchEntryTableColumn.EXPDATE && (BatchGrid.Rows[i].Cells[j].Value == null || ((DateTime)BatchGrid.Rows[i].Cells[j].Value).Date < Global.getTransactionDate().Date))
                        {
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.EXPDATE, i];
                            BatchGrid.BeginEdit(true);
                            ErrorMsg.Text = EnterExpDateErrorMsg;
                            return false;
                        }
                        if ((BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RPRICE].Value != null && !string.IsNullOrEmpty(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RPRICE].Value.ToString().Trim()) &&
                            BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value != null && !string.IsNullOrEmpty(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value.ToString().Trim())) &&
                            (double.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value.ToString()) != 0 && double.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.MSRP].Value.ToString()) < double.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.RPRICE].Value.ToString())))
                        {
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.MSRP, i];
                            BatchGrid.BeginEdit(true);
                            ErrorMsg.Text = EnterMsrpErrorMsg;
                            return false;
                        }
                    }
                }
                for (int i = 0; i < BatchGrid.Rows.Count - 1; i++)
                {
                    int RentPeriod = 0;
                    for (int k = 0; k < BatchGrid.Rows.Count - 1; k++)
                    {
                        if (BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.LOCATION].Value != null)
                        {
                            if ((!IsBatch && BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString() == BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString()
                                && BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString() == BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString())
                                || (IsBatch && BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString() == BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString()
                                && BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.BATCHNO].Value.ToString() == BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.BATCHNO].Value.ToString()
                                && BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString() == BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString()))
                            {
                                RentPeriod++;
                            }
                        }
                        if (RentPeriod > 1)
                        {
                            InventoryLocation Location = HospitalInventoryManager.Instance.GetLocationById(long.Parse(BatchGrid.Rows[k].Cells[(int)BatchEntryTableColumn.LOCATION].Value.ToString()));
                            if (Location != null)
                            {
                                ErrorMsg.Text = string.Format(Grid_LocationRepeatedErrorMsg, Location.Name);
                            }
                            else
                            {
                                ErrorMsg.Text = string.Format(Grid_LocationRepeatedErrorMsg, "");
                            }
                            BatchGrid.Select();
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.LOCATION, k];
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private void BatchGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ErrorMsg.Text = string.Empty;
                if (e.ColumnIndex == (int)BatchEntryTableColumn.REMOVE && (BatchGrid.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.SNO].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        BatchGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        BatchGrid.Rows.RemoveAt(e.RowIndex);
                        ReSequence();
                    }
                }

            }
            if (e.RowIndex > -1 && e.RowIndex != BatchGrid.Rows.Count - 1 && e.ColumnIndex == (int)BatchEntryTableColumn.QRCODE)
            {
                if (BatchGrid.CurrentRow.Cells[(int)BatchEntryTableColumn.BATCHNO].Value != null && !BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QRCODE].ReadOnly)
                {
                    FormCatalogQRCodePrint FormCatalogQRCodePrint = new FormCatalogQRCodePrint();
                    FormCatalogQRCodePrint.ProductId = long.Parse(ProductId);
                    FormCatalogQRCodePrint.BatchNo = BatchGrid.CurrentRow.Cells[(int)BatchEntryTableColumn.BATCHNO].Value.ToString();
                    FormCatalogQRCodePrint.Show();
                }
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < BatchGrid.Rows.Count; i++)
            {
                BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SNO].Value = i + 1;
            }
            TotalQty();
        }
        private void TotalQty()
        {
            double Qty = 0.00;
            Product ProductInfoFromDB = CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId));
            for (int i = 0; i < BatchGrid.Rows.Count; i++)
            {
                if (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value != null && !string.IsNullOrEmpty(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value.ToString()))
                {
                    double lQty = double.Parse(BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.QTY].Value.ToString());
                    if (BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value != null)
                    {
                        Qty += ProductInfoFromDB.RetailUOM == BatchGrid.Rows[i].Cells[(int)BatchEntryTableColumn.SUOM].Value.ToString() ? ((lQty / ProductInfoFromDB.RetailXFactor)) : ((lQty / ProductInfoFromDB.WholesaleXFactor));
                    }
                }
            }
            TextBoxOpeningStock.Text = Math.Round(Qty, Global.Company.QuantityPricision).ToString();
        }
        private void BatchGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                LoadGridCombo(e.RowIndex);
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QTY].Value = Math.Round(0.00, Global.Company.QuantityPricision);
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.SNO].Value = BatchGrid.Rows.Count;
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.ASOF].Value = null;
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.EXPDATE].Value = null;
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QRCODE].Value = null;
            }
        }
        private void DirtyFlag(bool Dirty)
        {
            this.formIsDirty = Dirty;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
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
                if (!IsBatch && keyData == (Keys.Tab) && (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.ASOF))
                {
                    BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.QRCODE, (BatchGrid.CurrentCell.RowIndex)];
                        BatchGrid.BeginEdit(true);
                    }));
                }
                if (keyData == (Keys.Tab) && (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.MSRP))
                {
                    BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.QRCODE, (BatchGrid.CurrentCell.RowIndex)];
                        BatchGrid.BeginEdit(true);
                    }));
                }
                if (keyData == (Keys.Tab) && (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.QRCODE))
                {
                    if (BatchGrid.CurrentCell.RowIndex != BatchGrid.Rows.Count - 1)
                    {
                        BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                        {
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.LOCATION, (BatchGrid.CurrentCell.RowIndex + 1)];
                            BatchGrid.BeginEdit(true);
                        }));

                    }
                    else
                    {
                        BtnSave.Select();
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.LOCATION)
                {
                    if (BatchGrid.CurrentRow.Index != 0)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                    else
                    {
                        BtnSave.Focus();
                        return true;
                    }
                }
                if (keyData == (Keys.Tab | Keys.Shift) && BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.QRCODE)
                {
                    if (!IsBatch)
                    {
                        BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                        {
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.ASOF, (BatchGrid.CurrentCell.RowIndex)];
                            BatchGrid.BeginEdit(true);
                        }));
                    }
                    else
                    {
                        BatchGrid.BeginInvoke(new MethodInvoker(delegate ()
                        {
                            BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.MSRP, (BatchGrid.CurrentCell.RowIndex)];
                            BatchGrid.BeginEdit(true);
                        }));
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
        private void BatchGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            BatchGrid.ReadOnly = false;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QTY].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.SNO].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.SUOM].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.ASOF].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.EXPDATE].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.RUOM].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.RXFACT].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.WUOM].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.WXFACT].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.PPRICE].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.COST].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.RPRICE].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.WPRICE].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.MSRP].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.REMOVE].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.BATCHNO].ReadOnly = true;
            BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.LOCATION].ReadOnly = false;
            if (BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.LOCATION].Value != null)
            {
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QTY].ReadOnly = false;
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.SUOM].ReadOnly = false;
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.ASOF].ReadOnly = false;

                if (IsBatch)
                {
                    BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.BATCHNO].ReadOnly = false;
                    if (BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.BATCHNO].Value != null)
                    {
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.EXPDATE].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QTY].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.PPRICE].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.COST].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.RPRICE].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.WPRICE].ReadOnly = false;
                        BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.MSRP].ReadOnly = false;
                    }
                }
            }
            if (IsBatch && BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.INVBATCHID].Value == null)
            {
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QRCODE].ReadOnly = true;
            }
            else
            {
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.QRCODE].ReadOnly = false;
            }
        }
        private void FormCatalogBatchEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        DataGridViewEditingControlShowingEventArgs BackupContextMenuStrip = null;
        private void BatchGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (BackupContextMenuStrip == null)
            {
                BackupContextMenuStrip = e;
            }
            if (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.BATCHNO)
            {
                KeypressValidation.Instance.AddContextMenuGridCell(e, BatchGrid, "TaxDetailsNumberChecking", BatchGrid.CurrentCell.ColumnIndex);
                e.Control.KeyPress += new KeyPressEventHandler(BatchGrid_KeyPress);
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown += BatchGrid_KeyDown;
                ((TextBox)e.Control).CharacterCasing = CharacterCasing.Upper;
            }
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                if (BatchGrid.CurrentCell.Value == null || BatchGrid.CurrentCell.RowIndex == BatchGrid.Rows.Count - 1)
                { ((ComboBox)e.Control).SelectedIndex = -1; }
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).FormattingEnabled = true;
                if (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.LOCATION)
                {
                    ((ComboBox)e.Control).TextChanged -= LocationColumnComboSelectionChanged;
                    ((ComboBox)e.Control).TextChanged += LocationColumnComboSelectionChanged;
                    ((ComboBox)e.Control).TextChanged -= LocationColumnComboTextChanged;
                    ((ComboBox)e.Control).TextChanged += LocationColumnComboTextChanged;
                    e.Control.KeyPress += new KeyPressEventHandler(BatchGrid_KeyPress1);
                }
            }
            else
            {
                e.Control.ContextMenuStrip = BackupContextMenuStrip.Control.ContextMenuStrip;
            }
        }
        private void LocationColumnComboTextChanged(object sender, EventArgs e)
        {
            if ((int)BatchEntryTableColumn.LOCATION == BatchGrid.CurrentCell.ColumnIndex)
            {
                if (((ComboBox)sender).SelectedIndex == -1)
                {
                    if (!string.IsNullOrEmpty(((ComboBox)sender).Text))
                    {
                        int index = ((ComboBox)sender).FindStringExact(((ComboBox)sender).Text);
                        if (index != -1)
                        {
                            DataGridViewComboBoxEditingControl ck = (DataGridViewComboBoxEditingControl)sender;
                            LocationColumnComboSelectionChanged(ck, e);
                            return;
                        }
                        else
                        {
                            int Index = BatchGrid.CurrentCell.RowIndex;
                            ResetBachDeails(Index);
                        }
                    }
                }
                else
                {
                    LocationColumnComboSelectionChanged(sender, e);
                    return;
                }
            }
        }
        private void LocationColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = BatchGrid.CurrentCell.RowIndex;
            if ((int)BatchEntryTableColumn.LOCATION == BatchGrid.CurrentCell.ColumnIndex)
            {
                if (((ComboBox)sender).SelectedIndex > -1)
                {
                    BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.LOCATION].Value = ((InventoryLocation)((ComboBox)sender).SelectedItem).Id;
                    BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.LID].Value = ((InventoryLocation)((ComboBox)sender).SelectedItem).Id;
                    if (!string.IsNullOrEmpty(ProductId))
                    {
                        bool IsDirty = this.formIsDirty;
                        Product Product = CatalogProductManager.Instance.GetProductInfoById(long.Parse(ProductId));
                        if (Product != null)
                        {
                            (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.EXPDATE] as CalendarCell).MinDate = Global.getTransactionDate();
                            (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Clear();
                            (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(Product.RetailUOM);
                            (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.SUOM] as DataGridViewComboBoxCell).Items.Add(Product.WholesaleUOM);
                            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.RUOM].Value = Product.RetailUOM;
                            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.RXFACT].Value = Product.RetailXFactor;
                            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.WUOM].Value = Product.WholesaleUOM;
                            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.WXFACT].Value = Product.WholesaleXFactor;
                            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.QRCODE].Value = "Print";
                            DirtyFlag(IsDirty);
                        }
                    }
                }
                else
                {
                    if (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.LOCATION].Value == null && IsBatch)
                    {
                        ResetBachDeails(Index);
                    }
                }
            }
        }
        private void ResetBachDeails(int Index)
        {
            (BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.EXPDATE] as CalendarCell).MinDate = Global.getTransactionDate();
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.EXPDATE].Value = null;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.RUOM].Value = string.Empty;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.RXFACT].Value = 0;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.WUOM].Value = string.Empty;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.WXFACT].Value = 0;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.COST].Value = 0.00;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.PPRICE].Value = 0.00;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.WPRICE].Value = 0.00;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.RPRICE].Value = 0.00;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.MSRP].Value = 0.00;
            BatchGrid.Rows[Index].Cells[(int)BatchEntryTableColumn.LID].Value = null;
        }
        private void BatchGrid_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)BatchGrid.EditingControl).DroppedDown = false;
        }
        private void BatchGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            BatchGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.BATCHNO)
            {
                KeypressValidation.Instance.Keypress_TaxDetailsNumberChecking(sender, e);
            }
            if (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.RUOM ||
                BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.WUOM)
            {
                KeypressValidation.Instance.Keypress_NameChecking(sender, e);
            }
        }
        private void BatchGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (BatchGrid.CurrentCell.ColumnIndex == (int)BatchEntryTableColumn.BATCHNO)
                {
                    KeypressValidation.Instance.Keypress_PasteChecking(sender, e, "TaxDetailsNumberChecking");
                }
            }
        }
        private void BatchGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)BatchEntryTableColumn.QTY || e.ColumnIndex == (int)BatchEntryTableColumn.SUOM)
            {
                TotalQty();
            }
            if (e.ColumnIndex == (int)BatchEntryTableColumn.LOCATION && BatchGrid.CurrentCell.RowIndex == BatchGrid.Rows.Count - 1)
            {
                BatchGrid.Rows[e.RowIndex].Cells[(int)BatchEntryTableColumn.LOCATION].Value = null;
            }
        }
        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BatchGrid.Select();
                BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.LOCATION, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BatchGrid.Select();
                BatchGrid.CurrentCell = BatchGrid[(int)BatchEntryTableColumn.QRCODE, BatchGrid.Rows.Count - 1];
            }
        }
        private void BatchGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
