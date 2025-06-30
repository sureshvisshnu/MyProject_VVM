using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class AccountingMethodConfiguration : IEntityTypeConfiguration<AccountingMethod>
    {
        public void Configure(EntityTypeBuilder<AccountingMethod> builder)
        {
            builder.HasData(
                new AccountingMethod() { AccountingMethodId = 1, Name = "Cash" },
                new AccountingMethod() { AccountingMethodId = 2, Name = "Accrual" }
            );
        }
    }
}
