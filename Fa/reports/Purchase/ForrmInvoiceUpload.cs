using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDataReader;
using fa;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.libraries.utils;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.views.purchase;
using Fa.views.controls.grid;
using Fa.views.purchase;
using FADataAccessLibrary.Api.OrderManagement;
using FADataAccessLibrary.Model.Purchase;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.reports.Purchase
{
    public partial class ForrmInvoiceUpload : Form
    {
        private readonly string[] _invoiceHeaders = new string[]
        {
            "SNO", "PRODUCT", "UOM", "QTY", "FREE", "BATNO",
            "EXPDATE", "PRICE", "COST", "TAXP", "TAX", "DISP", "DIS", "AMOUNT"
        };

        private ProgressBar progressBarInvoiceUpload;
        private Dictionary<string, string> _columnMappings = new Dictionary<string, string>();
        private readonly string[] _InvoiceTransactionHeader = { "PRODUCT", "MaterialId", "UOM", "CatalogProductName", "CatalogUOM", "QTY", "FREE", "BATNO", "EXPDATE", "PRICE", "COST", "TAXP", "TAX", "DISP", "DIS", "AMOUNT" };
        private readonly string[] _InvoiceHeaders = { "Supplier", "Address", "InvoiceNo", "InvoiceDate", "PaymentMethod", "Buyers GST No" };
        private readonly string[] _InvoiceFooters = { "TotalAmount" };

        public int productColumnIndex = -1;
        public int uomColumnIndex = -1;

        public long SupplierId = 0;

        private bool validateTemplate = true;
        private bool validateProductMapping = true;
        public class HeaderInfo
        {
            public string Name { get; set; } = "";
            public char Type { get; set; }
            public override string ToString() => Name;
        }

        private Dictionary<string, string> MappHeader = new();
        private Dictionary<string, string> MappTransaction = new();
        private Dictionary<string, string> MappFooter = new();

        private DataGridView GridViewProductRectification;

        private List<HeaderInfo> allHeaders;

        private List<HeaderInfo> allHeadHeaders;
        private List<HeaderInfo> allTransHeaders;
        private List<HeaderInfo> allFootHeaders;

        private Dictionary<string, string> _headerControlMap = new Dictionary<string, string>
        {
            { "Supplier", "TextBoxPurchaseEntrySupplier"},
            { "SupplierId", "TextBoxPurchaseEntrySupplier"},
            { "Address", "TextBoxPurchaseEntryAddress" },
            { "InvoiceNo", "TextBoxPurcahaseInvoiceNo"},
            { "InvoiceDate", "DatetimePickerPurchaseInvoiceDate" },
            { "PaymentMethod", "YesNoRbtPurchaseMethod" }
        };

        private Dictionary<string, string> _footerControlMap = new Dictionary<string, string>
        {
            { "TotalAmount", "LabelPurchaseEntryFinalAmount" }
        };

        private DataTable? _sourceData;
        private Dictionary<int, bool> _columnSelections = new Dictionary<int, bool>();

        public bool FileHasHeader;

        public static string EnterDefaultInvalidErrorMsg = "Invalid file type. Please select a valid Excel or CSV file.";
        public static string EnterInvalidFileErrorMsg = "No data loaded. Please select a file first.";
        public static string SelectColumnErrorMsg = "Please select all column needed for Invoice .";
        public static string EnterNameErrorMsg = "Template Name could not be empty, please enter Name";
        public static string TmpltSaveSuccessMsg = "Template Saved successfully";
        public static string DeleteSuccessMsg = "Deleted successfully";
        public static string TmpltAssignErrorMsg = "Missing : Headers are not assigned properly";
        public static string TmpltCreationErrorMsg = "Template Name already Exists";
        public static string TmpltSupplierErrorMsg = "Supplier is invalid.";
        public static string TmpltPrdctMapingErrorMsg = "Product is not mapped properly.";

        private Dictionary<string, string> _headerMappings = new Dictionary<string, string>();
        private Dictionary<string, string> _transactionMappings = new Dictionary<string, string>();
        private Dictionary<string, string> _footerMappings = new Dictionary<string, string>();

        private bool _isMergedFile;

        private readonly FormPurchaseEntryNew _parentForm;

        public ForrmInvoiceUpload(FormPurchaseEntryNew parentForm)
        {
            InitializeComponent(); // This must be called first
            _parentForm = parentForm;

            DataViewInvoiceSelecter.AutoGenerateColumns = false;
            DataViewInvoiceSelecter.ColumnAdded += DataViewInvoiceSelecter_ColumnAdded!;
            DataViewInvoiceSelecter.ColumnHeaderMouseClick += DataViewInvoiceSelecter_ColumnHeaderMouseClick!;
            DataViewInvoiceSelecter.CellMouseClick += DataViewInvoiceSelecter_CellMouseClick!;
            DataViewInvoiceSelecter.CellMouseDown += DataViewInvoiceSelecter_CellMouseDown!;
            DataViewInvoiceSelecter.SelectionChanged += DataViewInvoiceSelecter_SelectionChanged!;
        }

        public void ResetForm()
        {
            ToolStripStatusLabelErrorInvoice.Text = "";
            TextBoxFileName.Text = "";
            comboBoxSwapTextBoxAvailabelTemplet.SelectedIndex = -1;
        }
        public void ResetForTemplate()
        {
            ToolStripStatusLabelErrorInvoice.Text = "";
            comboBoxSwapTextBoxAvailabelTemplet.SelectedIndex = -1;
            TextBoxInvoiceTemplateId.Text = string.Empty;
        }
        public void EnableButton(int butonclick)
        {
            if (butonclick == 0)
            {
                BtnChoose.Enabled = true;
                BtnReset.Enabled = false;
                BtnUpload.Enabled = false;
            }
            else if (butonclick == 1)
            {
                BtnChoose.Enabled = true;
                BtnReset.Enabled = true;
                BtnUpload.Enabled = false;
            }
            else if (butonclick == 2)
            {
                BtnChoose.Enabled = true;
                BtnReset.Enabled = true;
                BtnUpload.Enabled = true;
            }
        }

        private void ConfigureFooterGridView(DataTable footerData)
        {
            DataViewInvoiceFooterSelecter.Columns.Clear();
            DataViewInvoiceFooterSelecter.AutoGenerateColumns = false;
            DataViewInvoiceFooterSelecter.AllowUserToAddRows = false;

            foreach (DataColumn column in footerData.Columns)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    Name = column.ColumnName,
                    HeaderText = column.ColumnName,
                    DataPropertyName = column.ColumnName,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataViewInvoiceFooterSelecter.Columns.Add(textColumn);
            }

            DataRow mappingRow = footerData.NewRow();
            footerData.Rows.InsertAt(mappingRow, 0);

            DataViewInvoiceFooterSelecter.DataSource = footerData;

            if (DataViewInvoiceFooterSelecter.Rows.Count > 0)
            {
                DataGridViewRow firstRow = DataViewInvoiceFooterSelecter.Rows[0];
                foreach (DataGridViewColumn column in DataViewInvoiceFooterSelecter.Columns)
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = _InvoiceFooters.ToList(),
                        Value = _InvoiceFooters.FirstOrDefault()
                    };
                    firstRow.Cells[column.Index] = comboCell;
                }
                firstRow.ReadOnly = false;
            }

            foreach (DataGridViewRow row in DataViewInvoiceFooterSelecter.Rows)
            {
                if (row.Index > 0)
                {
                    row.ReadOnly = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewComboBoxCell comboCell)
                        {
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        }
                    }
                }
            }

            DataViewInvoiceFooterSelecter.CurrentCellDirtyStateChanged += HandleFooterGridCommit;
            DataViewInvoiceFooterSelecter.CellValueChanged += HandleFooterGridValueChange;
            DataViewInvoiceFooterSelecter.DataError += (senderObj, errorArgs) =>
            {
                errorArgs.ThrowException = false;
            };
        }

        // Event handlers for footer grid
        private void HandleFooterGridCommit(object? sender, EventArgs e)
        {
            if (DataViewInvoiceFooterSelecter.IsCurrentCellDirty)
            {
                DataViewInvoiceFooterSelecter.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void HandleFooterGridValueChange(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && DataViewInvoiceFooterSelecter.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                DataViewInvoiceFooterSelecter.EndEdit();
                DataViewInvoiceFooterSelecter.Update();
            }
        }
        private void ConfigureHeaderGridView(DataTable headerData)
        {
            DataViewInvoiceHeaderSelecter.Columns.Clear();
            DataViewInvoiceHeaderSelecter.AutoGenerateColumns = false;
            DataViewInvoiceHeaderSelecter.AllowUserToAddRows = false;

            foreach (DataColumn column in headerData.Columns)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    Name = column.ColumnName,
                    HeaderText = column.ColumnName,
                    DataPropertyName = column.ColumnName,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataViewInvoiceHeaderSelecter.Columns.Add(textColumn);
            }

            DataRow mappingRow = headerData.NewRow();
            headerData.Rows.InsertAt(mappingRow, 0);

            DataViewInvoiceHeaderSelecter.DataSource = headerData;

            if (DataViewInvoiceHeaderSelecter.Rows.Count > 0)
            {
                DataGridViewRow firstRow = DataViewInvoiceHeaderSelecter.Rows[0];
                foreach (DataGridViewColumn column in DataViewInvoiceHeaderSelecter.Columns)
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = _InvoiceHeaders.ToList(),
                        Value = _InvoiceHeaders.FirstOrDefault()
                    };
                    firstRow.Cells[column.Index] = comboCell;
                }
                firstRow.ReadOnly = false;
            }

            foreach (DataGridViewRow row in DataViewInvoiceHeaderSelecter.Rows)
            {
                if (row.Index > 0)
                {
                    row.ReadOnly = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewComboBoxCell comboCell)
                        {
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        }
                    }
                }
            }

            DataViewInvoiceHeaderSelecter.CurrentCellDirtyStateChanged += HandleHeaderGridCommit;
            DataViewInvoiceHeaderSelecter.CellValueChanged += HandleHeaderGridValueChange;
            DataViewInvoiceHeaderSelecter.DataError += (senderObj, errorArgs) =>
            {
                errorArgs.ThrowException = false;
            };
        }

        private void HandleHeaderGridCommit(object? sender, EventArgs e)
        {
            if (DataViewInvoiceHeaderSelecter.IsCurrentCellDirty)
            {
                DataViewInvoiceHeaderSelecter.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void HandleHeaderGridValueChange(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && DataViewInvoiceHeaderSelecter.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
            {
                DataViewInvoiceHeaderSelecter.EndEdit();
                DataViewInvoiceHeaderSelecter.Update();
            }
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.UseWaitCursor = true;
            ResetForTemplate();
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel/CSV Files|*.xls;*.xlsx;*.csv",
                Title = "Select Invoice File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataViewInvoiceSelecter.Columns.Clear();
                    DataViewInvoiceFooterSelecter.Columns.Clear();
                    DataViewInvoiceHeaderSelecter.Columns.Clear();

                    TextBoxFileName.Text = openFileDialog.FileName;
                    DataTable fullData = LoadFile(openFileDialog.FileName);

                    bool hasHeaders = false, hasFooters = false;

                    foreach (DataRow row in fullData.Rows)
                    {
                        string rowType = row[0].ToString()!.ToUpper();
                        if (rowType == "H") hasHeaders = true;
                        if (rowType == "F") hasFooters = true;
                    }

                    if (checkBoxHeader.Checked && checkBoxFooter.Checked && (!hasHeaders && !hasFooters))
                    {
                        string message = !hasHeaders && !hasFooters ?
                            "Both header (H) and footer (F) rows are missing" :
                            !hasHeaders ? "Header rows (H) are missing" : "Footer rows (F) are missing";

                        MessageBox.Show($"Header & Footer checkboxes are checked but {message}", "File Selection");
                        return;
                    }
                    else if (!checkBoxHeader.Checked && !checkBoxFooter.Checked && (hasHeaders && hasFooters))
                    {
                        string message = !hasHeaders && !hasFooters ?
                            "Both header (H) and footer (F) rows are present" :
                            !hasHeaders ? "Header rows (H) are present" : "Footer rows (F) are present";

                        MessageBox.Show($"Header & Footer checkboxes are Unchecked but {message}", "File Selection");
                        return;
                    }

                    bool useFirstRowAsHeader = !checkBoxHeader.Checked && !checkBoxFooter.Checked &&
                                              !hasHeaders && !hasFooters;



                    if (useFirstRowAsHeader)
                    {
                        checkBoxHeader.Enabled = false;
                        checkBoxFooter.Enabled = false;
                        FileHasHeader = true;                        
                    }

                    
                    CollectInvoiceData(fullData, useFirstRowAsHeader);
                    _sourceData = fullData;

                    EnableButton(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}");
                }
                finally
                {
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                this.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
            BtnReset.Enabled = true;
        }

        private void CollectInvoiceData(DataTable fullData, bool useFirstRowAsHeader)
        {
            DataTable headerData = fullData.Clone();
            DataTable transactionData = fullData.Clone();
            DataTable footerData = fullData.Clone();

            _isMergedFile = useFirstRowAsHeader;

            if (useFirstRowAsHeader)
            {
                if (fullData.Rows.Count > 0)
                {
                    transactionData = new DataTable();

                    DataRow firstRow = fullData.Rows[0];
                    foreach (object? item in firstRow.ItemArray)
                    {
                        transactionData.Columns.Add(item?.ToString());
                    }

                    for (int i = 1; i < fullData.Rows.Count; i++)
                    {
                        DataRow newRow = transactionData.NewRow();
                        newRow.ItemArray = fullData.Rows[i].ItemArray;
                        transactionData.Rows.Add(newRow);
                    }
                }
            }
            else
            {
                foreach (DataRow row in fullData.Rows)
                {
                    string rowType = row[0].ToString()!.ToUpper();
                    if (rowType == "H") headerData.ImportRow(row);
                    else if (rowType == "T") transactionData.ImportRow(row);
                    else if (rowType == "F") footerData.ImportRow(row);
                }
            }

            if (useFirstRowAsHeader)
            {
                DataViewInvoiceHeaderSelecter.Columns.Clear();
                DataViewInvoiceFooterSelecter.Columns.Clear();
                ConfigureTransactionGridView(transactionData, true);
            }
            else
            {
                ConfigureHeaderGridView(headerData);
                ConfigureTransactionGridView(transactionData, false);
                ConfigureFooterGridView(footerData);
                DataViewInvoiceHeaderSelecter.Visible = checkBoxHeader.Checked;
                DataViewInvoiceFooterSelecter.Visible = checkBoxFooter.Checked;
            }
        }
        private List<HeaderInfo> allFilterHeaders;
        private void ConfigureTransactionGridView(DataTable transactionData, bool useFirstRowAsHeader)
        {
            DataViewInvoiceSelecter.Columns.Clear();
            DataViewInvoiceSelecter.AutoGenerateColumns = false;
            DataViewInvoiceSelecter.AllowUserToAddRows = false;

            DataRow mappingRow = transactionData.NewRow();
            transactionData.Rows.InsertAt(mappingRow, 0);

            foreach (DataColumn column in transactionData.Columns)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    Name = column.ColumnName,
                    HeaderText = useFirstRowAsHeader ?
                        column.ColumnName :
                        $"Column{column.Ordinal + 1}",
                    DataPropertyName = column.ColumnName,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                DataViewInvoiceSelecter.Columns.Add(textColumn);
            }

            DataViewInvoiceSelecter.DataSource = transactionData;

            allHeaders = new List<HeaderInfo>();
            allHeadHeaders = new List<HeaderInfo>();
            allFootHeaders = new List<HeaderInfo>();   
            allTransHeaders = new List<HeaderInfo>();
            
            if(FileHasHeader == true)
            {
                allHeaders.AddRange(_InvoiceHeaders.Select(h => new HeaderInfo { Name = h, Type = 'H' }));
                allHeaders.AddRange(_InvoiceTransactionHeader.Select(t => new HeaderInfo { Name = t, Type = 'T' }));
                allHeaders.AddRange(_InvoiceFooters.Select(f => new HeaderInfo { Name = f, Type = 'F' }));
            }
            else
            {
                allHeadHeaders.AddRange(_InvoiceHeaders.Select(h => new HeaderInfo { Name = h, Type = 'H' }));
                allTransHeaders.AddRange(_InvoiceTransactionHeader.Select(t => new HeaderInfo { Name = t, Type = 'T' }));
                allFootHeaders.AddRange(_InvoiceFooters.Select(f => new HeaderInfo { Name = f, Type = 'F' }));
            }

            // HEADER GRID CONFIGURATION
            if (DataViewInvoiceHeaderSelecter.Rows.Count > 0)
            {
                DataGridViewRow firstRow = DataViewInvoiceHeaderSelecter.Rows[0];
                foreach (DataGridViewColumn column in DataViewInvoiceHeaderSelecter.Columns)
                {
                    var comboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = FileHasHeader ? allHeaders : allHeadHeaders,
                        DisplayMember = "Name",
                        ValueType = typeof(HeaderInfo)
                    };

                    firstRow.Cells[column.Index] = comboCell;
                }
                firstRow.ReadOnly = false;
            }

            foreach (DataGridViewRow row in DataViewInvoiceHeaderSelecter.Rows)
            {
                if (row.Index > 0)
                {
                    row.ReadOnly = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewComboBoxCell comboCell)
                        {
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        }
                    }
                }
            }

            DataViewInvoiceHeaderSelecter.DataError += (sender, e) =>
            {
                e.ThrowException = false;
            };


            // TRANSACTION GRID CONFIGURATION
            if (DataViewInvoiceSelecter.Rows.Count > 0)
            {
                DataGridViewRow firstRow = DataViewInvoiceSelecter.Rows[0];
                foreach (DataGridViewColumn column in DataViewInvoiceSelecter.Columns)
                {
                    var comboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = FileHasHeader ? allHeaders : allTransHeaders,
                        DisplayMember = "Name",
                        ValueType = typeof(HeaderInfo)
                    };

                    firstRow.Cells[column.Index] = comboCell;
                }
                firstRow.ReadOnly = false;
            }

            foreach (DataGridViewRow row in DataViewInvoiceSelecter.Rows)
            {
                if (row.Index > 0)
                {
                    row.ReadOnly = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewComboBoxCell comboCell)
                        {
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        }
                    }
                }
            }

            DataViewInvoiceSelecter.DataError += (sender, e) =>
            {
                e.ThrowException = false;
            };

            // FOOTER GRID CONFIGURATION
            if (DataViewInvoiceFooterSelecter.Rows.Count > 0)
            {
                DataGridViewRow firstRow = DataViewInvoiceFooterSelecter.Rows[0];
                foreach (DataGridViewColumn column in DataViewInvoiceFooterSelecter.Columns)
                {
                    var comboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = FileHasHeader ? allHeaders : allFootHeaders,
                        DisplayMember = "Name",
                        ValueType = typeof(HeaderInfo)
                    };

                    firstRow.Cells[column.Index] = comboCell;
                }
                firstRow.ReadOnly = false;
            }

            foreach (DataGridViewRow row in DataViewInvoiceFooterSelecter.Rows)
            {
                if (row.Index > 0)
                {
                    row.ReadOnly = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell is DataGridViewComboBoxCell comboCell)
                        {
                            comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        }
                    }
                }
            }

            DataViewInvoiceFooterSelecter.DataError += (sender, e) =>
            {
                e.ThrowException = false;
            };
        }

        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            if (_sourceData == null)
            {
                ToolStripStatusLabelErrorInvoice.Text = EnterInvalidFileErrorMsg;
                return;
            }

            this.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.UseWaitCursor = true;
            Application.DoEvents(); // Force UI refresh

            try
            {
                await Task.Run(() =>
                {
                    MappHeader.Clear();
                    MappTransaction.Clear();
                    MappFooter.Clear();

                    Dictionary<string, string> headerMappings, transactionMappings, footerMappings;

                    if (!_isMergedFile)
                    {
                        headerMappings = GetHeaderMappings();
                        transactionMappings = GetTransactionMappings();
                        footerMappings = GetFooterMappings();
                    }
                    else
                    {
                        transactionMappings = GetTransactionMappings();
                        headerMappings = MappHeader;
                        footerMappings = MappFooter;
                    }

                    var excludedTransactionKeys = new[] { "MaterialId", "CatalogProductName", "CatalogUOM" };

                    var missingHeaders = _InvoiceHeaders.Except(headerMappings.Keys);
                    var missingTransactions = _InvoiceTransactionHeader
                        .Where(k => !excludedTransactionKeys.Contains(k))
                        .Except(transactionMappings.Keys);
                    var missingFooters = _InvoiceFooters.Except(footerMappings.Keys);

                    var allErrors = missingHeaders.Concat(missingTransactions).Concat(missingFooters).ToList();

                    this.Invoke((MethodInvoker)delegate
                    {
                        if (allErrors.Any())
                        {
                            ToolStripStatusLabelErrorInvoice.Text = $"Missing: {string.Join(", ", allErrors.Take(3))}";
                            return;
                        }

                        if (!ValidateSupplier(headerMappings))
                        {
                            ToolStripStatusLabelErrorInvoice.Text = TmpltSupplierErrorMsg;
                            return;
                        }

                        if (!validateProductMapping)
                        {
                            ToolStripStatusLabelErrorInvoice.Text = TmpltPrdctMapingErrorMsg;
                            return;
                        }
                    });

                    // Final processing
                    this.Invoke((MethodInvoker)delegate
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        _parentForm.SetHeaderValues(MapHeaderData(headerMappings));
                        _parentForm.LoadTransactionData(MapTransactionData(transactionMappings));
                        _parentForm.SetFooterValues(MapFooterData(footerMappings));
                        _parentForm.CalculateTotal();
                    });
                });

                this.Close();
            }
            finally
            {
                this.Enabled = true;
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                Application.DoEvents();
            }
        }
        private bool ValidateSupplier(Dictionary<string, string> headerMappings)
        {
            if (!headerMappings.TryGetValue("Supplier", out string? supplierColumn))
            {
                return false;
            }

            if (_sourceData == null || _sourceData.Rows.Count == 0)
            {
                return false;
            }

            string supplierValue = _sourceData.Rows[0][supplierColumn]?.ToString()!;

            Supplier lSupplierName = SupplierManager.Instance.GetSupplierByName(supplierValue, Global.Company.CompanyId);
            if(lSupplierName != null)
            {
                SupplierId = lSupplierName.Id;
                return true;
            }
            return false; //SupplierService.IsSupplierValid(supplierValue); // Replace with your validation logic
        }
        private DataTable LoadFile(string filePath)
        {
            DataTable dt = new DataTable();
            string ext = Path.GetExtension(filePath).ToLower();
            bool hasHeader = false;

            if (ext == ".csv")
            {
                var lines = File.ReadAllLines(filePath).ToList();
                int maxColumns = lines.Max(line => line.Split(',').Length);

                for (int i = 0; i < maxColumns; i++)
                {
                    dt.Columns.Add($"Column{i + 1}");
                }

                foreach (var line in lines)
                {
                    string[] values = line.Split(',');
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < maxColumns; i++)
                    {
                        if (i < values.Length)
                            row[i] = values[i].Trim();
                        else
                            row[i] = DBNull.Value;
                    }
                    dt.Rows.Add(row);
                }
            }
            else // Excel files (.xls or .xlsx)
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = hasHeader
                        }
                    });

                    dt = result.Tables[0];

                    int maxColumns = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.ItemArray.Length > maxColumns)
                            maxColumns = row.ItemArray.Length;
                    }

                    for (int i = dt.Columns.Count; i < maxColumns; i++)
                    {
                        dt.Columns.Add($"Column{i + 1}");
                    }
                }
            }

            return dt;
        }

        private void DataViewInvoiceSelecter_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (DataViewInvoiceSelecter.Columns[e.Column.Index].HeaderCell is CheckBoxHeaderCell)
            {
                DataViewInvoiceSelecter.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            else
            {
                DataViewInvoiceSelecter.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private void DataViewInvoiceSelecter_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            var headerCell = DataViewInvoiceSelecter.Columns[e.ColumnIndex].HeaderCell as CheckBoxHeaderCell;
            if (headerCell != null && IsCheckBoxClicked(e.Location, headerCell))
            {
                headerCell.Checked = !headerCell.Checked;
                _columnSelections[e.ColumnIndex] = headerCell.Checked;

                DataViewInvoiceSelecter.InvalidateColumn(e.ColumnIndex);
            }
        }
        private bool IsCheckBoxClicked(Point location, CheckBoxHeaderCell header)
        {
            return true;
        }

        private void DataViewInvoiceSelecter_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                DataViewInvoiceSelecter.ClearSelection();
            }
        }

        private void DataViewInvoiceSelecter_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                DataViewInvoiceSelecter.ClearSelection();
                DataViewInvoiceSelecter.CurrentCell = null;
            }
        }

        private void DataViewInvoiceSelecter_SelectionChanged(object sender, EventArgs e)
        {
            if (DataViewInvoiceSelecter.CurrentCell?.RowIndex == -1)
            {
                DataViewInvoiceSelecter.ClearSelection();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            TextBoxFileName.Text = "";

            ToolStripStatusLabelErrorInvoice.Text = string.Empty;

            checkBoxHeader.Enabled = true;
            checkBoxFooter.Enabled = true;

            checkBoxHeader.Checked = false;
            checkBoxFooter.Checked = false;
            
            DataViewInvoiceSelecter.Columns.Clear();
            DataViewInvoiceFooterSelecter.Columns.Clear();
            DataViewInvoiceHeaderSelecter.Columns.Clear();

            EnableButton(0);
        }

        private Dictionary<string, string> GetMappingsFromGrid(DataGridView grid, char? typeFilter = null)
        {
            var mappings = new Dictionary<string, string>();
            if (grid.Rows.Count == 0 || grid.Columns.Count == 0) return mappings;

            DataGridViewRow mappingRow = grid.Rows[0];

            List<HeaderInfo> currentHeaderCollection;

            if (FileHasHeader)
            {
                currentHeaderCollection = allHeaders;
            }
            else
            {
                currentHeaderCollection = grid == DataViewInvoiceHeaderSelecter ? allHeadHeaders :
                                          grid == DataViewInvoiceSelecter ? allTransHeaders :
                                          grid == DataViewInvoiceFooterSelecter ? allFootHeaders :
                                          new List<HeaderInfo>(); // Fallback (adjust as needed)
            }

            if (typeFilter == 'H')
            {
                MappHeader.Clear();
            }
            else if (typeFilter == 'T')
            {
                MappTransaction.Clear();
            }
            else if (typeFilter == 'F')
            {
                MappFooter.Clear();
            }

            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (mappingRow.Cells[col.Index] is DataGridViewComboBoxCell comboCell)
                {
                    string? selectedHeaderName = comboCell.Value?.ToString();
                    if (string.IsNullOrEmpty(selectedHeaderName)) continue;

                    // Search in appropriate header collection
                    HeaderInfo? headerInfo = currentHeaderCollection
                        .FirstOrDefault(h => h.Name.Equals(selectedHeaderName, StringComparison.OrdinalIgnoreCase));

                    if (headerInfo == null) continue;

                    switch (headerInfo.Type)
                    {
                        case 'H': MappHeader[headerInfo.Name] = col.HeaderText; break;
                        case 'T': MappTransaction[headerInfo.Name] = col.HeaderText; break;
                        case 'F': MappFooter[headerInfo.Name] = col.HeaderText; break;
                    }

                    if (!_isMergedFile || (headerInfo.Type == typeFilter))
                    {
                        mappings[headerInfo.Name] = col.HeaderText;
                    }
                }
            }
            return mappings;
        }

        private Dictionary<string, string> GetHeaderMappings()
        {
            return _isMergedFile ?
                GetMappingsFromGrid(DataViewInvoiceSelecter, 'H') :
                GetMappingsFromGrid(DataViewInvoiceHeaderSelecter); ;
        }

        private Dictionary<string, string> GetTransactionMappings()
        {
            return _isMergedFile ?
                GetMappingsFromGrid(DataViewInvoiceSelecter, 'T') :
                GetMappingsFromGrid(DataViewInvoiceSelecter);
        }

        private Dictionary<string, string> GetFooterMappings()
        {
            return _isMergedFile ?
                GetMappingsFromGrid(DataViewInvoiceSelecter, 'F') :
                GetMappingsFromGrid(DataViewInvoiceFooterSelecter);
        }

        private Dictionary<string, object> MapHeaderData(Dictionary<string, string> mappings)
        {
            var result = new Dictionary<string, object>();
            var targetGrid = !_isMergedFile ? DataViewInvoiceHeaderSelecter : DataViewInvoiceSelecter;

            // Process mapped headers (e.g., "Supplier", "InvoiceDate")
            foreach (var mapping in mappings)
            {
                if (targetGrid.Columns.Contains(mapping.Value))
                {
                    result[mapping.Key] = targetGrid.Rows[1].Cells[mapping.Value].Value;
                }
            }

            try
            {
                object supplierIdValue = SupplierId;
                result["SupplierId"] = supplierIdValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SupplierId column missing or inaccessible: {ex.Message}");
                result["SupplierId"] = 0; // Default fallback value
            }

            return result;
        }

        // "CatalogProductName", "CatalogUOM"
        private DataTable MapTransactionData(Dictionary<string, string> mappings)
        {
            DataTable dt = new DataTable();

            foreach (var header in _InvoiceTransactionHeader)
            {
                dt.Columns.Add(header, GetColumnType(header));
            }

            string[] additionalColumns = { "CatalogProductName", "CatalogUOM", "MaterialId" };
            foreach (var col in additionalColumns)
            {
                if (!dt.Columns.Contains(col))
                {
                    dt.Columns.Add(col, typeof(string)); 
                }
            }

            foreach (DataGridViewRow row in DataViewInvoiceSelecter.Rows)
            {
                if (row.IsNewRow || row.Index == 0) continue;

                DataRow newRow = dt.NewRow();

                foreach (var mapping in mappings)
                {
                    if (!dt.Columns.Contains(mapping.Key)) continue;

                    if (DataViewInvoiceSelecter.Columns.Contains(mapping.Value))
                    {
                        object cellValue = row.Cells[mapping.Value].Value;
                        newRow[mapping.Key] = ConvertValue(cellValue, dt.Columns[mapping.Key].DataType);
                    }
                }

                if (DataViewInvoiceSelecter.Columns.Contains("CatalogProductName") && dt.Columns.Contains("PRODUCT"))
                {
                    newRow["PRODUCT"] = row.Cells["CatalogProductName"].Value;
                }

                if (DataViewInvoiceSelecter.Columns.Contains("CatalogUOM") && dt.Columns.Contains("UOM"))
                {
                    newRow["UOM"] = row.Cells["CatalogUOM"].Value;
                }

                if (DataViewInvoiceSelecter.Columns.Contains("MaterialId") && dt.Columns.Contains("MaterialId"))
                {
                    newRow["MaterialId"] = row.Cells["MaterialId"].Value;
                }

                dt.Rows.Add(newRow);
            }

            return dt;
        }
        private Dictionary<string, object> MapFooterData(Dictionary<string, string> mappings)
        {
            var result = new Dictionary<string, object>();
            var targetGrid = !_isMergedFile ? DataViewInvoiceFooterSelecter : DataViewInvoiceSelecter;

            foreach (var mapping in mappings)
            {
                if (targetGrid.Columns.Contains(mapping.Value))
                {
                    result[mapping.Key] = targetGrid.Rows[1].Cells[mapping.Value].Value;
                }
            }
            return result;
        }

        private Type GetColumnType(string columnName)
        {
            return columnName switch
            {
                "SNO" => typeof(int),
                "QTY" or "PRICE" or "TAX" or "DIS" or "AMOUNT" => typeof(decimal),
                "EXPDATE" => typeof(DateTime),
                _ => typeof(string)
            };
        }

        private object ConvertValue(object value, Type targetType)
        {
            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType)! : null!;
            }
        }
        private void BtnItemVerify_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> transactionMappings;

            if (_isMergedFile)
            {
                transactionMappings = GetTransactionMappings();
            }
            else
            {
                transactionMappings = GetTransactionMappings();
            }

            if (!transactionMappings.ContainsKey("PRODUCT"))
            {
                MessageBox.Show("PRODUCT column is not mapped! Please map the PRODUCT column first.",
                              "Mapping Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }
            if (!transactionMappings.ContainsKey("UOM"))
            {
                MessageBox.Show("UOM column is not mapped! Please map the UOM column first.",
                              "Mapping Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            string productColumn = transactionMappings["PRODUCT"];

            string uomColumn = transactionMappings["UOM"];

            var invoiceProducts = DataViewInvoiceSelecter.Rows
                .Cast<DataGridViewRow>()
                .Where(row => !row.IsNewRow &&
                              row.Cells[productColumn].Value != null &&
                              row.Cells[uomColumn].Value != null)
                .Select(row => new
                {
                    Product = row.Cells[productColumn].Value.ToString(),
                    Uom = row.Cells[uomColumn].Value.ToString()
                })
                .GroupBy(x => x.Product, StringComparer.OrdinalIgnoreCase)
                .Select(g => new
                {
                    ProductName = g.Key,
                    InvoiceUom = g.First().Uom
                })
                .ToList();

            long invoiceTemplateId = Convert.ToInt64(TextBoxInvoiceTemplateId.Text);
            var catalogItems = CatalogProductManager.Instance
                .ListProductByCompanyId(Global.Company.CompanyId);

            var TemplateProducts = ProductMappingManager.Instance.GetAllProductMappingTemplate(invoiceTemplateId);

            var resolutionData = new List<ProductResolutionDto>();
            for (int i = 1; i < invoiceProducts.Count; i++)
            {
                var invoiceProduct = invoiceProducts[i];
                DataGridViewRow row = DataViewInvoiceSelecter.Rows[i];

                ProductMappingTemplate tempProduct = null!;
                Product catProduct = null!;
                var catTempProduct = new
                {
                    UOM = "",
                    Name = "",
                    MaterialId = "",
                    Id = -1L
                };

                if (TemplateProducts != null && TemplateProducts.Count > 0)
                {
                    tempProduct = TemplateProducts.FirstOrDefault(p =>
                        p.InvoiceProductName.Equals(invoiceProduct.ProductName, StringComparison.OrdinalIgnoreCase))!;

                    if (tempProduct != null)
                    {
                        catTempProduct = new
                        {
                            UOM = tempProduct.CatalogUOM,
                            Name = tempProduct.CatalogProductName,
                            MaterialId = tempProduct.MaterialId,
                            Id = tempProduct.Id
                        };
                    }
                }

                if (tempProduct == null)
                {
                    catProduct = CatalogProductManager.Instance
                        .GetProductByName(invoiceProduct.ProductName, Global.Company.CompanyId);

                    if (catProduct != null)
                    {
                        catTempProduct = new
                        {
                            UOM = catProduct.UOM,
                            Name = catProduct.Name,
                            MaterialId = catProduct.MaterialId,
                            Id = catProduct.Id
                        };
                    }
                }

                resolutionData.Add(new ProductResolutionDto
                {
                    SlNo = i,
                    ProductName = invoiceProduct.ProductName,
                    InvoiceUom = invoiceProduct.InvoiceUom,
                    CatalogUom = catTempProduct.UOM,
                    CatalogProductName = catTempProduct.Name,
                    MaterialId = catTempProduct.MaterialId,
                    IsMatch = tempProduct != null || catProduct != null,
                    CatalogId = (int)catTempProduct.Id,
                    BatNo = transactionMappings.TryGetValue("BATNO", out var batNoCol)
                        ? row.Cells[batNoCol].Value?.ToString()
                        : null,
                    ExpDate = transactionMappings.TryGetValue("EXPDATE", out var expDateCol)
                        ? row.Cells[expDateCol].Value as DateTime?
                        : null,
                    Price = transactionMappings.TryGetValue("PRICE", out var priceCol)
                        ? row.Cells[priceCol].Value as decimal?
                        : null
                });
            }

            using (var resolutionForm = new FormProductResolution(resolutionData, invoiceTemplateId))
            {
                if (resolutionForm.ShowDialog() == DialogResult.OK)
                {
                    if (resolutionForm.ProductMappingId > 0)
                    {
                        TextBoxInvoiceTemplateId.Text = resolutionForm.ProductMappingId.ToString();
                    }
                    validateProductMapping = resolutionForm.ProductMapping;
                    EnableButton(2);
                    EnsureOptionalMappings(ref transactionMappings);
                    UpdateCatalogMappings(resolutionForm.ResolvedItems.ToList(), transactionMappings);
                }
            }
        }
        private void EnsureOptionalMappings(ref Dictionary<string, string> transactionMappings)
        {
            var optionalColumns = new[] { "MaterialId", "CatalogProductName", "CatalogUOM" };
            foreach (var column in optionalColumns)
            {
                if (!transactionMappings.ContainsKey(column))
                {
                    var sourceColumn = DataViewInvoiceSelecter.Columns
                        .Cast<DataGridViewColumn>()
                        .FirstOrDefault(c => c.HeaderText == column)?.HeaderText;

                    if (sourceColumn != null)
                        transactionMappings[column] = sourceColumn;
                }
            }
        }
        private void UpdateCatalogMappings1(List<ProductResolutionDto> resolvedItems, Dictionary<string, string> transactionMappings)
        {
            string productColumn = transactionMappings["PRODUCT"];
            string materialIdColumn = transactionMappings["MaterialId"];
            string uomColumn = transactionMappings["UOM"];

            string batNoColumn = transactionMappings.ContainsKey("BATNO") ? transactionMappings["BATNO"] : "";
            string expDateColumn = transactionMappings.ContainsKey("EXPDATE") ? transactionMappings["EXPDATE"] : "";
            string priceColumn = transactionMappings.ContainsKey("PRICE") ? transactionMappings["PRICE"] : "";

            foreach (var resolvedItem in resolvedItems.Where(r => r.IsMatch))
            {
                foreach (DataGridViewRow row in DataViewInvoiceSelecter.Rows)
                {
                    if (!row.IsNewRow && (row.Cells[productColumn].Value?.ToString()?.Equals(resolvedItem.ProductName, StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        row.Cells[productColumn].Value = resolvedItem.ProductName;
                        row.Cells[materialIdColumn].Value = resolvedItem.MaterialId;
                        row.Cells[uomColumn].Value = resolvedItem.CatalogUom;

                        if (!string.IsNullOrEmpty(batNoColumn))
                            row.Cells[batNoColumn].Value = resolvedItem.BatNo;
                        if (!string.IsNullOrEmpty(expDateColumn))
                            row.Cells[expDateColumn].Value = resolvedItem.ExpDate;
                        if (!string.IsNullOrEmpty(priceColumn))
                            row.Cells[priceColumn].Value = resolvedItem.Price;
                    }
                }
            }
        }
        private void UpdateCatalogMappings2(List<ProductResolutionDto> resolvedItems, Dictionary<string, string> transactionMappings)
        {
            string productColumn = transactionMappings["PRODUCT"];
            string materialIdColumn = transactionMappings["MaterialId"];
            string uomColumn = transactionMappings["UOM"];
            string catalogProductColumn = transactionMappings["CatalogProductName"];
            string catalogUomColumn = transactionMappings["CatalogUOM"];

            foreach (var resolvedItem in resolvedItems.Where(r => r.IsMatch))
            {
                foreach (DataGridViewRow row in DataViewInvoiceSelecter.Rows)
                {
                    if (!row.IsNewRow &&
                        (row.Cells[productColumn].Value?.ToString()?
                            .Equals(resolvedItem.ProductName, StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        row.Cells[materialIdColumn].Value = resolvedItem.MaterialId;
                        row.Cells[uomColumn].Value = resolvedItem.CatalogUom;

                        row.Cells[catalogProductColumn].Value = resolvedItem.CatalogProductName;
                        row.Cells[catalogUomColumn].Value = resolvedItem.CatalogUom;
                    }
                }
            }
        }
        private void UpdateCatalogMappings(List<ProductResolutionDto> resolvedItems, Dictionary<string, string> transactionMappings)
        {
            if (!transactionMappings.TryGetValue("PRODUCT", out string? productColumn) ||
                !transactionMappings.TryGetValue("UOM", out string? uomColumn))
            {
                MessageBox.Show("PRODUCT or UOM columns are not mapped!");
                return;
            }

            string[] newColumns = { "MaterialId", "CatalogProductName", "CatalogUOM" };

            foreach (string columnName in newColumns)
            {
                if (!DataViewInvoiceSelecter.Columns.Contains(columnName))
                {
                    DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn
                    {
                        Name = columnName,
                        HeaderText = columnName,
                        DataPropertyName = columnName,
                        SortMode = DataGridViewColumnSortMode.NotSortable
                    };
                    DataViewInvoiceSelecter.Columns.Add(newColumn);
                }
            }

            foreach (string columnName in newColumns)
            {
                if (!transactionMappings.ContainsKey(columnName))
                {
                    transactionMappings[columnName] = columnName;
                }
            }

            string materialIdColumn = transactionMappings["MaterialId"];
            string catalogProductColumn = transactionMappings["CatalogProductName"];
            string catalogUomColumn = transactionMappings["CatalogUOM"];

            foreach (var resolvedItem in resolvedItems.Where(r => r.IsMatch))
            {
                foreach (DataGridViewRow row in DataViewInvoiceSelecter.Rows)
                {
                    if (!row.IsNewRow &&
                        (row.Cells[productColumn].Value?.ToString()?
                            .Equals(resolvedItem.ProductName, StringComparison.OrdinalIgnoreCase) ?? false))
                    {
                        row.Cells[materialIdColumn].Value = resolvedItem.MaterialId;
                        row.Cells[catalogProductColumn].Value = resolvedItem.CatalogProductName;
                        row.Cells[catalogUomColumn].Value = resolvedItem.CatalogUom;
                    }
                }
            }
            RearrangeDataGridViewColumns();
        }

        private void RearrangeDataGridViewColumns()
        {
            int productIndex = -1;
            int materialIdIndex = -1;
            int catalogProductIndex = -1;

            foreach (DataGridViewColumn column in DataViewInvoiceSelecter.Columns)
            {
                switch (column.HeaderText) 
                {
                    case "PRODUCT":
                        productIndex = column.Index;
                        break;
                    case "MaterialID":
                        materialIdIndex = column.Index;
                        break;
                    case "CatalogProduct":
                        catalogProductIndex = column.Index;
                        break;
                }
            }

            if (productIndex == -1 || materialIdIndex == -1 || catalogProductIndex == -1)
            {
                Console.WriteLine("One or more columns are missing.");
                return;
            }

            DataViewInvoiceSelecter.Columns["MaterialID"].DisplayIndex = productIndex + 1;
            DataViewInvoiceSelecter.Columns["CatalogProduct"].DisplayIndex = productIndex + 2;
        }
        public InvoiceMappingTemplate GetInvoiceMappingFromForm()
        {
            validateTemplate = true;
            InvoiceMappingTemplate InvoiceMappingTemplateInfo = new InvoiceMappingTemplate();

            if (!string.IsNullOrEmpty(TextBoxInvoiceTemplateId.Text))
            {
                InvoiceMappingTemplateInfo.Id = Convert.ToInt64(TextBoxInvoiceTemplateId.Text);
            }
            else
            {
                InvoiceMappingTemplateInfo.Id = 0L;
            }
            InvoiceMappingTemplateInfo.Name = TextBoxTemplateName.Text.Trim();
            InvoiceMappingTemplateInfo.CompanyId = Global.Company.CompanyId;
            Dictionary<string, string> headerMappings, transactionMappings, footerMappings;

            if (!_isMergedFile)
            {
                headerMappings = GetHeaderMappings();
                transactionMappings = GetTransactionMappings();
                footerMappings = GetFooterMappings();
            }
            else
            {
                transactionMappings = GetTransactionMappings();
                headerMappings = MappHeader;
                footerMappings = MappFooter;
            }

            var excludedTransactionKeys = new[] { "MaterialId", "CatalogProductName", "CatalogUOM" };

            var missingHeaders = _InvoiceHeaders.Except(headerMappings.Keys);
            var missingTransactions = _InvoiceTransactionHeader
                .Where(k => !excludedTransactionKeys.Contains(k))
                .Except(transactionMappings.Keys);
            var missingFooters = _InvoiceFooters.Except(footerMappings.Keys);

            var allErrors = missingHeaders.Concat(missingTransactions).Concat(missingFooters).ToList();

            if (allErrors.Any())
            {
                validateTemplate = false;
            }

            InvoiceMappingTemplateInfo.HeaderMappings = headerMappings.Select(kvp => new HeaderMapping
            {
                MappedHeader = kvp.Key,
                MappedColumn = GetColumnIndex(kvp.Value),
                InvoiceMappingTemplateId = InvoiceMappingTemplateInfo.Id,
            }).ToList();

            InvoiceMappingTemplateInfo.TransactionMappings = transactionMappings.Select(kvp => new TransactionMapping
            {
                MappedHeader = kvp.Key,
                MappedColumn = GetColumnIndex(kvp.Value),
                InvoiceMappingTemplateId = InvoiceMappingTemplateInfo.Id,
            }).ToList();

            InvoiceMappingTemplateInfo.FooterMappings = footerMappings.Select(kvp => new FooterMapping
            {
                MappedHeader = kvp.Key,
                MappedColumn = GetColumnIndex(kvp.Value),
                InvoiceMappingTemplateId = InvoiceMappingTemplateInfo.Id,
            }).ToList();

            return InvoiceMappingTemplateInfo;
        }
        private int GetColumnIndex(string columnName)
        {
            if (FileHasHeader)
            {
                // Case 1: File has headers → Find index by header name
                // ----------------------------------------------------
                string normalizedInput = Regex.Replace(columnName, "[^a-zA-Z0-9]", "")
                                              .Trim()
                                              .ToLower();

                foreach (DataGridViewColumn col in DataViewInvoiceSelecter.Columns)
                {
                    string normalizedHeader = Regex.Replace(col.HeaderText, "[^a-zA-Z0-9]", "")
                                                   .Trim()
                                                   .ToLower();

                    if (normalizedHeader == normalizedInput)
                    {
                        return col.Index; 
                    }
                }

                throw new ArgumentException($"Header '{columnName}' not found in grid.");
            }
            else
            {
                // Case 2: File has no headers → Parse "ColumnX" format
                // ----------------------------------------------------
                var match = Regex.Match(columnName, @"Column\s*(\d+)", RegexOptions.IgnoreCase);
                if (match.Success && int.TryParse(match.Groups[1].Value, out int index))
                {
                    return index - 1; 
                }

                throw new ArgumentException($"Invalid column name: '{columnName}'. Expected format: 'Column<number>'.");
            }
        }
        private int GetColumnIndex1(string columnName)
        {
            var match = Regex.Match(columnName, @"Column\s*(\d+)", RegexOptions.IgnoreCase);

            if (match.Success && int.TryParse(match.Groups[1].Value, out int index))
            {
                return index;
            }
            throw new ArgumentException($"Invalid column name format: '{columnName}'. Expected format: 'Column<number>' or 'Column <number>'.");
        }

        private void ForrmInvoiceUpload_Load(object sender, EventArgs e)
        {
            TextBoxFileName.ResetText();
            LoadTemplet();
        }

        private void LoadTemplet()
        {
            ComboUtils.InitializeInvoiceMappingTempletCombo(comboBoxSwapTextBoxAvailabelTemplet, Global.Company.CompanyId);
        }

        private Boolean ValidateForTemplate()
        {
            ToolStripStatusLabelErrorInvoice.Text = "";
            if (string.IsNullOrEmpty(TextBoxTemplateName.Text.Trim()))
            {
                ToolStripStatusLabelErrorInvoice.Text = EnterNameErrorMsg;
                TextBoxTemplateName.Select();
                return false;
            }
            return true;
        }

        private InvoiceMappingTemplate GetInvoiceMappingInfo()
        {
            string selectedTemplateName = comboBoxSwapTextBoxAvailabelTemplet.Text?.Trim()!;

            if (!string.IsNullOrEmpty(selectedTemplateName))
            {
                return InvoiceMappingManager.Instance.GetInvoiceMappingByName(
                    selectedTemplateName,
                    Global.Company.CompanyId
                );
            }

            return null!;
        }

        private void comboBoxSwapTextBoxAvailabelTemplet_SelectedIndexChanged(object sender, EventArgs e)
        {
            var templatemapping = GetInvoiceMappingInfo();
            if (templatemapping != null)
            {
                TextBoxInvoiceTemplateId.Text = templatemapping.Id.ToString();
            }
        }

        private void BtnApplyTemplet_Click(object sender, EventArgs e)
        {
            if (comboBoxSwapTextBoxAvailabelTemplet.SelectedItem == null)
            {
                MessageBox.Show("Please select a template!");
                return;
            }

            object selectedValue = TextBoxInvoiceTemplateId.Text;

            if (!long.TryParse(selectedValue.ToString(), out long templateId))
            {
                MessageBox.Show("Invalid template ID format!");
                return;
            }

            ApplyTemplate(templateId);
        }

        private void ApplyTemplate(long templateId)
        {

            ResetAllGrids();

            var template = InvoiceMappingManager.Instance.GetFullTemplateWithMappings(templateId);

            if (template == null)
            {
                MessageBox.Show("Template not found!");
                return;
            }

            ApplyHeaderMappings(template.HeaderMappings);
            ApplyTransactionMappings(template.TransactionMappings);
            ApplyFooterMappings(template.FooterMappings);
            ApplyProductMappings(template.ProductMappings);
        }

        private void ResetAllGrids()
        {
            ResetGrid(DataViewInvoiceHeaderSelecter);

            ResetGrid(DataViewInvoiceSelecter);

            ResetGrid(DataViewInvoiceFooterSelecter);

        }

        private void ResetGrid(DataGridView grid)
        {
            if (grid?.Columns == null) return;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderText = $"Column{col.Index + 1}";
            }

            if (grid.Rows.Count > 0)
            {
                foreach (DataGridViewCell cell in grid.Rows[0].Cells)
                {
                    if (cell is DataGridViewComboBoxCell comboCell)
                    {
                        comboCell.Value = null;
                    }
                }
            }
        }
        private void ApplyHeaderMappings(ICollection<HeaderMapping> headerMappings)
        {
            ApplyMappingsToGrid(DataViewInvoiceHeaderSelecter, headerMappings);
        }

        private void ApplyTransactionMappings(ICollection<TransactionMapping> transactionMappings)
        {
            ApplyMappingsToGrid(DataViewInvoiceSelecter, transactionMappings);
        }

        private void ApplyFooterMappings(ICollection<FooterMapping> footerMappings)
        {
            ApplyMappingsToGrid(DataViewInvoiceFooterSelecter, footerMappings);
        }

        private void ApplyProductMappings(ICollection<ProductMappingTemplate> productMappings)
        {
            if (GridViewProductRectification != null)
            {
                GridViewProductRectification.DataSource = productMappings;
            }
        }
        
        private void ApplyMappingsToGrid(DataGridView targetGrid, IEnumerable<BaseMapping> mappings)
        {
            if (targetGrid?.Columns == null || targetGrid.Rows.Count == 0) return;

            int mapcolumnIndex = 0;
            foreach (var mapping in mappings)
            {

                // Use dynamic indices for PRODUCT/UOM
                if (mapping.MappedHeader == "PRODUCT" && productColumnIndex != -1)
                {
                    productColumnIndex = mapcolumnIndex;
                }
                else if (mapping.MappedHeader == "UOM" && uomColumnIndex != -1)
                { uomColumnIndex = mapcolumnIndex; }
                mapcolumnIndex++;
            }

            foreach (var mapping in mappings)
            {
                // Convert 1-based MappedColumn to 0-based index
                int columnIndex = mapping.MappedColumn - 1;

                if (columnIndex < 0 || columnIndex >= targetGrid.Columns.Count)
                    continue;

                if (targetGrid.Rows[0].Cells[columnIndex] is DataGridViewComboBoxCell comboCell)
                {
                    comboCell.Value = mapping.MappedHeader;
                }
            }
        }

        private void BtnSveTemplet_Click_1(object sender, EventArgs e)
        {
            if (ValidateForTemplate())
            {
                InvoiceMappingTemplate mappings = this.GetInvoiceMappingFromForm();
                if (validateTemplate == true)
                {
                    if (InvoiceMappingManager.Instance.InvoiceTemplateUniqueById(mappings))
                    {
                        InvoiceMappingTemplate InvoiceMappingFromDB = null!;
                        if (mappings.Id == 0)
                        {
                            InvoiceMappingFromDB = InvoiceMappingManager.Instance.AddInvoiceTemplate(mappings);
                            LoadTemplet();
                            TextBoxInvoiceTemplateId.Text = InvoiceMappingFromDB.Id.ToString();
                            ToolStripStatusLabelErrorInvoice.Text = TmpltSaveSuccessMsg;
                        }
                    }
                    else
                    {
                        ToolStripStatusLabelErrorInvoice.Text = TmpltCreationErrorMsg;
                        TextBoxInvoiceTemplateId.Select();
                    }
                }
                else
                {
                    ToolStripStatusLabelErrorInvoice.Text = TmpltAssignErrorMsg;
                }
            }
        }
        public void SetProgressMax(int max)
        {
            progressBarInvoiceUpload.Invoke(new Action(() => progressBarInvoiceUpload.Value = 0));
            progressBarInvoiceUpload.Invoke(new Action(() => progressBarInvoiceUpload.Minimum = 0));
            progressBarInvoiceUpload.Invoke(new Action(() => progressBarInvoiceUpload.Maximum = max));
        }
        public void IncrementProgress(int incrementval)
        {
            progressBarInvoiceUpload.Invoke(new Action(() => progressBarInvoiceUpload.Increment(incrementval)));
        }
        public void CompleteIncrementProgress(int maxval)
        {
            progressBarInvoiceUpload.Invoke(new Action(() => progressBarInvoiceUpload.Increment(maxval)));
        }
    }

    public class ProductResolutionDto
    {
        public int SlNo { get; set; }
        public string? ProductName { get; set; }
        public string? InvoiceUom { get; set; }
        public string? CatalogUom { get; set; }
        public string? CatalogProductName { get; set; }
        public string? MaterialId { get; set; }
        public bool IsMatch { get; set; }
        public int CatalogId { get; set; }
        public string? BatNo { get; set; }
        public DateTime? ExpDate { get; set; }
        public decimal? Price { get; set; }
    }

    public class HeaderMappinges
    {
        public string? TargetField { get; set; }
        public string? SourceColumn { get; set; }
    }

    public class TransactionMappinges
    {
        public string? GridColumn { get; set; }
        public string? TargetColumn { get; set; }
    }

    public class FooterMappinges
    {
        public string? GridColumn { get; set; }
        public string? TargetControl { get; set; }
    }
}
