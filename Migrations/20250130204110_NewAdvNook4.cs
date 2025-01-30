using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class NewAdvNook4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Shippings",
                type: "money",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Shippings",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
