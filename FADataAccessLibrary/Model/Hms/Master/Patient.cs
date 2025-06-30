using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Common;
using fa.model.Hms.common;
using fa.api.Hms;
using FaData.Utils;

namespace fa.model.Hms.Master
{
    [Table("Patients")]
    public class Patient : Person
    {
        public string PatientNumber { get; set; }
        public long? ResponsiblePartyId { get; set; }
        [ForeignKey("ResponsiblePartyId")]
        public Person ResponsibleParty { get; set; }
        public virtual ICollection<Guardian> Guardians { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContact { get; set; }
        public virtual ICollection<InsuranceInfo> InsuranceInfo { get; set; }
        public virtual ICollection<PatientPreMedicalHistory> History { get; set; }
        public virtual ICollection<Vital> Vitals { get; set; }
        public bool IsDeceased { get; set; }
        public string OtherNotes { get; set; }

        public string GetStatus(DateTime Date)
        {
            if (IsDeceased)
            {
                return "Deceased";
            }
            else if (IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId(Id) != null)
            {
                return "In IP";
            }
            else if (OpManager.Instance.GetPatientInOpQueue(Id, Date) != null)
            {
                return "In OP";
            }
            return string.Empty;
        }
    }

    public class PatientPreMedicalHistory:AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long HistoryItemId { get; set; }
        [ForeignKey("HistoryItemId")]
        public PatientHistoryQuestion HistoryItem { get; set; }
        private string _HistoryItemValue { get; set; }
        public string HistoryItemValue 
        {
            set
            {
                //Validate the value bases on history item value
            }
            get
            {
                return _HistoryItemValue;
            }
        }
        public string AdditionalValue { get; set;}
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        
    }

    public enum Gender
    {
        MALE, FEMALE, OTHERS
    }

    [Table("Guardians")]
    public class Guardian : Person
    {
        public RelationShip RelationShip { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }

    [Table("EmergencyContacts")]
    public class EmergencyContact : Person
    {
        public RelationShip RelationShip { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
    public enum RelationShip
    {
        MOTHER, FATHER, GRANDPARENT, GAURDIAN, FRIEND, SELF, HUSBAND, SPOUSE
    }

    [Table("Persons")]
    public class Person : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(1)]
        public string MiddleInitial { get; set; }
        public Gender Gender { get; set; }
        public string TaxId { get; set; }
        public string Occupation { get; set; }
        public string Income { get; set; }
        public string Employer { get; set; }
        public string Bloodgroup { get; set; }


        public string Name
        {
            get
            {
                return this.FirstName + (String.IsNullOrEmpty(MiddleInitial)?"":" " + MiddleInitial) + (String.IsNullOrEmpty(LastName) ? "" : " " + LastName);
            }
        }
        public DateTime DateOfBirth { get; set; }
        //Age will be computed based on todays Date and return.
        public int? Age
        {
            get
            {
                return DateUtils.ComputeAge(DateTime.Now, DateOfBirth);
            }
        }
        public long? AddressId { get; set; }
        public virtual Address Address { get; set; }
        public long? ContactInfoId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public byte[] Photo { get; set; }
        public override string ToString()
        {
            return Name;
        }
        
    }
    

    public class InsuranceInfo : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }        
        public long InsuranceHolderId { get; set; }
        [ForeignKey("InsuranceHolderId")]
        public virtual Person InsuranceHolder { get; set; }
        public RelationShip InsuranceHolderRelationShip { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuranceName { get; set; }
        public string GroupNumber { get; set; }
        public string EmployerName { get; set; }
        public long? EmployerAddressId { get; set; }
        [ForeignKey("EmployerAddressId")]
        public virtual Address EmployerAddress { get; set; }
        public long? EmployerContactInfoId { get; set; }
        public virtual ContactInfo EmployerContactInfo { get; set; }
        public string EmployerPhone { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public override string ToString()
        {
            return InsuranceName;
        }
        public bool IPInsuranceCoverage { get; set; }
        public bool OPInsuranceCoverage { get; set; }
    }


    public class PatientId
    {
        [Key, Column(Order = 0)]
        public long CompanyId { get; set; }
        [Key, Column(Order = 1)]
        public DateTime Date { get; set; }
        public int NextNumber { get; set; }
        public string PatientNo
        {
            get
            {
                return String.Format("{0:00}", CompanyId)+ String.Format("{0:00}", Date.Day) + String.Format("{0:00}", Date.Month) +String.Format("{0:00}", Date.Year%1000) + String.Format("{0:0000}", NextNumber);
            }
        }
    }

    public class Token
    {
        
        [Key ,Column(Order = 0)]
        public long CompanyId { get; set; }
        [Key ,Column(Order = 1)]
        public DateTime Date { get; set; }
        public int NextNumber { get; set; }
        public string TokenNo
        {
            get
            {
                return String.Format("{0:000}", NextNumber);
            }
        }
    }
    public class DocumentCategory : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual DocumentCategory Parent { get; set; }
        private string _displayAs;

        [MaxLength(50)]
        public string DisplayAs
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return this.Name;
                }
                else
                    return _displayAs;
            }
            set
            {
                if (!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        public string Discription { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class PatientDocument : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public long PatientDocumentCategoryId { get; set; }
        [ForeignKey("PatientDocumentCategoryId")]
        public virtual DocumentCategory PatientDocumentCategory { get; set; }
        public byte[] File { get; set; }        
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return FileName;
        }
    }    
    public enum FileType
    {
        DOC, PDF, JPG, PNG, JPEG, DOCX, GIF, TIFF, PSD, EPS, AI, INDD, RAW, JFIF, BMP, PCX, TGA, CR2, NEF, ORF, SR2, DWG, DXF, TPL, CVX, CNV, CVI

    }
    public enum PatientSearchType
    {
        Patient,
        Inpatient,
        SearchPatient,
        PatientProcedure
    }
}
