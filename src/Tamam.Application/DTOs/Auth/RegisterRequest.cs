using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Auth;

public record RegisterRequest
{
    [Required, MaxLength(150)]
    public string BusinessName { get; init; } = string.Empty;

    [Required, MaxLength(150)]
    public string OwnerName { get; init; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; init; } = string.Empty;

    [Required, StringLength(2, MinimumLength = 2)]
    public string CountryCode { get; init; } = string.Empty;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; init; } = string.Empty;
}
