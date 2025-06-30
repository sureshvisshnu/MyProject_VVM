using fa.model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasData(
                new Currency() { CurrencyId = 1, CurrencyCodeISO = "BDT", Name = "Taka", DisplayAs = "BDT", RoundingPrecision = 2, CurrencyFormat = "" },
                new Currency() { CurrencyId = 2, CurrencyCodeISO = "LKR", Name = "Sri Lanka Rupee", DisplayAs = "LKR", RoundingPrecision = 2, CurrencyFormat = "" },
                new Currency() { CurrencyId = 3, CurrencyCodeISO = "PKR", Name = "Pakistan Rupee", DisplayAs = "PKRLKR", RoundingPrecision = 2, CurrencyFormat = "" },
                new Currency() { CurrencyId = 165, CurrencyCodeISO = "INR", Name = "Indian Rupee", DisplayAs = "INR", RoundingPrecision = 2, CurrencyFormat = "#,##,##,##0.00" }
            );
        }
    }
}
