using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderRiskAssessmentConfiguration : IEntityTypeConfiguration<OrderRiskAssessment>
{
    public void Configure(EntityTypeBuilder<OrderRiskAssessment> builder)
    {
        builder.ToTable("OrderRiskAssessments", table =>
        {
            table.HasCheckConstraint("CK_OrderRiskAssessments_Score", "[Score] BETWEEN 0 AND 100");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ModelVersion).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.RiskAssessments)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DecidedByUser)
            .WithMany()
            .HasForeignKey(x => x.DecidedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.CreatedAt })
            .HasDatabaseName("IX_OrderRiskAssessments_Order")
            .IsDescending(false, true);
    }
}
