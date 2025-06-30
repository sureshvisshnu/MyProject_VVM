using fa.model;
using fa.model.hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Hms.common
{
    public class DischargeNote : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public long InPatientAdmissionId { get; set; }
        [ForeignKey("InPatientAdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public string DiagnosisSummary { get; set; }
        public string TreatmentSummary { get; set; }
        public string DischargeSummary { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DischargeOn { get; set; }
        public DateTime? NextFollowUp { get; set; }
        public long EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public fa.model.Employee.Employee Employee { get; set; }
        public bool Waive { get; set; }
        public bool IsDeceased { get; set; }
        public double Amount { get; set; }
        [NotMapped]
        public long UserId { get; set; }
        public virtual ICollection<DischargePrescription> DischargePrescription { get; set; } = new List<DischargePrescription>();
    }
    public class DischargePrescription : AuditableEntityForCompany
    {
        [Key]
        public long DischargePrescriptionId { get; set; }
        public long DischargeNoteId { get; set; }
        [ForeignKey("DischargeNoteId")]
        public virtual DischargeNote DischargeNote { get; set; }
        public long PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }
        public string CustomPrescription { get; set; }
        public bool? IsCustomPrescription { get; set; }
    }
}
