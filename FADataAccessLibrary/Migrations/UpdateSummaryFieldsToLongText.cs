using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateSummaryFieldsToLongText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "DiagnosisSummary",
            table: "DischargeNotes",
            type: "longtext",
            nullable: true,
            oldClrType: typeof(string),
            oldMaxLength: 3072,
            oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentSummary",
                table: "DischargeNotes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3072,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DischargeSummary",
                table: "DischargeNotes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3072,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "DiagnosisSummary",
            table: "DischargeNotes",
            type: "varchar(3072)",
            maxLength: 3072,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "longtext",
            oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentSummary",
                table: "DischargeNotes",
                type: "varchar(3072)",
                maxLength: 3072,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DischargeSummary",
                table: "DischargeNotes",
                type: "varchar(3072)",
                maxLength: 3072,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
