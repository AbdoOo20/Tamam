using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Messaging;

public class MessageService(IUnitOfWork unitOfWork, IUserAppService userAppService) : IMessageService
{
    public async Task<Result<IReadOnlyList<MessageTemplateDto>>> GetTemplatesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<MessageTemplateDto> templates = await unitOfWork.Repository<MessageTemplate>().Query()
            .Where(template => template.IsActive)
            .OrderBy(template => template.Code)
            .ThenBy(template => template.Language)
            .Select(template => new MessageTemplateDto(template.Id, template.Code, template.Language, template.Channel, template.Category, template.Body))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<MessageTemplateDto>>.Ok(templates);
    }

    public async Task<Result<IReadOnlyList<MessageLogDto>>> GetOrderMessagesAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (!await unitOfWork.Repository<Order>().AnyAsync(order => order.Id == orderId && order.MerchantId == merchantId, cancellationToken))
        {
            return Result<IReadOnlyList<MessageLogDto>>.NotFound("Order was not found.");
        }

        IReadOnlyList<MessageLogDto> logs = await unitOfWork.Repository<MessageLog>().Query()
            .Where(log => log.OrderId == orderId)
            .OrderByDescending(log => log.CreatedAt)
            .Select(log => new MessageLogDto(log.Id, log.Direction, log.Channel, log.ToPhoneE164, log.Body, log.DeliveryStatus, log.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<MessageLogDto>>.Ok(logs);
    }
}
