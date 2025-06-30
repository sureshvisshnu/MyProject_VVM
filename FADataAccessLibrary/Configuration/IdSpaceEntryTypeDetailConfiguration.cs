using fa.model.Accounting.Masters;
using fa.model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class IdSpaceEntryTypeDetailConfiguration : IEntityTypeConfiguration<IdSpaceEntryTypeDetail>
    {
        public void Configure(EntityTypeBuilder<IdSpaceEntryTypeDetail> builder)
        {
            builder.HasData(
                new IdSpaceEntryTypeDetail() { Id = 1, EntryType = EntryType.INVOICE, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 2, EntryType = EntryType.BILL, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 3, EntryType = EntryType.RECEIPT, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 4, EntryType = EntryType.PAYMENT, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 5, EntryType = EntryType.DEBIT_NOTE, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 6, EntryType = EntryType.CREDIT_NOTE, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 7, EntryType = EntryType.EXPENSE, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 8, EntryType = EntryType.JOURNAL, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 9, EntryType = EntryType.SALES, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 10, EntryType = EntryType.SALES_QUOTE, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 11, EntryType = EntryType.SALES_RETURN, HasPrinterSetup = true, HasDotMatrix = true, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 12, EntryType = EntryType.PURCHASE, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 13, EntryType = EntryType.PURCHASE_ORDER, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 14, EntryType = EntryType.PURCHASE_RETURN, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 15, EntryType = EntryType.STOCK_IN, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 16, EntryType = EntryType.STOCK_OUT, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 17, EntryType = EntryType.STOCK_OPENING, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 18, EntryType = EntryType.STOCK_PURCHASE, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 19, EntryType = EntryType.STOCK_PURCHASERETURN, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 20, EntryType = EntryType.STOCK_SALE, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 21, EntryType = EntryType.STOCK_SALERETURN, HasPrinterSetup = false, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 22, EntryType = EntryType.PRESCRIPTION, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 23, EntryType = EntryType.PATIENT_FEE_RECEIPT, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 24, EntryType = EntryType.PATIENT_INVOICE, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = true, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 25, EntryType = EntryType.OP_TOKEN, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 26, EntryType = EntryType.STOCK_ADJUSTMENT, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 27, EntryType = EntryType.STOCK_DAMAGE, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty },
                new IdSpaceEntryTypeDetail() { Id = 28, EntryType = EntryType.STOCK_TOPATIENT, HasPrinterSetup = true, HasDotMatrix = false, HasRoundOff = false, IsDotMatrix = false, IsResetDaily = false, RoundOff = 0.00, Prefix = string.Empty }
            );
        }
    }
}
