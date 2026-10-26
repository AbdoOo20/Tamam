using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Public;

public record PublicCancelRequest
{
    [EnumDataType(typeof(RejectionReasonType))]
    public RejectionReasonType ReasonId { get; init; }

    [MaxLength(1000)]
    public string? Details { get; init; }
}
