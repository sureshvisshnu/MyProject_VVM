using fa.libraries.Validation;
using fa.views.controls;
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

namespace fa.views.hms.patient
{
    public partial class FormParentOrGuardian : FormPatientBase
    {
        public static string EnterFirstNameErrorMsg = "Please enter first name.";
        public static string EnterLastNameErrorMsg = "Please enter last name.";
        public static string SelectGenderErrorMsg = "Please select gender.";
        public static string EnterDOBErrorMsg = "Please enter date of birth.";
        public static string EnterValidDOBErrorMsg = "Please enter valid date";
        public static string SelectRelationshipErrorMsg = "Please select relationship.";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string SaveChangesErrorMsg = "Do you want to save the changes?";

        public long? GuardianId = null;
        GuardianManager GuardianManager = null!;
        AddressManager AddressManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        DateValidation DateValidation = null!;
        FormPatientBase parent = null!;

        public FormParentOrGuardian(object sender)
        {
            parent = (FormPatientBase)sender;
            GuardianManager = GuardianManager.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
        }

        private void FormParentOrGuardian_Load(object sender, EventArgs e)
        {
            ResetForm();
            GroupBoxGuardianAddress.CountryId = (long)Global.Company.CountryId!;
            GroupBoxGuardianAddress.StateId = (long)Global.Company.Address.StatesId!;
            if (GuardianId != null)
            {
                LoadGuardianInfo();
            }
            DateTimePickerGuardianDob.MaxDate = DateTime.Now;
            TextBoxGuardianFirstName.Select();
            ResetDirtyFlag();
        }

