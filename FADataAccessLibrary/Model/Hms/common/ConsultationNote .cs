using fa.api.Accounting;
using fa.api.UserProfile;
using fa.model;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.model.UserProfile;
using FADataAccessLibrary.Model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.hms.common
{
   public class ConsultationNote : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public virtual ICollection<ConsultedSymptom> ConsultedSymptom { get; set; }
        public virtual ICollection<ConsultedAllergie> ConsultedAllergie { get; set; }
        public virtual ICollection<ConsultedPrescription> ConsultedPrescription { get; set; }
        public virtual ICollection<ConsultedConsultationFee> ConsultedConsultationFee { get; set; }
        public virtual ICollection<ConsultedLabTest> ConsultedLabTest { get; set; }
        public virtual ICollection<ConsultedProcedure> ConsultedProcedure { get; set; }
        public long ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]
        public User Consultant { get; set; }
        public bool IsPrescriptionDispatchedForMedical { get; set; }
        public bool IsPrescriptionDone { get; set; }
        public long? SaleEntryId { get; set; }
        [ForeignKey("SaleEntryId")]
        public SaleEntry SaleRefPrescription { get; set; }
        public long? OpRegistrationId { get; set; }
        [ForeignKey("OpRegistrationId")]
        public Registration OpRegistration { get; set; }
        public long? InPatientAdmissionId { get; set; }
        [ForeignKey("InPatientAdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public double Fees { get; set; }
        public string ConsultantName()
        {
            Consultant = UserManager.Instance.GetUserById(ConsultantId);
            if (Consultant.EmployeeId!=null)
            {
                Consultant.Employee = EmployeeManager.Instance.GetEmployeeInfoById((long)Consultant.EmployeeId);
                if (Consultant.Employee!=null)
                return string.Format("{0}", Consultant.Employee.Name);
            }
            return string.Format("{0} {1}", Consultant.FirstName, Consultant.LastName);
        }
        public long GetConsultantId()
        {
            if (Consultant.EmployeeId != null)
            {                
                    return (long)Consultant.EmployeeId;
            }
            return ConsultantId;
        }
        public bool IsDischarged { get; set; }
        public bool IsInvoiced { get; set; }

    }
    public class ConsultedLabTest : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedLabTestId { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId")]
        public MedicalTest MedicalTest { get; set; }
        public bool HasElement { get; set; }
        public long? RequestedById { get; set; }
        [ForeignKey("RequestedById")]
        public virtual User RequestedBy { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Fees { get; set; }
        public DateTime? PerformOn { get; set; }
        public long? PerformedById { get; set; }
        [ForeignKey("PerformedById")]
        public virtual User PerformedBy { get; set; }
        public DateTime RequestedOn { get; set; }

        public ICollection<ConsultedLabTestElements> ConsultedLabTestElements { get; set; }
        public ICollection<LabTestAttachment> LabTestAttachments { get; set; }

    }
    public class ConsultedLabTestElements : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long ConsLabTestId { get; set; }
        [ForeignKey("ConsLabTestId")]
        public virtual ConsultedLabTest ConsultedLabTest { get; set; }
        public long MedicalTestElementId { get; set; }
        [ForeignKey("MedicalTestElementId")]
        public virtual MedicalTestElement MedicalTestElement { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string SubClass { get; set; }
        public string SingleValue { get; set; }
        public string RangeFrom { get; set; }
        public string RangeTo { get; set; }
        public string ResultDescription { get; set; }
        public long? UomId { get; set; }
        [ForeignKey("UomId")]
        public MedicalTestUOM Uom { get; set; }
    }
    public class LabTestAttachment : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long ConsLabTestId { get; set; }
        [ForeignKey("ConsLabTestId")]
        public virtual ConsultedLabTest ConsultedLabTest { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public string Description { get; set; }
    }

    public class ConsultedConsultationFee : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedConsultationId { get; set; }
        public long? ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long ConsultationId { get; set; }
        [ForeignKey("ConsultationId")]
        public Consultation Consultation { get; set; }
        public long? ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]
        public User Consultant { get; set; }
        public bool IsOverrideFee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Fee { get; set; }
    }
    public class ConsultedSymptom : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedSymptomId { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long SymptomId { get; set; }
        [ForeignKey("SymptomId")]
        public Symptom Symptom { get; set; }
        public string Description { get; set; }
    }
    public class ConsultedAllergie : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedAllergieId { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long AllergieId { get; set; }
        [ForeignKey("AllergieId")]
        public Allergie Allergie { get; set; }
        public string Description { get; set; }
    }

    public class ConsultedPrescription : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedPrescriptionId { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long? PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }
        public string CustomPrescription { get; set; }
        public bool? IsCustomPrescription { get; set; }

    }
    public class ConsultedProcedure : AuditableEntityForCompany
    {
        [Key]
        public long ConsultedProcedureId { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long MedicalProcedureId { get; set; }
        [ForeignKey("MedicalProcedureId")]
        public virtual MedicalProcedure MedicalProcedure { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public ProcedureStatus ProStatus { get; set; }
        public string Description { get; set; }        
        public string Note { get; set; }
        public double Fees { get; set; }
        public DateTime? PerformOn { get; set; }
        public long? PerformedById { get; set; }
        [ForeignKey("PerformedById")]
        public virtual User PerformedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public long? RequestedById { get; set; }
        [ForeignKey("RequestedById")]
        public virtual User RequestedBy { get; set; }
    }
    public enum ProcedureStatus
    {
        REQUESTED, INPROGRESS, COMPLETED, CANCEL
    }
    public class ConsultedProcedureHistory : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long ConsultedProcedureId { get; set; }
        [ForeignKey("ConsultedProcedureId")]
        public virtual ConsultedProcedure ConsultedProcedure { get; set; }      
        public ProcedureStatus ProStatus { get; set; }
        public string Note { get; set; }
        public DateTime? PerformOn { get; set; }
        public long? PerformedById { get; set; }
        [ForeignKey("PerformedById")]
        public virtual User PerformedBy { get; set; }
        
    }
}
