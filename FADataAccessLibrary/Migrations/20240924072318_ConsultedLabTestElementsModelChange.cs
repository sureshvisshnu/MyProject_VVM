using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class ConsultedLabTestElementsModelChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiAbsolute",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "HiCritical",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "HiNormal",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "LowAbsolute",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "LowCritical",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "LowNormal",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "ObservedHi",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "ObservedLow",
                table: "ConsultedLabTestElements");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ConsultedLabTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueFrom",
                table: "ConsultedLabTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueTo",
                table: "ConsultedLabTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Variation",
                table: "ConsultedLabTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 53, 16, 448, DateTimeKind.Local).AddTicks(3893));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 53, 16, 448, DateTimeKind.Local).AddTicks(3921));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 53, 16, 448, DateTimeKind.Local).AddTicks(3927));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 53, 16, 448, DateTimeKind.Local).AddTicks(3932));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "ValueFrom",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "ValueTo",
                table: "ConsultedLabTestElements");

            migrationBuilder.DropColumn(
                name: "Variation",
                table: "ConsultedLabTestElements");

            migrationBuilder.AddColumn<double>(
                name: "HiAbsolute",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiCritical",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HiNormal",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowAbsolute",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowCritical",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LowNormal",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ObservedHi",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ObservedLow",
                table: "ConsultedLabTestElements",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 41, 55, 576, DateTimeKind.Local).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 41, 55, 576, DateTimeKind.Local).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 41, 55, 576, DateTimeKind.Local).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 24, 12, 41, 55, 576, DateTimeKind.Local).AddTicks(9855));
        }
    }
}
