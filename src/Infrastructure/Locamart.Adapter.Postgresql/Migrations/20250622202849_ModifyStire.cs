using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locamart.Adapter.Postgresql.Migrations
{
    /// <inheritdoc />
    public partial class ModifyStire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "StoreEntity",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StoreEntity_CategoryId",
                table: "StoreEntity",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreEntity_StoreCategoryEntity_CategoryId",
                table: "StoreEntity",
                column: "CategoryId",
                principalTable: "StoreCategoryEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreEntity_StoreCategoryEntity_CategoryId",
                table: "StoreEntity");

            migrationBuilder.DropIndex(
                name: "IX_StoreEntity_CategoryId",
                table: "StoreEntity");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "StoreEntity");
        }
    }
}
