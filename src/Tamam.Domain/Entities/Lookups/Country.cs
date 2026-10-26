using Tamam.Domain.Common;

namespace Tamam.Domain.Entities;

public class Country : BaseEntity<string>
{
    public string NameAr { get; set; } = string.Empty;

    public string NameEn { get; set; } = string.Empty;

    public string PhoneCode { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
