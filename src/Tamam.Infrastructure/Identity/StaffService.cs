using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Application.Common;

using Tamam.Application.Staff;

using Tamam.Domain.Constants;

using Tamam.Domain.Entities;

using Tamam.Infrastructure.Persistence;

namespace Tamam.Infrastructure.Identity;

public class StaffService(TamamDbContext context, UserManager<User> userManager, IUserAppService userAppService) : IStaffService
{
    public async Task<IReadOnlyList<StaffDto>> GetMyStaffAsync(CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        return await context.Users
            .AsNoTracking()
            .Where(user => user.MerchantId == merchantId)
            .Join(context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole.RoleId })
            .Join(context.Roles, item => item.RoleId, role => role.Id, (item, role) => new StaffDto(item.user.Id, item.user.FullName, item.user.Email!, item.user.PhoneNumber, role.Name!, item.user.IsActive, item.user.LastLoginAt))
            .OrderBy(dto => dto.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<StaffDto>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Result<StaffDto>.Fail("Email is already registered.");
        }

        var user = new User
        {
            UserName = request.Email,

            Email = request.Email,

            EmailConfirmed = true,

            PhoneNumber = request.PhoneNumber,

            FullName = request.FullName,

            MerchantId = merchantId
        };

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return Result<StaffDto>.Fail(createResult.Errors.Select(error => error.Description));
        }

        await userManager.AddToRoleAsync(user, RoleNames.Staff);

        return Result<StaffDto>.Ok(new StaffDto(user.Id, user.FullName, user.Email, user.PhoneNumber, RoleNames.Staff, user.IsActive, user.LastLoginAt));
    }

    public async Task<Result<bool>> DeleteAsync(int userId, CancellationToken cancellationToken = default)
    {
        var merchantId = userAppService.RequiredMerchantId;

        if (userId == userAppService.RequiredUserId)
        {
            return Result<bool>.Fail("You cannot delete your own account.");
        }

        var user = await userManager.Users.FirstOrDefaultAsync(item => item.Id == userId && item.MerchantId == merchantId, cancellationToken);

        if (user is null)
        {
            return Result<bool>.Fail("User was not found.");
        }

        if (await userManager.IsInRoleAsync(user, RoleNames.Owner))
        {
            return Result<bool>.Fail("The owner account cannot be deleted.");
        }

        var deleteResult = await userManager.DeleteAsync(user);

        return deleteResult.Succeeded ? Result<bool>.Ok(true) : Result<bool>.Fail(deleteResult.Errors.Select(error => error.Description));
    }
}
