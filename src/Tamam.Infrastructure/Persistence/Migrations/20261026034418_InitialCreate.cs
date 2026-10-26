using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tamam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneCode = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MessageTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    Language = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false),
                    Category = table.Column<byte>(type: "tinyint", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ProviderTemplateName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ApprovalStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MonthlyFee = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PricePerCheckedOrder = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    IncludedOrders = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReasons",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Attribution = table.Column<byte>(type: "tinyint", nullable: false),
                    CustomerFaultWeight = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReasons", x => x.Id);
                    table.CheckConstraint("CK_RejectionReasons_Weight", "[CustomerFaultWeight] BETWEEN 0 AND 1");
                });

            migrationBuilder.CreateTable(
                name: "RiskRules",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ExplanationTemplateAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Points = table.Column<short>(type: "smallint", nullable: false),
                    Scope = table.Column<byte>(type: "tinyint", nullable: false),
                    ParametersJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskRules", x => x.Id);
                    table.CheckConstraint("CK_RiskRules_Json", "[ParametersJson] IS NULL OR ISJSON([ParametersJson]) = 1");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneHash = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    IsOptedOut = table.Column<bool>(type: "bit", nullable: false),
                    OptedOutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstSeenAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    DefaultLanguage = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    StoreUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InstagramHandle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ShadowMode = table.Column<bool>(type: "bit", nullable: false),
                    ApiKeyHash = table.Column<byte[]>(type: "binary(32)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merchants_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    ParentRegionId = table.Column<int>(type: "int", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_ParentRegionId",
                        column: x => x.ParentRegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRiskProfiles",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    DeliveredCount = table.Column<int>(type: "int", nullable: false),
                    CustomerFaultRefusals = table.Column<int>(type: "int", nullable: false),
                    NoResponseCount = table.Column<int>(type: "int", nullable: false),
                    WeightedFaultScore = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    DistinctMerchants = table.Column<int>(type: "int", nullable: false),
                    ReliabilityScore = table.Column<byte>(type: "tinyint", nullable: true),
                    LastOrderAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCustomerFaultRefusalAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRiskProfiles", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerRiskProfiles_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: true),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TrackingUrlTemplate = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carriers_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carriers_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantCustomers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneE164 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    AltPhoneE164 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    HasWhatsApp = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantCustomers_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantQualityStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    PeriodMonth = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    DeliveredCount = table.Column<int>(type: "int", nullable: false),
                    RefusedTotal = table.Column<int>(type: "int", nullable: false),
                    RefusedCustomerFault = table.Column<int>(type: "int", nullable: false),
                    RefusedMerchantFault = table.Column<int>(type: "int", nullable: false),
                    RefusedCarrierFault = table.Column<int>(type: "int", nullable: false),
                    RefusedNeutral = table.Column<int>(type: "int", nullable: false),
                    DisputedCount = table.Column<int>(type: "int", nullable: false),
                    TotalLoss = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QualityComplaintRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    NetworkAvgComplaintRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantQualityStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantQualityStats_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<short>(type: "smallint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    TrialEndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantSubscriptions_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantSubscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DefaultPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AdUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDataRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDataRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDataRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDataRequests_Users_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchantConsents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    TermsVersion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ShareOutcomesToNetwork = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedByUserId = table.Column<int>(type: "int", nullable: true),
                    AcceptedFromIp = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: true),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantConsents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantConsents_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MerchantConsents_Users_AcceptedByUserId",
                        column: x => x.AcceptedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderImports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: true),
                    Kind = table.Column<byte>(type: "tinyint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    RowsTotal = table.Column<int>(type: "int", nullable: false),
                    RowsImported = table.Column<int>(type: "int", nullable: false),
                    RowsFailed = table.Column<int>(type: "int", nullable: false),
                    ErrorReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderImports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderImports_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderImports_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    MerchantCustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Source = table.Column<byte>(type: "tinyint", nullable: false),
                    ImportId = table.Column<long>(type: "bigint", nullable: true),
                    IsHistorical = table.Column<bool>(type: "bit", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RecipientPhoneE164 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    AltPhoneE164 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CountryCode = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    CurrencyCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    ItemsTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<byte>(type: "tinyint", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositPaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountToCollect = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "CASE WHEN [PaymentMethod] = 2 THEN CAST(0 AS decimal(18,2)) ELSE [TotalAmount] - [DepositAmount] END", stored: true),
                    ExpectedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CarrierId = table.Column<int>(type: "int", nullable: true),
                    TrackingNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    LatestRiskScore = table.Column<byte>(type: "tinyint", nullable: true),
                    LatestRiskLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PlacedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Orders_Deposit", "[DepositAmount] >= 0 AND [DepositAmount] <= [TotalAmount]");
                    table.CheckConstraint("CK_Orders_Total", "[TotalAmount] = [ItemsTotal] + [ShippingFee] - [DiscountAmount]");
                    table.ForeignKey(
                        name: "FK_Orders_Carriers_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_MerchantCustomers_MerchantCustomerId",
                        column: x => x.MerchantCustomerId,
                        principalTable: "MerchantCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderImports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "OrderImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    AttemptNo = table.Column<byte>(type: "tinyint", nullable: false),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProposedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ResponseNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmationRequests_MessageTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "MessageTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfirmationRequests_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    AttemptNo = table.Column<byte>(type: "tinyint", nullable: false),
                    AttemptedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<byte>(type: "tinyint", nullable: false),
                    CarrierNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProofImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAttempts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelatedOrderId = table.Column<long>(type: "bigint", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Orders_RelatedOrderId",
                        column: x => x.RelatedOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Variant = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "[Quantity] * [UnitPrice]", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.CheckConstraint("CK_OrderItems_Qty", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderOutcomes",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    FinalResult = table.Column<byte>(type: "tinyint", nullable: false),
                    FinalReasonId = table.Column<short>(type: "smallint", nullable: true),
                    Attribution = table.Column<byte>(type: "tinyint", nullable: true),
                    IsDisputed = table.Column<bool>(type: "bit", nullable: false),
                    ResolutionMethod = table.Column<byte>(type: "tinyint", nullable: true),
                    CustomerFaultWeight = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    OutboundShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReturnShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OtherLoss = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LossAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, computedColumnSql: "[OutboundShippingCost] + [ReturnShippingCost] + [OtherLoss]", stored: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOutcomes", x => x.OrderId);
                    table.CheckConstraint("CK_OrderOutcomes_Weight", "[CustomerFaultWeight] BETWEEN 0 AND 1");
                    table.ForeignKey(
                        name: "FK_OrderOutcomes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderOutcomes_RejectionReasons_FinalReasonId",
                        column: x => x.FinalReasonId,
                        principalTable: "RejectionReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderPublicLinks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Purpose = table.Column<byte>(type: "tinyint", nullable: false),
                    Token = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstOpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpenCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPublicLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPublicLinks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderRiskAssessments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<byte>(type: "tinyint", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false),
                    RecommendedAction = table.Column<byte>(type: "tinyint", nullable: false),
                    ModelVersion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IsShadow = table.Column<bool>(type: "bit", nullable: false),
                    MerchantDecision = table.Column<byte>(type: "tinyint", nullable: true),
                    DecidedByUserId = table.Column<int>(type: "int", nullable: true),
                    DecidedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRiskAssessments", x => x.Id);
                    table.CheckConstraint("CK_OrderRiskAssessments_Score", "[Score] BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_OrderRiskAssessments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderRiskAssessments_Users_DecidedByUserId",
                        column: x => x.DecidedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    FromStatusId = table.Column<byte>(type: "tinyint", nullable: true),
                    ToStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    ChangedByType = table.Column<byte>(type: "tinyint", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_OrderStatuses_FromStatusId",
                        column: x => x.FromStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_OrderStatuses_ToStatusId",
                        column: x => x.ToStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Users_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ReporterType = table.Column<byte>(type: "tinyint", nullable: false),
                    ReportedByUserId = table.Column<int>(type: "int", nullable: true),
                    ReasonId = table.Column<short>(type: "smallint", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EvidenceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RejectionReports_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RejectionReports_RejectionReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "RejectionReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RejectionReports_Users_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsageRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, computedColumnSql: "CAST([Quantity] AS decimal(18,4)) * [UnitCost]", stored: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageRecords_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsageRecords_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    ConfirmationRequestId = table.Column<long>(type: "bigint", nullable: true),
                    Direction = table.Column<byte>(type: "tinyint", nullable: false),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false),
                    ToPhoneE164 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ProviderMessageId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DeliveryStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    ErrorCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageLogs_ConfirmationRequests_ConfirmationRequestId",
                        column: x => x.ConfirmationRequestId,
                        principalTable: "ConfirmationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageLogs_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderRiskSignals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<long>(type: "bigint", nullable: false),
                    RuleId = table.Column<short>(type: "smallint", nullable: false),
                    Points = table.Column<short>(type: "smallint", nullable: false),
                    ExplanationAr = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRiskSignals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRiskSignals_OrderRiskAssessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "OrderRiskAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderRiskSignals_RiskRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "RiskRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Code", "CreatedBy", "CurrencyCode", "IsActive", "IsDeleted", "NameAr", "NameEn", "PhoneCode", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "AE", null, "AED", true, false, "الإمارات", "UAE", "+971", null, null },
                    { "BH", null, "BHD", true, false, "البحرين", "Bahrain", "+973", null, null },
                    { "EG", null, "EGP", true, false, "مصر", "Egypt", "+20", null, null },
                    { "IN", null, "INR", true, false, "الهند", "India", "+91", null, null },
                    { "JO", null, "JOD", true, false, "الأردن", "Jordan", "+962", null, null },
                    { "KW", null, "KWD", true, false, "الكويت", "Kuwait", "+965", null, null },
                    { "NG", null, "NGN", true, false, "نيجيريا", "Nigeria", "+234", null, null },
                    { "OM", null, "OMR", true, false, "عُمان", "Oman", "+968", null, null },
                    { "PK", null, "PKR", true, false, "باكستان", "Pakistan", "+92", null, null },
                    { "QA", null, "QAR", true, false, "قطر", "Qatar", "+974", null, null },
                    { "SA", null, "SAR", true, false, "السعودية", "Saudi Arabia", "+966", null, null }
                });

            migrationBuilder.InsertData(
                table: "MessageTemplates",
                columns: new[] { "Id", "ApprovalStatus", "Body", "Category", "Channel", "Code", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "Language", "ProviderTemplateName", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, (byte)0, "أهلاً {CustomerName}،\nطلبك من {StoreName}:\n{ItemsSummary}\nالإجمالي: {Total} {Currency}\nالعنوان: {Address}\nميعاد التوصيل المتوقع: {DeliveryDate}\nعشان نأكد الطلب اضغط على اللينك: {ConfirmLink}", (byte)1, (byte)1, "ORDER_CONFIRM", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "ar-EG", null, null, null },
                    { 2, (byte)0, "حياك الله {CustomerName}،\nطلبك من {StoreName}:\n{ItemsSummary}\nالإجمالي: {Total} {Currency}\nالعنوان: {Address}\nموعد التوصيل المتوقع: {DeliveryDate}\nلتأكيد الطلب تفضل بفتح الرابط: {ConfirmLink}", (byte)1, (byte)1, "ORDER_CONFIRM", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "ar-SA", null, null, null },
                    { 3, (byte)0, "نأسف لعدم إتمام طلبك من {StoreName}. ساعدنا بمعرفة السبب في ثوانٍ: {SurveyLink}", (byte)1, (byte)1, "REJECTION_SURVEY", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, "ar", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Code", "CreatedBy", "IsDeleted", "IsFinal", "NameAr", "NameEn", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { (byte)1, "New", null, false, false, "جديد", "New", (byte)1, null, null },
                    { (byte)2, "AwaitingConfirmation", null, false, false, "بانتظار التأكيد", "Awaiting confirmation", (byte)2, null, null },
                    { (byte)3, "Confirmed", null, false, false, "تم التأكيد", "Confirmed", (byte)3, null, null },
                    { (byte)4, "DepositRequested", null, false, false, "مطلوب عربون", "Deposit requested", (byte)4, null, null },
                    { (byte)5, "Shipped", null, false, false, "تم الشحن", "Shipped", (byte)5, null, null },
                    { (byte)6, "OutForDelivery", null, false, false, "خرج للتسليم", "Out for delivery", (byte)6, null, null },
                    { (byte)7, "Delivered", null, false, true, "تم التسليم", "Delivered", (byte)7, null, null },
                    { (byte)8, "Refused", null, false, true, "مرفوض عند الاستلام", "Refused", (byte)8, null, null },
                    { (byte)9, "FailedDelivery", null, false, true, "تعذر التسليم", "Failed delivery", (byte)9, null, null },
                    { (byte)10, "Returned", null, false, true, "تم الإرجاع للتاجر", "Returned to merchant", (byte)10, null, null },
                    { (byte)11, "CancelledByMerchant", null, false, true, "ألغاه التاجر", "Cancelled by merchant", (byte)11, null, null },
                    { (byte)12, "CancelledByCustomer", null, false, true, "ألغاه العميل", "Cancelled by customer", (byte)12, null, null }
                });

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Code", "CreatedBy", "CurrencyCode", "IncludedOrders", "IsActive", "IsDeleted", "MonthlyFee", "NameAr", "PricePerCheckedOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[] { (short)1, "TRIAL", null, "USD", 200, true, false, 0m, "تجريبي", 0m, null, null });

            migrationBuilder.InsertData(
                table: "RejectionReasons",
                columns: new[] { "Id", "Attribution", "Code", "CreatedBy", "CustomerFaultWeight", "IsActive", "IsDeleted", "NameAr", "NameEn", "SortOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { (short)1, (byte)1, "CHANGED_MIND", null, 0.50m, true, false, "غيّرت رأيي", "Changed mind", (byte)1, null, null },
                    { (short)2, (byte)1, "NO_CASH", null, 0.80m, true, false, "مفيش معايا فلوس وقت الاستلام", "No cash at delivery", (byte)2, null, null },
                    { (short)3, (byte)1, "NOT_ANSWERING", null, 1.00m, true, false, "ما ردّيتش على المندوب", "Did not answer courier", (byte)3, null, null },
                    { (short)4, (byte)1, "DUPLICATE_ORDER", null, 0.30m, true, false, "طلب مكرر", "Duplicate order", (byte)4, null, null },
                    { (short)5, (byte)1, "FOUND_CHEAPER", null, 0.40m, true, false, "لقيت سعر أرخص", "Found cheaper elsewhere", (byte)5, null, null },
                    { (short)6, (byte)2, "NOT_AS_ADVERTISED", null, 0m, true, false, "المنتج مش زي الإعلان (الخامة/الشكل)", "Not as advertised", (byte)6, null, null },
                    { (short)7, (byte)2, "DEFECTIVE_DAMAGED", null, 0m, true, false, "المنتج تالف أو فيه عيب", "Defective or damaged", (byte)7, null, null },
                    { (short)8, (byte)2, "WRONG_ITEM", null, 0m, true, false, "المنتج أو المقاس أو اللون غلط", "Wrong item/size/color", (byte)8, null, null },
                    { (short)9, (byte)2, "PRICE_MISMATCH", null, 0m, true, false, "السعر مختلف عن المتفق عليه", "Price mismatch", (byte)9, null, null },
                    { (short)10, (byte)3, "LATE_DELIVERY", null, 0m, true, false, "تأخر التوصيل", "Late delivery", (byte)10, null, null },
                    { (short)11, (byte)3, "COURIER_BEHAVIOR", null, 0m, true, false, "تصرف سيئ من المندوب", "Courier behavior", (byte)11, null, null },
                    { (short)12, (byte)4, "ADDRESS_NOT_FOUND", null, 0.20m, true, false, "تعذر الوصول للعنوان", "Address not found", (byte)12, null, null },
                    { (short)13, (byte)4, "DID_NOT_ORDER", null, 0m, true, false, "أنا ما طلبتش (الرقم اتستخدم بدون علمي)", "I did not place this order", (byte)13, null, null },
                    { (short)14, (byte)4, "OTHER", null, 0.10m, true, false, "سبب آخر", "Other", (byte)14, null, null }
                });

            migrationBuilder.InsertData(
                table: "RiskRules",
                columns: new[] { "Id", "Code", "CreatedBy", "ExplanationTemplateAr", "IsActive", "IsDeleted", "NameAr", "ParametersJson", "Points", "Scope", "UpdatedAt", "UpdatedBy", "Version" },
                values: new object[,]
                {
                    { (short)1, "INVALID_PHONE", null, "رقم الهاتف غير صالح أو غير مكتمل", true, false, "رقم هاتف غير صالح", null, (short)40, (byte)1, null, null, 1 },
                    { (short)2, "DUPLICATE_ORDER_24H", null, "نفس الرقم أو العنوان طلب أوردر آخر خلال آخر 24 ساعة", true, false, "أوردر مكرر خلال 24 ساعة", null, (short)20, (byte)1, null, null, 1 },
                    { (short)3, "ADDRESS_INCOMPLETE", null, "العنوان ناقص أو غامض (لا يوجد شارع أو علامة مميزة)", true, false, "عنوان ناقص", null, (short)20, (byte)1, null, null, 1 },
                    { (short)4, "LATE_NIGHT_ORDER", null, "الأوردر اتطلب بعد منتصف الليل", true, false, "أوردر بعد منتصف الليل", null, (short)5, (byte)1, null, null, 1 },
                    { (short)5, "HIGH_VALUE_ORDER", null, "قيمة الأوردر أعلى بكثير من متوسط أوردراتك", true, false, "قيمة أعلى من المعتاد", null, (short)15, (byte)1, null, null, 1 },
                    { (short)6, "FIRST_TIME_CUSTOMER", null, "أول مرة يطلب منك", true, false, "عميل جديد", null, (short)5, (byte)2, null, null, 1 },
                    { (short)7, "NO_CONFIRMATION_RESPONSE", null, "العميل لم يرد على رسالة التأكيد", true, false, "لم يرد على التأكيد", null, (short)30, (byte)2, null, null, 1 },
                    { (short)8, "NO_WHATSAPP", null, "الرقم غالباً غير مسجل على واتساب", true, false, "غير موجود على واتساب", null, (short)10, (byte)2, null, null, 1 },
                    { (short)9, "CONFIRMED_QUICKLY", null, "العميل أكد الطلب خلال دقائق", true, false, "أكد بسرعة", null, (short)-10, (byte)2, null, null, 1 },
                    { (short)10, "GOOD_HISTORY", null, "العميل استلم أوردرات سابقة بنجاح", true, false, "سجل ممتاز", null, (short)-20, (byte)2, null, null, 1 },
                    { (short)11, "NETWORK_PRIOR_REFUSALS", null, "الرقم رفض أوردرات عند تجار آخرين مؤخراً بسبب يخصه", true, false, "رفض سابق في الشبكة", null, (short)35, (byte)3, null, null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0001", "Owner", "OWNER" },
                    { 2, "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0002", "Staff", "STAFF" },
                    { 3, "6a1f0c52-2b7e-4a56-9c0d-1d6d6a3b0003", "PlatformAdmin", "PLATFORMADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Entity",
                table: "AuditLogs",
                columns: new[] { "EntityName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_MerchantId",
                table: "AuditLogs",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carriers_CountryCode",
                table: "Carriers",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Carriers_MerchantId",
                table: "Carriers",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationRequests_Status",
                table: "ConfirmationRequests",
                columns: new[] { "Status", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationRequests_TemplateId",
                table: "ConfirmationRequests",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "UQ_ConfirmationRequests",
                table: "ConfirmationRequests",
                columns: new[] { "OrderId", "AttemptNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDataRequests_CustomerId",
                table: "CustomerDataRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDataRequests_ProcessedByUserId",
                table: "CustomerDataRequests",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CountryCode",
                table: "Customers",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "UQ_Customers_PhoneHash",
                table: "Customers",
                column: "PhoneHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_DeliveryAttempts",
                table: "DeliveryAttempts",
                columns: new[] { "OrderId", "AttemptNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantConsents_AcceptedByUserId",
                table: "MerchantConsents",
                column: "AcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantConsents_Merchant",
                table: "MerchantConsents",
                columns: new[] { "MerchantId", "AcceptedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantCustomers_CustomerId",
                table: "MerchantCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "UQ_MerchantCustomers",
                table: "MerchantCustomers",
                columns: new[] { "MerchantId", "CustomerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_MerchantQualityStats",
                table: "MerchantQualityStats",
                columns: new[] { "MerchantId", "PeriodMonth" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_CountryCode",
                table: "Merchants",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantSubscriptions_Merchant",
                table: "MerchantSubscriptions",
                columns: new[] { "MerchantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantSubscriptions_PlanId",
                table: "MerchantSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_ConfirmationRequestId",
                table: "MessageLogs",
                column: "ConfirmationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_Order",
                table: "MessageLogs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_Provider",
                table: "MessageLogs",
                column: "ProviderMessageId",
                filter: "[ProviderMessageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_MessageTemplates",
                table: "MessageTemplates",
                columns: new[] { "Code", "Language", "Channel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Merchant",
                table: "Notifications",
                columns: new[] { "MerchantId", "IsRead", "CreatedAt" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RelatedOrderId",
                table: "Notifications",
                column: "RelatedOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderImports_MerchantId",
                table: "OrderImports",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderImports_UploadedByUserId",
                table: "OrderImports",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Order",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Product",
                table: "OrderItems",
                column: "ProductId",
                filter: "[ProductId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOutcomes_FinalReasonId",
                table: "OrderOutcomes",
                column: "FinalReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOutcomes_Result",
                table: "OrderOutcomes",
                columns: new[] { "FinalResult", "Attribution" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPublicLinks_Order",
                table: "OrderPublicLinks",
                columns: new[] { "OrderId", "Purpose" });

            migrationBuilder.CreateIndex(
                name: "UQ_OrderPublicLinks_Token",
                table: "OrderPublicLinks",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderRiskAssessments_DecidedByUserId",
                table: "OrderRiskAssessments",
                column: "DecidedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRiskAssessments_Order",
                table: "OrderRiskAssessments",
                columns: new[] { "OrderId", "CreatedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRiskSignals_Assessment",
                table: "OrderRiskSignals",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRiskSignals_RuleId",
                table: "OrderRiskSignals",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Carrier",
                table: "Orders",
                column: "CarrierId",
                filter: "[CarrierId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CountryCode",
                table: "Orders",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedByUserId",
                table: "Orders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer",
                table: "Orders",
                columns: new[] { "CustomerId", "PlacedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ImportId",
                table: "Orders",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Merchant_Placed",
                table: "Orders",
                columns: new[] { "MerchantId", "PlacedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "StatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Merchant_Status",
                table: "Orders",
                columns: new[] { "MerchantId", "StatusId", "ExpectedDeliveryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MerchantCustomerId",
                table: "Orders",
                column: "MerchantCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RegionId",
                table: "Orders",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UQ_Orders_Merchant_OrderNo",
                table: "Orders",
                columns: new[] { "MerchantId", "OrderNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_OrderStatuses_Code",
                table: "OrderStatuses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_ChangedByUserId",
                table: "OrderStatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_FromStatusId",
                table: "OrderStatusHistories",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_Order",
                table: "OrderStatusHistories",
                columns: new[] { "OrderId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_ToStatusId",
                table: "OrderStatusHistories",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "UQ_Plans_Code",
                table: "Plans",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Products_Merchant_Sku",
                table: "Products",
                columns: new[] { "MerchantId", "Sku" },
                unique: true,
                filter: "[Sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Country",
                table: "Regions",
                columns: new[] { "CountryCode", "ParentRegionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentRegionId",
                table: "Regions",
                column: "ParentRegionId");

            migrationBuilder.CreateIndex(
                name: "UQ_RejectionReasons_Code",
                table: "RejectionReasons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RejectionReports_ReasonId",
                table: "RejectionReports",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_RejectionReports_ReportedByUserId",
                table: "RejectionReports",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "UQ_RejectionReports",
                table: "RejectionReports",
                columns: new[] { "OrderId", "ReporterType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_RiskRules_Code",
                table: "RiskRules",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_Merchant_Date",
                table: "UsageRecords",
                columns: new[] { "MerchantId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_OrderId",
                table: "UsageRecords",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Merchant",
                table: "Users",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CustomerDataRequests");

            migrationBuilder.DropTable(
                name: "CustomerRiskProfiles");

            migrationBuilder.DropTable(
                name: "DeliveryAttempts");

            migrationBuilder.DropTable(
                name: "MerchantConsents");

            migrationBuilder.DropTable(
                name: "MerchantQualityStats");

            migrationBuilder.DropTable(
                name: "MerchantSubscriptions");

            migrationBuilder.DropTable(
                name: "MessageLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderOutcomes");

            migrationBuilder.DropTable(
                name: "OrderPublicLinks");

            migrationBuilder.DropTable(
                name: "OrderRiskSignals");

            migrationBuilder.DropTable(
                name: "OrderStatusHistories");

            migrationBuilder.DropTable(
                name: "RejectionReports");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UsageRecords");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "ConfirmationRequests");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "OrderRiskAssessments");

            migrationBuilder.DropTable(
                name: "RiskRules");

            migrationBuilder.DropTable(
                name: "RejectionReasons");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MessageTemplates");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropTable(
                name: "MerchantCustomers");

            migrationBuilder.DropTable(
                name: "OrderImports");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
