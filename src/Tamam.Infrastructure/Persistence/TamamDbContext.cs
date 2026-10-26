using System.Linq.Expressions;

using System.Text.Json;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        var pending = CollectAuditTrail();

        using var transaction = pending.Count > 0 && Database.CurrentTransaction is null ? Database.BeginTransaction() : null;

        var affected = base.SaveChanges(acceptAllChangesOnSuccess);

        if (pending.Count > 0)
        {
            AuditLogs.AddRange(pending.Select(CreateAuditLog));

            ApplyAuditAndSoftDelete();

            base.SaveChanges(acceptAllChangesOnSuccess);
        }

        transaction?.Commit();

        return affected;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditAndSoftDelete();

        var pending = CollectAuditTrail();

        await using var transaction = pending.Count > 0 && Database.CurrentTransaction is null ? await Database.BeginTransactionAsync(cancellationToken) : null;

        var affected = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        if (pending.Count > 0)
        {
            AuditLogs.AddRange(pending.Select(CreateAuditLog));

            ApplyAuditAndSoftDelete();

            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        if (transaction is not null)
        {
            await transaction.CommitAsync(cancellationToken);
        }

        return affected;
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

    private static readonly HashSet<Type> AuditedTypes =

    [

    typeof(Order),

    typeof(Product),

    typeof(Carrier),

    typeof(Merchant),

    typeof(MerchantConsent),

    typeof(MerchantSubscription),

    typeof(User),

    typeof(Plan),

    typeof(RiskRule),

    typeof(CustomerDataRequest)

    ];

    private static readonly HashSet<string> IgnoredAuditProperties =

    [

    nameof(IAuditable.CreatedAt),

    nameof(IAuditable.CreatedBy),

    nameof(IAuditable.UpdatedAt),

    nameof(IAuditable.UpdatedBy),

    nameof(ISoftDeletable.IsDeleted),

    "RowVersion",

    "ConcurrencyStamp",

    "SecurityStamp",

    "NormalizedUserName",

    "NormalizedEmail",

    "AccessFailedCount",

    "LockoutEnd",

    "LastLoginAt"

    ];

    private static readonly HashSet<string> RedactedAuditProperties =

    [

    "PasswordHash",

    "ApiKeyHash"

    ];

    private sealed record PendingAudit(EntityEntry Entry, string Action, Dictionary<string, object?>? OldValues, Dictionary<string, object?>? NewValues);

    private List<PendingAudit> CollectAuditTrail()
    {
        var pending = new List<PendingAudit>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (!AuditedTypes.Contains(entry.Entity.GetType()))
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                var created = new Dictionary<string, object?>();

                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey() || property.Metadata.ValueGenerated != Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never || IgnoredAuditProperties.Contains(property.Metadata.Name) || property.CurrentValue is null)
                    {
                        continue;
                    }

                    created[property.Metadata.Name] = AuditValue(property.Metadata.Name, property.CurrentValue);
                }

                pending.Add(new PendingAudit(entry, "Create", null, created));

                continue;
            }

            if (entry.State != EntityState.Modified)
            {
                continue;
            }

            if (entry.Entity is ISoftDeletable { IsDeleted: true } && entry.Property(nameof(ISoftDeletable.IsDeleted)).IsModified)
            {
                pending.Add(new PendingAudit(entry, "Delete", null, null));

                continue;
            }

            var before = new Dictionary<string, object?>();

            var after = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                if (!property.IsModified || IgnoredAuditProperties.Contains(property.Metadata.Name) || Equals(property.OriginalValue, property.CurrentValue))
                {
                    continue;
                }

                before[property.Metadata.Name] = AuditValue(property.Metadata.Name, property.OriginalValue);

                after[property.Metadata.Name] = AuditValue(property.Metadata.Name, property.CurrentValue);
            }

            if (after.Count > 0)
            {
                pending.Add(new PendingAudit(entry, "Update", before, after));
            }
        }

        return pending;
    }

    private static object? AuditValue(string propertyName, object? value) => RedactedAuditProperties.Contains(propertyName) ? "[redacted]" : value;

    private AuditLog CreateAuditLog(PendingAudit pending)
    {
        var entry = pending.Entry;

        var keyProperties = entry.Metadata.FindPrimaryKey()?.Properties ?? [];

        var entityId = string.Join(",", keyProperties.Select(property => entry.Property(property.Name).CurrentValue?.ToString()));

        return new AuditLog
        {
            MerchantId = ResolveMerchantId(entry),

            UserId = userAppService.UserId,

            Action = pending.Action,

            EntityName = entry.Metadata.ClrType.Name,

            EntityId = entityId.Length == 0 ? null : entityId,

            OldValues = pending.OldValues is null ? null : JsonSerializer.Serialize(pending.OldValues),

            NewValues = pending.NewValues is null ? null : JsonSerializer.Serialize(pending.NewValues),

            IpAddress = userAppService.IpAddress
        };
    }

    private int? ResolveMerchantId(EntityEntry entry)
    {
        if (entry.Entity is Merchant merchant)
        {
            return merchant.Id;
        }

        if (entry.Metadata.FindProperty("MerchantId") is not null && entry.Property("MerchantId").CurrentValue is int merchantId)
        {
            return merchantId;
        }

        return userAppService.MerchantId;
    }
}
