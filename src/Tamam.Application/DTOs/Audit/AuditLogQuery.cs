using System.ComponentModel.DataAnnotations;

using Tamam.Application.Common;

namespace Tamam.Application.DTOs.Audit;

public class AuditLogQuery : PagedQuery
{
    public int? MerchantId { get; set; }

    public int? UserId { get; set; }

    [MaxLength(100)]
    public string? EntityName { get; set; }

    [MaxLength(50)]
    public string? EntityId { get; set; }

    [MaxLength(50)]
    public string? Action { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}
