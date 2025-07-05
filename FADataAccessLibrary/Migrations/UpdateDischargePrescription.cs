using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateDischargePrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "CustomPrescription",
                table: "DischargePrescriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomPrescription",
                table: "DischargePrescriptions",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "CustomPrescription",
                table: "DischargePrescriptions");

            migrationBuilder.DropColumn(
                name: "IsCustomPrescription",
                table: "DischargePrescriptions");
        }
    }
}
