using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Services.Notifications;

using Tamam.Application.Services.Reports;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Maintenance;

public class MaintenanceService(IUnitOfWork unitOfWork, IQualityStatService qualityStats) : IMaintenanceService
{
    private const int BatchSize = 500;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        await qualityStats.RecalculateAsync(now.Year, now.Month, cancellationToken);

        var previousMonth = now.AddMonths(-1);

        await qualityStats.RecalculateAsync(previousMonth.Year, previousMonth.Month, cancellationToken);

        await ExpireConfirmationsAsync(now, cancellationToken);

        await NotifyEndingTrialsAsync(now, cancellationToken);
    }

    private async Task ExpireConfirmationsAsync(DateTime now, CancellationToken cancellationToken)
    {
        var open = new List<ConfirmationStatus>
        {
            ConfirmationStatus.Created,

            ConfirmationStatus.Sent,

            ConfirmationStatus.Delivered,

            ConfirmationStatus.Opened
        };

        var stale = await unitOfWork.Repository<ConfirmationRequest>().Query(false)
            .Where(request => open.Contains(request.Status)

        && (request.Order.StatusId == OrderStatusType.New || request.Order.StatusId == OrderStatusType.AwaitingConfirmation)

        && !request.Order.PublicLinks.Any(link => link.Purpose == PublicLinkPurpose.OrderConfirmation && link.ExpiresAt >= now))
            .OrderBy(request => request.Id)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        if (stale.Count == 0)
        {
            return;
        }

        foreach (var request in stale)
        {
            request.Status = ConfirmationStatus.NoResponse;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task NotifyEndingTrialsAsync(DateTime now, CancellationToken cancellationToken)
    {
        var soon = now.AddDays(3);

        var trials = await unitOfWork.Repository<MerchantSubscription>().Query()
            .Where(subscription => subscription.Status == SubscriptionStatus.Trial && subscription.TrialEndsAt != null && subscription.TrialEndsAt >= now && subscription.TrialEndsAt <= soon)
            .Select(subscription => new { subscription.MerchantId, subscription.StartedAt })
            .ToListAsync(cancellationToken);

        var added = false;

        foreach (var trial in trials)
        {
            var alreadyNotified = await unitOfWork.Repository<Notification>().AnyAsync(
                notification => notification.MerchantId == trial.MerchantId && notification.Type == NotificationTypes.TrialEnding && notification.CreatedAt >= trial.StartedAt,

                cancellationToken);

            if (alreadyNotified)
            {
                continue;
            }

            await unitOfWork.Repository<Notification>().AddAsync(
                NotificationFactory.ForMerchant(trial.MerchantId, NotificationTypes.TrialEnding, "الفترة التجريبية قاربت على الانتهاء", "اختر باقة مناسبة لتستمر في استخدام تمام بدون انقطاع."),

                cancellationToken);

            added = true;
        }

        if (added)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
