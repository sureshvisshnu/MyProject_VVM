using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.System;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.views.catalog;
using fa.views.utils.Report.Catalog;
using Fa.api.OrderManagement;
using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using Timer = System.Windows.Forms.Timer;
using ExcelDataReader;
using VisioForge.Libs.ZXing;
using System.Globalization;
using NPOI.SS.Formula.Functions;
using K4os.Compression.LZ4.Internal;
using VisioForge.Libs.MediaFoundation.OPM;

namespace fa.views.controls
{
    enum ItemTableColumn
    {
        CODE, HSN, NAME, PFAMILY, CATEGORY, DESC, PUOM, RUOM, RXFACTOR, WUOM, WXFACTOR, PPRICE, RPRICE, WPRICE, MSRP, COST, ISBATCH, SACCOUNT, PACCOUNT, IACCOUNT, MANUFACTURE, SUPPLIER, ISHSNCODEUSE, GST, CGST, SGST, IGST, DIS, EFROM, ETO
    }
    enum StockTableColumn
    {
        LOCATION, CODE, BATCHNO, EXPDATE, COST, PPRICE, RPRICE, WPRICE, MSRP, SUOM, STOCK
    }
    public partial class FormDataMigrator : Form
    {
        public static string DeleteConfirmText = "The product update is in progress, do you want to cancel?";
        public static string ExitConfirmText = "The product update is in progress, do you want to Exit?";
        public static string SelectExcelFileErrorMsg = "Please choose .xls or .xlsx file only.";
        public static string PrcessingRowText = "Processing row {0} {1} issue in {2}.";
        public static string SelectValidFileErrorMsg = "Please choose valid product file";
        public static string BatchUploadErrorMsg = "Error in stock upload";
        public static string PrcessingSuccessRowText = "Processing row {0} {1}.";
        private string ProductUpdateErrorMsg = "";
        public static string ColoumnValidFileErrorMsg = "Selected File for uploading is mismatched with coloumn header";

        SampleProductImportFile SampleProductImportFiles = new SampleProductImportFile();

        AccountManager AccountManager = null;
        CategoryManager CategoryManager = null;
        CatalogProductFamilyManager CatalogProductFamilyManager = null;
        CatalogProductManager CatalogProductManager = null;
        DataMigratorManager DataMigratorManager = null;
        DataTable TableItemFromExcel = null;
        DataTable TableStockFromExcel = null;

        FormBase parent = null;
        public string FormTitle;
        bool CloseStatus = true;
        bool DataUpdationOS = true;
        string InsertFile;
        public FormDataMigrator(object sender)
        {
            if (sender is FormTaxCode)
            {
                parent = (FormTaxCode)sender;
            }
            else if (sender is FormCatalog)
            {
                parent = (FormCatalog)sender;
            }

            AccountManager = AccountManager.Instance;
            CategoryManager = CategoryManager.Instance;
            CatalogProductFamilyManager = CatalogProductFamilyManager.Instance;
            CatalogProductManager = CatalogProductManager.Instance;
            DataMigratorManager = DataMigratorManager.Instance;
            InitializeComponent();
        }
        private void FormDataMigration_Load(object sender, EventArgs e)
        {
            if (parent is FormTaxCode)
            {
                this.Text = "ItemTax - File Uplod";
            }
            ResetForm();
            LabelTimeTacken.Text = "0.00";
            ErrorMsg.Text = string.Empty;
            InsertProductSampleDownload.Text = "\u2B73";
            InsertAccountSampleDownLoad.Text = "\u2B73";
            BtnChoose.Select();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }
        private void BtnInsertProductFileImport_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadFile();
            TextBoxFileName.Text = InsertFile;
            TextBoxFileName.SelectionStart = TextBoxFileName.Text.Length;
            if (TableItemFromExcel != null)
            {
                BtnUpload.Select();
            }
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            LabelTimeTacken.Text = "0.00";
            TextBoxError.ResetText();
            TextBoxLog.ResetText();
            DataMigrationProgressMax(0);
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
                        TableItemFromExcel = new DataTable();

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

