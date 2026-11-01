namespace Tamam.Application.Abstractions;

/// <summary>
/// Tells whether a merchant may use the system right now (not suspended, closed or deleted).
/// The answer is cached for a few seconds, and <see cref="Forget"/> clears it when an admin changes the status.
/// </summary>
public interface IMerchantAccessService
{
    const string NotActiveMessage = "This store account is suspended. Contact support.";

    Task<bool> IsActiveAsync(int merchantId, CancellationToken cancellationToken = default);

    void Forget(int merchantId);
}
