using Tamam.Api.Extensions;

using Tamam.Api.Middleware;

using Tamam.Api.OpenApi;

using Tamam.Api.Services;

using Tamam.Application;

using Tamam.Application.Abstractions;

using Tamam.Application.Settings;

using Tamam.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("The 'Jwt' configuration section was not found.");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddControllers();

builder.Services.AddApiResponses();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserAppService, UserAppService>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddJwtAuthentication(jwtSettings);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Tamam API v1"));
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
