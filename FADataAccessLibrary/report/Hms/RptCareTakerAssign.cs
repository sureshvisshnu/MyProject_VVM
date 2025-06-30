using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Op;
using fa.report;
using fa.report.Ip;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.report.Hms
{
    public class RptCareTakerAssign : Report
    {
        public int FilterIndex = 0;
        public long[] ConsultantId { get; set; }
        public long[] NurseId { get; set; }
        public long[] TechnicianId { get; set; }

        public long[] DepartmentId { get; set; }
        public List<CareTakerReport> LineCareTakerReport = new List<CareTakerReport>();

        public override string ReportTitle()
        {
            return string.Format("Care Taker Assign Report");
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
            return String.Format("Care Taker Assign Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<InPatientAdmission> InPatientAdmissions = null;
                InPatientAdmissions = (Context.InPatientAdmissions.Where(pa => pa.DateOfAdmission >= this.FromDate.Date && pa.CompanyId == Company.CompanyId).OrderByDescending(x => x.DateOfAdmission)).ToList();

                if (InPatientAdmissions != null)
                {
                    DateTime? TempDate = null;
                    foreach (InPatientAdmission Admission in InPatientAdmissions.Where(x => x.DateOfAdmission.Date <= ToDate.Date))
                    {
                        if (FilterIndex == 0)
                        {
                            InPatientLocation InPatientLocation = Context.InPatientLocations
                                .Include("Ward")
                                .Include("Bed")
                                .FirstOrDefault(x => x.AdmissionId == Admission.Id);

                            if (InPatientLocation != null)
                            {
                                IList<MedicalTeam> CareTakerHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByAdmitionId(Admission.Id);
                                if (CareTakerHistoryInfo != null && CareTakerHistoryInfo.Count > 0)
                                {
                                    foreach (MedicalTeam History in CareTakerHistoryInfo)
                                    {
                                        CareTakerReport CTRreport = CreateCareTakerReport(Context, History, Admission, ref TempDate);
                                        if (CTRreport != null)
                                        {
                                            LineCareTakerReport.Add(CTRreport);
                                        }
                                    }
                                }
                            }
                        }
                        if (FilterIndex == 1)
                        {
                            InPatientLocation InPatientLocation = Context.InPatientLocations
                                .Include("Ward")
                                .Include("Bed")
                                .FirstOrDefault(x => x.AdmissionId == Admission.Id);

                            if (InPatientLocation != null)
                            {
                                IList<MedicalTeam> ConsultantHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByConsultantIds(ConsultantId, Admission.Id);
                                if (ConsultantHistoryInfo != null && ConsultantHistoryInfo.Count > 0)
                                {
                                    foreach (MedicalTeam History in ConsultantHistoryInfo)
                                    {
                                        CareTakerReport CTRreport = CreateCareTakerReport(Context, History, Admission, ref TempDate);
                                        if (CTRreport != null)
                                        {
                                            LineCareTakerReport.Add(CTRreport);
                                        }
                                    }
                                }
                                LineCareTakerReport = LineCareTakerReport.OrderBy(r => r.PrimaryDr).ToList();
                            }
                        }

                        if (FilterIndex == 2)
                        {
                            InPatientLocation InPatientLocation = Context.InPatientLocations
                                .Include("Ward")
                                .Include("Bed")
                                .FirstOrDefault(x => x.AdmissionId == Admission.Id);

                            if (InPatientLocation != null)
                            {
                                IList<MedicalTeam> CareTakerHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByCareTakerIds(NurseId, Admission.Id);
                                if (CareTakerHistoryInfo != null && CareTakerHistoryInfo.Count > 0)
                                {
                                    foreach (MedicalTeam History in CareTakerHistoryInfo)
                                    {
                                        CareTakerReport CTRreport = CreateCareTakerReport(Context, History, Admission, ref TempDate);
                                        if (CTRreport != null)
                                        {
                                            LineCareTakerReport.Add(CTRreport);
                                        }
                                    }
                                }
                                LineCareTakerReport = LineCareTakerReport.OrderBy(r => r.PrimaryCT).ToList();
                            }
                        }

                        if (FilterIndex == 3)
                        {

                        }
                        if (FilterIndex == 4)
                        {                            
                            InPatientLocation InPatientLocation = DepartmentId == null || DepartmentId.Any(id => id != 0L)
                                ? Context.InPatientLocations
                                    .Include(x => x.Ward)
                                    .Include(x => x.Bed)
                                    .Include(x => x.AuthorizedByDoctor.Department)
                                    .FirstOrDefault(x => x.AdmissionId == Admission.Id &&
                                                          x.AuthorizedByDoctorId != null &&
                                                          x.AuthorizedByDoctor.Department != null /* &&
                                                          DepartmentId.Contains(x.AuthorizedByDoctor.Department.Id) */ )
                                : null;

                            if (InPatientLocation != null)
                            {
                                IList<MedicalTeam> CareTakerHistoryInfo = MedicalTeamManager.Instance.ListAllMedicalTeamHistoryByAdmitionId(Admission.Id);
                                if (CareTakerHistoryInfo != null && CareTakerHistoryInfo.Count > 0)
                                {
                                    foreach (MedicalTeam History in CareTakerHistoryInfo)
                                    {
                                        foreach (long departmentId in DepartmentId)
                                        {
                                            var DrTitleId = Context.Titles
                                                .Where(d => d.Name == "Doctor" && d.CompanyId == Company.CompanyId)
                                                .Select(d => d.Id)
                                                .FirstOrDefault();

                                            if (History.PrimaryDoctorId.HasValue)
                                            {
                                                long primaryDoctorId = History.PrimaryDoctorId.Value;

                                                // Check if the primaryDoctorId exists in the EmployeeTable for the current department Id and title Id
                                                bool DoctorExists = Context.Employees.Any(emp => emp.Id == primaryDoctorId && emp.DepartmentId == departmentId && emp.TitleId == DrTitleId);

                                                if (DoctorExists)
                                                {
                                                    CareTakerReport doctorReport = CreateCareTakerDeptReport(Context, History, Admission, ref TempDate, primaryDoctorId, 0);

                                                    if (doctorReport != null)
                                                    {
                                                        LineCareTakerReport.Add(doctorReport);
                                                    }
                                                }
                                            }

                                            var NrTitleId = Context.Titles
                                                .Where(d => d.Name == "Nurse" && d.CompanyId == Company.CompanyId)
                                                .Select(d => d.Id)
                                                .FirstOrDefault();

                                            if (History.PrimaryCareGiverId.HasValue)
                                            {
                                                long primaryCareTakerId = History.PrimaryCareGiverId.Value;

                                                // Check if the primaryCareTakerId exists in the EmployeeTable for the current department Id and title Id
                                                bool NurseExists = Context.Employees.Any(emp => emp.Id == primaryCareTakerId && emp.DepartmentId == departmentId && emp.TitleId == NrTitleId);

                                                if (NurseExists)
                                                {
                                                    CareTakerReport careTakerReport = CreateCareTakerDeptReport(Context, History, Admission, ref TempDate, 0, primaryCareTakerId);

                                                    if (careTakerReport != null)
                                                    {
                                                        //LineCareTakerReport.Add(careTakerReport);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    LineCareTakerReport = LineCareTakerReport.OrderBy(r => r.DepartmentOfConsultant).ToList();
                                }
                            }
                        }
                    }
                }
            }
        }
        private CareTakerReport CreateCareTakerDeptReport(AccountMasterContext Context, MedicalTeam CareTakerInfo, InPatientAdmission Admission, ref DateTime? TempDate, long primaryDoctorId, long primaryCareTakerId)
        {
            CareTakerReport CareTaker = new CareTakerReport();

            CareTaker.AdmissionId = CareTakerInfo?.AdmissionId != null ? CareTakerInfo.AdmissionId.ToString() : string.Empty;
            CareTaker.From = CareTakerInfo?.From != null ? CareTakerInfo.From.Date : DateTime.MinValue;
            CareTaker.To = CareTakerInfo.To != null ? CareTakerInfo.To.Value.Date : DateTime.MinValue;

            if (Admission.CurrentMedicalTeam != null)
            {
                Employee Emp = null;
                if (primaryDoctorId != 0)
                {
                    Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                }
                else if(primaryCareTakerId != 0)
                {
                    Emp = EmployeeManager.Instance.GetEmployeeInfoById(primaryCareTakerId);
                }
                CareTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryDr = CareTakerInfo.PrimaryDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryCT = CareTakerInfo.PrimaryCareGiver?.Name ?? string.Empty;
            }
            else
            {
                CareTaker.DepartmentOfConsultant = "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryDr = CareTakerInfo.PrimaryDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryCT = CareTakerInfo.PrimaryCareGiver?.Name ?? string.Empty;
            }

            CareTaker.SecondaryDr = CareTakerInfo.SecondaryDoctor?.ToString() ?? string.Empty;
            CareTaker.SecondaryCT = CareTakerInfo.SecondaryCareGiver?.ToString() ?? string.Empty;
            CareTaker.Notes = CareTakerInfo.Notes ?? string.Empty;

            return CareTaker;
        }
        
        private CareTakerReport CreateCareTakerReport(AccountMasterContext Context, MedicalTeam CareTakerInfo, InPatientAdmission Admission, ref DateTime? TempDate)
        {
            CareTakerReport CareTaker = new CareTakerReport();

            CareTaker.AdmissionId = CareTakerInfo?.AdmissionId != null ? CareTakerInfo.AdmissionId.ToString() : string.Empty;
            CareTaker.From = CareTakerInfo?.From != null ? CareTakerInfo.From.Date : DateTime.MinValue;
            CareTaker.To = CareTakerInfo.To != null ? CareTakerInfo.To.Value.Date : DateTime.MinValue;

            if (Admission.CurrentMedicalTeam != null)
            {
                Employee Emp = null;
                if (FilterIndex == 2)
                {
                    Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryCareGiverId ?? 0);
                }
                else
                {
                    Emp = EmployeeManager.Instance.GetEmployeeInfoById(CareTakerInfo.PrimaryDoctorId ?? 0);
                }
                CareTaker.DepartmentOfConsultant = Emp?.Department?.Name ?? "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryDr = CareTakerInfo.PrimaryDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryCT = CareTakerInfo.PrimaryCareGiver?.Name ?? string.Empty;
            }
            else
            {
                CareTaker.DepartmentOfConsultant = "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryDr = CareTakerInfo.PrimaryDoctor?.Name ?? string.Empty;
                CareTaker.PrimaryCT = CareTakerInfo.PrimaryCareGiver?.Name ?? string.Empty;
            }

            CareTaker.SecondaryDr = CareTakerInfo.SecondaryDoctor?.ToString() ?? string.Empty;
            CareTaker.SecondaryCT = CareTakerInfo.SecondaryCareGiver?.ToString() ?? string.Empty;
            CareTaker.Notes = CareTakerInfo.Notes ?? string.Empty;

            return CareTaker;
        }


        private CareTakerReport CreateCareTakerReportOld(AccountMasterContext Context, MedicalTeam CareTakerInfo, InPatientAdmission Admission, ref DateTime? TempDate)
        {
            CareTakerReport CareTaker = new CareTakerReport();

            CareTaker.AdmissionId = CareTaker.AdmissionId != null ? CareTakerInfo.AdmissionId.ToString() : string.Empty;
            CareTaker.From =  CareTakerInfo.From.Date;
            CareTaker.To = CareTakerInfo.To != null ? CareTakerInfo.To.Value.Date : DateTime.MinValue;
            if (Admission.CurrentMedicalTeam != null)
            {
                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                if (Emp != null)
                {
                    CareTaker.DepartmentOfConsultant = Emp.Department?.Name ?? "";
                    CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor != null ? Admission.CurrentMedicalTeam.AuthorizedByDoctor.Name : string.Empty;
                }
                CareTaker.PrimaryDr = CareTakerInfo.PrimaryDoctor != null ? Admission.CurrentMedicalTeam.PrimaryDoctor.Name : string.Empty;
                CareTaker.PrimaryCT = CareTakerInfo.PrimaryCareGiver != null ? Admission.CurrentMedicalTeam.PrimaryCareGiver.Name : string.Empty;
            }            
            else
            {
                CareTaker.DepartmentOfConsultant = "";
                CareTaker.AuthorizedDr = CareTakerInfo.AuthorizedByDoctor != null ? CareTakerInfo.AuthorizedByDoctor.Name : string.Empty;
                CareTaker.PrimaryDr = (CareTakerInfo != null && CareTakerInfo.PrimaryDoctor != null) ? CareTakerInfo.PrimaryDoctor.ToString() : string.Empty;
                CareTaker.PrimaryCT = (CareTakerInfo != null && CareTakerInfo.PrimaryCareGiver != null) ? CareTakerInfo.PrimaryCareGiver.ToString() : string.Empty;
            }
            CareTaker.SecondaryDr = (CareTakerInfo != null && CareTakerInfo.SecondaryDoctor != null) ? CareTakerInfo.SecondaryDoctor.ToString() : string.Empty;
            CareTaker.SecondaryCT = (CareTakerInfo != null && CareTakerInfo.SecondaryCareGiver != null) ? CareTakerInfo.SecondaryCareGiver.ToString() : string.Empty;
            CareTaker.Notes = (CareTakerInfo != null && CareTakerInfo.Notes != null) ? CareTakerInfo.Notes.ToString() : string.Empty;
           
            return CareTaker;
        }
    }
    public class CareTakerReport
    {
        public String AdmissionId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public String DepartmentOfConsultant { get; set; }

        public String PrimaryDr { get; set; }
        public String SecondaryDr { get; set; }
        public String PrimaryCT { get; set; }
        public String SecondaryCT { get; set; }       
        public String Notes { get; set; }
        public String AuthorizedDr { get; set; }
    }
}
