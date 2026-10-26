using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class RiskRule : BaseEntity<short>
{
    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public string ExplanationTemplateAr { get; set; } = string.Empty;

    public short Points { get; set; }

    public RuleScope Scope { get; set; }

    public string? ParametersJson { get; set; }

    public bool IsActive { get; set; } = true;

    public int Version { get; set; } = 1;
}
