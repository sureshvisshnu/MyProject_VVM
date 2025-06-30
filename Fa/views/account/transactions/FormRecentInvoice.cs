using fa.api.Accounting;
using fa.model.Accounting.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace fa.views.account.transactions
{
    public partial class FormRecentInvoice : Form
    {
        InvoiceManager InvoiceManager = null;

        public FormRecentInvoice()
        {            
            InitializeComponent();
            InvoiceManager = InvoiceManager.Instance;
        }
        private void FormRecentInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                List<Invoice> Invoice = InvoiceManager.GetInvoiceByCompanyId(Global.Company.CompanyId);
                var list = new BindingList<Invoice>(Invoice);
                DataGridViewRecentInvoice.DataSource = list;

                DataGridViewRecentInvoice.ReadOnly = true;

                DataGridViewRecentInvoice.Columns["InvoiceId"].Visible =
                DataGridViewRecentInvoice.Columns["TermId"].Visible =
                DataGridViewRecentInvoice.Columns["Term"].Visible =
                DataGridViewRecentInvoice.Columns["DueDate"].Visible =
                DataGridViewRecentInvoice.Columns["CustomerId"].Visible =
                DataGridViewRecentInvoice.Columns["Customer"].Visible =
                DataGridViewRecentInvoice.Columns["InternalMemo"].Visible =
                DataGridViewRecentInvoice.Columns["InvoiceDetails"].Visible =
                DataGridViewRecentInvoice.Columns["Discount"].Visible =
                DataGridViewRecentInvoice.Columns["DiscountType"].Visible = false;

                DataGridViewRecentInvoice.Columns["InvoiceDate"].HeaderText = "Date";
                DataGridViewRecentInvoice.Columns["ReferenceNumber"].HeaderText = "Reference #";
                DataGridViewRecentInvoice.Columns["Memo"].HeaderText = "Account / Description";
                DataGridViewRecentInvoice.Columns["Total"].HeaderText = "Amount";
                DataGridViewRecentInvoice.Columns["InvoiceDate"].Width = 150;
                DataGridViewRecentInvoice.Columns["ReferenceNumber"].Width = 200;
                DataGridViewRecentInvoice.Columns["Memo"].Width = 300;
                DataGridViewRecentInvoice.Columns["Total"].Width = 115;

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void DataGridViewRecentInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
