using Tamam.Application.Services.Maintenance;

namespace Tamam.Api.BackgroundServices;

public class MaintenanceWorker(IServiceScopeFactory scopeFactory, ILogger<MaintenanceWorker> logger) : BackgroundService
{
    private static readonly TimeSpan StartupDelay = TimeSpan.FromMinutes(1);

    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await DelayAsync(StartupDelay, stoppingToken))
        {
            return;
        }

        do
        {
            await RunOnceAsync(stoppingToken);
        }

        while (await DelayAsync(Interval, stoppingToken));
    }

    private async Task RunOnceAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IMaintenanceService>().RunAsync(stoppingToken);
        }

        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }

        catch (Exception exception)
        {
            logger.LogError(exception, "The maintenance run failed.");
        }
    }

    private static async Task<bool> DelayAsync(TimeSpan delay, CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(delay, stoppingToken);

            return true;
        }

        catch (OperationCanceledException)
        {
            return false;
        }
    }
}
