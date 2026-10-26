using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Messaging;

using Tamam.Application.DTOs.Orders;

using Tamam.Application.DTOs.Rejections;

using Tamam.Application.DTOs.Risk;

using Tamam.Application.Services.Messaging;

using Tamam.Application.Services.Orders;

using Tamam.Application.Services.Rejections;

using Tamam.Application.Services.Risk;

namespace Tamam.Api.Controllers;

[Authorize]
public class OrdersController(
    IOrderService orderService,

    IOrderImportService importService,

    IConfirmationService confirmationService,

    IMessageService messageService,

    IRiskService riskService,

    IRejectionService rejectionService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Result<PagedResult<OrderListItemDto>>>> GetAll([FromQuery] OrderQuery query, CancellationToken cancellationToken)
        {
            return Respond(await orderService.GetAllAsync(query, cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Result<OrderDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return Respond(await orderService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<Result<OrderDetailDto>>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        return Respond(await orderService.CreateAsync(request, cancellationToken));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Result<OrderDetailDto>>> Update(long id, UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        return Respond(await orderService.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Result>> Delete(long id, CancellationToken cancellationToken)
    {
        return Respond(await orderService.DeleteAsync(id, cancellationToken));
    }

    [HttpPost("{id:long}/status")]
    public async Task<ActionResult<Result<OrderDetailDto>>> ChangeStatus(long id, ChangeOrderStatusRequest request, CancellationToken cancellationToken)
    {
        return Respond(await orderService.ChangeStatusAsync(id, request, cancellationToken));
    }

    [HttpPost("{id:long}/delivery-attempts")]
    public async Task<ActionResult<Result<OrderDetailDto>>> AddDeliveryAttempt(long id, AddDeliveryAttemptRequest request, CancellationToken cancellationToken)
    {
        return Respond(await orderService.AddDeliveryAttemptAsync(id, request, cancellationToken));
    }

    [HttpPost("import")]
    public async Task<ActionResult<Result<OrderImportResultDto>>> Import(OrderImportRequest request, CancellationToken cancellationToken)
    {
        return Respond(await importService.ImportAsync(request, cancellationToken));
    }

    [HttpGet("{id:long}/confirmation")]
    public async Task<ActionResult<Result<ConfirmationDto>>> GetConfirmation(long id, CancellationToken cancellationToken)
    {
        return Respond(await confirmationService.GetCurrentAsync(id, cancellationToken));
    }

    [HttpPost("{id:long}/confirmation/regenerate")]
    public async Task<ActionResult<Result<ConfirmationDto>>> RegenerateConfirmation(long id, CancellationToken cancellationToken)
    {
        return Respond(await confirmationService.RegenerateAsync(id, cancellationToken));
    }

    [HttpPost("{id:long}/confirmation/sent")]
    public async Task<ActionResult<Result<ConfirmationDto>>> MarkConfirmationSent(long id, CancellationToken cancellationToken)
    {
        return Respond(await confirmationService.MarkSentAsync(id, cancellationToken));
    }

    [HttpGet("{id:long}/messages")]
    public async Task<ActionResult<Result<IReadOnlyList<MessageLogDto>>>> GetMessages(long id, CancellationToken cancellationToken)
    {
        return Respond(await messageService.GetOrderMessagesAsync(id, cancellationToken));
    }

    [HttpGet("{id:long}/risk")]
    public async Task<ActionResult<Result<IReadOnlyList<RiskAssessmentDto>>>> GetRisk(long id, CancellationToken cancellationToken)
    {
        return Respond(await riskService.GetAssessmentsAsync(id, cancellationToken));
    }

    [HttpPost("{id:long}/risk/recalculate")]
    public async Task<ActionResult<Result<RiskAssessmentDto>>> RecalculateRisk(long id, CancellationToken cancellationToken)
    {
        return Respond(await riskService.RecalculateAsync(id, cancellationToken));
    }

    [HttpPost("{id:long}/risk/decision")]
    public async Task<ActionResult<Result<RiskAssessmentDto>>> DecideRisk(long id, RiskDecisionRequest request, CancellationToken cancellationToken)
    {
        return Respond(await riskService.DecideAsync(id, request, cancellationToken));
    }

    [HttpGet("{id:long}/rejections")]
    public async Task<ActionResult<Result<IReadOnlyList<RejectionReportDto>>>> GetRejections(long id, CancellationToken cancellationToken)
    {
        return Respond(await rejectionService.GetReportsAsync(id, cancellationToken));
    }

    [HttpGet("{id:long}/outcome")]
    public async Task<ActionResult<Result<OrderOutcomeDto>>> GetOutcome(long id, CancellationToken cancellationToken)
    {
        return Respond(await rejectionService.GetOutcomeAsync(id, cancellationToken));
    }

    [HttpPut("{id:long}/outcome/loss")]
    public async Task<ActionResult<Result<OrderOutcomeDto>>> UpdateLoss(long id, UpdateLossRequest request, CancellationToken cancellationToken)
    {
        return Respond(await rejectionService.UpdateLossAsync(id, request, cancellationToken));
    }
}
