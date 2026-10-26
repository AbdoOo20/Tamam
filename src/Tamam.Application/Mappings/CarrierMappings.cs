using Tamam.Application.DTOs.Carriers;

using Tamam.Domain.Entities;

namespace Tamam.Application.Mappings;

public static class CarrierMappings
{
    public static CarrierDto ToDto(this Carrier carrier) => new(carrier.Id, carrier.Name, carrier.Phone, carrier.TrackingUrlTemplate, carrier.IsActive, carrier.MerchantId is null);
}
