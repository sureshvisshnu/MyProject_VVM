using fa;
using fa.views.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.hms.patient;
using fa.views.hms;
using NPOI.POIFS.Properties;
using VisioForge.Libs.NDI;
using fa.views.controls;
using fa.libraries.utils;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.api.utils;
using fa.model.Employee;
using fa.api.Accounting;
using fa.model.Hms.Op;
using FADataAccessLibrary.Api.Hms;
using fa.model.OrderManagement;
using VisioForge.MediaFramework.Helpers;
using System.Windows.Controls;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing;
using iTextSharp.text.pdf.parser.clipper;
using Timer = System.Windows.Forms.Timer;

namespace Fa.views.hms.op
{
    public partial class FormAppointment : FormPatientBase
    {
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteErrorText = "Error in appointment deleting...";
        public static string cancelConfirmText = "Are you sure you want to cancel this appointment?";
        public static string ChoosePatientErrorMsg = "Please select the patient...";
        public static string ReasonForVisitErrMsg = "Please enter the reason for visit...";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";
        public static string ExistAppointmentErrorMsg = "This time slot already have appointment! please check it.";
        public DateTime AppointmentDateFrom { get; set; }
        public DateTime AppointmentDateTo { get; set; }
        public string StartingTime { get; set; }
        public string EndTime { get; set; }
        public string ReasonforVisit { get; set; }
        public long ConsultantID { get; set; }
        public long AppointmentID { get; set; }
        public long PatientID { get; set; }
        public bool isReschedule = false;
        public TimeSpan timeDuration { get; set; }

        private Timer errorBlinkTimer;
        private int blinkCount = 0;
        private const int maxBlink = 6;

        public FormAppointment()
        {
            InitializeComponent();
            errorBlinkTimer = new Timer();
            errorBlinkTimer.Interval = 300;
            errorBlinkTimer.Tick += ErrorBlinkTimer_Tick;
        }

        private void FormAppointment_Load(object sender, EventArgs e)
        {
            ComboUtils.InitializeDoctorCombo(ComboBoxConsultant, Global.Company.CompanyId);
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = AppointmentDateFrom;
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = AppointmentDateTo;
            ToDate.MinDate = AppointmentDateFrom;
            if (PatientID != 0)
            {
                LoadPatientInfo(PatientID, isReschedule);
            }
            else
            {
                ComboBoxConsultant.Visible = false;
                Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(ConsultantID);
                ComboBoxConsultant.SelectedIndex = ComboBoxConsultant.FindStringExact(employee.Name);
                TextBoxAppointmentDepartment.Text = employee != null ? employee.Department.Name : "";
                TextBoxAppointmentReasonForVisit.Select();
            }
            var timeList = GetTimeIntervals();
            ComboBoxStartingTime.Items.AddRange(timeList.ToArray());
            ComboBoxEndTime.Items.AddRange(timeList.ToArray());
            ComboBoxStartingTime.SelectedItem = timeList.FirstOrDefault(item => item.ToString() == StartingTime);
        }

        private List<string> GetTimeIntervals()
        {
            List<string> times = new List<string>();
            DateTime start = DateTime.Today;
            for (int i = 0; i < 96; i++)
            {
                times.Add(start.ToString("hh:mm tt", CultureInfo.InvariantCulture));
                start = start.AddMinutes(15);
            }
            return times;
        }
        private void ComboBoxStartingTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxStartingTime.SelectedIndex == -1) return;
            ComboBoxEndTime.Items.Clear();

