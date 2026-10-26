using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class MessageTemplate : BaseEntity<int>
{
    public string Code { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public MessageChannel Channel { get; set; }

    public MessageCategory Category { get; set; }

    public string Body { get; set; } = string.Empty;

    public string? ProviderTemplateName { get; set; }

    public TemplateApprovalStatus ApprovalStatus { get; set; } = TemplateApprovalStatus.NotRequired;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
}
