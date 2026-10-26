using Microsoft.Extensions.DependencyInjection;

using Tamam.Application.Services.Billing;

using Tamam.Application.Services.Carriers;

using Tamam.Application.Services.Countries;

using Tamam.Application.Services.Customers;

using Tamam.Application.Services.Lookups;

using Tamam.Application.Services.Merchants;

using Tamam.Application.Services.Messaging;

using Tamam.Application.Services.Notifications;

using Tamam.Application.Services.Orders;

using Tamam.Application.Services.Products;

using Tamam.Application.Services.Public;

using Tamam.Application.Services.Rejections;

using Tamam.Application.Services.Reports;

using Tamam.Application.Services.Risk;

namespace Tamam.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICountryService, CountryService>();

        services.AddScoped<ILookupService, LookupService>();

        services.AddScoped<IMerchantService, MerchantService>();

        services.AddScoped<ICarrierService, CarrierService>();

        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<ICustomerService, CustomerService>();

        services.AddScoped<PublicLinkFactory>();

        services.AddScoped<IMessageBuilder, MessageBuilder>();

        services.AddScoped<IConfirmationService, ConfirmationService>();

        services.AddScoped<IMessageService, MessageService>();

        services.AddScoped<IRiskScoringService, RiskScoringService>();

        services.AddScoped<IRiskService, RiskService>();

        services.AddScoped<IOrderOutcomeService, OrderOutcomeService>();

        services.AddScoped<IRejectionService, RejectionService>();

        services.AddScoped<IOrderCreationService, OrderCreationService>();

        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<IOrderImportService, OrderImportService>();

        services.AddScoped<IPublicOrderService, PublicOrderService>();

        services.AddScoped<IBillingService, BillingService>();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
