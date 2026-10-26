using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.FullName).HasMaxLength(150);

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.NormalizedUserName)
            .HasDatabaseName("UserNameIndex")
            .IsUnique()
            .HasFilter("[NormalizedUserName] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasIndex(x => x.NormalizedEmail)
            .HasDatabaseName("EmailIndex")
            .IsUnique()
            .HasFilter("[NormalizedEmail] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasIndex(x => x.MerchantId)
            .HasDatabaseName("IX_Users_Merchant");
    }
}
