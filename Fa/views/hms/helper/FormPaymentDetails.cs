using fa.api.accounting.doubleentry;
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
            LoadData();
        }
        private void LoadData()
        {
            var data = PaymentsNewManager.Instance.GetCustomerPaymentDetails(CustomerId);

            GridViewPayments.AutoGenerateColumns = true;
            GridViewPayments.DataSource = data;
        }
    }
    
}
