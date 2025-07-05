using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateprescriptionChangeDatatypeBooltoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dosage",
                table: "Prescriptions",
                newName: "Total");

            migrationBuilder.AlterColumn<string>(
                name: "Night",
                table: "Prescriptions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Morning",
                table: "Prescriptions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Evening",
                table: "Prescriptions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Afternoon",
                table: "Prescriptions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 47, 29, 310, DateTimeKind.Local).AddTicks(7618));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 47, 29, 310, DateTimeKind.Local).AddTicks(7636));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 47, 29, 310, DateTimeKind.Local).AddTicks(7641));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 47, 29, 310, DateTimeKind.Local).AddTicks(7646));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Prescriptions",
                newName: "Dosage");

            migrationBuilder.AlterColumn<bool>(
                name: "Night",
                table: "Prescriptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Morning",
                table: "Prescriptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Evening",
                table: "Prescriptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Afternoon",
                table: "Prescriptions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 42, 53, 657, DateTimeKind.Local).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 42, 53, 657, DateTimeKind.Local).AddTicks(242));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 42, 53, 657, DateTimeKind.Local).AddTicks(248));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 7, 13, 42, 53, 657, DateTimeKind.Local).AddTicks(252));
        }
    }
}
