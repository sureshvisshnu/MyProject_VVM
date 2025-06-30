using System;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;

namespace fa.model
{
    public interface IAuditableEntity
    {
        DateTime? CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        string LastModifiedBy { get; set; }
    }

    public abstract class AuditableEntity : IAuditableEntity
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public abstract class AuditableEntityForCompany:AuditableEntity
    {
        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
    }

    public abstract class AuditableEntityForCostCenter : AuditableEntityForCompany
    {
        public long? CostCenterId { get; set; }
        [ForeignKey("CostCenterId")]
        public virtual CostCenter CostCenter { get; set; }
    }
    public abstract class AuditableEntityForWorkStation : AuditableEntityForCostCenter
    {
        public string WorkStationId { get; set; }
        public string WorkStationName { get; set; }
    }

}
