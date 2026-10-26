using Tamam.Domain.Enums;

namespace Tamam.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public int? MerchantId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Staff;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Merchant? Merchant { get; set; }
}
