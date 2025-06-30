using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FADataAccessLibrary.Model.Common;
using System.Globalization;

namespace FADataAccessLibrary.Configuration
{
    public class CountryTaxConfiguration : IEntityTypeConfiguration<CountrySaleTax>
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        public void Configure(EntityTypeBuilder<CountrySaleTax> builder)
        {
            builder.HasData(
                new CountrySaleTax() { Id = 1, Name = "IGST", CountryId = 99, Discription = "Integrated Sales Tax Payable Account",Rule="RunIGST()",EffectiveFrom= DateTime.ParseExact("09-01-2017", "MM-dd-yyyy", provider), EffectiveTo=DateTime.Now.AddYears(2400-DateTime.Now.Year) },
                new CountrySaleTax() { Id = 2, Name = "CGST", CountryId = 99, Discription = "Central Sales Tax Payable Account", Rule = "RunCGST()", EffectiveFrom = DateTime.ParseExact("09-01-2017", "MM-dd-yyyy", provider), EffectiveTo = DateTime.Now.AddYears(2400 - DateTime.Now.Year) },
                new CountrySaleTax() { Id = 3, Name = "SGST", CountryId = 99, Discription = "State Sales Tax Payable Account", Rule = "RunSGST()", EffectiveFrom = DateTime.ParseExact("09-01-2017", "MM-dd-yyyy", provider), EffectiveTo = DateTime.Now.AddYears(2400 - DateTime.Now.Year) },
                new CountrySaleTax() { Id = 4, Name = "TCS", CountryId = 99, Discription = "Tax at Source Payable Account", Rule = "RunTCS()", EffectiveFrom = DateTime.ParseExact("09-01-2017", "MM-dd-yyyy", provider), EffectiveTo = DateTime.Now.AddYears(2400 - DateTime.Now.Year) }
                );
        }
    }
}
