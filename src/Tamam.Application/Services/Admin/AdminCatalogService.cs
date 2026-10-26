using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Admin;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Admin;

public class AdminCatalogService(IUnitOfWork unitOfWork) : IAdminCatalogService
{
    public async Task<Result<IReadOnlyList<AdminPlanDto>>> GetPlansAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<AdminPlanDto> plans = await unitOfWork.Repository<Plan>().Query()
            .OrderBy(plan => plan.MonthlyFee)
            .ThenBy(plan => plan.Id)
            .Select(plan => new AdminPlanDto(plan.Id, plan.Code, plan.NameAr, plan.MonthlyFee, plan.PricePerCheckedOrder, plan.IncludedOrders, plan.CurrencyCode, plan.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<AdminPlanDto>>.Ok(plans);
    }

    public async Task<Result<AdminPlanDto>> CreatePlanAsync(AdminPlanRequest request, CancellationToken cancellationToken = default)
    {
        var code = request.Code.Trim().ToUpperInvariant();

        if (await unitOfWork.Repository<Plan>().AnyAsync(plan => plan.Code == code, cancellationToken))
        {
            return Result<AdminPlanDto>.Conflict($"Plan code '{code}' already exists.");
        }

        var entity = new Plan
        {
            Code = code,

            NameAr = request.NameAr.Trim(),

            MonthlyFee = request.MonthlyFee,

            PricePerCheckedOrder = request.PricePerCheckedOrder,

            IncludedOrders = request.IncludedOrders,

            CurrencyCode = request.CurrencyCode.Trim().ToUpperInvariant(),

            IsActive = request.IsActive
        };

        await unitOfWork.Repository<Plan>().AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AdminPlanDto>.Created(ToPlanDto(entity), "Plan created.");
    }

    public async Task<Result<AdminPlanDto>> UpdatePlanAsync(short id, AdminPlanRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Repository<Plan>().FirstOrDefaultAsync(plan => plan.Id == id, cancellationToken);

        if (entity is null)
        {
            return Result<AdminPlanDto>.NotFound("Plan was not found.");
        }

        if (!string.Equals(entity.Code, request.Code.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return Result<AdminPlanDto>.BadRequest("Plan code cannot be changed.");
        }

        entity.NameAr = request.NameAr.Trim();

        entity.MonthlyFee = request.MonthlyFee;

        entity.PricePerCheckedOrder = request.PricePerCheckedOrder;

        entity.IncludedOrders = request.IncludedOrders;

        entity.CurrencyCode = request.CurrencyCode.Trim().ToUpperInvariant();

        entity.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AdminPlanDto>.Ok(ToPlanDto(entity), "Plan updated.");
    }

    public async Task<Result<IReadOnlyList<AdminRiskRuleDto>>> GetRiskRulesAsync(CancellationToken cancellationToken = default)
    {
        var rules = await unitOfWork.Repository<RiskRule>().Query().OrderBy(rule => rule.Id).ToListAsync(cancellationToken);

        IReadOnlyList<AdminRiskRuleDto> result = rules.Select(rule => ToRuleDto(rule)).ToList();

        return Result<IReadOnlyList<AdminRiskRuleDto>>.Ok(result);
    }

    public async Task<Result<AdminRiskRuleDto>> UpdateRiskRuleAsync(short id, UpdateRiskRuleRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Repository<RiskRule>().FirstOrDefaultAsync(rule => rule.Id == id, cancellationToken);

        if (entity is null)
        {
            return Result<AdminRiskRuleDto>.NotFound("Risk rule was not found.");
        }

        entity.NameAr = request.NameAr.Trim();

        entity.ExplanationTemplateAr = request.ExplanationTemplateAr.Trim();

        entity.Points = request.Points;

        entity.IsActive = request.IsActive;

        entity.Version += 1;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AdminRiskRuleDto>.Ok(ToRuleDto(entity), "Risk rule updated.");
    }

    private static AdminPlanDto ToPlanDto(Plan plan) => new(plan.Id, plan.Code, plan.NameAr, plan.MonthlyFee, plan.PricePerCheckedOrder, plan.IncludedOrders, plan.CurrencyCode, plan.IsActive);

    private static AdminRiskRuleDto ToRuleDto(RiskRule rule) => new(rule.Id, rule.Code, rule.NameAr, rule.ExplanationTemplateAr, rule.Points, rule.Scope.ToString(), rule.ParametersJson, rule.IsActive, rule.Version);
}
