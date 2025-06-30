using System;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using fa.api.utils;
using System.Text.RegularExpressions;

namespace fa.views.controls.hms
{
    public partial class PatientVitalEntry : UserControl
    {
        protected static int MIN_WIDTH = 173;
        protected static int MIN_HEIGHT = 514;
        protected static int WIDTH_DIFF = 10;
        public bool PatientVitalEntryLastFocus { get; private set; } = false;

        public PatientVitalEntry()
        {
            InitializeComponent();
        }
        public String ErrorMsg()
        {
            return ErrorMsgs;
        }
        public int FocusIndex { get; set; }

        private void PatientVitalEntry_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void GroupBoxVitalEntry_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            GroupBoxVitalEntry.Width = this.Width - WIDTH_DIFF;
            GroupBoxVitalEntry.Height = (this.Height - WIDTH_DIFF) - 15;
        }
        public void Clear()
        {
            VitalId = 0L;
            TextBoxVitalEntryBloodPressure.ResetText();
            TextBoxVitalEntryBloodPressureOver.ResetText();
            TextBoxVitalEntryBMI.ResetText();
            TextBoxVitalEntryHeight.ResetText();
            TextBoxVitalEntryOxigenLevel.ResetText();
            TextBoxVitalEntryPulse.ResetText();
            TextBoxVitalEntryRespiratoryRate.ResetText();
            TextBoxVitalEntryTemperature.ResetText();
            TextBoxVitalEntryWeight.ResetText();
            TextBoxVitalEntryHeight.Select();
        }
        private long _VitalId = 0L;
        public long VitalId
        {
            get
            {
                return _VitalId;
            }
            set
            {
                _VitalId = value;
                LoadVitalDetails();
            }
        }
        private long? _PatientId;
        public long? PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
            }
        }
        string ErrorMsgs;
       
        public void VitalEntryWeight()
        {
            TextBoxVitalEntryWeight.Select();
        }
        public void VitalEntryHeight()
        {
            TextBoxVitalEntryHeight.Select();
            TextBoxVitalEntryHeight.Focus();
        }
        public void VitalEntryTemparature()
        {
            TextBoxVitalEntryTemperature.Select();
        }
        public void VitalEntryPulse()
        {
            TextBoxVitalEntryPulse.Select();
        }
        public void VitalEntryRespiratoryRate()
        {
            TextBoxVitalEntryRespiratoryRate.Select();
        }
        public void VitalEntryBloodPressure()
        {
            TextBoxVitalEntryBloodPressure.Select();
        }
        public void VitalBloodPressureOver()
        {
            TextBoxVitalEntryBloodPressureOver.Select();
        }
        public void VitalOxigenLevel()
        {
            TextBoxVitalEntryOxigenLevel.Select();
        }
        public Boolean VitalEntryValidationResult()
        {
            ErrorMsgs = string.Empty;
            if (string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text.Trim()) || double.Parse(TextBoxVitalEntryWeight.Text) == 0)
            {
                ErrorMsgs = "Please enter patient weight.. ";
                TextBoxVitalEntryWeight.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text.Trim()) || string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text.Trim()))
            {
                if (string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text.Trim()))
                {
                    ErrorMsgs = "Please enter patient blood pressure.. ";
                    TextBoxVitalEntryBloodPressure.Select();
                    return false;
                }
                else if (string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text.Trim()))
                {
                    ErrorMsgs = "Please enter patient blood pressure.. ";
                    TextBoxVitalEntryBloodPressureOver.Select();
                    return false;
                }
            }
            return true;

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (TextBoxVitalEntryHeight.Focused)
                {
                    TextBoxVitalEntryWeight.Focus();
                    return true;
                }
                if (TextBoxVitalEntryWeight.Focused)
                {
                    TextBoxVitalEntryTemperature.Focus();
                    return true;
                }
                if (TextBoxVitalEntryTemperature.Focused)
                {
                    TextBoxVitalEntryPulse.Focus();
                    return true;
                }
                if (TextBoxVitalEntryPulse.Focused)
                {
                    TextBoxVitalEntryRespiratoryRate.Focus();
                    return true;
                }
                if (TextBoxVitalEntryRespiratoryRate.Focused)
                {
                    TextBoxVitalEntryBloodPressure.Focus();
                    return true;
                }
                if (TextBoxVitalEntryBloodPressure.Focused)
                {
                    TextBoxVitalEntryBloodPressureOver.Focus();
                    return true;
                }
                if (TextBoxVitalEntryBloodPressureOver.Focused)
                {
                    TextBoxVitalEntryOxigenLevel.Focus();
                    return true;
                }
                if (TextBoxVitalEntryOxigenLevel.Focused)
                {
                    PatientVitalEntryLastFocus = true;
                    this.Parent.SelectNextControl(this, true, true, true, true);
                    return true;
                }
            }
            else if (keyData == (Keys.Tab | Keys.Shift))
            {
                if (TextBoxVitalEntryOxigenLevel.Focused)
                {
                    TextBoxVitalEntryBloodPressureOver.Focus();
                    return true;
                }
                if (TextBoxVitalEntryBloodPressureOver.Focused)
                {
                    TextBoxVitalEntryBloodPressure.Focus();
                    return true;
                }
                if (TextBoxVitalEntryBloodPressure.Focused)
                {
                    TextBoxVitalEntryRespiratoryRate.Focus();
                    return true;
                }
                if (TextBoxVitalEntryRespiratoryRate.Focused)
                {
                    TextBoxVitalEntryPulse.Focus();
                    return true;
                }
                if (TextBoxVitalEntryPulse.Focused)
                {
                    TextBoxVitalEntryTemperature.Focus();
                    return true;
                }
                if (TextBoxVitalEntryTemperature.Focused)
                {
                    TextBoxVitalEntryWeight.Focus();
                    return true;
                }
                if (TextBoxVitalEntryWeight.Focused)
                {
                    TextBoxVitalEntryHeight.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        /*
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Msg = string.Empty;
            if (keyData == (Keys.Tab))
            {
                if (!string.IsNullOrEmpty(TextBoxVitalEntryHeight.Text))
                {
                    int height;

                    if (int.TryParse(TextBoxVitalEntryHeight.Text, out height))
                    {
                        if (height > 210)
                        {
                            Msg = " Please check the height, maximum height should not exceed above 210 cm.";
                            TextBoxVitalEntryHeight.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text))
                {
                    decimal weight;
                    if (decimal.TryParse(TextBoxVitalEntryWeight.Text, out weight))
                    {
                        if (weight > 300)
                        {
                            Msg = " Please check the weight, maximum weight should not exceed above 300kg.";
                            TextBoxVitalEntryWeight.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (TextBoxVitalEntryTemperature.Text != "" && TextBoxVitalEntryTemperature.Text != "0.00")
                {
                    double Temperature;
                    if (double.TryParse(TextBoxVitalEntryTemperature.Text, out Temperature))
                    {
                        if (Temperature > 110)
                        {
                            Msg = ("Please check the temperature, maximum temperature should not exceed above 110 C.");
                            TextBoxVitalEntryTemperature.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryPulse.Text))
                {
                    int Pulse;
                    if (int.TryParse(TextBoxVitalEntryPulse.Text, out Pulse))
                    {
                        if (Pulse > 200)
                        {
                            Msg = ("Please check the pulse rate, maximum pulse rate should not exceed above 200.");
                            TextBoxVitalEntryPulse.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryRespiratoryRate.Text))
                {
                    int RespiratoryRate;
                    if (int.TryParse(TextBoxVitalEntryRespiratoryRate.Text, out RespiratoryRate))
                    {
                        if (RespiratoryRate > 100)
                        {
                            Msg = ("Please check the respiratoryRate, maximum respiratoryrate should not exceed above 100.");
                            TextBoxVitalEntryRespiratoryRate.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text))
                {
                    int BloodPressure;
                    if (int.TryParse(TextBoxVitalEntryBloodPressure.Text, out BloodPressure))
                    {
                        if (BloodPressure > 250)
                        {
                            Msg = ("Please check the blood Pressure level, maximum blood pressure should not exceed above 250.");
                            TextBoxVitalEntryBloodPressure.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text))
                {
                    int BloodPressureOver;
                    if (int.TryParse(TextBoxVitalEntryBloodPressureOver.Text, out BloodPressureOver))
                    {
                        if (BloodPressureOver > 250)
                        {
                            Msg = "Please check the blood pressure, maximum blood pressure should not exceed above 250.";
                            TextBoxVitalEntryBloodPressureOver.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text))
                {
                    int BloodPressureOver;
                    if (int.TryParse(TextBoxVitalEntryBloodPressureOver.Text, out BloodPressureOver))
                    {
                        if (BloodPressureOver > 250)
                        {
                            Msg = "Please check the blood pressure, maximum blood pressure should not exceed above 250.";
                            TextBoxVitalEntryBloodPressureOver.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(TextBoxVitalEntryOxigenLevel.Text))
                {
                    int BloodOxygen;
                    if (int.TryParse(TextBoxVitalEntryOxigenLevel.Text, out BloodOxygen))
                    {
                        if (BloodOxygen > 100)
                        {
                            Msg = ("Please check the blood oxygen level, maximum blood oxygen level should not exceed above 100.");
                            TextBoxVitalEntryOxigenLevel.Select();
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        } */
        public Vital GetVitalDetails()
        {
            var Vital = new Vital
            {
                Id = VitalId,
                CompanyId = Global.Company.CompanyId,
                BMI = string.IsNullOrEmpty(TextBoxVitalEntryBMI.Text.Trim()) ? 0 : float.Parse(TextBoxVitalEntryBMI.Text),
                BOxyLevel = string.IsNullOrEmpty(TextBoxVitalEntryOxigenLevel.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryOxigenLevel.Text),
                BPressure = string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryBloodPressure.Text),
                BPressureOver = string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryBloodPressureOver.Text),
                Height = string.IsNullOrEmpty(TextBoxVitalEntryHeight.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryHeight.Text),
                Weight = string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text.Trim()) ? 0 : decimal.Parse(TextBoxVitalEntryWeight.Text),
                Temperature = string.IsNullOrEmpty(TextBoxVitalEntryTemperature.Text.Trim()) ? 0 : float.Parse(TextBoxVitalEntryTemperature.Text),
                RespRate = string.IsNullOrEmpty(TextBoxVitalEntryRespiratoryRate.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryRespiratoryRate.Text),
                Pulse = string.IsNullOrEmpty(TextBoxVitalEntryPulse.Text.Trim()) ? 0 : int.Parse(TextBoxVitalEntryPulse.Text),
                PatientId = (long)PatientId!,
                Date = Global.getTransactionDate()
            };
            return Vital;
        }
        public void LoadVitalDetails()
        {
            if (VitalId != 0L)
            {
                Vital Vital = VitalEntryManager.Instance.GetVitalEntryById(VitalId);
                if (Vital != null)
                {
                    Vital.CompanyId = Global.Company.CompanyId;
                    TextBoxVitalEntryBMI.Text = Vital.BMI.ToString();
                    TextBoxVitalEntryOxigenLevel.Text = Vital.BOxyLevel.ToString();
                    TextBoxVitalEntryBloodPressure.Text = Vital.BPressure.ToString();
                    TextBoxVitalEntryBloodPressureOver.Text = Vital.BPressureOver.ToString();
                    TextBoxVitalEntryHeight.Text = Vital.Height.ToString();
                    TextBoxVitalEntryWeight.Text = Vital.Weight.ToString();
                    TextBoxVitalEntryTemperature.Text = Vital.Temperature.ToString();
                    TextBoxVitalEntryRespiratoryRate.Text = Vital.RespRate.ToString();
                    TextBoxVitalEntryPulse.Text = Vital.Pulse.ToString();
                    PatientId = Vital.PatientId;
                }
                TextBoxVitalEntryHeight.Select();
            }
        }
        private void PatientVitalEntry_Enter(object sender, EventArgs e)
        {
            switch (FocusIndex)
            {
                case 0:
                    break;
                case 1:
                    TextBoxVitalEntryHeight.Focus();
                    break;
                case 2:
                    TextBoxVitalEntryWeight.Focus();
                    break;
                case 3:
                    TextBoxVitalEntryBMI.Focus();
                    break;
                case 4:
                    TextBoxVitalEntryTemperature.Focus();
                    break;
                case 5:
                    TextBoxVitalEntryPulse.Focus();
                    break;
                case 6:
                    TextBoxVitalEntryRespiratoryRate.Focus();
                    break;
                case 7:
                    TextBoxVitalEntryBloodPressure.Focus();
                    break;
                case 8:
                    TextBoxVitalEntryBloodPressureOver.Focus();
                    break;
                case 9:
                    this.BeginInvoke(new Action(() => {
                        TextBoxVitalEntryOxigenLevel.Focus();
                    }));
                    break;                
                default:
                    TextBoxVitalEntryHeight.Focus();
                    break;
            }
        }

        private void TextBoxVitalEntryHeight_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text.Trim()) &&
                !string.IsNullOrEmpty(TextBoxVitalEntryHeight.Text.Trim()))
            {
                double height = double.Parse(TextBoxVitalEntryHeight.Text.Trim());
                double Weight = double.Parse(TextBoxVitalEntryWeight.Text.Trim());

                if (height > 0 && Weight > 0)
                {
                    height = (height / 100);
                    height = (height * height);
                    TextBoxVitalEntryBMI.Text = (Weight / height).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                }
                else
                {
                    TextBoxVitalEntryBMI.Text = string.Empty;
                }
            }
        }

        private void TextBoxVitalEntryBMI_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxVitalEntryBMI.Text.Trim()) && float.Parse(TextBoxVitalEntryBMI.Text) == 0)
            {
                TextBoxVitalEntryBMI.Text = string.Empty;
            }
        }

        private void TextBoxVitalEntryWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            string pattern = @"^\d{0,3}(\.\d{0,3})?$";
            Regex regex = new Regex(pattern);
            int selectionStart = textBox!.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            string newText = textBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, e.KeyChar.ToString());
            if (!regex.IsMatch(newText))
            {
                e.Handled = true;
            }
            else
            {
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart + 1;
                e.Handled = true;
            }
        }
        private void TextBoxVitalEntryWeight_Leave(object sender, EventArgs e)
        {
            string weight = TextBoxVitalEntryWeight.Text;
            if (decimal.TryParse(weight, out decimal wt))
            {
                TextBoxVitalEntryWeight.Text = wt.ToString("F3");
            }
        }
        public void SetVitalDetails(Vital vital)
        {
            TextBoxVitalEntryBMI.Text = vital.BMI.ToString();
            TextBoxVitalEntryOxigenLevel.Text = vital.BOxyLevel.ToString();
            TextBoxVitalEntryBloodPressure.Text = vital.BPressure.ToString();
            TextBoxVitalEntryBloodPressureOver.Text = vital.BPressureOver.ToString();
            TextBoxVitalEntryHeight.Text = vital.Height.ToString();
            TextBoxVitalEntryWeight.Text = vital.Weight.ToString();
            TextBoxVitalEntryTemperature.Text = vital.Temperature.ToString();
            TextBoxVitalEntryRespiratoryRate.Text = vital.RespRate.ToString();
            TextBoxVitalEntryPulse.Text = vital.Pulse.ToString();
            TextBoxVitalEntryHeight.Select();
        }

        private void TextBoxVitalEntryOxigenLevel_Leave(object sender, EventArgs e)
        {
            PatientVitalEntryLastFocus = true;
        }
        
        public bool ValidatePatientVitalEntry()
        {
            // Reset any previous error message
            ErrorMsgs = string.Empty;

            bool isValid = false;

            // Check if at least one field has a value other than empty, "0.00", or zero
            if ((!string.IsNullOrEmpty(TextBoxVitalEntryHeight.Text.Trim()) && TextBoxVitalEntryHeight.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryHeight.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text.Trim()) && TextBoxVitalEntryWeight.Text.Trim() != "0.00" && decimal.Parse(TextBoxVitalEntryWeight.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryTemperature.Text.Trim()) && TextBoxVitalEntryTemperature.Text.Trim() != "0.00" && double.Parse(TextBoxVitalEntryTemperature.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryPulse.Text.Trim()) && TextBoxVitalEntryPulse.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryPulse.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryRespiratoryRate.Text.Trim()) && TextBoxVitalEntryRespiratoryRate.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryRespiratoryRate.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text.Trim()) && TextBoxVitalEntryBloodPressure.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryBloodPressure.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text.Trim()) && TextBoxVitalEntryBloodPressureOver.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryBloodPressureOver.Text) != 0) ||
                (!string.IsNullOrEmpty(TextBoxVitalEntryOxigenLevel.Text.Trim()) && TextBoxVitalEntryOxigenLevel.Text.Trim() != "0.00" && int.Parse(TextBoxVitalEntryOxigenLevel.Text) != 0))
            {
                isValid = true;
            }
            else
            {
                ErrorMsgs = "At least Height should be entered.";
                TextBoxVitalEntryHeight.Focus();
                return false;
            }

            // Validate only the fields that have meaningful values
            if (!string.IsNullOrEmpty(TextBoxVitalEntryHeight.Text.Trim()) && TextBoxVitalEntryHeight.Text.Trim() != "0.00")
            {
                int height = int.Parse(TextBoxVitalEntryHeight.Text);
                if (height > 210)
                {
                    ErrorMsgs = "Height should not exceed 210.";
                    TextBoxVitalEntryHeight.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryWeight.Text.Trim()) && TextBoxVitalEntryWeight.Text.Trim() != "0.00")
            {
                decimal weight = decimal.Parse(TextBoxVitalEntryWeight.Text);
                if (weight > 300)
                {
                    ErrorMsgs = "Weight should not exceed 300.";
                    TextBoxVitalEntryWeight.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryTemperature.Text.Trim()) && TextBoxVitalEntryTemperature.Text.Trim() != "0.00")
            {
                double temperature = double.Parse(TextBoxVitalEntryTemperature.Text);
                if (temperature > 110)
                {
                    ErrorMsgs = "Temperature should not exceed 110.";
                    TextBoxVitalEntryTemperature.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryPulse.Text.Trim()) && TextBoxVitalEntryPulse.Text.Trim() != "0.00")
            {
                int pulse = int.Parse(TextBoxVitalEntryPulse.Text);
                if (pulse > 200)
                {
                    ErrorMsgs = "Pulse should not exceed 200.";
                    TextBoxVitalEntryPulse.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryRespiratoryRate.Text.Trim()) && TextBoxVitalEntryRespiratoryRate.Text.Trim() != "0.00")
            {
                int respiratoryRate = int.Parse(TextBoxVitalEntryRespiratoryRate.Text);
                if (respiratoryRate > 100)
                {
                    ErrorMsgs = "Respiratory rate should not exceed 100.";
                    TextBoxVitalEntryRespiratoryRate.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressure.Text.Trim()) && TextBoxVitalEntryBloodPressure.Text.Trim() != "0.00")
            {
                int bloodPressure = int.Parse(TextBoxVitalEntryBloodPressure.Text);
                if (bloodPressure > 250)
                {
                    ErrorMsgs = "Blood pressure should not exceed 250.";
                    TextBoxVitalEntryBloodPressure.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryBloodPressureOver.Text.Trim()) && TextBoxVitalEntryBloodPressureOver.Text.Trim() != "0.00")
            {
                int bloodPressureOver = int.Parse(TextBoxVitalEntryBloodPressureOver.Text);
                if (bloodPressureOver > 250)
                {
                    ErrorMsgs = "Blood pressure over should not exceed 250.";
                    TextBoxVitalEntryBloodPressureOver.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(TextBoxVitalEntryOxigenLevel.Text.Trim()) && TextBoxVitalEntryOxigenLevel.Text.Trim() != "0.00")
            {
                int oxygenLevel = int.Parse(TextBoxVitalEntryOxigenLevel.Text);
                if (oxygenLevel > 100)
                {
                    ErrorMsgs = "Oxygen level should not exceed 100.";
                    TextBoxVitalEntryOxigenLevel.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}
