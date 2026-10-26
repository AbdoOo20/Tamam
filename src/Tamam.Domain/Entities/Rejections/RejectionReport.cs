using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class RejectionReport
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public ReporterKind ReporterType { get; set; }

    public int? ReportedByUserId { get; set; }

    public RejectionReasonType ReasonId { get; set; }

    public string? Details { get; set; }

    public string? EvidenceUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public Order Order { get; set; } = null!;

    public User? ReportedByUser { get; set; }

    public RejectionReason Reason { get; set; } = null!;
}
