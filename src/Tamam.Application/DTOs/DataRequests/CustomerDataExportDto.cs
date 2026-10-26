namespace Tamam.Application.DTOs.DataRequests;

public record CustomerDataExportDto(
    long CustomerId,

    DateTime FirstSeenAt,

    bool IsOptedOut,

    IReadOnlyList<CustomerDataExportOrderDto> Orders);

public record CustomerDataExportOrderDto(
    string MerchantName,

    string OrderNo,

    DateTime PlacedAt,

    string RecipientName,

    string AddressLine,

    string CurrencyCode,

    decimal TotalAmount,

    string Status);
