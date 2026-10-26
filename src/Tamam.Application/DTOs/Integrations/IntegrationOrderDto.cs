using Tamam.Domain.Enums;

namespace Tamam.Application.DTOs.Integrations;

public record IntegrationOrderDto(
    long OrderId,

    string OrderNo,

    OrderStatusType StatusId,

    string StatusName,

    decimal TotalAmount,

    decimal AmountToCollect,

    byte? RiskScore,

    RiskLevel? RiskLevel,

    RecommendedActionType? RecommendedAction,

    string? ConfirmationLink,

    string? WhatsAppUrl,

    string? MessageBody,

    bool AlreadyExisted);
