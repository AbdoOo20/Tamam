namespace Tamam.Application.DTOs.Staff;

public record CreateStaffRequest(string FullName, string Email, string? PhoneNumber, string Password);
