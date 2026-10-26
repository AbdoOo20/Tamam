namespace Tamam.Application.DTOs.Admin;

public record AdminOverviewDto(
    int Merchants,

    int ActiveMerchants,

    int TrialMerchants,

    int SuspendedMerchants,

    int TotalOrders,

    int OrdersLast30Days,

    int DeliveredLast30Days,

    int RefusedLast30Days,

    decimal RefusalRateLast30DaysPercent,

    int OpenDataRequests);
