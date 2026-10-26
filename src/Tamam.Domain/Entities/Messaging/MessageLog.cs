using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class MessageLog : BaseEntity<long>
{
    public long? OrderId { get; set; }

    public long? ConfirmationRequestId { get; set; }

    public MessageDirection Direction { get; set; }

    public MessageChannel Channel { get; set; }

    public string? ToPhoneE164 { get; set; }

    public string? ProviderMessageId { get; set; }

    public string? Body { get; set; }

    public MessageDeliveryStatus DeliveryStatus { get; set; } = MessageDeliveryStatus.Queued;

    public string? ErrorCode { get; set; }

    public decimal Cost { get; set; }

    public Order? Order { get; set; }

    public ConfirmationRequest? ConfirmationRequest { get; set; }
}
