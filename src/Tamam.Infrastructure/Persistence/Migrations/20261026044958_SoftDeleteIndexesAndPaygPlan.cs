using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamam.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteIndexesAndPaygPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Products_Merchant_Sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UQ_Orders_Merchant_OrderNo",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Code", "CreatedBy", "CurrencyCode", "IncludedOrders", "IsActive", "IsDeleted", "MonthlyFee", "NameAr", "PricePerCheckedOrder", "UpdatedAt", "UpdatedBy" },
                values: new object[] { (short)2, "PAYG", null, "USD", 0, true, false, 0m, "الدفع حسب الاستخدام", 0.05m, null, null });

            migrationBuilder.CreateIndex(
                name: "UX_Products_Merchant_Sku",
                table: "Products",
                columns: new[] { "MerchantId", "Sku" },
                unique: true,
                filter: "[Sku] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Orders_Merchant_OrderNo",
                table: "Orders",
                columns: new[] { "MerchantId", "OrderNo" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Products_Merchant_Sku",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UQ_Orders_Merchant_OrderNo",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.CreateIndex(
                name: "UX_Products_Merchant_Sku",
                table: "Products",
                columns: new[] { "MerchantId", "Sku" },
                unique: true,
                filter: "[Sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Orders_Merchant_OrderNo",
                table: "Orders",
                columns: new[] { "MerchantId", "OrderNo" },
                unique: true);
        }
    }
}
