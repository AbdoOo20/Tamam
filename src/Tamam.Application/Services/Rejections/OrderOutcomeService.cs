using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Rejections;

public class OrderOutcomeService(IUnitOfWork unitOfWork) : IOrderOutcomeService
{
    public async Task ApplyFinalResultAsync(Order order, OrderFinalResult result, CancellationToken cancellationToken = default)
    {
        var outcomes = unitOfWork.Repository<OrderOutcome>();

        var outcome = await outcomes.GetByIdAsync(order.Id, cancellationToken);

        var changed = outcome is null || outcome.FinalResult != result;

        if (outcome is null)
        {
            outcome = new OrderOutcome { Id = order.Id };

            await outcomes.AddAsync(outcome, cancellationToken);
        }

        outcome.FinalResult = result;

        if (result is OrderFinalResult.Delivered or OrderFinalResult.Cancelled)
        {
            outcome.ResolvedAt ??= DateTime.UtcNow;
        }

        if (result == OrderFinalResult.Refused && outcome.OutboundShippingCost == 0)
        {
            outcome.OutboundShippingCost = order.ShippingFee;
        }

        if (changed && result is OrderFinalResult.Delivered or OrderFinalResult.Refused)
        {
            var profile = await GetProfileAsync(order.CustomerId, cancellationToken);

            profile.TotalOrders++;

            profile.LastOrderAt = DateTime.UtcNow;

            if (result == OrderFinalResult.Delivered)
            {
                profile.DeliveredCount++;
            }

            Recompute(profile);
        }
    }

    public async Task ResolveRefusalAsync(Order order, CancellationToken cancellationToken = default)
    {
        var outcome = await unitOfWork.Repository<OrderOutcome>().GetByIdAsync(order.Id, cancellationToken);

        if (outcome is null || outcome.FinalResult != OrderFinalResult.Refused)
        {
            return;
        }

        var reports = await unitOfWork.Repository<RejectionReport>().Query()
            .Where(report => report.OrderId == order.Id)
            .Select(report => new { report.ReporterType, report.ReasonId, report.Reason.Attribution, report.Reason.CustomerFaultWeight, report.CreatedAt })
            .ToListAsync(cancellationToken);

        if (reports.Count == 0)
        {
            return;
        }

        var wasCustomerFault = outcome.ResolvedAt is not null && outcome.Attribution == RejectionAttribution.Customer;

        var previousWeight = wasCustomerFault ? outcome.CustomerFaultWeight : 0m;

        var latest = reports.OrderByDescending(report => report.CreatedAt).First();

        var attributions = reports.Select(report => report.Attribution).Distinct().ToList();

        outcome.FinalReasonId = latest.ReasonId;

        if (attributions.Count == 1)
        {
            outcome.Attribution = attributions[0];

            outcome.IsDisputed = false;

            outcome.ResolutionMethod = reports.Select(report => report.ReporterType).Distinct().Count() > 1 ? ResolutionMethodType.BothAgreed : ResolutionMethodType.SingleReport;

            outcome.CustomerFaultWeight = attributions[0] == RejectionAttribution.Customer ? reports.Max(report => report.CustomerFaultWeight) : 0m;
        }

        else
        {
            outcome.Attribution = RejectionAttribution.Neutral;

            outcome.IsDisputed = true;

            outcome.ResolutionMethod = ResolutionMethodType.AutoNeutral;

            outcome.CustomerFaultWeight = 0m;
        }

        outcome.ResolvedAt ??= DateTime.UtcNow;

        var isCustomerFault = outcome.Attribution == RejectionAttribution.Customer;

        if (isCustomerFault == wasCustomerFault && outcome.CustomerFaultWeight == previousWeight)
        {
            return;
        }

        var profile = await GetProfileAsync(order.CustomerId, cancellationToken);

        profile.CustomerFaultRefusals += (isCustomerFault ? 1 : 0) - (wasCustomerFault ? 1 : 0);

        profile.WeightedFaultScore += (isCustomerFault ? outcome.CustomerFaultWeight : 0m) - previousWeight;

        if (isCustomerFault && !wasCustomerFault)
        {
            profile.LastCustomerFaultRefusalAt = DateTime.UtcNow;
        }

        Recompute(profile);
    }

    private async Task<CustomerRiskProfile> GetProfileAsync(long customerId, CancellationToken cancellationToken)
    {
        var profiles = unitOfWork.Repository<CustomerRiskProfile>();

        var profile = await profiles.GetByIdAsync(customerId, cancellationToken);

        if (profile is not null)
        {
            return profile;
        }

        profile = new CustomerRiskProfile
        {
            Id = customerId,

            DistinctMerchants = await unitOfWork.Repository<MerchantCustomer>().CountAsync(customer => customer.CustomerId == customerId, cancellationToken)
        };

        await profiles.AddAsync(profile, cancellationToken);

        return profile;
    }

    private static void Recompute(CustomerRiskProfile profile)
    {
        if (profile.TotalOrders <= 0)
        {
            return;
        }

        var faultRate = (double)profile.WeightedFaultScore / profile.TotalOrders;

        profile.ReliabilityScore = (byte)Math.Clamp((int)Math.Round(100 * (1 - faultRate)), 0, 100);
    }
}
