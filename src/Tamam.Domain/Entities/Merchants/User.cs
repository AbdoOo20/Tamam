using Microsoft.AspNetCore.Identity;

using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class User : IdentityUser<int>, IAuditable, ISoftDeletable
{
    public int? MerchantId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public Merchant? Merchant { get; set; }
}
