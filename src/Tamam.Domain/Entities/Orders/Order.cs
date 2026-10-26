using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class Order
{
    public long Id { get; set; }

    public int MerchantId { get; set; }

    public long MerchantCustomerId { get; set; }

    public long CustomerId { get; set; }

    public string OrderNo { get; set; } = string.Empty;

    public OrderSource Source { get; set; } = OrderSource.EntryPage;

    public long? ImportId { get; set; }

    public bool IsHistorical { get; set; }

    public string RecipientName { get; set; } = string.Empty;

    public string RecipientPhoneE164 { get; set; } = string.Empty;

    public string? AltPhoneE164 { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public int? RegionId { get; set; }

    public string? City { get; set; }

    public string AddressLine { get; set; } = string.Empty;

    public string? Landmark { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public decimal ItemsTotal { get; set; }

    public decimal ShippingFee { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public PaymentMethodType PaymentMethod { get; set; } = PaymentMethodType.Cod;

    public decimal DepositAmount { get; set; }

    public DateTime? DepositPaidAt { get; set; }

    public decimal AmountToCollect { get; private set; }

    public DateOnly? ExpectedDeliveryDate { get; set; }

    public int? CarrierId { get; set; }

    public string? TrackingNo { get; set; }

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public OrderStatusType StatusId { get; set; } = OrderStatusType.New;

    public byte? LatestRiskScore { get; set; }

    public RiskLevel? LatestRiskLevel { get; set; }

    public string? Notes { get; set; }

    public DateTime PlacedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = [];

    public Merchant Merchant { get; set; } = null!;

    public MerchantCustomer MerchantCustomer { get; set; } = null!;

    public Customer Customer { get; set; } = null!;

    public OrderImport? Import { get; set; }

    public Country Country { get; set; } = null!;

    public Region? Region { get; set; }

    public Carrier? Carrier { get; set; }

    public OrderStatus Status { get; set; } = null!;

    public User? CreatedByUser { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];

    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = [];

    public ICollection<DeliveryAttempt> DeliveryAttempts { get; set; } = [];

    public ICollection<OrderPublicLink> PublicLinks { get; set; } = [];

    public ICollection<ConfirmationRequest> ConfirmationRequests { get; set; } = [];

    public ICollection<MessageLog> MessageLogs { get; set; } = [];

    public ICollection<RejectionReport> RejectionReports { get; set; } = [];

    public OrderOutcome? Outcome { get; set; }

    public ICollection<OrderRiskAssessment> RiskAssessments { get; set; } = [];
}
