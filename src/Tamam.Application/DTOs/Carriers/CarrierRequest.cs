using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Carriers;

public record CarrierRequest
{
    [Required, MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; init; }

    [MaxLength(300)]
    public string? TrackingUrlTemplate { get; init; }

    public bool IsActive { get; init; } = true;
}
