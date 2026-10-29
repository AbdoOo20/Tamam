using Tamam.Api.BackgroundServices;

using Tamam.Api.Extensions;

using Tamam.Api.Hubs;

using Tamam.Api.ExceptionHandling;

using Tamam.Api.OpenApi;

using Tamam.Api.Services;

using Tamam.Application;

using Tamam.Application.Abstractions;

using Tamam.Application.Settings;

using Tamam.Infrastructure;

using Tamam.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("The 'Jwt' configuration section was not found.");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.Configure<PublicLinksSettings>(builder.Configuration.GetSection(PublicLinksSettings.SectionName));

builder.Services.Configure<PlatformAdminSettings>(builder.Configuration.GetSection(PlatformAdminSettings.SectionName));

builder.Services.AddControllers();

builder.Services.AddApiResponses();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserAppService, UserAppService>();

builder.Services.AddSignalR();

builder.Services.AddSingleton<INotificationPublisher, SignalRNotificationPublisher>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddJwtAuthentication(jwtSettings);

builder.Services.AddAuthorization();

builder.Services.AddApiRateLimiting();

builder.Services.AddFrontendCors(builder.Configuration);

builder.Services.AddHostedService<MaintenanceWorker>();

var app = builder.Build();

await app.Services.SeedPlatformAdminAsync();

app.UseExceptionHandler();

app.UseApiStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Tamam API v1"));
}

app.UseHttpsRedirection();

app.UseCors(CorsExtensions.PolicyName);

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationsHub>(NotificationsHub.Route);

app.Run();
