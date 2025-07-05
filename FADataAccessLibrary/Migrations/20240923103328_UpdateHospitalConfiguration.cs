using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateHospitalConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DigitalSingnature",
                table: "HospitalConfigurations",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FooterDetails",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDigitalSingatureDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSingnatureNameDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SingnatureName",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 3, 26, 923, DateTimeKind.Local).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 3, 26, 923, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 3, 26, 923, DateTimeKind.Local).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 3, 26, 923, DateTimeKind.Local).AddTicks(8866));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitalSingnature",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "FooterDetails",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsDigitalSingatureDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsSingnatureNameDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "SingnatureName",
                table: "HospitalConfigurations");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 1, 37, 313, DateTimeKind.Local).AddTicks(8260));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 1, 37, 313, DateTimeKind.Local).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 1, 37, 313, DateTimeKind.Local).AddTicks(8283));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 9, 23, 16, 1, 37, 313, DateTimeKind.Local).AddTicks(8287));
        }
    }
}
