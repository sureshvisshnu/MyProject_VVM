using System;
using System.Drawing;
using System.Windows.Forms;
using fa.views.hms.ip;
using fa.views.hms.patient;
using fa.model.Hms.Master;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.views.controls;
using fa.model.Hms.Op;
using System.IO;
using fa.model.hms.common;
using fa.model.hms.config;
using fa.api.Accounting;
using fa.model.Common;
using System.Collections.Generic;
using System.Linq;
using fa.model.Accounting.Masters;
using DocumentFormat.OpenXml.InkML;
using fa.model.Hms.common;

namespace fa.views.hms.op
{
    public partial class FormOPRegistration : FormPatientBase
    {
        public static string DoNotAllowToSave_PatientIsDeceasedMsg = "{0} is deceased, could not create OP registration.";
        public static string DoNotAllowToSave_PatientInOpMsg = "{0} is in OP queue, could not create a new one";
        public static string DoNotAllowToSave_PatientInIpMsg = "{0} has been admitted as in-patient, could not create OP registration.";
        public static string RegistrationFeeMessage = "Registration Fee of {0} {1} is required to register as an Out Patient";
        public static string SaveSuccessMsg = "Saved...";
        public static string SelectPatientErrorMsg = "Patient is required, please select patient";
        public static string EnterVisitResonErrorMsg = "Visit reason is required, please enter visit reson";
        public static string DoNotAllowToDeleteMsg = "Cannot delete this OP Details as the patient has been consulted";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string EnterRegistrationFeeErrorMsg = "Registration fee is required, please enter registration fee";
        public static string RefNoErrorMsg = "Please contact administrator to generate token number.";
        public static string PatientNotRegisteredErrorMsg = "The patient {0} do not have any OP registration";
        public static string DeleteSuccessMsg = "The patient {0}'s OP Registration has been deleted";
        public static string SelectPatientInsuranceErrorMsg = "Please select insurance.";
        public static string EnterFeeErrorMsg = "Registration fee is Rs {0} only.";
        public bool CreateOpRegisterOnLoad = false;
        PatientManager PatientManager = null;
        OpManager OpManager = null;
        FormPatientBase parent = null;

