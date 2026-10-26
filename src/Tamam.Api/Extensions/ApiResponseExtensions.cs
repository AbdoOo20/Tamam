using Microsoft.AspNetCore.Mvc;

using Tamam.Application.Common;

namespace Tamam.Api.Extensions;

public static class ApiResponseExtensions
{
    public static IServiceCollection AddApiResponses(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(entry => entry.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                var result = Result.BadRequest(errors);

                return new ObjectResult(result) { StatusCode = result.StatusCode };
            };
        });

        return services;
    }
}
