using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderOutcome : BaseEntity<long>
{
    public OrderFinalResult FinalResult { get; set; }

    public RejectionReasonType? FinalReasonId { get; set; }

    public RejectionAttribution? Attribution { get; set; }

    public bool IsDisputed { get; set; }

    public ResolutionMethodType? ResolutionMethod { get; set; }

    public decimal CustomerFaultWeight { get; set; }

    public decimal OutboundShippingCost { get; set; }

    public decimal ReturnShippingCost { get; set; }

    public decimal OtherLoss { get; set; }

    public decimal LossAmount { get; private set; }

    public DateTime? ResolvedAt { get; set; }

    public Order Order { get; set; } = null!;

    public RejectionReason? FinalReason { get; set; }
}
