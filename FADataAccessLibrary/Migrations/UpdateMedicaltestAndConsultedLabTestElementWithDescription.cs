using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateMedicaltestAndConsultedLabTestElementWithDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "ResultDescription",
                table: "ConsultedLabTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResultDescription",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "ResultDescription",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "ResultDescription",
                table: "MedicalTestElements");
        }
    }
}
