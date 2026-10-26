using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Merchants;

public record AcceptConsentRequest
{
    [Required, MaxLength(20)]
    public string TermsVersion { get; init; } = string.Empty;

    public bool ShareOutcomesToNetwork { get; init; }
}
