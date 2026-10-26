using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderPublicLink : BaseEntity<long>
{
    public long OrderId { get; set; }

    public PublicLinkPurpose Purpose { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public DateTime? FirstOpenedAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public int OpenCount { get; set; }

    public Order Order { get; set; } = null!;
}
