using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MerchantQualityStatConfiguration : IEntityTypeConfiguration<MerchantQualityStat>
{
    public void Configure(EntityTypeBuilder<MerchantQualityStat> builder)
    {
        builder.ToTable("MerchantQualityStats");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalLoss).HasPrecision(18, 2);

        builder.Property(x => x.QualityComplaintRate).HasPrecision(5, 2);

        builder.Property(x => x.NetworkAvgComplaintRate).HasPrecision(5, 2);

        builder.Property(x => x.CalculatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.QualityStats)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.PeriodMonth })
            .HasDatabaseName("UQ_MerchantQualityStats")
            .IsUnique();
    }
}
