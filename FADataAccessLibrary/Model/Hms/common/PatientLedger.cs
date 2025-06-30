using fa.api.Accounting;
using fa.api.UserProfile;
using fa.model;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Hms.common
{
    public class PatientLedger : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public double Amount { get; set; }
        public string RefNumber { get; set; }
        public TransactionType Type { get; set; }
        public string RentTimeDuration { get; set; }
        public string Bed { get; set; }
        public long? ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]
        public User Consultant { get; set; }
        public long? OpRegistrationId { get; set; }
        [ForeignKey("OpRegistrationId")]
        public Registration OpRegistration { get; set; }
        public long? InPatientAdmissionId { get; set; }
        [ForeignKey("InPatientAdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public long? ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public long? ConsultedConsultationId { get; set; }
        [ForeignKey("ConsultedConsultationId")]
        public virtual ConsultedConsultationFee ConsultedConsultationFee { get; set; }
        public long? ConsultedProcedureId { get; set; }
        [ForeignKey("ConsultedProcedureId")]
        public virtual ConsultedProcedure ConsultedProcedure { get; set; }
        public string Description { get; set; }
        public bool Invoiced { get; set; }
        public long? PatientInvoiceId { get; set; }
        [ForeignKey("PatientInvoiceId")]
        public virtual PatientInvoice PatientInvoice { get; set; }
        public double InvoiceAmount { get; set; }
        public bool IsReceivedPayment { get; set; }
        public long? ConsultedLabTestId { get; set; }
        [ForeignKey("ConsultedLabTestId")]
        public virtual ConsultedLabTest ConsultedLabTest { get; set; }
        public double GetPendingAmount()
        {
            return (Amount - InvoiceAmount);
        }
        public string ConsultantName()
        {
            if (ConsultantId != null)
            {
                Consultant = UserManager.Instance.GetUserById((long)ConsultantId);
                if (Consultant.EmployeeId != null)
                {
                    Consultant.Employee = EmployeeManager.Instance.GetEmployeeInfoById((long)Consultant.EmployeeId);
                    if (Consultant.Employee != null)
                        return string.Format("{0}", Consultant.Employee.Name);
                }
                return string.Format("{0} {1}", Consultant.FirstName, Consultant.LastName);
            }
            else
            {
                return string.Empty;
            }
        }
        public ICollection<PatientPaymentDetail> PatientPaymentDetails { get; set; } = new List<PatientPaymentDetail>();

    }

    public enum TransactionType
    {
        REGISTRATION_FEE = 0,  //Registration
        CONSULTATION_FEE = 1,  // Consultation
        PAYMENT = 2,
        WAIVER = 3,               
        MEDICALPROCEDURE_FEE = 4, //Procedure
        ROOM_RENT = 5,             //Room
        LAB_FEE = 6,              //Lab
        PHARMACY_FEE = 7,         //Parmacy
        MISCELLENEOUS = 8,         //Others
        ROOM_CLEANING_CHARGE = 9  //Room

}
    public class PatientLedgerTransactionTypeGroup : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
        public ICollection<PatientLedgerTransactionTypeGroupMapping> PatientLedgerTransactionTypeGroupMappings { get; set; } = new List<PatientLedgerTransactionTypeGroupMapping>();
        public override string ToString()
        {
            return Name;
        }
    }
    public class PatientLedgerTransactionTypeGroupMapping : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? PatientLedgerTransactionTypeGroupId { get; set; }
        [ForeignKey("PatientLedgerTransactionTypeGroupId")]
        public PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroup { get; set; }
        public TransactionType TransactionType { get; set; }
    }
    
    public class PatientPaymentDetail : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? PatientLedgerId { get; set; }
        [ForeignKey("PatientLedgerId")]
        public PatientLedger PatientLedger { get; set; }
        public DateTime Date { get; set; }
        public PaymentType PaymentType { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
