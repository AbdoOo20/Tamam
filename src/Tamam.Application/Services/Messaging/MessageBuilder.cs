using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Messaging;

public class MessageBuilder(IUnitOfWork unitOfWork) : IMessageBuilder
{
    private const string ConfirmTemplateCode = "ORDER_CONFIRM";

    private const string SurveyTemplateCode = "REJECTION_SURVEY";

    public async Task<BuiltMessage> BuildOrderConfirmationAsync(Order order, IEnumerable<OrderItem> items, Merchant merchant, string confirmationUrl, CancellationToken cancellationToken = default)
    {
        var template = await FindTemplateAsync(ConfirmTemplateCode, merchant.DefaultLanguage, cancellationToken);

        var summary = string.Join("\n", items.Select(item => $"- {item.ProductName}{(string.IsNullOrWhiteSpace(item.Variant) ? string.Empty : $" ({item.Variant})")} x{item.Quantity}"));

        var body = (template?.Body ?? "{ConfirmLink}")
            .Replace("{CustomerName}", order.RecipientName)
            .Replace("{StoreName}", merchant.BusinessName)
            .Replace("{ItemsSummary}", summary)
            .Replace("{Total}", order.TotalAmount.ToString("0.##"))
            .Replace("{Currency}", order.CurrencyCode)
            .Replace("{Address}", $"{order.City} {order.AddressLine}".Trim())
            .Replace("{DeliveryDate}", order.ExpectedDeliveryDate?.ToString("yyyy-MM-dd") ?? "-")
            .Replace("{ConfirmLink}", confirmationUrl);

        return new BuiltMessage(template?.Id, body, BuildWhatsAppUrl(order.RecipientPhoneE164, body));
    }

    public async Task<BuiltMessage> BuildRejectionSurveyAsync(Order order, Merchant merchant, string surveyUrl, CancellationToken cancellationToken = default)
    {
        var template = await FindTemplateAsync(SurveyTemplateCode, merchant.DefaultLanguage, cancellationToken);

        var body = (template?.Body ?? "{SurveyLink}")
            .Replace("{StoreName}", merchant.BusinessName)
            .Replace("{SurveyLink}", surveyUrl);

        return new BuiltMessage(template?.Id, body, BuildWhatsAppUrl(order.RecipientPhoneE164, body));
    }

    private async Task<MessageTemplate?> FindTemplateAsync(string code, string language, CancellationToken cancellationToken)
    {
        var templates = await unitOfWork.Repository<MessageTemplate>().ListAsync(template => template.Code == code && template.IsActive, cancellationToken);

        return templates.FirstOrDefault(template => template.Language == language)
            ?? templates.FirstOrDefault(template => template.Language == language.Split('-')[0])
            ?? templates.FirstOrDefault(template => template.Language.StartsWith("ar"))
            ?? templates.FirstOrDefault();
    }

    private static string BuildWhatsAppUrl(string phoneE164, string body) => $"https://wa.me/{PhoneHelper.DigitsOnly(phoneE164)}?text={Uri.EscapeDataString(body)}";
}
