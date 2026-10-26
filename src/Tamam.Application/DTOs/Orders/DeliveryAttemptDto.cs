using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record DeliveryAttemptDto(long Id, byte AttemptNo, DateTime AttemptedAt, DeliveryAttemptResult Result, string? CarrierNote, string? ProofImageUrl);
