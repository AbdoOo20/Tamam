using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Reports;

public class QualityStatService(IUnitOfWork unitOfWork) : IQualityStatService
{
    public async Task<int> RecalculateAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);

        var end = start.AddMonths(1);

        var period = new DateOnly(year, month, 1);

        var rows = await unitOfWork.Repository<OrderOutcome>().Query()
            .Where(outcome => !outcome.Order.IsHistorical && outcome.Order.PlacedAt >= start && outcome.Order.PlacedAt < end)
            .GroupBy(outcome => outcome.Order.MerchantId)
            .Select(group => new
        {
            MerchantId = group.Key,

            Total = group.Count(),

            Delivered = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Delivered),

            Refused = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Refused),

            RefusedCustomer = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Refused && outcome.Attribution == RejectionAttribution.Customer),

            RefusedMerchant = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Refused && outcome.Attribution == RejectionAttribution.Merchant),

            RefusedCarrier = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Refused && outcome.Attribution == RejectionAttribution.Carrier),

            RefusedNeutral = group.Count(outcome => outcome.FinalResult == OrderFinalResult.Refused && outcome.Attribution == RejectionAttribution.Neutral),

            Disputed = group.Count(outcome => outcome.IsDisputed),

            Loss = group.Sum(outcome => outcome.LossAmount)
        })
            .ToListAsync(cancellationToken);

        if (rows.Count == 0)
        {
            return 0;
        }

        var networkRate = Rate(rows.Sum(row => row.RefusedMerchant), rows.Sum(row => row.Total));

        var existing = await unitOfWork.Repository<MerchantQualityStat>().Query(false)
            .Where(stat => stat.PeriodMonth == period)
            .ToListAsync(cancellationToken);

        var byMerchant = existing.ToDictionary(stat => stat.MerchantId);

        var now = DateTime.UtcNow;

        foreach (var row in rows)
        {
            if (!byMerchant.TryGetValue(row.MerchantId, out var stat))
            {
                stat = new MerchantQualityStat { MerchantId = row.MerchantId, PeriodMonth = period };

                await unitOfWork.Repository<MerchantQualityStat>().AddAsync(stat, cancellationToken);
            }

            stat.TotalOrders = row.Total;

            stat.DeliveredCount = row.Delivered;

            stat.RefusedTotal = row.Refused;

            stat.RefusedCustomerFault = row.RefusedCustomer;

            stat.RefusedMerchantFault = row.RefusedMerchant;

            stat.RefusedCarrierFault = row.RefusedCarrier;

            stat.RefusedNeutral = row.RefusedNeutral;

            stat.DisputedCount = row.Disputed;

            stat.TotalLoss = row.Loss;

            stat.QualityComplaintRate = Rate(row.RefusedMerchant, row.Total);

            stat.NetworkAvgComplaintRate = networkRate;

            stat.CalculatedAt = now;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return rows.Count;
    }

    private static decimal Rate(int part, int total) => total == 0 ? 0m : Math.Round(part * 100m / total, 2);
}
