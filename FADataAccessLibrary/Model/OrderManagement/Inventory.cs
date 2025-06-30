using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Catalog;
using fa.model.Hms.Master;

namespace fa.model.OrderManagement
{
    public class Inventory: AuditableEntityForCostCenter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }        
        public double Purchased { get; set; }
        public double Sold { get; set; }
        public double OpeningStock { get; set; }
        public double In { get; set; }
        public double Out { get; set; }
        public double Damage { get; set; }
        public double ToPatient { get; set; }
        public double Adjust { get; set; }
        public string StockUOM { get; set; }
        public List<InventoryBatch> InventoryBatchs { get; set; } = new List<InventoryBatch>();
        private DateTime? _StockDate;
        public DateTime? StockDate
        {
            get
            {
                if (_StockDate != null)
                {
                    return _StockDate;
                }
                else
                    return CreatedDate;
            }
            set
            {
                _StockDate = value;
            }
        }
        [NotMapped]
        public double QuantityOnHand
        {
            get
            {
                return ((Purchased+In) - (Sold+Out+Damage+ToPatient)+Adjust);
            }
        }
        public long InventoryLocationId { get; set; }
        [ForeignKey("InventoryLocationId")]
        public InventoryLocation InventoryLocation { get; set; }
    }

    public class InventoryBatch : AuditableEntityForCostCenter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? InventoryId { get; set; }
        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [MaxLength(10)]
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public double Purchased { get; set; }
        public double Sold { get; set; }
        public double OpeningStock { get; set; }
        public double In { get; set; }
        public double Out { get; set; }
        public double Damage { get; set; }
        public double ToPatient { get; set; }
        public double Adjust { get; set; }
        public string RetailUOM { get; set; }
        public int RetailXFactor { get; set; }
        public string WholesaleUOM { get; set; }
        public int WholesaleXFactor { get; set; }
        public float PurchasePrice { get; set; }
        public float Cost { get; set; }
        public float RetailSalePrice { get; set; }
        public float WholeSalePrice { get; set; }
        public float MaxRetailPrice { get; set; }
        public string StockUOM { get; set; }
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
                _StockDate = value;
            }
        }

        [NotMapped]
        public double QuantityOnHand
        {
            get
            {               
                return ((Purchased+In)-(Sold+Out+Damage+ToPatient)+Adjust);
            }
        }
        [NotMapped]
        public long LocationId { get; set; }
    }


    public class InventoryLocation : AuditableEntityForCostCenter
    {
        [Key]
        public long Id { get; set; }
        public InventoryLocationType Type { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public enum InventoryLocationType
    {
        WARD = 0, STORE = 1, GODOWN = 2, NURSHING_STATION = 3, PHARMACY =4
    }

    public enum InventoryJournalType
    {
        OPEN_STOCK = 0, STOCK_IN = 1, STOCK_OUT = 2, PATIENT_USE = 3, DAMAGED = 4, PURCHASE=5, SALES=6, PURCASE_RETURN=7, SALES_RETURN=8, ADJUSTMENT = 9, STOCK_REQUEST = 10
    }


}
