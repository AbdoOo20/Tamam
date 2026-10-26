using Tamam.Application.Common;

namespace Tamam.Api.Extensions;

public static class HttpResponseExtensions
{
    public static Task WriteResultAsync(this HttpResponse response, Result result, CancellationToken cancellationToken = default)
    {
        response.StatusCode = result.StatusCode;

        return response.WriteAsJsonAsync(result, result.GetType(), cancellationToken);
    }
}
