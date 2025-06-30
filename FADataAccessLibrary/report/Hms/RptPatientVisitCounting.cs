using fa.context;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.model.Hms.Ip;
using fa.model.Hms.common;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using fa.model.Accounting.Masters;
using fa.report;
using fa.model.OrderManagement;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using fa.report.Ip;
using FADataAccessLibrary.report.Inventory;
using System.Linq;
using fa.model.Hms.Op;
using Fa.model.Hms.common;
using fa.model.hms.common;


namespace FADataAccessLibrary.report.Hms
{
    public enum PatientVisitCountingType
    {
        BYDATE, BYDEPARTMENT, BYDIAGONSIS
    }
    public enum PatientType
    {
        NEWPATIENT, REPEATEDPATIENT
    }
    public class RptPatientVisitCounting : Report
    {
        public long[] DeptIds { get; set; }
        public string Dept;
        public bool IsAllDept { get; set; }
        public long[] DiagIds { get; set; }
        public string Diag;
        public bool IsAllDiag { get; set; }
        public PatientVisitCountingType Type { get; set; }
        public PatientType PType { get; set; }
        public string ReportHeader { get; set; }

        public List<PatientVisitCountingLineItem> PatientVisitCountingLineItems = null;
        public List<PatientVisitCountingLineItemForDiagnosis> PatientVisitCountingLineItemForDiagnosiss = null;
        public override string ReportTitle()
        {
            return "Patient Visit Counting " + (Type == PatientVisitCountingType.BYDATE ? "By Date" : Type == PatientVisitCountingType.BYDEPARTMENT ? "By Department" : "By Diagnosis");
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
            return String.Format("Patient Visit Counting Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (Type == PatientVisitCountingType.BYDATE)
                {
                    if(PType == PatientType.NEWPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration < ToDate.AddDays(1).Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItems = new List<PatientVisitCountingLineItem>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count == 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        PatientVisitCountingLineItem LineItem = new PatientVisitCountingLineItem(Reg);
                                        PatientVisitCountingLineItems.Add(LineItem);
                                    }
                                }
                            }
                        }
                    }
                    if (PType == PatientType.REPEATEDPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration < ToDate.AddDays(1).Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItems = new List<PatientVisitCountingLineItem>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count > 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        PatientVisitCountingLineItem LineItem = new PatientVisitCountingLineItem(Reg);
                                        PatientVisitCountingLineItems.Add(LineItem);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Type == PatientVisitCountingType.BYDEPARTMENT)
                {
                    List<long> DeptId = DeptIds.ToList();
                    if (PType == PatientType.NEWPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient").Include("RequestedDoctor")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration <= ToDate.Date && x.Company == Company && x.RequestedDoctor != null && DeptId.Contains(x.RequestedDoctor.Department.Id))
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItems = new List<PatientVisitCountingLineItem>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count == 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        if (Reg.RequestedDoctor != null)
                                        {
                                            PatientVisitCountingLineItem LineItem = new PatientVisitCountingLineItem(Reg);
                                            PatientVisitCountingLineItems.Add(LineItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (PType == PatientType.REPEATEDPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient").Include("RequestedDoctor")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration <= ToDate.Date && x.Company == Company && x.RequestedDoctor != null && DeptId.Contains(x.RequestedDoctor.Department.Id))
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItems = new List<PatientVisitCountingLineItem>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count > 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        if(Reg.RequestedDoctor != null)
                                        {
                                            PatientVisitCountingLineItem LineItem = new PatientVisitCountingLineItem(Reg);
                                            PatientVisitCountingLineItems.Add(LineItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<long> DiagId = DiagIds.ToList();
                    if (PType == PatientType.NEWPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration <= ToDate.Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItemForDiagnosiss = new List<PatientVisitCountingLineItemForDiagnosis>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count == 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        if (Reg.HasConsulted)
                                        {
                                            var consultedNotes = Context.ConsultationNotes
                                                .Include("Consultant")
                                                .Include("ConsultedSymptom")
                                                .Where(x => x.PatientId == patient.Id
                                                        && x.Date >= FromDate
                                                        && x.Date <= ToDate
                                                        && x.Company == Company
                                                        && x.ConsultedSymptom.Any(cs => DiagId.Contains(cs.SymptomId)))
                                                .ToList();

                                            if (consultedNotes.Count > 0)
                                            {
                                                foreach (var Consultation in consultedNotes)
                                                {
                                                    if (Consultation.ConsultedSymptom != null)
                                                    {
                                                        IList<ConsultedSymptom> ConsultedSymptoms = ConsultationNoteManager.Instance.ListSymptomByNoteId(Consultation.Id);
                                                        if (ConsultedSymptoms.Count > 0)
                                                        {
                                                            foreach (ConsultedSymptom consultedSymptom in ConsultedSymptoms)
                                                            {
                                                                PatientVisitCountingLineItemForDiagnosis LineItem = new PatientVisitCountingLineItemForDiagnosis(consultedSymptom, Reg);
                                                                PatientVisitCountingLineItemForDiagnosiss.Add(LineItem);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    if (PType == PatientType.REPEATEDPATIENT)
                    {
                        var distinctPatients = Context.Registrationes.Include("Patient")
                            .Where(x => x.DateOfRegistration >= FromDate.Date && x.DateOfRegistration <= ToDate.Date && x.Company == Company)
                            .Select(x => x.Patient)
                            .Distinct()
                            .ToList();
                        if (distinctPatients.Count > 0)
                        {
                            PatientVisitCountingLineItemForDiagnosiss = new List<PatientVisitCountingLineItemForDiagnosis>();

                            foreach (var patient in distinctPatients)
                            {
                                IList<Registration> OpReg = OpManager.Instance.ListAllOpByPatientIdForReport(patient.Id, FromDate, ToDate);

                                if (OpReg.Count > 1)
                                {
                                    foreach (Registration Reg in OpReg)
                                    {
                                        if (Reg.HasConsulted)
                                        {
                                            var consultedNotes = Context.ConsultationNotes
                                                .Include("ConsultedSymptom")
                                                .Where(x => x.PatientId == patient.Id
                                                        && x.Date >= FromDate
                                                        && x.Date <= ToDate
                                                        && x.Company == Company
                                                        && x.ConsultedSymptom.Any(cs => DiagId.Contains(cs.SymptomId)))
                                                .ToList();

                                            if (consultedNotes.Count > 0)
                                            {
                                                foreach (var Consultation in consultedNotes)
                                                {
                                                    if (Consultation.ConsultedSymptom != null)
                                                    {
                                                        IList<ConsultedSymptom> ConsultedSymptoms = ConsultationNoteManager.Instance.ListSymptomByNoteId(Consultation.Id);
                                                        if(ConsultedSymptoms.Count > 0)
                                                        {
                                                            foreach (ConsultedSymptom consultedSymptom in ConsultedSymptoms)
                                                            {
                                                                PatientVisitCountingLineItemForDiagnosis LineItem = new PatientVisitCountingLineItemForDiagnosis(consultedSymptom, Reg);
                                                                PatientVisitCountingLineItemForDiagnosiss.Add(LineItem);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class PatientVisitCountingLineItem
    {
        public DateTime DateOfVisit { get; set; }
        public int MaleAdult { get; set; } = 0;
        public int FemaleAdult { get; set; } = 0;
        public int MaleChild { get; set; } = 0;
        public int FemaleChild { get; set; } = 0;
        public int Others { get; set; } = 0;
        public long DeptId { get; set; }

        public PatientVisitCountingLineItem()
        {
        }
        public PatientVisitCountingLineItem(Registration Reg)
        {
            this.DateOfVisit = Reg.DateOfRegistration;
            if(Reg.Patient.Age > 10)
            {
                if (Reg.Patient.Gender == Gender.MALE)
                {
                    MaleAdult++;
                }
                if (Reg.Patient.Gender == Gender.FEMALE)
                {
                    FemaleAdult++;
                }
                if (Reg.Patient.Gender == Gender.OTHERS)
                {
                    Others++;
                }
            }
            else
            {
                if (Reg.Patient.Gender == Gender.MALE)
                {
                    MaleChild++;
                }
                if (Reg.Patient.Gender == Gender.FEMALE)
                {
                    FemaleChild++;
                }
                if (Reg.Patient.Gender == Gender.OTHERS)
                {
                    Others++;
                }
            }
            if(Reg.RequestedDoctor != null)
            {
                this.DeptId = (long)Reg.RequestedDoctor.DepartmentId;
            }
        }
    }
    public class PatientVisitCountingLineItemForDiagnosis
    {
        public DateTime DateOfVisit { get; set; }
        public int MaleAdult { get; set; } = 0;
        public int FemaleAdult { get; set; } = 0;
        public int MaleChild { get; set; } = 0;
        public int FemaleChild { get; set; } = 0;
        public int Others { get; set; } = 0;
        public long DiagId { get; set; }

        public PatientVisitCountingLineItemForDiagnosis()
        {
        }
        public PatientVisitCountingLineItemForDiagnosis(ConsultedSymptom consultedSymptom, Registration Reg)
        {
            this.DateOfVisit = Reg.DateOfRegistration;
            if (Reg.Patient.Age > 10)
            {
                if (Reg.Patient.Gender == Gender.MALE)
                {
                    MaleAdult++;
                }
                else if (Reg.Patient.Gender == Gender.FEMALE)
                {
                    FemaleAdult++;
                }
                else if (Reg.Patient.Gender == Gender.OTHERS)
                {
                    Others++;
                }
            }
            else
            {
                if (Reg.Patient.Gender == Gender.MALE)
                {
                    MaleChild++;
                }
                else if (Reg.Patient.Gender == Gender.FEMALE)
                {
                    FemaleChild++;
                }
                else if (Reg.Patient.Gender == Gender.OTHERS)
                {
                    Others++;
                }
            }
            this.DiagId = consultedSymptom.SymptomId;
        }
    }
}
