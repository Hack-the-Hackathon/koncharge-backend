using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonChargeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserChargeSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserChargeSettings",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false,
                defaultValue: "{}")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserChargeSettings",
                table: "AspNetUsers");
        }
    }
}
