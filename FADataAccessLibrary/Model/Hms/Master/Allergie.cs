using fa.model;
using fa.model.Common;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Model.Hms.Master
{
    public class Allergie : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? AllergieCategoryId { get; set; }
        [ForeignKey("AllergieCategoryId")]
        public virtual AllergieCategory AllergieCategory { get; set; }
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
        public AllergieClass Class { get; set; }
        public Boolean IsActive { get; set; }
        public string ReasonForInactive { get; set; }
        public ICollection<AllergieKeyword> Keywords { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class AllergieKeyword : Keyword
    {
        public long? AllergieId { get; set; }
        [ForeignKey("AllergieId")]
        public Allergie Allergie { get; set; }
    }


    public class PatientReportedAllergie : Allergie
    {
        public PatientReportedAllergie()
        {
            Class = AllergieClass.PATIENT_REPORTED;
        }
    }

    public class TestObsorbedAllergie : Allergie
    {
        public TestObsorbedAllergie()
        {
            Class = AllergieClass.TEST_OBSORBED;
        }
    }

    public class AllergieCategory : AuditableEntityForCompany
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
        public bool IsSubAllergieCategory { get; set; }
        public long? ParentAllergieCategoryId { get; set; }
        [ForeignKey("ParentAllergieCategoryId")]
        public virtual AllergieCategory ParentAllergieCategory { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public enum AllergieClass
    {
        PATIENT_REPORTED, TEST_OBSORBED
    }
}
