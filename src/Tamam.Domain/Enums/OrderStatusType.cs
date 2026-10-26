namespace Tamam.Domain.Enums;

public enum OrderStatusType : byte
{
    New = 1,

    AwaitingConfirmation = 2,

    Confirmed = 3,

    DepositRequested = 4,

    Shipped = 5,

    OutForDelivery = 6,

    Delivered = 7,

    Refused = 8,

    FailedDelivery = 9,

    Returned = 10,

    CancelledByMerchant = 11,

    CancelledByCustomer = 12,
}
