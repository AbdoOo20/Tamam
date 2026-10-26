using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Billing;

public record ChangePlanRequest
{
    [Required, MaxLength(30)]
    public string PlanCode { get; init; } = string.Empty;
}
