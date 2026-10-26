using Tamam.Application.Common;

using Tamam.Application.DTOs.Customers;

namespace Tamam.Application.Services.Customers;

public interface ICustomerService
{
    Task<Result<PagedResult<CustomerDto>>> GetAllAsync(PagedQuery query, CancellationToken cancellationToken = default);

    Task<Result<CustomerDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Result<CustomerDto>> UpdateAsync(long id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
}
