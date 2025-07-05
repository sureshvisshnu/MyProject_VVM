using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleEntryIdinPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SalesId",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 19, 12, 20, 35, 327, DateTimeKind.Local).AddTicks(5493));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 19, 12, 20, 35, 327, DateTimeKind.Local).AddTicks(5516));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 19, 12, 20, 35, 327, DateTimeKind.Local).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 19, 12, 20, 35, 327, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SalesId",
                table: "Payments",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_saleentries_SalesId",
                table: "Payments",
                column: "SalesId",
                principalTable: "saleentries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_saleentries_SalesId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SalesId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7079));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7084));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7088));
        }
    }
}
