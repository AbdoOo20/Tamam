using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class RejectionReasonConfiguration : IEntityTypeConfiguration<RejectionReason>
{
    public void Configure(EntityTypeBuilder<RejectionReason> builder)
    {
        builder.ToTable("RejectionReasons", table =>
        {
            table.HasCheckConstraint("CK_RejectionReasons_Weight", "[CustomerFaultWeight] BETWEEN 0 AND 1");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Code).HasMaxLength(40).IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(150);

        builder.Property(x => x.NameEn).HasMaxLength(150);

        builder.Property(x => x.CustomerFaultWeight).HasPrecision(3, 2);

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_RejectionReasons_Code")
            .IsUnique();

        builder.HasData(
            new RejectionReason { Id = RejectionReasonType.ChangedMind, Code = "CHANGED_MIND", NameAr = "غيّرت رأيي", NameEn = "Changed mind", Attribution = RejectionAttribution.Customer, CustomerFaultWeight = 0.50m, IsActive = true, SortOrder = 1 },

            new RejectionReason { Id = RejectionReasonType.NoCash, Code = "NO_CASH", NameAr = "مفيش معايا فلوس وقت الاستلام", NameEn = "No cash at delivery", Attribution = RejectionAttribution.Customer, CustomerFaultWeight = 0.80m, IsActive = true, SortOrder = 2 },

            new RejectionReason { Id = RejectionReasonType.NotAnswering, Code = "NOT_ANSWERING", NameAr = "ما ردّيتش على المندوب", NameEn = "Did not answer courier", Attribution = RejectionAttribution.Customer, CustomerFaultWeight = 1.00m, IsActive = true, SortOrder = 3 },

            new RejectionReason { Id = RejectionReasonType.DuplicateOrder, Code = "DUPLICATE_ORDER", NameAr = "طلب مكرر", NameEn = "Duplicate order", Attribution = RejectionAttribution.Customer, CustomerFaultWeight = 0.30m, IsActive = true, SortOrder = 4 },

            new RejectionReason { Id = RejectionReasonType.FoundCheaper, Code = "FOUND_CHEAPER", NameAr = "لقيت سعر أرخص", NameEn = "Found cheaper elsewhere", Attribution = RejectionAttribution.Customer, CustomerFaultWeight = 0.40m, IsActive = true, SortOrder = 5 },

            new RejectionReason { Id = RejectionReasonType.NotAsAdvertised, Code = "NOT_AS_ADVERTISED", NameAr = "المنتج مش زي الإعلان (الخامة/الشكل)", NameEn = "Not as advertised", Attribution = RejectionAttribution.Merchant, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 6 },

            new RejectionReason { Id = RejectionReasonType.DefectiveDamaged, Code = "DEFECTIVE_DAMAGED", NameAr = "المنتج تالف أو فيه عيب", NameEn = "Defective or damaged", Attribution = RejectionAttribution.Merchant, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 7 },

            new RejectionReason { Id = RejectionReasonType.WrongItem, Code = "WRONG_ITEM", NameAr = "المنتج أو المقاس أو اللون غلط", NameEn = "Wrong item/size/color", Attribution = RejectionAttribution.Merchant, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 8 },

            new RejectionReason { Id = RejectionReasonType.PriceMismatch, Code = "PRICE_MISMATCH", NameAr = "السعر مختلف عن المتفق عليه", NameEn = "Price mismatch", Attribution = RejectionAttribution.Merchant, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 9 },

            new RejectionReason { Id = RejectionReasonType.LateDelivery, Code = "LATE_DELIVERY", NameAr = "تأخر التوصيل", NameEn = "Late delivery", Attribution = RejectionAttribution.Carrier, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 10 },

            new RejectionReason { Id = RejectionReasonType.CourierBehavior, Code = "COURIER_BEHAVIOR", NameAr = "تصرف سيئ من المندوب", NameEn = "Courier behavior", Attribution = RejectionAttribution.Carrier, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 11 },

            new RejectionReason { Id = RejectionReasonType.AddressNotFound, Code = "ADDRESS_NOT_FOUND", NameAr = "تعذر الوصول للعنوان", NameEn = "Address not found", Attribution = RejectionAttribution.Neutral, CustomerFaultWeight = 0.20m, IsActive = true, SortOrder = 12 },

            new RejectionReason { Id = RejectionReasonType.DidNotOrder, Code = "DID_NOT_ORDER", NameAr = "أنا ما طلبتش (الرقم اتستخدم بدون علمي)", NameEn = "I did not place this order", Attribution = RejectionAttribution.Neutral, CustomerFaultWeight = 0m, IsActive = true, SortOrder = 13 },

            new RejectionReason { Id = RejectionReasonType.Other, Code = "OTHER", NameAr = "سبب آخر", NameEn = "Other", Attribution = RejectionAttribution.Neutral, CustomerFaultWeight = 0.10m, IsActive = true, SortOrder = 14 });
    }
}
