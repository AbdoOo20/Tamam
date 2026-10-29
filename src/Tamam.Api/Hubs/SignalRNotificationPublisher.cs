using Microsoft.AspNetCore.SignalR;

using Tamam.Application.Abstractions;

namespace Tamam.Api.Hubs;

public class SignalRNotificationPublisher(IHubContext<NotificationsHub> hub, ILogger<SignalRNotificationPublisher> logger) : INotificationPublisher
{
    public async Task PublishAsync(IReadOnlyCollection<NotificationPush> pushes, CancellationToken cancellationToken = default)
    {
        foreach (var push in pushes)
        {
            try
            {
                var group = push.UserId is { } userId ? NotificationsHub.UserGroup(userId) : NotificationsHub.MerchantGroup(push.MerchantId);

                await hub.Clients.Group(group).SendAsync(NotificationsHub.ReceiveMethod, push.Notification, cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Could not push notification {NotificationId} over SignalR.", push.Notification.Id);
            }
        }
    }
}
