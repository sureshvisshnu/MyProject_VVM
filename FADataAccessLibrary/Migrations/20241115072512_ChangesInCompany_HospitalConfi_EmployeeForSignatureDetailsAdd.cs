using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInCompanyHospitalConfiEmployeeForSignatureDetailsAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "LabTestDigitalSignature",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "LabTestFooter",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "LabTestSignatureName",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "SingnatureName",
                table: "HospitalConfigurations");

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

            migrationBuilder.RenameColumn(
                name: "IsSingnatureNameDisplayOn",
                table: "HospitalConfigurations",
                newName: "IsSignatureNameDisplayOnPresc");

            migrationBuilder.RenameColumn(
                name: "IsLabTestSignatureNameDisplayOn",
                table: "HospitalConfigurations",
                newName: "IsSignatureNameDisplayOnLabtest");

            migrationBuilder.RenameColumn(
                name: "IsLabTestFooterDisplayOn",
                table: "HospitalConfigurations",
                newName: "IsDigitalSignatureDisplayOnPresc");

            migrationBuilder.RenameColumn(
                name: "IsLabTestDigitalSignatureDisplayOn",
                table: "HospitalConfigurations",
                newName: "IsDigitalSignatureDisplayOnLabtest");

            migrationBuilder.AddColumn<string>(
                name: "FooterDetailsForLabtest",
                table: "Companies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FooterDetailsForPresc",
                table: "Companies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOnLabtest",
                table: "Companies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOnPresc",
                table: "Companies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 55, 8, 820, DateTimeKind.Local).AddTicks(7384));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 55, 8, 820, DateTimeKind.Local).AddTicks(7403));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 55, 8, 820, DateTimeKind.Local).AddTicks(7408));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 55, 8, 820, DateTimeKind.Local).AddTicks(7414));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterDetailsForLabtest",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FooterDetailsForPresc",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOnLabtest",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOnPresc",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "IsSignatureNameDisplayOnPresc",
                table: "HospitalConfigurations",
                newName: "IsSingnatureNameDisplayOn");

            migrationBuilder.RenameColumn(
                name: "IsSignatureNameDisplayOnLabtest",
                table: "HospitalConfigurations",
                newName: "IsLabTestSignatureNameDisplayOn");

            migrationBuilder.RenameColumn(
                name: "IsDigitalSignatureDisplayOnPresc",
                table: "HospitalConfigurations",
                newName: "IsLabTestFooterDisplayOn");

            migrationBuilder.RenameColumn(
                name: "IsDigitalSignatureDisplayOnLabtest",
                table: "HospitalConfigurations",
                newName: "IsLabTestDigitalSignatureDisplayOn");

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

            migrationBuilder.AddColumn<string>(
                name: "SingnatureName",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 35, 20, 648, DateTimeKind.Local).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 35, 20, 648, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 35, 20, 648, DateTimeKind.Local).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 15, 12, 35, 20, 648, DateTimeKind.Local).AddTicks(9718));
        }
    }
}
