using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class ConfirmationRequest
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public byte AttemptNo { get; set; } = 1;

    public MessageChannel Channel { get; set; }

    public int? TemplateId { get; set; }

    public ConfirmationStatus Status { get; set; } = ConfirmationStatus.Created;

    public DateTime? SentAt { get; set; }

    public DateTime? RespondedAt { get; set; }

    public DateOnly? ProposedDeliveryDate { get; set; }

    public string? ResponseNote { get; set; }

    public decimal Cost { get; set; }

    public DateTime CreatedAt { get; set; }

    public Order Order { get; set; } = null!;

    public MessageTemplate? Template { get; set; }

    public ICollection<MessageLog> MessageLogs { get; set; } = [];
}
