using Tamam.Application.Common;

namespace Tamam.Application.Staff;

public interface IStaffService
{
    Task<IReadOnlyList<StaffDto>> GetMyStaffAsync(CancellationToken cancellationToken = default);

    Task<Result<StaffDto>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteAsync(int userId, CancellationToken cancellationToken = default);
}
