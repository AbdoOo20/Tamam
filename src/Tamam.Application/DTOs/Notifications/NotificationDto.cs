namespace Tamam.Application.DTOs.Notifications;

public record NotificationDto(long Id, string Type, string Title, string? Body, long? RelatedOrderId, bool IsRead, DateTime CreatedAt);
