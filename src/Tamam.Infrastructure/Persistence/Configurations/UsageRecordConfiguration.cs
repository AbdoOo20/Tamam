using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
{
    public void Configure(EntityTypeBuilder<UsageRecord> builder)
    {
        builder.ToTable("UsageRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitCost).HasPrecision(10, 4);

        builder.Property(x => x.TotalCost).HasPrecision(18, 4).HasComputedColumnSql("CAST([Quantity] AS decimal(18,4)) * [UnitCost]", stored: true);

        builder.Property(x => x.OccurredAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.OccurredAt })
            .HasDatabaseName("IX_UsageRecords_Merchant_Date");
    }
}
