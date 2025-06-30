using fa.libraries.utils;
using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Common;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.controls;

namespace fa.views.hms.patient
{
    public partial class FormInsurance : FormPatientBase
    {
        public static string EnterNameErrorMsg = "Please enter Insurance name.";
        public static string EnterInsurancePolicyNumberErrorMsg = "Please enter insurance policy number.";
        public static string EnterGroupNameErrorMsg = "Please enter group.";
        public static string EnterInsurerNameErrorMsg = "Please enter insurer name.";
        public static string SelectRelationshipErrorMsg = "Please select relationship.";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string SaveChangesErrorMsg = "Do you want to save the changes?";

        public long? InsuranceId = null;
        InsuranceInfoManager InsuranceInfoManager = null!;
        AddressManager AddressManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        DateValidation DateValidation = null!;
        PersonManager PersonManager = null!;
        FormPatientBase parent = null!;

        public FormInsurance(object sender)
        {
            parent = (FormPatientBase)sender;
            InsuranceInfoManager = InsuranceInfoManager.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            DateValidation = DateValidation.Instance;
            PersonManager = PersonManager.Instance;
            InitializeComponent();
            GroupBoxInsuranceAddress.StateComboBoxSelectedIndexChanged += ComboBoxSwapTextBoxState_SelectedIndexChanged!;
        }

