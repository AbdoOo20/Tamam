using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Rejections;

public record ReportRejectionRequest
{
    [Range(1, long.MaxValue)]
    public long OrderId { get; init; }

    [EnumDataType(typeof(RejectionReasonType))]
    public RejectionReasonType ReasonId { get; init; }

    [MaxLength(1000)]
    public string? Details { get; init; }

    [Url, MaxLength(500)]
    public string? EvidenceUrl { get; init; }
}
