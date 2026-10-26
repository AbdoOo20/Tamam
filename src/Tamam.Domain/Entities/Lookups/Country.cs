namespace Tamam.Domain.Entities;

public class Country
{
    public string Code { get; set; } = string.Empty;

    public string NameAr { get; set; } = string.Empty;

    public string NameEn { get; set; } = string.Empty;

    public string PhoneCode { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
