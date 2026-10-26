namespace Tamam.Application.DTOs.Customers;

public record CustomerDto(
    long Id,

    string FullName,

    string PhoneE164,

    string? AltPhoneE164,

    bool? HasWhatsApp,

    string? Notes,

    int TotalOrders,

    int DeliveredOrders,

    int RefusedOrders,

    DateTime? LastOrderAt,

    byte? NetworkReliabilityScore);
