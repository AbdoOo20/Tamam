using Tamam.Application.Common;

using Tamam.Application.DTOs.Lookups;

namespace Tamam.Application.Services.Lookups;

public interface ILookupService
{
    Task<Result<IReadOnlyList<RegionDto>>> GetRegionsAsync(string countryCode, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<OrderStatusDto>>> GetOrderStatusesAsync(CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<RejectionReasonDto>>> GetRejectionReasonsAsync(CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<RiskRuleDto>>> GetRiskRulesAsync(CancellationToken cancellationToken = default);

    Result<IReadOnlyDictionary<string, IReadOnlyList<EnumItemDto>>> GetEnums();
}
