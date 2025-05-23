﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStockApi.Migrations
{
    /// <inheritdoc />
    public partial class removepricefromline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "SalesOrderLines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "SalesOrderLines",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "SalesOrderLines",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
