using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class DeliveryAttemptConfiguration : IEntityTypeConfiguration<DeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<DeliveryAttempt> builder)
    {
        builder.ToTable("DeliveryAttempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CarrierNote).HasMaxLength(500);

        builder.Property(x => x.ProofImageUrl).HasMaxLength(500);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.DeliveryAttempts)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.AttemptNo })
            .HasDatabaseName("UQ_DeliveryAttempts")
            .IsUnique();
    }
}
