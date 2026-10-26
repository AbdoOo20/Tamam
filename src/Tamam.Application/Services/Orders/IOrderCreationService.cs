using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Orders;

public interface IOrderCreationService
{
    Task<Result<long>> CreateAsync(CreateOrderRequest request, Merchant merchant, string phoneCode, long? importId, OrderSource source, bool isHistorical, CancellationToken cancellationToken = default);
}
