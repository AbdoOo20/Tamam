namespace Tamam.Application.DTOs.Billing;

public record UsageSummaryDto(DateTime From, DateTime To, IReadOnlyList<UsageLineDto> Lines, decimal TotalCost);
