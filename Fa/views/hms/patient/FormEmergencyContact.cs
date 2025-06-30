using fa.libraries.Validation;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.utils;
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
    public partial class FormEmergencyContact : FormPatientBase
    {
        public static string EnterFirstNameErrorMsg = "Please enter first name.";
        public static string EnterLastNameErrorMsg = "Please enter last name.";
        public static string SelectGenderErrorMsg = "Please select gender.";
        public static string EnterPhoneNumberErrorMsg = "Please enter phone number.";
        public static string SelectRelationshipErrorMsg = "Please select relationship.";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string SaveChangesErrorMsg = "Do you want to save the changes?";

        public long? EmergencyContId = null;
        EmergencyContactManager EmergencyContactManager = null;
        AddressManager AddressManager = null;
        ContactInfoManager ContactInfoManager = null;
        DateValidation DateValidation = null;
        FormPatientBase parent = null;

        public FormEmergencyContact(object sender)
        {
            parent = (FormPatientBase)sender;
            EmergencyContactManager = EmergencyContactManager.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
        }

        private void FormEmergencyContact_Load(object sender, EventArgs e)
        {
            ResetForm();
            GroupBoxEmergencyContAddress.CountryId = (long)Global.Company.CountryId!;
            GroupBoxEmergencyContAddress.StateId = (long)Global.Company.Address.StatesId!;
            if (EmergencyContId != null)
            {
                LoadEmergencyContactInfo();
            }
            TextBoxEmergencyContFirstName.Select();
            ResetDirtyFlag();
        }
        private void LoadEmergencyContactInfo()
        {
            EmergencyContact EmergencyContactFromDB = EmergencyContactManager.GetEmergencyContactById((long)EmergencyContId);
            if (EmergencyContactFromDB != null)
            {
                TextBoxPatientId.Text = EmergencyContactFromDB.PatientId.ToString();
                TextBoxEmergencyContFirstName.Text = EmergencyContactFromDB.FirstName;
                TextBoxEmergencyContId.Text = EmergencyContactFromDB.Id.ToString();
                TextBoxEmergencyContInitial.Text = EmergencyContactFromDB.MiddleInitial;
                TextBoxEmergencyContLastName.Text = EmergencyContactFromDB.LastName;
                RbtEmergencyContGender.Gender = (GenderSelection)EmergencyContactFromDB.Gender + 1;
                GroupBoxEmergencyContAddress.CountryId = (long)Global.Company.CountryId;
                ComboBoxSwapTextBoxEmergencyContRelationShip.SelectedIndex = ComboBoxSwapTextBoxEmergencyContRelationShip.FindStringExact(EmergencyContactFromDB.RelationShip.ToString());
                if (EmergencyContactFromDB.Address != null)
                {
                    Address Address = EmergencyContactFromDB.Address;
                    GroupBoxEmergencyContAddress.AddressLine1 = Address.AddressLine1;
                    GroupBoxEmergencyContAddress.AddressLine2 = Address.AddressLine2;
                    GroupBoxEmergencyContAddress.CityName = Address.CityOrTown;
                    GroupBoxEmergencyContAddress.DistrictName = Address.District;
                    GroupBoxEmergencyContAddress.PinCode = Address.PinCode;
                    GroupBoxEmergencyContAddress.StateId = Address.StatesId != null ? (long)Address.StatesId : 0L;
                }
                if (EmergencyContactFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = EmergencyContactFromDB.ContactInfo;
                    TextBoxEmergencyContPhone.Text = lContactInfo.Phone;
                    TextBoxEmergencyContMobile.Text = lContactInfo.Mobile;
                }
            }
        }
        private EmergencyContact GetEmergencyContactFromForm()
        {
            EmergencyContact lEmergencyContact = new EmergencyContact();
            if (!string.IsNullOrEmpty(TextBoxEmergencyContId.Text))
            {
                lEmergencyContact.Id = Convert.ToInt64(TextBoxEmergencyContId.Text);
            }
            else
            {
                lEmergencyContact.Id = 0L;
            }
            lEmergencyContact.PatientId = long.Parse(TextBoxPatientId.Text);
            lEmergencyContact.FirstName = TextBoxEmergencyContFirstName.Text;
            lEmergencyContact.MiddleInitial = TextBoxEmergencyContInitial.Text;
            lEmergencyContact.LastName = TextBoxEmergencyContLastName.Text;
            lEmergencyContact.Gender = (Gender)RbtEmergencyContGender.Gender - 1;
            lEmergencyContact.CompanyId = Global.Company.CompanyId;
            lEmergencyContact.Address = GetEmergencyContAddressFromForm();
            lEmergencyContact.ContactInfo = GetEmergencyContContactInfoFromForm();
            lEmergencyContact.RelationShip = (ComboBoxSwapTextBoxEmergencyContRelationShip.SelectedIndex > -1) ? (RelationShip)ComboBoxSwapTextBoxEmergencyContRelationShip.SelectedIndex : RelationShip.SELF;

            return lEmergencyContact;
        }
        private Address GetEmergencyContAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = GroupBoxEmergencyContAddress.AddressLine1.Trim();
            lAddress.AddressLine2 = GroupBoxEmergencyContAddress.AddressLine2.Trim();
            lAddress.CityOrTown = GroupBoxEmergencyContAddress.CityName.Trim();
            lAddress.District = GroupBoxEmergencyContAddress.DistrictName.Trim();
            lAddress.PinCode = GroupBoxEmergencyContAddress.PinCode.Trim();
            lAddress.StatesId = GroupBoxEmergencyContAddress.StateId;
            return lAddress;
        }

        private ContactInfo GetEmergencyContContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxEmergencyContPhone.Text.Replace("-", "");
            lContactInfo.Mobile = TextBoxEmergencyContMobile.Text.Replace("-", "");
            return lContactInfo;
        }
        private void BtnEmergencyContCancel_Click(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                if (MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnEmergencyContSave.PerformClick();
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

        private void BtnEmergencyContSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {

                EmergencyContact lEmergencyContact = GetEmergencyContactFromForm();
                EmergencyContact EmergencyContactFromDB = null;
                if (lEmergencyContact.Id == 0)
                {
                    EmergencyContactFromDB = EmergencyContactManager.AddEmergencyContact(lEmergencyContact);
                }
                else
                {
                    EmergencyContact EmergencyContactById = EmergencyContactManager.GetEmergencyContactById(lEmergencyContact.Id);
                    if (EmergencyContactById != null)
                    {
                        lEmergencyContact.ContactInfoId = lEmergencyContact.ContactInfo.Id = (long)EmergencyContactById.ContactInfoId;
                        lEmergencyContact.AddressId = lEmergencyContact.Address.AddressId = (long)EmergencyContactById.AddressId;
                        AddressManager.UpdateAddress(lEmergencyContact.Address);
                        ContactInfoManager.UpdateContactInfo(lEmergencyContact.ContactInfo);
                        EmergencyContactFromDB = EmergencyContactManager.UpdateEmergencyContact(lEmergencyContact);
                    }
                }
                formIsDirty = false;
                this.Close();
            }
        }

        private Boolean validate()
        {
            ToolStripStatusLabelErrorEmergency.Text = "";
            if (string.IsNullOrEmpty(TextBoxEmergencyContFirstName.Text.Trim()))
            {
                ToolStripStatusLabelErrorEmergency.Text = EnterFirstNameErrorMsg;
                TextBoxEmergencyContFirstName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxEmergencyContLastName.Text.Trim()))
            {
                ToolStripStatusLabelErrorEmergency.Text = EnterLastNameErrorMsg;
                TextBoxEmergencyContLastName.Select();
                return false;
            }
            if (RbtEmergencyContGender.Gender == GenderSelection.None)
            {
                ToolStripStatusLabelErrorEmergency.Text = SelectGenderErrorMsg;
                RbtEmergencyContGender.Select();
                return false;
            }
            if (GroupBoxEmergencyContAddress.StateId == 0L)
            {
                ToolStripStatusLabelErrorEmergency.Text = ChooseStateErrorMsg;
                GroupBoxEmergencyContAddress.selected_field(Fields.state);
                return false;
            }
            if (ComboBoxSwapTextBoxEmergencyContRelationShip.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorEmergency.Text = SelectRelationshipErrorMsg;
                ComboBoxSwapTextBoxEmergencyContRelationShip.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxEmergencyContPhone.Text.Trim('-').Trim()))
            {
                ToolStripStatusLabelErrorEmergency.Text = EnterPhoneNumberErrorMsg;
                TextBoxEmergencyContPhone.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorEmergency.Text = "";
            TextBoxEmergencyContFirstName.ResetText();
            TextBoxEmergencyContId.ResetText();
            TextBoxEmergencyContInitial.ResetText();
            TextBoxEmergencyContLastName.ResetText();
            TextBoxEmergencyContMobile.ResetText();
            TextBoxEmergencyContPhone.ResetText();
            GroupBoxEmergencyContAddress.Clear();
            GroupBoxEmergencyContAddress.StateId = (long)Global.Company.Address.StatesId!;
            GroupBoxEmergencyContAddress.ReadOnly = false;
            RbtEmergencyContGender.Gender = GenderSelection.Male;
            ComboBoxSwapTextBoxEmergencyContRelationShip.SelectedIndex = -1;

        }

        private void ComboBoxSwapTextBoxEmergencyContRelationShip_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxEmergencyContRelationShip.DroppedDown = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnEmergencyContSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnEmergencyContCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }

        private void FormEmergencyContact_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show(SaveChangesErrorMsg, "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnEmergencyContSave.PerformClick();
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
