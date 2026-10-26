using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BusinessName).HasMaxLength(200);

        builder.Property(x => x.OwnerName).HasMaxLength(150);

        builder.Property(x => x.Phone).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.Email).HasMaxLength(256);

        builder.Property(x => x.CountryCode).HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.DefaultLanguage).HasMaxLength(10).IsUnicode(false);

        builder.Property(x => x.StoreUrl).HasMaxLength(500);

        builder.Property(x => x.InstagramHandle).HasMaxLength(100);

        builder.Property(x => x.ApiKeyHash).HasColumnType("binary(32)");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
