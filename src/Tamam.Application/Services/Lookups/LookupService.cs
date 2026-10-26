using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Lookups;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Lookups;

public class LookupService(IUnitOfWork unitOfWork) : ILookupService
{
    private static readonly Type[] ExposedEnums =

    [

    typeof(OrderStatusType),

    typeof(PaymentMethodType),

    typeof(OrderSource),

    typeof(RiskLevel),

    typeof(RecommendedActionType),

    typeof(MerchantDecisionType),

    typeof(RejectionAttribution),

    typeof(DeliveryAttemptResult),

    typeof(ConfirmationStatus),

    typeof(MessageChannel),

    typeof(MerchantStatus),

    typeof(SubscriptionStatus),

    typeof(ImportKind),

    typeof(UsageType),

    typeof(OrderFinalResult)

    ];

    public async Task<Result<IReadOnlyList<RegionDto>>> GetRegionsAsync(string countryCode, CancellationToken cancellationToken = default)
    {
        var code = countryCode.Trim().ToUpperInvariant();

        IReadOnlyList<RegionDto> regions = await unitOfWork.Repository<Region>().Query()
            .Where(region => region.CountryCode == code && region.IsActive)
            .OrderBy(region => region.NameAr)
            .Select(region => new RegionDto(region.Id, region.CountryCode, region.ParentRegionId, region.NameAr, region.NameEn))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<RegionDto>>.Ok(regions);
    }

    public async Task<Result<IReadOnlyList<OrderStatusDto>>> GetOrderStatusesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<OrderStatusDto> statuses = await unitOfWork.Repository<OrderStatus>().Query()
            .OrderBy(status => status.SortOrder)
            .Select(status => new OrderStatusDto((int)status.Id, status.Code, status.NameAr, status.NameEn, status.IsFinal, status.SortOrder))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<OrderStatusDto>>.Ok(statuses);
    }

    public async Task<Result<IReadOnlyList<RejectionReasonDto>>> GetRejectionReasonsAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<RejectionReasonDto> reasons = await unitOfWork.Repository<RejectionReason>().Query()
            .Where(reason => reason.IsActive)
            .OrderBy(reason => reason.SortOrder)
            .Select(reason => new RejectionReasonDto((int)reason.Id, reason.Code, reason.NameAr, reason.NameEn, reason.Attribution.ToString(), reason.SortOrder))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<RejectionReasonDto>>.Ok(reasons);
    }

    public async Task<Result<IReadOnlyList<RiskRuleDto>>> GetRiskRulesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<RiskRuleDto> rules = await unitOfWork.Repository<RiskRule>().Query()
            .Where(rule => rule.IsActive)
            .OrderBy(rule => rule.Id)
            .Select(rule => new RiskRuleDto(rule.Id, rule.Code, rule.NameAr, rule.ExplanationTemplateAr, rule.Points, rule.Scope.ToString()))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<RiskRuleDto>>.Ok(rules);
    }

    public Result<IReadOnlyDictionary<string, IReadOnlyList<EnumItemDto>>> GetEnums()
    {
        IReadOnlyDictionary<string, IReadOnlyList<EnumItemDto>> enums = ExposedEnums.ToDictionary(
            type => type.Name,

            type => (IReadOnlyList<EnumItemDto>)Enum.GetValues(type).Cast<object>().Select(value => new EnumItemDto(Convert.ToInt32(value), value.ToString()!)).ToList());

        return Result<IReadOnlyDictionary<string, IReadOnlyList<EnumItemDto>>>.Ok(enums);
    }
}
