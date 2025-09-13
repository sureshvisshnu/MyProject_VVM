using fa;
using fa.api.catalog;
using fa.context;
using fa.libraries.utils;
using fa.model.Catalog;
using fa.model.OrderManagement;
using fa.reports.Inventory;
using fa.views.controls;
using fa.views.controls.ComboListView;
using fa.views.controls.ComboTreeView;
using FADataAccessLibrary.Api.OrderManagement;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Font = System.Drawing.Font;
using Global = fa.Global;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace Fa.reports.sales
{
    public enum ItemSaleReportTableColumn
    {
        SNO, PRODUCT, CUSTOMER,  SALESDATE, BILLNO, RATE, QTY, SALESID
    }
    public partial class FormItemSalesReport : Form
    {
        private long _itemId;
        private string? _itemName;
        private DateTime _fromDate;
        private DateTime _toDate;
        private string? _BillNo;
        private List<long> _selectedItemIds = new List<long>();
        private List<long> _selectedLocationIds = new List<long>();
        private List<long> _selectedCustomerIds = new List<long>();

        public int ReportIndex = 0;

        public FormItemSalesReport(long itemId, string itemName, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            _itemId = itemId;
            _itemName = itemName;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        private void FormItemSalesReport_Load(object sender, EventArgs e)
        {
            this.Text = $"Sales Report for {_itemName}";
            LoadItemInCombo();
            ComboBoxReportType.SelectedIndex = 0; // default to "By Item"
            ComboBoxReportType_SelectedIndexChanged(null!, null!); // apply settings
            GenerateItemSalesReportNew(); // run default report with all data
        }
        private void ResetForm()
        {
            DataGridViewItemSales.DataSource = null;
            LabelTotalQuantity.Text = "0";
            LabelTotalAmount.Text = "0.00";
            LabelTotalSales.Text = "0";
        }

        private void LoadItemInCombo()
        {
            ResetForm();
            ComboUtils.InitializeAllItemComboForReport(ComboBoxSelectItem, Global.Company.CompanyId);
            ComboUtils.InitializeStockLocationCombo(ComboStockReportLocation, Global.Company.CompanyId);
            ComboUtils.InitializeAllCustomerCombo(ComboBoxSelectCustomer, Global.Company.CompanyId);

        }
        private void GenerateItemSalesReport()
        {
            ItemSalesReportErrorMsg.Text = string.Empty;
            EnableButtons(false);

            try
            {
                using (var context = new AccountMasterContext())
                {
                    // 🔹 Reset headers first (important when switching)
                    if (ComboBoxReportType.SelectedIndex == 1)
                    {
                        DataGridViewItemSales.Columns[1].HeaderText = "Customer";
                        DataGridViewItemSales.Columns[2].HeaderText = "Item";
                    }
                    else
                    {
                        DataGridViewItemSales.Columns[1].HeaderText = "Item";
                        DataGridViewItemSales.Columns[2].HeaderText = "Customer";
                    }
                    // inclusive date range: from.Date <= SaleDate < to.Date + 1 day
                    if (ComboBoxReportType.SelectedIndex == 0 && (_fromDate == DateTime.MinValue || _toDate == DateTime.MinValue))
                    {
                        GenerateItemSalesReportNew();
                        return;
                    }

                    if (ComboBoxReportType.SelectedIndex == 1 && (_fromDate == DateTime.MinValue || _toDate == DateTime.MinValue))
                    {
                        GenerateCustomerSalesReportNoDate();
                        return;
                    }
                    // 🔹 handle null dates (if DateTime.MinValue → treat as null)
                    DateTime? fromDate = (_fromDate == DateTime.MinValue) ? (DateTime?)null : _fromDate.Date;
                    DateTime? toDate = (_toDate == DateTime.MinValue) ? (DateTime?)null : _toDate.Date;

                    // base query
                    var entriesQuery = context.SaleEntry.AsQueryable();

                    entriesQuery = ApplyDateFilter(entriesQuery, _fromDate, _toDate);

                    // apply date filter only if dates are provided
                    if (fromDate.HasValue)
                    {
                        entriesQuery = entriesQuery.Where(se => se.SaleDate >= fromDate.Value);
                    }
                    if (toDate.HasValue)
                    {
                        DateTime toExclusive = toDate.Value.AddDays(1);
                        entriesQuery = entriesQuery.Where(se => se.SaleDate < toExclusive);
                    }

                    // Location filter
                    if (_selectedLocationIds != null && _selectedLocationIds.Count > 0)
                    {
                        entriesQuery = entriesQuery.Where(se =>
                            se.InventoryLocationId.HasValue &&
                            _selectedLocationIds.Contains(se.InventoryLocationId.Value));
                    }

                    // 🔹 Decide filter based on ReportType
                    bool isItemReport = ComboBoxReportType.SelectedIndex == 0;

                    var detailsQuery = entriesQuery
                        .SelectMany(se => se.SaleDetails
                            .Where(sd =>
                                sd.ProductId != null &&
                                (
                                    isItemReport
                                        ? (_selectedItemIds == null || _selectedItemIds.Count == 0 ||
                                           _selectedItemIds.Contains(sd.ProductId.Value))
                                        : true // customer filter later
                                )
                            )
                            .Select(sd => new
                            {
                                ProductName = sd.Product != null ? sd.Product.Name : sd.MaterialId,
                                CustomerName = se.CustomerName,
                                CustomerId = se.AccountsId,
                                SaleDate = se.SaleDate,
                                billno = sd.Sale.RefNumber,
                                Quantity = sd.Quantity,
                                Price = sd.Price,
                                SaleId = se.Id
                            })
                        );

                    // Apply customer filter if in Customer mode
                    if (!isItemReport && _selectedCustomerIds != null && _selectedCustomerIds.Count > 0)
                    {
                        detailsQuery = detailsQuery.Where(x => x.CustomerId.HasValue &&
                                                               _selectedCustomerIds.Contains(x.CustomerId.Value));
                    }

                    var rawList = detailsQuery.ToList();

                    var salesData = rawList.Select(x => new ItemSalesReportRow
                    {
                        Product = x.ProductName ?? "(Unknown)",
                        Customer = x.CustomerName ?? "(Unknown)",
                        SaleDate = x.SaleDate,
                        BillNo = x.billno,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        SaleId = x.SaleId
                    }).ToList();

                    if (((isItemReport && _selectedItemIds?.Count > 0) ||
                         (!isItemReport && _selectedCustomerIds?.Count > 0) ||
                         (_selectedLocationIds?.Count > 0)) &&
                        salesData.Count == 0)
                    {
                        ItemSalesReportErrorMsg.Text =
                            "No data matched the selected filters. Try clearing filters to confirm data exists.";
                    }

                    // Clear old data
                    DataGridViewItemSales.Rows.Clear();
                    if (salesData.Count == 0)
                    {
                        LabelTotalQuantity.Text = "0.00";
                        LabelTotalAmount.Text = "0.00";
                        LabelTotalSales.Text = "0";
                        return;
                    }

                    // 🔹 Group differently depending on mode
                    var groupedData = isItemReport
                        ? salesData.GroupBy(s => s.Product).ToList()
                        : salesData.GroupBy(s => s.Customer).ToList();

                    int sno = 1;
                    double totalQty = 0;
                    double totalAmount = 0;

                    foreach (var group in groupedData)
                    {
                        // Header row (Product or Customer)
                        int headerIndex = DataGridViewItemSales.Rows.Add();
                        var headerRow = DataGridViewItemSales.Rows[headerIndex];
                        headerRow.DefaultCellStyle.BackColor = Color.LightGray;
                        headerRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
                        headerRow.Cells[1].Value = group.Key;
                        headerRow.Cells[1].Style.ForeColor = Color.DarkBlue;
                        headerRow.ReadOnly = true;

                        // Child rows
                        foreach (var item in group)
                        {
                            int rowIndex = DataGridViewItemSales.Rows.Add();
                            var row = DataGridViewItemSales.Rows[rowIndex];

                            row.Cells[(int)ItemSaleReportTableColumn.SNO].Value = sno++;
                            row.Cells[(int)ItemSaleReportTableColumn.PRODUCT].Value = "";
                            row.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = isItemReport ? "   " + item.Customer : "   " + item.Product;
                            row.Cells[(int)ItemSaleReportTableColumn.SALESDATE].Value = item.SaleDate.ToString("dd-MM-yyyy");
                            row.Cells[(int)ItemSaleReportTableColumn.BILLNO].Value = item.BillNo!.ToString()!;
                            row.Cells[(int)ItemSaleReportTableColumn.RATE].Value = item.Price.ToString("N2");
                            row.Cells[(int)ItemSaleReportTableColumn.QTY].Value = item.Quantity.ToString("N2");

                            totalQty += item.Quantity;
                            totalAmount += (item.Price * item.Quantity);
                        }

                        DataGridViewItemSales.Rows.Add(); // spacer
                    }

                    // Summary row
                    int summaryIndex = DataGridViewItemSales.Rows.Add();
                    var summaryRow = DataGridViewItemSales.Rows[summaryIndex];
                    summaryRow.DefaultCellStyle.BackColor = Color.Beige;
                    summaryRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = "TOTAL";
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.RATE].Value = totalAmount.ToString("N2");
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.QTY].Value = totalQty.ToString("N2");

                    LabelTotalQuantity.Text = totalQty.ToString("N2");
                    LabelTotalAmount.Text = totalAmount.ToString("N2");
                    LabelTotalSales.Text = salesData.Count.ToString();

                    DataGridViewItemSales.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    DataGridViewItemSales.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                ItemSalesReportErrorMsg.Text = $"Error generating report: {ex.Message}";
            }
            finally
            {
                EnableButtons(true);
            }
        }

        private void GenerateCustomerSalesReportNoDate()
        {
            ItemSalesReportErrorMsg.Text = string.Empty;
            EnableButtons(false);

            try
            {
                using (var context = new AccountMasterContext())
                {
                    var entriesQuery = context.SaleEntry.AsQueryable();

                    // Location filter
                    if (_selectedLocationIds != null && _selectedLocationIds.Count > 0)
                    {
                        entriesQuery = entriesQuery.Where(se =>
                            se.InventoryLocationId.HasValue &&
                            _selectedLocationIds.Contains(se.InventoryLocationId.Value));
                    }

                    var detailsQuery = entriesQuery
                        .SelectMany(se => se.SaleDetails
                            .Where(sd => sd.ProductId != null)
                            .Select(sd => new
                            {
                                ProductName = sd.Product != null ? sd.Product.Name : sd.MaterialId,
                                CustomerName = se.CustomerName,
                                CustomerId = se.AccountsId,
                                SaleDate = se.SaleDate,
                                billno = se.RefNumber,
                                Quantity = sd.Quantity,
                                Price = sd.Price,
                                SaleId = se.Id
                            })
                        );

                    // Customer filter
                    if (_selectedCustomerIds != null && _selectedCustomerIds.Count > 0)
                    {
                        detailsQuery = detailsQuery.Where(x => x.CustomerId.HasValue &&
                                                               _selectedCustomerIds.Contains(x.CustomerId.Value));
                    }

                    var rawList = detailsQuery.ToList();

                    var salesData = rawList.Select(x => new ItemSalesReportRow
                    {
                        Product = x.ProductName ?? "(Unknown)",
                        Customer = x.CustomerName ?? "(Unknown)",
                        SaleDate = x.SaleDate,
                        BillNo = x.billno,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        SaleId = x.SaleId
                    }).ToList();

                    RenderReportGrid(salesData, groupByCustomer: true);
                }
            }
            catch (Exception ex)
            {
                ItemSalesReportErrorMsg.Text = $"Error generating report: {ex.Message}";
            }
            finally
            {
                EnableButtons(true);
            }
        }

        private void GenerateItemSalesReportNew()
        {
            ItemSalesReportErrorMsg.Text = string.Empty;
            EnableButtons(false);

            try
            {
                using (var context = new AccountMasterContext())
                {
                    // ✅ Normalize dates: if not selected, default to full range
                    DateTime fromDate = (_fromDate == DateTime.MinValue) ? DateTime.MinValue : _fromDate.Date;
                    DateTime toDate = (_toDate == DateTime.MinValue) ? DateTime.MaxValue : _toDate.Date;

                    DateTime? toExclusive = DateTime.MaxValue;
                    if (toDate == DateTime.MaxValue)
                    {
                        toExclusive = toDate.AddDays(-1); // safe now
                    }

                    // Base query
                    var entriesQuery = context.SaleEntry.AsQueryable();

                    if (fromDate > DateTime.MinValue)
                        entriesQuery = entriesQuery.Where(se => se.SaleDate >= fromDate);

                    if (toExclusive.HasValue)
                        entriesQuery = entriesQuery.Where(se => se.SaleDate < toExclusive.Value);

                    // 🔹 Filter by location
                    if (_selectedLocationIds != null && _selectedLocationIds.Count > 0)
                    {
                        entriesQuery = entriesQuery.Where(se =>
                            se.InventoryLocationId.HasValue &&
                            _selectedLocationIds.Contains(se.InventoryLocationId.Value));
                    }

                    // 🔹 Select SaleDetails and apply product filter
                    var detailsQuery = entriesQuery
                        .SelectMany(se => se.SaleDetails
                            .Where(sd => sd.ProductId != null &&
                                         (_selectedItemIds == null || _selectedItemIds.Count == 0 ||
                                          _selectedItemIds.Contains(sd.ProductId.Value)))
                            .Select(sd => new
                            {
                                ProductName = sd.Product != null ? sd.Product.Name : sd.MaterialId,
                                CustomerName = se.CustomerName,
                                SaleDate = se.SaleDate,
                                billno = sd.Sale.RefNumber,
                                Quantity = sd.Quantity,
                                Price = sd.Price,
                                SaleId = se.Id
                            })
                        );

                    var rawList = detailsQuery.ToList();

                    var salesData = rawList.Select(x => new ItemSalesReportRow
                    {
                        Product = x.ProductName ?? "(Unknown)",
                        Customer = x.CustomerName,
                        SaleDate = x.SaleDate,
                        BillNo = x.billno,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        SaleId = x.SaleId
                    }).ToList();

                    // 🔹 Show warning if nothing found
                    if ((_selectedItemIds?.Count > 0 || _selectedLocationIds?.Count > 0) && salesData.Count == 0)
                    {
                        ItemSalesReportErrorMsg.Text = "No data matched the selected Product / Location / Date range. Try clearing filters to confirm data exists.";
                    }

                    // 🔹 Render grid
                    DataGridViewItemSales.Rows.Clear();

                    if (salesData.Count == 0)
                    {
                        LabelTotalQuantity.Text = "0.00";
                        LabelTotalAmount.Text = "0.00";
                        LabelTotalSales.Text = "0";
                        return;
                    }

                    var groupedData = salesData.GroupBy(s => s.Product).ToList();

                    int sno = 1;
                    double totalQty = 0;
                    double totalAmount = 0;

                    foreach (var productGroup in groupedData)
                    {
                        int headerIndex = DataGridViewItemSales.Rows.Add();
                        var headerRow = DataGridViewItemSales.Rows[headerIndex];
                        headerRow.DefaultCellStyle.BackColor = Color.LightGray;
                        headerRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
                        headerRow.Cells[1].Value = productGroup.Key;
                        headerRow.Cells[1].Style.ForeColor = Color.DarkBlue;
                        headerRow.ReadOnly = true;

                        foreach (var item in productGroup)
                        {
                            int rowIndex = DataGridViewItemSales.Rows.Add();
                            var row = DataGridViewItemSales.Rows[rowIndex];

                            row.Cells[(int)ItemSaleReportTableColumn.SNO].Value = sno++;
                            row.Cells[(int)ItemSaleReportTableColumn.PRODUCT].Value = "";
                            row.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = "   " + item.Customer;
                            row.Cells[(int)ItemSaleReportTableColumn.SALESDATE].Value = item.SaleDate.ToString("dd-MM-yyyy");
                            row.Cells[(int)ItemSaleReportTableColumn.BILLNO].Value = item.BillNo!.ToString();
                            row.Cells[(int)ItemSaleReportTableColumn.RATE].Value = item.Price.ToString("N2");
                            row.Cells[(int)ItemSaleReportTableColumn.QTY].Value = item.Quantity.ToString("N2");

                            totalQty += item.Quantity;
                            totalAmount += (item.Price * item.Quantity);
                        }

                        DataGridViewItemSales.Rows.Add(); // spacer
                    }

                    int summaryIndex = DataGridViewItemSales.Rows.Add();
                    var summaryRow = DataGridViewItemSales.Rows[summaryIndex];
                    summaryRow.DefaultCellStyle.BackColor = Color.Beige;
                    summaryRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = "TOTAL";
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.RATE].Value = totalAmount.ToString("N2");
                    summaryRow.Cells[(int)ItemSaleReportTableColumn.QTY].Value = totalQty.ToString("N2");

                    LabelTotalQuantity.Text = totalQty.ToString("N2");
                    LabelTotalAmount.Text = totalAmount.ToString("N2");
                    LabelTotalSales.Text = salesData.Count.ToString();

                    DataGridViewItemSales.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    DataGridViewItemSales.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
        }
            catch (Exception ex)
            {
                ItemSalesReportErrorMsg.Text = $"Error generating report: {ex.Message}";
            }
            finally
            {
                EnableButtons(true);
}
        }

        private void RenderReportGrid(List<ItemSalesReportRow> salesData, bool groupByCustomer)
        {
            DataGridViewItemSales.Rows.Clear();

            // 🔹 Set headers dynamically
            if (groupByCustomer)
            {
                DataGridViewItemSales.Columns[1].HeaderText = "Customer";
                DataGridViewItemSales.Columns[2].HeaderText = "Item";
            }
            else
            {
                DataGridViewItemSales.Columns[1].HeaderText = "Item";
                DataGridViewItemSales.Columns[2].HeaderText = "Customer";
            }

            if (salesData.Count == 0)
            {
                LabelTotalQuantity.Text = "0.00";
                LabelTotalAmount.Text = "0.00";
                LabelTotalSales.Text = "0";
                return;
            }

            var groupedData = groupByCustomer
                ? salesData.GroupBy(s => s.Customer).ToList()
                : salesData.GroupBy(s => s.Product).ToList();

            int sno = 1;
            double totalQty = 0;
            double totalAmount = 0;

            foreach (var group in groupedData)
            {
                int headerIndex = DataGridViewItemSales.Rows.Add();
                var headerRow = DataGridViewItemSales.Rows[headerIndex];
                headerRow.DefaultCellStyle.BackColor = Color.LightGray;
                headerRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
                headerRow.Cells[1].Value = group.Key;
                headerRow.Cells[1].Style.ForeColor = Color.DarkBlue;
                headerRow.ReadOnly = true;

                foreach (var item in group)
                {
                    int rowIndex = DataGridViewItemSales.Rows.Add();
                    var row = DataGridViewItemSales.Rows[rowIndex];

                    row.Cells[(int)ItemSaleReportTableColumn.SNO].Value = sno++;
                    row.Cells[(int)ItemSaleReportTableColumn.PRODUCT].Value = "";
                    row.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = groupByCustomer ? "   " + item.Product : "   " + item.Customer;
                    row.Cells[(int)ItemSaleReportTableColumn.SALESDATE].Value = item.SaleDate.ToString("dd-MM-yyyy");
                    row.Cells[(int)ItemSaleReportTableColumn.BILLNO].Value = item.BillNo!.ToString();
                    row.Cells[(int)ItemSaleReportTableColumn.RATE].Value = item.Price.ToString("N2");
                    row.Cells[(int)ItemSaleReportTableColumn.QTY].Value = item.Quantity.ToString("N2");

                    totalQty += item.Quantity;
                    totalAmount += (item.Price * item.Quantity);
                }

                DataGridViewItemSales.Rows.Add();
            }

            int summaryIndex = DataGridViewItemSales.Rows.Add();
            var summaryRow = DataGridViewItemSales.Rows[summaryIndex];
            summaryRow.DefaultCellStyle.BackColor = Color.Beige;
            summaryRow.DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);
            summaryRow.Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = "TOTAL";
            summaryRow.Cells[(int)ItemSaleReportTableColumn.RATE].Value = totalAmount.ToString("N2");
            summaryRow.Cells[(int)ItemSaleReportTableColumn.QTY].Value = totalQty.ToString("N2");

            LabelTotalQuantity.Text = totalQty.ToString("N2");
            LabelTotalAmount.Text = totalAmount.ToString("N2");
            LabelTotalSales.Text = salesData.Count.ToString();

            DataGridViewItemSales.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DataGridViewItemSales.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }


        private void EnableButtons(bool enable)
        {
            //ButtonExport.Enabled = enable;
            //ButtonRefresh.Enabled = enable;
            // Add other buttons as needed
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            _fromDate = (StockReportFromDate.Date ?? DateTime.MinValue).Date;
            _toDate = (StockReportToDate.Date ?? DateTime.MaxValue).Date;

            // Clear selections
            _selectedItemIds.Clear();
            _selectedLocationIds.Clear();
            _selectedCustomerIds.Clear();

            // Update locations (always needed)
            UpdateSelectedLocationsFromCheckedComboBox();

            // Decide based on report type
            if (ComboBoxReportType.SelectedIndex == 0) // Item
            {
                UpdateSelectedItemsFromCheckedComboBox();
            }
            else if (ComboBoxReportType.SelectedIndex == 1) // Customer
            {
                UpdateSelectedCustomersFromCheckedComboBox();
            }

            // Update title for debugging
            DisplayCheckedInformation();

            GenerateItemSalesReport();
        }


        private void ComboBoxSelectItem_NodeClickedEvent(object sender, fa.views.controls.ComboTreeView.ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
        private void DisplayCheckedInformation()
        {
            string locationNodes = GetCheckedNodes(ComboStockReportLocation, "@ All Location");
            string displayFor = "";
            string title = "Item Sales Report";

            if (ComboBoxReportType.SelectedIndex == 0) // Item
            {
                string itemNodes = GetCheckedNodes(ComboBoxSelectItem, "@ All Items");
                displayFor = itemNodes + " @ " + SelectedNodesText(ComboStockReportLocation);
            }
            else if (ComboBoxReportType.SelectedIndex == 1) // Customer
            {
                string customerNodes = GetCheckedNodes(ComboBoxSelectCustomer, "@ All Customers");
                displayFor = customerNodes + " @ " + SelectedNodesText(ComboStockReportLocation);
            }

            string displayLocation = "";
            if (!string.IsNullOrEmpty(locationNodes) && locationNodes != "@ All Location")
            {
                displayLocation = " For " + displayFor + " @ Location ";
                title += AppendToTitleIfNotEmpty(locationNodes, displayLocation);
            }
            else
            {
                title += " For " + displayFor;
            }

            this.Text = title;
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
        private string AppendToTitleIfNotEmpty(string nodes, string prefix)
        {
            return string.IsNullOrEmpty(nodes) ? "" : $"{prefix} {nodes}";
        }
        private void DisplayCheckedInformationz()
        {
            string productFamilyText = string.Empty;
            string productLocationText = string.Empty;
            string productCategoryText = string.Empty;
            string productItemText = string.Empty;
            string productSupplierText = string.Empty;
            string productManufacturerText = string.Empty;

            productItemText = GetCheckedNodesText(ComboBoxSelectItem, "All");

            this.Text = "Stock Report";
            if (!string.IsNullOrEmpty(productItemText)) this.Text += " @ " + productItemText;
            if (!string.IsNullOrEmpty(productLocationText)) this.Text += " @ " + productLocationText;
        }
        private string GetCheckedNodesText(ToolstripCheckedTreeComboBox comboBox, string allText)
        {
            if (comboBox.CheckedNodes.Count == 0) return "";

            var checkedNodes = comboBox.CheckedNodes.ToList();
            if (checkedNodes.Any(node => node.Name == "All")) return allText;

            return string.Join(", ", checkedNodes.Select(node => node.Text));
        }
        // ------------- ******** -------------

        private void UpdateSelectedLocationsFromCheckedComboBox()
        {
            _selectedLocationIds = GetCheckedIdsFromCheckedTreeComboBox(ComboStockReportLocation);
        }

        private void TraverseLocationNodes(TreeNode node)
        {
            if (IsNodeChecked(node))
            {
                long id = GetIdFromNode(node);
                if (id != 0) _selectedLocationIds.Add(id);
                else if (string.Equals(node.Text, "All", StringComparison.OrdinalIgnoreCase))
                {
                    // collect all children
                    AddAllIdsFrom(root: node, into: _selectedLocationIds);
                    return;
                }
            }
            foreach (TreeNode c in node.Nodes) TraverseLocationNodes(c);
        }

        private long GetIdFromNode(TreeNode node)
        {
            if (node == null) return 0;
            try
            {
                if (node.Tag is KeyValuePair<string, long> kvp) return kvp.Value;
                if (node.Tag is long l) return l;
                if (node.Tag != null)
                {
                    var prop = node.Tag.GetType().GetProperty("Id");
                    if (prop != null)
                    {
                        var val = prop.GetValue(node.Tag);
                        if (val != null && long.TryParse(val.ToString(), out long parsed)) return parsed;
                    }
                }
                if (long.TryParse(node.Name, out long fromName)) return fromName;
            }
            catch { }
            return 0;
        }

        private void AddAllIdsFrom(TreeNode root, List<long> into)
        {
            if (root == null) return;
            foreach (TreeNode n in root.Nodes)
            {
                var id = GetIdFromNode(n);
                if (id != 0) into.Add(id);
                if (n.Nodes != null && n.Nodes.Count > 0) AddAllIdsFrom(n, into);
            }
        }

        private long GetLocationIdFromNode(TreeNode node)
        {
            if (node.Tag is KeyValuePair<string, long> kvp)
                return kvp.Value;
            if (node.Tag is long id)
                return id;
            if (long.TryParse(node.Name, out long parsed))
                return parsed;
            return 0;
        }

        private void AddAllLocationIds()
        {
            _selectedLocationIds.Clear();
            var rootNode = GetRootNode(ComboStockReportLocation);
            if (rootNode != null)
            {
                AddLocationIdsFromNodes(rootNode);
            }
        }

        private void AddLocationIdsFromNodes(TreeNode node)
        {
            if (node.Text != "All")
            {
                long locationId = GetLocationIdFromNode(node);
                if (locationId != 0)
                    _selectedLocationIds.Add(locationId);
            }
            foreach (TreeNode child in node.Nodes)
            {
                AddLocationIdsFromNodes(child);
            }
        }

        // overload GetRootNode to handle both item combo and location combo
        private TreeNode GetRootNode(object combo)
        {
            if (combo == null) return null!;
            try
            {
                var prop = combo.GetType().GetProperty("RootNode");
                if (prop != null)
                    return prop.GetValue(combo) as TreeNode;

                var nodesProp = combo.GetType().GetProperty("Nodes");
                if (nodesProp != null)
                {
                    var nodes = nodesProp.GetValue(combo) as TreeNodeCollection;
                    if (nodes != null && nodes.Count > 0) return nodes[0];
                }
            }
            catch { }
            return null!;
        }



        // ------------- ******** -------------

        // returns distinct product/location ids (0 or more). If "All" found it returns all product ids.
        private List<long> GetCheckedIdsFromCheckedTreeComboBox(object combo)
        {
            var ids = new List<long>();
            if (combo == null) return ids;

            try
            {
                // Try to read a CheckedNodes property (your control exposes this in other methods)
                var prop = combo.GetType().GetProperty("CheckedNodes");
                if (prop != null)
                {
                    var checkedNodes = prop.GetValue(combo) as System.Collections.IEnumerable;
                    if (checkedNodes != null)
                    {
                        foreach (var nodeObj in checkedNodes)
                        {
                            // Check for an "All" node by Name or Text
                            var nameVal = TryGetPropertyValue(nodeObj, "Name") ?? TryGetPropertyValue(nodeObj, "Text");
                            if (nameVal != null && nameVal.ToString() == "All")
                            {
                                // return all product ids (or all locations) depending on which combo
                                return GetAllIdsForCombo(combo);
                            }

                            long id = TryGetIdFromNodeObject(nodeObj);
                            if (id > 0) ids.Add(id);
                        }
                    }
                }

                // If none found via CheckedNodes, fallback to RootNode -> traverse
                if (ids.Count == 0)
                {
                    var root = GetRootNode(combo);
                    if (root != null)
                    {
                        TraverseCollectIds(root, ids);
                    }
                }
            }
            catch
            {
                // swallow - return what we collected so far
            }

            return ids.Distinct().ToList();
        }
        private object TryGetPropertyValue(object obj, string propName)
        {
            if (obj == null) return null;
            try
            {
                var p = obj.GetType().GetProperty(propName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (p != null) return p.GetValue(obj)!;
            }
            catch { }
            return null!;
        }
        private long TryGetIdFromNodeObject(object nodeObj)
        {
            if (nodeObj == null) return 0;
            try
            {
                // 1) Tag property
                var tag = TryGetPropertyValue(nodeObj, "Tag");
                if (tag != null)
                {
                    // KeyValuePair<string,long>
                    if (tag is System.Collections.DictionaryEntry de)
                    {
                        if (long.TryParse(de.Value?.ToString(), out long v1)) return v1;
                    }

                    if (tag is KeyValuePair<string, long> kvp) return kvp.Value;
                    if (tag is long l) return l;
                    if (tag is int i) return i;

                    // try Id property on tag object
                    var idFromTagProp = TryGetPropertyValue(tag, "Id") ?? TryGetPropertyValue(tag, "ID") ?? TryGetPropertyValue(tag, "Value");
                    if (idFromTagProp != null && long.TryParse(idFromTagProp.ToString(), out long parsedTagId)) return parsedTagId;
                }

                // 2) direct Id/ID/Value property on node object
                var idVal = TryGetPropertyValue(nodeObj, "Id") ?? TryGetPropertyValue(nodeObj, "ID") ?? TryGetPropertyValue(nodeObj, "Value");
                if (idVal != null && long.TryParse(idVal.ToString(), out long parsedId)) return parsedId;

                // 3) Name property (sometimes the node.Name stores numeric id)
                var nameVal = TryGetPropertyValue(nodeObj, "Name");
                if (nameVal != null && long.TryParse(nameVal.ToString(), out long parsedName)) return parsedName;
            }
            catch { }

            return 0;
        }

        private void TraverseCollectIds(System.Windows.Forms.TreeNode node, List<long> into)
        {
            if (node == null) return;
            try
            {
                if (node.Checked)
                {
                    long id = GetIdFromNode(node); // you already have GetIdFromNode(TreeNode) in your class that handles Tag/Product
                    if (id != 0) into.Add(id);
                    else if (string.Equals(node.Text, "All", StringComparison.OrdinalIgnoreCase))
                    {
                        AddAllIdsFrom(node, into);
                        return;
                    }
                }
                foreach (System.Windows.Forms.TreeNode child in node.Nodes)
                    TraverseCollectIds(child, into);
            }
            catch { }
        }
        private List<long> GetAllIdsForCombo(object combo)
        {
            // If products combo -> use Global.ProductDetailList ideally
            // If locations combo -> try to get location list (your app probably has a method)
            if (combo == ComboBoxSelectItem)
            {
                // ensure global product list exists
                if (Global.ProductDetailList == null || Global.ProductDetailList.Count == 0)
                {
                    Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
                }
                return Global.ProductDetailList?.Where(p => p != null).Select(p => p.Id).ToList() ?? new List<long>();
            }
            else if (combo == ComboStockReportLocation)
            {
                // try to read DataSource or Items of combo to extract location ids
                var items = GetItemsFromTreeComboBox();
                var ids = new List<long>();
                foreach (var it in items)
                {
                    try
                    {
                        // item could be KeyValuePair<string,long> or object with Id property
                        if (it is KeyValuePair<string, long> kvp) ids.Add(kvp.Value);
                        else
                        {
                            var idProp = it.GetType().GetProperty("Id") ?? it.GetType().GetProperty("ID") ?? it.GetType().GetProperty("Value");
                            if (idProp != null)
                            {
                                var v = idProp.GetValue(it);
                                if (v != null && long.TryParse(v.ToString(), out long parsed)) ids.Add(parsed);
                            }
                        }
                    }
                    catch { }
                }
                return ids.Distinct().ToList();
            }

            return new List<long>();
        }
        private List<object> GetCheckedItemsFromTreeComboBox()
        {
            var checkedItems = new List<object>();

            // Try different approaches to get checked items
            // Approach 1: Check if there's a GetCheckedItems method
            try
            {
                var method = ComboBoxSelectItem.GetType().GetMethod("GetCheckedItems");
                if (method != null)
                {
                    return method.Invoke(ComboBoxSelectItem, null) as List<object> ?? new List<object>();
                }
            }
            catch { }

            // Approach 2: Check if there's a CheckedItems property
            try
            {
                var property = ComboBoxSelectItem.GetType().GetProperty("CheckedItems");
                if (property != null)
                {
                    return property.GetValue(ComboBoxSelectItem) as List<object> ?? new List<object>();
                }
            }
            catch { }

            // Approach 3: Check if there's a SelectedItems property that works for checked items
            try
            {
                var property = ComboBoxSelectItem.GetType().GetProperty("SelectedItems");
                if (property != null)
                {
                    return property.GetValue(ComboBoxSelectItem) as List<object> ?? new List<object>();
                }
            }
            catch { }

            return checkedItems;
        }

        //private void AddAllProductIds()
        //{
        //    _selectedItemIds.Clear();

        //    // Get all items from the combo box
        //    var allItems = GetItemsFromTreeComboBox();

        //    foreach (var item in allItems)
        //    {
        //        if (item is KeyValuePair<string, long> kvp && kvp.Value != 0)
        //        {
        //            _selectedItemIds.Add(kvp.Value);
        //        }
        //        else if (item is Product product)
        //        {
        //            _selectedItemIds.Add(product.Id);
        //        }
        //    }
        //}

        private List<object> GetItemsFromTreeComboBox()
        {
            var items = new List<object>();

            // Try different approaches to get items
            // Approach 1: Check if there's an Items property
            try
            {
                var property = ComboBoxSelectItem.GetType().GetProperty("Items");
                if (property != null)
                {
                    return property.GetValue(ComboBoxSelectItem) as List<object> ?? new List<object>();
                }
            }
            catch { }

            // Approach 2: Check if there's a DataSource property
            try
            {
                var property = ComboBoxSelectItem.GetType().GetProperty("DataSource");
                if (property != null)
                {
                    return property.GetValue(ComboBoxSelectItem) as List<object> ?? new List<object>();
                }
            }
            catch { }

            return items;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {

        }
        //  Update window title based on selected items 
        // ------------- ******** -------------

        private void UpdateSelectedItemsFromCheckedComboBox()
        {
            _selectedItemIds = GetCheckedIdsFromCheckedTreeComboBox(ComboBoxSelectItem);
        }
        private void UpdateSelectedCustomersFromCheckedComboBox()
        {
            _selectedCustomerIds = GetCheckedIdsFromCheckedTreeComboBox(ComboBoxSelectCustomer);
        }

        private void TraverseNodes(TreeNode node) // your existing product traversal
        {
            if (IsNodeChecked(node))
            {
                long id = GetIdFromNode(node);
                if (id != 0) _selectedItemIds.Add(id);
                else if (string.Equals(node.Text, "All", StringComparison.OrdinalIgnoreCase))
                {
                    AddAllIdsFrom(root: node, into: _selectedItemIds);
                    return;
                }
            }
            foreach (TreeNode c in node.Nodes) TraverseNodes(c);
        }

        private TreeNode GetRootNode()
        {
            // Try to get the root node of the tree
            try
            {
                var property = ComboBoxSelectItem.GetType().GetProperty("RootNode");
                if (property != null)
                {
                    return property.GetValue(ComboBoxSelectItem) as TreeNode;
                }

                // Alternatively, try to get Nodes collection
                var nodesProperty = ComboBoxSelectItem.GetType().GetProperty("Nodes");
                if (nodesProperty != null)
                {
                    var nodes = nodesProperty.GetValue(ComboBoxSelectItem) as TreeNodeCollection;
                    return nodes?.Count > 0 ? nodes[0] : null!;
                }
            }
            catch { }

            return null;
        }

        private bool IsNodeChecked(TreeNode node)
        {
            try
            {
                return node.Checked; // typical TreeNode property
            }
            catch
            {
                return false;
            }
        }

        private long GetProductIdFromNode(TreeNode node)
        {
            // Extract product ID from node tag or text
            if (node.Tag is KeyValuePair<string, long> kvp)
            {
                return kvp.Value;
            }
            else if (node.Tag is long productId)
            {
                return productId;
            }
            else if (node.Tag is Product product)
            {
                return product.Id;
            }

            // Try to parse from node text or name
            if (long.TryParse(node.Name, out long id))
            {
                return id;
            }

            return 0;
        }

        private void AddAllProductIds()
        {
            _selectedItemIds.Clear();

            // Traverse all nodes and add product IDs
            var rootNode = GetRootNode();
            if (rootNode != null)
            {
                AddProductIdsFromNodes(rootNode);
            }
        }
        private void AddProductIdsFromNodes(TreeNode node)
        {
            // Skip the "All" node itself
            if (node.Text != "All")
            {
                long productId = GetProductIdFromNode(node);
                if (productId != 0)
                {
                    _selectedItemIds.Add(productId);
                }
            }

            // Recursively process child nodes
            foreach (TreeNode childNode in node.Nodes)
            {
                AddProductIdsFromNodes(childNode);
            }
        }

        private void ClearTreeNodes()
        {
            try
            {
                var method = ComboBoxSelectItem.GetType().GetMethod("Clear");
                if (method != null)
                {
                    method.Invoke(ComboBoxSelectItem, null);
                }

                var nodesProperty = ComboBoxSelectItem.GetType().GetProperty("Nodes");
                if (nodesProperty != null && nodesProperty.CanWrite)
                {
                    // nodesProperty.SetValue(ComboBoxSelectItem, new TreeNodeCollection());
                }
            }
            catch { }
        }

        private void SetTreeNodes(TreeNode rootNode)
        {
            try
            {
                var nodesProperty = ComboBoxSelectItem.GetType().GetProperty("Nodes");
                if (nodesProperty != null && nodesProperty.CanWrite)
                {
                    // You might need to add nodes one by one
                }

                // Alternatively, set the root node
                var rootProperty = ComboBoxSelectItem.GetType().GetProperty("RootNode");
                if (rootProperty != null && rootProperty.CanWrite)
                {
                    rootProperty.SetValue(ComboBoxSelectItem, rootNode);
                }
            }
            catch { }
        }
        private void SelectTreeNode(long itemId)
        {
            try
            {
                // Find the node with the specified ID and check it
                var rootNode = GetRootNode();
                if (rootNode != null)
                {
                    TreeNode foundNode = FindTreeNode(rootNode, itemId);
                    if (foundNode != null)
                    {
                        SetNodeChecked(foundNode, true);
                    }
                }
            }
            catch { }
        }

        private TreeNode FindTreeNode(TreeNode parentNode, long itemId)
        {
            if (GetProductIdFromNode(parentNode) == itemId)
            {
                return parentNode;
            }

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                TreeNode found = FindTreeNode(childNode, itemId);
                if (found != null)
                {
                    return found;
                }
            }

            return null!;
        }

        private void SetNodeChecked(TreeNode node, bool isChecked)
        {
            try
            {
                var checkedProperty = node.GetType().GetProperty("Checked");
                if (checkedProperty != null)
                {
                    checkedProperty.SetValue(node, isChecked);
                }
            }
            catch { }
        }

        // ************ ------------- Export to Excel ------------- ************
        private void ExportToExcel(List<ItemSalesReportRow> salesData, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Item Sales Report");

                int row = 1;

                // Title
                ws.Cells[row, 1].Value = "Item Sales Report";
                ws.Cells[row, 1, row, 6].Merge = true;
                ws.Cells[row, 1].Style.Font.Bold = true;
                ws.Cells[row, 1].Style.Font.Size = 14;
                ws.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                row += 2;

                // Headers
                ws.Cells[row, 1].Value = "S.No";
                ws.Cells[row, 2].Value = "Product";
                ws.Cells[row, 3].Value = "Customer";
                ws.Cells[row, 4].Value = "Date";
                ws.Cells[row, 5].Value = "Bill No";
                ws.Cells[row, 6].Value = "Price";
                ws.Cells[row, 7].Value = "Quantity";
                ws.Row(row).Style.Font.Bold = true;
                row++;

                int sno = 1;
                double totalQty = 0;
                double totalAmount = 0;

                var grouped = salesData.GroupBy(s => s.Product);

                foreach (var group in grouped)
                {
                    // Product row
                    ws.Cells[row, 2].Value = group.Key;
                    ws.Row(row).Style.Font.Bold = true;
                    ws.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    row++;

                    // Customer rows
                    foreach (var item in group)
                    {
                        ws.Cells[row, 1].Value = sno++;
                        ws.Cells[row, 3].Value = item.Customer;
                        ws.Cells[row, 4].Value = item.SaleDate.ToString("dd-MM-yyyy");
                        ws.Cells[row, 5].Value = item.Price;
                        ws.Cells[row, 6].Value = item.Quantity;

                        totalQty += item.Quantity;
                        totalAmount += item.Price * item.Quantity;
                        row++;
                    }

                    row++; // space between products
                }

                // Summary row
                ws.Cells[row, 3].Value = "TOTAL";
                ws.Cells[row, 3].Style.Font.Bold = true;
                ws.Cells[row, 5].Value = totalAmount;
                ws.Cells[row, 6].Value = totalQty;
                ws.Row(row).Style.Font.Bold = true;
                ws.Row(row).Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Row(row).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Beige);

                // Formatting
                ws.Cells[4, 5, row, 5].Style.Numberformat.Format = "#,##0.00"; // Price
                ws.Cells[4, 6, row, 6].Style.Numberformat.Format = "#,##0.00"; // Qty

                ws.Cells.AutoFitColumns();

                // Save file
                package.SaveAs(new FileInfo(filePath));
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Files|*.xlsx";
                    sfd.FileName = "ItemSalesReport.xlsx";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Recreate the same list you used for grid
                        using (var context = new AccountMasterContext())
                        {
                            var data = context.SaleEntry
                                .Where(se => se.SaleDate >= _fromDate && se.SaleDate <= _toDate)
                                .SelectMany(se => se.SaleDetails.Select(sd => new ItemSalesReportRow
                                {
                                    Product = sd.Product.Name,
                                    Customer = se.CustomerName,
                                    SaleDate = se.SaleDate,
                                    Quantity = sd.Quantity,
                                    Price = sd.Price,
                                    SaleId = se.Id
                                }))
                                .ToList();

                            ExportToExcel(data, sfd.FileName);
                            MessageBox.Show("Report exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}");
            }
        }
        private IQueryable<SaleEntry> ApplyDateFilter(IQueryable<SaleEntry> query, DateTime fromDate, DateTime toDate)
        {
            // Apply fromDate filter only if it's valid
            if (fromDate != DateTime.MinValue)
            {
                query = query.Where(se => se.SaleDate >= fromDate.Date);
            }

            // Apply toDate filter only if it's valid
            if (toDate != DateTime.MaxValue)
            {
                DateTime toExclusive = toDate.Date.AddDays(1); // safe, since not MaxValue
                query = query.Where(se => se.SaleDate < toExclusive);
            }

            return query;
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();

            if (ComboBoxReportType.SelectedIndex == 0)
            {
                ReportIndex = 0;
                ComboBoxReportType.Text = "By Item";
                ComboBoxSelectItem.Visible = true;
                ComboBoxSelectCustomer.Visible = false;
                ComboBoxSelectItem.Size = new Size(200, 25);
                LabelSelectItem.Visible = true;
                LabelSelectItem.Text = "Items";

            }
            else if (ComboBoxReportType.SelectedIndex == 1)
            {
                ReportIndex = 1;
                ComboBoxReportType.Text = "By Customer";
                ComboBoxSelectCustomer.Visible = true;
                ComboBoxSelectItem.Visible = false;
                ComboBoxSelectCustomer.Size = new Size(200, 25);
                LabelSelectItem.Visible = true;
                LabelSelectItem.Text = "Customers";

            }

            Cursor.Current = Cursors.Default;
        }
    }
    public class ItemSalesReportRow
    {
        public string? Product { get; set; }
        public string? Customer { get; set; }
        public DateTime SaleDate { get; set; }
        public string? BillNo { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public long SaleId { get; set; }
    }

}