        public FormOPRegistration(Object sender)
        {
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
            else
                parent = null;
            PatientManager = PatientManager.Instance;
            OpManager = OpManager.Instance;
            excludedObjects = new string[] { "toolStripOpRegistration", "OpQueueGrid", "TextBoxPatientId", "BtnPatientSearch", "BtnNewPatient" };
            InitializeComponent();
            OpQueueGrid.IsMyQueue = false;
        }
        private void FormOPRegistration_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                if (parent != null) { TextBoxPatientId.Text = parent.PatientIdTransport.Text; }
                loadCombo();
                ComboBoxOpStatus.SelectedIndex = 1;
                if (CreateOpRegisterOnLoad)
                {
                    SetOpRegistrationViewFromPatient();
                    GroupBoxOutpatientRegister.Location = new Point(12, 12);
                    this.Size = new Size(GroupBoxOutpatientRegister.Right + 25, GroupBoxOutpatientRegister.Bottom + 65);
                    this.CenterToParent();
                    if (parent != null) { TextBoxPatientId.Text = parent.PatientIdTransport.Text; }
                    LoadPatientInfo();
                    EnableForm(true);
                    ComboBoxSwapTextBoxOpRegistrationConsultant.Select();
                }
                else
                {
                    EnableForm(false);
                    LoadOpQueue();
                    BtnPatientSearch.Select();
                }
                this.formIsDirty = false;
                btnprintOpRegistration.Enabled = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadOpQueue()
        {
            OpQueueGrid.SearchString = TextBoxOpSearch.Text;
            if (ComboBoxOpStatus.SelectedIndex > -1)
            {
                OpQueueGrid.OPStatus = (Status)ComboBoxOpStatus.SelectedIndex;
            }
            else
            {
                OpQueueGrid.OPStatus = Status.OPEN;
            }
            OpQueueGrid.IsMyQueue = false;
        }
        private bool FrmTxtFocus(string input, TextBox sender)
        {
            if (input.Length < 0)
            {
                return false;
            }
            else
            {
                sender.Focus();
                sender.SelectionStart = 0;
                sender.SelectionLength = sender.Text.Length;
                return true;
            }
        }
        private void loadCombo()
        {
            ComboUtils.InitializeDepartmentCombo(ComboBoxSwapTextBoxOpRegistrationDepartment, Global.Company.CompanyId);
            ComboUtils.InitializeDoctorCombo(ComboBoxSwapTextBoxOpRegistrationConsultant, Global.Company.CompanyId);
        }
        public void LoadPatientInfo()
        {
            Patient PatientData = PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text));
            if (PatientData != null)
            {
                LinkLabelChangePatientInfo.Enabled = true;
                ComboUtils.InitializePatientInsurerCombo(ComboBoxSelectInsurance, PatientData.Id, "OP");
                TextBoxOpRegistrationName.Text = PatientData.Name;
                if (DateUtils.ValidDate(PatientData.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    TextBoxOpRegistrationAge.Text = PatientData.Age.ToString();
                    TextBoxOpRegistrationDOB.Text = PatientData.DateOfBirth.ToString(Global.Company.DateFormat);
                }
                RbtOpRegistrationGender.Gender = (GenderSelection)PatientData.Gender + 1;
                PatientNumberOp.PatientNumber = PatientData.PatientNumber;
                if (PatientData.Photo != null)
                {
                    MemoryStream Stream = new MemoryStream(PatientData.Photo);
                    PatientPhoto.Photo = System.Drawing.Image.FromStream(Stream);
                }
                if (PatientData.Address != null)
                {
                    TextBoxAddress.Text = PatientData.Address.FullAddress.Replace("\n", "").Replace(", ", "," + System.Environment.NewLine);
                }
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllOPActiveInsuranceInfoByPatientId(PatientData.Id).Where(x => x.OPInsuranceCoverage == true).ToList();
                if (InsuranceInfo != null && InsuranceInfo.Count > 0)
                {
                    LabelInsurance.Text = "**INSURANCE**";
                    CheckBoxBillToInsurance.Enabled = true;
                    CheckBoxBillToInsurance.Checked = false;
                    CheckBoxBillToInsurance.Checked = true;
                }
                else
                {
                    CheckBoxBillToInsurance.Enabled = false;
                }
            }
        }
        private Registration GetRegistrationFromForm()
        {
            Registration lRegistration = new Registration();
            if (!string.IsNullOrEmpty(TextBoxOpRegistrationId.Text))
            {
                lRegistration.Id = Convert.ToInt64(TextBoxOpRegistrationId.Text);
            }
            else
            {
                lRegistration.Id = 0L;
            }
            lRegistration.HasRegistrationFeePaid = (FeeGroupBox1.Visible && !CheckBoxOpRegistrationNoFeeReceived.Checked) ? true : false;
            lRegistration.RegistrationFee = lRegistration.HasRegistrationFeePaid ? double.Parse(TextBoxOpRegistrationAmountReceived.Text) : 0.00;
            lRegistration.Status = Status.OPEN;
            lRegistration.IsBillToInsurance = CheckBoxBillToInsurance.Checked;
            lRegistration.PatientId = long.Parse(TextBoxPatientId.Text);
            lRegistration.TockenNo = LabelTokenNumber.Text;
            lRegistration.CompanyId = Global.Company.CompanyId;
            lRegistration.ReasonForTheVisit = TextBoxOpRegistrationReasonForVisit.Text;
            if (ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex > -1)
            {
                lRegistration.RequestedDoctorId = ((Employee)ComboBoxSwapTextBoxOpRegistrationConsultant.Items[ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex]).Id;
            }
            if (ComboBoxSelectInsurance.SelectedIndex > -1)
            {
                lRegistration.InsuranceInfoId = ((InsuranceInfo)ComboBoxSelectInsurance.Items[ComboBoxSelectInsurance.SelectedIndex]).Id;
            }
            TimeSpan timeSpan = DateTime.Now.TimeOfDay;
            lRegistration.DateOfRegistration = Global.getTransactionDate().Date + timeSpan;
            return lRegistration;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            ResetForm();
            TextBoxPatientId.ResetText();
            EnableForm(false);
            LoadOpQueue();
            this.formIsDirty = false;
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                DialogResult Result = MessageBox.Show("Your changes are not saved. Do you want to save?", "Save Changes",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnSave.PerformClick();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            ResetForm();
            TextBoxPatientId.ResetText();
            this.formIsDirty = false;
            EnableForm(false);
            BtnPatientSearch.Select();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                string TockenNo = string.Empty;
                DateTime transactionDate = Global.getTransactionDate();
                Registration lOpRegistration = GetRegistrationFromForm();
                Registration OPRecord = OpManager.Instance.GetOPRecordIfAdmitted((long)lOpRegistration.PatientId!);
                if (OPRecord == null)
                {
                    Registration? OpRegistrationFromDB = null;
                    if (lOpRegistration.Id == 0)
                    {
                        Registration lRegistrationFromDB = OpManager.Instance.GetPatientInOpQueue((long)lOpRegistration.PatientId, Global.getTransactionDate());
                        if (lRegistrationFromDB == null)
                        {
                            //TockenNo = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.OP_TOKEN, lOpRegistration.DateOfRegistration);
                            Registration RegInfo = OpManager.Instance.GetPatientLastOpByTransactionDate(long.Parse(TextBoxPatientId.Text), transactionDate.Date, Global.Company.CompanyId);
                            TockenNo = (RegInfo != null && RegInfo.TockenNo != null) ? (int.Parse(RegInfo.TockenNo) + 1).ToString() : "1";
                            if (!string.IsNullOrEmpty(TockenNo))
                            {
                                lOpRegistration.TockenNo = TockenNo;
                                //lOpRegistration.PatientOPNumber = OpManager.FeachPatientOPID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                                lOpRegistration.PatientOPNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.OP_ID, Global.getTransactionDate().Date);
                                OpRegistrationFromDB = OpManager.AddOpRegistration(lOpRegistration);
                                //OpManager.GenerateNextPatientOPID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                                if (OpRegistrationFromDB == null)
                                {
                                    MessageBox.Show(RefNoErrorMsg);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(RefNoErrorMsg);
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format(DoNotAllowToSave_PatientInOpMsg, lRegistrationFromDB.Patient.Name), "Warning");
                            return;
                        }
                    }
                    else
                    {
                        Registration OpRegistrationById = OpManager.GetOpRegistrationById(lOpRegistration.Id);
                        if (OpRegistrationById != null)
                        {
                            OpRegistrationById.RequestedDoctorId = lOpRegistration.RequestedDoctorId;
                            OpRegistrationById.ReasonForTheVisit = lOpRegistration.ReasonForTheVisit;
                            OpRegistrationById.HasRegistrationFeePaid = lOpRegistration.HasRegistrationFeePaid;
                            OpRegistrationById.RegistrationFee = lOpRegistration.RegistrationFee;
                            OpRegistrationById.IsBillToInsurance = lOpRegistration.IsBillToInsurance;
                            OpRegistrationById.InsuranceInfoId = lOpRegistration.InsuranceInfoId;
                            OpRegistrationById.Patient = null;
                            OpRegistrationFromDB = OpManager.UpdateOpRegistrations(OpRegistrationById/*, Address*/);
                        }
                        if (CreateOpRegisterOnLoad)
                        {
                            this.Close();
                        }
                    }
                    if (OpRegistrationFromDB != null)
                    {
                        ResetForm();
                        LoadOpQueue();
                        TextBoxOpRegistrationId.Text = OpRegistrationFromDB.Id.ToString();
                        LabelTokenNumber.Text = OpRegistrationFromDB.TockenNo;
                        OpQueueGrid.SelectedById(OpRegistrationFromDB.Id.ToString());
                        loadCombo();
                        LoadPatientInfo();
                        LoadOpInfo();
                        OpRegistrationErrorMsg.Text = SaveSuccessMsg;
                        BtnSave.Select();
                        this.formIsDirty = false;
                        EnableForm(true);
                    }
                }
                else
                {
                    MessageBox.Show(string.Format(DoNotAllowToSave_PatientInIpMsg, OPRecord.Patient.Name), "Warning");
                    return;
                }
            }
        }
        private Boolean validate()
        {
            HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
            OpRegistrationErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                OpRegistrationErrorMsg.Text = SelectPatientErrorMsg;
                BtnPatientSearch.Select();
                return false;
            }
            Patient Patient = PatientManager.Instance.GetPatientById(long.Parse(TextBoxPatientId.Text));
            if (Patient.IsDeceased)
            {
                MessageBox.Show(string.Format(DoNotAllowToSave_PatientIsDeceasedMsg, Patient.Name), "Warning");
                return false;
            }
            if (CheckBoxBillToInsurance.Checked && ComboBoxSelectInsurance.SelectedIndex == -1)
            {
                OpRegistrationErrorMsg.Text = SelectPatientInsuranceErrorMsg;
                ComboBoxSelectInsurance.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxOpRegistrationReasonForVisit.Text.Trim()))
            {
                OpRegistrationErrorMsg.Text = EnterVisitResonErrorMsg;
                TextBoxOpRegistrationReasonForVisit.Select();
                return false;
            }
            if (HospitalConfiguration != null && string.IsNullOrEmpty(TextBoxOpRegistrationId.Text) && FeeGroupBox1.Visible && !CheckBoxOpRegistrationNoFeeReceived.Checked && !string.IsNullOrEmpty(TextBoxOpRegistrationAmountReceived.Text) && double.Parse(TextBoxOpRegistrationAmountReceived.Text) > HospitalConfiguration.DefaultOPConsultingFee)
            {
                OpRegistrationErrorMsg.Text = string.Format(EnterFeeErrorMsg, HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat));
                TextBoxOpRegistrationAmountReceived.Select();
                return false;
            }
            return true;
        }
        private void BtnPatientSearch_Click(object sender, EventArgs e)
        {
            FormPatientSearch FormPatientSearch = null;
            string patientId = TextBoxPatientId.Text;
            TextBoxPatientId.Text = string.Empty;
            if (FormPatientSearch == null || FormPatientSearch.IsDisposed)
            {
                FormPatientSearch = new FormPatientSearch(this);
            }
            FormPatientSearch.ShowDialog(this);
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                PatientSelectionChange();
                BtnPatientSearch.Select();
                AllowEntry();
            }
            else
            {
                TextBoxPatientId.Text = patientId;
            }
        }

        private void BtnNewPatient_Click(object sender, EventArgs e)
        {
            PatientRegistration FormPatient = null;
            string patientId = TextBoxPatientId.Text;
            TextBoxPatientId.Text = string.Empty;
            if (FormPatient == null || FormPatient.IsDisposed)
            {
                FormPatient = new PatientRegistration(this);
            }
            FormPatient.CreatePatientOnLoad = true;
            FormPatient.ShowDialog(this);
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                PatientSelectionChange();
                BtnNewPatient.Select();
                AllowEntry();
            }
            else
            {
                TextBoxPatientId.Text = patientId;
            }
        }
        private void PatientSelectionChange()
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                ResetForm();
                loadCombo();
                LoadPatientInfo();
                LoadOpQueue();
                ComboBoxSwapTextBoxOpRegistrationConsultant.Select();
            }
        }
        public FormIPRegistration FormIPRegistration = null;
        private void BtnSwitchToIP_Click(object sender, EventArgs e)
        {
            if (FormIPRegistration == null || FormIPRegistration.IsDisposed)
            {
                FormIPRegistration = new FormIPRegistration(this);
            }
            if (!string.IsNullOrEmpty(TextBoxOpRegistrationId.Text))
            {
                FormIPRegistration.OpId = long.Parse(TextBoxOpRegistrationId.Text);
                this.PatientIdTransport.Text = TextBoxPatientId.Text;
                FormIPRegistration.ShowDialog(this);
                ResetForm();
                LoadOpQueue();
                TextBoxPatientId.ResetText();
                this.formIsDirty = false;
                EnableForm(false);
                BtnPatientSearch.Select();
            }
            else
            {
                if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
                {
                    OpRegistrationErrorMsg.Text = string.Format(PatientNotRegisteredErrorMsg, PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text)));
                }
                else
                {
                    OpRegistrationErrorMsg.Text = SelectPatientErrorMsg;
                }
            }
        }
        protected override void PatientIdTransportReload(object sender, EventArgs e)
        {
            TextBoxPatientId.Text = PatientIdTransport.Text;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                LoadPatientInfo();
            }
        }
        private void ComboBoxSwapTextBoxConsultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex != -1)
            {
                ComboBoxSwapTextBoxOpRegistrationDepartment.SelectedIndex = ComboBoxSwapTextBoxOpRegistrationDepartment.FindStringExact(((Employee)ComboBoxSwapTextBoxOpRegistrationConsultant.Items[ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex]).Department.Name);
            }
            else
            {
                ComboBoxSwapTextBoxOpRegistrationDepartment.SelectedIndex = -1;
            }
        }
        private void ResetForm()
        {
            LabelInsurance.Text = "**NO INSURANCE**";
            TextBoxOpRegistrationName.ResetText();
            TextBoxOpRegistrationDOB.ResetText();
            TextBoxOpRegistrationAge.ResetText();
            TextBoxOpRegistrationId.ResetText();
            RbtOpRegistrationGender.Gender = GenderSelection.None;
            TextBoxAddress.ResetText();
            ComboBoxSelectInsurance.SelectedIndex = -1;
            CheckBoxBillToInsurance.Checked = false;
            CheckBoxBillToInsurance.Enabled = false;
            ComboBoxSelectInsurance.Visible = false;
            TextBoxOpRegistrationReasonForVisit.ResetText();
            OpRegistrationErrorMsg.Text = string.Empty;
            ComboBoxSwapTextBoxOpRegistrationConsultant.ResetText();
            ComboBoxSwapTextBoxOpRegistrationDepartment.ResetText();
            ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex = -1;
            ComboBoxSwapTextBoxOpRegistrationDepartment.SelectedIndex = -1;
            PatientNumberOp.PatientNumber = "000000000000";
            LabelTokenNumber.ResetText();
            BtnSwitchToIP.Enabled = false;
            PatientPhoto.Clear();
            ResetReceiveAmount();


        }
        private void ResetReceiveAmount()
        {
            HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
            if (HospitalConfiguration != null)
            {
                if (HospitalConfiguration.OPRegistrationFeeAccountId != null && HospitalConfiguration.DefaultOPConsultingFee > 0)
                {
                    VisibleReceiveAmount(true);
                    LabelOpRegFee.Text = String.Format(RegistrationFeeMessage, HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat), HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyCodeISO));
                    TextBoxOpRegistrationAmountReceived.Text = HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                }
                else
                {
                    FeeGroupBox1.Visible = false;
                }
            }
            else
            {
                FeeGroupBox1.Visible = false;
            }
        }
        private void VisibleReceiveAmount(Boolean enable)
        {
            TextBoxOpRegistrationAmountReceived.ReadOnly = !enable;
            TextBoxOpRegistrationAmountReceived.TabStop = enable;
            CheckBoxOpRegistrationNoFeeReceived.Checked = false;
            CheckBoxOpRegistrationNoFeeReceived.Enabled = enable;
        }
        private void SetOpRegistrationViewFromPatient()
        {
            BtnNew.Visible = false;
            BtnDelete.Visible = false;
            BtnPatientSearch.Visible = false;
            BtnNewPatient.Visible = false;
            BtnSwitchToIP.Visible = false;
            OpQueueGrid.Visible = false;
            toolStripOpRegistration.Visible = false;


        }
        private void AllowEntry()
        {
            ComboBoxSwapTextBoxOpRegistrationConsultant.Visible = true;
            TextBoxOpRegistrationReasonForVisit.ReadOnly = false;
        }
        private void EnableForm(Boolean enable)
        {

            FeeGroupBox1.Enabled = true;
            ComboBoxSwapTextBoxOpRegistrationConsultant.Visible = true;
            TextBoxOpRegistrationReasonForVisit.ReadOnly = !enable;
            TextBoxOpRegistrationReasonForVisit.TabStop = enable;
            ComboBoxSwapTextBoxOpRegistrationConsultant.Visible = enable;
            BtnPatientSearch.Enabled = true;
            BtnNewPatient.Enabled = true;
            BtnSave.Enabled = true;
            BtnCancel.Enabled = true;
            BtnNew.Enabled = enable;
            BtnDelete.Enabled = enable;
            btnprintOpRegistration.Enabled = enable;
            BtnSwitchToIP.Enabled = enable;
            btnprintOpRegistration.Enabled = enable;
            LinkLabelChangePatientInfo.Enabled = string.IsNullOrEmpty(TextBoxPatientId.Text) ? false : true;
            TextBoxOpRegistrationAmountReceived.ReadOnly = enable;
            TextBoxOpRegistrationAmountReceived.TabStop = !enable;
        }
        private void ComboBoxSwapTextBoxOpRegistrationConsultant_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxOpRegistrationConsultant.DroppedDown = false;
        }
        private void GridViewOpRegistrationToken_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void OpQueueGrid_DoubleClick(object sender, EventArgs e)
        {
            if (OpQueueGrid.OpId != null)
            {
                if (!formIsDirty)
                {
                    LoadOpInfo();
                }
                else
                {
                    DialogResult Result = MessageBox.Show("Do you want to save the current changes?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (validate())
                        {
                            long? temp = OpQueueGrid.OpId;
                            BtnSave_Click(sender, e);
                            OpQueueGrid.OpId = temp;
                            LoadOpInfo();
                        }
                    }
                    else
                    {
                        LoadOpInfo();
                    }
                }
                GroupBoxOutpatientRegister.Focus();
                BtnPatientSearch.Focus();
            }
        }
        public void LoadOpInfo()
        {
            ResetForm();
            loadCombo();
            if (OpQueueGrid.OpId != null)
            {
                Registration OpRegistration = OpManager.GetOpRegistrationById((long)OpQueueGrid.OpId);
                if (OpRegistration != null)
                {
                    TextBoxPatientId.Text = OpRegistration.PatientId.ToString();
                    LoadPatientInfo();
                    TextBoxOpRegistrationId.Text = OpRegistration.Id.ToString();
                    LoadInactiveInsurance();
                    if (OpRegistration.RequestedDoctorId != null)
                    {
                        Employee Employee = EmployeeManager.Instance.GetEmployeeInfoById((long)OpRegistration.RequestedDoctorId);
                        if (Employee != null)
                        {
                            ComboBoxSwapTextBoxOpRegistrationConsultant.SelectedIndex = ComboBoxSwapTextBoxOpRegistrationConsultant.FindStringExact(Employee.Name);
                        }
                    }
                    CheckBoxBillToInsurance.Checked = OpRegistration.IsBillToInsurance;
                    if (OpRegistration.InsuranceInfoId != null)
                    {
                        InsuranceInfo InsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)OpRegistration.InsuranceInfoId, true);
                        if (InsuranceInfo != null)
                        {
                            ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(InsuranceInfo.InsuranceName);
                        }
                    }
                    TextBoxOpRegistrationReasonForVisit.Text = OpRegistration.ReasonForTheVisit;
                    LabelTokenNumber.Text = String.Format("{0:000}", OpRegistration.TockenNo);
                    CheckBoxOpRegistrationNoFeeReceived.Checked = !OpRegistration.HasRegistrationFeePaid;
                    TextBoxOpRegistrationAmountReceived.Text = OpRegistration.RegistrationFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    EnableForm(true);
                    AllowEntry();

                    if (OpRegistration.Status == Status.COMPLETED)
                    {
                        EnableChangeInOPComplete(false);
                    }

                }

            }
            this.formIsDirty = false;
        }
        private void EnableChangeInOPComplete(bool Enable)
        {
            BtnNew.Enabled = !Enable;
            BtnDelete.Enabled = Enable;
            btnprintOpRegistration.Enabled = Enable;
            BtnSwitchToIP.Enabled = Enable;
            LinkLabelChangePatientInfo.Enabled = Enable;
            BtnSave.Enabled = Enable;
            BtnCancel.Enabled = Enable;
            ComboBoxSwapTextBoxOpRegistrationConsultant.Visible = Enable;
            TextBoxOpRegistrationReasonForVisit.ReadOnly = !Enable;
            TextBoxOpRegistrationReasonForVisit.TabStop = Enable;
            BtnPatientSearch.Enabled = Enable;
            BtnNewPatient.Enabled = Enable;
            FeeGroupBox1.Enabled = Enable;

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                OpQueueGrid.OpId = null;
                return true;
            }
            if (keyData == (Keys.F3))
            {
                if (BtnNew.Enabled)
                {
                    BtnNew.PerformClick();
                    return true;
                }
                else
                {
                    BtnNewPatient.PerformClick();
                }
            }
            else if (keyData == (Keys.F2))
            {
                BtnPatientSearch.PerformClick();
            }
            else if (keyData == (Keys.F6))
            {
                BtnSwitchToIP.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F4))
            {
                BtnDelete.PerformClick();
            }
            else if (keyData == (Keys.F9))
            {
                btnprintOpRegistration.PerformClick();
            }
            if (keyData == Keys.Tab && ActiveControl == ComboBoxSwapTextBoxOpRegistrationConsultant)
            {
                TextBoxOpRegistrationReasonForVisit.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == TextBoxOpRegistrationAmountReceived && !TextBoxOpRegistrationReasonForVisit.ReadOnly)
            {
                TextBoxOpRegistrationReasonForVisit.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ComboBoxOpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnOpSearch_Click(sender, e);
            if (OpQueueGrid.IsEmpty)
            {
                ResetForm();
                TextBoxPatientId.ResetText();
                this.formIsDirty = false;
                EnableForm(false);
                BtnPatientSearch.Select();
            }
        }
        private void TextBoxOpSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnOpSearch_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                OpQueueGrid.Select();
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnOpSearch.PerformClick();
            }
        }
        private void BtnOpSearch_Click(object sender, EventArgs e)
        {
            LoadOpQueue();
            OpQueueGrid.Select();
        }
        private void FormOPRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show("Your changes are not saved. Do you want to exit?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    formIsDirty = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void BtnPatientSearch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                BtnNewPatient.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Select();
            }
        }
        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (BtnPatientSearch.Visible)
                {
                    BtnPatientSearch.Focus();
                }
                else if (BtnNewPatient.Visible)
                {
                    BtnNewPatient.Focus();
                }
                else if (CheckBoxBillToInsurance.Enabled)
                {
                    CheckBoxBillToInsurance.Focus();
                }
                else
                {
                    ComboBoxSwapTextBoxOpRegistrationConsultant.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (CheckBoxOpRegistrationNoFeeReceived.Visible && CheckBoxOpRegistrationNoFeeReceived.Enabled)
                {
                    CheckBoxOpRegistrationNoFeeReceived.Focus();
                }
                else
                {
                    TextBoxOpRegistrationReasonForVisit.Select();
                }
            }
        }
        private void OpQueueGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                BtnPatientSearch.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxOpSearch.TextBox.Select();
            }
        }
        private void BtnPrintOpRegistration_Click(object sender, EventArgs e)
        {
            FormOPRegistrationPrint FormOPRegistrationPrint = new FormOPRegistrationPrint();
            FormOPRegistrationPrint.FeeReceipt = CheckBoxOpRegistrationNoFeeReceived.Checked;
            FormOPRegistrationPrint.TextBoxRegtId.Text = TextBoxOpRegistrationId.Text;
            FormOPRegistrationPrint.ShowDialog();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            OpRegistrationErrorMsg.Text = string.Empty;
            if (!string.IsNullOrEmpty(TextBoxOpRegistrationId.Text))
            {
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteByOPRegisterId(long.Parse(TextBoxOpRegistrationId.Text));
                if (Note == null)
                {
                    if (MessageBox.Show("Do you want to delete the OP?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        OpManager.Instance.DeleteOpRegister(long.Parse(TextBoxOpRegistrationId.Text));
                        BtnOpSearch_Click(sender, e);
                        ResetForm();
                        OpRegistrationErrorMsg.Text = string.Format(DeleteSuccessMsg, PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text)));
                        TextBoxPatientId.ResetText();
                        this.formIsDirty = false;
                        EnableForm(false);
                        LoadOpQueue();
                        BtnPatientSearch.Select();
                    }
                }
                else
                {
                    OpRegistrationErrorMsg.Text = DoNotAllowToDeleteMsg;
                }
            }
            else
            {
                OpRegistrationErrorMsg.Text = string.Format(PatientNotRegisteredErrorMsg, PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text)));
                BtnSave.Select();
            }
        }
        private void OpQueueGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadOpInfo();
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (CheckBoxBillToInsurance.Enabled == true)
                {
                    CheckBoxBillToInsurance.Select();
                }
                else
                {
                    ComboBoxSwapTextBoxOpRegistrationConsultant.Focus();
                }
            }
        }
        private void OpQueueGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadOpInfo();
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (CheckBoxBillToInsurance.Enabled == true)
                {
                    CheckBoxBillToInsurance.Select();
                }
                else
                {
                    ComboBoxSwapTextBoxOpRegistrationConsultant.Focus();
                }
            }
        }
        private void TextBoxOpRegistrationName_Click(object sender, EventArgs e)
        {
            FrmTxtFocus(TextBoxOpRegistrationName.Text, TextBoxOpRegistrationName);
        }
        private void TextBoxOpRegistrationDOB_Click(object sender, EventArgs e)
        {
            FrmTxtFocus(TextBoxOpRegistrationDOB.Text, TextBoxOpRegistrationDOB);
        }
        private void TextBoxOpRegistrationAge_Click(object sender, EventArgs e)
        {
            FrmTxtFocus(TextBoxOpRegistrationAge.Text, TextBoxOpRegistrationAge);
        }
        private void TextBoxOpRegistrationReasonForVisit_Click(object sender, EventArgs e)
        {
            FrmTxtFocus(TextBoxOpRegistrationReasonForVisit.Text, TextBoxOpRegistrationReasonForVisit);
        }
        private void CheckBoxOpRegistrationNoFeeReceived_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxOpRegistrationAmountReceived.Text = "0.00";
            if (CheckBoxOpRegistrationNoFeeReceived.Checked)
            {
                TextBoxOpRegistrationAmountReceived.ReadOnly = true;
                TextBoxOpRegistrationAmountReceived.TabStop = false;
            }
            else
            {
                if (string.IsNullOrEmpty(TextBoxOpRegistrationId.Text))
                {
                    TextBoxOpRegistrationAmountReceived.ReadOnly = false;
                    TextBoxOpRegistrationAmountReceived.TabStop = true;
                    HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
                    if (HospitalConfiguration != null)
                    {
                        TextBoxOpRegistrationAmountReceived.Text = HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                }
                else
                {
                    Registration OPReg = OpManager.Instance.GetRegisterByRegId(long.Parse(TextBoxOpRegistrationId.Text));
                    if (OPReg != null)
                    {
                        if (!OPReg.HasRegistrationFeePaid)
                        {
                            PatientLedger Ledger = PatientLedgerManager.Instance.GetPatientByOpIdType(OPReg.Id, TransactionType.REGISTRATION_FEE);
                            if (Ledger != null)
                            {
                                TextBoxOpRegistrationAmountReceived.Text = Ledger.Amount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                            else
                            {
                                TextBoxOpRegistrationAmountReceived.Text = OPReg.RegistrationFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                            }
                        }
                        else
                        {
                            TextBoxOpRegistrationAmountReceived.Text = OPReg.RegistrationFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                        }
                    }
                }
            }
        }

        private void LinkLabelChangePatientInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                long Id = !string.IsNullOrEmpty(TextBoxOpRegistrationId.Text) ? long.Parse(TextBoxOpRegistrationId.Text) : 0L;
                PatientRegistration PatientRegistration = new PatientRegistration(this);
                PatientRegistration.CreatePatientOnLoad = true;
                PatientRegistration.PatientId = long.Parse(TextBoxPatientId.Text);
                PatientRegistration.ShowDialog();
                if (Id != 0L)
                {
                    LoadOpQueue();
                    LoadPatientInfo();
                    OpQueueGrid.SelectedById(Id.ToString());
                }
                else
                {
                    LoadPatientInfo();
                }
                if (CheckBoxBillToInsurance.Enabled == true)
                {
                    CheckBoxBillToInsurance.Select();
                }
                else
                {
                    ComboBoxSwapTextBoxOpRegistrationConsultant.Focus();
                }
            }
        }
        private void CheckBoxBillToInsurance_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxBillToInsurance.Checked)
            {
                LabelSelectInsurance.Font = new Font("Tahoma", 8, FontStyle.Bold);
                ComboBoxSelectInsurance.Visible = true;
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllOPActiveInsuranceInfoByPatientId(long.Parse(TextBoxPatientId.Text));
                if (InsuranceInfo != null && InsuranceInfo.Count > 0)
                {
                    if (InsuranceInfo.Where(x => x.IsPrimary && x.OPInsuranceCoverage == true).ToList().Count > 0)
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
        private void LoadInactiveInsurance()
        {
            if (!string.IsNullOrEmpty(TextBoxOpRegistrationId.Text))
            {
                Registration OpRegistration = OpManager.GetOpRegistrationById(long.Parse(TextBoxOpRegistrationId.Text));
                if (OpRegistration.IsBillToInsurance)
                {
                    InsuranceInfo lInsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)OpRegistration.InsuranceInfoId, false);
                    int Index = ComboBoxSelectInsurance.FindStringExact(lInsuranceInfo.InsuranceName);
                    if (Index == -1)
                    {
                        LabelInsurance.Text = "**INSURANCE**";
                        ComboBoxSelectInsurance.Items.Add(lInsuranceInfo);
                    }
                }
            }
        }
    }
}
