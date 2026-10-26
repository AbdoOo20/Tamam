using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Reports;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Reports;

public class ReportService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IReportService
{
    public async Task<Result<DashboardDto>> GetDashboardAsync(DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var end = (to ?? DateTime.UtcNow).Date.AddDays(1);

        var start = (from ?? end.AddDays(-30)).Date;

        var currency = await unitOfWork.Repository<Merchant>().Query().Where(merchant => merchant.Id == merchantId).Select(merchant => merchant.CurrencyCode).FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var orders = unitOfWork.Repository<Order>().Query().Where(order => order.MerchantId == merchantId && !order.IsHistorical && order.PlacedAt >= start && order.PlacedAt < end);

        var statusCounts = await orders.GroupBy(order => order.StatusId).Select(group => new { StatusId = group.Key, Count = group.Count() }).ToListAsync(cancellationToken);

        var statusNames = await unitOfWork.Repository<OrderStatus>().Query().ToDictionaryAsync(status => status.Id, status => status.NameAr, cancellationToken);

        var riskCounts = await orders.GroupBy(order => order.LatestRiskLevel).Select(group => new { Level = group.Key, Count = group.Count() }).ToListAsync(cancellationToken);

        var perDay = await orders.GroupBy(order => order.PlacedAt.Date).Select(group => new { Date = group.Key, Count = group.Count() }).OrderBy(item => item.Date).ToListAsync(cancellationToken);

        var revenue = await orders.Where(order => order.StatusId == OrderStatusType.Delivered).SumAsync(order => (decimal?)order.TotalAmount, cancellationToken) ?? 0m;

        var outcomes = unitOfWork.Repository<OrderOutcome>().Query().Where(outcome => outcome.Order.MerchantId == merchantId && !outcome.Order.IsHistorical && outcome.Order.PlacedAt >= start && outcome.Order.PlacedAt < end);

        var attributionCounts = await outcomes
            .Where(outcome => outcome.FinalResult == OrderFinalResult.Refused)
            .GroupBy(outcome => outcome.Attribution)
            .Select(group => new { Attribution = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        var totalLoss = await outcomes.SumAsync(outcome => (decimal?)outcome.LossAmount, cancellationToken) ?? 0m;

        int Count(params OrderStatusType[] statuses) => statusCounts.Where(item => statuses.Contains(item.StatusId)).Sum(item => item.Count);

        var total = statusCounts.Sum(item => item.Count);

        var delivered = Count(OrderStatusType.Delivered);

        var refused = Count(OrderStatusType.Refused, OrderStatusType.Returned);

        var cancelled = Count(OrderStatusType.CancelledByMerchant, OrderStatusType.CancelledByCustomer);

        var completed = delivered + refused;

        var dashboard = new DashboardDto(
            start,

            end.AddTicks(-1),

            currency,

            total,

            delivered,

            refused,

            cancelled,

            total - delivered - refused - cancelled,

            completed == 0 ? 0m : Math.Round(delivered * 100m / completed, 1),

            revenue,

            totalLoss,

            statusCounts.OrderBy(item => (int)item.StatusId).Select(item => new CountByKeyDto(statusNames.GetValueOrDefault(item.StatusId, item.StatusId.ToString()), item.Count)).ToList(),

            riskCounts.Select(item => new CountByKeyDto(item.Level?.ToString() ?? "Unscored", item.Count)).ToList(),

            attributionCounts.Select(item => new CountByKeyDto(item.Attribution?.ToString() ?? "Unresolved", item.Count)).ToList(),

            perDay.Select(item => new DailyCountDto(item.Date, item.Count)).ToList());

        return Result<DashboardDto>.Ok(dashboard);
    }

    public async Task<Result<IReadOnlyList<QualityMonthDto>>> GetQualityAsync(int months, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var safeMonths = Math.Clamp(months, 1, 12);

        var today = DateTime.UtcNow;

        var start = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-(safeMonths - 1));

        var all = unitOfWork.Repository<OrderOutcome>().Query().Where(outcome => outcome.Order.PlacedAt >= start && !outcome.Order.IsHistorical);

        var mine = await Group(all.Where(outcome => outcome.Order.MerchantId == merchantId), cancellationToken);

        var network = await Group(all, cancellationToken);

        IReadOnlyList<QualityMonthDto> result = mine
            .OrderBy(row => row.Year)
            .ThenBy(row => row.Month)
            .Select(row =>
        {
            var networkRow = network.FirstOrDefault(item => item.Year == row.Year && item.Month == row.Month);

            return new QualityMonthDto(
                row.Year,

                row.Month,

                row.Total,

                row.Delivered,

                row.Refused,

                row.RefusedCustomer,

                row.RefusedMerchant,

                row.RefusedCarrier,

                row.RefusedNeutral,

                row.Disputed,

                row.Loss,

                Rate(row.RefusedMerchant, row.Total),

                networkRow is null ? 0m : Rate(networkRow.RefusedMerchant, networkRow.Total));
        })
            .ToList();

        return Result<IReadOnlyList<QualityMonthDto>>.Ok(result);
    }

    private sealed record MonthRow(int Year, int Month, int Total, int Delivered, int Refused, int RefusedCustomer, int RefusedMerchant, int RefusedCarrier, int RefusedNeutral, int Disputed, decimal Loss);

    private static async Task<List<MonthRow>> Group(IQueryable<OrderOutcome> outcomes, CancellationToken cancellationToken)
    {
        var rows = await outcomes
            .GroupBy(outcome => new { outcome.Order.PlacedAt.Year, outcome.Order.PlacedAt.Month })
            .Select(group => new
        {
            group.Key.Year,

            group.Key.Month,

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

        return rows.Select(row => new MonthRow(row.Year, row.Month, row.Total, row.Delivered, row.Refused, row.RefusedCustomer, row.RefusedMerchant, row.RefusedCarrier, row.RefusedNeutral, row.Disputed, row.Loss)).ToList();
    }

    private static decimal Rate(int part, int total) => total == 0 ? 0m : Math.Round(part * 100m / total, 1);
}
