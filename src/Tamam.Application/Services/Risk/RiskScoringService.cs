using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Risk;

public class RiskScoringService(IUnitOfWork unitOfWork) : IRiskScoringService
{
    private const string ModelVersion = "rules-v1";

    private static readonly Dictionary<string, int> UtcOffsetHours = new()
    {
        ["EG"] = 2,

        ["SA"] = 3,

        ["AE"] = 4,

        ["KW"] = 3,

        ["QA"] = 3,

        ["BH"] = 3,

        ["OM"] = 4,

        ["JO"] = 3,

        ["PK"] = 5,

        ["IN"] = 5,

        ["NG"] = 1
    };

    public async Task<OrderRiskAssessment> EvaluateAsync(RiskInput input, CancellationToken cancellationToken = default)
    {
        var rules = (await unitOfWork.Repository<RiskRule>().ListAsync(rule => rule.IsActive, cancellationToken)).ToDictionary(rule => rule.Code);

        var triggered = new List<RiskRule>();

        void Trigger(string code, bool condition)
        {
            if (condition && rules.TryGetValue(code, out var rule))
            {
                triggered.Add(rule);
            }
        }

        var merchantOrders = unitOfWork.Repository<Order>().Query().Where(order => order.MerchantId == input.MerchantId && order.Id != input.CurrentOrderId);

        var since = input.PlacedAt.AddHours(-24);

        var duplicate = await merchantOrders.AnyAsync(
            order => order.RecipientPhoneE164 == input.RecipientPhoneE164

            && order.PlacedAt >= since

            && order.StatusId != OrderStatusType.CancelledByMerchant

            && order.StatusId != OrderStatusType.CancelledByCustomer,

            cancellationToken);

        var recentTotals = await merchantOrders.OrderByDescending(order => order.PlacedAt).Take(50).Select(order => order.TotalAmount).ToListAsync(cancellationToken);

        var previousOrders = 0;

        var delivered = 0;

        var refused = 0;

        var noResponse = false;

        var confirmedQuickly = false;

        var noWhatsApp = false;

        if (input.MerchantCustomerId is { } merchantCustomerId)
        {
            var history = merchantOrders.Where(order => order.MerchantCustomerId == merchantCustomerId);

            previousOrders = await history.CountAsync(cancellationToken);

            delivered = await history.CountAsync(order => order.StatusId == OrderStatusType.Delivered, cancellationToken);

            refused = await history.CountAsync(order => order.StatusId == OrderStatusType.Refused, cancellationToken);

            noResponse = await unitOfWork.Repository<ConfirmationRequest>().AnyAsync(request => request.Order.MerchantCustomerId == merchantCustomerId && request.Status == ConfirmationStatus.NoResponse, cancellationToken);

            var confirmations = await unitOfWork.Repository<ConfirmationRequest>().Query()
                .Where(request => request.Order.MerchantCustomerId == merchantCustomerId && request.Status == ConfirmationStatus.Confirmed && request.SentAt != null && request.RespondedAt != null)
                .OrderByDescending(request => request.CreatedAt)
                .Take(5)
                .Select(request => new { request.SentAt, request.RespondedAt })
                .ToListAsync(cancellationToken);

            confirmedQuickly = confirmations.Count > 0 && confirmations.All(item => (item.RespondedAt!.Value - item.SentAt!.Value).TotalMinutes <= 15);

            noWhatsApp = await unitOfWork.Repository<MerchantCustomer>().AnyAsync(customer => customer.Id == merchantCustomerId && customer.HasWhatsApp == false, cancellationToken);
        }

        var networkRefusals = false;

        if (input.CustomerId is { } customerId && await unitOfWork.Repository<MerchantConsent>().AnyAsync(consent => consent.MerchantId == input.MerchantId && consent.ShareOutcomesToNetwork, cancellationToken))
        {
            var profile = await unitOfWork.Repository<CustomerRiskProfile>().GetByIdAsync(customerId, cancellationToken);

            networkRefusals = profile is { CustomerFaultRefusals: > 0 };
        }

        var localHour = input.PlacedAt.AddHours(UtcOffsetHours.GetValueOrDefault(input.CountryCode, 0)).Hour;

        Trigger("INVALID_PHONE", PhoneHelper.LooksSuspicious(input.RecipientPhoneE164));

        Trigger("DUPLICATE_ORDER_24H", duplicate);

        Trigger("ADDRESS_INCOMPLETE", input.AddressLine.Trim().Length < 15 || (string.IsNullOrWhiteSpace(input.City) && input.RegionId is null));

        Trigger("LATE_NIGHT_ORDER", localHour < 6);

        Trigger("HIGH_VALUE_ORDER", recentTotals.Count >= 5 && input.TotalAmount > recentTotals.Average() * 2);

        Trigger("FIRST_TIME_CUSTOMER", previousOrders == 0);

        Trigger("NO_CONFIRMATION_RESPONSE", noResponse);

        Trigger("NO_WHATSAPP", noWhatsApp);

        Trigger("CONFIRMED_QUICKLY", confirmedQuickly);

        Trigger("GOOD_HISTORY", delivered >= 3 && refused == 0);

        Trigger("NETWORK_PRIOR_REFUSALS", networkRefusals);

        var score = (byte)Math.Clamp(triggered.Sum(rule => rule.Points), 0, 100);

        var assessment = new OrderRiskAssessment
        {
            Score = score,

            Level = score < 30 ? RiskLevel.Low : score < 60 ? RiskLevel.Medium : RiskLevel.High,

            RecommendedAction = score < 30 ? RecommendedActionType.None : score < 60 ? RecommendedActionType.ConfirmOnly : score < 80 ? RecommendedActionType.RequestDeposit : RecommendedActionType.ManualReview,

            ModelVersion = ModelVersion,

            IsShadow = input.ShadowMode
        };

        foreach (var rule in triggered)
        {
            assessment.Signals.Add(new OrderRiskSignal
            {
                Assessment = assessment,

                RuleId = rule.Id,

                Points = rule.Points,

                ExplanationAr = rule.ExplanationTemplateAr
            });
        }

        return assessment;
    }
}
