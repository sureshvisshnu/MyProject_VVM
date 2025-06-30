using fa.libraries.utils;
using fa.api.Accounting;
using fa.api.UserProfile;
using fa.model.Accounting.Masters;
using System;

using System.Windows.Forms;

namespace fa.views.users
{
    public partial class FormSelectCompany : Form
    {
        public static string ChooseCostCenterErrorMsg = "Costcenter field could not be empty, Please select Costcenter";
        public static string ChooseCompanyErrorMsg = "Company field could not be empty, Please select Company";

        public FormSelectCompany()
        {
            InitializeComponent();
        }
        private void FormSelectCompany_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                UserManager UserManager = UserManager.Instance;
                ComboUtils.InitializeBusinessTypeCompanyCombo(ComboBoxSelectCompanyCompany, UserManager.GetAccessibleCompanies(Global.User), Global.softwareType);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
           
        }
        private void BtnSelectCompanyContinue_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (ValidateForm())
                {
                    Global.Company = null;
                    Global.CostCenter = null;
                    Company SelectedCompany = (Company)ComboBoxSelectCompanyCompany.Items[ComboBoxSelectCompanyCompany.SelectedIndex];
                    if (SelectedCompany != null)
                    {
                        CompanyManager CompanyManager = CompanyManager.Instance;
                        Company lCompany= CompanyManager.GetCompany(SelectedCompany.CompanyId);
                        if (lCompany != null)
                        {
                            Global.Company = lCompany;
                        }
                        else
                        {
                            MessageBox.Show("Somthing went wrong, the selected company is not valid.");
                            return;
                        }
                    }

                    else if (ComboBoxSelectCompanyCostcenter.Items.Count > 0)
                    {
                        //changes
                        CostCenter SelectedCostCenter = null;
                        if (ComboBoxSelectCompanyCostcenter.Items.Count == 1)
                        {
                            SelectedCostCenter = (CostCenter)ComboBoxSelectCompanyCostcenter.Items[0];
                        }
                        else
                        {
                            SelectedCostCenter = (CostCenter)ComboBoxSelectCompanyCostcenter.Items[ComboBoxSelectCompanyCostcenter.SelectedIndex];
                        }
                        if (SelectedCostCenter != null)
                        {
                            CostCenterManager CostCenterManager = CostCenterManager.Instance;
                            Global.CostCenter = CostCenterManager.GetCostCenterById(SelectedCostCenter.CostCenterId);
                        }
                    }

                    this.Close();
                }
            }   
            finally
            {
                Cursor.Current = Cursors.Default;
            }   
            
        }
        private void BtnSelectCompanyCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBoxSelectCompanyCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxSelectCompanyCompany.SelectedIndex>-1)
            {
                Company SelectedCompany = (Company)ComboBoxSelectCompanyCompany.Items[ComboBoxSelectCompanyCompany.SelectedIndex];
                ComboUtils.InitializeCostCenterCombo(ComboBoxSelectCompanyCostcenter, SelectedCompany.CompanyId);
                if (ComboBoxSelectCompanyCostcenter.Items.Count>1)
                {
                    LabelSelectCompanyCostcente.Visible = true;
                    ComboBoxSelectCompanyCostcenter.Visible = true;
                }
                else
                {
                    LabelSelectCompanyCostcente.Visible = false;
                    ComboBoxSelectCompanyCostcenter.Visible = false;
                }
            }
        }

        private Boolean ValidateForm()
        {
            SelectCompanyErrorMsg.Text = "";        
            if (ComboBoxSelectCompanyCompany.SelectedIndex<0)
            {
                SelectCompanyErrorMsg.Text =ChooseCompanyErrorMsg;
                ComboBoxSelectCompanyCompany.Select();
                return false;
            }
            if (ComboBoxSelectCompanyCompany.SelectedIndex >= 0 && CompanyManager.Instance.GetCompany(((Company)ComboBoxSelectCompanyCompany.Items[ComboBoxSelectCompanyCompany.SelectedIndex]).CompanyId) ==null)
            {
                SelectCompanyErrorMsg.Text = "Something went wrong, please check the selected company is till valid"; ;
                ComboBoxSelectCompanyCompany.Select();
                return false;
            }
            if (ComboBoxSelectCompanyCostcenter.Visible==true && ComboBoxSelectCompanyCostcenter.SelectedIndex<0)
            {
                SelectCompanyErrorMsg.Text= ChooseCostCenterErrorMsg;
                ComboBoxSelectCompanyCostcenter.Select();
                return false;
            }
            if (ComboBoxSelectCompanyCostcenter.Visible == true && ComboBoxSelectCompanyCostcenter.SelectedIndex >= 0 && CostCenterManager.Instance.GetCostCenterById(((CostCenter)ComboBoxSelectCompanyCostcenter.Items[ComboBoxSelectCompanyCostcenter.SelectedIndex]).CostCenterId) == null)
            {
                SelectCompanyErrorMsg.Text = "Something went wrong, please check the selected cost center is till valid";
                ComboBoxSelectCompanyCostcenter.Select();
                return false;
            }
            return true;
        }

        private void ComboBoxSelectCompanyCompany_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSelectCompanyCompany.DroppedDown = false;
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
