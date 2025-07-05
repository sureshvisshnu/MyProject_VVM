using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class updateHospitalSettingsWithLabTestDigitalSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestDigitalSignatureDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestFooterDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestSignatureNameDisplayOn",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "LabTestDigitalSignature",
                table: "HospitalConfigurations",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabTestFooter",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LabTestSignatureName",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 7, 24, 846, DateTimeKind.Local).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 7, 24, 846, DateTimeKind.Local).AddTicks(9453));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 7, 24, 846, DateTimeKind.Local).AddTicks(9458));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 7, 24, 846, DateTimeKind.Local).AddTicks(9462));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLabTestDigitalSignatureDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsLabTestFooterDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsLabTestSignatureNameDisplayOn",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "LabTestDigitalSignature",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "LabTestFooter",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "LabTestSignatureName",
                table: "HospitalConfigurations");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 2, 37, 987, DateTimeKind.Local).AddTicks(878));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 2, 37, 987, DateTimeKind.Local).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 2, 37, 987, DateTimeKind.Local).AddTicks(900));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 21, 15, 2, 37, 987, DateTimeKind.Local).AddTicks(905));
        }
    }
}
