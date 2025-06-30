using fa.api.Accounting;
using fa.api.catalog;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Employee;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report;
using fa.report.sales;
using FaData.Utils;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.sales;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.report.sales.SalesReportByProductFamily;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fa.model.Hms;
using fa.api.Hms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using fa.model.Common;
using fa.model.hms.config;
using System.Text.RegularExpressions;
using fa.model.Accounting.Transactions;
using System.Drawing.Printing;


namespace FADataAccessLibrary.report.Hms
{
    public enum ReportType
    {
        BYDATE, BYDOCTOR, BYNURSE, BYDEPARTMENT
    }
    public class RptUnAssignedReport : Report
    {
        public long[] DoctorId { get; set; }
        public string Doctor;
        public bool IsAllDoctorId { get; set; }
        public long[] NurseId { get; set; }
        public string Nurse;
        public bool IsAllNurseId { get; set; }
        public long[] DepartmentId { get; set; }
        public string Department;
        public bool IsAllDepartment { get; set; }
        public ReportType Type { get; set; }
        public string ReportHeader { get; set; }

        public List<UnAssignCareTakerReportLineItemByDate> UnAssignCareTakerReportLineItemByDate = null;

        public override string ReportTitle()
        {
            return string.Format("Care Taker UnAssign Report ") + (Type == ReportType.BYDATE ? "By Date" : Type == ReportType.BYDOCTOR ? "By Doctor": Type == ReportType.BYNURSE ? "By Nurse" : "By Department"); ;
        }
        public string ReportSubTitle()
        {
            return String.Format("From {0} To {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Care Taker UnAssign Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == ReportType.BYDATE)
                {
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    List<DateTime> reportDateRange = Enumerable.Range(0, (ToDate - FromDate).Days + 1).Select(d => FromDate.AddDays(d).Add(currentTime)).ToList();

                    List<Employee> allEmployees = EmployeeManager.Instance.ListEmployeeByCompanyIdTitleForReport(Company.CompanyId, "Doctor", "Nurse").ToList();

                    UnAssignCareTakerReportLineItemByDate = new List<UnAssignCareTakerReportLineItemByDate>();

                    var inPatientAdmissions = Context.InPatientAdmissions.Where(pa => pa.Company == Company).AsEnumerable().ToList();

                    var medicalTeamsByAdmission = inPatientAdmissions.ToDictionary(admission => admission.Id, admission => MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionIdForReport(admission.Id));

                    foreach (Employee emp in allEmployees)
                    {
                        var employeeAdmissions = inPatientAdmissions
                            .Where(admission =>
                            {
                                if (!medicalTeamsByAdmission.ContainsKey(admission.Id))
                                    return false;

                                var medicalTeams = medicalTeamsByAdmission[admission.Id];

                                return medicalTeams.Any(medicalTeam => (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id));
                            })
                            .ToList();

                        foreach (DateTime reportDate in reportDateRange)
                        {
                            Address address = AddressManager.Instance.GetAddressById((long)emp.AddressId);
                            Title title = TitleManager.Instance.GetTitleInfoById((long)emp.TitleId);

                            int numberOfPatientsAssigned = employeeAdmissions.Count(admission =>
                            {
                                var medicalTeams = medicalTeamsByAdmission[admission.Id];
                                return medicalTeams.Any(medicalTeam =>
                                {
                                    DateTime fromDate = medicalTeam.From;
                                    DateTime? toDate = medicalTeam.To ?? (DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId(admission.Id)?.DischargeOn ?? DateTime.MaxValue);

                                    return fromDate <= reportDate && reportDate <= toDate && (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id);
                                });
                            });
                            string taskStatus;
                            if (numberOfPatientsAssigned > 0)
                            {
                                taskStatus = $"Assigned ({numberOfPatientsAssigned} patients)";
                            }
                            else
                            {
                                taskStatus = "UnAssigned";
                            }
                            UnAssignCareTakerReportLineItemByDate.Add(new UnAssignCareTakerReportLineItemByDate(emp, taskStatus, reportDate, address, title));
                        }
                    }
                }
                else if (Type == ReportType.BYDOCTOR)
                {
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    List<DateTime> reportDateRange = Enumerable.Range(0, (ToDate - FromDate).Days + 1).Select(d => FromDate.AddDays(d).Add(currentTime)).ToList();
                    if (DoctorId != null && DoctorId.Length > 0)
                    {
                        UnAssignCareTakerReportLineItemByDate = new List<UnAssignCareTakerReportLineItemByDate>();
                        foreach (long doctorId in DoctorId)
                        {
                            Employee emp = EmployeeManager.Instance.GetEmployeeInfoByIdType(doctorId);

                            IList<InPatientAdmission> InPatientAdmissions = Context.InPatientAdmissions.Where(pa => pa.Company == Company).AsEnumerable().ToList();

                            var inPatientAdmissions = Context.InPatientAdmissions.Where(pa => pa.Company == Company).AsEnumerable().ToList();

                            var medicalTeamsByAdmission = inPatientAdmissions.ToDictionary(admission => admission.Id, admission => MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionIdForReport(admission.Id));

                            var employeeAdmissions = inPatientAdmissions
                        .Where(admission =>
                        {
                            if (!medicalTeamsByAdmission.ContainsKey(admission.Id))
                                return false;

                            var medicalTeams = medicalTeamsByAdmission[admission.Id];

                            return medicalTeams.Any(medicalTeam => (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id));
                        })
                                .ToList();
                            foreach (DateTime reportDate in reportDateRange)
                            {
                                Address address = AddressManager.Instance.GetAddressById((long)emp.AddressId);
                                Title title = TitleManager.Instance.GetTitleInfoById((long)emp.TitleId);

                                int numberOfPatientsAssigned = employeeAdmissions.Count(admission =>
                                {
                                    var medicalTeams = medicalTeamsByAdmission[admission.Id];
                                    return medicalTeams.Any(medicalTeam =>
                                    {
                                        DateTime fromDate = medicalTeam.From;
                                        DateTime? toDate = medicalTeam.To ?? (DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId(admission.Id)?.DischargeOn ?? DateTime.MaxValue);

                                        return fromDate <= reportDate && reportDate <= toDate && (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id);
                                    });
                                });
                                string taskStatus;
                                if (numberOfPatientsAssigned > 0)
                                {
                                    taskStatus = $"Assigned ({numberOfPatientsAssigned} patients)";
                                }
                                else
                                {
                                    taskStatus = "UnAssigned";
                                }
                                UnAssignCareTakerReportLineItemByDate.Add(new UnAssignCareTakerReportLineItemByDate(emp, taskStatus, reportDate, address, title));
                            }
                        }
                    }
                }
                else if (Type == ReportType.BYNURSE)
                {
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    List<DateTime> reportDateRange = Enumerable.Range(0, (ToDate - FromDate).Days + 1).Select(d => FromDate.AddDays(d).Add(currentTime)).ToList();
                    if (NurseId != null && NurseId.Length > 0)
                    {
                        UnAssignCareTakerReportLineItemByDate = new List<UnAssignCareTakerReportLineItemByDate>();
                        foreach (long NurseId in NurseId)
                        {
                            Employee emp = EmployeeManager.Instance.GetEmployeeInfoByIdType(NurseId);

                            var inPatientAdmissions = Context.InPatientAdmissions.Where(pa => pa.Company == Company).AsEnumerable().ToList();

                            var medicalTeamsByAdmission = inPatientAdmissions.ToDictionary(admission => admission.Id, admission => MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionIdForReport(admission.Id));

                            var employeeAdmissions = inPatientAdmissions
                        .Where(admission =>
                        {
                            if (!medicalTeamsByAdmission.ContainsKey(admission.Id))
                                return false;

                            var medicalTeams = medicalTeamsByAdmission[admission.Id];

                            return medicalTeams.Any(medicalTeam => (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id));
                        })
                        .ToList();

                            foreach (DateTime reportDate in reportDateRange)
                            {
                                Address address = AddressManager.Instance.GetAddressById((long)emp.AddressId);
                                Title title = TitleManager.Instance.GetTitleInfoById((long)emp.TitleId);

                                int numberOfPatientsAssigned = employeeAdmissions.Count(admission =>
                                {
                                    var medicalTeams = medicalTeamsByAdmission[admission.Id];
                                    return medicalTeams.Any(medicalTeam =>
                                    {
                                        DateTime fromDate = medicalTeam.From;
                                        DateTime? toDate = medicalTeam.To ?? (DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId(admission.Id)?.DischargeOn ?? DateTime.MaxValue);

                                        return fromDate <= reportDate && reportDate <= toDate && (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id);
                                    });
                                });
                                string taskStatus;
                                if (numberOfPatientsAssigned > 0)
                                {
                                    taskStatus = $"Assigned ({numberOfPatientsAssigned} patients)";
                                }
                                else
                                {
                                    taskStatus = "UnAssigned";
                                }
                                UnAssignCareTakerReportLineItemByDate.Add(new UnAssignCareTakerReportLineItemByDate(emp, taskStatus, reportDate, address, title));
                            }
                        }
                    }
                }
                else if (Type == ReportType.BYDEPARTMENT)
                {
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    List<DateTime> reportDateRange = Enumerable.Range(0, (ToDate - FromDate).Days + 1).Select(d => FromDate.AddDays(d).Add(currentTime)).ToList();
                    if (DepartmentId != null && DepartmentId.Length > 0)
                    {
                        UnAssignCareTakerReportLineItemByDate = new List<UnAssignCareTakerReportLineItemByDate>();
                        foreach (long departmentId in DepartmentId)
                        {
                            IList<Employee> employees = EmployeeManager.Instance.ListEmployeeByDepartmentId(departmentId);

                            var inPatientAdmissions = Context.InPatientAdmissions.Where(pa => pa.Company == Company).AsEnumerable().ToList();

                            var medicalTeamsByAdmission = inPatientAdmissions.ToDictionary(admission => admission.Id, admission => MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionIdForReport(admission.Id));

                            foreach (Employee emp in employees)
                            {
                                var employeeAdmissions = inPatientAdmissions
                            .Where(admission =>
                            {
                                if (!medicalTeamsByAdmission.ContainsKey(admission.Id))
                                    return false;

                                var medicalTeams = medicalTeamsByAdmission[admission.Id];

                                return medicalTeams.Any(medicalTeam => (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id));
                            })
                            .ToList();
                                foreach (DateTime reportDate in reportDateRange)
                                {
                                    Address address = AddressManager.Instance.GetAddressById((long)emp.AddressId);
                                    Title title = TitleManager.Instance.GetTitleInfoById((long)emp.TitleId);

                                    int numberOfPatientsAssigned = employeeAdmissions.Count(admission =>
                                    {
                                        var medicalTeams = medicalTeamsByAdmission[admission.Id];
                                        return medicalTeams.Any(medicalTeam =>
                                        {
                                            DateTime fromDate = medicalTeam.From;
                                            DateTime? toDate = medicalTeam.To ?? (DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId(admission.Id)?.DischargeOn ?? DateTime.MaxValue);

                                            return fromDate <= reportDate && reportDate <= toDate && (medicalTeam.PrimaryDoctorId == emp.Id || medicalTeam.SecondaryDoctorId == emp.Id || medicalTeam.PrimaryCareGiverId == emp.Id || medicalTeam.SecondaryCareGiverId == emp.Id);
                                        });
                                    });
                                    string taskStatus;
                                    if (numberOfPatientsAssigned > 0)
                                    {
                                        taskStatus = $"Assigned ({numberOfPatientsAssigned} patients)";
                                    }
                                    else
                                    {
                                        taskStatus = "UnAssigned";
                                    }
                                    UnAssignCareTakerReportLineItemByDate.Add(new UnAssignCareTakerReportLineItemByDate(emp, taskStatus, reportDate, address, title));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class UnAssignCareTakerReportLineItemByDate
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string JobTitle { get; set; }
        public string DepartmentOfConsultant { get; set; }
        public string Address { get; set; }
        public DateTime AssignDate { get; set; }
        public string TaskStatus { get; set; }

        public UnAssignCareTakerReportLineItemByDate(Employee Emp, string taskStatus, DateTime reportDate, Address Address, Title Title)
        {
            this.Name = Emp.Name;
            this.Age = Emp.DateOfBirth.HasValue ? CalculateAge(Emp.DateOfBirth.Value).ToString() : "Unknown";
            this.JobTitle = Title.Name;
            this.DepartmentOfConsultant = Emp.Department.Name;
            this.Address = Address.FullAddress;
            this.AssignDate = reportDate;
            this.TaskStatus = taskStatus;
        }
        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}
