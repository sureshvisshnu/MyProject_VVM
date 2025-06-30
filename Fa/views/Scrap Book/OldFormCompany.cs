using fa.libraries.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fa.Api.Accounting;
using Fa.Model.Accounting.Masters;
using Fa.Model.Common;
using Fa.Context;

namespace fa.views.account.masters
{
    public partial class OldFormCompany : Form
    {
        AddressManager Am = null;
        CompanyManager Cm = null;

        public OldFormCompany()
        {
            Am = new AddressManager();
            Cm = new CompanyManager();
            InitializeComponent();
            loadCompanies();
            enableForm(false);
        }

        private void loadCompanies()
        {
            listBoxCompanies.Items.Clear();
            IList<Company> Companies = Cm.GetAllCompanies();
            if(Companies!=null)
            {
                foreach (var lCompany in Companies)
                {
                    listBoxCompanies.Items.Add(lCompany);
                }

                if (listBoxCompanies.Items.Count > 0)
                {
                    listBoxCompanies.SelectedIndex = 0;
                }
            }
        }

        private void listBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCompanyInfo();
        }

        private void loadCompanyInfo()
        {
            int index = listBoxCompanies.SelectedIndex;
            if (index > -1)
            {
                Company lCompany = (Company) listBoxCompanies.Items[index];
                if(lCompany!=null)
                {
                    long CompanyId = lCompany.CompanyId;
                    Company CompanyFromDB = Cm.GetCompanyInfoById(CompanyId);
                    if (CompanyFromDB!=null)
                    {
                        TextBoxCompanyId.Text = CompanyFromDB.CompanyId.ToString();                            
                        TextBoxCompanyName.Text = CompanyFromDB.Name;
                        TextBoxCompanyDisplayAs.Text = CompanyFromDB.DisplayAs;
                        CheckBoxIsBranchOffice.Checked = CompanyFromDB.IsBranch;
                        //MaskedTextBoxCompanyPhone.Text = CompanyFromDB.Phone;
                        //MaskedTextBoxCompanyFax.Text = CompanyFromDB.Fax;
                        //TextBoxCompanyEmail.Text = CompanyFromDB.Email;
                        //TextBoxCompanyTIN.Text = CompanyFromDB.Tin;
                        //TextBoxCompanyPAN.Text =CompanyFromDB.Pan;
                        //TextBoxCompanyCSTNo.Text = CompanyFromDB.CSTNumber;
                        //TextBoxCompanyGSTNo.Text = CompanyFromDB.GSTNumber;
                        //TextBoxCompanyWebSite.Text = CompanyFromDB.Website;                        
                        if (CompanyFromDB.Address !=null)
                        {
                            Address lAddress = CompanyFromDB.Address;                                
                            //TextBoxCompanyAddress1.Text = lAddress.Address1;
                            //TextBoxCompanyAddress2.Text = lAddress.Address2;
                            TextBoxCompanyCityTown.Text = lAddress.CityOrTown;
                            TextBoxCompanyDistrict.Text = lAddress.District;
                            if(lAddress.State!=null)
                            {
                                ComboBoxCompanyState.SelectedIndex = ComboBoxCompanyState.FindStringExact(lAddress.State.ToString());
                            }
                            //TextBoxCompanyPin.Text = lAddress.pinCode; 
                        }                            
                    }

                }
            }
        }

        private void TextBoxCompanyPhone_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void GroupBoxAddress_Enter(object sender, EventArgs e)
        {

        }

