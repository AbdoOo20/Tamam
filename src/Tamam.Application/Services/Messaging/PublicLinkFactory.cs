using System.Security.Cryptography;

using Microsoft.Extensions.Options;

using Tamam.Application.Settings;

using Tamam.Domain.Entities;

using Tamam.Domain.Enums;

namespace Tamam.Application.Services.Messaging;

public class PublicLinkFactory(IOptions<PublicLinksSettings> options)
{
    private readonly PublicLinksSettings settings = options.Value;

    public OrderPublicLink Create(Order order, PublicLinkPurpose purpose)
    {
        var hours = purpose == PublicLinkPurpose.OrderConfirmation ? settings.ConfirmationLinkHours : settings.SurveyLinkHours;

        return new OrderPublicLink
        {
            Order = order,

            Purpose = purpose,

            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(24)).ToLowerInvariant(),

            ExpiresAt = DateTime.UtcNow.AddHours(hours)
        };
    }

    public string BuildUrl(PublicLinkPurpose purpose, string token)
    {
        var prefix = purpose == PublicLinkPurpose.OrderConfirmation ? "c" : "s";

        return $"{settings.BaseUrl.TrimEnd('/')}/{prefix}/{token}";
    }
}
