namespace Tamam.Application.DTOs.Audit;

public record AuditLogDto(
    long Id,

    int? MerchantId,

    int? UserId,

    string? UserName,

    string Action,

    string EntityName,

    string? EntityId,

    string? OldValues,

    string? NewValues,

    string? IpAddress,

    DateTime CreatedAt);
