using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleAndsystemfunctionUpdatePharmacyAndOPRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.UserRoleAndsystemfunctionUpdatePharmacyAndOPRegister();

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7677));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7699));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7709));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 31, 16, 57, 30, 747, DateTimeKind.Local).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 31, 16, 57, 30, 747, DateTimeKind.Local).AddTicks(2231));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 31, 16, 57, 30, 747, DateTimeKind.Local).AddTicks(2243));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 31, 16, 57, 30, 747, DateTimeKind.Local).AddTicks(2255));
        }
    }
}
