using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addSearchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_MerchantId",
                table: "AuditLogs");

            migrationBuilder.AddColumn<string>(
                name: "SearchText",
                table: "Products",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SearchText",
                table: "Orders",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SearchText",
                table: "MerchantCustomers",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDataRequests_Status",
                table: "CustomerDataRequests",
                columns: new[] { "Status", "RequestedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Merchant_Created",
                table: "AuditLogs",
                columns: new[] { "MerchantId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerDataRequests_Status",
                table: "CustomerDataRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_Merchant_Created",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "SearchText",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SearchText",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SearchText",
                table: "MerchantCustomers");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_MerchantId",
                table: "AuditLogs",
                column: "MerchantId");
        }
    }
}
