namespace Tamam.Application.Staff;

public record StaffDto(int Id, string FullName, string Email, string? PhoneNumber, string Role, bool IsActive, DateTime? LastLoginAt);

public record CreateStaffRequest(string FullName, string Email, string? PhoneNumber, string Password);
