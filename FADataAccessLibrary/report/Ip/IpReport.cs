using fa.context;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.model.Hms.Ip;
using fa.model.Hms.common;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using fa.api.Accounting;
using fa.model.Employee;
using NPOI.SS.Formula.Functions;

namespace fa.report.Ip
{
    public class IpReport : Report
    {
        public long[] WardId { get; set; }
        public long[] ConsultantId { get; set; }
        public long[] InsuranceId { get; set; }
        public long[] DepartmentId { get; set; }

        public int FltrStatus { get; set; }
        public int OrdrStatus { get; set; }
        public int FilterIndex = 0;
        public List<InPatientReport> LinePatients = new List<InPatientReport>();
        public override string ReportTitle()
        {
            Ward Ward = null;
            //if (WardId != 0L) { Ward=WardManager.Instance.GetWardById(WardId); }
            return String.Format("{0} Inpatient Report", Ward != null ? (Ward.Name + ":") : string.Empty);
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
            return String.Format("Inpatient Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        }
        private InPatientReport CreateInPatientReport(AccountMasterContext Context, InPatientAdmission Admission, int OrdrStatus, ref DateTime? TempDate, ref string WardName)
        {
            InPatientReport temp = new InPatientReport();
            Patient patient = Context.Patients.Include("Address").FirstOrDefault(x => x.Id == (long)Admission.PatientId && x.CompanyId == Company.CompanyId);

            if (patient != null)
            {
                temp.PatientId = patient.PatientNumber;
                temp.PatientName = patient.Name;
                temp.DOB = patient.DateOfBirth;
                temp.Age = (int)patient.Age;
                temp.Address = patient.Address != null ? patient.Address.FullAddress : string.Empty;
                temp.CatType= FilterIndex.ToString();
                
                if (OrdrStatus == 1)
                {
                    if (TempDate == null || Admission.DateOfAdmission.Date.ToShortDateString() != ((DateTime)TempDate).ToShortDateString())
                    {
                        temp.AdmittedOn = Admission.DateOfAdmission;
                        TempDate = Admission.DateOfAdmission.Date;
                    }
                }
                else
                {
                    temp.AdmittedOn = Admission.DateOfAdmission;
                }

                if (Admission.Status == InPatientStatus.DISCHARGED)
                {
                    DischargeNote dischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(Admission.Id);
                    if (dischargeNote != null)
                    {
                        if (OrdrStatus == 2)
                        {
                            if (TempDate == null || dischargeNote.DischargeOn.GetValueOrDefault().Date.ToShortDateString() != ((DateTime)TempDate).ToShortDateString())
                            {
                                temp.DischargedOn = dischargeNote.DischargeOn.GetValueOrDefault();
                                TempDate = dischargeNote.DischargeOn.GetValueOrDefault().Date;
                            }
                        }
                        else
                        {
                            if (dischargeNote.DischargeOn.HasValue)
                            {
                                temp.DischargedOn = dischargeNote.DischargeOn.Value;
                                TempDate = dischargeNote.DischargeOn.Value.Date;
                            }
                        }
                    }
                }
                if (Admission.CurrentLocation != null)
                {
                    if (Admission.CurrentLocation.Ward != null)
                    {
                        if (OrdrStatus == 0)
                        {
                            if (WardName != Admission.CurrentLocation.Ward.Name)
                            {
                                temp.WardName = Admission.CurrentLocation.Ward.Name;
                                WardName = Admission.CurrentLocation.Ward.Name;
                            }
                        }
                        else
                        {
                            temp.WardName = Admission.CurrentLocation.Ward.Name;
                        }
                    }
                    temp.BedName = Admission.CurrentLocation.Bed != null ? Admission.CurrentLocation.Bed.Name : string.Empty;
                }

                if (Admission.CurrentMedicalTeam != null)
                {
                    if(FilterIndex.ToString() == "2")
                    {
                        Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(Admission.CurrentMedicalTeam?.PrimaryDoctorId ?? 0);
                        if(Emp != null)
                        {
                            temp.DepartmenOfConsutant = Emp.Department?.Name ?? "";
                        }
                    }
                    temp.PrimaryDr = Admission.CurrentMedicalTeam.PrimaryDoctor != null ? Admission.CurrentMedicalTeam.PrimaryDoctor.Name : string.Empty;
                    temp.PrimaryCT = Admission.CurrentMedicalTeam.PrimaryCareGiver != null ? Admission.CurrentMedicalTeam.PrimaryCareGiver.Name : string.Empty;
                }
                if(Admission.InsuranceInfoId != null)
                {
                    InsuranceInfo lInsuranceInfo = InsuranceInfoManager.Instance.GetInsuranceInfoById((long)Admission.InsuranceInfoId, true);
                    if (lInsuranceInfo != null && lInsuranceInfo.IPInsuranceCoverage)
                    {
                        temp.InsuranceComp = lInsuranceInfo.InsuranceName;
                    }
                }
                if(Admission.MedicalTeamHistory != null)
                {
                    temp.DepartmenOfConsutant = Admission.CurrentMedicalTeam.PrimaryDoctor != null ? Admission.CurrentMedicalTeam.PrimaryDoctor.Name : string.Empty;

                }
            }
            return temp;
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<InPatientAdmission> InPatientAdmissions = null;
                if (FltrStatus == 0)
                {
                    InPatientAdmissions = (Context.InPatientAdmissions.Where(pa => pa.CompanyId == Company.CompanyId && pa.Status == 0 &&
                                        pa.DateOfAdmission >= this.FromDate.Date).OrderByDescending(x => x.DateOfAdmission)).ToList();
                }
                else if (FltrStatus == 1)
                {
                    InPatientAdmissions = (Context.InPatientAdmissions.Where(pa => pa.CompanyId == Company.CompanyId && pa.DateOfAdmission >= this.FromDate.Date).OrderByDescending(x => x.DateOfAdmission)).ToList();
                }

                if (InPatientAdmissions != null)
                {
                    DateTime? TempDate = null;
                    string WardName = string.Empty;
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
                                InPatientReport report = CreateInPatientReport(Context, Admission, OrdrStatus, ref TempDate, ref WardName);
                                if (report != null)
                                {
                                    LinePatients.Add(report);
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
                                                          ConsultantId.Contains(x.AuthorizedByDoctorId ?? 0L))
                                : null;

                            if (InPatientLocation != null)
                            {
                                InPatientReport report = CreateInPatientReport(Context, Admission, OrdrStatus, ref TempDate, ref WardName);
                                if (report != null)
                                {
                                    LinePatients.Add(report);
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
                                                          DepartmentId.Contains(x.AuthorizedByDoctor.Department.Id))
                                : null;

                            if(InPatientLocation != null)
                            {
                                InPatientReport report = CreateInPatientReport(Context, Admission, OrdrStatus, ref TempDate, ref WardName);
                                if (report != null)
                                {
                                    LinePatients.Add(report);
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
                                                          InsuranceValues.Contains(x.InPatientAdmission.InsuranceInfoId ?? 0L))
                                : null;

                            if (InPatientLocation != null)
                            {
                                InPatientReport report = CreateInPatientReport(Context, Admission, OrdrStatus, ref TempDate, ref WardName);
                                if (report != null)
                                {
                                    LinePatients.Add(report);
                                }
                            }
                        }                                             
                        if (FilterIndex == 4)
                        {
                            InPatientLocation InPatientLocation = WardId == null || WardId.Any(id => id == 0L)
                                ? Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == Admission.Id)
                                : Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == Admission.Id && WardId.Contains(x.WardId ?? 0L));

                            //InPatientLocation InPatientLocation =WardId==0L? Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == Admission.Id): Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == Admission.Id && x.WardId == WardId);
                            if (InPatientLocation != null)
                            {
                                InPatientReport report = CreateInPatientReport(Context, Admission, OrdrStatus, ref TempDate, ref WardName);
                                if (report != null)
                                {
                                    LinePatients.Add(report);
                                }
                            }
                        }
                    }
                }
            }
            if (LinePatients != null && LinePatients.Count > 0)
            {
                if (OrdrStatus == 0)
                {
                    LinePatients.OrderBy(x => x.WardName);
                }
                else if (OrdrStatus == 1)
                {
                    LinePatients.OrderBy(x => x.AdmittedOn);
                }
                else if (OrdrStatus == 2)
                {
                    LinePatients.OrderBy(x => x.DischargedOn);
                }
            }
        }
    }
    public class InPatientReport
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
