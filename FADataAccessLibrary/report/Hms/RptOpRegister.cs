using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using FaData.Utils;
using FADataAccessLibrary.report.Hms;
using Microsoft.EntityFrameworkCore;
using static fa.report.Hms.RptOPLineItem;

namespace fa.report.Hms
{
    public enum OpPatientTypeSelection
    {
        BYALLPATIENT, BYNEWPATIENT, BYREPATEDPATIENT
    }
    public class RptOpRegister : Report
    {
        public long? CostcenterId { get; set; }
        public OpPatientTypeSelection Type { get; set; }
        public IList<RptOPLineItem> LineItems { get; } = new List<RptOPLineItem>();
        public List<PatientOpRepoertLineItem> PatientOpRepoertLineItems = null;

        public override string ReportTitle()
        {
            return "Out Patient Report :" + (Type == OpPatientTypeSelection.BYALLPATIENT ? "By All Patient" : Type == OpPatientTypeSelection.BYNEWPATIENT ? "By New Patient" : "By Repated Patient");
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
            return String.Format("Out Patient Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            List<Registration> Registration = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == OpPatientTypeSelection.BYALLPATIENT)
                {
                    Registration = (from registration in Context.Registrationes
                                    where (registration.CompanyId == this.Company.CompanyId
                                    && registration.DateOfRegistration >= this.FromDate
                                    && registration.DateOfRegistration <= ToDate
                                    )
                                    select registration).OrderBy(x => x.DateOfRegistration).ToList<Registration>();

                    if (Registration != null && Registration.Count > 0)
                    {
                        foreach (Registration db in Registration)
                        {

                            RptOPLineItem LineItem = null;
                            ConsultationNote Note = Context.ConsultationNotes.Include("Patient").Include("Consultant").FirstOrDefault(x => x.OpRegistrationId == db.Id);
                            if (Note != null)
                            {
                                double FeeAdded = 0;
                                Registration RegistrationFee = OpManager.Instance.GetOpRegistrationById(db.Id);
                                if (RegistrationFee != null)
                                {
                                    FeeAdded = RegistrationFee.RegistrationFee;
                                }
                                IList<ConsultedProcedure> ConsultedProcedureFee = ConsultationNoteManager.Instance.ListProcedureByNoteId(Note.Id);
                                if (ConsultedProcedureFee != null && ConsultedProcedureFee.Count > 0)
                                {
                                    double ProcedureFeeAdded = 0;
                                    foreach (ConsultedProcedure ConsultedProcedureFees in ConsultedProcedureFee)
                                    {
                                        ProcedureFeeAdded += ConsultedProcedureFees.Fees;
                                    }
                                    FeeAdded += ProcedureFeeAdded;
                                }
                                FeeAdded += Note.Fees;
                                LineItem = new RptOPLineItem(db.DateOfRegistration, Note.Patient, Note.Patient.PatientNumber, db.TockenNo, db.RequestedDoctor != null ? db.RequestedDoctor.Name : "", FeeAdded);
                                LineItems.Add(LineItem);
                            }
                            else
                            {
                                double FeeAdded = 0;
                                string EmpName = null;
                                Registration RegistrationFee = OpManager.Instance.GetOpRegistrationById(db.Id);
                                if (RegistrationFee != null)
                                {
                                    FeeAdded = RegistrationFee.RegistrationFee;
                                }
                                if (RegistrationFee.RequestedDoctorId != null)
                                {
                                    fa.model.Employee.Employee Employee = EmployeeManager.Instance.GetEmployeeInfoById((long)RegistrationFee.RequestedDoctorId);
                                    if (Employee != null)
                                    {
                                        EmpName = Employee.Name;
                                    }
                                }
                                LineItem = new RptOPLineItem(db.DateOfRegistration, RegistrationFee.Patient, RegistrationFee.Patient.PatientNumber, db.TockenNo, EmpName != null ? EmpName : string.Empty, FeeAdded);
                                LineItems.Add(LineItem);
                            }
                        }
                    }
                }
                else if (Type == OpPatientTypeSelection.BYNEWPATIENT)
                {
                    var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration < ToDate.AddDays(1).Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                    if (distinctPatients.Count > 0)
                    {
                        PatientOpRepoertLineItems = new List<PatientOpRepoertLineItem>();

                        foreach (var patient in distinctPatients)
                        {
                            IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                            if (OpReg.Count == 1)
                            {
                                foreach (Registration Reg in OpReg)
                                {
                                    double TotalFee = 0;
                                    ConsultationNote Note = Context.ConsultationNotes.Include("Patient").Include("Consultant").FirstOrDefault(x => x.OpRegistrationId == Reg.Id);
                                    if (Note != null)
                                    {
                                        IList<ConsultedProcedure> ConsultedProcedureFee = ConsultationNoteManager.Instance.ListProcedureByNoteId(Note.Id);
                                        if (ConsultedProcedureFee != null && ConsultedProcedureFee.Count > 0)
                                        {
                                            double ProcedureFeeAdded = 0;
                                            foreach (ConsultedProcedure ConsultedProcedureFees in ConsultedProcedureFee)
                                            {
                                                ProcedureFeeAdded += ConsultedProcedureFees.Fees;
                                            }
                                            TotalFee += ProcedureFeeAdded;
                                        }
                                        TotalFee += Note.Fees;
                                    }
                                    else
                                    {
                                        TotalFee = Reg.RegistrationFee;
                                    }
                                    PatientOpRepoertLineItem LineItem = new PatientOpRepoertLineItem(Reg, TotalFee);
                                    PatientOpRepoertLineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration < ToDate.AddDays(1).Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                    if (distinctPatients.Count > 0)
                    {
                        PatientOpRepoertLineItems = new List<PatientOpRepoertLineItem>();

                        foreach (var patient in distinctPatients)
                        {
                            IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                            if (OpReg.Count > 1)
                            {
                                foreach (Registration Reg in OpReg)
                                {
                                    double TotalFee = 0;
                                    ConsultationNote Note = Context.ConsultationNotes.Include("Patient").Include("Consultant").FirstOrDefault(x => x.OpRegistrationId == Reg.Id);
                                    if (Note != null)
                                    {
                                        IList<ConsultedProcedure> ConsultedProcedureFee = ConsultationNoteManager.Instance.ListProcedureByNoteId(Note.Id);
                                        if (ConsultedProcedureFee != null && ConsultedProcedureFee.Count > 0)
                                        {
                                            double ProcedureFeeAdded = 0;
                                            foreach (ConsultedProcedure ConsultedProcedureFees in ConsultedProcedureFee)
                                            {
                                                ProcedureFeeAdded += ConsultedProcedureFees.Fees;
                                            }
                                            TotalFee += ProcedureFeeAdded;
                                        }
                                        TotalFee += Note.Fees;
                                    }
                                    else
                                    {
                                        TotalFee = Reg.RegistrationFee;
                                    }
                                    PatientOpRepoertLineItem LineItem = new PatientOpRepoertLineItem(Reg, TotalFee);
                                    PatientOpRepoertLineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                }
            }
            
        }
    }
    public class RptOPLineItem
    {
        public DateTime Date { get; set; }
        public string Patientdetail { get; set; }
        public String PatientNo { get; set; }
        public String TokenNo { get; set; }
        public string ConsultantDetail { get; set; }
        public double Fee { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public RptOPLineItem(DateTime Date, Patient Patient, String PatientNo, String TokenNo, String Consultant, double Fee)
        {
            this.Date = Date;
            string Address = (Patient.AddressId != null ? AddressManager.Instance.GetAddressById((long)Patient.AddressId).FullAddressInSingleLine : "");
            this.Patientdetail = Patient.Name + (!string.IsNullOrEmpty(Address) ? ("\n" + Address) : "");
            this.PatientNo = PatientNo;
            this.TokenNo = TokenNo;
            this.ConsultantDetail = Consultant;
            this.Fee = Fee;
            this.Age = (int)Patient.Age;
            this.Gender = Patient.Gender.ToString();
        }
        public class PatientOpRepoertLineItem
        {
            public DateTime Date { get; set; }
            public string Patientdetail { get; set; }
            public String PatientNo { get; set; }
            public String TokenNo { get; set; }
            public string ConsultantDetail { get; set; }
            public double Fee { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }

            public PatientOpRepoertLineItem(Registration Reg , double TotalFee)
            {
                this.Date = Reg.DateOfRegistration;
                string Address = (Reg.Patient.AddressId != null ? AddressManager.Instance.GetAddressById((long)Reg.Patient.AddressId).FullAddressInSingleLine : "");
                this.Patientdetail = Reg.Patient.Name + (!string.IsNullOrEmpty(Address) ? ("\n" + Address) : "");
                this.PatientNo = Reg.Patient.PatientNumber;
                this.TokenNo = Reg.TockenNo;
                this.ConsultantDetail = Reg.RequestedDoctor != null ? Reg.RequestedDoctor.Name : "";
                this.Fee = TotalFee;
                this.Age = (int)Reg.Patient.Age;
                this.Gender = Reg.Patient.Gender.ToString();
            }
        }
    }
}
