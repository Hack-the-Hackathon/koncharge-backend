using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonChargeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserChargeSettings",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserSettings",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSettings",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserChargeSettings",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false,
                defaultValue: "{}")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
