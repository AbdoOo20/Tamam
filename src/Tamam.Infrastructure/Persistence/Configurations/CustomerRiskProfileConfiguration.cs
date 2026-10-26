using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CustomerRiskProfileConfiguration : IEntityTypeConfiguration<CustomerRiskProfile>
{
    public void Configure(EntityTypeBuilder<CustomerRiskProfile> builder)
    {
        builder.ToTable("CustomerRiskProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("CustomerId").ValueGeneratedNever();

        builder.Property(x => x.WeightedFaultScore).HasPrecision(8, 2);

        builder.HasOne(x => x.Customer)
            .WithOne(x => x.RiskProfile)
            .HasForeignKey<CustomerRiskProfile>(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
