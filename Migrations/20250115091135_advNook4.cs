using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlämningsuppgift_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class advNook4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderDetailId",
                table: "Products",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailId",
                table: "Products",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
