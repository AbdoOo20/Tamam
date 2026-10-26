using Tamam.Application.Common;

using Tamam.Application.DTOs.Carriers;

namespace Tamam.Application.Services.Carriers;

public interface ICarrierService
{
    Task<Result<IReadOnlyList<CarrierDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<CarrierDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<CarrierDto>> CreateAsync(CarrierRequest request, CancellationToken cancellationToken = default);

    Task<Result<CarrierDto>> UpdateAsync(int id, CarrierRequest request, CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
