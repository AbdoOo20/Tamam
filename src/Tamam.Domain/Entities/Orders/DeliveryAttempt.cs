using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class DeliveryAttempt : BaseEntity<long>
{
    public long OrderId { get; set; }

    public byte AttemptNo { get; set; }

    public DateTime AttemptedAt { get; set; }

    public DeliveryAttemptResult Result { get; set; }

    public string? CarrierNote { get; set; }

    public string? ProofImageUrl { get; set; }

    public Order Order { get; set; } = null!;
}
