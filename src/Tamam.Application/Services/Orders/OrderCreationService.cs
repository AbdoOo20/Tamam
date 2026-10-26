using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

using Tamam.Application.Services.Messaging;

using Tamam.Application.Services.Notifications;

using Tamam.Application.Services.Rejections;

using Tamam.Application.Services.Risk;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Orders;

public class OrderCreationService(
    IUnitOfWork unitOfWork,

    IUserAppService userAppService,

    IRiskScoringService riskScoring,

    IConfirmationService confirmations,

    IOrderOutcomeService outcomes) : IOrderCreationService
    {
        public async Task<Result<long>> CreateAsync(CreateOrderRequest request, Merchant merchant, string phoneCode, long? importId, OrderSource source, bool isHistorical, CancellationToken cancellationToken = default)
        {
            var phone = PhoneHelper.Normalize(request.RecipientPhone, phoneCode);

        if (phone is null)
        {
            return Result<long>.BadRequest("Recipient phone number is invalid.");
        }

        string? altPhone = null;

        if (!string.IsNullOrWhiteSpace(request.AltPhone))
        {
            altPhone = PhoneHelper.Normalize(request.AltPhone, phoneCode);

            if (altPhone is null)
            {
                return Result<long>.BadRequest("Alternative phone number is invalid.");
            }
        }

        var itemsTotal = request.Items.Sum(item => item.Quantity * item.UnitPrice);

        var total = itemsTotal + request.ShippingFee - request.DiscountAmount;

        if (total < 0)
        {
            return Result<long>.BadRequest("Discount cannot exceed the order value.");
        }

        if (request.DepositAmount > total)
        {
            return Result<long>.BadRequest("Deposit cannot exceed the order total.");
        }

        if (request.CarrierId is { } carrierId && !await unitOfWork.Repository<Carrier>().AnyAsync(carrier => carrier.Id == carrierId && (carrier.MerchantId == merchant.Id || carrier.MerchantId == null), cancellationToken))
        {
            return Result<long>.BadRequest("Carrier was not found.");
        }

        var productIds = request.Items.Where(item => item.ProductId.HasValue).Select(item => item.ProductId!.Value).Distinct().ToList();

        if (productIds.Count > 0 && await unitOfWork.Repository<Product>().CountAsync(product => productIds.Contains(product.Id) && product.MerchantId == merchant.Id, cancellationToken) != productIds.Count)
        {
            return Result<long>.BadRequest("One or more products were not found.");
        }

        var orderNo = request.OrderNo?.Trim();

        if (string.IsNullOrEmpty(orderNo))
        {
            orderNo = await GenerateOrderNoAsync(merchant.Id, cancellationToken);
        }

        else if (await unitOfWork.Repository<Order>().AnyAsync(existing => existing.MerchantId == merchant.Id && existing.OrderNo == orderNo, cancellationToken))
        {
            return Result<long>.Conflict($"Order number '{orderNo}' already exists.");
        }

        var now = DateTime.UtcNow;

        var placedAt = isHistorical && request.PlacedAt.HasValue ? request.PlacedAt.Value : now;

        var hash = PhoneHelper.Hash(phone);

        var customer = await unitOfWork.Repository<Customer>().FirstOrDefaultAsync(item => item.PhoneHash == hash, cancellationToken);

        MerchantCustomer? merchantCustomer = null;

        if (customer is not null)
        {
            merchantCustomer = await unitOfWork.Repository<MerchantCustomer>().FirstOrDefaultAsync(item => item.MerchantId == merchant.Id && item.CustomerId == customer.Id, cancellationToken);
        }

        var assessment = await riskScoring.EvaluateAsync(
            new RiskInput(merchant.Id, 0, customer?.Id, merchantCustomer?.Id, phone, merchant.CountryCode, request.AddressLine, request.City, request.RegionId, total, placedAt, merchant.ShadowMode),

            cancellationToken);

        customer ??= new Customer { PhoneHash = hash, CountryCode = merchant.CountryCode, FirstSeenAt = now };

        if (merchantCustomer is null)
        {
            merchantCustomer = new MerchantCustomer
            {
                MerchantId = merchant.Id,

                Customer = customer,

                FullName = request.RecipientName.Trim(),

                PhoneE164 = phone,

                AltPhoneE164 = altPhone
            };
        }

        else if (altPhone is not null)
        {
            merchantCustomer.AltPhoneE164 = altPhone;
        }

        var order = new Order
        {
            MerchantId = merchant.Id,

            MerchantCustomer = merchantCustomer,

            Customer = customer,

            OrderNo = orderNo,

            Source = source,

            ImportId = importId,

            IsHistorical = isHistorical,

            RecipientName = request.RecipientName.Trim(),

            RecipientPhoneE164 = phone,

            AltPhoneE164 = altPhone,

            CountryCode = merchant.CountryCode,

            RegionId = request.RegionId,

            City = request.City?.Trim(),

            AddressLine = request.AddressLine.Trim(),

            Landmark = request.Landmark?.Trim(),

            Latitude = request.Latitude,

            Longitude = request.Longitude,

            CurrencyCode = merchant.CurrencyCode,

            ItemsTotal = itemsTotal,

            ShippingFee = request.ShippingFee,

            DiscountAmount = request.DiscountAmount,

            TotalAmount = total,

            PaymentMethod = request.PaymentMethod,

            DepositAmount = request.DepositAmount,

            ExpectedDeliveryDate = request.ExpectedDeliveryDate,

            CarrierId = request.CarrierId,

            Notes = request.Notes?.Trim(),

            PlacedAt = placedAt,

            CreatedByUserId = userAppService.UserId,

            StatusId = OrderStatusType.New,

            LatestRiskScore = assessment.Score,

            LatestRiskLevel = assessment.Level
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,

                ProductName = item.ProductName.Trim(),

                Variant = item.Variant?.Trim(),

                Quantity = item.Quantity,

                UnitPrice = item.UnitPrice
            });
        }

        order.StatusHistory.Add(new OrderStatusHistory
        {
            FromStatusId = null,

            ToStatusId = OrderStatusType.New,

            ChangedByType = userAppService.UserId is null ? ActorType.System : ActorType.MerchantUser,

            ChangedByUserId = userAppService.UserId,

            Note = isHistorical ? "Imported historical order." : "Order created.",

            ChangedAt = now
        });

        assessment.Order = order;

        order.RiskAssessments.Add(assessment);

        if (!isHistorical)
        {
            await confirmations.AttachNewConfirmationAsync(order, order.Items, merchant, 1, cancellationToken);

            var unitCost = await unitOfWork.Repository<MerchantSubscription>().Query()
                .Where(subscription => subscription.MerchantId == merchant.Id)
                .OrderByDescending(subscription => subscription.StartedAt)
                .Select(subscription => subscription.Plan.PricePerCheckedOrder)
                .FirstOrDefaultAsync(cancellationToken);

            await unitOfWork.Repository<UsageRecord>().AddAsync(new UsageRecord
            {
                MerchantId = merchant.Id,

                Order = order,

                Type = UsageType.OrderCheck,

                Quantity = 1,

                UnitCost = unitCost,

                OccurredAt = now
            }, cancellationToken);

            if (assessment.Level == RiskLevel.High)
            {
                await unitOfWork.Repository<Notification>().AddAsync(
                    NotificationFactory.ForOrder(order, NotificationTypes.HighRiskOrder, "طلب عالي المخاطر", $"الطلب {orderNo} درجة المخاطرة {assessment.Score}"),

                    cancellationToken);
            }
        }

        await unitOfWork.Repository<Order>().AddAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (isHistorical && request.HistoricalStatus is { } historicalStatus && TryMapFinalResult(historicalStatus, out var finalResult))
        {
            order.StatusId = historicalStatus;

            await outcomes.ApplyFinalResultAsync(order, finalResult, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result<long>.Ok(order.Id);
    }

    private static bool TryMapFinalResult(OrderStatusType status, out OrderFinalResult result)
    {
        switch (status)
        {
            case OrderStatusType.Delivered:

            result = OrderFinalResult.Delivered;

            return true;

            case OrderStatusType.Refused:

            result = OrderFinalResult.Refused;

            return true;

            case OrderStatusType.CancelledByMerchant:

            case OrderStatusType.CancelledByCustomer:

            result = OrderFinalResult.Cancelled;

            return true;

            default:

            result = default;

            return false;
        }
    }

    private async Task<string> GenerateOrderNoAsync(int merchantId, CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var candidate = $"ORD-{DateTime.UtcNow:yyMMdd}-{RandomNumberGenerator.GetInt32(10000, 99999)}";

            if (!await unitOfWork.Repository<Order>().AnyAsync(order => order.MerchantId == merchantId && order.OrderNo == candidate, cancellationToken))
            {
                return candidate;
            }
        }

        return $"ORD-{Guid.NewGuid():N}"[..20];
    }
}
