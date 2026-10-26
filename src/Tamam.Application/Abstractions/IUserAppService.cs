namespace Tamam.Application.Abstractions;

public interface IUserAppService
{
    bool IsAuthenticated { get; }

    int? UserId { get; }

    int? MerchantId { get; }

    string? Role { get; }

    string? IpAddress { get; }

    int RequiredUserId { get; }

    int RequiredMerchantId { get; }
}
