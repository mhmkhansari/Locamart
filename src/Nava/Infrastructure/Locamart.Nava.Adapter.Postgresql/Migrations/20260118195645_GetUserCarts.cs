using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locamart.Nava.Adapter.Postgresql.Migrations
{
    /// <inheritdoc />
    public partial class GetUserCarts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                schema: "nava",
                table: "CartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                schema: "nava",
                table: "CartItems",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
