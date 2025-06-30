using fa.model;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.model.Hms.common
{
    public class ConsultationNote : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public ICollection<NotesDetail> NotesDetails { get; set; }

    }
    public class NotesDetail : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long ConsultationNoteId { get; set; }
        [ForeignKey("ConsultationNoteId")]
        public virtual ConsultationNote ConsultationNote { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
