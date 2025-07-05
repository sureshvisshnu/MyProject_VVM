using System;
using fa.context;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateMedicalTestWIthNewElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTestElements_MedicalTestUOMs_UomId",
                table: "MedicalTestElements");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTestElements_UomId",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "HiAbsolute",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "HiCritical",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "HiNormal",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "LowAbsolute",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "LowCritical",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "MedicalTestElements");

            migrationBuilder.RenameColumn(
                name: "ResultDescription",
                table: "MedicalTestElements",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "LowNormal",
                table: "MedicalTestElements",
                newName: "Fee");

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "MedicalTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueFrom",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueTo",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTests_UomId",
                table: "MedicalTests",
                column: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_MedicalTestUOMs_UomId",
                table: "MedicalTests",
                column: "UomId",
                principalTable: "MedicalTestUOMs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_MedicalTestUOMs_UomId",
                table: "MedicalTests");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTests_UomId",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "ValueFrom",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "ValueTo",
                table: "MedicalTestElements");

            migrationBuilder.RenameColumn(
                name: "Variation",
                table: "MedicalTestElements",
                newName: "ResultDescription");

            migrationBuilder.RenameColumn(
                name: "Fee",
                table: "MedicalTestElements",
                newName: "LowNormal");

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "MedicalTests",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiAbsolute",
                table: "MedicalTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiCritical",
                table: "MedicalTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiNormal",
                table: "MedicalTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowAbsolute",
                table: "MedicalTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowCritical",
                table: "MedicalTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "MedicalTestElements",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestElements_UomId",
                table: "MedicalTestElements",
                column: "UomId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTestElements_MedicalTestUOMs_UomId",
                table: "MedicalTestElements",
                column: "UomId",
                principalTable: "MedicalTestUOMs",
                principalColumn: "Id");
        }
    }
}
