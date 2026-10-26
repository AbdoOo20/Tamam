using Tamam.Application.Common;

using Tamam.Application.DTOs.Staff;

namespace Tamam.Application.Services.Staff;

public interface IStaffService
{
    Task<IReadOnlyList<StaffDto>> GetMyStaffAsync(CancellationToken cancellationToken = default);

    Task<Result<StaffDto>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(int userId, CancellationToken cancellationToken = default);
}
