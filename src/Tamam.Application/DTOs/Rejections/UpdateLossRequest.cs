using System.ComponentModel.DataAnnotations;

namespace Tamam.Application.DTOs.Rejections;

public record UpdateLossRequest
{
    [Range(0, 100000000)]
    public decimal OutboundShippingCost { get; init; }

    [Range(0, 100000000)]
    public decimal ReturnShippingCost { get; init; }

    [Range(0, 100000000)]
    public decimal OtherLoss { get; init; }
}