        private void LoadGuardianInfo()
        {
            Guardian GuardianFromDB = GuardianManager.GetGuardianById((long)GuardianId!);
            if (GuardianFromDB != null)
            {
                if (DateUtils.ValidDate(GuardianFromDB.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    TextBoxGuardianAge.Text = GuardianFromDB.Age.ToString();
                    DateTimePickerGuardianDob.Date = (DateTime)DateUtils.ToDate(GuardianFromDB.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                }
                TextBoxPatientId.Text = GuardianFromDB.PatientId.ToString();
                TextBoxGuardianEmployer.Text = GuardianFromDB.Employer;
                TextBoxGuardianFirstName.Text = GuardianFromDB.FirstName;
                TextBoxGuardianId.Text = GuardianFromDB.Id.ToString();
                TextBoxGuardianIncome.Text = double.Parse(GuardianFromDB.Income).ToString(TextUtils.DecimalPlace(TextBoxGuardianIncome.Decimals));
                TextBoxGuardianInitial.Text = GuardianFromDB.MiddleInitial;
                TextBoxGuardianLastName.Text = GuardianFromDB.LastName;
                TextBoxGuardianOccupation.Text = GuardianFromDB.Occupation;
                TextBoxGuardianTaxId.Text = GuardianFromDB.TaxId;
                RbtGuardianGender.Gender = (GenderSelection)GuardianFromDB.Gender + 1;
                GroupBoxGuardianAddress.CountryId = (long)Global.Company.CountryId!;
                ComboBoxSwapTextBoxGuardianRelationShip.SelectedIndex = ComboBoxSwapTextBoxGuardianRelationShip.FindStringExact(GuardianFromDB.RelationShip.ToString());
                if (GuardianFromDB.Address != null)
                {
                    Address Address = GuardianFromDB.Address;
                    GroupBoxGuardianAddress.AddressLine1 = Address.AddressLine1;
                    GroupBoxGuardianAddress.AddressLine2 = Address.AddressLine2;
                    GroupBoxGuardianAddress.CityName = Address.CityOrTown;
                    GroupBoxGuardianAddress.DistrictName = Address.District;
                    GroupBoxGuardianAddress.PinCode = Address.PinCode;
                    GroupBoxGuardianAddress.StateId = Address.StatesId != null ? (long)Address.StatesId : 0L;
                }
                if (GuardianFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = GuardianFromDB.ContactInfo;
                    TextBoxGuardianPhone.Text = lContactInfo.Phone;
                    TextBoxGuardianMobile.Text = lContactInfo.Mobile;
                }
            }
        }
        private Guardian GetGuardianFromForm()
        {
            Guardian lGuardian = new Guardian();
            if (!string.IsNullOrEmpty(TextBoxGuardianId.Text))
            {
                lGuardian.Id = Convert.ToInt64(TextBoxGuardianId.Text);
            }
            else
            {
                lGuardian.Id = 0L;
            }

            lGuardian.PatientId = long.Parse(TextBoxPatientId.Text);
            lGuardian.FirstName = TextBoxGuardianFirstName.Text;
            lGuardian.MiddleInitial = TextBoxGuardianInitial.Text;
            lGuardian.LastName = TextBoxGuardianLastName.Text;
            lGuardian.Gender = (Gender)RbtGuardianGender.Gender - 1;
            if (DateUtils.ToDate(((DateTime)DateTimePickerGuardianDob.Date!).ToString(Global.Company.DateFormat), Global.Company.DateFormat) != null)
                lGuardian.DateOfBirth = (DateTime)DateUtils.ToDate(((DateTime)DateTimePickerGuardianDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            lGuardian.Employer = TextBoxGuardianEmployer.Text;
            lGuardian.TaxId = TextBoxGuardianTaxId.Text;
            lGuardian.Occupation = TextBoxGuardianOccupation.Text;
            lGuardian.Income = TextBoxGuardianIncome.Text;
            lGuardian.CompanyId = Global.Company.CompanyId;
            lGuardian.Address = GetGuardianAddressFromForm();
            lGuardian.ContactInfo = GetGuardianContactInfoFromForm();
            lGuardian.RelationShip = (ComboBoxSwapTextBoxGuardianRelationShip.SelectedIndex > -1) ? (RelationShip)ComboBoxSwapTextBoxGuardianRelationShip.SelectedIndex : RelationShip.SELF;

            return lGuardian;
        }
        private Address GetGuardianAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = GroupBoxGuardianAddress.AddressLine1.Trim();
            lAddress.AddressLine2 = GroupBoxGuardianAddress.AddressLine2.Trim();
            lAddress.CityOrTown = GroupBoxGuardianAddress.CityName.Trim();
            lAddress.District = GroupBoxGuardianAddress.DistrictName.Trim();
            lAddress.PinCode = GroupBoxGuardianAddress.PinCode.Trim();
            lAddress.StatesId = GroupBoxGuardianAddress.StateId;
            return lAddress;
        }

        private ContactInfo GetGuardianContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxGuardianPhone.Text.Replace("-", "");
            lContactInfo.Mobile = TextBoxGuardianMobile.Text.Replace("-", "");
            return lContactInfo;
        }

        private void BtnGuardianCancel_Click(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                if (MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnGuardianSave.PerformClick();
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

        private void BtnGuardianSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                Guardian lGuardian = GetGuardianFromForm();
                if (lGuardian.Id == 0)
                {
                    Guardian GuardianFromDB = GuardianManager.AddGuardian(lGuardian);
                    TextBoxGuardianId.Text = GuardianFromDB.Id.ToString();
                }
                else
                {
                    Guardian GuardianById = GuardianManager.GetGuardianById(lGuardian.Id);
                    if (GuardianById != null)
                    {
                        lGuardian.ContactInfoId = lGuardian.ContactInfo.Id = (long)GuardianById.ContactInfoId!;
                        lGuardian.AddressId = lGuardian.Address.AddressId = (long)GuardianById.AddressId!;
                        AddressManager.UpdateAddress(lGuardian.Address);
                        ContactInfoManager.UpdateContactInfo(lGuardian.ContactInfo);
                        GuardianManager.UpdateGuardian(lGuardian);

                    }
                }
                formIsDirty = false;
                this.Close();
            }
        }
        private Boolean validate()
        {
            ToolStripStatusLabelErrorParent.Text = "";
            if (string.IsNullOrEmpty(TextBoxGuardianFirstName.Text.Trim()))
            {
                ToolStripStatusLabelErrorParent.Text = EnterFirstNameErrorMsg;
                TextBoxGuardianFirstName.Select();
                return false;
            }

            if (string.IsNullOrEmpty(TextBoxGuardianLastName.Text.Trim()))
            {
                ToolStripStatusLabelErrorParent.Text = EnterLastNameErrorMsg;
                TextBoxGuardianLastName.Select();
                return false;
            }
            if (RbtGuardianGender.Gender == GenderSelection.None)
            {
                ToolStripStatusLabelErrorParent.Text = SelectGenderErrorMsg;
                RbtGuardianGender.Select();
                return false;
            }
            if (DateTimePickerGuardianDob.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerGuardianDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorParent.Text = EnterValidDOBErrorMsg;
                DateTimePickerGuardianDob.Focus();
                return false;
            }
            if (!DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerGuardianDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ToolStripStatusLabelErrorParent.Text = EnterDOBErrorMsg;
                DateTimePickerGuardianDob.Focus();
                return false;
            }
            if (GroupBoxGuardianAddress.StateId == 0L)
            {
                ToolStripStatusLabelErrorParent.Text = ChooseStateErrorMsg;
                GroupBoxGuardianAddress.selected_field(Fields.state);
                return false;
            }
            if (ComboBoxSwapTextBoxGuardianRelationShip.SelectedIndex < 0)
            {
                ToolStripStatusLabelErrorParent.Text = SelectRelationshipErrorMsg;
                ComboBoxSwapTextBoxGuardianRelationShip.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            ToolStripStatusLabelErrorParent.Text = "";
            TextBoxGuardianAge.ResetText();
            TextBoxGuardianEmployer.ResetText();
            TextBoxGuardianFirstName.ResetText();
            TextBoxGuardianId.ResetText();
            TextBoxGuardianIncome.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxGuardianIncome.Text = TextUtils.DecimalPlace(TextBoxGuardianIncome.Decimals);
            TextBoxGuardianInitial.ResetText();
            TextBoxGuardianLastName.ResetText();
            TextBoxGuardianMobile.ResetText();
            TextBoxGuardianOccupation.ResetText();
            TextBoxGuardianPhone.ResetText();
            TextBoxGuardianTaxId.ResetText();
            GroupBoxGuardianAddress.Clear();
            GroupBoxGuardianAddress.ReadOnly = false;
            RbtGuardianGender.Gender = GenderSelection.Male;
            ComboBoxSwapTextBoxGuardianRelationShip.SelectedIndex = -1;
            DateTimePickerGuardianDob.Format = Global.Company.DateFormat;
            DateTimePickerGuardianDob.MaxDate = (DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerGuardianDob.Reset();
            TextBoxGuardianIncome.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
        }

        private void ComboBoxSwapTextBoxGuardianRelationShip_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxGuardianRelationShip.DroppedDown = false;
        }

        private void DateTimePickerGuardianDob_Leave(object sender, EventArgs e)
        {

            if (DateTimePickerGuardianDob.Date != null && DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerGuardianDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                TextBoxGuardianAge.Text = DateUtils.ComputeAge(DateTime.Now, (DateTime)DateUtils.ToDate(((DateTime)DateTimePickerGuardianDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)!).ToString();
            }
            else
            {
                DateTimePickerGuardianDob.Reset();
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnGuardianSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnGuardianCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }

        private void FormParentOrGuardian_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show(SaveChangesErrorMsg, "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnGuardianSave.PerformClick();
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
