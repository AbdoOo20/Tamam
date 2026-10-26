using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(30).IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(100);

        builder.Property(x => x.MonthlyFee).HasPrecision(10, 2);

        builder.Property(x => x.PricePerCheckedOrder).HasPrecision(10, 4);

        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsFixedLength().IsUnicode(false);

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_Plans_Code")
            .IsUnique();

        builder.HasData(
            new Plan { Id = 1, Code = "TRIAL", NameAr = "تجريبي", MonthlyFee = 0m, PricePerCheckedOrder = 0m, IncludedOrders = 200, CurrencyCode = "USD", IsActive = true });
    }
}
