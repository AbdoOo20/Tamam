using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Public;

public record PublicConfirmRequest
{
    public DateOnly? ProposedDeliveryDate { get; init; }

    [MaxLength(500)]
    public string? Note { get; init; }
}
