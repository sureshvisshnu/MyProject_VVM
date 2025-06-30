using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class TaxDocumentTypeConfiguration : IEntityTypeConfiguration<TaxDocumentType>
    {
        public void Configure(EntityTypeBuilder<TaxDocumentType> builder)
        {
            builder.HasData(
                new TaxDocumentType() { TaxTypeId = 1, Name = "PAN" },
                new TaxDocumentType() { TaxTypeId = 2, Name = "CST" },
                new TaxDocumentType() { TaxTypeId = 3, Name = "GST" },
                new TaxDocumentType() { TaxTypeId = 4, Name = "TIN" }
            );
        }
    }
}
