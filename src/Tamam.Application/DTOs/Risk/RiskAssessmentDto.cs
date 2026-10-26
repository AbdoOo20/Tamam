using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Risk;

public record RiskAssessmentDto(
    long Id,

    byte Score,

    RiskLevel Level,

    RecommendedActionType RecommendedAction,

    bool IsShadow,

    MerchantDecisionType? MerchantDecision,

    DateTime? DecidedAt,

    DateTime CreatedAt,

    IReadOnlyList<RiskSignalDto> Signals);
