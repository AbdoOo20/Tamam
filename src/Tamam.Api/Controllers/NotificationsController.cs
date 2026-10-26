using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Notifications;

using Tamam.Application.Services.Notifications;

namespace Tamam.Api.Controllers;

[Authorize]
public class NotificationsController(INotificationService notificationService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<NotificationDto>>>> GetAll([FromQuery] NotificationQuery query, CancellationToken cancellationToken)
    {
        return Respond(await notificationService.GetAllAsync(query, cancellationToken));
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<Result<int>>> GetUnreadCount(CancellationToken cancellationToken)
    {
        return Respond(await notificationService.GetUnreadCountAsync(cancellationToken));
    }

    [HttpPost("{id:long}/read")]
    public async Task<ActionResult<Result>> MarkRead(long id, CancellationToken cancellationToken)
    {
        return Respond(await notificationService.MarkReadAsync(id, cancellationToken));
    }

    [HttpPost("read-all")]
    public async Task<ActionResult<Result>> MarkAllRead(CancellationToken cancellationToken)
    {
        return Respond(await notificationService.MarkAllReadAsync(cancellationToken));
    }
}
