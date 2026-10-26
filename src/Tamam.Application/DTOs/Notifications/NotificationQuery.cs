using Tamam.Application.Common;

namespace Tamam.Application.DTOs.Notifications;

public class NotificationQuery : PagedQuery
{
    public bool UnreadOnly { get; set; }
}
