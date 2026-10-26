using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Admin;

public record UpdateMerchantStatusRequest
{
    [EnumDataType(typeof(MerchantStatus))]
    public MerchantStatus Status { get; init; }
}
