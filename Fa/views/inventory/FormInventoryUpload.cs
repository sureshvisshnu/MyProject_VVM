using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using fa;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.System;
using fa.api.utils;
using fa.context;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.utils.Report.Catalog;
using Fa.api.OrderManagement;
using Microsoft.Office.Interop.Excel;
using NPOI.OpenXmlFormats.Dml.Diagram;
using VisioForge.Libs.ZXing;
using Action = System.Action;
using DataTable = System.Data.DataTable;
using Global = fa.Global;
using Timer = System.Windows.Forms.Timer;

namespace Fa.views.inventory
{
    enum StockTableColumn
    {
        LOCATION, CODE, BATCHNO, EXPDATE, COST, PPRICE, RPRICE, WPRICE, MSRP, SUOM, STOCK,STOCKDATE
    }
    public partial class FormInventoryUpload : Form
    {
        public static string DeleteConfirmText = "The product update is in progress, do you want to cancel?";
        public static string ExitConfirmText = "The product update is in progress, do you want to exit?";
        public static string SelectExcelFileErrorMsg = "Please choose .xls or .xlsx file only.";
        public static string PrcessingRowText = "Processing row {0} {1} issue in {2}.";
        public static string SelectValidFileErrorMsg = "Please choose valid product file";
        public static string BatchUploadErrorMsg = "Error in stock upload";
        public static string PrcessingSuccessRowText = "Processing row {0} {1}.";
        public static string HeaderMismatchedErr = "Selected file for uploading is mismatched with coloumn header";

