using Tamam.Application.Common;

using Tamam.Application.DTOs.Reports;

namespace Tamam.Application.Services.Reports;

public interface IReportService
{
    Task<Result<DashboardDto>> GetDashboardAsync(DateTime? from, DateTime? to, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<QualityMonthDto>>> GetQualityAsync(int months, CancellationToken cancellationToken = default);
}
