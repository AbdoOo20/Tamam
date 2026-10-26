using Tamam.Application.Common;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public class OrderQuery : PagedQuery
{
    public OrderStatusType? StatusId { get; set; }

    public RiskLevel? RiskLevel { get; set; }

    public int? CarrierId { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}
