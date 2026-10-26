using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", table =>
        {
            table.HasCheckConstraint("CK_Orders_Total", "[TotalAmount] = [ItemsTotal] + [ShippingFee] - [DiscountAmount]");

            table.HasCheckConstraint("CK_Orders_Deposit", "[DepositAmount] >= 0 AND [DepositAmount] <= [TotalAmount]");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNo).HasMaxLength(50);

        builder.Property(x => x.RecipientName).HasMaxLength(150);

        builder.Property(x => x.RecipientPhoneE164).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.AltPhoneE164).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.CountryCode).HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.City).HasMaxLength(100);

        builder.Property(x => x.AddressLine).HasMaxLength(500);

        builder.Property(x => x.Landmark).HasMaxLength(200);

        builder.Property(x => x.Latitude).HasPrecision(9, 6);

        builder.Property(x => x.Longitude).HasPrecision(9, 6);

        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.ItemsTotal).HasPrecision(18, 2);

        builder.Property(x => x.ShippingFee).HasPrecision(18, 2);

        builder.Property(x => x.DiscountAmount).HasPrecision(18, 2);

        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);

        builder.Property(x => x.DepositAmount).HasPrecision(18, 2);

        builder.Property(x => x.AmountToCollect)
            .HasPrecision(18, 2)
            .HasComputedColumnSql("CASE WHEN [PaymentMethod] = 2 THEN CAST(0 AS decimal(18,2)) ELSE [TotalAmount] - [DepositAmount] END", stored: true);

        builder.Property(x => x.TrackingNo).HasMaxLength(100);

        builder.Property(x => x.Notes).HasMaxLength(1000);

        builder.Property(x => x.PlacedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MerchantCustomer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.MerchantCustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Import)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ImportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Region)
            .WithMany()
            .HasForeignKey(x => x.RegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Carrier)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CarrierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.OrderNo })
            .HasDatabaseName("UQ_Orders_Merchant_OrderNo")
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => new { x.MerchantId, x.PlacedAt })
            .HasDatabaseName("IX_Orders_Merchant_Placed")
            .IsDescending(false, true)
            .IncludeProperties(x => x.StatusId);

        builder.HasIndex(x => new { x.MerchantId, x.StatusId, x.ExpectedDeliveryDate })
            .HasDatabaseName("IX_Orders_Merchant_Status");

        builder.HasIndex(x => new { x.CustomerId, x.PlacedAt })
            .HasDatabaseName("IX_Orders_Customer")
            .IsDescending(false, true);

        builder.HasIndex(x => x.CarrierId)
            .HasDatabaseName("IX_Orders_Carrier")
            .HasFilter("[CarrierId] IS NOT NULL");
    }
}
