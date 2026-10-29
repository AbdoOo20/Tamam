using Tamam.Application.DTOs.Notifications;

namespace Tamam.Application.Abstractions;

public record NotificationPush(int MerchantId, int? UserId, NotificationDto Notification);

public interface INotificationPublisher
{
    Task PublishAsync(IReadOnlyCollection<NotificationPush> pushes, CancellationToken cancellationToken = default);
}
