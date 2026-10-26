using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

namespace Tamam.Application.Services.Messaging;

public interface IMessageService
{
    Task<Result<IReadOnlyList<MessageTemplateDto>>> GetTemplatesAsync(CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<MessageLogDto>>> GetOrderMessagesAsync(long orderId, CancellationToken cancellationToken = default);
}
