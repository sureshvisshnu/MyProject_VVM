using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class catalogproductscheduleandrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MaintainRackNumber",
                table: "Companies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RackNumber",
                table: "CatalogItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "CatalogItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaintainRackNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RackNumber",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "CatalogItems");
        }
    }
}
