using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office.CustomUI;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.views.hms.patient;

namespace fa.views.hms.ip
{
    public partial class FormTransferPatient : FormPatientBase
    {
        public static string CancelConfirmText = "There are unsaved changes, do you want cancel?";
        public static string ChangePatientErrorMsg = "There are unsaved changes, do you want to change the patient?";
        public static string EnterWardorBedErrorMsg = "Please {0} for Transfer";
        public static string SaveSuccessMsg = "Saved success.";
        public static string IpPatientSelectMsg = "This patient not found in IP!";
        public static string IpPatientStatusMsg = "This patient is not active now";
        public long? PatientIpId = null;
        public long PatientId = 0L;
        long AdmissionId = 0L;
        public long OldBId = 0L;
        public long LctnHistId = 0L;
        public string patient;
        IpManager IpManager = IpManager.Instance;
        public FormTransferPatient(object sender)
        {
            excludedObjects = new string[] { "ab2ToolStripIpTransfer", "DataViewLocationHist", "PatientIdTransport" };
            InitializeComponent();
        }
        public bool TransStatus = false;
        public void GetWard()
        {
            ComboxWardTo.ResetText();
            ComboxBedTo.ResetText();
            ComboxBedTo.Items.Clear();
            ComboxWardTo.Items.Clear();
            ComboxAuthor.ResetText();
            ComboxAuthor.Items.Clear();
            ComboUtils.InitializeWardCombo(ComboxWardTo, Global.Company.CompanyId);
            ComboUtils.InitializeDoctorCombo(ComboxAuthor, Global.Company.CompanyId);
            ComboxAuthor.SelectedIndex = -1;
            ComboxWardTo.SelectedIndex = -1;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                ToolStripBtnSearch.PerformClick();
                return true;
            }
            if (keyData == (Keys.F8))
            {
                BtnTransfer.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
            }
            if (keyData == Keys.Tab && ActiveControl == ab2ToolStripIpTransfer && ToolStripBtnSearch.Selected)
            {
                GroupBoxTransferToDetail.Select();
                ComboxWardTo.Select();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnCancel)
            {
                GroupBoxTransferToDetail.Select();
                ComboxWardTo.Select();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnTransfer)
            {
                BtnCancel.Select();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }
        private void FormTransferPatient_Load(object sender, EventArgs e)
        {
            if (TransStatus)
            {
                ResetForm();
                Patient IpPatientInfo = PatientManager.Instance.GetPatientById(long.Parse(PatientIdTransport.Text));
                TextBoxPatientSearch.Text = IpPatientInfo.PatientNumber;
                LoadIpInfo();
                ComboxWardTo.Select();
                ResetDirtyFlag();
            }
            else
            {
                BtnTransfer.Enabled = false;
                ResetForm();
                EnableFormControl(false);
                ComboxWardTo.Select();
                ResetDirtyFlag();
                return;
            }
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
                        ComboxWardTo.Focus();
                        return;
                    }
                }
                PatientId = long.Parse(PatientIdTransport.Text);
                LoadIpInfo();
                ResetDirtyFlag();
            }
        }
        private void LoadIpInfo()
        {
            ResetForm();
            EnableFormControl(true);
            InPatientInfoMin.Type = fa.common.PatientTypes.InPatient;
            InPatientInfoMin.PatientId = PatientId;
            LoadActiveIp();
        }
        private void LoadActiveIp()
        {
            AdmissionId = 0L;
            InPatientAdmission InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId(PatientId);
            if (InPatientAdmission != null)
            {
                AdmissionId = InPatientAdmission.Id;
                LoadIpLocationHistory();
                if (InPatientAdmission.CurrentLocation != null)
                {
                    InPatientLocation IpLoctn = IpManager.GetInPatientLocationbyAdmitId(InPatientAdmission.Id);
                    if (IpLoctn != null)
                    {
                        LctnHistId = IpLoctn.Id;
                        TxtboxWardFrom.Text = IpLoctn.Ward != null ? IpLoctn.Ward.Name : string.Empty;
                        TxtboxBedFrom.Text = IpLoctn.Bed != null ? IpLoctn.Bed.Name : string.Empty;
                    }
                }
            }
        }
        private void LoadIpLocationHistory()
        {
            IList<InPatientLocation> Historys = IpManager.ListAllEntryByPatientId(AdmissionId, Global.Company.CompanyId);
            if (Historys != null && Historys.Count > 0)
            {
                int cnt = 0;
                foreach (InPatientLocation IpHistory in Historys)
                {
                    DataViewLocationHist.Rows.Add();
                    DataViewLocationHist.Rows[cnt].Cells[0].Value = IpHistory.DateMovedIn.ToString(Global.Company.DateFormat);
                    if (IpHistory.DateMovedOut.Year > 1900)
                    {
                        DataViewLocationHist.Rows[cnt].Cells[1].Value = IpHistory.DateMovedOut != null ? ((DateTime)IpHistory.DateMovedOut).ToString(Global.Company.DateFormat) : string.Empty; /*String.Format("{0:d}", IpHistory.DateMovedOut);*/
                    }
                    Ward GetWardnam = WardManager.Instance.GetWardById((long)IpHistory.WardId);
                    DataViewLocationHist.Rows[cnt].Cells[2].Value = GetWardnam.Name;
                    OldBId = (long)IpHistory.BedId;
                    Bed GetBednam = WardManager.Instance.GetWardBedById(OldBId);
                    DataViewLocationHist.Rows[cnt].Cells[3].Value = GetBednam.Name;
                    DataViewLocationHist.Rows[cnt].Cells[4].Value = IpHistory.Notes;
                    cnt++;
                }
            }
        }
        private InPatientLocation GetIpHistoryDetails()
        {
            InPatientLocation IpLctnHistory = new InPatientLocation();
            {
                IpLctnHistory.AdmissionId = AdmissionId;
                IpLctnHistory.Notes = TxtboxTransferResn.Text.Trim();
                TimeSpan timeSpan = DateTime.Now.TimeOfDay;
                IpLctnHistory.DateMovedIn = Global.getTransactionDate().Date + timeSpan;
                IpLctnHistory.Active = true;
                if (ComboxWardTo.SelectedIndex > -1)
                {
                    IpLctnHistory.WardId = ((Ward)ComboxWardTo.Items[ComboxWardTo.SelectedIndex]).Id;
                }
                if (ComboxBedTo.SelectedIndex > -1)
                {
                    IpLctnHistory.BedId = ((Bed)ComboxBedTo.Items[ComboxBedTo.SelectedIndex]).Id;
                }
                if (ComboxAuthor.SelectedIndex > -1)
                {
                    IpLctnHistory.AuthorizedByDoctorId = ((Employee)ComboxAuthor.Items[ComboxAuthor.SelectedIndex]).Id;
                }
            }
            return IpLctnHistory;
        }
        private void ComboxWardTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboxWardTo.SelectedIndex > -1)
            {
                ComboxBedTo.ResetText();
                ComboxBedTo.Visible = true;
                ComboUtils.InitializeWardBedCombo(ComboxBedTo, ((Ward)ComboxWardTo.Items[ComboxWardTo.SelectedIndex]).Id);
            }
            else
            {
                ComboxBedTo.SelectedIndex = -1;
                ComboxBedTo.Visible = false;
            }
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        private void ResetForm()
        {
            IpTransferErrorMsg.Text = "";
            TextBoxPatientSearch.TextBox.ResetText();
            InPatientInfoMin.Clear();
            TxtboxWardFrom.ResetText();
            TxtboxBedFrom.ResetText();
            TxtboxTransferResn.ResetText();
            DataViewLocationHist.Rows.Clear();
            GetWard();
            TextBoxPatientSearch.Focus();
        }
        private void EnableFormControl(Boolean enable)
        {
            BtnTransfer.Enabled = enable;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    ComboxWardTo.Focus();
                    return;
                }
            }
            EnableFormControl(false);
            ResetForm();
            ResetDirtyFlag();
        }
        private Boolean ValidateFormForIpTranser()
        {
            IpTransferErrorMsg.Text = "";
            if (ComboxWardTo.SelectedIndex < 0)
            {
                IpTransferErrorMsg.Text = string.Format(EnterWardorBedErrorMsg, "Select Ward");
                ComboxWardTo.Select();
                return false;
            }
            if (ComboxBedTo.SelectedIndex < 0)
            {

                IpTransferErrorMsg.Text = string.Format(EnterWardorBedErrorMsg, "Select Bed");
                ComboxBedTo.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TxtboxTransferResn.Text.Trim()))
            {
                IpTransferErrorMsg.Text = string.Format(EnterWardorBedErrorMsg, "Give Reason");
                TxtboxTransferResn.Select();
                return false;
            }
            if (ComboxAuthor.SelectedIndex < 0)
            {
                IpTransferErrorMsg.Text = string.Format(EnterWardorBedErrorMsg, "Select Authorizing Dr.");
                ComboxAuthor.Select();
                return false;
            }
            return true;
        }
        private void BtnTransfer_Click(object sender, EventArgs e)
        {
            if (ValidateFormForIpTranser())
            {
                InPatientLocation IpLctnHistory = GetIpHistoryDetails();
                if (IpLctnHistory.AdmissionId != 0)
                {
                    if (IpLctnHistory.Id == 0)
                    {
                        IpManager.AddIpLocationHistory(IpLctnHistory, LctnHistId);
                        LoadIpInfo();
                        GetWard();
                        TxtboxTransferResn.ResetText();
                        IpTransferErrorMsg.Text = SaveSuccessMsg;
                    }
                    else
                    {
                        IpManager.UpdateIpLocationHistory(IpLctnHistory);
                        IpTransferErrorMsg.Text = SaveSuccessMsg;
                    }
                    ResetDirtyFlag();
                }
                else
                {
                    IpTransferErrorMsg.Text = "Please select Ip patient only...";
                }
            }
        }
        private void ToolStripBtnSearch_Click_1(object sender, EventArgs e)
        {
            FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
            FormPatientSearch.PatientSearchType = PatientSearchType.Inpatient;
            FormPatientSearch.Searchstring = TextBoxPatientSearch.Text.Trim();
            FormPatientSearch.ShowDialog();
            if (string.IsNullOrEmpty(PatientIdTransport.Text))
            {
                TextBoxPatientSearch.Focus();
            }
            else
            {
                ComboxWardTo.Focus();
            }
        }
        private void TextBoxPatientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ToolStripBtnSearch.PerformClick();
            }
        }

        private void ComboxWardTo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.Shift)
            {
                e.IsInputKey = true;
                BtnCancel.Select();
            }
        }
    }
}
