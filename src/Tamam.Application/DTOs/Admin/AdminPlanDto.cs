namespace Tamam.Application.DTOs.Admin;

public record AdminPlanDto(
    short Id,

    string Code,

    string NameAr,

    decimal MonthlyFee,

    decimal PricePerCheckedOrder,

    int IncludedOrders,

    string CurrencyCode,

    bool IsActive);
