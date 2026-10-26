namespace Tamam.Application.DTOs.Reports;

public record DashboardDto(
    DateTime From,

    DateTime To,

    string CurrencyCode,

    int TotalOrders,

    int DeliveredOrders,

    int RefusedOrders,

    int CancelledOrders,

    int InProgressOrders,

    decimal DeliveryRatePercent,

    decimal DeliveredRevenue,

    decimal TotalLoss,

    IReadOnlyList<CountByKeyDto> ByStatus,

    IReadOnlyList<CountByKeyDto> RiskDistribution,

    IReadOnlyList<CountByKeyDto> RefusalsByAttribution,

    IReadOnlyList<DailyCountDto> OrdersPerDay);
