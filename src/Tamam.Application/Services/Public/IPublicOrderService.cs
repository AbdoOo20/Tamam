using Tamam.Application.Common;

using Tamam.Application.DTOs.Public;

namespace Tamam.Application.Services.Public;

public interface IPublicOrderService
{
    Task<Result<PublicOrderDto>> GetAsync(string token, CancellationToken cancellationToken = default);

    Task<Result<PublicOrderDto>> ConfirmAsync(string token, PublicConfirmRequest request, CancellationToken cancellationToken = default);

    Task<Result<PublicOrderDto>> RescheduleAsync(string token, PublicRescheduleRequest request, CancellationToken cancellationToken = default);

    Task<Result<PublicOrderDto>> CancelAsync(string token, PublicCancelRequest request, CancellationToken cancellationToken = default);

    Task<Result<PublicOrderDto>> SubmitSurveyAsync(string token, PublicSurveyRequest request, CancellationToken cancellationToken = default);

    Task<Result> OptOutAsync(string token, CancellationToken cancellationToken = default);
}
