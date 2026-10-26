using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        builder.ToTable("OrderStatusHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Note).HasMaxLength(500);

        builder.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromStatus)
            .WithMany()
            .HasForeignKey(x => x.FromStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToStatus)
            .WithMany()
            .HasForeignKey(x => x.ToStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ChangedByUser)
            .WithMany()
            .HasForeignKey(x => x.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.ChangedAt })
            .HasDatabaseName("IX_OrderStatusHistories_Order");
    }
}
