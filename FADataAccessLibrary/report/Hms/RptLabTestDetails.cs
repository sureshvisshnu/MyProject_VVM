using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using fa.api.Hms;
using fa.context;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.report;
using fa.report.Inventory;
using Fa.api.Hms;
using Fa.report.accounting.master;
using FaData.Utils;
using FADataAccessLibrary.report.Inventory;
using Microsoft.EntityFrameworkCore;
using static FADataAccessLibrary.report.Hms.RptLabTestDetails.RptLabTestLineItem;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;

namespace FADataAccessLibrary.report.Hms
{
    public enum LabTestReportFilterType
    {
        BYDATE, BYPATIENT, BYTESTNAME
    }
    public class RptLabTestDetails : Report
    {
        public int FilterIndex = 0;
        public long[] PatientId { get; set; }
        public long[] LabTestId { get; set; }
        public LabTestReportFilterType Type { get; set; }
        // public IList<RptLabTestLineItem> LineItems { get; } = new List<RptLabTestLineItem>();
        public List<RptLabTestLineItem> LineLabTestReport = new List<RptLabTestLineItem>();
        public override string ReportTitle()
        {
            return string.Format("Medical Lab Test Report");
        }
        public string ReportTitle(LabTestReportFilterType Type)
        {
            return "Lab Test " + (Type == LabTestReportFilterType.BYDATE ? "By Date" : Type == LabTestReportFilterType.BYPATIENT ? "By Patient" : "By Test Name");
        }
        public string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", FaData.Utils.DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));

        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("LabTestReport {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {

                List<PatientLedger> patientLedgers = new List<PatientLedger>();

                if (FilterIndex == 0)
                {
                    patientLedgers = Context.PatientLedgers
                        .Include("Patient")
                        .Include("ConsultedLabTest")
                        .Where(x => x.CompanyId == Company.CompanyId
                                    && x.Date >= FromDate
                                    && x.Date <= ToDate
                                    && x.Type == TransactionType.LAB_FEE)
                        .OrderBy(x => x.Date)  // Order the results by date
                        .ToList();
                    
                }
                else if (FilterIndex == 1)
                {
                    patientLedgers = Context.PatientLedgers
                        .Include("Patient")
                        .Include("ConsultedLabTest")
                        .Where(x => x.CompanyId == Company.CompanyId
                                    && x.Date >= FromDate
                                    && x.Date <= ToDate
                                    && x.Type == TransactionType.LAB_FEE
                                    && PatientId.Contains(x.PatientId)) // Filter by PatientId
                        .ToList();
                    
                }
                else if (FilterIndex == 2)
                {
                    patientLedgers = Context.PatientLedgers
                        .Include("Patient")
                        .Include("ConsultedLabTest")
                        .Where(x => x.CompanyId == Company.CompanyId
                                    && x.Date >= FromDate
                                    && x.Date <= ToDate
                                    && x.Type == TransactionType.LAB_FEE
                                    && LabTestId.Contains(x.ConsultedLabTest.MedicalTestId)) // Filter by MedicalTestId
                        .ToList();
                }

                if (patientLedgers != null && patientLedgers.Count > 0)
                {
                    foreach (PatientLedger ledger in patientLedgers)
                    {
                        IList<ConsultationNote> ConsultationNotes = ConsultationNoteManager.Instance.ListNotesEntryByPatientId((long)ledger.PatientId); //  IList<ConsultationNote> ConsultationNotes = ConsultationNoteManager.Instance.ListNotesEntryByMedicalTest((long)ledger.PatientId, (long)ledger.ConsultationNoteId);

                        if (ConsultationNotes != null && ConsultationNotes.Count > 0)
                        {
                            foreach (ConsultationNote Note in ConsultationNotes.Where(n => n.Date >= FromDate && n.Date <= ToDate))
                            {
                                //MedicalTestUOM medicalTestUOM = null!;
                                IList<ConsultedLabTest> lConsultedLabTest = ConsultationNoteManager.Instance.ListMedicalTestsByConsultedLabTestId(Note.Id, (long)ledger.ConsultedLabTestId);
                                
                                if (lConsultedLabTest != null && lConsultedLabTest.Count > 0)
                                {
                                    foreach (ConsultedLabTest ConsLabTest in lConsultedLabTest)
                                    {
                                        MedicalTest lmedicalTest = MedicalTestManager.Instance.GetMedicalTestById(ConsLabTest.MedicalTestId);
                                        RptLabTestLineItem ReportLabTestLineItem = new RptLabTestLineItem(); // Initialize here
                                        ReportLabTestLineItem.Date = Note.Date;
                                        ReportLabTestLineItem.PatientId = Note.PatientId != 0 ? Note.PatientId.ToString() : string.Empty;
                                        ReportLabTestLineItem.Patient = ledger.Patient?.ToString() ?? string.Empty;
                                        ReportLabTestLineItem.TestName = ConsLabTest.Name?.ToString() ?? string.Empty;
                                        ReportLabTestLineItem.HasElemnet = ConsLabTest.HasElement;
                                        ReportLabTestLineItem.PatientType = ledger.Patient?.ToString() ?? string.Empty;
                                        ReportLabTestLineItem.TestType = ConsLabTest.Name?.ToString() ?? string.Empty;
                                        ReportLabTestLineItem.Elements = new List<Element>();
                                        List<ConsultedLabTestElements> labTestElements = ConsultationNoteManager.Instance.GetConsultedLabTestElementsByConsLabTestId(ConsLabTest.ConsultedLabTestId).ToList();
                                        foreach (ConsultedLabTestElements labTestElement in labTestElements)
                                        {
                                            if (labTestElement != null)
                                            {
                                                Element element = new Element();
                                                element.ElementName = labTestElement.Name?.ToString() ?? string.Empty;
                                                element.Class = labTestElement.Class?.ToString() ?? string.Empty;
                                                element.SubClass = labTestElement.SubClass?.ToString() ?? string.Empty;
                                                element.UOM = labTestElement.Uom?.Name?.ToString() ?? string.Empty;
                                                element.SingleValue = labTestElement.SingleValue?.ToString() ?? string.Empty;
                                                element.RangeFrom = labTestElement.RangeFrom?.ToString() ?? string.Empty;
                                                element.RangeTo = labTestElement.RangeTo?.ToString() ?? string.Empty;
                                                element.ResultDescription = labTestElement.ResultDescription?.ToString() ?? string.Empty;
                                                ReportLabTestLineItem.Elements.Add(element);
                                            }
                                        }
                                        LineLabTestReport.Add(ReportLabTestLineItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private RptLabTestLineItem CreateLabTestReport(AccountMasterContext Context, ConsultedLabTestElements LabTestElementInfo, ConsultationNote Notes, ref DateTime? TempDate)
        {
            RptLabTestLineItem ReportLabTestLineItem = new RptLabTestLineItem();

            return ReportLabTestLineItem;
        }

        public class RptLabTestLineItem
        {
            public DateTime Date { get; set; }
            public string Patient { get; set; }
            public string PatientId { get; set; }
            public long LabTestId { get; set; }
            public string TestName { get; set; }
            public bool HasElemnet { get; set; }
            public double Fees { get; set; }
            public String PatientType { get; set; }
            public String TestType { get; set; }
            public List<Element> Elements { get; set; } = new List<Element>();

            public class Element
            {
                public string ElementName { get; set; }
                public string Class { get; set; }
                public string SubClass { get; set; }
                public string UOM { get; set; }
                public string SingleValue { get; set; }
                public string RangeFrom { get; set; }
                public string RangeTo { get; set; }
                public string ResultDescription { get; set; }
            }
        }
    }
}