        string InsertFile;
        DataTable TableStockFromExcel = null;
        AccountManager AccountManager = null;
        CategoryManager CategoryManager = null;
        CatalogProductFamilyManager CatalogProductFamilyManager = null;
        CatalogProductManager CatalogProductManager = null;
        DataMigratorManager DataMigratorManager = null;
        DataTable TableItemFromExcel = null;
        bool CloseStatus = true;
        public FormInventoryUpload()
        {
            AccountManager = AccountManager.Instance;
            CategoryManager = CategoryManager.Instance;
            CatalogProductFamilyManager = CatalogProductFamilyManager.Instance;
            CatalogProductManager = CatalogProductManager.Instance;
            DataMigratorManager = DataMigratorManager.Instance;
            InitializeComponent();
        }
        private void FormInventoryUpload_Load(object sender, EventArgs e)
        {
            ResetForm();
            LabelTimeTacken.Text = "0.00";
            ErrorMsgStockUpload.Text = string.Empty;
            BtnBrowse.Select();
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;
        }
        private void ResetForm()
        {
            TextBoxFileName.Text = string.Empty;
            LabelTimeTacken.Text = "0.00";
            textBoxAccessLog.ResetText();
            textBoxErrorLog.ResetText();
            InventoryUploadprogressBarMax(0);
            ErrorMsgStockUpload.Text = string.Empty;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //ResetForm();
            LoadFile();
            TextBoxFileName.Text = InsertFile;
            TextBoxFileName.SelectionStart = TextBoxFileName.Text.Length;
            if (TableStockFromExcel != null)
            {
                BtnUpload.Select();
            }
            Cursor.Current = Cursors.Default;
        }
        DataTableCollection DataTableCollection;
        private void LoadFile()
        {
            InsertFile = string.Empty;
            string FilePath = string.Empty;
            string FileExt = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowHelp = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                FilePath = openFileDialog.FileName;
                FileExt = System.IO.Path.GetExtension(FilePath);
                if (FileExt.CompareTo(".xls") == 0 || FileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        TableStockFromExcel = new DataTable();

                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet Resultset = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                DataTableCollection = Resultset.Tables;
                                int i = 0;
                                foreach (DataTable Table in DataTableCollection)
                                {
                                    if (i == 0) 
                                    {
                                        TableStockFromExcel = Table;
                                        //TableStockFromExcel.Rows.RemoveAt(0);
                                    }
                                    i++;
                                }
                            }
                            InsertFile = openFileDialog.FileName;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    TableStockFromExcel = null;
                    MessageBox.Show(SelectExcelFileErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFileName.Text))
            {
                ErrorMsgStockUpload.Text = "Please choose a file before upload";
                BtnBrowse.Select();
            }
            else if (TableStockFromExcel != null)
            {
                LabelTimeTacken.Text = "0.00";
                InventoryUploadTimer = new Timer();
                InventoryUploadTimer.Interval = 1000;
                InventoryUploadTimer.Start();
                InventoryUploadTimer.Tick += new EventHandler(InventoryUploadTimer_Tick);

                if (BackgroundWorker.IsBusy != true)
                {
                    BackgroundWorker.RunWorkerAsync();
                }
            }
        }
        public void InventoryUploadprogressBarMax(int max)
        {
            InventoryUploadprogressBar.Invoke(new Action(() => InventoryUploadprogressBar.Value = 0));
            InventoryUploadprogressBar.Invoke(new Action(() => InventoryUploadprogressBar.Minimum = 0));
            InventoryUploadprogressBar.Invoke(new Action(() => InventoryUploadprogressBar.Maximum = max));
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] InventoryHeader = new string[] { "Location[*]", "Product Code[*]", "Batch No", "Exp Date", "Cost", "PPrice", "RPrice", "Wprice", "MRP", "Stock UOM[*]", "Stock[*]", "Stock_Date[*]" };
            ErrorMsgStockUpload.Text = string.Empty;
            BackgroundWorker worker = sender as BackgroundWorker;
            BtnBrowse.Invoke(new Action(() => BtnBrowse.Enabled = false));
            BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = false));
            BtnExit.Invoke(new Action(() => BtnExit.Enabled = false));
            BtnDownload.Invoke(new Action(() => BtnDownload.Enabled = false));
            BtnCancel.Invoke(new Action(() => BtnCancel.Visible = true));
            textBoxAccessLog.Invoke(new Action(() => textBoxAccessLog.Text = ""));
            textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.Text = ""));

            if (TableStockFromExcel != null && TableStockFromExcel.Rows.Count > 0 && InventoryHeader.Length == TableStockFromExcel.Columns.Count)
            {
                bool ProcessBegin = true;
                int cols = TableStockFromExcel.Columns.Count;
                int rows = TableStockFromExcel.Rows.Count;
                InventoryUploadprogressBarMax(rows);
                if (rows > 0)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        if (BackgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            InventoryUploadTimer.Stop();
                            return;
                        }
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            InventoryUploadIncrementProgress(1);
                            var Location = TableStockFromExcel.Rows[i][(int)StockTableColumn.LOCATION].ToString();
                            var ProductCode = TableStockFromExcel.Rows[i][(int)StockTableColumn.CODE].ToString();
                            var BatchNo = TableStockFromExcel.Rows[i][(int)StockTableColumn.BATCHNO].ToString();
                            var ExpDate = TableStockFromExcel.Rows[i][(int)StockTableColumn.EXPDATE].ToString();
                            var StockUom = TableStockFromExcel.Rows[i][(int)StockTableColumn.SUOM].ToString();
                            var Stock = TableStockFromExcel.Rows[i][(int)StockTableColumn.STOCK].ToString();
                            var StockDate = TableStockFromExcel.Rows[i][(int)StockTableColumn.STOCKDATE].ToString();

                            if (ProductCode == null || string.IsNullOrEmpty(ProductCode.Trim()) || ProductCode.Trim().Length > 14)
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", "", "materialId"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                                continue;
                            }
                            Product Product = CatalogProductManager.Instance.GetProductByProductCode(ProductCode.Trim(), fa.Global.Company.CompanyId);
                            if (Product == null)
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", "" + ProductCode, "product code. This coded product not available.") + Environment.NewLine)));
                                continue;
                            }
                            else if (Location == null || string.IsNullOrEmpty(Location.Trim()) || Location.Trim().Length > 35)
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "stock location"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else if (Stock == null || string.IsNullOrEmpty(Stock.Trim()) || Stock.Trim().Length > 10 || !IsNumber(Stock.Trim()) || double.Parse(Stock) == 0)
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "stock"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else if (StockUom == null || string.IsNullOrEmpty(StockUom.Trim()) || StockUom.Trim().Length > 30 || !(StockUom == Product.RetailUOM || StockUom == Product.WholesaleUOM))
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "stock UOM"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else if ((StockDate == null || string.IsNullOrEmpty(StockDate.Trim()) || !IsDate(StockDate.Trim())))
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "stock date"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else if (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && (BatchNo == null || string.IsNullOrEmpty(BatchNo.Trim()) || BatchNo.Trim().Length > 10))
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "batch number"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else if (Product._isInventoryAtBatch != null && (bool)Product._isInventoryAtBatch && (ExpDate == null || string.IsNullOrEmpty(ExpDate.Trim()) || !IsDate(ExpDate.Trim())))
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", Product.Name.Trim(), "stock expiry date"))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                            else
                            {
                                InsertProductsBatch(TableStockFromExcel.Rows[i], Product, ProcessBegin);
                                ProcessBegin = false;
                                textBoxErrorLog.Invoke(new Action(() => textBoxAccessLog.AppendText(string.Format(PrcessingSuccessRowText, (i + 1) + ", ", Product.Name.Trim()) + Environment.NewLine)));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (textBoxErrorLog.InvokeRequired)
                            {
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), ex.ToString()))));
                                textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                            }
                        }
                        finally
                        {
                            //break; 
                        }
                    }
                    textBoxErrorLog.Invoke(new Action(() => textBoxErrorLog.AppendText(Environment.NewLine)));
                }
                else
                {
                    ErrorMsgStockUpload.Text = BatchUploadErrorMsg;
                }
                InventoryUploadTimer.Stop();
                if (BtnCancel.InvokeRequired) BtnCancel.Invoke(new Action(() => BtnCancel.Visible = false));
                if (BtnBrowse.InvokeRequired) BtnBrowse.Invoke(new Action(() => BtnBrowse.Enabled = true));
                if (BtnUpload.InvokeRequired) BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));
                if (BtnDownload.InvokeRequired) BtnDownload.Invoke(new Action(() => BtnDownload.Enabled = true));
                if (BtnExit.InvokeRequired) BtnExit.Invoke(new Action(() => BtnExit.Enabled = true));
                textBoxErrorLog.Invoke(new Action(() => textBoxAccessLog.AppendText(" - - Process Completed" + Environment.NewLine)));
            }
            else
            {
                InventoryUploadTimer.Stop();
                if (BtnCancel.InvokeRequired) BtnCancel.Invoke(new Action(() => BtnCancel.Visible = false));
                if (BtnBrowse.InvokeRequired) BtnBrowse.Invoke(new Action(() => BtnBrowse.Enabled = true));
                if (BtnBrowse.InvokeRequired) BtnBrowse.Invoke(new Action(() => BtnBrowse.Select()));
                if (BtnUpload.InvokeRequired) BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));
                if (BtnDownload.InvokeRequired) BtnDownload.Invoke(new Action(() => BtnDownload.Enabled = true));
                if (BtnExit.InvokeRequired) BtnExit.Invoke(new Action(() => BtnExit.Enabled = true));
                textBoxErrorLog.Invoke(new Action(() => textBoxAccessLog.AppendText(" - - Header Mismatched.." + Environment.NewLine)));
                ErrorMsgStockUpload.Text = HeaderMismatchedErr;
            }
        }

        public string Location = string.Empty;
        InventoryLocation? InventoryLocation = null;
        public void InsertProductsBatch(DataRow StockDetail, Product Item, bool IsBegin)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        var lLocation = StockDetail[(int)StockTableColumn.LOCATION].ToString();
                        if (lLocation != null && lLocation != Location)
                        {
                            InventoryLocation = Context.InventoryLocation.FirstOrDefault(x => x.CompanyId == Global.Company.CompanyId && x.Name == lLocation);
                            if (InventoryLocation == null)
                            {
                                InventoryLocation lInventoryLocation = new InventoryLocation();
                                lInventoryLocation.Name = lLocation;
                                lInventoryLocation.Type = InventoryLocationType.GODOWN;
                                lInventoryLocation.CompanyId = Global.Company.CompanyId;
                                Context.InventoryLocation.Add(lInventoryLocation);
                                Context.SaveChanges();
                                InventoryLocation = lInventoryLocation;
                            }
                            Location = InventoryLocation.Name;
                        }
                        if (InventoryLocation != null)
                        {
                            var BatchNo = StockDetail[(int)StockTableColumn.BATCHNO].ToString();
                            var ExpDate = StockDetail[(int)StockTableColumn.EXPDATE].ToString();
                            var StockUom = StockDetail[(int)StockTableColumn.SUOM].ToString();
                            var StockDate = StockDetail[(int)StockTableColumn.STOCKDATE].ToString();
                            var Cost = StockDetail[(int)StockTableColumn.COST].ToString();
                            var PurchasePrice = StockDetail[(int)StockTableColumn.PPRICE].ToString();
                            var RetailPrice = StockDetail[(int)StockTableColumn.RPRICE].ToString();
                            var WholeSalePrice = StockDetail[(int)StockTableColumn.WPRICE].ToString();
                            var MSRP = StockDetail[(int)StockTableColumn.MSRP].ToString();
                            var OpenStock = StockDetail[(int)StockTableColumn.STOCK].ToString();
                            bool IsBatch = Item._isInventoryAtBatch != null && (bool)Item._isInventoryAtBatch ? true : false;

                            StockMovementOpeningStock lStockMovementOpeningStock = new StockMovementOpeningStock();
                            lStockMovementOpeningStock.Id = 0L;
                            lStockMovementOpeningStock.Type = InventoryJournalType.OPEN_STOCK;
                            lStockMovementOpeningStock.MovementDate = string.IsNullOrEmpty(StockDate) ? Global.getTransactionDate().Date : DateTime.Parse(StockDate).Date;
                            lStockMovementOpeningStock.CompanyId = Global.Company.CompanyId;
                            lStockMovementOpeningStock.InventoryStockLocationId = InventoryLocation.Id;
                            StockMovementDetail StockMovementDetail = new StockMovementDetail();
                            StockMovementDetail.CompanyId = Global.Company.CompanyId;
                            StockMovementDetail.Uom = string.IsNullOrEmpty(StockUom) ? string.Empty : StockUom;
                            StockMovementDetail.ProductId = Item.Id;
                            StockMovementDetail.MaterialId = Item.MaterialId;
                            StockMovementDetail.isBatch = IsBatch;
                            StockMovementDetail.ExpDate = string.IsNullOrEmpty(ExpDate) ? DateTime.Now : DateTime.Parse(ExpDate).Date;
                            StockMovementDetail.StockDate = string.IsNullOrEmpty(StockDate) ? DateTime.Now : DateTime.Parse(StockDate).Date;
                            if (StockMovementDetail.isBatch)
                            {
                                StockMovementDetail.BatchNo = BatchNo;
                                StockMovementDetail.ExpDate = string.IsNullOrEmpty(ExpDate) ? DateTime.Now : DateTime.Parse(ExpDate).Date;
                                StockMovementDetail.PurchasePrice = PurchasePrice != null && TextUtils.isAmount(PurchasePrice.Trim()) ? float.Parse(PurchasePrice) : 0;
                                StockMovementDetail.PurchaseCost = Cost != null && TextUtils.isAmount(Cost.Trim()) ? float.Parse(Cost) : 0;
                                StockMovementDetail.Retailprice = RetailPrice != null && TextUtils.isAmount(RetailPrice.Trim()) ? float.Parse(RetailPrice) : 0;
                                StockMovementDetail.Wholesaleprice = WholeSalePrice != null && TextUtils.isAmount(WholeSalePrice.Trim()) ? float.Parse(WholeSalePrice) : 0;
                                StockMovementDetail.Msrp = MSRP != null && TextUtils.isAmount(MSRP.Trim()) ? float.Parse(MSRP) : 0;
                                StockMovementDetail.RetailUOM = Item.RetailUOM;
                                StockMovementDetail.RetailXFactor = Item.RetailXFactor;
                                StockMovementDetail.WholesaleUOM = Item.WholesaleUOM;
                                StockMovementDetail.WholesaleXFactor = Item.WholesaleXFactor;
                            }
                            StockMovementDetail.isFree = false;
                            StockMovementDetail.Quantity = OpenStock != null ? double.Parse(OpenStock) : 0.00;
                            lStockMovementOpeningStock.StockMovementDetails.Add(StockMovementDetail);
                            StockMovementManager.Instance.AddStockMovementOpeningStock(lStockMovementOpeningStock, Item, Global.Company.CompanyId, Context, IsBegin);
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }

        public void InventoryUploadIncrementProgress(int incrementvalue)
        {
            if (InvokeRequired)
            {
                InventoryUploadprogressBar.Invoke(new Action(() => InventoryUploadprogressBar.Increment(incrementvalue)));
            }

        }
        private bool IsNumber(string Number)
        {
            try
            {
                Convert.ToDouble(Number);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        private bool IsDate(string Date)
        {
            try
            {
                if (!Date.Contains('/'))
                {
                    Date = Date.Replace('-', '/');
                }
                DateUtils.ValidDate(Date, fa.Global.Company.DateFormat);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            ErrorMsgStockUpload.Text = string.Empty;
            InventoryExportFile StockDownloadFile = new InventoryExportFile();
            StockDownloadFile.GenerateFile();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (BackgroundWorker.WorkerSupportsCancellation == true && BackgroundWorker.IsBusy)
            {
                DialogResult Result = MessageBox.Show(DeleteConfirmText, "Confirm",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    CloseStatus = false;
                    BackgroundWorker.CancelAsync();
                    BackgroundWorker.Dispose();
                    BtnExit.Enabled = true;
                    BtnExit.Focus();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InventoryUploadTimer_Tick(object sender, EventArgs e)
        {
            LabelTimeTacken.Text = (float.Parse(LabelTimeTacken.Text) + 1).ToString("0.00");
        }

        private void FormInventoryUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseStatus && BtnCancel.Visible)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
