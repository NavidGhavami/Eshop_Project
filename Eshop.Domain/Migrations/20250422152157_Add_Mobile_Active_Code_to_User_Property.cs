using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Add_Mobile_Active_Code_to_User_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailActiveCode",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMobileActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MobileActiveCode",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailActiveCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsMobileActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MobileActiveCode",
                table: "Users");
        }
    }
}
