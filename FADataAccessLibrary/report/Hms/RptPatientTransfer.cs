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

namespace FADataAccessLibrary.report.Hms
{
    public enum TransferType
    {
        BYDATE, BYWARD, BYCONSULTANT
    }
    public class RptPatientTransfer : Report
    {
        public long[] WardIds { get; set; }
        public string Ward;
        public bool IsAllWard { get; set; }
        public long[] ConsultantIds { get; set; }
        public string AuthorizedByDoctor;
        public bool IsAllConsultant { get; set; }
        public TransferType Type { get; set; }
        public string ReportHeader { get; set; }

        public List<PatientTransferLineItem> PatientTransferLineItems = null;
        public override string ReportTitle()
        {
            return "Transfer Patient " + (Type == TransferType.BYDATE ? "By Date" : Type == TransferType.BYWARD ? "By Ward" : "By Consultant");
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
            return String.Format("Transfer Patient Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(Type == TransferType.BYDATE)
                {
                    IList<InPatientAdmission> InPatientAdmissions = (Context.InPatientAdmissions.Include("Patient").Where(pa => pa.DateOfAdmission >= this.FromDate.Date && pa.Company == Company)).ToList();
                    if (InPatientAdmissions != null && InPatientAdmissions.Count > 0)
                    {
                        PatientTransferLineItems = new List<PatientTransferLineItem>();
                        foreach (InPatientAdmission detail in InPatientAdmissions)
                        {
                            List<InPatientLocation> InPatientLocation = Context.InPatientLocations.Include("InPatientAdmission").Include("Ward").Include("Bed").Include("AuthorizedByDoctor").Where(x => x.AdmissionId == detail.Id).ToList();
                            if (InPatientLocation != null)
                            {
                                foreach (InPatientLocation locationdetail in InPatientLocation)
                                {
                                    PatientTransferLineItem LineItem = new PatientTransferLineItem(detail, locationdetail);
                                    PatientTransferLineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                }
                else if (Type == TransferType.BYWARD)
                {
                    List<long> wardId = WardIds.ToList();
                    IList<InPatientAdmission> InPatientAdmissions = (Context.InPatientAdmissions.Include("Patient").Where(pa => pa.DateOfAdmission >= this.FromDate.Date && pa.Company == Company)).ToList();
                    if (InPatientAdmissions != null && InPatientAdmissions.Count > 0)
                    {
                        PatientTransferLineItems = new List<PatientTransferLineItem>();
                        foreach (InPatientAdmission detail in InPatientAdmissions)
                        {
                            IList<InPatientLocation> InPatientLocation = Context.InPatientLocations.Include("InPatientAdmission").Include("Ward").Include("Bed").Include("AuthorizedByDoctor").Where(x => x.AdmissionId == detail.Id && wardId.Contains(x.Ward.Id)).ToList();
                            if (InPatientLocation != null)
                            {
                                foreach (InPatientLocation locationdetail in InPatientLocation)
                                {
                                    PatientTransferLineItem LineItem = new PatientTransferLineItem(detail, locationdetail);
                                    PatientTransferLineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<long> conslutantId = ConsultantIds.ToList();
                    IList<InPatientAdmission> InPatientAdmissions = (Context.InPatientAdmissions.Include("Patient").Where(pa => pa.DateOfAdmission >= this.FromDate.Date && pa.Company == Company)).ToList();
                    if (InPatientAdmissions != null && InPatientAdmissions.Count > 0)
                    {
                        PatientTransferLineItems = new List<PatientTransferLineItem>();
                        foreach (InPatientAdmission detail in InPatientAdmissions)
                        {
                            List<InPatientLocation> InPatientLocation = Context.InPatientLocations.Include("InPatientAdmission").Include("Ward").Include("Bed").Include("AuthorizedByDoctor").Where(x => x.AdmissionId == detail.Id && conslutantId.Contains(x.AuthorizedByDoctor.Id)).ToList();
                            if (InPatientLocation != null)
                            {
                                foreach (InPatientLocation locationdetail in InPatientLocation)
                                {
                                    PatientTransferLineItem LineItem = new PatientTransferLineItem(detail, locationdetail);
                                    PatientTransferLineItems.Add(LineItem);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class PatientTransferLineItem
    {
        public String WardName { get; set; }
        public String BedName { get; set; }
        public String PatientNumber { get; set; }
        public String PatientName { get; set; }
        public String Notes { get; set; }
        public String AuthorizedDoctor { get; set; }
        public DateTime AdmittedOn { get; set; }
        public DateTime DischargedOn { get; set; }
        public bool AreActive { get; set; }
        public long patientId { get; set; }
        public PatientTransferLineItem()
        {
        }
        public PatientTransferLineItem(InPatientAdmission detail , InPatientLocation locationdetail)
        {
            this.WardName = locationdetail.Ward.Name;
            this.BedName = locationdetail.Bed.Name;
            this.Notes = locationdetail.Notes;
            this.AuthorizedDoctor = locationdetail.AuthorizedByDoctor.Name;
            this.AdmittedOn = locationdetail.DateMovedIn;
            this.PatientName = detail.Patient.Name;
            this.PatientNumber = detail.Patient.PatientNumber;
            this.DischargedOn = locationdetail.DateMovedOut.Date;
            this.AreActive = locationdetail.Active;
            this.patientId = (long)detail.PatientId;
        }
    }
}
