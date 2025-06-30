using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role() { RoleId = 1, Name = "Admin", Description = "Admin User, This user will have access to the entire system" },
                new Role() { RoleId = 2, Name = "Accountant", Description = "Accountant User, This user will have access to most of the finacial accounting functionality" },
                new Role() { RoleId = 3, Name = "Standard", Description = "POS User, This user will have access to Point of Sale" },
                new Role() { RoleId = 4, Name = "POSUser", Description = "POS User, This user will have access to Point of Sale" },
                new Role() { RoleId = 5, Name = "Purchaser", Description = "POS User, This user will have access to Point of Sale" },
                new Role() { RoleId = 6, Name = "Doctor", Description = "Hospital User, This user will have access to Point of Hospital" },
                new Role() { RoleId = 7, Name = "Nurse", Description = "Hospital User, This user will have access to Point of Hospital" }
            );
        }
    }
}
