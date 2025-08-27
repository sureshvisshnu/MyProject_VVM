using fa;
using fa.api.catalog;
using fa.context;
using fa.libraries.utils;
using fa.model.Catalog;
using fa.views.controls;
using FADataAccessLibrary.Api.OrderManagement;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Font = System.Drawing.Font;
using Global = fa.Global;

namespace Fa.reports.sales
{
    public enum ItemSaleReportTableColumn
    {
        SNO, PRODUCT, CUSTOMER, QTY, SALESDATE, RATE, SALESID
    }
    public partial class FormItemSalesReport : Form
    {
        private long _itemId;
        private string? _itemName;
        private DateTime _fromDate;
        private DateTime _toDate;
        private List<long> _selectedItemIds = new List<long>();
        private List<string> selectedItemNumbers = new List<string>();
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
            GenerateItemSalesReport();
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

        }

        private void GenerateItemSalesReport()
        {
            ItemSalesReportErrorMsg.Text = string.Empty;
            EnableButtons(false);

            if (_selectedItemIds.Count == 0)
            {
                ItemSalesReportErrorMsg.Text = "Please select at least one item.";
                return;
            }

            try
            {
                using (var context = new AccountMasterContext())
                {
                    var itemSalesManager = new ItemSalesManager(context);
                    List<ItemSalesData> salesData = new List<ItemSalesData>();

                    // Get sales for all selected items
                    foreach (var itemId in _selectedItemIds)
                    {
                        if (itemId == 0) continue; // Skip "All" if it's in the list

                        var itemSales = itemSalesManager.GetItemSalesReport(
                            itemId: itemId,
                            fromDate: _fromDate,
                            toDate: _toDate
                        );
                        salesData.AddRange(itemSales);
                    }

                    if (salesData != null && salesData.Count > 0)
                    {
                        EnableButtons(true);

                        // Clear existing rows and add enough for data + summary
                        DataGridViewItemSales.Rows.Clear();
                        DataGridViewItemSales.Rows.Add(salesData.Count + 1);

                        int rowCount = 0;
                        double totalQuantity = 0;
                        double totalAmount = 0;

                        foreach (ItemSalesData item in salesData)
                        {
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.SNO].Value = item.SaleDate.ToString(Global.Company.DateFormat);
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.PRODUCT].Value = item.RefNumber;
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = item.CustomerName;
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.SALESDATE].Value = item.Quantity.ToString("N2");
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.QTY].Value = item.Price.ToString("C");
                            DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.RATE].Value = item.Amount.ToString("C");

                            totalQuantity += item.Quantity;
                            totalAmount += item.Amount;
                            rowCount++;
                        }

                        // Add summary row
                        DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.CUSTOMER].Value = "TOTAL";
                        DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.QTY].Value = totalQuantity.ToString("N2");
                        DataGridViewItemSales.Rows[rowCount].Cells[(int)ItemSaleReportTableColumn.RATE].Value = totalAmount.ToString("C");

                        // Make summary row bold
                        DataGridViewItemSales.Rows[rowCount].DefaultCellStyle.Font = new Font(DataGridViewItemSales.Font, FontStyle.Bold);

                        // Update summary labels
                        LabelTotalQuantity.Text = totalQuantity.ToString("N2");
                        LabelTotalAmount.Text = totalAmount.ToString("C");
                        LabelTotalSales.Text = salesData.Count.ToString();
                    }
                    else
                    {
                        ItemSalesReportErrorMsg.Text = "No sales data found for the selected criteria.";
                    }
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

        private void EnableButtons(bool enable)
        {
            //ButtonExport.Enabled = enable;
            //ButtonRefresh.Enabled = enable;
            // Add other buttons as needed
        }
        private void BtnGo_Click(object sender, EventArgs e)
        {
            GenerateItemSalesReport();
        }

        private void ComboBoxSelectItem_NodeClickedEvent(object sender, fa.views.controls.ComboTreeView.ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
            string rackNumber = e.Node.Text;

            if (e.Node.Checked)
            {
                if (!selectedItemNumbers.Contains(rackNumber))
                {
                    selectedItemNumbers.Add(rackNumber);
                }
            }
            else
            {
                selectedItemNumbers.Remove(rackNumber);
            }
        }
        private void DisplayCheckedInformation()
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

        private void BtnExit_Click(object sender, EventArgs e)
        {

        }
    }
}
