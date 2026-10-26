using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Admin;

public record UpdateRiskRuleRequest
{
    [Required, MaxLength(150)]
    public string NameAr { get; init; } = string.Empty;

    [Required, MaxLength(300)]
    public string ExplanationTemplateAr { get; init; } = string.Empty;

    [Range(-100, 100)]
    public short Points { get; init; }

    public bool IsActive { get; init; } = true;
}
