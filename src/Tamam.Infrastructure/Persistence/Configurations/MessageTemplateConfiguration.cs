using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Infrastructure.Persistence.Configurations;

public class MessageTemplateConfiguration : IEntityTypeConfiguration<MessageTemplate>
{
    public void Configure(EntityTypeBuilder<MessageTemplate> builder)
    {
        builder.ToTable("MessageTemplates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(60).IsUnicode(false);

        builder.Property(x => x.Language).HasMaxLength(10).IsUnicode(false);

        builder.Property(x => x.Body).HasMaxLength(1000);

        builder.Property(x => x.ProviderTemplateName).HasMaxLength(100).IsUnicode(false);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(x => new { x.Code, x.Language, x.Channel })
            .HasDatabaseName("UQ_MessageTemplates")
            .IsUnique();

        builder.HasData(
            new MessageTemplate { Id = 1, Code = "ORDER_CONFIRM", Language = "ar-EG", Channel = MessageChannel.WaMeLink, Category = MessageCategory.Utility, Body = "أهلاً {CustomerName}،\nطلبك من {StoreName}:\n{ItemsSummary}\nالإجمالي: {Total} {Currency}\nالعنوان: {Address}\nميعاد التوصيل المتوقع: {DeliveryDate}\nعشان نأكد الطلب اضغط على اللينك: {ConfirmLink}", ApprovalStatus = TemplateApprovalStatus.NotRequired, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            new MessageTemplate { Id = 2, Code = "ORDER_CONFIRM", Language = "ar-SA", Channel = MessageChannel.WaMeLink, Category = MessageCategory.Utility, Body = "حياك الله {CustomerName}،\nطلبك من {StoreName}:\n{ItemsSummary}\nالإجمالي: {Total} {Currency}\nالعنوان: {Address}\nموعد التوصيل المتوقع: {DeliveryDate}\nلتأكيد الطلب تفضل بفتح الرابط: {ConfirmLink}", ApprovalStatus = TemplateApprovalStatus.NotRequired, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            new MessageTemplate { Id = 3, Code = "REJECTION_SURVEY", Language = "ar", Channel = MessageChannel.WaMeLink, Category = MessageCategory.Utility, Body = "نأسف لعدم إتمام طلبك من {StoreName}. ساعدنا بمعرفة السبب في ثوانٍ: {SurveyLink}", ApprovalStatus = TemplateApprovalStatus.NotRequired, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) });
    }
}
