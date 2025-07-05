using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdatePrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                 name: "ProductId",
                 table: "Prescriptions",
                 type: "bigint",
                 nullable: true,
                 oldClrType: typeof(long),
                 oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "CustomProduct",
                table: "Prescriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomProduct",
                table: "Prescriptions",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "ProductId",
               table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "CustomProduct",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "IsCustomProduct",
                table: "Prescriptions");
        }
    }
}
