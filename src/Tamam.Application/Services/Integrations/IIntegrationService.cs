using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Application.DTOs.Orders;

namespace Tamam.Application.Services.Integrations;

public interface IIntegrationService
{
    Task<Result<IntegrationOrderDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    Task<Result<IntegrationOrderDto>> GetOrderAsync(string orderNo, CancellationToken cancellationToken = default);
}
