using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Constants;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasData(
            new Role { Id = 1, Name = RoleNames.Owner, NormalizedName = "OWNER", ConcurrencyStamp = "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0001" },

            new Role { Id = 2, Name = RoleNames.Staff, NormalizedName = "STAFF", ConcurrencyStamp = "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0002" },

            new Role { Id = 3, Name = RoleNames.PlatformAdmin, NormalizedName = "PLATFORMADMIN", ConcurrencyStamp = "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0003" });
    }
}
