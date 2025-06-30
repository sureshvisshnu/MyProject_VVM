using fa.model.Catalog;
using fa.model.UserProfile;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace fa.model.hms.common
{
    public class Prescription: AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public string Total { get; set; }
        public string Days { get; set; }
        public TakeDosage TakeDosage { get; set; }
        public string Hours { get; set; }
        public string Morning { get; set; }
        public string Afternoon { get; set; }
        public string Evening { get; set; }
        public string Night { get; set; }
        public string PrescriptionNumber { get; set; }
        [MaxLength(3072)]
        public string AdditionalNotes { get; set; }
        public long? PrescribedByDoctorId { get; set; }
        [ForeignKey("PrescribedByDoctorId")]
        public Employee.Employee PrescribedByDoctor { get; set; }
        public override string ToString()
        {
            return String.IsNullOrEmpty(CreatedBy)?"": CreatedBy.Substring(0, CreatedBy.IndexOf("["));
        }
        public string CustomProduct { get; set; }
        public bool? IsCustomProduct { get; set; }
    }
    public enum TakeDosage
    {
        None = 0, BEFORE = 1, AFTER = 2        
    }
}