            if (ComboBoxStartingTime.SelectedIndex == ComboBoxStartingTime.Items.Count - 1)
            {
                ToDate.Date = FromDate.Date!.Value.AddDays(1);
                foreach (var item in ComboBoxStartingTime.Items)
                {
                    ComboBoxEndTime.Items.Add(item);
                }
            }
            else if (FromDate.Date == ToDate.Date)
            {
                for (int i = ComboBoxStartingTime.SelectedIndex + 1; i < ComboBoxStartingTime.Items.Count; i++)
                {
                    ComboBoxEndTime.Items.Add(ComboBoxStartingTime.Items[i]);
                }
            }
            else
            {
                foreach (var item in ComboBoxStartingTime.Items)
                {
                    ComboBoxEndTime.Items.Add(item);
                }
            }
            ComboBoxEndTime.SelectedIndex = EndTime == "0" ? (ComboBoxEndTime.Items.Count > 0 ? 0 : -1) : (ComboBoxEndTime.FindStringExact(EndTime) == -1 ? 0 : ComboBoxEndTime.FindStringExact(EndTime));
            UpdateDuration();
        }

        private void ComboBoxEndTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxStartingTime.SelectedIndex == -1 || ComboBoxEndTime.SelectedIndex == -1)
                return;

            UpdateDuration();
        }

        private void BtnPatientSearch_Click(object sender, EventArgs e)
        {
            FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
            TextBoxPatientId.Text = PatientIdTransport.Text;
            string patientId = TextBoxPatientId.Text;
            TextBoxPatientId.Text = string.Empty;
            FormPatientSearch.ShowDialog(this);
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                PatientSelectionChange();
            }
            else
            {
                TextBoxPatientId.Text = patientId;
            }
        }

        private void BtnNewPatient_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientRegistration FormPatient = null!;
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
            }
            else
            {
                TextBoxPatientId.Text = patientId;
            }
        }
        protected override void PatientIdTransportReload(object sender, EventArgs e)
        {
            TextBoxPatientId.Text = PatientIdTransport.Text;
        }
        private void PatientSelectionChange()
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                ResetForm();
                LoadPatientInfo(long.Parse(TextBoxPatientId.Text), isReschedule);
                TextBoxAppointmentReasonForVisit.Select();
            }
        }
        private void ResetForm()
        {
            TextBoxAppointmentName.ResetText();
            TextBoxAppointmentDOB.ResetText();
            TextBoxAppointmentAge.ResetText();
            PatientGender.Gender = GenderSelection.None;
            TextBoxAptPatientAddress.ResetText();
            TextBoxAppointmentReasonForVisit.ResetText();
            AppointmentErrMsg.Text = string.Empty;
            TextBoxAppointmentDepartment.ResetText();
            ComboBoxConsultant.ResetText();
            ComboBoxConsultant.SelectedIndex = -1;
            PatientNumberOp.PatientNumber = "000000000000";
            PatientPhoto.Clear();
        }

        public void LoadPatientInfo(long PatientID, bool isReschedule)
        {
            TextBoxPatientId.Text = PatientID.ToString();
            textBoxAppointmentID.Text = AppointmentID.ToString();
            Patient PatientData = PatientManager.Instance.GetPatientById(PatientID == 0 ? long.Parse(TextBoxPatientId.Text) : PatientID);
            Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(ConsultantID);
            if (PatientData != null)
            {
                TextBoxAppointmentName.Text = PatientData.Name;
                if (DateUtils.ValidDate(PatientData.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    TextBoxAppointmentAge.Text = PatientData.Age.ToString();
                    TextBoxAppointmentDOB.Text = PatientData.DateOfBirth.ToString(Global.Company.DateFormat);
                }
                PatientGender.Gender = (GenderSelection)PatientData.Gender + 1;
                PatientNumberOp.PatientNumber = PatientData.PatientNumber;
                if (PatientData.Photo != null)
                {
                    MemoryStream Stream = new MemoryStream(PatientData.Photo);
                    PatientPhoto.Photo = System.Drawing.Image.FromStream(Stream);
                }
                if (PatientData.Address != null)
                {
                    TextBoxAptPatientAddress.Text = PatientData.Address.FullAddress.Replace("\n", "").Replace(", ", "," + System.Environment.NewLine);
                }
            }
            if (isReschedule)
            {
                ComboBoxConsultant.Visible = true;
                ComboBoxConsultant.SelectedIndex = ComboBoxConsultant.FindStringExact(employee.Name);
            }
            else
            {
                ComboBoxConsultant.SelectedIndex = ComboBoxConsultant.FindStringExact(employee.Name);
                TextBoxAppointmentDepartment.Text = employee != null ? employee.Department.Name : "";
                ComboBoxConsultant.Visible = false;
            }
            TextBoxAppointmentReasonForVisit.Text = ReasonforVisit ?? "";
            TextBoxAppointmentReasonForVisit.Select();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == Keys.F2)
            {
                BtnPatientSearch.PerformClick();
                return true;
            }
            else if (keyData == Keys.F3)
            {
                BtnNewPatient.PerformClick();
                return true;
            }
            else if (keyData == Keys.F8)
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == FromDate.Control)
            {
                ComboBoxStartingTime.Focus();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == ToDate.Control)
            {
                ComboBoxEndTime.Focus();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == ComboBoxEndTime.Control)
            {
                BtnPatientSearch.Focus();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == BtnNewPatient)
            {
                TextBoxAppointmentReasonForVisit.Focus();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == TextBoxAppointmentReasonForVisit)
            {
                BtnSave.Focus();
                return true;
            }
            else if (keyData == Keys.Tab && ActiveControl == BtnSave)
            {
                FromDate.Focus();
                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == FromDate.Control)
            {
                BtnSave.Focus();
                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnSave)
            {
                TextBoxAppointmentReasonForVisit.Focus();
                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == ToDate.Control)
            {
                ComboBoxStartingTime.Focus();
                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnPatientSearch)
            {
                ComboBoxEndTime.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (FormValidation())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    PatientAppointment lPatientAppointmentFromDB = null!;
                    PatientAppointment lPatientAppointment = getAppointmentDetailsFromForm();
                    if (lPatientAppointment.Id == 0)
                    {
                        lPatientAppointmentFromDB = PatientAppointmentManager.Instance.AddPatientAppointment(lPatientAppointment);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        lPatientAppointmentFromDB = PatientAppointmentManager.Instance.UpdatePatientAppointment(lPatientAppointment);
                        this.DialogResult = DialogResult.OK;
                    }
                    this.Tag = lPatientAppointmentFromDB;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    AppointmentErrMsg.Text = SaveSuccessText;
                    this.Close();
                }
            }
        }

        private PatientAppointment getAppointmentDetailsFromForm()
        {
            PatientAppointment patientAppointment = new PatientAppointment();
            patientAppointment.Id = string.IsNullOrEmpty(textBoxAppointmentID.Text) ? 0 : long.Parse(textBoxAppointmentID.Text);
            patientAppointment.PatientId = long.Parse(TextBoxPatientId.Text);
            patientAppointment.FromDateOfAppointment = (DateTime)FromDate.Date!;
            patientAppointment.ToDateOfAppointment = (DateTime)ToDate.Date!;
            patientAppointment.StartingTime = ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString();
            patientAppointment.EndTime = ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString();
            patientAppointment.ReasonForTheAppointment = TextBoxAppointmentReasonForVisit.Text;
            patientAppointment.ConsultantId = ((Employee)ComboBoxConsultant.Items[ComboBoxConsultant.SelectedIndex]).Id;
            patientAppointment.CompanyId = Global.Company.CompanyId;

            return patientAppointment;
        }

        private bool FormValidation()
        {
            if (string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                TextBoxAppointmentName.Focus();
                AppointmentErrMsg.Text = ChoosePatientErrorMsg;
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxAppointmentReasonForVisit.Text))
            {
                TextBoxAppointmentReasonForVisit.Focus();
                AppointmentErrMsg.Text = ReasonForVisitErrMsg;
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                AppointmentErrMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                AppointmentErrMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            if (FromDate.Date != null && ToDate.Date != null && FromDate.Date > ToDate.Date)
            {
                AppointmentErrMsg.Text = CheckValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            List<PatientAppointment> appointment = PatientAppointmentManager.Instance.GetExistingAppointments((DateTime)FromDate.Date!, (DateTime)ToDate.Date!, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString(), ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString()).OrderBy(a => a.StartingTime).ToList();
            if (appointment.Count > 0)
            {
                return true;
            }
            List<PatientAppointment> appointments = PatientAppointmentManager.Instance.GetAppointmentsBetweenFromToDate((DateTime)FromDate.Date!, (DateTime)ToDate.Date!, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString(), ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString(), ConsultantID).OrderBy(a => a.StartingTime).ToList();
            appointments = appointments.Where(a => TimeRangesOverlap(a.StartingTime, a.EndTime, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString()!, ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString()!)).ToList();
            if (AppointmentID != 0 ? appointments.Count > 1 : appointments.Count > 0)
            {
                labelAppointmentErrMsg.Text = ExistAppointmentErrorMsg;
                labelAppointmentErrMsg.Visible = true;
                blinkCount = 0;
                errorBlinkTimer.Start();
                return false;
            }
            return true;
        }
        private bool TimeRangesOverlap(string existingStart, string existingEnd, string newStart, string newEnd)
        {
            TimeSpan existingStartTime = DateTime.ParseExact(existingStart, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan existingEndTime = DateTime.ParseExact(existingEnd, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan newStartTime = DateTime.ParseExact(newStart, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan newEndTime = DateTime.ParseExact(newEnd, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

            return newStartTime < existingEndTime && newEndTime > existingStartTime;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (AppointmentID != 0)
            {
                var confirmResult = MessageBox.Show(cancelConfirmText, "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (confirmResult != DialogResult.Yes)
                    return;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool deletionSuccess = PatientAppointmentManager.Instance.DeletePatientAppointment(AppointmentID);
                    this.DialogResult = deletionSuccess ? DialogResult.OK : DialogResult.Cancel;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    this.Close();
                }
            }
        }

        private void ComboBoxConsultant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxConsultant.SelectedIndex != -1)
            {
                Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(((Employee)ComboBoxConsultant.Items[ComboBoxConsultant.SelectedIndex]).Id);
                TextBoxAppointmentDepartment.Text = employee.Department.Name.ToString();
            }
            else
            {
                TextBoxAppointmentDepartment.Text = "";
            }
        }
        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime fromdate = (DateTime)FromDate.Date!;
            ToDate.MinDate = fromdate;
        }

        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            if (ComboBoxStartingTime.SelectedIndex == -1) return;
            ComboBoxEndTime.Items.Clear();

            if (ToDate.Date != FromDate.Date)
            {
                foreach (var item in ComboBoxStartingTime.Items)
                {
                    ComboBoxEndTime.Items.Add(item);
                }
            }
            else
            {
                for (int i = ComboBoxStartingTime.SelectedIndex + 1; i < ComboBoxStartingTime.Items.Count; i++)
                {
                    ComboBoxEndTime.Items.Add(ComboBoxStartingTime.Items[i]);
                }
            }

            ComboBoxEndTime.SelectedIndex = ComboBoxEndTime.Items.Count > 0 ? 0 : -1;
            UpdateDuration();
        }
        private void UpdateDuration()
        {
            if (ComboBoxStartingTime.SelectedItem == null || ComboBoxEndTime.SelectedItem == null)
            {
                labelDuration.Text = "0 min";
                return;
            }
            try
            {
                string dateFormat = Global.Company.DateFormat;
                string timeFormat = "hh:mm tt";

                string fullFormat = $"{dateFormat} {timeFormat}";

                string startDateTimeStr = FromDate.Date!.Value.ToString(dateFormat) + " " + ComboBoxStartingTime.SelectedItem.ToString();
                string endDateTimeStr = ToDate.Date!.Value.ToString(dateFormat) + " " + ComboBoxEndTime.SelectedItem.ToString();

                if (DateTime.TryParseExact(startDateTimeStr, fullFormat, null, System.Globalization.DateTimeStyles.None, out DateTime startDateTime) &&
                    DateTime.TryParseExact(endDateTimeStr, fullFormat, null, System.Globalization.DateTimeStyles.None, out DateTime endDateTime))
                {
                    TimeSpan duration = endDateTime - startDateTime;

                    if (duration.TotalMinutes > 0)
                        labelDuration.Text = $"{(int)duration.TotalHours} hr {duration.Minutes} min";
                    else
                        labelDuration.Text = "0 min";
                }
                else
                {
                    labelDuration.Text = "Invalid time format";
                }
            }
            catch
            {
                labelDuration.Text = "Invalid time format";
            }
        }

        private void ComboBoxStartingTime_Leave(object sender, EventArgs e)
        {
            labelAppointmentErrMsg.Text = "";
            List<PatientAppointment> appointments = PatientAppointmentManager.Instance.GetAppointmentsBetweenFromToDate((DateTime)FromDate.Date!, (DateTime)ToDate.Date!, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString(), ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString(), ConsultantID).OrderBy(a => a.StartingTime).ToList();
            appointments = appointments.Where(a => TimeRangesOverlap(a.StartingTime, a.EndTime, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString()!, ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString()!)).ToList();
            if (AppointmentID != 0 ? appointments.Count > 1 : appointments.Count > 0)
            {
                labelAppointmentErrMsg.Text = ExistAppointmentErrorMsg;
                labelAppointmentErrMsg.Visible = true;
                blinkCount = 0;
                errorBlinkTimer.Start();
            }
        }

        private void ComboBoxEndTime_Leave(object sender, EventArgs e)
        {
            labelAppointmentErrMsg.Text = "";
            List<PatientAppointment> appointments = PatientAppointmentManager.Instance.GetAppointmentsBetweenFromToDate((DateTime)FromDate.Date!, (DateTime)ToDate.Date!, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString(), ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString(), ConsultantID).OrderBy(a => a.StartingTime).ToList();
            appointments = appointments.Where(a => TimeRangesOverlap(a.StartingTime, a.EndTime, ComboBoxStartingTime.Items[ComboBoxStartingTime.SelectedIndex].ToString()!, ComboBoxEndTime.Items[ComboBoxEndTime.SelectedIndex].ToString()!)).ToList();
            if (AppointmentID != 0 ? appointments.Count > 1 : appointments.Count > 0)
            {
                labelAppointmentErrMsg.Text = ExistAppointmentErrorMsg;
                labelAppointmentErrMsg.Visible = true;
                blinkCount = 0;
                errorBlinkTimer.Start();
            }
        }
        private void ErrorBlinkTimer_Tick(object? sender, EventArgs e)
        {
            labelAppointmentErrMsg.Visible = !labelAppointmentErrMsg.Visible;

            blinkCount++;
            if (blinkCount >= maxBlink)
            {
                errorBlinkTimer.Stop();
                labelAppointmentErrMsg.Visible = true;
            }
        }
    }
}
