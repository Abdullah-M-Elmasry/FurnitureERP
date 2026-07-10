using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "PurchaseInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmedBy",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                table: "PurchaseInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "PurchaseInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "PurchaseInvoiceItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter",
                table: "InventoryTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "ConfirmedBy",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "BalanceAfter",
                table: "InventoryTransactions");
        }
    }
}
