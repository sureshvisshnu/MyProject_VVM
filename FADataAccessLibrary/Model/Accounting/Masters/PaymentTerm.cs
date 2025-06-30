using System.ComponentModel.DataAnnotations;

namespace fa.model.Accounting.Masters
{
    public class PaymentTerm : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(30)]
        public string Name { get; set; }
        //To determin fixed date payment or relative date terms
        public bool FixedDays { get; set; }
        public int NoOfDays { get; set; }
        public int DueOnDate { get; set; }
        public int LeadPeriodToDue { get; set; }
       
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
