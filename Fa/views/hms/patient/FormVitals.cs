using System;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using fa.views.controls.hms;
using fa.views.utils;
using DocumentFormat.OpenXml.Drawing.Charts;
namespace fa.views.hms.patient
{
    public partial class FormVitals : Form
    {
        long PatientId = 0L;
        public long? OpId = null;
        FormPatientBase parent = null;
        public string SaveSuccessMsg = "Save Success";
        public static string EnterValidValueErrorMsg = "Please check the {0} , {1} should not exceed above {2}";
        public FormVitals(object sender)
        {
            InitializeComponent();
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
        }
        private void FormVitals_Load(object sender, EventArgs e)
        {
            PatientId = long.Parse(parent.PatientIdTransport.Text);
            VitalsPatientInfoMin.PatientId = PatientId;
            PatientVitalHistory.PatientId = PatientId;
            VitalVitalEntry.PatientId = PatientId;
            GraphChartControlVital.PatientId = PatientId;
            toolStripVitalsErrMsg.Text = "";
        }
        private void BtnVitalSave_Click(object sender, EventArgs e)
        {
            if (VitalVitalEntry.VitalEntryValidationResult())
            {
                Vital vital = VitalVitalEntry.GetVitalDetails();
                if (vital.Height > 210)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "height", "maximum height", "210 cm");
                    VitalVitalEntry.VitalEntryHeight();
                }
                else if (vital.Weight > 300)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "weight", "maximum weight", "300 kg");
                    VitalVitalEntry.VitalEntryWeight();
                }
                else if (vital.Temperature > 110)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "temperature", "maximum temperature", "110 C");
                    VitalVitalEntry.VitalEntryTemparature();
                }
                else if (vital.Pulse > 200)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "pulse rate", "maximum pulse rate", "200");
                    VitalVitalEntry.VitalEntryPulse();
                }
                else if (vital.RespRate > 100)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "respiratoryRate", "maximum respiratoryRate", "100");
                    VitalVitalEntry.VitalEntryRespiratoryRate();
                }
                else if (vital.BPressure > 250)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "blood Pressure level", "maximum blood Pressure level", "250");
                    VitalVitalEntry.VitalEntryBloodPressure();
                }
                else if (vital.BPressureOver > 250)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "blood Pressure level", "maximum blood Pressure level", "250");
                    VitalVitalEntry.VitalBloodPressureOver();
                }
                else if (vital.BOxyLevel > 100)
                {
                    toolStripVitalsErrMsg.Text = string.Format(EnterValidValueErrorMsg, "blood oxygen level", "maximum blood oxygen level", "100");
                    VitalVitalEntry.VitalOxigenLevel();
                }
                else
                {
                    vital.OpRegistrationId = OpId;
                    if (vital.Id == 0L)
                    {
                        VitalEntryManager.Instance.AddVital(vital);
                    }
                    else
                    {
                        VitalEntryManager.Instance.UpdateVital(vital);
                    }
                    VitalVitalEntry.Clear();
                    PatientVitalHistory.PatientId = PatientId;
                    toolStripVitalsErrMsg.Text = SaveSuccessMsg;
                }
            }
            else
            {
                VitalVitalEntry.Focus();
                toolStripVitalsErrMsg.Text = VitalVitalEntry.ErrorMsg();
            }
            GraphChartControlVital.PatientId = PatientId;
        }
        private void BtnVitalReset_Click(object sender, EventArgs e)
        {
            VitalVitalEntry.Clear();
            VitalVitalEntry.Select();
            toolStripVitalsErrMsg.Text = "";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnVitalReset.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnVitalSave.PerformClick();
            }
            toolStripVitalsErrMsg.Text = VitalVitalEntry.ErrorMsg();

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void PatientVitalHistory_Enter(object sender, EventArgs e)
        {
            toolStripVitalsErrMsg.Text = string.Empty;
        }
                

        private void PatientVitalHistory_Click(object sender, EventArgs e)
        {
            if (PatientVitalHistory.VitalsId != 0L)
            {
                VitalVitalEntry.VitalId = (long)PatientVitalHistory.VitalsId!;
            }
        }
    }
}
