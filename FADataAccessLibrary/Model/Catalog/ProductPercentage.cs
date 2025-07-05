using fa.api.System;
using fa.model;
using fa.model.Catalog;
using fa.model.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Catalog
{
    [Table("ProductPercentages")]
    [Index(nameof(ProductCode), Name = "IX_ProductPercentage_ProductCode")] // Moved Index attribute here
    public class ProductPercentage : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(14)]
        public string ProductCode { get; set; }

        // Reference to the Product
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AddedCostPercentage { get; set; } = 15m;

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal RetailMarginPercentage { get; set; } = 30m;

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal WholesaleMarginPercentage { get; set; } = 40m;

        [Required]
        [Range(0, 200)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MrpPercentage { get; set; } = 120m;

        // Additional fields
        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string Notes { get; set; }

        // Versioning for concurrency control
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
