using System.Linq.Expressions;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using Tamam.Application.Abstractions;

using Tamam.Domain.Common;

using Tamam.Domain.Entities;

namespace Tamam.Infrastructure.Persistence;

public class TamamDbContext(DbContextOptions<TamamDbContext> options, IUserAppService userAppService) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<Country> Countries => Set<Country>();

    public DbSet<Region> Regions => Set<Region>();

    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();

    public DbSet<RejectionReason> RejectionReasons => Set<RejectionReason>();

    public DbSet<Merchant> Merchants => Set<Merchant>();

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

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditAndSoftDelete();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");

        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");

        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TamamDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(IAuditable).IsAssignableFrom(clrType))
            {
                modelBuilder.Entity(clrType).Property(nameof(IAuditable.CreatedAt)).HasDefaultValueSql("SYSUTCDATETIME()");
            }

            if (typeof(ISoftDeletable).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "entity");

                var isNotDeleted = Expression.Equal(Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)), Expression.Constant(false));

                modelBuilder.Entity(clrType).HasQueryFilter(Expression.Lambda(isNotDeleted, parameter));
            }
        }
    }

    private void ApplyAuditAndSoftDelete()
    {
        var now = DateTime.UtcNow;

        var currentUserId = userAppService.UserId;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry is { State: EntityState.Deleted, Entity: ISoftDeletable softDeletable })
            {
                entry.State = EntityState.Modified;

                softDeletable.IsDeleted = true;
            }

            if (entry.Entity is not IAuditable auditable)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = now;

                auditable.CreatedBy = currentUserId;
            }

            else if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;

                entry.Property(nameof(IAuditable.CreatedBy)).IsModified = false;

                auditable.UpdatedAt = now;

                auditable.UpdatedBy = currentUserId;
            }
        }
    }
}
