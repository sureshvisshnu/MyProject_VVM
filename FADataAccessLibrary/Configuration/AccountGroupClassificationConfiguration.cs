using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class AccountGroupClassificationConfiguration : IEntityTypeConfiguration<AccountGroupClassification>
    {
        public void Configure(EntityTypeBuilder<AccountGroupClassification> builder)
        {
            builder.HasData(
                new AccountGroupClassification() { Id = 1, Name = "Income", CreditMultiplier = 1, DebitMultiplier = -1 },
                new AccountGroupClassification() { Id = 2, Name = "Expense", CreditMultiplier = -1, DebitMultiplier = 1 },
                new AccountGroupClassification() { Id = 3, Name = "Asset", CreditMultiplier = -1, DebitMultiplier = 1 },
                new AccountGroupClassification() { Id = 4, Name = "Liability", CreditMultiplier = 1, DebitMultiplier = -1 }
            );
        }
    }
}
