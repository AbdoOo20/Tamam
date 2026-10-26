using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Rejections;

public record OrderOutcomeDto(
    OrderFinalResult FinalResult,

    RejectionReasonType? FinalReasonId,

    RejectionAttribution? Attribution,

    bool IsDisputed,

    ResolutionMethodType? ResolutionMethod,

    decimal CustomerFaultWeight,

    decimal OutboundShippingCost,

    decimal ReturnShippingCost,

    decimal OtherLoss,

    decimal LossAmount,

    DateTime? ResolvedAt);
