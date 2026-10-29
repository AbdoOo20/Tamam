using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku).HasMaxLength(100);

        builder.Property(x => x.Name).HasMaxLength(250);

        builder.Property(x => x.SearchText).HasMaxLength(400);

        builder.Property(x => x.DefaultPrice).HasPrecision(18, 2);

        builder.Property(x => x.AdUrl).HasMaxLength(500);

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.Sku })
            .HasDatabaseName("UX_Products_Merchant_Sku")
            .IsUnique()
            .HasFilter("[Sku] IS NOT NULL AND [IsDeleted] = 0");
    }
}
