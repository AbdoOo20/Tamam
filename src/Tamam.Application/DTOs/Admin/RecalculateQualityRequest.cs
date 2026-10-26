using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Admin;

public record RecalculateQualityRequest
{
    [Range(2020, 2100)]
    public int? Year { get; init; }

    [Range(1, 12)]
    public int? Month { get; init; }
}
