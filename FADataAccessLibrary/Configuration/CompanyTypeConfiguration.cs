using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class CompanyTypeConfiguration : IEntityTypeConfiguration<CompanyType>
    {
        public void Configure(EntityTypeBuilder<CompanyType> builder)
        {
            builder.HasData(
                new CompanyType() { Id = 1, Name = "Proprietorship", DisplayAs = "Proprietorship", Description = "Sole Proprietorship company" },
                new CompanyType() { Id = 2, Name = "Partnership", DisplayAs = "Partnership", Description = "One or more partnership company" },
                new CompanyType() { Id = 3, Name = "Limited", DisplayAs = "Limited Company", Description = "Limited Company" },
                new CompanyType() { Id = 4, Name = "Private Limited", DisplayAs = "Prinvate Limited Company", Description = "Private Limited Company" }                
            );
        }
    }
}
