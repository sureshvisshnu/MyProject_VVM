using System;
using System.Windows.Forms;
using fa.api.Accounting;
using fa.api.UserProfile;
using fa.libraries.utils;
using fa.model.Accounting.Masters;

namespace fa.views.users
{
    public partial class FormChangeCostCenter : Form
    {
        public static string ChooseCostCenterErrorMsg = "Costcenter field could not be empty, Please select Costcenter";
        public FormChangeCostCenter()
        {
            InitializeComponent();
        }

        private void FormChangeCostCenter_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                UserManager UserManager =  UserManager.Instance;
                ComboUtils.InitializeCostCenterCombo(ComboBoxSelectCompanyCostcenter, Global.Company.CompanyId);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void BtnSelectCompanyContinue_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                CostCenterManager CostCenterManager = CostCenterManager.Instance;
                CostCenter SelectedCostCenter = CostCenterManager.GetCostCenterById(((CostCenter)ComboBoxSelectCompanyCostcenter.Items[ComboBoxSelectCompanyCostcenter.SelectedIndex]).CostCenterId);
                if (SelectedCostCenter != null)
                {
                    Global.CostCenter = SelectedCostCenter;
                }
                else
                {
                    MessageBox.Show("Something went wrong, please check the selected cost center is till valid");
                }
                this.Close();
            }
        }

        private void BtnSelectCompanyCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Boolean ValidateForm()
        {
            SelectCostCenterErrorMsg.Text = "";        
            if (ComboBoxSelectCompanyCostcenter.SelectedIndex < 0)
            {
                SelectCostCenterErrorMsg.Text= ChooseCostCenterErrorMsg;
                ComboBoxSelectCompanyCostcenter.Select();
                return false;
            }
            if (ComboBoxSelectCompanyCostcenter.SelectedIndex >= 0 && CostCenterManager.Instance.GetCostCenterById(((CostCenter)ComboBoxSelectCompanyCostcenter.Items[ComboBoxSelectCompanyCostcenter.SelectedIndex]).CostCenterId)==null)
            {
                SelectCostCenterErrorMsg.Text = "Something went wrong, please check the selected cost center is till valid";
                ComboBoxSelectCompanyCostcenter.Select();
                return false;
            }
            return true;
        }

        private void ComboBoxSelectCompanyCostcenter_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSelectCompanyCostcenter.DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelectCompanyContinue.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
