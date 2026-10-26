using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

namespace Tamam.Application.Services.Orders;

public interface IOrderService
{
    Task<Result<PagedResult<OrderListItemDto>>> GetAllAsync(OrderQuery query, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailDto>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailDto>> UpdateAsync(long id, UpdateOrderRequest request, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailDto>> ChangeStatusAsync(long id, ChangeOrderStatusRequest request, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailDto>> AddDeliveryAttemptAsync(long id, AddDeliveryAttemptRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
