namespace Tamam.Domain.Enums;

public enum MessageDeliveryStatus : byte
{
    Queued = 1,

    Sent = 2,

    Delivered = 3,

    Read = 4,

    Failed = 5,
}
