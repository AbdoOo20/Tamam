using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatuses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Code).HasMaxLength(30).IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(60);

        builder.Property(x => x.NameEn).HasMaxLength(60);

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_OrderStatuses_Code")
            .IsUnique();

        builder.HasData(
            new OrderStatus { Id = OrderStatusType.New, Code = "New", NameAr = "جديد", NameEn = "New", IsFinal = false, SortOrder = 1 },

            new OrderStatus { Id = OrderStatusType.AwaitingConfirmation, Code = "AwaitingConfirmation", NameAr = "بانتظار التأكيد", NameEn = "Awaiting confirmation", IsFinal = false, SortOrder = 2 },

            new OrderStatus { Id = OrderStatusType.Confirmed, Code = "Confirmed", NameAr = "تم التأكيد", NameEn = "Confirmed", IsFinal = false, SortOrder = 3 },

            new OrderStatus { Id = OrderStatusType.DepositRequested, Code = "DepositRequested", NameAr = "مطلوب عربون", NameEn = "Deposit requested", IsFinal = false, SortOrder = 4 },

            new OrderStatus { Id = OrderStatusType.Shipped, Code = "Shipped", NameAr = "تم الشحن", NameEn = "Shipped", IsFinal = false, SortOrder = 5 },

            new OrderStatus { Id = OrderStatusType.OutForDelivery, Code = "OutForDelivery", NameAr = "خرج للتسليم", NameEn = "Out for delivery", IsFinal = false, SortOrder = 6 },

            new OrderStatus { Id = OrderStatusType.Delivered, Code = "Delivered", NameAr = "تم التسليم", NameEn = "Delivered", IsFinal = true, SortOrder = 7 },

            new OrderStatus { Id = OrderStatusType.Refused, Code = "Refused", NameAr = "مرفوض عند الاستلام", NameEn = "Refused", IsFinal = true, SortOrder = 8 },

            new OrderStatus { Id = OrderStatusType.FailedDelivery, Code = "FailedDelivery", NameAr = "تعذر التسليم", NameEn = "Failed delivery", IsFinal = true, SortOrder = 9 },

            new OrderStatus { Id = OrderStatusType.Returned, Code = "Returned", NameAr = "تم الإرجاع للتاجر", NameEn = "Returned to merchant", IsFinal = true, SortOrder = 10 },

            new OrderStatus { Id = OrderStatusType.CancelledByMerchant, Code = "CancelledByMerchant", NameAr = "ألغاه التاجر", NameEn = "Cancelled by merchant", IsFinal = true, SortOrder = 11 },

            new OrderStatus { Id = OrderStatusType.CancelledByCustomer, Code = "CancelledByCustomer", NameAr = "ألغاه العميل", NameEn = "Cancelled by customer", IsFinal = true, SortOrder = 12 });
    }
}
