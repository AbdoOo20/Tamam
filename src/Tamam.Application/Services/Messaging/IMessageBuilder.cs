using Tamam.Domain.Entities;

namespace Tamam.Application.Services.Messaging;

public interface IMessageBuilder
{
    Task<BuiltMessage> BuildOrderConfirmationAsync(Order order, IEnumerable<OrderItem> items, Merchant merchant, string confirmationUrl, CancellationToken cancellationToken = default);

    Task<BuiltMessage> BuildRejectionSurveyAsync(Order order, Merchant merchant, string surveyUrl, CancellationToken cancellationToken = default);
}
