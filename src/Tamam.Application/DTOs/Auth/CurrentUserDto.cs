namespace Tamam.Application.DTOs.Auth;

public record CurrentUserDto(int? UserId, int? MerchantId, string? Role);
