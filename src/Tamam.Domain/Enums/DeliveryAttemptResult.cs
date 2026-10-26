namespace Tamam.Domain.Enums;

public enum DeliveryAttemptResult : byte
{
    Delivered = 1,

    Refused = 2,

    NoAnswer = 3,

    AddressNotFound = 4,

    Postponed = 5,

    Other = 6,
}
