using fa.model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasData(
                new State() { Id = 1, Name = "Andhra Pradesh", ISOCode = "IN-AP", DisplayAs = "AP", CountryId = 99, Code = "28" },
                new State() { Id = 2, Name = "Arunachal Pradesh", ISOCode = "IN-AR", DisplayAs = "AR", CountryId = 99, Code = "12" },
                new State() { Id = 3, Name = "Assam", ISOCode = "IN-AS", DisplayAs = "AS", CountryId = 99, Code = "18" },
                new State() { Id = 4, Name = "Bihar", ISOCode = "IN-BR", DisplayAs = "BR", CountryId = 99, Code = "10" },
                new State() { Id = 5, Name = "Chhattisgarh", ISOCode = "IN-CT", DisplayAs = "CT", CountryId = 99, Code = "22" },
                new State() { Id = 6, Name = "Goa", ISOCode = "IN-GA", DisplayAs = "GA", CountryId = 99, Code = "30" },
                new State() { Id = 7, Name = "Gujarat", ISOCode = "IN-GJ", DisplayAs = "GJ", CountryId = 99, Code = "24" },
                new State() { Id = 8, Name = "Haryana", ISOCode = "IN-HR", DisplayAs = "HR", CountryId = 99, Code = "6" },
                new State() { Id = 9, Name = "Himachal Pradesh", ISOCode = "IN-HP", DisplayAs = "HP", CountryId = 99, Code = "2" },
                new State() { Id = 10, Name = "Jammu and Kashmir", ISOCode = "IN-JK", DisplayAs = "JK", CountryId = 99, Code = "1" },
                new State() { Id = 11, Name = "Jharkhand", ISOCode = "IN-JH", DisplayAs = "JH", CountryId = 99, Code = "20" },
                new State() { Id = 12, Name = "Karnataka", ISOCode = "IN-KA", DisplayAs = "KA", CountryId = 99, Code = "29" },
                new State() { Id = 13, Name = "Kerala", ISOCode = "IN-KL", DisplayAs = "KL", CountryId = 99, Code = "32" },
                new State() { Id = 14, Name = "Madhya Pradesh", ISOCode = "IN-MP", DisplayAs = "MP", CountryId = 99, Code = "23" },
                new State() { Id = 15, Name = "Maharashtra", ISOCode = "IN-MH", DisplayAs = "MH", CountryId = 99, Code = "27" },
                new State() { Id = 16, Name = "Manipur", ISOCode = "IN-MN", DisplayAs = "MN", CountryId = 99, Code = "14" },
                new State() { Id = 17, Name = "Meghalaya", ISOCode = "IN-ML", DisplayAs = "ML", CountryId = 99, Code = "17" },
                new State() { Id = 18, Name = "Mizoram", ISOCode = "IN-MZ", DisplayAs = "MZ", CountryId = 99, Code = "15" },
                new State() { Id = 19, Name = "Nagaland", ISOCode = "IN-NL", DisplayAs = "NL", CountryId = 99, Code = "13" },
                new State() { Id = 20, Name = "Odisha (Orissa,99)", ISOCode = "IN-OR", DisplayAs = "OR", CountryId = 99, Code = "21" },
                new State() { Id = 21, Name = "Punjab", ISOCode = "IN-PB", DisplayAs = "PB", CountryId = 99, Code = "3" },
                new State() { Id = 22, Name = "Rajasthan", ISOCode = "IN-RJ", DisplayAs = "RJ", CountryId = 99, Code = "8" },
                new State() { Id = 23, Name = "Sikkim", ISOCode = "IN-SK", DisplayAs = "SK", CountryId = 99, Code = "11" },
                new State() { Id = 24, Name = "Tamil Nadu", ISOCode = "IN-TN", DisplayAs = "TN", CountryId = 99, Code = "33" },
                new State() { Id = 25, Name = "Tripura", ISOCode = "IN-TR", DisplayAs = "TR", CountryId = 99, Code = "16" },
                new State() { Id = 26, Name = "Uttar Pradesh", ISOCode = "IN-UP", DisplayAs = "UP", CountryId = 99, Code = "9" },
                new State() { Id = 27, Name = "Uttarakhand", ISOCode = "IN-UT", DisplayAs = "UT", CountryId = 99, Code = "5" },
                new State() { Id = 28, Name = "West Bengal", ISOCode = "IN-WB", DisplayAs = "WB", CountryId = 99, Code = "19" },
                new State() { Id = 31, Name = "Andaman and Nicobar Islands", ISOCode = "IN-AN", DisplayAs = "AN", CountryId = 99, Code = "35" },
                new State() { Id = 32, Name = "Chandigarh", ISOCode = "IN-CH", DisplayAs = "CH", CountryId = 99, Code = "4" },
                new State() { Id = 33, Name = "Dadra and Nagar Haveli", ISOCode = "IN-DN", DisplayAs = "DN", CountryId = 99, Code = "26" },
                new State() { Id = 34, Name = "Telangana", ISOCode = "IN-CT", DisplayAs = "CT", CountryId = 99, Code = "36" },
                new State() { Id = 35, Name = "Lakshadweep", ISOCode = "IN-LD", DisplayAs = "LD", CountryId = 99, Code = "31" },
                new State() { Id = 36, Name = "National Capital Territory of Delhi", ISOCode = "IN-DL", DisplayAs = "DL", CountryId = 99, Code = "7" },
                new State() { Id = 37, Name = "Puducherry", ISOCode = "IN-PY", DisplayAs = "PY", CountryId = 99, Code = "34" },
                new State() { Id = 38, Name = "Hyderabad", ISOCode = "IN-HY", DisplayAs = "HY", CountryId = 99, Code = "37" },
                new State() { Id = 39, Name = "Ladakh", ISOCode = "IN-LH", DisplayAs = "LH", CountryId = 99, Code = "38" },
                new State() { Id = 40, Name = "Other Territory", ISOCode = "IN-OT", DisplayAs = "OT", CountryId = 99, Code = "97" },
                new State() { Id = 41, Name = "Center Jurisdiction", ISOCode = "IN-CJ", DisplayAs = "CJ", CountryId = 99, Code = "99" }             
                );
        }
    }
}
