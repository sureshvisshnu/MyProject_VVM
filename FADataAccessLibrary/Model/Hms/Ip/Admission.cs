using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.api.Hms;

namespace fa.model.Hms.Ip
{
    public class InPatientAdmission: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string AdmissionNote { get; set; }        
        public InPatientStatus Status { get; set; }
        [NotMapped]
        public InPatientLocation CurrentLocation
        {
            get
            {
                if(Id!=0L)
                {
                    return IpManager.Instance.GetInPatientLocationbyAdmissionId(Id);
                }
                return null;
            }
            set { }
        }
        public virtual ICollection<InPatientLocation> PatientLocationHistory { get; set; }
        public long OpRegistrationId { get; set; }
        [ForeignKey("OpRegistrationId")]
        public Registration OpRegistration { get; set; }
        public virtual ICollection<MedicalTeam> MedicalTeamHistory { get; set; }
        [NotMapped]
        public MedicalTeam CurrentMedicalTeam
        {
            get
            {
                if (Id != 0L)
                {
                    return MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionId(Id);
                }
                return null;
            }
            set { }
        }
        public bool IsBillToInsurance { get; set; }
        public long? InsuranceInfoId { get; set; }
        [ForeignKey("InsuranceInfoId")]
        public InsuranceInfo InsuranceInfo { get; set; }
        [MaxLength(20)]
        public string PatientIPNumber { get; set; }

    }
    public class MedicalTeam
    {
        [Key]
        public long Id { get; set; }
        public long AdmissionId { get; set; }
        [ForeignKey("AdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public long? PrimaryDoctorId { get; set; }
        [ForeignKey("PrimaryDoctorId")]
        public Employee.Employee PrimaryDoctor { get; set; }
        public long? SecondaryDoctorId { get; set; }
        [ForeignKey("SecondaryDoctorId")]
        public Employee.Employee SecondaryDoctor { get; set; }
        public long? PrimaryCareGiverId { get; set; }
        [ForeignKey("PrimaryCareGiverId")]
        public Employee.Employee PrimaryCareGiver { get; set; }
        public long? SecondaryCareGiverId { get; set; }
        [ForeignKey("SecondaryCareGiverId")]
        public Employee.Employee SecondaryCareGiver { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public long? AuthorizedByDoctorId { get; set; }
        [ForeignKey("AuthorizedByDoctorId")]
        public Employee.Employee AuthorizedByDoctor { get; set; }
    }
    public class InPatientLocation
    {
        [Key]
        public long Id { get; set; }
        public long AdmissionId { get; set; }
        [ForeignKey("AdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public DateTime DateMovedIn { get; set; }
        public DateTime DateMovedOut { get; set; }
        public long? WardId { get; set; }
        [ForeignKey("WardId")]
        public Ward Ward { get; set; }
        public long? BedId { get; set; }
        [ForeignKey("BedId")]
        public Bed Bed { get; set; }
        public long? AuthorizedByDoctorId { get; set; }
        [ForeignKey("AuthorizedByDoctorId")]
        public Employee.Employee AuthorizedByDoctor { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
    }
    public class PatientIPId
    {
        [Key, Column(Order = 0)]
        public long CompanyId { get; set; }
        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }
        public int NextNumber { get; set; }
        [NotMapped]
        public string PatientIPNo
        {
            get
            {
                return String.Format("I" + "{0:00}", CompanyId) + String.Format("{0:00}", Date.Day) + String.Format("{0:00}", Date.Month) + String.Format("{0:00}", Date.Year % 1000) + String.Format("{0:0000}", NextNumber);
            }
        }
    }
    public enum InPatientStatus
    {
        ADMITTED, RELEASED_FOR_LAB_WORK, DISCHARGED
    }
}
