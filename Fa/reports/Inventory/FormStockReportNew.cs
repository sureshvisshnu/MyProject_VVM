using fa;
using fa.reports.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.controls.ComboTreeView;
using fa.views.controls;
using fa.libraries.utils;
using fa.api.utils;
using FaData.Utils;
using fa.report.Inventory;
using FADataAccessLibrary.report.Inventory;
using VisioForge.Libs.MediaFoundation.OPM;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Office.Interop.Excel;
using NPOI.SS.Formula.Functions;
using Global = fa.Global;
using fa.reports.catalog;
using Font = System.Drawing.Font;
using NPOI.SS.UserModel;
using Fa.reports.sales;
using fa.views.utils.Report.Inventory;

namespace Fa.reports.Inventory
{
    enum StockReportNewTableColumn
    {
        SNO, MID, PNAME, UOM, RACKNO, BAT, OPS, OPA, CLSTK, CSA, P_QTY, PR_QTY, S_QTY, SR_QTY, STIN_QTY, STOT_QTY, PCON, DAM, AD_QTY, LASTMONTHSALE, LOCATION
    }
    public partial class FormStockReportNew : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select type";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public long LocationId = 0L;

        RptStockReport RptStockReport = null;
        public FormStockReportNew()
        {
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
        private void LoadLocationWithFilter()
        {
            ComboUtils.InitializeStockLocationCombo(ComboStockReportLocation, Global.Company.CompanyId);
            ComboUtils.InitializeAllCategoryCombo(ComboBoxCategory, Global.Company.CompanyId);
            ComboUtils.InitializeAllFamilyProductComboForReport(ComboBoxProductFamily, Global.Company.CompanyId);
            ComboUtils.InitializeAlltManufactureCombo(ComboBoxManufacturer, Global.Company.CompanyId);
            ComboUtils.InitializeAllSupplierComboForReport(ComboBoxSupplier, Global.Company.CompanyId);
            ComboUtils.InitializeAllRackNumberComboForReport(ComboBoxRack, Global.Company.CompanyId);
        }

        private void FormStockReportNew_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private bool FormValidate()
        {
            ErrorMsg.Text = "";
            if (StockReportFromDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)StockReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                StockReportFromDate.Focus();
                return false;
            }
            if (StockReportToDate.Date == null || !fa.api.utils.DateUtils.ValidDate(((DateTime)StockReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsg.Text = EnterValidDateErrorMsg;
                StockReportToDate.Focus();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                ErrorMsg.Text = string.Format(SelectferenceErrorMsg, LabelCategory.Text);
                ComboBoxReportType.Focus();
                return false;
            }
            if (ComboBoxReportType.SelectedIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxCategory).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxCategory.Focus();
                    return false;
                }
            }
            if (ComboBoxReportType.SelectedIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxProductFamily.Focus();
                    return false;
                }
            }
            if (ComboBoxReportType.SelectedIndex == 3)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxManufacturer).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxManufacturer.Focus();
                    return false;
                }
            }
            if (ComboBoxReportType.SelectedIndex == 4)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxSupplier).Count < 1)
                {
                    ErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelCategory.Text);
                    ComboBoxSupplier.Focus();
                    return false;
                }
            }
            if (ComboBoxReportType.SelectedIndex == 5)
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
            StockReportDataGridView.DefaultCellStyle.SelectionForeColor = StockReportDataGridView.DefaultCellStyle.ForeColor;
            StockReportDataGridView.DefaultCellStyle.SelectionBackColor = StockReportDataGridView.DefaultCellStyle.BackColor;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadLocationWithFilter();
        }
        private void DisplayCheckedInformation()
        {
            string productFamilyText = string.Empty;
            string productLocationText = string.Empty;
            string productCategoryText = string.Empty;
            string productRackText = string.Empty;
            string productSupplierText = string.Empty;
            string productManufacturerText = string.Empty;
            if (ComboBoxReportType.SelectedIndex == 0)
            {
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");
                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                productCategoryText = GetCheckedNodesText(ComboBoxCategory, "All Product Category");
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");

                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productCategoryText)) this.Text += " @ " + productCategoryText;
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
            else if (ComboBoxReportType.SelectedIndex == 2)
            {
                productFamilyText = GetCheckedNodesText(ComboBoxProductFamily, "All Product Family");
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");

                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productFamilyText)) this.Text += " @ " + productFamilyText;
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
            else if (ComboBoxReportType.SelectedIndex == 3)
            {
                productManufacturerText = GetCheckedNodesText(ComboBoxManufacturer, "All Product Manufacturer");
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");

                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productManufacturerText)) this.Text += " @ " + productManufacturerText;
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
            else if (ComboBoxReportType.SelectedIndex == 4)
            {
                productSupplierText = GetCheckedNodesText(ComboBoxSupplier, "All Product Supplier");
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");

                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productSupplierText)) this.Text += " @ " + productSupplierText;
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
            else if (ComboBoxReportType.SelectedIndex == 5)
            {
                productRackText = GetCheckedNodesText(ComboBoxRack, "All Product Rack");
                productLocationText = GetCheckedNodesText(ComboStockReportLocation, "All Location");

                this.Text = "Stock Report";
                if (!string.IsNullOrEmpty(productRackText)) this.Text += " @ " + productRackText;
                if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
            }
        }

        private string GetCheckedNodesText(ToolstripCheckedTreeComboBox comboBox, string allText)
        {
            if (comboBox.CheckedNodes.Count == 0) return "";

            var checkedNodes = comboBox.CheckedNodes.ToList();
            if (checkedNodes.Any(node => node.Name == "All")) return allText;

            return string.Join(", ", checkedNodes.Select(node => node.Text));
        }
        private HashSet<string> selectedProductFamilies = new HashSet<string>();
        private HashSet<string> selectedProductCategory = new HashSet<string>();

        private async void ComboBoxProductFamily_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            await Task.Run(() => UpdateProductFamilySelection(e.Node));
            DisplayCheckedInformation();
        }

        private void UpdateProductFamilySelection(ComboTreeNode node)
        {
            string productFamily = node.Text;

            lock (selectedProductFamilies)
            {
                if (node.Checked)
                {
                    selectedProductFamilies.Add(productFamily);
                }
                else
                {
                    selectedProductFamilies.Remove(productFamily);
                }
            }
        }

        private async void ComboBoxCategory_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            await Task.Run(() => UpdateProductCategorySelection(e.Node));
            DisplayCheckedInformation();
        }

        private void UpdateProductCategorySelection(ComboTreeNode node)
        {
            string productCategory = node.Text;

            lock (selectedProductCategory)
            {
                if (node.Checked)
                {
                    selectedProductCategory.Add(productCategory);
                }
                else
                {
                    selectedProductCategory.Remove(productCategory);
                }
            }
        }

        private void ComboStockReportLocation_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
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
        private void ComboBoxRack_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
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
        private void LoadStockReport()
        {
            RptStockReport = new RptStockReport();
            RptStockReport.FromDate = (DateTime)StockReportFromDate.Date;
            RptStockReport.ToDate = (DateTime)StockReportToDate.Date;
            RptStockReport.Company = Global.Company;
            RptStockReport.ReportHeader = "For" + " @ " + SelectedNodesText(ComboStockReportLocation);
            RptStockReport.BatchFlag = BatchwiseCheck.Checked;
            RptStockReport.LocationIds = CheckedTreeUtils.SelectedNodes(ComboStockReportLocation).ToArray();
            RptStockReport.Location = this.Text;
            RptStockReport.IsAllLocation = SelectedNodesText(ComboStockReportLocation) == "All Location" ? true : false;
            RptStockReport.LocationCount = ComboStockReportLocation.Nodes.Count;
            RptStockReport.CategoryIds = CheckedTreeUtils.SelectedNodes(ComboBoxCategory).ToArray();
            RptStockReport.ProductFamilyNames = CheckedTreeUtils.SelectedNameNodes(ComboBoxProductFamily).ToArray();
            RptStockReport.ProductFamilyIds = CheckedTreeUtils.SelectedNodes(ComboBoxProductFamily).ToArray();
            RptStockReport.SupplierName = selectedSuppliers.ToArray();
            RptStockReport.ManufactureName = selectedManufacturers.ToArray();
            RptStockReport.RackNumber = selectedRackNumbers.ToArray();
            RptStockReport.IsBatchWise = BatchwiseCheck.Checked;
            RptStockReport.Type = ComboBoxReportType.SelectedIndex == 0 ? ComboTypeSelection.BYDATE :
                      ComboBoxReportType.SelectedIndex == 1 ? ComboTypeSelection.BYCATEGORY :
                      ComboBoxReportType.SelectedIndex == 2 ? ComboTypeSelection.BYPRODUCTFAMILY :
                      ComboBoxReportType.SelectedIndex == 3 ? ComboTypeSelection.BYMANUFACTURER :
                      ComboBoxReportType.SelectedIndex == 4 ? ComboTypeSelection.BYSUPPLIER :
                      ComboBoxReportType.SelectedIndex == 5 ? ComboTypeSelection.BYRACK :
                      RptStockReport.Type;

            RptStockReport.GenerateReport();

            if (RptStockReport.StockReportLineItemData != null && RptStockReport.StockReportLineItemData.Count > 0)
            {
                StockReportDataGridView.Rows.Clear();
                EnableButton(true);
                double locationOpeningStock = 0, locationOpeningAmount = 0, locationClosingStock = 0, locationClosingAmount = 0, locationPurchaseStock = 0,
                    locationPurchaseReturnStock = 0, locationSalesStock = 0, locationSalesReturnStock = 0, locationMoveInStock = 0, locationMoveOutStock = 0,
                    locationToPatientStock = 0, locationDamageStock = 0, locationAdjustmentStock = 0, locationLastMonthSaleStock = 0;
                double grandOpeningStock = 0, grandOpeningAmount = 0, grandClosingStock = 0, grandClosingAmount = 0, grandPurchaseStock = 0,
                    grandPurchaseReturnStock = 0, grandSalesStock = 0, grandSalesReturnStock = 0, grandMoveInStock = 0, grandMoveOutStock = 0,
                    grandToPatientStock = 0, grandDamageStock = 0, grandAdjustmentStock = 0, grandLastMonthSaleStock = 0;
                if (RptStockReport.Type == ComboTypeSelection.BYDATE)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(currentLocation))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }

                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(currentLocation))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
                else if (RptStockReport.Type == ComboTypeSelection.BYCATEGORY)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string CategoryName = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.CategoryName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(CategoryName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                CategoryName = string.Empty;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (CategoryName != lineItem.CategoryName)
                        {
                            if (!string.IsNullOrEmpty(CategoryName))
                            {
                                var subtotalRow = new DataGridViewRow();
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Category Name : {lineItem.CategoryName}";
                            rows.Add(locationRow);

                            CategoryName = lineItem.CategoryName;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(CategoryName))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
                else if (RptStockReport.Type == ComboTypeSelection.BYPRODUCTFAMILY)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string ProductFamilyName = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.ProductFamilyName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(ProductFamilyName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                 locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                 locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                ProductFamilyName = string.Empty;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (ProductFamilyName != lineItem.ProductFamilyName)
                        {
                            if (!string.IsNullOrEmpty(ProductFamilyName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Product Family Name : {lineItem.ProductFamilyName}";
                            rows.Add(locationRow);

                            ProductFamilyName = lineItem.ProductFamilyName;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(ProductFamilyName))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
                else if (RptStockReport.Type == ComboTypeSelection.BYMANUFACTURER)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string ManufactureName = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.ManufactureName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(ManufactureName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                ManufactureName = string.Empty;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (ManufactureName != lineItem.ManufactureName)
                        {
                            if (!string.IsNullOrEmpty(ManufactureName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Manufacture Name : {lineItem.ManufactureName}";
                            rows.Add(locationRow);

                            ManufactureName = lineItem.ManufactureName;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(ManufactureName))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
                else if (RptStockReport.Type == ComboTypeSelection.BYSUPPLIER)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string SupplierName = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.SupplierName).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(SupplierName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                 locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                 locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                SupplierName = string.Empty;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (SupplierName != lineItem.SupplierName)
                        {
                            if (!string.IsNullOrEmpty(SupplierName))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Supplier Name : {lineItem.SupplierName}";
                            rows.Add(locationRow);

                            SupplierName = lineItem.SupplierName;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(SupplierName))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
                else if (RptStockReport.Type == ComboTypeSelection.BYRACK)
                {
                    StockReportDataGridView.SuspendLayout();

                    var rows = new List<DataGridViewRow>();
                    int Sno = 1;
                    string currentLocation = string.Empty;
                    string RackNumber = string.Empty;

                    foreach (var lineItem in RptStockReport.StockReportLineItemData.OrderByDescending(x => x.LocationName).ThenBy(x => x.RackNumber).ThenBy(x => x.Name))
                    {
                        if (currentLocation != lineItem.LocationName)
                        {
                            if (!string.IsNullOrEmpty(RackNumber))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                                RackNumber = string.Empty;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Location Name : {lineItem.LocationName}";
                            rows.Add(locationRow);

                            currentLocation = lineItem.LocationName;
                            Sno = 1;
                        }
                        if (RackNumber != lineItem.RackNumber)
                        {
                            if (!string.IsNullOrEmpty(RackNumber))
                            {
                                AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                 locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                 locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);

                                locationOpeningStock = 0; locationOpeningAmount = 0; locationClosingStock = 0; locationClosingAmount = 0; locationPurchaseStock = 0;
                                locationPurchaseReturnStock = 0; locationSalesStock = 0; locationSalesReturnStock = 0; locationMoveInStock = 0; locationMoveOutStock = 0;
                                locationToPatientStock = 0; locationDamageStock = 0; locationAdjustmentStock = 0; locationLastMonthSaleStock = 0;
                            }
                            var locationRow = new DataGridViewRow();
                            locationRow.CreateCells(StockReportDataGridView);
                            locationRow.Cells[(int)StockReportNewTableColumn.LOCATION].Value = $"Rack Number : {lineItem.RackNumber}";
                            rows.Add(locationRow);

                            RackNumber = lineItem.RackNumber;
                            Sno = 1;
                        }
                        AddDataRow(rows, lineItem, Sno);
                        Sno++;
                        locationOpeningStock += lineItem.OpeningStock; locationOpeningAmount += lineItem.OpeningStockAmount; locationClosingStock += lineItem.CloseingStock; locationClosingAmount += lineItem.ClosingStockAmount;
                        locationPurchaseStock += lineItem.PurchaseQty; locationPurchaseReturnStock += lineItem.PurchaseReturnQty; locationSalesStock += lineItem.SalesQty; locationSalesReturnStock += lineItem.SalesReturnQty;
                        locationMoveInStock += lineItem.StockInQty; locationMoveOutStock += lineItem.StockOutQty; locationToPatientStock += lineItem.ToPatientQty; locationDamageStock += lineItem.DamageQty;
                        locationAdjustmentStock += lineItem.AdjustQty; locationLastMonthSaleStock += lineItem.LastMonthSale;

                        grandOpeningStock += lineItem.OpeningStock; grandOpeningAmount += lineItem.OpeningStockAmount; grandClosingStock += lineItem.CloseingStock; grandClosingAmount += lineItem.ClosingStockAmount;
                        grandPurchaseStock += lineItem.PurchaseQty; grandPurchaseReturnStock += lineItem.PurchaseReturnQty; grandSalesStock += lineItem.SalesQty; grandSalesReturnStock += lineItem.SalesReturnQty;
                        grandMoveInStock += lineItem.StockInQty; grandMoveOutStock += lineItem.StockOutQty; grandToPatientStock += lineItem.ToPatientQty; grandDamageStock += lineItem.DamageQty;
                        grandAdjustmentStock += lineItem.AdjustQty; grandLastMonthSaleStock += lineItem.LastMonthSale;
                    }
                    if (!string.IsNullOrEmpty(RackNumber))
                    {
                        AddSubTotalRow(rows, locationOpeningStock, locationOpeningAmount, locationClosingStock, locationClosingAmount, locationPurchaseStock,
                                                locationPurchaseReturnStock, locationSalesStock, locationSalesReturnStock, locationMoveInStock, locationMoveOutStock,
                                                locationToPatientStock, locationDamageStock, locationAdjustmentStock, locationLastMonthSaleStock);
                    }
                    AddGrandTotalRow(rows, grandOpeningStock, grandOpeningAmount, grandClosingStock, grandClosingAmount, grandPurchaseStock,
                                                grandPurchaseReturnStock, grandSalesStock, grandSalesReturnStock, grandMoveInStock, grandMoveOutStock,
                                                grandToPatientStock, grandDamageStock, grandAdjustmentStock, grandLastMonthSaleStock);

                    StockReportDataGridView.Rows.AddRange(rows.ToArray());
                    StockReportDataGridView.ResumeLayout();
                }
            }
            else
            {
                ErrorMsg.Text = InformationMsg;
            }
        }
        private void AddDataRow(List<DataGridViewRow> rows, StockReportLineItemsData lineItem, int sno)
        {
            var row = new DataGridViewRow();
            row.CreateCells(StockReportDataGridView,
                sno,
                lineItem.MaterialId,
                lineItem.Name,
                lineItem.Uom,
                lineItem.RackNumber,
                BatchwiseCheck.Checked ? $"{lineItem.Batch + "  "} {(lineItem.BatchExpDate != DateTime.MinValue ? lineItem.BatchExpDate.ToString(Global.Company.DateFormat) : "")}" : "",
                lineItem.OpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.OpeningStockAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)),
                lineItem.CloseingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.ClosingStockAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)),
                lineItem.PurchaseQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.PurchaseReturnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.SalesQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.SalesReturnQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.StockInQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.StockOutQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.ToPatientQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.DamageQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.AdjustQty.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision)),
                lineItem.LastMonthSale.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision))
            );
            rows.Add(row);
        }
        private void AddSubTotalRow(List<DataGridViewRow> rows, double locationOpeningStock, double locationOpeningAmount, double locationClosingStock, double locationClosingAmount,
                                                                double locationPurchaseStock, double locationPurchaseReturnStock, double locationSalesStock, double locationSalesReturnStock,
                                                                double locationMoveInStock, double locationMoveOutStock, double locationToPatientStock, double locationDamageStock,
                                                                double locationAdjustmentStock, double locationLastMonthSaleStock)
        {
            var subtotalRow = new DataGridViewRow();
            subtotalRow.CreateCells(StockReportDataGridView);
            subtotalRow.Cells[(int)StockReportNewTableColumn.BAT].Value = "Subtotal";
            subtotalRow.Cells[(int)StockReportNewTableColumn.OPS].Value = locationOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.OPA].Value = locationOpeningAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.CLSTK].Value = locationClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.CSA].Value = locationClosingAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.P_QTY].Value = locationPurchaseStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.PR_QTY].Value = locationPurchaseReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.S_QTY].Value = locationSalesStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.SR_QTY].Value = locationSalesReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.STIN_QTY].Value = locationMoveInStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.STOT_QTY].Value = locationMoveOutStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.PCON].Value = locationToPatientStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.DAM].Value = locationDamageStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.AD_QTY].Value = locationAdjustmentStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            subtotalRow.Cells[(int)StockReportNewTableColumn.LASTMONTHSALE].Value = locationLastMonthSaleStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            rows.Add(subtotalRow);
        }
        private void AddGrandTotalRow(List<DataGridViewRow> rows, double grandOpeningStock, double grandOpeningAmount, double grandClosingStock, double grandClosingAmount,
                                                                  double grandPurchaseStock, double grandPurchaseReturnStock, double grandSalesStock, double grandSalesReturnStock,
                                                                  double grandMoveInStock, double grandMoveOutStock, double grandToPatientStock, double grandDamageStock,
                                                                  double grandAdjustmentStock, double grandLastMonthSaleStock)
        {
            var grandTotalRow = new DataGridViewRow();
            grandTotalRow.CreateCells(StockReportDataGridView);
            grandTotalRow.Cells[(int)StockReportNewTableColumn.BAT].Value = "Grand Total";
            grandTotalRow.Cells[(int)StockReportNewTableColumn.OPS].Value = grandOpeningStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.OPA].Value = grandOpeningAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.CLSTK].Value = grandClosingStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.CSA].Value = grandClosingAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.P_QTY].Value = grandPurchaseStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.PR_QTY].Value = grandPurchaseReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.S_QTY].Value = grandSalesStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.SR_QTY].Value = grandSalesReturnStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.STIN_QTY].Value = grandMoveInStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.STOT_QTY].Value = grandMoveOutStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.PCON].Value = grandToPatientStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.DAM].Value = grandDamageStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.AD_QTY].Value = grandAdjustmentStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            grandTotalRow.Cells[(int)StockReportNewTableColumn.LASTMONTHSALE].Value = grandLastMonthSaleStock.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
            rows.Add(grandTotalRow);
        }
        private void StockReportDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var LocationCell = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.LOCATION];
            var LastmonthCell = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.LASTMONTHSALE];
            var SubTotalCell = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.BAT];
            var SnoCell = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.SNO];

            if (LocationCell.Value != null && LastmonthCell.Value == null)
            {
                e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            }
            else if (SnoCell.Value == null && SubTotalCell.Value != null)
            {
                if (e.ColumnIndex == (int)StockReportNewTableColumn.SNO || e.ColumnIndex == (int)StockReportNewTableColumn.MID ||
                    e.ColumnIndex == (int)StockReportNewTableColumn.PNAME || e.ColumnIndex == (int)StockReportNewTableColumn.UOM ||
                    e.ColumnIndex == (int)StockReportNewTableColumn.RACKNO)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            if((LocationCell.Value != null && LastmonthCell.Value == null))
            {
                e.CellStyle.BackColor = Color.LightGray;
            }
            if(SnoCell.Value == null && SubTotalCell.Value != null)
            {
                e.CellStyle.BackColor = Color.LightGray;
            }
        }

        private void StockReportDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.LASTMONTHSALE].Value == null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                    0, e.RowBounds.Top,
                    this.StockReportDataGridView.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.StockReportDataGridView.HorizontalScrollingOffset,
                    e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string rr = StockReportDataGridView.Rows[e.RowIndex].Cells[(int)StockReportNewTableColumn.LOCATION].Value?.ToString() ?? string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            StockReportSavePrint StockReportSavePrint = new StockReportSavePrint();
            StockReportSavePrint.ExportOrPrintToFile(RptStockReport, "StockReport", "pdf", false, selectedRackNumbers, selectedManufacturers, selectedSuppliers);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            StockReportSavePrint StockReportSavePrint = new StockReportSavePrint();
            StockReportSavePrint.ExportOrPrintToFile(RptStockReport, "StockReport", "pdf", true, selectedRackNumbers, selectedManufacturers, selectedSuppliers);
        }
    }
}
