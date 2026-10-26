namespace Tamam.Application.DTOs.Lookups;

public record RegionDto(int Id, string CountryCode, int? ParentRegionId, string NameAr, string? NameEn);
