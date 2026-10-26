using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class RejectionReportConfiguration : IEntityTypeConfiguration<RejectionReport>
{
    public void Configure(EntityTypeBuilder<RejectionReport> builder)
    {
        builder.ToTable("RejectionReports");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Details).HasMaxLength(1000);

        builder.Property(x => x.EvidenceUrl).HasMaxLength(500);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.RejectionReports)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReportedByUser)
            .WithMany()
            .HasForeignKey(x => x.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Reason)
            .WithMany()
            .HasForeignKey(x => x.ReasonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OrderId, x.ReporterType })
            .HasDatabaseName("UQ_RejectionReports")
            .IsUnique();
    }
}
