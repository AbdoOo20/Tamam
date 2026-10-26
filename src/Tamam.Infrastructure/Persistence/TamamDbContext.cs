using Microsoft.EntityFrameworkCore;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence;

public class TamamDbContext(DbContextOptions<TamamDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();

    public DbSet<Region> Regions => Set<Region>();

    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();

    public DbSet<RejectionReason> RejectionReasons => Set<RejectionReason>();

    public DbSet<Merchant> Merchants => Set<Merchant>();

    public DbSet<User> Users => Set<User>();

    public DbSet<MerchantConsent> MerchantConsents => Set<MerchantConsent>();

    public DbSet<Carrier> Carriers => Set<Carrier>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<MerchantCustomer> MerchantCustomers => Set<MerchantCustomer>();

    public DbSet<CustomerRiskProfile> CustomerRiskProfiles => Set<CustomerRiskProfile>();

    public DbSet<CustomerDataRequest> CustomerDataRequests => Set<CustomerDataRequest>();

    public DbSet<OrderImport> OrderImports => Set<OrderImport>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();

    public DbSet<DeliveryAttempt> DeliveryAttempts => Set<DeliveryAttempt>();

    public DbSet<OrderPublicLink> OrderPublicLinks => Set<OrderPublicLink>();

    public DbSet<MessageTemplate> MessageTemplates => Set<MessageTemplate>();

    public DbSet<ConfirmationRequest> ConfirmationRequests => Set<ConfirmationRequest>();

    public DbSet<MessageLog> MessageLogs => Set<MessageLog>();

    public DbSet<RejectionReport> RejectionReports => Set<RejectionReport>();

    public DbSet<OrderOutcome> OrderOutcomes => Set<OrderOutcome>();

    public DbSet<RiskRule> RiskRules => Set<RiskRule>();

    public DbSet<OrderRiskAssessment> OrderRiskAssessments => Set<OrderRiskAssessment>();

    public DbSet<OrderRiskSignal> OrderRiskSignals => Set<OrderRiskSignal>();

    public DbSet<MerchantQualityStat> MerchantQualityStats => Set<MerchantQualityStat>();

    public DbSet<Plan> Plans => Set<Plan>();

    public DbSet<MerchantSubscription> MerchantSubscriptions => Set<MerchantSubscription>();

    public DbSet<UsageRecord> UsageRecords => Set<UsageRecord>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TamamDbContext).Assembly);
    }
}
