using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateMedicalTestWithNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Variation",
                table: "MedicalTestElements",
                newName: "SubClass");

            migrationBuilder.RenameColumn(
                name: "ValueTo",
                table: "MedicalTestElements",
                newName: "SingleValue");

            migrationBuilder.RenameColumn(
                name: "ValueFrom",
                table: "MedicalTestElements",
                newName: "ResultDuration");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "MedicalTestElements",
                newName: "RangeTo");

            migrationBuilder.RenameColumn(
                name: "RangeValue",
                table: "MedicalTestElements",
                newName: "RangeFrom");

            migrationBuilder.AddColumn<string>(
                name: "TestCode",
                table: "MedicalTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TestShortName",
                table: "MedicalTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ElementCode",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ElementShortName",
                table: "MedicalTestElements",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "MedicalTestElements",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalTestResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MedicalTestId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    MedicalTestElementId = table.Column<long>(type: "bigint", nullable: false),
                    ResultValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResultDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FeeCollected = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTestResults_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 56, 15, 612, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 56, 15, 612, DateTimeKind.Local).AddTicks(6739));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 56, 15, 612, DateTimeKind.Local).AddTicks(6744));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 56, 15, 612, DateTimeKind.Local).AddTicks(6748));

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestElements_UomId",
                table: "MedicalTestElements",
                column: "UomId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestResults_CompanyId",
                table: "MedicalTestResults",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTestElements_MedicalTestUOMs_UomId",
                table: "MedicalTestElements",
                column: "UomId",
                principalTable: "MedicalTestUOMs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTestElements_MedicalTestUOMs_UomId",
                table: "MedicalTestElements");

            migrationBuilder.DropTable(
                name: "MedicalTestResults");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTestElements_UomId",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "TestCode",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "TestShortName",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "ElementCode",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "ElementShortName",
                table: "MedicalTestElements");

            migrationBuilder.DropColumn(
                name: "UomId",
                table: "MedicalTestElements");

            migrationBuilder.RenameColumn(
                name: "SubClass",
                table: "MedicalTestElements",
                newName: "Variation");

            migrationBuilder.RenameColumn(
                name: "SingleValue",
                table: "MedicalTestElements",
                newName: "ValueTo");

            migrationBuilder.RenameColumn(
                name: "ResultDuration",
                table: "MedicalTestElements",
                newName: "ValueFrom");

            migrationBuilder.RenameColumn(
                name: "RangeTo",
                table: "MedicalTestElements",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "RangeFrom",
                table: "MedicalTestElements",
                newName: "RangeValue");

            migrationBuilder.AddColumn<long>(
                name: "UomId",
                table: "MedicalTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 39, 31, 619, DateTimeKind.Local).AddTicks(3498));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 39, 31, 619, DateTimeKind.Local).AddTicks(3529));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 39, 31, 619, DateTimeKind.Local).AddTicks(3538));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 23, 17, 39, 31, 619, DateTimeKind.Local).AddTicks(3545));

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
    }
}
