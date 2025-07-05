using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ProductPercentageStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 7, 4, 21, 8, 22, 620, DateTimeKind.Local).AddTicks(1547));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 7, 4, 21, 8, 22, 620, DateTimeKind.Local).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 7, 4, 21, 8, 22, 620, DateTimeKind.Local).AddTicks(1579));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 7, 4, 21, 8, 22, 620, DateTimeKind.Local).AddTicks(1584));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
