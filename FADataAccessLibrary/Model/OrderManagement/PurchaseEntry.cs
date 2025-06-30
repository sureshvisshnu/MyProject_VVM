using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using fa.model.Catalog;

namespace fa.model.OrderManagement
{
    public class PurchaseEntry:AuditableEntityForCostCenter
    {        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }        
        public string RefNumber { get; set; }        
        public long? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public PurchaseMethod PurchaseMethod { get; set; }
        public DateTime RefDate { get; set; }
        [MaxLength(25)]
        public string PurchaseInvNumber { get; set; }
        public DateTime PurchaseInvDate { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double DisAmount { get; set; }
        public double RoundOff { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public List<OrderLevelPurchaseDiscount> Discounts { get; set; } = new List<OrderLevelPurchaseDiscount>();
        public List<OrderLevelPurchaseTaxDetail> TaxDetails { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; } = new List<PurchaseDetails>();
        public List<PurchaseAttachment> PurchaseAttachments  { get; set; } = new List<PurchaseAttachment>();
        public List<PurchaseAdditionalTransaction> PurchaseAdditionalTransactions { get; set; } = new List<PurchaseAdditionalTransaction>();
        public long? InventoryLocationId { get; set; }
        [ForeignKey("InventoryLocationId")]
        public InventoryLocation InventoryLocation { get; set; }
        public DateTime ReturnDate { get; set; }
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId")]
        public PurchaseEntry PurchaseEntryRefReturn { get; set; }
        public PurchaseEntrytype PurchaseEntrytype { get; set; }
        public bool isPurchaseEntryLocked { get; set; }
        public PurchaseTaxType PurchaseTaxType { get; set; }


    }
    public enum PurchaseEntrytype
    {
        PURCHASE, ORDER, RETURN
    }
    public enum PurchaseMethod
    {
        Cash, Credit
    }
    public enum PurchaseTaxType
    {
        INTER = 0, INTRA = 1
    }
    public class PurchaseAttachment : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId")]
        public PurchaseEntry PurchaseEntry { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
    }
    public class PurchaseDetails : AuditableEntityForCostCenter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? PurchaseEntryId { get; set; }
        [ForeignKey("PurchaseEntryId")]
        public PurchaseEntry PurchaseEntry { get; set; }
        public long? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public double FreeQuantity { get; set; }
        public float Amount { get; set; }
        public bool isFree { get; set; }
        public float PurchasePrice { get; set; }
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
        public List<LineLevelPurchaseDiscount> Discounts { get; set; }= new List<LineLevelPurchaseDiscount>();
        public List<LineLevelPurchaseTaxDetail> TaxDetails { get; set; } = new List<LineLevelPurchaseTaxDetail>();
        public string MaterialId { get; set; }
        public bool isBatch { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public long? PurchaseDetailsId { get; set; }
        [ForeignKey("PurchaseDetailsId")]
        public PurchaseDetails PurchaseDetailsRefReturn { get; set; }
        public float ReturnFee { get; set; }
    }
}
