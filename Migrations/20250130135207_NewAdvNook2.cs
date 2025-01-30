using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class NewAdvNook2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Baskets_BasketsId",
                table: "BasketProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Products_ProductsId",
                table: "BasketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct");

            migrationBuilder.DropIndex(
                name: "IX_BasketProduct_ProductsId",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "BasketProduct",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "BasketsId",
                table: "BasketProduct",
                newName: "ProductId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BasketProduct",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "BasketProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_BasketId",
                table: "BasketProduct",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_ProductId",
                table: "BasketProduct",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Baskets_BasketId",
                table: "BasketProduct",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Products_ProductId",
                table: "BasketProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Baskets_BasketId",
                table: "BasketProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Products_ProductId",
                table: "BasketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct");

            migrationBuilder.DropIndex(
                name: "IX_BasketProduct_BasketId",
                table: "BasketProduct");

            migrationBuilder.DropIndex(
                name: "IX_BasketProduct_ProductId",
                table: "BasketProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BasketProduct");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "BasketProduct",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "BasketProduct",
                newName: "BasketsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct",
                columns: new[] { "BasketsId", "ProductsId" });

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_ProductsId",
                table: "BasketProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Baskets_BasketsId",
                table: "BasketProduct",
                column: "BasketsId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Products_ProductsId",
                table: "BasketProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
