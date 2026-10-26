namespace Tamam.Application.DTOs.Auth;

public record AuthResponse(string AccessToken, DateTime ExpiresAtUtc, int UserId, int? MerchantId, string FullName, string Role);
