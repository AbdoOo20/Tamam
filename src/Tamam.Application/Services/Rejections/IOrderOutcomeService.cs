using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Rejections;

public interface IOrderOutcomeService
{
    Task ApplyFinalResultAsync(Order order, OrderFinalResult result, CancellationToken cancellationToken = default);

    Task ResolveRefusalAsync(Order order, CancellationToken cancellationToken = default);
}
