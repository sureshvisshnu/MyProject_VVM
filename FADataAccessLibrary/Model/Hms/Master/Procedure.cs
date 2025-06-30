using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Common;

namespace fa.model.Hms.Master
{
    public class MedicalProcedure : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public string ProcedureCode { get; set; }
        public long? MedicalProcedureCategoryId { get; set; }
        [ForeignKey("MedicalProcedureCategoryId")]
        public virtual MedicalProcedureCategory MedicalProcedureCategory { get; set; }
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
                if (value!=null && !value.Equals(this.Name))
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
        public bool IsActive { get; set; }
        public string Reason { get; set; }
        public bool HasElement { get; set; }
        public double Fee { get; set; }
        public ICollection<MedicalProcedureElement> ProcedureElements { get; set; }
        public ICollection<MedicalProcedureKeyword> Keywords { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class MedicalProcedureKeyword : Keyword
    {
        public long? MedicalProcedureId { get; set; } 
        [ForeignKey("MedicalProcedureId ")]
        public MedicalProcedure MedicalProcedure { get; set; }
    }
    public class MedicalProcedureElement : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? MedicalProcedureId { get; set; }
        [ForeignKey("MedicalProcedureId ")]
        public MedicalProcedure Procedure { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public double Fee { get; set; }
    }
    public class MedicalProcedureCategory : AuditableEntityForCompany
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
        public bool IsSubMedicalProcedureCategory { get; set; }
        public long? ParentMedicalProcedureCategoryId { get; set; }
        [ForeignKey("ParentMedicalProcedureCategoryId")]
        public virtual MedicalProcedureCategory ParentMedicalProcedureCategory { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
