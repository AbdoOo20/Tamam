namespace Tamam.Application.DTOs.Lookups;

public record OrderStatusDto(int Id, string Code, string NameAr, string NameEn, bool IsFinal, int SortOrder);
