using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class MerchantSubscription
{
    public int Id { get; set; }

    public int MerchantId { get; set; }

    public short PlanId { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;

    public DateTime StartedAt { get; set; }

    public DateTime? TrialEndsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public Plan Plan { get; set; } = null!;
}