        private void FormInsurance_Load(object sender, EventArgs e)
        {
            ResetForm();
            if (TextBoxPatientId.Text != "")
            {
                ComboUtils.InitializeInsurerCombo(ComboBoxSwapTextBoxInsuranceInsurer, long.Parse(TextBoxPatientId.Text));
            }
            if (InsuranceId != null)
            {
                LoadInsuranceInfo();
            }
            TextBoxInsuranceName.Select();
            ResetDirtyFlag();
        }
        private void LoadInsuranceInfo()
        {
            InsuranceInfo InsuranceInfoFromDB = InsuranceInfoManager.GetInsuranceInfoById((long)InsuranceId!, false);
            if (InsuranceInfoFromDB != null)
            {
                TextBoxPatientId.Text = InsuranceInfoFromDB.PatientId.ToString();
                TextBoxInsuranceName.Text = InsuranceInfoFromDB.InsuranceName;
                TextBoxInsurancePolicy.Text = InsuranceInfoFromDB.PolicyNumber;
                TextBoxInsuranceGroup.Text = InsuranceInfoFromDB.GroupNumber;
                RbtInsurancePrimaryInsurance.Checked = InsuranceInfoFromDB.IsPrimary;
                RbtInsuranceActiveInsurance.Checked = InsuranceInfoFromDB.IsActive;
                ComboBoxSwapTextBoxInsuranceInsurer.SelectedIndex = ComboBoxSwapTextBoxInsuranceInsurer.FindStringExact(PersonManager.GetPersonById(InsuranceInfoFromDB.InsuranceHolderId).Name);
                ComboBoxSwapTextBoxInsurerRelationShip.SelectedIndex = ComboBoxSwapTextBoxInsurerRelationShip.FindStringExact(InsuranceInfoFromDB.InsuranceHolderRelationShip.ToString());
                TextBoxInsuranceEmployerName.Text = InsuranceInfoFromDB.EmployerName;
                TextBoxInsuranceId.Text = InsuranceInfoFromDB.Id.ToString();
                TextBoxInsuranceAdditionalInfo.Text = InsuranceInfoFromDB.AdditionalInformation;
                GroupBoxInsuranceAddress.CountryId = (long)Global.Company.CountryId!;
                ComboBoxSwapTextBoxInsuranceInsurer.Visible = false;
                if (!InsuranceInfoFromDB.IsActive)
                {
                    ComboBoxSwapTextBoxInsuranceInsurer.Visible = true;
                    //ComboBoxSwapTextBoxInsuranceInsurer.SelectedIndex = -1;
                    ComboBoxSwapTextBoxInsuranceInsurer.Enabled = true;
                    RbtInsuranceActiveInsurance.Checked = InsuranceInfoFromDB.IsActive;
                }
                else
                {
                    ComboBoxSwapTextBoxInsuranceInsurer.Visible = true;
                    ComboBoxSwapTextBoxInsuranceInsurer.Enabled = true;
                    RbtInsuranceActiveInsurance.Checked = InsuranceInfoFromDB.IsActive;
                }
                CheckBoxIPCoverage.Checked = InsuranceInfoFromDB.IPInsuranceCoverage;
                CheckBoxOPCoverage.Checked = InsuranceInfoFromDB.OPInsuranceCoverage;
                if (InsuranceInfoFromDB.EmployerAddress != null)
                {
                    Address Address = InsuranceInfoFromDB.EmployerAddress;
                    GroupBoxInsuranceAddress.AddressLine1 = Address.AddressLine1;
                    GroupBoxInsuranceAddress.AddressLine2 = Address.AddressLine2;
                    GroupBoxInsuranceAddress.CityName = Address.CityOrTown;
                    GroupBoxInsuranceAddress.DistrictName = Address.District;
                    GroupBoxInsuranceAddress.PinCode = Address.PinCode;
                    GroupBoxInsuranceAddress.StateId = Address.StatesId != null ? (long)Address.StatesId : 0L;
                }
                if (InsuranceInfoFromDB.EmployerContactInfo != null)
                {
                    ContactInfo lContactInfo = InsuranceInfoFromDB.EmployerContactInfo;
                    TextBoxInsurancePhone.Text = lContactInfo.Phone;
                }
            }
        }
        private InsuranceInfo GetInsuranceInfoFromForm()
        {
            InsuranceInfo lInsuranceInfo = new InsuranceInfo();
            if (!string.IsNullOrEmpty(TextBoxInsuranceId.Text))
            {
                lInsuranceInfo.Id = Convert.ToInt64(TextBoxInsuranceId.Text);
            }
            else
            {
                lInsuranceInfo.Id = 0L;
            }
            lInsuranceInfo.PatientId = long.Parse(TextBoxPatientId.Text);
            lInsuranceInfo.InsuranceName = TextBoxInsuranceName.Text;
            lInsuranceInfo.PolicyNumber = TextBoxInsurancePolicy.Text;
            lInsuranceInfo.GroupNumber = TextBoxInsuranceGroup.Text;
            lInsuranceInfo.IsPrimary = RbtInsurancePrimaryInsurance.Checked;
            lInsuranceInfo.IsActive = RbtInsuranceActiveInsurance.Checked;
            lInsuranceInfo.InsuranceHolderId = ((Person)ComboBoxSwapTextBoxInsuranceInsurer.Items[ComboBoxSwapTextBoxInsuranceInsurer.SelectedIndex]).Id;
            lInsuranceInfo.InsuranceHolderRelationShip = (ComboBoxSwapTextBoxInsurerRelationShip.SelectedIndex > -1) ? (RelationShip)ComboBoxSwapTextBoxInsurerRelationShip.SelectedIndex : RelationShip.SELF;
            lInsuranceInfo.EmployerName = TextBoxInsuranceEmployerName.Text;
            lInsuranceInfo.AdditionalInformation = TextBoxInsuranceAdditionalInfo.Text;
            lInsuranceInfo.CompanyId = Global.Company.CompanyId;
            lInsuranceInfo.EmployerAddress = GetInsuranceInfoAddressFromForm();
            lInsuranceInfo.EmployerContactInfo = GetInsuranceInfoContactInfoFromForm();
            lInsuranceInfo.IsActive = RbtInsuranceActiveInsurance.Checked;
            lInsuranceInfo.IPInsuranceCoverage = CheckBoxIPCoverage.Checked;
            lInsuranceInfo.OPInsuranceCoverage = CheckBoxOPCoverage.Checked;

            return lInsuranceInfo;
        }
        private Address GetInsuranceInfoAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = GroupBoxInsuranceAddress.AddressLine1.Trim();
            lAddress.AddressLine2 = GroupBoxInsuranceAddress.AddressLine2.Trim();
            lAddress.CityOrTown = GroupBoxInsuranceAddress.CityName.Trim();
            lAddress.District = GroupBoxInsuranceAddress.DistrictName.Trim();
            lAddress.PinCode = GroupBoxInsuranceAddress.PinCode.Trim();
            lAddress.StatesId = GroupBoxInsuranceAddress.StateId;
            return lAddress;
        }

