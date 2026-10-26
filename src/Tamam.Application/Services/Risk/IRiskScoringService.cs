using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Risk;

public interface IRiskScoringService
{
    Task<OrderRiskAssessment> EvaluateAsync(RiskInput input, CancellationToken cancellationToken = default);
}
