using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Merchants;

public record UpdateMerchantProfileRequest
{
    [Required, MaxLength(150)]
    public string BusinessName { get; init; } = string.Empty;

    [Required, MaxLength(150)]
    public string OwnerName { get; init; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; init; } = string.Empty;

    [EmailAddress, MaxLength(256)]
    public string? Email { get; init; }

    [Required, MaxLength(10)]
    public string DefaultLanguage { get; init; } = "ar-EG";

    [Url, MaxLength(300)]
    public string? StoreUrl { get; init; }

    [MaxLength(100)]
    public string? InstagramHandle { get; init; }

    public bool ShadowMode { get; init; }
}
