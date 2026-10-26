using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class MerchantConsent : BaseEntity<int>
{
    public int MerchantId { get; set; }

    public string TermsVersion { get; set; } = string.Empty;

    public bool ShareOutcomesToNetwork { get; set; }

    public int? AcceptedByUserId { get; set; }

    public string? AcceptedFromIp { get; set; }

    public DateTime AcceptedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public User? AcceptedByUser { get; set; }
}
