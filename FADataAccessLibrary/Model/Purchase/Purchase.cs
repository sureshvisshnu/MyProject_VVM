using fa.model;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fa.model.Purchase
{
    public class Purchase: AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
        public DateTime RefDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string DispachPlace { get; set; }
        public DateTime DispachDate { get; set; }
        public float SplDiscount { get; set; }
        public float TaxAmount { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public float Total { get; set; }       
        public ICollection<PurchaseDetail> PurchaseDetail { get; set; } = new List<PurchaseDetail>();
        public DateTime ReturnDate { get; set; }
        public long? PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public Purchase PurchaseRefReturn { get; set; }

    }
    public class TransportDetail
    {
        public long PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }
        public float Cost { get; set; }
        public string VehicleNo { get; set; }
        public string ReceiptNumber { get; set; }
        public float EractionCharge { get; set; }
        public float PackingCharge { get; set; }
        public float OtherCharge { get; set; }

    }
    public class PurchaseDetail : AuditableEntity
    {
        [Key]
        public long PurchaseDetailId { get; set; }
        public long PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public Purchase Purchase { get; set; }
        public long ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public float Rate { get; set; }
        public float Discount { get; set; }
        public float Amount { get; set; }
    }



}
