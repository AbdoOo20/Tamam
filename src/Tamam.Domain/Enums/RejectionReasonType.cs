namespace Tamam.Domain.Enums;

public enum RejectionReasonType : short
{
    ChangedMind = 1,

    NoCash = 2,

    NotAnswering = 3,

    DuplicateOrder = 4,

    FoundCheaper = 5,

    NotAsAdvertised = 6,

    DefectiveDamaged = 7,

    WrongItem = 8,

    PriceMismatch = 9,

    LateDelivery = 10,

    CourierBehavior = 11,

    AddressNotFound = 12,

    DidNotOrder = 13,

    Other = 14,
}
