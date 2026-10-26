using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Admin;

public record AdminPlanRequest
{
    [Required, MaxLength(30)]
    public string Code { get; init; } = string.Empty;

    [Required, MaxLength(100)]
    public string NameAr { get; init; } = string.Empty;

    [Range(0, 100000000)]
    public decimal MonthlyFee { get; init; }

    [Range(0, 100000000)]
    public decimal PricePerCheckedOrder { get; init; }

    [Range(0, 100000000)]
    public int IncludedOrders { get; init; }

    [Required, StringLength(3, MinimumLength = 3)]
    public string CurrencyCode { get; init; } = "USD";

    public bool IsActive { get; init; } = true;
}
