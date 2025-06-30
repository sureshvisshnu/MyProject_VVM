using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.Hms;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.report;
using Microsoft.EntityFrameworkCore;

namespace FADataAccessLibrary.report.Hms
{
    public class RptPatientProcedure : Report
    {
        public IList<PatientProcedureReportLineItem> LineItems { get; } = new List<PatientProcedureReportLineItem>();
        public List<ProcedureStatus> Status { get; set; }
        public long PatientId { get; set; }
        public long CompanyId { get; set; }
        public override string ReportTitle()
        {
            return String.Empty;
        }
        public override string ReportName()
        {
            return String.Format("PatientProcedureReport" + " " + DateTime.Now.ToString("dd-MM-yyyy").Replace(" / ", " - "));
        }
        public override void GenerateReport()
        {
            IList<ConsultedProcedure> ProcedureInfo = null;
            IList<Patient> patients = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                patients = PatientManager.Instance.ListAllPatient(CompanyId);
                foreach (Patient patient in patients)
                {
                    List<ConsultationNote> consultationNotes = Context.ConsultationNotes.Where(x => x.CompanyId == CompanyId && x.PatientId == patient.Id).ToList();
                    foreach (ConsultationNote consultationNote in consultationNotes)
                    {
                        ProcedureInfo = Context.ConsultedProcedures.Where(x => x.ConsultationNoteId == consultationNote.Id && x.Date >= FromDate && x.Date <= ToDate && x.CompanyId == CompanyId).ToList();
                        if (ProcedureInfo.Count > 0)
                        {
                            MedicalTeam IpMedicalTeam = null;
                            Registration OpMedicalTeam = null;
                            if (consultationNote.InPatientAdmissionId != null)
                            {
                                IpMedicalTeam = MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionId((long)consultationNote.InPatientAdmissionId);
                            }
                            else if(consultationNote.OpRegistrationId != null)
                            {
                                OpMedicalTeam = OpManager.Instance.GetOpRegistrationById((long)consultationNote.OpRegistrationId);
                            }
                            LoadConsultedProcedure(ProcedureInfo, Context, patient, IpMedicalTeam, OpMedicalTeam);
                        }
                    }
                }
            }
        }
        private void LoadConsultedProcedure(IList<ConsultedProcedure> ProcedureInfo, AccountMasterContext Context, Patient patient, MedicalTeam IpMedicalTeam, Registration OpMedicalTeam)
        {
            foreach (ConsultedProcedure Pro in ProcedureInfo)
            {
                IList<ConsultedProcedureHistory> ConsultedProcedureHistory = Context.ConsultedProcedureHistorys.Include("PerformedBy").Include("ConsultedProcedure").Where(x => x.ConsultedProcedureId == Pro.ConsultedProcedureId && x.CompanyId == CompanyId).ToList();
                PatientProcedureReportLineItem patientProcedureReportLineItem = new PatientProcedureReportLineItem();
                patientProcedureReportLineItem.Date = Pro.Date;
                patientProcedureReportLineItem.PatientId = patient.PatientNumber;
                patientProcedureReportLineItem.PatientName = patient.Name;
                patientProcedureReportLineItem.ProcedureName = Pro.Name;
                patientProcedureReportLineItem.Doctor = IpMedicalTeam != null ? (IpMedicalTeam?.PrimaryDoctor?.Name?? "") : (OpMedicalTeam != null ? OpMedicalTeam?.RequestedDoctor?.Name?? "" : "");
                patientProcedureReportLineItem.Nurse = IpMedicalTeam?.PrimaryCareGiver?.Name?? "";
                if (ConsultedProcedureHistory != null && ConsultedProcedureHistory.Count > 0)
                {
                    patientProcedureReportLineItem.Status = ConsultedProcedureHistory.Last().ProStatus.ToString();
                }
                else
                {
                    patientProcedureReportLineItem.Status = Pro.ProStatus.ToString();
                }
                LineItems.Add(patientProcedureReportLineItem);
            }
        }
    }
    public class PatientProcedureReportLineItem
    {
        public DateTime Date { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string ProcedureName { get; set; }
        public string Doctor { get; set; }
        public string Nurse { get; set; }
        public string Status { get; set; }
    }
}
