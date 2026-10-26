using Tamam.Api.Extensions;

using Tamam.Application.Common;

namespace Tamam.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }

        catch (UnauthorizedAccessException exception)
        {
            await context.Response.WriteResultAsync(Result.Unauthorized(exception.Message), context.RequestAborted);
        }

        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
        }

        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);

            await context.Response.WriteResultAsync(Result.InternalServerError(), context.RequestAborted);
        }
    }
}