                                if (DataTableCollection.Count > 0)
                                {
                                    TableItemFromExcel = DataTableCollection[0];
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
                    TableItemFromExcel = null;
                    MessageBox.Show(SelectExcelFileErrorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if(parent is FormCatalog) 
            { 
                ((FormCatalog)parent).IsResetCatalog = true; 
            }
            if (!string.IsNullOrEmpty(TextBoxFileName.Text))
            {
                if (TableItemFromExcel != null)
                {
                    LabelTimeTacken.Text = "0.00";
                    DataMigrationTimer = new Timer();
                    DataMigrationTimer.Interval = 1000;
                    DataMigrationTimer.Start();
                    DataMigrationTimer.Tick += new EventHandler(DataMigrationTimer_Tick);
                }
                if (backgroundWorker1.IsBusy != true)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            else
            {
                ErrorMsg.Text = SelectValidFileErrorMsg;
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] CatalogHeader = new string[] { "Product Code[*]", "HSN Code[*]", "SKU Name[*]", "Product Family Name[*]", "Product Category Name[*]", "Product Description", "Purchase UOM[*]", "Retail UOM[*]", "Retail X - Factor[*]", "Wholesale UOM[*]", "Wholesale X - Factor[*]", "Purchase Price[*]", "Retail Price[*]", "Wholesale Price[*]", "MSRP[*]", "Cost[*]", "Is Batch[*]", "Sales Account", "Purchase Account", "Inventory Account", "Manufacture", "Supplier", "Use HSN Code", "GST", "CGST", "SGST", "IGST", "Discount", "EffectiveFromDate", "EffectiveToDate" };
            ErrorMsg.Text = string.Empty;
            BackgroundWorker worker = sender as BackgroundWorker;
            BtnChoose.Invoke(new Action(() => BtnChoose.Enabled = false));
            BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = false));
            BtnExit.Invoke(new Action(() => BtnExit.Enabled = false));
            BtnDownload.Invoke(new Action(() => BtnDownload.Enabled = false));
            BtnCancel.Invoke(new Action(() => BtnCancel.Visible = true));
            TextBoxLog.Invoke(new Action(() => TextBoxLog.Text = ""));
            TextBoxError.Invoke(new Action(() => TextBoxError.Text = ""));
            if (TableItemFromExcel != null)
            {
                int Headercols = TableItemFromExcel.Columns.Count;
                if (Headercols == CatalogHeader.Length)
                {
                    if (TableItemFromExcel != null && TableItemFromExcel.Rows.Count > 0)
                    {
                        int cols = TableItemFromExcel.Columns.Count;
                        int rows = TableItemFromExcel.Rows.Count;
                        int fc = 0;
                        DataMigrationProgressMax(rows);
                        if (rows > 0)
                        {
                            for (int i = 0; i < rows; i++)
                            {
                                try
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    DataMigrationIncrementProgress(1);
                                    var ProductCode = TableItemFromExcel.Rows[i][(int)ItemTableColumn.CODE].ToString();
                                    var HsnCode = TableItemFromExcel.Rows[i][(int)ItemTableColumn.HSN].ToString();
                                    var ProductName = TableItemFromExcel.Rows[i][(int)ItemTableColumn.NAME].ToString();
                                    var ProductFamily = TableItemFromExcel.Rows[i][(int)ItemTableColumn.PFAMILY].ToString();
                                    var ProductCategory = TableItemFromExcel.Rows[i][(int)ItemTableColumn.CATEGORY].ToString();
                                    var PurchaseUMO = TableItemFromExcel.Rows[i][(int)ItemTableColumn.PUOM].ToString();
                                    var RetailUMO = TableItemFromExcel.Rows[i][(int)ItemTableColumn.RUOM].ToString();
                                    var RetailXFactor = TableItemFromExcel.Rows[i][(int)ItemTableColumn.RXFACTOR].ToString();
                                    var WholeSaleUMO = TableItemFromExcel.Rows[i][(int)ItemTableColumn.WUOM].ToString();
                                    var WholeSaleXFactor = TableItemFromExcel.Rows[i][(int)ItemTableColumn.WXFACTOR].ToString();
                                    var IsBatch = TableItemFromExcel.Rows[i][(int)ItemTableColumn.ISBATCH].ToString();

                                    if (ProductName==null || string.IsNullOrEmpty(ProductName.Trim()) || ProductName.Trim().Length > 50)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "product name") + Environment.NewLine)));
                                    }
                                    else if (ProductCode==null || string.IsNullOrEmpty(ProductCode.Trim()) || ProductCode.Trim().Length > 14)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "materialId") + Environment.NewLine)));
                                    }
                                    else if (HsnCode!=null &&  HsnCode.Trim().Length > 14)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "HSN code") + Environment.NewLine)));
                                    }
                                    else if (ProductFamily == null || string.IsNullOrEmpty(ProductFamily.Trim()) || ProductFamily.Trim().Length > 50)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "product family") + Environment.NewLine)));

                                    }
                                    else if (ProductCategory == null || string.IsNullOrEmpty(ProductCategory.Trim()) || ProductCategory.Trim().Length > 50)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "category") + Environment.NewLine)));

                                    }
                                    else if (PurchaseUMO == null || string.IsNullOrEmpty(PurchaseUMO.Trim()) || PurchaseUMO.Trim().Length > 30)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "purchase UOM") + Environment.NewLine)));

                                    }
                                    else if (RetailUMO == null || string.IsNullOrEmpty(RetailUMO.Trim()) || RetailUMO.Trim().Length > 30)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "retail UOM") + Environment.NewLine)));

                                    }
                                    else if (WholeSaleUMO == null || string.IsNullOrEmpty(WholeSaleUMO.Trim()) || WholeSaleUMO.Trim().Length > 30)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "wholesale UOM") + Environment.NewLine)));

                                    }
                                    else if (RetailXFactor == null || string.IsNullOrEmpty(RetailXFactor.Trim()) || !IsNumber(RetailXFactor) || RetailXFactor.Trim().Length > 5)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "retail xfactor") + Environment.NewLine)));

                                    }
                                    else if (WholeSaleXFactor == null || string.IsNullOrEmpty(WholeSaleXFactor.Trim()) || !IsNumber(WholeSaleXFactor) || WholeSaleXFactor.Trim().Length > 5)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "wholesale xfactor") + Environment.NewLine)));

                                    }
                                    else if (IsBatch == null || string.IsNullOrEmpty(IsBatch.Trim()) || !(IsBatch.Trim().ToLower() == "yes" || IsBatch.Trim().ToLower() == "no" || IsBatch.Trim().ToLower() == "true" || IsBatch.Trim().ToLower() == "false"))
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), "Batch") + Environment.NewLine)));

                                    }
                                    else
                                    {
                                        InsertProducts(TableItemFromExcel.Rows[i], CatalogItemType.PRODUCT);
                                        TextBoxError.Invoke(new Action(() => TextBoxLog.AppendText(string.Format(PrcessingSuccessRowText, (i + 1) + ", ", ProductUpdateErrorMsg) + Environment.NewLine)));

                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (TextBoxError.InvokeRequired)
                                    {
                                        TextBoxError.Invoke(new Action(() => TextBoxError.AppendText(string.Format(PrcessingRowText, (i + 1) + ", ", ProductName.Trim(), ex.ToString() + Environment.NewLine) + Environment.NewLine)));
                                    }
                                }
                                finally
                                {

                                }
                            }
                        }
                    }
                }
                else
                {
                    TextBoxError.Invoke(new Action(() => TextBoxLog.AppendText(string.Format("Invalid File ", ": " + ",", ColoumnValidFileErrorMsg) + Environment.NewLine)));
                    Cursor.Current = Cursors.Default;
                    this.UseWaitCursor = false;
                }
                DataMigrationTimer.Stop();
                if (BtnCancel.InvokeRequired) BtnCancel.Invoke(new Action(() => BtnCancel.Visible = false));
                if (BtnChoose.InvokeRequired) BtnChoose.Invoke(new Action(() => BtnChoose.Enabled = true));
                if (BtnUpload.InvokeRequired) BtnUpload.Invoke(new Action(() => BtnUpload.Enabled = true));
                if (BtnDownload.InvokeRequired) BtnDownload.Invoke(new Action(() => BtnDownload.Enabled = true));
                if (BtnExit.InvokeRequired) BtnExit.Invoke(new Action(() => BtnExit.Enabled = true));
                TextBoxError.Invoke(new Action(() => TextBoxLog.AppendText(" - - Process Completed" + Environment.NewLine)));
            }
            else
            {
                ErrorMsg.Text = SelectValidFileErrorMsg;
            }
        }
        public long InsertProducts(DataRow ProductDetails, CatalogItemType CatalogItemType)
        {
            var ProductCode = ProductDetails[(int)ItemTableColumn.CODE].ToString();
            var HsnCode = ProductDetails[(int)ItemTableColumn.HSN].ToString();
            var ProductName = ProductDetails[(int)ItemTableColumn.NAME].ToString().Trim();
            var ProductFamily = ProductDetails[(int)ItemTableColumn.PFAMILY].ToString().Trim();
            var ProductCategory = ProductDetails[(int)ItemTableColumn.CATEGORY].ToString().Trim();
            var ProductDescription = ProductDetails[(int)ItemTableColumn.DESC].ToString();
            if (ProductDescription != null)
            {
                ProductDescription = ReplaceWhiteSpace(ProductDescription);
                ProductDescription = Truncate(ProductDescription, 250);
            }
            var PurchaseUMO = ProductDetails[(int)ItemTableColumn.PUOM].ToString();
            var RetailUMO = ProductDetails[(int)ItemTableColumn.RUOM].ToString();
            var RetailXFactor = ProductDetails[(int)ItemTableColumn.RXFACTOR].ToString();
            var WholeSaleUMO = ProductDetails[(int)ItemTableColumn.WUOM].ToString();
            var WholeSaleXFactor = ProductDetails[(int)ItemTableColumn.WXFACTOR].ToString();
            var PurchasePrice = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.PPRICE].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.PPRICE].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.PPRICE].ToString();
            var RetailPrice = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.RPRICE].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.RPRICE].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.RPRICE].ToString();
            var WholeSalePrice = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.WPRICE].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.WPRICE].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.WPRICE].ToString();
            var MSRP = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.MSRP].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.MSRP].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.MSRP].ToString();
            var Cost = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.COST].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.COST].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.COST].ToString();
            var IsBatch = ProductDetails[(int)ItemTableColumn.ISBATCH].ToString();
            var SalesAc = ProductDetails[(int)ItemTableColumn.SACCOUNT].ToString();
            var PurchaseAc = ProductDetails[(int)ItemTableColumn.PACCOUNT].ToString();
            var InventoryAc = ProductDetails[(int)ItemTableColumn.IACCOUNT].ToString();
            var Manufacture = ProductDetails[(int)ItemTableColumn.MANUFACTURE].ToString();
            var Supplier = ProductDetails[(int)ItemTableColumn.SUPPLIER].ToString();
            var IsUseHSN = ProductDetails[(int)ItemTableColumn.ISHSNCODEUSE].ToString();
            var Dis = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.DIS].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.DIS].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.DIS].ToString();
            var CGST = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.CGST].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.CGST].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.CGST].ToString();
            var SGST = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.SGST].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.SGST].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.SGST].ToString();
            var IGST = string.IsNullOrEmpty(ProductDetails[(int)ItemTableColumn.IGST].ToString()) || !IsNumber(ProductDetails[(int)ItemTableColumn.IGST].ToString()) ? "0.00" : ProductDetails[(int)ItemTableColumn.IGST].ToString();
            var EFrom = ProductDetails[(int)ItemTableColumn.EFROM];
            var ETo = ProductDetails[(int)ItemTableColumn.ETO];
            DateTime EffectiveStartDate = new DateTime(2017, 07, 01);
            DateTime EffectiveEndDate = new DateTime(2400, 12, 31);
            if (EFrom != null && DateTime.TryParse(EFrom.ToString(), out DateTime ESDresult))
            {
                var dateTime = DateUtils.ToDate(ESDresult.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern), CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);
                if (dateTime != null)
                {
                    EffectiveStartDate = (DateTime)dateTime;
                }
            }
            if (ETo != null && DateTime.TryParse(ETo.ToString(), out DateTime EEDresult))
            {
                var dateTime = DateUtils.ToDate(EEDresult.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern), CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);
                if (dateTime != null)
                {
                    EffectiveEndDate = (DateTime)dateTime;
                }
            }
            string formattedEFromDate = EffectiveStartDate.ToString(Global.Company.DateFormat);
            string formattedEToDate = EffectiveEndDate.ToString(Global.Company.DateFormat);
            if (CatalogItemType == CatalogItemType.PRODUCT)
            {
                Product lProduct = new Product();
                lProduct.CompanyId = Global.Company.CompanyId;
                lProduct.Name = ProductName;
                lProduct.Description = ProductDescription;
                lProduct.UOM = PurchaseUMO;
                lProduct.RetailUOM = RetailUMO;
                lProduct.RetailXFactor = RetailXFactor!=null && IsNumber(RetailXFactor.Trim()) ? int.Parse(RetailXFactor.Trim()) : 1;
                lProduct.WholesaleUOM = WholeSaleUMO;
                lProduct.WholesaleXFactor = WholeSaleXFactor!=null &&IsNumber(WholeSaleXFactor.Trim()) ? int.Parse(WholeSaleXFactor.Trim()) : 1;
                lProduct.PurchasePrice = PurchasePrice!=null? float.Parse(PurchasePrice):0;
                lProduct.CostPrice = Cost!=null ? float.Parse(Cost.Trim()) : 0;
                lProduct.RetailPrice = RetailPrice!=null? float.Parse(RetailPrice.Trim()) : 0;
                lProduct.WholdSalePrice = WholeSalePrice!=null?  float.Parse(WholeSalePrice.Trim()) : 0;
                lProduct.Msrp = MSRP!=null? float.Parse(MSRP.ToString()) : 0;
                lProduct.Manufacturer = Manufacture;
                lProduct.SupplierName = Supplier;
                lProduct.MaterialId = ProductCode;
                lProduct.HSNCode = HsnCode;

                bool isUseHSN = IsUseHSN != null && (IsUseHSN.ToLower() == "yes" || IsUseHSN.ToLower() == "true");
                bool isBatch = IsBatch != null && (IsBatch.ToLower() == "yes" || IsBatch.ToLower() == "true");

                lProduct.UseHsnTax = isUseHSN;
                lProduct._isInventoryAtBatch = isBatch;
                
                lProduct._shouldMaintainInventory = true;
                lProduct.Type = CatalogItemType.PRODUCT;
                lProduct.DefaultDiscount = Dis!= null ? float.Parse(Dis.Trim()):0;
                lProduct.CompanyId = Global.Company.CompanyId;
                if (Global.Company.SalesTaxAccountMaps.Count > 0)
                {
                    int i = 1;
                    int Count = Global.Company.SalesTaxAccountMaps.Count;
                    List<CatalogItemSalesTaxMap> SalesTax = new List<CatalogItemSalesTaxMap>();
                    CatalogItemSalesTaxMap CatalogItemSalesTaxMap = null;
                    foreach (CompanySalesTaxAccountMap Map in Global.Company.SalesTaxAccountMaps)
                    {
                        CatalogItemSalesTaxMap = new CatalogItemSalesTaxMap();
                        CatalogItemSalesTaxMap.SalesTaxMapId = Map.MapId;
                        string format = Global.Company.DateFormat.ToString().Trim();
                        bool CheckforDate = DateUtils.TryParseDate(formattedEFromDate, out DateTime EFNewdate, format);
                        if (CheckforDate)
                        {
                            CatalogItemSalesTaxMap.EffectiveFrom = DateTime.Parse(EffectiveStartDate.ToString());
                        }
                        else
                        {
                            CatalogItemSalesTaxMap.EffectiveFrom = EFNewdate;
                        }

                        CheckforDate = DateUtils.TryParseDate(formattedEToDate, out DateTime ETNewdate, format);
                        if (CheckforDate)
                        {
                            CatalogItemSalesTaxMap.EffectiveTo = DateTime.Parse(EffectiveEndDate.ToString()); 
                        }
                        else
                        {
                            CatalogItemSalesTaxMap.EffectiveTo = ETNewdate;
                        }

                        if (i == 1)
                        {
                            CatalogItemSalesTaxMap.TaxPercentage = IGST != null ? float.Parse(IGST) : 0;
                        }
                        if (i == 2)
                        {
                            CatalogItemSalesTaxMap.TaxPercentage = CGST != null ? float.Parse(CGST) : 0;
                        }
                        if (i == 3)
                        {
                            CatalogItemSalesTaxMap.TaxPercentage = SGST != null ? float.Parse(SGST) : 0;
                        }
                        SalesTax.Add(CatalogItemSalesTaxMap);
                        i++;
                    }
                    lProduct.SalesTax = SalesTax;
                }
                if (DataMigratorManager.UpdateProducts(lProduct, ProductCategory, ProductFamily, SalesAc, PurchaseAc, InventoryAc))
                {
                    ProductUpdateErrorMsg = lProduct.Name!= null ? lProduct.Name:string.Empty;
                }
                else
                {
                    ProductUpdateErrorMsg = "Product Name already exists ";
                }
            }
            return 0;
        }
        private bool IsDate(string Date)
        {
            try
            {
                if (!Date.Contains('/'))
                {
                    Date = Date.Replace('-', '/');
                }
                DateUtils.ValidDate(Date, Global.Company.DateFormat);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        private bool IsNumber(string? Number)
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
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                DialogResult Result = MessageBox.Show(DeleteConfirmText, "Confirm",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    CloseStatus = false;
                    backgroundWorker1.CancelAsync();
                    backgroundWorker1.Dispose();
                    this.Close();
                }
            }
        }
        private void FormDataMigrator_FormClosing(object sender, FormClosingEventArgs e)
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
        

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnCancel_MouseEnter(object sender, EventArgs e)
        {
            BtnCancel.Focus();
        }
        private void DataMigrationTimer_Tick(object sender, EventArgs e)
        {
            LabelTimeTacken.Text = (float.Parse(LabelTimeTacken.Text) + 1).ToString("0.00");
        }
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            SampleProductImportFile SampleProductImportFile = new SampleProductImportFile();
            SampleProductImportFile.GenerateCatalogExcelFileViaClosedXML();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public void DataMigrationProgressMax(int max)
        {
            DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.Value = 0));
            DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.Minimum = 0));
            DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.Maximum = max));
        }
        public void DataMigrationProgressAnimat(int animat)
        {
            // Application.EnableVisualStyles();
            DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.Style = ProgressBarStyle.Marquee));
            DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.MarqueeAnimationSpeed = animat));
        }
        public void DataMigrationIncrementProgress(int incrementvalue)
        {
            if (InvokeRequired)
            {
                DataMigrationProgressBar.Invoke(new Action(() => DataMigrationProgressBar.Increment(incrementvalue)));
            }

        }
        public static string ReplaceWhiteSpace(string inputString)
        {
            StringBuilder stringBuild = new StringBuilder();

            if (!string.IsNullOrEmpty(inputString))
            {
                bool IsWhiteSpace = false;
                for (int i = 0; i < inputString.Length; i++)
                {
                    if (char.IsWhiteSpace(inputString[i]))
                    {
                        if (!IsWhiteSpace)
                        {
                            stringBuild.Append(inputString[i]);
                        }
                        IsWhiteSpace = true;
                    }
                    else
                    {
                        IsWhiteSpace = false;
                        stringBuild.Append(inputString[i]);
                    }
                }
            }
            return stringBuild.ToString().Trim();
        }
        public static string? Truncate(string? value, int maxLength)
        {
            if (value != null)
            {
                return value.Length > maxLength ? value.Substring(0, maxLength) : value;
            }
            else
            {
                return null;
            }
        }

        static string IdentifyDateFormat(string dateString)
        {
            string[] dateFormats = {
            "MM-dd-yyyy",
            "dd-MM-yyyy",
            "yyyy-MM-dd",
            "MM-dd-yy",
            "M-d-yyyy",
            "M-d-yy",
            "yy-MM-dd",
            "dd-MMM-yy",
            "MM/dd/yyyy",
            "dd/MM/yyyy",
            "yyyy/MM/dd",
            "M/d/yyyy",
            "M/d/yy",
            "MM/dd/yy",
            "yy/MM/dd",
            "dd/MMM/yy"
        };
            foreach (string format in dateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
                {
                    return format;
                }
            }

            return null;
        }
    }    
}
