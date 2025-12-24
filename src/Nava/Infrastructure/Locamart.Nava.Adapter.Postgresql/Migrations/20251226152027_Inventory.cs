using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locamart.Nava.Adapter.Postgresql.Migrations
{
    /// <inheritdoc />
    public partial class Inventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoreId",
                schema: "nava",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StoreId",
                schema: "nava",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "nava",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "nava",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StoreId",
                schema: "nava",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "nava",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                schema: "nava",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "integer", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "nava",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalSchema: "nava",
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "nava",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => new { x.CartId, x.InventoryId });
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "nava",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                schema: "nava",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId_StoreId",
                schema: "nava",
                table: "Inventories",
                columns: new[] { "ProductId", "StoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_StoreId",
                schema: "nava",
                table: "Inventories",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "nava");

            migrationBuilder.DropTable(
                name: "Inventories",
                schema: "nava");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "nava");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "nava",
                table: "Products",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "nava",
                table: "Products",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                schema: "nava",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                schema: "nava",
                table: "Products",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreId",
                schema: "nava",
                table: "Products",
                column: "StoreId",
                principalSchema: "nava",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
