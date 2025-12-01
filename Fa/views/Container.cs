using DocumentFormat.OpenXml.Drawing;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.UserProfile;
using fa.api.utils;
using fa.common;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using fa.reports.account.transaction;
using fa.reports.account.transaction.trialbalance;
using fa.reports.catalog;
using fa.reports.Hms;
using fa.reports.Inventory;
using fa.reports.master;
using fa.reports.Purchase;
using fa.reports.sales;
using fa.views.account.masters;
using fa.views.account.transactions;
using fa.views.catalog;
using fa.views.controls;
using fa.views.controls.hms;
using fa.views.employee;
using fa.views.hms;
using fa.views.hms.config;
using fa.views.hms.inventory;
using fa.views.hms.ip;
using fa.views.hms.masters;
using fa.views.hms.Masters;
using fa.views.hms.op;
using fa.views.hms.patient;
using fa.views.hms.ward;
using fa.views.inventory;
using fa.views.purchase;
using fa.views.sales;
using fa.views.Systems;
using fa.views.users;
using fa.views.utils;
using Fa.api.Hms;
using Fa.reports.account.transaction;
using Fa.reports.catalog;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using Fa.reports.Purchase;
using Fa.reports.sales;
using Fa.views.catalog;
using Fa.views.hms.Masters;
using Fa.views.hms.op;
using Fa.views.hms.patient;
using Fa.views.inventory;
using Fa.views.purchase;
using Fa.views.sales;
using Fa.views.utils.Common;
using FADataAccessLibrary.Api.Hms;
using iTextSharp.text.pdf.parser.clipper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;
using Rectangle = System.Drawing.Rectangle;

namespace fa.views
{
    public partial class Container : Form
    {
        public static string cancelConfirmText = "Are you sure you want to cancel this appointment?";
        public static string NotAllowToOpenPastAppointmentText = "You cannot open appointments for past dates.";
        public static string NotAllowToCreatePastDaysText = "You cannot create appointments for past dates.";
        public static string NotAllowToDeletePastAppointmentText = "You cannot delete appointments for past dates.";
        public static string NotAllowToConsultPastAppointmentText = "You cannot consult patients for past appointments.";
        public static string NotAllowToMarkPastAppointmentText = "You cannot change this for the past appointments.";

        private int childFormNumber = 0;
        SplashScreen parent = null!;
        private DateTime currentWeekStart;
        private DateTime currentDay = DateTime.Today;
        private enum CalendarView { Week, Day }
        private CalendarView currentView = CalendarView.Week;
        private string ConsultantName = string.Empty;
        private long ConsultantID = 0;
        private User User;
        private DataGridView? selectedGridView;
        private const int ConsultantIdColumnIndexInDayGrid = 97;
        List<DayAppointment> lappointments = new List<DayAppointment>();
        List<WeekAppointment> lWeekAppointments = new List<WeekAppointment>();

