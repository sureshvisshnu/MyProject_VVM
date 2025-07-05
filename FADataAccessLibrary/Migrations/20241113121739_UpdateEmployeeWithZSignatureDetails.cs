using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeWithZSignatureDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DigitalSignature",
                table: "employees",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FooterDetails",
                table: "employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDigitalSignatureDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestDigitalSignatureDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestFooterDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabTestSignatureNameDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRxSymbolDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSignatureNameDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "LabTestDigitalSignature",
                table: "employees",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabTestFooter",
                table: "employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LabTestSignatureName",
                table: "employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "RxSymbol",
                table: "employees",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureName",
                table: "employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 47, 36, 475, DateTimeKind.Local).AddTicks(7011));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 47, 36, 475, DateTimeKind.Local).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 47, 36, 475, DateTimeKind.Local).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 47, 36, 475, DateTimeKind.Local).AddTicks(7042));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitalSignature",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "FooterDetails",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsDigitalSignatureDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsLabTestDigitalSignatureDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsLabTestFooterDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsLabTestSignatureNameDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsRxSymbolDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "IsSignatureNameDisplayOn",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "LabTestDigitalSignature",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "LabTestFooter",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "LabTestSignatureName",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "RxSymbol",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "SignatureName",
                table: "employees");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 44, 24, 995, DateTimeKind.Local).AddTicks(9155));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 44, 24, 995, DateTimeKind.Local).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 44, 24, 995, DateTimeKind.Local).AddTicks(9189));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 13, 17, 44, 24, 995, DateTimeKind.Local).AddTicks(9194));
        }
    }
}
