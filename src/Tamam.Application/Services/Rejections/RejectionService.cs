using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Rejections;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Rejections;

public class RejectionService(IUnitOfWork unitOfWork, IUserAppService userAppService, IOrderOutcomeService outcomes) : IRejectionService
{
    public async Task<Result<RejectionReportDto>> ReportAsync(ReportRejectionRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var order = await unitOfWork.Repository<Order>().FirstOrDefaultAsync(item => item.Id == request.OrderId && item.MerchantId == merchantId, cancellationToken);

        if (order is null)
        {
            return Result<RejectionReportDto>.NotFound("Order was not found.");
        }

        if (order.StatusId is not (OrderStatusType.Refused or OrderStatusType.Returned))
        {
            return Result<RejectionReportDto>.BadRequest("Mark the order as refused before reporting the rejection reason.");
        }

        var report = new RejectionReport
        {
            OrderId = order.Id,

            ReporterType = ReporterKind.Merchant,

            ReportedByUserId = userAppService.UserId,

            ReasonId = request.ReasonId,

            Details = request.Details?.Trim(),

            EvidenceUrl = request.EvidenceUrl?.Trim()
        };

        await unitOfWork.Repository<RejectionReport>().AddAsync(report, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await outcomes.ResolveRefusalAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var reports = await GetReportsAsync(order.Id, cancellationToken);

        var created = reports.Data!.First(item => item.Id == report.Id);

        return Result<RejectionReportDto>.Created(created, "Rejection reported.");
    }

    public async Task<Result<IReadOnlyList<RejectionReportDto>>> GetReportsAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (!await unitOfWork.Repository<Order>().AnyAsync(order => order.Id == orderId && order.MerchantId == merchantId, cancellationToken))
        {
            return Result<IReadOnlyList<RejectionReportDto>>.NotFound("Order was not found.");
        }

        IReadOnlyList<RejectionReportDto> reports = await unitOfWork.Repository<RejectionReport>().Query()
            .Where(report => report.OrderId == orderId)
            .OrderBy(report => report.CreatedAt)
            .Select(report => new RejectionReportDto(
            report.Id,

            report.OrderId,

            report.ReporterType,

            report.ReasonId,

            report.Reason.NameAr,

            report.Reason.Attribution,

            report.Details,

            report.EvidenceUrl,

            report.CreatedAt))
                .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<RejectionReportDto>>.Ok(reports);
    }

    public async Task<Result<OrderOutcomeDto>> GetOutcomeAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var outcome = await unitOfWork.Repository<OrderOutcome>().Query()
            .Where(item => item.Id == orderId && item.Order.MerchantId == merchantId)
            .FirstOrDefaultAsync(cancellationToken);

        return outcome is null ? Result<OrderOutcomeDto>.NotFound("No outcome has been recorded for this order yet.") : Result<OrderOutcomeDto>.Ok(outcome.ToDto());
    }

    public async Task<Result<OrderOutcomeDto>> UpdateLossAsync(long orderId, UpdateLossRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var outcome = await unitOfWork.Repository<OrderOutcome>().Query(asNoTracking: false)
            .Where(item => item.Id == orderId && item.Order.MerchantId == merchantId)
            .FirstOrDefaultAsync(cancellationToken);

        if (outcome is null)
        {
            return Result<OrderOutcomeDto>.NotFound("No outcome has been recorded for this order yet.");
        }

        outcome.OutboundShippingCost = request.OutboundShippingCost;

        outcome.ReturnShippingCost = request.ReturnShippingCost;

        outcome.OtherLoss = request.OtherLoss;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<OrderOutcomeDto>.Ok(outcome.ToDto(), "Loss updated.");
    }
}
