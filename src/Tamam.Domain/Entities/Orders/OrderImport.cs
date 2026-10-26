using Tamam.Domain.Common;

using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class OrderImport : BaseEntity<long>
{
    public int MerchantId { get; set; }

    public int? UploadedByUserId { get; set; }

    public ImportKind Kind { get; set; }

    public string FileName { get; set; } = string.Empty;

    public int RowsTotal { get; set; }

    public int RowsImported { get; set; }

    public int RowsFailed { get; set; }

    public string? ErrorReport { get; set; }

    public ImportStatus Status { get; set; } = ImportStatus.Processing;

    public DateTime? CompletedAt { get; set; }

    public Merchant Merchant { get; set; } = null!;

    public User? UploadedByUser { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
}
