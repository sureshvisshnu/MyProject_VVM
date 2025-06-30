using fa.api.Accounting;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.System
{
    public class IdSpace : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public EntryType EntryType { get; set; }
        public long? PrintPaperFormat_Id { get; set; }
        [ForeignKey("PrintPaperFormat_Id")]
        public PrintPaperFormat PrintPaperFormat { get; set; }
        public bool IsDotMatrix { get; set; }
        public bool IsResetDaily { get; set; }
        public double RoundOff { get; set; }
        [Required]
        public long Seed { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        public String Prefix { get; set; }
        [Required]
        public DateTime YearStartDate { get; set; }
        [Required]
        public DateTime YearEndDate { get; set; }
        [Required]
        public long RunningSeed { get; set; }
        public bool HasPrinterSetup 
        { 
            get; 
            set;
        }
        public bool HasRoundOff { get; set; }
        public bool HasDotMatrix { get; set; }
    }

    public class IdSpaceEntryTypeDetail
    {
        [Key]
        public long Id { get; set; }
        public EntryType EntryType { get; set; }
        public bool IsDotMatrix { get; set; }
        public bool IsResetDaily { get; set; }
        public double RoundOff { get; set; }
        public bool HasPrinterSetup { get; set; }
        public bool HasRoundOff { get; set; }
        public bool HasDotMatrix { get; set; }
        public String Prefix { get; set; }
        
    }

}
