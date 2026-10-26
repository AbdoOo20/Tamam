using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Billing;

public record SubscriptionDto(
    int Id,

    PlanDto Plan,

    SubscriptionStatus Status,

    DateTime StartedAt,

    DateTime? TrialEndsAt,

    DateTime? EndsAt,

    int UsedOrders,

    int RemainingIncludedOrders);
