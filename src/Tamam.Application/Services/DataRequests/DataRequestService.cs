using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.DataRequests;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.DataRequests;

public class DataRequestService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IDataRequestService
{
    private const string DeletedMarker = "[deleted]";

    public async Task<Result> SubmitFromLinkAsync(string token, CreateDataRequestRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Type == DataRequestType.OptOut)
        {
            return Result.BadRequest("Use the opt-out option to stop receiving messages.");
        }

        var link = await unitOfWork.Repository<OrderPublicLink>().FirstOrDefaultAsync(item => item.Token == token, cancellationToken);

        if (link is null)
        {
            return Result.NotFound("This link is not valid.");
        }

        var customerId = await unitOfWork.Repository<Order>().Query()
            .Where(order => order.Id == link.OrderId)
            .Select(order => (long?)order.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (customerId is null)
        {
            return Result.NotFound("This link is not valid.");
        }

        var type = request.Type;

        var alreadyOpen = await unitOfWork.Repository<CustomerDataRequest>().AnyAsync(item => item.CustomerId == customerId && item.Type == type && item.Status == DataRequestStatus.Open, cancellationToken);

        if (!alreadyOpen)
        {
            await unitOfWork.Repository<CustomerDataRequest>().AddAsync(new CustomerDataRequest
            {
                CustomerId = customerId,

                Type = type,

                Status = DataRequestStatus.Open,

                RequestedAt = DateTime.UtcNow
            }, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Created("Your request was received and will be reviewed.");
    }

    public async Task<Result<PagedResult<DataRequestDto>>> GetAllAsync(DataRequestQuery query, CancellationToken cancellationToken = default)
    {
        var requests = unitOfWork.Repository<CustomerDataRequest>().Query();

        if (query.Status is { } status)
        {
            requests = requests.Where(item => item.Status == status);
        }

        if (query.Type is { } type)
        {
            requests = requests.Where(item => item.Type == type);
        }

        var page = await requests
            .OrderByDescending(item => item.RequestedAt)
            .Select(item => new DataRequestDto(item.Id, item.CustomerId, item.Type, item.Status, item.RequestedAt, item.ProcessedAt, item.ProcessedByUserId))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<DataRequestDto>>.Ok(page);
    }

    public async Task<Result<DataRequestDto>> ProcessAsync(int id, ProcessDataRequestRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Repository<CustomerDataRequest>().FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (entity is null)
        {
            return Result<DataRequestDto>.NotFound("Data request was not found.");
        }

        if (entity.Status != DataRequestStatus.Open)
        {
            return Result<DataRequestDto>.Conflict("This request was already processed.");
        }

        var now = DateTime.UtcNow;

        if (!request.Approve)
        {
            entity.Status = DataRequestStatus.Rejected;
        }

        else
        {
            if (entity.CustomerId is { } customerId)
            {
                if (entity.Type == DataRequestType.Delete)
                {
                    await AnonymizeAsync(customerId, now, cancellationToken);
                }

                else if (entity.Type == DataRequestType.OptOut)
                {
                    await OptOutAsync(customerId, now, cancellationToken);
                }
            }

            entity.Status = DataRequestStatus.Done;
        }

        entity.ProcessedAt = now;

        entity.ProcessedByUserId = userAppService.UserId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<DataRequestDto>.Ok(new DataRequestDto(entity.Id, entity.CustomerId, entity.Type, entity.Status, entity.RequestedAt, entity.ProcessedAt, entity.ProcessedByUserId));
    }

    public async Task<Result<CustomerDataExportDto>> ExportAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Repository<CustomerDataRequest>().FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (entity is null)
        {
            return Result<CustomerDataExportDto>.NotFound("Data request was not found.");
        }

        if (entity.Type != DataRequestType.Access || entity.CustomerId is null)
        {
            return Result<CustomerDataExportDto>.BadRequest("Only access requests can be exported.");
        }

        var customerId = entity.CustomerId.Value;

        var customer = await unitOfWork.Repository<Customer>().Query()
            .Where(item => item.Id == customerId)
            .Select(item => new { item.Id, item.FirstSeenAt, item.IsOptedOut })
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            return Result<CustomerDataExportDto>.NotFound("Customer was not found.");
        }

        IReadOnlyList<CustomerDataExportOrderDto> orders = await unitOfWork.Repository<Order>().Query()
            .Where(order => order.CustomerId == customerId)
            .OrderBy(order => order.PlacedAt)
            .Select(order => new CustomerDataExportOrderDto(
            order.Merchant.BusinessName,

            order.OrderNo,

            order.PlacedAt,

            order.RecipientName,

            order.AddressLine,

            order.CurrencyCode,

            order.TotalAmount,

            order.Status.NameAr))
                .ToListAsync(cancellationToken);

        return Result<CustomerDataExportDto>.Ok(new CustomerDataExportDto(customer.Id, customer.FirstSeenAt, customer.IsOptedOut, orders));
    }

    private async Task OptOutAsync(long customerId, DateTime now, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Repository<Customer>().GetByIdAsync(customerId, cancellationToken);

        if (customer is null || customer.IsOptedOut)
        {
            return;
        }

        customer.IsOptedOut = true;

        customer.OptedOutAt = now;
    }

    private async Task AnonymizeAsync(long customerId, DateTime now, CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Repository<Customer>().GetByIdAsync(customerId, cancellationToken);

        if (customer is null)
        {
            return;
        }

        var merchantCustomers = await unitOfWork.Repository<MerchantCustomer>().Query(false)
            .Where(item => item.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        foreach (var merchantCustomer in merchantCustomers)
        {
            merchantCustomer.FullName = DeletedMarker;

            merchantCustomer.PhoneE164 = string.Empty;

            merchantCustomer.AltPhoneE164 = null;

            merchantCustomer.HasWhatsApp = null;

            merchantCustomer.Notes = null;
        }

        var orders = await unitOfWork.Repository<Order>().Query(false)
            .Where(item => item.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        foreach (var order in orders)
        {
            order.RecipientName = DeletedMarker;

            order.RecipientPhoneE164 = string.Empty;

            order.AltPhoneE164 = null;

            order.AddressLine = DeletedMarker;

            order.Landmark = null;

            order.Latitude = null;

            order.Longitude = null;

            order.Notes = null;
        }

        var messages = await unitOfWork.Repository<MessageLog>().Query(false)
            .Where(item => item.Order != null && item.Order.CustomerId == customerId)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.ToPhoneE164 = null;

            message.Body = DeletedMarker;
        }

        customer.PhoneHash = SHA256.HashData(RandomNumberGenerator.GetBytes(32));

        customer.IsOptedOut = true;

        customer.OptedOutAt ??= now;
    }
}
