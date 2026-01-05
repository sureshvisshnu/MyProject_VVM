using fa;
using fa.api.OrderManagement;
using fa.context;
using fa.views;
using fa.views.sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.sales
{
    public partial class FormSalsePriceSeeking : FormBase
    {
        public long LocationId = 0L;
        public long ProductId { get; set; } // Set from FormItembasedSales
        public long ProductBatchId { get; set; } // Set from FormItembasedSales
        public long CustomerId { get; set; } // Set from FormItembasedSales
        public string? ProductName { get; set; }
        public FormSalsePriceSeeking(object sender)
        {
            InitializeComponent();
            //this.Shown += FormSalsePriceSeeking_Shown; // Load data when form is shown
        }
        
        private void LoadPreviousPrices()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewItems.Rows.Clear();

                // Get customer ID from the calling form (FormItembasedSales)
                var callingForm = this.Owner as FormItembasedSales;
                long customerId = callingForm?.CustomerId ?? 0;

                // Get last 5 sales prices
                var sales = SalesManager.Instance.GetLastPricesByProductAndCustomer(Global.Company.CompanyId,
                    productId: this.ProductId,
                    customerId: this.CustomerId
                );

                // Clear old rows
                GridViewItems.Rows.Clear();
                TextBoxSearchProduct.Text = ProductName ?? string.Empty;
                int rowNum = 1;
                foreach (var sale in sales.Take(5)) // limit to last 5
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(GridViewItems);

                    row.Cells[0].Value = rowNum++;
                    row.Cells[1].Value = sale.Sale?.SaleDate.ToString(Global.Company.DateFormat);
                    row.Cells[2].Value = sale.Price.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    row.Cells[3].Value = sale.Id;

                    GridViewItems.Rows.Add(row);
                }

                GridViewItems.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading prices: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void FormSalsePriceSeeking_Load(object sender, EventArgs e)
        {
            LoadPreviousPrices();
        }

        private void BtnOKExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
