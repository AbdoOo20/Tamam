using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Messaging;

public record ConfirmationDto(
    long OrderId,

    byte AttemptNo,

    ConfirmationStatus Status,

    string Link,

    DateTime ExpiresAt,

    string MessageBody,

    string? WhatsAppUrl,

    DateTime? SentAt,

    DateTime? RespondedAt,

    DateOnly? ProposedDeliveryDate,

    string? ResponseNote);
