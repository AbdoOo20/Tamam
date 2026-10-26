using Tamam.Application.Common;

namespace Tamam.Application.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterMerchantAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
