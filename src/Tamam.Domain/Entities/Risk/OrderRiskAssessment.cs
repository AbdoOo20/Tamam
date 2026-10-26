using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderRiskAssessment
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public byte Score { get; set; }

    public RiskLevel Level { get; set; }

    public RecommendedActionType RecommendedAction { get; set; } = RecommendedActionType.None;

    public string ModelVersion { get; set; } = string.Empty;

    public bool IsShadow { get; set; } = true;

    public MerchantDecisionType? MerchantDecision { get; set; }

    public int? DecidedByUserId { get; set; }

    public DateTime? DecidedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Order Order { get; set; } = null!;

    public User? DecidedByUser { get; set; }

    public ICollection<OrderRiskSignal> Signals { get; set; } = [];
}
