namespace Tamam.Application.DTOs.Billing;

public record PlanDto(short Id, string Code, string NameAr, decimal MonthlyFee, decimal PricePerCheckedOrder, int IncludedOrders, string CurrencyCode);
