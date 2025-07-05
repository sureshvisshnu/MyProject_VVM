using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class hmsmasterandledgerchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ConsultedLabTestId",
                table: "PatientLedgers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "MedicalTests",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ConsultedLabTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "Fees",
                table: "ConsultedLabTests",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ConsultedLabTests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "PerformOn",
                table: "ConsultedLabTests",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PerformedById",
                table: "ConsultedLabTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RequestedById",
                table: "ConsultedLabTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedOn",
                table: "ConsultedLabTests",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "ConsultedConsultationFees",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ConsultedConsultationFees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgers_ConsultedLabTestId",
                table: "PatientLedgers",
                column: "ConsultedLabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTests_PerformedById",
                table: "ConsultedLabTests",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedLabTests_RequestedById",
                table: "ConsultedLabTests",
                column: "RequestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultedLabTests_Users_PerformedById",
                table: "ConsultedLabTests",
                column: "PerformedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultedLabTests_Users_RequestedById",
                table: "ConsultedLabTests",
                column: "RequestedById",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLedgers_ConsultedLabTests_ConsultedLabTestId",
                table: "PatientLedgers",
                column: "ConsultedLabTestId",
                principalTable: "ConsultedLabTests",
                principalColumn: "ConsultedLabTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultedLabTests_Users_PerformedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsultedLabTests_Users_RequestedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLedgers_ConsultedLabTests_ConsultedLabTestId",
                table: "PatientLedgers");

            migrationBuilder.DropIndex(
                name: "IX_PatientLedgers_ConsultedLabTestId",
                table: "PatientLedgers");

            migrationBuilder.DropIndex(
                name: "IX_ConsultedLabTests_PerformedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropIndex(
                name: "IX_ConsultedLabTests_RequestedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "ConsultedLabTestId",
                table: "PatientLedgers");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "Fees",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "PerformOn",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "PerformedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "RequestedOn",
                table: "ConsultedLabTests");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "ConsultedConsultationFees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ConsultedConsultationFees");
        }
    }
}
