namespace Tamam.Application.DTOs.Auth;

public record RegisterRequest(string BusinessName, string OwnerName, string Phone, string CountryCode, string Email, string Password);
