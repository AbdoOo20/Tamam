namespace Tamam.Application.DTOs.Admin;

public record AdminRiskRuleDto(
    short Id,

    string Code,

    string NameAr,

    string ExplanationTemplateAr,

    short Points,

    string Scope,

    string? ParametersJson,

    bool IsActive,

    int Version);
