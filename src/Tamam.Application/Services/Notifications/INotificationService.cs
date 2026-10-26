using Tamam.Application.Common;

using Tamam.Application.DTOs.Notifications;

namespace Tamam.Application.Services.Notifications;

public interface INotificationService
{
    Task<Result<PagedResult<NotificationDto>>> GetAllAsync(NotificationQuery query, CancellationToken cancellationToken = default);

    Task<Result<int>> GetUnreadCountAsync(CancellationToken cancellationToken = default);

    Task<Result> MarkReadAsync(long id, CancellationToken cancellationToken = default);

    Task<Result> MarkAllReadAsync(CancellationToken cancellationToken = default);
}
