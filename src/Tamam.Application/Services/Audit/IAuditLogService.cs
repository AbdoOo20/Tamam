using Tamam.Application.Common;

using Tamam.Application.DTOs.Audit;

namespace Tamam.Application.Services.Audit;

public interface IAuditLogService
{
    Task<Result<PagedResult<AuditLogDto>>> GetForMerchantAsync(AuditLogQuery query, CancellationToken cancellationToken = default);

    Task<Result<PagedResult<AuditLogDto>>> GetForAdminAsync(AuditLogQuery query, CancellationToken cancellationToken = default);
}
