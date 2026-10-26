using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class AuditLog : BaseEntity<long>
{
    public int? MerchantId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public string? EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? IpAddress { get; set; }

    public Merchant? Merchant { get; set; }

    public User? User { get; set; }
}
