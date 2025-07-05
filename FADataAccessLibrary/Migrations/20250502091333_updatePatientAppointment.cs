using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updatePatientAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOfAppointment",
                table: "PatientAppointments",
                newName: "StartingTime");

            migrationBuilder.RenameColumn(
                name: "DateOfAppointment",
                table: "PatientAppointments",
                newName: "FromDateOfAppointment");

            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "PatientAppointments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDateOfAppointment",
                table: "PatientAppointments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 1, 16, 30, 16, 112, DateTimeKind.Local).AddTicks(845));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 1, 16, 30, 16, 112, DateTimeKind.Local).AddTicks(864));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 1, 16, 30, 16, 112, DateTimeKind.Local).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 5, 1, 16, 30, 16, 112, DateTimeKind.Local).AddTicks(874));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "PatientAppointments");

            migrationBuilder.DropColumn(
                name: "ToDateOfAppointment",
                table: "PatientAppointments");

            migrationBuilder.RenameColumn(
                name: "FromDateOfAppointment",
                table: "PatientAppointments",
                newName: "DateOfAppointment");

            migrationBuilder.RenameColumn(
                name: "StartingTime",
                table: "PatientAppointments",
                newName: "TimeOfAppointment");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 4, 18, 15, 16, 11, 109, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 4, 18, 15, 16, 11, 109, DateTimeKind.Local).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 4, 18, 15, 16, 11, 109, DateTimeKind.Local).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 4, 18, 15, 16, 11, 109, DateTimeKind.Local).AddTicks(9571));
        }
    }
}
