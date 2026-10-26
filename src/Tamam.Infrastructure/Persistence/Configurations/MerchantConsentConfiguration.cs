using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MerchantConsentConfiguration : IEntityTypeConfiguration<MerchantConsent>
{
    public void Configure(EntityTypeBuilder<MerchantConsent> builder)
    {
        builder.ToTable("MerchantConsents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TermsVersion).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.AcceptedFromIp).HasMaxLength(45).IsUnicode(false);

        builder.Property(x => x.AcceptedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Merchant)
            .WithMany(x => x.Consents)
            .HasForeignKey(x => x.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AcceptedByUser)
            .WithMany()
            .HasForeignKey(x => x.AcceptedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.MerchantId, x.AcceptedAt })
            .HasDatabaseName("IX_MerchantConsents_Merchant")
            .IsDescending(false, true);
    }
}
