using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CustomerRiskProfileConfiguration : IEntityTypeConfiguration<CustomerRiskProfile>
{
    public void Configure(EntityTypeBuilder<CustomerRiskProfile> builder)
    {
        builder.ToTable("CustomerRiskProfiles");

        builder.HasKey(x => x.CustomerId);

        builder.Property(x => x.CustomerId).ValueGeneratedNever();

        builder.Property(x => x.WeightedFaultScore).HasPrecision(8, 2);

        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Customer)
            .WithOne(x => x.RiskProfile)
            .HasForeignKey<CustomerRiskProfile>(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
