using fa.context;
using fa.model.hms.common;
using fa.model.Hms.Ip;
using fa.report;
using FaData.Utils;
using MySqlConnector;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using fa.model.Hms.Master;
using static NPOI.HSSF.Util.HSSFColor;

namespace FADataAccessLibrary.report.Hms
{
    public class RptFeeCollection : Report
    {
        public int FilterIndex { get; set; }
        public long[] ConsultantId { get; set; }
        public string[] FeeTypes { get; set; }

        public List<FeeChargeReportLineItem> LineItems = new List<FeeChargeReportLineItem>();
        public override string ReportTitle()
        {
            return string.Format("Fee Charge Report");
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
            return String.Format("Fee Charge Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            string consultantName = string.Empty;
            List<ConsultedConsultationFee> consultedConsultation = null!;
            List<ConsultedProcedure> consultedprocedures = null!;
            List<ConsultationNote> ConsultationNotes = null!;
            string FeeType = string.Empty;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (FilterIndex == 1 && ConsultantId.Length > 0)
                {
                    ConsultationNotes = Context.ConsultationNotes.Include("Patient").Include("ConsultedConsultationFee").Include("ConsultedConsultationFee.Consultation").Include("ConsultedProcedure").Include("ConsultedProcedure.MedicalProcedure").Include("Consultant").Include("InPatientAdmission").Include("OpRegistration").Where(x => x.CompanyId == this.Company.CompanyId && !x.IsDischarged && x.Date >= this.FromDate && x.Date <= this.ToDate && ConsultantId.Contains(x.ConsultantId)).OrderByDescending(x => x.Date).ToList();
                }
                else
                {
                    ConsultationNotes = Context.ConsultationNotes.Include("Patient").Include("ConsultedConsultationFee").Include("ConsultedConsultationFee.Consultation").Include("ConsultedProcedure").Include("ConsultedProcedure.MedicalProcedure").Include("Consultant").Include("InPatientAdmission").Include("OpRegistration").Where(x => x.CompanyId == this.Company.CompanyId && !x.IsDischarged && x.Date >= this.FromDate && x.Date <= this.ToDate).OrderByDescending(x => x.Date).ToList();
                }
                foreach (ConsultationNote consultationNote in ConsultationNotes)
                {
                    consultedprocedures = consultationNote.ConsultedProcedure.ToList();
                    consultedConsultation = consultationNote.ConsultedConsultationFee.ToList();
                    if ((consultedprocedures != null && consultedprocedures.Count > 0) || (consultedConsultation != null && consultedConsultation.Count > 0))
                    {
                        foreach (ConsultedProcedure consultedprocedure in consultedprocedures)
                        {
                            FeeType = consultedprocedure.MedicalProcedure.Name;
                            FeeChargeReportLineItem feeChargeReportLineItem = new FeeChargeReportLineItem(consultationNote, consultedprocedure.Fees, FeeType);
                            {
                                if (FilterIndex == 2)
                                {
                                    if (FeeTypes.Contains("Procedure Fee"))
                                    {
                                        LineItems.Add(feeChargeReportLineItem);
                                    }
                                }
                                else
                                {
                                    LineItems.Add(feeChargeReportLineItem);
                                }
                            }
                        }
                        foreach (ConsultedConsultationFee Consultation in consultedConsultation)
                        {
                            FeeType = Consultation.Consultation.Name;
                            FeeChargeReportLineItem feeChargeReportLineItem = new FeeChargeReportLineItem(consultationNote, Consultation.Fee, FeeType);
                            {
                                if (FilterIndex == 2)
                                {
                                    if (FeeTypes.Contains(Consultation.Consultation.Name))
                                    {
                                        LineItems.Add(feeChargeReportLineItem);
                                    }
                                }
                                else
                                {
                                    LineItems.Add(feeChargeReportLineItem);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class FeeChargeReportLineItem
    {
        public DateTime Date { get; set; }
        public string PatientName { get; set; }
        public string OpIp { get; set; }
        public string ConsultantName { get; set; }
        public string FeeType { get; set; }
        public double Fee { get; set; }

        public FeeChargeReportLineItem(ConsultationNote consultationNote, double Fees, string FeeType)
        {
            this.Date = consultationNote.Date;
            this.PatientName = consultationNote?.Patient?.Name?? "";
            this.OpIp = consultationNote.InPatientAdmissionId != null ? "IP" : "OP";
            this.ConsultantName = consultationNote.ConsultantName();
            this.FeeType = FeeType;
            this.Fee = Fees;
        }
    }
}
