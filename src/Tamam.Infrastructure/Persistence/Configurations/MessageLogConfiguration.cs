using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MessageLogConfiguration : IEntityTypeConfiguration<MessageLog>
{
    public void Configure(EntityTypeBuilder<MessageLog> builder)
    {
        builder.ToTable("MessageLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToPhoneE164).HasMaxLength(20).IsUnicode(false);

        builder.Property(x => x.ProviderMessageId).HasMaxLength(100).IsUnicode(false);

        builder.Property(x => x.Body).HasMaxLength(1000);

        builder.Property(x => x.ErrorCode).HasMaxLength(50).IsUnicode(false);

        builder.Property(x => x.Cost).HasPrecision(10, 4);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.MessageLogs)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ConfirmationRequest)
            .WithMany(x => x.MessageLogs)
            .HasForeignKey(x => x.ConfirmationRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("IX_MessageLogs_Order");

        builder.HasIndex(x => x.ProviderMessageId)
            .HasDatabaseName("IX_MessageLogs_Provider")
            .HasFilter("[ProviderMessageId] IS NOT NULL");
    }
}
