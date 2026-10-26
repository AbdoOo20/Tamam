using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CarrierConfiguration : IEntityTypeConfiguration<Carrier>
{
    public void Configure(EntityTypeBuilder<Carrier> builder)
    {
        builder.ToTable("Carriers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CountryCode).HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.Name).HasMaxLength(150);

        builder.Property(x => x.Phone).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.TrackingUrlTemplate).HasMaxLength(500);

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Carriers)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
