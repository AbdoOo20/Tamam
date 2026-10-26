namespace Tamam.Application.DTOs.Merchants;

public record ConsentDto(int Id, string TermsVersion, bool ShareOutcomesToNetwork, DateTime AcceptedAt);