        private ContactInfo GetInsuranceInfoContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxInsurancePhone.Text.Replace("-", "");
            return lContactInfo;
        }
        private void BtnInsuranceCancel_Click(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                if (MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnInsuranceSave.PerformClick();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            formIsDirty = false;
            this.Close();
        }

        private void BtnInsuranceSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                InsuranceInfo lInsuranceInfo = GetInsuranceInfoFromForm();
                InsuranceInfo InsuranceInfoFromDB = null!;
                if (lInsuranceInfo.Id == 0)
                {
                    InsuranceInfoFromDB = InsuranceInfoManager.AddInsuranceInfo(lInsuranceInfo);
                    TextBoxInsuranceId.Text = InsuranceInfoFromDB.Id.ToString();
                }
                else
                {
                    InsuranceInfo InsuranceInfoById = InsuranceInfoManager.GetInsuranceInfoById(lInsuranceInfo.Id, false);
                    if (InsuranceInfoById != null)
                    {
                        lInsuranceInfo.EmployerContactInfoId = lInsuranceInfo.EmployerContactInfo.Id = (long)InsuranceInfoById.EmployerContactInfoId!;
                        lInsuranceInfo.EmployerAddressId = lInsuranceInfo.EmployerAddress.AddressId = (long)InsuranceInfoById.EmployerAddressId!;
                        AddressManager.UpdateAddress(lInsuranceInfo.EmployerAddress);
                        ContactInfoManager.UpdateContactInfo(lInsuranceInfo.EmployerContactInfo);
                        InsuranceInfoFromDB = InsuranceInfoManager.UpdateInsuranceInfo(lInsuranceInfo);

                    }
                }
                formIsDirty = false;
                this.Close();
            }
        }

        private Boolean validate()
        {
            ToolStripStatusLabelErrorInsurance.Text = "";
            if (string.IsNullOrEmpty(TextBoxInsuranceName.Text.Trim()))
            {
                ToolStripStatusLabelErrorInsurance.Text = EnterNameErrorMsg;
                TextBoxInsuranceName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxInsurancePolicy.Text.Trim()))
            {
                ToolStripStatusLabelErrorInsurance.Text = EnterInsurancePolicyNumberErrorMsg;
                TextBoxInsurancePolicy.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxInsuranceGroup.Text.Trim()))
            {
                ToolStripStatusLabelErrorInsurance.Text = EnterGroupNameErrorMsg;
                TextBoxInsuranceGroup.Select();
                return false;
            }

            if (ComboBoxSwapTextBoxInsuranceInsurer.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorInsurance.Text = EnterInsurerNameErrorMsg;
                ComboBoxSwapTextBoxInsuranceInsurer.Select();
                return false;
            }
            if (ComboBoxSwapTextBoxInsurerRelationShip.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorInsurance.Text = SelectRelationshipErrorMsg;
                ComboBoxSwapTextBoxInsurerRelationShip.Select();
                return false;
            }
            if (GroupBoxInsuranceAddress.StateId == 0L || GroupBoxInsuranceAddress.StateSelectedIndex < 0)
            {
                ToolStripStatusLabelErrorInsurance.Text = ChooseStateErrorMsg;
                GroupBoxInsuranceAddress.selected_field(Fields.state);
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorInsurance.Text = String.Empty;
            TextBoxInsuranceAdditionalInfo.ResetText();
            TextBoxInsuranceEmployerName.ResetText();
            TextBoxInsuranceGroup.ResetText();
            TextBoxInsuranceId.ResetText();
            TextBoxInsuranceName.ResetText();
            TextBoxInsurancePhone.ResetText();
            TextBoxInsurancePolicy.ResetText();
            GroupBoxInsuranceAddress.Clear();
            GroupBoxInsuranceAddress.ReadOnly = false;
            RbtInsurancePrimaryInsurance.Checked = true;
            RbtInsuranceActiveInsurance.Checked = true;
            ComboBoxSwapTextBoxInsurerRelationShip.SelectedIndex = -1;
            ComboBoxSwapTextBoxInsuranceInsurer.SelectedIndex = -1;
            ComboBoxSwapTextBoxInsuranceInsurer.Visible = true;
            GroupBoxInsuranceAddress.CountryId = (long)Global.Company.CountryId!;
            GroupBoxInsuranceAddress.StateId = (long)Global.Company.Address.StatesId!;
        }

        private void ComboBoxSwapTextBoxInsuranceInsurer_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxInsuranceInsurer.DroppedDown = false;

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnInsuranceSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnInsuranceCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        private void ComboBoxSwapTextBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GroupBoxInsuranceAddress.StateSelectedIndex > -1)
            {
                ToolStripStatusLabelErrorInsurance.Text = string.Empty;
            }
        }

        private void FormInsurance_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show(SaveChangesErrorMsg, "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnInsuranceSave.PerformClick();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
    }
}
