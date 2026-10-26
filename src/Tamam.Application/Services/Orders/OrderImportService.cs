using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Orders;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Orders;

public class OrderImportService(IUnitOfWork unitOfWork, IUserAppService userAppService, IOrderCreationService orderCreation) : IOrderImportService
{
    public async Task<Result<OrderImportResultDto>> ImportAsync(OrderImportRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var merchant = await unitOfWork.Repository<Merchant>().GetByIdAsync(merchantId, cancellationToken);

        if (merchant is null)
        {
            return Result<OrderImportResultDto>.NotFound("Merchant was not found.");
        }

        var country = await unitOfWork.Repository<Country>().GetByIdAsync(merchant.CountryCode, cancellationToken);

        var import = new OrderImport
        {
            MerchantId = merchantId,

            UploadedByUserId = userAppService.UserId,

            Kind = request.Kind,

            FileName = request.FileName.Trim(),

            RowsTotal = request.Orders.Count,

            Status = ImportStatus.Processing
        };

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await unitOfWork.Repository<OrderImport>().AddAsync(import, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var errors = new List<string>();

            var imported = 0;

            for (var index = 0; index < request.Orders.Count; index++)
            {
                var result = await orderCreation.CreateAsync(
                    request.Orders[index],

                    merchant,

                    country?.PhoneCode ?? "+",

                    import.Id,

                    OrderSource.FileUpload,

                    request.Kind == ImportKind.Historical,

                    cancellationToken);

                if (result.Succeeded)
                {
                    imported++;
                }

                else
                {
                    errors.Add($"Row {index + 1}: {(result.Errors.Count > 0 ? string.Join("; ", result.Errors) : result.Message)}");
                }
            }

            import.RowsImported = imported;

            import.RowsFailed = request.Orders.Count - imported;

            import.Status = imported == 0 ? ImportStatus.Failed : ImportStatus.Completed;

            import.CompletedAt = DateTime.UtcNow;

            import.ErrorReport = errors.Count == 0 ? null : string.Join("\n", errors);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result<OrderImportResultDto>.Ok(new OrderImportResultDto(import.Id, import.Status, import.RowsTotal, import.RowsImported, import.RowsFailed, errors), "Import finished.");
        }

        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            throw;
        }
    }
}
