using fa.model;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Hms.Inventory
{
    //public class HospitalInventory
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public long? HospitalInventoryLocationId { get; set; }
    //    [ForeignKey("HospitalInventoryLocationId")]
    //    public HospitalInventoryLocation InventoryLocation { get; set; }
    //    public long ProductId { get; set; }
    //    [ForeignKey("ProductId")]
    //    public Product Product { get; set; }
    //    public float InStock { get; set; }
    //}

    //public class HospitalInventoryLocation
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public InventoryLocationType Type { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //}

    //public enum HospitalInventoryLocationType
    //{
    //    WARD = 0, STORE = 1, GODOWN = 2, NURSHING_STATION=3
    //}

    //public enum InventoryJournalType
    //{
    //    OPEN_STOCK = 0, STOCK_IN = 1, STOCK_OUT = 2, PATIENT_USE = 3, DAMAGED = 4
    //}

    //public class HospitalInventoryJournal : AuditableEntityForWorkStation
    //{
    //    [Key]
    //    public long Id { get; set; }
    //    public float Qty { get; set; }
    //    public InventoryJournalType Type { get; set; }
    //}

    //public class HospitalInventoryJournalStockIn : HospitalInventoryJournal
    //{
    //    public long InventoryLocationFromId { get; set; }
    //    [ForeignKey("InventoryLocationFromId")]
    //    public HospitalInventoryLocation InventoryLocationFrom { get; set; }
    //    public HospitalInventoryJournalStockIn()
    //    {
    //        this.Type = InventoryJournalType.STOCK_IN;
    //    }
    //}

    //public class HospitalInventoryJournalStockOut : HospitalInventoryJournal
    //{
    //    public long InventoryLocationToId { get; set; }
    //    [ForeignKey("InventoryLocationToId")]
    //    public HospitalInventoryLocation InventoryLocationTo { get; set; }
    //    public HospitalInventoryJournalStockOut()
    //    {
    //        this.Type = InventoryJournalType.STOCK_OUT;
    //    }
    //}

    //public class HospitalInventoryJournalPatientUse : HospitalInventoryJournal
    //{
    //    public long PatientId { get; set; }
    //    [ForeignKey("PatientId")]
    //    public Patient Patient { get; set; }
    //    public HospitalInventoryJournalPatientUse()
    //    {
    //        Type = InventoryJournalType.PATIENT_USE;
    //    }
    //}

    //public class HospitalInventoryJournalStockOpeningStock : HospitalInventoryJournal
    //{
    //    public HospitalInventoryJournalStockOpeningStock()
    //    {
    //        Type = InventoryJournalType.OPEN_STOCK;
    //    }
    //}

    //public class HospitalInventoryJournalStockDamaged : HospitalInventoryJournal
    //{
    //    public HospitalInventoryJournalStockDamaged()
    //    {
    //        Type = InventoryJournalType.DAMAGED;
    //    }
    //}
}
