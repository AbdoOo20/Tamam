using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.SignalR;

using Tamam.Application.Abstractions;

namespace Tamam.Api.Hubs;

[Authorize]
public class NotificationsHub : Hub
{
    public const string Route = "/hubs/notifications";

    public const string ReceiveMethod = "notification";

    public static string MerchantGroup(int merchantId) => $"merchant:{merchantId}";

    public static string UserGroup(int userId) => $"user:{userId}";

    public override async Task OnConnectedAsync()
    {
        var principal = Context.User;

        if (int.TryParse(principal?.FindFirst(AppClaimTypes.MerchantId)?.Value, out var merchantId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, MerchantGroup(merchantId));
        }

        if (int.TryParse(principal?.FindFirst(AppClaimTypes.UserId)?.Value, out var userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, UserGroup(userId));
        }

        await base.OnConnectedAsync();
    }
}
