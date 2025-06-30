using System;
using System.Windows.Forms;
using fa.model.Hms.Ip;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.libraries.utils;
using fa.views.controls.hms;
using fa.views.hms.patient;
using fa.common;
using System.Windows.Controls;
using VisioForge.Libs.MediaFoundation.OPM;

namespace fa.views.hms.ip
{
    public enum EnumIPQueueMode
    {
        DOCTOR_VIEW=0, NURSE_VIEW=1, TECHNICIAN_VIEW=2
    }

    public partial class FormIPQueue : FormPatientBase
    {
        public static string NothingFoundMsg = "No patient in the queue";
        public static string SelectWardErrorMsg = "Please select ward.";
        public EnumIPQueueMode IPQueueMode = EnumIPQueueMode.NURSE_VIEW;

        WardManager WardManager = WardManager.Instance;
        IpManager IpManager = IpManager.Instance;
        long PatientId = 0L;
        public bool IpTrans = false;
        public bool DischargePatientOnload = false;

        public FormIPQueue()
        {
            InitializeComponent();
        }
        private void FormIpQueue_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                ComboBoxStatus.SelectedIndex = DischargePatientOnload ? 0 : 1;
                BtnQueueSearch_Click(sender, e);
                InPatientInformation();
                if (DischargePatientOnload)
                {
                    BtnDischargePatient.Location = BtnInpatientVisit.Location;
                    //BtnDischargePatient.Enabled = false;
                    buttonEditDisCharge.Location = new Point(873, 585);
                    //buttonEditDisCharge.Enabled = false;
                    BtnReAssignCareTaker.Visible = false;
                    BtnPatientWardTransfer.Visible = false;
                    BtnInpatientVisit.Visible = false;
                }
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ComboBoxFilter.Focus();
                });
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ResetForm()
        {
            ErrorMsg.Text = "";
            patientInfoMin1.Clear();
            IpQueueGrid.Clear();
            PatientSearchBox.ResetText();
            ComboUtils.InitializeWardComboWithAll(ComboBoxFilter, Global.Company.CompanyId);
            ComboBoxFilter.SelectedIndex = 0;
            //ComboBoxStatus.SelectedIndex = DischargePatientOnload ? 0 : 1;
        }

        private void BtnQueueSearch_Click(object sender, EventArgs e)
        {
            IpQueueGrid.Clear();
            ErrorMsg.Text = "";
            if (ValidateForm())
            {
                LoadIPQueue();
                if (IpQueueGrid.IsEmpty)
                {
                    ErrorMsg.Text = NothingFoundMsg;
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        ComboBoxFilter.Focus();
                    });
                }
                else
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        ComboBoxFilter.Focus();
                    });
                }
                int Count = IpQueueGrid.CheckRowCount();
                if (Count > 0)
                {
                    long IpId = (long)IpQueueGrid.IpId!;
                    IpQueueGrid.FocusModifiedRowByIpId(IpId);
                    int RowIndex = IpQueueGrid.RowIndex;

                    BtnDischargePatient.Enabled = (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus(RowIndex) == "Discharged") ? false : true;
                    buttonEditDisCharge.Enabled = (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus(RowIndex) == "Discharged") ? true : false;
                }
                else
                {
                    BtnDischargePatient.Enabled = false;
                    buttonEditDisCharge.Enabled = false;
                }
            }
        }
        private bool ValidateForm()
        {
            if (ComboBoxFilter.SelectedIndex < 0)
            {
                ComboBoxFilter.Select();
                ErrorMsg.Text = SelectWardErrorMsg;
                return false;
            }
            return true;
        }

        private void LoadIPQueue()
        {
            IpQueueGrid.SearchString = PatientSearchBox.Text;
            if (ComboBoxStatus.SelectedIndex == 0)
            {
                IpQueueGrid.FillterBy = IpQueueStatus.ALL;
            }
            else if (ComboBoxStatus.SelectedIndex == 1)
            {
                IpQueueGrid.FillterBy = IpQueueStatus.FOR_DOCTORS;
            }
            else if (ComboBoxStatus.SelectedIndex == 2)
            {
                IpQueueGrid.FillterBy = IpQueueStatus.DISCHARGED;
            }
            IpQueueGrid.StatusIndex = ComboBoxStatus.SelectedIndex;
            IpQueueGrid.WardId = ComboBoxFilter.SelectedIndex > 0 ? ((Ward)ComboBoxFilter.Items[ComboBoxFilter.SelectedIndex]).Id : 0L;
            BtnInpatientVisit.Enabled = !IpQueueGrid.IsEmpty;
            BtnPatientWardTransfer.Enabled = !IpQueueGrid.IsEmpty;
            BtnReAssignCareTaker.Enabled = !IpQueueGrid.IsEmpty;
            BtnDischargePatient.Enabled = !IpQueueGrid.IsEmpty;
            if (IpQueueGrid.IpId != null)
            {
                patientInfoMin1.Type = PatientTypes.InPatient;
                patientInfoMin1.PatientId = GePatientIdByIpId((long)IpQueueGrid.IpId);
            }
        }

        private void ButtonInPatientVisit_Click(object sender, EventArgs e)
        {
            if (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "IP")
            {
                FormConsulting formConsulting = null;
                FormInPatientCareForNurse InPatientConsultation = null;
                this.PatientIdTransport.Text = IpManager.Instance.GetInPatientAdmissionById((long)IpQueueGrid.IpId).PatientId.ToString();
                if (IPQueueMode == EnumIPQueueMode.DOCTOR_VIEW)
                {
                    formConsulting = new FormConsulting(this);
                    formConsulting.PatientIPId = (long)IpQueueGrid.IpId;
                    formConsulting.PatientOpId = SetOPId();
                    formConsulting.ShowDialog();
                }
                else
                {
                    InPatientConsultation = new FormInPatientCareForNurse
                    {
                        PatientId = (long)IpManager.Instance.GetInPatientAdmissionById((long)IpQueueGrid.IpId).PatientId!,
                        PatientIpId = (long)IpQueueGrid.IpId
                    };
                    InPatientConsultation.ShowDialog();
                }
                BtnQueueSearch_Click(sender, e);
                IpQueueGrid.FocusModifiedRowByIpId(IPQueueMode == EnumIPQueueMode.DOCTOR_VIEW ? (long)formConsulting.PatientIPId : (long)InPatientConsultation.PatientIpId);
                InPatientInformation();
            }
        }
        private long? SetOPId()
        {
            if (IpQueueGrid.IpId != null)
            {
                InPatientAdmission InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById((long)IpQueueGrid.IpId);
                if (InPatientAdmission != null)
                {
                    return InPatientAdmission.OpRegistrationId;
                }
            }
            return null;
        }
        private void IpQueueGrid_Click(object sender, EventArgs e)
        {
            if (IpQueueGrid.IpId != null)
            {
                PatientId = GePatientIdByIpId((long)IpQueueGrid.IpId);
                if (PatientId > 0)
                {
                    BtnInpatientVisit.Enabled = true;
                    BtnReAssignCareTaker.Enabled = true;
                    BtnPatientWardTransfer.Enabled = true;
                    BtnDischargePatient.Enabled = (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "Discharged") ? false : true;
                    buttonEditDisCharge.Enabled = (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "Discharged") ? true : false;
                    patientInfoMin1.Type = PatientTypes.InPatient;
                    patientInfoMin1.PatientId = PatientId;
                }

            }
        }
        private long GePatientIdByIpId(long IpId)
        {
            InPatientAdmission inPatientAdmission = IpManager.GetInPatientAdmissionById(IpId);
            if (inPatientAdmission != null)
            {
                return (long)inPatientAdmission.PatientId;
            }
            return 0L;
        }
        private void BtnPatientWardTransfer_Click(object sender, EventArgs e)
        {
            if (IpTrans == false && IpQueueGrid.GetStatus() == "IP")
            {
                long IpId = (long)IpQueueGrid.IpId;
                this.PatientIdTransport.Text = IpManager.Instance.GetInPatientAdmissionById(IpId).PatientId.ToString();
                FormTransferPatient IpTransfer = new FormTransferPatient(this);
                IpTransfer.PatientIdTransport.Text = PatientIdTransport.Text;
                IpTransfer.TransStatus = true;
                IpTransfer.ShowDialog();
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                BtnQueueSearch_Click(sender, e);
                IpQueueGrid.FocusModifiedRowByIpId(IpId);
                InPatientInformation();
            }
            else if (IpTrans == true)
            {
                FormTransferPatient IpTransfer = new FormTransferPatient(this);
                IpTransfer.PatientIdTransport.Text = PatientIdTransport.Text;
                this.Close();

            }
        }

        private void BtnReassign_Click(object sender, EventArgs e)
        {
            if (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "IP")
            {
                long IpId = (long)IpQueueGrid.IpId;
                FormAssignCareTaker FormAssignCareTaker = new FormAssignCareTaker();
                FormAssignCareTaker.CreateReAssignmentOnLoad = true;
                FormAssignCareTaker.PatientId = GePatientIdByIpId(IpId);
                FormAssignCareTaker.ShowDialog();
                ResetForm();
                BtnQueueSearch_Click(sender, e);
                IpQueueGrid.FocusModifiedRowByIpId(IpId);
                InPatientInformation();
            }
        }

        private void IpQueueGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (IpQueueGrid.IpId != null)
            {
                PatientId = GePatientIdByIpId((long)IpQueueGrid.IpId);
                if (PatientId > 0)
                {
                    BtnInpatientVisit.Enabled = true;
                    BtnReAssignCareTaker.Enabled = true;
                    BtnPatientWardTransfer.Enabled = true;
                    BtnDischargePatient.Enabled = true;
                    patientInfoMin1.Type = PatientTypes.InPatient;
                    patientInfoMin1.PatientId = PatientId;
                }
            }
        }
        public void InPatientInformation()
        {
            if (IpQueueGrid.IpId != null)
            {
                InPatientAdmission InPatientId = IpManager.GetInPatientAdmissionById((long)IpQueueGrid.IpId);
                if (InPatientId != null)
                {
                    RefreshIpDetails(true);
                    patientInfoMin1.Type = PatientTypes.InPatient;
                    patientInfoMin1.PatientId = InPatientId.PatientId;
                }
            }
            else
            {
                RefreshIpDetails(false);
                BtnDischargePatient.Enabled = false;
                patientInfoMin1.Clear();
            }
        }
        private void RefreshIpDetails(bool enable)
        {
            BtnInpatientVisit.Enabled = enable;
            BtnReAssignCareTaker.Enabled = enable;
            BtnPatientWardTransfer.Enabled = enable;
        }
        private void IpQueueGrid_KeyDown(object sender, KeyEventArgs e)
        {
            InPatientInformation();
        }

        private void IpQueueGrid_KeyUp(object sender, KeyEventArgs e)
        {
            InPatientInformation();
        }

        private void BtnDischargePatient_Click(object sender, EventArgs e)
        {
            if (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "IP")
            {
                FormDischargePatient FormDischargePatient = new FormDischargePatient
                {
                    PatientId = (long)IpManager.Instance.GetInPatientAdmissionById((long)IpQueueGrid.IpId).PatientId,
                    PatientIpId = (long)IpQueueGrid.IpId
                };
                FormDischargePatient.ShowDialog();
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                BtnQueueSearch_Click(sender, e);
                InPatientAdmission Admission = IpManager.Instance.GetInPatientAdmissionById(FormDischargePatient.PatientIpId);
                if (Admission != null && Admission.Status != InPatientStatus.DISCHARGED)
                {
                    IpQueueGrid.FocusModifiedRowByIpId(Admission.Id);
                    InPatientInformation();
                }
                Cursor.Current = Cursors.Default;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F6))
            {
                BtnReAssignCareTaker.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F7))
            {
                BtnPatientWardTransfer.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnInpatientVisit.PerformClick();
                return true;
            }
            if (this.ActiveControl == BtnInpatientVisit)
            {
                if (keyData == (Keys.Tab | Keys.Shift))
                {
                    ActiveControl = IpQueueGrid;
                    IpQueueGrid.Focus();
                    IpQueueGrid.SelectLastRow();
                    return true;
                }
            }
            if (this.ActiveControl == ComboBoxFilter.Control)
            {
                if (keyData == (Keys.Tab | Keys.Shift))
                {
                    BtnDischargePatient.Select();
                    return true;
                }
            }
            if (toolStripGoButton.Selected && keyData == Keys.Tab)
            {
                if (!IpQueueGrid.IsEmpty)
                {
                    ActiveControl = IpQueueGrid;
                    IpQueueGrid.Focus();
                    IpQueueGrid.SelectFirstRow();
                    return true;
                }
                else
                {
                    ActiveControl = toolStrip1;
                    toolStrip1.Select();
                    ComboBoxFilter.Focus();
                }
            }
            if (keyData == Keys.Up && ActiveControl == IpQueueGrid)
            {
                HandleArrowKey(-1);
            }
            if (keyData == Keys.Down && ActiveControl == IpQueueGrid)
            {
                HandleArrowKey(1);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleArrowKey(int rowAdjustment)
        {
            int count = IpQueueGrid.CheckRowCount();
            if (count > 0)
            {
                int newRowIndex = IpQueueGrid.RowIndex + (count == 1 ? 0 : rowAdjustment);
                string status = IpQueueGrid.GetStatus(newRowIndex);

                bool isDischarged = IpQueueGrid.IpId != null && status == "Discharged";
                BtnDischargePatient.Enabled = !isDischarged;
                buttonEditDisCharge.Enabled = isDischarged;
            }
            else
            {
                BtnDischargePatient.Enabled = false;
                buttonEditDisCharge.Enabled = false;
            }
        }

        private void BtnDischargePatient_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                ActiveControl = toolStrip1;
                toolStrip1.Select();
                ComboBoxFilter.Focus();
                e.IsInputKey = true;
            }
        }

        private void IpQueueGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (IpQueueGrid.CheckIsLastRow())
            {
                if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
                {
                    BtnInpatientVisit.Focus();
                    e.IsInputKey = true;
                }
            }
            if (IpQueueGrid.CheckIsFirstRow())
            {
                if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
                {
                    ActiveControl = toolStrip1;
                    toolStrip1.Focus();
                    toolStripGoButton.Select();
                    e.IsInputKey = true;
                }
            }
        }
        private void ComboBoxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxFilter.SelectedIndex = 0;
            if (ComboBoxStatus.SelectedIndex == 0)
            {
                StatusSeparator.Visible = true;
                LabelWard.Visible = true;
                ComboBoxFilter.Visible = true;
                buttonEditDisCharge.Visible = true;
            }
            else if (ComboBoxStatus.SelectedIndex == 1)
            {
                StatusSeparator.Visible = true;
                LabelWard.Visible = true;
                ComboBoxFilter.Visible = true;
                buttonEditDisCharge.Visible = false;
            }
            else if (ComboBoxStatus.SelectedIndex == 2)
            {
                StatusSeparator.Visible = false;
                LabelWard.Visible = false;
                ComboBoxFilter.Visible = false;
                buttonEditDisCharge.Visible = true;
            }
        }
        private void buttonDisChargePrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (IpQueueGrid.IpId != null && IpQueueGrid.GetStatus() == "Discharged")
            {
                if (IpQueueGrid.IpId != null)
                {
                    FormDischargePatient FormDischargePatient = new FormDischargePatient
                    {
                        PatientId = (long)IpManager.Instance.GetInPatientAdmissionById((long)IpQueueGrid.IpId).PatientId,
                        PatientIpId = (long)IpQueueGrid.IpId
                    };
                    FormDischargePatient.IsDischarged = true;
                    FormDischargePatient.ShowDialog();
                    Cursor.Current = Cursors.WaitCursor;
                    ResetForm();
                    BtnQueueSearch_Click(sender, e);
                    InPatientAdmission Admission = IpManager.Instance.GetInPatientAdmissionById(FormDischargePatient.PatientIpId);
                    if (Admission != null && Admission.Status != InPatientStatus.DISCHARGED)
                    {
                        IpQueueGrid.FocusModifiedRowByIpId(Admission.Id);
                        InPatientInformation();
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
