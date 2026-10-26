using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Audit;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Audit;

public class AuditLogService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IAuditLogService
{
    public Task<Result<PagedResult<AuditLogDto>>> GetForMerchantAsync(AuditLogQuery query, CancellationToken cancellationToken = default)
    {
        return QueryAsync(query, userAppService.RequiredMerchantId, cancellationToken);
    }

    public Task<Result<PagedResult<AuditLogDto>>> GetForAdminAsync(AuditLogQuery query, CancellationToken cancellationToken = default)
    {
        return QueryAsync(query, query.MerchantId, cancellationToken);
    }

    private async Task<Result<PagedResult<AuditLogDto>>> QueryAsync(AuditLogQuery query, int? merchantId, CancellationToken cancellationToken)
    {
        var logs = unitOfWork.Repository<AuditLog>().Query();

        if (merchantId is { } scopedMerchantId)
        {
            logs = logs.Where(log => log.MerchantId == scopedMerchantId);
        }

        if (query.UserId is { } userId)
        {
            logs = logs.Where(log => log.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(query.EntityName))
        {
            var entityName = query.EntityName.Trim();

            logs = logs.Where(log => log.EntityName == entityName);
        }

        if (!string.IsNullOrWhiteSpace(query.EntityId))
        {
            var entityId = query.EntityId.Trim();

            logs = logs.Where(log => log.EntityId == entityId);
        }

        if (!string.IsNullOrWhiteSpace(query.Action))
        {
            var action = query.Action.Trim();

            logs = logs.Where(log => log.Action == action);
        }

        if (query.From is { } from)
        {
            logs = logs.Where(log => log.CreatedAt >= from);
        }

        if (query.To is { } to)
        {
            var exclusiveEnd = to.Date.AddDays(1);

            logs = logs.Where(log => log.CreatedAt < exclusiveEnd);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();

            logs = logs.Where(log => log.EntityName.Contains(term) || (log.EntityId != null && log.EntityId.Contains(term)));
        }

        var page = await logs
            .OrderByDescending(log => log.CreatedAt)
            .ThenByDescending(log => log.Id)
            .Select(log => new AuditLogDto(
            log.Id,

            log.MerchantId,

            log.UserId,

            log.User != null ? log.User.FullName : null,

            log.Action,

            log.EntityName,

            log.EntityId,

            log.OldValues,

            log.NewValues,

            log.IpAddress,

            log.CreatedAt))
                .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<AuditLogDto>>.Ok(page);
    }
}
