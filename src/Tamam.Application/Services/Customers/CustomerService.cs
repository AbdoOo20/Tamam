using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Customers;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Customers;

public class CustomerService(IUnitOfWork unitOfWork, IUserAppService userAppService) : ICustomerService
{
    public async Task<Result<PagedResult<CustomerDto>>> GetAllAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var customers = unitOfWork.Repository<MerchantCustomer>().Query().Where(customer => customer.MerchantId == merchantId);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();

            customers = customers.Where(customer => customer.FullName.Contains(term) || customer.PhoneE164.Contains(term));
        }

        var page = await Project(customers.OrderByDescending(customer => customer.Orders.Max(order => (DateTime?)order.PlacedAt)))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        if (await SharesNetworkAsync(merchantId, cancellationToken))
        {
            return Result<PagedResult<CustomerDto>>.Ok(page);
        }

        var hidden = page.Items.Select(customer => customer with { NetworkReliabilityScore = null }).ToList();

        return Result<PagedResult<CustomerDto>>.Ok(page with { Items = hidden });
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var customer = await Project(unitOfWork.Repository<MerchantCustomer>().Query().Where(item => item.MerchantId == merchantId && item.Id == id)).FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        {
            return Result<CustomerDto>.NotFound("Customer was not found.");
        }

        if (!await SharesNetworkAsync(merchantId, cancellationToken))
        {
            customer = customer with { NetworkReliabilityScore = null };
        }

        return Result<CustomerDto>.Ok(customer);
    }

    public async Task<Result<CustomerDto>> UpdateAsync(long id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var customer = await unitOfWork.Repository<MerchantCustomer>().FirstOrDefaultAsync(item => item.MerchantId == merchantId && item.Id == id, cancellationToken);

        if (customer is null)
        {
            return Result<CustomerDto>.NotFound("Customer was not found.");
        }

        customer.FullName = request.FullName.Trim();

        customer.AltPhoneE164 = request.AltPhone?.Trim();

        customer.HasWhatsApp = request.HasWhatsApp;

        customer.Notes = request.Notes?.Trim();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    private static IQueryable<CustomerDto> Project(IQueryable<MerchantCustomer> customers) => customers.Select(customer => new CustomerDto(
        customer.Id,

        customer.FullName,

        customer.PhoneE164,

        customer.AltPhoneE164,

        customer.HasWhatsApp,

        customer.Notes,

        customer.Orders.Count(),

        customer.Orders.Count(order => order.StatusId == OrderStatusType.Delivered),

        customer.Orders.Count(order => order.StatusId == OrderStatusType.Refused),

        customer.Orders.Max(order => (DateTime?)order.PlacedAt),

        customer.Customer.RiskProfile != null ? customer.Customer.RiskProfile.ReliabilityScore : null));

    private Task<bool> SharesNetworkAsync(int merchantId, CancellationToken cancellationToken) =>

    unitOfWork.Repository<MerchantConsent>().AnyAsync(consent => consent.MerchantId == merchantId && consent.ShareOutcomesToNetwork, cancellationToken);
}
