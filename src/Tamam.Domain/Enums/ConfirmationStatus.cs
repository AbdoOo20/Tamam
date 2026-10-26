namespace Tamam.Domain.Enums;

public enum ConfirmationStatus : byte
{
    Created = 1,

    Sent = 2,

    Delivered = 3,

    Opened = 4,

    Confirmed = 5,

    Cancelled = 6,

    Rescheduled = 7,

    NoResponse = 8,

    Failed = 9,
}
