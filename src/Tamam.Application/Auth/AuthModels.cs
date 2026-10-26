namespace Tamam.Application.Auth;

public record RegisterRequest(string BusinessName, string OwnerName, string Phone, string CountryCode, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string AccessToken, DateTime ExpiresAtUtc, int UserId, int? MerchantId, string FullName, string Role);
