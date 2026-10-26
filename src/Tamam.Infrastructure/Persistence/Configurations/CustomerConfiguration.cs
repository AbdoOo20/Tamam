using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneHash).HasColumnType("binary(32)");

        builder.Property(x => x.CountryCode).HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.FirstSeenAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.PhoneHash)
            .HasDatabaseName("UQ_Customers_PhoneHash")
            .IsUnique();
    }
}
