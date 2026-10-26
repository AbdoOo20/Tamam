using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MerchantCustomerConfiguration : IEntityTypeConfiguration<MerchantCustomer>
{
    public void Configure(EntityTypeBuilder<MerchantCustomer> builder)
    {
        builder.ToTable("MerchantCustomers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).HasMaxLength(150);

        builder.Property(x => x.PhoneE164).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.AltPhoneE164).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.Notes).HasMaxLength(500);

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.MerchantCustomers)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.MerchantCustomers)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.CustomerId })
            .HasDatabaseName("UQ_MerchantCustomers")
            .IsUnique();
    }
}
