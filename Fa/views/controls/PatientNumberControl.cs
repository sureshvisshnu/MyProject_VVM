using System;
using System.Windows.Forms;
using fa.api.utils;
using fa.views.hms.patient;

namespace fa.views.controls
{
    public partial class PatientNumberControl : UserControl
    {
        private long? _PatientId;
        string _patientNumber=string.Empty;
        public string PatientNumber {
            get
            {
                return _patientNumber;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    _patientNumber = value;
                    PrintPatientNumber();

                }
            }
        }

        public PatientNumberControl()
        {
            InitializeComponent();           
        }

        private void PrintPatientNumber()
        {
            if (!string.IsNullOrEmpty(PatientNumber))
            {
                pictureBoxPatientNumber.BackgroundImage = BarCode.GenerateImageBarcode(PatientNumber);
                LabelValue.Text = PatientNumber;
            }
            else
            {
                pictureBoxPatientNumber.ResetText();
                LabelValue.Text = string.Empty;
            }
        }
    }
}
