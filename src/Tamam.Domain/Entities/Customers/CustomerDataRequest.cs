using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class CustomerDataRequest
{
    public int Id { get; set; }

    public long? CustomerId { get; set; }

    public DataRequestType Type { get; set; }

    public DataRequestStatus Status { get; set; } = DataRequestStatus.Open;

    public DateTime RequestedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public int? ProcessedByUserId { get; set; }

    public Customer? Customer { get; set; }

    public User? ProcessedByUser { get; set; }
}
