using Tamam.Application.Common;

using Tamam.Application.DTOs.Rejections;

namespace Tamam.Application.Services.Rejections;

public interface IRejectionService
{
    Task<Result<RejectionReportDto>> ReportAsync(ReportRejectionRequest request, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<RejectionReportDto>>> GetReportsAsync(long orderId, CancellationToken cancellationToken = default);

    Task<Result<OrderOutcomeDto>> GetOutcomeAsync(long orderId, CancellationToken cancellationToken = default);

    Task<Result<OrderOutcomeDto>> UpdateLossAsync(long orderId, UpdateLossRequest request, CancellationToken cancellationToken = default);
}
