using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class ConfirmationRequestConfiguration : IEntityTypeConfiguration<ConfirmationRequest>
{
    public void Configure(EntityTypeBuilder<ConfirmationRequest> builder)
    {
        builder.ToTable("ConfirmationRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResponseNote).HasMaxLength(500);

        builder.Property(x => x.Cost).HasPrecision(10, 4);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.ConfirmationRequests)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.AttemptNo })
            .HasDatabaseName("UQ_ConfirmationRequests")
            .IsUnique();

        builder.HasIndex(x => new { x.Status, x.SentAt })
            .HasDatabaseName("IX_ConfirmationRequests_Status");
    }
}
