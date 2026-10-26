using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MerchantSubscriptionConfiguration : IEntityTypeConfiguration<MerchantSubscription>
{
    public void Configure(EntityTypeBuilder<MerchantSubscription> builder)
    {
        builder.ToTable("MerchantSubscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Plan)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.Status })
            .HasDatabaseName("IX_MerchantSubscriptions_Merchant");
    }
}
