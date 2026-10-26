namespace Tamam.Application.Services.Maintenance;

public interface IMaintenanceService
{
    Task RunAsync(CancellationToken cancellationToken = default);
}
