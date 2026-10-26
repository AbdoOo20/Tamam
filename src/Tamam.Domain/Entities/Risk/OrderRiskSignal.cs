namespace Tamam.Domain.Entities;

public class OrderRiskSignal
{
    public long Id { get; set; }

    public long AssessmentId { get; set; }

    public short RuleId { get; set; }

    public short Points { get; set; }

    public string ExplanationAr { get; set; } = string.Empty;

    public OrderRiskAssessment Assessment { get; set; } = null!;

    public RiskRule Rule { get; set; } = null!;
}
