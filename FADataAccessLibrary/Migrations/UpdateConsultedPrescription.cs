using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateConsultedPrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                 name: "PrescriptionId",
                 table: "ConsultedPrescriptions",
                 type: "bigint",
                 nullable: true,
                 oldClrType: typeof(long),
                 oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "CustomPrescription",
                table: "ConsultedPrescriptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomPrescription",
                table: "ConsultedPrescriptions",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "PrescriptionId",
               table: "ConsultedPrescriptions");

            migrationBuilder.DropColumn(
                name: "CustomPrescription",
                table: "ConsultedPrescriptions");

            migrationBuilder.DropColumn(
                name: "IsCustomPrescription",
                table: "ConsultedPrescriptions");
        }
    }
}
