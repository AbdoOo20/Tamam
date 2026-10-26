using Tamam.Application.Common;

using Tamam.Application.DTOs.DataRequests;

namespace Tamam.Application.Services.DataRequests;

public interface IDataRequestService
{
    Task<Result> SubmitFromLinkAsync(string token, CreateDataRequestRequest request, CancellationToken cancellationToken = default);

    Task<Result<PagedResult<DataRequestDto>>> GetAllAsync(DataRequestQuery query, CancellationToken cancellationToken = default);

    Task<Result<DataRequestDto>> ProcessAsync(int id, ProcessDataRequestRequest request, CancellationToken cancellationToken = default);

    Task<Result<CustomerDataExportDto>> ExportAsync(int id, CancellationToken cancellationToken = default);
}
