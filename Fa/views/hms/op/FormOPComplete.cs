using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using fa.model.Hms.Op;
using fa.model.Hms.Master;
using fa.libraries.utils;
using fa.common;

namespace fa.views.hms.op
{
    public partial class FormOPComplete : FormPatientBase
    {
        public static string OpVisitCompleteMsg = "Already visit was completed";
        public static string SaveSuccessMsg = "Saved success.";
        public long PatientId = 0L;
        public long? OpId = null;
        FormPatientBase parent = null;
        bool visitconfirm = false;
        public FormOPComplete(object sender)
        {
            InitializeComponent();
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
        }
        private void FormOPComplete_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.CenterToParent();
            LoadVisitInfo();
            if (validateVisit())
            {
                BtnCompleteVisit.Enabled = true;
            }
            else
            {
                BtnCompleteVisit.Enabled = false;
            }
        }
        private void LoadVisitInfo()
        {
            Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpId);
            if (OpRegistration != null)
            {
                ComboUtils.InitializePatientInsurerCombo(ComboBoxSelectInsurance, PatientId, "OP");
                PatientInfoMiniHorizontal.Type = PatientTypes.OutPatient;
                PatientInfoMiniHorizontal.PatientId = OpRegistration.PatientId;
                CheckBoxNurseStationActivities.Checked = OpRegistration.IsNurseActivitiesCompleted;
                CheckBoxTechnicianStationActivities.Checked = OpRegistration.IsTechnicianActivitiesCompleted;
                CheckBoxFeesPaid.Checked = OpRegistration.IsFeePaid;
                if(OpRegistration.IsBillToInsurance)
                {
                    CheckBoxBillToInsurance.Enabled = true;
                    CheckBoxBillToInsurance.Checked = OpRegistration.IsBillToInsurance;
                    InsuranceInfo InsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)OpRegistration.InsuranceInfoId, true);
                    if (InsuranceInfo != null)
                    {
                        ComboBoxSelectInsurance.Visible = true;
                        ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(InsuranceInfo.InsuranceName);
                        BtnCompleteVisit.Enabled = true;
                    }
                }
                else
                {
                    IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllOPActiveInsuranceInfoByPatientId((long)OpRegistration.PatientId);
                    if (InsuranceInfo != null && InsuranceInfo.Count > 0 && InsuranceInfo.Where(x =>x.OPInsuranceCoverage == true).ToList().Count > 0)
                    {
                        CheckBoxBillToInsurance.Enabled = true;
                    }
                }
            }
        }

        private void CheckBoxNurseStationActivities_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxNurseStationActivities.Checked && CheckBoxTechnicianStationActivities.Checked && CheckBoxFeesPaid.Checked && CheckBoxBillToInsurance.Checked)
            {
                if(visitconfirm)
                {
                    BtnCompleteVisit.Enabled = false;
                }
                else
                {
                    BtnCompleteVisit.Enabled = true;
                }
            }
            else if (!CheckBoxNurseStationActivities.Checked && !CheckBoxTechnicianStationActivities.Checked && !CheckBoxFeesPaid.Checked && !CheckBoxBillToInsurance.Checked)
            {
                BtnCompleteVisit.Enabled = false;
            }
            else
            {
                BtnCompleteVisit.Enabled = true;
            }
        }

        private void CheckBoxTechnicianStationActivities_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxNurseStationActivities.Checked && CheckBoxTechnicianStationActivities.Checked && CheckBoxFeesPaid.Checked && CheckBoxBillToInsurance.Checked)
            {
                if (visitconfirm)
                {
                    BtnCompleteVisit.Enabled = false;
                }
                else
                {
                    BtnCompleteVisit.Enabled = true;
                }
            }
            else if (!CheckBoxNurseStationActivities.Checked && !CheckBoxTechnicianStationActivities.Checked && !CheckBoxFeesPaid.Checked && !CheckBoxBillToInsurance.Checked)
            {
                BtnCompleteVisit.Enabled = false;
            }
            else
            {
                BtnCompleteVisit.Enabled = true;
            }
        }

        private void CheckBoxFeesPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxFeesPaid.Checked || CheckBoxBillToInsurance.Checked)
            {
                BtnCompleteVisit.Enabled = true;
            }
            else if (!CheckBoxNurseStationActivities.Checked && !CheckBoxTechnicianStationActivities.Checked)
            {
                BtnCompleteVisit.Enabled = false;
            }
            else
            {
                BtnCompleteVisit.Enabled = true;
            }
        }

        private Registration GetRegistrationVisitFromForm()
        {
            Registration lRegistration = new Registration();
            lRegistration.Status = Status.OPEN;
            lRegistration.Id = (long)OpId;
            lRegistration.PatientId = PatientId; 
            lRegistration.CompanyId = Global.Company.CompanyId;
            lRegistration.IsNurseActivitiesCompleted = CheckBoxNurseStationActivities.Checked;
            lRegistration.IsTechnicianActivitiesCompleted = CheckBoxTechnicianStationActivities.Checked;
            lRegistration.IsFeePaid = CheckBoxFeesPaid.Checked;
            lRegistration.IsBillToInsurance = CheckBoxBillToInsurance.Checked;
            if(ComboBoxSelectInsurance.SelectedIndex>-1)
            {
                lRegistration.InsuranceInfoId = ((InsuranceInfo)ComboBoxSelectInsurance.Items[ComboBoxSelectInsurance.SelectedIndex]).Id;
            }
            return lRegistration;
        }

        private void BtnCompleteVisit_Click(object sender, EventArgs e)
        {           
            Registration lOpRegistration = GetRegistrationVisitFromForm();
            lOpRegistration.Status = Status.COMPLETED;
            Registration OpRegistrationFromDB = null; 
            Registration OpRegistrationById = OpManager.Instance.GetOpRegistrationById(lOpRegistration.Id);
            if (OpRegistrationById != null)
            {
                lOpRegistration.TockenNo = OpRegistrationById.TockenNo;
                lOpRegistration.RequestedDoctorId = OpRegistrationById.RequestedDoctorId;
                lOpRegistration.DateOfRegistration = OpRegistrationById.DateOfRegistration;
                lOpRegistration.HasConsulted = OpRegistrationById.HasConsulted;
                lOpRegistration.HasRegistrationFeePaid = OpRegistrationById.HasRegistrationFeePaid;
                lOpRegistration.ReasonForTheVisit = OpRegistrationById.ReasonForTheVisit;
                lOpRegistration.RegistrationFee = OpRegistrationById.RegistrationFee;
                OpRegistrationFromDB = OpManager.Instance.UpdateOpRegistration(lOpRegistration);
                toolStripStatusVisitComplete.Text = SaveSuccessMsg;
                this.Close();
            }
        }

   
        private Boolean validateVisit()
        {
            if ((CheckBoxNurseStationActivities.Checked || CheckBoxTechnicianStationActivities.Checked) ||(CheckBoxFeesPaid.Checked || CheckBoxBillToInsurance.Checked))
            {
                return true;
            }
            else
            {
                return false;              
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            LoadVisitInfo();
            if (validateVisit())
            {
                BtnCompleteVisit.Enabled = true;
            }
            else
            {
                BtnCompleteVisit.Enabled = false;
            }
        }

        private void CheckBoxFeesPaid_Click(object sender, EventArgs e)
        {
            if(CheckBoxFeesPaid.Checked)
            {
                CheckBoxBillToInsurance.Checked = false;
            }
            else if(OpId!=null)
            {
                Registration OpRegistration = OpManager.Instance.GetOpRegistrationById((long)OpId);
                if (OpRegistration != null)
                {
                    if (OpRegistration.IsBillToInsurance)
                    {
                        CheckBoxBillToInsurance.Enabled = true;
                        CheckBoxBillToInsurance.Checked = OpRegistration.IsBillToInsurance;
                        InsuranceInfo InsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)OpRegistration.InsuranceInfoId, true);
                        if (InsuranceInfo != null)
                        {
                            ComboBoxSelectInsurance.Visible = true;
                            ComboBoxSelectInsurance.SelectedIndex = ComboBoxSelectInsurance.FindStringExact(InsuranceInfo.InsuranceName);
                            BtnCompleteVisit.Enabled = true;
                        }
                    }
                    else
                    {
                        IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllOPActiveInsuranceInfoByPatientId((long)OpRegistration.PatientId);
                        if (InsuranceInfo != null && InsuranceInfo.Count > 0 && InsuranceInfo.Where(x => x.OPInsuranceCoverage == true).ToList().Count > 0)
                        {
                            CheckBoxBillToInsurance.Enabled = true;
                            CheckBoxBillToInsurance.Checked = true;
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
                }
            }
        }

        private void CheckBoxBillToInsurance_Click(object sender, EventArgs e)
        {
            if (CheckBoxBillToInsurance.Checked)
            {
                CheckBoxFeesPaid.Checked = false;
            }
            else
            {
                CheckBoxFeesPaid.Checked = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnCompleteVisit.PerformClick();
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
                ComboBoxSelectInsurance.Visible = true;
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.Instance.ListAllOPActiveInsuranceInfoByPatientId(PatientId);
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
                ComboBoxSelectInsurance.SelectedIndex = -1;
                ComboBoxSelectInsurance.Visible = false;
            }
        }
    }
}
