using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("Code").HasMaxLength(2).IsFixedLength().IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(100);

        builder.Property(x => x.NameEn).HasMaxLength(100);

        builder.Property(x => x.PhoneCode).HasMaxLength(6).IsUnicode(false);

        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsFixedLength().IsUnicode(false);

        builder.HasData(
            new Country { Id = "EG", NameAr = "مصر", NameEn = "Egypt", PhoneCode = "+20", CurrencyCode = "EGP", IsActive = true },

            new Country { Id = "SA", NameAr = "السعودية", NameEn = "Saudi Arabia", PhoneCode = "+966", CurrencyCode = "SAR", IsActive = true },

            new Country { Id = "AE", NameAr = "الإمارات", NameEn = "UAE", PhoneCode = "+971", CurrencyCode = "AED", IsActive = true },

            new Country { Id = "KW", NameAr = "الكويت", NameEn = "Kuwait", PhoneCode = "+965", CurrencyCode = "KWD", IsActive = true },

            new Country { Id = "QA", NameAr = "قطر", NameEn = "Qatar", PhoneCode = "+974", CurrencyCode = "QAR", IsActive = true },

            new Country { Id = "BH", NameAr = "البحرين", NameEn = "Bahrain", PhoneCode = "+973", CurrencyCode = "BHD", IsActive = true },

            new Country { Id = "OM", NameAr = "عُمان", NameEn = "Oman", PhoneCode = "+968", CurrencyCode = "OMR", IsActive = true },

            new Country { Id = "JO", NameAr = "الأردن", NameEn = "Jordan", PhoneCode = "+962", CurrencyCode = "JOD", IsActive = true },

            new Country { Id = "PK", NameAr = "باكستان", NameEn = "Pakistan", PhoneCode = "+92", CurrencyCode = "PKR", IsActive = true },

            new Country { Id = "IN", NameAr = "الهند", NameEn = "India", PhoneCode = "+91", CurrencyCode = "INR", IsActive = true },

            new Country { Id = "NG", NameAr = "نيجيريا", NameEn = "Nigeria", PhoneCode = "+234", CurrencyCode = "NGN", IsActive = true });
    }
}
