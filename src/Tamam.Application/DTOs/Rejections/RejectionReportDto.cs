using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Rejections;

public record RejectionReportDto(
    long Id,

    long OrderId,

    ReporterKind ReporterType,

    RejectionReasonType ReasonId,

    string ReasonName,

    RejectionAttribution Attribution,

    string? Details,

    string? EvidenceUrl,

    DateTime CreatedAt);
