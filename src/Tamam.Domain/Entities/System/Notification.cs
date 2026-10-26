namespace Tamam.Domain.Entities;

public class Notification
{
    public long Id { get; set; }

    public int MerchantId { get; set; }

    public int? UserId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Body { get; set; }

    public long? RelatedOrderId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public User? User { get; set; }

    public Order? RelatedOrder { get; set; }
}
