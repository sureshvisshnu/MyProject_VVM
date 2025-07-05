using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updatePatientAppointmentwithIsConsulted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConsulted",
                table: "PatientAppointments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 8, 12, 29, 47, 644, DateTimeKind.Local).AddTicks(5613));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 8, 12, 29, 47, 644, DateTimeKind.Local).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 8, 12, 29, 47, 644, DateTimeKind.Local).AddTicks(5649));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 8, 12, 29, 47, 644, DateTimeKind.Local).AddTicks(5653));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConsulted",
                table: "PatientAppointments");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 2, 14, 43, 29, 721, DateTimeKind.Local).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 2, 14, 43, 29, 721, DateTimeKind.Local).AddTicks(6892));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 2, 14, 43, 29, 721, DateTimeKind.Local).AddTicks(6898));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 2, 14, 43, 29, 721, DateTimeKind.Local).AddTicks(6902));
        }
    }
}
