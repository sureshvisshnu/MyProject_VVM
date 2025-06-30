using fa.model;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Model.Hms.common
{
    public class PatientRoomRent: AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DateOfRental { get; set; }
        public string Ward { get; set; }
        public string Bed { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public long InPatientAdmissionId { get; set; }
        [ForeignKey("InPatientAdmissionId")]
        public InPatientAdmission InPatientAdmission { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
