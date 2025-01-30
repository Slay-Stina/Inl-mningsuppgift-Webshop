using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class NewAdvNook3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "money",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "UnitPrice",
                table: "OrderDetails",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");
        }
    }
}
