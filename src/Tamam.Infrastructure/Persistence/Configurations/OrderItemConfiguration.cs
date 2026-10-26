using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", table =>
        {
            table.HasCheckConstraint("CK_OrderItems_Qty", "[Quantity] > 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName).HasMaxLength(250);

        builder.Property(x => x.Variant).HasMaxLength(150);

        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);

        builder.Property(x => x.LineTotal).HasPrecision(18, 2).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("IX_OrderItems_Order");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("IX_OrderItems_Product")
            .HasFilter("[ProductId] IS NOT NULL");
    }
}
