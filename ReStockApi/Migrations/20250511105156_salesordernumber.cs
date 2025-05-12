using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStockApi.Migrations
{
    /// <inheritdoc />
    public partial class salesordernumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrderNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    No = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderNumber", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderNumber_No",
                table: "SalesOrderNumber",
                column: "No",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderNumber");
        }
    }
}
