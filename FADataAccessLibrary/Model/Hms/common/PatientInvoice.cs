using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Hms.common
{
    public class PatientInvoice:AuditableEntityForCompany
    {
        [Key]
        public long InvoiceId { get; set; }        
        public DateTime InvoiceDate { get; set; }
        public string ReferenceNumber { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public string Description { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public ICollection<PatientInvoicePayment> PatientInvoicePayments { get; set; } = new List<PatientInvoicePayment>();
        public ICollection<PatientLedger> PatientLedgers { get; set; } = new List<PatientLedger>();

    }
    public class PatientInvoicePayment
    {
        [Key]
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual PatientInvoice PatientInvoice { get; set; }
        public long? LedgerId { get; set; }
        [ForeignKey("LedgerId")]
        public virtual PatientLedger PatientLedger { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
