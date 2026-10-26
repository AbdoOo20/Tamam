using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderRiskSignalConfiguration : IEntityTypeConfiguration<OrderRiskSignal>
{
    public void Configure(EntityTypeBuilder<OrderRiskSignal> builder)
    {
        builder.ToTable("OrderRiskSignals");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExplanationAr).HasMaxLength(300);

        builder.HasOne(x => x.Assessment)
            .WithMany(x => x.Signals)
            .HasForeignKey(x => x.AssessmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Rule)
            .WithMany()
            .HasForeignKey(x => x.RuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.AssessmentId)
            .HasDatabaseName("IX_OrderRiskSignals_Assessment");
    }
}
