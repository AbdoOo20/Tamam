using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasMaxLength(40).IsUnicode(false);

        builder.Property(x => x.Title).HasMaxLength(200);

        builder.Property(x => x.Body).HasMaxLength(1000);

        builder.HasOne(x => x.Merchant)
            .WithMany()
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RelatedOrder)
            .WithMany()
            .HasForeignKey(x => x.RelatedOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.IsRead, x.CreatedAt })
            .HasDatabaseName("IX_Notifications_Merchant")
            .IsDescending(false, false, true);
    }
}
