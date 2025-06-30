using fa.api.Hms;
using fa.model.Hms.Master;
using System;
using fa.model.Accounting.Masters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.api.System;

namespace fa.views.hms.patient
{
    public partial class FormPatientIdEdit : FormPatientBase
    {
        public string? PatientNumber;
        public bool PatientNumberUpdated;
        FormPatientBase parent = null!;
        public string PatientNo
        {
            get { return TextBoxPatientId.Text; }
            set { TextBoxPatientId.Text = value; }
        }

        public FormPatientIdEdit(object sender)
        {
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
            else
                parent = null!;
            InitializeComponent();
        }

        private void CancelButnPatientId_Click(object sender, EventArgs e)
        {
            TextBoxPatientId.Select();
            TextBoxPatientId.Text = PatientNumber;
            PatientNumberUpdated = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                SaveButnPatientId.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                CancelButnPatientId.PerformClick();
            }            
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void SaveButnPatientId_Click(object sender, EventArgs e)
        {
            Patient lPatient = new Patient();
            Patient PatientFromDB = null!;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                lPatient.PatientNumber = TextBoxPatientId.Text;
            }
            if (PatientNo != PatientNumber)
            {
                Patient PatientByNo = PatientManager.Instance.GetPatientByNo(PatientNumber);
                if (PatientByNo != null)
                {
                    Patient CheckPatientByNo = PatientManager.Instance.GetPatientByNo(PatientNo);
                    if (CheckPatientByNo == null)
                    {
                        PatientFromDB = PatientManager.Instance.UpdatePatientNo(PatientNo, PatientByNo.Id);
                        Patient LastPatient = PatientManager.Instance.GetLastPatient();
                        if (LastPatient.Name == PatientByNo.Name)
                        {
                            IdGenerator.IdSpaceCompanyGetPreviousRunningSeed(Global.Company, EntryType.PATIENT_ID, Global.getTransactionDate().Date);
                        }
                        PatientNo = TextBoxPatientId.Text;
                        PatientNumberUpdated = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This Patient Number is already Existing!", "Try Another", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        TextBoxPatientId.Select();
                        TextBoxPatientId.Text = PatientNo;
                        PatientNumberUpdated = false;
                    }
                }
            }
            else
            {
                TextBoxPatientId.Select();
                TextBoxPatientId.Text = PatientNo;
                PatientNumberUpdated = false;
                this.Close();
            }
        }
    }
}
