using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Common;

namespace fa.model.Hms.Master
{
    public class MedicalTest : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? MedicalTestCategoryId { get; set; }
        [ForeignKey("MedicalTestCategoryId")]
        public virtual MedicalTestCategory MedicalTestCategory { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        private string _displayAs;
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
        public string Description { get; set; }
        public string TestCode { get; set; }
        public string TestShortName { get; set; }
        public bool IsActive { get; set; }
        public string Reason { get; set; }
        public string SampleRequirement { get; set; }
        public bool HasElement { get; set; }
        public ICollection<MedicalTestElement> TestElements { get; set; }
        public ICollection<MedicalTestKeyword> Keywords { get; set; }
    }

    public class MedicalTestKeyword : Keyword
    {
        public long? MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId ")]
        public MedicalTest MedicalTest { get; set; }
    }

    public class MedicalTestElement : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId")]
        public virtual MedicalTest MedicalTest { get; set; }
        public string Name { get; set; }
        public string ElementCode { get; set; }
        public string ElementShortName { get; set; }
        public long? UomId { get; set; }
        [ForeignKey("UomId")]
        public virtual MedicalTestUOM Uom { get; set; }
        public string RangeFrom { get; set; }
        public string RangeTo { get; set; }
        public string Description { get; set; }
        public string SingleValue { get; set; }
        public string Class { get; set; }
        public string SubClass { get; set; }
        public string ResultDuration { get; set; }
        public double Fee { get; set; }
    }
    public class MedicalTestResult : AuditableEntityForCompany
    {
        public long Id { get; set; }
        public long? MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId")]
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public long MedicalTestElementId { get; set; }
        [ForeignKey("MedicalTestElementId")]
        public string ResultValue { get; set; }
        public DateTime ResultDate { get; set; }
        public double FeeCollected { get; set; }
    }
    public class MedicalTestUOM : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        public string Discription { get; set; }
    }
    public class MedicalTestCategory : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        private string _displayAs;
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
        public string Description { get; set; }
        public bool IsSubMedicalTestCategory { get; set; }
        public long? ParentMedicalTestCategoryId { get; set; }
        [ForeignKey("ParentMedicalTestCategoryId")]
        public virtual MedicalTestCategory ParentMedicalTestCategory { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
