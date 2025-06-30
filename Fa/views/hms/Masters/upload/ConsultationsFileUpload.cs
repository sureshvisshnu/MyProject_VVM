using ExcelDataReader;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.model.Employee;
using fa.model.Hms.Master;
using FADataAccessLibrary.Api.Hms;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace fa.views.hms.masters.upload
{
    public partial class ConsultationsFileUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;

        public ConsultationsFileUpload()
        {
            InitializeComponent();
        }
        private void ExitFileUploadProcess()
        {
            if (FileUploadConsultation.WorkFlow == true)
            {
                FileUploadConsultation.EnableExitButton(false);
                PauseProcessing = true;
            }
            else
            {
                this.Close();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                ExitFileUploadProcess();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FileUploadConsultation_OnClickExit(object sender, EventArgs e)
        {
            ExitFileUploadProcess();
        }

        private void FileUploadConsultation_OnClickUpload(object sender, EventArgs e)
        {
            string FileName = FileUploadConsultation.FileName();
            bool HasHeader = FileUploadConsultation.HasHeader();
            bool IsColumnValid = true;
            double Fees = 0;
            string[] ConsultationsHeader = new string[] { "Name", "DisplayName", "Description", "ServiceProviders", "Fees", "DOB", "Title" };
            Stopwatch stopw = new Stopwatch();
            using (var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    FileUploadComplete = true;
                    int rowcount = reader.RowCount;
                    stopw.Start();
                    this.UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                    FileUploadConsultation.SetProgressMax(rowcount);
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            if (FileUploadConsultation.PauseWaitCursor == true)
                            {
                                this.UseWaitCursor = false;
                            }
                            else
                            {
                                this.UseWaitCursor = true;
                            }
                            if (PauseProcessing == true)
                            {
                                this.UseWaitCursor = false;
                                FileUploadConsultation.AddAccessLog("Paused");
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    FileUploadConsultation.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    this.UseWaitCursor = true;
                                    FileUploadConsultation.AddAccessLog("Continuing");
                                    PauseProcessing = false;
                                    FileUploadConsultation.StopProcessing = false;
                                    FileUploadConsultation.EnableExitButton(true);
                                    continue;
                                }
                            }
                            if (FileUploadConsultation.StopProcessing == false)
                            {
                                string ConsultationName = "";
                                string DisplayName = "";
                                string Description = "";
                                string employee = "";
                                string DOB = "";
                                string Title = "";
                                int fc = 0;
                                try
                                {
                                    if (ConsultationsHeader.Length == reader.FieldCount)
                                    {
                                        if (row == 1 && HasHeader)
                                        {
                                            foreach (string sh in ConsultationsHeader)
                                            {
                                                if (sh != reader.GetValue(fc).ToString())
                                                {
                                                    IsColumnValid = false;
                                                }
                                                fc++;
                                            }
                                            row++;
                                            continue;
                                        }
                                        if (IsColumnValid)
                                        {
                                            FileUploadConsultation.IncrementProgress();
                                            ConsultationName = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                            DisplayName = reader.GetValue(1) != null ? reader.GetValue(1).ToString()! : string.Empty;
                                            Description = reader.GetValue(2) != null ? reader.GetValue(2).ToString()! : string.Empty;
                                            employee = reader.GetValue(3) != null ? reader.GetValue(3).ToString()! : string.Empty;
                                            dynamic value = reader.GetValue(4) != null ? reader.GetValue(4) : 0;
                                            if (double.TryParse(value.ToString(), out double feesValue))
                                            {
                                                Fees = feesValue;
                                            }
                                            else
                                            {
                                                Fees = 0;
                                            }
                                            DOB = reader.GetValue(5) != null ? reader.GetValue(5).ToString()!.Replace("00:00:00", "")!.Trim() : string.Empty;
                                            Title = reader.GetValue(6) != null ? reader.GetValue(6).ToString()! : string.Empty;
                                            if (!string.IsNullOrEmpty(ConsultationName))
                                            {
                                                Consultation Consultations = new model.Hms.Master.Consultation
                                                {
                                                    CompanyId = Global.Company.CompanyId,
                                                    Name = ConsultationName,
                                                    DisplayAs = DisplayName,
                                                    Discription = Description,
                                                };
                                                Consultation ConsultationById = null!;
                                                if (ConsultationManager.Instance.ConsultationNameUniqueById(Consultations))
                                                {
                                                    if (Consultations.Id == 0)
                                                    {
                                                        ConsultationById = ConsultationManager.Instance.AddConsultation(Consultations);
                                                    }
                                                }
                                                else
                                                {
                                                    Consultation lConsultationById = ConsultationManager.Instance.GetConsultationByName(Consultations.Name, Global.Company.CompanyId);
                                                    if (lConsultationById != null)
                                                    {
                                                        Consultations.Id = lConsultationById.Id;
                                                        ConsultationById = ConsultationManager.Instance.UpdateConsultation(Consultations);
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(employee))
                                                {
                                                    Department department = new Department()
                                                    {
                                                        CompanyId = Global.Company.CompanyId,
                                                        Name = "Service Providers",
                                                        DisplayAs = "Service Providers",
                                                        Discription = "",
                                                        IsSubDepartment = false
                                                    };
                                                    Department departmentById = null!;
                                                    if (DepartmentManager.Instance.DepartmentNameUniqueById(department))
                                                    {
                                                        if (department.Id == 0)
                                                        {
                                                            departmentById = DepartmentManager.Instance.AddDepartment(department);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Department ldepartmentById = DepartmentManager.Instance.GetDepartmentByName(department.Name, Global.Company.CompanyId);
                                                        if (ldepartmentById != null)
                                                        {
                                                            department.Id = ldepartmentById.Id;
                                                            departmentById = DepartmentManager.Instance.UpdateDepartment(department);
                                                        }
                                                    }

                                                    Title title = new Title()
                                                    {
                                                        CompanyId = Global.Company.CompanyId,
                                                        Name = !string.IsNullOrEmpty(Title) ? Title : "Doctor",
                                                        DisplayAs = Title,
                                                        Discription = ""
                                                    };
                                                    Title titleById = null!;
                                                    if (TitleManager.Instance.TitleNameUniqueById(title))
                                                    {
                                                        if (title.Id == 0)
                                                        {
                                                            titleById = TitleManager.Instance.AddTitles(title);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Title ltitleById = TitleManager.Instance.GetTitleInfoByName(title.Name, Global.Company.CompanyId);
                                                        if (ltitleById != null)
                                                        {
                                                            title.Id = ltitleById.Id;
                                                            titleById = TitleManager.Instance.UpdateTitle(title);
                                                        }
                                                    }

                                                    Employee Employee = new Employee();
                                                    {
                                                        Employee.CompanyId = Global.Company.CompanyId;
                                                        Employee.Name = employee;
                                                        Employee.DepartmentId = departmentById.Id;
                                                        Employee.TitleId = titleById.Id;
                                                        DateTime Date;
                                                        if (DateTime.TryParseExact(DOB.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Date))
                                                        {
                                                            Employee.DateOfBirth = Date.Date;
                                                        }
                                                        else
                                                        {
                                                            Employee.DateOfBirth = DateTime.Now.Date;
                                                        }
                                                        Employee.IsServiceProvider = true;
                                                    }
                                                    Employee employeeById = null!;
                                                    if (EmployeeManager.Instance.EmployeeNameUniqueById(Employee))
                                                    {
                                                        if (Employee.Id == 0)
                                                        {
                                                            Address address = new Address();
                                                            address.StatesId = Global.Company.Address.StatesId!;
                                                            ContactInfo contactInfo = new ContactInfo();
                                                            TaxInfo taxInfo = new TaxInfo();
                                                            Employee.Address = address;
                                                            Employee.ContactInfo = contactInfo;
                                                            Employee.TaxInfo = taxInfo;
                                                            employeeById = EmployeeManager.Instance.AddEmployee(Employee);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Employee lemployeeById = EmployeeManager.Instance.GetEmployeeByName(employee, Global.Company.CompanyId);
                                                        if (lemployeeById != null)
                                                        {
                                                            Employee.Id = lemployeeById.Id;
                                                            Employee.AddressId = lemployeeById.AddressId;
                                                            Employee.ContactInfoId = lemployeeById.ContactInfoId;
                                                            Employee.TaxInfoId = lemployeeById.TaxInfoId;
                                                            employeeById = EmployeeManager.Instance.UpdateEmployee(Employee);
                                                        }
                                                    }

                                                    ConsultationDetail consultationDetail = new ConsultationDetail();
                                                    {
                                                        consultationDetail.CompanyId = Global.Company.CompanyId;
                                                        consultationDetail.ConsultationId = ConsultationById.Id;
                                                        consultationDetail.EmployeeId = employeeById.Id;
                                                        consultationDetail.Fee = Fees;
                                                    }
                                                    ConsultationDetail lDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, employeeById.Id, ConsultationById);
                                                    if (lDetail == null)
                                                    {
                                                        ConsultationDetailManager.Instance.AddConsultationDetail(consultationDetail);
                                                    }
                                                    else
                                                    {
                                                        ConsultationDetailManager.Instance.UpdateConsultationDetail(consultationDetail);
                                                    }
                                                    if (!DateTime.TryParseExact(DOB.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lDate))
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            FileUploadConsultation.AddAccessLog("Processing Row No : " + (row - 1) + " - " + "Check the date format as dd/MM/yyyy");
                                                            FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            FileUploadConsultation.AddAccessLog("Processing Row No : " + row + " - " + "Check the date format as dd/MM/yyyy");
                                                            FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        continue;
                                                    }
                                                    if (HasHeader)
                                                    {
                                                        FileUploadConsultation.AddAccessLog("Processing Row No : " + (row - 1) + " - " + ConsultationName);
                                                        FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else
                                                    {
                                                        FileUploadConsultation.AddAccessLog("Processing Row No : " + row + " - " + ConsultationName);
                                                        FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    if (HasHeader)
                                                    {
                                                        FileUploadConsultation.AddErrorLog("Row No - " + (row - 1) + " : Employee Name is Empty");
                                                        FileUploadConsultation.AddTimingLog(stopw.Elapsed.ToString());
                                                    }
                                                    else
                                                    {
                                                        FileUploadConsultation.AddErrorLog("Row No - " + row + " : Employee Name is Empty");
                                                        FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (HasHeader)
                                                {
                                                    FileUploadConsultation.AddErrorLog("Row No - " + (row - 1) + " Empty Consultation Name");
                                                    FileUploadConsultation.AddTimingLog(stopw.Elapsed.ToString());
                                                }
                                                else
                                                {
                                                    FileUploadConsultation.AddErrorLog("Row No - " + row + " Empty Consultation Name");
                                                    FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            FileUploadConsultation.AddAccessLog(FileUploadConsultation.FileUploadMismatchColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        FileUploadConsultation.AddAccessLog(FileUploadConsultation.FileUploadMismatchColoumn);
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.HResult.ToString() == "-2146233080")
                                    {
                                        FileUploadComplete = false;
                                        FileUploadConsultation.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;//from the Form/Window instance
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            FileUploadConsultation.AddErrorLog("Row No : " + (row - 1) + " - " + Name + " : " + ex.InnerException!.Message);
                                            FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            FileUploadConsultation.AddErrorLog("Row No : " + row + " - " + Name + " : " + ex.InnerException!.Message);
                                            FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                    }
                                }
                                row++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (reader.NextResult());
                    if (IsBreak == false) { FileUploadConsultation.CompleteIncrementProgress(row); } else { FileUploadConsultation.CompleteIncrementProgress(0); }
                    FileUploadConsultation.CompleteIncrementProgress(row);
                    FileUploadConsultation.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (FileUploadConsultation.StopProcessing == true)
                    {
                        FileUploadConsultation.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        FileUploadConsultation.AddAccessLog("Finished");
                    }
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void ConsultationsFileUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FileUploadConsultation.WorkFlow == true)
            {
                FileUploadConsultation.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
