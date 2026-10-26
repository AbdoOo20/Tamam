using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class RiskRuleConfiguration : IEntityTypeConfiguration<RiskRule>
{
    public void Configure(EntityTypeBuilder<RiskRule> builder)
    {
        builder.ToTable("RiskRules", table =>
        {
            table.HasCheckConstraint("CK_RiskRules_Json", "[ParametersJson] IS NULL OR ISJSON([ParametersJson]) = 1");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(50).IsUnicode(false);

        builder.Property(x => x.NameAr).HasMaxLength(150);

        builder.Property(x => x.ExplanationTemplateAr).HasMaxLength(300);

        builder.HasIndex(x => x.Code)
            .HasDatabaseName("UQ_RiskRules_Code")
            .IsUnique();

        builder.HasData(
            new RiskRule { Id = 1, Code = "INVALID_PHONE", NameAr = "رقم هاتف غير صالح", ExplanationTemplateAr = "رقم الهاتف غير صالح أو غير مكتمل", Points = 40, Scope = RuleScope.Order, IsActive = true, Version = 1 },

            new RiskRule { Id = 2, Code = "DUPLICATE_ORDER_24H", NameAr = "أوردر مكرر خلال 24 ساعة", ExplanationTemplateAr = "نفس الرقم أو العنوان طلب أوردر آخر خلال آخر 24 ساعة", Points = 20, Scope = RuleScope.Order, IsActive = true, Version = 1 },

            new RiskRule { Id = 3, Code = "ADDRESS_INCOMPLETE", NameAr = "عنوان ناقص", ExplanationTemplateAr = "العنوان ناقص أو غامض (لا يوجد شارع أو علامة مميزة)", Points = 20, Scope = RuleScope.Order, IsActive = true, Version = 1 },

            new RiskRule { Id = 4, Code = "LATE_NIGHT_ORDER", NameAr = "أوردر بعد منتصف الليل", ExplanationTemplateAr = "الأوردر اتطلب بعد منتصف الليل", Points = 5, Scope = RuleScope.Order, IsActive = true, Version = 1 },

            new RiskRule { Id = 5, Code = "HIGH_VALUE_ORDER", NameAr = "قيمة أعلى من المعتاد", ExplanationTemplateAr = "قيمة الأوردر أعلى بكثير من متوسط أوردراتك", Points = 15, Scope = RuleScope.Order, IsActive = true, Version = 1 },

            new RiskRule { Id = 6, Code = "FIRST_TIME_CUSTOMER", NameAr = "عميل جديد", ExplanationTemplateAr = "أول مرة يطلب منك", Points = 5, Scope = RuleScope.MerchantCustomerHistory, IsActive = true, Version = 1 },

            new RiskRule { Id = 7, Code = "NO_CONFIRMATION_RESPONSE", NameAr = "لم يرد على التأكيد", ExplanationTemplateAr = "العميل لم يرد على رسالة التأكيد", Points = 30, Scope = RuleScope.MerchantCustomerHistory, IsActive = true, Version = 1 },

            new RiskRule { Id = 8, Code = "NO_WHATSAPP", NameAr = "غير موجود على واتساب", ExplanationTemplateAr = "الرقم غالباً غير مسجل على واتساب", Points = 10, Scope = RuleScope.MerchantCustomerHistory, IsActive = true, Version = 1 },

            new RiskRule { Id = 9, Code = "CONFIRMED_QUICKLY", NameAr = "أكد بسرعة", ExplanationTemplateAr = "العميل أكد الطلب خلال دقائق", Points = -10, Scope = RuleScope.MerchantCustomerHistory, IsActive = true, Version = 1 },

            new RiskRule { Id = 10, Code = "GOOD_HISTORY", NameAr = "سجل ممتاز", ExplanationTemplateAr = "العميل استلم أوردرات سابقة بنجاح", Points = -20, Scope = RuleScope.MerchantCustomerHistory, IsActive = true, Version = 1 },

            new RiskRule { Id = 11, Code = "NETWORK_PRIOR_REFUSALS", NameAr = "رفض سابق في الشبكة", ExplanationTemplateAr = "الرقم رفض أوردرات عند تجار آخرين مؤخراً بسبب يخصه", Points = 35, Scope = RuleScope.Network, IsActive = true, Version = 1 });
    }
}
