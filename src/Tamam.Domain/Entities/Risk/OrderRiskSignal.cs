using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class OrderRiskSignal : BaseEntity<long>
{
    public long AssessmentId { get; set; }

    public short RuleId { get; set; }

    public short Points { get; set; }

    public string ExplanationAr { get; set; } = string.Empty;

    public OrderRiskAssessment Assessment { get; set; } = null!;

    public RiskRule Rule { get; set; } = null!;
}
