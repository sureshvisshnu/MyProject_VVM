using fa.model;
using fa.model.Employee;
using fa.model.UserProfile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Hms.Master
{
    public class Consultation: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
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
        public string Discription { get; set; }
        public double Fee { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public virtual List<ConsultationDetail> ConsultationDetail { get; set; }
    }

    public class ConsultedDoctorConsultationFee : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]
        public User Consultant { get; set; }
        public long? ConsultationId { get; set; }
        [ForeignKey("ConsultationId")]
        public Consultation Consultation { get; set; }
        public double Fee { get; set; }

    }
    public class ConsultationDetail : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? ConsultationId { get; set; }
        [ForeignKey("ConsultationId")]
        public virtual Consultation Consultation { get; set; }
        public long? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual fa.model.Employee.Employee Employee { get; set; }
        public double? Fee { get; set; }
    }
}
