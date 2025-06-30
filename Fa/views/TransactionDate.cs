using fa.api.Accounting;
using System;
using System.Windows.Forms;

namespace fa.views.utils
{
    public partial class FormTransactionDate : Form
    {
        public FormTransactionDate()
        {
            InitializeComponent();
        }

        private void FormTransactionDate_Load(object sender, EventArgs e)
        {
            MonthCalendarTransactionDate.SetDate(Global.getTransactionDate());
        }

        private void BtnChangeTransactionDate_Click(object sender, EventArgs e)
        {
            Global.setTransactionDate(MonthCalendarTransactionDate.SelectionRange.Start);
            if (Global.Company != null)
            {
                CompanyManager.GenerateIdSpace(Global.Company);
            }
            this.Close();
        }
    }
}
