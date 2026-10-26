using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Billing;

public record UsageLineDto(UsageType Type, int Quantity, decimal TotalCost);
