using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Hms.Master
{
    public class Ward:AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public virtual List<Bed> Beds { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public bool MaintainInventory { get; set; }
        public long? InventoryLocationId { get; set; }
        [ForeignKey("InventoryLocationId")]
        public InventoryLocation InventoryLocation { get; set; }       
    }

    [Index(nameof(WardId), nameof(Name), Name = "IX_Ward_Bed_Unique", IsUnique = false)]
    public class Bed: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? WardId { get; set; }
        [ForeignKey("WardId")]
        public Ward Ward { get; set; }
        [MaxLength(10)]
        public string Name { get; set; }
        public long? BedTypeId { get; set; }
        [ForeignKey("BedTypeId")]
        public BedType BedType { get; set; }
        public BedStatus BedStatus { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }

    public class BedType : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(10)]
        public string Name { get; set; }
        public virtual List<Rent> Rents { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Rent: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public long? BedTypeId { get; set; }
        [ForeignKey("BedTypeId")]
        public BedType BedType { get; set; }
        public float Amount { get; set; }
        public RentPeriod RentPeriod { get; set; }

    }
    public enum BedStatus
    {
        AVAILABLE, OCCUPIED, IN_MAINTENANCE
    }
    public enum RentPeriod
    {
       MONTHLY, DAILY, WEEKLY, HOURLY, TWELVEHOURS, FOURHOURS
    }
}
