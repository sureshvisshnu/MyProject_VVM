using System;
using System.Drawing;
using System.Windows.Forms;
using fa.views.controls.ComboTreeView;
using fa.api.utils;
using fa.api.Log;
using fa.report.Inventory;
using fa.api.Hms;
using fa.views.utils.Report.Inventory;
using fa.libraries.utils;
using fa.views.controls;
using fa.report;
using fa.context;
using fa.model.Catalog;
using fa.views.sales;
using VisioForge.Libs.DirectShowLib.DMO;
using Fa.reports.Inventory;

namespace fa.reports.Inventory
{
    enum StockReportTableColumn
    {
        SNO, MID, PNAME, UOM, RACKNO, BAT, OPS, CLSTK, P_QTY, PR_QTY, S_QTY, SR_QTY, STIN_QTY, STOT_QTY, PCON, DAM, AD_QTY, LASTMONTHSALE, LOCATION
    }
    public partial class FormStockReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public long LocationId = 0L;
        public int ReportIndex = 0;
        ReportStock ReportStock = null;
        HospitalInventoryManager HospitalInventoryManager = null;
        public FormStockReport()
        {
            HospitalInventoryManager = HospitalInventoryManager.Instance;
            InitializeComponent();
        }
        public void AddItemsToReportTypeComboBox(bool maintainRackNumber)
        {
            if (!maintainRackNumber)
            {
                ComboBoxReportType.Items.Remove("By Rack");
            }
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void ResetForm()
        {
            this.Text = "Stock Report";
            AddItemsToReportTypeComboBox(Global.Company.MaintainRackNumber);
            ComboStockReportLocation.Size = new Size(225, 25);
            LabelCategory.Size = new Size(100, 25);
            ComboBoxReportType.Size = new Size(150, 25);

            StockReportDataGridView.Rows.Clear();
            if (!Global.Company.MaintainRackNumber)
            {
                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
            }
            else
            {
                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = true;
            }
            ErrorMsg.Text = "";
            EnableButton(false);
            BatchwiseCheck.Checked = false;
            ComboStockReportLocation.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboStockReportLocation.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxCategory.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxProductFamily.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxManufacturer.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxSupplier.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxRack.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            StockReportFromDate.Format = Global.Company.DateFormat;
            StockReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            StockReportToDate.Format = Global.Company.DateFormat;
            StockReportToDate.Date = Global.getTransactionDate();
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Location";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }
        private void LoadStockReport()
        {
            ReportStock = new ReportStock();
            ReportStock.FromDate = (DateTime)StockReportFromDate.Date;
            ReportStock.ToDate = (DateTime)StockReportToDate.Date;
            ReportStock.Company = Global.Company;
            ReportStock.ReportHeader = "For" + " @ " + SelectedNodesText(ComboStockReportLocation);
            ReportStock.BatchFlag = BatchwiseCheck.Checked;
            ReportStock.LocationIds = CheckedTreeUtils.SelectedNodes(ComboStockReportLocation).ToArray();
            ReportStock.Location = this.Text;
            ReportStock.IsAllLocation = SelectedNodesText(ComboStockReportLocation) == "All" ? true : false;
            ReportStock.IsExpiryReport = false;
            ReportStock.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
            ReportStock.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();
            ReportStock.SupplierName = selectedSuppliers.ToArray();
            ReportStock.CategoryCall = ReportIndex;
            ReportStock.GenerateReport();
            StockReportDataGridView.Rows.Clear();
            if (ReportStock.StockLedger != null && ReportStock.StockLedger.Count > 0)
            {
                int j = 2;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                EnableButton(true);
                int rowCount = 0;
                int k = 0;
                List<StockLedger> FilteredStockLedger = ReportStock.StockLedger.ToList();
                if (FilteredStockLedger != null && FilteredStockLedger.Count > 0)
                {
                    if (ReportIndex == 0 || ReportIndex == 1 || ReportIndex == 2)
                    {
                        FilteredStockLedger = ReportStock.StockLedger
                                                .GroupBy(lineItem => lineItem.Location.Name)
                                                .SelectMany(group => group.OrderBy(item => item.Location.Name).ThenBy(item => item.CatPFType))
                                                .ToList();
                        FilteredStockLedgerReport(FilteredStockLedger, rowCount);
                    }
                    else if (ReportIndex == 3)
                    {
                        FilteredStockLedger = ReportStock.StockLedger
                                                .Where(lineItem => selectedManufacturers.Contains(lineItem.Manufacturer))
                                                .OrderBy(lineItem => lineItem.Location.Name)
                                                .ThenBy(lineItem => lineItem.Manufacturer)
                                                .GroupBy(lineItem => new { lineItem.Location.Name, lineItem.Manufacturer })
                                                .SelectMany(group => group.OrderBy(item => item.Manufacturer))
                                                .ToList();
                        FilteredStockLedgerReport(FilteredStockLedger, rowCount);
                    }
                    else if (ReportIndex == 4)
                    {
                        FilteredStockLedger = ReportStock.StockLedger
                                                .Where(lineItem => selectedSuppliers.Contains(lineItem.Supplier))
                                                .OrderBy(lineItem => lineItem.Location.Name)
                                                .ThenBy(lineItem => lineItem.Supplier)
                                                .GroupBy(lineItem => new { lineItem.Location.Name, lineItem.Supplier })
                                                .SelectMany(group => group.OrderBy(item => item.Supplier))
                                                .ToList();
                        FilteredStockLedgerReport(FilteredStockLedger, rowCount);
                    }
                    else if (ReportIndex == 5)
                    {
                        FilteredStockLedger = ReportStock.StockLedger
                                                .Where(lineItem => selectedRackNumbers.Contains(lineItem.RackNumber))
                                                .OrderBy(lineItem => lineItem.Location.Name)
                                                .ThenBy(lineItem => lineItem.RackNumber)
                                                .GroupBy(lineItem => new { lineItem.Location.Name, lineItem.RackNumber })
                                                .SelectMany(group => group)
                                                .ToList();
                        FilteredStockLedgerReport(FilteredStockLedger, rowCount);
                    }
                }
            }
            else
            {
                ResetForm();
                ErrorMsg.Text = InformationMsg;
            }
        }
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(ComboStockReportLocation, Global.Company.CompanyId);
            ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
            ComboUtils.InitializeAllFamilyProductComboForReport(ComboBoxProductFamily, Global.Company.CompanyId);
            ComboUtils.InitializeAlltManufactureCombo(ComboBoxManufacturer, Global.Company.CompanyId);
            ComboUtils.InitializeAllSupplierComboForReport(ComboBoxSupplier, Global.Company.CompanyId);
            ComboUtils.InitializeAllRackNumberComboForReport(ComboBoxRack, Global.Company.CompanyId);

        }
        private void FormStockReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            if (StockReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)StockReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                StockReportFromDate.Focus();
                return false;
            }
            if (StockReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)StockReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                StockReportToDate.Focus();
                return false;
            }
            if (ReportIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxCategory.Focus();
                    return false;
                }
            }
            if (ReportIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxProductFamily.Focus();
                    return false;
                }
            }
            if (ReportIndex == 3)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxManufacturer).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxManufacturer.Focus();
                    return false;
                }
            }
            if (ReportIndex == 4)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxSupplier).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxSupplier.Focus();
                    return false;
                }
            }
            if (ReportIndex == 5)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxRack).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxRack.Focus();
                    return false;
                }
            }
            if (CheckedTreeUtils.SelectedNodes(ComboStockReportLocation).Count < 1)
            {
                ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, "Location");
                ComboStockReportLocation.Focus();
                return false;
            }
            return true;
        }
        private void RunReportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                StockReportDataGridView.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadStockReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex.Message);
                Logger.LogError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //StockReportSavePrint StockReportSavePrint = new StockReportSavePrint();
            //StockReportSavePrint.ExportOrPrintToFile(ReportStock, "StockReport", "pdf", false, ReportIndex, selectedRackNumbers, selectedManufacturers, selectedSuppliers);

        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            //StockReportSavePrint StockReportSavePrint = new StockReportSavePrint();
            //StockReportSavePrint.ExportOrPrintToFile(ReportStock, "StockReport", "pdf", true, ReportIndex, selectedRackNumbers, selectedManufacturers, selectedSuppliers);
        }
        private void BtnReset_Click_1(object sender, EventArgs e)
        {

        }
        private void DisplayCheckedCategory()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxCategory.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxCategory.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Category"; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
            }
        }
        private void DisplayCheckedProductFamily()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxProductFamily.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxProductFamily.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All ProductFamily"; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
            }
        }
        private void DisplayCheckedManufactrurer()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxManufacturer.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxManufacturer.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Manufactrurer"; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
            }
        }
        private void DisplayCheckedSupplier()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxSupplier.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxSupplier.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Supplier"; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
            }
        }
        private void DisplayCheckedRackNo()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxRack.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboBoxRack.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All RackNo"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = "Stock Report " + " For  " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Report ";
            }
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboStockReportLocation.CheckedNodes.Count > 0)
            {
                foreach (ComboTreeNode node in ComboStockReportLocation.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedNodes = "All Location"; break; }
                    CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                }
                this.Text = " @ " + CheckedNodes;
            }
            else
            {
                this.Text = "Stock Report";
            }
        }
        private void ComboStockReportLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            //DisplayCheckedAccount();
            DisplayCheckedInformation();
        }

        private void StockReportDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportTableColumn.MID].Value == null)
            {
                if (e.ColumnIndex == (int)StockReportTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)StockReportTableColumn.LASTMONTHSALE)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }
        private void StockReportDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportTableColumn.MID].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.StockReportDataGridView.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.StockReportDataGridView.HorizontalScrollingOffset,
            e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportTableColumn.LOCATION].Value != null ? StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportTableColumn.LOCATION].Value.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReset.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            if (!Global.Company.MaintainRackNumber)
            {
                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
            }
            else
            {
                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = true;
            }
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ReportIndex = 0;
                ComboBoxReportType.Text = "By Date";
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = false;
                ComboBoxManufacturer.Visible = false;
                ComboBoxSupplier.Visible = false;
                ComboBoxRack.Visible = false;
                LabelCategory.Visible = false;

            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ReportIndex = 1;
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = true;
                ComboBoxCategory.Size = new Size(200, 25);
                ComboBoxProductFamily.Visible = false;
                ComboBoxManufacturer.Visible = false;
                ComboBoxSupplier.Visible = false;
                ComboBoxRack.Visible = false;
                LabelCategory.Visible = true;
                LabelCategory.Text = "Category";

            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                ReportIndex = 2;
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = true;
                ComboBoxProductFamily.Size = new Size(200, 25);
                ComboBoxManufacturer.Visible = false;
                ComboBoxSupplier.Visible = false;
                ComboBoxRack.Visible = false;
                LabelCategory.Visible = true;
                LabelCategory.Text = "Product Family";

            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                ReportIndex = 3;
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = false;
                ComboBoxManufacturer.Visible = true;
                ComboBoxManufacturer.Size = new Size(200, 25);
                ComboBoxSupplier.Visible = false;
                ComboBoxRack.Visible = false;
                LabelCategory.Visible = true;
                LabelCategory.Text = "Manufacturer";

            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                ReportIndex = 4;
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = false;
                ComboBoxManufacturer.Visible = false;
                ComboBoxSupplier.Visible = true;
                ComboBoxSupplier.Size = new Size(200, 25);
                ComboBoxRack.Visible = false;
                LabelCategory.Visible = true;
                LabelCategory.Text = "Supplier";

            }
            else if (ComboBoxReportType.SelectedIndex == 5)
            {
                ReportIndex = 5;
                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
                StockReportDataGridView.Visible = true;
                ComboBoxCategory.Visible = false;
                ComboBoxProductFamily.Visible = false;
                ComboBoxManufacturer.Visible = false;
                ComboBoxSupplier.Visible = false;
                ComboBoxRack.Visible = true;
                ComboBoxRack.Size = new Size(200, 25);
                LabelCategory.Visible = true;
                LabelCategory.Text = "Rack";
            }
            Cursor.Current = Cursors.Default;
        }
        private List<string> selectedRackNumbers = new List<string>();
        private List<string> selectedSuppliers = new List<string>();
        private List<string> selectedManufacturers = new List<string>();


        private void ComboBoxRack_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            //DisplayCheckedRackNo();
            DisplayCheckedInformation();
            string rackNumber = e.Node.Text;

            if (e.Node.Checked)
            {
                if (!selectedRackNumbers.Contains(rackNumber))
                {
                    selectedRackNumbers.Add(rackNumber);
                }

            }
            else
            {
                selectedRackNumbers.Remove(rackNumber);
            }
        }
        private void FilteredStockLedgerReport(List<StockLedger> FilteredStockLedger, int rowCount)
        {
            int j = 2;
            int k = 0;
            Color[] RowColor = new Color[2];
            RowColor[0] = Color.White;
            RowColor[1] = Color.WhiteSmoke;
            string Loaction = string.Empty;
            string ProductCategory = string.Empty;
            string ProductFamilyCat = string.Empty;
            string RNo = string.Empty;
            string PSuply = string.Empty;
            string PManfact = string.Empty;
            if (FilteredStockLedger != null && FilteredStockLedger.Count > 0)
            {
                foreach (StockLedger LineItem in FilteredStockLedger)
                {
                    int irow = rowCount;
                    if (Loaction != LineItem.Location.Name && !ReportStock.IsAllLocation)
                    {
                        k = 0;
                        irow = StockReportDataGridView.Rows.Add();
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = LineItem.Location.Name;
                        Loaction = LineItem.Location.Name;
                        ProductCategory = string.Empty;
                        ProductFamilyCat = string.Empty;
                        PManfact = string.Empty;
                        PSuply = string.Empty;
                        RNo = string.Empty;
                    }
                    if (LineItem.Batch && LineItem.OpeningStockforBatch.Count > 0)
                    {
                        if (ReportIndex == 1)
                        {
                            if (ProductCategory != LineItem.CatType)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Category : " + LineItem.CatType;
                                ProductCategory = LineItem.CatType;
                            }
                        }
                        if (ReportIndex == 2)
                        {
                            if (ProductFamilyCat != LineItem.CatPFType)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "ProductFamily : " + LineItem.CatPFType;
                                ProductFamilyCat = LineItem.CatPFType;
                            }
                        }
                        if (ReportIndex == 3)
                        {
                            if (PManfact != LineItem.Manufacturer)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Manufacturer : " + LineItem.Manufacturer;
                                PManfact = LineItem.Manufacturer;
                            }
                        }
                        if (ReportIndex == 4)
                        {
                            if (PSuply != LineItem.Supplier)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Supplier : " + LineItem.Supplier;
                                PSuply = LineItem.Supplier;
                            }
                        }
                        if (ReportIndex == 5)
                        {
                            if (RNo != LineItem.RackNumber)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                RNo = LineItem.RackNumber;
                            }
                        }
                        foreach (BatchOpeningStock BatchWise in LineItem.OpeningStockforBatch)
                        {
                            irow = StockReportDataGridView.Rows.Add();
                            StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            j++;
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.SNO].Value = k + 1;
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.MID].Value = LineItem.MaterialId;
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PNAME].Value = LineItem.Name;
                            if (ReportIndex != 5)
                            {
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.RACKNO].Value = LineItem.RackNumber;
                                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = true;
                            }
                            else
                            {
                                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
                            }
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.UOM].Value = LineItem.uom;
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.BAT].Value = BatchWise.BatchNo + Environment.NewLine + String.Format("{0:d}", BatchWise.ExpDate);
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.OPS].Value = BatchWise.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.CLSTK].Value = BatchWise.ClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.P_QTY].Value = BatchWise.BatchPurchaseQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PR_QTY].Value = BatchWise.BatchPurchaseRtnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.S_QTY].Value = BatchWise.BatchSalesQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.SR_QTY].Value = BatchWise.BatchSalesRtnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.STIN_QTY].Value = BatchWise.BatchStockMovedInQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.STOT_QTY].Value = BatchWise.BatchStockMovedOutQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PCON].Value = BatchWise.BatchPatientConsumedQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.DAM].Value = BatchWise.BatchDamagedQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.AD_QTY].Value = BatchWise.BatchAdjustQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LASTMONTHSALE].Value = BatchWise.BatchLMSQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            rowCount++;
                            k++;
                        }
                    }
                    else
                    {
                        if (ReportIndex == 1)
                        {
                            if (ProductCategory != LineItem.CatType)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Category : " + LineItem.CatType;
                                ProductCategory = LineItem.CatType;
                            }
                        }
                        if (ReportIndex == 2)
                        {
                            if (ProductFamilyCat != LineItem.CatPFType)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "ProductFamily : " + LineItem.CatPFType;
                                ProductFamilyCat = LineItem.CatPFType;
                            }
                        }
                        if (ReportIndex == 3)
                        {
                            if (PManfact != LineItem.Manufacturer)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Manufacturer : " + LineItem.Manufacturer;
                                PManfact = LineItem.Manufacturer;
                            }
                        }
                        if (ReportIndex == 4)
                        {
                            if (PSuply != LineItem.Supplier)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Supplier : " + LineItem.Supplier;
                                PSuply = LineItem.Supplier;
                            }
                        }
                        if (ReportIndex == 5)
                        {
                            if (RNo != LineItem.RackNumber)
                            {
                                k = 0;
                                irow = StockReportDataGridView.Rows.Add();
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                j++;
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LOCATION].Value = "Rack No : " + LineItem.RackNumber;
                                RNo = LineItem.RackNumber;
                            }
                        }
                        irow = StockReportDataGridView.Rows.Add();
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        StockReportDataGridView.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.SNO].Value = k + 1;
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.MID].Value = LineItem.MaterialId;
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PNAME].Value = LineItem.Name;
                        if (ReportIndex != 5)
                        {
                            if (!Global.Company.MaintainRackNumber)
                            {
                                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
                            }
                            else
                            {
                                StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.RACKNO].Value = LineItem.RackNumber;
                                StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = true;
                            }
                        }
                        else
                        {
                            StockReportDataGridView.Columns[(int)StockReportTableColumn.RACKNO].Visible = false;
                        }
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.UOM].Value = LineItem.uom;
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.OPS].Value = LineItem.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.CLSTK].Value = LineItem.CloseingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.P_QTY].Value = LineItem.PurchaseQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PR_QTY].Value = LineItem.PurchaseRtnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.S_QTY].Value = LineItem.SalesQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.SR_QTY].Value = LineItem.SalesRtnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.STIN_QTY].Value = LineItem.StockMovedInQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.STOT_QTY].Value = LineItem.StockMovedOutQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.PCON].Value = LineItem.PatientConsumedQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.DAM].Value = LineItem.DamagedQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.AD_QTY].Value = LineItem.AdjustQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        StockReportDataGridView.Rows[irow].Cells[(int)StockReportTableColumn.LASTMONTHSALE].Value = LineItem.LMSQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                        k++;
                    }
                    rowCount++;
                }
            }
            else
            {
                ResetForm();
                ErrorMsg.Text = InformationMsg;
            }
        }

        private void ComboBoxSupplier_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
            string supplier = e.Node.Text;

            if (e.Node.Checked)
            {
                if (!selectedSuppliers.Contains(supplier))
                {
                    selectedSuppliers.Add(supplier);
                }
            }
            else
            {
                selectedSuppliers.Remove(supplier);
            }
        }
        private void ComboBoxManufacturer_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
            string manufacturer = e.Node.Text;

            if (e.Node.Checked)
            {
                if (!selectedManufacturers.Contains(manufacturer))
                {
                    selectedManufacturers.Add(manufacturer);
                }
            }
            else
            {
                selectedManufacturers.Remove(manufacturer);
            }
        }

        private void ComboBoxProductFamily_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            //DisplayCheckedProductFamily();
            DisplayCheckedInformation();
        }

        private void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            //DisplayCheckedCategory();
            DisplayCheckedInformation();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }

        private void DisplayCheckedInformation()
        {

            string rackNodes = GetCheckedNodes(ComboBoxRack, "For All Rack");
            string locationNodes = GetCheckedNodes(ComboStockReportLocation, "@ All Location");
            string categoryNodes = GetCheckedNodes(ComboBoxCategory, "For All Category");
            string productFamilyNodes = GetCheckedNodes(ComboBoxProductFamily, "For All Product Family");
            string manufacturerNodes = GetCheckedNodes(ComboBoxManufacturer, "For All Manufacturer");
            string supplierNodes = GetCheckedNodes(ComboBoxSupplier, "For All Supplier");

            string DisplayFor = "";
            string DisplayLocation = "";
            string title = "Stock Report";
            if (ReportIndex == 1)
            {
                if (categoryNodes != null && categoryNodes.ToString() != "For All Category")
                {
                    DisplayFor = " For Category ";
                }
            }
            if (ReportIndex == 2)
            {
                if (productFamilyNodes != null && productFamilyNodes.ToString() != "For All Product Family")
                {
                    DisplayFor = " For Product Family ";
                }
            }
            if (ReportIndex == 3)
            {
                if (manufacturerNodes != null && manufacturerNodes.ToString() != "For All Manufacturer")
                {
                    DisplayFor = " For Manufacturer ";
                }
            }
            if (ReportIndex == 4)
            {
                if (supplierNodes != null && supplierNodes.ToString() != "For All Supplier")
                {
                    DisplayFor = " For Supplier ";
                }
            }
            if (ReportIndex == 5)
            {
                if (rackNodes != null && rackNodes.ToString() != "For All Rack")
                {
                    DisplayFor = " For RackNo ";
                }
            }
            if (locationNodes != null && locationNodes.ToString() != "@ All Location")
            {
                DisplayLocation = " @ Location ";
            }

            title += AppendToTitleIfNotEmpty(rackNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(categoryNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(productFamilyNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(manufacturerNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(supplierNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(locationNodes!, DisplayLocation);

            this.Text = title;
        }

        private string AppendToTitleIfNotEmpty(string nodes, string prefix)
        {
            return string.IsNullOrEmpty(nodes) ? "" : $"{prefix} {nodes}";
        }
        private string GetCheckedNodes(ToolstripCheckedTreeComboBox comboBox, string nodeName)
        {
            if (comboBox == null || comboBox.CheckedNodes == null || comboBox.CheckedNodes.Count == 0)
            {
                return string.Empty;
            }

            bool isAllSelected = comboBox.CheckedNodes.Any(node => node.Name == "All");

            if (isAllSelected)
            {
                return nodeName;
            }

            string checkedNodes = string.Join(", ", comboBox.CheckedNodes.Select(node => node.Text));
            return checkedNodes;
        }

        private void New_Click(object sender, EventArgs e)
        {
            FormStockReportNew formStockReportNew = new FormStockReportNew();
            formStockReportNew.ShowDialog();
        }
    }
}
