using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record AddDeliveryAttemptRequest
{
    [EnumDataType(typeof(DeliveryAttemptResult))]
    public DeliveryAttemptResult Result { get; init; }

    [MaxLength(500)]
    public string? CarrierNote { get; init; }

    [Url, MaxLength(500)]
    public string? ProofImageUrl { get; init; }

    public DateTime? AttemptedAt { get; init; }
}
