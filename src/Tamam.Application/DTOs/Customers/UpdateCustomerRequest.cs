using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Customers;

public record UpdateCustomerRequest
{
    [Required, MaxLength(150)]
    public string FullName { get; init; } = string.Empty;

    [MaxLength(20)]
    public string? AltPhone { get; init; }

    public bool? HasWhatsApp { get; init; }

    [MaxLength(500)]
    public string? Notes { get; init; }
}
