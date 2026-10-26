using Tamam.Application.Common;

using Tamam.Application.DTOs.Admin;

namespace Tamam.Application.Services.Admin;

public interface IAdminCatalogService
{
    Task<Result<IReadOnlyList<AdminPlanDto>>> GetPlansAsync(CancellationToken cancellationToken = default);

    Task<Result<AdminPlanDto>> CreatePlanAsync(AdminPlanRequest request, CancellationToken cancellationToken = default);

    Task<Result<AdminPlanDto>> UpdatePlanAsync(short id, AdminPlanRequest request, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<AdminRiskRuleDto>>> GetRiskRulesAsync(CancellationToken cancellationToken = default);

    Task<Result<AdminRiskRuleDto>> UpdateRiskRuleAsync(short id, UpdateRiskRuleRequest request, CancellationToken cancellationToken = default);
}
