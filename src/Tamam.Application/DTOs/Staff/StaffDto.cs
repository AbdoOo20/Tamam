namespace Tamam.Application.DTOs.Staff;

public record StaffDto(int Id, string FullName, string Email, string? PhoneNumber, string Role, bool IsActive, DateTime? LastLoginAt);
