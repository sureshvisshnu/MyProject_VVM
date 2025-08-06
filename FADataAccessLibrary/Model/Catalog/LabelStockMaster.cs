using fa.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Model.Catalog
{
    public class LabelStockMaster : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string LabelType { get; set; } // e.g., "25mm × 20mm"

        [MaxLength(20)]
        public string LabelSizeCode { get; set; } // e.g., "q812"

        [MaxLength(100)]
        public string RollName { get; set; }

        [Required]
        public int TotalLabelCount { get; set; }

        [Required]
        public int LabelsPerRow { get; set; }

        public int LabelsUsed { get; set; } = 0;

        public int WastedLabelCount { get; set; } = 0;

        public int RemainingCount { get; set; } = 0;

        public int ThresholdWarning { get; set; } = 50;

        [Required]
        public DateTime DateLoaded { get; set; }

        public DateTime? DateEnded { get; set; }

        public bool IsActive { get; set; } = true;

        public long? RibbonId { get; set; }

        [ForeignKey("RibbonId")]
        public virtual RibbonUsage Ribbon { get; set; }

        public string Comments { get; set; }

        public virtual ICollection<LabelStockUsage> Usages { get; set; }
    }

    public class LabelStockUsage 
    {
        [Key]
        public long Id { get; set; }

        public long LabelStockId { get; set; }

        [ForeignKey("LabelStockId")]
        public virtual LabelStockMaster LabelStock { get; set; }

        [Required]
        public DateTime DatePrinted { get; set; }

        [Required]
        public int PrintedCount { get; set; }

        public int WastedCount { get; set; } = 0;

        [MaxLength(100)]
        public string ReferenceId { get; set; } // e.g., ProductId, OrderId

        [MaxLength(100)]
        public string PrintedBy { get; set; }

        public decimal? RibbonUsedLengthM { get; set; }
    }
    public class RibbonUsage
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(100)]
        public string RibbonName { get; set; }

        [MaxLength(50)]
        public string RibbonType { get; set; }

        public decimal? TotalPrintLengthM { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal UsedLengthM { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<LabelStockMaster> LabelStocks { get; set; }
    }
}
