using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

namespace Tamam.Application.Services.Orders;

public interface IOrderImportService
{
    Task<Result<OrderImportResultDto>> ImportAsync(OrderImportRequest request, CancellationToken cancellationToken = default);
}
