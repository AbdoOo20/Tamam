using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record OrderImportRequest
{
    [Required, MaxLength(260)]
    public string FileName { get; init; } = string.Empty;

    [EnumDataType(typeof(ImportKind))]
    public ImportKind Kind { get; init; } = ImportKind.Regular;

    [Required, MinLength(1), MaxLength(200)]
    public List<CreateOrderRequest> Orders { get; init; } = [];
}
