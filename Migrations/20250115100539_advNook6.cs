using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlämningsuppgift_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class advNook6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Users",
                newName: "Admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Admin",
                table: "Users",
                newName: "Type");
        }
    }
}
