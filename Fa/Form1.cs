using fa.api.Accounting;
using fa.model.Accounting.Masters;

namespace Fa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Account acc = AccountManager.Instance.GetAccountById(1);
        }
    }
}