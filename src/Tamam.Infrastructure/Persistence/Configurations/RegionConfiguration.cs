using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class RegionConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable("Regions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CountryCode).HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(100);

        builder.Property(x => x.NameEn).HasMaxLength(100);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ParentRegion)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentRegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CountryCode, x.ParentRegionId })
            .HasDatabaseName("IX_Regions_Country");
    }
}
