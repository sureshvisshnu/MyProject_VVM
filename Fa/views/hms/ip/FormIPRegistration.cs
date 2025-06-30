using fa.libraries.utils;
using fa.views.hms.op;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using System;
using System.IO;
using System.Windows.Forms;
using fa.api.Accounting;
using System.Linq;
using System.Collections.Generic;
using fa.model.Hms.Op;
using System.Drawing;
using fa.model.Accounting.Masters;

namespace fa.views.hms.ip
{
    public partial class FormIPRegistration : FormPatientBase
    {
        public static string SelectPatientErrorMsg = "Please select patient";
        public static string SelectPrimaryConsultantErrorMsg = "Primary consultant could not be empty, Please select primary consultant";
        public static string SelectPrimaryNurseErrorMsg = "Primary nurse could not be empty, Please select primary nurse";
        public static string SelectWardErrorMsg = "Ward could not be empty, Please select ward";
        public static string SelectBedErrorMsg = "Bed could not be empty, Please select bed";
        public static string DoNotAllowToSave_PatientInIpMsg = "{0} has been admitted as in-patient, could not create IP registration.";
        public static string SelectPatientInsuranceErrorMsg = "Please select insurance.";
        public static string SaveSuccessMsg = "Saved...";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";

        PatientManager PatientManager = null;
        IpManager IpManager = null;
        FormPatientBase parent = null;
        public long OpId;
        public long? IpId = 0L;
        public FormIPRegistration(object sender)
        {
            parent = (FormPatientBase)sender;
            PatientManager = PatientManager.Instance;
            IpManager = IpManager.Instance;
            InitializeComponent();
        }
        private bool IsInPatient()
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                InPatientAdmission InPatientAdmission = IpManager.GetUnDischargedInPatientAdmissionByPatientId(long.Parse(TextBoxPatientId.Text));
                if (InPatientAdmission == null)
                {
                    return true;
                }
                else
                {
                    OpId = InPatientAdmission.OpRegistrationId;
                    IList<InPatientLocation> InPatientLocation = IpManager.ListInPatientLocationId(InPatientAdmission.Id);
                    if (InPatientLocation != null && InPatientLocation.Count > 0)
                    {
                        InPatientLocation lInPatientLocation = InPatientLocation.FirstOrDefault(x => x.Active == true);
                        if (lInPatientLocation != null)
                        {
                            if (InPatientLocation.First().WardId != null)
                            {
                                Ward Ward = WardManager.Instance.GetWardById((long)InPatientLocation.First().WardId);
                                if (Ward != null)
                                {
                                    ComboBoxWard.SelectedIndex = ComboBoxWard.FindStringExact(Ward.Name);
                                }

                            }
                            if (InPatientLocation.First().BedId != null)
                            {
                                Bed Bed = WardManager.Instance.GetWardBedById((long)InPatientLocation.First().BedId);
                                if (Bed != null)
                                {
                                    if (!ComboBoxSwapTextBoxBed.Items.Contains(Bed)) { ComboBoxSwapTextBoxBed.Items.Add(Bed); }
                                    ComboBoxSwapTextBoxBed.SelectedIndex = ComboBoxSwapTextBoxBed.FindStringExact(Bed.Name);
                                }
                            }
                        }
                    }
                    if (InPatientAdmission.MedicalTeamHistory != null && InPatientAdmission.MedicalTeamHistory.Count > 0)
                    {
                        MedicalTeam MedicalTeam = InPatientAdmission.MedicalTeamHistory.FirstOrDefault(x => x.Active == true);
                        if (MedicalTeam != null)
                        {
                            if (MedicalTeam.PrimaryDoctorId != null)
                            {
                                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById((long)MedicalTeam.PrimaryDoctorId);
                                ComboBoxPrimaryConsultant.SelectedIndex = ComboBoxPrimaryConsultant.FindStringExact(Emp.Name);
                            }
                            if (MedicalTeam.SecondaryDoctorId != null)
                            {
                                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById((long)MedicalTeam.SecondaryDoctorId);
                                ComboBoxSecondaryConsultant.SelectedIndex = ComboBoxSecondaryConsultant.FindStringExact(Emp.Name);
                            }
                            if (MedicalTeam.PrimaryCareGiverId != null)
                            {
                                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById((long)MedicalTeam.PrimaryCareGiverId);
                                ComboBoxPrimaryNurse.SelectedIndex = ComboBoxPrimaryNurse.FindStringExact(Emp.Name);
                            }
                            if (MedicalTeam.SecondaryCareGiverId != null)
                            {
                                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById((long)MedicalTeam.SecondaryCareGiverId);
                                ComboBoxSecondaryNurse.SelectedIndex = ComboBoxSecondaryNurse.FindStringExact(Emp.Name);
                            }
                        }
                    }
                }
            }
            return false;
        }
        private void FormIPRegistration_Load(object sender, EventArgs e)
        {
            ResetForm();
            TextBoxPatientId.Text = parent.PatientIdTransport.Text;
            LoadCombo();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                BindPatientData();
            }
            EnableForm(IsInPatient());
            if (CheckBoxBillToInsurance.Enabled == true)
            {
                CheckBoxBillToInsurance.Focus();
            }
            else
            {
                ComboBoxPrimaryConsultant.Select();
            }
            this.formIsDirty = false;
        }
        private void EnableForm(bool Enable)
        {
            ComboBoxPrimaryConsultant.Visible = Enable;
            ComboBoxSecondaryConsultant.Visible = Enable;
            ComboBoxPrimaryNurse.Visible = Enable;
            ComboBoxSecondaryNurse.Visible = Enable;
            ComboBoxWard.Visible = Enable;
            ComboBoxSwapTextBoxBed.Visible = Enable;

            BtnAdmit.Enabled = Enable;
            BtnCancel.Enabled = Enable;

        }
        public void BindPatientData()
        {
            Patient PatientData;
            PatientData = PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text));

            if (PatientData != null)
            {
                ComboUtils.InitializePatientInsurerCombo(ComboBoxSelectInsurance, PatientData.Id, "IP");
                TextBoxPatientName.Text = PatientData.Name;
                if (DateUtils.ValidDate(PatientData.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    TextBoxPatientAge.Text = PatientData.Age.ToString();
                    TextBoxPatientDOB.Text = PatientData.DateOfBirth.ToShortDateString();
                }
                PatientNumberIp.PatientNumber = PatientData.PatientNumber;
                if (PatientData.Photo != null)
                {
                    MemoryStream Stream = new MemoryStream(PatientData.Photo);
                    PatientPhoto.Photo = System.Drawing.Image.FromStream(Stream);
                }
                if (PatientData.Address != null)
                {
                    TextBoxAddress.Text = PatientData.Address.FullAddress.Replace("\n", "").Replace(", ", "," + System.Environment.NewLine);
                }
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllIPActiveInsuranceInfoByPatientId(PatientData.Id);
                if (InsuranceInfo != null && InsuranceInfo.Count > 0)
                {
                    LabelInsurance.Text = "**INSURANCE**";
                    CheckBoxBillToInsurance.Enabled = true;
                    ComboBoxSelectInsurance.Visible = true;
                }
                else
                {
                    LabelInsurance.Text = "**NO INSURANCE**";
                    CheckBoxBillToInsurance.Enabled = false;
                    ComboBoxSelectInsurance.Visible = false;
                }
                if (OpId != 0L)
                {
                    Registration OpRegistrationById = OpManager.Instance.GetOpRegistrationById(OpId);
                    if (OpRegistrationById != null && OpRegistrationById.InsuranceInfoId != null)
                    {
                        InsuranceInfo lInsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)OpRegistrationById.InsuranceInfoId, true);
                        if (lInsuranceInfo != null && lInsuranceInfo.IPInsuranceCoverage)
                        {
                            CheckBoxBillToInsurance.Checked = true;
                            ComboBoxSelectInsurance.Visible = true;
                            LabelSelectInsurance.Font = new Font("Tahoma", 8, FontStyle.Regular);
                            ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(lInsuranceInfo.InsuranceName);
                        }
                    }
                }
            }
            else
            {
                BtnAdmit.Enabled = false;
            }
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeDoctorCombo(ComboBoxPrimaryConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeDoctorCombo(ComboBoxSecondaryConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeWardCombo(ComboBoxWard, Global.Company.CompanyId);
            ComboUtils.InitializeDepartmentCombo(ComboBoxDepartment, Global.Company.CompanyId);
            ComboUtils.InitializeNurseCombo(ComboBoxPrimaryNurse, Global.Company.CompanyId);
            ComboUtils.InitializeNurseCombo(ComboBoxSecondaryNurse, Global.Company.CompanyId);
        }
        private void ComboBoxWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxWard.SelectedIndex > -1)
            {
                ComboBoxSwapTextBoxBed.ResetText();
                ComboBoxSwapTextBoxBed.Visible = true;
                ComboUtils.InitializeWardBedCombo(ComboBoxSwapTextBoxBed, ((Ward)ComboBoxWard.Items[ComboBoxWard.SelectedIndex]).Id);
            }
            else
            {
                ComboBoxSwapTextBoxBed.SelectedIndex = -1;
                ComboBoxSwapTextBoxBed.Visible = false;
            }
        }
        private InPatientAdmission GetInPatientAdmissionFromForm()
        {
            InPatientAdmission lInPatientAdmission = new InPatientAdmission();
            if (!string.IsNullOrEmpty(TextBoxInpatientAdmissionId.Text))
            {
                lInPatientAdmission.Id = Convert.ToInt64(TextBoxInpatientAdmissionId.Text);
            }
            else
            {
                lInPatientAdmission.Id = 0L;
            }
            if (ComboBoxSelectInsurance.SelectedIndex > -1)
            {
                lInPatientAdmission.InsuranceInfoId = ((InsuranceInfo)ComboBoxSelectInsurance.Items[ComboBoxSelectInsurance.SelectedIndex]).Id;
            }
            lInPatientAdmission.IsBillToInsurance = CheckBoxBillToInsurance.Checked;
            lInPatientAdmission.Status = InPatientStatus.ADMITTED;
            lInPatientAdmission.PatientId = long.Parse(TextBoxPatientId.Text);
            lInPatientAdmission.CompanyId = Global.Company.CompanyId;
            lInPatientAdmission.DateOfAdmission = Global.getTransactionDate();
            lInPatientAdmission.OpRegistrationId = OpId;
            lInPatientAdmission.AdmissionNote = string.Empty;
            return lInPatientAdmission;
        }

        private InPatientLocation GetInPatientLocationFromForm()
        {
            InPatientLocation InPatientLocation = new InPatientLocation();
            if (ComboBoxWard.SelectedIndex > -1)
            {
                InPatientLocation.WardId = ((Ward)ComboBoxWard.Items[ComboBoxWard.SelectedIndex]).Id;
            }
            if (ComboBoxSwapTextBoxBed.SelectedIndex > -1)
            {
                InPatientLocation.BedId = ((Bed)ComboBoxSwapTextBoxBed.Items[ComboBoxSwapTextBoxBed.SelectedIndex]).Id;
            }
            if (ComboBoxPrimaryConsultant.SelectedIndex > -1)
            {
                InPatientLocation.AuthorizedByDoctorId = ((Employee)ComboBoxPrimaryConsultant.Items[ComboBoxPrimaryConsultant.SelectedIndex]).Id;
            }
            InPatientLocation.DateMovedIn = Global.getTransactionDate();
            InPatientLocation.Active = true;
            InPatientLocation.Notes = string.Empty;
            return InPatientLocation;
        }
        private MedicalTeam GetMedicalTeamFromForm()
        {
            MedicalTeam MedicalTeam = new MedicalTeam();
            if (ComboBoxPrimaryConsultant.SelectedIndex > -1)
            {
                MedicalTeam.PrimaryDoctorId = ((Employee)ComboBoxPrimaryConsultant.Items[ComboBoxPrimaryConsultant.SelectedIndex]).Id;
            }
            if (ComboBoxSecondaryConsultant.SelectedIndex > -1)
            {
                MedicalTeam.SecondaryDoctorId = ((Employee)ComboBoxSecondaryConsultant.Items[ComboBoxSecondaryConsultant.SelectedIndex]).Id;
            }
            if (ComboBoxPrimaryNurse.SelectedIndex > -1)
            {
                MedicalTeam.PrimaryCareGiverId = ((Employee)ComboBoxPrimaryNurse.Items[ComboBoxPrimaryNurse.SelectedIndex]).Id;
            }
            if (ComboBoxSecondaryNurse.SelectedIndex > -1)
            {
                MedicalTeam.SecondaryCareGiverId = ((Employee)ComboBoxSecondaryNurse.Items[ComboBoxSecondaryNurse.SelectedIndex]).Id;
            }
            MedicalTeam.From = Global.getTransactionDate();
            MedicalTeam.Active = true;
            MedicalTeam.Notes = string.Empty;
            return MedicalTeam;
        }
        private void BtnAdmit_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                Registration RegistrationFromDB = OpManager.Instance.GetOPRecordIfAdmitted(long.Parse(TextBoxPatientId.Text));
                if (RegistrationFromDB == null)
                {
                    InPatientAdmission lInPatientAdmission = GetInPatientAdmissionFromForm();
                    lInPatientAdmission.PatientLocationHistory = new List<InPatientLocation>();
                    lInPatientAdmission.PatientLocationHistory.Add(GetInPatientLocationFromForm());
                    lInPatientAdmission.MedicalTeamHistory = new List<MedicalTeam>();
                    lInPatientAdmission.MedicalTeamHistory.Add(GetMedicalTeamFromForm());

                    if (lInPatientAdmission.Id == 0)
                    {
                        //lInPatientAdmission.PatientIPNumber = IpManager.FeachPatientIPID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                        lInPatientAdmission.PatientIPNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.IP_ID, Global.getTransactionDate().Date);
                        IpManager.AddInPatientAdmission(lInPatientAdmission);
                        //IpManager.GenerateNextPatientIPID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                    }
                    else
                    {
                        InPatientAdmission InPatientAdmissionById = IpManager.GetInPatientAdmissionById(lInPatientAdmission.Id);
                        if (InPatientAdmissionById != null)
                        {
                            IpManager.UpdateInPatientAdmission(lInPatientAdmission);
                        }

                    }
                }
                else
                {
                    MessageBox.Show(string.Format(DoNotAllowToSave_PatientInIpMsg, RegistrationFromDB.Patient.Name), "Warning");
                    return;
                }
                formIsDirty = false;
                this.Close();
            }
        }
        private Boolean validate()
        {
            InPatientErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                InPatientErrorMsg.Text = SelectPatientErrorMsg;
                return false;
            }
            if (CheckBoxBillToInsurance.Checked && ComboBoxSelectInsurance.SelectedIndex == -1 && CheckBoxBillToInsurance.Enabled == true)
            {
                InPatientErrorMsg.Text = SelectPatientInsuranceErrorMsg;
                ComboBoxSelectInsurance.Select();
                return false;
            }
            if (ComboBoxPrimaryConsultant.SelectedIndex < 0)
            {
                InPatientErrorMsg.Text = SelectPrimaryConsultantErrorMsg;
                ComboBoxPrimaryConsultant.Select();
                return false;
            }
            if (ComboBoxPrimaryNurse.SelectedIndex < 0)
            {
                InPatientErrorMsg.Text = SelectPrimaryNurseErrorMsg;
                ComboBoxPrimaryNurse.Select();
                return false;
            }
            if (ComboBoxWard.SelectedIndex < 0)
            {
                InPatientErrorMsg.Text = SelectWardErrorMsg;
                ComboBoxWard.Select();
                return false;
            }
            if (ComboBoxSwapTextBoxBed.SelectedIndex < 0)
            {
                InPatientErrorMsg.Text = SelectBedErrorMsg;
                ComboBoxSwapTextBoxBed.Select();
                return false;
            }

            return true;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (ComboBoxSelectInsurance.Text != "" || ComboBoxPrimaryConsultant.Text != "" || ComboBoxSecondaryConsultant.Text != "" || ComboBoxPrimaryNurse.Text != "" ||
                ComboBoxSecondaryNurse.Text != "" || ComboBoxWard.Text != "" || ComboBoxSwapTextBoxBed.Text != "")
            {
                if (formIsDirty)
                {
                    if (MessageBox.Show(CancelConfirmText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
                ResetForm();
                TextBoxPatientId.ResetText();
                EnableForm(false);
                FormIPRegistration_Load(this, EventArgs.Empty);
                this.formIsDirty = false;
                this.Close();
            }
            else if (ComboBoxSelectInsurance.Text == "" && ComboBoxPrimaryConsultant.Text == "" && ComboBoxSecondaryConsultant.Text == "" && ComboBoxPrimaryNurse.Text == "" &&
                ComboBoxSecondaryNurse.Text == "" && ComboBoxWard.Text == "" && ComboBoxSwapTextBoxBed.Text == "")
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(CancelConfirmText, " Exit Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }
        private void ResetForm()
        {
            TextBoxPatientAge.ResetText();
            TextBoxPatientDOB.ResetText();
            TextBoxPatientName.ResetText();
            GroupBoxPatientAddress.Clear();
            ComboBoxPrimaryConsultant.ResetText();
            ComboBoxPrimaryConsultant.SelectedIndex = -1;
            ComboBoxSecondaryConsultant.ResetText();
            ComboBoxSecondaryConsultant.SelectedIndex = -1;
            ComboBoxPrimaryNurse.ResetText();
            ComboBoxPrimaryNurse.SelectedIndex = -1;
            ComboBoxSecondaryNurse.ResetText();
            ComboBoxSecondaryNurse.SelectedIndex = -1;
            ComboBoxWard.ResetText();
            ComboBoxWard.SelectedIndex = -1;
            ComboBoxSwapTextBoxBed.ResetText();
            ComboBoxSwapTextBoxBed.SelectedIndex = -1;
            ComboBoxDepartment.ResetText();
            ComboBoxDepartment.SelectedIndex = -1;
            InPatientErrorMsg.Text = "";
            PatientNumberIp.PatientNumber = "000000000000";
            ComboBoxSwapTextBoxBed.Visible = false;
            PatientPhoto.Clear();
        }


        private void ComboBoxPrimaryConsultant_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPrimaryConsultant.DroppedDown = false;
        }

        private void ComboBoxSecondaryConsultant_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSecondaryConsultant.DroppedDown = false;
        }

        private void ComboBoxWard_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxWard.DroppedDown = false;
        }

        private void ComboBoxDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxDepartment.DroppedDown = false;
        }

        private void ComboBoxPrimaryNurse_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxPrimaryNurse.DroppedDown = false;
        }

        private void ComboBoxSecondaryNurse_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSecondaryNurse.DroppedDown = false;
        }

        private void ComboBoxSwapTextBoxBed_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxBed.DroppedDown = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnAdmit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CheckBoxBillToInsurance_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxBillToInsurance.Checked)
            {
                LabelSelectInsurance.Font = new Font("Tahoma", 8, FontStyle.Bold);
                ComboBoxSelectInsurance.Visible = true;
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllIPActiveInsuranceInfoByPatientId(long.Parse(TextBoxPatientId.Text));
                if (InsuranceInfo != null && InsuranceInfo.Count > 0)
                {
                    if (InsuranceInfo.Where(x => x.IsPrimary).ToList().Count > 0)
                    {
                        ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(InsuranceInfo.Where(x => x.IsPrimary).ToList().First().InsuranceName);
                    }
                    else
                    {
                        ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(InsuranceInfo.First().InsuranceName);
                    }
                }
            }
            else
            {
                LabelSelectInsurance.Font = new Font("Tahoma", 8, FontStyle.Regular);
                ComboBoxSelectInsurance.SelectedIndex = -1;
                ComboBoxSelectInsurance.Visible = false;
            }
        }

        private void ComboBoxSwapTextBoxBed_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                BtnAdmit.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxWard.Select();
            }
        }

        private void BtnAdmit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (CheckBoxBillToInsurance.Enabled == true)
                {
                    CheckBoxBillToInsurance.Focus();
                }
                else
                {
                    ComboBoxPrimaryConsultant.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxSwapTextBoxBed.Select();
            }
        }

        private void ComboBoxSelectInsurance_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                ComboBoxPrimaryConsultant.Select();
            }
        }

        private void CheckBoxBillToInsurance_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnAdmit.Focus();
            }
        }

        private void ComboBoxPrimaryConsultant_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (CheckBoxBillToInsurance.Enabled == true)
                {
                    ComboBoxSelectInsurance.Select();
                }
                else
                {
                    BtnAdmit.Focus();
                }
            }
        }
        private void FormIPRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show(CancelConfirmText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
    }
}
