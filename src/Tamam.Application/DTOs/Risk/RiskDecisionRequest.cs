using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Risk;

public record RiskDecisionRequest
{
    [EnumDataType(typeof(MerchantDecisionType))]
    public MerchantDecisionType Decision { get; init; }
}
