using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using fa.model.Hms.Master;

namespace fa.model.Hms.Op
{
    public class Registration : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public DateTime DateOfRegistration {get;set;}
        public string ReasonForTheVisit { get; set; }
        public string TockenNo { get; set; }
        public long? RequestedDoctorId { get; set; }
        [ForeignKey("RequestedDoctorId")]
        public Employee.Employee RequestedDoctor { get; set; }
        public Status Status { get; set; }
        public bool HasConsulted { get; set; }
        public bool IsNurseActivitiesCompleted { get; set; }
        public bool IsTechnicianActivitiesCompleted { get; set; }
        public bool IsFeePaid { get; set; }
        public bool IsFeeInsurance { get; set; }
        public bool HasRegistrationFeePaid { get; set; }
        public double RegistrationFee { get; set; }
        public bool IsBillToInsurance { get; set; }
        public long? InsuranceInfoId { get; set; }
        [ForeignKey("InsuranceInfoId")]
        public InsuranceInfo InsuranceInfo { get; set; }
        [MaxLength(20)]
        public string PatientOPNumber { get; set; }

    }
    public class PatientOPId
    {
        [Key, Column(Order = 0)]
        public long CompanyId { get; set; }
        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }
        public int NextNumber { get; set; }
        [NotMapped]
        public string PatientOPNo
        {
            get
            {
                return String.Format("O"+"{0:00}", CompanyId) + String.Format("{0:00}", Date.Day) + String.Format("{0:00}", Date.Month) + String.Format("{0:00}", Date.Year % 1000) + String.Format("{0:0000}", NextNumber);
            }
        }
    }
    public enum Status
    {
        ALL, OPEN, COMPLETED, INPATIENT
    }
    public class PatientAppointment : AuditableEntityForCompany
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public DateTime FromDateOfAppointment { get; set; }
        public DateTime ToDateOfAppointment { get; set; }
        public string StartingTime { get; set; }
        public string EndTime { get; set; }
        public string ReasonForTheAppointment { get; set; }
        public long ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]
        public virtual fa.model.Employee.Employee Consultant { get; set; }

        [NotMapped]
        public TimeSpan TimeDuration
        {
            get
            {
                var format = "hh:mm tt";
                var culture = CultureInfo.InvariantCulture;

                DateTime startDateTime = DateTime.ParseExact(StartingTime, format, culture);
                DateTime endDateTime = DateTime.ParseExact(EndTime, format, culture);

                DateTime fullStart = FromDateOfAppointment.Date.Add(startDateTime.TimeOfDay);
                DateTime fullEnd = ToDateOfAppointment.Date.Add(endDateTime.TimeOfDay);

                return fullEnd - fullStart;
            }
        }

        public bool IsConsulted { get; set; }
    }
}
