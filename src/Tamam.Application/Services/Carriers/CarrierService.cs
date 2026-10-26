using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.DTOs.Carriers;

using Tamam.Application.Mappings;

using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Carriers;

public class CarrierService(IUnitOfWork unitOfWork, IUserAppService userAppService) : ICarrierService
{
    public async Task<Result<IReadOnlyList<CarrierDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var carriers = await unitOfWork.Repository<Carrier>().Query()
            .Where(carrier => carrier.MerchantId == merchantId || carrier.MerchantId == null)
            .OrderBy(carrier => carrier.Name)
            .ToListAsync(cancellationToken);

        IReadOnlyList<CarrierDto> dtos = carriers.Select(carrier => carrier.ToDto()).ToList();

        return Result<IReadOnlyList<CarrierDto>>.Ok(dtos);
    }

    public async Task<Result<CarrierDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var carrier = await unitOfWork.Repository<Carrier>().FirstOrDefaultAsync(item => item.Id == id && (item.MerchantId == merchantId || item.MerchantId == null), cancellationToken);

        return carrier is null ? Result<CarrierDto>.NotFound("Carrier was not found.") : Result<CarrierDto>.Ok(carrier.ToDto());
    }

    public async Task<Result<CarrierDto>> CreateAsync(CarrierRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var exists = await unitOfWork.Repository<Carrier>().AnyAsync(item => item.MerchantId == merchantId && item.Name == request.Name.Trim(), cancellationToken);

        if (exists)
        {
            return Result<CarrierDto>.Conflict("A carrier with the same name already exists.");
        }

        var carrier = new Carrier
        {
            MerchantId = merchantId,

            Name = request.Name.Trim(),

            Phone = request.Phone?.Trim(),

            TrackingUrlTemplate = request.TrackingUrlTemplate?.Trim(),

            IsActive = request.IsActive
        };

        await unitOfWork.Repository<Carrier>().AddAsync(carrier, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CarrierDto>.Created(carrier.ToDto());
    }

    public async Task<Result<CarrierDto>> UpdateAsync(int id, CarrierRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var carrier = await unitOfWork.Repository<Carrier>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (carrier is null)
        {
            return Result<CarrierDto>.NotFound("Carrier was not found.");
        }

        carrier.Name = request.Name.Trim();

        carrier.Phone = request.Phone?.Trim();

        carrier.TrackingUrlTemplate = request.TrackingUrlTemplate?.Trim();

        carrier.IsActive = request.IsActive;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CarrierDto>.Ok(carrier.ToDto());
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        var carrier = await unitOfWork.Repository<Carrier>().FirstOrDefaultAsync(item => item.Id == id && item.MerchantId == merchantId, cancellationToken);

        if (carrier is null)
        {
            return Result.NotFound("Carrier was not found.");
        }

        unitOfWork.Repository<Carrier>().Remove(carrier);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok("Carrier deleted.");
    }
}
