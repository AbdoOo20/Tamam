using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

using Tamam.Application.Services.Messaging;

namespace Tamam.Api.Controllers;

[Authorize]
public class MessagesController(IMessageService messageService) : ApiControllerBase
{
    [HttpGet("templates")]
    public async Task<ActionResult<Result<IReadOnlyList<MessageTemplateDto>>>> GetTemplates(CancellationToken cancellationToken)
    {
        return Respond(await messageService.GetTemplatesAsync(cancellationToken));
    }
}
