using fa.api.Hms;
using fa.model.Common;
using fa.model.Hms.Master;
using fa.report;
using FaData.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;

namespace FADataAccessLibrary.report.Hms
{
    public class RptPatientDetails : Report
    {
        public IList<RptPatientDetailsLineItems> PatientDetailsLineItems { get; } = new List<RptPatientDetailsLineItems>();
        public override string ReportTitle()
        {
            return String.Format("Patient Details");
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
            return String.Format("PatientDetailsReport {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            IList<Patient> PatientFromDB = PatientManager.Instance.ListAllPatient(Company.CompanyId);
            if (PatientFromDB != null && PatientFromDB.Count > 0)
            {
                List<Patient> filteredPatients = PatientFromDB.Where(x => x.CreatedDate >= FromDate && x.CreatedDate <= ToDate).OrderBy(x => x.CreatedDate).ToList();

                foreach (Patient lPatient in filteredPatients)
                {
                    List<Guardian> guardians = lPatient.Guardians?.ToList() ?? new List<Guardian>();
                    List<EmergencyContact> emergencyContacts = lPatient.EmergencyContact?.ToList() ?? new List<EmergencyContact>();

                    int maxRows = Math.Max(1, Math.Max(guardians.Count, emergencyContacts.Count));
                    bool firstRow = true;
                    for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
                    {
                        RptPatientDetailsLineItems lineItem = new RptPatientDetailsLineItems();
                        lineItem.Date = DateTime.Parse(lPatient.CreatedDate.ToString()).ToString(Company.DateFormat);
                        if (firstRow) 
                        {
                            lineItem.PatientName = lPatient.Name;
                            lineItem.PatientID = lPatient.PatientNumber;
                            lineItem.Gender = lPatient.Gender == Gender.MALE ? "Male" : (lPatient.Gender == Gender.FEMALE ? "Female" : (lPatient.Gender == Gender.OTHERS ? "Others" : ""));
                            lineItem.DOB = lPatient.DateOfBirth.Date.ToString(Company.DateFormat);
                            lineItem.Age = lPatient.Age.ToString();
                            lineItem.BloodGroup = lPatient.Bloodgroup;
                            lineItem.PAddress = lPatient.Address != null ? (lPatient?.Address?.FullAddress ?? "") : "";
                            lineItem.Mobile = lPatient.ContactInfo != null ? (!string.IsNullOrEmpty(lPatient.ContactInfo.Mobile) && !string.IsNullOrEmpty(lPatient.ContactInfo.Phone) ? (lPatient.ContactInfo.Mobile + "\n"+ lPatient.ContactInfo.Phone) : 
                                              (!string.IsNullOrEmpty(lPatient.ContactInfo.Mobile) ? lPatient.ContactInfo.Mobile : (!string.IsNullOrEmpty(lPatient.ContactInfo.Phone) ? lPatient.ContactInfo.Phone : ""))) : "";
                        }
                        if (rowIndex < guardians.Count)
                        {
                            var guardian = guardians[rowIndex];
                            Address GaudAddress = PatientManager.Instance.GetAddressById(guardian.AddressId);
                            string GName = guardian?.Name?? "";
                            string GPhone = guardian.ContactInfo != null ? ((!string.IsNullOrEmpty(guardian.ContactInfo.Mobile) && !string.IsNullOrEmpty(guardian.ContactInfo.Phone)) ? (guardian.ContactInfo.Phone + "\n" +guardian.ContactInfo.Mobile) : 
                                            (!string.IsNullOrEmpty(guardian.ContactInfo.Phone) ? guardian.ContactInfo.Phone : (!string.IsNullOrEmpty(guardian.ContactInfo.Mobile) ? guardian.ContactInfo.Mobile : ""))): "";
                            lineItem.GAddress = guardian != null ? (GName + "\n" + (GaudAddress != null ? ((GaudAddress.FullAddress) + "\n" + GPhone) : GPhone)) : "";
                        }
                        if (rowIndex < emergencyContacts.Count)
                        {
                            var emergencyContact = emergencyContacts[rowIndex];
                            Address EmergAddress = PatientManager.Instance.GetAddressById(emergencyContact.AddressId);
                            string EName = emergencyContact?.Name?? "";
                            string EPhone = emergencyContact.ContactInfo != null ? ((!string.IsNullOrEmpty(emergencyContact.ContactInfo.Mobile) && !string.IsNullOrEmpty(emergencyContact.ContactInfo.Phone)) ? (emergencyContact.ContactInfo.Phone + "\n" + emergencyContact.ContactInfo.Mobile) :
                                            (!string.IsNullOrEmpty(emergencyContact.ContactInfo.Phone) ? emergencyContact.ContactInfo.Phone : (!string.IsNullOrEmpty(emergencyContact.ContactInfo.Mobile) ? emergencyContact.ContactInfo.Mobile : ""))) : "";
                            lineItem.EAddress = emergencyContact != null ? (EName + "\n" + (EmergAddress != null ? ((EmergAddress.FullAddress) + "\n" + EPhone) : EPhone)) : "";
                        }
                        PatientDetailsLineItems.Add(lineItem);
                        firstRow = false;
                    }
                }
            }
        }
    }
    public class RptPatientDetailsLineItems
    {
        public string Date { get; set; }
        public string PatientName { get; set; }
        public string PatientID { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        public string BloodGroup { get; set; }
        public string PAddress { get; set; }
        public string Mobile { get; set; }
        public string GAddress { get; set; }
        public string EAddress { get; set; }
    }
}
