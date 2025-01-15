using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inlämningsuppgift_Webshop.Migrations
{
    /// <inheritdoc />
    public partial class advNook5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LoggedIn",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoggedIn",
                table: "Users");
        }
    }
}
