using fa.api.Hms;
using fa.context;
using fa.model.Accounting.Transactions;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.model.UserProfile;
using fa.report;
using fa.report.common;
using fa.report.Hms;
using fa.report.Inventory;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.report.Hms
{
    public enum InventoryReportFilterType
    {
        BYDATE, BYPATIENT, BYAMOUNT , BYCONSULTANT
    }
    public class RptReceiveAmount : Report
    {
        public InventoryReportFilterType Type { get; set; }

        //public long CompanyId;
        public long PatientId;
        
        public string ReportHeader { get; set; }
        public bool IsAllLocation { get; set; }
        public bool IsAllDoctor { get; set; }
        public long[] PatientIds { get; set; }
        public string Patient;
        public long[] DoctorIds { get; set; }
        public string Doctor;

        public List<ReceiveAmountReportLine> ReceiveAmountReportLines = null;
        public List<ReceiveAmountReportLineByConsult> ReceiveAmountReportLineByConsult = null;
        public override string ReportTitle()
        {
            return "Receive Amount " + (Type == InventoryReportFilterType.BYDATE ? "By Date" : Type == InventoryReportFilterType.BYPATIENT ? "By Patient" : Type == InventoryReportFilterType.BYAMOUNT ? "By Amount": "By Consultant");
        }
        public string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));

        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Receive Amount {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            if (Type == InventoryReportFilterType.BYPATIENT)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    List<long> patientIdList = PatientIds.ToList();
                    IList<PatientLedger> lPatientLedger = Context.PatientLedgers.Include("Patient").Include("OpRegistration").Include("Consultant").Include("InPatientAdmission").Where(x => x.CompanyId == Company.CompanyId && x.Date <= ToDate && x.Date >= FromDate && patientIdList.Contains(x.Patient.Id)).ToList();
                    if (lPatientLedger != null && lPatientLedger.Count > 0)
                    {
                        ReceiveAmountReportLines = new List<ReceiveAmountReportLine>();
                        foreach (PatientLedger detail in lPatientLedger)
                        {
                            List<PatientPaymentDetail> paymentDetails = Context.PatientPaymentDetails.Where(x => x.CompanyId == Company.CompanyId && x.PatientLedgerId == detail.Id).ToList();
                            foreach (PatientPaymentDetail paymentDetail in paymentDetails)
                            {
                                if (paymentDetail != null)
                                {
                                    ReceiveAmountReportLine receiveAmountReportLine = new ReceiveAmountReportLine(detail, paymentDetail);
                                    ReceiveAmountReportLines.Add(receiveAmountReportLine);
                                }
                            }
                        }
                    }
                }
            }
            else if (Type == InventoryReportFilterType.BYCONSULTANT)
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    List<long> DoctorIdList = DoctorIds.ToList();
                    List<long> patientId = Context.PatientLedgers.Where(x => DoctorIdList.Contains((long)x.ConsultantId)).Select(x=> x.PatientId).ToList<long>();
                    IList<PatientLedger> lPatientLedger = Context.PatientLedgers.Include("Patient").Include("OpRegistration").Include("Consultant").Include("InPatientAdmission").Where(x => x.CompanyId == Company.CompanyId && x.Date <= ToDate && x.Date >= FromDate && patientId.Contains(x.PatientId) && x.ConsultantId == null).ToList<PatientLedger>();
                    if (lPatientLedger != null && lPatientLedger.Count > 0)
                    {
                        ReceiveAmountReportLineByConsult = new List<ReceiveAmountReportLineByConsult>();
                        foreach (PatientLedger detail in lPatientLedger)
                        {
                            List<PatientPaymentDetail> paymentDetails = Context.PatientPaymentDetails.Where(x => x.CompanyId == Company.CompanyId && x.PatientLedgerId == detail.Id).ToList();
                            foreach (PatientPaymentDetail paymentDetail in paymentDetails)
                            {
                                if (paymentDetail != null)
                                {
                                    ReceiveAmountReportLineByConsult receiveAmountReportLine = new ReceiveAmountReportLineByConsult(detail, paymentDetail);
                                    ReceiveAmountReportLineByConsult.Add(receiveAmountReportLine);

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                using (AccountMasterContext Context = new AccountMasterContext())
                {
                    List<long> patientIdList = PatientIds.ToList();
                    IList<PatientLedger> lPatientLedger = Context.PatientLedgers.Include("Patient").Include("OpRegistration").Include("InPatientAdmission").Include("PatientPaymentDetails").Where(x => x.CompanyId == Company.CompanyId && x.Date <= ToDate && x.Date >= FromDate).ToList();
                    if (lPatientLedger != null && lPatientLedger.Count > 0)
                    {
                        ReceiveAmountReportLines = new List<ReceiveAmountReportLine>();
                        foreach (PatientLedger detail in lPatientLedger)
                        {
                            List<PatientPaymentDetail> paymentDetails = Context.PatientPaymentDetails.Where(x => x.CompanyId == Company.CompanyId && x.PatientLedgerId == detail.Id).ToList();
                            foreach (PatientPaymentDetail paymentDetail in paymentDetails)
                            {
                                if(paymentDetail != null)
                                {
                                    ReceiveAmountReportLine receiveAmountReportLine = new ReceiveAmountReportLine(detail, paymentDetail);
                                    ReceiveAmountReportLines.Add(receiveAmountReportLine);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class ReceiveAmountReportLine
    {
        public long Id { get; set; }
        public Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public User Consultant { get; set; }
        public string RefNumber { get; set; }
        public double Received { get; set; }
        public PaymentType PaymentType { get; set; }

        public ReceiveAmountReportLine()
        {
        }
        public ReceiveAmountReportLine(PatientLedger detail, PatientPaymentDetail paymentDetail)
        {
            this.Id = detail.Id;
            this.Date = detail.Date;
            this.Patient = detail.Patient;
            this.Description = detail.Description;
            this.RefNumber = detail.RefNumber;
            this.Received = detail.Amount;
            this.Consultant = detail.Consultant;
            this.PaymentType = paymentDetail.PaymentType;
        }
    }
    public class ReceiveAmountReportLineByConsult
    {
        public long Id { get; set; }
        public Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public User Consultant { get; set; }
        public string RefNumber { get; set; }
        public double Received { get; set; }
        public PaymentType PaymentType { get; set; }

        public ReceiveAmountReportLineByConsult()
        {
        }
        public ReceiveAmountReportLineByConsult(PatientLedger detail, PatientPaymentDetail paymentDetail)
        {
            this.Id = detail.Id;
            this.Date = detail.Date;
            this.Patient = detail.Patient;
            this.Description = detail.Description;
            this.RefNumber = detail.RefNumber;
            this.Received = detail.Amount;
            this.Consultant = detail.Consultant;
            this.PaymentType = paymentDetail.PaymentType;
        }
    }
}
