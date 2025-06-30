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

namespace fa.model.OrderManagement
{
    public class StockMovement : AuditableEntityForWorkStation
    {
        [Key]
        public long Id { get; set; }
        public string RefNumber { get; set; }
        public DateTime MovementDate { get; set; }
        public InventoryJournalType Type { get; set; }
        public ICollection<StockMovementDetail> StockMovementDetails { get; set; } = new List<StockMovementDetail>();
        public long InventoryStockLocationId { get; set; }
        [ForeignKey("InventoryStockLocationId")]
        public InventoryLocation InventoryStockLocation { get; set; }
    }
    public class StockMovementDetail : AuditableEntityForWorkStation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? StockMovementId { get; set; }
        [ForeignKey("StockMovementId")]
        public StockMovement StockMovement { get; set; }
        public long? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public bool isFree { get; set; }
        public double FreeQuantity { get; set; }
        public string MaterialId { get; set; }
        public bool isBatch { get; set; }
        [MaxLength(10)]
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public string Uom { get; set; }
        [NotMapped]
        public float PurchasePrice { get; set; }
        [NotMapped]
        public float PurchaseCost { get; set; }
        [NotMapped]
        public float Retailprice { get; set; }
        [NotMapped]
        public float Wholesaleprice { get; set; }
        [NotMapped]
        public float Msrp { get; set; }
        [NotMapped]
        public string RetailUOM { get; set; }
        [NotMapped]
        public int RetailXFactor { get; set; }
        [NotMapped]
        public string WholesaleUOM { get; set; }
        [NotMapped]
        public int WholesaleXFactor { get; set; }
        public string Description { get; set; }
        private DateTime? _StockDate;
        public DateTime? StockDate
        {
            get
            {
                if (_StockDate != null)
                {
                    return this._StockDate;
                }
                else
                    return CreatedDate;
            }
            set
            {
                _StockDate=value;
            }
        }

    }
    public class StockMovementIn : StockMovement
    {
        public long InventoryLocationFromId { get; set; }
        [ForeignKey("InventoryLocationFromId")]
        public InventoryLocation InventoryLocationFrom { get; set; }
        public long StockMovementOutId { get; set; }
        [ForeignKey("StockMovementOutId")]
        public StockMovementOut StockMovementOut { get; set; }
   
        public StockMovementIn()
        {
            this.Type = InventoryJournalType.STOCK_IN;
        }
    }

    public class StockMovementOut : StockMovement
    {
        public long InventoryLocationToId { get; set; }
        [ForeignKey("InventoryLocationToId")]
        public InventoryLocation InventoryLocationTo { get; set; }
        [NotMapped]
        public long? RequestId { get; set; }
        public StockMovementOut()
        {
            this.Type = InventoryJournalType.STOCK_OUT;
        }
        public override string ToString()
        {
            return RefNumber;
        }
    }
    public class StockMovementRequest : StockMovement
    {
        public long RequestInventoryLocationId { get; set; }
        [ForeignKey("RequestInventoryLocationId")]
        public InventoryLocation RequestInventoryLocation { get; set; }
        public bool HasRequestCompleted { get; set; }
        public bool IsResponsed { get; set; }
        public long? StockOutId { get; set; }
        [ForeignKey("StockOutId")]
        public StockMovement StockMovement { get; set; }
        public StockMovementRequest()
        {
            this.Type = InventoryJournalType.STOCK_REQUEST;
        }
        public override string ToString()
        {
            return RefNumber;
        }
    }
    public class StockMovementPatientUse : StockMovement
    {
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public StockMovementPatientUse()
        {
            Type = InventoryJournalType.PATIENT_USE;
        }
    }

    public class StockMovementOpeningStock : StockMovement
    {
        public StockMovementOpeningStock()
        {
            Type = InventoryJournalType.OPEN_STOCK;
        }
    }

    public class StockMovementDamaged : StockMovement
    {
        public StockMovementDamaged()
        {
            Type = InventoryJournalType.DAMAGED;
        }
    }

    public class StockMovementPurchase : StockMovement
    {
        public StockMovementPurchase()
        {
            Type = InventoryJournalType.PURCHASE;
        }
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId")]
        public PurchaseEntry PurchaseEntry { get; set; }
    }

    public class StockMovementSales : StockMovement
    {
        public StockMovementSales()
        {
            Type = InventoryJournalType.SALES;
        }
        public long? SaleId { get; set; }
        [ForeignKey("SaleId")]
        public SaleEntry Sale { get; set; }
    }

    public class StockMovementPurchaseReturn : StockMovement
    {
        public StockMovementPurchaseReturn()
        {
            Type = InventoryJournalType.PURCASE_RETURN;
        }
        public long? PurchaseReturnEntryId { get; set; }
        [ForeignKey("PurchaseReturnEntryId")]
        public PurchaseEntry PurchaseReturnEntry { get; set; }
    }

    public class StockMovementSalesReturn : StockMovement
    {
        public StockMovementSalesReturn()
        {
            Type = InventoryJournalType.SALES_RETURN;
        }
        public long? SaleReturnId { get; set; }
        [ForeignKey("SaleReturnId")]
        public SaleEntry SaleReturn { get; set; }
    }
    public class StockMovementAdjustment : StockMovement
    {
        public StockMovementAdjustment()
        {
            Type = InventoryJournalType.ADJUSTMENT;
        }
    }
}
