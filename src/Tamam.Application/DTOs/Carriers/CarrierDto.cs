namespace Tamam.Application.DTOs.Carriers;

public record CarrierDto(int Id, string Name, string? Phone, string? TrackingUrlTemplate, bool IsActive, bool IsGlobal);
