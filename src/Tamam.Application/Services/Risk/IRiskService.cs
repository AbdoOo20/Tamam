using Tamam.Application.Common;

using Tamam.Application.DTOs.Risk;

namespace Tamam.Application.Services.Risk;

public interface IRiskService
{
    Task<Result<IReadOnlyList<RiskAssessmentDto>>> GetAssessmentsAsync(long orderId, CancellationToken cancellationToken = default);

    Task<Result<RiskAssessmentDto>> DecideAsync(long orderId, RiskDecisionRequest request, CancellationToken cancellationToken = default);

    Task<Result<RiskAssessmentDto>> RecalculateAsync(long orderId, CancellationToken cancellationToken = default);
}
