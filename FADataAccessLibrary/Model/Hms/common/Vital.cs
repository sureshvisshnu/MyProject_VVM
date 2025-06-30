using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.Hms.Ip;
using fa.api.Accounting;

namespace fa.model.Hms.common
{
    public class Vital: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
        public float BMI { get; set; }
        public float Temperature { get; set; }
        public int Pulse { get; set; }
        public int RespRate { get; set; }
        public int BPressure { get; set; }
        public int BPressureOver { get; set; }
        public int BOxyLevel { get; set; }
        public long? OpRegistrationId { get; set; }
        [ForeignKey("OpRegistrationId")]
        public Registration OpRegistration { get; set; }
        public long? InPatientAdmissionId { get; set; }
        [ForeignKey("InPatientAdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        [NotMapped]
        public string DisDate
        {
            get
            { return Date.ToString(CompanyManager.Instance.GetCompanyForModel(CompanyId).DateFormat); }
        }
    }

}
