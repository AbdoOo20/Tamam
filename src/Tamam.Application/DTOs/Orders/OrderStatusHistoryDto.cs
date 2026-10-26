using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record OrderStatusHistoryDto(long Id, OrderStatusType? FromStatusId, OrderStatusType ToStatusId, ActorType ChangedByType, int? ChangedByUserId, string? Note, DateTime ChangedAt);
