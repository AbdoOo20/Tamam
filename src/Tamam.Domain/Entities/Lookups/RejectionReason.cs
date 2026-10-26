using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class RejectionReason
{
    public RejectionReasonType Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public string NameEn { get; set; } = string.Empty;

    public RejectionAttribution Attribution { get; set; }

    public decimal CustomerFaultWeight { get; set; }

    public bool IsActive { get; set; } = true;

    public byte SortOrder { get; set; }
}
