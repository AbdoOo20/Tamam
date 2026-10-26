using Tamam.Application.DTOs.Orders;

using Tamam.Application.DTOs.Rejections;

using Tamam.Application.DTOs.Risk;

using Tamam.Domain.Entities;

namespace Tamam.Application.Mappings;

public static class OrderMappings
{
    public static OrderItemDto ToDto(this OrderItem item) => new(item.Id, item.ProductId, item.ProductName, item.Variant, item.Quantity, item.UnitPrice, item.LineTotal);

    public static OrderStatusHistoryDto ToDto(this OrderStatusHistory history) => new(history.Id, history.FromStatusId, history.ToStatusId, history.ChangedByType, history.ChangedByUserId, history.Note, history.ChangedAt);

    public static DeliveryAttemptDto ToDto(this DeliveryAttempt attempt) => new(attempt.Id, attempt.AttemptNo, attempt.AttemptedAt, attempt.Result, attempt.CarrierNote, attempt.ProofImageUrl);

    public static OrderOutcomeDto ToDto(this OrderOutcome outcome) => new(
        outcome.FinalResult,

        outcome.FinalReasonId,

        outcome.Attribution,

        outcome.IsDisputed,

        outcome.ResolutionMethod,

        outcome.CustomerFaultWeight,

        outcome.OutboundShippingCost,

        outcome.ReturnShippingCost,

        outcome.OtherLoss,

        outcome.LossAmount,

        outcome.ResolvedAt);

    public static RiskAssessmentDto ToDto(this OrderRiskAssessment assessment, IEnumerable<OrderRiskSignal> signals) => new(
        assessment.Id,

        assessment.Score,

        assessment.Level,

        assessment.RecommendedAction,

        assessment.IsShadow,

        assessment.MerchantDecision,

        assessment.DecidedAt,

        assessment.CreatedAt,

        signals.Select(signal => new RiskSignalDto(signal.RuleId, signal.Points, signal.ExplanationAr)).ToList());
}