        private void BtnExitCompany_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();            
        }

        private void BtnNewCompany_Click(object sender, EventArgs e)
        {
            TextBoxCompanyId.Text = "";
            resetForm();
            enableForm(true);
        }

        private void resetForm()
        {
            TextBoxCompanyName.ResetText();
            TextBoxCompanyDisplayAs.ResetText();
            CheckBoxIsBranchOffice.Checked = false;
            TextBoxCompanyAddress1.ResetText();
            TextBoxCompanyAddress2.ResetText();
            TextBoxCompanyCityTown.ResetText();
            TextBoxCompanyDistrict.ResetText();
            ComboBoxCompanyState.SelectedIndex = -1;
            TextBoxCompanyPin.ResetText();
            TextBoxCompanyWebSite.ResetText();
            MaskedTextBoxCompanyPhone.ResetText();
            MaskedTextBoxCompanyFax.ResetText();
            TextBoxCompanyEmail.ResetText();
            TextBoxCompanyTIN.ResetText();
            TextBoxCompanyPAN.ResetText();
            TextBoxCompanyCSTNo.ResetText();
            TextBoxCompanyGSTNo.ResetText();
            ComboBoxCompanyGSTState.ResetText();
        }

        private void enableForm(Boolean enable)
        {
            TextBoxCompanyName.Enabled=enable;
            TextBoxCompanyDisplayAs.Enabled = enable;
            CheckBoxIsBranchOffice.Enabled = enable;
            TextBoxCompanyAddress1.Enabled = enable;
            TextBoxCompanyAddress2.Enabled = enable;
            TextBoxCompanyCityTown.Enabled = enable;
            TextBoxCompanyDistrict.Enabled = enable;
            ComboBoxCompanyState.Enabled = enable;
            TextBoxCompanyPin.Enabled = enable;
            TextBoxCompanyWebSite.Enabled = enable;
            MaskedTextBoxCompanyPhone.Enabled = enable;
            MaskedTextBoxCompanyFax.Enabled = enable;
            TextBoxCompanyEmail.Enabled = enable;
            TextBoxCompanyTIN.Enabled = enable;
            TextBoxCompanyPAN.Enabled = enable;
            TextBoxCompanyCSTNo.Enabled = enable;
            TextBoxCompanyGSTNo.Enabled = enable;
            ComboBoxCompanyGSTState.Enabled = enable;
            if (CheckBoxIsBranchOffice.Checked && enable)
                ComboBoxParentCompany.Enabled = true;
            else
                ComboBoxParentCompany.Enabled = false;
        }

        private void BtnSaveCompany_Click(object sender, EventArgs e)
        {
            if (TextBoxCompanyId.Text.Equals(""))
            {
                Company lCompany = getCompanyFromForm();
                Address lAddress = getCompanyAddressFromForm();                
                Address lAddress1 = Am.AddAddress(lAddress);
                lCompany.AddressId = lAddress1.AddressId;
                Company lCompany1 = Cm.AddCompany(lCompany);
                loadCompanies();
                TextBoxCompanyId.ResetText();
                enableForm(false);
            }
            else if (TextBoxCompanyId.Text.Equals("edit"))
            {
            }
        }


        private Address getCompanyAddressFromForm()
        {
            Address lAddress = new Address();
            //lAddress.Address1 = TextBoxCompanyAddress1.Text;
            //lAddress.Address2 = TextBoxCompanyAddress2.Text;
            lAddress.CityOrTown = TextBoxCompanyCityTown.Text;
            lAddress.District = TextBoxCompanyDistrict.Text;
            //lAddress.pinCode = TextBoxCompanyPin.Text;
            lAddress.State = new State();

            lAddress.Country = new Country() ;
            return lAddress;

        }
        private Company getCompanyFromForm()
        {
            Company lCompany = new Company();
            lCompany.Name = TextBoxCompanyName.Text;
            lCompany.DisplayAs = TextBoxCompanyDisplayAs.Text;            
            lCompany.IsBranch = CheckBoxIsBranchOffice.Checked;
            //lCompany.Phone = MaskedTextBoxCompanyPhone.Text;
            //lCompany.Fax = MaskedTextBoxCompanyFax.Text;
            //lCompany.Email = TextBoxCompanyEmail.Text;
            //lCompany.Tin = TextBoxCompanyTIN.Text;
            //lCompany.Pan = TextBoxCompanyPAN.Text;
            //lCompany.CSTNumber = TextBoxCompanyCSTNo.Text;
            //lCompany.GSTNumber = TextBoxCompanyGSTNo.Text;
            //lCompany.Website = TextBoxCompanyWebSite.Text;
            return lCompany;
        }

        public class Customer
        {
            public String CustomerId { set; get; }
            public String Name { set; get; }
            public String DisplayAs { set; get; }
            public virtual Address Address { get; set; }
            public double OpeningBalance { set; get; }
            public DateTime OpeningBalanceAsOf { set; get; }
            public String Phone { set; get; }
            public String Mobile { set; get; }
            public String Fax { set; get; }
            public override string ToString()
            {
                return string.Format("{0}", Name);
            }

        }

        private void CheckBoxIsBranchOffice_CheckedChanged(object sender, EventArgs e)
        {
            ComboBoxParentCompany.Enabled = CheckBoxIsBranchOffice.Checked;
        }

        private void FormCompany_Load(object sender, EventArgs e)
        {

        }

        private void groupBoxCompanyDetails_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCurrency CurrencyForm = new FormCurrency();
            CurrencyForm.ShowDialog(this);
        }

        private void TextBoxOp_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FormCurrency frmCurrency = new FormCurrency();
            frmCurrency.ShowDialog(this);
        }
    }
}
