using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.RateLimiting;

using Tamam.Api.Authentication;

using Tamam.Api.RateLimiting;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Integrations;

using Tamam.Application.DTOs.Orders;

using Tamam.Application.Services.Integrations;

namespace Tamam.Api.Controllers;

[Authorize(AuthenticationSchemes = ApiKeyDefaults.Scheme)]
[EnableRateLimiting(RateLimitPolicies.Integration)]
public class IntegrationsController(IIntegrationService integrationService) : ApiControllerBase
{
    [HttpPost("orders")]
    public async Task<ActionResult<Result<IntegrationOrderDto>>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        return Respond(await integrationService.CreateOrderAsync(request, cancellationToken));
    }

    [HttpGet("orders/{orderNo}")]
    public async Task<ActionResult<Result<IntegrationOrderDto>>> GetOrder(string orderNo, CancellationToken cancellationToken)
    {
        return Respond(await integrationService.GetOrderAsync(orderNo, cancellationToken));
    }
}
