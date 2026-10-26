using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Notifications;

public static class NotificationFactory
{
    public static Notification ForOrder(Order order, string type, string title, string? body = null) => new()
    {
        MerchantId = order.MerchantId,

        Type = type,

        Title = title,

        Body = body,

        RelatedOrder = order
    };

    public static Notification ForOrderId(int merchantId, long orderId, string type, string title, string? body = null) => new()
    {
        MerchantId = merchantId,

        Type = type,

        Title = title,

        Body = body,

        RelatedOrderId = orderId
    };
}
