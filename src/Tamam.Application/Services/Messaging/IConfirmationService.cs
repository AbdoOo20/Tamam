using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Messaging;

public interface IConfirmationService
{
    Task<ConfirmationDto> AttachNewConfirmationAsync(Order order, IEnumerable<OrderItem> items, Merchant merchant, byte attemptNo, CancellationToken cancellationToken = default);

    Task<Result<ConfirmationDto>> GetCurrentAsync(long orderId, CancellationToken cancellationToken = default);

    Task<Result<ConfirmationDto>> RegenerateAsync(long orderId, CancellationToken cancellationToken = default);

    Task<Result<ConfirmationDto>> MarkSentAsync(long orderId, CancellationToken cancellationToken = default);
}
