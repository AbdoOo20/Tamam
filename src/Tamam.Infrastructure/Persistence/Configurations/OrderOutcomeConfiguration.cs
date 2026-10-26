using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderOutcomeConfiguration : IEntityTypeConfiguration<OrderOutcome>
{
    public void Configure(EntityTypeBuilder<OrderOutcome> builder)
    {
        builder.ToTable("OrderOutcomes", table =>
        {
            table.HasCheckConstraint("CK_OrderOutcomes_Weight", "[CustomerFaultWeight] BETWEEN 0 AND 1");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("OrderId").ValueGeneratedNever();

        builder.Property(x => x.CustomerFaultWeight).HasPrecision(3, 2);

        builder.Property(x => x.OutboundShippingCost).HasPrecision(18, 2);

        builder.Property(x => x.ReturnShippingCost).HasPrecision(18, 2);

        builder.Property(x => x.OtherLoss).HasPrecision(18, 2);

        builder.Property(x => x.LossAmount)
            .HasPrecision(18, 2)
            .HasComputedColumnSql("[OutboundShippingCost] + [ReturnShippingCost] + [OtherLoss]", stored: true);

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Outcome)
            .HasForeignKey<OrderOutcome>(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FinalReason)
            .WithMany()
            .HasForeignKey(x => x.FinalReasonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.FinalResult, x.Attribution })
            .HasDatabaseName("IX_OrderOutcomes_Result");
    }
}
