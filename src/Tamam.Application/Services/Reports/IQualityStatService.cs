namespace Tamam.Application.Services.Reports;

public interface IQualityStatService
{
    Task<int> RecalculateAsync(int year, int month, CancellationToken cancellationToken = default);
}
