using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeAndHospitalConfigAddSignatureDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RxSymbol",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "SignatureName",
                table: "employees");

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
                name: "Variation",
                table: "ConsultedLabTestElements",
                newName: "Class");

            migrationBuilder.RenameColumn(
                name: "ValueTo",
                table: "ConsultedLabTestElements",
                newName: "RangeTo");

            migrationBuilder.RenameColumn(
                name: "ValueFrom",
                table: "ConsultedLabTestElements",
                newName: "RangeFrom");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ConsultedLabTestElements",
                newName: "SubClass");

            migrationBuilder.RenameColumn(
                name: "RangeValue",
                table: "ConsultedLabTestElements",
                newName: "SingleValue");

            migrationBuilder.AddColumn<string>(
                name: "FooterDetailsLabtest",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FooterDetailsPrescroption",
                table: "HospitalConfigurations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOnLabtest",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFooterDetailsDisplayOnPresc",
                table: "HospitalConfigurations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRSignatureDisplayOn",
                table: "employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 49, 0, 895, DateTimeKind.Local).AddTicks(856));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 49, 0, 895, DateTimeKind.Local).AddTicks(882));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 49, 0, 895, DateTimeKind.Local).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 49, 0, 895, DateTimeKind.Local).AddTicks(893));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterDetailsLabtest",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "FooterDetailsPrescroption",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOnLabtest",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsFooterDetailsDisplayOnPresc",
                table: "HospitalConfigurations");

            migrationBuilder.DropColumn(
                name: "IsRSignatureDisplayOn",
                table: "employees");

            migrationBuilder.RenameColumn(
                name: "Class",
                table: "ConsultedLabTestElements",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "RangeTo",
                table: "ConsultedLabTestElements",
                newName: "ValueTo");

            migrationBuilder.RenameColumn(
                name: "RangeFrom",
                table: "ConsultedLabTestElements",
                newName: "ValueFrom");

            migrationBuilder.RenameColumn(
                name: "SubClass",
                table: "ConsultedLabTestElements",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "SingleValue",
                table: "ConsultedLabTestElements",
                newName: "RangeValue");

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
                value: new DateTime(2400, 11, 19, 11, 16, 20, 24, DateTimeKind.Local).AddTicks(1448));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 16, 20, 24, DateTimeKind.Local).AddTicks(1472));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 16, 20, 24, DateTimeKind.Local).AddTicks(1477));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 11, 19, 11, 16, 20, 24, DateTimeKind.Local).AddTicks(1482));
        }
    }
}
