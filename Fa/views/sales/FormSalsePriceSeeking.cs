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

                // Populate the grid
                int rowNum = 1;

                foreach (var sale in sales)
                {
                    GridViewItems.Rows.Add(
                        rowNum++,
                        sale.Sale?.SaleDate.ToString(Global.Company.DateFormat), // formatted date
                        sale.Price.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), // correct
                        sale.Id
                    );
                }
                GridViewItems.ClearSelection();
                // Update product details
                using (var context = new AccountMasterContext())
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == ProductId);
                    if (product != null)
                    {
                        //TextBoxCategory.Text = product.c;
                        //TextBoxProductFamily.Text = product.ProductFamily;
                        //TextBoxManufacturer.Text = product.Manufacturer;
                        //TextBoxSupplier.Text = product.Supplier;
                    }
                }
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
