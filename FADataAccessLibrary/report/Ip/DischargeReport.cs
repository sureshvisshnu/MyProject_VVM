using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.model.Employee;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.report;
using fa.report.Ip;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.report.Ip
{
    public class DischargeReport : Report
    {
        public long[] WardId { get; set; }
        public long[] ConsultantId { get; set; }
        public long[] InsuranceId { get; set; }
        public long[] DepartmentId { get; set; }
        public int FilterIndex = 0;
        public IList<DischargePatientReport> DischargePatientReport=new List<DischargePatientReport>();
        public override string ReportTitle()
        {
            Ward Ward = null;
            return String.Format("{0} Discharge Report", Ward != null ? (Ward.Name + ":") : string.Empty);
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
            return String.Format("Discharge Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<InPatientAdmission> InPatientAdmissions = (Context.InPatientAdmissions.Where(pa => pa.CompanyId == Company.CompanyId && pa.Status == InPatientStatus.DISCHARGED && pa.DateOfAdmission <= ToDate.Date).OrderByDescending(x => x.DateOfAdmission)).ToList();
                if (InPatientAdmissions != null && InPatientAdmissions.Count > 0)
                {
                    DateTime? TempDate = null;
                    string WardName = string.Empty;
                    foreach (InPatientAdmission Admission in InPatientAdmissions)
                    {
                        DischargeNote dischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(Admission.Id, this.FromDate, this.ToDate);
                        if (dischargeNote != null)
                        {
                            if (FilterIndex == 0)
                            {
                                InPatientLocation InPatientLocation = Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == Admission.Id);

                                if (InPatientLocation != null)
                                {
                                    DischargePatientReport report = CreateDischargePatientReport(Context, Admission, dischargeNote, ref TempDate, ref WardName);
                                    if (report != null)
                                    {
                                        DischargePatientReport.Add(report);
                                    }
                                }
                            }
                            if (FilterIndex == 1)
                            {
                                InPatientLocation InPatientLocation = ConsultantId == null || ConsultantId.Any(id => id != 0L)
                                    ? Context.InPatientLocations
                                        .Include("Ward")
                                        .Include("Bed")
                                        .FirstOrDefault(x => x.AdmissionId == Admission.Id &&
                                                              x.AuthorizedByDoctorId != null &&
                                                              ConsultantId.Contains(x.AuthorizedByDoctorId ?? 0L)) : null;

                                if (InPatientLocation != null)
                                {
                                    DischargePatientReport report = CreateDischargePatientReport(Context, Admission, dischargeNote, ref TempDate, ref WardName);
                                    if (report != null)
                                    {
                                        DischargePatientReport.Add(report);
                                    }
                                }
                            }
                            if (FilterIndex == 2)
                            {
                                InPatientLocation InPatientLocation = DepartmentId == null || DepartmentId.Any(id => id != 0L)
                                    ? Context.InPatientLocations
                                        .Include(x => x.Ward)
                                        .Include(x => x.Bed)
                                        .Include(x => x.AuthorizedByDoctor.Department)
                                        .FirstOrDefault(x => x.AdmissionId == Admission.Id &&
                                                              x.AuthorizedByDoctorId != null &&
                                                              x.AuthorizedByDoctor.Department != null &&
                                                              DepartmentId.Contains(x.AuthorizedByDoctor.Department.Id)) : null;

                                if (InPatientLocation != null)
                                {
                                    DischargePatientReport report = CreateDischargePatientReport(Context, Admission, dischargeNote, ref TempDate, ref WardName);
                                    if (report != null)
                                    {
                                        DischargePatientReport.Add(report);
                                    }
                                }
                            }
                            if (FilterIndex == 3)
                            {
                                long[] InsuranceValues = InsuranceId;

                                InPatientLocation InPatientLocation = InsuranceId == null || InsuranceId.Any(id => id != 0L)
                                    ? Context.InPatientLocations
                                        .Include("Ward")
                                        .Include("Bed")
                                        .FirstOrDefault(x => x.AdmissionId == Admission.Id &&
                                                              x.InPatientAdmission.InsuranceInfoId != null &&
                                                              InsuranceValues.Contains(x.InPatientAdmission.InsuranceInfoId ?? 0L)) : null;

                                if (InPatientLocation != null)
                                {
                                    DischargePatientReport report = CreateDischargePatientReport(Context, Admission, dischargeNote, ref TempDate, ref WardName);
                                    if (report != null)
                                    {
                                        DischargePatientReport.Add(report);
                                    }
                                }
                            }
                            if (FilterIndex == 4)
                            {
                                List<InPatientLocation> InPatientLocations = Context.InPatientLocations
                                    .Include("InPatientAdmission")
                                    .Include("Ward")
                                    .Include("Bed")
                                    .Include("AuthorizedByDoctor")
                                    .Where(x => x.AdmissionId == Admission.Id &&
                                                (WardId == null || WardId.Any(id => id == 0L) || WardId.Contains(x.WardId ?? 0L))).ToList();

                                if (InPatientLocations != null)
                                {
                                    foreach (InPatientLocation locationDetail in InPatientLocations)
                                    {
                                        DischargePatientReport report = CreateDischargePatientReport(Context, Admission, dischargeNote, ref TempDate, ref WardName);
                                        if (report != null)
                                        {
                                            report.WardName = locationDetail.Ward.Name;
                                            report.BedName = locationDetail.Bed.Name;
                                            DischargePatientReport.Add(report);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (DischargePatientReport != null && DischargePatientReport.Count > 0)
            {
                DischargePatientReport.OrderBy(x => x.DischargedOn);
            }
        }
        private DischargePatientReport CreateDischargePatientReport(AccountMasterContext Context, InPatientAdmission Admission, DischargeNote dischargeNote, ref DateTime? TempDate, ref string WardName)
        {
            DischargePatientReport temp = new DischargePatientReport();
            Patient patient = Context.Patients.Include("Address").FirstOrDefault(x => x.Id == (long)Admission.PatientId);

            if (patient != null)
            {
                temp.PatientId = patient.PatientNumber;
                temp.PatientName = patient.Name;
                temp.DOB = patient.DateOfBirth;
                temp.Age = (int)patient.Age;
                temp.Address = patient.Address != null ? patient.Address.FullAddress : string.Empty;
                temp.CatType = FilterIndex.ToString();

                temp.AdmittedOn = Admission.DateOfAdmission;

                if (Admission.Status == InPatientStatus.DISCHARGED)
                {
                    if (dischargeNote != null)
                    {
                        if (TempDate == null || dischargeNote.DischargeOn.GetValueOrDefault().Date.ToShortDateString() != ((DateTime)TempDate).ToShortDateString())
                        {
                            temp.DischargedOn = dischargeNote.DischargeOn.GetValueOrDefault();
                            TempDate = dischargeNote.DischargeOn.GetValueOrDefault().Date;
                        }
                    }
                }
                if (FilterIndex != 4)
                {
                    List<InPatientLocation> InPatientLocation = Context.InPatientLocations.Include("InPatientAdmission").Include("Ward").Include("Bed").Include("AuthorizedByDoctor").Where(x => x.AdmissionId == Admission.Id).ToList();
                    if (InPatientLocation != null)
                    {
                        foreach (InPatientLocation locationdetail in InPatientLocation)
                        {
                            temp.WardName = locationdetail.Ward.Name;
                            temp.BedName = locationdetail.Bed.Name;
                        }
                    }
                }
                if (Admission.CurrentMedicalTeam != null)
                {
                    if (FilterIndex.ToString() == "2")
                    {
                        Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                        if (Emp != null)
                        {
                            temp.DepartmenOfConsutant = Emp.Department?.Name ?? "";
                        }
                    }
                    temp.PrimaryDr = Admission.CurrentMedicalTeam.PrimaryDoctor != null ? Admission.CurrentMedicalTeam.PrimaryDoctor.Name : string.Empty;
                    temp.PrimaryCT = Admission.CurrentMedicalTeam.PrimaryCareGiver != null ? Admission.CurrentMedicalTeam.PrimaryCareGiver.Name : string.Empty;
                }
                if (Admission.InsuranceInfoId != null)
                {
                    InsuranceInfo lInsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)Admission.InsuranceInfoId, true);
                    if (lInsuranceInfo != null && lInsuranceInfo.IPInsuranceCoverage)
                    {
                        temp.InsuranceComp = lInsuranceInfo.InsuranceName;
                    }
                }
                if (Admission.MedicalTeamHistory != null)
                {
                    temp.DepartmenOfConsutant = Admission.CurrentMedicalTeam.PrimaryDoctor != null ? Admission.CurrentMedicalTeam.PrimaryDoctor.Name : string.Empty;

                }
            }
            return temp;
        }

    }
    public class DischargePatientReport
    {
        public String WardName { get; set; }
        public String BedName { get; set; }
        public String PatientId { get; set; }
        public String PatientName { get; set; }
        public String Address { get; set; }
        public String PrimaryDr { get; set; }
        public String PrimaryCT { get; set; }
        public DateTime DOB { get; set; }
        public DateTime AdmittedOn { get; set; }
        public DateTime DischargedOn { get; set; }
        public int Age { get; set; }
        public String CatType { get; set; }
        public String InsuranceComp { get; set; }
        public String DepartmenOfConsutant { get; set; }
    }

}
