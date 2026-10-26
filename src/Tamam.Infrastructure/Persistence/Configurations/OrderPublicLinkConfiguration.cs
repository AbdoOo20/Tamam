using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderPublicLinkConfiguration : IEntityTypeConfiguration<OrderPublicLink>
{
    public void Configure(EntityTypeBuilder<OrderPublicLink> builder)
    {
        builder.ToTable("OrderPublicLinks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Token).HasMaxLength(64).IsUnicode(false);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.PublicLinks)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Token)
            .HasDatabaseName("UQ_OrderPublicLinks_Token")
            .IsUnique();

        builder.HasIndex(x => new { x.OrderId, x.Purpose })
            .HasDatabaseName("IX_OrderPublicLinks_Order");
    }
}
