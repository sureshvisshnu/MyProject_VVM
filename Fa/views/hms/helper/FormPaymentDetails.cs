using fa;
using fa.api.accounting.doubleentry;
using fa.api.Accounting;
using fa.api.OrderManagement;
using FADataAccessLibrary.Api.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.hms.helper
{
    public partial class FormPaymentDetails : Form
    {
        public long CustomerId { get; set; }
        public FormPaymentDetails()
        {
            InitializeComponent();
        }

        private void FormPaymentDetails_Load(object sender, EventArgs e)
        {
            LoadPaymentDetails();
        }
        private void LoadData()
        {
            var data = PaymentsNewManager.Instance.GetCustomerPaymentDetails(CustomerId);

            GridViewPayments.AutoGenerateColumns = true;
            GridViewPayments.DataSource = data;
        }
        private void LoadPaymentDetails()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                GridViewPayments.Rows.Clear();

                var ledger = PaymentsNewManager.Instance
                                             .GetCustomerLedger(CustomerId)
                                             .OrderBy(x => x.Date)
                                             .ToList();

                decimal runningBalance = 0;
                int rowNum = 1;

                foreach (var item in ledger)
                {
                    if (item.Type == "Invoice")
                    {
                        runningBalance += item.Amount;
                    }
                    else if (item.Type == "Payment")
                    {
                        runningBalance -= item.Amount;
                    }

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(GridViewPayments);

                    row.Cells[0].Value = rowNum++;
                    row.Cells[1].Value = item.Date.ToString(Global.Company.DateFormat);
                    row.Cells[2].Value = item.Reference;
                    row.Cells[3].Value = item.Type;
                    row.Cells[4].Value = item.Amount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    row.Cells[5].Value = runningBalance.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                    GridViewPayments.Rows.Add(row);
                }

                // Total row
                DataGridViewRow totalRow = new DataGridViewRow();
                totalRow.CreateCells(GridViewPayments);

                totalRow.Cells[3].Value = "Total";
                totalRow.Cells[5].Value = runningBalance.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                GridViewPayments.Rows.Add(totalRow);

                GridViewPayments.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadPaymentDetailsLast()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                GridViewPayments.Rows.Clear();

                var invoices = SalesManager.Instance.GetCustomerInvoices(CustomerId);

                int rowNum = 1;

                foreach (var invoice in invoices)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(GridViewPayments);

                    row.Cells[0].Value = rowNum++;
                    row.Cells[1].Value = invoice.SaleDate.ToString(Global.Company.DateFormat);
                    row.Cells[2].Value = invoice.RefNumber;
                    row.Cells[3].Value = invoice.NetAmount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    row.Cells[4].Value = invoice.Balance.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                    GridViewPayments.Rows.Add(row);
                }

                GridViewPayments.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadPaymentDetailsOld()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                GridViewPayments.Rows.Clear();

                var payments = PaymentsNewManager.Instance. GetPaymentsByCustomer(CustomerId);

                int rowNum = 1;

                foreach (var payment in payments)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(GridViewPayments);

                    row.Cells[0].Value = rowNum++;
                    row.Cells[1].Value = payment.TransactionDate.ToString(Global.Company.DateFormat);
                    row.Cells[2].Value = payment.Reference;
                    row.Cells[3].Value = payment.Amount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);

                    row.Cells[4].Value =
                        payment.TransactionType == 0
                        ? "Receipt"
                        : "Payment";

                    row.Cells[5].Value = payment.Description;

                    row.Cells[6].Value = payment.SalesId;

                    GridViewPayments.Rows.Add(row);
                }

                GridViewPayments.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment details: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }
    
}
