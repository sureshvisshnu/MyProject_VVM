using fa.model.Accounting.Masters;
using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User()
                {
                    UserId = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Login = "admin",
                    IsSuperAdmin = true,
                    IsResetPassword = true,
                    Password = "5wiGU7luIvOPsyEa58V7+A=="
                }
            );

            builder.HasMany<Role>(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "userrole",
                    ur => ur.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    ur => ur.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    ur => ur.HasData(new { UserId = 1L, RoleId = 1L })
                );
        }
    }
}
