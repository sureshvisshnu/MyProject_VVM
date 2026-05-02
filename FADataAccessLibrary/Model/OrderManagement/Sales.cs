using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using FADataAccessLibrary.Model.Accounting.Transactions;

namespace fa.model.OrderManagement
{
    public class SaleEntry : AuditableEntityForWorkStation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string RefNumber { get; set; }
        public long? AccountsId { get; set; }
        [ForeignKey("AccountsId")]
        public Account Account { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public SaleMethod SaleMethod { get; set; }
        public SaleType SaleType { get; set; }
        public SaleTaxType SaleTaxType { get; set; }
        public DateTime SaleDate { get; set; }
        public double TotalAmount { get; set; }
        public double NetAmount { get; set; }
        public double TaxAmount { get; set; }
        public double DisAmount { get; set; }
        public double RoundOff { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public List<OrderLevelSaleDiscount> Discounts { get; set; } = new List<OrderLevelSaleDiscount>();
        public List<OrderLevelSaleTaxDetail> TaxDetails { get; set; } = new List<OrderLevelSaleTaxDetail>();
        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        public List<SaleAdditionalTransaction> SaleAdditionalTransactions { get; set; } = new List<SaleAdditionalTransaction>();
        public Entrytype EntryType { get; set; }
        public DateTime QuotaionExpireAt { get; set; }
        public long? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment SalePayment { get; set; }
        public long? PaymentNewId { get; set; }
        [ForeignKey("PaymentNewId")]
        public PaymentNew SalePaymentNew { get; set; }
        public bool isPaymentReceived { get; set; }
        public bool hasDelivered { get; set; }
        public bool isSaleLocked { get; set; }
        public DateTime ReturnDate { get; set; }
        public long? SaleEntryId { get; set; }
        [ForeignKey("SaleEntryId")]
        public SaleEntry SaleRefQuoteReturn { get; set; }

        public long? SoldById { get; set; }
        [ForeignKey("SoldById")]
        public Refered SoldBy { get; set; }
        public long? ReferedById { get; set; }
        [ForeignKey("ReferedById")]
        public Refered ReferedBy { get; set; }

        [NotMapped]
        public long? NoteId { get; set; }
        public long? InventoryLocationId { get; set; }
        [ForeignKey("InventoryLocationId")]
        public InventoryLocation InventoryLocation { get; set; }
        public long? StateId { get; set; }
        public virtual State State { get; set; }
        public long? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public long? RegistrationId { get; set; }
        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }
        public long? PaymentType { get; set; }
    }

    public enum Entrytype
    {
        SALE, QUOTE, RETURN
    }
    public enum SaleMethod
    {
        Cash, Credit
    }
    public enum SaleType
    {
        Retail, WholeSale
    }

    public class SaleDetail : AuditableEntityForWorkStation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? SaleId { get; set; }
        [ForeignKey("SaleId")]
        public SaleEntry Sale { get; set; }
        public long? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public double FreeQuantity { get; set; }
        public float Amount { get; set; }
        public bool isFree { get; set; }
        public float Price { get; set; }
        public float Msrp { get; set; }
        public float OverridePrice { get; set; }
        public float ReturnFee { get; set; }
        public float OverrideenBy { get; set; }
        public List<ItemLevelSaleTaxDetail> TaxDetails { get; set; } = new List<ItemLevelSaleTaxDetail>();
        public List<ItemLevelSaleDiscount> Discounts { get; set; } = new List<ItemLevelSaleDiscount>();
        public string MaterialId { get; set; }
        public bool isBatch { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpDate { get; set; }
        public long? SaleDetailId { get; set; }
        [ForeignKey("SaleDetailId")]
        public SaleDetail SaleDetailRefReturn { get; set; }
        public string Uom { get; set; }
        public int OrderNo { get; set; }
        public int PrintOrderNo { get; set; }

    }
}
