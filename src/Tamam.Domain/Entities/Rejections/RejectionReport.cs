using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class RejectionReport : BaseEntity<long>
{
    public long OrderId { get; set; }

    public ReporterKind ReporterType { get; set; }

    public int? ReportedByUserId { get; set; }

    public RejectionReasonType ReasonId { get; set; }

    public string? Details { get; set; }

    public string? EvidenceUrl { get; set; }

    public Order Order { get; set; } = null!;

    public User? ReportedByUser { get; set; }

    public RejectionReason Reason { get; set; } = null!;
}
