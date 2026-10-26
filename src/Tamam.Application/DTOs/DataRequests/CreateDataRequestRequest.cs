using System.ComponentModel.DataAnnotations;

using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.DataRequests;

public record CreateDataRequestRequest
{
    [EnumDataType(typeof(DataRequestType))]
    public DataRequestType Type { get; init; }
}
