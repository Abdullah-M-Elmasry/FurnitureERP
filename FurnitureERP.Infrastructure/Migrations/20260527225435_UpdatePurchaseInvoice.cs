using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierName",
                table: "PurchaseInvoices",
                newName: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "PurchaseInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseInvoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_SupplierId",
                table: "PurchaseInvoices",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoices_Suppliers_SupplierId",
                table: "PurchaseInvoices",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoices_Suppliers_SupplierId",
                table: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_SupplierId",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseInvoices");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "PurchaseInvoices",
                newName: "SupplierName");
        }
    }
}