        public Container(object sender)
        {
            parent = (SplashScreen)sender;
            InitializeComponent();
            LoadCompanyLogo();
            tableLayoutPanelAppointment.Visible = false;
            GridviewDayAppointment.Visible = false;
            GridviewWeekAppointment.Visible = false;
        }
        private void HideCompanyLogo()
        {
            pictureBox1.Hide();
            pictureBox2.Hide();
        }
        private void LoadCompanyLogo()
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                this.Text = "MediCare";
                LoginMenu.Image = Properties.Resources.terapeia_logo_square;
                aboutToolStripMenuItem.Image = Properties.Resources.terapeia_logo_square;
                this.Icon = Icon.FromHandle(Properties.Resources.terapeia_logo_square.GetHicon());
                aboutToolStripMenuItem.Text = "MediCare";
                pictureBox2.Show();
                pictureBox1.Hide();
            }
            else
            {
                this.Text = "VV Matrix";
                //if (Properties.Resources.BerklySoftVV MatrixSymbol != null)
                //{
                //    this.Icon = Icon.FromHandle(Properties.Resources.BerklySoftVV MatrixSymbol.GetHicon());                
                //    aboutToolStripMenuItem.Image = Properties.Resources.BerklySoftEqualsSymbol;
                //}
                aboutToolStripMenuItem.Text = "VV Matrix";
                pictureBox2.Hide();
                pictureBox1.Show();
            }
        }
        private BuisnessType GetCompanyLoginType()
        {
            try
            {
                BuisnessType businessType = BuisnessType.Wholesale;
                if (Global.Company != null && CompanyManager.Instance != null)
                {
                    Company company = CompanyManager.Instance.GetCompanyForModel((long)Global.Company.CompanyId);
                    if (company != null)
                    {
                        businessType = company.BusinessType;
                        return businessType;
                    }
                    else
                    {
                        throw new InvalidOperationException("Company is null");
                    }
                }
                else
                {
                    if (Global.softwareType == SoftwareType.MEDICARE)
                    {
                        businessType = BuisnessType.Hospital;
                    }
                    Console.WriteLine("Global.Company or CompanyManager.Instance is null");
                    return businessType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BuisnessType.Wholesale;
            }
        }


        private void loadAllDefaultValues()
        {
            try
            {
                CompanyManager cm = CompanyManager.Instance;
                toolStripLoginInfo.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void CompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCompany FormCompany = new FormCompany(this);
            FormCompany.ShowDialog();
            if (Global.Company == null)
            {
                AfterCompanyDelete();
            }
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin loginFrm = new FormLogin(this);
            loginFrm.ShowDialog(this);
            Cursor.Current = Cursors.WaitCursor;
            checkPasswordReset();
            selectCompany();
            Global.LoginType = GetCompanyLoginType();
            SetMainMenu();
            Cursor.Current = Cursors.WaitCursor;
            if (Global.Company != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Global.setTransactionDate((DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat.ToString()), Global.Company.DateFormat));
                    CompanyManager.GenerateIdSpace(Global.Company);
                    CompanyManager.GenerateIdSpace(Global.Company);
                    LoadGlobalData();
                    LoadAppointment();
                    LoadAppointmentsIntoGrid();
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void GridviewDayAppointment_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                selectedGridView = GridviewDayAppointment;
                GridviewDayAppointment.ClearSelection();
                GridviewDayAppointment.CurrentCell = GridviewDayAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex];
                GridviewDayAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;

                var cellValue = GridviewDayAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                contextMenuAppointment.Items.Clear();

                if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    contextMenuAppointment.Items.Add("Reschedule", null, DayRescheduleItem_Click);
                    contextMenuAppointment.Items.Add("Cancel", null, CancelItem_Click);
                }
                else
                {
                    contextMenuAppointment.Items.Add("New", null, NewItem_Click);
                }
                contextMenuAppointment.Show(Cursor.Position);
            }
        }

        private void GridviewWeekAppointment_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                if (User.Roles.Any(x => x.Name.Equals("Doctor")) || User.Roles.Any(x => x.Name.Equals("Nurse")))
                {
                    selectedGridView = GridviewWeekAppointment;
                    GridviewWeekAppointment.ClearSelection();
                    GridviewWeekAppointment.CurrentCell = GridviewWeekAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    GridviewWeekAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;

                    var cellValue = GridviewWeekAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                    contextMenuAppointment.Items.Clear();

                    if (cellValue != null && cellValue is long appointmentId)
                    {
                        var appointment = lWeekAppointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                        PatientAppointment lappointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);

                        if (appointment != null)
                        {
                            if (!appointment.IsConsulted)
                            {
                                contextMenuAppointment.Items.Add("Mark as Consulted", null, MarkasConsulted_Click);
                            }
                            else
                            {
                                contextMenuAppointment.Items.Add("Mark as Not Consulted", null, MarkasUnConsulted_Click);
                            }

                            contextMenuAppointment.Items.Add("Consulting", null, ConsultingPatient_Click);
                            contextMenuAppointment.Items.Add("Reschedule", null, WeekRescheduleItem_Click);
                            contextMenuAppointment.Items.Add("Cancel", null, CancelItem_Click);
                        }
                    }
                    contextMenuAppointment.Show(Cursor.Position);
                }
            }
        }
        private void MarkasUnConsulted_Click(object? sender, EventArgs e)
        {
            UpdateConsultationStatus(false);
        }
        private void MarkasConsulted_Click(object? sender, EventArgs e)
        {
            UpdateConsultationStatus(true);
        }
        private void UpdateConsultationStatus(bool isConsulted)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int rowIndex = GridviewWeekAppointment.CurrentCell.RowIndex;
                int columnIndex = GridviewWeekAppointment.CurrentCell.ColumnIndex;
                var cell = GridviewWeekAppointment.Rows[rowIndex].Cells[columnIndex];

                if (cell.Tag != null && cell.Tag is long appointmentId)
                {
                    PatientAppointment appointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);

                    if (appointment != null && appointment.FromDateOfAppointment.Day < DateTime.Today.Day)
                    {
                        MessageBox.Show(NotAllowToMarkPastAppointmentText, "Access Denied",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var lappointment = lWeekAppointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                    if (lappointment != null)
                    {
                        lappointment.IsConsulted = isConsulted;
                    }

                    appointment!.IsConsulted = isConsulted;
                    PatientAppointmentManager.Instance.UpdatePatientAppointment(appointment);
                    GridviewWeekAppointment.Invalidate();

                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ConsultingPatient_Click(object? sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int rowIndex = GridviewWeekAppointment.CurrentCell.RowIndex;
                int columnIndex = GridviewWeekAppointment.CurrentCell.ColumnIndex;
                var cell = GridviewWeekAppointment.Rows[rowIndex].Cells[columnIndex];

                if (cell.Tag != null && cell.Tag is long appointmentId)
                {
                    PatientAppointment appointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);

                    if (appointment != null && appointment.FromDateOfAppointment.Day < DateTime.Today.Day)
                    {
                        MessageBox.Show(NotAllowToConsultPastAppointmentText, "Access Denied",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    FormConsulting formConsulting = new FormConsulting(this);
                    formConsulting.PatientIdTransport.Text = appointment!.PatientId.ToString();
                    formConsulting.PatientType = PatientTypes.OutPatient;
                    formConsulting.ShowDialog();
                    if (formConsulting.isConsulted == DialogResult.OK)
                    {
                        var weekAppt = lWeekAppointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                        if (weekAppt != null)
                        {
                            weekAppt.IsConsulted = true;
                            appointment.IsConsulted = true;
                        }

                        PatientAppointmentManager.Instance.UpdatePatientAppointment(appointment);

                        GridviewWeekAppointment.Invalidate();
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void NewItem_Click(object? sender, EventArgs e)
        {
            if (currentDay.Day >= DateTime.Today.Day)
            {
                if (GridviewDayAppointment.CurrentCell != null)
                {
                    int rowIndex = GridviewDayAppointment.CurrentCell.RowIndex;
                    int columnIndex = GridviewDayAppointment.CurrentCell.ColumnIndex;
                    OpenAppointment(rowIndex, columnIndex, false, GridviewDayAppointment, false);
                }
            }
            else
            {
                MessageBox.Show(NotAllowToCreatePastDaysText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void WeekRescheduleItem_Click(object? sender, EventArgs e)
        {
            if (selectedGridView != null && selectedGridView.CurrentCell != null)
            {
                int rowIndex = selectedGridView.CurrentCell.RowIndex;
                int columnIndex = selectedGridView.CurrentCell.ColumnIndex;
                var cell = selectedGridView.Rows[rowIndex].Cells[columnIndex];

                if (cell?.Tag is long appointmentId)
                {
                    PatientAppointment existingAppointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);
                    if (existingAppointment != null && existingAppointment.FromDateOfAppointment.Day < DateTime.Today.Day)
                    {
                        MessageBox.Show(NotAllowToOpenPastAppointmentText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (currentDay.Day >= DateTime.Today.Day)
                {
                    if (selectedGridView != null && selectedGridView.CurrentCell != null)
                    {
                        OpenAppointment(rowIndex, columnIndex, true, GridviewWeekAppointment, true);
                    }
                }
                else
                {
                    MessageBox.Show(NotAllowToOpenPastAppointmentText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void DayRescheduleItem_Click(object? sender, EventArgs e)
        {
            if (currentDay.Day >= DateTime.Today.Day)
            {
                if (selectedGridView != null && selectedGridView.CurrentCell != null)
                {
                    int rowIndex = selectedGridView.CurrentCell.RowIndex;
                    int columnIndex = selectedGridView.CurrentCell.ColumnIndex;
                    OpenAppointment(rowIndex, columnIndex, true, GridviewDayAppointment, false);
                }
            }
            else
            {
                MessageBox.Show(NotAllowToOpenPastAppointmentText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CancelItem_Click(object? sender, EventArgs e)
        {
            if (selectedGridView != null && selectedGridView.CurrentCell != null)
            {
                int rowIndex = selectedGridView.CurrentCell.RowIndex;
                int columnIndex = selectedGridView.CurrentCell.ColumnIndex;
                var cell = selectedGridView.Rows[rowIndex].Cells[columnIndex];

                if (cell?.Tag is long appointmentId)
                {
                    PatientAppointment existingAppointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);

                    if (existingAppointment != null && existingAppointment.FromDateOfAppointment.Date < DateTime.Today)
                    {
                        MessageBox.Show(NotAllowToDeletePastAppointmentText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var confirmResult = MessageBox.Show(cancelConfirmText, "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (confirmResult != DialogResult.Yes)
                        return;

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        bool deletionSuccess = PatientAppointmentManager.Instance.DeletePatientAppointment(appointmentId);
                        this.DialogResult = deletionSuccess ? DialogResult.OK : DialogResult.Cancel;

                        if (this.DialogResult == DialogResult.OK)
                        {
                            LoadAppointmentsIntoGrid();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void LoadAppointment()
        {
            if (Global.softwareType == SoftwareType.MEDICARE)
            {
                currentDay = DateTime.Today;
                GridviewDayAppointment.Rows.Clear();
                GridviewWeekAppointment.Rows.Clear();
                User user = UserManager.Instance.GetUserByLogin(Global.User.Login);
                if (user.Employee != null)
                {
                    ConsultantID = user.Employee.Id;
                    currentWeekStart = GetStartOfWeek(DateTime.Today);
                    lableMonth.Text = currentDay.ToString("MMMM") + " - " + currentDay.Day.ToString() + ", " + currentDay.Year.ToString();
                    User = user;

                    if (!user.Roles.Any(x => x.Name.Equals("Doctor")) && !user.Roles.Any(x => x.Name.Equals("Nurse")))
                    {
                        currentView = CalendarView.Day;
                        tableLayoutPanelAppointment.Visible = true;
                        GridviewDayAppointment.Visible = true;
                        GridviewWeekAppointment.Visible = false;

                        IList<Employee> consultants = EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company!.CompanyId, "Doctor").ToList();
                        if (consultants.Count > 0)
                        {
                            LoadConsultants(consultants);
                        }
                    }
                    else if (user.Roles.Any(x => x.Name.Equals("Doctor")) || user.Roles.Any(x => x.Name.Equals("Nurse")))
                    {
                        currentView = CalendarView.Week;
                        tableLayoutPanelAppointment.Visible = true;
                        GridviewDayAppointment.Visible = false;
                        GridviewWeekAppointment.Visible = true;

                        UpdateHeader(CalendarView.Week, currentWeekStart);
                        CreateTimeSlots();
                        lableMonth.Text = GetWeekRangeText(currentWeekStart);
                    }
                }
            }
        }
        private void LoadAppointmentsIntoGrid()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (currentView == CalendarView.Day)
                {
                    LoadDayViewAppointments();
                }
                else if (currentView == CalendarView.Week)
                {
                    LoadWeekViewAppointments();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void LoadDayViewAppointments()
        {
            lappointments.Clear();
            foreach (DataGridViewRow row in GridviewDayAppointment.Rows)
            {
                for (int col = 1; col < GridviewDayAppointment.Columns.Count - 1; col++)
                {
                    row.Cells[col].Value = null;
                    row.Cells[col].Style.BackColor = Color.White;
                    row.Cells[col].Tag = null;
                }
            }
            List<PatientAppointment> appointments = PatientAppointmentManager.Instance.GetAppointmentsByDate(currentDay).OrderBy(a => a.StartingTime).ToList();

            foreach (PatientAppointment appointment in appointments)
            {
                foreach (DataGridViewRow row in GridviewDayAppointment.Rows)
                {
                    if (row.Cells[97].Value?.ToString() == appointment.ConsultantId.ToString())
                    {
                        TimeSpan appointmentStartTime;
                        TimeSpan appointmentEndTime;
                        TimeSpan dayStartTime = TimeSpan.Zero;
                        TimeSpan dayEndTime = new TimeSpan(24, 0, 0);

                        TimeSpan originalStartTime = DateTime.ParseExact(appointment.StartingTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                        TimeSpan originalEndTime = DateTime.ParseExact(appointment.EndTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                        if (currentDay.Date > appointment.FromDateOfAppointment.Date)
                        {
                            appointmentStartTime = dayStartTime;

                            TimeSpan originalDuration = originalEndTime - originalStartTime;
                            TimeSpan usedDuration = dayEndTime - originalStartTime;
                            if (currentDay.Date < appointment.ToDateOfAppointment.Date)
                            {
                                appointmentEndTime = dayEndTime;
                            }
                            else
                            {
                                appointmentEndTime = originalEndTime;
                            }
                        }
                        else
                        {
                            appointmentStartTime = originalStartTime;
                            if (currentDay.Date < appointment.ToDateOfAppointment.Date)
                            {
                                appointmentEndTime = dayEndTime;
                            }
                            else
                            {
                                appointmentEndTime = originalEndTime;
                            }
                        }

                        string startTimeString = DateTime.Today.Add(appointmentStartTime).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        for (int colIndex = 1; colIndex < 97; colIndex++)
                        {
                            if (GridviewDayAppointment.Columns[colIndex].HeaderText == startTimeString)
                            {
                                TimeSpan duration = appointmentEndTime - appointmentStartTime;
                                int spanCount = (int)Math.Max(1, duration.TotalMinutes / 15);

                                var patient = PatientManager.Instance.GetPatientById(appointment.PatientId);
                                string displayText = $"{patient?.Name ?? "Unknown"}\n{appointment.ReasonForTheAppointment}";

                                bool isFirstDay = currentDay.Date == appointment.FromDateOfAppointment.Date;
                                string cellText = isFirstDay ? displayText : displayText;

                                DayAppointment lpatientAppointment = new DayAppointment();
                                lpatientAppointment.DurationMinutes = (int)duration.TotalMinutes;
                                lpatientAppointment.ReasonForVisit = appointment.ReasonForTheAppointment;
                                lpatientAppointment.RowIndex = row.Index;
                                lpatientAppointment.ColumnIndex = colIndex;
                                lpatientAppointment.PatientName = appointment.Patient.Name.ToString();
                                lappointments.Add(lpatientAppointment);

                                for (int i = 0; i < spanCount; i++)
                                {
                                    int currentColIndex = colIndex + i;
                                    if (currentColIndex >= 97) break;

                                    var cell = row.Cells[currentColIndex];
                                    cell.Style.BackColor = isFirstDay ? SystemColors.ControlLight : Color.LightBlue;
                                    cell.Style.WrapMode = DataGridViewTriState.True;
                                    cell.Style.Alignment = DataGridViewContentAlignment.TopLeft;

                                    cell.Value = i == 0 ? cellText : "";
                                    cell.Tag = appointment.Id;
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            DateTime startTime = DateTime.Today;
            DateTime endTime = startTime.AddDays(1);

            DateTime now = DateTime.Now;
            int currentMinutes = (now.Hour * 60) + now.Minute;
            int roundedMinutes = (currentMinutes / 15) * 15;
            DateTime currentSlotTime = DateTime.Today.AddMinutes(roundedMinutes);

            int targetRowIndex = -1;
            int rowIndex = 0;
            while (startTime < endTime)
            {
                string timeText = startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                rowIndex++;

                if (startTime == currentSlotTime)
                {
                    targetRowIndex = rowIndex;
                    break;
                }
                startTime = startTime.AddMinutes(15);
            }
            if (targetRowIndex >= 0)
            {
                GridviewDayAppointment.FirstDisplayedScrollingColumnIndex = targetRowIndex;
                GridviewDayAppointment.Columns[targetRowIndex].Selected = true;
            }
            GridviewDayAppointment.Invalidate();
        }


        private void LoadWeekViewAppointments()
        {
            lWeekAppointments.Clear();
            foreach (DataGridViewRow row in GridviewWeekAppointment.Rows)
            {
                for (int col = 1; col < GridviewWeekAppointment.Columns.Count; col++)
                {
                    row.Cells[col].Value = null;
                    row.Cells[col].Style.BackColor = Color.White;
                    row.Cells[col].Tag = null;
                }
            }
            List<PatientAppointment> appointments = PatientAppointmentManager.Instance.GetAppointmentsByDateRange(ConsultantID, currentWeekStart, currentWeekStart.AddDays(7)).OrderBy(a => a.FromDateOfAppointment).ThenBy(a => a.StartingTime).ToList();

            foreach (PatientAppointment appointment in appointments)
            {
                TimeSpan appointmentStartTime = DateTime.ParseExact(appointment.StartingTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                TimeSpan appointmentEndTime = DateTime.ParseExact(appointment.EndTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

                DateTime currentAppointmentDay = appointment.FromDateOfAppointment.Date;
                DateTime lastAppointmentDay = appointment.ToDateOfAppointment.Date;

                while (currentAppointmentDay <= lastAppointmentDay)
                {
                    if (currentAppointmentDay >= currentWeekStart && currentAppointmentDay < currentWeekStart.AddDays(7))
                    {
                        int dayIndex = (currentAppointmentDay - currentWeekStart).Days + 1;
                        if (dayIndex < 1 || dayIndex >= GridviewWeekAppointment.Columns.Count)
                            continue;

                        TimeSpan dayStartTime = TimeSpan.Zero;
                        TimeSpan dayEndTime = new TimeSpan(24, 0, 0);
                        TimeSpan currentDayStartTime = appointmentStartTime;
                        TimeSpan currentDayEndTime = appointmentEndTime;

                        if (currentAppointmentDay > appointment.FromDateOfAppointment.Date)
                        {
                            currentDayStartTime = dayStartTime;
                        }

                        if (currentAppointmentDay < appointment.ToDateOfAppointment.Date)
                        {
                            currentDayEndTime = dayEndTime;
                        }

                        TimeSpan currentDayDuration = currentDayEndTime - currentDayStartTime;
                        int span = (int)Math.Ceiling(currentDayDuration.TotalMinutes / 15);

                        string startTimeString = DateTime.Today.Add(currentDayStartTime).ToString("hh:mm tt", CultureInfo.InvariantCulture);

                        for (int rowIndex = 0; rowIndex < GridviewWeekAppointment.Rows.Count; rowIndex++)
                        {
                            if (GridviewWeekAppointment.Rows[rowIndex].Cells[0].Value?.ToString()!.Contains(startTimeString) == true)
                            {
                                var patient = PatientManager.Instance.GetPatientById(appointment.PatientId);
                                bool isFirstDay = currentAppointmentDay == appointment.FromDateOfAppointment.Date;
                                string displayText = isFirstDay
                                    ? $"{patient?.Name ?? "Unknown"}\n{appointment.ReasonForTheAppointment}"
                                    : $"{patient?.Name ?? "Unknown"}\n{appointment.ReasonForTheAppointment}";

                                lWeekAppointments.Add(new WeekAppointment
                                {
                                    RowIndex = rowIndex,
                                    ColumnIndex = dayIndex,
                                    RowSpan = span,
                                    ReasonForVisit = displayText,
                                    IsContinuation = !isFirstDay,
                                    IsConsulted = appointment.IsConsulted,
                                    AppointmentId = appointment.Id
                                });

                                for (int i = 0; i < span; i++)
                                {
                                    if ((rowIndex + i) >= GridviewWeekAppointment.Rows.Count) break;

                                    var cell = GridviewWeekAppointment.Rows[rowIndex + i].Cells[dayIndex];
                                    cell.Value = i == 0 ? displayText : "";
                                    cell.Tag = appointment.Id;
                                    cell.Style.BackColor = isFirstDay ? SystemColors.ControlLight : Color.LightBlue;
                                    cell.Style.Alignment = DataGridViewContentAlignment.TopLeft;
                                    cell.Style.WrapMode = DataGridViewTriState.True;
                                }
                                break;
                            }
                        }
                    }
                    currentAppointmentDay = currentAppointmentDay.AddDays(1);
                }
            }
            GridviewWeekAppointment.RowTemplate.Height = 40;
            GridviewWeekAppointment.Invalidate();
        }
        private void LoadConsultants(IList<Employee> consultants)
        {
            if (consultants.Count > 0)
            {
                int i = 0;
                GridviewDayAppointment.Rows.Clear();
                GridviewDayAppointment.RowTemplate.Height = 40;
                foreach (Employee consultant in consultants)
                {
                    GridviewDayAppointment.Rows.Add();
                    GridviewDayAppointment.Rows[i].Cells[0].Value = consultant.Name.ToString();
                    GridviewDayAppointment.Rows[i].Cells[97].Value = consultant.Id.ToString();
                    i++;
                }
            }
        }

        private string GetWeekRangeText(DateTime startOfWeek)
        {
            DateTime endOfWeek = startOfWeek.AddDays(6);
            return $"{startOfWeek:MMMM} {startOfWeek:dd} - {endOfWeek:dd}, {startOfWeek:yyyy}";
        }
        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        public void LoadGlobalData()
        {
            Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            if (Global.Company.BusinessType == BuisnessType.Hospital)
            {
                Global.ConsultationDetailList = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
                Global.AllProcesdureDetailList = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(Global.Company.CompanyId);
                Global.AllSymptomDetailList = SymptomsManager.Instance.ListSymptomByCompanyId(Global.Company.CompanyId);
            }
            RenderLoginDetails();
        }
        private void checkPasswordReset()
        {
            if (Global.isAuthenticated && Global.User.IsResetPassword)
            {
                FormResetPassword FormResetPassword = new FormResetPassword();
                FormResetPassword.ShowDialog();
                this.Show();
            }
        }

        private void costCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCostCenter FormCostCenter = new FormCostCenter(this);
            FormCostCenter.ShowDialog(this);
            Cursor.Current = Cursors.Default;
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutVVMApps AboutFrom = new AboutVVMApps();
            AboutFrom.Show();
        }

        private void customersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormCustomers CustomerFrom = new FormCustomers(this);
            CustomerFrom.ShowDialog(this);
            Cursor.Current = Cursors.Default;
        }
        bool IsDbError = false;
        private void Container_Load(object sender, EventArgs e)
        {
            IsDbError = false;
            try
            {
                this.TopMost = true;
                MainMenuStrip.Dock = DockStyle.Top;
                MainMenuStrip.Visible = true;
                toolStripLoginInfo.Visible = false;
                if (Global.softwareType == SoftwareType.MEDICARE)
                {
                    Global.ApplicationName = "MEDICARE v1.0";
                }
                else
                {
                    Global.ApplicationName = "VV MATRIX v1.0";
                }
                this.Text = Global.ApplicationName;
                this.KeyPreview = true;
                this.TopMost = false;
                FormLogin loginFrm = new FormLogin(this);
                loginFrm.ShowDialog(this);
                checkPasswordReset();
                selectCompany();
                Global.LoginType = GetCompanyLoginType();
                LoadCompanyLogo();
                SetMainMenu();
                if (Global.Company != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Global.setTransactionDate((DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat.ToString()), Global.Company.DateFormat)!);
                    CompanyManager.GenerateIdSpace(Global.Company);
                    LoadGlobalData();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Logout();
                    return;
                }
                LoadAppointment();
                LoadAppointmentsIntoGrid();
            }
#pragma warning disable 0168 // variable declared but not used.
            catch (Exception ex)
            {
                IsDbError = true;
                //tokenSource2.Cancel();
                MessageBox.Show("Connection Error Please Contact Administrator");
                Application.Exit();
            }
        }

        public void RenderLoginDetails()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                if (Global.Company != null)
                {
                    User user = Global.User;
                    UserManager UserManager = UserManager.Instance;
                    IList<Role> userRole = UserManager.ListAllUserRoleByUserId(user.UserId);
                    toolStripTextBoxCompany.Text = Global.Company.Name;
                    toolStripTextBoxCompany.Visible = true;
                    Cursor.Current = Cursors.WaitCursor;
                    List<CostCenter> AccessibleCostCenter = UserManager.GetAccessibleCostCenter(Global.User, Global.Company.CompanyId);
                    Global.Company.CostCenters = AccessibleCostCenter;
                    if (Global.Company.CostCenters != null && Global.Company.CostCenters.Count > 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (Global.CostCenter == null)
                        {
                            Global.CostCenter = AccessibleCostCenter.First();
                        }
                        toolStripTextBoxCostCenter.Text = Global.CostCenter.Name;
                        toolStripTextBoxCostCenter.Visible = false;
                        toolStripCostCenterLabel.Visible = false;
                        toolStripCostCenterSeparator.Visible = false;
                        toolStripCostCenterButton.Visible = false;
                    }
                    else if (Global.Company.CostCenters != null && Global.Company.CostCenters.Count == 1)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Global.CostCenter = AccessibleCostCenter.First();
                        toolStripTextBoxCostCenter.Text = Global.CostCenter.Name;
                        toolStripCostCenterLabel.Visible = false;
                        toolStripTextBoxCostCenter.Visible = false;
                        toolStripCostCenterSeparator.Visible = false;
                        toolStripCostCenterButton.Visible = false;
                    }
                    else
                    {
                        toolStripTextBoxCostCenter.Text = "None";
                        toolStripCostCenterLabel.Visible = false;
                        toolStripTextBoxCostCenter.Visible = false;
                        toolStripCostCenterSeparator.Visible = false;
                        toolStripCostCenterButton.Visible = false;
                    }
                    //List<Company> AccessibleCompanies = UserManager.GetAccessibleCompanies(Global.User); 
                    List<Company> AccessibleCompanies = UserManager.GetAccessibleCompaniesBySoftwareType(Global.User, Global.softwareType);
                    toolStripChangeCompanyButton.Visible = (AccessibleCompanies.Count < 2 ? false : true);
                    toolStripTextBoxTransactionDate.Text = Global.getTransactionDate().ToString(Global.Company.DateFormat);
                    toolStripLoginInfo.Visible = true;

                    if (Global.Company.BusinessType == BuisnessType.Hospital && (user.IsSuperAdmin || Global.User.Roles.Any(role => role.Name == "Admin")))
                    {
                        hospitalToolStripMenuItem.Visible = true;
                        inPatientCareToolStripMenuItem1.Visible = true;
                        nursesTechToolStripMenuItem.Visible = true;
                        doctorToolStripMenuItem.Visible = true;
                        OutPatientCareToolStripMenuItem.Visible = true;
                        manageCatalogToolStripMenuItem.Visible = true;
                        manageItemTaxToolStripMenuItem.Visible = true;
                    }

                    if (Global.Company.BusinessType != BuisnessType.Hospital)
                    {
                        hospitalToolStripMenuItem.Visible = false;
                        inPatientCareToolStripMenuItem1.Visible = false;
                        nursesTechToolStripMenuItem.Visible = false;
                        doctorToolStripMenuItem.Visible = false;
                        OutPatientCareToolStripMenuItem.Visible = false;
                    }
                    if (Global.Company.BusinessType == BuisnessType.Professionals)
                    {
                        salesMenuItem.Visible = false;
                        purchaseMenuItem.Visible = false;
                        pOSToolStripMenuItem.Visible = false;
                        catalogToolStripMenuItem.Visible = false;
                        companyMenuItem.Visible = false;
                    }
                    if (Global.Company.BusinessType != BuisnessType.Professionals && (user.IsSuperAdmin || Global.User.Roles.Any(role => role.Name == "Admin")))
                    {
                        salesMenuItem.Visible = true;
                        purchaseMenuItem.Visible = true;
                        pOSToolStripMenuItem.Visible = true;
                        catalogToolStripMenuItem.Visible = true;
                        companyMenuItem.Visible = true;
                    }
                    if (Global.Company != null && !Global.Company.HasProductCatalog)
                    {
                        pOSToolStripMenuItem.Visible = false;
                        salesMenuItem.Visible = false;
                        purchaseMenuItem.Visible = false;
                        catalogToolStripMenuItem.Visible = false;
                        if (Global.Company.BusinessType == BuisnessType.Hospital)
                        {
                            catalogToolStripMenuItem.Visible = true;
                            manageCatalogToolStripMenuItem.Visible = false;
                            manageItemTaxToolStripMenuItem.Visible = false;
                        }
                        //OutPatientCareToolStripMenuItem.Visible = false;
                    }
                    if (userRole.Any(x => x.RoleId == 14) && userRole.Any(x => x.Name == "Register-OP"))
                    {
                        careTakerAssignReportToolStripMenuItem.Visible = false;
                        careTakerUnAssignReportToolStripMenuItem.Visible = false;
                        opReportToolStripMenuItem.Visible = false;
                        iPReportToolStripMenuItem.Visible = false;
                        patientLedgerToolStripMenuItem.Visible = false;
                        patientDueListToolStripMenuItem.Visible = false;
                        wardAndBedReportToolStripMenuItem.Visible = false;
                        paymentReceiveReportToolStripMenuItem.Visible = false;
                        patientTransferReportToolStripMenuItem.Visible = false;
                        patientProcedureReportToolStripMenuItem.Visible = false;
                        labTestReportToolStripMenuItem.Visible = false;
                        dischargeReportToolStripMenuItem.Visible = false;
                        patientVisitCountingReportToolStripMenuItem.Visible = false;
                        feeCollectionReportToolStripMenuItem.Visible = false;
                    }
                }
                else
                {
                    toolStripLoginInfo.Visible = false;
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ResetForm()
        {
            toolStripTextBoxCompany.Clear();
            toolStripTextBoxCostCenter.Clear();
            toolStripTextBoxTransactionDate.Clear();
        }

        public void AfterCompanyDelete()
        {
            ResetForm();
            selectCompany();
            if (Global.Company != null)
            {
                Global.setTransactionDate((DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat.ToString()), Global.Company.DateFormat));
                LoadGlobalData();
                LoadCompanyLogo();
                RenderLoginDetails();
            }
        }
        public void selectCompany()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (Global.isAuthenticated)
                {
                    UserManager UserManager = UserManager.Instance;
                    //List<Company> AccessibleCompanies = UserManager.GetAccessibleCompanies(Global.User); 
                    List<Company> AccessibleCompanies = UserManager.GetAccessibleCompaniesBySoftwareType(Global.User, Global.softwareType);
                    if (AccessibleCompanies.Count > 1)
                    {
                        FormSelectCompany FormSelectCompany = new FormSelectCompany();
                        FormSelectCompany.ShowDialog(this);
                    }
                    else if (AccessibleCompanies.Count == 1)
                    {
                        Global.Company = CompanyManager.Instance.GetCompany(AccessibleCompanies.First().CompanyId);
                    }
                    else if (Global.User.IsSuperAdmin)
                    {
                        if (fa.Data.Global.TransactionDate == new DateTime(1, 1, 1, 0, 0, 0))
                        {
                            fa.Data.Global.TransactionDate = DateTime.Now;
                        }
                        FormCompany FormCompany = new FormCompany(this);
                        FormCompany.CreateCompanyOnLoad = true;
                        FormCompany.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show("You do not have access to any company!, Please contact your administrator", "Invalid Access");
                        Logout();
                    }
                    if (Global.Company == null)
                    {
                        Logout();
                    }
                    else
                    {
                        EnableReceivePayment();
                    }

                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        public void EnableReceivePayment()
        {
            receivePaymentToolStripMenuItem.Visible = true;
            deliveryToolStripMenuItem.Visible = true;
            if (!Global.Company.CompanySalesSetup.IsReceivePayment)
            {
                receivePaymentToolStripMenuItem.Visible = false;
                deliveryToolStripMenuItem.Visible = false;
            }
            else if (!Global.Company.CompanySalesSetup.IsDelivery)
            {
                deliveryToolStripMenuItem.Visible = false;
            }
        }
        private void SelectCostCenter()
        {
            if (Global.isAuthenticated)
            {
                UserManager UserManager = UserManager.Instance;
                List<CostCenter> AccessibleCostCenter = UserManager.GetAccessibleCostCenter(Global.User, Global.Company.CompanyId);
                if (AccessibleCostCenter.Count > 1)
                {
                    FormChangeCostCenter FormChangeCostCenter = new FormChangeCostCenter();
                    FormChangeCostCenter.ShowDialog(this);
                }
            }
        }
        private void AccountsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormGeneralAccounts FormGeneralAccounts = new FormGeneralAccounts(this);
            FormGeneralAccounts.ShowDialog();
            Cursor.Current = Cursors.Default;
        }
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormUsers FormUser = new FormUsers();
            FormUser.ShowDialog();
            Cursor.Current = Cursors.Default;
        }

        public void SetMainMenu()
        {
            User user = Global.User;
            if (user != null && Global.isAuthenticated)
            {
                if (user.IsSuperAdmin)
                {
                    EnableAllMenu();
                    return;
                }
                var accessibleMenus = user.GetSystemFunctions();
                foreach (ToolStripMenuItem item in menuStripMain.Items)
                {
                    if (item.Name.Equals("helpMenu"))
                    {
                        aboutToolStripMenuItem.Visible = true;
                        continue;
                    }
                    else if (item.Name.Equals("LoginMenu"))
                    {
                        loginMenuItem.Visible = false;
                        logoutMenuItem.Visible = true;
                        exitMenuItem.Visible = true;
                        continue;
                    }
                    Boolean hasMenuItem = false;
                    if (item.HasDropDown)
                    {
                        ToolStripItemCollection ddi = item.DropDownItems;
                        foreach (object obj in ddi)
                        {
                            if (obj.GetType().Equals(typeof(ToolStripMenuItem)))
                            {
                                ToolStripMenuItem subMenu = (ToolStripMenuItem)obj;
                                subMenu.Visible = false;
                                foreach (var accessibleMenu in accessibleMenus)
                                {
                                    if (accessibleMenu.Name.Equals(subMenu.Name))
                                    {
                                        subMenu.Visible = true;
                                        hasMenuItem = true;
                                    }
                                }
                            }
                        }
                        if (hasMenuItem == true)
                        {
                            item.Visible = true;
                        }
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
                if (Global.Company != null && Global.Company.BusinessType != BuisnessType.Hospital)
                {
                    hospitalToolStripMenuItem.Visible = false;
                    inPatientCareToolStripMenuItem1.Visible = false;
                    nursesTechToolStripMenuItem.Visible = false;
                    doctorToolStripMenuItem.Visible = false;
                    OutPatientCareToolStripMenuItem.Visible = false;
                }
                if (Global.Company != null && !Global.Company.HasProductCatalog)
                {
                    pOSToolStripMenuItem.Visible = false;
                    salesMenuItem.Visible = false;
                    purchaseMenuItem.Visible = false;
                    catalogToolStripMenuItem.Visible = false;
                    //OutPatientCareToolStripMenuItem.Visible = false;
                }
            }
        }

        private bool LoadChildMenu(ToolStripMenuItem item)
        {
            var accessibleMenus = Global.User.GetSystemFunctions();
            Boolean hasMenuItem = false;

            ToolStripItemCollection ddi = item.DropDownItems;
            foreach (object obj in ddi)
            {
                if (obj.GetType().Equals(typeof(ToolStripMenuItem)))
                {
                    ToolStripMenuItem subMenu = (ToolStripMenuItem)obj;
                    if (subMenu.HasDropDown)
                    {
                        hasMenuItem = LoadChildMenu(subMenu);
                    }
                    else
                    {
                        subMenu.Visible = false;
                        foreach (var accessibleMenu in accessibleMenus)
                        {
                            if (accessibleMenu.Name.Equals(subMenu.Name))
                            {
                                subMenu.Visible = true;
                                hasMenuItem = true;
                                if (subMenu.Name.Equals("costCenterMenuItem"))
                                {
                                    costCenterMenuItem.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            if (hasMenuItem == true)
            {
                item.Visible = true;
            }

            return hasMenuItem;
        }
        private void EnableAllMenu()
        {
            User user = Global.User;
            if (user != null && Global.User.IsSuperAdmin)
            {
                menuStripMain.ResumeLayout();
                foreach (ToolStripMenuItem item in menuStripMain.Items)
                {
                    if (item.Name.Equals("helpMenu"))
                    {
                        continue;
                    }
                    if (item.Name.Equals("LoginMenu"))
                    {
                        loginMenuItem.Visible = false;
                        logoutMenuItem.Visible = true;
                        exitMenuItem.Visible = true;
                        continue;
                    }
                    if (item.HasDropDown)
                    {
                        ToolStripItemCollection ddi = item.DropDownItems;
                        foreach (object obj in ddi)
                        {
                            if (obj.GetType().Equals(typeof(ToolStripMenuItem)))
                            {
                                ToolStripMenuItem subMenu = (ToolStripMenuItem)obj;
                                if (subMenu.HasDropDown && subMenu.DropDownItems.Count > 0)
                                {
                                    EnableAllChildMenu(subMenu);
                                }
                                else
                                {
                                    subMenu.Visible = true;
                                    if (subMenu.Name.Equals("receivePaymentToolStripMenuItem"))
                                    {
                                        subMenu.Visible = Global.Company.CompanySalesSetup.IsReceivePayment;
                                    }
                                    if (subMenu.Name.Equals("deliveryToolStripMenuItem"))
                                    {
                                        subMenu.Visible = Global.Company.CompanySalesSetup.IsDelivery;
                                    }
                                    if (subMenu.Name.Equals("costCenterMenuItem"))
                                    {
                                        costCenterMenuItem.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    item.Visible = true;
                }
            }
        }
        private bool EnableAllChildMenu(ToolStripMenuItem item)
        {
            ToolStripItemCollection ddi = item.DropDownItems;
            foreach (object obj in ddi)
            {
                if (obj.GetType().Equals(typeof(ToolStripMenuItem)))
                {
                    ToolStripMenuItem subMenu = (ToolStripMenuItem)obj;
                    if (subMenu.HasDropDown)
                    {
                        LoadChildMenu(subMenu);
                    }
                    else
                    {
                        subMenu.Visible = true;
                    }
                }
            }
            item.Visible = true;
            return true;
        }
        public void ResetMainMenu()
        {
            foreach (ToolStripMenuItem item in menuStripMain.Items)
            {
                if (item.Name.Equals("LoginMenu"))
                {
                    item.Visible = true;
                    loginMenuItem.Visible = true;
                    logoutMenuItem.Visible = false;
                    exitMenuItem.Visible = true;
                }
                else if (item.Name.Equals("helpMenu"))
                {
                    item.Visible = true;
                }
                else
                {
                    ToolStripItemCollection ddi = item.DropDownItems;
                    item.Visible = false;
                }
            }
        }
        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }
        public void Logout()
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            Global.User = null;
            Global.Company = null;
            Global.isAuthenticated = false;
            toolStripLoginInfo.Visible = false;
            ResetMainMenu();
            tableLayoutPanelAppointment.Visible = false;
            GridviewDayAppointment.Visible = false;
            GridviewWeekAppointment.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void ProfitLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProfitLoss FormProfitLoss = new FormProfitLoss();
            FormProfitLoss.ShowDialog();
        }

        private void ChangePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormResetPassword ResetPasswordForm = new FormResetPassword();
            ResetPasswordForm.ShowDialog();
        }

        private void SuppliersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormSupplier SupplierForm = new FormSupplier(this);
            SupplierForm.ShowDialog(this);
            Cursor.Current = Cursors.Default;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            selectCompany();
            if (Global.Company != null)
            {
                Global.setTransactionDate((DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat.ToString()), Global.Company.DateFormat));
                LoadGlobalData();
                LoadCompanyLogo();
                RenderLoginDetails();
            }
            CloseAllToolStripMenuItem_Click(this, null);
        }

        private void receiptsMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormReceipts ReceiptsForm = new FormReceipts();
            ReceiptsForm.ShowDialog();
            ReceiptsForm.BringToFront();
            Cursor.Current = Cursors.Default;
        }

        private void paymentsMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormPayment PaymentForm = new FormPayment();
            PaymentForm.ShowDialog();
            PaymentForm.BringToFront();
            Cursor.Current = Cursors.Default;
        }

        private void journalsMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormJournal FormJournal = new FormJournal();
            FormJournal.ShowDialog();
            FormJournal.BringToFront();
            Cursor.Current = Cursors.Default;
        }

        private void ledgerMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormLedger LedgerForm = new FormLedger();
            LedgerForm.ShowDialog();
            LedgerForm.BringToFront();
            Cursor.Current = Cursors.Default;
        }

        private void toolStripChangeDateButton_Click(object sender, EventArgs e)
        {
            //open change date form
            FormTransactionDate dateForm = new FormTransactionDate();
            dateForm.ShowDialog();
            RenderLoginDetails();
        }

        private void Container_Leave(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure To Exit Programme ?", "Exit", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {
                return;
            }
        }

        private void toolStripCostCenterButton_Click(object sender, EventArgs e)
        {
            SelectCostCenter();
            Cursor.Current = Cursors.WaitCursor;
            RenderLoginDetails();
            CloseAllToolStripMenuItem_Click(this, null);
            Cursor.Current = Cursors.Default;
        }

        private void Container_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F4)
            {
                this.Close();
            }
            if (e.Control && e.KeyCode == Keys.F1)
            {
                this.helpMenu.ShowDropDown();
            }
            if (e.Control && e.KeyCode == Keys.M)
            {
                if (toolsMenu.Visible == true)
                {
                    this.toolsMenu.ShowDropDown();
                }
            }
        }
        bool IsApplicationExit = true;
        private void Container_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsDbError && IsApplicationExit)
            {
                DialogResult Result = MessageBox.Show("Exit From " + Global.ApplicationName, "Exit Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            if (e.Cancel == false)
            {
                IsApplicationExit = false;
                Application.Exit();
            }
        }
        private void creditNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCreditNote FormCreditNote = new FormCreditNote();
            FormCreditNote.ShowDialog();
        }
        private void debitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDebitNote FormDebitNote = new FormDebitNote();
            FormDebitNote.ShowDialog();
        }
        private void createInvoiceMenuItem_Click(object sender, EventArgs e)
        {
            FormAdjustInventory InventoryAdjustmentEntry = new FormAdjustInventory();
            InventoryAdjustmentEntry.ShowDialog();
        }
        private void chartOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChartOfAccounts FormChartOfAccounts = new FormChartOfAccounts();
            FormChartOfAccounts.ShowDialog(this);
        }
        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmployee FormEmployees = new FormEmployee(this);
            FormEmployees.ShowDialog(this);
        }
        private void patientToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PatientRegistration PatientRegistration = new PatientRegistration(this);
            PatientRegistration.ShowDialog(this);
        }
        private void registrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOPRegistration FormOPRegistration = new FormOPRegistration(this);
            FormOPRegistration.ShowDialog();
        }
        private void wardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWardAndBed FormWardAndBed = new FormWardAndBed();
            FormWardAndBed.ShowDialog();
        }
        private void bedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBedType FormBedType = new FormBedType(this);
            FormBedType.ShowDialog();
        }
        private void manageCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCatalog FormCatalog = new FormCatalog(this);
            FormCatalog.ShowDialog();
        }
        private void expenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExpense FormExpense = new FormExpense();
            FormExpense.ShowDialog();
        }
        private void purchaseEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseEntryNew FormPurchaseEntryNew = new FormPurchaseEntryNew();
            FormPurchaseEntryNew.ShowDialog();
            //FormPurchaseEntry FormPurchaseEntry = new FormPurchaseEntry();
            //FormPurchaseEntry.ShowDialog();
        }
        private void oPQueueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormOPQueueForDoctor FormOPQueue = new FormOPQueueForDoctor();
            FormOPQueue.ShowDialog();
        }
        private void symptomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSymptom FormSymptom = new FormSymptom();
            FormSymptom.ShowDialog();
        }
        private void medicalTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMedicalTest FormMedicalTest = new FormMedicalTest();
            FormMedicalTest.ShowDialog();
        }
        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormItembasedSales FormSales = new FormItembasedSales();
            if (FormSales.IsDisposed)
            {
                FormSales = null;
            }
            else
            {
                FormSales.ShowDialog();
            }
        }
        private void reportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormItemReport ItemReports = new FormItemReport();
            ItemReports.ShowDialog();
        }
        private void quoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuote FormSaleQuote = new FormQuote();
            if (FormSaleQuote.IsDisposed)
            {
                FormSaleQuote = null;
            }
            else
            {
                FormSaleQuote.ShowDialog();
            }
        }
        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataMigrator Jarvis = new FormDataMigrator(this);
            Jarvis.ShowDialog();
        }
        private void returnToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormSalesReturn SalesReturn = new FormSalesReturn();
            if (SalesReturn.IsDisposed)
            {
                SalesReturn = null;
            }
            else
            {
                SalesReturn.ShowDialog();
            }
        }
        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPOSReceivePayment ReceivePymt = new FormPOSReceivePayment();
            ReceivePymt.ShowDialog();
        }
        private void workStationsetupStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWorkStationSetup FormWorkStationSetup = new FormWorkStationSetup();
            FormWorkStationSetup.ShowDialog();
        }
        private void DeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaleDelivery FormSaleDelivery = new FormSaleDelivery();
            FormSaleDelivery.ShowDialog();
        }
        private void BillMenuItem_Click(object sender, EventArgs e)
        {
            FormBill FormBill = new FormBill();
            FormBill.ShowDialog();
        }
        private void InvoiceMenuItem_Click(object sender, EventArgs e)
        {
            FormInvoice FormInvoice = new FormInvoice();
            FormInvoice.ShowDialog();
        }
        private void daybookMenuItem_Click(object sender, EventArgs e)
        {
            FormDaybook FormDaybook = new FormDaybook();
            FormDaybook.ShowDialog();
        }
        private void paymentTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPaymentTerm FormPaymentTerm = new FormPaymentTerm();
            FormPaymentTerm.ShowDialog();
        }
        private void paymentMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPaymentMethod FormPaymentMethod = new FormPaymentMethod();
            FormPaymentMethod.ShowDialog();
        }
        private void trialBalanceMenuItem_Click(object sender, EventArgs e)
        {
            FormTrialBalance TrialBalance = new FormTrialBalance();
            TrialBalance.ShowDialog();
        }
        private void manageItemTaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTaxCode FormTaxCode = new FormTaxCode();
            FormTaxCode.ShowDialog();
        }
        private void saleReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormSalesReport SalesReport = new FormSalesReport();
            SalesReport.ShowDialog();
        }
        private void gstReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSalesGstrReport FormSalesGstrReport = new FormSalesGstrReport();
            FormSalesGstrReport.ShowDialog();
        }
        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseReport FormPurchaseReport = new FormPurchaseReport();
            FormPurchaseReport.ShowDialog();
        }
        private void GstrReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseGstrReport FormPurchaseGstrReport = new FormPurchaseGstrReport();
            FormPurchaseGstrReport.ShowDialog();
        }
        private void NursestechOPQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOPQueue FormOPQueue = new FormOPQueue();
            FormOPQueue.ShowDialog();
        }
        private void IPQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIPQueue FormIpQueueForDoctor = new FormIPQueue
            {
                IPQueueMode = EnumIPQueueMode.DOCTOR_VIEW
            };
            FormIpQueueForDoctor.ShowDialog();
        }
        private void ConsultationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConsultations FormConsultations = new FormConsultations();
            FormConsultations.ShowDialog();
        }
        private void ReceiveFeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReceiveAmount FormReceiveFee = new FormReceiveAmount();
            FormReceiveFee.ShowDialog();
        }
        private void ConsultingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientSearchForConsulting FormPatientSearchForConsulting = new FormPatientSearchForConsulting(this);
            FormPatientSearchForConsulting.PatientSearchType = PatientSearchType.SearchPatient;
            FormPatientSearchForConsulting.ShowDialog();
        }
        private void OpReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOpReport FormOpReport = new FormOpReport();
            FormOpReport.ShowDialog();
        }
        private void PatientLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientLedger FormPatientLedger = new FormPatientLedger();
            FormPatientLedger.ShowDialog();
        }
        private void InPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIPQueue FormIpQueueForNurse = new FormIPQueue
            {
                IPQueueMode = EnumIPQueueMode.NURSE_VIEW
            };
            FormIpQueueForNurse.ShowDialog();
        }
        private void InventoryLocationToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormInventoryLocations FormInventoryLocations = new FormInventoryLocations();
            FormInventoryLocations.ShowDialog();
        }
        private void InventoryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStockReportNew FormStockReportNew = new FormStockReportNew();
            FormStockReportNew.ShowDialog();
        }
        private void PurchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseReturn FormPurchaseReturn = new FormPurchaseReturn();
            FormPurchaseReturn.ShowDialog();
        }
        private void PurchaseReturnReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseReturnReport FormPurchaseReturnReport = new FormPurchaseReturnReport();
            FormPurchaseReturnReport.ShowDialog();
        }
        private void CurrentStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormItemLedger FormItemLedger = new FormItemLedger();
            FormItemLedger.ShowDialog();
        }
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHospitalSettings FormHospitalSettings = new FormHospitalSettings(this);
            FormHospitalSettings.ShowDialog();
        }
        private void IPReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIPReport FormIpReport = new FormIPReport();
            FormIpReport.ShowDialog();
        }
        private void TransactionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAccountTransactions FormAccountTransactions = new FormAccountTransactions();
            FormAccountTransactions.ShowDialog();
        }
        private void intraStockMovementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIntraStockMovement FormIntraStockMovement = new FormIntraStockMovement();
            FormIntraStockMovement.ShowDialog();
        }

        private void intraStockReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIntraStockReceive FormIntraStockReceive = new FormIntraStockReceive();
            FormIntraStockReceive.ShowDialog();
        }
        private void intraStockRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIntraStockRequest FormIntraStockRequest = new FormIntraStockRequest();
            FormIntraStockRequest.ShowDialog();
        }
        private void priceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPriceList FormPriceList = new FormPriceList();
            FormPriceList.ShowDialog();
        }

        private void medicalProceduresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProcedures ProcedureMaster = new FormProcedures(false);
            ProcedureMaster.ShowDialog();
        }

        private void reAssignCareTakerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormAssignCareTaker FormAssignCareTaker = new FormAssignCareTaker();
            FormAssignCareTaker.ShowDialog();
        }
        private void transferPatientToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormTransferPatient FormTransferPatient = new FormTransferPatient(this);
            FormTransferPatient.ShowDialog();
        }

        private void dischargePatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormIPQueue FormIpQueueForDoctor = new FormIPQueue
            {
                IPQueueMode = EnumIPQueueMode.DOCTOR_VIEW,
                DischargePatientOnload = true
            };
            FormIpQueueForDoctor.ShowDialog();
        }

        private void performProceduresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientSearchForConsulting FormPatientSearchForConsulting = new FormPatientSearchForConsulting(this);
            FormPatientSearchForConsulting.PatientSearchType = PatientSearchType.PatientProcedure;
            FormPatientSearchForConsulting.ShowDialog();
        }
        private void damageEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDamageEntry FormDamageEntry = new FormDamageEntry();
            FormDamageEntry.ShowDialog();
        }

        private void stockRequestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStockRequestReport FormStockRequestReport = new FormStockRequestReport();
            FormStockRequestReport.ShowDialog();
        }

        private void expeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExpiryReport FormExpiryReport = new FormExpiryReport();
            FormExpiryReport.ShowDialog();
        }

        private void patientDueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientDueList FormPatientDueList = new FormPatientDueList();
            FormPatientDueList.ShowDialog();
        }

        private void saleReturnReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSaleReturnReport FormSaleReturnReport = new FormSaleReturnReport();
            FormSaleReturnReport.ShowDialog();
        }

        private void purchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseOrder FormPurchaseOrder = new FormPurchaseOrder();
            FormPurchaseOrder.ShowDialog();
        }

        private void wardAndBedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWardBedReport FormWardBedReport = new FormWardBedReport();
            FormWardBedReport.ShowDialog();
        }

        private void inventoryUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInventoryUpload FormInventoryUpload = new FormInventoryUpload();
            FormInventoryUpload.ShowDialog();
        }

        private void deliveryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDeliveryReport FormDeliveryReport = new FormDeliveryReport();
            FormDeliveryReport.ShowDialog();
        }

        private void damageEntryReportToolStrip_Click(object sender, EventArgs e)
        {
            FormDamageEntryReport formDamageEntryReport = new FormDamageEntryReport();
            formDamageEntryReport.ShowDialog();
        }

        private void stockReciveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FromStockreceivereports intraStockreceivereports = new FromStockreceivereports();
            intraStockreceivereports.ShowDialog();
        }

        private void stockMovementReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStockMovementReports formStockMovementReports = new FormStockMovementReports();
            formStockMovementReports.ShowDialog();
        }

        private void adjustmentEntryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAdjustmentEntryReport FormAdjustmentEntryReport = new FormAdjustmentEntryReport();
            FormAdjustmentEntryReport.ShowDialog();
        }

        private void paymentReceiveReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FromReceivePaymentReport ReceivePaymentReport = new FromReceivePaymentReport();
            ReceivePaymentReport.ShowDialog();
        }
        private void patientTransferReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientTransferReport FormPatientTransferReport = new FormPatientTransferReport();
            FormPatientTransferReport.ShowDialog();
        }

        private void patientProcedureReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientProcedureReport formPatientProcedureReport = new FormPatientProcedureReport();
            formPatientProcedureReport.ShowDialog();
        }

        private void labTestReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLabTestReport formLabTestReport = new FormLabTestReport();
            formLabTestReport.ShowDialog();
        }

        private void patientDischargeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FromDischargePatientDetailReport FromDischargePatientDetailReport = new FromDischargePatientDetailReport();
            FromDischargePatientDetailReport.ShowDialog();
        }

        private void careTakerAssignReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCareTakerAssignReport formCareTakerAssignReport = new FormCareTakerAssignReport();
            formCareTakerAssignReport.ShowDialog();
        }

        private void quoteReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormQuoteReport FormQuoteReport = new FormQuoteReport();
            FormQuoteReport.ShowDialog();
        }

        private void careTakerUnAssignReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCareTakerUnAssignedReport formCareTakerUnAssignedReport = new FormCareTakerUnAssignedReport();
            formCareTakerUnAssignedReport.ShowDialog();
        }

        private void patientVisitCountingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientVisitCountingReports formPatientVisitCountingReports = new FormPatientVisitCountingReports();
            formPatientVisitCountingReports.ShowDialog();
        }

        private void careTakerUnAssignedReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCareTakerUnAssignedReport formCareTakerUnAssignedReport = new FormCareTakerUnAssignedReport();
            formCareTakerUnAssignedReport.ShowDialog();
        }

        private void patientUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientUpload formPatientUpload = new FormPatientUpload();
            formPatientUpload.ShowDialog();
        }

        private void feeCollectionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFeeCollectionReport formFeeCollectionReport = new FormFeeCollectionReport();
            formFeeCollectionReport.ShowDialog();
        }

        private void patientDetailsReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPatientDetailsReport formPatientDetailsReport = new FormPatientDetailsReport();
            formPatientDetailsReport.ShowDialog();
        }

        private void purchaseOrderReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPurchaseOrderReport formPurchaseOrderReport = new FormPurchaseOrderReport();
            formPurchaseOrderReport.ShowDialog();
        }

        private void ButtonBackward_Click(object sender, EventArgs e)
        {
            if (currentView == CalendarView.Week)
            {
                currentWeekStart = currentWeekStart.AddDays(-7);
                UpdateHeader(currentView, currentWeekStart);
                lableMonth.Text = GetWeekRangeText(currentWeekStart);
                CreateTimeSlots();
            }
            else
            {
                currentDay = currentDay.AddDays(-1);
                UpdateHeader(currentView, currentDay);
                lableMonth.Text = currentDay.ToString("MMMM") + " - " + currentDay.Day.ToString() + ", " + currentDay.Year.ToString();
            }
            LoadAppointmentsIntoGrid();
        }

        private void ButtonForward_Click(object sender, EventArgs e)
        {
            if (currentView == CalendarView.Week)
            {
                currentWeekStart = currentWeekStart.AddDays(7);
                UpdateHeader(currentView, currentWeekStart);
                lableMonth.Text = GetWeekRangeText(currentWeekStart);
                CreateTimeSlots();
                LoadAppointmentsIntoGrid();
            }
            else
            {
                currentDay = currentDay.AddDays(1);
                UpdateHeader(currentView, currentDay);
                LoadAppointmentsIntoGrid();
                lableMonth.Text = currentDay.ToString("MMMM") + " - " + currentDay.Day.ToString() + ", " + currentDay.Year.ToString();
            }
        }

        private void buttonToday_Click(object sender, EventArgs e)
        {
            currentDay = DateTime.Today;
            if (currentView == CalendarView.Week)
            {
                currentWeekStart = GetStartOfWeek(DateTime.Today);
                UpdateHeader(currentView, currentWeekStart);
                lableMonth.Text = GetWeekRangeText(currentWeekStart);
                CreateTimeSlots();
                LoadAppointmentsIntoGrid();
            }
            else
            {
                UpdateHeader(currentView, currentDay);
                LoadAppointmentsIntoGrid();
                lableMonth.Text = currentDay.ToString("MMMM") + " - " + currentDay.Day.ToString() + ", " + currentDay.Year.ToString();
            }
        }

        private void CreateTimeSlots()
        {
            if (GridviewWeekAppointment.Visible)
            {
                GridviewWeekAppointment.Rows.Clear();

                DateTime startTime = DateTime.Today;
                DateTime endTime = startTime.AddDays(1);

                DateTime now = DateTime.Now;
                int currentMinutes = (now.Hour * 60) + now.Minute;
                int roundedMinutes = (currentMinutes / 15) * 15;
                DateTime currentSlotTime = DateTime.Today.AddMinutes(roundedMinutes);

                int targetRowIndex = -1;
                int rowIndex = 0;

                while (startTime < endTime)
                {
                    string timeText = startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    rowIndex = GridviewWeekAppointment.Rows.Add();
                    GridviewWeekAppointment.Rows[rowIndex].Cells[0].Value = timeText;

                    if (startTime == currentSlotTime)
                    {
                        targetRowIndex = rowIndex;
                    }

                    startTime = startTime.AddMinutes(15);
                }

                GridviewWeekAppointment.RowTemplate.Height = 40;

                if (targetRowIndex >= 0)
                {
                    GridviewWeekAppointment.FirstDisplayedScrollingRowIndex = targetRowIndex;
                    GridviewWeekAppointment.Rows[targetRowIndex].Selected = true;
                }
            }
        }


        private void GridviewWeekAppointment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            foreach (var appt in lWeekAppointments)
            {
                if (e.ColumnIndex == appt.ColumnIndex &&
                    e.RowIndex >= appt.RowIndex &&
                    e.RowIndex < appt.RowIndex + appt.RowSpan)
                {
                    if (e.RowIndex == appt.RowIndex)
                    {
                        Rectangle mergedRect = e.CellBounds;

                        for (int i = 1; i < appt.RowSpan; i++)
                        {
                            if (appt.RowIndex + i < GridviewWeekAppointment.RowCount)
                            {
                                Rectangle nextCell = GridviewWeekAppointment.GetCellDisplayRectangle(appt.ColumnIndex, appt.RowIndex + i, true);
                                mergedRect.Height += nextCell.Height;
                            }
                        }
                        using (Brush backColorBrush = new SolidBrush(SystemColors.ControlLight))
                        {
                            e.Graphics.FillRectangle(backColorBrush, mergedRect);
                        }

                        using (Pen pen = new Pen(Color.Gray, 0.1f))
                        {
                            e.Graphics.DrawLine(pen, mergedRect.Right - 1, mergedRect.Top, mergedRect.Right - 1, mergedRect.Bottom);
                            e.Graphics.DrawLine(pen, mergedRect.Left, mergedRect.Bottom - 1, mergedRect.Right, mergedRect.Bottom - 1);
                        }

                        TextRenderer.DrawText(
                            e.Graphics,
                            appt.PatientName + "\n" + appt.ReasonForVisit,
                            e.CellStyle.Font,
                            mergedRect,
                            Color.Black,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak
                        );

                        if (appt.IsConsulted)
                        {
                            using (Pen strikePen = new Pen(Color.FromArgb(150, 255, 0, 0), 1.8f))
                            {
                                TextRenderer.DrawText(
                                    e.Graphics,
                                    "CONSULTED",
                                    new Font(e.CellStyle.Font, FontStyle.Bold),
                                    new System.Drawing.Point(mergedRect.Right - 70, mergedRect.Top + 5),
                                    Color.FromArgb(150, 255, 0, 0));
                            }
                        }
                    }
                    e.Handled = true;
                }
            }
        }
        private void UpdateHeader(CalendarView viewMode, DateTime startDate)
        {
            GridviewWeekAppointment.Columns.Clear();
            GridviewWeekAppointment.Columns.Add("Time", "Time");

            if (viewMode == CalendarView.Week)
            {
                for (int i = 0; i < 7; i++)
                {
                    DateTime date = startDate.AddDays(i);
                    string headerText = date.ToString("dddd") + "\n" + date.ToString("dd");
                    GridviewWeekAppointment.Columns.Add("col" + i, headerText);
                }
            }
            else
            {
                string headerText = startDate.ToString("dddd") + "\n" + startDate.ToString("dd");
                GridviewWeekAppointment.Columns.Add("colDay", headerText);
            }
            GridviewWeekAppointment.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            GridviewWeekAppointment.Columns["Time"].Width = 80;
            GridviewWeekAppointment.Columns["Time"].SortMode = DataGridViewColumnSortMode.NotSortable;
            GridviewWeekAppointment.Columns["Time"].ReadOnly = true;
            GridviewWeekAppointment.Columns["Time"].Resizable = DataGridViewTriState.False;

            for (int i = 1; i < GridviewWeekAppointment.Columns.Count; i++)
            {
                GridviewWeekAppointment.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                GridviewWeekAppointment.Columns[i].ReadOnly = true;
                GridviewWeekAppointment.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                GridviewWeekAppointment.Columns[i].Resizable = DataGridViewTriState.False;
            }
            GridviewWeekAppointment.RowTemplate.Height = 40;
        }

        private void GridviewDayAppointment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 0 || GridviewDayAppointment.Rows.Count == 0)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                var consultantCell = GridviewDayAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (consultantCell.Value == null)
                {
                    MessageBox.Show("No consultant information available", "Information",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ConsultantName = consultantCell.Value.ToString()!;
                var consultantIdCell = GridviewDayAppointment.Rows[e.RowIndex].Cells[97];
                ConsultantID = long.Parse(GridviewDayAppointment.Rows[e.RowIndex].Cells[97].Value.ToString()!);
                if (consultantIdCell.Value == null || !long.TryParse(consultantIdCell.Value.ToString(), out long consultantId))
                {
                    MessageBox.Show("Could not identify consultant", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                GridviewDayAppointment.Visible = false;
                GridviewWeekAppointment.Visible = true;
                currentView = CalendarView.Week;
                currentWeekStart = GetStartOfWeek(currentDay);
                UpdateHeader(currentView, currentWeekStart);
                CreateTimeSlots();

                LoadAppointmentsIntoGrid();

                lableMonth.Text = GetWeekRangeText(currentWeekStart);
            }
            catch (Exception ex)
            {
                GridviewDayAppointment.Visible = true;
                GridviewWeekAppointment.Visible = false;
                currentView = CalendarView.Day;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                if (GridviewWeekAppointment.Visible && !User.Roles.Any(x => x.Name.Equals("Doctor")) && !User.Roles.Any(x => x.Name.Equals("Nurse")))
                {
                    GridviewWeekAppointment.Visible = false;

                    GridviewDayAppointment.Visible = true;
                    currentView = CalendarView.Day;
                    UpdateHeader(currentView, currentDay);
                    CreateTimeSlots();
                    LoadAppointmentsIntoGrid();
                    lableMonth.Text = currentDay.ToString("MMMM") + " - " + currentDay.Day.ToString() + ", " + currentDay.Year.ToString();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenAppointment(int rowIndex, int columnIndex, bool isReschedule, DataGridView gridView, bool isWeekGrid)
        {
            if (gridView == null || rowIndex < 0 || columnIndex <= 0 || (isWeekGrid ? columnIndex >= 8 : columnIndex >= ConsultantIdColumnIndexInDayGrid))
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Application.DoEvents();

                long consultantId;
                string timeSlot;

                if (isWeekGrid)
                {
                    consultantId = ConsultantID;
                    timeSlot = gridView.Rows[rowIndex].Cells[0].Value?.ToString()!;
                }
                else
                {
                    var consultantCell = gridView.Rows[rowIndex].Cells[ConsultantIdColumnIndexInDayGrid];
                    if (consultantCell?.Value == null || !long.TryParse(consultantCell.Value.ToString(), out consultantId))
                    {
                        MessageBox.Show("Invalid consultant information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    timeSlot = gridView.Columns[columnIndex].HeaderText;
                }

                if (string.IsNullOrWhiteSpace(timeSlot))
                {
                    MessageBox.Show("Invalid time slot", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var consultant = EmployeeManager.Instance.GetEmployeeInfoById(consultantId);
                if (consultant == null)
                {
                    MessageBox.Show("Consultant not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var cell = gridView.Rows[rowIndex].Cells[columnIndex];
                PatientAppointment existingAppointment = null!;

                if (cell?.Tag is long appointmentId)
                {
                    existingAppointment = PatientAppointmentManager.Instance.GetAppointmentById(appointmentId);
                    if (existingAppointment == null)
                    {
                        MessageBox.Show("Appointment not found in database", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cell.Tag = null;
                    }
                }

                using (var formAppointment = new FormAppointment())
                {
                    formAppointment.AppointmentDateFrom = (existingAppointment?.FromDateOfAppointment != null && existingAppointment.FromDateOfAppointment != DateTime.MinValue) ? existingAppointment.FromDateOfAppointment : currentDay;
                    formAppointment.AppointmentDateTo = (existingAppointment?.ToDateOfAppointment != null && existingAppointment.ToDateOfAppointment != DateTime.MinValue) ? existingAppointment.ToDateOfAppointment : currentDay;
                    formAppointment.StartingTime = existingAppointment?.StartingTime ?? timeSlot;
                    formAppointment.EndTime = existingAppointment?.EndTime ?? "0";
                    formAppointment.ConsultantID = existingAppointment?.ConsultantId ?? consultantId;
                    formAppointment.AppointmentID = existingAppointment?.Id ?? 0;
                    formAppointment.PatientID = existingAppointment?.PatientId ?? 0;
                    formAppointment.ReasonforVisit = existingAppointment?.ReasonForTheAppointment ?? "";
                    formAppointment.isReschedule = isReschedule;

                    formAppointment.Text = existingAppointment != null
                        ? $"Edit Appointment - {timeSlot} with {consultant.Name}"
                        : $"New Appointment - {timeSlot} with {consultant.Name}";

                    if (formAppointment.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAppointmentsIntoGrid();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void GridviewDayAppointment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = GridviewDayAppointment.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (currentDay.Day >= DateTime.Today.Day)
            {
                OpenAppointment(e.RowIndex, e.ColumnIndex, false, GridviewDayAppointment, false);
            }
            else if (cell?.Tag is long appointmentId)
            {
                MessageBox.Show(NotAllowToOpenPastAppointmentText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(NotAllowToCreatePastDaysText, "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GridviewDayAppointment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            foreach (var appt in lappointments)
            {
                int span = appt.DurationMinutes / 15;
                if (e.RowIndex == appt.RowIndex &&
                    e.ColumnIndex >= appt.ColumnIndex &&
                    e.ColumnIndex < appt.ColumnIndex + span)
                {
                    if (e.ColumnIndex == appt.ColumnIndex)
                    {
                        Rectangle cellRect = e.CellBounds;

                        for (int i = 1; i < span; i++)
                        {
                            if (appt.ColumnIndex + i < GridviewDayAppointment.ColumnCount)
                            {
                                Rectangle nextCell = GridviewDayAppointment.GetCellDisplayRectangle(appt.ColumnIndex + i, appt.RowIndex, true);
                                cellRect.Width += nextCell.Width;
                            }
                        }

                        using (Brush backColorBrush = new SolidBrush(SystemColors.ControlLight))
                        {
                            e.Graphics.FillRectangle(backColorBrush, cellRect);
                        }

                        using (Pen borderPen = new Pen(Color.Gray, 0.1F))
                        {
                            e.Graphics.DrawLine(borderPen, cellRect.Right - 1, cellRect.Top, cellRect.Right - 1, cellRect.Bottom);
                            e.Graphics.DrawLine(borderPen, cellRect.Left, cellRect.Bottom - 1, cellRect.Right, cellRect.Bottom - 1);
                        }

                        TextRenderer.DrawText(
                            e.Graphics,
                            appt.PatientName + "\n" + appt.ReasonForVisit,
                            e.CellStyle.Font,
                            cellRect,
                            Color.Black,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak
                        );
                    }

                    e.Handled = true;
                }
            }
        }

        private void barCodeLabelReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBarCodeCounterSetUp formBarCodeCounterSetUp = new FormBarCodeCounterSetUp();
            formBarCodeCounterSetUp.ShowDialog();
        }

        private void itemSalesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormItemSalesReport formItemSalesReport = new FormItemSalesReport(0, "", DateTime.Today.AddMonths(-1), DateTime.Today);
            formItemSalesReport.ShowDialog();
        }

        private void priceViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductPriceSeeker formProductPriceSeeker = new FormProductPriceSeeker();
            formProductPriceSeeker.ShowDialog();
        }

        private void salesNoTaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSalesEntryWithoutTax formSalesEntryWithoutTax = new FormSalesEntryWithoutTax();
            formSalesEntryWithoutTax.ShowDialog();
        }

        private void uOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormModifyXFactor formModifyXFactor = new FormModifyXFactor();
            //formModifyXFactor.ShowDialog();
        }
    }
    public class DayAppointment
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public int DurationMinutes { get; set; }
        public string ReasonForVisit { get; set; }
        public string PatientName { get; set; }
    }
    public class WeekAppointment
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public int RowSpan { get; set; }
        public string ReasonForVisit { get; set; }
        public string PatientName { get; set; }
        public bool IsContinuation { get; set; }
        public bool IsConsulted { get; set; }
        public long AppointmentId { get; set; }
    }
}
