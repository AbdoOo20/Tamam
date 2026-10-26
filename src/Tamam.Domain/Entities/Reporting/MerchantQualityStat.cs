using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class MerchantQualityStat : BaseEntity<long>
{
    public int MerchantId { get; set; }

    public DateOnly PeriodMonth { get; set; }

    public int TotalOrders { get; set; }

    public int DeliveredCount { get; set; }

    public int RefusedTotal { get; set; }

    public int RefusedCustomerFault { get; set; }

    public int RefusedMerchantFault { get; set; }

    public int RefusedCarrierFault { get; set; }

    public int RefusedNeutral { get; set; }

    public int DisputedCount { get; set; }

    public decimal TotalLoss { get; set; }

    public decimal? QualityComplaintRate { get; set; }

    public decimal? NetworkAvgComplaintRate { get; set; }

    public DateTime CalculatedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;
}
