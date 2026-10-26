using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Risk;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Risk;

public class RiskService(IUnitOfWork unitOfWork, IUserAppService userAppService, IRiskScoringService riskScoring) : IRiskService
{
    public async Task<Result<IReadOnlyList<RiskAssessmentDto>>> GetAssessmentsAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (!await unitOfWork.Repository<Order>().AnyAsync(order => order.Id == orderId && order.MerchantId == merchantId, cancellationToken))
        {
            return Result<IReadOnlyList<RiskAssessmentDto>>.NotFound("Order was not found.");
        }

        var assessments = await unitOfWork.Repository<OrderRiskAssessment>().Query()
            .Where(assessment => assessment.OrderId == orderId)
            .OrderByDescending(assessment => assessment.CreatedAt)
            .ThenByDescending(assessment => assessment.Id)
            .ToListAsync(cancellationToken);

        var ids = assessments.Select(assessment => assessment.Id).ToList();

        var signals = await unitOfWork.Repository<OrderRiskSignal>().Query().Where(signal => ids.Contains(signal.AssessmentId)).ToListAsync(cancellationToken);

        IReadOnlyList<RiskAssessmentDto> dtos = assessments.Select(assessment => assessment.ToDto(signals.Where(signal => signal.AssessmentId == assessment.Id))).ToList();

        return Result<IReadOnlyList<RiskAssessmentDto>>.Ok(dtos);
    }

    public async Task<Result<RiskAssessmentDto>> DecideAsync(long orderId, RiskDecisionRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (!await unitOfWork.Repository<Order>().AnyAsync(order => order.Id == orderId && order.MerchantId == merchantId, cancellationToken))
        {
            return Result<RiskAssessmentDto>.NotFound("Order was not found.");
        }

        var assessment = await unitOfWork.Repository<OrderRiskAssessment>().Query(asNoTracking: false)
            .Where(item => item.OrderId == orderId)
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (assessment is null)
        {
            return Result<RiskAssessmentDto>.NotFound("No risk assessment exists for this order.");
        }

        assessment.MerchantDecision = request.Decision;

        assessment.DecidedByUserId = userAppService.UserId;

        assessment.DecidedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var signals = await unitOfWork.Repository<OrderRiskSignal>().ListAsync(signal => signal.AssessmentId == assessment.Id, cancellationToken);

        return Result<RiskAssessmentDto>.Ok(assessment.ToDto(signals));
    }

    public async Task<Result<RiskAssessmentDto>> RecalculateAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == orderId && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<RiskAssessmentDto>.NotFound("Order was not found.");
        }

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        var assessment = await riskScoring.EvaluateAsync(
            new RiskInput(merchantId, order.Id, order.CustomerId, order.MerchantCustomerId, order.RecipientPhoneE164, order.CountryCode, order.AddressLine, order.City, order.RegionId, order.TotalAmount, order.PlacedAt, merchant?.ShadowMode ?? true),

            cancellationToken);

        assessment.Order = order;

        order.LatestRiskScore = assessment.Score;

        order.LatestRiskLevel = assessment.Level;

        await unitOfWork.Repository<OrderRiskAssessment>().AddAsync(assessment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RiskAssessmentDto>.Ok(assessment.ToDto(assessment.Signals), "Risk recalculated.");
    }
}
