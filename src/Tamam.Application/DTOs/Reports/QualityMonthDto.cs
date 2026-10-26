namespace Tamam.Application.DTOs.Reports;

public record QualityMonthDto(
    int Year,

    int Month,

    int FinalizedOrders,

    int Delivered,

    int Refused,

    int RefusedCustomerFault,

    int RefusedMerchantFault,

    int RefusedCarrierFault,

    int RefusedNeutral,

    int Disputed,

    decimal TotalLoss,

    decimal MerchantFaultRatePercent,

    decimal NetworkMerchantFaultRatePercent);
