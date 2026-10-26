using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Notifications;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Notifications;

public class NotificationService(IUnitOfWork unitOfWork, IUserAppService userAppService) : INotificationService
{
    public async Task<Result<PagedResult<NotificationDto>>> GetAllAsync(NotificationQuery query, CancellationToken cancellationToken = default)
    {
        var notifications = Visible();

        if (query.UnreadOnly)
        {
            notifications = notifications.Where(notification => !notification.IsRead);
        }

        var page = await notifications
            .OrderByDescending(notification => notification.CreatedAt)
            .Select(notification => new NotificationDto(notification.Id, notification.Type, notification.Title, notification.Body, notification.RelatedOrderId, notification.IsRead, notification.CreatedAt))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<NotificationDto>>.Ok(page);
    }

    public async Task<Result<int>> GetUnreadCountAsync(CancellationToken cancellationToken = default)
    {
        var count = await Visible().CountAsync(notification => !notification.IsRead, cancellationToken);

        return Result<int>.Ok(count);
    }

    public async Task<Result> MarkReadAsync(long id, CancellationToken cancellationToken = default)
    {
        var notification = await Visible(asNoTracking: false).FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (notification is null)
        {
            return Result.NotFound("Notification was not found.");
        }

        notification.IsRead = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> MarkAllReadAsync(CancellationToken cancellationToken = default)
    {
        var unread = await Visible(asNoTracking: false).Where(notification => !notification.IsRead).ToListAsync(cancellationToken);

        foreach (var notification in unread)
        {
            notification.IsRead = true;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok($"{unread.Count} notifications marked as read.");
    }

    private IQueryable<Notification> Visible(bool asNoTracking = true)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var userId = userAppService.UserId;

        return unitOfWork.Repository<Notification>().Query(asNoTracking)
            .Where(notification => notification.MerchantId == merchantId && (notification.UserId == null || notification.UserId == userId));
    }
}
