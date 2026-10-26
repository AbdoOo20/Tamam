using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CustomerDataRequestConfiguration : IEntityTypeConfiguration<CustomerDataRequest>
{
    public void Configure(EntityTypeBuilder<CustomerDataRequest> builder)
    {
        builder.ToTable("CustomerDataRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ProcessedByUser)
            .WithMany()
            .HasForeignKey(x => x.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
