using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).HasMaxLength(150);

        builder.Property(x => x.Email).HasMaxLength(256);

        builder.Property(x => x.PasswordHash).HasMaxLength(500);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("UQ_Users_Email")
            .IsUnique();

        builder.HasIndex(x => x.MerchantId)
            .HasDatabaseName("IX_Users_Merchant");
    }
}
