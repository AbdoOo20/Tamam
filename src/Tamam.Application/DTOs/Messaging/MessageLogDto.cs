using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Messaging;

public record MessageLogDto(long Id, MessageDirection Direction, MessageChannel Channel, string? ToPhoneE164, string? Body, MessageDeliveryStatus DeliveryStatus, DateTime CreatedAt);
