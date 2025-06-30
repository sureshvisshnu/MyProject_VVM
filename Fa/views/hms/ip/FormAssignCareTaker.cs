using fa.api.Hms;
using fa.common;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.views.hms.patient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.hms.ip
{
    enum CareTakerHisTableColumn
    {
        FROM, TO, PDOCTOR, SDOCTOR, PNURSE, SNURSE, NOTE, ADOCTOR
    }
    public partial class FormAssignCareTaker : FormPatientBase
    {
        public static string CancelConfirmText = "There are unsaved changes, do you want cancel?";
        public static string ChangePatientErrorMsg = "There are unsaved changes, do you want to change the patient?";
        public static string SelectErrorMsg = "Please select {0}.";
        public static string EnterNoteErrorMsg = "Please enter note.";
        public long PatientId = 0L;
        long AdmissionId = 0L;
        public bool CreateReAssignmentOnLoad = false;
        public FormAssignCareTaker()
        {
            excludedObjects = new string[] { "ab2ToolStripIpTransfer", "GridViewCareTakerHistory", "PatientIdTransport" };
            InitializeComponent();
        }
        private void FormAssignCareTaker_Load(object sender, EventArgs e)
        {
            ResetForm();
            EnableButton(false);
            if (CreateReAssignmentOnLoad)
            {
                LoadPatientInfo();
                ComboBoxNewPrimaryDoctor.Select();
            }
            else
            {
                TextBoxPatientSearch.TextBox.Select();
            }
            formIsDirty = false;
        }
        private void ResetForm()
        {
            TextBoxPatientSearch.TextBox.ResetText();
            CareTakerErrorMsg.Text = "";
            GridViewCareTakerHistory.Rows.Clear();
            InPatientInfoMin.Clear();
            TextBoxPrimaryDoctor.ResetText();
            TextBoxPrimaryNurse.ResetText();
            TextBoxSecondaryDoctor.ResetText();
            TextBoxSecondaryNurse.ResetText();
            TextBoxNote.ResetText();
            LoadCombo();
            ComboBoxNewPrimaryDoctor.ResetText();
            ComboBoxNewSecondaryDoctor.ResetText();
            ComboBoxNewPrimaryNurse.ResetText();
            ComboBoxNewSecondaryNurse.ResetText();
            ComboBoxAuthorizedBy.ResetText();
            ComboBoxNewPrimaryDoctor.SelectedIndex = -1;
            ComboBoxNewSecondaryDoctor.SelectedIndex = -1;
            ComboBoxNewPrimaryNurse.SelectedIndex = -1;
            ComboBoxNewSecondaryNurse.SelectedIndex = -1;
            ComboBoxAuthorizedBy.SelectedIndex = -1;
        }
        private void LoadCombo()
        {
            ComboUtils.InitializeDoctorCombo(ComboBoxNewPrimaryDoctor, Global.Company.CompanyId);
            ComboUtils.InitializeDoctorCombo(ComboBoxNewSecondaryDoctor, Global.Company.CompanyId);
            ComboUtils.InitializeNurseCombo(ComboBoxNewPrimaryNurse, Global.Company.CompanyId);
            ComboUtils.InitializeNurseCombo(ComboBoxNewSecondaryNurse, Global.Company.CompanyId);
            ComboUtils.InitializeDoctorCombo(ComboBoxAuthorizedBy, Global.Company.CompanyId);
        }
        private void EnableButton(bool enable)
        {
            BtnReAssign.Enabled = enable;
        }
        private void BtnPatientSearch_Click(object sender, EventArgs e)
        {
            FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
            FormPatientSearch.PatientSearchType = PatientSearchType.Inpatient;
            FormPatientSearch.Searchstring = TextBoxPatientSearch.Text;
            FormPatientSearch.ShowDialog();
            ComboBoxNewPrimaryDoctor.Focus();
        }
        protected override void PatientIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PatientIdTransport.Text))
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show(ChangePatientErrorMsg, "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.No)
                    {
                        ComboBoxNewPrimaryDoctor.Focus();
                        return;
                    }
                }
                PatientId = long.Parse(PatientIdTransport.Text);
                LoadPatientInfo();
                formIsDirty = false;
            }
        }
        private void LoadPatientInfo()
        {
            ResetForm();
            EnableButton(true);
            InPatientInfoMin.Type = PatientTypes.InPatient;
            InPatientInfoMin.PatientId = PatientId;
            LoadCurrentCareTaker();
        }
        private void LoadCurrentCareTaker()
        {
            AdmissionId = 0L;
            InPatientAdmission InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId(PatientId);
            if (InPatientAdmission != null)
            {
                AdmissionId = InPatientAdmission.Id;
                LoadCareTakerHistory();
                if (InPatientAdmission.CurrentMedicalTeam != null)
                {
                    MedicalTeam MedicalTeam = InPatientAdmission.CurrentMedicalTeam;
                    TextBoxPrimaryDoctor.Text = MedicalTeam.PrimaryDoctor != null ? MedicalTeam.PrimaryDoctor.Name : string.Empty;
                    TextBoxSecondaryDoctor.Text = MedicalTeam.SecondaryDoctor != null ? MedicalTeam.SecondaryDoctor.Name : string.Empty;
                    TextBoxPrimaryNurse.Text = MedicalTeam.PrimaryCareGiver != null ? MedicalTeam.PrimaryCareGiver.Name : string.Empty;
                    TextBoxSecondaryNurse.Text = MedicalTeam.SecondaryCareGiver != null ? MedicalTeam.SecondaryCareGiver.Name : string.Empty;
                }
            }
        }
        private void LoadCareTakerHistory()
        {
            int Row = -1;
            IList<MedicalTeam> IpMedicalTeamHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByAdmitionId(AdmissionId);
            if (IpMedicalTeamHistoryInfo != null && IpMedicalTeamHistoryInfo.Count > 0)
            {
                foreach (MedicalTeam History in IpMedicalTeamHistoryInfo)
                {
                    Row = GridViewCareTakerHistory.Rows.Add();
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.FROM].Value = History.From.ToString(Global.Company.DateFormat);
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.TO].Value = History.To != null ? ((DateTime)History.To).ToString(Global.Company.DateFormat) : string.Empty;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.PDOCTOR].Value = History.PrimaryDoctor != null ? History.PrimaryDoctor.Name : string.Empty;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.SDOCTOR].Value = History.SecondaryDoctor != null ? History.SecondaryDoctor.Name : string.Empty;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.PNURSE].Value = History.PrimaryCareGiver != null ? History.PrimaryCareGiver.Name : string.Empty;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.SNURSE].Value = History.SecondaryCareGiver != null ? History.SecondaryCareGiver.Name : string.Empty;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.NOTE].Value = History.Notes;
                    GridViewCareTakerHistory.Rows[Row].Cells[(int)CareTakerHisTableColumn.ADOCTOR].Value = History.AuthorizedByDoctor != null ? History.AuthorizedByDoctor.Name : string.Empty;
                }
            }
        }
        private void TextBoxPatientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                BtnPatientSearch.PerformClick();
            }
        }
        private MedicalTeam GetMedicalTeamFromFrom()
        {
            MedicalTeam MedicalTeam = new MedicalTeam();
            MedicalTeam.Active = true;
            MedicalTeam.Notes = TextBoxNote.Text;
            MedicalTeam.AdmissionId = AdmissionId;
            MedicalTeam.AuthorizedByDoctorId = ((Employee)ComboBoxAuthorizedBy.Items[ComboBoxAuthorizedBy.SelectedIndex]).Id;
            MedicalTeam.PrimaryDoctorId = ((Employee)ComboBoxNewPrimaryDoctor.Items[ComboBoxNewPrimaryDoctor.SelectedIndex]).Id;
            MedicalTeam.PrimaryCareGiverId = ((Employee)ComboBoxNewPrimaryNurse.Items[ComboBoxNewPrimaryNurse.SelectedIndex]).Id;
            if (ComboBoxNewSecondaryDoctor.SelectedIndex > -1)
            {
                MedicalTeam.SecondaryDoctorId = ((Employee)ComboBoxNewSecondaryDoctor.Items[ComboBoxNewSecondaryDoctor.SelectedIndex]).Id;
            }
            if (ComboBoxNewSecondaryNurse.SelectedIndex > -1)
            {
                MedicalTeam.SecondaryCareGiverId = ((Employee)ComboBoxNewSecondaryNurse.Items[ComboBoxNewSecondaryNurse.SelectedIndex]).Id;
            }
            MedicalTeam.From = Global.getTransactionDate();
            return MedicalTeam;
        }
        private void BtnReAssign_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                MedicalTeam MedicalTeam = GetMedicalTeamFromFrom();
                if(MedicalTeam.AdmissionId != 0)
                {
                    if (MedicalTeam != null)
                    {
                        if (MedicalTeamManager.Instance.AddMidicalTeam(MedicalTeam) != null)
                        {
                            LoadPatientInfo();
                        }
                    }
                    if (CreateReAssignmentOnLoad)
                    {
                        this.Close();
                    }
                    ComboBoxNewPrimaryDoctor.Select();
                    formIsDirty = false;
                }
                else
                {
                    CareTakerErrorMsg.Text = "Please select Ip patient only...";
                }
            }
        }
        private bool ValidateForm()
        {
            if (ComboBoxNewPrimaryDoctor.SelectedIndex < 0)
            {
                CareTakerErrorMsg.Text = string.Format(SelectErrorMsg, "Primary Doctor");
                ComboBoxNewPrimaryDoctor.Select();
                return false;
            }
            if (ComboBoxNewPrimaryNurse.SelectedIndex < 0)
            {
                CareTakerErrorMsg.Text = string.Format(SelectErrorMsg, "Primary Nurse");
                ComboBoxNewPrimaryNurse.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxNote.Text.Trim()))
            {
                CareTakerErrorMsg.Text = EnterNoteErrorMsg;
                TextBoxNote.Select();
                return false;
            }
            if (ComboBoxAuthorizedBy.SelectedIndex < 0)
            {
                CareTakerErrorMsg.Text = string.Format(SelectErrorMsg, "Authorized Person");
                ComboBoxAuthorizedBy.Select();
                return false;
            }
            return true;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    ComboBoxNewPrimaryDoctor.Focus();
                    return;
                }
            }
            ResetForm();
            EnableButton(false);
            formIsDirty = false;
            TextBoxPatientSearch.TextBox.Select();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnPatientSearch.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnReAssign.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.Tab && BtnPatientSearch.Selected && ActiveControl == ab2ToolStripIpTransfer)
            {
                ComboBoxNewPrimaryDoctor.Select();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnCancel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                ComboBoxNewPrimaryDoctor.Select();
            }
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.Shift)
            {
                e.IsInputKey = true;
                if (BtnReAssign.Enabled)
                {
                    BtnReAssign.Select();
                }
                else
                {
                    ComboBoxAuthorizedBy.Select();
                }
            }
        }

        private void ComboBoxNewPrimaryDoctor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.Shift)
            {
                e.IsInputKey = true;
                BtnCancel.Select();
            }
        }
    }
}
