using fa.model.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class PrintPaperFormatConfiguration : IEntityTypeConfiguration<PrintPaperFormat>
    {
        public void Configure(EntityTypeBuilder<PrintPaperFormat> builder)
        {
            builder.HasData(
                new PrintPaperFormat() { Id = 1, Name = "105 MM ROLL" },
                new PrintPaperFormat() { Id = 2, Name = "A4 PORTRAIT" },
                new PrintPaperFormat() { Id = 3, Name = "A4 LANDSCAPE" },
                new PrintPaperFormat() { Id = 4, Name = "A5 PORTRAIT" },
                new PrintPaperFormat() { Id = 5, Name = "A5 LANDSCAPE" },
                new PrintPaperFormat() { Id = 6, Name = "80 MM ROLL" }
            );
        }
    }
}