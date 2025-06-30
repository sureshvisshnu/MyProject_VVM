using fa.api.Accounting;
using fa.api.Hms;
using fa.context;
using fa.Data;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.report;
using fa.report.Hms;
using Fa.report.accounting.master;
using Fa.report.Hms;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace FADataAccessLibrary.report.Hms
{
    public class RptPatientDueList : Report
    {
        public string[] PatientType { get; set; }
        public long?[] WardId { get; set; }
        public long?[] PatientIds { get; set; }
        public DateTime TransactionDate { get; set; }

        public IList<RptPatientDueListLineItem> LineItems { get; } = new List<RptPatientDueListLineItem>();

        public override string ReportTitle()
        {
            return String.Format("Patient Due List");
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
            return String.Format("PatientDueList {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            List<PatientLedger> patientLedgers = null;
            double OpeningAmount = 0;
            double CurrentDueAmount = 0;
            bool isIp = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (PatientType.Contains("All") && PatientType.Contains("By OutPatient") && PatientType.Contains("By InPatient"))
                {
                    List<Patient> patients = Context.PatientLedgers.Where(x => x.CompanyId == Company.CompanyId && PatientIds.Contains(x.PatientId)).Select(x => x.Patient).Distinct().ToList<Patient>();
                    if (patients.Count > 0)
                    {
                        foreach (var patient in patients)
                        {
                            isIp = false;
                            Registration Registration = OpManager.Instance.GetPatientLastOp(patient.Id, TransactionDate.Date);
                            if (Registration != null)
                            {
                                InPatientAdmission InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId(Registration.Id);
                                isIp = InPatientAdmission == null ? false : true;
                            }
                            CurrentDueAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate && x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate && (x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            if (CurrentDueAmount == 0) { continue; }
                            OpeningAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && (x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            patientLedgers = Context.PatientLedgers.Include("Patient").Where(x => x.CompanyId == Company.CompanyId && x.Patient.Id == patient.Id && x.Date >= FromDate && x.Date <= ToDate).ToList();

                            if (patientLedgers != null && patientLedgers.Count > 0)
                            {
                                int dayCount = 0;
                                double Amount = 0;
                                string bed = "yyy";
                                int rowCount = 0;
                                foreach (PatientLedger ledger in patientLedgers)
                                {
                                    int LineCount = patientLedgers.Where(x => x.RentTimeDuration == "1 day" && x.Bed == ledger.Bed).Count();
                                    if (ledger.RentTimeDuration == "1 day")
                                    {
                                        rowCount++;
                                        dayCount++;
                                        if (bed == "yyy" || ledger.Bed == bed)
                                        {
                                            Amount += ledger.Amount;
                                            bed = ledger.Bed;
                                            if (rowCount == LineCount)
                                            {
                                                string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                                LineItems.Add(RptPatientDueListLineItem);
                                                Amount = 0;
                                                rowCount = 0;
                                                dayCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                            RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                            LineItems.Add(RptPatientDueListLineItem);
                                            bed = ledger.Bed;
                                            Amount = 0;
                                            dayCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, 0, "");
                                        double fee = (ledger.Type == TransactionType.PAYMENT || ledger.Type == TransactionType.WAIVER) ? -ledger.Amount : ledger.Amount;

                                        LineItems.Add(RptPatientDueListLineItem);
                                    }
                                }
                            }
                            else if (OpeningAmount != 0)
                            {
                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(patient, OpeningAmount, isIp, 0, "");
                                LineItems.Add(RptPatientDueListLineItem);
                            }
                        }
                    }
                }
                else if((PatientType.Contains("By InPatient") && !PatientType.Contains("By OutPatient")))
                {
                    isIp = true;
                    long?[] patientIds = Context.InPatientLocations.Where(x => WardId.Contains(x.WardId)).Select(x => x.InPatientAdmission.PatientId).Distinct().ToArray();
                    long?[] commonIds = patientIds.Intersect(PatientIds).ToArray();
                    List<Patient> patients = Context.Patients.Where(x => commonIds.Contains(x.Id)).ToList();
                    if (patients.Count > 0)
                    {
                        foreach (var patient in patients)
                        {
                            CurrentDueAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate && x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate && (x.Type == TransactionType.WAIVER)).Sum(x => x.Amount); 
                            if (CurrentDueAmount == 0) { continue; }
                            OpeningAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && (x.InPatientAdmissionId != null && x.OpRegistrationId != null)  && x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && (x.InPatientAdmissionId != null && x.OpRegistrationId != null) && (x.Type == TransactionType.PAYMENT || x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            double OpAmt = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && (x.InPatientAdmissionId == null  && x.Type != TransactionType.WAIVER)).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && (x.InPatientAdmissionId == null &&  x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            OpeningAmount = OpeningAmount + OpAmt;
                            patientLedgers = Context.PatientLedgers.Include("Patient").Where(x => x.CompanyId == Company.CompanyId && x.Patient.Id == patient.Id && x.Date >= FromDate && x.Date <= ToDate && (x.InPatientAdmissionId != null && x.OpRegistrationId != null)).ToList();

                            if (patientLedgers != null && patientLedgers.Count > 0)
                            {
                                int dayCount = 0;
                                double Amount = 0;
                                string bed = "yyy";
                                int rowCount = 0;
                                foreach (PatientLedger ledger in patientLedgers)
                                {
                                    int LineCount = patientLedgers.Where(x => x.RentTimeDuration == "1 day" && x.Bed == ledger.Bed).Count();
                                    if (ledger.RentTimeDuration == "1 day")
                                    {
                                        rowCount++;
                                        dayCount++;
                                        if (bed == "yyy" || ledger.Bed == bed)
                                        {
                                            Amount += ledger.Amount;
                                            bed = ledger.Bed;
                                            if (rowCount == LineCount)
                                            {
                                                string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                                LineItems.Add(RptPatientDueListLineItem);
                                                Amount = 0;
                                                rowCount = 0;
                                                dayCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                            RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                            LineItems.Add(RptPatientDueListLineItem);
                                            bed = ledger.Bed;
                                            Amount = 0;
                                            dayCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, 0, "");
                                        double fee = (ledger.Type == TransactionType.PAYMENT || ledger.Type == TransactionType.WAIVER) ? -ledger.Amount : ledger.Amount;
                                        LineItems.Add(RptPatientDueListLineItem);
                                    }
                                }
                            }
                            else if (OpeningAmount != 0)
                            {
                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(patient, OpeningAmount, isIp, 0, "");
                                LineItems.Add(RptPatientDueListLineItem);
                            }
                        }
                    }
                }
                else if ((PatientType.Contains("By OutPatient") && !PatientType.Contains("By InPatient")))
                {
                    isIp = false;
                    List<Patient> patients = Context.PatientLedgers.Select(x => x.Patient).Distinct().ToList<Patient>();
                    if (patients.Count > 0)
                    {
                        foreach (var patient in patients)
                        {
                            Registration Registration = OpManager.Instance.GetPatientLastOp(patient.Id, TransactionDate.Date);
                            if (Registration != null)
                            {
                                InPatientAdmission InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId(Registration.Id);
                                if (InPatientAdmission != null) { continue; }
                            }
                            CurrentDueAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate &&  x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date <= ToDate && (x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            if (CurrentDueAmount == 0) { continue; }
                            OpeningAmount = Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && (x.OpRegistrationId != null && x.InPatientAdmissionId == null)  && x.Type != TransactionType.WAIVER).Sum(x => x.Amount) - Context.PatientLedgers.Where(x => x.Patient.Id == patient.Id && x.Date < FromDate && (x.OpRegistrationId != null && x.InPatientAdmissionId == null) && ( x.Type == TransactionType.WAIVER)).Sum(x => x.Amount);
                            patientLedgers = Context.PatientLedgers.Include("Patient").Where(x => PatientIds.Contains(x.PatientId) && x.CompanyId == Company.CompanyId && x.Patient.Id == patient.Id && x.Date >= FromDate && x.Date <= ToDate && ((x.OpRegistrationId != null && x.InPatientAdmissionId == null) | (x.OpRegistrationId == null && x.InPatientAdmissionId == null))).ToList();

                            if (patientLedgers != null && patientLedgers.Count > 0)
                            {
                                int dayCount = 0;
                                double Amount = 0;
                                string bed = "yyy";
                                int rowCount = 0;
                                foreach (PatientLedger ledger in patientLedgers)
                                {
                                    int LineCount = patientLedgers.Where(x => x.RentTimeDuration == "1 day" && x.Bed == ledger.Bed).Count();
                                    if (ledger.RentTimeDuration == "1 day")
                                    {
                                        rowCount++;
                                        dayCount++;
                                        if (bed == "yyy" || ledger.Bed == bed)
                                        {
                                            Amount += ledger.Amount;
                                            bed = ledger.Bed;
                                            if (rowCount == LineCount)
                                            {
                                                string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                                LineItems.Add(RptPatientDueListLineItem);
                                                Amount = 0;
                                                rowCount = 0;
                                                dayCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                            RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, Amount, Description);
                                            LineItems.Add(RptPatientDueListLineItem);
                                            bed = ledger.Bed;
                                            Amount = 0;
                                            dayCount = 0;
                                        }
                                    }
                                    else
                                    {
                                        RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(ledger, OpeningAmount, isIp, 0, "");
                                        double fee = (ledger.Type == TransactionType.PAYMENT || ledger.Type == TransactionType.WAIVER) ? -ledger.Amount : ledger.Amount;

                                        LineItems.Add(RptPatientDueListLineItem);
                                    }
                                }
                            }
                            else if (OpeningAmount != 0)
                            {
                                RptPatientDueListLineItem RptPatientDueListLineItem = new RptPatientDueListLineItem(patient, OpeningAmount, isIp, 0, "");
                                LineItems.Add(RptPatientDueListLineItem);
                            }
                        }
                    }
                }
            }
        }
        public class RptPatientDueListLineItem
        {
            public DateTime Date { get; set; }
            public string Patientdetail { get; set; }
            public long? PatientId { get; set; }
            public String PatientNo { get; set; }
            public String Age { get; set; }
            public string FeeType { get; set; }
            public double Fee { get; set; }
            public String Description { get; set; }
            public double openingAmount { get; set; }
            public bool isIp {  get; set; }
            public RptPatientDueListLineItem(PatientLedger ledger, double OpeningAmount, bool isIp, double Amount, string description)
            {
                this.Date = Date;
                string Address = (ledger.Patient.AddressId != null ? AddressManager.Instance.GetAddressById((long)ledger.Patient.AddressId).FullAddressInSingleLine : "");
                this.Patientdetail = ledger.Patient.Name + (!string.IsNullOrEmpty(Address) ? ("\n" + Address) : "");
                this.PatientNo = ledger.Patient.PatientNumber;
                this.Age = ledger.Patient.Age.ToString();
                this.Fee = Amount == 0 ? ((ledger.Type == TransactionType.PAYMENT || ledger.Type == TransactionType.WAIVER) ? -ledger.Amount : ledger.Amount) : Amount;
                this.FeeType = ledger.Type == TransactionType.REGISTRATION_FEE ? "Registration fee" : ledger.Type == TransactionType.CONSULTATION_FEE ? "Consultation fee" : ledger.Type == TransactionType.PAYMENT ? "Payment" : ledger.Type == TransactionType.WAIVER ? "Waiver" : ledger.Type == TransactionType.MEDICALPROCEDURE_FEE ? "MedicalProcedure fee" : ledger.Type == TransactionType.ROOM_RENT ? "Room rent" : ledger.Type == TransactionType.LAB_FEE ? "Lab fee" : ledger.Type == TransactionType.PHARMACY_FEE ? "Pharmacy fee" : ledger.Type == TransactionType.MISCELLENEOUS ? "Miscelleneous" : "Room Cleaning Charge";
                this.Description = description == "" ? ledger.Description : description;
                this.PatientId = ledger.PatientId;
                this.openingAmount = OpeningAmount;
                this.isIp = isIp;
            }
            public RptPatientDueListLineItem(Patient Patient, double OpeningAmount, bool isIp, double Amount, string description)
            {
                this.Date = Date;
                string Address = (Patient.AddressId != null ? AddressManager.Instance.GetAddressById((long)Patient.AddressId).FullAddressInSingleLine : "");
                this.Patientdetail = Patient.Name + (!string.IsNullOrEmpty(Address) ? ("\n" + Address) : "");
                this.PatientNo = Patient.PatientNumber;
                this.Age = Patient.Age.ToString();
                this.Fee = Amount;
                this.FeeType = "";
                this.Description = description;
                this.PatientId = Patient.Id;
                this.openingAmount = OpeningAmount;
                this.isIp = isIp;
            }
        }
    }
}
