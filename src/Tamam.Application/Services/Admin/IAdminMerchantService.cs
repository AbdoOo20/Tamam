using Tamam.Application.Common;

using Tamam.Application.DTOs.Admin;

using Tamam.Application.DTOs.Billing;

namespace Tamam.Application.Services.Admin;

public interface IAdminMerchantService
{
    Task<Result<AdminOverviewDto>> GetOverviewAsync(CancellationToken cancellationToken = default);

    Task<Result<PagedResult<AdminMerchantListItemDto>>> GetAllAsync(AdminMerchantQuery query, CancellationToken cancellationToken = default);

    Task<Result<AdminMerchantDetailDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<AdminMerchantDetailDto>> UpdateStatusAsync(int id, UpdateMerchantStatusRequest request, CancellationToken cancellationToken = default);

    Task<Result<SubscriptionDto>> ChangePlanAsync(int id, ChangePlanRequest request, CancellationToken cancellationToken = default);

    Task<Result<int>> RecalculateQualityAsync(RecalculateQualityRequest request, CancellationToken cancellationToken = default);
}
