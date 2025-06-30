using fa.model;
using fa.model.Accounting.Masters;
using fa.model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace fa.model.hms.config
{
    public class HospitalConfiguration : AuditableEntityForCompany
    {
        [Key]
        public long Id { set; get; }
        public double DefaultOPConsultingFee { get; set; }
        public long? OPRegistrationFeeAccountId { get; set; }
        [ForeignKey("OPRegistrationFeeAccountId")]
        public Account OPRegistrationFeeAccount { get; set; }
        public double DefaultIPConsultingFee { get; set; }
        public long? IPRegistrationFeeAccountId { get; set; }
        [ForeignKey("IPRegistrationFeeAccountId")]
        public Account IPRegistrationFeeAccount { get; set; }
        [NotMapped]
        public Customer PatientPurchaseAccount { get; set; }
        public bool HasInvoicingGroup { get; set; }
        public bool IsDigitalSignatureDisplayOnPresc { get; set; }
        public bool IsSignatureNameDisplayOnPresc { get; set; }
        public bool IsDigitalSignatureDisplayOnLabtest { get; set; }
        public bool IsSignatureNameDisplayOnLabtest { get; set; }
        public bool IsFooterDetailsDisplayOnPresc { get; set; }
        public bool IsFooterDetailsDisplayOnLabtest { get; set; }
        public string FooterDetailsPrescroption { get; set; }
        public string FooterDetailsLabtest { get; set; }
    }
}
