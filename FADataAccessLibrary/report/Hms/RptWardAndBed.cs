using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.api.Hms;
using fa.context;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.report;
using fa.report.Ip;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using static fa.report.Hms.RptWardAndBed;

namespace fa.report.Hms
{
    public class RptWardAndBed : Report
    {
        public long?[] WardId { get; set; }
        public List<WardBedReport> LineItems = new List<WardBedReport>();
        public override string ReportTitle()
        {
            return string.Format("Form Ward and Bed Report");
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
            return String.Format("Ward and Bed Report {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                string wardname = string.Empty;
                string bedname = string.Empty;
                string bedType = string.Empty;

                foreach (var wardId in WardId)
                {
                    Ward wards = WardManager.Instance.GetWardById((long)wardId);
                    if (wards != null)
                    {
                        foreach (Bed bed in wards.Beds)
                        {
                            IList<Rent> bedTypeRents = Context.Rents.Include("BedType").Where(b => b.BedTypeId == bed.BedTypeId).ToList();
                            if (bedTypeRents != null)
                            {
                                bool flag = true;
                                foreach (Rent bedTypeRent in bedTypeRents)
                                {
                                    WardBedReport WardBedReport = new WardBedReport();

                                    WardBedReport.Ward = wardname == string.Empty || wards.Name != wardname
                                        ? wards.Name
                                        : string.Empty;
                                    wardname = wards.Name;

                                    WardBedReport.BedNumber = bedname == string.Empty || bed.Name != bedname
                                        ? bed.Name
                                        : string.Empty;
                                    bedname = bed.Name;

                                    WardBedReport.BedType = bedType == string.Empty || bedTypeRent.BedType.ToString() != bedType
                                        ? bedTypeRent.BedType
                                        : null;
                                    bedType = bedTypeRent.BedType.ToString();

                                    WardBedReport.RentType = bedTypeRent.RentPeriod;
                                    WardBedReport.Rent = bedTypeRent.Amount;

                                    List<InPatientLocation> inPatientLocations = null;
                                    inPatientLocations = Context.InPatientLocations.Include("InPatientAdmission").Where(i => i.BedId == bed.Id && i.WardId == wardId && i.Active == true).ToList();
                                    
                                    if (inPatientLocations.Count > 0  && inPatientLocations.Any())
                                    {
                                        if (flag)
                                        {
                                            foreach (InPatientLocation inPatientLocation in inPatientLocations)
                                            {
                                                if (inPatientLocation.InPatientAdmission.DateOfAdmission.Date < ToDate)
                                                {
                                                    WardBedReport.AdmittedOn = inPatientLocation.InPatientAdmission.DateOfAdmission.Date;

                                                    Patient patient = Context.Patients.FirstOrDefault(
                                                        p => p.Id == inPatientLocation.InPatientAdmission.PatientId);

                                                    if (patient != null)
                                                    {
                                                        WardBedReport.PatientName = patient.Name;
                                                        WardBedReport.IPNumber = patient.PatientNumber;
                                                        WardBedReport.Available = "No";
                                                    }
                                                }
                                                else
                                                {
                                                    WardBedReport.Available = "Yes";
                                                }
                                            }
                                        }
                                        flag = false;
                                    }
                                    else
                                    {
                                        WardBedReport.Available = "Yes";
                                    }
                                    LineItems.Add(WardBedReport);
                                }
                            }
                        }
                    }
                }
            }
        }
        public class WardBedReport
        {
            public String Ward { get; set; }
            public String BedNumber { get; set; }
            public BedType BedType { get; set; }
            public RentPeriod RentType { get; set; }
            public float Rent { get; set; }
            public DateTime AdmittedOn { get; set; }
            public String IPNumber { get; set; }
            public String PatientName { get; set; }
            public String Available { get; set; }
        }
    }
}
