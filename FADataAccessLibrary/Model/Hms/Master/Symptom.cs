using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Common;

namespace fa.model.Hms.Master
{
    public class Symptom : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public string SymptomCode { get; set; }
        public long? SymptomCategoryId { get; set; }
        [ForeignKey("SymptomCategoryId")]
        public virtual SymptomCategory SymptomCategory { get; set; }
        [Required, MaxLength(20)]
        public string Codes { get; set; }
        [Required, MaxLength(50)]        
        public string Name { get; set; }
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
        public SymptomClass Class { get; set; }
        public Boolean IsActive { get; set; }
        public string ReasonForInactive { get; set; }            
        public ICollection<SymptomKeyword> Keywords { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class SymptomKeyword: Keyword
    {
        public long? SymptomId { get; set; }
        [ForeignKey("SymptomId")]
        public Symptom Symptom { get; set; }
    }


    public class PatientReportedSymptom : Symptom
    {
        public PatientReportedSymptom()
        {
            Class = SymptomClass.PATIENT_REPORTED;
        }
    }

    public class TestObsorbedSymptom : Symptom
    {
        public TestObsorbedSymptom()
        {
            Class = SymptomClass.TEST_OBSORBED;
        }
    }

    public class SymptomCategory : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
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
        [MaxLength(250)]
        public string Discription { get; set; }
        public bool IsSubSymptomCategory { get; set; }
        public long? ParentSymptomCategoryId { get; set; }
        [ForeignKey("ParentSymptomCategoryId")]
        public virtual SymptomCategory ParentSymptomCategory { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public enum SymptomClass
    {
        PATIENT_REPORTED, TEST_OBSORBED
    }
}
