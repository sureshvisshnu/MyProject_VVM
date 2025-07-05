using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomrentWithWardBedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentStartDate",
                table: "PatientRoomRents");
            
            migrationBuilder.DropColumn(
                name: "RentEndDate",
                table: "PatientRoomRents");

            migrationBuilder.RenameColumn(
                name: "EntryDate",
                table: "PatientRoomRents",
                newName: "DatePosted");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfRental",
                table: "PatientRoomRents",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bed",
                table: "PatientRoomRents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "PatientRoomRents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 42, 55, 473, DateTimeKind.Local).AddTicks(5476));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 42, 55, 473, DateTimeKind.Local).AddTicks(5494));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 42, 55, 473, DateTimeKind.Local).AddTicks(5505));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 42, 55, 473, DateTimeKind.Local).AddTicks(5510));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bed",
                table: "PatientRoomRents");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "PatientRoomRents");

            migrationBuilder.DropColumn(
                name: "DateOfRental",
                table: "PatientRoomRents");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "PatientRoomRents",
                newName: "EntryDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentEndDate",
                table: "PatientRoomRents",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.AddColumn<DateTime>(
                name: "RentStartDate",
                table: "PatientRoomRents",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 27, 11, 24, DateTimeKind.Local).AddTicks(1758));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 27, 11, 24, DateTimeKind.Local).AddTicks(1774));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 27, 11, 24, DateTimeKind.Local).AddTicks(1779));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 13, 11, 27, 11, 24, DateTimeKind.Local).AddTicks(1784));
        }
    }
}
