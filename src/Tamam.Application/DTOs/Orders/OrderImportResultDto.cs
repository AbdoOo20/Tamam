using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Orders;

public record OrderImportResultDto(long ImportId, ImportStatus Status, int RowsTotal, int RowsImported, int RowsFailed, IReadOnlyList<string> Errors);
