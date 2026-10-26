namespace Tamam.Domain.Entities;

public class CustomerRiskProfile
{
    public long CustomerId { get; set; }

    public int TotalOrders { get; set; }

    public int DeliveredCount { get; set; }

    public int CustomerFaultRefusals { get; set; }

    public int NoResponseCount { get; set; }

    public decimal WeightedFaultScore { get; set; }

    public int DistinctMerchants { get; set; }

    public byte? ReliabilityScore { get; set; }

    public DateTime? LastOrderAt { get; set; }

    public DateTime? LastCustomerFaultRefusalAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Customer Customer { get; set; } = null!;
}
