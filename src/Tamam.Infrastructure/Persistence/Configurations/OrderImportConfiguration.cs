using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class OrderImportConfiguration : IEntityTypeConfiguration<OrderImport>
{
    public void Configure(EntityTypeBuilder<OrderImport> builder)
    {
        builder.ToTable("OrderImports");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).HasMaxLength(260);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Imports)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UploadedByUser)
            .WithMany()
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
