using fa.context;
using fa.model.hms.common;
using Microsoft.EntityFrameworkCore;

namespace fa.report.Hms
{
    public class RptConsultedProcedure : Report
    {
        public IList<ConsultedProcedureLineItem> LineItems { get; } = new List<ConsultedProcedureLineItem>();
        public long PatientId { get; set; }
        public long CompanyId { get; set; }
        public List<ProcedureStatus> ProceduresStatuses { get; set; }
        public override string ReportTitle()
        {
            return String.Empty;
        }
        public override string ReportName()
        {
            return String.Empty;
        }
        public override void GenerateReport()
        {
            IList<ConsultedProcedure> ProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProcedureInfo = Context.ConsultedProcedures
                    .Include("MedicalProcedure")
                    .Include("RequestedBy")
                    .Include("PerformedBy")
                    .Where(x => x.ConsultationNote.PatientId == PatientId
                                && x.Date >= FromDate
                                && x.Date <= ToDate
                                && x.CompanyId == CompanyId)
                    .ToList();

                if (ProcedureInfo != null)
                {
                    LoadConsultedProcedure(ProcedureInfo, Context);
                }
            }
        }
        private void LoadConsultedProcedure(IList<ConsultedProcedure> ProcedureInfo,AccountMasterContext Context)
        {
            foreach(ConsultedProcedure Pro in ProcedureInfo)
            {
                IList<ConsultedProcedureHistory> ConsultedProcedureHistory= Context.ConsultedProcedureHistorys.Include("PerformedBy").Include("ConsultedProcedure").Where(x => x.ConsultedProcedureId == Pro.ConsultedProcedureId && x.CompanyId == CompanyId).ToList();
                ConsultedProcedureLineItem ConsultedProcedureLineItem = new ConsultedProcedureLineItem();
                ConsultedProcedureLineItem.Id = Pro.ConsultedProcedureId;
                ConsultedProcedureLineItem.Name = Pro.Name;
                ConsultedProcedureLineItem.Desc = Pro.Description;
                ConsultedProcedureLineItem.Date = Pro.Date;
                ConsultedProcedureLineItem.RequestedBy = Pro.RequestedBy!=null? Pro.RequestedBy.Name:"";
                if (ConsultedProcedureHistory != null && ConsultedProcedureHistory.Count > 0)
                {
                    ConsultedProcedureLineItem.PerformBy = ConsultedProcedureHistory.Last().PerformedBy != null ? ConsultedProcedureHistory.Last().PerformedBy.Name : "";
                    ConsultedProcedureLineItem.PerformOn = ConsultedProcedureHistory.Last().PerformOn;
                    ConsultedProcedureLineItem.Note = ConsultedProcedureHistory.Last().Note;
                    ConsultedProcedureLineItem.Status = ConsultedProcedureHistory.Last().ProStatus;
                }
                else
                {
                    ConsultedProcedureLineItem.PerformBy = Pro.PerformedBy != null ? Pro.PerformedBy.Name : "";
                    ConsultedProcedureLineItem.PerformOn = Pro.PerformOn;
                    ConsultedProcedureLineItem.Note = Pro.Note;
                    ConsultedProcedureLineItem.Status = Pro.ProStatus;
                }
                ConsultedProcedureLineItem.Fee = Pro.Fees;
                LineItems.Add(ConsultedProcedureLineItem);
            }
        }
    }    
    public class ConsultedProcedureLineItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime Date { get; set; }
        public string RequestedBy { get; set; }
        public string PerformBy { get; set; }
        public DateTime? PerformOn { get; set; }
        public string Note { get; set; }
        public ProcedureStatus Status { get; set; }
        public Double Fee { get; set; }

    }
}
