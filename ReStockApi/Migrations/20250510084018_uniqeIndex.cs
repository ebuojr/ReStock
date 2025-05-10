using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStockApi.Migrations
{
    /// <inheritdoc />
    public partial class uniqeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stores_No",
                table: "Stores",
                column: "No",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreInventories_StoreNo_ItemNo",
                table: "StoreInventories",
                columns: new[] { "StoreNo", "ItemNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_HeaderNo",
                table: "SalesOrders",
                column: "HeaderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_HeaderNo_LineNo",
                table: "SalesOrderLines",
                columns: new[] { "HeaderNo", "LineNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reorders_StoreNo_ItemNo",
                table: "Reorders",
                columns: new[] { "StoreNo", "ItemNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ItemNo",
                table: "Products",
                column: "ItemNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryThresholds_StoreNo_ItemNo",
                table: "InventoryThresholds",
                columns: new[] { "StoreNo", "ItemNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DistributionCenterInventories_ItemNo",
                table: "DistributionCenterInventories",
                column: "ItemNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stores_No",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_StoreInventories_StoreNo_ItemNo",
                table: "StoreInventories");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_HeaderNo",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_HeaderNo_LineNo",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_Reorders_StoreNo_ItemNo",
                table: "Reorders");

            migrationBuilder.DropIndex(
                name: "IX_Products_ItemNo",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_InventoryThresholds_StoreNo_ItemNo",
                table: "InventoryThresholds");

            migrationBuilder.DropIndex(
                name: "IX_DistributionCenterInventories_ItemNo",
                table: "DistributionCenterInventories");
        }
    }
}
